' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace DownloadObjects.STDownloader
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Public Class MediaItem : Inherits System.Windows.Forms.UserControl
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
            Dim TP_MAIN As System.Windows.Forms.TableLayoutPanel
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MediaItem))
            Me.ICON_VIDEO = New System.Windows.Forms.PictureBox()
            Me.CONTEXT_MAIN = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.BTT_DOWN = New System.Windows.Forms.ToolStripMenuItem()
            Me.SEP_DOWN = New System.Windows.Forms.ToolStripSeparator()
            Me.BTT_OPEN_FOLDER = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_OPEN_FILE = New System.Windows.Forms.ToolStripMenuItem()
            Me.SEP_FOLDER = New System.Windows.Forms.ToolStripSeparator()
            Me.BTT_PLS_ITEM_EDIT = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_PLS_ITEM_EDIT_FULL = New System.Windows.Forms.ToolStripMenuItem()
            Me.SEP_PLS_ITEM_EDIT = New System.Windows.Forms.ToolStripSeparator()
            Me.BTT_COPY_LINK = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_OPEN_IN_BROWSER = New System.Windows.Forms.ToolStripMenuItem()
            Me.SEP_DOWN_AGAIN = New System.Windows.Forms.ToolStripSeparator()
            Me.BTT_DOWN_AGAIN = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_VIEW_SETTINGS = New System.Windows.Forms.ToolStripMenuItem()
            Me.SEP_DEL = New System.Windows.Forms.ToolStripSeparator()
            Me.BTT_REMOVE_FROM_LIST = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_DELETE_FILE = New System.Windows.Forms.ToolStripMenuItem()
            Me.TP_INFO = New System.Windows.Forms.TableLayoutPanel()
            Me.TP_CHECKED_TITLE = New System.Windows.Forms.TableLayoutPanel()
            Me.LBL_TITLE = New System.Windows.Forms.Label()
            Me.CH_CHECKED = New System.Windows.Forms.CheckBox()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            TP_MAIN.SuspendLayout()
            CType(Me.ICON_VIDEO, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.CONTEXT_MAIN.SuspendLayout()
            Me.TP_INFO.SuspendLayout()
            Me.TP_CHECKED_TITLE.SuspendLayout()
            Me.SuspendLayout()
            '
            'TP_MAIN
            '
            TP_MAIN.ColumnCount = 2
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125.0!))
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(Me.ICON_VIDEO, 0, 0)
            TP_MAIN.Controls.Add(Me.TP_INFO, 1, 0)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Margin = New System.Windows.Forms.Padding(0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 1
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65.0!))
            TP_MAIN.Size = New System.Drawing.Size(549, 65)
            TP_MAIN.TabIndex = 0
            AddHandler TP_MAIN.Click, AddressOf Me.Controls_Click
            '
            'ICON_VIDEO
            '
            Me.ICON_VIDEO.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
            Me.ICON_VIDEO.ContextMenuStrip = Me.CONTEXT_MAIN
            Me.ICON_VIDEO.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ICON_VIDEO.Location = New System.Drawing.Point(3, 3)
            Me.ICON_VIDEO.Name = "ICON_VIDEO"
            Me.ICON_VIDEO.Size = New System.Drawing.Size(119, 59)
            Me.ICON_VIDEO.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.ICON_VIDEO.TabIndex = 0
            Me.ICON_VIDEO.TabStop = False
            '
            'CONTEXT_MAIN
            '
            Me.CONTEXT_MAIN.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_DOWN, Me.SEP_DOWN, Me.BTT_OPEN_FOLDER, Me.BTT_OPEN_FILE, Me.SEP_FOLDER, Me.BTT_PLS_ITEM_EDIT, Me.BTT_PLS_ITEM_EDIT_FULL, Me.SEP_PLS_ITEM_EDIT, Me.BTT_COPY_LINK, Me.BTT_OPEN_IN_BROWSER, Me.SEP_DOWN_AGAIN, Me.BTT_DOWN_AGAIN, Me.BTT_VIEW_SETTINGS, Me.SEP_DEL, Me.BTT_REMOVE_FROM_LIST, Me.BTT_DELETE_FILE})
            Me.CONTEXT_MAIN.Name = "CONTEXT_MAIN"
            Me.CONTEXT_MAIN.ShowItemToolTips = False
            Me.CONTEXT_MAIN.Size = New System.Drawing.Size(185, 298)
            '
            'BTT_DOWN
            '
            Me.BTT_DOWN.Image = Global.SCrawler.My.Resources.Resources.ArrowDownPic_Blue_24
            Me.BTT_DOWN.Name = "BTT_DOWN"
            Me.BTT_DOWN.Size = New System.Drawing.Size(184, 22)
            Me.BTT_DOWN.Text = "Download"
            '
            'SEP_DOWN
            '
            Me.SEP_DOWN.Name = "SEP_DOWN"
            Me.SEP_DOWN.Size = New System.Drawing.Size(181, 6)
            '
            'BTT_OPEN_FOLDER
            '
            Me.BTT_OPEN_FOLDER.Image = CType(resources.GetObject("BTT_OPEN_FOLDER.Image"), System.Drawing.Image)
            Me.BTT_OPEN_FOLDER.Name = "BTT_OPEN_FOLDER"
            Me.BTT_OPEN_FOLDER.Size = New System.Drawing.Size(184, 22)
            Me.BTT_OPEN_FOLDER.Text = "Open folder"
            '
            'BTT_OPEN_FILE
            '
            Me.BTT_OPEN_FILE.Image = CType(resources.GetObject("BTT_OPEN_FILE.Image"), System.Drawing.Image)
            Me.BTT_OPEN_FILE.Name = "BTT_OPEN_FILE"
            Me.BTT_OPEN_FILE.Size = New System.Drawing.Size(184, 22)
            Me.BTT_OPEN_FILE.Text = "Open file"
            '
            'SEP_FOLDER
            '
            Me.SEP_FOLDER.Name = "SEP_FOLDER"
            Me.SEP_FOLDER.Size = New System.Drawing.Size(181, 6)
            '
            'BTT_PLS_ITEM_EDIT
            '
            Me.BTT_PLS_ITEM_EDIT.Image = CType(resources.GetObject("BTT_PLS_ITEM_EDIT.Image"), System.Drawing.Image)
            Me.BTT_PLS_ITEM_EDIT.Name = "BTT_PLS_ITEM_EDIT"
            Me.BTT_PLS_ITEM_EDIT.Size = New System.Drawing.Size(184, 22)
            Me.BTT_PLS_ITEM_EDIT.Text = "Edit"
            Me.BTT_PLS_ITEM_EDIT.Visible = False
            '
            'BTT_PLS_ITEM_EDIT_FULL
            '
            Me.BTT_PLS_ITEM_EDIT_FULL.AutoToolTip = True
            Me.BTT_PLS_ITEM_EDIT_FULL.Image = CType(resources.GetObject("BTT_PLS_ITEM_EDIT_FULL.Image"), System.Drawing.Image)
            Me.BTT_PLS_ITEM_EDIT_FULL.Name = "BTT_PLS_ITEM_EDIT_FULL"
            Me.BTT_PLS_ITEM_EDIT_FULL.Size = New System.Drawing.Size(184, 22)
            Me.BTT_PLS_ITEM_EDIT_FULL.Text = "Edit (inherit settings)"
            Me.BTT_PLS_ITEM_EDIT_FULL.ToolTipText = "Inherit settings selected in the form"
            Me.BTT_PLS_ITEM_EDIT_FULL.Visible = False
            '
            'SEP_PLS_ITEM_EDIT
            '
            Me.SEP_PLS_ITEM_EDIT.Name = "SEP_PLS_ITEM_EDIT"
            Me.SEP_PLS_ITEM_EDIT.Size = New System.Drawing.Size(181, 6)
            Me.SEP_PLS_ITEM_EDIT.Visible = False
            '
            'BTT_COPY_LINK
            '
            Me.BTT_COPY_LINK.Image = Global.SCrawler.My.Resources.Resources.LinkPic_32
            Me.BTT_COPY_LINK.Name = "BTT_COPY_LINK"
            Me.BTT_COPY_LINK.Size = New System.Drawing.Size(184, 22)
            Me.BTT_COPY_LINK.Text = "Copy link address"
            '
            'BTT_OPEN_IN_BROWSER
            '
            Me.BTT_OPEN_IN_BROWSER.Image = CType(resources.GetObject("BTT_OPEN_IN_BROWSER.Image"), System.Drawing.Image)
            Me.BTT_OPEN_IN_BROWSER.Name = "BTT_OPEN_IN_BROWSER"
            Me.BTT_OPEN_IN_BROWSER.Size = New System.Drawing.Size(184, 22)
            Me.BTT_OPEN_IN_BROWSER.Text = "Open link in browser"
            '
            'SEP_DOWN_AGAIN
            '
            Me.SEP_DOWN_AGAIN.Name = "SEP_DOWN_AGAIN"
            Me.SEP_DOWN_AGAIN.Size = New System.Drawing.Size(181, 6)
            '
            'BTT_DOWN_AGAIN
            '
            Me.BTT_DOWN_AGAIN.Image = CType(resources.GetObject("BTT_DOWN_AGAIN.Image"), System.Drawing.Image)
            Me.BTT_DOWN_AGAIN.Name = "BTT_DOWN_AGAIN"
            Me.BTT_DOWN_AGAIN.Size = New System.Drawing.Size(184, 22)
            Me.BTT_DOWN_AGAIN.Text = "Download again"
            '
            'BTT_VIEW_SETTINGS
            '
            Me.BTT_VIEW_SETTINGS.Image = Global.SCrawler.My.Resources.Resources.SettingsPic_16
            Me.BTT_VIEW_SETTINGS.Name = "BTT_VIEW_SETTINGS"
            Me.BTT_VIEW_SETTINGS.Size = New System.Drawing.Size(184, 22)
            Me.BTT_VIEW_SETTINGS.Text = "View settings"
            '
            'SEP_DEL
            '
            Me.SEP_DEL.Name = "SEP_DEL"
            Me.SEP_DEL.Size = New System.Drawing.Size(181, 6)
            '
            'BTT_REMOVE_FROM_LIST
            '
            Me.BTT_REMOVE_FROM_LIST.Image = CType(resources.GetObject("BTT_REMOVE_FROM_LIST.Image"), System.Drawing.Image)
            Me.BTT_REMOVE_FROM_LIST.Name = "BTT_REMOVE_FROM_LIST"
            Me.BTT_REMOVE_FROM_LIST.Size = New System.Drawing.Size(184, 22)
            Me.BTT_REMOVE_FROM_LIST.Text = "Remove from the list"
            '
            'BTT_DELETE_FILE
            '
            Me.BTT_DELETE_FILE.Image = CType(resources.GetObject("BTT_DELETE_FILE.Image"), System.Drawing.Image)
            Me.BTT_DELETE_FILE.Name = "BTT_DELETE_FILE"
            Me.BTT_DELETE_FILE.Size = New System.Drawing.Size(184, 22)
            Me.BTT_DELETE_FILE.Text = "Delete file"
            '
            'TP_INFO
            '
            Me.TP_INFO.ColumnCount = 1
            Me.TP_INFO.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_INFO.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TP_INFO.Controls.Add(Me.TP_CHECKED_TITLE, 0, 0)
            Me.TP_INFO.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_INFO.Location = New System.Drawing.Point(125, 0)
            Me.TP_INFO.Margin = New System.Windows.Forms.Padding(0)
            Me.TP_INFO.Name = "TP_INFO"
            Me.TP_INFO.RowCount = 2
            Me.TP_INFO.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TP_INFO.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TP_INFO.Size = New System.Drawing.Size(424, 65)
            Me.TP_INFO.TabIndex = 1
            '
            'TP_CHECKED_TITLE
            '
            Me.TP_CHECKED_TITLE.ColumnCount = 2
            Me.TP_CHECKED_TITLE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TP_CHECKED_TITLE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_CHECKED_TITLE.Controls.Add(Me.LBL_TITLE, 1, 0)
            Me.TP_CHECKED_TITLE.Controls.Add(Me.CH_CHECKED, 0, 0)
            Me.TP_CHECKED_TITLE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_CHECKED_TITLE.Location = New System.Drawing.Point(0, 0)
            Me.TP_CHECKED_TITLE.Margin = New System.Windows.Forms.Padding(0)
            Me.TP_CHECKED_TITLE.Name = "TP_CHECKED_TITLE"
            Me.TP_CHECKED_TITLE.RowCount = 1
            Me.TP_CHECKED_TITLE.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_CHECKED_TITLE.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32.0!))
            Me.TP_CHECKED_TITLE.Size = New System.Drawing.Size(424, 32)
            Me.TP_CHECKED_TITLE.TabIndex = 0
            '
            'LBL_TITLE
            '
            Me.LBL_TITLE.ContextMenuStrip = Me.CONTEXT_MAIN
            Me.LBL_TITLE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LBL_TITLE.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
            Me.LBL_TITLE.Location = New System.Drawing.Point(23, 0)
            Me.LBL_TITLE.Name = "LBL_TITLE"
            Me.LBL_TITLE.Size = New System.Drawing.Size(398, 32)
            Me.LBL_TITLE.TabIndex = 1
            Me.LBL_TITLE.Text = "Video title"
            Me.LBL_TITLE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'CH_CHECKED
            '
            Me.CH_CHECKED.AutoSize = True
            Me.CH_CHECKED.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_CHECKED.Location = New System.Drawing.Point(3, 3)
            Me.CH_CHECKED.Name = "CH_CHECKED"
            Me.CH_CHECKED.Size = New System.Drawing.Size(14, 26)
            Me.CH_CHECKED.TabIndex = 0
            Me.CH_CHECKED.UseVisualStyleBackColor = True
            '
            'MediaItem
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(TP_MAIN)
            Me.Name = "MediaItem"
            Me.Size = New System.Drawing.Size(549, 65)
            TP_MAIN.ResumeLayout(False)
            CType(Me.ICON_VIDEO, System.ComponentModel.ISupportInitialize).EndInit()
            Me.CONTEXT_MAIN.ResumeLayout(False)
            Me.TP_INFO.ResumeLayout(False)
            Me.TP_CHECKED_TITLE.ResumeLayout(False)
            Me.TP_CHECKED_TITLE.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents ICON_VIDEO As PictureBox
        Private WithEvents LBL_TITLE As Label
        Private WithEvents CONTEXT_MAIN As ContextMenuStrip
        Private WithEvents BTT_OPEN_FOLDER As ToolStripMenuItem
        Private WithEvents BTT_COPY_LINK As ToolStripMenuItem
        Private WithEvents BTT_OPEN_IN_BROWSER As ToolStripMenuItem
        Private WithEvents BTT_DOWN_AGAIN As ToolStripMenuItem
        Private WithEvents BTT_REMOVE_FROM_LIST As ToolStripMenuItem
        Private WithEvents BTT_DELETE_FILE As ToolStripMenuItem
        Private WithEvents BTT_DOWN As ToolStripMenuItem
        Private WithEvents SEP_DOWN As ToolStripSeparator
        Private WithEvents TP_INFO As TableLayoutPanel
        Friend WithEvents TP_CHECKED_TITLE As TableLayoutPanel
        Private WithEvents CH_CHECKED As CheckBox
        Private WithEvents SEP_FOLDER As ToolStripSeparator
        Private WithEvents SEP_DOWN_AGAIN As ToolStripSeparator
        Private WithEvents SEP_DEL As ToolStripSeparator
        Private WithEvents BTT_VIEW_SETTINGS As ToolStripMenuItem
        Private WithEvents BTT_OPEN_FILE As ToolStripMenuItem
        Private WithEvents BTT_PLS_ITEM_EDIT As ToolStripMenuItem
        Private WithEvents SEP_PLS_ITEM_EDIT As ToolStripSeparator
        Private WithEvents BTT_PLS_ITEM_EDIT_FULL As ToolStripMenuItem
    End Class
End Namespace