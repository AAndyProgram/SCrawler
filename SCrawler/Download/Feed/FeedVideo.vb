' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports LibVLCSharp.Shared
Imports System.Threading
Imports System.ComponentModel
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.Web
Imports VLCState = LibVLCSharp.Shared.VLCState
Namespace DownloadObjects
    <ToolboxItem(False), DesignTimeVisible(False)>
    Public Class FeedVideo
        Private WithEvents MediaPlayer As MediaPlayer
        Private ReadOnly TimeChange As Action = Sub()
                                                    If _Disposed Then Exit Sub
                                                    Dim v# = DivideWithZeroChecking(MediaPlayer.Time, MediaPlayer.Length) * 10
                                                    If v > 10 Then TR_POSITION.Value = 10 Else TR_POSITION.Value = v
                                                End Sub
        Private ReadOnly TimeChangeLabel As Action = Sub()
                                                         If _Disposed Then Exit Sub
                                                         If MediaPlayer.Time >= 0 Then
                                                             Dim t As TimeSpan = TimeSpan.FromMilliseconds(MediaPlayer.Time)
                                                             If Not VideoLength.HasValue Then
                                                                 VideoLengthMs = MediaPlayer.Length
                                                                 VideoLength = TimeSpan.FromMilliseconds(VideoLengthMs)
                                                                 VideoLengthStr = VideoLength.Value.ToStringTime(FeedVideoLengthProvider)
                                                             End If
                                                             LBL_TIME.Text = $"{t.ToStringTime(FeedVideoLengthProvider)}/{VideoLengthStr}"
                                                         End If
                                                     End Sub
        Private ReadOnly MyImage As ImageRenderer
        Private VideoLength As TimeSpan?
        Private VideoLengthMs As Integer = 0
        Private VideoLengthStr As String
        Private MediaFile As SFile = Nothing
        Friend ReadOnly HasError As Boolean = False
        Public Sub New()
            InitializeComponent()
        End Sub
        Friend Sub New(ByVal File As SFile)
            InitializeComponent()
            Try
                MediaFile = File
                Dim debugLogs As Boolean = False
                '#If DEBUG Then
                '            debugLogs = True
                '#End If
                MediaPlayer = New MediaPlayer(New Media(New LibVLC(enableDebugLogs:=debugLogs), New Uri(File.ToString)))
                MyVideo.MediaPlayer = MediaPlayer
                TR_VOLUME.Value = MediaPlayer.Volume / 10
                If Settings.UseM3U8 Then
                    Dim f As SFile = $"{Settings.Cache.RootDirectory.PathWithSeparator}FeedSnapshots\{File.GetHashCode}.png"
                    If Not f.Exists Then f = FFMPEG.TakeSnapshot(File, f, Settings.FfmpegFile.File, TimeSpan.FromSeconds(1))
                    If f.Exists Then
                        MyImage = New ImageRenderer(f, EDP.None)
                        Try
                            If Not MyImage.HasError Then
                                MyVideo.BackgroundImage = MyImage
                                MyVideo.BackgroundImageLayout = ImageLayout.Zoom
                            End If
                        Catch img_set_ex As Exception
                            ErrorsDescriber.Execute(EDP.SendToLog, img_set_ex, "Error setting background image for media player." & vbCr &
                                                                               $"File: {File}{vbCr}Image: {f}")
                        End Try
                    End If
                End If
                UpdateButtons()
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"Media player initialization error({File})")
                HasError = True
            End Try
        End Sub
        Private _Disposed As Boolean = False
        Private Sub FeedVideo_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            _Disposed = True
            If Not MediaPlayer Is Nothing Then MediaPlayer.Dispose()
            If Not MyImage Is Nothing Then MyImage.Dispose()
        End Sub
        Private Async Sub BTT_PLAY_Click(sender As Object, e As EventArgs) Handles BTT_PLAY.Click
            If _Disposed Then Exit Sub
            Try
                Dim p As Boolean = False
                Select Case MediaPlayer.State
                    Case VLCState.NothingSpecial, VLCState.Stopped, VLCState.Paused : p = True
                    Case VLCState.Ended : Await Task.Run(Sub() [Stop]()) : p = True
                End Select
                If p Then Await RunAction(Sub()
                                              Try : MediaPlayer.Play() : Catch : End Try
                                          End Sub, "Play")
            Catch
            Finally
                UpdateButtons()
            End Try
        End Sub
        Private Async Sub BTT_PAUSE_Click(sender As Object, e As EventArgs) Handles BTT_PAUSE.Click
            If _Disposed Then Exit Sub
            Await RunAction(Sub()
                                Try : MediaPlayer.Pause() : Catch : End Try
                            End Sub, "Pause")
            UpdateButtons()
        End Sub
        Friend Async Sub [Stop]() Handles BTT_STOP.Click
            If _Disposed Then Exit Sub
            Await RunAction(Sub()
                                Try : MediaPlayer.Stop() : Catch : End Try
                            End Sub, "Stop")
            UpdateButtons()
        End Sub
        Private Sub MediaPlayer_TimeChanged(sender As Object, e As MediaPlayerTimeChangedEventArgs) Handles MediaPlayer.TimeChanged
            If _Disposed Then Exit Sub
            If TR_POSITION.InvokeRequired Then TR_POSITION.Invoke(TimeChange) Else TimeChange.Invoke
            If LBL_TIME.InvokeRequired Then LBL_TIME.Invoke(TimeChangeLabel) Else TimeChangeLabel.Invoke
        End Sub
        Private Async Sub TR_POSITION_MouseUp(sender As Object, e As MouseEventArgs) Handles TR_POSITION.MouseUp
            If _Disposed Then Exit Sub
            Try
                Dim p% = e.X
                Dim w% = TR_POSITION.Width
                Dim v#
                If p >= w Then
                    [Stop]()
                Else
                    If p <= 0 Then
                        v = 0
                    Else
                        v = VideoLengthMs / 100 * (DivideWithZeroChecking(p, w) * 100).RoundVal(2)
                    End If
                    Await RunAction(Sub()
                                        Try : MediaPlayer.Time = v : Catch : End Try
                                    End Sub, "TimeChange")
                End If
            Catch
            End Try
        End Sub
        Private Sub TR_VOLUME_MouseUp(sender As Object, e As MouseEventArgs) Handles TR_VOLUME.MouseUp
            If _Disposed Then Exit Sub
            Try : MediaPlayer.Volume = TR_VOLUME.Value * 10 : Catch : End Try
        End Sub
        Private Sub MediaPlayer_Stopped(sender As Object, e As EventArgs) Handles MediaPlayer.Stopped
            If _Disposed Then Exit Sub
            Dim a As Action = Sub() TR_POSITION.Value = TR_POSITION.Maximum
            If TR_POSITION.InvokeRequired Then TR_POSITION.Invoke(a) Else a.Invoke
            UpdateButtons()
        End Sub
        Private Sub UpdateButtons() Handles MediaPlayer.Playing, MediaPlayer.Paused, MediaPlayer.Opening
            Try
                If _Disposed Then Exit Sub
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
        Private Async Function RunAction(ByVal Action As Action, ByVal ActionName As String) As Task
            Try
                If _Disposed Then Exit Function
                Using ts As New CancellationTokenSource
                    Dim token As CancellationToken = ts.Token
                    Using t As New Timer(Sub(cts) ts.Cancel(), ts, 5000, Timeout.Infinite) : Await Task.Run(Action, token) : End Using
                End Using
            Catch oex As OperationCanceledException
                MyMainLOG = $"Cannot perform action [{ActionName}] on file [{MediaFile}]"
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"An error occurred while performing action [{ActionName}] on file [{MediaFile}]")
            End Try
        End Function
        Private Sub MyVideo_DoubleClick(sender As Object, e As EventArgs) Handles MyVideo.DoubleClick
            [Stop]()
            OnDoubleClick(e)
        End Sub
    End Class
End Namespace