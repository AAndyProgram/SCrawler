' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports LibVLCSharp
Imports System.ComponentModel
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.WEB
Namespace DownloadObjects
    <ToolboxItem(False), DesignTimeVisible(False)>
    Public Class FeedVideo
        Private WithEvents MediaPlayer As [Shared].MediaPlayer
        Private ReadOnly TimeChange As Action = Sub()
                                                    Dim v# = DivideWithZeroChecking(MediaPlayer.Time, MediaPlayer.Length) * 10
                                                    If v > 10 Then TR_POSITION.Value = 10 Else TR_POSITION.Value = v
                                                End Sub
        Private ReadOnly TimeChangeLabel As Action = Sub()
                                                         If MediaPlayer.Time >= 0 Then
                                                             Dim t As TimeSpan = TimeSpan.FromMilliseconds(MediaPlayer.Time)
                                                             LBL_TIME.Text = $"{VideoLength}/{t}"
                                                         End If
                                                     End Sub
        Private ReadOnly MyImage As ImageRenderer
        Friend ReadOnly VideoLength As TimeSpan
        Public Sub New()
            InitializeComponent()
        End Sub
        Friend Sub New(ByVal File As SFile)
            InitializeComponent()
            Dim debugLogs As Boolean = False
#If DEBUG Then
            debugLogs = True
#End If
            MediaPlayer = New [Shared].MediaPlayer(New [Shared].Media(New [Shared].LibVLC(enableDebugLogs:=debugLogs), New Uri(File.ToString)))
            MyVideo.MediaPlayer = MediaPlayer
            TR_VOLUME.Value = MediaPlayer.Volume / 10
            If MediaPlayer.Length >= 0 Then VideoLength = TimeSpan.FromMilliseconds(MediaPlayer.Length)
            If Settings.UseM3U8 Then
                Dim f As SFile = $"{Settings.CachePath.PathWithSeparator}FeedSnapshots\{File.GetHashCode}.png"
                If Not f.Exists Then f = FFMPEG.TakeSnapshot(File, f, Settings.FfmpegFile, TimeSpan.FromSeconds(1))
                If f.Exists Then
                    MyImage = New ImageRenderer(f, EDP.None)
                    If Not MyImage.HasError Then
                        MyVideo.BackgroundImage = MyImage
                        MyVideo.BackgroundImageLayout = ImageLayout.Zoom
                    End If
                End If
            End If
        End Sub
        Private Sub FeedVideo_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            If Not MediaPlayer Is Nothing Then MediaPlayer.Dispose()
            If Not MyImage Is Nothing Then MyImage.Dispose()
        End Sub
        Private Sub BTT_PLAY_Click(sender As Object, e As EventArgs) Handles BTT_PLAY.Click
            Try : MediaPlayer.Play() : Catch : End Try
        End Sub
        Private Sub BTT_PAUSE_Click(sender As Object, e As EventArgs) Handles BTT_PAUSE.Click
            Try : MediaPlayer.Pause() : Catch : End Try
        End Sub
        Private Sub BTT_STOP_Click(sender As Object, e As EventArgs) Handles BTT_STOP.Click
            Try : MediaPlayer.Stop() : Catch : End Try
        End Sub
        Private Sub MediaPlayer_TimeChanged(sender As Object, e As [Shared].MediaPlayerTimeChangedEventArgs) Handles MediaPlayer.TimeChanged
            If TR_POSITION.InvokeRequired Then TR_POSITION.Invoke(TimeChange) Else TimeChange.Invoke
            If LBL_TIME.InvokeRequired Then LBL_TIME.Invoke(TimeChangeLabel) Else TimeChangeLabel.Invoke
        End Sub
        Private Sub TR_POSITION_MouseUp(sender As Object, e As MouseEventArgs) Handles TR_POSITION.MouseUp
            Try : MediaPlayer.Time = (MediaPlayer.Length / 100) * (TR_POSITION.Value * 10) : Catch : End Try
        End Sub
        Private Sub TR_VOLUME_MouseUp(sender As Object, e As MouseEventArgs) Handles TR_VOLUME.MouseUp
            Try : MediaPlayer.Volume = TR_VOLUME.Value * 10 : Catch : End Try
        End Sub
        Private Sub MediaPlayer_Stopped(sender As Object, e As EventArgs) Handles MediaPlayer.Stopped
            Dim a As Action = Sub() TR_POSITION.Value = TR_POSITION.Maximum
            If TR_POSITION.InvokeRequired Then TR_POSITION.Invoke(a) Else a.Invoke
        End Sub
    End Class
End Namespace