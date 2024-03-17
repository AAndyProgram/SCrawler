' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin.Attributes
Namespace API.OnlyFans
    Friend Class UserExchangeOptions
        <PSetting(NameOf(SiteSettings.DownloadTimeline), NameOf(MySettings))>
        Friend Property DownloadTimeline As Boolean
        <PSetting(NameOf(SiteSettings.DownloadStories), NameOf(MySettings))>
        Friend Property DownloadStories As Boolean
        <PSetting(NameOf(SiteSettings.DownloadHighlights), NameOf(MySettings))>
        Friend Property DownloadHighlights As Boolean
        <PSetting(NameOf(SiteSettings.DownloadChatMedia), NameOf(MySettings))>
        Friend Property DownloadChatMedia As Boolean
        Private ReadOnly MySettings As SiteSettings
        Friend Sub New(ByVal u As UserData)
            DownloadTimeline = u.MediaDownloadTimeline
            DownloadStories = u.MediaDownloadStories
            DownloadHighlights = u.MediaDownloadHighlights
            DownloadChatMedia = u.MediaDownloadChatMedia
            MySettings = u.HOST.Source
        End Sub
        Friend Sub New(ByVal s As SiteSettings)
            DownloadTimeline = s.DownloadTimeline.Value
            DownloadStories = s.DownloadStories.Value
            DownloadHighlights = s.DownloadHighlights.Value
            DownloadChatMedia = s.DownloadChatMedia.Value
            MySettings = s
        End Sub
    End Class
End Namespace