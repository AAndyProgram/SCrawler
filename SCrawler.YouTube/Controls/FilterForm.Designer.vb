' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.YouTube.Controls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class FilterForm : Inherits System.Windows.Forms.Form
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
            Dim TP_MAIN As System.Windows.Forms.TableLayoutPanel
            Dim TP_TYPES As System.Windows.Forms.TableLayoutPanel
            Dim TP_USERS As System.Windows.Forms.TableLayoutPanel
            Dim TP_USERS_2 As System.Windows.Forms.TableLayoutPanel
            Me.CH_FILTER_ALL = New System.Windows.Forms.CheckBox()
            Me.CH_FILTER_SINGLE = New System.Windows.Forms.CheckBox()
            Me.CH_FILTER_CHANNEL = New System.Windows.Forms.CheckBox()
            Me.CH_FILTER_PLS = New System.Windows.Forms.CheckBox()
            Me.TP_MUSIC = New System.Windows.Forms.TableLayoutPanel()
            Me.CH_M_ALL = New System.Windows.Forms.CheckBox()
            Me.CH_M_VIDEO = New System.Windows.Forms.CheckBox()
            Me.CH_M_MUSIC = New System.Windows.Forms.CheckBox()
            Me.LIST_USERS = New System.Windows.Forms.CheckedListBox()
            Me.CH_USERS_USE = New System.Windows.Forms.CheckBox()
            Me.BTT_SELECT_ALL = New System.Windows.Forms.Button()
            Me.BTT_SELECT_NONE = New System.Windows.Forms.Button()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            TP_TYPES = New System.Windows.Forms.TableLayoutPanel()
            TP_USERS = New System.Windows.Forms.TableLayoutPanel()
            TP_USERS_2 = New System.Windows.Forms.TableLayoutPanel()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            TP_MAIN.SuspendLayout()
            TP_TYPES.SuspendLayout()
            Me.TP_MUSIC.SuspendLayout()
            TP_USERS.SuspendLayout()
            TP_USERS_2.SuspendLayout()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(384, 361)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(384, 361)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(TP_TYPES, 0, 0)
            TP_MAIN.Controls.Add(Me.TP_MUSIC, 0, 1)
            TP_MAIN.Controls.Add(TP_USERS, 0, 2)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 3
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_MAIN.Size = New System.Drawing.Size(384, 361)
            TP_MAIN.TabIndex = 0
            '
            'TP_TYPES
            '
            TP_TYPES.ColumnCount = 4
            TP_TYPES.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            TP_TYPES.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            TP_TYPES.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            TP_TYPES.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            TP_TYPES.Controls.Add(Me.CH_FILTER_ALL, 0, 0)
            TP_TYPES.Controls.Add(Me.CH_FILTER_SINGLE, 1, 0)
            TP_TYPES.Controls.Add(Me.CH_FILTER_CHANNEL, 2, 0)
            TP_TYPES.Controls.Add(Me.CH_FILTER_PLS, 3, 0)
            TP_TYPES.Dock = System.Windows.Forms.DockStyle.Fill
            TP_TYPES.Location = New System.Drawing.Point(0, 0)
            TP_TYPES.Margin = New System.Windows.Forms.Padding(0)
            TP_TYPES.Name = "TP_TYPES"
            TP_TYPES.RowCount = 1
            TP_TYPES.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_TYPES.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_TYPES.Size = New System.Drawing.Size(384, 25)
            TP_TYPES.TabIndex = 0
            '
            'CH_FILTER_ALL
            '
            Me.CH_FILTER_ALL.AutoSize = True
            Me.CH_FILTER_ALL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FILTER_ALL.Location = New System.Drawing.Point(3, 3)
            Me.CH_FILTER_ALL.Name = "CH_FILTER_ALL"
            Me.CH_FILTER_ALL.Size = New System.Drawing.Size(90, 19)
            Me.CH_FILTER_ALL.TabIndex = 0
            Me.CH_FILTER_ALL.Text = "ALL"
            Me.CH_FILTER_ALL.UseVisualStyleBackColor = True
            '
            'CH_FILTER_SINGLE
            '
            Me.CH_FILTER_SINGLE.AutoSize = True
            Me.CH_FILTER_SINGLE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FILTER_SINGLE.Location = New System.Drawing.Point(99, 3)
            Me.CH_FILTER_SINGLE.Name = "CH_FILTER_SINGLE"
            Me.CH_FILTER_SINGLE.Size = New System.Drawing.Size(90, 19)
            Me.CH_FILTER_SINGLE.TabIndex = 1
            Me.CH_FILTER_SINGLE.Text = "Single"
            Me.CH_FILTER_SINGLE.UseVisualStyleBackColor = True
            '
            'CH_FILTER_CHANNEL
            '
            Me.CH_FILTER_CHANNEL.AutoSize = True
            Me.CH_FILTER_CHANNEL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FILTER_CHANNEL.Location = New System.Drawing.Point(195, 3)
            Me.CH_FILTER_CHANNEL.Name = "CH_FILTER_CHANNEL"
            Me.CH_FILTER_CHANNEL.Size = New System.Drawing.Size(90, 19)
            Me.CH_FILTER_CHANNEL.TabIndex = 2
            Me.CH_FILTER_CHANNEL.Text = "Channel"
            Me.CH_FILTER_CHANNEL.UseVisualStyleBackColor = True
            '
            'CH_FILTER_PLS
            '
            Me.CH_FILTER_PLS.AutoSize = True
            Me.CH_FILTER_PLS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FILTER_PLS.Location = New System.Drawing.Point(291, 3)
            Me.CH_FILTER_PLS.Name = "CH_FILTER_PLS"
            Me.CH_FILTER_PLS.Size = New System.Drawing.Size(90, 19)
            Me.CH_FILTER_PLS.TabIndex = 3
            Me.CH_FILTER_PLS.Text = "Playlist"
            Me.CH_FILTER_PLS.UseVisualStyleBackColor = True
            '
            'TP_MUSIC
            '
            Me.TP_MUSIC.ColumnCount = 3
            Me.TP_MUSIC.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            Me.TP_MUSIC.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            Me.TP_MUSIC.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            Me.TP_MUSIC.Controls.Add(Me.CH_M_ALL, 0, 0)
            Me.TP_MUSIC.Controls.Add(Me.CH_M_VIDEO, 1, 0)
            Me.TP_MUSIC.Controls.Add(Me.CH_M_MUSIC, 2, 0)
            Me.TP_MUSIC.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_MUSIC.Location = New System.Drawing.Point(0, 25)
            Me.TP_MUSIC.Margin = New System.Windows.Forms.Padding(0)
            Me.TP_MUSIC.Name = "TP_MUSIC"
            Me.TP_MUSIC.RowCount = 1
            Me.TP_MUSIC.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MUSIC.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TP_MUSIC.Size = New System.Drawing.Size(384, 25)
            Me.TP_MUSIC.TabIndex = 1
            '
            'CH_M_ALL
            '
            Me.CH_M_ALL.AutoSize = True
            Me.CH_M_ALL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_M_ALL.Location = New System.Drawing.Point(3, 3)
            Me.CH_M_ALL.Name = "CH_M_ALL"
            Me.CH_M_ALL.Size = New System.Drawing.Size(122, 19)
            Me.CH_M_ALL.TabIndex = 0
            Me.CH_M_ALL.Text = "ALL"
            Me.CH_M_ALL.UseVisualStyleBackColor = True
            '
            'CH_M_VIDEO
            '
            Me.CH_M_VIDEO.AutoSize = True
            Me.CH_M_VIDEO.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_M_VIDEO.Location = New System.Drawing.Point(131, 3)
            Me.CH_M_VIDEO.Name = "CH_M_VIDEO"
            Me.CH_M_VIDEO.Size = New System.Drawing.Size(122, 19)
            Me.CH_M_VIDEO.TabIndex = 1
            Me.CH_M_VIDEO.Text = "Video"
            Me.CH_M_VIDEO.UseVisualStyleBackColor = True
            '
            'CH_M_MUSIC
            '
            Me.CH_M_MUSIC.AutoSize = True
            Me.CH_M_MUSIC.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_M_MUSIC.Location = New System.Drawing.Point(259, 3)
            Me.CH_M_MUSIC.Name = "CH_M_MUSIC"
            Me.CH_M_MUSIC.Size = New System.Drawing.Size(122, 19)
            Me.CH_M_MUSIC.TabIndex = 2
            Me.CH_M_MUSIC.Text = "Music"
            Me.CH_M_MUSIC.UseVisualStyleBackColor = True
            '
            'TP_USERS
            '
            TP_USERS.ColumnCount = 1
            TP_USERS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_USERS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_USERS.Controls.Add(Me.LIST_USERS, 0, 1)
            TP_USERS.Controls.Add(TP_USERS_2, 0, 0)
            TP_USERS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_USERS.Location = New System.Drawing.Point(0, 50)
            TP_USERS.Margin = New System.Windows.Forms.Padding(0)
            TP_USERS.Name = "TP_USERS"
            TP_USERS.RowCount = 2
            TP_USERS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_USERS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_USERS.Size = New System.Drawing.Size(384, 311)
            TP_USERS.TabIndex = 3
            '
            'LIST_USERS
            '
            Me.LIST_USERS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LIST_USERS.FormattingEnabled = True
            Me.LIST_USERS.Location = New System.Drawing.Point(3, 28)
            Me.LIST_USERS.Name = "LIST_USERS"
            Me.LIST_USERS.Size = New System.Drawing.Size(378, 280)
            Me.LIST_USERS.TabIndex = 1
            '
            'TP_USERS_2
            '
            TP_USERS_2.ColumnCount = 3
            TP_USERS_2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_USERS_2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_USERS_2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_USERS_2.Controls.Add(Me.CH_USERS_USE, 0, 0)
            TP_USERS_2.Controls.Add(Me.BTT_SELECT_ALL, 1, 0)
            TP_USERS_2.Controls.Add(Me.BTT_SELECT_NONE, 2, 0)
            TP_USERS_2.Dock = System.Windows.Forms.DockStyle.Fill
            TP_USERS_2.Location = New System.Drawing.Point(0, 0)
            TP_USERS_2.Margin = New System.Windows.Forms.Padding(0)
            TP_USERS_2.Name = "TP_USERS_2"
            TP_USERS_2.RowCount = 1
            TP_USERS_2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_USERS_2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_USERS_2.Size = New System.Drawing.Size(384, 25)
            TP_USERS_2.TabIndex = 0
            '
            'CH_USERS_USE
            '
            Me.CH_USERS_USE.AutoSize = True
            Me.CH_USERS_USE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_USERS_USE.Location = New System.Drawing.Point(3, 3)
            Me.CH_USERS_USE.Name = "CH_USERS_USE"
            Me.CH_USERS_USE.Size = New System.Drawing.Size(122, 19)
            Me.CH_USERS_USE.TabIndex = 0
            Me.CH_USERS_USE.Text = "Filter users"
            Me.CH_USERS_USE.UseVisualStyleBackColor = True
            '
            'BTT_SELECT_ALL
            '
            Me.BTT_SELECT_ALL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_SELECT_ALL.Location = New System.Drawing.Point(129, 1)
            Me.BTT_SELECT_ALL.Margin = New System.Windows.Forms.Padding(1)
            Me.BTT_SELECT_ALL.Name = "BTT_SELECT_ALL"
            Me.BTT_SELECT_ALL.Size = New System.Drawing.Size(126, 23)
            Me.BTT_SELECT_ALL.TabIndex = 1
            Me.BTT_SELECT_ALL.Text = "All"
            Me.BTT_SELECT_ALL.UseVisualStyleBackColor = True
            '
            'BTT_SELECT_NONE
            '
            Me.BTT_SELECT_NONE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_SELECT_NONE.Location = New System.Drawing.Point(257, 1)
            Me.BTT_SELECT_NONE.Margin = New System.Windows.Forms.Padding(1)
            Me.BTT_SELECT_NONE.Name = "BTT_SELECT_NONE"
            Me.BTT_SELECT_NONE.Size = New System.Drawing.Size(126, 23)
            Me.BTT_SELECT_NONE.TabIndex = 2
            Me.BTT_SELECT_NONE.Text = "None"
            Me.BTT_SELECT_NONE.UseVisualStyleBackColor = True
            '
            'FilterForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(384, 361)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(400, 400)
            Me.Name = "FilterForm"
            Me.ShowInTaskbar = False
            Me.Text = "Filter"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            TP_TYPES.ResumeLayout(False)
            TP_TYPES.PerformLayout()
            Me.TP_MUSIC.ResumeLayout(False)
            Me.TP_MUSIC.PerformLayout()
            TP_USERS.ResumeLayout(False)
            TP_USERS_2.ResumeLayout(False)
            TP_USERS_2.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents CH_FILTER_ALL As CheckBox
        Private WithEvents CH_FILTER_SINGLE As CheckBox
        Private WithEvents CH_FILTER_CHANNEL As CheckBox
        Private WithEvents CH_FILTER_PLS As CheckBox
        Private WithEvents TP_MUSIC As TableLayoutPanel
        Private WithEvents CH_M_ALL As CheckBox
        Private WithEvents CH_M_VIDEO As CheckBox
        Private WithEvents CH_M_MUSIC As CheckBox
        Private WithEvents LIST_USERS As CheckedListBox
        Private WithEvents CH_USERS_USE As CheckBox
        Private WithEvents BTT_SELECT_ALL As Button
        Private WithEvents BTT_SELECT_NONE As Button
    End Class
End Namespace