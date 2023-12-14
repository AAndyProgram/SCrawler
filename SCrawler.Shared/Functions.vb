' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace [Shared]
    Public Module Functions
        Public Const NewReleaseFolderName As String = "__NewRelease"
        Public Function GetCurrentMaxVer(Optional ByVal Path As SFile = Nothing) As Version
            Try
                If Path.IsEmptyString Then Path = Application.StartupPath.CSFileP
                If Path.Exists(SFO.Path, False) Then
                    Dim versions As New List(Of Version)
                    Dim v As FileVersionInfo
                    With SFile.GetFiles(Path, "*.exe",, EDP.ReturnValue).ListIfNothing.Where(Function(f) f.Name = "SCrawler" Or f.Name = "YouTubeDownloader")
                        If .ListExists Then
                            For Each f As SFile In .Self
                                v = FileVersionInfo.GetVersionInfo(f)
                                versions.Add(New Version(v.ProductVersion))
                            Next
                        End If
                    End With
                    If versions.Count > 0 Then Return versions.Max
                End If
            Catch
            End Try
            Return Nothing
        End Function
    End Module
End Namespace