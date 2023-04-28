' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Tools
Imports SCrawler.DownloadObjects.STDownloader
Public Module MainModShared
    Public Property BATCH As BatchExecutor
    Private _BatchLogSent As Boolean = False
    ''' <param name="e"><see cref="EDP.None"/></param>
    Public Sub GlobalOpenPath(ByVal f As SFile, Optional ByVal e As ErrorsDescriber = Nothing)
        Dim b As Boolean = False
        If Not e.Exists Then e = EDP.None
        Try
            If f.Exists(SFO.Path, False) Then
                If MyDownloaderSettings.OpenFolderInOtherProgram AndAlso Not MyDownloaderSettings.OpenFolderInOtherProgram_Command.IsEmptyString Then
                    If BATCH Is Nothing Then BATCH = New BatchExecutor With {.RedirectStandardError = True}
                    b = True
                    With BATCH
                        .Reset()
                        .Execute({String.Format(MyDownloaderSettings.OpenFolderInOtherProgram_Command, f.PathWithSeparator)}, EDP.SendToLog + EDP.ThrowException)
                        If .HasError Or Not .ErrorOutput.IsEmptyString Then Throw New Exception(.ErrorOutput, .ErrorException)
                    End With
                Else
                    f.Open(SFO.Path,, e)
                End If
            End If
        Catch ex As Exception
            If b Then
                If Not _BatchLogSent Then ErrorsDescriber.Execute(EDP.SendToLog, ex, $"GlobalOpenPath({f.Path})") : _BatchLogSent = True
                f.Open(SFO.Path,, e)
            End If
        End Try
    End Sub
End Module