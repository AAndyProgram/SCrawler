﻿' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports SCrawler.Plugin.Attributes
Namespace API.ThisVid
    Friend Class UserExchangeOptions : Inherits EditorExchangeOptionsBase_P
        <PSetting(Caption:="Download public videos")>
        Friend Property DownloadPublic As Boolean = True
        <PSetting(Caption:="Download private videos")>
        Friend Property DownloadPrivate As Boolean = True
        <PSetting(Caption:="Download favourite videos")>
        Friend Property DownloadFavourite As Boolean = False
        <PSetting(NameOf(SiteSettings.DifferentFolders), NameOf(MySettings), Caption:="Different video folders")>
        Friend Property DifferentFolders As Boolean = True
        Private ReadOnly Property MySettings As SiteSettings
        Friend Sub New(ByVal s As SiteSettings)
            MyBase.New(s)
            DownloadPublic = s.DownloadPublic.Value
            DownloadPrivate = s.DownloadPrivate.Value
            DownloadFavourite = s.DownloadFavourite.Value
            DifferentFolders = s.DifferentFolders.Value
            MySettings = s
        End Sub
        Friend Sub New(ByVal u As UserData)
            MyBase.New(u)
            DownloadPublic = u.DownloadPublic
            DownloadPrivate = u.DownloadPrivate
            DownloadFavourite = u.DownloadFavourite
            DifferentFolders = u.DifferentFolders
            MySettings = u.HOST.Source
        End Sub
        Friend Overrides Sub Apply(ByRef u As IPSite)
            MyBase.Apply(u)
            With DirectCast(u, UserData)
                .DownloadPublic = DownloadPublic
                .DownloadPrivate = DownloadPrivate
                .DownloadFavourite = DownloadFavourite
                .DifferentFolders = DifferentFolders
            End With
        End Sub
    End Class
End Namespace