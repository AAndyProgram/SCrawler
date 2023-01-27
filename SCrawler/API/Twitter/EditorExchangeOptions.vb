' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.Twitter
    Friend Class EditorExchangeOptions
        Friend Property GifsDownload As Boolean
        Friend Property GifsSpecialFolder As String
        Friend Property GifsPrefix As String
        Friend Sub New()
        End Sub
        Friend Sub New(ByVal s As SiteSettings)
            GifsDownload = s.GifsDownload.Value
            GifsSpecialFolder = s.GifsSpecialFolder.Value
            GifsPrefix = s.GifsPrefix.Value
        End Sub
        Friend Sub New(ByVal u As UserData)
            GifsDownload = u.GifsDownload
            GifsSpecialFolder = u.GifsSpecialFolder
            GifsPrefix = u.GifsPrefix
        End Sub
    End Class
End Namespace