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
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports IG = SCrawler.API.Instagram.SiteSettings
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Imports UStates = SCrawler.API.Base.UserMedia.States
Namespace API.Facebook
    Friend Class UserData : Inherits Instagram.UserData
#Region "XML names"
        Private Const Name_IsNoNameProfile As String = "IsNoNameProfile"
        Private Const Name_OptionsParsed As String = "OptionsParsed"
        Private Const Name_VideoPageID As String = "VideoPageID"
        Private Const Name_StoryBucket As String = "StoryBucket"
        Private Const Name_ParsePhotoBlock As String = "ParsePhotoBlock"
        Private Const Name_ParseVideoBlock As String = "ParseVideoBlock"
        Private Const Name_ParseStoriesBlock As String = "ParseStoriesBlock"
#End Region
#Region "Declarations"
        Friend ReadOnly Property MySettings As SiteSettings
            Get
                Return HOST.Source
            End Get
        End Property
        Private IsNoNameProfile As Boolean = False
        Private OptionsParsed As Boolean = False
        Private Property VideoPageID As String = String.Empty
        Private Property StoryBucket As String = String.Empty
        Friend Property ParsePhotoBlock As Boolean = True
        Friend Property ParseVideoBlock As Boolean = True
        Friend Property ParseStoriesBlock As Boolean = True
        Private Enum PageBlock As Integer
            Timeline = Sections.Timeline
            Stories = Sections.Stories
            Photos = 100
            Videos = 101
            Undefined = -1
        End Enum
#End Region
#Region "GetProfileUrl"
        Friend Function GetProfileUrl() As String
            If IsNoNameProfile Then
                Return $"https://www.facebook.com/profile.php?id={ID}"
            Else
                Return $"https://www.facebook.com/{NameTrue}"
            End If
        End Function
#End Region
#Region "Exchange"
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return New UserExchangeOptions(Me)
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            If Not Obj Is Nothing AndAlso TypeOf Obj Is UserExchangeOptions Then
                With DirectCast(Obj, UserExchangeOptions)
                    ParsePhotoBlock = .ParsePhotoBlock
                    ParseVideoBlock = .ParseVideoBlock
                    ParseStoriesBlock = .ParseStoriesBlock
                End With
            End If
        End Sub
#End Region
#Region "Loader"
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            Dim updateNames As Action = Sub()
                                            If Not OptionsParsed AndAlso Not Options.IsEmptyString Then
                                                OptionsParsed = True
                                                Dim v$ = RegexReplace(Options, Regex_ProfileUrlID)
                                                If Not v.IsEmptyString Then ID = v : IsNoNameProfile = True
                                            End If
                                        End Sub
            With Container
                If Loading Then
                    If .Contains(Name_IsNoNameProfile) Then
                        IsNoNameProfile = .Value(Name_IsNoNameProfile).FromXML(Of Boolean)(False)
                    Else
                        updateNames.Invoke
                    End If
                    OptionsParsed = .Value(Name_OptionsParsed).FromXML(Of Boolean)(False)
                    VideoPageID = .Value(Name_VideoPageID)
                    StoryBucket = .Value(Name_StoryBucket)
                    ParsePhotoBlock = .Value(Name_ParsePhotoBlock).FromXML(Of Boolean)(True)
                    ParseVideoBlock = .Value(Name_ParseVideoBlock).FromXML(Of Boolean)(True)
                    ParseStoriesBlock = .Value(Name_ParseStoriesBlock).FromXML(Of Boolean)(True)
                Else
                    updateNames.Invoke
                    .Add(Name_IsNoNameProfile, IsNoNameProfile.BoolToInteger)
                    .Add(Name_OptionsParsed, OptionsParsed.BoolToInteger)
                    .Add(Name_VideoPageID, VideoPageID)
                    .Add(Name_StoryBucket, StoryBucket)
                    .Add(Name_ParsePhotoBlock, ParsePhotoBlock.BoolToInteger)
                    .Add(Name_ParseVideoBlock, ParseVideoBlock.BoolToInteger)
                    .Add(Name_ParseStoriesBlock, ParseStoriesBlock.BoolToInteger)
                End If
            End With
        End Sub
#End Region
#Region "Initializer"
        Friend Sub New()
            _ResponserAutoUpdateCookies = True
        End Sub
#End Region
#Region "Download functions"
        Private Class TokensException : Inherits Plugin.ExitException
            Friend ReadOnly Property BasicTokens As Boolean
            Public Sub New(ByVal Message As String, ByVal _BasicTokens As Boolean)
                MyBase.New(Message)
                BasicTokens = _BasicTokens
            End Sub
            Friend Shared Sub SendToLog(ByVal Source As UserData, ByVal ex As TokensException, ByVal f As String)
                ErrorsDescriber.Execute(EDP.SendToLog, New ErrorsDescriberException($"{Source.ToStringForLog()} ({f}): {ex.Message}",,, ex) With {
                                        .SendToLogOnlyMessage = True, .ReplaceMainMessage = True})
            End Sub
        End Class
        Private Token_Photosby As String = String.Empty
        Private Limit As Integer = -1
        Private Sub WaitTimer()
            If CInt(MySettings.RequestsWaitTimer_Any.Value) > 0 Then Thread.Sleep(CInt(MySettings.RequestsWaitTimer_Any.Value))
        End Sub
        Private Sub DisableDownload()
            MySettings.DownloadData_Impl.Value = False
            MyMainLOG = $"{Site} downloading is disabled until you update your credentials"
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            If CBool(MySettings.DownloadData_Impl.Value) Then
                Try
                    If Responser.Headers.Value(IG.Header_IG_APP_ID).IsEmptyString Then Responser.Headers.Remove(IG.Header_IG_APP_ID)
                    ResetBaseTokens()
                    GetUserTokens(Token)
                    LoadSavePostsKV(True)
                    Limit = If(DownloadTopCount, -1)
                    If IsSavedPosts Then
                        DownloadData_SavedPosts(String.Empty, Token)
                    Else
                        If DownloadImages And ParsePhotoBlock Then DownloadData_Photo(String.Empty, Token)
                        If DownloadVideos And ParseVideoBlock Then DownloadData_Video(String.Empty, Token)
                        If (DownloadImages Or DownloadVideos) And ParseStoriesBlock Then DownloadData_Stories(Token)
                    End If
                    LoadSavePostsKV(False)
                Finally
                    MySettings.UpdateResponserData(Responser)
                End Try
            End If
        End Sub
        Private Const Header_fb_fr_name_Photo As String = "ProfileCometAppCollectionPhotosRendererPaginationQuery"
        Private Const Header_fb_fr_name_Video As String = "PagesCometChannelTabAllVideosCardImplPaginationQuery"
        Private Const Header_fb_fr_name_Stories As String = "StoriesSuspenseContentPaneRootWithEntryPointQuery"
        Private Const Header_fb_fr_name_SavedPosts As String = "CometSaveDashboardAllItemsPaginationQuery"
        Private Const DocID_Photo As String = "6684543058255697"
        Private Const DocID_Video As String = "24545934291687581"
        Private Const DocID_Stories As String = "6771064226315961"
        Private Const DocID_SavedPosts As String = "7112228098805003"
        Private Const Graphql_UrlPattern As String = "https://www.facebook.com/api/graphql?lsd={0}&doc_id={1}&server_timestamps=true&fb_dtsg={3}&fb_api_req_friendly_name={2}&variables={4}"
        Private Const VideoHtmlUrlPattern As String = "https://www.facebook.com/watch/?v={0}"
        Private Sub DownloadData_Photo(ByVal Cursor As String, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Const VarPattern$ = """count"":8,""cursor"":""{0}"",""scale"":1,""id"":""{1}"""
            Try
                Dim nextCursor$ = String.Empty
                Dim newPostsDetected As Boolean = False
                Dim pUrl$, pUrlBase$
                Dim pid As PostKV

                ValidateBaseTokens()
                If Token_Photosby.IsEmptyString Then Throw New TokensException("Unable to obtain token 'Token_Photosby'", False)

                URL = String.Format(Graphql_UrlPattern, Token_lsd, DocID_Photo, Header_fb_fr_name_Photo, Token_dtsg_Var,
                                    SymbolsConverter.ASCII.EncodeSymbolsOnly("{" & String.Format(VarPattern, Cursor, Token_Photosby) & "}"))

                ResponserApplyDefs(Header_fb_fr_name_Photo)
                ThrowAny(Token)

                WaitTimer()
                Dim r$ = Responser.GetResponse(URL)
                If Not r.IsEmptyString Then
                    Using j As EContainer = JsonDocument.Parse(r)
                        If j.ListExists Then
                            With j({"data", "node", "pageItems", "edges"})
                                If .ListExists Then
                                    ProgressPre.ChangeMax(.Count)
                                    For Each jNode As EContainer In .Self
                                        ProgressPre.Perform()
                                        With jNode
                                            If Not .Value("cursor").IsEmptyString Then nextCursor = .Value("cursor")
                                            With .Item({"node"})
                                                If .ListExists Then
                                                    pUrl = .Value({"node", "viewer_image"}, "uri")
                                                    pUrlBase = .Value("url")
                                                    If Not pUrl.IsEmptyString Then
                                                        pid = New PostKV(.Value("id"), .Value({"node"}, "id"), PageBlock.Photos)
                                                        If Not PostKvExists(pid) Then
                                                            newPostsDetected = True
                                                            PostsKVIDs.ListAddValue(pid, LNC)
                                                            _TempPostsList.Add(pid.ID)
                                                            _TempMediaList.ListAddValue(New UserMedia(pUrl, UTypes.Picture) With {
                                                                                        .URL_BASE = pUrlBase,
                                                                                        .File = CreateFileFromUrl(pUrl),
                                                                                        .Post = pid.ID.IfNullOrEmpty(pid.Code)}, LNC)
                                                            If Limit > 0 And _TempMediaList.Count >= Limit Then Exit Sub
                                                        Else
                                                            Exit Sub
                                                        End If
                                                    End If
                                                End If
                                            End With
                                        End With
                                    Next
                                End If
                            End With
                        End If
                    End Using
                End If

                If newPostsDetected And Not nextCursor.IsEmptyString Then DownloadData_Photo(nextCursor, Token)
            Catch tex As TokensException When Not tex.BasicTokens
                TokensException.SendToLog(Me, tex, "data (photo)")
            Catch ex As Exception
                ProcessException(ex, Token, $"data (photo) downloading error [{URL}]",, Responser)
            End Try
        End Sub
        Private Sub DownloadData_Video(ByVal Cursor As String, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Const VarPattern$ = """alwaysIncludeAudioRooms"":true,""count"":6,""cursor"":{0},""pageID"":""{1}"",""scale"":4,""showReactions"":true,""useDefaultActor"":false,""id"":""{1}"""
            Try
                Dim nextCursor$ = String.Empty
                Dim newPostsDetected As Boolean = False
                Dim pid As PostKV

                If VideoPageID.IsEmptyString Then GetVideoPageID(Token)
                If VideoPageID.IsEmptyString Then Throw New TokensException("Unable to obtain 'VideoPageID'", False)
                ValidateBaseTokens()

                URL = String.Format(Graphql_UrlPattern, Token_lsd, DocID_Video, Header_fb_fr_name_Video, Token_dtsg_Var,
                                    SymbolsConverter.ASCII.EncodeSymbolsOnly("{" & String.Format(VarPattern, If(Cursor.IsEmptyString, "null", $"""{Cursor}"""), VideoPageID) & "}"))

                ResponserApplyDefs(Header_fb_fr_name_Video)
                ThrowAny(Token)

                WaitTimer()
                Dim r$ = Responser.GetResponse(URL)
                If Not r.IsEmptyString Then
                    Using j As EContainer = JsonDocument.Parse(r)
                        If j.ListExists Then
                            With j({"data", "node", "all_videos", "edges"})
                                If .ListExists Then
                                    ProgressPre.ChangeMax(.Count)
                                    For Each jNode As EContainer In .Self
                                        ProgressPre.Perform()
                                        pid = New PostKV(String.Empty, jNode.Value({"node"}, "id"), PageBlock.Videos)
                                        pid.Code = $"Stories:{pid.ID}"
                                        nextCursor = jNode.Value("cursor")
                                        If Not PostKvExists(pid) Then
                                            newPostsDetected = True
                                            PostsKVIDs.ListAddValue(pid, LNC)
                                            _TempPostsList.Add(pid.Code)
                                            _TempMediaList.ListAddValue(New UserMedia(String.Format(VideoHtmlUrlPattern, pid.ID),
                                                                                      UTypes.VideoPre) With {.Post = pid.ID}, LNC)
                                            If Limit > 0 And _TempMediaList.Count >= Limit Then Exit Sub
                                        Else
                                            Exit Sub
                                        End If
                                    Next
                                End If
                            End With
                        End If
                    End Using
                End If

                If newPostsDetected And Not nextCursor.IsEmptyString Then DownloadData_Video(nextCursor, Token)
            Catch tex As TokensException When Not tex.BasicTokens
                TokensException.SendToLog(Me, tex, "data (video)")
            Catch ex As Exception
                ProcessException(ex, Token, $"data (video) downloading error [{URL}]",, Responser)
            End Try
        End Sub
        Private Sub DownloadData_Stories(ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Const VarPattern$ = """UFI2CommentsProvider_commentsKey"":""StoriesSuspenseContentPaneRootWithEntryPointQuery"",""blur"":10,""bucketID"":""{0}"",""displayCommentsContextEnableComment"":true,""displayCommentsContextIsAdPreview"":false,""displayCommentsContextIsAggregatedShare"":false,""displayCommentsContextIsStorySet"":false,""displayCommentsFeedbackContext"":null,""feedbackSource"":65,""feedLocation"":""COMET_MEDIA_VIEWER"",""focusCommentID"":null,""initialBucketID"":""{0}"",""initialLoad"":true,""isInitialLoadFromCommentsNotification"":false,""isStoriesArchive"":false,""isStoryCommentingEnabled"":false,""scale"":1,""shouldDeferLoad"":false,""shouldEnableArmadilloStoryReply"":false,""shouldEnableLiveInStories"":true,""__relay_internal__pv__StoriesIsCommentEnabledrelayprovider"":false,""__relay_internal__pv__StoriesIsContextualReplyDisabledrelayprovider"":false,""__relay_internal__pv__StoriesIsShareToStoryEnabledrelayprovider"":false,""__relay_internal__pv__StoriesRingrelayprovider"":false,""__relay_internal__pv__StoriesLWRVariantrelayprovider"":""www_new_reactions"""
            Try
                Dim pUrl$, pUrlBase$
                Dim pid As PostKV
                Dim t As UTypes
                Dim postDate As Date?

                ValidateBaseTokens()
                If StoryBucket.IsEmptyString Then Throw New TokensException("Unable to obtain 'StoryBucket'", False)

                URL = String.Format(Graphql_UrlPattern, Token_lsd, DocID_Stories, Header_fb_fr_name_Stories, Token_dtsg_Var,
                                    SymbolsConverter.ASCII.EncodeSymbolsOnly("{" & String.Format(VarPattern, StoryBucket) & "}"))

                ResponserApplyDefs(Header_fb_fr_name_Stories)
                ThrowAny(Token)

                WaitTimer()
                Dim r$ = Responser.GetResponse(URL)
                If Not r.IsEmptyString Then r = RegexReplace(r, RParams.DM("[^\r\n]+", 0, EDP.ReturnValue))
                If Not r.IsEmptyString Then
                    Using j As EContainer = JsonDocument.Parse(r)
                        If j.ListExists Then
                            With j({"data", "bucket", "unified_stories", "edges"})
                                If .ListExists Then
                                    ProgressPre.ChangeMax(.Count)
                                    For Each jNode As EContainer In .Self
                                        ProgressPre.Perform()
                                        With jNode({"node"})
                                            If .ListExists Then
                                                pid = New PostKV(.Value("id"), "", Sections.Stories)
                                                With .ItemF({"attachments", 0, "media"})
                                                    If .ListExists Then
                                                        pid.ID = .Value("id")
                                                        pUrl = String.Empty
                                                        postDate = AConvert(Of Date)(.Value("creation_time"), UnixDate32Provider, Nothing)
                                                        Select Case .Value("__typename")
                                                            Case "Photo"
                                                                t = UTypes.Picture
                                                                pUrl = .Value({"image"}, "uri")
                                                            Case "Video"
                                                                t = UTypes.Video
                                                                pUrl = .Value("browser_native_hd_url").IfNullOrEmpty(.Value("browser_native_sd_url"))
                                                        End Select
                                                        If Not pUrl.IsEmptyString AndAlso Not PostKvExists(pid) Then
                                                            pUrlBase = $"https://www.facebook.com/stories/{StoryBucket}"
                                                            PostsKVIDs.Add(pid)
                                                            _TempMediaList.ListAddValue(New UserMedia(pUrl, t) With {
                                                                                        .URL_BASE = pUrlBase,
                                                                                        .File = CreateFileFromUrl(pUrl),
                                                                                        .SpecialFolder = $"{StoriesFolder} (user)",
                                                                                        .Post = New UserPost(pid.ID, postDate)}, LNC)
                                                        End If
                                                    End If
                                                End With
                                            End If
                                        End With
                                    Next
                                End If
                            End With
                        End If
                    End Using
                End If
            Catch tex As TokensException When Not tex.BasicTokens
                TokensException.SendToLog(Me, tex, "data (stories)")
            Catch ex As Exception
                ProcessException(ex, Token, $"data (stories) downloading error [{URL}]",, Responser)
            End Try
        End Sub
        Private Sub DownloadData_SavedPosts(ByVal Cursor As String, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Const VarPattern$ = """content_filter"":[],""count"":10,""cursor"":{0},""scale"":1,""use_case"":""SAVE_DEFAULT"""
            Try
                Dim nextCursor$ = String.Empty
                Dim newPostsDetected As Boolean = False
                Dim pUrl$, videoId$, imgUri$
                Dim imgFile As SFile
                Dim pid As PostKV

                ValidateBaseTokens()
                URL = String.Format(Graphql_UrlPattern, Token_lsd, DocID_SavedPosts, Header_fb_fr_name_SavedPosts, Token_dtsg_Var,
                                    SymbolsConverter.ASCII.EncodeSymbolsOnly("{" & String.Format(VarPattern, If(Cursor.IsEmptyString, "null", $"""{Cursor}""")) & "}"))

                ResponserApplyDefs(Header_fb_fr_name_SavedPosts)
                ThrowAny(Token)

                WaitTimer()
                Dim r$ = Responser.GetResponse(URL)
                If Not r.IsEmptyString Then
                    Using j As EContainer = JsonDocument.Parse(r)
                        If j.ListExists Then
                            With j({"data", "viewer", "saver_info", "all_saves", "edges"})
                                If .ListExists Then
                                    ProgressPre.ChangeMax(.Count)
                                    For Each jNode As EContainer In .Self
                                        ProgressPre.Perform()
                                        nextCursor = jNode.Value("cursor")
                                        pid = New PostKV("", jNode.Value({"node"}, "id"), Sections.SavedPosts)
                                        If Not PostKvExists(pid) Then
                                            PostsKVIDs.Add(pid)
                                            newPostsDetected = True
                                            With jNode({"node", "savable"})
                                                If .ListExists Then
                                                    pUrl = .Value("savable_permalink")
                                                    If Not pUrl.IsEmptyString Then
                                                        Select Case .Value("savable_default_category").StringToLower
                                                            Case "post_with_photo"
                                                                imgUri = .Value({"savable_image"}, "uri")
                                                                If Not imgUri.IsEmptyString Then
                                                                    imgFile = CreateFileFromUrl(imgUri)
                                                                    If Not imgFile.Name.IsEmptyString Then
                                                                        ThrowAny(Token)
                                                                        _TempMediaList.ListAddList(DownloadData_SavedPosts_ParseImagePost(pUrl, imgFile.Name, Token))
                                                                    End If
                                                                End If
                                                            Case "video"
                                                                videoId = RegexReplace(pUrl, Regex_VideoIDFromURL)
                                                                If Not videoId.IsEmptyString Then _
                                                                   _TempMediaList.ListAddValue(New UserMedia(pUrl, UTypes.VideoPre) With {.Post = videoId}, LNC)
                                                            Case Else : Continue For
                                                        End Select
                                                    End If
                                                End If
                                            End With
                                        End If
                                    Next
                                End If
                            End With
                        End If
                    End Using
                End If

                If newPostsDetected And Not nextCursor.IsEmptyString Then DownloadData_SavedPosts(nextCursor, Token)
            Catch ex As Exception
                ProcessException(ex, Token, $"data (saved posts) downloading error [{URL}]",, Responser)
            End Try
        End Sub
        Private Function DownloadData_SavedPosts_ParseImagePost(ByVal PostUrl As String, ByVal ImageName As String, ByVal Token As CancellationToken,
                                                                Optional ByVal Round As Integer = 0) As IEnumerable(Of UserMedia)
            Dim resp As Responser = HtmlResponserCreate()
            Try
                If Round > 0 Then ThrowAny(Token)
                Dim script$, newUrl$
                Dim jNode As EContainer, jNode2 As EContainer
                WaitTimer()
                Dim r$ = resp.GetResponse(PostUrl)

                If Not r.IsEmptyString Then
                    script = RegexReplace(r, RParams.DMS($"<script type=""application/json""[^\>]*data-sjs>([^<]+?{ImageName}[^<]+)<", 1, EDP.ReturnValue))
                    If Not script.IsEmptyString Then
                        Using j As EContainer = JsonDocument.Parse(script)
                            If j.ListExists Then
                                jNode = j.Find(Function(jj) Not jj.Name.IsEmptyString AndAlso jj.Name.ToLower = "prefetch_uris_v2", True)
                                If jNode.ListExists Then
                                    For Each vNode As EContainer In jNode
                                        newUrl = RegexReplace(vNode.Value("uri"), Regex_PostHtmlFullPicture)
                                        If Not newUrl.IsEmptyString Then _
                                           Return {New UserMedia(newUrl, UTypes.Picture) With {.URL_BASE = PostUrl, .File = CreateFileFromUrl(newUrl)}}
                                    Next
                                End If
                                If Round = 0 Then
                                    j.SetSourceReferences()
                                    jNode = j.GetNode(SpecialNode)
                                    If Not jNode Is Nothing AndAlso Not jNode.Value.IsEmptyString AndAlso Not jNode.Source Is Nothing Then
                                        With DirectCast(jNode.Source, EContainer)
                                            If Not .Source Is Nothing Then
                                                newUrl = DirectCast(.Source, EContainer).Value("url")
                                                If Not newUrl.IsEmptyString Then
                                                    Dim __data As IEnumerable(Of UserMedia) =
                                                        DownloadData_SavedPosts_ParseImagePost(newUrl, CreateFileFromUrl(jNode.Value).Name, Token, Round + 1)
                                                    If __data.ListExists Then Return __data
                                                End If
                                            End If
                                        End With
                                    End If
                                End If

                                jNode = j.Find(Function(jj) Not jj.Name.IsEmptyString AndAlso jj.Name = "viewer_image", True)
                                If Not jNode Is Nothing AndAlso Not jNode.Source Is Nothing Then
                                    Dim doRound% = 0
                                    Do : doRound += 1 : jNode = jNode.Source : Loop While doRound <= 30 AndAlso Not jNode Is Nothing AndAlso Not jNode.Name = "nodes"
                                    If Not jNode Is Nothing AndAlso jNode.Name = "nodes" AndAlso jNode.Count > 0 Then
                                        Dim mList As New List(Of UserMedia)
                                        For Each jNode2 In jNode
                                            With jNode2
                                                newUrl = .Value({"media", "viewer_image"}, "uri")
                                                If Not newUrl.IsEmptyString Then _
                                                   mList.Add(New UserMedia(newUrl, UTypes.Picture) With {.URL_BASE = PostUrl, .File = CreateFileFromUrl(newUrl)})
                                            End With
                                        Next
                                        Return mList
                                    End If
                                End If

                                newUrl = j.GetNode(SpecialNode2).XmlIfNothingValue
                                If Not newUrl.IsEmptyString Then _
                                   Return {New UserMedia(newUrl, UTypes.Picture) With {.URL_BASE = PostUrl, .File = CreateFileFromUrl(newUrl)}}
                            End If
                        End Using
                    End If
                End If
                Return Nothing
            Catch ex As Exception
                ProcessException(ex, Token, $"data (saved posts) downloading error [{PostUrl}]",, resp, False)
                Return Nothing
            Finally
                HtmlResponserDispose(resp)
            End Try
        End Function
#End Region
#Region "ValidateBaseTokens, GetVideoPageID, GetUserTokens"
        ''' <exception cref="ArgumentNullException"></exception>
        Protected Overrides Function ValidateBaseTokens() As Boolean
            Dim tokens$ = String.Empty
            If Not ValidateBaseTokens(tokens) Then
                DisableDownload()
                Throw New TokensException($"Unable to obtain token(s) ({tokens}). Your credentials may have expired.", True)
            Else
                Return True
            End If
        End Function
        Private Sub GetVideoPageID(ByVal Token As CancellationToken)
            Dim URL$ = $"{GetProfileUrl()}\videos"
            Dim resp As Responser = HtmlResponserCreate()
            Try
                WaitTimer()
                Dim r$ = resp.GetResponse(URL)
                If Not r.IsEmptyString Then VideoPageID = RegexReplace(r, Regex_VideoPageID)
            Catch ex As Exception
                ProcessException(ex, Token, "get video page ID",, resp)
            Finally
                HtmlResponserDispose(resp)
            End Try
        End Sub
        Private Sub GetUserTokens(ByVal Token As CancellationToken)
            Dim URL$ = If(IsSavedPosts, "https://www.facebook.com/saved", GetProfileUrl())
            Dim resp As Responser = HtmlResponserCreate()
            Try
                ResetBaseTokens()
                Token_Photosby = String.Empty
                WaitTimer()
                Dim r$ = resp.GetResponse(URL)
                If Not r.IsEmptyString Then
                    If Responser.CookiesExists Then Responser.Cookies.Update(resp.Cookies)
                    ParseTokens(r, 0)
                    Dim app_id$ = RegexReplace(r, Regex_AppID)
                    If Not app_id.IsEmptyString Then
                        If Not AEquals(Of String)(MySettings.HH_IG_APP_ID.Value, app_id) Then
                            MySettings.HH_IG_APP_ID.Value = app_id
                            Responser.Headers.Add(IG.Header_IG_APP_ID, app_id)
                        End If
                    End If
                    Token_Photosby = RegexReplace(r, Regex_Photos_by)
                    If StoryBucket.IsEmptyString Then StoryBucket = RegexReplace(r, Regex_StoryBucket)
                    If ID.IsEmptyString Then
                        ID = RegexReplace(r, Regex_UserID)
                        If Not ID.IsEmptyString Then _ForceSaveUserInfo = True
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex, Token, "get user token",, resp)
            Finally
                HtmlResponserDispose(resp)
            End Try
        End Sub
#End Region
#Region "Responser options"
        Private Sub ResponserApplyDefs(ByVal __fb_friendly_name As String)
            With Responser
                UpdateHeadersGQL(__fb_friendly_name)
                .Method = "POST"
                .Accept = "*/*"
                .Referer = GetProfileUrl()
            End With
        End Sub
        Private Function HtmlResponserCreate() As Responser
            Dim r As Responser = Responser.Copy
            With r
                .Accept = CStr(AConvert(Of String)(MySettings.Header_Accept.Value, String.Empty))
                .Referer = Nothing
                .Method = "GET"
                With .Headers
                    .Clear()
                    .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.Authority, "www.facebook.com"))
                    .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecFetchDest, "document"))
                    .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecFetchMode, "navigate"))
                    .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecFetchSite, "none"))
                    .Add("Sec-Fetch-User", "?1")
                    .Add("Upgrade-Insecure-Requests", 1)
                    Dim cloneHeader As Action(Of String) = Sub(ByVal hName As String)
                                                               Dim hValue$ = Responser.Headers.Value(hName)
                                                               If Not hValue.IsEmptyString Then .Add(hName, hValue)
                                                           End Sub
                    cloneHeader.Invoke(IG.Header_Browser)
                    cloneHeader.Invoke(IG.Header_BrowserExt)
                    cloneHeader.Invoke(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecChUaPlatform).Name)
                    cloneHeader.Invoke(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecChUaPlatformVersion).Name)
                    .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecChUaMobile, "?0"))
                    .Add("Sec-Ch-Ua-Model", "")
                End With
            End With
            Return r
        End Function
        Private Sub HtmlResponserDispose(ByVal r As Responser)
            If Not r Is Nothing Then
                Responser.Cookies.Update(r.Cookies)
                r.Dispose()
            End If
        End Sub
#End Region
#Region "ReparseMissing"
        Protected Overrides Sub ReparseMissing(ByVal Token As CancellationToken)
            Dim rList As New List(Of Integer)
            Dim resp As Responser = HtmlResponserCreate()
            Try
                If ContentMissingExists Then
                    Dim m As UserMedia
                    Dim result As Boolean
                    ProgressPre.ChangeMax(_ContentList.Count)
                    For i% = 0 To _ContentList.Count - 1
                        ProgressPre.Perform()
                        m = _ContentList(i)
                        If (m.State = UStates.Missing And (m.Type = UTypes.Video Or m.Type = UTypes.VideoPre)) AndAlso Not m.URL_BASE.IsEmptyString Then
                            ThrowAny(Token)
                            result = False
                            m = ReparseSingleVideo(m, resp, result)
                            If result Then
                                rList.Add(i)
                                m.State = UStates.Missing
                                _TempMediaList.ListAddValue(m, LNC)
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
                HtmlResponserDispose(resp)
            End Try
        End Sub
#End Region
#Region "ReparseVideo"
        Protected Overrides Sub ReparseVideo(ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Dim resp As Responser = HtmlResponserCreate()
            Try
                If _TempMediaList.Count > 0 AndAlso _TempMediaList.Exists(Function(mm) mm.Type = UTypes.VideoPre) Then
                    ProgressPre.ChangeMax(_TempMediaList.Count)
                    Dim m As UserMedia
                    Dim result As Boolean
                    For i% = 0 To _TempMediaList.Count - 1
                        m = _TempMediaList(i)
                        If m.Type = UTypes.VideoPre Then
                            ThrowAny(Token)
                            result = False
                            m = ReparseSingleVideo(m, resp, result)
                            If Not result Then m.State = UStates.Missing
                            _TempMediaList(i) = m
                        End If
                        ProgressPre.Perform()
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex, Token, $"video reparsing error [{URL}]",, resp)
            Finally
                HtmlResponserDispose(resp)
            End Try
        End Sub
        Protected Function ReparseSingleVideo(ByVal m As UserMedia, ByVal resp As Responser, ByRef result As Boolean) As UserMedia
            Const nameSD$ = "browser_native_sd_url"
            Const nameHD$ = "browser_native_hd_url"
            Const pattern$ = "<script type=""application/json""[^\>]*data-sjs>([^<]+?{0}[^<]+)<"
            Dim URL$ = String.Empty
            Dim j As EContainer = Nothing
            Try
                Dim r$, script$, __url$
                Dim jNode As EContainer
                Dim jf As Predicate(Of EContainer) = Function(ee) Not ee.Name.IsEmptyString AndAlso (ee.Name.ToLower = nameSD Or ee.Name.ToLower = nameHD)
                Dim re As RParams = RParams.DMS("", 1, RegexOptions.IgnoreCase, EDP.ReturnValue)
                If m.Post.ID.IsEmptyString Then
                    URL = m.URL_BASE
                Else
                    URL = String.Format(VideoHtmlUrlPattern, m.Post.ID)
                End If
                WaitTimer()
                r = resp.GetResponse(URL)
                If Not r.IsEmptyString Then
                    re.Pattern = String.Format(pattern, nameHD)
                    script = RegexReplace(r, re)
                    If script.IsEmptyString Then
                        re.Pattern = String.Format(pattern, nameSD)
                        script = RegexReplace(r, re)
                    End If
                    If Not script.IsEmptyString Then
                        j = JsonDocument.Parse(script)
                        If j.ListExists Then
                            j.SetSourceReferences()
                            jNode = j.Find(jf, True)
                            If Not jNode Is Nothing Then
                                With DirectCast(jNode.Source, EContainer)
                                    __url = .Value(nameHD).IfNullOrEmpty(.Value(nameSD))
                                    If Not __url.IsEmptyString Then
                                        m.URL = __url
                                        m.URL_BASE = URL
                                        m.Type = UTypes.Video
                                        m.File = CreateFileFromUrl(__url)
                                        m.Post.Date = AConvert(Of Date)(.Value("publish_time"), UnixDate32Provider, Nothing)
                                        result = True
                                        Return m
                                    End If
                                End With
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
            End Try
            j.DisposeIfReady
            result = False
            Return m
        End Function
#End Region
#Region "CreateFileFromUrl"
        Protected Overrides Function CreateFileFromUrl(ByVal URL As String) As SFile
            If Not URL.IsEmptyString Then
                Dim f$ = RegexReplace(URL, Regex_FileName)
                If Not f.IsEmptyString Then
                    Return f
                Else
                    Dim ff As New SFile(URL)
                    If Not ff.Extension.IsEmptyString Then
                        If ff.Length > 4 Then ff.Extension = ff.Extension.Split("?").FirstOrDefault
                        ff.Extension = ff.Extension.StringRemoveWinForbiddenSymbols
                    End If
                    ff.Name = ff.Name.StringRemoveWinForbiddenSymbols
                    Return ff
                End If
            End If
            Return String.Empty
        End Function
#End Region
#Region "DownloadContent"
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
#End Region
#Region "DownloadSingleObject"
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            _ContentList.Add(New UserMedia(Data.URL, UTypes.VideoPre) With {.Post = CStr(AConvert(Of String)(Data.URL, Regex_VideoIDFromURL, String.Empty))})
            ReparseMissing(Token)
        End Sub
#End Region
#Region "ThrowAny"
        Friend Overrides Sub ThrowAny(ByVal Token As CancellationToken)
            ThrowAnyImpl(Token)
        End Sub
#End Region
#Region "Exception"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            Return 0
        End Function
#End Region
    End Class
End Namespace