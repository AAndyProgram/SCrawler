' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Runtime.CompilerServices
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.YouTube
    Public Module YTDeclarations
        Public Const YouTubeSite As String = "YouTube"
        Public Const YouTubeSiteKey As String = "AndyProgram_YouTube"
        Public Const YouTubeSettingsFile As String = "Settings\SettingsYouTube.xml"
        Public Const DownloaderDataFolderYouTube As String = DownloadObjects.STDownloader.DownloaderDataFolder & "YouTube\"
        Friend Const YouTubeDownloadPathDefault As String = "YouTubeDownloads\"
        Friend Const SimpleArraysFormNode As String = "SimpleFormatsChooserForm"
        Private Const YTDLP_DefaultName As String = "yt-dlp"
        Public Property MyYouTubeSettings As Base.YouTubeSettings
        Public Property MyCache As CacheKeeper
        Friend ReadOnly Property MyCacheSettings As New CacheKeeper(DownloaderDataFolderYouTube) With {.DeleteCacheOnDispose = False, .DeleteRootOnDispose = False}
        Public ReadOnly Property YouTubeCookieNetscapeFile As New SFile($"Settings\Responser_{YouTubeSite}_Cookies_Netscape.txt")
        Friend ReadOnly Property YTDLP_NAME As String
            Get
                Dim n$ = MyYouTubeSettings.YTDLP.Value.Name
                If Not n.IsEmptyString Then
                    Return If(n.ToLower = YTDLP_DefaultName, n, $"""{n}""")
                Else
                    Return YTDLP_DefaultName
                End If
            End Get
        End Property
        Friend ReadOnly Property AvailableSubtitlesFormats As String()
            Get
                Return {"ASS", "LRC", "SRT", "VTT"}
            End Get
        End Property
        Friend ReadOnly Property AvailableVideoFormats As String()
            Get
                Return {"AVI", "FLV", "GIF", "MKV", "MOV", "MP4", "WEBM", "AAC", "AIFF", "ALAC", "FLAC", "M4A", "MKA", "MP3", "OGG", "OPUS", "VORBIS", "WAV"}
            End Get
        End Property
        Friend ReadOnly Property AvailableAudioFormats As String()
            Get
                'AC3 not supported
                Return {"AC3", "AAC", "ALAC", "FLAC", "M4A", "MP3", "OPUS", "VORBIS", "WAV"}
            End Get
        End Property
        Friend ReadOnly VideoSizeProvider As New ANumbers(ANumbers.Cultures.USA, ANumbers.Options.DecimalsTrim) With {.DeclaredError = EDP.ReturnValue}
        Friend ReadOnly NumberProvider As New ANumbers(ANumbers.Cultures.Primitive) With {.DeclaredError = EDP.ReturnValue}
        Friend ReadOnly DateBaseProvider As New ADateTime(ADateTime.Formats.BaseDateTime)
        Friend ReadOnly DateAddedProvider As New ADateTime(ADateTime.Formats.yyyymmdd) With {.DateSeparator = String.Empty}
        Friend ReadOnly TimeToStringProvider As IFormatProvider = New TimeToStringConverter
        Friend ReadOnly TitleHtmlConverter As Func(Of String, String) = Function(Input) Input.StringRemoveWinForbiddenSymbols().StringTrim()
        Friend ReadOnly ProgressProvider As IMyProgressNumberProvider = MyProgressNumberProvider.Percentage
        Public ReadOnly TrueUrlRegEx As RParams = RParams.DM(Base.YouTubeFunctions.TrueUrlPattern, 0, EDP.ReturnValue)
        Friend ReadOnly MusicUrlApply As RParams = RParams.DMS("https://([w\.]*)youtube.com.+", 1, RegexReturn.Replace, EDP.ReturnValue,
                                                               CType(Function(input$) "music.", Func(Of String, String)), String.Empty)
        Friend ReadOnly M3U8ExcludedSymbols As String() = {".", ",", ":", "/", "\", "(", ")", "[", "]"}
        <Extension> Friend Function ToMusicUrl(ByVal URL As String, ByVal IsMusic As Boolean) As String
            Try : Return If(IsMusic And Not URL.IsEmptyString, CStr(RegexReplace(URL, MusicUrlApply)).IfNullOrEmpty(URL), URL) : Catch : Return URL : End Try
        End Function
        Friend Function CleanFileName(ByVal f As SFile) As SFile
            If Not f.IsEmptyString And Not f.Name.IsEmptyString Then
                Dim ff As SFile = f
                ff.Name = ff.Name.StringRemoveWinForbiddenSymbols.StringTrim
                ff.Name = ff.Name.StringReplaceSymbols({vbLf, vbCr, vbCrLf}, String.Empty, EDP.ReturnValue)
                ff.Name = ff.Name.StringTrimEnd(".")
                If Not ff.Name.IsEmptyString And Not MyYouTubeSettings.FileRemoveCharacters.IsEmptyString Then _
                   ff.Name = ff.Name.StringReplaceSymbols(MyYouTubeSettings.FileRemoveCharacters.Value.AsList.ListCast(Of String).ToArray, String.Empty, EDP.ReturnValue)
                If ff.Name.IsEmptyString Then ff.Name = "file"
                Return ff
            Else
                Return f
            End If
        End Function
        Private Class TimeToStringConverter : Implements ICustomProvider
            Private ReadOnly _Provider As New ADateTime("mm\:ss") With {.TimeParseMode = ADateTime.TimeModes.TimeSpan}
            Private ReadOnly _ProviderWithHours As New ADateTime("h\:mm\:ss") With {.TimeParseMode = ADateTime.TimeModes.TimeSpan}
            Private ReadOnly Property Provider(ByVal t As TimeSpan) As IFormatProvider
                Get
                    Return If(t.Hours > 0, _ProviderWithHours, _Provider)
                End Get
            End Property
            Private Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                     Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object Implements ICustomProvider.Convert
                If Not IsNothing(Value) Then
                    If TypeOf Value Is Nullable(Of TimeSpan) Then
                        With DirectCast(Value, Nullable(Of TimeSpan))
                            If .HasValue Then Return AConvert(Of String)(.Value, Me.Provider(.Value), String.Empty)
                        End With
                    ElseIf TypeOf Value Is TimeSpan Then
                        Dim t As TimeSpan = Value
                        Return AConvert(Of String)(t, Me.Provider(t), String.Empty)
                    End If
                End If
                Return String.Empty
            End Function
            Private Function GetFormat(ByVal FormatType As Type) As Object Implements IFormatProvider.GetFormat
                Throw New NotImplementedException("'GetFormat' is not available in the 'TimeToStringConverter' context")
            End Function
        End Class
        Friend Class DurationXmlConverter : Implements ICustomProvider
            Private Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                     Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object Implements ICustomProvider.Convert
                Try
                    If DestinationType Is GetType(String) Then
                        If IsNothing(Value) Then
                            Return 0
                        ElseIf TypeOf Value Is TimeSpan Then
                            Return DirectCast(Value, TimeSpan).TotalSeconds
                        Else
                            Throw New Exception
                        End If
                    ElseIf DestinationType Is GetType(TimeSpan) Then
                        If IsNothing(Value) Then
                            Return New TimeSpan
                        ElseIf TypeOf Value Is String Then
                            If CStr(Value).IsEmptyString Then
                                Return New TimeSpan
                            Else
                                Return TimeSpan.FromSeconds(AConvert(Of Double)(Value, EDP.ThrowException))
                            End If
                        ElseIf TypeOf Value Is Double Or IsNumeric(Value) Then
                            Return TimeSpan.FromSeconds(Value)
                        Else
                            Throw New Exception
                        End If
                    Else
                        Throw New Exception
                    End If
                Catch ex As Exception
                    Throw New Exception($"Cannot convert {Value.GetType.Name} to {DestinationType.Name}", ex)
                End Try
            End Function
            Private Function GetFormat(ByVal FormatType As Type) As Object Implements IFormatProvider.GetFormat
                Throw New NotImplementedException("'GetFormat' is not available in the 'DurationXmlConverter' context")
            End Function
        End Class
        Friend Sub CheckVersion(ByVal Force As Boolean)
            If Not MyYouTubeSettings Is Nothing Then
                With MyYouTubeSettings
                    If .CheckUpdatesAtStart Or Force Then
                        ShowProgramInfo(.ProgramText.Value.IfNullOrEmpty("YouTube Downloader"),
                                        SCrawler.Shared.GetCurrentMaxVer(Application.StartupPath.CSFileP).IfNullOrEmpty(My.Application.Info.Version),
                                        True, Force, .Self, True,, False, .ProgramDescription)
                    End If
                End With
            End If
        End Sub
    End Module
End Namespace