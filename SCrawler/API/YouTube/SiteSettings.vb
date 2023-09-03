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
Namespace API.YouTube
    <Manifest(YouTubeSiteKey), SpecialForm(True), SpecialForm(False), SeparatedTasks(1)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return My.Resources.SiteYouTube.YouTubeIcon_32
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.SiteYouTube.YouTubePic_96
            End Get
        End Property
        <PXML, PropertyOption(ControlText:="Download user videos")>
        Friend ReadOnly Property DownloadVideos As PropertyValue
        <PXML, PropertyOption(ControlText:="Download user shorts")>
        Friend ReadOnly Property DownloadShorts As PropertyValue
        <PXML, PropertyOption(ControlText:="Download user playlists")>
        Friend ReadOnly Property DownloadPlaylists As PropertyValue
        <PXML, PropertyOption(ControlText:="Use cookies", ControlToolTip:="Default value for new users." & vbCr & "Use cookies when downloading data.")>
        Friend ReadOnly Property UseCookies As PropertyValue
#End Region
#Region "Initializer"
        Friend Sub New()
            MyBase.New(YouTubeSite, "youtube.com")
            Responser.Cookies.ChangedAllowInternalDrop = False
            DownloadVideos = New PropertyValue(True)
            DownloadShorts = New PropertyValue(False)
            DownloadPlaylists = New PropertyValue(False)
            UseCookies = New PropertyValue(False)
            _SubscriptionsAllowed = True
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
                            .PerformUpdate()
                        End With
                    End If
                End With
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
                Return $"https://{IIf(DirectCast(User, UserData).IsMusic, "music", "www")}.youtube.com/watch?v={Media.Post.ID}"
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