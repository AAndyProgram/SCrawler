' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace DownloadObjects
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class AutoDownloaderEditorForm : Inherits System.Windows.Forms.Form
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
            Dim TP_MODE As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AutoDownloaderEditorForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim TP_NOTIFY As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton4 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton5 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim TT_MAIN As System.Windows.Forms.ToolTip
            Me.DEF_GROUP = New SCrawler.DownloadObjects.Groups.GroupDefaults()
            Me.OPT_SPEC = New System.Windows.Forms.RadioButton()
            Me.OPT_DISABLED = New System.Windows.Forms.RadioButton()
            Me.OPT_GROUP = New System.Windows.Forms.RadioButton()
            Me.TXT_GROUPS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_NOTIFY = New System.Windows.Forms.CheckBox()
            Me.CH_SHOW_PIC = New System.Windows.Forms.CheckBox()
            Me.CH_SHOW_PIC_USER = New System.Windows.Forms.CheckBox()
            Me.CH_NOTIFY_SIMPLE = New System.Windows.Forms.CheckBox()
            Me.TXT_TIMER = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.NUM_DELAY = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.LBL_LAST_TIME_UP = New System.Windows.Forms.Label()
            Me.CH_MANUAL = New System.Windows.Forms.CheckBox()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MODE = New System.Windows.Forms.TableLayoutPanel()
            TP_NOTIFY = New System.Windows.Forms.TableLayoutPanel()
            TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            Me.DEF_GROUP.SuspendLayout()
            TP_MODE.SuspendLayout()
            CType(Me.TXT_GROUPS, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_NOTIFY.SuspendLayout()
            CType(Me.TXT_TIMER, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.NUM_DELAY, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(Me.DEF_GROUP)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(476, 519)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(476, 519)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'DEF_GROUP
            '
            Me.DEF_GROUP.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            Me.DEF_GROUP.ColumnCount = 1
            Me.DEF_GROUP.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.DEF_GROUP.Controls.Add(TP_MODE, 0, 0)
            Me.DEF_GROUP.Controls.Add(Me.TXT_GROUPS, 0, 12)
            Me.DEF_GROUP.Controls.Add(TP_NOTIFY, 0, 13)
            Me.DEF_GROUP.Controls.Add(Me.TXT_TIMER, 0, 15)
            Me.DEF_GROUP.Controls.Add(Me.NUM_DELAY, 0, 16)
            Me.DEF_GROUP.Controls.Add(Me.LBL_LAST_TIME_UP, 0, 17)
            Me.DEF_GROUP.Controls.Add(Me.CH_MANUAL, 0, 14)
            Me.DEF_GROUP.Dock = System.Windows.Forms.DockStyle.Fill
            Me.DEF_GROUP.Location = New System.Drawing.Point(0, 0)
            Me.DEF_GROUP.Name = "DEF_GROUP"
            Me.DEF_GROUP.RowCount = 19
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEF_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.DEF_GROUP.Size = New System.Drawing.Size(476, 519)
            Me.DEF_GROUP.TabIndex = 0
            '
            'TP_MODE
            '
            TP_MODE.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_MODE.ColumnCount = 3
            TP_MODE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_MODE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_MODE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_MODE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_MODE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_MODE.Controls.Add(Me.OPT_SPEC, 1, 0)
            TP_MODE.Controls.Add(Me.OPT_DISABLED, 0, 0)
            TP_MODE.Controls.Add(Me.OPT_GROUP, 2, 0)
            TP_MODE.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MODE.Location = New System.Drawing.Point(1, 1)
            TP_MODE.Margin = New System.Windows.Forms.Padding(0)
            TP_MODE.Name = "TP_MODE"
            TP_MODE.RowCount = 1
            TP_MODE.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MODE.Size = New System.Drawing.Size(474, 25)
            TP_MODE.TabIndex = 0
            '
            'OPT_SPEC
            '
            Me.OPT_SPEC.AutoSize = True
            Me.OPT_SPEC.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_SPEC.Location = New System.Drawing.Point(161, 4)
            Me.OPT_SPEC.Name = "OPT_SPEC"
            Me.OPT_SPEC.Size = New System.Drawing.Size(150, 17)
            Me.OPT_SPEC.TabIndex = 3
            Me.OPT_SPEC.TabStop = True
            Me.OPT_SPEC.Text = "Specified"
            TT_MAIN.SetToolTip(Me.OPT_SPEC, "Select parameters")
            Me.OPT_SPEC.UseVisualStyleBackColor = True
            '
            'OPT_DISABLED
            '
            Me.OPT_DISABLED.AutoSize = True
            Me.OPT_DISABLED.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_DISABLED.Location = New System.Drawing.Point(4, 4)
            Me.OPT_DISABLED.Name = "OPT_DISABLED"
            Me.OPT_DISABLED.Size = New System.Drawing.Size(150, 17)
            Me.OPT_DISABLED.TabIndex = 0
            Me.OPT_DISABLED.TabStop = True
            Me.OPT_DISABLED.Text = "Disabled"
            TT_MAIN.SetToolTip(Me.OPT_DISABLED, "Automation disabled")
            Me.OPT_DISABLED.UseVisualStyleBackColor = True
            '
            'OPT_GROUP
            '
            Me.OPT_GROUP.AutoSize = True
            Me.OPT_GROUP.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_GROUP.Location = New System.Drawing.Point(318, 4)
            Me.OPT_GROUP.Name = "OPT_GROUP"
            Me.OPT_GROUP.Size = New System.Drawing.Size(152, 17)
            Me.OPT_GROUP.TabIndex = 4
            Me.OPT_GROUP.TabStop = True
            Me.OPT_GROUP.Text = "Groups"
            TT_MAIN.SetToolTip(Me.OPT_GROUP, "Download groups")
            Me.OPT_GROUP.UseVisualStyleBackColor = True
            '
            'TXT_GROUPS
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Name = "Edit"
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Name = "Info"
            ActionButton2.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Info
            ActionButton2.ToolTipText = "Open group"
            ActionButton3.BackgroundImage = CType(resources.GetObject("ActionButton3.BackgroundImage"), System.Drawing.Image)
            ActionButton3.Name = "Clear"
            Me.TXT_GROUPS.Buttons.Add(ActionButton1)
            Me.TXT_GROUPS.Buttons.Add(ActionButton2)
            Me.TXT_GROUPS.Buttons.Add(ActionButton3)
            Me.TXT_GROUPS.CaptionText = "Groups"
            Me.TXT_GROUPS.CaptionWidth = 50.0R
            Me.TXT_GROUPS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_GROUPS.Location = New System.Drawing.Point(4, 331)
            Me.TXT_GROUPS.Name = "TXT_GROUPS"
            Me.TXT_GROUPS.Size = New System.Drawing.Size(468, 22)
            Me.TXT_GROUPS.TabIndex = 1
            Me.TXT_GROUPS.TextBoxReadOnly = True
            '
            'TP_NOTIFY
            '
            TP_NOTIFY.ColumnCount = 4
            TP_NOTIFY.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            TP_NOTIFY.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            TP_NOTIFY.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            TP_NOTIFY.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            TP_NOTIFY.Controls.Add(Me.CH_NOTIFY, 0, 0)
            TP_NOTIFY.Controls.Add(Me.CH_SHOW_PIC, 2, 0)
            TP_NOTIFY.Controls.Add(Me.CH_SHOW_PIC_USER, 3, 0)
            TP_NOTIFY.Controls.Add(Me.CH_NOTIFY_SIMPLE, 1, 0)
            TP_NOTIFY.Dock = System.Windows.Forms.DockStyle.Fill
            TP_NOTIFY.Location = New System.Drawing.Point(1, 357)
            TP_NOTIFY.Margin = New System.Windows.Forms.Padding(0)
            TP_NOTIFY.Name = "TP_NOTIFY"
            TP_NOTIFY.RowCount = 1
            TP_NOTIFY.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_NOTIFY.Size = New System.Drawing.Size(474, 28)
            TP_NOTIFY.TabIndex = 2
            '
            'CH_NOTIFY
            '
            Me.CH_NOTIFY.AutoSize = True
            Me.CH_NOTIFY.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_NOTIFY.Location = New System.Drawing.Point(3, 3)
            Me.CH_NOTIFY.Name = "CH_NOTIFY"
            Me.CH_NOTIFY.Size = New System.Drawing.Size(112, 22)
            Me.CH_NOTIFY.TabIndex = 0
            Me.CH_NOTIFY.Text = "Show notifications"
            TT_MAIN.SetToolTip(Me.CH_NOTIFY, "Show notification when some data has been downloaded")
            Me.CH_NOTIFY.UseVisualStyleBackColor = True
            '
            'CH_SHOW_PIC
            '
            Me.CH_SHOW_PIC.AutoSize = True
            Me.CH_SHOW_PIC.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_SHOW_PIC.Location = New System.Drawing.Point(239, 3)
            Me.CH_SHOW_PIC.Name = "CH_SHOW_PIC"
            Me.CH_SHOW_PIC.Size = New System.Drawing.Size(112, 22)
            Me.CH_SHOW_PIC.TabIndex = 2
            Me.CH_SHOW_PIC.Text = "Image"
            TT_MAIN.SetToolTip(Me.CH_SHOW_PIC, "Show downloaded image in notification")
            Me.CH_SHOW_PIC.UseVisualStyleBackColor = True
            '
            'CH_SHOW_PIC_USER
            '
            Me.CH_SHOW_PIC_USER.AutoSize = True
            Me.CH_SHOW_PIC_USER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_SHOW_PIC_USER.Location = New System.Drawing.Point(357, 3)
            Me.CH_SHOW_PIC_USER.Name = "CH_SHOW_PIC_USER"
            Me.CH_SHOW_PIC_USER.Size = New System.Drawing.Size(114, 22)
            Me.CH_SHOW_PIC_USER.TabIndex = 3
            Me.CH_SHOW_PIC_USER.Text = "User icon"
            TT_MAIN.SetToolTip(Me.CH_SHOW_PIC_USER, "Show user image in notification")
            Me.CH_SHOW_PIC_USER.UseVisualStyleBackColor = True
            '
            'CH_NOTIFY_SIMPLE
            '
            Me.CH_NOTIFY_SIMPLE.AutoSize = True
            Me.CH_NOTIFY_SIMPLE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_NOTIFY_SIMPLE.Location = New System.Drawing.Point(121, 3)
            Me.CH_NOTIFY_SIMPLE.Name = "CH_NOTIFY_SIMPLE"
            Me.CH_NOTIFY_SIMPLE.Size = New System.Drawing.Size(112, 22)
            Me.CH_NOTIFY_SIMPLE.TabIndex = 1
            Me.CH_NOTIFY_SIMPLE.Text = "Simple"
            TT_MAIN.SetToolTip(Me.CH_NOTIFY_SIMPLE, resources.GetString("CH_NOTIFY_SIMPLE.ToolTip"))
            Me.CH_NOTIFY_SIMPLE.UseVisualStyleBackColor = True
            '
            'TXT_TIMER
            '
            ActionButton4.BackgroundImage = CType(resources.GetObject("ActionButton4.BackgroundImage"), System.Drawing.Image)
            ActionButton4.Name = "Refresh"
            Me.TXT_TIMER.Buttons.Add(ActionButton4)
            Me.TXT_TIMER.CaptionText = "Timer"
            Me.TXT_TIMER.CaptionToolTipEnabled = True
            Me.TXT_TIMER.CaptionToolTipText = "Timer (in minutes)"
            Me.TXT_TIMER.CaptionWidth = 50.0R
            Me.TXT_TIMER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_TIMER.Location = New System.Drawing.Point(4, 415)
            Me.TXT_TIMER.Name = "TXT_TIMER"
            Me.TXT_TIMER.Size = New System.Drawing.Size(468, 22)
            Me.TXT_TIMER.TabIndex = 4
            '
            'NUM_DELAY
            '
            ActionButton5.BackgroundImage = CType(resources.GetObject("ActionButton5.BackgroundImage"), System.Drawing.Image)
            ActionButton5.Name = "Refresh"
            Me.NUM_DELAY.Buttons.Add(ActionButton5)
            Me.NUM_DELAY.CaptionText = "Delay"
            Me.NUM_DELAY.CaptionToolTipEnabled = True
            Me.NUM_DELAY.CaptionToolTipText = "Startup delay"
            Me.NUM_DELAY.CaptionWidth = 50.0R
            Me.NUM_DELAY.ClearTextByButtonClear = False
            Me.NUM_DELAY.ControlMode = PersonalUtilities.Forms.Controls.TextBoxExtended.ControlModes.NumericUpDown
            Me.NUM_DELAY.Dock = System.Windows.Forms.DockStyle.Fill
            Me.NUM_DELAY.Location = New System.Drawing.Point(4, 444)
            Me.NUM_DELAY.Name = "NUM_DELAY"
            Me.NUM_DELAY.NumberMaximum = New Decimal(New Integer() {1440, 0, 0, 0})
            Me.NUM_DELAY.NumberUpDownAlign = System.Windows.Forms.LeftRightAlignment.Left
            Me.NUM_DELAY.Size = New System.Drawing.Size(468, 22)
            Me.NUM_DELAY.TabIndex = 5
            Me.NUM_DELAY.Text = "0"
            '
            'LBL_LAST_TIME_UP
            '
            Me.LBL_LAST_TIME_UP.AutoSize = True
            Me.LBL_LAST_TIME_UP.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LBL_LAST_TIME_UP.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
            Me.LBL_LAST_TIME_UP.Location = New System.Drawing.Point(4, 470)
            Me.LBL_LAST_TIME_UP.Name = "LBL_LAST_TIME_UP"
            Me.LBL_LAST_TIME_UP.Size = New System.Drawing.Size(468, 25)
            Me.LBL_LAST_TIME_UP.TabIndex = 6
            Me.LBL_LAST_TIME_UP.Text = "Last download date: "
            Me.LBL_LAST_TIME_UP.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'CH_MANUAL
            '
            Me.CH_MANUAL.AutoSize = True
            Me.CH_MANUAL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_MANUAL.Location = New System.Drawing.Point(4, 389)
            Me.CH_MANUAL.Name = "CH_MANUAL"
            Me.CH_MANUAL.Size = New System.Drawing.Size(468, 19)
            Me.CH_MANUAL.TabIndex = 3
            Me.CH_MANUAL.Text = "Run this task manually"
            TT_MAIN.SetToolTip(Me.CH_MANUAL, "If this checkbox is selected, this task can only be started manually (using the '" &
        "Start (force)' button)")
            Me.CH_MANUAL.UseVisualStyleBackColor = True
            '
            'AutoDownloaderEditorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(476, 519)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Icon = Global.SCrawler.My.Resources.Resources.ArrowDownIcon_Blue_24
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(492, 558)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(492, 558)
            Me.Name = "AutoDownloaderEditorForm"
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "AutoDownloader settings"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            Me.DEF_GROUP.ResumeLayout(False)
            Me.DEF_GROUP.PerformLayout()
            TP_MODE.ResumeLayout(False)
            TP_MODE.PerformLayout()
            CType(Me.TXT_GROUPS, System.ComponentModel.ISupportInitialize).EndInit()
            TP_NOTIFY.ResumeLayout(False)
            TP_NOTIFY.PerformLayout()
            CType(Me.TXT_TIMER, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.NUM_DELAY, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents DEF_GROUP As DownloadObjects.Groups.GroupDefaults
        Private WithEvents TXT_GROUPS As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents OPT_SPEC As RadioButton
        Private WithEvents OPT_DISABLED As RadioButton
        Private WithEvents CH_NOTIFY As CheckBox
        Private WithEvents TXT_TIMER As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents OPT_GROUP As RadioButton
        Private WithEvents LBL_LAST_TIME_UP As Label
        Private WithEvents NUM_DELAY As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CH_SHOW_PIC As CheckBox
        Private WithEvents CH_SHOW_PIC_USER As CheckBox
        Private WithEvents CH_NOTIFY_SIMPLE As CheckBox
        Private WithEvents CH_MANUAL As CheckBox
    End Class
End Namespace