' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin.Attributes
Namespace API.ThisVid
    Friend Class UserExchangeOptions
        <PSetting(Caption:="Download public videos")>
        Friend Property DownloadPublic As Boolean = True
        <PSetting(Caption:="Download private videos")>
        Friend Property DownloadPrivate As Boolean = True
        <PSetting(NameOf(SiteSettings.DifferentFolders), NameOf(MySettings), Caption:="Different video folders")>
        Friend Property DifferentFolders As Boolean = True
        Private ReadOnly Property MySettings As SiteSettings
        Friend Sub New(ByVal s As SiteSettings)
            DownloadPublic = s.DownloadPublic.Value
            DownloadPrivate = s.DownloadPrivate.Value
            DifferentFolders = s.DifferentFolders.Value
            MySettings = s
        End Sub
        Friend Sub New(ByVal u As UserData)
            DownloadPublic = u.DownloadPublic
            DownloadPrivate = u.DownloadPrivate
            DifferentFolders = u.DifferentFolders
            MySettings = u.HOST.Source
        End Sub
    End Class
End Namespace