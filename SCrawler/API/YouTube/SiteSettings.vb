' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin
Imports SCrawler.Plugin.Attributes
Imports SCrawler.API.Base
Imports SCrawler.API.YouTube.Base
Imports DN = SCrawler.API.Base.DeclaredNames
Namespace API.YouTube
    <Manifest(YouTubeSiteKey), SpecialForm(True), SpecialForm(False), SeparatedTasks(1)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Categories"
        Private Const CAT_COMMUNITY As String = "Communities"
#End Region
#Region "Declarations"
        <PXML, PropertyOption(ControlText:="Use cookies", ControlToolTip:="Default value for new users." & vbCr & "Use cookies when downloading data.", IsAuth:=True), PClonable>
        Friend ReadOnly Property UseCookies As PropertyValue
#Region "New user defaults"
        <PXML, PropertyOption(ControlText:="Download user videos", Category:=DN.CAT_UserDefs), PClonable>
        Friend ReadOnly Property DownloadVideos As PropertyValue
        <PXML, PropertyOption(ControlText:="Download user shorts", Category:=DN.CAT_UserDefs), PClonable>
        Friend ReadOnly Property DownloadShorts As PropertyValue
        <PXML, PropertyOption(ControlText:="Download user playlists", Category:=DN.CAT_UserDefs), PClonable>
        Friend ReadOnly Property DownloadPlaylists As PropertyValue
        <PXML, PropertyOption(ControlText:="Download user community: images", Category:=DN.CAT_UserDefs), PClonable>
        Friend ReadOnly Property DownloadCommunityImages As PropertyValue
        <PXML, PropertyOption(ControlText:="Download user community: videos", Category:=DN.CAT_UserDefs), PClonable>
        Friend ReadOnly Property DownloadCommunityVideos As PropertyValue
#End Region
#Region "Communities"
        <PXML, PropertyOption(ControlText:="YouTube API host",
                              ControlToolTip:="YouTube API instance host (YouTube-operational-API). Example: 'localhost/YouTube-operational-API', 'http://localhost/YouTube-operational-API'.",
                              Category:=CAT_COMMUNITY), PClonable>
        Friend ReadOnly Property CommunityHost As PropertyValue
        <PXML, PropertyOption(ControlText:="YouTube API key", ControlToolTip:="YouTube Data API v3 developer key", Category:=CAT_COMMUNITY), PClonable>
        Friend ReadOnly Property YouTubeAPIKey As PropertyValue
        <PXML, PropertyOption(ControlText:="Ignore community errors", ControlToolTip:="If true, community errors will not be added to the log.", Category:=CAT_COMMUNITY), PClonable>
        Friend ReadOnly Property IgnoreCommunityErrors As PropertyValue
#End Region
#End Region
#Region "Initializer"
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New(YouTubeSite, "youtube.com", AccName, Temp, My.Resources.SiteYouTube.YouTubeIcon_32, My.Resources.SiteYouTube.YouTubePic_96)
            Responser.Cookies.ChangedAllowInternalDrop = False
            UseCookies = New PropertyValue(False)
            DownloadVideos = New PropertyValue(True)
            DownloadShorts = New PropertyValue(False)
            DownloadPlaylists = New PropertyValue(False)
            DownloadCommunityImages = New PropertyValue(False)
            DownloadCommunityVideos = New PropertyValue(False)
            CommunityHost = New PropertyValue(String.Empty, GetType(String))
            YouTubeAPIKey = New PropertyValue(String.Empty, GetType(String))
            IgnoreCommunityErrors = New PropertyValue(False)
            _SubscriptionsAllowed = True
            UseNetscapeCookies = True
        End Sub
#End Region
#Region "GetInstance"
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
#End Region
#Region "Edit, Update"
        Friend Overrides Sub Update()
            If _SiteEditorFormOpened Then
                With Responser.Cookies
                    If .Changed Then
                        .Changed = False
                        With DirectCast(MyYouTubeSettings, YTSettings_Internal)
                            .Cookies.Clear()
                            .Cookies.AddRange(Responser.Cookies)
                            .CookiesUpdated = True
                        End With
                    End If
                End With
                DirectCast(MyYouTubeSettings, YTSettings_Internal).PerformUpdate()
            End If
            MyBase.Update()
        End Sub
        Friend Overrides Sub EndEdit()
            If _SiteEditorFormOpened Then DirectCast(MyYouTubeSettings, YTSettings_Internal).ResetUpdate()
            MyBase.EndEdit()
        End Sub
#End Region
#Region "Available"
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            Return Settings.YtdlpFile.Exists And Settings.FfmpegFile.Exists
        End Function
#End Region
#Region "MyUser, MyUrl, get urls"
        Friend Const ChannelUserInt As Integer = 10000
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            Dim isMusic As Boolean = False
            Dim id$ = String.Empty
            Dim isChannelUser As Boolean = False
            Dim t As YouTubeMediaType = YouTubeFunctions.Info_GetUrlType(UserURL, isMusic,, isChannelUser, id)
            If Not t = YouTubeMediaType.Undefined And Not t = YouTubeMediaType.Single And Not id.IsEmptyString Then
                Return New ExchangeOptions(Site, $"{id}@{CInt(t) + IIf(isMusic, UserMedia.Types.Audio, 0) + IIf(isChannelUser, ChannelUserInt, 0)}")
            End If
            Return Nothing
        End Function
        Friend Overrides Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions
            If YouTubeFunctions.IsMyUrl(URL) Then Return New ExchangeOptions(Site, URL) Else Return Nothing
        End Function
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            If Not User Is Nothing AndAlso TypeOf User Is UserData Then
                If DirectCast(User, UserData).IsMusic Or Media.URL_BASE.IsEmptyString Then
                    Return $"https://{IIf(DirectCast(User, UserData).IsMusic, "music", "www")}.youtube.com/watch?v={Media.Post.ID}"
                Else
                    Return Media.URL_BASE
                End If
            Else
                Return String.Empty
            End If
        End Function
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider) As String
            If Not User Is Nothing AndAlso TypeOf User Is UserData Then Return DirectCast(User, UserData).GetUserUrl Else Return String.Empty
        End Function
#End Region
#Region "Settings form, options"
        Friend Overrides Sub OpenSettingsForm()
            MyYouTubeSettings.ShowForm(False)
        End Sub
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse Not TypeOf Options Is UserExchangeOptions Then Options = New UserExchangeOptions(Me)
            If OpenForm Then
                Using f As New InternalSettingsForm(Options, Me, False) : f.ShowDialog() : End Using
            End If
        End Sub
#End Region
    End Class
End Namespace