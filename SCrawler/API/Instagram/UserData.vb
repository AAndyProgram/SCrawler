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
Imports PersonalUtilities.Tools.Web.Clients.Base
Imports PersonalUtilities.Tools.Web.Documents
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Imports UStates = SCrawler.API.Base.UserMedia.States
Namespace API.Instagram
    Friend Class UserData : Inherits UserDataBase
#Region "XML Names"
        Private Const Name_LastCursor As String = "LastCursor"
        Private Const Name_FirstLoadingDone As String = "FirstLoadingDone"
        Private Const Name_GetTimeline As String = "GetTimeline"
        Private Const Name_GetReels As String = "GetReels"
        Private Const Name_GetStories As String = "GetStories"
        Private Const Name_GetStoriesUser As String = "GetStoriesUser"
        Private Const Name_GetTagged As String = "GetTaggedData"
        Private Const Name_TaggedChecked As String = "TaggedChecked"
        Private Const Name_NameTrue As String = "NameTrue"
#End Region
#Region "Declarations"
        Protected Structure PostKV : Implements IEContainerProvider
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
        Friend Const Header_FB_LSD As String = "x-fb-lsd"
        Private ReadOnly Property MySiteSettings As SiteSettings
            Get
                Return DirectCast(HOST.Source, SiteSettings)
            End Get
        End Property
        Protected ReadOnly PostsKVIDs As List(Of PostKV)
        Private ReadOnly PostsToReparse As List(Of PostKV)
        Private LastCursor As String = String.Empty
        Private FirstLoadingDone As Boolean = False
        Friend Property GetTimeline As Boolean = True
        Friend Property GetReels As Boolean = False
        Friend Property GetStories As Boolean
        Friend Property GetStoriesUser As Boolean
        Friend Property GetTaggedData As Boolean
        Protected _NameTrue As String = String.Empty
        Friend ReadOnly Property NameTrue As String
            Get
                Return _NameTrue.IfNullOrEmpty(Name)
            End Get
        End Property
        Private UserNameRequested As Boolean = False
#End Region
#Region "Loader"
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            With Container
                If Loading Then
                    LastCursor = .Value(Name_LastCursor)
                    FirstLoadingDone = .Value(Name_FirstLoadingDone).FromXML(Of Boolean)(False)
                    GetTimeline = .Value(Name_GetTimeline).FromXML(Of Boolean)(CBool(MySiteSettings.GetTimeline.Value))
                    GetReels = .Value(Name_GetReels).FromXML(Of Boolean)(MySiteSettings.GetReels.Value)
                    GetStories = .Value(Name_GetStories).FromXML(Of Boolean)(CBool(MySiteSettings.GetStories.Value))
                    GetStoriesUser = .Value(Name_GetStoriesUser).FromXML(Of Boolean)(MySiteSettings.GetStoriesUser.Value)
                    GetTaggedData = .Value(Name_GetTagged).FromXML(Of Boolean)(CBool(MySiteSettings.GetTagged.Value))
                    TaggedChecked = .Value(Name_TaggedChecked).FromXML(Of Boolean)(False)
                    _NameTrue = .Value(Name_NameTrue)
                Else
                    .Add(Name_LastCursor, LastCursor)
                    .Add(Name_FirstLoadingDone, FirstLoadingDone.BoolToInteger)
                    .Add(Name_GetTimeline, GetTimeline.BoolToInteger)
                    .Add(Name_GetReels, GetReels.BoolToInteger)
                    .Add(Name_GetStories, GetStories.BoolToInteger)
                    .Add(Name_GetStoriesUser, GetStoriesUser.BoolToInteger)
                    .Add(Name_GetTagged, GetTaggedData.BoolToInteger)
                    .Add(Name_TaggedChecked, TaggedChecked.BoolToInteger)
                    .Add(Name_NameTrue, _NameTrue)
                End If
            End With
        End Sub
#End Region
#Region "Exchange options"
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return New EditorExchangeOptions(Me)
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            If Not Obj Is Nothing AndAlso TypeOf Obj Is EditorExchangeOptions Then
                With DirectCast(Obj, EditorExchangeOptions)
                    GetTimeline = .GetTimeline
                    GetReels = .GetReels
                    GetStories = .GetStories
                    GetStoriesUser = .GetStoriesUser
                    GetTaggedData = .GetTagged
                End With
            End If
        End Sub
#End Region
#Region "Initializer"
        Friend Sub New()
            PostsKVIDs = New List(Of PostKV)
            PostsToReparse = New List(Of PostKV)
            _ResponserAutoUpdateCookies = True
        End Sub
#End Region
#Region "Download data"
        Private E560Thrown As Boolean = False
        Friend Err5xx As Integer = -1
        Private Class ExitException : Inherits Exception
            Friend Property Is560 As Boolean = False
            Friend Shared Sub Throw560(ByRef Source As UserData)
                If Not Source.E560Thrown Then
                    MyMainLOG = $"{Source.ToStringForLog}: ({IIf(Source.Err5xx > 0, Source.Err5xx, 560)}) Download skipped until next session"
                    Source.E560Thrown = True
                End If
                Throw New ExitException With {.Is560 = True}
            End Sub
        End Class
        Private ReadOnly Property MyFilePostsKV As SFile
            Get
                Dim f As SFile = MyFilePosts
                If Not f.IsEmptyString Then
                    f.Name &= "_KV"
                    f.Extension = "xml"
                    Return f
                Else
                    Return Nothing
                End If
            End Get
        End Property
        Protected Sub LoadSavePostsKV(ByVal Load As Boolean)
            Dim x As XmlFile
            Dim f As SFile = MyFilePostsKV
            If Not f.IsEmptyString Then
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
        Protected Overloads Function PostKvExists(ByVal pkv As PostKV) As Boolean
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
                    Dim f As SFile = MyFilePostsKV
                    If Not f.IsEmptyString Then
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
        Private _Limit As Integer = -1
        Private _TotalPostsParsed As Integer = 0
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            UserNameRequested = False
            Dim s As Sections = Sections.Timeline
            Dim errorFound As Boolean = False
            Try
                Err5xx = -1
                _Limit = If(DownloadTopCount, -1)
                _TotalPostsParsed = 0
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
                    If CBool(MySiteSettings.DownloadReels.Value) And GetReels Then
                        s = Sections.Reels
                        DefaultParser_ElemNode = {"node", "media"}
                        DownloadData(String.Empty, s, Token)
                        DefaultParser_ElemNode = Nothing
                        DownloadReels_SetEnvir = False
                        ProgressPre.Done()
                    End If
                    If CBool(MySiteSettings.DownloadStories.Value) And GetStories Then s = Sections.Stories : DownloadData(String.Empty, s, Token) : ProgressPre.Done()
                    If CBool(MySiteSettings.DownloadStoriesUser.Value) And GetStoriesUser Then s = Sections.UserStories : DownloadData(String.Empty, s, Token) : ProgressPre.Done()
                    If CBool(MySiteSettings.DownloadTagged.Value) And GetTaggedData Then
                        s = Sections.Tagged
                        DownloadData(String.Empty, s, Token)
                        ProgressPre.Done()
                        If PostsToReparse.Count > 0 Then DownloadPosts(Token, True)
                    End If
                End If
                If WaitNotificationMode = WNM.SkipTemp Or WaitNotificationMode = WNM.SkipCurrent Then WaitNotificationMode = WNM.Notify
            Catch eex As ExitException
            Catch ex As Exception
                errorFound = True
                Throw ex
            Finally
                DefaultParser_ElemNode = Nothing
                DownloadReels_SetEnvir = False
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
        Protected Overridable Sub UpdateResponser()
            Try
                If _DownloadingInProgress AndAlso Not Responser Is Nothing AndAlso Not Responser.Disposed Then
                    _DownloadingInProgress = False
                    Responser_ResponseReceived_RemoveHandler()
                    Declarations.UpdateResponser(Responser, MySiteSettings.Responser)
                End If
            Catch
            End Try
        End Sub
        Protected Overrides Sub Responser_ResponseReceived(ByVal Sender As Object, ByVal e As EventArguments.WebDataResponse)
            Declarations.UpdateResponser(e, Responser)
        End Sub
        Protected Enum Sections : Timeline : Reels : Tagged : Stories : UserStories : SavedPosts : End Enum
        Protected Const StoriesFolder As String = "Stories"
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
                        Dim r$ = String.Empty
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
                            Case Sections.Reels
                                r = DownloadReels(Cursor, Token)
                                ENode = {"data", "xdt_api__v1__clips__user__connection_v2"}
                            Case Sections.SavedPosts
                                SavedPostsDownload(String.Empty, Token)
                                Exit Sub
                            Case Sections.Tagged
                                Dim vars$ = "{""id"":" & ID & ",""first"":50,""after"":""" & Cursor & """}"
                                vars = SymbolsConverter.ASCII.EncodeSymbolsOnly(vars)
                                URL = $"https://www.instagram.com/graphql/query/?doc_id=17946422347485809&variables={vars}"
                                ENode = {"data", "user", "edge_user_to_photos_of_you"}
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
                                    GetStoriesData(StoriesList, False, Token)
                                    MySiteSettings.TooManyRequests(False)
                                    RequestsCount += 1
                                End If
                                If StoriesList.ListExists Then
                                    Continue Do
                                Else
                                    Throw New ExitException
                                End If
                            Case Sections.UserStories
                                GetStoriesData(Nothing, True, Token)
                                MySiteSettings.TooManyRequests(False)
                                RequestsCount += 1
                                Throw New ExitException
                        End Select

                        'Get response
                        If Not Section = Sections.Reels Then r = Responser.GetResponse(URL,, EDP.ThrowException)
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
                                        Case Sections.Reels
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
                                                    If Not DefaultParser(.Item("edges"), Section, Token, "Reels*") Then Throw New ExitException
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
                    Catch jsonNull As JsonDocumentException When jsonNull.State = WebDocumentEventArgs.States.Error And
                                                                 (Section = Sections.Reels Or Section = Sections.SavedPosts)
                        Throw jsonNull
                    Catch eex As ExitException
                        Throw eex
                    Catch ex As Exception
                        dValue = ProcessException(ex, Token, $"data downloading error [{URL}]",, Section, False)
                    End Try
                Loop
            Catch jsonNull2 As JsonDocumentException When jsonNull2.State = WebDocumentEventArgs.States.Error And
                                                          (Section = Sections.Reels Or Section = Sections.SavedPosts)
                If Section = Sections.SavedPosts Then DisableSection(Section)
            Catch eex2 As ExitException
                If eex2.Is560 Then
                    Throw New Plugin.ExitException With {.Silent = True}
                Else
                    If Not Section = Sections.Reels And (Section = Sections.Timeline Or Section = Sections.Tagged) And Not Cursor.IsEmptyString Then Throw eex2
                End If
            Catch oex2 As OperationCanceledException When Token.IsCancellationRequested Or oex2.HelpLink = InstAborted
                If oex2.HelpLink = InstAborted Then HasError = True
            Catch DoEx As Exception
                ProcessException(DoEx, Token, $"data downloading error [{URL}]",, Section)
            End Try
        End Sub
        Private Sub DownloadPosts(ByVal Token As CancellationToken, Optional ByVal IsTagged As Boolean = False)
            Dim URL$ = String.Empty
            Dim dValue% = 1
            Dim _Index% = 0
            Dim before%
            Dim specFolder$ = IIf(IsTagged, "Tagged", String.Empty)
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
                                                For Each jj In .Self
                                                    before = _TempMediaList.Count
                                                    ObtainMedia(jj, PostsToReparse(i).ID, specFolder)
                                                    If Not before = _TempMediaList.Count Then _TotalPostsParsed += 1
                                                    If _Limit > 0 And _TotalPostsParsed >= _Limit Then Throw New ExitException
                                                Next
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
                        If nodes.ListExists AndAlso DefaultParser(nodes, Sections.SavedPosts, Token) AndAlso
                           HasNextPage AndAlso Not NextCursor.IsEmptyString Then SavedPostsDownload(NextCursor, Token)
                    End If
                End Using
            End If
        End Sub
        Protected DefaultParser_ElemNode() As Object = Nothing
        Protected DefaultParser_IgnorePass As Boolean = False
        Private ReadOnly DefaultParser_PostUrlCreator_Default As Func(Of PostKV, String) = Function(post) $"https://www.instagram.com/p/{post.Code}/"
        Protected DefaultParser_PostUrlCreator As Func(Of PostKV, String) = Function(post) $"https://www.instagram.com/p/{post.Code}/"
        Protected Function DefaultParser(ByVal Items As IEnumerable(Of EContainer), ByVal Section As Sections, ByVal Token As CancellationToken,
                                         Optional ByVal SpecFolder As String = Nothing, Optional ByVal State As UStates = UStates.Unknown,
                                         Optional ByVal Attempts As Integer = 0) As Boolean
            ThrowAny(Token)
            If Items.Count > 0 Then
                Dim PostIDKV As PostKV
                Dim Pinned As Boolean
                Dim PostDate$, PostOriginUrl$
                Dim before%
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
                    With If(Not DefaultParser_ElemNode Is Nothing, nn.ItemF(DefaultParser_ElemNode), nn)
                        PostIDKV = New PostKV(.Value("code"), .Value("id"), Section)
                        PostOriginUrl = DefaultParser_PostUrlCreator(PostIDKV)
                        Pinned = .Contains("timeline_pinned_user_ids")
                        If Not DefaultParser_IgnorePass AndAlso PostKvExists(PostIDKV) Then
                            If Not Pinned Then Return False
                        Else
                            _TempPostsList.Add(PostIDKV.ID)
                            PostsKVIDs.ListAddValue(PostIDKV, LNC)
                            PostDate = .Value("taken_at")
                            If Not DefaultParser_IgnorePass And Not IsSavedPosts Then
                                Select Case CheckDatesLimit(PostDate, UnixDate32Provider)
                                    Case DateResult.Skip : Continue For
                                    Case DateResult.Exit : If Not Pinned Then Return False
                                End Select
                            End If
                            before = _TempMediaList.Count
                            ObtainMedia(.Self, PostIDKV.ID, SpecFolder, PostDate,, PostOriginUrl, State, Attempts)
                            If Not before = _TempMediaList.Count Then _TotalPostsParsed += 1
                            If _Limit > 0 And _TotalPostsParsed >= _Limit Then Return False
                        End If
                    End With
                Next
                Return True
            Else
                Return False
            End If
        End Function
#End Region
#Region "Get reels"
        Private _GetReels_LSD As String = String.Empty
        Private _GetReels_dtsg As String = String.Empty
        Private ReadOnly Property DownloadReels_Tokens_Valid As Boolean
            Get
                Return Not _GetReels_LSD.IsEmptyString And Not _GetReels_dtsg.IsEmptyString
            End Get
        End Property
        Private WriteOnly Property DownloadReels_SetEnvir As Boolean
            Set(ByVal init As Boolean)
                If init Then
                    ObtainMedia_SetReelsFunc()
                    DefaultParser_PostUrlCreator = Function(post) $"{MySiteSettings.GetUserUrl(Me).TrimEnd("/")}/reel/{post.Code}"
                Else
                    ObtainMedia_SizeFuncPic = Nothing
                    ObtainMedia_SizeFuncVid = Nothing
                    DefaultParser_PostUrlCreator = DefaultParser_PostUrlCreator_Default
                End If
            End Set
        End Property
        Private Class Responser2 : Inherits Responser
            Friend Sub New(ByVal Source As Responser)
                MyBase.New
                Copy(Source)
                ErrorProcessor = New ResponserErrorProcessor(Source)
            End Sub
        End Class
        ''' <returns>Response</returns>
        Private Function DownloadReels(ByVal Cursor As String, ByVal Token As CancellationToken) As String
            Const requestPattern$ = "https://www.instagram.com/api/graphql?fb_dtsg={0}&fb_api_req_friendly_name=PolarisProfileReelsTabContentQuery&lsd={1}&doc_id=7191572580905225&variables={2}"

            DownloadReels_SetEnvir = True

            If Cursor.IsEmptyString And Not DownloadReels_Tokens_Valid Then GetPageTokens()
            If Cursor.IsEmptyString And Not DownloadReels_Tokens_Valid Then Throw New ExitException

            Using resp As New Responser2(Responser)
                Try
                    resp.Method = "POST"
                    AddHandler resp.ResponseReceived, AddressOf Responser_ResponseReceived
                    resp.Headers.Add(Header_FB_LSD, _GetReels_LSD)

                    Dim vars$ = """data"":{""include_feed_video"":true,""page_size"":50,""target_user_id"":""" & ID & """}"
                    If Not Cursor.IsEmptyString Then vars = $"""after"":""{Cursor}"",""before"":null,{vars},""first"":4,""last"":null"
                    vars = "{" & vars & "}"

                    Dim url$ = String.Format(requestPattern, _GetReels_dtsg, _GetReels_LSD, SymbolsConverter.ASCII.EncodeSymbolsOnly(vars))

                    Return resp.GetResponse(url,, EDP.ThrowException)
                Finally
                    With resp
                        Responser.Cookies.Update(.Cookies)
                        With .Headers
                            If .Contains(SiteSettings.Header_IG_WWW_CLAIM) Then Responser.Headers.Add(SiteSettings.Header_IG_WWW_CLAIM, .Value(SiteSettings.Header_IG_WWW_CLAIM))
                            If .Contains(SiteSettings.Header_CSRF_TOKEN) Then Responser.Headers.Add(SiteSettings.Header_CSRF_TOKEN, .Value(SiteSettings.Header_CSRF_TOKEN))
                        End With
                    End With
                End Try
            End Using
        End Function
        Private Function GetPageTokens() As Boolean
            _GetReels_LSD = String.Empty
            _GetReels_dtsg = String.Empty
            Try
                Dim r$ = Responser.GetResponse(MySiteSettings.GetUserUrl(Me),, EDP.ThrowException)
                If Not r.IsEmptyString Then
                    Dim rr As RParams = RParams.DM(PageTokenRegexPatternDefault, 0, RegexReturn.List, EDP.ReturnValue)
                    Dim tokens As List(Of String) = RegexReplace(r, rr)
                    Dim tt$, ttVal$
                    If tokens.ListExists Then
                        With rr
                            .Match = Nothing
                            .MatchSub = 1
                            .WhatGet = RegexReturn.Value
                        End With
                        For Each tt In tokens
                            If Not _GetReels_LSD.IsEmptyString And Not _GetReels_dtsg.IsEmptyString Then
                                Exit For
                            Else
                                ttVal = RegexReplace(tt, rr)
                                If Not ttVal.IsEmptyString Then
                                    If ttVal.Contains(":") Then
                                        If _GetReels_dtsg.IsEmptyString Then _GetReels_dtsg = ttVal
                                    Else
                                        If _GetReels_LSD.IsEmptyString Then _GetReels_LSD = ttVal
                                    End If
                                End If
                            End If
                        Next
                    End If
                End If
            Catch ex As Exception
                Dim notFound$ = String.Empty
                If _GetReels_dtsg.IsEmptyString Then notFound.StringAppend(Header_FB_LSD)
                If _GetReels_LSD.IsEmptyString Then notFound.StringAppend("lsd")
                LogError(ex, $"failed to update some{IIf(notFound.IsEmptyString, String.Empty, $" ({notFound})")} credentials", EDP.SendToLog)
            End Try
            Return DownloadReels_Tokens_Valid
        End Function
#End Region
#Region "Code ID converters"
        Protected Function CodeToID(ByVal Code As String) As String
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
        Protected ObtainMedia_SizeFuncVid As Func(Of EContainer, Sizes) = Nothing
        Protected ObtainMedia_SizeFuncPic As Func(Of EContainer, Sizes) = Nothing
        Protected ObtainMedia_AllowAbstract As Boolean = False
        Protected Sub ObtainMedia_SetReelsFunc()
            ObtainMedia_SizeFuncPic = Function(ByVal ss As EContainer) As Sizes
                                          If ss.Value("url").IsEmptyString Then
                                              Return New Sizes("----", "")
                                          ElseIf Not ss.Value("width").IsEmptyString Or Not ss.Value("width").IsEmptyString Then
                                              Return New Sizes(CInt(AConvert(Of Integer)(ss.Value("width"), 0)) +
                                                               CInt(AConvert(Of Integer)(ss.Value("height"), 0)), ss.Value("url"))
                                          Else
                                              Dim rval$ = RegexReplace(ss.Value("url"), ObtainMedia_SizeFuncPic_RegexP)
                                              If Not rval.IsEmptyString Then Return New Sizes(rval, ss.Value("url"))
                                              rval = RegexReplace(ss.Value("url"), ObtainMedia_SizeFuncPic_RegexS)
                                              If Not rval.IsEmptyString Then Return New Sizes(AConvert(Of Integer)(rval, 1) * -1, ss.Value("url"))
                                              Return New Sizes(10000, ss.Value("url"))
                                          End If
                                      End Function
            ObtainMedia_SizeFuncVid = Function(ss) If(ss.Value("url").IsEmptyString, New Sizes("----", ""), New Sizes(10000, ss.Value("url")))
        End Sub
        Protected Sub ObtainMedia(ByVal n As EContainer, ByVal PostID As String, Optional ByVal SpecialFolder As String = Nothing,
                                  Optional ByVal DateObj As String = Nothing, Optional ByVal InitialType As Integer = -1,
                                  Optional ByVal PostOriginUrl As String = Nothing,
                                  Optional ByVal State As UStates = UStates.Unknown, Optional ByVal Attempts As Integer = 0)
            Try
                Dim maxSize As Func(Of EContainer, Integer) = Function(ByVal _ss As EContainer) As Integer
                                                                  Dim w% = AConvert(Of Integer)(_ss.Value("width"), 0)
                                                                  Dim h% = AConvert(Of Integer)(_ss.Value("height"), 0)
                                                                  'Return w + h
                                                                  Return Math.Max(w, h)
                                                              End Function
                Dim wrongData As Predicate(Of Sizes) = Function(_ss) _ss.HasError Or _ss.Data.IsEmptyString
                Dim img As Predicate(Of EContainer) = Function(_img) Not _img.Name.IsEmptyString AndAlso _img.Name.StartsWith("image_versions") AndAlso _img.Count > 0
                Dim vid As Predicate(Of EContainer) = Function(_vid) Not _vid.Name.IsEmptyString AndAlso _vid.Name.StartsWith("video_versions") AndAlso _vid.Count > 0
                Dim ss As Func(Of EContainer, Sizes) = Function(_ss) New Sizes(maxSize(_ss), _ss.Value("url"))
                Dim ssVid As Func(Of EContainer, Sizes) = ss
                Dim ssPic As Func(Of EContainer, Sizes) = ss
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
                If Not ObtainMedia_SizeFuncVid Is Nothing Then ssVid = ObtainMedia_SizeFuncVid
                If Not ObtainMedia_SizeFuncPic Is Nothing Then ssPic = ObtainMedia_SizeFuncPic
                If n.Count > 0 Then
                    Dim l As New List(Of Sizes)
                    Dim d As EContainer
                    Dim t%
                    Dim abstractDecision As Boolean = False
                    '8 - gallery
                    '2 - one video
                    '1 - one picture
                    t = n.Value("media_type").FromXML(Of Integer)(-1)
                    If t = -1 And InitialType = 8 And ObtainMedia_AllowAbstract Then
                        If n.Contains(vid) Then
                            t = 2
                            abstractDecision = True
                        ElseIf n.Contains(img) Then
                            t = 1
                            abstractDecision = True
                        End If
                    End If
                    If t >= 0 Then
                        Select Case t
                            Case 1
                                If n.Contains(img) Then
                                    If Not abstractDecision Then t = n.Value("media_type").FromXML(Of Integer)(-1)
                                    DateObj = mDate(n)
                                    If t >= 0 Then
                                        With n.ItemF({img, "candidates"}).XmlIfNothing
                                            If .Count > 0 Then
                                                l.Clear()
                                                l.ListAddList(.Select(ssPic), LNC)
                                                If l.Count > 0 Then l.RemoveAll(wrongData)
                                                If l.Count > 0 Then
                                                    l.Sort()
                                                    _TempMediaList.ListAddValue(MediaFromData(UTypes.Picture, l.First.Data, PostID, DateObj, SpecialFolder, PostOriginUrl, State, Attempts), LNC)
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
                                            l.ListAddList(.Select(ssVid), LNC)
                                            If l.Count > 0 Then l.RemoveAll(wrongData)
                                            If l.Count > 0 Then
                                                l.Sort()
                                                _TempMediaList.ListAddValue(MediaFromData(UTypes.Video, l.First.Data, PostID, DateObj, SpecialFolder, PostOriginUrl, State, Attempts), LNC)
                                                l.Clear()
                                            End If
                                        End If
                                    End With
                                End If
                            Case 8
                                DateObj = mDate(n)
                                With n("carousel_media").XmlIfNothing
                                    If .Count > 0 Then
                                        For Each d In .Self : ObtainMedia(d, PostID, SpecialFolder, DateObj, 8, PostOriginUrl) : Next
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
        Private Sub GetStoriesData(ByRef StoriesList As List(Of String), ByVal GetUserStory As Boolean, ByVal Token As CancellationToken)
            Const ReqUrl$ = "https://i.instagram.com/api/v1/feed/reels_media/?{0}"
            Dim tmpList As IEnumerable(Of String) = Nothing
            Dim qStr$, r$, sFolder$, storyID$, pid$
            Dim i% = -1
            Dim jj As EContainer, s As EContainer
            ThrowAny(Token)
            If StoriesList.ListExists Or GetUserStory Then
                If Not GetUserStory Then tmpList = StoriesList.Take(5)
                If tmpList.ListExists Or GetUserStory Then
                    If GetUserStory Then
                        qStr = $"https://www.instagram.com/api/v1/feed/reels_media/?reel_ids={ID}"
                    Else
                        qStr = String.Format(ReqUrl, tmpList.Select(Function(q) $"reel_ids=highlight:{q}").ListToString("&"))
                    End If
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
                                    If GetUserStory Then
                                        sFolder = $"{StoriesFolder} (user)"
                                    Else
                                        If sFolder.IsEmptyString Then sFolder = $"Story_{storyID}"
                                        If sFolder.IsEmptyString Then sFolder = $"Story_{i}"
                                        sFolder = $"{StoriesFolder}\{sFolder}"
                                    End If
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
                    If Not GetUserStory Then StoriesList.RemoveRange(0, tmpList.Count)
                End If
            End If
        End Sub
        Private Function GetStoriesList() As List(Of String)
            Try
                Dim r$ = Responser.GetResponse($"https://i.instagram.com/api/v1/highlights/{ID}/highlights_tray/",, EDP.ThrowException)
                If Not r.IsEmptyString Then
                    Dim ee As New ErrorsDescriber(EDP.ReturnValue) With {.DeclaredMessage = New MMessage($"{ToStringForLog()}:")}
                    Using j As EContainer = JsonDocument.Parse(r, ee).XmlIfNothing()("tray").XmlIfNothing
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
#Region "Erase"
        Protected Overrides Sub EraseData_AdditionalDataFiles()
            Dim f As SFile = MyFilePostsKV
            If f.Exists Then f.Delete(SFO.File, SFODelete.DeleteToRecycleBin, EDP.ReturnValue)
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
            If Responser.StatusCode = HttpStatusCode.NotFound Then '404
                If Not UserNameRequested AndAlso GetUserNameById() Then Return 1 Else UserExists = False
            ElseIf Responser.StatusCode = HttpStatusCode.BadRequest Or Responser.StatusCode = HttpStatusCode.Unauthorized Then '400, 401
                HasError = True
                MyMainLOG = $"Instagram credentials have expired [{CInt(Responser.StatusCode)}]: {ToStringForLog()} [{s}]"
                DisableSection(s)
            ElseIf Responser.StatusCode = HttpStatusCode.Forbidden And s = Sections.Tagged Then '403
                Return 3
            ElseIf Responser.StatusCode = 429 Then '429
                With MySiteSettings
                    Dim WaiterExists As Boolean = .LastApplyingValue.HasValue
                    .TooManyRequests(True)
                    If Not WaiterExists Then .LastApplyingValue = 2
                End With
                Caught429 = True
                MyMainLOG = $"Number of requests before error 429: {RequestsCount}"
                Return 1
            ElseIf Responser.StatusCode = 560 Or Responser.StatusCode = HttpStatusCode.InternalServerError Then '560, 500
                MySiteSettings.SkipUntilNextSession = True
                Err5xx = Responser.StatusCode
            Else
                MyMainLOG = $"Something is wrong. Your credentials may have expired [{CInt(Responser.StatusCode)}/{CInt(Responser.Status)}]: {ToString()} [{s}]"
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
                    Case Sections.Reels : MySiteSettings.DownloadReels.Value = False
                    Case Sections.Tagged : MySiteSettings.DownloadTagged.Value = False
                    Case Sections.Timeline, Sections.Stories, Sections.UserStories, Sections.SavedPosts
                        MySiteSettings.DownloadTimeline.Value = False
                        MySiteSettings.DownloadStories.Value = False
                        MySiteSettings.DownloadStoriesUser.Value = False
                End Select
                MyMainLOG = $"[{s}] downloading is disabled until you update your credentials".ToUpper
            End If
        End Sub
#End Region
#Region "Create media"
        Private Function MediaFromData(ByVal t As UTypes, ByVal _URL As String, ByVal PostID As String, ByVal PostDate As String,
                                       Optional ByVal SpecialFolder As String = Nothing, Optional ByVal PostOriginUrl As String = Nothing,
                                       Optional ByVal State As UStates = UStates.Unknown, Optional ByVal Attempts As Integer = 0) As UserMedia
            _URL = LinkFormatterSecure(RegexReplace(_URL.Replace("\", String.Empty), LinkPattern))
            Dim m As New UserMedia(_URL, t) With {.URL_BASE = PostOriginUrl.IfNullOrEmpty(_URL), .Post = New UserPost With {.ID = PostID}}
            If Not m.URL.IsEmptyString Then m.File = CStr(RegexReplace(m.URL, FilesPattern))
            If Not PostDate.IsEmptyString Then m.Post.Date = AConvert(Of Date)(PostDate, UnixDate32Provider, Nothing) Else m.Post.Date = Nothing
            m.SpecialFolder = SpecialFolder
            If State = UStates.Missing Then m.State = UStates.Missing : m.Attempts = Attempts
            Return m
        End Function
#End Region
#Region "Standalone downloader"
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            Dim PID$ = RegexReplace(Data.URL, RParams.DMS(String.Format(UserRegexDefaultPattern, "instagram.com/p/"), 1))
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