' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin.Attributes
Namespace API.Twitter
    Friend Class EditorExchangeOptions
        Private Const DefaultOffset As Integer = 100
        Friend Property SiteKey As String = TwitterSiteKey
        <PSetting(NameOf(SiteSettings.GifsDownload), NameOf(MySettings), LeftOffset:=DefaultOffset)>
        Friend Property GifsDownload As Boolean
        <PSetting(NameOf(SiteSettings.GifsSpecialFolder), NameOf(MySettings), LeftOffset:=DefaultOffset)>
        Friend Property GifsSpecialFolder As String
        <PSetting(NameOf(SiteSettings.GifsPrefix), NameOf(MySettings), LeftOffset:=DefaultOffset)>
        Friend Property GifsPrefix As String
        <PSetting(NameOf(SiteSettings.UseMD5Comparison), NameOf(MySettings), LeftOffset:=DefaultOffset)>
        Friend Property UseMD5Comparison As Boolean = False
        <PSetting(Caption:="Remove existing duplicates",
                  ToolTip:="Existing files will be checked for duplicates and duplicates removed." & vbCr &
                           "Works only on the first activation 'Use MD5 comparison'.", LeftOffset:=DefaultOffset)>
        Friend Property RemoveExistingDuplicates As Boolean = False
        Private ReadOnly Property MySettings As Object
        Friend Sub New(ByVal s As SiteSettings)
            GifsDownload = s.GifsDownload.Value
            GifsSpecialFolder = s.GifsSpecialFolder.Value
            GifsPrefix = s.GifsPrefix.Value
            UseMD5Comparison = s.UseMD5Comparison.Value
            MySettings = s
        End Sub
        Friend Sub New(ByVal s As Mastodon.SiteSettings)
            GifsDownload = s.GifsDownload.Value
            GifsSpecialFolder = s.GifsSpecialFolder.Value
            GifsPrefix = s.GifsPrefix.Value
            UseMD5Comparison = s.UseMD5Comparison.Value
            MySettings = s
        End Sub
        Friend Sub New(ByVal u As UserData)
            GifsDownload = u.GifsDownload
            GifsSpecialFolder = u.GifsSpecialFolder
            GifsPrefix = u.GifsPrefix
            UseMD5Comparison = u.UseMD5Comparison
            RemoveExistingDuplicates = u.RemoveExistingDuplicates
            MySettings = u.HOST.Source
        End Sub
    End Class
End Namespace