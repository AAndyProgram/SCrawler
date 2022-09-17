' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports PersonalUtilities.Tools
Imports SCrawler.API.Base
Imports SCrawler.Plugin.Hosts
Imports Download = SCrawler.Plugin.ISiteSettings.Download
Namespace DownloadObjects
    Friend Class TDownloader : Implements IDisposable
#Region "Events"
        Friend Event JobsChange(ByVal JobsCount As Integer)
        Friend Event DownloadCountChange()
        Friend Event Downloading(ByVal Value As Boolean)
        Friend Event SendNotification As NotificationEventHandler
        Friend Event Reconfigured()
        Friend Event FeedFilesChanged(ByVal Added As Boolean)
#End Region
#Region "Declarations"
#Region "Files"
        Friend Structure UserMediaD : Implements IComparable(Of UserMediaD), IEquatable(Of UserMediaD)
            Friend ReadOnly User As IUserData
            Friend ReadOnly Data As UserMedia
            Friend ReadOnly [Date] As Date
            Private ReadOnly Session As Integer
            Friend Sub New(ByVal Data As UserMedia, ByVal User As IUserData, ByVal Session As Integer)
                Me.Data = Data
                Me.User = User
                [Date] = Now
                Me.Session = Session
            End Sub
            Private Function CompareTo(ByVal Other As UserMediaD) As Integer Implements IComparable(Of UserMediaD).CompareTo
                If Not Session = Other.Session Then
                    Return Session.CompareTo(Other.Session) * -1
                ElseIf Not If(User?.GetHashCode, 0) = If(Other.User?.GetHashCode, 0) Then
                    Return If(User?.GetHashCode, 0).CompareTo(If(Other.User?.GetHashCode, 0))
                Else
                    Return [Date].Ticks.CompareTo(Other.Date.Ticks) * -1
                End If
            End Function
            Private Overloads Function Equals(ByVal Other As UserMediaD) As Boolean Implements IEquatable(Of UserMediaD).Equals
                Return Data.File = Other.Data.File
            End Function
            Public Overloads Overrides Function Equals(ByVal Obj As Object) As Boolean
                Return Equals(DirectCast(Obj, UserMedia))
            End Function
        End Structure
        Friend ReadOnly Property Files As List(Of UserMediaD)
        Friend Property FilesChanged As Boolean = False
        Private ReadOnly FilesLP As New ListAddParams(LAP.NotContainsOnly)
#End Region
        Friend ReadOnly Property Downloaded As List(Of IUserData)
        Private ReadOnly NProv As IFormatProvider
#End Region
#Region "Working, Count"
        Friend ReadOnly Property Working As Boolean
            Get
                Return Pool.Count > 0 AndAlso Pool.Exists(Function(j) j.Working)
            End Get
        End Property
        Friend ReadOnly Property Count As Integer
            Get
                Return If(Pool.Count = 0, 0, Pool.Sum(Function(j) j.Count))
            End Get
        End Property
#End Region
#Region "Automation Support"
        Private _AutoDownloaderTasks As Integer = 0
        Friend Property AutoDownloaderWorking As Boolean
            Private Get
                Return _AutoDownloaderTasks > 0
            End Get
            Set(ByVal adw As Boolean)
                _AutoDownloaderTasks += IIf(adw, 1, -1)
            End Set
        End Property
        Friend Sub InvokeDownloadsChangeEvent()
            RaiseEvent DownloadCountChange()
        End Sub
#End Region
#Region "Jobs"
        Friend Class Job : Inherits JobThread(Of IUserData)
            Private ReadOnly Hosts As List(Of SettingsHost)
            Private ReadOnly Keys As List(Of String)
            Private ReadOnly RemovingKeys As List(Of String)
            Friend ReadOnly Property [Type] As Download
            Friend ReadOnly Property IsSeparated As Boolean
                Get
                    Return Hosts.Count = 1 AndAlso Hosts(0).IsSeparatedTasks
                End Get
            End Property
            Friend ReadOnly Property Name As String
                Get
                    Return Hosts(0).Name
                End Get
            End Property
            Friend ReadOnly Property TaskCount As Integer
                Get
                    Return Hosts(0).TaskCount
                End Get
            End Property
            Friend ReadOnly Property Host As SettingsHost
                Get
                    If Hosts.Count > 0 Then
                        Dim k$ = Hosts(0).Key
                        Dim i% = Settings.Plugins.FindIndex(Function(p) p.Key = k)
                        If i >= 0 Then Return Settings.Plugins(i).Settings
                    End If
                    Return Nothing
                End Get
            End Property
            Friend Sub New(ByVal JobType As Download)
                Hosts = New List(Of SettingsHost)
                RemovingKeys = New List(Of String)
                Keys = New List(Of String)
                [Type] = JobType
            End Sub
            Public Overrides Function Add(ByVal User As IUserData) As Boolean
                With DirectCast(User, UserDataBase)
                    If Keys.Count > 0 Then
                        Dim i% = Keys.IndexOf(.User.Plugin)
                        If i >= 0 Then
                            Items.Add(User)
                            OnItemsCountChange(Me, Count)
                            Return True
                        Else
                            If RemovingKeys.Count > 0 Then Return RemovingKeys.IndexOf(.User.Plugin) >= 0
                        End If
                    End If
                End With
                Return False
            End Function
            Friend Sub AddHost(ByRef h As SettingsHost)
                Hosts.Add(h)
                Keys.Add(h.Key)
            End Sub
            Friend Function UserHost(ByVal User As IUserData) As SettingsHost
                Dim i% = Keys.IndexOf(DirectCast(User, UserDataBase).User.Plugin)
                If i >= 0 Then Return Hosts(i) Else Throw New KeyNotFoundException($"Plugin key [{DirectCast(User, UserDataBase).User.Plugin}] not found")
            End Function
            Friend Function Available(ByVal Silent As Boolean) As Boolean
                If Hosts.Count > 0 Then
                    Dim k$
                    For i% = Hosts.Count - 1 To 0 Step -1
                        If Not Hosts(i).Available(Type, Silent) Then
                            k = Hosts(i).Key
                            If Not RemovingKeys.Contains(k) Then RemovingKeys.Add(k)
                            Hosts(i).DownloadDone(Type)
                            Hosts.RemoveAt(i)
                            Keys.RemoveAt(i)
                            If Items.Count > 0 Then Items.RemoveAll(Function(u) DirectCast(u, UserDataBase).HOST.Key = k)
                        End If
                    Next
                    Return Hosts.Count > 0
                Else
                    Return False
                End If
            End Function
            Public Overrides Sub Start()
                If Hosts.Count > 0 Then Hosts.ForEach(Sub(h) h.DownloadStarted([Type]))
                TokenSource = New CancellationTokenSource
                Token = TokenSource.Token
                _Working = True
            End Sub
            Public Overrides Sub Stopped()
                _Working = False
                TokenSource = Nothing
                Try
                    If Not Thread Is Nothing Then
                        If Thread.IsAlive Then Thread.Abort()
                        Thread = Nothing
                    End If
                Catch ex As Exception
                End Try
                If Hosts.Count > 0 Then Hosts.ForEach(Sub(h) h.DownloadDone([Type]))
            End Sub
#Region "IDisposable Support"
            Protected Overrides Sub Dispose(ByVal disposing As Boolean)
                If Not disposedValue And disposing Then
                    Hosts.Clear()
                    Keys.Clear()
                    RemovingKeys.Clear()
                End If
                MyBase.Dispose(disposing)
            End Sub
#End Region
        End Class
        Friend ReadOnly Pool As List(Of Job)
#End Region
#Region "Initializer"
        Friend Sub New()
            Files = New List(Of UserMediaD)
            Downloaded = New List(Of IUserData)
            NProv = New ANumbers With {.FormatOptions = ANumbers.Options.GroupIntegral}
            Pool = New List(Of Job)
        End Sub
#End Region
#Region "Pool"
        Friend Sub ReconfPool()
            If Pool.Count = 0 OrElse Not Pool.Exists(Function(j) j.Working Or j.Count > 0) Then
                Pool.ListClearDispose
                If Settings.Plugins.Count > 0 Then
                    Pool.Add(New Job(Download.Main))
                    For Each p As PluginHost In Settings.Plugins
                        If p.Settings.IsSeparatedTasks Then
                            Pool.Add(New Job(Download.Main))
                            Pool.Last.AddHost(p.Settings)
                        Else
                            Pool(0).AddHost(p.Settings)
                        End If
                    Next
                End If
                RaiseEvent Reconfigured()
            End If
        End Sub
#End Region
#Region "Thread"
        Private CheckerThread As Thread
        Private MissingPostsDetected As Boolean = False
        Private Session As Integer = 0
        Private Sub [Start]()
            If Not AutoDownloaderWorking AndAlso MyProgressForm.ReadyToOpen AndAlso Pool.LongCount(Function(p) p.Count > 0) > 1 Then MyProgressForm.Show() : MainFrameObj.Focus()
            If Not If(CheckerThread?.IsAlive, False) Then
                MainProgress.Visible = True
                If Not AutoDownloaderWorking AndAlso InfoForm.ReadyToOpen Then InfoForm.Show() : MainFrameObj.Focus()
                MissingPostsDetected = False
                Session += 1
                CheckerThread = New Thread(New ThreadStart(AddressOf JobsChecker))
                CheckerThread.SetApartmentState(ApartmentState.MTA)
                CheckerThread.Start()
            End If
        End Sub
        Private Sub JobsChecker()
            Dim fBefore% = Files.Count
            RaiseEvent Downloading(True)
            Try
                MainProgress.Maximum = 0
                MainProgress.Value = 0
                MyProgressForm.DisableProgressChange = False
                Do While Pool.Exists(Function(p) p.Count > 0 Or p.Working)
                    For Each j As Job In Pool
                        If j.Count > 0 And Not j.Working Then j.Start(New ThreadStart(Sub() StartDownloading(j)))
                    Next
                    Thread.Sleep(200)
                Loop
            Catch
            Finally
                With MainProgress
                    .Maximum = 0
                    .Value = 0
                    .InformationTemporary = "All data downloaded"
                    .Visible(, False) = False
                End With
                MyProgressForm.DisableProgressChange = True
                If Pool.Count > 0 Then Pool.ForEach(Sub(p) If Not p.Progress Is Nothing Then p.Progress.Maximum = 0)
                ExecuteCommand(Settings.DownloadsCompleteCommand)
                UpdateJobsLabel()
                If MissingPostsDetected And Settings.AddMissingToLog Then
                    MyMainLOG = "Some posts didn't download. You can see them in the 'Missing posts' form."
                    MainFrameObj.UpdateLogButton()
                End If
                Files.Sort()
                FilesChanged = Not fBefore = Files.Count
                RaiseEvent Downloading(False)
                If FilesChanged Then RaiseEvent FeedFilesChanged(True)
            End Try
        End Sub
        Private Sub StartDownloading(ByRef _Job As Job)
            Dim isSeparated As Boolean = _Job.IsSeparated
            Dim n$ = _Job.Name
            Dim pt As Func(Of String, String) = Function(ByVal t As String) As String
                                                    Dim _t$ = If(isSeparated, $"{n} {Left(t, 1).ToLower}{Right(t, t.Length - 1)}", t)
                                                    If Not AutoDownloaderWorking Then RaiseEvent SendNotification(_t)
                                                    Return _t
                                                End Function
            Try
                _Job.Start()
                _Job.Progress.Maximum = 0
                _Job.Progress.Value = 0
                _Job.Progress.Visible = True
                Dim SiteChecked As Boolean = False
                Do While _Job.Count > 0
                    _Job.ThrowIfCancellationRequested()
                    If Not SiteChecked Then _Job.Available(AutoDownloaderWorking) : SiteChecked = True : Continue Do
                    UpdateJobsLabel()
                    DownloadData(_Job, _Job.Token)
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
                If _Job.Count > 0 Then _Job.Clear()
                _Job.Stopped()
            End Try
        End Sub
        Friend Sub [Stop]()
            If Pool.Count > 0 Then Pool.ForEach(Sub(j) If j.Working Then j.Stop())
        End Sub
        Private Sub UpdateJobsLabel()
            RaiseEvent JobsChange(Count)
        End Sub
        Private Sub DownloadData(ByRef _Job As Job, ByVal Token As CancellationToken)
            Try
                If _Job.Count > 0 Then
                    Const nf As ANumbers.Formats = ANumbers.Formats.Number
                    Dim t As New List(Of Task)
                    Dim i% = 0
                    Dim limit% = _Job.TaskCount
                    Dim Keys As New List(Of String)
                    Dim h As Boolean = False
                    Dim host As SettingsHost = Nothing
                    For Each _Item As IUserData In _Job.Items
                        If Not _Item.Disposed Then
                            Keys.Add(_Item.Key)
                            host = _Job.UserHost(_Item)
                            If host.Source.ReadyToDownload(Download.Main) Then
                                host.BeforeStartDownload(_Item, Download.Main)
                                _Job.ThrowIfCancellationRequested()
                                DirectCast(_Item, UserDataBase).Progress = _Job.Progress
                                t.Add(Task.Run(Sub() _Item.DownloadData(Token)))
                                i += 1
                                If i >= limit Then Exit For
                            End If
                        End If
                    Next
                    If t.Count > 0 Or Keys.Count > 0 Then
                        With _Job.Progress
                            .Visible = True
                            .Information = IIf(_Job.IsSeparated, $"{_Job.Name} d", "D")
                            .Information &= $"ownloading {t.Count.NumToString(nf, NProv)}/{_Job.Items.Count.NumToString(nf, NProv)} profiles' data"
                            .InformationTemporary = .Information
                        End With
                        If t.Count > 0 Then Task.WaitAll(t.ToArray)
                        Dim dcc As Boolean = False
                        If Keys.Count > 0 Then
                            For Each k$ In Keys
                                i = _Job.Items.FindIndex(Function(ii) ii.Key = k)
                                If i >= 0 Then
                                    With _Job.Items(i)
                                        If DirectCast(.Self, UserDataBase).ContentMissingExists Then MissingPostsDetected = True
                                        host.AfterDownload(_Job.Items(i), Download.Main)
                                        If Not .Disposed AndAlso Not .IsCollection AndAlso .DownloadedTotal(False) > 0 Then
                                            If Not Downloaded.Contains(.Self) Then Downloaded.Add(Settings.GetUser(.Self))
                                            With DirectCast(.Self, UserDataBase)
                                                If .LatestData.Count > 0 Then Files.ListAddList(.LatestData.Select(Function(d) New UserMediaD(d, .Self, Session)), FilesLP)
                                            End With
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
                        If dcc And Downloaded.Count > 0 Then RaiseEvent DownloadCountChange()
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
            End Try
        End Sub
#End Region
#Region "Add"
        Private Sub AddItem(ByVal Item As IUserData, ByVal _UpdateJobsLabel As Boolean)
            ReconfPool()
            If Item.IsCollection Then
                Item.DownloadData(Nothing)
            Else
                If Not Contains(Item) Then
                    If Pool.Count > 0 Then
                        For i% = 0 To Pool.Count - 1
                            If Pool(i).Add(Item) Then Exit For
                        Next
                    End If
                    If _UpdateJobsLabel Then UpdateJobsLabel()
                End If
            End If
        End Sub
        Friend Sub Add(ByVal Item As IUserData)
            AddItem(Item, True)
            Start()
        End Sub
        Friend Sub AddRange(ByVal _Items As IEnumerable(Of IUserData))
            If _Items.ListExists Then
                For i% = 0 To _Items.Count - 1 : AddItem(_Items(i), False) : Next
                UpdateJobsLabel()
            End If
            Start()
        End Sub
#End Region
#Region "Contains, Remove"
        Private Function Contains(ByVal _Item As IUserData)
            If Pool.Count > 0 Then
                For Each j As Job In Pool
                    If j.Items.Count > 0 AndAlso j.Items.Contains(_Item) Then Return True
                Next
            End If
            Return False
        End Function
        Friend Sub UserRemove(ByVal _Item As IUserData)
            If Downloaded.Count > 0 AndAlso Downloaded.Contains(_Item) Then Downloaded.Remove(_Item) : RaiseEvent DownloadCountChange()
            If Files.Count > 0 AndAlso Files.RemoveAll(Function(f) Not f.User Is Nothing AndAlso f.User.Equals(_Item)) > 0 Then RaiseEvent FeedFilesChanged(False)
        End Sub
#End Region
#Region "IDisposable Support"
        Private disposedValue As Boolean = False
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    [Stop]()
                    Pool.ListClearDispose
                    Files.Clear()
                    Downloaded.Clear()
                End If
                disposedValue = True
            End If
        End Sub
        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub
        Friend Overloads Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace