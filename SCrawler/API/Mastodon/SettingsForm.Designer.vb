' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.Mastodon
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class SettingsForm : Inherits System.Windows.Forms.Form
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SettingsForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton4 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton5 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton6 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Me.CMB_DOMAINS = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.TXT_AUTH = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_TOKEN = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            TP_MAIN.SuspendLayout()
            CType(Me.CMB_DOMAINS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_AUTH, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_TOKEN, System.ComponentModel.ISupportInitialize).BeginInit()
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
            TP_MAIN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_MAIN.Controls.Add(Me.CMB_DOMAINS, 0, 0)
            TP_MAIN.Controls.Add(Me.TXT_AUTH, 0, 1)
            TP_MAIN.Controls.Add(Me.TXT_TOKEN, 0, 2)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 3
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.Size = New System.Drawing.Size(384, 361)
            TP_MAIN.TabIndex = 0
            '
            'CMB_DOMAINS
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Name = "Add"
            ActionButton1.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Add
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Name = "Delete"
            ActionButton2.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Delete
            ActionButton3.BackgroundImage = CType(resources.GetObject("ActionButton3.BackgroundImage"), System.Drawing.Image)
            ActionButton3.Name = "Clear"
            ActionButton3.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            ActionButton4.BackgroundImage = CType(resources.GetObject("ActionButton4.BackgroundImage"), System.Drawing.Image)
            ActionButton4.Name = "ArrowDown"
            ActionButton4.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.ArrowDown
            ActionButton4.Visible = False
            Me.CMB_DOMAINS.Buttons.Add(ActionButton1)
            Me.CMB_DOMAINS.Buttons.Add(ActionButton2)
            Me.CMB_DOMAINS.Buttons.Add(ActionButton3)
            Me.CMB_DOMAINS.Buttons.Add(ActionButton4)
            Me.CMB_DOMAINS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_DOMAINS.ListDropDownStyle = PersonalUtilities.Forms.Controls.ComboBoxExtended.ListMode.Simple
            Me.CMB_DOMAINS.Location = New System.Drawing.Point(4, 4)
            Me.CMB_DOMAINS.Name = "CMB_DOMAINS"
            Me.CMB_DOMAINS.Size = New System.Drawing.Size(378, 296)
            Me.CMB_DOMAINS.TabIndex = 0
            '
            'TXT_AUTH
            '
            ActionButton5.BackgroundImage = CType(resources.GetObject("ActionButton5.BackgroundImage"), System.Drawing.Image)
            ActionButton5.Name = "Clear"
            ActionButton5.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_AUTH.Buttons.Add(ActionButton5)
            Me.TXT_AUTH.CaptionText = "Auth"
            Me.TXT_AUTH.CaptionToolTipEnabled = True
            Me.TXT_AUTH.CaptionToolTipText = "Bearer token"
            Me.TXT_AUTH.CaptionWidth = 50.0R
            Me.TXT_AUTH.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_AUTH.Location = New System.Drawing.Point(4, 306)
            Me.TXT_AUTH.Name = "TXT_AUTH"
            Me.TXT_AUTH.Size = New System.Drawing.Size(376, 22)
            Me.TXT_AUTH.TabIndex = 1
            '
            'TXT_TOKEN
            '
            ActionButton6.BackgroundImage = CType(resources.GetObject("ActionButton6.BackgroundImage"), System.Drawing.Image)
            ActionButton6.Name = "Clear"
            ActionButton6.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_TOKEN.Buttons.Add(ActionButton6)
            Me.TXT_TOKEN.CaptionText = "Token"
            Me.TXT_TOKEN.CaptionToolTipEnabled = True
            Me.TXT_TOKEN.CaptionToolTipText = "csrf token"
            Me.TXT_TOKEN.CaptionWidth = 50.0R
            Me.TXT_TOKEN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_TOKEN.Location = New System.Drawing.Point(4, 335)
            Me.TXT_TOKEN.Name = "TXT_TOKEN"
            Me.TXT_TOKEN.Size = New System.Drawing.Size(376, 22)
            Me.TXT_TOKEN.TabIndex = 2
            '
            'SettingsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(384, 361)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.Icon = Global.SCrawler.My.Resources.SiteResources.MastodonIcon_48
            Me.MinimumSize = New System.Drawing.Size(400, 400)
            Me.Name = "SettingsForm"
            Me.ShowInTaskbar = False
            Me.Text = "Mastodon domains"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            CType(Me.CMB_DOMAINS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_AUTH, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_TOKEN, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents CMB_DOMAINS As PersonalUtilities.Forms.Controls.ComboBoxExtended
        Private WithEvents TXT_AUTH As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_TOKEN As PersonalUtilities.Forms.Controls.TextBoxExtended
    End Class
End Namespace