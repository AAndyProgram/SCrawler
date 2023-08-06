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
    Partial Friend Class UserDownloadQueueForm : Inherits System.Windows.Forms.Form
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
            Dim TOOLBAR_BOTTOM As System.Windows.Forms.StatusStrip
            Me.LIST_QUEUE = New System.Windows.Forms.ListBox()
            TOOLBAR_BOTTOM = New System.Windows.Forms.StatusStrip()
            Me.SuspendLayout()
            '
            'TOOLBAR_BOTTOM
            '
            TOOLBAR_BOTTOM.Location = New System.Drawing.Point(0, 189)
            TOOLBAR_BOTTOM.Name = "TOOLBAR_BOTTOM"
            TOOLBAR_BOTTOM.Size = New System.Drawing.Size(284, 22)
            TOOLBAR_BOTTOM.TabIndex = 1
            TOOLBAR_BOTTOM.Text = "StatusStrip1"
            '
            'LIST_QUEUE
            '
            Me.LIST_QUEUE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LIST_QUEUE.FormattingEnabled = True
            Me.LIST_QUEUE.Location = New System.Drawing.Point(0, 0)
            Me.LIST_QUEUE.Name = "LIST_QUEUE"
            Me.LIST_QUEUE.Size = New System.Drawing.Size(284, 189)
            Me.LIST_QUEUE.TabIndex = 0
            '
            'UserDownloadQueueForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(284, 211)
            Me.Controls.Add(Me.LIST_QUEUE)
            Me.Controls.Add(TOOLBAR_BOTTOM)
            Me.Icon = Global.SCrawler.My.Resources.Resources.ArrowDownIcon_Blue_24
            Me.KeyPreview = True
            Me.MinimumSize = New System.Drawing.Size(300, 250)
            Me.Name = "UserDownloadQueueForm"
            Me.Text = "User download queue"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Private WithEvents LIST_QUEUE As ListBox
    End Class
End Namespace