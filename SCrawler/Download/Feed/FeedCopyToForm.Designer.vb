' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace DownloadObjects
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class FeedCopyToForm : Inherits System.Windows.Forms.Form
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
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FeedCopyToForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton4 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim FRM_FILES As System.Windows.Forms.GroupBox
            Dim ActionButton5 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton6 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ListColumn1 As PersonalUtilities.Forms.Controls.Base.ListColumn = New PersonalUtilities.Forms.Controls.Base.ListColumn()
            Dim ListColumn2 As PersonalUtilities.Forms.Controls.Base.ListColumn = New PersonalUtilities.Forms.Controls.Base.ListColumn()
            Dim ActionButton7 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton8 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ListColumn3 As PersonalUtilities.Forms.Controls.Base.ListColumn = New PersonalUtilities.Forms.Controls.Base.ListColumn()
            Dim ListColumn4 As PersonalUtilities.Forms.Controls.Base.ListColumn = New PersonalUtilities.Forms.Controls.Base.ListColumn()
            Dim TP_PROFILES As System.Windows.Forms.TableLayoutPanel
            Dim TT_MAIN As System.Windows.Forms.ToolTip
            Me.CMB_DEST = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.TXT_FILES = New System.Windows.Forms.RichTextBox()
            Me.CH_VIDEO_SEP = New System.Windows.Forms.CheckBox()
            Me.CMB_PROFILE = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.CMB_PROFILE_PATH = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.CH_PROFILE_REPLACE = New System.Windows.Forms.CheckBox()
            Me.CH_PROFILE_CREATE = New System.Windows.Forms.CheckBox()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            FRM_FILES = New System.Windows.Forms.GroupBox()
            TP_PROFILES = New System.Windows.Forms.TableLayoutPanel()
            TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            TP_MAIN.SuspendLayout()
            CType(Me.CMB_DEST, System.ComponentModel.ISupportInitialize).BeginInit()
            FRM_FILES.SuspendLayout()
            CType(Me.CMB_PROFILE, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.CMB_PROFILE_PATH, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_PROFILES.SuspendLayout()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(534, 266)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(534, 266)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(Me.CMB_DEST, 0, 0)
            TP_MAIN.Controls.Add(FRM_FILES, 0, 5)
            TP_MAIN.Controls.Add(Me.CH_VIDEO_SEP, 0, 1)
            TP_MAIN.Controls.Add(Me.CMB_PROFILE, 0, 3)
            TP_MAIN.Controls.Add(Me.CMB_PROFILE_PATH, 0, 4)
            TP_MAIN.Controls.Add(TP_PROFILES, 0, 2)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 6
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Size = New System.Drawing.Size(534, 266)
            TP_MAIN.TabIndex = 0
            '
            'CMB_DEST
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Name = "Open"
            ActionButton1.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Open
            ActionButton1.ToolTipText = "Choose a new location (Ctrl+O)"
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Name = "Add"
            ActionButton2.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Add
            ActionButton2.ToolTipText = "Choose a new location and add it to the list (Alt+O)"
            ActionButton3.BackgroundImage = CType(resources.GetObject("ActionButton3.BackgroundImage"), System.Drawing.Image)
            ActionButton3.Name = "Clear"
            ActionButton3.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            ActionButton4.BackgroundImage = CType(resources.GetObject("ActionButton4.BackgroundImage"), System.Drawing.Image)
            ActionButton4.Name = "ArrowDown"
            ActionButton4.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.ArrowDown
            Me.CMB_DEST.Buttons.Add(ActionButton1)
            Me.CMB_DEST.Buttons.Add(ActionButton2)
            Me.CMB_DEST.Buttons.Add(ActionButton3)
            Me.CMB_DEST.Buttons.Add(ActionButton4)
            Me.CMB_DEST.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
            Me.CMB_DEST.CaptionText = "Destination"
            Me.CMB_DEST.CaptionToolTipEnabled = True
            Me.CMB_DEST.CaptionToolTipText = "If checked, the profile path will be ignored"
            Me.CMB_DEST.CaptionVisible = True
            Me.CMB_DEST.CaptionWidth = 90.0R
            Me.CMB_DEST.ChangeControlsEnableOnCheckedChange = False
            Me.CMB_DEST.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_DEST.Location = New System.Drawing.Point(3, 3)
            Me.CMB_DEST.Name = "CMB_DEST"
            Me.CMB_DEST.Size = New System.Drawing.Size(528, 22)
            Me.CMB_DEST.TabIndex = 0
            Me.CMB_DEST.TextBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            '
            'FRM_FILES
            '
            FRM_FILES.Controls.Add(Me.TXT_FILES)
            FRM_FILES.Dock = System.Windows.Forms.DockStyle.Fill
            FRM_FILES.Location = New System.Drawing.Point(3, 137)
            FRM_FILES.Name = "FRM_FILES"
            FRM_FILES.Size = New System.Drawing.Size(528, 126)
            FRM_FILES.TabIndex = 5
            FRM_FILES.TabStop = False
            FRM_FILES.Text = "Files:"
            '
            'TXT_FILES
            '
            Me.TXT_FILES.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_FILES.Location = New System.Drawing.Point(3, 16)
            Me.TXT_FILES.Name = "TXT_FILES"
            Me.TXT_FILES.ReadOnly = True
            Me.TXT_FILES.Size = New System.Drawing.Size(522, 107)
            Me.TXT_FILES.TabIndex = 0
            Me.TXT_FILES.Text = ""
            '
            'CH_VIDEO_SEP
            '
            Me.CH_VIDEO_SEP.AutoSize = True
            Me.CH_VIDEO_SEP.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_VIDEO_SEP.Location = New System.Drawing.Point(3, 31)
            Me.CH_VIDEO_SEP.Name = "CH_VIDEO_SEP"
            Me.CH_VIDEO_SEP.Size = New System.Drawing.Size(528, 19)
            Me.CH_VIDEO_SEP.TabIndex = 1
            Me.CH_VIDEO_SEP.Text = "Place videos in a separate video folder"
            Me.CH_VIDEO_SEP.UseVisualStyleBackColor = True
            '
            'CMB_PROFILE
            '
            ActionButton5.BackgroundImage = CType(resources.GetObject("ActionButton5.BackgroundImage"), System.Drawing.Image)
            ActionButton5.Enabled = False
            ActionButton5.Name = "Clear"
            ActionButton5.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            ActionButton6.BackgroundImage = CType(resources.GetObject("ActionButton6.BackgroundImage"), System.Drawing.Image)
            ActionButton6.Enabled = False
            ActionButton6.Name = "ArrowDown"
            ActionButton6.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.ArrowDown
            Me.CMB_PROFILE.Buttons.Add(ActionButton5)
            Me.CMB_PROFILE.Buttons.Add(ActionButton6)
            Me.CMB_PROFILE.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.RadioButton
            Me.CMB_PROFILE.CaptionPadding = New System.Windows.Forms.Padding(0, 0, 1, 0)
            Me.CMB_PROFILE.CaptionText = "Profile"
            Me.CMB_PROFILE.CaptionVisible = True
            ListColumn1.DisplayMember = True
            ListColumn1.Name = "COL_NAME"
            ListColumn1.Text = "Name"
            ListColumn1.Width = -1
            ListColumn2.Name = "COL_VALUE"
            ListColumn2.Text = "Value"
            ListColumn2.ValueMember = True
            ListColumn2.Visible = False
            Me.CMB_PROFILE.Columns.Add(ListColumn1)
            Me.CMB_PROFILE.Columns.Add(ListColumn2)
            Me.CMB_PROFILE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_PROFILE.Location = New System.Drawing.Point(3, 81)
            Me.CMB_PROFILE.Name = "CMB_PROFILE"
            Me.CMB_PROFILE.Size = New System.Drawing.Size(528, 22)
            Me.CMB_PROFILE.TabIndex = 3
            Me.CMB_PROFILE.TextBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            '
            'CMB_PROFILE_PATH
            '
            ActionButton7.BackgroundImage = CType(resources.GetObject("ActionButton7.BackgroundImage"), System.Drawing.Image)
            ActionButton7.Enabled = False
            ActionButton7.Name = "Clear"
            ActionButton7.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            ActionButton8.BackgroundImage = CType(resources.GetObject("ActionButton8.BackgroundImage"), System.Drawing.Image)
            ActionButton8.Enabled = False
            ActionButton8.Name = "ArrowDown"
            ActionButton8.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.ArrowDown
            Me.CMB_PROFILE_PATH.Buttons.Add(ActionButton7)
            Me.CMB_PROFILE_PATH.Buttons.Add(ActionButton8)
            Me.CMB_PROFILE_PATH.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.RadioButton
            Me.CMB_PROFILE_PATH.CaptionPadding = New System.Windows.Forms.Padding(0, 0, 1, 0)
            Me.CMB_PROFILE_PATH.CaptionText = "Profile (path)"
            Me.CMB_PROFILE_PATH.CaptionVisible = True
            ListColumn3.DisplayMember = True
            ListColumn3.Name = "COL_NAME"
            ListColumn3.Text = "Name"
            ListColumn3.Width = -1
            ListColumn4.Name = "COL_VALUE"
            ListColumn4.Text = "Value"
            ListColumn4.ValueMember = True
            ListColumn4.Visible = False
            Me.CMB_PROFILE_PATH.Columns.Add(ListColumn3)
            Me.CMB_PROFILE_PATH.Columns.Add(ListColumn4)
            Me.CMB_PROFILE_PATH.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_PROFILE_PATH.Location = New System.Drawing.Point(3, 109)
            Me.CMB_PROFILE_PATH.Name = "CMB_PROFILE_PATH"
            Me.CMB_PROFILE_PATH.Size = New System.Drawing.Size(528, 22)
            Me.CMB_PROFILE_PATH.TabIndex = 4
            Me.CMB_PROFILE_PATH.TextBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            '
            'TP_PROFILES
            '
            TP_PROFILES.ColumnCount = 2
            TP_PROFILES.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_PROFILES.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_PROFILES.Controls.Add(Me.CH_PROFILE_REPLACE, 0, 0)
            TP_PROFILES.Controls.Add(Me.CH_PROFILE_CREATE, 1, 0)
            TP_PROFILES.Dock = System.Windows.Forms.DockStyle.Fill
            TP_PROFILES.Location = New System.Drawing.Point(0, 53)
            TP_PROFILES.Margin = New System.Windows.Forms.Padding(0)
            TP_PROFILES.Name = "TP_PROFILES"
            TP_PROFILES.RowCount = 1
            TP_PROFILES.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_PROFILES.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_PROFILES.Size = New System.Drawing.Size(534, 25)
            TP_PROFILES.TabIndex = 2
            '
            'CH_PROFILE_REPLACE
            '
            Me.CH_PROFILE_REPLACE.AutoSize = True
            Me.CH_PROFILE_REPLACE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_PROFILE_REPLACE.Location = New System.Drawing.Point(3, 3)
            Me.CH_PROFILE_REPLACE.Name = "CH_PROFILE_REPLACE"
            Me.CH_PROFILE_REPLACE.Size = New System.Drawing.Size(261, 19)
            Me.CH_PROFILE_REPLACE.TabIndex = 0
            Me.CH_PROFILE_REPLACE.Text = "Replace user profile"
            TT_MAIN.SetToolTip(Me.CH_PROFILE_REPLACE, "The user profile will be replaced with the selected one")
            Me.CH_PROFILE_REPLACE.UseVisualStyleBackColor = True
            '
            'CH_PROFILE_CREATE
            '
            Me.CH_PROFILE_CREATE.AutoSize = True
            Me.CH_PROFILE_CREATE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_PROFILE_CREATE.Location = New System.Drawing.Point(270, 3)
            Me.CH_PROFILE_CREATE.Name = "CH_PROFILE_CREATE"
            Me.CH_PROFILE_CREATE.Size = New System.Drawing.Size(261, 19)
            Me.CH_PROFILE_CREATE.TabIndex = 1
            Me.CH_PROFILE_CREATE.Text = "Create path profile"
            TT_MAIN.SetToolTip(Me.CH_PROFILE_CREATE, "Create a path profile if it doesn't exist")
            Me.CH_PROFILE_CREATE.UseVisualStyleBackColor = True
            '
            'FeedCopyToForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(534, 266)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.KeyPreview = True
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(550, 305)
            Me.Name = "FeedCopyToForm"
            Me.ShowInTaskbar = False
            Me.Text = "Copy to..."
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            TP_MAIN.PerformLayout()
            CType(Me.CMB_DEST, System.ComponentModel.ISupportInitialize).EndInit()
            FRM_FILES.ResumeLayout(False)
            CType(Me.CMB_PROFILE, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.CMB_PROFILE_PATH, System.ComponentModel.ISupportInitialize).EndInit()
            TP_PROFILES.ResumeLayout(False)
            TP_PROFILES.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents CMB_DEST As PersonalUtilities.Forms.Controls.ComboBoxExtended
        Private WithEvents TXT_FILES As RichTextBox
        Private WithEvents CH_VIDEO_SEP As CheckBox
        Private WithEvents CMB_PROFILE As PersonalUtilities.Forms.Controls.ComboBoxExtended
        Private WithEvents CMB_PROFILE_PATH As PersonalUtilities.Forms.Controls.ComboBoxExtended
        Private WithEvents CH_PROFILE_REPLACE As CheckBox
        Private WithEvents CH_PROFILE_CREATE As CheckBox
    End Class
End Namespace