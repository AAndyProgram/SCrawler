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
                URL = URL.StringTrim
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
        Public Shared Function StandardizeURL_Channel(ByVal URL As String, Optional ByVal Process As Boolean = True) As String
            Try
                URL = URL.StringTrim
                Dim ct As YouTubeChannelTab = YouTubeChannelTab.All
                Dim isMusic As Boolean = False
                If Process AndAlso Info_GetUrlType(URL, isMusic,,,, ct) = YouTubeMediaType.Channel AndAlso Not isMusic Then
                    If Not ct = YouTubeChannelTab.All Then
                        Dim rValue$ = String.Empty
                        Select Case ct
                            Case YouTubeChannelTab.Videos : rValue = "/videos"
                            Case YouTubeChannelTab.Shorts : rValue = "/shorts"
                            Case YouTubeChannelTab.Playlists : rValue = "/playlists"
                        End Select
                        If Not rValue.IsEmptyString Then
                            Dim startIndx% = InStr(URL, rValue)
                            If startIndx > 0 Then URL = URL.Remove(startIndx - 1)
                        End If
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
                                               Optional ByRef IsChannelUser As Boolean = False, Optional ByRef Id As String = Nothing,
                                               Optional ByRef ChannelOptions As YouTubeChannelTab = YouTubeChannelTab.All) As YouTubeMediaType
            URL = URL.StringTrim
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
                            Case UserChannelOption, "@"
                                IsChannelUser = data(2).ToLower = UserChannelOption
                                If data.Count > 6 Then
                                    Select Case data(6).StringToLower.StringTrimStart("/")
                                        Case "videos" : ChannelOptions = YouTubeChannelTab.Videos
                                        Case "shorts" : ChannelOptions = YouTubeChannelTab.Shorts
                                        Case "playlists" : ChannelOptions = YouTubeChannelTab.Playlists
                                        Case Else : ChannelOptions = YouTubeChannelTab.All
                                    End Select
                                End If
                                Return YouTubeMediaType.Channel
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
                                     Optional ByVal DateAfter As Date? = Nothing, Optional ByVal DateBefore As Date? = Nothing,
                                     Optional ByVal ChannelOption As YouTubeChannelTab? = Nothing, Optional ByVal UrlAsIs As Boolean = False) As IYouTubeMediaContainer
            URL = URL.StringTrim
            If URL.IsEmptyString Then Throw New ArgumentNullException("URL", "URL cannot be null")
            If Not MyYouTubeSettings.YTDLP.Value.Exists Then Throw New IO.FileNotFoundException("Path to 'yt-dlp.exe' not set or program not found at destination", MyYouTubeSettings.YTDLP.Value.ToString)
            Dim urlOrig$ = URL
            URL = RegexReplace(URL, TrueUrlRegEx)
            If URL.IsEmptyString Then Throw New ArgumentNullException("URL", $"Can't get true URL from [{urlOrig}]")
            Dim isMusic As Boolean = False, isShorts As Boolean = False
            Dim channelTab As YouTubeChannelTab = YouTubeChannelTab.All
            Dim objType As YouTubeMediaType = Info_GetUrlType(URL, isMusic, isShorts,,, channelTab)
            If ChannelOption.HasValue Then channelTab = ChannelOption.Value
            If Not objType = YouTubeMediaType.Undefined Then
                Dim container As IYouTubeMediaContainer
                Dim pattern$ = "%(channel_id)s_%(id)s_%(playlist_index)s"

                Select Case objType
                    Case YouTubeMediaType.Single
                        If isMusic Then container = New Track Else container = New Video
                    Case YouTubeMediaType.PlayList : container = New PlayList : pattern = "%(playlist_index)s_%(id)s"
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
                    result = Parse_Internal(URL, pattern, _CachePathDefault, True, YouTubeCookieNetscapeFile, DateAfter, DateBefore, objType, channelTab, isMusic, UrlAsIs)
                End If
                If Not result And Not withCookieRequested Then
                    If Not UseCookies.HasValue OrElse Not UseCookies.Value Then result = Parse_Internal(URL, pattern, _CachePathDefault, False, YouTubeCookieNetscapeFile, DateAfter, DateBefore, objType, channelTab, isMusic, UrlAsIs)
                    If Not result And Not UseCookies.HasValue And cookiesExists Then result = Parse_Internal(URL, pattern, _CachePathDefault, True, YouTubeCookieNetscapeFile, DateAfter, DateBefore, objType, channelTab, isMusic, UrlAsIs)
                End If

                If result Then
                    container.Parse(Nothing, _CachePathDefault, isMusic, Token, Progress)
                    If Not container.HasError Then container.URL = URL.ToMusicUrl(isMusic) : container.IsShorts = isShorts : Return container
                End If
                container.Dispose()
            End If
            Return Nothing
        End Function
        Private Shared Function Parse_Internal(ByVal URL As String, ByVal OutputPattern As String, ByVal OutputPath As SFile,
                                               ByVal UseCookies As Boolean, ByVal CookiesFile As SFile,
                                               ByVal DateAfter As Date?, ByVal DateBefore As Date?,
                                               ByVal ObjType As YouTubeMediaType, ByVal ChannelTab As YouTubeChannelTab,
                                               ByVal IsMusic As Boolean, ByVal UrlAsIs As Boolean) As Boolean
            Try
                Dim command$ = $"{YTDLP_NAME} --write-info-json --skip-download"
                command.StringAppend(GetCookiesCommand(UseCookies, CookiesFile), " ")
                If DateAfter.HasValue Then command.StringAppend($"--dateafter {DateAfter.Value:yyyyMMdd}", " ")
                If DateBefore.HasValue Then command.StringAppend($"--datebefore {DateBefore.Value:yyyyMMdd}", " ")
                command.StringAppend("{0}" & $" -o ""{OutputPattern}""", " ")
                '#If DEBUG Then
                'Debug.WriteLine(String.Format(command, URL))
                '#End If
                Dim debugString As Func(Of String, String) = Function(ByVal input As String) As String
#If DEBUG Then
                                                                 Debug.WriteLine(input)
#End If
                                                                 Return input
                                                             End Function
                Using batch As New BatchExecutor(True)
                    With batch
                        .CommandPermanent = BatchExecutor.GetDirectoryCommand(MyYouTubeSettings.YTDLP.Value)
                        If ObjType = YouTubeMediaType.Channel And Not IsMusic And Not UrlAsIs Then
                            Dim ct As List(Of YouTubeChannelTab) = EnumExtract(Of YouTubeChannelTab)(ChannelTab,, True).ListIfNothing
                            If ct.Count = 0 Then
                                .Execute(debugString(String.Format(command, $"{URL.StringTrimEnd("/")}/videos")))
                                .Execute(debugString(String.Format(command, $"{URL.StringTrimEnd("/")}/shorts")))
                                .Execute(debugString(String.Format(command, $"{URL.StringTrimEnd("/")}/playlists")))
                            Else
                                If ct.Contains(YouTubeChannelTab.Videos) Then .Execute(debugString(String.Format(command, $"{URL.StringTrimEnd("/")}/videos")))
                                If ct.Contains(YouTubeChannelTab.Shorts) Then .Execute(debugString(String.Format(command, $"{URL.StringTrimEnd("/")}/shorts")))
                                If ct.Contains(YouTubeChannelTab.Playlists) Then .Execute(debugString(String.Format(command, $"{URL.StringTrimEnd("/")}/playlists")))
                            End If
                        Else
                            .Execute(debugString(String.Format(command, URL)))
                        End If
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