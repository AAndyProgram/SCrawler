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
    Partial Friend Class PlayListParserForm : Inherits System.Windows.Forms.Form
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
            Dim FRM_IN As System.Windows.Forms.GroupBox
            Dim FRM_OUT As System.Windows.Forms.GroupBox
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PlayListParserForm))
            Dim CONTAINER_MAIN As System.Windows.Forms.ToolStripContainer
            Dim TT_MAIN As System.Windows.Forms.ToolTip
            Me.TXT_IN = New System.Windows.Forms.RichTextBox()
            Me.TXT_OUT = New System.Windows.Forms.RichTextBox()
            Me.TXT_LIMIT = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            FRM_IN = New System.Windows.Forms.GroupBox()
            FRM_OUT = New System.Windows.Forms.GroupBox()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
            TP_MAIN.SuspendLayout()
            FRM_IN.SuspendLayout()
            FRM_OUT.SuspendLayout()
            CType(Me.TXT_LIMIT, System.ComponentModel.ISupportInitialize).BeginInit()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            Me.SuspendLayout()
            '
            'TP_MAIN
            '
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(FRM_IN, 0, 1)
            TP_MAIN.Controls.Add(FRM_OUT, 0, 2)
            TP_MAIN.Controls.Add(Me.TXT_LIMIT, 0, 0)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Margin = New System.Windows.Forms.Padding(0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 3
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_MAIN.Size = New System.Drawing.Size(384, 261)
            TP_MAIN.TabIndex = 0
            '
            'FRM_IN
            '
            FRM_IN.Controls.Add(Me.TXT_IN)
            FRM_IN.Dock = System.Windows.Forms.DockStyle.Fill
            FRM_IN.Location = New System.Drawing.Point(3, 31)
            FRM_IN.Name = "FRM_IN"
            FRM_IN.Size = New System.Drawing.Size(378, 110)
            FRM_IN.TabIndex = 0
            FRM_IN.TabStop = False
            FRM_IN.Text = "In"
            TT_MAIN.SetToolTip(FRM_IN, "In your browser's DevTools, find the page starting with the following URL and cop" &
        "y the response text into this window." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "https://music.youtube.com/youtubei/v1/bro" &
        "wse?key=")
            '
            'TXT_IN
            '
            Me.TXT_IN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_IN.Location = New System.Drawing.Point(3, 16)
            Me.TXT_IN.Name = "TXT_IN"
            Me.TXT_IN.Size = New System.Drawing.Size(372, 91)
            Me.TXT_IN.TabIndex = 0
            Me.TXT_IN.Text = ""
            '
            'FRM_OUT
            '
            FRM_OUT.Controls.Add(Me.TXT_OUT)
            FRM_OUT.Dock = System.Windows.Forms.DockStyle.Fill
            FRM_OUT.Location = New System.Drawing.Point(3, 147)
            FRM_OUT.Name = "FRM_OUT"
            FRM_OUT.Size = New System.Drawing.Size(378, 111)
            FRM_OUT.TabIndex = 1
            FRM_OUT.TabStop = False
            FRM_OUT.Text = "Out"
            '
            'TXT_OUT
            '
            Me.TXT_OUT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_OUT.Location = New System.Drawing.Point(3, 16)
            Me.TXT_OUT.Name = "TXT_OUT"
            Me.TXT_OUT.Size = New System.Drawing.Size(372, 92)
            Me.TXT_OUT.TabIndex = 0
            Me.TXT_OUT.Text = ""
            '
            'TXT_LIMIT
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Name = "Clear"
            ActionButton1.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_LIMIT.Buttons.Add(ActionButton1)
            Me.TXT_LIMIT.CaptionText = "Remove"
            Me.TXT_LIMIT.CaptionToolTipEnabled = True
            Me.TXT_LIMIT.CaptionToolTipText = "Remove playlists starts with..."
            Me.TXT_LIMIT.CaptionWidth = 50.0R
            Me.TXT_LIMIT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_LIMIT.Location = New System.Drawing.Point(3, 3)
            Me.TXT_LIMIT.Name = "TXT_LIMIT"
            Me.TXT_LIMIT.PlaceholderEnabled = True
            Me.TXT_LIMIT.PlaceholderText = "e.g. RDAMP"
            Me.TXT_LIMIT.Size = New System.Drawing.Size(378, 22)
            Me.TXT_LIMIT.TabIndex = 2
            Me.TXT_LIMIT.Text = "RDAMP"
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(384, 261)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(384, 261)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'PlayListParserForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(384, 261)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.Icon = Global.SCrawler.My.Resources.SiteYouTube.YouTubeMusicIcon_32
            Me.KeyPreview = True
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(400, 300)
            Me.Name = "PlayListParserForm"
            Me.ShowInTaskbar = False
            Me.Text = "Playlist parser"
            TP_MAIN.ResumeLayout(False)
            FRM_IN.ResumeLayout(False)
            FRM_OUT.ResumeLayout(False)
            CType(Me.TXT_LIMIT, System.ComponentModel.ISupportInitialize).EndInit()
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents TXT_IN As RichTextBox
        Private WithEvents TXT_OUT As RichTextBox
        Private WithEvents TXT_LIMIT As PersonalUtilities.Forms.Controls.TextBoxExtended
    End Class
End Namespace