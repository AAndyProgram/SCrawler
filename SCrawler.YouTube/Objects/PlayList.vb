' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Forms.Toolbars
Namespace API.YouTube.Objects
    Public Class PlayList : Inherits YouTubeMediaContainerBase
        Public Sub New()
            _ObjectType = Base.YouTubeMediaType.PlayList
        End Sub
        Public Overrides Function ToString(ByVal ForMediaItem As Boolean) As String
            Dim t$ = String.Empty
            Dim s$ = SizeStr
            Dim __title$ = $" - {Title}"
            If Not s.IsEmptyString Then s = $" [{s}]"
            If Not PlaylistTitle.IsEmptyString And Not ForMediaItem Then t = $"{PlaylistTitle} - "
            Dim c% = {Count, ElementsNumber}.Max
            If IsMusic Then
                If c <= 1 Then t &= "Single" Else t &= "Album"
            Else
                t &= "Playlist"
            End If
            If Not PlaylistTitle.IsEmptyString And Not ForMediaItem Then t &= $" - {PlaylistTitle}"
            If PlaylistTitle = Title Then __title = String.Empty
            If ForMediaItem Then
                Return $"{t} ({c}){__title}"
            Else
                Return $"{t} ({c}){__title} ({AConvert(Of String)(Duration, TimeToStringProvider)}){s}"
            End If
        End Function
        Public Overrides Function Parse(ByVal Container As EContainer, ByVal Path As SFile, ByVal IsMusic As Boolean,
                                        Optional ByVal Token As Threading.CancellationToken = Nothing, Optional ByVal Progress As IMyProgress = Nothing) As Boolean
            _MediaType = IIf(IsMusic, Plugin.UserMediaTypes.Audio, Plugin.UserMediaTypes.Video)
            _ObjectType = Base.YouTubeMediaType.PlayList
            Me.IsMusic = IsMusic
            Return ParseFiles(Path, IsMusic, Token, Progress)
        End Function
    End Class
End Namespace