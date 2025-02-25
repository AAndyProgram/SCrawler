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
Imports SCrawler.API.YouTube.Objects
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Imports UStates = SCrawler.API.Base.UserMedia.States
Namespace API.Bluesky
    Friend Class UserData : Inherits UserDataBase
#Region "Declarations"
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return HOST.Source
            End Get
        End Property
        Private ReadOnly Property ID_Encoded As String
            Get
                Return If(ID.IsEmptyString, String.Empty, SymbolsConverter.ASCII.EncodeSymbolsOnly(ID))
            End Get
        End Property
#End Region
#Region "Loader"
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
        End Sub
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return New EditorExchangeOptionsBase(Me) With {.SiteKey = BlueskySiteKey}
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            If Not Obj Is Nothing AndAlso TypeOf Obj Is EditorExchangeOptionsBase AndAlso
               DirectCast(Obj, EditorExchangeOptionsBase).SiteKey = BlueskySiteKey Then NameTrue = DirectCast(Obj, EditorExchangeOptionsBase).UserName
        End Sub
#End Region
#Region "Initializer"
        Friend Sub New()
            UseInternalM3U8Function = True
        End Sub
#End Region
#Region "Token"
        Private Function UpdateToken(Optional ByVal Force As Boolean = False, Optional ByVal OnlyAddHeader As Boolean = False) As Boolean
            Dim process As Boolean = True
            If CDate(MySettings.TokenUpdateTime.Value).AddHours(2) <= Now Or Force Then
                process = MySettings.UpdateToken(Force)
                If process Then _TokenUpdateCount += 1
            End If
            If process Or OnlyAddHeader Then Responser.Headers.Add("authorization", MySettings.Token.Value)
            Return Not Responser.Headers.Value("authorization").IsEmptyString
        End Function
        Private _TokenUpdateCount As Integer = 0
        Private Sub TokenUpdateCountReset()
            _TokenUpdateCount = 0
        End Sub
#End Region
#Region "Download"
        Private _PostCount As Integer = 0
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            If Not CBool(MySettings.CookiesEnabled.Value) Then Responser.Cookies.Clear()
            UpdateToken(, True)
            _TokenUpdateCount = 0
            _PostCount = 0
            DownloadData(String.Empty, Token)
        End Sub
        Private Overloads Sub DownloadData(ByVal Cursor As String, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                If ID.IsEmptyString Then GetProfileInfo(Token)
                If ID.IsEmptyString Then Throw New ArgumentNullException("ID", "ID is null")
                If UpdateToken() Then
                    Dim nextCursor$ = String.Empty
                    Dim c%
                    URL = $"https://bsky.social/xrpc/app.bsky.feed.getAuthorFeed?actor={ID_Encoded}&filter=posts_and_author_threads&includePins=false&limit=99"
                    If Not Cursor.IsEmptyString Then URL &= $"&cursor={SymbolsConverter.ASCII.EncodeSymbolsOnly(Cursor)}"
                    Dim r$ = Responser.GetResponse(URL)
                    TokenUpdateCountReset()
                    If Not r.IsEmptyString Then
                        Using j As EContainer = JsonDocument.Parse(r)
                            If j.ListExists Then
                                With j("feed")
                                    If .ListExists Then
                                        For Each post As EContainer In .Self
                                            With post({"post"})
                                                c = DefaultParser(.Self,, nextCursor)
                                                Select Case c
                                                    Case CInt(DateResult.Skip) * -1 : Continue For
                                                    Case CInt(DateResult.Exit) * -1 : Exit Sub
                                                    Case Is > 0 : _PostCount += c
                                                End Select
                                                If DownloadTopCount.HasValue AndAlso DownloadTopCount.Value <= _PostCount Then Exit Sub
                                            End With
                                        Next
                                    End If
                                End With
                            End If
                        End Using

                        If Not nextCursor.IsEmptyString Then DownloadData(nextCursor, Token)
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex, Token, $"DownloadData({URL})")
            End Try
        End Sub
#End Region
#Region "DefaultParser"
        Private Const Down_ImageAddress As String = "https://cdn.bsky.app/img/feed_fullsize/plain/{0}/{1}"
        Private Function GetPostID(ByVal PostUri As String) As String
            Return If(PostUri.IsEmptyString, String.Empty, PostUri.Split("/").LastOrDefault)
        End Function
        Private Function DefaultParser(ByVal e As EContainer, Optional ByVal CheckDateLimits As Boolean = True, Optional ByRef NextCursor As String = Nothing,
                                       Optional ByVal CheckTempPosts As Boolean = True, Optional ByVal State As UStates = UStates.Unknown) As Integer
            Const exitReturn% = CInt(DateResult.Exit) * -1
            Dim postID$, postDate$, __url$, __urlBase$
            Dim updateUrl As Boolean
            Dim c% = 0
            Dim m As UserMedia
            Dim d As EContainer
            With e
                If .ListExists Then
                    postID = GetPostID(.Value("uri"))
                    postDate = String.Empty
                    __urlBase = String.Empty
                    With .Item({"record"})
                        If .ListExists Then
                            '2025-01-28T02:42:12.415Z
                            postDate = .Value("createdAt")
                            NextCursor = postDate
                            If CheckDateLimits Then
                                Select Case CheckDatesLimit(postDate, DateProvider)
                                    Case DateResult.Skip : Return CInt(DateResult.Skip) * -1 'Continue For
                                    Case DateResult.Exit : Return exitReturn 'Exit Sub
                                End Select
                            End If

                            If CheckTempPosts Then
                                If _TempPostsList.Contains(postID) Then Return exitReturn Else _TempPostsList.Add(postID)
                            End If
                            __urlBase = $"https://bsky.app/profile/{NameTrue}/post/{postID}"
                        End If
                    End With

                    Dim createMedia As Func(Of String, UTypes, UserMedia) =
                        Function(ByVal url As String, ByVal type As UTypes) As UserMedia
                            m = New UserMedia(url, type) With {
                                .URL_BASE = __urlBase,
                                .File = CreateFileFromUrl(url, type),
                                .Post = New UserPost(postID, If(AConvert(Of Date)(postDate, DateProvider, Nothing, EDP.ReturnValue), Nothing)),
                                .State = State
                            }
                            _TempMediaList.ListAddValue(m, LNC)
                            c += 1
                            Return m
                        End Function

                    For Each SecondExtraction As Boolean In {False, True}
                        With If(SecondExtraction, .Item({"record", "embed"}), .Item("embed"))
                            If .ListExists Then

                                If If(.Item("images")?.Count, 0) > 0 Then
                                    With .Item("images")
                                        For Each d In .Self
                                            updateUrl = False
                                            __url = d.Value("fullsize")
                                            If __url.IsEmptyString Then __url = d.Value({"image", "ref"}, "$link") : updateUrl = True
                                            If __url.IsEmptyString And SecondExtraction Then updateUrl = False : __url = e.Value({"embed"}, "thumb")
                                            If Not __url.IsEmptyString Then createMedia(__url, UTypes.Picture)
                                        Next
                                    End With
                                End If

                                If Not .Value("playlist").IsEmptyString Then createMedia(.Value("playlist"), UTypes.m3u8)

                                If If(.Item("external")?.Count, 0) > 0 Then createMedia(.Value({"external"}, "uri"), UTypes.GIF)

                            End If
                        End With

                        If c > 0 Then Exit For
                    Next
                End If
            End With
            Return c
        End Function
#End Region
#Region "GetProfileInfo"
        Private Sub GetProfileInfo(ByVal Token As CancellationToken)
            Try
                If UpdateToken() Then
                    Dim r$ = Responser.GetResponse($"https://bsky.social/xrpc/app.bsky.actor.getProfile?actor={ID.IfNullOrEmpty(NameTrue)}")
                    TokenUpdateCountReset()
                    If Not r.IsEmptyString Then
                        Using j As EContainer = JsonDocument.Parse(r)
                            If j.ListExists Then
                                ID = j.Value("did")
                                UserSiteNameUpdate(j.Value("displayName"))
                                UserDescriptionUpdate(j.Value("description"))
                                NameTrue = j.Value("handle")
                                SimpleDownloadAvatar(j.Value("avatar"))
                                SimpleDownloadAvatar(j.Value("banner"))
                            End If
                        End Using
                    End If
                Else
                    Throw New ArgumentException("Token is null", "Token")
                End If
            Catch ex As Exception
                ProcessException(ex, Token, "GetProfileInfo")
            End Try
        End Sub
#End Region
#Region "ReparseMissing"
        Protected Overrides Sub ReparseMissing(ByVal Token As CancellationToken)
            Const uriPattern$ = "at://{0}/app.bsky.feed.post/{1}"
            Dim rList As New List(Of Integer)
            Try
                If ContentMissingExists AndAlso UpdateToken() Then
                    Dim r$, url$, uri$
                    Dim tu As Byte
                    Dim m As UserMedia
                    Dim j As EContainer
                    For i% = 0 To _ContentList.Count - 1
                        m = _ContentList(i)
                        If m.State = UStates.Missing Then
                            uri = SymbolsConverter.ASCII.EncodeSymbolsOnly(String.Format(uriPattern, NameTrue, m.Post.ID))
                            url = $"https://bsky.social/xrpc/app.bsky.feed.getPostThread?uri={uri}&depth=10"
                            For tu = 0 To 1
                                Try
                                    Responser.ResetStatus()
                                    r = Responser.GetResponse(url)
                                    TokenUpdateCountReset()
                                    If Not r.IsEmptyString Then
                                        j = JsonDocument.Parse(r)
                                        If j.ListExists Then
                                            If DefaultParser(j({"thread", "post"}), False,, False, UStates.Missing) > 0 Then rList.Add(i)
                                            j.Dispose()
                                        End If
                                    End If
                                    Exit For
                                Catch eex As Exception
                                    If ProcessException(eex, Token, $"ReparseMissing({url})",,, False) <> 1 Then Throw eex
                                End Try
                            Next
                        End If
                    Next
                Else
                    Throw New ArgumentException("Token is null", "Token")
                End If
            Catch ex As Exception
                ProcessException(ex, Token, "ReparseMissing error")
            Finally
                If rList.Count > 0 Then
                    For i% = rList.Count - 1 To 0 Step -1 : _ContentList.RemoveAt(rList(i)) : Next
                    rList.Clear()
                End If
            End Try
        End Sub
#End Region
#Region "CreateFileFromUrl"
        Protected Overloads Overrides Function CreateFileFromUrl(ByVal URL As String) As SFile
            Return CreateFileFromUrl(URL, UTypes.Undefined)
        End Function
        Protected Overloads Function CreateFileFromUrl(ByVal URL As String, ByVal Type As UTypes) As SFile
            Dim f As SFile = MyBase.CreateFileFromUrl(URL)
            Dim force As Boolean = False
            f.Separator = "\"
            With URL.Split("/")
                If .ListExists Then
                    With DirectCast(RegexReplace(.Last, RegEx_FilePattern), List(Of String))
                        If .ListExists(4) Then
                            f.Name = .Item(1).IfNullOrEmpty(f.Name)
                            f.Extension = .Item(3)
                        End If
                    End With
                End If
            End With
            If Not f.Extension.IsEmptyString AndAlso f.Extension.ToLower = "m3u8" Then force = True : Type = UTypes.m3u8
            If f.Extension.IsEmptyString Or force Then
                Select Case Type
                    Case UTypes.Picture : f.Extension = "jpg"
                    Case UTypes.GIF : f.Extension = "gif"
                    Case UTypes.m3u8 : f.Name = "Video" : f.Extension = "mp4"
                End Select
            End If
            Return f
        End Function
#End Region
#Region "DownloadContent"
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
        Protected Overrides Function DownloadM3U8(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile, ByVal Token As CancellationToken) As SFile
            Return M3U8.Download(URL, DestinationFile, Token, Progress, Not IsSingleObjectDownload)
        End Function
#End Region
#Region "DownloadSingleObject"
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            _TokenUpdateCount = 0
            UpdateToken()
            Dim l As List(Of String) = RegexReplace(Data.URL, RegEx_SinglePostPattern)
            If l.ListExists(3) Then
                NameTrue = l(1)
                _ContentList.Add(New UserMedia(Data.URL) With {.State = UStates.Missing, .Post = l(2)})
                ReparseMissing(Token)
            End If
            MyBase.DownloadSingleObject_GetPosts(Data, Token)
        End Sub
#End Region
#Region "Exception"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            If Responser.StatusCode = Net.HttpStatusCode.BadRequest Then '400
                If _TokenUpdateCount = 0 AndAlso UpdateToken(True) Then
                    Return 1
                Else
                    Return 0
                End If
            Else
                Return 0
            End If
        End Function
#End Region
    End Class
End Namespace