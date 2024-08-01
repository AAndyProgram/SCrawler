' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports SCrawler.API.Base
Imports SCrawler.Plugin.Hosts
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
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
        Friend Event UserDownloadStateChanged As UserDownloadStateChangedEventHandler
#End Region
#Region "Declarations"
#Region "Files"
        Friend Const Name_SessionXML As String = "Session"
        Friend Structure UserMediaD : Implements IComparable(Of UserMediaD), IEquatable(Of UserMediaD), IEContainerProvider
#Region "XML Names"
            Private Const Name_Data As String = "Data"
            Private Const Name_User As String = UserInfo.Name_UserNode
            Private Const Name_Media As String = UserMedia.Name_MediaNode
            Private Const Name_Date As String = "Date"
            Private Const Name_Session As String = "Session"
            Private Const Name_File As String = "File"
            Private Const Name_IsSavedPosts As String = "IsSavedPosts"
            Private Const Name_PostUrl As String = "PostUrl"
#End Region
            Friend ReadOnly User As IUserData
            Friend ReadOnly Data As UserMedia
            Friend UserInfo As UserInfo
            Friend ReadOnly [Date] As Date
            Friend Session As Integer
            Friend IsSavedPosts As Boolean
            Private _PostUrl As String
            Friend Property PostUrl(Optional ByVal Generate As Boolean = False) As String
                Get
                    Try
                        If Not _PostUrl.IsEmptyString Then
                            Return _PostUrl
                        ElseIf Generate Then
                            Dim url$ = String.Empty
                            With UserInfo
                                If Not .Plugin.IfNullOrEmpty(.Site).IsEmptyString And Not .Name.IsEmptyString And Not Data.Post.ID.IsEmptyString Then
                                    Dim u As IUserData
                                    If IsSavedPosts Then
                                        If Not .Plugin.IsEmptyString Then
                                            Dim host As SettingsHostCollection = Settings(.Plugin)
                                            If Not host Is Nothing Then
                                                u = host.Default.GetInstance(Download.SavedPosts, UserInfo, False, False)
                                                If Not u Is Nothing AndAlso Not u.HOST Is Nothing Then
                                                    With DirectCast(u, UserDataBase)
                                                        .IsSavedPosts = True
                                                        .HostStatic = True
                                                    End With
                                                    Try : url = u.HOST.Source.GetUserPostUrl(u, Data) : Catch : End Try
                                                    u.Dispose()
                                                End If
                                            End If
                                        End If
                                    Else
                                        u = Settings.GetUser(UserInfo)
                                        If Not u Is Nothing Then url = UserDataBase.GetPostUrl(u, Data)
                                    End If
                                End If
                            End With
                            Return url
                        End If
                    Catch
                    End Try
                    Return String.Empty
                End Get
                Set(ByVal _PostUrl As String)
                    Me._PostUrl = _PostUrl
                End Set
            End Property
            Friend Sub New(ByVal Data As UserMedia, ByVal User As IUserData, ByVal Session As Integer)
                Me.Data = Data
                Me.User = User
                If Not Me.User Is Nothing Then Me.UserInfo = DirectCast(Me.User, UserDataBase).User
                [Date] = Now
                Me.Session = Session
            End Sub
            Friend Sub New(ByVal Data As UserMedia, ByVal User As IUserData, ByVal Session As Integer, ByVal _Date As Date)
                Me.New(Data, User, Session)
                [Date] = _Date
            End Sub
            Private Sub New(ByVal e As EContainer)
                If Not e Is Nothing Then
                    If e.Contains(Name_User) Then
                        IsSavedPosts = e.Value(Name_IsSavedPosts).FromXML(Of Boolean)(False)
                        Dim u As UserInfo = e(Name_User)
                        If Not u.Name.IsEmptyString And Not u.Site.IsEmptyString Then
                            If Not IsSavedPosts Then
                                User = Settings.GetUser(u)
                                If Not User Is Nothing Then UserInfo = DirectCast(User, UserDataBase).User Else UserInfo = u
                            ElseIf Not u.Plugin.IsEmptyString Then
                                UserInfo = u
                                User = Settings(u.Plugin).Default.GetInstance(Download.SavedPosts, u, False, False)
                                If Not User Is Nothing Then
                                    With DirectCast(User, UserDataBase)
                                        .HostStatic = True
                                        .IsSavedPosts = True
                                    End With
                                End If
                            End If
                        End If
                    End If
                    Data = New UserMedia(e(Name_Media), User)
                    [Date] = AConvert(Of Date)(e.Value(Name_Date), DateTimeDefaultProvider, Now)
                    Session = e.Value(Name_Session).FromXML(Of Integer)(0)
                    _PostUrl = e.Value(Name_PostUrl)
                    Dim f As SFile = e.Value(Name_File)
                    If f.Exists Then Data.File = f
                End If
            End Sub
            Public Shared Widening Operator CType(ByVal e As EContainer) As UserMediaD
                Return New UserMediaD(e)
            End Operator
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
            Friend Function ToEContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
                Return ListAddValue(New EContainer(Name_Data, String.Empty) From {
                                        Data.ToEContainer,
                                        New EContainer(Name_Date, AConvert(Of String)([Date], DateTimeDefaultProvider, String.Empty)),
                                        New EContainer(Name_Session, Session),
                                        New EContainer(Name_File, Data.File),
                                        New EContainer(Name_IsSavedPosts, IsSavedPosts.BoolToInteger),
                                        New EContainer(Name_PostUrl, _PostUrl)},
                                    If(IsSavedPosts, UserInfo.ToEContainer, If(Not User Is Nothing, DirectCast(User, UserDataBase).User.ToEContainer, Nothing)), LAP.IgnoreICopier)
            End Function
        End Structure
        Friend ReadOnly Property Files As List(Of UserMediaD)
        Friend Property FilesChanged As Boolean = False
        Private ReadOnly FilesLP As New ListAddParams(LAP.NotContainsOnly)
        Friend Const SessionsPath As String = "Settings\Sessions\"
        Private _FilesSessionCleared As Boolean = False
        Private _FilesSessionActual As SFile = Nothing
        Private _FilesSessionChecked As Boolean = False
        Friend ReadOnly Property FilesSessionActual(Optional ByVal GenerateFileName As Boolean = True) As SFile
            Get
                FilesLoadLastSession()
                If _FilesSessionActual.IsEmptyString And GenerateFileName Then _
                   _FilesSessionActual = $"{SessionsPath}{AConvert(Of String)(Now, SessionDateTimeProvider)}.xml"
                Return _FilesSessionActual
            End Get
        End Property
        Private _FilesSaving As Boolean = False
        Friend Sub FilesSave()
            While _FilesSaving Or _FilesUpdating : Thread.Sleep(100) : End While
            Dim i% = 0
            While Not FilesSaveImpl() And i < 10 : i += 1 : End While
        End Sub
        Private Function FilesSaveImpl() As Boolean
            _FilesSaving = True
            Try
                If Settings.FeedStoreSessionsData And Files.Count > 0 Then
                    ClearSessions()
                    Using x As New XmlFile With {.Name = Name_SessionXML, .AllowSameNames = True}
                        x.AddRange(Files)
                        x.Save(FilesSessionActual)
                    End Using
                End If
                Return True
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadObjects.TDownloader.FilesSave]", False)
            Finally
                _FilesSaving = False
            End Try
        End Function
        Private _FilesSessionChecked_Impl As Boolean = False
        Friend Sub FilesLoadLastSession(Optional ByVal SelectedSessionFile As SFile = Nothing)
            Try
                If Not SelectedSessionFile.IsEmptyString Or (Not _FilesSessionChecked And Not _FilesSessionChecked_Impl And _FilesSessionActual.IsEmptyString) Then
                    _FilesSessionChecked = True
                    _FilesSessionChecked_Impl = True
                    Dim settingValue% = Settings.FeedCurrentTryLoadLastSession
                    Dim ssfExists As Boolean = Not SelectedSessionFile.IsEmptyString AndAlso SelectedSessionFile.Exists
                    If settingValue >= 0 Or ssfExists Then
                        Dim startTime As Date = Process.GetCurrentProcess.StartTime
                        Dim files As List(Of SFile)
                        If ssfExists Then
                            files = New List(Of SFile) From {SelectedSessionFile}
                        Else
                            files = SFile.GetFiles(SessionsPath.CSFileP, "*.xml",, EDP.ReturnValue)
                            If files.ListExists Then files.RemoveAll(Settings.Feeds.FeedSpecialRemover)
                            If files.ListExists Then
                                Dim nd$ = Now.ToString("yyyyMMdd")
                                files.RemoveAll(Function(f) Not f.Name.StartsWith(nd))
                            End If
                        End If
                        If files.ListExists Then
                            files.Sort()
                            Dim lastDate As Date = AConvert(Of Date)(files.Last.Name, SessionDateTimeProvider)
                            If ssfExists Or lastDate.Date = startTime.Date Then
                                Dim __files As New List(Of UserMediaD)
                                Using x As New XmlFile(files.Last, Protector.Modes.All, False) With {.AllowSameNames = True, .XmlReadOnly = True}
                                    x.LoadData()
                                    If x.Count > 0 Then __files.ListAddList(x, LAP.IgnoreICopier)
                                    If __files.Count > 0 AndAlso (settingValue = 0 OrElse ssfExists OrElse
                                                                  (startTime - {lastDate, __files.Max(Function(f) f.Date)}.Max).TotalMinutes <= settingValue) Then
                                        _Session = __files.Max(Function(f) f.Session)
                                        Me.Files.AddRange(__files)
                                        _FilesSessionActual = files.Last
                                    End If
                                    __files.Clear()
                                End Using
                            End If
                        End If
                    End If
                    _FilesSessionChecked_Impl = False
                End If
            Catch ex As Exception
                _FilesSessionChecked_Impl = False
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[TDownloader.FilesLoadLastSession]")
            End Try
        End Sub
        Private _FilesUpdating As Boolean = False
        Friend Sub FilesUpdatePendingUsers()
            Try
                If Files.Count > 0 Then
                    _FilesUpdating = True
                    With Settings.Feeds
                        Dim pUsers As List(Of KeyValuePair(Of UserInfo, UserInfo))
                        Dim pendingUser As KeyValuePair(Of UserInfo, UserInfo)
                        Dim item As UserMediaD
                        Dim result As Boolean
                        Dim changed As Boolean = False
                        While .PendingUsersToUpdate.Count > 0
                            pUsers = New List(Of KeyValuePair(Of UserInfo, UserInfo))(.PendingUsersToUpdate)
                            .PendingUsersToUpdate.Clear()
                            For Each pendingUser In pUsers
                                With Files
                                    If .Count > 0 Then
                                        For i% = 0 To .Count - 1
                                            item = .Item(i)
                                            result = False
                                            item = FeedSpecial.UpdateUsers(item, pendingUser.Key, pendingUser.Value, result)
                                            If result Then changed = True : .Item(i) = item
                                        Next
                                    End If
                                End With
                                If changed Then _FilesUpdating = False : FilesSave()
                            Next
                            pUsers.Clear()
                        End While
                    End With
                    _FilesUpdating = False
                End If
            Catch aex As ArgumentOutOfRangeException
                _FilesUpdating = False
            Catch iex As IndexOutOfRangeException
                _FilesUpdating = False
            Catch ex As Exception
                _FilesUpdating = False
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[TDownloader.FilesUpdatePendingUsers]")
            End Try
        End Sub
        Friend Sub ClearSessions()
            Try
                If Not _FilesSessionCleared Then
                    _FilesSessionCleared = True
                    Dim files As List(Of SFile) = SFile.GetFiles(SessionsPath.CSFileP, "*.xml",, EDP.ReturnValue)
                    If files.ListExists Then files.RemoveAll(Settings.Feeds.FeedSpecialRemover)
                    If files.ListExists Then
                        Const ds$ = "yyyyMMdd"
                        Dim nd$ = Now.ToString(ds), d1$ = Now.AddDays(-1).ToString(ds), d2$ = Now.AddDays(-2).ToString(ds)
                        files.RemoveAll(Function(f) f.Name.StartsWith(nd) Or f.Name.StartsWith(d1) Or f.Name.StartsWith(d2))
                    End If
                    Dim filesCount% = Settings.FeedStoredSessionsNumber
                    If files.ListExists And filesCount > 0 Then
                        Dim fe As New ErrorsDescriber(EDP.None)
                        Do While files.Count > filesCount And files.Count > 0 : files(0).Delete(,, fe) : files.RemoveAt(0) : Loop
                    End If
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadObjects.TDownloader.ClearSessions]")
            End Try
        End Sub
        Friend Sub ResetSession()
            Files.Clear()
            _FilesSessionActual = Nothing
            _FilesSessionChecked = True
            _Session = 0
            _SessionSavedPosts = -1
        End Sub
#End Region
        Friend ReadOnly Property Downloaded As List(Of IUserData)
        Private ReadOnly NProv As IFormatProvider
#End Region
#Region "Working, Count"
        Friend ReadOnly Property Working(Optional ByVal CheckThread As Boolean = True) As Boolean
            Get
                Return _PoolReconfiguration Or (Pool.Count > 0 AndAlso Pool.Exists(Function(j) j.Working)) Or
                       (CheckThread AndAlso If(CheckerThread?.IsAlive, False)) Or _FilesUpdating
            End Get
        End Property
        Friend ReadOnly Property Count As Integer
            Get
                Return If(Pool.Count = 0, 0, Pool.Sum(Function(j) j.Count))
            End Get
        End Property
        Friend Property Suspended As Boolean = False
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
            Private ReadOnly Hosts As List(Of SettingsHostCollection)
            Private ReadOnly Keys As List(Of String)
            Private ReadOnly RemovingKeys As List(Of String)
            Friend ReadOnly Property [Type] As Download
            Friend ReadOnly Property IsSeparated As Boolean
                Get
                    Return Hosts.Count = 1 AndAlso Hosts(0).Default.IsSeparatedTasks
                End Get
            End Property
            Friend ReadOnly Property Name As String
                Get
                    If Not GroupName.IsEmptyString Then Return GroupName Else Return Hosts(0).Name
                End Get
            End Property
            Friend ReadOnly Property GroupName As String
            Friend ReadOnly Property TaskCount(ByVal AccountName As String) As Integer
                Get
                    Return Hosts(0)(AccountName).TaskCount
                End Get
            End Property
            Friend ReadOnly Property HostCollection As SettingsHostCollection
                Get
                    Return Hosts(0)
                End Get
            End Property
            Friend ReadOnly Property Host(ByVal AccountName As String) As SettingsHost
                Get
                    If Hosts.Count > 0 Then
                        Dim k$ = Hosts(0).Key
                        Dim i% = Settings.Plugins.FindIndex(Function(p) p.Key = k)
                        If i >= 0 Then Return Settings.Plugins(i).Settings(AccountName)
                    End If
                    Return Nothing
                End Get
            End Property
            Friend Sub New(ByVal JobType As Download)
                Hosts = New List(Of SettingsHostCollection)
                RemovingKeys = New List(Of String)
                Keys = New List(Of String)
                [Type] = JobType
            End Sub
            Friend Sub New(ByVal JobType As Download, ByVal GroupName As String)
                Me.New(JobType)
                Me.GroupName = GroupName
            End Sub
            Public Overloads Function Add(ByVal User As IUserData, ByVal _IncludedInTheFeed As Boolean) As Boolean
                With DirectCast(User, UserDataBase)
                    If Keys.Count > 0 Then
                        Dim i% = Keys.IndexOf(.User.Plugin)
                        If i >= 0 Then
                            Items.Add(User)
                            DirectCast(Items.Last, UserDataBase).IncludeInTheFeed = _IncludedInTheFeed
                            OnItemsCountChange(Me, Count)
                            Return True
                        Else
                            If RemovingKeys.Count > 0 Then Return RemovingKeys.IndexOf(.User.Plugin) >= 0
                        End If
                    End If
                End With
                Return False
            End Function
            Friend Sub AddHost(ByRef h As SettingsHostCollection)
                Hosts.Add(h)
                Keys.Add(h.Key)
            End Sub
            Friend Function UserHost(ByVal User As IUserData) As SettingsHost
                Dim i% = Keys.IndexOf(DirectCast(User, UserDataBase).User.Plugin)
                If i >= 0 Then Return Hosts(i)(User.AccountName) Else Throw New KeyNotFoundException($"Plugin key [{DirectCast(User, UserDataBase).User.Plugin}] not found")
            End Function
            Friend Function Available(ByVal Silent As Boolean) As Boolean
                If Hosts.Count > 0 Then
                    Dim k$
                    Dim h As SettingsHostCollection
                    Dim hList As IEnumerable(Of String)
                    For i% = Hosts.Count - 1 To 0 Step -1
                        h = Hosts(i)
                        k = h.Key
                        hList = Nothing
                        If Items.Count > 0 Then
                            hList = (From u As UserDataBase In Items
                                     Where u.HOST.Key = k
                                     Select u.HOST.AccountName.IfNullOrEmpty(SettingsHost.NameAccountNameDefault)).Distinct
                            If Not h.Available(Type, Silent,, hList, True) Then
                                If Not RemovingKeys.Contains(k) Then RemovingKeys.Add(k)
                                h.DownloadDone(Type)
                                Hosts.RemoveAt(i)
                                Keys.RemoveAt(i)
                                If Items.Count > 0 Then Items.RemoveAll(Function(u) DirectCast(u, UserDataBase).HOST.Key = k)
                            ElseIf h.CountUnavailable > 0 AndAlso Items.Count > 0 Then
                                Items.RemoveAll(Function(u) DirectCast(u, UserDataBase).HOST.Key = k And Not h.AvailablePartial(u.AccountName))
                            End If
                        Else
                            Hosts.Clear()
                        End If
                    Next
                    Return Hosts.Count > 0 And Items.Count > 0
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
            Public Overrides Sub Finish()
                _Working = False
                TokenSource.DisposeIfReady
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
        Private _PoolReconfiguration As Boolean = False
        Friend Overloads Sub ReconfPool()
            _PoolReconfiguration = True
            Try : ReconfPool(0) : Finally : _PoolReconfiguration = False : End Try
        End Sub
        Friend Overloads Sub ReconfPool(ByVal Round As Integer)
            Try
                If Pool.Count = 0 OrElse Not Pool.Exists(Function(j) j.Working Or j.Count > 0) Then
                    Dim i%
                    Pool.ListClearDispose
                    If Settings.Plugins.Count > 0 Then
                        Pool.Add(New Job(Download.Main))
                        For Each p As PluginHost In Settings.Plugins
                            If p.Settings.Default.IsSeparatedTasks Then
                                Pool.Add(New Job(Download.Main))
                                Pool.Last.AddHost(p.Settings)
                            ElseIf Not p.Settings.Default.TaskGroupName.IsEmptyString Then
                                i = -1
                                If Pool.Count > 0 Then i = Pool.FindIndex(Function(pt) pt.GroupName = p.Settings.Default.TaskGroupName)
                                If i >= 0 Then
                                    Pool(i).AddHost(p.Settings)
                                Else
                                    Pool.Add(New Job(Download.Main, p.Settings.Default.TaskGroupName))
                                    Pool.Last.AddHost(p.Settings)
                                End If
                            Else
                                Pool(0).AddHost(p.Settings)
                            End If
                        Next
                    End If
                    RaiseEvent Reconfigured()
                End If
            Catch ex As Exception
                If Round = 0 Then ReconfPool(Round + 1) Else Throw ex
            End Try
        End Sub
#End Region
#Region "Thread"
        Private CheckerThread As Thread
        Private MissingPostsDetected As Boolean = False
        Private _SessionSavedPosts As Integer = -1
        Friend Property SessionSavedPosts As Integer
            Get
                If Not Working Then
                    Session += 1
                    Return Session
                ElseIf _SessionSavedPosts >= 0 Then
                    _SessionSavedPosts += 1
                    Return _SessionSavedPosts
                Else
                    _SessionSavedPosts = Session + 1
                    Return _SessionSavedPosts
                End If
            End Get
            Set(ByVal NewSessionValue As Integer)
                If Not Working Then
                    Session = NewSessionValue
                Else
                    _SessionSavedPosts = NewSessionValue
                End If
            End Set
        End Property
        Private _Session As Integer = 0
        Private Property Session As Integer
            Get
                FilesLoadLastSession()
                Return _Session
            End Get
            Set(ByVal _Session As Integer)
                Me._Session = _Session
            End Set
        End Property
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
                        If j.Count > 0 And Not j.Working And Not Suspended Then j.StartThread(New ThreadStart(Sub() StartDownloading(j)))
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
                If MissingPostsDetected And Settings.AddMissingToLog Then _
                   MyMainLOG = "Some posts didn't download. You can see them in the 'Missing posts' form."
                Files.Sort()
                FilesChanged = Not fBefore = Files.Count
                RaiseEvent Downloading(False)
                FilesUpdatePendingUsers()
                If FilesChanged Then FilesSave() : RaiseEvent FeedFilesChanged(True)
                If _SessionSavedPosts <> -1 Then Session = _SessionSavedPosts : _SessionSavedPosts = -1
            End Try
        End Sub
        Private Sub StartDownloading(ByRef _Job As Job)
            Dim isSeparated As Boolean = _Job.IsSeparated
            Dim n$ = _Job.Name
            Dim pt As Func(Of String, String) = Function(ByVal t As String) As String
                                                    Dim _t$ = If(isSeparated, $"{n} {Left(t, 1).ToLower}{Right(t, t.Length - 1)}", t)
                                                    If Not AutoDownloaderWorking Then RaiseEvent SendNotification(SettingsCLS.NotificationObjects.Profiles, _t)
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
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "TDownloader.Start")
            Finally
                If _Job.Count > 0 Then _Job.Clear()
                _Job.Finish()
            End Try
        End Sub
        Friend Sub [Stop]()
            If Pool.Count > 0 Then Pool.ForEach(Sub(j) If j.Working Then j.Cancel())
        End Sub
        Private Sub UpdateJobsLabel()
            RaiseEvent JobsChange(Count)
        End Sub
        Private Structure HostLimit
            Friend Key As String
            Friend Limit As Integer
            Friend Value As Integer
            Public Shared Widening Operator CType(ByVal Host As SettingsHost) As HostLimit
                Return New HostLimit With {.Key = Host.KeyDownloader, .Limit = Host.TaskCount, .Value = 0}
            End Operator
            Friend Function [Next]() As HostLimit
                Value += 1
                Return Me
            End Function
            Public Overrides Function Equals(ByVal Obj As Object) As Boolean
                Return Key = CType(Obj, HostLimit).Key
            End Function
        End Structure
        Private Sub DownloadData(ByRef _Job As Job, ByVal Token As CancellationToken)
            Try
                If _Job.Count > 0 Then
                    Const nf As ANumbers.Formats = ANumbers.Formats.Number
                    Dim t As New List(Of Task)
                    Dim i% = 0
                    Dim limit As HostLimit
                    Dim limitIndex%
                    Dim limits As New List(Of HostLimit)
                    Dim Keys As New List(Of String)
                    Dim KeysSkipped As New List(Of String)
                    Dim h As Boolean = False
                    Dim host As SettingsHost = Nothing
                    Dim hostAvailable As Boolean
                    For Each _Item As IUserData In _Job.Items
                        If Not _Item.Disposed Then
                            host = _Job.UserHost(_Item)
                            hostAvailable = DirectCast(_Item, UserDataBase).HostCollection.AvailablePartial(_Item.AccountName)
                            Keys.Add(_Item.Key)
                            If hostAvailable Then
                                limit = host
                                If Not limits.Contains(limit) Then
                                    limits.Add(limit)
                                    limitIndex = limits.Count - 1
                                Else
                                    limitIndex = limits.IndexOf(limit)
                                    limit = limits(limitIndex)
                                End If
                                If host.Source.ReadyToDownload(Download.Main) Then
                                    host.BeforeStartDownload(_Item, Download.Main)
                                    _Job.ThrowIfCancellationRequested()
                                    DirectCast(_Item, UserDataBase).Progress = _Job.Progress
                                    t.Add(Task.Run(Sub() _Item.DownloadData(Token)))
                                    RaiseEvent UserDownloadStateChanged(_Item, True)
                                    limit = limit.Next
                                    limits(limitIndex) = limit
                                    If limit.Value >= limit.Limit Then Exit For
                                Else
                                    KeysSkipped.Add(_Item.Key)
                                End If
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
                                    If KeysSkipped.Count = 0 OrElse Not KeysSkipped.Contains(k) Then
                                        With _Job.Items(i)
                                            If DirectCast(.Self, UserDataBase).ContentMissingExists Then MissingPostsDetected = True
                                            RaiseEvent UserDownloadStateChanged(.Self, False)
                                            host = _Job.UserHost(.Self)
                                            host.AfterDownload(.Self, Download.Main)
                                            If Not .Disposed AndAlso Not .IsCollection AndAlso .DownloadedTotal(False) > 0 Then
                                                If Not Downloaded.Contains(.Self) Then Downloaded.Add(Settings.GetUser(.Self))
                                                With DirectCast(.Self, UserDataBase)
                                                    If .LatestData.Count > 0 And .IncludeInTheFeed Then Files.ListAddList(.LatestData.Select(Function(d) New UserMediaD(d, .Self, Session)), FilesLP)
                                                End With
                                                dcc = True
                                            End If
                                        End With
                                    End If
                                    _Job.Items.RemoveAt(i)
                                End If
                            Next
                        End If
                        Keys.Clear()
                        KeysSkipped.Clear()
                        _Job.Items.RemoveAll(Function(ii) ii.Disposed)
                        If dcc Then Downloaded.RemoveAll(Function(u) u Is Nothing)
                        If dcc And Downloaded.Count > 0 Then RaiseEvent DownloadCountChange()
                        t.Clear()
                    End If
                End If
            Catch aoex As ArgumentOutOfRangeException
                ErrorsDescriber.Execute(EDP.SendToLog, aoex, $"TDownloader.DownloadData: index out of range ({_Job.Count})")
            Catch oex As OperationCanceledException When _Job.IsCancellationRequested
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "TDownloader.DownloadData")
            Finally
                If Settings.UserListUpdateRequired Then _
                Task.WaitAll(Task.Run(Sub()
                                          While Settings.UserListUpdateRequired : Settings.UpdateUsersList() : End While
                                      End Sub))
            End Try
        End Sub
#End Region
#Region "Add"
        Private Sub AddItem(ByVal Item As IUserData, ByVal _UpdateJobsLabel As Boolean, ByVal _IncludedInTheFeed As Boolean)
            ReconfPool()
            If Item.IsCollection Then
                DirectCast(Item, API.UserDataBind).DownloadData(Nothing, _IncludedInTheFeed)
            Else
                If Not Contains(Item) Then
                    If Pool.Count > 0 Then
                        For i% = 0 To Pool.Count - 1
                            If Pool(i).Add(Item, _IncludedInTheFeed) Then Exit For
                        Next
                    End If
                    If _UpdateJobsLabel Then UpdateJobsLabel()
                End If
            End If
        End Sub
        Friend Sub Add(ByVal Item As IUserData, ByVal _IncludedInTheFeed As Boolean)
            AddItem(Item, True, _IncludedInTheFeed)
            Start()
        End Sub
        Friend Sub AddRange(ByVal _Items As IEnumerable(Of IUserData), ByVal _IncludedInTheFeed As Boolean)
            If _Items.ListExists Then
                For i% = 0 To _Items.Count - 1 : AddItem(_Items(i), False, _IncludedInTheFeed) : Next
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