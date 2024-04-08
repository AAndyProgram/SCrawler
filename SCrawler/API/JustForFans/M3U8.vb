' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Net
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
        Private Structure M3U8URL_Indexed
            Friend Index As Integer
            Friend File As SFile
        End Structure
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
        Private FileVideo_M3U8 As SFile
        Private FileAudio_M3U8 As SFile
        Private ReadOnly FileVideo_IndexedParts As List(Of M3U8URL_Indexed)
        Private ReadOnly FileAudio_IndexedParts As List(Of M3U8URL_Indexed)
        Private RootPlaylistUrl As String
        Private ReadOnly Cache As CacheKeeper
        Private ReadOnly Progress As MyProgress
        Private ReadOnly ProgressPre As PreProgress
        Private ReadOnly ProgressExists As Boolean
        Private ReadOnly UsePreProgress As Boolean
        Private ReadOnly REGEX_FILE_EXT As RParams = RParams.DMS("[^\s""]+\.(\w+)([\?&]{1}.+|)", 1, EDP.ReturnValue)
        Private ReadOnly REGEX_FILE_EXT_M4S As RParams = RParams.DM("[^\s""]+\.m4s([\?&]{1}.+|)", 0, EDP.ReturnValue)
        Private ReadOnly MyFileNumberProvider As ANumbers
#End Region
#Region "Initializer"
        Private Sub New(ByVal m As UserMedia, ByVal Destination As SFile, ByVal Resp As Responser, ByVal _Thrower As Plugin.IThrower,
                        ByVal _Progress As MyProgress, ByVal _UsePreProgress As Boolean)
            Media = m
            DestinationFile = Destination
            Thrower = _Thrower
            'Responser = Resp
            Responser = New Responser
            ResponserInternal = True
            FileVideo_IndexedParts = New List(Of M3U8URL_Indexed)
            FileAudio_IndexedParts = New List(Of M3U8URL_Indexed)
            Progress = _Progress
            ProgressExists = Not Progress Is Nothing
            If ProgressExists Then ProgressPre = New PreProgress(Progress)
            UsePreProgress = _UsePreProgress
            MyFileNumberProvider = M3U8Base.NumberProviderDefault
            Cache = New CacheKeeper($"{DestinationFile.PathWithSeparator}_{M3U8Base.TempCacheFolderName}\") With {.DisposeSuspended = True}
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
                    GetFileParts(UrlVideo, FileVideo_M3U8, FileVideo_IndexedParts, False)
                    Thrower.ThrowAny()
                    If Not UrlAudio.IsEmptyString Then GetFileParts(UrlAudio, FileAudio_M3U8, FileAudio_IndexedParts, True)

                    If FileVideo_IndexedParts.Count > 0 Then _
                       FileVideo = GetTempFile(FileVideo_M3U8, FileVideo_IndexedParts, False, FileAudio_IndexedParts, FileAudio_IndexedParts.Count = 0)
                    If FileAudio_IndexedParts.Count > 0 Then _
                       FileAudio = GetTempFile(FileAudio_M3U8, FileAudio_IndexedParts, True, FileVideo_IndexedParts, False)
                    Thrower.ThrowAny()
                    MergeFiles()
                End If
            End If
        End Sub
        Private Function GetTempFile(ByVal M3U8File As SFile, ByVal IndexedList As List(Of M3U8URL_Indexed), ByVal IsAudio As Boolean,
                                     ByVal IndexedListOther As List(Of M3U8URL_Indexed), ByVal IgnoreAudio As Boolean) As SFile
            Const mapStr$ = "#EXT-X-MAP:URI"
            Const extinfStr$ = "#EXTINF:"
            Const m4s$ = "m4s"
            Dim M3U8FileLines$() = M3U8File.GetLines
            If M3U8FileLines.ListExists AndAlso IndexedList.Count > 0 AndAlso (IndexedListOther.Count > 0 Or (Not IsAudio And IgnoreAudio)) Then
                Dim outputFile As SFile = $"{Cache.RootDirectory.PathWithSeparator}{IIf(IsAudio, "AUDIO.aac", "VIDEO.mp4")}"
                Dim M3U8FileNew As SFile = M3U8File
                M3U8FileNew.Path = IndexedList(0).File.Path
                Dim v$
                Dim i%, fIndx%, fIndx2%
                Dim extIsm4s As Boolean
                Dim LookingIndex% = -1
                Dim ignoreOtherList As Boolean = IndexedListOther.Count = 0 And (Not IsAudio And IgnoreAudio)
                Dim fileFinder As Predicate(Of M3U8URL_Indexed) = Function(input) input.Index = LookingIndex

                Using m3u8Text As New TextSaver
                    For i = 0 To M3U8FileLines.Length - 1
                        v = M3U8FileLines(i)

                        If Not v.IsEmptyString Then
                            If v.StartsWith(mapStr) Then
                                LookingIndex += 1
                                fIndx = IndexedList.FindIndex(fileFinder)
                                If fIndx >= 0 Then
                                    extIsm4s = Not IndexedList(fIndx).File.Extension.IsEmptyString AndAlso IndexedList(fIndx).File.Extension = m4s
                                    v = v.Replace(RegexReplace(v, If(extIsm4s, REGEX_FILE_EXT_M4S, REGEX_FILE_EXT)), IndexedList(fIndx).File.File)
                                    m3u8Text.AppendLine(v)
                                Else
                                    Throw New Exception($"The map file is missing ({IIf(IsAudio, "audio", "video")})")
                                End If
                            ElseIf v.StartsWith(extinfStr) Then
                                LookingIndex += 1
                                If (i + 1) <= M3U8FileLines.Length - 1 Then
                                    fIndx = IndexedList.FindIndex(fileFinder)
                                    fIndx2 = If(ignoreOtherList, -1, IndexedListOther.FindIndex(fileFinder))
                                    If fIndx >= 0 And (fIndx2 >= 0 Or ignoreOtherList) Then
                                        If ignoreOtherList OrElse IndexedListOther(fIndx2).Index = IndexedList(fIndx).Index Then
                                            m3u8Text.AppendLine(v)
                                            m3u8Text.AppendLine(IndexedList(fIndx).File.File)
                                        End If
                                    End If
                                    i += 1
                                Else
                                    Throw New Exception($"Unexpected end of m3u8 file ({IIf(IsAudio, "audio", "video")})")
                                End If
                            Else
                                m3u8Text.AppendLine(v)
                            End If
                        End If
                    Next

                    m3u8Text.SaveAs(M3U8FileNew)
                End Using

                If M3U8FileNew.Exists Then
                    Using b As New BatchExecutor
                        AddHandler b.ErrorDataReceived, AddressOf Batch_OutputDataReceived
                        Thrower.ThrowAny()
                        ProgressChangeMax(IndexedList.Count)
                        b.ChangeDirectory(M3U8FileNew)
                        b.Execute($"""{Settings.FfmpegFile}"" -i {M3U8FileNew.File} -vcodec copy -strict -2 ""{outputFile}""")
                    End Using
                    If Not outputFile.Exists Then outputFile = Nothing
                End If

                Return outputFile
            Else
                Return Nothing
            End If
        End Function
        Private Sub GetFileParts(ByVal URL As String, ByRef M3U8File As SFile, ByRef IndexedList As List(Of M3U8URL_Indexed), ByVal IsAudio As Boolean)
            Try
                Dim r$ = Responser.GetResponse(URL)
                If Not r.IsEmptyString Then
                    Dim data As List(Of RegexMatchStruct) = RegexFields(Of RegexMatchStruct)(r, {REGEX_PLS_FILES}, {1, 2}, EDP.ReturnValue)
                    If data.ListExists Then
                        Dim appender$ = URL.Replace(URL.Split("/").LastOrDefault, String.Empty)
                        Dim createM3U8URL As Func(Of String, M3U8URL) =
                            Function(input) New M3U8URL(M3U8Base.CreateUrl(appender, input), RegexReplace(input, REGEX_FILE_EXT))
                        With (From d As RegexMatchStruct In data
                              Where Not d.Arr(0).IfNullOrEmpty(d.Arr(1)).IsEmptyString
                              Select createM3U8URL.Invoke(d.Arr(0).IfNullOrEmpty(d.Arr(1)).StringTrim))
                            If .ListExists Then
                                ProgressChangeMax(.Count)
                                M3U8File = $"{Cache.RootDirectory.PathWithSeparator}{IIf(IsAudio, "AUDIO", "VIDEO")}.m3u8"
                                M3U8File = TextSaver.SaveTextToFile(r, M3U8File, True)

                                Dim tmpCache As CacheKeeper = Cache.NewInstance
                                Dim dFile As SFile = tmpCache.RootDirectory
                                dFile.Extension = .ElementAt(0).Extension.IfNullOrEmpty("m4s")
                                MyFileNumberProvider.GroupSize = { .Count.ToString.Length, 3}.Max
                                If tmpCache.Validate Then
                                    Using w As New WebClient
                                        For i% = 0 To .Count - 1
                                            Thrower.ThrowAny()
                                            dFile.Name = $"{M3U8Base.TempFilePrefix}{i.NumToString(MyFileNumberProvider)}"
                                            dFile.Extension = .ElementAt(i).Extension.IfNullOrEmpty(M3U8Base.TempFileDefaultExtension)
                                            Try
                                                ProgressPerform()
                                                w.DownloadFile(.ElementAt(i).URL, dFile)
                                                tmpCache.AddFile(dFile, True)
                                                IndexedList.Add(New M3U8URL_Indexed With {.File = dFile, .Index = i})
                                            Catch down_oex As OperationCanceledException
                                                Throw down_oex
                                            Catch down_dex As ObjectDisposedException
                                                Throw down_dex
                                            Catch ex As Exception
                                            End Try
                                        Next
                                    End Using
                                Else
                                    Throw New Exception("Can't create cache directory")
                                End If
                            End If
                        End With
                    End If
                End If
            Catch oex As OperationCanceledException
                Throw oex
            Catch dex As ObjectDisposedException
                Throw dex
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog + EDP.ThrowException, ex,
                                        $"API.JustForFans.M3U8.GetFileParts({IIf(IsAudio, "audio", "video")}):{vbCr}URL: {URL}{vbCr}Post: {Media.URL_BASE}")
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
                ElseIf FileVideo.Exists Then
                    If Not SFile.Move(FileVideo, f) Then DestinationFile = FileVideo
                Else
                    Throw New Exception($"Unable to download file ({Media.URL_BASE})")
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
                                        ByVal Progress As MyProgress, ByVal UsePreProgress As Boolean) As SFile
            Using m As New M3U8(Media, DestinationFile, Resp, Thrower, Progress, UsePreProgress)
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
                    FileVideo_IndexedParts.Clear()
                    FileAudio_IndexedParts.Clear()
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