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
            Dim TP_MAIN As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GlobalSettingsForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim TP_IMAGES As System.Windows.Forms.TableLayoutPanel
            Dim TP_CHANNELS_IMGS As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton4 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton5 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim TP_DEFAULTS As System.Windows.Forms.TableLayoutPanel
            Dim TP_FILE_NAME As System.Windows.Forms.TableLayoutPanel
            Dim TP_FILE_PATTERNS As System.Windows.Forms.TableLayoutPanel
            Dim LBL_DATE_POS As System.Windows.Forms.Label
            Dim TT_MAIN As System.Windows.Forms.ToolTip
            Me.TXT_GLOBAL_PATH = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_SEPARATE_VIDEO_FOLDER = New System.Windows.Forms.CheckBox()
            Me.TXT_COLLECTIONS_PATH = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_IMAGE_LARGE = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_IMAGE_SMALL = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_CHANNELS_ROWS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_CHANNELS_COLUMNS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_CHANNEL_USER_POST_LIMIT = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_COPY_CHANNEL_USER_IMAGE = New System.Windows.Forms.CheckBox()
            Me.CH_CHECK_VER_START = New System.Windows.Forms.CheckBox()
            Me.TXT_MAX_JOBS_USERS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_MAX_JOBS_CHANNELS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_DEF_TEMP = New System.Windows.Forms.CheckBox()
            Me.CH_DOWN_IMAGES = New System.Windows.Forms.CheckBox()
            Me.CH_DOWN_VIDEOS = New System.Windows.Forms.CheckBox()
            Me.OPT_FILE_NAME_REPLACE = New System.Windows.Forms.RadioButton()
            Me.OPT_FILE_NAME_ADD_DATE = New System.Windows.Forms.RadioButton()
            Me.CH_FILE_NAME_CHANGE = New System.Windows.Forms.CheckBox()
            Me.CH_FILE_DATE = New System.Windows.Forms.CheckBox()
            Me.CH_FILE_TIME = New System.Windows.Forms.CheckBox()
            Me.OPT_FILE_DATE_START = New System.Windows.Forms.RadioButton()
            Me.OPT_FILE_DATE_END = New System.Windows.Forms.RadioButton()
            Me.CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            TP_IMAGES = New System.Windows.Forms.TableLayoutPanel()
            TP_CHANNELS_IMGS = New System.Windows.Forms.TableLayoutPanel()
            TP_DEFAULTS = New System.Windows.Forms.TableLayoutPanel()
            TP_FILE_NAME = New System.Windows.Forms.TableLayoutPanel()
            TP_FILE_PATTERNS = New System.Windows.Forms.TableLayoutPanel()
            LBL_DATE_POS = New System.Windows.Forms.Label()
            TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
            TP_MAIN.SuspendLayout()
            CType(Me.TXT_GLOBAL_PATH, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_COLLECTIONS_PATH, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_IMAGES.SuspendLayout()
            CType(Me.TXT_IMAGE_LARGE, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_IMAGE_SMALL, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_CHANNELS_IMGS.SuspendLayout()
            CType(Me.TXT_CHANNELS_ROWS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_CHANNELS_COLUMNS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_CHANNEL_USER_POST_LIMIT, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_MAX_JOBS_USERS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_MAX_JOBS_CHANNELS, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_DEFAULTS.SuspendLayout()
            TP_FILE_NAME.SuspendLayout()
            TP_FILE_PATTERNS.SuspendLayout()
            Me.CONTAINER_MAIN.ContentPanel.SuspendLayout()
            Me.CONTAINER_MAIN.SuspendLayout()
            Me.SuspendLayout()
            '
            'TP_MAIN
            '
            TP_MAIN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(Me.TXT_GLOBAL_PATH, 0, 0)
            TP_MAIN.Controls.Add(Me.CH_SEPARATE_VIDEO_FOLDER, 0, 2)
            TP_MAIN.Controls.Add(Me.TXT_COLLECTIONS_PATH, 0, 4)
            TP_MAIN.Controls.Add(TP_IMAGES, 0, 1)
            TP_MAIN.Controls.Add(TP_CHANNELS_IMGS, 0, 5)
            TP_MAIN.Controls.Add(Me.TXT_CHANNEL_USER_POST_LIMIT, 0, 6)
            TP_MAIN.Controls.Add(Me.CH_COPY_CHANNEL_USER_IMAGE, 0, 7)
            TP_MAIN.Controls.Add(Me.CH_CHECK_VER_START, 0, 8)
            TP_MAIN.Controls.Add(Me.TXT_MAX_JOBS_USERS, 0, 9)
            TP_MAIN.Controls.Add(Me.TXT_MAX_JOBS_CHANNELS, 0, 10)
            TP_MAIN.Controls.Add(TP_DEFAULTS, 0, 3)
            TP_MAIN.Controls.Add(TP_FILE_NAME, 0, 11)
            TP_MAIN.Controls.Add(TP_FILE_PATTERNS, 0, 12)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 13
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692121!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692121!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692121!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692121!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692121!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692121!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.693649!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.693649!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692503!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.691732!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.692427!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.691657!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.691657!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_MAIN.Size = New System.Drawing.Size(584, 359)
            TP_MAIN.TabIndex = 0
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
            Me.TXT_GLOBAL_PATH.Size = New System.Drawing.Size(576, 22)
            Me.TXT_GLOBAL_PATH.TabIndex = 0
            '
            'CH_SEPARATE_VIDEO_FOLDER
            '
            Me.CH_SEPARATE_VIDEO_FOLDER.AutoSize = True
            Me.CH_SEPARATE_VIDEO_FOLDER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_SEPARATE_VIDEO_FOLDER.Location = New System.Drawing.Point(4, 58)
            Me.CH_SEPARATE_VIDEO_FOLDER.Name = "CH_SEPARATE_VIDEO_FOLDER"
            Me.CH_SEPARATE_VIDEO_FOLDER.Padding = New System.Windows.Forms.Padding(100, 0, 0, 0)
            Me.CH_SEPARATE_VIDEO_FOLDER.Size = New System.Drawing.Size(576, 20)
            Me.CH_SEPARATE_VIDEO_FOLDER.TabIndex = 2
            Me.CH_SEPARATE_VIDEO_FOLDER.Text = "Separate video folders"
            TT_MAIN.SetToolTip(Me.CH_SEPARATE_VIDEO_FOLDER, resources.GetString("CH_SEPARATE_VIDEO_FOLDER.ToolTip"))
            Me.CH_SEPARATE_VIDEO_FOLDER.UseVisualStyleBackColor = True
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
            Me.TXT_COLLECTIONS_PATH.Location = New System.Drawing.Point(4, 112)
            Me.TXT_COLLECTIONS_PATH.Name = "TXT_COLLECTIONS_PATH"
            Me.TXT_COLLECTIONS_PATH.Size = New System.Drawing.Size(576, 22)
            Me.TXT_COLLECTIONS_PATH.TabIndex = 4
            '
            'TP_IMAGES
            '
            TP_IMAGES.ColumnCount = 2
            TP_IMAGES.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_IMAGES.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_IMAGES.Controls.Add(Me.TXT_IMAGE_LARGE, 0, 0)
            TP_IMAGES.Controls.Add(Me.TXT_IMAGE_SMALL, 1, 0)
            TP_IMAGES.Dock = System.Windows.Forms.DockStyle.Fill
            TP_IMAGES.Location = New System.Drawing.Point(1, 28)
            TP_IMAGES.Margin = New System.Windows.Forms.Padding(0)
            TP_IMAGES.Name = "TP_IMAGES"
            TP_IMAGES.RowCount = 1
            TP_IMAGES.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_IMAGES.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_IMAGES.Size = New System.Drawing.Size(582, 26)
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
            Me.TXT_IMAGE_LARGE.Size = New System.Drawing.Size(285, 22)
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
            Me.TXT_IMAGE_SMALL.Location = New System.Drawing.Point(294, 3)
            Me.TXT_IMAGE_SMALL.Name = "TXT_IMAGE_SMALL"
            Me.TXT_IMAGE_SMALL.NumberMaximum = New Decimal(New Integer() {500, 0, 0, 0})
            Me.TXT_IMAGE_SMALL.NumberMinimum = New Decimal(New Integer() {10, 0, 0, 0})
            Me.TXT_IMAGE_SMALL.Size = New System.Drawing.Size(285, 22)
            Me.TXT_IMAGE_SMALL.TabIndex = 1
            Me.TXT_IMAGE_SMALL.Text = "10"
            Me.TXT_IMAGE_SMALL.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'TP_CHANNELS_IMGS
            '
            TP_CHANNELS_IMGS.ColumnCount = 2
            TP_CHANNELS_IMGS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_CHANNELS_IMGS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_CHANNELS_IMGS.Controls.Add(Me.TXT_CHANNELS_ROWS, 0, 0)
            TP_CHANNELS_IMGS.Controls.Add(Me.TXT_CHANNELS_COLUMNS, 1, 0)
            TP_CHANNELS_IMGS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_CHANNELS_IMGS.Location = New System.Drawing.Point(1, 136)
            TP_CHANNELS_IMGS.Margin = New System.Windows.Forms.Padding(0)
            TP_CHANNELS_IMGS.Name = "TP_CHANNELS_IMGS"
            TP_CHANNELS_IMGS.RowCount = 1
            TP_CHANNELS_IMGS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_CHANNELS_IMGS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_CHANNELS_IMGS.Size = New System.Drawing.Size(582, 26)
            TP_CHANNELS_IMGS.TabIndex = 5
            '
            'TXT_CHANNELS_ROWS
            '
            Me.TXT_CHANNELS_ROWS.CaptionText = "Channels rows"
            Me.TXT_CHANNELS_ROWS.CaptionToolTipEnabled = True
            Me.TXT_CHANNELS_ROWS.CaptionToolTipText = "How many images' rows should be shown in the channels form"
            Me.TXT_CHANNELS_ROWS.ControlMode = PersonalUtilities.Forms.Controls.TextBoxExtended.ControlModes.NumericUpDown
            Me.TXT_CHANNELS_ROWS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_CHANNELS_ROWS.Location = New System.Drawing.Point(3, 3)
            Me.TXT_CHANNELS_ROWS.Name = "TXT_CHANNELS_ROWS"
            Me.TXT_CHANNELS_ROWS.Size = New System.Drawing.Size(285, 22)
            Me.TXT_CHANNELS_ROWS.TabIndex = 0
            Me.TXT_CHANNELS_ROWS.Text = "0"
            Me.TXT_CHANNELS_ROWS.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'TXT_CHANNELS_COLUMNS
            '
            Me.TXT_CHANNELS_COLUMNS.CaptionText = "Channels columns"
            Me.TXT_CHANNELS_COLUMNS.CaptionToolTipEnabled = True
            Me.TXT_CHANNELS_COLUMNS.CaptionToolTipText = "How many images' columns should be shown in the channels form"
            Me.TXT_CHANNELS_COLUMNS.ControlMode = PersonalUtilities.Forms.Controls.TextBoxExtended.ControlModes.NumericUpDown
            Me.TXT_CHANNELS_COLUMNS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_CHANNELS_COLUMNS.Location = New System.Drawing.Point(294, 3)
            Me.TXT_CHANNELS_COLUMNS.Name = "TXT_CHANNELS_COLUMNS"
            Me.TXT_CHANNELS_COLUMNS.Size = New System.Drawing.Size(285, 22)
            Me.TXT_CHANNELS_COLUMNS.TabIndex = 1
            Me.TXT_CHANNELS_COLUMNS.Text = "0"
            Me.TXT_CHANNELS_COLUMNS.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'TXT_CHANNEL_USER_POST_LIMIT
            '
            Me.TXT_CHANNEL_USER_POST_LIMIT.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
            Me.TXT_CHANNEL_USER_POST_LIMIT.CaptionSizeType = System.Windows.Forms.SizeType.Percent
            Me.TXT_CHANNEL_USER_POST_LIMIT.CaptionText = "Download limit for channel user"
            Me.TXT_CHANNEL_USER_POST_LIMIT.CaptionToolTipText = "Set count of downloading posts limit if user added from channel"
            Me.TXT_CHANNEL_USER_POST_LIMIT.CaptionWidth = 50.0R
            Me.TXT_CHANNEL_USER_POST_LIMIT.ControlMode = PersonalUtilities.Forms.Controls.TextBoxExtended.ControlModes.NumericUpDown
            Me.TXT_CHANNEL_USER_POST_LIMIT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_CHANNEL_USER_POST_LIMIT.Location = New System.Drawing.Point(4, 166)
            Me.TXT_CHANNEL_USER_POST_LIMIT.Name = "TXT_CHANNEL_USER_POST_LIMIT"
            Me.TXT_CHANNEL_USER_POST_LIMIT.NumberMaximum = New Decimal(New Integer() {1000, 0, 0, 0})
            Me.TXT_CHANNEL_USER_POST_LIMIT.NumberMinimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.TXT_CHANNEL_USER_POST_LIMIT.Size = New System.Drawing.Size(576, 22)
            Me.TXT_CHANNEL_USER_POST_LIMIT.TabIndex = 6
            Me.TXT_CHANNEL_USER_POST_LIMIT.Text = "1"
            Me.TXT_CHANNEL_USER_POST_LIMIT.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'CH_COPY_CHANNEL_USER_IMAGE
            '
            Me.CH_COPY_CHANNEL_USER_IMAGE.AutoSize = True
            Me.CH_COPY_CHANNEL_USER_IMAGE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_COPY_CHANNEL_USER_IMAGE.Location = New System.Drawing.Point(4, 193)
            Me.CH_COPY_CHANNEL_USER_IMAGE.Name = "CH_COPY_CHANNEL_USER_IMAGE"
            Me.CH_COPY_CHANNEL_USER_IMAGE.Padding = New System.Windows.Forms.Padding(100, 0, 0, 0)
            Me.CH_COPY_CHANNEL_USER_IMAGE.Size = New System.Drawing.Size(576, 20)
            Me.CH_COPY_CHANNEL_USER_IMAGE.TabIndex = 7
            Me.CH_COPY_CHANNEL_USER_IMAGE.Text = "Copy channel user image"
            TT_MAIN.SetToolTip(Me.CH_COPY_CHANNEL_USER_IMAGE, "Copy image posted by user (in the channel you added from) to the user destination" &
        " folder.")
            Me.CH_COPY_CHANNEL_USER_IMAGE.UseVisualStyleBackColor = True
            '
            'CH_CHECK_VER_START
            '
            Me.CH_CHECK_VER_START.AutoSize = True
            Me.CH_CHECK_VER_START.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_CHECK_VER_START.Location = New System.Drawing.Point(4, 220)
            Me.CH_CHECK_VER_START.Name = "CH_CHECK_VER_START"
            Me.CH_CHECK_VER_START.Padding = New System.Windows.Forms.Padding(100, 0, 0, 0)
            Me.CH_CHECK_VER_START.Size = New System.Drawing.Size(576, 20)
            Me.CH_CHECK_VER_START.TabIndex = 8
            Me.CH_CHECK_VER_START.Text = "Check new version at start"
            Me.CH_CHECK_VER_START.UseVisualStyleBackColor = True
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
            Me.TXT_MAX_JOBS_USERS.Location = New System.Drawing.Point(4, 247)
            Me.TXT_MAX_JOBS_USERS.Name = "TXT_MAX_JOBS_USERS"
            Me.TXT_MAX_JOBS_USERS.NumberMinimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.TXT_MAX_JOBS_USERS.Size = New System.Drawing.Size(576, 22)
            Me.TXT_MAX_JOBS_USERS.TabIndex = 9
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
            Me.TXT_MAX_JOBS_CHANNELS.Location = New System.Drawing.Point(4, 274)
            Me.TXT_MAX_JOBS_CHANNELS.Name = "TXT_MAX_JOBS_CHANNELS"
            Me.TXT_MAX_JOBS_CHANNELS.NumberMinimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.TXT_MAX_JOBS_CHANNELS.Size = New System.Drawing.Size(576, 22)
            Me.TXT_MAX_JOBS_CHANNELS.TabIndex = 10
            Me.TXT_MAX_JOBS_CHANNELS.Text = "1"
            Me.TXT_MAX_JOBS_CHANNELS.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
            '
            'TP_DEFAULTS
            '
            TP_DEFAULTS.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_DEFAULTS.ColumnCount = 3
            TP_DEFAULTS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_DEFAULTS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_DEFAULTS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_DEFAULTS.Controls.Add(Me.CH_DEF_TEMP, 0, 0)
            TP_DEFAULTS.Controls.Add(Me.CH_DOWN_IMAGES, 1, 0)
            TP_DEFAULTS.Controls.Add(Me.CH_DOWN_VIDEOS, 2, 0)
            TP_DEFAULTS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_DEFAULTS.Location = New System.Drawing.Point(1, 82)
            TP_DEFAULTS.Margin = New System.Windows.Forms.Padding(0)
            TP_DEFAULTS.Name = "TP_DEFAULTS"
            TP_DEFAULTS.RowCount = 1
            TP_DEFAULTS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_DEFAULTS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27.0!))
            TP_DEFAULTS.Size = New System.Drawing.Size(582, 26)
            TP_DEFAULTS.TabIndex = 3
            '
            'CH_DEF_TEMP
            '
            Me.CH_DEF_TEMP.AutoSize = True
            Me.CH_DEF_TEMP.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DEF_TEMP.Location = New System.Drawing.Point(4, 4)
            Me.CH_DEF_TEMP.Name = "CH_DEF_TEMP"
            Me.CH_DEF_TEMP.Size = New System.Drawing.Size(186, 18)
            Me.CH_DEF_TEMP.TabIndex = 0
            Me.CH_DEF_TEMP.Text = "Temporary default"
            TT_MAIN.SetToolTip(Me.CH_DEF_TEMP, "Default value on user creating (can be changed in the new user form)")
            Me.CH_DEF_TEMP.UseVisualStyleBackColor = True
            '
            'CH_DOWN_IMAGES
            '
            Me.CH_DOWN_IMAGES.AutoSize = True
            Me.CH_DOWN_IMAGES.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DOWN_IMAGES.Location = New System.Drawing.Point(197, 4)
            Me.CH_DOWN_IMAGES.Name = "CH_DOWN_IMAGES"
            Me.CH_DOWN_IMAGES.Size = New System.Drawing.Size(186, 18)
            Me.CH_DOWN_IMAGES.TabIndex = 1
            Me.CH_DOWN_IMAGES.Text = "Download images"
            TT_MAIN.SetToolTip(Me.CH_DOWN_IMAGES, "By default, download images for new users (can be changed in the new user form)")
            Me.CH_DOWN_IMAGES.UseVisualStyleBackColor = True
            '
            'CH_DOWN_VIDEOS
            '
            Me.CH_DOWN_VIDEOS.AutoSize = True
            Me.CH_DOWN_VIDEOS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DOWN_VIDEOS.Location = New System.Drawing.Point(390, 4)
            Me.CH_DOWN_VIDEOS.Name = "CH_DOWN_VIDEOS"
            Me.CH_DOWN_VIDEOS.Size = New System.Drawing.Size(188, 18)
            Me.CH_DOWN_VIDEOS.TabIndex = 2
            Me.CH_DOWN_VIDEOS.Text = "Download videos"
            TT_MAIN.SetToolTip(Me.CH_DOWN_VIDEOS, "By default, download videos for new users (can be changed in the new user form)")
            Me.CH_DOWN_VIDEOS.UseVisualStyleBackColor = True
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
            TP_FILE_NAME.Location = New System.Drawing.Point(1, 298)
            TP_FILE_NAME.Margin = New System.Windows.Forms.Padding(0)
            TP_FILE_NAME.Name = "TP_FILE_NAME"
            TP_FILE_NAME.RowCount = 1
            TP_FILE_NAME.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_FILE_NAME.Size = New System.Drawing.Size(582, 26)
            TP_FILE_NAME.TabIndex = 12
            '
            'OPT_FILE_NAME_REPLACE
            '
            Me.OPT_FILE_NAME_REPLACE.AutoSize = True
            Me.OPT_FILE_NAME_REPLACE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_FILE_NAME_REPLACE.Location = New System.Drawing.Point(197, 4)
            Me.OPT_FILE_NAME_REPLACE.Name = "OPT_FILE_NAME_REPLACE"
            Me.OPT_FILE_NAME_REPLACE.Size = New System.Drawing.Size(186, 18)
            Me.OPT_FILE_NAME_REPLACE.TabIndex = 1
            Me.OPT_FILE_NAME_REPLACE.TabStop = True
            Me.OPT_FILE_NAME_REPLACE.Text = "Replace file name by date"
            Me.OPT_FILE_NAME_REPLACE.UseVisualStyleBackColor = True
            '
            'OPT_FILE_NAME_ADD_DATE
            '
            Me.OPT_FILE_NAME_ADD_DATE.AutoSize = True
            Me.OPT_FILE_NAME_ADD_DATE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_FILE_NAME_ADD_DATE.Location = New System.Drawing.Point(390, 4)
            Me.OPT_FILE_NAME_ADD_DATE.Name = "OPT_FILE_NAME_ADD_DATE"
            Me.OPT_FILE_NAME_ADD_DATE.Size = New System.Drawing.Size(188, 18)
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
            Me.CH_FILE_NAME_CHANGE.Size = New System.Drawing.Size(186, 18)
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
            TP_FILE_PATTERNS.Location = New System.Drawing.Point(1, 325)
            TP_FILE_PATTERNS.Margin = New System.Windows.Forms.Padding(0)
            TP_FILE_PATTERNS.Name = "TP_FILE_PATTERNS"
            TP_FILE_PATTERNS.RowCount = 1
            TP_FILE_PATTERNS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_FILE_PATTERNS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40.0!))
            TP_FILE_PATTERNS.Size = New System.Drawing.Size(582, 33)
            TP_FILE_PATTERNS.TabIndex = 13
            '
            'CH_FILE_DATE
            '
            Me.CH_FILE_DATE.AutoSize = True
            Me.CH_FILE_DATE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FILE_DATE.Location = New System.Drawing.Point(4, 4)
            Me.CH_FILE_DATE.Name = "CH_FILE_DATE"
            Me.CH_FILE_DATE.Size = New System.Drawing.Size(109, 25)
            Me.CH_FILE_DATE.TabIndex = 0
            Me.CH_FILE_DATE.Text = "Date"
            Me.CH_FILE_DATE.UseVisualStyleBackColor = True
            '
            'CH_FILE_TIME
            '
            Me.CH_FILE_TIME.AutoSize = True
            Me.CH_FILE_TIME.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FILE_TIME.Location = New System.Drawing.Point(120, 4)
            Me.CH_FILE_TIME.Name = "CH_FILE_TIME"
            Me.CH_FILE_TIME.Size = New System.Drawing.Size(109, 25)
            Me.CH_FILE_TIME.TabIndex = 1
            Me.CH_FILE_TIME.Text = "Time"
            Me.CH_FILE_TIME.UseVisualStyleBackColor = True
            '
            'LBL_DATE_POS
            '
            LBL_DATE_POS.AutoSize = True
            LBL_DATE_POS.Dock = System.Windows.Forms.DockStyle.Fill
            LBL_DATE_POS.Location = New System.Drawing.Point(236, 1)
            LBL_DATE_POS.Name = "LBL_DATE_POS"
            LBL_DATE_POS.Size = New System.Drawing.Size(109, 31)
            LBL_DATE_POS.TabIndex = 2
            LBL_DATE_POS.Text = "Date position:"
            LBL_DATE_POS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'OPT_FILE_DATE_START
            '
            Me.OPT_FILE_DATE_START.AutoSize = True
            Me.OPT_FILE_DATE_START.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_FILE_DATE_START.Location = New System.Drawing.Point(352, 4)
            Me.OPT_FILE_DATE_START.Name = "OPT_FILE_DATE_START"
            Me.OPT_FILE_DATE_START.Size = New System.Drawing.Size(109, 25)
            Me.OPT_FILE_DATE_START.TabIndex = 3
            Me.OPT_FILE_DATE_START.TabStop = True
            Me.OPT_FILE_DATE_START.Text = "Start"
            Me.OPT_FILE_DATE_START.UseVisualStyleBackColor = True
            '
            'OPT_FILE_DATE_END
            '
            Me.OPT_FILE_DATE_END.AutoSize = True
            Me.OPT_FILE_DATE_END.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_FILE_DATE_END.Location = New System.Drawing.Point(468, 4)
            Me.OPT_FILE_DATE_END.Name = "OPT_FILE_DATE_END"
            Me.OPT_FILE_DATE_END.Size = New System.Drawing.Size(110, 25)
            Me.OPT_FILE_DATE_END.TabIndex = 4
            Me.OPT_FILE_DATE_END.TabStop = True
            Me.OPT_FILE_DATE_END.Text = "End"
            Me.OPT_FILE_DATE_END.UseVisualStyleBackColor = True
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            Me.CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            Me.CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(584, 359)
            Me.CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CONTAINER_MAIN.LeftToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            Me.CONTAINER_MAIN.RightToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Size = New System.Drawing.Size(584, 384)
            Me.CONTAINER_MAIN.TabIndex = 0
            Me.CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'GlobalSettingsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(584, 384)
            Me.Controls.Add(Me.CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(600, 423)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(600, 423)
            Me.Name = "GlobalSettingsForm"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Settings"
            TP_MAIN.ResumeLayout(False)
            TP_MAIN.PerformLayout()
            CType(Me.TXT_GLOBAL_PATH, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_COLLECTIONS_PATH, System.ComponentModel.ISupportInitialize).EndInit()
            TP_IMAGES.ResumeLayout(False)
            CType(Me.TXT_IMAGE_LARGE, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_IMAGE_SMALL, System.ComponentModel.ISupportInitialize).EndInit()
            TP_CHANNELS_IMGS.ResumeLayout(False)
            CType(Me.TXT_CHANNELS_ROWS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_CHANNELS_COLUMNS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_CHANNEL_USER_POST_LIMIT, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_MAX_JOBS_USERS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_MAX_JOBS_CHANNELS, System.ComponentModel.ISupportInitialize).EndInit()
            TP_DEFAULTS.ResumeLayout(False)
            TP_DEFAULTS.PerformLayout()
            TP_FILE_NAME.ResumeLayout(False)
            TP_FILE_NAME.PerformLayout()
            TP_FILE_PATTERNS.ResumeLayout(False)
            TP_FILE_PATTERNS.PerformLayout()
            Me.CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            Me.CONTAINER_MAIN.ResumeLayout(False)
            Me.CONTAINER_MAIN.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents CONTAINER_MAIN As ToolStripContainer
        Private WithEvents TXT_GLOBAL_PATH As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CH_SEPARATE_VIDEO_FOLDER As CheckBox
        Private WithEvents TXT_COLLECTIONS_PATH As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_IMAGE_LARGE As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_IMAGE_SMALL As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_CHANNELS_ROWS As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_CHANNELS_COLUMNS As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_CHANNEL_USER_POST_LIMIT As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CH_COPY_CHANNEL_USER_IMAGE As CheckBox
        Private WithEvents CH_CHECK_VER_START As CheckBox
        Private WithEvents TXT_MAX_JOBS_USERS As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_MAX_JOBS_CHANNELS As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CH_DEF_TEMP As CheckBox
        Private WithEvents CH_DOWN_IMAGES As CheckBox
        Private WithEvents CH_DOWN_VIDEOS As CheckBox
        Private WithEvents OPT_FILE_NAME_REPLACE As RadioButton
        Private WithEvents OPT_FILE_NAME_ADD_DATE As RadioButton
        Private WithEvents CH_FILE_DATE As CheckBox
        Private WithEvents CH_FILE_TIME As CheckBox
        Private WithEvents OPT_FILE_DATE_START As RadioButton
        Private WithEvents OPT_FILE_DATE_END As RadioButton
        Private WithEvents CH_FILE_NAME_CHANGE As CheckBox
    End Class
End Namespace