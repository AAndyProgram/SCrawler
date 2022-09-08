' Copyright (C) 2022  Andy
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
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.RedGifs
    <Manifest("AndyProgram_RedGifs"), UseClassAsIs>
    Friend Class SiteSettings : Inherits SiteSettingsBase
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return My.Resources.RedGifsIcon
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.RedGifsPic32
            End Get
        End Property
        Friend Sub New()
            MyBase.New(RedGifsSite, "redgifs.com")
            UrlPatternUser = "https://www.redgifs.com/users/{0}/"
            UserRegex = RParams.DMS("[htps:/]{7,8}.*?redgifs.com/users/([^/]+)", 1)
            ImageVideoContains = "redgifs"
        End Sub
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
        Friend Overrides Function GetSpecialDataF(ByVal URL As String) As IEnumerable(Of UserMedia)
            Return Reddit.UserData.GetVideoInfo(URL, Nothing)
        End Function
        Friend Overrides Function GetUserPostUrl(ByVal UserID As String, ByVal PostID As String) As String
            Return $"https://www.redgifs.com/watch/{PostID}"
        End Function
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            Return False
        End Function
    End Class
End Namespace