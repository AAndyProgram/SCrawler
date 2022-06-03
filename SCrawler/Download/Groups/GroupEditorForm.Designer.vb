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
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GroupEditorForm))
            Me.DEFS_GROUP = New SCrawler.DownloadObjects.Groups.GroupDefaults()
            Me.TXT_NAME = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            Me.DEFS_GROUP.SuspendLayout()
            CType(Me.TXT_NAME, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(Me.DEFS_GROUP)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(476, 134)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(476, 134)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'DEFS_GROUP
            '
            Me.DEFS_GROUP.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            Me.DEFS_GROUP.ColumnCount = 1
            Me.DEFS_GROUP.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.DEFS_GROUP.Controls.Add(Me.TXT_NAME, 0, 0)
            Me.DEFS_GROUP.Dock = System.Windows.Forms.DockStyle.Fill
            Me.DEFS_GROUP.Location = New System.Drawing.Point(0, 0)
            Me.DEFS_GROUP.Name = "DEFS_GROUP"
            Me.DEFS_GROUP.RowCount = 5
            Me.DEFS_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEFS_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEFS_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.DEFS_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.DEFS_GROUP.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.DEFS_GROUP.Size = New System.Drawing.Size(476, 134)
            Me.DEFS_GROUP.TabIndex = 1
            '
            'TXT_NAME
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Index = 0
            ActionButton1.Name = "BTT_CLEAR"
            Me.TXT_NAME.Buttons.Add(ActionButton1)
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
            'GroupEditorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(476, 134)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(492, 173)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(492, 173)
            Me.Name = "GroupEditorForm"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Group"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            Me.DEFS_GROUP.ResumeLayout(False)
            CType(Me.TXT_NAME, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents TXT_NAME As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents DEFS_GROUP As GroupDefaults
    End Class
End Namespace