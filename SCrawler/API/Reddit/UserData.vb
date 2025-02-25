' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Net
Imports System.Threading
Imports SCrawler.API.Base
Imports SCrawler.API.Reddit.RedditViewExchange
Imports SCrawler.API.YouTube.Objects
Imports SCrawler.Plugin.Hosts
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.ImageRenderer
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports UStates = SCrawler.API.Base.UserMedia.States
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Imports CView = SCrawler.API.Reddit.IRedditView.View
Imports CPeriod = SCrawler.API.Reddit.IRedditView.Period
Namespace API.Reddit
    Friend Class UserData : Inherits UserDataBase : Implements IChannelLimits, IRedditView
#Region "Declarations"
        Private Const CannelsLabelName As String = "Channels"
        Friend Const CannelsLabelName_ChannelsForm As String = "RChannels"
        Private ReadOnly Property MySiteSettings As SiteSettings
            Get
                Return DirectCast(HOST.Source, SiteSettings)
            End Get
        End Property
        Private ReadOnly Property DateTrueProvider(ByVal IsChannel As Boolean) As IFormatProvider
            Get
                Return If(IsChannel, UnixDate32ProviderReddit, UnixDate64Provider)
            End Get
        End Property
        Private ReadOnly Property UseM3U8 As Boolean
            Get
                Return Settings.UseM3U8 And CBool(DirectCast(HOST.Source, SiteSettings).UseM3U8.Value)
            End Get
        End Property
        Friend Property IsChannel As Boolean = False
        Friend Overrides ReadOnly Property SpecialLabels As IEnumerable(Of String)
            Get
                Return {CannelsLabelName, CannelsLabelName_ChannelsForm, UserLabelName}
            End Get
        End Property
        Private _RedGifsAccount As String = String.Empty
        Friend Property RedGifsAccount As String Implements IRedditView.RedGifsAccount
            Get
                If Not _RedGifsAccount.IsEmptyString Then
                    Return _RedGifsAccount
                ElseIf Not ChannelInfo Is Nothing Then
                    Return ChannelInfo.RedGifsAccount
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal acc As String)
                _RedGifsAccount = acc
            End Set
        End Property
        Private _RedditAccount As String = String.Empty
        Friend Property RedditAccount As String Implements IRedditView.RedditAccount
            Get
                If IsChannelForm Then
                    Return _RedditAccount
                Else
                    Return MyBase.AccountName
                End If
            End Get
            Set(ByVal acc As String)
                _RedditAccount = acc
            End Set
        End Property
        Friend Overrides Property AccountName As String
            Get
                Return RedditAccount
            End Get
            Set(ByVal acc As String)
                MyBase.AccountName = acc
            End Set
        End Property
#End Region
#Region "Channels Support"
#Region "IChannelLimits Support"
        Friend Property DownloadLimitCount As Integer? Implements IChannelLimits.DownloadLimitCount
        Friend Property DownloadLimitPost As String Implements IChannelLimits.DownloadLimitPost
        Friend Property DownloadLimitDate As Date? Implements IChannelLimits.DownloadLimitDate
        Friend Overloads Sub SetLimit(Optional ByVal MaxPost As String = "", Optional ByVal MaxCount As Integer? = Nothing,
                                      Optional ByVal MinDate As Date? = Nothing) Implements IChannelLimits.SetLimit
            DownloadLimitPost = MaxPost
            DownloadLimitCount = MaxCount
            DownloadLimitDate = MinDate
        End Sub
        Friend Overloads Sub SetLimit(ByVal Source As IChannelLimits) Implements IChannelLimits.SetLimit
            With Source
                DownloadLimitCount = .DownloadLimitCount
                DownloadLimitPost = .DownloadLimitPost
                DownloadLimitDate = .DownloadLimitDate
                AutoGetLimits = .AutoGetLimits
            End With
        End Sub
        Friend Property AutoGetLimits As Boolean = True Implements IChannelLimits.AutoGetLimits
        Private ReadOnly Property IsChannelForm As Boolean
            Get
                Return Not IsSavedPosts AndAlso IsChannel AndAlso Not ChannelInfo Is Nothing
            End Get
        End Property
#End Region
        Friend Property ChannelInfo As Channel
        Private ReadOnly ChannelPostsNames As List(Of String)
        Friend Property SkipExistsUsers As Boolean = False
        Private ReadOnly _ExistsUsersNames As List(Of String)
        Friend Property SaveToCache As Boolean = False
        Friend Function GetNewChannelPosts() As IEnumerable(Of UserPost)
            If _ContentNew.Count > 0 Then Return (From c As UserMedia In _ContentNew
                                                  Where Not c.Post.CachedFile.IsEmptyString And c.State = UStates.Downloaded
                                                  Select c.Post) Else Return Nothing
        End Function
#End Region
#Region "IRedditView Support"
        Friend Property ViewMode As CView Implements IRedditView.ViewMode
        Friend Property ViewPeriod As CPeriod Implements IRedditView.ViewPeriod
        Friend Sub SetView(ByVal Options As IRedditView) Implements IRedditView.SetView
            If Not Options Is Nothing Then
                ViewMode = Options.ViewMode
                ViewPeriod = Options.ViewPeriod
                RedGifsAccount = Options.RedGifsAccount
                RedditAccount = Options.RedditAccount
            End If
        End Sub
        Private ReadOnly Property View As String
            Get
                Select Case ViewMode
                    Case CView.Hot : Return "hot"
                    Case CView.Top : Return "top"
                    Case Else : Return "new"
                End Select
            End Get
        End Property
        Private ReadOnly Property Period As String
            Get
                If ViewMode = CView.Top Then
                    Select Case ViewPeriod
                        Case CPeriod.Hour : Return "hour"
                        Case CPeriod.Day : Return "day"
                        Case CPeriod.Week : Return "week"
                        Case CPeriod.Month : Return "month"
                        Case CPeriod.Year : Return "year"
                        Case Else : Return "all"
                    End Select
                Else
                    Return "all"
                End If
            End Get
        End Property
#End Region
#Region "Initializer"
        Friend Sub New()
            ChannelPostsNames = New List(Of String)
            _ExistsUsersNames = New List(Of String)
            _CrossPosts = New List(Of String)
            UseMD5Comparison = True
            StartMD5Checked = True
            RemoveExistingDuplicates = False
            UseInternalDownloadFileFunction = True
            UseInternalM3U8Function = True
        End Sub
#End Region
#Region "Load and Update user info"
        Private Function UpdateNames() As Boolean
            If NameTrue(True).IsEmptyString Then
                Dim n$() = Name.Split("@")
                If n.ListExists Then
                    If n.Length = 2 Then
                        NameTrue = n(0)
                        IsChannel = True
                    ElseIf IsChannel Then
                        NameTrue = Name
                    Else
                        NameTrue = n(0)
                    End If
                End If
                If Not IsSavedPosts Then
                    Dim l$ = IIf(IsChannel, CannelsLabelName, UserLabelName)
                    Settings.Labels.Add(l)
                    Labels.ListAddValue(l, LNC)
                    Labels.Sort()
                    Return True
                End If
            End If
            Return False
        End Function
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            With Container
                If Loading Then
                    ViewMode = .Value(Name_ViewMode).FromXML(Of Integer)(CInt(CView.New))
                    ViewPeriod = .Value(Name_ViewPeriod).FromXML(Of Integer)(CInt(CPeriod.All))
                    IsChannel = .Value(Name_IsChannel).FromXML(Of Boolean)(False)
                    RedGifsAccount = .Value(Name_RedGifsAccount)
                    RedditAccount = .Value(Name_RedditAccount)
                    UpdateNames()
                Else
                    If UpdateNames() Then .Value(Name_LabelsName) = LabelsString
                    .Add(Name_ViewMode, CInt(ViewMode))
                    .Add(Name_ViewPeriod, CInt(ViewPeriod))
                    .Add(Name_IsChannel, IsChannel.BoolToInteger)
                    .Add(Name_TrueName, NameTrue(True))
                    .Add(Name_RedGifsAccount, RedGifsAccount)
                    .Add(Name_RedditAccount, RedditAccount)
                End If
            End With
        End Sub
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return New RedditViewExchange With {.ViewMode = ViewMode, .ViewPeriod = ViewPeriod, .RedGifsAccount = RedGifsAccount, .RedditAccount = RedditAccount}
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            If Not Obj Is Nothing AndAlso TypeOf Obj Is IRedditView Then SetView(DirectCast(Obj, IRedditView))
        End Sub
#End Region
#Region "Download Overrides"
        Friend Overrides Sub DownloadData(ByVal Token As CancellationToken)
            Err429Count = 0
            _CrossPosts.Clear()
            If CreatedByChannel And Settings.FromChannelDownloadTopUse And Settings.FromChannelDownloadTop > 0 Then _
               DownloadTopCount = Settings.FromChannelDownloadTop.Value
            If IsChannel Or IsSavedPosts Then UseMD5Comparison = False
            If IsSavedPosts Then NameTrue = MySiteSettings.SavedPostsUserName.Value
            UpdateNames()
            If IsChannelForm Then
                UseMD5Comparison = False
                EnvirDownloadSet()
                If Not Responser Is Nothing Then Responser.Dispose()
                Responser = New Responser
                Responser.Copy(MySiteSettings.Responser)
                ChannelPostsNames.ListAddList(ChannelInfo.PostsAll.Select(Function(p) p.ID), LNC)
                If Not ViewMode = CView.New Then ChannelPostsNames.ListAddList(ChannelInfo.PostsNames, LNC)
                If SkipExistsUsers Then _ExistsUsersNames.ListAddList(Settings.UsersList.Select(Function(p) p.Name), LNC)
                DownloadDataF(Token)
                ReparseVideo(Token)
                _ContentNew.ListAddList(_TempMediaList, LAP.ClearBeforeAdd)
                DownloadContent(Token)
            Else
                MyBase.DownloadData(Token)
            End If
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            With MySiteSettings
                If IsSavedPosts Then
                    If Not CBool(.UseTokenForSavedPosts.Value) Then Responser.Headers.Remove(DeclaredNames.Header_Authorization)
                Else
                    If Not CBool(.UseCookiesForTimelines.Value) Then Responser.Cookies.Clear()
                    If Not CBool(.UseTokenForTimelines.Value) Then Responser.Headers.Remove(DeclaredNames.Header_Authorization)
                End If
            End With

            _TotalPostsDownloaded = 0
            If IsSavedPosts Then
                Responser.DecodersError = EDP.ReturnValue
                DownloadDataChannel(String.Empty, Token)
            ElseIf IsChannel Then
                If ChannelInfo Is Nothing Then
                    ChannelPostsNames.ListAddList(_TempPostsList, LNC)
                    If ChannelPostsNames.Count > 0 Then
                        DownloadLimitCount = Nothing
                        With _ContentList.Where(Function(c) c.Post.Date.HasValue)
                            If .Count > 0 Then DownloadLimitDate = .Max(Function(p) p.Post.Date.Value).AddMinutes(-10)
                        End With
                    End If
                    If DownloadTopCount.HasValue Then DownloadLimitCount = DownloadTopCount
                Else
                    GetUserInfo()
                End If
                If SaveToCache AndAlso Not Responser.Decoders.Contains(SymbolsConverter.Converters.HTML) Then _
                   Responser.Decoders.Add(SymbolsConverter.Converters.HTML)
                DownloadDataChannel(String.Empty, Token)
                If ChannelInfo Is Nothing Then _TempPostsList.ListAddList(_TempMediaList.Select(Function(m) m.Post.ID), LNC)
            Else
                GetUserInfo()
                DownloadDataUser(String.Empty, Token)
            End If
            ProgressPre.Done()
        End Sub
#End Region
#Region "Download Functions (User, Channel)"
        Private Err429Count As Integer = 0
        Private _TotalPostsDownloaded As Integer = 0
        Private ReadOnly _CrossPosts As List(Of String)
        Private Const SiteGfycatKey As String = "gfycat"
        Private Const SiteRedGifsKey As String = "redgifs"
        Private Const Node_CrosspostRootId As String = "crosspostRootId"
        Private Const Node_CrosspostParentId As String = "crosspostParentId"
        Private Const Node_CrosspostParent As String = "crosspost_parent"
        Private Sub DownloadDataUser(ByVal POST As String, ByVal Token As CancellationToken)
            Dim eObj% = 0
            Dim round% = 0
            Dim URL$ = String.Empty
            Dim _completed As Boolean = False
            Do
                round += 1
                Try
                    Dim PostID$ = String.Empty, PostTmp$ = String.Empty
                    Dim PostDate$
                    Dim n As EContainer, nn As EContainer
                    Dim NewPostDetected As Boolean = False
                    Dim ExistsDetected As Boolean = False
                    Dim IsCrossPost As Predicate(Of EContainer) = Function(e) Not e.Value(Node_CrosspostRootId).IsEmptyString Or Not e.Value(Node_CrosspostParentId).IsEmptyString Or Not e.Value(Node_CrosspostParent).IsEmptyString
                    Dim CheckNode As Predicate(Of EContainer) = Function(e) Not ParseUserMediaOnly OrElse If(e("author")?.Value, "/").ToLower.Equals(NameTrue.StringToLower)
                    Dim _PostID As Func(Of String) = Function() PostTmp.IfNullOrEmpty(PostID)

                    URL = $"https://gateway.reddit.com/desktopapi/v1/user/{NameTrue}/posts?rtj=only&allow_quarantined=true&allow_over18=1&include=identity&after={POST}&dist=25&sort={View}&t={Period}&layout=classic"
                    ThrowAny(Token)
                    Dim r$ = Responser.GetResponse(URL)
                    If Not r.IsEmptyString Then
                        Using w As EContainer = JsonDocument.Parse(r).XmlIfNothing
                            If w.Count > 0 Then
                                n = w.GetNode(JsonNodesJson)
                                If Not n Is Nothing AndAlso n.Count > 0 Then
                                    ProgressPre.ChangeMax(n.Count)
                                    For Each nn In n
                                        ProgressPre.Perform()
                                        ThrowAny(Token)
                                        If nn.Count > 0 Then
                                            If CheckNode(nn) Then

                                                'Obtain post ID
                                                PostTmp = nn.Name
                                                If PostTmp.IsEmptyString Then PostTmp = nn.Value("id")
                                                If PostTmp.IsEmptyString Then Continue For
                                                'Check for CrossPost
                                                If IsCrossPost(nn) Then
                                                    _CrossPosts.ListAddList({nn.Value(Node_CrosspostRootId),
                                                                             nn.Value(Node_CrosspostParentId),
                                                                             nn.Value(Node_CrosspostParent)}, LNC)
                                                    Continue For
                                                Else
                                                    If Not _CrossPosts.Contains(PostTmp) Then PostID = PostTmp : PostTmp = String.Empty
                                                End If

                                                'Download decision
                                                If Not _TempPostsList.Contains(_PostID()) Then
                                                    NewPostDetected = True
                                                    _TempPostsList.Add(_PostID())
                                                Else
                                                    If Not _CrossPosts.Contains(_PostID()) Then ExistsDetected = True
                                                    Continue For
                                                End If
                                                If nn.Contains("created") Then PostDate = nn("created").Value Else PostDate = String.Empty
                                                Select Case CheckDatesLimit(PostDate, DateTrueProvider(IsChannel))
                                                    Case DateResult.Skip : Continue For
                                                    Case DateResult.Exit : Exit Sub
                                                End Select

                                                ParseContainer(nn, _PostID(), PostDate)
                                            End If
                                        End If
                                    Next
                                End If
                            End If
                        End Using
                        If POST.IsEmptyString And ExistsDetected Then Exit Sub
                        If Not _PostID().IsEmptyString And NewPostDetected Then DownloadDataUser(_PostID(), Token)
                    End If
                    _completed = True
                Catch ex As Exception
                    If ProcessException(ex, Token, $"data downloading error [{URL}]",, eObj) = HttpStatusCode.InternalServerError Then
                        If round = 2 Then eObj = HttpStatusCode.InternalServerError
                    Else
                        _completed = True
                    End If
                End Try
            Loop While Not _completed
        End Sub
        Private Sub DownloadDataChannel(ByVal POST As String, ByVal Token As CancellationToken)
            Const savedPostsSleepTimer% = 2000
            Dim eObj% = 0
            Dim round% = 0
            Dim URL$ = String.Empty
            Dim _completed As Boolean = False
            Do
                round += 1
                Try
                    Dim PostID$ = String.Empty
                    Dim PostDate$, _UserID$
                    Dim n As EContainer, nn As EContainer, s As EContainer
                    Dim NewPostDetected As Boolean = False
                    Dim ExistsDetected As Boolean = False
                    Dim eCount As Predicate(Of EContainer) = Function(e) e.Count > 0
                    Dim lDate As Date?

                    If IsSavedPosts Then
                        URL = $"https://www.reddit.com/user/{NameTrue}/saved.json?after={POST}"
                        If Not POST.IsEmptyString Then Thread.Sleep(savedPostsSleepTimer)
                    Else
                        URL = $"https://reddit.com/r/{NameTrue}/{View}.json?allow_quarantined=true&allow_over18=1&include=identity&after={POST}&dist=25&sort={View}&t={Period}&layout=classic"
                    End If

                    ThrowAny(Token)
                    Dim r$ = Responser.GetResponse(URL)
                    If IsSavedPosts Then Err429Count = 0
                    If Not r.IsEmptyString Then
                        Using w As EContainer = JsonDocument.Parse(r).XmlIfNothing
                            If w.Count > 0 Then
                                n = w.GetNode(ChannelJsonNodes)
                                If Not n Is Nothing AndAlso n.Count > 0 Then
                                    ProgressPre.ChangeMax(n.Count)
                                    For Each nn In n
                                        ProgressPre.Perform()
                                        ThrowAny(Token)
                                        s = nn.ItemF({eCount})
                                        If If(s?.Count, 0) > 0 Then
                                            PostID = s.Value("name")
                                            If PostID.IsEmptyString AndAlso s.Contains("id") Then PostID = s("id").Value

                                            If ChannelPostsNames.Contains(PostID) Then
                                                If ViewMode = CView.New Then ExistsDetected = True Else NewPostDetected = True 'bypass
                                                Continue For
                                            End If
                                            If DownloadLimitCount.HasValue AndAlso _TotalPostsDownloaded >= DownloadLimitCount.Value Then Exit Sub
                                            If Not DownloadLimitPost.IsEmptyString AndAlso DownloadLimitPost = PostID Then Exit Sub
                                            If ViewMode = CView.New AndAlso DownloadLimitDate.HasValue AndAlso _TempMediaList.Count > 0 Then
                                                With (From __u In _TempMediaList Where __u.Post.Date.HasValue Select __u.Post.Date.Value)
                                                    If .Count > 0 Then lDate = .Min Else lDate = Nothing
                                                End With
                                                If lDate.HasValue AndAlso lDate.Value <= DownloadLimitDate.Value Then Exit Sub
                                            End If

                                            If IsSavedPosts Then
                                                If Not _TempPostsList.Contains(PostID) Then
                                                    NewPostDetected = True
                                                    _TempPostsList.Add(PostID)
                                                Else
                                                    ExistsDetected = True
                                                    Continue For
                                                End If
                                            Else
                                                NewPostDetected = True
                                            End If

                                            If s.Contains("created") Then PostDate = s("created").Value Else PostDate = String.Empty
                                            _UserID = s.Value("author")

                                            If Not IsSavedPosts AndAlso SkipExistsUsers AndAlso _ExistsUsersNames.Count > 0 AndAlso
                                               Not _UserID.IsEmptyString AndAlso _ExistsUsersNames.Contains(_UserID) Then
                                                If Not IsSavedPosts AndAlso Not ChannelInfo Is Nothing Then _
                                                   ChannelInfo.ChannelExistentUserNames.ListAddValue(_UserID, LNC)
                                                Continue For
                                            End If

                                            ParseContainer(s, PostID, PostDate, _UserID)
                                        End If
                                    Next
                                End If
                            End If
                        End Using
                        If POST.IsEmptyString And ExistsDetected Then Exit Sub
                        If Not PostID.IsEmptyString And NewPostDetected Then DownloadDataChannel(PostID, Token)
                    End If
                    _completed = True
                Catch ex As Exception
                    Dim errValue% = ProcessException(ex, Token, $"{IIf(IsSavedPosts, "saved posts", "channel")} data downloading error [{URL}]",, eObj)
                    If errValue = HttpStatusCode.InternalServerError Then
                        If round = 2 Then eObj = HttpStatusCode.InternalServerError
                    ElseIf errValue = 429 And round = 0 Then
                        Thread.Sleep(savedPostsSleepTimer)
                        round += 1
                    Else
                        _completed = True
                    End If
                End Try
            Loop While Not _completed
        End Sub
#End Region
#Region "GetUserInfo"
        Private Sub GetUserInfo()
            Try
                If Not IsSavedPosts And ChannelInfo Is Nothing Then
                    Dim r$ = Responser.GetResponse($"https://reddit.com/{IIf(IsChannel, "r", "user")}/{NameTrue}/about.json",, EDP.ReturnValue)
                    If Not r.IsEmptyString Then
                        Using j As EContainer = JsonDocument.Parse(r)
                            If Not j Is Nothing AndAlso j.Contains({"data", "subreddit"}) Then
                                If ID.IsEmptyString Then ID = j.Value({"data"}, "id")
                                With j({"data", "subreddit"})
                                    UserSiteNameUpdate(.Value("title"))
                                    UserDescriptionUpdate(.Value("public_description"))
                                    Dim dir As SFile = MyFile.CutPath
                                    Dim fileCrFunc As Func(Of String, SFile) = Function(img) CreateFileFromUrl(img)
                                    If DownloadIconBanner Then
                                        SimpleDownloadAvatar(.Value("icon_img"), fileCrFunc)
                                        SimpleDownloadAvatar(.Value("banner_img"), fileCrFunc)
                                    End If
                                End With
                            End If
                        End Using
                    End If
                End If
            Catch ex As Exception
            End Try
        End Sub
#End Region
#Region "ParseContainer"
        Private Function ParseContainer(ByVal e As EContainer, ByVal PostID As String, ByVal PostDate As String, Optional ByVal UserID As String = Nothing,
                                        Optional ByVal AllowReparse As Boolean = True) As Boolean
            If Not e Is Nothing Then
                Dim UPicType As Func(Of String, UTypes) = Function(input) IIf(input = "image", UTypes.Picture, UTypes.GIF)
                Dim eCount As Predicate(Of EContainer) = Function(item) item.Count > 0
                Dim added As Boolean = True
                Dim tmpUrl$ = e.Value("url").IfNullOrEmpty(e.Value({"source"}, "url"))
                If Not tmpUrl.IsEmptyString AndAlso tmpUrl.StringContains({$"{SiteRedGifsKey}.com", $"{SiteGfycatKey}.com"}) Then
                    If SaveToCache Then
                        tmpUrl = e.Value({"media", "oembed"}, "thumbnail_url")
                        If Not tmpUrl.IsEmptyString Then
                            _TempMediaList.ListAddValue(MediaFromData(UTypes.Picture, tmpUrl, PostID, PostDate, UserID), LNC)
                            _TotalPostsDownloaded += 1
                        Else
                            added = False
                        End If
                    Else
                        _TempMediaList.ListAddValue(MediaFromData(UTypes.VideoPre, tmpUrl, PostID, PostDate, UserID), LNC)
                        _TotalPostsDownloaded += 1
                    End If
                ElseIf CreateImgurMedia(tmpUrl, PostID, PostDate, UserID, IsChannel) Then
                    _TotalPostsDownloaded += 1
                ElseIf DownloadGallery(e, PostID, PostDate, UserID, SaveToCache) Then
                    _TotalPostsDownloaded += 1
                ElseIf Not If(e({"media"}, "type")?.Value, String.Empty).IsEmptyString Then
                    With e("media")
                        Dim t$ = .Item("type").Value
                        Select Case t
                            Case "gallery" : If DownloadGallery(.Self, PostID, PostDate) Then _TotalPostsDownloaded += 1 Else added = False
                            Case "image", "gifvideo"

                                Dim resolution As Sizes = Nothing
                                Dim content As Sizes = Nothing
                                Dim chosenVal$ = String.Empty
                                ParseResolutions(e("media"), e("preview"), resolution)
                                If .Contains("content") Then
                                    content = CreateSize(.Self, "content")
                                    If content.HasError Or content.Data.IsEmptyString Then content = Nothing
                                End If

                                If UPicType(t) = UTypes.Picture Then
                                    If Not content.Data.IsEmptyString Then
                                        If Not resolution.Data.IsEmptyString Then
                                            If content.Value >= resolution.Value AndAlso TryImage(content.Data) Then
                                                chosenVal = content.Data
                                            Else
                                                chosenVal = resolution.Data
                                            End If
                                        Else
                                            chosenVal = content.Data
                                        End If
                                    Else
                                        chosenVal = resolution.Data
                                    End If
                                Else
                                    If Not resolution.Data.IsEmptyString Then
                                        chosenVal = resolution.Data
                                    ElseIf Not content.Data.IsEmptyString Then
                                        chosenVal = content.Data
                                    End If
                                End If

                                If Not chosenVal.IsEmptyString Then
                                    _TempMediaList.ListAddValue(MediaFromData(UPicType(t), chosenVal, PostID, PostDate, UserID), LNC)
                                    _TotalPostsDownloaded += 1
                                Else
                                    added = False
                                End If
                            Case "video"
                                If UseM3U8 AndAlso .Item("hlsUrl").XmlIfNothingValue("/").ToLower.Contains("m3u8") Then
                                    _TempMediaList.ListAddValue(MediaFromData(UTypes.m3u8, .Value("hlsUrl"), PostID, PostDate, UserID), LNC)
                                    _TotalPostsDownloaded += 1
                                ElseIf Not UseM3U8 AndAlso .Item("fallback_url").XmlIfNothingValue("/").ToLower.Contains("mp4") Then
                                    _TempMediaList.ListAddValue(MediaFromData(UTypes.Video, .Value("fallback_url"), PostID, PostDate, UserID), LNC)
                                    _TotalPostsDownloaded += 1
                                Else
                                    added = False
                                End If
                            Case Else : added = False
                        End Select
                    End With
                ElseIf Not If(e({"media", "reddit_video"}, "fallback_url")?.Value, String.Empty).IsEmptyString Then
                    tmpUrl = e({"media", "reddit_video"}, "fallback_url").Value
                    If SaveToCache Then
                        tmpUrl = GetVideoRedditPreview(e)
                        If Not tmpUrl.IsEmptyString Then
                            _TempMediaList.ListAddValue(MediaFromData(UTypes.Picture, tmpUrl, PostID, PostDate, UserID, False), LNC)
                            _TotalPostsDownloaded += 1
                        Else
                            added = False
                        End If
                    ElseIf UseM3U8 AndAlso Not If(e({"media", "reddit_video"}, "hls_url")?.Value, String.Empty).IsEmptyString Then
                        _TempMediaList.ListAddValue(MediaFromData(UTypes.m3u8, e.Value({"media", "reddit_video"}, "hls_url"), PostID, PostDate, UserID), LNC)
                        _TotalPostsDownloaded += 1
                    Else
                        _TempMediaList.ListAddValue(MediaFromData(UTypes.Video, tmpUrl, PostID, PostDate, UserID), LNC)
                        _TotalPostsDownloaded += 1
                    End If
                Else
                    added = False
                End If
                If Not added Then
                    If AllowReparse Then
                        If If(e.ItemF({"crosspost_parent_list", 0})?.Count, 0) > 0 Then
                            added = ParseContainer(e.ItemF({"crosspost_parent_list", 0}), PostID, PostDate, UserID, True)
                        Else
                            Dim tPostId$ = e.Value(Node_CrosspostParent).IfNullOrEmpty(e.Value(Node_CrosspostParentId)).IfNullOrEmpty(e.Value(Node_CrosspostRootId))
                            If Not PostID.IsEmptyString Then
                                Dim r$ = Responser.GetResponse($"https://www.reddit.com/comments/{tPostId.Split("_").LastOrDefault}/.json",, EDP.ReturnValue)
                                If Not r.IsEmptyString Then
                                    Using j As EContainer = JsonDocument.Parse(r, EDP.ReturnValue)
                                        If j.ListExists Then
                                            With j.ItemF({0, "data", "children", 0, "data"})
                                                If .ListExists Then added = ParseContainer(.Self, PostID, PostDate, UserID, False)
                                            End With
                                        End If
                                    End Using
                                End If
                            End If
                        End If
                    End If
                    If Not added Then
                        Dim node As EContainer = e({"source", "url"})
                        Dim tmpType As UTypes = UTypes.Undefined
                        If Not If(node?.Value, String.Empty).IsEmptyString Then
                            With node.Value.ToLower
                                Select Case True
                                    Case .Contains(SiteRedGifsKey), .Contains(SiteGfycatKey) : If Not SaveToCache Then tmpType = UTypes.VideoPre
                                    Case .Contains("m3u8") : If Settings.UseM3U8 And Not SaveToCache Then tmpType = UTypes.m3u8
                                    Case .Contains(".gif") And TryFile(node.Value) : tmpType = UTypes.GIF
                                    Case TryFile(node.Value) : tmpType = UTypes.Picture
                                    Case Else : tmpType = UTypes.Undefined
                                End Select
                            End With
                            If Not tmpType = UTypes.Undefined Then
                                _TempMediaList.ListAddValue(MediaFromData(tmpType, node.Value, PostID, PostDate, UserID), LNC)
                                added = True
                            End If
                        End If
                        If Not added And e.Contains("preview") Then
                            With e.ItemF({"preview", "images", eCount})
                                If .ListExists Then
                                    tmpType = UTypes.Undefined
                                    tmpUrl = String.Empty
                                    Dim sv$ = .Value({"source"}, "url")
                                    If Not sv.IsEmptyString AndAlso sv.Contains(".gif") Then
                                        tmpUrl = .Value({"variants", "gif", "source"}, "url")
                                        If Not tmpUrl.IsEmptyString Then tmpType = UTypes.GIF
                                    End If
                                    If tmpUrl.IsEmptyString Then
                                        tmpUrl = .Value({"variants", "mp4", "source"}, "url")
                                        If Not tmpUrl.IsEmptyString Then tmpType = UTypes.Video
                                    End If
                                    If tmpUrl.IsEmptyString Then
                                        tmpUrl = .Value({"source"}, "url")
                                        If Not tmpUrl.IsEmptyString Then tmpType = UTypes.Picture
                                    End If
                                    If Not tmpUrl.IsEmptyString And Not tmpType = UTypes.Undefined Then
                                        Dim m As UserMedia = MediaFromData(tmpType, tmpUrl, PostID, PostDate, UserID)
                                        If tmpType = UTypes.Video Then m.File.Extension = "mp4"
                                        _TempMediaList.ListAddValue(m, LNC)
                                        _TotalPostsDownloaded += 1
                                        added = True
                                    End If
                                End If
                            End With
                        End If
                    End If
                End If
                Return added
            Else
                Return False
            End If
        End Function
        Private Function TryImage(ByVal URL As String) As Boolean
            Try
                If Not CBool(MySiteSettings.CheckImage.Value) Then
                    Return MySiteSettings.CheckImageReturnOrig.Value
                Else
                    Dim img As Image = GetImage(SFile.GetBytesFromNet(URL, EDP.ThrowException), EDP.ThrowException)
                    If Not img Is Nothing Then
                        img.Dispose()
                        Return True
                    Else
                        Return False
                    End If
                End If
            Catch
                Return False
            End Try
        End Function
#End Region
#Region "Download Base Functions"
        Private Function CreateImgurMedia(ByVal _URL As String, ByVal PostID As String, ByVal PostDate As String,
                                          Optional ByVal _UserID As String = "", Optional ByVal IsChannel As Boolean = False) As Boolean
            If Not _URL.IsEmptyString AndAlso _URL.Contains("imgur") Then
                If _URL.StringContains({".jpg", ".png", ".jpeg"}) Then
                    _TempMediaList.ListAddValue(MediaFromData(UTypes.Picture, _URL, PostID, PostDate, _UserID), LNC)
                ElseIf _URL.Contains(".gifv") Then
                    If SaveToCache Then
                        _TempMediaList.ListAddValue(MediaFromData(UTypes.Picture, _URL.Replace(".gifv", ".gif"), PostID, PostDate, _UserID), LNC)
                    Else
                        _TempMediaList.ListAddValue(MediaFromData(UTypes.Video, _URL.Replace(".gifv", ".mp4"), PostID, PostDate, _UserID), LNC)
                    End If
                ElseIf _URL.Contains(".mp4") Then
                    _TempMediaList.ListAddValue(MediaFromData(UTypes.Video, _URL, PostID, PostDate, _UserID), LNC)
                ElseIf _URL.Contains(".gif") Then
                    _TempMediaList.ListAddValue(MediaFromData(UTypes.GIF, _URL, PostID, PostDate, _UserID), LNC)
                Else
                    Dim obj As IEnumerable(Of UserMedia) = Imgur.Envir.GetVideoInfo(_URL, EDP.ReturnValue)
                    If Not obj.ListExists Then
                        If Not TryFile(_URL) Then _URL &= ".jpg"
                        _TempMediaList.ListAddValue(MediaFromData(UTypes.Picture, _URL, PostID, PostDate, _UserID), LNC)
                    Else
                        Dim ut As UTypes
                        Dim m As UserMedia
                        For Each data As UserMedia In obj
                            With data
                                If Not .URL.IsEmptyString Then
                                    If Not .File.IsEmptyString Then
                                        Select Case .File.Extension
                                            Case "jpg", "png", "jpeg" : ut = UTypes.Picture
                                            Case "gifv" : ut = IIf(SaveToCache, UTypes.Picture, UTypes.Video)
                                            Case "mp4" : ut = UTypes.Video
                                            Case "gif" : ut = UTypes.GIF
                                            Case Else : ut = UTypes.Picture : .File.Extension = "jpg"
                                        End Select
                                        m = MediaFromData(ut, _URL, PostID, PostDate, _UserID)
                                        m.URL = .URL
                                        m.File = .File.File
                                        _TempMediaList.ListAddValue(m, LNC)
                                    End If
                                End If
                            End With
                        Next
                    End If
                End If
                Return True
            Else
                Return False
            End If
        End Function
        Private Function DownloadGallery(ByVal e As EContainer, ByVal PostID As String, ByVal PostDate As String,
                                         Optional ByVal _UserID As String = Nothing, Optional ByVal FirstOnly As Boolean = False) As Boolean
            Try
                Dim added As Boolean = False
                Dim node As EContainer = Nothing
                If e.Contains("media_metadata") Then
                    node = e("media_metadata")
                ElseIf e.Contains("mediaMetadata") Then
                    node = e("mediaMetadata")
                End If
                If If(node?.Count, 0) > 0 Then
                    Dim t As EContainer
                    For Each n As EContainer In node
                        t = n.ItemF({"s", "u"})
                        If Not t Is Nothing AndAlso Not t.Value.IsEmptyString Then
                            _TempMediaList.ListAddValue(MediaFromData(UTypes.Picture, t.Value, PostID, PostDate, _UserID), LNC)
                            added = True
                            If FirstOnly Then Exit For
                        End If
                    Next
                End If
                Return added
            Catch ex As Exception
                ProcessException(ex, Nothing, "gallery parsing error", False)
                Return False
            End Try
        End Function
        Private Function GetVideoRedditPreview(ByVal Node As EContainer) As String
            Try
                If Not Node Is Nothing Then
                    Dim n As EContainer = Node.ItemF({"preview", "images", 0})
                    Dim DestNode$() = Nothing
                    If If(n?.Count, 0) > 0 Then Return ParseResolutions(n)
                End If
                Return String.Empty
            Catch ex As Exception
                ProcessException(ex, Nothing, "reddit video preview parsing error", False)
                Return String.Empty
            End Try
        End Function
        Private Function ParseResolutions(ByVal Node As EContainer, Optional ByVal PreviewNode As EContainer = Nothing,
                                          Optional ByRef SResult As Sizes = Nothing) As String
            Try
                If If(Node?.Count, 0) > 0 Then
                    Dim DestNode$() = Nothing
                    If If(Node("resolutions")?.Count, 0) > 0 Then
                        DestNode = {"resolutions"}
                    ElseIf If(Node({"variants", "nsfw", "resolutions"})?.Count, 0) > 0 Then
                        DestNode = {"variants", "nsfw", "resolutions"}
                    End If
                    If Not DestNode Is Nothing Then
                        With Node(DestNode)
                            Dim sl As List(Of Sizes) = .Select(Function(e) CreateSize(e)).
                                                        ListWithRemove(Function(ss) ss.HasError Or ss.Data.IsEmptyString)
                            If If(PreviewNode?.Count, 0) > 0 Then
                                Dim sp As Sizes = CreateSize(PreviewNode)
                                If Not sp.HasError And Not sp.Data.IsEmptyString Then
                                    If sl Is Nothing Then sl = New List(Of Sizes)
                                    sl.Add(sp)
                                End If
                            End If
                            If sl.ListExists Then
                                Dim s As Sizes
                                sl.Sort()
                                s = sl.First
                                sl.Clear()
                                SResult = s
                                Return s.Data
                            End If
                        End With
                    End If
                End If
                Return String.Empty
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function
        Private Function CreateSize(ByVal Node As EContainer, Optional ByVal UrlNodeName As String = "url") As Sizes
            Return New Sizes(Node.Value("width"), Node.Value(UrlNodeName))
        End Function
#End Region
#Region "ReparseVideo"
        Protected Overrides Sub ReparseVideo(ByVal Token As CancellationToken)
            Dim RedGifsResponser As Responser = Nothing
            Try
                ThrowAny(Token)
                Const v2 As UTypes = UTypes.VideoPre + UTypes.m3u8
                If _TempMediaList.Count > 0 AndAlso _TempMediaList.Exists(Function(p) p.Type = UTypes.VideoPre Or p.Type = v2) Then
                    Dim r$, v$
                    Dim e As New ErrorsDescriber(EDP.ReturnValue)
                    Dim m As UserMedia, m2 As UserMedia
                    Dim RedGifsHost As SettingsHost = Settings(RedGifs.RedGifsSiteKey, RedGifsAccount)
                    Dim _repeatForRedgifs As Boolean
                    If RedGifsHost Is Nothing Then RedGifsHost = Settings(RedGifs.RedGifsSiteKey).Default
                    RedGifsResponser = RedGifsHost.Responser.Copy
                    ProgressPre.ChangeMax(_TempMediaList.Count)
                    For i% = _TempMediaList.Count - 1 To 0 Step -1
                        ThrowAny(Token)
                        ProgressPre.Perform()
                        If _TempMediaList(i).Type = UTypes.VideoPre Or _TempMediaList(i).Type = v2 Then
                            m = _TempMediaList(i)
                            If _TempMediaList(i).Type = UTypes.VideoPre Then
                                Do
                                    _repeatForRedgifs = False
                                    If m.URL.Contains($"{SiteGfycatKey}.com") Then
                                        r = Gfycat.Envir.GetVideo(m.URL)
                                        If Not r.IsEmptyString AndAlso r.Contains("redgifs.com") Then m.URL = r : _repeatForRedgifs = True
                                    ElseIf m.URL.Contains(SiteRedGifsKey) Then
                                        m2 = RedGifs.UserData.GetDataFromUrlId(m.URL, False, RedGifsResponser, RedGifsHost, RedGifsAccount)
                                        If m2.State = UStates.Missing Then
                                            m.State = UStates.Missing
                                            _ContentList.Add(m)
                                            _TempMediaList.RemoveAt(i)
                                        ElseIf m2.State = RedGifs.UserData.DataGone Then
                                            _TempMediaList.RemoveAt(i)
                                        Else
                                            m2.URL_BASE = m.URL
                                            m2.Post = m.Post
                                            _TempMediaList(i) = m2
                                        End If
                                        Continue For
                                    Else
                                        r = Responser.GetResponse(m.URL,, e)
                                    End If
                                Loop While _repeatForRedgifs
                            Else
                                r = m.URL
                            End If
                            _TempMediaList(i) = New UserMedia
                            If Not r.IsEmptyString Then
                                v = RegexReplace(r, VideoRegEx)
                                If Not v.IsEmptyString Then
                                    _TempMediaList(i) = New UserMedia With {.Type = UTypes.Video, .URL = v, .File = v, .Post = m.Post}
                                Else
                                    _TempMediaList.RemoveAt(i)
                                End If
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex, Token, "video reparsing error", False)
            Finally
                If Not RedGifsResponser Is Nothing Then RedGifsResponser.Dispose()
                ProgressPre.Done()
            End Try
        End Sub
#End Region
#Region "ReparseMissing"
        Protected Overrides Sub ReparseMissing(ByVal Token As CancellationToken)
            Dim rList As New List(Of Integer)
            Dim RedGifsResponser As Responser = Nothing
            Try
                If Not ChannelInfo Is Nothing Or SaveToCache Then Exit Sub
                If ContentMissingExists Then
                    Dim RedGifsHost As SettingsHost = Settings(RedGifs.RedGifsSiteKey, RedGifsAccount)
                    If RedGifsHost Is Nothing Then RedGifsHost = Settings(RedGifs.RedGifsSiteKey).Default
                    RedGifsResponser = RedGifsHost.Responser.Copy
                    Dim respNoHeaders As Responser = Responser.Copy
                    Dim m As UserMedia, m2 As UserMedia
                    Dim r$, url$
                    Dim j As EContainer
                    Dim lastCount%, li%
                    Dim rv As New ErrorsDescriber(EDP.ReturnValue)
                    respNoHeaders.Headers.Clear()
                    ProgressPre.ChangeMax(_ContentList.Count)
                    For i% = 0 To _ContentList.Count - 1
                        m = _ContentList(i)
                        ProgressPre.Perform()
                        If m.State = UStates.Missing AndAlso Not m.Post.ID.IsEmptyString Then
                            ThrowAny(Token)
                            url = $"https://www.reddit.com/comments/{m.Post.ID.Split("_").LastOrDefault}/.json"
                            r = Responser.GetResponse(url,, rv)
                            If r.IsEmptyString Then r = respNoHeaders.GetResponse(url,, rv)
                            If Not r.IsEmptyString Then
                                j = JsonDocument.Parse(r, rv)
                                If Not j Is Nothing Then
                                    If j.Count > 0 Then
                                        lastCount = _TempMediaList.Count
                                        With j.GetNode(SingleJsonNodes)
                                            If .ListExists AndAlso ParseContainer(.Self, m.Post.ID, String.Empty) Then
                                                If lastCount <> _TempMediaList.Count Then
                                                    For li = IIf(lastCount < 0, 0, lastCount) To _TempMediaList.Count - 1
                                                        m2 = _TempMediaList(i)
                                                        m2.Post.Date = m.Post.Date
                                                        m2.State = UStates.Missing
                                                        m2.Attempts = m.Attempts
                                                        _TempMediaList(i) = m2
                                                    Next
                                                End If
                                                rList.Add(i)
                                            End If
                                        End With
                                    End If
                                    j.Dispose()
                                End If
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex, Token, "missing data downloading error")
            Finally
                If Not RedGifsResponser Is Nothing Then RedGifsResponser.Dispose()
                If rList.Count > 0 Then
                    For i% = rList.Count - 1 To 0 Step -1 : _ContentList.RemoveAt(rList(i)) : Next
                    rList.Clear()
                End If
                ProgressPre.Done()
            End Try
        End Sub
#End Region
#Region "DownloadSingleObject"
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            Dim __id$ = RegexReplace(Data.URL, RParams.DMS("comments/([^/]+)", 1, EDP.ReturnValue))
            If Not __id.IsEmptyString Then
                User.File = Data.File
                User.File.Name = String.Empty
                User.File.Extension = String.Empty
                _ContentList.Add(New UserMedia With {.State = UStates.Missing, .Post = __id})
                ReparseMissing(Token)
                ReparseVideo(Token)
            End If
        End Sub
#End Region
#Region "Structure creator"
        Private Function MediaFromData(ByVal t As UTypes, ByVal _URL As String, ByVal PostID As String, ByVal PostDate As String,
                                       Optional ByVal _UserID As String = "", Optional ByVal ReplacePreview As Boolean = True) As UserMedia
            If _URL.IsEmptyString And t = UTypes.Picture Then Return Nothing
            _URL = LinkFormatterSecure(RegexReplace(_URL.Replace("\", String.Empty), LinkPattern))
            Dim m As New UserMedia(_URL, t) With {.Post = New UserPost With {.ID = PostID, .UserID = _UserID}}
            If t = UTypes.Picture Or t = UTypes.GIF Then m.File = CreateFileFromUrl(m.URL) Else m.File = Nothing
            If ReplacePreview And m.URL.Contains("preview") And Not t = UTypes.Picture Then m.URL = $"https://i.redd.it/{m.File.File}"
            If Not PostDate.IsEmptyString Then m.Post.Date = AConvert(Of Date)(PostDate, DateTrueProvider(IsChannel Or IsSavedPosts), Nothing) Else m.Post.Date = Nothing
            Return m
        End Function
        Private Function TryFile(ByVal URL As String) As Boolean
            Try
                Return Not URL.IsEmptyString AndAlso Not CreateFileFromUrl(URL).IsEmptyString
            Catch ex As Exception
                Return False
            End Try
        End Function
        Protected Overrides Function CreateFileFromUrl(ByVal URL As String) As SFile
            Return New SFile(CStr(RegexReplace(URL, FilesPattern)))
        End Function
#End Region
#Region "DownloadContent"
        Private _RedGifsResponser As Responser = Nothing
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            If _ContentNew.Count > 0 Then
                Try
                    If Not _RedGifsResponser Is Nothing Then _RedGifsResponser.Dispose()
                    _RedGifsResponser = If(Settings(RedGifs.RedGifsSiteKey, RedGifsAccount), Settings(RedGifs.RedGifsSiteKey).Default).Responser.Copy
                    DownloadContentDefault(Token)
                Finally
                    If Not _RedGifsResponser Is Nothing Then _RedGifsResponser.Dispose() : _RedGifsResponser = Nothing
                End Try
            End If
        End Sub
        Protected Overrides Function DownloadContentDefault_GetRootDir() As String
            If Not IsSavedPosts AndAlso (IsChannel And SaveToCache And Not ChannelInfo Is Nothing) Then
                Return ChannelInfo.CachePath.PathNoSeparator
            Else
                Return MyBase.DownloadContentDefault_GetRootDir()
            End If
        End Function
        Protected Overrides Sub DownloadContentDefault_PostProcessing(ByRef m As UserMedia, ByVal File As SFile, ByVal Token As CancellationToken)
            m.Post.CachedFile = File
            MyBase.DownloadContentDefault_PostProcessing(m, File, Token)
        End Sub
        Protected Overrides Function DownloadContentDefault_ProcessDownloadException() As Boolean
            Return Not IsChannel Or Not SaveToCache
        End Function
        Protected Overrides Function DownloadFile(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile, ByVal Token As CancellationToken) As SFile
            If _RedGifsResponser.DownloadFile(URL, DestinationFile, EDP.ThrowException) Then
                Return DestinationFile
            Else
                Return Nothing
            End If
        End Function
        Protected Overrides Function ValidateDownloadFile(ByVal URL As String, ByVal Media As UserMedia, ByRef Interrupt As Boolean) As Boolean
            Return URL.Contains(SiteRedGifsKey)
        End Function
        Protected Overrides Function DownloadM3U8(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile, ByVal Token As CancellationToken) As SFile
            Return M3U8.Download(URL, Media, DestinationFile, Token, Progress, Not IsSingleObjectDownload)
        End Function
        Protected Overrides Function ChangeFileNameByProvider(ByVal f As SFile, ByVal m As UserMedia) As SFile
            If Not IsChannel Or Not SaveToCache Then
                Return MyBase.ChangeFileNameByProvider(f, m)
            Else
                Return f
            End If
        End Function
#End Region
#Region "Exception"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            With Responser
                If .StatusCode = HttpStatusCode.NotFound Then '404
                    UserExists = False
                ElseIf .StatusCode = HttpStatusCode.Forbidden Then '403
                    UserSuspended = True
                ElseIf .StatusCode = HttpStatusCode.BadGateway Or .StatusCode = HttpStatusCode.ServiceUnavailable Then '502, 503
                    MyMainLOG = $"{ToStringForLog()}: [{CInt(Responser.StatusCode)}] Reddit is currently unavailable"
                    Throw New Plugin.ExitException With {.Silent = True}
                ElseIf .StatusCode = HttpStatusCode.GatewayTimeout Then '504
                    Return 1
                ElseIf .StatusCode = HttpStatusCode.Unauthorized Then '401
                    MyMainLOG = $"{ToStringForLog()}: [{CInt(Responser.StatusCode)}] Reddit credentials expired"
                    MySiteSettings.SessionInterrupted = True
                    Throw New Plugin.ExitException With {.Silent = True}
                ElseIf .StatusCode = HttpStatusCode.InternalServerError Then '500
                    If Not IsNothing(EObj) AndAlso IsNumeric(EObj) AndAlso CInt(EObj) = HttpStatusCode.InternalServerError Then Return 1
                    Return HttpStatusCode.InternalServerError
                ElseIf .StatusCode = 429 And IsSavedPosts And Err429Count = 0 Then
                    Err429Count += 1
                    Return 429
                ElseIf .StatusCode = 429 AndAlso
                       ((Not IsSavedPosts And CBool(MySiteSettings.UseTokenForTimelines.Value)) Or (IsSavedPosts And CBool(MySiteSettings.UseTokenForSavedPosts.Value))) AndAlso
                       Not MySiteSettings.CredentialsExists Then '429
                    MyMainLOG = $"{ToStringForLog()}: [{CInt(Responser.StatusCode)}] You should use OAuth authorization or disable " &
                                IIf(IsSavedPosts, "token usage for downloading saved posts", "the use of token and cookies for downloading timelines")
                    MySiteSettings.SessionInterrupted = True
                    Throw New Plugin.ExitException With {.Silent = True}
                Else
                    If Not FromPE Then LogError(ex, Message) : HasError = True
                    Return 0
                End If
            End With
            Return 1
        End Function
#End Region
#Region "IDisposable Support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then ChannelPostsNames.Clear() : _ExistsUsersNames.Clear() : _CrossPosts.Clear()
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace