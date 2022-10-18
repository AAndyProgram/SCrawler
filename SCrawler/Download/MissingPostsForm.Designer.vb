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
    Partial Friend Class MissingPostsForm : Inherits System.Windows.Forms.Form
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
            Dim CONTAINER_MAIN As System.Windows.Forms.ToolStripContainer
            Dim CONTEXT_SEP_1 As System.Windows.Forms.ToolStripSeparator
            Dim CONTEXT_SEP_2 As System.Windows.Forms.ToolStripSeparator
            Dim CONTEXT_SEP_3 As System.Windows.Forms.ToolStripSeparator
            Me.LIST_DATA = New System.Windows.Forms.ListView()
            Me.CONTEXT_OPERATIONS = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.BTT_DOWN = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_OPEN_POST = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_OPEN_USER = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FIND_USER = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_DELETE = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CONTEXT_SHOW_POST_INFO = New System.Windows.Forms.ToolStripMenuItem()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            CONTEXT_SEP_1 = New System.Windows.Forms.ToolStripSeparator()
            CONTEXT_SEP_2 = New System.Windows.Forms.ToolStripSeparator()
            CONTEXT_SEP_3 = New System.Windows.Forms.ToolStripSeparator()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            Me.CONTEXT_OPERATIONS.SuspendLayout()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            CONTAINER_MAIN.BottomToolStripPanelVisible = False
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(Me.LIST_DATA)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(284, 236)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(284, 261)
            CONTAINER_MAIN.TabIndex = 0
            '
            'LIST_DATA
            '
            Me.LIST_DATA.Activation = System.Windows.Forms.ItemActivation.OneClick
            Me.LIST_DATA.ContextMenuStrip = Me.CONTEXT_OPERATIONS
            Me.LIST_DATA.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LIST_DATA.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
            Me.LIST_DATA.HideSelection = False
            Me.LIST_DATA.Location = New System.Drawing.Point(0, 0)
            Me.LIST_DATA.Name = "LIST_DATA"
            Me.LIST_DATA.Size = New System.Drawing.Size(284, 236)
            Me.LIST_DATA.TabIndex = 0
            Me.LIST_DATA.TileSize = New System.Drawing.Size(168, 15)
            Me.LIST_DATA.UseCompatibleStateImageBehavior = False
            Me.LIST_DATA.View = System.Windows.Forms.View.Tile
            '
            'CONTEXT_SEP_1
            '
            CONTEXT_SEP_1.Name = "CONTEXT_SEP_1"
            CONTEXT_SEP_1.Size = New System.Drawing.Size(178, 6)
            '
            'CONTEXT_SEP_2
            '
            CONTEXT_SEP_2.Name = "CONTEXT_SEP_2"
            CONTEXT_SEP_2.Size = New System.Drawing.Size(178, 6)
            '
            'CONTEXT_SEP_3
            '
            CONTEXT_SEP_3.Name = "CONTEXT_SEP_3"
            CONTEXT_SEP_3.Size = New System.Drawing.Size(178, 6)
            '
            'CONTEXT_OPERATIONS
            '
            Me.CONTEXT_OPERATIONS.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_DOWN, CONTEXT_SEP_1, Me.BTT_OPEN_POST, Me.BTT_OPEN_USER, Me.BTT_CONTEXT_SHOW_POST_INFO, CONTEXT_SEP_2, Me.BTT_FIND_USER, CONTEXT_SEP_3, Me.BTT_DELETE})
            Me.CONTEXT_OPERATIONS.Name = "CONTEXT_OPERATIONS"
            Me.CONTEXT_OPERATIONS.Size = New System.Drawing.Size(181, 176)
            '
            'BTT_DOWN
            '
            Me.BTT_DOWN.AutoToolTip = True
            Me.BTT_DOWN.Image = Global.SCrawler.My.Resources.Resources.StartPic_Green_16
            Me.BTT_DOWN.Name = "BTT_DOWN"
            Me.BTT_DOWN.Size = New System.Drawing.Size(181, 22)
            Me.BTT_DOWN.Text = "Download"
            Me.BTT_DOWN.ToolTipText = "Try downloading the selected posts again"
            '
            'BTT_OPEN_POST
            '
            Me.BTT_OPEN_POST.Image = Global.SCrawler.My.Resources.Resources.GlobePic_32
            Me.BTT_OPEN_POST.Name = "BTT_OPEN_POST"
            Me.BTT_OPEN_POST.Size = New System.Drawing.Size(181, 22)
            Me.BTT_OPEN_POST.Text = "Open post"
            '
            'BTT_OPEN_USER
            '
            Me.BTT_OPEN_USER.Image = Global.SCrawler.My.Resources.Resources.FolderPic_32
            Me.BTT_OPEN_USER.Name = "BTT_OPEN_USER"
            Me.BTT_OPEN_USER.Size = New System.Drawing.Size(181, 22)
            Me.BTT_OPEN_USER.Text = "Open user folder"
            '
            'BTT_FIND_USER
            '
            Me.BTT_FIND_USER.Image = Global.SCrawler.My.Resources.Resources.InfoPic_32
            Me.BTT_FIND_USER.Name = "BTT_FIND_USER"
            Me.BTT_FIND_USER.Size = New System.Drawing.Size(181, 22)
            Me.BTT_FIND_USER.Text = "Find user"
            '
            'BTT_DELETE
            '
            Me.BTT_DELETE.AutoToolTip = True
            Me.BTT_DELETE.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
            Me.BTT_DELETE.Name = "BTT_DELETE"
            Me.BTT_DELETE.Size = New System.Drawing.Size(181, 22)
            Me.BTT_DELETE.Text = "Delete post"
            Me.BTT_DELETE.ToolTipText = "Remove selected posts from user data"
            '
            'BTT_CONTEXT_SHOW_POST_INFO
            '
            Me.BTT_CONTEXT_SHOW_POST_INFO.AutoToolTip = True
            Me.BTT_CONTEXT_SHOW_POST_INFO.Image = Global.SCrawler.My.Resources.Resources.InfoPic_32
            Me.BTT_CONTEXT_SHOW_POST_INFO.Name = "BTT_CONTEXT_SHOW_POST_INFO"
            Me.BTT_CONTEXT_SHOW_POST_INFO.Size = New System.Drawing.Size(180, 22)
            Me.BTT_CONTEXT_SHOW_POST_INFO.Text = "Show post info"
            Me.BTT_CONTEXT_SHOW_POST_INFO.ToolTipText = "Show information about the missing post"
            '
            'MissingPostsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(284, 261)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.KeyPreview = True
            Me.MinimumSize = New System.Drawing.Size(300, 300)
            Me.Name = "MissingPostsForm"
            Me.ShowIcon = False
            Me.Text = "Missing posts"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            Me.CONTEXT_OPERATIONS.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents LIST_DATA As ListView
        Private WithEvents CONTEXT_OPERATIONS As ContextMenuStrip
        Private WithEvents BTT_DOWN As ToolStripMenuItem
        Private WithEvents BTT_OPEN_POST As ToolStripMenuItem
        Private WithEvents BTT_OPEN_USER As ToolStripMenuItem
        Private WithEvents BTT_FIND_USER As ToolStripMenuItem
        Private WithEvents BTT_DELETE As ToolStripMenuItem
        Private WithEvents BTT_CONTEXT_SHOW_POST_INFO As ToolStripMenuItem
    End Class
End Namespace