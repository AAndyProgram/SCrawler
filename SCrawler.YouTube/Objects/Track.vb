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
    Public Class Track : Inherits YouTubeMediaContainerBase
        Public Sub New()
            IsMusic = True
            ObjectType = Base.YouTubeMediaType.Single
        End Sub
        Protected Overrides Sub GenerateFileName()
            If Not FileSetManually Or _File.IsEmptyString Then
                Dim indx$ = String.Empty
                If PlaylistIndex > 0 Then indx = PlaylistIndex.NumToString(ANumbers.Formats.NumberGroup, PlaylistCount.ToString.Length)
                If Not indx.IsEmptyString Then indx &= ". "
                _File.Name = $"{indx}{Title}"
                If Not OutputAudioCodec.IsEmptyString Then
                    _File.Extension = OutputAudioCodec.StringToLower
                ElseIf Not MyYouTubeSettings.DefaultAudioCodecMusic.IsEmptyString Then
                    _File.Extension = MyYouTubeSettings.DefaultAudioCodecMusic.Value.StringToLower
                Else
                    _File.Extension = mp3
                End If
                _File = CleanFileName(_File)
            End If
        End Sub
        Public Overrides Function ToString(ByVal ForMediaItem As Boolean) As String
            Dim s$ = SizeStr
            If Not s.IsEmptyString Then s = $" [{s}]"
            Dim pls$ = String.Empty
            If PlaylistIndex > 0 Then pls = $"{PlaylistIndex.NumToString(ANumbers.Formats.NumberGroup, PlaylistCount.ToString.Length)}. "
            If ForMediaItem Then
                Return Title
            Else
                Return $"{pls}{Title} ({AConvert(Of String)(Duration, TimeToStringProvider)}){s}"
            End If
        End Function
        Public Overrides Function Parse(ByVal Container As EContainer, ByVal Path As SFile, ByVal IsMusic As Boolean,
                                        Optional ByVal Token As Threading.CancellationToken = Nothing, Optional ByVal Progress As IMyProgress = Nothing) As Boolean
            _MediaType = Plugin.UserMediaTypes.Audio
            _ObjectType = Base.YouTubeMediaType.Single
            Me.IsMusic = IsMusic
            If MyBase.Parse(Container, Path, IsMusic, Token, Progress) Then
                With MyYouTubeSettings
                    Dim f As SFile = .OutputPath
                    If f.IsEmptyString Then f = "YouTubeDownloads\OutputFile.mp3"
                    Dim ext$ = .DefaultAudioCodecMusic.Value.StringToLower.IfNullOrEmpty(.DefaultAudioCodec.Value.StringToLower)
                    If ext.IsEmptyString Then ext = "mp3"
                    f.Extension = ext
                    'If f.Name.IsEmptyString Then f.Name = File.Name
                    File = f
                    If _File.Extension.IsEmptyString Then _File.Extension = ext
                    _File = CleanFileName(_File)
                End With
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace