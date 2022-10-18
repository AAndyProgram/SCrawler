' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Friend Class ChannelsStatsForm : Inherits System.Windows.Forms.Form
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
        Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ChannelsStatsForm))
        Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
        Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
        Dim ListColumn1 As PersonalUtilities.Forms.Controls.Base.ListColumn = New PersonalUtilities.Forms.Controls.Base.ListColumn()
        Dim ListColumn2 As PersonalUtilities.Forms.Controls.Base.ListColumn = New PersonalUtilities.Forms.Controls.Base.ListColumn()
        Me.CMB_CHANNELS = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
        CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
        CONTAINER_MAIN.ContentPanel.SuspendLayout()
        CONTAINER_MAIN.SuspendLayout()
        CType(Me.CMB_CHANNELS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CONTAINER_MAIN
        '
        '
        'CONTAINER_MAIN.ContentPanel
        '
        CONTAINER_MAIN.ContentPanel.Controls.Add(Me.CMB_CHANNELS)
        CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(384, 236)
        CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
        CONTAINER_MAIN.LeftToolStripPanelVisible = False
        CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
        CONTAINER_MAIN.Name = "CONTAINER_MAIN"
        CONTAINER_MAIN.RightToolStripPanelVisible = False
        CONTAINER_MAIN.Size = New System.Drawing.Size(384, 261)
        CONTAINER_MAIN.TabIndex = 0
        CONTAINER_MAIN.TopToolStripPanelVisible = False
        '
        'CMB_CHANNELS
        '
        ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
        ActionButton1.Name = "Clear"
        ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
        ActionButton2.Name = "Delete"
        ActionButton3.BackgroundImage = CType(resources.GetObject("ActionButton3.BackgroundImage"), System.Drawing.Image)
        ActionButton3.Name = "ArrowDown"
        ActionButton3.Visible = False
        Me.CMB_CHANNELS.Buttons.Add(ActionButton1)
        Me.CMB_CHANNELS.Buttons.Add(ActionButton2)
        Me.CMB_CHANNELS.Buttons.Add(ActionButton3)
        Me.CMB_CHANNELS.ClearTextByButtonClear = False
        ListColumn1.DisplayMember = True
        ListColumn1.Name = "COL_INFO"
        ListColumn1.Text = "Information"
        ListColumn1.Width = -2
        ListColumn2.Name = "COL_VALUE"
        ListColumn2.Text = "Channel"
        ListColumn2.ValueMember = True
        ListColumn2.Visible = False
        Me.CMB_CHANNELS.Columns.Add(ListColumn1)
        Me.CMB_CHANNELS.Columns.Add(ListColumn2)
        Me.CMB_CHANNELS.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CMB_CHANNELS.ListCheckBoxes = True
        Me.CMB_CHANNELS.ListDropDownStyle = PersonalUtilities.Forms.Controls.ComboBoxExtended.ListMode.Simple
        Me.CMB_CHANNELS.ListGridVisible = True
        Me.CMB_CHANNELS.ListMultiSelect = True
        Me.CMB_CHANNELS.Location = New System.Drawing.Point(0, 0)
        Me.CMB_CHANNELS.Name = "CMB_CHANNELS"
        Me.CMB_CHANNELS.Size = New System.Drawing.Size(386, 237)
        Me.CMB_CHANNELS.TabIndex = 0
        '
        'ChannelsStatsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 261)
        Me.Controls.Add(CONTAINER_MAIN)
        Me.Icon = Global.SCrawler.My.Resources.SiteResources.RedditIcon_128
        Me.KeyPreview = True
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(400, 300)
        Me.Name = "ChannelsStatsForm"
        Me.ShowInTaskbar = False
        Me.Text = "Channels statistics"
        CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
        CONTAINER_MAIN.ResumeLayout(False)
        CONTAINER_MAIN.PerformLayout()
        CType(Me.CMB_CHANNELS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents CMB_CHANNELS As PersonalUtilities.Forms.Controls.ComboBoxExtended
End Class