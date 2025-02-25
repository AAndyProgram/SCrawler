' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports System.Text.RegularExpressions
Imports SCrawler.API.Base
Imports SCrawler.API.YouTube.Objects
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.Web.Documents
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports UStates = SCrawler.API.Base.UserMedia.States
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Imports PKV = SCrawler.API.Instagram.UserData.PostKV
Namespace API.Twitter
    Friend Class UserData : Inherits UserDataBase
#Region "XML names"
        Private Const Name_FirstDownloadComplete As String = "FirstDownloadComplete"
        Private Const Name_DownloadModel As String = "DownloadModel"
        Private Const Name_DownloadModelForceApply As String = "DownloadModelForceApply"
        Private Const Name_MediaModelAllowNonUserTweets As String = "MediaModelAllowNonUserTweets"
        Private Const Name_DownloadBroadcasts As String = "DownloadBroadcasts"
        Private Const Name_GifsDownload As String = "GifsDownload"
        Private Const Name_GifsSpecialFolder As String = "GifsSpecialFolder"
        Private Const Name_GifsPrefix As String = "GifsPrefix"
        Private Const Name_IsCommunity As String = "IsCommunity"
        Private Const Name_DownloadModelChanged As String = "DownloadModelChanged"
#End Region
#Region "Declarations"
        Private Const BroadCastPartUrl As String = "i/broadcasts"
        Private Const Label_Community As String = "Community"
        Friend Overrides ReadOnly Property SpecialLabels As IEnumerable(Of String)
            Get
                Return {Label_Community}
            End Get
        End Property
        Friend Enum DownloadModels As Integer
            Undefined = 0
            Media = 1
            Profile = 2
            Search = 5
            Likes = 10
        End Enum
        Private FirstDownloadComplete As Boolean = False
        Friend Property DownloadModelForceApply As Boolean = False
        Friend Property DownloadModel As DownloadModels = DownloadModels.Undefined
        Private ReadOnly Property IsMultiMode As Boolean
            Get
                Return EnumExtract(Of DownloadModels)(DownloadModel).ListIfNothing.Count > 1
            End Get
        End Property
        Private Property DownloadModelChanged As Boolean = False
        Friend Property MediaModelAllowNonUserTweets As Boolean = False
        Friend Property DownloadBroadcasts As Boolean = False
        Friend Property GifsDownload As Boolean = True
        Friend Property GifsSpecialFolder As String = String.Empty
        Friend Property GifsPrefix As String = String.Empty
        Friend Property IsCommunity As Boolean = False
        Private ReadOnly LikesPosts As List(Of String)
        Private ReadOnly PostsKV As List(Of PKV)
        Private ReadOnly _DataNames As List(Of String)
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return HOST.Source
            End Get
        End Property
        Private FileNameProvider As ANumbers = Nothing
        Private Sub ResetFileNameProvider(Optional ByVal GroupSize As Integer? = Nothing)
            FileNameProvider = New ANumbers With {.FormatOptions = ANumbers.Options.FormatNumberGroup + ANumbers.Options.Groups}
            FileNameProvider.GroupSize = If(GroupSize, 3)
        End Sub
        Private Function RenameGdlFile(ByVal Input As SFile, ByVal i As Integer) As SFile
            Return SFile.Rename(Input, $"{Input.PathWithSeparator}{i.NumToString(FileNameProvider)}.{Input.Extension}",, EDP.ThrowException)
        End Function
        Friend Function GetUserUrl() As String
            Return $"https://x.com{IIf(IsCommunity, SiteSettings.CommunitiesUser, String.Empty)}/{NameTrue}"
        End Function
#End Region
#Region "Exchange options"
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return New EditorExchangeOptions(Me) With {.SiteKey = HOST.Key}
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            If Not Obj Is Nothing AndAlso TypeOf Obj Is EditorExchangeOptions Then
                With DirectCast(Obj, EditorExchangeOptions)
                    GifsDownload = .GifsDownload
                    GifsSpecialFolder = .GifsSpecialFolder
                    GifsPrefix = .GifsPrefix
                    UseMD5Comparison = .UseMD5Comparison
                    RemoveExistingDuplicates = .RemoveExistingDuplicates
                    If RemoveExistingDuplicates Then StartMD5Checked = False
                    DownloadModel = DownloadModels.Undefined
                    DownloadModelForceApply = .DownloadModelForceApply
                    MediaModelAllowNonUserTweets = .MediaModelAllowNonUserTweets
                    DownloadBroadcasts = .DownloadBroadcasts
                    Dim dModel As DownloadModels = DownloadModel
                    If .DownloadModelMedia Then DownloadModel += DownloadModels.Media
                    If .DownloadModelProfile Or .DownloadBroadcasts Then DownloadModel += DownloadModels.Profile
                    If .DownloadModelSearch Then DownloadModel += DownloadModels.Search
                    If .DownloadModelLikes Then DownloadModel += DownloadModels.Likes
                    If Not dModel = DownloadModel Then DownloadModelChanged = True
                    NameTrue = .UserName
                End With
            End If
        End Sub
#End Region
#Region "Initializer, loader"
        Friend Sub New()
            _DataNames = New List(Of String)
            LikesPosts = New List(Of String)
            PostsKV = New List(Of PKV)
            UseInternalM3U8Function = True
        End Sub
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            With Container
                If Loading Then
                    DownloadBroadcasts = .Value(Name_DownloadBroadcasts).FromXML(Of Boolean)(False)
                    DownloadModelForceApply = .Value(Name_DownloadModelForceApply).FromXML(Of Boolean)(False)
                    DownloadModelChanged = .Value(Name_DownloadModelChanged).FromXML(Of Boolean)(False)
                    If .Contains(Name_FirstDownloadComplete) Then
                        FirstDownloadComplete = .Value(Name_FirstDownloadComplete).FromXML(Of Boolean)(False)
                        DownloadModel = .Value(Name_DownloadModel).FromXML(Of Integer)(DownloadModels.Undefined)
                    Else
                        FirstDownloadComplete = DownloadedVideos(True) + DownloadedPictures(True) > 0
                        If .Contains(Name_DownloadModel) Then
                            DownloadModel = .Value(Name_DownloadModel).FromXML(Of Integer)(DownloadModels.Undefined)
                        Else
                            If FirstDownloadComplete Then
                                DownloadModelForceApply = False
                                If ParseUserMediaOnly Then
                                    DownloadModel = DownloadModels.Media
                                Else
                                    DownloadModel = DownloadModels.Media + DownloadModels.Profile + DownloadModels.Search
                                End If
                            Else
                                DownloadModel = DownloadModels.Undefined
                            End If
                        End If
                    End If
                    GifsDownload = .Value(Name_GifsDownload).FromXML(Of Boolean)(True)
                    GifsSpecialFolder = .Value(Name_GifsSpecialFolder)
                    If Not .Contains(Name_GifsPrefix) Then
                        GifsPrefix = "GIF_"
                    Else
                        GifsPrefix = .Value(Name_GifsPrefix)
                    End If
                    UseMD5Comparison = .Value(Name_UseMD5Comparison).FromXML(Of Boolean)(False)
                    RemoveExistingDuplicates = .Value(Name_RemoveExistingDuplicates).FromXML(Of Boolean)(False)
                    StartMD5Checked = .Value(Name_StartMD5Checked).FromXML(Of Boolean)(False)
                    MediaModelAllowNonUserTweets = .Value(Name_MediaModelAllowNonUserTweets).FromXML(Of Boolean)(False)
                    IsCommunity = .Value(Name_IsCommunity).FromXML(Of Boolean)(False)
                Else
                    If Name.Contains("@") And Not IsCommunity Then
                        IsCommunity = True
                        NameTrue = Name.Split("@")(0)
                        ID = NameTrue
                        ParseUserMediaOnly = False
                        Labels.ListAddValue(Label_Community, LNC)
                        Labels.Sort()
                        .Add(Name_UserID, ID)
                        .Add(Name_LabelsName, LabelsString)
                        .Add(Name_ParseUserMediaOnly, ParseUserMediaOnly.BoolToInteger)
                    End If
                    .Add(Name_FirstDownloadComplete, FirstDownloadComplete.BoolToInteger)
                    .Add(Name_DownloadModelForceApply, DownloadModelForceApply.BoolToInteger)
                    .Add(Name_DownloadModelChanged, DownloadModelChanged.BoolToInteger)
                    .Add(Name_DownloadModel, CInt(DownloadModel))
                    .Add(Name_DownloadBroadcasts, DownloadBroadcasts.BoolToInteger)
                    .Add(Name_GifsDownload, GifsDownload.BoolToInteger)
                    .Add(Name_GifsSpecialFolder, GifsSpecialFolder)
                    .Add(Name_GifsPrefix, GifsPrefix)
                    .Add(Name_UseMD5Comparison, UseMD5Comparison.BoolToInteger)
                    .Add(Name_RemoveExistingDuplicates, RemoveExistingDuplicates.BoolToInteger)
                    .Add(Name_StartMD5Checked, StartMD5Checked.BoolToInteger)
                    .Add(Name_MediaModelAllowNonUserTweets, MediaModelAllowNonUserTweets.BoolToInteger)
                    .Add(Name_IsCommunity, IsCommunity.BoolToInteger)
                    .Add(Name_TrueName, NameTrue(True))
                End If
            End With
        End Sub
#End Region
#Region "Download functions"
        Private Function GetContainerSubnodes() As List(Of String())
            Return New List(Of String()) From {
                {{"content", "itemContent", "tweet_results", "result", "legacy"}},
                {{"content", "itemContent", "tweet_results", "result", "tweet", "legacy"}},
                {{"item", "itemContent", "tweet_results", "result", "legacy"}},
                {{"item", "itemContent", "tweet_results", "result", "tweet", "legacy"}}
            }
        End Function
        Private Function ExtractBroadcast(ByVal e As EContainer, Optional ByVal PostID As String = Nothing, Optional ByVal PostDate As String = Nothing,
                                          Optional ByVal Nodes As List(Of String()) = Nothing,
                                          Optional ByVal IgnoreNodes As Boolean = False) As UserMedia
            If e.ListExists Then
                Dim __nodes As List(Of String()) = If(Nodes, GetContainerSubnodes())
                Dim urlValue$
                Dim m As UserMedia = Nothing
                Dim __parseContainer As Func(Of EContainer, Boolean) =
                    Function(ByVal ee As EContainer) As Boolean
                        With ee
                            If .ListExists Then
                                urlValue = .ItemF(BroadcastsUrls, EDP.ReturnValue).XmlIfNothingValue
                                If Not urlValue.IsEmptyString AndAlso urlValue.Contains(BroadCastPartUrl) Then
                                    m = MediaFromData(urlValue, PostID, PostDate,,, UTypes.m3u8)
                                    If Not IsSingleObjectDownload Then m.SpecialFolder = "Broadcasts*"
                                    Return True
                                End If
                            End If
                        End With
                        Return False
                    End Function
                If IgnoreNodes Then
                    If __parseContainer(e) Then Return m
                Else
                    For Each n As String() In __nodes
                        If __parseContainer(e(n)) Then Return m
                    Next
                End If
                m = ExtractBroadcast(e.ItemF(Of Object)({0}), PostID, PostDate, Nodes)
                If Not m.URL.IsEmptyString Then Return m
            End If
            Return Nothing
        End Function
        Private ReadOnly Property MyFilePostsKV As SFile
            Get
                Dim f As SFile = MyFilePosts
                If Not f.IsEmptyString Then
                    f.Name &= "_KV"
                    f.Extension = "xml"
                    Return f
                Else
                    Return Nothing
                End If
            End Get
        End Property
        Protected Sub LoadSavePostsKV(ByVal Load As Boolean)
            Instagram.UserData.LoadSavePostsKV(Load, MyFilePostsKV, PostsKV)
        End Sub
        Private Function PostKVExists(ByVal PID As String, ByVal Model As DownloadModels,
                                      ByVal MultiMode As Boolean, ByVal IgnorePKV As Boolean, ByVal AutoAdd As Boolean) As Boolean
            Dim result As Boolean
            If IgnorePKV Or PostsKV.Count = 0 Then
                result = _TempPostsList.Contains(PID)
            Else
                result = PostsKV.Contains(New PKV(PID, PID, Model)) Or (Not MultiMode AndAlso _TempPostsList.Contains(PID))
            End If
            If Not result And AutoAdd Then
                PostsKV.ListAddValue(New PKV(PID, PID, Model), LNC)
                _TempPostsList.ListAddValue(PID, LNC)
            End If
            Return result
        End Function
        Friend Property GDL_REQUESTS_COUNT As Integer = 0
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            Try
                GDL_REQUESTS_COUNT = 0
                If MySettings.LIMIT_ABORT Then
                    Throw New TwitterLimitException(Me)
                Else
                    If IsSavedPosts Then
                        If _ContentList.Count > 0 Then _DataNames.ListAddList(_ContentList.Select(Function(c) c.Post.ID), LAP.ClearBeforeAdd, LAP.NotContainsOnly)
                        DownloadData_SavedPosts(Token)
                    Else
                        LoadSavePostsKV(True)
                        If PostsKV.Count = 0 And (_ContentList.Count > 0 Or _TempPostsList.Count > 0) Then
                            Dim m As DownloadModels = IIf(IsMultiMode, DownloadModels.Media, DownloadModel)
                            PostsKV.ListAddList(_TempPostsList.Select(Function(p) New PKV(p, p, m)), LNC)
                            PostsKV.ListAddList(_ContentList.Select(Function(p) New PKV(p.Post.ID, p.Post.ID, m)), LNC)
                            _ForceSaveUserData = True
                        End If
                        LikesPosts.Clear()
                        If _ContentList.Count > 0 Then _DataNames.ListAddList(_ContentList.Select(Function(c) c.File.File), LAP.ClearBeforeAdd, LAP.NotContainsOnly)
                        DownloadData_Timeline(Token)
                        LoadSavePostsKV(False)
                        If LikesPosts.Count > 0 Then
                            _ReparseLikes = True
                            ReparseMissing(Token)
                            _ReparseLikes = False
                        End If
                    End If
                End If
            Finally
                _ReparseLikes = False
            End Try
        End Sub
        Private Sub DownloadData_Timeline(ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Dim tCache As CacheKeeper = Nothing
            Dim likesDetected As Boolean = False
            Try
                Const entry$ = "entry"
                Dim PostID$ = String.Empty
                Dim PostDate$, tmpUserId$
                Dim i%, nodeIndx%
                Dim dirIndx% = -1
                Dim nodes As List(Of String()) = GetContainerSubnodes()
                Dim node$()
                Dim timelineNode As Predicate(Of EContainer) = Function(ee) ee.Value("type").StringToLower = "timelineaddentries"
                Dim pinNode As Predicate(Of EContainer) = Function(ee) ee.Value("type").StringToLower = "timelinepinentry"
                Dim entriesNode As Predicate(Of EContainer) = Function(ee) ee.Name = "entries" Or ee.Name = entry
                Dim sourceIdPredicate As Predicate(Of EContainer) = Function(ee) ee.Name = "source_user_id_str" Or ee.Name = "source_user_id"
                Dim moduleItemsPredicate As Predicate(Of EContainer) = Function(ee) ee.Name.StringToLower = "moduleitems"
                Dim newTwitterNodes() As Object = {0, "content", "items"}
                Dim p As Predicate(Of EContainer)
                Dim pIndx%
                Dim indxChanged As Boolean = False
                Dim isOneNode As Boolean, isPins As Boolean, ExistsDetected As Boolean, userInfoParsed As Boolean = False
                Dim j As EContainer, rootNode As EContainer, optionalNode As EContainer, workingNode As EContainer, tmpNode As EContainer, nn As EContainer = Nothing
                Dim multiMode As Boolean = IsMultiMode
                Dim currentModel As DownloadModels = DownloadModels.Undefined

                Dim __parseContainer As Func(Of EContainer, Boolean) =
                    Function(ByVal ee As EContainer) As Boolean
                        nn = Nothing
                        If dirIndx > 1 Or IsCommunity Then nn = ee
                        If Not nn.ListExists Or IsCommunity Then
                            For Each node In nodes
                                nn = ee(node)
                                If nn.ListExists Then Exit For
                            Next
                        End If

                        If nn.ListExists Then
                            PostID = nn.Value("id_str").IfNullOrEmpty(nn.Value("id"))

                            'Date Pattern:
                            'Sat Jan 01 01:10:15 +0000 2000
                            If nn.Contains("created_at") Then PostDate = nn("created_at").Value Else PostDate = String.Empty
                            Select Case CheckDatesLimit(PostDate, Declarations.DateProvider)
                                Case DateResult.Skip, DateResult.Exit : Return False
                            End Select

                            If Not PostKVExists(PostID, currentModel, multiMode, False, True) Then
                            ElseIf dirIndx = 3 Then
                            ElseIf isPins Then
                                Return False
                            Else
                                ExistsDetected = Not multiMode
                                Return multiMode
                            End If

                            tmpUserId = nn({"retweeted_status_result", "result", "legacy", "user_id_str"}).XmlIfNothingValue

                            If tmpUserId.IsEmptyString Then tmpUserId = nn.ItemF({"extended_entities", "media", 0, sourceIdPredicate}).XmlIfNothingValue.
                                                                                IfNullOrEmpty(nn.Value("user_id")).IfNullOrEmpty(nn.Value("user_id_str")).IfNullOrEmpty("/")

                            If (Not ParseUserMediaOnly Or dirIndx = 3) OrElse
                               (dirIndx = 0 AndAlso MediaModelAllowNonUserTweets) OrElse
                               (Not ID.IsEmptyString AndAlso tmpUserId = ID) Then
                                If dirIndx = 1 And DownloadBroadcasts Then
                                    Dim m As UserMedia = ExtractBroadcast(nn, PostID, PostDate, nodes)
                                    If Not m.URL.IsEmptyString Then
                                        _TempMediaList.ListAddValue(m, LNC)
                                    Else
                                        m = ExtractBroadcast(ee, PostID, PostDate, nodes)
                                        If Not m.URL.IsEmptyString Then _TempMediaList.ListAddValue(m, LNC)
                                    End If
                                End If
                                If dirIndx = 3 Then
                                    Dim lUrl$ = nn.ItemF({"content", "itemContent", "tweet_results", "result", "legacy", "entities", "media", 0}, "expanded_url").XmlIfNothingValue
                                    If Not lUrl.IsEmptyString Then
                                        lUrl = RegexReplace(lUrl, StatusRegEx)
                                        If Not lUrl.IsEmptyString Then
                                            If PostKVExists(lUrl, currentModel, multiMode, False, True) Then Return multiMode
                                            LikesPosts.ListAddValue(lUrl, LNC)
                                        End If
                                    End If
                                Else
                                    ObtainMedia(nn, PostID, PostDate)
                                End If
                            End If
                        End If
                        Return True
                    End Function

                tCache = CreateCache()

                '0 - media
                '1 - profile
                '2 - search
                '3 - likes
                Dim dirs As List(Of SFile) = GetTimelineFromGalleryDL(tCache, Token)
                If dirs.ListExists Then
                    For Each dir As SFile In dirs
                        dirIndx += 1

                        Select Case dirIndx
                            Case 0 : currentModel = DownloadModels.Media
                            Case 1 : currentModel = DownloadModels.Profile
                            Case 2 : currentModel = DownloadModels.Search
                            Case 3 : currentModel = DownloadModels.Likes
                            Case Else : currentModel = DownloadModels.Undefined
                        End Select

                        If dirIndx = 3 Then likesDetected = True

                        ExistsDetected = False

                        If Not dir.IsEmptyString Then
                            ThrowAny(Token)
                            Dim timelineFiles As List(Of SFile) = SFile.GetFiles(dir, "*.txt",, EDP.ReturnValue)
                            If timelineFiles.ListExists Then
                                ResetFileNameProvider(Math.Max(timelineFiles.Count.ToString.Length, 2))
                                'rename files
                                For i = 0 To timelineFiles.Count - 1 : timelineFiles(i) = RenameGdlFile(timelineFiles(i), i) : Next
                                'parse files
                                For i = 0 To timelineFiles.Count - 1
                                    j = JsonDocument.Parse(timelineFiles(i).GetText)
                                    If Not j Is Nothing Then
                                        If i = 0 And Not indxChanged Then
                                            If Not userInfoParsed Then
                                                userInfoParsed = True
                                                Dim resValue$ = j.Value({"data", IIf(IsCommunity, "communityResults", "user"), "result"}, "__typename").StringTrim.StringToLower
                                                Dim icon$
                                                Dim fileCrFunc As Func(Of String, SFile) = Function(img) UrlFile(img, True)
                                                If resValue.IsEmptyString Then
                                                    UserExists = False
                                                    j.Dispose()
                                                    Exit Sub
                                                ElseIf resValue = "userunavailable" Then
                                                    UserSuspended = True
                                                    j.Dispose()
                                                    Exit Sub
                                                ElseIf IsCommunity Then
                                                    With j({"data", "communityResults", "result", "community_media_timeline", "timeline", "instructions"})
                                                        If .ListExists Then
                                                            With .Find(entriesNode, True)
                                                                If .ListExists Then
                                                                    With .ItemF({0, "content", "items", 0, "item", "itemContent", "tweet_results", "result", "tweet", "community_results", "result"})
                                                                        If .ListExists Then
                                                                            If ID = .Value("id_str") Then
                                                                                UserSiteNameUpdate(.Value("name"))
                                                                                UserDescriptionUpdate(.Value("description"))

                                                                                icon = .Value({"custom_banner_media", "media_info"}, "original_img_url").
                                                                                       IfNullOrEmpty(.Value({"default_banner_media", "media_info"}, "original_img_url"))
                                                                                If Not icon.IsEmptyString And DownloadIconBanner Then SimpleDownloadAvatar(icon, fileCrFunc)
                                                                            End If
                                                                        End If
                                                                    End With
                                                                End If
                                                            End With
                                                        End If
                                                    End With
                                                    i = -1
                                                    indxChanged = True
                                                Else
                                                    With j({"data", "user", "result"})
                                                        If .ListExists Then
                                                            If ID.IsEmptyString Then
                                                                ID = .Value("rest_id")
                                                                If Not ID.IsEmptyString Then _ForceSaveUserInfo = True
                                                            End If
                                                            With .Item({"legacy"})
                                                                If .ListExists Then
                                                                    If .Value("screen_name").StringToLower = NameTrue.ToLower Then
                                                                        UserSiteNameUpdate(.Value("name"))
                                                                        UserDescriptionUpdate(.Value("description"))

                                                                        icon = .Value("profile_image_url_https")
                                                                        If Not icon.IsEmptyString Then icon = icon.Replace("_normal", String.Empty)
                                                                        If DownloadIconBanner Then
                                                                            SimpleDownloadAvatar(.Value("profile_banner_url"), fileCrFunc)
                                                                            SimpleDownloadAvatar(icon, fileCrFunc)
                                                                        End If
                                                                    End If
                                                                End If
                                                            End With
                                                        End If
                                                    End With
                                                End If
                                            ElseIf IsCommunity Then
                                                i = -1
                                                indxChanged = True
                                            End If
                                        Else
                                            For pIndx = 0 To IIf(dirIndx < 2 Or dirIndx = 3, 1, 0)
                                                optionalNode = Nothing
                                                rootNode = Nothing
                                                If IsCommunity Then
                                                    With j({"data", "communityResults", "result", "community_media_timeline", "timeline", "instructions"})
                                                        If .ListExists Then
                                                            If i = 0 Then
                                                                rootNode = .Find(entriesNode, True)
                                                            Else
                                                                rootNode = .Find(moduleItemsPredicate, True)
                                                            End If
                                                            optionalNode = rootNode
                                                        End If
                                                    End With
                                                Else
                                                    Select Case dirIndx
                                                        Case 0, 1, 3
                                                            rootNode = j({"data", "user", "result", "timeline_v2", "timeline", "instructions"})
                                                            If rootNode.ListExists Then
                                                                If dirIndx = 3 Then
                                                                    p = entriesNode
                                                                    isPins = False
                                                                Else
                                                                    p = If(pIndx = 0, pinNode, timelineNode)
                                                                    isPins = pIndx = 0
                                                                End If
                                                                optionalNode = rootNode
                                                                rootNode = rootNode.Find(p, dirIndx = 3)
                                                                If dirIndx <> 3 And rootNode.ListExists Then rootNode = rootNode.Find(entriesNode, dirIndx = 3)
                                                            End If
                                                        Case Else
                                                            isPins = False
                                                            rootNode = j({"globalObjects", "tweets"})
                                                            optionalNode = rootNode
                                                    End Select
                                                End If

                                                If rootNode.ListExists Then
                                                    With rootNode
                                                        If IsCommunity Then
                                                            isOneNode = pIndx = 0
                                                        Else
                                                            isOneNode = dirIndx < 2 AndAlso .Name = entry
                                                        End If
                                                        ProgressPre.ChangeMax(If(isOneNode, 1, .Count))
                                                        If isOneNode Then
                                                            ProgressPre.Perform()
                                                            If Not __parseContainer(.Self) Then Continue For 'Exit For
                                                        Else
                                                            For nodeIndx = 0 To 1
                                                                If nodeIndx = 0 Then
                                                                    workingNode = rootNode
                                                                Else
                                                                    workingNode = optionalNode
                                                                    If workingNode.ListExists Then workingNode = workingNode.Find(moduleItemsPredicate, True)
                                                                End If
                                                                If workingNode.ListExists Then
                                                                    With workingNode
                                                                        For Each tmpNode In If(If(.ItemF(newTwitterNodes)?.Count, 0) > 0,
                                                                                                  .ItemF(newTwitterNodes),
                                                                                                  .Self)
                                                                            ProgressPre.Perform()
                                                                            If Not __parseContainer(tmpNode) Then
                                                                                If isPins Then GoTo nextpIndx
                                                                                Exit For
                                                                            End If
                                                                        Next
                                                                    End With
                                                                End If
nextNodeIndx:
                                                            Next
                                                        End If
                                                    End With
                                                End If
nextpIndx:
                                            Next

                                            If ExistsDetected And i = 1 Then Exit For Else ExistsDetected = False
                                        End If
                                        j.Dispose()
                                    End If
                                Next
                                timelineFiles.Clear()
                            End If
                        End If
                    Next
                    dirs.Clear()
                End If
                ThrowAny(Token)
                If Not FirstDownloadComplete Then
                    _ForceSaveUserInfo = True
                    If DownloadModel = DownloadModels.Undefined Then
                        If ParseUserMediaOnly Then
                            DownloadModel = DownloadModels.Media
                            If DownloadBroadcasts Then DownloadModel += DownloadModels.Profile
                        Else
                            DownloadModel = DownloadModels.Media + DownloadModels.Profile + DownloadModels.Search
                        End If
                    End If
                End If
                DownloadModelForceApply = False
                FirstDownloadComplete = True
            Catch jsonNull_ex As JsonDocumentException When jsonNull_ex.State = WebDocumentEventArgs.States.Error
                Throw New Plugin.ExitException("No deserialized data found")
            Catch limit_ex As TwitterLimitException
                Throw limit_ex
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]")
            Finally
                If Not tCache Is Nothing Then tCache.Dispose()
                If _TempPostsList.Count > 0 And Not likesDetected Then _TempPostsList.Sort()
            End Try
        End Sub
        Private Sub DownloadData_SavedPosts(ByVal Token As CancellationToken)
            Try
                Dim f As SFile = GetDataFromGalleryDL("https://x.com/i/bookmarks", Settings.Cache, True, Token)
                Dim files As List(Of SFile) = SFile.GetFiles(f, "*.txt")
                If files.ListExists Then
                    ResetFileNameProvider(Math.Max(files.Count.ToString.Length, 3))
                    Dim id$
                    Dim nodes As List(Of String()) = GetContainerSubnodes()
                    Dim node$()
                    Dim j As EContainer, jj As EContainer
                    Dim jErr As New ErrorsDescriber(EDP.ReturnValue)
                    For i% = 0 To files.Count - 1
                        f = RenameGdlFile(files(i), i)
                        j = JsonDocument.Parse(f.GetText, jErr)
                        If Not j Is Nothing Then
                            With j.ItemF({"data", 0, "timeline", "instructions", 0, "entries"})
                                If .ListExists Then
                                    ProgressPre.ChangeMax(.Count)
                                    For Each jj In .Self
                                        ProgressPre.Perform()
                                        For Each node In nodes
                                            With jj(node)
                                                If .ListExists Then
                                                    id = .Value("id_str")
                                                    If _TempPostsList.Contains(id) Then j.Dispose() : Exit Sub Else ObtainMedia(.Self, id, .Value("created_at"))
                                                    Exit For
                                                End If
                                            End With
                                        Next
                                    Next
                                End If
                            End With
                            j.Dispose()
                        End If
                    Next
                    nodes.Clear()
                End If
            Catch limit_ex As TwitterLimitException
            Catch ex As Exception
                ProcessException(ex, Token, "data downloading error (Saved Posts)")
            End Try
        End Sub
#End Region
#Region "Obtain media"
        Private Sub ObtainMedia(ByVal e As EContainer, ByVal PostID As String, ByVal PostDate As String, Optional ByVal State As UStates = UStates.Unknown,
                                Optional ByVal Attempts As Integer = 0, Optional ByVal SpecialFolder As String = Nothing)
            Dim s As EContainer = e({"extended_entities", "media"})
            If If(s?.Count, 0) = 0 Then s = e({"retweeted_status", "extended_entities", "media"})
            If If(s?.Count, 0) = 0 Then s = e({"retweeted_status_result", "result", "legacy", "extended_entities", "media"})

            If If(s?.Count, 0) > 0 Then
                Dim mUrl$
                Dim media As UserMedia
                For Each m As EContainer In s
                    If Not CheckVideoNode(m, PostID, PostDate, State, SpecialFolder) Then
                        mUrl = m.Value("media_url").IfNullOrEmpty(m.Value("media_url_https"))
                        If Not mUrl.IsEmptyString Then
                            Dim dName$ = UrlFile(mUrl)
                            If Not dName.IsEmptyString AndAlso Not _DataNames.Contains(dName) Then
                                _DataNames.Add(dName)
                                media = MediaFromData(mUrl, PostID, PostDate, GetPictureOption(m), State, UTypes.Picture, Attempts)
                                If Not SpecialFolder.IsEmptyString Then media.SpecialFolder = SpecialFolder
                                _TempMediaList.ListAddValue(media, LNC)
                            End If
                        End If
                    End If
                Next
            End If
        End Sub
        Private Function CheckVideoNode(ByVal w As EContainer, ByVal PostID As String, ByVal PostDate As String,
                                        Optional ByVal State As UStates = UStates.Unknown, Optional ByVal SpecialFolder As String = Nothing) As Boolean
            Try
                If CheckForGif(w, PostID, PostDate, State, SpecialFolder) Then Return True
                Dim URL$ = GetVideoNodeURL(w)
                If Not URL.IsEmptyString Then
                    Dim f$ = UrlFile(URL)
                    If Not f.IsEmptyString AndAlso Not _DataNames.Contains(f) Then
                        _DataNames.Add(f)
                        Dim m As UserMedia = MediaFromData(URL, PostID, PostDate,, State, UTypes.Video)
                        If Not SpecialFolder.IsEmptyString Then m.SpecialFolder = SpecialFolder
                        _TempMediaList.ListAddValue(m, LNC)
                    End If
                    Return True
                End If
                Return False
            Catch ex As Exception
                LogError(ex, "[API.Twitter.UserData.CheckVideoNode]")
                Return False
            End Try
        End Function
        Private Function CheckForGif(ByVal w As EContainer, ByVal PostID As String, ByVal PostDate As String,
                                     Optional ByVal State As UStates = UStates.Unknown, Optional ByVal SpecialFolder As String = Nothing) As Boolean
            Try
                Dim gifUrl As Predicate(Of EContainer) = Function(e) Not e.Value("content_type").IsEmptyString AndAlso
                                                                     e.Value("content_type").Contains("mp4") AndAlso
                                                                     Not e.Value("url").IsEmptyString
                Dim url$, ff$
                Dim f As SFile
                Dim m As UserMedia
                If w.ListExists Then
                    If w.Value("type") = "animated_gif" Then
                        With w({"video_info", "variants"})
                            If .ListExists Then
                                With .ItemF({gifUrl})
                                    If .ListExists Then
                                        url = .Value("url")
                                        ff = UrlFile(url)
                                        If Not ff.IsEmptyString Then
                                            If GifsDownload And Not _DataNames.Contains(ff) Then
                                                m = MediaFromData(url, PostID, PostDate,, State, UTypes.Video)
                                                If Not SpecialFolder.IsEmptyString Then m.SpecialFolder = SpecialFolder
                                                f = m.File
                                                If Not f.IsEmptyString And Not GifsPrefix.IsEmptyString Then f.Name = $"{GifsPrefix}{f.Name}" : m.File = f
                                                If Not GifsSpecialFolder.IsEmptyString Then
                                                    If Not m.SpecialFolder.IsEmptyString Then m.SpecialFolder &= "\"
                                                    m.SpecialFolder &= $"{GifsSpecialFolder}*"
                                                End If
                                                _TempMediaList.ListAddValue(m, LNC)
                                            End If
                                            Return True
                                        End If
                                    End If
                                End With
                            End If
                        End With
                    End If
                End If
                Return False
            Catch ex As Exception
                LogError(ex, "[API.Twitter.UserData.CheckForGif]")
                Return False
            End Try
        End Function
        Private Function GetVideoNodeURL(ByVal w As EContainer) As String
            With w({"video_info", "variants"})
                If .ListExists Then
                    Dim l As New List(Of Sizes)
                    Dim u$
                    For Each n As EContainer In .Self
                        If n.Count > 0 Then
                            If n("content_type").XmlIfNothingValue("none").Contains("mp4") AndAlso n.Contains("url") Then
                                u = n.Value("url")
                                l.Add(New Sizes(RegexReplace(u, VideoSizeRegEx), u))
                            End If
                        End If
                    Next
                    If l.Count > 0 Then l.RemoveAll(Function(s) s.HasError)
                    If l.Count > 0 Then l.Sort() : Return l(0).Data
                End If
            End With
            Return String.Empty
        End Function
#End Region
#Region "Gallery-DL Support"
        Private Class TwitterLimitException : Inherits Plugin.ExitException
            Friend Sub New(ByVal User As UserData)
                Silent = True
                User.MySettings.LimitSkippedUsers.Add(User)
            End Sub
        End Class
        Private Class TwitterGDL : Inherits GDL.GDLBatch
            Private ReadOnly KillOnLimit As Boolean
            Friend LimitReached As Boolean = False
            Friend Sub New(ByVal Dir As SFile, ByVal _Token As CancellationToken, ByVal _KillOnLimit As Boolean)
                MyBase.New(_Token)
                Commands.Clear()
                If Not Dir.IsEmptyString Then ChangeDirectory(Dir)
                KillOnLimit = _KillOnLimit
            End Sub
            Protected Overrides Async Function Validate(ByVal Value As String) As Task
                If Not ProcessKilled AndAlso Await Task.Run(Function() Token.IsCancellationRequested OrElse IdExists(Value)) Then Kill()
            End Function
            Private Function IdExists(ByVal Value As String) As Boolean
                Try
                    Value = Value.StringTrim
                    If Not Value.IsEmptyString AndAlso (Value.StartsWith("*") Or Value.StartsWith(".\gallery-dl\")) Then
                        Dim id$ = Value.Split("\").Last.Split(".").First.Split("_").First
                        If Not id.IsEmptyString Then Return TempPostsList.Contains(id)
                    End If
                Catch ex As Exception
                End Try
                Return False
            End Function
            Protected Overrides Async Sub ErrorDataReceiver(ByVal Sender As Object, ByVal e As DataReceivedEventArgs)
                Await Task.Run(Sub() CheckForLimit(e.Data))
            End Sub
            Private Sub CheckForLimit(ByVal Value As String)
                If Token.IsCancellationRequested Or (KillOnLimit AndAlso Not ProcessKilled AndAlso
                   Not Value.IsEmptyString AndAlso (Value.ToLower.Contains("for rate limit reset") OrElse
                                                    Not CStr(RegexReplace(Value, GdlLimitRegEx)).IsEmptyString)) Then
                    LimitReached = True
                    Kill()
                End If
            End Sub
        End Class
        Private ReadOnly Property SleepTimerValue(ByVal First As Boolean) As Integer
            Get
                Dim fTimer% = If(First, MySettings.SleepTimerBeforeFirst, MySettings.SleepTimer).Value
                If First And fTimer = SiteSettings.TimerFirstUseTheSame Then fTimer = MySettings.SleepTimer.Value
                Return fTimer
            End Get
        End Property
        Private ReadOnly Property SleepRequest As String
            Get
                Dim s% = SleepTimerValue(False)
                Return If(s = SiteSettings.TimerDisabled, String.Empty, $" --sleep-request {s}")
            End Get
        End Property
        Private Sub GdlWaitFirstTimer(ByVal fTimer As Integer)
            If GDL_REQUESTS_COUNT = 0 And Not fTimer = SiteSettings.TimerDisabled And MySettings.UserNumber > 0 Then Thread.Sleep(fTimer * 1000)
            GDL_REQUESTS_COUNT += 1
        End Sub
        Private _GdlPreProgressPerformEnabled As Boolean = False
        Private Sub GdlPreProgressPerform(ByVal Path As SFile, ByVal UpDir As SFile)
            Dim f As SFile = Nothing
            Try
                Dim c% = -1, lb% = -1, cc%
                Dim e As New ErrorsDescriber(EDP.ReturnValue)
                While _GdlPreProgressPerformEnabled
                    Thread.Sleep(100)
                    If c > 0 Then
                        cc = If(SFile.GetFiles(Path,,, e)?.Count, 0)
                        If cc > c Then
                            Exit Sub
                        ElseIf cc > 0 And (lb = -1 Or cc > lb) Then
                            ProgressPre.Perform(cc - IIf(lb = -1, 0, lb))
                            lb = cc
                        End If
                    ElseIf Path.Exists(SFO.Path, False) Then
                        Dim files As List(Of SFile) = SFile.GetFiles(Path,,, e)
                        If files.ListExists Then
                            f = files.FirstOrDefault(Function(ff) Not ff.Name.IsEmptyString AndAlso ff.Name.StartsWith("01_"))
                            If UpDir.Exists(SFO.Path, False) Then
                                Dim fNew As SFile = $"{UpDir.PathWithSeparator}00.txt"
                                If f.Copy(fNew,, True, SFODelete.DeletePermanently,, e) Then
                                    f = fNew
                                    Using j As EContainer = JsonDocument.Parse(f.GetText(e), e)
                                        If j.ListExists Then
                                            c = j(GdlPostCoutNumberNodes).XmlIfNothingValue.FromXML(Of Integer)(-1)
                                            If c > 0 Then c /= 30 : ProgressPre.ChangeMax(c) : Continue While
                                        End If
                                    End Using
                                End If
                            End If
                            Exit Sub
                        End If
                    End If
                End While
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"{ToStringForLog()}:{vbCr}Path: '{Path.Path}'{vbCr}Path 2: '{UpDir.Path}'")
            Finally
                If f.Exists Then f.Delete(SFO.File, SFODelete.DeletePermanently, EDP.ReturnValue)
            End Try
        End Sub
        Private Sub GdlPreProgressPerformWait(ByVal t As Task)
            _GdlPreProgressPerformEnabled = False
            Try
                While t.Status = TaskStatus.Running Or t.Status = TaskStatus.WaitingToRun : Thread.Sleep(100) : End While
            Catch
            End Try
        End Sub
        Private Function GetDataFromGalleryDL(ByVal URL As String, ByVal Cache As CacheKeeper, ByVal UseTempPostList As Boolean,
                                              Optional ByVal Token As CancellationToken = Nothing) As SFile
            Dim command$ = String.Empty
            Try
                Dim conf As SFile = GdlCreateConf(Cache.NewPath)
                Dim fTimer% = SleepTimerValue(True)
                command = $"""{Settings.GalleryDLFile}"" --verbose --no-download --no-skip{SleepRequest} --config ""{conf}"" --write-pages "
                'command &= GdlGetIdFilterString()
                Dim dir As SFile = Cache.NewPath
                If dir.Exists(SFO.Path,, EDP.ThrowException) Then
                    Using batch As New TwitterGDL(dir, Token, MySettings.AbortOnLimit.Value)
                        If UseTempPostList Then
                            batch.TempPostsList = _TempPostsList
                            command &= GdlGetIdFilterString()
                        End If
                        command &= URL
                        '#If DEBUG Then
                        'Debug.WriteLine(command)
                        '#End If
                        GdlWaitFirstTimer(fTimer)
                        batch.Execute(command)
                        If batch.LimitReached Then
                            If CBool(MySettings.DownloadAlreadyParsed.Value) And
                               SFile.GetFiles(dir, "*.txt", IO.SearchOption.AllDirectories, EDP.ReturnValue).Count > 0 Then
                                MySettings.LIMIT_ABORT = True
                                Return dir
                            Else
                                Throw New TwitterLimitException(Me)
                            End If
                        End If
                    End Using
                    Return dir
                End If
                Return Nothing
            Catch limit_ex As TwitterLimitException
                MySettings.LIMIT_ABORT = True
                Throw limit_ex
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, $"{ToStringForLog()}: GetDataFromGalleryDL({command})")
            End Try
        End Function
        Private Function GetTimelineFromGalleryDL(ByVal Cache As CacheKeeper, ByVal Token As CancellationToken) As List(Of SFile)
            Dim command$ = String.Empty
            Try
                Dim confCache As CacheKeeper = Cache.NewInstance(Of BatchFileExchanger)
                Dim conf As SFile = GdlCreateConf(confCache.RootDirectory)

                If DownloadModel = DownloadModels.Undefined And Not FirstDownloadComplete And DownloadModelForceApply Then
                    If ParseUserMediaOnly And DownloadBroadcasts Then
                        DownloadModel = DownloadModels.Media + DownloadModels.Profile
                    ElseIf ParseUserMediaOnly Then
                        DownloadModel = DownloadModels.Media
                    Else
                        DownloadModel = DownloadModels.Media + DownloadModels.Profile + DownloadModels.Search
                    End If
                End If

                Dim outList As New List(Of SFile)
                Dim rootDir As CacheKeeper = Cache.NewInstance
                Dim dir As SFile
                Dim dm As List(Of DownloadModels) = EnumExtract(Of DownloadModels)(DownloadModel).ListIfNothing
                Dim process As Boolean, multiMode As Boolean
                Dim currentModel As DownloadModels
                Dim urlPrePattern$ = $"https://x.com{IIf(IsCommunity, SiteSettings.CommunitiesUser, String.Empty)}/"
                Dim fTimer% = SleepTimerValue(True)
                Dim t As Task

                If DownloadBroadcasts AndAlso Not dm.Contains(DownloadModels.Profile) Then dm.Add(DownloadModels.Profile)

                multiMode = dm.Count > 1

                Using tgdl As New TwitterGDL(Nothing, Token, MySettings.AbortOnLimit.Value) With {
                    .AutoClear = True,
                    .AutoReset = True,
                    .CommandPermanent = $"chcp {BatchExecutor.UnicodeEncoding}",
                    .FileExchanger = confCache
                }
                    tgdl.FileExchanger.DeleteCacheOnDispose = False
                    tgdl.FileExchanger.DeleteRootOnDispose = False
                    For i As Byte = 0 To IIf(IsCommunity, 0, 3)
                        dir = rootDir.NewPath
                        dir.Exists(SFO.Path, True, EDP.ThrowException)
                        outList.Add(dir)
                        tgdl.ChangeDirectory(dir)
                        command = $"""{Settings.GalleryDLFile}"" --verbose --no-download --no-skip{SleepRequest} --config ""{conf}"" --write-pages "
                        If multiMode Then
                            command &= "{0}"
                        Else
                            command &= GdlGetIdFilterString()
                        End If
                        Select Case i
                            Case 0 : command &= $"{urlPrePattern}{NameTrue}/media" : currentModel = DownloadModels.Media : process = dm.Contains(currentModel) Or IsCommunity
                            Case 1 : command &= $"{urlPrePattern}{NameTrue}" : currentModel = DownloadModels.Profile : process = dm.Contains(currentModel)
                            Case 2 : command &= $"-o search-endpoint=graphql https://x.com/search?q=from:{NameTrue}+include:nativeretweets" : currentModel = DownloadModels.Search : process = dm.Contains(currentModel) And Not IsCommunity
                            Case 3 : command &= $"{urlPrePattern}{NameTrue}/likes" : currentModel = DownloadModels.Likes : process = dm.Contains(currentModel)
                            Case Else : process = False
                        End Select
                        '#If DEBUG Then
                        'Debug.WriteLine(command)
                        '#End If
                        ThrowAny(Token)
                        If process Then
                            If multiMode Then
                                If PostsKV.Count = 0 Then
                                    tgdl.TempPostsList = New List(Of String)
                                Else
                                    tgdl.TempPostsList = (From p As PKV In PostsKV Where p.Section = currentModel Select p.ID).ListIfNothing
                                End If
                                command = String.Format(command, GdlGetIdFilterString(tgdl.TempPostsList))
                            Else
                                tgdl.TempPostsList = _TempPostsList
                            End If

                            GdlWaitFirstTimer(fTimer)

                            _GdlPreProgressPerformEnabled = True
                            t = New Task(Sub() GdlPreProgressPerform(dir, conf))
                            t.Start()

                            tgdl.Execute(command)

                            _GdlPreProgressPerformEnabled = False
                            GdlPreProgressPerformWait(t)

                            If tgdl.LimitReached Then
                                If CBool(MySettings.DownloadAlreadyParsed.Value) And
                                   SFile.GetFiles(rootDir, "*.txt", IO.SearchOption.AllDirectories, EDP.ReturnValue).Count > 0 Then
                                    MySettings.LIMIT_ABORT = True
                                    Exit For
                                Else
                                    Throw New TwitterLimitException(Me)
                                End If
                            End If
                        End If
                        ThrowAny(Token)
                    Next
                End Using
                dm.Clear()

                Return outList
            Catch limit_ex As TwitterLimitException
                MySettings.LIMIT_ABORT = True
                Throw limit_ex
            Catch ex As Exception
                ProcessException(ex, Token, $"{ToStringForLog()}: GetTimelineFromGalleryDL({command})")
                Return Nothing
            Finally
                _GdlPreProgressPerformEnabled = False
            End Try
        End Function
        Private Function GdlGetIdFilterString(Optional ByVal TL As List(Of String) = Nothing) As String
            If TL.ListExists Then TL.Sort()
            With If(TL, _TempPostsList) : Return If(.Count > 0, $"--filter ""int(tweet_id) > { .Last} or abort()"" ", String.Empty) : End With
        End Function
        Private Function GdlCreateConf(ByVal Path As SFile) As SFile
            Try
                Dim conf As SFile = $"{Path.PathWithSeparator}TwitterGdlConfig.conf"
                Dim __userAgent$ = MySettings.UserAgent
                If Not __userAgent.IsEmptyString Then __userAgent = $"""user-agent"": ""{__userAgent}"","
                Dim confText$ = "{""extractor"":{""cookies"": """ & MySettings.CookiesNetscapeFile.ToString.Replace("\", "/") &
                                 $""",""cookies-update"": {IIf(CBool(MySettings.CookiesUpdate.Value), "true", "false")}," & __userAgent &
                                 """twitter"":{""tweet-endpoint"": ""detail"",""cards"": false,""conversations"": true,""pinned"": false,""quoted"": false,""replies"": true,""retweets"": true,""strategy"": null,""text-tweets"": false,""twitpic"": false,""unique"": true,""users"": ""timeline"",""videos"": true}}}"
                If conf.Exists(SFO.Path, True, EDP.ThrowException) Then TextSaver.SaveTextToFile(confText, conf)
                If Not conf.Exists Then Throw New IO.FileNotFoundException("Can't find Twitter GDL config file", conf)
                Return conf
            Catch file_ex As IO.FileNotFoundException
                Throw file_ex
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "gallery-dl configuration file creating error", New SFile)
            End Try
        End Function
#End Region
#Region "ReparseMissing"
        Private _ReparseLikes As Boolean = False
        Protected Overrides Sub ReparseMissing(ByVal Token As CancellationToken)
            Dim rList As New List(Of Integer)
            Dim URL$ = String.Empty
            Dim cache As CacheKeeper = Nothing
            Try
                If ContentMissingExists Or (_ReparseLikes And LikesPosts.Count > 0) Then
                    Dim m As UserMedia, mTmp As UserMedia
                    Dim PostDate$
                    Dim nodes As List(Of String()) = GetContainerSubnodes()
                    Dim node$()
                    Dim j As EContainer, n As EContainer
                    Dim f As SFile
                    Dim i%, ii%
                    Dim files As List(Of SFile)
                    Dim lim%
                    Dim specFolder$ = IIf(_ReparseLikes, "Likes", String.Empty)
                    ResetFileNameProvider()
                    cache = If(IsSingleObjectDownload, Settings.Cache, CreateCache())
                    If _ReparseLikes Then lim = LikesPosts.Count Else lim = _ContentList.Count
                    ProgressPre.ChangeMax(lim)
                    For i = 0 To lim - 1
                        ProgressPre.Perform()
                        If _ReparseLikes OrElse _ContentList(i).State = UStates.Missing Then
                            m = If(_ReparseLikes, Nothing, _ContentList(i))
                            If Not m.Post.ID.IsEmptyString Or (IsSingleObjectDownload And Not m.URL_BASE.IsEmptyString) Or _ReparseLikes Then
                                ThrowAny(Token)
                                If m.Type = UTypes.m3u8 Then
                                    _TempMediaList.Add(m)
                                    rList.ListAddValue(i, LNC)
                                    Continue For
                                ElseIf IsSingleObjectDownload Then
                                    URL = m.URL_BASE
                                ElseIf _ReparseLikes Then
                                    URL = LikesPosts(i)
                                Else
                                    URL = String.Format(SiteSettings.SinglePostPattern, m.Post.ID)
                                End If
                                f = GetDataFromGalleryDL(URL, cache, False, Token)
                                If Not f.IsEmptyString Then
                                    files = SFile.GetFiles(f, "*.txt")
                                    If files.ListExists Then
                                        For ii = 0 To files.Count - 1
                                            f = RenameGdlFile(files(ii), ii)
                                            j = JsonDocument.Parse(f.GetText)
                                            If Not j Is Nothing Then
                                                With j.ItemF({"data", 0, "instructions", 0, "entries"})
                                                    If .ListExists Then
                                                        If IsSingleObjectDownload Or DownloadBroadcasts Then
                                                            mTmp = ExtractBroadcast(.Self, m.Post.ID, String.Empty, nodes)
                                                            If Not mTmp.URL.IsEmptyString Then
                                                                _TempMediaList.ListAddValue(mTmp, LNC)
                                                                rList.ListAddValue(i, LNC)
                                                            End If
                                                        End If
                                                        For Each n In .Self
                                                            For Each node In nodes
                                                                With n(node)
                                                                    If .ListExists Then
                                                                        PostDate = String.Empty
                                                                        If .Contains("created_at") Then PostDate = .Value("created_at") Else PostDate = String.Empty
                                                                        ObtainMedia(.Self, m.Post.ID, PostDate, UStates.Missing, m.Attempts, specFolder)
                                                                        rList.ListAddValue(i, LNC)
                                                                    End If
                                                                End With
                                                            Next
                                                        Next
                                                    End If
                                                End With
                                                j.Dispose()
                                            End If
                                        Next
                                        files.Clear()
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex, Token, $"ReparseMissing error [{URL}]")
            Finally
                If Not cache Is Nothing And Not IsSingleObjectDownload Then cache.Dispose()
                If rList.Count > 0 And Not _ReparseLikes Then
                    For i% = rList.Count - 1 To 0 Step -1 : _ContentList.RemoveAt(rList(i)) : Next
                    rList.Clear()
                End If
            End Try
        End Sub
#End Region
#Region "DownloadSingleObject"
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            GDL_REQUESTS_COUNT = 0
            Dim um As New UserMedia(Data.URL) With {.State = UStates.Missing}
            If Not Data.URL.IsEmptyString AndAlso Data.URL.Contains(BroadCastPartUrl) Then um.Type = UTypes.m3u8
            _ContentList.Add(um)
            ReparseMissing(Token)
        End Sub
#End Region
#Region "Picture options"
        Private Function GetPictureOption(ByVal w As EContainer) As String
            Const P4K As String = "4096x4096"
            Try
                Dim ww As EContainer = w("sizes")
                If ww.ListExists Then
                    Dim l As New List(Of Sizes)
                    Dim Orig As Sizes? = New Sizes(w.Value({"original_info"}, "height").FromXML(Of Integer)(-1), P4K)
                    If Orig.Value.Value = -1 Then Orig = Nothing
                    Dim LargeContained As Boolean = ww.Contains("large")
                    For Each v As EContainer In ww
                        If v.Count > 0 AndAlso v.Contains("h") Then l.Add(New Sizes(v.Value("h"), v.Name))
                    Next
                    If l.Count > 0 Then
                        l.Sort()
                        If Orig.HasValue AndAlso l(0).Value < Orig.Value.Value Then
                            Return P4K
                        ElseIf l(0).Data.IsEmptyString Then
                            Return P4K
                        Else
                            Return l(0).Data
                        End If
                    Else
                        Return P4K
                    End If
                ElseIf Not w.Value({"original_info"}, "height").IsEmptyString Then
                    Return P4K
                Else
                    Return String.Empty
                End If
            Catch ex As Exception
                LogError(ex, "[API.Twitter.UserData.GetPictureOption]")
                Return String.Empty
            End Try
        End Function
#End Region
#Region "UrlFile"
        Private Function UrlFile(ByVal URL As String, Optional ByVal GetWithoutExtension As Boolean = False) As String
            Try
                If Not URL.IsEmptyString Then
                    Dim f As SFile = CStr(RegexReplace(LinkFormatterSecure(RegexReplace(URL.Replace("\", String.Empty), LinkPattern)), FilesPattern))
                    If f.IsEmptyString And GetWithoutExtension Then
                        URL = LinkFormatterSecure(RegexReplace(URL.Replace("\", String.Empty), LinkPattern))
                        If Not URL.IsEmptyString Then f = New SFile With {.Name = URL.Split("/").LastOrDefault}
                    End If
                    If Not f.IsEmptyString Then Return f.File
                End If
                Return String.Empty
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function
#End Region
#Region "Clear"
        Protected Overrides Sub EraseData_AdditionalDataFiles()
            MyFilePostsKV.Delete(SFO.File, SFODelete.DeleteToRecycleBin, EDP.SendToLog + EDP.ReturnValue)
            _DataNames.Clear()
            MyBase.EraseData_AdditionalDataFiles()
        End Sub
#End Region
#Region "Create media"
        Private Function MediaFromData(ByVal _URL As String, ByVal PostID As String, ByVal PostDate As String,
                                       Optional ByVal _PictureOption As String = Nothing,
                                       Optional ByVal State As UStates = UStates.Unknown,
                                       Optional ByVal Type As UTypes = UTypes.Undefined,
                                       Optional ByVal Attempts As Integer = 0) As UserMedia
            _URL = LinkFormatterSecure(RegexReplace(_URL.Replace("\", String.Empty), LinkPattern))
            Dim m As New UserMedia(_URL) With {.PictureOption = _PictureOption, .Post = New UserPost With {.ID = PostID}, .Type = Type}
            If Not m.URL.IsEmptyString Then m.File = CStr(RegexReplace(m.URL, FilesPattern))
            If Not m.PictureOption.IsEmptyString And Not m.File.IsEmptyString And Not m.URL.IsEmptyString Then
                m.URL = $"{m.URL.Replace($".{m.File.Extension}", String.Empty)}?format={m.File.Extension}&name={m.PictureOption}"
            End If
            If Not PostDate.IsEmptyString Then m.Post.Date = AConvert(Of Date)(PostDate, Declarations.DateProvider, Nothing) Else m.Post.Date = Nothing
            m.State = State
            m.Attempts = Attempts
            Return m
        End Function
#End Region
#Region "Downloader"
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
        Protected Overrides Function DownloadM3U8(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile, ByVal Token As CancellationToken) As SFile
            Const ytDest$ = "[download] destination"
            Dim f As SFile = Nothing
            If MySettings.CookiesNetscapeFile.Exists And Settings.YtdlpFile.Exists And (Not URL.IsEmptyString AndAlso URL.Contains(BroadCastPartUrl)) Then
                Dim destPath$ = DestinationFile.PathWithSeparator.Replace("\", "\\")
                Dim rr As RParams = RParams.DM($"{destPath}.+mp4", 0, RegexOptions.IgnoreCase, EDP.ReturnValue)
                Dim cmd$ = $"""{Settings.YtdlpFile.File}"" --no-cookies-from-browser --cookies ""{MySettings.CookiesNetscapeFile}"" "
                cmd &= $"{URL} -P ""{destPath}"" --no-mtime"
                Using ytdlp As New YTDLP.YTDLPBatch(Token)
                    With ytdlp
                        .Execute(cmd)
                        If .OutputData.Count > 0 Then
                            For Each outStr$ In .OutputData
                                If Not outStr.IsEmptyString AndAlso outStr.ToLower.Trim.StartsWith(ytDest) Then
                                    f = CStr(RegexReplace(outStr, rr))
                                    If Not f.Exists Then f = Nothing
                                    Exit For
                                End If
                            Next
                        End If
                    End With
                End Using
            End If
            Return f
        End Function
#End Region
#Region "Exception"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            Return 0
        End Function
#End Region
#Region "IDisposable support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then _DataNames.Clear() : LikesPosts.Clear() : PostsKV.Clear()
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace