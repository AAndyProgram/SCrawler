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
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Clients.EventArguments
Imports PersonalUtilities.Tools.Web.Cookies
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Imports UStates = SCrawler.API.Base.UserMedia.States
Namespace API.OnlyFans
    Friend Class UserData : Inherits UserDataBase
#Region "XML names"
        Private Const Name_MediaDownloadTimeline As String = "MediaDownloadTimeline"
        Private Const Name_MediaDownloadStories As String = "MediaDownloadStories"
        Private Const Name_MediaDownloadHighlights As String = "DownloadHighlights"
        Private Const Name_MediaDownloadChatMedia As String = "DownloadChatMedia"
#End Region
#Region "Declarations"
        Friend Property CCookie As CookieKeeper = Nothing
        Private Const HeaderSign As String = "Sign"
        Private Const HeaderTime As String = "Time"
        Private ReadOnly HighlightsList As List(Of String)
        Friend Property MediaDownloadTimeline As Boolean = True
        Friend Property MediaDownloadStories As Boolean = True
        Friend Property MediaDownloadHighlights As Boolean = True
        Friend Property MediaDownloadChatMedia As Boolean = True
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return HOST.Source
            End Get
        End Property
#End Region
#Region "Load"
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            With Container
                If Loading Then
                    MediaDownloadTimeline = .Value(Name_MediaDownloadTimeline).FromXML(Of Boolean)(True)
                    MediaDownloadStories = .Value(Name_MediaDownloadStories).FromXML(Of Boolean)(True)
                    MediaDownloadHighlights = .Value(Name_MediaDownloadHighlights).FromXML(Of Boolean)(True)
                    MediaDownloadChatMedia = .Value(Name_MediaDownloadChatMedia).FromXML(Of Boolean)(True)
                Else
                    .Add(Name_MediaDownloadTimeline, MediaDownloadTimeline.BoolToInteger)
                    .Add(Name_MediaDownloadStories, MediaDownloadStories.BoolToInteger)
                    .Add(Name_MediaDownloadHighlights, MediaDownloadHighlights.BoolToInteger)
                    .Add(Name_MediaDownloadChatMedia, MediaDownloadChatMedia.BoolToInteger)
                End If
            End With
        End Sub
#End Region
#Region "Exchange"
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return New UserExchangeOptions(Me)
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            If Not Obj Is Nothing AndAlso TypeOf Obj Is UserExchangeOptions Then
                With DirectCast(Obj, UserExchangeOptions)
                    MediaDownloadTimeline = .DownloadTimeline
                    MediaDownloadStories = .DownloadStories
                    MediaDownloadHighlights = .DownloadHighlights
                    MediaDownloadChatMedia = .DownloadChatMedia
                End With
            End If
        End Sub
#End Region
#Region "Initializer"
        Friend Sub New()
            HighlightsList = New List(Of String)
            UseInternalDownloadFileFunction = True
        End Sub
#End Region
#Region "Download functions"
        Private _OFScraperExists As Boolean = False
        Private OFSCache As CacheKeeper = Nothing
        Private _AbsMediaIndex As Integer = 0
        Private FunctionErr As Integer = FunctionErrDef
        Private Const FunctionErrDef As Integer = -100
        Private Sub ValidateOFScraper()
            _OFScraperExists = ACheck(MySettings.OFScraperPath.Value) AndAlso CStr(MySettings.OFScraperPath.Value).CSFile.Exists
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            Try
                _DownloadingException_AuthFileUpdate = False
                If Not MySettings.SessionAborted Then
                    ValidateOFScraper()
                    _AbsMediaIndex = 0
                    FunctionErr = FunctionErrDef
                    If Not CCookie Is Nothing Then CCookie.Dispose()
                    CCookie = Responser.Cookies.Copy
                    Responser.Cookies.Clear()
                    If MySettings.EnableCookiesUpdate.Value Then AddHandler Responser.ResponseReceived, AddressOf Responser_ResponseReceived
                    UpdateCookieHeader()

                    If Not IsSavedPosts Then
                        If ID.IsEmptyString Then GetUserID()
                        If ID.IsEmptyString Then Throw New ArgumentNullException("ID", "Unable to get user ID")
                    End If

                    If MediaDownloadTimeline Then DownloadTimeline(IIf(IsSavedPosts, 0, String.Empty), Token)
                    If Not IsSavedPosts Then
                        If MediaDownloadStories And FunctionErr = FunctionErrDef Then DownloadStories(Token)
                        If MediaDownloadHighlights And FunctionErr = FunctionErrDef Then DownloadHighlights(Token)
                        If MediaDownloadChatMedia And FunctionErr = FunctionErrDef Then DownloadChatMedia(0, Token)
                    End If
                End If
            Finally
                Responser_ResponseReceived_RemoveHandler()
            End Try
        End Sub
        Protected Overrides Sub Responser_ResponseReceived(ByVal Sender As Object, ByVal e As WebDataResponse)
            If e.CookiesExists And CBool(MySettings.EnableCookiesUpdate.Value) Then
                CCookie.Update(e.Cookies, CookieKeeper.UpdateModes.ReplaceByNameAll,, EDP.ReturnValue)
                UpdateCookieHeader()
            End If
        End Sub
        Private Sub UpdateCookieHeader()
            Responser.Headers.Add("Cookie", CCookie.ToString(False))
        End Sub
        Private Function ProcessFunctionErrComplete(ByVal ErrValue As Integer) As Boolean
            If ErrValue <= 0 Or (ErrValue > 0 And ErrValue <> 2) Then FunctionErr = ErrValue
            Return ErrValue <> 2
        End Function
        Friend Const A_HIGHLIGHT As String = "HL"
        Friend Const A_STORIES As String = "ST"
        Friend Const A_MESSAGE As String = "MSG"
        Private Const BaseUrlPattern As String = "https://onlyfans.com{0}"
#Region "Download timeline"
        Private Overloads Sub DownloadTimeline(ByVal Cursor As String, ByVal Token As CancellationToken)
            Dim url$ = String.Empty
            Dim _complete As Boolean = True
            Do
                Try
                    Dim tmpCursor$ = String.Empty
                    Dim hasMore As Boolean = False
                    Dim path$ = String.Empty
                    Dim postDate$, postID$
                    Dim n As EContainer
                    Dim mediaList As List(Of UserMedia)
                    Dim mediaResult As Boolean

                    If IsSavedPosts Then
                        path = $"/api2/v2/posts/bookmarks/all/?format=infinite&limit=10&offset={Cursor}"
                    Else
                        path = $"/api2/v2/users/{ID}/posts/medias?limit=50&order=publish_date_desc&skip_users=all&format=infinite&counters=1"
                        If Not Cursor.IsEmptyString Then path &= $"&counters=0&beforePublishTime={Cursor}" Else path &= "&counters=1"
                    End If

                    If UpdateSignature(path) Then
                        url = String.Format(BaseUrlPattern, path)
                        ThrowAny(Token)

                        Dim r$ = Responser.GetResponse(url)
                        If Not r.IsEmptyString Then
                            Using j As EContainer = JsonDocument.Parse(r)
                                If j.ListExists Then
                                    If IsSavedPosts Then
                                        hasMore = j.Value("hasMore").FromXML(Of Boolean)(False)
                                    Else
                                        tmpCursor = j.Value("tailMarker")
                                        hasMore = Not tmpCursor.IsEmptyString
                                    End If
                                    With j("list")
                                        If .ListExists Then
                                            ProgressPre.ChangeMax(.Count)
                                            For Each n In .Self
                                                ProgressPre.Perform()
                                                postID = n.Value("id")
                                                postDate = n.Value("postedAt")

                                                If Not _TempPostsList.Contains(postID) Then
                                                    _TempPostsList.Add(postID)
                                                Else
                                                    Exit Sub
                                                End If

                                                Select Case MyBase.CheckDatesLimit(postDate, DateProvider)
                                                    Case DateResult.Skip : Continue For
                                                    Case DateResult.Exit : Exit Sub
                                                End Select

                                                mediaResult = False
                                                mediaList = TryCreateMedia(n, postID, postDate, mediaResult)
                                                If mediaResult Then _TempMediaList.ListAddList(mediaList, LNC)
                                            Next
                                        Else
                                            hasMore = False
                                        End If
                                    End With
                                End If
                            End Using
                        End If
                    End If

                    If hasMore Then
                        If IsSavedPosts Then tmpCursor = CInt(Cursor.IfNullOrEmpty(0)) + 10
                        DownloadTimeline(tmpCursor, Token)
                    End If
                Catch ex As Exception
                    _complete = ProcessFunctionErrComplete(ProcessException(ex, Token, $"data downloading error [{url}]"))
                End Try
            Loop While Not _complete
        End Sub
#End Region
#Region "Download highlights"
        Private Overloads Sub DownloadHighlights(ByVal Token As CancellationToken)
            HighlightsList.Clear()
            DownloadHighlights(0, Token)
            If HighlightsList.Count > 0 Then HighlightsList.ForEach(Sub(hl) DownloadHighlightMedia(hl, Token))
        End Sub
        Private Overloads Sub DownloadHighlights(ByVal Cursor As Integer, ByVal Token As CancellationToken)
            Dim url$ = String.Empty
            Dim _complete As Boolean = True
            Do
                Try
                    Dim hasMore As Boolean = False
                    Dim path$ = $"/api2/v2/users/{ID}/stories/highlights?limit=5&offset={Cursor}"
                    If UpdateSignature(path) Then
                        url = String.Format(BaseUrlPattern, path)
                        ThrowAny(Token)
                        Dim r$ = Responser.GetResponse(url)
                        If Not r.IsEmptyString Then
                            Using j As EContainer = JsonDocument.Parse(r)
                                If j.ListExists Then
                                    hasMore = j.Value("hasMore").FromXML(Of Boolean)(False)
                                    With j("list")
                                        If .ListExists Then
                                            HighlightsList.AddRange(.Select(Function(e) e.Value("id")))
                                        Else
                                            hasMore = False
                                        End If
                                    End With
                                End If
                            End Using
                        End If
                    End If
                    If hasMore Then DownloadHighlights(Cursor + 5, Token)
                Catch ex As Exception
                    _complete = ProcessFunctionErrComplete(ProcessException(ex, Token, $"highlights downloading error [{url}]"))
                End Try
            Loop While Not _complete
        End Sub
        Private Sub DownloadHighlightMedia(ByVal HLID As String, ByVal Token As CancellationToken)
            Dim url$ = String.Empty
            Dim _complete As Boolean = True
            Do
                Try
                    Dim specFolder$, postID$, postDate$
                    Dim media As List(Of UserMedia)
                    Dim result As Boolean
                    Dim path$ = $"/api2/v2/stories/highlights/{HLID}"
                    If UpdateSignature(path) Then
                        url = String.Format(BaseUrlPattern, path)
                        ThrowAny(Token)
                        Dim r$ = Responser.GetResponse(url)
                        If Not r.IsEmptyString Then
                            Using j As EContainer = JsonDocument.Parse(r)
                                If j.ListExists Then
                                    specFolder = "Highlights\" & j.Value("title").StringRemoveWinForbiddenSymbols.IfNullOrEmpty(HLID)
                                    specFolder &= "*"
                                    With j("stories")
                                        If .ListExists Then
                                            ProgressPre.ChangeMax(.Count)
                                            For Each m As EContainer In .Self
                                                ProgressPre.Perform()
                                                postID = $"{A_HIGHLIGHT}_{HLID}_{m.Value("id")}"
                                                postDate = m.Value("createdAt")
                                                If Not _TempPostsList.Contains(postID) Then
                                                    _TempPostsList.Add(postID)
                                                Else
                                                    Exit Sub
                                                End If
                                                result = False
                                                media = TryCreateMedia(m, postID, postDate, result, True, specFolder)
                                                If result Then _TempMediaList.ListAddList(media, LNC)
                                            Next
                                        End If
                                    End With
                                End If
                            End Using
                        End If
                    End If
                Catch ex As Exception
                    _complete = ProcessFunctionErrComplete(ProcessException(ex, Token, $"highlights downloading error [{url}]"))
                End Try
            Loop While Not _complete
        End Sub
#End Region
#Region "Download stories"
        Private Sub DownloadStories(ByVal Token As CancellationToken)
            Dim url$ = String.Empty
            Dim _complete As Boolean = True
            Do
                Try
                    Dim specFolder$ = "Stories"
                    Dim postID$, postDate$
                    Dim media As List(Of UserMedia)
                    Dim result As Boolean
                    Dim path$ = $"/api2/v2/users/{ID}/stories"
                    If UpdateSignature(path) Then
                        url = String.Format(BaseUrlPattern, path)
                        ThrowAny(Token)
                        Dim r$ = Responser.GetResponse(url)
                        If Not r.IsEmptyString Then
                            Using j As EContainer = JsonDocument.Parse(r)
                                If j.ListExists Then
                                    ProgressPre.ChangeMax(j.Count)
                                    For Each n As EContainer In j
                                        ProgressPre.Perform()
                                        With n.ItemF({"media", 0})
                                            If .ListExists Then
                                                postID = $"{A_STORIES}_{ .Value("id")}"
                                                postDate = .Value("createdAt")
                                            Else
                                                postID = String.Empty
                                                postDate = String.Empty
                                            End If
                                        End With
                                        If Not postID.IsEmptyString Then
                                            If Not _TempPostsList.Contains(postID) Then
                                                _TempPostsList.Add(postID)
                                            Else
                                                Exit Sub
                                            End If
                                        End If
                                        result = False
                                        media = TryCreateMedia(n, postID, postDate, result, True, specFolder,, False)
                                        If result Then _TempMediaList.ListAddList(media, LNC)
                                    Next
                                End If
                            End Using
                        End If
                    End If
                Catch ex As Exception
                    _complete = ProcessFunctionErrComplete(ProcessException(ex, Token, $"stories downloading error [{url}]"))
                End Try
            Loop While Not _complete
        End Sub
#End Region
#Region "Download chat media"
        Private Sub DownloadChatMedia(ByVal Cursor As Integer, ByVal Token As CancellationToken)
            Dim url$ = String.Empty
            Dim _complete As Boolean = True
            Do
                Try
                    Dim hasMore As Boolean = False
                    Dim postID$, postDate$
                    Dim media As List(Of UserMedia)
                    Dim result As Boolean
                    Dim path$ = $"/api2/v2/chats/{ID}/media/?opened=1&limit=20&skip_users=all"
                    If Cursor > 0 Then path &= $"&offset={Cursor}"
                    If UpdateSignature(path) Then
                        url = String.Format(BaseUrlPattern, path)
                        ThrowAny(Token)
                        Dim r$ = Responser.GetResponse(url)
                        If Not r.IsEmptyString Then
                            Using j As EContainer = JsonDocument.Parse(r)
                                If j.ListExists Then
                                    hasMore = j.Value("hasMore").FromXML(Of Boolean)(False)
                                    With j("list")
                                        If .ListExists Then
                                            For Each m As EContainer In .Self
                                                postID = $"{A_MESSAGE}_{m.Value("id")}"
                                                postDate = m.Value("createdAt")
                                                If Not _TempPostsList.Contains(postID) Then
                                                    _TempPostsList.Add(postID)
                                                Else
                                                    Exit Sub
                                                End If
                                                result = False
                                                media = TryCreateMedia(m, postID, postDate, result,, "Chats*")
                                                If result Then _TempMediaList.ListAddList(media, LNC)
                                            Next
                                        End If
                                    End With
                                End If
                            End Using
                        End If
                    End If
                    If hasMore Then DownloadChatMedia(Cursor + 20, Token)
                Catch ex As Exception
                    _complete = ProcessFunctionErrComplete(ProcessException(ex, Token, $"chats downloading error [{url}]"))
                End Try
            Loop While Not _complete
        End Sub
#End Region
        Private Function GetMediaURL(ByVal m As EContainer) As String
            Dim v$
            For Each node As Object() In FilesSources
                v = If(m.ItemF(node)?.Value, String.Empty)
                If Not v.IsEmptyString Then Return v
            Next
            Return String.Empty
        End Function
        Private Function TryCreateMedia(ByVal n As EContainer, ByVal PostID As String, Optional ByVal PostDate As String = Nothing,
                                        Optional ByRef Result As Boolean = False, Optional ByVal IsHL As Boolean = False,
                                        Optional ByVal SpecFolder As String = Nothing, Optional ByVal PostUserID As String = Nothing,
                                        Optional ByVal TryUseOFS As Boolean = True) As List(Of UserMedia)
            Dim postUrl$, postUrlBase$, ext$
            Dim t As UTypes
            Dim mList As New List(Of UserMedia)
            Result = False
            With n("media")
                If .ListExists Then
                    For Each m In .Self
                        postUrl = GetMediaURL(m)
                        'If IsHL Then
                        '    'postUrl = m.Value({"files", "source"}, "url")
                        '    postUrl = GetMediaURL(m)
                        'Else
                        '    'postUrl = m.Value({"source"}, "source").IfNullOrEmpty(m.Value("full"))
                        '    postUrl = GetMediaURL(m)
                        'End If
                        postUrlBase = String.Empty
                        Select Case m.Value("type")
                            Case "photo" : t = UTypes.Picture : ext = "jpg"
                            Case "video"
                                t = UTypes.Video
                                ext = "mp4"
                                If postUrl.IsEmptyString And Not IsHL And TryUseOFS Then
                                    t = UTypes.VideoPre
                                    _AbsMediaIndex += 1
                                    If Not PostUserID.IsEmptyString And IsSingleObjectDownload Then _
                                       postUrlBase = String.Format(SiteSettings.UserPostPattern, PostID, $"u{PostUserID}")
                                End If
                            Case Else : t = UTypes.Undefined : ext = String.Empty
                        End Select
                        If Not t = UTypes.Undefined And (Not postUrl.IsEmptyString Or t = UTypes.VideoPre) Then
                            Dim media As New UserMedia(postUrl.IfNullOrEmpty(IIf(t = UTypes.VideoPre, $"{t}{_AbsMediaIndex}", String.Empty)), t) With {
                                .Post = New UserPost(PostID, AConvert(Of Date)(PostDate, DateProvider, Nothing)),
                                .SpecialFolder = SpecFolder
                            }
                            If postUrlBase.IsEmptyString And Not IsSingleObjectDownload Then postUrlBase = GetPostUrl(Me, media)
                            If Not postUrlBase.IsEmptyString Then media.URL_BASE = postUrlBase
                            media.File.Extension = ext
                            Result = True
                            mList.Add(media)
                        End If
                    Next
                End If
            End With
            Return mList
        End Function
        Private Sub GetUserID()
            Const brTag$ = "<br />"
            Dim path$ = $"/api2/v2/users/{Name}"
            Dim url$ = String.Format(BaseUrlPattern, path)
            Try
                If ID.IsEmptyString AndAlso UpdateSignature(path) Then
                    Dim r$ = Responser.GetResponse(url)
                    If Not r.IsEmptyString Then
                        Using j As EContainer = JsonDocument.Parse(r)
                            If j.ListExists Then
                                ID = j.Value("id")
                                If Not ID.IsEmptyString Then _ForceSaveUserInfo = True
                                UserSiteNameUpdate(j.Value("name"))
                                Dim descr$ = j.Value("about")
                                If Not descr.IsEmptyString Then descr = descr.Replace(brTag, String.Empty)
                                UserDescriptionUpdate(descr)
                                SimpleDownloadAvatar(j.Value("avatar"))
                                SimpleDownloadAvatar(j.Value("header"))
                            End If
                        End Using
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex, Nothing, $"user info parsing error [{url}]")
            End Try
        End Sub
        Protected Overrides Sub ReparseMissing(ByVal Token As CancellationToken)
            Const PathPattern$ = "/api2/v2/posts/{0}?skip_users=all"
            Dim rList As New List(Of Integer)
            Dim URL$ = String.Empty
            Try
                If ContentMissingExists Then
                    Dim m As UserMedia
                    Dim stateRefill As Func(Of UserMedia, Integer, UserMedia) = Function(ByVal input As UserMedia, ByVal ii As Integer) As UserMedia
                                                                                    input.State = UStates.Missing
                                                                                    input.Attempts = m.Attempts
                                                                                    Return input
                                                                                End Function
                    Dim mList As List(Of UserMedia)
                    Dim mediaResult As Boolean
                    Dim r$, path$, postDate$, postUserID$
                    Dim j As EContainer
                    ProgressPre.ChangeMax(_ContentList.Count)
                    For i% = 0 To _ContentList.Count - 1
                        ProgressPre.Perform()
                        If _ContentList(i).State = UStates.Missing Then
                            m = _ContentList(i)
                            If Not m.Post.ID.IsEmptyString Then
                                ThrowAny(Token)
                                path = String.Format(PathPattern, m.Post.ID)
                                If UpdateSignature(path) Then
                                    URL = String.Format(BaseUrlPattern, path)
                                    r = Responser.GetResponse(URL,, EDP.ReturnValue)
                                    If Not r.IsEmptyString Then
                                        j = JsonDocument.Parse(r)
                                        If Not j Is Nothing Then
                                            postDate = j.Value("postedAt")
                                            postUserID = j.Value({"author"}, "id")
                                            mediaResult = False
                                            mList = TryCreateMedia(j, m.Post.ID, postDate, mediaResult,,, postUserID)
                                            If mediaResult Then
                                                _TempMediaList.ListAddList(mList.ListForEachCopy(stateRefill, True), LNC)
                                                rList.Add(i)
                                                mList.Clear()
                                            End If
                                            j.Dispose()
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex, Token, $"ReparseMissing error [{URL}]")
            Finally
                If rList.Count > 0 Then
                    For i% = rList.Count - 1 To 0 Step -1 : _ContentList.RemoveAt(rList(i)) : Next
                    rList.Clear()
                End If
            End Try
        End Sub
#End Region
#Region "DownloadSingleObject"
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            Dim postID$ = RegexReplace(Data.URL, RegExPostID)
            If Not postID.IsEmptyString Then _ContentList.Add(New UserMedia With {.Post = postID, .State = UStates.Missing}) : ReparseMissing(Token)
        End Sub
#End Region
#Region "Auth"
        Private Function UpdateSignature(ByVal Path As String) As Boolean
            Try
                If Not Rules.Update(False) Then Rules.Update(True)
                If Rules.Exists Then
                    Const nullMsg$ = "The auth parameter(s) is null"
                    Dim j As EContainer = Rules.CurrentContainer
                    If Not j Is Nothing Then
                        Dim pattern$ = DynamicRulesEnv.GetFormat(j)

                        If pattern.IsEmptyString Then Throw New ArgumentNullException("format", nullMsg)

                        Dim li%() = j("checksum_indexes").Select(Function(e) CInt(e(0).Value)).ToArray

                        If Not li.ListExists Then Throw New ArgumentNullException("checksum_indexes", nullMsg)
                        If j.Value("static_param").IsEmptyString Then Throw New ArgumentNullException("static_param", nullMsg)
                        If j.Value("checksum_constant").IsEmptyString Then Throw New ArgumentNullException("checksum_constant", nullMsg)

                        Dim t$ = ADateTime.ConvertToUnix64(Now.ToUniversalTime).ToString
                        Dim h$ = String.Join(vbLf, j.Value("static_param"), t, Path, MySettings.HH_USER_ID.Value.ToString)

                        Dim hash$ = GetHashSha1(h)
                        Dim hashBytes() As Byte = System.Text.Encoding.ASCII.GetBytes(hash)
                        Dim hashSum% = li.Sum(Function(i) hashBytes(i)) + CInt(j.Value("checksum_constant"))
                        Dim sign$ = String.Format(pattern, hash, Math.Abs(hashSum))

                        '#If DEBUG Then
                        'Debug.WriteLine(sign)
                        'Debug.WriteLine(t)
                        '#End If

                        Responser.Headers.Add(HeaderSign, sign)
                        Responser.Headers.Add(HeaderTime, t)

                        Return True
                    End If
                Else
                    MySettings.SessionAborted = True
                End If
                Return False
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, $"{ToStringForLog()}: UpdateSignature", False)
            End Try
        End Function
        Private Function GetHashSha1(ByVal Input As String) As String
            Dim s As New Security.Cryptography.SHA1CryptoServiceProvider
            Dim inputBytes() As Byte = System.Text.Encoding.UTF8.GetBytes(Input)
            Dim hashBytes() As Byte = s.ComputeHash(inputBytes)
            s.Dispose()
            Dim result As String = String.Empty
            For Each b As Byte In hashBytes : result &= b.ToString("x2") : Next
            Return result
        End Function
#End Region
#Region "OFScraper support"
        Private Function OFS_DownloadFile(ByVal URL As String, ByVal Token As CancellationToken) As List(Of SFile)
            Try
                Const requestPattern$ = """{0}"" manual --config ""{1}"" --url {2}"
                Dim conf As SFile = OFS_CreateConfig()
                If conf.Exists Then
                    Dim command$ = String.Format(requestPattern, MySettings.OFScraperPath.Value, conf, URL)
                    '#If DEBUG Then
                    'Debug.WriteLine(command)
                    '#End If
                    Using b As New TokenBatch(Token) With {.DebugMode = False} : b.Execute(command) : End Using
                    Return SFile.GetFiles(conf, "*.mp4", IO.SearchOption.AllDirectories, EDP.ReturnValue)
                End If
                Return Nothing
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "OnlyFans.UserData.OFS_DownloadFile", Nothing)
            End Try
        End Function
        Private Function OFS_CreateConfig() As SFile
            Try
                Const confMainPattern$ = "{0}"": ""([^""]*)"""
                Const confMainPattern_Keys$ = "{0}"": ([^,]*)"
                Const confMainPatternRulesManual$ = "DYNAMIC_RULE"": (""[^""]*"")"

                Const m1 As Byte = 0 'not rules
                Const m2 As Byte = 1 'rules
                Const m3 As Byte = 2 'keys

                If OFSCache Is Nothing Then OFSCache = If(IsSingleObjectDownload, Settings.Cache.NewInstance, CreateCache())
                Dim currentCache As CacheKeeper = OFSCache.NewInstance
                currentCache.Validate()
                Dim cacheRoot As SFile = currentCache.NewPath
                cacheRoot.Exists(SFO.Path, True, EDP.ThrowException)
                Dim f As SFile = Rules.OFScraperConfigPatternFile
                Dim configText$
                If f.Exists Then
                    Dim replaceValue$ = String.Empty
                    Dim rp As RParams = RParams.DMS(String.Empty, 1, RegexReturn.Replace, RegexOptions.IgnoreCase,
                                                    CType(Function(input) replaceValue, Func(Of String, String)), String.Empty, EDP.ReturnValue)
                    Dim ff As SFile
                    configText = f.GetText
                    Dim updateConf As Action(Of String, String, Byte) =
                        Sub(ByVal patternValue As String, ByVal __replaceValue As String, ByVal mode As Byte)
                            Select Case mode
                                Case m1 : rp.Pattern = String.Format(confMainPattern, patternValue)
                                Case m2 : rp.Pattern = String.Format(confMainPatternRulesManual, patternValue)
                                Case m3 : rp.Pattern = String.Format(confMainPattern_Keys, patternValue) : __replaceValue = $"""{__replaceValue}"""
                                Case Else : Throw New ArgumentException($"Mode '{mode}' is not implemented", "mode")
                            End Select
                            rp.Nothing = configText
                            replaceValue = __replaceValue
                            configText = RegexReplace(configText, rp)
                        End Sub
                    If Not configText.IsEmptyString Then
                        updateConf("save_location", cacheRoot.PathNoSeparator.Replace("\", "/"), m1)
                        If ACheck(MySettings.OFScraperMP4decrypt.Value) Then
                            ff = CStr(MySettings.OFScraperMP4decrypt.Value)
                            If ff.Exists Then updateConf("mp4decrypt", ff.ToString.Replace("\", "/"), m1)
                        End If
                        If Settings.FfmpegFile.Exists Then updateConf("ffmpeg", Settings.FfmpegFile.File.ToString.Replace("\", "/"), m1)

                        updateConf("key-mode-default", CStr(MySettings.KeyModeDefault.Value).IfNullOrEmpty(SiteSettings.KeyModeDefault_Default), m1)
                        updateConf("keydb_api", CStr(MySettings.Keydb_Api.Value), m1)

                        If Not CStr(MySettings.OFS_KEYS_Key.Value).IsEmptyString And Not CStr(MySettings.OFS_KEYS_ClientID.Value).IsEmptyString Then
                            updateConf("private-key", CStr(MySettings.OFS_KEYS_Key.Value).Replace("\", "/"), m3)
                            updateConf("client-id", CStr(MySettings.OFS_KEYS_ClientID.Value).Replace("\", "/"), m3)
                        End If

                        If Rules.RulesReplaceConfig Then
                            If Rules.RulesConfigManualMode Then
                                updateConf(DynamicRulesEnv.DynamicRulesConfig_Mode_NodeName, "manual", m1)
                                configText = configText.Replace(DynamicRulesEnv.DynamicRulesConfigNodeName_URL, DynamicRulesEnv.DynamicRulesConfigNodeName_RULES)
                                updateConf(DynamicRulesEnv.DynamicRulesConfigNodeName_RULES, Rules.CurrentContainerRulesText, m2)
                            Else
                                Dim confUrlNode$ = If(Rules.RulesConstants.ContainsKey(DynamicRulesEnv.DynamicRulesConfigNodeName_URL_CONST_NAME),
                                                      Rules.RulesConstants(DynamicRulesEnv.DynamicRulesConfigNodeName_URL_CONST_NAME),
                                                      DynamicRulesEnv.DynamicRulesConfigNodeName_URL)
                                updateConf(DynamicRulesEnv.DynamicRulesConfigNodeName_URL, Rules.CurrentRule.UrlRaw, m1)
                                configText = configText.Replace(DynamicRulesEnv.DynamicRulesConfigNodeName_URL, confUrlNode)
                                If Rules.RulesConstants.ContainsKey(DynamicRulesEnv.DynamicRulesConfig_Mode_NodeName) Then _
                                   updateConf(DynamicRulesEnv.DynamicRulesConfig_Mode_NodeName, Rules.RulesConstants(DynamicRulesEnv.DynamicRulesConfig_Mode_NodeName), m1)
                            End If
                        End If
                        f = currentCache
                        f.Name = "config"
                        f.Extension = "json"
                        If TextSaver.SaveTextToFile(configText, f, True).Exists AndAlso OFS_CreateAuth(currentCache) Then Return f
                    End If
                End If
                Return Nothing
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "OnlyFans.UserData.OFS_CreateConfig", Nothing)
            End Try
        End Function
        Private Function OFS_CreateAuth(ByVal DestinationPath As SFile) As Boolean
            Const authText$ = """user_agent"": ""{0}"",""app-token"": ""{1}"",""x-bc"": ""{2}"",""auth_id"": ""{3}"",""sess"": ""{4}"",""auth_uid_"": ""{3}"",""cookie"": ""{5}"""
            Try
                Dim sess$ = If(If(CCookie, Responser.Cookies).FirstOrDefault(Function(c) c.Name.StringToLower = "sess")?.Value, String.Empty)
                Dim outText$ = "{""auth"":{" &
                               String.Format(authText,
                                             MySettings.UserAgent.Value,
                                             Responser.Headers.Value(SiteSettings.HeaderAppToken),
                                             Responser.Headers.Value(SiteSettings.HeaderXBC),
                                             MySettings.HH_USER_ID.Value,
                                             sess,
                                             If(CCookie, Responser.Cookies).ToString()) &
                                             "}}"
                If DestinationPath.Exists(SFO.Path, False) Then
                    Dim f As SFile = $"{DestinationPath.PathWithSeparator}main_profile\auth.json"
                    Return TextSaver.SaveTextToFile(outText, f, True).Exists
                End If
                Return False
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "OnlyFans.UserData.OFS_CreateAuth", False)
            End Try
        End Function
#End Region
#Region "DownloadContent"
        Private OFSPostFiles As Dictionary(Of String, List(Of SFile)) = Nothing
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
            OFSCache.DisposeIfReady
            OFSPostFiles.ListClearDispose
        End Sub
        Protected Overrides Function ValidateDownloadFile(ByVal URL As String, ByVal Media As UserMedia, ByRef Interrupt As Boolean) As Boolean
            Return Media.Type = UTypes.VideoPre
        End Function
        Protected Overrides Function DownloadFile(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile,
                                                  ByVal Token As CancellationToken) As SFile
            ValidateOFScraper()
            If _OFScraperExists Then
                If OFSPostFiles Is Nothing Then OFSPostFiles = New Dictionary(Of String, List(Of SFile))
                If IsSingleObjectDownload Then
                    URL = Media.URL_BASE
                Else
                    URL = GetPostUrl(Me, Media)
                End If
                If Not URL.IsEmptyString Then
                    Dim f As SFile = Nothing
                    If OFSPostFiles.Count > 0 AndAlso OFSPostFiles.ContainsKey(Media.Post.ID) AndAlso OFSPostFiles(Media.Post.ID).Count > 0 Then
                        f = OFSPostFiles(Media.Post.ID)(0)
                        OFSPostFiles(Media.Post.ID).RemoveAt(0)
                    Else
                        Dim files As List(Of SFile) = OFS_DownloadFile(URL, Token)
                        If files.ListExists Then
                            Dim ff As SFile
                            For i% = files.Count - 1 To 0 Step -1
                                ff = files(i)
                                DestinationFile.Name = ff.Name
                                DestinationFile.Extension = ff.Extension
                                If SFile.Move(ff, DestinationFile,,,, EDP.ThrowException) Then
                                    files(i) = DestinationFile
                                Else
                                    files.RemoveAt(i)
                                End If
                            Next
                            If files.Count > 0 Then
                                f = files(0)
                                files.RemoveAt(0)
                                If files.Count > 0 Then OFSPostFiles.Add(Media.Post.ID, files)
                            End If
                        End If
                    End If
                    Return f
                End If
                Return Nothing
            Else
                Throw New InvalidProgramException("OF-Scraper not found")
            End If
        End Function
#End Region
#Region "DownloadingException"
        Private _DownloadingException_AuthFileUpdate As Boolean = False
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            If Responser.StatusCode = Net.HttpStatusCode.BadRequest Or
               (Responser.StatusCode = Net.HttpStatusCode.Unauthorized And CBool(MySettings.UpdateRules401.Value)) Then '400, [401]
                If Not _DownloadingException_AuthFileUpdate AndAlso Rules.Update(True) Then
                    _DownloadingException_AuthFileUpdate = True
                    Return 2
                Else
                    MySettings.SessionAborted = True
                    MyMainLOG = $"{ToStringForLog()} [{CInt(Responser.StatusCode)}]: OnlyFans credentials expired"
                    If Responser.StatusCode = Net.HttpStatusCode.BadRequest Then
                        Return 1
                    Else
                        MyMainLOG = $"{ToStringForLog()}: Rules updated (401)"
                        Return 3
                    End If
                End If
            ElseIf Responser.StatusCode = Net.HttpStatusCode.NotFound Then '404
                UserExists = False
                Return 3
            ElseIf Responser.StatusCode = Net.HttpStatusCode.GatewayTimeout Or Responser.StatusCode = 429 Then '504, 429
                If Responser.StatusCode = 429 Then MyMainLOG = $"[429] OnlyFans too many requests ({ToStringForLog()})"
                MySettings.SessionAborted = True
                Return 3
            ElseIf Responser.StatusCode = Net.HttpStatusCode.Unauthorized Then '401
                MySettings.SessionAborted = True
                MyMainLOG = $"{ToStringForLog()} [{CInt(Responser.StatusCode)}]: OnlyFans credentials expired"
                Return 3
            ElseIf Responser.StatusCode = Net.HttpStatusCode.InternalServerError Then '500
                Return 3
            Else
                Return 0
            End If
        End Function
#End Region
#Region "IDisposable Support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then
                CCookie.DisposeIfReady(False)
                CCookie = Nothing
                HighlightsList.Clear()
                OFSCache.DisposeIfReady
                OFSPostFiles.ListClearDispose
            End If
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace