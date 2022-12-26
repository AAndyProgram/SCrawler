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
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace API.PornHub
    Friend Class UserData : Inherits UserDataBase
        Private Const UrlPattern As String = "https://www.pornhub.com/{0}"
#Region "Declarations"
#Region "XML names"
        Private Const Name_PersonType As String = "PersonType"
        Private Const Name_NameTrue As String = "NameTrue"
        Private Const Name_VideoPageModel As String = "VideoPageModel"
        Private Const Name_PhotoPageModel As String = "PhotoPageModel"
        Private Const Name_DownloadGifs As String = "DownloadGifs"
        Private Const Name_DownloadPhotoOnlyFromModelHub As String = "DownloadPhotoOnlyFromModelHub"
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
            Friend Function ToUserMedia() As UserMedia
                Return New UserMedia(URL, UTypes.VideoPre) With {
                    .File = If(Title.IsEmptyString, .File, New SFile($"{Title}.mp4")),
                    .Post = ID
                }
            End Function
            Private Function CreateFromArray(ByVal ParamsArray() As String) As Object Implements IRegExCreator.CreateFromArray
                If ParamsArray.ListExists Then
                    URL = ParamsArray(0)
                    ID = RegexReplace(URL, RegexVideo_Video_VideoKey)
                    URL = String.Format(UrlPattern, URL.TrimStart("/"))
                    Title = HtmlConverter(ParamsArray(1)).StringRemoveWinForbiddenSymbols.StringTrim
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
        Friend Enum VideoPageModels As Integer
            [Default] = 0
            ConcatPage = 1
            Favorite = 2
            Undefined = -1
        End Enum
        Private Enum PhotoPageModels As Integer
            Undefined = 0
            PornHubPage = 1
            ModelHubPage = 2
        End Enum
#End Region
#Region "Constants"
        Private Const PersonTypeModel As String = "model"
        Friend Const PersonTypeUser As String = "users"
#End Region
#Region "Person"
        Friend Property PersonType As String
        Friend Property NameTrue As String
        Private _FriendlyName As String = String.Empty
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
        Friend Property VideoPageModel As VideoPageModels = VideoPageModels.Undefined
        Private Property PhotoPageModel As PhotoPageModels = PhotoPageModels.Undefined
        Friend Property DownloadGifs As Boolean
        Friend Property DownloadPhotoOnlyFromModelHub As Boolean = True
#End Region
#Region "ExchangeOptions"
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return New UserExchangeOptions(Me)
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            If Not Obj Is Nothing AndAlso TypeOf Obj Is UserExchangeOptions Then
                With DirectCast(Obj, UserExchangeOptions)
                    DownloadGifs = .DownloadGifs
                    DownloadPhotoOnlyFromModelHub = .DownloadPhotoOnlyFromModelHub
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
#Region "Initializer, loader"
        Friend Sub New()
            UseInternalM3U8Function = True
        End Sub
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            With Container
                Dim SetNames As Action = Sub()
                                             If Not Name.IsEmptyString And NameTrue.IsEmptyString Then
                                                 Dim n$() = Name.Split("_")
                                                 If n.ListExists(2) Then
                                                     NameTrue = Name.Replace($"{n(0)}_", String.Empty)
                                                     PersonType = n(0)
                                                     If (PersonType = PersonTypeModel Or PersonType = PersonTypeUser) And
                                                        VideoPageModel = VideoPageModels.Undefined Then VideoPageModel = VideoPageModels.Default
                                                 End If
                                             End If
                                         End Sub
                If Loading Then
                    PersonType = .Value(Name_PersonType)
                    NameTrue = .Value(Name_NameTrue)
                    VideoPageModel = .Value(Name_VideoPageModel).FromXML(Of Integer)(VideoPageModels.Undefined)
                    PhotoPageModel = .Value(Name_PhotoPageModel).FromXML(Of Integer)(PhotoPageModels.Undefined)
                    DownloadGifs = .Value(Name_DownloadGifs).FromXML(Of Integer)(False)
                    DownloadPhotoOnlyFromModelHub = .Value(Name_DownloadPhotoOnlyFromModelHub).FromXML(Of Boolean)(True)
                    SetNames.Invoke()
                Else
                    SetNames.Invoke()
                    .Add(Name_PersonType, PersonType)
                    .Add(Name_NameTrue, NameTrue)
                    .Add(Name_VideoPageModel, CInt(VideoPageModel))
                    .Add(Name_PhotoPageModel, CInt(PhotoPageModel))
                    .Add(Name_DownloadGifs, DownloadGifs.BoolToInteger)
                    .Add(Name_DownloadPhotoOnlyFromModelHub, DownloadPhotoOnlyFromModelHub.BoolToInteger)
                End If
            End With
        End Sub
#End Region
#Region "Downloading"
#Region "Download override"
        Private Const DataDownloaded As Integer = -10
        Private Const DataDownloaded_NotFound As Integer = -20
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            Try
                Responser.ResetStatus()
                If PersonType = PersonTypeUser Then Responser.Mode = Responser.Modes.Curl

                If IsSavedPosts Then VideoPageModel = VideoPageModels.Favorite

                Dim page% = 1
                Dim __continue As Boolean = True
                Dim __videoDone As Boolean = False
                Dim d%
                If DownloadVideos Then
                    If PersonType = PersonTypeUser Then Responser.Mode = Responser.Modes.Curl : Responser.Method = "POST"
                    If VideoPageModel = VideoPageModels.Undefined Then
                        __continue = False
                        d = DownloadUserVideos(page, Token)
                        Select Case d
                            Case DataDownloaded : __continue = True : page += 1
                            Case 1 : VideoPageModel = VideoPageModels.ConcatPage
                            Case EXCEPTION_OPERATION_CANCELED : ThrowAny(Token)
                            Case DataDownloaded_NotFound : __videoDone = True
                        End Select
                        If Not __continue And Not __videoDone Then
                            d = DownloadUserVideos(page, Token)
                            Select Case d
                                Case DataDownloaded : __continue = True : page += 1
                                Case 1 : VideoPageModel = VideoPageModels.Undefined
                                Case EXCEPTION_OPERATION_CANCELED : ThrowAny(Token)
                                Case DataDownloaded_NotFound : __videoDone = True
                            End Select
                        End If
                    End If
                    If __continue And Not __videoDone Then
                        Do While DownloadUserVideos(page, Token) = DataDownloaded And page < 100 : page += 1 : Loop
                    End If
                End If

                Responser.Method = "GET"
                If DownloadGifs And Not IsSavedPosts Then DownloadUserGifs(Token)
                If DownloadImages Then DownloadUserPhotos(Token)
            Finally
                Responser.Mode = Responser.Modes.Default
                Responser.Method = "GET"
            End Try
        End Sub
#End Region
#Region "Download video"
        Private ReadOnly Property VideoPageType As String
            Get
                Select Case VideoPageModel
                    Case VideoPageModels.Default : Return "/videos/upload"
                    Case VideoPageModels.Favorite : Return "/videos/favorites/"
                    Case Else : Return String.Empty
                End Select
            End Get
        End Property
        Private ReadOnly Property VideoPageAppender As String
            Get
                Return If(PersonType = PersonTypeUser, "ajax?o=newest&page=", String.Empty)
            End Get
        End Property
        Private Overloads Function DownloadUserVideos(ByVal Page As Integer, ByVal Token As CancellationToken) As Integer
            Const VideoUrlPattern$ = "https://www.pornhub.com/{0}/{1}{2}{3}"
            Const HtmlPageNotFoundVideo$ = "<span>Error Page Not Found</span>"
            Dim URL$ = String.Empty
            Try
                Dim p$
                If PersonType = PersonTypeUser Then
                    p = Page
                Else
                    p = IIf(Page = 1, String.Empty, $"?page={Page}")
                End If

                URL = $"{String.Format(VideoUrlPattern, PersonType, NameTrue, VideoPageType, VideoPageAppender)}{p}"
                ThrowAny(Token)

                Dim r$ = Responser.GetResponse(URL)
                If Not r.IsEmptyString Then
                    If PersonType = PersonTypeUser And r.Contains(HtmlPageNotFoundVideo) Then Return DataDownloaded_NotFound
                    Dim l As List(Of UserVideo) = RegexFields(Of UserVideo)(r, {RegexVideo_Video_All}, {1, 2})
                    Dim lw As List(Of UserVideo) = Nothing
                    If Not PersonType = PersonTypeUser Then RegexFields(Of UserVideo)(r, {RegexVideo_Video_Wrong}, RegexVideo_Video_Wrong_Fields)
                    If l.ListExists Then
                        If lw.ListExists Then l.ListWithRemove(lw)
                        If l.Count > 0 Then
                            Dim lBefore% = l.Count
                            l.RemoveAll(Function(ByVal uv As UserVideo) As Boolean
                                            If Not _TempPostsList.Contains(uv.ID) Then
                                                _TempPostsList.Add(uv.ID)
                                                Return False
                                            Else
                                                Return True
                                            End If
                                        End Function)
                            If l.Count > 0 Then _TempMediaList.ListAddList(l.Select(Function(uv) uv.ToUserMedia))
                            If l.Count = lBefore And l.Count > 0 Then Return DataDownloaded
                        End If
                    End If
                End If
                Return DataDownloaded_NotFound
            Catch regex_ex As RegexFieldsTextBecameNullException
                If PersonType = PersonTypeUser Or IsSavedPosts Then
                    Return DataDownloaded_NotFound
                Else
                    Return ProcessException(regex_ex, Token, $"videos downloading error [{URL}]")
                End If
            Catch ex As Exception
                Return ProcessException(ex, Token, $"videos downloading error [{URL}]")
            End Try
        End Function
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
                    Dim l As List(Of RegexMatchStruct) = RegexFields(Of RegexMatchStruct)(r, {Regex_Gif_Array}, {1})
                    Dim l2 As List(Of String) = Nothing
                    Dim l3 As List(Of String) = Nothing
                    If l.ListExists Then l2 = l.Select(Function(ll) $"gif/{ll.Arr(0).Replace("gif", String.Empty)}").ToList
                    If l2.ListExists Then
                        For Each gif$ In l2
                            If Not _TempPostsList.Contains(gif) Then
                                _TempPostsList.Add(gif)
                                URL = $"https://www.pornhub.com/{gif}"
                                m = New UserMedia(URL, UTypes.Video) With {.Post = gif, .SpecialFolder = "GIFs\"}
                                ThrowAny(Token)
                                Try
                                    r = Responser.GetResponse(URL)
                                    If Not r.IsEmptyString Then
                                        If l3.ListExists Then l3.Clear() : l3 = Nothing
                                        l3 = RegexReplace(r, Regex_Gif_UrlName)
                                        If l3.ListExists(3) Then
                                            m.URL = l3(2)
                                            m.File = m.URL
                                            n = HtmlConverter(l3(1)).StringRemoveWinForbiddenSymbols.StringTrim
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
        Private Const PhotoUrlPattern_ModelHub As String = "https://www.modelhub.com/{0}/photos"
        Private Const PhotoUrlPattern_PornHub As String = "https://www.pornhub.com/{0}/{1}/photos"
        Private Sub DownloadUserPhotos(ByVal Token As CancellationToken)
            Try
                If IsSavedPosts Then
                    DownloadUserPhotos_SavedPosts(Token)
                ElseIf PersonType = PersonTypeModel Then
                    If PhotoPageModel = PhotoPageModels.Undefined Then
                        If DownloadUserPhotos_ModelHub(Token) Then PhotoPageModel = PhotoPageModels.ModelHubPage
                        ThrowAny(Token)
                        If PhotoPageModel = PhotoPageModels.Undefined AndAlso Not DownloadPhotoOnlyFromModelHub AndAlso
                           DownloadUserPhotos_PornHub(Token) Then PhotoPageModel = PhotoPageModels.PornHubPage
                    Else
                        Select Case PhotoPageModel
                            Case PhotoPageModels.ModelHubPage : DownloadUserPhotos_ModelHub(Token)
                            Case PhotoPageModels.PornHubPage : If Not DownloadPhotoOnlyFromModelHub Then DownloadUserPhotos_PornHub(Token)
                        End Select
                    End If
                ElseIf Not DownloadPhotoOnlyFromModelHub Then
                    DownloadUserPhotos_PornHub(Token)
                End If
                ThrowAny(Token)
            Catch ex As Exception
                ProcessException(ex, Token, "photos downloading error")
            End Try
        End Sub
        Private Function DownloadUserPhotos_ModelHub(ByVal Token As CancellationToken) As Boolean
            Dim URL$ = String.Empty
            Try
                Dim jErr As New ErrorsDescriber(EDP.SendInLog + EDP.ReturnValue)
                Dim albumName$
                If PersonType = PersonTypeModel Then
                    URL = String.Format(PhotoUrlPattern_ModelHub, NameTrue)
                    Dim r$ = Responser.GetResponse(URL)
                    If Not r.IsEmptyString Then
                        Dim l As List(Of PhotoBlock) = RegexFields(Of PhotoBlock)(r, {Regex_Photo_ModelHub_PhotoBlocks}, {1, 2})
                        If l.ListExists Then l.RemoveAll(Function(ll) ll.Data.IsEmptyString)
                        If l.ListExists Then
                            Dim albumRegex As RParams = RParams.DMS("", 1, EDP.ReturnValue)
                            For Each block As PhotoBlock In l
                                If Not _TempPostsList.Contains(block.AlbumID) Then _TempPostsList.Add(block.AlbumID) Else Continue For
                                albumRegex.Pattern = "<li id=""" & block.AlbumID & """ class=""modelBox"">[\r\n\s]*?<div class=""modelPhoto"">[\r\n\s]*?\<[^\>]*?alt=""([^""]*)"""
                                albumName = StringTrim(RegexReplace(r, albumRegex))
                                If albumName.IsEmptyString Then albumName = block.AlbumID
                                Using j As EContainer = JsonDocument.Parse("{" & block.Data & "}", jErr)
                                    If Not j Is Nothing Then
                                        If If(j("urls")?.Count, 0) > 0 Then
                                            _TempMediaList.ListAddList(j("urls").Select(Function(jj) _
                                                                       New UserMedia(jj.ItemF({0}).XmlIfNothingValue, UTypes.Picture) With {
                                                                                     .SpecialFolder = $"Albums\{albumName}\"}), LNC)
                                        End If
                                    End If
                                End Using
                            Next
                            l.Clear()
                        End If
                    End If
                End If
                Return True
            Catch ex As Exception
                ThrowAny(Token)
                Return False
            End Try
        End Function
        Private Overloads Function DownloadUserPhotos_PornHub(ByVal Token As CancellationToken) As Boolean
            Try
                Dim albumName$
                Dim page%
                Dim r$ = Responser.GetResponse(String.Format(PhotoUrlPattern_PornHub, PersonType, NameTrue))
                If Not r.IsEmptyString Then
                    Dim l As List(Of PhotoBlock) = RegexFields(Of PhotoBlock)(r, {Regex_Photo_PornHub_PhotoBlocks}, {2, 1})
                    If l.ListExists Then l.RemoveAll(Function(ll) ll.AlbumID.IsEmptyString)
                    If l.ListExists Then
                        For Each block As PhotoBlock In l
                            If Not _TempPostsList.Contains(block.AlbumID) Then _TempPostsList.Add(block.AlbumID) Else Continue For
                            albumName = block.Data
                            If albumName.IsEmptyString Then
                                albumName = block.AlbumID.Split("/").LastOrDefault.StringTrim
                            Else
                                albumName = HtmlConverter(albumName).StringRemoveWinForbiddenSymbols.StringTrim
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
        Private Overloads Function DownloadUserPhotos_PornHub(ByVal Page As Integer, ByVal AlbumID As String, ByVal AlbumName As String,
                                                              ByVal Token As CancellationToken) As Boolean
            Try
                Dim r$ = Responser.GetResponse($"https://www.pornhub.com{AlbumID}{IIf(Page = 1, String.Empty, $"?page={Page}")}")
                If Not r.IsEmptyString Then
                    Dim l As List(Of String) = RegexReplace(r, Regex_Photo_PornHub_AlbumPhotoArr)
                    If l.ListExists Then l.RemoveAll(Function(_url) _url.IsEmptyString)
                    If l.ListExists Then
                        For Each url$ In l
                            ThrowAny(Token)
                            Try
                                r = Responser.GetResponse(url)
                                If Not r.IsEmptyString Then
                                    url = RegexReplace(r, Regex_Photo_PornHub_SinglePhoto)
                                    If Not url.IsEmptyString Then _
                                       _TempMediaList.ListAddValue(New UserMedia(url, UTypes.Picture) With {.SpecialFolder = $"Albums\{AlbumName}\"}, LNC)
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
                        Dim NewUrl$
                        Dim m As UserMedia
                        Dim l2 As List(Of UserMedia) = urls.Select(Function(__url) New UserMedia(__url, UTypes.Picture) With {
                                                                                                 .Post = __url.Split("/").LastOrDefault}).ToList
                        urls.Clear()
                        If l2.ListExists Then l2.RemoveAll(Function(media) media.URL.IsEmptyString)
                        If l2.ListExists Then
                            Dim lBefore% = l2.Count
                            If _TempPostsList.Count > 0 Then l2.RemoveAll(Function(media) _TempPostsList.Contains(media.Post.ID))
                            If l2.Count > 0 Then
                                For i% = 0 To l2.Count - 1
                                    m = l2(i)
                                    ThrowAny(Token)
                                    Try
                                        r = Responser.GetResponse(m.URL)
                                        If Not r.IsEmptyString Then
                                            NewUrl = RegexReplace(r, Regex_Photo_PornHub_SinglePhoto)
                                            If Not NewUrl.IsEmptyString Then
                                                m.URL = NewUrl
                                                m.File = NewUrl
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
                Return ProcessException(ex, Token, $"photos downloading error [{URL}]")
            End Try
        End Function
#End Region
#End Region
#Region "ReparseVideo"
        Protected Overrides Sub ReparseVideo(ByVal Token As CancellationToken)
            Const ERR_NEW_URL$ = "ERR_NEW_URL"
            Dim URL$ = String.Empty
            Try
                If _TempMediaList.Count > 0 AndAlso _TempMediaList.Exists(Function(tm) tm.Type = UTypes.VideoPre) Then
                    Dim m As UserMedia
                    Dim r$, NewUrl$
                    For i% = _TempMediaList.Count - 1 To 0 Step -1
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
#End Region
#Region "ReparseMissing"
        Protected Overrides Sub ReparseMissing(ByVal Token As CancellationToken)
            Dim rList As New List(Of Integer)
            Try
                If ContentMissingExists Then
                    Dim m As UserMedia
                    Dim r$
                    Dim eCurl As New ErrorsDescriber(EDP.ReturnValue)
                    For i% = 0 To _ContentList.Count - 1
                        m = _ContentList(i)
                        If m.State = UserMedia.States.Missing AndAlso Not m.URL_BASE.IsEmptyString Then
                            ThrowAny(Token)
                            r = Responser.Curl(m.URL_BASE, eCurl)
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
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
        Protected Overrides Function DownloadM3U8(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile) As SFile
            Return M3U8.Download(URL, Responser, DestinationFile)
        End Function
#End Region
#Region "CreateVideoURL"
        Private Shared Function CreateVideoURL(ByVal r As String) As String
            Try
                Dim OutStr$ = String.Empty
                If Not r.IsEmptyString Then
                    Dim _VarBlock$ = RegexReplace(r, RegexVideo_FlashVarsBlock)
                    If Not _VarBlock.IsEmptyString Then
                        Dim vars As List(Of FlashVar) = RegexFields(Of FlashVar)(_VarBlock, {RegexVideo_FlashVars_Vars}, {1, 2})
                        Dim compiler As List(Of String) = RegexReplace(_VarBlock, RegexVideo_FlashVars_Compiler)
                        If vars.ListExists And compiler.ListExists Then
                            Dim v$
                            Dim i%
                            For Each var$ In compiler
                                i = vars.IndexOf(var)
                                If i >= 0 Then
                                    v = vars(i).Value
                                    If Not v.IsEmptyString Then OutStr &= v
                                End If
                            Next
                        End If
                    End If
                End If
                Return OutStr
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendInLog, ex, "[API.PornHub.UserData.CreateVideoURL]", String.Empty)
            End Try
        End Function
#End Region
#Region "Standalone downloader"
        Friend Shared Function GetVideoInfo(ByVal URL As String, ByVal Responser As Responser, ByVal Destination As SFile) As UserMedia
            Try
                Dim r$ = Responser.Curl(URL)
                If Not r.IsEmptyString Then
                    Dim NewUrl$ = CreateVideoURL(r)
                    If Not NewUrl.IsEmptyString Then
                        Dim f As SFile = M3U8.Download(NewUrl, Responser, Destination)
                        If Not f.IsEmptyString Then Return New UserMedia With {.State = UserMedia.States.Downloaded}
                    End If
                End If
                Return Nothing
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendInLog + EDP.ReturnValue, ex, $"PornHub standalone download error: [{URL}]", New UserMedia)
            End Try
        End Function
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
    End Class
End Namespace