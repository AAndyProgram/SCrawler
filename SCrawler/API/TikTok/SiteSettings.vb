' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports SCrawler.Plugin
Imports SCrawler.Plugin.Attributes
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.TikTok
    <Manifest("AndyProgram_TikTok")>
    Friend Class SiteSettings : Inherits SiteSettingsBase
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return My.Resources.SiteResources.TikTokIcon_32
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.SiteResources.TikTokPic_192
            End Get
        End Property
        Friend Sub New()
            MyBase.New("TikTok", "www.tiktok.com")
            UrlPatternUser = "https://www.tiktok.com/@{0}/"
            UserRegex = RParams.DMS("[htps:/]{7,8}.*?tiktok.com/@([^/]+)", 1)
            ImageVideoContains = "tiktok.com"
        End Sub
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
        Friend Overrides Function BaseAuthExists() As Boolean
            Return Responser.CookiesExists
        End Function
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            'TODO: TikTok disabled
            Return False
        End Function
    End Class
End Namespace