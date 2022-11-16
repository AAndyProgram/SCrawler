' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Friend Class ChannelViewForm : Inherits System.Windows.Forms.Form
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
        Dim SEP_1 As System.Windows.Forms.ToolStripSeparator
        Dim CONTEXT_SEP_1 As System.Windows.Forms.ToolStripSeparator
        Dim CONTEXT_SEP_2 As System.Windows.Forms.ToolStripSeparator
        Me.TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
        Me.ToolbarTOP = New System.Windows.Forms.ToolStrip()
        Me.BTT_DOWNLOAD = New System.Windows.Forms.ToolStripButton()
        Me.BTT_STOP = New System.Windows.Forms.ToolStripButton()
        Me.BTT_ADD_USERS = New System.Windows.Forms.ToolStripButton()
        Me.ToolbarBOTTOM = New System.Windows.Forms.StatusStrip()
        Me.PR_CN = New System.Windows.Forms.ToolStripProgressBar()
        Me.LBL_STATUS = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LIST_POSTS = New System.Windows.Forms.ListView()
        Me.LCONTEXT = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.BTT_C_OPEN_USER = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_C_OPEN_POST = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_C_OPEN_PICTURE = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_C_OPEN_FOLDER = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_C_REMOVE_FROM_SELECTED = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_C_ADD_TO_BLACKLIST = New System.Windows.Forms.ToolStripMenuItem()
        SEP_1 = New System.Windows.Forms.ToolStripSeparator()
        CONTEXT_SEP_1 = New System.Windows.Forms.ToolStripSeparator()
        CONTEXT_SEP_2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolbarTOP.SuspendLayout()
        Me.ToolbarBOTTOM.SuspendLayout()
        Me.LCONTEXT.SuspendLayout()
        Me.SuspendLayout()
        '
        'SEP_1
        '
        SEP_1.Name = "SEP_1"
        SEP_1.Size = New System.Drawing.Size(6, 25)
        '
        'CONTEXT_SEP_1
        '
        CONTEXT_SEP_1.Name = "CONTEXT_SEP_1"
        CONTEXT_SEP_1.Size = New System.Drawing.Size(302, 6)
        '
        'CONTEXT_SEP_2
        '
        CONTEXT_SEP_2.Name = "CONTEXT_SEP_2"
        CONTEXT_SEP_2.Size = New System.Drawing.Size(302, 6)
        '
        'ToolbarTOP
        '
        Me.ToolbarTOP.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolbarTOP.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_DOWNLOAD, Me.BTT_STOP, SEP_1, Me.BTT_ADD_USERS})
        Me.ToolbarTOP.Location = New System.Drawing.Point(0, 0)
        Me.ToolbarTOP.Name = "ToolbarTOP"
        Me.ToolbarTOP.Size = New System.Drawing.Size(744, 25)
        Me.ToolbarTOP.TabIndex = 0
        '
        'BTT_DOWNLOAD
        '
        Me.BTT_DOWNLOAD.AutoToolTip = False
        Me.BTT_DOWNLOAD.Image = Global.SCrawler.My.Resources.Resources.StartPic_Green_16
        Me.BTT_DOWNLOAD.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_DOWNLOAD.Name = "BTT_DOWNLOAD"
        Me.BTT_DOWNLOAD.Size = New System.Drawing.Size(104, 22)
        Me.BTT_DOWNLOAD.Text = "Download (F5)"
        '
        'BTT_STOP
        '
        Me.BTT_STOP.Enabled = False
        Me.BTT_STOP.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
        Me.BTT_STOP.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_STOP.Name = "BTT_STOP"
        Me.BTT_STOP.Size = New System.Drawing.Size(51, 22)
        Me.BTT_STOP.Text = "Stop"
        Me.BTT_STOP.ToolTipText = "Stop downloading"
        '
        'BTT_ADD_USERS
        '
        Me.BTT_ADD_USERS.Image = Global.SCrawler.My.Resources.Resources.PlusPic_24
        Me.BTT_ADD_USERS.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_ADD_USERS.Name = "BTT_ADD_USERS"
        Me.BTT_ADD_USERS.Size = New System.Drawing.Size(49, 22)
        Me.BTT_ADD_USERS.Text = "Add"
        Me.BTT_ADD_USERS.ToolTipText = "Add pending users to collection (F8)"
        '
        'ToolbarBOTTOM
        '
        Me.ToolbarBOTTOM.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PR_CN, Me.LBL_STATUS})
        Me.ToolbarBOTTOM.Location = New System.Drawing.Point(0, 439)
        Me.ToolbarBOTTOM.Name = "ToolbarBOTTOM"
        Me.ToolbarBOTTOM.Size = New System.Drawing.Size(744, 22)
        Me.ToolbarBOTTOM.TabIndex = 1
        '
        'PR_CN
        '
        Me.PR_CN.Name = "PR_CN"
        Me.PR_CN.Size = New System.Drawing.Size(200, 16)
        '
        'LBL_STATUS
        '
        Me.LBL_STATUS.Name = "LBL_STATUS"
        Me.LBL_STATUS.Size = New System.Drawing.Size(0, 17)
        '
        'LIST_POSTS
        '
        Me.LIST_POSTS.CheckBoxes = True
        Me.LIST_POSTS.ContextMenuStrip = Me.LCONTEXT
        Me.LIST_POSTS.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LIST_POSTS.HideSelection = False
        Me.LIST_POSTS.Location = New System.Drawing.Point(0, 25)
        Me.LIST_POSTS.MultiSelect = False
        Me.LIST_POSTS.Name = "LIST_POSTS"
        Me.LIST_POSTS.Size = New System.Drawing.Size(744, 414)
        Me.LIST_POSTS.TabIndex = 2
        Me.LIST_POSTS.UseCompatibleStateImageBehavior = False
        '
        'LCONTEXT
        '
        Me.LCONTEXT.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_C_OPEN_USER, Me.BTT_C_OPEN_POST, Me.BTT_C_OPEN_PICTURE, Me.BTT_C_OPEN_FOLDER, CONTEXT_SEP_1, Me.BTT_C_REMOVE_FROM_SELECTED, CONTEXT_SEP_2, Me.BTT_C_ADD_TO_BLACKLIST})
        Me.LCONTEXT.Name = "LCONTEXT"
        Me.LCONTEXT.Size = New System.Drawing.Size(306, 148)
        '
        'BTT_C_OPEN_USER
        '
        Me.BTT_C_OPEN_USER.Image = Global.SCrawler.My.Resources.Resources.GlobePic_32
        Me.BTT_C_OPEN_USER.Name = "BTT_C_OPEN_USER"
        Me.BTT_C_OPEN_USER.Size = New System.Drawing.Size(305, 22)
        Me.BTT_C_OPEN_USER.Text = "Open user"
        '
        'BTT_C_OPEN_POST
        '
        Me.BTT_C_OPEN_POST.Image = Global.SCrawler.My.Resources.Resources.GlobePic_32
        Me.BTT_C_OPEN_POST.Name = "BTT_C_OPEN_POST"
        Me.BTT_C_OPEN_POST.Size = New System.Drawing.Size(305, 22)
        Me.BTT_C_OPEN_POST.Text = "Open post"
        '
        'BTT_C_OPEN_PICTURE
        '
        Me.BTT_C_OPEN_PICTURE.Image = Global.SCrawler.My.Resources.Resources.PicturePic_32
        Me.BTT_C_OPEN_PICTURE.Name = "BTT_C_OPEN_PICTURE"
        Me.BTT_C_OPEN_PICTURE.Size = New System.Drawing.Size(305, 22)
        Me.BTT_C_OPEN_PICTURE.Text = "Open picture"
        '
        'BTT_C_OPEN_FOLDER
        '
        Me.BTT_C_OPEN_FOLDER.Image = Global.SCrawler.My.Resources.Resources.FolderPic_32
        Me.BTT_C_OPEN_FOLDER.Name = "BTT_C_OPEN_FOLDER"
        Me.BTT_C_OPEN_FOLDER.Size = New System.Drawing.Size(305, 22)
        Me.BTT_C_OPEN_FOLDER.Text = "Open folder"
        '
        'BTT_C_REMOVE_FROM_SELECTED
        '
        Me.BTT_C_REMOVE_FROM_SELECTED.AutoToolTip = True
        Me.BTT_C_REMOVE_FROM_SELECTED.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
        Me.BTT_C_REMOVE_FROM_SELECTED.Name = "BTT_C_REMOVE_FROM_SELECTED"
        Me.BTT_C_REMOVE_FROM_SELECTED.Size = New System.Drawing.Size(305, 22)
        Me.BTT_C_REMOVE_FROM_SELECTED.Text = "Remove user from selected"
        Me.BTT_C_REMOVE_FROM_SELECTED.ToolTipText = "Remove this user from selected users if user was added to"
        '
        'BTT_C_ADD_TO_BLACKLIST
        '
        Me.BTT_C_ADD_TO_BLACKLIST.Image = Global.SCrawler.My.Resources.Resources.DBPic_32
        Me.BTT_C_ADD_TO_BLACKLIST.Name = "BTT_C_ADD_TO_BLACKLIST"
        Me.BTT_C_ADD_TO_BLACKLIST.Size = New System.Drawing.Size(305, 22)
        Me.BTT_C_ADD_TO_BLACKLIST.Text = "Add/Remove this user to/from the BlackList"
        '
        'ChannelViewForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(744, 461)
        Me.Controls.Add(Me.LIST_POSTS)
        Me.Controls.Add(Me.ToolbarBOTTOM)
        Me.Controls.Add(Me.ToolbarTOP)
        Me.Icon = Global.SCrawler.My.Resources.SiteResources.RedditIcon_128
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(760, 500)
        Me.Name = "ChannelViewForm"
        Me.Text = "Channels"
        Me.ToolbarTOP.ResumeLayout(False)
        Me.ToolbarTOP.PerformLayout()
        Me.ToolbarBOTTOM.ResumeLayout(False)
        Me.ToolbarBOTTOM.PerformLayout()
        Me.LCONTEXT.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents ToolbarTOP As ToolStrip
    Private WithEvents ToolbarBOTTOM As StatusStrip
    Private WithEvents LIST_POSTS As ListView
    Private WithEvents PR_CN As ToolStripProgressBar
    Private WithEvents LBL_STATUS As ToolStripStatusLabel
    Private WithEvents BTT_DOWNLOAD As ToolStripButton
    Private WithEvents TT_MAIN As ToolTip
    Private WithEvents BTT_STOP As ToolStripButton
    Private WithEvents BTT_ADD_USERS As ToolStripButton
    Private WithEvents LCONTEXT As ContextMenuStrip
    Private WithEvents BTT_C_OPEN_USER As ToolStripMenuItem
    Private WithEvents BTT_C_OPEN_POST As ToolStripMenuItem
    Private WithEvents BTT_C_OPEN_PICTURE As ToolStripMenuItem
    Private WithEvents BTT_C_OPEN_FOLDER As ToolStripMenuItem
    Private WithEvents BTT_C_ADD_TO_BLACKLIST As ToolStripMenuItem
    Private WithEvents BTT_C_REMOVE_FROM_SELECTED As ToolStripMenuItem
End Class