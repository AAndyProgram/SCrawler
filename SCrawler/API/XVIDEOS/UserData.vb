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
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.WEB
Imports PersonalUtilities.Tools.WebDocuments.JSON
Imports UStates = SCrawler.API.Base.UserMedia.States
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace API.XVIDEOS
    Friend Class UserData : Inherits UserDataBase
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return DirectCast(HOST.Source, SiteSettings)
            End Get
        End Property
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
        End Sub
        Friend Sub New()
            SeparateVideoFolder = False
            UseInternalM3U8Function = True
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                If Not Settings.UseM3U8 Then
                    If Not Settings.OS64 Then
                        MyMainLOG = $"XVIDEOS [{ToStringForLog()}]: The plugin only works with x64 OS."
                    Else
                        MyMainLOG = $"{ToStringForLog()}: File [ffmpeg.exe] not found"
                    End If
                    Exit Sub
                End If

                Dim NextPage% = 0
                Dim r$
                Dim jj As EContainer
                Dim e As ErrorsDescriber = EDP.ThrowException
                Dim user$ = MySettings.GetUserUrl(Name, False)
                Dim p As UserMedia
                Dim EnvirSet As Boolean = False

                Do
                    ThrowAny(Token)
                    URL = $"https://www.xvideos.com/{user}/videos/new/{If(NextPage = 0, String.Empty, NextPage)}"
                    r = Responser.GetResponse(URL,, e)
                    If Not r.IsEmptyString Then
                        If Not EnvirSet Then UserExists = True : UserSuspended = False : EnvirSet = True
                        With JsonDocument.Parse(r).XmlIfNothing
                            If .Contains("videos") Then
                                With .Item("videos")
                                    If .Count > 0 Then
                                        NextPage += 1
                                        For Each jj In .Self
                                            p = New UserMedia With {
                                                .Post = New UserPost With {.ID = jj.Value("id")},
                                                .URL = $"https://www.xvideos.com{jj.Value("u")}"
                                            }
                                            If Not p.Post.ID.IsEmptyString And Not jj.Value("u").IsEmptyString Then
                                                If Not _TempPostsList.Contains(p.Post.ID) Then
                                                    _TempPostsList.Add(p.Post.ID)
                                                    _TempMediaList.Add(p)
                                                Else
                                                    .Dispose()
                                                    Exit Do
                                                End If
                                            End If
                                        Next
                                    Else
                                        .Dispose()
                                        Exit Do
                                    End If
                                End With
                            Else
                                .Dispose()
                                Exit Do
                            End If
                            .Dispose()
                        End With
                    Else
                        Exit Do
                    End If
                Loop

                If _TempMediaList.Count > 0 Then
                    For i% = 0 To _TempMediaList.Count - 1
                        ThrowAny(Token)
                        With _TempMediaList(i) : _TempMediaList(i) = GetVideoData(.URL, Responser, MySettings.DownloadUHD.Value, .Post.ID) : End With
                    Next
                    _TempMediaList.RemoveAll(Function(m) m.URL.IsEmptyString)
                End If
            Catch oex As OperationCanceledException
            Catch dex As ObjectDisposedException
            Catch ex As Exception
                If Responser.StatusCode = Net.HttpStatusCode.NotFound Then
                    UserExists = False
                Else
                    ProcessException(ex, Token, $"data downloading error [{URL}]")
                End If
            Finally
                If _TempMediaList.ListExists Then _TempMediaList.RemoveAll(Function(m) m.URL.IsEmptyString)
            End Try
        End Sub
        Private Function GetVideoData(ByVal URL As String, ByVal resp As Response, ByVal DownloadUHD As Boolean, ByVal ID As String) As UserMedia
            Try
                If Not URL.IsEmptyString Then
                    Dim r$ = resp.GetResponse(URL,, EDP.ThrowException)
                    If Not r.IsEmptyString Then
                        Dim m$ = RegexReplace(r, M3U8Regex)
                        If Not m.IsEmptyString Then
                            Dim appender$ = RegexReplace(m, M3U8Appender)
                            Dim t$ = RegexReplace(r, VideoTitleRegex)
                            r = resp.GetResponse(m,, EDP.ThrowException)
                            If Not r.IsEmptyString Then
                                Dim ls As List(Of Sizes) = RegexFields(Of Sizes)(r, {M3U8Reparse}, {1, 2})
                                If ls.ListExists And Not DownloadUHD Then ls.RemoveAll(Function(v) Not v.Value.ValueBetween(1, 1080))
                                If ls.ListExists Then
                                    ls.Sort()
                                    m = $"{appender}/{ls(0).Data}"
                                    ls.Clear()
                                    Dim pID$ = ID
                                    If pID.IsEmptyString Then pID = RegexReplace(r, VideoID)
                                    If pID.IsEmptyString Then pID = "0"

                                    If Not t.IsEmptyString Then t = t.StringRemoveWinForbiddenSymbols(" ")
                                    If t.IsEmptyString Then
                                        t = pID
                                    Else
                                        If t.Length > 100 Then t = Left(t, 100)
                                    End If
                                    If Not m.IsEmptyString Then
                                        Return New UserMedia With {
                                            .Type = UTypes.m3u8,
                                            .Post = New UserPost With {.ID = pID},
                                            .URL = m,
                                            .File = $"{t}.mp4",
                                            .PictureOption = appender
                                        }
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                Return Nothing
            Catch ex As Exception
                LogError(ex, $"[XVIDEOS.UserData.GetVideoData({URL})]")
                Return Nothing
            End Try
        End Function
        Friend Function Download(ByVal URL As String, ByVal resp As Response, ByVal DownloadUHD As Boolean, ByVal ID As String)
            Dim m As UserMedia = GetVideoData(URL, resp, DownloadUHD, ID)
            If Not m.URL.IsEmptyString Then
                Dim f As SFile = m.File
                f.Path = MyFile.PathNoSeparator
                m.State = UStates.Tried
                Try
                    f = M3U8.Download(m.URL, m.PictureOption, Settings.FfmpegFile, f)
                    m.File = f
                    m.State = UStates.Downloaded
                Catch ex As Exception
                    m.State = UStates.Missing
                End Try
            End If
            Return m
        End Function
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
        Protected Overrides Function DownloadM3U8(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile) As SFile
            Return M3U8.Download(Media.URL, Media.PictureOption, Settings.FfmpegFile, DestinationFile)
        End Function
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            Return 0
        End Function
    End Class
End Namespace