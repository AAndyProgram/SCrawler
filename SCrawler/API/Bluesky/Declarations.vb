' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Bluesky
    Friend Module Declarations
        Friend Const BlueskySiteKey As String = "AndyProgram_Bluesky"
        Friend ReadOnly DateProvider As New ADateTime("yyyy-MM-ddTHH:mm:ss.FFF%K")
        Friend ReadOnly RegEx_PlayLists As RParams = RParams.DM("RESOLUTION=\d+x(\d+)\s*(\S+)", 0, RegexReturn.List, EDP.ReturnValue)
        Friend ReadOnly RegEx_FilePattern As RParams = RParams.DM("(.+?)(\.|@)(gif|m3u8|[^/\?\&]+)", 0, RegexReturn.ListByMatch, EDP.ReturnValue)
        Friend ReadOnly RegEx_SinglePostPattern As RParams = RParams.DM("profile/([^/]+)/post/([^/\?\&]+)", 0, RegexReturn.ListByMatch, EDP.ReturnValue)
    End Module
End Namespace