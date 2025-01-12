' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin.Attributes
Namespace API.Mastodon
    Friend Class EditorExchangeOptions : Inherits Twitter.EditorExchangeOptions
        <PSetting(Address:=SettingAddress.None)> Friend Overrides Property MediaModelAllowNonUserTweets As Boolean
        <PSetting(Address:=SettingAddress.None)> Friend Overrides Property DownloadModelMedia As Boolean
        <PSetting(Address:=SettingAddress.None)> Friend Overrides Property DownloadModelProfile As Boolean
        <PSetting(Address:=SettingAddress.None)> Friend Overrides Property DownloadModelSearch As Boolean
        <PSetting(Address:=SettingAddress.None)> Friend Overrides Property DownloadModelForceApply As Boolean
        <PSetting(Address:=SettingAddress.None)> Friend Overrides Property DownloadModelLikes As Boolean
        <PSetting(Address:=SettingAddress.None)> Friend Overrides Property DownloadBroadcasts As Boolean
        <PSetting(Address:=SettingAddress.None)> Friend Overrides Property UserName As String
        Friend Sub New(ByVal s As SiteSettings)
            MyBase.New(s)
        End Sub
        Friend Sub New(ByVal u As UserData)
            MyBase.New(u)
        End Sub
    End Class
End Namespace