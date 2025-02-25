' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin.Attributes
Namespace API.Facebook
    Friend Class UserExchangeOptions
        <PSetting(NameOf(SiteSettings.ParsePhotoBlock), NameOf(MySettings))>
        Friend Property ParsePhotoBlock As Boolean = True
        <PSetting(NameOf(SiteSettings.ParseVideoBlock), NameOf(MySettings))>
        Friend Property ParseVideoBlock As Boolean = True
        <PSetting(NameOf(SiteSettings.ParseReelsBlock), NameOf(MySettings))>
        Friend Property ParseReelsBlock As Boolean = False
        <PSetting(NameOf(SiteSettings.ParseStoriesBlock), NameOf(MySettings))>
        Friend Property ParseStoriesBlock As Boolean = True
        Private ReadOnly Property MySettings As SiteSettings
        Friend Sub New(ByVal u As UserData)
            MySettings = u.HostCollection.Default.Source
            ParsePhotoBlock = u.ParsePhotoBlock
            ParseVideoBlock = u.ParseVideoBlock
            ParseReelsBlock = u.ParseReelsBlock
            ParseStoriesBlock = u.ParseStoriesBlock
        End Sub
        Friend Sub New(ByVal s As SiteSettings)
            MySettings = s
            ParsePhotoBlock = s.ParsePhotoBlock.Value
            ParseVideoBlock = s.ParseVideoBlock.Value
            ParseReelsBlock = s.ParseReelsBlock.Value
            ParseStoriesBlock = s.ParseStoriesBlock.Value
        End Sub
    End Class
End Namespace