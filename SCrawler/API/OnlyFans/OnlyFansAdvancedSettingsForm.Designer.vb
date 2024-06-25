' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.OnlyFans
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class OnlyFansAdvancedSettingsForm : Inherits System.Windows.Forms.Form
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
            Dim TP_MAIN As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton7 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OnlyFansAdvancedSettingsForm))
            Dim ActionButton8 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton9 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim TP_RULES_LIST As System.Windows.Forms.TableLayoutPanel
            Dim TP_RULES_LIST_LEFT As System.Windows.Forms.TableLayoutPanel
            Dim TT_MAIN As System.Windows.Forms.ToolTip
            Me.TXT_UP_INTERVAL = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_PERSONAL_RULE = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CONTAINER_LIST = New System.Windows.Forms.ToolStripContainer()
            Me.LIST_RULES = New System.Windows.Forms.ListBox()
            Me.OPT_RULES_LIST = New System.Windows.Forms.RadioButton()
            Me.CH_PROTECTED = New System.Windows.Forms.CheckBox()
            Me.CH_FORCE_UPDATE = New System.Windows.Forms.CheckBox()
            Me.CH_LOG_ERR = New System.Windows.Forms.CheckBox()
            Me.CH_RULES_REPLACE_CONFIG = New System.Windows.Forms.CheckBox()
            Me.CH_UPDATE_CONF = New System.Windows.Forms.CheckBox()
            Me.CH_UPDATE_RULES_CONST = New System.Windows.Forms.CheckBox()
            Me.CH_CONFIG_MANUAL_MODE = New System.Windows.Forms.CheckBox()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            TP_RULES_LIST = New System.Windows.Forms.TableLayoutPanel()
            TP_RULES_LIST_LEFT = New System.Windows.Forms.TableLayoutPanel()
            TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            TP_MAIN.SuspendLayout()
            CType(Me.TXT_UP_INTERVAL, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_PERSONAL_RULE, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_RULES_LIST.SuspendLayout()
            Me.CONTAINER_LIST.ContentPanel.SuspendLayout()
            Me.CONTAINER_LIST.SuspendLayout()
            TP_RULES_LIST_LEFT.SuspendLayout()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(464, 341)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(464, 341)
            CONTAINER_MAIN.TabIndex = 1
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            TP_MAIN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(Me.TXT_UP_INTERVAL, 0, 5)
            TP_MAIN.Controls.Add(Me.TXT_PERSONAL_RULE, 0, 6)
            TP_MAIN.Controls.Add(TP_RULES_LIST, 0, 7)
            TP_MAIN.Controls.Add(Me.CH_LOG_ERR, 0, 0)
            TP_MAIN.Controls.Add(Me.CH_RULES_REPLACE_CONFIG, 0, 1)
            TP_MAIN.Controls.Add(Me.CH_UPDATE_CONF, 0, 4)
            TP_MAIN.Controls.Add(Me.CH_UPDATE_RULES_CONST, 0, 2)
            TP_MAIN.Controls.Add(Me.CH_CONFIG_MANUAL_MODE, 0, 3)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 8
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Size = New System.Drawing.Size(464, 341)
            TP_MAIN.TabIndex = 0
            '
            'TXT_UP_INTERVAL
            '
            ActionButton7.BackgroundImage = CType(resources.GetObject("ActionButton7.BackgroundImage"), System.Drawing.Image)
            ActionButton7.Name = "Refresh"
            ActionButton7.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Refresh
            ActionButton8.BackgroundImage = CType(resources.GetObject("ActionButton8.BackgroundImage"), System.Drawing.Image)
            ActionButton8.Name = "Clear"
            ActionButton8.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_UP_INTERVAL.Buttons.Add(ActionButton7)
            Me.TXT_UP_INTERVAL.Buttons.Add(ActionButton8)
            Me.TXT_UP_INTERVAL.CaptionText = "Dynamic rules update"
            Me.TXT_UP_INTERVAL.CaptionToolTipEnabled = True
            Me.TXT_UP_INTERVAL.CaptionToolTipText = "'Dynamic rules' update interval (minutes). Default: 1440"
            Me.TXT_UP_INTERVAL.CaptionWidth = 120.0R
            Me.TXT_UP_INTERVAL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_UP_INTERVAL.Location = New System.Drawing.Point(4, 134)
            Me.TXT_UP_INTERVAL.Name = "TXT_UP_INTERVAL"
            Me.TXT_UP_INTERVAL.Size = New System.Drawing.Size(456, 22)
            Me.TXT_UP_INTERVAL.TabIndex = 5
            '
            'TXT_PERSONAL_RULE
            '
            Me.TXT_PERSONAL_RULE.AutoShowClearButton = True
            ActionButton9.BackgroundImage = CType(resources.GetObject("ActionButton9.BackgroundImage"), System.Drawing.Image)
            ActionButton9.Enabled = False
            ActionButton9.Name = "Clear"
            ActionButton9.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            ActionButton9.Visible = False
            Me.TXT_PERSONAL_RULE.Buttons.Add(ActionButton9)
            Me.TXT_PERSONAL_RULE.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.RadioButton
            Me.TXT_PERSONAL_RULE.CaptionText = "Dynamic rules URL"
            Me.TXT_PERSONAL_RULE.CaptionToolTipEnabled = True
            Me.TXT_PERSONAL_RULE.CaptionToolTipText = "Overwrite 'Dynamic rules' with this URL." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Change this value only if you know what" &
    " you are doing."
            Me.TXT_PERSONAL_RULE.CaptionWidth = 120.0R
            Me.TXT_PERSONAL_RULE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_PERSONAL_RULE.LeaveDefaultButtons = True
            Me.TXT_PERSONAL_RULE.Location = New System.Drawing.Point(4, 163)
            Me.TXT_PERSONAL_RULE.Name = "TXT_PERSONAL_RULE"
            Me.TXT_PERSONAL_RULE.Size = New System.Drawing.Size(456, 22)
            Me.TXT_PERSONAL_RULE.TabIndex = 6
            '
            'TP_RULES_LIST
            '
            TP_RULES_LIST.ColumnCount = 2
            TP_RULES_LIST.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120.0!))
            TP_RULES_LIST.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_RULES_LIST.Controls.Add(Me.CONTAINER_LIST, 1, 0)
            TP_RULES_LIST.Controls.Add(TP_RULES_LIST_LEFT, 0, 0)
            TP_RULES_LIST.Dock = System.Windows.Forms.DockStyle.Fill
            TP_RULES_LIST.Location = New System.Drawing.Point(4, 192)
            TP_RULES_LIST.Name = "TP_RULES_LIST"
            TP_RULES_LIST.RowCount = 1
            TP_RULES_LIST.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_RULES_LIST.Size = New System.Drawing.Size(456, 145)
            TP_RULES_LIST.TabIndex = 7
            '
            'CONTAINER_LIST
            '
            Me.CONTAINER_LIST.BottomToolStripPanelVisible = False
            '
            'CONTAINER_LIST.ContentPanel
            '
            Me.CONTAINER_LIST.ContentPanel.Controls.Add(Me.LIST_RULES)
            Me.CONTAINER_LIST.ContentPanel.Size = New System.Drawing.Size(330, 114)
            Me.CONTAINER_LIST.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CONTAINER_LIST.LeftToolStripPanelVisible = False
            Me.CONTAINER_LIST.Location = New System.Drawing.Point(123, 3)
            Me.CONTAINER_LIST.Name = "CONTAINER_LIST"
            Me.CONTAINER_LIST.RightToolStripPanelVisible = False
            Me.CONTAINER_LIST.Size = New System.Drawing.Size(330, 139)
            Me.CONTAINER_LIST.TabIndex = 1
            '
            'LIST_RULES
            '
            Me.LIST_RULES.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LIST_RULES.FormattingEnabled = True
            Me.LIST_RULES.Location = New System.Drawing.Point(0, 0)
            Me.LIST_RULES.Name = "LIST_RULES"
            Me.LIST_RULES.Size = New System.Drawing.Size(330, 114)
            Me.LIST_RULES.TabIndex = 0
            '
            'TP_RULES_LIST_LEFT
            '
            TP_RULES_LIST_LEFT.ColumnCount = 1
            TP_RULES_LIST_LEFT.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_RULES_LIST_LEFT.Controls.Add(Me.OPT_RULES_LIST, 0, 0)
            TP_RULES_LIST_LEFT.Controls.Add(Me.CH_PROTECTED, 0, 1)
            TP_RULES_LIST_LEFT.Controls.Add(Me.CH_FORCE_UPDATE, 0, 2)
            TP_RULES_LIST_LEFT.Dock = System.Windows.Forms.DockStyle.Fill
            TP_RULES_LIST_LEFT.Location = New System.Drawing.Point(0, 0)
            TP_RULES_LIST_LEFT.Margin = New System.Windows.Forms.Padding(0)
            TP_RULES_LIST_LEFT.Name = "TP_RULES_LIST_LEFT"
            TP_RULES_LIST_LEFT.RowCount = 4
            TP_RULES_LIST_LEFT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_RULES_LIST_LEFT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_RULES_LIST_LEFT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_RULES_LIST_LEFT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_RULES_LIST_LEFT.Size = New System.Drawing.Size(120, 145)
            TP_RULES_LIST_LEFT.TabIndex = 0
            '
            'OPT_RULES_LIST
            '
            Me.OPT_RULES_LIST.AutoSize = True
            Me.OPT_RULES_LIST.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.OPT_RULES_LIST.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_RULES_LIST.Location = New System.Drawing.Point(3, 3)
            Me.OPT_RULES_LIST.Margin = New System.Windows.Forms.Padding(3, 3, 0, 3)
            Me.OPT_RULES_LIST.Name = "OPT_RULES_LIST"
            Me.OPT_RULES_LIST.Size = New System.Drawing.Size(117, 19)
            Me.OPT_RULES_LIST.TabIndex = 0
            Me.OPT_RULES_LIST.TabStop = True
            Me.OPT_RULES_LIST.Text = "Dynamic rules list"
            Me.OPT_RULES_LIST.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            TT_MAIN.SetToolTip(Me.OPT_RULES_LIST, "List of dynamic rules sources." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "If selected, the most recently updated source wil" &
        "l be selected.")
            Me.OPT_RULES_LIST.UseVisualStyleBackColor = True
            '
            'CH_PROTECTED
            '
            Me.CH_PROTECTED.AutoSize = True
            Me.CH_PROTECTED.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.CH_PROTECTED.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_PROTECTED.Location = New System.Drawing.Point(3, 28)
            Me.CH_PROTECTED.Margin = New System.Windows.Forms.Padding(3, 3, 0, 3)
            Me.CH_PROTECTED.Name = "CH_PROTECTED"
            Me.CH_PROTECTED.Size = New System.Drawing.Size(117, 19)
            Me.CH_PROTECTED.TabIndex = 1
            Me.CH_PROTECTED.Text = "Protected list"
            Me.CH_PROTECTED.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            TT_MAIN.SetToolTip(Me.CH_PROTECTED, "If checked, the new source will be added, but the rules list will not be overwrit" &
        "ten by the updated one.")
            Me.CH_PROTECTED.UseVisualStyleBackColor = True
            '
            'CH_FORCE_UPDATE
            '
            Me.CH_FORCE_UPDATE.AutoSize = True
            Me.CH_FORCE_UPDATE.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.CH_FORCE_UPDATE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FORCE_UPDATE.Location = New System.Drawing.Point(3, 53)
            Me.CH_FORCE_UPDATE.Margin = New System.Windows.Forms.Padding(3, 3, 0, 3)
            Me.CH_FORCE_UPDATE.Name = "CH_FORCE_UPDATE"
            Me.CH_FORCE_UPDATE.Size = New System.Drawing.Size(117, 19)
            Me.CH_FORCE_UPDATE.TabIndex = 2
            Me.CH_FORCE_UPDATE.Text = "Force update"
            Me.CH_FORCE_UPDATE.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            TT_MAIN.SetToolTip(Me.CH_FORCE_UPDATE, "Check this if you want to force the rules to update.")
            Me.CH_FORCE_UPDATE.UseVisualStyleBackColor = True
            '
            'CH_LOG_ERR
            '
            Me.CH_LOG_ERR.AutoSize = True
            Me.CH_LOG_ERR.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_LOG_ERR.Location = New System.Drawing.Point(4, 4)
            Me.CH_LOG_ERR.Name = "CH_LOG_ERR"
            Me.CH_LOG_ERR.Size = New System.Drawing.Size(456, 19)
            Me.CH_LOG_ERR.TabIndex = 0
            Me.CH_LOG_ERR.Text = "Add dynamic rules errors to the log"
            TT_MAIN.SetToolTip(Me.CH_LOG_ERR, "OnlyFans errors will be added to a separate log." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "A checked checkbox means that e" &
        "rror notification will be added to the main log.")
            Me.CH_LOG_ERR.UseVisualStyleBackColor = True
            '
            'CH_RULES_REPLACE_CONFIG
            '
            Me.CH_RULES_REPLACE_CONFIG.AutoSize = True
            Me.CH_RULES_REPLACE_CONFIG.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_RULES_REPLACE_CONFIG.Location = New System.Drawing.Point(4, 30)
            Me.CH_RULES_REPLACE_CONFIG.Name = "CH_RULES_REPLACE_CONFIG"
            Me.CH_RULES_REPLACE_CONFIG.Size = New System.Drawing.Size(456, 19)
            Me.CH_RULES_REPLACE_CONFIG.TabIndex = 1
            Me.CH_RULES_REPLACE_CONFIG.Text = "Replace rules in OF-Scraper configuration file"
            TT_MAIN.SetToolTip(Me.CH_RULES_REPLACE_CONFIG, "If checked, the dynamic rules (in the config) will be replaced with actual values" &
        ".")
            Me.CH_RULES_REPLACE_CONFIG.UseVisualStyleBackColor = True
            '
            'CH_UPDATE_CONF
            '
            Me.CH_UPDATE_CONF.AutoSize = True
            Me.CH_UPDATE_CONF.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_UPDATE_CONF.Location = New System.Drawing.Point(4, 108)
            Me.CH_UPDATE_CONF.Name = "CH_UPDATE_CONF"
            Me.CH_UPDATE_CONF.Size = New System.Drawing.Size(456, 19)
            Me.CH_UPDATE_CONF.TabIndex = 4
            Me.CH_UPDATE_CONF.Text = "Update configuration file during update"
            TT_MAIN.SetToolTip(Me.CH_UPDATE_CONF, "Update the configuration pattern from the site during update.")
            Me.CH_UPDATE_CONF.UseVisualStyleBackColor = True
            '
            'CH_UPDATE_RULES_CONST
            '
            Me.CH_UPDATE_RULES_CONST.AutoSize = True
            Me.CH_UPDATE_RULES_CONST.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_UPDATE_RULES_CONST.Location = New System.Drawing.Point(4, 56)
            Me.CH_UPDATE_RULES_CONST.Name = "CH_UPDATE_RULES_CONST"
            Me.CH_UPDATE_RULES_CONST.Size = New System.Drawing.Size(456, 19)
            Me.CH_UPDATE_RULES_CONST.TabIndex = 2
            Me.CH_UPDATE_RULES_CONST.Text = "Update rules constants file during update"
            TT_MAIN.SetToolTip(Me.CH_UPDATE_RULES_CONST, "Update rules constants from the site during update")
            Me.CH_UPDATE_RULES_CONST.UseVisualStyleBackColor = True
            '
            'CH_CONFIG_MANUAL_MODE
            '
            Me.CH_CONFIG_MANUAL_MODE.AutoSize = True
            Me.CH_CONFIG_MANUAL_MODE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_CONFIG_MANUAL_MODE.Location = New System.Drawing.Point(4, 82)
            Me.CH_CONFIG_MANUAL_MODE.Name = "CH_CONFIG_MANUAL_MODE"
            Me.CH_CONFIG_MANUAL_MODE.Size = New System.Drawing.Size(456, 19)
            Me.CH_CONFIG_MANUAL_MODE.TabIndex = 3
            Me.CH_CONFIG_MANUAL_MODE.Text = "Dynamic rules 'Manual' mode"
            TT_MAIN.SetToolTip(Me.CH_CONFIG_MANUAL_MODE, "The rules will be added to the config as is, without using a link.")
            Me.CH_CONFIG_MANUAL_MODE.UseVisualStyleBackColor = True
            '
            'OnlyFansAdvancedSettingsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(464, 341)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.Icon = Global.SCrawler.My.Resources.SiteResources.OnlyFansIcon_32
            Me.KeyPreview = True
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(480, 380)
            Me.Name = "OnlyFansAdvancedSettingsForm"
            Me.ShowInTaskbar = False
            Me.Text = "Settings"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            TP_MAIN.PerformLayout()
            CType(Me.TXT_UP_INTERVAL, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_PERSONAL_RULE, System.ComponentModel.ISupportInitialize).EndInit()
            TP_RULES_LIST.ResumeLayout(False)
            Me.CONTAINER_LIST.ContentPanel.ResumeLayout(False)
            Me.CONTAINER_LIST.ResumeLayout(False)
            Me.CONTAINER_LIST.PerformLayout()
            TP_RULES_LIST_LEFT.ResumeLayout(False)
            TP_RULES_LIST_LEFT.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents TXT_UP_INTERVAL As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_PERSONAL_RULE As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CONTAINER_LIST As ToolStripContainer
        Private WithEvents LIST_RULES As ListBox
        Private WithEvents OPT_RULES_LIST As RadioButton
        Private WithEvents CH_PROTECTED As CheckBox
        Private WithEvents CH_FORCE_UPDATE As CheckBox
        Private WithEvents CH_LOG_ERR As CheckBox
        Private WithEvents CH_RULES_REPLACE_CONFIG As CheckBox
        Private WithEvents CH_UPDATE_CONF As CheckBox
        Private WithEvents CH_UPDATE_RULES_CONST As CheckBox
        Private WithEvents CH_CONFIG_MANUAL_MODE As CheckBox
    End Class
End Namespace