' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Text
Imports System.Threading
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports SCrawler.API.Base
Imports SCrawler.API.YouTube.Objects
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace API.Xhamster
    Friend Class UserData : Inherits UserDataBase : Implements IPSite
#Region "XML names"
        Private Const Name_Gender As String = "Gender"
        Private Const Name_IsCreator As String = "IsCreator"
        Private Const Name_GetMoments As String = "GetMoments"
#End Region
#Region "Declarations"
        Friend Overrides ReadOnly Property FeedIsUser As Boolean
            Get
                Return SiteMode = SiteModes.User
            End Get
        End Property
        Friend Property IsChannel As Boolean = False
        Friend Property IsCreator As Boolean = False
        Friend Property GetMoments As Boolean = False
        Friend Property Gender As String = String.Empty
        Friend Property SiteMode As SiteModes = SiteModes.User
        Friend Property Arguments As String = String.Empty
        Friend Overrides ReadOnly Property IsUser As Boolean
            Get
                Return SiteMode = SiteModes.User Or SiteMode = SiteModes.Pornstars
            End Get
        End Property
        Friend ReadOnly Property IsSearch As Boolean
            Get
                Return SiteMode = SiteModes.Search Or SiteMode = SiteModes.Tags Or SiteMode = SiteModes.Categories
            End Get
        End Property
        Friend Overrides ReadOnly Property SpecialLabels As IEnumerable(Of String)
            Get
                Return {SearchRequestLabelName}
            End Get
        End Property
        Friend Property QueryString As String Implements IPSite.QueryString
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
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return DirectCast(HOST.Source, SiteSettings)
            End Get
        End Property
        Private Structure ExchObj
            Friend IsPhoto As Boolean
        End Structure
        Private ReadOnly _TempPhotoData As List(Of UserMedia)
        Private Function UpdateUserOptions(Optional ByVal Force As Boolean = False, Optional ByVal NewUrl As String = Nothing) As Boolean
            If Not Force OrElse (Not SiteMode = SiteModes.User AndAlso Not NewUrl.IsEmptyString AndAlso MyFileSettings.Exists) Then
                Dim eObj As Plugin.ExchangeOptions = Nothing
                If Force Then eObj = MySettings.IsMyUser(NewUrl)
                If (Force And Not eObj.UserName.IsEmptyString) Or (Not Force And NameTrue(True).IsEmptyString) Then
                    Dim n$() = If(Force, eObj.UserName, Name).Split("@")
                    If n.ListExists Then
                        If n.Length = 2 And If(Force, eObj.Options, Options).IsEmptyString Then
                            If Force Then Return False
                            NameTrue = n(0)
                            IsChannel = n(1) = SiteSettings.ChannelOption
                            IsCreator = n(1) = SiteSettings.P_Creators
                        ElseIf IsChannel Then
                            If Force Then Return False
                            NameTrue = Name
                        ElseIf Not If(Force, eObj.Options, Options).IsEmptyString Then
                            Dim __TrueName$, __Arguments$, __Gender$
                            Dim __Mode As SiteModes
                            Dim __ForceApply As Boolean = False
                            Dim n2 As List(Of String) = If(Force, eObj.Options, Options).Split("@").ListIfNothing
                            If n2.ListExists Then
                                IsChannel = False
                                __Mode = CInt(n2(0))
                                IsCreator = __Mode = SiteModes.User
                                __Gender = n2(1)
                                __Arguments = n2(3)
                                __TrueName = n2.ListTake(3, 100, EDP.ReturnValue).ListToString(String.Empty)

                                If Force AndAlso (Not NameTrue(True) = __TrueName Or Not SiteMode = __Mode Or Not Gender = __Gender) Then
                                    If ValidateChangeSearchOptions(ToStringForLog,
                                                                   $"{__Mode}{IIf(__Gender.IsEmptyString, String.Empty, $" ({__Gender})")}: {__TrueName}",
                                                                   $"{SiteMode}{IIf(Gender.IsEmptyString, String.Empty, $" ({Gender})")}: {NameTrue(True)}") Then
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
                                    Gender = __Gender

                                    UserSiteName = $"{SiteMode}: {NameTrue}"
                                    If FriendlyName.IsEmptyString Then FriendlyName = UserSiteName
                                    Settings.Labels.Add(SearchRequestLabelName)
                                    Labels.ListAddValue(SearchRequestLabelName, LNC)
                                    Labels.Sort()
                                ElseIf Force And __ForceApply Then
                                    NameTrue = __TrueName
                                    SiteMode = __Mode
                                    Gender = __Gender
                                End If

                                Return True
                            Else
                                If Force Then Return False
                                UserExists = False
                            End If
                        Else
                            If Force Then Return False
                            NameTrue = n(0)
                        End If
                    End If
                End If
            End If
            Return False
        End Function
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            With Container
                If Loading Then
                    IsChannel = .Value(Name_IsChannel).FromXML(Of Boolean)(False)
                    IsCreator = .Value(Name_IsCreator).FromXML(Of Boolean)(False)
                    GetMoments = .Value(Name_GetMoments).FromXML(Of Boolean)(False)
                    Gender = .Value(Name_Gender)
                    SiteMode = .Value(Name_SiteMode).FromXML(Of Integer)(SiteModes.User)
                    Arguments = .Value(Name_Arguments)
                    UpdateUserOptions()
                Else
                    If UpdateUserOptions() Then
                        .Value(Name_LabelsName) = LabelsString
                        .Value(Name_UserSiteName) = UserSiteName
                        .Value(Name_FriendlyName) = FriendlyName
                    End If
                    .Add(Name_IsChannel, IsChannel.BoolToInteger)
                    .Add(Name_IsCreator, IsCreator.BoolToInteger)
                    .Add(Name_GetMoments, GetMoments.BoolToInteger)
                    .Add(Name_TrueName, NameTrue(True))
                    .Add(Name_Gender, Gender)
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
            If Not Obj Is Nothing AndAlso TypeOf Obj Is UserExchangeOptions Then DirectCast(Obj, UserExchangeOptions).Apply(Me)
        End Sub
        Private MyCache As CacheKeeper = Nothing
        Private Sub ResetCache()
            MyCache.DisposeIfReady(False)
            MyCache = Nothing
        End Sub
#End Region
#Region "Initializer"
        Friend Sub New()
            UseInternalM3U8Function = True
            UseInternalDownloadFileFunction = True
            UseClientTokens = True
            _TempPhotoData = New List(Of UserMedia)
            SessionPosts = New List(Of String)
        End Sub
#End Region
#Region "Download functions"
        Friend Function GetNonUserUrl(ByVal Page As Integer) As String
            Const newest$ = "/newest"
            If SiteMode = SiteModes.User And Not IsCreator Then
                Return String.Empty
            Else
                Dim url$ = "https://xhamster.com/"
                If Not Gender.IsEmptyString Then url &= $"{Gender}/"
                Select Case SiteMode
                    Case SiteModes.Tags : url &= SiteSettings.P_Tags
                    Case SiteModes.Categories : url &= SiteSettings.P_Categories
                    Case SiteModes.Search : url &= SiteSettings.P_Search
                    Case SiteModes.Pornstars : url &= SiteSettings.P_Pornstars
                    Case SiteModes.User : url &= SiteSettings.P_Creators
                    Case Else : Return String.Empty
                End Select
                url &= $"/{NameTrue}"

                Dim args$ = Arguments
                If (args.IsEmptyString OrElse Not args.Contains(newest)) And Not SiteMode = SiteModes.Search Then url &= newest
                If Page > 1 Then
                    If args.IsEmptyString Then
                        If SiteMode = SiteModes.Search Then
                            args = $"?page={Page}"
                        Else
                            args = $"/{Page}"
                        End If
                    Else
                        If SiteMode = SiteModes.Search Then
                            args = $"?{args}&page={Page}"
                        Else
                            If args.Contains("?") Then
                                args = $"/{args.Replace("?", $"/{Page}?")}"
                            Else
                                args = $"/{args.StringTrimEnd("/")}/{Page}"
                            End If
                        End If
                    End If
                Else
                    If Not args.IsEmptyString Then args = $"{IIf(SiteMode = SiteModes.Search, "?", "/")}{args}"
                End If

                url &= args

                Return url
            End If
        End Function
        Private SearchPostsCount As Integer = 0
        Private ReadOnly SessionPosts As List(Of String)
        Private _PageVideosRepeat As Integer = 0
        Friend Overrides Sub DownloadData(Token As CancellationToken)
            Try : MyBase.DownloadData(Token) : Finally : ResetCache() : End Try
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            Try
                _TempPhotoData.Clear()
                SearchPostsCount = 0
                _PageVideosRepeat = 0
                SessionPosts.Clear()
                Responser.CookiesAsHeader = True
                If DownloadVideos Then DownloadData(1, True, False, Token)
                If DownloadVideos And GetMoments Then DownloadData(1, True, True, Token)
                If Not IsChannel And Not IsCreator And DownloadImages And Not IsSubscription Then
                    DownloadData(1, False, False, Token)
                    ReparsePhoto(Token)
                End If
            Finally
                Responser.CookiesAsHeader = False
            End Try
        End Sub
        Private Overloads Sub DownloadData(ByVal Page As Integer, ByVal IsVideo As Boolean, ByVal GetMoments As Boolean, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                Dim MaxPage% = -1
                Dim Type As UTypes = IIf(IsVideo, UTypes.VideoPre, UTypes.Picture)
                Dim mPages$ = IIf(IsVideo, "maxVideoPages", "maxPhotoPages")
                Dim specFolder$ = IIf(GetMoments, "Moments*", String.Empty)
                Dim listNode$()
                Dim containerNodes As New List(Of String())
                Dim skipped As Boolean = False
                Dim limit% = If(DownloadTopCount, -1)
                Dim cBefore% = _TempMediaList.Count
                Dim pageRepeatSet As Boolean = False, prevPostsFound As Boolean = False, newPostsFound As Boolean = False
                Dim pids As New List(Of String)
                Dim m As UserMedia
                Dim checkLimit As Func(Of Boolean) = Function() limit > 0 And SearchPostsCount >= limit And IsVideo

                If IsSavedPosts Then
                    If IsVideo Then
                        containerNodes.Add({"favoriteVideoListComponent", "models"})
                        containerNodes.Add({"favoriteVideoListComponent", "videoThumbProps"})
                    Else
                        containerNodes.Add({"favoritesGalleriesAndPhotosCollection"})
                    End If
                ElseIf Not SiteMode = SiteModes.Search Then
                    If IsVideo Then
                        If GetMoments Then containerNodes.Add({"momentListComponent", "videoThumbProps"})
                        containerNodes.Add({"trendingVideoListComponent", "models"})
                        containerNodes.Add({"pagesCategoryComponent", "trendingVideoListProps", "models"})
                        containerNodes.Add({"trendingVideoSectionComponent", "videoModels"})
                        containerNodes.Add({"trendingVideoSectionComponent", "videoListProps", "videoThumbProps"})
                        containerNodes.Add({"userVideoCollection"})
                        containerNodes.Add({"videoListComponent", "models"})
                        containerNodes.Add({"videoListComponent", "videoThumbProps"})
                    Else
                        containerNodes.Add({"userGalleriesCollection"})
                    End If
                End If

                If IsSavedPosts Then
                    URL = $"https://xhamster.com/my/favorites/{IIf(IsVideo, "videos", "photos-and-galleries")}{IIf(Page = 1, String.Empty, $"/{Page}")}"
                    containerNodes.Add(If(IsVideo, {"favoriteVideoListComponent", "models"}, {"favoritesGalleriesAndPhotosCollection"}))
                ElseIf IsChannel Then
                    URL = $"https://xhamster.com/channels/{NameTrue}/newest{IIf(Page = 1, String.Empty, $"/{Page}")}"
                ElseIf SiteMode = SiteModes.Search Then
                    URL = GetNonUserUrl(Page)
                    containerNodes.Add({"searchResult", "models"})
                ElseIf IsCreator Or SiteMode = SiteModes.Tags Or SiteMode = SiteModes.Categories Or SiteMode = SiteModes.Pornstars Then
                    URL = GetNonUserUrl(Page)
                Else
                    URL = $"https://xhamster.com/{SiteSettings.UserOption}/{NameTrue}/{If(GetMoments, "moments", IIf(IsVideo, "videos", "photos"))}{IIf(Page = 1, String.Empty, $"/{Page}")}"
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

                            For Each listNode In containerNodes
                                With j(listNode)
                                    If .ListExists Then
                                        ProgressPre.ChangeMax(.Count)
                                        For Each e As EContainer In .Self
                                            ProgressPre.Perform()
                                            m = ExtractMedia(e, Type,,,, specFolder)
                                            If Not m.URL.IsEmptyString Then
                                                pids.ListAddValue(m.Post.ID, LNC)
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
                                                    SearchPostsCount += 1
                                                    newPostsFound = True
                                                    If checkLimit.Invoke Then Exit Sub
                                                ElseIf Not IsVideo Then
                                                    If DirectCast(m.Object, ExchObj).IsPhoto Then
                                                        If Not m.Post.ID.IsEmptyString AndAlso Not _TempPostsList.Contains(m.Post.ID) Then
                                                            _TempPostsList.Add(m.Post.ID)
                                                            _TempMediaList.ListAddValue(m, LNC)
                                                        End If
                                                    Else
                                                        _TempPhotoData.ListAddValue(m, LNC)
                                                    End If
                                                ElseIf IsVideo And _TempPostsList.Contains(m.Post.ID) Then
                                                    If SessionPosts.Count > 0 AndAlso SessionPosts.Contains(m.Post.ID) Then
                                                        prevPostsFound = True
                                                        Continue For
                                                    ElseIf _PageVideosRepeat >= 2 Then
                                                        Exit Sub
                                                    ElseIf Not pageRepeatSet And Not newPostsFound Then
                                                        pageRepeatSet = True
                                                        _PageVideosRepeat += 1
                                                    End If
                                                Else
                                                    Exit Sub
                                                End If
                                            End If
                                        Next
                                        If prevPostsFound And Not pageRepeatSet And Not newPostsFound Then pageRepeatSet = True : _PageVideosRepeat += 1
                                        If prevPostsFound And newPostsFound And pageRepeatSet Then _PageVideosRepeat -= 1
                                        SessionPosts.ListAddList(pids, LNC)
                                        pids.Clear()
                                        Exit For
                                    End If
                                End With
                            Next
                        End If
                    End Using
                End If

                containerNodes.Clear()

                If _PageVideosRepeat < 2 And ((
                    (MaxPage = -1 Or Page < MaxPage) And
                    ((Not _TempMediaList.Count = cBefore Or skipped) And (IsUser Or Page < 1000))
                   ) Or
                   (IsChannel Or (Not IsUser And Page < 1000 And prevPostsFound And Not newPostsFound))) Then DownloadData(Page + 1, IsVideo, GetMoments, Token)
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]")
            End Try
        End Sub
#End Region
#Region "Reparse video, photo"
        Protected Overrides Sub ReparseVideo(ByVal Token As CancellationToken)
            If IsSubscription Then
                ReparseVideoSubscriptions(Token)
            Else
                Try
                    If _TempMediaList.Count > 0 AndAlso _TempMediaList.Exists(Function(tm) tm.Type = UTypes.VideoPre) Then
                        Dim m As UserMedia, m2 As UserMedia
                        ProgressPre.ChangeMax(_TempMediaList.Count)
                        For i% = _TempMediaList.Count - 1 To 0 Step -1
                            ProgressPre.Perform()
                            If _TempMediaList(i).Type = UTypes.VideoPre Then
                                m = _TempMediaList(i)
                                If Not m.URL_BASE.IsEmptyString Then
                                    m2 = m
                                    ThrowAny(Token)
                                    If GetM3U8_Init(m2, m.URL_BASE, m.SpecialFolder, i) Then
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
            End If
        End Sub
        Private Sub ReparseVideoSubscriptions(ByVal Token As CancellationToken)
            Try
                If _TempMediaList.Count > 0 AndAlso _TempMediaList.Exists(Function(tm) tm.Type = UTypes.VideoPre) Then
                    Dim m As UserMedia, m2 As UserMedia
                    Dim c% = 0
                    Progress.Maximum += _TempMediaList.Count
                    For i% = _TempMediaList.Count - 1 To 0 Step -1
                        Progress.Perform()
                        If _TempMediaList(i).Type = UTypes.VideoPre Then
                            If Not DownloadTopCount.HasValue OrElse c <= DownloadTopCount.Value Then
                                m = _TempMediaList(i)
                                If Not m.URL_BASE.IsEmptyString Then
                                    m2 = m
                                    ThrowAny(Token)
                                    If GetM3U8_Init(m2, m.URL_BASE, String.Empty, i) Then
                                        m2.URL_BASE = m.URL_BASE
                                        _TempMediaList(i) = m2
                                        c += 1
                                    Else
                                        _TempMediaList.RemoveAt(i)
                                    End If
                                End If
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
        Private Overloads Sub ReparsePhoto(ByVal Token As CancellationToken)
            If _TempPhotoData.Count > 0 Then
                ProgressPre.ChangeMax(_TempPhotoData.Count)
                For i% = 0 To _TempPhotoData.Count - 1 : ProgressPre.Perform() : ReparsePhoto(i, 1, Token) : Next
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
                            MaxPage = j.Value({"pagination"}, "maxPage").IfNullOrEmpty(j.Value({"galleryPage", "paginationProps"}, "lastPageNumber")).FromXML(Of Integer)(-1)
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
                    ProgressPre.ChangeMax(_ContentList.Count)
                    For i% = 0 To _ContentList.Count - 1
                        ProgressPre.Perform()
                        m = _ContentList(i)
                        If m.State = UserMedia.States.Missing AndAlso Not m.URL_BASE.IsEmptyString Then
                            ThrowAny(Token)
                            m2 = Nothing
                            If GetM3U8_Init(m2, m.URL_BASE, m.SpecialFolder, i) Then
                                m2.URL_BASE = m.URL_BASE
                                m2.State = UserMedia.States.Missing
                                m2.Attempts = m.Attempts
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
        Private Structure XMMediaInfo : Implements IComparable(Of XMMediaInfo)
            Friend URL As String
            Friend Type As UTypes
            Friend IsInternal As Boolean
            Friend Thumb As String
            Friend FormatID As String
            Friend Width As Integer
            Friend Height As Integer
            Friend Title As String
            Private Function CompareTo(ByVal Other As XMMediaInfo) As Integer Implements IComparable(Of XMMediaInfo).CompareTo
                Return Width.CompareTo(Other.Width) * -1
            End Function
        End Structure
        Private Function GetM3U8_Init(ByRef m As UserMedia, ByVal URL As String, ByVal SpecFolder As String, ByVal n As Integer) As Boolean
            Try
                If Not URL.IsEmptyString Then
                    Dim IsInternal As Boolean = False
                    Dim r$ = GetMediaInfo(URL, n, IsInternal)
                    If Not r.IsEmptyString Then
                        Using j As EContainer = JsonDocument.Parse(r)
                            If j.ListExists Then
                                If IsInternal AndAlso GetM3U8_Internal(m, URL, j, SpecFolder) Then
                                    Return True
                                Else
                                    Dim xmm As New XMMediaInfo
                                    Dim __checkURL As Func(Of EContainer, XMMediaInfo, XMMediaInfo) =
                                        Function(ByVal jj As EContainer, ByVal __xmm As XMMediaInfo) As XMMediaInfo
                                            With jj.Value("url").StringToLower
                                                If Not .IsEmptyString AndAlso .EndsWith(".m3u8") Or .EndsWith(".mp4") Then __xmm.URL = .Self
                                            End With
                                            Return __xmm
                                        End Function
                                    Dim __applyXMM As Func(Of EContainer, XMMediaInfo, XMMediaInfo) =
                                        Function(ByVal jj As EContainer, ByVal __xmm As XMMediaInfo) As XMMediaInfo
                                            With jj
                                                __xmm.Type = IIf(__xmm.URL.ToLower.EndsWith(".m3u8"), UTypes.m3u8, UTypes.Video)
                                                __xmm.Width = AConvert(Of Integer)(.Value("width"), 1, EDP.ReturnValue)
                                                __xmm.Height = AConvert(Of Integer)(.Value("height"), 1, EDP.ReturnValue)
                                                __xmm.FormatID = .Value("format_id")
                                            End With
                                            Return __xmm
                                        End Function
                                    xmm = __checkURL(j, xmm)
                                    If Not xmm.URL.IsEmptyString Then
                                        xmm = __applyXMM(j, xmm)
                                    Else
                                        With j("formats")
                                            If .ListExists Then
                                                Dim l As New List(Of XMMediaInfo)
                                                Dim tmpXMM As XMMediaInfo
                                                For Each format As EContainer In .Self
                                                    tmpXMM = New XMMediaInfo
                                                    tmpXMM = __checkURL(format, tmpXMM)
                                                    If Not tmpXMM.URL.IsEmptyString Then
                                                        tmpXMM = __applyXMM(format, tmpXMM)
                                                        l.Add(tmpXMM)
                                                    End If
                                                Next
                                                If l.Count > 0 Then
                                                    If Not CBool(MySettings.DownloadUHD.Value) AndAlso l.LongCount(Function(v) v.Height <= 1080) > 0 Then _
                                                       l.RemoveAll(Function(v) v.Height > 1080)
                                                    l.Sort()
                                                    xmm = l.First
                                                    l.Clear()
                                                End If
                                            End If
                                        End With
                                    End If
                                    If Not xmm.URL.IsEmptyString Then
                                        xmm.IsInternal = False
                                        xmm.Thumb = j.Value("thumbnail")
                                        xmm.Title = TitleHtmlConverter(j.Value("title").IfNullOrEmpty(j.Value("fulltitle")))
                                        If Not xmm.Title.IsEmptyString Then m.File.Name = xmm.Title
                                        m.Type = xmm.Type
                                        m.URL = IIf(IsSubscription, xmm.Thumb, xmm.URL)
                                        m.Object = xmm
                                        m.SpecialFolder = SpecFolder
                                        Return True
                                    End If
                                End If
                            End If
                        End Using
                    End If
                End If
                Return False
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.ReturnValue, ex, $"[{ToStringForLog()}]: API.Xhamster.GetM3U8_Init({URL})", False)
            End Try
        End Function
        Private Function GetMediaInfo(ByVal URL As String, ByVal n As Integer, ByRef IsInternal As Boolean) As String
            Try
                If Not URL.IsEmptyString Then
                    Dim r$ = String.Empty
                    Dim f As SFile
                    If IsSubscription Then
                        Try
                            If Not CBool(MySettings.UseYTDLPForceDisableInternal.Value) Then r = Responser.GetResponse(URL)
                        Catch exr As Exception
                            ErrorsDescriber.Execute(EDP.SendToLog, exr, $"[{ToStringForLog()}]: API.Xhamster.GetMediaInfo({URL})", False)
                        End Try
                        If Not r.IsEmptyString Then r = RegexReplace(r, HtmlScript)
                        If Not r.IsEmptyString Then
                            IsInternal = True
                            Return r
                        Else
                            f = YTDLPGetInfo(URL, n)
                            If f.Exists Then IsInternal = False : Return f.GetText
                        End If
                    Else
                        f = YTDLPGetInfo(URL, n)
                        If f.Exists Then IsInternal = False : Return f.GetText
                    End If
                End If
                Return String.Empty
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, $"[{ToStringForLog()}]: API.Xhamster.GetMediaInfo({URL})", String.Empty)
            End Try
        End Function
        Private Function GetM3U8_Internal(ByRef m As UserMedia, ByVal URL As String, ByVal j As EContainer, ByVal SpecFolder As String) As Boolean
            Try
                If j.ListExists Then
                    m = ExtractMedia(j("videoModel"), UTypes.VideoPre,,,, SpecFolder)
                    m.URL_BASE = URL
                    m.SpecialFolder = SpecFolder
                    If IsSubscription Then
                        With j("videoModel")
                            If .ListExists Then
                                m.URL = .Value("thumbURL").IfNullOrEmpty(.Value("previewThumbURL"))
                                m.Object = New XMMediaInfo With {
                                    .IsInternal = True,
                                    .Thumb = m.URL,
                                    .URL = URL,
                                    .Type = UTypes.VideoPre
                                }
                                Return Not m.URL.IsEmptyString
                            End If
                        End With
                    Else
                        Return GetM3U8_Internal_GetURL(m, j)
                    End If
                End If
                Return False
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.ReturnValue, ex, $"[{ToStringForLog()}]: API.Xhamster.GetM3U8_Internal({URL})", False)
            End Try
        End Function
        Private Function GetM3U8_Internal_GetURL(ByRef m As UserMedia, ByVal j As EContainer, Optional ByVal r As Integer = 0) As Boolean
            Const urlNode$ = "url"
            Dim node As EContainer = j({"xplayerSettings", "sources", If(r = 0, "hls", "standard")})
            Dim t As UTypes = UTypes.Undefined
            If node.ListExists Then
                Dim url$ 'node.GetNode({New NodeParams("url", True, True, True, True, 2)}).XmlIfNothingValue
                Dim jn As EContainer, jn2 As EContainer
                Dim __getUrl As Func(Of EContainer, String) = Function(jj) If(jj.Contains(urlNode), jj.Value(urlNode), String.Empty)
                url = __getUrl(node)
                If url.IsEmptyString Then
                    For Each jn In node
                        If jn.Contains(urlNode) Then
                            url = __getUrl(jn)
                        ElseIf jn.Count > 0 Then
                            For Each jn2 In jn
                                url = __getUrl(jn2)
                                If Not url.IsEmptyString Then Exit For
                            Next
                        End If
                        If Not url.IsEmptyString Then Exit For
                    Next
                End If
                If Not url.IsEmptyString Then
                    If url.ToLower.EndsWith(".m3u8") Then
                        t = UTypes.m3u8
                    ElseIf url.ToLower.EndsWith(".mp4") Then
                        t = UTypes.Video
                    End If
                    If Not t = UTypes.Undefined Then
                        m.URL = url
                        m.Type = t
                        m.Object = New XMMediaInfo With {
                            .IsInternal = True,
                            .Type = t,
                            .URL = url,
                            .Thumb = j.Value({"videoModel"}, "thumbURL").IfNullOrEmpty(j.Value({"videoModel"}, "previewThumbURL"))
                        }
                        Return True
                    End If
                End If
            End If
            If r = 0 Then Return GetM3U8_Internal_GetURL(m, j, r + 1)
            Return False
        End Function
#End Region
#Region "yt-dlp support"
        Private Function YTDLPGetInfo(ByVal URL As String, ByVal n As Integer) As SFile
            Try
                Dim cc As CacheKeeper
                If IsSingleObjectDownload Then
                    cc = Settings.Cache
                Else
                    If MyCache Is Nothing Then MyCache = CreateCache() : MyCache.Validate()
                    cc = MyCache
                End If
                Dim path As SFile = cc.NewPath
                Dim c$ = If(MySettings.CookiesNetscapeFile.Exists, $" --no-cookies-from-browser --cookies ""{MySettings.CookiesNetscapeFile}""", String.Empty)
                Dim cmd$ = $"{Settings.YtdlpFile} --write-info-json --skip-download{c} {URL} -o ""{path.PathWithSeparator}file"""
                path.Exists()
                Using ytdlp As New YTDLP.YTDLPBatch(TokenPersonal,, path) : ytdlp.Encoding = Settings.CMDEncoding : ytdlp.Execute(cmd) : End Using
                Return SFile.GetFiles(path, "*.json",, EDP.ReturnValue).FirstOrDefault
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, $"API.Xhamster.UserData.YTDLPGetInfo({URL})", New SFile)
            End Try
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
        Private Function XMMObjectExists(ByVal Media As UserMedia) As Boolean
            Return Not IsNothing(Media.Object) AndAlso TypeOf Media.Object Is XMMediaInfo
        End Function
        Protected Overrides Function DownloadM3U8(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile, ByVal Token As CancellationToken) As SFile
            If CBool(MySettings.UseYTDLPDownload.Value) Then
                If XMMObjectExists(Media) Then Return YTDLPDownload(Media, DestinationFile, Token)
                Return Nothing
            Else
                Media.File = DestinationFile
                Return M3U8.Download(Media, Responser, MySettings.DownloadUHD.Value, Token, Progress, Not IsSingleObjectDownload, MySettings.ReencodeVideos.Value)
            End If
        End Function
        Protected Overrides Function ValidateDownloadFile(ByVal URL As String, ByVal Media As UserMedia, ByRef Interrupt As Boolean) As Boolean
            If Not Media.IsPhotoType AndAlso CBool(MySettings.UseYTDLPDownload.Value) Then
                If Not Media.URL_BASE.IsEmptyString And XMMObjectExists(Media) AndAlso
                   Not DirectCast(Media.Object, XMMediaInfo).FormatID.IsEmptyString Then Return True
                Interrupt = True
            End If
            Return False
        End Function
        Protected Overrides Function DownloadFile(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile, ByVal Token As CancellationToken) As SFile
            Return YTDLPDownload(Media, DestinationFile, Token)
        End Function
        Private Function YTDLPDownload(ByVal Media As UserMedia, ByVal DestinationFile As SFile, ByVal Token As CancellationToken) As SFile
            DestinationFile.Extension = "mp4"
            Dim c$ = If(MySettings.CookiesNetscapeFile.Exists, $" --no-cookies-from-browser --cookies ""{MySettings.CookiesNetscapeFile}""", String.Empty)
            Dim cmd$ = $"{Settings.YtdlpFile} --format {DirectCast(Media.Object, XMMediaInfo).FormatID}{c} {Media.URL_BASE} -o ""{DestinationFile}"""
            Using ytdlp As New YTDLP.YTDLPBatch(TokenPersonal,, DestinationFile) : ytdlp.Encoding = Settings.CMDEncoding : ytdlp.Execute(cmd) : End Using
            Return DestinationFile
        End Function
#End Region
#Region "Create media"
        Private Function ExtractMedia(ByVal j As EContainer, ByVal t As UTypes, Optional ByVal UrlNode As String = "pageURL",
                                      Optional ByVal DetectGalery As Boolean = True, Optional ByVal PostDate As Date? = Nothing,
                                      Optional ByVal SpecFolder As String = Nothing) As UserMedia
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
                    If setSpecialFolder Then m.SpecialFolder = IIf(t = UTypes.Picture, "Photo\", String.Empty) & m.PictureOption

                    If processFile Then
                        If Not m.PictureOption.IsEmptyString Then
                            m.File = $"{m.PictureOption}.{ext}"
                        ElseIf Not m.Post.ID.IsEmptyString Then
                            m.File = $"{m.Post.ID}.{ext}"
                        End If
                    End If
                    m.File.Separator = "\"
                End If
                If Not SpecFolder.IsEmptyString Then _
                   m.SpecialFolder = $"{m.SpecialFolder.StringTrimEnd("\")}{IIf(m.SpecialFolder.IsEmptyString, String.Empty, "\")}{SpecFolder}"
                Return m
            Else
                Return Nothing
            End If
        End Function
#End Region
#Region "Exception"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            '8, 503
            Return If(Responser.Status = Net.WebExceptionStatus.ConnectionClosed Or Responser.StatusCode = Net.HttpStatusCode.ServiceUnavailable, 1, 0)
        End Function
#End Region
#Region "IDisposable support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then _TempPhotoData.Clear() : SessionPosts.Clear() : ResetCache()
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace