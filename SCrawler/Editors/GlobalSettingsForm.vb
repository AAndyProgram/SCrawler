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
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Namespace Editors
    Friend Class GlobalSettingsForm
        Private WithEvents MyDefs As DefaultFormOptions
        Friend Property FeedParametersChanged As Boolean = False
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
        End Sub
        Private Sub GlobalSettingsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDefs
                    .MyViewInitialize(True)
                    .AddOkCancelToolbar()
                    With Settings
                        'Basis
                        TXT_GLOBAL_PATH.Text = .GlobalPath.Value
                        TXT_IMAGE_LARGE.Value = .MaxLargeImageHeight.Value
                        TXT_IMAGE_SMALL.Value = .MaxSmallImageHeight.Value
                        TXT_COLLECTIONS_PATH.Text = .CollectionsPath
                        TXT_MAX_JOBS_USERS.Value = .MaxUsersJobsCount.Value
                        TXT_MAX_JOBS_CHANNELS.Value = .ChannelsMaxJobsCount.Value
                        CH_CHECK_VER_START.Checked = .CheckUpdatesAtStart
                        TXT_IMGUR_CLIENT_ID.Text = .ImgurClientID
                        CH_SHOW_GROUPS.Checked = .ShowGroups
                        CH_USERS_GROUPING.Checked = .UseGrouping
                        'Behavior
                        CH_EXIT_CONFIRM.Checked = .ExitConfirm
                        CH_CLOSE_TO_TRAY.Checked = .CloseToTray
                        CH_SHOW_NOTIFY.Checked = .ShowNotifications
                        CH_FAST_LOAD.Checked = .FastProfilesLoading
                        CH_RECYCLE_DEL.Checked = .DeleteToRecycleBin
                        CH_DOWN_OPEN_INFO.Checked = .DownloadOpenInfo
                        CH_DOWN_OPEN_INFO_SUSPEND.Checked = Not .DownloadOpenInfo.Attribute
                        CH_DOWN_OPEN_PROGRESS.Checked = .DownloadOpenProgress
                        CH_DOWN_OPEN_PROGRESS_SUSPEND.Checked = Not .DownloadOpenProgress.Attribute
                        TXT_FOLDER_CMD.Text = .OpenFolderInOtherProgram
                        TXT_FOLDER_CMD.Checked = .OpenFolderInOtherProgram.Attribute
                        TXT_CLOSE_SCRIPT.Text = .ClosingCommand
                        TXT_CLOSE_SCRIPT.Checked = .ClosingCommand.Attribute
                        'Defaults
                        CH_SEPARATE_VIDEO_FOLDER.Checked = .SeparateVideoFolder.Value
                        CH_DEF_TEMP.Checked = .DefaultTemporary
                        CH_DOWN_IMAGES.Checked = .DefaultDownloadImages
                        CH_DOWN_VIDEOS.Checked = .DefaultDownloadVideos
                        CH_DOWN_IMAGES_NATIVE.Checked = .DownloadNativeImageFormat
                        'Downloading
                        CH_UDESCR_UP.Checked = .UpdateUserDescriptionEveryTime
                        TXT_SCRIPT.Checked = .ScriptData.Attribute
                        TXT_SCRIPT.Text = .ScriptData.Value
                        TXT_DOWN_COMPLETE_SCRIPT.Text = .DownloadsCompleteCommand
                        TXT_DOWN_COMPLETE_SCRIPT.Checked = .DownloadsCompleteCommand.Attribute
                        CH_ADD_MISSING_TO_LOG.Checked = .AddMissingToLog
                        CH_ADD_MISSING_ERROS_TO_LOG.Checked = .AddMissingErrorsToLog
                        'Downloading: file names
                        CH_FILE_NAME_CHANGE.Checked = Not .FileReplaceNameByDate.Value = FileNameReplaceMode.None
                        OPT_FILE_NAME_REPLACE.Checked = .FileReplaceNameByDate.Value = FileNameReplaceMode.Replace
                        OPT_FILE_NAME_ADD_DATE.Checked = .FileReplaceNameByDate.Value = FileNameReplaceMode.Add
                        CH_FILE_DATE.Checked = .FileAddDateToFileName
                        CH_FILE_TIME.Checked = .FileAddTimeToFileName
                        OPT_FILE_DATE_START.Checked = Not .FileDateTimePositionEnd
                        OPT_FILE_DATE_END.Checked = .FileDateTimePositionEnd
                        'Channels
                        TXT_CHANNELS_ROWS.Value = .ChannelsImagesRows.Value
                        TXT_CHANNELS_COLUMNS.Value = .ChannelsImagesColumns.Value
                        TXT_CHANNEL_USER_POST_LIMIT.Value = .FromChannelDownloadTop.Value
                        TXT_CHANNEL_USER_POST_LIMIT.Checked = .FromChannelDownloadTopUse.Value
                        CH_COPY_CHANNEL_USER_IMAGE.Checked = .FromChannelCopyImageToUser
                        CH_COPY_CHANNEL_USER_IMAGE_ALL.Checked = .ChannelsAddUserImagesFromAllChannels
                        CH_COPY_CHANNEL_USER_IMAGE_ALL.Enabled = CH_COPY_CHANNEL_USER_IMAGE.Checked
                        CH_CHANNELS_USERS_TEMP.Checked = .ChannelsDefaultTemporary
                        'Feed
                        TXT_FEED_ROWS.Value = .FeedDataRows.Value
                        TXT_FEED_COLUMNS.Value = .FeedDataColumns.Value
                        CH_FEED_ENDLESS.Checked = .FeedEndless
                    End With
                    .MyFieldsChecker = New FieldsChecker
                    With .MyFieldsCheckerE
                        .AddControl(Of String)(TXT_GLOBAL_PATH, TXT_GLOBAL_PATH.CaptionText)
                        .AddControl(Of String)(TXT_COLLECTIONS_PATH, TXT_COLLECTIONS_PATH.CaptionText)
                        .EndLoaderOperations()
                    End With
                    ChangeFileNameChangersEnabling()
                    .EndLoaderOperations()
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
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
                    .MaxLargeImageHeight.Value = CInt(TXT_IMAGE_LARGE.Value)
                    .MaxSmallImageHeight.Value = CInt(TXT_IMAGE_SMALL.Value)
                    .CollectionsPath.Value = TXT_COLLECTIONS_PATH.Text
                    .MaxUsersJobsCount.Value = CInt(TXT_MAX_JOBS_USERS.Value)
                    .ChannelsMaxJobsCount.Value = TXT_MAX_JOBS_CHANNELS.Value
                    .CheckUpdatesAtStart.Value = CH_CHECK_VER_START.Checked
                    .ImgurClientID.Value = TXT_IMGUR_CLIENT_ID.Text
                    .ShowGroups.Value = CH_SHOW_GROUPS.Checked
                    .UseGrouping.Value = CH_USERS_GROUPING.Checked
                    'Behavior
                    .ExitConfirm.Value = CH_EXIT_CONFIRM.Checked
                    .CloseToTray.Value = CH_CLOSE_TO_TRAY.Checked
                    .ShowNotifications.Value = CH_SHOW_NOTIFY.Checked
                    .FastProfilesLoading.Value = CH_FAST_LOAD.Checked
                    .DeleteToRecycleBin.Value = CH_RECYCLE_DEL.Checked
                    .DownloadOpenInfo.Value = CH_DOWN_OPEN_INFO.Checked
                    .DownloadOpenInfo.Attribute.Value = Not CH_DOWN_OPEN_INFO_SUSPEND.Checked
                    .DownloadOpenProgress.Value = CH_DOWN_OPEN_PROGRESS.Checked
                    .DownloadOpenProgress.Attribute.Value = Not CH_DOWN_OPEN_PROGRESS_SUSPEND.Checked
                    .OpenFolderInOtherProgram.Value = TXT_FOLDER_CMD.Text
                    .OpenFolderInOtherProgram.Attribute.Value = TXT_FOLDER_CMD.Checked
                    .ClosingCommand.Value = TXT_CLOSE_SCRIPT.Text
                    .ClosingCommand.Attribute.Value = TXT_CLOSE_SCRIPT.Checked
                    'Defaults
                    .SeparateVideoFolder.Value = CH_SEPARATE_VIDEO_FOLDER.Checked
                    .DefaultTemporary.Value = CH_DEF_TEMP.Checked
                    .DefaultDownloadImages.Value = CH_DOWN_IMAGES.Checked
                    .DefaultDownloadVideos.Value = CH_DOWN_VIDEOS.Checked
                    .DownloadNativeImageFormat.Value = CH_DOWN_IMAGES_NATIVE.Checked
                    'Downloading
                    .UpdateUserDescriptionEveryTime.Value = CH_UDESCR_UP.Checked
                    .ScriptData.Value = TXT_SCRIPT.Text
                    .ScriptData.Attribute.Value = TXT_SCRIPT.Checked
                    .DownloadsCompleteCommand.Value = TXT_DOWN_COMPLETE_SCRIPT.Text
                    .DownloadsCompleteCommand.Attribute.Value = TXT_DOWN_COMPLETE_SCRIPT.Checked
                    .AddMissingToLog.Value = CH_ADD_MISSING_TO_LOG.Checked
                    .AddMissingErrorsToLog.Value = CH_ADD_MISSING_ERROS_TO_LOG.Checked
                    'Downloading: file names
                    If CH_FILE_NAME_CHANGE.Checked Then
                        .FileReplaceNameByDate.Value = If(OPT_FILE_NAME_REPLACE.Checked, FileNameReplaceMode.Replace, FileNameReplaceMode.Add)
                        .FileAddDateToFileName.Value = CH_FILE_DATE.Checked
                        .FileAddTimeToFileName.Value = CH_FILE_TIME.Checked
                        .FileDateTimePositionEnd.Value = OPT_FILE_DATE_END.Checked
                    Else
                        .FileAddDateToFileName.Value = False
                        .FileAddTimeToFileName.Value = False
                        .FileReplaceNameByDate.Value = FileNameReplaceMode.None
                    End If
                    'Channels
                    .ChannelsImagesRows.Value = CInt(TXT_CHANNELS_ROWS.Value)
                    .ChannelsImagesColumns.Value = CInt(TXT_CHANNELS_COLUMNS.Value)
                    .FromChannelDownloadTop.Value = CInt(TXT_CHANNEL_USER_POST_LIMIT.Value)
                    .FromChannelDownloadTopUse.Value = TXT_CHANNEL_USER_POST_LIMIT.Checked
                    .FromChannelCopyImageToUser.Value = CH_COPY_CHANNEL_USER_IMAGE.Checked
                    .ChannelsAddUserImagesFromAllChannels.Value = CH_COPY_CHANNEL_USER_IMAGE_ALL.Checked
                    .ChannelsDefaultTemporary.Value = CH_CHANNELS_USERS_TEMP.Checked
                    'Feed
                    .FeedDataRows.Value = CInt(TXT_FEED_ROWS.Value)
                    .FeedDataColumns.Value = CInt(TXT_FEED_COLUMNS.Value)
                    .FeedEndless.Value = CH_FEED_ENDLESS.Checked
                    FeedParametersChanged = .FeedDataRows.ChangesDetected Or .FeedDataColumns.ChangesDetected Or .FeedEndless.ChangesDetected

                    .EndUpdate()
                End With
                MyDefs.CloseForm()
            End If
        End Sub
        Private Sub TXT_GLOBAL_PATH_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As EventArgs) Handles TXT_GLOBAL_PATH.ActionOnButtonClick
            If Sender.DefaultButton = ADB.Open Then
                Dim f As SFile = SFile.SelectPath(Settings.GlobalPath.Value)
                If Not f.IsEmptyString Then TXT_GLOBAL_PATH.Text = f
            End If
        End Sub
        Private Sub TXT_MAX_JOBS_USERS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As EventArgs) Handles TXT_MAX_JOBS_USERS.ActionOnButtonClick
            If Sender.DefaultButton = ADB.Refresh Then TXT_MAX_JOBS_USERS.Value = SettingsCLS.DefaultMaxDownloadingTasks
        End Sub
        Private Sub TXT_MAX_JOBS_CHANNELS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As EventArgs) Handles TXT_MAX_JOBS_CHANNELS.ActionOnButtonClick
            If Sender.DefaultButton = ADB.Refresh Then TXT_MAX_JOBS_CHANNELS.Value = SettingsCLS.DefaultMaxDownloadingTasks
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
            If Not OPT_FILE_NAME_REPLACE.Checked And Not OPT_FILE_NAME_ADD_DATE.Checked Then OPT_FILE_NAME_REPLACE.Checked = True
            CH_FILE_DATE.Enabled = b
            CH_FILE_TIME.Enabled = b
            ChangePositionControlsEnabling()
        End Sub
        Private Sub TXT_SCRIPT_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As EventArgs) Handles TXT_SCRIPT.ActionOnButtonClick
            SettingsCLS.ScriptTextBoxButtonClick(TXT_SCRIPT, Sender)
        End Sub
        Private Sub CH_COPY_CHANNEL_USER_IMAGE_CheckedChanged(sender As Object, e As EventArgs) Handles CH_COPY_CHANNEL_USER_IMAGE.CheckedChanged
            CH_COPY_CHANNEL_USER_IMAGE_ALL.Enabled = CH_COPY_CHANNEL_USER_IMAGE.Checked
        End Sub
    End Class
End Namespace