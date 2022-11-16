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
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports UStates = SCrawler.API.Base.UserMedia.States
Namespace API.Twitter
    Friend Class UserData : Inherits UserDataBase
        Private Const SinglePostUrl As String = "https://api.twitter.com/1.1/statuses/show.json?id={0}&tweet_mode=extended"
#Region "Declarations"
        Private ReadOnly _DataNames As List(Of String)
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
        End Sub
#End Region
#Region "Initializer"
        Friend Sub New()
            _DataNames = New List(Of String)
        End Sub
#End Region
#Region "Download functions"
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            If IsSavedPosts Then
                If _ContentList.Count > 0 Then _DataNames.ListAddList(_ContentList.Select(Function(c) c.Post.ID), LAP.ClearBeforeAdd, LAP.NotContainsOnly)
                DownloadData(String.Empty, Token)
            Else
                If _ContentList.Count > 0 Then _DataNames.ListAddList(_ContentList.Select(Function(c) c.File.File), LAP.ClearBeforeAdd, LAP.NotContainsOnly)
                DownloadData(String.Empty, Token)
            End If
        End Sub
        Private Overloads Sub DownloadData(ByVal POST As String, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                Dim NextCursor$ = String.Empty
                Dim __NextCursor As Predicate(Of EContainer) = Function(e) e.Value({"content", "operation", "cursor"}, "cursorType") = "Bottom"
                Dim PostID$ = String.Empty
                Dim PostDate$
                Dim nn As EContainer, s As EContainer
                Dim NewPostDetected As Boolean = False
                Dim ExistsDetected As Boolean = False

                Dim UID As Func(Of EContainer, String) = Function(e) e.XmlIfNothing.Item({"user", "id"}).XmlIfNothingValue

                If IsSavedPosts Then
                    If Name.IsEmptyString Then Throw New ArgumentNullException With {.HelpLink = 1}
                    URL = $"https://api.twitter.com/2/timeline/bookmark.json?screen_name={Name}&count=200" &
                           "&tweet_mode=extended&include_entities=true&include_user_entities=true&include_ext_media_availability=true"
                    If Not POST.IsEmptyString Then URL &= $"&cursor={SymbolsConverter.ASCII.EncodeSymbolsOnly(POST)}"
                Else
                    URL = $"https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={Name}&count=200&exclude_replies=false&include_rts=1&tweet_mode=extended"
                    If Not POST.IsEmptyString Then URL &= $"&max_id={POST}"
                End If

                ThrowAny(Token)
                Dim r$ = Responser.GetResponse(URL,, EDP.ThrowException)
                If Not r.IsEmptyString Then
                    Using w As EContainer = JsonDocument.Parse(r)
                        If w.ListExists Then
                            For Each nn In If(IsSavedPosts, w({"globalObjects", "tweets"}).XmlIfNothing, w)
                                ThrowAny(Token)
                                If nn.Count > 0 Then
                                    If IsSavedPosts Then
                                        PostID = nn.Value
                                        If PostID.IsEmptyString Then PostID = nn.Value("id_str")
                                    Else
                                        PostID = nn.Value("id")
                                        If ID.IsEmptyString Then
                                            ID = UID(nn)
                                            If Not ID.IsEmptyString Then UpdateUserInformation()
                                        End If
                                    End If

                                    If Not IsSavedPosts AndAlso UserDescriptionNeedToUpdate() AndAlso nn.Value({"user"}, "screen_name") = Name Then _
                                       UserDescriptionUpdate(nn.Value({"user"}, "description"))

                                    'Date Pattern:
                                    'Sat Jan 01 01:10:15 +0000 2000
                                    If nn.Contains("created_at") Then PostDate = nn("created_at").Value Else PostDate = String.Empty
                                    Select Case CheckDatesLimit(PostDate, Declarations.DateProvider)
                                        Case DateResult.Skip : Continue For
                                        Case DateResult.Exit : Exit Sub
                                    End Select

                                    If Not _TempPostsList.Contains(PostID) Then
                                        NewPostDetected = True
                                        _TempPostsList.Add(PostID)
                                    Else
                                        ExistsDetected = True
                                        Continue For
                                    End If

                                    If IsSavedPosts OrElse Not ParseUserMediaOnly OrElse
                                        (
                                            Not nn.Contains("retweeted_status") OrElse
                                            (Not ID.IsEmptyString AndAlso UID(nn("retweeted_status")) = ID)
                                        ) Then ObtainMedia(nn, PostID, PostDate)
                                End If
                            Next

                            If IsSavedPosts Then
                                s = w.ItemF({"timeline", "instructions", 0, "addEntries", "entries"}).XmlIfNothing
                                If s.Count > 0 Then NextCursor = If(s.ItemF({__NextCursor})?.Value({"content", "operation", "cursor"}, "value"), String.Empty)
                            End If
                        End If
                    End Using

                    If IsSavedPosts Then
                        If Not NextCursor.IsEmptyString And Not NextCursor = POST Then DownloadData(NextCursor, Token)
                    Else
                        If POST.IsEmptyString And ExistsDetected Then Exit Sub
                        If Not PostID.IsEmptyString And NewPostDetected Then DownloadData(PostID, Token)
                    End If
                End If
            Catch ane As ArgumentNullException When ane.HelpLink = 1
                MyMainLOG = "Username not set for saved Twitter posts"
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error{IIf(IsSavedPosts, " (Saved Posts)", String.Empty)} [{URL}]")
            End Try
        End Sub
#End Region
#Region "Obtain media"
        Private Sub ObtainMedia(ByVal e As EContainer, ByVal PostID As String, ByVal PostDate As String, Optional ByVal State As UStates = UStates.Unknown)
            If Not CheckVideoNode(e, PostID, PostDate, State) Then
                Dim s As EContainer = e.ItemF({"extended_entities", "media"})
                If s Is Nothing OrElse s.Count = 0 Then s = e.ItemF({"retweeted_status", "extended_entities", "media"})
                If If(s?.Count, 0) > 0 Then
                    For Each m In s
                        If m.Contains("media_url") Then
                            Dim dName$ = UrlFile(m("media_url").Value)
                            If Not dName.IsEmptyString AndAlso Not _DataNames.Contains(dName) Then
                                _DataNames.Add(dName)
                                _TempMediaList.ListAddValue(MediaFromData(m("media_url").Value,
                                                                          PostID, PostDate, GetPictureOption(m), State), LNC)
                            End If
                        End If
                    Next
                End If
            End If
        End Sub
        Private Function CheckVideoNode(ByVal w As EContainer, ByVal PostID As String, ByVal PostDate As String,
                                        Optional ByVal State As UStates = UStates.Unknown) As Boolean
            Try
                If CheckForGif(w, PostID, PostDate, State) Then Return True
                Dim URL$ = GetVideoNodeURL(w)
                If Not URL.IsEmptyString Then
                    Dim f$ = UrlFile(URL)
                    If Not f.IsEmptyString AndAlso Not _DataNames.Contains(f) Then
                        _DataNames.Add(f)
                        _TempMediaList.ListAddValue(MediaFromData(URL, PostID, PostDate,, State), LNC)
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
                                     Optional ByVal State As UStates = UStates.Unknown) As Boolean
            Try
                Dim gifUrl As Predicate(Of EContainer) = Function(e) Not e.Value("content_type").IsEmptyString AndAlso
                                                                     e.Value("content_type").Contains("mp4") AndAlso
                                                                     Not e.Value("url").IsEmptyString
                Dim url$, ff$
                Dim f As SFile
                Dim m As UserMedia
                With w({"extended_entities", "media"})
                    If .ListExists Then
                        For Each n As EContainer In .Self
                            If n.Value("type") = "animated_gif" Then
                                With n({"video_info", "variants"}).XmlIfNothing.ItemF({gifUrl}).XmlIfNothing
                                    url = .Value("url")
                                    ff = UrlFile(url)
                                    If Not ff.IsEmptyString Then
                                        If Not _DataNames.Contains(ff) Then
                                            m = MediaFromData(url, PostID, PostDate,, State)
                                            f = m.File
                                            If Not f.IsEmptyString Then f.Name = $"GIF_{f.Name}" : m.File = f
                                            _TempMediaList.ListAddValue(m, LNC)
                                        End If
                                        Return True
                                    End If
                                End With
                            End If
                        Next
                    End If
                End With
                Return False
            Catch ex As Exception
                LogError(ex, "[API.Twitter.UserData.CheckForGif]")
                Return False
            End Try
        End Function
        Private Shared Function GetVideoNodeURL(ByVal w As EContainer) As String
            Dim v As EContainer = w.GetNode(VideoNode)
            If v.ListExists Then
                Dim l As New List(Of Sizes)
                Dim u$
                Dim nn As EContainer
                For Each n As EContainer In v
                    If n.Count > 0 Then
                        For Each nn In n
                            If nn("content_type").XmlIfNothingValue("none").Contains("mp4") AndAlso nn.Contains("url") Then
                                u = nn.Value("url")
                                l.Add(New Sizes(RegexReplace(u, VideoSizeRegEx), u))
                            End If
                        Next
                    End If
                Next
                If l.Count > 0 Then l.RemoveAll(Function(s) s.HasError)
                If l.Count > 0 Then l.Sort() : Return l(0).Data
            End If
            Return String.Empty
        End Function
#End Region
#Region "ReparseMissing"
        Protected Overrides Sub ReparseMissing(ByVal Token As CancellationToken)
            Dim rList As New List(Of Integer)
            Dim URL$ = String.Empty
            Try
                If ContentMissingExists Then
                    Dim m As UserMedia
                    Dim r$, PostDate$
                    Dim j As EContainer
                    For i% = 0 To _ContentList.Count - 1
                        If _ContentList(i).State = UStates.Missing Then
                            m = _ContentList(i)
                            If Not m.Post.ID.IsEmptyString Then
                                ThrowAny(Token)
                                URL = String.Format(SinglePostUrl, m.Post.ID)
                                r = Responser.GetResponse(URL,, EDP.ReturnValue)
                                If Not r.IsEmptyString Then
                                    j = JsonDocument.Parse(r)
                                    If Not j Is Nothing Then
                                        PostDate = String.Empty
                                        If j.Contains("created_at") Then PostDate = j("created_at").Value Else PostDate = String.Empty
                                        ObtainMedia(j, m.Post.ID, PostDate, UStates.Missing)
                                        rList.Add(i)
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
#Region "Get video static"
        Friend Shared Function GetVideoInfo(ByVal URL As String, ByVal resp As Response) As IEnumerable(Of UserMedia)
            Try
                If URL.Contains("twitter") Then
                    Dim PostID$ = RegexReplace(URL, RParams.DM("(?<=/)\d+", 0))
                    If Not PostID.IsEmptyString Then
                        Dim r$
                        Using rc As Response = resp.Copy() : r = rc.GetResponse(String.Format(SinglePostUrl, PostID),, EDP.ReturnValue) : End Using
                        If Not r.IsEmptyString Then
                            Using j As EContainer = JsonDocument.Parse(r)
                                If j.ListExists Then
                                    Dim u$ = GetVideoNodeURL(j)
                                    If Not u.IsEmptyString Then Return {MediaFromData(u, PostID, String.Empty)}
                                End If
                            End Using
                        End If
                    End If
                End If
                Return Nothing
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.ShowMainMsg + EDP.SendInLog, ex, $"Twitter standalone downloader: fetch media error ({URL})")
            End Try
        End Function
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
        Private Function UrlFile(ByVal URL As String) As String
            Try
                Dim f As SFile = CStr(RegexReplace(LinkFormatterSecure(RegexReplace(URL.Replace("\", String.Empty), LinkPattern)), FilesPattern))
                If Not f.IsEmptyString Then Return f.File Else Return String.Empty
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function
#End Region
#Region "Create media"
        Private Shared Function MediaFromData(ByVal _URL As String, ByVal PostID As String, ByVal PostDate As String,
                                              Optional ByVal _PictureOption As String = Nothing,
                                              Optional ByVal State As UStates = UStates.Unknown) As UserMedia
            _URL = LinkFormatterSecure(RegexReplace(_URL.Replace("\", String.Empty), LinkPattern))
            Dim m As New UserMedia(_URL) With {.PictureOption = _PictureOption, .Post = New UserPost With {.ID = PostID}}
            If Not m.URL.IsEmptyString Then m.File = CStr(RegexReplace(m.URL, FilesPattern))
            If Not m.PictureOption.IsEmptyString And Not m.File.IsEmptyString And Not m.URL.IsEmptyString Then
                m.URL = $"{m.URL.Replace($".{m.File.Extension}", String.Empty)}?format={m.File.Extension}&name={m.PictureOption}"
            End If
            If Not PostDate.IsEmptyString Then m.Post.Date = AConvert(Of Date)(PostDate, Declarations.DateProvider, Nothing) Else m.Post.Date = Nothing
            m.State = State
            Return m
        End Function
#End Region
#Region "Downloader"
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
#End Region
#Region "Exception"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            With Responser
                If .StatusCode = HttpStatusCode.NotFound Then
                    UserExists = False
                ElseIf .StatusCode = HttpStatusCode.Unauthorized Then
                    UserSuspended = True
                ElseIf .StatusCode = HttpStatusCode.BadRequest Then
                    MyMainLOG = "Twitter has invalid credentials"
                ElseIf .StatusCode = HttpStatusCode.ServiceUnavailable Or .StatusCode = HttpStatusCode.InternalServerError Then
                    MyMainLOG = $"[{CInt(.StatusCode)}] Twitter is currently unavailable ({ToString()})"
                Else
                    If Not FromPE Then LogError(ex, Message) : HasError = True
                    Return 0
                End If
            End With
            Return 1
        End Function
#End Region
#Region "IDisposable support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then _DataNames.Clear()
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace