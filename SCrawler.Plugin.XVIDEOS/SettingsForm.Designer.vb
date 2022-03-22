' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class SettingsForm : Inherits System.Windows.Forms.Form
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SettingsForm))
        Me.LIST_DOMAINS = New System.Windows.Forms.ListBox()
        Me.ToolbarTOP = New System.Windows.Forms.ToolStrip()
        Me.BTT_ADD = New System.Windows.Forms.ToolStripButton()
        Me.BTT_DELETE = New System.Windows.Forms.ToolStripButton()
        CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
        CONTAINER_MAIN.ContentPanel.SuspendLayout()
        CONTAINER_MAIN.TopToolStripPanel.SuspendLayout()
        CONTAINER_MAIN.SuspendLayout()
        Me.ToolbarTOP.SuspendLayout()
        Me.SuspendLayout()
        '
        'CONTAINER_MAIN
        '
        '
        'CONTAINER_MAIN.ContentPanel
        '
        CONTAINER_MAIN.ContentPanel.Controls.Add(Me.LIST_DOMAINS)
        CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(384, 266)
        CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
        CONTAINER_MAIN.LeftToolStripPanelVisible = False
        CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
        CONTAINER_MAIN.Name = "CONTAINER_MAIN"
        CONTAINER_MAIN.RightToolStripPanelVisible = False
        CONTAINER_MAIN.Size = New System.Drawing.Size(384, 291)
        CONTAINER_MAIN.TabIndex = 0
        '
        'CONTAINER_MAIN.TopToolStripPanel
        '
        CONTAINER_MAIN.TopToolStripPanel.Controls.Add(Me.ToolbarTOP)
        '
        'LIST_DOMAINS
        '
        Me.LIST_DOMAINS.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LIST_DOMAINS.FormattingEnabled = True
        Me.LIST_DOMAINS.Location = New System.Drawing.Point(0, 0)
        Me.LIST_DOMAINS.Name = "LIST_DOMAINS"
        Me.LIST_DOMAINS.Size = New System.Drawing.Size(384, 266)
        Me.LIST_DOMAINS.TabIndex = 0
        '
        'ToolbarTOP
        '
        Me.ToolbarTOP.Dock = System.Windows.Forms.DockStyle.None
        Me.ToolbarTOP.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolbarTOP.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_ADD, Me.BTT_DELETE})
        Me.ToolbarTOP.Location = New System.Drawing.Point(0, 0)
        Me.ToolbarTOP.Name = "ToolbarTOP"
        Me.ToolbarTOP.Size = New System.Drawing.Size(384, 25)
        Me.ToolbarTOP.Stretch = True
        Me.ToolbarTOP.TabIndex = 0
        '
        'BTT_ADD
        '
        Me.BTT_ADD.AutoToolTip = False
        Me.BTT_ADD.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.BTT_ADD.ForeColor = System.Drawing.Color.DarkGreen
        Me.BTT_ADD.Image = CType(resources.GetObject("BTT_ADD.Image"), System.Drawing.Image)
        Me.BTT_ADD.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_ADD.Name = "BTT_ADD"
        Me.BTT_ADD.Size = New System.Drawing.Size(49, 22)
        Me.BTT_ADD.Text = "Add"
        '
        'BTT_DELETE
        '
        Me.BTT_DELETE.AutoToolTip = False
        Me.BTT_DELETE.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.BTT_DELETE.ForeColor = System.Drawing.Color.Maroon
        Me.BTT_DELETE.Image = CType(resources.GetObject("BTT_DELETE.Image"), System.Drawing.Image)
        Me.BTT_DELETE.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_DELETE.Name = "BTT_DELETE"
        Me.BTT_DELETE.Size = New System.Drawing.Size(60, 22)
        Me.BTT_DELETE.Text = "Delete"
        '
        'SettingsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 291)
        Me.Controls.Add(CONTAINER_MAIN)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(400, 330)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(400, 330)
        Me.Name = "SettingsForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Settings"
        CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
        CONTAINER_MAIN.TopToolStripPanel.ResumeLayout(False)
        CONTAINER_MAIN.TopToolStripPanel.PerformLayout()
        CONTAINER_MAIN.ResumeLayout(False)
        CONTAINER_MAIN.PerformLayout()
        Me.ToolbarTOP.ResumeLayout(False)
        Me.ToolbarTOP.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Private WithEvents LIST_DOMAINS As Windows.Forms.ListBox
    Private WithEvents ToolbarTOP As Windows.Forms.ToolStrip
    Private WithEvents BTT_ADD As Windows.Forms.ToolStripButton
    Private WithEvents BTT_DELETE As Windows.Forms.ToolStripButton
End Class