' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.YouTube.Controls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class ChaptersForm : Inherits System.Windows.Forms.Form
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
            Dim TP_BUTTONS As System.Windows.Forms.TableLayoutPanel
            Me.BTT_ALL = New System.Windows.Forms.Button()
            Me.BTT_NONE = New System.Windows.Forms.Button()
            Me.BTT_INVERT = New System.Windows.Forms.Button()
            Me.LIST_CHAPTERS = New System.Windows.Forms.CheckedListBox()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            TP_BUTTONS = New System.Windows.Forms.TableLayoutPanel()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            TP_MAIN.SuspendLayout()
            TP_BUTTONS.SuspendLayout()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(384, 361)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(384, 361)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_MAIN.Controls.Add(TP_BUTTONS, 0, 1)
            TP_MAIN.Controls.Add(Me.LIST_CHAPTERS, 0, 0)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 2
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            TP_MAIN.Size = New System.Drawing.Size(384, 361)
            TP_MAIN.TabIndex = 0
            '
            'TP_BUTTONS
            '
            TP_BUTTONS.ColumnCount = 3
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_BUTTONS.Controls.Add(Me.BTT_ALL, 0, 0)
            TP_BUTTONS.Controls.Add(Me.BTT_NONE, 1, 0)
            TP_BUTTONS.Controls.Add(Me.BTT_INVERT, 2, 0)
            TP_BUTTONS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_BUTTONS.Location = New System.Drawing.Point(0, 331)
            TP_BUTTONS.Margin = New System.Windows.Forms.Padding(0)
            TP_BUTTONS.Name = "TP_BUTTONS"
            TP_BUTTONS.RowCount = 1
            TP_BUTTONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_BUTTONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_BUTTONS.Size = New System.Drawing.Size(384, 30)
            TP_BUTTONS.TabIndex = 1
            '
            'BTT_ALL
            '
            Me.BTT_ALL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_ALL.Location = New System.Drawing.Point(3, 3)
            Me.BTT_ALL.Name = "BTT_ALL"
            Me.BTT_ALL.Size = New System.Drawing.Size(122, 24)
            Me.BTT_ALL.TabIndex = 0
            Me.BTT_ALL.Text = "All"
            Me.BTT_ALL.UseVisualStyleBackColor = True
            '
            'BTT_NONE
            '
            Me.BTT_NONE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_NONE.Location = New System.Drawing.Point(131, 3)
            Me.BTT_NONE.Name = "BTT_NONE"
            Me.BTT_NONE.Size = New System.Drawing.Size(122, 24)
            Me.BTT_NONE.TabIndex = 1
            Me.BTT_NONE.Text = "None"
            Me.BTT_NONE.UseVisualStyleBackColor = True
            '
            'BTT_INVERT
            '
            Me.BTT_INVERT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_INVERT.Location = New System.Drawing.Point(259, 3)
            Me.BTT_INVERT.Name = "BTT_INVERT"
            Me.BTT_INVERT.Size = New System.Drawing.Size(122, 24)
            Me.BTT_INVERT.TabIndex = 2
            Me.BTT_INVERT.Text = "Invert"
            Me.BTT_INVERT.UseVisualStyleBackColor = True
            '
            'LIST_CHAPTERS
            '
            Me.LIST_CHAPTERS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LIST_CHAPTERS.FormattingEnabled = True
            Me.LIST_CHAPTERS.Location = New System.Drawing.Point(3, 3)
            Me.LIST_CHAPTERS.Name = "LIST_CHAPTERS"
            Me.LIST_CHAPTERS.Size = New System.Drawing.Size(378, 325)
            Me.LIST_CHAPTERS.TabIndex = 0
            '
            'ChaptersForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(384, 361)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.KeyPreview = True
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(400, 400)
            Me.Name = "ChaptersForm"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.Text = "Chapters"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            TP_BUTTONS.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents BTT_ALL As Button
        Private WithEvents BTT_NONE As Button
        Private WithEvents BTT_INVERT As Button
        Private WithEvents LIST_CHAPTERS As CheckedListBox
    End Class
End Namespace