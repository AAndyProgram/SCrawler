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
    Partial Friend Class ChannelTabsChooserForm : Inherits System.Windows.Forms.Form
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
            Me.CH_ALL = New System.Windows.Forms.CheckBox()
            Me.CH_VIDEOS = New System.Windows.Forms.CheckBox()
            Me.CH_SHORTS = New System.Windows.Forms.CheckBox()
            Me.CH_PLS = New System.Windows.Forms.CheckBox()
            Me.TXT_URL = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_URL_ASIS = New System.Windows.Forms.CheckBox()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            TP_MAIN.SuspendLayout()
            CType(Me.TXT_URL, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(474, 159)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(474, 184)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            TP_MAIN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(Me.CH_ALL, 0, 2)
            TP_MAIN.Controls.Add(Me.CH_VIDEOS, 0, 3)
            TP_MAIN.Controls.Add(Me.CH_SHORTS, 0, 4)
            TP_MAIN.Controls.Add(Me.CH_PLS, 0, 5)
            TP_MAIN.Controls.Add(Me.TXT_URL, 0, 0)
            TP_MAIN.Controls.Add(Me.CH_URL_ASIS, 0, 1)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 7
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Size = New System.Drawing.Size(474, 159)
            TP_MAIN.TabIndex = 0
            '
            'CH_ALL
            '
            Me.CH_ALL.AutoSize = True
            Me.CH_ALL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_ALL.Location = New System.Drawing.Point(4, 59)
            Me.CH_ALL.Name = "CH_ALL"
            Me.CH_ALL.Size = New System.Drawing.Size(466, 19)
            Me.CH_ALL.TabIndex = 2
            Me.CH_ALL.Text = "ALL"
            Me.CH_ALL.UseVisualStyleBackColor = True
            '
            'CH_VIDEOS
            '
            Me.CH_VIDEOS.AutoSize = True
            Me.CH_VIDEOS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_VIDEOS.Location = New System.Drawing.Point(4, 85)
            Me.CH_VIDEOS.Name = "CH_VIDEOS"
            Me.CH_VIDEOS.Size = New System.Drawing.Size(466, 19)
            Me.CH_VIDEOS.TabIndex = 3
            Me.CH_VIDEOS.Text = "Videos"
            Me.CH_VIDEOS.UseVisualStyleBackColor = True
            '
            'CH_SHORTS
            '
            Me.CH_SHORTS.AutoSize = True
            Me.CH_SHORTS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_SHORTS.Location = New System.Drawing.Point(4, 111)
            Me.CH_SHORTS.Name = "CH_SHORTS"
            Me.CH_SHORTS.Size = New System.Drawing.Size(466, 19)
            Me.CH_SHORTS.TabIndex = 4
            Me.CH_SHORTS.Text = "Shorts"
            Me.CH_SHORTS.UseVisualStyleBackColor = True
            '
            'CH_PLS
            '
            Me.CH_PLS.AutoSize = True
            Me.CH_PLS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_PLS.Location = New System.Drawing.Point(4, 137)
            Me.CH_PLS.Name = "CH_PLS"
            Me.CH_PLS.Size = New System.Drawing.Size(466, 19)
            Me.CH_PLS.TabIndex = 5
            Me.CH_PLS.Text = "Playlists"
            Me.CH_PLS.UseVisualStyleBackColor = True
            '
            'TXT_URL
            '
            Me.TXT_URL.CaptionText = "Channel URL"
            Me.TXT_URL.CaptionWidth = 80.0R
            Me.TXT_URL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_URL.Location = New System.Drawing.Point(4, 4)
            Me.TXT_URL.Name = "TXT_URL"
            Me.TXT_URL.Size = New System.Drawing.Size(466, 22)
            Me.TXT_URL.TabIndex = 0
            '
            'CH_URL_ASIS
            '
            Me.CH_URL_ASIS.AutoSize = True
            Me.CH_URL_ASIS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_URL_ASIS.Location = New System.Drawing.Point(4, 33)
            Me.CH_URL_ASIS.Name = "CH_URL_ASIS"
            Me.CH_URL_ASIS.Size = New System.Drawing.Size(466, 19)
            Me.CH_URL_ASIS.TabIndex = 1
            Me.CH_URL_ASIS.Text = "Download URL as is"
            Me.CH_URL_ASIS.UseVisualStyleBackColor = True
            '
            'ChannelTabsChooserForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(474, 184)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Icon = Global.SCrawler.My.Resources.SiteYouTube.YouTubeIcon_32
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(490, 223)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(490, 223)
            Me.Name = "ChannelTabsChooserForm"
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Tabs"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            TP_MAIN.PerformLayout()
            CType(Me.TXT_URL, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents CH_ALL As CheckBox
        Private WithEvents CH_VIDEOS As CheckBox
        Private WithEvents CH_SHORTS As CheckBox
        Private WithEvents CH_PLS As CheckBox
        Private WithEvents TXT_URL As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CH_URL_ASIS As CheckBox
    End Class
End Namespace