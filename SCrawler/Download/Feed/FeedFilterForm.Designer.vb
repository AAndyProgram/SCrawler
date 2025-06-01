' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace DownloadObjects
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class FeedFilterForm : Inherits System.Windows.Forms.Form
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
            Dim CONTAINER_MAIN As System.Windows.Forms.ToolStripContainer
            Dim TP_TYPES As System.Windows.Forms.TableLayoutPanel
            Dim TP_USERS As System.Windows.Forms.TableLayoutPanel
            Dim TP_USERS_2 As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FeedFilterForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Me.TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            Me.CH_T_ALL = New System.Windows.Forms.CheckBox()
            Me.CH_T_IMG = New System.Windows.Forms.CheckBox()
            Me.CH_T_VID = New System.Windows.Forms.CheckBox()
            Me.CH_T_GIF = New System.Windows.Forms.CheckBox()
            Me.CH_T_TXT = New System.Windows.Forms.CheckBox()
            Me.CH_U_USE = New System.Windows.Forms.CheckBox()
            Me.CH_U_SHOW_ALL = New System.Windows.Forms.CheckBox()
            Me.BTT_ALL = New System.Windows.Forms.Button()
            Me.BTT_NONE = New System.Windows.Forms.Button()
            Me.LIST_USERS = New System.Windows.Forms.CheckedListBox()
            Me.TXT_NAME = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_SITE = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_TYPES = New System.Windows.Forms.TableLayoutPanel()
            TP_USERS = New System.Windows.Forms.TableLayoutPanel()
            TP_USERS_2 = New System.Windows.Forms.TableLayoutPanel()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            Me.TP_MAIN.SuspendLayout()
            TP_TYPES.SuspendLayout()
            TP_USERS.SuspendLayout()
            TP_USERS_2.SuspendLayout()
            CType(Me.TXT_NAME, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_SITE, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(Me.TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(522, 336)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(522, 336)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            Me.TP_MAIN.ColumnCount = 1
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.Controls.Add(TP_TYPES, 0, 1)
            Me.TP_MAIN.Controls.Add(TP_USERS, 0, 3)
            Me.TP_MAIN.Controls.Add(Me.TXT_NAME, 0, 0)
            Me.TP_MAIN.Controls.Add(Me.TXT_SITE, 0, 2)
            Me.TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.TP_MAIN.Name = "TP_MAIN"
            Me.TP_MAIN.RowCount = 4
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.Size = New System.Drawing.Size(522, 336)
            Me.TP_MAIN.TabIndex = 0
            '
            'TP_TYPES
            '
            TP_TYPES.ColumnCount = 5
            TP_TYPES.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_TYPES.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_TYPES.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_TYPES.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_TYPES.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_TYPES.Controls.Add(Me.CH_T_ALL, 0, 0)
            TP_TYPES.Controls.Add(Me.CH_T_IMG, 1, 0)
            TP_TYPES.Controls.Add(Me.CH_T_VID, 2, 0)
            TP_TYPES.Controls.Add(Me.CH_T_GIF, 3, 0)
            TP_TYPES.Controls.Add(Me.CH_T_TXT, 4, 0)
            TP_TYPES.Dock = System.Windows.Forms.DockStyle.Fill
            TP_TYPES.Location = New System.Drawing.Point(0, 28)
            TP_TYPES.Margin = New System.Windows.Forms.Padding(0)
            TP_TYPES.Name = "TP_TYPES"
            TP_TYPES.RowCount = 1
            TP_TYPES.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_TYPES.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_TYPES.Size = New System.Drawing.Size(522, 25)
            TP_TYPES.TabIndex = 1
            '
            'CH_T_ALL
            '
            Me.CH_T_ALL.AutoSize = True
            Me.CH_T_ALL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_T_ALL.Location = New System.Drawing.Point(3, 3)
            Me.CH_T_ALL.Name = "CH_T_ALL"
            Me.CH_T_ALL.Size = New System.Drawing.Size(98, 19)
            Me.CH_T_ALL.TabIndex = 0
            Me.CH_T_ALL.Text = "ALL"
            Me.CH_T_ALL.UseVisualStyleBackColor = True
            '
            'CH_T_IMG
            '
            Me.CH_T_IMG.AutoSize = True
            Me.CH_T_IMG.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_T_IMG.Location = New System.Drawing.Point(107, 3)
            Me.CH_T_IMG.Name = "CH_T_IMG"
            Me.CH_T_IMG.Size = New System.Drawing.Size(98, 19)
            Me.CH_T_IMG.TabIndex = 1
            Me.CH_T_IMG.Text = "Image"
            Me.CH_T_IMG.UseVisualStyleBackColor = True
            '
            'CH_T_VID
            '
            Me.CH_T_VID.AutoSize = True
            Me.CH_T_VID.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_T_VID.Location = New System.Drawing.Point(211, 3)
            Me.CH_T_VID.Name = "CH_T_VID"
            Me.CH_T_VID.Size = New System.Drawing.Size(98, 19)
            Me.CH_T_VID.TabIndex = 2
            Me.CH_T_VID.Text = "Video"
            Me.CH_T_VID.UseVisualStyleBackColor = True
            '
            'CH_T_GIF
            '
            Me.CH_T_GIF.AutoSize = True
            Me.CH_T_GIF.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_T_GIF.Location = New System.Drawing.Point(315, 3)
            Me.CH_T_GIF.Name = "CH_T_GIF"
            Me.CH_T_GIF.Size = New System.Drawing.Size(98, 19)
            Me.CH_T_GIF.TabIndex = 3
            Me.CH_T_GIF.Text = "GIF"
            Me.CH_T_GIF.UseVisualStyleBackColor = True
            '
            'CH_T_TXT
            '
            Me.CH_T_TXT.AutoSize = True
            Me.CH_T_TXT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_T_TXT.Location = New System.Drawing.Point(419, 3)
            Me.CH_T_TXT.Name = "CH_T_TXT"
            Me.CH_T_TXT.Size = New System.Drawing.Size(100, 19)
            Me.CH_T_TXT.TabIndex = 4
            Me.CH_T_TXT.Text = "Text"
            Me.CH_T_TXT.UseVisualStyleBackColor = True
            '
            'TP_USERS
            '
            TP_USERS.ColumnCount = 1
            TP_USERS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_USERS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_USERS.Controls.Add(TP_USERS_2, 0, 0)
            TP_USERS.Controls.Add(Me.LIST_USERS, 0, 1)
            TP_USERS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_USERS.Location = New System.Drawing.Point(0, 81)
            TP_USERS.Margin = New System.Windows.Forms.Padding(0)
            TP_USERS.Name = "TP_USERS"
            TP_USERS.RowCount = 2
            TP_USERS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_USERS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_USERS.Size = New System.Drawing.Size(522, 255)
            TP_USERS.TabIndex = 3
            '
            'TP_USERS_2
            '
            TP_USERS_2.ColumnCount = 4
            TP_USERS_2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            TP_USERS_2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            TP_USERS_2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            TP_USERS_2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            TP_USERS_2.Controls.Add(Me.CH_U_USE, 0, 0)
            TP_USERS_2.Controls.Add(Me.CH_U_SHOW_ALL, 1, 0)
            TP_USERS_2.Controls.Add(Me.BTT_ALL, 2, 0)
            TP_USERS_2.Controls.Add(Me.BTT_NONE, 3, 0)
            TP_USERS_2.Dock = System.Windows.Forms.DockStyle.Fill
            TP_USERS_2.Location = New System.Drawing.Point(0, 0)
            TP_USERS_2.Margin = New System.Windows.Forms.Padding(0)
            TP_USERS_2.Name = "TP_USERS_2"
            TP_USERS_2.RowCount = 1
            TP_USERS_2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_USERS_2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_USERS_2.Size = New System.Drawing.Size(522, 25)
            TP_USERS_2.TabIndex = 0
            '
            'CH_U_USE
            '
            Me.CH_U_USE.AutoSize = True
            Me.CH_U_USE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_U_USE.Location = New System.Drawing.Point(3, 3)
            Me.CH_U_USE.Name = "CH_U_USE"
            Me.CH_U_USE.Size = New System.Drawing.Size(124, 19)
            Me.CH_U_USE.TabIndex = 0
            Me.CH_U_USE.Text = "Filter users"
            Me.CH_U_USE.UseVisualStyleBackColor = True
            '
            'CH_U_SHOW_ALL
            '
            Me.CH_U_SHOW_ALL.AutoSize = True
            Me.CH_U_SHOW_ALL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_U_SHOW_ALL.Location = New System.Drawing.Point(133, 3)
            Me.CH_U_SHOW_ALL.Name = "CH_U_SHOW_ALL"
            Me.CH_U_SHOW_ALL.Size = New System.Drawing.Size(124, 19)
            Me.CH_U_SHOW_ALL.TabIndex = 1
            Me.CH_U_SHOW_ALL.Text = "Show all users"
            Me.CH_U_SHOW_ALL.UseVisualStyleBackColor = True
            '
            'BTT_ALL
            '
            Me.BTT_ALL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_ALL.Location = New System.Drawing.Point(261, 1)
            Me.BTT_ALL.Margin = New System.Windows.Forms.Padding(1)
            Me.BTT_ALL.Name = "BTT_ALL"
            Me.BTT_ALL.Size = New System.Drawing.Size(128, 23)
            Me.BTT_ALL.TabIndex = 2
            Me.BTT_ALL.Text = "All"
            Me.BTT_ALL.UseVisualStyleBackColor = True
            '
            'BTT_NONE
            '
            Me.BTT_NONE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_NONE.Location = New System.Drawing.Point(391, 1)
            Me.BTT_NONE.Margin = New System.Windows.Forms.Padding(1)
            Me.BTT_NONE.Name = "BTT_NONE"
            Me.BTT_NONE.Size = New System.Drawing.Size(130, 23)
            Me.BTT_NONE.TabIndex = 3
            Me.BTT_NONE.Text = "None"
            Me.BTT_NONE.UseVisualStyleBackColor = True
            '
            'LIST_USERS
            '
            Me.LIST_USERS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LIST_USERS.FormattingEnabled = True
            Me.LIST_USERS.Location = New System.Drawing.Point(3, 28)
            Me.LIST_USERS.Name = "LIST_USERS"
            Me.LIST_USERS.Size = New System.Drawing.Size(516, 224)
            Me.LIST_USERS.TabIndex = 1
            '
            'TXT_NAME
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Name = "Clear"
            ActionButton1.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_NAME.Buttons.Add(ActionButton1)
            Me.TXT_NAME.CaptionText = "Filter name"
            Me.TXT_NAME.CaptionWidth = 80.0R
            Me.TXT_NAME.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_NAME.Location = New System.Drawing.Point(3, 3)
            Me.TXT_NAME.Name = "TXT_NAME"
            Me.TXT_NAME.PlaceholderEnabled = True
            Me.TXT_NAME.PlaceholderText = "Enter filter name here..."
            Me.TXT_NAME.Size = New System.Drawing.Size(516, 22)
            Me.TXT_NAME.TabIndex = 0
            '
            'TXT_SITE
            '
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Name = "Edit"
            ActionButton2.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Edit
            ActionButton2.ToolTipText = "Edit sites"
            ActionButton3.BackgroundImage = CType(resources.GetObject("ActionButton3.BackgroundImage"), System.Drawing.Image)
            ActionButton3.Name = "Clear"
            ActionButton3.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            ActionButton3.ToolTipText = "Delete all sites"
            Me.TXT_SITE.Buttons.Add(ActionButton2)
            Me.TXT_SITE.Buttons.Add(ActionButton3)
            Me.TXT_SITE.CaptionText = "Sites"
            Me.TXT_SITE.CaptionWidth = 40.0R
            Me.TXT_SITE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_SITE.Location = New System.Drawing.Point(3, 56)
            Me.TXT_SITE.Name = "TXT_SITE"
            Me.TXT_SITE.Size = New System.Drawing.Size(516, 22)
            Me.TXT_SITE.TabIndex = 2
            Me.TXT_SITE.TextBoxReadOnly = True
            '
            'FeedFilterForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(522, 336)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.Name = "FeedFilterForm"
            Me.Text = "Filter"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            Me.TP_MAIN.ResumeLayout(False)
            TP_TYPES.ResumeLayout(False)
            TP_TYPES.PerformLayout()
            TP_USERS.ResumeLayout(False)
            TP_USERS_2.ResumeLayout(False)
            TP_USERS_2.PerformLayout()
            CType(Me.TXT_NAME, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_SITE, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents CH_T_ALL As CheckBox
        Private WithEvents CH_T_IMG As CheckBox
        Private WithEvents CH_T_VID As CheckBox
        Private WithEvents CH_T_GIF As CheckBox
        Private WithEvents CH_T_TXT As CheckBox
        Private WithEvents CH_U_USE As CheckBox
        Private WithEvents CH_U_SHOW_ALL As CheckBox
        Private WithEvents BTT_ALL As Button
        Private WithEvents BTT_NONE As Button
        Private WithEvents LIST_USERS As CheckedListBox
        Private WithEvents TXT_NAME As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TP_MAIN As TableLayoutPanel
        Private WithEvents TXT_SITE As PersonalUtilities.Forms.Controls.TextBoxExtended
    End Class
End Namespace