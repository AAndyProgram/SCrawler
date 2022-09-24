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
Imports VLCState = LibVLCSharp.Shared.VLCState
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
                                                             If Not VideoLength.HasValue Then
                                                                 VideoLength = TimeSpan.FromMilliseconds(MediaPlayer.Length)
                                                                 VideoLengthStr = VideoLength.Value.ToStringTime(FeedVideoLengthProvider)
                                                             End If
                                                             LBL_TIME.Text = $"{t.ToStringTime(FeedVideoLengthProvider)}/{VideoLengthStr}"
                                                         End If
                                                     End Sub
        Private ReadOnly MyImage As ImageRenderer
        Private VideoLength As TimeSpan?
        Private VideoLengthStr As String
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
            UpdateButtons()
        End Sub
        Private Sub FeedVideo_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            If Not MediaPlayer Is Nothing Then MediaPlayer.Dispose()
            If Not MyImage Is Nothing Then MyImage.Dispose()
        End Sub
        Private Sub BTT_PLAY_Click(sender As Object, e As EventArgs) Handles BTT_PLAY.Click
            Try
                Select Case MediaPlayer.State
                    Case VLCState.NothingSpecial, VLCState.Stopped, VLCState.Paused : MediaPlayer.Play()
                    Case VLCState.Ended : MediaPlayer.Stop() : MediaPlayer.Play()
                End Select
            Catch
            Finally
                UpdateButtons()
            End Try
        End Sub
        Private Sub BTT_PAUSE_Click(sender As Object, e As EventArgs) Handles BTT_PAUSE.Click
            Try : MediaPlayer.Pause() : Catch : End Try
            UpdateButtons()
        End Sub
        Friend Sub [Stop]() Handles BTT_STOP.Click
            Try : MediaPlayer.Stop() : Catch : End Try
            UpdateButtons()
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
            UpdateButtons()
        End Sub
        Private Sub UpdateButtons() Handles MediaPlayer.Playing, MediaPlayer.Paused, MediaPlayer.Opening
            Try
                Dim _play As Boolean = False, _pause As Boolean = False, _stop As Boolean = False
                Select Case MediaPlayer.State
                    Case VLCState.NothingSpecial, VLCState.Stopped : _play = True
                    Case VLCState.Paused : _play = True : _stop = True
                    Case VLCState.Ended : _play = True
                    Case VLCState.Playing : _pause = True : _stop = True
                End Select
                ControlInvoke(BTT_PLAY, Sub() BTT_PLAY.Enabled = _play)
                ControlInvoke(BTT_PAUSE, Sub() BTT_PAUSE.Enabled = _pause)
                ControlInvoke(BTT_STOP, Sub() BTT_STOP.Enabled = _stop)
            Catch
            End Try
        End Sub
    End Class
End Namespace