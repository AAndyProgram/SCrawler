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
        Private Const Name_DownloadFavourite As String = "DownloadFavourite"
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
        Friend Overrides ReadOnly Property FeedIsUser As Boolean
            Get
                Return IsUser
            End Get
        End Property
        Friend Property DownloadPublic As Boolean = True
        Friend Property DownloadPrivate As Boolean = True
        Friend Property DownloadFavourite As Boolean = False
        Friend Property DifferentFolders As Boolean = True
        Friend Property SiteMode As SiteModes = SiteModes.User
        Private Property Arguments As String = String.Empty
        Friend Overrides ReadOnly Property SpecialLabels As IEnumerable(Of String)
            Get
                Return {SearchRequestLabelName}
            End Get
        End Property
        Friend Property QueryString As String
            Get
                If SiteMode = SiteModes.User Then
                    Return String.Empty
                Else
                    Return GetNonUserUrl(0)
                End If
            End Get
            Set(ByVal q As String)
                UpdateUserOptions(True, q)
            End Set
        End Property
        Friend Overrides ReadOnly Property IsUser As Boolean
            Get
                Return SiteMode = SiteModes.User
            End Get
        End Property
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return DirectCast(HOST.Source, SiteSettings)
            End Get
        End Property
#End Region
#Region "Loaders"
        Private Function UpdateUserOptions(Optional ByVal Force As Boolean = False, Optional ByVal NewUrl As String = Nothing) As Boolean
            If Not Force OrElse (Not SiteMode = SiteModes.User AndAlso Not NewUrl.IsEmptyString AndAlso MyFileSettings.Exists) Then
                Dim eObj As Plugin.ExchangeOptions = Nothing
                If Force Then eObj = MySettings.IsMyUser(NewUrl)
                If (Force And Not eObj.UserName.IsEmptyString) Or (Not Force And NameTrue(True).IsEmptyString) Then
                    Dim n$() = If(Force, eObj.UserName, Name).Split("@")
                    If n.ListExists(2) Then

                        If Force And SiteMode = SiteModes.User Then Return False

                        Dim __TrueName$, __Arguments$
                        Dim __Mode As SiteModes
                        Dim __ForceApply As Boolean = False
                        Dim opt$() = If(Force, eObj.Options, Options).Split("@")
                        __Mode = CInt(n(0))
                        If opt.Length > 1 Then
                            __Arguments = opt.ListTake(0, 100, EDP.ReturnValue).ListToString(String.Empty)
                        Else
                            __Arguments = String.Empty
                        End If
                        __TrueName = n(1)

                        If Force AndAlso (Not NameTrue(True) = __TrueName Or Not SiteMode = __Mode) Then
                            If ValidateChangeSearchOptions(ToStringForLog, $"{__Mode}: {__TrueName}", $"{SiteMode}: {NameTrue(True)}") Then
                                __ForceApply = True
                            Else
                                Return False
                            End If
                        End If

                        Arguments = __Arguments
                        Options = If(Force, eObj.Options, Options)
                        If Not Force Then
                            NameTrue = __TrueName
                            SiteMode = __Mode
                            Settings.Labels.Add(SearchRequestLabelName)
                            Labels.ListAddValue(SearchRequestLabelName, LNC)
                            Labels.Sort()
                            UserSiteName = $"{SiteMode}: {NameTrue}"
                            If FriendlyName.IsEmptyString Then FriendlyName = UserSiteName
                        ElseIf Force And __ForceApply Then
                            NameTrue = __TrueName
                            SiteMode = __Mode
                        End If
                        Return True
                    Else
                        SiteMode = SiteModes.User
                        NameTrue = Name
                    End If
                End If
            End If
            Return False
        End Function
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            With Container
                If Loading Then
                    DownloadPublic = .Value(Name_DownloadPublic).FromXML(Of Boolean)(True)
                    DownloadPrivate = .Value(Name_DownloadPrivate).FromXML(Of Boolean)(True)
                    DownloadFavourite = .Value(Name_DownloadFavourite).FromXML(Of Boolean)(False)
                    DifferentFolders = .Value(Name_DifferentFolders).FromXML(Of Boolean)(True)
                    SiteMode = .Value(Name_SiteMode).FromXML(Of Integer)(SiteModes.User)
                    Arguments = .Value(Name_Arguments)
                    UpdateUserOptions()
                Else
                    If UpdateUserOptions() Then
                        .Value(Name_LabelsName) = LabelsString
                        .Value(Name_UserSiteName) = UserSiteName
                        .Value(Name_FriendlyName) = FriendlyName
                    End If
                    .Add(Name_DownloadPublic, DownloadPublic.BoolToInteger)
                    .Add(Name_DownloadPrivate, DownloadPrivate.BoolToInteger)
                    .Add(Name_DownloadFavourite, DownloadFavourite.BoolToInteger)
                    .Add(Name_DifferentFolders, DifferentFolders.BoolToInteger)
                    .Add(Name_TrueName, NameTrue(True))
                    .Add(Name_SiteMode, CInt(SiteMode))
                    .Add(Name_Arguments, Arguments)

                    'Debug.WriteLine(GetNonUserUrl(0))
                    'Debug.WriteLine(GetNonUserUrl(2))
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
                    DownloadFavourite = .DownloadFavourite
                    DifferentFolders = .DifferentFolders
                    QueryString = .QueryString
                End With
            End If
        End Sub
#End Region
#Region "Initializer"
        Friend Sub New()
            UseClientTokens = True
            SessionPosts = New List(Of String)
            _ResponserAutoUpdateCookies = True
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
        Private ReadOnly SessionPosts As List(Of String)
        Private AddedCount As Integer = 0
        Private _PageVideosRepeat As Integer = 0
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            Try
                SessionPosts.Clear()
                AddedCount = 0
                _PageVideosRepeat = 0
                SessionPosts.Clear()
                Responser.Cookies.ChangedAllowInternalDrop = False
                Responser.Cookies.Changed = False
                If ID.IsEmptyString Then ID = Name
                If Not IsUser OrElse IsValid() Then
                    If IsSavedPosts Then
                        DownloadData(1, 0, Token)
                        DownloadData_Images(Token)
                    Else
                        If IsUser Then
                            If DownloadVideos Then
                                If DownloadPublic Then DownloadData(1, 0, Token)
                                If DownloadPrivate Then DownloadData(1, 1, Token)
                                If DownloadFavourite Then DownloadData(1, 2, Token)
                            End If
                            If DownloadImages And Not IsSubscription Then DownloadData_Images(Token)
                        Else
                            DownloadData(1, 0, Token)
                        End If
                    End If
                End If
            Finally
                If Responser.Cookies.Changed Then MySettings.UpdateCookies(Responser) : Responser.Cookies.Changed = False
            End Try
        End Sub
        Friend Function GetNonUserUrl(ByVal Page As Integer) As String
            Dim url$ = String.Empty
            Select Case SiteMode
                Case SiteModes.Tags
                    url = $"https://thisvid.com/{SiteSettings.P_Tags}/{NameTrue}/"
                    If Not Arguments.IsEmptyString Then url &= $"{Arguments}/"
                    If Page > 1 Then url &= $"{Page}/"
                Case SiteModes.Categories
                    url = $"https://thisvid.com/{SiteSettings.P_Categories}/{NameTrue}/"
                    If Not Arguments.IsEmptyString Then url &= $"{Arguments}/"
                    If Page > 1 Then url &= $"{Page}/"
                Case SiteModes.Search
                    If Not Arguments.IsEmptyString Then
                        url = $"https://thisvid.com/{Arguments}/"
                        If Page > 1 Then url &= $"{Page}/"
                        url &= $"?q={NameTrue}/"
                    End If
            End Select
            Return url
        End Function
        Private Overloads Sub DownloadData(ByVal Page As Integer, ByVal Model As Byte, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                ProgressPre.ChangeMax(1)
                Dim limit% = If(DownloadTopCount, -1)
                Dim p$ = IIf(Page = 1, String.Empty, $"{Page}/")
                If IsSavedPosts Then
                    URL = $"https://thisvid.com/my_favourite_videos/{p}"
                ElseIf IsUser Then
                    URL = $"https://thisvid.com/members/{ID}/{Interaction.Switch(Model = 0, "public", Model = 1, "private", Model = 2, "favourite")}_videos/{p}"
                Else
                    URL = GetNonUserUrl(Page)
                    If URL.IsEmptyString Then Throw New ArgumentNullException With {.HelpLink = 1}
                End If
                ThrowAny(Token)
                ProgressPre.Perform()
                Dim r$ = Responser.GetResponse(URL)
                Dim cBefore% = _TempMediaList.Count
                Dim pageRepeatSet As Boolean = False, prevPostsFound As Boolean = False, newPostsFound As Boolean = False

                If Not r.IsEmptyString Then
                    Dim __SpecialFolder$ = If(DifferentFolders And Not IsSavedPosts And IsUser,
                                              Interaction.Switch(Model = 0, "Public", Model = 1, "Private", Model = 2, "Favourite"),
                                              String.Empty)
                    Dim l As List(Of String) = RegexReplace(r, If(IsSavedPosts, RegExVideoListSavedPosts, RegExVideoList))
                    If l.ListExists Then
                        For Each u$ In l
                            If Not u.IsEmptyString Then
                                If Not _TempPostsList.Contains(u) Then
                                    _TempPostsList.Add(u)
                                    _TempMediaList.Add(New UserMedia(u) With {.Type = UserMedia.Types.VideoPre, .SpecialFolder = __SpecialFolder})
                                    AddedCount += 1
                                    newPostsFound = True
                                    If limit > 0 And AddedCount >= limit Then Exit Sub
                                ElseIf SessionPosts.Count > 0 AndAlso SessionPosts.Contains(u) Then
                                    prevPostsFound = True
                                    Continue For
                                Else
                                    If _PageVideosRepeat >= 2 Then
                                        Exit Sub
                                    ElseIf Not pageRepeatSet And Not newPostsFound Then
                                        pageRepeatSet = True
                                        _PageVideosRepeat += 1
                                    End If
                                End If
                            End If
                        Next
                        If prevPostsFound And Not pageRepeatSet And Not newPostsFound Then pageRepeatSet = True : _PageVideosRepeat += 1
                        If prevPostsFound And newPostsFound And pageRepeatSet Then _PageVideosRepeat -= 1
                        SessionPosts.ListAddList(l, LNC)
                        l.Clear()
                    End If
                End If
                If _PageVideosRepeat < 2 And
                   ((Not IsUser And prevPostsFound And Not newPostsFound And Page < 1000) Or
                   (Not cBefore = _TempMediaList.Count And (IsUser Or Page < 1000))) Then DownloadData(Page + 1, Model, Token)
            Catch aex As ArgumentNullException When aex.HelpLink = 1
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
                            ProgressPre.ChangeMax(albums.Count)
                            For Each a As Album In albums
                                ProgressPre.Perform()
                                If Not a.URL.IsEmptyString Then
                                    ThrowAny(Token)
                                    r = Responser.GetResponse(a.URL,, rErr)
                                    If Not r.IsEmptyString Then
                                        albumId = RegexReplace(r, RegExAlbumID)
                                        If a.Title.IsEmptyString Then a.Title = albumId
                                        images = RegexReplace(r, RegExAlbumImagesList)
                                        If images.ListExists Then
                                            ProgressPre.ChangeMax(images.Count)
                                            For Each img In images
                                                ProgressPre.Perform()
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
            If IsSubscription Then
                ReparseVideoSubscriptions(Token)
            Else
                Try
                    If _TempMediaList.Count > 0 Then
                        Dim u As UserMedia
                        Dim dirCmd$ = String.Empty
                        Dim f As SFile = Settings.YtdlpFile.File
                        Dim n$
                        Dim cookieFile As SFile = MySettings.CookiesNetscapeFile
                        Dim command$
                        Dim e As EContainer
                        ProgressPre.ChangeMax(_TempMediaList.Count)
                        For i% = _TempMediaList.Count - 1 To 0 Step -1
                            ProgressPre.Perform()
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
            End If
        End Sub
        Private Sub ReparseVideoSubscriptions(ByVal Token As CancellationToken)
            Try
                If _TempMediaList.Count > 0 Then
                    Dim u As UserMedia
                    Dim n$, r$
                    Dim c% = 0
                    Dim ii As Byte
                    Dim repeat As Boolean
                    Progress.Maximum += _TempMediaList.Count
                    For i% = _TempMediaList.Count - 1 To 0 Step -1
                        Progress.Perform()
                        u = _TempMediaList(i)
                        If u.Type = UserMedia.Types.VideoPre Then
                            If Not DownloadTopCount.HasValue OrElse c <= DownloadTopCount.Value Then
                                repeat = False
                                For ii = 0 To 1
                                    ThrowAny(Token)
                                    r = Responser.GetResponse(u.URL,, EDP.ReturnValue)
                                    If Not r.IsEmptyString Then
                                        n = TitleHtmlConverter(RegexReplace(r, RegExVideoTitle))
                                        u.Post.ID = u.URL
                                        If Not n.IsEmptyString Then n = n.Replace("ThisVid.com", String.Empty).StringTrim.StringTrimEnd("-").StringTrim
                                        If n.IsEmptyString Then n = TitleHtmlConverter(u.URL.Replace("https://thisvid.com/videos/", String.Empty).StringTrim.StringTrimEnd("-").StringTrim)
                                        If n.IsEmptyString Then n = "VideoFile"
                                        u.File = $"{n}.mp4"
                                        u.PictureOption = n
                                        u.URL = RegexReplace(r, Regex_VideosThumb_OG_IMAGE)
                                        If u.URL.IsEmptyString And Not repeat And ii = 0 Then
                                            Thread.Sleep(250)
                                            u = _TempMediaList(i)
                                            repeat = True
                                            Continue For
                                        End If
                                        If u.URL.IsEmptyString Then u.URL = RegexReplace(r, RegExVideosThumb1)
                                        If u.URL.IsEmptyString Then u.URL = RegexReplace(r, RegExVideosThumb2)
                                        If Not u.URL.IsEmptyString Then
                                            u.URL = LinkFormatterSecure(u.URL)
                                            u.Type = UserMedia.Types.Video
                                            _TempPostsList.Add(u.Post.ID)
                                            _TempMediaList(i) = u
                                            c += 1
                                        Else
                                            _TempMediaList.RemoveAt(i)
                                        End If
                                    End If
                                    If Not repeat Then Exit For
                                Next
                            Else
                                _TempMediaList.RemoveAt(i)
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex, Token, "subscriptions video reparsing error")
            Finally
                If Responser.Cookies.Changed Then MySettings.UpdateCookies(Responser) : Responser.Cookies.Changed = False
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
#Region "IDisposable Support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then SessionPosts.Clear()
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace