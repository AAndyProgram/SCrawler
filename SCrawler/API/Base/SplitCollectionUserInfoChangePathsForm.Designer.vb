' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.Base
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class SplitCollectionUserInfoChangePathsForm : Inherits System.Windows.Forms.Form
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
            Dim LBL_INFO As System.Windows.Forms.Label
            Me.LIST_USERS = New System.Windows.Forms.ListBox()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            LBL_INFO = New System.Windows.Forms.Label()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            TP_MAIN.SuspendLayout()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(384, 261)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(384, 261)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_MAIN.Controls.Add(LBL_INFO, 0, 0)
            TP_MAIN.Controls.Add(Me.LIST_USERS, 0, 1)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 2
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Size = New System.Drawing.Size(384, 261)
            TP_MAIN.TabIndex = 0
            '
            'LBL_INFO
            '
            LBL_INFO.Dock = System.Windows.Forms.DockStyle.Fill
            LBL_INFO.Location = New System.Drawing.Point(3, 0)
            LBL_INFO.Name = "LBL_INFO"
            LBL_INFO.Size = New System.Drawing.Size(378, 50)
            LBL_INFO.TabIndex = 0
            LBL_INFO.Text = "Check the user destination paths and change them if necessary." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Double-click to c" &
    "hange."
            LBL_INFO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'LIST_USERS
            '
            Me.LIST_USERS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LIST_USERS.FormattingEnabled = True
            Me.LIST_USERS.Location = New System.Drawing.Point(3, 53)
            Me.LIST_USERS.Name = "LIST_USERS"
            Me.LIST_USERS.Size = New System.Drawing.Size(378, 205)
            Me.LIST_USERS.TabIndex = 1
            '
            'SplitCollectionUserInfoChangePathsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(384, 261)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.Icon = Global.SCrawler.My.Resources.Resources.UsersIcon_32
            Me.KeyPreview = True
            Me.MinimumSize = New System.Drawing.Size(400, 300)
            Me.Name = "SplitCollectionUserInfoChangePathsForm"
            Me.ShowInTaskbar = False
            Me.Text = "Collection users"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents LIST_USERS As ListBox
    End Class
End Namespace