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
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Functions.Messaging
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace API.Instagram
    Friend Class UserData : Inherits UserDataBase
#Region "XML Names"
        Private Const Name_LastCursor As String = "LastCursor"
        Private Const Name_FirstLoadingDone As String = "FirstLoadingDone"
        Private Const Name_GetTimeline As String = "GetTimeline"
        Private Const Name_GetStories As String = "GetStories"
        Private Const Name_GetTagged As String = "GetTaggedData"
        Private Const Name_TaggedChecked As String = "TaggedChecked"
        Private Const Name_NameTrue As String = "NameTrue"
#End Region
#Region "Declarations"
        Private Structure PostKV : Implements IEContainerProvider
            Private Const Name_Code As String = "Code"
            Private Const Name_Section As String = "Section"
            Friend Code As String
            Friend ID As String
            Friend Section As Sections
            Friend Sub New(ByVal _Section As Sections)
                Section = _Section
            End Sub
            Friend Sub New(ByVal _Code As String, ByVal _ID As String, ByVal _Section As Sections)
                Code = _Code
                ID = _ID
                Section = _Section
            End Sub
            Private Sub New(ByVal e As EContainer)
                Code = e.Attribute(Name_Code)
                Section = e.Attribute(Name_Section)
                ID = e.Value
            End Sub
            Public Shared Widening Operator CType(ByVal e As EContainer) As PostKV
                Return New PostKV(e)
            End Operator
            Public Overrides Function Equals(ByVal Obj As Object) As Boolean
                If Not IsNothing(Obj) AndAlso TypeOf Obj Is PostKV Then
                    With DirectCast(Obj, PostKV)
                        Return Code = .Code And ID = .ID And Section = .Section
                    End With
                Else
                    Return False
                End If
            End Function
            Private Function ToEContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
                Return New EContainer("Post", ID, {New EAttribute(Name_Section, CInt(Section)), New EAttribute(Name_Code, Code)})
            End Function
        End Structure
        Private ReadOnly Property MySiteSettings As SiteSettings
            Get
                Return DirectCast(HOST.Source, SiteSettings)
            End Get
        End Property
        Private ReadOnly PostsKVIDs As List(Of PostKV)
        Private ReadOnly PostsToReparse As List(Of PostKV)
        Private LastCursor As String = String.Empty
        Private FirstLoadingDone As Boolean = False
        Friend Property GetTimeline As Boolean = True
        Friend Property GetStories As Boolean
        Friend Property GetTaggedData As Boolean
        Private _NameTrue As String = String.Empty
        Private ReadOnly Property NameTrue As String
            Get
                Return _NameTrue.IfNullOrEmpty(Name)
            End Get
        End Property
        Private UserNameRequested As Boolean = False
#End Region
#Region "Exchange options"
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return New EditorExchangeOptions(Me)
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            If Not Obj Is Nothing AndAlso TypeOf Obj Is EditorExchangeOptions Then
                With DirectCast(Obj, EditorExchangeOptions)
                    GetTimeline = .GetTimeline
                    GetStories = .GetStories
                    GetTaggedData = .GetTagged
                End With
            End If
        End Sub
#End Region
#Region "Initializer, loader"
        Friend Sub New()
            PostsKVIDs = New List(Of PostKV)
            PostsToReparse = New List(Of PostKV)
        End Sub
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            With Container
                If Loading Then
                    LastCursor = .Value(Name_LastCursor)
                    FirstLoadingDone = .Value(Name_FirstLoadingDone).FromXML(Of Boolean)(False)
                    GetTimeline = .Value(Name_GetTimeline).FromXML(Of Boolean)(CBool(MySiteSettings.GetTimeline.Value))
                    GetStories = .Value(Name_GetStories).FromXML(Of Boolean)(CBool(MySiteSettings.GetStories.Value))
                    GetTaggedData = .Value(Name_GetTagged).FromXML(Of Boolean)(CBool(MySiteSettings.GetTagged.Value))
                    TaggedChecked = .Value(Name_TaggedChecked).FromXML(Of Boolean)(False)
                    _NameTrue = .Value(Name_NameTrue)
                Else
                    .Add(Name_LastCursor, LastCursor)
                    .Add(Name_FirstLoadingDone, FirstLoadingDone.BoolToInteger)
                    .Add(Name_GetTimeline, GetTimeline.BoolToInteger)
                    .Add(Name_GetStories, GetStories.BoolToInteger)
                    .Add(Name_GetTagged, GetTaggedData.BoolToInteger)
                    .Add(Name_TaggedChecked, TaggedChecked.BoolToInteger)
                    .Add(Name_NameTrue, _NameTrue)
                End If
            End With
        End Sub
#End Region
#Region "Download data"
        Private E560Thrown As Boolean = False
        Private Class ExitException : Inherits Exception
            Friend Shared Sub Throw560(ByRef Source As UserData)
                If Not Source.E560Thrown Then
                    MyMainLOG = $"{Source.ToStringForLog}: (560) Download skipped until next session"
                    Source.E560Thrown = True
                End If
                Throw New ExitException
            End Sub
        End Class
        Private Sub LoadSavePostsKV(ByVal Load As Boolean)
            Dim x As XmlFile
            Dim f As SFile = MyFilePosts
            If Not f.IsEmptyString Then
                f.Name &= "_KV"
                f.Extension = "xml"
                If Load Then
                    PostsKVIDs.Clear()
                    x = New XmlFile(f, Protector.Modes.All, False) With {.AllowSameNames = True, .XmlReadOnly = True}
                    x.LoadData()
                    If x.Count > 0 Then PostsKVIDs.ListAddList(x, LAP.IgnoreICopier)
                    x.Dispose()
                Else
                    x = New XmlFile With {.AllowSameNames = True}
                    x.AddRange(PostsKVIDs)
                    x.Name = "Posts"
                    x.Save(f, EDP.SendToLog)
                    x.Dispose()
                End If
            End If
        End Sub
        Private Overloads Function PostKvExists(ByVal pkv As PostKV) As Boolean
            Return PostKvExists(pkv.ID, False, pkv.Section) OrElse PostKvExists(pkv.Code, True, pkv.Section)
        End Function
        Private Overloads Function PostKvExists(ByVal PostCodeId As String, ByVal IsCode As Boolean, ByVal Section As Sections) As Boolean
            If Not PostCodeId.IsEmptyString And PostsKVIDs.Count > 0 Then
                If PostsKVIDs.FindIndex(Function(p) p.Section = Section AndAlso If(IsCode, p.Code = PostCodeId, p.ID = PostCodeId)) >= 0 Then
                    Return True
                ElseIf Not IsCode Then
                    Return _TempPostsList.Contains(GetPostIdBySection(PostCodeId, Section)) Or
                           _TempPostsList.Contains(PostCodeId.Replace($"_{ID}", String.Empty)) Or
                           _TempPostsList.Contains(GetPostIdBySection(PostCodeId.Replace($"_{ID}", String.Empty), Section))
                End If
            End If
            Return False
        End Function
        Friend Function GetPostCodeById(ByVal PostID As String) As String
            Try
                If Not PostID.IsEmptyString Then
                    Dim f As SFile = MyFilePosts
                    If Not f.IsEmptyString Then
                        f.Name &= "_KV"
                        f.Extension = "xml"
                        Dim l As List(Of PostKV) = Nothing
                        Using x As New XmlFile(f, Protector.Modes.All, False) With {.AllowSameNames = True, .XmlReadOnly = True}
                            x.LoadData()
                            l.ListAddList(x, LAP.IgnoreICopier)
                        End Using
                        Dim code$ = String.Empty
                        If l.ListExists Then
                            Dim i% = l.FindIndex(Function(p) p.ID = PostID)
                            If i >= 0 Then code = l(i).Code
                            l.Clear()
                        End If
                        Return code
                    End If
                End If
                Return String.Empty
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, $"{ToStringForLog()}: Cannot find post code by ID ({PostID})", String.Empty)
            End Try
        End Function
        Private Function GetPostIdBySection(ByVal ID As String, ByVal Section As Sections) As String
            If Section = Sections.Timeline Then
                Return ID
            Else
                Return $"{Section}_{ID}"
            End If
        End Function
        Private _DownloadingInProgress As Boolean = False
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            UserNameRequested = False
            Dim s As Sections = Sections.Timeline
            Dim errorFound As Boolean = False
            Try
                LoadSavePostsKV(True)
                _DownloadingInProgress = True
                AddHandler Responser.ResponseReceived, AddressOf Responser_ResponseReceived
                ThrowAny(Token)
                HasError = False
                Dim dt As Func(Of Boolean) = Function() (CBool(MySiteSettings.DownloadTimeline.Value) And GetTimeline) Or IsSavedPosts
                If dt.Invoke And Not LastCursor.IsEmptyString Then
                    s = IIf(IsSavedPosts, Sections.SavedPosts, Sections.Timeline)
                    DownloadData(LastCursor, s, Token)
                    ProgressPre.Done()
                    ThrowAny(Token)
                    If Not HasError Then FirstLoadingDone = True
                End If
                If dt.Invoke And Not HasError Then
                    s = IIf(IsSavedPosts, Sections.SavedPosts, Sections.Timeline)
                    DownloadData(String.Empty, s, Token)
                    ProgressPre.Done()
                    ThrowAny(Token)
                    If Not HasError Then FirstLoadingDone = True
                End If
                If FirstLoadingDone Then LastCursor = String.Empty
                If Not IsSavedPosts AndAlso MySiteSettings.BaseAuthExists() Then
                    If CBool(MySiteSettings.DownloadStories.Value) And GetStories Then s = Sections.Stories : DownloadData(String.Empty, s, Token) : ProgressPre.Done()
                    If CBool(MySiteSettings.DownloadTagged.Value) And ACheck(MySiteSettings.HashTagged.Value) And GetTaggedData Then s = Sections.Tagged : DownloadData(String.Empty, s, Token) : ProgressPre.Done()
                End If
                If WaitNotificationMode = WNM.SkipTemp Or WaitNotificationMode = WNM.SkipCurrent Then WaitNotificationMode = WNM.Notify
            Catch eex As ExitException
            Catch ex As Exception
                errorFound = True
                Throw ex
            Finally
                E560Thrown = False
                UpdateResponser()
                ValidateExtension()
                If Not errorFound Then LoadSavePostsKV(False)
            End Try
        End Sub
        Private Sub ValidateExtension()
            Try
                Const heic$ = "heic"
                If _TempMediaList.Count > 0 AndAlso _TempMediaList.Exists(Function(mm) mm.File.Extension = heic) Then
                    Dim m As UserMedia
                    For i% = 0 To _TempMediaList.Count - 1
                        m = _TempMediaList(i)
                        If m.Type = UTypes.Picture AndAlso Not m.File.Extension.IsEmptyString AndAlso m.File.Extension = heic Then _
                           m.File.Extension = "jpg" : _TempMediaList(i) = m
                    Next
                End If
            Catch ex As Exception
            End Try
        End Sub
        Private Sub UpdateResponser()
            Try
                If _DownloadingInProgress AndAlso Not Responser Is Nothing AndAlso Not Responser.Disposed Then
                    _DownloadingInProgress = False
                    RemoveHandler Responser.ResponseReceived, AddressOf Responser_ResponseReceived
                    Declarations.UpdateResponser(Responser, MySiteSettings.Responser)
                End If
            Catch
            End Try
        End Sub
        Private Sub Responser_ResponseReceived(ByVal Sender As Object, ByVal e As EventArguments.WebDataResponse)
            Declarations.UpdateResponser(e, Responser)
        End Sub
        Private Enum Sections : Timeline : Tagged : Stories : SavedPosts : End Enum
        Private Const StoriesFolder As String = "Stories"
        Private Const TaggedFolder As String = "Tagged"
#Region "429 bypass"
        Private Const MaxPostsCount As Integer = 200
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
                                               New MsgBoxButton("Wait (disable current)") With {.ToolTip = "Wait and skip future prompts while downloading the current profile."},
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
                    If Not ProgressTempSet Then Progress.InformationTemporary = $"Waiting until { .GetWaitDate().ToString(DateTimeDefaultProvider)}"
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
#Region "Tags"
        Private TaggedChecked As Boolean = False
        Friend TaggedCheckSession As Boolean = True
        Private DownloadTagsLimit As Integer? = Nothing
        Private ReadOnly Property TaggedLimitsNotifications(ByVal v As Integer) As Boolean
            Get
                Return Not TaggedChecked AndAlso TaggedCheckSession AndAlso
                       CInt(MySiteSettings.TaggedNotifyLimit.Value) > 0 AndAlso v > CInt(MySiteSettings.TaggedNotifyLimit.Value)
            End Get
        End Property
        Private Function SetTagsLimit(ByVal Max As Integer, ByVal p As ANumbers) As DialogResult
            Dim v%?
            Dim aStr$ = $"Enter the number of posts from user {ToString()} that you want to download{vbCr}" &
                        $"(Max: {Max.NumToString(p)}; Requests: {(Max / 12).RoundUp.NumToString(p)})"
            Dim tryBtt As New MsgBoxButton("Try again") With {.ToolTip = "You will be asked again about the limit"}
            Dim cancelBtt As New MsgBoxButton("Cancel") With {.ToolTip = "Cancel tagged posts download operation"}
            Dim selectBtt As New MsgBoxButton("Other options") With {.ToolTip = "The main message with options will be displayed again"}
            Dim m As New MMessage("You have not entered a valid posts limit", "Tagged posts download limit", {tryBtt, selectBtt, cancelBtt})
            Dim mh As New MMessage("", "Tagged posts download limit", {"Confirm", tryBtt, selectBtt, cancelBtt}) With {.ButtonsPerRow = 2}
            Do
                v = AConvert(Of Integer)(InputBoxE(aStr, "Tagged posts download limit", CInt(MySiteSettings.TaggedNotifyLimit.Value)), AModes.Var, Nothing)
                If v.HasValue Then
                    mh.Text = $"You have entered a limit of {v.Value.NumToString(p)} posts"
                    Select Case MsgBoxE(mh).Index
                        Case 0 : DownloadTagsLimit = v : Return DialogResult.OK
                        Case 1 : v = Nothing
                        Case 2 : Return DialogResult.Retry
                        Case 3 : Return DialogResult.Cancel
                    End Select
                Else
                    Select Case MsgBoxE(m).Index
                        Case 1 : Return DialogResult.Retry
                        Case 2 : Return DialogResult.Cancel
                    End Select
                End If
            Loop While Not v.HasValue
            Return DialogResult.Retry
        End Function
        Private Function TaggedContinue(ByVal TaggedCount As Integer) As DialogResult
            Dim agi As New ANumbers With {.FormatOptions = ANumbers.Options.GroupIntegral}
            Dim msg As New MMessage($"The number of already downloaded tagged posts by user [{ToString()}] is {TaggedCount.NumToString(agi)}" & vbCr &
                                    "There is currently no way to know how many posts exist." & vbCr &
                                    "One request will be spent per post." & vbCr &
                                    "The tagged data download operation can take a long time.",
                                    "Too much tagged data",
                                    {
                                        "Continue",
                                        New MsgBoxButton("Continue unnotified") With {
                                            .ToolTip = "Continue downloading and cancel further notifications in the current downloading session."},
                                        New MsgBoxButton("Limit") With {
                                            .ToolTip = "Enter the limit of posts you want to download."},
                                        New MsgBoxButton("Disable and cancel") With {
                                            .ToolTip = "Disable downloading tagged data and cancel downloading tagged data."},
                                        "Cancel"
                                    }, MsgBoxStyle.Exclamation) With {.DefaultButton = 0, .CancelButton = 4, .ButtonsPerRow = 2}
            Do
                Select Case MsgBoxE(msg).Index
                    Case 0 : Return DialogResult.OK
                    Case 1 : TaggedCheckSession = False : Return DialogResult.OK
                    Case 2
                        Select Case SetTagsLimit(TaggedCount, agi)
                            Case DialogResult.OK : Return DialogResult.OK
                            Case DialogResult.Cancel : Return DialogResult.Cancel
                        End Select
                    Case 3 : GetTaggedData = False : Return DialogResult.Cancel
                    Case 4 : Return DialogResult.Cancel
                End Select
            Loop
        End Function
#End Region
        Private Overloads Sub DownloadData(ByVal Cursor As String, ByVal Section As Sections, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Dim StoriesList As List(Of String) = Nothing
            Dim StoriesRequested As Boolean = False
            Dim dValue% = 1
            LastCursor = Cursor
            Try
                Do While dValue = 1
                    ThrowAny(Token)
                    If Not Ready() Then Thread.Sleep(10000) : ThrowAny(Token) : Continue Do
                    ReconfigureAwaiter()

                    Try
                        Dim n As EContainer, nn As EContainer
                        Dim HasNextPage As Boolean = False
                        Dim EndCursor$ = String.Empty
                        Dim PostID$ = String.Empty, PostDate$ = String.Empty, SpecFolder$ = String.Empty
                        Dim PostIDKV As PostKV
                        Dim ENode() As Object = Nothing
                        NextRequest(True)

                        'Check environment
                        If Not IsSavedPosts Then
                            If ID.IsEmptyString Then GetUserId()
                            If ID.IsEmptyString Then Throw New Plugin.ExitException("can't get user ID")
                        End If

                        'Create query
                        Select Case Section
                            Case Sections.Timeline
                                URL = $"https://www.instagram.com/api/v1/feed/user/{NameTrue}/username/?count=50" &
                                        If(Cursor.IsEmptyString, String.Empty, $"&max_id={Cursor}")
                                ENode = Nothing
                            Case Sections.SavedPosts
                                SavedPostsDownload(String.Empty, Token)
                                Exit Sub
                            Case Sections.Tagged
                                Dim h$ = AConvert(Of String)(MySiteSettings.HashTagged.Value, String.Empty)
                                If h.IsEmptyString Then Throw New ExitException
                                Dim vars$ = "{""id"":" & ID & ",""first"":50,""after"":""" & Cursor & """}"
                                vars = SymbolsConverter.ASCII.EncodeSymbolsOnly(vars)
                                URL = $"https://www.instagram.com/graphql/query/?query_hash={h}&variables={vars}"
                                ENode = {"data", "user", 0}
                                SpecFolder = TaggedFolder
                            Case Sections.Stories
                                If Not StoriesRequested Then
                                    StoriesList = GetStoriesList()
                                    StoriesRequested = True
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
                                    Throw New ExitException
                                End If
                        End Select

                        'Get response
                        Dim r$ = Responser.GetResponse(URL,, EDP.ThrowException)
                        MySiteSettings.TooManyRequests(False)
                        RequestsCount += 1
                        ThrowAny(Token)

                        'Parsing
                        If Not r.IsEmptyString Then
                            Using j As EContainer = JsonDocument.Parse(r).XmlIfNothing
                                n = If(ENode Is Nothing, j, j.ItemF(ENode)).XmlIfNothing
                                If n.Count > 0 Then
                                    Select Case Section
                                        Case Sections.Timeline
                                            With n
                                                HasNextPage = .Value("more_available").FromXML(Of Boolean)(False)
                                                EndCursor = .Value("next_max_id")
                                                If If(.Item("items")?.Count, 0) > 0 Then
                                                    UserSiteNameUpdate(.ItemF({"items", 0, "user", "full_name"}).XmlIfNothingValue)
                                                    If Not DefaultParser(.Item("items"), Section, Token) Then Throw New ExitException
                                                Else
                                                    HasNextPage = False
                                                End If
                                            End With
                                        Case Sections.Tagged
                                            With n
                                                If .Contains("page_info") Then
                                                    With .Item("page_info")
                                                        HasNextPage = .Value("has_next_page").FromXML(Of Boolean)(False)
                                                        EndCursor = .Value("end_cursor")
                                                    End With
                                                Else
                                                    HasNextPage = False
                                                End If
                                                If If(.Item("edges")?.Count, 0) > 0 Then
                                                    ProgressPre.ChangeMax(.Item("edges").Count)
                                                    For Each nn In .Item("edges")
                                                        ProgressPre.Perform()
                                                        PostIDKV = New PostKV(Section)
                                                        If nn.Count > 0 AndAlso nn(0).Count > 0 Then
                                                            With nn(0)
                                                                PostIDKV = New PostKV(.Value("shortcode"), .Value("id"), Section)
                                                                If PostKvExists(PostIDKV) Then
                                                                    Throw New ExitException
                                                                Else
                                                                    If Not DownloadTagsLimit.HasValue OrElse PostsToReparse.Count + 1 < DownloadTagsLimit.Value Then
                                                                        _TempPostsList.Add(GetPostIdBySection(PostIDKV.ID, Section))
                                                                        PostsKVIDs.ListAddValue(PostIDKV, LAP.NotContainsOnly)
                                                                        PostsToReparse.ListAddValue(PostIDKV, LNC)
                                                                    ElseIf DownloadTagsLimit.HasValue OrElse PostsToReparse.Count + 1 >= DownloadTagsLimit.Value Then
                                                                        Throw New ExitException
                                                                    End If
                                                                End If
                                                            End With
                                                        End If
                                                    Next
                                                Else
                                                    HasNextPage = False
                                                End If
                                            End With
                                            If TaggedLimitsNotifications(PostsToReparse.Count) Then
                                                TaggedChecked = True
                                                If TaggedContinue(PostsToReparse.Count) = DialogResult.Cancel Then Throw New ExitException
                                            End If
                                    End Select
                                Else
                                    If j.Value("status") = "ok" AndAlso If(j("items")?.Count, 0) = 0 AndAlso
                                       _TempMediaList.Count = 0 AndAlso Section = Sections.Timeline Then _
                                       UserExists = False : Throw New ExitException
                                End If
                            End Using
                        Else
                            Throw New ExitException
                        End If
                        dValue = 0
                        If HasNextPage And Not EndCursor.IsEmptyString Then DownloadData(EndCursor, Section, Token)
                    Catch eex As ExitException
                        Throw eex
                    Catch ex As Exception
                        dValue = ProcessException(ex, Token, $"data downloading error [{URL}]",, Section, False)
                    End Try
                Loop
            Catch eex2 As ExitException
                If (Section = Sections.Timeline Or Section = Sections.Tagged) And Not Cursor.IsEmptyString Then Throw eex2
            Catch oex2 As OperationCanceledException When Token.IsCancellationRequested Or oex2.HelpLink = InstAborted
                If oex2.HelpLink = InstAborted Then HasError = True
            Catch DoEx As Exception
                ProcessException(DoEx, Token, $"data downloading error [{URL}]",, Section)
            End Try
        End Sub
        Private Sub DownloadPosts(ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Dim dValue% = 1
            Dim _Index% = 0
            If PostsToReparse.Count > 0 Then ProgressPre.ChangeMax(PostsToReparse.Count)
            Try
                Do While dValue = 1
                    ThrowAny(Token)
                    If Not Ready() Then Thread.Sleep(10000) : ThrowAny(Token) : Continue Do
                    ReconfigureAwaiter()

                    Try
                        Dim r$
                        Dim j As EContainer, jj As EContainer
                        If PostsToReparse.Count > 0 And _Index <= PostsToReparse.Count - 1 Then
                            Dim e As New ErrorsDescriber(EDP.ThrowException)
                            If Index > 0 Then ProgressPre.ChangeMax(1)
                            For i% = _Index To PostsToReparse.Count - 1
                                ProgressPre.Perform()
                                _Index = i
                                URL = $"https://www.instagram.com/api/v1/media/{PostsToReparse(i).ID}/info/"
                                ThrowAny(Token)
                                NextRequest(((i + 1) Mod 5) = 0)
                                ThrowAny(Token)
                                r = Responser.GetResponse(URL,, e)
                                MySiteSettings.TooManyRequests(False)
                                RequestsCount += 1
                                If Not r.IsEmptyString Then
                                    j = JsonDocument.Parse(r)
                                    If Not j Is Nothing Then
                                        If If(j("items")?.Count, 0) > 0 Then
                                            With j("items")
                                                For Each jj In .Self : ObtainMedia(jj, PostsToReparse(i).ID) : Next
                                            End With
                                        End If
                                        j.Dispose()
                                    End If
                                End If
                            Next
                        End If
                        dValue = 0
                    Catch eex As ExitException
                        Throw eex
                    Catch ex As Exception
                        dValue = ProcessException(ex, Token, $"downloading posts error [{URL}]",, Sections.Tagged, False)
                    End Try
                Loop
            Catch eex2 As ExitException
            Catch oex2 As OperationCanceledException When Token.IsCancellationRequested Or oex2.HelpLink = InstAborted
                If oex2.HelpLink = InstAborted Then HasError = True
            Catch DoEx As Exception
                ProcessException(DoEx, Token, $"downloading posts error [{URL}]",, Sections.Tagged)
            End Try
        End Sub
        Private Sub SavedPostsDownload(ByVal Cursor As String, ByVal Token As CancellationToken)
            Dim URL$ = $"https://www.instagram.com/api/v1/feed/saved/posts/?max_id={Cursor}"
            Dim HasNextPage As Boolean = False
            Dim NextCursor$ = String.Empty
            ThrowAny(Token)
            Dim r$ = Responser.GetResponse(URL)
            Dim nodes As IEnumerable(Of EContainer) = Nothing
            If Not r.IsEmptyString Then
                Using e As EContainer = JsonDocument.Parse(r)
                    If If(e?.Count, 0) > 0 Then
                        With e
                            HasNextPage = .Value("more_available").FromXML(Of Boolean)(False)
                            NextCursor = .Value("next_max_id")
                            If .Contains("items") Then nodes = (From ee As EContainer In .Item("items") Where ee.Count > 0 Select ee(0))
                        End With
                        If nodes.ListExists Then
                            DefaultParser(nodes, Sections.SavedPosts, Token)
                            If HasNextPage And Not NextCursor.IsEmptyString Then SavedPostsDownload(NextCursor, Token)
                        End If
                    End If
                End Using
            End If
        End Sub
        Private Function DefaultParser(ByVal Items As IEnumerable(Of EContainer), ByVal Section As Sections, ByVal Token As CancellationToken,
                                       Optional ByVal SpecFolder As String = Nothing) As Boolean
            ThrowAny(Token)
            If Items.Count > 0 Then
                Dim PostIDKV As PostKV
                Dim Pinned As Boolean
                Dim PostDate$
                If SpecFolder.IsEmptyString Then
                    Select Case Section
                        Case Sections.Tagged : SpecFolder = TaggedFolder
                        Case Sections.Stories : SpecFolder = StoriesFolder
                        Case Else : SpecFolder = String.Empty
                    End Select
                End If
                ProgressPre.ChangeMax(Items.Count)
                For Each nn In Items
                    ProgressPre.Perform()
                    With nn
                        PostIDKV = New PostKV(.Value("code"), .Value("id"), Section)
                        Pinned = .Contains("timeline_pinned_user_ids")
                        If PostKvExists(PostIDKV) Then
                            If Not Pinned Then Return False
                        Else
                            _TempPostsList.Add(PostIDKV.ID)
                            PostsKVIDs.ListAddValue(PostIDKV, LNC)
                            PostDate = .Value("taken_at")
                            If Not IsSavedPosts Then
                                Select Case CheckDatesLimit(PostDate, UnixDate32Provider)
                                    Case DateResult.Skip : Continue For
                                    Case DateResult.Exit : If Not Pinned Then Return False
                                End Select
                            End If
                            ObtainMedia(.Self, PostIDKV.ID, SpecFolder, PostDate)
                        End If
                    End With
                Next
                Return True
            Else
                Return False
            End If
        End Function
#End Region
#Region "Code ID converters"
        Private Function CodeToID(ByVal Code As String) As String
            Const CodeSymbols$ = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789-_"
            Try
                If Not Code.IsEmptyString Then
                    Dim c As Char
                    Dim id& = 0
                    For i% = 0 To Code.Length - 1
                        c = Code(i)
                        id = (id * 64) + CodeSymbols.IndexOf(c)
                    Next
                    Return id
                Else
                    Return String.Empty
                End If
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, $"[API.Instagram.UserData.CodeToID({Code})", String.Empty)
            End Try
        End Function
#End Region
#Region "Obtain Media"
        Private Sub ObtainMedia(ByVal n As EContainer, ByVal PostID As String, Optional ByVal SpecialFolder As String = Nothing,
                                Optional ByVal DateObj As String = Nothing)
            Try
                Dim img As Predicate(Of EContainer) = Function(_img) Not _img.Name.IsEmptyString AndAlso _img.Name.StartsWith("image_versions") AndAlso _img.Count > 0
                Dim vid As Predicate(Of EContainer) = Function(_vid) Not _vid.Name.IsEmptyString AndAlso _vid.Name.StartsWith("video_versions") AndAlso _vid.Count > 0
                Dim ss As Func(Of EContainer, Sizes) = Function(_ss) New Sizes(_ss.Value("width"), _ss.Value("url"))
                Dim mDate As Func(Of EContainer, String) = Function(ByVal elem As EContainer) As String
                                                               If Not DateObj.IsEmptyString Then Return DateObj
                                                               If elem.Contains("taken_at") Then
                                                                   Return elem.Value("taken_at")
                                                               ElseIf elem.Contains("imported_taken_at") Then
                                                                   Return elem.Value("imported_taken_at")
                                                               Else
                                                                   Dim ev$ = elem.Value("device_timestamp")
                                                                   If Not ev.IsEmptyString Then
                                                                       If ev.Length > 10 Then
                                                                           Return ev.Substring(0, 10)
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
                                        For Each d In .Self : ObtainMedia(d, PostID, SpecialFolder, DateObj) : Next
                                    End If
                                End With
                        End Select
                    End If
                    l.Clear()
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "API.Instagram.ObtainMedia2")
            End Try
        End Sub
#End Region
#Region "GetUserId, GetUserName"
        Private Sub GetUserId()
            Dim __idFound As Boolean = False
            Try
                RequestsCount += 1
                Dim r$ = Responser.GetResponse($"https://i.instagram.com/api/v1/users/web_profile_info/?username={NameTrue}",, EDP.ThrowException)
                If Not r.IsEmptyString Then
                    Using j As EContainer = JsonDocument.Parse(r)
                        If Not j Is Nothing AndAlso j.Contains({"data", "user"}) Then
                            With j({"data", "user"})
                                ID = .Value("id")
                                _ForceSaveUserData = True
                                __idFound = True
                                UserSiteNameUpdate(.Value("full_name"))
                                Dim descr$ = .Value("biography")
                                If If(.Item("bio_links")?.Count, 0) > 0 Then descr.StringAppend(.Item("bio_links").Select(Function(bl) bl.Value("url")).ListToString(vbNewLine), vbNewLine)
                                Dim eUrl$ = .Value("external_url")
                                If Not eUrl.IsEmptyString AndAlso (descr.IsEmptyString OrElse Not descr.Contains(eUrl)) Then descr.StringAppendLine(eUrl)
                                UserDescriptionUpdate(descr)
                                Dim f As New SFile With {.Path = MyFile.CutPath.Path, .Name = "ProfilePicture", .Extension = "jpg"}
                                If Not f.Exists Then
                                    Dim profilePicture$ = .Value("profile_pic_url_hd")
                                    If profilePicture.IsEmptyString OrElse Not GetWebFile(profilePicture, f, EDP.ReturnValue) Then
                                        profilePicture = .Value("profile_pic_url")
                                        If Not profilePicture.IsEmptyString Then GetWebFile(profilePicture, f, EDP.ReturnValue)
                                    End If
                                End If
                            End With
                        End If
                    End Using
                End If
            Catch ex As Exception
                If Not __idFound Then
                    If Responser.StatusCode = HttpStatusCode.NotFound Or Responser.StatusCode = HttpStatusCode.BadRequest Then
                        Throw ex
                    Else
                        LogError(ex, "get Instagram user ID")
                    End If
                End If
            End Try
        End Sub
        Private Function GetUserNameById() As Boolean
            UserNameRequested = True
            Try
                If Not ID.IsEmptyString Then
                    RequestsCount += 1
                    Dim r$ = Responser.GetResponse($"https://i.instagram.com/api/v1/users/{ID}/info/",, EDP.ReturnValue)
                    If Not r.IsEmptyString Then
                        Using j As EContainer = JsonDocument.Parse(r, EDP.ReturnValue)
                            If j.ListExists Then
                                Dim newName$ = j.Value({"user"}, "username")
                                If Not newName.IsEmptyString Then
                                    Dim oldName$ = NameTrue
                                    If Not newName = oldName Then
                                        MyMainLOG = $"{ToStringForLog()}: username changed from '{oldName}' to '{newName}'"
                                        _NameTrue = newName
                                        Dim descr$ = $"Username changed from '{oldName}' to '{newName}' ({Now.ToStringDate(ADateTime.Formats.BaseDateTime)})!"
                                        descr.StringAppendLine(UserDescription)
                                        UserDescription = descr
                                        _ForceSaveUserData = True
                                    End If
                                    Return True
                                End If
                            End If
                        End Using
                    End If
                End If
                Return False
            Catch ex As Exception
                LogError(ex, "get Instagram user name by ID")
                Return False
            End Try
        End Function
#End Region
#Region "Pinned stories"
        Private Sub GetStoriesData(ByRef StoriesList As List(Of String), ByVal Token As CancellationToken)
            Const ReqUrl$ = "https://i.instagram.com/api/v1/feed/reels_media/?{0}"
            Dim tmpList As IEnumerable(Of String)
            Dim qStr$, r$, sFolder$, storyID$, pid$
            Dim i% = -1
            Dim jj As EContainer, s As EContainer
            ThrowAny(Token)
            If StoriesList.ListExists Then
                tmpList = StoriesList.Take(5)
                If tmpList.ListExists Then
                    qStr = String.Format(ReqUrl, tmpList.Select(Function(q) $"reel_ids=highlight:{q}").ListToString("&"))
                    r = Responser.GetResponse(qStr,, EDP.ThrowException)
                    ThrowAny(Token)
                    If Not r.IsEmptyString Then
                        Using j As EContainer = JsonDocument.Parse(r).XmlIfNothing
                            If j.Contains("reels") Then
                                ProgressPre.ChangeMax(j("reels").Count)
                                For Each jj In j("reels")
                                    ProgressPre.Perform()
                                    i += 1
                                    sFolder = jj.Value("title").StringRemoveWinForbiddenSymbols
                                    storyID = jj.Value("id").Replace("highlight:", String.Empty)
                                    If sFolder.IsEmptyString Then sFolder = $"Story_{storyID}"
                                    If sFolder.IsEmptyString Then sFolder = $"Story_{i}"
                                    sFolder = $"{StoriesFolder}\{sFolder}"
                                    If Not storyID.IsEmptyString Then storyID &= ":"
                                    With jj("items").XmlIfNothing
                                        If .Count > 0 Then
                                            For Each s In .Self
                                                pid = storyID & s.Value("id")
                                                If Not _TempPostsList.Contains(pid) Then
                                                    ThrowAny(Token)
                                                    ObtainMedia(s, pid, sFolder)
                                                    _TempPostsList.Add(pid)
                                                End If
                                            Next
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
                DownloadingException(ex, "API.Instagram.GetStoriesList", False, Sections.Stories)
                Return Nothing
            End Try
        End Function
#End Region
#Region "Download content"
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
#End Region
#Region "Exceptions"
        ''' <exception cref="ExitException"></exception>
        ''' <inheritdoc cref="UserDataBase.ThrowAny(CancellationToken)"/>
        Friend Overrides Sub ThrowAny(ByVal Token As CancellationToken)
            If MySiteSettings.SkipUntilNextSession Then ExitException.Throw560(Me)
            MyBase.ThrowAny(Token)
        End Sub
        ''' <summary>
        ''' <inheritdoc cref="UserDataBase.DownloadingException(Exception, String, Boolean, Object)"/><br/>
        ''' 1 - continue
        ''' </summary>
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal s As Object = Nothing) As Integer
            If Responser.StatusCode = HttpStatusCode.NotFound Then
                If Not UserNameRequested AndAlso GetUserNameById() Then Return 1 Else UserExists = False
            ElseIf Responser.StatusCode = HttpStatusCode.BadRequest Then
                HasError = True
                MyMainLOG = $"Instagram credentials have expired [{CInt(Responser.StatusCode)}]: {ToStringForLog()} [{s}]"
                DisableSection(s)
            ElseIf Responser.StatusCode = HttpStatusCode.Forbidden And s = Sections.Tagged Then
                Return 3
            ElseIf Responser.StatusCode = 429 Then
                With MySiteSettings
                    Dim WaiterExists As Boolean = .LastApplyingValue.HasValue
                    .TooManyRequests(True)
                    If Not WaiterExists Then .LastApplyingValue = 2
                End With
                Caught429 = True
                MyMainLOG = $"Number of requests before error 429: {RequestsCount}"
                Return 1
            ElseIf Responser.StatusCode = 560 Then
                MySiteSettings.SkipUntilNextSession = True
            Else
                MyMainLOG = $"Something is wrong. Your credentials may have expired [{CInt(Responser.StatusCode)}]: {ToString()} [{s}]"
                DisableSection(s)
                If Not FromPE Then LogError(ex, Message) : HasError = True
                Return 0
            End If
            Return 2
        End Function
        Private Sub DisableSection(ByVal Section As Object)
            If Not IsNothing(Section) AndAlso TypeOf Section Is Sections Then
                Dim s As Sections = DirectCast(Section, Sections)
                Select Case s
                    Case Sections.Timeline : MySiteSettings.DownloadTimeline.Value = False
                    Case Else : MySiteSettings.DownloadTagged.Value = False
                End Select
                MyMainLOG = $"[{s}] downloading is disabled until you update your credentials".ToUpper
            End If
        End Sub
#End Region
#Region "Create media"
        Private Function MediaFromData(ByVal t As UTypes, ByVal _URL As String, ByVal PostID As String, ByVal PostDate As String,
                                       Optional ByVal SpecialFolder As String = Nothing) As UserMedia
            _URL = LinkFormatterSecure(RegexReplace(_URL.Replace("\", String.Empty), LinkPattern))
            Dim m As New UserMedia(_URL, t) With {.Post = New UserPost With {.ID = PostID}}
            If Not m.URL.IsEmptyString Then m.File = CStr(RegexReplace(m.URL, FilesPattern))
            If Not PostDate.IsEmptyString Then m.Post.Date = AConvert(Of Date)(PostDate, UnixDate32Provider, Nothing) Else m.Post.Date = Nothing
            m.SpecialFolder = SpecialFolder
            Return m
        End Function
#End Region
#Region "Standalone downloader"
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            Dim PID$ = RegexReplace(Data.URL, RParams.DMS(".*?instagram.com/p/([_\w\d]+)", 1))
            If Not PID.IsEmptyString AndAlso Not ACheck(Of Long)(PID) Then PID = CodeToID(PID)
            If Not PID.IsEmptyString Then
                PostsToReparse.Add(New PostKV With {.ID = PID})
                DownloadPosts(Token)
            End If
        End Sub
#End Region
#Region "IDisposable Support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                UpdateResponser()
                If disposing Then
                    PostsKVIDs.Clear()
                    PostsToReparse.Clear()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace