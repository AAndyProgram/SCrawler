' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports SCrawler.API.Base
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.Web
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Functions.RegularExpressions
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace API.JustForFans
    Friend NotInheritable Class M3U8 : Implements IDisposable
#Region "Declarations"
        Friend Const AllVid As UTypes = UTypes.m3u8 + UTypes.VideoPre
        Private ReadOnly DataVideo As List(Of String)
        Private ReadOnly DataAudio As List(Of String)
        Private Media As UserMedia
        Private DestinationFile As SFile
        Private ReadOnly Thrower As Plugin.IThrower
        Private ReadOnly Responser As Responser
        Private ReadOnly ResponserInternal As Boolean
        Private Const R_VIDEO_REGEX_PATTERN As String = "(#EXT-X-STREAM-INF)(.+)(RESOLUTION=\d+x)(\d+)(.+""\s*)(\S+)(\s*)"
        Private ReadOnly REGEX_AUDIO_URL As RParams = RParams.DMS("EXT-X-MEDIA.*?URI=.([^""]+)"".*?TYPE=""AUDIO""", 1, EDP.ReturnValue)
        Private ReadOnly REGEX_PLS_FILES As RParams = RParams.DM("EXT-X-MAP:URI=""([^""]+)""|EXTINF.+?[\r\n]{1,2}(.+)", 0, RegexReturn.List, EDP.ReturnValue)
        Private UrlVideo As String
        Private UrlAudio As String
        Private FileVideo As SFile
        Private FileAudio As SFile
        Private RootPlaylistUrl As String
        Private ReadOnly Cache As CacheKeeper
        Private ReadOnly Progress As MyProgress
        Private ReadOnly ProgressPre As PreProgress
        Private ReadOnly ProgressExists As Boolean
        Private ReadOnly UsePreProgress As Boolean
        Private Property Token As CancellationToken
#End Region
#Region "Initializer"
        Private Sub New(ByVal m As UserMedia, ByVal Destination As SFile, ByVal Resp As Responser, ByVal _Thrower As Plugin.IThrower,
                        ByVal _Progress As MyProgress, ByVal _UsePreProgress As Boolean, ByVal _Token As CancellationToken)
            Media = m
            DataVideo = New List(Of String)
            DataAudio = New List(Of String)
            DestinationFile = Destination
            Thrower = _Thrower
            'Responser = Resp
            Responser = New Responser
            ResponserInternal = True
            Progress = _Progress
            ProgressExists = Not Progress Is Nothing
            If ProgressExists Then ProgressPre = New PreProgress(Progress)
            UsePreProgress = _UsePreProgress
            Token = _Token
            Cache = New CacheKeeper($"{DestinationFile.PathWithSeparator}_{M3U8Base.TempCacheFolderName}\")
            With Cache
                .CacheDeleteError = CacheDeletionError(Cache)
                .DisposeSuspended = True
                .Validate()
            End With
        End Sub
#End Region
#Region "Download functions"
        Private Sub DownloadPre()
            If Media.Type = AllVid Then
                Dim r$ = Responser.GetResponse(Media.URL)
                If Not r.IsEmptyString Then
                    Dim s As List(Of Sizes) = RegexFields(Of Sizes)(r, {RParams.DM(R_VIDEO_REGEX_PATTERN, 0, RegexReturn.List, EDP.ReturnValue)}, {4, 6}, EDP.ReturnValue)
                    If s.ListExists Then
                        s.Sort()
                        RootPlaylistUrl = s(0).Data
                        s.Clear()
                    End If
                End If
            Else
                RootPlaylistUrl = Media.URL
            End If
        End Sub
        Private Sub Download()
            DownloadPre()
            If RootPlaylistUrl.IsEmptyString Then
                DestinationFile = Nothing
            Else
                Thrower.ThrowAny()
                Dim r$ = Responser.GetResponse(RootPlaylistUrl)
                If Not r.IsEmptyString Then
                    UrlVideo = RegexReplace(r, RParams.DMS(R_VIDEO_REGEX_PATTERN, 6, EDP.ReturnValue))
                    UrlAudio = RegexReplace(r, REGEX_AUDIO_URL)
                    If UrlVideo.IsEmptyString Then Throw New ArgumentException("Unable to identify m3u8 video track", "M3U8 video track")
                    Thrower.ThrowAny()
                    GetFiles(UrlVideo, FileVideo, False)
                    Thrower.ThrowAny()
                    If Not UrlAudio.IsEmptyString Then GetFiles(UrlAudio, FileAudio, True)
                    Thrower.ThrowAny()
                    MergeFiles()
                End If
            End If
        End Sub
        Private Sub GetFiles(ByVal URL As String, ByRef File As SFile, ByVal IsAudio As Boolean)
            Try
                Dim r$ = Responser.GetResponse(URL)
                If Not r.IsEmptyString Then
                    Dim data As List(Of RegexMatchStruct) = RegexFields(Of RegexMatchStruct)(r, {REGEX_PLS_FILES}, {1, 2}, EDP.ReturnValue)
                    If data.ListExists Then
                        File = $"{Cache.RootDirectory.PathWithSeparator}{IIf(IsAudio, "AUDIO.aac", "VIDEO.mp4")}"
                        Using b As New TokenBatch(Token) With {.Encoding = Settings.CMDEncoding, .MainProcessName = "ffmpeg"}
                            AddHandler b.ErrorDataReceived, AddressOf Batch_OutputDataReceived
                            ProgressChangeMax(data.Count)
                            b.ChangeDirectory(Cache.RootDirectory)
                            b.Execute($"""{Settings.FfmpegFile}"" -i {URL} -vcodec copy -strict -2 ""{File}""")
                            Token.ThrowIfCancellationRequested()
                            If Not File.Exists Then File = Nothing
                        End Using
                    End If
                End If
            Catch oex As OperationCanceledException
                Throw oex
            Catch dex As ObjectDisposedException
                Throw dex
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog + EDP.ThrowException, ex,
                                        $"API.JustForFans.M3U8.GetFiles({IIf(IsAudio, "audio", "video")}):{vbCr}URL: {URL}{vbCr}File: {File}")
            End Try
        End Sub
        Private Async Sub Batch_OutputDataReceived(ByVal Sender As Object, ByVal e As DataReceivedEventArgs)
            Await Task.Run(Sub() If Not e.Data.IsEmptyString AndAlso e.Data.Contains("] Opening") Then ProgressPerform())
        End Sub
        Private Sub MergeFiles()
            Try
                Dim p As SFileNumbers = SFileNumbers.Default(DestinationFile.Name)
                Dim f As SFile = SFile.IndexReindex(DestinationFile,,, p, EDP.ReturnValue).IfNullOrEmpty(DestinationFile)
                If Not FileVideo.IsEmptyString And Not FileAudio.IsEmptyString Then
                    DestinationFile = FFMPEG.MergeFiles({FileVideo, FileAudio}, Settings.FfmpegFile, f, Settings.CMDEncoding, p, EDP.ThrowException)
                Else
                    If Not SFile.Move(FileVideo, f) Then DestinationFile = FileVideo
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog + EDP.ThrowException, ex, $"[M3U8.MergeFiles]")
            End Try
        End Sub
#End Region
#Region "Progress support"
        Private Sub ProgressChangeMax(ByVal Count As Integer)
            If ProgressExists Then
                If UsePreProgress Then
                    ProgressPre.ChangeMax(Count)
                Else
                    Progress.Maximum += Count
                End If
            End If
        End Sub
        Private Sub ProgressPerform()
            If ProgressExists Then
                If UsePreProgress Then
                    ProgressPre.Perform()
                Else
                    Progress.Perform()
                End If
            End If
        End Sub
#End Region
#Region "Static Download"
        Friend Shared Function Download(ByVal Media As UserMedia, ByVal DestinationFile As SFile, ByVal Resp As Responser, ByVal Thrower As Plugin.IThrower,
                                        ByVal Progress As MyProgress, ByVal UsePreProgress As Boolean, ByVal _Token As CancellationToken) As SFile
            Using m As New M3U8(Media, DestinationFile, Resp, Thrower, Progress, UsePreProgress, _Token)
                m.Download()
                If m.DestinationFile.Exists Then Return m.DestinationFile Else Return Nothing
            End Using
        End Function
#End Region
#Region "IDisposable Support"
        Private disposedValue As Boolean = False
        Private Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    DataVideo.Clear()
                    DataAudio.Clear()
                    ProgressPre.DisposeIfReady
                    Cache.Dispose()
                    If ResponserInternal Then Responser.DisposeIfReady
                End If
                disposedValue = True
            End If
        End Sub
        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub
        Friend Overloads Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace