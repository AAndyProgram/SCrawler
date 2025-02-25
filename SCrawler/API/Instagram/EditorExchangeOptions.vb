' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin.Attributes
Namespace API.Instagram
    Friend NotInheritable Class EditorExchangeOptions : Inherits Base.EditorExchangeOptionsBase
#Region "Download"
        <PSetting(Caption:="Get timeline", ToolTip:="Download user timeline")>
        Friend Property GetTimeline As Boolean
        <PSetting(Caption:="Get reels", ToolTip:="Download user reels")>
        Friend Property GetReels As Boolean
        <PSetting(Caption:="Get stories", ToolTip:="Download user stories (pinned)")>
        Friend Property GetStories As Boolean
        <PSetting(Caption:="Get stories: user", ToolTip:="Download user stories")>
        Friend Property GetStoriesUser As Boolean
        <PSetting(Caption:="Get tagged posts", ToolTip:="Download user tagged posts")>
        Friend Property GetTagged As Boolean
#End Region
#Region "Extract image"
        <PSetting(Caption:="Extract image from video: timeline")>
        Friend Property GetTimeline_VideoPic As Boolean
        <PSetting(Caption:="Extract image from video: reels")>
        Friend Property GetReels_VideoPic As Boolean
        <PSetting(Caption:="Extract image from video: stories")>
        Friend Property GetStories_VideoPic As Boolean
        <PSetting(Caption:="Extract image from video: stories: user")>
        Friend Property GetStoriesUser_VideoPic As Boolean
        <PSetting(Caption:="Extract image from video: tagged posts")>
        Friend Property GetTagged_VideoPic As Boolean
#End Region
        <PSetting(Caption:="Place the extracted image into the video folder")>
        Friend Property PutImageVideoFolder As Boolean
        Friend Overrides Property UserName As String
        <PSetting(Address:=SettingAddress.User, Caption:="Force update UserName", ToolTip:="Try to force update UserName if it is not found on the site")>
        Friend Property ForceUpdateUserName As Boolean = False
        <PSetting(Address:=SettingAddress.User, Caption:="Force update user information")>
        Friend Property ForceUpdateUserInfo As Boolean = False
        Friend Sub New(ByVal u As UserData)
            MyBase.New(u)
            With u
                GetTimeline = .GetTimeline
                GetReels = .GetReels
                GetStories = .GetStories
                GetStoriesUser = .GetStoriesUser
                GetTagged = .GetTaggedData

                GetTimeline_VideoPic = .GetTimeline_VideoPic
                GetReels_VideoPic = .GetReels_VideoPic
                GetStories_VideoPic = .GetStories_VideoPic
                GetStoriesUser_VideoPic = .GetStoriesUser_VideoPic
                GetTagged_VideoPic = .GetTaggedData_VideoPic

                PutImageVideoFolder = .PutImageVideoFolder

                ForceUpdateUserName = .ForceUpdateUserName
                ForceUpdateUserInfo = .ForceUpdateUserInfo
            End With
        End Sub
        Friend Sub New(ByVal s As SiteSettings)
            With s
                GetTimeline = CBool(.GetTimeline.Value)
                GetReels = CBool(.GetReels.Value)
                GetStories = CBool(.GetStories.Value)
                GetStoriesUser = CBool(.GetStoriesUser.Value)
                GetTagged = CBool(.GetTagged.Value)

                GetTimeline_VideoPic = CBool(.GetTimeline_VideoPic.Value)
                GetReels_VideoPic = CBool(.GetReels_VideoPic.Value)
                GetStories_VideoPic = CBool(.GetStories_VideoPic.Value)
                GetStoriesUser_VideoPic = CBool(.GetStoriesUser_VideoPic.Value)
                GetTagged_VideoPic = CBool(.GetTagged_VideoPic.Value)

                PutImageVideoFolder = CBool(.PutImageVideoFolder.Value)
            End With
        End Sub
    End Class
End Namespace