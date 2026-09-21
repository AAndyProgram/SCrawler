' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports SCrawler.Plugin.Attributes
Namespace API.Bluesky
    Friend Class EditorExchangeOptions : Inherits EditorExchangeOptionsBase
        Friend Overrides Property SiteKey As String = BlueskySiteKey
        <PSetting(NameOf(SiteSettings.DownloadModelMedia), NameOf(MySettings), Address:=SettingAddress.User)>
        Friend Overridable Property DownloadModelMedia As Boolean = False
        <PSetting(NameOf(SiteSettings.DownloadModelProfile), NameOf(MySettings), Address:=SettingAddress.User)>
        Friend Overridable Property DownloadModelProfile As Boolean = False
        <PSetting(NameOf(Twitter.SiteSettings.UseMD5Comparison), NameOf(MySettings))>
        Friend Property UseMD5Comparison As Boolean = False
        <PSetting(Caption:="Remove existing duplicates",
                  ToolTip:="Existing files will be checked for duplicates and duplicates removed." & vbCr &
                           "Works only on the first activation 'Use MD5 comparison'.")>
        Friend Property RemoveExistingDuplicates As Boolean = False
        Private ReadOnly Property MySettings As Object
        Friend Sub New(ByVal s As SiteSettings)
            MyBase.New(s)
            DownloadModelMedia = s.DownloadModelMedia.Value
            DownloadModelProfile = s.DownloadModelProfile.Value
            MySettings = s
        End Sub
        Friend Sub New(ByVal u As UserData)
            MyBase.New(u)
            DownloadModelMedia = u.DownloadModelMedia
            DownloadModelProfile = u.DownloadModelProfile
            UseMD5Comparison = u.UseMD5Comparison
            RemoveExistingDuplicates = u.RemoveExistingDuplicates
            MySettings = u.HOST.Source
        End Sub
        Friend Overrides Sub Apply(ByRef u As UserDataBase)
            MyBase.Apply(u)
            If Not DownloadModelMedia And Not DownloadModelProfile Then
                DownloadModelMedia = True
            ElseIf DownloadModelMedia Then
                DownloadModelProfile = False
            Else
                DownloadModelMedia = False
            End If
            With DirectCast(u, UserData)
                .DownloadModelMedia = DownloadModelMedia
                .DownloadModelProfile = DownloadModelProfile
                .UseMD5Comparison = UseMD5Comparison
                .RemoveExistingDuplicates = RemoveExistingDuplicates
            End With
        End Sub
    End Class
End Namespace