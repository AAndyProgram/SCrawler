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
    <Manifest("AndyProgram_TikTok"), SpecialForm(False), SeparatedTasks(1)>
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
        <PropertyOption(ControlText:="Remove tags from title"), PXML>
        Friend Property RemoveTagsFromTitle As PropertyValue
        <PropertyOption(ControlText:="Use native title", ControlToolTip:="Use a user-created video title for the filename instead of the video ID."), PXML>
        Friend Property TitleUseNative As PropertyValue
        <PropertyOption(ControlText:="Use native title in standalone downloader",
                        ControlToolTip:="Use a user-created video title for the filename instead of the video ID."), PXML>
        Friend Property TitleUseNativeSTD As PropertyValue
        <PropertyOption(ControlText:="Add video ID to video title"), PXML>
        Friend Property TitleAddVideoID As PropertyValue
        Friend Sub New()
            MyBase.New("TikTok", "www.tiktok.com")
            RemoveTagsFromTitle = New PropertyValue(False)
            TitleUseNative = New PropertyValue(True)
            TitleUseNativeSTD = New PropertyValue(False)
            TitleAddVideoID = New PropertyValue(True)
            UseNetscapeCookies = True
            UrlPatternUser = "https://www.tiktok.com/@{0}/"
            UserRegex = RParams.DMS("[htps:/]{7,8}.*?tiktok.com/@([^/]+)", 1)
            ImageVideoContains = "tiktok.com"
        End Sub
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            Return Settings.YtdlpFile.Exists
        End Function
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse Not TypeOf Options Is UserExchangeOptions Then Options = New UserExchangeOptions(Me)
            If OpenForm Then
                Using f As New InternalSettingsForm(Options, Me, False) : f.ShowDialog() : End Using
            End If
        End Sub
    End Class
End Namespace