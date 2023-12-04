' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports SCrawler.API.YouTube.Objects
Namespace API.YouTube.Base
    Public NotInheritable Class YouTubeFunctions
        Public Const YouTubeCachePathRoot As String = "_CacheYouTube\"
        Public Const UserChannelOption As String = "channel"
        Public Const TrueUrlPattern As String = "https?://[^/]*?youtube.com/[^\?/&]+((\??[^\?/&]+|/[^\?/&]+))"
        '2 - type; 5 - id
        Public Const UrlTypePattern As String = "(?<=https?://[^/]*?youtube.com/)((@|[^\?/&]+))([/\?]{0,1}(list=|v=|)([^\?/&]*))(?=(\S+|\Z|))"
        Private Sub New()
        End Sub
        Public Shared Function StandardizeURL(ByVal URL As String) As String
            Try
                Dim isMusic As Boolean = False, isShorts As Boolean = False
                If Info_GetUrlType(URL, isMusic, isShorts) = YouTubeMediaType.Single Then
                    If Not isMusic And Not isShorts Then
                        Dim videoOptionRegex As RParams = RParams.DMS("[\?&]v=([^\?&]+)", 1, EDP.ReturnValue)
                        Dim data As List(Of String) = RegexReplace(URL, RParams.DMS(UrlTypePattern, 0, RegexReturn.ListByMatch, EDP.ReturnValue))
                        Dim val$ = String.Empty
                        If data.ListExists Then
                            For Each d$ In data
                                val = RegexReplace(d, videoOptionRegex)
                                If Not val.IsEmptyString Then Exit For
                            Next
                            data.Clear()
                        End If
                        If Not val.IsEmptyString Then Return $"https://www.youtube.com/watch?v={val}"
                    End If
                End If
                Return URL
            Catch ex As Exception
                Return URL
            End Try
        End Function
        Public Shared Function IsMyUrl(ByVal URL As String) As Boolean
            Return Not Info_GetUrlType(URL) = YouTubeMediaType.Undefined
        End Function
        Public Shared Function Info_GetUrlType(ByVal URL As String, Optional ByRef IsMusic As Boolean = False, Optional ByRef IsShorts As Boolean = False,
                                               Optional ByRef IsChannelUser As Boolean = False, Optional ByRef Id As String = Nothing) As YouTubeMediaType
            If Not URL.IsEmptyString Then
                IsMusic = URL.Contains("music.youtube.com")
                IsChannelUser = False
                IsShorts = False
                Dim data As List(Of String) = RegexReplace(URL, RParams.DMS(UrlTypePattern, 0, RegexReturn.ListByMatch, EDP.ReturnValue))
                If data.ListExists Then
                    If data.Count >= 6 Then Id = data(5)
                    If data.Count >= 3 And Not data(2).IsEmptyString Then
                        Select Case data(2).ToLower
                            Case "watch" : Return YouTubeMediaType.Single
                            Case "shorts" : IsShorts = True : Return YouTubeMediaType.Single
                            Case "playlist" : Return YouTubeMediaType.PlayList
                            Case UserChannelOption, "@" : IsChannelUser = data(2).ToLower = UserChannelOption : Return YouTubeMediaType.Channel
                        End Select
                    End If
                End If
            End If
            Return YouTubeMediaType.Undefined
        End Function
        ''' <summary>'--no-cookies-from-browser --cookies CookiesFile'</summary>
        Public Shared Function GetCookiesCommand(ByVal UseCookies As Boolean, ByVal CookiesFile As SFile) As String
            If UseCookies And CookiesFile.Exists Then
                Return $"--no-cookies-from-browser --cookies ""{CookiesFile}"""
            Else
                Return String.Empty
            End If
        End Function
        ''' <param name="DateAfter">Data with upload date 'more than or equal to' date will be downloaded</param>
        ''' <param name="DateBefore">Data with upload date 'less than or equal to' date will be downloaded</param>
        ''' <exception cref="ArgumentNullException"></exception>
        ''' <exception cref="IO.FileNotFoundException"></exception>
        ''' <exception cref="InvalidOperationException"></exception>
        Public Shared Function Parse(ByVal URL As String, Optional ByVal UseCookies As Boolean? = Nothing,
                                     Optional ByVal Token As Threading.CancellationToken = Nothing, Optional ByVal Progress As IMyProgress = Nothing,
                                     Optional ByVal GetDefault As Boolean? = Nothing, Optional ByVal GetShorts As Boolean? = Nothing,
                                     Optional ByVal DateAfter As Date? = Nothing, Optional ByVal DateBefore As Date? = Nothing) As IYouTubeMediaContainer
            If URL.IsEmptyString Then Throw New ArgumentNullException("URL", "URL cannot be null")
            If Not MyYouTubeSettings.YTDLP.Value.Exists Then Throw New IO.FileNotFoundException("Path to 'yt-dlp.exe' not set or program not found at destination", MyYouTubeSettings.YTDLP.Value.ToString)
            Dim urlOrig$ = URL
            URL = RegexReplace(URL, TrueUrlRegEx)
            If URL.IsEmptyString Then Throw New ArgumentNullException("URL", $"Can't get true URL from [{urlOrig}]")
            Dim isMusic As Boolean = False, isShorts As Boolean = False
            Dim objType As YouTubeMediaType = Info_GetUrlType(URL, isMusic, isShorts)
            If Not objType = YouTubeMediaType.Undefined Then
                Dim __GetDefault As Boolean = If(GetDefault, True)
                Dim __GetShorts As Boolean = If(GetShorts, True)
                If isMusic Then __GetShorts = False
                Dim container As IYouTubeMediaContainer
                Dim pattern$ = "%(channel_id)s_%(id)s_%(playlist_index)s"

                Select Case objType
                    Case YouTubeMediaType.Single
                        __GetShorts = False
                        If isMusic Then container = New Track Else container = New Video
                    Case YouTubeMediaType.PlayList : container = New PlayList : pattern = "%(playlist_index)s_%(id)s" : __GetShorts = False
                    Case YouTubeMediaType.Channel
                        container = New Channel
                        If isMusic Then pattern = "%(playlist_id)s/%(channel_id)s_%(id)s_%(playlist_index)s"
                    Case Else : Throw New InvalidOperationException($"Type '{objType}' is not supported by YouTubeDownloader")
                End Select

                If UseCookies.HasValue Then container.UseCookies = UseCookies.Value
                Dim result As Boolean = False
                Dim cookiesExists As Boolean = YouTubeCookieNetscapeFile.Exists
                Dim _CachePathDefault As SFile = MyCache.NewPath(, EDP.ReturnValue)
                If _CachePathDefault.IsEmptyString Then _CachePathDefault = $"{YouTubeCachePathRoot}{SFile.GetDirectories(YouTubeCachePathRoot,,, EDP.ReturnValue).Count + 1}"
                _CachePathDefault.Exists(SFO.Path, True, EDP.ThrowException)
                pattern = $"{_CachePathDefault.PathWithSeparator}{pattern}"

                Dim withCookieRequested As Boolean = False
                Dim useCookiesForce As Boolean = UseCookies.HasValue AndAlso UseCookies.Value AndAlso cookiesExists
                If UseCookies.HasValue AndAlso UseCookies.Value Then
                    withCookieRequested = True
                    result = Parse_Internal(URL, pattern, _CachePathDefault, True, YouTubeCookieNetscapeFile, DateAfter, DateBefore, __GetDefault, __GetShorts)
                End If
                If Not result And Not withCookieRequested Then
                    If Not UseCookies.HasValue OrElse Not UseCookies.Value Then result = Parse_Internal(URL, pattern, _CachePathDefault, False, YouTubeCookieNetscapeFile, DateAfter, DateBefore, __GetDefault, __GetShorts)
                    If Not result And Not UseCookies.HasValue And cookiesExists Then result = Parse_Internal(URL, pattern, _CachePathDefault, True, YouTubeCookieNetscapeFile, DateAfter, DateBefore, __GetDefault, __GetShorts)
                End If

                If result Then
                    container.Parse(Nothing, _CachePathDefault, isMusic, Token, Progress)
                    If Not container.HasError Then container.URL = URL : container.IsShorts = isShorts : Return container
                End If
                container.Dispose()
            End If
            Return Nothing
        End Function
        Private Shared Function Parse_Internal(ByVal URL As String, ByVal OutputPattern As String, ByVal OutputPath As SFile,
                                               ByVal UseCookies As Boolean, ByVal CookiesFile As SFile,
                                               ByVal DateAfter As Date?, ByVal DateBefore As Date?,
                                               ByVal GetDefault As Boolean, ByVal GetShorts As Boolean) As Boolean
            Try
                Dim command$ = "yt-dlp --write-info-json --skip-download"
                command.StringAppend(GetCookiesCommand(UseCookies, CookiesFile), " ")
                If DateAfter.HasValue Then command.StringAppend($"--dateafter {DateAfter.Value:yyyyMMdd}", " ")
                If DateBefore.HasValue Then command.StringAppend($"--datebefore {DateBefore.Value:yyyyMMdd}", " ")
                command.StringAppend("{0}" & $" -o ""{OutputPattern}""", " ")
#If DEBUG Then
                Debug.WriteLine(String.Format(command, URL))
#End If
                Using batch As New BatchExecutor(True)
                    With batch
                        .CommandPermanent = BatchExecutor.GetDirectoryCommand(MyYouTubeSettings.YTDLP.Value)
                        If GetDefault Then .Execute(String.Format(command, URL))
                        If GetShorts Then .Execute(String.Format(command, $"{URL.StringTrimEnd("/")}/shorts"))
                    End With
                End Using
                Return SFile.GetFiles(OutputPath,, IO.SearchOption.AllDirectories, EDP.ReturnValue).Count > 0
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex,
                                               $"[API.YouTube.Base.YouTubeFunctions.Parse_Internal({URL}, {UseCookies})]", False)
            End Try
        End Function
        Friend Shared Function CreateContainer(ByVal f As SFile) As IYouTubeMediaContainer
            Dim c As IYouTubeMediaContainer = Nothing
            If f.Exists(SFO.File, False) Then
                Using x As New XmlFile(f, Protector.Modes.All, False) With {.AllowSameNames = True, .XmlReadOnly = True}
                    x.LoadData()
                    If x.Value(YouTubeMediaContainerBase.Name_SiteKey) = YouTubeSiteKey Then
                        Select Case x.Value(YouTubeMediaContainerBase.Name_ObjectType).FromXML(Of Integer)(YouTubeMediaType.Undefined)
                            Case YouTubeMediaType.Channel : c = New Channel
                            Case YouTubeMediaType.PlayList : c = New PlayList
                            Case YouTubeMediaType.Single
                                If x.Value(YouTubeMediaContainerBase.Name_IsMusic).FromXML(Of Boolean)(False) Then
                                    c = New Track
                                Else
                                    c = New Video
                                End If
                            Case Else : Throw New ArgumentException($"Object type '{x.Value(YouTubeMediaContainerBase.Name_ObjectType)}' is not identified",
                                                                    "ObjectType") With {.HelpLink = NameOf(CreateContainer)}
                        End Select
                    End If
                End Using
                If Not c Is Nothing Then c.Load(f)
            End If
            Return c
        End Function
    End Class
End Namespace