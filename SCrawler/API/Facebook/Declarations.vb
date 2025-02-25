' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Text.RegularExpressions
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Facebook
    Friend Module Declarations
        Friend ReadOnly Regex_UserID As RParams = RParams.DMS("userid.:.(\d+)", 1, RegexOptions.IgnoreCase, EDP.ReturnValue)
        Friend ReadOnly Regex_AppID As RParams = RParams.DMS("APP_ID.:.(\d+)", 1, RegexOptions.IgnoreCase, EDP.ReturnValue)

        Friend ReadOnly Regex_Photos_by As RParams = RParams.DMS("photos_by"",""id"":""([^""]+)", 1, EDP.ReturnValue)
        Friend ReadOnly Regex_FileName As RParams = RParams.DM("([^/\?]+\..{3,4})(?=(\?|\Z))", 0, EDP.ReturnValue)
        Friend ReadOnly Regex_ProfileUrlID As RParams = RParams.DMS("profile.php\?id=(\d+)", 1, EDP.ReturnValue)
        Friend ReadOnly Regex_VideoPageID As RParams = RParams.DMS("pageid.:.(\d+)", 1, RegexOptions.IgnoreCase, EDP.ReturnValue)
        Friend ReadOnly Regex_ReelsPageID As RParams = RParams.DMS("\{[^\}]*""tab_key"":""owner_reels"",?[^\}]*""id"":""([^\}""]+)""", 1, RegexOptions.IgnoreCase, EDP.ReturnValue)
        Friend ReadOnly Regex_ReelsFilePattern As RParams = RParams.DM("[^/]+\.mp4", 0, EDP.ReturnValue)
        Friend ReadOnly Regex_StoryBucket As RParams = RParams.DMS("story_bucket[^\>]*?(\d+)", 1, EDP.ReturnValue)

        Friend ReadOnly Regex_VideoIDFromURL As RParams = RParams.DMS("facebook.com/([^/]+/videos/|watch/\D*[\?&]{1}v=)(\d+)", 2, EDP.ReturnValue)
        Friend ReadOnly Regex_PostHtmlFullPicture As RParams = RParams.DM("^((?!_[ps]{1}\d+x\d+).)*$", 0, EDP.ReturnValue)

        Friend ReadOnly SpecialNode() As NodeParams = {New NodeParams("attachment", True, True, True, True, 30),
                                                       New NodeParams("media", True, True, True, True, 0),
                                                       New NodeParams("photo_image", True, True, True, True, 0),
                                                       New NodeParams("uri", True, True, True, True, 0)}
        Friend ReadOnly SpecialNode2() As NodeParams = {New NodeParams("result", True, True, True, True, 30),
                                                        New NodeParams("data", True, True, True, True, 0),
                                                        New NodeParams("currmedia", True, True, True, True, 0),
                                                        New NodeParams("image", True, True, True, True, 0),
                                                        New NodeParams("uri", True, True, True, True, 0)}
    End Module
End Namespace