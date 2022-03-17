' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Twitter
    Friend Module Declarations
        Friend Const TwitterSite As String = "Twitter"
        Friend DateProvider As New ADateTime(ADateTime.Formats.BaseDateTime)
        Friend ReadOnly VideoNode As NodeParams() = {New NodeParams("video_info", True, True, True, True, 10)}
        Friend ReadOnly VideoSizeRegEx As RParams = RParams.DMS("\d+x(\d+)", 1, EDP.ReturnValue)
        Friend ReadOnly UserIdRegEx As RParams = RParams.DMS("user_id.:.(\d+)", 1, EDP.ReturnValue)
    End Module
End Namespace