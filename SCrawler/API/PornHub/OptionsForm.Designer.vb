' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.PornHub
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class OptionsForm : Inherits System.Windows.Forms.Form
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
            Me.CH_DOWN_GIFS = New System.Windows.Forms.CheckBox()
            Me.CH_DOWN_PHOTO_MODELHUB = New System.Windows.Forms.CheckBox()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
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
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(278, 52)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(278, 77)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            TP_MAIN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(Me.CH_DOWN_GIFS, 0, 0)
            TP_MAIN.Controls.Add(Me.CH_DOWN_PHOTO_MODELHUB, 0, 1)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 3
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Size = New System.Drawing.Size(278, 52)
            TP_MAIN.TabIndex = 0
            '
            'CH_DOWN_GIFS
            '
            Me.CH_DOWN_GIFS.AutoSize = True
            Me.CH_DOWN_GIFS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DOWN_GIFS.Location = New System.Drawing.Point(4, 4)
            Me.CH_DOWN_GIFS.Name = "CH_DOWN_GIFS"
            Me.CH_DOWN_GIFS.Size = New System.Drawing.Size(270, 19)
            Me.CH_DOWN_GIFS.TabIndex = 0
            Me.CH_DOWN_GIFS.Text = "Download gifs"
            Me.CH_DOWN_GIFS.UseVisualStyleBackColor = True
            '
            'CH_DOWN_PHOTO_MODELHUB
            '
            Me.CH_DOWN_PHOTO_MODELHUB.AutoSize = True
            Me.CH_DOWN_PHOTO_MODELHUB.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DOWN_PHOTO_MODELHUB.Location = New System.Drawing.Point(4, 30)
            Me.CH_DOWN_PHOTO_MODELHUB.Name = "CH_DOWN_PHOTO_MODELHUB"
            Me.CH_DOWN_PHOTO_MODELHUB.Size = New System.Drawing.Size(270, 19)
            Me.CH_DOWN_PHOTO_MODELHUB.TabIndex = 1
            Me.CH_DOWN_PHOTO_MODELHUB.Text = "Download photo only from ModelHub"
            Me.CH_DOWN_PHOTO_MODELHUB.UseVisualStyleBackColor = True
            '
            'OptionsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(278, 77)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Icon = Global.SCrawler.My.Resources.SiteResources.InstagramIcon_32
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(294, 116)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(294, 116)
            Me.Name = "OptionsForm"
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Options"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            TP_MAIN.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents CH_DOWN_GIFS As CheckBox
        Private WithEvents CH_DOWN_PHOTO_MODELHUB As CheckBox
    End Class
End Namespace