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
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Tools
Imports SCrawler.API
Imports SCrawler.API.Base
Imports SCrawler.Plugin.Hosts
Imports SCrawler.DownloadObjects
Imports IDownloaderSettings = SCrawler.DownloadObjects.STDownloader.IDownloaderSettings
Imports DoubleClickBehavior = SCrawler.DownloadObjects.STDownloader.DoubleClickBehavior
Friend Class SettingsCLS : Implements IDownloaderSettings, IDisposable
#Region "Constants: defaults"
    Friend Const DefaultMaxDownloadingTasks As Integer = 5
    Friend Const TaskStackNamePornSite As String = "Porn sites"
    Friend Const Name_Node_Sites As String = "Sites"
    Private Const SitesValuesSeparator As String = ","
    Friend Const CookieEncryptKey As String = "SCrawlerCookiesEncryptKeyword"
    Private Const EnvironmentPath As String = "Environment\"
    Friend Const CollectionsFolderName As String = "Collections"
    Private Const PermanentCacheSnapshotsPath As String = "_CacheSnapshots\"
    Friend Const DefaultCmdEncoding As Integer = BatchExecutor.UnicodeEncoding
    Friend Const FeedCurrentLoadLastSessionDefault As Integer = 60 * 6
#End Region
#Region "Constants: headers"
    Friend Const HEADER_DEF_sec_ch_ua As String = "sec-ch-ua"
    Friend Const HEADER_DEF_sec_ch_ua_full_version_list As String = "sec-ch-ua-full-version-list"
    Friend Const HEADER_DEF_sec_ch_ua_platform As String = "sec-ch-ua-platform"
    Friend Const HEADER_DEF_sec_ch_ua_platform_version As String = "sec-ch-ua-platform-version"
    Friend Const HEADER_DEF_UserAgent As String = "UserAgent"
#End Region
#Region "XML files"
    Friend ReadOnly Design As XmlFile
    Private ReadOnly MyXML As XmlFile
#End Region
#Region "Media environment"
    Friend ReadOnly Property EnvironmentProgramsList As List(Of String)
    Friend Sub UpdateEnvironmentPrograms()
        Dim a As Action(Of ProgramFile) = Sub(f) If f.Exists Then EnvironmentProgramsList.Add(f.File.ToString)
        EnvironmentProgramsList.Clear()
        a.Invoke(FfmpegFile)
        a.Invoke(YtdlpFile)
        a.Invoke(GalleryDLFile)
        a.Invoke(CurlFile)
        Plugins.ForEach(Sub(p) p.Settings.UpdateEnvironmentPrograms(EnvironmentProgramsList, CMDEncoding.Value))
    End Sub
    Friend Sub UpdatePluginsUserAgent(Optional ByVal InvokeUpdate As Boolean = True)
        If Not UserAgent.IsEmptyString Then
            If InvokeUpdate Then BeginUpdate()
            Dim __userAgent$ = UserAgent
            Plugins.ForEach(Sub(p) p.Settings.ListForEach(Sub(ps, psi) ps.Source.UserAgentDefault = __userAgent))
            If InvokeUpdate Then EndUpdate()
        End If
    End Sub
    Friend Class ProgramFile
        Friend Const File_FFMPEG As String = "ffmpeg.exe"
        Friend Const File_YTDLP As String = "yt-dlp.exe"
        Friend Const File_GDL As String = "gallery-dl.exe"
        Friend Const File_CURL As String = "curl.exe"
        Friend Const File_CIRL_ADDIT_PATH As String = "cURL"
        Private ReadOnly XML As XMLValue(Of SFile)
        Friend Property File As SFile
            Get
                Return XML.Value
            End Get
            Set(ByVal f As SFile)
                XML.Value = f
                _Exists = f.Exists
            End Set
        End Property
        Private _Exists As Boolean = False
        Friend ReadOnly Property Exists As Boolean
            Get
                Return _Exists
            End Get
        End Property
        Friend ReadOnly Property ChangesDetected As Boolean
            Get
                Return XML.ChangesDetected
            End Get
        End Property
        Friend Sub New(ByVal ElementName As String, ByRef Container As EContainer, ByVal Nodes As String(), ByVal Program As String,
                       Optional ByVal AdditPath As String = Nothing)
            XML = New XMLValue(Of SFile)(ElementName,, Container, Nodes)
            If Not XML.Value.Exists Then
                If Program.CSFile.Exists Then
                    XML.Value = Program
                Else
                    XML.Value = Find(Program, AdditPath)
                End If
            End If
            If Not XML.Value.Exists Then XML.Value = Nothing
            _Exists = File.Exists
        End Sub
        Friend Shared Function Find(ByVal Program As String, Optional ByVal AdditPath As String = Nothing) As SFile
            If $"{EnvironmentPath}{Program}".CSFile.Exists Then
                Return $"{EnvironmentPath}{Program}"
            ElseIf Not AdditPath.IsEmptyString AndAlso $"{AdditPath}\{Program}".CSFile.Exists Then
                Return $"{AdditPath}\{Program}"
            ElseIf Not AdditPath.IsEmptyString AndAlso $"{EnvironmentPath}{AdditPath}\{Program}".CSFile.Exists Then
                Return $"{EnvironmentPath}{AdditPath}\{Program}"
            Else
                Return SystemEnvironment.FindFileInPaths(Program).ListIfNothing.FirstOrDefault
            End If
        End Function
        Public Shared Widening Operator CType(ByVal f As ProgramFile) As SFile
            Return f.File
        End Operator
        Public Shared Narrowing Operator CType(ByVal f As ProgramFile) As String
            Return f.ToString
        End Operator
        Public Overrides Function ToString() As String
            Return File.ToString
        End Function
    End Class
    Friend ReadOnly Property FfmpegFile As ProgramFile
    Friend ReadOnly Property UseM3U8 As Boolean
        Get
            Return FfmpegFile.Exists
        End Get
    End Property
    Private ReadOnly FFMPEGNotification As XMLValue(Of Boolean)
    Friend ReadOnly Property YtdlpFile As ProgramFile
    Friend ReadOnly Property GalleryDLFile As ProgramFile
    Friend ReadOnly Property CurlFile As ProgramFile
    Private ReadOnly Property ENVIR_FFMPEG As SFile Implements IDownloaderSettings.ENVIR_FFMPEG
        Get
            Return FfmpegFile
        End Get
    End Property
    Private ReadOnly Property ENVIR_YTDLP As SFile Implements IDownloaderSettings.ENVIR_YTDLP
        Get
            Return YtdlpFile
        End Get
    End Property
    Private ReadOnly Property ENVIR_GDL As SFile Implements IDownloaderSettings.ENVIR_GDL
        Get
            Return GalleryDLFile
        End Get
    End Property
    Private ReadOnly Property ENVIR_CURL As SFile Implements IDownloaderSettings.ENVIR_CURL
        Get
            Return CurlFile
        End Get
    End Property
#End Region
#Region "Declarations"
    Friend ReadOnly Property Cache As CacheKeeper
    Private _CacheSnapshots As CacheKeeper = Nothing
    Friend ReadOnly Property CacheSnapshots(ByVal Permanent As Boolean) As CacheKeeper
        Get
            If Permanent Then
                If _CacheSnapshots Is Nothing Then _CacheSnapshots = New CacheKeeper(PermanentCacheSnapshotsPath) With {.DeleteCacheOnDispose = False, .DeleteRootOnDispose = False}
                Return _CacheSnapshots
            Else
                Dim dir As SFile = $"{Cache.RootDirectory.PathWithSeparator}Snapshots\"
                Cache.AddPath(dir)
                Return Cache.GetInstance(dir)
            End If
        End Get
    End Property
    Friend ReadOnly Plugins As List(Of PluginHost)
    Friend ReadOnly Property Users As List(Of IUserData)
    Friend ReadOnly Property UsersList As List(Of UserInfo)
    Private ReadOnly Property UsersListProtected As Boolean = False
    Friend Property Channels As Reddit.ChannelsCollection
    Friend ReadOnly Property Labels As LabelsKeeper
    Friend ReadOnly Property Groups As Groups.DownloadGroupCollection
    Friend ReadOnly Property AdvancedFilter As Groups.DownloadGroup
    Friend ReadOnly Property LastCollections As List(Of String)
    Friend ReadOnly Property DownloadLocations As STDownloader.DownloadLocationsCollection
    Friend ReadOnly Property GlobalLocations As STDownloader.DownloadLocationsCollection
#Region "Scheduler"
    Friend Property Automation As Scheduler
    Friend ReadOnly Property AutomationFile As XMLValue(Of String)
    Friend ReadOnly Property AutomationScript As XMLValueUse(Of String)
    Friend ReadOnly Property AutomationScript_ExcludeManual As XMLValue(Of Boolean)
#End Region
    Friend ReadOnly Property Feeds As FeedSpecialCollection
    Friend ReadOnly Property BlackList As List(Of UserBan)
    Friend ReadOnly Property Colors As Editors.DataColorCollection
    Friend Property FeedViews As FeedViewCollection
    Private ReadOnly BlackListFile As SFile = $"{SettingsFolderName}\BlackList.txt"
    Private ReadOnly UsersSettingsFile As SFile = $"{SettingsFolderName}\Users.xml"
    Private ReadOnly Property SettingsVersion As XMLValue(Of Integer)
    Private Const SettingsVersionCurrent As Integer = 2
    Friend ShortcutOpenFeed As New ButtonKey(Keys.F, True)
    Friend ShortcutOpenSearch As New ButtonKey(Keys.F,, True)
    Private Sub ChangeFeedOpenMode()
        If FeedOpenCtrlF Then
            ShortcutOpenFeed = New ButtonKey(Keys.F, True)
            ShortcutOpenSearch = New ButtonKey(Keys.F,, True)
        Else
            ShortcutOpenFeed = New ButtonKey(Keys.F,, True)
            ShortcutOpenSearch = New ButtonKey(Keys.F, True)
        End If
    End Sub
#End Region
#Region "Initializer"
    Friend Sub New()
        CheckNewReleaseFolder()

        Cache = CacheKeeper.Default
        Cache.DisposeSuspended = True

        Design = XmlFile.Design
        MyXML = New XmlFile(Nothing) With {.AutoUpdateFile = True}
        MyXML.BeginUpdate()
        Users = New List(Of IUserData)
        UsersList = New List(Of UserInfo)
        BlackList = New List(Of UserBan)
        Plugins = New List(Of PluginHost)
        LastCollections = New List(Of String)
        GlobalLocations = New STDownloader.DownloadLocationsCollection
        GlobalLocations.Load(True,, $"{SettingsFolderName}\GlobalLocations.xml")
        Feeds = New FeedSpecialCollection
        Colors = New Editors.DataColorCollection
        EnvironmentProgramsList = New List(Of String)

        SettingsVersion = New XMLValue(Of Integer)("SettingsVersion", 0, MyXML)
        UsersListProtected = MyXML.Value("UsersListProtected").FromXML(Of Boolean)(False)

        Dim n() As String = {"Scheduler"}
        AutomationFile = New XMLValue(Of String)("File",, MyXML, n)
        AutomationScript = New XMLValueUse(Of String)("Script", String.Empty,, MyXML, n)
        AutomationScript_ExcludeManual = New XMLValue(Of Boolean)("ScriptExcludeManual", True, MyXML, n)

        SiteSettingsShowHiddenControls = MyXML.Value("SiteSettingsShowHiddenControls").FromXML(Of Boolean)(False)

#Region "Properties: environment"
        'Environment
        n = {"MediaEnvironment"}
        EnvironmentPath.CSFileP.Exists(SFO.Path, True)
        FfmpegFile = New ProgramFile("ffmpeg", MyXML, n, ProgramFile.File_FFMPEG)
        FFMPEGNotification = New XMLValue(Of Boolean)("FFMPEGNotification", True, MyXML, n)
        If Not FfmpegFile.Exists Then
            If FFMPEGNotification.Value AndAlso
               MsgBoxE({"[ffmpeg.exe] is missing", "ffmpeg.exe"}, vbExclamation,,,
                       {"OK", New MsgBoxButton("Disable notification", "Disable ffmpeg missing notification")}) = 1 Then FFMPEGNotification.Value = False
        Else
            FFMPEGNotification.Value = True
        End If

        YtdlpFile = New ProgramFile("ytdlp", MyXML, n, ProgramFile.File_YTDLP)
        GalleryDLFile = New ProgramFile("gallerydl", MyXML, n, ProgramFile.File_GDL)
        CurlFile = New ProgramFile("curl", MyXML, n, ProgramFile.File_CURL, ProgramFile.File_CIRL_ADDIT_PATH)
        CMDEncoding = New XMLValue(Of Integer)("CMDEncoding", DefaultCmdEncoding, MyXML, n)
#End Region
#Region "Properties"
        'Basis
        n = {"Basis"}
        GlobalPath = New XMLValue(Of SFile)("GlobalPath", "Data\".CSFileP, MyXML, n, New XMLToFilePathProvider)
        MaxLargeImageHeight = New XMLValue(Of Integer)("MaxLargeImageHeight", 150, MyXML, n)
        MaxSmallImageHeight = New XMLValue(Of Integer)("MaxSmallImageHeight", 15, MyXML, n)
        CollectionsPath = New XMLValue(Of String)("CollectionsPath", CollectionsFolderName, MyXML, n)
        MaxUsersJobsCount = New XMLValue(Of Integer)("MaxJobsCount", DefaultMaxDownloadingTasks, MyXML, n)
        ImgurClientID = New XMLValue(Of String)("ImgurClientID", String.Empty, MyXML, {Name_Node_Sites})

        'Basis: new version
        n = {"ProgramVersion"}
        CheckUpdatesAtStart = New XMLValue(Of Boolean)("CheckUpdatesAtStart", True, MyXML, n)
        ShowNewVersionNotification = New XMLValue(Of Boolean)("ShowNewVersionNotification", True, MyXML, n)
        LatestVersion = New XMLValue(Of String)("LatestVersion", String.Empty, MyXML, n)

        'Design
        n = {"Design"}
        ProgramText = New XMLValue(Of String)("ProgramText",, MyXML, n)
        ProgramDescription = New XMLValue(Of String)("ProgramDescription",, MyXML, n)
        UserListImage = New XMLValue(Of SFile)("UserListImage",, MyXML, n)
        UserListBackColor = New XMLValue(Of Color)
        UserListBackColor.SetExtended("UserListBackColor",, MyXML, n)
        UserListForeColor = New XMLValue(Of Color)
        UserListForeColor.SetExtended("UserListForeColor",, MyXML, n)
        MainFrameUsersSubscriptionsColorBack = New XMLValue(Of Color)("UsersSubscriptionsColorBack", MyColor.OkBack, MyXML, n)
        MainFrameUsersSubscriptionsColorFore = New XMLValue(Of Color)("UsersSubscriptionsColorFore", MyColor.OkFore, MyXML, n)
        MainFrameUsersSubscriptionsColorBack_USERS = New XMLValue(Of Color)
        MainFrameUsersSubscriptionsColorBack_USERS.SetExtended("UsersSubscriptionsColorBack_USERS",, MyXML, n)
        MainFrameUsersSubscriptionsColorFore_USERS = New XMLValue(Of Color)
        MainFrameUsersSubscriptionsColorFore_USERS.SetExtended("UsersSubscriptionsColorFore_USERS",, MyXML, n)

        'Default headers
        n = {"DefaultHeaders"}
        HEADER_sec_ch_ua = New XMLValue(Of String)("sec_ch_ua",, MyXML, n)
        HEADER_sec_ch_ua_full_version_list = New XMLValue(Of String)("sec_ch_ua_full_version_list",, MyXML, n)
        HEADER_sec_ch_ua_platform = New XMLValue(Of String)("sec_ch_ua_platform",, MyXML, n)
        HEADER_sec_ch_ua_platform_version = New XMLValue(Of String)("sec_ch_ua_platform_version",, MyXML, n)
        HEADER_UserAgent = New XMLValue(Of String)("UserAgent",, MyXML, n)

        'Behavior
        n = {"Behavior"}
        ExitConfirm = New XMLValue(Of Boolean)("ExitConfirm", True, MyXML, n)
        CloseToTray = New XMLValue(Of Boolean)("CloseToTray", True, MyXML, n)
        FastProfilesLoading = New XMLValue(Of Boolean)("FastProfilesLoading", True, MyXML, n)
        DeleteToRecycleBin = New XMLValue(Of Boolean)("DeleteToRecycleBin", True, MyXML, n)
        OpenFolderInOtherProgram = New XMLValueUse(Of String)("OpenFolderInOtherProgram",,, MyXML, n)
        DownloadOpenInfo = New XMLValueAttribute(Of Boolean, Boolean)("DownloadOpenInfo", "OpenAgain", False, False, MyXML, n)
        DownloadOpenProgress = New XMLValueAttribute(Of Boolean, Boolean)("DownloadOpenProgress", "OpenAgain", False, False, MyXML, n)
        ClosingCommand = New XMLValueAttribute(Of String, Boolean)("ClosingCommand", "Use",,, MyXML, n)
        AddHandler ClosingCommand.ValueChanged, Sub(s, ev) MainFrameObj?.ChangeCloseVisible()
        DownloadAll_UseF6 = New XMLValue(Of Boolean)("DownloadAll_UseF6", True, MyXML, n)
        DownloadAll_UseF6_Confirm = New XMLValue(Of Boolean)("DownloadAll_UseF6_Confirm", False, MyXML, n)
        DownloadAll_Confirm = New XMLValue(Of Boolean)("DownloadAll_Confirm", False, MyXML, n)
        FeedOpenCtrlF = New XMLValue(Of Boolean)("FeedOpenCtrlF", True, MyXML, n)
        AddHandler FeedOpenCtrlF.ValueChanged, AddressOf ChangeFeedOpenMode
        ChangeFeedOpenMode()

        'Notifications
        n = {"Notifications"}
        ShowNotifications = New XMLValue(Of Boolean)("ShowNotifications", True, MyXML, n)
        ShowNotificationsDownProfiles = New XMLValue(Of Boolean)("Profiles", True, MyXML, n)
        ShowNotificationsDownAutoDownloader = New XMLValue(Of Boolean)("AutoDownloader", True, MyXML, n)
        ShowNotificationsDownChannels = New XMLValue(Of Boolean)("Channels", True, MyXML, n)
        ShowNotificationsDownSavedPosts = New XMLValue(Of Boolean)("SavedPosts", True, MyXML, n)
        ShowNotificationsSTDownloader = New XMLValue(Of Boolean)("STDownloader", True, MyXML, n)
        ShowNotificationsSTDownloaderEveryDownload = New XMLValue(Of Boolean)("STDownloaderEveryDownload", True, MyXML, n)
        ShowNotificationsLOG = New XMLValue(Of Boolean)("LOG", True, MyXML, n)

        'Defaults
        n = {"Defaults"}
        SeparateVideoFolder = New XMLValue(Of Boolean)("SeparateVideoFolder", True, MyXML, n)
        DefaultTemporary = New XMLValue(Of Boolean)("Temporary", False, MyXML, n)
        DefaultDownloadImages = New XMLValue(Of Boolean)("DownloadImages", True, MyXML, n)
        DefaultDownloadVideos = New XMLValue(Of Boolean)("DownloadVideos", True, MyXML, n)
        DownloadNativeImageFormat = New XMLValue(Of Boolean)("DownloadNativeImageFormat", True, MyXML, n)
        UserSiteNameAsFriendly = New XMLValue(Of Boolean)("UserSiteNameAsFriendly", True, MyXML, n)

        'STDownloader
        n = {"Downloader"}
        STDownloader_UpdateYouTubeOutputPath = New XMLValue(Of Boolean)("UpdateYouTubeOutputPath", False, MyXML, n)
        STDownloader_MaxJobsCount = New XMLValue(Of Integer)("MaxJobsCount", 1, MyXML, n)
        STDownloader_DownloadAutomatically = New XMLValue(Of Boolean)("DownloadAutomatically", True, MyXML, n)
        STDownloader_RemoveDownloadedAutomatically = New XMLValue(Of Boolean)("RemoveDownloadedAutomatically", False, MyXML, n)
        STDownloader_OnItemDoubleClick = New XMLValue(Of DoubleClickBehavior)("OnItemDoubleClick", DoubleClickBehavior.Folder, MyXML, n)
        STDownloader_TakeSnapshot = New XMLValue(Of Boolean)("TakeSnapshot", True, MyXML, n)
        STDownloader_SnapshotsKeepWithFiles = New XMLValue(Of Boolean)("SnapshotsKeepWithFiles", True, MyXML, n)
        STDownloader_SnapshotsKeepWithFiles_ThumbAlong = New XMLValue(Of Boolean)("SnapshotsKeepWithFiles_ThumbAlong", False, MyXML, n)
        STDownloader_SnapShotsCachePermamnent = New XMLValue(Of Boolean)("SnapShotsCachePermamnent", False, MyXML, n)
        STDownloader_RemoveYTVideosOnClear = New XMLValue(Of Boolean)("RemoveYouTubeVideosOnClear", False, MyXML, n)
        STDownloader_LoadYTVideos = New XMLValue(Of Boolean)("LoadYouTubeVideos", False, MyXML, n)
        STDownloader_OutputPathUseYT = New XMLValue(Of Boolean)("OutputPathUseYT", False, MyXML, n)
        STDownloader_OutputPathAskForName = New XMLValue(Of Boolean)("OutputPathAskForName", True, MyXML, n)
        STDownloader_OutputPathAutoAddPaths = New XMLValue(Of Boolean)("OutputPathAutoAddPaths", True, MyXML, n)
        STDownloader_CreateUrlFiles = New XMLValue(Of Boolean)("CreateUrlFiles", False, MyXML, n)
        DownloadLocations = New STDownloader.DownloadLocationsCollection
        DownloadLocations.Load(False, STDownloader_OutputPathUseYT)

        'Downloading
        n = {"Downloading"}
        UpdateUserDescriptionEveryTime = New XMLValue(Of Boolean)("UpdateUserDescriptionEveryTime", True, MyXML, n)
        UpdateUserSiteNameEveryTime = New XMLValue(Of Boolean)("UpdateUserSiteNameEveryTime", False, MyXML, n)
        UpdateUserIconBannerEveryTime = New XMLValue(Of Boolean)("UpdateUserIconBannerEveryTime", True, MyXML, n)
        AddMissingToLog = New XMLValue(Of Boolean)("AddMissingToLog", True, MyXML, n)
        AddMissingErrorsToLog = New XMLValue(Of Boolean)("AddMissingErrorsToLog", False, MyXML, n)
        DownloadsCompleteCommand = New XMLValueAttribute(Of String, Boolean)("DownloadsCompleteCommand", "Use",,, MyXML, n)
        ReparseMissingInTheRoutine = New XMLValue(Of Boolean)("ReparseMissingInTheRoutine", False, MyXML, n)
        UseDefaultAccountIfMissing = New XMLValue(Of Boolean)("UseDefaultAccountIfMissing", True, MyXML, n)
        AutomationBrushUndownloadedPlansMinutes = New XMLValue(Of Integer)("AutomationBrushUndownloadedPlansMinutes", 10080, MyXML, n)
        DownDetectorEnabled = New XMLValue(Of Boolean)("DownDetectorEnabled", True, MyXML, n)
        'TODELETE: DownDetectorEnabled change
        If SettingsVersion.Value < SettingsVersionCurrent Then DownDetectorEnabled.Value = False 'SettingsVersionCurrent = 2

        'Downloading: file naming
        n = {"Downloading", "FileName"}
        FileAddDateToFileName = New XMLValue(Of Boolean)("FileAddDateToFileName", False, MyXML, n)
        AddHandler FileAddDateToFileName.ValueChanged, AddressOf ChangeDateProvider
        FileAddTimeToFileName = New XMLValue(Of Boolean)("FileAddTimeToFileName", False, MyXML, n)
        AddHandler FileAddTimeToFileName.ValueChanged, AddressOf ChangeDateProvider
        FileDateTimePositionEnd = New XMLValue(Of Boolean)("FileDateTimePositionEnd", True, MyXML, n)
        AddHandler FileDateTimePositionEnd.ValueChanged, AddressOf ChangeDateProvider
        FileReplaceNameByDate = New XMLValue(Of Integer)("FileReplaceNameByDate", FileNameReplaceMode.None, MyXML, n)

        'Downloading: script
        n = {"Downloading", "Script"}
        ScriptData = New XMLValueAttribute(Of String, Boolean)("ScriptData", "Use",,, MyXML, n)

        'Channels
        n = {Name_Node_Sites, "Channels"}
        ChannelsImagesRows = New XMLValue(Of Integer)("ImagesRows", 2, MyXML, n)
        ChannelsImagesColumns = New XMLValue(Of Integer)("ImagesColumns", 5, MyXML, n)
        ChannelsAddUserImagesFromAllChannels = New XMLValue(Of Boolean)("AddUserImagesFromAllChannels", True, MyXML, n)
        ChannelsDefaultReadyForDownload = New XMLValue(Of Boolean)("ChannelsDefaultReadyForDownload", False, MyXML, n)
        ChannelsDefaultTemporary = New XMLValue(Of Boolean)("ChannelsDefaultTemporary", True, MyXML, n)
        ChannelsHideExistsUser = New XMLValue(Of Boolean)("HideExistsUser", True, MyXML, n)
        ChannelsMaxJobsCount = New XMLValue(Of Integer)("MaxJobsCount", DefaultMaxDownloadingTasks, MyXML, n)
        n = {Name_Node_Sites, "Channels", "Users"}
        FromChannelDownloadTop = New XMLValue(Of Integer)("FromChannelDownloadTop", 10, MyXML, n)
        FromChannelDownloadTopUse = New XMLValue(Of Boolean)("FromChannelDownloadTopUse", False, MyXML, n)
        FromChannelCopyImageToUser = New XMLValue(Of Boolean)("FromChannelCopyImageToUser", True, MyXML, n)

        'Feed
        n = {"Feed"}
        FeedDataRows = New XMLValue(Of Integer)("DataRows", 10, MyXML, n)
        FeedDataColumns = New XMLValue(Of Integer)("DataColumns", 1, MyXML, n)
        FeedCenterImage = New XMLValueUse(Of Integer)("FeedCenterImage", 1,, MyXML, n)
        FeedBackColor = New XMLValue(Of Color)
        FeedBackColor.SetExtended("FeedColorBack",, MyXML, n)
        FeedForeColor = New XMLValue(Of Color)
        FeedForeColor.SetExtended("FeedColorFore",, MyXML, n)
        FeedEndless = New XMLValue(Of Boolean)("Endless", True, MyXML, n)
        FeedAddSessionToCaption = New XMLValue(Of Boolean)("AddSessionToCaption", False, MyXML, n)
        FeedAddSiteToCaption = New XMLValue(Of Boolean)("AddSiteToCaption", True, MyXML, n)
        FeedAddTypeToCaption = New XMLValue(Of Boolean)("AddTypeToCaption", False, MyXML, n)
        FeedAddDateToCaption = New XMLValue(Of Boolean)("AddDateToCaption", True, MyXML, n)
        FeedStoreSessionsData = New XMLValue(Of Boolean)("StoreSessionsData", True, MyXML, n)
        FeedStoredSessionsNumber = New XMLValue(Of Integer)("StoredSessionsNumber", 20, MyXML, n)
        FeedCurrentTryLoadLastSession = New XMLValue(Of Integer)("CurrentTryLoadLastSession", FeedCurrentLoadLastSessionDefault, MyXML, n)
        FeedOpenLastMode = New XMLValue(Of Boolean)("OpenLastMode", False, MyXML, n)
        FeedLastModeSubscriptions = New XMLValue(Of Boolean)("LastModeSubscriptions", False, MyXML, n)
        FeedShowFriendlyNames = New XMLValue(Of Boolean)("ShowFriendlyNames", True, MyXML, n)
        FeedShowSpecialFeedsMediaItem = New XMLValue(Of Boolean)("ShowSpecialFeedsMediaItem", False, MyXML, n)
        FeedEscToClose = New XMLValue(Of Boolean)("EscToClose", True, MyXML, n)
        FeedSpecialSearchForMissing = New XMLValue(Of Boolean)("FeedSpecialSearchForMissing", True, MyXML, n)
        FeedSpecialSearchForMissing_Deep = New XMLValue(Of Boolean)("FeedSpecialSearchForMissing_Deep", False, MyXML, n)
        n = {"Feed", "MoveCopy"}
        FeedMoveCopyLastLocation = New XMLValue(Of SFile)("LastLocation",, MyXML, n)
        FeedMoveCopyUpdateFileLocationOnMove = New XMLValue(Of Boolean)("UpdateFileLocationOnMove", True, MyXML, n)
        FeedMoveCopyIsProfileChecked = New XMLValue(Of Boolean)("IsProfileChecked", True, MyXML, n)
        FeedMoveCopySeparateVideo = New XMLValue(Of Boolean)("SeparateVideo",, MyXML, n)
        FeedMoveCopyReplaceUserProfile = New XMLValue(Of Boolean)("ReplaceUserProfile",, MyXML, n)
        FeedMoveCopyCreatePathProfile = New XMLValue(Of Boolean)("CreatePathProfile",, MyXML, n)

        'View (filters)
        n = {"Filter"}
        ViewMode = New XMLValue(Of Integer)("ViewMode", ViewModes.IconLarge, MyXML, n)
        ShowAllUsers = New XMLValue(Of Boolean)("ShowAllUsers", True, MyXML, n)
        GroupUsers = New XMLValue(Of Boolean)("GroupUsers", True, MyXML, n)
        ShowGroupsInsteadLabels = New XMLValue(Of Boolean)("ShowGroupsInsteadLabels", False, MyXML, n)

        'Info form
        n = {"InfoForm"}
        InfoViewMode = New XMLValue(Of Integer)("ViewMode", DownloadedInfoForm.ViewModes.Session, MyXML, n)
        InfoViewDefault = New XMLValue(Of Boolean)("ViewDefault", True, MyXML, n)

        'Search form
        n = {"Search"}
        SearchInName = New XMLValue(Of Boolean)("SearchInName", True, MyXML, n)
        SearchInDescription = New XMLValue(Of Boolean)("SearchInDescription", False, MyXML, n)
        SearchInLabel = New XMLValue(Of Boolean)("SearchInLabel", False, MyXML, n)

        'User metrics form
        n = {"Metrics"}
        UMetrics_What = New XMLValue(Of Integer)("What", -1, MyXML, n)
        UMetrics_Order = New XMLValue(Of Integer)("Order", SortOrder.Descending, MyXML, n)
        UMetrics_ShowDrives = New XMLValue(Of Boolean)("ShowDrives", True, MyXML, n)
        UMetrics_ShowCollections = New XMLValue(Of Boolean)("ShowCollections", True, MyXML, n)

        'Latest values
        n = {"LastValues"}
        LastCopyPath = New XMLValue(Of SFile)("LastCopyPath",, MyXML, n, New XMLToFilePathProvider)
        LatestSavingPath = New XMLValue(Of SFile)("LatestSavingPath", Nothing, MyXML, n, New XMLToFilePathProvider)
        LatestSelectedChannel = New XMLValue(Of String)("LatestSelectedChannel",, MyXML, n)
#End Region
#Region "Loading plugins"
        Plugins.AddRange(PluginHost.GetMyHosts(MyXML, GlobalPath.Value, DefaultTemporary, DefaultDownloadImages, DefaultDownloadVideos))
        Dim tmpPluginList As IEnumerable(Of PluginHost) = PluginHost.GetPluginsHosts(MyXML, GlobalPath.Value, DefaultTemporary,
                                                                                     DefaultDownloadImages, DefaultDownloadVideos)
        If tmpPluginList.ListExists Then
            Dim tplIndx% = -1
            For Each tpl As PluginHost In tmpPluginList
                If tpl.IsReplacer Then
                    tplIndx = Plugins.FindIndex(Function(pl) pl.Key.StringToLower = tpl.Replacer.PluginKey.StringToLower Or
                                                             pl.Name.StringToLower = tpl.Replacer.SiteName.StringToLower)
                    If tplIndx >= 0 Then
                        Plugins(tplIndx).Dispose()
                        Plugins.RemoveAt(tplIndx)
                        If Plugins.Count = 0 Then Exit For
                    End If
                End If
            Next
            Plugins.AddRange(tmpPluginList)
        End If
        UpdateEnvironmentPrograms()
        UpdatePluginsUserAgent(False)
#End Region

        Labels = New LabelsKeeper
        Groups = New Groups.DownloadGroupCollection
        Labels.AddRange(Groups.GetGroupsLabels, False)
        AdvancedFilter = New Groups.DownloadGroup(False) With {.IsViewFilter = True}
        AdvancedFilter.LoadFromFile($"{SettingsFolderName}\AdvancedFilter.xml")
        AdvancedFilter.IsViewFilter = True
        Labels.AddRange({AdvancedFilter}.GetGroupsLabels, False)

        SettingsVersion.Value = SettingsVersionCurrent

        MyXML.EndUpdate()
        If MyXML.ChangesDetected Then MyXML.Sort() : MyXML.UpdateData()

        If BlackListFile.Exists Then
            BlackList.ListAddList(IO.File.ReadAllLines(BlackListFile), LAP.NotContainsOnly)
            If BlackList.Count > 0 Then BlackList.RemoveAll(Function(b) Not b.Exists)
        End If
        _UpdatesSuspended = False
        ChangeDateProvider(Nothing, Nothing)
    End Sub
#End Region
#Region "Users"
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
                Dim i%, indx%
                Dim errStr As Func(Of Date, String) = Function(d) IIf(UsersListProtected, String.Empty,
                                                                      $"It will be removed from SCrawler on {d.ToStringDate(DateTimeDefaultProvider)}.")
                Dim UsersListInitialCount% = UsersList.Count
                Dim iUser As UserInfo
                Dim userFileExists As Boolean, pluginFound As Boolean
                Dim __plugins As List(Of KeyValuePair(Of String, String))

                If UsersList.Count > 0 Then
                    __plugins = Plugins.Select(Function(p) New KeyValuePair(Of String, String)(p.Key, p.Name)).ToList
                    For i% = UsersList.Count - 1 To 0 Step -1
                        iUser = UsersList(i)
                        pluginFound = True
                        With iUser
                            'Check plugins
                            If .Plugin.IsEmptyString Then
                                pluginFound = False
                                If .Site.IsEmptyString Then
                                    MyMainLOG = $"The corresponding plugin was not found for the user [{ .Name}].{IIf(UsersListProtected, String.Empty, " The user has been removed from SCrawler.")}"
                                Else
                                    indx = __plugins.FindIndex(Function(p) p.Value.ToLower = .Site.ToLower)
                                    If indx >= 0 Then
                                        .Plugin = __plugins(indx).Key
                                        .Site = __plugins(indx).Value
                                        NeedUpdate = True
                                        pluginFound = True
                                    Else
                                        .Protected = True
                                        MyMainLOG = $"The corresponding plugin was not found for the user [{ .Name}]."
                                        pluginFound = False
                                    End If
                                End If
                            Else
                                If Not __plugins.Exists(Function(p) p.Key.ToLower = .Plugin.ToLower) Then
                                    pluginFound = False
                                    .Protected = True
                                    MyMainLOG = $"The corresponding plugin was not found for the user [{ .Plugin}:{ .Site}: { .Name}]."
                                End If
                            End If

                            'Check paths
                            userFileExists = .File.Exists
                            If Not pluginFound Or Not userFileExists Then
                                If Not .IsProtected Then
                                    If userFileExists Then
                                        If .LastSeen.HasValue Then .LastSeen = Nothing : NeedUpdate = True
                                    Else
                                        .LastSeen = Now
                                        MyMainLOG = $"The user [{ .Site}: { .Name}]  was not found. " & errStr(.LastSeen.Value)
                                        NeedUpdate = True
                                    End If
                                ElseIf userFileExists Then
                                    If .Protected Then
                                        If Not .LastSeen.HasValue Then .LastSeen = Now : NeedUpdate = True
                                        MyMainLOG = $"The corresponding plugin was not found for the user [{ .Site}: { .Name}]. " & errStr(.LastSeen.Value)
                                    Else
                                        If .LastSeen.HasValue Then .LastSeen = Nothing : NeedUpdate = True
                                    End If
                                ElseIf If(.LastSeen, Now).AddDays(30) < Now And Not UsersListProtected Then
                                    UsersList.RemoveAt(i)
                                    MyMainLOG = $"The user [{ .Site}: { .Name}] was not found and was removed from SCrawler."
                                    NeedUpdate = True
                                    Continue For
                                End If
                            Else
                                If .LastSeen.HasValue Then .LastSeen = Nothing : NeedUpdate = True
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
                                                                              Dim uu As UserInfo = UsersList(uIndex)
                                                                              If Not uu.LastSeen.HasValue Then
                                                                                  uu.LastSeen = Now
                                                                                  NeedUpdate = True
                                                                                  UsersList(uIndex) = uu
                                                                                  MyMainLOG = $"The user [{uu.Site}: {uu.Name}]  was not found. " & errStr(uu.LastSeen.Value)
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
                                                                               If Not findWrongUser(.Self) And Not UsersListProtected Then
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
    Friend Overloads Sub UpdateUsersList(ByVal u As UserInfo, Optional ByVal OnlyDiff As Boolean = False)
        Dim result As Boolean = True
        Dim i% = UsersList.IndexOf(u)
        If i >= 0 Then
            If Not OnlyDiff OrElse Not UserInfo.ExactEquals(UsersList(i), u) Then
                UsersList(i) = u
            Else
                result = False
            End If
        Else
            UsersList.Add(u)
        End If
        If result Then UpdateUsersList()
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
#Region "User exists"
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
#End Region
#Region "Cache"
    Friend Sub DeleteCachePath()
        Reddit.ChannelsCollection.ChannelsPathCache.Delete(SFO.Path, SFODelete.None, EDP.None)
    End Sub
    Private Sub DeleteCachePathPermanent()
        Try
            Dim f As New SFile(PermanentCacheSnapshotsPath)
            If f.Exists(SFO.Path, False) AndAlso Not SFile.GetFiles(f,, IO.SearchOption.AllDirectories, EDP.ReturnValue).ListExists Then _
               f.Delete(SFO.Path, SFODelete.DeletePermanently, EDP.None)
        Catch
        End Try
    End Sub
#End Region
#Region "Update"
    Private _UpdatesSuspended As Boolean = True
    Friend Sub BeginUpdate()
        MyXML.BeginUpdate()
        _UpdatesSuspended = True
        If Plugins.Count > 0 Then Plugins.ForEach(Sub(p) p.Settings.BeginUpdate())
    End Sub
    Friend Sub EndUpdate()
        If Plugins.Count > 0 Then Plugins.ForEach(Sub(p) p.Settings.EndUpdate())
        MyXML.EndUpdate()
        If MyXML.ChangesDetected Then MyXML.UpdateData()
        _UpdatesSuspended = False
        ChangeDateProvider(Nothing, Nothing)
    End Sub
#End Region
#Region "Update black list"
    Friend Sub UpdateBlackList()
        If BlackList.Count > 0 Then
            TextSaver.SaveTextToFile(BlackList.ListToString(vbNewLine), BlackListFile, True, False, EDP.None)
        Else
            BlackListFile.Delete(, Settings.DeleteMode)
        End If
    End Sub
#End Region
#Region "Site (plugins)"
    Default Friend Overloads ReadOnly Property Site(ByVal PluginKey As String) As SettingsHostCollection
        Get
            Dim i% = Plugins.FindIndex(Function(p) p.Key = PluginKey)
            If i >= 0 Then Return Plugins(i).Settings Else Return Nothing
        End Get
    End Property
    Default Friend Overloads ReadOnly Property Site(ByVal PluginKey As String, ByVal AccountName As String) As SettingsHost
        Get
            Dim i% = Plugins.FindIndex(Function(p) p.Key = PluginKey)
            If i >= 0 Then Return Plugins(i).Settings(AccountName) Else Return Nothing
        End Get
    End Property
#End Region
#Region "Properties"
#Region "Basis"
    Friend ReadOnly Property GlobalPath As XMLValue(Of SFile)
    Friend ReadOnly Property MaxLargeImageHeight As XMLValue(Of Integer)
    Friend ReadOnly Property MaxSmallImageHeight As XMLValue(Of Integer)
    Friend ReadOnly Property CollectionsPath As XMLValue(Of String)
    Friend ReadOnly Property CollectionsPathF As SFile
        Get
            If GlobalPath.IsEmptyString Then
                Throw New ArgumentNullException("GlobalPath", "Global path not set")
            Else
                Return SFile.GetPath($"{GlobalPath.Value.PathWithSeparator}{CollectionsPath.Value}")
            End If
        End Get
    End Property
    Friend ReadOnly Property MaxUsersJobsCount As XMLValue(Of Integer)
    Friend ReadOnly Property UserAgent As String
        Get
            Return HEADER_UserAgent
        End Get
    End Property
    Friend ReadOnly Property ImgurClientID As XMLValue(Of String)
#End Region
#Region "Basis: new version"
    Friend ReadOnly Property CheckUpdatesAtStart As XMLValue(Of Boolean)
    Friend ReadOnly Property ShowNewVersionNotification As XMLValue(Of Boolean)
    Friend ReadOnly Property LatestVersion As XMLValue(Of String)
#End Region
#Region "Design"
    Friend ReadOnly Property ProgramText As XMLValue(Of String)
    Friend ReadOnly Property ProgramDescription As XMLValue(Of String)
    Friend ReadOnly Property UserListImage As XMLValue(Of SFile)
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
    Friend ReadOnly Property MainFrameUsersSubscriptionsColorBack As XMLValue(Of Color)
    Friend ReadOnly Property MainFrameUsersSubscriptionsColorFore As XMLValue(Of Color)
    Friend ReadOnly Property MainFrameUsersSubscriptionsColorBack_USERS As XMLValue(Of Color)
    Friend ReadOnly Property MainFrameUsersSubscriptionsColorFore_USERS As XMLValue(Of Color)
#End Region
#Region "Environment"
    Friend ReadOnly Property CMDEncoding As XMLValue(Of Integer)
#End Region
#Region "Default headers"
    Friend ReadOnly Property HEADER_sec_ch_ua As XMLValue(Of String)
    Friend ReadOnly Property HEADER_sec_ch_ua_full_version_list As XMLValue(Of String)
    Friend ReadOnly Property HEADER_sec_ch_ua_platform As XMLValue(Of String)
    Friend ReadOnly Property HEADER_sec_ch_ua_platform_version As XMLValue(Of String)
    Friend ReadOnly Property HEADER_UserAgent As XMLValue(Of String)
#End Region
#Region "Behavior"
    Friend ReadOnly Property ExitConfirm As XMLValue(Of Boolean)
    Friend ReadOnly Property CloseToTray As XMLValue(Of Boolean)
    Friend ReadOnly Property FastProfilesLoading As XMLValue(Of Boolean)
    Friend ReadOnly Property DeleteToRecycleBin As XMLValue(Of Boolean)
    Friend ReadOnly Property DeleteMode As SFODelete
        Get
            Return If(DeleteToRecycleBin, SFODelete.DeleteToRecycleBin, SFODelete.None)
        End Get
    End Property
    Friend ReadOnly Property OpenFolderInOtherProgram As XMLValueUse(Of String)
    Private ReadOnly Property IDownloaderSettings_OpenFolderInOtherProgram As Boolean Implements IDownloaderSettings.OpenFolderInOtherProgram
        Get
            Return OpenFolderInOtherProgram.Use
        End Get
    End Property
    Private ReadOnly Property IDownloaderSettings_OpenFolderInOtherProgram_Command As String Implements IDownloaderSettings.OpenFolderInOtherProgram_Command
        Get
            Return OpenFolderInOtherProgram
        End Get
    End Property
    Friend ReadOnly Property DownloadOpenInfo As XMLValueAttribute(Of Boolean, Boolean)
    Friend ReadOnly Property DownloadOpenProgress As XMLValueAttribute(Of Boolean, Boolean)
    Friend ReadOnly Property ClosingCommand As XMLValueAttribute(Of String, Boolean)
    Friend ReadOnly Property DownloadAll_UseF6 As XMLValue(Of Boolean)
    Friend ReadOnly Property DownloadAll_UseF6_Confirm As XMLValue(Of Boolean)
    Friend ReadOnly Property DownloadAll_Confirm As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedOpenCtrlF As XMLValue(Of Boolean)
#End Region
#Region "Notifications"
    Friend Enum NotificationObjects
        All
        Profiles
        AutoDownloader
        Channels
        SavedPosts
        STDownloader
        LOG
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
                    Case NotificationObjects.STDownloader : Return ShowNotificationsSTDownloader
                    Case NotificationObjects.LOG : Return ShowNotificationsLOG
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
    Friend ReadOnly Property ShowNotificationsSTDownloader As XMLValue(Of Boolean)
    Friend ReadOnly Property ShowNotificationsSTDownloaderEveryDownload As XMLValue(Of Boolean)
    Friend ReadOnly Property ShowNotificationsLOG As XMLValue(Of Boolean)
#End Region
#Region "Defaults"
    Friend ReadOnly Property SeparateVideoFolder As XMLValue(Of Boolean)
    Friend ReadOnly Property DefaultTemporary As XMLValue(Of Boolean)
    Friend ReadOnly Property DefaultDownloadImages As XMLValue(Of Boolean)
    Friend ReadOnly Property DefaultDownloadVideos As XMLValue(Of Boolean)
    Friend ReadOnly Property DownloadNativeImageFormat As XMLValue(Of Boolean)
    Friend ReadOnly Property UserSiteNameAsFriendly As XMLValue(Of Boolean)
#End Region
#Region "STDownloader"
    Friend ReadOnly Property STDownloader_UpdateYouTubeOutputPath As XMLValue(Of Boolean)
    Private ReadOnly Property IDownloaderSettings_ShowNotifications As Boolean Implements IDownloaderSettings.ShowNotifications
        Get
            Return ProcessNotification(NotificationObjects.STDownloader)
        End Get
    End Property
    Private ReadOnly Property IDownloaderSettings_ShowNotificationsEveryDownload As Boolean Implements IDownloaderSettings.ShowNotificationsEveryDownload
        Get
            If ProcessNotification(NotificationObjects.STDownloader) Then
                Return ShowNotificationsSTDownloaderEveryDownload
            Else
                Return False
            End If
        End Get
    End Property
    Friend ReadOnly Property STDownloader_MaxJobsCount As XMLValue(Of Integer)
    Private ReadOnly Property IDownloaderSettings_MaxJobsCount As Integer Implements IDownloaderSettings.MaxJobsCount
        Get
            Return STDownloader_MaxJobsCount
        End Get
    End Property
    Friend ReadOnly Property STDownloader_DownloadAutomatically As XMLValue(Of Boolean)
    Private ReadOnly Property IDownloaderSettings_DownloadAutomatically As Boolean Implements IDownloaderSettings.DownloadAutomatically
        Get
            Return STDownloader_DownloadAutomatically
        End Get
    End Property
    Friend ReadOnly Property STDownloader_RemoveDownloadedAutomatically As XMLValue(Of Boolean)
    Private ReadOnly Property IDownloaderSettings_RemoveDownloadedAutomatically As Boolean Implements IDownloaderSettings.RemoveDownloadedAutomatically
        Get
            Return STDownloader_RemoveDownloadedAutomatically
        End Get
    End Property
    Friend ReadOnly Property STDownloader_OnItemDoubleClick As XMLValue(Of DoubleClickBehavior)
    Private ReadOnly Property IDownloaderSettings_OnItemDoubleClick As DoubleClickBehavior Implements IDownloaderSettings.OnItemDoubleClick
        Get
            Return STDownloader_OnItemDoubleClick
        End Get
    End Property
    Friend ReadOnly Property STDownloader_TakeSnapshot As XMLValue(Of Boolean)
    Friend ReadOnly Property STDownloader_SnapshotsKeepWithFiles As XMLValue(Of Boolean)
    Friend ReadOnly Property STDownloader_SnapshotsKeepWithFiles_ThumbAlong As XMLValue(Of Boolean)
    Friend ReadOnly Property STDownloader_SnapShotsCachePermamnent As XMLValue(Of Boolean)
    Friend ReadOnly Property STDownloader_RemoveYTVideosOnClear As XMLValue(Of Boolean)
    Friend ReadOnly Property STDownloader_LoadYTVideos As XMLValue(Of Boolean)
    Friend ReadOnly Property STDownloader_OutputPathUseYT As XMLValue(Of Boolean)
    Friend ReadOnly Property STDownloader_OutputPathAskForName As XMLValue(Of Boolean)
    Private ReadOnly Property IDownloaderSettings_OutputPathAskForName As Boolean Implements IDownloaderSettings.OutputPathAskForName
        Get
            Return STDownloader_OutputPathAskForName
        End Get
    End Property
    Friend ReadOnly Property STDownloader_OutputPathAutoAddPaths As XMLValue(Of Boolean)
    Private ReadOnly Property IDownloaderSettings_OutputPathAutoAddPaths As Boolean Implements IDownloaderSettings.OutputPathAutoAddPaths
        Get
            Return STDownloader_OutputPathAutoAddPaths
        End Get
    End Property
    Friend ReadOnly Property STDownloader_CreateUrlFiles As XMLValue(Of Boolean)
    Private ReadOnly Property IDownloaderSettings_CreateUrlFiles As Boolean Implements IDownloaderSettings.CreateUrlFiles
        Get
            Return STDownloader_CreateUrlFiles
        End Get
    End Property
#End Region
#Region "Downloading"
    Friend ReadOnly Property UpdateUserDescriptionEveryTime As XMLValue(Of Boolean)
    Friend ReadOnly Property UpdateUserSiteNameEveryTime As XMLValue(Of Boolean)
    Friend ReadOnly Property UpdateUserIconBannerEveryTime As XMLValue(Of Boolean)
    Friend ReadOnly Property AddMissingToLog As XMLValue(Of Boolean)
    Friend ReadOnly Property AddMissingErrorsToLog As XMLValue(Of Boolean)
    Friend ReadOnly Property DownloadsCompleteCommand As XMLValueAttribute(Of String, Boolean)
    Friend ReadOnly Property ReparseMissingInTheRoutine As XMLValue(Of Boolean)
    Friend ReadOnly Property UseDefaultAccountIfMissing As XMLValue(Of Boolean)
    Friend ReadOnly Property AutomationBrushUndownloadedPlansMinutes As XMLValue(Of Integer)
    Friend ReadOnly Property DownDetectorEnabled As XMLValue(Of Boolean)
#End Region
#Region "Downloading: file naming"
    Friend ReadOnly Property FileAddDateToFileName As XMLValue(Of Boolean)
    Friend ReadOnly Property FileAddTimeToFileName As XMLValue(Of Boolean)
    Friend ReadOnly Property FileDateTimePositionEnd As XMLValue(Of Boolean)
    Friend ReadOnly Property FileReplaceNameByDate As XMLValue(Of Integer)
#End Region
#Region "Downloading: file naming: PROVIDER"
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
#End Region
#Region "Downloading: script"
    Friend Shared Sub ScriptTextBoxButtonClick(ByRef TXT As TextBoxExtended, ByVal Sender As ActionButton)
        If Sender.DefaultButton = ActionButton.DefaultButtons.Open Then
            Dim f As SFile = SFile.SelectFiles(TXT.Text, False, "Select script file").FirstOrDefault
            If Not f.IsEmptyString Then TXT.Text = f.ToString & " ""{0}"""
        End If
    End Sub
    Friend ReadOnly Property ScriptData As XMLValueAttribute(Of String, Boolean)
#End Region
#Region "Channels"
    Friend ReadOnly Property ChannelsImagesRows As XMLValue(Of Integer)
    Friend ReadOnly Property ChannelsImagesColumns As XMLValue(Of Integer)
#Region "Channels: user"
    Friend ReadOnly Property FromChannelDownloadTop As XMLValue(Of Integer)
    Friend ReadOnly Property FromChannelDownloadTopUse As XMLValue(Of Boolean)
    Friend ReadOnly Property FromChannelCopyImageToUser As XMLValue(Of Boolean)
#End Region
    Friend ReadOnly Property ChannelsAddUserImagesFromAllChannels As XMLValue(Of Boolean)
    Friend ReadOnly Property ChannelsDefaultReadyForDownload As XMLValue(Of Boolean)
    Friend ReadOnly Property ChannelsDefaultTemporary As XMLValue(Of Boolean)
    Friend ReadOnly Property ChannelsHideExistsUser As XMLValue(Of Boolean)
    Friend ReadOnly Property ChannelsMaxJobsCount As XMLValue(Of Integer)
#End Region
#Region "Feed"
    Friend ReadOnly Property FeedDataRows As XMLValue(Of Integer)
    Friend ReadOnly Property FeedDataColumns As XMLValue(Of Integer)
    Friend ReadOnly Property FeedCenterImage As XMLValueUse(Of Integer)
    Friend ReadOnly Property FeedBackColor As XMLValue(Of Color)
    Friend ReadOnly Property FeedForeColor As XMLValue(Of Color)
    Friend ReadOnly Property FeedEndless As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedAddSessionToCaption As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedAddSiteToCaption As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedAddTypeToCaption As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedAddDateToCaption As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedStoreSessionsData As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedStoredSessionsNumber As XMLValue(Of Integer)
    Friend ReadOnly Property FeedCurrentTryLoadLastSession As XMLValue(Of Integer)
    Friend ReadOnly Property FeedOpenLastMode As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedLastModeSubscriptions As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedShowFriendlyNames As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedShowSpecialFeedsMediaItem As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedEscToClose As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedSpecialSearchForMissing As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedSpecialSearchForMissing_Deep As XMLValue(Of Boolean)
#Region "MoveCopy"
    Friend ReadOnly Property FeedMoveCopyLastLocation As XMLValue(Of SFile)
    Friend ReadOnly Property FeedMoveCopyUpdateFileLocationOnMove As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedMoveCopyIsProfileChecked As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedMoveCopySeparateVideo As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedMoveCopyReplaceUserProfile As XMLValue(Of Boolean)
    Friend ReadOnly Property FeedMoveCopyCreatePathProfile As XMLValue(Of Boolean)
#End Region
#End Region
#Region "View (filters)"
    Friend ReadOnly Property ViewMode As XMLValue(Of Integer)
    Friend ReadOnly Property ViewModeIsPicture As Boolean
        Get
            Select Case ViewMode.Value
                Case View.LargeIcon, View.SmallIcon : Return True
                Case Else : Return False
            End Select
        End Get
    End Property
    Friend ReadOnly Property ShowAllUsers As XMLValue(Of Boolean)
    Friend ReadOnly Property GroupUsers As XMLValue(Of Boolean)
    Friend ReadOnly Property ShowGroupsInsteadLabels As XMLValue(Of Boolean)
#End Region
#Region "Info form"
    Friend ReadOnly Property InfoViewMode As XMLValue(Of Integer)
    Friend ReadOnly Property InfoViewDefault As XMLValue(Of Boolean)
#End Region
#Region "Search form"
    Friend ReadOnly Property SearchInName As XMLValue(Of Boolean)
    Friend ReadOnly Property SearchInDescription As XMLValue(Of Boolean)
    Friend ReadOnly Property SearchInLabel As XMLValue(Of Boolean)
#End Region
#Region "User metrics form"
    Friend ReadOnly Property UMetrics_What As XMLValue(Of Integer)
    Friend ReadOnly Property UMetrics_Order As XMLValue(Of Integer)
    Friend ReadOnly Property UMetrics_ShowDrives As XMLValue(Of Boolean)
    Friend ReadOnly Property UMetrics_ShowCollections As XMLValue(Of Boolean)
#End Region
#Region "Latest values"
    Friend ReadOnly Property LastCopyPath As XMLValue(Of SFile)
    Friend ReadOnly Property LatestSavingPath As XMLValue(Of SFile)
    Friend ReadOnly Property LatestSelectedChannel As XMLValue(Of String)
#End Region
#End Region
#Region "IDisposable Support"
    Private disposedValue As Boolean = False
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                If UserListUpdateRequired Then UpdateUsersList()
                If UsersList.Count = 0 Then UsersSettingsFile.Delete()
                If Not Channels Is Nothing Then
                    Channels.Dispose()
                    DeleteCachePath()
                End If
                If Not Automation Is Nothing Then Automation.Dispose()
                Cache.Dispose()
                DeleteCachePathPermanent()
                Plugins.Clear()
                LastCollections.Clear()
                Users.ListClearDispose
                UsersList.Clear()
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