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
    Partial Public Class ColorPicker : Inherits System.Windows.Forms.UserControl
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
            Dim TT_MAIN As System.Windows.Forms.ToolTip
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ColorPicker))
            Me.BTT_COLORS_FORE = New System.Windows.Forms.Button()
            Me.BTT_COLORS_BACK = New System.Windows.Forms.Button()
            Me.BTT_COLORS_CLEAR = New System.Windows.Forms.Button()
            Me.BTT_SELECT = New System.Windows.Forms.Button()
            Me.TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            Me.LBL_CAPTION = New System.Windows.Forms.Label()
            Me.LBL_COLORS = New System.Windows.Forms.Label()
            TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
            Me.TP_MAIN.SuspendLayout()
            Me.SuspendLayout()
            '
            'BTT_COLORS_FORE
            '
            Me.BTT_COLORS_FORE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_COLORS_FORE.Location = New System.Drawing.Point(254, 2)
            Me.BTT_COLORS_FORE.Margin = New System.Windows.Forms.Padding(2)
            Me.BTT_COLORS_FORE.Name = "BTT_COLORS_FORE"
            Me.BTT_COLORS_FORE.Size = New System.Drawing.Size(18, 24)
            Me.BTT_COLORS_FORE.TabIndex = 2
            Me.BTT_COLORS_FORE.Tag = "F"
            Me.BTT_COLORS_FORE.Text = "F"
            TT_MAIN.SetToolTip(Me.BTT_COLORS_FORE, "Font color")
            Me.BTT_COLORS_FORE.UseVisualStyleBackColor = True
            '
            'BTT_COLORS_BACK
            '
            Me.BTT_COLORS_BACK.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_COLORS_BACK.Location = New System.Drawing.Point(276, 2)
            Me.BTT_COLORS_BACK.Margin = New System.Windows.Forms.Padding(2)
            Me.BTT_COLORS_BACK.Name = "BTT_COLORS_BACK"
            Me.BTT_COLORS_BACK.Size = New System.Drawing.Size(18, 24)
            Me.BTT_COLORS_BACK.TabIndex = 3
            Me.BTT_COLORS_BACK.Tag = "C"
            Me.BTT_COLORS_BACK.Text = "C"
            TT_MAIN.SetToolTip(Me.BTT_COLORS_BACK, "Back color")
            Me.BTT_COLORS_BACK.UseVisualStyleBackColor = True
            '
            'BTT_COLORS_CLEAR
            '
            Me.BTT_COLORS_CLEAR.BackgroundImage = CType(resources.GetObject("BTT_COLORS_CLEAR.BackgroundImage"), System.Drawing.Image)
            Me.BTT_COLORS_CLEAR.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
            Me.BTT_COLORS_CLEAR.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_COLORS_CLEAR.Location = New System.Drawing.Point(320, 2)
            Me.BTT_COLORS_CLEAR.Margin = New System.Windows.Forms.Padding(2)
            Me.BTT_COLORS_CLEAR.Name = "BTT_COLORS_CLEAR"
            Me.BTT_COLORS_CLEAR.Size = New System.Drawing.Size(18, 24)
            Me.BTT_COLORS_CLEAR.TabIndex = 4
            Me.BTT_COLORS_CLEAR.Tag = "D"
            TT_MAIN.SetToolTip(Me.BTT_COLORS_CLEAR, "Reset")
            Me.BTT_COLORS_CLEAR.UseVisualStyleBackColor = True
            '
            'BTT_SELECT
            '
            Me.BTT_SELECT.BackgroundImage = CType(resources.GetObject("BTT_SELECT.BackgroundImage"), System.Drawing.Image)
            Me.BTT_SELECT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
            Me.BTT_SELECT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_SELECT.Location = New System.Drawing.Point(298, 2)
            Me.BTT_SELECT.Margin = New System.Windows.Forms.Padding(2)
            Me.BTT_SELECT.Name = "BTT_SELECT"
            Me.BTT_SELECT.Size = New System.Drawing.Size(18, 24)
            Me.BTT_SELECT.TabIndex = 5
            TT_MAIN.SetToolTip(Me.BTT_SELECT, "Select color from saved ones")
            Me.BTT_SELECT.UseVisualStyleBackColor = True
            '
            'TP_MAIN
            '
            Me.TP_MAIN.ColumnCount = 6
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105.0!))
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 22.0!))
            Me.TP_MAIN.Controls.Add(Me.LBL_CAPTION, 0, 0)
            Me.TP_MAIN.Controls.Add(Me.LBL_COLORS, 1, 0)
            Me.TP_MAIN.Controls.Add(Me.BTT_COLORS_FORE, 2, 0)
            Me.TP_MAIN.Controls.Add(Me.BTT_COLORS_BACK, 3, 0)
            Me.TP_MAIN.Controls.Add(Me.BTT_COLORS_CLEAR, 5, 0)
            Me.TP_MAIN.Controls.Add(Me.BTT_SELECT, 4, 0)
            Me.TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.TP_MAIN.Margin = New System.Windows.Forms.Padding(0)
            Me.TP_MAIN.Name = "TP_MAIN"
            Me.TP_MAIN.RowCount = 1
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.Size = New System.Drawing.Size(340, 28)
            Me.TP_MAIN.TabIndex = 3
            '
            'LBL_CAPTION
            '
            Me.LBL_CAPTION.AutoSize = True
            Me.LBL_CAPTION.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LBL_CAPTION.Location = New System.Drawing.Point(3, 0)
            Me.LBL_CAPTION.Name = "LBL_CAPTION"
            Me.LBL_CAPTION.Size = New System.Drawing.Size(99, 28)
            Me.LBL_CAPTION.TabIndex = 0
            Me.LBL_CAPTION.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'LBL_COLORS
            '
            Me.LBL_COLORS.AutoSize = True
            Me.LBL_COLORS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LBL_COLORS.Location = New System.Drawing.Point(108, 3)
            Me.LBL_COLORS.Margin = New System.Windows.Forms.Padding(3)
            Me.LBL_COLORS.Name = "LBL_COLORS"
            Me.LBL_COLORS.Size = New System.Drawing.Size(141, 22)
            Me.LBL_COLORS.TabIndex = 1
            Me.LBL_COLORS.Text = "Here's what it looks like."
            Me.LBL_COLORS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'ColorPicker
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.TP_MAIN)
            Me.Name = "ColorPicker"
            Me.Size = New System.Drawing.Size(340, 28)
            Me.TP_MAIN.ResumeLayout(False)
            Me.TP_MAIN.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents LBL_COLORS As Label
        Private WithEvents BTT_COLORS_FORE As Button
        Private WithEvents BTT_COLORS_BACK As Button
        Private WithEvents BTT_COLORS_CLEAR As Button
        Private WithEvents TP_MAIN As TableLayoutPanel
        Private WithEvents LBL_CAPTION As Label
        Private WithEvents BTT_SELECT As Button
    End Class
End Namespace