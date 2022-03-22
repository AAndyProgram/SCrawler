' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.RegularExpressions
Friend Module Declarations
    Friend ReadOnly Property M3U8Regex As RParams = RParams.DM("http.+?.m3u8.*?(?=')", 0)
    Friend ReadOnly Property VideoTitleRegex As RParams = RParams.DMS("html5player.setVideoTitle\('(.+)(?='\);)", 1)
    Friend ReadOnly Property VideoID As RParams = RParams.DMS(".*?www.xvideos.com/(video\d+).*", 1)
    Friend ReadOnly Property M3U8Reparse As RParams = RParams.DM("NAME=""(\d+).*?""[\r\n]*?(.+)(?=(|[\r\n]+?))", 0, RegexReturn.List)
    Friend ReadOnly Property M3U8Appender As RParams = RParams.DM("(.+)(?=/.+?\.m3u8.*?)", 0)
End Module