' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Net
Imports System.Threading
Imports SCrawler.API.Base
Imports SCrawler.API.Imgur.Declarations
Imports SCrawler.API.YouTube.Objects
Imports SCrawler.Plugin.Hosts
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Documents.JSON
Namespace API.Imgur
    Namespace Declarations
        Friend Module Imgur_Declarations
            Friend ReadOnly PostRegex As RParams = RParams.DMS("/([^/]+?)(|#.*?|\.[\w]{0,4})(|\?.*?)\Z", 1)
        End Module
    End Namespace
    Friend NotInheritable Class Envir : Inherits UserDataBase
        Friend Const SiteKey As String = "AndyProgram_Imgur"
        Friend Const SiteName As String = "Imgur"
        Friend Sub New()
        End Sub
#Region "UserDataBase Support"
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
        End Sub
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            SeparateVideoFolder = False
            DownloadContentDefault(Token)
        End Sub
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            Return 0
        End Function
#End Region
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            Dim videos As IEnumerable(Of UserMedia) = GetVideoInfo(Data.URL, EDP.SendToLog)
            If videos.ListExists Then _TempMediaList.AddRange(videos)
        End Sub
        Friend Shared Function GetGallery(ByVal URL As String, Optional ByVal e As ErrorsDescriber = Nothing) As List(Of String)
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
                Return DownloadingException_Internal(ex, $"[API.Imgur.Envir.GetGallery({URL})]", Nothing, e)
            End Try
        End Function
        Friend Shared Function GetImage(ByVal URL As String, Optional ByVal e As ErrorsDescriber = Nothing) As String
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
                Return DownloadingException_Internal(ex, $"[API.Imgur.Envir.GetImage({URL})]", String.Empty, e)
            End Try
        End Function
        Friend Shared Function GetVideoInfo(ByVal URL As String, Optional ByVal e As ErrorsDescriber = Nothing) As IEnumerable(Of UserMedia)
            Try
                If Not URL.IsEmptyString AndAlso URL.ToLower.Contains("imgur") AndAlso Not Settings.ImgurClientID.IsEmptyString Then
                    Dim imgList As List(Of String) = GetGallery(URL, EDP.ReturnValue)
                    If imgList.ListExists Then
                        Return imgList.Select(Function(u) New UserMedia(u))
                    Else
                        Dim img$ = GetImage(URL, EDP.ReturnValue)
                        If Not img.IsEmptyString Then Return {New UserMedia(img)}
                    End If
                End If
                Return Nothing
            Catch ex As Exception
                If Not e.Exists Then e = EDP.LogMessageValue
                Return ErrorsDescriber.Execute(e, ex, $"[API.Imgur.Envir.GetVideoInfo({URL})]: fetch media error")
            End Try
        End Function
        Friend Shared Function GetSingleMediaInstance(ByVal URL As String, ByVal OutputFile As SFile) As IYouTubeMediaContainer
            If Not URL.IsEmptyString AndAlso URL.Contains("imgur.com") Then
                Return New DownloadableMediaHost(URL, OutputFile) With {
                    .Instance = New Envir,
                    .Site = SiteName,
                    .SiteKey = SiteKey,
                    .SiteIcon = Nothing
                }
            Else
                Return Nothing
            End If
        End Function
        Private Shared Function DownloadingException_Internal(ByVal ex As Exception, ByVal Message As String,
                                                              ByVal NullArg As Object, ByVal e As ErrorsDescriber) As Object
            If TypeOf ex Is WebException Then
                Dim obj As HttpWebResponse = TryCast(DirectCast(ex, WebException).Response, HttpWebResponse)
                If Not obj Is Nothing Then
                    If obj.StatusCode = HttpStatusCode.NotFound Then
                        Return NullArg
                    ElseIf obj.StatusCode = HttpStatusCode.Unauthorized Then
                        MyMainLOG = "Imgur credentials expired"
                        Return NullArg
                    End If
                End If
            End If
            If Not e.Exists Then e = New ErrorsDescriber(EDP.ReturnValue + EDP.SendToLog)
            Return ErrorsDescriber.Execute(e, ex, Message, NullArg)
        End Function
    End Class
End Namespace