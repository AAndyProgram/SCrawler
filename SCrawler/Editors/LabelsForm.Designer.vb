' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Friend Class LabelsForm : Inherits System.Windows.Forms.Form
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LabelsForm))
        Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
        Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
        Me.CMB_LABELS = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
        CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
        CONTAINER_MAIN.ContentPanel.SuspendLayout()
        CONTAINER_MAIN.SuspendLayout()
        CType(Me.CMB_LABELS, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CONTAINER_MAIN
        '
        '
        'CONTAINER_MAIN.ContentPanel
        '
        CONTAINER_MAIN.ContentPanel.Controls.Add(Me.CMB_LABELS)
        CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(374, 421)
        CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
        CONTAINER_MAIN.LeftToolStripPanelVisible = False
        CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
        CONTAINER_MAIN.Name = "CONTAINER_MAIN"
        CONTAINER_MAIN.RightToolStripPanelVisible = False
        CONTAINER_MAIN.Size = New System.Drawing.Size(374, 421)
        CONTAINER_MAIN.TabIndex = 0
        CONTAINER_MAIN.TopToolStripPanelVisible = False
        '
        'CMB_LABELS
        '
        ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
        ActionButton1.Name = "Add"
        ActionButton1.ToolTipText = "Add new label (Insert)"
        ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
        ActionButton2.Name = "Clear"
        ActionButton2.ToolTipText = "Clear checked labels"
        ActionButton3.BackgroundImage = CType(resources.GetObject("ActionButton3.BackgroundImage"), System.Drawing.Image)
        ActionButton3.Name = "ArrowDown"
        ActionButton3.Visible = False
        Me.CMB_LABELS.Buttons.Add(ActionButton1)
        Me.CMB_LABELS.Buttons.Add(ActionButton2)
        Me.CMB_LABELS.Buttons.Add(ActionButton3)
        Me.CMB_LABELS.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CMB_LABELS.Lines = New String(-1) {}
        Me.CMB_LABELS.ListCheckBoxes = True
        Me.CMB_LABELS.ListDropDownStyle = PersonalUtilities.Forms.Controls.ComboBoxExtended.ListMode.Simple
        Me.CMB_LABELS.ListGridVisible = True
        Me.CMB_LABELS.ListMultiSelect = True
        Me.CMB_LABELS.Location = New System.Drawing.Point(0, 0)
        Me.CMB_LABELS.Name = "CMB_LABELS"
        Me.CMB_LABELS.Size = New System.Drawing.Size(376, 422)
        Me.CMB_LABELS.TabIndex = 0
        '
        'LabelsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(374, 421)
        Me.Controls.Add(CONTAINER_MAIN)
        Me.Icon = Global.SCrawler.My.Resources.Resources.TagIcon_32
        Me.KeyPreview = True
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(390, 460)
        Me.Name = "LabelsForm"
        Me.ShowInTaskbar = False
        Me.Text = "Labels"
        CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
        CONTAINER_MAIN.ResumeLayout(False)
        CONTAINER_MAIN.PerformLayout()
        CType(Me.CMB_LABELS, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents CMB_LABELS As PersonalUtilities.Forms.Controls.ComboBoxExtended
End Class