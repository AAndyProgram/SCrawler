' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Net
Imports SCrawler.API.Reddit.M3U8_Declarations
Imports PersonalUtilities.Tools.Web
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
            Friend ReadOnly DPED As New ErrorsDescriber(EDP.SendInLog + EDP.ReturnValue)
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
        Private CachePath As SFile
#End Region
        Private Sub New(ByVal URL As String, ByVal OutFile As SFile)
            PlayListURL = URL
            BaseURL = RegexReplace(URL, BaseUrlPattern)
            Video = New List(Of String)
            Audio = New List(Of String)
            Me.OutFile = OutFile
            Me.OutFile.Name = "PlayListFile"
            Me.OutFile.Extension = "mp4"
            CachePath = $"{OutFile.PathWithSeparator}_Cache\{SFile.GetDirectories($"{OutFile.PathWithSeparator}_Cache\",,, EDP.ReturnValue).ListIfNothing.Count + 1}\"
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
                                l = RegexFields(Of Resolution)(r, {PlayListRegEx_1}, {6, 4})
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
        Private Overloads Sub ConcatData()
            ConcatData(Video, Types.Video, VideoFile)
            ConcatData(Audio, Types.Audio, AudioFile)
            MergeFiles()
        End Sub
        Private Overloads Sub ConcatData(ByVal Urls As List(Of String), ByVal Type As Types, ByRef TFile As SFile)
            Try
                If Urls.ListExists Then
                    Dim ConcatFile As SFile = OutFile
                    If Type = Types.Audio Then
                        ConcatFile.Name &= "_AUDIO"
                        ConcatFile.Extension = "aac"
                    Else
                        If Audio.Count > 0 Then ConcatFile.Name &= "_VIDEO"
                        ConcatFile.Extension = "mp4"
                    End If
                    If CachePath.Exists(SFO.Path) Then
                        Dim p As New SFileNumbers(ConcatFile.Name,,, New ANumbers With {.Format = ANumbers.Formats.General})
                        ConcatFile = SFile.Indexed_IndexFile(ConcatFile,, p, EDP.ThrowException)
                        Dim i%
                        Dim eFiles As New List(Of SFile)
                        Dim dFile As SFile = CachePath
                        dFile.Extension = New SFile(Urls(0)).Extension
                        If dFile.Extension.IsEmptyString Then dFile.Extension = "ts"
                        Using w As New WebClient
                            For i = 0 To Urls.Count - 1
                                dFile.Name = $"ConPart_{i}"
                                w.DownloadFile(Urls(i), dFile)
                                eFiles.Add(dFile)
                            Next
                        End Using
                        TFile = FFMPEG.ConcatenateFiles(eFiles, Settings.FfmpegFile, ConcatFile, p, DPED)
                        eFiles.Clear()
                    End If
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(DPED, ex, $"[M3U8.Save({Type})]")
            End Try
        End Sub
#End Region
        Private Sub MergeFiles()
            Try
                If Not VideoFile.IsEmptyString And Not AudioFile.IsEmptyString Then
                    Dim p As New SFileNumbers(OutFile.Name,, RParams.DMS("PlayListFile_(\d*)", 1), New ANumbers With {.Format = ANumbers.Formats.General})
                    OutFile = FFMPEG.MergeFiles({VideoFile, AudioFile}, Settings.FfmpegFile, OutFile, p, DPED)
                Else
                    OutFile = VideoFile
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(DPED, ex, $"[M3U8.MergeFiles]")
            End Try
        End Sub
        Friend Function Download() As SFile
            GetPlaylistUrls()
            ConcatData()
            Return OutFile
        End Function
#End Region
#Region "Statics"
        Friend Shared Function Download(ByVal URL As String, ByVal f As SFile) As SFile
            Using m As New M3U8(URL, f) : Return m.Download() : End Using
        End Function
#End Region
#Region "IDisposable Support"
        Private disposedValue As Boolean = False
        Private Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Video.Clear()
                    Audio.Clear()
                    CachePath.Delete(SFO.Path, SFODelete.None, DPED)
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