' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin.Attributes
Namespace API.YouTube
    Friend Class UserExchangeOptions
        <PSetting(Caption:="Download videos")>
        Friend Property DownloadVideos As Boolean
        <PSetting(Caption:="Download shorts")>
        Friend Property DownloadShorts As Boolean
        <PSetting(Caption:="Download playlists")>
        Friend Property DownloadPlaylists As Boolean
        <PSetting(Caption:="Download community images")>
        Friend Property DownloadCommunityImages As Boolean
        <PSetting(Caption:="Download community videos")>
        Friend Property DownloadCommunityVideos As Boolean
        <PSetting(Caption:="Use cookies", ToolTip:="Use cookies when downloading data.")>
        Friend Property UseCookies As Boolean
        <PSetting(Caption:="Channel ID", Address:=SettingAddress.User)>
        Friend Property ChannelID As String
        Friend Sub New(ByVal u As UserData)
            DownloadVideos = u.DownloadYTVideos
            DownloadShorts = u.DownloadYTShorts
            DownloadPlaylists = u.DownloadYTPlaylists
            DownloadCommunityImages = u.DownloadYTCommunityImages
            DownloadCommunityVideos = u.DownloadYTCommunityVideos
            UseCookies = u.YTUseCookies
            ChannelID = u.ChannelID
        End Sub
        Friend Sub New(ByVal s As SiteSettings)
            DownloadVideos = s.DownloadVideos.Value
            DownloadShorts = s.DownloadShorts.Value
            DownloadPlaylists = s.DownloadPlaylists.Value
            DownloadCommunityImages = s.DownloadCommunityImages.Value
            DownloadCommunityVideos = s.DownloadCommunityVideos.Value
            UseCookies = s.UseCookies.Value
        End Sub
    End Class
End Namespace