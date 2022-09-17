' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Friend Class FDatePickerForm : Inherits System.Windows.Forms.Form
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
        Me.DT_FROM = New PersonalUtilities.Forms.Controls.TextBoxExtended()
        Me.DT_TO = New PersonalUtilities.Forms.Controls.TextBoxExtended()
        CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
        TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
        CONTAINER_MAIN.ContentPanel.SuspendLayout()
        CONTAINER_MAIN.SuspendLayout()
        TP_MAIN.SuspendLayout()
        CType(Me.DT_FROM, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.DT_TO, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CONTAINER_MAIN
        '
        '
        'CONTAINER_MAIN.ContentPanel
        '
        CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
        CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(395, 28)
        CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
        CONTAINER_MAIN.LeftToolStripPanelVisible = False
        CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
        CONTAINER_MAIN.Name = "CONTAINER_MAIN"
        CONTAINER_MAIN.RightToolStripPanelVisible = False
        CONTAINER_MAIN.Size = New System.Drawing.Size(395, 53)
        CONTAINER_MAIN.TabIndex = 0
        CONTAINER_MAIN.TopToolStripPanelVisible = False
        '
        'TP_MAIN
        '
        TP_MAIN.ColumnCount = 2
        TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        TP_MAIN.Controls.Add(Me.DT_FROM, 0, 0)
        TP_MAIN.Controls.Add(Me.DT_TO, 1, 0)
        TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
        TP_MAIN.Location = New System.Drawing.Point(0, 0)
        TP_MAIN.Name = "TP_MAIN"
        TP_MAIN.RowCount = 1
        TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 53.0!))
        TP_MAIN.Size = New System.Drawing.Size(395, 28)
        TP_MAIN.TabIndex = 1
        '
        'DT_FROM
        '
        Me.DT_FROM.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
        Me.DT_FROM.CaptionText = "From"
        Me.DT_FROM.CaptionWidth = 50.0R
        Me.DT_FROM.ControlMode = PersonalUtilities.Forms.Controls.TextBoxExtended.ControlModes.DateTimePicker
        Me.DT_FROM.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DT_FROM.Location = New System.Drawing.Point(3, 3)
        Me.DT_FROM.Name = "DT_FROM"
        Me.DT_FROM.Size = New System.Drawing.Size(191, 22)
        Me.DT_FROM.TabIndex = 0
        Me.DT_FROM.Text = "17.09.2022 2:13:36"
        Me.DT_FROM.TextBoxWidthMinimal = 50
        '
        'DT_TO
        '
        Me.DT_TO.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
        Me.DT_TO.CaptionText = "To"
        Me.DT_TO.CaptionWidth = 50.0R
        Me.DT_TO.ControlMode = PersonalUtilities.Forms.Controls.TextBoxExtended.ControlModes.DateTimePicker
        Me.DT_TO.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DT_TO.Location = New System.Drawing.Point(200, 3)
        Me.DT_TO.Name = "DT_TO"
        Me.DT_TO.Size = New System.Drawing.Size(192, 22)
        Me.DT_TO.TabIndex = 1
        Me.DT_TO.Text = "17.09.2022 2:13:40"
        Me.DT_TO.TextBoxWidthMinimal = 50
        '
        'FDatePickerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(395, 53)
        Me.Controls.Add(CONTAINER_MAIN)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(411, 92)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(411, 92)
        Me.Name = "FDatePickerForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Date limit"
        CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
        CONTAINER_MAIN.ResumeLayout(False)
        CONTAINER_MAIN.PerformLayout()
        TP_MAIN.ResumeLayout(False)
        CType(Me.DT_FROM, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.DT_TO, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Private WithEvents DT_FROM As PersonalUtilities.Forms.Controls.TextBoxExtended
    Private WithEvents DT_TO As PersonalUtilities.Forms.Controls.TextBoxExtended
End Class