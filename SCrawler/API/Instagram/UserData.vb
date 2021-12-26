' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Tools.WebDocuments.JSON
Imports SCrawler.API.Base
Imports System.Threading
Imports System.Net
Imports UStates = SCrawler.API.Base.UserMedia.States
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace API.Instagram
    Friend Class UserData : Inherits UserDataBase
        Friend Overrides Property Site As Sites = Sites.Instagram
        ''' <summary>Video downloader initializer</summary>
        Private Sub New()
        End Sub
        ''' <summary>Default initializer</summary>
        Friend Sub New(ByVal u As UserInfo, Optional ByVal _LoadUserInformation As Boolean = True)
            User = u
            If _LoadUserInformation Then LoadUserInformation()
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            DownloadData(String.Empty, Token)
        End Sub
        Private _InstaHash As String = String.Empty
        Private Overloads Sub DownloadData(ByVal Cursor As String, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                Dim n As EContainer, nn As EContainer, node As EContainer
                Dim HasNextPage As Boolean = False
                Dim EndCursor$ = String.Empty
                Dim PostID$ = String.Empty, PostDate$ = String.Empty

                'Check environment
                If Cursor.IsEmptyString And _InstaHash.IsEmptyString Then _InstaHash = Settings(Sites.Instagram).InstaHash
                If _InstaHash.IsEmptyString Then Throw New ArgumentNullException("InstHash", "Query hash is null")
                If ID.IsEmptyString Then GetUserId()
                If ID.IsEmptyString Then Throw New ArgumentException("User ID is not detected", "ID")

                'Create query
                Dim vars$ = "{""id"":" & ID & ",""first"":12,""after"":""" & Cursor & """}"
                vars = SymbolsConverter.ASCII.EncodeSymbolsOnly(vars)
                URL = $"https://www.instagram.com/graphql/query/?query_hash={_InstaHash}&variables={vars}"

                'Get response
                Dim r$ = Responser.GetResponse(URL,, EDP.ThrowException)
                Settings(Sites.Instagram).InstagramTooManyRequests(False)
                ThrowAny(Token)

                'Data
                If Not r.IsEmptyString Then
                    Using j As EContainer = JsonDocument.Parse(r).XmlIfNothing
                        n = j.ItemF({"data", "user", 0}).XmlIfNothing
                        If n.Count > 0 Then
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

                                    PostID = node.Value("id")
                                    If Not PostID.IsEmptyString And _TempPostsList.Contains(PostID) Then Exit Sub
                                    _TempPostsList.Add(PostID)
                                    PostDate = node.Value("taken_at_timestamp")

                                    ObtainMedia(node, PostID, PostDate)
                                Next
                            End If
                        Else
                            If j.Value("status") = "ok" AndAlso j({"data", "user"}).XmlIfNothing.Count = 0 AndAlso _TempMediaList.Count = 0 Then
                                Settings(Sites.Instagram).InstaHashUpdateRequired.Value = True
                                UserExists = False
                                Exit Sub
                            End If
                        End If
                    End Using
                End If
                If HasNextPage And Not EndCursor.IsEmptyString Then DownloadData(EndCursor, Token)
            Catch oex As OperationCanceledException When Token.IsCancellationRequested
            Catch dex As ObjectDisposedException When Disposed
            Catch ex As Exception
                If Responser.StatusCode = HttpStatusCode.NotFound Then
                    UserExists = False
                ElseIf Responser.StatusCode = HttpStatusCode.BadRequest Then
                    MyMainLOG = "Instagram credentials have expired"
                    Settings(Sites.Instagram).InstaHashUpdateRequired.Value = True
                ElseIf Responser.StatusCode = 429 Then
                    Settings(Sites.Instagram).InstagramTooManyRequests(True)
                Else
                    Settings(Sites.Instagram).InstaHashUpdateRequired.Value = True
                    LogError(ex, $"data downloading error [{URL}]")
                End If
                HasError = True
            Finally
                _InstaHash = String.Empty
            End Try
        End Sub
        Private Sub ObtainMedia(ByVal node As EContainer, ByVal PostID As String, ByVal PostDate As String)
            Dim CreateMedia As Action(Of EContainer) =
                Sub(ByVal e As EContainer)
                    Dim t As UTypes = If(e.Value("is_video").FromXML(Of Boolean)(False), UTypes.Video, UTypes.Picture)
                    Dim tmpValue$
                    If t = UTypes.Picture Then
                        tmpValue = e.Value("display_url")
                    Else
                        tmpValue = e.Value("video_url")
                    End If
                    If Not tmpValue.IsEmptyString Then _TempMediaList.ListAddValue(MediaFromData(t, tmpValue, PostID, PostDate), LNC)
                End Sub
            If node.Contains({"edge_sidecar_to_children", "edges"}) Then
                For Each edge As EContainer In node({"edge_sidecar_to_children", "edges"}) : CreateMedia(edge("node").XmlIfNothing) : Next
            Else
                CreateMedia(node)
            End If
        End Sub
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
                    LogError(ex, "get instagram user id")
                End If
            End Try
        End Sub
        Protected Overrides Sub ReparseVideo(ByVal Token As CancellationToken)
        End Sub
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            Try
                Dim i%
                Dim dCount% = 0, dTotal% = 0
                ThrowAny(Token)
                If _ContentNew.Count > 0 Then
                    _ContentNew.RemoveAll(Function(c) c.URL.IsEmptyString)
                    If _ContentNew.Count > 0 Then
                        MyFile.Exists(SFO.Path)
                        Dim MyDir$ = MyFile.CutPath.PathNoSeparator
                        Dim vsf As Boolean = SeparateVideoFolderF
                        Dim f As SFile
                        Dim v As UserMedia
                        Using w As New WebClient
                            If vsf Then SFileShares.SFileExists($"{MyDir}\Video\", SFO.Path)
                            MainProgress.TotalCount += _ContentNew.Count
                            For i = 0 To _ContentNew.Count - 1
                                ThrowAny(Token)
                                v = _ContentNew(i)
                                v.State = UStates.Tried
                                If v.File.IsEmptyString Then
                                    f = v.URL
                                Else
                                    f = v.File
                                End If
                                f.Separator = "\"
                                f.Path = MyDir

                                If v.URL_BASE.IsEmptyString Then v.URL_BASE = v.URL

                                If Not v.File.IsEmptyString AndAlso Not v.URL_BASE.IsEmptyString Then
                                    Try
                                        If v.Type = UTypes.Video And vsf Then f.Path = $"{f.PathWithSeparator}Video"
                                        w.DownloadFile(v.URL_BASE, f.ToString)
                                        Select Case v.Type
                                            Case UTypes.Video : DownloadedVideos += 1 : _CountVideo += 1
                                            Case UTypes.Picture : DownloadedPictures += 1 : _CountPictures += 1
                                        End Select
                                        v.File = ChangeFileNameByProvider(f, v)
                                        v.State = UStates.Downloaded
                                    Catch wex As Exception
                                        ErrorDownloading(f, v.URL_BASE)
                                    End Try
                                Else
                                    v.State = UStates.Skipped
                                End If
                                _ContentNew(i) = v
                                If DownloadTopCount.HasValue AndAlso dCount >= DownloadTopCount.Value Then
                                    MainProgress.Perform(_ContentNew.Count - dTotal)
                                    Exit Sub
                                Else
                                    dTotal += 1
                                    MainProgress.Perform()
                                End If
                            Next
                        End Using
                    End If
                End If
            Catch oex As OperationCanceledException When Token.IsCancellationRequested
            Catch dex As ObjectDisposedException When Disposed
            Catch ex As Exception
                LogError(ex, "content downloading error")
                HasError = True
            End Try
        End Sub
        Private Shared Function MediaFromData(ByVal t As UTypes, ByVal _URL As String, ByVal PostID As String, ByVal PostDate As String) As UserMedia
            _URL = LinkFormatterSecure(RegexReplace(_URL.Replace("\", String.Empty), LinkPattern))
            Dim m As New UserMedia(_URL, t) With {.Post = New UserPost With {.ID = PostID}}
            If Not m.URL.IsEmptyString Then m.File = CStr(RegexReplace(m.URL, FilesPattern))
            If Not PostDate.IsEmptyString Then m.Post.Date = AConvert(Of Date)(PostDate, Declarations.DateProvider, Nothing) Else m.Post.Date = Nothing
            Return m
        End Function
        Friend Shared Function GetVideoInfo(ByVal URL As String) As IEnumerable(Of UserMedia)
            Try
                If Not URL.IsEmptyString AndAlso URL.Contains("instagram.com") Then
                    Do While Right(URL, 1) = "/" : URL = Left(URL, URL.Length - 1) : Loop
                    URL = $"{URL}/?__a=1"
                    Using t As New UserData
                        t.Responser = New PersonalUtilities.Tools.WEB.Response
                        t.Responser.Copy(Settings(Sites.Instagram).Responser)
                        Dim r$ = t.Responser.GetResponse(URL,, EDP.ThrowException)
                        If Not r.IsEmptyString Then
                            Using j As EContainer = JsonDocument.Parse(r).XmlIfNothing
                                Dim node As EContainer = j({"graphql", "shortcode_media"}).XmlIfNothing
                                If node.Count > 0 Then t.ObtainMedia(node, String.Empty, String.Empty)
                            End Using
                        End If
                        If t._TempMediaList.Count > 0 Then Return ListAddList(Nothing, t._TempMediaList)
                    End Using
                End If
                Return Nothing
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.ShowMainMsg + EDP.SendInLog, ex, "Instagram standalone downloader: fetch media error")
            End Try
        End Function
    End Class
End Namespace