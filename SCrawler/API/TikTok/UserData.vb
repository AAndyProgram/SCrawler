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
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace API.TikTok
    Friend Class UserData : Inherits UserDataBase
#Region "XML names"
        Private Const Name_RemoveTagsFromTitle As String = "RemoveTagsFromTitle"
        Private Const Name_TitleUseNative As String = "TitleUseNative"
        Private Const Name_TitleAddVideoID As String = "TitleAddVideoID"
        Private Const Name_LastDownloadDate As String = "LastDownloadDate"
        Private Const Name_TitleUseRegexForTitle As String = "TitleUseRegexForTitle"
        Private Const Name_TitleUseRegexForTitle_Value As String = "TitleUseRegexForTitle_Value"
        Private Const Name_TitleUseGlobalRegexOptions As String = "TitleUseGlobalRegexOptions"
        Private Const Name_PhotosDownloaded As String = "PhotosDownloaded"
#End Region
#Region "Declarations"
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return HOST.Source
            End Get
        End Property
        Private UserCache As CacheKeeper = Nothing
        Private ReadOnly Property RootCacheTikTok As ICacheKeeper
            Get
                If Not UserCache Is Nothing AndAlso Not UserCache.Disposed Then
                    With DirectCast(UserCache.NewInstance(Of BatchFileExchanger), BatchFileExchanger)
                        .Validate()
                        Return .Self
                    End With
                Else
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
                End If
            End Get
        End Property
        Friend Property RemoveTagsFromTitle As Boolean = False
        Friend Property TitleUseNative As Boolean = True
        Friend Property TitleAddVideoID As Boolean = True
        Friend Property TitleUseRegexForTitle As Boolean = False
        Friend Property TitleUseRegexForTitle_Value As String = String.Empty
        Friend Property TitleUseGlobalRegexOptions As Boolean = True
        Private Property LastDownloadDate As Date? = Nothing
        Private Property PhotosDownloaded As Boolean = False
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
                    TitleUseRegexForTitle = .TitleUseRegexForTitle
                    TitleUseRegexForTitle_Value = .TitleUseRegexForTitle_Value
                    TitleUseGlobalRegexOptions = .TitleUseGlobalRegexOptions
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
                    If Not LastDownloadDate.HasValue Then LastDownloadDate = LastUpdated
                    TitleUseRegexForTitle = .Value(Name_TitleUseRegexForTitle).FromXML(Of Boolean)(False)
                    TitleUseRegexForTitle_Value = .Value(Name_TitleUseRegexForTitle_Value)
                    TitleUseGlobalRegexOptions = .Value(Name_TitleUseGlobalRegexOptions).FromXML(Of Boolean)(True)
                    PhotosDownloaded = .Value(Name_PhotosDownloaded).FromXML(Of Boolean)(False)
                Else
                    .Add(Name_RemoveTagsFromTitle, RemoveTagsFromTitle.BoolToInteger)
                    .Add(Name_TitleUseNative, TitleUseNative.BoolToInteger)
                    .Add(Name_TitleAddVideoID, TitleAddVideoID.BoolToInteger)
                    .Add(Name_LastDownloadDate, AConvert(Of String)(LastDownloadDate, AModes.XML, ADateTime.Formats.BaseDateTime, String.Empty))
                    .Add(Name_TitleUseRegexForTitle, TitleUseRegexForTitle.BoolToInteger)
                    .Add(Name_TitleUseRegexForTitle_Value, TitleUseRegexForTitle_Value)
                    .Add(Name_TitleUseGlobalRegexOptions, TitleUseGlobalRegexOptions.BoolToInteger)
                    .Add(Name_PhotosDownloaded, PhotosDownloaded.BoolToInteger)
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
        Private Function GetTitleRegex() As RParams
            Dim titleRegex As RParams = Nothing
            If TitleUseGlobalRegexOptions Then
                If CBool(MySettings.TitleUseRegexForTitle.Value) AndAlso Not CStr(MySettings.TitleUseRegexForTitle_Value.Value).IsEmptyString Then _
                   titleRegex = RParams.DM(MySettings.TitleUseRegexForTitle_Value.Value, 0, RegexReturn.List, EDP.ReturnValue)
            ElseIf TitleUseRegexForTitle And Not TitleUseRegexForTitle_Value.IsEmptyString Then
                titleRegex = RParams.DM(TitleUseRegexForTitle_Value, 0, RegexReturn.List, EDP.ReturnValue)
            End If
            If Not titleRegex Is Nothing Then
                titleRegex.NothingExists = True
                titleRegex.Nothing = New List(Of String)
                titleRegex.Converter = Function(input) input.StringTrim
            End If
            Return titleRegex
        End Function
        Private Function ChangeTitleRegex(ByVal Title As String, ByVal Regex As RParams) As String
            Try
                If Not Regex Is Nothing Then
                    With DirectCast(RegexReplace(Title, Regex), List(Of String))
                        If .ListExists Then
                            Dim newTitle$ = .ListToString(String.Empty, EDP.ReturnValue).StringTrim
                            If Not newTitle.IsEmptyString Then Return newTitle
                        End If
                    End With
                End If
            Catch ex As Exception
            End Try
            Return Title
        End Function
        Private Function GetNewFileName(ByVal Title As String, ByVal Native As Boolean, ByVal RemoveTags As Boolean, ByVal AddVideoID As Boolean,
                                        ByVal PostID As String, ByVal TitleRegex As RParams) As String
            If Not Title.IsEmptyString Then Title = TitleHtmlConverter(Left(Title, 150)).StringTrim
            If Title.IsEmptyString Or Not Native Then
                Title = PostID
            Else
                If RemoveTags Then Title = RegexReplace(Title, RegexTagsReplacer)
                Title = Title.StringTrim
                If Title.IsEmptyString Then
                    Title = PostID
                ElseIf AddVideoID Then
                    Title &= $" ({PostID})"
                End If
                Title = ChangeTitleRegex(Title, TitleRegex)
            End If
            Return Title
        End Function
        Private Function GetPhotoNode() As Object()
            Return {"imageURL", "urlList", 0, 0}
        End Function
        Friend Overrides Sub DownloadData(ByVal Token As CancellationToken)
            MyBase.DownloadData(Token)
            UserCache.DisposeIfReady(False)
            UserCache = Nothing
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            Dim URL$ = $"https://www.tiktok.com/@{NameTrue}"
            UserCache = CreateCache()
            Try
                Const photoPrefix$ = "photo_"
                Dim postID$, title$, postUrl$, newName$, t$, postID2$, imgUrl$
                Dim postDate As Date?
                Dim dateAfterC As Date? = Nothing
                Dim dateBefore As Date? = DownloadDateTo
                Dim dateAfter As Date? = DownloadDateFrom
                Dim baseDataObtained As Boolean = False
                Dim titleRegex As RParams = GetTitleRegex()
                Dim vPath As SFile = Nothing, pPath As SFile = Nothing
                Dim file As SFile
                Dim j As EContainer, photo As EContainer
                Dim photoNode As Object() = GetPhotoNode()
                Dim c%, cc%, i%
                Dim errDef As New ErrorsDescriber(EDP.ReturnValue)

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
                    If Not DownloadDateTo.HasValue And Not DownloadDateFrom.HasValue Then
                        If LastDownloadDate.Value.AddDays(1) <= Now Then
                            dateAfter = LastDownloadDate.Value
                        Else
                            dateAfter = LastDownloadDate.Value.AddDays(-1)
                        End If
                        dateBefore = Nothing
                    ElseIf dateAfter.HasValue And Not DownloadDateFrom.HasValue Then
                        If (LastDownloadDate.Value - dateAfter.Value).TotalDays > 1 Then dateAfter = dateAfter.Value.AddDays(1)
                    End If
                End If

                If DownloadVideos And Settings.YtdlpFile.Exists And CBool(MySettings.DownloadTTVideos.Value) Then
                    With UserCache.NewInstance : .Validate() : vPath = .RootDirectory : End With
                    Using b As New YTDLP.YTDLPBatch(Token) With {.TempPostsList = _TempPostsList}
                        b.Commands.Clear()
                        b.ChangeDirectory(vPath)
                        b.Encoding = BatchExecutor.UnicodeEncoding
                        b.Execute(CreateYTCommand(vPath, URL, False, dateBefore, dateAfter))
                    End Using
                End If

                If DownloadImages And Settings.GalleryDLFile.Exists And CBool(MySettings.DownloadTTPhotos.Value) Then
                    With UserCache.NewInstance : .Validate() : pPath = .RootDirectory : End With
                    Using b As New GDL.GDLBatch(Token)
                        With b
                            If PhotosDownloaded And _TempPostsList.Count > 0 Then
                                .TempPostsList = (From p As String In _TempPostsList
                                                  Where Not p.IsEmptyString AndAlso p.StartsWith(photoPrefix)
                                                  Select p.Replace(photoPrefix, String.Empty)).ListIfNothing
                            Else
                                .TempPostsList = New List(Of String)
                            End If
                            .ChangeDirectory(pPath)
                            .Encoding = BatchExecutor.UnicodeEncoding
                            .Execute(CreateGDLCommand(URL))
                            If Not PhotosDownloaded Then _ForceSaveUserInfo = True : _ForceSaveUserInfoOnException = True
                            PhotosDownloaded = True
                        End With
                    End Using
                End If

                ThrowAny(Token)

                Dim files As List(Of SFile)
                If Not vPath.IsEmptyString AndAlso vPath.Exists(SFO.Path, False) Then
                    files = SFile.GetFiles(vPath, "*.json",, errDef)
                    If files.ListExists Then
                        For Each file In files
                            j = JsonDocument.Parse(file.GetText, errDef)
                            If j.ListExists Then
                                If j.Value("_type").StringToLower = "video" Then
                                    If Not baseDataObtained Then
                                        baseDataObtained = True
                                        If ID.IsEmptyString Then
                                            ID = j.Value("uploader_id")
                                            If Not ID.IsEmptyString Then _ForceSaveUserInfo = True
                                        End If
                                        newName = j.Value("uploader")
                                        If Not newName.IsEmptyString Then NameTrue = newName
                                        newName = j.Value("creator")
                                        If Not newName.IsEmptyString Then UserSiteName = newName
                                    End If
                                    postID = j.Value("id")
                                    If Not _TempPostsList.Contains(postID) Then
                                        _TempPostsList.ListAddValue(postID, LNC)
                                    Else
                                        Exit For 'Exit Sub
                                    End If
                                    title = GetNewFileName(j.Value("title").StringRemoveWinForbiddenSymbols,
                                                           TitleUseNative, RemoveTagsFromTitle, TitleAddVideoID, postID, titleRegex)
                                    postDate = AConvert(Of Date)(j.Value("timestamp"), UnixDate32Provider, Nothing)
                                    If Not postDate.HasValue Then postDate = AConvert(Of Date)(j.Value("upload_date"), SimpleDateConverter, Nothing)
                                    Select Case CheckDatesLimit(postDate, SimpleDateConverter)
                                        Case DateResult.Skip : Continue For
                                        Case DateResult.Exit : Exit For 'Exit Sub
                                    End Select

                                    postUrl = j.Value("webpage_url")
                                    If postUrl.IsEmptyString Then postUrl = $"https://www.tiktok.com/@{Name}/video/{postID}"
                                    _TempMediaList.Add(New UserMedia(postUrl, UTypes.Video) With {
                                                       .File = $"{title}.mp4", .Post = New UserPost(postID, postDate)})
                                End If
                                j.Dispose()
                            End If
                        Next
                    End If
                End If

                If Not pPath.IsEmptyString AndAlso pPath.Exists(SFO.Path, False) Then
                    files = SFile.GetFiles(pPath, "*.txt",, errDef)
                    If files.ListExists Then
                        For Each file In files
                            t = file.GetText(errDef)
                            If Not t.IsEmptyString Then t = RegexReplace(t, RegexPhotoJson)
                            If Not t.IsEmptyString Then
                                j = JsonDocument.Parse(t, errDef)
                                If j.ListExists Then
                                    With j.ItemF({0, "webapp.video-detail", "itemInfo", "itemStruct"})
                                        If .ListExists Then
                                            postID = .Value("id")
                                            postID2 = $"{photoPrefix}{postID}"
                                            If Not _TempPostsList.Contains(postID2) Then _TempPostsList.ListAddValue(postID2, LNC) Else Exit For 'Exit Sub
                                            postDate = AConvert(Of Date)(j.Value("createTime"), UnixDate32Provider, Nothing)
                                            Select Case CheckDatesLimit(postDate, SimpleDateConverter)
                                                Case DateResult.Skip : Continue For
                                                Case DateResult.Exit : Exit For 'Exit Sub
                                            End Select
                                            title = GetNewFileName(j.Value({"imagePost"}, "title").StringRemoveWinForbiddenSymbols,
                                                                   TitleUseNative, RemoveTagsFromTitle, TitleAddVideoID, postID, titleRegex)
                                            postUrl = $"https://www.tiktok.com/@{Name}/photo/{postID}"
                                            With .Item({"imagePost", "images"})
                                                If .ListExists Then
                                                    i = 0
                                                    c = .Count
                                                    cc = Math.Max(c.ToString.Length, 3)
                                                    For Each photo In .Self
                                                        i += 1
                                                        imgUrl = photo.ItemF(photoNode).XmlIfNothingValue
                                                        If Not imgUrl.IsEmptyString Then _
                                                           _TempMediaList.Add(New UserMedia(imgUrl, UTypes.Picture) With {
                                                                              .URL_BASE = postUrl,
                                                                              .SpecialFolder = "Photo",
                                                                              .File = $"{title}{IIf(c > 1, $"_{i.NumToString(ANumbers.Formats.NumberGroup, cc)}", String.Empty)}.jpg",
                                                                              .Post = New UserPost(postID, postDate)})
                                                    Next
                                                End If
                                            End With
                                        End If
                                    End With
                                    j.Dispose()
                                End If
                            End If
                        Next
                    End If
                End If

                If _TempMediaList.Count > 0 Then LastDownloadDate = Now
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]")
            End Try
        End Sub
#End Region
#Region "ReparseMissing"
        Protected Overrides Sub ReparseMissing(ByVal Token As CancellationToken)
            If ContentMissingExists Then
                Dim m As UserMedia
                Dim d As IYouTubeMediaContainer = Nothing
                Dim i%
                Dim rList As New List(Of Integer)
                Dim picIDs As New List(Of String)
                Dim defDir As SFile = SFile.GetPath(DownloadContentDefault_GetRootDir())
                Dim result As Boolean
                For i = 0 To _ContentList.Count - 1
                    If _ContentList(i).State = UserMedia.States.Missing Then
                        m = _ContentList(i)
                        result = False
                        Try
                            If m.Type = UTypes.Video Then
                                d = MySettings.GetSingleMediaInstance(m.URL_BASE, defDir)
                                result = False
                                If If(UserCache?.Disposed, True) Then UserCache = CreateCache()
                                DownloadSingleObject_GetPosts(d, Token, UserCache, result)
                            ElseIf m.Type = UTypes.Picture Then
                                If picIDs.Contains(m.Post.ID) Then
                                    rList.Add(i)
                                Else
                                    d = MySettings.GetSingleMediaInstance(m.URL_BASE, defDir)
                                    If If(UserCache?.Disposed, True) Then UserCache = CreateCache()
                                    DownloadSingleObject_GetPosts(d, Token, UserCache, result)
                                    picIDs.Add(m.Post.ID)
                                End If
                            End If
                        Catch ex As Exception
                            result = False
                            ProcessException(ex, Token, "ReparseMissing")
                        End Try
                        If result Then rList.Add(i)
                        d.DisposeIfReady(False)
                    End If
                Next
                picIDs.Clear()
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
            If Not CBool(If(IsSingleObjectDownload, MySettings.UseParsedVideoDateSTD, MySettings.UseParsedVideoDate).Value) Then command &= "--no-mtime "
            If MySettings.CookiesNetscapeFile.Exists Then command &= $"--no-cookies-from-browser --cookies ""{MySettings.CookiesNetscapeFile}"" "
            command &= $"{URL} "
            If SupportOutput Then
                If IsDownload Then
                    command &= $"-o ""{Output}"""
                Else
                    command &= "-o %(id)s"
                End If
            End If
            '#If DEBUG Then
            'Debug.WriteLine(command)
            '#End If
            Return command
        End Function
#End Region
#Region "GDL Support"
        Private Function CreateGDLCommand(ByVal URL As String) As String
            Return $"""{Settings.GalleryDLFile}"" --verbose --no-download --no-skip --write-pages {URL}"
        End Function
#End Region
#Region "DownloadContent, DownloadFile"
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
        Protected Overrides Function ValidateDownloadFile(ByVal URL As String, ByVal Media As UserMedia, ByRef Interrupt As Boolean) As Boolean
            Return Not Media.Type = UTypes.Picture
        End Function
        Protected Overrides Function DownloadFile(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile, ByVal Token As CancellationToken) As SFile
            Using b As New TokenBatch(Token) With {.FileExchanger = RootCacheTikTok}
                b.Encoding = BatchExecutor.UnicodeEncoding
                b.Execute(CreateYTCommand(DestinationFile, URL, True))
            End Using
            If DestinationFile.Exists Then Return DestinationFile Else Return Nothing
        End Function
#End Region
#Region "DownloadSingleObject"
        Protected Overloads Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            DownloadSingleObject_GetPosts(Data, Token, Nothing, Nothing)
        End Sub
        Private Overloads Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken,
                                                            ByRef Cache As CacheKeeper, ByRef Result As Boolean)
            Dim f$ = String.Empty
            Dim urlsList As New List(Of String)
            Dim t As UTypes
            Dim defName$ = New SFile(Data.URL).Name
            If Data.URL.ToLower.Contains("/video/") Then
                urlsList.Add(Data.URL)
                t = UTypes.Video
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
            Else
                t = UTypes.Picture
                Data.ContentType = Plugin.UserMediaTypes.Picture
                Data.Title = defName
                Dim dir As SFile
                With If(Cache, Settings.Cache).NewInstance() : .Validate() : dir = .RootDirectory : End With
                Using b As New GDL.GDLBatch(Token)
                    b.ChangeDirectory(dir)
                    b.Encoding = BatchExecutor.UnicodeEncoding
                    b.Execute(CreateGDLCommand(Data.URL))
                End Using
                Dim file As SFile = SFile.GetFiles(dir, "*.txt",, EDP.ReturnValue).FirstOrDefault
                If file.Exists Then
                    Dim r$ = file.GetText(EDP.ReturnValue)
                    If Not r.IsEmptyString Then r = RegexReplace(r, RegexPhotoJson)
                    If Not r.IsEmptyString Then
                        Using j As EContainer = JsonDocument.Parse(r, EDP.ReturnValue)
                            If j.ListExists Then
                                With j.ItemF({0, "webapp.video-detail", "itemInfo", "itemStruct"})
                                    If CBool(MySettings.TitleUseNativeSTD.Value) Then f = j.Value({"imagePost"}, "title").StringRemoveWinForbiddenSymbols
                                    With .Item({"imagePost", "images"})
                                        If .ListExists Then
                                            For Each photo As EContainer In .Self : urlsList.Add(photo.ItemF(GetPhotoNode()).XmlIfNothingValue) : Next
                                        End If
                                    End With
                                End With
                            End If
                        End Using
                    End If
                End If
            End If

            Dim m As UserMedia
            Dim i% = 0, c%, cc%
            Dim ff As Boolean = False
            If urlsList.Count > 0 Then
                c = urlsList.Count
                cc = Math.Max(c.ToString.Length, 3)
                For Each url$ In urlsList
                    i += 1
                    m = New UserMedia(url, t) With {.URL_BASE = Data.URL}
                    If Not f.IsEmptyString Then f = TitleHtmlConverter(f)
                    If Not f.IsEmptyString Or t = UTypes.Picture Then
                        If Not ff Then f = GetNewFileName(f, MySettings.TitleUseNativeSTD.Value, MySettings.RemoveTagsFromTitle.Value, MySettings.TitleAddVideoIDSTD.Value,
                                                          defName, GetTitleRegex)
                        ff = True
                        If Not f.IsEmptyString Then
                            m.File.Name = $"{f.StringTrim}{IIf(c > 1, $"_{i.NumToString(ANumbers.Formats.NumberGroup, cc)}", String.Empty)}"
                            If t = UTypes.Picture Then m.File.Extension = "jpg"
                        End If
                    End If

                    _TempMediaList.Add(m)
                    Result = True
                Next
            End If
        End Sub
        Protected Overrides Sub DownloadSingleObject_PostProcessing(ByVal Data As IYouTubeMediaContainer, Optional ByVal ResetTitle As Boolean = True)
            MyBase.DownloadSingleObject_PostProcessing(Data, Not Data.ContentType = Plugin.UserMediaTypes.Picture)
        End Sub
#End Region
#Region "EraseData"
        Protected Overrides Sub EraseData_AdditionalDataFiles()
            LastDownloadDate = Nothing
        End Sub
#End Region
#Region "Exception"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            Return 0
        End Function
#End Region
#Region "IDisposable Support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then
                UserCache.DisposeIfReady(False)
                UserCache = Nothing
            End If
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace