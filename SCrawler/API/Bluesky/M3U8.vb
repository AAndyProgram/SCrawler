' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports SCrawler.API.Base
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Bluesky
    Friend NotInheritable Class M3U8
        Private Sub New()
        End Sub
        Private Shared Function GetUrlsList(ByVal URL As String) As List(Of String)
            Using resp As New Responser With {.AllowAutoRedirect = False}
                Dim r$ = resp.GetResponse(URL)
                If Not r.IsEmptyString Then
                    Dim file$ = String.Empty, appender$
                    Dim files As List(Of Sizes) = RegexFields(Of Sizes)(r, {RegEx_PlayLists}, {1, 2})
                    If files.ListExists Then files.RemoveAll(Function(ff) ff.Value = 0 Or ff.Data.IsEmptyString)
                    If files.ListExists Then
                        files.Sort()
                        file = files(0).Data
                        appender = URL.Replace(URL.Split("/").Last, String.Empty)
                        file = M3U8Base.CreateUrl(appender, file)
                        If Not file.IsEmptyString Then
                            r = resp.GetResponse(file)
                            If Not r.IsEmptyString Then
                                Dim l As List(Of String) = RegexReplace(r, M3U8Declarations.TsFilesRegEx)
                                If l.ListExists Then
                                    appender = file.Replace(file.Split("/").Last, String.Empty)
                                    For i% = 0 To l.Count - 1 : l(i) = M3U8Base.CreateUrl(appender, l(i)) : Next
                                    Return l
                                End If
                            End If
                        End If
                    End If
                End If
            End Using
            Return Nothing
        End Function
        Friend Shared Function Download(ByVal URL As String, ByVal Destination As SFile, ByVal Token As CancellationToken,
                                        ByVal Progress As MyProgress, ByVal UsePreProgress As Boolean) As SFile
            Return M3U8Base.Download(GetUrlsList(URL), Destination,, Token, Progress, UsePreProgress)
        End Function
    End Class
End Namespace