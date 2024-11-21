' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports System.ComponentModel
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Functions.Messaging
Imports PersonalUtilities.Tools
Imports SCrawler.API
Imports SCrawler.API.Base
Imports SCrawler.Editors
Imports SCrawler.DownloadObjects
Imports SCrawler.DownloadObjects.Groups
Imports SCrawler.Plugin.Hosts
Imports PauseModes = SCrawler.DownloadObjects.AutoDownloader.PauseModes
Public Class MainFrame
#Region "Declarations"
    Private MyView As FormView
    Private WithEvents MyActivator As FormActivator
    Private WithEvents BTT_IMPORT_USERS As ToolStripMenuItem
    Private WithEvents BTT_NEW_PROFILE As ToolStripMenuItem
    Private BTT_SHOW_ALL_GROUPS_ADDED As Boolean = False
    Private WithEvents BTT_SHOW_ALL_GROUPS As ToolStripMenuItem
    Private WithEvents BTT_GROUPS_OTHER As ToolStripMenuItem
    Friend MyChannels As ChannelViewForm
    Friend MySavedPosts As DownloadSavedPostsForm
    Private MyMissingPosts As MissingPostsForm
    Private DownloadQueue As UserDownloadQueueForm
    Friend MyFeed As DownloadFeedForm
    Private MySearch As UserSearchForm
    Private MyUserMetrics As UsersInfoForm = Nothing
    Private _UFinit As Boolean = True
#End Region
#Region "Initializer"
    Public Sub New()
        InitializeComponent()
        Settings = New SettingsCLS
        With Settings.Plugins
            If .Count > 0 Then
                For i% = 0 To .Count - 1
                    If Not .Item(i).Key = PathPlugin.PluginKey Then _
                       MENU_SETTINGS.DropDownItems.Insert(MENU_SETTINGS.DropDownItems.Count - 2, .Item(i).Settings.GetSettingsButton)
                Next
            End If
        End With
        BTT_IMPORT_USERS = New ToolStripMenuItem("Import", My.Resources.UsersIcon_32.ToBitmap)
        BTT_NEW_PROFILE = New ToolStripMenuItem("Add new profile", My.Resources.PlusPic_24)
        MENU_SETTINGS.DropDownItems.Insert(MENU_SETTINGS.DropDownItems.Count - 2, New ToolStripSeparator)
        MENU_SETTINGS.DropDownItems.Insert(MENU_SETTINGS.DropDownItems.Count - 2, BTT_NEW_PROFILE)
        MENU_SETTINGS.DropDownItems.AddRange({New ToolStripSeparator, BTT_IMPORT_USERS})
        BTT_BUG_REPORT.Image = My.Resources.MailPic_16
        BTT_GROUPS_OTHER = New ToolStripMenuItem("Other groups", DownloadGroup.GroupImage)
        BTT_SHOW_ALL_GROUPS = New ToolStripMenuItem("Show all groups", DownloadGroup.GroupImage)
    End Sub
#End Region
#Region "Form handlers"
    Private Async Sub MainFrame_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Now.Month.ValueBetween(6, 8) Then
            Text = "SCrawler: Happy LGBT Pride Month! :-)"
        ElseIf Not Settings.ProgramText.IsEmptyString Then
            Text = Settings.ProgramText
        End If
        Settings.DeleteCachePath()
        MainFrameObj = New MainFrameObjects(Me)
        MainFrameObj.ChangeCloseVisible()
        MainFrameObj.PauseButtons.AddButtons()
        STDownloader.MyNotificator = MainFrameObj
        STDownloader.MyDownloaderSettings = Settings
        YouTube.MyCache = Settings.Cache
        YouTube.MyYouTubeSettings = New YouTube.YTSettings_Internal
        UpdateYouTubeSettings()
        MainProgress = New MyProgressExt(Toolbar_BOTTOM, PR_MAIN, PR_PRE, LBL_STATUS, "Downloading profiles' data") With {
            .ResetProgressOnMaximumChanges = False, .Visible = False}
        Downloader = New TDownloader
        InfoForm = New DownloadedInfoForm
        DownloadQueue = New UserDownloadQueueForm
        MyProgressForm = New ActiveDownloadingProgress
        Downloader.ReconfPool()
        AddHandler Downloader.JobsChange, AddressOf Downloader_UpdateJobsCount
        AddHandler Downloader.Downloading, AddressOf Downloader_Downloading
        AddHandler Downloader.DownloadCountChange, AddressOf InfoForm.Downloader_DownloadCountChange
        AddHandler Downloader.SendNotification, AddressOf MainFrameObj.ShowNotification
        AddHandler Downloader.UserDownloadStateChanged, AddressOf DownloadQueue.Downloader_UserDownloadStateChanged
        AddHandler Downloader.Downloading, AddressOf DownloadQueue.Downloader_Downloading
        AddHandler InfoForm.UserFind, AddressOf FocusUser
        Settings.LoadUsers()
        MyView = New FormView(Me)
        MyView.Import(Settings.Design)
        MyView.SetFormSize()
        If Settings.CloseToTray Then TrayIcon.Visible = True
        MyActivator = New FormActivator(Me)
        With LIST_PROFILES.Groups
            .AddRange(GetLviGroupName(Nothing, True)) 'collections
            If Settings.Plugins.Count > 0 Then
                For Each h As SettingsHost In Settings.Plugins.Select(Function(hh) hh.Settings.Default) : .AddRange(GetLviGroupName(h, False)) : Next
            End If
            If Settings.Labels.Count > 0 Then Settings.Labels.ToList.ForEach(Sub(l) .Add(New ListViewGroup(l, l)))
            .Add(Settings.Labels.NoLabel)
        End With
        With Settings
            LIST_PROFILES.View = .ViewMode
            LIST_PROFILES.ShowGroups = .GroupUsers
            ApplyViewPattern(.ViewMode.Value)
            AddHandler .Labels.NewLabelAdded, AddressOf UpdateLabelsGroups
            UpdateImageColor()
            UserListLoader = New ListImagesLoader(LIST_PROFILES)
            RefillList()
            UpdateLabelsGroups()
            SetShowButtonsCheckers(.ShowAllUsers)
            CheckVersion(False)
            UpdateUserGroupControls()
            With .Groups
                AddHandler .Added, AddressOf GROUPS_Added
                AddHandler .Deleted, AddressOf GROUPS_Deleted
                AddHandler .Updated, AddressOf GROUPS_Updated
                If .Count > 0 Then
                    For Each ugroup As DownloadGroup In Settings.Groups : GROUPS_Added(ugroup) : Next
                End If
            End With
            .Automation = New Scheduler
            AddHandler .Groups.Updated, AddressOf .Automation.GROUPS_Updated
            AddHandler .Groups.Deleted, AddressOf .Automation.GROUPS_Deleted
            AddHandler .Automation.PauseChanged, AddressOf MainFrameObj.PauseButtons.UpdatePauseButtons_Handler
            If .Automation.Count > 0 Then .Labels.AddRange(.Automation.GetGroupsLabels, False) : .Labels.Update()
            _UFinit = False
            Await .Automation.Start(True)
        End With
        UpdatePauseButtonsVisibility()
    End Sub
    Private _CloseInvoked As Boolean = False
    Private _IgnoreTrayOptions As Boolean = False
    Private _IgnoreCloseConfirm As Boolean = False
    Private Async Sub MainFrame_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Settings.CloseToTray And Not _IgnoreTrayOptions Then
            e.Cancel = True
            Hide()
        Else
            If CheckForClose(_IgnoreCloseConfirm) Then
                If _CloseInvoked Then GoTo CloseResume
                Dim ChannelsWorking As Func(Of Boolean) = Function() If(MyChannels?.Working, False)
                Dim SP_Working As Func(Of Boolean) = Function() If(MySavedPosts?.Working, False)
                If (Not Downloader.Working And Not ChannelsWorking.Invoke And Not SP_Working.Invoke) OrElse
                        MsgBoxE({"The program is still downloading something..." & vbNewLine &
                                 "Are you sure you want to stop downloading and exit the program?",
                                 "Downloading in progress"},
                                MsgBoxStyle.Exclamation,,,
                                {"Stop downloading and close", "Cancel"}) = 0 Then
                    If Downloader.Working Then _CloseInvoked = True : Downloader.Stop()
                    If ChannelsWorking.Invoke Then _CloseInvoked = True : MyChannels.Stop(False)
                    If SP_Working.Invoke Then _CloseInvoked = True : MySavedPosts.Stop()
                    MyActivator.DisposeIfReady()
                    Settings.Automation.Stop()
                    If _CloseInvoked Then
                        e.Cancel = True
                        Await Task.Run(Sub()
                                           While Downloader.Working Or ChannelsWorking.Invoke Or SP_Working.Invoke : Thread.Sleep(500) : End While
                                       End Sub)
                    End If
                    MainFrameObj.OpenedGroupUsersForms.ListClearDispose
                    Downloader.Dispose()
                    MyProgressForm.Dispose()
                    InfoForm.Dispose()
                    DownloadQueue.DisposeIfReady()
                    MyMissingPosts.DisposeIfReady()
                    MyFeed.DisposeIfReady()
                    MainFrameObj.ClearNotifications()
                    MainFrameObj.PauseButtons.Dispose()
                    MyChannels.DisposeIfReady()
                    VideoDownloader.DisposeIfReady()
                    MySavedPosts.DisposeIfReady()
                    MySearch.DisposeIfReady()
                    MyUserMetrics.DisposeIfReady()
                    MyView.Dispose(Settings.Design)
                    Settings.Dispose()
                Else
                    GoTo DropCloseParams
                End If
            Else
                GoTo DropCloseParams
            End If
            GoTo CloseContinue
DropCloseParams:
            e.Cancel = True
            _IgnoreTrayOptions = False
            _IgnoreCloseConfirm = False
            _CloseInvoked = False
            Exit Sub
CloseContinue:
            If Not BATCH Is Nothing Then BATCH.Dispose() : BATCH = Nothing
            If _CloseInvoked Then Close()
CloseResume:
        End If
    End Sub
    Private _DisableClosingScript As Boolean = False
    Private Sub MainFrame_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        If Not _DisableClosingScript Then ExecuteCommand(Settings.ClosingCommand)
        If Not MyMainLOG.IsEmptyString Then SaveLogToFile()
    End Sub
    Private Sub MainFrame_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        If Not _UFinit Then UpdateImageColor()
    End Sub
    Private ListImageLastWidth As Integer = -1
    Private Sub UpdateImageColor(Optional ByVal UpdateOnlyImage As Boolean = False, Optional ByVal ForceImageUpdate As Boolean = False)
        Try
            If Settings.UserListImage.Value.Exists Then
                If ForceImageUpdate Or Not ListImageLastWidth = LIST_PROFILES.Width Or LIST_PROFILES.BackgroundImage Is Nothing Then
                    ListImageLastWidth = LIST_PROFILES.Width
                    Using ir As New ImageRenderer(Settings.UserListImage) : LIST_PROFILES.BackgroundImage = ir.FitToWidth(LIST_PROFILES.Width) : End Using
                End If
            Else
                LIST_PROFILES.BackgroundImage = Nothing
            End If
            If Not UpdateOnlyImage Then
                With Settings
                    If Not .UserListBackColorF = LIST_PROFILES.BackColor Or Not .UserListForeColorF = LIST_PROFILES.ForeColor Then
                        LIST_PROFILES.BackColor = .UserListBackColorF
                        LIST_PROFILES.ForeColor = .UserListForeColorF
                    End If
                End With
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub MainFrame_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Dim b As Boolean = True
        Select Case e.KeyCode
            Case Keys.Insert : BTT_ADD_USER_Click(Me, New Controls.KeyClick.KeyClickEventArgs(e))
            Case Keys.Delete : DeleteSelectedUser()
            Case Keys.Enter : OpenFolder()
            Case Keys.F1 : BTT_VERSION_INFO.PerformClick()
            Case Keys.F3 : EditSelectedUser()
            Case Keys.F5 : DownloadSelectedUser(DownUserLimits.None, New MyKeyEventArgs(e).IncludeInTheFeed)
            Case Keys.F6 : BTT_DOWN_ALL_KeyClick(Nothing, New MyKeyEventArgs(e))
            Case Else : b = NumGroup(e)
        End Select

        If Not b Then
            b = True
            If Settings.ShortcutOpenFeed = e Then
                BTT_FEED.PerformClick()
            ElseIf e.Alt And e.KeyCode = Keys.A Then
                BTT_DOWN_AUTOMATION.PerformClick()
            ElseIf e.Alt And e.KeyCode = Keys.P Then
                BTT_PR_INFO.PerformClick()
            ElseIf Settings.ShortcutOpenSearch = e Or (e.Alt And e.KeyCode = Keys.U) Or (e.Control And e.KeyCode = Keys.U) Then
                MySearch.FormShow()
            Else
                b = False
            End If
        End If

        If b Then e.Handled = True
    End Sub
    Private Function NumGroup(ByVal e As KeyEventArgs) As Boolean
        Dim GroupExists As Func(Of Integer, Boolean) = Function(i) Settings.Groups.DownloadGroupIfExists(i - 1)
        If e.Control And Settings.Groups.Count > 0 Then
            Select Case e.KeyCode
                Case Keys.D1, Keys.NumPad1 : Return GroupExists(1)
                Case Keys.D2, Keys.NumPad2 : Return GroupExists(2)
                Case Keys.D3, Keys.NumPad3 : Return GroupExists(3)
                Case Keys.D4, Keys.NumPad4 : Return GroupExists(4)
                Case Keys.D5, Keys.NumPad5 : Return GroupExists(5)
                Case Keys.D6, Keys.NumPad6 : Return GroupExists(6)
                Case Keys.D7, Keys.NumPad7 : Return GroupExists(7)
                Case Keys.D8, Keys.NumPad8 : Return GroupExists(8)
                Case Keys.D9, Keys.NumPad9 : Return GroupExists(9)
            End Select
        End If
        Return False
    End Function
#End Region
#Region "Form Tray"
    Private Sub MyActivator_TrayIconClick(Sender As Object, e As Controls.KeyClick.KeyClickEventArgs) Handles MyActivator.TrayIconClick
        If e.Control Then ShowFeed() : e.Handled = True
    End Sub
    Private Sub BTT_TRAY_SHOW_HIDE_Click(sender As Object, e As EventArgs) Handles BTT_TRAY_SHOW_HIDE.Click
        If Visible Then Hide() Else Show()
    End Sub
    Private Sub BTT_TRAY_CLOSE_Click(sender As Object, e As EventArgs) Handles BTT_TRAY_CLOSE.Click
        ClosePressed(False)
    End Sub
    Private Sub BTT_TRAY_CLOSE_NO_SCRIPT_Click(sender As Object, e As EventArgs) Handles BTT_TRAY_CLOSE_NO_SCRIPT.Click
        ClosePressed(True)
    End Sub
    Private Sub ClosePressed(ByVal DisableScript As Boolean)
        _DisableClosingScript = DisableScript
        If CheckForClose(False) Then _IgnoreTrayOptions = True : _IgnoreCloseConfirm = True : Close()
    End Sub
    Private Function CheckForClose(ByVal _Ignore As Boolean) As Boolean
        If Settings.ExitConfirm And Not _Ignore Then
            Return MsgBoxE({"Do you want to close the program?", "Closing the program"}, MsgBoxStyle.YesNo) = MsgBoxResult.Yes
        Else
            Return True
        End If
    End Function
#End Region
#Region "List refill, update"
    Friend Sub RefillList(Optional ByVal ForceImageUpdate As Boolean = False)
        UpdateImageColor(True, ForceImageUpdate)
        UserListLoader.Update()
    End Sub
    Private Sub UserListUpdate(ByVal User As IUserData, ByVal Add As Boolean)
        UserListLoader.UpdateUser(User, Add)
    End Sub
#End Region
#Region "Toolbar buttons"
#Region "Settings"
    Private Sub BTT_SETTINGS_Click(sender As Object, e As EventArgs) Handles BTT_SETTINGS.Click
        With Settings
            Dim mhl% = .MaxLargeImageHeight.Value
            Dim mhs% = .MaxSmallImageHeight.Value
            Using f As New GlobalSettingsForm
                f.ShowDialog()
                If f.DialogResult = DialogResult.OK Then
                    UpdateYouTubeSettings()
                    If (Not .MaxLargeImageHeight = mhl Or Not .MaxSmallImageHeight = mhs) And .ViewModeIsPicture Then
                        RefillList(f.PictureChanged)
                    ElseIf f.PictureChanged Then
                        UpdateImageColor(True, True)
                    End If
                    TrayIcon.Visible = .CloseToTray
                    If f.EnvironmentProgramsChanged Then Settings.UpdateEnvironmentPrograms()
                    If f.FeedParametersChanged And Not MyFeed Is Nothing Then MyFeed.UpdateSettings()
                    If f.HeadersChanged Or (f.UserAgentChanged And Not Settings.UserAgent.IsEmptyString) Then
                        Settings.BeginUpdate()
                        If f.UserAgentChanged Then Settings.UpdatePluginsUserAgent(False)
                        If f.HeadersChanged Then Settings.Plugins.ForEach(Sub(p) p.Settings.UpdateInheritance())
                        Settings.EndUpdate()
                    End If
                    UpdateSilentButtons()
                    UpdateImageColor()
                End If
            End Using
        End With
    End Sub
    Private Sub UpdateYouTubeSettings()
        With YouTube.MyYouTubeSettings
            If Not .YTDLP.Value.Exists And Settings.YtdlpFile.Exists Then .YTDLP.Value = Settings.YtdlpFile.File
            If Not .FFMPEG.Value.Exists And Settings.FfmpegFile.Exists Then .FFMPEG.Value = Settings.FfmpegFile.File
            If .OutputPath.IsEmptyString And Not Settings.LatestSavingPath.IsEmptyString Then .OutputPath.Value = Settings.LatestSavingPath.Value
        End With
    End Sub
    Private Sub BTT_NEW_PROFILE_Click(sender As Object, e As EventArgs) Handles BTT_NEW_PROFILE.Click
        Try
            Using f As New SimpleListForm(Of PluginHost)(Settings.Plugins, Settings.Design) With {
                .DesignXMLNodeName = "PluginsChooserForm",
                .Mode = SimpleListFormModes.SelectedItems,
                .MultiSelect = False,
                .FormText = "Available plugins",
                .Icon = My.Resources.SettingsIcon_48
            }
                If f.ShowDialog = DialogResult.OK AndAlso f.DataResult.Count > 0 Then f.DataResult.First.Settings.CreateAbstract()
            End Using
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "MainFrame.CreateNewProfile]")
        End Try
    End Sub
    Private Sub BTT_IMPORT_USERS_Click(sender As Object, e As EventArgs) Handles BTT_IMPORT_USERS.Click
        Const MsgTitle$ = "Import users"
        Try
            Dim file As SFile = Nothing
            Dim _OriginalLocations As Boolean = False
            Select Case MsgBoxE({"Where do you want to import users from?" & vbCr & vbCr &
                                 "This feature is not for importing users from the site. It's more like searching for missing users.", MsgTitle}, vbQuestion,,,
                                {"Select path", New MsgBoxButton("Current", "All plugin paths will be checked"), "Cancel"}).Index
                Case 0 : file = SFile.SelectPath
                Case 1 : _OriginalLocations = True
                Case Else : MsgBoxE({"Operation canceled", MsgTitle}) : Exit Sub
            End Select
            If Not file.IsEmptyString Or _OriginalLocations Then
                Using import As New UserFinder(file)
                    With import
                        .Find(_OriginalLocations)
                        If .Count > 0 Then
                            .Verify()
                            .Dialog()
                        Else
                            MsgBoxE({"No users found", MsgTitle})
                        End If
                    End With
                End Using
            End If
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, MsgTitle)
        End Try
    End Sub
#End Region
#Region "Add, Edit, Delete, Refresh"
    Private Sub OnUsersAddedHandler(ByVal StartIndex As Integer)
        If StartIndex <= Settings.Users.Count - 1 Then
            For i% = StartIndex To Settings.Users.Count - 1 : UserListUpdate(Settings.Users(i), True) : Next
        End If
    End Sub
    Private Sub BTT_ADD_USER_Click(ByVal Sender As Object, ByVal e As Controls.KeyClick.KeyClickEventArgs) Handles BTT_ADD_USER.KeyClick
        Dim f As UserCreatorForm = Nothing
        If e.Control Then
            Dim tmpBufferUrl$ = BufferText
            If Not tmpBufferUrl.IsEmptyString Then f = UserCreatorForm.TryCreate(tmpBufferUrl)
        End If
        If f Is Nothing Then
            f = New UserCreatorForm
            f.ShowDialog()
            If Not (f.DialogResult = DialogResult.OK Or f.StartIndex >= 0) Then f.Dispose() : f = Nothing
        End If
        If Not f Is Nothing Then
            Dim i%
            If f.StartIndex >= 0 Then
                OnUsersAddedHandler(f.StartIndex)
            Else
                Dim SimpleUser As Predicate(Of IUserData) = Function(u) u.Site = f.User.Site And u.Name = f.User.Name
                i = Settings.Users.FindIndex(Function(u) If(u.IsCollection, DirectCast(u, UserDataBind).Collections.Exists(SimpleUser), SimpleUser.Invoke(u)))
                If i < 0 Then
                    If Not UserBanned(f.User.Name) Then
                        Settings.UpdateUsersList(f.User)
                        Settings.Users.Add(UserDataBase.GetInstance(f.User))
                        With Settings.Users.Last
                            If Not .FileExists Then
                                .Options = f.Options
                                .Favorite = f.UserFavorite
                                .Temporary = f.UserTemporary
                                .ParseUserMediaOnly = f.UserMediaOnly
                                .ReadyForDownload = f.UserReady
                                .DownloadImages = f.DownloadImages
                                .DownloadVideos = f.DownloadVideos
                                .FriendlyName = f.UserFriendly
                                .BackColor = f.UserBackColor
                                .ForeColor = f.UserForeColor
                                .Description = f.UserDescr
                                .ScriptUse = f.ScriptUse
                                .ScriptData = f.ScriptData
                                If Not f.MyExchangeOptions Is Nothing Then DirectCast(.Self, UserDataBase).ExchangeOptionsSet(f.MyExchangeOptions)
                                If Not .HOST.Key = PathPlugin.PluginKey Then
                                    Settings.Labels.Add(LabelsKeeper.NoParsedUser)
                                    f.UserLabels.ListAddValue(LabelsKeeper.NoParsedUser)
                                End If
                                .Self.Labels.ListAddList(f.UserLabels, LAP.ClearBeforeAdd, LAP.NotContainsOnly)
                                .UpdateUserInformation()
                            End If
                        End With
                        UserListUpdate(Settings.Users.Last, True)
                        FocusUser(Settings.Users(Settings.Users.Count - 1).Key)
                    Else
                        MsgBoxE($"User [{f.User.Name}] was not added")
                    End If
                Else
                    FocusUser(Settings.Users(i).Key)
                    MsgBoxE($"User [{f.User.Name}] already exists", MsgBoxStyle.Exclamation)
                End If
            End If
            f.Dispose()
        End If
    End Sub
    Private Sub BTT_EDIT_USER_Click(sender As Object, e As EventArgs) Handles BTT_EDIT_USER.Click
        EditSelectedUser()
    End Sub
    Private Sub BTT_DELETE_USER_Click(sender As Object, e As EventArgs) Handles BTT_DELETE_USER.Click
        DeleteSelectedUser()
    End Sub
    Private Sub BTT_REFRESH_Click(sender As Object, e As EventArgs) Handles BTT_REFRESH.Click
        RefillList()
    End Sub
#End Region
#Region "Info, Feed, Channels, Saved posts"
#Region "Info"
    Private Sub MENU_INFO_SHOW_INFO_Click(sender As Object, e As EventArgs) Handles MENU_INFO_SHOW_INFO.Click
        InfoForm.FormShow(EDP.LogMessageValue)
    End Sub
    Private Sub MENU_INFO_SHOW_QUEUE_Click(sender As Object, e As EventArgs) Handles MENU_INFO_SHOW_QUEUE.Click
        Try : DownloadQueue.FormShow(EDP.LogMessageValue) : Catch : End Try
    End Sub
    Private Sub MENU_INFO_SHOW_MISSING_Click(sender As Object, e As EventArgs) Handles MENU_INFO_SHOW_MISSING.Click
        MyMissingPosts.FormShow(EDP.LogMessageValue)
    End Sub
    Private Sub MENU_INFO_SHOW_USER_METRICS_Click(sender As Object, e As EventArgs) Handles MENU_INFO_SHOW_USER_METRICS.Click
        MyUserMetrics.FormShow(EDP.LogMessageValue)
    End Sub
    Private Sub MENU_INFO_USER_SEARCH_Click(sender As Object, e As EventArgs) Handles MENU_INFO_USER_SEARCH.Click
        MySearch.FormShow()
    End Sub
#End Region
    Friend Sub ShowFeed() Handles BTT_FEED.Click, BTT_TRAY_FEED_SHOW.Click
        If MyFeed Is Nothing Then
            MyFeed = New DownloadFeedForm
            AddHandler Downloader.FeedFilesChanged, AddressOf MyFeed.Downloader_FilesChanged
            AddHandler MyFeed.UsersAdded, AddressOf OnUsersAddedHandler
            If Not MySavedPosts Is Nothing Then AddHandler MySavedPosts.FeedFilesChanged, AddressOf MyFeed.Downloader_FilesChanged
        End If
        If MyFeed.Visible Then MyFeed.BringToFront() Else MyFeed.Show()
    End Sub
    Private Sub BTT_CHANNELS_Click(sender As Object, e As EventArgs) Handles BTT_CHANNELS.Click, BTT_TRAY_CHANNELS.Click
        If MyChannels Is Nothing Then
            MyChannels = New ChannelViewForm
            AddHandler MyChannels.OnUsersAdded, AddressOf OnUsersAddedHandler
            AddHandler MyChannels.OnDownloadDone, AddressOf MainFrameObj.ShowNotification
        End If
        If MyChannels.Visible Then MyChannels.BringToFront() Else MyChannels.Show()
    End Sub
    Private Sub BTT_DOWN_SAVED_Click(sender As Object, e As EventArgs) Handles BTT_DOWN_SAVED.Click
        If MySavedPosts Is Nothing Then
            MySavedPosts = New DownloadSavedPostsForm
            AddHandler MySavedPosts.DownloadDone, AddressOf MainFrameObj.ShowNotification
            If Not MyFeed Is Nothing Then AddHandler MySavedPosts.FeedFilesChanged, AddressOf MyFeed.Downloader_FilesChanged
        End If
        With MySavedPosts
            If .Visible Then .BringToFront() Else .Show()
        End With
    End Sub
#End Region
#Region "Download"
    Private Sub BTT_DOWN_SELECTED_KeyClick(ByVal Sender As Object, ByVal e As MyKeyEventArgs) Handles BTT_DOWN_SELECTED.KeyClick
        DownloadSelectedUser(DownUserLimits.None, e.IncludeInTheFeed)
    End Sub
    Private Sub BTT_DOWN_ALL_KeyClick(ByVal Sender As Object, ByVal e As MyKeyEventArgs) Handles BTT_DOWN_ALL.KeyClick
        Dim ask As Boolean = False
        With Settings
            If e.KeyCode = Keys.F6 Then
                If .DownloadAll_UseF6 Then
                    ask = .DownloadAll_UseF6_Confirm
                Else
                    Exit Sub
                End If
            Else
                ask = .DownloadAll_Confirm
            End If
        End With
        If ask AndAlso MsgBoxE({"Are you sure you want to download all users?", "Download ALL"}, vbExclamation,,, {"Process", "Cancel"}) = 1 Then Exit Sub
        Using group As New DownloadGroup(False) With {.DownloadSubscriptions = e.IncludeInTheFeed} : group.ProcessDownloadUsers(e.IncludeInTheFeed, False) : End Using
    End Sub
    Private Sub BTT_DOWN_ALL_FULL_KeyClick(ByVal Sender As Object, ByVal e As MyKeyEventArgs) Handles BTT_DOWN_ALL_FULL.KeyClick
        Using group As New DownloadGroup(False) With {.DownloadSubscriptions = e.IncludeInTheFeed, .ReadyForDownloadIgnore = True} : group.ProcessDownloadUsers(e.IncludeInTheFeed, False) : End Using
    End Sub
    Private Sub BTT_DOWN_SPEC_KeyClick(ByVal Sender As Object, ByVal e As MyKeyEventArgs) Handles BTT_DOWN_SPEC.KeyClick
        Dim group As DownloadGroup = Nothing
        Using f As New GroupEditorForm(Nothing) With {.DownloadMode = True}
            f.ShowDialog()
            If f.DialogResult = DialogResult.OK AndAlso Not f.MyGroup Is Nothing Then group = f.MyGroup
        End Using
        If Not group Is Nothing Then group.ProcessDownloadUsers(e.IncludeInTheFeed, False) : group.Dispose()
    End Sub
#Region "Download groups"
    Private Sub GROUPS_AddRemoveAllGroupsButton(Optional ByVal DValue As Integer = 0)
        Try
            If Settings.Groups.LongCount(Function(g) Not g.IsViewFilter) - DValue > 0 Then
                If Not BTT_SHOW_ALL_GROUPS_ADDED Then
                    Dim i% = GetGroupIndex()
                    If i >= 0 Then
                        BTT_SHOW_ALL_GROUPS_ADDED = True
                        ControlInvoke(Toolbar_TOP, MENU_DOWN_ALL, Sub() MENU_DOWN_ALL.DropDownItems.Insert(i, BTT_SHOW_ALL_GROUPS))
                    End If
                End If
            ElseIf BTT_SHOW_ALL_GROUPS_ADDED Then
                ControlInvoke(Toolbar_TOP, MENU_DOWN_ALL, Sub() MENU_DOWN_ALL.DropDownItems.Remove(BTT_SHOW_ALL_GROUPS))
                BTT_SHOW_ALL_GROUPS_ADDED = False
            End If
        Catch
        End Try
    End Sub
    Private Function GetGroupIndex() As Integer
        If BTT_SHOW_ALL_GROUPS_ADDED Then
            Return MENU_DOWN_ALL.DropDownItems.IndexOf(BTT_SHOW_ALL_GROUPS)
        Else
            Return MENU_DOWN_ALL.DropDownItems.IndexOf(BTT_ADD_NEW_GROUP)
        End If
    End Function
    Private Sub BTT_ADD_NEW_GROUP_Click(sender As Object, e As EventArgs) Handles BTT_ADD_NEW_GROUP.Click
        Settings.Groups.Add()
    End Sub
    Private Sub GROUPS_Added(ByVal Sender As DownloadGroup)
        If Not Sender.IsViewFilter Then
            Dim i%
            If Sender.Index > 8 Then
                If MENU_DOWN_ALL.DropDownItems.IndexOf(BTT_GROUPS_OTHER) = -1 Then
                    i = GetGroupIndex()
                    If i >= 0 Then MENU_DOWN_ALL.DropDownItems.Insert(i, BTT_GROUPS_OTHER)
                End If
                ControlInvoke(Toolbar_TOP, BTT_GROUPS_OTHER, Sub() BTT_GROUPS_OTHER.DropDownItems.Add(Sender.GetControl))
            Else
                i = GetGroupIndex()
                ControlInvoke(Toolbar_TOP, MENU_DOWN_ALL, Sub() MENU_DOWN_ALL.DropDownItems.Insert(i, Sender.GetControl))
            End If
            GROUPS_AddRemoveAllGroupsButton()
        End If
    End Sub
    Private Sub GROUPS_Updated(ByVal Sender As DownloadGroup)
        If Not Sender.IsViewFilter Then
            Dim i% = BTT_GROUPS_OTHER.DropDownItems.IndexOf(Sender.GetControl)
            If i >= 0 Then
                ControlInvoke(Toolbar_TOP, BTT_GROUPS_OTHER, Sub() BTT_GROUPS_OTHER.DropDownItems(i).Text = Sender.ToString)
            Else
                i = MENU_DOWN_ALL.DropDownItems.IndexOf(Sender.GetControl)
                If i >= 0 Then ControlInvoke(Toolbar_TOP, MENU_DOWN_ALL, Sub() MENU_DOWN_ALL.DropDownItems(i).Text = Sender.ToString)
            End If
        End If
    End Sub
    Private Sub GROUPS_Deleted(ByVal Sender As DownloadGroup)
        If Not Sender.IsViewFilter Then
            MENU_DOWN_ALL.DropDownItems.Remove(Sender.GetControl)
            BTT_GROUPS_OTHER.DropDownItems.Remove(Sender.GetControl)
            If BTT_GROUPS_OTHER.DropDownItems.Count = 0 Then MENU_DOWN_ALL.DropDownItems.Remove(BTT_GROUPS_OTHER)
            GROUPS_AddRemoveAllGroupsButton(1)
        End If
    End Sub
    Private Sub BTT_SHOW_ALL_GROUPS_Click(sender As Object, e As EventArgs) Handles BTT_SHOW_ALL_GROUPS.Click
        Try
            Using f As New GroupListForm(False)
                f.ShowDialog()
                Dim g As DownloadGroup
                With Settings.Groups
                    If .Count > 0 Then
                        If f.GroupsUpdated Then
                            For Each g In .Self
                                If Not g.IsViewFilter Then GROUPS_Deleted(g)
                            Next
                            For Each g In .Self
                                If Not g.IsViewFilter Then GROUPS_Added(g)
                            Next
                        End If
                        If Not f.GroupToDownload.IsEmptyString Then
                            Dim i% = .IndexOf(f.GroupToDownload)
                            If i >= 0 Then .Item(i).ProcessDownloadUsers(f.GroupToDownloadIncludeInTheFeed)
                        End If
                    End If
                End With
            End Using
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.SendToLog, ex, "[MainFrame.ShowGroups]")
        End Try
    End Sub
#End Region
    Private Sub BTT_SILENT_MODE_Click(sender As Object, e As EventArgs) Handles BTT_SILENT_MODE.Click, BTT_TRAY_SILENT_MODE.Click
        With Settings : .NotificationsSilentMode = Not .NotificationsSilentMode : End With
        UpdateSilentButtons()
    End Sub
    Private Sub UpdateSilentButtons()
        With Settings
            ControlInvokeFast(Toolbar_TOP, BTT_SILENT_MODE, Sub() BTT_SILENT_MODE.Checked = .NotificationsSilentMode)
            ControlInvokeFast(Me, Sub() BTT_TRAY_SILENT_MODE.Checked = .NotificationsSilentMode)
        End With
    End Sub
    Private Sub UpdatePauseButtonsVisibility()
        Dim b As Boolean = Settings.Automation.Count > 0
        ControlInvokeFast(Toolbar_TOP, BTT_DOWN_AUTOMATION_PAUSE, Sub() BTT_DOWN_AUTOMATION_PAUSE.Visible = b)
        ControlInvokeFast(Me, Sub() BTT_TRAY_PAUSE_AUTOMATION.Visible = b)
    End Sub
    Private Async Sub BTT_DOWN_AUTOMATION_Click(sender As Object, e As EventArgs) Handles BTT_DOWN_AUTOMATION.Click, BTT_TRAY_SCHEDULER.Click
        Try
            Using f As New SchedulerEditorForm : f.ShowDialog() : End Using
            Await Settings.Automation.Start(False)
            UpdatePauseButtonsVisibility()
            MainFrameObj.PauseButtons.UpdatePauseButtons()
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Start automation")
        End Try
    End Sub
    Private Sub BTT_DOWN_AUTOMATION_PAUSE_Click(sender As Object, e As EventArgs) Handles BTT_DOWN_AUTOMATION_PAUSE.Click, BTT_TRAY_PAUSE_AUTOMATION.Click
        Dim p As PauseModes = Settings.Automation.Pause
        If p = PauseModes.Disabled Then p = PauseModes.Unlimited Else p = PauseModes.Disabled
        Settings.Automation.Pause = p
        MENU_DOWN_ALL.HideDropDown()
        TrayIcon.ContextMenuStrip.Hide()
        MainFrameObj.PauseButtons.UpdatePauseButtons()
    End Sub
    Private Sub BTT_DOWN_VIDEO_Click(sender As Object, e As EventArgs) Handles BTT_DOWN_VIDEO.Click, BTT_TRAY_DOWNLOADER.Click
        VideoDownloader.FormShow()
    End Sub
    Private Sub BTT_DOWN_STOP_Click(sender As Object, e As EventArgs) Handles BTT_DOWN_STOP.Click
        Downloader.Stop()
    End Sub
#End Region
#Region "View"
#Region "1 - view mode list"
    Private Sub BTT_VIEW_LARGE_Click(sender As Object, e As EventArgs) Handles BTT_VIEW_LARGE.Click
        ApplyViewPattern(ViewModes.IconLarge)
    End Sub
    Private Sub BTT_VIEW_SMALL_Click(sender As Object, e As EventArgs) Handles BTT_VIEW_SMALL.Click
        ApplyViewPattern(ViewModes.IconSmall)
    End Sub
    Private Sub BTT_VIEW_LIST_Click(sender As Object, e As EventArgs) Handles BTT_VIEW_LIST.Click
        ApplyViewPattern(ViewModes.List)
    End Sub
    Private Sub BTT_VIEW_DETAILS_Click(sender As Object, e As EventArgs) Handles BTT_VIEW_DETAILS.Click
        ApplyViewPattern(ViewModes.Details)
    End Sub
    Private Sub ApplyViewPattern(ByVal v As ViewModes, Optional ByVal OnlyButtons As Boolean = False)
        LIST_PROFILES.View = v
        Dim b As Boolean = Not (Settings.ViewMode.Value = v)
        If Not OnlyButtons Then Settings.ViewMode.Value = v

        BTT_VIEW_LARGE.Checked = v = ViewModes.IconLarge
        BTT_VIEW_SMALL.Checked = v = ViewModes.IconSmall
        BTT_VIEW_LIST.Checked = v = ViewModes.List
        BTT_VIEW_DETAILS.Checked = v = ViewModes.Details

        If v = View.Details Then
            LIST_PROFILES.Columns(0).Width = -2
            LIST_PROFILES.FullRowSelect = True
            LIST_PROFILES.GridLines = True
        End If

        If b Then
            If Settings.ViewModeIsPicture Then
                With LIST_PROFILES : .LargeImageList.Images.Clear() : .SmallImageList.Images.Clear() : End With
            End If
            If Not OnlyButtons Then RefillList()
        End If
    End Sub
#End Region
#Region "2 - view filters"
    Private Sub BTT_SHOW_ALL_Click(sender As Object, e As EventArgs) Handles BTT_SHOW_ALL.Click
        SetShowButtonsCheckers(True)
    End Sub
    Private Sub BTT_SHOW_SHOW_GROUPS_Click(sender As Object, e As EventArgs) Handles BTT_SHOW_SHOW_GROUPS.Click
        Settings.ShowGroupsInsteadLabels.Value = Not Settings.ShowGroupsInsteadLabels.Value
        SetShowButtonsCheckers(Settings.ShowAllUsers,, True)
        RefillList()
    End Sub
    Private Sub SetShowButtonsCheckers(ByVal ShowAll As Boolean, Optional ByVal ForceRefill As Boolean = False, Optional ByVal OnlyButtons As Boolean = False)
        BTT_SHOW_ALL.Checked = ShowAll
        BTT_SHOW_SHOW_GROUPS.Checked = Settings.ShowGroupsInsteadLabels
        BTT_SHOW_FILTER_ADV.Checked = Not ShowAll
        If Not OnlyButtons Then
            With Settings
                If Not .ShowAllUsers = ShowAll Or ForceRefill Then
                    .ShowAllUsers.Value = ShowAll
                    RefillList()
                Else
                    .ShowAllUsers.Value = ShowAll
                End If
            End With
        End If
    End Sub
    Private Sub BTT_SHOW_GROUP_USERS_Click(sender As Object, e As EventArgs) Handles BTT_SHOW_GROUP_USERS.Click
        With Settings.GroupUsers
            .Value = Not .Value
            BTT_SHOW_GROUP_USERS.Checked = .Value
            LIST_PROFILES.ShowGroups = .Value
        End With
        RefillList()
    End Sub
    Private Sub BTT_SHOW_FILTER_ADV_Click(sender As Object, e As EventArgs) Handles BTT_SHOW_FILTER_ADV.Click
        Try
            Using g As New GroupEditorForm(Settings.AdvancedFilter) With {.FilterMode = True}
                g.ShowDialog()
                If g.DialogResult = DialogResult.OK Then
                    Settings.AdvancedFilter.UpdateFile()
                    SetShowButtonsCheckers(False, True)
                End If
            End Using
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.SendToLog, ex, "Changing advanced filter options")
        End Try
    End Sub
#End Region
#Region "3 - saved filters"
    Private Sub BTT_VIEW_FILTER_SAVE_Click(sender As Object, e As EventArgs) Handles BTT_VIEW_FILTER_SAVE.Click, BTT_VIEW_FILTER_SAVE_AS_GROUP.Click
        Dim fName$ = String.Empty
        Dim isFilter As Boolean = sender Is BTT_VIEW_FILTER_SAVE
        Dim __process As Boolean = False
        Do
            fName = InputBoxE($"Enter a new name for the {IIf(isFilter, "view", "group")}:", $"{IIf(isFilter, "Filter", "Group")} name", fName)
            If Not fName.IsEmptyString Then
                If Settings.Groups.IndexOf(fName, isFilter) >= 0 Then
                    Select Case MsgBoxE({$"The '{fName}' {IIf(isFilter, "filter", "group")} already exists!", $"Save {IIf(isFilter, "filter", "group")}"},
                                        vbExclamation,,, {"Try again", "Replace", "Cancel"}).Index
                        Case 1 : __process = True
                        Case 2 : Exit Sub
                    End Select
                Else
                    __process = True
                End If
            Else
                Exit Sub
            End If
        Loop While Not __process
        If __process Then
            Dim f As New DownloadGroup(Not isFilter)
            f.Copy(Settings.AdvancedFilter)
            f.IsViewFilter = isFilter
            f.FilterViewMode = Settings.ViewMode
            f.FilterGroupUsers = Settings.GroupUsers
            f.FilterShowGroupsInsteadLabels = Settings.ShowGroupsInsteadLabels
            f.FilterShowAllUsers = Settings.ShowAllUsers
            f.Name = fName
            Settings.Groups.Add(f, isFilter, True)
            MsgBoxE({$"The '{fName}' {IIf(isFilter, "filter", "group")} has been saved", $"Save {IIf(isFilter, "filter", "group")}"})
        End If
    End Sub
    Private Sub BTT_VIEW_FILTER_LOAD_Click(sender As Object, e As EventArgs) Handles BTT_VIEW_FILTER_LOAD.Click
        Const msgTitle$ = "Load filter"
        Try
            If Settings.Groups.Count + Settings.Automation.Count = 0 Then
                MsgBoxE({"There are no saved filters", "Load filter"}, vbExclamation)
            Else
                Using f As New GroupListForm(True)
                    f.ShowDialog()
                    If f.DialogResult = DialogResult.OK Then
                        Dim filter As GroupParameters = f.FilterSelected
                        If Not filter Is Nothing AndAlso TypeOf filter Is AutoDownloader Then
                            With DirectCast(filter, AutoDownloader)
                                If .Mode = AutoDownloader.Modes.Groups Then
                                    If .Groups.Count = 0 Then
                                        MsgBoxE({"The scheduler plan you select doesn't contain any group!", msgTitle}, vbCritical)
                                        Exit Sub
                                    ElseIf .Groups.Count > 1 Then
                                        MsgBoxE({"The scheduler plan you select contains more than one group." & vbCr &
                                                 "You need to choose a plan with one group or without groups!", msgTitle}, vbCritical)
                                        Exit Sub
                                    Else
                                        Dim i% = Settings.Groups.IndexOf(.Groups(0))
                                        If i >= 0 Then
                                            filter = Settings.Groups(i).Copy
                                        Else
                                            MsgBoxE({$"A group named '{ .Groups(0)}' cannot be found in existing groups.", msgTitle}, vbCritical)
                                            filter = Nothing
                                            Exit Sub
                                        End If
                                    End If
                                End If
                            End With
                        End If
                        If Not filter Is Nothing Then
                            If filter.IsViewFilter Then
                                With DirectCast(filter, DownloadGroup)
                                    Settings.ViewMode.Value = .FilterViewMode
                                    Settings.GroupUsers.Value = .FilterGroupUsers
                                    Settings.ShowGroupsInsteadLabels.Value = .FilterShowGroupsInsteadLabels
                                    Settings.ShowAllUsers.Value = .FilterShowAllUsers
                                End With
                                ApplyViewPattern(Settings.ViewMode.Value, True)
                            Else
                                Settings.ShowAllUsers.Value = False
                            End If
                            Settings.AdvancedFilter.Copy(filter)
                            Settings.AdvancedFilter.UpdateFile()
                            SetShowButtonsCheckers(Settings.ShowAllUsers,, True)
                            UpdateUserGroupControls()
                            RefillList()
                        End If
                    End If
                End Using
            End If
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, msgTitle)
        End Try
    End Sub
    Private Sub UpdateUserGroupControls()
        With Settings
            BTT_SHOW_GROUP_USERS.Checked = Settings.GroupUsers
            LIST_PROFILES.ShowGroups = Settings.GroupUsers
        End With
    End Sub
#End Region
#End Region
    Private Sub BTT_LOG_Click(sender As Object, e As EventArgs) Handles BTT_LOG.Click
        MainFrameObj.ShowLog()
    End Sub
    Private Sub BTT_VERSION_INFO_Click(sender As Object, e As EventArgs) Handles BTT_VERSION_INFO.Click
        CheckVersion(True)
    End Sub
    Private Sub BTT_DONATE_Click(sender As Object, e As EventArgs) Handles BTT_DONATE.Click
        Try : Process.Start("https://github.com/AAndyProgram/SCrawler/blob/main/HowToSupport.md") : Catch : End Try
    End Sub
    Private Sub BTT_BUG_REPORT_Click(sender As Object, e As EventArgs) Handles BTT_BUG_REPORT.Click
        Try
            With Settings
                Using f As New BugReporterForm(.Cache, .Design, .ProgramText, My.Application.Info.Version,
                                               False, .Self, .ProgramDescription) : f.ShowDialog() : End Using
            End With
        Catch
        End Try
    End Sub
#End Region
#Region "List handlers"
    Private _LatestSelected As Integer = -1
    Private Sub LIST_PROFILES_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LIST_PROFILES.SelectedIndexChanged
        Dim a As Action = Sub()
                              If LIST_PROFILES.SelectedIndices.Count > 0 Then
                                  _LatestSelected = LIST_PROFILES.SelectedIndices(0)
                              Else
                                  _LatestSelected = -1
                              End If
                          End Sub
        If LIST_PROFILES.InvokeRequired Then LIST_PROFILES.Invoke(a) Else a.Invoke
    End Sub
    Private Sub LIST_PROFILES_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles LIST_PROFILES.MouseDoubleClick
        OpenFolder()
    End Sub
#End Region
#Region "Context"
#Region "1 - download"
    Private Sub BTT_CONTEXT_DOWN_KeyClick(sender As Object, e As MyKeyEventArgs) Handles BTT_CONTEXT_DOWN.KeyClick
        DownloadSelectedUser(DownUserLimits.None, e.IncludeInTheFeed)
    End Sub
    Private Sub BTT_CONTEXT_DOWN_LIMITED_KeyClick(sender As Object, e As MyKeyEventArgs) Handles BTT_CONTEXT_DOWN_LIMITED.KeyClick
        DownloadSelectedUser(DownUserLimits.Number, e.IncludeInTheFeed)
    End Sub
    Private Sub BTT_CONTEXT_DOWN_DATE_LIMIT_KeyClick(sender As Object, e As MyKeyEventArgs) Handles BTT_CONTEXT_DOWN_DATE_LIMIT.KeyClick
        DownloadSelectedUser(DownUserLimits.Date, e.IncludeInTheFeed)
    End Sub
#End Region
#Region "1 - edit, delete, copy"
    Private Sub BTT_CONTEXT_EDIT_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_EDIT.Click
        EditSelectedUser()
    End Sub
    Private Sub BTT_CONTEXT_DELETE_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_DELETE.Click
        DeleteSelectedUser()
    End Sub
    Private Sub BTT_CONTEXT_ERASE_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_ERASE.Click
        Const msgTitle$ = "Erase data"
        Try
            Dim users As List(Of IUserData) = GetSelectedUserArray()
            If users.ListExists Then
                Dim m As IUserData.EraseMode = UserDataBase.GetEraseMode(users)
                If Not m = IUserData.EraseMode.None Then
                    Dim nd As New List(Of IUserData)
                    For Each user As IUserData In users
                        If Not user.EraseData(m) Then nd.Add(user)
                    Next
                    If nd.Count = 0 Then
                        MsgBoxE({"All user data has been erased.", msgTitle})
                    Else
                        MsgBoxE(New MMessage("The data of the following users has not been erased:" &
                                             vbCr.StringDup(2) & nd.ListToStringE(vbCr, GetUserListProvider(True)), msgTitle,,
                                             MsgBoxStyle.Exclamation) With {.Editable = True})
                    End If
                End If
            Else
                MsgBoxE({"No user selected", msgTitle}, vbExclamation)
            End If
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, msgTitle)
        End Try
    End Sub
    Private Sub BTT_CONTEXT_COPY_TO_FOLDER_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_COPY_TO_FOLDER.Click
        CopyUserData()
    End Sub
#End Region
#Region "2 - user parameters"
    Private Sub BTT_CONTEXT_FAV_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_FAV.Click
        Dim users As List(Of IUserData) = GetSelectedUserArray()
        If AskForMassReplace(users, "Favorite") Then users.ForEach(Sub(ByVal u As IUserData)
                                                                       u.Favorite = Not u.Favorite
                                                                       u.UpdateUserInformation()
                                                                       UserListUpdate(u, False)
                                                                   End Sub)
    End Sub
    Private Sub BTT_CONTEXT_TEMP_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_TEMP.Click
        Dim users As List(Of IUserData) = GetSelectedUserArray()
        If AskForMassReplace(users, "Temporary") Then users.ForEach(Sub(ByVal u As IUserData)
                                                                        u.Temporary = Not u.Temporary
                                                                        u.UpdateUserInformation()
                                                                        UserListUpdate(u, False)
                                                                    End Sub)
    End Sub
    Private Sub BTT_CONTEXT_READY_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_READY.Click
        Dim users As List(Of IUserData) = GetSelectedUserArray()
        If AskForMassReplace(users, "Ready for download") Then
            Dim r As Boolean = MsgBoxE({"What state do you want to set for selected users", "Select ready state"}, vbQuestion,,, {"Not Ready", "Ready"}).Index
            users.ForEach(Sub(ByVal u As IUserData)
                              u.ReadyForDownload = r
                              u.UpdateUserInformation()
                          End Sub)
        End If
    End Sub
    Private Sub BTT_CONTEXT_GROUPS_Click(ByVal Sender As Object, ByVal e As Controls.KeyClick.KeyClickEventArgs) Handles BTT_CONTEXT_GROUPS.KeyClick
        Const MsgTitle$ = "Label change"
        Try
            Dim users As List(Of IUserData) = GetSelectedUserArray()
            If users.ListExists Then
                Dim l As List(Of String) = ListAddList(Nothing, users.SelectMany(Function(u) u.Labels), LAP.NotContainsOnly)
                Dim lex As List(Of String) = ListAddList(Nothing, users.SelectMany(Function(u As UserDataBase) u.SpecialLabels), LNC)
                Dim initialCount% = l.Count
                Dim isOneUser As Boolean = users.Count = 1 AndAlso Not users(0).IsCollection
                Dim inclSpec As Boolean = (e.Control And (users.Count > 1 Or (users.Count = 1 And users(0).IsCollection))) Or isOneUser
                If Not inclSpec Then l.ListWithRemove(lex)
                Using f As New LabelsForm(l) With {.WithDeleteButton = l.Count > 0}
                    f.ShowDialog()
                    If f.DialogResult = DialogResult.OK Then
                        Dim labels As List(Of String) = f.LabelsList
                        Dim cMsg As New MMessage("Operation canceled", MsgTitle)
                        Dim upMode As Byte
                        Dim keepSpecial As Boolean = True
                        If labels.ListExists Then
                            Select Case MsgBoxE(New MMessage($"What do you want to do with the selected labels?{vbCr}Selected labels:{vbCr}{labels.ListToString(vbCr)}",
                                                             MsgTitle,
                                                             {
                                                                New MsgBoxButton("Replace", "All existing labels will be removed and replaced with these labels"),
                                                                New MsgBoxButton("Add", "These labels will be added to the existing ones"),
                                                                New MsgBoxButton("Remove", "These labels will be removed from the existing ones"),
                                                                "Cancel"
                                                             }, vbExclamation) With {.ButtonsPerRow = 2}).Index
                                Case 0 : upMode = 1
                                Case 1 : upMode = 0
                                Case 2 : upMode = 2
                                Case Else : cMsg.Show() : Exit Sub
                            End Select
                        Else
                            If MsgBoxE({"Are you sure you want to remove all labels?", MsgTitle}, vbExclamation + vbYesNo) = vbYes Then
                                upMode = 1
                            Else
                                cMsg.Show()
                                Exit Sub
                            End If
                        End If

                        If lex.ListExists AndAlso Not isOneUser AndAlso (l.ListContains(lex) Or (Not inclSpec And Not l.Count = initialCount)) Then _
                           keepSpecial = UserDataBase.UpdateLabelsKeepSpecial(upMode)

                        users.ForEach(Sub(ByVal u As IUserData)
                                          UserDataBase.UpdateLabels(u, labels, upMode, keepSpecial)
                                          u.UpdateUserInformation()
                                      End Sub)
                    End If
                End Using
            Else
                MsgBoxE("No user found", vbExclamation)
            End If
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.ShowAllMsg, ex, "[ChangeUserGroups]")
        End Try
    End Sub
    Private Sub BTT_CONTEXT_SCRIPT_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_SCRIPT.Click
        Try
            Dim users As List(Of IUserData) = GetSelectedUserArray()
            If users.ListExists Then
                Dim ans% = MsgBoxE({"You want to change the script usage for selected users." & vbCr &
                                    "Which script usage mode do you want to set?",
                                    "Change script usage"}, vbExclamation,,, {"Use", "Do not use", "Cancel"})
                If ans < 2 Then
                    Dim s As Boolean = IIf(ans = 0, True, False)
                    users.ForEach(Sub(ByVal u As IUserData)
                                      Dim b As Boolean = u.ScriptUse = s
                                      u.ScriptUse = s
                                      If Not b Then u.UpdateUserInformation()
                                  End Sub)
                    MsgBoxE($"Script mode was set to [{IIf(s, "Use", "Do not use")}] for all selected users")
                Else
                    MsgBoxE("Operation canceled")
                End If
            Else
                MsgBoxE("Users not selected", vbExclamation)
            End If
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Change script usage")
        End Try
    End Sub
    Private Function AskForMassReplace(ByVal users As List(Of IUserData), ByVal param As String) As Boolean
        Dim u$ = users.ListIfNothing.Take(20).Select(Function(uu) uu.Name).ListToString(vbCr)
        If Not u.IsEmptyString And users.ListExists(21) Then u &= vbCr & "..."
        Return users.ListExists AndAlso (users.Count = 1 OrElse MsgBoxE({$"Do you really want to change [{param}] for {users.Count} users?{vbCr}{vbCr}{u}",
                                                                         "Users' parameters change"},
                                                                        MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo) = MsgBoxResult.Yes)
    End Function
    Private Sub BTT_CONTEXT_ADD_TO_COL_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_ADD_TO_COL.Click
        Const MsgTitle$ = "Add users to the collection"
        If Settings.CollectionsPath.Value.IsEmptyString Then
            MsgBoxE({"Collection path not specified", MsgTitle}, MsgBoxStyle.Exclamation)
        Else
            Dim users As List(Of IUserData) = GetSelectedUserArray()
            If users.ListExists Then
                Dim i%
                Dim _col_user As Predicate(Of IUserData) = Function(u) u.IsCollection
                Dim userCollection As UserDataBind = users.Find(_col_user)
                Dim _col_name$ = String.Empty
                Dim _col_dest As SFile = Nothing
                Dim allUsersIsSubscriptions As Boolean
                Dim userProvider As IFormatProvider = GetUserListProvider(False)
                If Not userCollection Is Nothing Then
                    i = users.LongCount(Function(u) _col_user(u))
                    If i > 1 OrElse i = users.Count OrElse
                       (i = 1 AndAlso
                        MsgBoxE({$"Do you want to add the following users to the [{userCollection.Name}] collection?" & vbCr &
                                 users.Where(Function(u) Not _col_user(u)).ListToStringE(vbCr, userProvider),
                                 MsgTitle}, vbQuestion + vbYesNo) = vbNo) Then _
                        MsgBoxE({"The collection cannot be added to the collection!", MsgTitle}, MsgBoxStyle.Critical) : Exit Sub
                    _col_name = userCollection.Name
                End If
                If _col_name.IsEmptyString Then
                    Using f As New CollectionEditorForm
                        f.ShowDialog()
                        If f.DialogResult = DialogResult.OK Then
                            _col_name = f.MyCollection
                            _col_dest = f.MyCollectionSpecialPath
                        End If
                    End Using
                End If
                If _col_name.IsEmptyString Then
                    MsgBoxE({"The destination collection has not been selected.", MsgTitle}, vbExclamation)
                Else
                    With (From u In users Where Not u.IsCollection Select u.IsSubscription)
                        allUsersIsSubscriptions = .ListExists AndAlso .All(Function(u) u)
                    End With
                    With Settings
                        userCollection = .Users.Find(Function(u) u.IsCollection And u.CollectionName = _col_name)
                        Dim Added As Boolean = userCollection Is Nothing
                        If Added Then
                            .Users.Add(New UserDataBind(_col_name, _col_dest))
                            MainFrameObj.CollectionHandler(DirectCast(.Users.Last, UserDataBind))
                            userCollection = .Users.Last
                        End If

                        Dim __modelUser As UsageModel = -1
                        Dim __modelCollection As UsageModel = -1
                        Dim __ModelAskForDecision As Boolean = False
                        If Not Added Then __modelCollection = userCollection.CollectionModel
                        If Added Then
                            If allUsersIsSubscriptions Then
                                __modelUser = UsageModel.Virtual
                                __modelCollection = UsageModel.Virtual
                            Else
                                __ModelAskForDecision = True
                            End If
                        ElseIf userCollection.CollectionModel = UsageModel.Virtual Then
                            __modelUser = UsageModel.Virtual
                            __modelCollection = UsageModel.Virtual
                        ElseIf allUsersIsSubscriptions Then
                            __modelCollection = userCollection.CollectionModel
                            __modelUser = UsageModel.Virtual
                        Else
                            __ModelAskForDecision = True
                        End If

                        If (users.Count = 1 AndAlso Not users(0).IsCollection AndAlso users(0).HOST.Key = PathPlugin.PluginKey) OrElse
                           (users.Count = 2 AndAlso users.All(Function(u) u.IsCollection OrElse u.HOST.Key = PathPlugin.PluginKey)) Then
                            __modelUser = UsageModel.Virtual
                            If Added Then
                                __modelCollection = UsageModel.Virtual
                            Else
                                i = users.FindIndex(_col_user)
                                If i >= 0 Then
                                    __modelCollection = users(i).CollectionModel
                                Else
                                    __modelCollection = UsageModel.Virtual
                                End If
                            End If
                            __ModelAskForDecision = False
                        End If

                        If __ModelAskForDecision Then
                            Select Case MsgBoxE({"How do you want to add users to the collection?", MsgTitle}, vbQuestion,,,
                                                {
                                                    New MsgBoxButton("Default", "User files will be moved to the collection") With {.KeyCode = Keys.Enter},
                                                    New MsgBoxButton("Virtual", "The user will be included in the collection, but user files will not be moved") With {
                                                                     .KeyCode = New ButtonKey(Keys.Enter, True)}
                                                }).Index
                                Case 0
                                    __modelUser = UsageModel.Default
                                    If __modelCollection = -1 Then __modelCollection = UsageModel.Default
                                Case 1
                                    __modelUser = UsageModel.Virtual
                                    If __modelCollection = -1 Then __modelCollection = UsageModel.Virtual
                            End Select
                        End If
                        If __modelUser = -1 Or __modelCollection = -1 Then
                            MsgBoxE({$"Some parameters cannot be processed:{vbCr}" &
                                     $"UserModel: {CInt(__modelUser)}{vbCr}CollectionModel: {CInt(__modelCollection)}{vbCr}" &
                                     "Operation canceled", MsgTitle}, vbCritical)
                            Exit Sub
                        End If

                        Dim __added_users As New List(Of IUserData)
                        Dim __added_users_not As New List(Of IUserData)
                        For Each user As UserDataBase In users
                            If Not user.IsCollection Then
                                Try
                                    If user.IsSubscription Then
                                        user.User.UserModel = UsageModel.Virtual
                                    Else
                                        user.User.UserModel = IIf(user.HOST.Key = PathPlugin.PluginKey, UsageModel.Virtual, __modelUser)
                                    End If
                                    user.User.CollectionModel = __modelCollection
                                    userCollection.Add(user)
                                    RemoveUserFromList(user)
                                    UserListUpdate(userCollection, Added)
                                    If Not Added Then FocusUser(userCollection.LVIKey)
                                    Added = False
                                    __added_users.Add(user)
                                Catch ex As InvalidOperationException
                                    userCollection.Remove(user)
                                    If __added_users.Count > 0 AndAlso __added_users.Contains(user) Then __added_users.Remove(user)
                                    __added_users_not.Add(user)
                                End Try
                            End If
                        Next
                        If userCollection.Count = 0 Then
                            RemoveUserFromList(userCollection)
                            If Settings.Users.Remove(userCollection) Then userCollection.Dispose()
                            MsgBoxE({$"No users have been added to the [{_col_name}] collection.", MsgTitle}, vbCritical)
                        ElseIf __added_users.Count = 1 And __added_users_not.Count = 0 Then
                            MsgBoxE({$"The user [{__added_users(0)}] has been added to the collection [{_col_name}].", MsgTitle})
                        ElseIf __added_users.Count = 0 And __added_users_not.Count = 1 Then
                            MsgBoxE({$"The user [{__added_users_not(0)}] was not added to the collection [{_col_name}].", MsgTitle}, vbCritical)
                        Else
                            Dim m As New MMessage($"The following users have been added to the [{_col_name}] collection:{vbCr}", MsgTitle,,
                                                  If(__added_users_not.Count > 0, vbExclamation, vbInformation))
                            m.Text &= __added_users.ListToStringE(vbCr, userProvider)
                            If __added_users_not.Count > 0 Then
                                m.Text &= $"{vbNewLine.StringDup(2)}The following users have not been added to the [{_col_name}] collection:{vbCr}"
                                m.Text &= __added_users_not.ListToStringE(vbCr, userProvider)
                            End If
                            MsgBoxE(m)
                        End If
                        __added_users.Clear()
                        __added_users_not.Clear()
                    End With
                End If
            End If
        End If
    End Sub
    Private Sub BTT_CONTEXT_COL_MERGE_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_COL_MERGE.Click
        Const MsgTitle$ = "Merging files"
        Dim user As IUserData = GetSelectedUser()
        If Not user Is Nothing Then
            If user.IsCollection Then
                If DirectCast(user, UserDataBind).DataMerging Then
                    MsgBoxE({"Collection files are already merged", MsgTitle})
                ElseIf user.IsVirtual Then
                    MsgBoxE({"The action cannot be performed. This is a virtual collection.", MsgTitle}, vbCritical)
                Else
                    If MsgBoxE({"Are you sure you want to merge the collection files into one folder?" & vbNewLine &
                                "This action is not turnable!", MsgTitle}, vbExclamation + vbYesNo) = vbYes Then
                        DirectCast(user, UserDataBind).DataMerging = True
                    End If
                End If
            Else
                MsgBoxE("This is not collection!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub
    Private Sub BTT_CONTEXT_CHANGE_FOLDER_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_CHANGE_FOLDER.Click
        ChangeUserDestination(GetSelectedUserArray(), True)
    End Sub
    Private Function ChangeUserDestination(ByVal users As IEnumerable(Of IUserData), ByVal InitialInvoke As Boolean,
                                           Optional ByVal NewUsersLocation As STDownloader.DownloadLocation? = Nothing) As Boolean
        Const MsgTitle$ = "Change user folder"
        Dim automationPaused As Boolean = Not Settings.Automation.Pause = PauseModes.Disabled
        Try
            Dim msgShowing As New ErrorsDescriber(If(InitialInvoke, EDP.ShowMainMsg, EDP.None))
            If users.ListExists Then
                If Downloader.Working Then
                    MsgBoxE({"Some users are currently downloading." & vbCr &
                             "You cannot change paths while downloading." & vbCr &
                             "Wait until the download is complete.", MsgTitle}, vbCritical)
                    Return False
                Else
                    If InitialInvoke Then
                        Downloader.Suspended = True
                        If Not automationPaused Then Settings.Automation.Pause = PauseModes.Unlimited
                    End If
                End If

                Dim locationChooser As GlobalLocationsChooserForm
                Dim newLoc As STDownloader.DownloadLocation
                If users.Count > 1 Then
                    Dim multiUserMsgTxt$ = "You have selected multiple users to change their destinations." & vbCr &
                                           "It is highly recommended to change the destination for one user at a time."
                    If users.ListExists(Function(u) u.IsCollection And Not u.IsVirtual) Then _
                        multiUserMsgTxt &= vbCr & vbCr & "A collection was also found in your selection." & vbCr &
                                           "The collection movement model is always the only one, regardless of the path model you choose."
                    multiUserMsgTxt &= vbCr & vbCr & $"Selected users:{vbCr}{users.ListToStringE(vbCr, GetUserListProvider(True))}."

                    Select Case MsgBoxE({multiUserMsgTxt, MsgTitle}, vbExclamation,,,
                                        {New MsgBoxButton("Process", "Change the destination for all the users you selected"),
                                         New MsgBoxButton("First only", "Process only the first user in the selection"),
                                         "Cancel"}).Index
                        Case 0
                            locationChooser = New GlobalLocationsChooserForm With {
                                .MyIsMultipleUsers = True,
                                .MyNonMyltipleUser = If(users.FirstOrDefault(Function(u) Not u.IsCollection), users(0)),
                                .MyIsCollectionSelector = users.All(Function(u) u.IsCollection)
                            }
                            With locationChooser
                                .ShowDialog()
                                If .DialogResult = DialogResult.OK Then
                                    newLoc = .MyDestination
                                    .Dispose()
                                Else
                                    .Dispose()
                                    ShowOperationCanceledMsg(MsgTitle)
                                    Return False
                                End If
                            End With
                            With users.Where(Function(u) Not ChangeUserDestination({u}, False, newLoc))
                                If .ListExists Then
                                    If .Count = users.Count Then
                                        MsgBoxE({"None of the users' destinations have been changed!", MsgTitle}, vbCritical)
                                        Return False
                                    Else
                                        MsgBoxE({$"The following users' destinations have not been changed:{vbCr}" &
                                                 users.ListToStringE(vbCr, GetUserListProvider(True)), MsgTitle}, vbCritical)
                                        Return True
                                    End If
                                Else
                                    MsgBoxE({"Users' data has been moved", MsgTitle})
                                    Return True
                                End If
                            End With
                        Case 1 : users = New List(Of IUserData) From {users.First}
                        Case Else : ShowOperationCanceledMsg(MsgTitle) : Return False
                    End Select
                End If

                If users.Count = 1 Then
                    Dim CutOption% = 1
                    Dim _IsCollection As Boolean = False
                    Dim CurrDir As SFile
                    Dim colName$ = String.Empty
                    Dim pathHandler As PathMoverHandler
                    With users(0)
                        If .IsCollection Then
                            _IsCollection = True
                            With DirectCast(.Self, UserDataBind)
                                If .Count = 0 Then
                                    Throw New ArgumentOutOfRangeException("Collection", "Collection is empty")
                                ElseIf .IsVirtual Then
                                    MsgBoxE({"This is a virtual collection." & vbCr &
                                             "The virtual collection path cannot be changed." & vbCr &
                                             "To change the paths of users included in a virtual collection, " &
                                             "you must split the collection and then change the user paths.", MsgTitle}, vbCritical, msgShowing)
                                    Return False
                                Else
                                    CurrDir = .GetRealUserFile
                                    If CurrDir.IsEmptyString Then
                                        MsgBoxE({"Non-virtual users not found", MsgTitle}, vbCritical, msgShowing)
                                        Return False
                                    End If
                                    CurrDir = CurrDir.CutPath(IIf(.DataMerging, 3, 2))
                                    colName = .CollectionName
                                    Dim vu As IEnumerable(Of IUserData) = .Where(Function(vuu) vuu.UserModel = UsageModel.Virtual Or vuu.HOST.Key = PathPlugin.PluginKey)
                                    If vu.ListExists Then
                                        If InitialInvoke AndAlso MsgBoxE({"This collection contains virtual users and/or paths." & vbCr &
                                                                          "If you continue, the virtual user paths will not be changed." & vbCr &
                                                                          "The following users have been added to the collection in virtual mode:" & vbCr &
                                                                          vu.ListToStringE(vbCr, GetUserListProvider(False)), MsgTitle},
                                                                         vbExclamation,,, {"Continue", "Cancel"}) = 1 Then ShowOperationCanceledMsg(MsgTitle) : Return False
                                    End If
                                End If
                            End With
                        ElseIf .HOST.Key = PathPlugin.PluginKey Then
                            MsgBoxE({"This is the path (not user). The paths cannot be changed.", MsgTitle}, vbCritical, msgShowing)
                            Return False
                        Else
                            CurrDir = .Self.File.CutPath(1)
                        End If

                        If NewUsersLocation.HasValue Then
                            newLoc = NewUsersLocation.Value
                        Else
                            locationChooser = New GlobalLocationsChooserForm With {.MyInitialLocation = CurrDir}
                            locationChooser.MyNonMyltipleUser = .Self()
                            If _IsCollection Then
                                locationChooser.MyIsCollectionSelector = True
                                locationChooser.MyCollectionName = colName
                            End If
                            With locationChooser
                                .ShowDialog()
                                If .DialogResult = DialogResult.OK Then
                                    newLoc = .MyDestination
                                    colName = .MyCollectionName
                                    .Dispose()
                                Else
                                    .Dispose()
                                    If InitialInvoke Then ShowOperationCanceledMsg(MsgTitle)
                                    Return False
                                End If
                            End With
                        End If

                        If .IsCollection Then
                            pathHandler = GlobalLocationsChooserForm.ModelHandler(PathCreationModel.Collection)
                        Else
                            pathHandler = GlobalLocationsChooserForm.ModelHandler(newLoc.Model)
                        End If

                        Dim NewDest As SFile
                        If .IsCollection Then
                            If Not InitialInvoke Then
                                NewDest = $"{newLoc.Path.CSFilePS}{SettingsCLS.CollectionsFolderName}\{ .CollectionName}\"
                            Else
                                NewDest = $"{newLoc.Path.CSFilePS}{ .CollectionName}\"
                            End If
                        Else
                            NewDest = pathHandler.Invoke(DirectCast(.Self, UserDataBase).User, newLoc.Path.CSFileP)
                        End If
                        If Not NewDest.IsEmptyString Then
                            If Not NewDest.IsEmptyString AndAlso
                                   (Not NewDest.Exists(SFO.Path, False) OrElse
                                        (
                                            SFile.GetFiles(NewDest,, IO.SearchOption.AllDirectories, EDP.ThrowException).ListIfNothing.Count = 0 AndAlso
                                            NewDest.Delete(SFO.Path, Settings.DeleteMode, EDP.ThrowException) AndAlso
                                            Not NewDest.Exists(SFO.Path, False)
                                        )
                                   ) Then
                                If SFile.Move(CurrDir, NewDest, SFO.Path,,, EDP.ReturnValue + If(InitialInvoke, EDP.ShowMainMsg, 0)) Then
                                    Dim colRootDef As SFile = Settings.CollectionsPathF
                                    Dim __UserSpecialPathsEquals As Func(Of UserInfo, Boolean, Boolean) =
                                        Function(ByVal __user As UserInfo, ByVal __isCol As Boolean) As Boolean
                                            Dim u1 As UserInfo = __user
                                            Dim u2 As UserInfo = __user
                                            If __isCol Then
                                                u1.CollectionName = colName
                                                u1.SpecialPath = Nothing
                                                u1.SpecialCollectionPath = Nothing
                                                u2.CollectionName = colName
                                                u2.SpecialPath = Nothing
                                                u2.SpecialCollectionPath = NewDest
                                            Else
                                                u1.CollectionName = String.Empty
                                                u1.SpecialPath = Nothing
                                                u1.SpecialCollectionPath = Nothing
                                                u2.CollectionName = String.Empty
                                                u2.SpecialPath = NewDest
                                                u2.SpecialCollectionPath = Nothing
                                            End If
                                            u1.UpdateUserFile()
                                            u2.UpdateUserFile()
                                            Return u1.File = u2.File
                                        End Function
                                    Dim ApplyChanges As Action(Of IUserData) = Sub(ByVal __user As IUserData)
                                                                                   With DirectCast(__user, UserDataBase)
                                                                                       Dim u As UserInfo = .User
                                                                                       Settings.UsersList.Remove(u)
                                                                                       If _IsCollection Then
                                                                                           u.CollectionName = colName
                                                                                           If Not __UserSpecialPathsEquals(u, True) Then
                                                                                               u.SpecialCollectionPath = NewDest
                                                                                           Else
                                                                                               u.SpecialCollectionPath = Nothing
                                                                                           End If
                                                                                           u.SpecialPath = Nothing
                                                                                       Else
                                                                                           u.CollectionName = String.Empty
                                                                                           If Not __UserSpecialPathsEquals(u, False) Then
                                                                                               u.SpecialPath = NewDest
                                                                                           Else
                                                                                               u.SpecialPath = Nothing
                                                                                           End If
                                                                                           u.SpecialCollectionPath = Nothing
                                                                                       End If
                                                                                       u.UpdateUserFile()
                                                                                       Settings.UsersList.Add(u)
                                                                                       .User = u
                                                                                       .UpdateUserInformation()
                                                                                   End With
                                                                               End Sub
                                    If .IsCollection Then
                                        With DirectCast(.Self, UserDataBind)
                                            For Each user In .Collections : ApplyChanges(user) : Next
                                        End With
                                    Else
                                        ApplyChanges(.Self)
                                    End If
                                    Settings.UpdateUsersList()
                                    MsgBoxE({"User data has been moved", MsgTitle},, msgShowing)
                                    Return True
                                End If
                            Else
                                MsgBoxE({$"Unable to move user data to new destination [{NewDest}]{vbCr}Operation canceled", MsgTitle}, MsgBoxStyle.Critical, msgShowing)
                            End If
                        Else
                            MsgBoxE({$"You have not entered a new destination{vbCr}Operation canceled", MsgTitle}, MsgBoxStyle.Exclamation, msgShowing)
                        End If
                    End With
                Else
                    MsgBoxE({"You have selected multiple users. You can change the folder only for one user!", MsgTitle}, MsgBoxStyle.Critical, msgShowing)
                End If
            Else
                MsgBoxE({"No one user selected", MsgTitle}, MsgBoxStyle.Exclamation, msgShowing)
            End If
            Return False
        Catch ex As Exception
            Return ErrorsDescriber.Execute(EDP.ReturnValue + If(InitialInvoke, EDP.ShowAllMsg, EDP.SendToLog), ex, "Error while moving user", False)
        Finally
            If InitialInvoke Then
                Downloader.Suspended = False
                If Not automationPaused Then Settings.Automation.Pause = PauseModes.Disabled
            End If
        End Try
    End Function
#End Region
#Region "3 - change image"
    Private Sub BTT_CHANGE_IMAGE_Click(sender As Object, e As EventArgs) Handles BTT_CHANGE_IMAGE.Click
        Dim user As IUserData = GetSelectedUser()
        If Not user Is Nothing Then
            Dim f As SFile = SFile.SelectFiles(user.File.CutPath(IIf(user.IsCollection, 2, 1)), False, "Select new user picture",
                                               "Pictures|*.jpeg;*.jpg;*.png;*.webp|GIF|*.gif|All Files|*.*").FirstOrDefault
            If Not f.IsEmptyString Then
                user.SetPicture(f)
                UserListUpdate(user, False)
            End If
        End If
    End Sub
#End Region
#Region "4 - open folder"
    Private Sub BTT_CONTEXT_OPEN_PATH_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_OPEN_PATH.Click
        OpenFolder()
    End Sub
#End Region
#Region "5 - open site"
    Private Sub BTT_CONTEXT_OPEN_SITE_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_OPEN_SITE.Click
        Dim user As IUserData = GetSelectedUser()
        If Not user Is Nothing Then user.OpenSite()
    End Sub
#End Region
#Region "6 - information"
    Private Sub BTT_CONTEXT_INFO_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_INFO.Click
        Dim user As IUserData = GetSelectedUser()
        If Not user Is Nothing Then MsgBoxE(New MMessage(DirectCast(user, UserDataBase).GetUserInformation(), "User information") With {.Editable = True})
    End Sub
#End Region
    Private Sub USER_CONTEXT_VisibleChanged(sender As Object, e As EventArgs) Handles USER_CONTEXT.VisibleChanged
        Try
            If USER_CONTEXT.Visible Then
                Dim user As IUserData = GetSelectedUser()
                If Not user Is Nothing AndAlso user.IsCollection Then
                    With DirectCast(user, UserDataBind)
                        BTT_CONTEXT_DOWN.DropDownItems.AddRange(.ContextDown)
                        BTT_CONTEXT_DOWN_LIMITED.DropDownItems.AddRange(.ContextDownLimit)
                        BTT_CONTEXT_DOWN_DATE_LIMIT.DropDownItems.AddRange(.ContextDownDate)
                        BTT_CONTEXT_EDIT.DropDownItems.AddRange(.ContextEdit)
                        BTT_CONTEXT_DELETE.DropDownItems.AddRange(.ContextDelete)
                        BTT_CONTEXT_ERASE.DropDownItems.AddRange(.ContextErase)
                        BTT_CONTEXT_OPEN_PATH.DropDownItems.AddRange(.ContextPath)
                        BTT_CONTEXT_OPEN_SITE.DropDownItems.AddRange(.ContextSite)
                    End With
                End If
            Else
                BTT_CONTEXT_DOWN.DropDownItems.Clear()
                BTT_CONTEXT_DOWN_LIMITED.DropDownItems.Clear()
                BTT_CONTEXT_DOWN_DATE_LIMIT.DropDownItems.Clear()
                BTT_CONTEXT_EDIT.DropDownItems.Clear()
                BTT_CONTEXT_DELETE.DropDownItems.Clear()
                BTT_CONTEXT_ERASE.DropDownItems.Clear()
                BTT_CONTEXT_OPEN_PATH.DropDownItems.Clear()
                BTT_CONTEXT_OPEN_SITE.DropDownItems.Clear()
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region
#Region "Focus user"
    Private Overloads Sub FocusUser(ByVal Key As String)
        FocusUser(Key, True)
    End Sub
    Friend Overloads Sub FocusUser(ByVal Key As String, Optional ByVal ActivateMe As Boolean = False)
        If Not Key.IsEmptyString Then
            Dim a As Action = Sub()
                                  Dim i% = LIST_PROFILES.Items.IndexOfKey(Key)
                                  If i < 0 Then
                                      Dim u As IUserData = Settings.GetUser(Key, True)
                                      If Not u Is Nothing Then
                                          i = LIST_PROFILES.Items.IndexOfKey(u.Key)
                                          If i < 0 Then
                                              UserListUpdate(u, True)
                                              i = LIST_PROFILES.Items.IndexOfKey(u.Key)
                                          End If
                                      End If
                                  End If
                                  If i >= 0 Then
                                      LIST_PROFILES.Select()
                                      LIST_PROFILES.SelectedIndices.Clear()
                                      With LIST_PROFILES.Items(i) : .Selected = True : .Focused = True : End With
                                      LIST_PROFILES.EnsureVisible(i)
                                      If ActivateMe Then
                                          If Visible Then BringToFront() Else Visible = True
                                      End If
                                  End If
                              End Sub
            If LIST_PROFILES.InvokeRequired Then LIST_PROFILES.Invoke(a) Else a.Invoke
        End If
    End Sub
#End Region
#Region "Toolbar bottom"
    Private Sub BTT_PR_INFO_Click(sender As Object, e As EventArgs) Handles BTT_PR_INFO.Click
        If MyProgressForm.Visible Then MyProgressForm.BringToFront() Else MyProgressForm.Show()
    End Sub
#End Region
#Region "Operation providers"
    Private OperationsUserListProvider As IFormatProvider = Nothing
    Private OperationsUserListProviderCollections As IFormatProvider = Nothing
    Friend Function GetUserListProvider(ByVal WithCollections As Boolean) As IFormatProvider
        If WithCollections Then
            If OperationsUserListProviderCollections Is Nothing Then _
               OperationsUserListProviderCollections = New CustomProvider(Function(ByVal v As Object) As Object
                                                                              Dim OutStr$
                                                                              With DirectCast(v, IUserData)
                                                                                  If .IsCollection Then
                                                                                      OutStr = $"Collection [{ .Name}]"
                                                                                  Else
                                                                                      OutStr = $"User [{ .Site}] { .Name}"
                                                                                  End If
                                                                              End With
                                                                              Return OutStr
                                                                          End Function)
            Return OperationsUserListProviderCollections
        Else
            If OperationsUserListProvider Is Nothing Then _
               OperationsUserListProvider = New CustomProvider(Function(v) $"[{DirectCast(v, IUserData).Site}] {DirectCast(v, IUserData).Name}")
            Return OperationsUserListProvider
        End If
    End Function
#End Region
#Region "Operations with selected users: modify"
    Private Function GetSelectedUser() As IUserData
        If _LatestSelected.ValueBetween(0, LIST_PROFILES.Items.Count - 1) Then
            Dim k$ = LIST_PROFILES.Items(_LatestSelected).Name
            Dim i% = Settings.Users.FindIndex(Function(u) u.Key = k)
            If i >= 0 Then
                Return Settings.Users(i)
            Else
                MsgBoxE("User not found", MsgBoxStyle.Critical)
            End If
        End If
        Return Nothing
    End Function
    Private Function GetSelectedUserArray() As List(Of IUserData)
        Try
            With LIST_PROFILES
                If .SelectedIndices.Count > 0 Then
                    Dim l As New List(Of IUserData)
                    Dim k$
                    Dim indx%
                    For i% = 0 To .SelectedIndices.Count - 1
                        k = .Items(.SelectedIndices(i)).Name
                        indx = Settings.Users.FindIndex(Function(u) u.Key = k)
                        If i >= 0 Then l.Add(Settings.Users(indx))
                    Next
                    Return l
                End If
            End With
            Return New List(Of IUserData)
        Catch ex As Exception
            Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, "[MainFrame.GetSelectedUserArray]", New List(Of IUserData))
        End Try
    End Function
    Friend Enum DownUserLimits : None : Number : [Date] : End Enum
    Friend Sub DownloadSelectedUser(ByVal UseLimits As DownUserLimits, Optional ByVal IncludeInTheFeed As Boolean = True,
                                    Optional ByVal SUser As IUserData = Nothing)
        Const MsgTitle$ = "Download limit"
        Dim users As List(Of IUserData) = If(SUser Is Nothing, GetSelectedUserArray(), New List(Of IUserData) From {SUser})
        If users.ListExists Then
            Dim limit%? = Nothing
            Dim _from As Date? = Nothing
            Dim _to As Date? = Nothing
            Dim _fromStr$, _toStr$
            If UseLimits = DownUserLimits.Number Then
                Do
                    limit = AConvert(Of Integer)(InputBoxE("Enter top posts limit for downloading:", MsgTitle, 10), AModes.Var, Nothing)
                    If limit.HasValue Then
                        Select Case MsgBoxE(New MMessage($"You are set up downloading top [{limit.Value}] posts", MsgTitle,
                                            {"Confirm", "Try again", "Disable limit", "Cancel"}) With {.ButtonsPerRow = 2}).Index
                            Case 0 : Exit Do
                            Case 2 : limit = Nothing : Exit Do
                            Case 3 : GoTo CancelDownloadingOperation
                        End Select
                    Else
                        Select Case MsgBoxE({"You are not set up downloading limit", MsgTitle},,,, {"Confirm", "Try again", "Cancel"}).Index
                            Case 0 : Exit Do
                            Case 2 : GoTo CancelDownloadingOperation
                        End Select
                    End If
                Loop
            ElseIf UseLimits = DownUserLimits.Date Then
                Do
                    Using fd As New DateTimeSelectionForm(DateTimeSelectionForm.ModesAllDate, Settings.Design)
                        fd.ShowDialog()
                        If fd.DialogResult = DialogResult.OK Then
                            _from = fd.MyDateStart
                            _to = fd.MyDateEnd
                        ElseIf fd.DialogResult = DialogResult.Abort Then
                            _from = Nothing
                            _to = Nothing
                        End If
                    End Using
                    If _from.HasValue Or _to.HasValue Then
                        _fromStr = AConvert(Of String)(_from, ADateTime.Formats.BaseDate, String.Empty)
                        _toStr = AConvert(Of String)(_to, ADateTime.Formats.BaseDate, String.Empty)
                        If Not _fromStr.IsEmptyString Then _fromStr = $"FROM [{_fromStr}]"
                        If Not _toStr.IsEmptyString Then _toStr = $"TO [{_toStr}]"
                        If Not _toStr.IsEmptyString And Not _fromStr.IsEmptyString Then _fromStr &= " "
                        Select Case MsgBoxE(New MMessage($"You have set a date limit for downloading posts: {_fromStr}{_toStr}", MsgTitle,
                                                         {"Confirm", "Try again", "Disable limit", "Cancel"}) With {.ButtonsPerRow = 2}).Index
                            Case 0 : Exit Do
                            Case 2 : _from = Nothing : _to = Nothing : Exit Do
                            Case 3 : GoTo CancelDownloadingOperation
                        End Select
                    Else
                        Select Case MsgBoxE({"You have not set a date limit", MsgTitle},,,, {"Confirm", "Try again", "Cancel"}).Index
                            Case 0 : Exit Do
                            Case 2 : GoTo CancelDownloadingOperation
                        End Select
                    End If
                Loop
            End If
            If USER_CONTEXT.Visible Then USER_CONTEXT.Hide()
            GoTo ResumeDownloadingOperation
CancelDownloadingOperation:
            MsgBoxE("Operation canceled")
            Exit Sub
ResumeDownloadingOperation:
            Dim uStr$ = If(users.Count = 1, String.Empty, users.ListToStringE(vbNewLine, GetUserListProvider(True)))
            Dim fStr$ = $" ({IIf(IncludeInTheFeed, "included in", "excluded from")} the feed)"
            If users.Count = 1 OrElse MsgBoxE({$"You have selected {users.Count} user profiles" & vbCr &
                                               $"Do you want to download them all{fStr}?{vbNewLine.StringDup(2)}" &
                                               $"Selected users:{vbNewLine}{uStr}", "Multiple users selected"},
                                              MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                users.ForEach(Sub(ByVal u As IUserData)
                                  u.DownloadTopCount = limit
                                  u.DownloadDateFrom = _from
                                  u.DownloadDateTo = _to
                              End Sub)
                Downloader.AddRange(users, IncludeInTheFeed)
            End If
        End If
    End Sub
    Friend Sub EditSelectedUser(Optional ByVal CUser As IUserData = Nothing)
        Const MsgTitle$ = "User update"
        Dim user As IUserData = If(CUser, GetSelectedUser())
        If Not user Is Nothing Then
            On Error Resume Next
            If Not user.IsCollection OrElse DirectCast(user, UserDataBind).Count > 0 Then
                If user.IsCollection And USER_CONTEXT.Visible Then USER_CONTEXT.Hide()
                Using f As New UserCreatorForm(user)
                    f.ShowDialog()
                    If f.DialogResult = DialogResult.OK Then
                        Dim NeedToUpdate As Boolean = True
                        If user.IsCollection And Not user.CollectionName = f.CollectionName Then
                            If Not user.IsVirtual AndAlso Downloader.Working Then
                                MsgBoxE({"Some users are currently downloading." & vbCr &
                                         "You cannot change collection name while downloading." & vbCr &
                                         "Wait until the download is complete.", MsgTitle}, vbCritical)
                                Exit Sub
                            Else
                                If Not user.IsVirtual Then
                                    Dim rUser As UserDataBase = DirectCast(user, UserDataBind).GetRealUser
                                    If Not rUser Is Nothing Then
                                        Dim colPathCurr As SFile = rUser.User.GetCollectionRootPath
                                        Dim colPathNew As SFile = SFile.GetPath(colPathCurr.CutPath.PathWithSeparator & f.CollectionName)
                                        If Not colPathCurr.Exists(SFO.Path, False) Then
                                            MsgBoxE({"Original location of collection not found. Operation canceled.", MsgTitle}, vbCritical)
                                        ElseIf colPathNew.Exists(SFO.Path, False) Then
                                            MsgBoxE({"The new collection location already exists. Operation canceled.", MsgTitle}, vbCritical)
                                        Else
                                            If Not SFile.Rename(colPathCurr, colPathNew, SFO.Path, New ErrorsDescriber(True, False, False, New SFile)).IsEmptyString Then
                                                RemoveUserFromList(user)
                                                Dim __user As UserInfo
                                                For Each ColUser As UserDataBase In DirectCast(user, UserDataBind).Collections
                                                    __user = ColUser.User
                                                    Settings.UsersList.Remove(__user)
                                                    __user.CollectionName = f.CollectionName
                                                    If Not __user.SpecialCollectionPath.IsEmptyString Then __user.SpecialCollectionPath = colPathNew
                                                    __user.UpdateUserFile()
                                                    ColUser.User = __user
                                                    Settings.UsersList.Add(__user)
                                                Next
                                                user.UpdateUserInformation()
                                                UserListUpdate(user, True)
                                                NeedToUpdate = False
                                            End If
                                        End If
                                    End If
                                Else
                                    RemoveUserFromList(user)
                                    DirectCast(user, UserDataBind).ChangeVirtualCollectionName(f.CollectionName)
                                    UserListUpdate(user, True)
                                    NeedToUpdate = False
                                End If
                            End If
                        End If
                        If NeedToUpdate Then UserListUpdate(user, False)
                    End If
                End Using
            End If
        End If
    End Sub
    Private Sub DeleteSelectedUser()
        Try
            Dim users As List(Of IUserData) = GetSelectedUserArray()
            If users.ListExists Then
                If USER_CONTEXT.Visible Then USER_CONTEXT.Hide()
                Dim userProvider As IFormatProvider = GetUserListProvider(True)
                Dim ugn As Func(Of IUserData, String) = Function(u) AConvert(Of String)(u, userProvider)
                Dim m As New MMessage(users.ListToStringE(vbNewLine, userProvider), "Users deleting",
                                      {New MsgBoxButton("Delete and ban") With {
                                            .ToolTip = "Users and their data will be deleted and added to the blacklist",
                                            .KeyCode = Keys.Enter},
                                       New MsgBoxButton("Delete user only and ban") With {
                                            .ToolTip = "Users will be deleted and added to the blacklist (user data will not be deleted)"},
                                       New MsgBoxButton("Delete and ban with reason") With {
                                            .ToolTip = "Users and their data will be deleted and added to the blacklist with set a reason to delete",
                                            .KeyCode = New ButtonKey(Keys.Enter,, True)},
                                       New MsgBoxButton("Delete user only and ban with reason") With {
                                            .ToolTip = "Users will be deleted and added to the blacklist with set a reason to delete (user data will not be deleted)"},
                                       New MsgBoxButton("Delete") With {
                                            .ToolTip = "Delete users and their data",
                                            .KeyCode = New ButtonKey(Keys.Enter, True)},
                                       New MsgBoxButton("Delete user only") With {.ToolTip = "Delete users but keep data"}, "Cancel"},
                                      MsgBoxStyle.Exclamation) With {.ButtonsPerRow = 2, .ButtonsPlacing = MMessage.ButtonsPlacings.StartToEnd}
                m.Text = $"The following users ({users.Count}) will be deleted:{vbNewLine}{m.Text}"
                Dim result% = MsgBoxE(m)
                If result < 6 Then
                    Dim collectionResult% = -1
                    Dim tmpResult%
                    Dim tmpUserNames As New List(Of String)
                    Dim IsMultiple As Boolean = users.Count > 1
                    Dim removedUsers As New List(Of String)
                    Dim keepData As Boolean = Not (result Mod 2) = 0
                    Dim banUser As Boolean = result < 4
                    Dim setReason As Boolean = banUser And result > 1
                    Dim leftUsers As New List(Of String)
                    Dim l As New ListAddParams(LAP.NotContainsOnly)
                    Dim b As Boolean = False
                    Dim reason$ = String.Empty
                    If setReason Then reason = InputBoxE("Enter a deletion reason:", "Deletion reason")
                    For Each user In users
                        If keepData Then
                            If banUser Then
                                If user.IsCollection Then
                                    Settings.BlackList.ListAddList(DirectCast(user, UserDataBind).
                                                                   Collections.Select(Function(u) New UserBan(u.Name, reason)), l)
                                Else
                                    Settings.BlackList.ListAddValue(New UserBan(user.Name, reason), l)
                                End If
                                b = True
                            End If
                            If user.IsCollection Then
                                With DirectCast(user, UserDataBind)
                                    If .Count > 0 Then .Collections.ForEach(Sub(c) Settings.UsersList.Remove(DirectCast(c, UserDataBase).User))
                                End With
                            Else
                                Settings.UsersList.Remove(DirectCast(user, UserDataBase).User)
                            End If
                            Settings.Users.Remove(user)
                            Settings.UpdateUsersList()
                            RemoveUserFromList(user)
                            removedUsers.Add(ugn(user))
                            user.Dispose()
                        Else
                            If banUser Then
                                tmpUserNames.Clear()
                                If user.IsCollection Then
                                    tmpUserNames.ListAddList(DirectCast(user, UserDataBind).Collections.Select(Function(u) u.Name), l)
                                Else
                                    tmpUserNames.Add(user.Name)
                                End If
                            End If
                            tmpResult = user.Delete(IsMultiple, collectionResult)
                            If user.IsCollection And collectionResult = -1 Then collectionResult = tmpResult
                            If tmpResult > 0 Then
                                If banUser And tmpUserNames.Count > 0 Then Settings.BlackList.ListAddList(tmpUserNames.Select(Function(u) New UserBan(u, reason)), l) : b = True
                                RemoveUserFromList(user)
                                removedUsers.Add(ugn(user))
                            Else
                                leftUsers.Add(ugn(user))
                            End If
                        End If
                    Next
                    m = New MMessage(String.Empty, "Users deleting")
                    If removedUsers.Count = users.Count Then
                        If removedUsers.Count = 1 Then
                            m.Text = "User deleted"
                        Else
                            m.Text = "All users were deleted"
                        End If
                    ElseIf removedUsers.Count = 0 Then
                        m.Text = "No one user deleted!"
                        m.Style = MsgBoxStyle.Critical
                    Else
                        m.Text = $"The following users were deleted:{vbNewLine}{removedUsers.ListToString(vbNewLine)}{vbNewLine.StringDup(2)}"
                        m.Text &= $"The following users were NOT deleted:{vbNewLine}{leftUsers.ListToString(vbNewLine)}"
                        m.Style = MsgBoxStyle.Exclamation
                    End If
                    If b Then Settings.UpdateBlackList()
                    MsgBoxE(m)
                Else
                    MsgBoxE("Operation canceled")
                End If
            End If
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Error when trying to delete user / collection")
        End Try
    End Sub
    Private Sub CopyUserData()
        Const MsgTitle$ = "Copying user data"
        Try
            Dim users As List(Of IUserData) = GetSelectedUserArray()
            If users.ListExists Then
                Dim f As SFile = Settings.LastCopyPath
                Dim _select_path As Func(Of Boolean) = Function() As Boolean
                                                           f = SFile.SelectPath(f)
                                                           If f.Exists(SFO.Path, False) Then
                                                               Return MsgBoxE({$"Are you sure you want to copy the data to the selected folder?{vbCr}{f}",
                                                                              MsgTitle}, vbQuestion + vbYesNo) = vbYes
                                                           Else
                                                               MsgBoxE({$"Destination path not selected.{vbCr}Operation canceled.", MsgTitle}, vbExclamation)
                                                               Return False
                                                           End If
                                                       End Function
                If f.Exists(SFO.Path, False) Then
                    Select Case MsgBoxE({$"Last folder you copied to:{vbCr}{f}" & vbCr &
                                         "Do you want to copy to this folder or choose another destination?", MsgTitle}, vbQuestion,,,
                                        {New MsgBoxButton("Process") With {.ToolTip = "Use last folder"},
                                         New MsgBoxButton("Choose new") With {.ToolTip = "Choose a new destination"},
                                         New MsgBoxButton("Cancel")})
                        Case 1 : If Not _select_path.Invoke Then Exit Sub
                        Case 2 : MsgBoxE({"Operation canceled", MsgTitle}) : Exit Sub
                    End Select
                Else
                    If Not _select_path.Invoke Then Exit Sub
                End If
                If f.Exists(SFO.Path, False) Then
                    Dim userProvider As IFormatProvider = GetUserListProvider(True)
                    Settings.LastCopyPath.Value = f
                    Using logger As New TextSaver With {.LogMode = True}
                        Dim m As New MMessage("", MsgTitle,,, {logger})
                        Dim err As New ErrorsDescriber(EDP.SendToLog) With {.DeclaredMessage = m}
                        Dim __copied_users As New List(Of IUserData)
                        Dim __copied_users_not As New List(Of IUserData)
                        For Each user As IUserData In users
                            If user.CopyFiles(f, err) Then
                                __copied_users.Add(user)
                            Else
                                __copied_users_not.Add(user)
                            End If
                        Next
                        err = Nothing
                        Dim buttons As New List(Of MsgBoxButton) From {New MsgBoxButton("OK")}
                        If __copied_users_not.Count > 0 Then
                            err = New ErrorsDescriber(EDP.ShowAllMsg)
                            m.Style = If(__copied_users.Count > 0, vbExclamation, vbCritical)
                            If Not logger.IsEmptyString Then
                                m.DefaultButton = 0
                                m.CancelButton = 0
                                buttons.Add(New MsgBoxButton("Show LOG") With {
                                            .IsDialogResultButton = False,
                                            .BackColor = MyColor.DeleteBack,
                                            .ForeColor = MyColor.DeleteFore,
                                            .KeyCode = Keys.F1,
                                            .ToolTip = "Show error log",
                                            .CallBack = Sub(r, mm, b)
                                                            Using ff As New LOG_FORM(logger) : ff.ShowDialog() : End Using
                                                        End Sub})
                            End If
                        End If
                        m.Buttons = buttons
                        If __copied_users_not.Count = 0 Then
                            m.Text = "All users are copied."
                        ElseIf __copied_users.Count = 0 And __copied_users_not.Count > 0 Then
                            m.Text = "No users have been copied."
                        Else
                            m.Text = $"The following users have been copied:{vbNewLine}"
                            m.Text &= __copied_users.ListToStringE(vbNewLine, userProvider)
                            If __copied_users_not.Count > 0 Then
                                m.Text = $"{vbNewLine.StringDup(2)}The following users have not been copied:{vbNewLine}"
                                m.Text &= __copied_users_not.ListToStringE(vbNewLine, userProvider)
                            End If
                        End If
                        MsgBoxE(m,, err)
                    End Using
                End If
            End If
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Error when trying to copy data")
        End Try
    End Sub
    Private Sub OpenFolder()
        Dim user As IUserData = GetSelectedUser()
        If Not user Is Nothing Then user.OpenFolder()
    End Sub
#End Region
#Region "Operations with selected users: list"
    Private Overloads Sub RemoveUserFromList(ByVal _User As IUserData)
        RemoveUserFromList(LIST_PROFILES.Items.IndexOfKey(_User.Key), _User.Key)
    End Sub
    Private Overloads Sub RemoveUserFromList(ByVal _Index As Integer, ByVal Key As String)
        Dim a As Action = Sub()
                              With LIST_PROFILES
                                  If _Index >= 0 Then
                                      .Items.RemoveAt(_Index)
                                      If Settings.ViewModeIsPicture Then
                                          Dim ImgIndx%
                                          Select Case Settings.ViewMode.Value
                                              Case View.LargeIcon
                                                  ImgIndx = .LargeImageList.Images.IndexOfKey(Key)
                                                  If ImgIndx >= 0 Then .LargeImageList.Images.RemoveAt(_Index)
                                              Case View.SmallIcon
                                                  ImgIndx = .SmallImageList.Images.IndexOfKey(Key)
                                                  If ImgIndx >= 0 Then .SmallImageList.Images.RemoveAt(_Index)
                                          End Select
                                      End If
                                  End If
                              End With
                          End Sub
        If LIST_PROFILES.InvokeRequired Then LIST_PROFILES.Invoke(a) Else a.Invoke
    End Sub
#End Region
#Region "Handlers"
    Private Sub UpdateLabelsGroups()
        If Settings.Labels.NewLabelsExists Then
            If Settings.Labels.NewLabels.Count > 0 Then
                Dim ll As ListViewGroup = Nothing
                Dim a As Action = Sub() LIST_PROFILES.Groups.Add(ll)
                For Each l$ In Settings.Labels.NewLabels
                    ll = New ListViewGroup(l, l)
                    If Not LIST_PROFILES.Groups.Contains(ll) Then
                        If LIST_PROFILES.InvokeRequired Then LIST_PROFILES.Invoke(a) Else a.Invoke
                    End If
                Next
            End If
            Settings.Labels.Update()
            Settings.Labels.NewLabels.Clear()
        End If
    End Sub
    Friend Sub UserRemovedFromCollection(ByVal User As IUserData)
        If LIST_PROFILES.Items.Count = 0 OrElse Not LIST_PROFILES.Items.ContainsKey(User.Key) Then UserListUpdate(User, True)
    End Sub
    Friend Sub CollectionRemoved(ByVal User As IUserData)
        With LIST_PROFILES.Items
            If .Count > 0 AndAlso .ContainsKey(User.Key) Then .RemoveByKey(User.Key)
        End With
    End Sub
    Friend Sub User_OnUserUpdated(ByVal User As IUserData)
        UserListUpdate(User, False)
    End Sub
    Private Sub Downloader_UpdateJobsCount(ByVal TotalCount As Integer)
        ControlInvokeFast(Toolbar_BOTTOM, LBL_JOBS_COUNT, Sub() LBL_JOBS_COUNT.Text = IIf(TotalCount = 0, String.Empty, $"[Jobs {TotalCount}]"))
    End Sub
    Private Sub Downloader_Downloading(ByVal Value As Boolean)
        Dim __isDownloading As Boolean = Value Or Downloader.Working(False)
        ControlInvokeFast(Toolbar_TOP, BTT_DOWN_STOP, Sub() BTT_DOWN_STOP.Enabled = __isDownloading)
        TrayIcon.Icon = If(__isDownloading, My.Resources.ArrowDownIcon_Blue_24, My.Resources.RainbowIcon_48)
    End Sub
#End Region
End Class