' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace Editors
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class SiteSelectionForm : Inherits System.Windows.Forms.Form
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SiteSelectionForm))
            Dim ListColumn1 As PersonalUtilities.Forms.Controls.Base.ListColumn = New PersonalUtilities.Forms.Controls.Base.ListColumn()
            Me.CMB_SITES = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            CType(Me.CMB_SITES, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(Me.CMB_SITES)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(284, 276)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(284, 276)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'CMB_SITES
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Name = "ArrowDown"
            ActionButton1.Visible = False
            Me.CMB_SITES.Buttons.Add(ActionButton1)
            ListColumn1.DisplayMember = True
            ListColumn1.Name = "COL_DISPLAY"
            ListColumn1.Text = "Site"
            ListColumn1.ValueMember = True
            ListColumn1.Width = -1
            Me.CMB_SITES.Columns.Add(ListColumn1)
            Me.CMB_SITES.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_SITES.Lines = New String(-1) {}
            Me.CMB_SITES.ListCheckBoxes = True
            Me.CMB_SITES.ListDropDownStyle = PersonalUtilities.Forms.Controls.ComboBoxExtended.ListMode.Simple
            Me.CMB_SITES.ListGridVisible = True
            Me.CMB_SITES.ListMultiSelect = True
            Me.CMB_SITES.Location = New System.Drawing.Point(0, 0)
            Me.CMB_SITES.Name = "CMB_SITES"
            Me.CMB_SITES.Size = New System.Drawing.Size(286, 277)
            Me.CMB_SITES.TabIndex = 0
            '
            'SiteSelectionForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(284, 276)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(300, 315)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(300, 315)
            Me.Name = "SiteSelectionForm"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Sites"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            CType(Me.CMB_SITES, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents CMB_SITES As PersonalUtilities.Forms.Controls.ComboBoxExtended
    End Class
End Namespace