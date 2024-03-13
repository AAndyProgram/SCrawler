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
            Dim ActionButton6 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DownloaderUrlsArrForm))
            Dim ActionButton7 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton8 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton9 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ListColumn3 As PersonalUtilities.Forms.Controls.Base.ListColumn = New PersonalUtilities.Forms.Controls.Base.ListColumn()
            Dim ListColumn4 As PersonalUtilities.Forms.Controls.Base.ListColumn = New PersonalUtilities.Forms.Controls.Base.ListColumn()
            Dim FRM_URLS As System.Windows.Forms.GroupBox
            Dim ActionButton10 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Me.TXT_OUTPUT = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.TXT_URLS = New System.Windows.Forms.TextBox()
            Me.CMB_ACCOUNT = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.CH_THUMB_ALONG = New System.Windows.Forms.CheckBox()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            FRM_URLS = New System.Windows.Forms.GroupBox()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            TP_MAIN.SuspendLayout()
            CType(Me.TXT_OUTPUT, System.ComponentModel.ISupportInitialize).BeginInit()
            FRM_URLS.SuspendLayout()
            CType(Me.CMB_ACCOUNT, System.ComponentModel.ISupportInitialize).BeginInit()
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
            TP_MAIN.Controls.Add(Me.TXT_OUTPUT, 0, 0)
            TP_MAIN.Controls.Add(FRM_URLS, 0, 3)
            TP_MAIN.Controls.Add(Me.CMB_ACCOUNT, 0, 1)
            TP_MAIN.Controls.Add(Me.CH_THUMB_ALONG, 0, 2)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 4
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Size = New System.Drawing.Size(384, 261)
            TP_MAIN.TabIndex = 0
            '
            'TXT_OUTPUT
            '
            ActionButton6.BackgroundImage = CType(resources.GetObject("ActionButton6.BackgroundImage"), System.Drawing.Image)
            ActionButton6.Name = "Open"
            ActionButton6.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Open
            ActionButton6.ToolTipText = "Choose a new location (Ctrl+O)"
            ActionButton7.BackgroundImage = CType(resources.GetObject("ActionButton7.BackgroundImage"), System.Drawing.Image)
            ActionButton7.Name = "Add"
            ActionButton7.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Add
            ActionButton7.ToolTipText = "Choose a new location and add it to the list (Alt+O)"
            ActionButton8.BackgroundImage = CType(resources.GetObject("ActionButton8.BackgroundImage"), System.Drawing.Image)
            ActionButton8.Name = "Clear"
            ActionButton8.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            ActionButton9.BackgroundImage = CType(resources.GetObject("ActionButton9.BackgroundImage"), System.Drawing.Image)
            ActionButton9.Name = "ArrowDown"
            ActionButton9.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.ArrowDown
            Me.TXT_OUTPUT.Buttons.Add(ActionButton6)
            Me.TXT_OUTPUT.Buttons.Add(ActionButton7)
            Me.TXT_OUTPUT.Buttons.Add(ActionButton8)
            Me.TXT_OUTPUT.Buttons.Add(ActionButton9)
            Me.TXT_OUTPUT.CaptionText = "Output path"
            Me.TXT_OUTPUT.CaptionWidth = 70.0R
            ListColumn3.Name = "COL_NAME"
            ListColumn3.Text = "Name"
            ListColumn3.Width = -1
            ListColumn4.DisplayMember = True
            ListColumn4.Name = "COL_VALUE"
            ListColumn4.Text = "Value"
            ListColumn4.ValueMember = True
            ListColumn4.Visible = False
            Me.TXT_OUTPUT.Columns.Add(ListColumn3)
            Me.TXT_OUTPUT.Columns.Add(ListColumn4)
            Me.TXT_OUTPUT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_OUTPUT.ListAutoCompleteMode = PersonalUtilities.Forms.Controls.ComboBoxExtended.AutoCompleteModes.Disabled
            Me.TXT_OUTPUT.Location = New System.Drawing.Point(3, 3)
            Me.TXT_OUTPUT.Name = "TXT_OUTPUT"
            Me.TXT_OUTPUT.Size = New System.Drawing.Size(378, 22)
            Me.TXT_OUTPUT.TabIndex = 0
            Me.TXT_OUTPUT.TextBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            '
            'FRM_URLS
            '
            FRM_URLS.Controls.Add(Me.TXT_URLS)
            FRM_URLS.Dock = System.Windows.Forms.DockStyle.Fill
            FRM_URLS.Location = New System.Drawing.Point(3, 84)
            FRM_URLS.Name = "FRM_URLS"
            FRM_URLS.Size = New System.Drawing.Size(378, 174)
            FRM_URLS.TabIndex = 2
            FRM_URLS.TabStop = False
            FRM_URLS.Text = "URLs (new line as delimiter)"
            '
            'TXT_URLS
            '
            Me.TXT_URLS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_URLS.Location = New System.Drawing.Point(3, 16)
            Me.TXT_URLS.MaxLength = 2147483647
            Me.TXT_URLS.Multiline = True
            Me.TXT_URLS.Name = "TXT_URLS"
            Me.TXT_URLS.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.TXT_URLS.Size = New System.Drawing.Size(372, 155)
            Me.TXT_URLS.TabIndex = 0
            '
            'CMB_ACCOUNT
            '
            ActionButton10.BackgroundImage = CType(resources.GetObject("ActionButton10.BackgroundImage"), System.Drawing.Image)
            ActionButton10.Name = "ArrowDown"
            ActionButton10.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.ArrowDown
            Me.CMB_ACCOUNT.Buttons.Add(ActionButton10)
            Me.CMB_ACCOUNT.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.Label
            Me.CMB_ACCOUNT.CaptionText = "Account"
            Me.CMB_ACCOUNT.CaptionToolTipEnabled = True
            Me.CMB_ACCOUNT.CaptionToolTipText = "Select an account to download media array"
            Me.CMB_ACCOUNT.CaptionVisible = True
            Me.CMB_ACCOUNT.CaptionWidth = 50.0R
            Me.CMB_ACCOUNT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_ACCOUNT.Location = New System.Drawing.Point(3, 31)
            Me.CMB_ACCOUNT.Name = "CMB_ACCOUNT"
            Me.CMB_ACCOUNT.Size = New System.Drawing.Size(378, 22)
            Me.CMB_ACCOUNT.TabIndex = 1
            Me.CMB_ACCOUNT.TextBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            '
            'CH_THUMB_ALONG
            '
            Me.CH_THUMB_ALONG.AutoSize = True
            Me.CH_THUMB_ALONG.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_THUMB_ALONG.Location = New System.Drawing.Point(3, 59)
            Me.CH_THUMB_ALONG.Name = "CH_THUMB_ALONG"
            Me.CH_THUMB_ALONG.Size = New System.Drawing.Size(378, 19)
            Me.CH_THUMB_ALONG.TabIndex = 3
            Me.CH_THUMB_ALONG.Text = "Save thumbnail with file"
            Me.CH_THUMB_ALONG.UseVisualStyleBackColor = True
            '
            'DownloaderUrlsArrForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(384, 261)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.Icon = Global.SCrawler.My.Resources.Resources.ArrowDownIcon_Blue_24
            Me.KeyPreview = True
            Me.MinimumSize = New System.Drawing.Size(400, 300)
            Me.Name = "DownloaderUrlsArrForm"
            Me.ShowInTaskbar = False
            Me.Text = "Urls array"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            TP_MAIN.PerformLayout()
            CType(Me.TXT_OUTPUT, System.ComponentModel.ISupportInitialize).EndInit()
            FRM_URLS.ResumeLayout(False)
            FRM_URLS.PerformLayout()
            CType(Me.CMB_ACCOUNT, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents TXT_OUTPUT As PersonalUtilities.Forms.Controls.ComboBoxExtended
        Private WithEvents TXT_URLS As TextBox
        Private WithEvents CMB_ACCOUNT As PersonalUtilities.Forms.Controls.ComboBoxExtended
        Private WithEvents CH_THUMB_ALONG As CheckBox
    End Class
End Namespace