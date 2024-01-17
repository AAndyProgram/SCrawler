' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.YouTube.Controls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class PlaylistArrayForm : Inherits System.Windows.Forms.Form
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
            Dim FRM_PLS As System.Windows.Forms.GroupBox
            Me.CH_PLS_ONE = New System.Windows.Forms.CheckBox()
            Me.TXT_URLS = New System.Windows.Forms.TextBox()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            FRM_PLS = New System.Windows.Forms.GroupBox()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            TP_MAIN.SuspendLayout()
            FRM_PLS.SuspendLayout()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(384, 311)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(384, 311)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_MAIN.Controls.Add(Me.CH_PLS_ONE, 0, 0)
            TP_MAIN.Controls.Add(FRM_PLS, 0, 1)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 2
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Size = New System.Drawing.Size(384, 311)
            TP_MAIN.TabIndex = 0
            '
            'CH_PLS_ONE
            '
            Me.CH_PLS_ONE.AutoSize = True
            Me.CH_PLS_ONE.Checked = True
            Me.CH_PLS_ONE.CheckState = System.Windows.Forms.CheckState.Checked
            Me.CH_PLS_ONE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_PLS_ONE.Location = New System.Drawing.Point(3, 3)
            Me.CH_PLS_ONE.Name = "CH_PLS_ONE"
            Me.CH_PLS_ONE.Size = New System.Drawing.Size(378, 19)
            Me.CH_PLS_ONE.TabIndex = 1
            Me.CH_PLS_ONE.Text = "Playlists / Albums by one artist"
            Me.CH_PLS_ONE.UseVisualStyleBackColor = True
            '
            'FRM_PLS
            '
            FRM_PLS.Controls.Add(Me.TXT_URLS)
            FRM_PLS.Dock = System.Windows.Forms.DockStyle.Fill
            FRM_PLS.Location = New System.Drawing.Point(3, 28)
            FRM_PLS.Name = "FRM_PLS"
            FRM_PLS.Size = New System.Drawing.Size(378, 280)
            FRM_PLS.TabIndex = 0
            FRM_PLS.TabStop = False
            FRM_PLS.Text = "URLs (new line as delimiter); Ctrl+O to parse playlists from response"
            '
            'TXT_URLS
            '
            Me.TXT_URLS.AcceptsReturn = True
            Me.TXT_URLS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_URLS.Location = New System.Drawing.Point(3, 16)
            Me.TXT_URLS.MaxLength = 2147483647
            Me.TXT_URLS.Multiline = True
            Me.TXT_URLS.Name = "TXT_URLS"
            Me.TXT_URLS.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.TXT_URLS.Size = New System.Drawing.Size(372, 261)
            Me.TXT_URLS.TabIndex = 0
            '
            'PlaylistArrayForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(384, 311)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.Icon = Global.SCrawler.My.Resources.SiteYouTube.YouTubeMusicIcon_32
            Me.KeyPreview = True
            Me.MinimumSize = New System.Drawing.Size(400, 350)
            Me.Name = "PlaylistArrayForm"
            Me.Text = "Playlists / Albums"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            TP_MAIN.PerformLayout()
            FRM_PLS.ResumeLayout(False)
            FRM_PLS.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents CH_PLS_ONE As CheckBox
        Private WithEvents TXT_URLS As TextBox
    End Class
End Namespace