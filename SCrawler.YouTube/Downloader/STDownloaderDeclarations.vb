' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace DownloadObjects.STDownloader
    Public Module STDownloaderDeclarations
        Public Const DownloaderDataFolder As String = "Settings\DownloaderData\"
        Public Enum DoubleClickBehavior As Integer
            None = SFO.None
            Folder = SFO.Path
            File = SFO.File
        End Enum
        Public Property MyNotificator As INotificator
        Public Property MyDownloaderSettings As IDownloaderSettings
    End Module
End Namespace