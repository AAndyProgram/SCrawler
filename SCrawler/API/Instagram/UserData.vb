' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.Messaging
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.WEB
Imports PersonalUtilities.Tools.WebDocuments.JSON
Imports SCrawler.API.Base
Imports System.Threading
Imports System.Net
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace API.Instagram
    Friend Class UserData : Inherits UserDataBase
        Private Const MaxPostsCount As Integer = 200
        Private Const Name_LastCursor As String = "LastCursor"
        Private Const Name_FirstLoadingDone As String = "FirstLoadingDone"
        Private Const Name_GetStories As String = "GetStories"
        Private Const Name_GetTagged As String = "GetTaggedData"
        Private Const Name_TaggedChecked As String = "TaggedChecked"
        Private ReadOnly Property MySiteSettings As SiteSettings
            Get
                Return DirectCast(HOST.Source, SiteSettings)
            End Get
        End Property
        Private ReadOnly _SavedPostsIDs As New List(Of String)
        Private LastCursor As String = String.Empty
        Private FirstLoadingDone As Boolean = False
        Friend Property GetStories As Boolean
        Friend Property GetTaggedData As Boolean
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return New EditorExchangeOptions(HOST.Source) With {.GetStories = GetStories, .GetTagged = GetTaggedData}
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            If Not Obj Is Nothing AndAlso TypeOf Obj Is EditorExchangeOptions Then
                With DirectCast(Obj, EditorExchangeOptions)
                    GetStories = .GetStories
                    GetTaggedData = .GetTagged
                End With
            End If
        End Sub
        Friend Sub New()
        End Sub
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            If Loading Then
                LastCursor = Container.Value(Name_LastCursor)
                FirstLoadingDone = Container.Value(Name_FirstLoadingDone).FromXML(Of Boolean)(False)
                GetStories = Container.Value(Name_GetStories).FromXML(Of Boolean)(CBool(MySiteSettings.GetStories.Value))
                GetTaggedData = Container.Value(Name_GetTagged).FromXML(Of Boolean)(CBool(MySiteSettings.GetTagged.Value))
                TaggedChecked = Container.Value(Name_TaggedChecked).FromXML(Of Boolean)(False)
            Else
                Container.Add(Name_LastCursor, LastCursor)
                Container.Add(Name_FirstLoadingDone, FirstLoadingDone.BoolToInteger)
                Container.Add(Name_GetStories, GetStories.BoolToInteger)
                Container.Add(Name_GetTagged, GetTaggedData.BoolToInteger)
                Container.Add(Name_TaggedChecked, TaggedChecked.BoolToInteger)
            End If
        End Sub
#Region "Download data"
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            Try
                _InstaHash = String.Empty
                HasError = False
                If Not LastCursor.IsEmptyString Then
                    DownloadData(LastCursor, Sections.Timeline, Token)
                    ThrowAny(Token)
                    If Not HasError Then FirstLoadingDone = True
                End If
                If Not HasError Then
                    DownloadData(String.Empty, Sections.Timeline, Token)
                    ThrowAny(Token)
                    If Not HasError Then FirstLoadingDone = True
                End If
                If FirstLoadingDone Then LastCursor = String.Empty
                If IsSavedPosts Then
                    DownloadPosts(Token)
                ElseIf MySiteSettings.StoriesAndTaggedReady Then
                    If GetStories Then DownloadData(String.Empty, Sections.Stories, Token)
                    If GetTaggedData Then DownloadData(String.Empty, Sections.Tagged, Token)
                End If
                If WaitNotificationMode = WNM.SkipTemp Or WaitNotificationMode = WNM.SkipCurrent Then WaitNotificationMode = WNM.Notify
            Catch eex As ExitException
            Catch ex As Exception
                ProcessException(ex, Token, "[API.Instagram.UserData.DownloadDataF", False)
            End Try
        End Sub
        Private _InstaHash As String = String.Empty
        Friend Enum Sections
            Timeline
            Tagged
            Stories
        End Enum
#Region "429 bypass"
        Friend Property RequestsCount As Integer = 0
        Friend Enum WNM As Integer
            Notify = 0
            SkipCurrent = 1
            SkipAll = 2
            SkipTemp = 3
        End Enum
        Friend WaitNotificationMode As WNM = WNM.Notify
        Private Caught429 As Boolean = False
        Private ProgressTempSet As Boolean = False
        Private Const InstAborted As String = "InstAborted"
        Private Function Ready() As Boolean
            With MySiteSettings
                If Not .ReadyForDownload Then
                    If WaitNotificationMode = WNM.Notify Then
                        Dim m As New MMessage("Instagram [too many requests] error." & vbCr &
                                              $"The program suggests waiting {If(.LastApplyingValue, 0)} minutes." & vbCr &
                                              "What do you want to do?", "Waiting for Instagram download...",
                                              {
                                               New MsgBoxButton("Wait") With {.ToolTip = "Wait and ask again when the error is found."},
                                               New MsgBoxButton("Wait (disable current") With {.ToolTip = "Wait and skip future prompts while downloading the current profile."},
                                               New MsgBoxButton("Abort") With {.ToolTip = "Abort operation"},
                                               New MsgBoxButton("Wait (disable all)") With {.ToolTip = "Wait and skip future prompts while downloading the current session."}
                                              },
                                              vbExclamation) With {.ButtonsPerRow = 2, .DefaultButton = 0, .CancelButton = 2}
                        Select Case MsgBoxE(m).Index
                            Case 1 : WaitNotificationMode = WNM.SkipCurrent
                            Case 2 : Throw New OperationCanceledException("Instagram download operation aborted") With {.HelpLink = InstAborted}
                            Case 3 : WaitNotificationMode = WNM.SkipAll
                            Case Else : WaitNotificationMode = WNM.SkipTemp
                        End Select
                    End If
                    If Not ProgressTempSet Then Progress.InformationTemporary = $"Waiting until { .GetWaitDate().ToString(ParsersDataDateProvider)}"
                    ProgressTempSet = True
                    Return False
                Else
                    Return True
                End If
            End With
        End Function
        Private Sub ReconfigureAwaiter()
            If WaitNotificationMode = WNM.SkipTemp Then WaitNotificationMode = WNM.Notify
            If Caught429 Then Caught429 = False
            ProgressTempSet = False
        End Sub
        Private Sub NextRequest(ByVal StartWait As Boolean)
            With MySiteSettings
                If StartWait And RequestsCount > 0 And (RequestsCount Mod .RequestsWaitTimerTaskCount.Value) = 0 Then Thread.Sleep(CInt(.RequestsWaitTimer.Value))
                If RequestsCount >= MaxPostsCount - 5 Then Thread.Sleep(CInt(.SleepTimerOnPostsLimit.Value))
            End With
        End Sub
#End Region
        Private Const StoriesFolder As String = "Stories"
        Private Const TaggedFolder As String = "Tagged"
        Private TaggedChecked As Boolean = False
        Private Overloads Sub DownloadData(ByVal Cursor As String, ByVal Section As Sections, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Dim StoriesList As List(Of String) = Nothing
            Dim StoriesRequested As Boolean = False
            Dim _DownloadComplete As Boolean = False
            LastCursor = Cursor
            Try
                Do While Not _DownloadComplete
                    ThrowAny(Token)
                    If Not Ready() Then Thread.Sleep(10000) : ThrowAny(Token) : Continue Do
                    ReconfigureAwaiter()

                    Try
                        Dim n As EContainer, nn As EContainer, node As EContainer
                        Dim HasNextPage As Boolean = False
                        Dim EndCursor$ = String.Empty
                        Dim PostID$ = String.Empty, PostDate$ = String.Empty, SpecFolder$ = String.Empty
                        Dim TaggedCount%
                        Dim ENode() As Object = Nothing
                        NextRequest(True)

                        'Check environment
                        If Cursor.IsEmptyString And _InstaHash.IsEmptyString Then _
                            _InstaHash = CStr(If(IsSavedPosts, MySiteSettings.HashSavedPosts, MySiteSettings.Hash).Value)
                        AuthNullException.ThrowIfNull(Section, IsSavedPosts, MySiteSettings)
                        If ID.IsEmptyString Then GetUserId()
                        If ID.IsEmptyString Then Throw New ArgumentException("User ID is not detected", "ID")

                        'Create query
                        Select Case Section
                            Case Sections.Timeline
                                Dim vars$ = "{""id"":" & ID & ",""first"":50,""after"":""" & Cursor & """}"
                                vars = SymbolsConverter.ASCII.EncodeSymbolsOnly(vars)
                                URL = $"https://www.instagram.com/graphql/query/?query_hash={_InstaHash}&variables={vars}"
                                ENode = {"data", "user", 0}
                            Case Sections.Tagged
                                URL = $"https://i.instagram.com/api/v1/usertags/{ID}/feed/?count=50&max_id={Cursor}"
                                ENode = {"items"}
                                SpecFolder = TaggedFolder
                            Case Sections.Stories
                                If Not StoriesRequested Then
                                    StoriesList = GetStoriesList()
                                    MySiteSettings.TooManyRequests(False)
                                    RequestsCount += 1
                                    ThrowAny(Token)
                                End If
                                If StoriesList.ListExists Then
                                    GetStoriesData(StoriesList, Token)
                                    MySiteSettings.TooManyRequests(False)
                                    RequestsCount += 1
                                End If
                                If StoriesList.ListExists Then
                                    Continue Do
                                Else
                                    Throw New ExitException(_DownloadComplete)
                                End If
                        End Select

                        'Get response
                        Dim r$ = Responser.GetResponse(URL,, EDP.ThrowException)
                        MySiteSettings.TooManyRequests(False)
                        RequestsCount += 1
                        ThrowAny(Token)

                        'Data
                        If Not r.IsEmptyString Then
                            Using j As EContainer = JsonDocument.Parse(r).XmlIfNothing
                                n = j.ItemF(ENode).XmlIfNothing
                                If n.Count > 0 Then
                                    Select Case Section
                                        Case Sections.Timeline
                                            If n.Contains("page_info") Then
                                                With n("page_info")
                                                    HasNextPage = .Value("has_next_page").FromXML(Of Boolean)(False)
                                                    EndCursor = .Value("end_cursor")
                                                End With
                                            End If
                                            n = n("edges").XmlIfNothing
                                            If n.Count > 0 Then
                                                For Each nn In n
                                                    ThrowAny(Token)
                                                    node = nn(0).XmlIfNothing
                                                    If IsSavedPosts Then
                                                        PostID = node.Value("shortcode")
                                                        If Not PostID.IsEmptyString Then
                                                            If _TempPostsList.Contains(PostID) Then Throw New ExitException(_DownloadComplete) Else _SavedPostsIDs.Add(PostID)
                                                        End If
                                                    Else
                                                        PostID = node.Value("id")
                                                        If Not PostID.IsEmptyString And _TempPostsList.Contains(PostID) Then Throw New ExitException(_DownloadComplete)
                                                        _TempPostsList.Add(PostID)
                                                        PostDate = node.Value("taken_at_timestamp")
                                                        If Not CheckDatesLimit(PostDate, DateProvider) Then Throw New ExitException(_DownloadComplete)
                                                        ObtainMedia(node, PostID, PostDate, SpecFolder)
                                                    End If
                                                Next
                                            End If
                                        Case Sections.Tagged
                                            HasNextPage = j.Value("more_available").FromXML(Of Boolean)(False)
                                            EndCursor = j.Value("next_max_id")
                                            For Each nn In n
                                                PostID = $"Tagged_{nn.Value("id")}"
                                                If Not PostID.IsEmptyString And _TempPostsList.Contains(PostID) Then Throw New ExitException(_DownloadComplete)
                                                _TempPostsList.Add(PostID)
                                                ObtainMedia2(nn, PostID, SpecFolder)
                                            Next
                                            If Not TaggedChecked Then
                                                TaggedCount = j.Value("total_count").FromXML(Of Integer)(0)
                                                TaggedChecked = True
                                                If TaggedCount > 200 Then
                                                    Dim a% = MsgBoxE({$"The number of tagged posts is {TaggedCount.NumToString(New ANumbers With {
                                                                                                       .FormatOptions = ANumbers.Options.GroupIntegral})}" & vbCr &
                                                                      "The tagged data download operation can take a long time.", "Too much tagged data"}, vbExclamation,,,
                                                                      {"Continue",
                                                                       New MsgBoxButton("Disable and cancel") With {
                                                                        .ToolTip = "Disable downloading tagged data and cancel downloading tagged data."},
                                                                       "Cancel"})
                                                    If a > 0 Then
                                                        If a = 1 Then GetTaggedData = False
                                                        Throw New ExitException(_DownloadComplete)
                                                    End If
                                                End If
                                            End If
                                    End Select
                                Else
                                    If j.Value("status") = "ok" AndAlso j({"data", "user"}).XmlIfNothing.Count = 0 AndAlso _TempMediaList.Count = 0 Then
                                        MySiteSettings.HashUpdateRequired.Value = True
                                        UserExists = False
                                        Throw New ExitException(_DownloadComplete)
                                    End If
                                End If
                            End Using
                        Else
                            Throw New ExitException(_DownloadComplete)
                        End If
                        _DownloadComplete = True
                        If HasNextPage And Not EndCursor.IsEmptyString Then DownloadData(EndCursor, Section, Token)
                    Catch iane As AuthNullException
                        ErrorsDescriber.Execute(EDP.SendInLog, iane)
                        Throw New ExitException(_DownloadComplete)
                    Catch eex As ExitException
                        Throw eex
                    Catch oex As OperationCanceledException When Token.IsCancellationRequested
                        Exit Do
                    Catch dex As ObjectDisposedException When Disposed
                        Exit Do
                    Catch ex As Exception
                        If DownloadingException(ex, $"data downloading error [{URL}]") = 1 Then Continue Do Else Exit Do
                    End Try
                Loop
            Catch eex2 As ExitException
                If (Section = Sections.Timeline Or Section = Sections.Tagged) And Not Cursor.IsEmptyString Then Throw eex2
            Catch oex2 As OperationCanceledException When Token.IsCancellationRequested Or oex2.HelpLink = InstAborted
                If oex2.HelpLink = InstAborted Then HasError = True
            Catch DoEx As Exception
                ProcessException(DoEx, Token, $"data downloading error [{URL}]")
            End Try
        End Sub
        Private Sub DownloadPosts(ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Dim _DownloadComplete As Boolean = False
            Dim _Index% = 0
            Try
                Do While Not _DownloadComplete
                    ThrowAny(Token)
                    If Not Ready() Then Thread.Sleep(10000) : ThrowAny(Token) : Continue Do
                    ReconfigureAwaiter()

                    Try
                        Dim r$
                        Dim j As EContainer, jj As EContainer
                        Dim _MediaObtained As Boolean
                        If _SavedPostsIDs.Count > 0 And _Index <= _SavedPostsIDs.Count - 1 Then
                            Dim e As New ErrorsDescriber(EDP.ThrowException)
                            For i% = _Index To _SavedPostsIDs.Count - 1
                                _Index = i
                                URL = $"https://instagram.com/p/{_SavedPostsIDs(i)}/?__a=1"
                                ThrowAny(Token)
                                NextRequest(((i + 1) Mod 5) = 0)
                                ThrowAny(Token)
                                r = Responser.GetResponse(URL,, e)
                                MySiteSettings.TooManyRequests(False)
                                RequestsCount += 1
                                If Not r.IsEmptyString Then
                                    j = JsonDocument.Parse(r)
                                    If Not j Is Nothing Then
                                        _MediaObtained = False
                                        If j.Contains({"graphql", "shortcode_media"}) Then
                                            With j({"graphql", "shortcode_media"}).XmlIfNothing
                                                If .Count > 0 Then ObtainMedia(.Self, _SavedPostsIDs(i), String.Empty, String.Empty) : _MediaObtained = True
                                            End With
                                        End If
                                        If Not _MediaObtained AndAlso j.Contains("items") Then
                                            With j("items")
                                                If .Count > 0 Then
                                                    For Each jj In .Self : ObtainMedia2(jj, _SavedPostsIDs(i)) : Next
                                                End If
                                            End With
                                        End If
                                        j.Dispose()
                                    End If
                                End If
                            Next
                        End If
                        _DownloadComplete = True
                    Catch oex As OperationCanceledException When Token.IsCancellationRequested
                        Exit Do
                    Catch dex As ObjectDisposedException When Disposed
                        Exit Do
                    Catch ex As Exception
                        If DownloadingException(ex, $"downloading saved posts error [{URL}]") = 1 Then Continue Do Else Exit Do
                    End Try
                Loop
            Catch oex2 As OperationCanceledException When Token.IsCancellationRequested Or oex2.HelpLink = InstAborted
                If oex2.HelpLink = InstAborted Then HasError = True
            Catch DoEx As Exception
                ProcessException(DoEx, Token, $"downloading saved posts error [{URL}]")
            End Try
        End Sub
#End Region
#Region "Obtain Media"
        Private Sub ObtainMedia(ByVal node As EContainer, ByVal PostID As String, ByVal PostDate As String, ByVal SpecFolder As String)
            Dim CreateMedia As Action(Of EContainer) =
                Sub(ByVal e As EContainer)
                    Dim t As UTypes = If(e.Value("is_video").FromXML(Of Boolean)(False), UTypes.Video, UTypes.Picture)
                    Dim tmpValue$
                    If t = UTypes.Picture Then
                        tmpValue = e.Value("display_url")
                    Else
                        tmpValue = e.Value("video_url")
                    End If
                    If Not tmpValue.IsEmptyString Then _TempMediaList.ListAddValue(MediaFromData(t, tmpValue, PostID, PostDate, SpecFolder), LNC)
                End Sub
            If node.Contains({"edge_sidecar_to_children", "edges"}) Then
                For Each edge As EContainer In node({"edge_sidecar_to_children", "edges"}) : CreateMedia(edge("node").XmlIfNothing) : Next
            Else
                CreateMedia(node)
            End If
        End Sub
        Private Sub ObtainMedia2(ByVal n As EContainer, ByVal PostID As String, Optional ByVal SpecialFolder As String = Nothing,
                                 Optional ByVal DateObj As String = Nothing)
            Try
                Dim img As Predicate(Of EContainer) = Function(_img) Not _img.Name.IsEmptyString AndAlso _img.Name.StartsWith("image_versions") AndAlso _img.Count > 0
                Dim vid As Predicate(Of EContainer) = Function(_vid) Not _vid.Name.IsEmptyString AndAlso _vid.Name.StartsWith("video_versions") AndAlso _vid.Count > 0
                Dim ss As Func(Of EContainer, Sizes) = Function(_ss) New Sizes(_ss.Value("width"), _ss.Value("url"))
                Dim mDate As Func(Of EContainer, String) = Function(ByVal elem As EContainer) As String
                                                               If elem.Contains("taken_at") Then
                                                                   Return elem.Value("taken_at")
                                                               ElseIf elem.Contains("imported_taken_at") Then
                                                                   Return elem.Value("imported_taken_at")
                                                               Else
                                                                   Dim ev$ = elem.Value("device_timestamp")
                                                                   If Not ev.IsEmptyString Then
                                                                       If ev.Length > 10 Then
                                                                           Return elem.Value("device_timestamp").Substring(0, 10)
                                                                       Else
                                                                           Return ev
                                                                       End If
                                                                   Else
                                                                       Return String.Empty
                                                                   End If
                                                               End If
                                                           End Function
                If n.Count > 0 Then
                    Dim l As New List(Of Sizes)
                    Dim d As EContainer
                    Dim t%
                    '8 - gallery
                    '2 - one video
                    '1 - one picture
                    t = n.Value("media_type").FromXML(Of Integer)(-1)
                    If t >= 0 Then
                        Select Case t
                            Case 1
                                If n.Contains(img) Then
                                    t = n.Value("media_type").FromXML(Of Integer)(-1)
                                    DateObj = mDate(n)
                                    If t >= 0 Then
                                        With n.ItemF({img, "candidates"}).XmlIfNothing
                                            If .Count > 0 Then
                                                l.Clear()
                                                l.ListAddList(.Select(ss), LNC)
                                                If l.Count > 0 Then
                                                    l.Sort()
                                                    _TempMediaList.ListAddValue(MediaFromData(UTypes.Picture, l.First.Data, PostID, DateObj, SpecialFolder), LNC)
                                                    l.Clear()
                                                End If
                                            End If
                                        End With
                                    End If
                                End If
                            Case 2
                                If n.Contains(vid) Then
                                    DateObj = mDate(n)
                                    With n.ItemF({vid}).XmlIfNothing
                                        If .Count > 0 Then
                                            l.Clear()
                                            l.ListAddList(.Select(ss), LNC)
                                            If l.Count > 0 Then
                                                l.Sort()
                                                _TempMediaList.ListAddValue(MediaFromData(UTypes.Video, l.First.Data, PostID, DateObj, SpecialFolder), LNC)
                                                l.Clear()
                                            End If
                                        End If
                                    End With
                                End If
                            Case 8
                                DateObj = mDate(n)
                                With n("carousel_media").XmlIfNothing
                                    If .Count > 0 Then
                                        For Each d In .Self : ObtainMedia2(d, PostID, SpecialFolder, DateObj) : Next
                                    End If
                                End With
                        End Select
                    End If
                    l.Clear()
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendInLog, ex, "API.Instagram.ObtainMedia2")
            End Try
        End Sub
#End Region
        Private Sub GetUserId()
            Try
                Dim r$ = Responser.GetResponse($"https://www.instagram.com/{Name}/?__a=1",, EDP.ThrowException)
                If Not r.IsEmptyString Then
                    Using j As EContainer = JsonDocument.Parse(r).XmlIfNothing
                        ID = j({"graphql", "user"}, "id").XmlIfNothingValue
                    End Using
                End If
            Catch ex As Exception
                If Responser.StatusCode = HttpStatusCode.NotFound Or Responser.StatusCode = HttpStatusCode.BadRequest Then
                    Throw ex
                Else
                    LogError(ex, "get Instagram user id")
                End If
            End Try
        End Sub
#Region "Pinned stories"
        Private Sub GetStoriesData(ByRef StoriesList As List(Of String), ByVal Token As CancellationToken)
            Const ReqUrl$ = "https://i.instagram.com/api/v1/feed/reels_media/?{0}"
            Dim tmpList As IEnumerable(Of String)
            Dim qStr$, r$, sFolder$, storyID$
            Dim i% = -1
            Dim jj As EContainer, s As EContainer
            ThrowAny(Token)
            If StoriesList.ListExists Then
                tmpList = StoriesList.Take(5)
                If tmpList.ListExists Then
                    qStr = String.Format(ReqUrl, tmpList.Select(Function(q) $"reel_ids=highlight:{q}").ListToString(, "&"))
                    r = Responser.GetResponse(qStr,, EDP.ThrowException)
                    ThrowAny(Token)
                    If Not r.IsEmptyString Then
                        Using j As EContainer = JsonDocument.Parse(r).XmlIfNothing
                            If j.Contains("reels") Then
                                For Each jj In j("reels")
                                    i += 1
                                    sFolder = jj.Value("title")
                                    storyID = jj.Value("id").Replace("highlight:", String.Empty)
                                    If sFolder.IsEmptyString Then sFolder = $"Story_{storyID}"
                                    If sFolder.IsEmptyString Then sFolder = $"Story_{i}"
                                    sFolder = $"{StoriesFolder}\{sFolder}"
                                    If Not storyID.IsEmptyString Then storyID &= ":"
                                    With jj("items").XmlIfNothing
                                        If .Count > 0 Then
                                            For Each s In .Self : ThrowAny(Token) : ObtainMedia2(s, storyID & s.Value("id"), sFolder) : Next
                                        End If
                                    End With
                                Next
                            End If
                        End Using
                    End If
                    StoriesList.RemoveRange(0, tmpList.Count)
                End If
            End If
        End Sub
        Private Function GetStoriesList() As List(Of String)
            Try
                Dim r$ = Responser.GetResponse($"https://i.instagram.com/api/v1/highlights/{ID}/highlights_tray/",, EDP.ThrowException)
                If Not r.IsEmptyString Then
                    Using j As EContainer = JsonDocument.Parse(r).XmlIfNothing()("tray").XmlIfNothing
                        If j.Count > 0 Then Return j.Select(Function(jj) jj.Value("id").Replace("highlight:", String.Empty)).ListIfNothing
                    End Using
                End If
                Return Nothing
            Catch ex As Exception
                DownloadingException(ex, "API.Instagram.GetStoriesList")
                Return Nothing
            End Try
        End Function
#End Region
        Protected Overrides Sub ReparseVideo(ByVal Token As CancellationToken)
        End Sub
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
        ''' <summary>
        ''' <inheritdoc cref="UserDataBase.DownloadingException(Exception, String)"/><br/>
        ''' 1 - continue
        ''' </summary>
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False) As Integer
            If Responser.StatusCode = HttpStatusCode.NotFound Then
                UserExists = False
            ElseIf Responser.StatusCode = HttpStatusCode.BadRequest Then
                HasError = True
                MyMainLOG = "Instagram credentials have expired"
                MySiteSettings.HashUpdateRequired.Value = True
            ElseIf Responser.StatusCode = 429 Then
                With MySiteSettings
                    Dim WaiterExists As Boolean = .LastApplyingValue.HasValue
                    .TooManyRequests(True)
                    If Not WaiterExists Then .LastApplyingValue = 2
                End With
                Caught429 = True
                MyMainLOG = $"Number of requests before error 429: {RequestsCount}"
                Return 1
            Else
                MySiteSettings.HashUpdateRequired.Value = True
                If Not FromPE Then LogError(ex, Message) : HasError = True
                Return 0
            End If
            Return 2
        End Function
        Private Shared Function MediaFromData(ByVal t As UTypes, ByVal _URL As String, ByVal PostID As String, ByVal PostDate As String,
                                              Optional ByVal SpecialFolder As String = Nothing) As UserMedia
            _URL = LinkFormatterSecure(RegexReplace(_URL.Replace("\", String.Empty), LinkPattern))
            Dim m As New UserMedia(_URL, t) With {.Post = New UserPost With {.ID = PostID}}
            If Not m.URL.IsEmptyString Then m.File = CStr(RegexReplace(m.URL, FilesPattern))
            If Not PostDate.IsEmptyString Then m.Post.Date = AConvert(Of Date)(PostDate, DateProvider, Nothing) Else m.Post.Date = Nothing
            m.SpecialFolder = SpecialFolder
            Return m
        End Function
        Friend Shared Function GetVideoInfo(ByVal URL As String, ByVal r As Response) As IEnumerable(Of UserMedia)
            Try
                If Not URL.IsEmptyString AndAlso URL.Contains("instagram.com") Then
                    Dim PID$ = RegexReplace(URL, RParams.DMS(".*?instagram.com/p/([_\w\d]+)", 1))
                    If Not PID.IsEmptyString Then
                        Using t As New UserData
                            t.Responser = New Response
                            t.Responser.Copy(r)
                            t._SavedPostsIDs.Add(PID)
                            t.DownloadPosts(Nothing)
                            Return ListAddList(Nothing, t._TempMediaList)
                        End Using
                    End If
                End If
                Return Nothing
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.ShowMainMsg + EDP.SendInLog, ex, "Instagram standalone downloader: fetch media error")
            End Try
        End Function
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then _SavedPostsIDs.Clear()
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace