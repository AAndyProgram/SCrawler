' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace DownloadObjects
    Friend Structure FeedMoveCopyTo
        Friend Destination As SFile
        Friend SeparateVideoFolder As Boolean
        Friend ReplaceUserProfile As Boolean
        Friend ReplaceUserProfile_CreateIfNull As Boolean
        Friend ReplaceUserProfile_Profile As API.Base.IUserData
        Friend ReadOnly Property DestinationTrue(ByVal Media As TDownloader.UserMediaD) As SFile
            Get
                If SeparateVideoFolder Then
                    Dim f$ = Destination.PathWithSeparator
                    With Media.Data
                        If Not (.Type = UTypes.Picture Or .Type = UTypes.GIF) Then f &= "Video\"
                    End With
                    Return f
                Else
                    Return Destination
                End If
            End Get
        End Property
    End Structure
End Namespace