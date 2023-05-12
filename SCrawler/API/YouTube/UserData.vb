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
Namespace API.YouTube
    Friend Class UserData : Inherits UserDataBase
#Region "XML names"
        Private Const Name_DownloadYTVideos As String = "YTDownloadVideos"
        Private Const Name_DownloadYTShorts As String = "YTDownloadShorts"
        Private Const Name_DownloadYTPlaylists As String = "YTDownloadPlaylists"
        Private Const Name_YTUseCookies As String = "YTUseCookies"
        Private Const Name_IsMusic As String = "YTIsMusic"
        Private Const Name_IsChannelUser As String = "YTIsChannelUser"
        Private Const Name_YTMediaType As String = "YTMediaType"
        Private Const Name_LastDownloadDateVideos As String = "YTLastDownloadDateVideos"
        Private Const Name_LastDownloadDateShorts As String = "YTLastDownloadDateShorts"
        Private Const Name_LastDownloadDatePlaylist As String = "YTLastDownloadDatePlaylist"
#End Region
#Region "Declarations"
        Friend Property DownloadYTVideos As Boolean = True
        Friend Property DownloadYTShorts As Boolean = False
        Friend Property DownloadYTPlaylists As Boolean = False
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
                    YTUseCookies = .UseCookies
                End With
            End If
        End Sub
#End Region
#Region "Download"
        'Playlist reconfiguration implemented only for channels + music
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            Dim pr As New YTPreProgress(ProgressPre)
            Try
                Dim container As IYouTubeMediaContainer = Nothing
                Dim list As New List(Of IYouTubeMediaContainer)
                Dim url$ = String.Empty
                Dim maxDate As Date? = Nothing
                Dim nDate As Func(Of Date?, Date?) = Function(ByVal dInput As Date?) As Date?
                                                         If dInput.HasValue Then
                                                             If dInput.Value.AddDays(3) < Now Then Return dInput.Value.AddDays(1) Else Return dInput
                                                         Else
                                                             Return Nothing
                                                         End If
                                                     End Function
                Dim fillList As Func(Of Date?, Boolean) = Function(ByVal lDate As Date?) As Boolean
                                                              If Not container Is Nothing AndAlso container.HasElements Then
                                                                  Dim ce As IEnumerable(Of IYouTubeMediaContainer)
                                                                  ce = container.Elements
                                                                  If ce.ListExists Then ce = ce.Where(Function(e) e.ObjectType = YouTubeMediaType.Single)
                                                                  If ce.ListExists AndAlso lDate.HasValue Then _
                                                                     ce = ce.Where(Function(e) e.DateAdded <= lDate.Value AndAlso
                                                                                               Not e.ID.IsEmptyString AndAlso Not _TempPostsList.Contains(e.ID))
                                                                  If ce.ListExists Then
                                                                      maxDate = ce.Max(Function(e) e.DateAdded)
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
                    container = YouTubeFunctions.Parse(url, YTUseCookies, Token, pr, True, False,, LastDownloadDatePlaylist)
                    applySpecFolder.Invoke(String.Empty, False)
                    If fillList.Invoke(LastDownloadDatePlaylist) Then LastDownloadDatePlaylist = If(maxDate, Now)
                ElseIf YTMediaType = YouTubeMediaType.Channel Then
                    If IsMusic Or DownloadYTVideos Then
                        maxDate = Nothing
                        LastDownloadDateVideos = nDate(LastDownloadDateVideos)
                        url = $"https://{IIf(IsMusic, "music", "www")}.youtube.com/{IIf(IsMusic Or IsChannelUser, $"{YouTubeFunctions.UserChannelOption}/", "@")}{ID}"
                        container = YouTubeFunctions.Parse(url, YTUseCookies, Token, pr, True, False,, LastDownloadDateVideos)
                        applySpecFolder.Invoke(IIf(IsMusic, String.Empty, "Videos"), False)
                        If fillList.Invoke(LastDownloadDateVideos) Then LastDownloadDateVideos = If(maxDate, Now)
                    End If
                    If Not IsMusic And DownloadYTShorts Then
                        maxDate = Nothing
                        LastDownloadDateShorts = nDate(LastDownloadDateShorts)
                        url = $"https://www.youtube.com/{IIf(IsChannelUser, $"{YouTubeFunctions.UserChannelOption}/", "@")}{ID}/shorts"
                        container = YouTubeFunctions.Parse(url, YTUseCookies, Token, pr, True, False,, LastDownloadDateShorts)
                        applySpecFolder.Invoke("Shorts", False)
                        If fillList.Invoke(LastDownloadDateShorts) Then LastDownloadDateShorts = If(maxDate, Now)
                    End If
                    If Not IsMusic And DownloadYTPlaylists Then
                        maxDate = Nothing
                        LastDownloadDatePlaylist = nDate(LastDownloadDatePlaylist)
                        url = $"https://www.youtube.com/{IIf(IsChannelUser, $"{YouTubeFunctions.UserChannelOption}/", "@")}{ID}/playlists"
                        container = YouTubeFunctions.Parse(url, YTUseCookies, Token, pr, True, False,, LastDownloadDatePlaylist)
                        applySpecFolder.Invoke("Playlists", True)
                        If fillList.Invoke(LastDownloadDatePlaylist) Then LastDownloadDatePlaylist = If(maxDate, Now)
                    End If
                Else
                    Throw New InvalidOperationException($"Media type {YTMediaType} not implemented")
                End If
                If list.Count > 0 Then
                    With list(0)
                        If Settings.UserSiteNameUpdateEveryTime Or UserSiteName.IsEmptyString Then UserSiteName = .UserTitle
                        If FriendlyName.IsEmptyString Then FriendlyName = UserSiteName
                    End With
                    _TempMediaList.AddRange(list.Select(Function(c) New UserMedia(c)))
                    _TempPostsList.ListAddList(_TempMediaList.Select(Function(m) m.Post.ID), LNC)
                    list.Clear()
                End If
            Catch ex As Exception
                ProcessException(ex, Token, "data downloading error")
            Finally
                pr.Dispose()
            End Try
        End Sub
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            SeparateVideoFolder = False
            DownloadContentDefault(Token)
        End Sub
        Protected Overrides Function DownloadFile(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile,
                                                  ByVal Token As CancellationToken) As SFile
            If Not Media.Object Is Nothing AndAlso TypeOf Media.Object Is IYouTubeMediaContainer Then
                With DirectCast(Media.Object, YouTubeMediaContainerBase)
                    Dim f As SFile = .File
                    f.Path = DestinationFile.Path
                    If Not IsSingleObjectDownload And Not .FileIsPlaylistObject Then .FileIgnorePlaylist = True
                    .File = f
                    If IsSingleObjectDownload Then .Progress = Progress
                    .Download(YTUseCookies, Token)
                    If .File.Exists Then Return .File
                End With
            End If
            Return Nothing
        End Function
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            _TempMediaList.Add(New UserMedia(Data))
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