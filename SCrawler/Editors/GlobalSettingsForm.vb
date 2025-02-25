' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Functions.XML.Objects
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Imports StdDblClck = SCrawler.DownloadObjects.STDownloader.DoubleClickBehavior
Namespace Editors
    Friend Class GlobalSettingsForm
        Private WithEvents MyDefs As DefaultFormOptions
        Friend Property FeedParametersChanged As Boolean = False
        Friend Property HeadersChanged As Boolean = False
        Friend Property PictureChanged As Boolean = False
        Friend Property EnvironmentProgramsChanged As Boolean = False
        Friend Property UserAgentChanged As Boolean = False
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
                        'Design
                        TXT_PRG_TITLE.Text = .ProgramText
                        TXT_PRG_DESCR.Text = .ProgramDescription
                        TXT_USER_LIST_IMAGE.Text = .UserListImage.Value
                        COLORS_USERLIST.ColorsSet(.UserListBackColor, .UserListForeColor, SystemColors.Window, SystemColors.WindowText)
                        COLORS_SUBSCRIPTIONS.ColorsSet(.MainFrameUsersSubscriptionsColorBack, .MainFrameUsersSubscriptionsColorFore,
                                                       SystemColors.Window, SystemColors.WindowText)
                        COLORS_SUBSCRIPTIONS_USERS.ColorsSet(.MainFrameUsersSubscriptionsColorBack_USERS, .MainFrameUsersSubscriptionsColorFore_USERS,
                                                             SystemColors.Window, SystemColors.WindowText)
                        'Environment
                        TXT_FFMPEG.Text = .FfmpegFile.File
                        TXT_CURL.Text = .CurlFile.File
                        TXT_YTDLP.Text = .YtdlpFile.File
                        TXT_GALLERYDL.Text = .GalleryDLFile.File
                        TXT_CMD_ENCODING.Text = .CMDEncoding
                        'Headers
                        TXT_H_DEF_UserAgent.Text = .HEADER_UserAgent
                        TXT_H_DEF_sec_ch_ua.Text = .HEADER_sec_ch_ua
                        TXT_H_DEF_sec_ch_ua_full_version_list.Text = .HEADER_sec_ch_ua_full_version_list
                        TXT_H_DEF_sec_ch_ua_platform.Text = .HEADER_sec_ch_ua_platform
                        TXT_H_DEF_sec_ch_ua_platform_version.Text = .HEADER_sec_ch_ua_platform_version
                        'Behavior
                        CH_EXIT_CONFIRM.Checked = .ExitConfirm
                        CH_CLOSE_TO_TRAY.Checked = .CloseToTray
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
                        CH_DOWN_ALL_F6_USE.Checked = .DownloadAll_UseF6
                        CH_DOWN_ALL_F6_NOTIFY.Checked = .DownloadAll_UseF6_Confirm
                        CH_DOWN_ALL_NOTIFY.Checked = .DownloadAll_Confirm
                        TXT_SCHEDULER_SCRIPT.Text = .AutomationScript
                        TXT_SCHEDULER_SCRIPT.Checked = .AutomationScript.Use
                        CH_SCHEDULER_SCRIPT_EX_MANUAL.Checked = .AutomationScript_ExcludeManual
                        CH_FEED_OPEN_CTRLF.Checked = .FeedOpenCtrlF
                        'Notifications
                        CH_NOTIFY_SILENT.Checked = .NotificationsSilentMode
                        CH_NOTIFY_SHOW_BASE.Checked = .ShowNotifications
                        CH_NOTIFY_PROFILES.Checked = .ShowNotificationsDownProfiles
                        CH_NOTIFY_AUTO_DOWN.Checked = .ShowNotificationsDownAutoDownloader
                        CH_NOTIFY_CHANNELS.Checked = .ShowNotificationsDownChannels
                        CH_NOTIFY_SAVED_POSTS.Checked = .ShowNotificationsDownSavedPosts
                        CH_STD.Checked = .ShowNotificationsSTDownloader
                        CH_STD_EVERY.Checked = .ShowNotificationsSTDownloaderEveryDownload
                        CH_NOTIFY_LOG.Checked = .ShowNotificationsLOG
                        'Defaults
                        CH_SEPARATE_VIDEO_FOLDER.Checked = .SeparateVideoFolder.Value
                        CH_DEF_TEMP.Checked = .DefaultTemporary
                        CH_DOWN_IMAGES.Checked = .DefaultDownloadImages
                        CH_DOWN_VIDEOS.Checked = .DefaultDownloadVideos
                        CH_DOWN_IMAGES_NATIVE.Checked = .DownloadNativeImageFormat
                        CH_NAME_SITE_FRIENDLY.Checked = .UserSiteNameAsFriendly
                        'STDownloader
                        TXT_STD_MAX_JOBS_COUNT.Value = .STDownloader_MaxJobsCount.Value
                        CH_STD_AUTO_DOWN.Checked = .STDownloader_DownloadAutomatically
                        CH_STD_AUTO_REMOVE.Checked = .STDownloader_RemoveDownloadedAutomatically
                        'CMB_STD_OPEN_DBL.BeginUpdate()
                        CMB_STD_OPEN_DBL.Items.AddRange([Enum].GetValues(GetType(StdDblClck)).ToObjectsList(Of StdDblClck).Select(Function(dcb) New ListItem({dcb.ToString, dcb})))
                        CMB_STD_OPEN_DBL.EndUpdate(True)
                        CMB_STD_OPEN_DBL.SelectedIndex = [Enum].GetValues(GetType(StdDblClck)).ToObjectsList(Of StdDblClck).ToList.IndexOf(.STDownloader_OnItemDoubleClick.Value)
                        CH_STD_TAKESNAP.Checked = .STDownloader_TakeSnapshot
                        CH_STD_SNAP_KEEP_WITH_FILES.Checked = .STDownloader_SnapshotsKeepWithFiles
                        CH_STD_SNAP_CACHE_PERMANENT.Checked = .STDownloader_SnapShotsCachePermamnent
                        CH_STD_UPDATE_YT_PATH.Checked = .STDownloader_UpdateYouTubeOutputPath
                        CH_STD_YT_LOAD.Checked = .STDownloader_LoadYTVideos
                        CH_STD_YT_REMOVE.Checked = .STDownloader_RemoveYTVideosOnClear
                        CH_STD_YT_OUTPUT_ASK_NAME.Checked = .STDownloader_OutputPathAskForName
                        CH_STD_YT_OUTPUT_AUTO_ADD.Checked = .STDownloader_OutputPathAutoAddPaths
                        CH_STD_YT_CREATE_URL.Checked = .STDownloader_CreateUrlFiles
                        'Downloading
                        CH_UDESCR_UP.Checked = .UpdateUserDescriptionEveryTime
                        CH_UNAME_UP.Checked = .UpdateUserSiteNameEveryTime
                        CH_UICON_UP.Checked = .UpdateUserIconBannerEveryTime
                        TXT_SCRIPT.Checked = .ScriptData.Attribute
                        TXT_SCRIPT.Text = .ScriptData.Value
                        TXT_DOWN_COMPLETE_SCRIPT.Text = .DownloadsCompleteCommand
                        TXT_DOWN_COMPLETE_SCRIPT.Checked = .DownloadsCompleteCommand.Attribute
                        CH_ADD_MISSING_TO_LOG.Checked = .AddMissingToLog
                        CH_ADD_MISSING_ERROS_TO_LOG.Checked = .AddMissingErrorsToLog
                        CH_DOWN_REPARSE_MISSING.Checked = .ReparseMissingInTheRoutine
                        CH_USE_DEF_ACC.Checked = .UseDefaultAccountIfMissing
                        TXT_AUTO_BRUSH_MIN.Value = .AutomationBrushUndownloadedPlansMinutes.Value
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
                        CH_CHANNELS_USERS_READY_MARK.Checked = .ChannelsDefaultReadyForDownload
                        'Feed
                        TXT_FEED_ROWS.Value = .FeedDataRows.Value
                        TXT_FEED_COLUMNS.Value = .FeedDataColumns.Value
                        TXT_FEED_CENTER_IMAGE.Checked = .FeedCenterImage.Use
                        TXT_FEED_CENTER_IMAGE.Value = .FeedCenterImage.Value
                        TXT_FEED_CENTER_IMAGE.Enabled = .FeedDataColumns = 1
                        COLORS_FEED.ColorsSet(.FeedBackColor, .FeedForeColor, SystemColors.Window, SystemColors.WindowText)
                        CH_FEED_ENDLESS.Checked = .FeedEndless
                        CH_FEED_ADD_SESSION.Checked = .FeedAddSessionToCaption
                        CH_FEED_ADD_SITE.Checked = .FeedAddSiteToCaption
                        CH_FEED_ADD_TYPE.Checked = .FeedAddTypeToCaption
                        CH_FEED_ADD_DATE.Checked = .FeedAddDateToCaption
                        NUM_FEED_STORE_SESSION_DATA.Checked = .FeedStoreSessionsData
                        NUM_FEED_STORE_SESSION_DATA.Value = .FeedStoredSessionsNumber.Value
                        NUM_FEED_SES_CURR_LOAD_LAST.Value = .FeedCurrentTryLoadLastSession.Value
                        CH_FEED_OPEN_LAST_MODE.Checked = .FeedOpenLastMode
                        CH_FEED_SHOW_FRIENDLY.Checked = .FeedShowFriendlyNames
                        CH_FEED_SHOW_SPEC_MEDIAITEM.Checked = .FeedShowSpecialFeedsMediaItem
                        CH_FEED_UP_FILE_LOC_MOVE.Checked = .FeedMoveCopyUpdateFileLocationOnMove
                        CH_FEED_ESC_TO_CLOSE.Checked = .FeedEscToClose
                        CH_FEED_SPEC_SEARCH.Checked = .FeedSpecialSearchForMissing
                        CH_FEED_SPEC_SEARCH_DEEP.Checked = .FeedSpecialSearchForMissing_Deep
                    End With
                    .MyFieldsChecker = New FieldsChecker
                    With .MyFieldsCheckerE
                        .AddControl(Of String)(TXT_GLOBAL_PATH, TXT_GLOBAL_PATH.CaptionText)
                        .AddControl(Of String)(TXT_COLLECTIONS_PATH, TXT_COLLECTIONS_PATH.CaptionText)
                        .AddControl(Of Integer)(TXT_CMD_ENCODING, TXT_CMD_ENCODING.CaptionText)
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

                    Dim paths$ = String.Empty
                    If Not TXT_FFMPEG.Text.CSFile.Exists Then paths.StringAppendLine("ffmpeg.exe")
                    If Not TXT_YTDLP.Text.CSFile.Exists Then paths.StringAppendLine("yt-dlp.exe")
                    If Not TXT_GALLERYDL.Text.CSFile.Exists Then paths.StringAppendLine("gallery-dl.exe")
                    If Not TXT_CURL.Text.CSFile.Exists Then paths.StringAppendLine("curl.exe")
                    If Not paths.IsEmptyString AndAlso
                       MsgBoxE({$"The following paths to programs are missing or invalid:{vbCr}{vbCr}{paths}{vbCr}{vbCr}" &
                                "Do you still want to process without setting these fields?" & vbCr &
                                "If this case, the functionality of SCrawler will be limited, and some sites will not work at all.",
                                "Environment missing"}, vbExclamation,,, {"Process", "Cancel"}) = 1 Then Exit Sub

                    If Not .GlobalPath.Value.PathWithSeparator = TXT_GLOBAL_PATH.Text.CSFilePS Or Not .CollectionsPath.Value = TXT_COLLECTIONS_PATH.Text Then
                        If Not Plugin.Hosts.SettingsHostCollection.UpdateHostPath_CheckDownloader Then Exit Sub
                        If MsgBoxE({"You have changed the global path or collections folder!" & vbCr & vbCr &
                                    $"Global path ({IIf(.GlobalPath.Value.PathWithSeparator = TXT_GLOBAL_PATH.Text.CSFilePS, "not changed", "CHANGED")})" & vbCr &
                                    $"Current: { .GlobalPath.Value}" & vbCr &
                                    $"New: {TXT_GLOBAL_PATH.Text}" & vbCr & vbCr &
                                    $"Collections folder ({IIf(.CollectionsPath.Value = TXT_COLLECTIONS_PATH.Text, "not changed", "CHANGED")})" & vbCr &
                                    $"Current: { .CollectionsPath.Value}" & vbCr &
                                    $"New: {TXT_COLLECTIONS_PATH.Text}" & vbCr & vbCr &
                                    "Are you sure you want to continue?",
                                    "Global path changed"}, vbExclamation,,, {"Process", "Cancel"}) = 0 Then
                            If Not Plugin.Hosts.SettingsHostCollection.UpdateHostPath(.GlobalPath, TXT_GLOBAL_PATH.Text.CSFileP,
                                                                                      .CollectionsPath, TXT_COLLECTIONS_PATH.Text) Then _
                               MsgBoxE({"Something went wrong while updating the global paths.", "Global path changed"}, vbCritical)
                        Else
                            Exit Sub
                        End If
                    End If

                    Dim detector As Func(Of IXMLValue, Boolean) = Function(hh) hh.ChangesDetected
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
                    'Design
                    .ProgramText.Value = TXT_PRG_TITLE.Text
                    .ProgramDescription.Value = TXT_PRG_DESCR.Text
                    .UserListImage.Value = TXT_USER_LIST_IMAGE.Text
                    PictureChanged = .UserListImage.ChangesDetected
                    COLORS_USERLIST.ColorsGet(.UserListBackColor, .UserListForeColor)
                    COLORS_SUBSCRIPTIONS.ColorsGet(.MainFrameUsersSubscriptionsColorBack, .MainFrameUsersSubscriptionsColorFore)
                    COLORS_SUBSCRIPTIONS_USERS.ColorsGet(.MainFrameUsersSubscriptionsColorBack_USERS, .MainFrameUsersSubscriptionsColorFore_USERS)
                    'Environment
                    .FfmpegFile.File = TXT_FFMPEG.Text
                    .CurlFile.File = TXT_CURL.Text
                    .YtdlpFile.File = TXT_YTDLP.Text
                    .GalleryDLFile.File = TXT_GALLERYDL.Text
                    .CMDEncoding.Value = AConvert(Of Integer)(TXT_CMD_ENCODING.Text, SettingsCLS.DefaultCmdEncoding)
                    EnvironmentProgramsChanged = { .FfmpegFile, .CurlFile, .YtdlpFile, .GalleryDLFile}.Any(Function(p) p.ChangesDetected) Or
                                                 .CMDEncoding.ChangesDetected
                    'Headers
                    .HEADER_UserAgent.Value = TXT_H_DEF_UserAgent.Text
                    UserAgentChanged = .HEADER_UserAgent.ChangesDetected
                    .HEADER_sec_ch_ua.Value = TXT_H_DEF_sec_ch_ua.Text
                    .HEADER_sec_ch_ua_full_version_list.Value = TXT_H_DEF_sec_ch_ua_full_version_list.Text
                    .HEADER_sec_ch_ua_platform.Value = TXT_H_DEF_sec_ch_ua_platform.Text
                    .HEADER_sec_ch_ua_platform_version.Value = TXT_H_DEF_sec_ch_ua_platform_version.Text
                    HeadersChanged = { .HEADER_UserAgent, .HEADER_sec_ch_ua, .HEADER_sec_ch_ua_full_version_list,
                                       .HEADER_sec_ch_ua_platform, .HEADER_sec_ch_ua_platform_version}.Cast(Of IXMLValue).Any(detector)
                    'Behavior
                    .ExitConfirm.Value = CH_EXIT_CONFIRM.Checked
                    .CloseToTray.Value = CH_CLOSE_TO_TRAY.Checked
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
                    .DownloadAll_UseF6.Value = CH_DOWN_ALL_F6_USE.Checked
                    .DownloadAll_UseF6_Confirm.Value = CH_DOWN_ALL_F6_NOTIFY.Checked
                    .DownloadAll_Confirm.Value = CH_DOWN_ALL_NOTIFY.Checked
                    .AutomationScript.Value = TXT_SCHEDULER_SCRIPT.Text
                    .AutomationScript.Use = TXT_SCHEDULER_SCRIPT.Checked
                    .AutomationScript_ExcludeManual.Value = CH_SCHEDULER_SCRIPT_EX_MANUAL.Checked
                    .FeedOpenCtrlF.Value = CH_FEED_OPEN_CTRLF.Checked
                    'Notifications
                    .NotificationsSilentMode = CH_NOTIFY_SILENT.Checked
                    .ShowNotifications.Value = CH_NOTIFY_SHOW_BASE.Checked
                    .ShowNotificationsDownProfiles.Value = CH_NOTIFY_PROFILES.Checked
                    .ShowNotificationsDownAutoDownloader.Value = CH_NOTIFY_AUTO_DOWN.Checked
                    .ShowNotificationsDownChannels.Value = CH_NOTIFY_CHANNELS.Checked
                    .ShowNotificationsDownSavedPosts.Value = CH_NOTIFY_SAVED_POSTS.Checked
                    .ShowNotificationsSTDownloader.Value = CH_STD.Checked
                    .ShowNotificationsSTDownloaderEveryDownload.Value = CH_STD_EVERY.Checked
                    .ShowNotificationsLOG.Value = CH_NOTIFY_LOG.Checked
                    'Defaults
                    .SeparateVideoFolder.Value = CH_SEPARATE_VIDEO_FOLDER.Checked
                    .DefaultTemporary.Value = CH_DEF_TEMP.Checked
                    .DefaultDownloadImages.Value = CH_DOWN_IMAGES.Checked
                    .DefaultDownloadVideos.Value = CH_DOWN_VIDEOS.Checked
                    .DownloadNativeImageFormat.Value = CH_DOWN_IMAGES_NATIVE.Checked
                    .UserSiteNameAsFriendly.Value = CH_NAME_SITE_FRIENDLY.Checked
                    'STDownloader
                    .STDownloader_MaxJobsCount.Value = CInt(TXT_STD_MAX_JOBS_COUNT.Value)
                    .STDownloader_DownloadAutomatically.Value = CH_STD_AUTO_DOWN.Checked
                    .STDownloader_RemoveDownloadedAutomatically.Value = CH_STD_AUTO_REMOVE.Checked
                    .STDownloader_OnItemDoubleClick.Value = CInt(CMB_STD_OPEN_DBL.Value)
                    .STDownloader_TakeSnapshot.Value = CH_STD_TAKESNAP.Checked
                    .STDownloader_SnapshotsKeepWithFiles.Value = CH_STD_SNAP_KEEP_WITH_FILES.Checked
                    .STDownloader_SnapShotsCachePermamnent.Value = CH_STD_SNAP_CACHE_PERMANENT.Checked
                    .STDownloader_UpdateYouTubeOutputPath.Value = CH_STD_UPDATE_YT_PATH.Checked
                    .STDownloader_LoadYTVideos.Value = CH_STD_YT_LOAD.Checked
                    .STDownloader_RemoveYTVideosOnClear.Value = CH_STD_YT_REMOVE.Checked
                    .STDownloader_OutputPathAskForName.Value = CH_STD_YT_OUTPUT_ASK_NAME.Checked
                    .STDownloader_OutputPathAutoAddPaths.Value = CH_STD_YT_OUTPUT_AUTO_ADD.Checked
                    .STDownloader_CreateUrlFiles.Value = CH_STD_YT_CREATE_URL.Checked
                    'Downloading
                    .UpdateUserDescriptionEveryTime.Value = CH_UDESCR_UP.Checked
                    .UpdateUserSiteNameEveryTime.Value = CH_UNAME_UP.Checked
                    .UpdateUserIconBannerEveryTime.Value = CH_UICON_UP.Checked
                    .ScriptData.Value = TXT_SCRIPT.Text
                    .ScriptData.Attribute.Value = TXT_SCRIPT.Checked
                    .DownloadsCompleteCommand.Value = TXT_DOWN_COMPLETE_SCRIPT.Text
                    .DownloadsCompleteCommand.Attribute.Value = TXT_DOWN_COMPLETE_SCRIPT.Checked
                    .AddMissingToLog.Value = CH_ADD_MISSING_TO_LOG.Checked
                    .AddMissingErrorsToLog.Value = CH_ADD_MISSING_ERROS_TO_LOG.Checked
                    .ReparseMissingInTheRoutine.Value = CH_DOWN_REPARSE_MISSING.Checked
                    .UseDefaultAccountIfMissing.Value = CH_USE_DEF_ACC.Checked
                    .AutomationBrushUndownloadedPlansMinutes.Value = TXT_AUTO_BRUSH_MIN.Value
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
                    .FeedCenterImage.Use = TXT_FEED_CENTER_IMAGE.Checked
                    .FeedCenterImage.Value = TXT_FEED_CENTER_IMAGE.Value
                    .FromChannelDownloadTop.Value = CInt(TXT_CHANNEL_USER_POST_LIMIT.Value)
                    .FromChannelDownloadTopUse.Value = TXT_CHANNEL_USER_POST_LIMIT.Checked
                    .FromChannelCopyImageToUser.Value = CH_COPY_CHANNEL_USER_IMAGE.Checked
                    .ChannelsAddUserImagesFromAllChannels.Value = CH_COPY_CHANNEL_USER_IMAGE_ALL.Checked
                    .ChannelsDefaultTemporary.Value = CH_CHANNELS_USERS_TEMP.Checked
                    .ChannelsDefaultReadyForDownload.Value = CH_CHANNELS_USERS_READY_MARK.Checked
                    'Feed
                    .FeedDataRows.Value = CInt(TXT_FEED_ROWS.Value)
                    .FeedDataColumns.Value = CInt(TXT_FEED_COLUMNS.Value)
                    COLORS_FEED.ColorsGet(.FeedBackColor, .FeedForeColor)
                    .FeedEndless.Value = CH_FEED_ENDLESS.Checked
                    .FeedAddSessionToCaption.Value = CH_FEED_ADD_SESSION.Checked
                    .FeedAddSiteToCaption.Value = CH_FEED_ADD_SITE.Checked
                    .FeedAddTypeToCaption.Value = CH_FEED_ADD_TYPE.Checked
                    .FeedAddDateToCaption.Value = CH_FEED_ADD_DATE.Checked
                    .FeedStoreSessionsData.Value = NUM_FEED_STORE_SESSION_DATA.Checked
                    .FeedStoredSessionsNumber.Value = NUM_FEED_STORE_SESSION_DATA.Value
                    .FeedCurrentTryLoadLastSession.Value = NUM_FEED_SES_CURR_LOAD_LAST.Value
                    .FeedOpenLastMode.Value = CH_FEED_OPEN_LAST_MODE.Checked
                    .FeedShowFriendlyNames.Value = CH_FEED_SHOW_FRIENDLY.Checked
                    .FeedShowSpecialFeedsMediaItem.Value = CH_FEED_SHOW_SPEC_MEDIAITEM.Checked
                    .FeedMoveCopyUpdateFileLocationOnMove.Value = CH_FEED_UP_FILE_LOC_MOVE.Checked
                    .FeedEscToClose.Value = CH_FEED_ESC_TO_CLOSE.Checked
                    .FeedSpecialSearchForMissing.Value = CH_FEED_SPEC_SEARCH.Checked
                    .FeedSpecialSearchForMissing_Deep.Value = CH_FEED_SPEC_SEARCH_DEEP.Checked
                    FeedParametersChanged = { .FeedDataRows, .FeedDataColumns, .FeedEndless, .FeedBackColor,
                                              .FeedForeColor, .FeedCenterImage}.Cast(Of IXMLValue).Any(detector)

                    .EndUpdate()
                End With
                MyDefs.CloseForm()
            End If
        End Sub
        Private Sub TXT_GLOBAL_PATH_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As EventArgs) Handles TXT_GLOBAL_PATH.ActionOnButtonClick
            If Sender.DefaultButton = ADB.Open Then
                Dim f As SFile = SFile.SelectPath(Settings.GlobalPath.Value).IfNullOrEmpty(Settings.GlobalPath.Value)
                If Not f.IsEmptyString Then TXT_GLOBAL_PATH.Text = f
            End If
        End Sub
        Private Sub TXT_MAX_JOBS_USERS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As EventArgs) Handles TXT_MAX_JOBS_USERS.ActionOnButtonClick
            If Sender.DefaultButton = ADB.Refresh Then TXT_MAX_JOBS_USERS.Value = SettingsCLS.DefaultMaxDownloadingTasks
        End Sub
        Private Sub TXT_MAX_JOBS_CHANNELS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As EventArgs) Handles TXT_MAX_JOBS_CHANNELS.ActionOnButtonClick
            If Sender.DefaultButton = ADB.Refresh Then TXT_MAX_JOBS_CHANNELS.Value = SettingsCLS.DefaultMaxDownloadingTasks
        End Sub
        Private Sub ChangePositionControlsEnabling() Handles OPT_FILE_NAME_REPLACE.CheckedChanged, OPT_FILE_NAME_ADD_DATE.CheckedChanged
            Dim b As Boolean = OPT_FILE_NAME_ADD_DATE.Checked And OPT_FILE_NAME_ADD_DATE.Enabled
            OPT_FILE_DATE_START.Enabled = b
            OPT_FILE_DATE_END.Enabled = b
        End Sub
        Private Sub ChangeFileNameChangersEnabling() Handles CH_FILE_NAME_CHANGE.CheckedChanged
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
        Private Sub CH_NOTIFY_SHOW_BASE_CheckedChanged(sender As Object, e As EventArgs) Handles CH_NOTIFY_SHOW_BASE.CheckedChanged
            Dim b As Boolean = CH_NOTIFY_SHOW_BASE.Checked
            CH_NOTIFY_PROFILES.Enabled = b
            CH_NOTIFY_AUTO_DOWN.Enabled = b
            CH_NOTIFY_CHANNELS.Enabled = b
            CH_NOTIFY_SAVED_POSTS.Enabled = b
        End Sub
        Private Sub TXT_USER_LIST_IMAGE_ActionOnButtonClick(ByVal Sender As ActionButton, e As ActionButtonEventArgs) Handles TXT_USER_LIST_IMAGE.ActionOnButtonClick
            If Sender.DefaultButton = ADB.Open Then
                Dim f As SFile = SFile.SelectFiles(TXT_USER_LIST_IMAGE.Text, False, "Select a new image for the user list:", "Pictures|*.jpg;*.jpeg;*.png").FirstOrDefault
                If Not f.IsEmptyString Then TXT_USER_LIST_IMAGE.Text = f
            End If
        End Sub
        Private Sub TXT_FEED_COLUMNS_ActionOnValueChanged(sender As Object, e As EventArgs) Handles TXT_FEED_COLUMNS.ActionOnValueChanged
            TXT_FEED_CENTER_IMAGE.Enabled = TXT_FEED_COLUMNS.Value = 1
        End Sub
        Private Sub NUM_FEED_SES_CURR_LOAD_LAST_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles NUM_FEED_SES_CURR_LOAD_LAST.ActionOnButtonClick
            Select Case e.DefaultButton
                Case ADB.Clear : NUM_FEED_SES_CURR_LOAD_LAST.Value = -1
                Case ADB.Refresh : NUM_FEED_SES_CURR_LOAD_LAST.Value = SettingsCLS.FeedCurrentLoadLastSessionDefault
            End Select
        End Sub
        Private Sub EnvirPrograms_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles TXT_FFMPEG.ActionOnButtonClick,
                                                                                                                              TXT_CURL.ActionOnButtonClick,
                                                                                                                              TXT_YTDLP.ActionOnButtonClick,
                                                                                                                              TXT_GALLERYDL.ActionOnButtonClick
            If (e.DefaultButton = ADB.Open Or e.DefaultButton = ADB.Refresh) And Not e.AssociatedControl Is Nothing Then
                Dim __chooseNewFile As Action(Of TextBoxExtended, String) =
                    Sub(ByVal cnt As TextBoxExtended, ByVal __name As String)
                        Dim f As SFile = cnt.Text
                        f = SFile.SelectFiles(f, False, $"Select the {__name} program file.", "EXE|*.exe", EDP.ReturnValue).FirstOrDefault
                        If Not f.IsEmptyString Then cnt.Text = f
                    End Sub
                Dim __findEnvir As Action(Of TextBoxExtended, String, String) =
                    Sub(ByVal cnt As TextBoxExtended, ByVal program As String, ByVal additPath As String)
                        Dim f As SFile = SettingsCLS.ProgramFile.Find(program, additPath)
                        If Not f.IsEmptyString Then
                            cnt.Text = f
                        Else
                            MsgBoxE({$"'{program}' not found in the environment!", "Find a program..."}, vbCritical)
                        End If
                    End Sub
                Dim __autoDecision As Action(Of TextBoxExtended, String, String, String) =
                    Sub(ByVal cnt As TextBoxExtended, ByVal __name As String, ByVal program As String, ByVal additPath As String)
                        If e.DefaultButton = ADB.Open Then
                            __chooseNewFile.Invoke(cnt, __name)
                        ElseIf e.DefaultButton = ADB.Refresh Then
                            __findEnvir.Invoke(cnt, program, additPath)
                        End If
                    End Sub
                Select Case CStr(DirectCast(e.AssociatedControl, Control).Tag)
                    Case "f" : __autoDecision.Invoke(e.AssociatedControl, "ffmpeg", SettingsCLS.ProgramFile.File_FFMPEG, String.Empty)
                    Case "c" : __autoDecision.Invoke(e.AssociatedControl, "curl", SettingsCLS.ProgramFile.File_CURL, SettingsCLS.ProgramFile.File_CIRL_ADDIT_PATH)
                    Case "y" : __autoDecision.Invoke(e.AssociatedControl, "yt-dlp", SettingsCLS.ProgramFile.File_YTDLP, String.Empty)
                    Case "g" : __autoDecision.Invoke(e.AssociatedControl, "gallery-dl", SettingsCLS.ProgramFile.File_GDL, String.Empty)
                End Select
            End If
        End Sub
        Private Sub TXT_CMD_ENCODING_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles TXT_CMD_ENCODING.ActionOnButtonClick
            If Sender.DefaultButton = ADB.Refresh Then TXT_CMD_ENCODING.Text = SettingsCLS.DefaultCmdEncoding
        End Sub
        Private Sub BTT_RESET_DOWNLOAD_LOCATIONS_Click(sender As Object, e As EventArgs) Handles BTT_RESET_DOWNLOAD_LOCATIONS.Click
            Try
                Const msgTitle$ = "Reset download locations"
                If Settings.DownloadLocations.Count = 0 Then
                    MsgBoxE({"There are no saved download locations.", msgTitle})
                ElseIf MsgBoxE({$"Are you sure you want to delete all ({Settings.DownloadLocations.Count}) download locations?", msgTitle},
                               vbExclamation + vbYesNo) = vbYes Then
                    Settings.DownloadLocations.Clear()
                    MsgBoxE({"All download locations deleted.", msgTitle})
                End If
            Catch
            End Try
        End Sub
    End Class
End Namespace