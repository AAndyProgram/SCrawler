' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML.Base
Namespace API.YouTube
    Friend Class YTSettings_Internal : Inherits Base.YouTubeSettings
        Private DataUpdated As Boolean = False
        Friend CookiesUpdated As Boolean = False
        Friend Sub ResetUpdate()
            XMLValuesEndEdit(Me)
            DataUpdated = False
            CookiesUpdated = False
        End Sub
        Friend Sub PerformUpdate()
            If DataUpdated Then
                MyBase.BeginUpdate()
                MyBase.Apply()
                MyBase.EndUpdate()
            ElseIf CookiesUpdated Then
                ApplyCookies()
            End If
        End Sub
        Protected Overrides Sub Apply()
            DataUpdated = True
        End Sub
        Protected Overrides Sub BeginUpdate()
        End Sub
        Protected Overrides Sub EndUpdate()
        End Sub
        Protected Overrides Sub EndEdit()
        End Sub
    End Class
End Namespace