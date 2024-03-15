' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace Editors
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class ColorPickerInternalForm : Inherits System.Windows.Forms.Form
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
            Dim TP_MAIN As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ColorPickerInternalForm))
            Me.TXT_NAME = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.COLOR_PICKER = New SCrawler.Editors.ColorPicker()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            TP_MAIN.SuspendLayout()
            CType(Me.TXT_NAME, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(444, 81)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(444, 81)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            TP_MAIN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(Me.TXT_NAME, 0, 0)
            TP_MAIN.Controls.Add(Me.COLOR_PICKER, 0, 1)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 3
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle())
            TP_MAIN.Size = New System.Drawing.Size(444, 81)
            TP_MAIN.TabIndex = 0
            '
            'TXT_NAME
            '
            Me.TXT_NAME.AutoShowClearButton = True
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Name = "Clear"
            ActionButton1.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            ActionButton1.Visible = False
            Me.TXT_NAME.Buttons.Add(ActionButton1)
            Me.TXT_NAME.CaptionText = "Name"
            Me.TXT_NAME.CaptionWidth = 50.0R
            Me.TXT_NAME.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_NAME.LeaveDefaultButtons = True
            Me.TXT_NAME.Location = New System.Drawing.Point(4, 4)
            Me.TXT_NAME.Name = "TXT_NAME"
            Me.TXT_NAME.Size = New System.Drawing.Size(436, 22)
            Me.TXT_NAME.TabIndex = 0
            '
            'COLOR_PICKER
            '
            Me.COLOR_PICKER.ButtonsMargin = New System.Windows.Forms.Padding(1, 2, 1, 2)
            Me.COLOR_PICKER.CaptionText = "Color"
            Me.COLOR_PICKER.CaptionWidth = 57
            Me.COLOR_PICKER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.COLOR_PICKER.ListButtonEnabled = False
            Me.COLOR_PICKER.Location = New System.Drawing.Point(1, 30)
            Me.COLOR_PICKER.Margin = New System.Windows.Forms.Padding(0)
            Me.COLOR_PICKER.Name = "COLOR_PICKER"
            Me.COLOR_PICKER.Padding = New System.Windows.Forms.Padding(0, 0, 2, 0)
            Me.COLOR_PICKER.Size = New System.Drawing.Size(442, 25)
            Me.COLOR_PICKER.TabIndex = 1
            '
            'ColorPickerInternalForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(444, 81)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(460, 120)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(460, 120)
            Me.Name = "ColorPickerInternalForm"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Color"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            CType(Me.TXT_NAME, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents TXT_NAME As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents COLOR_PICKER As ColorPicker
    End Class
End Namespace