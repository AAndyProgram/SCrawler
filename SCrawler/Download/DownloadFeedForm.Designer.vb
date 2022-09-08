' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace DownloadObjects
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class DownloadFeedForm : Inherits System.Windows.Forms.Form
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
            Dim SEP_1 As System.Windows.Forms.ToolStripSeparator
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DownloadFeedForm))
            Me.ToolbarTOP = New System.Windows.Forms.ToolStrip()
            Me.BTT_REFRESH = New System.Windows.Forms.ToolStripButton()
            Me.BTT_CLEAR = New System.Windows.Forms.ToolStripButton()
            Me.TP_DATA = New System.Windows.Forms.TableLayoutPanel()
            SEP_1 = New System.Windows.Forms.ToolStripSeparator()
            Me.ToolbarTOP.SuspendLayout()
            Me.SuspendLayout()
            '
            'SEP_1
            '
            SEP_1.Name = "SEP_1"
            SEP_1.Size = New System.Drawing.Size(6, 25)
            '
            'ToolbarTOP
            '
            Me.ToolbarTOP.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
            Me.ToolbarTOP.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_REFRESH, Me.BTT_CLEAR, SEP_1})
            Me.ToolbarTOP.Location = New System.Drawing.Point(0, 0)
            Me.ToolbarTOP.Name = "ToolbarTOP"
            Me.ToolbarTOP.Size = New System.Drawing.Size(484, 25)
            Me.ToolbarTOP.TabIndex = 0
            '
            'BTT_REFRESH
            '
            Me.BTT_REFRESH.Image = Global.SCrawler.My.Resources.Resources.Refresh
            Me.BTT_REFRESH.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_REFRESH.Name = "BTT_REFRESH"
            Me.BTT_REFRESH.Size = New System.Drawing.Size(66, 22)
            Me.BTT_REFRESH.Text = "Refresh"
            Me.BTT_REFRESH.ToolTipText = "Refresh data list"
            '
            'BTT_CLEAR
            '
            Me.BTT_CLEAR.Image = Global.SCrawler.My.Resources.Resources.Delete
            Me.BTT_CLEAR.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_CLEAR.Name = "BTT_CLEAR"
            Me.BTT_CLEAR.Size = New System.Drawing.Size(54, 22)
            Me.BTT_CLEAR.Text = "Clear"
            Me.BTT_CLEAR.ToolTipText = "Clear data list"
            '
            'TP_DATA
            '
            Me.TP_DATA.AutoScroll = True
            Me.TP_DATA.ColumnCount = 1
            Me.TP_DATA.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_DATA.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TP_DATA.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_DATA.Location = New System.Drawing.Point(0, 25)
            Me.TP_DATA.Name = "TP_DATA"
            Me.TP_DATA.RowCount = 11
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.Size = New System.Drawing.Size(484, 436)
            Me.TP_DATA.TabIndex = 1
            '
            'DownloadFeedForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Window
            Me.ClientSize = New System.Drawing.Size(484, 461)
            Me.Controls.Add(Me.TP_DATA)
            Me.Controls.Add(Me.ToolbarTOP)
            Me.ForeColor = System.Drawing.SystemColors.WindowText
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.KeyPreview = True
            Me.MinimumSize = New System.Drawing.Size(300, 300)
            Me.Name = "DownloadFeedForm"
            Me.Text = "Download Feed"
            Me.ToolbarTOP.ResumeLayout(False)
            Me.ToolbarTOP.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Private WithEvents ToolbarTOP As ToolStrip
        Private WithEvents TP_DATA As TableLayoutPanel
        Private WithEvents BTT_REFRESH As ToolStripButton
        Private WithEvents BTT_CLEAR As ToolStripButton
    End Class
End Namespace