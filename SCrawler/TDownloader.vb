' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports EOptions = PersonalUtilities.Forms.Toolbars.MyProgress.EnableOptions
Imports SCrawler.API
Imports SCrawler.API.Base
Friend Class TDownloader : Implements IDisposable
    Friend Event OnJobsChange(ByVal JobsCount As Integer)
    Friend Event OnDownloadCountChange()
    Friend Event OnDownloading(ByVal Value As Boolean)
    Private TokenSource As CancellationTokenSource
    Private ReadOnly Items As List(Of IUserData)
    Friend ReadOnly Property Downloaded As List(Of IUserData)
    Private ReadOnly NProv As IFormatProvider
    Private _Working As Boolean = False
    Friend ReadOnly Property Working As Boolean
        Get
            Return _Working
        End Get
    End Property
    Private DThread As Thread
    Friend ReadOnly Property Count As Integer
        Get
            Return Items.Count
        End Get
    End Property
    Friend Sub New()
        Items = New List(Of IUserData)
        Downloaded = New List(Of IUserData)
        NProv = New ANumbers(ANumbers.Modes.USA) With {
            .FormatMode = ANumbers.Formats.Number,
            .GroupSize = 3,
            .GroupSeparator = ANumbers.DefaultGroupSeparator,
            .DecimalDigits = 0
        }
    End Sub
    Friend Sub [Start]()
        If Not _Working AndAlso Count > 0 AndAlso Not If(DThread?.IsAlive, False) Then
            DThread = New Thread(New ThreadStart(AddressOf StartDownloading))
            DThread.SetApartmentState(ApartmentState.MTA)
            DThread.Start()
        End If
    End Sub
    Private Sub StartDownloading()
        Dim Token As CancellationToken
        RaiseEvent OnDownloading(True)
        Try
            _Working = True
            TokenSource = New CancellationTokenSource
            Token = TokenSource.Token
            MainProgress.TotalCount = 0
            MainProgress.CurrentCounter = 0
            Do While Count > 0
                Token.ThrowIfCancellationRequested()
                UpdateJobsLabel()
                DownloadData(Token)
                Token.ThrowIfCancellationRequested()
                Thread.Sleep(500)
            Loop
            MainProgress.InformationTemporary = "All data downloaded"
        Catch oex As OperationCanceledException When Token.IsCancellationRequested
            MainProgress.InformationTemporary = "Downloading canceled"
        Catch ex As Exception
            MainProgress.InformationTemporary = "Downloading error"
            ErrorsDescriber.Execute(EDP.SendInLog, ex, "TDownloader.Start")
        Finally
            _Working = False
            TokenSource = Nothing
            UpdateJobsLabel()
            If Settings(Sites.Instagram).InstaHashUpdateRequired Then MyMainLOG = "Check your Instagram credentials"
            RaiseEvent OnDownloading(False)
        End Try
    End Sub
    Friend Sub [Stop]()
        If _Working Then TokenSource.Cancel()
    End Sub
    Private Sub UpdateJobsLabel()
        RaiseEvent OnJobsChange(Count)
    End Sub
    Private _CurrentDownloadingTasks As Integer = 0
    Private Sub DownloadData(ByVal Token As CancellationToken)
        Try
            If Items.Count > 0 Then
                Const nf As ANumbers.Formats = ANumbers.Formats.Number
                Dim t As New List(Of Task)
                Dim i% = -1
                Dim j% = Settings.MaxUsersJobsCount - 1
                Dim Keys As New List(Of String)
                Dim h As Boolean = False
                Dim InstaReady As Boolean = Settings(Sites.Instagram).InstagramReadyForDownload
                For Each _Item As IUserData In Items
                    If Not _Item.Disposed Then
                        Keys.Add(_Item.LVIKey)
                        If Not _Item.Site = Sites.Instagram Or InstaReady Then
                            If _Item.Site = Sites.Instagram Then h = True : Settings(Sites.Instagram).InstagramTooManyRequestsReadyForCatch = True
                            Token.ThrowIfCancellationRequested()
                            t.Add(Task.Run(Sub() _Item.DownloadData(Token)))
                            i += 1
                            If i >= j Then Exit For
                        End If
                    End If
                Next
                If t.Count > 0 Or Keys.Count > 0 Then
                    If h Then
                        With Settings(Sites.Instagram)
                            If .InstaHash.IsEmptyString Or .InstaHashUpdateRequired Then .GatherInstaHash()
                        End With
                    End If
                    _CurrentDownloadingTasks = t.Count
                    With MainProgress
                        .Enabled(EOptions.All) = True
                        .Information = $"Downloading {_CurrentDownloadingTasks.NumToString(nf, NProv)}/{Items.Count.NumToString(nf, NProv)} profiles' data"
                        .InformationTemporary = .Information
                    End With
                    If t.Count > 0 Then Task.WaitAll(t.ToArray)
                    Dim dcc As Boolean = False
                    If Keys.Count > 0 Then
                        For Each k$ In Keys
                            i = Items.FindIndex(Function(ii) ii.LVIKey = k)
                            If i >= 0 Then
                                With Items(i)
                                    If Not .Disposed AndAlso Not .IsCollection AndAlso .DownloadedTotal(False) > 0 Then
                                        If Not Downloaded.Contains(.Self) Then Downloaded.Add(GetUserFromMainCollection(.Self))
                                        dcc = True
                                    End If
                                End With
                                Items.RemoveAt(i)
                            End If
                        Next
                    End If
                    Keys.Clear()
                    Items.RemoveAll(Function(ii) ii.Disposed)
                    If dcc Then Downloaded.RemoveAll(Function(u) u Is Nothing)
                    If dcc And Downloaded.Count > 0 Then RaiseEvent OnDownloadCountChange()
                    t.Clear()
                End If
            End If
        Catch aoex As ArgumentOutOfRangeException
            ErrorsDescriber.Execute(EDP.SendInLog, aoex, $"TDownloader.DownloadData: index out of range ({Count})")
        Catch oex As OperationCanceledException When Token.IsCancellationRequested
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.SendInLog, ex, "TDownloader.DownloadData")
        Finally
            If Settings.UserListUpdateRequired Then _
                Task.WaitAll(Task.Run(Sub()
                                          While Settings.UserListUpdateRequired : Settings.UpdateUsersList() : End While
                                      End Sub))
            MainProgress.Enabled(EOptions.ProgressBar) = False
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
#Region "Saved posts downloading"
    Friend ReadOnly Property SavedPostsDownloading As Boolean
        Get
            Return If(_SavedPostsThread?.IsAlive, False)
        End Get
    End Property
    Private _SavedPostsThread As Thread
    Friend Sub DownloadSavedPostsStart(ByVal Toolbar As StatusStrip, ByVal PR As ToolStripProgressBar)
        If Not SavedPostsDownloading Then
            If Settings(Sites.Reddit).SavedPostsUserName.IsEmptyString Then
                MsgBoxE($"Username of saved posts not set{vbNewLine}Operation canceled", MsgBoxStyle.Critical)
            Else
                _SavedPostsThread = New Thread(New ThreadStart(Sub() Reddit.ProfileSaved.Download(Toolbar, PR)))
                _SavedPostsThread.SetApartmentState(ApartmentState.MTA)
                _SavedPostsThread.Start()
            End If
        Else
            MsgBoxE("Saved posts are already downloading", MsgBoxStyle.Exclamation)
        End If
    End Sub
    Friend Sub DownloadSavedPostsStop()
        Try
            If SavedPostsDownloading Then _SavedPostsThread.Abort()
        Catch ex As Exception
        End Try
    End Sub
#End Region
    Friend Sub Add(ByVal Item As IUserData)
        If Not Items.Contains(Item) Then
            If Item.IsCollection Then Item.DownloadData(Nothing) Else Items.Add(Item)
            UpdateJobsLabel()
        End If
        If Items.Count > 0 Then Start()
    End Sub
    Friend Sub AddRange(ByVal _Items As IEnumerable(Of IUserData))
        If _Items.ListExists Then
            For i% = 0 To _Items.Count - 1
                'If i = 5 Then UpdateJobsLabel() : Start()
                If _Items(i).IsCollection Then _Items(i).DownloadData(Nothing) Else Items.Add(_Items(i))
            Next
            UpdateJobsLabel()
        End If
        If Items.Count > 0 Then Start()
    End Sub
    Friend Sub UserRemove(ByVal _Item As IUserData)
        If Downloaded.Count > 0 AndAlso Downloaded.Contains(_Item) Then Downloaded.Remove(_Item) : RaiseEvent OnDownloadCountChange()
    End Sub
#Region "IDisposable Support"
    Private disposedValue As Boolean = False
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                [Stop]()
                Items.Clear()
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