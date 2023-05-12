' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports SCrawler.API.Base
Imports SCrawler.API.Base.M3U8Declarations
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.PornHub
    Friend NotInheritable Class M3U8
        Private Sub New()
        End Sub
        Private Shared Function GetUrlsList(ByVal URL As String, ByVal Responser As Responser, ByVal DownloadUHD As Boolean) As List(Of String)
            Dim appender$ = RegexReplace(URL, Regex_M3U8_FileUrl)
            Dim r$ = Responser.GetResponse(URL)
            If Not r.IsEmptyString Then
                Dim files As List(Of Sizes) = RegexFields(Of Sizes)(r, {Regex_M3U8_FilesList}, {1, 2}, EDP.ReturnValue)
                Dim file$
                If files.ListExists Then files.RemoveAll(Function(f) f.Value = 0 Or (Not DownloadUHD And f.Value > 1080))
                If files.ListExists Then
                    files.Sort()
                    file = files(0).Data
                Else
                    file = RegexReplace(r, Regex_M3U8_FirstFileRegEx)
                End If
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
        Friend Shared Function Download(ByVal URL As String, ByVal Responser As Responser, ByVal Destination As SFile, ByVal DownloadUHD As Boolean,
                                        ByVal Token As CancellationToken, ByVal Progress As MyProgress, ByVal UsePreProgress As Boolean) As SFile
            Return M3U8Base.Download(GetUrlsList(URL, Responser, DownloadUHD), Destination, Responser, Token, Progress, UsePreProgress)
        End Function
    End Class
End Namespace