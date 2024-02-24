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
Imports SCrawler.API.Base
Imports SCrawler.API.Reddit
Imports SCrawler.Plugin.Hosts
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Tools
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Imports RButton = PersonalUtilities.Forms.Toolbars.RangeSwitcherToolbar.ControlItem
Friend Class ChannelViewForm : Implements IChannelLimits
#Region "Events"
    Friend Event OnUsersAdded As UsersAddedEventHandler
    Friend Event OnDownloadDone As NotificationEventHandler
#End Region
#Region "Appended user structure"
    Private Structure PendingUser
        Friend ID As String
        Friend File As SFile
        Friend Channel As Channel
        Friend Sub New(ByVal _ID As String, ByRef _Channel As Channel, Optional ByVal _File As SFile = Nothing)
            ID = _ID
            Channel = _Channel
            If Settings.FromChannelCopyImageToUser Then File = _File
        End Sub
        Public Shared Widening Operator CType(ByVal _ID As String) As PendingUser
            Return New PendingUser(_ID, Nothing)
        End Operator
        Public Shared Widening Operator CType(ByVal u As PendingUser) As String
            Return u.ToString
        End Operator
        Friend Sub ChannelUserAdded(Optional ByVal IsAdded As Boolean = True)
            If Not Channel Is Nothing Then Channel.UserAdded(ID, IsAdded)
        End Sub
        Public Overrides Function ToString() As String
            Return ID
        End Function
        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            Return Obj.ToString = ID
        End Function
    End Structure
#End Region
#Region "Declarations"
    Private ReadOnly MyDefs As DefaultFormOptions
#Region "Controls"
    Private WithEvents CMB_CHANNELS As ComboBoxExtended
    Private WithEvents CH_HIDE_EXISTS_USERS As CheckBox
    Private WithEvents TXT_LIMIT As TextBoxExtended
    Private ReadOnly LBL_LIMITS As ToolStripLabel
    Private ReadOnly LBL_LIMIT_TEXT As ToolStripLabel
    Private WithEvents OPT_LIMITS_DEFAULT As RadioButton
    Private WithEvents OPT_LIMITS_COUNT As RadioButton
    Private WithEvents OPT_LIMITS_POST As RadioButton
    Private WithEvents OPT_LIMITS_DATE As RadioButton
    Private WithEvents BTT_SHOW_STATS As ToolStripButton
#End Region
    Private ReadOnly CProvider As ANumbers
    Private ReadOnly CProgress As MyProgress
    Private ReadOnly LimitProvider As ADateTime
    Friend ReadOnly Property ImagesInRow As Integer
        Get
            Return Settings.ChannelsImagesColumns.Value
        End Get
    End Property
    Friend ReadOnly Property ImagesRows As Integer
        Get
            Return Settings.ChannelsImagesRows.Value
        End Get
    End Property
#Region "Limits Support"
    Private Property AutoGetLimits As Boolean Implements IChannelLimits.AutoGetLimits
        Get
            Return OPT_LIMITS_DEFAULT.Checked
        End Get
        Set(ByVal NewLimit As Boolean)
        End Set
    End Property
    Private Property DownloadLimitCount As Integer? Implements IChannelLimits.DownloadLimitCount
        Get
            If OPT_LIMITS_COUNT.Checked Then
                Return AConvert(Of Integer)(TXT_LIMIT.Text, AModes.Var, Nothing)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal NewLimit As Integer?)
        End Set
    End Property
    Private Property DownloadLimitPost As String Implements IChannelLimits.DownloadLimitPost
        Get
            If OPT_LIMITS_POST.Checked Then
                Return TXT_LIMIT.Text
            Else
                Return String.Empty
            End If
        End Get
        Set(ByVal NewLimit As String)
        End Set
    End Property
    Private Property DownloadLimitDate As Date? Implements IChannelLimits.DownloadLimitDate
        Get
            If OPT_LIMITS_DATE.Checked Then
                Return AConvert(Of Date)(TXT_LIMIT.Value, AModes.Var, Nothing)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal NewDate As Date?)
        End Set
    End Property
    Private Sub SetLimit(Optional ByVal Post As String = "", Optional ByVal Count As Integer? = Nothing,
                         Optional ByVal [Date] As Date? = Nothing) Implements IChannelLimits.SetLimit
    End Sub
    Private Sub SetLimit(ByVal Source As IChannelLimits) Implements IChannelLimits.SetLimit
    End Sub
#End Region
    Private ReadOnly HOST_COLLECTION As SettingsHostCollection
    Private ReadOnly PendingUsers As List(Of PendingUser)
    Private ReadOnly LNC As New ListAddParams(LAP.NotContainsOnly)
    Private WithEvents MyRange As RangeSwitcherToolbar(Of UserPost)
    Private ReadOnly SelectorExpression As Predicate(Of UserPost) = Function(ByVal Post As UserPost) As Boolean
                                                                        If Post.UserID.ToLower = "[deleted]" Or Settings.BlackList.Contains(Post.UserID) Then
                                                                            Return False
                                                                        Else
                                                                            If CH_HIDE_EXISTS_USERS.Checked Then
                                                                                Return Not Settings.UsersList.Exists(Function(u) u.Name = Post.UserID)
                                                                            Else
                                                                                Return True
                                                                            End If
                                                                        End If
                                                                    End Function
#End Region
#Region "Initializer"
    Friend Sub New()
        InitializeComponent()
        MyDefs = New DefaultFormOptions
        CProgress = New MyProgress(ToolbarBOTTOM, PR_CN, LBL_STATUS, "Downloading data") With {.PerformMod = 10, .ResetProgressOnMaximumChanges = False}
        CProvider = New ANumbers With {.FormatOptions = ANumbers.Options.GroupIntegral}
        LimitProvider = New ADateTime("dd.MM.yyyy HH:mm")
        PendingUsers = New List(Of PendingUser)
        HOST_COLLECTION = Settings(RedditSiteKey)

        CMB_CHANNELS = New ComboBoxExtended With {
            .CaptionMode = ICaptionControl.Modes.CheckBox,
            .CaptionText = "All Channels",
            .Margin = New Padding(3),
            .ChangeControlsEnableOnCheckedChange = False,
            .CaptionBackColor = Color.Transparent,
            .ListMaxDropDownItems = 15,
            .CaptionPadding = New Padding(0, 3, 0, 0)
        }
        CMB_CHANNELS.Buttons.AddRange({ADB.Refresh, ADB.Add, ADB.Delete,
                                       New ActionButton(ADB.Up) With {.ToolTipText = "Previous item (F1)"},
                                       New ActionButton(ADB.Down) With {.ToolTipText = "Next item (F4)"},
                                       ADB.Edit, ADB.Info})
        TXT_LIMIT = New TextBoxExtended With {
            .CaptionText = "Limit",
            .Margin = New Padding(2),
            .CaptionSizeType = SizeType.Absolute,
            .CaptionWidth = 50,
            .CaptionBackColor = Color.Transparent,
            .TextBoxWidthMinimal = 200,
            .Width = 200,
            .CaptionPadding = New Padding(0, 3, 0, 0)
        }
        LBL_LIMITS = New ToolStripLabel With {.Text = "Limits:", .Margin = New Padding(2)}
        LBL_LIMIT_TEXT = New ToolStripLabel With {.Text = String.Empty, .Margin = New Padding(2)}
        OPT_LIMITS_DEFAULT = New RadioButton With {.Text = "Default", .BackColor = Color.Transparent, .Margin = New Padding(2)}
        OPT_LIMITS_COUNT = New RadioButton With {.Text = "Count", .BackColor = Color.Transparent, .Margin = New Padding(2)}
        OPT_LIMITS_POST = New RadioButton With {.Text = "Post", .BackColor = Color.Transparent, .Margin = New Padding(2)}
        OPT_LIMITS_DATE = New RadioButton With {.Text = "Date", .BackColor = Color.Transparent, .Margin = New Padding(2)}
        CH_HIDE_EXISTS_USERS = New CheckBox With {.Text = "Hide exists users", .BackColor = Color.Transparent, .Margin = New Padding(2),
                                                  .Checked = Settings.ChannelsHideExistsUser}
        BTT_SHOW_STATS = New ToolStripButton With {.Text = "Info", .Image = PersonalUtilities.My.Resources.InfoPic_32,
                                                   .DisplayStyle = ToolStripItemDisplayStyle.ImageAndText, .Alignment = ToolStripItemAlignment.Right,
                                                   .AutoToolTip = True, .ToolTipText = "Show channels statistic"}

        TT_MAIN.SetToolTip(CH_HIDE_EXISTS_USERS, "Hide users which already exists in collection")
        TT_MAIN.SetToolTip(OPT_LIMITS_COUNT, "Total posts count limit")
        TT_MAIN.SetToolTip(OPT_LIMITS_POST, "Looking limit till post(-s) (comma separated)")
        MyRange = New RangeSwitcherToolbar(Of UserPost)(ToolbarTOP)
        With MyRange
            .Switcher = New RangeSwitcher(Of UserPost) With {.Selector = SelectorExpression}
            .Buttons = {RButton.First, RButton.Previous, RButton.Label, RButton.Next, RButton.Last, RButton.Separator}
            .AutoToolTip = True
            .ButtonKey(RButton.Previous) = Keys.F2
            .ButtonKey(RButton.Next) = Keys.F3
            .LabelNumbersProvider = CProvider
            .Limit = ImagesInRow * ImagesRows
            .AddThisToolbar()
        End With
        ToolbarTOP.Items.AddRange({CMB_CHANNELS.GetControlHost,
                                  New ToolStripSeparator,
                                  LBL_LIMITS,
                                  New ToolStripControlHost(OPT_LIMITS_DEFAULT),
                                  New ToolStripControlHost(OPT_LIMITS_COUNT),
                                  New ToolStripControlHost(OPT_LIMITS_POST),
                                  New ToolStripControlHost(OPT_LIMITS_DATE),
                                  TXT_LIMIT.GetControlHost,
                                  LBL_LIMIT_TEXT,
                                  New ToolStripSeparator,
                                  New ToolStripControlHost(CH_HIDE_EXISTS_USERS),
                                  BTT_SHOW_STATS})
        AddHandler Settings.ChannelsImagesColumns.ValueChanged, AddressOf ImagesCountChanged
        AddHandler Settings.ChannelsImagesRows.ValueChanged, AddressOf ImagesCountChanged
    End Sub
#End Region
#Region "Form handlers"
    Private Sub ChannelViewForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        MyDefs.MyViewInitialize(Me, Settings.Design)
        RefillChannels(Settings.LatestSelectedChannel.Value)
        ChangeComboIndex(0)
        MyRange.LabelText = String.Empty
        CMB_CHANNELS_ActionOnCheckedChange(Nothing, Nothing, CMB_CHANNELS.Checked)
        With LIST_POSTS
            Dim s As Size = GetImageSize()
            .LargeImageList = New ImageList With {.ColorDepth = ColorDepth.Depth32Bit, .ImageSize = s}
            .SmallImageList = New ImageList With {.ColorDepth = ColorDepth.Depth32Bit, .ImageSize = s}
        End With
        CMB_CHANNELS.Enabled(False) = Not CMB_CHANNELS.Checked
        MyDefs.DelegateClosingChecker = False
        MyDefs.EndLoaderOperations(False)
        SetLimitsByChannel(, False)
    End Sub
    Private Sub ChannelViewForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        AppendPendingUsers()
        e.Cancel = True
        Hide()
    End Sub
    Private Sub ChannelViewForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        CMB_CHANNELS.Dispose()
        CH_HIDE_EXISTS_USERS.Dispose()
        TXT_LIMIT.Dispose()
        LBL_LIMITS.Dispose()
        OPT_LIMITS_DEFAULT.Dispose()
        OPT_LIMITS_COUNT.Dispose()
        OPT_LIMITS_POST.Dispose()
        LBL_LIMIT_TEXT.Dispose()
        BTT_SHOW_STATS.Dispose()
        MyRange.Dispose()
        PendingUsers.Clear()
        MyDefs.Dispose()
    End Sub
    Private Sub ChannelViewForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown, CMB_CHANNELS.KeyDown
        Dim b As Boolean = True
        If Not Working Then
            Select Case e.KeyCode
                Case Keys.Insert : AddNewChannel()
                Case Keys.F1 : ChangeComboIndex(-1)
                Case Keys.F4 : ChangeComboIndex(1)
                Case Keys.F5 : BTT_DOWNLOAD.PerformClick()
                Case Keys.F8 : BTT_ADD_USERS.PerformClick()
                Case Else : b = False
            End Select
        Else
            b = False
            If e.KeyCode = Keys.F8 Then BTT_ADD_USERS.PerformClick() : b = True
        End If
        If b Then LIST_POSTS.Select() : e.Handled = True
    End Sub
#End Region
#Region "Refill"
    Private Sub RefillChannels(Optional ByVal SelectedChannel As String = Nothing)
        CMB_CHANNELS.BeginUpdate()
        Dim indx%? = Nothing
        Dim t$ = If(SelectedChannel.IsEmptyString, CMB_CHANNELS.Text, SelectedChannel)
        CMB_CHANNELS.Clear(ComboBoxExtended.ClearMode.Items + ComboBoxExtended.ClearMode.Text)
        If Settings.Channels Is Nothing Then Settings.Channels = New ChannelsCollection : Settings.Channels.Load()
        With Settings.Channels
            If .Count > 0 Then
                For i% = 0 To .Count - 1
                    CMB_CHANNELS.Items.Add(.Item(i).ID)
                    If .Item(i).ID = t Then indx = i
                Next
                If indx.HasValue AndAlso indx.Value.ValueBetween(0, CMB_CHANNELS.Count - 1) Then CMB_CHANNELS.SelectedIndex = indx
            End If
        End With
        CMB_CHANNELS.EndUpdate()
    End Sub
#End Region
#Region "User add, append"
    Private Sub AppendPendingUsers()
        If LIST_POSTS.CheckedIndices.Count > 0 Then
            Dim c As Channel = GetCurrentChannel(False)
            Dim lp As New ListAddParams(LAP.NotContainsOnly) With {.OnProcessAction = Sub(ByVal u As PendingUser) u.ChannelUserAdded()}
            PendingUsers.ListAddList((From p As ListViewItem In LIST_POSTS.Items
                                      Where p.Checked
                                      Select New PendingUser(p.Text, c, GetPostBySelected(CStr(p.Tag)).CachedFile)), lp)
            Dim a As Action = Sub() BTT_ADD_USERS.Text = $"Add ({PendingUsers.Count.ToString(CProvider)})"
            If ToolbarTOP.InvokeRequired Then ToolbarTOP.Invoke(a) Else a.Invoke
        End If
    End Sub
    Private Sub BTT_ADD_USERS_Click(sender As Object, e As EventArgs) Handles BTT_ADD_USERS.Click
        AppendPendingUsers()
        Dim i%
        If LIST_POSTS.CheckedItems.Count > 0 Then
            For i = 0 To LIST_POSTS.Items.Count - 1
                If LIST_POSTS.Items(i).Checked Then LIST_POSTS.Items(i).Checked = False
            Next
        End If
        If PendingUsers.Count > 0 Then
            Dim Added% = 0, Skipped% = 0
            Dim StartIndex% = Settings.Users.Count
            Dim f As SFile
            Dim umo As Boolean
            Settings.Labels.Add(UserData.CannelsLabelName_ChannelsForm)
            Settings.Labels.Add(LabelsKeeper.NoParsedUser)
            Dim rUsers$() = UserBanned(PendingUsers.Select(Function(u) u.ID).ToArray)
            If rUsers.ListExists Then PendingUsers.RemoveAll(Function(u) rUsers.Contains(u))
            If PendingUsers.Count > 0 Then
                Dim c As New ListAddParams(LAP.NotContainsOnly)
                Dim cn$
                Dim tmpUser As IUserData
                Dim h As SettingsHost
                With PendingUsers.Select(Function(u) New UserInfo(u, If(u.Channel?.HOST, HOST_COLLECTION.Default)))
                    For i = 0 To .Count - 1
                        If Not Settings.UsersList.Contains(.ElementAt(i)) Then
                            f = PendingUsers(i).File
                            If Not PendingUsers(i).Channel Is Nothing Then
                                cn = PendingUsers(i).Channel.Name
                                umo = PendingUsers(i).Channel.HOST.GetUserMediaOnly
                                h = PendingUsers(i).Channel.HOST
                            Else
                                cn = String.Empty
                                umo = True
                                h = Nothing
                            End If
                            If h Is Nothing Then h = HOST_COLLECTION.Default
                            Settings.UpdateUsersList(.ElementAt(i))
                            tmpUser = h.GetInstance(Plugin.ISiteSettings.Download.Main, .ElementAt(i), False)
                            With DirectCast(tmpUser, UserData)
                                .Temporary = Settings.ChannelsDefaultTemporary
                                .CreatedByChannel = True
                                .ReadyForDownload = Settings.ChannelsDefaultReadyForDownload
                                .ParseUserMediaOnly = umo
                            End With
                            Settings.Users.Add(tmpUser)
                            With Settings.Users.Last
                                .Labels.ListAddList({UserData.CannelsLabelName_ChannelsForm, LabelsKeeper.NoParsedUser})
                                .UpdateUserInformation()
                                If Settings.FromChannelCopyImageToUser And Not f.IsEmptyString And Not .File.IsEmptyString Then _
                                   CopyFile(ListAddValue(Nothing, New ChannelsCollection.ChannelImage(cn, f)).ListAddList(Settings.Channels.GetUserFiles(.Name), c), .File)
                            End With
                            Added += 1
                        Else
                            Skipped += 1
                        End If
                    Next
                End With
            End If
            PendingUsers.Clear()
            BTT_ADD_USERS.Text = "Add"
            MsgBoxE($"Added users: {Added.ToString(CProvider)}{vbCr}Skipped users: {Skipped.ToString(CProvider)}{vbCr}Total: {PendingUsers.Count.ToString(CProvider)}")
            RaiseEvent OnUsersAdded(StartIndex)
            Settings.Channels.UpdateUsersStats()
        Else
            MsgBoxE("No user has been selected to add to the collection")
        End If
    End Sub
    Private Sub CopyFile(ByVal Source As IEnumerable(Of ChannelsCollection.ChannelImage), ByVal Destination As SFile)
        Try
            If Source.ListExists And Not Destination.IsEmptyString Then
                Destination = Destination.CutPath.PathWithSeparator & "ChannelImage\"
                Dim f As SFile
                Dim i% = 0
                If Destination.Exists(SFO.Path) Then
                    For Each ff As ChannelsCollection.ChannelImage In Source
                        f = Destination
                        f.Extension = ff.File.Extension
                        f.Name = $"{IIf(i = 0, "!", String.Empty)}{ff.Channel}_{ff.File.Name}"
                        If ff.File.Exists Then IO.File.Copy(ff.File, f)
                        i += 1
                    Next
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
#End Region
#Region "List images"
    Friend Function GetImageSize() As Size
        Const mhw% = 256
        Dim s As Size = LIST_POSTS.Size
        With LIST_POSTS
            s.Width -= .Margin.Horizontal
            s.Height -= .Margin.Vertical
            s.Width = s.Width / ImagesInRow - .Padding.Left * ImagesInRow - .Padding.Right * ImagesInRow
            s.Height = s.Height / ImagesRows - .Padding.Top * ImagesRows - .Padding.Bottom * ImagesRows
            If s.Width = 0 Then s.Width = 50
            If s.Height = 0 Then s.Height = 100
            If s.Width >= mhw Then
                s.Width = mhw / 100 * 75
                s.Height = mhw
            Else
                s.Height = s.Width / 75 * 100
                If s.Height > mhw Then s.Height = mhw
            End If
        End With
        Return s
    End Function
    Private Sub ImagesCountChanged(ByVal Sender As Object, ByVal e As EventArgs)
        AppendPendingUsers()
        MyRange.Limit = ImagesInRow * ImagesRows
        MyRange.GoTo(0)
    End Sub
#End Region
#Region "Downloader"
    Private TokenSource As CancellationTokenSource
    Private Token As CancellationToken
    Friend ReadOnly Property Working As Boolean
        Get
            Return Not TokenSource Is Nothing
        End Get
    End Property
    Private _ShowCancelNotification As Boolean = True
    Private _CollectionDownloading As Boolean = False
    Private Async Sub BTT_DOWNLOAD_Click(sender As Object, e As EventArgs) Handles BTT_DOWNLOAD.Click
        Try
            AppendPendingUsers()
            Dim c As Channel
            If CMB_CHANNELS.Count > 0 Then
                Dim hList As IEnumerable(Of String) = Nothing
                If CMB_CHANNELS.Checked Then
                    hList = Settings.Channels.Select(Function(cc) cc.RedditAccount.IfNullOrEmpty(SettingsHost.NameAccountNameDefault)).Distinct
                Else
                    c = GetCurrentChannel()
                    If Not c Is Nothing Then hList = {c.RedditAccount}
                End If
                If Not TokenSource Is Nothing OrElse Not HOST_COLLECTION.Available(Plugin.ISiteSettings.Download.Main, False, False,
                                                                                   hList, Not hList Is Nothing) Then Exit Sub
            End If
            Dim InvokeToken As Action = Sub()
                                            If TokenSource Is Nothing Then
                                                CProgress.Maximum = 0
                                                CProgress.Value = 0
                                                CProgress.Visible = True
                                                TokenSource = New CancellationTokenSource
                                                Token = TokenSource.Token
                                                BTT_DOWNLOAD.Enabled = False
                                                OPT_LIMITS_DEFAULT.Enabled = False
                                                OPT_LIMITS_COUNT.Enabled = False
                                                OPT_LIMITS_POST.Enabled = False
                                                OPT_LIMITS_DATE.Enabled = False
                                                TXT_LIMIT.Enabled = False
                                                CH_HIDE_EXISTS_USERS.Enabled = False
                                                CMB_CHANNELS.Enabled(True) = False
                                                BTT_SHOW_STATS.Enabled = False
                                                MyRange.Enabled = False
                                            End If
                                        End Sub
            If CMB_CHANNELS.Count > 0 Then
                BTT_DOWNLOAD.Enabled = False
                BTT_STOP.Enabled = True
                If CMB_CHANNELS.Checked Then
                    InvokeToken.Invoke()
                    _CollectionDownloading = True
                    Settings.Channels.SetLimit(Me)
                    Await Task.Run(Sub() Settings.Channels.DownloadData(Token, CH_HIDE_EXISTS_USERS.Checked, CProgress))
                    Settings.Channels.UpdateUsersStats()
                    RaiseEvent OnDownloadDone(SettingsCLS.NotificationObjects.Channels, "All channels downloaded")
                    Token.ThrowIfCancellationRequested()
                    c = GetCurrentChannel()
                Else
                    c = GetCurrentChannel()
                    If Not c Is Nothing Then
                        InvokeToken.Invoke()
                        c.SetLimit(Me)
                        Await Task.Run(Sub() c.DownloadData(Token, CH_HIDE_EXISTS_USERS.Checked, CProgress))
                        c.UpdateUsersStats()
                        RaiseEvent OnDownloadDone(SettingsCLS.NotificationObjects.Channels, $"Channel [{c.Name}] downloaded")
                        Token.ThrowIfCancellationRequested()
                    End If
                End If
                If Not c Is Nothing Then
                    SetLimitsByChannel(c)
                    MyRange.Source = c
                End If
            Else
                MsgBoxE("No one channels detected", MsgBoxStyle.Exclamation)
            End If
        Catch aex As ArgumentException When aex.HelpLink = 1
            ErrorsDescriber.Execute(EDP.ShowAllMsg, aex)
        Catch oex As OperationCanceledException When Token.IsCancellationRequested
            Dim ee As EDP = EDP.SendToLog
            If _ShowCancelNotification Then ee += EDP.ShowMainMsg
            ErrorsDescriber.Execute(ee, oex, New MMessage("Downloading operation canceled", "Status...",, MsgBoxStyle.Exclamation))
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Channels downloading error")
        Finally
            If Not TokenSource Is Nothing AndAlso Not Settings.Channels.Downloading Then
                TokenSource = Nothing
                CProgress.Visible = False
                BTT_DOWNLOAD.Enabled = True
                BTT_STOP.Enabled = False
                _CollectionDownloading = False
                OPT_LIMITS_DEFAULT.Enabled = True
                OPT_LIMITS_COUNT.Enabled = True
                OPT_LIMITS_POST.Enabled = True
                OPT_LIMITS_DATE.Enabled = True
                TXT_LIMIT.Enabled = True
                CH_HIDE_EXISTS_USERS.Enabled = True
                CMB_CHANNELS.Enabled(True) = True
                BTT_SHOW_STATS.Enabled = True
                CMB_CHANNELS_ActionOnCheckedChange(Nothing, Nothing, CMB_CHANNELS.Checked)
                MyRange.Enabled = True
                MyRange.UpdateControls()
            End If
        End Try
    End Sub
    Private Function GetCurrentChannel(Optional ByVal ShowExclamation As Boolean = True) As Channel
        If CMB_CHANNELS.SelectedIndex >= 0 Then
            Dim ChannelID$ = CMB_CHANNELS.Value
            If Not ChannelID.IsEmptyString Then Return Settings.Channels.Find(ChannelID)
        Else
            If ShowExclamation Then MsgBoxE("No one channel selected", MsgBoxStyle.Exclamation)
        End If
        Return Nothing
    End Function
    Private Sub BTT_STOP_Click(sender As Object, e As EventArgs) Handles BTT_STOP.Click
        [Stop]()
    End Sub
    Friend Sub [Stop](Optional ByVal ShowCancelNotification As Boolean = True)
        _ShowCancelNotification = ShowCancelNotification
        If Not TokenSource Is Nothing Then TokenSource.Cancel() : BTT_STOP.Enabled = False
    End Sub
#End Region
#Region "Limits changer"
    Private Sub OPT_LIMITS_DEFAULT_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_LIMITS_DEFAULT.CheckedChanged
        If OPT_LIMITS_DEFAULT.Checked Then
            TXT_LIMIT.CheckForCompatible = False
            TXT_LIMIT.Enabled = False
            ChangeLimitMode(TextBoxExtended.ControlModes.TextBox)
        End If
    End Sub
    Private Sub OPT_LIMITS_COUNT_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_LIMITS_COUNT.CheckedChanged
        If OPT_LIMITS_COUNT.Checked Then
            TXT_LIMIT.Enabled = True
            TXT_LIMIT.CheckingType = GetType(Integer)
            TXT_LIMIT.CheckForCompatible = True
            ChangeLimitMode(TextBoxExtended.ControlModes.TextBox)
            If Not ACheck(Of Integer)(TXT_LIMIT.Text) Then TXT_LIMIT.Text = Channel.DefaultDownloadLimitCount
        End If
    End Sub
    Private Sub OPT_LIMITS_POST_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_LIMITS_POST.CheckedChanged
        If OPT_LIMITS_POST.Checked Then
            TXT_LIMIT.Enabled = True
            TXT_LIMIT.CheckForCompatible = False
            ChangeLimitMode(TextBoxExtended.ControlModes.TextBox)
        End If
    End Sub
    Private Sub OPT_LIMITS_DATE_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_LIMITS_DATE.CheckedChanged
        If OPT_LIMITS_DATE.Checked Then
            TXT_LIMIT.CheckForCompatible = False
            ChangeLimitMode(TextBoxExtended.ControlModes.DateTimePicker)
        End If
    End Sub
    Private Sub ChangeLimitMode(ByVal Mode As TextBoxExtended.ControlModes)
        If Not TXT_LIMIT.ControlMode = Mode Then TXT_LIMIT.ControlMode = Mode
    End Sub
#End Region
#Region "CMB_CHANNELS"
    Private Sub SetLimitsByChannel(Optional ByVal SelectedChannel As Channel = Nothing, Optional ByVal ShowExclamation As Boolean = True)
        LBL_STATUS.Text = String.Empty
        Dim c As Channel = If(SelectedChannel, GetCurrentChannel(ShowExclamation))
        LBL_LIMIT_TEXT.Text = String.Empty
        If Not c Is Nothing Then
            Settings.LatestSelectedChannel.Value = c.ID
            Dim d As Date?
            If c.ViewMode = IRedditView.View.New Then
                With c.PostsAll
                    If .Count > 0 Then
                        OPT_LIMITS_DEFAULT.Checked = True
                        d = .FirstOrDefault(Function(p) p.Date.HasValue).Date
                        If d.HasValue Then
                            LBL_LIMIT_TEXT.Text = $"to date {AConvert(Of String)(d, ADateTime.Formats.BaseDateTime, String.Empty)}"
                        Else
                            LBL_LIMIT_TEXT.Text = $"to post [{c.FirstOrDefault(Function(p) Not p.ID.IsEmptyString).ID}]"
                        End If
                    Else
                        OPT_LIMITS_COUNT.Checked = True
                        If TXT_LIMIT.Text.IsEmptyString Then TXT_LIMIT.Value = Channel.DefaultDownloadLimitCount
                        LBL_LIMIT_TEXT.Text = $"first {TXT_LIMIT.Text} posts"
                    End If
                End With
            Else
                OPT_LIMITS_DEFAULT.Checked = True
                d = c.LatestParsedDate
                Dim per$ = IIf(c.ViewMode = IRedditView.View.Top, c.ViewPeriod.ToString, String.Empty)
                If Not per.IsEmptyString Then per = $" ({per})"
                LBL_LIMIT_TEXT.Text = $"[{c.ViewMode}{per}] to date {AConvert(Of String)(d, ADateTime.Formats.BaseDateTime, String.Empty)}"
            End If
        End If
    End Sub
    Private Sub CMB_CHANNELS_ActionSelectedItemChanged(ByVal Sender As Object, ByVal e As EventArgs, ByVal Item As ListViewItem) Handles CMB_CHANNELS.ActionSelectedItemChanged
        SetLimitsByChannel()
        Dim c As Channel = GetCurrentChannel()
        If Not c Is Nothing Then MyRange.Source = c
    End Sub
    Private Sub CMB_CHANNELS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As EventArgs) Handles CMB_CHANNELS.ActionOnButtonClick
        Dim c As Channel
        Select Case Sender.DefaultButton
            Case ADB.Refresh : RefillChannels()
            Case ADB.Add : AddNewChannel()
            Case ADB.Delete
                Try
                    c = GetCurrentChannel()
                    If Not c Is Nothing AndAlso MsgBoxE($"Are you sure you want to delete the channel [{c}]?", vbExclamation + vbYesNo) = vbYes Then
                        Settings.Channels.Remove(c)
                        RefillChannels()
                    End If
                Catch del_ex As Exception
                    ErrorsDescriber.Execute(EDP.LogMessageValue, del_ex, "An error occurred while trying to delete a channel")
                End Try
            Case ADB.Up : ChangeComboIndex(-1)
            Case ADB.Down : ChangeComboIndex(1)
            Case ADB.Edit
                Try
                    c = GetCurrentChannel()
                    If Not c Is Nothing Then
                        Using f As New RedditViewSettingsForm(c, False)
                            f.ShowDialog()
                            If f.DialogResult = DialogResult.OK Then c.Save()
                        End Using
                    End If
                Catch edit_ex As Exception
                    ErrorsDescriber.Execute(EDP.LogMessageValue, edit_ex, "An error occurred while trying to edit a channel")
                End Try
            Case ADB.Info
                Try
                    c = GetCurrentChannel()
                    If Not c Is Nothing Then MsgBoxE({c.GetChannelStats(True), "Channel statistics"})
                Catch info_ex As Exception
                    ErrorsDescriber.Execute(EDP.LogMessageValue, info_ex, "An error occurred while trying to display channel information")
                End Try
        End Select
    End Sub
    Private Sub CMB_CHANNELS_ActionOnCheckedChange(ByVal Sender As Object, ByVal e As EventArgs, ByVal Checked As Boolean) Handles CMB_CHANNELS.ActionOnCheckedChange
        Dim OneChannel As Boolean = Not CMB_CHANNELS.Checked
        CMB_CHANNELS.Enabled(False) = OneChannel
        If OneChannel Then
            OPT_LIMITS_DEFAULT.Checked = True
            LBL_LIMIT_TEXT.Text = String.Empty
            ChangeComboIndex(0)
        Else
            CMB_CHANNELS.Button(ADB.Up).Enabled = False
            CMB_CHANNELS.Button(ADB.Down).Enabled = False
            SetLimitsByChannel()
        End If
    End Sub
    Private Sub AddNewChannel()
        Const MsgTitle$ = "Add channel"
        Dim c$ = InputBoxE("Enter Reddit channel ID:", "New channel")
        If Not c.IsEmptyString Then
            Dim cc As New Channel With {.Name = c, .ID = c}
            If Settings.Channels.Count = 0 OrElse Not Settings.Channels.Contains(cc) Then
                Settings.Channels.Add(cc)
                Settings.Channels.Last.Save()
                RefillChannels()
                MsgBoxE({$"Channel [{c}] added", MsgTitle})
            Else
                MsgBoxE({$"Channel [{c}] already exists", MsgTitle})
            End If
        Else
            MsgBoxE({"You didn't enter a channel name. Operation canceled.", MsgTitle}, MsgBoxStyle.Exclamation)
        End If
    End Sub
    Private Sub ChangeComboIndex(ByVal Appender As Integer)
        Try
            AppendPendingUsers()
            Dim _ComboUpEnabled As Boolean = False
            Dim _ComboDownEnabled As Boolean = False
            If CMB_CHANNELS.Count > 0 Then
                Dim i% = CMB_CHANNELS.SelectedIndex
                If i < 0 And Appender = 0 Then
                    CMB_CHANNELS.SelectedIndex = 0
                Else
                    If i < 0 Then i = 0
                    i += Appender
                    If i.ValueBetween(0, CMB_CHANNELS.Count - 1) And Not CMB_CHANNELS.SelectedIndex = i Then CMB_CHANNELS.SelectedIndex = i
                End If
                i = CMB_CHANNELS.SelectedIndex
                Dim c% = CMB_CHANNELS.Count - 1
                _ComboUpEnabled = i > 0 And c > 0
                _ComboDownEnabled = i < c And c > 0
            End If
            CMB_CHANNELS.Button(ADB.Up).Enabled = _ComboUpEnabled
            CMB_CHANNELS.Button(ADB.Down).Enabled = _ComboDownEnabled
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "ComboBox index changing")
        End Try
    End Sub
#End Region
#Region "Toolbar controls"
    Private Sub CH_HIDE_EXISTS_USERS_CheckedChanged(sender As Object, e As EventArgs) Handles CH_HIDE_EXISTS_USERS.CheckedChanged
        If Not MyDefs.Initializing Then
            Settings.ChannelsHideExistsUser.Value = CH_HIDE_EXISTS_USERS.Checked
            MyRange.Update()
        End If
    End Sub
    Private Sub BTT_SHOW_STATS_Click(sender As Object, e As EventArgs) Handles BTT_SHOW_STATS.Click
        Using f As New ChannelsStatsForm
            f.ShowDialog()
            If f.DeletedChannels > 0 Then RefillChannels()
        End Using
    End Sub
#End Region
#Region "CONTEXT"
    Private Sub BTT_C_OPEN_USER_Click(sender As Object, e As EventArgs) Handles BTT_C_OPEN_USER.Click
        Dim p As UserPost = GetPostBySelected()
        Try
            If Not p.UserID.IsEmptyString Then Process.Start($"https://www.reddit.com/user/{p.UserID}")
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, $"Error opening user by [https://www.reddit.com/user/{p.UserID}]")
        End Try
    End Sub
    Private Sub BTT_C_OPEN_POST_Click(sender As Object, e As EventArgs) Handles BTT_C_OPEN_POST.Click
        Dim p As UserPost = GetPostBySelected()
        Dim URL$ = String.Empty
        Try
            URL = $"https://www.reddit.com/r/{CMB_CHANNELS.Value}/comments/{p.ID.Split("_").Last}"
            If Not p.ID.IsEmptyString Then Process.Start(URL)
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, $"Error opening post by [{URL}]")
        End Try
    End Sub
    Private Sub BTT_C_OPEN_PICTURE_Click(sender As Object, e As EventArgs) Handles BTT_C_OPEN_PICTURE.Click
        OpenPostPicture()
    End Sub
    Private Sub BTT_C_OPEN_FOLDER_Click(sender As Object, e As EventArgs) Handles BTT_C_OPEN_FOLDER.Click
        Dim f As SFile = GetPostBySelected().CachedFile
        If Not f.IsEmptyString Then GlobalOpenPath(f, EDP.LogMessageValue)
    End Sub
    Private Sub BTT_C_REMOVE_FROM_SELECTED_Click(sender As Object, e As EventArgs) Handles BTT_C_REMOVE_FROM_SELECTED.Click
        Try
            Dim u$ = GetPostBySelected().UserID
            If Not u.IsEmptyString Then
                Dim uRemoved As Boolean = False
                Dim i% = PendingUsers.IndexOf(u)
                If i >= 0 Then
                    PendingUsers(i).ChannelUserAdded(False)
                    PendingUsers.RemoveAt(i)
                    uRemoved = True
                End If
                With LIST_POSTS
                    If .Items.Count > 0 Then
                        Dim a As Action = Sub() .Items(i).Checked = False
                        For i = 0 To .Items.Count - 1
                            If .Items(i).Text = u And .Items(i).Checked Then
                                If LIST_POSTS.InvokeRequired Then LIST_POSTS.Invoke(a) Else a.Invoke
                            End If
                        Next
                    End If
                End With
                If uRemoved Then
                    MsgBoxE($"User [{u}] has been successfully removed")
                Else
                    MsgBoxE($"User [{u}] is not added to the selected users")
                End If
                BTT_ADD_USERS.Text = $"Add ({PendingUsers.Count.ToString(CProvider)})"
            Else
                MsgBoxE("User does not selected", MsgBoxStyle.Exclamation)
            End If
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Error removing user from selected")
        End Try
    End Sub
    Private Sub BTT_C_ADD_TO_BLACKLIST_Click(sender As Object, e As EventArgs) Handles BTT_C_ADD_TO_BLACKLIST.Click
        Try
            Dim u$ = GetPostBySelected().UserID
            If Not u.IsEmptyString Then
                Dim result% = MsgBoxE(New MMessage($"Are you sure you want to add user [{u}] to the BlackList?",
                                                   "Adding user to the BlackList",
                                                   {"Add", "Add and update ranges",
                                                    "Add with the reason", "Add with the reason and update ranges",
                                                    "Remove from BlackList", "Cancel"},
                                                   MsgBoxStyle.Exclamation) With {.ButtonsPerRow = 2})
                If result < 4 Then
                    Dim reason$ = String.Empty
                    If result = 2 Or result = 3 Then reason = InputBoxE("Enter the ban reason:", "Ban reason")
                    Settings.BlackList.ListAddValue(New UserBan(u, reason), LAP.NotContainsOnly)
                    Settings.UpdateBlackList()
                    If result = 1 Or result = 3 Then MyRange.Update()
                    MsgBoxE($"User {u} was added to the BlackList")
                ElseIf result = 4 Then
                    If Settings.BlackList.Contains(u) Then
                        Settings.BlackList.Remove(u)
                        Settings.UpdateBlackList()
                        MsgBoxE($"User [{u}] was removed from the BlackList")
                    Else
                        MsgBoxE($"User [{u}] was not banned")
                    End If
                End If
            End If
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Adding user to the BlackList")
        End Try
    End Sub
#End Region
#Region "Additional functions: OpenPostPicture, GetPostBySelected"
    Private Sub OpenPostPicture()
        Dim f As SFile = GetPostBySelected().CachedFile
        If f.Exists Then f.Open() Else MsgBoxE($"Picture file [{f}] not found", MsgBoxStyle.Critical)
    End Sub
    Private Function GetPostBySelected(Optional ByVal SpecificTag As String = Nothing) As UserPost
        Dim p As UserPost = Nothing
        Try
            If LIST_POSTS.SelectedItems.Count > 0 Or Not SpecificTag.IsEmptyString Then
                Dim t$ = If(SpecificTag.IsEmptyString, LIST_POSTS.SelectedItems(0).Tag, SpecificTag)
                With Settings.Channels.Find(CMB_CHANNELS.Value)
                    If .Count > 0 Then p = .Posts.Find(Function(pp) pp.ID = t)
                End With
            End If
        Catch aex As ArgumentException When aex.HelpLink = 1
            ErrorsDescriber.Execute(EDP.LogMessageValue, aex)
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.SendToLog, ex, "Post searching error")
        End Try
        Return p
    End Function
#End Region
#Region "List handlers"
    Private Sub LIST_POSTS_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles LIST_POSTS.MouseDoubleClick
        OpenPostPicture()
    End Sub
#End Region
#Region "MyRange"
    Private ReadOnly GetListImage_Error As New ErrorsDescriber(EDP.ReturnValue)
    Private Function GetListImage(ByVal Post As UserPost, ByVal s As Size, ByVal NullArg As Image) As Image
        If Not Post.CachedFile.IsEmptyString Then
            Return If(ImageRenderer.GetImage(SFile.GetBytes(Post.CachedFile), s, GetListImage_Error), NullArg.Clone)
        Else
            Return NullArg.Clone
        End If
    End Function
    Private Sub MyRange_IndexChanged(ByVal Sender As Object, ByVal e As EventArgs) Handles MyRange.IndexChanged
        Try
            If MyDefs.Initializing Then Exit Sub
            AppendPendingUsers()
            LIST_POSTS.LargeImageList.Images.Clear()
            LIST_POSTS.Items.Clear()
            Dim p As UserPost
            With MyRange.Current(EDP.ReturnValue).ListIfNothing
                If .Count > 0 Then
                    Dim s As Size = GetImageSize()
                    Dim NullImage As Image = New Bitmap(s.Width, s.Height)
                    For i% = 0 To .Count - 1
                        p = .Item(i)
                        With p
                            LIST_POSTS.LargeImageList.Images.Add(GetListImage(p, s, NullImage))
                            LIST_POSTS.Items.Add(New ListViewItem(.UserID, i) With {.Tag = p.ID})
                            With LIST_POSTS.Items(LIST_POSTS.Items.Count - 1)
                                If PendingUsers.Contains(.Text) Then .Checked = True
                            End With
                        End With
                    Next
                End If
            End With
        Catch aex As ArgumentException When aex.HelpLink = 1
            MsgBoxE(aex.Message, MsgBoxStyle.Critical)
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex)
        End Try
    End Sub
    Private Sub MyRange_RangesChanged(ByVal Sender As IRangeSwitcherProvider, ByVal e As EventArgs) Handles MyRange.RangesChanged
        If Sender.Count > 0 Then Sender.CurrentIndex = 0
    End Sub
#End Region
End Class