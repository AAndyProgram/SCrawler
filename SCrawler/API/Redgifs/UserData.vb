' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.WebDocuments.JSON
Imports System.Net
Imports System.Threading
Imports SCrawler.API.Base
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace API.RedGifs
    Friend Class UserData : Inherits UserDataBase
        Friend Sub New()
        End Sub
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            DownloadData(1, Token)
        End Sub
        Private Overloads Sub DownloadData(ByVal Page As Integer, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                URL = $"https://api.redgifs.com/v2/users/{Name}/search?order=recent&page={Page}"
                Dim r$ = Responser.GetResponse(URL,, EDP.ThrowException)
                Dim postDate$, postID$
                Dim pTotal% = 0
                Dim u$
                Dim ut As UTypes
                If Not r.IsEmptyString Then
                    Using j As EContainer = JsonDocument.Parse(r).XmlIfNothing
                        If j.Contains("gifs") Then
                            pTotal = j.Value("pages").FromXML(Of Integer)(0)
                            For Each g As EContainer In j("gifs")
                                postDate = g.Value("createDate")
                                If Not CheckDatesLimit(postDate, DateProvider) Then Exit Sub
                                postID = g.Value("id")
                                If Not _TempPostsList.Contains(postID) Then _TempPostsList.Add(postID) Else Exit For
                                With g("urls")
                                    If .ListExists Then
                                        u = If(.Item("hd"), .Item("sd")).XmlIfNothingValue
                                        If Not u.IsEmptyString Then
                                            ut = UTypes.Undefined
                                            'Type 1: video
                                            'Type 2: image
                                            Select Case g.Value("type").FromXML(Of Integer)(0)
                                                Case 1 : ut = UTypes.Video
                                                Case 2 : ut = UTypes.Picture
                                            End Select
                                            If Not ut = UTypes.Undefined Then _TempMediaList.ListAddValue(MediaFromData(ut, u, postID, postDate))
                                        End If
                                    End If
                                End With
                            Next
                        End If
                    End Using
                End If
                If pTotal > 0 And Page < pTotal Then DownloadData(Page + 1, Token)
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]")
            End Try
        End Sub
        Protected Overrides Sub ReparseVideo(ByVal Token As CancellationToken)
        End Sub
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
        Private Shared Function MediaFromData(ByVal t As UTypes, ByVal _URL As String, ByVal PostID As String, ByVal PostDate As String) As UserMedia
            _URL = LinkFormatterSecure(RegexReplace(_URL.Replace("\", String.Empty), LinkPattern))
            Dim m As New UserMedia(_URL, t) With {.Post = New UserPost With {.ID = PostID}}
            If Not m.URL.IsEmptyString Then m.File = CStr(RegexReplace(m.URL, FilesPattern)) : m.URL_BASE = m.URL
            If Not PostDate.IsEmptyString Then m.Post.Date = AConvert(Of Date)(PostDate, DateProvider, Nothing) Else m.Post.Date = Nothing
            Return m
        End Function
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False) As Integer
            If Responser.StatusCode = HttpStatusCode.NotFound Then
                UserExists = False
            Else
                If Not FromPE Then LogError(ex, Message) : HasError = True
                Return 0
            End If
            Return 1
        End Function
    End Class
End Namespace