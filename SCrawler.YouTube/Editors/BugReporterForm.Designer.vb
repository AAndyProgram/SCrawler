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
    Partial Public Class BugReporterForm : Inherits System.Windows.Forms.Form
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
            Dim TP_BUTTONS As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(BugReporterForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton4 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton5 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton6 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton7 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton8 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton9 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Me.BTT_EMAIL = New System.Windows.Forms.Button()
            Me.BTT_GITHUB = New System.Windows.Forms.Button()
            Me.BTT_COPY = New System.Windows.Forms.Button()
            Me.BTT_CANCEL = New System.Windows.Forms.Button()
            Me.BTT_ANON = New System.Windows.Forms.Button()
            Me.TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
            Me.TXT_DESCR = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_URL_PROFILE = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_URL_POST = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_REPRODUCE = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_EXPECT = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_LOG = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_FILES = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            TP_BUTTONS = New System.Windows.Forms.TableLayoutPanel()
            TP_MAIN.SuspendLayout()
            TP_BUTTONS.SuspendLayout()
            CType(Me.TXT_DESCR, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_URL_PROFILE, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_URL_POST, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_REPRODUCE, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_EXPECT, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_LOG, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_FILES, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'TP_MAIN
            '
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(Me.TXT_DESCR, 0, 0)
            TP_MAIN.Controls.Add(Me.TXT_URL_PROFILE, 0, 1)
            TP_MAIN.Controls.Add(Me.TXT_URL_POST, 0, 2)
            TP_MAIN.Controls.Add(Me.TXT_REPRODUCE, 0, 3)
            TP_MAIN.Controls.Add(Me.TXT_EXPECT, 0, 4)
            TP_MAIN.Controls.Add(Me.TXT_LOG, 0, 5)
            TP_MAIN.Controls.Add(TP_BUTTONS, 0, 7)
            TP_MAIN.Controls.Add(Me.TXT_FILES, 0, 6)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 8
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            TP_MAIN.Size = New System.Drawing.Size(584, 461)
            TP_MAIN.TabIndex = 0
            '
            'TP_BUTTONS
            '
            TP_BUTTONS.ColumnCount = 6
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_BUTTONS.Controls.Add(Me.BTT_EMAIL, 2, 0)
            TP_BUTTONS.Controls.Add(Me.BTT_GITHUB, 3, 0)
            TP_BUTTONS.Controls.Add(Me.BTT_COPY, 4, 0)
            TP_BUTTONS.Controls.Add(Me.BTT_CANCEL, 5, 0)
            TP_BUTTONS.Controls.Add(Me.BTT_ANON, 1, 0)
            TP_BUTTONS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_BUTTONS.Location = New System.Drawing.Point(0, 431)
            TP_BUTTONS.Margin = New System.Windows.Forms.Padding(0)
            TP_BUTTONS.Name = "TP_BUTTONS"
            TP_BUTTONS.RowCount = 1
            TP_BUTTONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_BUTTONS.Size = New System.Drawing.Size(584, 30)
            TP_BUTTONS.TabIndex = 7
            '
            'BTT_EMAIL
            '
            Me.BTT_EMAIL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_EMAIL.Location = New System.Drawing.Point(187, 3)
            Me.BTT_EMAIL.Name = "BTT_EMAIL"
            Me.BTT_EMAIL.Size = New System.Drawing.Size(94, 24)
            Me.BTT_EMAIL.TabIndex = 1
            Me.BTT_EMAIL.Text = "email"
            Me.TT_MAIN.SetToolTip(Me.BTT_EMAIL, "Create a message to send via email.")
            Me.BTT_EMAIL.UseVisualStyleBackColor = True
            '
            'BTT_GITHUB
            '
            Me.BTT_GITHUB.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_GITHUB.Location = New System.Drawing.Point(287, 3)
            Me.BTT_GITHUB.Name = "BTT_GITHUB"
            Me.BTT_GITHUB.Size = New System.Drawing.Size(94, 24)
            Me.BTT_GITHUB.TabIndex = 2
            Me.BTT_GITHUB.Text = "GitHub"
            Me.TT_MAIN.SetToolTip(Me.BTT_GITHUB, "Create a MarkDown message to post to GitHub.")
            Me.BTT_GITHUB.UseVisualStyleBackColor = True
            '
            'BTT_COPY
            '
            Me.BTT_COPY.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_COPY.Location = New System.Drawing.Point(387, 3)
            Me.BTT_COPY.Name = "BTT_COPY"
            Me.BTT_COPY.Size = New System.Drawing.Size(94, 24)
            Me.BTT_COPY.TabIndex = 3
            Me.BTT_COPY.Text = "Copy"
            Me.TT_MAIN.SetToolTip(Me.BTT_COPY, "Create a message and copy to your clipboard.")
            Me.BTT_COPY.UseVisualStyleBackColor = True
            '
            'BTT_CANCEL
            '
            Me.BTT_CANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.BTT_CANCEL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_CANCEL.Location = New System.Drawing.Point(487, 3)
            Me.BTT_CANCEL.Name = "BTT_CANCEL"
            Me.BTT_CANCEL.Size = New System.Drawing.Size(94, 24)
            Me.BTT_CANCEL.TabIndex = 4
            Me.BTT_CANCEL.Text = "Cancel"
            Me.BTT_CANCEL.UseVisualStyleBackColor = True
            '
            'BTT_ANON
            '
            Me.BTT_ANON.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_ANON.Location = New System.Drawing.Point(87, 3)
            Me.BTT_ANON.Name = "BTT_ANON"
            Me.BTT_ANON.Size = New System.Drawing.Size(94, 24)
            Me.BTT_ANON.TabIndex = 0
            Me.BTT_ANON.Text = "Anon message"
            Me.TT_MAIN.SetToolTip(Me.BTT_ANON, resources.GetString("BTT_ANON.ToolTip"))
            Me.BTT_ANON.UseVisualStyleBackColor = True
            '
            'TXT_DESCR
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Dock = System.Windows.Forms.DockStyle.Top
            ActionButton1.Name = "Clear"
            ActionButton1.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_DESCR.Buttons.Add(ActionButton1)
            Me.TXT_DESCR.CaptionDock = System.Windows.Forms.DockStyle.Top
            Me.TXT_DESCR.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.None
            Me.TXT_DESCR.CaptionVisible = False
            Me.TXT_DESCR.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_DESCR.GroupBoxed = True
            Me.TXT_DESCR.GroupBoxText = "Describe the bug or write your message"
            Me.TXT_DESCR.Lines = New String(-1) {}
            Me.TXT_DESCR.Location = New System.Drawing.Point(3, 3)
            Me.TXT_DESCR.Multiline = True
            Me.TXT_DESCR.Name = "TXT_DESCR"
            Me.TXT_DESCR.Size = New System.Drawing.Size(578, 69)
            Me.TXT_DESCR.TabIndex = 0
            Me.TXT_DESCR.TextToolTip = "A clear and concise description of what the bug is"
            Me.TXT_DESCR.TextToolTipEnabled = True
            '
            'TXT_URL_PROFILE
            '
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Name = "Clear"
            ActionButton2.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_URL_PROFILE.Buttons.Add(ActionButton2)
            Me.TXT_URL_PROFILE.CaptionText = "Profile URL"
            Me.TXT_URL_PROFILE.CaptionWidth = 75.0R
            Me.TXT_URL_PROFILE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_URL_PROFILE.Lines = New String(-1) {}
            Me.TXT_URL_PROFILE.Location = New System.Drawing.Point(3, 78)
            Me.TXT_URL_PROFILE.Name = "TXT_URL_PROFILE"
            Me.TXT_URL_PROFILE.Size = New System.Drawing.Size(578, 22)
            Me.TXT_URL_PROFILE.TabIndex = 1
            '
            'TXT_URL_POST
            '
            ActionButton3.BackgroundImage = CType(resources.GetObject("ActionButton3.BackgroundImage"), System.Drawing.Image)
            ActionButton3.Name = "Clear"
            ActionButton3.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_URL_POST.Buttons.Add(ActionButton3)
            Me.TXT_URL_POST.CaptionText = "Post URL"
            Me.TXT_URL_POST.CaptionWidth = 75.0R
            Me.TXT_URL_POST.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_URL_POST.Lines = New String(-1) {}
            Me.TXT_URL_POST.Location = New System.Drawing.Point(3, 106)
            Me.TXT_URL_POST.Name = "TXT_URL_POST"
            Me.TXT_URL_POST.Size = New System.Drawing.Size(578, 22)
            Me.TXT_URL_POST.TabIndex = 2
            '
            'TXT_REPRODUCE
            '
            ActionButton4.BackgroundImage = CType(resources.GetObject("ActionButton4.BackgroundImage"), System.Drawing.Image)
            ActionButton4.Dock = System.Windows.Forms.DockStyle.Top
            ActionButton4.Name = "Clear"
            ActionButton4.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_REPRODUCE.Buttons.Add(ActionButton4)
            Me.TXT_REPRODUCE.CaptionDock = System.Windows.Forms.DockStyle.Top
            Me.TXT_REPRODUCE.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.None
            Me.TXT_REPRODUCE.CaptionVisible = False
            Me.TXT_REPRODUCE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_REPRODUCE.GroupBoxed = True
            Me.TXT_REPRODUCE.GroupBoxText = "To Reproduce"
            Me.TXT_REPRODUCE.Lines = New String(-1) {}
            Me.TXT_REPRODUCE.Location = New System.Drawing.Point(3, 134)
            Me.TXT_REPRODUCE.Multiline = True
            Me.TXT_REPRODUCE.Name = "TXT_REPRODUCE"
            Me.TXT_REPRODUCE.Size = New System.Drawing.Size(578, 69)
            Me.TXT_REPRODUCE.TabIndex = 3
            Me.TXT_REPRODUCE.TextToolTip = "Steps to reproduce the behavior:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "1. Do something" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "2. See error"
            Me.TXT_REPRODUCE.TextToolTipEnabled = True
            '
            'TXT_EXPECT
            '
            ActionButton5.BackgroundImage = CType(resources.GetObject("ActionButton5.BackgroundImage"), System.Drawing.Image)
            ActionButton5.Dock = System.Windows.Forms.DockStyle.Top
            ActionButton5.Name = "Clear"
            ActionButton5.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_EXPECT.Buttons.Add(ActionButton5)
            Me.TXT_EXPECT.CaptionDock = System.Windows.Forms.DockStyle.Top
            Me.TXT_EXPECT.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.None
            Me.TXT_EXPECT.CaptionVisible = False
            Me.TXT_EXPECT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_EXPECT.GroupBoxed = True
            Me.TXT_EXPECT.GroupBoxText = "Expected behavior"
            Me.TXT_EXPECT.Lines = New String(-1) {}
            Me.TXT_EXPECT.Location = New System.Drawing.Point(3, 209)
            Me.TXT_EXPECT.Multiline = True
            Me.TXT_EXPECT.Name = "TXT_EXPECT"
            Me.TXT_EXPECT.Size = New System.Drawing.Size(578, 69)
            Me.TXT_EXPECT.TabIndex = 4
            Me.TXT_EXPECT.TextToolTip = "A clear and concise description of what you expected to happen."
            Me.TXT_EXPECT.TextToolTipEnabled = True
            '
            'TXT_LOG
            '
            ActionButton6.BackgroundImage = CType(resources.GetObject("ActionButton6.BackgroundImage"), System.Drawing.Image)
            ActionButton6.Dock = System.Windows.Forms.DockStyle.Top
            ActionButton6.Name = "Open"
            ActionButton6.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Open
            ActionButton6.ToolTipText = "Select log files to add their text to the message"
            ActionButton7.BackgroundImage = CType(resources.GetObject("ActionButton7.BackgroundImage"), System.Drawing.Image)
            ActionButton7.Dock = System.Windows.Forms.DockStyle.Top
            ActionButton7.Name = "Clear"
            ActionButton7.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            ActionButton7.ToolTipText = "Empty"
            Me.TXT_LOG.Buttons.Add(ActionButton6)
            Me.TXT_LOG.Buttons.Add(ActionButton7)
            Me.TXT_LOG.CaptionDock = System.Windows.Forms.DockStyle.Top
            Me.TXT_LOG.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.None
            Me.TXT_LOG.CaptionVisible = False
            Me.TXT_LOG.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_LOG.GroupBoxed = True
            Me.TXT_LOG.GroupBoxText = "Log data"
            Me.TXT_LOG.Lines = New String(-1) {}
            Me.TXT_LOG.Location = New System.Drawing.Point(3, 284)
            Me.TXT_LOG.Multiline = True
            Me.TXT_LOG.Name = "TXT_LOG"
            Me.TXT_LOG.Size = New System.Drawing.Size(578, 69)
            Me.TXT_LOG.TabIndex = 5
            '
            'TXT_FILES
            '
            ActionButton8.BackgroundImage = CType(resources.GetObject("ActionButton8.BackgroundImage"), System.Drawing.Image)
            ActionButton8.Dock = System.Windows.Forms.DockStyle.Top
            ActionButton8.Name = "Add"
            ActionButton8.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Add
            ActionButton8.ToolTipText = "Add files"
            ActionButton9.BackgroundImage = CType(resources.GetObject("ActionButton9.BackgroundImage"), System.Drawing.Image)
            ActionButton9.Dock = System.Windows.Forms.DockStyle.Top
            ActionButton9.Name = "Clear"
            ActionButton9.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            ActionButton9.ToolTipText = "Clear files"
            Me.TXT_FILES.Buttons.Add(ActionButton8)
            Me.TXT_FILES.Buttons.Add(ActionButton9)
            Me.TXT_FILES.CaptionDock = System.Windows.Forms.DockStyle.Top
            Me.TXT_FILES.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.None
            Me.TXT_FILES.CaptionVisible = False
            Me.TXT_FILES.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_FILES.GroupBoxed = True
            Me.TXT_FILES.GroupBoxText = "Files"
            Me.TXT_FILES.Lines = New String(-1) {}
            Me.TXT_FILES.Location = New System.Drawing.Point(3, 359)
            Me.TXT_FILES.Multiline = True
            Me.TXT_FILES.Name = "TXT_FILES"
            Me.TXT_FILES.Size = New System.Drawing.Size(578, 69)
            Me.TXT_FILES.TabIndex = 6
            Me.TXT_FILES.TextBoxReadOnly = True
            Me.TXT_FILES.TextToolTip = "Attach files to your message (only works with anonymous message)"
            Me.TXT_FILES.TextToolTipEnabled = True
            '
            'BugReporterForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.BTT_CANCEL
            Me.ClientSize = New System.Drawing.Size(584, 461)
            Me.Controls.Add(TP_MAIN)
            Me.KeyPreview = True
            Me.MinimumSize = New System.Drawing.Size(600, 500)
            Me.Name = "BugReporterForm"
            Me.Text = "New message"
            TP_MAIN.ResumeLayout(False)
            TP_BUTTONS.ResumeLayout(False)
            CType(Me.TXT_DESCR, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_URL_PROFILE, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_URL_POST, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_REPRODUCE, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_EXPECT, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_LOG, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_FILES, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents TXT_DESCR As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_URL_PROFILE As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_URL_POST As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_REPRODUCE As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_EXPECT As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_LOG As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TT_MAIN As ToolTip
        Private WithEvents BTT_EMAIL As Button
        Private WithEvents BTT_GITHUB As Button
        Private WithEvents BTT_COPY As Button
        Private WithEvents BTT_CANCEL As Button
        Private WithEvents BTT_ANON As Button
        Private WithEvents TXT_FILES As PersonalUtilities.Forms.Controls.TextBoxExtended
    End Class
End Namespace