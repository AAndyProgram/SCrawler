' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Tools
Namespace DownloadObjects.STDownloader
    Friend Module Declarations
        Private _MyFilesCache As CacheKeeper = Nothing
        Friend ReadOnly Property MyFilesCache As CacheKeeper
            Get
                If _MyFilesCache Is Nothing Then _MyFilesCache = New CacheKeeper(DownloaderDataFolder) With {.DeleteCacheOnDispose = False, .DeleteRootOnDispose = False}
                Return _MyFilesCache
            End Get
        End Property
    End Module
End Namespace