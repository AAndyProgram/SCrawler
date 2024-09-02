' Copyright (C) Andy https://github.com/AAndyProgram
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
Namespace API.Instagram
    Partial Friend Class UserData
#Region "Tokens"
        Protected Property Token_dtsg As String = String.Empty
        Protected ReadOnly Property Token_dtsg_Var As String
            Get
                Return If(Token_dtsg.IsEmptyString, String.Empty, SymbolsConverter.ASCII.EncodeSymbolsOnly(Token_dtsg))
            End Get
        End Property
        Protected Property Token_lsd As String = String.Empty
        Protected Sub ResetBaseTokens()
            Token_dtsg = String.Empty
            Token_lsd = String.Empty
        End Sub
#End Region
#Region "Headers"
        Friend Const GQL_HEADER_FB_FRINDLY_NAME As String = "x-fb-friendly-name"
        Friend Const GQL_HEADER_FB_LSD As String = "x-fb-lsd"
#End Region
#Region "Data constants"
        Private Const GQL_UserData_DocId As String = "7381344031985950"
        Private Const GQL_UserData_FbFriendlyName As String = "PolarisProfilePageContentQuery"

        Private Const GQL_Highlights_DocId As String = "8298007123561120"
        Private Const GQL_Highlights_DocId_Second As String = "7559771384111300"
        Private Const GQL_Highlights_FbFriendlyName As String = "PolarisProfileStoryHighlightsTrayContentQuery"
        Private Const GQL_Highlights_FbFriendlyName_Second As String = "PolarisStoriesV3HighlightsPageQuery"

        Private Const GQL_UserStories_DocId As String = "25231722019806941"
        Private Const GQL_UserStories_FbFriendlyName As String = "PolarisStoriesV3ReelPageStandaloneQuery"

        Private Const GQL_Timeline_DocId As String = "7268577773270422"
        Private Const GQL_Timeline_FbFriendlyName As String = "PolarisProfilePostsQuery"
        Private Const GQL_Timeline_DocId_Second As String = "7286316061475375"
        Private Const GQL_Timeline_FbFriendlyName_Second As String = "PolarisProfilePostsTabContentQuery_connection"

        Private Const GQL_Reels_DocId As String = "7191572580905225"
        Private Const GQL_Reels_FbFriendlyName As String = "PolarisProfileReelsTabContentQuery"

        Private Const GQL_Tagged_DocId As String = "7289408964443685"
        Private Const GQL_Tagged_FbFriendlyName As String = "PolarisProfileTaggedTabContentQuery"
#End Region
#Region "Url & var constants"
        Private Const GQL_URL_PATTERN_VARS As String = "doc_id={0}&lsd={1}&fb_dtsg={2}&fb_api_req_friendly_name={3}&variables={4}"
        Private Const GQL_URL As String = "https://www.instagram.com/api/graphql"
        Private Const GQL_URL_Q As String = "https://www.instagram.com/graphql/query"
#End Region
#Region "Download functions"
        Protected Sub UpdateHeadersGQL(ByVal HeaderValue As String)
            Responser.Headers.Add(GQL_HEADER_FB_FRINDLY_NAME, HeaderValue)
            Responser.Headers.Add(GQL_HEADER_FB_LSD, Token_lsd)
        End Sub
        <Obsolete("Use 'GET' function: 'GetUserData'", False)>
        Private Sub GetUserDataGQL(ByVal Token As CancellationToken)
            Dim vars$ = String.Format(GQL_URL_PATTERN_VARS, GQL_UserData_DocId, Token_lsd, Token_dtsg_Var, GQL_UserData_FbFriendlyName,
                                      SymbolsConverter.ASCII.EncodeSymbolsOnly("{" & $"""id"":""{ID}"",""relay_header"":false,""render_surface"":""PROFILE""" & "}"))
            UpdateRequestNumber()
            ChangeResponserMode(True)
            UpdateHeadersGQL(GQL_UserData_FbFriendlyName)
            Dim r$ = Responser.GetResponse(GQL_URL, vars)
            If Not r.IsEmptyString Then
                Using j As EContainer = JsonDocument.Parse(r)
                    If j.ListExists Then
                        With j({"data", "user"})
                            If .ListExists Then
                                UserSiteName = .Value("full_name").IfNullOrEmpty(UserSiteName)
                                Dim f As New SFile With {.Path = DownloadContentDefault_GetRootDir(), .Name = "ProfilePicture", .Extension = "jpg"}
                                Dim pic$ = .Value({"hd_profile_pic_url_info"}, "url").IfNullOrEmpty(.Value("profile_pic_url"))
                                If Not pic.IsEmptyString Then GetWebFile(pic, f, EDP.ReturnValue)
                                UserDescriptionUpdate(.Value("biography"))
                            End If
                        End With
                    End If
                End Using
            End If
        End Sub
        Private Function GetTimelineGQL(ByVal Cursor As String, ByVal Token As CancellationToken) As String
            Const none_cursor$ = "none"
            Dim nextCursor$ = String.Empty, hasNextPage$ = String.Empty
            Dim vars$

            ThrowAny(Token)
            UpdateRequestNumber()
            ChangeResponserMode(True)

            If Cursor.IsEmptyString Then
                vars = "{""data"":{""count"":50,""include_relationship_info"":true,""latest_besties_reel_media"":true,""latest_reel_media"":true},""username"":""" &
                        NameTrue & """,""__relay_internal__pv__PolarisShareMenurelayprovider"":false}"
                vars = String.Format(GQL_URL_PATTERN_VARS, GQL_Timeline_DocId, Token_lsd, Token_dtsg_Var, GQL_Timeline_FbFriendlyName,
                                     SymbolsConverter.ASCII.EncodeSymbolsOnly(vars))
                UpdateHeadersGQL(GQL_Timeline_FbFriendlyName)
            Else
                vars = "{""after"":""" & Cursor & """,""before"":null,""data"":{""count"":50,""include_relationship_info"":true,""latest_besties_reel_media"":true,""latest_reel_media"":true},""first"":50,""last"":null,""username"":""" &
                        NameTrue & """,""__relay_internal__pv__PolarisShareMenurelayprovider"":false}"
                vars = String.Format(GQL_URL_PATTERN_VARS, GQL_Timeline_DocId_Second, Token_lsd, Token_dtsg_Var, GQL_Timeline_FbFriendlyName_Second,
                                     SymbolsConverter.ASCII.EncodeSymbolsOnly(vars))
                UpdateHeadersGQL(GQL_Timeline_FbFriendlyName_Second)
            End If

            DefaultParser_ElemNode = {"node"}

            Dim r$ = Responser.GetResponse(GQL_URL, vars)
            If Not r.IsEmptyString Then
                Using j As EContainer = JsonDocument.Parse(r)
                    If j.ListExists Then
                        With j({"data", "xdt_api__v1__feed__user_timeline_graphql_connection"})
                            If .ListExists Then
                                With .Item("page_info")
                                    If .ListExists Then
                                        nextCursor = .Value("end_cursor")
                                        hasNextPage = .Value("has_next_page").FromXML(Of Boolean)(False)
                                    End If
                                End With
                                With .Item("edges")
                                    If .ListExists Then
                                        If Not DefaultParser(.Self, Sections.Timeline, Token) Then Throw New ExitException
                                    End If
                                End With
                            End If
                        End With
                    End If
                End Using
            End If

            Return If(hasNextPage And (Not nextCursor.IsEmptyString AndAlso Not nextCursor.StringToLower = none_cursor), nextCursor, String.Empty)
        End Function
        Private Function GetHighlightsGQL_List() As List(Of String)

            Dim nextCursor$ = String.Empty, hasNextPage$ = String.Empty
            Dim i% = -1
            Dim hList As New List(Of String)
            Dim tmpList As New List(Of String)
            Dim vars$ = String.Format(GQL_URL_PATTERN_VARS, GQL_Highlights_DocId, Token_lsd, Token_dtsg_Var, GQL_Highlights_FbFriendlyName,
                                      SymbolsConverter.ASCII.EncodeSymbolsOnly("{" & $"""user_id"":""{ID}""" & "}"))
            UpdateRequestNumber()
            ChangeResponserMode(True)
            UpdateHeadersGQL(GQL_Highlights_FbFriendlyName)
            Dim r$ = Responser.GetResponse(GQL_URL_Q, vars)

            If Not r.IsEmptyString Then
                Using j As EContainer = JsonDocument.Parse(r)
                    If j.ListExists Then
                        'With j({"data"})
                        With j({"data", "highlights"})
                            If .ListExists Then
                                With .Item("page_info")
                                    If .ListExists Then
                                        nextCursor = .Value("end_cursor")
                                        hasNextPage = .Value("has_next_page").FromXML(Of Boolean)(False)
                                    End If
                                End With
                                With .Item({"edges"})
                                    If .ListExists Then hList.ListAddList(.Select(Function(jj) jj.Value({"node"}, "id")), LNC)
                                End With
                            End If
                        End With
                    End If
                End Using
            End If
            Return hList
        End Function
        Private Sub GetHighlightsGQL(ByRef StoriesList As List(Of String), ByVal Token As CancellationToken)
            Const highlightData$ = """first"":50,""initial_reel_id"":""{0}"",""last"":2,""reel_ids"":[{1}]"
            Dim tmpList As New List(Of String)
            Dim i% = -1
            If StoriesList.ListExists Then
                tmpList.AddRange(StoriesList.Take(10))
                StoriesList.RemoveRange(0, tmpList.Count)

                Dim vars$ = String.Format(GQL_URL_PATTERN_VARS, GQL_Highlights_DocId_Second, Token_lsd, Token_dtsg_Var, GQL_Highlights_FbFriendlyName_Second,
                                          SymbolsConverter.ASCII.EncodeSymbolsOnly("{" & String.Format(highlightData, tmpList(0), tmpList.Select(Function(hl) $"""{hl}""").ListToString(",")) & "}"))
                ThrowAny(Token)
                UpdateRequestNumber()
                ChangeResponserMode(True)
                UpdateHeadersGQL(GQL_Highlights_FbFriendlyName_Second)
                Dim r$ = Responser.GetResponse(GQL_URL_Q, vars)
                If Not r.IsEmptyString Then
                    Using j As EContainer = JsonDocument.Parse(r)
                        If j.ListExists Then
                            With j({"data", "xdt_api__v1__feed__reels_media__connection", "edges"})
                                If .ListExists Then
                                    ProgressPre.ChangeMax(.Count)
                                    For Each n As EContainer In .Self : GetStoriesData_ParseSingleHighlight(n("node"), i, False, Token, Sections.Stories) : Next
                                End If
                            End With
                        End If
                    End Using
                End If
                tmpList.Clear()
            End If

            tmpList.Clear()
        End Sub
        Private Sub GetUserStoriesGQL(ByVal Token As CancellationToken)
            '"{" & $"""user_id"":""{ID}""" & "}"
            Dim vars$ = String.Format(GQL_URL_PATTERN_VARS, GQL_UserStories_DocId, Token_lsd, Token_dtsg_Var, GQL_UserStories_FbFriendlyName,
                                      SymbolsConverter.ASCII.EncodeSymbolsOnly("{" & $"""reel_ids_arr"":[""{ID}""]" & "}"))
            UpdateRequestNumber()
            ChangeResponserMode(True)
            UpdateHeadersGQL(GQL_UserStories_FbFriendlyName)
            Dim r$ = Responser.GetResponse(GQL_URL, vars)
            If Not r.IsEmptyString Then
                Using j As EContainer = JsonDocument.Parse(r)
                    If j.ListExists Then
                        Dim i% = -1
                        GetStoriesData_ParseSingleHighlight(j.ItemF({"data", "xdt_api__v1__feed__reels_media", "reels_media", 0}), i, True, Token, Sections.UserStories)
                    End If
                End Using
            End If
        End Sub
        Private WriteOnly Property GetReelsGQL_SetEnvir As Boolean
            Set(ByVal init As Boolean)
                If init Then
                    ObtainMedia_SetReelsFunc()
                    DefaultParser_PostUrlCreator = Function(post) $"{MySiteSettings.GetUserUrl(Me).TrimEnd("/")}/reel/{post.Code}"
                Else
                    ObtainMedia_SizeFuncPic = Nothing
                    ObtainMedia_SizeFuncVid = Nothing
                    DefaultParser_PostUrlCreator = DefaultParser_PostUrlCreator_Default
                End If
            End Set
        End Property
        ''' <returns>Response</returns>
        Private Function GetReelsGQL(ByVal Cursor As String) As String
            GetReelsGQL_SetEnvir = True

            Dim errData$ = String.Empty
            If Cursor.IsEmptyString And Not ValidateBaseTokens() Then GetPageTokens()
            If Cursor.IsEmptyString And Not ValidateBaseTokens(errData) Then ValidateBaseTokens_Error(errData)

            Dim vars$ = """data"":{""include_feed_video"":true,""page_size"":50,""target_user_id"":""" & ID & """}"
            If Not Cursor.IsEmptyString Then vars = $"""after"":""{Cursor}"",""before"":null,{vars},""first"":4,""last"":null"
            vars = String.Format(GQL_URL_PATTERN_VARS, GQL_Reels_DocId, Token_lsd, Token_dtsg_Var, GQL_Reels_FbFriendlyName,
                                 SymbolsConverter.ASCII.EncodeSymbolsOnly("{" & vars & "}"))
            UpdateRequestNumber()
            ChangeResponserMode(True)
            UpdateHeadersGQL(GQL_Reels_FbFriendlyName)
            Return Responser.GetResponse(GQL_URL, vars)
        End Function
        ''' <summary>Response</summary>
        Private Function GetTaggedGQL(ByVal Cursor As String) As String
            'default count = 12
            'max count = 21
            Dim vars$
            If Cursor.IsEmptyString Then
                vars = String.Format(GQL_URL_PATTERN_VARS, GQL_Tagged_DocId, Token_lsd, Token_dtsg_Var, GQL_Tagged_FbFriendlyName,
                                     SymbolsConverter.ASCII.EncodeSymbolsOnly("{" & $"""count"":50,""user_id"":""{ID}""" & "}"))
            Else
                vars = String.Format(GQL_URL_PATTERN_VARS, GQL_Tagged_DocId, Token_lsd, Token_dtsg_Var, GQL_Tagged_FbFriendlyName,
                                     SymbolsConverter.ASCII.EncodeSymbolsOnly("{" & $"""after"":""{Cursor}"",""before"":null,""count"":50,""first"":50,""last"":null,""user_id"":""{ID}""" & "}"))
            End If
            UpdateRequestNumber()
            ChangeResponserMode(True)
            UpdateHeadersGQL(GQL_Tagged_FbFriendlyName)
            Return Responser.GetResponse(GQL_URL, vars)
        End Function
#End Region
#Region "ValidateBaseTokens"
        Protected Overridable Overloads Function ValidateBaseTokens() As Boolean
            Return ValidateBaseTokens(Nothing)
        End Function
        Protected Overridable Overloads Function ValidateBaseTokens(ByRef ErrData As String) As Boolean
            ErrData = String.Empty
            If Token_dtsg.IsEmptyString Then ErrData.StringAppend("dtsg")
            If Token_lsd.IsEmptyString Then ErrData.StringAppend("lsd")
            Return ErrData.IsEmptyString
        End Function
        Protected Overridable Sub ValidateBaseTokens_Error(Optional ByVal ErrData As String = "")
            If _UseGQL Then DisableSection(Sections.Timeline)
            ExitException.ThrowTokens(Me, ErrData)
        End Sub
#End Region
#Region "GetPageTokens"
        Private Sub GetPageTokens()
            ResetBaseTokens()
            Try
                UpdateRequestNumber()
                ChangeResponserMode(False, Not _UseGQL)
                With Responser
                    With .Headers
                        .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecFetchDest, "document"))
                        .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecFetchMode, "navigate"))
                    End With
                End With
                Dim r$ = Responser.GetResponse(MySiteSettings.GetUserUrl(Me))
                ParseTokens(r, 0)
            Catch ex As Exception
            Finally
                ChangeResponserMode(_UseGQL, Not _UseGQL)
            End Try
        End Sub
        Protected Sub ParseTokens(ByVal r As String, ByVal Attempt As Integer)
            Try
                If Not r.IsEmptyString Then
                    ResetBaseTokens()
                    Select Case Attempt
                        Case 0
                            Dim rr As RParams = RParams.DM(PageTokenRegexPatternDefault, 0, RegexReturn.List, EDP.ReturnValue)
                            Dim tokens As List(Of String) = RegexReplace(r, rr)
                            Dim tt$, ttVal$
                            If tokens.ListExists Then
                                With rr
                                    .Match = Nothing
                                    .MatchSub = 1
                                    .WhatGet = RegexReturn.Value
                                End With
                                For Each tt In tokens
                                    If Not Token_lsd.IsEmptyString And Not Token_dtsg.IsEmptyString Then
                                        Exit For
                                    Else
                                        ttVal = RegexReplace(tt, rr)
                                        If Not ttVal.IsEmptyString Then
                                            If ttVal.Contains(":") Then
                                                If Token_dtsg.IsEmptyString Then Token_dtsg = ttVal
                                            Else
                                                If Token_lsd.IsEmptyString Then Token_lsd = ttVal
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                        Case 1
                            Token_dtsg = RegexReplace(r, Regex_UserToken_dtsg)
                            Token_lsd = RegexReplace(r, Regex_UserToken_lsd)
                    End Select
                    If Not ValidateBaseTokens() And Attempt = 0 Then ParseTokens(r, Attempt + 1)
                End If
            Catch
            End Try
        End Sub
#End Region
    End Class
End Namespace