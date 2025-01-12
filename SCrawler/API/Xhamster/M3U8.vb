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
Imports SCrawler.API.Base.M3U8Declarations
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Xhamster
    Friend NotInheritable Class M3U8
        Private Sub New()
        End Sub
        Private Shared Function ParseFirstM3U8(ByVal URL As String, ByVal Responser As Responser, ByVal UHD As Boolean) As String
            Dim r$, d$
            Dim _DataObtained As Boolean = False
            For i% = 0 To 1
                Try
                    Responser.UseGZipStream = i
                    r = Responser.GetResponse(URL.Replace("\", String.Empty))
                    If Not r.IsEmptyString Then
                        r = r.StringFormatLines
                        Dim sList As List(Of Sizes) = RegexFields(Of Sizes)(r, {FirstM3U8FileRegEx}, {1, 2})
                        If sList.ListExists Then _DataObtained = True : sList.RemoveAll(Function(sv) sv.HasError Or sv.Data.IsEmptyString Or
                                                                                                     sv.Value = 0 Or (Not UHD And sv.Value > 1080))
                        If sList.ListExists Then
                            sList.Sort()
                            d = sList.First.Data.Trim
                            If Not d.IsEmptyString Then Return d
                        End If
                    End If
                Catch
                End Try
                If _DataObtained Then Exit For
            Next
            Return String.Empty
        End Function
        Private Shared Function ParseSecondM3U8(ByVal URL As String, ByVal Responser As Responser, ByVal Appender As String) As List(Of M3U8URL)
            Dim r$
            Dim l As List(Of String)
            Dim ll As List(Of M3U8URL) = Nothing
            Dim u As M3U8URL
            Dim rmsF As Func(Of RegexMatchStruct, M3U8URL) =
                Function(ByVal rms As RegexMatchStruct) As M3U8URL
                    With rms
                        If .Arr(0).IsEmptyString Then
                            Return New M3U8URL(.Arr(3).IfNullOrEmpty(.Arr(4)), .Arr(5).IfNullOrEmpty(.Arr(6)))
                        Else
                            Return New M3U8URL(.Arr(0), .Arr(1).IfNullOrEmpty(.Arr(2)))
                        End If
                    End With
                End Function
            For i% = 0 To 1
                Try
                    Responser.UseGZipStream = i
                    r = Responser.GetResponse(URL)
                    If Not r.IsEmptyString Then
                        l = RegexReplace(r, TsFilesRegEx)
                        If Not l.ListExists Then
                            With RegexFields(Of RegexMatchStruct)(r, {SecondM3U8FileRegEx}, {2, 4, 3, 8, 7, 10, 9}, EDP.ReturnValue)
                                If .ListExists Then ll = .Select(rmsF).ListWithRemove(Function(v) v.URL.IsEmptyString)
                            End With
                        End If
                        If Not ll.ListExists And l.ListExists Then ll = l.ListCast(Of M3U8URL)
                        If ll.ListExists Then
                            For indx% = 0 To ll.Count - 1
                                u = ll(indx)
                                u.URL = M3U8Base.CreateUrl(Appender, u.URL)
                                ll(indx) = u
                            Next
                            Return ll
                        End If
                    End If
                Catch
                End Try
            Next
            Return Nothing
        End Function
        Private Shared Function ObtainUrls(ByVal URL As String, ByVal Responser As Responser, ByVal UHD As Boolean) As List(Of M3U8URL)
            Try
                Const sk$ = "/key="
                Dim file$ = ParseFirstM3U8(URL, Responser, UHD)
                If Not file.IsEmptyString Then
                    Responser.UseGZipStream = False
                    Dim appender$ = URL.Replace(URL.Split("/").LastOrDefault, String.Empty)
                    If file.StartsWith(sk) Then
                        Dim position% = InStr(URL, sk)
                        If position > 0 Then appender = URL.Remove(position - 1)
                    End If
                    If file.StartsWith("//") Then
                        URL = LinkFormatterSecure(file.TrimStart("/"))
                    Else
                        URL = M3U8Base.CreateUrl(appender, file)
                    End If
                    Dim l As List(Of M3U8URL) = ParseSecondM3U8(URL, Responser, appender)
                    If l.ListExists Then Return l
                End If
                Return Nothing
            Finally
                Responser.UseGZipStream = False
            End Try
        End Function
        Friend Shared Function Download(ByVal Media As UserMedia, ByVal Responser As Responser, ByVal UHD As Boolean,
                                        ByVal Token As CancellationToken, ByVal Progress As MyProgress, ByVal UsePreProgress As Boolean,
                                        ByVal ReencodeVideos As Boolean) As SFile
            'Return M3U8Base.Download(ObtainUrls(Media.URL, Responser, UHD), Media.File, Responser, Token, Progress, UsePreProgress)
            Dim Cache As CacheKeeper = Nothing
            Try
                Dim urls As List(Of M3U8URL) = ObtainUrls(Media.URL, Responser, UHD)
                If urls.ListExists Then
                    Cache = New CacheKeeper($"{Media.File.PathWithSeparator}_{M3U8Base.TempCacheFolderName}\") With {.DisposeSuspended = True}
                    Cache.CacheDeleteError = CacheDeletionError(Cache)

                    Dim isNewWay As Boolean = Not urls(0).Extension.IsEmptyString AndAlso urls(0).Extension = "mp4" AndAlso urls.Count > 1 AndAlso
                                              urls.Exists(Function(u) Not u.Extension = urls(0).Extension)

                    Dim f As SFile = M3U8Base.Download(urls, Media.File, Responser, Token, Progress, UsePreProgress, Cache, isNewWay)

                    If isNewWay Then
                        f = Media.File
                        With DirectCast(Cache.CurrentInstance, CacheKeeper)
                            If .Count > 0 Then
                                Using batch As New BatchExecutor With {.Encoding = Settings.CMDEncoding}
                                    batch.ChangeDirectory(.Self.RootDirectory)
                                    Using bat As New TextSaver($"{ .RootDirectory.PathWithSeparator}Merge.bat")
                                        Dim tmpFile As SFile
                                        Dim tmpFileStr$
                                        If ReencodeVideos Then
                                            tmpFile = $"{ .Self.RootDirectory.PathWithSeparator}NewVideo.{urls(1).Extension}"
                                            tmpFileStr = tmpFile.File
                                            .AddFile(tmpFile)
                                        Else
                                            tmpFile = Media.File
                                            tmpFileStr = $"""{tmpFile}"""
                                        End If

                                        bat.AppendLine($"copy /b { .First.File} + {M3U8Base.TempFilePrefix}*.{urls(1).Extension} {tmpFileStr}")
                                        If ReencodeVideos Then bat.AppendLine($"""{Settings.FfmpegFile}"" -i ""{tmpFile}"" ""{Media.File}""")
                                        bat.Save()
                                        .AddFile(bat.File)
                                        batch.Execute($"""{bat.File}""")
                                        If f.Exists Then Return f
                                    End Using
                                End Using
                            End If
                        End With
                    ElseIf f.Exists Then
                        Return f
                    End If
                End If
                Return Nothing
            Finally
                Cache.DisposeIfReady(False)
            End Try
        End Function
    End Class
End Namespace