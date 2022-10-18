' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Net
Imports PersonalUtilities.Tools.WEB
Namespace API.XVIDEOS
    Friend NotInheritable Class M3U8
        Private Sub New()
        End Sub
        Private Shared Function Save(ByVal URLs As List(Of String), ByVal ffmpegFile As SFile, ByVal f As SFile) As SFile
            Dim CachePath As SFile = Nothing
            Try
                If URLs.ListExists Then
                    Dim ConcatFile As SFile = f
                    If ConcatFile.Name.IsEmptyString Then ConcatFile.Name = "PlayListFile"
                    ConcatFile.Extension = "mp4"
                    CachePath = $"{f.PathWithSeparator}_Cache\{SFile.GetDirectories($"{f.PathWithSeparator}_Cache\",,, EDP.ReturnValue).ListIfNothing.Count + 1}\"
                    If CachePath.Exists(SFO.Path) Then
                        Dim p As New SFileNumbers(ConcatFile.Name,,, New ANumbers With {.Format = ANumbers.Formats.General})
                        ConcatFile = SFile.Indexed_IndexFile(ConcatFile,, p, EDP.ReturnValue)
                        Dim i%
                        Dim eFiles As New List(Of SFile)
                        Dim dFile As SFile = CachePath
                        dFile.Extension = "ts"
                        Using w As New WebClient
                            For i = 0 To URLs.Count - 1
                                dFile.Name = $"ConPart_{i}"
                                w.DownloadFile(URLs(i), dFile)
                                eFiles.Add(dFile)
                            Next
                        End Using
                        f = FFMPEG.ConcatenateFiles(eFiles, ffmpegFile, ConcatFile, p, EDP.ThrowException)
                        eFiles.Clear()
                        Return f
                    End If
                End If
                Return Nothing
            Finally
                CachePath.Delete(SFO.Path, SFODelete.None, EDP.None)
            End Try
        End Function
        Friend Shared Function Download(ByVal URL As String, ByVal Appender As String, ByVal ffmpegFile As SFile, ByVal f As SFile) As SFile
            Try
                If Not URL.IsEmptyString Then
                    Using w As New WebClient
                        Dim r$ = w.DownloadString(URL)
                        If Not r.IsEmptyString Then
                            Dim l As List(Of String) = ListAddList(Nothing, r.StringFormatLines.StringToList(Of String)(vbNewLine).ListWithRemove(Function(v) v.Trim.StartsWith("#")),
                                                                   New ListAddParams With {.Converter = Function(Input) $"{Appender}/{Input.ToString.Trim}"})
                            If l.ListExists Then Return Save(l, ffmpegFile, f)
                        End If
                    End Using
                End If
                Return Nothing
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendInLog, ex, $"[M3U8.Download({URL}, {Appender}, {ffmpegFile}, {f})]")
                Throw ex
            End Try
        End Function
    End Class
End Namespace