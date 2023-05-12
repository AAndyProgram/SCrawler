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
    Partial Friend Class UsersInfoForm : Inherits System.Windows.Forms.Form
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
            Dim MENU_SEP_1 As System.Windows.Forms.ToolStripSeparator
            Dim MENU_SEP_2 As System.Windows.Forms.ToolStripSeparator
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UsersInfoForm))
            Me.Toolbar_TOP = New System.Windows.Forms.ToolStrip()
            Me.BTT_START = New System.Windows.Forms.ToolStripButton()
            Me.BTT_CANCEL = New System.Windows.Forms.ToolStripButton()
            Me.MENU_VIEW = New System.Windows.Forms.ToolStripDropDownButton()
            Me.OPT_DATE = New System.Windows.Forms.ToolStripMenuItem()
            Me.OPT_SIZE = New System.Windows.Forms.ToolStripMenuItem()
            Me.OPT_AMOUNT = New System.Windows.Forms.ToolStripMenuItem()
            Me.OPT_ASC = New System.Windows.Forms.ToolStripMenuItem()
            Me.OPT_DESC = New System.Windows.Forms.ToolStripMenuItem()
            Me.CH_GROUP_DRIVE = New System.Windows.Forms.ToolStripMenuItem()
            Me.CH_GROUP_COL = New System.Windows.Forms.ToolStripMenuItem()
            Me.Toolbar_BOTTOM = New System.Windows.Forms.StatusStrip()
            Me.PR_MAIN = New System.Windows.Forms.ToolStripProgressBar()
            Me.LBL_STATUS = New System.Windows.Forms.ToolStripStatusLabel()
            Me.LIST_DATA = New System.Windows.Forms.ListView()
            Me.COL_DEFAULT = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            Me.CONTEXT_LIST = New System.Windows.Forms.ContextMenuStrip(Me.components)
            Me.CONTEXT_BTT_FIND = New System.Windows.Forms.ToolStripMenuItem()
            Me.CONTEXT_BTT_INFO = New System.Windows.Forms.ToolStripMenuItem()
            Me.CONTEXT_BTT_OPEN_FOLDER = New System.Windows.Forms.ToolStripMenuItem()
            Me.CONTEXT_BTT_OPEN_SITE = New System.Windows.Forms.ToolStripMenuItem()
            SEP_1 = New System.Windows.Forms.ToolStripSeparator()
            CONTEXT_SEP_1 = New System.Windows.Forms.ToolStripSeparator()
            MENU_SEP_1 = New System.Windows.Forms.ToolStripSeparator()
            MENU_SEP_2 = New System.Windows.Forms.ToolStripSeparator()
            Me.Toolbar_TOP.SuspendLayout()
            Me.Toolbar_BOTTOM.SuspendLayout()
            Me.CONTEXT_LIST.SuspendLayout()
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
            CONTEXT_SEP_1.Size = New System.Drawing.Size(166, 6)
            '
            'MENU_SEP_1
            '
            MENU_SEP_1.Name = "MENU_SEP_1"
            MENU_SEP_1.Size = New System.Drawing.Size(175, 6)
            '
            'MENU_SEP_2
            '
            MENU_SEP_2.Name = "MENU_SEP_2"
            MENU_SEP_2.Size = New System.Drawing.Size(175, 6)
            '
            'Toolbar_TOP
            '
            Me.Toolbar_TOP.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
            Me.Toolbar_TOP.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_START, Me.BTT_CANCEL, SEP_1, Me.MENU_VIEW})
            Me.Toolbar_TOP.Location = New System.Drawing.Point(0, 0)
            Me.Toolbar_TOP.Name = "Toolbar_TOP"
            Me.Toolbar_TOP.ShowItemToolTips = False
            Me.Toolbar_TOP.Size = New System.Drawing.Size(284, 25)
            Me.Toolbar_TOP.TabIndex = 0
            '
            'BTT_START
            '
            Me.BTT_START.AutoToolTip = False
            Me.BTT_START.Image = Global.SCrawler.My.Resources.Resources.StartPic_Green_16
            Me.BTT_START.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_START.Name = "BTT_START"
            Me.BTT_START.Size = New System.Drawing.Size(76, 22)
            Me.BTT_START.Text = "Calculate"
            '
            'BTT_CANCEL
            '
            Me.BTT_CANCEL.AutoToolTip = False
            Me.BTT_CANCEL.Enabled = False
            Me.BTT_CANCEL.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
            Me.BTT_CANCEL.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_CANCEL.Name = "BTT_CANCEL"
            Me.BTT_CANCEL.Size = New System.Drawing.Size(63, 22)
            Me.BTT_CANCEL.Text = "Cancel"
            '
            'MENU_VIEW
            '
            Me.MENU_VIEW.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OPT_DATE, Me.OPT_SIZE, Me.OPT_AMOUNT, MENU_SEP_1, Me.OPT_ASC, Me.OPT_DESC, MENU_SEP_2, Me.CH_GROUP_DRIVE, Me.CH_GROUP_COL})
            Me.MENU_VIEW.Image = CType(resources.GetObject("MENU_VIEW.Image"), System.Drawing.Image)
            Me.MENU_VIEW.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.MENU_VIEW.Name = "MENU_VIEW"
            Me.MENU_VIEW.Size = New System.Drawing.Size(61, 22)
            Me.MENU_VIEW.Text = "View"
            '
            'OPT_DATE
            '
            Me.OPT_DATE.CheckOnClick = True
            Me.OPT_DATE.Name = "OPT_DATE"
            Me.OPT_DATE.Size = New System.Drawing.Size(178, 22)
            Me.OPT_DATE.Text = "Sort by date"
            '
            'OPT_SIZE
            '
            Me.OPT_SIZE.CheckOnClick = True
            Me.OPT_SIZE.Name = "OPT_SIZE"
            Me.OPT_SIZE.Size = New System.Drawing.Size(178, 22)
            Me.OPT_SIZE.Text = "Sort by size"
            '
            'OPT_AMOUNT
            '
            Me.OPT_AMOUNT.CheckOnClick = True
            Me.OPT_AMOUNT.Name = "OPT_AMOUNT"
            Me.OPT_AMOUNT.Size = New System.Drawing.Size(178, 22)
            Me.OPT_AMOUNT.Text = "Sort by amount"
            '
            'OPT_ASC
            '
            Me.OPT_ASC.CheckOnClick = True
            Me.OPT_ASC.Name = "OPT_ASC"
            Me.OPT_ASC.Size = New System.Drawing.Size(178, 22)
            Me.OPT_ASC.Text = "Ascending"
            '
            'OPT_DESC
            '
            Me.OPT_DESC.CheckOnClick = True
            Me.OPT_DESC.Name = "OPT_DESC"
            Me.OPT_DESC.Size = New System.Drawing.Size(178, 22)
            Me.OPT_DESC.Text = "Descending"
            '
            'CH_GROUP_DRIVE
            '
            Me.CH_GROUP_DRIVE.CheckOnClick = True
            Me.CH_GROUP_DRIVE.Name = "CH_GROUP_DRIVE"
            Me.CH_GROUP_DRIVE.Size = New System.Drawing.Size(178, 22)
            Me.CH_GROUP_DRIVE.Text = "Group by drive"
            '
            'CH_GROUP_COL
            '
            Me.CH_GROUP_COL.CheckOnClick = True
            Me.CH_GROUP_COL.Name = "CH_GROUP_COL"
            Me.CH_GROUP_COL.Size = New System.Drawing.Size(178, 22)
            Me.CH_GROUP_COL.Text = "Group by collection"
            '
            'Toolbar_BOTTOM
            '
            Me.Toolbar_BOTTOM.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PR_MAIN, Me.LBL_STATUS})
            Me.Toolbar_BOTTOM.Location = New System.Drawing.Point(0, 239)
            Me.Toolbar_BOTTOM.Name = "Toolbar_BOTTOM"
            Me.Toolbar_BOTTOM.Size = New System.Drawing.Size(284, 22)
            Me.Toolbar_BOTTOM.TabIndex = 1
            '
            'PR_MAIN
            '
            Me.PR_MAIN.Name = "PR_MAIN"
            Me.PR_MAIN.Size = New System.Drawing.Size(200, 16)
            Me.PR_MAIN.Visible = False
            '
            'LBL_STATUS
            '
            Me.LBL_STATUS.Name = "LBL_STATUS"
            Me.LBL_STATUS.Size = New System.Drawing.Size(0, 17)
            '
            'LIST_DATA
            '
            Me.LIST_DATA.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.COL_DEFAULT})
            Me.LIST_DATA.ContextMenuStrip = Me.CONTEXT_LIST
            Me.LIST_DATA.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LIST_DATA.FullRowSelect = True
            Me.LIST_DATA.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
            Me.LIST_DATA.HideSelection = False
            Me.LIST_DATA.Location = New System.Drawing.Point(0, 25)
            Me.LIST_DATA.MultiSelect = False
            Me.LIST_DATA.Name = "LIST_DATA"
            Me.LIST_DATA.Size = New System.Drawing.Size(284, 214)
            Me.LIST_DATA.TabIndex = 2
            Me.LIST_DATA.UseCompatibleStateImageBehavior = False
            Me.LIST_DATA.View = System.Windows.Forms.View.Details
            '
            'COL_DEFAULT
            '
            Me.COL_DEFAULT.Text = "User"
            Me.COL_DEFAULT.Width = 280
            '
            'CONTEXT_LIST
            '
            Me.CONTEXT_LIST.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CONTEXT_BTT_FIND, Me.CONTEXT_BTT_INFO, CONTEXT_SEP_1, Me.CONTEXT_BTT_OPEN_FOLDER, Me.CONTEXT_BTT_OPEN_SITE})
            Me.CONTEXT_LIST.Name = "CONTEXT_LIST"
            Me.CONTEXT_LIST.Size = New System.Drawing.Size(170, 98)
            '
            'CONTEXT_BTT_FIND
            '
            Me.CONTEXT_BTT_FIND.Image = Global.SCrawler.My.Resources.Resources.InfoPic_32
            Me.CONTEXT_BTT_FIND.Name = "CONTEXT_BTT_FIND"
            Me.CONTEXT_BTT_FIND.Size = New System.Drawing.Size(169, 22)
            Me.CONTEXT_BTT_FIND.Text = "Find user"
            '
            'CONTEXT_BTT_INFO
            '
            Me.CONTEXT_BTT_INFO.Image = Global.SCrawler.My.Resources.Resources.InfoPic_32
            Me.CONTEXT_BTT_INFO.Name = "CONTEXT_BTT_INFO"
            Me.CONTEXT_BTT_INFO.Size = New System.Drawing.Size(169, 22)
            Me.CONTEXT_BTT_INFO.Text = "Show information"
            '
            'CONTEXT_BTT_OPEN_FOLDER
            '
            Me.CONTEXT_BTT_OPEN_FOLDER.Image = Global.SCrawler.My.Resources.Resources.FolderPic_32
            Me.CONTEXT_BTT_OPEN_FOLDER.Name = "CONTEXT_BTT_OPEN_FOLDER"
            Me.CONTEXT_BTT_OPEN_FOLDER.Size = New System.Drawing.Size(169, 22)
            Me.CONTEXT_BTT_OPEN_FOLDER.Text = "Open folder"
            '
            'CONTEXT_BTT_OPEN_SITE
            '
            Me.CONTEXT_BTT_OPEN_SITE.Image = Global.SCrawler.My.Resources.Resources.GlobePic_32
            Me.CONTEXT_BTT_OPEN_SITE.Name = "CONTEXT_BTT_OPEN_SITE"
            Me.CONTEXT_BTT_OPEN_SITE.Size = New System.Drawing.Size(169, 22)
            Me.CONTEXT_BTT_OPEN_SITE.Text = "Open site"
            '
            'UsersInfoForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(284, 261)
            Me.Controls.Add(Me.LIST_DATA)
            Me.Controls.Add(Me.Toolbar_BOTTOM)
            Me.Controls.Add(Me.Toolbar_TOP)
            Me.Icon = Global.SCrawler.My.Resources.Resources.UsersIcon_32
            Me.KeyPreview = True
            Me.MinimumSize = New System.Drawing.Size(300, 300)
            Me.Name = "UsersInfoForm"
            Me.Text = "Users info"
            Me.Toolbar_TOP.ResumeLayout(False)
            Me.Toolbar_TOP.PerformLayout()
            Me.Toolbar_BOTTOM.ResumeLayout(False)
            Me.Toolbar_BOTTOM.PerformLayout()
            Me.CONTEXT_LIST.ResumeLayout(False)
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Private WithEvents Toolbar_TOP As ToolStrip
        Private WithEvents Toolbar_BOTTOM As StatusStrip
        Private WithEvents PR_MAIN As ToolStripProgressBar
        Private WithEvents LBL_STATUS As ToolStripStatusLabel
        Private WithEvents LIST_DATA As ListView
        Private WithEvents BTT_START As ToolStripButton
        Private WithEvents BTT_CANCEL As ToolStripButton
        Private WithEvents COL_DEFAULT As ColumnHeader
        Private WithEvents CONTEXT_LIST As ContextMenuStrip
        Private WithEvents CONTEXT_BTT_FIND As ToolStripMenuItem
        Private WithEvents CONTEXT_BTT_INFO As ToolStripMenuItem
        Private WithEvents CONTEXT_BTT_OPEN_FOLDER As ToolStripMenuItem
        Private WithEvents CONTEXT_BTT_OPEN_SITE As ToolStripMenuItem
        Private WithEvents MENU_VIEW As ToolStripDropDownButton
        Private WithEvents OPT_DATE As ToolStripMenuItem
        Private WithEvents OPT_SIZE As ToolStripMenuItem
        Private WithEvents OPT_AMOUNT As ToolStripMenuItem
        Private WithEvents OPT_ASC As ToolStripMenuItem
        Private WithEvents OPT_DESC As ToolStripMenuItem
        Private WithEvents CH_GROUP_DRIVE As ToolStripMenuItem
        Private WithEvents CH_GROUP_COL As ToolStripMenuItem
    End Class
End Namespace