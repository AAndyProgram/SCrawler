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
    Public Class Channel : Inherits YouTubeMediaContainerBase
        Public Sub New()
            ObjectType = Base.YouTubeMediaType.Channel
        End Sub
        Public Overrides Function ToString(ByVal ForMediaItem As Boolean) As String
            Return UserTitle
        End Function
        Public Overrides Function Parse(ByVal Container As EContainer, ByVal Path As SFile, ByVal IsMusic As Boolean,
                                        Optional ByVal Token As Threading.CancellationToken = Nothing, Optional ByVal Progress As IMyProgress = Nothing) As Boolean
            _MediaType = IIf(IsMusic, Plugin.UserMediaTypes.Audio, Plugin.UserMediaTypes.Video)
            _ObjectType = Base.YouTubeMediaType.Channel
            Me.IsMusic = IsMusic
            If ParseFiles(Path, IsMusic, Token, Progress) Then
                PlaylistID = String.Empty
                PlaylistIndex = 0
                PlaylistTitle = String.Empty
                ThrowAny(Token)

                'Reconfiguration
                If IsMusic AndAlso HasElements AndAlso Elements.Exists(Function(e) Not e.PlaylistID.IsEmptyString) Then
                    Dim elems As New List(Of IYouTubeMediaContainer)(Elements)
                    Dim elemsNew As New List(Of IYouTubeMediaContainer)
                    Dim playlistDic As New Dictionary(Of String, List(Of IYouTubeMediaContainer))
                    Elements.Clear()
                    For Each elem In elems
                        If Not elem.PlaylistTitle.IsEmptyString Then
                            If Not playlistDic.ContainsKey(elem.PlaylistTitle) Then playlistDic.Add(elem.PlaylistTitle, New List(Of IYouTubeMediaContainer))
                            playlistDic(elem.PlaylistTitle).Add(elem)
                        ElseIf elem.PlaylistID = elem.UserID Then
                            elem.PlaylistID = String.Empty
                            elem.PlaylistIndex = -1
                            elem.PlaylistTitle = String.Empty
                            elemsNew.Add(elem)
                        Else
                            elemsNew.Add(elem)
                        End If
                    Next
                    If playlistDic.Count > 0 Then
                        Dim i%, ii%
                        Dim v As YouTubeMediaContainerBase
                        For Each kv In playlistDic
                            i = -1
                            If elemsNew.Count > 0 Then i = elemsNew.FindIndex(Function(e) e.PlaylistID = kv.Key)
                            If i = -1 Then
                                elemsNew.Add(New PlayList)
                                v = kv.Value.First
                                With DirectCast(elemsNew.Last, YouTubeMediaContainerBase)
                                    .ObjectType = Base.YouTubeMediaType.PlayList
                                    .MediaType = v.MediaType
                                    .IsMusic = v.IsMusic
                                    .ID = v.PlaylistID
                                    .Title = v.PlaylistTitle
                                    .PlaylistID = .ID
                                    .PlaylistTitle = .Title
                                    .PlaylistIndex = -1
                                    .UserID = v.UserID
                                    .UserTitle = v.UserTitle
                                End With
                                i = elemsNew.Count - 1
                            End If
                            With elemsNew(i).Elements
                                .AddRange(kv.Value)
                                If .Count > 0 Then
                                    For ii = 0 To .Count - 1
                                        With DirectCast(.Item(ii), YouTubeMediaContainerBase)
                                            .PlaylistIndex = ii + 1
                                            .PlaylistCount = kv.Value.Count
                                        End With
                                    Next
                                End If
                            End With
                        Next
                        playlistDic.Clear()
                    End If
                    If elemsNew.Count > 0 Then Elements.AddRange(elemsNew)
                    elems.Clear()
                    elemsNew.Clear()
                    File = MyYouTubeSettings.OutputPath
                End If

                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace