' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace DownloadObjects.STDownloader
    Public Interface IDownloaderSettings
        ReadOnly Property ShowNotifications As Boolean
        ReadOnly Property ShowNotificationsEveryDownload As Boolean
        ReadOnly Property MaxJobsCount As Integer
        ReadOnly Property DownloadAutomatically As Boolean
        ReadOnly Property RemoveDownloadedAutomatically As Boolean
        ReadOnly Property OnItemDoubleClick As DoubleClickBehavior
        ReadOnly Property OpenFolderInOtherProgram As Boolean
        ReadOnly Property OpenFolderInOtherProgram_Command As String
        ReadOnly Property OutputPathAskForName As Boolean
        ReadOnly Property OutputPathAutoAddPaths As Boolean
        ReadOnly Property CreateUrlFiles As Boolean
        ReadOnly Property ENVIR_FFMPEG As SFile
        ReadOnly Property ENVIR_YTDLP As SFile
        ReadOnly Property ENVIR_GDL As SFile
        ReadOnly Property ENVIR_CURL As SFile
    End Interface
End Namespace