' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Public Class MainFrame : Inherits SCrawler.DownloadObjects.STDownloader.VideoListForm
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
    Private components As System.ComponentModel.IContainer
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainFrame))
        Dim CONTEXT_SEP_1 As System.Windows.Forms.ToolStripSeparator
        Me.TRAY_ICON = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.TRAY_CONTEXT = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.BTT_TRAY_CLOSE = New System.Windows.Forms.ToolStripMenuItem()
        Me.CONTEXT_BTT_ADD = New PersonalUtilities.Forms.Controls.KeyClick.ToolStripMenuItemKeyClick()
        CONTEXT_SEP_1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TRAY_CONTEXT.SuspendLayout()
        Me.SuspendLayout()
        '
        'TRAY_ICON
        '
        Me.TRAY_ICON.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info
        Me.TRAY_ICON.BalloonTipTitle = "YouTube Downloader"
        Me.TRAY_ICON.ContextMenuStrip = Me.TRAY_CONTEXT
        Me.TRAY_ICON.Icon = CType(resources.GetObject("TRAY_ICON.Icon"), System.Drawing.Icon)
        Me.TRAY_ICON.Text = "YouTube Downloader" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Ctrl+Click to add download"
        '
        'TRAY_CONTEXT
        '
        Me.TRAY_CONTEXT.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CONTEXT_BTT_ADD, CONTEXT_SEP_1, Me.BTT_TRAY_CLOSE})
        Me.TRAY_CONTEXT.Name = "ContextMenuStrip1"
        Me.TRAY_CONTEXT.Size = New System.Drawing.Size(181, 76)
        '
        'BTT_TRAY_CLOSE
        '
        Me.BTT_TRAY_CLOSE.Image = CType(resources.GetObject("BTT_TRAY_CLOSE.Image"), System.Drawing.Image)
        Me.BTT_TRAY_CLOSE.Name = "BTT_TRAY_CLOSE"
        Me.BTT_TRAY_CLOSE.Size = New System.Drawing.Size(180, 22)
        Me.BTT_TRAY_CLOSE.Text = "Close"
        '
        'CONTEXT_BTT_ADD
        '
        Me.CONTEXT_BTT_ADD.Name = "CONTEXT_BTT_ADD"
        Me.CONTEXT_BTT_ADD.Size = New System.Drawing.Size(180, 22)
        Me.CONTEXT_BTT_ADD.Text = "Add"
        Me.CONTEXT_BTT_ADD.Image = Global.PersonalUtilities.My.Resources.PlusPic_Green_24
        '
        'CONTEXT_SEP_1
        '
        CONTEXT_SEP_1.Name = "CONTEXT_SEP_1"
        CONTEXT_SEP_1.Size = New System.Drawing.Size(177, 6)
        '
        'MainFrame
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(1008, 729)
        Me.Name = "MainFrame"
        Me.TRAY_CONTEXT.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents TRAY_ICON As NotifyIcon
    Private WithEvents TRAY_CONTEXT As ContextMenuStrip
    Private WithEvents BTT_TRAY_CLOSE As ToolStripMenuItem
    Private WithEvents CONTEXT_BTT_ADD As PersonalUtilities.Forms.Controls.KeyClick.ToolStripMenuItemKeyClick
End Class