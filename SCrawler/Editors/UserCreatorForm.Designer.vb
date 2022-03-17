Namespace Editors
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class UserCreatorForm : Inherits System.Windows.Forms.Form
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
            Dim TP_SITE As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UserCreatorForm))
            Dim ListColumn1 As PersonalUtilities.Forms.Controls.Base.ListColumn = New PersonalUtilities.Forms.Controls.Base.ListColumn()
            Dim ListColumn2 As PersonalUtilities.Forms.Controls.Base.ListColumn = New PersonalUtilities.Forms.Controls.Base.ListColumn()
            Dim TP_PARAMS As System.Windows.Forms.TableLayoutPanel
            Dim TP_OTHER As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton4 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim TP_DOWN_OPTIONS As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton5 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton6 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim TT_MAIN As System.Windows.Forms.ToolTip
            Me.TXT_USER = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_IS_CHANNEL = New System.Windows.Forms.CheckBox()
            Me.CMB_SITE = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.BTT_OTHER_SETTINGS = New System.Windows.Forms.Button()
            Me.CH_TEMP = New System.Windows.Forms.CheckBox()
            Me.CH_FAV = New System.Windows.Forms.CheckBox()
            Me.CH_PARSE_USER_MEDIA = New System.Windows.Forms.CheckBox()
            Me.CH_READY_FOR_DOWN = New System.Windows.Forms.CheckBox()
            Me.TXT_DESCR = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_USER_FRIENDLY = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TP_ADD_BY_LIST = New System.Windows.Forms.TableLayoutPanel()
            Me.CH_ADD_BY_LIST = New System.Windows.Forms.CheckBox()
            Me.CH_AUTO_DETECT_SITE = New System.Windows.Forms.CheckBox()
            Me.TXT_LABELS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_DOWN_IMAGES = New System.Windows.Forms.CheckBox()
            Me.CH_DOWN_VIDEOS = New System.Windows.Forms.CheckBox()
            Me.TXT_SPEC_FOLDER = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            TP_SITE = New System.Windows.Forms.TableLayoutPanel()
            TP_PARAMS = New System.Windows.Forms.TableLayoutPanel()
            TP_OTHER = New System.Windows.Forms.TableLayoutPanel()
            TP_DOWN_OPTIONS = New System.Windows.Forms.TableLayoutPanel()
            TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
            TP_MAIN.SuspendLayout()
            CType(Me.TXT_USER, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_SITE.SuspendLayout()
            CType(Me.CMB_SITE, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_PARAMS.SuspendLayout()
            TP_OTHER.SuspendLayout()
            CType(Me.TXT_DESCR, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_USER_FRIENDLY, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TP_ADD_BY_LIST.SuspendLayout()
            CType(Me.TXT_LABELS, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_DOWN_OPTIONS.SuspendLayout()
            CType(Me.TXT_SPEC_FOLDER, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.CONTAINER_MAIN.ContentPanel.SuspendLayout()
            Me.CONTAINER_MAIN.SuspendLayout()
            Me.SuspendLayout()
            '
            'TP_MAIN
            '
            TP_MAIN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(Me.TXT_USER, 0, 0)
            TP_MAIN.Controls.Add(TP_SITE, 0, 3)
            TP_MAIN.Controls.Add(TP_PARAMS, 0, 4)
            TP_MAIN.Controls.Add(TP_OTHER, 0, 6)
            TP_MAIN.Controls.Add(Me.TXT_DESCR, 0, 9)
            TP_MAIN.Controls.Add(Me.TXT_USER_FRIENDLY, 0, 1)
            TP_MAIN.Controls.Add(Me.TP_ADD_BY_LIST, 0, 7)
            TP_MAIN.Controls.Add(Me.TXT_LABELS, 0, 8)
            TP_MAIN.Controls.Add(TP_DOWN_OPTIONS, 0, 5)
            TP_MAIN.Controls.Add(Me.TXT_SPEC_FOLDER, 0, 2)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 10
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66708!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66542!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150.0!))
            TP_MAIN.Size = New System.Drawing.Size(454, 431)
            TP_MAIN.TabIndex = 0
            '
            'TXT_USER
            '
            Me.TXT_USER.CaptionText = "User name"
            Me.TXT_USER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_USER.Location = New System.Drawing.Point(4, 4)
            Me.TXT_USER.Name = "TXT_USER"
            Me.TXT_USER.PlaceholderEnabled = True
            Me.TXT_USER.PlaceholderText = "Enter user name or url here..."
            Me.TXT_USER.Size = New System.Drawing.Size(446, 22)
            Me.TXT_USER.TabIndex = 0
            '
            'TP_SITE
            '
            TP_SITE.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_SITE.ColumnCount = 3
            TP_SITE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 79.0!))
            TP_SITE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_SITE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 88.0!))
            TP_SITE.Controls.Add(Me.CH_IS_CHANNEL, 0, 0)
            TP_SITE.Controls.Add(Me.CMB_SITE, 1, 0)
            TP_SITE.Controls.Add(Me.BTT_OTHER_SETTINGS, 2, 0)
            TP_SITE.Dock = System.Windows.Forms.DockStyle.Fill
            TP_SITE.Location = New System.Drawing.Point(1, 88)
            TP_SITE.Margin = New System.Windows.Forms.Padding(0)
            TP_SITE.Name = "TP_SITE"
            TP_SITE.RowCount = 1
            TP_SITE.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_SITE.Size = New System.Drawing.Size(452, 31)
            TP_SITE.TabIndex = 3
            '
            'CH_IS_CHANNEL
            '
            Me.CH_IS_CHANNEL.AutoSize = True
            Me.CH_IS_CHANNEL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_IS_CHANNEL.Location = New System.Drawing.Point(4, 4)
            Me.CH_IS_CHANNEL.Name = "CH_IS_CHANNEL"
            Me.CH_IS_CHANNEL.Size = New System.Drawing.Size(73, 23)
            Me.CH_IS_CHANNEL.TabIndex = 0
            Me.CH_IS_CHANNEL.Text = "Channel"
            Me.CH_IS_CHANNEL.UseVisualStyleBackColor = True
            '
            'CMB_SITE
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Index = 0
            ActionButton1.Name = "BTT_COMBOBOX_ARROW"
            Me.CMB_SITE.Buttons.Add(ActionButton1)
            ListColumn1.Name = "_COL_KEY"
            ListColumn1.Text = "Key"
            ListColumn1.ValueMember = True
            ListColumn1.Visible = False
            ListColumn2.DisplayMember = True
            ListColumn2.Name = "_COL_VALUE"
            ListColumn2.Text = "Value"
            ListColumn2.Width = -1
            Me.CMB_SITE.Columns.Add(ListColumn1)
            Me.CMB_SITE.Columns.Add(ListColumn2)
            Me.CMB_SITE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_SITE.Location = New System.Drawing.Point(84, 2)
            Me.CMB_SITE.Margin = New System.Windows.Forms.Padding(3, 1, 3, 3)
            Me.CMB_SITE.Name = "CMB_SITE"
            Me.CMB_SITE.Size = New System.Drawing.Size(275, 22)
            Me.CMB_SITE.TabIndex = 1
            Me.CMB_SITE.TextBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            '
            'BTT_OTHER_SETTINGS
            '
            Me.BTT_OTHER_SETTINGS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_OTHER_SETTINGS.Location = New System.Drawing.Point(364, 2)
            Me.BTT_OTHER_SETTINGS.Margin = New System.Windows.Forms.Padding(1)
            Me.BTT_OTHER_SETTINGS.Name = "BTT_OTHER_SETTINGS"
            Me.BTT_OTHER_SETTINGS.Size = New System.Drawing.Size(86, 27)
            Me.BTT_OTHER_SETTINGS.TabIndex = 2
            Me.BTT_OTHER_SETTINGS.Text = "Options"
            TT_MAIN.SetToolTip(Me.BTT_OTHER_SETTINGS, "Other settings")
            Me.BTT_OTHER_SETTINGS.UseVisualStyleBackColor = True
            '
            'TP_PARAMS
            '
            TP_PARAMS.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_PARAMS.ColumnCount = 2
            TP_PARAMS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_PARAMS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_PARAMS.Controls.Add(Me.CH_TEMP, 0, 0)
            TP_PARAMS.Controls.Add(Me.CH_FAV, 1, 0)
            TP_PARAMS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_PARAMS.Location = New System.Drawing.Point(1, 120)
            TP_PARAMS.Margin = New System.Windows.Forms.Padding(0)
            TP_PARAMS.Name = "TP_PARAMS"
            TP_PARAMS.RowCount = 1
            TP_PARAMS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_PARAMS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            TP_PARAMS.Size = New System.Drawing.Size(452, 31)
            TP_PARAMS.TabIndex = 4
            '
            'CH_TEMP
            '
            Me.CH_TEMP.AutoSize = True
            Me.CH_TEMP.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_TEMP.Location = New System.Drawing.Point(4, 4)
            Me.CH_TEMP.Name = "CH_TEMP"
            Me.CH_TEMP.Size = New System.Drawing.Size(218, 23)
            Me.CH_TEMP.TabIndex = 0
            Me.CH_TEMP.Text = "Temporary"
            Me.CH_TEMP.UseVisualStyleBackColor = True
            '
            'CH_FAV
            '
            Me.CH_FAV.AutoSize = True
            Me.CH_FAV.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FAV.Location = New System.Drawing.Point(229, 4)
            Me.CH_FAV.Name = "CH_FAV"
            Me.CH_FAV.Size = New System.Drawing.Size(219, 23)
            Me.CH_FAV.TabIndex = 1
            Me.CH_FAV.Text = "Favorite"
            Me.CH_FAV.UseVisualStyleBackColor = True
            '
            'TP_OTHER
            '
            TP_OTHER.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_OTHER.ColumnCount = 2
            TP_OTHER.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_OTHER.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_OTHER.Controls.Add(Me.CH_PARSE_USER_MEDIA, 1, 0)
            TP_OTHER.Controls.Add(Me.CH_READY_FOR_DOWN, 0, 0)
            TP_OTHER.Dock = System.Windows.Forms.DockStyle.Fill
            TP_OTHER.Location = New System.Drawing.Point(1, 184)
            TP_OTHER.Margin = New System.Windows.Forms.Padding(0)
            TP_OTHER.Name = "TP_OTHER"
            TP_OTHER.RowCount = 1
            TP_OTHER.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_OTHER.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            TP_OTHER.Size = New System.Drawing.Size(452, 31)
            TP_OTHER.TabIndex = 6
            '
            'CH_PARSE_USER_MEDIA
            '
            Me.CH_PARSE_USER_MEDIA.AutoSize = True
            Me.CH_PARSE_USER_MEDIA.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_PARSE_USER_MEDIA.Location = New System.Drawing.Point(229, 4)
            Me.CH_PARSE_USER_MEDIA.Name = "CH_PARSE_USER_MEDIA"
            Me.CH_PARSE_USER_MEDIA.Size = New System.Drawing.Size(219, 23)
            Me.CH_PARSE_USER_MEDIA.TabIndex = 0
            Me.CH_PARSE_USER_MEDIA.Text = "Get user media only"
            TT_MAIN.SetToolTip(Me.CH_PARSE_USER_MEDIA, "For twitter only!" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "If checked then user media only will be downloaded." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Otherwise" &
        " all media (include comments and retwits) will be downloaded.")
            Me.CH_PARSE_USER_MEDIA.UseVisualStyleBackColor = True
            '
            'CH_READY_FOR_DOWN
            '
            Me.CH_READY_FOR_DOWN.AutoSize = True
            Me.CH_READY_FOR_DOWN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_READY_FOR_DOWN.Location = New System.Drawing.Point(4, 4)
            Me.CH_READY_FOR_DOWN.Name = "CH_READY_FOR_DOWN"
            Me.CH_READY_FOR_DOWN.Size = New System.Drawing.Size(218, 23)
            Me.CH_READY_FOR_DOWN.TabIndex = 1
            Me.CH_READY_FOR_DOWN.Text = "Ready for download"
            TT_MAIN.SetToolTip(Me.CH_READY_FOR_DOWN, "Can be downloaded by [Download All]")
            Me.CH_READY_FOR_DOWN.UseVisualStyleBackColor = True
            '
            'TXT_DESCR
            '
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Dock = System.Windows.Forms.DockStyle.Top
            ActionButton2.Index = 0
            ActionButton2.Name = "BTT_CLEAR"
            Me.TXT_DESCR.Buttons.Add(ActionButton2)
            Me.TXT_DESCR.CaptionDock = System.Windows.Forms.DockStyle.Top
            Me.TXT_DESCR.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.None
            Me.TXT_DESCR.CaptionVisible = False
            Me.TXT_DESCR.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_DESCR.GroupBoxed = True
            Me.TXT_DESCR.GroupBoxText = "Description"
            Me.TXT_DESCR.Location = New System.Drawing.Point(4, 282)
            Me.TXT_DESCR.Multiline = True
            Me.TXT_DESCR.Name = "TXT_DESCR"
            Me.TXT_DESCR.Size = New System.Drawing.Size(446, 145)
            Me.TXT_DESCR.TabIndex = 9
            '
            'TXT_USER_FRIENDLY
            '
            Me.TXT_USER_FRIENDLY.CaptionText = "Friendly name"
            Me.TXT_USER_FRIENDLY.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_USER_FRIENDLY.Location = New System.Drawing.Point(4, 33)
            Me.TXT_USER_FRIENDLY.Name = "TXT_USER_FRIENDLY"
            Me.TXT_USER_FRIENDLY.Size = New System.Drawing.Size(446, 22)
            Me.TXT_USER_FRIENDLY.TabIndex = 1
            '
            'TP_ADD_BY_LIST
            '
            Me.TP_ADD_BY_LIST.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            Me.TP_ADD_BY_LIST.ColumnCount = 2
            Me.TP_ADD_BY_LIST.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TP_ADD_BY_LIST.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TP_ADD_BY_LIST.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TP_ADD_BY_LIST.Controls.Add(Me.CH_ADD_BY_LIST, 0, 0)
            Me.TP_ADD_BY_LIST.Controls.Add(Me.CH_AUTO_DETECT_SITE, 1, 0)
            Me.TP_ADD_BY_LIST.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_ADD_BY_LIST.Location = New System.Drawing.Point(1, 216)
            Me.TP_ADD_BY_LIST.Margin = New System.Windows.Forms.Padding(0)
            Me.TP_ADD_BY_LIST.Name = "TP_ADD_BY_LIST"
            Me.TP_ADD_BY_LIST.RowCount = 1
            Me.TP_ADD_BY_LIST.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_ADD_BY_LIST.Size = New System.Drawing.Size(452, 30)
            Me.TP_ADD_BY_LIST.TabIndex = 7
            '
            'CH_ADD_BY_LIST
            '
            Me.CH_ADD_BY_LIST.AutoSize = True
            Me.CH_ADD_BY_LIST.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_ADD_BY_LIST.Location = New System.Drawing.Point(4, 4)
            Me.CH_ADD_BY_LIST.Name = "CH_ADD_BY_LIST"
            Me.CH_ADD_BY_LIST.Size = New System.Drawing.Size(218, 22)
            Me.CH_ADD_BY_LIST.TabIndex = 0
            Me.CH_ADD_BY_LIST.Text = "Add by list"
            Me.CH_ADD_BY_LIST.UseVisualStyleBackColor = True
            '
            'CH_AUTO_DETECT_SITE
            '
            Me.CH_AUTO_DETECT_SITE.AutoSize = True
            Me.CH_AUTO_DETECT_SITE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_AUTO_DETECT_SITE.Location = New System.Drawing.Point(229, 4)
            Me.CH_AUTO_DETECT_SITE.Name = "CH_AUTO_DETECT_SITE"
            Me.CH_AUTO_DETECT_SITE.Size = New System.Drawing.Size(219, 22)
            Me.CH_AUTO_DETECT_SITE.TabIndex = 1
            Me.CH_AUTO_DETECT_SITE.Text = "Auto detect site"
            Me.CH_AUTO_DETECT_SITE.UseVisualStyleBackColor = True
            '
            'TXT_LABELS
            '
            ActionButton3.BackgroundImage = CType(resources.GetObject("ActionButton3.BackgroundImage"), System.Drawing.Image)
            ActionButton3.Index = 0
            ActionButton3.Name = "BTT_OPEN"
            ActionButton4.BackgroundImage = CType(resources.GetObject("ActionButton4.BackgroundImage"), System.Drawing.Image)
            ActionButton4.Index = 1
            ActionButton4.Name = "BTT_CLEAR"
            Me.TXT_LABELS.Buttons.Add(ActionButton3)
            Me.TXT_LABELS.Buttons.Add(ActionButton4)
            Me.TXT_LABELS.CaptionText = "Labels"
            Me.TXT_LABELS.CaptionWidth = 50.0R
            Me.TXT_LABELS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_LABELS.Location = New System.Drawing.Point(4, 250)
            Me.TXT_LABELS.Name = "TXT_LABELS"
            Me.TXT_LABELS.Size = New System.Drawing.Size(446, 22)
            Me.TXT_LABELS.TabIndex = 8
            Me.TXT_LABELS.TextBoxReadOnly = True
            '
            'TP_DOWN_OPTIONS
            '
            TP_DOWN_OPTIONS.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_DOWN_OPTIONS.ColumnCount = 2
            TP_DOWN_OPTIONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_DOWN_OPTIONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_DOWN_OPTIONS.Controls.Add(Me.CH_DOWN_IMAGES, 0, 0)
            TP_DOWN_OPTIONS.Controls.Add(Me.CH_DOWN_VIDEOS, 1, 0)
            TP_DOWN_OPTIONS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_DOWN_OPTIONS.Location = New System.Drawing.Point(1, 152)
            TP_DOWN_OPTIONS.Margin = New System.Windows.Forms.Padding(0)
            TP_DOWN_OPTIONS.Name = "TP_DOWN_OPTIONS"
            TP_DOWN_OPTIONS.RowCount = 1
            TP_DOWN_OPTIONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_DOWN_OPTIONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            TP_DOWN_OPTIONS.Size = New System.Drawing.Size(452, 31)
            TP_DOWN_OPTIONS.TabIndex = 5
            '
            'CH_DOWN_IMAGES
            '
            Me.CH_DOWN_IMAGES.AutoSize = True
            Me.CH_DOWN_IMAGES.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DOWN_IMAGES.Location = New System.Drawing.Point(4, 4)
            Me.CH_DOWN_IMAGES.Name = "CH_DOWN_IMAGES"
            Me.CH_DOWN_IMAGES.Size = New System.Drawing.Size(218, 23)
            Me.CH_DOWN_IMAGES.TabIndex = 0
            Me.CH_DOWN_IMAGES.Text = "Download Images"
            Me.CH_DOWN_IMAGES.UseVisualStyleBackColor = True
            '
            'CH_DOWN_VIDEOS
            '
            Me.CH_DOWN_VIDEOS.AutoSize = True
            Me.CH_DOWN_VIDEOS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DOWN_VIDEOS.Location = New System.Drawing.Point(229, 4)
            Me.CH_DOWN_VIDEOS.Name = "CH_DOWN_VIDEOS"
            Me.CH_DOWN_VIDEOS.Size = New System.Drawing.Size(219, 23)
            Me.CH_DOWN_VIDEOS.TabIndex = 1
            Me.CH_DOWN_VIDEOS.Text = "Download videos"
            Me.CH_DOWN_VIDEOS.UseVisualStyleBackColor = True
            '
            'TXT_SPEC_FOLDER
            '
            ActionButton5.BackgroundImage = CType(resources.GetObject("ActionButton5.BackgroundImage"), System.Drawing.Image)
            ActionButton5.Index = 0
            ActionButton5.Name = "BTT_OPEN"
            ActionButton5.ToolTipText = "Select a new path in the folder selection dialog"
            ActionButton6.BackgroundImage = CType(resources.GetObject("ActionButton6.BackgroundImage"), System.Drawing.Image)
            ActionButton6.Index = 1
            ActionButton6.Name = "BTT_CLEAR"
            ActionButton6.ToolTipText = "Clear"
            Me.TXT_SPEC_FOLDER.Buttons.Add(ActionButton5)
            Me.TXT_SPEC_FOLDER.Buttons.Add(ActionButton6)
            Me.TXT_SPEC_FOLDER.CaptionText = "Special path"
            Me.TXT_SPEC_FOLDER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_SPEC_FOLDER.Location = New System.Drawing.Point(4, 62)
            Me.TXT_SPEC_FOLDER.Name = "TXT_SPEC_FOLDER"
            Me.TXT_SPEC_FOLDER.Size = New System.Drawing.Size(446, 22)
            Me.TXT_SPEC_FOLDER.TabIndex = 2
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            Me.CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            Me.CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(454, 431)
            Me.CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CONTAINER_MAIN.LeftToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            Me.CONTAINER_MAIN.RightToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Size = New System.Drawing.Size(454, 431)
            Me.CONTAINER_MAIN.TabIndex = 0
            Me.CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'UserCreatorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(454, 431)
            Me.Controls.Add(Me.CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(470, 470)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(470, 470)
            Me.Name = "UserCreatorForm"
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Create User"
            TP_MAIN.ResumeLayout(False)
            CType(Me.TXT_USER, System.ComponentModel.ISupportInitialize).EndInit()
            TP_SITE.ResumeLayout(False)
            TP_SITE.PerformLayout()
            CType(Me.CMB_SITE, System.ComponentModel.ISupportInitialize).EndInit()
            TP_PARAMS.ResumeLayout(False)
            TP_PARAMS.PerformLayout()
            TP_OTHER.ResumeLayout(False)
            TP_OTHER.PerformLayout()
            CType(Me.TXT_DESCR, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_USER_FRIENDLY, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TP_ADD_BY_LIST.ResumeLayout(False)
            Me.TP_ADD_BY_LIST.PerformLayout()
            CType(Me.TXT_LABELS, System.ComponentModel.ISupportInitialize).EndInit()
            TP_DOWN_OPTIONS.ResumeLayout(False)
            TP_DOWN_OPTIONS.PerformLayout()
            CType(Me.TXT_SPEC_FOLDER, System.ComponentModel.ISupportInitialize).EndInit()
            Me.CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            Me.CONTAINER_MAIN.ResumeLayout(False)
            Me.CONTAINER_MAIN.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents CONTAINER_MAIN As ToolStripContainer
        Private WithEvents TXT_USER As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CH_TEMP As CheckBox
        Private WithEvents CH_FAV As CheckBox
        Private WithEvents CH_PARSE_USER_MEDIA As CheckBox
        Private WithEvents TXT_USER_FRIENDLY As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CH_READY_FOR_DOWN As CheckBox
        Private WithEvents TXT_DESCR As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TP_ADD_BY_LIST As TableLayoutPanel
        Private WithEvents CH_ADD_BY_LIST As CheckBox
        Private WithEvents CH_AUTO_DETECT_SITE As CheckBox
        Private WithEvents TXT_LABELS As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CH_DOWN_IMAGES As CheckBox
        Private WithEvents CH_DOWN_VIDEOS As CheckBox
        Private WithEvents CH_IS_CHANNEL As CheckBox
        Private WithEvents TXT_SPEC_FOLDER As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CMB_SITE As PersonalUtilities.Forms.Controls.ComboBoxExtended
        Private WithEvents BTT_OTHER_SETTINGS As Button
    End Class
End Namespace