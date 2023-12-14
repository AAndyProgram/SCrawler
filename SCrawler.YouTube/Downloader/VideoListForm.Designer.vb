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
    Partial Public Class VideoListForm : Inherits System.Windows.Forms.Form
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
            Dim SEP_2 As System.Windows.Forms.ToolStripSeparator
            Dim SEP_3 As System.Windows.Forms.ToolStripSeparator
            Dim MENU_DEL_CLEAR As System.Windows.Forms.ToolStripDropDownButton
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(VideoListForm))
            Dim MENU_DEL_SEP_1 As System.Windows.Forms.ToolStripSeparator
            Dim MENU_DEL_SEP_2 As System.Windows.Forms.ToolStripSeparator
            Me.BTT_DELETE = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CLEAR_SELECTED = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CLEAR_DONE = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CLEAR_ALL = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_SELECT_ALL = New System.Windows.Forms.ToolStripMenuItem()
            Me.TOOLBAR_BOTTOM = New System.Windows.Forms.StatusStrip()
            Me.PR_MAIN = New System.Windows.Forms.ToolStripProgressBar()
            Me.LBL_INFO = New System.Windows.Forms.ToolStripStatusLabel()
            Me.TP_CONTROLS = New System.Windows.Forms.TableLayoutPanel()
            Me.TOOLBAR_TOP = New System.Windows.Forms.ToolStrip()
            Me.BTT_SETTINGS = New System.Windows.Forms.ToolStripButton()
            Me.SEP_1 = New System.Windows.Forms.ToolStripSeparator()
            Me.MENU_ADD = New System.Windows.Forms.ToolStripDropDownButton()
            Me.BTT_ADD = New PersonalUtilities.Forms.Controls.KeyClick.ToolStripMenuItemKeyClick()
            Me.BTT_ADD_PLS_ARR = New PersonalUtilities.Forms.Controls.KeyClick.ToolStripMenuItemKeyClick()
            Me.BTT_DOWN = New System.Windows.Forms.ToolStripButton()
            Me.BTT_STOP = New System.Windows.Forms.ToolStripButton()
            Me.SEP_LOG = New System.Windows.Forms.ToolStripSeparator()
            Me.BTT_LOG = New System.Windows.Forms.ToolStripButton()
            Me.BTT_INFO = New System.Windows.Forms.ToolStripButton()
            Me.BTT_DONATE = New System.Windows.Forms.ToolStripButton()
            Me.BTT_BUG_REPORT = New System.Windows.Forms.ToolStripButton()
            Me.BTT_SELECT_NONE = New System.Windows.Forms.ToolStripMenuItem()
            SEP_2 = New System.Windows.Forms.ToolStripSeparator()
            SEP_3 = New System.Windows.Forms.ToolStripSeparator()
            MENU_DEL_CLEAR = New System.Windows.Forms.ToolStripDropDownButton()
            MENU_DEL_SEP_1 = New System.Windows.Forms.ToolStripSeparator()
            MENU_DEL_SEP_2 = New System.Windows.Forms.ToolStripSeparator()
            Me.TOOLBAR_BOTTOM.SuspendLayout()
            Me.TOOLBAR_TOP.SuspendLayout()
            Me.SuspendLayout()
            '
            'SEP_2
            '
            SEP_2.Name = "SEP_2"
            SEP_2.Size = New System.Drawing.Size(6, 25)
            '
            'SEP_3
            '
            SEP_3.Name = "SEP_3"
            SEP_3.Size = New System.Drawing.Size(6, 25)
            '
            'MENU_DEL_CLEAR
            '
            MENU_DEL_CLEAR.AutoToolTip = False
            MENU_DEL_CLEAR.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_DELETE, MENU_DEL_SEP_1, Me.BTT_CLEAR_SELECTED, Me.BTT_CLEAR_DONE, Me.BTT_CLEAR_ALL, MENU_DEL_SEP_2, Me.BTT_SELECT_ALL, Me.BTT_SELECT_NONE})
            MENU_DEL_CLEAR.Image = CType(resources.GetObject("MENU_DEL_CLEAR.Image"), System.Drawing.Image)
            MENU_DEL_CLEAR.ImageTransparentColor = System.Drawing.Color.Magenta
            MENU_DEL_CLEAR.Name = "MENU_DEL_CLEAR"
            MENU_DEL_CLEAR.Size = New System.Drawing.Size(107, 22)
            MENU_DEL_CLEAR.Text = "Delete / Clear"
            '
            'BTT_DELETE
            '
            Me.BTT_DELETE.Image = CType(resources.GetObject("BTT_DELETE.Image"), System.Drawing.Image)
            Me.BTT_DELETE.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_DELETE.Name = "BTT_DELETE"
            Me.BTT_DELETE.Size = New System.Drawing.Size(185, 22)
            Me.BTT_DELETE.Text = "Delete selected items"
            Me.BTT_DELETE.ToolTipText = "Delete selected items"
            '
            'MENU_DEL_SEP_1
            '
            MENU_DEL_SEP_1.Name = "MENU_DEL_SEP_1"
            MENU_DEL_SEP_1.Size = New System.Drawing.Size(182, 6)
            '
            'BTT_CLEAR_SELECTED
            '
            Me.BTT_CLEAR_SELECTED.AutoToolTip = True
            Me.BTT_CLEAR_SELECTED.Image = CType(resources.GetObject("BTT_CLEAR_SELECTED.Image"), System.Drawing.Image)
            Me.BTT_CLEAR_SELECTED.Name = "BTT_CLEAR_SELECTED"
            Me.BTT_CLEAR_SELECTED.Size = New System.Drawing.Size(185, 22)
            Me.BTT_CLEAR_SELECTED.Text = "Clear selected"
            Me.BTT_CLEAR_SELECTED.ToolTipText = "Remove all checked items from the list"
            '
            'BTT_CLEAR_DONE
            '
            Me.BTT_CLEAR_DONE.AutoToolTip = True
            Me.BTT_CLEAR_DONE.Image = CType(resources.GetObject("BTT_CLEAR_DONE.Image"), System.Drawing.Image)
            Me.BTT_CLEAR_DONE.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_CLEAR_DONE.Name = "BTT_CLEAR_DONE"
            Me.BTT_CLEAR_DONE.Size = New System.Drawing.Size(185, 22)
            Me.BTT_CLEAR_DONE.Text = "Clear downloaded"
            Me.BTT_CLEAR_DONE.ToolTipText = "Remove all downloaded items from the list"
            '
            'BTT_CLEAR_ALL
            '
            Me.BTT_CLEAR_ALL.AutoToolTip = True
            Me.BTT_CLEAR_ALL.Image = CType(resources.GetObject("BTT_CLEAR_ALL.Image"), System.Drawing.Image)
            Me.BTT_CLEAR_ALL.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_CLEAR_ALL.Name = "BTT_CLEAR_ALL"
            Me.BTT_CLEAR_ALL.Size = New System.Drawing.Size(185, 22)
            Me.BTT_CLEAR_ALL.Text = "Clear all"
            Me.BTT_CLEAR_ALL.ToolTipText = "Remove all items from the list"
            '
            'MENU_DEL_SEP_2
            '
            MENU_DEL_SEP_2.Name = "MENU_DEL_SEP_2"
            MENU_DEL_SEP_2.Size = New System.Drawing.Size(182, 6)
            '
            'BTT_SELECT_ALL
            '
            Me.BTT_SELECT_ALL.Name = "BTT_SELECT_ALL"
            Me.BTT_SELECT_ALL.Size = New System.Drawing.Size(185, 22)
            Me.BTT_SELECT_ALL.Text = "Select all"
            '
            'TOOLBAR_BOTTOM
            '
            Me.TOOLBAR_BOTTOM.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PR_MAIN, Me.LBL_INFO})
            Me.TOOLBAR_BOTTOM.Location = New System.Drawing.Point(0, 439)
            Me.TOOLBAR_BOTTOM.Name = "TOOLBAR_BOTTOM"
            Me.TOOLBAR_BOTTOM.Size = New System.Drawing.Size(584, 22)
            Me.TOOLBAR_BOTTOM.TabIndex = 0
            '
            'PR_MAIN
            '
            Me.PR_MAIN.Name = "PR_MAIN"
            Me.PR_MAIN.Size = New System.Drawing.Size(200, 16)
            '
            'LBL_INFO
            '
            Me.LBL_INFO.Name = "LBL_INFO"
            Me.LBL_INFO.Size = New System.Drawing.Size(0, 17)
            '
            'TP_CONTROLS
            '
            Me.TP_CONTROLS.AutoScroll = True
            Me.TP_CONTROLS.BackColor = System.Drawing.SystemColors.Window
            Me.TP_CONTROLS.ColumnCount = 1
            Me.TP_CONTROLS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_CONTROLS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_CONTROLS.Location = New System.Drawing.Point(0, 25)
            Me.TP_CONTROLS.Name = "TP_CONTROLS"
            Me.TP_CONTROLS.RowCount = 1
            Me.TP_CONTROLS.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_CONTROLS.Size = New System.Drawing.Size(584, 414)
            Me.TP_CONTROLS.TabIndex = 0
            '
            'TOOLBAR_TOP
            '
            Me.TOOLBAR_TOP.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
            Me.TOOLBAR_TOP.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_SETTINGS, Me.SEP_1, Me.MENU_ADD, SEP_2, Me.BTT_DOWN, Me.BTT_STOP, SEP_3, MENU_DEL_CLEAR, Me.SEP_LOG, Me.BTT_LOG, Me.BTT_INFO, Me.BTT_DONATE, Me.BTT_BUG_REPORT})
            Me.TOOLBAR_TOP.Location = New System.Drawing.Point(0, 0)
            Me.TOOLBAR_TOP.Name = "TOOLBAR_TOP"
            Me.TOOLBAR_TOP.Size = New System.Drawing.Size(584, 25)
            Me.TOOLBAR_TOP.TabIndex = 2
            '
            'BTT_SETTINGS
            '
            Me.BTT_SETTINGS.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.BTT_SETTINGS.Image = Global.SCrawler.My.Resources.Resources.SettingsPic_16
            Me.BTT_SETTINGS.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_SETTINGS.Name = "BTT_SETTINGS"
            Me.BTT_SETTINGS.Size = New System.Drawing.Size(23, 22)
            Me.BTT_SETTINGS.Text = "Settings"
            '
            'SEP_1
            '
            Me.SEP_1.Name = "SEP_1"
            Me.SEP_1.Size = New System.Drawing.Size(6, 25)
            '
            'MENU_ADD
            '
            Me.MENU_ADD.AutoToolTip = False
            Me.MENU_ADD.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_ADD, Me.BTT_ADD_PLS_ARR})
            Me.MENU_ADD.Image = CType(resources.GetObject("MENU_ADD.Image"), System.Drawing.Image)
            Me.MENU_ADD.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.MENU_ADD.Name = "MENU_ADD"
            Me.MENU_ADD.Size = New System.Drawing.Size(84, 22)
            Me.MENU_ADD.Text = "Add (Ins)"
            '
            'BTT_ADD
            '
            Me.BTT_ADD.AutoToolTip = True
            Me.BTT_ADD.Image = CType(resources.GetObject("BTT_ADD.Image"), System.Drawing.Image)
            Me.BTT_ADD.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_ADD.Name = "BTT_ADD"
            Me.BTT_ADD.Size = New System.Drawing.Size(149, 22)
            Me.BTT_ADD.Tag = "a"
            Me.BTT_ADD.Text = "Add (Ins)"
            Me.BTT_ADD.ToolTipText = "Click to add." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Ctrl+click to use cookies for download (if supported)." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Shift to a" &
    "dd without downloading."
            '
            'BTT_ADD_PLS_ARR
            '
            Me.BTT_ADD_PLS_ARR.AutoToolTip = True
            Me.BTT_ADD_PLS_ARR.Image = CType(resources.GetObject("BTT_ADD_PLS_ARR.Image"), System.Drawing.Image)
            Me.BTT_ADD_PLS_ARR.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_ADD_PLS_ARR.Name = "BTT_ADD_PLS_ARR"
            Me.BTT_ADD_PLS_ARR.Size = New System.Drawing.Size(149, 22)
            Me.BTT_ADD_PLS_ARR.Tag = "pls"
            Me.BTT_ADD_PLS_ARR.Text = "Add URL array"
            Me.BTT_ADD_PLS_ARR.ToolTipText = "Click to add." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Ctrl+click to use cookies for download (if supported)." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Shift to a" &
    "dd without downloading."
            '
            'BTT_DOWN
            '
            Me.BTT_DOWN.Image = Global.SCrawler.My.Resources.Resources.StartPic_Green_16
            Me.BTT_DOWN.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_DOWN.Name = "BTT_DOWN"
            Me.BTT_DOWN.Size = New System.Drawing.Size(81, 22)
            Me.BTT_DOWN.Text = "Download"
            Me.BTT_DOWN.ToolTipText = "Download pending items (F5)"
            '
            'BTT_STOP
            '
            Me.BTT_STOP.AutoToolTip = False
            Me.BTT_STOP.Enabled = False
            Me.BTT_STOP.Image = CType(resources.GetObject("BTT_STOP.Image"), System.Drawing.Image)
            Me.BTT_STOP.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_STOP.Name = "BTT_STOP"
            Me.BTT_STOP.Size = New System.Drawing.Size(51, 22)
            Me.BTT_STOP.Text = "Stop"
            '
            'SEP_LOG
            '
            Me.SEP_LOG.Name = "SEP_LOG"
            Me.SEP_LOG.Size = New System.Drawing.Size(6, 25)
            '
            'BTT_LOG
            '
            Me.BTT_LOG.AutoToolTip = False
            Me.BTT_LOG.Image = CType(resources.GetObject("BTT_LOG.Image"), System.Drawing.Image)
            Me.BTT_LOG.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_LOG.Name = "BTT_LOG"
            Me.BTT_LOG.Size = New System.Drawing.Size(50, 22)
            Me.BTT_LOG.Text = "LOG"
            '
            'BTT_INFO
            '
            Me.BTT_INFO.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
            Me.BTT_INFO.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.BTT_INFO.Image = Global.SCrawler.My.Resources.Resources.InfoPic_32
            Me.BTT_INFO.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_INFO.Name = "BTT_INFO"
            Me.BTT_INFO.Size = New System.Drawing.Size(23, 22)
            Me.BTT_INFO.ToolTipText = "Show program information and check for updates"
            '
            'BTT_DONATE
            '
            Me.BTT_DONATE.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
            Me.BTT_DONATE.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.BTT_DONATE.Image = Global.SCrawler.My.Resources.Resources.HeartPic_32
            Me.BTT_DONATE.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_DONATE.Name = "BTT_DONATE"
            Me.BTT_DONATE.Size = New System.Drawing.Size(23, 22)
            Me.BTT_DONATE.ToolTipText = "Support"
            '
            'BTT_BUG_REPORT
            '
            Me.BTT_BUG_REPORT.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
            Me.BTT_BUG_REPORT.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.BTT_BUG_REPORT.Image = Global.SCrawler.My.Resources.Resources.MailPic_16
            Me.BTT_BUG_REPORT.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_BUG_REPORT.Name = "BTT_BUG_REPORT"
            Me.BTT_BUG_REPORT.Size = New System.Drawing.Size(23, 22)
            Me.BTT_BUG_REPORT.Text = "Bug report"
            '
            'BTT_SELECT_NONE
            '
            Me.BTT_SELECT_NONE.Name = "BTT_SELECT_NONE"
            Me.BTT_SELECT_NONE.Size = New System.Drawing.Size(185, 22)
            Me.BTT_SELECT_NONE.Text = "Select none"
            '
            'VideoListForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(584, 461)
            Me.Controls.Add(Me.TP_CONTROLS)
            Me.Controls.Add(Me.TOOLBAR_TOP)
            Me.Controls.Add(Me.TOOLBAR_BOTTOM)
            Me.Icon = Global.SCrawler.My.Resources.SiteYouTube.YouTubeIcon_32
            Me.KeyPreview = True
            Me.MinimumSize = New System.Drawing.Size(300, 200)
            Me.Name = "VideoListForm"
            Me.Text = "YouTube Downloader"
            Me.TOOLBAR_BOTTOM.ResumeLayout(False)
            Me.TOOLBAR_BOTTOM.PerformLayout()
            Me.TOOLBAR_TOP.ResumeLayout(False)
            Me.TOOLBAR_TOP.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Private WithEvents TOOLBAR_BOTTOM As StatusStrip
        Private WithEvents PR_MAIN As ToolStripProgressBar
        Private WithEvents LBL_INFO As ToolStripStatusLabel
        Protected WithEvents TP_CONTROLS As TableLayoutPanel
        Private WithEvents TOOLBAR_TOP As ToolStrip
        Private WithEvents BTT_DELETE As ToolStripMenuItem
        Private WithEvents BTT_CLEAR_DONE As ToolStripMenuItem
        Private WithEvents BTT_CLEAR_ALL As ToolStripMenuItem
        Private WithEvents BTT_SETTINGS As ToolStripButton
        Private WithEvents SEP_1 As ToolStripSeparator
        Private WithEvents SEP_LOG As ToolStripSeparator
        Private WithEvents BTT_LOG As ToolStripButton
        Private WithEvents BTT_STOP As ToolStripButton
        Private WithEvents BTT_INFO As ToolStripButton
        Private WithEvents BTT_DONATE As ToolStripButton
        Protected WithEvents BTT_ADD As PersonalUtilities.Forms.Controls.KeyClick.ToolStripMenuItemKeyClick
        Protected WithEvents BTT_ADD_PLS_ARR As PersonalUtilities.Forms.Controls.KeyClick.ToolStripMenuItemKeyClick
        Protected WithEvents MENU_ADD As ToolStripDropDownButton
        Protected WithEvents BTT_DOWN As ToolStripButton
        Private WithEvents BTT_BUG_REPORT As ToolStripButton
        Private WithEvents BTT_CLEAR_SELECTED As ToolStripMenuItem
        Private WithEvents BTT_SELECT_ALL As ToolStripMenuItem
        Private WithEvents BTT_SELECT_NONE As ToolStripMenuItem
    End Class
End Namespace