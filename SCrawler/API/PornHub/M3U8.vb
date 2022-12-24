' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports SCrawler.API.Base.M3U8Declarations
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Clients
Namespace API.PornHub
    Friend NotInheritable Class M3U8
        Private Sub New()
        End Sub
        Private Shared Function GetUrlsList(ByVal URL As String, ByVal Responser As Responser) As List(Of String)
            Dim appender$ = RegexReplace(URL, Regex_M3U8_FileUrl)
            Dim r$ = Responser.GetResponse(URL)
            If Not r.IsEmptyString Then
                Dim file$ = RegexReplace(r, Regex_M3U8_FirstFileRegEx)
                If Not file.IsEmptyString Then
                    Dim NewUrl$ = M3U8Base.CreateUrl(appender, file)
                    If Not NewUrl.IsEmptyString Then
                        r = Responser.GetResponse(NewUrl)
                        If Not r.IsEmptyString Then
                            Dim l As List(Of String) = RegexReplace(r, TsFilesRegEx)
                            If l.ListExists Then
                                For i% = 0 To l.Count - 1 : l(i) = M3U8Base.CreateUrl(appender, l(i)) : Next
                                Return l
                            End If
                        End If
                    End If
                End If
            End If
            Return Nothing
        End Function
        Friend Shared Function Download(ByVal URL As String, ByVal Responser As Responser, ByVal Destination As SFile) As SFile
            Return M3U8Base.Download(GetUrlsList(URL, Responser), Destination, Responser)
        End Function
    End Class
End Namespace