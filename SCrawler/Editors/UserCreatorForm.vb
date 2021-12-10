Imports System.ComponentModel
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Forms.Toolbars
Imports SCrawler.API.Base
Namespace Editors
    Friend Class UserCreatorForm : Implements IOkCancelToolbar
        Private ReadOnly MyDef As DefaultFormProps(Of FieldsChecker)
        Friend Property User As UserInfo
        Friend Property UserInstance As IUserData
        Friend Property StartIndex As Integer = -1
        Friend ReadOnly Property UserTemporary As Boolean
            Get
                If CH_FAV.Checked Then
                    Return False
                Else
                    Return CH_TEMP.Checked
                End If
            End Get
        End Property
        Friend ReadOnly Property UserFavorite As Boolean
            Get
                Return CH_FAV.Checked
            End Get
        End Property
        Friend ReadOnly Property UserMediaOnly As Boolean
            Get
                If User.Site = Sites.Twitter Then
                    Return CH_PARSE_USER_MEDIA.Checked
                Else
                    Return False
                End If
            End Get
        End Property
        Friend ReadOnly Property UserReady As Boolean
            Get
                Return CH_READY_FOR_DOWN.Checked
            End Get
        End Property
        Friend ReadOnly Property DownloadImages As Boolean
            Get
                Return CH_DOWN_IMAGES.Checked
            End Get
        End Property
        Friend ReadOnly Property DownloadVideos As Boolean
            Get
                Return CH_DOWN_VIDEOS.Checked
            End Get
        End Property
        Friend ReadOnly Property UserDescr As String
            Get
                Return TXT_DESCR.Text
            End Get
        End Property
        Friend ReadOnly Property UserFriendly As String
            Get
                Return TXT_USER_FRIENDLY.Text
            End Get
        End Property
        Friend ReadOnly Property UserLabels As List(Of String)
        Friend Sub New()
            InitializeComponent()
            UserLabels = New List(Of String)
            MyDef = New DefaultFormProps(Of FieldsChecker)
        End Sub
        Friend Sub New(ByVal _User As UserInfo)
            Me.New
            User = _User
        End Sub
        Friend Sub New(ByVal _Instance As IUserData)
            Me.New
            If Not _Instance Is Nothing Then
                UserInstance = _Instance
                User = DirectCast(UserInstance, UserDataBase).User
            End If
        End Sub
        Private Sub UserCreatorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDef
                    .MyViewInitialize(Me, Settings.Design, True)
                    .AddOkCancelToolbar()
                    If User.Name.IsEmptyString Then
                        OPT_REDDIT.Checked = False
                        OPT_TWITTER.Checked = False
                        CH_PARSE_USER_MEDIA.Enabled = False
                        CH_READY_FOR_DOWN.Checked = True
                        CH_TEMP.Checked = Settings.DefaultTemporary
                        CH_DOWN_IMAGES.Checked = Settings.DefaultDownloadImages
                        CH_DOWN_VIDEOS.Checked = Settings.DefaultDownloadVideos
                    Else
                        TP_ADD_BY_LIST.Enabled = False
                        TXT_USER.Text = User.Name
                        Select Case User.Site
                            Case Sites.Reddit : OPT_REDDIT.Checked = True : CH_PARSE_USER_MEDIA.Enabled = False
                            Case Sites.Twitter : OPT_TWITTER.Checked = True
                        End Select
                        OPT_REDDIT.Enabled = False
                        OPT_TWITTER.Enabled = False
                        If Not UserInstance Is Nothing Then
                            TXT_USER.Enabled = False
                            With UserInstance
                                TXT_USER_FRIENDLY.Text = .FriendlyName
                                CH_FAV.Checked = .Favorite
                                CH_TEMP.Checked = .Temporary
                                CH_PARSE_USER_MEDIA.Checked = .ParseUserMediaOnly
                                CH_READY_FOR_DOWN.Checked = .ReadyForDownload
                                CH_DOWN_IMAGES.Checked = .DownloadImages
                                CH_DOWN_VIDEOS.Checked = .DownloadedVideos
                                TXT_DESCR.Text = .Description
                                UserLabels.ListAddList(.Labels)
                                If UserLabels.ListExists Then TXT_LABELS.Text = UserLabels.ListToString
                            End With
                        Else
                            CH_READY_FOR_DOWN.Checked = Settings.DefaultTemporary
                            CH_DOWN_IMAGES.Checked = Settings.DefaultDownloadImages
                            CH_DOWN_VIDEOS.Checked = Settings.DefaultDownloadVideos
                        End If
                    End If
                    .MyFieldsChecker = New FieldsChecker
                    .MyFieldsChecker.AddControl(Of String)(TXT_USER, TXT_USER.CaptionText)
                    .MyFieldsChecker.EndLoaderOperations()
                    .AppendDetectors()
                    .EndLoaderOperations()
                End With
            Catch ex As Exception
                MyDef.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub UserCreatorForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            If e.KeyCode = Keys.F4 Then ChangeLabels() : e.Handled = True
        End Sub
        Private Sub UserCreatorForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            If Not BeforeCloseChecker(MyDef.ChangesDetected) Then
                e.Cancel = True
            Else
                MyDef.Dispose()
                If Not DialogResult = DialogResult.OK And Not DialogResult = DialogResult.Cancel And StartIndex >= 0 Then DialogResult = DialogResult.OK
            End If
        End Sub
        Private Sub UserCreatorForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            UserLabels.Clear()
        End Sub
        Private Sub ToolbarBttOK() Implements IOkCancelToolbar.ToolbarBttOK
            If Not CH_ADD_BY_LIST.Checked Then
                If MyDef.MyFieldsChecker.AllParamsOK Then
                    If OPT_REDDIT.Checked Or OPT_TWITTER.Checked Then
                        Dim tmpUser As UserInfo = User.Clone
                        With tmpUser
                            .Name = TXT_USER.Text
                            .Site = IIf(OPT_REDDIT.Checked, Sites.Reddit, Sites.Twitter)
                            .UpdateUserFile()
                        End With
                        User = tmpUser
                        If Not UserInstance Is Nothing Then
                            With DirectCast(UserInstance, UserDataBase)
                                .User = User
                                .FriendlyName = TXT_USER_FRIENDLY.Text
                                .Favorite = CH_FAV.Checked
                                .Temporary = CH_TEMP.Checked
                                .ReadyForDownload = CH_READY_FOR_DOWN.Checked
                                .DownloadImages = CH_DOWN_IMAGES.Checked
                                .DownloadVideos = CH_DOWN_VIDEOS.Checked
                                .UserDescription = TXT_DESCR.Text
                                Dim l As New ListAddParams(LAP.NotContainsOnly + LAP.ClearBeforeAdd)
                                If .IsCollection Then
                                    With DirectCast(UserInstance, API.UserDataBind)
                                        If .Count > 0 Then .Collections.ForEach(Sub(c) c.Labels.ListAddList(UserLabels, l))
                                    End With
                                Else
                                    .Labels.ListAddList(UserLabels, LAP.NotContainsOnly, LAP.ClearBeforeAdd)
                                End If
                                If OPT_TWITTER.Checked Then .ParseUserMediaOnly = CH_PARSE_USER_MEDIA.Checked
                                .UpdateUserInformation()
                            End With
                        End If
                        GoTo CloseForm
                    Else
                        MsgBoxE("User site does not selected", MsgBoxStyle.Exclamation)
                    End If
                End If
            Else
                If CreateUsersByList() Then GoTo CloseForm
            End If
            Exit Sub
CloseForm:
            MyDef.CloseForm()
        End Sub
        Private Sub ToolbarBttCancel() Implements IOkCancelToolbar.ToolbarBttCancel
            MyDef.CloseForm(IIf(StartIndex >= 0, DialogResult.OK, DialogResult.Cancel))
        End Sub
        Private ReadOnly TwitterRegEx As New RegexStructure("[htps:/]{7,8}.*?twitter.com/([^/]+)", 1)
        Private ReadOnly RedditRegEx1 As New RegexStructure("[htps:/]{7,8}.*?reddit.com/user/([^/]+)", 1)
        Private ReadOnly RedditRegEx2 As New RegexStructure(".?u/([^/]+)", 1)
        Private _TextChangeInvoked As Boolean = False
        Private Sub TXT_USER_ActionOnTextChange() Handles TXT_USER.ActionOnTextChange
            Try
                If Not _TextChangeInvoked Then
                    _TextChangeInvoked = True
                    If Not CH_ADD_BY_LIST.Checked Then
                        Select Case GetSiteByText(TXT_USER.Text)
                            Case Sites.Twitter : OPT_TWITTER.Checked = True
                            Case Sites.Reddit : OPT_REDDIT.Checked = True
                            Case Else : OPT_TWITTER.Checked = False : OPT_REDDIT.Checked = False
                        End Select
                    End If
                    MyDef.Detector()
                    _TextChangeInvoked = False
                End If
            Catch ex As Exception
            End Try
        End Sub
        Private Function GetSiteByText(ByRef TXT As String) As Sites
            If Not TXT.IsEmptyString AndAlso TXT.Length > 8 Then
                Dim s$ = RegexReplace(TXT, TwitterRegEx)
                If Not s.IsEmptyString Then
                    TXT = s
                    Return Sites.Twitter
                Else
                    s = RegexReplace(TXT, RedditRegEx1)
                    If Not s.IsEmptyString Then
                        TXT = s
                        Return Sites.Reddit
                    Else
                        s = RegexReplace(TXT, RedditRegEx2)
                        If Not s.IsEmptyString Then
                            TXT = s
                            Return Sites.Reddit
                        End If
                    End If
                End If
            End If
            Return Sites.Undefined
        End Function
        Private Sub OPT_REDDIT_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_REDDIT.CheckedChanged
            MyDef.Detector()
        End Sub
        Private Sub OPT_TWITTER_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_TWITTER.CheckedChanged
            MyDef.Detector()
            CH_PARSE_USER_MEDIA.Enabled = OPT_TWITTER.Checked
        End Sub
        Private Sub CH_TEMP_CheckedChanged(sender As Object, e As EventArgs) Handles CH_TEMP.CheckedChanged
            If CH_TEMP.Checked Then CH_FAV.Checked = False
            MyDef.Detector()
        End Sub
        Private Sub CH_FAV_CheckedChanged(sender As Object, e As EventArgs) Handles CH_FAV.CheckedChanged
            If CH_FAV.Checked Then CH_TEMP.Checked = False
            MyDef.Detector()
        End Sub
        Private Sub CH_READY_FOR_DOWN_CheckedChanged(sender As Object, e As EventArgs) Handles CH_READY_FOR_DOWN.CheckedChanged
            MyDef.Detector()
        End Sub
        Private Sub CH_PARSE_USER_MADIA_CheckedChanged(sender As Object, e As EventArgs) Handles CH_PARSE_USER_MEDIA.CheckedChanged
            MyDef.Detector()
        End Sub
        Private Sub CH_ADD_BY_LIST_CheckedChanged(sender As Object, e As EventArgs) Handles CH_ADD_BY_LIST.CheckedChanged
            If CH_ADD_BY_LIST.Checked Then
                TXT_DESCR.GroupBoxText = "Users list"
            Else
                TXT_DESCR.GroupBoxText = "Description"
            End If
            TXT_USER.Enabled = Not CH_ADD_BY_LIST.Checked
            TXT_USER_FRIENDLY.Enabled = Not CH_ADD_BY_LIST.Checked
        End Sub
        Private Sub CH_AUTO_DETECT_SITE_CheckedChanged(sender As Object, e As EventArgs) Handles CH_AUTO_DETECT_SITE.CheckedChanged
            OPT_REDDIT.Enabled = Not CH_AUTO_DETECT_SITE.Checked
            OPT_TWITTER.Enabled = Not CH_AUTO_DETECT_SITE.Checked
        End Sub
        Private Function CreateUsersByList() As Boolean
            Try
                If CH_ADD_BY_LIST.Checked Then
                    If Not TXT_DESCR.IsEmptyString Then
                        Dim u As List(Of String) = TXT_DESCR.Text.StringToList(Of String, List(Of String))(vbNewLine).ListForEach(Function(s, ii) s.Trim,, False)
                        If u.ListExists Then
                            Dim NonIdentified As New List(Of String)
                            Dim UsersForCreate As New List(Of UserInfo)
                            Dim BannedUsers() As String = Nothing
                            Dim uu$
                            Dim tmpUser As UserInfo
                            Dim s As Sites
                            Dim Added% = 0
                            Dim Skipped% = 0
                            Dim uid%

                            Select Case True
                                Case OPT_REDDIT.Checked : s = Sites.Reddit
                                Case OPT_TWITTER.Checked : s = Sites.Twitter
                                Case Else : s = Sites.Undefined
                            End Select

                            For i% = 0 To u.Count - 1
                                uu = u(i)
                                If CH_AUTO_DETECT_SITE.Checked Then s = GetSiteByText(uu)

                                If Not s = Sites.Undefined Then
                                    tmpUser = New UserInfo(uu, s)
                                    uid = -1
                                    If Settings.UsersList.Count > 0 Then uid = Settings.UsersList.IndexOf(tmpUser)
                                    If uid < 0 And Not UsersForCreate.Contains(tmpUser) Then
                                        UsersForCreate.Add(tmpUser)
                                    Else
                                        Skipped += 1
                                    End If
                                Else
                                    NonIdentified.Add(u(i))
                                End If
                            Next

                            If UsersForCreate.Count > 0 Then
                                BannedUsers = UserBanned(UsersForCreate.Select(Function(uuu) uuu.Name).ToArray)
                                If BannedUsers.ListExists Then UsersForCreate.RemoveAll(Function(uuu) BannedUsers.Contains(uuu.Name))
                                If UsersForCreate.Count > 0 Then
                                    For Each tmpUser In UsersForCreate
                                        Settings.UpdateUsersList(tmpUser)
                                        If StartIndex = -1 Then StartIndex = Settings.Users.Count
                                        Settings.Users.Add(UserDataBase.GetInstance(tmpUser, False))
                                        With Settings.Users.Last
                                            .FriendlyName = TXT_USER_FRIENDLY.Text
                                            .Favorite = CH_FAV.Checked
                                            .Temporary = CH_TEMP.Checked
                                            .ReadyForDownload = CH_READY_FOR_DOWN.Checked
                                            .DownloadImages = CH_DOWN_IMAGES.Checked
                                            .DownloadVideos = CH_DOWN_VIDEOS.Checked
                                            .Labels.ListAddList(UserLabels)
                                            If s = Sites.Twitter Then .ParseUserMediaOnly = CH_PARSE_USER_MEDIA.Checked
                                            .UpdateUserInformation()
                                        End With
                                        Added += 1
                                    Next
                                End If
                            End If

                            Dim m As New MMessage($"Added {Added} users (skipped (already exists and/or duplicated) {Skipped})")
                            If BannedUsers.ListExists Or NonIdentified.Count > 0 Then
                                Dim t$ = String.Empty
                                If BannedUsers.ListExists Then t.StringAppendLine($"Banned users:{vbNewLine}{BannedUsers.ListToString(, vbNewLine)}")
                                If NonIdentified.Count > 0 Then t.StringAppendLine($"Non-Identified users:{vbNewLine}{NonIdentified.ListToString(, vbNewLine)}", vbNewLine.StringDup(2))
                                m.Style = MsgBoxStyle.Exclamation
                                m.Text.StringAppendLine("Some of users does not recognized and/or banned")
                                m.Text.StringAppendLine(t, vbNewLine.StringDup(2))
                                TXT_DESCR.Text = t
                            Else
                                TXT_DESCR.Clear()
                            End If

                            MsgBoxE(m)
                            Return Added > 0 And Not BannedUsers.ListExists And NonIdentified.Count = 0
                        Else
                            MsgBoxE("No one user can not recognized", MsgBoxStyle.Exclamation)
                        End If
                    Else
                        MsgBoxE("[Users list] is empty", MsgBoxStyle.Critical)
                    End If
                End If
                Return False
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Error on adding users by list", False)
            End Try
        End Function
        Private Sub TXT_LABELS_ActionOnButtonClick(ByVal Sender As ActionButton) Handles TXT_LABELS.ActionOnButtonClick
            Select Case Sender.DefaultButton
                Case ActionButton.DefaultButtons.Open : ChangeLabels()
                Case ActionButton.DefaultButtons.Clear : UserLabels.Clear()
            End Select
        End Sub
        Private Sub ChangeLabels()
            Using fl As New LabelsForm(UserLabels)
                fl.ShowDialog()
                If fl.DialogResult = DialogResult.OK Then
                    UserLabels.ListAddList(fl.LabelsList, LAP.NotContainsOnly, LAP.ClearBeforeAdd)
                    If UserLabels.ListExists Then
                        TXT_LABELS.Text = UserLabels.ListToString
                    Else
                        TXT_LABELS.Clear()
                    End If
                End If
            End Using
        End Sub
    End Class
End Namespace