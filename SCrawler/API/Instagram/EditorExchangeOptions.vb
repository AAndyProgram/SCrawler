' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin
Namespace API.Instagram
    Friend Class EditorExchangeOptions
        Friend Property GetTimeline As Boolean
        Friend Property GetStories As Boolean
        Friend Property GetTagged As Boolean
        Friend Sub New(ByVal h As ISiteSettings)
            With DirectCast(h, SiteSettings)
                GetTimeline = CBool(.GetTimeline.Value)
                GetStories = CBool(.GetStories.Value)
                GetTagged = CBool(.GetTagged.Value)
            End With
        End Sub
    End Class
End Namespace