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
            Dim TAB_DEFS_REDDIT As System.Windows.Forms.TabPage
            Dim TAB_DEFS_TWITTER As System.Windows.Forms.TabPage
            Me.TXT_GLOBAL_PATH = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_IMAGE_LARGE = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_IMAGE_SMALL = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_COLLECTIONS_PATH = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_MAX_JOBS_USERS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_MAX_JOBS_CHANNELS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_CHECK_VER_START = New System.Windows.Forms.CheckBox()
            Me.TXT_IMGUR_CLIENT_ID = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.OPT_FILE_NAME_REPLACE = New System.Windows.Forms.RadioButton()
            Me.OPT_FILE_NAME_ADD_DATE = New System.Windows.Forms.RadioButton()
            Me.CH_FILE_NAME_CHANGE = New System.Windows.Forms.CheckBox()
            Me.CH_FILE_DATE = New System.Windows.Forms.CheckBox()
            Me.CH_FILE_TIME = New System.Windows.Forms.CheckBox()
            Me.OPT_FILE_DATE_START = New System.Windows.Forms.RadioButton()
            Me.OPT_FILE_DATE_END = New System.Windows.Forms.RadioButton()
            Me.CH_EXIT_CONFIRM = New System.Windows.Forms.CheckBox()
            Me.CH_CLOSE_TO_TRAY = New System.Windows.Forms.CheckBox()
            Me.CH_SHOW_NOTIFY = New System.Windows.Forms.CheckBox()
            Me.CH_COPY_CHANNEL_USER_IMAGE = New System.Windows.Forms.CheckBox()
            Me.CH_DEF_TEMP = New System.Windows.Forms.CheckBox()
            Me.CH_DOWN_IMAGES = New System.Windows.Forms.CheckBox()
            Me.CH_DOWN_VIDEOS = New System.Windows.Forms.CheckBox()
            Me.CH_SEPARATE_VIDEO_FOLDER = New System.Windows.Forms.CheckBox()
            Me.CH_CHANNELS_USERS_TEMP = New System.Windows.Forms.CheckBox()
            Me.TXT_CHANNELS_ROWS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_CHANNELS_COLUMNS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_CHANNEL_USER_POST_LIMIT = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.DEFS_REDDIT = New SCrawler.Editors.SiteDefaults()
            Me.TXT_REDDIT_SAVED_POSTS_USER = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.DEFS_TWITTER = New SCrawler.Editors.SiteDefaults()
            Me.CH_TWITTER_USER_MEDIA = New System.Windows.Forms.CheckBox()
            Me.TXT_REQ_WAIT_TIMER = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_REQ_COUNT = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_LIMIT_TIMER = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TAB_MAIN = New System.Windows.Forms.TabControl()
            Me.TAB_DEFS_INSTAGRAM = New System.Windows.Forms.TabPage()
            Me.DEFS_INST = New SCrawler.Editors.SiteDefaults()
            Me.TXT_INST_SAVED_POSTS_USER = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TAB_DEFS_REDGIFS = New System.Windows.Forms.TabPage()
            Me.DEFS_REDGIFS = New SCrawler.Editors.SiteDefaults()
            Me.CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            Me.CH_REDDIT_USER_MEDIA = New System.Windows.Forms.CheckBox()
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
            TAB_DEFS_REDDIT = New System.Windows.Forms.TabPage()
            TAB_DEFS_TWITTER = New System.Windows.Forms.TabPage()
            TP_BASIS.SuspendLayout()
            CType(Me.TXT_GLOBAL_PATH, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_IMAGES.SuspendLayout()
            CType(Me.TXT_IMAGE_LARGE, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_IMAGE_SMALL, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_COLLECTIONS_PATH, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_MAX_JOBS_USERS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_MAX_JOBS_CHANNELS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_IMGUR_CLIENT_ID, System.ComponentModel.ISupportInitialize).BeginInit()
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
            TAB_DEFS_REDDIT.SuspendLayout()
            Me.DEFS_REDDIT.SuspendLayout()
            CType(Me.TXT_REDDIT_SAVED_POSTS_USER, System.ComponentModel.ISupportInitialize).BeginInit()
            TAB_DEFS_TWITTER.SuspendLayout()
            Me.DEFS_TWITTER.SuspendLayout()
            CType(Me.TXT_REQ_WAIT_TIMER, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_REQ_COUNT, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_LIMIT_TIMER, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TAB_MAIN.SuspendLayout()
            Me.TAB_DEFS_INSTAGRAM.SuspendLayout()
            Me.DEFS_INST.SuspendLayout()
            CType(Me.TXT_INST_SAVED_POSTS_USER, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TAB_DEFS_REDGIFS.SuspendLayout()
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
            TP_BASIS.Controls.Add(Me.TXT_IMGUR_CLIENT_ID, 0, 6)
            TP_BASIS.Controls.Add(TP_FILE_NAME, 0, 7)
            TP_BASIS.Controls.Add(TP_FILE_PATTERNS, 0, 8)
            TP_BASIS.Controls.Add(Me.CH_EXIT_CONFIRM, 0, 9)
            TP_BASIS.Controls.Add(Me.CH_CLOSE_TO_TRAY, 0, 10)
            TP_BASIS.Controls.Add(Me.CH_SHOW_NOTIFY, 0, 11)
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
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_BASIS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_BASIS.Size = New System.Drawing.Size(570, 366)
            TP_BASIS.TabIndex = 0
            '
            'TXT_GLOBAL_PATH
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Index = 0
            ActionButton1.Name = "BTT_OPEN"
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Index = 1
            ActionButton2.Name = "BTT_CLEAR"
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
            Me.TXT_IMAGE_LARGE.NumberMaximum = New Decimal(New Integer() {1000, 0, 0, 0})
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
            Me.TXT_IMAGE_SMALL.NumberMaximum = New Decimal(New Integer() {500, 0, 0, 0})
            Me.TXT_IMAGE_SMALL.NumberMinimum = New Decimal(New Integer() {10, 0, 0, 0})
            Me.TXT_IMAGE_SMALL.Size = New System.Drawing.Size(278, 22)
            Me.TXT_IMAGE_SMALL.TabIndex = 1
            Me.TXT_IMAGE_SMALL.Text = "10"
            Me.TXT_IMAGE_SMALL.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'TXT_COLLECTIONS_PATH
            '
            ActionButton3.BackgroundImage = CType(resources.GetObject("ActionButton3.BackgroundImage"), System.Drawing.Image)
            ActionButton3.Index = 0
            ActionButton3.Name = "BTT_CLEAR"
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
            ActionButton4.Index = 0
            ActionButton4.Name = "BTT_REFRESH"
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
            ActionButton5.Index = 0
            ActionButton5.Name = "BTT_REFRESH"
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
            ActionButton6.Index = 0
            ActionButton6.Name = "BTT_CLEAR"
            Me.TXT_IMGUR_CLIENT_ID.Buttons.Add(ActionButton6)
            Me.TXT_IMGUR_CLIENT_ID.CaptionText = "Imgur Client ID"
            Me.TXT_IMGUR_CLIENT_ID.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_IMGUR_CLIENT_ID.Location = New System.Drawing.Point(4, 175)
            Me.TXT_IMGUR_CLIENT_ID.Name = "TXT_IMGUR_CLIENT_ID"
            Me.TXT_IMGUR_CLIENT_ID.Size = New System.Drawing.Size(562, 22)
            Me.TXT_IMGUR_CLIENT_ID.TabIndex = 6
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
            TP_FILE_NAME.Location = New System.Drawing.Point(1, 201)
            TP_FILE_NAME.Margin = New System.Windows.Forms.Padding(0)
            TP_FILE_NAME.Name = "TP_FILE_NAME"
            TP_FILE_NAME.RowCount = 1
            TP_FILE_NAME.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_FILE_NAME.Size = New System.Drawing.Size(568, 30)
            TP_FILE_NAME.TabIndex = 7
            '
            'OPT_FILE_NAME_REPLACE
            '
            Me.OPT_FILE_NAME_REPLACE.AutoSize = True
            Me.OPT_FILE_NAME_REPLACE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_FILE_NAME_REPLACE.Location = New System.Drawing.Point(193, 4)
            Me.OPT_FILE_NAME_REPLACE.Name = "OPT_FILE_NAME_REPLACE"
            Me.OPT_FILE_NAME_REPLACE.Size = New System.Drawing.Size(182, 22)
            Me.OPT_FILE_NAME_REPLACE.TabIndex = 1
            Me.OPT_FILE_NAME_REPLACE.TabStop = True
            Me.OPT_FILE_NAME_REPLACE.Text = "Replace file name by date"
            Me.OPT_FILE_NAME_REPLACE.UseVisualStyleBackColor = True
            '
            'OPT_FILE_NAME_ADD_DATE
            '
            Me.OPT_FILE_NAME_ADD_DATE.AutoSize = True
            Me.OPT_FILE_NAME_ADD_DATE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_FILE_NAME_ADD_DATE.Location = New System.Drawing.Point(382, 4)
            Me.OPT_FILE_NAME_ADD_DATE.Name = "OPT_FILE_NAME_ADD_DATE"
            Me.OPT_FILE_NAME_ADD_DATE.Size = New System.Drawing.Size(182, 22)
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
            Me.CH_FILE_NAME_CHANGE.Size = New System.Drawing.Size(182, 22)
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
            TP_FILE_PATTERNS.Location = New System.Drawing.Point(1, 232)
            TP_FILE_PATTERNS.Margin = New System.Windows.Forms.Padding(0)
            TP_FILE_PATTERNS.Name = "TP_FILE_PATTERNS"
            TP_FILE_PATTERNS.RowCount = 1
            TP_FILE_PATTERNS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_FILE_PATTERNS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29.0!))
            TP_FILE_PATTERNS.Size = New System.Drawing.Size(568, 30)
            TP_FILE_PATTERNS.TabIndex = 8
            '
            'CH_FILE_DATE
            '
            Me.CH_FILE_DATE.AutoSize = True
            Me.CH_FILE_DATE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FILE_DATE.Location = New System.Drawing.Point(4, 4)
            Me.CH_FILE_DATE.Name = "CH_FILE_DATE"
            Me.CH_FILE_DATE.Size = New System.Drawing.Size(106, 22)
            Me.CH_FILE_DATE.TabIndex = 0
            Me.CH_FILE_DATE.Text = "Date"
            Me.CH_FILE_DATE.UseVisualStyleBackColor = True
            '
            'CH_FILE_TIME
            '
            Me.CH_FILE_TIME.AutoSize = True
            Me.CH_FILE_TIME.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FILE_TIME.Location = New System.Drawing.Point(117, 4)
            Me.CH_FILE_TIME.Name = "CH_FILE_TIME"
            Me.CH_FILE_TIME.Size = New System.Drawing.Size(106, 22)
            Me.CH_FILE_TIME.TabIndex = 1
            Me.CH_FILE_TIME.Text = "Time"
            Me.CH_FILE_TIME.UseVisualStyleBackColor = True
            '
            'LBL_DATE_POS
            '
            LBL_DATE_POS.AutoSize = True
            LBL_DATE_POS.Dock = System.Windows.Forms.DockStyle.Fill
            LBL_DATE_POS.Location = New System.Drawing.Point(230, 1)
            LBL_DATE_POS.Name = "LBL_DATE_POS"
            LBL_DATE_POS.Size = New System.Drawing.Size(106, 28)
            LBL_DATE_POS.TabIndex = 2
            LBL_DATE_POS.Text = "Date position:"
            LBL_DATE_POS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'OPT_FILE_DATE_START
            '
            Me.OPT_FILE_DATE_START.AutoSize = True
            Me.OPT_FILE_DATE_START.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_FILE_DATE_START.Location = New System.Drawing.Point(343, 4)
            Me.OPT_FILE_DATE_START.Name = "OPT_FILE_DATE_START"
            Me.OPT_FILE_DATE_START.Size = New System.Drawing.Size(106, 22)
            Me.OPT_FILE_DATE_START.TabIndex = 3
            Me.OPT_FILE_DATE_START.TabStop = True
            Me.OPT_FILE_DATE_START.Text = "Start"
            Me.OPT_FILE_DATE_START.UseVisualStyleBackColor = True
            '
            'OPT_FILE_DATE_END
            '
            Me.OPT_FILE_DATE_END.AutoSize = True
            Me.OPT_FILE_DATE_END.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_FILE_DATE_END.Location = New System.Drawing.Point(456, 4)
            Me.OPT_FILE_DATE_END.Name = "OPT_FILE_DATE_END"
            Me.OPT_FILE_DATE_END.Size = New System.Drawing.Size(108, 22)
            Me.OPT_FILE_DATE_END.TabIndex = 4
            Me.OPT_FILE_DATE_END.TabStop = True
            Me.OPT_FILE_DATE_END.Text = "End"
            Me.OPT_FILE_DATE_END.UseVisualStyleBackColor = True
            '
            'CH_EXIT_CONFIRM
            '
            Me.CH_EXIT_CONFIRM.AutoSize = True
            Me.CH_EXIT_CONFIRM.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_EXIT_CONFIRM.Location = New System.Drawing.Point(4, 266)
            Me.CH_EXIT_CONFIRM.Name = "CH_EXIT_CONFIRM"
            Me.CH_EXIT_CONFIRM.Size = New System.Drawing.Size(562, 19)
            Me.CH_EXIT_CONFIRM.TabIndex = 9
            Me.CH_EXIT_CONFIRM.Text = "Exit confirm"
            Me.CH_EXIT_CONFIRM.UseVisualStyleBackColor = True
            '
            'CH_CLOSE_TO_TRAY
            '
            Me.CH_CLOSE_TO_TRAY.AutoSize = True
            Me.CH_CLOSE_TO_TRAY.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_CLOSE_TO_TRAY.Location = New System.Drawing.Point(4, 292)
            Me.CH_CLOSE_TO_TRAY.Name = "CH_CLOSE_TO_TRAY"
            Me.CH_CLOSE_TO_TRAY.Size = New System.Drawing.Size(562, 19)
            Me.CH_CLOSE_TO_TRAY.TabIndex = 10
            Me.CH_CLOSE_TO_TRAY.Text = "Close to tray"
            Me.CH_CLOSE_TO_TRAY.UseVisualStyleBackColor = True
            '
            'CH_SHOW_NOTIFY
            '
            Me.CH_SHOW_NOTIFY.AutoSize = True
            Me.CH_SHOW_NOTIFY.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_SHOW_NOTIFY.Location = New System.Drawing.Point(4, 318)
            Me.CH_SHOW_NOTIFY.Name = "CH_SHOW_NOTIFY"
            Me.CH_SHOW_NOTIFY.Size = New System.Drawing.Size(562, 19)
            Me.CH_SHOW_NOTIFY.TabIndex = 11
            Me.CH_SHOW_NOTIFY.Text = "Show notifications"
            Me.CH_SHOW_NOTIFY.UseVisualStyleBackColor = True
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
            Me.CH_CHANNELS_USERS_TEMP.Location = New System.Drawing.Point(4, 88)
            Me.CH_CHANNELS_USERS_TEMP.Name = "CH_CHANNELS_USERS_TEMP"
            Me.CH_CHANNELS_USERS_TEMP.Size = New System.Drawing.Size(562, 19)
            Me.CH_CHANNELS_USERS_TEMP.TabIndex = 3
            Me.CH_CHANNELS_USERS_TEMP.Text = "Create temporary users"
            TT_MAIN.SetToolTip(Me.CH_CHANNELS_USERS_TEMP, "Users added from channels will be created with this parameter")
            Me.CH_CHANNELS_USERS_TEMP.UseVisualStyleBackColor = True
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
            TAB_BASIS.Size = New System.Drawing.Size(576, 372)
            TAB_BASIS.TabIndex = 0
            TAB_BASIS.Text = "Basis"
            '
            'TAB_DEFAULTS
            '
            TAB_DEFAULTS.Controls.Add(TP_DEFS)
            TAB_DEFAULTS.Location = New System.Drawing.Point(4, 22)
            TAB_DEFAULTS.Name = "TAB_DEFAULTS"
            TAB_DEFAULTS.Padding = New System.Windows.Forms.Padding(3)
            TAB_DEFAULTS.Size = New System.Drawing.Size(576, 372)
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
            TP_DEFS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_DEFS.Location = New System.Drawing.Point(3, 3)
            TP_DEFS.Name = "TP_DEFS"
            TP_DEFS.RowCount = 5
            TP_DEFS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_DEFS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_DEFS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_DEFS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_DEFS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_DEFS.Size = New System.Drawing.Size(570, 366)
            TP_DEFS.TabIndex = 0
            '
            'TAB_DEFS_CHANNELS
            '
            TAB_DEFS_CHANNELS.Controls.Add(TP_CHANNELS)
            TAB_DEFS_CHANNELS.Location = New System.Drawing.Point(4, 22)
            TAB_DEFS_CHANNELS.Name = "TAB_DEFS_CHANNELS"
            TAB_DEFS_CHANNELS.Padding = New System.Windows.Forms.Padding(3)
            TAB_DEFS_CHANNELS.Size = New System.Drawing.Size(576, 372)
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
            TP_CHANNELS.Controls.Add(Me.CH_CHANNELS_USERS_TEMP, 0, 3)
            TP_CHANNELS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_CHANNELS.Location = New System.Drawing.Point(3, 3)
            TP_CHANNELS.Name = "TP_CHANNELS"
            TP_CHANNELS.RowCount = 5
            TP_CHANNELS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_CHANNELS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_CHANNELS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_CHANNELS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_CHANNELS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_CHANNELS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_CHANNELS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_CHANNELS.Size = New System.Drawing.Size(570, 366)
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
            'TAB_DEFS_REDDIT
            '
            TAB_DEFS_REDDIT.Controls.Add(Me.DEFS_REDDIT)
            TAB_DEFS_REDDIT.Location = New System.Drawing.Point(4, 22)
            TAB_DEFS_REDDIT.Name = "TAB_DEFS_REDDIT"
            TAB_DEFS_REDDIT.Padding = New System.Windows.Forms.Padding(3)
            TAB_DEFS_REDDIT.Size = New System.Drawing.Size(576, 372)
            TAB_DEFS_REDDIT.TabIndex = 2
            TAB_DEFS_REDDIT.Text = "Reddit"
            '
            'DEFS_REDDIT
            '
            Me.DEFS_REDDIT.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            Me.DEFS_REDDIT.ColumnCount = 1
            Me.DEFS_REDDIT.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.DEFS_REDDIT.Controls.Add(Me.TXT_REDDIT_SAVED_POSTS_USER, 0, 4)
            Me.DEFS_REDDIT.Controls.Add(Me.CH_REDDIT_USER_MEDIA, 0, 3)
            Me.DEFS_REDDIT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.DEFS_REDDIT.Location = New System.Drawing.Point(3, 3)
            Me.DEFS_REDDIT.Name = "DEFS_REDDIT"
            Me.DEFS_REDDIT.RowCount = 6
            Me.DEFS_REDDIT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEFS_REDDIT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEFS_REDDIT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEFS_REDDIT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEFS_REDDIT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.DEFS_REDDIT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.DEFS_REDDIT.Size = New System.Drawing.Size(570, 366)
            Me.DEFS_REDDIT.TabIndex = 1
            '
            'TXT_REDDIT_SAVED_POSTS_USER
            '
            Me.TXT_REDDIT_SAVED_POSTS_USER.CaptionText = "Saved posts user"
            Me.TXT_REDDIT_SAVED_POSTS_USER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_REDDIT_SAVED_POSTS_USER.Location = New System.Drawing.Point(4, 108)
            Me.TXT_REDDIT_SAVED_POSTS_USER.Name = "TXT_REDDIT_SAVED_POSTS_USER"
            Me.TXT_REDDIT_SAVED_POSTS_USER.Size = New System.Drawing.Size(562, 22)
            Me.TXT_REDDIT_SAVED_POSTS_USER.TabIndex = 4
            '
            'TAB_DEFS_TWITTER
            '
            TAB_DEFS_TWITTER.Controls.Add(Me.DEFS_TWITTER)
            TAB_DEFS_TWITTER.Location = New System.Drawing.Point(4, 22)
            TAB_DEFS_TWITTER.Name = "TAB_DEFS_TWITTER"
            TAB_DEFS_TWITTER.Padding = New System.Windows.Forms.Padding(3)
            TAB_DEFS_TWITTER.Size = New System.Drawing.Size(576, 372)
            TAB_DEFS_TWITTER.TabIndex = 3
            TAB_DEFS_TWITTER.Text = "Twitter"
            '
            'DEFS_TWITTER
            '
            Me.DEFS_TWITTER.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            Me.DEFS_TWITTER.ColumnCount = 1
            Me.DEFS_TWITTER.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.DEFS_TWITTER.Controls.Add(Me.CH_TWITTER_USER_MEDIA, 0, 3)
            Me.DEFS_TWITTER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.DEFS_TWITTER.Location = New System.Drawing.Point(3, 3)
            Me.DEFS_TWITTER.Name = "DEFS_TWITTER"
            Me.DEFS_TWITTER.RowCount = 5
            Me.DEFS_TWITTER.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEFS_TWITTER.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEFS_TWITTER.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEFS_TWITTER.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEFS_TWITTER.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.DEFS_TWITTER.Size = New System.Drawing.Size(570, 366)
            Me.DEFS_TWITTER.TabIndex = 1
            '
            'CH_TWITTER_USER_MEDIA
            '
            Me.CH_TWITTER_USER_MEDIA.AutoSize = True
            Me.CH_TWITTER_USER_MEDIA.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_TWITTER_USER_MEDIA.Location = New System.Drawing.Point(4, 82)
            Me.CH_TWITTER_USER_MEDIA.Name = "CH_TWITTER_USER_MEDIA"
            Me.CH_TWITTER_USER_MEDIA.Size = New System.Drawing.Size(562, 19)
            Me.CH_TWITTER_USER_MEDIA.TabIndex = 3
            Me.CH_TWITTER_USER_MEDIA.Text = "Get user media only"
            Me.CH_TWITTER_USER_MEDIA.UseVisualStyleBackColor = True
            '
            'TXT_REQ_WAIT_TIMER
            '
            Me.TXT_REQ_WAIT_TIMER.CaptionText = "Request timer"
            Me.TXT_REQ_WAIT_TIMER.CaptionWidth = 120.0R
            Me.TXT_REQ_WAIT_TIMER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_REQ_WAIT_TIMER.Location = New System.Drawing.Point(4, 82)
            Me.TXT_REQ_WAIT_TIMER.Name = "TXT_REQ_WAIT_TIMER"
            Me.TXT_REQ_WAIT_TIMER.Size = New System.Drawing.Size(568, 22)
            Me.TXT_REQ_WAIT_TIMER.TabIndex = 3
            '
            'TXT_REQ_COUNT
            '
            Me.TXT_REQ_COUNT.CaptionText = "Request timer counter"
            Me.TXT_REQ_COUNT.CaptionWidth = 120.0R
            Me.TXT_REQ_COUNT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_REQ_COUNT.Location = New System.Drawing.Point(4, 111)
            Me.TXT_REQ_COUNT.Name = "TXT_REQ_COUNT"
            Me.TXT_REQ_COUNT.Size = New System.Drawing.Size(568, 22)
            Me.TXT_REQ_COUNT.TabIndex = 4
            '
            'TXT_LIMIT_TIMER
            '
            Me.TXT_LIMIT_TIMER.CaptionText = "Posts limit timer"
            Me.TXT_LIMIT_TIMER.CaptionWidth = 120.0R
            Me.TXT_LIMIT_TIMER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_LIMIT_TIMER.Location = New System.Drawing.Point(4, 140)
            Me.TXT_LIMIT_TIMER.Name = "TXT_LIMIT_TIMER"
            Me.TXT_LIMIT_TIMER.Size = New System.Drawing.Size(568, 22)
            Me.TXT_LIMIT_TIMER.TabIndex = 5
            '
            'TAB_MAIN
            '
            Me.TAB_MAIN.Controls.Add(TAB_BASIS)
            Me.TAB_MAIN.Controls.Add(TAB_DEFAULTS)
            Me.TAB_MAIN.Controls.Add(TAB_DEFS_CHANNELS)
            Me.TAB_MAIN.Controls.Add(TAB_DEFS_REDDIT)
            Me.TAB_MAIN.Controls.Add(TAB_DEFS_TWITTER)
            Me.TAB_MAIN.Controls.Add(Me.TAB_DEFS_INSTAGRAM)
            Me.TAB_MAIN.Controls.Add(Me.TAB_DEFS_REDGIFS)
            Me.TAB_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TAB_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.TAB_MAIN.Name = "TAB_MAIN"
            Me.TAB_MAIN.SelectedIndex = 0
            Me.TAB_MAIN.Size = New System.Drawing.Size(584, 398)
            Me.TAB_MAIN.TabIndex = 1
            '
            'TAB_DEFS_INSTAGRAM
            '
            Me.TAB_DEFS_INSTAGRAM.BackColor = System.Drawing.SystemColors.Control
            Me.TAB_DEFS_INSTAGRAM.Controls.Add(Me.DEFS_INST)
            Me.TAB_DEFS_INSTAGRAM.Location = New System.Drawing.Point(4, 22)
            Me.TAB_DEFS_INSTAGRAM.Name = "TAB_DEFS_INSTAGRAM"
            Me.TAB_DEFS_INSTAGRAM.Size = New System.Drawing.Size(576, 372)
            Me.TAB_DEFS_INSTAGRAM.TabIndex = 5
            Me.TAB_DEFS_INSTAGRAM.Text = "Instagram"
            '
            'DEFS_INST
            '
            Me.DEFS_INST.BaseControlsPadding = New System.Windows.Forms.Padding(120, 0, 0, 0)
            Me.DEFS_INST.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            Me.DEFS_INST.ColumnCount = 1
            Me.DEFS_INST.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.DEFS_INST.Controls.Add(Me.TXT_LIMIT_TIMER, 0, 5)
            Me.DEFS_INST.Controls.Add(Me.TXT_REQ_COUNT, 0, 4)
            Me.DEFS_INST.Controls.Add(Me.TXT_REQ_WAIT_TIMER, 0, 3)
            Me.DEFS_INST.Controls.Add(Me.TXT_INST_SAVED_POSTS_USER, 0, 6)
            Me.DEFS_INST.Dock = System.Windows.Forms.DockStyle.Fill
            Me.DEFS_INST.Location = New System.Drawing.Point(0, 0)
            Me.DEFS_INST.Name = "DEFS_INST"
            Me.DEFS_INST.RowCount = 8
            Me.DEFS_INST.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEFS_INST.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEFS_INST.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEFS_INST.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.DEFS_INST.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.DEFS_INST.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.DEFS_INST.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.DEFS_INST.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.DEFS_INST.Size = New System.Drawing.Size(576, 372)
            Me.DEFS_INST.TabIndex = 1
            '
            'TXT_INST_SAVED_POSTS_USER
            '
            Me.TXT_INST_SAVED_POSTS_USER.CaptionText = "Saved posts user"
            Me.TXT_INST_SAVED_POSTS_USER.CaptionWidth = 120.0R
            Me.TXT_INST_SAVED_POSTS_USER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_INST_SAVED_POSTS_USER.Location = New System.Drawing.Point(4, 169)
            Me.TXT_INST_SAVED_POSTS_USER.Name = "TXT_INST_SAVED_POSTS_USER"
            Me.TXT_INST_SAVED_POSTS_USER.Size = New System.Drawing.Size(568, 22)
            Me.TXT_INST_SAVED_POSTS_USER.TabIndex = 9
            '
            'TAB_DEFS_REDGIFS
            '
            Me.TAB_DEFS_REDGIFS.BackColor = System.Drawing.SystemColors.Control
            Me.TAB_DEFS_REDGIFS.Controls.Add(Me.DEFS_REDGIFS)
            Me.TAB_DEFS_REDGIFS.Location = New System.Drawing.Point(4, 22)
            Me.TAB_DEFS_REDGIFS.Name = "TAB_DEFS_REDGIFS"
            Me.TAB_DEFS_REDGIFS.Size = New System.Drawing.Size(576, 372)
            Me.TAB_DEFS_REDGIFS.TabIndex = 6
            Me.TAB_DEFS_REDGIFS.Text = "RedGifs"
            '
            'DEFS_REDGIFS
            '
            Me.DEFS_REDGIFS.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            Me.DEFS_REDGIFS.ColumnCount = 1
            Me.DEFS_REDGIFS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.DEFS_REDGIFS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.DEFS_REDGIFS.Location = New System.Drawing.Point(0, 0)
            Me.DEFS_REDGIFS.Name = "DEFS_REDGIFS"
            Me.DEFS_REDGIFS.RowCount = 4
            Me.DEFS_REDGIFS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEFS_REDGIFS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEFS_REDGIFS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEFS_REDGIFS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.DEFS_REDGIFS.Size = New System.Drawing.Size(576, 372)
            Me.DEFS_REDGIFS.TabIndex = 0
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            Me.CONTAINER_MAIN.ContentPanel.Controls.Add(Me.TAB_MAIN)
            Me.CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(584, 398)
            Me.CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CONTAINER_MAIN.LeftToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            Me.CONTAINER_MAIN.RightToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Size = New System.Drawing.Size(584, 398)
            Me.CONTAINER_MAIN.TabIndex = 0
            Me.CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'CH_REDDIT_USER_MEDIA
            '
            Me.CH_REDDIT_USER_MEDIA.AutoSize = True
            Me.CH_REDDIT_USER_MEDIA.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_REDDIT_USER_MEDIA.Location = New System.Drawing.Point(4, 82)
            Me.CH_REDDIT_USER_MEDIA.Name = "CH_REDDIT_USER_MEDIA"
            Me.CH_REDDIT_USER_MEDIA.Size = New System.Drawing.Size(562, 19)
            Me.CH_REDDIT_USER_MEDIA.TabIndex = 3
            Me.CH_REDDIT_USER_MEDIA.Text = "Get user media only"
            Me.CH_REDDIT_USER_MEDIA.UseVisualStyleBackColor = True
            '
            'GlobalSettingsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(584, 398)
            Me.Controls.Add(Me.CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(600, 437)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(600, 437)
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
            TAB_DEFS_REDDIT.ResumeLayout(False)
            Me.DEFS_REDDIT.ResumeLayout(False)
            Me.DEFS_REDDIT.PerformLayout()
            CType(Me.TXT_REDDIT_SAVED_POSTS_USER, System.ComponentModel.ISupportInitialize).EndInit()
            TAB_DEFS_TWITTER.ResumeLayout(False)
            Me.DEFS_TWITTER.ResumeLayout(False)
            Me.DEFS_TWITTER.PerformLayout()
            CType(Me.TXT_REQ_WAIT_TIMER, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_REQ_COUNT, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_LIMIT_TIMER, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TAB_MAIN.ResumeLayout(False)
            Me.TAB_DEFS_INSTAGRAM.ResumeLayout(False)
            Me.DEFS_INST.ResumeLayout(False)
            CType(Me.TXT_INST_SAVED_POSTS_USER, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TAB_DEFS_REDGIFS.ResumeLayout(False)
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
        Private WithEvents CH_TWITTER_USER_MEDIA As CheckBox
        Private WithEvents CH_CHANNELS_USERS_TEMP As CheckBox
        Private WithEvents TAB_DEFS_INSTAGRAM As TabPage
        Private WithEvents TXT_IMGUR_CLIENT_ID As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_REDDIT_SAVED_POSTS_USER As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents OPT_FILE_NAME_REPLACE As RadioButton
        Private WithEvents OPT_FILE_NAME_ADD_DATE As RadioButton
        Private WithEvents CH_FILE_NAME_CHANGE As CheckBox
        Private WithEvents CH_FILE_DATE As CheckBox
        Private WithEvents CH_FILE_TIME As CheckBox
        Private WithEvents OPT_FILE_DATE_START As RadioButton
        Private WithEvents OPT_FILE_DATE_END As RadioButton
        Private WithEvents CH_EXIT_CONFIRM As CheckBox
        Private WithEvents CH_CLOSE_TO_TRAY As CheckBox
        Private WithEvents TXT_REQ_WAIT_TIMER As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_REQ_COUNT As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_LIMIT_TIMER As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TAB_DEFS_REDGIFS As TabPage
        Private WithEvents TAB_MAIN As TabControl
        Private WithEvents DEFS_TWITTER As SiteDefaults
        Private WithEvents DEFS_REDDIT As SiteDefaults
        Private WithEvents DEFS_INST As SiteDefaults
        Private WithEvents DEFS_REDGIFS As SiteDefaults
        Private WithEvents TXT_INST_SAVED_POSTS_USER As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CH_SHOW_NOTIFY As CheckBox
        Private WithEvents CH_REDDIT_USER_MEDIA As CheckBox
    End Class
End Namespace