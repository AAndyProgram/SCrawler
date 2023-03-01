' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace Editors
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class GlobalSettingsForm : Inherits System.Windows.Forms.Form
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
            Dim TP_BASIS As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GlobalSettingsForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim TP_IMAGES As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton4 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton5 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton6 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton7 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton8 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton9 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton10 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim TP_FILE_NAME As System.Windows.Forms.TableLayoutPanel
            Dim TP_FILE_PATTERNS As System.Windows.Forms.TableLayoutPanel
            Dim LBL_DATE_POS As System.Windows.Forms.Label
            Dim TT_MAIN As System.Windows.Forms.ToolTip
            Dim TP_CHANNELS_IMGS As System.Windows.Forms.TableLayoutPanel
            Dim TAB_BASIS As System.Windows.Forms.TabPage
            Dim TAB_DEFAULTS As System.Windows.Forms.TabPage
            Dim TP_DEFS As System.Windows.Forms.TableLayoutPanel
            Dim TAB_DEFS_CHANNELS As System.Windows.Forms.TabPage
            Dim TP_CHANNELS As System.Windows.Forms.TableLayoutPanel
            Dim TAB_BEHAVIOR As System.Windows.Forms.TabPage
            Dim TP_BEHAVIOR As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton11 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton12 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim TP_OPEN_INFO As System.Windows.Forms.TableLayoutPanel
            Dim TP_OPEN_PROGRESS As System.Windows.Forms.TableLayoutPanel
            Dim TAB_DOWN As System.Windows.Forms.TabPage
            Dim TP_DOWNLOADING As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton13 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton14 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim TP_MISSING_DATA As System.Windows.Forms.TableLayoutPanel
            Dim TAB_FEED As System.Windows.Forms.TabPage
            Dim TP_FEED As System.Windows.Forms.TableLayoutPanel
            Dim TP_FEED_IMG_COUNT As System.Windows.Forms.TableLayoutPanel
            Dim TAB_NOTIFY As System.Windows.Forms.TabPage
            Dim TP_NOTIFY_MAIN As System.Windows.Forms.TableLayoutPanel
            Me.TXT_GLOBAL_PATH = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_IMAGE_LARGE = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_IMAGE_SMALL = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_COLLECTIONS_PATH = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_MAX_JOBS_USERS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_MAX_JOBS_CHANNELS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_CHECK_VER_START = New System.Windows.Forms.CheckBox()
            Me.TXT_IMGUR_CLIENT_ID = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_SHOW_GROUPS = New System.Windows.Forms.CheckBox()
            Me.CH_USERS_GROUPING = New System.Windows.Forms.CheckBox()
            Me.TXT_USER_AGENT = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_USER_LIST_IMAGE = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.COLORS_USERLIST = New SCrawler.Editors.ColorPicker()
            Me.OPT_FILE_NAME_REPLACE = New System.Windows.Forms.RadioButton()
            Me.OPT_FILE_NAME_ADD_DATE = New System.Windows.Forms.RadioButton()
            Me.CH_FILE_NAME_CHANGE = New System.Windows.Forms.CheckBox()
            Me.CH_FILE_DATE = New System.Windows.Forms.CheckBox()
            Me.CH_FILE_TIME = New System.Windows.Forms.CheckBox()
            Me.OPT_FILE_DATE_START = New System.Windows.Forms.RadioButton()
            Me.OPT_FILE_DATE_END = New System.Windows.Forms.RadioButton()
            Me.CH_FAST_LOAD = New System.Windows.Forms.CheckBox()
            Me.CH_COPY_CHANNEL_USER_IMAGE = New System.Windows.Forms.CheckBox()
            Me.CH_DEF_TEMP = New System.Windows.Forms.CheckBox()
            Me.CH_DOWN_IMAGES = New System.Windows.Forms.CheckBox()
            Me.CH_DOWN_VIDEOS = New System.Windows.Forms.CheckBox()
            Me.CH_SEPARATE_VIDEO_FOLDER = New System.Windows.Forms.CheckBox()
            Me.CH_CHANNELS_USERS_TEMP = New System.Windows.Forms.CheckBox()
            Me.CH_COPY_CHANNEL_USER_IMAGE_ALL = New System.Windows.Forms.CheckBox()
            Me.CH_UDESCR_UP = New System.Windows.Forms.CheckBox()
            Me.CH_DOWN_OPEN_INFO_SUSPEND = New System.Windows.Forms.CheckBox()
            Me.CH_DOWN_OPEN_PROGRESS_SUSPEND = New System.Windows.Forms.CheckBox()
            Me.CH_ADD_MISSING_TO_LOG = New System.Windows.Forms.CheckBox()
            Me.CH_ADD_MISSING_ERROS_TO_LOG = New System.Windows.Forms.CheckBox()
            Me.CH_FEED_STORE_SESSION_DATA = New System.Windows.Forms.CheckBox()
            Me.CH_NOTIFY_SHOW_BASE = New System.Windows.Forms.CheckBox()
            Me.CH_NOTIFY_SILENT = New System.Windows.Forms.CheckBox()
            Me.CH_NOTIFY_PROFILES = New System.Windows.Forms.CheckBox()
            Me.CH_NOTIFY_AUTO_DOWN = New System.Windows.Forms.CheckBox()
            Me.CH_NOTIFY_CHANNELS = New System.Windows.Forms.CheckBox()
            Me.CH_NOTIFY_SAVED_POSTS = New System.Windows.Forms.CheckBox()
            Me.CH_DOWN_REPARSE_MISSING = New System.Windows.Forms.CheckBox()
            Me.CH_NAME_SITE_FRIENDLY = New System.Windows.Forms.CheckBox()
            Me.TXT_CHANNELS_ROWS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_CHANNELS_COLUMNS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_DOWN_IMAGES_NATIVE = New System.Windows.Forms.CheckBox()
            Me.TXT_CHANNEL_USER_POST_LIMIT = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_FOLDER_CMD = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_EXIT_CONFIRM = New System.Windows.Forms.CheckBox()
            Me.CH_CLOSE_TO_TRAY = New System.Windows.Forms.CheckBox()
            Me.TXT_CLOSE_SCRIPT = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_DOWN_OPEN_INFO = New System.Windows.Forms.CheckBox()
            Me.CH_RECYCLE_DEL = New System.Windows.Forms.CheckBox()
            Me.CH_DOWN_OPEN_PROGRESS = New System.Windows.Forms.CheckBox()
            Me.TXT_SCRIPT = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_DOWN_COMPLETE_SCRIPT = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_UNAME_UP = New System.Windows.Forms.CheckBox()
            Me.TXT_FEED_ROWS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_FEED_COLUMNS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_FEED_ENDLESS = New System.Windows.Forms.CheckBox()
            Me.CH_FEED_ADD_SESSION = New System.Windows.Forms.CheckBox()
            Me.CH_FEED_ADD_DATE = New System.Windows.Forms.CheckBox()
            Me.TXT_FEED_CENTER_IMAGE = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.COLORS_FEED = New SCrawler.Editors.ColorPicker()
            Me.TAB_MAIN = New System.Windows.Forms.TabControl()
            Me.CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_BASIS = New System.Windows.Forms.TableLayoutPanel()
            TP_IMAGES = New System.Windows.Forms.TableLayoutPanel()
            TP_FILE_NAME = New System.Windows.Forms.TableLayoutPanel()
            TP_FILE_PATTERNS = New System.Windows.Forms.TableLayoutPanel()
            LBL_DATE_POS = New System.Windows.Forms.Label()
            TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
            TP_CHANNELS_IMGS = New System.Windows.Forms.TableLayoutPanel()
            TAB_BASIS = New System.Windows.Forms.TabPage()
            TAB_DEFAULTS = New System.Windows.Forms.TabPage()
            TP_DEFS = New System.Windows.Forms.TableLayoutPanel()
            TAB_DEFS_CHANNELS = New System.Windows.Forms.TabPage()
            TP_CHANNELS = New System.Windows.Forms.TableLayoutPanel()
            TAB_BEHAVIOR = New System.Windows.Forms.TabPage()
            TP_BEHAVIOR = New System.Windows.Forms.TableLayoutPanel()
            TP_OPEN_INFO = New System.Windows.Forms.TableLayoutPanel()
            TP_OPEN_PROGRESS = New System.Windows.Forms.TableLayoutPanel()
            TAB_DOWN = New System.Windows.Forms.TabPage()
            TP_DOWNLOADING = New System.Windows.Forms.TableLayoutPanel()
            TP_MISSING_DATA = New System.Windows.Forms.TableLayoutPanel()
            TAB_FEED = New System.Windows.Forms.TabPage()
            TP_FEED = New System.Windows.Forms.TableLayoutPanel()
            TP_FEED_IMG_COUNT = New System.Windows.Forms.TableLayoutPanel()
            TAB_NOTIFY = New System.Windows.Forms.TabPage()
            TP_NOTIFY_MAIN = New System.Windows.Forms.TableLayoutPanel()
            TP_BASIS.SuspendLayout()
            CType(Me.TXT_GLOBAL_PATH, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_IMAGES.SuspendLayout()
            CType(Me.TXT_IMAGE_LARGE, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_IMAGE_SMALL, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_COLLECTIONS_PATH, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_MAX_JOBS_USERS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_MAX_JOBS_CHANNELS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_IMGUR_CLIENT_ID, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_USER_AGENT, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_USER_LIST_IMAGE, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_FILE_NAME.SuspendLayout()
            TP_FILE_PATTERNS.SuspendLayout()
            TP_CHANNELS_IMGS.SuspendLayout()
            CType(Me.TXT_CHANNELS_ROWS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_CHANNELS_COLUMNS, System.ComponentModel.ISupportInitialize).BeginInit()
            TAB_BASIS.SuspendLayout()
            TAB_DEFAULTS.SuspendLayout()
            TP_DEFS.SuspendLayout()
            TAB_DEFS_CHANNELS.SuspendLayout()
            TP_CHANNELS.SuspendLayout()
            CType(Me.TXT_CHANNEL_USER_POST_LIMIT, System.ComponentModel.ISupportInitialize).BeginInit()
            TAB_BEHAVIOR.SuspendLayout()
            TP_BEHAVIOR.SuspendLayout()
            CType(Me.TXT_FOLDER_CMD, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_CLOSE_SCRIPT, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_OPEN_INFO.SuspendLayout()
            TP_OPEN_PROGRESS.SuspendLayout()
            TAB_DOWN.SuspendLayout()
            TP_DOWNLOADING.SuspendLayout()
            CType(Me.TXT_SCRIPT, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_DOWN_COMPLETE_SCRIPT, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_MISSING_DATA.SuspendLayout()
            TAB_FEED.SuspendLayout()
            TP_FEED.SuspendLayout()
            TP_FEED_IMG_COUNT.SuspendLayout()
            CType(Me.TXT_FEED_ROWS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_FEED_COLUMNS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_FEED_CENTER_IMAGE, System.ComponentModel.ISupportInitialize).BeginInit()
            TAB_NOTIFY.SuspendLayout()
            TP_NOTIFY_MAIN.SuspendLayout()
            Me.TAB_MAIN.SuspendLayout()
            Me.CONTAINER_MAIN.ContentPanel.SuspendLayout()
            Me.CONTAINER_MAIN.SuspendLayout()
            Me.SuspendLayout()
            '
            'TP_BASIS
            '
            TP_BASIS.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_BASIS.ColumnCount = 1
            TP_BASIS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_BASIS.Controls.Add(Me.TXT_GLOBAL_PATH, 0, 0)
            TP_BASIS.Controls.Add(TP_IMAGES, 0, 1)
            TP_BASIS.Controls.Add(Me.TXT_COLLECTIONS_PATH, 0, 2)
            TP_BASIS.Controls.Add(Me.TXT_MAX_JOBS_USERS, 0, 3)
            TP_BASIS.Controls.Add(Me.TXT_MAX_JOBS_CHANNELS, 0, 4)
            TP_BASIS.Controls.Add(Me.CH_CHECK_VER_START, 0, 5)
            TP_BASIS.Controls.Add(Me.TXT_IMGUR_CLIENT_ID, 0, 7)
            TP_BASIS.Controls.Add(Me.CH_SHOW_GROUPS, 0, 10)
            TP_BASIS.Controls.Add(Me.CH_USERS_GROUPING, 0, 11)
            TP_BASIS.Controls.Add(Me.TXT_USER_AGENT, 0, 6)
            TP_BASIS.Controls.Add(Me.TXT_USER_LIST_IMAGE, 0, 8)
            TP_BASIS.Controls.Add(Me.COLORS_USERLIST, 0, 9)
            TP_BASIS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_BASIS.Location = New System.Drawing.Point(3, 3)
            TP_BASIS.Name = "TP_BASIS"
            TP_BASIS.RowCount = 13
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_BASIS.Size = New System.Drawing.Size(570, 337)
            TP_BASIS.TabIndex = 0
            '
            'TXT_GLOBAL_PATH
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Name = "Open"
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Name = "Clear"
            Me.TXT_GLOBAL_PATH.Buttons.Add(ActionButton1)
            Me.TXT_GLOBAL_PATH.Buttons.Add(ActionButton2)
            Me.TXT_GLOBAL_PATH.CaptionText = "Data Path"
            Me.TXT_GLOBAL_PATH.CaptionToolTipEnabled = True
            Me.TXT_GLOBAL_PATH.CaptionToolTipText = "Root path for storing users' data"
            Me.TXT_GLOBAL_PATH.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_GLOBAL_PATH.Location = New System.Drawing.Point(4, 4)
            Me.TXT_GLOBAL_PATH.Name = "TXT_GLOBAL_PATH"
            Me.TXT_GLOBAL_PATH.Size = New System.Drawing.Size(562, 22)
            Me.TXT_GLOBAL_PATH.TabIndex = 0
            '
            'TP_IMAGES
            '
            TP_IMAGES.ColumnCount = 2
            TP_IMAGES.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_IMAGES.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_IMAGES.Controls.Add(Me.TXT_IMAGE_LARGE, 0, 0)
            TP_IMAGES.Controls.Add(Me.TXT_IMAGE_SMALL, 1, 0)
            TP_IMAGES.Dock = System.Windows.Forms.DockStyle.Fill
            TP_IMAGES.Location = New System.Drawing.Point(1, 30)
            TP_IMAGES.Margin = New System.Windows.Forms.Padding(0)
            TP_IMAGES.Name = "TP_IMAGES"
            TP_IMAGES.RowCount = 1
            TP_IMAGES.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_IMAGES.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_IMAGES.Size = New System.Drawing.Size(568, 28)
            TP_IMAGES.TabIndex = 1
            '
            'TXT_IMAGE_LARGE
            '
            Me.TXT_IMAGE_LARGE.CaptionText = "Large image size"
            Me.TXT_IMAGE_LARGE.CaptionToolTipEnabled = True
            Me.TXT_IMAGE_LARGE.CaptionToolTipText = "Maximum large image size by height"
            Me.TXT_IMAGE_LARGE.ControlMode = PersonalUtilities.Forms.Controls.TextBoxExtended.ControlModes.NumericUpDown
            Me.TXT_IMAGE_LARGE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_IMAGE_LARGE.Location = New System.Drawing.Point(3, 3)
            Me.TXT_IMAGE_LARGE.Name = "TXT_IMAGE_LARGE"
            Me.TXT_IMAGE_LARGE.NumberMaximum = New Decimal(New Integer() {256, 0, 0, 0})
            Me.TXT_IMAGE_LARGE.NumberMinimum = New Decimal(New Integer() {50, 0, 0, 0})
            Me.TXT_IMAGE_LARGE.Size = New System.Drawing.Size(278, 22)
            Me.TXT_IMAGE_LARGE.TabIndex = 0
            Me.TXT_IMAGE_LARGE.Text = "50"
            Me.TXT_IMAGE_LARGE.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'TXT_IMAGE_SMALL
            '
            Me.TXT_IMAGE_SMALL.CaptionText = "Small image size"
            Me.TXT_IMAGE_SMALL.CaptionToolTipEnabled = True
            Me.TXT_IMAGE_SMALL.CaptionToolTipText = "Maximum small image size by height"
            Me.TXT_IMAGE_SMALL.ControlMode = PersonalUtilities.Forms.Controls.TextBoxExtended.ControlModes.NumericUpDown
            Me.TXT_IMAGE_SMALL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_IMAGE_SMALL.Location = New System.Drawing.Point(287, 3)
            Me.TXT_IMAGE_SMALL.Name = "TXT_IMAGE_SMALL"
            Me.TXT_IMAGE_SMALL.NumberMaximum = New Decimal(New Integer() {256, 0, 0, 0})
            Me.TXT_IMAGE_SMALL.NumberMinimum = New Decimal(New Integer() {10, 0, 0, 0})
            Me.TXT_IMAGE_SMALL.Size = New System.Drawing.Size(278, 22)
            Me.TXT_IMAGE_SMALL.TabIndex = 1
            Me.TXT_IMAGE_SMALL.Text = "10"
            Me.TXT_IMAGE_SMALL.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'TXT_COLLECTIONS_PATH
            '
            ActionButton3.BackgroundImage = CType(resources.GetObject("ActionButton3.BackgroundImage"), System.Drawing.Image)
            ActionButton3.Name = "Clear"
            Me.TXT_COLLECTIONS_PATH.Buttons.Add(ActionButton3)
            Me.TXT_COLLECTIONS_PATH.CaptionText = "Collections folder"
            Me.TXT_COLLECTIONS_PATH.CaptionToolTipEnabled = True
            Me.TXT_COLLECTIONS_PATH.CaptionToolTipText = "Set collections folder name (name only)"
            Me.TXT_COLLECTIONS_PATH.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_COLLECTIONS_PATH.Location = New System.Drawing.Point(4, 62)
            Me.TXT_COLLECTIONS_PATH.Name = "TXT_COLLECTIONS_PATH"
            Me.TXT_COLLECTIONS_PATH.Size = New System.Drawing.Size(562, 22)
            Me.TXT_COLLECTIONS_PATH.TabIndex = 2
            '
            'TXT_MAX_JOBS_USERS
            '
            ActionButton4.BackgroundImage = CType(resources.GetObject("ActionButton4.BackgroundImage"), System.Drawing.Image)
            ActionButton4.Name = "Refresh"
            ActionButton4.ToolTipText = "Set to default"
            Me.TXT_MAX_JOBS_USERS.Buttons.Add(ActionButton4)
            Me.TXT_MAX_JOBS_USERS.CaptionSizeType = System.Windows.Forms.SizeType.Percent
            Me.TXT_MAX_JOBS_USERS.CaptionText = "Maximum downloading tasks of users"
            Me.TXT_MAX_JOBS_USERS.CaptionWidth = 50.0R
            Me.TXT_MAX_JOBS_USERS.ControlMode = PersonalUtilities.Forms.Controls.TextBoxExtended.ControlModes.NumericUpDown
            Me.TXT_MAX_JOBS_USERS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_MAX_JOBS_USERS.Location = New System.Drawing.Point(4, 91)
            Me.TXT_MAX_JOBS_USERS.Name = "TXT_MAX_JOBS_USERS"
            Me.TXT_MAX_JOBS_USERS.NumberMinimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.TXT_MAX_JOBS_USERS.Size = New System.Drawing.Size(562, 22)
            Me.TXT_MAX_JOBS_USERS.TabIndex = 3
            Me.TXT_MAX_JOBS_USERS.Text = "1"
            Me.TXT_MAX_JOBS_USERS.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'TXT_MAX_JOBS_CHANNELS
            '
            ActionButton5.BackgroundImage = CType(resources.GetObject("ActionButton5.BackgroundImage"), System.Drawing.Image)
            ActionButton5.Name = "Refresh"
            ActionButton5.ToolTipText = "Set to default"
            Me.TXT_MAX_JOBS_CHANNELS.Buttons.Add(ActionButton5)
            Me.TXT_MAX_JOBS_CHANNELS.CaptionSizeType = System.Windows.Forms.SizeType.Percent
            Me.TXT_MAX_JOBS_CHANNELS.CaptionText = "Maximum downloading tasks of channels"
            Me.TXT_MAX_JOBS_CHANNELS.CaptionWidth = 50.0R
            Me.TXT_MAX_JOBS_CHANNELS.ControlMode = PersonalUtilities.Forms.Controls.TextBoxExtended.ControlModes.NumericUpDown
            Me.TXT_MAX_JOBS_CHANNELS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_MAX_JOBS_CHANNELS.Location = New System.Drawing.Point(4, 120)
            Me.TXT_MAX_JOBS_CHANNELS.Name = "TXT_MAX_JOBS_CHANNELS"
            Me.TXT_MAX_JOBS_CHANNELS.NumberMinimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.TXT_MAX_JOBS_CHANNELS.Size = New System.Drawing.Size(562, 22)
            Me.TXT_MAX_JOBS_CHANNELS.TabIndex = 4
            Me.TXT_MAX_JOBS_CHANNELS.Text = "1"
            Me.TXT_MAX_JOBS_CHANNELS.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'CH_CHECK_VER_START
            '
            Me.CH_CHECK_VER_START.AutoSize = True
            Me.CH_CHECK_VER_START.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_CHECK_VER_START.Location = New System.Drawing.Point(4, 149)
            Me.CH_CHECK_VER_START.Name = "CH_CHECK_VER_START"
            Me.CH_CHECK_VER_START.Size = New System.Drawing.Size(562, 19)
            Me.CH_CHECK_VER_START.TabIndex = 5
            Me.CH_CHECK_VER_START.Text = "Check new version at start"
            Me.CH_CHECK_VER_START.UseVisualStyleBackColor = True
            '
            'TXT_IMGUR_CLIENT_ID
            '
            ActionButton6.BackgroundImage = CType(resources.GetObject("ActionButton6.BackgroundImage"), System.Drawing.Image)
            ActionButton6.Name = "Clear"
            Me.TXT_IMGUR_CLIENT_ID.Buttons.Add(ActionButton6)
            Me.TXT_IMGUR_CLIENT_ID.CaptionText = "Imgur Client ID"
            Me.TXT_IMGUR_CLIENT_ID.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_IMGUR_CLIENT_ID.Location = New System.Drawing.Point(4, 204)
            Me.TXT_IMGUR_CLIENT_ID.Name = "TXT_IMGUR_CLIENT_ID"
            Me.TXT_IMGUR_CLIENT_ID.Size = New System.Drawing.Size(562, 22)
            Me.TXT_IMGUR_CLIENT_ID.TabIndex = 7
            '
            'CH_SHOW_GROUPS
            '
            Me.CH_SHOW_GROUPS.AutoSize = True
            Me.CH_SHOW_GROUPS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_SHOW_GROUPS.Location = New System.Drawing.Point(4, 288)
            Me.CH_SHOW_GROUPS.Name = "CH_SHOW_GROUPS"
            Me.CH_SHOW_GROUPS.Size = New System.Drawing.Size(562, 19)
            Me.CH_SHOW_GROUPS.TabIndex = 10
            Me.CH_SHOW_GROUPS.Text = "Show groups"
            TT_MAIN.SetToolTip(Me.CH_SHOW_GROUPS, "Grouping users by site")
            Me.CH_SHOW_GROUPS.UseVisualStyleBackColor = True
            '
            'CH_USERS_GROUPING
            '
            Me.CH_USERS_GROUPING.AutoSize = True
            Me.CH_USERS_GROUPING.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_USERS_GROUPING.Location = New System.Drawing.Point(4, 314)
            Me.CH_USERS_GROUPING.Name = "CH_USERS_GROUPING"
            Me.CH_USERS_GROUPING.Size = New System.Drawing.Size(562, 19)
            Me.CH_USERS_GROUPING.TabIndex = 11
            Me.CH_USERS_GROUPING.Text = "Use user grouping"
            TT_MAIN.SetToolTip(Me.CH_USERS_GROUPING, "Group users by groups and/or labels")
            Me.CH_USERS_GROUPING.UseVisualStyleBackColor = True
            '
            'TXT_USER_AGENT
            '
            ActionButton7.BackgroundImage = CType(resources.GetObject("ActionButton7.BackgroundImage"), System.Drawing.Image)
            ActionButton7.Name = "Refresh"
            ActionButton7.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Refresh
            ActionButton8.BackgroundImage = CType(resources.GetObject("ActionButton8.BackgroundImage"), System.Drawing.Image)
            ActionButton8.Name = "Clear"
            ActionButton8.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_USER_AGENT.Buttons.Add(ActionButton7)
            Me.TXT_USER_AGENT.Buttons.Add(ActionButton8)
            Me.TXT_USER_AGENT.CaptionText = "UserAgent"
            Me.TXT_USER_AGENT.CaptionToolTipEnabled = True
            Me.TXT_USER_AGENT.CaptionToolTipText = "Default user agent to use in requests"
            Me.TXT_USER_AGENT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_USER_AGENT.Location = New System.Drawing.Point(4, 175)
            Me.TXT_USER_AGENT.Name = "TXT_USER_AGENT"
            Me.TXT_USER_AGENT.Size = New System.Drawing.Size(562, 22)
            Me.TXT_USER_AGENT.TabIndex = 6
            '
            'TXT_USER_LIST_IMAGE
            '
            ActionButton9.BackgroundImage = CType(resources.GetObject("ActionButton9.BackgroundImage"), System.Drawing.Image)
            ActionButton9.Name = "Open"
            ActionButton9.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Open
            ActionButton10.BackgroundImage = CType(resources.GetObject("ActionButton10.BackgroundImage"), System.Drawing.Image)
            ActionButton10.Name = "Clear"
            ActionButton10.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_USER_LIST_IMAGE.Buttons.Add(ActionButton9)
            Me.TXT_USER_LIST_IMAGE.Buttons.Add(ActionButton10)
            Me.TXT_USER_LIST_IMAGE.CaptionText = "Userlist image"
            Me.TXT_USER_LIST_IMAGE.CaptionToolTipEnabled = True
            Me.TXT_USER_LIST_IMAGE.CaptionToolTipText = "Background image for user list"
            Me.TXT_USER_LIST_IMAGE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_USER_LIST_IMAGE.Location = New System.Drawing.Point(4, 233)
            Me.TXT_USER_LIST_IMAGE.Name = "TXT_USER_LIST_IMAGE"
            Me.TXT_USER_LIST_IMAGE.Size = New System.Drawing.Size(562, 22)
            Me.TXT_USER_LIST_IMAGE.TabIndex = 8
            '
            'COLORS_USERLIST
            '
            Me.COLORS_USERLIST.ButtonsMargin = New System.Windows.Forms.Padding(1, 2, 1, 2)
            Me.COLORS_USERLIST.CaptionText = "Userlist colors"
            Me.COLORS_USERLIST.CaptionWidth = 103
            Me.COLORS_USERLIST.Dock = System.Windows.Forms.DockStyle.Fill
            Me.COLORS_USERLIST.Location = New System.Drawing.Point(1, 259)
            Me.COLORS_USERLIST.Margin = New System.Windows.Forms.Padding(0)
            Me.COLORS_USERLIST.Name = "COLORS_USERLIST"
            Me.COLORS_USERLIST.Padding = New System.Windows.Forms.Padding(0, 0, 2, 0)
            Me.COLORS_USERLIST.Size = New System.Drawing.Size(568, 25)
            Me.COLORS_USERLIST.TabIndex = 9
            '
            'TP_FILE_NAME
            '
            TP_FILE_NAME.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_FILE_NAME.ColumnCount = 3
            TP_FILE_NAME.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_FILE_NAME.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_FILE_NAME.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_FILE_NAME.Controls.Add(Me.OPT_FILE_NAME_REPLACE, 1, 0)
            TP_FILE_NAME.Controls.Add(Me.OPT_FILE_NAME_ADD_DATE, 2, 0)
            TP_FILE_NAME.Controls.Add(Me.CH_FILE_NAME_CHANGE, 0, 0)
            TP_FILE_NAME.Dock = System.Windows.Forms.DockStyle.Fill
            TP_FILE_NAME.Location = New System.Drawing.Point(1, 53)
            TP_FILE_NAME.Margin = New System.Windows.Forms.Padding(0)
            TP_FILE_NAME.Name = "TP_FILE_NAME"
            TP_FILE_NAME.RowCount = 1
            TP_FILE_NAME.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_FILE_NAME.Size = New System.Drawing.Size(574, 30)
            TP_FILE_NAME.TabIndex = 2
            '
            'OPT_FILE_NAME_REPLACE
            '
            Me.OPT_FILE_NAME_REPLACE.AutoSize = True
            Me.OPT_FILE_NAME_REPLACE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_FILE_NAME_REPLACE.Location = New System.Drawing.Point(195, 4)
            Me.OPT_FILE_NAME_REPLACE.Name = "OPT_FILE_NAME_REPLACE"
            Me.OPT_FILE_NAME_REPLACE.Size = New System.Drawing.Size(184, 22)
            Me.OPT_FILE_NAME_REPLACE.TabIndex = 1
            Me.OPT_FILE_NAME_REPLACE.TabStop = True
            Me.OPT_FILE_NAME_REPLACE.Text = "Replace file name by date"
            Me.OPT_FILE_NAME_REPLACE.UseVisualStyleBackColor = True
            '
            'OPT_FILE_NAME_ADD_DATE
            '
            Me.OPT_FILE_NAME_ADD_DATE.AutoSize = True
            Me.OPT_FILE_NAME_ADD_DATE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_FILE_NAME_ADD_DATE.Location = New System.Drawing.Point(386, 4)
            Me.OPT_FILE_NAME_ADD_DATE.Name = "OPT_FILE_NAME_ADD_DATE"
            Me.OPT_FILE_NAME_ADD_DATE.Size = New System.Drawing.Size(184, 22)
            Me.OPT_FILE_NAME_ADD_DATE.TabIndex = 2
            Me.OPT_FILE_NAME_ADD_DATE.TabStop = True
            Me.OPT_FILE_NAME_ADD_DATE.Text = "Add date/time to file name"
            Me.OPT_FILE_NAME_ADD_DATE.UseVisualStyleBackColor = True
            '
            'CH_FILE_NAME_CHANGE
            '
            Me.CH_FILE_NAME_CHANGE.AutoSize = True
            Me.CH_FILE_NAME_CHANGE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FILE_NAME_CHANGE.Location = New System.Drawing.Point(4, 4)
            Me.CH_FILE_NAME_CHANGE.Name = "CH_FILE_NAME_CHANGE"
            Me.CH_FILE_NAME_CHANGE.Size = New System.Drawing.Size(184, 22)
            Me.CH_FILE_NAME_CHANGE.TabIndex = 0
            Me.CH_FILE_NAME_CHANGE.Text = "Change file names"
            Me.CH_FILE_NAME_CHANGE.UseVisualStyleBackColor = True
            '
            'TP_FILE_PATTERNS
            '
            TP_FILE_PATTERNS.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_FILE_PATTERNS.ColumnCount = 5
            TP_FILE_PATTERNS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_FILE_PATTERNS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_FILE_PATTERNS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_FILE_PATTERNS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_FILE_PATTERNS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_FILE_PATTERNS.Controls.Add(Me.CH_FILE_DATE, 0, 0)
            TP_FILE_PATTERNS.Controls.Add(Me.CH_FILE_TIME, 1, 0)
            TP_FILE_PATTERNS.Controls.Add(LBL_DATE_POS, 2, 0)
            TP_FILE_PATTERNS.Controls.Add(Me.OPT_FILE_DATE_START, 3, 0)
            TP_FILE_PATTERNS.Controls.Add(Me.OPT_FILE_DATE_END, 4, 0)
            TP_FILE_PATTERNS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_FILE_PATTERNS.Location = New System.Drawing.Point(1, 84)
            TP_FILE_PATTERNS.Margin = New System.Windows.Forms.Padding(0)
            TP_FILE_PATTERNS.Name = "TP_FILE_PATTERNS"
            TP_FILE_PATTERNS.RowCount = 1
            TP_FILE_PATTERNS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_FILE_PATTERNS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29.0!))
            TP_FILE_PATTERNS.Size = New System.Drawing.Size(574, 30)
            TP_FILE_PATTERNS.TabIndex = 3
            '
            'CH_FILE_DATE
            '
            Me.CH_FILE_DATE.AutoSize = True
            Me.CH_FILE_DATE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FILE_DATE.Location = New System.Drawing.Point(4, 4)
            Me.CH_FILE_DATE.Name = "CH_FILE_DATE"
            Me.CH_FILE_DATE.Size = New System.Drawing.Size(107, 22)
            Me.CH_FILE_DATE.TabIndex = 0
            Me.CH_FILE_DATE.Text = "Date"
            Me.CH_FILE_DATE.UseVisualStyleBackColor = True
            '
            'CH_FILE_TIME
            '
            Me.CH_FILE_TIME.AutoSize = True
            Me.CH_FILE_TIME.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FILE_TIME.Location = New System.Drawing.Point(118, 4)
            Me.CH_FILE_TIME.Name = "CH_FILE_TIME"
            Me.CH_FILE_TIME.Size = New System.Drawing.Size(107, 22)
            Me.CH_FILE_TIME.TabIndex = 1
            Me.CH_FILE_TIME.Text = "Time"
            Me.CH_FILE_TIME.UseVisualStyleBackColor = True
            '
            'LBL_DATE_POS
            '
            LBL_DATE_POS.AutoSize = True
            LBL_DATE_POS.Dock = System.Windows.Forms.DockStyle.Fill
            LBL_DATE_POS.Location = New System.Drawing.Point(232, 1)
            LBL_DATE_POS.Name = "LBL_DATE_POS"
            LBL_DATE_POS.Size = New System.Drawing.Size(107, 28)
            LBL_DATE_POS.TabIndex = 2
            LBL_DATE_POS.Text = "Date position:"
            LBL_DATE_POS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'OPT_FILE_DATE_START
            '
            Me.OPT_FILE_DATE_START.AutoSize = True
            Me.OPT_FILE_DATE_START.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_FILE_DATE_START.Location = New System.Drawing.Point(346, 4)
            Me.OPT_FILE_DATE_START.Name = "OPT_FILE_DATE_START"
            Me.OPT_FILE_DATE_START.Size = New System.Drawing.Size(107, 22)
            Me.OPT_FILE_DATE_START.TabIndex = 3
            Me.OPT_FILE_DATE_START.TabStop = True
            Me.OPT_FILE_DATE_START.Text = "Start"
            Me.OPT_FILE_DATE_START.UseVisualStyleBackColor = True
            '
            'OPT_FILE_DATE_END
            '
            Me.OPT_FILE_DATE_END.AutoSize = True
            Me.OPT_FILE_DATE_END.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_FILE_DATE_END.Location = New System.Drawing.Point(460, 4)
            Me.OPT_FILE_DATE_END.Name = "OPT_FILE_DATE_END"
            Me.OPT_FILE_DATE_END.Size = New System.Drawing.Size(110, 22)
            Me.OPT_FILE_DATE_END.TabIndex = 4
            Me.OPT_FILE_DATE_END.TabStop = True
            Me.OPT_FILE_DATE_END.Text = "End"
            Me.OPT_FILE_DATE_END.UseVisualStyleBackColor = True
            '
            'CH_FAST_LOAD
            '
            Me.CH_FAST_LOAD.AutoSize = True
            Me.CH_FAST_LOAD.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FAST_LOAD.Location = New System.Drawing.Point(4, 56)
            Me.CH_FAST_LOAD.Name = "CH_FAST_LOAD"
            Me.CH_FAST_LOAD.Size = New System.Drawing.Size(568, 19)
            Me.CH_FAST_LOAD.TabIndex = 2
            Me.CH_FAST_LOAD.Text = "Fast profiles loading"
            TT_MAIN.SetToolTip(Me.CH_FAST_LOAD, "Fast loading of profiles in the main window")
            Me.CH_FAST_LOAD.UseVisualStyleBackColor = True
            '
            'CH_COPY_CHANNEL_USER_IMAGE
            '
            Me.CH_COPY_CHANNEL_USER_IMAGE.AutoSize = True
            Me.CH_COPY_CHANNEL_USER_IMAGE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_COPY_CHANNEL_USER_IMAGE.Location = New System.Drawing.Point(4, 62)
            Me.CH_COPY_CHANNEL_USER_IMAGE.Name = "CH_COPY_CHANNEL_USER_IMAGE"
            Me.CH_COPY_CHANNEL_USER_IMAGE.Size = New System.Drawing.Size(562, 19)
            Me.CH_COPY_CHANNEL_USER_IMAGE.TabIndex = 2
            Me.CH_COPY_CHANNEL_USER_IMAGE.Text = "Copy channel user image"
            TT_MAIN.SetToolTip(Me.CH_COPY_CHANNEL_USER_IMAGE, "Copy image posted by user (in the channel you added from) to the user's destinati" &
        "on folder.")
            Me.CH_COPY_CHANNEL_USER_IMAGE.UseVisualStyleBackColor = True
            '
            'CH_DEF_TEMP
            '
            Me.CH_DEF_TEMP.AutoSize = True
            Me.CH_DEF_TEMP.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DEF_TEMP.Location = New System.Drawing.Point(4, 30)
            Me.CH_DEF_TEMP.Name = "CH_DEF_TEMP"
            Me.CH_DEF_TEMP.Size = New System.Drawing.Size(562, 19)
            Me.CH_DEF_TEMP.TabIndex = 1
            Me.CH_DEF_TEMP.Text = "Temporary"
            TT_MAIN.SetToolTip(Me.CH_DEF_TEMP, "Default value when creating a new user (can be changed in the new user form)")
            Me.CH_DEF_TEMP.UseVisualStyleBackColor = True
            '
            'CH_DOWN_IMAGES
            '
            Me.CH_DOWN_IMAGES.AutoSize = True
            Me.CH_DOWN_IMAGES.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DOWN_IMAGES.Location = New System.Drawing.Point(4, 56)
            Me.CH_DOWN_IMAGES.Name = "CH_DOWN_IMAGES"
            Me.CH_DOWN_IMAGES.Size = New System.Drawing.Size(562, 19)
            Me.CH_DOWN_IMAGES.TabIndex = 2
            Me.CH_DOWN_IMAGES.Text = "Download images"
            TT_MAIN.SetToolTip(Me.CH_DOWN_IMAGES, "Default value when creating a new user (can be changed in the new user form)")
            Me.CH_DOWN_IMAGES.UseVisualStyleBackColor = True
            '
            'CH_DOWN_VIDEOS
            '
            Me.CH_DOWN_VIDEOS.AutoSize = True
            Me.CH_DOWN_VIDEOS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DOWN_VIDEOS.Location = New System.Drawing.Point(4, 82)
            Me.CH_DOWN_VIDEOS.Name = "CH_DOWN_VIDEOS"
            Me.CH_DOWN_VIDEOS.Size = New System.Drawing.Size(562, 19)
            Me.CH_DOWN_VIDEOS.TabIndex = 3
            Me.CH_DOWN_VIDEOS.Text = "Download videos"
            TT_MAIN.SetToolTip(Me.CH_DOWN_VIDEOS, "Default value when creating a new user (can be changed in the new user form)")
            Me.CH_DOWN_VIDEOS.UseVisualStyleBackColor = True
            '
            'CH_SEPARATE_VIDEO_FOLDER
            '
            Me.CH_SEPARATE_VIDEO_FOLDER.AutoSize = True
            Me.CH_SEPARATE_VIDEO_FOLDER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_SEPARATE_VIDEO_FOLDER.Location = New System.Drawing.Point(4, 4)
            Me.CH_SEPARATE_VIDEO_FOLDER.Name = "CH_SEPARATE_VIDEO_FOLDER"
            Me.CH_SEPARATE_VIDEO_FOLDER.Size = New System.Drawing.Size(562, 19)
            Me.CH_SEPARATE_VIDEO_FOLDER.TabIndex = 0
            Me.CH_SEPARATE_VIDEO_FOLDER.Text = "Separate video folders"
            TT_MAIN.SetToolTip(Me.CH_SEPARATE_VIDEO_FOLDER, resources.GetString("CH_SEPARATE_VIDEO_FOLDER.ToolTip"))
            Me.CH_SEPARATE_VIDEO_FOLDER.UseVisualStyleBackColor = True
            '
            'CH_CHANNELS_USERS_TEMP
            '
            Me.CH_CHANNELS_USERS_TEMP.AutoSize = True
            Me.CH_CHANNELS_USERS_TEMP.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_CHANNELS_USERS_TEMP.Location = New System.Drawing.Point(4, 114)
            Me.CH_CHANNELS_USERS_TEMP.Name = "CH_CHANNELS_USERS_TEMP"
            Me.CH_CHANNELS_USERS_TEMP.Size = New System.Drawing.Size(562, 19)
            Me.CH_CHANNELS_USERS_TEMP.TabIndex = 4
            Me.CH_CHANNELS_USERS_TEMP.Text = "Create temporary users"
            TT_MAIN.SetToolTip(Me.CH_CHANNELS_USERS_TEMP, "Users added from channels will be created with this parameter")
            Me.CH_CHANNELS_USERS_TEMP.UseVisualStyleBackColor = True
            '
            'CH_COPY_CHANNEL_USER_IMAGE_ALL
            '
            Me.CH_COPY_CHANNEL_USER_IMAGE_ALL.AutoSize = True
            Me.CH_COPY_CHANNEL_USER_IMAGE_ALL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_COPY_CHANNEL_USER_IMAGE_ALL.Location = New System.Drawing.Point(4, 88)
            Me.CH_COPY_CHANNEL_USER_IMAGE_ALL.Name = "CH_COPY_CHANNEL_USER_IMAGE_ALL"
            Me.CH_COPY_CHANNEL_USER_IMAGE_ALL.Size = New System.Drawing.Size(562, 19)
            Me.CH_COPY_CHANNEL_USER_IMAGE_ALL.TabIndex = 3
            Me.CH_COPY_CHANNEL_USER_IMAGE_ALL.Text = "Copy images from all channels"
            TT_MAIN.SetToolTip(Me.CH_COPY_CHANNEL_USER_IMAGE_ALL, "Copy user images from all channels you have when adding a user from a channel")
            Me.CH_COPY_CHANNEL_USER_IMAGE_ALL.UseVisualStyleBackColor = True
            '
            'CH_UDESCR_UP
            '
            Me.CH_UDESCR_UP.AutoSize = True
            Me.CH_UDESCR_UP.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_UDESCR_UP.Location = New System.Drawing.Point(4, 4)
            Me.CH_UDESCR_UP.Name = "CH_UDESCR_UP"
            Me.CH_UDESCR_UP.Size = New System.Drawing.Size(568, 19)
            Me.CH_UDESCR_UP.TabIndex = 0
            Me.CH_UDESCR_UP.Text = "Update user description every time"
            TT_MAIN.SetToolTip(Me.CH_UDESCR_UP, "If the user description does not contain a new user description, then the new one" &
        " will be added via a new line")
            Me.CH_UDESCR_UP.UseVisualStyleBackColor = True
            '
            'CH_DOWN_OPEN_INFO_SUSPEND
            '
            Me.CH_DOWN_OPEN_INFO_SUSPEND.AutoSize = True
            Me.CH_DOWN_OPEN_INFO_SUSPEND.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DOWN_OPEN_INFO_SUSPEND.Location = New System.Drawing.Point(290, 4)
            Me.CH_DOWN_OPEN_INFO_SUSPEND.Name = "CH_DOWN_OPEN_INFO_SUSPEND"
            Me.CH_DOWN_OPEN_INFO_SUSPEND.Size = New System.Drawing.Size(280, 17)
            Me.CH_DOWN_OPEN_INFO_SUSPEND.TabIndex = 1
            Me.CH_DOWN_OPEN_INFO_SUSPEND.Text = "Don't open again"
            TT_MAIN.SetToolTip(Me.CH_DOWN_OPEN_INFO_SUSPEND, "Do not open the form automatically if it was once closed")
            Me.CH_DOWN_OPEN_INFO_SUSPEND.UseVisualStyleBackColor = True
            '
            'CH_DOWN_OPEN_PROGRESS_SUSPEND
            '
            Me.CH_DOWN_OPEN_PROGRESS_SUSPEND.AutoSize = True
            Me.CH_DOWN_OPEN_PROGRESS_SUSPEND.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DOWN_OPEN_PROGRESS_SUSPEND.Location = New System.Drawing.Point(290, 4)
            Me.CH_DOWN_OPEN_PROGRESS_SUSPEND.Name = "CH_DOWN_OPEN_PROGRESS_SUSPEND"
            Me.CH_DOWN_OPEN_PROGRESS_SUSPEND.Size = New System.Drawing.Size(280, 17)
            Me.CH_DOWN_OPEN_PROGRESS_SUSPEND.TabIndex = 1
            Me.CH_DOWN_OPEN_PROGRESS_SUSPEND.Text = "Don't open again"
            TT_MAIN.SetToolTip(Me.CH_DOWN_OPEN_PROGRESS_SUSPEND, "Do not open the form automatically if it was once closed")
            Me.CH_DOWN_OPEN_PROGRESS_SUSPEND.UseVisualStyleBackColor = True
            '
            'CH_ADD_MISSING_TO_LOG
            '
            Me.CH_ADD_MISSING_TO_LOG.AutoSize = True
            Me.CH_ADD_MISSING_TO_LOG.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_ADD_MISSING_TO_LOG.Location = New System.Drawing.Point(4, 4)
            Me.CH_ADD_MISSING_TO_LOG.Name = "CH_ADD_MISSING_TO_LOG"
            Me.CH_ADD_MISSING_TO_LOG.Size = New System.Drawing.Size(279, 17)
            Me.CH_ADD_MISSING_TO_LOG.TabIndex = 0
            Me.CH_ADD_MISSING_TO_LOG.Text = "Add 'missing' information to log"
            TT_MAIN.SetToolTip(Me.CH_ADD_MISSING_TO_LOG, resources.GetString("CH_ADD_MISSING_TO_LOG.ToolTip"))
            Me.CH_ADD_MISSING_TO_LOG.UseVisualStyleBackColor = True
            '
            'CH_ADD_MISSING_ERROS_TO_LOG
            '
            Me.CH_ADD_MISSING_ERROS_TO_LOG.AutoSize = True
            Me.CH_ADD_MISSING_ERROS_TO_LOG.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_ADD_MISSING_ERROS_TO_LOG.Location = New System.Drawing.Point(290, 4)
            Me.CH_ADD_MISSING_ERROS_TO_LOG.Name = "CH_ADD_MISSING_ERROS_TO_LOG"
            Me.CH_ADD_MISSING_ERROS_TO_LOG.Size = New System.Drawing.Size(280, 17)
            Me.CH_ADD_MISSING_ERROS_TO_LOG.TabIndex = 1
            Me.CH_ADD_MISSING_ERROS_TO_LOG.Text = "Add 'missing' errors to log"
            TT_MAIN.SetToolTip(Me.CH_ADD_MISSING_ERROS_TO_LOG, resources.GetString("CH_ADD_MISSING_ERROS_TO_LOG.ToolTip"))
            Me.CH_ADD_MISSING_ERROS_TO_LOG.UseVisualStyleBackColor = True
            '
            'CH_FEED_STORE_SESSION_DATA
            '
            Me.CH_FEED_STORE_SESSION_DATA.AutoSize = True
            Me.CH_FEED_STORE_SESSION_DATA.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FEED_STORE_SESSION_DATA.Location = New System.Drawing.Point(4, 166)
            Me.CH_FEED_STORE_SESSION_DATA.Name = "CH_FEED_STORE_SESSION_DATA"
            Me.CH_FEED_STORE_SESSION_DATA.Size = New System.Drawing.Size(568, 19)
            Me.CH_FEED_STORE_SESSION_DATA.TabIndex = 6
            Me.CH_FEED_STORE_SESSION_DATA.Text = "Store session data"
            TT_MAIN.SetToolTip(Me.CH_FEED_STORE_SESSION_DATA, "If checked, session data will be stored in an xml file.")
            Me.CH_FEED_STORE_SESSION_DATA.UseVisualStyleBackColor = True
            '
            'CH_NOTIFY_SHOW_BASE
            '
            Me.CH_NOTIFY_SHOW_BASE.AutoSize = True
            Me.CH_NOTIFY_SHOW_BASE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_NOTIFY_SHOW_BASE.Location = New System.Drawing.Point(4, 30)
            Me.CH_NOTIFY_SHOW_BASE.Name = "CH_NOTIFY_SHOW_BASE"
            Me.CH_NOTIFY_SHOW_BASE.Size = New System.Drawing.Size(568, 19)
            Me.CH_NOTIFY_SHOW_BASE.TabIndex = 1
            Me.CH_NOTIFY_SHOW_BASE.Text = "Show notifications"
            TT_MAIN.SetToolTip(Me.CH_NOTIFY_SHOW_BASE, "This is the base value of notifications. If you disable it, notifications will no" &
        "t appear at all.")
            Me.CH_NOTIFY_SHOW_BASE.UseVisualStyleBackColor = True
            '
            'CH_NOTIFY_SILENT
            '
            Me.CH_NOTIFY_SILENT.AutoSize = True
            Me.CH_NOTIFY_SILENT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_NOTIFY_SILENT.Location = New System.Drawing.Point(4, 4)
            Me.CH_NOTIFY_SILENT.Name = "CH_NOTIFY_SILENT"
            Me.CH_NOTIFY_SILENT.Size = New System.Drawing.Size(568, 19)
            Me.CH_NOTIFY_SILENT.TabIndex = 0
            Me.CH_NOTIFY_SILENT.Text = "Silent mode"
            TT_MAIN.SetToolTip(Me.CH_NOTIFY_SILENT, "Turn off notifications temporarily." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "This setting is not stored in the settings f" &
        "ile. It is valid until you turn it off or close the program.")
            Me.CH_NOTIFY_SILENT.UseVisualStyleBackColor = True
            '
            'CH_NOTIFY_PROFILES
            '
            Me.CH_NOTIFY_PROFILES.AutoSize = True
            Me.CH_NOTIFY_PROFILES.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_NOTIFY_PROFILES.Location = New System.Drawing.Point(4, 56)
            Me.CH_NOTIFY_PROFILES.Name = "CH_NOTIFY_PROFILES"
            Me.CH_NOTIFY_PROFILES.Size = New System.Drawing.Size(568, 19)
            Me.CH_NOTIFY_PROFILES.TabIndex = 2
            Me.CH_NOTIFY_PROFILES.Text = "Profiles"
            TT_MAIN.SetToolTip(Me.CH_NOTIFY_PROFILES, "Show notifications when profiles download is complete")
            Me.CH_NOTIFY_PROFILES.UseVisualStyleBackColor = True
            '
            'CH_NOTIFY_AUTO_DOWN
            '
            Me.CH_NOTIFY_AUTO_DOWN.AutoSize = True
            Me.CH_NOTIFY_AUTO_DOWN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_NOTIFY_AUTO_DOWN.Location = New System.Drawing.Point(4, 82)
            Me.CH_NOTIFY_AUTO_DOWN.Name = "CH_NOTIFY_AUTO_DOWN"
            Me.CH_NOTIFY_AUTO_DOWN.Size = New System.Drawing.Size(568, 19)
            Me.CH_NOTIFY_AUTO_DOWN.TabIndex = 3
            Me.CH_NOTIFY_AUTO_DOWN.Text = "AutoDownloader"
            TT_MAIN.SetToolTip(Me.CH_NOTIFY_AUTO_DOWN, "Show AutoDownloader notifications")
            Me.CH_NOTIFY_AUTO_DOWN.UseVisualStyleBackColor = True
            '
            'CH_NOTIFY_CHANNELS
            '
            Me.CH_NOTIFY_CHANNELS.AutoSize = True
            Me.CH_NOTIFY_CHANNELS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_NOTIFY_CHANNELS.Location = New System.Drawing.Point(4, 108)
            Me.CH_NOTIFY_CHANNELS.Name = "CH_NOTIFY_CHANNELS"
            Me.CH_NOTIFY_CHANNELS.Size = New System.Drawing.Size(568, 19)
            Me.CH_NOTIFY_CHANNELS.TabIndex = 4
            Me.CH_NOTIFY_CHANNELS.Text = "Channels"
            TT_MAIN.SetToolTip(Me.CH_NOTIFY_CHANNELS, "Show notifications when channels download is complete")
            Me.CH_NOTIFY_CHANNELS.UseVisualStyleBackColor = True
            '
            'CH_NOTIFY_SAVED_POSTS
            '
            Me.CH_NOTIFY_SAVED_POSTS.AutoSize = True
            Me.CH_NOTIFY_SAVED_POSTS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_NOTIFY_SAVED_POSTS.Location = New System.Drawing.Point(4, 134)
            Me.CH_NOTIFY_SAVED_POSTS.Name = "CH_NOTIFY_SAVED_POSTS"
            Me.CH_NOTIFY_SAVED_POSTS.Size = New System.Drawing.Size(568, 19)
            Me.CH_NOTIFY_SAVED_POSTS.TabIndex = 5
            Me.CH_NOTIFY_SAVED_POSTS.Text = "Saved posts"
            TT_MAIN.SetToolTip(Me.CH_NOTIFY_SAVED_POSTS, "Show notifications when saved posts download is complete")
            Me.CH_NOTIFY_SAVED_POSTS.UseVisualStyleBackColor = True
            '
            'CH_DOWN_REPARSE_MISSING
            '
            Me.CH_DOWN_REPARSE_MISSING.AutoSize = True
            Me.CH_DOWN_REPARSE_MISSING.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DOWN_REPARSE_MISSING.Location = New System.Drawing.Point(4, 202)
            Me.CH_DOWN_REPARSE_MISSING.Name = "CH_DOWN_REPARSE_MISSING"
            Me.CH_DOWN_REPARSE_MISSING.Size = New System.Drawing.Size(568, 19)
            Me.CH_DOWN_REPARSE_MISSING.TabIndex = 7
            Me.CH_DOWN_REPARSE_MISSING.Text = "Trying to download missing posts using regular download"
            TT_MAIN.SetToolTip(Me.CH_DOWN_REPARSE_MISSING, "If missing posts exist, the missing posts will attempt to be downloaded via user " &
        "download")
            Me.CH_DOWN_REPARSE_MISSING.UseVisualStyleBackColor = True
            '
            'CH_NAME_SITE_FRIENDLY
            '
            Me.CH_NAME_SITE_FRIENDLY.AutoSize = True
            Me.CH_NAME_SITE_FRIENDLY.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_NAME_SITE_FRIENDLY.Location = New System.Drawing.Point(4, 134)
            Me.CH_NAME_SITE_FRIENDLY.Name = "CH_NAME_SITE_FRIENDLY"
            Me.CH_NAME_SITE_FRIENDLY.Size = New System.Drawing.Size(562, 19)
            Me.CH_NAME_SITE_FRIENDLY.TabIndex = 5
            Me.CH_NAME_SITE_FRIENDLY.Text = "Use the site name as a friendly name"
            TT_MAIN.SetToolTip(Me.CH_NAME_SITE_FRIENDLY, "Use the user's site name as a friendly name")
            Me.CH_NAME_SITE_FRIENDLY.UseVisualStyleBackColor = True
            '
            'TP_CHANNELS_IMGS
            '
            TP_CHANNELS_IMGS.ColumnCount = 2
            TP_CHANNELS_IMGS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_CHANNELS_IMGS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_CHANNELS_IMGS.Controls.Add(Me.TXT_CHANNELS_ROWS, 0, 0)
            TP_CHANNELS_IMGS.Controls.Add(Me.TXT_CHANNELS_COLUMNS, 1, 0)
            TP_CHANNELS_IMGS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_CHANNELS_IMGS.Location = New System.Drawing.Point(1, 1)
            TP_CHANNELS_IMGS.Margin = New System.Windows.Forms.Padding(0)
            TP_CHANNELS_IMGS.Name = "TP_CHANNELS_IMGS"
            TP_CHANNELS_IMGS.RowCount = 1
            TP_CHANNELS_IMGS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_CHANNELS_IMGS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_CHANNELS_IMGS.Size = New System.Drawing.Size(568, 28)
            TP_CHANNELS_IMGS.TabIndex = 0
            '
            'TXT_CHANNELS_ROWS
            '
            Me.TXT_CHANNELS_ROWS.CaptionText = "Channels rows"
            Me.TXT_CHANNELS_ROWS.CaptionToolTipEnabled = True
            Me.TXT_CHANNELS_ROWS.CaptionToolTipText = "How many lines of images should be shown in the channels form"
            Me.TXT_CHANNELS_ROWS.ControlMode = PersonalUtilities.Forms.Controls.TextBoxExtended.ControlModes.NumericUpDown
            Me.TXT_CHANNELS_ROWS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_CHANNELS_ROWS.Location = New System.Drawing.Point(3, 3)
            Me.TXT_CHANNELS_ROWS.Name = "TXT_CHANNELS_ROWS"
            Me.TXT_CHANNELS_ROWS.Size = New System.Drawing.Size(278, 22)
            Me.TXT_CHANNELS_ROWS.TabIndex = 0
            Me.TXT_CHANNELS_ROWS.Text = "0"
            Me.TXT_CHANNELS_ROWS.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'TXT_CHANNELS_COLUMNS
            '
            Me.TXT_CHANNELS_COLUMNS.CaptionText = "Channels columns"
            Me.TXT_CHANNELS_COLUMNS.CaptionToolTipEnabled = True
            Me.TXT_CHANNELS_COLUMNS.CaptionToolTipText = "How many columns of images should be shown in the channels form"
            Me.TXT_CHANNELS_COLUMNS.ControlMode = PersonalUtilities.Forms.Controls.TextBoxExtended.ControlModes.NumericUpDown
            Me.TXT_CHANNELS_COLUMNS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_CHANNELS_COLUMNS.Location = New System.Drawing.Point(287, 3)
            Me.TXT_CHANNELS_COLUMNS.Name = "TXT_CHANNELS_COLUMNS"
            Me.TXT_CHANNELS_COLUMNS.Size = New System.Drawing.Size(278, 22)
            Me.TXT_CHANNELS_COLUMNS.TabIndex = 1
            Me.TXT_CHANNELS_COLUMNS.Text = "0"
            Me.TXT_CHANNELS_COLUMNS.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'TAB_BASIS
            '
            TAB_BASIS.Controls.Add(TP_BASIS)
            TAB_BASIS.Location = New System.Drawing.Point(4, 22)
            TAB_BASIS.Name = "TAB_BASIS"
            TAB_BASIS.Padding = New System.Windows.Forms.Padding(3)
            TAB_BASIS.Size = New System.Drawing.Size(576, 343)
            TAB_BASIS.TabIndex = 0
            TAB_BASIS.Text = "Basis"
            '
            'TAB_DEFAULTS
            '
            TAB_DEFAULTS.Controls.Add(TP_DEFS)
            TAB_DEFAULTS.Location = New System.Drawing.Point(4, 22)
            TAB_DEFAULTS.Name = "TAB_DEFAULTS"
            TAB_DEFAULTS.Padding = New System.Windows.Forms.Padding(3)
            TAB_DEFAULTS.Size = New System.Drawing.Size(576, 486)
            TAB_DEFAULTS.TabIndex = 1
            TAB_DEFAULTS.Text = "Defaults"
            '
            'TP_DEFS
            '
            TP_DEFS.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_DEFS.ColumnCount = 1
            TP_DEFS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_DEFS.Controls.Add(Me.CH_SEPARATE_VIDEO_FOLDER, 0, 0)
            TP_DEFS.Controls.Add(Me.CH_DOWN_VIDEOS, 0, 3)
            TP_DEFS.Controls.Add(Me.CH_DOWN_IMAGES, 0, 2)
            TP_DEFS.Controls.Add(Me.CH_DEF_TEMP, 0, 1)
            TP_DEFS.Controls.Add(Me.CH_DOWN_IMAGES_NATIVE, 0, 4)
            TP_DEFS.Controls.Add(Me.CH_NAME_SITE_FRIENDLY, 0, 5)
            TP_DEFS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_DEFS.Location = New System.Drawing.Point(3, 3)
            TP_DEFS.Name = "TP_DEFS"
            TP_DEFS.RowCount = 7
            TP_DEFS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_DEFS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_DEFS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_DEFS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_DEFS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_DEFS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_DEFS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_DEFS.Size = New System.Drawing.Size(570, 480)
            TP_DEFS.TabIndex = 0
            '
            'CH_DOWN_IMAGES_NATIVE
            '
            Me.CH_DOWN_IMAGES_NATIVE.AutoSize = True
            Me.CH_DOWN_IMAGES_NATIVE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DOWN_IMAGES_NATIVE.Location = New System.Drawing.Point(4, 108)
            Me.CH_DOWN_IMAGES_NATIVE.Name = "CH_DOWN_IMAGES_NATIVE"
            Me.CH_DOWN_IMAGES_NATIVE.Size = New System.Drawing.Size(562, 19)
            Me.CH_DOWN_IMAGES_NATIVE.TabIndex = 4
            Me.CH_DOWN_IMAGES_NATIVE.Text = "Download 'jpg' instead of 'webp'"
            Me.CH_DOWN_IMAGES_NATIVE.UseVisualStyleBackColor = True
            '
            'TAB_DEFS_CHANNELS
            '
            TAB_DEFS_CHANNELS.Controls.Add(TP_CHANNELS)
            TAB_DEFS_CHANNELS.Location = New System.Drawing.Point(4, 22)
            TAB_DEFS_CHANNELS.Name = "TAB_DEFS_CHANNELS"
            TAB_DEFS_CHANNELS.Padding = New System.Windows.Forms.Padding(3)
            TAB_DEFS_CHANNELS.Size = New System.Drawing.Size(576, 486)
            TAB_DEFS_CHANNELS.TabIndex = 4
            TAB_DEFS_CHANNELS.Text = "Channels"
            '
            'TP_CHANNELS
            '
            TP_CHANNELS.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_CHANNELS.ColumnCount = 1
            TP_CHANNELS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_CHANNELS.Controls.Add(Me.TXT_CHANNEL_USER_POST_LIMIT, 0, 1)
            TP_CHANNELS.Controls.Add(TP_CHANNELS_IMGS, 0, 0)
            TP_CHANNELS.Controls.Add(Me.CH_COPY_CHANNEL_USER_IMAGE, 0, 2)
            TP_CHANNELS.Controls.Add(Me.CH_CHANNELS_USERS_TEMP, 0, 4)
            TP_CHANNELS.Controls.Add(Me.CH_COPY_CHANNEL_USER_IMAGE_ALL, 0, 3)
            TP_CHANNELS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_CHANNELS.Location = New System.Drawing.Point(3, 3)
            TP_CHANNELS.Name = "TP_CHANNELS"
            TP_CHANNELS.RowCount = 6
            TP_CHANNELS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_CHANNELS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_CHANNELS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_CHANNELS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_CHANNELS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_CHANNELS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_CHANNELS.Size = New System.Drawing.Size(570, 480)
            TP_CHANNELS.TabIndex = 0
            '
            'TXT_CHANNEL_USER_POST_LIMIT
            '
            Me.TXT_CHANNEL_USER_POST_LIMIT.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
            Me.TXT_CHANNEL_USER_POST_LIMIT.CaptionSizeType = System.Windows.Forms.SizeType.Percent
            Me.TXT_CHANNEL_USER_POST_LIMIT.CaptionText = "Download limit for channel user"
            Me.TXT_CHANNEL_USER_POST_LIMIT.CaptionToolTipEnabled = True
            Me.TXT_CHANNEL_USER_POST_LIMIT.CaptionToolTipText = "Set a limit on the number of downloads of posts limit if the user is added from t" &
    "he channel"
            Me.TXT_CHANNEL_USER_POST_LIMIT.CaptionWidth = 50.0R
            Me.TXT_CHANNEL_USER_POST_LIMIT.ControlMode = PersonalUtilities.Forms.Controls.TextBoxExtended.ControlModes.NumericUpDown
            Me.TXT_CHANNEL_USER_POST_LIMIT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_CHANNEL_USER_POST_LIMIT.Location = New System.Drawing.Point(4, 33)
            Me.TXT_CHANNEL_USER_POST_LIMIT.Name = "TXT_CHANNEL_USER_POST_LIMIT"
            Me.TXT_CHANNEL_USER_POST_LIMIT.NumberMaximum = New Decimal(New Integer() {1000, 0, 0, 0})
            Me.TXT_CHANNEL_USER_POST_LIMIT.NumberMinimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.TXT_CHANNEL_USER_POST_LIMIT.Size = New System.Drawing.Size(562, 22)
            Me.TXT_CHANNEL_USER_POST_LIMIT.TabIndex = 1
            Me.TXT_CHANNEL_USER_POST_LIMIT.Text = "1"
            Me.TXT_CHANNEL_USER_POST_LIMIT.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'TAB_BEHAVIOR
            '
            TAB_BEHAVIOR.Controls.Add(TP_BEHAVIOR)
            TAB_BEHAVIOR.Location = New System.Drawing.Point(4, 22)
            TAB_BEHAVIOR.Name = "TAB_BEHAVIOR"
            TAB_BEHAVIOR.Size = New System.Drawing.Size(576, 486)
            TAB_BEHAVIOR.TabIndex = 5
            TAB_BEHAVIOR.Text = "Behavior"
            '
            'TP_BEHAVIOR
            '
            TP_BEHAVIOR.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_BEHAVIOR.ColumnCount = 1
            TP_BEHAVIOR.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_BEHAVIOR.Controls.Add(Me.TXT_FOLDER_CMD, 0, 6)
            TP_BEHAVIOR.Controls.Add(Me.CH_EXIT_CONFIRM, 0, 0)
            TP_BEHAVIOR.Controls.Add(Me.CH_CLOSE_TO_TRAY, 0, 1)
            TP_BEHAVIOR.Controls.Add(Me.CH_FAST_LOAD, 0, 2)
            TP_BEHAVIOR.Controls.Add(Me.TXT_CLOSE_SCRIPT, 0, 7)
            TP_BEHAVIOR.Controls.Add(TP_OPEN_INFO, 0, 4)
            TP_BEHAVIOR.Controls.Add(Me.CH_RECYCLE_DEL, 0, 3)
            TP_BEHAVIOR.Controls.Add(TP_OPEN_PROGRESS, 0, 5)
            TP_BEHAVIOR.Dock = System.Windows.Forms.DockStyle.Fill
            TP_BEHAVIOR.Location = New System.Drawing.Point(0, 0)
            TP_BEHAVIOR.Name = "TP_BEHAVIOR"
            TP_BEHAVIOR.RowCount = 9
            TP_BEHAVIOR.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_BEHAVIOR.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_BEHAVIOR.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_BEHAVIOR.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_BEHAVIOR.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_BEHAVIOR.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_BEHAVIOR.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_BEHAVIOR.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_BEHAVIOR.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_BEHAVIOR.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_BEHAVIOR.Size = New System.Drawing.Size(576, 486)
            TP_BEHAVIOR.TabIndex = 0
            '
            'TXT_FOLDER_CMD
            '
            Me.TXT_FOLDER_CMD.AutoShowClearButton = True
            ActionButton11.BackgroundImage = CType(resources.GetObject("ActionButton11.BackgroundImage"), System.Drawing.Image)
            ActionButton11.Enabled = False
            ActionButton11.Name = "Clear"
            ActionButton11.Visible = False
            Me.TXT_FOLDER_CMD.Buttons.Add(ActionButton11)
            Me.TXT_FOLDER_CMD.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
            Me.TXT_FOLDER_CMD.CaptionText = "Folder cmd"
            Me.TXT_FOLDER_CMD.CaptionToolTipEnabled = True
            Me.TXT_FOLDER_CMD.CaptionToolTipText = "The command to open a folder."
            Me.TXT_FOLDER_CMD.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_FOLDER_CMD.LeaveDefaultButtons = True
            Me.TXT_FOLDER_CMD.Location = New System.Drawing.Point(4, 160)
            Me.TXT_FOLDER_CMD.Name = "TXT_FOLDER_CMD"
            Me.TXT_FOLDER_CMD.PlaceholderEnabled = True
            Me.TXT_FOLDER_CMD.PlaceholderText = "MyCommand /arg {0}"
            Me.TXT_FOLDER_CMD.Size = New System.Drawing.Size(568, 22)
            Me.TXT_FOLDER_CMD.TabIndex = 6
            '
            'CH_EXIT_CONFIRM
            '
            Me.CH_EXIT_CONFIRM.AutoSize = True
            Me.CH_EXIT_CONFIRM.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_EXIT_CONFIRM.Location = New System.Drawing.Point(4, 4)
            Me.CH_EXIT_CONFIRM.Name = "CH_EXIT_CONFIRM"
            Me.CH_EXIT_CONFIRM.Size = New System.Drawing.Size(568, 19)
            Me.CH_EXIT_CONFIRM.TabIndex = 0
            Me.CH_EXIT_CONFIRM.Text = "Exit confirm"
            Me.CH_EXIT_CONFIRM.UseVisualStyleBackColor = True
            '
            'CH_CLOSE_TO_TRAY
            '
            Me.CH_CLOSE_TO_TRAY.AutoSize = True
            Me.CH_CLOSE_TO_TRAY.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_CLOSE_TO_TRAY.Location = New System.Drawing.Point(4, 30)
            Me.CH_CLOSE_TO_TRAY.Name = "CH_CLOSE_TO_TRAY"
            Me.CH_CLOSE_TO_TRAY.Size = New System.Drawing.Size(568, 19)
            Me.CH_CLOSE_TO_TRAY.TabIndex = 1
            Me.CH_CLOSE_TO_TRAY.Text = "Close to tray"
            Me.CH_CLOSE_TO_TRAY.UseVisualStyleBackColor = True
            '
            'TXT_CLOSE_SCRIPT
            '
            Me.TXT_CLOSE_SCRIPT.AutoShowClearButton = True
            ActionButton12.BackgroundImage = CType(resources.GetObject("ActionButton12.BackgroundImage"), System.Drawing.Image)
            ActionButton12.Enabled = False
            ActionButton12.Name = "Clear"
            ActionButton12.Visible = False
            Me.TXT_CLOSE_SCRIPT.Buttons.Add(ActionButton12)
            Me.TXT_CLOSE_SCRIPT.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
            Me.TXT_CLOSE_SCRIPT.CaptionText = "Close cmd"
            Me.TXT_CLOSE_SCRIPT.CaptionToolTipEnabled = True
            Me.TXT_CLOSE_SCRIPT.CaptionToolTipText = "This command will be executed when SCrawler is closed"
            Me.TXT_CLOSE_SCRIPT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_CLOSE_SCRIPT.LeaveDefaultButtons = True
            Me.TXT_CLOSE_SCRIPT.Location = New System.Drawing.Point(4, 189)
            Me.TXT_CLOSE_SCRIPT.Name = "TXT_CLOSE_SCRIPT"
            Me.TXT_CLOSE_SCRIPT.PlaceholderEnabled = True
            Me.TXT_CLOSE_SCRIPT.PlaceholderText = "Enter command here..."
            Me.TXT_CLOSE_SCRIPT.Size = New System.Drawing.Size(568, 22)
            Me.TXT_CLOSE_SCRIPT.TabIndex = 7
            '
            'TP_OPEN_INFO
            '
            TP_OPEN_INFO.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_OPEN_INFO.ColumnCount = 2
            TP_OPEN_INFO.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_OPEN_INFO.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_OPEN_INFO.Controls.Add(Me.CH_DOWN_OPEN_INFO, 0, 0)
            TP_OPEN_INFO.Controls.Add(Me.CH_DOWN_OPEN_INFO_SUSPEND, 1, 0)
            TP_OPEN_INFO.Dock = System.Windows.Forms.DockStyle.Fill
            TP_OPEN_INFO.Location = New System.Drawing.Point(1, 105)
            TP_OPEN_INFO.Margin = New System.Windows.Forms.Padding(0)
            TP_OPEN_INFO.Name = "TP_OPEN_INFO"
            TP_OPEN_INFO.RowCount = 1
            TP_OPEN_INFO.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_OPEN_INFO.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
            TP_OPEN_INFO.Size = New System.Drawing.Size(574, 25)
            TP_OPEN_INFO.TabIndex = 4
            '
            'CH_DOWN_OPEN_INFO
            '
            Me.CH_DOWN_OPEN_INFO.AutoSize = True
            Me.CH_DOWN_OPEN_INFO.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DOWN_OPEN_INFO.Location = New System.Drawing.Point(4, 4)
            Me.CH_DOWN_OPEN_INFO.Name = "CH_DOWN_OPEN_INFO"
            Me.CH_DOWN_OPEN_INFO.Size = New System.Drawing.Size(279, 17)
            Me.CH_DOWN_OPEN_INFO.TabIndex = 0
            Me.CH_DOWN_OPEN_INFO.Text = "Open the 'Info' form when the download starts"
            Me.CH_DOWN_OPEN_INFO.UseVisualStyleBackColor = True
            '
            'CH_RECYCLE_DEL
            '
            Me.CH_RECYCLE_DEL.AutoSize = True
            Me.CH_RECYCLE_DEL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_RECYCLE_DEL.Location = New System.Drawing.Point(4, 82)
            Me.CH_RECYCLE_DEL.Name = "CH_RECYCLE_DEL"
            Me.CH_RECYCLE_DEL.Size = New System.Drawing.Size(568, 19)
            Me.CH_RECYCLE_DEL.TabIndex = 3
            Me.CH_RECYCLE_DEL.Text = "Delete data to recycle bin"
            Me.CH_RECYCLE_DEL.UseVisualStyleBackColor = True
            '
            'TP_OPEN_PROGRESS
            '
            TP_OPEN_PROGRESS.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_OPEN_PROGRESS.ColumnCount = 2
            TP_OPEN_PROGRESS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_OPEN_PROGRESS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_OPEN_PROGRESS.Controls.Add(Me.CH_DOWN_OPEN_PROGRESS, 0, 0)
            TP_OPEN_PROGRESS.Controls.Add(Me.CH_DOWN_OPEN_PROGRESS_SUSPEND, 1, 0)
            TP_OPEN_PROGRESS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_OPEN_PROGRESS.Location = New System.Drawing.Point(1, 131)
            TP_OPEN_PROGRESS.Margin = New System.Windows.Forms.Padding(0)
            TP_OPEN_PROGRESS.Name = "TP_OPEN_PROGRESS"
            TP_OPEN_PROGRESS.RowCount = 1
            TP_OPEN_PROGRESS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_OPEN_PROGRESS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
            TP_OPEN_PROGRESS.Size = New System.Drawing.Size(574, 25)
            TP_OPEN_PROGRESS.TabIndex = 5
            '
            'CH_DOWN_OPEN_PROGRESS
            '
            Me.CH_DOWN_OPEN_PROGRESS.AutoSize = True
            Me.CH_DOWN_OPEN_PROGRESS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DOWN_OPEN_PROGRESS.Location = New System.Drawing.Point(4, 4)
            Me.CH_DOWN_OPEN_PROGRESS.Name = "CH_DOWN_OPEN_PROGRESS"
            Me.CH_DOWN_OPEN_PROGRESS.Size = New System.Drawing.Size(279, 17)
            Me.CH_DOWN_OPEN_PROGRESS.TabIndex = 0
            Me.CH_DOWN_OPEN_PROGRESS.Text = "Open the 'Progress' form when the download starts"
            Me.CH_DOWN_OPEN_PROGRESS.UseVisualStyleBackColor = True
            '
            'TAB_DOWN
            '
            TAB_DOWN.Controls.Add(TP_DOWNLOADING)
            TAB_DOWN.Location = New System.Drawing.Point(4, 22)
            TAB_DOWN.Name = "TAB_DOWN"
            TAB_DOWN.Size = New System.Drawing.Size(576, 486)
            TAB_DOWN.TabIndex = 6
            TAB_DOWN.Text = "Downloading"
            '
            'TP_DOWNLOADING
            '
            TP_DOWNLOADING.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_DOWNLOADING.ColumnCount = 1
            TP_DOWNLOADING.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_DOWNLOADING.Controls.Add(TP_FILE_NAME, 0, 2)
            TP_DOWNLOADING.Controls.Add(TP_FILE_PATTERNS, 0, 3)
            TP_DOWNLOADING.Controls.Add(Me.TXT_SCRIPT, 0, 4)
            TP_DOWNLOADING.Controls.Add(Me.CH_UDESCR_UP, 0, 0)
            TP_DOWNLOADING.Controls.Add(Me.TXT_DOWN_COMPLETE_SCRIPT, 0, 5)
            TP_DOWNLOADING.Controls.Add(TP_MISSING_DATA, 0, 6)
            TP_DOWNLOADING.Controls.Add(Me.CH_DOWN_REPARSE_MISSING, 0, 7)
            TP_DOWNLOADING.Controls.Add(Me.CH_UNAME_UP, 0, 1)
            TP_DOWNLOADING.Dock = System.Windows.Forms.DockStyle.Fill
            TP_DOWNLOADING.Location = New System.Drawing.Point(0, 0)
            TP_DOWNLOADING.Name = "TP_DOWNLOADING"
            TP_DOWNLOADING.RowCount = 9
            TP_DOWNLOADING.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_DOWNLOADING.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_DOWNLOADING.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            TP_DOWNLOADING.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            TP_DOWNLOADING.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_DOWNLOADING.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_DOWNLOADING.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_DOWNLOADING.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_DOWNLOADING.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_DOWNLOADING.Size = New System.Drawing.Size(576, 486)
            TP_DOWNLOADING.TabIndex = 1
            '
            'TXT_SCRIPT
            '
            ActionButton13.BackgroundImage = CType(resources.GetObject("ActionButton13.BackgroundImage"), System.Drawing.Image)
            ActionButton13.Name = "Open"
            ActionButton14.BackgroundImage = CType(resources.GetObject("ActionButton14.BackgroundImage"), System.Drawing.Image)
            ActionButton14.Name = "Clear"
            Me.TXT_SCRIPT.Buttons.Add(ActionButton13)
            Me.TXT_SCRIPT.Buttons.Add(ActionButton14)
            Me.TXT_SCRIPT.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
            Me.TXT_SCRIPT.CaptionText = "Script"
            Me.TXT_SCRIPT.CaptionToolTipEnabled = True
            Me.TXT_SCRIPT.CaptionToolTipText = "Default script. If the checkbox is checked, newly created users will be created u" &
    "sing the script option."
            Me.TXT_SCRIPT.ChangeControlsEnableOnCheckedChange = False
            Me.TXT_SCRIPT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_SCRIPT.Location = New System.Drawing.Point(4, 118)
            Me.TXT_SCRIPT.Name = "TXT_SCRIPT"
            Me.TXT_SCRIPT.PlaceholderEnabled = True
            Me.TXT_SCRIPT.PlaceholderText = "Enter script path here..."
            Me.TXT_SCRIPT.Size = New System.Drawing.Size(568, 22)
            Me.TXT_SCRIPT.TabIndex = 4
            '
            'TXT_DOWN_COMPLETE_SCRIPT
            '
            Me.TXT_DOWN_COMPLETE_SCRIPT.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
            Me.TXT_DOWN_COMPLETE_SCRIPT.CaptionText = "After download cmd"
            Me.TXT_DOWN_COMPLETE_SCRIPT.CaptionToolTipEnabled = True
            Me.TXT_DOWN_COMPLETE_SCRIPT.CaptionToolTipText = "This command will be executed after all downloads are completed"
            Me.TXT_DOWN_COMPLETE_SCRIPT.CaptionWidth = 120.0R
            Me.TXT_DOWN_COMPLETE_SCRIPT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_DOWN_COMPLETE_SCRIPT.Location = New System.Drawing.Point(4, 147)
            Me.TXT_DOWN_COMPLETE_SCRIPT.Name = "TXT_DOWN_COMPLETE_SCRIPT"
            Me.TXT_DOWN_COMPLETE_SCRIPT.PlaceholderEnabled = True
            Me.TXT_DOWN_COMPLETE_SCRIPT.PlaceholderText = "Enter command here..."
            Me.TXT_DOWN_COMPLETE_SCRIPT.Size = New System.Drawing.Size(568, 22)
            Me.TXT_DOWN_COMPLETE_SCRIPT.TabIndex = 5
            '
            'TP_MISSING_DATA
            '
            TP_MISSING_DATA.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_MISSING_DATA.ColumnCount = 2
            TP_MISSING_DATA.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_MISSING_DATA.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_MISSING_DATA.Controls.Add(Me.CH_ADD_MISSING_TO_LOG, 0, 0)
            TP_MISSING_DATA.Controls.Add(Me.CH_ADD_MISSING_ERROS_TO_LOG, 1, 0)
            TP_MISSING_DATA.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MISSING_DATA.Location = New System.Drawing.Point(1, 173)
            TP_MISSING_DATA.Margin = New System.Windows.Forms.Padding(0)
            TP_MISSING_DATA.Name = "TP_MISSING_DATA"
            TP_MISSING_DATA.RowCount = 1
            TP_MISSING_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MISSING_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24.0!))
            TP_MISSING_DATA.Size = New System.Drawing.Size(574, 25)
            TP_MISSING_DATA.TabIndex = 6
            '
            'CH_UNAME_UP
            '
            Me.CH_UNAME_UP.AutoSize = True
            Me.CH_UNAME_UP.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_UNAME_UP.Location = New System.Drawing.Point(4, 30)
            Me.CH_UNAME_UP.Name = "CH_UNAME_UP"
            Me.CH_UNAME_UP.Size = New System.Drawing.Size(568, 19)
            Me.CH_UNAME_UP.TabIndex = 7
            Me.CH_UNAME_UP.Text = "Update user site name every time"
            Me.CH_UNAME_UP.UseVisualStyleBackColor = True
            '
            'TAB_FEED
            '
            TAB_FEED.Controls.Add(TP_FEED)
            TAB_FEED.Location = New System.Drawing.Point(4, 22)
            TAB_FEED.Name = "TAB_FEED"
            TAB_FEED.Size = New System.Drawing.Size(576, 486)
            TAB_FEED.TabIndex = 7
            TAB_FEED.Text = "Feed"
            '
            'TP_FEED
            '
            TP_FEED.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_FEED.ColumnCount = 1
            TP_FEED.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_FEED.Controls.Add(TP_FEED_IMG_COUNT, 0, 0)
            TP_FEED.Controls.Add(Me.CH_FEED_ENDLESS, 0, 3)
            TP_FEED.Controls.Add(Me.CH_FEED_ADD_SESSION, 0, 4)
            TP_FEED.Controls.Add(Me.CH_FEED_ADD_DATE, 0, 5)
            TP_FEED.Controls.Add(Me.CH_FEED_STORE_SESSION_DATA, 0, 6)
            TP_FEED.Controls.Add(Me.TXT_FEED_CENTER_IMAGE, 0, 1)
            TP_FEED.Controls.Add(Me.COLORS_FEED, 0, 2)
            TP_FEED.Dock = System.Windows.Forms.DockStyle.Fill
            TP_FEED.Location = New System.Drawing.Point(0, 0)
            TP_FEED.Name = "TP_FEED"
            TP_FEED.RowCount = 8
            TP_FEED.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_FEED.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_FEED.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_FEED.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_FEED.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_FEED.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_FEED.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_FEED.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_FEED.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_FEED.Size = New System.Drawing.Size(576, 486)
            TP_FEED.TabIndex = 0
            '
            'TP_FEED_IMG_COUNT
            '
            TP_FEED_IMG_COUNT.ColumnCount = 2
            TP_FEED_IMG_COUNT.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_FEED_IMG_COUNT.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_FEED_IMG_COUNT.Controls.Add(Me.TXT_FEED_ROWS, 0, 0)
            TP_FEED_IMG_COUNT.Controls.Add(Me.TXT_FEED_COLUMNS, 1, 0)
            TP_FEED_IMG_COUNT.Dock = System.Windows.Forms.DockStyle.Fill
            TP_FEED_IMG_COUNT.Location = New System.Drawing.Point(1, 1)
            TP_FEED_IMG_COUNT.Margin = New System.Windows.Forms.Padding(0)
            TP_FEED_IMG_COUNT.Name = "TP_FEED_IMG_COUNT"
            TP_FEED_IMG_COUNT.RowCount = 1
            TP_FEED_IMG_COUNT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_FEED_IMG_COUNT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_FEED_IMG_COUNT.Size = New System.Drawing.Size(574, 28)
            TP_FEED_IMG_COUNT.TabIndex = 0
            '
            'TXT_FEED_ROWS
            '
            Me.TXT_FEED_ROWS.CaptionText = "Feed rows"
            Me.TXT_FEED_ROWS.CaptionToolTipEnabled = True
            Me.TXT_FEED_ROWS.CaptionToolTipText = "How many lines of images should be shown in the feed form"
            Me.TXT_FEED_ROWS.ControlMode = PersonalUtilities.Forms.Controls.TextBoxExtended.ControlModes.NumericUpDown
            Me.TXT_FEED_ROWS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_FEED_ROWS.Location = New System.Drawing.Point(3, 3)
            Me.TXT_FEED_ROWS.Name = "TXT_FEED_ROWS"
            Me.TXT_FEED_ROWS.NumberMaximum = New Decimal(New Integer() {50, 0, 0, 0})
            Me.TXT_FEED_ROWS.NumberMinimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.TXT_FEED_ROWS.Size = New System.Drawing.Size(281, 22)
            Me.TXT_FEED_ROWS.TabIndex = 0
            Me.TXT_FEED_ROWS.Text = "1"
            Me.TXT_FEED_ROWS.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'TXT_FEED_COLUMNS
            '
            Me.TXT_FEED_COLUMNS.CaptionText = "Feed columns"
            Me.TXT_FEED_COLUMNS.CaptionToolTipEnabled = True
            Me.TXT_FEED_COLUMNS.CaptionToolTipText = "How many columns of images should be shown in the feed form"
            Me.TXT_FEED_COLUMNS.ControlMode = PersonalUtilities.Forms.Controls.TextBoxExtended.ControlModes.NumericUpDown
            Me.TXT_FEED_COLUMNS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_FEED_COLUMNS.Location = New System.Drawing.Point(290, 3)
            Me.TXT_FEED_COLUMNS.Name = "TXT_FEED_COLUMNS"
            Me.TXT_FEED_COLUMNS.NumberMaximum = New Decimal(New Integer() {20, 0, 0, 0})
            Me.TXT_FEED_COLUMNS.NumberMinimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.TXT_FEED_COLUMNS.Size = New System.Drawing.Size(281, 22)
            Me.TXT_FEED_COLUMNS.TabIndex = 1
            Me.TXT_FEED_COLUMNS.Text = "1"
            Me.TXT_FEED_COLUMNS.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'CH_FEED_ENDLESS
            '
            Me.CH_FEED_ENDLESS.AutoSize = True
            Me.CH_FEED_ENDLESS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FEED_ENDLESS.Location = New System.Drawing.Point(4, 88)
            Me.CH_FEED_ENDLESS.Name = "CH_FEED_ENDLESS"
            Me.CH_FEED_ENDLESS.Size = New System.Drawing.Size(568, 19)
            Me.CH_FEED_ENDLESS.TabIndex = 3
            Me.CH_FEED_ENDLESS.Text = "Endless feed"
            Me.CH_FEED_ENDLESS.UseVisualStyleBackColor = True
            '
            'CH_FEED_ADD_SESSION
            '
            Me.CH_FEED_ADD_SESSION.AutoSize = True
            Me.CH_FEED_ADD_SESSION.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FEED_ADD_SESSION.Location = New System.Drawing.Point(4, 114)
            Me.CH_FEED_ADD_SESSION.Name = "CH_FEED_ADD_SESSION"
            Me.CH_FEED_ADD_SESSION.Size = New System.Drawing.Size(568, 19)
            Me.CH_FEED_ADD_SESSION.TabIndex = 4
            Me.CH_FEED_ADD_SESSION.Text = "Add the session number to the post title"
            Me.CH_FEED_ADD_SESSION.UseVisualStyleBackColor = True
            '
            'CH_FEED_ADD_DATE
            '
            Me.CH_FEED_ADD_DATE.AutoSize = True
            Me.CH_FEED_ADD_DATE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FEED_ADD_DATE.Location = New System.Drawing.Point(4, 140)
            Me.CH_FEED_ADD_DATE.Name = "CH_FEED_ADD_DATE"
            Me.CH_FEED_ADD_DATE.Size = New System.Drawing.Size(568, 19)
            Me.CH_FEED_ADD_DATE.TabIndex = 5
            Me.CH_FEED_ADD_DATE.Text = "Add the date to the post title"
            Me.CH_FEED_ADD_DATE.UseVisualStyleBackColor = True
            '
            'TXT_FEED_CENTER_IMAGE
            '
            Me.TXT_FEED_CENTER_IMAGE.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
            Me.TXT_FEED_CENTER_IMAGE.CaptionSizeType = System.Windows.Forms.SizeType.Percent
            Me.TXT_FEED_CENTER_IMAGE.CaptionText = "Center images in grid (number of visible images)"
            Me.TXT_FEED_CENTER_IMAGE.CaptionToolTipEnabled = True
            Me.TXT_FEED_CENTER_IMAGE.CaptionToolTipText = "Don't fit images to the grid, but center them and set the number of visible image" &
    "s." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Only works when the number of columns is 1."
            Me.TXT_FEED_CENTER_IMAGE.CaptionWidth = 50.0R
            Me.TXT_FEED_CENTER_IMAGE.ControlMode = PersonalUtilities.Forms.Controls.TextBoxExtended.ControlModes.NumericUpDown
            Me.TXT_FEED_CENTER_IMAGE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_FEED_CENTER_IMAGE.Location = New System.Drawing.Point(4, 33)
            Me.TXT_FEED_CENTER_IMAGE.Margin = New System.Windows.Forms.Padding(3, 3, 2, 3)
            Me.TXT_FEED_CENTER_IMAGE.Name = "TXT_FEED_CENTER_IMAGE"
            Me.TXT_FEED_CENTER_IMAGE.NumberMaximum = New Decimal(New Integer() {50, 0, 0, 0})
            Me.TXT_FEED_CENTER_IMAGE.NumberMinimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.TXT_FEED_CENTER_IMAGE.Size = New System.Drawing.Size(569, 22)
            Me.TXT_FEED_CENTER_IMAGE.TabIndex = 1
            Me.TXT_FEED_CENTER_IMAGE.Text = "1"
            Me.TXT_FEED_CENTER_IMAGE.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'COLORS_FEED
            '
            Me.COLORS_FEED.ButtonsMargin = New System.Windows.Forms.Padding(1, 2, 1, 2)
            Me.COLORS_FEED.CaptionText = "Feed colors"
            Me.COLORS_FEED.Dock = System.Windows.Forms.DockStyle.Fill
            Me.COLORS_FEED.Location = New System.Drawing.Point(1, 59)
            Me.COLORS_FEED.Margin = New System.Windows.Forms.Padding(0)
            Me.COLORS_FEED.Name = "COLORS_FEED"
            Me.COLORS_FEED.Size = New System.Drawing.Size(574, 25)
            Me.COLORS_FEED.TabIndex = 2
            '
            'TAB_NOTIFY
            '
            TAB_NOTIFY.Controls.Add(TP_NOTIFY_MAIN)
            TAB_NOTIFY.Location = New System.Drawing.Point(4, 22)
            TAB_NOTIFY.Name = "TAB_NOTIFY"
            TAB_NOTIFY.Size = New System.Drawing.Size(576, 486)
            TAB_NOTIFY.TabIndex = 8
            TAB_NOTIFY.Text = "Notifications"
            '
            'TP_NOTIFY_MAIN
            '
            TP_NOTIFY_MAIN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_NOTIFY_MAIN.ColumnCount = 1
            TP_NOTIFY_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_NOTIFY_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_NOTIFY_MAIN.Controls.Add(Me.CH_NOTIFY_SHOW_BASE, 0, 1)
            TP_NOTIFY_MAIN.Controls.Add(Me.CH_NOTIFY_SILENT, 0, 0)
            TP_NOTIFY_MAIN.Controls.Add(Me.CH_NOTIFY_PROFILES, 0, 2)
            TP_NOTIFY_MAIN.Controls.Add(Me.CH_NOTIFY_AUTO_DOWN, 0, 3)
            TP_NOTIFY_MAIN.Controls.Add(Me.CH_NOTIFY_CHANNELS, 0, 4)
            TP_NOTIFY_MAIN.Controls.Add(Me.CH_NOTIFY_SAVED_POSTS, 0, 5)
            TP_NOTIFY_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_NOTIFY_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_NOTIFY_MAIN.Name = "TP_NOTIFY_MAIN"
            TP_NOTIFY_MAIN.RowCount = 7
            TP_NOTIFY_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_NOTIFY_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_NOTIFY_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_NOTIFY_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_NOTIFY_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_NOTIFY_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_NOTIFY_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_NOTIFY_MAIN.Size = New System.Drawing.Size(576, 486)
            TP_NOTIFY_MAIN.TabIndex = 0
            '
            'TAB_MAIN
            '
            Me.TAB_MAIN.Controls.Add(TAB_BASIS)
            Me.TAB_MAIN.Controls.Add(TAB_BEHAVIOR)
            Me.TAB_MAIN.Controls.Add(TAB_NOTIFY)
            Me.TAB_MAIN.Controls.Add(TAB_DEFAULTS)
            Me.TAB_MAIN.Controls.Add(TAB_DOWN)
            Me.TAB_MAIN.Controls.Add(TAB_DEFS_CHANNELS)
            Me.TAB_MAIN.Controls.Add(TAB_FEED)
            Me.TAB_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TAB_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.TAB_MAIN.Name = "TAB_MAIN"
            Me.TAB_MAIN.SelectedIndex = 0
            Me.TAB_MAIN.Size = New System.Drawing.Size(584, 369)
            Me.TAB_MAIN.TabIndex = 1
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            Me.CONTAINER_MAIN.ContentPanel.Controls.Add(Me.TAB_MAIN)
            Me.CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(584, 369)
            Me.CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CONTAINER_MAIN.LeftToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            Me.CONTAINER_MAIN.RightToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Size = New System.Drawing.Size(584, 394)
            Me.CONTAINER_MAIN.TabIndex = 0
            Me.CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'GlobalSettingsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(584, 394)
            Me.Controls.Add(Me.CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Icon = Global.SCrawler.My.Resources.Resources.SettingsIcon_48
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(600, 433)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(600, 433)
            Me.Name = "GlobalSettingsForm"
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Settings"
            TP_BASIS.ResumeLayout(False)
            TP_BASIS.PerformLayout()
            CType(Me.TXT_GLOBAL_PATH, System.ComponentModel.ISupportInitialize).EndInit()
            TP_IMAGES.ResumeLayout(False)
            CType(Me.TXT_IMAGE_LARGE, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_IMAGE_SMALL, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_COLLECTIONS_PATH, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_MAX_JOBS_USERS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_MAX_JOBS_CHANNELS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_IMGUR_CLIENT_ID, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_USER_AGENT, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_USER_LIST_IMAGE, System.ComponentModel.ISupportInitialize).EndInit()
            TP_FILE_NAME.ResumeLayout(False)
            TP_FILE_NAME.PerformLayout()
            TP_FILE_PATTERNS.ResumeLayout(False)
            TP_FILE_PATTERNS.PerformLayout()
            TP_CHANNELS_IMGS.ResumeLayout(False)
            CType(Me.TXT_CHANNELS_ROWS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_CHANNELS_COLUMNS, System.ComponentModel.ISupportInitialize).EndInit()
            TAB_BASIS.ResumeLayout(False)
            TAB_DEFAULTS.ResumeLayout(False)
            TP_DEFS.ResumeLayout(False)
            TP_DEFS.PerformLayout()
            TAB_DEFS_CHANNELS.ResumeLayout(False)
            TP_CHANNELS.ResumeLayout(False)
            TP_CHANNELS.PerformLayout()
            CType(Me.TXT_CHANNEL_USER_POST_LIMIT, System.ComponentModel.ISupportInitialize).EndInit()
            TAB_BEHAVIOR.ResumeLayout(False)
            TP_BEHAVIOR.ResumeLayout(False)
            TP_BEHAVIOR.PerformLayout()
            CType(Me.TXT_FOLDER_CMD, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_CLOSE_SCRIPT, System.ComponentModel.ISupportInitialize).EndInit()
            TP_OPEN_INFO.ResumeLayout(False)
            TP_OPEN_INFO.PerformLayout()
            TP_OPEN_PROGRESS.ResumeLayout(False)
            TP_OPEN_PROGRESS.PerformLayout()
            TAB_DOWN.ResumeLayout(False)
            TP_DOWNLOADING.ResumeLayout(False)
            TP_DOWNLOADING.PerformLayout()
            CType(Me.TXT_SCRIPT, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_DOWN_COMPLETE_SCRIPT, System.ComponentModel.ISupportInitialize).EndInit()
            TP_MISSING_DATA.ResumeLayout(False)
            TP_MISSING_DATA.PerformLayout()
            TAB_FEED.ResumeLayout(False)
            TP_FEED.ResumeLayout(False)
            TP_FEED.PerformLayout()
            TP_FEED_IMG_COUNT.ResumeLayout(False)
            CType(Me.TXT_FEED_ROWS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_FEED_COLUMNS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_FEED_CENTER_IMAGE, System.ComponentModel.ISupportInitialize).EndInit()
            TAB_NOTIFY.ResumeLayout(False)
            TP_NOTIFY_MAIN.ResumeLayout(False)
            TP_NOTIFY_MAIN.PerformLayout()
            Me.TAB_MAIN.ResumeLayout(False)
            Me.CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            Me.CONTAINER_MAIN.ResumeLayout(False)
            Me.CONTAINER_MAIN.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents CONTAINER_MAIN As ToolStripContainer
        Private WithEvents TXT_GLOBAL_PATH As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_COLLECTIONS_PATH As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_IMAGE_LARGE As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_IMAGE_SMALL As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CH_CHECK_VER_START As CheckBox
        Private WithEvents TXT_MAX_JOBS_USERS As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_MAX_JOBS_CHANNELS As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_CHANNEL_USER_POST_LIMIT As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_CHANNELS_ROWS As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_CHANNELS_COLUMNS As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CH_COPY_CHANNEL_USER_IMAGE As CheckBox
        Private WithEvents CH_SEPARATE_VIDEO_FOLDER As CheckBox
        Private WithEvents CH_DOWN_VIDEOS As CheckBox
        Private WithEvents CH_DOWN_IMAGES As CheckBox
        Private WithEvents CH_DEF_TEMP As CheckBox
        Private WithEvents CH_CHANNELS_USERS_TEMP As CheckBox
        Private WithEvents TXT_IMGUR_CLIENT_ID As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents OPT_FILE_NAME_REPLACE As RadioButton
        Private WithEvents OPT_FILE_NAME_ADD_DATE As RadioButton
        Private WithEvents CH_FILE_NAME_CHANGE As CheckBox
        Private WithEvents CH_FILE_DATE As CheckBox
        Private WithEvents CH_FILE_TIME As CheckBox
        Private WithEvents OPT_FILE_DATE_START As RadioButton
        Private WithEvents OPT_FILE_DATE_END As RadioButton
        Private WithEvents CH_EXIT_CONFIRM As CheckBox
        Private WithEvents CH_CLOSE_TO_TRAY As CheckBox
        Private WithEvents TAB_MAIN As TabControl
        Private WithEvents CH_NOTIFY_SHOW_BASE As CheckBox
        Private WithEvents CH_COPY_CHANNEL_USER_IMAGE_ALL As CheckBox
        Private WithEvents CH_UDESCR_UP As CheckBox
        Private WithEvents CH_FAST_LOAD As CheckBox
        Private WithEvents TXT_FOLDER_CMD As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CH_RECYCLE_DEL As CheckBox
        Private WithEvents TXT_SCRIPT As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CH_SHOW_GROUPS As CheckBox
        Private WithEvents CH_USERS_GROUPING As CheckBox
        Private WithEvents CH_DOWN_OPEN_INFO As CheckBox
        Private WithEvents CH_DOWN_OPEN_PROGRESS As CheckBox
        Private WithEvents TXT_CLOSE_SCRIPT As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_DOWN_COMPLETE_SCRIPT As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CH_DOWN_OPEN_INFO_SUSPEND As CheckBox
        Private WithEvents CH_DOWN_OPEN_PROGRESS_SUSPEND As CheckBox
        Private WithEvents CH_DOWN_IMAGES_NATIVE As CheckBox
        Private WithEvents CH_ADD_MISSING_TO_LOG As CheckBox
        Private WithEvents CH_ADD_MISSING_ERROS_TO_LOG As CheckBox
        Private WithEvents TXT_FEED_ROWS As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_FEED_COLUMNS As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CH_FEED_ENDLESS As CheckBox
        Private WithEvents CH_FEED_ADD_SESSION As CheckBox
        Private WithEvents CH_FEED_ADD_DATE As CheckBox
        Private WithEvents CH_FEED_STORE_SESSION_DATA As CheckBox
        Private WithEvents CH_NOTIFY_SILENT As CheckBox
        Private WithEvents CH_NOTIFY_SAVED_POSTS As CheckBox
        Private WithEvents CH_NOTIFY_PROFILES As CheckBox
        Private WithEvents CH_NOTIFY_AUTO_DOWN As CheckBox
        Private WithEvents CH_NOTIFY_CHANNELS As CheckBox
        Private WithEvents CH_DOWN_REPARSE_MISSING As CheckBox
        Private WithEvents TXT_USER_AGENT As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_USER_LIST_IMAGE As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_FEED_CENTER_IMAGE As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CH_NAME_SITE_FRIENDLY As CheckBox
        Private WithEvents CH_UNAME_UP As CheckBox
        Private WithEvents COLORS_USERLIST As ColorPicker
        Private WithEvents COLORS_FEED As ColorPicker
    End Class
End Namespace