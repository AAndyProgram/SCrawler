' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Forms.Toolbars
Namespace Editors
    Friend Class GlobalSettingsForm : Implements IOkCancelToolbar
        Private ReadOnly MyDefs As DefaultFormProps(Of FieldsChecker)
#Region "Checkers declarations"
        Private Class SavedPostsChecker : Implements ICustomProvider
            Private Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                     Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object Implements ICustomProvider.Convert
                If Not ACheck(Value) OrElse CStr(Value).Contains("/") Then
                    Return Nothing
                Else
                    Return Value
                End If
            End Function
            Private Function GetFormat(ByVal FormatType As Type) As Object Implements IFormatProvider.GetFormat
                Throw New NotImplementedException()
            End Function
        End Class
        Private Class InstaTimersChecker : Implements ICustomProvider
            Private ReadOnly _LowestValue As Integer
            Friend Sub New(ByVal LowestValue As Integer)
                _LowestValue = LowestValue
            End Sub
            Private Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                     Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object Implements ICustomProvider.Convert
                If ACheck(Of Integer)(Value) AndAlso CInt(Value) >= _LowestValue Then
                    Return Value
                Else
                    Return Nothing
                End If
            End Function
            Private Function GetFormat(ByVal FormatType As Type) As Object Implements IFormatProvider.GetFormat
                Throw New NotImplementedException()
            End Function
        End Class
#End Region
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormProps(Of FieldsChecker)
        End Sub
        Private Sub GlobalSettingsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDefs
                    .MyViewInitialize(Me, Settings.Design, True)
                    .AddOkCancelToolbar()
                    .DelegateClosingChecker()
                    With Settings
                        'Basis
                        TXT_GLOBAL_PATH.Text = .GlobalPath.Value
                        TXT_IMAGE_LARGE.Value = .MaxLargeImageHeigh.Value
                        TXT_IMAGE_SMALL.Value = .MaxSmallImageHeigh.Value
                        TXT_COLLECTIONS_PATH.Text = .CollectionsPath
                        TXT_MAX_JOBS_USERS.Value = .MaxUsersJobsCount.Value
                        TXT_MAX_JOBS_CHANNELS.Value = .ChannelsMaxJobsCount.Value
                        CH_CHECK_VER_START.Checked = .CheckUpdatesAtStart
                        TXT_IMGUR_CLIENT_ID.Text = .ImgurClientID
                        'Defaults
                        CH_SEPARATE_VIDEO_FOLDER.Checked = .SeparateVideoFolder.Value
                        CH_DEF_TEMP.Checked = .DefaultTemporary
                        CH_DOWN_IMAGES.Checked = .DefaultDownloadImages
                        CH_DOWN_VIDEOS.Checked = .DefaultDownloadVideos
                        'Channels
                        TXT_CHANNELS_ROWS.Value = .ChannelsImagesRows.Value
                        TXT_CHANNELS_COLUMNS.Value = .ChannelsImagesColumns.Value
                        TXT_CHANNEL_USER_POST_LIMIT.Value = .FromChannelDownloadTop.Value
                        TXT_CHANNEL_USER_POST_LIMIT.Checked = .FromChannelDownloadTopUse.Value
                        CH_COPY_CHANNEL_USER_IMAGE.Checked = .FromChannelCopyImageToUser
                        CH_CHANNELS_USERS_TEMP.Checked = .ChannelsDefaultTemporary
                        'Channels filenames
                        CH_FILE_NAME_CHANGE.Checked = .FileReplaceNameByDate Or .FileAddDateToFileName Or .FileAddTimeToFileName
                        OPT_FILE_NAME_REPLACE.Checked = .FileReplaceNameByDate
                        OPT_FILE_NAME_ADD_DATE.Checked = Not .FileReplaceNameByDate
                        CH_FILE_DATE.Checked = .FileAddDateToFileName
                        CH_FILE_TIME.Checked = .FileAddTimeToFileName
                        OPT_FILE_DATE_START.Checked = Not .FileDateTimePositionEnd
                        OPT_FILE_DATE_END.Checked = .FileDateTimePositionEnd
                        'Other program settings
                        CH_EXIT_CONFIRM.Checked = .ExitConfirm
                        CH_CLOSE_TO_TRAY.Checked = .CloseToTray
                        CH_SHOW_NOTIFY.Checked = .ShowNotifications
                        'Reddit
                        With .Site(Sites.Reddit)
                            SetChecker(DEFS_REDDIT, Sites.Reddit)
                            TXT_REDDIT_SAVED_POSTS_USER.Text = .SavedPostsUserName
                        End With
                        'Twitter
                        With .Site(Sites.Twitter)
                            SetChecker(DEFS_TWITTER, Sites.Twitter)
                            CH_TWITTER_USER_MEDIA.Checked = .GetUserMediaOnly
                        End With
                        'Instagram
                        With .Site(Sites.Instagram)
                            SetChecker(DEFS_INST, Sites.Instagram)
                            TXT_REQ_WAIT_TIMER.Text = .RequestsWaitTimer
                            TXT_REQ_COUNT.Text = .RequestsWaitTimerTaskCount
                            TXT_LIMIT_TIMER.Text = .SleepTimerOnPostsLimit
                            TXT_INST_SAVED_POSTS_USER.Text = .SavedPostsUserName
                        End With
                        'RedGifs
                        SetChecker(DEFS_REDGIFS, Sites.RedGifs)
                    End With
                    .MyFieldsChecker = New FieldsChecker
                    With .MyFieldsChecker
                        .AddControl(Of String)(TXT_GLOBAL_PATH, TXT_GLOBAL_PATH.CaptionText)
                        .AddControl(Of String)(TXT_COLLECTIONS_PATH, TXT_COLLECTIONS_PATH.CaptionText)
                        .AddControl(Of String)(TXT_REDDIT_SAVED_POSTS_USER, TXT_REDDIT_SAVED_POSTS_USER.CaptionText, True, New SavedPostsChecker)
                        .AddControl(Of Integer)(TXT_REQ_WAIT_TIMER, TXT_REQ_WAIT_TIMER.CaptionText,, New InstaTimersChecker(100))
                        .AddControl(Of Integer)(TXT_REQ_COUNT, TXT_REQ_COUNT.CaptionText,, New InstaTimersChecker(1))
                        .AddControl(Of Integer)(TXT_LIMIT_TIMER, TXT_LIMIT_TIMER.CaptionText,, New InstaTimersChecker(10000))
                        .EndLoaderOperations()
                    End With
                    .AppendDetectors()
                    .EndLoaderOperations()
                    ChangeFileNameChangersEnabling()
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Overloads Sub SetChecker(ByRef CH As SiteDefaults, ByVal s As Sites)
            With Settings(s)
                SetChecker(CH.MyTemporary, .Temporary)
                SetChecker(CH.MyImagesDown, .DownloadImages)
                SetChecker(CH.MyVideosDown, .DownloadVideos)
            End With
        End Sub
        Private Overloads Sub SetChecker(ByRef State As CheckState, ByVal Prop As XML.Base.XMLValue(Of Boolean))
            If Prop.ValueF.Exists Then
                State = If(Prop.Value, CheckState.Checked, CheckState.Unchecked)
            Else
                State = CheckState.Indeterminate
            End If
        End Sub
        Private Overloads Sub SetPropByChecker(ByRef CH As SiteDefaults, ByVal s As Sites)
            With Settings(s)
                SetPropByChecker(CH.MyTemporary, .Temporary)
                SetPropByChecker(CH.MyTemporary, .Temporary)
                SetPropByChecker(CH.MyImagesDown, .DownloadImages)
                SetPropByChecker(CH.MyVideosDown, .DownloadVideos)
            End With
        End Sub
        Private Overloads Sub SetPropByChecker(ByVal State As CheckState, ByRef Prop As XML.Base.XMLValue(Of Boolean))
            Select Case State
                Case CheckState.Checked : Prop.Value = True
                Case CheckState.Unchecked : Prop.Value = False
                Case CheckState.Indeterminate : Prop.ValueF = Nothing
            End Select
        End Sub
        Private Sub ToolbarBttOK() Implements IOkCancelToolbar.ToolbarBttOK
            If MyDefs.MyFieldsChecker.AllParamsOK Then
                With Settings
                    Dim a As Func(Of String, Object, Integer) =
                        Function(t, v) MsgBoxE({$"You are set up higher than default count of along {t} downloading tasks." & vbNewLine &
                                                $"Default: {SettingsCLS.DefaultMaxDownloadingTasks}" & vbNewLine &
                                                $"Your value: {CInt(v)}" & vbNewLine &
                                                "Increasing this value may lead to higher CPU usage." & vbNewLine &
                                                "Do you really want to continue?",
                                                "Increasing download tasks"},
                                               vbExclamation,,, {"Confirm", $"Set to default ({SettingsCLS.DefaultMaxDownloadingTasks})", "Cancel"})
                    If CInt(TXT_MAX_JOBS_USERS.Value) > SettingsCLS.DefaultMaxDownloadingTasks Then
                        Select Case a.Invoke("users", TXT_MAX_JOBS_USERS.Value)
                            Case 1 : TXT_MAX_JOBS_USERS.Value = SettingsCLS.DefaultMaxDownloadingTasks
                            Case 2 : Exit Sub
                        End Select
                    End If
                    If CInt(TXT_MAX_JOBS_CHANNELS.Value) > SettingsCLS.DefaultMaxDownloadingTasks Then
                        Select Case a.Invoke("channels", TXT_MAX_JOBS_CHANNELS.Value)
                            Case 1 : TXT_MAX_JOBS_CHANNELS.Value = SettingsCLS.DefaultMaxDownloadingTasks
                            Case 2 : Exit Sub
                        End Select
                    End If

                    If CH_FILE_NAME_CHANGE.Checked And (Not CH_FILE_DATE.Checked And Not CH_FILE_TIME.Checked) Then
                        MsgBoxE({"You must select at least one option (Date and/or Time) if you want to change file names by date or disable file names changes",
                                 "File name options"}, vbCritical)
                        Exit Sub
                    End If

                    .BeginUpdate()
                    'Basis
                    .GlobalPath.Value = TXT_GLOBAL_PATH.Text
                    .MaxLargeImageHeigh.Value = CInt(TXT_IMAGE_LARGE.Value)
                    .MaxSmallImageHeigh.Value = CInt(TXT_IMAGE_SMALL.Value)
                    .CollectionsPath.Value = TXT_COLLECTIONS_PATH.Text
                    .MaxUsersJobsCount.Value = CInt(TXT_MAX_JOBS_USERS.Value)
                    .ChannelsMaxJobsCount.Value = TXT_MAX_JOBS_CHANNELS.Value
                    .CheckUpdatesAtStart.Value = CH_CHECK_VER_START.Checked
                    .ImgurClientID.Value = TXT_IMGUR_CLIENT_ID.Text
                    'Defaults
                    .SeparateVideoFolder.Value = CH_SEPARATE_VIDEO_FOLDER.Checked
                    .DefaultTemporary.Value = CH_DEF_TEMP.Checked
                    .DefaultDownloadImages.Value = CH_DOWN_IMAGES.Checked
                    .DefaultDownloadVideos.Value = CH_DOWN_VIDEOS.Checked
                    'Channels
                    .ChannelsImagesRows.Value = CInt(TXT_CHANNELS_ROWS.Value)
                    .ChannelsImagesColumns.Value = CInt(TXT_CHANNELS_COLUMNS.Value)
                    .FromChannelDownloadTop.Value = CInt(TXT_CHANNEL_USER_POST_LIMIT.Value)
                    .FromChannelDownloadTopUse.Value = TXT_CHANNEL_USER_POST_LIMIT.Checked
                    .FromChannelCopyImageToUser.Value = CH_COPY_CHANNEL_USER_IMAGE.Checked
                    .ChannelsDefaultTemporary.Value = CH_CHANNELS_USERS_TEMP.Checked
                    'Other program settings
                    .ExitConfirm.Value = CH_EXIT_CONFIRM.Checked
                    .CloseToTray.Value = CH_CLOSE_TO_TRAY.Checked
                    .ShowNotifications.Value = CH_SHOW_NOTIFY.Checked

                    If CH_FILE_NAME_CHANGE.Checked Then
                        .FileReplaceNameByDate.Value = OPT_FILE_NAME_REPLACE.Checked
                        .FileAddDateToFileName.Value = CH_FILE_DATE.Checked
                        .FileAddTimeToFileName.Value = CH_FILE_TIME.Checked
                        .FileDateTimePositionEnd.Value = OPT_FILE_DATE_END.Checked
                    Else
                        .FileAddDateToFileName.Value = False
                        .FileAddTimeToFileName.Value = False
                        .FileReplaceNameByDate.Value = False
                    End If
                    'Reddit
                    With .Site(Sites.Reddit)
                        SetPropByChecker(DEFS_REDDIT, Sites.Reddit)
                        .SavedPostsUserName.Value = TXT_REDDIT_SAVED_POSTS_USER.Text
                    End With
                    'Twitter
                    With .Site(Sites.Twitter)
                        SetPropByChecker(DEFS_TWITTER, Sites.Twitter)
                        .GetUserMediaOnly.Value = CH_TWITTER_USER_MEDIA.Checked
                    End With
                    'Instagram
                    With .Site(Sites.Instagram)
                        SetPropByChecker(DEFS_INST, Sites.Instagram)
                        .RequestsWaitTimer.Value = AConvert(Of Integer)(TXT_REQ_WAIT_TIMER.Text)
                        .RequestsWaitTimerTaskCount.Value = AConvert(Of Integer)(TXT_REQ_COUNT.Text)
                        .SleepTimerOnPostsLimit.Value = AConvert(Of Integer)(TXT_LIMIT_TIMER.Text)
                        .SavedPostsUserName.Value = TXT_INST_SAVED_POSTS_USER.Text
                    End With
                    'RedGifs
                    SetPropByChecker(DEFS_REDGIFS, Sites.RedGifs)

                    .EndUpdate()
                End With
                MyDefs.CloseForm()
            End If
        End Sub
        Private Sub ToolbarBttCancel() Implements IOkCancelToolbar.ToolbarBttCancel
            MyDefs.CloseForm(DialogResult.Cancel)
        End Sub
        Private Sub TXT_GLOBAL_PATH_ActionOnButtonClick(ByVal Sender As ActionButton) Handles TXT_GLOBAL_PATH.ActionOnButtonClick
            If Sender.DefaultButton = ActionButton.DefaultButtons.Open Then
                Dim f As SFile = SFile.SelectPath(Settings.GlobalPath.Value)
                If Not f.IsEmptyString Then TXT_GLOBAL_PATH.Text = f
            End If
        End Sub
        Private Sub TXT_MAX_JOBS_USERS_ActionOnButtonClick(ByVal Sender As ActionButton) Handles TXT_MAX_JOBS_USERS.ActionOnButtonClick
            If Sender.DefaultButton = ActionButton.DefaultButtons.Refresh Then TXT_MAX_JOBS_USERS.Value = SettingsCLS.DefaultMaxDownloadingTasks
        End Sub
        Private Sub TXT_MAX_JOBS_CHANNELS_ActionOnButtonClick(ByVal Sender As ActionButton) Handles TXT_MAX_JOBS_CHANNELS.ActionOnButtonClick
            If Sender.DefaultButton = ActionButton.DefaultButtons.Refresh Then TXT_MAX_JOBS_CHANNELS.Value = SettingsCLS.DefaultMaxDownloadingTasks
        End Sub
        Private Sub CH_FILE_NAME_CHANGE_CheckedChanged(sender As Object, e As EventArgs) Handles CH_FILE_NAME_CHANGE.CheckedChanged
            ChangeFileNameChangersEnabling()
        End Sub
        Private Sub OPT_FILE_NAME_REPLACE_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_FILE_NAME_REPLACE.CheckedChanged
            ChangePositionControlsEnabling()
        End Sub
        Private Sub OPT_FILE_NAME_ADD_DATE_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_FILE_NAME_ADD_DATE.CheckedChanged
            ChangePositionControlsEnabling()
        End Sub
        Private Sub ChangePositionControlsEnabling()
            Dim b As Boolean = OPT_FILE_NAME_ADD_DATE.Checked And OPT_FILE_NAME_ADD_DATE.Enabled
            OPT_FILE_DATE_START.Enabled = b
            OPT_FILE_DATE_END.Enabled = b
        End Sub
        Private Sub ChangeFileNameChangersEnabling()
            Dim b As Boolean = CH_FILE_NAME_CHANGE.Checked
            OPT_FILE_NAME_REPLACE.Enabled = b
            OPT_FILE_NAME_ADD_DATE.Enabled = b
            CH_FILE_DATE.Enabled = b
            CH_FILE_TIME.Enabled = b
            ChangePositionControlsEnabling()
        End Sub
    End Class
End Namespace