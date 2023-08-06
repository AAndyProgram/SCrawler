' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin.Attributes
Namespace API.TikTok
    Friend Class UserExchangeOptions
        <PSetting(NameOf(SiteSettings.RemoveTagsFromTitle), NameOf(MySettings))>
        Friend Property RemoveTagsFromTitle As Boolean
        <PSetting(NameOf(SiteSettings.TitleUseNative), NameOf(MySettings))>
        Friend Property TitleUseNative As Boolean
        <PSetting(NameOf(SiteSettings.TitleAddVideoID), NameOf(MySettings))>
        Friend Property TitleAddVideoID As Boolean
        Private ReadOnly MySettings As SiteSettings
        Friend Sub New(ByVal u As UserData)
            MySettings = u.HOST.Source
            RemoveTagsFromTitle = u.RemoveTagsFromTitle
            TitleUseNative = u.TitleUseNative
            TitleAddVideoID = u.TitleAddVideoID
        End Sub
        Friend Sub New(ByVal s As SiteSettings)
            MySettings = s
            RemoveTagsFromTitle = s.RemoveTagsFromTitle.Value
            TitleUseNative = s.TitleUseNative.Value
            TitleAddVideoID = s.TitleAddVideoID.Value
        End Sub
    End Class
End Namespace
