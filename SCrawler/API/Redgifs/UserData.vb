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
Imports UStates = SCrawler.API.Base.UserMedia.States
Namespace API.RedGifs
    Friend Class UserData : Inherits UserDataBase
        Friend Sub New()
        End Sub
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            ReparseMissing(Token)
            DownloadData(1, Token)
        End Sub
        Private Overloads Sub DownloadData(ByVal Page As Integer, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                URL = $"https://api.redgifs.com/v2/users/{Name}/search?order=recent&page={Page}"
                Dim r$ = Responser.GetResponse(URL,, EDP.ThrowException)
                Dim postDate$, postID$
                Dim pTotal% = 0
                If Not r.IsEmptyString Then
                    Using j As EContainer = JsonDocument.Parse(r).XmlIfNothing
                        If j.Contains("gifs") Then
                            pTotal = j.Value("pages").FromXML(Of Integer)(0)
                            For Each g As EContainer In j("gifs")
                                postDate = g.Value("createDate")
                                If Not CheckDatesLimit(postDate, DateProvider) Then Exit Sub
                                postID = g.Value("id")
                                If Not _TempPostsList.Contains(postID) Then _TempPostsList.Add(postID) Else Exit For
                                ObtainMedia(g, postID, postDate)
                            Next
                        End If
                    End Using
                End If
                If pTotal > 0 And Page < pTotal Then DownloadData(Page + 1, Token)
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]")
            End Try
        End Sub
        Private Sub ObtainMedia(ByVal j As EContainer, ByVal PostID As String,
                                Optional ByVal PostDateStr As String = Nothing, Optional ByVal PostDateDate As Date? = Nothing,
                                Optional ByVal State As UStates = UStates.Unknown)
            With j("urls")
                If .ListExists Then
                    Dim u$ = If(.Item("hd"), .Item("sd")).XmlIfNothingValue
                    If Not u.IsEmptyString Then
                        Dim ut As UTypes = UTypes.Undefined
                        'Type 1: video
                        'Type 2: image
                        Select Case j.Value("type").FromXML(Of Integer)(0)
                            Case 1 : ut = UTypes.Video
                            Case 2 : ut = UTypes.Picture
                        End Select
                        If Not ut = UTypes.Undefined Then _TempMediaList.ListAddValue(MediaFromData(ut, u, PostID, PostDateStr, PostDateDate, State))
                    End If
                End If
            End With
        End Sub
        Protected Overrides Sub ReparseMissing(ByVal Token As CancellationToken)
            Dim rList As New List(Of Integer)
            Try
                If _ContentList.Exists(MissingFinder) Then
                    Dim url$, r$
                    Dim u As UserMedia
                    Dim j As EContainer
                    For i% = 0 To _ContentList.Count - 1
                        If _ContentList(i).State = UserMedia.States.Missing Then
                            ThrowAny(Token)
                            u = _ContentList(i)
                            If Not u.Post.ID.IsEmptyString Then
                                url = $"https://api.redgifs.com/v2/gifs/{u.Post.ID}?views=yes&users=yes"
                                Try
                                    r = Responser.GetResponse(url,, EDP.ThrowException)
                                    If Not r.IsEmptyString Then
                                        j = JsonDocument.Parse(r)
                                        If Not j Is Nothing Then
                                            If If(j("gif")?.Count, 0) > 0 Then
                                                ObtainMedia(j("gif"), u.Post.ID,, u.Post.Date, UStates.Missing)
                                                rList.Add(i)
                                            End If
                                        End If
                                    End If
                                Catch down_ex As Exception
                                    u.Attempts += 1
                                    _ContentList(i) = u
                                End Try
                            Else
                                rList.Add(i)
                            End If
                        End If
                    Next
                End If
            Catch dex As ObjectDisposedException When Disposed
            Catch ex As Exception
                ProcessException(ex, Token, $"missing data downloading error")
            Finally
                If Not Disposed And rList.Count > 0 Then
                    For i% = rList.Count - 1 To 0 Step -1 : _ContentList.RemoveAt(rList(i)) : Next
                End If
            End Try
        End Sub
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
        Private Shared Function MediaFromData(ByVal t As UTypes, ByVal _URL As String, ByVal PostID As String,
                                              ByVal PostDateStr As String, ByVal PostDateDate As Date?, ByVal State As UStates) As UserMedia
            _URL = LinkFormatterSecure(RegexReplace(_URL.Replace("\", String.Empty), LinkPattern))
            Dim m As New UserMedia(_URL, t) With {.Post = New UserPost With {.ID = PostID}}
            If Not m.URL.IsEmptyString Then m.File = CStr(RegexReplace(m.URL, FilesPattern)) : m.URL_BASE = m.URL
            If Not PostDateStr.IsEmptyString Then
                m.Post.Date = AConvert(Of Date)(PostDateStr, DateProvider, Nothing)
            ElseIf PostDateDate.HasValue Then
                m.Post.Date = PostDateDate
            Else
                m.Post.Date = Nothing
            End If
            m.State = State
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