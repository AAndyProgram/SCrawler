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
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Clients
Namespace API.TikTok
    Friend Class UserData : Inherits UserDataBase
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
        End Sub
        Friend Sub New()
            SeparateVideoFolder = False
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                Dim PostIDs As List(Of String)
                Dim PostDate As Date? = Nothing
                Dim PostURL$ = String.Empty
                Dim r$
                URL = $"https://www.tiktok.com/@{Name}"
                r = Responser.GetResponse(URL)
                PostIDs = RegexEnvir.GetIDList(r)
                If PostIDs.ListExists Then
                    For Each __id$ In PostIDs
                        If Not _TempPostsList.Contains(__id) Then
                            _TempPostsList.Add(__id)
                            If RegexEnvir.GetVideoData(r, __id, PostURL, PostDate) Then
                                Select Case CheckDatesLimit(PostDate, CheckDateProvider)
                                    Case DateResult.Skip : Continue For
                                    Case DateResult.Exit : Exit Sub
                                End Select
                                If ID.IsEmptyString And Not PostURL.IsEmptyString Then ID = RegexEnvir.ExtractUserID(PostURL)
                                _TempMediaList.ListAddValue(MediaFromData(PostURL, __id, PostDate))
                            End If
                        Else
                            Exit Sub
                        End If
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]")
            End Try
        End Sub
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
        Private Function MediaFromData(ByVal _URL As String, ByVal PostID As String, ByVal PostDate As Date?) As UserMedia
            _URL = LinkFormatterSecure(RegexReplace(_URL.Replace("\", String.Empty), LinkPattern))
            Dim m As New UserMedia(_URL, UserMedia.Types.Video) With {.Post = New UserPost With {.ID = PostID}}
            If Not m.URL.IsEmptyString Then m.File = $"{PostID}.mp4"
            If PostDate.HasValue Then m.Post.Date = PostDate
            Return m
        End Function
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            If Responser.Status = Net.WebExceptionStatus.ConnectionClosed Then
                UserExists = False
                Return 1
            Else
                Return 0
            End If
        End Function
    End Class
End Namespace