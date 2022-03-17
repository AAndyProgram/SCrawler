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
        Me.DT = New System.Windows.Forms.DateTimePicker()
        CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
        CONTAINER_MAIN.ContentPanel.SuspendLayout()
        CONTAINER_MAIN.SuspendLayout()
        Me.SuspendLayout()
        '
        'CONTAINER_MAIN
        '
        '
        'CONTAINER_MAIN.ContentPanel
        '
        CONTAINER_MAIN.ContentPanel.Controls.Add(Me.DT)
        CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(209, 47)
        CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
        CONTAINER_MAIN.LeftToolStripPanelVisible = False
        CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
        CONTAINER_MAIN.Name = "CONTAINER_MAIN"
        CONTAINER_MAIN.RightToolStripPanelVisible = False
        CONTAINER_MAIN.Size = New System.Drawing.Size(209, 47)
        CONTAINER_MAIN.TabIndex = 0
        CONTAINER_MAIN.TopToolStripPanelVisible = False
        '
        'DT
        '
        Me.DT.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DT.Location = New System.Drawing.Point(0, 0)
        Me.DT.Name = "DT"
        Me.DT.ShowCheckBox = True
        Me.DT.Size = New System.Drawing.Size(209, 20)
        Me.DT.TabIndex = 0
        '
        'FDatePickerForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(209, 47)
        Me.Controls.Add(CONTAINER_MAIN)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(225, 86)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(225, 86)
        Me.Name = "FDatePickerForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Date limit"
        CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
        CONTAINER_MAIN.ResumeLayout(False)
        CONTAINER_MAIN.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents DT As DateTimePicker
End Class