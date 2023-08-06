' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.XVIDEOS
    Friend Module Declarations
        Friend Const XvideosSiteKey As String = "AndyProgram_XVIDEOS"
        Friend ReadOnly Regex_M3U8 As RParams = RParams.DM("http.+?.m3u8.*?(?=')", 0)
        Friend ReadOnly Regex_VideoTitle As RParams = RParams.DMS("html5player.setVideoTitle\('(.+)(?='\);)", 1, EDP.ReturnValue, TitleHtmlConverter)
        Friend ReadOnly Regex_VideoID As RParams = RParams.DMS(".*?www.xvideos.com/(video\d+).*", 1)
        Friend ReadOnly Regex_M3U8_Reparse As RParams = RParams.DM("NAME=""(\d+).*?""[\r\n]*?(.+)(?=(|[\r\n]+?))", 0, RegexReturn.List)
        Friend ReadOnly Regex_M3U8_Appender As RParams = RParams.DM("(.+)(?=/.+?\.m3u8.*?)", 0)
        Friend ReadOnly Regex_SavedVideosPlaylist As RParams = RParams.DM("\<div id=""video.+?data-id=""(\d+).+?a href=""([^""]+)"".+?title=""([^""]*)""",
                                                                          0, RegexReturn.List, EDP.ReturnValue)

        Friend ReadOnly Regex_VideoThumbBig As RParams = RParams.DMS("html5player.setThumbUrl\d+\('([^']+)'\)", 1, EDP.ReturnValue)
        Friend ReadOnly Regex_VideoThumbSmall As RParams = RParams.DMS("html5player.setThumbUrl\('([^']+)'\)", 1, EDP.ReturnValue)
    End Module
End Namespace