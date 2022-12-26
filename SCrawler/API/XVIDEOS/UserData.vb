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
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports UStates = SCrawler.API.Base.UserMedia.States
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace API.XVIDEOS
    Friend Class UserData : Inherits UserDataBase
        Private Structure PlayListVideo : Implements IRegExCreator
            Friend ID As String
            Friend URL As String
            Friend Title As String
            Private Function CreateFromArray(ByVal ParamsArray() As String) As Object Implements IRegExCreator.CreateFromArray
                If ParamsArray.ListExists(3) Then
                    ID = ParamsArray(0)
                    URL = ParamsArray(1)
                    If Not URL.IsEmptyString Then URL = $"https://www.xvideos.com/{URL.StringTrimStart("/")}"
                    Title = ParamsArray(2).StringRemoveWinForbiddenSymbols.StringTrim
                End If
                Return Me
            End Function
            Friend Function ToUserMedia() As UserMedia
                Return New UserMedia(URL, UTypes.VideoPre) With {.Object = Me, .PictureOption = Title, .Post = ID}
            End Function
        End Structure
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
            If Not Settings.UseM3U8 Then
                'TODELETE: XVideos m3u8 delete after debug ffmpeg x86
                'If Not Settings.OS64 Then
                '    MyMainLOG = $"XVIDEOS [{ToStringForLog()}]: The plugin only works with x64 OS."
                'Else
                '    'MyMainLOG = $"{ToStringForLog()}: File [ffmpeg.exe] not found"
                'End If
                MyMainLOG = $"{ToStringForLog()}: File [ffmpeg.exe] not found"
                Exit Sub
            End If
            If IsSavedPosts Then
                If Not ACheck(MySettings.SavedVideosPlaylist.Value) Then Throw New ArgumentNullException("SavedVideosPlaylist", "Playlist of saved videos cannot be null")
                DownloadSavedVideos(Token)
            Else
                DownloadUserVideo(Token)
            End If
        End Sub
        Private Sub DownloadUserVideo(ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                Dim NextPage% = 0
                Dim r$
                Dim j As EContainer = Nothing
                Dim jj As EContainer
                Dim user$ = MySettings.GetUserUrl(Me, False)
                Dim p As UserMedia
                Dim EnvirSet As Boolean = False

                Do
                    ThrowAny(Token)
                    URL = $"https://www.xvideos.com/{user}/videos/new/{If(NextPage = 0, String.Empty, NextPage)}"
                    r = Responser.GetResponse(URL)
                    If Not r.IsEmptyString Then
                        If Not EnvirSet Then UserExists = True : UserSuspended = False : EnvirSet = True
                        j = JsonDocument.Parse(r).XmlIfNothing
                        With j
                            If .Contains("videos") Then
                                With .Item("videos")
                                    If .Count > 0 Then
                                        NextPage += 1
                                        For Each jj In .Self
                                            p = New UserMedia With {
                                                .Post = jj.Value("id"),
                                                .URL = $"https://www.xvideos.com/{jj.Value("u").StringTrimStart("/")}"
                                            }
                                            If Not p.Post.ID.IsEmptyString And Not jj.Value("u").IsEmptyString Then
                                                If Not _TempPostsList.Contains(p.Post.ID) Then
                                                    _TempPostsList.Add(p.Post.ID)
                                                    _TempMediaList.Add(p)
                                                Else
                                                    Exit Do
                                                End If
                                            End If
                                        Next
                                        Continue Do
                                    End If
                                End With
                            End If
                        End With
                    End If
                    If Not j Is Nothing Then j.Dispose()
                    Exit Do
                Loop While NextPage < 100

                If Not j Is Nothing Then j.Dispose()

                If _TempMediaList.Count > 0 Then
                    For i% = 0 To _TempMediaList.Count - 1
                        ThrowAny(Token)
                        _TempMediaList(i) = GetVideoData(_TempMediaList(i), Responser, MySettings.DownloadUHD.Value)
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
        Private Sub DownloadSavedVideos(ByVal Token As CancellationToken)
            Dim URL$ = MySettings.SavedVideosPlaylist.Value
            Try
                Dim NextPage% = 0
                Dim __continue As Boolean = True
                Dim r$
                Dim data As List(Of PlayListVideo)
                Dim i%
                Do
                    ThrowAny(Token)
                    URL = $"{MySettings.SavedVideosPlaylist.Value}{If(NextPage = 0, String.Empty, $"/{NextPage}")}"
                    r = Responser.GetResponse(URL,, EDP.ReturnValue)
                    If Responser.HasError Then
                        If Responser.StatusCode = Net.HttpStatusCode.NotFound And NextPage > 0 Then Exit Do
                        Throw New Exception(Responser.ErrorText, Responser.ErrorException)
                    End If
                    NextPage += 1
                    If Not r.IsEmptyString Then
                        data = RegexFields(Of PlayListVideo)(r, {Regex_SavedVideosPlaylist}, {1, 2, 3}, EDP.ReturnValue)
                        If data.ListExists Then
                            If data.RemoveAll(Function(d) _TempPostsList.Contains(d.ID)) > 0 Then __continue = False
                            If data.ListExists Then
                                _TempPostsList.ListAddList(data.Select(Function(d) d.ID), LNC)
                                i = _TempMediaList.Count
                                _TempMediaList.ListAddList(data.Select(Function(d) d.ToUserMedia()), LNC)
                                If _TempMediaList.Count = i Or Not __continue Then Exit Do Else Continue Do
                            End If
                        End If
                    End If
                    Exit Do
                Loop While NextPage < 100 And __continue

                If _TempMediaList.Count > 0 Then
                    For i% = 0 To _TempMediaList.Count - 1
                        ThrowAny(Token)
                        _TempMediaList(i) = GetVideoData(_TempMediaList(i), Responser, MySettings.DownloadUHD.Value)
                    Next
                    _TempMediaList.RemoveAll(Function(m) m.URL.IsEmptyString)
                End If
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]")
            End Try
        End Sub
        Private Function GetVideoData(ByVal Media As UserMedia, ByVal resp As Responser, ByVal DownloadUHD As Boolean) As UserMedia
            Try
                If Not Media.URL.IsEmptyString Then
                    Dim r$ = resp.GetResponse(Media.URL)
                    If Not r.IsEmptyString Then
                        Dim NewUrl$ = RegexReplace(r, Regex_M3U8)
                        If Not NewUrl.IsEmptyString Then
                            Dim appender$ = RegexReplace(NewUrl, Regex_M3U8_Appender)
                            Dim t$ = If(Media.PictureOption.IsEmptyString, RegexReplace(r, Regex_VideoTitle), Media.PictureOption)
                            r = resp.GetResponse(NewUrl)
                            If Not r.IsEmptyString Then
                                Dim ls As List(Of Sizes) = RegexFields(Of Sizes)(r, {Regex_M3U8_Reparse}, {1, 2})
                                If ls.ListExists And Not DownloadUHD Then ls.RemoveAll(Function(v) Not v.Value.ValueBetween(1, 1080))
                                If ls.ListExists Then
                                    ls.Sort()
                                    NewUrl = $"{appender}/{ls(0).Data.StringTrimStart("/")}"
                                    ls.Clear()
                                    Dim pID$ = Media.Post.ID
                                    If pID.IsEmptyString Then pID = RegexReplace(r, Regex_VideoID)
                                    If pID.IsEmptyString Then pID = "0"

                                    t = t.StringRemoveWinForbiddenSymbols.StringTrim
                                    If t.IsEmptyString Then
                                        t = pID
                                    Else
                                        If t.Length > 100 Then t = Left(t, 100)
                                    End If
                                    If Not NewUrl.IsEmptyString Then
                                        Return New UserMedia(NewUrl, UTypes.m3u8) With {
                                            .Post = pID,
                                            .URL_BASE = Media.URL,
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
                LogError(ex, $"[XVIDEOS.UserData.GetVideoData({Media.URL})]")
                Return Nothing
            End Try
        End Function
        Friend Function Download(ByVal URL As String, ByVal resp As Responser, ByVal DownloadUHD As Boolean, ByVal ID As String)
            Dim m As UserMedia = GetVideoData(New UserMedia(URL, UTypes.VideoPre) With {.Post = ID}, resp, DownloadUHD)
            If Not m.URL.IsEmptyString Then
                Dim f As SFile = m.File
                f.Path = MyFile.PathNoSeparator
                m.State = UStates.Tried
                Try
                    f = M3U8.Download(m.URL, m.PictureOption, f)
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
            Return M3U8.Download(Media.URL, Media.PictureOption, DestinationFile)
        End Function
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            Return 0
        End Function
    End Class
End Namespace