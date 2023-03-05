' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.Messaging
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Functions.XML.Objects
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Imports SCrawler.API
Imports SCrawler.API.Base
Imports SCrawler.Plugin.Hosts
Imports SCrawler.DownloadObjects
Friend Class SettingsCLS : Implements IDisposable
    Friend Const DefaultMaxDownloadingTasks As Integer = 5
    Friend Const TaskStackNamePornSite As String = "Porn sites"
    Friend Const Name_Node_Sites As String = "Sites"
    Private Const SitesValuesSeparator As String = ","
    Friend Const CookieEncryptKey As String = "SCrawlerCookiesEncryptKeyword"
    Friend ReadOnly Design As XmlFile
    Private ReadOnly MyXML As XmlFile
    Private ReadOnly FfmpegExists As Boolean
    Friend ReadOnly FfmpegFile As SFile
    Friend ReadOnly Property UseM3U8 As Boolean
        Get
            Return FfmpegExists
        End Get
    End Property
    Private ReadOnly FFMPEGNotification As XMLValue(Of Boolean)
    Friend ReadOnly Property CachePath As SFile = "_Cache\"
    Friend ReadOnly Plugins As List(Of PluginHost)
    Friend ReadOnly Property Users As List(Of IUserData)
    Friend ReadOnly Property UsersList As List(Of UserInfo)
    Friend Property Channels As Reddit.ChannelsCollection
    Friend ReadOnly Property Labels As LabelsKeeper
    Friend ReadOnly Property Groups As Groups.DownloadGroupCollection
    Friend ReadOnly Property LastCollections As List(Of String)
    Friend Property Automation As Scheduler
    Friend ReadOnly Property BlackList As List(Of UserBan)
    Private ReadOnly BlackListFile As SFile = $"{SettingsFolderName}\BlackList.txt"
    Private ReadOnly UsersSettingsFile As SFile = $"{SettingsFolderName}\Users.xml"
    Private Sub RemoveUnusedPlugins()
        Dim f As SFile = PluginHost.PluginsPath
        If f.Exists(SFO.Path, False) Then
            Dim fpe As SFile = f
            fpe.Name = "PersonalUtilities"
            fpe.Extension = "dll"
            Dim fpp As SFile = fpe
            fpp.Name = "SCrawler.PluginProvider"
            Dim ff As List(Of SFile) = SFile.GetFiles(f, "*.dll")
            If ff.ListExists Then
                For i% = ff.Count - 1 To 0 Step -1
                    Select Case ff(i).Name
                        Case "SCrawler.Plugin.LPSG" : If ff(i).Delete Then ff.RemoveAt(i)
                        Case "SCrawler.Plugin.XVIDEOS" : If ff(i).Delete Then ff.RemoveAt(i)
                    End Select
                Next
                If ff.Count > 0 AndAlso ((ff.Count = 2 And ff.Contains(fpe) And ff.Contains(fpp)) Or (ff.Count = 1 And ff.Contains(fpp))) Then _
                   fpe.Delete() : fpp.Delete()
            End If
        End If
    End Sub
    Friend Sub New()
        RemoveUnusedPlugins()
        FfmpegFile = "ffmpeg.exe"
        FfmpegExists = FfmpegFile.Exists

        Design = New XmlFile("Settings\Design.xml", Protector.Modes.All)
        MyXML = New XmlFile(Nothing) With {.AutoUpdateFile = True}
        MyXML.BeginUpdate()
        Users = New List(Of IUserData)
        UsersList = New List(Of UserInfo)
        BlackList = New List(Of UserBan)
        Plugins = New List(Of PluginHost)
        LastCollections = New List(Of String)

        FFMPEGNotification = New XMLValue(Of Boolean)("FFMPEGNotification", True, MyXML)
        If Not FfmpegExists Then
            If FFMPEGNotification.Value AndAlso
               MsgBoxE({"[ffmpeg.exe] is missing", "ffmpeg.exe"}, vbExclamation,,,
                       {"OK", New MsgBoxButton("Disable notification", "Disable ffmpeg missing notification")}) = 1 Then FFMPEGNotification.Value = False
        Else
            FFMPEGNotification.Value = True
        End If

        GlobalPath = New XMLValue(Of SFile)("GlobalPath", New SFile($"{SFile.GetPath(Application.StartupPath).PathWithSeparator}Data\"), MyXML,,
                                            New XMLToFilePathProvider)
        LastCopyPath = New XMLValue(Of SFile)("LastCopyPath",, MyXML,, New XMLToFilePathProvider)

        CookiesEncrypted = New XMLValue(Of Boolean)("CookiesEncrypted", False, MyXML)
        EncryptCookies.CookiesEncrypted = CookiesEncrypted

        SeparateVideoFolder = New XMLValue(Of Boolean)("SeparateVideoFolder", True, MyXML)
        CollectionsPath = New XMLValue(Of String)("CollectionsPath", "Collections", MyXML)

        UserAgent = New XMLValue(Of String)("UserAgent",, MyXML)
        If Not UserAgent.IsEmptyString Then DefaultUserAgent = UserAgent

        Dim n() As String = {"Search"}
        SearchInName = New XMLValue(Of Boolean)("SearchInName", True, MyXML, n)
        SearchInDescription = New XMLValue(Of Boolean)("SearchInDescription", False, MyXML, n)
        SearchInLabel = New XMLValue(Of Boolean)("SearchInLabel", False, MyXML, n)

        n = {"Defaults"}
        DefaultTemporary = New XMLValue(Of Boolean)("Temporary", False, MyXML, n)
        DefaultDownloadImages = New XMLValue(Of Boolean)("DownloadImages", True, MyXML, n)
        DefaultDownloadVideos = New XMLValue(Of Boolean)("DownloadVideos", True, MyXML, n)
        ChangeReadyForDownOnTempChange = New XMLValue(Of Boolean)("ChangeReadyForDownOnTempChange", True, MyXML, n)
        DownloadNativeImageFormat = New XMLValue(Of Boolean)("DownloadNativeImageFormat", True, MyXML, n)
        ReparseMissingInTheRoutine = New XMLValue(Of Boolean)("ReparseMissingInTheRoutine", False, MyXML, n)
        UserSiteNameAsFriendly = New XMLValue(Of Boolean)("UserSiteNameAsFriendly", False, MyXML, n)
        UserSiteNameUpdateEveryTime = New XMLValue(Of Boolean)("UserSiteNameUpdateEveryTime", False, MyXML, n)

        Plugins.AddRange(PluginHost.GetMyHosts(MyXML, GlobalPath.Value, DefaultTemporary, DefaultDownloadImages, DefaultDownloadVideos))
        Dim tmpPluginList As IEnumerable(Of PluginHost) = PluginHost.GetPluginsHosts(MyXML, GlobalPath.Value, DefaultTemporary,
                                                                                     DefaultDownloadImages, DefaultDownloadVideos)
        If tmpPluginList.ListExists Then Plugins.AddRange(tmpPluginList)
        CookiesEncrypted.Value = True

        FastProfilesLoading = New XMLValue(Of Boolean)("FastProfilesLoading", True, MyXML)
        MaxLargeImageHeight = New XMLValue(Of Integer)("MaxLargeImageHeight", 150, MyXML)
        MaxSmallImageHeight = New XMLValue(Of Integer)("MaxSmallImageHeight", 15, MyXML)
        UserListBackColor = New XMLValue(Of Color)
        UserListBackColor.SetExtended("UserListBackColor",, MyXML)
        UserListForeColor = New XMLValue(Of Color)
        UserListForeColor.SetExtended("UserListForeColor",, MyXML)
        UserListImage = New XMLValue(Of SFile)("UserListImage",, MyXML)
        DownloadOpenInfo = New XMLValueAttribute(Of Boolean, Boolean)("DownloadOpenInfo", "OpenAgain", False, False, MyXML)
        DownloadOpenProgress = New XMLValueAttribute(Of Boolean, Boolean)("DownloadOpenProgress", "OpenAgain", False, False, MyXML)
        DownloadsCompleteCommand = New XMLValueAttribute(Of String, Boolean)("DownloadsCompleteCommand", "Use",,, MyXML)
        ClosingCommand = New XMLValueAttribute(Of String, Boolean)("ClosingCommand", "Use",,, MyXML)
        AddHandler ClosingCommand.ValueChanged, Sub(s, ev) MainFrameObj?.ChangeCloseVisible()
        InfoViewMode = New XMLValue(Of Integer)("InfoViewMode", DownloadedInfoForm.ViewModes.Session, MyXML)
        ViewMode = New XMLValue(Of Integer)("ViewMode", ViewModes.IconLarge, MyXML)
        ShowingMode = New XMLValue(Of Integer)("ShowingMode", ShowingModes.All, MyXML)
        ShowGroupsInsteadLabels = New XMLValue(Of Boolean)("ShowGroupsInsteadLabels", False, MyXML)
        ShowGroups = New XMLValue(Of Boolean)("ShowGroups", True, MyXML)
        UseGrouping = New XMLValue(Of Boolean)("UseGrouping", True, MyXML)

        AddMissingToLog = New XMLValue(Of Boolean)("AddMissingToLog", True, MyXML)
        AddMissingErrorsToLog = New XMLValue(Of Boolean)("AddMissingErrorsToLog", False, MyXML)

        LatestSavingPath = New XMLValue(Of SFile)("LatestSavingPath", Nothing, MyXML,, New XMLToFilePathProvider)
        LatestSelectedChannel = New XMLValue(Of String)("LatestSelectedChannel",, MyXML)

        _ViewDateFrom = New XMLValue(Of Date)
        _ViewDateFrom.SetExtended("ViewDateFrom",, MyXML)
        _ViewDateTo = New XMLValue(Of Date)
        _ViewDateTo.SetExtended("ViewDateTo",, MyXML)
        ViewDateMode = New XMLValue(Of Integer)("ViewDateMode", ShowingDates.Off, MyXML)

        LatestDownloadedSites = New XMLValuesCollection(Of String)(IXMLValuesCollection.Modes.String, "LatestDownloadedSites",, MyXML)

        SelectedSites = New XMLValuesCollection(Of String)(IXMLValuesCollection.Modes.String, "SelectedSites",, MyXML, {Name_Node_Sites})

        ImgurClientID = New XMLValue(Of String)("ImgurClientID", String.Empty, MyXML, {Name_Node_Sites})

        n = {Name_Node_Sites, "Channels"}
        ChannelsDefaultReadyForDownload = New XMLValue(Of Boolean)("ChannelsDefaultReadyForDownload", False, MyXML, n)
        ChannelsDefaultTemporary = New XMLValue(Of Boolean)("ChannelsDefaultTemporary", True, MyXML, n)
        ChannelsRegularCheckMD5 = New XMLValue(Of Boolean)("ChannelsRegularCheckMD5", False, MyXML, n)
        ChannelsImagesRows = New XMLValue(Of Integer)("ImagesRows", 2, MyXML, n)
        ChannelsImagesColumns = New XMLValue(Of Integer)("ImagesColumns", 5, MyXML, n)
        ChannelsHideExistsUser = New XMLValue(Of Boolean)("HideExistsUser", True, MyXML, n)
        ChannelsMaxJobsCount = New XMLValue(Of Integer)("MaxJobsCount", DefaultMaxDownloadingTasks, MyXML, n)
        ChannelsAddUserImagesFromAllChannels = New XMLValue(Of Boolean)("AddUserImagesFromAllChannels", True, MyXML, n)

        n = {"Feed"}
        FeedDataColumns = New XMLValue(Of Integer)("DataColumns", 1, MyXML, n)
        FeedDataRows = New XMLValue(Of Integer)("DataRows", 10, MyXML, n)
        FeedCenterImage = New XMLValueUse(Of Integer)("FeedCenterImage", 1,, MyXML, n)
        FeedEndless = New XMLValue(Of Boolean)("Endless", True, MyXML, n)
        FeedAddDateToCaption = New XMLValue(Of Boolean)("AddDateToCaption", True, MyXML, n)
        FeedAddSessionToCaption = New XMLValue(Of Boolean)("AddSessionToCaption", False, MyXML, n)
        FeedStoreSessionsData = New XMLValue(Of Boolean)("StoreSessionsData", True, MyXML, n)
        FeedBackColor = New XMLValue(Of Color)
        FeedBackColor.SetExtended("FeedColorBack",, MyXML, n)
        FeedForeColor = New XMLValue(Of Color)
        FeedForeColor.SetExtended("FeedColorFore",, MyXML, n)

        n = {"Users"}
        FromChannelDownloadTop = New XMLValue(Of Integer)("FromChannelDownloadTop", 10, MyXML, n)
        FromChannelDownloadTopUse = New XMLValue(Of Boolean)("FromChannelDownloadTopUse", False, MyXML, n)
        FromChannelCopyImageToUser = New XMLValue(Of Boolean)("FromChannelCopyImageToUser", True, MyXML, n)
        UpdateUserDescriptionEveryTime = New XMLValue(Of Boolean)("UpdateUserDescriptionEveryTime", True, MyXML, n)
        ScriptData = New XMLValueAttribute(Of String, Boolean)("ScriptData", "Use",,, MyXML, n)

        n = {"Users", "FileName"}
        MaxUsersJobsCount = New XMLValue(Of Integer)("MaxJobsCount", DefaultMaxDownloadingTasks, MyXML, n)
        FileAddDateToFileName = New XMLValue(Of Boolean)("FileAddDateToFileName", False, MyXML, n)
        AddHandler FileAddDateToFileName.ValueChanged, AddressOf ChangeDateProvider
        FileAddTimeToFileName = New XMLValue(Of Boolean)("FileAddTimeToFileName", False, MyXML, n)
        AddHandler FileAddTimeToFileName.ValueChanged, AddressOf ChangeDateProvider
        FileDateTimePositionEnd = New XMLValue(Of Boolean)("FileDateTimePositionEnd", True, MyXML, n)
        AddHandler FileDateTimePositionEnd.ValueChanged, AddressOf ChangeDateProvider
        FileReplaceNameByDate = New XMLValue(Of Integer)("FileReplaceNameByDate", FileNameReplaceMode.None, MyXML, n)

        CheckUpdatesAtStart = New XMLValue(Of Boolean)("CheckUpdatesAtStart", True, MyXML)
        ShowNewVersionNotification = New XMLValue(Of Boolean)("ShowNewVersionNotification", True, MyXML)
        LatestVersion = New XMLValue(Of String)("LatestVersion", String.Empty, MyXML)

        n = {"Notifications"}
        ShowNotifications = New XMLValue(Of Boolean)("ShowNotifications", True, MyXML, n)
        ShowNotificationsDownProfiles = New XMLValue(Of Boolean)("Profiles", True, MyXML, n)
        ShowNotificationsDownAutoDownloader = New XMLValue(Of Boolean)("AutoDownloader", True, MyXML, n)
        ShowNotificationsDownChannels = New XMLValue(Of Boolean)("Channels", True, MyXML, n)
        ShowNotificationsDownSavedPosts = New XMLValue(Of Boolean)("SavedPosts", True, MyXML, n)

        ExitConfirm = New XMLValue(Of Boolean)("ExitConfirm", True, MyXML)
        CloseToTray = New XMLValue(Of Boolean)("CloseToTray", True, MyXML)
        OpenFolderInOtherProgram = New XMLValueAttribute(Of String, Boolean)("OpenFolderInOtherProgram", "Use",,, MyXML)
        DeleteToRecycleBin = New XMLValue(Of Boolean)("DeleteToRecycleBin", True, MyXML)

        Labels = New LabelsKeeper(MyXML)
        Groups = New Groups.DownloadGroupCollection
        Labels.AddRange(Groups.GetGroupsLabels, False)

        MyXML.EndUpdate()
        If MyXML.ChangesDetected Then MyXML.Sort() : MyXML.UpdateData()

        If BlackListFile.Exists Then
            BlackList.ListAddList(IO.File.ReadAllLines(BlackListFile), LAP.NotContainsOnly)
            If BlackList.Count > 0 Then BlackList.RemoveAll(Function(b) Not b.Exists)
        End If
        _UpdatesSuspended = False
        ChangeDateProvider(Nothing, Nothing)
    End Sub
    Private Sub ChangeDateProvider(ByVal Sender As Object, ByVal e As EventArgs)
        If Not _UpdatesSuspended Then
            Dim p$ = String.Empty
            If FileAddDateToFileName Then p = "yyyyMMdd"
            If FileAddTimeToFileName Then p.StringAppend("HHmmss", "_")
            If Not p.IsEmptyString Then FileDateAppenderProvider = New ADateTime(p) Else FileDateAppenderProvider = New ADateTime("yyyyMMdd_HHmmss")
            If FileReplaceNameByDate.Value = FileNameReplaceMode.Replace Then
                FileDateAppenderPattern = "{1}"
            Else
                If FileDateTimePositionEnd Then FileDateAppenderPattern = "{0}_{1}" Else FileDateAppenderPattern = "{1}_{0}"
            End If
        End If
    End Sub
#Region "Script"
    Friend Shared Sub ScriptTextBoxButtonClick(ByRef TXT As TextBoxExtended, ByVal Sender As ActionButton)
        If Sender.DefaultButton = ActionButton.DefaultButtons.Open Then
            Dim f As SFile = SFile.SelectFiles(TXT.Text, False, "Select script file").FirstOrDefault
            If Not f.IsEmptyString Then TXT.Text = f.ToString & " ""{0}"""
        End If
    End Sub
    Friend ReadOnly Property ScriptData As XMLValueAttribute(Of String, Boolean)
#End Region
#Region "USERS"
    Friend Sub LoadUsers()
        Try
            Users.ListClearDispose
            UsersList.Clear()
            If UsersSettingsFile.Exists Then
                Using x As New XmlFile(UsersSettingsFile, Protector.Modes.All, False) With {.AllowSameNames = True}
                    x.LoadData()
                    UsersList.ListAddList(x, LAP.IgnoreICopier)
                End Using

                Dim NeedUpdate As Boolean = False
                Dim i%, indx%, c%
                Dim UsersListInitialCount% = UsersList.Count
                Dim iUser As UserInfo
                Dim userFileExists As Boolean
                Dim __plugins As List(Of KeyValuePair(Of String, String))

                If UsersList.Count > 0 Then
                    __plugins = Plugins.Select(Function(p) New KeyValuePair(Of String, String)(p.Key, p.Name)).ToList
                    For i% = UsersList.Count - 1 To 0 Step -1
                        iUser = UsersList(i)
                        With iUser
                            'Check plugins
                            If .Plugin.IsEmptyString Then
                                If .Site.IsEmptyString Then
                                    MyMainLOG = $"The corresponding plugin was not found for the user [{ .Name}]. The user was removed from SCrawler."
                                Else
                                    indx = __plugins.FindIndex(Function(p) p.Value.ToLower = .Site.ToLower)
                                    If indx >= 0 Then
                                        .Plugin = __plugins(indx).Key
                                        .Site = __plugins(indx).Value
                                        NeedUpdate = True
                                    Else
                                        .Protected = True
                                        MyMainLOG = $"The corresponding plugin was not found for the user [{ .Name}]."
                                    End If
                                End If
                            Else
                                If Not __plugins.Exists(Function(p) p.Key.ToLower = .Plugin.ToLower) Then
                                    .Protected = True
                                    MyMainLOG = $"The corresponding plugin was not found for the user [{ .Plugin}:{ .Site}: { .Name}]."
                                End If
                            End If

                            'Check paths
                            c = IIf((Not .IncludedInCollection Or (.Merged Or .IsVirtual)) And Not .Plugin = PathPlugin.PluginKey, 1, 2)
                            userFileExists = SFile.GetPath(.File.CutPath(c - 1).Path).Exists(SFO.Path, False)
                            If Not .IsProtected Then
                                If userFileExists Then
                                    If .LastSeen.HasValue Then .LastSeen = Nothing : NeedUpdate = True
                                Else
                                    .LastSeen = Now
                                    MyMainLOG = $"The user [{ .Site}: { .Name}]  was not found. " &
                                                $"It will be removed from SCrawler on { .LastSeen.Value.ToStringDate(DateTimeDefaultProvider)}."
                                    NeedUpdate = True
                                End If
                            ElseIf userFileExists Then
                                If .Protected Then
                                    If Not .LastSeen.HasValue Then .LastSeen = Now : NeedUpdate = True
                                    MyMainLOG = $"The corresponding plugin was not found for the user [{ .Site}: { .Name}]. " &
                                                $"It will be removed from SCrawler on { .LastSeen.Value.ToStringDate(DateTimeDefaultProvider)}."
                                Else
                                    If .LastSeen.HasValue Then .LastSeen = Nothing : NeedUpdate = True
                                End If
                            ElseIf If(.LastSeen, Now).AddDays(30) < Now Then
                                UsersList.RemoveAt(i)
                                MyMainLOG = $"The user [{ .Site}: { .Name}] was not found and was removed from SCrawler."
                                NeedUpdate = True
                                Continue For
                            End If
                        End With
                        UsersList(i) = iUser
                    Next

                    If UsersList.Count > 0 Then
                        With UsersList
                            'Create collections
                            Dim d As New Dictionary(Of String, List(Of UserInfo))
                            .Where(Function(u) u.IncludedInCollection And Not u.IsProtected).ListIfNothing.
                             ListForEachCopy(Of List(Of UserInfo))(Function(ByVal u As UserInfo, ByVal ii As Integer) As UserInfo
                                                                       If Not d.ContainsKey(u.CollectionName) Then
                                                                           d.Add(u.CollectionName, New List(Of UserInfo) From {u})
                                                                       Else
                                                                           d(u.CollectionName).Add(u)
                                                                       End If
                                                                       Return u
                                                                   End Function, True, EDP.ThrowException)
                            If d.Count > 0 Then
                                For Each kv As KeyValuePair(Of String, List(Of UserInfo)) In d
                                    Users.Add(New UserDataBind(kv.Key))
                                    With DirectCast(Users.Last, UserDataBind)
                                        MainFrameObj.CollectionHandler(.Self)
                                        For i = 0 To kv.Value.Count - 1 : .Self.Add(kv.Value(i), False) : Next
                                    End With
                                Next
                                d.Clear()
                            End If

                            'Create users
                            .Where(Function(u) Not u.IncludedInCollection And Not u.IsProtected).
                             ListIfNothing.ListForEach(Sub(u, ii) Users.Add(UserDataBase.GetInstance(u, False)))
                        End With
                    End If
                End If

                If Users.Count > 0 Then
                    'Load user data
                    Dim t As New List(Of Task)
                    For Each user As IUserData In Users : t.Add(Task.Run(AddressOf user.LoadUserInformation)) : Next
                    Task.WaitAll(t.ToArray)
                    t.Clear()

                    'Users final check
                    Dim findWrongUser As Func(Of UserInfo, Boolean) = Function(ByVal u As UserInfo) As Boolean
                                                                          Dim uIndex% = UsersList.IndexOf(u)
                                                                          If uIndex >= 0 Then
                                                                              Dim uu As UserInfo = UsersList(indx)
                                                                              If Not uu.LastSeen.HasValue Then
                                                                                  uu.LastSeen = Now
                                                                                  NeedUpdate = True
                                                                                  UsersList(indx) = uu
                                                                                  MyMainLOG = $"The user [{uu.Site}: {uu.Name}]  was not found. " &
                                                                                              $"It will be removed from SCrawler on {uu.LastSeen.Value.ToStringDate(DateTimeDefaultProvider)}."
                                                                              End If
                                                                          End If
                                                                          Return uIndex >= 0
                                                                      End Function
                    Users.ListDisposeRemoveAll(Function(ByVal u As UserDataBase) As Boolean
                                                   If Not u.User.IsProtected Then
                                                       If u.IsCollection Then
                                                           With DirectCast(u, UserDataBind)
                                                               If .Count > 0 Then
                                                                   Dim __del As Boolean
                                                                   For i% = .Count - 1 To 0 Step -1
                                                                       __del = False
                                                                       If Not .Item(i).FileExists Then
                                                                           With DirectCast(.Item(i), UserDataBase).User
                                                                               If Not findWrongUser(.Self) Then
                                                                                   __del = True
                                                                                   MyMainLOG = $"The user [{ .Site}: { .Name}] was not found and was removed from SCrawler."
                                                                               End If
                                                                           End With
                                                                           If __del Then .Item(i).Delete()
                                                                           .Item(i).Dispose()
                                                                           .Collections.RemoveAt(i)
                                                                       End If
                                                                   Next
                                                               End If
                                                               Return Not .FileExists
                                                           End With
                                                       Else
                                                           If Not u.FileExists Then
                                                               If Not findWrongUser(u.User) Then MyMainLOG = $"The user [{u.User.Site}: {u.User.Name}]  was not found."
                                                               Return True
                                                           Else
                                                               Return False
                                                           End If
                                                       End If
                                                   Else
                                                       Return False
                                                   End If
                                               End Function)
                End If

                If NeedUpdate Or Not UsersList.Count = UsersListInitialCount Then
                    If UsersList.Count = 0 Then UsersSettingsFile.Delete() Else UpdateUsersList()
                End If
            End If
            If Users.Count > 0 Then
                Labels.AddRange(Users.SelectMany(Function(u) u.Labels), False)
                Labels.Update()
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private _UserListUpdateRequired As Boolean = False
    Friend ReadOnly Property UserListUpdateRequired As Boolean
        Get
            Return _UserListUpdateRequired
        End Get
    End Property
    Friend Overloads Sub UpdateUsersList(ByVal u As UserInfo)
        Dim i% = UsersList.IndexOf(u)
        If i >= 0 Then
            UsersList(i) = u
        Else
            UsersList.Add(u)
        End If
        UpdateUsersList()
    End Sub
    Friend Overloads Sub UpdateUsersList()
        Try
            If UsersList.Count > 0 Then
                Using x As New XmlFile With {.AllowSameNames = True, .Name = "Users"}
                    x.AddRange(UsersList)
                    x.Save(UsersSettingsFile)
                End Using
            End If
            _UserListUpdateRequired = False
        Catch ex As Exception
            _UserListUpdateRequired = True
        End Try
    End Sub
    Friend Overloads Function GetUser(ByVal User As IUserData, Optional ByVal GetCollection As Boolean = False) As IUserData
        Return GetUser(If(User?.Key, String.Empty), GetCollection)
    End Function
    Friend Overloads Function GetUser(ByVal UserKey As String, Optional ByVal GetCollection As Boolean = False) As IUserData
        If Users.Count > 0 And Not UserKey.IsEmptyString Then
            Dim finder As Predicate(Of IUserData) = Function(u) u.Key = UserKey
            Dim i%, ii%
            For i = 0 To Users.Count - 1
                With Users(i)
                    If finder.Invoke(.Self) Then
                        Return .Self
                    ElseIf .IsCollection Then
                        With DirectCast(.Self, UserDataBind)
                            If .Count > 0 Then
                                ii = .Collections.FindIndex(finder)
                                If ii >= 0 Then Return If(GetCollection, .Self, .Collections(ii))
                            End If
                        End With
                    End If
                End With
            Next
        End If
        Return Nothing
    End Function
    Friend Overloads Function GetUser(ByVal User As UserInfo) As IUserData
        If Users.Count > 0 Then
            Dim i%, ii%
            For i = 0 To Users.Count - 1
                With Users(i)
                    If .IsCollection Then
                        If User.IncludedInCollection And .CollectionName = User.CollectionName Then
                            With DirectCast(.Self, UserDataBind)
                                If .Count > 0 Then
                                    For ii = 0 To .Count - 1
                                        If DirectCast(.Item(ii), UserDataBase).User.Equals(User) Then Return .Item(ii)
                                    Next
                                End If
                            End With
                            Return Nothing
                        End If
                    ElseIf Not User.IncludedInCollection Then
                        If DirectCast(.Self, UserDataBase).User.Equals(User) Then Return .Self
                    End If
                End With
            Next
        End If
        Return Nothing
    End Function
    Friend Function GetUsers(ByVal Predicate As Predicate(Of IUserData)) As IEnumerable(Of IUserData)
        With Users
            If .Count > 0 Then
                Dim fp As Func(Of IUserData, Boolean) = FPredicate(Of IUserData).ToFunc(Predicate)
                Return .SelectMany(Of IUserData)(Function(ByVal user As IUserData) As IEnumerable(Of IUserData)
                                                     If user.IsCollection Then
                                                         With DirectCast(user, UserDataBind)
                                                             If .Count > 0 Then Return .Where(fp)
                                                         End With
                                                     Else
                                                         If Predicate.Invoke(user) Then Return {user}
                                                     End If
                                                     Return New IUserData() {}
                                                 End Function)
            End If
        End With
        Return New IUserData() {}
    End Function
#End Region
    Friend Sub UpdateBlackList()
        If BlackList.Count > 0 Then
            TextSaver.SaveTextToFile(BlackList.ListToString(vbNewLine), BlackListFile, True, False, EDP.None)
        Else
            BlackListFile.Delete(, Settings.DeleteMode)
        End If
    End Sub
    Friend Sub DeleteCachePath()
        Reddit.ChannelsCollection.ChannelsPathCache.Delete(SFO.Path, SFODelete.None, EDP.None)
    End Sub
    Friend Overloads Function UserExists(ByVal UserSite As String, ByVal UserID As String) As Boolean
        Dim UserFinderBase As Predicate(Of IUserData) = Function(user) user.Site = UserSite And user.Name = UserID
        Dim UserFinder As Predicate(Of IUserData) = Function(ByVal user As IUserData) As Boolean
                                                        If user.IsCollection Then
                                                            With DirectCast(user, UserDataBind)
                                                                Return .Count > 0 AndAlso .Collections.Exists(UserFinderBase)
                                                            End With
                                                        Else
                                                            Return UserFinderBase.Invoke(user)
                                                        End If
                                                    End Function
        Return Users.Count > 0 AndAlso Users.Exists(UserFinder)
    End Function
    Friend Overloads Function UserExists(ByVal _User As IUserData) As Boolean
        Return UserExists(_User.Site, _User.Name)
    End Function
    Friend Overloads Function UserExists(ByVal _User As UserInfo) As Boolean
        Return UserExists(_User.Site, _User.Name)
    End Function
    Private _UpdatesSuspended As Boolean = True
    Friend Sub BeginUpdate()
        MyXML.BeginUpdate()
        _UpdatesSuspended = True
        If Plugins.Count > 0 Then Plugins.ForEach(Sub(p) p.Settings.Source.BeginUpdate())
    End Sub
    Friend Sub EndUpdate()
        If Plugins.Count > 0 Then Plugins.ForEach(Sub(p) p.Settings.Source.EndUpdate())
        MyXML.EndUpdate()
        If MyXML.ChangesDetected Then MyXML.UpdateData()
        _UpdatesSuspended = False
        ChangeDateProvider(Nothing, Nothing)
    End Sub
    Default Friend ReadOnly Property Site(ByVal PluginKey As String) As SettingsHost
        Get
            Dim i% = Plugins.FindIndex(Function(p) p.Key = PluginKey)
            If i >= 0 Then Return Plugins(i).Settings Else Return Nothing
        End Get
    End Property
    Friend ReadOnly Property GlobalPath As XMLValue(Of SFile)
    Friend ReadOnly Property LastCopyPath As XMLValue(Of SFile)
    Friend ReadOnly Property SeparateVideoFolder As XMLValue(Of Boolean)
    Friend ReadOnly Property CollectionsPath As XMLValue(Of String)
    Friend ReadOnly Property CollectionsPathF As SFile
        Get
            If GlobalPath.IsEmptyString Then
                Throw New ArgumentNullException("GlobalPath", "GlobalPath not set")
            Else
                Return SFile.GetPath($"{GlobalPath.Value.PathWithSeparator}{CollectionsPath.Value}")
            End If
        End Get
    End Property
    Friend ReadOnly Property MaxUsersJobsCount As XMLValue(Of Integer)
    Friend ReadOnly Property ImgurClientID As XMLValue(Of String)
    Private ReadOnly Property CookiesEncrypted As XMLValue(Of Boolean)
    Friend ReadOnly Property AddMissingToLog As XMLValue(Of Boolean)
    Friend ReadOnly Property AddMissingErrorsToLog As XMLValue(Of Boolean)
    Friend ReadOnly Property UserAgent As XMLValue(Of String)
#Region "Search"
    Friend ReadOnly Property SearchInName As XMLValue(Of Boolean)
    Friend ReadOnly Property SearchInDescription As XMLValue(Of Boolean)
    Friend ReadOnly Property SearchInLabel As XMLValue(Of Boolean)
#End Region
#Region "Defaults"
    Friend ReadOnly Property DefaultTemporary As XMLValue(Of Boolean)
    Friend ReadOnly Property DefaultDownloadImages As XMLValue(Of Boolean)
    Friend ReadOnly Property DefaultDownloadVideos As XMLValue(Of Boolean)
    Friend ReadOnly Property ChangeReadyForDownOnTempChange As XMLValue(Of Boolean)
    Friend ReadOnly Property DownloadNativeImageFormat As XMLValue(Of Boolean)
    Friend ReadOnly Property ReparseMissingInTheRoutine As XMLValue(Of Boolean)
    Friend ReadOnly Property UserSiteNameAsFriendly As XMLValue(Of Boolean)
    Friend ReadOnly Property UserSiteNameUpdateEveryTime As XMLValue(Of Boolean)
#End Region
#Region "User data"
    Friend ReadOnly Property FromChannelDownloadTop As XMLValue(Of Integer)
    Friend ReadOnly Property FromChannelDownloadTopUse As XMLValue(Of Boolean)
    Friend ReadOnly Property FromChannelCopyImageToUser As XMLValue(Of Boolean)
    Friend ReadOnly Property UpdateUserDescriptionEveryTime As XMLValue(Of Boolean)
#Region "File naming"
    Friend ReadOnly Property FileAddDateToFileName As XMLValue(Of Boolean)
    Friend ReadOnly Property FileAddTimeToFileName As XMLValue(Of Boolean)
    Friend ReadOnly Property FileDateTimePositionEnd As XMLValue(Of Boolean)
    Friend ReadOnly Property FileReplaceNameByDate As XMLValue(Of Integer)
#End Region
#End Region
#Region "View"
    Friend ReadOnly Property FastProfilesLoading As XMLValue(Of Boolean)
    Friend ReadOnly Property MaxLargeImageHeight As XMLValue(Of Integer)
    Friend ReadOnly Property MaxSmallImageHeight As XMLValue(Of Integer)
    Friend ReadOnly Property UserListBackColor As XMLValue(Of Color)
    Friend ReadOnly Property UserListBackColorF As Color
        Get
            Return If(UserListBackColor.Exists, UserListBackColor.Value, SystemColors.Window)
        End Get
    End Property
    Friend ReadOnly Property UserListForeColor As XMLValue(Of Color)
    Friend ReadOnly Property UserListForeColorF As Color
        Get
            Return If(UserListForeColor.Exists, UserListForeColor.Value, SystemColors.WindowText)
        End Get
    End Property
    Friend ReadOnly Property UserListImage As XMLValue(Of SFile)
    Friend ReadOnly Property DownloadOpenInfo As XMLValueAttribute(Of Boolean, Boolean)
    Friend ReadOnly Property DownloadOpenProgress As XMLValueAttribute(Of Boolean, Boolean)
    Friend ReadOnly Property DownloadsCompleteCommand As XMLValueAttribute(Of String, Boolean)
    Friend ReadOnly Property ClosingCommand As XMLValueAttribute(Of String, Boolean)
    Friend ReadOnly Property InfoViewMode As XMLValue(Of Integer)
    Friend ReadOnly Property ViewMode As XMLValue(Of Integer)
    Friend ReadOnly Property ViewModeIsPicture As Boolean
        Get
            Select Case ViewMode.Value
                Case View.LargeIcon, View.SmallIcon : Return True
                Case Else : Return False
            End Select
        End Get
    End Property
    Friend ReadOnly Property ShowingMode As XMLValue(Of Integer)
    Friend ReadOnly Property ShowGroups As XMLValue(Of Boolean)
    Friend ReadOnly Property UseGrouping As XMLValue(Of Boolean)
    Friend ReadOnly Property ShowGroupsInsteadLabels As XMLValue(Of Boolean)
    Friend ReadOnly Property SelectedSites As XMLValuesCollection(Of String)
#Region "View dates"
    Private ReadOnly _ViewDateFrom As XMLValue(Of Date)
    Friend Property ViewDateFrom As Date?
        Get
            If _ViewDateFrom.ValueF.Exists Then Return _ViewDateFrom.Value Else Return Nothing
        End Get
        Set(ByVal d As Date?)
            If Not d.HasValue Then _ViewDateFrom.ValueF = Nothing Else _ViewDateFrom.Value = d.Value.Date
        End Set
    End Property
    Private ReadOnly _ViewDateTo As XMLValue(Of Date)
    Friend Property ViewDateTo As Date?
        Get
            If _ViewDateTo.ValueF.Exists Then Return _ViewDateTo.Value Else Return Nothing
        End Get
        Set(ByVal d As Date?)
            If Not d.HasValue Then _ViewDateTo.ValueF = Nothing Else _ViewDateTo.Value = d.Value.Date
        End Set
    End Property
    Friend ReadOnly Property ViewDateMode As XMLValue(Of Integer)
#End Region
#End Region
#Region "Latest values"
    Friend ReadOnly Property LatestSavingPath As XMLValue(Of SFile)
    Friend ReadOnly Property LatestSelectedChannel As XMLValue(Of String)
    Friend ReadOnly Property LatestDownloadedSites As XMLValuesCollection(Of String)
#End Region
#Region "Channels properties"
    Friend ReadOnly Property ChannelsDefaultReadyForDownload As XMLValue(Of Boolean)
    Friend ReadOnly Property ChannelsDefaultTemporary As XMLValue(Of Boolean)
    Friend ReadOnly Property ChannelsRegularCheckMD5 As XMLValue(Of Boolean)
    Friend ReadOnly Property ChannelsImagesRows As XMLValue(Of Integer)
    Friend ReadOnly Property ChannelsImagesColumns As XMLValue(Of Integer)
    Friend ReadOnly Property ChannelsHideExistsUser As XMLValue(Of Boolean)
    Friend ReadOnly Property ChannelsMaxJobsCount As XMLValue(Of Integer)
    Friend ReadOnly Property ChannelsAddUserImagesFromAllChannels As XMLValue(Of Boolean)
#End Region
#Region "Feed properties"
    Friend ReadOnly Property FeedDataColumns As XMLValue(Of Integer)
    Friend ReadOnly Property FeedDataRows As XMLValue(Of Integer)
    Friend ReadOnly Property FeedCenterImage As XMLValueUse(Of Integer)
    Friend ReadOnly Property FeedEndless As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedAddDateToCaption As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedAddSessionToCaption As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedStoreSessionsData As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedBackColor As XMLValue(Of Color)
    Friend ReadOnly Property FeedForeColor As XMLValue(Of Color)
#End Region
#Region "New version properties"
    Friend ReadOnly Property CheckUpdatesAtStart As XMLValue(Of Boolean)
    Friend ReadOnly Property ShowNewVersionNotification As XMLValue(Of Boolean)
    Friend ReadOnly Property LatestVersion As XMLValue(Of String)
#End Region
#Region "Notifications"
    Friend Enum NotificationObjects
        All
        Profiles
        AutoDownloader
        Channels
        SavedPosts
    End Enum
    Friend ReadOnly Property ProcessNotification(ByVal Sender As NotificationObjects) As Boolean
        Get
            If Not NotificationsSilentMode And ShowNotifications Then
                Select Case Sender
                    Case NotificationObjects.All : Return ShowNotifications
                    Case NotificationObjects.Profiles : Return ShowNotificationsDownProfiles
                    Case NotificationObjects.AutoDownloader : Return ShowNotificationsDownAutoDownloader
                    Case NotificationObjects.Channels : Return ShowNotificationsDownChannels
                    Case NotificationObjects.SavedPosts : Return ShowNotificationsDownSavedPosts
                    Case Else : Return True
                End Select
            Else
                Return False
            End If
        End Get
    End Property
    Friend Property NotificationsSilentMode As Boolean = False
    Friend ReadOnly Property ShowNotifications As XMLValue(Of Boolean)
    Friend ReadOnly Property ShowNotificationsDownProfiles As XMLValue(Of Boolean)
    Friend ReadOnly Property ShowNotificationsDownAutoDownloader As XMLValue(Of Boolean)
    Friend ReadOnly Property ShowNotificationsDownChannels As XMLValue(Of Boolean)
    Friend ReadOnly Property ShowNotificationsDownSavedPosts As XMLValue(Of Boolean)
#End Region
#Region "Other program properties"
    Friend ReadOnly Property ExitConfirm As XMLValue(Of Boolean)
    Friend ReadOnly Property CloseToTray As XMLValue(Of Boolean)
    Friend ReadOnly Property OpenFolderInOtherProgram As XMLValueAttribute(Of String, Boolean)
    Friend ReadOnly Property DeleteToRecycleBin As XMLValue(Of Boolean)
    Friend ReadOnly Property DeleteMode As SFODelete
        Get
            Return If(DeleteToRecycleBin, SFODelete.DeleteToRecycleBin, SFODelete.None)
        End Get
    End Property
#End Region
#Region "IDisposable Support"
    Private disposedValue As Boolean = False
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                If UserListUpdateRequired Then UpdateUsersList()
                If Not Channels Is Nothing Then
                    Channels.Dispose()
                    DeleteCachePath()
                End If
                If Not Automation Is Nothing Then Automation.Dispose()
                CachePath.Delete(SFO.Path, SFODelete.DeletePermanently, EDP.None)
                Plugins.Clear()
                LastCollections.Clear()
                Users.ListClearDispose
                UsersList.Clear()
                SelectedSites.Dispose()
                Design.Dispose()
                MyXML.Dispose()
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