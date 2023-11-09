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
            Dim TT_MAIN As System.Windows.Forms.ToolTip
            Dim CONTAINER_MAIN As System.Windows.Forms.ToolStripContainer
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UserCreatorForm))
            Dim ListColumn1 As PersonalUtilities.Forms.Controls.Base.ListColumn = New PersonalUtilities.Forms.Controls.Base.ListColumn()
            Dim ListColumn2 As PersonalUtilities.Forms.Controls.Base.ListColumn = New PersonalUtilities.Forms.Controls.Base.ListColumn()
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton4 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton5 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton6 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton7 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton8 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton9 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton10 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Me.CH_PARSE_USER_MEDIA = New System.Windows.Forms.CheckBox()
            Me.CH_READY_FOR_DOWN = New System.Windows.Forms.CheckBox()
            Me.BTT_OTHER_SETTINGS = New System.Windows.Forms.Button()
            Me.TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            Me.TXT_USER = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TP_SITE = New System.Windows.Forms.TableLayoutPanel()
            Me.CMB_SITE = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.TP_TEMP_FAV = New System.Windows.Forms.TableLayoutPanel()
            Me.CH_TEMP = New System.Windows.Forms.CheckBox()
            Me.CH_FAV = New System.Windows.Forms.CheckBox()
            Me.TP_READY_USERMEDIA = New System.Windows.Forms.TableLayoutPanel()
            Me.TXT_DESCR = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_USER_FRIENDLY = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TP_ADD_BY_LIST = New System.Windows.Forms.TableLayoutPanel()
            Me.CH_ADD_BY_LIST = New System.Windows.Forms.CheckBox()
            Me.CH_AUTO_DETECT_SITE = New System.Windows.Forms.CheckBox()
            Me.TXT_LABELS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TP_DOWN_IMG_VID = New System.Windows.Forms.TableLayoutPanel()
            Me.CH_DOWN_IMAGES = New System.Windows.Forms.CheckBox()
            Me.CH_DOWN_VIDEOS = New System.Windows.Forms.CheckBox()
            Me.TXT_SPEC_FOLDER = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.TXT_SCRIPT = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.COLOR_USER = New SCrawler.Editors.ColorPicker()
            Me.CMB_ACCOUNT = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            Me.TP_MAIN.SuspendLayout()
            CType(Me.TXT_USER, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TP_SITE.SuspendLayout()
            CType(Me.CMB_SITE, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TP_TEMP_FAV.SuspendLayout()
            Me.TP_READY_USERMEDIA.SuspendLayout()
            CType(Me.TXT_DESCR, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_USER_FRIENDLY, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TP_ADD_BY_LIST.SuspendLayout()
            CType(Me.TXT_LABELS, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TP_DOWN_IMG_VID.SuspendLayout()
            CType(Me.TXT_SPEC_FOLDER, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_SCRIPT, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.CMB_ACCOUNT, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'CH_PARSE_USER_MEDIA
            '
            Me.CH_PARSE_USER_MEDIA.AutoSize = True
            Me.CH_PARSE_USER_MEDIA.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_PARSE_USER_MEDIA.Location = New System.Drawing.Point(229, 4)
            Me.CH_PARSE_USER_MEDIA.Name = "CH_PARSE_USER_MEDIA"
            Me.CH_PARSE_USER_MEDIA.Size = New System.Drawing.Size(219, 20)
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
            Me.CH_READY_FOR_DOWN.Size = New System.Drawing.Size(218, 20)
            Me.CH_READY_FOR_DOWN.TabIndex = 1
            Me.CH_READY_FOR_DOWN.Text = "Ready for download"
            TT_MAIN.SetToolTip(Me.CH_READY_FOR_DOWN, "Can be downloaded by [Download All]")
            Me.CH_READY_FOR_DOWN.UseVisualStyleBackColor = True
            '
            'BTT_OTHER_SETTINGS
            '
            Me.BTT_OTHER_SETTINGS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_OTHER_SETTINGS.Location = New System.Drawing.Point(353, 1)
            Me.BTT_OTHER_SETTINGS.Margin = New System.Windows.Forms.Padding(1)
            Me.BTT_OTHER_SETTINGS.Name = "BTT_OTHER_SETTINGS"
            Me.BTT_OTHER_SETTINGS.Size = New System.Drawing.Size(98, 26)
            Me.BTT_OTHER_SETTINGS.TabIndex = 1
            Me.BTT_OTHER_SETTINGS.Text = "Options (F2)"
            TT_MAIN.SetToolTip(Me.BTT_OTHER_SETTINGS, "Other settings")
            Me.BTT_OTHER_SETTINGS.UseVisualStyleBackColor = True
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(Me.TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(454, 489)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(454, 489)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            Me.TP_MAIN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            Me.TP_MAIN.ColumnCount = 1
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.Controls.Add(Me.TXT_USER, 0, 0)
            Me.TP_MAIN.Controls.Add(Me.TP_SITE, 0, 3)
            Me.TP_MAIN.Controls.Add(Me.TP_TEMP_FAV, 0, 5)
            Me.TP_MAIN.Controls.Add(Me.TP_READY_USERMEDIA, 0, 7)
            Me.TP_MAIN.Controls.Add(Me.TXT_DESCR, 0, 12)
            Me.TP_MAIN.Controls.Add(Me.TXT_USER_FRIENDLY, 0, 1)
            Me.TP_MAIN.Controls.Add(Me.TP_ADD_BY_LIST, 0, 8)
            Me.TP_MAIN.Controls.Add(Me.TXT_LABELS, 0, 9)
            Me.TP_MAIN.Controls.Add(Me.TP_DOWN_IMG_VID, 0, 6)
            Me.TP_MAIN.Controls.Add(Me.TXT_SPEC_FOLDER, 0, 2)
            Me.TP_MAIN.Controls.Add(Me.TXT_SCRIPT, 0, 11)
            Me.TP_MAIN.Controls.Add(Me.COLOR_USER, 0, 10)
            Me.TP_MAIN.Controls.Add(Me.CMB_ACCOUNT, 0, 4)
            Me.TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.TP_MAIN.Name = "TP_MAIN"
            Me.TP_MAIN.RowCount = 13
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.Size = New System.Drawing.Size(454, 489)
            Me.TP_MAIN.TabIndex = 0
            '
            'TXT_USER
            '
            Me.TXT_USER.CaptionText = "User name"
            Me.TXT_USER.CaptionToolTipEnabled = True
            Me.TXT_USER.CaptionToolTipText = "You must enter the user's URL in this field."
            Me.TXT_USER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_USER.Location = New System.Drawing.Point(4, 4)
            Me.TXT_USER.Name = "TXT_USER"
            Me.TXT_USER.PlaceholderEnabled = True
            Me.TXT_USER.PlaceholderText = "Enter user profile URL here..."
            Me.TXT_USER.Size = New System.Drawing.Size(446, 22)
            Me.TXT_USER.TabIndex = 0
            '
            'TP_SITE
            '
            Me.TP_SITE.ColumnCount = 2
            Me.TP_SITE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_SITE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
            Me.TP_SITE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TP_SITE.Controls.Add(Me.CMB_SITE, 0, 0)
            Me.TP_SITE.Controls.Add(Me.BTT_OTHER_SETTINGS, 1, 0)
            Me.TP_SITE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_SITE.Location = New System.Drawing.Point(1, 88)
            Me.TP_SITE.Margin = New System.Windows.Forms.Padding(0)
            Me.TP_SITE.Name = "TP_SITE"
            Me.TP_SITE.RowCount = 1
            Me.TP_SITE.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_SITE.Size = New System.Drawing.Size(452, 28)
            Me.TP_SITE.TabIndex = 3
            '
            'CMB_SITE
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Name = "ArrowDown"
            Me.CMB_SITE.Buttons.Add(ActionButton1)
            Me.CMB_SITE.CaptionCheckAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.CMB_SITE.CaptionMargin = New System.Windows.Forms.Padding(4, 3, 3, 3)
            Me.CMB_SITE.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
            Me.CMB_SITE.CaptionText = "Subscription"
            Me.CMB_SITE.CaptionTextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.CMB_SITE.CaptionToolTipEnabled = True
            Me.CMB_SITE.CaptionToolTipText = resources.GetString("CMB_SITE.CaptionToolTipText")
            Me.CMB_SITE.CaptionVisible = True
            Me.CMB_SITE.CaptionWidth = 103.0R
            Me.CMB_SITE.ChangeControlsEnableOnCheckedChange = False
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
            Me.CMB_SITE.Location = New System.Drawing.Point(0, 3)
            Me.CMB_SITE.Margin = New System.Windows.Forms.Padding(0, 3, 3, 3)
            Me.CMB_SITE.Name = "CMB_SITE"
            Me.CMB_SITE.Size = New System.Drawing.Size(349, 22)
            Me.CMB_SITE.TabIndex = 0
            Me.CMB_SITE.TextBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            '
            'TP_TEMP_FAV
            '
            Me.TP_TEMP_FAV.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            Me.TP_TEMP_FAV.ColumnCount = 2
            Me.TP_TEMP_FAV.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TP_TEMP_FAV.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TP_TEMP_FAV.Controls.Add(Me.CH_TEMP, 0, 0)
            Me.TP_TEMP_FAV.Controls.Add(Me.CH_FAV, 1, 0)
            Me.TP_TEMP_FAV.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_TEMP_FAV.Location = New System.Drawing.Point(1, 146)
            Me.TP_TEMP_FAV.Margin = New System.Windows.Forms.Padding(0)
            Me.TP_TEMP_FAV.Name = "TP_TEMP_FAV"
            Me.TP_TEMP_FAV.RowCount = 1
            Me.TP_TEMP_FAV.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_TEMP_FAV.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27.0!))
            Me.TP_TEMP_FAV.Size = New System.Drawing.Size(452, 28)
            Me.TP_TEMP_FAV.TabIndex = 5
            '
            'CH_TEMP
            '
            Me.CH_TEMP.AutoSize = True
            Me.CH_TEMP.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_TEMP.Location = New System.Drawing.Point(4, 4)
            Me.CH_TEMP.Name = "CH_TEMP"
            Me.CH_TEMP.Size = New System.Drawing.Size(218, 20)
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
            Me.CH_FAV.Size = New System.Drawing.Size(219, 20)
            Me.CH_FAV.TabIndex = 1
            Me.CH_FAV.Text = "Favorite"
            Me.CH_FAV.UseVisualStyleBackColor = True
            '
            'TP_READY_USERMEDIA
            '
            Me.TP_READY_USERMEDIA.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            Me.TP_READY_USERMEDIA.ColumnCount = 2
            Me.TP_READY_USERMEDIA.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TP_READY_USERMEDIA.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TP_READY_USERMEDIA.Controls.Add(Me.CH_PARSE_USER_MEDIA, 1, 0)
            Me.TP_READY_USERMEDIA.Controls.Add(Me.CH_READY_FOR_DOWN, 0, 0)
            Me.TP_READY_USERMEDIA.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_READY_USERMEDIA.Location = New System.Drawing.Point(1, 204)
            Me.TP_READY_USERMEDIA.Margin = New System.Windows.Forms.Padding(0)
            Me.TP_READY_USERMEDIA.Name = "TP_READY_USERMEDIA"
            Me.TP_READY_USERMEDIA.RowCount = 1
            Me.TP_READY_USERMEDIA.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_READY_USERMEDIA.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27.0!))
            Me.TP_READY_USERMEDIA.Size = New System.Drawing.Size(452, 28)
            Me.TP_READY_USERMEDIA.TabIndex = 7
            '
            'TXT_DESCR
            '
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Dock = System.Windows.Forms.DockStyle.Top
            ActionButton2.Name = "Clear"
            Me.TXT_DESCR.Buttons.Add(ActionButton2)
            Me.TXT_DESCR.CaptionDock = System.Windows.Forms.DockStyle.Top
            Me.TXT_DESCR.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.None
            Me.TXT_DESCR.CaptionVisible = False
            Me.TXT_DESCR.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_DESCR.GroupBoxed = True
            Me.TXT_DESCR.GroupBoxText = "Description"
            Me.TXT_DESCR.Location = New System.Drawing.Point(4, 346)
            Me.TXT_DESCR.Multiline = True
            Me.TXT_DESCR.Name = "TXT_DESCR"
            Me.TXT_DESCR.Size = New System.Drawing.Size(446, 139)
            Me.TXT_DESCR.TabIndex = 12
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
            Me.TP_ADD_BY_LIST.Location = New System.Drawing.Point(1, 233)
            Me.TP_ADD_BY_LIST.Margin = New System.Windows.Forms.Padding(0)
            Me.TP_ADD_BY_LIST.Name = "TP_ADD_BY_LIST"
            Me.TP_ADD_BY_LIST.RowCount = 1
            Me.TP_ADD_BY_LIST.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_ADD_BY_LIST.Size = New System.Drawing.Size(452, 28)
            Me.TP_ADD_BY_LIST.TabIndex = 8
            '
            'CH_ADD_BY_LIST
            '
            Me.CH_ADD_BY_LIST.AutoSize = True
            Me.CH_ADD_BY_LIST.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_ADD_BY_LIST.Location = New System.Drawing.Point(4, 4)
            Me.CH_ADD_BY_LIST.Name = "CH_ADD_BY_LIST"
            Me.CH_ADD_BY_LIST.Size = New System.Drawing.Size(218, 20)
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
            Me.CH_AUTO_DETECT_SITE.Size = New System.Drawing.Size(219, 20)
            Me.CH_AUTO_DETECT_SITE.TabIndex = 1
            Me.CH_AUTO_DETECT_SITE.Text = "Auto detect site"
            Me.CH_AUTO_DETECT_SITE.UseVisualStyleBackColor = True
            '
            'TXT_LABELS
            '
            ActionButton3.BackgroundImage = CType(resources.GetObject("ActionButton3.BackgroundImage"), System.Drawing.Image)
            ActionButton3.Name = "Open"
            ActionButton4.BackgroundImage = CType(resources.GetObject("ActionButton4.BackgroundImage"), System.Drawing.Image)
            ActionButton4.Name = "Clear"
            Me.TXT_LABELS.Buttons.Add(ActionButton3)
            Me.TXT_LABELS.Buttons.Add(ActionButton4)
            Me.TXT_LABELS.CaptionText = "Labels"
            Me.TXT_LABELS.CaptionWidth = 50.0R
            Me.TXT_LABELS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_LABELS.Location = New System.Drawing.Point(4, 264)
            Me.TXT_LABELS.Margin = New System.Windows.Forms.Padding(3, 2, 3, 3)
            Me.TXT_LABELS.Name = "TXT_LABELS"
            Me.TXT_LABELS.Size = New System.Drawing.Size(446, 22)
            Me.TXT_LABELS.TabIndex = 9
            Me.TXT_LABELS.TextBoxReadOnly = True
            '
            'TP_DOWN_IMG_VID
            '
            Me.TP_DOWN_IMG_VID.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            Me.TP_DOWN_IMG_VID.ColumnCount = 2
            Me.TP_DOWN_IMG_VID.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TP_DOWN_IMG_VID.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TP_DOWN_IMG_VID.Controls.Add(Me.CH_DOWN_IMAGES, 0, 0)
            Me.TP_DOWN_IMG_VID.Controls.Add(Me.CH_DOWN_VIDEOS, 1, 0)
            Me.TP_DOWN_IMG_VID.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_DOWN_IMG_VID.Location = New System.Drawing.Point(1, 175)
            Me.TP_DOWN_IMG_VID.Margin = New System.Windows.Forms.Padding(0)
            Me.TP_DOWN_IMG_VID.Name = "TP_DOWN_IMG_VID"
            Me.TP_DOWN_IMG_VID.RowCount = 1
            Me.TP_DOWN_IMG_VID.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_DOWN_IMG_VID.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27.0!))
            Me.TP_DOWN_IMG_VID.Size = New System.Drawing.Size(452, 28)
            Me.TP_DOWN_IMG_VID.TabIndex = 6
            '
            'CH_DOWN_IMAGES
            '
            Me.CH_DOWN_IMAGES.AutoSize = True
            Me.CH_DOWN_IMAGES.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DOWN_IMAGES.Location = New System.Drawing.Point(4, 4)
            Me.CH_DOWN_IMAGES.Name = "CH_DOWN_IMAGES"
            Me.CH_DOWN_IMAGES.Size = New System.Drawing.Size(218, 20)
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
            Me.CH_DOWN_VIDEOS.Size = New System.Drawing.Size(219, 20)
            Me.CH_DOWN_VIDEOS.TabIndex = 1
            Me.CH_DOWN_VIDEOS.Text = "Download videos"
            Me.CH_DOWN_VIDEOS.UseVisualStyleBackColor = True
            '
            'TXT_SPEC_FOLDER
            '
            ActionButton5.BackgroundImage = CType(resources.GetObject("ActionButton5.BackgroundImage"), System.Drawing.Image)
            ActionButton5.Name = "Open"
            ActionButton5.ToolTipText = "Select a new path in the folder selection dialog"
            ActionButton6.BackgroundImage = CType(resources.GetObject("ActionButton6.BackgroundImage"), System.Drawing.Image)
            ActionButton6.Name = "Clear"
            ActionButton6.ToolTipText = "Clear"
            ActionButton7.BackgroundImage = CType(resources.GetObject("ActionButton7.BackgroundImage"), System.Drawing.Image)
            ActionButton7.Name = "ArrowDown"
            ActionButton7.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.ArrowDown
            Me.TXT_SPEC_FOLDER.Buttons.Add(ActionButton5)
            Me.TXT_SPEC_FOLDER.Buttons.Add(ActionButton6)
            Me.TXT_SPEC_FOLDER.Buttons.Add(ActionButton7)
            Me.TXT_SPEC_FOLDER.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.Label
            Me.TXT_SPEC_FOLDER.CaptionText = "Special path"
            Me.TXT_SPEC_FOLDER.CaptionVisible = True
            Me.TXT_SPEC_FOLDER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_SPEC_FOLDER.Location = New System.Drawing.Point(4, 62)
            Me.TXT_SPEC_FOLDER.Name = "TXT_SPEC_FOLDER"
            Me.TXT_SPEC_FOLDER.Size = New System.Drawing.Size(446, 22)
            Me.TXT_SPEC_FOLDER.TabIndex = 2
            Me.TXT_SPEC_FOLDER.TextBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            '
            'TXT_SCRIPT
            '
            ActionButton8.BackgroundImage = CType(resources.GetObject("ActionButton8.BackgroundImage"), System.Drawing.Image)
            ActionButton8.Enabled = False
            ActionButton8.Name = "Open"
            ActionButton9.BackgroundImage = CType(resources.GetObject("ActionButton9.BackgroundImage"), System.Drawing.Image)
            ActionButton9.Enabled = False
            ActionButton9.Name = "Clear"
            Me.TXT_SCRIPT.Buttons.Add(ActionButton8)
            Me.TXT_SCRIPT.Buttons.Add(ActionButton9)
            Me.TXT_SCRIPT.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
            Me.TXT_SCRIPT.CaptionText = "Script"
            Me.TXT_SCRIPT.CaptionToolTipEnabled = True
            Me.TXT_SCRIPT.CaptionToolTipText = "Execute script after downloading this user"
            Me.TXT_SCRIPT.CaptionWidth = 65.0R
            Me.TXT_SCRIPT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_SCRIPT.Location = New System.Drawing.Point(4, 318)
            Me.TXT_SCRIPT.Margin = New System.Windows.Forms.Padding(3, 2, 3, 3)
            Me.TXT_SCRIPT.Name = "TXT_SCRIPT"
            Me.TXT_SCRIPT.PlaceholderEnabled = True
            Me.TXT_SCRIPT.PlaceholderText = "Leave blank to use the default script..."
            Me.TXT_SCRIPT.Size = New System.Drawing.Size(446, 22)
            Me.TXT_SCRIPT.TabIndex = 11
            '
            'COLOR_USER
            '
            Me.COLOR_USER.ButtonsMargin = New System.Windows.Forms.Padding(1)
            Me.COLOR_USER.CaptionText = "Color"
            Me.COLOR_USER.CaptionWidth = 55
            Me.COLOR_USER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.COLOR_USER.Location = New System.Drawing.Point(2, 290)
            Me.COLOR_USER.Margin = New System.Windows.Forms.Padding(1, 1, 2, 1)
            Me.COLOR_USER.Name = "COLOR_USER"
            Me.COLOR_USER.Size = New System.Drawing.Size(449, 24)
            Me.COLOR_USER.TabIndex = 10
            '
            'CMB_ACCOUNT
            '
            ActionButton10.BackgroundImage = CType(resources.GetObject("ActionButton10.BackgroundImage"), System.Drawing.Image)
            ActionButton10.Name = "ArrowDown"
            ActionButton10.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.ArrowDown
            Me.CMB_ACCOUNT.Buttons.Add(ActionButton10)
            Me.CMB_ACCOUNT.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.Label
            Me.CMB_ACCOUNT.CaptionText = "Account"
            Me.CMB_ACCOUNT.CaptionVisible = True
            Me.CMB_ACCOUNT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_ACCOUNT.Location = New System.Drawing.Point(4, 120)
            Me.CMB_ACCOUNT.Name = "CMB_ACCOUNT"
            Me.CMB_ACCOUNT.Size = New System.Drawing.Size(446, 22)
            Me.CMB_ACCOUNT.TabIndex = 4
            Me.CMB_ACCOUNT.TextBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            '
            'UserCreatorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(454, 489)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Icon = Global.SCrawler.My.Resources.Resources.UsersIcon_32
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(470, 528)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(470, 528)
            Me.Name = "UserCreatorForm"
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Create user"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            Me.TP_MAIN.ResumeLayout(False)
            CType(Me.TXT_USER, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TP_SITE.ResumeLayout(False)
            CType(Me.CMB_SITE, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TP_TEMP_FAV.ResumeLayout(False)
            Me.TP_TEMP_FAV.PerformLayout()
            Me.TP_READY_USERMEDIA.ResumeLayout(False)
            Me.TP_READY_USERMEDIA.PerformLayout()
            CType(Me.TXT_DESCR, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_USER_FRIENDLY, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TP_ADD_BY_LIST.ResumeLayout(False)
            Me.TP_ADD_BY_LIST.PerformLayout()
            CType(Me.TXT_LABELS, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TP_DOWN_IMG_VID.ResumeLayout(False)
            Me.TP_DOWN_IMG_VID.PerformLayout()
            CType(Me.TXT_SPEC_FOLDER, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_SCRIPT, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.CMB_ACCOUNT, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
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
        Private WithEvents TXT_SPEC_FOLDER As PersonalUtilities.Forms.Controls.ComboBoxExtended
        Private WithEvents CMB_SITE As PersonalUtilities.Forms.Controls.ComboBoxExtended
        Private WithEvents BTT_OTHER_SETTINGS As Button
        Private WithEvents TXT_SCRIPT As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TP_SITE As TableLayoutPanel
        Private WithEvents TP_MAIN As TableLayoutPanel
        Private WithEvents TP_TEMP_FAV As TableLayoutPanel
        Private WithEvents TP_READY_USERMEDIA As TableLayoutPanel
        Private WithEvents TP_DOWN_IMG_VID As TableLayoutPanel
        Private WithEvents COLOR_USER As ColorPicker
        Private WithEvents CMB_ACCOUNT As PersonalUtilities.Forms.Controls.ComboBoxExtended
    End Class
End Namespace