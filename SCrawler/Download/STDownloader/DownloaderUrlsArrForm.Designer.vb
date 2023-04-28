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
    Partial Friend Class DownloaderUrlsArrForm : Inherits System.Windows.Forms.Form
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
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DownloaderUrlsArrForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim FRM_URLS As System.Windows.Forms.GroupBox
            Me.TXT_OUTPUT = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_URLS = New System.Windows.Forms.RichTextBox()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            FRM_URLS = New System.Windows.Forms.GroupBox()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            TP_MAIN.SuspendLayout()
            CType(Me.TXT_OUTPUT, System.ComponentModel.ISupportInitialize).BeginInit()
            FRM_URLS.SuspendLayout()
            Me.SuspendLayout()
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
            'TP_MAIN
            '
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_MAIN.Controls.Add(Me.TXT_OUTPUT, 0, 0)
            TP_MAIN.Controls.Add(FRM_URLS, 0, 1)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 2
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Size = New System.Drawing.Size(384, 261)
            TP_MAIN.TabIndex = 0
            '
            'TXT_OUTPUT
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Name = "Open"
            ActionButton1.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Open
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Name = "Clear"
            ActionButton2.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_OUTPUT.Buttons.Add(ActionButton1)
            Me.TXT_OUTPUT.Buttons.Add(ActionButton2)
            Me.TXT_OUTPUT.CaptionText = "Output path"
            Me.TXT_OUTPUT.CaptionWidth = 70.0R
            Me.TXT_OUTPUT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_OUTPUT.Location = New System.Drawing.Point(3, 3)
            Me.TXT_OUTPUT.Name = "TXT_OUTPUT"
            Me.TXT_OUTPUT.Size = New System.Drawing.Size(378, 22)
            Me.TXT_OUTPUT.TabIndex = 0
            '
            'FRM_URLS
            '
            FRM_URLS.Controls.Add(Me.TXT_URLS)
            FRM_URLS.Dock = System.Windows.Forms.DockStyle.Fill
            FRM_URLS.Location = New System.Drawing.Point(3, 31)
            FRM_URLS.Name = "FRM_URLS"
            FRM_URLS.Size = New System.Drawing.Size(378, 227)
            FRM_URLS.TabIndex = 1
            FRM_URLS.TabStop = False
            FRM_URLS.Text = "URLs (new line as delimiter)"
            '
            'TXT_URLS
            '
            Me.TXT_URLS.DetectUrls = False
            Me.TXT_URLS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_URLS.Location = New System.Drawing.Point(3, 16)
            Me.TXT_URLS.Name = "TXT_URLS"
            Me.TXT_URLS.Size = New System.Drawing.Size(372, 208)
            Me.TXT_URLS.TabIndex = 0
            Me.TXT_URLS.Text = ""
            '
            'DownloaderUrlsArrForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(384, 261)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.MinimumSize = New System.Drawing.Size(400, 300)
            Me.Name = "DownloaderUrlsArrForm"
            Me.Icon = Global.SCrawler.My.Resources.ArrowDownIcon_Blue_24
            Me.ShowInTaskbar = False
            Me.Text = "Urls array"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            CType(Me.TXT_OUTPUT, System.ComponentModel.ISupportInitialize).EndInit()
            FRM_URLS.ResumeLayout(False)
            Me.ResumeLayout(False)
        End Sub
        Private WithEvents TXT_OUTPUT As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_URLS As RichTextBox
    End Class
End Namespace