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
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Clients.EventArguments
Imports PersonalUtilities.Tools.Web.Cookies
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Imports UStates = SCrawler.API.Base.UserMedia.States
Namespace API.OnlyFans
    Friend Class UserData : Inherits UserDataBase
#Region "XML names"
        Private Const Name_MediaDownloadHighlights As String = "DownloadHighlights"
        Private Const Name_MediaDownloadChatMedia As String = "DownloadChatMedia"
#End Region
#Region "Declarations"
        Friend Property CCookie As CookieKeeper = Nothing
        Private Const HeaderSign As String = "Sign"
        Private Const HeaderTime As String = "Time"
        Private ReadOnly HighlightsList As List(Of String)
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
                    MediaDownloadHighlights = .Value(Name_MediaDownloadHighlights).FromXML(Of Boolean)(True)
                    MediaDownloadChatMedia = .Value(Name_MediaDownloadChatMedia).FromXML(Of Boolean)(True)
                Else
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
                    MediaDownloadHighlights = .DownloadHighlights
                    MediaDownloadChatMedia = .DownloadChatMedia
                End With
            End If
        End Sub
#End Region
#Region "Initializer"
        Friend Sub New()
            HighlightsList = New List(Of String)
        End Sub
#End Region
#Region "Download functions"
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            If Not MySettings.SessionAborted Then
                If Not CCookie Is Nothing Then CCookie.Dispose()
                CCookie = Responser.Cookies.Copy
                Responser.Cookies.Clear()
                AddHandler Responser.ResponseReceived, AddressOf OnResponseReceived
                UpdateCookieHeader()
                DownloadTimeline(IIf(IsSavedPosts, 0, String.Empty), Token)
                If Not IsSavedPosts Then
                    If MediaDownloadHighlights Then DownloadHighlights(Token)
                    If MediaDownloadChatMedia Then DownloadChatMedia(0, Token)
                End If
            End If
        End Sub
        Private Sub OnResponseReceived(ByVal Sender As Object, ByVal e As WebDataResponse)
            If e.CookiesExists Then
                CCookie.Update(e.Cookies, CookieKeeper.UpdateModes.ReplaceByNameAll,, EDP.ReturnValue)
                UpdateCookieHeader()
            End If
        End Sub
        Private Sub UpdateCookieHeader()
            Responser.Headers.Add("Cookie", CCookie.ToString(False))
        End Sub
        Friend Const A_HIGHLIGHT As String = "HL"
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
                        If ID.IsEmptyString Then GetUserID()
                        If ID.IsEmptyString Then Throw New ArgumentNullException("ID", "Unable to get user ID")

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
                    _complete = Not ProcessException(ex, Token, $"data downloading error [{url}]") = 2
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
                    _complete = Not ProcessException(ex, Token, $"highlights downloading error [{url}]") = 2
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
                                    specFolder = j.Value("title").StringRemoveWinForbiddenSymbols.IfNullOrEmpty(HLID)
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
                    _complete = Not ProcessException(ex, Token, $"highlights downloading error [{url}]") = 2
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
                    _complete = Not ProcessException(ex, Token, $"chats downloading error [{url}]") = 2
                End Try
            Loop While Not _complete
        End Sub
#End Region
        Private Function TryCreateMedia(ByVal n As EContainer, ByVal PostID As String, Optional ByVal PostDate As String = Nothing,
                                        Optional ByRef Result As Boolean = False, Optional ByVal IsHL As Boolean = False,
                                        Optional ByVal SpecFolder As String = Nothing) As List(Of UserMedia)
            Dim postUrl$, ext$
            Dim t As UTypes
            Dim mList As New List(Of UserMedia)
            Result = False
            With n("media")
                If .ListExists Then
                    For Each m In .Self
                        If IsHL Then
                            postUrl = m.Value({"files", "source"}, "url")
                        Else
                            postUrl = m.Value({"source"}, "source").IfNullOrEmpty(m.Value("full"))
                        End If
                        Select Case m.Value("type")
                            Case "photo" : t = UTypes.Picture : ext = "jpg"
                            Case "video" : t = UTypes.Video : ext = "mp4"
                            Case Else : t = UTypes.Undefined : ext = String.Empty
                        End Select
                        If Not t = UTypes.Undefined And Not postUrl.IsEmptyString Then
                            Dim media As New UserMedia(postUrl, t) With {
                                .Post = New UserPost(PostID, AConvert(Of Date)(PostDate, DateProvider, Nothing)),
                                .SpecialFolder = SpecFolder
                            }
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
                                Dim a As Action(Of String) = Sub(ByVal address As String)
                                                                 If Not address.IsEmptyString Then
                                                                     Dim f As SFile = address
                                                                     f.Separator = "\"
                                                                     f.Path = DownloadContentDefault_GetRootDir()
                                                                     If Not f.Exists Then GetWebFile(address, f, EDP.None)
                                                                 End If
                                                             End Sub
                                a.Invoke(j.Value("avatar"))
                                a.Invoke(j.Value("header"))
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
                    Dim r$, path$, postDate$
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
                                            mediaResult = False
                                            mList = TryCreateMedia(j, m.Post.ID, postDate, mediaResult)
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
        Private ReadOnly Property AuthFile As SFile
            Get
                Dim f As SFile = MySettings.Responser.File
                f.Name &= "_Auth"
                f.Extension = "json"
                Return f
            End Get
        End Property
        Private Function UpdateSignature(ByVal Path As String, Optional ByVal ForceUpdateAuth As Boolean = False,
                                         Optional ByVal Round As Integer = 0) As Boolean
            Try
                If UpdateAuthFile(ForceUpdateAuth) Then
                    Const nullMsg$ = "The auth parameter is null"
                    Dim j As EContainer
                    Try
                        j = JsonDocument.Parse(AuthFile.GetText)
                    Catch jex As Exception
                        If Round = 0 Then
                            AuthFile.Delete()
                            UpdateAuthFile(True)
                            Return UpdateSignature(Path, ForceUpdateAuth, Round + 1)
                        Else
                            MySettings.SessionAborted = True
                            Return False
                        End If
                    End Try
                    If Not j Is Nothing Then
                        Dim pattern$ = j.Value("format")
                        If pattern.IsEmptyString Then Throw New ArgumentNullException("format", nullMsg)
                        pattern = pattern.Replace("{}", "{0}").Replace("{:x}", "{1:x}")

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

                        j.Dispose()
                        Return True
                    End If
                End If
                Return False
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, $"{ToStringForLog()}: UpdateSignature", False)
            End Try
        End Function
        Private Function UpdateAuthFile(ByVal Force As Boolean) As Boolean
            Const urlOld$ = "https://raw.githubusercontent.com/DATAHOARDERS/dynamic-rules/main/onlyfans.json"
            Const urlNew$ = "https://raw.githubusercontent.com/DIGITALCRIMINALS/dynamic-rules/main/onlyfans.json"
            Try
                If MySettings.LastDateUpdated.AddMinutes(CInt(MySettings.DynamicRulesUpdateInterval.Value)) < Now Or Not AuthFile.Exists Or Force Then
                    Dim r$ = GetWebString(If(ACheck(Of String)(MySettings.DynamicRules.Value),
                                             CStr(MySettings.DynamicRules.Value),
                                             IIf(MySettings.UseOldAuthRules.Value, urlOld, urlNew)),, EDP.ReturnValue)
                    If Not r.IsEmptyString Then
                        Using j As EContainer = JsonDocument.Parse(r, EDP.ReturnValue)
                            If j.ListExists Then
                                If Not j.Value("format").IsEmptyString And j("checksum_indexes").ListExists And
                                   Not j.Value("static_param").IsEmptyString And Not j.Value("checksum_constant").IsEmptyString Then _
                                   TextSaver.SaveTextToFile(r, AuthFile, True, False, EDP.ThrowException) : MySettings.LastDateUpdated = Now
                            End If
                        End Using
                    End If
                End If
                Return AuthFile.Exists
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, $"{ToStringForLog()}: UpdateAuthFile", False)
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
#Region "DownloadContent"
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
#End Region
#Region "DownloadingException"
        Private _DownloadingException_AuthFileUpdate As Boolean = False
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            If Responser.StatusCode = Net.HttpStatusCode.BadRequest Then '400
                If Not _DownloadingException_AuthFileUpdate AndAlso UpdateAuthFile(True) Then
                    _DownloadingException_AuthFileUpdate = True
                    Return 2
                Else
                    MySettings.SessionAborted = True
                    MyMainLOG = $"{ToStringForLog()}: OnlyFans credentials expired"
                    Return 1
                End If
            ElseIf Responser.StatusCode = Net.HttpStatusCode.NotFound Then '404
                UserExists = False
                Return 1
            ElseIf Responser.StatusCode = Net.HttpStatusCode.GatewayTimeout Or Responser.StatusCode = 429 Then '504, 429
                If Responser.StatusCode = 429 Then MyMainLOG = $"[429] OnlyFans too many requests ({ToStringForLog()})"
                MySettings.SessionAborted = True
                Return 1
            ElseIf Responser.StatusCode = Net.HttpStatusCode.Unauthorized Then '401
                MySettings.SessionAborted = True
                MyMainLOG = $"{ToStringForLog()}: OnlyFans credentials expired"
                Return 1
            Else
                Return 0
            End If
        End Function
#End Region
#Region "IDisposable Support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then CCookie.DisposeIfReady(False) : CCookie = Nothing : HighlightsList.Clear()
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace