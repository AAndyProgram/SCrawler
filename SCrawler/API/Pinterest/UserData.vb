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
Imports SCrawler.API.Base.GDL
Imports SCrawler.API.YouTube.Objects
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Tools.Web.Documents.JSON
Namespace API.Pinterest
    Friend Class UserData : Inherits UserDataBase
#Region "XML names"
        Private Const Name_IsUser As String = "IsUser"
        Private Const Name_TrueUserName As String = "TrueUserName"
        Private Const Name_TrueBoardName As String = "TrueBoardName"
#End Region
#Region "Structures"
        Private Structure BoardInfo
            Friend ID As String
            Friend Title As String
            Friend URL As String
            Friend Description As String
            Friend UserID As String
            Friend UserTitle As String
        End Structure
#End Region
#Region "Declarations"
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return HOST.Source
            End Get
        End Property
        Friend Property TrueUserName As String
        Friend Property TrueBoardName As String
        Friend Property IsUser As Boolean
#End Region
#Region "Load"
        Private Function ReconfUserName() As Boolean
            If TrueUserName.IsEmptyString Then
                Dim n$() = Name.Split("@")
                If n.ListExists Then
                    TrueUserName = n(0)
                    IsUser = True
                    If n.Length > 1 Then TrueBoardName = n(1) : IsUser = False
                    If Not IsSavedPosts And Not IsSingleObjectDownload Then
                        Dim l$ = IIf(IsUser, UserLabelName, "Board")
                        Settings.Labels.Add(l)
                        Labels.ListAddValue(l, LNC)
                        Labels.Sort()
                    End If
                    Return True
                End If
            End If
            Return False
        End Function
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            With Container
                If Loading Then
                    TrueUserName = .Value(Name_TrueUserName)
                    TrueBoardName = .Value(Name_TrueBoardName)
                    IsUser = .Value(Name_IsUser).FromXML(Of Boolean)(False)
                    ReconfUserName()
                Else
                    If ReconfUserName() Then .Value(Name_LabelsName) = Labels.ListToString("|", EDP.ReturnValue)
                    .Add(Name_TrueUserName, TrueUserName)
                    .Add(Name_TrueBoardName, TrueBoardName)
                    .Add(Name_IsUser, IsUser.BoolToInteger)
                End If
            End With
        End Sub
#End Region
#Region "Download overrides"
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                If IsSavedPosts Then
                    IsUser = True
                    TrueUserName = MySettings.SavedPostsUserName.Value
                    If TrueUserName.IsEmptyString Then Throw New ArgumentNullException("SavedPostsUserName", "Saved posts user not set")
                End If
                Dim boards As List(Of BoardInfo)
                Dim board As BoardInfo
                Dim b$ = TrueBoardName
                If Not b.IsEmptyString Then b &= "/"
                URL = $"https://www.pinterest.com/{TrueUserName}/{b}"
                If IsUser Then
                    boards = GetBoards(Token)
                Else
                    boards = New List(Of BoardInfo) From {New BoardInfo With {.URL = URL, .ID = ID, .Title = UserSiteName}}
                End If
                If boards.ListExists Then
                    For i% = 0 To boards.Count - 1
                        ThrowAny(Token)
                        board = boards(i)
                        DownloadBoardImages(board, Token)
                        boards(i) = board
                    Next
                    With boards.First
                        If IsUser Then
                            If ID.IsEmptyString Then ID = .UserID
                            UserSiteNameUpdate(.UserTitle)
                        Else
                            If ID.IsEmptyString Then ID = .ID
                            UserSiteNameUpdate(.Title)
                            UserDescriptionUpdate(.Description)
                        End If
                    End With
                End If
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]")
            End Try
        End Sub
#End Region
#Region "Get boards, images"
        Private Function GetBoards(ByVal Token As CancellationToken) As List(Of BoardInfo)
            Dim URL$ = $"https://www.pinterest.com/{TrueUserName}/"
            Try
                Dim boards As New List(Of BoardInfo)
                Dim b As BoardInfo
                Dim r$
                Dim j As EContainer, jj As EContainer
                Dim rootNode$() = {"resource_response", "data"}
                Dim jErr As New ErrorsDescriber(EDP.SendToLog + EDP.ReturnValue)
                Dim urls As List(Of String) = GetDataFromGalleryDL(URL, True)
                If urls.ListExists Then urls.RemoveAll(Function(__url) Not __url.Contains("BoardsResource/get/"))
                If urls.ListExists Then
                    For Each URL In urls
                        ThrowAny(Token)
                        r = Responser.GetResponse(URL,, EDP.ReturnValue)
                        If Not r.IsEmptyString Then
                            j = JsonDocument.Parse(r, jErr)
                            If Not j Is Nothing Then
                                If If(j(rootNode)?.Count, 0) > 0 Then
                                    For Each jj In j(rootNode)
                                        b = New BoardInfo With {
                                            .URL = jj.Value("url"),
                                            .Title = TitleHtmlConverter(jj.Value("name")),
                                            .ID = jj.Value("id")
                                        }
                                        If Not b.URL.IsEmptyString Then
                                            b.URL = $"https://www.pinterest.com/{b.URL.StringTrimStart("/").StringTrimEnd("/")}/"
                                            boards.Add(b)
                                        End If
                                    Next
                                End If
                                j.Dispose()
                            End If
                        End If
                    Next
                End If
                Return boards
            Catch ex As Exception
                ProcessException(ex, Token, $"data (gallery-dl boards) downloading error [{URL}]")
                Return Nothing
            End Try
        End Function
        Private Sub DownloadBoardImages(ByRef Board As BoardInfo, ByVal Token As CancellationToken)
            Dim bUrl$ = String.Empty
            Try
                Dim r$
                Dim j As EContainer, jj As EContainer
                Dim u As UserMedia
                Dim folder$ = If(IsUser, Board.Title.IfNullOrEmpty(Board.ID), String.Empty)
                Dim titleExists As Boolean = Not Board.Title.IsEmptyString
                Dim i% = -1
                Dim jErr As New ErrorsDescriber(EDP.SendToLog + EDP.ReturnValue)
                Dim rootNode$() = {"resource_response", "data"}
                Dim images As List(Of Sizes)
                Dim imgSelector As Func(Of EContainer, Sizes) = Function(img) New Sizes(img.Value("width"), img.Value("url"))
                Dim fullData As Predicate(Of EContainer) = Function(e) e.Count > 5
                Dim l As List(Of String) = GetDataFromGalleryDL(Board.URL, False)
                If l.ListExists Then l.RemoveAll(Function(ll) Not ll.Contains("BoardFeedResource/get/"))
                If l.ListExists Then
                    For Each bUrl In l
                        ThrowAny(Token)
                        r = Responser.GetResponse(bUrl,, EDP.ReturnValue)
                        If Not r.IsEmptyString Then
                            j = JsonDocument.Parse(r, jErr)
                            If Not j Is Nothing Then
                                If If(j(rootNode)?.Count, 0) > 0 Then
                                    For Each jj In j(rootNode)
                                        With jj
                                            If .Contains("images") Then
                                                images = .Item("images").Select(imgSelector).ToList
                                                If images.Count > 0 Then
                                                    images.Sort()
                                                    i += 1
                                                    u = New UserMedia(images(0).Data) With {
                                                        .Post = New UserPost(jj.Value("id"), AConvert(Of Date)(jj.Value("created_at"), DateProvider, Nothing)),
                                                        .Type = UserMedia.Types.Picture,
                                                        .SpecialFolder = folder
                                                    }
                                                    If i = 0 Then
                                                        If Board.Title.IsEmptyString Or Board.ID.IsEmptyString Then
                                                            Board.Title = TitleHtmlConverter(.Value({"board"}, "name"))
                                                            Board.ID = .Value({"board"}, "id")
                                                        End If
                                                        Board.UserID = .Value({"board", "owner"}, "id")
                                                        Board.UserTitle = TitleHtmlConverter(.Value({"board", "owner"}, "full_name"))
                                                        If Not titleExists And IsUser Then
                                                            If Not Board.Title.IsEmptyString Then
                                                                folder = Board.Title
                                                            ElseIf Not Board.ID.IsEmptyString Then
                                                                folder = Board.ID
                                                            End If
                                                            u.SpecialFolder = folder
                                                        End If
                                                    End If

                                                    If Not u.URL.IsEmptyString Then
                                                        If u.Post.Date.HasValue Then
                                                            Select Case CheckDatesLimit(u.Post.Date.Value, Nothing)
                                                                Case DateResult.Skip : _TempPostsList.ListAddValue(u.Post.ID, LNC) : Continue For
                                                                Case DateResult.Exit : Exit Sub
                                                            End Select
                                                        End If
                                                        If Not _TempPostsList.Contains(u.Post.ID) Then
                                                            _TempPostsList.ListAddValue(u.Post.ID, LNC)
                                                            _TempMediaList.ListAddValue(u, LNC)
                                                        Else
                                                            Exit For
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End With
                                    Next
                                End If
                                j.Dispose()
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex, Token, $"data (gallery-dl images) downloading error [{bUrl}]")
            End Try
        End Sub
#End Region
#Region "Gallery-DL Support"
        Private Class GDLBatch : Inherits GDL.GDLBatch
            Private ReadOnly Property Source As UserData
            Private ReadOnly IsBoardsRequested As Boolean
            Friend Sub New(ByRef s As UserData, ByVal IsBoardsRequested As Boolean)
                MyBase.New
                Source = s
                Me.IsBoardsRequested = IsBoardsRequested
            End Sub
            Protected Overrides Async Sub OutputDataReceiver(ByVal Sender As Object, ByVal e As DataReceivedEventArgs)
                If IsBoardsRequested Then
                    Await Validate(e.Data)
                Else
                    MyBase.OutputDataReceiver(Sender, e)
                    Await Validate(e.Data)
                End If
            End Sub
            Protected Overrides Async Function Validate(ByVal Value As String) As Task
                If IsBoardsRequested Then
                    If ErrorOutputData.Count > 0 Then
                        If Await Task.Run(Of Boolean)(Function() ErrorOutputData.Exists(Function(ee) Not ee.IsEmptyString AndAlso
                                                                                                     ee.StartsWith(UrlTextStart))) Then Kill(EDP.None)
                    End If
                Else
                    If Await Task.Run(Of Boolean)(Function() Not Value.IsEmptyString AndAlso
                                                             Source._TempPostsList.Exists(Function(v) Value.Contains(v))) Then Kill(EDP.None)
                End If
            End Function
        End Class
        Private Function GetDataFromGalleryDL(ByVal URL As String, ByVal IsBoardsRequested As Boolean) As List(Of String)
            Dim command$ = $"gallery-dl --verbose --simulate "
            Try
                If Not URL.IsEmptyString Then
                    If MySettings.CookiesNetscapeFile.Exists Then command &= $"--cookies ""{MySettings.CookiesNetscapeFile}"" "
                    command &= URL
                    Using batch As New GDLBatch(Me, IsBoardsRequested)
                        Return GetUrlsFromGalleryDl(batch, command)
                    End Using
                End If
                Return Nothing
            Catch ex As Exception
                HasError = True
                LogError(ex, $"GetJson({command})")
                Return Nothing
            End Try
        End Function
#End Region
#Region "DownloadContent"
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
#End Region
#Region "DownloadSingleObject"
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            User = New UserInfo(MySettings.IsMyUser(Data.URL).UserName, HOST)
            User.File.Path = Data.File.Path
            SeparateVideoFolder = False
            ReconfUserName()
            DownloadDataF(Token)
            Data.Title = UserSiteName
            If Data.Title.IsEmptyString Then
                Data.Title = TrueUserName
                If Not TrueBoardName.IsEmptyString Then Data.Title &= $"/{TrueBoardName}"
            End If
            Dim additPath$ = TitleHtmlConverter(UserSiteName)
            If additPath.IsEmptyString Then additPath = IIf(IsUser, TrueUserName, TrueBoardName)
            If Not additPath.IsEmptyString Then
                Dim f As SFile = User.File
                f.Path = f.PathWithSeparator & additPath
                User.File = f
                f = Data.File
                f.Path = User.File.Path
                Data.File = f
            End If
        End Sub
        Protected Overrides Sub DownloadSingleObject_PostProcessing(ByVal Data As IYouTubeMediaContainer, Optional ByVal ResetTitle As Boolean = True)
            MyBase.DownloadSingleObject_PostProcessing(Data, Data.Title.IsEmptyString Or Not Data.Title = UserSiteName)
        End Sub
#End Region
#Region "Exception"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            Return 0
        End Function
    End Class
#End Region
End Namespace