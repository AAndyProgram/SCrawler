' Copyright (C) 2023  Andy https://github.com/AAndyProgram
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
            Dim CONTAINER_MAIN As System.Windows.Forms.ToolStripContainer
            Me.LIST_PLANS = New System.Windows.Forms.ListView()
            Me.COL_MAIN = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            CONTAINER_MAIN.BottomToolStripPanelVisible = False
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(Me.LIST_PLANS)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(414, 316)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(414, 341)
            CONTAINER_MAIN.TabIndex = 0
            '
            'LIST_PLANS
            '
            Me.LIST_PLANS.Alignment = System.Windows.Forms.ListViewAlignment.Left
            Me.LIST_PLANS.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.COL_MAIN})
            Me.LIST_PLANS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LIST_PLANS.FullRowSelect = True
            Me.LIST_PLANS.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
            Me.LIST_PLANS.HideSelection = False
            Me.LIST_PLANS.Location = New System.Drawing.Point(0, 0)
            Me.LIST_PLANS.MultiSelect = False
            Me.LIST_PLANS.Name = "LIST_PLANS"
            Me.LIST_PLANS.ShowGroups = False
            Me.LIST_PLANS.Size = New System.Drawing.Size(414, 316)
            Me.LIST_PLANS.TabIndex = 0
            Me.LIST_PLANS.UseCompatibleStateImageBehavior = False
            Me.LIST_PLANS.View = System.Windows.Forms.View.Details
            '
            'COL_MAIN
            '
            Me.COL_MAIN.Text = "Task"
            Me.COL_MAIN.Width = 410
            '
            'SchedulerEditorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(414, 341)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.KeyPreview = True
            Me.MinimumSize = New System.Drawing.Size(430, 380)
            Me.Name = "SchedulerEditorForm"
            Me.ShowInTaskbar = False
            Me.Text = "Scheduler"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents LIST_PLANS As ListView
        Private WithEvents COL_MAIN As ColumnHeader
    End Class
End Namespace