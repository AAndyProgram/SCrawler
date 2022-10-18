' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.RedGifs
    Friend Module Declarations
        Friend Const RedGifsSiteKey As String = "AndyProgram_RedGifs"
        Friend Const RedGifsSite As String = "RedGifs"
        Friend ReadOnly DateProvider As New CustomProvider(Function(v, d, p, n, e) ADateTime.ParseUnicode(v, n, e))
        Friend ReadOnly WatchIDRegex As RParams = RParams.DMS(".+?watch/([^\?&""/]+)", 1, EDP.ReturnValue)
        Friend ReadOnly ThumbsIDRegex As RParams = RParams.DMS("([^/\?&""]+?)(-\w+?|)\.(mp4|jpg)", 1, EDP.ReturnValue,
                                                               Function(v) If(CStr(v).IsEmptyString, String.Empty, CStr(v).ToLower.Trim))
    End Module
End Namespace