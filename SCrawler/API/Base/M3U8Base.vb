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
    Friend Structure M3U8URL
        Friend URL As String
        Friend Extension As String
        Friend Sub New(ByVal _URL As String, Optional ByVal _Extension As String = Nothing)
            URL = _URL
            Extension = _Extension
        End Sub
        Public Shared Widening Operator CType(ByVal URL As String) As M3U8URL
            Return New M3U8URL(URL)
        End Operator
        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            If Not IsNothing(Obj) Then
                If TypeOf Obj Is M3U8URL Then
                    Return CType(Obj, M3U8URL).URL = URL
                Else
                    Return CStr(Obj) = URL
                End If
            End If
            Return False
        End Function
    End Structure
    Friend NotInheritable Class M3U8Base
        Friend Const TempCacheFolderName As String = "tmpCache"
        Friend Const TempFilePrefix As String = "ConPart_"
        Friend Const TempFileDefaultExtension As String = "ts"
        ''' <summary><c>SFileNumbers.NumberProviderDefault</c></summary>
        Friend Shared ReadOnly Property NumberProviderDefault As ANumbers
            Get
                Return SFileNumbers.NumberProviderDefault
            End Get
        End Property
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
        Friend Overloads Shared Function Download(ByVal URLs As List(Of String), ByVal DestinationFile As SFile, Optional ByVal Responser As Responser = Nothing,
                                                  Optional ByVal Token As CancellationToken = Nothing, Optional ByVal Progress As MyProgress = Nothing,
                                                  Optional ByVal UsePreProgress As Boolean = True, Optional ByVal ExistingCache As CacheKeeper = Nothing,
                                                  Optional ByVal OnlyDownload As Boolean = False) As SFile
            Return Download(URLs.ListCast(Of M3U8URL), DestinationFile, Responser, Token, Progress, UsePreProgress, ExistingCache, OnlyDownload)
        End Function
        Friend Overloads Shared Function Download(ByVal URLs As List(Of M3U8URL), ByVal DestinationFile As SFile, Optional ByVal Responser As Responser = Nothing,
                                                  Optional ByVal Token As CancellationToken = Nothing, Optional ByVal Progress As MyProgress = Nothing,
                                                  Optional ByVal UsePreProgress As Boolean = True, Optional ByVal ExistingCache As CacheKeeper = Nothing,
                                                  Optional ByVal OnlyDownload As Boolean = False, Optional ByVal SkipBroken As Boolean = False) As SFile
            Dim Cache As CacheKeeper = Nothing
            Using tmpPr As New PreProgress(Progress)
                Try
                    If URLs.ListExists Then
                        Dim ConcatFile As SFile = DestinationFile
                        If ConcatFile.Name.IsEmptyString Then ConcatFile.Name = "PlayListFile"
                        ConcatFile.Extension = "mp4"
                        If ExistingCache Is Nothing Then
                            Cache = New CacheKeeper($"{DestinationFile.PathWithSeparator}_{TempCacheFolderName}\")
                            Cache.CacheDeleteError = CacheDeletionError(Cache)
                        Else
                            Cache = ExistingCache
                        End If
                        Dim cache2 As CacheKeeper = Cache.NewInstance
                        If cache2.RootDirectory.Exists(SFO.Path) Then
                            Dim progressExists As Boolean = Not Progress Is Nothing
                            If progressExists Then
                                If UsePreProgress Then
                                    tmpPr.ChangeMax(URLs.Count)
                                Else
                                    Progress.Maximum += URLs.Count
                                End If
                            End If
                            Dim p As SFileNumbers = SFileNumbers.Default(ConcatFile.Name)
                            Dim pNum As ANumbers = NumberProviderDefault
                            p.NumberProvider = pNum
                            DirectCast(p.NumberProvider, ANumbers).GroupSize = {URLs.Count.ToString.Length, 3}.Max
                            ConcatFile = SFile.IndexReindex(ConcatFile,,, p, EDP.ReturnValue)
                            Dim i%
                            Dim dFile As SFile = cache2.RootDirectory
                            dFile.Extension = TempFileDefaultExtension
                            Using w As New DownloadObjects.WebClient2(Responser)
                                For i = 0 To URLs.Count - 1
                                    If progressExists Then
                                        If UsePreProgress Then
                                            tmpPr.Perform()
                                        Else
                                            Progress.Perform()
                                        End If
                                    End If
                                    Token.ThrowIfCancellationRequested()
                                    dFile.Name = $"{TempFilePrefix}{i.NumToString(pNum)}"
                                    dFile.Extension = URLs(i).Extension.IfNullOrEmpty(TempFileDefaultExtension)
                                    Try
                                        w.DownloadFile(URLs(i).URL, dFile)
                                        cache2.AddFile(dFile, True)
                                    Catch ex As Exception
                                        If Not SkipBroken Then Throw ex
                                    End Try
                                Next
                            End Using
                            If Not OnlyDownload Then _
                               DestinationFile = FFMPEG.ConcatenateFiles(cache2, Settings.FfmpegFile.File, ConcatFile, Settings.CMDEncoding, p, EDP.ThrowException)
                            Return DestinationFile
                        End If
                    End If
                    Return Nothing
                Finally
                    Cache.DisposeIfReady
                End Try
            End Using
        End Function
    End Class
End Namespace