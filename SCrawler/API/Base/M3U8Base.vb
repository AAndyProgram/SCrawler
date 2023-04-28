' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.Web
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Base
    Namespace M3U8Declarations
        Friend Module M3U8Defaults
            Friend ReadOnly TsFilesRegEx As RParams = RParams.DM(".+?\.ts[^\r\n]*", 0, RegexReturn.List)
        End Module
    End Namespace
    Friend NotInheritable Class M3U8Base
        Friend Const TempCacheFolderName As String = "tmpCache"
        Private Sub New()
        End Sub
        Friend Shared Function CreateUrl(ByVal Appender As String, ByVal File As String) As String
            File = File.StringTrimStart("/")
            If File.StartsWith("http") Then
                Return File
            Else
                If File.StartsWith("hls/") And Appender.Contains("hls/") Then _
                   Appender = LinkFormatterSecure(Appender.Replace("https://", String.Empty).Split("/").First)
                Return $"{Appender.StringTrimEnd("/")}/{File}"
            End If
        End Function
        Friend Shared Function Download(ByVal URLs As List(Of String), ByVal DestinationFile As SFile, Optional ByVal Responser As Responser = Nothing,
                                        Optional ByVal Token As CancellationToken = Nothing, Optional ByVal Progress As MyProgress = Nothing) As SFile
            Dim Cache As CacheKeeper = Nothing
            Try
                If URLs.ListExists Then
                    Dim ConcatFile As SFile = DestinationFile
                    If ConcatFile.Name.IsEmptyString Then ConcatFile.Name = "PlayListFile"
                    ConcatFile.Extension = "mp4"
                    Cache = New CacheKeeper($"{DestinationFile.PathWithSeparator}_{TempCacheFolderName}\")
                    Dim cache2 As CacheKeeper = Cache.NewInstance
                    If cache2.RootDirectory.Exists(SFO.Path) Then
                        Dim progressExists As Boolean = Not Progress Is Nothing
                        If progressExists Then Progress.Maximum += URLs.Count
                        Dim p As SFileNumbers = SFileNumbers.Default(ConcatFile.Name)
                        ConcatFile = SFile.IndexReindex(ConcatFile,,, p, EDP.ReturnValue)
                        Dim i%
                        Dim dFile As SFile = cache2.RootDirectory
                        dFile.Extension = "ts"
                        Using w As New DownloadObjects.WebClient2(Responser)
                            For i = 0 To URLs.Count - 1
                                If progressExists Then Progress.Perform()
                                Token.ThrowIfCancellationRequested()
                                dFile.Name = $"ConPart_{i}"
                                w.DownloadFile(URLs(i), dFile)
                                cache2.AddFile(dFile, True)
                            Next
                        End Using
                        DestinationFile = FFMPEG.ConcatenateFiles(cache2, Settings.FfmpegFile.File, ConcatFile, Settings.CMDEncoding, p, EDP.ThrowException)
                        Return DestinationFile
                    End If
                End If
                Return Nothing
            Finally
                Cache.DisposeIfReady
            End Try
        End Function
    End Class
End Namespace