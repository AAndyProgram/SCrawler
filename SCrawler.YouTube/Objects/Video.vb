' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.YouTube.Base
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Forms.Toolbars
Namespace API.YouTube.Objects
    Public Class Video : Inherits YouTubeMediaContainerBase
        Public Sub New()
            _ObjectType = YouTubeMediaType.Single
            _MediaType = Plugin.UserMediaTypes.Video
        End Sub
        Public Overrides Function ToString(ByVal ForMediaItem As Boolean) As String
            Return Title
        End Function
        Public Overrides Function Parse(ByVal Container As EContainer, ByVal Path As SFile, ByVal IsMusic As Boolean,
                                        Optional ByVal Token As Threading.CancellationToken = Nothing, Optional ByVal Progress As IMyProgress = Nothing) As Boolean
            _MediaType = Plugin.UserMediaTypes.Video
            _ObjectType = YouTubeMediaType.Single
            Me.IsMusic = IsMusic
            Return MyBase.Parse(Container, Path, IsMusic, Token, Progress)
        End Function
    End Class
End Namespace