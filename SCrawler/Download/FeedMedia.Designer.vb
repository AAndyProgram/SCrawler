' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace DownloadObjects
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Public Class FeedMedia : Inherits System.Windows.Forms.UserControl
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
            Dim CONTEXT_SEP_1 As System.Windows.Forms.ToolStripSeparator
            Dim CONTEXT_SEP_2 As System.Windows.Forms.ToolStripSeparator
            Dim CONTEXT_SEP_3 As System.Windows.Forms.ToolStripSeparator
            Dim TP_LBL As System.Windows.Forms.TableLayoutPanel
            Me.CH_CHECKED = New System.Windows.Forms.CheckBox()
            Me.LBL_INFO = New System.Windows.Forms.Label()
            Me.CONTEXT_DATA = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.BTT_CONTEXT_OPEN_MEDIA = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CONTEXT_OPEN_USER = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CONTEXT_OPEN_USER_URL = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CONTEXT_OPEN_USER_POST = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CONTEXT_FIND_USER = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CONTEXT_INFO = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CONTEXT_DELETE = New System.Windows.Forms.ToolStripMenuItem()
            Me.TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            CONTEXT_SEP_1 = New System.Windows.Forms.ToolStripSeparator()
            CONTEXT_SEP_2 = New System.Windows.Forms.ToolStripSeparator()
            CONTEXT_SEP_3 = New System.Windows.Forms.ToolStripSeparator()
            TP_LBL = New System.Windows.Forms.TableLayoutPanel()
            TP_LBL.SuspendLayout()
            Me.CONTEXT_DATA.SuspendLayout()
            Me.TP_MAIN.SuspendLayout()
            Me.SuspendLayout()
            '
            'CONTEXT_SEP_1
            '
            CONTEXT_SEP_1.Name = "CONTEXT_SEP_1"
            CONTEXT_SEP_1.Size = New System.Drawing.Size(134, 6)
            '
            'CONTEXT_SEP_2
            '
            CONTEXT_SEP_2.Name = "CONTEXT_SEP_2"
            CONTEXT_SEP_2.Size = New System.Drawing.Size(134, 6)
            '
            'CONTEXT_SEP_3
            '
            CONTEXT_SEP_3.Name = "CONTEXT_SEP_3"
            CONTEXT_SEP_3.Size = New System.Drawing.Size(134, 6)
            '
            'TP_LBL
            '
            TP_LBL.ColumnCount = 2
            TP_LBL.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_LBL.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_LBL.Controls.Add(Me.CH_CHECKED, 0, 0)
            TP_LBL.Controls.Add(Me.LBL_INFO, 1, 0)
            TP_LBL.Dock = System.Windows.Forms.DockStyle.Fill
            TP_LBL.Location = New System.Drawing.Point(0, 0)
            TP_LBL.Margin = New System.Windows.Forms.Padding(0)
            TP_LBL.Name = "TP_LBL"
            TP_LBL.RowCount = 1
            TP_LBL.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_LBL.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_LBL.Size = New System.Drawing.Size(146, 25)
            TP_LBL.TabIndex = 0
            '
            'CH_CHECKED
            '
            Me.CH_CHECKED.AutoSize = True
            Me.CH_CHECKED.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter
            Me.CH_CHECKED.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_CHECKED.Location = New System.Drawing.Point(3, 3)
            Me.CH_CHECKED.Name = "CH_CHECKED"
            Me.CH_CHECKED.Size = New System.Drawing.Size(19, 19)
            Me.CH_CHECKED.TabIndex = 0
            Me.CH_CHECKED.UseVisualStyleBackColor = True
            '
            'LBL_INFO
            '
            Me.LBL_INFO.AutoSize = True
            Me.LBL_INFO.ContextMenuStrip = Me.CONTEXT_DATA
            Me.LBL_INFO.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LBL_INFO.Location = New System.Drawing.Point(28, 0)
            Me.LBL_INFO.Name = "LBL_INFO"
            Me.LBL_INFO.Size = New System.Drawing.Size(115, 25)
            Me.LBL_INFO.TabIndex = 1
            Me.LBL_INFO.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'CONTEXT_DATA
            '
            Me.CONTEXT_DATA.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_CONTEXT_OPEN_MEDIA, Me.BTT_CONTEXT_OPEN_USER, CONTEXT_SEP_1, Me.BTT_CONTEXT_OPEN_USER_URL, Me.BTT_CONTEXT_OPEN_USER_POST, CONTEXT_SEP_2, Me.BTT_CONTEXT_FIND_USER, Me.BTT_CONTEXT_INFO, CONTEXT_SEP_3, Me.BTT_CONTEXT_DELETE})
            Me.CONTEXT_DATA.Name = "CONTEXT_PIC"
            Me.CONTEXT_DATA.Size = New System.Drawing.Size(138, 176)
            '
            'BTT_CONTEXT_OPEN_MEDIA
            '
            Me.BTT_CONTEXT_OPEN_MEDIA.Image = Global.SCrawler.My.Resources.Resources.Folder_32
            Me.BTT_CONTEXT_OPEN_MEDIA.Name = "BTT_CONTEXT_OPEN_MEDIA"
            Me.BTT_CONTEXT_OPEN_MEDIA.Size = New System.Drawing.Size(137, 22)
            Me.BTT_CONTEXT_OPEN_MEDIA.Text = "Open"
            '
            'BTT_CONTEXT_OPEN_USER
            '
            Me.BTT_CONTEXT_OPEN_USER.Image = Global.SCrawler.My.Resources.Resources.Folder_32
            Me.BTT_CONTEXT_OPEN_USER.Name = "BTT_CONTEXT_OPEN_USER"
            Me.BTT_CONTEXT_OPEN_USER.Size = New System.Drawing.Size(137, 22)
            Me.BTT_CONTEXT_OPEN_USER.Text = "Open user"
            '
            'BTT_CONTEXT_OPEN_USER_URL
            '
            Me.BTT_CONTEXT_OPEN_USER_URL.Image = Global.SCrawler.My.Resources.Resources.GlobeBlue_32
            Me.BTT_CONTEXT_OPEN_USER_URL.Name = "BTT_CONTEXT_OPEN_USER_URL"
            Me.BTT_CONTEXT_OPEN_USER_URL.Size = New System.Drawing.Size(137, 22)
            Me.BTT_CONTEXT_OPEN_USER_URL.Text = "Open user"
            '
            'BTT_CONTEXT_OPEN_USER_POST
            '
            Me.BTT_CONTEXT_OPEN_USER_POST.Image = Global.SCrawler.My.Resources.Resources.GlobeBlue_32
            Me.BTT_CONTEXT_OPEN_USER_POST.Name = "BTT_CONTEXT_OPEN_USER_POST"
            Me.BTT_CONTEXT_OPEN_USER_POST.Size = New System.Drawing.Size(137, 22)
            Me.BTT_CONTEXT_OPEN_USER_POST.Text = "Open post"
            '
            'BTT_CONTEXT_FIND_USER
            '
            Me.BTT_CONTEXT_FIND_USER.Image = Global.SCrawler.My.Resources.Resources.InfoPic_32
            Me.BTT_CONTEXT_FIND_USER.Name = "BTT_CONTEXT_FIND_USER"
            Me.BTT_CONTEXT_FIND_USER.Size = New System.Drawing.Size(137, 22)
            Me.BTT_CONTEXT_FIND_USER.Text = "Find user"
            '
            'BTT_CONTEXT_INFO
            '
            Me.BTT_CONTEXT_INFO.Image = Global.SCrawler.My.Resources.Resources.InfoPic_32
            Me.BTT_CONTEXT_INFO.Name = "BTT_CONTEXT_INFO"
            Me.BTT_CONTEXT_INFO.Size = New System.Drawing.Size(137, 22)
            Me.BTT_CONTEXT_INFO.Text = "Information"
            '
            'BTT_CONTEXT_DELETE
            '
            Me.BTT_CONTEXT_DELETE.Image = Global.SCrawler.My.Resources.Resources.Delete
            Me.BTT_CONTEXT_DELETE.Name = "BTT_CONTEXT_DELETE"
            Me.BTT_CONTEXT_DELETE.Size = New System.Drawing.Size(137, 22)
            Me.BTT_CONTEXT_DELETE.Text = "Delete"
            '
            'TP_MAIN
            '
            Me.TP_MAIN.ColumnCount = 1
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TP_MAIN.Controls.Add(TP_LBL, 0, 0)
            Me.TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.TP_MAIN.Margin = New System.Windows.Forms.Padding(0)
            Me.TP_MAIN.Name = "TP_MAIN"
            Me.TP_MAIN.RowCount = 2
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.Size = New System.Drawing.Size(146, 146)
            Me.TP_MAIN.TabIndex = 0
            '
            'FeedMedia
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Window
            Me.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.Controls.Add(Me.TP_MAIN)
            Me.ForeColor = System.Drawing.SystemColors.WindowText
            Me.Margin = New System.Windows.Forms.Padding(0)
            Me.Name = "FeedMedia"
            Me.Size = New System.Drawing.Size(146, 146)
            TP_LBL.ResumeLayout(False)
            TP_LBL.PerformLayout()
            Me.CONTEXT_DATA.ResumeLayout(False)
            Me.TP_MAIN.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents TP_MAIN As TableLayoutPanel
        Private WithEvents CONTEXT_DATA As ContextMenuStrip
        Private WithEvents BTT_CONTEXT_OPEN_MEDIA As ToolStripMenuItem
        Private WithEvents BTT_CONTEXT_OPEN_USER_URL As ToolStripMenuItem
        Private WithEvents BTT_CONTEXT_OPEN_USER_POST As ToolStripMenuItem
        Private WithEvents BTT_CONTEXT_FIND_USER As ToolStripMenuItem
        Private WithEvents BTT_CONTEXT_DELETE As ToolStripMenuItem
        Private WithEvents BTT_CONTEXT_OPEN_USER As ToolStripMenuItem
        Private WithEvents CH_CHECKED As CheckBox
        Private WithEvents LBL_INFO As Label
        Private WithEvents BTT_CONTEXT_INFO As ToolStripMenuItem
    End Class
End Namespace