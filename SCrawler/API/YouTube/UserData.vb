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
Imports SCrawler.API.YouTube.Base
Imports SCrawler.API.YouTube.Objects
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace API.YouTube
    Friend Class UserData : Inherits UserDataBase
#Region "XML names"
        Private Const Name_DownloadYTVideos As String = "YTDownloadVideos"
        Private Const Name_DownloadYTShorts As String = "YTDownloadShorts"
        Private Const Name_DownloadYTPlaylists As String = "YTDownloadPlaylists"
        Private Const Name_DownloadYTCommunityImages As String = "YTDownloadCommunityImages"
        Private Const Name_DownloadYTCommunityVideos As String = "YTDownloadCommunityVideos"
        Private Const Name_YTUseCookies As String = "YTUseCookies"
        Private Const Name_IsMusic As String = "YTIsMusic"
        Private Const Name_IsChannelUser As String = "YTIsChannelUser"
        Private Const Name_YTMediaType As String = "YTMediaType"
        Private Const Name_ChannelID As String = "ChannelID"
        Private Const Name_LastDownloadDateVideos As String = "YTLastDownloadDateVideos"
        Private Const Name_LastDownloadDateShorts As String = "YTLastDownloadDateShorts"
        Private Const Name_LastDownloadDatePlaylist As String = "YTLastDownloadDatePlaylist"
#End Region
#Region "Declarations"
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return HOST.Source
            End Get
        End Property
        Friend Property DownloadYTVideos As Boolean = True
        Friend Property DownloadYTShorts As Boolean = False
        Friend Property DownloadYTPlaylists As Boolean = False
        Friend Property DownloadYTCommunityImages As Boolean = False
        Friend Property DownloadYTCommunityVideos As Boolean = False
        Friend Property ChannelID As String = String.Empty
        Friend Property YTUseCookies As Boolean = False
        Friend Property IsMusic As Boolean = False
        Friend Property IsChannelUser As Boolean = False
        Friend Property YTMediaType As YouTubeMediaType = YouTubeMediaType.Undefined
        Private LastDownloadDateVideos As Date? = Nothing
        Private LastDownloadDateShorts As Date? = Nothing
        Private LastDownloadDatePlaylist As Date? = Nothing
        Friend Function GetUserUrl() As String
            If YTMediaType = YouTubeMediaType.PlayList Then
                Return $"https://{IIf(IsMusic, "music", "www")}.youtube.com/playlist?list={ID}"
            Else
                Return $"https://{IIf(IsMusic, "music", "www")}.youtube.com/{IIf(IsMusic Or IsChannelUser, $"{YouTubeFunctions.UserChannelOption}/", "@")}{ID}"
            End If
        End Function
#End Region
#Region "Initializer, loader"
        Friend Sub New()
            UseInternalDownloadFileFunction = True
            SeparateVideoFolder = False
        End Sub
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            With Container
                Dim SetNames As Action = Sub()
                                             If Not Name.IsEmptyString And ID.IsEmptyString Then
                                                 Dim n As List(Of String) = Name.Split("@").ToList
                                                 If n.ListExists(2) Then
                                                     Dim intValue% = n(1)
                                                     If intValue > 0 Then
                                                         If intValue >= SiteSettings.ChannelUserInt Then IsChannelUser = True : intValue -= SiteSettings.ChannelUserInt
                                                         If intValue >= UserMedia.Types.Audio Then IsMusic = True : intValue -= UserMedia.Types.Audio
                                                         YTMediaType = intValue
                                                         n.RemoveAt(1)
                                                         ID = n(0)
                                                     End If
                                                 End If
                                             End If
                                         End Sub
                If Loading Then
                    DownloadYTVideos = .Value(Name_DownloadYTVideos).FromXML(Of Boolean)(True)
                    DownloadYTShorts = .Value(Name_DownloadYTShorts).FromXML(Of Boolean)(False)
                    DownloadYTPlaylists = .Value(Name_DownloadYTPlaylists).FromXML(Of Boolean)(False)
                    DownloadYTCommunityImages = .Value(Name_DownloadYTCommunityImages).FromXML(Of Boolean)(False)
                    DownloadYTCommunityVideos = .Value(Name_DownloadYTCommunityVideos).FromXML(Of Boolean)(False)
                    ChannelID = .Value(Name_ChannelID)
                    IsMusic = .Value(Name_IsMusic).FromXML(Of Boolean)(False)
                    IsChannelUser = .Value(Name_IsChannelUser).FromXML(Of Boolean)(False)
                    YTMediaType = .Value(Name_YTMediaType).FromXML(Of Integer)(YouTubeMediaType.Undefined)
                    LastDownloadDateVideos = AConvert(Of Date)(.Value(Name_LastDownloadDateVideos), DateTimeDefaultProvider, Nothing)
                    LastDownloadDateShorts = AConvert(Of Date)(.Value(Name_LastDownloadDateShorts), DateTimeDefaultProvider, Nothing)
                    LastDownloadDatePlaylist = AConvert(Of Date)(.Value(Name_LastDownloadDatePlaylist), DateTimeDefaultProvider, Nothing)
                    SetNames.Invoke()
                Else
                    SetNames.Invoke()
                    If Not ID.IsEmptyString Then .Value(Name_UserID) = ID
                    .Add(Name_DownloadYTVideos, DownloadYTVideos.BoolToInteger)
                    .Add(Name_DownloadYTShorts, DownloadYTShorts.BoolToInteger)
                    .Add(Name_DownloadYTPlaylists, DownloadYTPlaylists.BoolToInteger)
                    .Add(Name_DownloadYTCommunityImages, DownloadYTCommunityImages.BoolToInteger)
                    .Add(Name_DownloadYTCommunityVideos, DownloadYTCommunityVideos.BoolToInteger)
                    .Add(Name_ChannelID, ChannelID)
                    .Add(Name_IsMusic, IsMusic.BoolToInteger)
                    .Add(Name_IsChannelUser, IsChannelUser.BoolToInteger)
                    .Add(Name_YTMediaType, CInt(YTMediaType))
                    .Add(Name_LastDownloadDateVideos, AConvert(Of String)(LastDownloadDateVideos, DateTimeDefaultProvider, String.Empty))
                    .Add(Name_LastDownloadDateShorts, AConvert(Of String)(LastDownloadDateShorts, DateTimeDefaultProvider, String.Empty))
                    .Add(Name_LastDownloadDatePlaylist, AConvert(Of String)(LastDownloadDatePlaylist, DateTimeDefaultProvider, String.Empty))
                End If
            End With
        End Sub
#End Region
#Region "Exchange options"
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return New UserExchangeOptions(Me)
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            If Not Obj Is Nothing AndAlso TypeOf Obj Is UserExchangeOptions Then
                With DirectCast(Obj, UserExchangeOptions)
                    DownloadYTVideos = .DownloadVideos
                    DownloadYTShorts = .DownloadShorts
                    DownloadYTPlaylists = .DownloadPlaylists
                    DownloadYTCommunityImages = .DownloadCommunityImages
                    DownloadYTCommunityVideos = .DownloadCommunityVideos
                    YTUseCookies = .UseCookies
                    ChannelID = .ChannelID
                End With
            End If
        End Sub
#End Region
#Region "Download"
        'Playlist reconfiguration implemented only for channels + music
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            Dim pr As New YTPreProgress(ProgressPre)
            Try
                If IsSubscription And IsMusic Then Exit Sub
                Dim container As IYouTubeMediaContainer = Nothing
                Dim list As New List(Of IYouTubeMediaContainer)
                Dim url$ = String.Empty
                Dim maxDate As Date? = Nothing
                Dim __minDate As Date? = DownloadDateFrom
                Dim __maxDate As Date? = DownloadDateTo
                Dim __getMinDate As Func(Of Date?, Date?) = Function(ByVal dInput As Date?) As Date?
                                                                If dInput.HasValue Then
                                                                    If __minDate.HasValue Then
                                                                        Return {__minDate.Value, dInput.Value}.Min
                                                                    Else
                                                                        Return dInput
                                                                    End If
                                                                ElseIf __minDate.HasValue Then
                                                                    Return __minDate
                                                                Else
                                                                    Return Nothing
                                                                End If
                                                            End Function
                Dim shortsUrlStandardize As Action(Of IYouTubeMediaContainer, Integer) = Sub(ByVal c As IYouTubeMediaContainer, ByVal ii As Integer)
                                                                                             Dim sUrl$ = $"https://www.youtube.com/shorts/{c.ID}"
                                                                                             'c.URL = sUrl
                                                                                             c.URL_BASE = sUrl
                                                                                         End Sub
                Dim nDate As Func(Of Date?, Date?) = Function(ByVal dInput As Date?) As Date?
                                                         If dInput.HasValue Then
                                                             If dInput.Value.AddDays(3) < Now Then Return dInput.Value.AddDays(1) Else Return dInput
                                                         Else
                                                             Return Nothing
                                                         End If
                                                     End Function
                Dim fillList As Func(Of Date?, Boolean, Boolean) = Function(ByVal lDate As Date?, ByVal ___isShorts As Boolean) As Boolean
                                                                       If Not container Is Nothing AndAlso container.HasElements Then
                                                                           Dim ce As IEnumerable(Of IYouTubeMediaContainer)
                                                                           ce = container.Elements
                                                                           If ce.ListExists Then ce = ce.Where(Function(e) e.ObjectType = YouTubeMediaType.Single)
                                                                           If ce.ListExists AndAlso lDate.HasValue Then _
                                                                              ce = ce.Where(Function(e) e.DateAdded >= lDate.Value AndAlso
                                                                                                        Not e.ID.IsEmptyString AndAlso Not _TempPostsList.Contains(e.ID))
                                                                           If ce.ListExists Then
                                                                               maxDate = ce.Max(Function(e) e.DateAdded)
                                                                               If ___isShorts Then ce.ListForEach(shortsUrlStandardize, EDP.None)
                                                                               list.AddRange(ce)
                                                                               Return True
                                                                           End If
                                                                       End If
                                                                       Return False
                                                                   End Function
                Dim applySpecFolder As Action(Of String, Boolean) = Sub(ByVal fName As String, ByVal isPls As Boolean)
                                                                        If If(container?.Count, 0) > 0 Then _
                                                                           container.Elements.ForEach(Sub(ByVal el As YouTubeMediaContainerBase)
                                                                                                          If isPls Then
                                                                                                              el.SpecialPathSetForPlaylist(fName)
                                                                                                          Else
                                                                                                              el.SpecialPath = fName
                                                                                                              el.SpecialPathDisabled = False
                                                                                                          End If
                                                                                                      End Sub)
                                                                    End Sub
                If YTMediaType = YouTubeMediaType.PlayList Then
                    maxDate = Nothing
                    LastDownloadDatePlaylist = nDate(LastDownloadDatePlaylist)
                    url = $"https://{IIf(IsMusic, "music", "www")}.youtube.com/playlist?list={ID}"
                    container = YouTubeFunctions.Parse(url, YTUseCookies, Token, pr, __getMinDate(LastDownloadDatePlaylist), __maxDate,, True)
                    applySpecFolder.Invoke(String.Empty, False)
                    If fillList.Invoke(LastDownloadDatePlaylist, False) Then LastDownloadDatePlaylist = If(maxDate, Now)
                ElseIf YTMediaType = YouTubeMediaType.Channel Then
                    If IsMusic Or DownloadYTVideos Then
                        maxDate = Nothing
                        LastDownloadDateVideos = nDate(LastDownloadDateVideos)
                        url = $"https://{IIf(IsMusic, "music", "www")}.youtube.com/{IIf(IsMusic Or IsChannelUser, $"{YouTubeFunctions.UserChannelOption}/", "@")}{ID}/videos"
                        container = YouTubeFunctions.Parse(url, YTUseCookies, Token, pr, __getMinDate(LastDownloadDateVideos), __maxDate,, True)
                        applySpecFolder.Invoke(IIf(IsMusic, String.Empty, "Videos"), False)
                        If fillList.Invoke(LastDownloadDateVideos, False) Then LastDownloadDateVideos = If(maxDate, Now)
                    End If
                    If Not IsMusic And DownloadYTShorts Then
                        maxDate = Nothing
                        LastDownloadDateShorts = nDate(LastDownloadDateShorts)
                        url = $"https://www.youtube.com/{IIf(IsChannelUser, $"{YouTubeFunctions.UserChannelOption}/", "@")}{ID}/shorts"
                        container = YouTubeFunctions.Parse(url, YTUseCookies, Token, pr, __getMinDate(LastDownloadDateShorts), __maxDate,, True)
                        applySpecFolder.Invoke("Shorts", False)
                        If fillList.Invoke(LastDownloadDateShorts, True) Then LastDownloadDateShorts = If(maxDate, Now)
                    End If
                    If Not IsMusic And DownloadYTPlaylists Then
                        maxDate = Nothing
                        LastDownloadDatePlaylist = nDate(LastDownloadDatePlaylist)
                        url = $"https://www.youtube.com/{IIf(IsChannelUser, $"{YouTubeFunctions.UserChannelOption}/", "@")}{ID}/playlists"
                        container = YouTubeFunctions.Parse(url, YTUseCookies, Token, pr, __getMinDate(LastDownloadDatePlaylist), __maxDate,, True)
                        applySpecFolder.Invoke("Playlists", True)
                        If fillList.Invoke(LastDownloadDatePlaylist, False) Then LastDownloadDatePlaylist = If(maxDate, Now)
                    End If
                    If Not IsMusic And (DownloadYTCommunityImages Or DownloadYTCommunityVideos) Then DownloadCommunity(String.Empty, Token)
                Else
                    Throw New InvalidOperationException($"Media type {YTMediaType} not implemented")
                End If
                If list.Count > 0 Then
                    With list(0)
                        If Settings.UpdateUserSiteNameEveryTime Or UserSiteName.IsEmptyString Then UserSiteName = .UserTitle
                        If FriendlyName.IsEmptyString Then FriendlyName = UserSiteName
                    End With
                    _TempMediaList.AddRange(list.Select(Function(c) New UserMedia(c) With {.URL = If(IsSubscription, c.ThumbnailUrlMedia, .URL)}))
                    _TempPostsList.ListAddList(_TempMediaList.Select(Function(m) m.Post.ID), LNC)
                    If IsSubscription Then _TempMediaList.RemoveAll(Function(m) m.URL.IsEmptyString)
                    list.Clear()
                End If
            Catch ex As Exception
                ProcessException(ex, Token, "data downloading error")
            Finally
                pr.Dispose()
            End Try
        End Sub
        Private Sub DownloadCommunity(ByVal Cursor As String, ByVal Token As CancellationToken, Optional ByVal Round As Integer = 0)
            Dim URL$ = String.Empty
            Const errMsg$ = "community data downloading error"
            Try
                Const postIdTemp$ = "Community_{0}"
                Const specFolder$ = "Community"
                Dim nextToken$ = String.Empty
                Dim postId$ = String.Empty, videoId$ = String.Empty
                Dim tmpPID$
                Dim imgCount%, imgNum%
                Dim postUrl As Func(Of String) = Function() $"https://www.youtube.com/post/{postId}"
                Dim image As EContainer, thumb As EContainer
                Dim sl As New List(Of Sizes)
                Dim m As UserMedia
                Dim v As IYouTubeMediaContainer

                If ChannelID.IsEmptyString Then GetChannelID()
                If ChannelID.IsEmptyString Then Throw New ArgumentNullException("ChannelID", "Channel ID cannot be null")

                URL = MySettings.CommunityHost.Value
                If URL.IsEmptyString Then
                    If Not CBool(MySettings.IgnoreCommunityErrors.Value) Then _
                       MyMainLOG = $"{ToStringForLog()}: YouTube API instance host is not specified for downloading communities"
                    Exit Sub
                Else
                    URL = LinkFormatterSecure(URL.Trim, "http").TrimEnd("/")
                End If

                URL = $"{URL}/channels?part=community&id={ChannelID}"
                If Not CStr(MySettings.YouTubeAPIKey.Value).IsEmptyString Then URL &= $"&key={CStr(MySettings.YouTubeAPIKey.Value).Trim}"
                If Not Cursor.IsEmptyString Then URL &= $"&pageToken={Cursor}"

                ProgressPre.ChangeMax(1)

                Using resp As New Responser
                    Dim r$ = resp.GetResponse(URL,, EDP.ReturnValue)
                    ProgressPre.Perform()
                    If Not r.IsEmptyString Then
                        Using j As EContainer = JsonDocument.Parse(r)
                            If j.ListExists Then
                                With j.ItemF({"items", 0})
                                    If .ListExists Then
                                        nextToken = .Value("nextPageToken")
                                        With .Item("community")
                                            If .ListExists Then
                                                ProgressPre.ChangeMax(.Count)
                                                For Each jj As EContainer In .Self
                                                    With jj
                                                        postId = .Value("id")
                                                        videoId = .Value("videoId")
                                                        tmpPID = String.Format(postIdTemp, postId)
                                                        If Not _TempPostsList.Contains(tmpPID) Then _TempPostsList.Add(tmpPID) Else Exit Sub

                                                        If Not videoId.IsEmptyString Then
                                                            If DownloadYTCommunityVideos Then
                                                                v = Nothing
                                                                Try : v = YouTubeFunctions.Parse($"https://www.youtube.com/watch?v={videoId}", YTUseCookies, Token) : Catch : End Try
                                                                If Not v Is Nothing Then
                                                                    With DirectCast(v, YouTubeMediaContainerBase)
                                                                        .SpecialPath = specFolder & "\Videos"
                                                                        .SpecialPathDisabled = False
                                                                    End With
                                                                    _TempMediaList.ListAddValue(New UserMedia(v) With {.Post = postId}, LNC)
                                                                End If
                                                            End If
                                                        ElseIf DownloadYTCommunityImages Then
                                                            With .Item("images")
                                                                If .ListExists Then
                                                                    imgCount = .Count
                                                                    imgNum = 0
                                                                    For Each image In .Self
                                                                        imgNum += 1
                                                                        sl.Clear()
                                                                        With image("thumbnails")
                                                                            If .ListExists Then
                                                                                For Each thumb In .Self : sl.Add(New Sizes(thumb.Value("width"), thumb.Value("url"))) : Next
                                                                                If sl.Count > 0 Then sl.RemoveAll(Function(s) s.HasError Or s.Data.IsEmptyString)
                                                                                If sl.Count > 0 Then
                                                                                    sl.Sort()
                                                                                    m = New UserMedia(sl(0).Data, UTypes.Picture) With {
                                                                                        .URL_BASE = postUrl.Invoke,
                                                                                        .Post = postId,
                                                                                        .SpecialFolder = specFolder,
                                                                                        .File = $"{postId}{IIf(imgCount > 1, $"_{imgNum}", String.Empty)}.jpg"
                                                                                    }
                                                                                    _TempMediaList.Add(m)
                                                                                End If
                                                                            End If
                                                                        End With
                                                                    Next
                                                                End If
                                                            End With
                                                        End If

                                                        ProgressPre.Perform()
                                                    End With
                                                Next
                                            End If
                                        End With
                                    ElseIf Not CBool(DirectCast(HOST.Source, SiteSettings).IgnoreCommunityErrors.Value) Then
                                        With j({"error"})
                                            If .ListExists Then MyMainLOG = $"{ToStringForLog()} {errMsg} [{ .Value("code")}]: { .Value("message")}"
                                        End With
                                    End If
                                End With
                            End If
                        End Using
                    ElseIf resp.HasError Then
                        If resp.Status = Net.WebExceptionStatus.ConnectFailure And Round < 2 Then
                            Thread.Sleep(1000)
                            DownloadCommunity(Cursor, Token, Round + 1)
                        ElseIf resp.StatusCode = Net.HttpStatusCode.NotFound Then
                            MyMainLOG = $"{ToStringForLog()} {errMsg} (not found)"
                        Else
                            Throw resp.ErrorException
                        End If
                    End If
                End Using

                If Not nextToken.IsEmptyString Then DownloadCommunity(nextToken, Token)
            Catch ex As Exception
                ProcessException(ex, Token, errMsg)
            End Try
        End Sub
        Private Sub GetChannelID()
            Try
                Dim r$ = GetWebString(GetUserUrl,, EDP.ThrowException)
                If Not r.IsEmptyString Then
                    Dim newUrl$ = RegexReplace(r, RParams.DMS("meta property=.og:url..content=.([^""]+)", 1, EDP.ReturnValue))
                    If Not newUrl.IsEmptyString Then
                        Dim newID$ = String.Empty
                        YouTubeFunctions.Info_GetUrlType(newUrl,,,, newID)
                        If Not newID.IsEmptyString And Not ChannelID = newID Then ChannelID = newID : _ForceSaveUserInfo = True
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex, Nothing, "error getting channel ID")
            End Try
        End Sub
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            SeparateVideoFolder = False
            DownloadContentDefault(Token)
        End Sub
        Private Class YTPreProgressContainer : Inherits PersonalUtilities.Forms.Toolbars.MyProgress
            Private ReadOnly MyPreProgress As PreProgress
            Friend Sub New(ByVal PR As PreProgress)
                MyBase.New(PR.Progress.MyControls)
                MyPreProgress = PR
            End Sub
            Private _MaxChanged As Boolean = False
            Public Overrides Property Maximum As Double
                Get
                    Return MyPreProgress.Progress.Maximum0
                End Get
                Set(ByVal max As Double)
                    MyPreProgress.Progress.Maximum0 += max
                    _MaxChanged = True
                End Set
            End Property
            Private _LastValue As Double = -1
            Private _FirstAdded As Boolean = False
            Public Overrides Property Value As Double
                Get
                    Return MyPreProgress.Progress.Value0
                End Get
                Set(ByVal v As Double)
                    If _MaxChanged Then
                        If Not _FirstAdded Then
                            _FirstAdded = True
                        ElseIf v > 0 Then
                            Dim newValue#
                            If _LastValue = -1 Then
                                newValue = v
                            ElseIf _LastValue > v Then
                                newValue = v
                            Else
                                newValue = v - _LastValue
                            End If
                            _LastValue = v
                            MyPreProgress.Progress.Value0 += newValue
                        End If
                    End If
                End Set
            End Property
            Public Overrides Sub Perform(Optional ByVal Value As Double = 1)
                MyPreProgress.Perform(Value)
            End Sub
            Public Overrides Sub Reset()
                MyPreProgress.Reset()
            End Sub
            Public Overrides Sub Done()
                MyPreProgress.Done()
            End Sub
            Public Overrides Property Information As String
                Get
                    Return String.Empty
                End Get
                Set : End Set
            End Property
            Public Overrides WriteOnly Property InformationTemporary(Optional ByVal AddPercentage As Boolean = False) As String
                Set : End Set
            End Property
            Public Overrides Function GetLabelText() As String
                Return String.Empty
            End Function
            Public Overrides Property Visible(Optional ByVal ProgressBar As Boolean = True, Optional ByVal Label As Boolean = True) As Boolean
                Get
                    Return True
                End Get
                Set : End Set
            End Property
        End Class
        Protected Overrides Function DownloadFile(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile,
                                                  ByVal Token As CancellationToken) As SFile
            If Not Media.Object Is Nothing AndAlso TypeOf Media.Object Is IYouTubeMediaContainer Then
                With DirectCast(Media.Object, YouTubeMediaContainerBase)
                    Dim f As SFile = .File
                    f.Path = DestinationFile.Path
                    If Not IsSingleObjectDownload And Not .FileIsPlaylistObject Then .FileIgnorePlaylist = True
                    .File = f
                    If IsSingleObjectDownload Then .Progress = Progress Else .Progress = New YTPreProgressContainer(ProgressPre)
                    .Download(YTUseCookies, Token)
                    If Not .Progress Is Nothing AndAlso TypeOf .Progress Is YTPreProgressContainer Then .Progress.Dispose()
                    If .File.Exists Then Return .File
                End With
            End If
            Return Nothing
        End Function
        Protected Overrides Function ValidateDownloadFile(ByVal URL As String, ByVal Media As UserMedia, ByRef Interrupt As Boolean) As Boolean
            Return Not Media.Type = UTypes.Picture
        End Function
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            _TempMediaList.Add(New UserMedia(Data))
        End Sub
#End Region
#Region "EraseData"
        Protected Overrides Sub EraseData_AdditionalDataFiles()
            LastDownloadDateVideos = Nothing
            LastDownloadDateShorts = Nothing
            LastDownloadDatePlaylist = Nothing
        End Sub
#End Region
#Region "DownloadingException"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            Return 0
        End Function
#End Region
#Region "IDisposable Support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then
                With _ContentList.Concat(_ContentNew)
                    If .Count > 0 Then
                        For Each m As UserMedia In .Self
                            If Not m.Object Is Nothing AndAlso TypeOf m.Object Is IYouTubeMediaContainer Then DirectCast(m.Object, IYouTubeMediaContainer).Dispose()
                        Next
                    End If
                End With
            End If
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace