Imports System.ComponentModel
Imports System.Globalization
Imports System.Threading
Imports PersonalUtilities.Forms
Imports SCrawler.API
Imports SCrawler.API.Base
Imports SCrawler.Editors
Public Class MainFrame
    Private MyView As FormsView
    Private ReadOnly _VideoDownloadingMode As Boolean = False
    Private MyChannels As ChannelViewForm
    Private _UFinit As Boolean = True
    Public Sub New()
        InitializeComponent()
        Dim n As DateTimeFormatInfo = CultureInfo.GetCultureInfo("en-us").DateTimeFormat.Clone
        n.FullDateTimePattern = "ddd MMM dd HH:mm:ss +ffff yyyy"
        n.TimeSeparator = String.Empty
        Twitter.DateProvider = New ADateTime(DirectCast(n.Clone, DateTimeFormatInfo)) With {.DateTimeStyle = DateTimeStyles.AssumeUniversal}
        Settings = New SettingsCLS
        Dim Args() As String = Environment.GetCommandLineArgs
        If Args.ListExists(2) AndAlso Args(1) = "v" Then
            Using f As New VideosDownloaderForm : f.ShowDialog() : End Using
            _VideoDownloadingMode = True
        Else
            Downloader = New TDownloader
        End If
    End Sub
    Private Sub MainFrame_Load(sender As Object, e As EventArgs) Handles Me.Load
        If _VideoDownloadingMode Then GoTo FormClosingInvoker
        InfoForm = New DownloadedInfoForm
        AddHandler Downloader.OnJobsChange, AddressOf Downloader_UpdateJobsCount
        AddHandler Downloader.OnDownloading, AddressOf Downloader_OnDownloading
        AddHandler Downloader.OnDownloadCountChange, AddressOf InfoForm.Downloader_OnDownloadCountChange
        Settings.LoadUsers()
        MyView = New FormsView(Me)
        MyView.ImportFromXML(Settings.Design)
        MyView.SetMeSize()
        MainProgress = New Toolbars.MyProgress(Toolbar_BOTTOM, PR_MAIN, LBL_STATUS) With {.DropCurrentProgressOnTotalChange = False}
        Dim gk$
        With LIST_PROFILES.Groups
            gk = GetLviGroupName(Sites.Undefined, False, False, True)
            .Add(New ListViewGroup(gk, gk))
            gk = GetLviGroupName(Sites.Undefined, False, True, True)
            .Add(New ListViewGroup(gk, gk))
            gk = GetLviGroupName(Sites.Undefined, True, False, True)
            .Add(New ListViewGroup(gk, gk))
            For Each s In {Sites.Reddit, Sites.Twitter}
                gk = GetLviGroupName(s, False, True, False)
                .Add(New ListViewGroup(gk, gk))
                gk = GetLviGroupName(s, False, False, False)
                .Add(New ListViewGroup(gk, gk))
                gk = GetLviGroupName(s, True, False, False)
                .Add(New ListViewGroup(gk, gk))
            Next
            If Settings.Labels.Count > 0 Then Settings.Labels.ToList.ForEach(Sub(l) .Add(New ListViewGroup(l, l)))
            .Add(Settings.Labels.NoLabel)
        End With
        With Settings
            LIST_PROFILES.View = .ViewMode
            SetViewButtonsCheckers(.ViewMode.Value = View.LargeIcon, .ViewMode.Value = View.SmallIcon, .ViewMode.Value = View.List)
            AddHandler .Labels.NewLabelAdded, AddressOf UpdateLabelsGroups
        End With
        RefillList()
        UpdateLabelsGroups()
        SetShowButtonsCheckers(Settings.ShowingMode.Value)
        CheckVersion(False)
        _UFinit = False
        GoTo EndFunction
FormClosingInvoker:
        Close()
EndFunction:
    End Sub
    Private _CloseInvoked As Boolean = False
    Private Async Sub MainFrame_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Not _VideoDownloadingMode Then
            If _CloseInvoked Then GoTo CloseResume
            Dim ChannelsWorking As Func(Of Boolean) = Function() If(MyChannels?.Working, False)
            If (Not Downloader.Working And Not ChannelsWorking.Invoke) OrElse
               MsgBoxE({"Program still downloading something..." & vbNewLine &
                        "Do you really want to stop downloading and exit of program?",
                        "Downloading in progress"},
                       MsgBoxStyle.Exclamation,,,
                       {"Stop downloading and close", "Cancel"}) = 0 Then
                If Downloader.Working Then _CloseInvoked = True : Downloader.Stop()
                If ChannelsWorking.Invoke Then _CloseInvoked = True : MyChannels.Stop(False)
                If _CloseInvoked Then
                    e.Cancel = True
                    Await Task.Run(Sub()
                                       While Downloader.Working Or ChannelsWorking.Invoke : Thread.Sleep(500) : End While
                                   End Sub)
                End If
                Downloader.Dispose()
                InfoForm.Dispose()
                If Not MyChannels Is Nothing Then MyChannels.Dispose()
                If Not VideoDownloader Is Nothing Then VideoDownloader.Dispose()
                MyView.Dispose(Settings.Design)
                Settings.Dispose()
            Else
                e.Cancel = True
                Exit Sub
            End If
        End If
        If Not MyMainLOG.IsEmptyString Then SaveLogToFile()
        If _CloseInvoked Then Close()
CloseResume:
    End Sub
    Private Sub MainFrame_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Dim b As Boolean = True
        Select Case e.KeyCode
            Case Keys.Insert : BTT_ADD_USER.PerformClick()
            Case Keys.Delete : DeleteSelectedUser()
            Case Keys.F1 : BTT_VERSION_INFO.PerformClick()
            Case Keys.F2 : DownloadVideoByURL()
            Case Keys.F3 : EditSelectedUser()
            Case Keys.F5 : BTT_DOWN_SELECTED.PerformClick()
            Case Keys.F6 : If Settings.ShowingMode.Value = ShowingModes.All Then BTT_DOWN_ALL.PerformClick()
            Case Else : b = False
        End Select
        If b Then e.Handled = True
    End Sub
    Private Sub BTT_VERSION_INFO_Click(sender As Object, e As EventArgs) Handles BTT_VERSION_INFO.Click
        CheckVersion(True)
    End Sub
    Friend Sub RefillList()
        Dim a As Action = Sub()
                              With LIST_PROFILES
                                  .Items.Clear()
                                  If Not .LargeImageList Is Nothing Then .LargeImageList.Images.Clear()
                                  .LargeImageList = New ImageList
                                  If Not .SmallImageList Is Nothing Then .SmallImageList.Images.Clear()
                                  .SmallImageList = New ImageList
                                  .LargeImageList.ColorDepth = ColorDepth.Depth32Bit
                                  .SmallImageList.ColorDepth = ColorDepth.Depth32Bit
                                  .LargeImageList.ImageSize = New Size(DivideWithZeroChecking(Settings.MaxLargeImageHeigh.Value, 100) * 75, Settings.MaxLargeImageHeigh.Value)
                                  .SmallImageList.ImageSize = New Size(DivideWithZeroChecking(Settings.MaxSmallImageHeigh.Value, 100) * 75, Settings.MaxSmallImageHeigh.Value)
                              End With
                          End Sub
        If LIST_PROFILES.InvokeRequired Then LIST_PROFILES.Invoke(a) Else a.Invoke
        If Settings.Users.Count > 0 Then
            Settings.Users.Sort()
            Dim t As New List(Of Task)
            For Each User As IUserData In Settings.Users
                If User.FitToAddParams Then
                    If Settings.ViewModeIsPicture Then
                        t.Add(Task.Run(Sub() UserListUpdate(User, True)))
                    Else
                        UserListUpdate(User, True)
                    End If
                End If
            Next
            If t.Count > 0 Then Task.WhenAll(t.ToArray) : t.Clear()
        End If
    End Sub
    Private Sub UserListUpdate(ByVal User As IUserData, ByVal Add As Boolean)
        Try
            Dim a As Action
            If Add Then
                a = Sub()
                        With LIST_PROFILES
                            Select Case Settings.ViewMode.Value
                                Case View.LargeIcon : .LargeImageList.Images.Add(User.LVIKey, User.GetPicture())
                                Case View.SmallIcon : .SmallImageList.Images.Add(User.LVIKey, User.GetPicture())
                            End Select
                            .Items.Add(User.GetLVI(LIST_PROFILES))
                        End With
                    End Sub
            Else
                a = Sub()
                        With LIST_PROFILES
                            Dim i% = .Items.IndexOfKey(User.LVIKey)
                            Dim ImgIndx%
                            If i >= 0 Then
                                Select Case Settings.ViewMode.Value
                                    Case View.LargeIcon
                                        ImgIndx = .LargeImageList.Images.IndexOfKey(User.LVIKey)
                                        If ImgIndx >= 0 Then .LargeImageList.Images(ImgIndx) = User.GetPicture()
                                    Case View.SmallIcon
                                        ImgIndx = .SmallImageList.Images.IndexOfKey(User.LVIKey)
                                        If ImgIndx >= 0 Then .SmallImageList.Images(ImgIndx) = User.GetPicture()
                                End Select
                                .Items(i).Text = User.ToString
                                .Items(i).Group = User.GetLVIGroup(LIST_PROFILES)
                            End If
                        End With
                    End Sub
            End If
            If LIST_PROFILES.InvokeRequired Then LIST_PROFILES.Invoke(a) Else a.Invoke
        Catch ex As Exception
        End Try
    End Sub
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
            Settings.Labels.NewLabels.Clear()
        End If
    End Sub
    Private Sub OnUsersAddedHandler(ByVal StartIndex As Integer)
        If StartIndex <= Settings.Users.Count - 1 Then
            For i% = StartIndex To Settings.Users.Count - 1 : UserListUpdate(Settings.Users(i), True) : Next
        End If
    End Sub
#Region "Toolbar buttons"
#Region "Settings"
    Private Sub BTT_SETTINGS_REDDIT_Click(sender As Object, e As EventArgs) Handles BTT_SETTINGS_REDDIT.Click
        Using f As New RedditEditorForm : f.ShowDialog() : End Using
    End Sub
    Private Sub BTT_SETTINGS_TWITTER_Click(sender As Object, e As EventArgs) Handles BTT_SETTINGS_TWITTER.Click
        Using f As New TwitterEditorForm : f.ShowDialog() : End Using
    End Sub
    Private Sub BTT_SETTINGS_Click(sender As Object, e As EventArgs) Handles BTT_SETTINGS.Click
        Dim mhl% = Settings.MaxLargeImageHeigh.Value
        Dim mhs% = Settings.MaxSmallImageHeigh.Value
        Using f As New GlobalSettingsForm
            f.ShowDialog()
            If f.DialogResult = DialogResult.OK Then
                If Not Settings.MaxLargeImageHeigh = mhl Or Not Settings.MaxSmallImageHeigh = mhs Then RefillList()
            End If
        End Using
    End Sub
#End Region
#Region "User"
    Private Sub BTT_ADD_USER_Click(sender As Object, e As EventArgs) Handles BTT_ADD_USER.Click
        Using f As New UserCreatorForm
            f.ShowDialog()
            If f.DialogResult = DialogResult.OK Then
                Dim i%
                If f.StartIndex >= 0 Then
                    OnUsersAddedHandler(f.StartIndex)
                Else
                    i = Settings.Users.FindIndex(Function(u) u.Site = f.User.Site And u.Name = f.User.Name)
                    If i < 0 Then
                        If Not UserBanned(f.User.Name) Then
                            Settings.UpdateUsersList(f.User)
                            Settings.Users.Add(UserDataBase.GetInstance(f.User))
                            With Settings.Users(Settings.Users.Count - 1)
                                .Favorite = f.UserFavorite
                                .Temporary = f.UserTemporary
                                .ParseUserMediaOnly = f.UserMediaOnly
                                .ReadyForDownload = f.UserReady
                                .FriendlyName = f.UserFriendly
                                .Description = f.UserDescr
                                .Labels.ListAddList(f.UserLabels, LAP.ClearBeforeAdd, LAP.NotContainsOnly)
                                .UpdateUserInformation()
                            End With
                            UserListUpdate(Settings.Users(Settings.Users.Count - 1), True)
                            i = LIST_PROFILES.Items.IndexOfKey(Settings.Users(Settings.Users.Count - 1).LVIKey)
                            If i >= 0 Then
                                LIST_PROFILES.SelectedIndices.Clear()
                                With LIST_PROFILES.Items(i)
                                    .Selected = True
                                    .Focused = True
                                End With
                                LIST_PROFILES.EnsureVisible(i)
                            End If
                        Else
                            MsgBoxE($"User [{f.User.Name}] was not added")
                        End If
                    Else
                        i = LIST_PROFILES.Items.IndexOfKey(Settings.Users(i).LVIKey)
                        If i >= 0 Then
                            LIST_PROFILES.SelectedIndices.Clear()
                            With LIST_PROFILES.Items(i)
                                .Selected = True
                                .Focused = True
                            End With
                            LIST_PROFILES.EnsureVisible(i)
                        End If
                        MsgBoxE($"User [{f.User.Name}] already exists", MsgBoxStyle.Exclamation)
                    End If
                End If
            End If
        End Using
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
    Private Sub BTT_SHOW_INFO_Click(sender As Object, e As EventArgs) Handles BTT_SHOW_INFO.Click
        ShowInfoForm(True)
    End Sub
    Private Overloads Sub ShowInfoForm()
        ShowInfoForm(False)
    End Sub
    Private Overloads Sub ShowInfoForm(ByVal BringToFrontIfOpen As Boolean)
        If InfoForm.Visible Then
            If BringToFrontIfOpen Then InfoForm.BringToFront()
        Else
            InfoForm.Show()
        End If
    End Sub
    Private Sub BTT_CHANNELS_Click(sender As Object, e As EventArgs) Handles BTT_CHANNELS.Click
        If MyChannels Is Nothing Then
            MyChannels = New ChannelViewForm
            AddHandler MyChannels.OnUsersAdded, AddressOf OnUsersAddedHandler
        End If
        If MyChannels.Visible Then MyChannels.BringToFront() Else MyChannels.Show()
    End Sub
#End Region
#Region "Download"
    Private Sub BTT_DOWN_SELECTED_Click(sender As Object, e As EventArgs) Handles BTT_DOWN_SELECTED.Click
        DownloadSelectedUser(False)
    End Sub
    Private Sub BTT_DOWN_ALL_Click(sender As Object, e As EventArgs) Handles BTT_DOWN_ALL.Click
        Downloader.AddRange(Settings.Users.Where(Function(u) u.ReadyForDownload))
    End Sub
    Private Sub BTT_DOWN_VIDEO_Click(sender As Object, e As EventArgs) Handles BTT_DOWN_VIDEO.Click
        DownloadVideoByURL()
    End Sub
    Private Sub BTT_DOWN_STOP_Click(sender As Object, e As EventArgs) Handles BTT_DOWN_STOP.Click
        Downloader.Stop()
    End Sub
#End Region
#Region "View"
    Private Sub BTT_VIEW_LARGE_Click(sender As Object, e As EventArgs) Handles BTT_VIEW_LARGE.Click
        LIST_PROFILES.View = View.LargeIcon
        Dim b As Boolean = Not (Settings.ViewMode.Value = View.LargeIcon)
        Settings.ViewMode.Value = View.LargeIcon
        SetViewButtonsCheckers(True, False, False)
        If b Then RefillList()
    End Sub
    Private Sub BTT_VIEW_SMALL_Click(sender As Object, e As EventArgs) Handles BTT_VIEW_SMALL.Click
        LIST_PROFILES.View = View.SmallIcon
        Dim b As Boolean = Not (Settings.ViewMode.Value = View.SmallIcon)
        Settings.ViewMode.Value = View.SmallIcon
        SetViewButtonsCheckers(False, True, False)
        If b Then RefillList()
    End Sub
    Private Sub BTT_VIEW_LIST_Click(sender As Object, e As EventArgs) Handles BTT_VIEW_LIST.Click
        LIST_PROFILES.View = View.List
        Dim b As Boolean = Not (Settings.ViewMode.Value = View.List)
        Settings.ViewMode.Value = View.List
        SetViewButtonsCheckers(False, False, True)
        If b Then
            With LIST_PROFILES
                .LargeImageList.Images.Clear()
                .SmallImageList.Images.Clear()
            End With
        End If
    End Sub
    Private Sub SetViewButtonsCheckers(ByVal Large As Boolean, ByVal Small As Boolean, ByVal List As Boolean)
        BTT_VIEW_LARGE.Checked = Large
        BTT_VIEW_SMALL.Checked = Small
        BTT_VIEW_LIST.Checked = List
    End Sub
#End Region
#Region "Labels"
    Private Sub BTT_SHOW_ALL_Click(sender As Object, e As EventArgs) Handles BTT_SHOW_ALL.Click
        SetShowButtonsCheckers(ShowingModes.All)
    End Sub
    Private Sub BTT_SHOW_REGULAR_Click(sender As Object, e As EventArgs) Handles BTT_SHOW_REGULAR.Click
        SetShowButtonsCheckers(ShowingModes.Regular)
    End Sub
    Private Sub BTT_SHOW_TEMP_Click(sender As Object, e As EventArgs) Handles BTT_SHOW_TEMP.Click
        SetShowButtonsCheckers(ShowingModes.Temporary)
    End Sub
    Private Sub BTT_SHOW_FAV_Click(sender As Object, e As EventArgs) Handles BTT_SHOW_FAV.Click
        SetShowButtonsCheckers(ShowingModes.Favorite)
    End Sub
    Private Sub BTT_SHOW_LABELS_Click(sender As Object, e As EventArgs) Handles BTT_SHOW_LABELS.Click
        SetShowButtonsCheckers(ShowingModes.Labels)
    End Sub
    Private Sub BTT_SHOW_NO_LABELS_Click(sender As Object, e As EventArgs) Handles BTT_SHOW_NO_LABELS.Click
        SetShowButtonsCheckers(ShowingModes.NoLabels)
    End Sub
    Private Sub SetShowButtonsCheckers(ByVal m As ShowingModes)
        BTT_SHOW_ALL.Checked = m = ShowingModes.All
        BTT_SHOW_REGULAR.Checked = m = ShowingModes.Regular
        BTT_SHOW_TEMP.Checked = m = ShowingModes.Temporary
        BTT_SHOW_FAV.Checked = m = ShowingModes.Favorite
        BTT_SHOW_LABELS.Checked = m = ShowingModes.Labels
        BTT_SHOW_NO_LABELS.Checked = m = ShowingModes.NoLabels
        BTT_SELECT_LABELS.Enabled = BTT_SHOW_LABELS.Checked
        If Not Settings.ShowingMode.Value = m Then
            If Not m = ShowingModes.Labels Or Settings.Labels.CurrentSelection.Count > 0 Then
                Settings.ShowingMode.Value = m
                RefillList()
            ElseIf m = ShowingModes.Labels And Settings.Labels.CurrentSelection.Count = 0 Then
                OpenLabelsForm()
                If Settings.Labels.CurrentSelection.Count > 0 Then
                    Settings.ShowingMode.Value = m
                    RefillList()
                Else
                    SetShowButtonsCheckers(Settings.ShowingMode.Value)
                    Exit Sub
                End If
            ElseIf m = ShowingModes.NoLabels Then
                Settings.ShowingMode.Value = m
                RefillList()
            End If
        End If
        Settings.ShowingMode.Value = m
        If Not m = ShowingModes.All Then BTT_DOWN_ALL.Enabled = False
    End Sub
    Private Sub BTT_SELECT_LABELS_Click(sender As Object, e As EventArgs) Handles BTT_SELECT_LABELS.Click
        OpenLabelsForm()
    End Sub
    Private Sub OpenLabelsForm()
        Using f As New LabelsForm(Settings.Labels.CurrentSelection)
            f.ShowDialog()
            If f.DialogResult = DialogResult.OK Then
                If f.LabelsList.Count > 0 Then
                    Dim b As Boolean = False
                    If Settings.Labels.CurrentSelection.Count = 0 Then
                        b = True
                    Else
                        If Settings.Labels.CurrentSelection.Exists(Function(l) Not f.LabelsList.Contains(l)) Then b = True
                        If Not b AndAlso f.LabelsList.Exists(Function(l) Not Settings.Labels.CurrentSelection.Contains(l)) Then b = True
                    End If
                    Settings.Labels.CurrentSelection.ListAddList(f.LabelsList, LAP.ClearBeforeAdd, LAP.NotContainsOnly)
                    Settings.LatestSelectedLabels.Value = Settings.Labels.CurrentSelection.ListToString(, "|")
                    If b Then RefillList()
                Else
                    Settings.Labels.CurrentSelection.Clear()
                    Settings.LatestSelectedLabels.Value = String.Empty
                    SetShowButtonsCheckers(ShowingModes.All)
                End If
            End If
        End Using
    End Sub
#End Region
    Private Sub BTT_LOG_Click(sender As Object, e As EventArgs) Handles BTT_LOG.Click
        MyMainLOG_ShowForm(Settings.Design)
    End Sub
#Region "List functions"
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
#Region "Context"
    Private Sub BTT_CONTEXT_DOWN_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_DOWN.Click
        DownloadSelectedUser(False)
    End Sub
    Private Sub BTT_CONTEXT_DOWN_LIMITED_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_DOWN_LIMITED.Click
        DownloadSelectedUser(True)
    End Sub
    Private Sub BTT_CONTEXT_EDIT_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_EDIT.Click
        EditSelectedUser()
    End Sub
    Private Sub BTT_CONTEXT_DELETE_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_DELETE.Click
        DeleteSelectedUser()
    End Sub
    Private Sub BTT_CONTEXT_FAV_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_FAV.Click
        Dim users As List(Of IUserData) = GetSelectedUserArray()
        If AskForMassReplace(users, "Favorite") Then
            users.ForEach(Sub(u)
                              u.Favorite = Not u.Favorite
                              u.UpdateUserInformation()
                              UserListUpdate(u, False)
                          End Sub)
        End If
    End Sub
    Private Sub BTT_CONTEXT_TEMP_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_TEMP.Click
        Dim users As List(Of IUserData) = GetSelectedUserArray()
        If AskForMassReplace(users, "Temporary") Then
            users.ForEach(Sub(u)
                              u.Temporary = Not u.Temporary
                              u.UpdateUserInformation()
                              UserListUpdate(u, False)
                          End Sub)
        End If
    End Sub
    Private Sub BTT_CONTEXT_GROUPS_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_GROUPS.Click
        Try
            Dim users As List(Of IUserData) = GetSelectedUserArray()
            If users.ListExists Then
                Dim l As List(Of String) = ListAddList(Nothing, users.SelectMany(Function(u) u.Labels), LAP.NotContainsOnly)
                Using f As New LabelsForm(l) With {.MultiUser = True}
                    f.ShowDialog()
                    If f.DialogResult = DialogResult.OK Then
                        Dim _lp As LAP = LAP.NotContainsOnly
                        If f.MultiUserClearExists Then _lp += LAP.ClearBeforeAdd
                        Dim lp As New ListAddParams(_lp)
                        users.ForEach(Sub(ByVal u As IUserData)
                                          If u.IsCollection Then
                                              With DirectCast(u, UserDataBind)
                                                  If .Count > 0 Then .Collections.ForEach(Sub(uu) uu.Labels.ListAddList(f.LabelsList, lp))
                                              End With
                                          Else
                                              u.Labels.ListAddList(f.LabelsList, lp)
                                          End If
                                          u.UpdateUserInformation()
                                      End Sub)
                    End If
                End Using
            Else
                MsgBoxE("No one user does not detected", vbExclamation)
            End If
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.ShowAllMsg, ex, "[ChangeUserGroups]")
        End Try
    End Sub
    Private Function AskForMassReplace(ByVal users As List(Of IUserData), ByVal param As String) As Boolean
        Dim u$ = users.ListIfNothing.Take(20).Select(Function(uu) uu.Name).ListToString(, vbCr)
        If Not u.IsEmptyString And users.ListExists(21) Then u &= vbCr & "..."
        Return users.ListExists AndAlso (users.Count = 1 OrElse MsgBoxE({$"Do you really want to change [{param}] for {users.Count} users?{vbCr}{vbCr}{u}",
                                                                         "Users' parameters change"},
                                                                        MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo) = MsgBoxResult.Yes)
    End Function
    Private Sub BTT_CHANGE_IMAGE_Click(sender As Object, e As EventArgs) Handles BTT_CHANGE_IMAGE.Click
        Dim user As IUserData = GetSelectedUser()
        If Not user Is Nothing Then
            Dim f As SFile = SFile.SelectFiles(user.File, False, "Select new user picture", "Pictures|*.jpeg;*.jpg;*.png").FirstOrDefault
            If Not f.IsEmptyString Then
                user.SetPicture(f)
                UserListUpdate(user, False)
            End If
        End If
    End Sub
    Private Sub BTT_CONTEXT_ADD_TO_COL_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_ADD_TO_COL.Click
        If Settings.CollectionsPath.Value.IsEmptyString Then
            MsgBoxE("Collection path does not set", MsgBoxStyle.Exclamation)
        Else
            Dim user As IUserData = GetSelectedUser()
            If Not user Is Nothing Then
                If user.IsCollection Then
                    MsgBoxE("Collection can not be added to collection!", MsgBoxStyle.Critical)
                Else
                    Using f As New CollectionEditorForm(user.CollectionName)
                        f.ShowDialog()
                        If f.DialogResult = DialogResult.OK Then
                            With Settings
                                Dim fCol As Predicate(Of IUserData) = Function(u) u.IsCollection And u.CollectionName = f.Collection
                                Dim i% = .Users.FindIndex(fCol)
                                Dim Added As Boolean = i < 0
                                If i < 0 Then
                                    .Users.Add(New UserDataBind(f.Collection))
                                    i = .Users.Count - 1
                                End If
                                Try
                                    DirectCast(.Users(i), UserDataBind).Add(user)
                                    RemoveUserFromList(user)
                                    i = .Users.FindIndex(fCol)
                                    If i >= 0 Then UserListUpdate(.Users(i), Added) Else RefillList()
                                    MsgBoxE($"[{user.Name}] was added to collection [{f.Collection}]")
                                Catch ex As InvalidOperationException
                                    i = .Users.FindIndex(fCol)
                                    If i >= 0 Then
                                        If DirectCast(.Users(i), UserDataBind).Count = 0 Then
                                            .Users(i).Dispose()
                                            .Users.RemoveAt(i)
                                        End If
                                    End If
                                End Try
                            End With
                        End If
                    End Using
                End If
            End If
        End If
    End Sub
    Private Sub BTT_CONTEXT_COL_MERGE_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_COL_MERGE.Click
        Dim user As IUserData = GetSelectedUser()
        If Not user Is Nothing Then
            If user.IsCollection Then
                If DirectCast(user, UserDataBind).DataMerging Then
                    MsgBoxE("Collection files are already merged")
                Else
                    If MsgBoxE({"Do you really want to merge collection files into one folder?" & vbNewLine &
                                "This action is not turnable!", "Merging files"},
                               MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        DirectCast(user, UserDataBind).DataMerging = True
                    End If
                End If
            Else
                MsgBoxE("This is not collection!", MsgBoxStyle.Exclamation)
            End If
        End If
    End Sub
    Private Sub BTT_CONTEXT_OPEN_PATH_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_OPEN_PATH.Click
        OpenFolder()
    End Sub
    Private Sub BTT_CONTEXT_OPEN_SITE_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_OPEN_SITE.Click
        Dim user As IUserData = GetSelectedUser()
        If Not user Is Nothing Then user.OpenSite()
    End Sub
    Private Sub BTT_CONTEXT_INFO_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_INFO.Click
        Dim user As IUserData = GetSelectedUser()
        If Not user Is Nothing Then MsgBoxE(DirectCast(user, UserDataBase).GetUserInformation())
    End Sub
    Private Sub USER_CONTEXT_VisibleChanged(sender As Object, e As EventArgs) Handles USER_CONTEXT.VisibleChanged
        Try
            If USER_CONTEXT.Visible Then
                Dim user As IUserData = GetSelectedUser()
                If Not user Is Nothing AndAlso user.IsCollection Then
                    With DirectCast(user, UserDataBind)
                        BTT_CONTEXT_DOWN.DropDownItems.AddRange(.ContextDown)
                        BTT_CONTEXT_EDIT.DropDownItems.AddRange(.ContextEdit)
                        BTT_CONTEXT_DELETE.DropDownItems.AddRange(.ContextDelete)
                        BTT_CONTEXT_OPEN_PATH.DropDownItems.AddRange(.ContextPath)
                        BTT_CONTEXT_OPEN_SITE.DropDownItems.AddRange(.ContextSite)
                    End With
                End If
            Else
                BTT_CONTEXT_DOWN.DropDownItems.Clear()
                BTT_CONTEXT_EDIT.DropDownItems.Clear()
                BTT_CONTEXT_DELETE.DropDownItems.Clear()
                BTT_CONTEXT_OPEN_PATH.DropDownItems.Clear()
                BTT_CONTEXT_OPEN_SITE.DropDownItems.Clear()
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region
#End Region
    Private Function GetSelectedUser() As IUserData
        If _LatestSelected >= 0 And _LatestSelected <= LIST_PROFILES.Items.Count - 1 Then
            Dim k$ = LIST_PROFILES.Items(_LatestSelected).Name
            Dim i% = Settings.Users.FindIndex(Function(u) u.LVIKey = k)
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
                        indx = Settings.Users.FindIndex(Function(u) u.LVIKey = k)
                        If i >= 0 Then l.Add(Settings.Users(indx))
                    Next
                    Return l
                End If
            End With
            Return New List(Of IUserData)
        Catch ex As Exception
            Return ErrorsDescriber.Execute(EDP.SendInLog + EDP.ReturnValue, ex, "[MainFrame.GetSelectedUserArray]", New List(Of IUserData))
        End Try
    End Function
    Private Overloads Sub RemoveUserFromList(ByVal _User As IUserData)
        RemoveUserFromList(LIST_PROFILES.Items.IndexOfKey(_User.LVIKey), _User.LVIKey)
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
    Private Sub EditSelectedUser()
        Dim user As IUserData = GetSelectedUser()
        If Not user Is Nothing Then
            On Error Resume Next
            If user.IsCollection Then
                If USER_CONTEXT.Visible Then USER_CONTEXT.Hide()
                MsgBoxE("This is collection!{vbNewLine}Edit collections does not allowed!", vbExclamation)
            Else
                Using f As New UserCreatorForm(user)
                    f.ShowDialog()
                    If f.DialogResult = DialogResult.OK Then UserListUpdate(user, False)
                End Using
            End If
        End If
    End Sub
    Private Sub DeleteSelectedUser()
        Try
            Dim users As List(Of IUserData) = GetSelectedUserArray()
            If users.ListExists Then
                If USER_CONTEXT.Visible Then USER_CONTEXT.Hide()
                Dim ugn As Func(Of IUserData, String) = Function(u) $"{IIf(u.IsCollection, "Collection", "User")}: {u.Name}"
                Dim m As New MMessage(users.Select(ugn).ListToString(, vbNewLine), "Users deleting",
                                      {New Messaging.MsgBoxButton("Delete and ban") With {.ToolTip = "Users will be deleted and added to the blacklist"},
                                       New Messaging.MsgBoxButton("Delete and ban with reason") With {
                                            .ToolTip = "Users will be deleted and added to the blacklist with set a reason to delete"},
                                       "Delete", "Cancel"}, MsgBoxStyle.Exclamation) With {.ButtonsPerRow = 2}
                m.Text = $"The following users ({users.Count}) will be deleted:{vbNewLine}{m.Text}"
                Dim result% = MsgBoxE(m)
                If result < 3 Then
                    Dim removedUsers As New List(Of String)
                    Dim leftUsers As New List(Of String)
                    Dim l As New ListAddParams(LAP.NotContainsOnly)
                    Dim b As Boolean = False
                    Dim reason$ = String.Empty
                    If result = 1 Then reason = InputBoxE("Enter a deletion reason:", "Deletion reason")
                    For Each user In users
                        If user.Delete > 0 Then
                            If result < 2 Then Settings.BlackList.ListAddValue(New UserBan(user.Name, reason), l) : b = True
                            RemoveUserFromList(user)
                            removedUsers.Add(ugn(user))
                        Else
                            leftUsers.Add(ugn(user))
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
                        m.Text = $"The following users were deleted:{vbNewLine}{removedUsers.ListToString(, vbNewLine)}{vbNewLine.StringDup(2)}"
                        m.Text &= $"The following users were NOT deleted:{vbNewLine}{leftUsers.ListToString(, vbNewLine)}"
                        m.Style = MsgBoxStyle.Exclamation
                    End If
                    If b Then Settings.UpdateBlackList()
                    MsgBoxE(m)
                Else
                    MsgBoxE("Operation canceled")
                End If
            End If
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Error on trying to delete user / collection")
        End Try
    End Sub
    Private Sub DownloadSelectedUser(ByVal UseLimits As Boolean)
        Dim users As List(Of IUserData) = GetSelectedUserArray()
        If users.ListExists Then
            Dim l%? = Nothing
            If UseLimits Then
                Do
                    l = AConvert(Of Integer)(InputBoxE("Enter top posts limit for downloading:", "Download limit", 10), Nothing)
                    If l.HasValue Then
                        Select Case MsgBoxE(New MMessage($"You are set up downloading top [{l.Value}] posts", "Download limit",
                                            {"Confirm", "Try again", "Disable limit", "Cancel"}) With {.ButtonsPerRow = 2}).Index
                            Case 0 : Exit Do
                            Case 2 : l = Nothing
                            Case 3 : GoTo CancelDownloadingOperation
                        End Select
                    Else
                        Select Case MsgBoxE({"You are not set up downloading limit", "Download limit"},,,, {"Confirm", "Try again", "Cancel"}).Index
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
            If users.Count = 1 Then
                users(0).DownloadTopCount = l
                Downloader.Add(users(0))
            Else
                Dim uStr$ = users.Select(Function(u) u.ToString()).ListToString(, vbNewLine)
                If MsgBoxE({$"You are select {users.Count} users' profiles{vbNewLine}Do you want to download all of them?{vbNewLine.StringDup(2)}" &
                            $"Selected users:{vbNewLine}{uStr}", "A few users selected"},
                           MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    users.ForEach(Sub(u) u.DownloadTopCount = l)
                    Downloader.AddRange(users)
                End If
            End If
        End If
    End Sub
    Private Sub OpenFolder()
        Dim user As IUserData = GetSelectedUser()
        If Not user Is Nothing Then user.OpenFolder()
    End Sub
#End Region
    Friend Sub User_OnPictureUpdated(ByVal User As IUserData)
        UserListUpdate(User, False)
    End Sub
    Private _LogVisible As Boolean = False
    Private Sub Downloader_UpdateJobsCount(ByVal TotalCount As Integer)
        Dim a As Action = Sub() LBL_JOBS_COUNT.Text = IIf(TotalCount = 0, String.Empty, $"[Jobs {TotalCount}]")
        If Toolbar_BOTTOM.InvokeRequired Then Toolbar_BOTTOM.Invoke(a) Else a.Invoke
        If Not _LogVisible AndAlso Not MyMainLOG.IsEmptyString Then
            a = Sub() BTT_LOG.ControlChangeColor(False)
            If Toolbar_TOP.InvokeRequired Then Toolbar_TOP.Invoke(a) Else a.Invoke
            _LogVisible = True
        End If
    End Sub
    Private Sub Downloader_OnDownloading(ByVal Value As Boolean)
        Dim a As Action = Sub() BTT_DOWN_STOP.Enabled = Not Value
        If Toolbar_TOP.InvokeRequired Then Toolbar_TOP.Invoke(a) Else a.Invoke
    End Sub
End Class