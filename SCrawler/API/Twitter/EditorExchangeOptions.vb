' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin.Attributes
Imports DModels = SCrawler.API.Twitter.UserData.DownloadModels
Namespace API.Twitter
    Friend Class EditorExchangeOptions : Inherits Base.EditorExchangeOptionsBase
        Private Const DefaultOffset As Integer = 100
        Friend Overrides Property SiteKey As String = TwitterSiteKey
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
        <PSetting(NameOf(SiteSettings.MediaModelAllowNonUserTweets), NameOf(MySettings), LeftOffset:=DefaultOffset)>
        Friend Overridable Property MediaModelAllowNonUserTweets As Boolean = False
        <PSetting(Address:=SettingAddress.User,
                  Caption:="Download model 'Media'",
                  ToolTip:="Download the data using the 'https://x.com/UserName/media' command.", LeftOffset:=DefaultOffset)>
        Friend Overridable Property DownloadModelMedia As Boolean = False
        <PSetting(Address:=SettingAddress.User,
                  Caption:="Download model 'Profile'",
                  ToolTip:="Download the data using the 'https://x.com/UserName' command.", LeftOffset:=DefaultOffset)>
        Friend Overridable Property DownloadModelProfile As Boolean = False
        <PSetting(Address:=SettingAddress.User,
                  Caption:="Download model 'Search'",
                  ToolTip:="Download the data using the 'https://x.com/search?q=from:UserName+include:nativeretweets' command.", LeftOffset:=DefaultOffset)>
        Friend Overridable Property DownloadModelSearch As Boolean = False
        <PSetting(Address:=SettingAddress.User,
                  Caption:="Download model 'Likes'",
                  ToolTip:="Download the data using the 'https://x.com/UserName/likes' command.", LeftOffset:=DefaultOffset)>
        Friend Overridable Property DownloadModelLikes As Boolean = False
        <PSetting(Address:=SettingAddress.User,
                  Caption:="Download 'Broadcasts'",
                  ToolTip:="Download broadcasts posted by the user using the 'https://x.com/i/broadcasts/abcdef1234567' URLs", LeftOffset:=DefaultOffset)>
        Friend Overridable Property DownloadBroadcasts As Boolean = False
        <PSetting(Address:=SettingAddress.User,
                  Caption:="Force apply",
                  ToolTip:="Force overrides the default parameters for the first download." & vbCr & "Applies to first download only.", LeftOffset:=DefaultOffset)>
        Friend Overridable Property DownloadModelForceApply As Boolean = False
        Private ReadOnly Property MySettings As Object
        Friend Sub New(ByVal s As SiteSettings)
            GifsDownload = s.GifsDownload.Value
            GifsSpecialFolder = s.GifsSpecialFolder.Value
            GifsPrefix = s.GifsPrefix.Value
            UseMD5Comparison = s.UseMD5Comparison.Value
            DownloadModelForceApply = s.UseAppropriateModel.Value
            MediaModelAllowNonUserTweets = s.MediaModelAllowNonUserTweets.Value
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
            MyBase.New(u)
            GifsDownload = u.GifsDownload
            GifsSpecialFolder = u.GifsSpecialFolder
            GifsPrefix = u.GifsPrefix
            UseMD5Comparison = u.UseMD5Comparison
            RemoveExistingDuplicates = u.RemoveExistingDuplicates
            MediaModelAllowNonUserTweets = u.MediaModelAllowNonUserTweets
            If Not TypeOf u Is Mastodon.UserData Then
                DownloadModelForceApply = u.DownloadModelForceApply
                DownloadBroadcasts = u.DownloadBroadcasts
                Dim dm As DModels() = EnumExtract(Of DModels)(u.DownloadModel)
                If dm.ListExists Then
                    DownloadModelMedia = dm.Contains(DModels.Media)
                    DownloadModelProfile = dm.Contains(DModels.Profile)
                    DownloadModelSearch = dm.Contains(DModels.Search)
                    DownloadModelLikes = dm.Contains(DModels.Likes)
                End If
            End If
            MySettings = u.HOST.Source
        End Sub
    End Class
End Namespace