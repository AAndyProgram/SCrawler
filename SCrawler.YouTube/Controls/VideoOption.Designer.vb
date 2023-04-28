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
    Partial Friend Class VideoOption : Inherits System.Windows.Forms.UserControl
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
            Me.OPT_CHECKED = New System.Windows.Forms.RadioButton()
            Me.LBL_DEFINITION_INFO = New System.Windows.Forms.Label()
            Me.LBL_DEFINITION = New System.Windows.Forms.Label()
            Me.LBL_CODECS = New System.Windows.Forms.Label()
            Me.LBL_SIZE = New System.Windows.Forms.Label()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            TP_MAIN.SuspendLayout()
            Me.SuspendLayout()
            '
            'TP_MAIN
            '
            TP_MAIN.ColumnCount = 5
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110.0!))
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 58.0!))
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 71.0!))
            TP_MAIN.Controls.Add(Me.OPT_CHECKED, 0, 0)
            TP_MAIN.Controls.Add(Me.LBL_DEFINITION_INFO, 1, 0)
            TP_MAIN.Controls.Add(Me.LBL_DEFINITION, 2, 0)
            TP_MAIN.Controls.Add(Me.LBL_CODECS, 3, 0)
            TP_MAIN.Controls.Add(Me.LBL_SIZE, 4, 0)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Margin = New System.Windows.Forms.Padding(0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 1
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Size = New System.Drawing.Size(459, 30)
            TP_MAIN.TabIndex = 0
            '
            'OPT_CHECKED
            '
            Me.OPT_CHECKED.AutoSize = True
            Me.OPT_CHECKED.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter
            Me.OPT_CHECKED.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_CHECKED.Location = New System.Drawing.Point(3, 3)
            Me.OPT_CHECKED.Name = "OPT_CHECKED"
            Me.OPT_CHECKED.Size = New System.Drawing.Size(30, 24)
            Me.OPT_CHECKED.TabIndex = 0
            Me.OPT_CHECKED.TabStop = True
            Me.OPT_CHECKED.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            Me.OPT_CHECKED.UseVisualStyleBackColor = True
            '
            'LBL_DEFINITION_INFO
            '
            Me.LBL_DEFINITION_INFO.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LBL_DEFINITION_INFO.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
            Me.LBL_DEFINITION_INFO.Location = New System.Drawing.Point(39, 0)
            Me.LBL_DEFINITION_INFO.Name = "LBL_DEFINITION_INFO"
            Me.LBL_DEFINITION_INFO.Size = New System.Drawing.Size(104, 30)
            Me.LBL_DEFINITION_INFO.TabIndex = 1
            Me.LBL_DEFINITION_INFO.Text = "Ultra High Definition"
            Me.LBL_DEFINITION_INFO.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'LBL_DEFINITION
            '
            Me.LBL_DEFINITION.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LBL_DEFINITION.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
            Me.LBL_DEFINITION.Location = New System.Drawing.Point(149, 0)
            Me.LBL_DEFINITION.Name = "LBL_DEFINITION"
            Me.LBL_DEFINITION.Size = New System.Drawing.Size(52, 30)
            Me.LBL_DEFINITION.TabIndex = 2
            Me.LBL_DEFINITION.Text = "1080p"
            Me.LBL_DEFINITION.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'LBL_CODECS
            '
            Me.LBL_CODECS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LBL_CODECS.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
            Me.LBL_CODECS.Location = New System.Drawing.Point(207, 0)
            Me.LBL_CODECS.Name = "LBL_CODECS"
            Me.LBL_CODECS.Size = New System.Drawing.Size(178, 30)
            Me.LBL_CODECS.TabIndex = 3
            Me.LBL_CODECS.Text = "MKV - VP9 - Opus"
            Me.LBL_CODECS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'LBL_SIZE
            '
            Me.LBL_SIZE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LBL_SIZE.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
            Me.LBL_SIZE.Location = New System.Drawing.Point(391, 0)
            Me.LBL_SIZE.Name = "LBL_SIZE"
            Me.LBL_SIZE.Size = New System.Drawing.Size(65, 30)
            Me.LBL_SIZE.TabIndex = 4
            Me.LBL_SIZE.Text = "1 062 MB"
            Me.LBL_SIZE.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'VideoOption
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(TP_MAIN)
            Me.Name = "VideoOption"
            Me.Size = New System.Drawing.Size(459, 30)
            TP_MAIN.ResumeLayout(False)
            TP_MAIN.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents OPT_CHECKED As RadioButton
        Private WithEvents LBL_DEFINITION_INFO As Label
        Private WithEvents LBL_DEFINITION As Label
        Private WithEvents LBL_CODECS As Label
        Private WithEvents LBL_SIZE As Label
    End Class
End Namespace