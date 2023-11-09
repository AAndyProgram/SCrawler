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
Imports SCrawler.API.YouTube.Objects
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Imports UStates = SCrawler.API.Base.UserMedia.States
Namespace API.RedGifs
    Friend Class UserData : Inherits UserDataBase
        Friend Const DataGone As HttpStatusCode = HttpStatusCode.Gone
        Private Const PostDataUrl As String = "https://api.redgifs.com/v2/gifs/{0}?views=yes&users=yes"
#Region "Base declarations"
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return DirectCast(HOST.Source, SiteSettings)
            End Get
        End Property
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
        End Sub
#End Region
#Region "Initializer"
        Friend Sub New()
            UseResponserClient = True
        End Sub
#End Region
#Region "Download functions"
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            DownloadData(1, Token)
        End Sub
        Private Overloads Sub DownloadData(ByVal Page As Integer, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                Dim _page As Func(Of String) = Function() If(Page = 1, String.Empty, $"&page={Page}")
                URL = $"https://api.redgifs.com/v2/users/{Name}/search?order=recent{_page.Invoke}"
                Dim r$ = Responser.GetResponse(URL)
                Dim postDate$, postID$
                Dim pTotal% = 0
                If Not r.IsEmptyString Then
                    Using j As EContainer = JsonDocument.Parse(r).XmlIfNothing
                        If j.Contains("gifs") Then
                            pTotal = j.Value("pages").FromXML(Of Integer)(0)
                            ProgressPre.ChangeMax(j("gifs").Count)
                            For Each g As EContainer In j("gifs")
                                ProgressPre.Perform()
                                postDate = g.Value("createDate")
                                Select Case CheckDatesLimit(postDate, UnixDate32Provider)
                                    Case DateResult.Skip : Continue For
                                    Case DateResult.Exit : Exit Sub
                                End Select
                                postID = g.Value("id")
                                If Not _TempPostsList.Contains(postID) Then _TempPostsList.Add(postID) Else Exit Sub
                                ObtainMedia(g, postID, postDate)
                            Next
                        End If
                    End Using
                End If
                If pTotal > 0 And Page < pTotal Then DownloadData(Page + 1, Token)
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]")
            End Try
        End Sub
#End Region
#Region "Media obtain, extract"
        Private Sub ObtainMedia(ByVal j As EContainer, ByVal PostID As String,
                                Optional ByVal PostDateStr As String = Nothing, Optional ByVal PostDateDate As Date? = Nothing,
                                Optional ByVal State As UStates = UStates.Unknown, Optional ByVal Attempts As Integer = 0)
            Dim tMedia As UserMedia = ExtractMedia(j)
            If Not tMedia.Type = UTypes.Undefined Then _
               _TempMediaList.ListAddValue(MediaFromData(tMedia.Type, tMedia.URL, PostID, PostDateStr, PostDateDate, State, Attempts))
        End Sub
        Private Shared Function ExtractMedia(ByVal j As EContainer) As UserMedia
            If Not j Is Nothing Then
                With j("urls")
                    If .ListExists Then
                        Dim u$ = .Value("hd").IfNullOrEmpty(.Value("sd"))
                        If Not u.IsEmptyString Then
                            Dim ut As UTypes = UTypes.Undefined
                            'Type 1: video
                            'Type 2: image
                            Select Case j.Value("type").FromXML(Of Integer)(0)
                                Case 1 : ut = UTypes.Video
                                Case 2 : ut = UTypes.Picture
                            End Select
                            Return New UserMedia(u, ut)
                        End If
                    End If
                End With
            End If
            Return Nothing
        End Function
#End Region
#Region "ReparseMissing"
        Protected Overrides Sub ReparseMissing(ByVal Token As CancellationToken)
            Dim rList As New List(Of Integer)
            Try
                If ContentMissingExists Then
                    Dim url$, r$
                    Dim u As UserMedia
                    Dim j As EContainer
                    ProgressPre.ChangeMax(_ContentList.Count)
                    For i% = 0 To _ContentList.Count - 1
                        ProgressPre.Perform()
                        If _ContentList(i).State = UStates.Missing Then
                            ThrowAny(Token)
                            u = _ContentList(i)
                            If Not u.Post.ID.IsEmptyString Then
                                url = String.Format(PostDataUrl, u.Post.ID.ToLower)
                                Try
                                    r = Responser.GetResponse(url)
                                    If Not r.IsEmptyString Then
                                        j = JsonDocument.Parse(r)
                                        If Not j Is Nothing Then
                                            If If(j("gif")?.Count, 0) > 0 Then
                                                ObtainMedia(j("gif"), u.Post.ID,, u.Post.Date, UStates.Missing, u.Attempts)
                                                rList.Add(i)
                                            End If
                                        End If
                                    End If
                                Catch down_ex As Exception
                                    u.Attempts += 1
                                    _ContentList(i) = u
                                End Try
                            Else
                                rList.Add(i)
                            End If
                        End If
                    Next
                End If
            Catch dex As ObjectDisposedException When Disposed
            Catch ex As Exception
                ProcessException(ex, Token, $"missing data downloading error",, False)
            Finally
                If Not Disposed And rList.Count > 0 Then
                    For i% = rList.Count - 1 To 0 Step -1 : _ContentList.RemoveAt(rList(i)) : Next
                End If
            End Try
        End Sub
#End Region
#Region "Downloader"
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
#End Region
#Region "Get post data statics"
        ''' <summary>
        ''' https://thumbs4.redgifs.com/abcde-large.jpg?expires -> abcde<br/>
        ''' https://thumbs4.redgifs.com/abcde.mp4?expires -> abcde<br/>
        ''' https://www.redgifs.com/watch/abcde?rel=a -> abcde
        ''' </summary>
        Friend Shared Function GetVideoIdFromUrl(ByVal URL As String) As String
            If Not URL.IsEmptyString Then
                Return RegexReplace(URL, If(URL.Contains("/watch/"), WatchIDRegex, ThumbsIDRegex))
            Else
                Return String.Empty
            End If
        End Function
        Friend Shared Function GetDataFromUrlId(ByVal Obj As String, ByVal ObjIsID As Boolean, ByVal Responser As Responser,
                                                ByVal Host As Plugin.Hosts.SettingsHost, ByVal AccountName As String) As UserMedia
            Dim URL$ = String.Empty
            Try
                If Obj.IsEmptyString Then Return Nothing
                If Not ObjIsID Then
                    Obj = GetVideoIdFromUrl(Obj)
                    If Not Obj.IsEmptyString Then Return GetDataFromUrlId(Obj, True, Responser, Host, AccountName)
                Else
                    If Host Is Nothing Then
                        Host = Settings(RedGifsSiteKey, AccountName)
                        If Host Is Nothing Then Host = Settings(RedGifsSiteKey).Default
                    End If
                    If Not Host Is Nothing AndAlso Host.Source.Available(Plugin.ISiteSettings.Download.Main, True) Then
                        If Responser Is Nothing Then Responser = Host.Responser.Copy
                        URL = String.Format(PostDataUrl, Obj.ToLower)
                        Dim r$ = Responser.GetResponse(URL,, EDP.ThrowException)
                        If Not r.IsEmptyString Then
                            Using j As EContainer = JsonDocument.Parse(r)
                                If Not j Is Nothing Then
                                    Dim tm As UserMedia = ExtractMedia(j("gif"))
                                    tm.Post.ID = Obj
                                    tm.File = CStr(RegexReplace(tm.URL, FilesPattern))
                                    If tm.File.IsEmptyString Then
                                        tm.File.Name = Obj
                                        Select Case tm.Type
                                            Case UTypes.Picture : tm.File.Extension = "jpg"
                                            Case UTypes.Video : tm.File.Extension = "mp4"
                                        End Select
                                    End If
                                    Return tm
                                End If
                            End Using
                        End If
                    Else
                        Return New UserMedia With {.State = UStates.Missing}
                    End If
                End If
                Return Nothing
            Catch ex As Exception
                If Not Responser Is Nothing AndAlso (Responser.Client.StatusCode = DataGone Or Responser.Client.StatusCode = HttpStatusCode.NotFound) Then
                    Return New UserMedia With {.State = DataGone}
                Else
                    Dim m As New UserMedia With {.State = UStates.Missing}
                    Dim _errText$ = "API.RedGifs.UserData.GetDataFromUrlId({0})"
                    If Responser.Client.StatusCode = HttpStatusCode.Unauthorized Then
                        _errText = $"RedGifs credentials have expired [{CInt(Responser.Client.StatusCode)}]: {_errText}"
                        MyMainLOG = String.Format(_errText, URL)
                        Return m
                    Else
                        Return ErrorsDescriber.Execute(EDP.SendToLog, ex, String.Format(_errText, URL), m)
                    End If
                End If
            End Try
        End Function
#End Region
#Region "Single data downloader"
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            Dim m As UserMedia = GetDataFromUrlId(Data.URL, False, Responser, HOST, AccountName)
            If Not m.State = UStates.Missing And Not m.State = DataGone And (m.Type = UTypes.Picture Or m.Type = UTypes.Video) Then
                m.URL_BASE = MySettings.GetUserPostUrl(Me, m)
                _TempMediaList.Add(m)
            End If
        End Sub
#End Region
#Region "Create media"
        Private Function MediaFromData(ByVal t As UTypes, ByVal _URL As String, ByVal PostID As String,
                                       ByVal PostDateStr As String, ByVal PostDateDate As Date?, ByVal State As UStates, Optional ByVal Attempts As Integer = 0) As UserMedia
            _URL = LinkFormatterSecure(RegexReplace(_URL.Replace("\", String.Empty), LinkPattern))
            Dim m As New UserMedia(_URL, t) With {.Post = New UserPost With {.ID = PostID}}
            If Not m.URL.IsEmptyString Then m.File = CStr(RegexReplace(m.URL, FilesPattern))
            If Not PostDateStr.IsEmptyString Then
                m.Post.Date = AConvert(Of Date)(PostDateStr, UnixDate32Provider, Nothing)
            ElseIf PostDateDate.HasValue Then
                m.Post.Date = PostDateDate
            Else
                m.Post.Date = Nothing
            End If
            m.State = State
            m.Attempts = Attempts
            Return m
        End Function
#End Region
#Region "Exception"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            Dim s As WebExceptionStatus = Responser.Status
            Dim sc As HttpStatusCode = Responser.StatusCode
            If sc = HttpStatusCode.NotFound Or s = DataGone Or sc = DataGone Then
                UserExists = False
            ElseIf sc = HttpStatusCode.Unauthorized Then
                MyMainLOG = $"RedGifs credentials have expired [{CInt(sc)}]: {ToStringForLog()}"
            Else
                If Not FromPE Then LogError(ex, Message) : HasError = True
                Return 0
            End If
            Return 1
        End Function
#End Region
    End Class
End Namespace