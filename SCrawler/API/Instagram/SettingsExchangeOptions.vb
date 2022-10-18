' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.Instagram
    Friend Structure SettingsExchangeOptions
        Friend DownloadTimeline As Boolean
        Friend DownloadStoriesTagged As Boolean
        Friend DownloadSaved As Boolean
        Friend Changed As Boolean
        Friend Sub New(ByVal Source As SiteSettings)
            With Source
                DownloadTimeline = .DownloadTimeline
                DownloadStoriesTagged = .DownloadStoriesTagged
                DownloadSaved = .DownloadSaved
            End With
        End Sub
    End Structure
End Namespace