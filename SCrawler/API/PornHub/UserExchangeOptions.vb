' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin.Attributes
Namespace API.PornHub
    Friend Class UserExchangeOptions
        <PSetting(NameOf(SiteSettings.DownloadUHD), NameOf(MySettings))>
        Friend Property DownloadUHD As Boolean
        <PSetting(Caption:="Download gifs")>
        Friend Property DownloadGifs As Boolean
        <PSetting(NameOf(SiteSettings.DownloadPhotoOnlyFromModelHub), NameOf(MySettings), Caption:="Download photo only from ModelHub")>
        Friend Property DownloadPhotoOnlyFromModelHub As Boolean
        Private ReadOnly Property MySettings As SiteSettings
        Friend Sub New(ByVal u As UserData)
            DownloadUHD = u.DownloadUHD
            DownloadGifs = u.DownloadGifs
            DownloadPhotoOnlyFromModelHub = u.DownloadPhotoOnlyFromModelHub
            MySettings = u.HOST.Source
        End Sub
        Friend Sub New(ByVal s As SiteSettings)
            Dim v As CheckState = CInt(s.DownloadGifs.Value)
            DownloadUHD = s.DownloadUHD.Value
            DownloadGifs = Not v = CheckState.Unchecked
            DownloadPhotoOnlyFromModelHub = s.DownloadPhotoOnlyFromModelHub.Value
            MySettings = s
        End Sub
    End Class
End Namespace