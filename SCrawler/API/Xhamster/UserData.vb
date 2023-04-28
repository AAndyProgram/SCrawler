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
Namespace API.Xhamster
    Friend Class UserData : Inherits UserDataBase
#Region "XML names"
        Private Const Name_TrueName As String = "TrueName"
#End Region
#Region "Declarations"
        Friend Property IsChannel As Boolean = False
        Friend Property TrueName As String = String.Empty
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return DirectCast(HOST.Source, SiteSettings)
            End Get
        End Property
        Private Structure ExchObj
            Friend IsPhoto As Boolean
        End Structure
        Private ReadOnly _TempPhotoData As List(Of UserMedia)
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            Dim setNames As Action = Sub()
                                         If TrueName.IsEmptyString Then
                                             Dim n$() = Name.Split("@")
                                             If n.ListExists Then
                                                 If n.Length = 2 Then
                                                     TrueName = n(0)
                                                     IsChannel = True
                                                 ElseIf IsChannel Then
                                                     TrueName = Name
                                                 Else
                                                     TrueName = n(0)
                                                 End If
                                             End If
                                         End If
                                     End Sub
            With Container
                If Loading Then
                    IsChannel = .Value(Name_IsChannel).FromXML(Of Boolean)(False)
                    TrueName = .Value(Name_TrueName)
                    setNames.Invoke
                Else
                    setNames.Invoke
                    .Add(Name_IsChannel, IsChannel.BoolToInteger)
                    .Add(Name_TrueName, TrueName)
                    setNames.Invoke
                End If
            End With
        End Sub
#End Region
#Region "Initializer"
        Friend Sub New()
            UseInternalM3U8Function = True
            UseClientTokens = True
            _TempPhotoData = New List(Of UserMedia)
        End Sub
#End Region
#Region "Download base functions"
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            _TempPhotoData.Clear()
            If DownloadVideos Then DownloadData(1, True, Token)
            If Not IsChannel And DownloadImages Then
                DownloadData(1, False, Token)
                ReparsePhoto(Token)
            End If
        End Sub
        Private Overloads Sub DownloadData(ByVal Page As Integer, ByVal IsVideo As Boolean, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                Dim MaxPage% = -1
                Dim Type As UTypes = IIf(IsVideo, UTypes.VideoPre, UTypes.Picture)
                Dim mPages$ = IIf(IsVideo, "maxVideoPages", "maxPhotoPages")
                Dim listNode$()
                Dim skipped As Boolean = False
                Dim cBefore% = _TempMediaList.Count
                Dim m As UserMedia

                If IsSavedPosts Then
                    URL = $"https://xhamster.com/my/favorites/{IIf(IsVideo, "videos", "photos-and-galleries")}{IIf(Page = 1, String.Empty, $"/{Page}")}"
                    listNode = If(IsVideo, {"favoriteVideoListComponent", "models"}, {"favoritesGalleriesAndPhotosCollection"})
                ElseIf IsChannel Then
                    URL = $"https://xhamster.com/channels/{TrueName}/newest{IIf(Page = 1, String.Empty, $"/{Page}")}"
                    listNode = {"trendingVideoListComponent", "models"}
                Else
                    URL = $"https://xhamster.com/users/{TrueName}/{IIf(IsVideo, "videos", "photos")}{IIf(Page = 1, String.Empty, $"/{Page}")}"
                    listNode = {If(IsVideo, "userVideoCollection", "userGalleriesCollection")}
                End If
                ThrowAny(Token)

                Dim r$ = Responser.GetResponse(URL)
                If Not r.IsEmptyString Then r = RegexReplace(r, HtmlScript)
                If Not r.IsEmptyString Then
                    Using j As EContainer = JsonDocument.Parse(r)
                        If j.ListExists Then
                            If Not MySettings.Domains.UpdatedBySite AndAlso j.Contains("trustURLs") Then _
                               MySettings.Domains.Add(j("trustURLs").Select(Function(d) d(0).XmlIfNothingValue), True)

                            MaxPage = j.Value(mPages).FromXML(Of Integer)(-1)

                            With j(listNode)
                                If .ListExists Then
                                    For Each e As EContainer In .Self
                                        m = ExtractMedia(e, Type)
                                        If Not m.URL.IsEmptyString Then
                                            If m.File.IsEmptyString Then Continue For

                                            If m.Post.Date.HasValue Then
                                                Select Case CheckDatesLimit(m.Post.Date.Value, Nothing)
                                                    Case DateResult.Skip : skipped = True : Continue For
                                                    Case DateResult.Exit : Exit Sub
                                                End Select
                                            End If

                                            If IsVideo AndAlso Not _TempPostsList.Contains(m.Post.ID) Then
                                                _TempPostsList.Add(m.Post.ID)
                                                _TempMediaList.ListAddValue(m, LNC)
                                            ElseIf Not IsVideo Then
                                                If DirectCast(m.Object, ExchObj).IsPhoto Then
                                                    If Not m.Post.ID.IsEmptyString AndAlso Not _TempPostsList.Contains(m.Post.ID) Then
                                                        _TempPostsList.Add(m.Post.ID)
                                                        _TempMediaList.ListAddValue(m, LNC)
                                                    End If
                                                Else
                                                    _TempPhotoData.ListAddValue(m, LNC)
                                                End If
                                            Else
                                                Exit Sub
                                            End If
                                        End If
                                    Next
                                End If
                            End With
                        End If
                    End Using
                End If

                If (Not _TempMediaList.Count = cBefore Or skipped) And
                   (IsChannel Or (MaxPage > 0 And Page < MaxPage)) Then DownloadData(Page + 1, IsVideo, Token)
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]")
            End Try
        End Sub
#End Region
#Region "Reparse video, photo"
        Protected Overrides Sub ReparseVideo(ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                If _TempMediaList.Count > 0 AndAlso _TempMediaList.Exists(Function(tm) tm.Type = UTypes.VideoPre) Then
                    Dim m As UserMedia, m2 As UserMedia
                    For i% = _TempMediaList.Count - 1 To 0 Step -1
                        If _TempMediaList(i).Type = UTypes.VideoPre Then
                            m = _TempMediaList(i)
                            If Not m.URL_BASE.IsEmptyString Then
                                m2 = Nothing
                                If GetM3U8(m2, m.URL_BASE) Then
                                    m2.URL_BASE = m.URL_BASE
                                    _TempMediaList(i) = m2
                                Else
                                    m.State = UserMedia.States.Missing
                                    _TempMediaList(i) = m
                                End If
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex, Token, "video reparsing error", False)
            End Try
        End Sub
        Private Overloads Sub ReparsePhoto(ByVal Token As CancellationToken)
            If _TempPhotoData.Count > 0 Then
                For i% = 0 To _TempPhotoData.Count - 1 : ReparsePhoto(i, 1, Token) : Next
                _TempPhotoData.Clear()
            End If
        End Sub
        Private Overloads Sub ReparsePhoto(ByVal Index As Integer, ByVal Page As Integer, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                Dim MaxPage% = -1
                Dim m As UserMedia
                Dim sm As UserMedia = _TempPhotoData(Index)

                URL = $"{sm.URL}{IIf(Page = 1, String.Empty, $"/{Page}")}"
                ThrowAny(Token)
                Dim r$ = Responser.GetResponse(URL)
                If Not r.IsEmptyString Then r = RegexReplace(r, HtmlScript)
                If Not r.IsEmptyString Then
                    Using j As EContainer = JsonDocument.Parse(r).XmlIfNothing
                        If j.Count > 0 Then
                            MaxPage = j.Value({"pagination"}, "maxPage").FromXML(Of Integer)(-1)
                            With j({"photosGalleryModel"}, "photos")
                                If .ListExists Then
                                    For Each e In .Self
                                        m = ExtractMedia(e, UTypes.Picture, "imageURL", False, sm.Post.Date)
                                        m.URL_BASE = sm.URL
                                        If Not m.URL.IsEmptyString Then
                                            m.Post.ID = $"{sm.Post.ID}_{m.Post.ID}"
                                            m.SpecialFolder = sm.SpecialFolder
                                            If Not _TempPostsList.Contains(m.Post.ID) Then
                                                _TempPostsList.Add(m.Post.ID)
                                                _TempMediaList.ListAddValue(m, LNC)
                                            Else
                                                Exit Sub
                                            End If
                                        End If
                                    Next
                                End If
                            End With
                        End If
                    End Using
                End If

                If MaxPage > 0 AndAlso Page < MaxPage Then ReparsePhoto(Index, Page + 1, Token)
            Catch ex As Exception
                ProcessException(ex, Token, "photo reparsing error", False)
            End Try
        End Sub
#End Region
#Region "Reparse missing"
        Protected Overrides Sub ReparseMissing(ByVal Token As CancellationToken)
            Dim rList As New List(Of Integer)
            Try
                If ContentMissingExists Then
                    Dim m As UserMedia, m2 As UserMedia
                    For i% = 0 To _ContentList.Count - 1
                        m = _ContentList(i)
                        If m.State = UserMedia.States.Missing AndAlso Not m.URL_BASE.IsEmptyString Then
                            ThrowAny(Token)
                            m2 = Nothing
                            If GetM3U8(m2, m.URL_BASE) Then
                                m2.URL_BASE = m.URL_BASE
                                _TempMediaList.ListAddValue(m2, LNC)
                                rList.Add(i)
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex, Token, "missing data downloading error")
            Finally
                If rList.Count > 0 Then
                    For i% = rList.Count - 1 To 0 Step -1 : _ContentList.RemoveAt(rList(i)) : Next
                    rList.Clear()
                End If
            End Try
        End Sub
#End Region
#Region "GetM3U8"
        Private Overloads Function GetM3U8(ByRef m As UserMedia, ByVal URL As String) As Boolean
            Try
                If Not URL.IsEmptyString Then
                    Dim r$ = Responser.GetResponse(URL)
                    If Not r.IsEmptyString Then r = RegexReplace(r, HtmlScript)
                    If Not r.IsEmptyString Then
                        Using j As EContainer = JsonDocument.Parse(r)
                            If j.ListExists Then
                                m = ExtractMedia(j("videoModel"), UTypes.VideoPre)
                                m.URL_BASE = URL
                                Return GetM3U8(m, j)
                            End If
                        End Using
                    End If
                End If
                Return False
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.ReturnValue, ex, $"[{ToStringForLog()}]: API.Xhamster.GetM3U8({URL})", False)
            End Try
        End Function
        Private Overloads Function GetM3U8(ByRef m As UserMedia, ByVal j As EContainer) As Boolean
            Dim url$ = j.Value({"xplayerSettings", "sources", "hls"}, "url")
            If Not url.IsEmptyString Then m.URL = url : m.Type = UTypes.m3u8 : Return True
            Return False
        End Function
#End Region
#Region "DownloadSingleObject"
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            _ContentList.Add(New UserMedia(Data.URL_BASE) With {.State = UserMedia.States.Missing})
            ReparseMissing(Token)
        End Sub
#End Region
#Region "Download data"
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
        Protected Overrides Function DownloadM3U8(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile, ByVal Token As CancellationToken) As SFile
            Media.File = DestinationFile
            Return M3U8.Download(Media, Responser, MySettings.DownloadUHD.Value, Token, If(UseInternalM3U8Function_UseProgress, Progress, Nothing))
        End Function
#End Region
#Region "Create media"
        Private Function ExtractMedia(ByVal j As EContainer, ByVal t As UTypes, Optional ByVal UrlNode As String = "pageURL",
                                      Optional ByVal DetectGalery As Boolean = True, Optional ByVal PostDate As Date? = Nothing) As UserMedia
            If Not j Is Nothing Then
                Dim m As New UserMedia(j.Value(UrlNode).Replace("\", String.Empty), t) With {
                    .Post = New UserPost With {
                        .ID = j.Value("id"),
                        .Date = AConvert(Of Date)(j.Value("created"), UnixDate32Provider, Nothing)
                    },
                    .PictureOption = TitleHtmlConverter(j.Value("title")),
                    .Object = New ExchObj
                }
                If PostDate.HasValue Then m.Post.Date = PostDate
                Dim setSpecialFolder As Boolean = False
                Dim processFile As Boolean = True
                Dim ext$ = "mp4"
                If t = UTypes.Picture Then
                    ext = "jpg"
                    If (Not DetectGalery OrElse j.Contains("galleryId")) AndAlso Not j.Value("imageURL").IsEmptyString Then
                        m.Object = New ExchObj With {.IsPhoto = True}
                        m.URL = j.Value("imageURL")
                        m.URL_BASE = m.URL
                        If DetectGalery Then m.Post.ID = $"{j.Value("galleryId")}_{m.Post.ID}"
                        m.File = m.URL
                        m.File.Separator = "\"
                        processFile = m.File.File.IsEmptyString
                    Else
                        setSpecialFolder = True
                    End If
                End If
                If Not m.URL.IsEmptyString Then
                    If m.Post.ID.IsEmptyString Then m.Post.ID = m.URL.Split("/").LastOrDefault
                    If m.PictureOption.IsEmptyString Then m.PictureOption = TitleHtmlConverter(j.Value("titleLocalized"))
                    If m.PictureOption.IsEmptyString Then m.PictureOption = m.Post.ID
                    If setSpecialFolder Then m.SpecialFolder = m.PictureOption

                    If processFile Then
                        If Not m.PictureOption.IsEmptyString Then
                            m.File = $"{m.PictureOption}.{ext}"
                        ElseIf Not m.Post.ID.IsEmptyString Then
                            m.File = $"{m.Post.ID}.{ext}"
                        End If
                    End If
                    m.File.Separator = "\"
                End If
                Return m
            Else
                Return Nothing
            End If
        End Function
#End Region
#Region "Exception"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            Return If(Responser.Status = Net.WebExceptionStatus.ConnectionClosed, 1, 0)
        End Function
#End Region
#Region "IDisposable support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then _TempPhotoData.Clear()
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace