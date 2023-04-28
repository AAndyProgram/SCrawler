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
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.Web.Documents.JSON
Namespace API.ThisVid
    Friend Class UserData : Inherits UserDataBase
#Region "XML names"
        Private Const Name_DownloadPublic As String = "DownloadPublic"
        Private Const Name_DownloadPrivate As String = "DownloadPrivate"
        Private Const Name_DifferentFolders As String = "DifferentFolders"
#End Region
#Region "Structures"
        Private Structure Album : Implements IRegExCreator
            Friend URL As String
            Friend Title As String
            Private Function CreateFromArray(ByVal ParamsArray() As String) As Object Implements IRegExCreator.CreateFromArray
                If ParamsArray.ListExists(2) Then
                    URL = ParamsArray(0)
                    Title = TitleHtmlConverter(ParamsArray(1))
                End If
                Return Me
            End Function
        End Structure
#End Region
#Region "Declarations"
        Friend Property DownloadPublic As Boolean = True
        Friend Property DownloadPrivate As Boolean = True
        Friend Property DifferentFolders As Boolean = True
#End Region
#Region "Loaders"
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            With Container
                If Loading Then
                    DownloadPublic = .Value(Name_DownloadPublic).FromXML(Of Boolean)(True)
                    DownloadPrivate = .Value(Name_DownloadPrivate).FromXML(Of Boolean)(True)
                    DifferentFolders = .Value(Name_DifferentFolders).FromXML(Of Boolean)(True)
                Else
                    .Add(Name_DownloadPublic, DownloadPublic.BoolToInteger)
                    .Add(Name_DownloadPrivate, DownloadPrivate.BoolToInteger)
                    .Add(Name_DifferentFolders, DifferentFolders.BoolToInteger)
                End If
            End With
        End Sub
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return New UserExchangeOptions(Me)
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            If Not Obj Is Nothing AndAlso TypeOf Obj Is UserExchangeOptions Then
                With DirectCast(Obj, UserExchangeOptions)
                    DownloadPublic = .DownloadPublic
                    DownloadPrivate = .DownloadPrivate
                    DifferentFolders = .DifferentFolders
                End With
            End If
        End Sub
#End Region
#Region "Initializer"
        Friend Sub New()
            UseClientTokens = True
        End Sub
#End Region
#Region "Validation"
        Private Function IsValid() As Boolean
            Const ProfileDataPattern$ = "{0}[\r\n\s\W]*:[\r\n\s\W]*\<strong\>[\r\n\s\W]*([^\<]*)[\r\n\s\W]*\</strong"
            Const DescriptionPattern$ = "span style=""line-height: \d*px;""\>[\r\n\s\W]*([^\<]*)[\r\n\s\W]*\<"
            Try
                If Not IsSavedPosts Then
                    Dim r$ = Responser.GetResponse($"https://thisvid.com/members/{ID}/")
                    If Not r.IsEmptyString Then
                        Dim rr As New RParams("", Nothing, 1, EDP.ReturnValue)
                        Dim __getValue As Func(Of String, Boolean, String) = Function(ByVal member As String, ByVal appendMember As Boolean) As String
                                                                                 rr.Pattern = String.Format(ProfileDataPattern, member)
                                                                                 Dim v$ = CStr(RegexReplace(r, rr)).StringTrim
                                                                                 If Not v.IsEmptyString And appendMember Then v = $"{member}: {v}"
                                                                                 Return v
                                                                             End Function
                        UserSiteNameUpdate(__getValue("Name", False))
                        If Not UserSiteName.IsEmptyString And FriendlyName.IsEmptyString Then FriendlyName = UserSiteName : _ForceSaveUserData = True
                        Dim descr$ = String.Empty
                        descr.StringAppendLine(__getValue("Birth date", True))
                        descr.StringAppendLine(__getValue("Country", True))
                        descr.StringAppendLine(__getValue("City", True))
                        descr.StringAppendLine(__getValue("Gender", True))
                        descr.StringAppendLine(__getValue("Orientation", True))
                        descr.StringAppendLine(__getValue("Relationship status", True))
                        descr.StringAppendLine(__getValue("Favourite category", True))
                        descr.StringAppendLine(__getValue("My interests", True))
                        rr.Pattern = DescriptionPattern
                        descr.StringAppendLine(CStr(RegexReplace(r, rr)).StringTrim)
                        UserDescriptionUpdate(descr)
                    Else
                        Return False
                    End If
                End If
                Return True
            Catch ex As Exception
                UserExists = False
                Return False
            End Try
        End Function
#End Region
#Region "Download functions"
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            If ID.IsEmptyString Then ID = Name
            If IsValid() Then
                If IsSavedPosts Then
                    DownloadData(1, True, Token)
                    DownloadData_Images(Token)
                Else
                    If DownloadVideos Then
                        If DownloadPublic Then DownloadData(1, True, Token)
                        If DownloadPrivate Then DownloadData(1, False, Token)
                    End If
                    If DownloadImages Then DownloadData_Images(Token)
                End If
            End If
        End Sub
        Private Overloads Sub DownloadData(ByVal Page As Integer, ByVal IsPublic As Boolean, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                Dim p$ = IIf(Page = 1, String.Empty, $"{Page}/")
                If IsSavedPosts Then
                    URL = $"https://thisvid.com/my_favourite_videos/{p}"
                Else
                    URL = $"https://thisvid.com/members/{ID}/{IIf(IsPublic, "public", "private")}_videos/{p}"
                End If
                ThrowAny(Token)
                Dim r$ = Responser.GetResponse(URL)
                Dim cBefore% = _TempMediaList.Count
                If Not r.IsEmptyString Then
                    Dim __SpecialFolder$ = IIf(DifferentFolders, IIf(IsPublic, "Public", "Private"), String.Empty)
                    Dim l As List(Of String) = RegexReplace(r, If(IsSavedPosts, RegExVideoListSavedPosts, RegExVideoList))
                    If l.ListExists Then
                        For Each u$ In l
                            If Not u.IsEmptyString Then
                                If Not _TempPostsList.Contains(u) Then
                                    _TempPostsList.Add(u)
                                    _TempMediaList.Add(New UserMedia(u) With {.Type = UserMedia.Types.VideoPre, .SpecialFolder = __SpecialFolder})
                                Else
                                    Exit Sub
                                End If
                            End If
                        Next
                    End If
                End If
                If Not cBefore = _TempMediaList.Count Then DownloadData(Page + 1, IsPublic, Token)
            Catch ex As Exception
                ProcessException(ex, Token, $"videos downloading error [{URL}]")
            End Try
        End Sub
        Private Sub DownloadData_Images(ByVal Token As CancellationToken)
            Dim __baseUrl$ = If(IsSavedPosts, "https://thisvid.com/my_favourite_albums/", $"https://thisvid.com/members/{ID}/albums/")
            Dim URL$ = String.Empty
            Try
                Dim r$
                Dim i% = 0
                Dim __continue As Boolean = False
                Dim rAlbums As RParams = If(IsSavedPosts, RegExAlbumsListSaved, RegExAlbumsList)
                Do
                    i += 1
                    __continue = False
                    URL = __baseUrl
                    If i > 1 Then URL &= $"{i}/"
                    r = Responser.GetResponse(URL)
                    If Not r.IsEmptyString() Then
                        Dim albums As List(Of Album) = RegexFields(Of Album)(r, {rAlbums}, {1, 2}, EDP.ReturnValue)
                        Dim images As List(Of String)
                        Dim albumId$, img$, imgUrl$, imgId$
                        Dim u As UserMedia
                        Dim rErr As New ErrorsDescriber(EDP.ReturnValue)
                        __continue = True
                        If albums.ListExists Then
                            If albums.Count < 20 Then __continue = False
                            For Each a As Album In albums
                                If Not a.URL.IsEmptyString Then
                                    ThrowAny(Token)
                                    r = Responser.GetResponse(a.URL,, rErr)
                                    If Not r.IsEmptyString Then
                                        albumId = RegexReplace(r, RegExAlbumID)
                                        If a.Title.IsEmptyString Then a.Title = albumId
                                        images = RegexReplace(r, RegExAlbumImagesList)
                                        If images.ListExists Then
                                            For Each img In images
                                                ThrowAny(Token)
                                                r = Responser.GetResponse(img,, rErr)
                                                If Not r.IsEmptyString Then
                                                    imgUrl = RegexReplace(r, RegExAlbumImageUrl)
                                                    If Not imgUrl.IsEmptyString Then
                                                        u = New UserMedia(imgUrl) With {
                                                            .SpecialFolder = a.Title,
                                                            .Type = UserMedia.Types.Picture,
                                                            .URL_BASE = img
                                                        }
                                                        If Not u.File.File.IsEmptyString Then
                                                            imgId = $"{albumId}_{u.File.Name}"
                                                            If u.File.Extension.IsEmptyString Then u.File.Extension = "jpg"
                                                            u.Post = imgId
                                                            If Not _TempPostsList.Contains(imgId) Then
                                                                _TempPostsList.Add(imgId)
                                                                _TempMediaList.Add(u)
                                                            Else
                                                                Exit For
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            Next
                                            images.Clear()
                                        End If
                                    End If
                                End If
                            Next
                        Else
                            Exit Do
                        End If
                    End If
                Loop While __continue
            Catch ex As Exception
                ProcessException(ex, Token, $"images downloading error [{URL}]")
            End Try
        End Sub
#End Region
#Region "ReparseVideo"
        Protected Overrides Sub ReparseVideo(ByVal Token As CancellationToken)
            Try
                If _TempMediaList.Count > 0 Then
                    Dim u As UserMedia
                    Dim dirCmd$ = String.Empty
                    Dim f As SFile = Settings.YtdlpFile.File
                    Dim n$
                    Dim cookieFile As SFile = DirectCast(HOST.Source, SiteSettings).CookiesNetscapeFile
                    Dim command$
                    Dim e As EContainer
                    For i% = _TempMediaList.Count - 1 To 0 Step -1
                        u = _TempMediaList(i)
                        If u.Type = UserMedia.Types.VideoPre Then
                            ThrowAny(Token)
                            command = $"""{f}"" --verbose --dump-json "
                            If cookieFile.Exists Then command &= $"--no-cookies-from-browser --cookies ""{cookieFile}"" "
                            command &= u.URL
                            e = GetJson(command)
                            If Not e Is Nothing Then
                                u.URL = e.Value("url")
                                u.Post = New UserPost(e.Value("id"), ADateTime.ParseUnix32(e.Value("epoch")))
                                If u.Post.Date.HasValue Then
                                    Select Case CheckDatesLimit(u.Post.Date.Value, Nothing)
                                        Case DateResult.Skip : _TempPostsList.ListAddValue(u.Post.ID, LNC) : _TempMediaList.RemoveAt(i) : Continue For
                                        Case DateResult.Exit : Exit Sub
                                    End Select
                                End If
                                n = TitleHtmlConverter(e.Value("title"))
                                If Not n.IsEmptyString Then n = n.Replace("ThisVid.com", String.Empty).StringTrim.StringTrimEnd("-").StringTrim
                                If n.IsEmptyString Then n = u.Post.ID
                                If n.IsEmptyString Then n = "VideoFile"
                                u.File = $"{n}.mp4"
                                If u.URL.IsEmptyString OrElse (Not u.Post.ID.IsEmptyString AndAlso _TempPostsList.Contains(u.Post.ID)) Then
                                    _TempMediaList.RemoveAt(i)
                                Else
                                    u.Type = UserMedia.Types.Video
                                    _TempPostsList.Add(u.Post.ID)
                                    _TempMediaList(i) = u
                                End If
                                e.Dispose()
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex, Token, "video reparsing error")
            End Try
        End Sub
#End Region
#Region "GetJson"
        Private Function GetJson(ByVal Command As String) As EContainer
            Try
                Using b As New BatchExecutor(True)
                    b.Execute(Command, EDP.ReturnValue)
                    If b.OutputData.Count > 0 Then
                        Dim e As EContainer
                        For Each d$ In b.OutputData
                            If Not d.IsEmptyString AndAlso d.StartsWith("{") Then
                                e = JsonDocument.Parse(d, EDP.ReturnValue)
                                If Not e Is Nothing Then Return e
                            End If
                        Next
                    End If
                End Using
                Return Nothing
            Catch ex As Exception
                HasError = True
                LogError(ex, $"GetJson({Command})")
                Return Nothing
            End Try
        End Function
#End Region
#Region "DownloadContent"
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            Dim s As Boolean? = SeparateVideoFolder
            If DifferentFolders Then SeparateVideoFolder = False Else SeparateVideoFolder = Nothing
            DownloadContentDefault(Token)
            SeparateVideoFolder = s
        End Sub
#End Region
#Region "Standalone downloader"
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            _TempMediaList.Add(New UserMedia(Data.URL) With {.Type = UserMedia.Types.VideoPre})
            ReparseVideo(Token)
        End Sub
#End Region
#Region "DownloadingException"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            If Responser.StatusCode = Net.HttpStatusCode.NotFound Then
                Return 1
            Else
                Return 0
            End If
        End Function
#End Region
    End Class
End Namespace