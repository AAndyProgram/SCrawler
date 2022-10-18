' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Instagram
    Friend Module Declarations
        Friend Const InstagramSite As String = "Instagram"
        Friend Const InstagramSiteKey As String = "AndyProgram_Instagram"
        Friend ReadOnly FilesPattern As RParams = RParams.DMS(".+?([^/\?]+?\.[\w\d]{3,4})(?=(\?|\Z))", 1, EDP.ReturnValue)
        Friend ReadOnly Property DateProvider As New CustomProvider(Function(v, d, p, n, e) ADateTime.ParseUnicode(v))
    End Module
End Namespace