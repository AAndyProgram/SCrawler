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
Namespace API.PornHub
    Friend Class UserData : Inherits UserDataBase
        Private Const UrlPattern As String = "https://www.pornhub.com/{0}"
#Region "Declarations"
#Region "XML names"
        Private Const Name_PersonType As String = "PersonType"
        Private Const Name_DownloadUHD As String = "DownloadUHD"
        Private Const Name_DownloadUploaded As String = "DownloadUploaded"
        Private Const Name_DownloadTagged As String = "DownloadTagged"
        Private Const Name_DownloadPrivate As String = "DownloadPrivate"
        Private Const Name_DownloadFavorite As String = "DownloadFavorite"
        Private Const Name_DownloadGifs As String = "DownloadGifs"
#End Region
#Region "Structures"
        Private Structure FlashVar : Implements IRegExCreator
            Friend Name As String
            Friend Value As String
            Public Shared Widening Operator CType(ByVal Name As String) As FlashVar
                Return New FlashVar With {.Name = Name}
            End Operator
            Private Function CreateFromArray(ByVal ParamsArray() As String) As Object Implements IRegExCreator.CreateFromArray
                If ParamsArray.ListExists(2) Then
                    Name = ParamsArray(0)
                    Value = ParamsArray(1)
                    If Not Value.IsEmptyString Then Value = Value.Replace(""" + """, String.Empty).Replace("""", String.Empty).StringTrim
                End If
                Return Me
            End Function
            Public Overrides Function Equals(ByVal Obj As Object) As Boolean
                Return CType(Obj, FlashVar).Name = Name
            End Function
        End Structure
        Private Structure UserVideo : Implements IRegExCreator
            Friend URL As String
            Friend ID As String
            Friend Title As String
            Friend Type As VideoTypes
            Friend Function ToUserMedia(Optional ByVal SpecialFolder As String = Nothing) As UserMedia
                Return New UserMedia(URL, UTypes.VideoPre) With {
                    .File = If(Title.IsEmptyString, .File, New SFile($"{Title}.mp4")),
                    .Post = ID,
                    .SpecialFolder = SpecialFolder
                }
            End Function
            Private Function CreateFromArray(ByVal ParamsArray() As String) As Object Implements IRegExCreator.CreateFromArray
                If ParamsArray.ListExists(4) Then
                    URL = ParamsArray(0)
                    ID = RegexReplace(URL, RegexVideo_Video_VideoKey)
                    If ID.IsEmptyString Then
                        URL = String.Empty
                    Else
                        URL = String.Format(UrlPattern, URL.TrimStart("/"))
                        Title = TitleHtmlConverter(ParamsArray(1))
                        If Not ParamsArray(2).IsEmptyString Then
                            Type = VideoTypes.Private
                        ElseIf Not ParamsArray(3).IsEmptyString Then
                            Type = VideoTypes.Tagged
                        Else
                            Type = VideoTypes.Uploaded
                        End If
                    End If
                End If
                Return Me
            End Function
            Public Overrides Function Equals(ByVal Obj As Object) As Boolean
                Return DirectCast(Obj, UserVideo).URL = URL
            End Function
        End Structure
        Private Structure PhotoBlock : Implements IRegExCreator
            Friend AlbumID As String
            Friend Data As String
            Private Function CreateFromArray(ByVal ParamsArray() As String) As Object Implements IRegExCreator.CreateFromArray
                If ParamsArray.ListExists(2) Then
                    AlbumID = ParamsArray(0)
                    Data = ParamsArray(1).StringTrim
                End If
                Return Me
            End Function
        End Structure
#End Region
#Region "Enums"
        Private Enum VideoTypes
            Undefined
            Uploaded
            [Private]
            Tagged
            Favorite
        End Enum
#End Region
#Region "Constants"
        Private Const PersonTypeModel As String = "model"
        Private Const PersonTypeUser As String = "users"
        Private Const PersonTypePornstar As String = "pornstar"
        Private Const PersonTypeCannel As String = "channels"
        Private Const PersonTypePlaylist As String = "playlist"
        Private Const PlaylistsLabelName As String = "Playlist"
#End Region
#Region "Person"
        Friend Property PersonType As String
        Friend Overrides Property FriendlyName As String
            Get
                If _FriendlyName.IsEmptyString Then Return NameTrue Else Return _FriendlyName
            End Get
            Set(ByVal n As String)
                _FriendlyName = n
            End Set
        End Property
#End Region
#Region "Advanced fields"
        Friend Overrides ReadOnly Property FeedIsUser As Boolean
            Get
                Return IsUser Or SiteMode = SiteModes.Playlists
            End Get
        End Property
        Friend Property DownloadUHD As Boolean = False
        Friend Property DownloadUploaded As Boolean = True
        Friend Property DownloadTagged As Boolean = False
        Friend Property DownloadPrivate As Boolean = False
        Friend Property DownloadFavorite As Boolean = False
        Friend Property DownloadGifs As Boolean
        Friend Overrides ReadOnly Property IsUser As Boolean
            Get
                Return SiteMode = SiteModes.User
            End Get
        End Property
        Friend Property SiteMode As SiteModes = SiteModes.User
        Friend Property QueryString As String
            Get
                If IsUser Then
                    Return String.Empty
                Else
                    Return GetNonUserUrl(0)
                End If
            End Get
            Set(ByVal q As String)
                UpdateUserOptions(True, q)
            End Set
        End Property
        Friend Overrides ReadOnly Property SpecialLabels As IEnumerable(Of String)
            Get
                Return {SearchRequestLabelName, PlaylistsLabelName}
            End Get
        End Property
#End Region
#Region "ExchangeOptions"
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return New UserExchangeOptions(Me)
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            If Not Obj Is Nothing AndAlso TypeOf Obj Is UserExchangeOptions Then
                With DirectCast(Obj, UserExchangeOptions)
                    DownloadUHD = .DownloadUHD
                    DownloadUploaded = .DownloadUploaded
                    DownloadTagged = .DownloadTagged
                    DownloadPrivate = .DownloadPrivate
                    DownloadFavorite = .DownloadFavorite
                    DownloadGifs = .DownloadGifs
                    QueryString = .QueryString
                End With
            End If
        End Sub
#End Region
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return DirectCast(HOST.Source, SiteSettings)
            End Get
        End Property
#End Region
#Region "Initializer"
        Friend Sub New()
            UseInternalM3U8Function = True
            UseClientTokens = True
            SessionPosts = New List(Of String)
        End Sub
#End Region
#Region "Loader"
        Private Function UpdateUserOptions(Optional ByVal Force As Boolean = False, Optional ByVal NewUrl As String = Nothing) As Boolean

            If Not Force OrElse (Not IsUser AndAlso Not SiteMode = SiteModes.Playlists AndAlso Not NewUrl.IsEmptyString AndAlso MyFileSettings.Exists) Then
                Dim eObj As Plugin.ExchangeOptions = Nothing
                If Force Then eObj = MySettings.IsMyUser(NewUrl)
                If (Force And Not eObj.UserName.IsEmptyString) Or (Not Force And Not Name.IsEmptyString And NameTrue(True).IsEmptyString) Then
                    If Not If(Force, eObj.Options, Options).IsEmptyString Then
                        If (IsUser Or SiteMode = SiteModes.Playlists) And Force Then
                            Return False
                        Else
                            SiteMode = SiteModes.Search
                            Options = If(Force, eObj.Options, Options)
                            If Options.ToLower.StartsWith(PersonTypePlaylist) Then
                                SiteMode = SiteModes.Playlists
                                NameTrue = Options.ToLower.Replace(PersonTypePlaylist, String.Empty).StringTrim.TrimStart("/")
                            Else
                                NameTrue = Options
                            End If
                            If Not Force Then
                                Dim l$ = IIf(SiteMode = SiteModes.Playlists, PlaylistsLabelName, SearchRequestLabelName)
                                Settings.Labels.Add(l)
                                Labels.ListAddValue(l, LNC)
                                Labels.Sort()
                                Return True
                            End If
                        End If
                    Else
                        SiteMode = SiteModes.User
                        Dim n$() = Name.Split("_")
                        If n.ListExists(2) Then
                            NameTrue = Name.Replace($"{n(0)}_", String.Empty)
                            PersonType = n(0)
                        End If
                    End If
                End If
            End If
            Return False
        End Function
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            With Container
                If Loading Then
                    PersonType = .Value(Name_PersonType)
                    DownloadUHD = .Value(Name_DownloadUHD).FromXML(Of Boolean)(False)
                    DownloadUploaded = .Value(Name_DownloadUploaded).FromXML(Of Boolean)(True)
                    DownloadTagged = .Value(Name_DownloadTagged).FromXML(Of Boolean)(False)
                    DownloadPrivate = .Value(Name_DownloadPrivate).FromXML(Of Boolean)(False)
                    DownloadFavorite = .Value(Name_DownloadFavorite).FromXML(Of Boolean)(False)
                    DownloadGifs = .Value(Name_DownloadGifs).FromXML(Of Integer)(False)
                    SiteMode = .Value(Name_SiteMode).FromXML(Of Integer)(SiteModes.User)
                    UpdateUserOptions()
                Else
                    If UpdateUserOptions() Then .Value(Name_LabelsName) = LabelsString : .Value(Name_TrueName) = NameTrue(True)
                    .Add(Name_PersonType, PersonType)
                    .Add(Name_DownloadUHD, DownloadUHD.BoolToInteger)
                    .Add(Name_DownloadUploaded, DownloadUploaded.BoolToInteger)
                    .Add(Name_DownloadTagged, DownloadTagged.BoolToInteger)
                    .Add(Name_DownloadPrivate, DownloadPrivate.BoolToInteger)
                    .Add(Name_DownloadFavorite, DownloadFavorite.BoolToInteger)
                    .Add(Name_DownloadGifs, DownloadGifs.BoolToInteger)
                    .Add(Name_SiteMode, CInt(SiteMode))

                    'Debug.WriteLine(GetNonUserUrl(0))
                    'Debug.WriteLine(GetNonUserUrl(2))
                End If
            End With
        End Sub
#End Region
#Region "Downloading"
#Region "Download override"
        Private Const PlayListUrlPattern As String = "https://www.pornhub.com/playlist/viewChunked?id={0}&token={1}&page={2}"
        Private PlaylistToken As String = String.Empty
        Private ReadOnly SessionPosts As List(Of String)
        Private _PageVideosRepeat As Integer = 0
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            Try
                UpdateM3U8URLS = False
                PlaylistToken = String.Empty
                Responser.ResetStatus()
                _PageVideosRepeat = 0
                SessionPosts.Clear()

                If IsSavedPosts Then
                    PersonType = PersonTypeUser
                    NameTrue = MySettings.SavedPostsUserName.Value
                End If

                Dim limit% = If(DownloadTopCount, -1)
                If DownloadVideos Then
                    If SiteMode = SiteModes.Playlists Then
                        Responser.Mode = Responser.Modes.Default
                        GetPlaylistToken(Token)
                        DownloadUserVideos(1, VideoTypes.Favorite, False, Token)
                    ElseIf IsSavedPosts Or Not IsUser Or PersonType = PersonTypeUser Then
                        DownloadUserVideos(1, VideoTypes.Favorite, False, Token)
                    Else
                        If DownloadUploaded Then
                            SessionPosts.Clear()
                            DownloadUserVideos(1, VideoTypes.Uploaded, False, Token)
                        End If
                        If DownloadTagged Then
                            SessionPosts.Clear()
                            Dim lBefore% = _TempMediaList.Count
                            DownloadUserVideos(1, VideoTypes.Tagged, False, Token)
                            If PersonType = PersonTypePornstar And lBefore = _TempMediaList.Count Then
                                SessionPosts.Clear()
                                DownloadUserVideos(1, VideoTypes.Tagged, True, Token)
                            End If
                        End If
                        If DownloadPrivate Then
                            SessionPosts.Clear()
                            DownloadUserVideos(1, VideoTypes.Private, False, Token)
                        End If
                        If DownloadFavorite Then
                            SessionPosts.Clear()
                            DownloadUserVideos(1, VideoTypes.Favorite, False, Token)
                        End If
                    End If

                    If _TempMediaList.Count > 0 Then
                        _TempMediaList.RemoveAll(Function(m) Not m.Type = UTypes.m3u8 And Not m.Type = UTypes.VideoPre)
                        If limit > 0 And _TempMediaList.Count > limit Then _TempMediaList.ListAddList(_TempMediaList.ListTake(-1, limit), LAP.ClearBeforeAdd)
                    End If
                End If

                If DownloadGifs And Not IsSavedPosts And Not IsSubscription And IsUser Then DownloadUserGifs(Token)
                If DownloadImages And Not IsSubscription And IsUser Then DownloadUserPhotos(Token)
            Finally
                Responser.Mode = Responser.Modes.Default
                Responser.Method = "GET"
                ProgressPre.Done()
            End Try
        End Sub
#End Region
#Region "Download video"
        Friend Function GetNonUserUrl(ByVal Page As Integer) As String
            If IsUser Then
                Return String.Empty
            Else
                Dim url$ = $"https://www.pornhub.com/{Options}"
                If Page > 1 Then
                    If url.Contains("?") Then
                        url &= $"&page={Page}"
                    Else
                        url = url.TrimEnd("/")
                        url &= $"?page={Page}"
                    End If
                End If
                Return url
            End If
        End Function
        Private Sub DownloadUserVideos(ByVal Page As Integer, ByVal Type As VideoTypes, ByVal SecondMode As Boolean, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            ProgressPre.ChangeMax(1)
            Try
                Dim specFolder$ = String.Empty
                Dim tryNextPage As Boolean = False
                Dim limit% = If(DownloadTopCount, -1)
                Dim cBefore% = _TempMediaList.Count
                If IsUser Then
                    URL = $"https://www.pornhub.com/{PersonType}/{NameTrue}"
                    If Type = VideoTypes.Uploaded Then
                        URL &= "/videos/upload"
                    ElseIf Type = VideoTypes.Tagged Then
                        If Not SecondMode Then URL &= "/videos"
                        specFolder = "Tagged"
                    ElseIf Type = VideoTypes.Private Then
                        URL &= "/videos/private"
                        specFolder = "Private"
                    ElseIf Type = VideoTypes.Favorite Then
                        URL &= "/videos/favorites"
                        If Not PersonType = PersonTypeUser Then specFolder = "Favorite"
                    Else
                        Throw New ArgumentException($"Type '{Type}' is not implemented in the video download function", "Type")
                    End If
                    If Page > 1 Then URL &= $"?page={Page}"
                ElseIf SiteMode = SiteModes.Playlists Then
                    If PlaylistToken.IsEmptyString Then Throw New ArgumentNullException("PlaylistToken", "Unable to get 'PlaylistToken'")
                    URL = String.Format(PlayListUrlPattern, NameTrue, PlaylistToken, Page)
                Else
                    URL = GetNonUserUrl(Page)
                End If

                ThrowAny(Token)

                'Debug.WriteLine(URL)
                Dim r$ = Responser.GetResponse(URL)
                If Not r.IsEmptyString Then
                    Dim l As List(Of UserVideo) = RegexFields(Of UserVideo)(r, {RegexUserVideos}, {6, 7, 3, 10})
                    'If l.ListExists And Not SiteMode = SiteModes.Playlists Then l = l.ListTake(3, l.Count).ToList
                    If l.ListExists And Not SiteMode = SiteModes.Playlists Then l = l.ListTake(1, l.Count).ToList
                    If l.ListExists Then
                        If IsUser Then
                            If Type = VideoTypes.Favorite Then
                                l.RemoveAll(Function(uv) uv.Type = VideoTypes.Private)
                            ElseIf Not PersonType = PersonTypeCannel Then
                                l.RemoveAll(Function(uv) Not uv.Type = Type)
                            End If
                        End If
                        If l.Count > 0 Then l.RemoveAll(Function(uv) uv.ID.IsEmptyString Or uv.URL.IsEmptyString)
                        If l.Count > 0 Then
                            Dim lBefore% = l.Count
                            Dim nonLastPageDetected As Boolean = False
                            Dim newLastPageIDs As New List(Of String)
                            Dim pageRepeatSet As Boolean = False, prevPostsFound As Boolean = False, newPostsFound As Boolean = False
                            l.RemoveAll(Function(ByVal uv As UserVideo) As Boolean
                                            newLastPageIDs.Add(uv.ID)
                                            If Not _TempPostsList.Contains(uv.ID) Then
                                                _TempPostsList.Add(uv.ID)
                                                newPostsFound = True
                                                Return False
                                            ElseIf SessionPosts.Count > 0 AndAlso SessionPosts.Contains(uv.id) Then
                                                prevPostsFound = True
                                                Return True
                                            Else
                                                If Not pageRepeatSet And Not newPostsFound Then pageRepeatSet = True : _PageVideosRepeat += 1
                                                'Debug.WriteLine($"[REMOVED]: {uv.Title}")
                                                Return True
                                            End If
                                        End Function)
                            'Debug.WriteLineIf(l.Count > 0, l.Select(Function(ll) ll.Title).ListToString(vbNewLine))
                            If prevPostsFound And Not pageRepeatSet And Not newPostsFound Then pageRepeatSet = True : _PageVideosRepeat += 1
                            If prevPostsFound And newPostsFound And pageRepeatSet Then _PageVideosRepeat -= 1
                            If l.Count > 0 Then _TempMediaList.ListAddList(l.Select(Function(uv) uv.ToUserMedia(specFolder)))
                            SessionPosts.ListAddList(newLastPageIDs, LNC)
                            newLastPageIDs.Clear()

                            If limit > 0 And _TempMediaList.Count >= limit Then Exit Sub
                            If _PageVideosRepeat < 2 And
                               ((Not IsUser And prevPostsFound And Not newPostsFound And Page < 1000) Or
                               (Not cBefore = _TempMediaList.Count And (IsUser Or Page < 1000))) Then tryNextPage = True

                            l.Clear()
                        End If
                    End If
                End If

                If tryNextPage Then DownloadUserVideos(Page + 1, Type, SecondMode, Token)
            Catch regex_ex As RegexFieldsTextBecameNullException
                If Not IsSavedPosts Then MyMainLOG = $"{ToStringForLog()}: videos not found. You may need to update your credentials."
            Catch ex As Exception
                ProcessException(ex, Token, $"videos downloading error [{URL}]")
            Finally
                ProgressPre.Perform()
            End Try
        End Sub
        Private Sub GetPlaylistToken(ByVal Token As CancellationToken)
            Dim URL$ = GetNonUserUrl(0)
            Try
                Dim r$ = Responser.GetResponse(URL)
                If Not r.IsEmptyString Then PlaylistToken = RegexReplace(r, RegexDataToken)
            Catch ex As Exception
                ProcessException(ex, Token, $"token getting error [{URL}]")
            End Try
        End Sub
#End Region
#Region "Download GIF"
        Private Sub DownloadUserGifs(ByVal Token As CancellationToken)
            Dim URL$ = $"https://www.pornhub.com/{PersonType}/{NameTrue}/gifs"
            Try
                ThrowAny(Token)
                Dim r$ = Responser.GetResponse(URL)
                If Not r.IsEmptyString Then
                    Dim n$
                    Dim m As UserMedia = Nothing
                    Dim l As List(Of RegexMatchStruct) = RegexFields(Of RegexMatchStruct)(r, {Regex_Gif_Array}, {1}, EDP.ReturnValue)
                    Dim l2 As List(Of String) = Nothing
                    Dim l3 As List(Of String) = Nothing
                    If l.ListExists Then l2 = l.Select(Function(ll) $"gif/{ll.Arr(0).Replace("gif", String.Empty)}").ToList
                    If l2.ListExists Then
                        ProgressPre.ChangeMax(l2.Count)
                        For Each gif$ In l2
                            If Not _TempPostsList.Contains(gif) Then
                                _TempPostsList.Add(gif)
                                URL = $"https://www.pornhub.com/{gif}"
                                m = New UserMedia(URL, UTypes.Video) With {.Post = gif, .SpecialFolder = "GIFs\"}
                                ProgressPre.Perform()
                                ThrowAny(Token)
                                Try
                                    r = Responser.GetResponse(URL)
                                    If Not r.IsEmptyString Then
                                        If l3.ListExists Then l3.Clear() : l3 = Nothing
                                        l3 = RegexReplace(r, Regex_Gif_UrlName)
                                        If l3.ListExists(3) Then
                                            m.URL = l3(2)
                                            m.File = m.URL
                                            n = TitleHtmlConverter(l3(1))
                                            If MySettings.DownloadGifsAsMp4.Value Then m.File.Extension = "mp4"
                                            If Not n.IsEmptyString Then m.File.Name = n
                                        End If
                                    End If
                                Catch gif_down_ex As Exception
                                    m.State = UserMedia.States.Missing
                                End Try
                                _TempMediaList.ListAddValue(m)
                            End If
                        Next
                    End If
                    If l.ListExists Then l.Clear()
                    If l2.ListExists Then l2.Clear()
                    If l3.ListExists Then l3.Clear()
                End If
            Catch ex As Exception
                ProcessException(ex, Token, $"gifs downloading error [{URL}]")
            End Try
        End Sub
#End Region
#Region "Download photo"
        Private Function CreatePhotoFile(ByVal URL As String, ByVal File As SFile) As SFile
            Dim pFile$ = RegexReplace(URL, Regex_Photo_File)
            If Not pFile.IsEmptyString Then Return New SFile(pFile) Else Return File
        End Function
        Private Const PhotoUrlPattern_PornHub As String = "https://www.pornhub.com/{0}/{1}/photos"
        Private Sub DownloadUserPhotos(ByVal Token As CancellationToken)
            Try
                If IsSavedPosts Then
                    DownloadUserPhotos_SavedPosts(Token)
                Else
                    DownloadUserPhotos_PornHub(Token)
                End If
                ThrowAny(Token)
            Catch ex As Exception
                ProcessException(ex, Token, "photos downloading error")
            End Try
        End Sub
        Private Overloads Function DownloadUserPhotos_PornHub(ByVal Token As CancellationToken) As Boolean
            Try
                Dim albumName$
                Dim page%
                Dim r$ = Responser.GetResponse(String.Format(PhotoUrlPattern_PornHub, PersonType, NameTrue))
                If Not r.IsEmptyString Then
                    Dim l As List(Of PhotoBlock) = RegexFields(Of PhotoBlock)(r, {Regex_Photo_PornHub_PhotoBlocks}, {2, 1}, EDP.ReturnValue)
                    l.ListAddList(RegexFields(Of PhotoBlock)(r, {Regex_Photo_PornHub_PhotoBlocks2}, {1, 2}, EDP.ReturnValue))
                    If l.ListExists Then l.RemoveAll(Function(ll) ll.AlbumID.IsEmptyString)
                    If l.ListExists Then
                        ProgressPre.ChangeMax(l.Count)
                        For Each block As PhotoBlock In l
                            ProgressPre.Perform()
                            If Not _TempPostsList.Contains(block.AlbumID) Then _TempPostsList.Add(block.AlbumID) Else Continue For
                            albumName = block.Data
                            If albumName.IsEmptyString Then
                                albumName = block.AlbumID.Split("/").LastOrDefault.StringTrim
                            Else
                                albumName = TitleHtmlConverter(albumName)
                            End If
                            page = 1
                            Do While DownloadUserPhotos_PornHub(page, block.AlbumID, albumName, Token) : page += 1 : Loop
                        Next
                        l.Clear()
                    End If
                End If
                Return True
            Catch ex As Exception
                ThrowAny(Token)
                Return False
            End Try
        End Function
        Private Function DownloadUserPhotos_PornHub_ParseSinglePhoto(ByVal r As String) As String
            Dim url$ = String.Empty
            With DirectCast(RegexReplace(r, Regex_Photo_PornHub_SinglePhoto), List(Of String))
                If .ListExists(3) Then url = .Item(2).IfNullOrEmpty(.Item(1)).StringTrim
            End With
            If url.IsEmptyString Then url = RegexReplace(r, Regex_Photo_PornHub_SinglePhoto2)
            Return url
        End Function
        Private Overloads Function DownloadUserPhotos_PornHub(ByVal Page As Integer, ByVal AlbumID As String, ByVal AlbumName As String,
                                                              ByVal Token As CancellationToken) As Boolean
            Try
                Dim r$ = Responser.GetResponse($"https://www.pornhub.com{AlbumID}{IIf(Page = 1, String.Empty, $"?page={Page}")}")
                If Not r.IsEmptyString Then
                    Dim l As List(Of String) = RegexReplace(r, Regex_Photo_PornHub_AlbumPhotoArr)
                    If l.ListExists Then l.RemoveAll(Function(_url) _url.IsEmptyString)
                    If l.ListExists Then
                        ProgressPre.ChangeMax(l.Count)
                        For Each url$ In l
                            ProgressPre.Perform()
                            ThrowAny(Token)
                            Try
                                r = Responser.GetResponse(url)
                                If Not r.IsEmptyString Then
                                    url = DownloadUserPhotos_PornHub_ParseSinglePhoto(r)
                                    If Not url.IsEmptyString Then _
                                       _TempMediaList.ListAddValue(New UserMedia(url, UTypes.Picture) With {
                                                                   .SpecialFolder = $"Albums\{AlbumName}\",
                                                                   .File = CreatePhotoFile(url, .File)}, LNC)
                                End If
                            Catch
                            End Try
                        Next
                        l.Clear()
                        Return True
                    End If
                End If
                Return False
            Catch ex As Exception
                ThrowAny(Token)
                Return False
            End Try
        End Function
        Private Function DownloadUserPhotos_SavedPosts(ByVal Token As CancellationToken) As Boolean
            Const HtmlPageNotFoundPhoto$ = "Page Not Found"
            Dim URL$ = $"https://www.pornhub.com/{PersonType}/{NameTrue}/photos/favorites"
            Try
                Dim r$ = Responser.GetResponse(URL)
                If Not r.IsEmptyString Then
                    If r.Contains(HtmlPageNotFoundPhoto) Then Return False
                    Dim urls As List(Of String) = RegexReplace(r, Regex_Photo_PornHub_AlbumPhotoArr)
                    If urls.ListExists Then
                        Dim NewUrl$, pFile$
                        Dim m As UserMedia
                        Dim l2 As List(Of UserMedia) = urls.Select(Function(__url) New UserMedia(__url, UTypes.Picture) With {
                                                                                                 .Post = __url.Split("/").LastOrDefault}).ToList
                        urls.Clear()
                        If l2.ListExists Then l2.RemoveAll(Function(media) media.URL.IsEmptyString)
                        If l2.ListExists Then
                            Dim lBefore% = l2.Count
                            If _TempPostsList.Count > 0 Then l2.RemoveAll(Function(media) _TempPostsList.Contains(media.Post.ID))
                            If l2.Count > 0 Then
                                ProgressPre.ChangeMax(l2.Count)
                                For i% = 0 To l2.Count - 1
                                    ProgressPre.Perform()
                                    m = l2(i)
                                    ThrowAny(Token)
                                    Try
                                        r = Responser.GetResponse(m.URL)
                                        If Not r.IsEmptyString Then
                                            NewUrl = DownloadUserPhotos_PornHub_ParseSinglePhoto(r)
                                            If Not NewUrl.IsEmptyString Then
                                                m.URL = NewUrl
                                                pFile = RegexReplace(NewUrl, Regex_Photo_File)
                                                If Not pFile.IsEmptyString Then m.File = pFile Else m.File = NewUrl
                                                _TempPostsList.ListAddValue(m.Post.ID, LNC)
                                            Else
                                                Throw New Exception
                                            End If
                                        End If
                                    Catch
                                        m.State = UserMedia.States.Missing
                                    End Try
                                    _TempMediaList.ListAddValue(m, LNC)
                                Next
                            End If
                            Return l2.Count = lBefore
                        End If
                    End If
                End If
                Return False
            Catch ex As Exception
                Return ProcessException(ex, Token, $"photos downloading error [{URL}]") = 1
            End Try
        End Function
#End Region
#End Region
#Region "ReparseVideo"
        Protected Overloads Overrides Sub ReparseVideo(ByVal Token As CancellationToken)
            If IsSubscription Then
                ReparseVideoSubscriptions(Token)
            Else
                ReparseVideo(Token, False)
            End If
        End Sub
        Private Overloads Sub ReparseVideo(ByVal Token As CancellationToken, ByVal CreateFileName As Boolean,
                                           Optional ByRef Data As IYouTubeMediaContainer = Nothing)
            Const ERR_NEW_URL$ = "ERR_NEW_URL"
            Dim URL$ = String.Empty
            Try
                If _TempMediaList.Count > 0 AndAlso _TempMediaList.Exists(Function(tm) tm.Type = UTypes.VideoPre) Then
                    Dim m As UserMedia
                    Dim r$, NewUrl$, tmpName$
                    ProgressPre.ChangeMax(_TempMediaList.Count)
                    For i% = _TempMediaList.Count - 1 To 0 Step -1
                        ProgressPre.Perform()
                        If _TempMediaList(i).Type = UTypes.VideoPre Then
                            m = _TempMediaList(i)
                            ThrowAny(Token)
                            Try
                                URL = m.URL
                                r = Responser.Curl(URL)
                                If Not r.IsEmptyString Then
                                    NewUrl = CreateVideoURL(r)
                                    If NewUrl.IsEmptyString Then
                                        Throw New Exception With {.HelpLink = ERR_NEW_URL}
                                    Else
                                        m.URL = NewUrl
                                        m.Type = UTypes.m3u8
                                        If CreateFileName Then
                                            tmpName = RegexReplace(r, RegexVideoPageTitle)
                                            If Not tmpName.IsEmptyString Then
                                                If Not Data Is Nothing Then Data.Title = tmpName
                                                m.File.Name = TitleHtmlConverter(tmpName)
                                                m.File.Extension = "mp4"
                                            End If
                                        End If
                                        _TempMediaList(i) = m
                                    End If
                                Else
                                    _TempMediaList.RemoveAt(i)
                                End If
                            Catch mid_ex As Exception
                                If mid_ex.HelpLink = ERR_NEW_URL OrElse DownloadingException(mid_ex, "") = 1 Then
                                    m.State = UserMedia.States.Missing
                                    _TempMediaList(i) = m
                                Else
                                    _TempMediaList.RemoveAt(i)
                                End If
                            End Try
                        End If
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex, Token, "video reparsing error", False)
            End Try
        End Sub
        Private Sub ReparseVideoSubscriptions(ByVal Token As CancellationToken)
            Try
                If _TempMediaList.Count > 0 AndAlso _TempMediaList.Exists(Function(tm) tm.Type = UTypes.VideoPre) Then
                    Dim m As UserMedia
                    Dim r$, URL$, tmpName$, thumb$
                    Dim c% = 0
                    Dim rErr As New ErrorsDescriber(EDP.ReturnValue)
                    Progress.Maximum += _TempMediaList.Count
                    For i% = _TempMediaList.Count - 1 To 0 Step -1
                        Progress.Perform()
                        If _TempMediaList(i).Type = UTypes.VideoPre Then
                            If Not DownloadTopCount.HasValue OrElse c <= DownloadTopCount.Value Then
                                m = _TempMediaList(i)
                                ThrowAny(Token)
                                Try
                                    URL = m.URL_BASE
                                    r = Responser.GetResponse(URL,, rErr)
                                    If Not r.IsEmptyString Then
                                        m.Type = UTypes.m3u8

                                        thumb = RegexReplace(r, Regex_VideosThumb_OG_IMAGE)
                                        If Not thumb.IsEmptyString Then m.URL = thumb

                                        tmpName = RegexReplace(r, RegexVideoPageTitle)
                                        If Not tmpName.IsEmptyString Then
                                            m.File.Name = TitleHtmlConverter(tmpName)
                                            m.File.Extension = "mp4"
                                            m.PictureOption = tmpName
                                        End If

                                        _TempMediaList(i) = m
                                        c += 1
                                    Else
                                        _TempMediaList.RemoveAt(i)
                                    End If
                                Catch mid_ex As Exception
                                    _TempMediaList.RemoveAt(i)
                                End Try
                            Else
                                _TempMediaList.RemoveAt(i)
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex, Token, "subscriptions video reparsing error", False)
            End Try
        End Sub
#End Region
#Region "ReparseMissing"
        Protected Overrides Sub ReparseMissing(ByVal Token As CancellationToken)
            Dim rList As New List(Of Integer)
            Try
                If ContentMissingExists Then
                    Dim m As UserMedia
                    Dim r$
                    Dim eCurl As New ErrorsDescriber(EDP.ReturnValue)
                    ProgressPre.ChangeMax(_ContentList.Count)
                    For i% = 0 To _ContentList.Count - 1
                        ProgressPre.Perform()
                        m = _ContentList(i)
                        If m.State = UserMedia.States.Missing AndAlso Not m.URL_BASE.IsEmptyString Then
                            ThrowAny(Token)
                            r = Responser.Curl(m.URL_BASE,, eCurl)
                            If Not r.IsEmptyString Then
                                Dim NewUrl$ = CreateVideoURL(r)
                                If Not NewUrl.IsEmptyString Then
                                    m.URL = NewUrl
                                    _TempMediaList.ListAddValue(m, LNC)
                                    rList.Add(i)
                                End If
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
#Region "Download content"
        Private UpdateM3U8URLS As Boolean = False
        Private UpdateM3U8URLS_Error As Boolean = False
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            Try : DownloadContentDefault(Token) : Finally : UpdateM3U8URLS = False : End Try
        End Sub
        Protected Overloads Overrides Function DownloadM3U8(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile,
                                                            ByVal Token As CancellationToken) As SFile
            UpdateM3U8URLS_Error = False
            Return DownloadM3U8(URL, Media, DestinationFile, Token, UpdateM3U8URLS)
        End Function
        Private Overloads Function DownloadM3U8(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile,
                                                ByVal Token As CancellationToken, ByVal Second As Boolean) As SFile
            Try
                If Second Then
                    Dim r$ = Responser.Curl(Media.URL_BASE,, EDP.ReturnValue)
                    If Not r.IsEmptyString Then Media.URL = CreateVideoURL(r).IfNullOrEmpty(URL) : URL = Media.URL
                End If
                Dim f As SFile = M3U8.Download(URL, Responser, DestinationFile, DownloadUHD, Token, Progress, Not IsSingleObjectDownload)
                If Not f.Exists And Not Second Then UpdateM3U8URLS = True : f = DownloadM3U8(URL, Media, DestinationFile, Token, True)
                Return f
            Catch ex As Exception
                If Not UpdateM3U8URLS_Error Then
                    UpdateM3U8URLS_Error = True
                    Thread.Sleep(1000)
                    Return DownloadM3U8(URL, Media, DestinationFile, Token, True)
                End If
                Return Nothing
            End Try
        End Function
#End Region
#Region "CreateVideoURL"
        Private Function CreateVideoURL(ByVal r As String) As String
            If r.IsEmptyString Then
                Return String.Empty
            Else
                Dim u$ = CreateVideoURL_FlashVars(r)
                If u.IsEmptyString Then u = CreateVideoURL_MediaDef(r)
                Return u
            End If
        End Function
        Private Function CreateVideoURL_FlashVars(ByVal r As String) As String
            Try
                Dim OutStr$ = String.Empty
                Dim OutList As New List(Of String)
                Dim tmpUrl$
                Dim i%
                If Not r.IsEmptyString Then
                    Dim _VarBlock$, var$, v$
                    Dim vars As List(Of FlashVar)
                    Dim compiler As List(Of String)
                    Dim _VarBlocks As List(Of String) = RegexReplace(r, RegexVideo_FlashVarsBlocks)
                    If _VarBlocks.ListExists Then
                        For Each _VarBlock In _VarBlocks
                            tmpUrl = String.Empty
                            vars = RegexFields(Of FlashVar)(_VarBlock, {RegexVideo_FlashVars_Vars}, {1, 2})
                            compiler = RegexReplace(_VarBlock, RegexVideo_FlashVars_Compiler)
                            If vars.ListExists And compiler.ListExists Then
                                For Each var In compiler
                                    i = vars.IndexOf(var)
                                    If i >= 0 Then
                                        v = vars(i).Value
                                        If Not v.IsEmptyString Then tmpUrl &= v
                                    End If
                                Next
                                vars.Clear()
                                compiler.Clear()
                            End If
                            If Not tmpUrl.IsEmptyString Then OutList.Add(tmpUrl)
                        Next
                    End If
                End If

                If OutList.Count > 0 Then OutList.RemoveAll(Function(u) u.IsEmptyString)
                If OutList.Count > 0 Then
                    i = OutList.FindIndex(Function(u) u.Contains("urlset"))
                    If i >= 0 Then
                        OutStr = OutList(i)
                    Else
                        Dim newUrls As New List(Of Sizes)
                        Dim tmpSize%?
                        For Each tmpUrl In OutList
                            tmpSize = AConvert(Of Integer)(RegexReplace(tmpUrl, RegexVideo_FlashVars_UrlResolution), AModes.Var, Nothing)
                            If tmpSize.HasValue Then newUrls.Add(New Sizes(tmpSize.Value, tmpUrl))
                        Next
                        If newUrls.Count > 0 Then
                            newUrls.Sort()
                            OutStr = newUrls(0).Data
                            newUrls.Clear()
                        Else
                            OutStr = OutList(0)
                        End If
                    End If
                End If
                OutList.Clear()
                Return OutStr
            Catch regex_ex As RegexFieldsTextBecameNullException
                MyMainLOG = $"{ToStringForLog()}: something is wrong when parsing flashvars.{vbCr}{regex_ex.Message}"
                Return String.Empty
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "[API.PornHub.UserData.CreateVideoURL_FlashVars]", String.Empty)
            End Try
        End Function
        Private Function CreateVideoURL_MediaDef(ByVal r As String) As String
            Try
                Dim result$ = String.Empty
                If Not r.IsEmptyString Then
                    Dim script$ = RegexReplace(r, RegexVideo_MediaDef)
                    If Not script.IsEmptyString Then
                        Using j As EContainer = JsonDocument.Parse(script)
                            If j.ListExists Then
                                Dim s As List(Of Sizes) = j.Select(Function(jj) New Sizes(jj.Value("quality"), jj.Value("videoUrl"))).ListWithRemove(Function(d) d.HasError Or d.Data.IsEmptyString)
                                If s.ListExists Then s.Sort() : result = s(0).Data : s.Clear()
                            End If
                        End Using
                    End If
                End If
                Return result
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "[API.PornHub.UserData.CreateVideoURL_MediaDef]", String.Empty)
            End Try
        End Function
#End Region
#Region "DownloadSingleObject"
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            UpdateM3U8URLS = False
            _TempMediaList.Add(New UserMedia(Data.URL, UTypes.VideoPre))
            ReparseVideo(Token, True, Data)
        End Sub
        Protected Overrides Sub DownloadSingleObject_PostProcessing(ByVal Data As IYouTubeMediaContainer, Optional ByVal ResetTitle As Boolean = True)
            MyBase.DownloadSingleObject_PostProcessing(Data, False)
        End Sub
#End Region
#Region "Exceptions"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String,
                                                          Optional ByVal FromPE As Boolean = False, Optional ByVal EObj As Object = Nothing) As Integer
            If Responser.Status = Net.WebExceptionStatus.ConnectionClosed Then
                Return 1
            ElseIf Responser.StatusCode = Net.HttpStatusCode.ServiceUnavailable Then
                Return 2
            Else
                Return 0
            End If
        End Function
#End Region
#Region "IDisposable Support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then SessionPosts.Clear()
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace