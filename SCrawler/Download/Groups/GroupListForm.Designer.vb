' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace DownloadObjects.Groups
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class GroupListForm : Inherits System.Windows.Forms.Form
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
            Me.LIST_GROUPS = New System.Windows.Forms.ListBox()
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
            Me.CONTAINER_MAIN.ContentPanel.Controls.Add(Me.LIST_GROUPS)
            Me.CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(284, 236)
            Me.CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CONTAINER_MAIN.LeftToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            Me.CONTAINER_MAIN.RightToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Size = New System.Drawing.Size(284, 261)
            Me.CONTAINER_MAIN.TabIndex = 0
            '
            'LIST_GROUPS
            '
            Me.LIST_GROUPS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LIST_GROUPS.FormattingEnabled = True
            Me.LIST_GROUPS.Location = New System.Drawing.Point(0, 0)
            Me.LIST_GROUPS.Name = "LIST_GROUPS"
            Me.LIST_GROUPS.Size = New System.Drawing.Size(284, 236)
            Me.LIST_GROUPS.TabIndex = 0
            '
            'GroupListForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(284, 261)
            Me.Controls.Add(Me.CONTAINER_MAIN)
            Me.Icon = Global.SCrawler.My.Resources.Resources.GroupByIcon_16
            Me.KeyPreview = True
            Me.MinimumSize = New System.Drawing.Size(300, 300)
            Me.Name = "GroupListForm"
            Me.ShowInTaskbar = False
            Me.Text = "Groups"
            Me.CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            Me.CONTAINER_MAIN.ResumeLayout(False)
            Me.CONTAINER_MAIN.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents LIST_GROUPS As ListBox
        Private WithEvents CONTAINER_MAIN As ToolStripContainer
    End Class
End Namespace