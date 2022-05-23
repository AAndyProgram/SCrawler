' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace DownloadObjects.Groups
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class GroupEditorForm : Inherits System.Windows.Forms.Form
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
            Dim ActionButton7 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GroupEditorForm))
            Dim ActionButton8 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton9 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim TP_TEMP_FAV As System.Windows.Forms.TableLayoutPanel
            Dim TP_READY_FOR_DOWN As System.Windows.Forms.TableLayoutPanel
            Me.TXT_NAME = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_LABELS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_TEMPORARY = New System.Windows.Forms.CheckBox()
            Me.CH_FAV = New System.Windows.Forms.CheckBox()
            Me.CH_READY_FOR_DOWN = New System.Windows.Forms.CheckBox()
            Me.CH_READY_FOR_DOWN_IGNORE = New System.Windows.Forms.CheckBox()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            TP_TEMP_FAV = New System.Windows.Forms.TableLayoutPanel()
            TP_READY_FOR_DOWN = New System.Windows.Forms.TableLayoutPanel()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            TP_MAIN.SuspendLayout()
            CType(Me.TXT_NAME, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_LABELS, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_TEMP_FAV.SuspendLayout()
            TP_READY_FOR_DOWN.SuspendLayout()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(476, 111)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(476, 136)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            TP_MAIN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(TP_READY_FOR_DOWN, 0, 2)
            TP_MAIN.Controls.Add(Me.TXT_NAME, 0, 0)
            TP_MAIN.Controls.Add(Me.TXT_LABELS, 0, 3)
            TP_MAIN.Controls.Add(TP_TEMP_FAV, 0, 1)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 5
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_MAIN.Size = New System.Drawing.Size(476, 111)
            TP_MAIN.TabIndex = 0
            '
            'TXT_NAME
            '
            ActionButton7.BackgroundImage = CType(resources.GetObject("ActionButton7.BackgroundImage"), System.Drawing.Image)
            ActionButton7.Index = 0
            ActionButton7.Name = "BTT_CLEAR"
            Me.TXT_NAME.Buttons.Add(ActionButton7)
            Me.TXT_NAME.CaptionText = "Name"
            Me.TXT_NAME.CaptionToolTipEnabled = True
            Me.TXT_NAME.CaptionToolTipText = "Group name"
            Me.TXT_NAME.CaptionWidth = 50.0R
            Me.TXT_NAME.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_NAME.Location = New System.Drawing.Point(4, 4)
            Me.TXT_NAME.Name = "TXT_NAME"
            Me.TXT_NAME.Size = New System.Drawing.Size(468, 22)
            Me.TXT_NAME.TabIndex = 0
            '
            'TXT_LABELS
            '
            ActionButton8.BackgroundImage = CType(resources.GetObject("ActionButton8.BackgroundImage"), System.Drawing.Image)
            ActionButton8.Index = 0
            ActionButton8.Name = "BTT_EDIT"
            ActionButton9.BackgroundImage = CType(resources.GetObject("ActionButton9.BackgroundImage"), System.Drawing.Image)
            ActionButton9.Index = 1
            ActionButton9.Name = "BTT_CLEAR"
            Me.TXT_LABELS.Buttons.Add(ActionButton8)
            Me.TXT_LABELS.Buttons.Add(ActionButton9)
            Me.TXT_LABELS.CaptionText = "Labels"
            Me.TXT_LABELS.CaptionToolTipEnabled = True
            Me.TXT_LABELS.CaptionToolTipText = "Group labels"
            Me.TXT_LABELS.CaptionWidth = 50.0R
            Me.TXT_LABELS.ClearTextByButtonClear = False
            Me.TXT_LABELS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_LABELS.Location = New System.Drawing.Point(4, 85)
            Me.TXT_LABELS.Name = "TXT_LABELS"
            Me.TXT_LABELS.Size = New System.Drawing.Size(468, 22)
            Me.TXT_LABELS.TabIndex = 1
            '
            'TP_TEMP_FAV
            '
            TP_TEMP_FAV.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_TEMP_FAV.ColumnCount = 2
            TP_TEMP_FAV.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_TEMP_FAV.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_TEMP_FAV.Controls.Add(Me.CH_TEMPORARY, 0, 0)
            TP_TEMP_FAV.Controls.Add(Me.CH_FAV, 1, 0)
            TP_TEMP_FAV.Dock = System.Windows.Forms.DockStyle.Fill
            TP_TEMP_FAV.Location = New System.Drawing.Point(1, 30)
            TP_TEMP_FAV.Margin = New System.Windows.Forms.Padding(0)
            TP_TEMP_FAV.Name = "TP_TEMP_FAV"
            TP_TEMP_FAV.RowCount = 1
            TP_TEMP_FAV.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_TEMP_FAV.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_TEMP_FAV.Size = New System.Drawing.Size(474, 25)
            TP_TEMP_FAV.TabIndex = 2
            '
            'TP_READY_FOR_DOWN
            '
            TP_READY_FOR_DOWN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_READY_FOR_DOWN.ColumnCount = 2
            TP_READY_FOR_DOWN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_READY_FOR_DOWN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_READY_FOR_DOWN.Controls.Add(Me.CH_READY_FOR_DOWN, 0, 0)
            TP_READY_FOR_DOWN.Controls.Add(Me.CH_READY_FOR_DOWN_IGNORE, 1, 0)
            TP_READY_FOR_DOWN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_READY_FOR_DOWN.Location = New System.Drawing.Point(1, 56)
            TP_READY_FOR_DOWN.Margin = New System.Windows.Forms.Padding(0)
            TP_READY_FOR_DOWN.Name = "TP_READY_FOR_DOWN"
            TP_READY_FOR_DOWN.RowCount = 1
            TP_READY_FOR_DOWN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_READY_FOR_DOWN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_READY_FOR_DOWN.Size = New System.Drawing.Size(474, 25)
            TP_READY_FOR_DOWN.TabIndex = 3
            '
            'CH_TEMPORARY
            '
            Me.CH_TEMPORARY.AutoSize = True
            Me.CH_TEMPORARY.Checked = True
            Me.CH_TEMPORARY.CheckState = System.Windows.Forms.CheckState.Indeterminate
            Me.CH_TEMPORARY.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_TEMPORARY.Location = New System.Drawing.Point(4, 4)
            Me.CH_TEMPORARY.Name = "CH_TEMPORARY"
            Me.CH_TEMPORARY.Size = New System.Drawing.Size(229, 17)
            Me.CH_TEMPORARY.TabIndex = 0
            Me.CH_TEMPORARY.Text = "Temporary"
            Me.CH_TEMPORARY.ThreeState = True
            Me.CH_TEMPORARY.UseVisualStyleBackColor = True
            '
            'CH_FAV
            '
            Me.CH_FAV.AutoSize = True
            Me.CH_FAV.Checked = True
            Me.CH_FAV.CheckState = System.Windows.Forms.CheckState.Indeterminate
            Me.CH_FAV.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_FAV.Location = New System.Drawing.Point(240, 4)
            Me.CH_FAV.Name = "CH_FAV"
            Me.CH_FAV.Size = New System.Drawing.Size(230, 17)
            Me.CH_FAV.TabIndex = 1
            Me.CH_FAV.Text = "Favorite"
            Me.CH_FAV.ThreeState = True
            Me.CH_FAV.UseVisualStyleBackColor = True
            '
            'CH_READY_FOR_DOWN
            '
            Me.CH_READY_FOR_DOWN.AutoSize = True
            Me.CH_READY_FOR_DOWN.Checked = True
            Me.CH_READY_FOR_DOWN.CheckState = System.Windows.Forms.CheckState.Checked
            Me.CH_READY_FOR_DOWN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_READY_FOR_DOWN.Location = New System.Drawing.Point(4, 4)
            Me.CH_READY_FOR_DOWN.Name = "CH_READY_FOR_DOWN"
            Me.CH_READY_FOR_DOWN.Size = New System.Drawing.Size(229, 17)
            Me.CH_READY_FOR_DOWN.TabIndex = 0
            Me.CH_READY_FOR_DOWN.Text = "Ready for download"
            Me.CH_READY_FOR_DOWN.UseVisualStyleBackColor = True
            '
            'CH_READY_FOR_DOWN_IGNORE
            '
            Me.CH_READY_FOR_DOWN_IGNORE.AutoSize = True
            Me.CH_READY_FOR_DOWN_IGNORE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_READY_FOR_DOWN_IGNORE.Location = New System.Drawing.Point(240, 4)
            Me.CH_READY_FOR_DOWN_IGNORE.Name = "CH_READY_FOR_DOWN_IGNORE"
            Me.CH_READY_FOR_DOWN_IGNORE.Size = New System.Drawing.Size(230, 17)
            Me.CH_READY_FOR_DOWN_IGNORE.TabIndex = 1
            Me.CH_READY_FOR_DOWN_IGNORE.Text = "Ignore ready for download"
            Me.CH_READY_FOR_DOWN_IGNORE.UseVisualStyleBackColor = True
            '
            'GroupEditorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(476, 136)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(492, 175)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(492, 175)
            Me.Name = "GroupEditorForm"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Group"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            CType(Me.TXT_NAME, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_LABELS, System.ComponentModel.ISupportInitialize).EndInit()
            TP_TEMP_FAV.ResumeLayout(False)
            TP_TEMP_FAV.PerformLayout()
            TP_READY_FOR_DOWN.ResumeLayout(False)
            TP_READY_FOR_DOWN.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents TXT_NAME As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_LABELS As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CH_READY_FOR_DOWN As CheckBox
        Private WithEvents CH_READY_FOR_DOWN_IGNORE As CheckBox
        Private WithEvents CH_TEMPORARY As CheckBox
        Private WithEvents CH_FAV As CheckBox
    End Class
End Namespace