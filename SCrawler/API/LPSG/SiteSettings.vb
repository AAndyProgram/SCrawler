' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin
Imports SCrawler.Plugin.Attributes
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.LPSG
    <Manifest("AndyProgram_LPSG")>
    Friend Class SiteSettings : Inherits Base.SiteSettingsBase
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New("LPSG", "www.lpsg.com", AccName, Temp, My.Resources.SiteResources.LPSGIcon_48, My.Resources.SiteResources.LPSGPic_32)
            UrlPatternUser = "https://www.lpsg.com/threads/{0}/"
            UserRegex = RParams.DMS(".+?lpsg.com/threads/[^/]+?\.(\d+)", 1, EDP.ReturnValue)
        End Sub
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            Return Responser.CookiesExists
        End Function
    End Class
End Namespace