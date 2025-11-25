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
#Region "Categories"
        Private Const CAT_DOWN As String = "Download"
#End Region
#Region "Download"
        <PropertyOption(ControlText:="Download videos", Category:=CAT_DOWN), PXML, PClonable>
        Friend ReadOnly Property DownloadTTVideos As PropertyValue
        <PropertyOption(ControlText:="Download photos", Category:=CAT_DOWN), PXML, PClonable>
        Friend ReadOnly Property DownloadTTPhotos As PropertyValue
#End Region
        <PropertyOption(ControlText:="Remove tags from title"), PXML, PClonable>
        Friend ReadOnly Property RemoveTagsFromTitle As PropertyValue
        <PropertyOption(ControlText:="Use native title", ControlToolTip:="Use a user-created video title for the filename instead of the video ID."), PXML, PClonable>
        Friend ReadOnly Property TitleUseNative As PropertyValue
        <PropertyOption(ControlText:="Use native title (standalone downloader)",
                        ControlToolTip:="Use a user-created video title for the filename instead of the video ID."), PXML, PClonable>
        Friend ReadOnly Property TitleUseNativeSTD As PropertyValue
        <PropertyOption(ControlText:="Add video ID to video title"), PXML, PClonable>
        Friend ReadOnly Property TitleAddVideoID As PropertyValue
        <PropertyOption(ControlText:="Add video ID to video title (standalone downloader)"), PXML, PClonable>
        Friend ReadOnly Property TitleAddVideoIDSTD As PropertyValue
        <PropertyOption(ControlText:="Use regex to clean video title"), PXML, PClonable>
        Friend ReadOnly Property TitleUseRegexForTitle As PropertyValue
        <PropertyOption(ControlText:="Title regex", ControlToolTip:="Regex to clean video title"), PXML, PClonable>
        Friend ReadOnly Property TitleUseRegexForTitle_Value As PropertyValue
        <PropertyOption(ControlText:="Use video date as file date",
                        ControlToolTip:="Set the file date to the date the video was added (website) (if available)."), PXML, PClonable>
        Friend ReadOnly Property UseParsedVideoDate As PropertyValue
        <PropertyOption(ControlText:="Use video date as file date (standalone downloader)",
                        ControlToolTip:="Set the file date to the date the video was added (website) (if available)."), PXML, PClonable>
        Friend ReadOnly Property UseParsedVideoDateSTD As PropertyValue
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New("TikTok", "www.tiktok.com", AccName, Temp, My.Resources.SiteResources.TikTokIcon_32, My.Resources.SiteResources.TikTokPic_192)

            DownloadTTVideos = New PropertyValue(True)
            DownloadTTPhotos = New PropertyValue(True)

            RemoveTagsFromTitle = New PropertyValue(False)
            TitleUseNative = New PropertyValue(True)
            TitleUseNativeSTD = New PropertyValue(True)
            TitleAddVideoID = New PropertyValue(True)
            TitleAddVideoIDSTD = New PropertyValue(True)
            TitleUseRegexForTitle = New PropertyValue(False)
            TitleUseRegexForTitle_Value = New PropertyValue(String.Empty, GetType(String))
            UseParsedVideoDate = New PropertyValue(True)
            UseParsedVideoDateSTD = New PropertyValue(False)

            UseNetscapeCookies = True
            UrlPatternUser = "https://www.tiktok.com/@{0}/"
            UserRegex = RParams.DMS(String.Format(UserRegexDefaultPattern, "tiktok.com/@"), 1)
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