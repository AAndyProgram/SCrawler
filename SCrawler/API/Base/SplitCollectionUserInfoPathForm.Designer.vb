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
    Partial Friend Class SplitCollectionUserInfoPathForm : Inherits System.Windows.Forms.Form
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SplitCollectionUserInfoPathForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Me.TXT_PATH_CURR = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_PATH_NEW = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            TP_MAIN.SuspendLayout()
            CType(Me.TXT_PATH_CURR, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_PATH_NEW, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(484, 84)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(484, 84)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            TP_MAIN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_MAIN.Controls.Add(Me.TXT_PATH_CURR, 0, 0)
            TP_MAIN.Controls.Add(Me.TXT_PATH_NEW, 0, 1)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 3
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Size = New System.Drawing.Size(484, 84)
            TP_MAIN.TabIndex = 0
            '
            'TXT_PATH_CURR
            '
            Me.TXT_PATH_CURR.CaptionText = "Current"
            Me.TXT_PATH_CURR.CaptionWidth = 50.0R
            Me.TXT_PATH_CURR.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_PATH_CURR.Location = New System.Drawing.Point(4, 4)
            Me.TXT_PATH_CURR.Name = "TXT_PATH_CURR"
            Me.TXT_PATH_CURR.Size = New System.Drawing.Size(476, 22)
            Me.TXT_PATH_CURR.TabIndex = 0
            Me.TXT_PATH_CURR.TextBoxReadOnly = True
            '
            'TXT_PATH_NEW
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Name = "Refresh"
            ActionButton1.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Refresh
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Name = "Open"
            ActionButton2.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Open
            Me.TXT_PATH_NEW.Buttons.Add(ActionButton1)
            Me.TXT_PATH_NEW.Buttons.Add(ActionButton2)
            Me.TXT_PATH_NEW.CaptionText = "New"
            Me.TXT_PATH_NEW.CaptionWidth = 50.0R
            Me.TXT_PATH_NEW.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_PATH_NEW.Location = New System.Drawing.Point(4, 33)
            Me.TXT_PATH_NEW.Name = "TXT_PATH_NEW"
            Me.TXT_PATH_NEW.Size = New System.Drawing.Size(476, 22)
            Me.TXT_PATH_NEW.TabIndex = 1
            '
            'SplitCollectionUserInfoPathForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(484, 84)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Icon = Global.SCrawler.My.Resources.Resources.UsersIcon_32
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(500, 123)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(500, 123)
            Me.Name = "SplitCollectionUserInfoPathForm"
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "User paths"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            CType(Me.TXT_PATH_CURR, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_PATH_NEW, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents TXT_PATH_CURR As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_PATH_NEW As PersonalUtilities.Forms.Controls.TextBoxExtended
    End Class
End Namespace