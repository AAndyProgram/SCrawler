' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports PersonalUtilities.Forms.Toolbars
Imports EOptions = PersonalUtilities.Forms.Toolbars.IMyProgress.EnableOptions
Imports SCrawler.API
Imports SCrawler.API.Base
Friend Class TDownloader : Implements IDisposable
    Friend Event OnJobsChange(ByVal Site As Sites, ByVal JobsCount As Integer)
    Friend Event OnDownloadCountChange()
    Friend Event OnDownloading(ByVal Value As Boolean)
    Friend Event SendNotification(ByVal Message As String)
    Friend ReadOnly Property Downloaded As List(Of IUserData)
    Private ReadOnly NProv As IFormatProvider
    Friend ReadOnly Property Working(Optional ByVal Site As Sites = Sites.Undefined) As Boolean
        Get
            If Site = Sites.Instagram Then
                Return JobInst.Working
            Else
                Return JobDefault.Working Or JobInst.Working
            End If
        End Get
    End Property
    Friend Property InstagramSavedPostsDownloading As Boolean = False
#Region "Jobs"
    Friend Structure Job
        Friend Site As Sites
        Private TokenSource As CancellationTokenSource
        Private Token As CancellationToken
        Private [Thread] As Thread
        Private _Working As Boolean
        Friend ReadOnly Items As List(Of IUserData)
        Friend ReadOnly Property Count As Integer
            Get
                Return Items.Count
            End Get
        End Property
        Friend ReadOnly Property Working As Boolean
            Get
                Return _Working OrElse If(Thread?.IsAlive, False)
            End Get
        End Property
        Friend ReadOnly Progress As MyProgress
        Friend Sub New(ByRef _Progress As MyProgress)
            Progress = _Progress
            Items = New List(Of IUserData)
        End Sub
        Public Shared Widening Operator CType(ByVal j As Job) As CancellationToken
            Return j.Token
        End Operator
        Public Shared Widening Operator CType(ByVal j As Job) As Boolean
            Return j.Working
        End Operator
        Public Shared Operator And(ByVal x As Job, ByVal y As Job) As Boolean
            Return x.Working And y.Working
        End Operator
        Public Shared Operator And(ByVal x As Job, ByVal y As Boolean) As Boolean
            Return x.Working And y
        End Operator
        Public Shared Operator And(ByVal x As Boolean, ByVal y As Job) As Boolean
            Return x And y.Working
        End Operator
        Public Shared Operator Or(ByVal x As Job, ByVal y As Job) As Boolean
            Return x.Working Or y.Working
        End Operator
        Public Shared Operator Or(ByVal x As Job, ByVal y As Boolean) As Boolean
            Return x.Working Or y
        End Operator
        Public Shared Operator Or(ByVal x As Boolean, ByVal y As Job) As Boolean
            Return x Or y.Working
        End Operator
        Public Shared Operator Not(ByVal j As Job) As Boolean
            Return Not j.Working
        End Operator
        Friend Sub ThrowIfCancellationRequested()
            Token.ThrowIfCancellationRequested()
        End Sub
        Friend ReadOnly Property IsCancellationRequested As Boolean
            Get
                Return Token.IsCancellationRequested
            End Get
        End Property
        Friend ReadOnly Property IsInstagram As Boolean
            Get
                Return Site = Sites.Instagram
            End Get
        End Property
        Friend Sub [Start](ByVal [ThreadStart] As ThreadStart)
            Thread = New Thread(ThreadStart) With {.IsBackground = True}
            Thread.SetApartmentState(ApartmentState.MTA)
            Thread.Start()
        End Sub
        Friend Sub [Start]()
            TokenSource = New CancellationTokenSource
            Token = TokenSource.Token
            _Working = True
        End Sub
        Friend Sub [Stop]()
            If Not TokenSource Is Nothing Then TokenSource.Cancel()
        End Sub
        Friend Sub Stopped()
            _Working = False
            TokenSource = Nothing
            Try
                If Not Thread Is Nothing Then
                    If Thread.IsAlive Then Thread.Abort()
                    Thread = Nothing
                End If
            Catch ex As Exception
            End Try
        End Sub
    End Structure
    Private JobDefault As Job
    Private JobInst As Job
#End Region
    Friend Sub New()
        Downloaded = New List(Of IUserData)
        NProv = New ANumbers With {.FormatOptions = ANumbers.Options.GroupIntegral}
        JobDefault = New Job(MainProgress)
        JobInst = New Job(MainProgressInst) With {.Site = Sites.Instagram}
    End Sub
    Friend Sub [Start]()
        If Not JobDefault.Working And JobDefault.Count > 0 Then JobDefault.Start(New ThreadStart(Sub() StartDownloading(JobDefault)))
        If Not JobInst.Working And JobInst.Count > 0 And Not InstagramSavedPostsDownloading Then _
           JobInst.Start(New ThreadStart(Sub() StartDownloading(JobInst)))
    End Sub
    Private Sub StartDownloading(ByRef _Job As Job)
        RaiseEvent OnDownloading(True)
        Dim isInst As Boolean = _Job.IsInstagram
        Dim pt As Func(Of String, String) = Function(ByVal t As String) As String
                                                Dim _t$ = If(isInst, $"Instagram {Left(t, 1).ToLower}{Right(t, t.Length - 1)}", t)
                                                RaiseEvent SendNotification(_t)
                                                Return _t
                                            End Function
        Try
            _Job.Start()
            _Job.Progress.TotalCount = 0
            _Job.Progress.CurrentCounter = 0
            _Job.Progress.Enabled = True
            Do While _Job.Count > 0
                _Job.ThrowIfCancellationRequested()
                UpdateJobsLabel(_Job)
                DownloadData(_Job, _Job)
                _Job.ThrowIfCancellationRequested()
                Thread.Sleep(500)
            Loop
            _Job.Progress.InformationTemporary = pt("All data downloaded")
        Catch oex As OperationCanceledException When _Job.IsCancellationRequested
            _Job.Progress.InformationTemporary = pt("Downloading canceled")
        Catch ex As Exception
            _Job.Progress.InformationTemporary = pt("Downloading error")
            ErrorsDescriber.Execute(EDP.SendInLog, ex, "TDownloader.Start")
        Finally
            _Job.Stopped()
            UpdateJobsLabel(_Job)
            If _Job.Site = Sites.Instagram Then
                Settings(Sites.Instagram).InstagramLastDownloadDate.Value = Now
                If Settings(Sites.Instagram).InstaHashUpdateRequired Then MyMainLOG = "Check your Instagram credentials"
            End If
            _Job.Progress.Enabled(EOptions.ProgressBar) = False
            RaiseEvent OnDownloading(False)
        End Try
    End Sub
    Friend Sub [Stop]()
        If JobDefault.Working Then JobDefault.Stop()
        If JobInst.Working Then JobInst.Stop()
    End Sub
    Private Overloads Sub UpdateJobsLabel()
        UpdateJobsLabel(JobDefault)
        UpdateJobsLabel(JobInst)
    End Sub
    Private Overloads Sub UpdateJobsLabel(ByVal _Job As Job)
        RaiseEvent OnJobsChange(_Job.Site, _Job.Count)
    End Sub
    Private _InstagramNextWNM As Instagram.UserData.WNM = Instagram.UserData.WNM.Notify
    Private Sub DownloadData(ByRef _Job As Job, ByVal Token As CancellationToken)
        Try
            If _Job.Count > 0 Then
                Const nf As ANumbers.Formats = ANumbers.Formats.Number
                Dim t As New List(Of Task)
                Dim i% = -1
                Dim j% = Settings.MaxUsersJobsCount - 1
                Dim limit% = IIf(_Job.Site = Sites.Instagram, 1, j)
                Dim Keys As New List(Of String)
                Dim h As Boolean = False
                Dim InstaReady As Boolean = Settings(Sites.Instagram).InstagramReadyForDownload
                For Each _Item As IUserData In _Job.Items
                    If Not _Item.Disposed Then
                        Keys.Add(_Item.LVIKey)
                        If Not _Item.Site = Sites.Instagram Or InstaReady Then
                            If _Item.Site = Sites.Instagram Then
                                h = True
                                With DirectCast(_Item, Instagram.UserData)
                                    .WaitNotificationMode = _InstagramNextWNM
                                    If Settings(Sites.Instagram).InstagramLastDownloadDate.Value < Now.AddMinutes(60) Then
                                        .RequestsCount = Settings(Sites.Instagram).InstagramLastRequestsCount
                                    End If
                                End With
                            End If
                            _Job.ThrowIfCancellationRequested()
                            t.Add(Task.Run(Sub() _Item.DownloadData(Token)))
                            i += 1
                            If i >= limit Then Exit For
                        End If
                    End If
                Next
                If t.Count > 0 Or Keys.Count > 0 Then
                    If h Then
                        With Settings(Sites.Instagram)
                            If .InstaHash.IsEmptyString Or .InstaHashUpdateRequired Then .GatherInstaHash()
                        End With
                    End If
                    With _Job.Progress
                        .Enabled(EOptions.All) = True
                        .Information = IIf(_Job.IsInstagram, "Instagram d", "D")
                        .Information &= $"ownloading {t.Count.NumToString(nf, NProv)}/{_Job.Items.Count.NumToString(nf, NProv)} profiles' data"
                        .InformationTemporary = .Information
                    End With
                    If t.Count > 0 Then Task.WaitAll(t.ToArray)
                    Dim dcc As Boolean = False
                    If Keys.Count > 0 Then
                        For Each k$ In Keys
                            i = _Job.Items.FindIndex(Function(ii) ii.LVIKey = k)
                            If i >= 0 Then
                                With _Job.Items(i)
                                    If _Job.Site = Sites.Instagram Then
                                        With DirectCast(.Self, Instagram.UserData)
                                            _InstagramNextWNM = .WaitNotificationMode
                                            If _InstagramNextWNM = Instagram.UserData.WNM.SkipTemp Or _InstagramNextWNM = Instagram.UserData.WNM.SkipCurrent Then _
                                               _InstagramNextWNM = Instagram.UserData.WNM.Notify
                                            Settings(Sites.Instagram).InstagramLastRequestsCount.Value = .RequestsCount
                                        End With
                                    End If
                                    If Not .Disposed AndAlso Not .IsCollection AndAlso .DownloadedTotal(False) > 0 Then
                                        If Not Downloaded.Contains(.Self) Then Downloaded.Add(GetUserFromMainCollection(.Self))
                                        dcc = True
                                    End If
                                End With
                                _Job.Items.RemoveAt(i)
                            End If
                        Next
                    End If
                    Keys.Clear()
                    _Job.Items.RemoveAll(Function(ii) ii.Disposed)
                    If dcc Then Downloaded.RemoveAll(Function(u) u Is Nothing)
                    If dcc And Downloaded.Count > 0 Then RaiseEvent OnDownloadCountChange()
                    t.Clear()
                End If
            End If
        Catch aoex As ArgumentOutOfRangeException
            ErrorsDescriber.Execute(EDP.SendInLog, aoex, $"TDownloader.DownloadData: index out of range ({_Job.Count})")
        Catch oex As OperationCanceledException When _Job.IsCancellationRequested
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.SendInLog, ex, "TDownloader.DownloadData")
        Finally
            If Settings.UserListUpdateRequired Then _
                Task.WaitAll(Task.Run(Sub()
                                          While Settings.UserListUpdateRequired : Settings.UpdateUsersList() : End While
                                      End Sub))
            If _Job.Site = Sites.Instagram Then Settings(Sites.Instagram).InstagramLastDownloadDate.Value = Now
        End Try
    End Sub
    Private Function GetUserFromMainCollection(ByVal User As IUserData) As IUserData
        Dim uSimple As Predicate(Of IUserData) = Function(u) u.Equals(DirectCast(User.Self, UserDataBase))
        Dim uCol As Predicate(Of IUserData) = Function(ByVal u As IUserData) As Boolean
                                                  If u.IsCollection Then
                                                      Return DirectCast(u, UserDataBind).Collections.Exists(uSimple)
                                                  Else
                                                      Return False
                                                  End If
                                              End Function
        Dim uu As Predicate(Of IUserData)
        If User.IncludedInCollection Then uu = uCol Else uu = uSimple
        Dim i% = Settings.Users.FindIndex(uu)
        If i >= 0 Then
            If Settings.Users(i).IsCollection Then
                With DirectCast(Settings.Users(i), UserDataBind)
                    i = .Collections.FindIndex(uSimple)
                    If i >= 0 Then Return .Collections(i)
                End With
            Else
                Return Settings.Users(i)
            End If
        End If
        Return Nothing
    End Function
    Private Sub AddItem(ByVal Item As IUserData, ByVal _UpdateJobsLabel As Boolean)
        If Not Contains(Item) Then
            If Item.IsCollection Then
                Item.DownloadData(Nothing)
            ElseIf Item.Site = Sites.Instagram Then
                JobInst.Items.Add(Item)
                If _UpdateJobsLabel Then UpdateJobsLabel(JobInst)
            Else
                JobDefault.Items.Add(Item)
                If _UpdateJobsLabel Then UpdateJobsLabel(JobDefault)
            End If
        End If
    End Sub
    Friend Sub Add(ByVal Item As IUserData)
        AddItem(Item, True)
        If JobDefault.Count > 0 Or JobInst.Count > 0 Then Start()
    End Sub
    Friend Sub AddRange(ByVal _Items As IEnumerable(Of IUserData))
        If _Items.ListExists Then
            For i% = 0 To _Items.Count - 1 : AddItem(_Items(i), False) : Next
            UpdateJobsLabel()
        End If
        If JobDefault.Count > 0 Or JobInst.Count > 0 Then Start()
    End Sub
    Private Function Contains(ByVal _Item As IUserData)
        If _Item.Site = Sites.Instagram Then
            Return JobInst.Items.Contains(_Item)
        Else
            Return JobDefault.Items.Contains(_Item)
        End If
    End Function
    Friend Sub UserRemove(ByVal _Item As IUserData)
        If Downloaded.Count > 0 AndAlso Downloaded.Contains(_Item) Then Downloaded.Remove(_Item) : RaiseEvent OnDownloadCountChange()
    End Sub
#Region "IDisposable Support"
    Private disposedValue As Boolean = False
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                [Stop]()
                JobDefault.Items.Clear()
                JobInst.Items.Clear()
                Downloaded.Clear()
            End If
            disposedValue = True
        End If
    End Sub
    Protected Overrides Sub Finalize()
        Dispose(False)
        MyBase.Finalize()
    End Sub
    Friend Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class