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
    Partial Friend Class DownloaderUrlForm : Inherits System.Windows.Forms.Form
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DownloaderUrlForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton4 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton5 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ListColumn1 As PersonalUtilities.Forms.Controls.Base.ListColumn = New PersonalUtilities.Forms.Controls.Base.ListColumn()
            Dim ListColumn2 As PersonalUtilities.Forms.Controls.Base.ListColumn = New PersonalUtilities.Forms.Controls.Base.ListColumn()
            Dim ActionButton6 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Me.TXT_URL = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_PATH = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.CMB_ACCOUNT = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.CH_THUMB_ALONG = New System.Windows.Forms.CheckBox()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            TP_MAIN.SuspendLayout()
            CType(Me.TXT_URL, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_PATH, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.CMB_ACCOUNT, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(484, 114)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(484, 139)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            TP_MAIN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(Me.TXT_URL, 0, 0)
            TP_MAIN.Controls.Add(Me.TXT_PATH, 0, 1)
            TP_MAIN.Controls.Add(Me.CMB_ACCOUNT, 0, 2)
            TP_MAIN.Controls.Add(Me.CH_THUMB_ALONG, 0, 3)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 5
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Size = New System.Drawing.Size(484, 114)
            TP_MAIN.TabIndex = 0
            '
            'TXT_URL
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Name = "Clear"
            ActionButton1.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_URL.Buttons.Add(ActionButton1)
            Me.TXT_URL.CaptionText = "URL"
            Me.TXT_URL.CaptionWidth = 50.0R
            Me.TXT_URL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_URL.Location = New System.Drawing.Point(4, 4)
            Me.TXT_URL.Name = "TXT_URL"
            Me.TXT_URL.Size = New System.Drawing.Size(476, 22)
            Me.TXT_URL.TabIndex = 0
            '
            'TXT_PATH
            '
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Name = "Open"
            ActionButton2.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Open
            ActionButton2.ToolTipText = "Choose a new location (Ctrl+O)"
            ActionButton3.BackgroundImage = CType(resources.GetObject("ActionButton3.BackgroundImage"), System.Drawing.Image)
            ActionButton3.Name = "Add"
            ActionButton3.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Add
            ActionButton3.ToolTipText = "Choose a new location and add it to the list (Alt+O)"
            ActionButton4.BackgroundImage = CType(resources.GetObject("ActionButton4.BackgroundImage"), System.Drawing.Image)
            ActionButton4.Name = "Clear"
            ActionButton4.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            ActionButton5.BackgroundImage = CType(resources.GetObject("ActionButton5.BackgroundImage"), System.Drawing.Image)
            ActionButton5.Name = "ArrowDown"
            ActionButton5.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.ArrowDown
            Me.TXT_PATH.Buttons.Add(ActionButton2)
            Me.TXT_PATH.Buttons.Add(ActionButton3)
            Me.TXT_PATH.Buttons.Add(ActionButton4)
            Me.TXT_PATH.Buttons.Add(ActionButton5)
            Me.TXT_PATH.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.Label
            Me.TXT_PATH.CaptionText = "Output"
            Me.TXT_PATH.CaptionToolTipEnabled = True
            Me.TXT_PATH.CaptionToolTipText = "Output path"
            Me.TXT_PATH.CaptionVisible = True
            Me.TXT_PATH.CaptionWidth = 50.0R
            ListColumn1.Name = "COL_NAME"
            ListColumn1.Text = "Name"
            ListColumn1.Width = -1
            ListColumn2.DisplayMember = True
            ListColumn2.Name = "COL_VALUE"
            ListColumn2.Text = "Value"
            ListColumn2.ValueMember = True
            ListColumn2.Visible = False
            Me.TXT_PATH.Columns.Add(ListColumn1)
            Me.TXT_PATH.Columns.Add(ListColumn2)
            Me.TXT_PATH.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_PATH.ListAutoCompleteMode = PersonalUtilities.Forms.Controls.ComboBoxExtended.AutoCompleteModes.Disabled
            Me.TXT_PATH.Location = New System.Drawing.Point(4, 33)
            Me.TXT_PATH.Name = "TXT_PATH"
            Me.TXT_PATH.Size = New System.Drawing.Size(476, 22)
            Me.TXT_PATH.TabIndex = 1
            Me.TXT_PATH.TextBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            '
            'CMB_ACCOUNT
            '
            ActionButton6.BackgroundImage = CType(resources.GetObject("ActionButton6.BackgroundImage"), System.Drawing.Image)
            ActionButton6.Name = "ArrowDown"
            ActionButton6.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.ArrowDown
            Me.CMB_ACCOUNT.Buttons.Add(ActionButton6)
            Me.CMB_ACCOUNT.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.Label
            Me.CMB_ACCOUNT.CaptionText = "Account"
            Me.CMB_ACCOUNT.CaptionToolTipEnabled = True
            Me.CMB_ACCOUNT.CaptionToolTipText = "Select an account to download this media"
            Me.CMB_ACCOUNT.CaptionVisible = True
            Me.CMB_ACCOUNT.CaptionWidth = 50.0R
            Me.CMB_ACCOUNT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_ACCOUNT.Location = New System.Drawing.Point(4, 62)
            Me.CMB_ACCOUNT.Name = "CMB_ACCOUNT"
            Me.CMB_ACCOUNT.Size = New System.Drawing.Size(476, 22)
            Me.CMB_ACCOUNT.TabIndex = 2
            Me.CMB_ACCOUNT.TextBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            '
            'CH_THUMB_ALONG
            '
            Me.CH_THUMB_ALONG.AutoSize = True
            Me.CH_THUMB_ALONG.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_THUMB_ALONG.Location = New System.Drawing.Point(4, 91)
            Me.CH_THUMB_ALONG.Name = "CH_THUMB_ALONG"
            Me.CH_THUMB_ALONG.Size = New System.Drawing.Size(476, 19)
            Me.CH_THUMB_ALONG.TabIndex = 3
            Me.CH_THUMB_ALONG.Text = "Save thumbnail with file"
            Me.CH_THUMB_ALONG.UseVisualStyleBackColor = True
            '
            'DownloaderUrlForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(484, 139)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Icon = Global.SCrawler.My.Resources.Resources.ArrowDownIcon_Blue_24
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(500, 178)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(500, 178)
            Me.Name = "DownloaderUrlForm"
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "URL"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            TP_MAIN.PerformLayout()
            CType(Me.TXT_URL, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_PATH, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.CMB_ACCOUNT, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents TXT_URL As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_PATH As PersonalUtilities.Forms.Controls.ComboBoxExtended
        Private WithEvents CMB_ACCOUNT As PersonalUtilities.Forms.Controls.ComboBoxExtended
        Private WithEvents CH_THUMB_ALONG As CheckBox
    End Class
End Namespace