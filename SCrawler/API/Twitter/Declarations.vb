' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Globalization
Imports System.Text.RegularExpressions
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Twitter
    Friend Module Declarations
        Friend Const TwitterSite As String = "Twitter"
        Friend Const TwitterSiteKey As String = "AndyProgram_Twitter"
        Friend ReadOnly DateProvider As ADateTime = GetDateProvider()
        Friend ReadOnly VideoSizeRegEx As RParams = RParams.DMS("\d+x(\d+)", 1, EDP.ReturnValue)
        Friend ReadOnly StatusRegEx As RParams = RParams.DM(".*?(twitter|x)\.com/\S+/status/\d+", 0, EDP.ReturnValue)
        Friend ReadOnly BroadcastsUrls As Object() = {"entities", "urls", 0, "expanded_url"}
        Friend ReadOnly GdlLimitRegEx As RParams = RParams.DM("Waiting until[\s\W\d\:]+\(rate limit\)", 0, RegexOptions.IgnoreCase, EDP.ReturnValue)
        Friend ReadOnly GdlPostCoutNumberNodes As String() = {"data", "user", "result", "legacy", "statuses_count"}
        Private Function GetDateProvider() As ADateTime
            Dim n As DateTimeFormatInfo = CultureInfo.GetCultureInfo("en-us").DateTimeFormat.Clone
            n.FullDateTimePattern = "ddd MMM dd HH:mm:ss +ffff yyyy"
            n.TimeSeparator = String.Empty
            Return New ADateTime(DirectCast(n.Clone, DateTimeFormatInfo)) With {.DateTimeStyle = DateTimeStyles.AssumeUniversal}
        End Function
    End Module
End Namespace