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
Namespace API.Xhamster
    Friend NotInheritable Class M3U8
        Private Sub New()
        End Sub
        Private Shared Function ParseFirstM3U8(ByVal URL As String, ByVal Responser As Responser, ByVal UHD As Boolean) As String
            Dim r$, d$
            Dim _DataObtained As Boolean = False
            For i% = 0 To 1
                Try
                    Responser.UseGZipStream = i
                    r = Responser.GetResponse(URL.Replace("\", String.Empty))
                    If Not r.IsEmptyString Then
                        r = r.StringFormatLines
                        Dim sList As List(Of Sizes) = RegexFields(Of Sizes)(r, {FirstM3U8FileRegEx}, {1, 2})
                        If sList.ListExists Then _DataObtained = True : sList.RemoveAll(Function(sv) sv.HasError Or sv.Data.IsEmptyString Or
                                                                                                     sv.Value = 0 Or (Not UHD And sv.Value > 1080))
                        If sList.ListExists Then
                            sList.Sort()
                            d = sList.First.Data.Trim
                            If Not d.IsEmptyString Then Return d
                        End If
                    End If
                Catch
                End Try
                If _DataObtained Then Exit For
            Next
            Return String.Empty
        End Function
        Private Shared Function ParseSecondM3U8(ByVal URL As String, ByVal Responser As Responser, ByVal Appender As String) As List(Of String)
            Dim r$
            Dim l As List(Of String)
            For i% = 0 To 1
                Try
                    Responser.UseGZipStream = i
                    r = Responser.GetResponse(URL)
                    If Not r.IsEmptyString Then
                        l = RegexReplace(r, TsFilesRegEx)
                        If l.ListExists Then
                            For indx% = 0 To l.Count - 1 : l(indx) = M3U8Base.CreateUrl(Appender, l(indx)) : Next
                            Return l
                        End If
                    End If
                Catch
                End Try
            Next
            Return Nothing
        End Function
        Private Shared Function ObtainUrls(ByVal URL As String, ByVal Responser As Responser, ByVal UHD As Boolean) As List(Of String)
            Try
                Dim file$ = ParseFirstM3U8(URL, Responser, UHD)
                If Not file.IsEmptyString Then
                    Responser.UseGZipStream = False
                    Dim appender$ = URL.Replace(URL.Split("/").LastOrDefault, String.Empty)
                    URL = M3U8Base.CreateUrl(appender, file)
                    Dim l As List(Of String) = ParseSecondM3U8(URL, Responser, appender)
                    If l.ListExists Then Return l
                End If
                Return Nothing
            Finally
                Responser.UseGZipStream = False
            End Try
        End Function
        Friend Shared Function Download(ByVal Media As UserMedia, ByVal Responser As Responser, ByVal UHD As Boolean) As SFile
            Return M3U8Base.Download(ObtainUrls(Media.URL, Responser, UHD), Media.File, Responser)
        End Function
    End Class
End Namespace