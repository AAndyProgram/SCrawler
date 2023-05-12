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
Imports SCrawler.API.YouTube.Objects
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Documents.JSON
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
                    If Not URL.IsEmptyString Then URL = $"https://www.xvideos.com/{HtmlConverter(URL).StringTrimStart("/")}"
                    Title = TitleHtmlConverter(ParamsArray(2))
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
            UseClientTokens = True
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            If Not Settings.UseM3U8 Then MyMainLOG = $"{ToStringForLog()}: File [ffmpeg.exe] not found" : Exit Sub
            If IsSavedPosts Then
                If Not ACheck(MySettings.SavedVideosPlaylist.Value) Then Throw New ArgumentNullException("SavedVideosPlaylist", "Playlist of saved videos cannot be null")
                DownloadSavedVideos(Token)
            Else
                DownloadUserVideo(Token)
            End If
        End Sub
        Private Sub DownloadUserVideo(ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Dim isQuickies As Boolean = False
            Try
                Dim NextPage%, d%
                Dim limit% = If(DownloadTopCount, -1)
                Dim r$, n$
                Dim j As EContainer = Nothing
                Dim jj As EContainer
                Dim user$ = MySettings.GetUserUrlPart(Me)
                Dim p As UserMedia
                Dim EnvirSet As Boolean = False

                If ID.IsEmptyString Then GetUserID()
                For i% = 0 To 1
                    If i = 1 And ID.IsEmptyString Then Exit For
                    NextPage = 0
                    d = 0
                    n = IIf(i = 0, "u", "url")
                    Do
                        ThrowAny(Token)
                        If i = 0 Then
                            URL = $"https://www.xvideos.com/{user}/videos/new/{If(NextPage = 0, String.Empty, NextPage)}"
                        Else 'Quickies
                            URL = $"https://www.xvideos.com/quickies-api/profilevideos/all/none/N/{ID}/{NextPage}"
                            isQuickies = True
                        End If
                        If Not j Is Nothing Then j.Dispose()
                        r = Responser.GetResponse(URL,, EDP.ReturnValue)
                        If Not r.IsEmptyString Then
                            If Not EnvirSet Then UserExists = True : UserSuspended = False : EnvirSet = True
                            j = JsonDocument.Parse(r)
                            If Not j Is Nothing Then
                                With j
                                    If .Contains("videos") Then
                                        With .Item("videos")
                                            If .Count > 0 Then
                                                ProgressPre.ChangeMax(.Count)
                                                NextPage += 1
                                                For Each jj In .Self
                                                    ProgressPre.Perform()
                                                    p = New UserMedia With {
                                                        .Post = jj.Value("id"),
                                                        .URL = $"https://www.xvideos.com/{jj.Value(n).StringTrimStart("/")}"
                                                    }
                                                    If Not p.Post.ID.IsEmptyString And Not jj.Value(n).IsEmptyString Then
                                                        If Not _TempPostsList.Contains(p.Post.ID) Then
                                                            _TempPostsList.Add(p.Post.ID)
                                                            _TempMediaList.Add(p)
                                                            d += 1
                                                            If limit > 0 And d = limit Then Exit Do
                                                        Else
                                                            Exit Do
                                                        End If
                                                    End If
                                                Next
                                                Continue Do
                                            End If
                                        End With
                                    End If
                                    .Dispose()
                                End With
                            End If
                        End If
                        Exit Do
                    Loop While NextPage < 100
                Next

                If Not j Is Nothing Then j.Dispose()

                If _TempMediaList.Count > 0 Then
                    ProgressPre.ChangeMax(_TempMediaList.Count)
                    For i% = 0 To _TempMediaList.Count - 1
                        ProgressPre.Perform()
                        ThrowAny(Token)
                        _TempMediaList(i) = GetVideoData(_TempMediaList(i))
                    Next
                    _TempMediaList.RemoveAll(Function(m) m.URL.IsEmptyString)
                End If
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]",, isQuickies)
            Finally
                If _TempMediaList.ListExists Then _TempMediaList.RemoveAll(Function(m) m.URL.IsEmptyString)
            End Try
        End Sub
        Private Sub GetUserID()
            Dim r$ = Responser.GetResponse($"https://www.xvideos.com/{MySettings.GetUserUrlPart(Me)}",, EDP.ReturnValue)
            If Not r.IsEmptyString Then ID = RegexReplace(r, RParams.DMS("""id_user"":(\d+)", 1, EDP.ReturnValue))
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
                        If Responser.StatusCode = Net.HttpStatusCode.NotFound Then
                            If NextPage = 0 Then
                                MyMainLOG = $"XVIDEOS saved video playlist {URL} not found."
                                Exit Sub
                            Else
                                Exit Do
                            End If
                        Else
                            Throw New Exception(Responser.ErrorText, Responser.ErrorException)
                        End If
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
                    ProgressPre.ChangeMax(_TempMediaList.Count)
                    For i% = 0 To _TempMediaList.Count - 1
                        ProgressPre.Perform()
                        ThrowAny(Token)
                        _TempMediaList(i) = GetVideoData(_TempMediaList(i))
                    Next
                    _TempMediaList.RemoveAll(Function(m) m.URL.IsEmptyString)
                End If
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]")
            End Try
        End Sub
        Private Function GetVideoData(ByVal Media As UserMedia) As UserMedia
            Try
                If Not Media.URL.IsEmptyString Then
                    Dim r$ = Responser.GetResponse(Media.URL)
                    If Not r.IsEmptyString Then
                        Dim NewUrl$ = RegexReplace(r, Regex_M3U8)
                        If Not NewUrl.IsEmptyString Then
                            Dim appender$ = RegexReplace(NewUrl, Regex_M3U8_Appender)
                            Dim t$ = If(Media.PictureOption.IsEmptyString, RegexReplace(r, Regex_VideoTitle), Media.PictureOption)
                            r = Responser.GetResponse(NewUrl)
                            If Not r.IsEmptyString Then
                                Dim ls As List(Of Sizes) = RegexFields(Of Sizes)(r, {Regex_M3U8_Reparse}, {1, 2})
                                If ls.ListExists And Not MySettings.DownloadUHD.Value Then ls.RemoveAll(Function(v) Not v.Value.ValueBetween(1, 1080))
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
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            Dim m As UserMedia = GetVideoData(New UserMedia(Data.URL, UTypes.VideoPre))
            If Not m.URL.IsEmptyString Then _TempMediaList.Add(m)
        End Sub
        Protected Overrides Function DownloadM3U8(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile, ByVal Token As CancellationToken) As SFile
            Return M3U8.Download(Media.URL, Media.PictureOption, DestinationFile, Token, Progress, Not IsSingleObjectDownload)
        End Function
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            Dim isQuickies As Boolean = False
            If Not IsNothing(EObj) AndAlso TypeOf EObj Is Boolean Then isQuickies = CBool(EObj)
            If Responser.StatusCode = Net.HttpStatusCode.NotFound Then
                UserExists = False
                Return 1
            ElseIf isQuickies And Responser.StatusCode = Net.HttpStatusCode.InternalServerError Then
                Return 1
            Else
                Return 0
            End If
        End Function
    End Class
End Namespace