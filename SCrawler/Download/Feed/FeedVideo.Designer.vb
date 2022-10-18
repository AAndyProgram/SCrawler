' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace DownloadObjects
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Public Class FeedVideo : Inherits System.Windows.Forms.UserControl
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
            Dim TP_MAIN As System.Windows.Forms.TableLayoutPanel
            Dim TP_BUTTONS As System.Windows.Forms.TableLayoutPanel
            Me.MyVideo = New LibVLCSharp.WinForms.VideoView()
            Me.TR_POSITION = New System.Windows.Forms.TrackBar()
            Me.BTT_PLAY = New System.Windows.Forms.Button()
            Me.BTT_PAUSE = New System.Windows.Forms.Button()
            Me.BTT_STOP = New System.Windows.Forms.Button()
            Me.TR_VOLUME = New System.Windows.Forms.TrackBar()
            Me.LBL_TIME = New System.Windows.Forms.Label()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            TP_BUTTONS = New System.Windows.Forms.TableLayoutPanel()
            TP_MAIN.SuspendLayout()
            CType(Me.MyVideo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TR_POSITION, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_BUTTONS.SuspendLayout()
            CType(Me.TR_VOLUME, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'TP_MAIN
            '
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(Me.MyVideo, 0, 0)
            TP_MAIN.Controls.Add(Me.TR_POSITION, 0, 1)
            TP_MAIN.Controls.Add(TP_BUTTONS, 0, 2)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Margin = New System.Windows.Forms.Padding(0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 3
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.Size = New System.Drawing.Size(180, 160)
            TP_MAIN.TabIndex = 0
            '
            'MyVideo
            '
            Me.MyVideo.BackColor = System.Drawing.Color.Black
            Me.MyVideo.Dock = System.Windows.Forms.DockStyle.Fill
            Me.MyVideo.Location = New System.Drawing.Point(1, 1)
            Me.MyVideo.Margin = New System.Windows.Forms.Padding(1)
            Me.MyVideo.MediaPlayer = Nothing
            Me.MyVideo.Name = "MyVideo"
            Me.MyVideo.Size = New System.Drawing.Size(178, 105)
            Me.MyVideo.TabIndex = 0
            '
            'TR_POSITION
            '
            Me.TR_POSITION.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TR_POSITION.Location = New System.Drawing.Point(3, 110)
            Me.TR_POSITION.Name = "TR_POSITION"
            Me.TR_POSITION.Size = New System.Drawing.Size(174, 19)
            Me.TR_POSITION.TabIndex = 1
            '
            'TP_BUTTONS
            '
            TP_BUTTONS.ColumnCount = 5
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
            TP_BUTTONS.Controls.Add(Me.BTT_PLAY, 0, 0)
            TP_BUTTONS.Controls.Add(Me.BTT_PAUSE, 1, 0)
            TP_BUTTONS.Controls.Add(Me.BTT_STOP, 2, 0)
            TP_BUTTONS.Controls.Add(Me.TR_VOLUME, 4, 0)
            TP_BUTTONS.Controls.Add(Me.LBL_TIME, 3, 0)
            TP_BUTTONS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_BUTTONS.Location = New System.Drawing.Point(1, 133)
            TP_BUTTONS.Margin = New System.Windows.Forms.Padding(1)
            TP_BUTTONS.Name = "TP_BUTTONS"
            TP_BUTTONS.RowCount = 1
            TP_BUTTONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_BUTTONS.Size = New System.Drawing.Size(178, 26)
            TP_BUTTONS.TabIndex = 2
            '
            'BTT_PLAY
            '
            Me.BTT_PLAY.BackgroundImage = Global.SCrawler.My.Resources.Resources.StartPic_Green_16
            Me.BTT_PLAY.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
            Me.BTT_PLAY.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_PLAY.Location = New System.Drawing.Point(1, 1)
            Me.BTT_PLAY.Margin = New System.Windows.Forms.Padding(1)
            Me.BTT_PLAY.Name = "BTT_PLAY"
            Me.BTT_PLAY.Size = New System.Drawing.Size(23, 24)
            Me.BTT_PLAY.TabIndex = 0
            Me.BTT_PLAY.UseVisualStyleBackColor = True
            '
            'BTT_PAUSE
            '
            Me.BTT_PAUSE.BackgroundImage = Global.SCrawler.My.Resources.Resources.Pause_Blue_16
            Me.BTT_PAUSE.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
            Me.BTT_PAUSE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_PAUSE.Location = New System.Drawing.Point(26, 1)
            Me.BTT_PAUSE.Margin = New System.Windows.Forms.Padding(1)
            Me.BTT_PAUSE.Name = "BTT_PAUSE"
            Me.BTT_PAUSE.Size = New System.Drawing.Size(23, 24)
            Me.BTT_PAUSE.TabIndex = 1
            Me.BTT_PAUSE.UseVisualStyleBackColor = True
            '
            'BTT_STOP
            '
            Me.BTT_STOP.BackgroundImage = Global.SCrawler.My.Resources.Resources.StopPic_32
            Me.BTT_STOP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
            Me.BTT_STOP.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_STOP.Location = New System.Drawing.Point(51, 1)
            Me.BTT_STOP.Margin = New System.Windows.Forms.Padding(1)
            Me.BTT_STOP.Name = "BTT_STOP"
            Me.BTT_STOP.Size = New System.Drawing.Size(23, 24)
            Me.BTT_STOP.TabIndex = 2
            Me.BTT_STOP.UseVisualStyleBackColor = True
            '
            'TR_VOLUME
            '
            Me.TR_VOLUME.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TR_VOLUME.Location = New System.Drawing.Point(81, 3)
            Me.TR_VOLUME.Name = "TR_VOLUME"
            Me.TR_VOLUME.Size = New System.Drawing.Size(94, 20)
            Me.TR_VOLUME.TabIndex = 3
            '
            'LBL_TIME
            '
            Me.LBL_TIME.AutoSize = True
            Me.LBL_TIME.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LBL_TIME.Location = New System.Drawing.Point(78, 0)
            Me.LBL_TIME.Name = "LBL_TIME"
            Me.LBL_TIME.Size = New System.Drawing.Size(1, 26)
            Me.LBL_TIME.TabIndex = 4
            Me.LBL_TIME.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'FeedVideo
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(TP_MAIN)
            Me.Name = "FeedVideo"
            Me.Size = New System.Drawing.Size(180, 160)
            TP_MAIN.ResumeLayout(False)
            TP_MAIN.PerformLayout()
            CType(Me.MyVideo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TR_POSITION, System.ComponentModel.ISupportInitialize).EndInit()
            TP_BUTTONS.ResumeLayout(False)
            TP_BUTTONS.PerformLayout()
            CType(Me.TR_VOLUME, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents MyVideo As LibVLCSharp.WinForms.VideoView
        Private WithEvents TR_POSITION As TrackBar
        Private WithEvents BTT_PLAY As Button
        Private WithEvents BTT_PAUSE As Button
        Private WithEvents BTT_STOP As Button
        Private WithEvents TR_VOLUME As TrackBar
        Private WithEvents LBL_TIME As Label
    End Class
End Namespace