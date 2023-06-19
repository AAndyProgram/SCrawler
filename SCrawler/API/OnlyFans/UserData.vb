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
#Region "Declarations"
        Friend Property CCookie As CookieKeeper = Nothing
        Private Const HeaderSign As String = "Sign"
        Private Const HeaderTime As String = "Time"
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return HOST.Source
            End Get
        End Property
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
        End Sub
#End Region
#Region "Download functions"
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            If Not CCookie Is Nothing Then CCookie.Dispose()
            CCookie = Responser.Cookies.Copy
            Responser.Cookies.Clear()
            AddHandler Responser.ResponseReceived, AddressOf OnResponseReceived
            UpdateCookieHeader()
            DownloadData(IIf(IsSavedPosts, 0, String.Empty), Token)
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
        Private Const BaseUrlPattern As String = "https://onlyfans.com{0}"
        Private Overloads Sub DownloadData(ByVal Cursor As String, ByVal Token As CancellationToken)

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
                        DownloadData(tmpCursor, Token)
                    End If
                Catch ex As Exception
                    If ProcessException(ex, Token, $"data downloading error [{url}]") = 2 Then _complete = False
                End Try
            Loop While Not _complete
        End Sub
        Private Function TryCreateMedia(ByVal n As EContainer, ByVal PostID As String, Optional ByVal PostDate As String = Nothing,
                                        Optional ByRef Result As Boolean = False) As List(Of UserMedia)
            Dim postUrl$, ext$
            Dim t As UTypes
            Dim mList As New List(Of UserMedia)
            Result = False
            With n("media")
                If .ListExists Then
                    For Each m In .Self
                        postUrl = m.Value({"source"}, "source").IfNullOrEmpty(m.Value("full"))
                        Select Case m.Value("type")
                            Case "photo" : t = UTypes.Picture : ext = "jpg"
                            Case "video" : t = UTypes.Video : ext = "mp4"
                            Case Else : t = UTypes.Undefined : ext = String.Empty
                        End Select
                        If Not t = UTypes.Undefined And Not postUrl.IsEmptyString Then
                            Dim media As New UserMedia(postUrl, t) With {
                                        .Post = New UserPost(PostID, AConvert(Of Date)(PostDate, DateProvider, Nothing))}
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
                                UserDescriptionUpdate(j.Value("about"))
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
                                                _TempMediaList.ListAddList(mList, LNC)
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
                    For i% = rList.Count - 1 To 0 Step -1 : _ContentList.RemoveAt(i) : Next
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
        Private Function UpdateSignature(ByVal Path As String, Optional ByVal ForceUpdateAuth As Boolean = False) As Boolean
            Try
                If UpdateAuthFile(ForceUpdateAuth) Then
                    Const nullMsg$ = "The auth parameter is null"
                    Dim j As EContainer = JsonDocument.Parse(AuthFile.GetText)
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
                Else
                    Return False
                End If
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
            If Responser.StatusCode = Net.HttpStatusCode.BadRequest Then
                If Not _DownloadingException_AuthFileUpdate AndAlso UpdateAuthFile(True) Then
                    _DownloadingException_AuthFileUpdate = True
                    Return 2
                Else
                    MySettings.SessionAborted = True
                    MyMainLOG = $"{ToStringForLog()}: OnlyFans credentials expired"
                    Return 1
                End If
            ElseIf Responser.StatusCode = Net.HttpStatusCode.NotFound Then
                UserExists = False
                Return 1
            Else
                Return 0
            End If
        End Function
#End Region
#Region "IDisposable Support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then CCookie.DisposeIfReady(False) : CCookie = Nothing
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace