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
Namespace API.PornHub
    Friend Class UserExchangeOptions : Inherits EditorExchangeOptionsBase_P
        <PSetting(NameOf(SiteSettings.DownloadUHD), NameOf(MySettings))>
        Friend Property DownloadUHD As Boolean
        <PSetting(NameOf(SiteSettings.DownloadUploaded), NameOf(MySettings))>
        Friend Property DownloadUploaded As Boolean
        <PSetting(NameOf(SiteSettings.DownloadTagged), NameOf(MySettings))>
        Friend Property DownloadTagged As Boolean
        <PSetting(NameOf(SiteSettings.DownloadPrivate), NameOf(MySettings))>
        Friend Property DownloadPrivate As Boolean
        <PSetting(NameOf(SiteSettings.DownloadFavorite), NameOf(MySettings))>
        Friend Property DownloadFavorite As Boolean
        <PSetting(Caption:="Download gifs")>
        Friend Property DownloadGifs As Boolean
        Private ReadOnly Property MySettings As SiteSettings
        Friend Sub New(ByVal u As UserData)
            MyBase.New(u)
            DownloadUHD = u.DownloadUHD
            DownloadUploaded = u.DownloadUploaded
            DownloadTagged = u.DownloadTagged
            DownloadPrivate = u.DownloadPrivate
            DownloadFavorite = u.DownloadFavorite
            DownloadGifs = u.DownloadGifs
            MySettings = u.HOST.Source
        End Sub
        Friend Sub New(ByVal s As SiteSettings)
            MyBase.New(s)
            Dim v As CheckState = CInt(s.DownloadGifs.Value)
            DownloadUHD = s.DownloadUHD.Value
            DownloadUploaded = s.DownloadUploaded.Value
            DownloadTagged = s.DownloadTagged.Value
            DownloadPrivate = s.DownloadPrivate.Value
            DownloadFavorite = s.DownloadFavorite.Value
            DownloadGifs = Not v = CheckState.Unchecked
            MySettings = s
        End Sub
        Friend Overrides Sub Apply(ByRef u As IPSite)
            MyBase.Apply(u)
            With DirectCast(u, UserData)
                .DownloadUHD = DownloadUHD
                .DownloadUploaded = DownloadUploaded
                .DownloadTagged = DownloadTagged
                .DownloadPrivate = DownloadPrivate
                .DownloadFavorite = DownloadFavorite
                .DownloadGifs = DownloadGifs
            End With
        End Sub
    End Class
End Namespace