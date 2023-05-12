' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Net
Imports System.Threading
Imports PersonalUtilities.Forms.Toolbars
Namespace API.XVIDEOS
    Friend NotInheritable Class M3U8
        Private Sub New()
        End Sub
        Friend Shared Function Download(ByVal URL As String, ByVal Appender As String, ByVal f As SFile,
                                        ByVal Token As CancellationToken, ByVal Progress As MyProgress, ByVal UsePreProgress As Boolean) As SFile
            Try
                If Not URL.IsEmptyString Then
                    Using w As New WebClient
                        Dim r$ = w.DownloadString(URL)
                        If Not r.IsEmptyString Then
                            Dim l As List(Of String) = ListAddList(Nothing, r.StringFormatLines.StringToList(Of String)(vbNewLine).ListWithRemove(Function(v) v.Trim.StartsWith("#")),
                                                                   New ListAddParams With {.Converter = Function(Input) $"{Appender}/{Input.ToString.Trim}"})
                            If l.ListExists Then Return Base.M3U8Base.Download(l, f,, Token, Progress, UsePreProgress)
                        End If
                    End Using
                End If
                Return Nothing
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"[M3U8.Download({URL}, {Appender}, {f})]")
                Throw ex
            End Try
        End Function
    End Class
End Namespace