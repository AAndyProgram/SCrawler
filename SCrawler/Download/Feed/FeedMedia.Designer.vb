' Copyright (C) 2023  Andy https://github.com/AAndyProgram
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
            Dim TP_LBL As System.Windows.Forms.TableLayoutPanel
            Dim CONTEXT_SEP_3 As System.Windows.Forms.ToolStripSeparator
            Dim CONTEXT_SEP_4 As System.Windows.Forms.ToolStripSeparator
            Me.CH_CHECKED = New System.Windows.Forms.CheckBox()
            Me.LBL_INFO = New System.Windows.Forms.Label()
            Me.CONTEXT_DATA = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.BTT_CONTEXT_DOWN = New System.Windows.Forms.ToolStripMenuItem()
            Me.CONTEXT_SEP_0 = New System.Windows.Forms.ToolStripSeparator()
            Me.BTT_CONTEXT_OPEN_MEDIA = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CONTEXT_OPEN_USER = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CONTEXT_OPEN_USER_URL = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CONTEXT_OPEN_USER_POST = New System.Windows.Forms.ToolStripMenuItem()
            Me.CONTEXT_SEP_2 = New System.Windows.Forms.ToolStripSeparator()
            Me.BTT_COPY_TO = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_MOVE_TO = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FEED_ADD_FAV = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FEED_ADD_FAV_REMOVE = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FEED_ADD_SPEC = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FEED_ADD_SPEC_REMOVE = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FEED_REMOVE_FAV = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FEED_REMOVE_SPEC = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CONTEXT_FIND_USER = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CONTEXT_INFO = New System.Windows.Forms.ToolStripMenuItem()
            Me.CONTEXT_SEP_5 = New System.Windows.Forms.ToolStripSeparator()
            Me.BTT_CONTEXT_DELETE = New System.Windows.Forms.ToolStripMenuItem()
            Me.ICON_SITE = New System.Windows.Forms.PictureBox()
            Me.TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            Me.LBL_TITLE = New System.Windows.Forms.Label()
            Me.BTT_CONTEXT_OPEN_FILE_FOLDER = New System.Windows.Forms.ToolStripMenuItem()
            CONTEXT_SEP_1 = New System.Windows.Forms.ToolStripSeparator()
            TP_LBL = New System.Windows.Forms.TableLayoutPanel()
            CONTEXT_SEP_3 = New System.Windows.Forms.ToolStripSeparator()
            CONTEXT_SEP_4 = New System.Windows.Forms.ToolStripSeparator()
            TP_LBL.SuspendLayout()
            Me.CONTEXT_DATA.SuspendLayout()
            CType(Me.ICON_SITE, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TP_MAIN.SuspendLayout()
            Me.SuspendLayout()
            '
            'CONTEXT_SEP_1
            '
            CONTEXT_SEP_1.Name = "CONTEXT_SEP_1"
            CONTEXT_SEP_1.Size = New System.Drawing.Size(302, 6)
            '
            'TP_LBL
            '
            TP_LBL.ColumnCount = 3
            TP_LBL.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_LBL.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_LBL.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_LBL.Controls.Add(Me.CH_CHECKED, 0, 0)
            TP_LBL.Controls.Add(Me.LBL_INFO, 2, 0)
            TP_LBL.Controls.Add(Me.ICON_SITE, 1, 0)
            TP_LBL.Dock = System.Windows.Forms.DockStyle.Fill
            TP_LBL.Location = New System.Drawing.Point(0, 0)
            TP_LBL.Margin = New System.Windows.Forms.Padding(0)
            TP_LBL.Name = "TP_LBL"
            TP_LBL.RowCount = 1
            TP_LBL.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
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
            Me.LBL_INFO.Location = New System.Drawing.Point(53, 0)
            Me.LBL_INFO.Name = "LBL_INFO"
            Me.LBL_INFO.Size = New System.Drawing.Size(90, 25)
            Me.LBL_INFO.TabIndex = 1
            Me.LBL_INFO.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'CONTEXT_DATA
            '
            Me.CONTEXT_DATA.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_CONTEXT_DOWN, Me.CONTEXT_SEP_0, Me.BTT_CONTEXT_OPEN_MEDIA, Me.BTT_CONTEXT_OPEN_USER, Me.BTT_CONTEXT_OPEN_FILE_FOLDER, CONTEXT_SEP_1, Me.BTT_CONTEXT_OPEN_USER_URL, Me.BTT_CONTEXT_OPEN_USER_POST, Me.CONTEXT_SEP_2, Me.BTT_COPY_TO, Me.BTT_MOVE_TO, CONTEXT_SEP_3, Me.BTT_FEED_ADD_FAV, Me.BTT_FEED_ADD_FAV_REMOVE, Me.BTT_FEED_ADD_SPEC, Me.BTT_FEED_ADD_SPEC_REMOVE, Me.BTT_FEED_REMOVE_FAV, Me.BTT_FEED_REMOVE_SPEC, CONTEXT_SEP_4, Me.BTT_CONTEXT_FIND_USER, Me.BTT_CONTEXT_INFO, Me.CONTEXT_SEP_5, Me.BTT_CONTEXT_DELETE})
            Me.CONTEXT_DATA.Name = "CONTEXT_PIC"
            Me.CONTEXT_DATA.Size = New System.Drawing.Size(306, 436)
            '
            'BTT_CONTEXT_DOWN
            '
            Me.BTT_CONTEXT_DOWN.Image = Global.SCrawler.My.Resources.Resources.StartPic_Green_16
            Me.BTT_CONTEXT_DOWN.Name = "BTT_CONTEXT_DOWN"
            Me.BTT_CONTEXT_DOWN.Size = New System.Drawing.Size(305, 22)
            Me.BTT_CONTEXT_DOWN.Text = "Download"
            Me.BTT_CONTEXT_DOWN.Visible = False
            '
            'CONTEXT_SEP_0
            '
            Me.CONTEXT_SEP_0.Name = "CONTEXT_SEP_0"
            Me.CONTEXT_SEP_0.Size = New System.Drawing.Size(302, 6)
            Me.CONTEXT_SEP_0.Visible = False
            '
            'BTT_CONTEXT_OPEN_MEDIA
            '
            Me.BTT_CONTEXT_OPEN_MEDIA.Image = Global.SCrawler.My.Resources.Resources.FolderPic_32
            Me.BTT_CONTEXT_OPEN_MEDIA.Name = "BTT_CONTEXT_OPEN_MEDIA"
            Me.BTT_CONTEXT_OPEN_MEDIA.Size = New System.Drawing.Size(305, 22)
            Me.BTT_CONTEXT_OPEN_MEDIA.Text = "Open"
            '
            'BTT_CONTEXT_OPEN_USER
            '
            Me.BTT_CONTEXT_OPEN_USER.Image = Global.SCrawler.My.Resources.Resources.FolderPic_32
            Me.BTT_CONTEXT_OPEN_USER.Name = "BTT_CONTEXT_OPEN_USER"
            Me.BTT_CONTEXT_OPEN_USER.Size = New System.Drawing.Size(305, 22)
            Me.BTT_CONTEXT_OPEN_USER.Text = "Open user"
            '
            'BTT_CONTEXT_OPEN_USER_URL
            '
            Me.BTT_CONTEXT_OPEN_USER_URL.Image = Global.SCrawler.My.Resources.Resources.GlobePic_32
            Me.BTT_CONTEXT_OPEN_USER_URL.Name = "BTT_CONTEXT_OPEN_USER_URL"
            Me.BTT_CONTEXT_OPEN_USER_URL.Size = New System.Drawing.Size(305, 22)
            Me.BTT_CONTEXT_OPEN_USER_URL.Text = "Open user"
            '
            'BTT_CONTEXT_OPEN_USER_POST
            '
            Me.BTT_CONTEXT_OPEN_USER_POST.Image = Global.SCrawler.My.Resources.Resources.GlobePic_32
            Me.BTT_CONTEXT_OPEN_USER_POST.Name = "BTT_CONTEXT_OPEN_USER_POST"
            Me.BTT_CONTEXT_OPEN_USER_POST.Size = New System.Drawing.Size(305, 22)
            Me.BTT_CONTEXT_OPEN_USER_POST.Text = "Open post"
            '
            'CONTEXT_SEP_2
            '
            Me.CONTEXT_SEP_2.Name = "CONTEXT_SEP_2"
            Me.CONTEXT_SEP_2.Size = New System.Drawing.Size(302, 6)
            '
            'BTT_COPY_TO
            '
            Me.BTT_COPY_TO.Image = Global.SCrawler.My.Resources.Resources.PastePic_32
            Me.BTT_COPY_TO.Name = "BTT_COPY_TO"
            Me.BTT_COPY_TO.Size = New System.Drawing.Size(305, 22)
            Me.BTT_COPY_TO.Text = "Copy to..."
            '
            'BTT_MOVE_TO
            '
            Me.BTT_MOVE_TO.Image = Global.SCrawler.My.Resources.Resources.CutPic_48
            Me.BTT_MOVE_TO.Name = "BTT_MOVE_TO"
            Me.BTT_MOVE_TO.Size = New System.Drawing.Size(305, 22)
            Me.BTT_MOVE_TO.Text = "Move to..."
            '
            'CONTEXT_SEP_3
            '
            CONTEXT_SEP_3.Name = "CONTEXT_SEP_3"
            CONTEXT_SEP_3.Size = New System.Drawing.Size(302, 6)
            '
            'BTT_FEED_ADD_FAV
            '
            Me.BTT_FEED_ADD_FAV.Image = Global.SCrawler.My.Resources.Resources.HeartPic_32
            Me.BTT_FEED_ADD_FAV.Name = "BTT_FEED_ADD_FAV"
            Me.BTT_FEED_ADD_FAV.Size = New System.Drawing.Size(305, 22)
            Me.BTT_FEED_ADD_FAV.Text = "Add to Favorite"
            '
            'BTT_FEED_ADD_FAV_REMOVE
            '
            Me.BTT_FEED_ADD_FAV_REMOVE.Image = Global.SCrawler.My.Resources.Resources.HeartPic_32
            Me.BTT_FEED_ADD_FAV_REMOVE.Name = "BTT_FEED_ADD_FAV_REMOVE"
            Me.BTT_FEED_ADD_FAV_REMOVE.Size = New System.Drawing.Size(305, 22)
            Me.BTT_FEED_ADD_FAV_REMOVE.Text = "Add to Favorite (remove from current)"
            '
            'BTT_FEED_ADD_SPEC
            '
            Me.BTT_FEED_ADD_SPEC.Image = Global.SCrawler.My.Resources.Resources.RSSPic_512
            Me.BTT_FEED_ADD_SPEC.Name = "BTT_FEED_ADD_SPEC"
            Me.BTT_FEED_ADD_SPEC.Size = New System.Drawing.Size(305, 22)
            Me.BTT_FEED_ADD_SPEC.Text = "Add to special feed..."
            '
            'BTT_FEED_ADD_SPEC_REMOVE
            '
            Me.BTT_FEED_ADD_SPEC_REMOVE.Image = Global.SCrawler.My.Resources.Resources.RSSPic_512
            Me.BTT_FEED_ADD_SPEC_REMOVE.Name = "BTT_FEED_ADD_SPEC_REMOVE"
            Me.BTT_FEED_ADD_SPEC_REMOVE.Size = New System.Drawing.Size(305, 22)
            Me.BTT_FEED_ADD_SPEC_REMOVE.Text = "Add to special feed (remove from current)..."
            '
            'BTT_FEED_REMOVE_FAV
            '
            Me.BTT_FEED_REMOVE_FAV.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
            Me.BTT_FEED_REMOVE_FAV.Name = "BTT_FEED_REMOVE_FAV"
            Me.BTT_FEED_REMOVE_FAV.Size = New System.Drawing.Size(305, 22)
            Me.BTT_FEED_REMOVE_FAV.Text = "Remove from Favorite"
            '
            'BTT_FEED_REMOVE_SPEC
            '
            Me.BTT_FEED_REMOVE_SPEC.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
            Me.BTT_FEED_REMOVE_SPEC.Name = "BTT_FEED_REMOVE_SPEC"
            Me.BTT_FEED_REMOVE_SPEC.Size = New System.Drawing.Size(305, 22)
            Me.BTT_FEED_REMOVE_SPEC.Text = "Remove from special feed..."
            '
            'CONTEXT_SEP_4
            '
            CONTEXT_SEP_4.Name = "CONTEXT_SEP_4"
            CONTEXT_SEP_4.Size = New System.Drawing.Size(302, 6)
            '
            'BTT_CONTEXT_FIND_USER
            '
            Me.BTT_CONTEXT_FIND_USER.Image = Global.SCrawler.My.Resources.Resources.InfoPic_32
            Me.BTT_CONTEXT_FIND_USER.Name = "BTT_CONTEXT_FIND_USER"
            Me.BTT_CONTEXT_FIND_USER.Size = New System.Drawing.Size(305, 22)
            Me.BTT_CONTEXT_FIND_USER.Text = "Find user"
            '
            'BTT_CONTEXT_INFO
            '
            Me.BTT_CONTEXT_INFO.Image = Global.SCrawler.My.Resources.Resources.InfoPic_32
            Me.BTT_CONTEXT_INFO.Name = "BTT_CONTEXT_INFO"
            Me.BTT_CONTEXT_INFO.Size = New System.Drawing.Size(305, 22)
            Me.BTT_CONTEXT_INFO.Text = "Information"
            '
            'CONTEXT_SEP_5
            '
            Me.CONTEXT_SEP_5.Name = "CONTEXT_SEP_5"
            Me.CONTEXT_SEP_5.Size = New System.Drawing.Size(302, 6)
            '
            'BTT_CONTEXT_DELETE
            '
            Me.BTT_CONTEXT_DELETE.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
            Me.BTT_CONTEXT_DELETE.Name = "BTT_CONTEXT_DELETE"
            Me.BTT_CONTEXT_DELETE.Size = New System.Drawing.Size(305, 22)
            Me.BTT_CONTEXT_DELETE.Text = "Delete"
            '
            'ICON_SITE
            '
            Me.ICON_SITE.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
            Me.ICON_SITE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ICON_SITE.Location = New System.Drawing.Point(28, 3)
            Me.ICON_SITE.Name = "ICON_SITE"
            Me.ICON_SITE.Size = New System.Drawing.Size(19, 19)
            Me.ICON_SITE.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.ICON_SITE.TabIndex = 2
            Me.ICON_SITE.TabStop = False
            '
            'TP_MAIN
            '
            Me.TP_MAIN.ColumnCount = 1
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.Controls.Add(TP_LBL, 0, 0)
            Me.TP_MAIN.Controls.Add(Me.LBL_TITLE, 0, 1)
            Me.TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.TP_MAIN.Margin = New System.Windows.Forms.Padding(0)
            Me.TP_MAIN.Name = "TP_MAIN"
            Me.TP_MAIN.RowCount = 3
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.Size = New System.Drawing.Size(146, 146)
            Me.TP_MAIN.TabIndex = 0
            '
            'LBL_TITLE
            '
            Me.LBL_TITLE.AutoSize = True
            Me.LBL_TITLE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LBL_TITLE.Location = New System.Drawing.Point(3, 25)
            Me.LBL_TITLE.Name = "LBL_TITLE"
            Me.LBL_TITLE.Size = New System.Drawing.Size(140, 25)
            Me.LBL_TITLE.TabIndex = 1
            '
            'BTT_CONTEXT_OPEN_FILE_FOLDER
            '
            Me.BTT_CONTEXT_OPEN_FILE_FOLDER.Image = Global.SCrawler.My.Resources.Resources.FolderPic_32
            Me.BTT_CONTEXT_OPEN_FILE_FOLDER.Name = "BTT_CONTEXT_OPEN_FILE_FOLDER"
            Me.BTT_CONTEXT_OPEN_FILE_FOLDER.Size = New System.Drawing.Size(305, 22)
            Me.BTT_CONTEXT_OPEN_FILE_FOLDER.Text = "Open file folder"
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
            CType(Me.ICON_SITE, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TP_MAIN.ResumeLayout(False)
            Me.TP_MAIN.PerformLayout()
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
        Private WithEvents ICON_SITE As PictureBox
        Private WithEvents BTT_CONTEXT_DOWN As ToolStripMenuItem
        Private WithEvents CONTEXT_SEP_0 As ToolStripSeparator
        Private WithEvents LBL_TITLE As Label
        Private WithEvents BTT_FEED_ADD_FAV As ToolStripMenuItem
        Private WithEvents BTT_FEED_ADD_SPEC As ToolStripMenuItem
        Private WithEvents BTT_FEED_REMOVE_FAV As ToolStripMenuItem
        Private WithEvents BTT_FEED_REMOVE_SPEC As ToolStripMenuItem
        Private WithEvents BTT_FEED_ADD_FAV_REMOVE As ToolStripMenuItem
        Private WithEvents BTT_FEED_ADD_SPEC_REMOVE As ToolStripMenuItem
        Private WithEvents BTT_COPY_TO As ToolStripMenuItem
        Private WithEvents BTT_MOVE_TO As ToolStripMenuItem
        Private WithEvents CONTEXT_SEP_5 As ToolStripSeparator
        Private WithEvents CONTEXT_SEP_2 As ToolStripSeparator
        Private WithEvents BTT_CONTEXT_OPEN_FILE_FOLDER As ToolStripMenuItem
    End Class
End Namespace