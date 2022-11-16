' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.PornHub
    Friend Class UserExchangeOptions
        Friend Property DownloadGifs As Boolean
        Friend Property DownloadPhotoOnlyFromModelHub As Boolean
        Friend Sub New(ByVal u As UserData)
            DownloadGifs = u.DownloadGifs
            DownloadPhotoOnlyFromModelHub = u.DownloadPhotoOnlyFromModelHub
        End Sub
        Friend Sub New(ByVal s As SiteSettings)
            Dim v As CheckState = CInt(s.DownloadGifs.Value)
            DownloadGifs = Not v = CheckState.Unchecked
            DownloadPhotoOnlyFromModelHub = s.DownloadPhotoOnlyFromModelHub.Value
        End Sub
    End Class
End Namespace