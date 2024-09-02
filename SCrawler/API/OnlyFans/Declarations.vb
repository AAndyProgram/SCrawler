' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.OnlyFans
    Friend Module Declarations
        Friend ReadOnly DateProvider As New ADateTime("O")
        Friend ReadOnly RegExPostID As RParams = RParams.DM("(?<=onlyfans\.com/)(\d+)", 0, EDP.ReturnValue)
        Friend ReadOnly FilesSources As New List(Of Object()) From {
            {{"source", "source"}},
            {{"files", "source", "url"}},
            {{"files", "full", "url"}}
        }
        Friend Property Rules As DynamicRulesEnv
    End Module
End Namespace