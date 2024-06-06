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
        Friend ReadOnly OFScraperConfigPatternFile As SFile = $"{SettingsFolderName}\OFScraperConfigPattern.json"
        Friend Function CheckOFSConfig() As Boolean
            If Not OFScraperConfigPatternFile.Exists Then
                Dim t$ = Text.Encoding.UTF8.GetString(My.Resources.OFResources.OFScraperConfigPattern)
                TextSaver.SaveTextToFile(t, OFScraperConfigPatternFile, True)
                Return OFScraperConfigPatternFile.Exists
            Else
                Return True
            End If
        End Function
    End Module
End Namespace