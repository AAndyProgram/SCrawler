' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Friend Class DownloadSavedPostsForm : Inherits System.Windows.Forms.Form
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
        Dim TP_BUTTONS As System.Windows.Forms.TableLayoutPanel
        Dim TT_MAIN As System.Windows.Forms.ToolTip
        Me.BTT_DOWN_ALL = New System.Windows.Forms.Button()
        Me.BTT_STOP_ALL = New System.Windows.Forms.Button()
        Me.TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
        TP_BUTTONS = New System.Windows.Forms.TableLayoutPanel()
        TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
        TP_BUTTONS.SuspendLayout()
        Me.TP_MAIN.SuspendLayout()
        Me.SuspendLayout()
        '
        'TP_BUTTONS
        '
        TP_BUTTONS.ColumnCount = 2
        TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        TP_BUTTONS.Controls.Add(Me.BTT_DOWN_ALL, 0, 0)
        TP_BUTTONS.Controls.Add(Me.BTT_STOP_ALL, 1, 0)
        TP_BUTTONS.Dock = System.Windows.Forms.DockStyle.Fill
        TP_BUTTONS.Location = New System.Drawing.Point(2, 2)
        TP_BUTTONS.Margin = New System.Windows.Forms.Padding(0)
        TP_BUTTONS.Name = "TP_BUTTONS"
        TP_BUTTONS.RowCount = 1
        TP_BUTTONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        TP_BUTTONS.Size = New System.Drawing.Size(480, 37)
        TP_BUTTONS.TabIndex = 0
        '
        'BTT_DOWN_ALL
        '
        Me.BTT_DOWN_ALL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BTT_DOWN_ALL.Location = New System.Drawing.Point(3, 3)
        Me.BTT_DOWN_ALL.Name = "BTT_DOWN_ALL"
        Me.BTT_DOWN_ALL.Size = New System.Drawing.Size(234, 31)
        Me.BTT_DOWN_ALL.TabIndex = 0
        Me.BTT_DOWN_ALL.Text = "Download ALL"
        TT_MAIN.SetToolTip(Me.BTT_DOWN_ALL, "Ctrl+Click: download, exclude from feed.")
        Me.BTT_DOWN_ALL.UseVisualStyleBackColor = True
        '
        'BTT_STOP_ALL
        '
        Me.BTT_STOP_ALL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BTT_STOP_ALL.Location = New System.Drawing.Point(243, 3)
        Me.BTT_STOP_ALL.Name = "BTT_STOP_ALL"
        Me.BTT_STOP_ALL.Size = New System.Drawing.Size(234, 31)
        Me.BTT_STOP_ALL.TabIndex = 1
        Me.BTT_STOP_ALL.Text = "Stop ALL"
        Me.BTT_STOP_ALL.UseVisualStyleBackColor = True
        '
        'TP_MAIN
        '
        Me.TP_MAIN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset
        Me.TP_MAIN.ColumnCount = 1
        Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TP_MAIN.Controls.Add(TP_BUTTONS, 0, 0)
        Me.TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TP_MAIN.Location = New System.Drawing.Point(0, 0)
        Me.TP_MAIN.Name = "TP_MAIN"
        Me.TP_MAIN.RowCount = 1
        Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        Me.TP_MAIN.Size = New System.Drawing.Size(484, 41)
        Me.TP_MAIN.TabIndex = 0
        '
        'DownloadSavedPostsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 41)
        Me.Controls.Add(Me.TP_MAIN)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = Global.SCrawler.My.Resources.Resources.BookmarkIcon_32
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(500, 80)
        Me.MinimumSize = New System.Drawing.Size(500, 80)
        Me.Name = "DownloadSavedPostsForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Saved posts"
        TP_BUTTONS.ResumeLayout(False)
        Me.TP_MAIN.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents BTT_DOWN_ALL As Button
    Private WithEvents BTT_STOP_ALL As Button
    Private WithEvents TP_MAIN As TableLayoutPanel
End Class