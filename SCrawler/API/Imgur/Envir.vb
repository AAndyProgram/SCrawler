' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Tools.WebDocuments.JSON
Imports System.Net
Imports SCrawler.API.Imgur.Declarations
Namespace API.Imgur.Declarations
    Friend Module Imgur_Declarations
        Friend ReadOnly PostRegex As New RegexStructure("/([\w\d]+?)(|\.[\w]{0,4})\Z", 1)
    End Module
End Namespace
Namespace API.Imgur
    Friend NotInheritable Class Envir
        Private Sub New()
        End Sub
        Friend Shared Function GetGallery(ByVal URL As String) As List(Of String)
            Try
                If Not Settings.ImgurClientID.IsEmptyString And Not URL.IsEmptyString Then
                    Dim __url$ = RegexReplace(URL, PostRegex)
                    If Not __url.IsEmptyString Then
                        __url = $"https://api.imgur.com/post/v1/albums/{__url}?client_id={Settings.ImgurClientID.Value}&include=media"
                        Using w As New WebClient
                            Dim r$ = w.DownloadString(__url)
                            If Not r.IsEmptyString Then
                                Using j As EContainer = JsonDocument.Parse(r).XmlIfNothing
                                    If j.Contains("media") Then
                                        Dim UrlsList As New List(Of String)
                                        Dim tmpUrl$
                                        For Each m As EContainer In j("media")
                                            tmpUrl = m.Value("url")
                                            If Not tmpUrl.IsEmptyString Then UrlsList.ListAddValue(tmpUrl, Base.LNC)
                                        Next
                                        Return UrlsList
                                    End If
                                End Using
                            End If
                        End Using
                    End If
                End If
                Return Nothing
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.ReturnValue + EDP.SendInLog, ex, $"[API.Imgur.Envir.GetGallery({URL})]", Nothing)
            End Try
        End Function
        Friend Shared Function GetImage(ByVal URL As String) As String
            Try
                If Not Settings.ImgurClientID.IsEmptyString And Not URL.IsEmptyString Then
                    Dim __url$ = RegexReplace(URL, PostRegex)
                    If Not __url.IsEmptyString Then
                        __url = $"https://api.imgur.com/3/image/{__url}?client_id={Settings.ImgurClientID.Value}&include=media"
                        Using w As New WebClient
                            Dim r$ = w.DownloadString(__url)
                            If Not r.IsEmptyString Then Return JsonDocument.Parse(r).XmlIfNothing.Value({"data"}, "link")
                        End Using
                    End If
                End If
                Return String.Empty
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.ReturnValue + EDP.SendInLog, ex, $"[API.Imgur.Envir.GetImage({URL})]", String.Empty)
            End Try
        End Function
    End Class
End Namespace