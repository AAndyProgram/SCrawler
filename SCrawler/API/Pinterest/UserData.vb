' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports System.Text.RegularExpressions
Imports SCrawler.API.Base
Imports SCrawler.API.Base.GDL
Imports SCrawler.API.YouTube.Objects
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Documents.JSON
Namespace API.Pinterest
    Friend Class UserData : Inherits UserDataBase
#Region "XML names"
        Private Const Name_IsUser As String = "IsUser"
        Private Const Name_TrueUserName As String = "TrueUserName"
        Private Const Name_TrueBoardName As String = "TrueBoardName"
        Private Const Name_ExtractSubBoards As String = "ExtractSubBoards"
        Private Const Name_IsSubBoard As String = "IsSubBoard"
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
        Friend Property IsUser_NB As Boolean
        Private Property IsSubBoard As Boolean = False
        Friend Property ExtractSubBoards As Boolean = True
        Private Const BoardLabelName As String = "Board"
        Friend Overrides ReadOnly Property SpecialLabels As IEnumerable(Of String)
            Get
                Return {UserLabelName, BoardLabelName}
            End Get
        End Property
#End Region
#Region "Load, Exchange"
        Private Function ReconfUserName() As Boolean
            If TrueUserName.IsEmptyString Then
                Dim n$() = Name.Split("@")
                If n.ListExists Then
                    TrueUserName = n(0)
                    IsUser_NB = True
                    If n.Length > 1 Then
                        TrueBoardName = n(1)
                        If n.Length > 2 AndAlso Not n(2).IsEmptyString Then TrueBoardName &= $"/{n(2)}" : IsSubBoard = True
                        IsUser_NB = False
                    End If
                    If Not IsSavedPosts And Not IsSingleObjectDownload Then
                        Dim l$ = IIf(IsUser_NB, UserLabelName, BoardLabelName)
                        Settings.Labels.Add(l)
                        Labels.ListAddValue(l, LNC)
                        Labels.Sort()
                        Return True
                    End If
                End If
            End If
            Return False
        End Function
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            With Container
                If Loading Then
                    TrueUserName = .Value(Name_TrueUserName)
                    TrueBoardName = .Value(Name_TrueBoardName)
                    IsUser_NB = .Value(Name_IsUser).FromXML(Of Boolean)(False)
                    ExtractSubBoards = .Value(Name_ExtractSubBoards).FromXML(Of Boolean)(True)
                    IsSubBoard = .Value(Name_IsSubBoard).FromXML(Of Boolean)(False)
                    ReconfUserName()
                Else
                    If ReconfUserName() Then .Value(Name_LabelsName) = LabelsString
                    .Add(Name_TrueUserName, TrueUserName)
                    .Add(Name_TrueBoardName, TrueBoardName)
                    .Add(Name_IsUser, IsUser_NB.BoolToInteger)
                    .Add(Name_ExtractSubBoards, ExtractSubBoards.BoolToInteger)
                    .Add(Name_IsSubBoard, IsSubBoard.BoolToInteger)
                End If
            End With
        End Sub
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return New EditorExchangeOptions(Me)
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            If Not Obj Is Nothing AndAlso TypeOf Obj Is EditorExchangeOptions Then ExtractSubBoards = DirectCast(Obj, EditorExchangeOptions).ExtractSubBoards
        End Sub
#End Region
#Region "Download overrides"
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                If IsSavedPosts Then
                    IsUser_NB = True
                    TrueUserName = MySettings.SavedPostsUserName.Value
                    If TrueUserName.IsEmptyString Then Throw New ArgumentNullException("SavedPostsUserName", "Saved posts user not set")
                End If
                Dim boards As List(Of BoardInfo)
                Dim board As BoardInfo
                Dim b$ = TrueBoardName
                If Not b.IsEmptyString Then b &= "/"
                URL = $"https://www.pinterest.com/{TrueUserName}/{b}"
                If IsUser_NB Then
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
                        If IsUser_NB Then
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
            Finally
                ProgressPre.Done()
            End Try
        End Sub
#End Region
#Region "Get boards, images"
        Private Function GetBoardInfo(ByVal e As EContainer) As BoardInfo
            If Not e Is Nothing Then
                Dim b As New BoardInfo With {
                    .URL = e.Value("url"),
                    .Title = TitleHtmlConverter(e.Value("name")).IfNullOrEmpty(TitleHtmlConverter(e.Value("title"))),
                    .ID = e.Value("id")
                }
                If Not b.URL.IsEmptyString Then b.URL = $"https://www.pinterest.com/{b.URL.StringTrimStart("/").StringTrimEnd("/")}/"
                Return b
            Else
                Return Nothing
            End If
        End Function
        Private Function GetBoards(ByVal Token As CancellationToken) As List(Of BoardInfo)
            Dim URL$ = $"https://www.pinterest.com/{TrueUserName}/"
            Try
                Dim boards As New List(Of BoardInfo)
                Dim b As BoardInfo
                Dim r$
                Dim j As EContainer, jj As EContainer
                Dim jErr As New ErrorsDescriber(EDP.SendToLog + EDP.ReturnValue)
                Dim urls As New List(Of String)
                urls.ListAddList(GetDataFromGalleryDL(URL, True, Token), LNC)
                If urls.ListExists Then urls.RemoveAll(Function(__url) Not __url.Contains("BoardsResource/get/"))
                If urls.ListExists Then
                    Responser.Headers.Add(PwsHeader)
                    ProgressPre.ChangeMax(urls.Count)
                    For Each URL In urls
                        ProgressPre.Perform()
                        ThrowAny(Token)
                        r = Responser.GetResponse(URL,, EDP.ReturnValue)
                        If Not r.IsEmptyString Then
                            j = JsonDocument.Parse(r, jErr)
                            If Not j Is Nothing Then
                                If If(j(BoardInfoRootNode)?.Count, 0) > 0 Then
                                    For Each jj In j(BoardInfoRootNode)
                                        b = GetBoardInfo(jj)
                                        If Not b.URL.IsEmptyString Then boards.Add(b)
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
            Finally
                Responser.Headers.Remove(PwsHeader)
            End Try
        End Function
        Private Sub DownloadBoardImages(ByRef Board As BoardInfo, ByVal Token As CancellationToken)
            Dim bUrl As GDLURL = Nothing
            Try
                Dim r$
                Dim j As EContainer, jj As EContainer
                Dim u As UserMedia
                Dim __getBoardTitle As Func(Of BoardInfo, String) = Function(__board) __board.Title.IfNullOrEmpty(__board.ID)
                Dim folderDef$ = If(IsUser_NB, __getBoardTitle(Board), String.Empty)
                Dim titleExists As Boolean = Not Board.Title.IsEmptyString
                Dim i% = -1
                Dim jErr As New ErrorsDescriber(EDP.SendToLog + EDP.ReturnValue)
                Dim rErr As New ErrorsDescriber(EDP.ReturnValue)
                Dim images As List(Of Sizes)
                Dim imgSelector As Func(Of EContainer, Sizes) = Function(img) New Sizes(img.Value("width"), img.Value("url"))
                Dim fullData As Predicate(Of EContainer) = Function(e) e.Count > 5
                Dim subBoard As BoardInfo = Nothing
                Dim subBoardAppender As Func(Of String) = Function() _
                    If(Not __getBoardTitle(subBoard).IsEmptyString,
                       $"{IIf(folderDef.IsEmptyString, String.Empty, "\")}{__getBoardTitle(subBoard)}",
                       String.Empty)
                Dim __getSubBoard As Func(Of Boolean) = Function() ExtractSubBoards Or (IsSubBoard And i = -1)
                Dim sbCount% = 0
                Dim __getBoardInfo As Action(Of GDLURL) = Sub(ByVal sb As GDLURL)
                                                              sbCount += 1
                                                              r = Responser.GetResponse(sb.URL,, rErr)
                                                              If Not r.IsEmptyString Then
                                                                  Using jsb As EContainer = JsonDocument.Parse(r, jErr)
                                                                      If jsb.ListExists Then subBoard = GetBoardInfo(jsb(BoardInfoRootNode)) : Exit Sub
                                                                  End Using
                                                              End If
                                                              subBoard = Nothing
                                                          End Sub
                Dim l As List(Of GDLURL) = GetDataFromGalleryDL(Board.URL, False, Token)
                If l.ListExists Then l.RemoveAll(Function(ll) ll.URL.IsEmptyString)
                If l.ListExists Then
                    Responser.Headers.Add(PwsHeader)
                    ProgressPre.ChangeMax(l.Count)
                    For Each bUrl In l
                        ProgressPre.Perform()
                        ThrowAny(Token)

                        If bUrl.URL.Contains("BoardFeedResource/get/") Or (bUrl.URL.Contains("BoardSectionPinsResource/get/") And (ExtractSubBoards Or (IsSubBoard And sbCount = 1))) Then
                            r = Responser.GetResponse(bUrl.URL,, rErr)
                            If Not r.IsEmptyString Then
                                j = JsonDocument.Parse(r, jErr)
                                If Not j Is Nothing Then
                                    If If(j(BoardInfoRootNode)?.Count, 0) > 0 Then
                                        ProgressPre.ChangeMax(j(BoardInfoRootNode).Count)
                                        For Each jj In j(BoardInfoRootNode)
                                            ProgressPre.Perform()
                                            With jj
                                                If .Contains("images") Then
                                                    images = .Item("images").Select(imgSelector).ToList
                                                    If images.Count > 0 Then
                                                        images.Sort()
                                                        i += 1
                                                        u = New UserMedia(images(0).Data) With {
                                                            .Post = New UserPost(jj.Value("id"), AConvert(Of Date)(jj.Value("created_at"), DateProvider, Nothing)),
                                                            .Type = UserMedia.Types.Picture,
                                                            .SpecialFolder = folderDef & subBoardAppender.Invoke
                                                        }
                                                        If i = 0 Then
                                                            If Board.Title.IsEmptyString Or Board.ID.IsEmptyString Then
                                                                Board.Title = TitleHtmlConverter(.Value({"board"}, "name"))
                                                                Board.ID = .Value({"board"}, "id")
                                                            End If
                                                            Board.UserID = .Value({"board", "owner"}, "id")
                                                            Board.UserTitle = TitleHtmlConverter(.Value({"board", "owner"}, "full_name"))
                                                            If Not titleExists And IsUser_NB Then
                                                                folderDef = Board.Title.IfNullOrEmpty(Board.ID).IfNullOrEmpty(folderDef)
                                                                u.SpecialFolder = folderDef & subBoardAppender.Invoke
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
                        ElseIf bUrl.URL.Contains("BoardSectionResource/get/") And (ExtractSubBoards Or (IsSubBoard And i = -1)) Then
                            __getBoardInfo(bUrl)
                            If IsSubBoard And i = -1 And Board.Title.IsEmptyString Then
                                Board.Title = subBoard.Title
                                If Board.ID.IsEmptyString Then Board.ID = subBoard.ID
                                subBoard = Nothing
                                folderDef = String.Empty
                            End If
                        End If

                    Next
                End If
            Catch ex As Exception
                ProcessException(ex, Token, $"data (gallery-dl images) downloading error [{bUrl.URL}]")
            Finally
                Responser.Headers.Remove(PwsHeader)
            End Try
        End Sub
#End Region
#Region "Gallery-DL Support"
        Private Structure GDLURL : Implements IRegExCreator
            Friend URL As String
            Friend BoardId As String
            Public Shared Widening Operator CType(ByVal u As String) As GDLURL
                Return New GDLURL With {.URL = u}
            End Operator
            Public Shared Widening Operator CType(ByVal u As GDLURL) As String
                Return u.URL
            End Operator
            Private Function CreateFromArray(ByVal ParamsArray() As String) As Object Implements IRegExCreator.CreateFromArray
                If ParamsArray.ListExists(2) Then
                    Dim u$ = ParamsArray(0).StringTrim.StringTrimEnd("/"), u2$
                    If Not u.IsEmptyString Then
                        u2 = ParamsArray(1).StringTrim
                        If Not u2.IsEmptyString AndAlso u2.StartsWith("GET", StringComparison.OrdinalIgnoreCase) Then
                            u2 = u2.Remove(0, 3).StringTrim.StringTrimStart("/")
                            If Not u2.IsEmptyString Then URL = $"{u}/{u2}"
                        End If
                    End If
                End If
                Return Me
            End Function
            Public Shared Operator =(ByVal x As GDLURL, ByVal y As GDLURL) As Boolean
                Return x.URL = y.URL
            End Operator
            Public Shared Operator <>(ByVal x As GDLURL, ByVal y As GDLURL) As Boolean
                Return Not x.URL = y.URL
            End Operator
            Public Overrides Function ToString() As String
                Return URL
            End Function
            Public Overrides Function Equals(ByVal Obj As Object) As Boolean
                Return URL = CType(Obj, String)
            End Function
        End Structure
        Private Class GDLBatch : Inherits GDL.GDLBatch
            Private ReadOnly Property Source As UserData
            Private ReadOnly IsBoardsRequested As Boolean
            Friend Sub New(ByRef s As UserData, ByVal IsBoardsRequested As Boolean, ByVal _Token As CancellationToken)
                MyBase.New(_Token)
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
                        If Await Task.Run(Of Boolean)(Function() Token.IsCancellationRequested OrElse
                                                                 ErrorOutputData.Exists(Function(ee) Not ee.IsEmptyString AndAlso
                                                                                                     ee.StartsWith(UrlTextStart))) Then Kill()
                    End If
                Else
                    If Await Task.Run(Of Boolean)(Function() Token.IsCancellationRequested OrElse
                                                             (Not Value.IsEmptyString AndAlso
                                                             Source._TempPostsList.Exists(Function(v) Value.Contains(v)))) Then Kill()
                End If
            End Function
        End Class
        Private Function GetDataFromGalleryDL(ByVal URL As String, ByVal IsBoardsRequested As Boolean, ByVal Token As CancellationToken) As List(Of GDLURL)
            Dim command$ = $"""{Settings.GalleryDLFile.File}"" --verbose --simulate "
            Try
                If Not URL.IsEmptyString Then
                    Dim urls As New List(Of GDLURL)
                    Dim u As GDLURL
                    Dim s$ = String.Empty
                    If MySettings.CookiesNetscapeFile.Exists Then command &= $"--cookies ""{MySettings.CookiesNetscapeFile}"" "
                    command &= URL
                    Using batch As New GDLBatch(Me, IsBoardsRequested, Token)
                        With batch
                            .Execute(command)
                            If .ErrorOutputData.Count > 0 Then
                                For Each eValue$ In .ErrorOutputData
                                    s = CStr(RegexReplace(eValue, SubBoardRegEx)).IfNullOrEmpty(s)
                                    u = RegexFields(Of GDLURL)(eValue, {GdlUrlPattern}, {1, 2}, EDP.ReturnValue).ListIfNothing.FirstOrDefault
                                    If Not u.URL.IsEmptyString Then
                                        If Not s.IsEmptyString Then u.BoardId = s
                                        urls.Add(u)
                                    End If
                                Next
                                Return urls
                            End If
                        End With
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
            If additPath.IsEmptyString Then additPath = IIf(IsUser_NB, TrueUserName, TrueBoardName)
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