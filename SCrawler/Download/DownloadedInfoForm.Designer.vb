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
    Partial Friend Class DownloadedInfoForm : Inherits System.Windows.Forms.Form
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
            Dim SEP_1 As System.Windows.Forms.ToolStripSeparator
            Dim SEP_2 As System.Windows.Forms.ToolStripSeparator
            Dim MENU_VIEW_SEP_1 As System.Windows.Forms.ToolStripSeparator
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DownloadedInfoForm))
            Me.ToolbarTOP = New System.Windows.Forms.ToolStrip()
            Me.MENU_VIEW = New System.Windows.Forms.ToolStripDropDownButton()
            Me.MENU_VIEW_SESSION = New System.Windows.Forms.ToolStripMenuItem()
            Me.MENU_VIEW_ALL = New System.Windows.Forms.ToolStripMenuItem()
            Me.OPT_DEFAULT = New System.Windows.Forms.ToolStripMenuItem()
            Me.OPT_SUBSCRIPTIONS = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_REFRESH = New System.Windows.Forms.ToolStripButton()
            Me.BTT_UP = New System.Windows.Forms.ToolStripButton()
            Me.BTT_DOWN = New System.Windows.Forms.ToolStripButton()
            Me.BTT_FIND = New System.Windows.Forms.ToolStripButton()
            Me.BTT_CLEAR = New System.Windows.Forms.ToolStripButton()
            Me.ToolbarBOTTOM = New System.Windows.Forms.StatusStrip()
            Me.LIST_DOWN = New System.Windows.Forms.ListBox()
            SEP_1 = New System.Windows.Forms.ToolStripSeparator()
            SEP_2 = New System.Windows.Forms.ToolStripSeparator()
            MENU_VIEW_SEP_1 = New System.Windows.Forms.ToolStripSeparator()
            Me.ToolbarTOP.SuspendLayout()
            Me.SuspendLayout()
            '
            'SEP_1
            '
            SEP_1.Name = "SEP_1"
            SEP_1.Size = New System.Drawing.Size(6, 25)
            '
            'SEP_2
            '
            SEP_2.Name = "SEP_2"
            SEP_2.Size = New System.Drawing.Size(6, 25)
            '
            'MENU_VIEW_SEP_1
            '
            MENU_VIEW_SEP_1.Name = "MENU_VIEW_SEP_1"
            MENU_VIEW_SEP_1.Size = New System.Drawing.Size(211, 6)
            '
            'ToolbarTOP
            '
            Me.ToolbarTOP.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
            Me.ToolbarTOP.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MENU_VIEW, Me.BTT_REFRESH, SEP_1, Me.BTT_UP, Me.BTT_DOWN, Me.BTT_FIND, SEP_2, Me.BTT_CLEAR})
            Me.ToolbarTOP.Location = New System.Drawing.Point(0, 0)
            Me.ToolbarTOP.Name = "ToolbarTOP"
            Me.ToolbarTOP.Size = New System.Drawing.Size(554, 25)
            Me.ToolbarTOP.TabIndex = 0
            '
            'MENU_VIEW
            '
            Me.MENU_VIEW.AutoToolTip = False
            Me.MENU_VIEW.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MENU_VIEW_SESSION, Me.MENU_VIEW_ALL, MENU_VIEW_SEP_1, Me.OPT_DEFAULT, Me.OPT_SUBSCRIPTIONS})
            Me.MENU_VIEW.Image = CType(resources.GetObject("MENU_VIEW.Image"), System.Drawing.Image)
            Me.MENU_VIEW.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.MENU_VIEW.Name = "MENU_VIEW"
            Me.MENU_VIEW.Size = New System.Drawing.Size(61, 22)
            Me.MENU_VIEW.Text = "View"
            '
            'MENU_VIEW_SESSION
            '
            Me.MENU_VIEW_SESSION.AutoToolTip = True
            Me.MENU_VIEW_SESSION.Name = "MENU_VIEW_SESSION"
            Me.MENU_VIEW_SESSION.Size = New System.Drawing.Size(214, 22)
            Me.MENU_VIEW_SESSION.Text = "Session"
            Me.MENU_VIEW_SESSION.ToolTipText = "Show downloaded users by this session"
            '
            'MENU_VIEW_ALL
            '
            Me.MENU_VIEW_ALL.AutoToolTip = True
            Me.MENU_VIEW_ALL.Name = "MENU_VIEW_ALL"
            Me.MENU_VIEW_ALL.Size = New System.Drawing.Size(214, 22)
            Me.MENU_VIEW_ALL.Text = "All"
            Me.MENU_VIEW_ALL.ToolTipText = "Show all users (sorted by latest download)"
            '
            'OPT_DEFAULT
            '
            Me.OPT_DEFAULT.AutoToolTip = True
            Me.OPT_DEFAULT.Name = "OPT_DEFAULT"
            Me.OPT_DEFAULT.Size = New System.Drawing.Size(214, 22)
            Me.OPT_DEFAULT.Text = "Downloaded users"
            Me.OPT_DEFAULT.ToolTipText = "Show downloaded users"
            '
            'OPT_SUBSCRIPTIONS
            '
            Me.OPT_SUBSCRIPTIONS.AutoToolTip = True
            Me.OPT_SUBSCRIPTIONS.Name = "OPT_SUBSCRIPTIONS"
            Me.OPT_SUBSCRIPTIONS.Size = New System.Drawing.Size(214, 22)
            Me.OPT_SUBSCRIPTIONS.Text = "Downloaded subscriptions"
            Me.OPT_SUBSCRIPTIONS.ToolTipText = "Show downloaded subscriptions"
            '
            'BTT_REFRESH
            '
            Me.BTT_REFRESH.Image = Global.SCrawler.My.Resources.Resources.RefreshPic_24
            Me.BTT_REFRESH.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_REFRESH.Name = "BTT_REFRESH"
            Me.BTT_REFRESH.Size = New System.Drawing.Size(89, 22)
            Me.BTT_REFRESH.Text = "Refresh (F5)"
            Me.BTT_REFRESH.ToolTipText = "Force list refresh"
            '
            'BTT_UP
            '
            Me.BTT_UP.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.BTT_UP.Image = CType(resources.GetObject("BTT_UP.Image"), System.Drawing.Image)
            Me.BTT_UP.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_UP.Name = "BTT_UP"
            Me.BTT_UP.Size = New System.Drawing.Size(23, 22)
            Me.BTT_UP.Text = "Up"
            Me.BTT_UP.ToolTipText = "Up (F3)"
            '
            'BTT_DOWN
            '
            Me.BTT_DOWN.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.BTT_DOWN.Image = CType(resources.GetObject("BTT_DOWN.Image"), System.Drawing.Image)
            Me.BTT_DOWN.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_DOWN.Name = "BTT_DOWN"
            Me.BTT_DOWN.Size = New System.Drawing.Size(23, 22)
            Me.BTT_DOWN.Text = "Down"
            Me.BTT_DOWN.ToolTipText = "Down (F2)"
            '
            'BTT_FIND
            '
            Me.BTT_FIND.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.BTT_FIND.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_FIND.Name = "BTT_FIND"
            Me.BTT_FIND.Size = New System.Drawing.Size(57, 22)
            Me.BTT_FIND.Text = "Find (F4)"
            Me.BTT_FIND.ToolTipText = "Find in the main window"
            '
            'BTT_CLEAR
            '
            Me.BTT_CLEAR.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
            Me.BTT_CLEAR.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_CLEAR.Name = "BTT_CLEAR"
            Me.BTT_CLEAR.Size = New System.Drawing.Size(38, 22)
            Me.BTT_CLEAR.Text = "Clear"
            Me.BTT_CLEAR.ToolTipText = "Clear info list"
            '
            'ToolbarBOTTOM
            '
            Me.ToolbarBOTTOM.Location = New System.Drawing.Point(0, 389)
            Me.ToolbarBOTTOM.Name = "ToolbarBOTTOM"
            Me.ToolbarBOTTOM.Size = New System.Drawing.Size(554, 22)
            Me.ToolbarBOTTOM.TabIndex = 1
            '
            'LIST_DOWN
            '
            Me.LIST_DOWN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LIST_DOWN.FormattingEnabled = True
            Me.LIST_DOWN.Location = New System.Drawing.Point(0, 25)
            Me.LIST_DOWN.Name = "LIST_DOWN"
            Me.LIST_DOWN.Size = New System.Drawing.Size(554, 364)
            Me.LIST_DOWN.TabIndex = 2
            '
            'DownloadedInfoForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(554, 411)
            Me.Controls.Add(Me.LIST_DOWN)
            Me.Controls.Add(Me.ToolbarBOTTOM)
            Me.Controls.Add(Me.ToolbarTOP)
            Me.Icon = Global.SCrawler.My.Resources.Resources.ArrowDownIcon_Blue_24
            Me.KeyPreview = True
            Me.MinimumSize = New System.Drawing.Size(570, 450)
            Me.Name = "DownloadedInfoForm"
            Me.Text = "Downloaded items"
            Me.ToolbarTOP.ResumeLayout(False)
            Me.ToolbarTOP.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Private WithEvents ToolbarTOP As ToolStrip
        Private WithEvents MENU_VIEW As ToolStripDropDownButton
        Private WithEvents MENU_VIEW_SESSION As ToolStripMenuItem
        Private WithEvents MENU_VIEW_ALL As ToolStripMenuItem
        Private WithEvents BTT_REFRESH As ToolStripButton
        Private WithEvents ToolbarBOTTOM As StatusStrip
        Private WithEvents LIST_DOWN As ListBox
        Private WithEvents BTT_CLEAR As ToolStripButton
        Private WithEvents BTT_FIND As ToolStripButton
        Private WithEvents BTT_UP As ToolStripButton
        Private WithEvents BTT_DOWN As ToolStripButton
        Private WithEvents OPT_DEFAULT As ToolStripMenuItem
        Private WithEvents OPT_SUBSCRIPTIONS As ToolStripMenuItem
    End Class
End Namespace