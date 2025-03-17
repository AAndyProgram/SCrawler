' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Text.RegularExpressions
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.TikTok
    Friend Module Declarations
        Friend ReadOnly SimpleDateConverter As New ADateTime("yyyyMMdd")
        Friend ReadOnly RegexTagsReplacer As RParams = RParams.DM("#\w+\s?", -1, RegexReturn.Replace,
                                                                  CType(Function(input$) String.Empty, Func(Of String, String)), EDP.ReturnValue)
        Friend ReadOnly RegexPhotoJson As RParams = RParams.DMS("UNIVERSAL_DATA_FOR_REHYDRATION__"" type=""application/json""\>([^\<]+)\<", 1,
                                                                RegexOptions.IgnoreCase, EDP.ReturnValue)
    End Module
End Namespace