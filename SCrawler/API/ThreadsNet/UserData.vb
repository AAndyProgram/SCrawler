' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports SCrawler.Plugin
Imports SCrawler.API.Base
Imports SCrawler.API.YouTube.Objects
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Clients.EventArguments
Imports IGS = SCrawler.API.Instagram.SiteSettings
Namespace API.ThreadsNet
    Friend Class UserData : Inherits Instagram.UserData
#Region "XML names"
        Private Const Name_MaxLastDownDate As String = "MaxLastDownDate"
        Private Const Name_FirstLoadingDone As String = "FirstLoadingDone"
#End Region
#Region "Declarations"
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return HOST.Source
            End Get
        End Property
        Private ReadOnly DefaultParser_ElemNode_Default() As Object = {"node", "thread_items", 0, "post"}
        Private ReadOnly Property Valid As Boolean
            Get
                Return ValidateBaseTokens() And Not ID.IsEmptyString
            End Get
        End Property
        Private Property MaxLastDownDate As Date? = Nothing
        Private Property FirstLoadingDone As Boolean = False
#End Region
#Region "Loader"
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            With Container
                If Loading Then
                    MaxLastDownDate = AConvert(Of Date)(.Value(Name_MaxLastDownDate), DateTimeDefaultProvider, Nothing)
                    FirstLoadingDone = .Value(Name_FirstLoadingDone).FromXML(Of Boolean)(False)
                Else
                    .Add(Name_MaxLastDownDate, AConvert(Of String)(MaxLastDownDate, DateTimeDefaultProvider, String.Empty))
                    .Add(Name_FirstLoadingDone, FirstLoadingDone.BoolToInteger)
                End If
            End With
        End Sub
#End Region
#Region "Exchange"
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return New EditorExchangeOptionsBase(Me)
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            If Not Obj Is Nothing AndAlso TypeOf Obj Is EditorExchangeOptionsBase Then NameTrue = DirectCast(Obj, EditorExchangeOptionsBase).UserName
        End Sub
#End Region
#Region "Initializer"
        Friend Sub New()
            ObtainMedia_SetReelsFunc()
            ObtainMedia_AllowAbstract = True
            DefaultParser_ElemNode = DefaultParser_ElemNode_Default
            DefaultParser_PostUrlCreator = Function(post) $"https://www.threads.net/@{NameTrue}/post/{post.Code}"
            _ResponserAutoUpdateCookies = True
            _ResponserAddResponseReceivedHandler = True
            DefaultParser_Pinned = AddressOf IsPinnedPost
        End Sub
#End Region
#Region "Download functions"
        Private Sub WaitTimer()
            If CInt(MySettings.RequestsWaitTimer_Any.Value) > 0 Then Thread.Sleep(CInt(MySettings.RequestsWaitTimer_Any.Value))
        End Sub
        Private Sub DisableDownload()
            MySettings.DownloadData_Impl.Value = False
            LogError(Nothing, $"{Site} downloading is disabled until you update your credentials")
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            If CBool(MySettings.DownloadData_Impl.Value) Then
                Dim errorFound As Boolean = False
                Try
                    _IdChanged = False
                    Responser.Method = "POST"
                    LoadSavePostsKV(True)
                    ResetBaseTokens()
                    Dim setMaxPostDate As Action(Of List(Of UserMedia)) =
                        Sub(ByVal l As List(Of UserMedia))
                            With (From c As UserMedia In l Where c.Post.Date.HasValue Select c.Post.Date.Value)
                                If .ListExists Then MaxLastDownDate = .Max : _ForceSaveUserInfo = True
                            End With
                        End Sub
                    If FirstLoadingDone Then
                        If Not MaxLastDownDate.HasValue And _ContentList.Count > 0 Then setMaxPostDate.Invoke(_ContentList)
                    Else
                        If _ContentList.Count > 0 Then
                            FirstLoadingDone = True
                            If Not MaxLastDownDate.HasValue Then setMaxPostDate.Invoke(_ContentList)
                        End If
                    End If
                    If FirstLoadingDone Then
                        DefaultParser_SkipPost = Nothing
                    Else
                        DefaultParser_SkipPost = AddressOf SkipPost
                    End If
                    If IsSavedPosts Then
                        DefaultParser_ElemNode = {"node", "thread_items", 0, "post"}
                        DownloadSavedPosts(String.Empty, 0, Token)
                    Else
                        DownloadData(String.Empty, 0, Token)
                    End If
                    If _TempMediaList.Count > 0 Then FirstLoadingDone = True : setMaxPostDate.Invoke(_TempMediaList)
                Catch ex As Exception
                    errorFound = True
                    Throw ex
                Finally
                    Responser.Method = "POST"
                    UpdateResponser()
                    MySettings.UpdateResponserData(Responser)
                    ValidateExtension()
                    If Not errorFound Then LoadSavePostsKV(False)
                End Try
            End If
        End Sub
        Private Function IsPinnedPost(ByVal Items As IEnumerable(Of EContainer), ByVal Index As Integer) As Boolean
            Try
                If IsSavedPosts Then
                    Return False
                Else
                    With Items(Index).ItemF(DefaultParser_ElemNode)
                        Return .Value({"text_post_app_info", "pinned_post_info"}, "is_pinned_to_profile").FromXML(Of Boolean)(False)
                        If MaxLastDownDate.HasValue Then
                            Dim d As Date? = AConvert(Of Date)(.Value("taken_at"), UnixDate32Provider, Nothing)
                            If d.HasValue Then Return d.Value <= MaxLastDownDate.Value
                        End If
                    End With
                    Return Not FirstLoadingDone
                End If
            Catch ex As Exception
                LogError(ex, "IsPinnedPost")
                Return Not FirstLoadingDone
            End Try
        End Function
        Private Function SkipPost(ByVal Items As IEnumerable(Of EContainer), ByVal Index As Integer, ByVal Post As PostKV) As Boolean
            Return PostKvExists(Post)
        End Function
        Protected Overrides Sub UpdateResponser()
            If Not Responser Is Nothing AndAlso Not Responser.Disposed Then
                RemoveHandler Responser.ResponseReceived, AddressOf Responser_ResponseReceived
            End If
        End Sub
        Protected Overrides Sub Responser_ResponseReceived(ByVal Sender As Object, ByVal e As WebDataResponse)
            If e.CookiesExists Then
                Dim csrf$ = If(e.Cookies.FirstOrDefault(Function(v) v.Name.StringToLower = IGS.Header_CSRF_TOKEN_COOKIE)?.Value, String.Empty)
                If Not csrf.IsEmptyString AndAlso Not AEquals(Of String)(csrf, Responser.Headers.Value(IGS.Header_CSRF_TOKEN)) Then _
                   Responser.Headers.Add(IGS.Header_CSRF_TOKEN, csrf)
            End If
        End Sub
        'Private Const GQL_Q As String = "https://www.threads.net/api/graphql?lsd={0}&fb_dtsg={1}&doc_id={2}&fb_api_req_friendly_name={3}&server_timestamps=true&variables={4}"
        Private Const GQL_Q2 As String = "https://www.threads.net/graphql/query"
        Private Const PayloadData As String = "lsd={0}&fb_dtsg={1}&doc_id={2}&fb_api_req_friendly_name={3}&server_timestamps=true&variables={4}"
        Private Const GQL_P_DOC_ID As String = "9039187972876777" '"8779269398849532" '"6371597506283707"
        Private Const GQL_P_NAME As String = "BarcelonaProfileThreadsTabRefetchableDirectQuery" '"BarcelonaProfileThreadsTabRefetchableQuery"
        'Private Const GQL_S_DOC_ID_1 As String = "9227844190587889" '"7758166704280174"
        'Private Const GQL_S_NAME_1 As String = "BarcelonaSavedPageViewerQuery"
        Private Const GQL_S_DOC_ID_2 As String = "9116629201788321" '"8617275414954442"
        Private Const GQL_S_NAME_2 As String = "BarcelonaSavedPageRefetchableQuery"
        Private Sub DownloadCheckCredentials()
            If Not Valid Then
                Dim idIsNull As Boolean = ID.IsEmptyString
                UpdateCredentials()
                If idIsNull And Not ID.IsEmptyString Then _ForceSaveUserInfo = True
            End If
            If Not Valid Then DisableDownload() : Throw New Plugin.ExitException("Some credentials are missing")
        End Sub
        Private Function CheckErrors(ByVal e As EContainer) As Boolean
            Return e.ListExists AndAlso Not JsonErrorMessage(e).IsEmptyString
        End Function
        Private Function JsonErrorMessage(ByVal e As EContainer) As String
            Return e.ItemF({"errors", 0, "summary"})?.Value
        End Function
        Private Sub ProcessJsonErrorException(ByVal uex As JsonErrorException, Optional ByVal ThrowEx As Boolean = True)
            If uex.UserNotFound Then
                UserExists = False
                _ForceSaveUserInfo = True
                _ForceSaveUserInfoOnException = True
            ElseIf ThrowEx Then
                Throw New ExitException(uex.ErrMessage) With {.SimpleLogLine = True}
            Else
                LogError(Nothing, uex.ErrMessage)
            End If
        End Sub
        Private Class JsonErrorException : Inherits Exception
            Friend Property UserNotFound As Boolean = False
            Private _ErrMessage As String = String.Empty
            Public Property ErrMessage As String
                Get
                    Return _ErrMessage
                End Get
                Set(ByVal m As String)
                    _ErrMessage = m
                    UserNotFound = _ErrMessage.StringToLower = "not found"
                End Set
            End Property
            Public Overrides ReadOnly Property Message As String
                Get
                    Return _ErrMessage
                End Get
            End Property
            Friend Sub New()
            End Sub
            Friend Sub New(ByVal Message As String)
                ErrMessage = Message
            End Sub
        End Class
        Private Overloads Sub DownloadData(ByVal Cursor As String, ByVal Round As Integer, ByVal Token As CancellationToken)
            'Const var_init$ = """userID"":""{0}"""
            'Const var_cursor$ = """after"":""{1}"",""before"":null,""first"":25,""last"":null,""userID"":""{0}"",""__relay_internal__pv__BarcelonaIsLoggedInrelayprovider"":true,""__relay_internal__pv__BarcelonaIsFeedbackHubEnabledrelayprovider"":false"
            Const var_cursor2$ = """after"":{1},""before"":null,""first"":10,""last"":null,""userID"":""{0}"",""__relay_internal__pv__BarcelonaIsLoggedInrelayprovider"":true,""__relay_internal__pv__BarcelonaHasSelfReplyContextrelayprovider"":false,""__relay_internal__pv__BarcelonaShareableListsrelayprovider"":true,""__relay_internal__pv__BarcelonaIsSearchDiscoveryEnabledrelayprovider"":false,""__relay_internal__pv__BarcelonaOptionalCookiesEnabledrelayprovider"":true,""__relay_internal__pv__BarcelonaQuotedPostUFIEnabledrelayprovider"":false,""__relay_internal__pv__BarcelonaIsCrawlerrelayprovider"":false,""__relay_internal__pv__BarcelonaHasDisplayNamesrelayprovider"":false,""__relay_internal__pv__BarcelonaCanSeeSponsoredContentrelayprovider"":false,""__relay_internal__pv__BarcelonaShouldShowFediverseM075Featuresrelayprovider"":true,""__relay_internal__pv__BarcelonaShouldShowTagRedesignrelayprovider"":false,""__relay_internal__pv__BarcelonaIsInternalUserrelayprovider"":false"
            Try
                DownloadCheckCredentials()

                With Responser
                    .Method = "POST"
                    .Referer = $"https://www.threads.net/@{NameTrue}"
                    .ContentType = "application/x-www-form-urlencoded"
                    With .Headers
                        .Add(GQL_HEADER_FB_LSD, Token_lsd)
                        .Add(GQL_HEADER_FB_FRINDLY_NAME, GQL_P_NAME)
                    End With
                End With

                Dim nextCursor$ = String.Empty
                Dim dataFound As Boolean = False

                Dim vars$ = String.Format(var_cursor2, ID, IIf(Cursor.IsEmptyString, "null", $"""{Cursor}"""))
                'If Cursor.IsEmptyString Then
                '    vars = String.Format(var_init, ID)
                'Else
                '    vars = String.Format(var_cursor, ID, Cursor)
                'End If
                vars = String.Format(PayloadData, Token_lsd, Token_dtsg_Var, GQL_P_DOC_ID, GQL_P_NAME,
                                     SymbolsConverter.ASCII.EncodeSymbolsOnly("{" & vars & "}"))

                'URL = String.Format(GQL_Q, Token_lsd, Token_dtsg_Var, GQL_P_DOC_ID, GQL_P_NAME, vars)

                Using j As EContainer = GetDocument(GQL_Q2, vars, Token)
                    If Not CheckErrors(j) Then
                        If j.ListExists Then
                            With j({"data", "mediaData"})
                                If .ListExists Then
                                    nextCursor = .Value({"page_info"}, "end_cursor")
                                    With .Item({"edges"})
                                        If .ListExists Then dataFound = DefaultParser(.Self, Sections.Timeline, Token)
                                    End With
                                End If
                            End With
                        End If
                    Else
                        Throw New JsonErrorException(JsonErrorMessage(j))
                    End If
                End Using

                If dataFound And Not nextCursor.IsEmptyString Then DownloadData(nextCursor, 0, Token)
            Catch uex As JsonErrorException
                If Round > 0 Then
                    ProcessJsonErrorException(uex)
                ElseIf Not _IdChanged AndAlso UpdateCredentials() Then
                    DownloadData(Cursor, Round + 1, Token)
                Else
                    ProcessJsonErrorException(uex)
                End If
            Catch eex As ExitException
                Throw eex
            Catch ex As Exception
                ProcessException(ex, Token, "data downloading error")
            End Try
        End Sub
        Private Sub DownloadSavedPosts(ByVal Cursor As String, ByVal Round As Integer, ByVal Token As CancellationToken)
            'Const var_init$ = """__relay_internal__pv__BarcelonaIsLoggedInrelayprovider"":true,""__relay_internal__pv__BarcelonaIsInlineReelsEnabledrelayprovider"":false,""__relay_internal__pv__BarcelonaUseCometVideoPlaybackEnginerelayprovider"":false,""__relay_internal__pv__BarcelonaOptionalCookiesEnabledrelayprovider"":true,""__relay_internal__pv__BarcelonaIsTextFragmentsEnabledForPostCaptionsrelayprovider"":true,""__relay_internal__pv__BarcelonaShouldShowFediverseM075Featuresrelayprovider"":true"
            'Const var_cursor$ = """after"":""{0}"",""first"":25,""__relay_internal__pv__BarcelonaIsLoggedInrelayprovider"":true,""__relay_internal__pv__BarcelonaIsInlineReelsEnabledrelayprovider"":false,""__relay_internal__pv__BarcelonaUseCometVideoPlaybackEnginerelayprovider"":false,""__relay_internal__pv__BarcelonaOptionalCookiesEnabledrelayprovider"":true,""__relay_internal__pv__BarcelonaIsTextFragmentsEnabledForPostCaptionsrelayprovider"":true,""__relay_internal__pv__BarcelonaShouldShowFediverseM075Featuresrelayprovider"":true"
            Const var_cursor2$ = """after"":{0},""first"":25,""__relay_internal__pv__BarcelonaQuotedPostUFIEnabledrelayprovider"":false,""__relay_internal__pv__BarcelonaIsLoggedInrelayprovider"":true,""__relay_internal__pv__BarcelonaHasSelfReplyContextrelayprovider"":false,""__relay_internal__pv__BarcelonaShareableListsrelayprovider"":true,""__relay_internal__pv__BarcelonaIsSearchDiscoveryEnabledrelayprovider"":false,""__relay_internal__pv__BarcelonaOptionalCookiesEnabledrelayprovider"":true,""__relay_internal__pv__BarcelonaIsCrawlerrelayprovider"":false,""__relay_internal__pv__BarcelonaHasDisplayNamesrelayprovider"":false,""__relay_internal__pv__BarcelonaCanSeeSponsoredContentrelayprovider"":false,""__relay_internal__pv__BarcelonaShouldShowFediverseM075Featuresrelayprovider"":true,""__relay_internal__pv__BarcelonaShouldShowTagRedesignrelayprovider"":false,""__relay_internal__pv__BarcelonaIsInternalUserrelayprovider"":false"
            Try
                DownloadCheckCredentials()

                With Responser
                    .Method = "POST"
                    .Referer = "https://www.threads.net/"
                    .ContentType = "application/x-www-form-urlencoded"
                    With .Headers
                        .Add(GQL_HEADER_FB_LSD, Token_lsd)
                        '.Add(GQL_HEADER_FB_FRINDLY_NAME, If(Cursor.IsEmptyString, GQL_S_NAME_1, GQL_S_NAME_2))
                        .Add(GQL_HEADER_FB_FRINDLY_NAME, GQL_S_NAME_2)
                    End With
                End With

                Dim nextCursor$ = String.Empty
                Dim dataFound As Boolean = False

                'Dim vars$ = SymbolsConverter.ASCII.EncodeSymbolsOnly("{" & If(Cursor.IsEmptyString, var_init, String.Format(var_cursor, Cursor)) & "}")
                Dim vars$ = String.Format(PayloadData, Token_lsd, Token_dtsg_Var, GQL_S_DOC_ID_2, GQL_S_NAME_2,
                                          SymbolsConverter.ASCII.EncodeSymbolsOnly("{" & String.Format(var_cursor2, IIf(Cursor.IsEmptyString, "null", $"""{Cursor}""")) & "}"))

                'If Cursor.IsEmptyString Then
                '    URL = String.Format(GQL_Q, Token_lsd, Token_dtsg_Var, GQL_S_DOC_ID_1, GQL_S_NAME_1, vars)
                'Else
                '    URL = String.Format(GQL_Q, Token_lsd, Token_dtsg_Var, GQL_S_DOC_ID_2, GQL_S_NAME_2, vars)
                'End If

                Using j As EContainer = GetDocument(GQL_Q2, vars, Token)
                    If Not CheckErrors(j) Then
                        If j.ListExists Then
                            With j({"data", "xdt_text_app_viewer", "saved_media"})
                                If .ListExists Then
                                    nextCursor = .Value({"page_info"}, "end_cursor")
                                    With .Item({"edges"})
                                        If .ListExists Then dataFound = DefaultParser(.Self, Sections.Timeline, Token)
                                    End With
                                End If
                            End With
                        End If
                    Else
                        Throw New JsonErrorException(JsonErrorMessage(j))
                    End If
                End Using

                If dataFound And Not nextCursor.IsEmptyString Then DownloadSavedPosts(nextCursor, 0, Token)
            Catch uex As JsonErrorException
                If Round > 0 Then
                    ProcessJsonErrorException(uex)
                ElseIf Not _IdChanged AndAlso UpdateCredentials() Then
                    DownloadSavedPosts(Cursor, Round + 1, Token)
                Else
                    ProcessJsonErrorException(uex)
                End If
            Catch eex As ExitException
                Throw eex
            Catch ex As Exception
                ProcessException(ex, Token, "saved posts downloading error")
            End Try
        End Sub
        Private Function GetDocument(ByVal URL As String, ByVal PayLoad As String, ByVal Token As CancellationToken, Optional ByVal Round As Integer = 0) As EContainer
            Try
                ThrowAny(Token)
                If Round > 0 AndAlso Not UpdateCredentials() Then DisableDownload() : Throw New Exception("Failed to update credentials")
                ThrowAny(Token)
                WaitTimer()
                Dim r$ = Responser.GetResponse(URL, PayLoad)
                If Not r.IsEmptyString Then Return JsonDocument.Parse(r) Else Throw New Exception("Failed to get a response")
            Catch ex As Exception
                If Round = 0 Then
                    Return GetDocument(URL, PayLoad, Token, Round + 1)
                Else
                    Throw ex
                End If
            End Try
        End Function
        Private _IdChanged As Boolean = False
        Private Function UpdateCredentials(Optional ByVal e As ErrorsDescriber = Nothing) As Boolean
            Dim URL$ = If(IsSavedPosts, "https://www.threads.net/", $"https://www.threads.net/@{NameTrue}")
            ResetBaseTokens()
            Dim headers As New HttpHeaderCollection
            headers.AddRange(Responser.Headers)
            Try
                With Responser
                    .Method = "GET"
                    .Referer = URL
                    With .Headers
                        .Clear()
                        .Add("dnt", 1)
                        .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.Authority, "www.threads.net"))
                        .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.Origin, "https://www.threads.net"))
                        .Add("Sec-Ch-Ua-Model", "")
                        .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecChUaMobile, "?0"))
                        .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecChUaPlatform, """Windows"""))
                        .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecFetchDest, "document"))
                        .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecFetchMode, "navigate"))
                        .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecFetchSite, "none"))
                        .Add("Upgrade-Insecure-Requests", 1)
                        .Add("Sec-Fetch-User", "?1")
                        .Add(IGS.Header_Browser, MySettings.HH_BROWSER.Value)
                        .Add(IGS.Header_BrowserExt, MySettings.HH_BROWSER_EXT.Value)
                    End With
                End With
                WaitTimer()
                Dim r$ = Responser.GetResponse(URL,, EDP.ThrowException)
                Dim newID$
                Dim idStr$ = String.Empty
                If Not r.IsEmptyString Then
                    ParseTokens(r, 0)
                    newID = RegexReplace(r, RParams.DMS("""props"":\{[^\{\}]*?""user_id"":""(\d+)""", 1, EDP.ReturnValue))
                    If ID.IsEmptyString OrElse ID = newID Then
                        _IdChanged = ID.IsEmptyString
                        ID = newID
                    Else
                        _IdChanged = True
                        idStr = $"user ID changed from {ID} to {newID}"
                        LogError(Nothing, idStr)
                        ID = newID
                    End If
                    If _IdChanged Then
                        If Not idStr.IsEmptyString Then UserDescriptionUpdate(idStr, True, True, True)
                        _ForceSaveUserInfo = True
                        _ForceSaveUserInfoOnException = True
                    End If
                End If
                Return Valid
            Catch ex As Exception
                Dim notFound$ = String.Empty
                ValidateBaseTokens(notFound)
                If ID.IsEmptyString Then notFound.StringAppend("User ID")
                DisableDownload()
                Dim eex As New ErrorsDescriberException($"{ToStringForLog()}: failed to update some{IIf(notFound.IsEmptyString, String.Empty, $" ({notFound})")} credentials",,, ex) With {
                    .ReplaceMainMessage = True,
                    .SendToLogOnlyMessage = Responser.StatusCode = Net.HttpStatusCode.InternalServerError And Responser.Status = Net.WebExceptionStatus.ProtocolError
                }
                'LogError(ex, $"failed to update some{IIf(notFound.IsEmptyString, String.Empty, $" ({notFound})")} credentials", e)
                LogError(eex, String.Empty, e)
                Return False
            Finally
                If headers.ListExists Then
                    Responser.Headers.Clear()
                    Responser.Headers.AddRange(headers)
                    headers.Dispose()
                End If
            End Try
        End Function
#End Region
#Region "ReparseMissing"
        Protected Overrides Sub ReparseMissing(ByVal Token As CancellationToken)
            'Const varsPattern$ = """postID"":""{0}"",""userID"":""{1}"",""__relay_internal__pv__BarcelonaIsLoggedInrelayprovider"":true,""__relay_internal__pv__BarcelonaIsFeedbackHubEnabledrelayprovider"":false"
            Const varsPattern$ = """postID"":""{0}"",""__relay_internal__pv__BarcelonaIsLoggedInrelayprovider"":true,""__relay_internal__pv__BarcelonaShouldShowFediverseM1Featuresrelayprovider"":true,""__relay_internal__pv__BarcelonaShareableListsrelayprovider"":true,""__relay_internal__pv__BarcelonaIsSearchDiscoveryEnabledrelayprovider"":false,""__relay_internal__pv__BarcelonaOptionalCookiesEnabledrelayprovider"":true,""__relay_internal__pv__BarcelonaQuotedPostUFIEnabledrelayprovider"":false,""__relay_internal__pv__BarcelonaIsCrawlerrelayprovider"":false,""__relay_internal__pv__BarcelonaHasDisplayNamesrelayprovider"":false,""__relay_internal__pv__BarcelonaCanSeeSponsoredContentrelayprovider"":false,""__relay_internal__pv__BarcelonaShouldShowFediverseM075Featuresrelayprovider"":true,""__relay_internal__pv__BarcelonaShouldShowTagRedesignrelayprovider"":false,""__relay_internal__pv__BarcelonaIsInternalUserrelayprovider"":false,""__relay_internal__pv__BarcelonaInlineComposerEnabledrelayprovider"":false"
            'Const varsPattern$ = "{""postID"":""{0}"",""__relay_internal__pv__BarcelonaIsLoggedInrelayprovider"":true,""__relay_internal__pv__BarcelonaIsFeedbackHubEnabledrelayprovider"":false}"
            'Const urlPattern$ = "https://www.threads.net/api/graphql?lsd={0}&variables={1}&fb_api_req_friendly_name=BarcelonaPostPageQuery&server_timestamps=true&fb_dtsg={2}&doc_id=25460088156920903"
            Dim rList As New List(Of Integer)
            DefaultParser_ElemNode = Nothing
            DefaultParser_IgnorePass = True
            Try
                If ContentMissingExists Then
                    Responser.Method = "POST"
                    Responser.ContentType = "application/x-www-form-urlencoded"
                    Responser.Referer = $"https://www.threads.net/@{NameTrue}"
                    If Not IsSingleObjectDownload AndAlso Not UpdateCredentials() Then Throw New Exception("Failed to update credentials")
                    Dim m As UserMedia
                    Dim vars$
                    Dim r As Byte
                    Dim j As EContainer
                    ProgressPre.ChangeMax(_ContentList.Count)
                    For i% = 0 To _ContentList.Count - 1
                        ProgressPre.Perform()
                        m = _ContentList(i)
                        If m.State = UserMedia.States.Missing And Not m.Post.ID.IsEmptyString Then
                            ThrowAny(Token)
                            'vars = SymbolsConverter.ASCII.EncodeSymbolsOnly("{" & String.Format(varsPattern, m.Post.ID.Split("_").FirstOrDefault, ID) & "}")
                            'URL = String.Format(urlPattern, Token_lsd, vars, Token_dtsg_Var)

                            vars = String.Format(PayloadData, Token_lsd, Token_dtsg_Var, "9094233770675261", "BarcelonaPostPageDirectQuery",
                                                 SymbolsConverter.ASCII.EncodeSymbolsOnly("{" & String.Format(varsPattern, m.Post.ID) & "}"))

                            For r = 0 To 1
                                j = GetDocument(GQL_Q2, vars, Token)
                                If Not CheckErrors(j) Then
                                    If j.ListExists Then
                                        With j.ItemF({"data", "data", "edges", 0, "node", "thread_items", 0, "post"})
                                            If .ListExists AndAlso DefaultParser({ .Self}, Sections.Timeline, Token) Then rList.Add(i)
                                        End With
                                        j.Dispose()
                                    End If
                                Else
                                    j.DisposeIfReady(False)
                                    If r > 0 Then
                                        ProcessJsonErrorException(New JsonErrorException(JsonErrorMessage(j)))
                                    ElseIf Not _IdChanged AndAlso UpdateCredentials() Then
                                        Continue For
                                    Else
                                        ProcessJsonErrorException(New JsonErrorException(JsonErrorMessage(j)))
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
            Catch eex As ExitException
                Throw eex
            Catch ex As Exception
                ProcessException(ex, Token, "reparseMissing error")
            Finally
                DefaultParser_ElemNode = DefaultParser_ElemNode_Default
                DefaultParser_IgnorePass = False
                If rList.Count > 0 Then
                    For i% = rList.Count - 1 To 0 Step -1 : _ContentList.RemoveAt(rList(i)) : Next
                    rList.Clear()
                End If
            End Try
        End Sub
#End Region
#Region "DownloadSingleObject"
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            Dim url$ = Data.URL_BASE.IfNullOrEmpty(Data.URL)
            Dim postCode$ = RegexReplace(url, RParams.DMS("post/([^/\?&]+)", 1, EDP.ReturnValue))
            If Not postCode.IsEmptyString Then
                Dim postId$ = CodeToID(postCode)
                If Not postId.IsEmptyString Then
                    NameTrue = MySettings.IsMyUser(url).UserName
                    DefaultParser_PostUrlCreator = Function(post) url
                    If Not NameTrue(True).IsEmptyString AndAlso UpdateCredentials(EDP.ReturnValue) Then
                        _ContentList.Add(New UserMedia(url) With {.State = UserMedia.States.Missing, .Post = postId})
                        ReparseMissing(Token)
                    End If
                End If
            End If
        End Sub
#End Region
#Region "ThrowAny"
        Friend Overrides Sub ThrowAny(ByVal Token As CancellationToken)
            ThrowAnyImpl(Token)
        End Sub
#End Region
#Region "DownloadingException"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            Return 0
        End Function
#End Region
    End Class
End Namespace