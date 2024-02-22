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
            Dim CONTAINER_MAIN As System.Windows.Forms.ToolStripContainer
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FeedCopyToForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton4 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim TP_MAIN As System.Windows.Forms.TableLayoutPanel
            Dim FRM_FILES As System.Windows.Forms.GroupBox
            Me.CMB_DEST = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.TXT_FILES = New System.Windows.Forms.RichTextBox()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            FRM_FILES = New System.Windows.Forms.GroupBox()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            CType(Me.CMB_DEST, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_MAIN.SuspendLayout()
            FRM_FILES.SuspendLayout()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(534, 116)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(534, 141)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
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
            Me.CMB_DEST.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.Label
            Me.CMB_DEST.CaptionText = "Destination:"
            Me.CMB_DEST.CaptionVisible = True
            Me.CMB_DEST.CaptionWidth = 70.0R
            Me.CMB_DEST.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_DEST.Location = New System.Drawing.Point(3, 3)
            Me.CMB_DEST.Name = "CMB_DEST"
            Me.CMB_DEST.Size = New System.Drawing.Size(528, 22)
            Me.CMB_DEST.TabIndex = 0
            Me.CMB_DEST.TextBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            '
            'TP_MAIN
            '
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(Me.CMB_DEST, 0, 0)
            TP_MAIN.Controls.Add(FRM_FILES, 0, 1)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 2
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_MAIN.Size = New System.Drawing.Size(534, 116)
            TP_MAIN.TabIndex = 0
            '
            'FRM_FILES
            '
            FRM_FILES.Controls.Add(Me.TXT_FILES)
            FRM_FILES.Dock = System.Windows.Forms.DockStyle.Fill
            FRM_FILES.Location = New System.Drawing.Point(3, 31)
            FRM_FILES.Name = "FRM_FILES"
            FRM_FILES.Size = New System.Drawing.Size(528, 82)
            FRM_FILES.TabIndex = 1
            FRM_FILES.TabStop = False
            FRM_FILES.Text = "Files:"
            '
            'TXT_FILES
            '
            Me.TXT_FILES.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_FILES.Location = New System.Drawing.Point(3, 16)
            Me.TXT_FILES.Name = "TXT_FILES"
            Me.TXT_FILES.ReadOnly = True
            Me.TXT_FILES.Size = New System.Drawing.Size(522, 63)
            Me.TXT_FILES.TabIndex = 0
            Me.TXT_FILES.Text = ""
            '
            'FeedCopyToForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(534, 141)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.KeyPreview = True
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(550, 150)
            Me.Name = "FeedCopyToForm"
            Me.ShowInTaskbar = False
            Me.Text = "Copy to..."
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            CType(Me.CMB_DEST, System.ComponentModel.ISupportInitialize).EndInit()
            TP_MAIN.ResumeLayout(False)
            FRM_FILES.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents CMB_DEST As PersonalUtilities.Forms.Controls.ComboBoxExtended
        Private WithEvents TXT_FILES As RichTextBox
    End Class
End Namespace