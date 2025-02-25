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
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Pinterest
    Friend Module Declarations
        Friend ReadOnly DateProvider As ADateTime = GetDateProvider()
        Friend ReadOnly PwsHeader As New HttpHeader("x-pinterest-pws-handler", "www/[username]/pins.js")
        Friend ReadOnly GdlUrlPattern As RParams = RParams.DM(Base.GDL.GDLBatch.UrlLibStart.Replace("[", "\[").Replace("]", "\]") &
                                                              "([^""]+?)""(GET [^""]+)""", 0, EDP.ReturnValue)
        Friend ReadOnly SubBoardRegEx As RParams = RParams.DMS("\[pinterest\]\[debug\] Using PinterestSectionExtractor for '[^']+id:(\d+)'", 1,
                                                               RegexOptions.IgnoreCase, EDP.ReturnValue)
        Friend ReadOnly BoardInfoRootNode As String() = {"resource_response", "data"}
        Private Function GetDateProvider() As ADateTime
            Dim n As DateTimeFormatInfo = CultureInfo.GetCultureInfo("en-us").DateTimeFormat.Clone
            n.FullDateTimePattern = "ddd dd MMM yyyy HH:mm:ss"
            n.TimeSeparator = String.Empty
            'Sat, 01 Jan 2000 01:10:15 +0000
            Return New ADateTime(DirectCast(n.Clone, DateTimeFormatInfo)) With {.DateTimeStyle = DateTimeStyles.AssumeUniversal}
        End Function
    End Module
End Namespace