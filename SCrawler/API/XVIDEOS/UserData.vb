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
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace API.XVIDEOS
    Friend Class UserData : Inherits UserDataBase
#Region "XML names"
        Private Const Name_PersonType As String = "PersonType"
#End Region
#Region "Structures"
        Private Structure PlayListVideo : Implements IRegExCreator
            Friend ID As String
            Friend URL As String
            Friend Title As String
            Private Function CreateFromArray(ByVal ParamsArray() As String) As Object Implements IRegExCreator.CreateFromArray
                If ParamsArray.ListExists(3) Then
                    ID = ParamsArray(0)
                    URL = ParamsArray(1)
                    If Not URL.IsEmptyString Then URL = $"https://www.xvideos.com/{HtmlConverter(URL).StringTrimStart("/")}"
                    Title = TitleHtmlConverter(ParamsArray(2))
                End If
                Return Me
            End Function
            Friend Function ToUserMedia() As UserMedia
                Return New UserMedia(URL, UTypes.VideoPre) With {.Object = Me, .PictureOption = Title, .Post = ID}
            End Function
        End Structure
#End Region
#Region "Declarations"
        Friend Overrides ReadOnly Property FeedIsUser As Boolean
            Get
                Return SiteMode = SiteModes.User
            End Get
        End Property
        Private Property SiteMode As SiteModes = SiteModes.User
        Private Property Arguments As String = String.Empty
        Private Property PersonType As String = String.Empty
        Friend Overrides ReadOnly Property IsUser As Boolean
            Get
                Return SiteMode = SiteModes.User
            End Get
        End Property
        Friend ReadOnly Property IsSearch As Boolean
            Get
                Return SiteMode = SiteModes.Search Or SiteMode = SiteModes.Tags Or SiteMode = SiteModes.Categories
            End Get
        End Property
        Friend Overrides ReadOnly Property SpecialLabels As IEnumerable(Of String)
            Get
                Return {SearchRequestLabelName}
            End Get
        End Property
        Friend Property QueryString As String
            Get
                If SiteMode = SiteModes.User Then
                    Return String.Empty
                Else
                    Return GetUserUrl(0)
                End If
            End Get
            Set(ByVal q As String)
                UpdateUserOptions(True, q)
            End Set
        End Property
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return DirectCast(HOST.Source, SiteSettings)
            End Get
        End Property
#End Region
#Region "Load"
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return New UserExchangeOptions(Me)
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            If Not Obj Is Nothing AndAlso TypeOf Obj Is UserExchangeOptions Then QueryString = DirectCast(Obj, UserExchangeOptions).QueryString
        End Sub
        Private Function UpdateUserOptions(Optional ByVal Force As Boolean = False, Optional ByVal NewUrl As String = Nothing) As Boolean
            If Not Force OrElse (Not SiteMode = SiteModes.User AndAlso Not NewUrl.IsEmptyString AndAlso MyFileSettings.Exists) Then
                Dim eObj As Plugin.ExchangeOptions = Nothing
                If Force Then eObj = MySettings.IsMyUser(NewUrl)
                If (Force And Not eObj.UserName.IsEmptyString) Or (Not Force And NameTrue(True).IsEmptyString) Then
                    Dim n$() = If(Force, eObj.UserName, Name).Split("@")
                    If n.ListExists(2) Then
                        Dim opt$ = If(Force, eObj.Options, Options)
                        If opt.IsEmptyString AndAlso Not IsNumeric(n(0)) Then
                            If Not Force Then
                                PersonType = n(0)
                                NameTrue = If(Force, eObj.UserName, Name).Replace($"{PersonType}@", String.Empty)
                            End If
                        ElseIf Not opt.IsEmptyString Then
                            Dim n2$() = opt.Split("@")
                            Dim __SiteMode As SiteModes = CInt(n(0))
                            Dim __TrueName$ = n2.FirstOrDefault
                            Dim __Arguments$ = opt.Replace($"{__TrueName}@", String.Empty)
                            Dim __ForceApply As Boolean = False

                            If Force AndAlso (Not NameTrue(True) = __TrueName Or Not SiteMode = __SiteMode) Then
                                If ValidateChangeSearchOptions(ToStringForLog, $"{__SiteMode}: {__TrueName}", $"{SiteMode}: {NameTrue(True)}") Then
                                    __ForceApply = True
                                Else
                                    Return False
                                End If
                            End If

                            Arguments = __Arguments
                            Options = opt
                            If Not Force Then
                                SiteMode = __SiteMode
                                NameTrue = __TrueName
                                UserSiteName = $"{SiteMode}: {NameTrue}"
                                If FriendlyName.IsEmptyString Then FriendlyName = UserSiteName
                                Settings.Labels.Add(SearchRequestLabelName)
                                Labels.ListAddValue(SearchRequestLabelName, LNC)
                                Labels.Sort()
                            ElseIf Force And __ForceApply Then
                                SiteMode = __SiteMode
                                NameTrue = __TrueName
                            End If

                            Return True
                        End If
                    End If
                End If
            End If
            Return False
        End Function
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            With Container
                If Loading Then
                    SiteMode = .Value(Name_SiteMode).FromXML(Of Integer)(SiteModes.User)
                    Arguments = .Value(Name_Arguments)
                    PersonType = .Value(Name_PersonType)
                    If PersonType.IsEmptyString And NameTrue(True).IsEmptyString And Not Name.IsEmptyString Then
                        If Not Name.Contains("@") Then
                            Dim n$() = Name.Split("_")
                            PersonType = n(0)
                            NameTrue = Name.Replace($"{PersonType}_", String.Empty)
                        End If
                    End If
                    UpdateUserOptions()
                Else
                    If UpdateUserOptions() Then
                        .Value(Name_LabelsName) = LabelsString
                        .Value(Name_UserSiteName) = UserSiteName
                        .Value(Name_FriendlyName) = FriendlyName
                    End If
                    .Add(Name_SiteMode, CInt(SiteMode))
                    .Add(Name_TrueName, NameTrue(True))
                    .Add(Name_Arguments, Arguments)
                    .Add(Name_PersonType, PersonType)

                    'Debug.WriteLine(GetUserUrl(0))
                    'Debug.WriteLine(GetUserUrl(2))
                End If
            End With
        End Sub
#End Region
#Region "Initializer"
        Friend Sub New()
            SeparateVideoFolder = False
            UseInternalM3U8Function = True
            UseClientTokens = True
        End Sub
#End Region
#Region "GetUserUrl"
        Friend Function GetUserUrl(ByVal Page As Integer) As String
            Dim url$ = String.Empty
            If SiteMode = SiteModes.User Then
                url = $"https://xvideos.com/{PersonType}/{NameTrue}"
            ElseIf SiteMode = SiteModes.Categories Then
                url = "https://xvideos.com/c/"
                If Not Arguments.IsEmptyString Then url &= $"{Arguments}/"
                url &= NameTrue
                If Page > 1 Then url &= $"/{Page - 1}"
            ElseIf SiteMode = SiteModes.Tags Then
                url = "https://www.xvideos.com/tags/"
                If Not Arguments.IsEmptyString Then url &= $"{Arguments}/"
                url &= $"{NameTrue}/"
                If Page > 1 Then url &= Page - 1
            ElseIf SiteMode = SiteModes.Search Then
                url = $"https://www.xvideos.com/?k={NameTrue}"
                If Not Arguments.IsEmptyString Then url &= $"&{Arguments}"
                If Page > 1 Then url &= $"&p={Page - 1}"
            End If
            Return url
        End Function
#End Region
#Region "Download functions"
        Private Sub Wait429(ByVal Round As Integer)
            If (Round Mod 5) = 0 Then
                Thread.Sleep(5000 + (Round / 5).RoundDown)
            Else
                Thread.Sleep(1000)
            End If
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            If Not Settings.UseM3U8 Then MyMainLOG = $"{ToStringForLog()}: File [ffmpeg.exe] not found" : Exit Sub
            If IsSavedPosts Then
                If Not ACheck(MySettings.SavedVideosPlaylist.Value) Then Throw New ArgumentNullException("SavedVideosPlaylist", "Playlist of saved videos cannot be null")
                DownloadSavedVideos(Token)
            ElseIf Not SiteMode = SiteModes.User Then
                DownloadSavedVideos(Token)
            Else
                DownloadUserVideo(Token)
            End If
        End Sub
        Private Sub DownloadUserVideo(ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Dim isQuickies As Boolean = False
            Try
                Dim NextPage%, d%
                Dim round% = 0
                Dim limit% = If(DownloadTopCount, -1)
                Dim r$, n$
                Dim j As EContainer = Nothing
                Dim jj As EContainer
                Dim p As UserMedia
                Dim EnvirSet As Boolean = False

                If ID.IsEmptyString Then GetUserID()
                For i% = 0 To 1
                    If i = 1 And ID.IsEmptyString Then Exit For
                    NextPage = 0
                    d = 0
                    n = IIf(i = 0, "u", "url")
                    Do
                        round += 1
                        Wait429(round)
                        ThrowAny(Token)
                        If i = 0 Then
                            URL = GetUserUrl(0)
                            URL &= $"/videos/new/{If(NextPage = 0, String.Empty, NextPage)}"
                        Else 'Quickies
                            URL = $"https://www.xvideos.com/quickies-api/profilevideos/all/none/N/{ID}/{NextPage}"
                            isQuickies = True
                        End If
                        If Not j Is Nothing Then j.Dispose()
                        r = Responser.GetResponse(URL,, EDP.ReturnValue)
                        If Not r.IsEmptyString Then
                            If Not EnvirSet Then UserExists = True : UserSuspended = False : EnvirSet = True
                            j = JsonDocument.Parse(r)
                            If Not j Is Nothing Then
                                With j
                                    If .Contains("videos") Then
                                        With .Item("videos")
                                            If .Count > 0 Then
                                                ProgressPre.ChangeMax(.Count)
                                                NextPage += 1
                                                For Each jj In .Self
                                                    ProgressPre.Perform()
                                                    p = New UserMedia($"https://www.xvideos.com/{jj.Value(n).StringTrimStart("/")}") With {.Post = jj.Value("id")}
                                                    If Not p.Post.ID.IsEmptyString And Not jj.Value(n).IsEmptyString Then
                                                        If Not _TempPostsList.Contains(p.Post.ID) Then
                                                            _TempPostsList.Add(p.Post.ID)
                                                            _TempMediaList.Add(p)
                                                            d += 1
                                                            If limit > 0 And d = limit Then Exit Do
                                                        Else
                                                            Exit Do
                                                        End If
                                                    End If
                                                Next
                                                Continue Do
                                            End If
                                        End With
                                    End If
                                    .Dispose()
                                End With
                            End If
                        End If
                        Exit Do
                    Loop While NextPage < 100
                Next

                If Not j Is Nothing Then j.Dispose()

                If limit > 0 And _TempMediaList.Count >= limit Then _TempMediaList.ListAddList(_TempMediaList.ListTake(-1, limit), LAP.ClearBeforeAdd)
                If _TempMediaList.Count > 0 Then
                    If IsSubscription Then
                        Progress.Maximum += _TempMediaList.Count
                    Else
                        ProgressPre.ChangeMax(_TempMediaList.Count)
                    End If
                    For i% = 0 To _TempMediaList.Count - 1
                        If IsSubscription Then
                            Progress.Perform()
                        Else
                            ProgressPre.Perform()
                        End If
                        ThrowAny(Token)
                        _TempMediaList(i) = GetVideoData(_TempMediaList(i))
                    Next
                    _TempMediaList.RemoveAll(Function(m) m.URL.IsEmptyString)
                End If
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]",, isQuickies)
            Finally
                If _TempMediaList.ListExists Then _TempMediaList.RemoveAll(Function(m) m.URL.IsEmptyString)
            End Try
        End Sub
        Private Sub GetUserID()
            Dim r$ = Responser.GetResponse(GetUserUrl(0),, EDP.ReturnValue)
            If Not r.IsEmptyString Then ID = RegexReplace(r, RParams.DMS("""id_user"":(\d+)", 1, EDP.ReturnValue))
        End Sub
        Private Sub DownloadSavedVideos(ByVal Token As CancellationToken)
            Dim URL$ = MySettings.SavedVideosPlaylist.Value
            Try
                Dim NextPage% = IIf(SiteMode = SiteModes.User, -1, 0)
                Dim startPage% = NextPage
                Dim __continue As Boolean = True
                Dim r$
                Dim round% = 0
                Dim data As List(Of PlayListVideo)
                Dim pids As New List(Of String)
                Dim cBefore%
                Dim pageRepeatSet As Boolean, prevPostsFound As Boolean, newPostsFound As Boolean
                Dim sessionPosts As New List(Of String)
                Dim pageVideosRepeat As Integer = 0

                Dim limit% = If(DownloadTopCount, -1)
                Do
                    round += 1
                    Wait429(round)
                    ThrowAny(Token)
                    NextPage += 1
                    newPostsFound = False
                    pageRepeatSet = False
                    prevPostsFound = False
                    cBefore = _TempMediaList.Count
                    pids.Clear()

                    If SiteMode = SiteModes.User Then
                        URL = $"{MySettings.SavedVideosPlaylist.Value}{If(NextPage = 0, String.Empty, $"/{NextPage}")}"
                    Else
                        URL = GetUserUrl(NextPage)
                    End If

                    r = Responser.GetResponse(URL,, EDP.ReturnValue)

                    If Responser.HasError Then
                        If Responser.StatusCode = Net.HttpStatusCode.NotFound Then
                            If NextPage = startPage Then
                                If SiteMode = SiteModes.User Then MyMainLOG = $"XVIDEOS saved video playlist {URL} not found."
                                Exit Sub
                            Else
                                Exit Do
                            End If
                        Else
                            Throw New Exception(Responser.ErrorText, Responser.ErrorException)
                        End If
                    End If

                    If Not r.IsEmptyString Then
                        data = RegexFields(Of PlayListVideo)(r, {Regex_SavedVideosPlaylist}, {1, 2, 3}, EDP.ReturnValue)
                        If data.ListExists Then
                            pids.ListAddList(data.Select(Function(d) d.ID), LNC)
                            If data.RemoveAll(Function(d) _TempPostsList.Contains(d.ID)) > 0 And Not IsSearch Then __continue = False
                            If data.ListExists Then
                                _TempPostsList.ListAddList(data.Select(Function(d) d.ID), LNC)
                                _TempMediaList.ListAddList(data.Select(Function(d) d.ToUserMedia()), LNC)
                                newPostsFound = cBefore <> _TempMediaList.Count
                            ElseIf sessionPosts.Count > 0 AndAlso sessionPosts.ListContains(pids) Then
                                prevPostsFound = True
                            Else
                                If pageVideosRepeat >= 2 Then
                                    Exit Do
                                ElseIf Not pageRepeatSet And Not newPostsFound Then
                                    pageRepeatSet = True
                                    pageVideosRepeat += 1
                                End If
                            End If
                            sessionPosts.ListAddList(pids, LNC)
                        End If
                    End If
                    If limit > 0 And _TempMediaList.Count >= limit Then Exit Do
                    If prevPostsFound And Not pageRepeatSet And Not newPostsFound Then pageRepeatSet = True : pageVideosRepeat += 1
                    If prevPostsFound And newPostsFound And pageRepeatSet Then pageVideosRepeat -= 1
                    If IsSearch Then
                        __continue = pageVideosRepeat < 2 And NextPage < 1000 And (newPostsFound Or (prevPostsFound And Not newPostsFound))
                    ElseIf __continue Then
                        __continue = Not cBefore = _TempMediaList.Count
                    End If
                Loop While NextPage < 1000 And __continue

                pids.Clear()
                sessionPosts.Clear()

                If limit > 0 And _TempMediaList.Count >= limit Then _TempMediaList.ListAddList(_TempMediaList.ListTake(-1, limit), LAP.ClearBeforeAdd)
                If _TempMediaList.Count > 0 Then
                    If SiteMode = SiteModes.User Then
                        ProgressPre.ChangeMax(_TempMediaList.Count)
                    Else
                        Progress.Maximum += _TempMediaList.Count
                    End If
                    For i% = 0 To _TempMediaList.Count - 1
                        If SiteMode = SiteModes.User Then
                            ProgressPre.Perform()
                        Else
                            Progress.Perform()
                        End If
                        ThrowAny(Token)
                        _TempMediaList(i) = GetVideoData(_TempMediaList(i))
                    Next
                    _TempMediaList.RemoveAll(Function(m) m.URL.IsEmptyString)
                End If
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]")
            End Try
        End Sub
        Private Function GetVideoData(ByVal Media As UserMedia) As UserMedia
            Try
                If Not Media.URL.IsEmptyString Then
                    Dim r$ = Responser.GetResponse(Media.URL)
                    If Not r.IsEmptyString Then
                        Dim NewUrl$ = RegexReplace(r, Regex_M3U8)
                        If Not NewUrl.IsEmptyString Then
                            Dim appender$ = RegexReplace(NewUrl, Regex_M3U8_Appender)
                            Dim t$ = If(Media.PictureOption.IsEmptyString, RegexReplace(r, Regex_VideoTitle), Media.PictureOption)
                            If IsSubscription Then
                                Dim thumb$ = RegexReplace(r, Regex_VideoThumbBig)
                                If thumb.IsEmptyString Then thumb = RegexReplace(r, Regex_VideoThumbSmall)
                                If thumb.IsEmptyString Then thumb = RegexReplace(r, Regex_VideosThumb_OG_IMAGE)
                                If Not thumb.IsEmptyString Then
                                    Media.URL = thumb
                                    If Not t.IsEmptyString Then
                                        Media.PictureOption = t
                                        Media.File = $"{t}.mp4"
                                    Else
                                        Media.PictureOption = "Video"
                                        Media.File = "Video.mp4"
                                    End If
                                    Return Media
                                Else
                                    Return Nothing
                                End If
                            Else
                                r = Responser.GetResponse(NewUrl)
                                If Not r.IsEmptyString Then
                                    Dim ls As List(Of Sizes) = RegexFields(Of Sizes)(r, {Regex_M3U8_Reparse}, {1, 2})
                                    If ls.ListExists And Not MySettings.DownloadUHD.Value Then ls.RemoveAll(Function(v) Not v.Value.ValueBetween(1, 1080))
                                    If ls.ListExists Then
                                        ls.Sort()
                                        NewUrl = $"{appender}/{ls(0).Data.StringTrimStart("/")}"
                                        ls.Clear()
                                        Dim pID$ = Media.Post.ID
                                        If pID.IsEmptyString Then pID = RegexReplace(r, Regex_VideoID)
                                        If pID.IsEmptyString Then pID = "0"

                                        t = t.StringRemoveWinForbiddenSymbols.StringTrim
                                        If t.IsEmptyString Then
                                            t = pID
                                        Else
                                            If t.Length > 100 Then t = Left(t, 100)
                                        End If
                                        If Not NewUrl.IsEmptyString Then
                                            Return New UserMedia(NewUrl, UTypes.m3u8) With {
                                                .Post = pID,
                                                .URL_BASE = Media.URL,
                                                .File = $"{t}.mp4",
                                                .PictureOption = appender
                                            }
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
                Return Nothing
            Catch ex As Exception
                LogError(ex, $"[XVIDEOS.UserData.GetVideoData({Media.URL})]")
                Return Nothing
            End Try
        End Function
#End Region
#Region "DownloadContent"
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
        Protected Overrides Function DownloadM3U8(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile, ByVal Token As CancellationToken) As SFile
            Return M3U8.Download(Media.URL, Media.PictureOption, DestinationFile, Token, Progress, Not IsSingleObjectDownload)
        End Function
#End Region
#Region "SingleObject"
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            Dim m As UserMedia = GetVideoData(New UserMedia(Data.URL, UTypes.VideoPre))
            If Not m.URL.IsEmptyString Then _TempMediaList.Add(m)
        End Sub
#End Region
#Region "Exception"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            Dim isQuickies As Boolean = False
            If Not IsNothing(EObj) AndAlso TypeOf EObj Is Boolean Then isQuickies = CBool(EObj)
            If Responser.StatusCode = Net.HttpStatusCode.NotFound Then
                UserExists = False
                Return 1
            ElseIf isQuickies And Responser.StatusCode = Net.HttpStatusCode.InternalServerError Then
                Return 1
            Else
                Return 0
            End If
        End Function
#End Region
    End Class
End Namespace