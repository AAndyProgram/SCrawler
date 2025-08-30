' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.YouTube.Controls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class VideoOptionsTrimForm : Inherits System.Windows.Forms.Form
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
            Me.components = New System.ComponentModel.Container()
            Dim CONTAINER_MAIN As System.Windows.Forms.ToolStripContainer
            Dim TP_MAIN As System.Windows.Forms.TableLayoutPanel
            Dim TP_OPTIONS As System.Windows.Forms.TableLayoutPanel
            Dim TT_MAIN As System.Windows.Forms.ToolTip
            Me.LIST_TRIM = New System.Windows.Forms.ListBox()
            Me.CH_DEL_ORIG = New System.Windows.Forms.CheckBox()
            Me.CH_ADD_M3U8 = New System.Windows.Forms.CheckBox()
            Me.CH_SEP_FOLDER = New System.Windows.Forms.CheckBox()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            TP_OPTIONS = New System.Windows.Forms.TableLayoutPanel()
            TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            TP_MAIN.SuspendLayout()
            TP_OPTIONS.SuspendLayout()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(434, 236)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(434, 261)
            CONTAINER_MAIN.TabIndex = 0
            '
            'TP_MAIN
            '
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(Me.LIST_TRIM, 0, 1)
            TP_MAIN.Controls.Add(TP_OPTIONS, 0, 0)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 2
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Size = New System.Drawing.Size(434, 236)
            TP_MAIN.TabIndex = 0
            '
            'LIST_TRIM
            '
            Me.LIST_TRIM.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LIST_TRIM.FormattingEnabled = True
            Me.LIST_TRIM.Location = New System.Drawing.Point(3, 33)
            Me.LIST_TRIM.Name = "LIST_TRIM"
            Me.LIST_TRIM.Size = New System.Drawing.Size(428, 200)
            Me.LIST_TRIM.TabIndex = 0
            '
            'TP_OPTIONS
            '
            TP_OPTIONS.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_OPTIONS.ColumnCount = 3
            TP_OPTIONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_OPTIONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334!))
            TP_OPTIONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334!))
            TP_OPTIONS.Controls.Add(Me.CH_DEL_ORIG, 0, 0)
            TP_OPTIONS.Controls.Add(Me.CH_ADD_M3U8, 1, 0)
            TP_OPTIONS.Controls.Add(Me.CH_SEP_FOLDER, 2, 0)
            TP_OPTIONS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_OPTIONS.Location = New System.Drawing.Point(3, 3)
            TP_OPTIONS.Name = "TP_OPTIONS"
            TP_OPTIONS.RowCount = 1
            TP_OPTIONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_OPTIONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 23.0!))
            TP_OPTIONS.Size = New System.Drawing.Size(428, 24)
            TP_OPTIONS.TabIndex = 1
            '
            'CH_DEL_ORIG
            '
            Me.CH_DEL_ORIG.AutoSize = True
            Me.CH_DEL_ORIG.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DEL_ORIG.Location = New System.Drawing.Point(4, 4)
            Me.CH_DEL_ORIG.Name = "CH_DEL_ORIG"
            Me.CH_DEL_ORIG.Size = New System.Drawing.Size(135, 16)
            Me.CH_DEL_ORIG.TabIndex = 0
            Me.CH_DEL_ORIG.Text = "Delete original file"
            TT_MAIN.SetToolTip(Me.CH_DEL_ORIG, "If checked, the original file will be deleted after trimming")
            Me.CH_DEL_ORIG.UseVisualStyleBackColor = True
            '
            'CH_ADD_M3U8
            '
            Me.CH_ADD_M3U8.AutoSize = True
            Me.CH_ADD_M3U8.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_ADD_M3U8.Location = New System.Drawing.Point(146, 4)
            Me.CH_ADD_M3U8.Name = "CH_ADD_M3U8"
            Me.CH_ADD_M3U8.Size = New System.Drawing.Size(135, 16)
            Me.CH_ADD_M3U8.TabIndex = 1
            Me.CH_ADD_M3U8.Text = "Add to M3U8"
            TT_MAIN.SetToolTip(Me.CH_ADD_M3U8, "If checked, the trimmed files will be added to the M3U8 playlist (if selected)")
            Me.CH_ADD_M3U8.UseVisualStyleBackColor = True
            '
            'CH_SEP_FOLDER
            '
            Me.CH_SEP_FOLDER.AutoSize = True
            Me.CH_SEP_FOLDER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_SEP_FOLDER.Location = New System.Drawing.Point(288, 4)
            Me.CH_SEP_FOLDER.Name = "CH_SEP_FOLDER"
            Me.CH_SEP_FOLDER.Size = New System.Drawing.Size(136, 16)
            Me.CH_SEP_FOLDER.TabIndex = 2
            Me.CH_SEP_FOLDER.Text = "Separate folder"
            TT_MAIN.SetToolTip(Me.CH_SEP_FOLDER, "Place the trimmed files in a separate folder")
            Me.CH_SEP_FOLDER.UseVisualStyleBackColor = True
            '
            'VideoOptionsTrimForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(434, 261)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.KeyPreview = True
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(450, 300)
            Me.Name = "VideoOptionsTrimForm"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.Text = "Trimming options"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            TP_OPTIONS.ResumeLayout(False)
            TP_OPTIONS.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents LIST_TRIM As ListBox
        Private WithEvents CH_DEL_ORIG As CheckBox
        Private WithEvents CH_ADD_M3U8 As CheckBox
        Private WithEvents CH_SEP_FOLDER As CheckBox
    End Class
End Namespace