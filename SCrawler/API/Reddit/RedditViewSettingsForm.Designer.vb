' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.Reddit
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class RedditViewSettingsForm : Inherits System.Windows.Forms.Form
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
            Dim CONTAINER_MAIN As System.Windows.Forms.ToolStripContainer
            Dim TP_VIEW_MODE As System.Windows.Forms.TableLayoutPanel
            Dim LBL_VIEW_MODE As System.Windows.Forms.Label
            Dim LBL_PERIOD As System.Windows.Forms.Label
            Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RedditViewSettingsForm))
            Dim ActionButton4 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim TP_TEXT As System.Windows.Forms.TableLayoutPanel
            Me.TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            Me.OPT_VIEW_MODE_NEW = New System.Windows.Forms.RadioButton()
            Me.OPT_VIEW_MODE_HOT = New System.Windows.Forms.RadioButton()
            Me.OPT_VIEW_MODE_TOP = New System.Windows.Forms.RadioButton()
            Me.OPT_VIEW_MODE_BEST = New System.Windows.Forms.RadioButton()
            Me.OPT_VIEW_MODE_RISING = New System.Windows.Forms.RadioButton()
            Me.TP_PERIOD = New System.Windows.Forms.TableLayoutPanel()
            Me.OPT_PERIOD_ALL = New System.Windows.Forms.RadioButton()
            Me.OPT_PERIOD_HOUR = New System.Windows.Forms.RadioButton()
            Me.OPT_PERIOD_DAY = New System.Windows.Forms.RadioButton()
            Me.OPT_PERIOD_WEEK = New System.Windows.Forms.RadioButton()
            Me.OPT_PERIOD_MONTH = New System.Windows.Forms.RadioButton()
            Me.OPT_PERIOD_YEAR = New System.Windows.Forms.RadioButton()
            Me.CMB_REDGIFS_ACC = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.CMB_REDDIT_ACC = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.CH_TXT_DOWN_TXT = New System.Windows.Forms.CheckBox()
            Me.CH_TXT_DOWN_POSTS = New System.Windows.Forms.CheckBox()
            Me.TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
            Me.CH_TXT_DOWN_SPEC_FOLDER = New System.Windows.Forms.CheckBox()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_VIEW_MODE = New System.Windows.Forms.TableLayoutPanel()
            LBL_VIEW_MODE = New System.Windows.Forms.Label()
            LBL_PERIOD = New System.Windows.Forms.Label()
            TP_TEXT = New System.Windows.Forms.TableLayoutPanel()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            Me.TP_MAIN.SuspendLayout()
            TP_VIEW_MODE.SuspendLayout()
            Me.TP_PERIOD.SuspendLayout()
            CType(Me.CMB_REDGIFS_ACC, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.CMB_REDDIT_ACC, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_TEXT.SuspendLayout()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(Me.TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(477, 222)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(477, 222)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            Me.TP_MAIN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            Me.TP_MAIN.ColumnCount = 1
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.Controls.Add(TP_VIEW_MODE, 0, 0)
            Me.TP_MAIN.Controls.Add(Me.TP_PERIOD, 0, 1)
            Me.TP_MAIN.Controls.Add(Me.CMB_REDGIFS_ACC, 0, 4)
            Me.TP_MAIN.Controls.Add(Me.CMB_REDDIT_ACC, 0, 3)
            Me.TP_MAIN.Controls.Add(TP_TEXT, 0, 2)
            Me.TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.TP_MAIN.Name = "TP_MAIN"
            Me.TP_MAIN.RowCount = 6
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.Size = New System.Drawing.Size(477, 222)
            Me.TP_MAIN.TabIndex = 0
            '
            'TP_VIEW_MODE
            '
            TP_VIEW_MODE.ColumnCount = 4
            TP_VIEW_MODE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            TP_VIEW_MODE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            TP_VIEW_MODE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            TP_VIEW_MODE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            TP_VIEW_MODE.Controls.Add(LBL_VIEW_MODE, 0, 0)
            TP_VIEW_MODE.Controls.Add(Me.OPT_VIEW_MODE_NEW, 1, 0)
            TP_VIEW_MODE.Controls.Add(Me.OPT_VIEW_MODE_HOT, 2, 0)
            TP_VIEW_MODE.Controls.Add(Me.OPT_VIEW_MODE_TOP, 3, 0)
            TP_VIEW_MODE.Controls.Add(Me.OPT_VIEW_MODE_BEST, 1, 1)
            TP_VIEW_MODE.Controls.Add(Me.OPT_VIEW_MODE_RISING, 2, 1)
            TP_VIEW_MODE.Dock = System.Windows.Forms.DockStyle.Fill
            TP_VIEW_MODE.Location = New System.Drawing.Point(1, 1)
            TP_VIEW_MODE.Margin = New System.Windows.Forms.Padding(0)
            TP_VIEW_MODE.Name = "TP_VIEW_MODE"
            TP_VIEW_MODE.RowCount = 2
            TP_VIEW_MODE.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_VIEW_MODE.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_VIEW_MODE.Size = New System.Drawing.Size(475, 56)
            TP_VIEW_MODE.TabIndex = 0
            '
            'LBL_VIEW_MODE
            '
            LBL_VIEW_MODE.AutoSize = True
            LBL_VIEW_MODE.Dock = System.Windows.Forms.DockStyle.Fill
            LBL_VIEW_MODE.Location = New System.Drawing.Point(3, 0)
            LBL_VIEW_MODE.Name = "LBL_VIEW_MODE"
            LBL_VIEW_MODE.Size = New System.Drawing.Size(112, 28)
            LBL_VIEW_MODE.TabIndex = 0
            LBL_VIEW_MODE.Text = "View"
            LBL_VIEW_MODE.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'OPT_VIEW_MODE_NEW
            '
            Me.OPT_VIEW_MODE_NEW.AutoSize = True
            Me.OPT_VIEW_MODE_NEW.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_VIEW_MODE_NEW.Location = New System.Drawing.Point(121, 3)
            Me.OPT_VIEW_MODE_NEW.Name = "OPT_VIEW_MODE_NEW"
            Me.OPT_VIEW_MODE_NEW.Size = New System.Drawing.Size(112, 22)
            Me.OPT_VIEW_MODE_NEW.TabIndex = 1
            Me.OPT_VIEW_MODE_NEW.TabStop = True
            Me.OPT_VIEW_MODE_NEW.Text = "New"
            Me.OPT_VIEW_MODE_NEW.UseVisualStyleBackColor = True
            '
            'OPT_VIEW_MODE_HOT
            '
            Me.OPT_VIEW_MODE_HOT.AutoSize = True
            Me.OPT_VIEW_MODE_HOT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_VIEW_MODE_HOT.Location = New System.Drawing.Point(239, 3)
            Me.OPT_VIEW_MODE_HOT.Name = "OPT_VIEW_MODE_HOT"
            Me.OPT_VIEW_MODE_HOT.Size = New System.Drawing.Size(112, 22)
            Me.OPT_VIEW_MODE_HOT.TabIndex = 2
            Me.OPT_VIEW_MODE_HOT.TabStop = True
            Me.OPT_VIEW_MODE_HOT.Text = "Hot"
            Me.OPT_VIEW_MODE_HOT.UseVisualStyleBackColor = True
            '
            'OPT_VIEW_MODE_TOP
            '
            Me.OPT_VIEW_MODE_TOP.AutoSize = True
            Me.OPT_VIEW_MODE_TOP.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_VIEW_MODE_TOP.Location = New System.Drawing.Point(357, 3)
            Me.OPT_VIEW_MODE_TOP.Name = "OPT_VIEW_MODE_TOP"
            Me.OPT_VIEW_MODE_TOP.Size = New System.Drawing.Size(115, 22)
            Me.OPT_VIEW_MODE_TOP.TabIndex = 3
            Me.OPT_VIEW_MODE_TOP.TabStop = True
            Me.OPT_VIEW_MODE_TOP.Text = "Top"
            Me.OPT_VIEW_MODE_TOP.UseVisualStyleBackColor = True
            '
            'OPT_VIEW_MODE_BEST
            '
            Me.OPT_VIEW_MODE_BEST.AutoSize = True
            Me.OPT_VIEW_MODE_BEST.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_VIEW_MODE_BEST.Location = New System.Drawing.Point(121, 31)
            Me.OPT_VIEW_MODE_BEST.Name = "OPT_VIEW_MODE_BEST"
            Me.OPT_VIEW_MODE_BEST.Size = New System.Drawing.Size(112, 22)
            Me.OPT_VIEW_MODE_BEST.TabIndex = 4
            Me.OPT_VIEW_MODE_BEST.TabStop = True
            Me.OPT_VIEW_MODE_BEST.Text = "Best"
            Me.OPT_VIEW_MODE_BEST.UseVisualStyleBackColor = True
            '
            'OPT_VIEW_MODE_RISING
            '
            Me.OPT_VIEW_MODE_RISING.AutoSize = True
            Me.OPT_VIEW_MODE_RISING.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_VIEW_MODE_RISING.Location = New System.Drawing.Point(239, 31)
            Me.OPT_VIEW_MODE_RISING.Name = "OPT_VIEW_MODE_RISING"
            Me.OPT_VIEW_MODE_RISING.Size = New System.Drawing.Size(112, 22)
            Me.OPT_VIEW_MODE_RISING.TabIndex = 5
            Me.OPT_VIEW_MODE_RISING.TabStop = True
            Me.OPT_VIEW_MODE_RISING.Text = "Rising"
            Me.OPT_VIEW_MODE_RISING.UseVisualStyleBackColor = True
            '
            'TP_PERIOD
            '
            Me.TP_PERIOD.ColumnCount = 4
            Me.TP_PERIOD.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            Me.TP_PERIOD.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            Me.TP_PERIOD.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            Me.TP_PERIOD.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            Me.TP_PERIOD.Controls.Add(LBL_PERIOD, 0, 0)
            Me.TP_PERIOD.Controls.Add(Me.OPT_PERIOD_ALL, 1, 0)
            Me.TP_PERIOD.Controls.Add(Me.OPT_PERIOD_HOUR, 2, 0)
            Me.TP_PERIOD.Controls.Add(Me.OPT_PERIOD_DAY, 3, 0)
            Me.TP_PERIOD.Controls.Add(Me.OPT_PERIOD_WEEK, 1, 1)
            Me.TP_PERIOD.Controls.Add(Me.OPT_PERIOD_MONTH, 2, 1)
            Me.TP_PERIOD.Controls.Add(Me.OPT_PERIOD_YEAR, 3, 1)
            Me.TP_PERIOD.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_PERIOD.Location = New System.Drawing.Point(1, 58)
            Me.TP_PERIOD.Margin = New System.Windows.Forms.Padding(0)
            Me.TP_PERIOD.Name = "TP_PERIOD"
            Me.TP_PERIOD.RowCount = 2
            Me.TP_PERIOD.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TP_PERIOD.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TP_PERIOD.Size = New System.Drawing.Size(475, 56)
            Me.TP_PERIOD.TabIndex = 2
            '
            'LBL_PERIOD
            '
            LBL_PERIOD.AutoSize = True
            LBL_PERIOD.Dock = System.Windows.Forms.DockStyle.Fill
            LBL_PERIOD.Location = New System.Drawing.Point(3, 0)
            LBL_PERIOD.Name = "LBL_PERIOD"
            LBL_PERIOD.Size = New System.Drawing.Size(112, 28)
            LBL_PERIOD.TabIndex = 0
            LBL_PERIOD.Text = "Period"
            LBL_PERIOD.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'OPT_PERIOD_ALL
            '
            Me.OPT_PERIOD_ALL.AutoSize = True
            Me.OPT_PERIOD_ALL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_PERIOD_ALL.Location = New System.Drawing.Point(121, 3)
            Me.OPT_PERIOD_ALL.Name = "OPT_PERIOD_ALL"
            Me.OPT_PERIOD_ALL.Size = New System.Drawing.Size(112, 22)
            Me.OPT_PERIOD_ALL.TabIndex = 1
            Me.OPT_PERIOD_ALL.TabStop = True
            Me.OPT_PERIOD_ALL.Text = "All"
            Me.OPT_PERIOD_ALL.UseVisualStyleBackColor = True
            '
            'OPT_PERIOD_HOUR
            '
            Me.OPT_PERIOD_HOUR.AutoSize = True
            Me.OPT_PERIOD_HOUR.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_PERIOD_HOUR.Location = New System.Drawing.Point(239, 3)
            Me.OPT_PERIOD_HOUR.Name = "OPT_PERIOD_HOUR"
            Me.OPT_PERIOD_HOUR.Size = New System.Drawing.Size(112, 22)
            Me.OPT_PERIOD_HOUR.TabIndex = 2
            Me.OPT_PERIOD_HOUR.TabStop = True
            Me.OPT_PERIOD_HOUR.Text = "Hour"
            Me.OPT_PERIOD_HOUR.UseVisualStyleBackColor = True
            '
            'OPT_PERIOD_DAY
            '
            Me.OPT_PERIOD_DAY.AutoSize = True
            Me.OPT_PERIOD_DAY.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_PERIOD_DAY.Location = New System.Drawing.Point(357, 3)
            Me.OPT_PERIOD_DAY.Name = "OPT_PERIOD_DAY"
            Me.OPT_PERIOD_DAY.Size = New System.Drawing.Size(115, 22)
            Me.OPT_PERIOD_DAY.TabIndex = 3
            Me.OPT_PERIOD_DAY.TabStop = True
            Me.OPT_PERIOD_DAY.Text = "Day"
            Me.OPT_PERIOD_DAY.UseVisualStyleBackColor = True
            '
            'OPT_PERIOD_WEEK
            '
            Me.OPT_PERIOD_WEEK.AutoSize = True
            Me.OPT_PERIOD_WEEK.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_PERIOD_WEEK.Location = New System.Drawing.Point(121, 31)
            Me.OPT_PERIOD_WEEK.Name = "OPT_PERIOD_WEEK"
            Me.OPT_PERIOD_WEEK.Size = New System.Drawing.Size(112, 22)
            Me.OPT_PERIOD_WEEK.TabIndex = 4
            Me.OPT_PERIOD_WEEK.TabStop = True
            Me.OPT_PERIOD_WEEK.Text = "Week"
            Me.OPT_PERIOD_WEEK.UseVisualStyleBackColor = True
            '
            'OPT_PERIOD_MONTH
            '
            Me.OPT_PERIOD_MONTH.AutoSize = True
            Me.OPT_PERIOD_MONTH.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_PERIOD_MONTH.Location = New System.Drawing.Point(239, 31)
            Me.OPT_PERIOD_MONTH.Name = "OPT_PERIOD_MONTH"
            Me.OPT_PERIOD_MONTH.Size = New System.Drawing.Size(112, 22)
            Me.OPT_PERIOD_MONTH.TabIndex = 5
            Me.OPT_PERIOD_MONTH.TabStop = True
            Me.OPT_PERIOD_MONTH.Text = "Month"
            Me.OPT_PERIOD_MONTH.UseVisualStyleBackColor = True
            '
            'OPT_PERIOD_YEAR
            '
            Me.OPT_PERIOD_YEAR.AutoSize = True
            Me.OPT_PERIOD_YEAR.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_PERIOD_YEAR.Location = New System.Drawing.Point(357, 31)
            Me.OPT_PERIOD_YEAR.Name = "OPT_PERIOD_YEAR"
            Me.OPT_PERIOD_YEAR.Size = New System.Drawing.Size(115, 22)
            Me.OPT_PERIOD_YEAR.TabIndex = 6
            Me.OPT_PERIOD_YEAR.TabStop = True
            Me.OPT_PERIOD_YEAR.Text = "Year"
            Me.OPT_PERIOD_YEAR.UseVisualStyleBackColor = True
            '
            'CMB_REDGIFS_ACC
            '
            ActionButton3.BackgroundImage = CType(resources.GetObject("ActionButton3.BackgroundImage"), System.Drawing.Image)
            ActionButton3.Name = "ArrowDown"
            ActionButton3.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.ArrowDown
            Me.CMB_REDGIFS_ACC.Buttons.Add(ActionButton3)
            Me.CMB_REDGIFS_ACC.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.Label
            Me.CMB_REDGIFS_ACC.CaptionSizeType = System.Windows.Forms.SizeType.Percent
            Me.CMB_REDGIFS_ACC.CaptionText = "RedGifs account"
            Me.CMB_REDGIFS_ACC.CaptionVisible = True
            Me.CMB_REDGIFS_ACC.CaptionWidth = 26.0R
            Me.CMB_REDGIFS_ACC.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_REDGIFS_ACC.Location = New System.Drawing.Point(4, 173)
            Me.CMB_REDGIFS_ACC.Name = "CMB_REDGIFS_ACC"
            Me.CMB_REDGIFS_ACC.Size = New System.Drawing.Size(469, 22)
            Me.CMB_REDGIFS_ACC.TabIndex = 4
            Me.CMB_REDGIFS_ACC.TextBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            '
            'CMB_REDDIT_ACC
            '
            ActionButton4.BackgroundImage = CType(resources.GetObject("ActionButton4.BackgroundImage"), System.Drawing.Image)
            ActionButton4.Name = "ArrowDown"
            ActionButton4.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.ArrowDown
            Me.CMB_REDDIT_ACC.Buttons.Add(ActionButton4)
            Me.CMB_REDDIT_ACC.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.Label
            Me.CMB_REDDIT_ACC.CaptionSizeType = System.Windows.Forms.SizeType.Percent
            Me.CMB_REDDIT_ACC.CaptionText = "Reddit account"
            Me.CMB_REDDIT_ACC.CaptionVisible = True
            Me.CMB_REDDIT_ACC.CaptionWidth = 26.0R
            Me.CMB_REDDIT_ACC.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_REDDIT_ACC.Location = New System.Drawing.Point(4, 144)
            Me.CMB_REDDIT_ACC.Name = "CMB_REDDIT_ACC"
            Me.CMB_REDDIT_ACC.Size = New System.Drawing.Size(469, 22)
            Me.CMB_REDDIT_ACC.TabIndex = 3
            Me.CMB_REDDIT_ACC.TextBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            '
            'TP_TEXT
            '
            TP_TEXT.ColumnCount = 3
            TP_TEXT.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_TEXT.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_TEXT.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_TEXT.Controls.Add(Me.CH_TXT_DOWN_TXT, 0, 0)
            TP_TEXT.Controls.Add(Me.CH_TXT_DOWN_POSTS, 1, 0)
            TP_TEXT.Controls.Add(Me.CH_TXT_DOWN_SPEC_FOLDER, 2, 0)
            TP_TEXT.Dock = System.Windows.Forms.DockStyle.Fill
            TP_TEXT.Location = New System.Drawing.Point(1, 115)
            TP_TEXT.Margin = New System.Windows.Forms.Padding(0)
            TP_TEXT.Name = "TP_TEXT"
            TP_TEXT.RowCount = 1
            TP_TEXT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_TEXT.Size = New System.Drawing.Size(475, 25)
            TP_TEXT.TabIndex = 5
            '
            'CH_TXT_DOWN_TXT
            '
            Me.CH_TXT_DOWN_TXT.AutoSize = True
            Me.CH_TXT_DOWN_TXT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_TXT_DOWN_TXT.Location = New System.Drawing.Point(3, 3)
            Me.CH_TXT_DOWN_TXT.Name = "CH_TXT_DOWN_TXT"
            Me.CH_TXT_DOWN_TXT.Size = New System.Drawing.Size(152, 19)
            Me.CH_TXT_DOWN_TXT.TabIndex = 0
            Me.CH_TXT_DOWN_TXT.Text = "Download text"
            Me.TT_MAIN.SetToolTip(Me.CH_TXT_DOWN_TXT, "Download text (if available) for posts with image and video")
            Me.CH_TXT_DOWN_TXT.UseVisualStyleBackColor = True
            '
            'CH_TXT_DOWN_POSTS
            '
            Me.CH_TXT_DOWN_POSTS.AutoSize = True
            Me.CH_TXT_DOWN_POSTS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_TXT_DOWN_POSTS.Location = New System.Drawing.Point(161, 3)
            Me.CH_TXT_DOWN_POSTS.Name = "CH_TXT_DOWN_POSTS"
            Me.CH_TXT_DOWN_POSTS.Size = New System.Drawing.Size(152, 19)
            Me.CH_TXT_DOWN_POSTS.TabIndex = 1
            Me.CH_TXT_DOWN_POSTS.Text = "Download text posts"
            Me.TT_MAIN.SetToolTip(Me.CH_TXT_DOWN_POSTS, "Download text (if available) for text posts (no image and video)")
            Me.CH_TXT_DOWN_POSTS.UseVisualStyleBackColor = True
            '
            'CH_TXT_DOWN_SPEC_FOLDER
            '
            Me.CH_TXT_DOWN_SPEC_FOLDER.AutoSize = True
            Me.CH_TXT_DOWN_SPEC_FOLDER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_TXT_DOWN_SPEC_FOLDER.Location = New System.Drawing.Point(319, 3)
            Me.CH_TXT_DOWN_SPEC_FOLDER.Name = "CH_TXT_DOWN_SPEC_FOLDER"
            Me.CH_TXT_DOWN_SPEC_FOLDER.Size = New System.Drawing.Size(153, 19)
            Me.CH_TXT_DOWN_SPEC_FOLDER.TabIndex = 2
            Me.CH_TXT_DOWN_SPEC_FOLDER.Text = "Text special folder"
            Me.TT_MAIN.SetToolTip(Me.CH_TXT_DOWN_SPEC_FOLDER, "If checked, text files will be saved to a separate folder")
            Me.CH_TXT_DOWN_SPEC_FOLDER.UseVisualStyleBackColor = True
            '
            'RedditViewSettingsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(477, 222)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Icon = Global.SCrawler.My.Resources.SiteResources.RedditIcon_128
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(493, 261)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(493, 261)
            Me.Name = "RedditViewSettingsForm"
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Options"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            Me.TP_MAIN.ResumeLayout(False)
            TP_VIEW_MODE.ResumeLayout(False)
            TP_VIEW_MODE.PerformLayout()
            Me.TP_PERIOD.ResumeLayout(False)
            Me.TP_PERIOD.PerformLayout()
            CType(Me.CMB_REDGIFS_ACC, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.CMB_REDDIT_ACC, System.ComponentModel.ISupportInitialize).EndInit()
            TP_TEXT.ResumeLayout(False)
            TP_TEXT.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents OPT_VIEW_MODE_NEW As RadioButton
        Private WithEvents OPT_VIEW_MODE_HOT As RadioButton
        Private WithEvents OPT_VIEW_MODE_TOP As RadioButton
        Private WithEvents OPT_PERIOD_ALL As RadioButton
        Private WithEvents OPT_PERIOD_HOUR As RadioButton
        Private WithEvents OPT_PERIOD_DAY As RadioButton
        Private WithEvents OPT_PERIOD_WEEK As RadioButton
        Private WithEvents OPT_PERIOD_MONTH As RadioButton
        Private WithEvents OPT_PERIOD_YEAR As RadioButton
        Private WithEvents TP_PERIOD As TableLayoutPanel
        Private WithEvents CMB_REDGIFS_ACC As PersonalUtilities.Forms.Controls.ComboBoxExtended
        Private WithEvents CMB_REDDIT_ACC As PersonalUtilities.Forms.Controls.ComboBoxExtended
        Private WithEvents TP_MAIN As TableLayoutPanel
        Private WithEvents OPT_VIEW_MODE_BEST As RadioButton
        Private WithEvents OPT_VIEW_MODE_RISING As RadioButton
        Private WithEvents CH_TXT_DOWN_TXT As CheckBox
        Private WithEvents TT_MAIN As ToolTip
        Private WithEvents CH_TXT_DOWN_POSTS As CheckBox
        Private WithEvents CH_TXT_DOWN_SPEC_FOLDER As CheckBox
    End Class
End Namespace