' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Net
Imports System.Threading
Imports SCrawler.API.Base
Imports SCrawler.API.Reddit.M3U8_Declarations
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.Web
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Reddit
    Namespace M3U8_Declarations
        Friend Module M3U8_Declarations
            Friend ReadOnly BaseUrlPattern As RParams = RParams.DM("([htps:/]{7,8}.+?/.+?)(?=/)", 0, EDP.ReturnValue)
            ''' <summary>Video</summary>
            Friend ReadOnly PlayListRegEx_1 As RParams = RParams.DM("(#EXT-X-STREAM-INF)(.+)(RESOLUTION=)(\d+)(.+?[\r\n]{1,2})(.+?)([\r\n]{1,2})", 0, RegexReturn.List)
            ''' <summary>Audio, Video</summary>
            Friend ReadOnly PlayListRegEx_2 As RParams = RParams.DM("(?<=#EXT-X-BYTERANGE.+?[\r\n]{1,2})(.+)(?=[\r\n]{0,2})", 0, RegexReturn.List)
            Friend ReadOnly PlayListAudioRegEx As RParams = RParams.DM("(HLS_AUDIO_(\d+)[^""]+)", 0, RegexReturn.List)
            Friend ReadOnly DPED As New ErrorsDescriber(EDP.SendToLog + EDP.ReturnValue)
        End Module
    End Namespace
    Friend NotInheritable Class M3U8 : Implements IDisposable
#Region "Declarations"
        Private Enum Types : Video : Audio : End Enum
        Private Structure Resolution : Implements IRegExCreator, IComparable(Of Resolution)
            Friend File As String
            Friend Resolution As Integer
            Friend HasError As Boolean
            Friend Function CreateFromArray(ByVal ParamsArray() As String) As Object Implements IRegExCreator.CreateFromArray
                If ParamsArray.ArrayExists Then
                    File = ParamsArray(0)
                    Try
                        If ParamsArray.Length > 1 Then Resolution = AConvert(Of Integer)(ParamsArray(1), EDP.ThrowException)
                    Catch ex As Exception
                        HasError = True
                        Resolution = 0
                    End Try
                End If
                Return Me
            End Function
            Friend Function CompareTo(ByVal Other As Resolution) As Integer Implements IComparable(Of Resolution).CompareTo
                Return Resolution.CompareTo(Other.Resolution) * -1
            End Function
        End Structure
        Private ReadOnly PlayListURL As String
        Private ReadOnly BaseURL As String
        Private ReadOnly Video As List(Of String)
        Private ReadOnly Audio As List(Of String)
        Private OutFile As SFile
        Private VideoFile As SFile
        Private AudioFile As SFile
        Private ReadOnly Cache As CacheKeeper
        Private ReadOnly CacheFiles As CacheKeeper
        Private ReadOnly Property Progress As MyProgress
        Private ReadOnly ProgressExists As Boolean
        Private ReadOnly Property ProgressPre As PreProgress
        Private ReadOnly UsePreProgress As Boolean
        Private ReadOnly Media As UserMedia
#End Region
        Private Sub New(ByVal URL As String, ByVal Media As UserMedia, ByVal OutFile As SFile, ByVal Progress As MyProgress, ByVal UsePreProgress As Boolean)
            PlayListURL = URL
            Me.Media = Media
            BaseURL = RegexReplace(URL, BaseUrlPattern)
            Video = New List(Of String)
            Audio = New List(Of String)
            Me.OutFile = OutFile
            Me.OutFile.Name = "PlayListFile"
            Me.OutFile.Extension = "mp4"
            If Media.Post.Date.HasValue Then Me.OutFile.Name = Media.Post.Date.Value.ToString("yyyyMMdd_HHmmss")
            Me.Progress = Progress
            ProgressExists = Not Me.Progress Is Nothing
            ProgressPre = New PreProgress(Progress)
            Me.UsePreProgress = UsePreProgress
            Cache = New CacheKeeper($"{OutFile.PathWithSeparator}_{Base.M3U8Base.TempCacheFolderName}\")
            Cache.CacheDeleteError = Base.CacheDeletionError(Cache)
            CacheFiles = Cache.NewInstance
        End Sub
#Region "Internal functions"
#Region "GetPlaylistUrls"
        Private Overloads Sub GetPlaylistUrls()
            Video.ListAddList(GetPlaylistUrls(PlayListURL, Types.Video))
            Audio.ListAddList(GetPlaylistUrls(PlayListURL, Types.Audio))
        End Sub
        Private Overloads Function GetPlaylistUrls(ByVal PlayListURL As String, ByVal Type As Types) As List(Of String)
            Try
                If Not BaseURL.IsEmptyString Then
                    Using w As New WebClient
                        Dim r$ = w.DownloadString(PlayListURL)
                        If Not r.IsEmptyString Then
                            Dim l As New List(Of Resolution)
                            If Type = Types.Video Then
                                l = RegexFields(Of Resolution)(r, {PlayListRegEx_1}, {6, 4}, EDP.ReturnValue)
                            Else
                                Try
                                    l = RegexFields(Of Resolution)(r, {PlayListAudioRegEx}, {1, 2})
                                Catch anull As RegexFieldsTextBecameNullException
                                    l.Clear()
                                End Try
                            End If
                            If l.ListExists Then
                                Dim plError As Predicate(Of Resolution) = Function(lr) lr.HasError
                                If l.Exists(plError) Then
                                    l.RemoveAll(plError)
                                    If l.Count = 0 Then Return New List(Of String)
                                End If
                                l.Sort()
                                Dim pls$ = $"{BaseURL}/{l.First.File}"
                                r = w.DownloadString(pls)
                                If Not r.IsEmptyString Then
                                    Dim lp As New ListAddParams(LAP.NotContainsOnly) With {
                                        .Converter = Function(input) $"{BaseURL}/{input}",
                                        .Error = New ErrorsDescriber(False, False, True, New List(Of String))}
                                    Return ListAddList(Of String, List(Of String))(Nothing, DirectCast(RegexReplace(r, PlayListRegEx_2), List(Of String)), lp).ListIfNothing
                                End If
                            End If
                        End If
                    End Using
                End If
                Return New List(Of String)
            Catch ex As Exception
                Return ErrorsDescriber.Execute(DPED, ex, $"[M3U8.GetPlaylistUrls({Type}): {PlayListURL}]", New List(Of String))
            End Try
        End Function
#End Region
#Region "ConcatData"
        Private Overloads Sub ConcatData(ByVal Token As CancellationToken)
            ConcatData(Video, Types.Video, VideoFile, Token)
            ConcatData(Audio, Types.Audio, AudioFile, Token)
            MergeFiles()
        End Sub
        Private Overloads Sub ConcatData(ByVal Urls As List(Of String), ByVal Type As Types, ByRef TFile As SFile, ByVal Token As CancellationToken)
            Try
                Token.ThrowIfCancellationRequested()
                If Urls.ListExists Then
                    Dim tmpCache As CacheKeeper = CacheFiles.NewInstance
                    Dim ConcatFile As SFile = CacheFiles
                    If Type = Types.Audio Then
                        ConcatFile.Name &= "AUDIO"
                        ConcatFile.Extension = "aac"
                    Else
                        If Audio.Count > 0 Then ConcatFile.Name &= "VIDEO"
                        ConcatFile.Extension = "mp4"
                    End If
                    If tmpCache.Validate Then
                        Dim i%
                        Dim dFile As SFile = tmpCache.RootDirectory
                        If ProgressExists Then
                            If UsePreProgress Then
                                ProgressPre.ChangeMax(Urls.Count)
                            Else
                                Progress.Maximum += Urls.Count
                            End If
                        End If
                        dFile.Extension = New SFile(Urls(0)).Extension
                        If dFile.Extension.IsEmptyString Then dFile.Extension = "ts"
                        Using w As New WebClient
                            For i = 0 To Urls.Count - 1
                                If ProgressExists Then
                                    If UsePreProgress Then
                                        ProgressPre.Perform()
                                    Else
                                        Progress.Perform()
                                    End If
                                End If
                                Token.ThrowIfCancellationRequested()
                                dFile.Name = $"ConPart_{i}"
                                w.DownloadFile(Urls(i), dFile)
                                tmpCache.AddFile(dFile, True)
                            Next
                        End Using
                        TFile = FFMPEG.ConcatenateFiles(tmpCache, Settings.FfmpegFile.File, ConcatFile, Settings.CMDEncoding,, DPED)
                    End If
                End If
            Catch oex As OperationCanceledException When Token.IsCancellationRequested
                Throw oex
            Catch ex As Exception
                ErrorsDescriber.Execute(DPED, ex, $"[M3U8.Save({Type})]")
            End Try
        End Sub
#End Region
        Private Sub MergeFiles()
            Try
                Dim p As SFileNumbers = SFileNumbers.Default(OutFile.Name)
                Dim f As SFile = SFile.IndexReindex(OutFile,,, p, EDP.ReturnValue)
                If Not VideoFile.IsEmptyString And Not AudioFile.IsEmptyString Then
                    OutFile = FFMPEG.MergeFiles({VideoFile, AudioFile}, Settings.FfmpegFile.File, f, Settings.CMDEncoding, p, DPED)
                Else
                    If f.IsEmptyString Then f = OutFile
                    If Not SFile.Move(VideoFile, f) Then OutFile = VideoFile
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(DPED, ex, $"[M3U8.MergeFiles]")
            End Try
        End Sub
        Friend Function Download(ByVal Token As CancellationToken) As SFile
            GetPlaylistUrls()
            ConcatData(Token)
            Return OutFile
        End Function
#End Region
#Region "Statics"
        Friend Shared Function Download(ByVal URL As String, ByVal Media As UserMedia, ByVal f As SFile, ByVal Token As CancellationToken,
                                        ByVal Progress As MyProgress, ByVal UsePreProgress As Boolean) As SFile
            Using m As New M3U8(URL, Media, f, Progress, UsePreProgress) : Return m.Download(Token) : End Using
        End Function
#End Region
#Region "IDisposable Support"
        Private disposedValue As Boolean = False
        Private Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Video.Clear()
                    Audio.Clear()
                    Cache.Dispose()
                    ProgressPre.Dispose()
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