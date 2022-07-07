' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace DownloadObjects
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class SchedulerEditorForm : Inherits System.Windows.Forms.Form
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
            Me.CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            Me.LIST_PLANS = New System.Windows.Forms.ListBox()
            Me.CONTAINER_MAIN.ContentPanel.SuspendLayout()
            Me.CONTAINER_MAIN.SuspendLayout()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            Me.CONTAINER_MAIN.BottomToolStripPanelVisible = False
            '
            'CONTAINER_MAIN.ContentPanel
            '
            Me.CONTAINER_MAIN.ContentPanel.Controls.Add(Me.LIST_PLANS)
            Me.CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(414, 316)
            Me.CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CONTAINER_MAIN.LeftToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            Me.CONTAINER_MAIN.RightToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Size = New System.Drawing.Size(414, 341)
            Me.CONTAINER_MAIN.TabIndex = 0
            '
            'LIST_PLANS
            '
            Me.LIST_PLANS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LIST_PLANS.FormattingEnabled = True
            Me.LIST_PLANS.Location = New System.Drawing.Point(0, 0)
            Me.LIST_PLANS.Name = "LIST_PLANS"
            Me.LIST_PLANS.Size = New System.Drawing.Size(414, 316)
            Me.LIST_PLANS.TabIndex = 0
            '
            'SchedulerEditorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(414, 341)
            Me.Controls.Add(Me.CONTAINER_MAIN)
            Me.KeyPreview = True
            Me.MinimumSize = New System.Drawing.Size(430, 380)
            Me.Name = "SchedulerEditorForm"
            Me.ShowIcon = False
            Me.Text = "Scheduler"
            Me.CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            Me.CONTAINER_MAIN.ResumeLayout(False)
            Me.CONTAINER_MAIN.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents CONTAINER_MAIN As ToolStripContainer
        Private WithEvents LIST_PLANS As ListBox
    End Class
End Namespace