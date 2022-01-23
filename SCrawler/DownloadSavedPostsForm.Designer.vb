' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Friend Class DownloadSavedPostsForm : Inherits System.Windows.Forms.Form
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
        Dim TP_MAIN As System.Windows.Forms.TableLayoutPanel
        Dim TP_BUTTONS As System.Windows.Forms.TableLayoutPanel
        Dim TP_REDDIT As System.Windows.Forms.TableLayoutPanel
        Dim TP_REDDIT_PR As System.Windows.Forms.TableLayoutPanel
        Dim TP_INST As System.Windows.Forms.TableLayoutPanel
        Dim TP_INST_PR As System.Windows.Forms.TableLayoutPanel
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DownloadSavedPostsForm))
        Dim TT_MAIN As System.Windows.Forms.ToolTip
        Me.BTT_DOWN_ALL = New System.Windows.Forms.Button()
        Me.BTT_STOP_ALL = New System.Windows.Forms.Button()
        Me.BTT_REDDIT_START = New System.Windows.Forms.Button()
        Me.BTT_REDDIT_STOP = New System.Windows.Forms.Button()
        Me.PR_REDDIT = New System.Windows.Forms.ProgressBar()
        Me.BTT_REDDIT_OPEN = New System.Windows.Forms.Button()
        Me.LBL_REDDIT = New System.Windows.Forms.Label()
        Me.BTT_INST_START = New System.Windows.Forms.Button()
        Me.BTT_INST_STOP = New System.Windows.Forms.Button()
        Me.PR_INST = New System.Windows.Forms.ProgressBar()
        Me.BTT_INST_OPEN = New System.Windows.Forms.Button()
        Me.LBL_INST = New System.Windows.Forms.Label()
        TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
        TP_BUTTONS = New System.Windows.Forms.TableLayoutPanel()
        TP_REDDIT = New System.Windows.Forms.TableLayoutPanel()
        TP_REDDIT_PR = New System.Windows.Forms.TableLayoutPanel()
        TP_INST = New System.Windows.Forms.TableLayoutPanel()
        TP_INST_PR = New System.Windows.Forms.TableLayoutPanel()
        TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
        TP_MAIN.SuspendLayout()
        TP_BUTTONS.SuspendLayout()
        TP_REDDIT.SuspendLayout()
        TP_REDDIT_PR.SuspendLayout()
        TP_INST.SuspendLayout()
        TP_INST_PR.SuspendLayout()
        Me.SuspendLayout()
        '
        'TP_MAIN
        '
        TP_MAIN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset
        TP_MAIN.ColumnCount = 1
        TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        TP_MAIN.Controls.Add(TP_BUTTONS, 0, 0)
        TP_MAIN.Controls.Add(TP_REDDIT, 0, 1)
        TP_MAIN.Controls.Add(TP_INST, 0, 2)
        TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
        TP_MAIN.Location = New System.Drawing.Point(0, 0)
        TP_MAIN.Name = "TP_MAIN"
        TP_MAIN.RowCount = 3
        TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        TP_MAIN.Size = New System.Drawing.Size(484, 156)
        TP_MAIN.TabIndex = 0
        '
        'TP_BUTTONS
        '
        TP_BUTTONS.ColumnCount = 2
        TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        TP_BUTTONS.Controls.Add(Me.BTT_DOWN_ALL, 0, 0)
        TP_BUTTONS.Controls.Add(Me.BTT_STOP_ALL, 1, 0)
        TP_BUTTONS.Dock = System.Windows.Forms.DockStyle.Fill
        TP_BUTTONS.Location = New System.Drawing.Point(2, 2)
        TP_BUTTONS.Margin = New System.Windows.Forms.Padding(0)
        TP_BUTTONS.Name = "TP_BUTTONS"
        TP_BUTTONS.RowCount = 1
        TP_BUTTONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        TP_BUTTONS.Size = New System.Drawing.Size(480, 30)
        TP_BUTTONS.TabIndex = 0
        '
        'BTT_DOWN_ALL
        '
        Me.BTT_DOWN_ALL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BTT_DOWN_ALL.Location = New System.Drawing.Point(3, 3)
        Me.BTT_DOWN_ALL.Name = "BTT_DOWN_ALL"
        Me.BTT_DOWN_ALL.Size = New System.Drawing.Size(234, 24)
        Me.BTT_DOWN_ALL.TabIndex = 0
        Me.BTT_DOWN_ALL.Text = "Download ALL"
        Me.BTT_DOWN_ALL.UseVisualStyleBackColor = True
        '
        'BTT_STOP_ALL
        '
        Me.BTT_STOP_ALL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BTT_STOP_ALL.Location = New System.Drawing.Point(243, 3)
        Me.BTT_STOP_ALL.Name = "BTT_STOP_ALL"
        Me.BTT_STOP_ALL.Size = New System.Drawing.Size(234, 24)
        Me.BTT_STOP_ALL.TabIndex = 1
        Me.BTT_STOP_ALL.Text = "Stop ALL"
        Me.BTT_STOP_ALL.UseVisualStyleBackColor = True
        '
        'TP_REDDIT
        '
        TP_REDDIT.ColumnCount = 1
        TP_REDDIT.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        TP_REDDIT.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        TP_REDDIT.Controls.Add(TP_REDDIT_PR, 0, 0)
        TP_REDDIT.Controls.Add(Me.LBL_REDDIT, 0, 1)
        TP_REDDIT.Dock = System.Windows.Forms.DockStyle.Fill
        TP_REDDIT.Location = New System.Drawing.Point(2, 34)
        TP_REDDIT.Margin = New System.Windows.Forms.Padding(0)
        TP_REDDIT.Name = "TP_REDDIT"
        TP_REDDIT.RowCount = 2
        TP_REDDIT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        TP_REDDIT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        TP_REDDIT.Size = New System.Drawing.Size(480, 59)
        TP_REDDIT.TabIndex = 1
        '
        'TP_REDDIT_PR
        '
        TP_REDDIT_PR.ColumnCount = 4
        TP_REDDIT_PR.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        TP_REDDIT_PR.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        TP_REDDIT_PR.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        TP_REDDIT_PR.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        TP_REDDIT_PR.Controls.Add(Me.BTT_REDDIT_START, 0, 0)
        TP_REDDIT_PR.Controls.Add(Me.BTT_REDDIT_STOP, 1, 0)
        TP_REDDIT_PR.Controls.Add(Me.PR_REDDIT, 3, 0)
        TP_REDDIT_PR.Controls.Add(Me.BTT_REDDIT_OPEN, 2, 0)
        TP_REDDIT_PR.Dock = System.Windows.Forms.DockStyle.Fill
        TP_REDDIT_PR.Location = New System.Drawing.Point(0, 0)
        TP_REDDIT_PR.Margin = New System.Windows.Forms.Padding(0)
        TP_REDDIT_PR.Name = "TP_REDDIT_PR"
        TP_REDDIT_PR.RowCount = 1
        TP_REDDIT_PR.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        TP_REDDIT_PR.Size = New System.Drawing.Size(480, 29)
        TP_REDDIT_PR.TabIndex = 0
        '
        'BTT_REDDIT_START
        '
        Me.BTT_REDDIT_START.BackgroundImage = Global.SCrawler.My.Resources.Resources.StartPic_01_Green_16
        Me.BTT_REDDIT_START.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BTT_REDDIT_START.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BTT_REDDIT_START.Location = New System.Drawing.Point(3, 3)
        Me.BTT_REDDIT_START.Name = "BTT_REDDIT_START"
        Me.BTT_REDDIT_START.Size = New System.Drawing.Size(24, 23)
        Me.BTT_REDDIT_START.TabIndex = 0
        TT_MAIN.SetToolTip(Me.BTT_REDDIT_START, "Start downloading saved Reddit posts")
        Me.BTT_REDDIT_START.UseVisualStyleBackColor = True
        '
        'BTT_REDDIT_STOP
        '
        Me.BTT_REDDIT_STOP.BackgroundImage = Global.SCrawler.My.Resources.Resources.Delete
        Me.BTT_REDDIT_STOP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BTT_REDDIT_STOP.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BTT_REDDIT_STOP.Enabled = False
        Me.BTT_REDDIT_STOP.Location = New System.Drawing.Point(33, 3)
        Me.BTT_REDDIT_STOP.Name = "BTT_REDDIT_STOP"
        Me.BTT_REDDIT_STOP.Size = New System.Drawing.Size(24, 23)
        Me.BTT_REDDIT_STOP.TabIndex = 1
        TT_MAIN.SetToolTip(Me.BTT_REDDIT_STOP, "Stop downloading saved Reddit posts")
        Me.BTT_REDDIT_STOP.UseVisualStyleBackColor = True
        '
        'PR_REDDIT
        '
        Me.PR_REDDIT.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PR_REDDIT.Location = New System.Drawing.Point(93, 3)
        Me.PR_REDDIT.Name = "PR_REDDIT"
        Me.PR_REDDIT.Size = New System.Drawing.Size(384, 23)
        Me.PR_REDDIT.TabIndex = 2
        '
        'BTT_REDDIT_OPEN
        '
        Me.BTT_REDDIT_OPEN.BackgroundImage = CType(resources.GetObject("BTT_REDDIT_OPEN.BackgroundImage"), System.Drawing.Image)
        Me.BTT_REDDIT_OPEN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BTT_REDDIT_OPEN.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BTT_REDDIT_OPEN.Location = New System.Drawing.Point(63, 3)
        Me.BTT_REDDIT_OPEN.Name = "BTT_REDDIT_OPEN"
        Me.BTT_REDDIT_OPEN.Size = New System.Drawing.Size(24, 23)
        Me.BTT_REDDIT_OPEN.TabIndex = 3
        Me.BTT_REDDIT_OPEN.UseVisualStyleBackColor = True
        '
        'LBL_REDDIT
        '
        Me.LBL_REDDIT.AutoSize = True
        Me.LBL_REDDIT.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LBL_REDDIT.Location = New System.Drawing.Point(3, 29)
        Me.LBL_REDDIT.Name = "LBL_REDDIT"
        Me.LBL_REDDIT.Size = New System.Drawing.Size(474, 30)
        Me.LBL_REDDIT.TabIndex = 1
        Me.LBL_REDDIT.Text = "Reddit"
        Me.LBL_REDDIT.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'TP_INST
        '
        TP_INST.ColumnCount = 1
        TP_INST.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        TP_INST.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        TP_INST.Controls.Add(TP_INST_PR, 0, 0)
        TP_INST.Controls.Add(Me.LBL_INST, 0, 1)
        TP_INST.Dock = System.Windows.Forms.DockStyle.Fill
        TP_INST.Location = New System.Drawing.Point(2, 95)
        TP_INST.Margin = New System.Windows.Forms.Padding(0)
        TP_INST.Name = "TP_INST"
        TP_INST.RowCount = 2
        TP_INST.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        TP_INST.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        TP_INST.Size = New System.Drawing.Size(480, 59)
        TP_INST.TabIndex = 2
        '
        'TP_INST_PR
        '
        TP_INST_PR.ColumnCount = 4
        TP_INST_PR.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        TP_INST_PR.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        TP_INST_PR.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
        TP_INST_PR.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        TP_INST_PR.Controls.Add(Me.BTT_INST_START, 0, 0)
        TP_INST_PR.Controls.Add(Me.BTT_INST_STOP, 1, 0)
        TP_INST_PR.Controls.Add(Me.PR_INST, 3, 0)
        TP_INST_PR.Controls.Add(Me.BTT_INST_OPEN, 2, 0)
        TP_INST_PR.Dock = System.Windows.Forms.DockStyle.Fill
        TP_INST_PR.Location = New System.Drawing.Point(0, 0)
        TP_INST_PR.Margin = New System.Windows.Forms.Padding(0)
        TP_INST_PR.Name = "TP_INST_PR"
        TP_INST_PR.RowCount = 1
        TP_INST_PR.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        TP_INST_PR.Size = New System.Drawing.Size(480, 29)
        TP_INST_PR.TabIndex = 0
        '
        'BTT_INST_START
        '
        Me.BTT_INST_START.BackgroundImage = Global.SCrawler.My.Resources.Resources.StartPic_01_Green_16
        Me.BTT_INST_START.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BTT_INST_START.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BTT_INST_START.Location = New System.Drawing.Point(3, 3)
        Me.BTT_INST_START.Name = "BTT_INST_START"
        Me.BTT_INST_START.Size = New System.Drawing.Size(24, 23)
        Me.BTT_INST_START.TabIndex = 0
        TT_MAIN.SetToolTip(Me.BTT_INST_START, "Start downloading saved Instagram posts")
        Me.BTT_INST_START.UseVisualStyleBackColor = True
        '
        'BTT_INST_STOP
        '
        Me.BTT_INST_STOP.BackgroundImage = Global.SCrawler.My.Resources.Resources.Delete
        Me.BTT_INST_STOP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BTT_INST_STOP.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BTT_INST_STOP.Enabled = False
        Me.BTT_INST_STOP.Location = New System.Drawing.Point(33, 3)
        Me.BTT_INST_STOP.Name = "BTT_INST_STOP"
        Me.BTT_INST_STOP.Size = New System.Drawing.Size(24, 23)
        Me.BTT_INST_STOP.TabIndex = 1
        TT_MAIN.SetToolTip(Me.BTT_INST_STOP, "Stop downloading saved Instagram posts")
        Me.BTT_INST_STOP.UseVisualStyleBackColor = True
        '
        'PR_INST
        '
        Me.PR_INST.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PR_INST.Location = New System.Drawing.Point(93, 3)
        Me.PR_INST.Name = "PR_INST"
        Me.PR_INST.Size = New System.Drawing.Size(384, 23)
        Me.PR_INST.TabIndex = 2
        '
        'BTT_INST_OPEN
        '
        Me.BTT_INST_OPEN.BackgroundImage = CType(resources.GetObject("BTT_INST_OPEN.BackgroundImage"), System.Drawing.Image)
        Me.BTT_INST_OPEN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.BTT_INST_OPEN.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BTT_INST_OPEN.Location = New System.Drawing.Point(63, 3)
        Me.BTT_INST_OPEN.Name = "BTT_INST_OPEN"
        Me.BTT_INST_OPEN.Size = New System.Drawing.Size(24, 23)
        Me.BTT_INST_OPEN.TabIndex = 3
        Me.BTT_INST_OPEN.UseVisualStyleBackColor = True
        '
        'LBL_INST
        '
        Me.LBL_INST.AutoSize = True
        Me.LBL_INST.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LBL_INST.Location = New System.Drawing.Point(3, 29)
        Me.LBL_INST.Name = "LBL_INST"
        Me.LBL_INST.Size = New System.Drawing.Size(474, 30)
        Me.LBL_INST.TabIndex = 1
        Me.LBL_INST.Text = "Instagram"
        Me.LBL_INST.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'DownloadSavedPostsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 156)
        Me.Controls.Add(TP_MAIN)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(500, 195)
        Me.MinimumSize = New System.Drawing.Size(500, 195)
        Me.Name = "DownloadSavedPostsForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.Text = "Saved posts"
        TP_MAIN.ResumeLayout(False)
        TP_BUTTONS.ResumeLayout(False)
        TP_REDDIT.ResumeLayout(False)
        TP_REDDIT.PerformLayout()
        TP_REDDIT_PR.ResumeLayout(False)
        TP_INST.ResumeLayout(False)
        TP_INST.PerformLayout()
        TP_INST_PR.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents BTT_DOWN_ALL As Button
    Private WithEvents BTT_STOP_ALL As Button
    Private WithEvents BTT_REDDIT_START As Button
    Private WithEvents BTT_REDDIT_STOP As Button
    Private WithEvents PR_REDDIT As ProgressBar
    Private WithEvents LBL_REDDIT As Label
    Private WithEvents BTT_INST_START As Button
    Private WithEvents BTT_INST_STOP As Button
    Private WithEvents PR_INST As ProgressBar
    Private WithEvents LBL_INST As Label
    Private WithEvents BTT_REDDIT_OPEN As Button
    Private WithEvents BTT_INST_OPEN As Button
End Class