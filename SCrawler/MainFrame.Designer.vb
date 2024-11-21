' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class MainFrame : Inherits System.Windows.Forms.Form
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
    Private components As System.ComponentModel.IContainer
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim SEP_1 As System.Windows.Forms.ToolStripSeparator
        Dim SEP_2 As System.Windows.Forms.ToolStripSeparator
        Dim CONTEXT_SEP_1 As System.Windows.Forms.ToolStripSeparator
        Dim MENU_SETTINGS_SEP_1 As System.Windows.Forms.ToolStripSeparator
        Dim SEP_3 As System.Windows.Forms.ToolStripSeparator
        Dim CONTEXT_SEP_2 As System.Windows.Forms.ToolStripSeparator
        Dim CONTEXT_SEP_3 As System.Windows.Forms.ToolStripSeparator
        Dim CONTEXT_SEP_4 As System.Windows.Forms.ToolStripSeparator
        Dim CONTEXT_SEP_5 As System.Windows.Forms.ToolStripSeparator
        Dim SEP_4 As System.Windows.Forms.ToolStripSeparator
        Dim MENU_VIEW_SEP_2 As System.Windows.Forms.ToolStripSeparator
        Dim TRAY_SEP_1 As System.Windows.Forms.ToolStripSeparator
        Dim MENU_DOWN_ALL_SEP_1 As System.Windows.Forms.ToolStripSeparator
        Dim TRAY_SEP_2 As System.Windows.Forms.ToolStripSeparator
        Dim MENU_DOWN_ALL_SEP_2 As System.Windows.Forms.ToolStripSeparator
        Dim MENU_DOWN_ALL_SEP_3 As System.Windows.Forms.ToolStripSeparator
        Dim MENU_DOWN_ALL_SEP_4 As System.Windows.Forms.ToolStripSeparator
        Dim MENU_INFO As System.Windows.Forms.ToolStripDropDownButton
        Dim MENU_INFO_SEP_1 As System.Windows.Forms.ToolStripSeparator
        Dim MENU_VIEW_SEP_1 As System.Windows.Forms.ToolStripSeparator
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainFrame))
        Me.MENU_INFO_SHOW_INFO = New System.Windows.Forms.ToolStripMenuItem()
        Me.MENU_INFO_SHOW_QUEUE = New System.Windows.Forms.ToolStripMenuItem()
        Me.MENU_INFO_SHOW_MISSING = New System.Windows.Forms.ToolStripMenuItem()
        Me.MENU_INFO_SHOW_USER_METRICS = New System.Windows.Forms.ToolStripMenuItem()
        Me.MENU_INFO_USER_SEARCH = New System.Windows.Forms.ToolStripMenuItem()
        Me.MENU_SETTINGS = New System.Windows.Forms.ToolStripDropDownButton()
        Me.BTT_SETTINGS = New System.Windows.Forms.ToolStripMenuItem()
        Me.Toolbar_TOP = New System.Windows.Forms.ToolStrip()
        Me.BTT_ADD_USER = New PersonalUtilities.Forms.Controls.KeyClick.ToolStripButtonKeyClick()
        Me.BTT_EDIT_USER = New System.Windows.Forms.ToolStripButton()
        Me.BTT_DELETE_USER = New System.Windows.Forms.ToolStripButton()
        Me.BTT_REFRESH = New System.Windows.Forms.ToolStripButton()
        Me.BTT_FEED = New System.Windows.Forms.ToolStripButton()
        Me.BTT_CHANNELS = New System.Windows.Forms.ToolStripButton()
        Me.BTT_DOWN_SAVED = New System.Windows.Forms.ToolStripButton()
        Me.MENU_DOWN_ALL = New System.Windows.Forms.ToolStripDropDownButton()
        Me.BTT_DOWN_SELECTED = New SCrawler.ToolStripKeyMenuItem()
        Me.BTT_DOWN_ALL = New SCrawler.ToolStripKeyMenuItem()
        Me.BTT_DOWN_ALL_FULL = New SCrawler.ToolStripKeyMenuItem()
        Me.BTT_DOWN_SPEC = New SCrawler.ToolStripKeyMenuItem()
        Me.BTT_DOWN_VIDEO = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_ADD_NEW_GROUP = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_SILENT_MODE = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_DOWN_AUTOMATION = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_DOWN_AUTOMATION_PAUSE = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_DOWN_STOP = New System.Windows.Forms.ToolStripButton()
        Me.MENU_VIEW = New System.Windows.Forms.ToolStripDropDownButton()
        Me.BTT_VIEW_LARGE = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_VIEW_SMALL = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_VIEW_LIST = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_VIEW_DETAILS = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_SHOW_ALL = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_SHOW_SHOW_GROUPS = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_SHOW_GROUP_USERS = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_SHOW_FILTER_ADV = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_VIEW_FILTER_SAVE = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_VIEW_FILTER_SAVE_AS_GROUP = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_VIEW_FILTER_LOAD = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_LOG = New System.Windows.Forms.ToolStripButton()
        Me.BTT_VERSION_INFO = New System.Windows.Forms.ToolStripButton()
        Me.BTT_DONATE = New System.Windows.Forms.ToolStripButton()
        Me.BTT_BUG_REPORT = New System.Windows.Forms.ToolStripButton()
        Me.Toolbar_BOTTOM = New System.Windows.Forms.StatusStrip()
        Me.BTT_PR_INFO = New System.Windows.Forms.ToolStripStatusLabel()
        Me.PR_PRE = New System.Windows.Forms.ToolStripProgressBar()
        Me.PR_MAIN = New System.Windows.Forms.ToolStripProgressBar()
        Me.LBL_JOBS_COUNT = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LBL_STATUS = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LIST_PROFILES = New System.Windows.Forms.ListView()
        Me.COL_DEF = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.USER_CONTEXT = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.BTT_CONTEXT_DOWN = New SCrawler.ToolStripKeyMenuItem()
        Me.BTT_CONTEXT_DOWN_LIMITED = New SCrawler.ToolStripKeyMenuItem()
        Me.BTT_CONTEXT_DOWN_DATE_LIMIT = New SCrawler.ToolStripKeyMenuItem()
        Me.BTT_CONTEXT_EDIT = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_CONTEXT_DELETE = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_CONTEXT_ERASE = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_CONTEXT_COPY_TO_FOLDER = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_CONTEXT_FAV = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_CONTEXT_TEMP = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_CONTEXT_READY = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_CONTEXT_GROUPS = New PersonalUtilities.Forms.Controls.KeyClick.ToolStripMenuItemKeyClick()
        Me.BTT_CONTEXT_SCRIPT = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_CONTEXT_ADD_TO_COL = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_CONTEXT_COL_MERGE = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_CONTEXT_CHANGE_FOLDER = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_CHANGE_IMAGE = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_CONTEXT_OPEN_PATH = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_CONTEXT_OPEN_SITE = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_CONTEXT_INFO = New System.Windows.Forms.ToolStripMenuItem()
        Me.TrayIcon = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.TRAY_CONTEXT = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.BTT_TRAY_PAUSE_AUTOMATION = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_TRAY_SILENT_MODE = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_TRAY_FEED_SHOW = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_TRAY_CHANNELS = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_TRAY_DOWNLOADER = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_TRAY_SCHEDULER = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_TRAY_SHOW_HIDE = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_TRAY_CLOSE = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_TRAY_CLOSE_NO_SCRIPT = New System.Windows.Forms.ToolStripMenuItem()
        SEP_1 = New System.Windows.Forms.ToolStripSeparator()
        SEP_2 = New System.Windows.Forms.ToolStripSeparator()
        CONTEXT_SEP_1 = New System.Windows.Forms.ToolStripSeparator()
        MENU_SETTINGS_SEP_1 = New System.Windows.Forms.ToolStripSeparator()
        SEP_3 = New System.Windows.Forms.ToolStripSeparator()
        CONTEXT_SEP_2 = New System.Windows.Forms.ToolStripSeparator()
        CONTEXT_SEP_3 = New System.Windows.Forms.ToolStripSeparator()
        CONTEXT_SEP_4 = New System.Windows.Forms.ToolStripSeparator()
        CONTEXT_SEP_5 = New System.Windows.Forms.ToolStripSeparator()
        SEP_4 = New System.Windows.Forms.ToolStripSeparator()
        MENU_VIEW_SEP_2 = New System.Windows.Forms.ToolStripSeparator()
        TRAY_SEP_1 = New System.Windows.Forms.ToolStripSeparator()
        MENU_DOWN_ALL_SEP_1 = New System.Windows.Forms.ToolStripSeparator()
        TRAY_SEP_2 = New System.Windows.Forms.ToolStripSeparator()
        MENU_DOWN_ALL_SEP_2 = New System.Windows.Forms.ToolStripSeparator()
        MENU_DOWN_ALL_SEP_3 = New System.Windows.Forms.ToolStripSeparator()
        MENU_DOWN_ALL_SEP_4 = New System.Windows.Forms.ToolStripSeparator()
        MENU_INFO = New System.Windows.Forms.ToolStripDropDownButton()
        MENU_INFO_SEP_1 = New System.Windows.Forms.ToolStripSeparator()
        MENU_VIEW_SEP_1 = New System.Windows.Forms.ToolStripSeparator()
        Me.Toolbar_TOP.SuspendLayout()
        Me.Toolbar_BOTTOM.SuspendLayout()
        Me.USER_CONTEXT.SuspendLayout()
        Me.TRAY_CONTEXT.SuspendLayout()
        Me.SuspendLayout()
        '
        'SEP_1
        '
        SEP_1.Name = "SEP_1"
        SEP_1.Size = New System.Drawing.Size(6, 25)
        '
        'SEP_2
        '
        SEP_2.Name = "SEP_2"
        SEP_2.Size = New System.Drawing.Size(6, 25)
        '
        'CONTEXT_SEP_1
        '
        CONTEXT_SEP_1.Name = "CONTEXT_SEP_1"
        CONTEXT_SEP_1.Size = New System.Drawing.Size(218, 6)
        '
        'MENU_SETTINGS_SEP_1
        '
        MENU_SETTINGS_SEP_1.Name = "MENU_SETTINGS_SEP_1"
        MENU_SETTINGS_SEP_1.Size = New System.Drawing.Size(113, 6)
        '
        'SEP_3
        '
        SEP_3.Name = "SEP_3"
        SEP_3.Size = New System.Drawing.Size(6, 25)
        '
        'CONTEXT_SEP_2
        '
        CONTEXT_SEP_2.Name = "CONTEXT_SEP_2"
        CONTEXT_SEP_2.Size = New System.Drawing.Size(218, 6)
        '
        'CONTEXT_SEP_3
        '
        CONTEXT_SEP_3.Name = "CONTEXT_SEP_3"
        CONTEXT_SEP_3.Size = New System.Drawing.Size(218, 6)
        '
        'CONTEXT_SEP_4
        '
        CONTEXT_SEP_4.Name = "CONTEXT_SEP_4"
        CONTEXT_SEP_4.Size = New System.Drawing.Size(218, 6)
        '
        'CONTEXT_SEP_5
        '
        CONTEXT_SEP_5.Name = "CONTEXT_SEP_5"
        CONTEXT_SEP_5.Size = New System.Drawing.Size(218, 6)
        '
        'SEP_4
        '
        SEP_4.Name = "SEP_4"
        SEP_4.Size = New System.Drawing.Size(6, 25)
        '
        'MENU_VIEW_SEP_2
        '
        MENU_VIEW_SEP_2.Name = "MENU_VIEW_SEP_2"
        MENU_VIEW_SEP_2.Size = New System.Drawing.Size(268, 6)
        '
        'TRAY_SEP_1
        '
        TRAY_SEP_1.Name = "TRAY_SEP_1"
        TRAY_SEP_1.Size = New System.Drawing.Size(167, 6)
        '
        'MENU_DOWN_ALL_SEP_1
        '
        MENU_DOWN_ALL_SEP_1.Name = "MENU_DOWN_ALL_SEP_1"
        MENU_DOWN_ALL_SEP_1.Size = New System.Drawing.Size(218, 6)
        '
        'TRAY_SEP_2
        '
        TRAY_SEP_2.Name = "TRAY_SEP_2"
        TRAY_SEP_2.Size = New System.Drawing.Size(167, 6)
        '
        'MENU_DOWN_ALL_SEP_2
        '
        MENU_DOWN_ALL_SEP_2.Name = "MENU_DOWN_ALL_SEP_2"
        MENU_DOWN_ALL_SEP_2.Size = New System.Drawing.Size(218, 6)
        '
        'MENU_DOWN_ALL_SEP_3
        '
        MENU_DOWN_ALL_SEP_3.Name = "MENU_DOWN_ALL_SEP_3"
        MENU_DOWN_ALL_SEP_3.Size = New System.Drawing.Size(218, 6)
        '
        'MENU_DOWN_ALL_SEP_4
        '
        MENU_DOWN_ALL_SEP_4.Name = "MENU_DOWN_ALL_SEP_4"
        MENU_DOWN_ALL_SEP_4.Size = New System.Drawing.Size(218, 6)
        '
        'MENU_INFO
        '
        MENU_INFO.AutoToolTip = False
        MENU_INFO.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MENU_INFO_SHOW_INFO, Me.MENU_INFO_SHOW_QUEUE, Me.MENU_INFO_SHOW_MISSING, MENU_INFO_SEP_1, Me.MENU_INFO_SHOW_USER_METRICS, Me.MENU_INFO_USER_SEARCH})
        MENU_INFO.Image = Global.SCrawler.My.Resources.Resources.InfoPic_32
        MENU_INFO.ImageTransparentColor = System.Drawing.Color.Magenta
        MENU_INFO.Name = "MENU_INFO"
        MENU_INFO.Size = New System.Drawing.Size(57, 22)
        MENU_INFO.Text = "Info"
        '
        'MENU_INFO_SHOW_INFO
        '
        Me.MENU_INFO_SHOW_INFO.AutoToolTip = True
        Me.MENU_INFO_SHOW_INFO.Image = Global.SCrawler.My.Resources.Resources.InfoPic_32
        Me.MENU_INFO_SHOW_INFO.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.MENU_INFO_SHOW_INFO.Name = "MENU_INFO_SHOW_INFO"
        Me.MENU_INFO_SHOW_INFO.Size = New System.Drawing.Size(212, 22)
        Me.MENU_INFO_SHOW_INFO.Text = "Info (download summary)"
        Me.MENU_INFO_SHOW_INFO.ToolTipText = "Open the 'Info' form (show download summary)."
        '
        'MENU_INFO_SHOW_QUEUE
        '
        Me.MENU_INFO_SHOW_QUEUE.Image = Global.SCrawler.My.Resources.Resources.InfoPic_32
        Me.MENU_INFO_SHOW_QUEUE.Name = "MENU_INFO_SHOW_QUEUE"
        Me.MENU_INFO_SHOW_QUEUE.Size = New System.Drawing.Size(212, 22)
        Me.MENU_INFO_SHOW_QUEUE.Text = "Users download queue"
        '
        'MENU_INFO_SHOW_MISSING
        '
        Me.MENU_INFO_SHOW_MISSING.AutoToolTip = True
        Me.MENU_INFO_SHOW_MISSING.Image = Global.SCrawler.My.Resources.Resources.InfoPic_32
        Me.MENU_INFO_SHOW_MISSING.Name = "MENU_INFO_SHOW_MISSING"
        Me.MENU_INFO_SHOW_MISSING.Size = New System.Drawing.Size(212, 22)
        Me.MENU_INFO_SHOW_MISSING.Text = "Missing posts"
        Me.MENU_INFO_SHOW_MISSING.ToolTipText = "Open the 'Missing' form (show information about missing posts)."
        '
        'MENU_INFO_SEP_1
        '
        MENU_INFO_SEP_1.Name = "MENU_INFO_SEP_1"
        MENU_INFO_SEP_1.Size = New System.Drawing.Size(209, 6)
        '
        'MENU_INFO_SHOW_USER_METRICS
        '
        Me.MENU_INFO_SHOW_USER_METRICS.AutoToolTip = True
        Me.MENU_INFO_SHOW_USER_METRICS.Image = Global.SCrawler.My.Resources.Resources.InfoPic_32
        Me.MENU_INFO_SHOW_USER_METRICS.Name = "MENU_INFO_SHOW_USER_METRICS"
        Me.MENU_INFO_SHOW_USER_METRICS.Size = New System.Drawing.Size(212, 22)
        Me.MENU_INFO_SHOW_USER_METRICS.Text = "User metrics"
        Me.MENU_INFO_SHOW_USER_METRICS.ToolTipText = "Open the ""User metrics' form (show information about the user's metrics (such as " &
    "size, number of files, etc.))."
        '
        'MENU_INFO_USER_SEARCH
        '
        Me.MENU_INFO_USER_SEARCH.Image = Global.SCrawler.My.Resources.Resources.FindPic_16
        Me.MENU_INFO_USER_SEARCH.Name = "MENU_INFO_USER_SEARCH"
        Me.MENU_INFO_USER_SEARCH.Size = New System.Drawing.Size(212, 22)
        Me.MENU_INFO_USER_SEARCH.Text = "Find user"
        '
        'MENU_VIEW_SEP_1
        '
        MENU_VIEW_SEP_1.Name = "MENU_VIEW_SEP_1"
        MENU_VIEW_SEP_1.Size = New System.Drawing.Size(268, 6)
        '
        'MENU_SETTINGS
        '
        Me.MENU_SETTINGS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.MENU_SETTINGS.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {MENU_SETTINGS_SEP_1, Me.BTT_SETTINGS})
        Me.MENU_SETTINGS.Image = Global.SCrawler.My.Resources.Resources.SettingsPic_16
        Me.MENU_SETTINGS.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.MENU_SETTINGS.Name = "MENU_SETTINGS"
        Me.MENU_SETTINGS.Size = New System.Drawing.Size(29, 22)
        Me.MENU_SETTINGS.Text = "Settings"
        '
        'BTT_SETTINGS
        '
        Me.BTT_SETTINGS.Image = Global.SCrawler.My.Resources.Resources.SettingsPic_16
        Me.BTT_SETTINGS.Name = "BTT_SETTINGS"
        Me.BTT_SETTINGS.Size = New System.Drawing.Size(116, 22)
        Me.BTT_SETTINGS.Text = "Settings"
        '
        'Toolbar_TOP
        '
        Me.Toolbar_TOP.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.Toolbar_TOP.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MENU_SETTINGS, SEP_1, Me.BTT_ADD_USER, Me.BTT_EDIT_USER, Me.BTT_DELETE_USER, Me.BTT_REFRESH, MENU_INFO, Me.BTT_FEED, Me.BTT_CHANNELS, Me.BTT_DOWN_SAVED, SEP_2, Me.MENU_DOWN_ALL, Me.BTT_DOWN_STOP, SEP_3, Me.MENU_VIEW, SEP_4, Me.BTT_LOG, Me.BTT_VERSION_INFO, Me.BTT_DONATE, Me.BTT_BUG_REPORT})
        Me.Toolbar_TOP.Location = New System.Drawing.Point(0, 0)
        Me.Toolbar_TOP.Name = "Toolbar_TOP"
        Me.Toolbar_TOP.Size = New System.Drawing.Size(934, 25)
        Me.Toolbar_TOP.TabIndex = 1
        '
        'BTT_ADD_USER
        '
        Me.BTT_ADD_USER.Image = Global.SCrawler.My.Resources.Resources.PlusPic_24
        Me.BTT_ADD_USER.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_ADD_USER.Name = "BTT_ADD_USER"
        Me.BTT_ADD_USER.Size = New System.Drawing.Size(49, 22)
        Me.BTT_ADD_USER.Text = "Add"
        Me.BTT_ADD_USER.ToolTipText = "Add user (Insert)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Ctrl+Insert to add user by URL from buffer without opening for" &
    "m"
        '
        'BTT_EDIT_USER
        '
        Me.BTT_EDIT_USER.Image = Global.SCrawler.My.Resources.Resources.PencilPic_16
        Me.BTT_EDIT_USER.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_EDIT_USER.Name = "BTT_EDIT_USER"
        Me.BTT_EDIT_USER.Size = New System.Drawing.Size(47, 22)
        Me.BTT_EDIT_USER.Text = "Edit"
        Me.BTT_EDIT_USER.ToolTipText = "Edit user (F3)"
        '
        'BTT_DELETE_USER
        '
        Me.BTT_DELETE_USER.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
        Me.BTT_DELETE_USER.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_DELETE_USER.Name = "BTT_DELETE_USER"
        Me.BTT_DELETE_USER.Size = New System.Drawing.Size(60, 22)
        Me.BTT_DELETE_USER.Text = "Delete"
        Me.BTT_DELETE_USER.ToolTipText = "Delete user (Del)"
        '
        'BTT_REFRESH
        '
        Me.BTT_REFRESH.Image = Global.SCrawler.My.Resources.Resources.RefreshPic_24
        Me.BTT_REFRESH.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_REFRESH.Name = "BTT_REFRESH"
        Me.BTT_REFRESH.Size = New System.Drawing.Size(66, 22)
        Me.BTT_REFRESH.Text = "Refresh"
        Me.BTT_REFRESH.ToolTipText = "Refresh user list"
        '
        'BTT_FEED
        '
        Me.BTT_FEED.Image = Global.SCrawler.My.Resources.Resources.RSSPic_512
        Me.BTT_FEED.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_FEED.Name = "BTT_FEED"
        Me.BTT_FEED.Size = New System.Drawing.Size(52, 22)
        Me.BTT_FEED.Text = "Feed"
        Me.BTT_FEED.ToolTipText = "Feed of recently downloaded data (Ctrl+F)"
        '
        'BTT_CHANNELS
        '
        Me.BTT_CHANNELS.AutoToolTip = False
        Me.BTT_CHANNELS.Image = Global.SCrawler.My.Resources.SiteResources.RedditPic_512
        Me.BTT_CHANNELS.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_CHANNELS.Name = "BTT_CHANNELS"
        Me.BTT_CHANNELS.Size = New System.Drawing.Size(76, 22)
        Me.BTT_CHANNELS.Text = "Channels"
        '
        'BTT_DOWN_SAVED
        '
        Me.BTT_DOWN_SAVED.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BTT_DOWN_SAVED.Image = Global.SCrawler.My.Resources.Resources.BookmarkBlack_16
        Me.BTT_DOWN_SAVED.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_DOWN_SAVED.Name = "BTT_DOWN_SAVED"
        Me.BTT_DOWN_SAVED.Size = New System.Drawing.Size(23, 22)
        Me.BTT_DOWN_SAVED.ToolTipText = "Download saved posts"
        '
        'MENU_DOWN_ALL
        '
        Me.MENU_DOWN_ALL.AutoToolTip = False
        Me.MENU_DOWN_ALL.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_DOWN_SELECTED, Me.BTT_DOWN_ALL, Me.BTT_DOWN_ALL_FULL, Me.BTT_DOWN_SPEC, MENU_DOWN_ALL_SEP_1, Me.BTT_DOWN_VIDEO, MENU_DOWN_ALL_SEP_2, Me.BTT_ADD_NEW_GROUP, MENU_DOWN_ALL_SEP_3, Me.BTT_SILENT_MODE, MENU_DOWN_ALL_SEP_4, Me.BTT_DOWN_AUTOMATION, Me.BTT_DOWN_AUTOMATION_PAUSE})
        Me.MENU_DOWN_ALL.Image = Global.SCrawler.My.Resources.Resources.StartPic_Green_16
        Me.MENU_DOWN_ALL.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.MENU_DOWN_ALL.Name = "MENU_DOWN_ALL"
        Me.MENU_DOWN_ALL.Size = New System.Drawing.Size(90, 22)
        Me.MENU_DOWN_ALL.Text = "Download"
        '
        'BTT_DOWN_SELECTED
        '
        Me.BTT_DOWN_SELECTED.AutoToolTip = True
        Me.BTT_DOWN_SELECTED.Image = Global.SCrawler.My.Resources.Resources.StartPic_Green_16
        Me.BTT_DOWN_SELECTED.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_DOWN_SELECTED.Name = "BTT_DOWN_SELECTED"
        Me.BTT_DOWN_SELECTED.Size = New System.Drawing.Size(221, 22)
        Me.BTT_DOWN_SELECTED.Text = "Download selected (F5)"
        Me.BTT_DOWN_SELECTED.ToolTipText = "Download selected user." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "F5: download, include in the feed."
        '
        'BTT_DOWN_ALL
        '
        Me.BTT_DOWN_ALL.AutoToolTip = True
        Me.BTT_DOWN_ALL.Image = Global.SCrawler.My.Resources.Resources.StartPic_Green_16
        Me.BTT_DOWN_ALL.Name = "BTT_DOWN_ALL"
        Me.BTT_DOWN_ALL.Size = New System.Drawing.Size(221, 22)
        Me.BTT_DOWN_ALL.Text = "Download all (F6)"
        Me.BTT_DOWN_ALL.ToolTipText = "Download all users marked 'Ready for download' from all sites." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "F6: download, inc" &
    "lude in the feed."
        '
        'BTT_DOWN_ALL_FULL
        '
        Me.BTT_DOWN_ALL_FULL.AutoToolTip = True
        Me.BTT_DOWN_ALL_FULL.Image = Global.SCrawler.My.Resources.Resources.StartPic_Green_16
        Me.BTT_DOWN_ALL_FULL.Name = "BTT_DOWN_ALL_FULL"
        Me.BTT_DOWN_ALL_FULL.Size = New System.Drawing.Size(221, 22)
        Me.BTT_DOWN_ALL_FULL.Text = "Download all [FULL]"
        Me.BTT_DOWN_ALL_FULL.ToolTipText = "Download all users from all sites. The 'Ready for download' option will be ignore" &
    "d."
        '
        'BTT_DOWN_SPEC
        '
        Me.BTT_DOWN_SPEC.AutoToolTip = True
        Me.BTT_DOWN_SPEC.Image = Global.SCrawler.My.Resources.Resources.StartPic_Green_16
        Me.BTT_DOWN_SPEC.Name = "BTT_DOWN_SPEC"
        Me.BTT_DOWN_SPEC.Size = New System.Drawing.Size(221, 22)
        Me.BTT_DOWN_SPEC.Text = "Download (advanced)"
        Me.BTT_DOWN_SPEC.ToolTipText = "Filter the users you want to download and download them."
        '
        'BTT_DOWN_VIDEO
        '
        Me.BTT_DOWN_VIDEO.AutoToolTip = True
        Me.BTT_DOWN_VIDEO.Image = Global.SCrawler.My.Resources.Resources.ArrowDownPic_Blue_24
        Me.BTT_DOWN_VIDEO.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_DOWN_VIDEO.Name = "BTT_DOWN_VIDEO"
        Me.BTT_DOWN_VIDEO.Size = New System.Drawing.Size(221, 22)
        Me.BTT_DOWN_VIDEO.Text = "Standalone downloader"
        Me.BTT_DOWN_VIDEO.ToolTipText = "Download video by URL"
        '
        'BTT_ADD_NEW_GROUP
        '
        Me.BTT_ADD_NEW_GROUP.Image = Global.SCrawler.My.Resources.Resources.PlusPic_24
        Me.BTT_ADD_NEW_GROUP.Name = "BTT_ADD_NEW_GROUP"
        Me.BTT_ADD_NEW_GROUP.Size = New System.Drawing.Size(221, 22)
        Me.BTT_ADD_NEW_GROUP.Text = "Add a new download group"
        '
        'BTT_SILENT_MODE
        '
        Me.BTT_SILENT_MODE.AutoToolTip = True
        Me.BTT_SILENT_MODE.Image = Global.SCrawler.My.Resources.Resources.MessagePic_16
        Me.BTT_SILENT_MODE.Name = "BTT_SILENT_MODE"
        Me.BTT_SILENT_MODE.Size = New System.Drawing.Size(221, 22)
        Me.BTT_SILENT_MODE.Text = "Silent mode"
        Me.BTT_SILENT_MODE.ToolTipText = "Turn off notifications temporarily." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This setting is not stored in the settings f" &
    "ile. It is valid until you turn it off or close the program." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'BTT_DOWN_AUTOMATION
        '
        Me.BTT_DOWN_AUTOMATION.Image = Global.SCrawler.My.Resources.Resources.ScriptPic_32
        Me.BTT_DOWN_AUTOMATION.Name = "BTT_DOWN_AUTOMATION"
        Me.BTT_DOWN_AUTOMATION.Size = New System.Drawing.Size(221, 22)
        Me.BTT_DOWN_AUTOMATION.Text = "Automation"
        '
        'BTT_DOWN_AUTOMATION_PAUSE
        '
        Me.BTT_DOWN_AUTOMATION_PAUSE.Image = Global.SCrawler.My.Resources.Resources.Pause_Blue_16
        Me.BTT_DOWN_AUTOMATION_PAUSE.Name = "BTT_DOWN_AUTOMATION_PAUSE"
        Me.BTT_DOWN_AUTOMATION_PAUSE.Size = New System.Drawing.Size(221, 22)
        Me.BTT_DOWN_AUTOMATION_PAUSE.Text = "Pause automation"
        '
        'BTT_DOWN_STOP
        '
        Me.BTT_DOWN_STOP.AutoToolTip = False
        Me.BTT_DOWN_STOP.Enabled = False
        Me.BTT_DOWN_STOP.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
        Me.BTT_DOWN_STOP.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_DOWN_STOP.Name = "BTT_DOWN_STOP"
        Me.BTT_DOWN_STOP.Size = New System.Drawing.Size(51, 22)
        Me.BTT_DOWN_STOP.Text = "Stop"
        '
        'MENU_VIEW
        '
        Me.MENU_VIEW.AutoToolTip = False
        Me.MENU_VIEW.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_VIEW_LARGE, Me.BTT_VIEW_SMALL, Me.BTT_VIEW_LIST, Me.BTT_VIEW_DETAILS, MENU_VIEW_SEP_1, Me.BTT_SHOW_ALL, Me.BTT_SHOW_SHOW_GROUPS, Me.BTT_SHOW_GROUP_USERS, Me.BTT_SHOW_FILTER_ADV, MENU_VIEW_SEP_2, Me.BTT_VIEW_FILTER_SAVE, Me.BTT_VIEW_FILTER_SAVE_AS_GROUP, Me.BTT_VIEW_FILTER_LOAD})
        Me.MENU_VIEW.Image = CType(resources.GetObject("MENU_VIEW.Image"), System.Drawing.Image)
        Me.MENU_VIEW.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.MENU_VIEW.Name = "MENU_VIEW"
        Me.MENU_VIEW.Size = New System.Drawing.Size(61, 22)
        Me.MENU_VIEW.Text = "View"
        '
        'BTT_VIEW_LARGE
        '
        Me.BTT_VIEW_LARGE.Name = "BTT_VIEW_LARGE"
        Me.BTT_VIEW_LARGE.Size = New System.Drawing.Size(271, 22)
        Me.BTT_VIEW_LARGE.Text = "Large images"
        '
        'BTT_VIEW_SMALL
        '
        Me.BTT_VIEW_SMALL.Name = "BTT_VIEW_SMALL"
        Me.BTT_VIEW_SMALL.Size = New System.Drawing.Size(271, 22)
        Me.BTT_VIEW_SMALL.Text = "Small images"
        '
        'BTT_VIEW_LIST
        '
        Me.BTT_VIEW_LIST.Name = "BTT_VIEW_LIST"
        Me.BTT_VIEW_LIST.Size = New System.Drawing.Size(271, 22)
        Me.BTT_VIEW_LIST.Text = "List"
        '
        'BTT_VIEW_DETAILS
        '
        Me.BTT_VIEW_DETAILS.Name = "BTT_VIEW_DETAILS"
        Me.BTT_VIEW_DETAILS.Size = New System.Drawing.Size(271, 22)
        Me.BTT_VIEW_DETAILS.Text = "Details"
        '
        'BTT_SHOW_ALL
        '
        Me.BTT_SHOW_ALL.Name = "BTT_SHOW_ALL"
        Me.BTT_SHOW_ALL.Size = New System.Drawing.Size(271, 22)
        Me.BTT_SHOW_ALL.Text = "All"
        '
        'BTT_SHOW_SHOW_GROUPS
        '
        Me.BTT_SHOW_SHOW_GROUPS.Name = "BTT_SHOW_SHOW_GROUPS"
        Me.BTT_SHOW_SHOW_GROUPS.Size = New System.Drawing.Size(271, 22)
        Me.BTT_SHOW_SHOW_GROUPS.Text = "Show groups instead of labels"
        '
        'BTT_SHOW_GROUP_USERS
        '
        Me.BTT_SHOW_GROUP_USERS.AutoToolTip = True
        Me.BTT_SHOW_GROUP_USERS.Name = "BTT_SHOW_GROUP_USERS"
        Me.BTT_SHOW_GROUP_USERS.Size = New System.Drawing.Size(271, 22)
        Me.BTT_SHOW_GROUP_USERS.Text = "Group users"
        Me.BTT_SHOW_GROUP_USERS.ToolTipText = "Group users by site or labels"
        '
        'BTT_SHOW_FILTER_ADV
        '
        Me.BTT_SHOW_FILTER_ADV.AutoToolTip = True
        Me.BTT_SHOW_FILTER_ADV.Name = "BTT_SHOW_FILTER_ADV"
        Me.BTT_SHOW_FILTER_ADV.Size = New System.Drawing.Size(271, 22)
        Me.BTT_SHOW_FILTER_ADV.Text = "Filter"
        Me.BTT_SHOW_FILTER_ADV.ToolTipText = "Filter of users you want to display"
        '
        'BTT_VIEW_FILTER_SAVE
        '
        Me.BTT_VIEW_FILTER_SAVE.Name = "BTT_VIEW_FILTER_SAVE"
        Me.BTT_VIEW_FILTER_SAVE.Size = New System.Drawing.Size(271, 22)
        Me.BTT_VIEW_FILTER_SAVE.Text = "Save current view"
        '
        'BTT_VIEW_FILTER_SAVE_AS_GROUP
        '
        Me.BTT_VIEW_FILTER_SAVE_AS_GROUP.Name = "BTT_VIEW_FILTER_SAVE_AS_GROUP"
        Me.BTT_VIEW_FILTER_SAVE_AS_GROUP.Size = New System.Drawing.Size(271, 22)
        Me.BTT_VIEW_FILTER_SAVE_AS_GROUP.Text = "Save current view as download group"
        '
        'BTT_VIEW_FILTER_LOAD
        '
        Me.BTT_VIEW_FILTER_LOAD.AutoToolTip = True
        Me.BTT_VIEW_FILTER_LOAD.Name = "BTT_VIEW_FILTER_LOAD"
        Me.BTT_VIEW_FILTER_LOAD.Size = New System.Drawing.Size(271, 22)
        Me.BTT_VIEW_FILTER_LOAD.Text = "Load view (from saved)"
        Me.BTT_VIEW_FILTER_LOAD.ToolTipText = "Load one of your previously saved views"
        '
        'BTT_LOG
        '
        Me.BTT_LOG.Image = CType(resources.GetObject("BTT_LOG.Image"), System.Drawing.Image)
        Me.BTT_LOG.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_LOG.Name = "BTT_LOG"
        Me.BTT_LOG.Size = New System.Drawing.Size(50, 22)
        Me.BTT_LOG.Text = "LOG"
        Me.BTT_LOG.ToolTipText = "Show error log"
        '
        'BTT_VERSION_INFO
        '
        Me.BTT_VERSION_INFO.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.BTT_VERSION_INFO.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BTT_VERSION_INFO.Image = Global.SCrawler.My.Resources.Resources.InfoPic_32
        Me.BTT_VERSION_INFO.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_VERSION_INFO.Name = "BTT_VERSION_INFO"
        Me.BTT_VERSION_INFO.Size = New System.Drawing.Size(23, 22)
        Me.BTT_VERSION_INFO.Text = "Info"
        Me.BTT_VERSION_INFO.ToolTipText = "Show program information and check for new version"
        '
        'BTT_DONATE
        '
        Me.BTT_DONATE.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.BTT_DONATE.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BTT_DONATE.Image = Global.SCrawler.My.Resources.Resources.HeartPic_32
        Me.BTT_DONATE.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_DONATE.Name = "BTT_DONATE"
        Me.BTT_DONATE.Size = New System.Drawing.Size(23, 22)
        Me.BTT_DONATE.Text = "Donate"
        Me.BTT_DONATE.ToolTipText = "Support"
        '
        'BTT_BUG_REPORT
        '
        Me.BTT_BUG_REPORT.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.BTT_BUG_REPORT.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BTT_BUG_REPORT.Image = CType(resources.GetObject("BTT_BUG_REPORT.Image"), System.Drawing.Image)
        Me.BTT_BUG_REPORT.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_BUG_REPORT.Name = "BTT_BUG_REPORT"
        Me.BTT_BUG_REPORT.Size = New System.Drawing.Size(23, 22)
        Me.BTT_BUG_REPORT.Text = "Bug report"
        '
        'Toolbar_BOTTOM
        '
        Me.Toolbar_BOTTOM.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_PR_INFO, Me.PR_PRE, Me.PR_MAIN, Me.LBL_JOBS_COUNT, Me.LBL_STATUS})
        Me.Toolbar_BOTTOM.Location = New System.Drawing.Point(0, 439)
        Me.Toolbar_BOTTOM.Name = "Toolbar_BOTTOM"
        Me.Toolbar_BOTTOM.Size = New System.Drawing.Size(934, 22)
        Me.Toolbar_BOTTOM.TabIndex = 2
        '
        'BTT_PR_INFO
        '
        Me.BTT_PR_INFO.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.BTT_PR_INFO.Image = Global.SCrawler.My.Resources.Resources.InfoPic_32
        Me.BTT_PR_INFO.Name = "BTT_PR_INFO"
        Me.BTT_PR_INFO.Padding = New System.Windows.Forms.Padding(0, 0, 3, 0)
        Me.BTT_PR_INFO.Size = New System.Drawing.Size(19, 17)
        '
        'PR_PRE
        '
        Me.PR_PRE.Name = "PR_PRE"
        Me.PR_PRE.Size = New System.Drawing.Size(100, 16)
        Me.PR_PRE.Visible = False
        '
        'PR_MAIN
        '
        Me.PR_MAIN.Name = "PR_MAIN"
        Me.PR_MAIN.Size = New System.Drawing.Size(200, 16)
        Me.PR_MAIN.Visible = False
        '
        'LBL_JOBS_COUNT
        '
        Me.LBL_JOBS_COUNT.Name = "LBL_JOBS_COUNT"
        Me.LBL_JOBS_COUNT.Size = New System.Drawing.Size(0, 17)
        '
        'LBL_STATUS
        '
        Me.LBL_STATUS.Name = "LBL_STATUS"
        Me.LBL_STATUS.Size = New System.Drawing.Size(0, 17)
        '
        'LIST_PROFILES
        '
        Me.LIST_PROFILES.Activation = System.Windows.Forms.ItemActivation.OneClick
        Me.LIST_PROFILES.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.COL_DEF})
        Me.LIST_PROFILES.ContextMenuStrip = Me.USER_CONTEXT
        Me.LIST_PROFILES.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LIST_PROFILES.GridLines = True
        Me.LIST_PROFILES.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.LIST_PROFILES.HideSelection = False
        Me.LIST_PROFILES.Location = New System.Drawing.Point(0, 25)
        Me.LIST_PROFILES.Name = "LIST_PROFILES"
        Me.LIST_PROFILES.Size = New System.Drawing.Size(934, 414)
        Me.LIST_PROFILES.TabIndex = 3
        Me.LIST_PROFILES.UseCompatibleStateImageBehavior = False
        '
        'COL_DEF
        '
        Me.COL_DEF.Text = "Data"
        '
        'USER_CONTEXT
        '
        Me.USER_CONTEXT.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_CONTEXT_DOWN, Me.BTT_CONTEXT_DOWN_LIMITED, Me.BTT_CONTEXT_DOWN_DATE_LIMIT, Me.BTT_CONTEXT_EDIT, Me.BTT_CONTEXT_DELETE, Me.BTT_CONTEXT_ERASE, Me.BTT_CONTEXT_COPY_TO_FOLDER, CONTEXT_SEP_1, Me.BTT_CONTEXT_FAV, Me.BTT_CONTEXT_TEMP, Me.BTT_CONTEXT_READY, Me.BTT_CONTEXT_GROUPS, Me.BTT_CONTEXT_SCRIPT, Me.BTT_CONTEXT_ADD_TO_COL, Me.BTT_CONTEXT_COL_MERGE, Me.BTT_CONTEXT_CHANGE_FOLDER, CONTEXT_SEP_2, Me.BTT_CHANGE_IMAGE, CONTEXT_SEP_3, Me.BTT_CONTEXT_OPEN_PATH, CONTEXT_SEP_4, Me.BTT_CONTEXT_OPEN_SITE, CONTEXT_SEP_5, Me.BTT_CONTEXT_INFO})
        Me.USER_CONTEXT.Name = "USER_CONTEXT"
        Me.USER_CONTEXT.Size = New System.Drawing.Size(222, 452)
        '
        'BTT_CONTEXT_DOWN
        '
        Me.BTT_CONTEXT_DOWN.Image = Global.SCrawler.My.Resources.Resources.StartPic_Green_16
        Me.BTT_CONTEXT_DOWN.Name = "BTT_CONTEXT_DOWN"
        Me.BTT_CONTEXT_DOWN.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_DOWN.Text = "Download"
        Me.BTT_CONTEXT_DOWN.ToolTipText = ""
        '
        'BTT_CONTEXT_DOWN_LIMITED
        '
        Me.BTT_CONTEXT_DOWN_LIMITED.AutoToolTip = True
        Me.BTT_CONTEXT_DOWN_LIMITED.Image = Global.SCrawler.My.Resources.Resources.StartPic_Green_16
        Me.BTT_CONTEXT_DOWN_LIMITED.Name = "BTT_CONTEXT_DOWN_LIMITED"
        Me.BTT_CONTEXT_DOWN_LIMITED.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_DOWN_LIMITED.Text = "Download (limited)"
        Me.BTT_CONTEXT_DOWN_LIMITED.ToolTipText = "Download top ... posts"
        '
        'BTT_CONTEXT_DOWN_DATE_LIMIT
        '
        Me.BTT_CONTEXT_DOWN_DATE_LIMIT.Image = Global.SCrawler.My.Resources.Resources.StartPic_Green_16
        Me.BTT_CONTEXT_DOWN_DATE_LIMIT.Name = "BTT_CONTEXT_DOWN_DATE_LIMIT"
        Me.BTT_CONTEXT_DOWN_DATE_LIMIT.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_DOWN_DATE_LIMIT.Text = "Download (to the date)"
        Me.BTT_CONTEXT_DOWN_DATE_LIMIT.ToolTipText = ""
        '
        'BTT_CONTEXT_EDIT
        '
        Me.BTT_CONTEXT_EDIT.Image = Global.SCrawler.My.Resources.Resources.PencilPic_16
        Me.BTT_CONTEXT_EDIT.Name = "BTT_CONTEXT_EDIT"
        Me.BTT_CONTEXT_EDIT.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_EDIT.Text = "Edit user information"
        '
        'BTT_CONTEXT_DELETE
        '
        Me.BTT_CONTEXT_DELETE.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
        Me.BTT_CONTEXT_DELETE.Name = "BTT_CONTEXT_DELETE"
        Me.BTT_CONTEXT_DELETE.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_DELETE.Text = "Delete user / collection"
        '
        'BTT_CONTEXT_ERASE
        '
        Me.BTT_CONTEXT_ERASE.Image = Global.SCrawler.My.Resources.Resources.BrushToolPic_16
        Me.BTT_CONTEXT_ERASE.Name = "BTT_CONTEXT_ERASE"
        Me.BTT_CONTEXT_ERASE.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_ERASE.Text = "Erase data"
        '
        'BTT_CONTEXT_COPY_TO_FOLDER
        '
        Me.BTT_CONTEXT_COPY_TO_FOLDER.Image = Global.SCrawler.My.Resources.Resources.PastePic_32
        Me.BTT_CONTEXT_COPY_TO_FOLDER.Name = "BTT_CONTEXT_COPY_TO_FOLDER"
        Me.BTT_CONTEXT_COPY_TO_FOLDER.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_COPY_TO_FOLDER.Text = "Copy data to another folder"
        '
        'BTT_CONTEXT_FAV
        '
        Me.BTT_CONTEXT_FAV.Image = Global.SCrawler.My.Resources.Resources.StarPic_24
        Me.BTT_CONTEXT_FAV.Name = "BTT_CONTEXT_FAV"
        Me.BTT_CONTEXT_FAV.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_FAV.Text = "Change favorite"
        '
        'BTT_CONTEXT_TEMP
        '
        Me.BTT_CONTEXT_TEMP.Image = Global.SCrawler.My.Resources.Resources.PinPic_32
        Me.BTT_CONTEXT_TEMP.Name = "BTT_CONTEXT_TEMP"
        Me.BTT_CONTEXT_TEMP.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_TEMP.Text = "Change temporary"
        '
        'BTT_CONTEXT_READY
        '
        Me.BTT_CONTEXT_READY.Image = Global.SCrawler.My.Resources.Resources.OkPic_32
        Me.BTT_CONTEXT_READY.Name = "BTT_CONTEXT_READY"
        Me.BTT_CONTEXT_READY.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_READY.Text = "Change ready for download"
        '
        'BTT_CONTEXT_GROUPS
        '
        Me.BTT_CONTEXT_GROUPS.AutoToolTip = True
        Me.BTT_CONTEXT_GROUPS.Image = Global.SCrawler.My.Resources.Resources.TagPic_24
        Me.BTT_CONTEXT_GROUPS.Name = "BTT_CONTEXT_GROUPS"
        Me.BTT_CONTEXT_GROUPS.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_GROUPS.Text = "Change labels"
        Me.BTT_CONTEXT_GROUPS.ToolTipText = "Change user labels." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Ctrl+click to include site-specific labels."
        '
        'BTT_CONTEXT_SCRIPT
        '
        Me.BTT_CONTEXT_SCRIPT.Image = Global.SCrawler.My.Resources.Resources.ScriptPic_32
        Me.BTT_CONTEXT_SCRIPT.Name = "BTT_CONTEXT_SCRIPT"
        Me.BTT_CONTEXT_SCRIPT.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_SCRIPT.Text = "Change script usage"
        '
        'BTT_CONTEXT_ADD_TO_COL
        '
        Me.BTT_CONTEXT_ADD_TO_COL.Image = Global.SCrawler.My.Resources.Resources.DBPic_32
        Me.BTT_CONTEXT_ADD_TO_COL.Name = "BTT_CONTEXT_ADD_TO_COL"
        Me.BTT_CONTEXT_ADD_TO_COL.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_ADD_TO_COL.Text = "Add to collection"
        '
        'BTT_CONTEXT_COL_MERGE
        '
        Me.BTT_CONTEXT_COL_MERGE.Image = Global.SCrawler.My.Resources.Resources.DBPic_32
        Me.BTT_CONTEXT_COL_MERGE.Name = "BTT_CONTEXT_COL_MERGE"
        Me.BTT_CONTEXT_COL_MERGE.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_COL_MERGE.Text = "Merge collection files"
        '
        'BTT_CONTEXT_CHANGE_FOLDER
        '
        Me.BTT_CONTEXT_CHANGE_FOLDER.AutoToolTip = True
        Me.BTT_CONTEXT_CHANGE_FOLDER.Image = Global.SCrawler.My.Resources.Resources.FolderPic_32
        Me.BTT_CONTEXT_CHANGE_FOLDER.Name = "BTT_CONTEXT_CHANGE_FOLDER"
        Me.BTT_CONTEXT_CHANGE_FOLDER.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_CHANGE_FOLDER.Text = "Change folder"
        Me.BTT_CONTEXT_CHANGE_FOLDER.ToolTipText = "Change user data folder"
        '
        'BTT_CHANGE_IMAGE
        '
        Me.BTT_CHANGE_IMAGE.Image = Global.SCrawler.My.Resources.Resources.PicturePic_32
        Me.BTT_CHANGE_IMAGE.Name = "BTT_CHANGE_IMAGE"
        Me.BTT_CHANGE_IMAGE.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CHANGE_IMAGE.Text = "Change image"
        '
        'BTT_CONTEXT_OPEN_PATH
        '
        Me.BTT_CONTEXT_OPEN_PATH.Image = Global.SCrawler.My.Resources.Resources.FolderPic_32
        Me.BTT_CONTEXT_OPEN_PATH.Name = "BTT_CONTEXT_OPEN_PATH"
        Me.BTT_CONTEXT_OPEN_PATH.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_OPEN_PATH.Text = "Open contains folder"
        '
        'BTT_CONTEXT_OPEN_SITE
        '
        Me.BTT_CONTEXT_OPEN_SITE.Image = Global.SCrawler.My.Resources.Resources.GlobePic_32
        Me.BTT_CONTEXT_OPEN_SITE.Name = "BTT_CONTEXT_OPEN_SITE"
        Me.BTT_CONTEXT_OPEN_SITE.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_OPEN_SITE.Text = "Open site"
        '
        'BTT_CONTEXT_INFO
        '
        Me.BTT_CONTEXT_INFO.Image = Global.SCrawler.My.Resources.Resources.InfoPic_32
        Me.BTT_CONTEXT_INFO.Name = "BTT_CONTEXT_INFO"
        Me.BTT_CONTEXT_INFO.Size = New System.Drawing.Size(221, 22)
        Me.BTT_CONTEXT_INFO.Text = "Information"
        '
        'TrayIcon
        '
        Me.TrayIcon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.TrayIcon.BalloonTipTitle = "Social networks crawler"
        Me.TrayIcon.ContextMenuStrip = Me.TRAY_CONTEXT
        Me.TrayIcon.Icon = Global.SCrawler.My.Resources.Resources.RainbowIcon_48
        Me.TrayIcon.Text = "Social networks crawler." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Ctrl+Click to show the feed."
        '
        'TRAY_CONTEXT
        '
        Me.TRAY_CONTEXT.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_TRAY_PAUSE_AUTOMATION, Me.BTT_TRAY_SILENT_MODE, Me.BTT_TRAY_FEED_SHOW, Me.BTT_TRAY_CHANNELS, Me.BTT_TRAY_DOWNLOADER, Me.BTT_TRAY_SCHEDULER, TRAY_SEP_1, Me.BTT_TRAY_SHOW_HIDE, TRAY_SEP_2, Me.BTT_TRAY_CLOSE, Me.BTT_TRAY_CLOSE_NO_SCRIPT})
        Me.TRAY_CONTEXT.Name = "TRAY_CONTEXT"
        Me.TRAY_CONTEXT.Size = New System.Drawing.Size(171, 214)
        '
        'BTT_TRAY_PAUSE_AUTOMATION
        '
        Me.BTT_TRAY_PAUSE_AUTOMATION.Image = Global.SCrawler.My.Resources.Resources.Pause_Blue_16
        Me.BTT_TRAY_PAUSE_AUTOMATION.Name = "BTT_TRAY_PAUSE_AUTOMATION"
        Me.BTT_TRAY_PAUSE_AUTOMATION.Size = New System.Drawing.Size(170, 22)
        Me.BTT_TRAY_PAUSE_AUTOMATION.Text = "Pause automation"
        '
        'BTT_TRAY_SILENT_MODE
        '
        Me.BTT_TRAY_SILENT_MODE.AutoToolTip = True
        Me.BTT_TRAY_SILENT_MODE.Image = Global.SCrawler.My.Resources.Resources.MessagePic_16
        Me.BTT_TRAY_SILENT_MODE.Name = "BTT_TRAY_SILENT_MODE"
        Me.BTT_TRAY_SILENT_MODE.Size = New System.Drawing.Size(170, 22)
        Me.BTT_TRAY_SILENT_MODE.Text = "Silent mode"
        Me.BTT_TRAY_SILENT_MODE.ToolTipText = "Turn off notifications temporarily." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This setting is not stored in the settings f" &
    "ile. It is valid until you turn it off or close the program." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'BTT_TRAY_FEED_SHOW
        '
        Me.BTT_TRAY_FEED_SHOW.AutoToolTip = True
        Me.BTT_TRAY_FEED_SHOW.Image = Global.SCrawler.My.Resources.Resources.RSSPic_512
        Me.BTT_TRAY_FEED_SHOW.Name = "BTT_TRAY_FEED_SHOW"
        Me.BTT_TRAY_FEED_SHOW.Size = New System.Drawing.Size(170, 22)
        Me.BTT_TRAY_FEED_SHOW.Text = "Feed"
        Me.BTT_TRAY_FEED_SHOW.ToolTipText = "Show feed of recently downloaded data." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Ctrl+Click the tray icon to show the feed" &
    "."
        '
        'BTT_TRAY_CHANNELS
        '
        Me.BTT_TRAY_CHANNELS.Image = Global.SCrawler.My.Resources.SiteResources.RedditPic_512
        Me.BTT_TRAY_CHANNELS.Name = "BTT_TRAY_CHANNELS"
        Me.BTT_TRAY_CHANNELS.Size = New System.Drawing.Size(170, 22)
        Me.BTT_TRAY_CHANNELS.Text = "Channels"
        '
        'BTT_TRAY_DOWNLOADER
        '
        Me.BTT_TRAY_DOWNLOADER.Image = Global.SCrawler.My.Resources.Resources.ArrowDownPic_Blue_24
        Me.BTT_TRAY_DOWNLOADER.Name = "BTT_TRAY_DOWNLOADER"
        Me.BTT_TRAY_DOWNLOADER.Size = New System.Drawing.Size(170, 22)
        Me.BTT_TRAY_DOWNLOADER.Text = "Downloader"
        '
        'BTT_TRAY_SCHEDULER
        '
        Me.BTT_TRAY_SCHEDULER.Image = Global.SCrawler.My.Resources.Resources.ScriptPic_32
        Me.BTT_TRAY_SCHEDULER.Name = "BTT_TRAY_SCHEDULER"
        Me.BTT_TRAY_SCHEDULER.Size = New System.Drawing.Size(170, 22)
        Me.BTT_TRAY_SCHEDULER.Text = "Scheduler"
        '
        'BTT_TRAY_SHOW_HIDE
        '
        Me.BTT_TRAY_SHOW_HIDE.Image = Global.SCrawler.My.Resources.Resources.ApplicationPic_16
        Me.BTT_TRAY_SHOW_HIDE.Name = "BTT_TRAY_SHOW_HIDE"
        Me.BTT_TRAY_SHOW_HIDE.Size = New System.Drawing.Size(170, 22)
        Me.BTT_TRAY_SHOW_HIDE.Text = "Show/Hide"
        '
        'BTT_TRAY_CLOSE
        '
        Me.BTT_TRAY_CLOSE.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.BTT_TRAY_CLOSE.ForeColor = System.Drawing.Color.Maroon
        Me.BTT_TRAY_CLOSE.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
        Me.BTT_TRAY_CLOSE.Name = "BTT_TRAY_CLOSE"
        Me.BTT_TRAY_CLOSE.Size = New System.Drawing.Size(170, 22)
        Me.BTT_TRAY_CLOSE.Text = "Close"
        '
        'BTT_TRAY_CLOSE_NO_SCRIPT
        '
        Me.BTT_TRAY_CLOSE_NO_SCRIPT.AutoToolTip = True
        Me.BTT_TRAY_CLOSE_NO_SCRIPT.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.BTT_TRAY_CLOSE_NO_SCRIPT.ForeColor = System.Drawing.Color.Maroon
        Me.BTT_TRAY_CLOSE_NO_SCRIPT.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
        Me.BTT_TRAY_CLOSE_NO_SCRIPT.Name = "BTT_TRAY_CLOSE_NO_SCRIPT"
        Me.BTT_TRAY_CLOSE_NO_SCRIPT.Size = New System.Drawing.Size(170, 22)
        Me.BTT_TRAY_CLOSE_NO_SCRIPT.Text = "Close (no script)"
        Me.BTT_TRAY_CLOSE_NO_SCRIPT.ToolTipText = "Close the program without executing the script"
        Me.BTT_TRAY_CLOSE_NO_SCRIPT.Visible = False
        '
        'MainFrame
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(934, 461)
        Me.Controls.Add(Me.LIST_PROFILES)
        Me.Controls.Add(Me.Toolbar_BOTTOM)
        Me.Controls.Add(Me.Toolbar_TOP)
        Me.Icon = Global.SCrawler.My.Resources.Resources.RainbowIcon_48
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(950, 500)
        Me.Name = "MainFrame"
        Me.Text = "Social networks crawler"
        Me.Toolbar_TOP.ResumeLayout(False)
        Me.Toolbar_TOP.PerformLayout()
        Me.Toolbar_BOTTOM.ResumeLayout(False)
        Me.Toolbar_BOTTOM.PerformLayout()
        Me.USER_CONTEXT.ResumeLayout(False)
        Me.TRAY_CONTEXT.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents BTT_SETTINGS As ToolStripMenuItem
    Private WithEvents BTT_ADD_USER As PersonalUtilities.Forms.Controls.KeyClick.ToolStripButtonKeyClick
    Private WithEvents BTT_DELETE_USER As ToolStripButton
    Private WithEvents BTT_DOWN_SELECTED As ToolStripKeyMenuItem
    Private WithEvents Toolbar_BOTTOM As StatusStrip
    Private WithEvents PR_MAIN As ToolStripProgressBar
    Private WithEvents LBL_STATUS As ToolStripStatusLabel
    Private WithEvents BTT_CONTEXT_DOWN As ToolStripKeyMenuItem
    Private WithEvents BTT_CONTEXT_EDIT As ToolStripMenuItem
    Private WithEvents BTT_CONTEXT_DELETE As ToolStripMenuItem
    Private WithEvents BTT_CONTEXT_FAV As ToolStripMenuItem
    Private WithEvents BTT_CONTEXT_TEMP As ToolStripMenuItem
    Private WithEvents USER_CONTEXT As ContextMenuStrip
    Private WithEvents BTT_VIEW_LARGE As ToolStripMenuItem
    Private WithEvents BTT_VIEW_SMALL As ToolStripMenuItem
    Private WithEvents BTT_VIEW_LIST As ToolStripMenuItem
    Private WithEvents BTT_CHANGE_IMAGE As ToolStripMenuItem
    Private WithEvents BTT_CONTEXT_OPEN_PATH As ToolStripMenuItem
    Private WithEvents BTT_CONTEXT_INFO As ToolStripMenuItem
    Private WithEvents BTT_CONTEXT_OPEN_SITE As ToolStripMenuItem
    Private WithEvents BTT_DOWN_STOP As ToolStripButton
    Private WithEvents BTT_CONTEXT_COL_MERGE As ToolStripMenuItem
    Private WithEvents LBL_JOBS_COUNT As ToolStripStatusLabel
    Private WithEvents BTT_DOWN_VIDEO As ToolStripMenuItem
    Private WithEvents MENU_INFO_SHOW_INFO As ToolStripMenuItem
    Private WithEvents BTT_CHANNELS As ToolStripButton
    Private WithEvents LIST_PROFILES As ListView
    Private WithEvents MENU_VIEW As ToolStripDropDownButton
    Private WithEvents BTT_CONTEXT_ADD_TO_COL As ToolStripMenuItem
    Private WithEvents BTT_REFRESH As ToolStripButton
    Private WithEvents BTT_SHOW_ALL As ToolStripMenuItem
    Private WithEvents BTT_EDIT_USER As ToolStripButton
    Private WithEvents BTT_CONTEXT_GROUPS As PersonalUtilities.Forms.Controls.KeyClick.ToolStripMenuItemKeyClick
    Private WithEvents BTT_VERSION_INFO As ToolStripButton
    Private WithEvents BTT_CONTEXT_DOWN_LIMITED As ToolStripKeyMenuItem
    Private WithEvents BTT_CONTEXT_READY As ToolStripMenuItem
    Private WithEvents BTT_CONTEXT_CHANGE_FOLDER As ToolStripMenuItem
    Private WithEvents BTT_DOWN_SAVED As ToolStripButton
    Friend WithEvents TrayIcon As NotifyIcon
    Private WithEvents BTT_TRAY_SHOW_HIDE As ToolStripMenuItem
    Private WithEvents BTT_TRAY_CLOSE As ToolStripMenuItem
    Private WithEvents BTT_DONATE As ToolStripButton
    Private WithEvents BTT_VIEW_DETAILS As ToolStripMenuItem
    Private WithEvents COL_DEF As ColumnHeader
    Private WithEvents MENU_SETTINGS As ToolStripDropDownButton
    Private WithEvents BTT_PR_INFO As ToolStripStatusLabel
    Private WithEvents BTT_CONTEXT_DOWN_DATE_LIMIT As ToolStripKeyMenuItem
    Private WithEvents BTT_CONTEXT_SCRIPT As ToolStripMenuItem
    Private WithEvents BTT_SHOW_SHOW_GROUPS As ToolStripMenuItem
    Private WithEvents BTT_ADD_NEW_GROUP As ToolStripMenuItem
    Friend WithEvents BTT_TRAY_CLOSE_NO_SCRIPT As ToolStripMenuItem
    Friend WithEvents TRAY_CONTEXT As ContextMenuStrip
    Private WithEvents BTT_DOWN_AUTOMATION As ToolStripMenuItem
    Friend WithEvents BTT_LOG As ToolStripButton
    Friend WithEvents Toolbar_TOP As ToolStrip
    Private WithEvents BTT_FEED As ToolStripButton
    Private WithEvents BTT_CONTEXT_COPY_TO_FOLDER As ToolStripMenuItem
    Private WithEvents BTT_TRAY_SILENT_MODE As ToolStripMenuItem
    Friend WithEvents BTT_TRAY_PAUSE_AUTOMATION As ToolStripMenuItem
    Private WithEvents BTT_SILENT_MODE As ToolStripMenuItem
    Friend WithEvents BTT_DOWN_AUTOMATION_PAUSE As ToolStripMenuItem
    Private WithEvents BTT_TRAY_FEED_SHOW As ToolStripMenuItem
    Friend WithEvents MENU_DOWN_ALL As ToolStripDropDownButton
    Private WithEvents BTT_TRAY_CHANNELS As ToolStripMenuItem
    Private WithEvents BTT_TRAY_DOWNLOADER As ToolStripMenuItem
    Private WithEvents PR_PRE As ToolStripProgressBar
    Private WithEvents BTT_CONTEXT_ERASE As ToolStripMenuItem
    Private WithEvents MENU_INFO_SHOW_MISSING As ToolStripMenuItem
    Private WithEvents MENU_INFO_SHOW_USER_METRICS As ToolStripMenuItem
    Private WithEvents BTT_DOWN_ALL As ToolStripKeyMenuItem
    Private WithEvents BTT_DOWN_ALL_FULL As ToolStripKeyMenuItem
    Private WithEvents BTT_BUG_REPORT As ToolStripButton
    Private WithEvents MENU_INFO_SHOW_QUEUE As ToolStripMenuItem
    Private WithEvents BTT_DOWN_SPEC As ToolStripKeyMenuItem
    Private WithEvents BTT_SHOW_FILTER_ADV As ToolStripMenuItem
    Private WithEvents BTT_TRAY_SCHEDULER As ToolStripMenuItem
    Private WithEvents BTT_SHOW_GROUP_USERS As ToolStripMenuItem
    Private WithEvents BTT_VIEW_FILTER_SAVE As ToolStripMenuItem
    Private WithEvents BTT_VIEW_FILTER_LOAD As ToolStripMenuItem
    Private WithEvents BTT_VIEW_FILTER_SAVE_AS_GROUP As ToolStripMenuItem
    Private WithEvents MENU_INFO_USER_SEARCH As ToolStripMenuItem
End Class