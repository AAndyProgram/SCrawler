' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.YouTube.Controls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Public Class ParsingProgressForm : Inherits System.Windows.Forms.Form
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
            Dim TP_MAIN As System.Windows.Forms.TableLayoutPanel
            Me.PR_MAIN = New System.Windows.Forms.ProgressBar()
            Me.LBL_MAIN = New System.Windows.Forms.Label()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            TP_MAIN.SuspendLayout()
            Me.SuspendLayout()
            '
            'TP_MAIN
            '
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(Me.PR_MAIN, 0, 0)
            TP_MAIN.Controls.Add(Me.LBL_MAIN, 0, 1)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Margin = New System.Windows.Forms.Padding(0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 2
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.Size = New System.Drawing.Size(334, 56)
            TP_MAIN.TabIndex = 0
            '
            'PR_MAIN
            '
            Me.PR_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.PR_MAIN.Location = New System.Drawing.Point(3, 3)
            Me.PR_MAIN.Name = "PR_MAIN"
            Me.PR_MAIN.Size = New System.Drawing.Size(328, 25)
            Me.PR_MAIN.TabIndex = 0
            '
            'LBL_MAIN
            '
            Me.LBL_MAIN.AutoSize = True
            Me.LBL_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LBL_MAIN.Location = New System.Drawing.Point(3, 31)
            Me.LBL_MAIN.Name = "LBL_MAIN"
            Me.LBL_MAIN.Size = New System.Drawing.Size(328, 25)
            Me.LBL_MAIN.TabIndex = 1
            Me.LBL_MAIN.TextAlign = System.Drawing.ContentAlignment.TopCenter
            '
            'ParsingProgressForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(334, 56)
            Me.ControlBox = False
            Me.Controls.Add(TP_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(350, 95)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(350, 95)
            Me.Name = "ParsingProgressForm"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Parsing progress"
            TP_MAIN.ResumeLayout(False)
            TP_MAIN.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents PR_MAIN As ProgressBar
        Private WithEvents LBL_MAIN As Label
    End Class
End Namespace