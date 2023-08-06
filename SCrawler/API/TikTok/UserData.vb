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
Imports SCrawler.API.YouTube.Objects
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.Web.Documents.JSON
Namespace API.TikTok
    Friend Class UserData : Inherits UserDataBase
#Region "XML names"
        Private Const Name_RemoveTagsFromTitle As String = "RemoveTagsFromTitle"
        Private Const Name_TitleUseNative As String = "TitleUseNative"
        Private Const Name_TitleAddVideoID As String = "TitleAddVideoID"
        Private Const Name_LastDownloadDate As String = "LastDownloadDate"
#End Region
#Region "Declarations"
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return HOST.Source
            End Get
        End Property
        Private ReadOnly Property RootCacheTikTok As ICacheKeeper
            Get
                With Settings.Cache
                    Dim f As SFile = $"{Settings.Cache.RootDirectory.PathWithSeparator}TikTokCache\"
                    If .ContainsFolder(f) Then
                        Return .GetInstance(f)
                    Else
                        f.Exists(SFO.Path, True)
                        With .NewInstance(Of BatchFileExchanger)(f)
                            .DeleteCacheOnDispose = False
                            .DeleteRootOnDispose = False
                            Return .Self
                        End With
                    End If
                End With
            End Get
        End Property
        Friend Property RemoveTagsFromTitle As Boolean = False
        Friend Property TitleUseNative As Boolean = True
        Friend Property TitleAddVideoID As Boolean = True
        Private Property LastDownloadDate As Date? = Nothing
#End Region
#Region "Exchange"
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return New UserExchangeOptions(Me)
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            If Not Obj Is Nothing AndAlso TypeOf Obj Is UserExchangeOptions Then
                With DirectCast(Obj, UserExchangeOptions)
                    RemoveTagsFromTitle = .RemoveTagsFromTitle
                    TitleUseNative = .TitleUseNative
                    TitleAddVideoID = .TitleAddVideoID
                End With
            End If
        End Sub
#End Region
#Region "Loader"
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            With Container
                If Loading Then
                    RemoveTagsFromTitle = .Value(Name_RemoveTagsFromTitle).FromXML(Of Boolean)(False)
                    TitleUseNative = .Value(Name_TitleUseNative).FromXML(Of Boolean)(True)
                    TitleAddVideoID = .Value(Name_TitleAddVideoID).FromXML(Of Boolean)(True)
                    LastDownloadDate = AConvert(Of Date)(.Value(Name_LastDownloadDate), ADateTime.Formats.BaseDateTime, Nothing)
                Else
                    .Add(Name_RemoveTagsFromTitle, RemoveTagsFromTitle.BoolToInteger)
                    .Add(Name_TitleUseNative, TitleUseNative.BoolToInteger)
                    .Add(Name_TitleAddVideoID, TitleAddVideoID.BoolToInteger)
                    .Add(Name_LastDownloadDate, AConvert(Of String)(LastDownloadDate, AModes.XML, ADateTime.Formats.BaseDateTime, String.Empty))
                End If
            End With
        End Sub
#End Region
#Region "Initializer"
        Friend Sub New()
            SeparateVideoFolder = False
            UseInternalDownloadFileFunction = True
        End Sub
#End Region
#Region "Download functions"
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            Dim URL$ = $"https://www.tiktok.com/@{Name}"
            Using cache As CacheKeeper = CreateCache()
                Try
                    Dim postID$, title$, postUrl$
                    Dim postDate As Date?
                    Dim dateAfterC As Date? = Nothing
                    Dim dateBefore As Date? = DownloadDateTo
                    Dim dateAfter As Date? = DownloadDateFrom

                    If _ContentList.Count > 0 Then
                        With (From d In _ContentList Where d.Post.Date.HasValue Select d.Post.Date.Value)
                            If .ListExists Then dateAfterC = .Min
                        End With
                    End If

                    With {CStr(AConvert(Of String)(dateAfter, SimpleDateConverter, String.Empty)).FromXML(Of Integer)(-1),
                          CStr(AConvert(Of String)(dateAfterC, SimpleDateConverter, String.Empty)).FromXML(Of Integer)(-1)}.ListWithRemove(Function(d) d = -1)
                        If .ListExists Then dateAfter = AConvert(Of Date)(CStr(.Min), SimpleDateConverter, Nothing)
                    End With

                    If LastDownloadDate.HasValue Then
                        If dateAfter.HasValue And Not DownloadDateFrom.HasValue Then
                            If (LastDownloadDate.Value - dateAfter.Value).TotalDays > 1 Then dateAfter = dateAfter.Value.AddDays(1)
                        End If
                    End If

                    Using b As New TokenBatch(Token)
                        b.ChangeDirectory(cache)
                        b.Encoding = BatchExecutor.UnicodeEncoding
                        b.Execute(CreateYTCommand(cache.RootDirectory, URL, False, dateBefore, dateAfter))
                    End Using

                    ThrowAny(Token)

                    Dim files As List(Of SFile) = SFile.GetFiles(cache, "*.json",, EDP.ReturnValue)
                    If files.ListExists Then
                        Dim j As EContainer
                        For Each file As SFile In files
                            j = JsonDocument.Parse(file.GetText, EDP.ReturnValue)
                            If j.ListExists Then
                                If j.Value("_type").StringToLower = "video" Then
                                    postID = j.Value("id")
                                    If Not _TempPostsList.Contains(postID) Then
                                        _TempPostsList.Add(postID)
                                    Else
                                        Exit Sub
                                    End If
                                    title = j.Value("title").StringRemoveWinForbiddenSymbols
                                    If title.IsEmptyString Or Not TitleUseNative Then
                                        title = postID
                                    Else
                                        If RemoveTagsFromTitle Then title = RegexReplace(title, RegexTagsReplacer)
                                        title = title.StringTrim
                                        If title.IsEmptyString Then
                                            title = postID
                                        ElseIf TitleAddVideoID Then
                                            title &= $" ({postID})"
                                        End If
                                    End If
                                    postDate = AConvert(Of Date)(j.Value("timestamp"), UnixDate32Provider, Nothing)
                                    If Not postDate.HasValue Then postDate = AConvert(Of Date)(j.Value("upload_date"), SimpleDateConverter, Nothing)
                                    Select Case CheckDatesLimit(postDate, SimpleDateConverter)
                                        Case DateResult.Skip : Continue For
                                        Case DateResult.Exit : Exit Sub
                                    End Select

                                    postUrl = j.Value("webpage_url")
                                    If postUrl.IsEmptyString Then postUrl = $"https://www.tiktok.com/@{Name}/video/{postID}"
                                    _TempMediaList.Add(New UserMedia(postUrl, UserMedia.Types.Video) With {
                                                       .File = $"{title}.mp4", .Post = New UserPost(postID, postDate)})
                                End If
                                j.Dispose()
                            End If
                        Next
                    End If
                Catch ex As Exception
                    ProcessException(ex, Token, $"data downloading error [{URL}]")
                End Try
            End Using
        End Sub
#End Region
#Region "ReparseMissing"
        Protected Overrides Sub ReparseMissing(ByVal Token As CancellationToken)
            If ContentMissingExists Then
                Dim m As UserMedia
                Dim i%
                Dim rList As New List(Of Integer)
                For i = 0 To _ContentList.Count - 1
                    If _ContentList(i).State = UserMedia.States.Missing Then
                        m = _ContentList(i)
                        m.URL = m.URL_BASE
                        _TempMediaList.Add(m)
                        rList.Add(i)
                    End If
                Next
                If rList.Count > 0 Then
                    For i% = rList.Count - 1 To 0 Step -1 : _ContentList.RemoveAt(rList(i)) : Next
                End If
            End If
        End Sub
#End Region
#Region "YT-DLP Support"
        Private Function CreateYTCommand(ByVal Output As SFile, ByVal URL As String, ByVal IsDownload As Boolean,
                                         Optional ByVal DateBefore As Date? = Nothing, Optional ByVal DateAfter As Date? = Nothing,
                                         Optional ByVal PrintTitle As Boolean = False, Optional ByVal SupportOutput As Boolean = True) As String
            Dim command$ = $"""{Settings.YtdlpFile}"" "
            If Not IsDownload Then command &= "--write-info-json --skip-download "
            If PrintTitle Then
                If Not command.Contains("--skip-download") Then command &= "--skip-download "
                command &= "--print title "
            End If
            If DateBefore.HasValue Then command &= $"--datebefore {DateBefore.Value.AddDays(1).ToStringDate(SimpleDateConverter)} "
            If DateAfter.HasValue Then command &= $"--dateafter {DateAfter.Value.AddDays(-1).ToStringDate(SimpleDateConverter)} "
            If MySettings.CookiesNetscapeFile.Exists Then command &= $"--no-cookies-from-browser --cookies ""{MySettings.CookiesNetscapeFile}"" "
            command &= $"{URL} "
            If SupportOutput Then
                If IsDownload Then
                    command &= $"-o ""{Output}"""
                Else
                    command &= "-o %(id)s"
                End If
            End If
            Return command
        End Function
#End Region
#Region "DownloadContent, DownloadFile"
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
        Protected Overrides Function DownloadFile(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile, ByVal Token As CancellationToken) As SFile
            Using b As New TokenBatch(Token) With {.FileExchanger = RootCacheTikTok}
                b.Encoding = BatchExecutor.UnicodeEncoding
                b.Execute(CreateYTCommand(DestinationFile, URL, True))
            End Using
            Return DestinationFile
        End Function
#End Region
#Region "DownloadSingleObject"
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            Dim f$ = String.Empty
            If CBool(MySettings.TitleUseNativeSTD.Value) Then
                Using b As New BatchExecutor(True) With {
                    .Encoding = BatchExecutor.UnicodeEncoding,
                    .CleanAutomaticallyViaRegEx = True,
                    .CleanAutomaticallyViaRegExRemoveAllCommands = True
                }
                    b.Execute(CreateYTCommand(Nothing, Data.URL, True,,, True, False))
                    b.Clean()
                    With b.OutputData
                        If .Count > 0 Then
                            For Each vData$ In .Self
                                If Not vData.Contains($": {BatchExecutor.UnicodeEncoding}") Then f = vData : Exit For
                            Next
                        End If
                    End With
                End Using
            End If
            Dim m As New UserMedia(Data.URL, UserMedia.Types.Video)
            If Not f.IsEmptyString Then f = TitleHtmlConverter(f)
            If Not f.IsEmptyString Then
                If CBool(MySettings.RemoveTagsFromTitle.Value) Then f = RegexReplace(f, RegexTagsReplacer)
                f = f.StringTrim
                If Not f.IsEmptyString Then
                    If CBool(MySettings.TitleAddVideoID.Value) Then f &= $" ({m.File.Name})"
                    m.File.Name = f
                End If
            End If
            _TempMediaList.Add(m)
        End Sub
#End Region
#Region "Exception"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            Return 0
        End Function
#End Region
    End Class
End Namespace