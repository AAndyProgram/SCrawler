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
Imports SCrawler.API.YouTube.Objects
Imports SCrawler.Plugin.Hosts
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Gfycat
    Friend NotInheritable Class Envir : Inherits UserDataBase
        Friend Const SiteKey As String = "AndyProgram_Gfycat"
        Friend Const SiteName As String = "Gfycat"
        Friend Sub New()
        End Sub
#Region "UserDataBase Support"
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As PersonalUtilities.Functions.XML.XmlFile, ByVal Loading As Boolean)
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
#Region "DownloadSingleObject"
        Private _IsRedGifs As Boolean = False
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            Dim urlVideo$ = GetVideo(Data.URL)
            If Not urlVideo.IsEmptyString Then
                If urlVideo.Contains("redgifs.com") Then
                    _IsRedGifs = True
                    DirectCast(Settings(RedGifs.RedGifsSiteKey).Default.Source, RedGifs.SiteSettings).UpdateTokenIfRequired()
                    Dim newData As IYouTubeMediaContainer = Settings(RedGifs.RedGifsSiteKey).Default.GetSingleMediaInstance(urlVideo, Data.File)
                    If Not newData Is Nothing Then
                        newData.Progress = Data.Progress
                        newData.Download(Data.UseCookies, Token)
                        YouTubeMediaContainerBase.Update(newData, Data)
                        DirectCast(Data, DownloadableMediaHost).ExchangeData(newData, Data)
                        With DirectCast(Data, YouTubeMediaContainerBase)
                            .Site = RedGifs.RedGifsSite
                            .SiteKey = RedGifs.RedGifsSiteKey
                            .SiteIcon = Settings(RedGifs.RedGifsSiteKey).Default.Source.Image
                        End With
                    Else
                        Throw New Exception($"Unable to get RedGifs instance{vbCr}{Data.URL}{vbCr}{urlVideo}")
                    End If
                Else
                    Dim m As New UserMedia(urlVideo, UserMedia.Types.Video) With {.URL_BASE = Data.URL}
                    m.File.Path = Data.File.Path
                    _TempMediaList.Add(m)
                End If
            End If
        End Sub
        Protected Overrides Sub DownloadSingleObject_CreateMedia(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            If Not _IsRedGifs Then MyBase.DownloadSingleObject_CreateMedia(Data, Token)
        End Sub
        Protected Overrides Sub DownloadSingleObject_PostProcessing(ByVal Data As IYouTubeMediaContainer, Optional ByVal ResetTitle As Boolean = True)
            If Not _IsRedGifs Then MyBase.DownloadSingleObject_PostProcessing(Data, ResetTitle)
        End Sub
        Protected Overrides Sub DownloadSingleObject_Download(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            If Not _IsRedGifs Then MyBase.DownloadSingleObject_Download(Data, Token)
        End Sub
#End Region
        Friend Shared Function GetVideo(ByVal URL As String) As String
            Try
                Dim r$
                Using w As New WebClient : r = w.DownloadString(URL) : End Using
                If Not r.IsEmptyString Then
                    Dim _url$ = RegexReplace(r, RParams.DMS("contentUrl.:.(http.?://[^""]+?\.mp4)", 1, EDP.ReturnValue))
                    If Not _url.IsEmptyString Then
                        If _url.Contains("redgifs.com") Then
                            _url = RegexReplace(_url, RParams.DMS("([^/-]+)[-\w]*\.mp4", 1, EDP.ReturnValue))
                            If Not _url.IsEmptyString Then Return $"https://www.redgifs.com/watch/{_url}"
                        Else
                            Return _url
                        End If
                    End If
                End If
                Return String.Empty
            Catch ex As Exception
                Dim e As EDP = EDP.ReturnValue
                If TypeOf ex Is WebException Then
                    Dim obj As HttpWebResponse = TryCast(DirectCast(ex, WebException).Response, HttpWebResponse)
                    If Not If(obj?.StatusCode, HttpStatusCode.OK) = HttpStatusCode.NotFound Then e += EDP.SendToLog
                End If
                Return ErrorsDescriber.Execute(e, ex, $"[API.Gfycat.Envir.GetVideo({URL})]", String.Empty)
            End Try
        End Function
        Friend Shared Function GetSingleMediaInstance(ByVal URL As String, ByVal OutputFile As SFile) As IYouTubeMediaContainer
            If Not URL.IsEmptyString AndAlso URL.Contains("gfycat") Then
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
    End Class
End Namespace