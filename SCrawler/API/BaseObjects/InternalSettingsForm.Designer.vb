' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.Base
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class InternalSettingsForm : Inherits System.Windows.Forms.Form
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
            Me.CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            Me.TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            Me.TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
            Me.CONTAINER_MAIN.ContentPanel.SuspendLayout()
            Me.CONTAINER_MAIN.SuspendLayout()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            Me.CONTAINER_MAIN.ContentPanel.Controls.Add(Me.TP_MAIN)
            Me.CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(184, 0)
            Me.CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CONTAINER_MAIN.LeftToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            Me.CONTAINER_MAIN.RightToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Size = New System.Drawing.Size(184, 25)
            Me.CONTAINER_MAIN.TabIndex = 0
            Me.CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            Me.TP_MAIN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            Me.TP_MAIN.ColumnCount = 1
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.TP_MAIN.Name = "TP_MAIN"
            Me.TP_MAIN.RowCount = 1
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 160.0!))
            Me.TP_MAIN.Size = New System.Drawing.Size(184, 0)
            Me.TP_MAIN.TabIndex = 0
            '
            'InternalSettingsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(184, 25)
            Me.Controls.Add(Me.CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "InternalSettingsForm"
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Settings"
            Me.CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            Me.CONTAINER_MAIN.ResumeLayout(False)
            Me.CONTAINER_MAIN.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents TP_MAIN As TableLayoutPanel
        Private WithEvents TT_MAIN As ToolTip
        Private WithEvents CONTAINER_MAIN As ToolStripContainer
    End Class
End Namespace