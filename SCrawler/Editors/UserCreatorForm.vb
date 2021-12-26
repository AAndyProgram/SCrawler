' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
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
        Private ReadOnly _SpecPathPattern As New RegexStructure("\w:\\.*", True, False,,,,, String.Empty, EDP.ReturnValue)
        Private ReadOnly Property SpecialPath(ByVal s As Sites) As SFile
            Get
                If TXT_SPEC_FOLDER.IsEmptyString Then
                    Return Nothing
                Else
                    If Not CStr(RegexReplace(TXT_SPEC_FOLDER.Text, _SpecPathPattern)).IsEmptyString Then
                        Return $"{TXT_SPEC_FOLDER.Text}\"
                    Else
                        Return $"{Settings(s).Path.PathWithSeparator}{TXT_SPEC_FOLDER.Text}\"
                    End If

                End If
            End Get
        End Property
        Friend ReadOnly Property UserLabels As List(Of String)
        ''' <summary>Create new user</summary>
        Friend Sub New()
            InitializeComponent()
            UserLabels = New List(Of String)
            MyDef = New DefaultFormProps(Of FieldsChecker)
        End Sub
        ''' <summary>Edit exist user</summary>
        Friend Sub New(ByVal _Instance As IUserData)
            Me.New
            If Not _Instance Is Nothing Then
                UserInstance = _Instance
                User = DirectCast(UserInstance.Self, UserDataBase).User
            End If
        End Sub
        Private Sub UserCreatorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDef
                    .MyViewInitialize(Me, Settings.Design, True)
                    .AddOkCancelToolbar()
                    CH_AUTO_DETECT_SITE.Enabled = False
                    If User.Name.IsEmptyString Then
                        OPT_REDDIT.Checked = False
                        OPT_TWITTER.Checked = False
                        OPT_INSTAGRAM.Checked = False
                        CH_PARSE_USER_MEDIA.Enabled = False
                        CH_READY_FOR_DOWN.Checked = True
                        CH_TEMP.Checked = Settings.DefaultTemporary
                        CH_DOWN_IMAGES.Checked = Settings.DefaultDownloadImages
                        CH_DOWN_VIDEOS.Checked = Settings.DefaultDownloadVideos
                    Else
                        TP_ADD_BY_LIST.Enabled = False
                        TXT_USER.Text = User.Name
                        TXT_SPEC_FOLDER.Text = User.SpecialPath
                        Select Case User.Site
                            Case Sites.Reddit : OPT_REDDIT.Checked = True : CH_PARSE_USER_MEDIA.Enabled = False
                            Case Sites.Twitter : OPT_TWITTER.Checked = True
                            Case Sites.Instagram : OPT_INSTAGRAM.Checked = True
                        End Select
                        OPT_REDDIT.Enabled = False
                        OPT_TWITTER.Enabled = False
                        OPT_INSTAGRAM.Enabled = False
                        CH_IS_CHANNEL.Checked = User.IsChannel
                        CH_IS_CHANNEL.Enabled = False
                        If Not UserInstance Is Nothing Then
                            TXT_USER.Enabled = False
                            TXT_SPEC_FOLDER.TextBoxReadOnly = True
                            TXT_SPEC_FOLDER.Buttons.Clear()
                            TXT_SPEC_FOLDER.Buttons.UpdateButtonsPositions()
                            With UserInstance
                                TXT_USER_FRIENDLY.Text = .FriendlyName
                                CH_FAV.Checked = .Favorite
                                CH_TEMP.Checked = .Temporary
                                CH_PARSE_USER_MEDIA.Checked = .ParseUserMediaOnly
                                CH_READY_FOR_DOWN.Checked = .ReadyForDownload
                                CH_DOWN_IMAGES.Checked = .DownloadImages
                                CH_DOWN_VIDEOS.Checked = .DownloadVideos
                                TXT_DESCR.Text = .Description
                                UserLabels.ListAddList(.Labels)
                                If UserLabels.ListExists Then TXT_LABELS.Text = UserLabels.ListToString
                            End With
                            CH_ADD_BY_LIST.Enabled = False
                        Else
                            CH_TEMP.Checked = Settings.DefaultTemporary
                            CH_READY_FOR_DOWN.Checked = Not Settings.DefaultTemporary
                            CH_DOWN_IMAGES.Checked = Settings.DefaultDownloadImages
                            CH_DOWN_VIDEOS.Checked = Settings.DefaultDownloadVideos
                        End If
                    End If
                    .MyFieldsChecker = New FieldsChecker
                    .MyFieldsChecker.AddControl(Of String)(TXT_USER, TXT_USER.CaptionText)
                    .MyFieldsChecker.EndLoaderOperations()
                    SetParamsBySite()
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
            End If
        End Sub
        Private Sub UserCreatorForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            UserLabels.Clear()
        End Sub
        Private Function GetSiteByCheckers() As Sites
            Select Case True
                Case OPT_REDDIT.Checked : Return Sites.Reddit
                Case OPT_TWITTER.Checked : Return Sites.Twitter
                Case OPT_INSTAGRAM.Checked : Return Sites.Instagram
                Case Else : Return Sites.Undefined
            End Select
        End Function
        Private Sub ToolbarBttOK() Implements IOkCancelToolbar.ToolbarBttOK
            If Not CH_ADD_BY_LIST.Checked Then
                If MyDef.MyFieldsChecker.AllParamsOK Then
                    Dim s As Sites = GetSiteByCheckers()
                    If Not s = Sites.Undefined Then
                        Dim tmpUser As UserInfo = User.Clone
                        With tmpUser
                            .Name = TXT_USER.Text
                            .SpecialPath = SpecialPath(s)
                            .Site = s
                            .IsChannel = CH_IS_CHANNEL.Checked
                            .UpdateUserFile()
                        End With
                        User = tmpUser
                        If Not UserInstance Is Nothing Then
                            With DirectCast(UserInstance.Self, UserDataBase)
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
        Private ReadOnly RedditChannelRegEx1 As New RegexStructure("[htps:/]{7,8}.*?reddit.com/r/([^/]+)", 1)
        Private ReadOnly RedditChannelRegEx2 As New RegexStructure(".?r/([^/]+)", 1)
        Private ReadOnly InstagramRegEx As New RegexStructure("[htps:/]{7,8}.*?instagram.com/([^/]+)", 1)
        Private _TextChangeInvoked As Boolean = False
        Private Sub TXT_USER_ActionOnTextChange() Handles TXT_USER.ActionOnTextChange
            Try
                If Not _TextChangeInvoked Then
                    _TextChangeInvoked = True
                    If Not CH_ADD_BY_LIST.Checked Then
                        Dim s() As Object = GetSiteByText(TXT_USER.Text)
                        Select Case s(0)
                            Case Sites.Twitter : OPT_TWITTER.Checked = True
                            Case Sites.Reddit : OPT_REDDIT.Checked = True
                            Case Sites.Instagram : OPT_INSTAGRAM.Checked = True
                            Case Else : OPT_TWITTER.Checked = False : OPT_REDDIT.Checked = False : OPT_INSTAGRAM.Checked = False
                        End Select
                        CH_IS_CHANNEL.Checked = CBool(s(1))
                    End If
                    _TextChangeInvoked = False
                End If
            Catch ex As Exception
            End Try
        End Sub
        Private Function GetSiteByText(ByRef TXT As String) As Object()
            If Not TXT.IsEmptyString AndAlso TXT.Length > 8 Then
                If CheckRegex(TXT, TwitterRegEx) Then
                    Return {Sites.Twitter, False}
                ElseIf CheckRegex(TXT, RedditRegEx1) OrElse CheckRegex(TXT, RedditRegEx2) Then
                    Return {Sites.Reddit, False}
                ElseIf CheckRegex(TXT, RedditChannelRegEx1) OrElse CheckRegex(TXT, RedditChannelRegEx2) Then
                    Return {Sites.Reddit, True}
                ElseIf CheckRegex(TXT, InstagramRegEx) Then
                    Return {Sites.Instagram, False}
                End If
            End If
            Return {Sites.Undefined, False}
        End Function
        Private Function CheckRegex(ByRef TXT As String, ByVal r As RegexStructure) As Boolean
            Dim s$ = RegexReplace(TXT, r)
            If Not s.IsEmptyString Then TXT = s : Return True Else Return False
        End Function
        Private Sub OPT_REDDIT_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_REDDIT.CheckedChanged
            If OPT_REDDIT.Checked Then CH_IS_CHANNEL.Enabled = True : SetParamsBySite()
        End Sub
        Private Sub OPT_TWITTER_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_TWITTER.CheckedChanged
            If OPT_TWITTER.Checked Then CH_IS_CHANNEL.Checked = False : CH_IS_CHANNEL.Enabled = False : SetParamsBySite()
        End Sub
        Private Sub OPT_INSTAGRAM_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_INSTAGRAM.CheckedChanged
            If OPT_INSTAGRAM.Checked Then CH_IS_CHANNEL.Checked = False : CH_IS_CHANNEL.Enabled = False : SetParamsBySite()
        End Sub
        Private Sub TXT_SPEC_FOLDER_ActionOnButtonClick(ByVal Sender As ActionButton) Handles TXT_SPEC_FOLDER.ActionOnButtonClick
            If Sender.DefaultButton = ActionButton.DefaultButtons.Open Then
                Dim f As SFile = Nothing
                If Not TXT_SPEC_FOLDER.Text.IsEmptyString Then f = $"{TXT_SPEC_FOLDER.Text}\"
                f = SFile.SelectPath(f, True)
                If Not f.IsEmptyString Then TXT_SPEC_FOLDER.Text = f.PathWithSeparator
            End If
        End Sub
        Private Sub CH_TEMP_CheckedChanged(sender As Object, e As EventArgs) Handles CH_TEMP.CheckedChanged
            If CH_TEMP.Checked Then CH_FAV.Checked = False : CH_READY_FOR_DOWN.Checked = False
        End Sub
        Private Sub CH_FAV_CheckedChanged(sender As Object, e As EventArgs) Handles CH_FAV.CheckedChanged
            If CH_FAV.Checked Then CH_TEMP.Checked = False
        End Sub
        Private Sub SetParamsBySite()
            Dim s As Sites = GetSiteByCheckers()
            If Not s = Sites.Undefined Then
                With Settings(s)
                    CH_TEMP.Checked = .Temporary
                    CH_DOWN_IMAGES.Checked = .DownloadImages
                    CH_DOWN_VIDEOS.Checked = .DownloadVideos
                    CH_PARSE_USER_MEDIA.Checked = s = Sites.Twitter AndAlso .GetUserMediaOnly.Value
                    CH_PARSE_USER_MEDIA.Enabled = s = Sites.Twitter
                    CH_READY_FOR_DOWN.Checked = Not CH_TEMP.Checked
                End With
            End If
        End Sub
        Private Sub CH_ADD_BY_LIST_CheckedChanged(sender As Object, e As EventArgs) Handles CH_ADD_BY_LIST.CheckedChanged
            If CH_ADD_BY_LIST.Checked Then
                TXT_DESCR.GroupBoxText = "Users list"
                CH_AUTO_DETECT_SITE.Enabled = True
                CH_PARSE_USER_MEDIA.Enabled = True
            Else
                TXT_DESCR.GroupBoxText = "Description"
                CH_AUTO_DETECT_SITE.Checked = False
                CH_AUTO_DETECT_SITE.Enabled = False
                SetParamsBySite()
            End If
            TXT_USER.Enabled = Not CH_ADD_BY_LIST.Checked
            TXT_USER_FRIENDLY.Enabled = Not CH_ADD_BY_LIST.Checked
        End Sub
        Private Sub CH_AUTO_DETECT_SITE_CheckedChanged(sender As Object, e As EventArgs) Handles CH_AUTO_DETECT_SITE.CheckedChanged
            OPT_REDDIT.Enabled = Not CH_AUTO_DETECT_SITE.Checked
            OPT_TWITTER.Enabled = Not CH_AUTO_DETECT_SITE.Checked
            OPT_INSTAGRAM.Enabled = Not CH_AUTO_DETECT_SITE.Checked
            CH_IS_CHANNEL.Enabled = Not CH_AUTO_DETECT_SITE.Checked
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
                            Dim s As Sites = GetSiteByCheckers()
                            Dim sObj() As Object
                            Dim _IsChannel As Boolean = CH_IS_CHANNEL.Checked
                            Dim Added% = 0
                            Dim Skipped% = 0
                            Dim uid%
                            Dim sf As Func(Of Sites, String) = Function(__s) SpecialPath(__s).PathWithSeparator
                            Dim __sf As Func(Of String, Sites, SFile) = Function(Input, __s) IIf(sf(__s).IsEmptyString, Nothing, New SFile($"{sf(__s)}{Input}\"))

                            For i% = 0 To u.Count - 1
                                uu = u(i)
                                If CH_AUTO_DETECT_SITE.Checked Then
                                    sObj = GetSiteByText(uu)
                                    s = sObj(0)
                                    _IsChannel = CBool(sObj(1))
                                End If

                                If Not s = Sites.Undefined Then
                                    tmpUser = New UserInfo(uu, s,,, __sf(uu, s)) With {.IsChannel = _IsChannel}
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
                            If Added > 0 Then MyDef.ChangesDetected = False
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