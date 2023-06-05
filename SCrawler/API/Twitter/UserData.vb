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
Imports UStates = SCrawler.API.Base.UserMedia.States
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace API.Twitter
    Friend Class UserData : Inherits UserDataBase
#Region "XML names"
        Private Const Name_GifsDownload As String = "GifsDownload"
        Private Const Name_GifsSpecialFolder As String = "GifsSpecialFolder"
        Private Const Name_GifsPrefix As String = "GifsPrefix"
#End Region
#Region "Declarations"
        Friend Property GifsDownload As Boolean = True
        Friend Property GifsSpecialFolder As String = String.Empty
        Friend Property GifsPrefix As String = String.Empty
        Private ReadOnly _DataNames As List(Of String)
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return HOST.Source
            End Get
        End Property
        Private FileNameProvider As ANumbers = Nothing
        Private Sub ResetFileNameProvider(Optional ByVal GroupSize As Integer? = Nothing)
            FileNameProvider = New ANumbers With {.FormatOptions = ANumbers.Options.FormatNumberGroup + ANumbers.Options.Groups}
            FileNameProvider.GroupSize = If(GroupSize, 3)
        End Sub
        Private Function RenameGdlFile(ByVal Input As SFile, ByVal i As Integer) As SFile
            Return SFile.Rename(Input, $"{Input.PathWithSeparator}{i.NumToString(FileNameProvider)}.{Input.Extension}",, EDP.ThrowException)
        End Function
#End Region
#Region "Exchange options"
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return New EditorExchangeOptions(Me) With {.SiteKey = HOST.Key}
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            If Not Obj Is Nothing AndAlso TypeOf Obj Is EditorExchangeOptions Then
                With DirectCast(Obj, EditorExchangeOptions)
                    GifsDownload = .GifsDownload
                    GifsSpecialFolder = .GifsSpecialFolder
                    GifsPrefix = .GifsPrefix
                    UseMD5Comparison = .UseMD5Comparison
                    RemoveExistingDuplicates = .RemoveExistingDuplicates
                End With
            End If
        End Sub
#End Region
#Region "Initializer, loader"
        Friend Sub New()
            _DataNames = New List(Of String)
        End Sub
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            If Loading Then
                GifsDownload = Container.Value(Name_GifsDownload).FromXML(Of Boolean)(True)
                GifsSpecialFolder = Container.Value(Name_GifsSpecialFolder)
                If Not Container.Contains(Name_GifsPrefix) Then
                    GifsPrefix = "GIF_"
                Else
                    GifsPrefix = Container.Value(Name_GifsPrefix)
                End If
                UseMD5Comparison = Container.Value(Name_UseMD5Comparison).FromXML(Of Boolean)(False)
                RemoveExistingDuplicates = Container.Value(Name_RemoveExistingDuplicates).FromXML(Of Boolean)(False)
                StartMD5Checked = Container.Value(Name_StartMD5Checked).FromXML(Of Boolean)(False)
            Else
                Container.Add(Name_GifsDownload, GifsDownload.BoolToInteger)
                Container.Add(Name_GifsSpecialFolder, GifsSpecialFolder)
                Container.Add(Name_GifsPrefix, GifsPrefix)
                Container.Add(Name_UseMD5Comparison, UseMD5Comparison.BoolToInteger)
                Container.Add(Name_RemoveExistingDuplicates, RemoveExistingDuplicates.BoolToInteger)
                Container.Add(Name_StartMD5Checked, StartMD5Checked.BoolToInteger)
            End If
        End Sub
#End Region
#Region "Download functions"
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            If IsSavedPosts Then
                If _ContentList.Count > 0 Then _DataNames.ListAddList(_ContentList.Select(Function(c) c.Post.ID), LAP.ClearBeforeAdd, LAP.NotContainsOnly)
                DownloadData_SavedPosts(Token)
            Else
                If _ContentList.Count > 0 Then _DataNames.ListAddList(_ContentList.Select(Function(c) c.File.File), LAP.ClearBeforeAdd, LAP.NotContainsOnly)
                DownloadData_Timeline(Token)
            End If
        End Sub
        Private Sub DownloadData_Timeline(ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Dim tCache As CacheKeeper = Nothing
            Try
                Dim PostID$ = String.Empty
                Dim PostDate$, tmpUserId$
                Dim j As EContainer
                Dim nn As EContainer
                Dim NewPostDetected As Boolean = False
                Dim ExistsDetected As Boolean = False

                tCache = New CacheKeeper($"{DownloadContentDefault_GetRootDir()}\_tCache\")
                If tCache.RootDirectory.Exists(SFO.Path, False) Then tCache.RootDirectory.Delete(SFO.Path, SFODelete.DeletePermanently, EDP.ReturnValue)
                tCache.Validate()
                Dim f As SFile = GetTimelineFromGalleryDL(tCache.RootDirectory, Token)
                If Not f.IsEmptyString Then
                    ThrowAny(Token)
                    Dim timelineFiles As List(Of SFile) = SFile.GetFiles(f, "*.txt",, EDP.ReturnValue)
                    If timelineFiles.ListExists Then
                        Dim i%
                        ResetFileNameProvider(Math.Max(timelineFiles.Count.ToString.Length, 2))
                        'rename files
                        For i = 0 To timelineFiles.Count - 1 : timelineFiles(i) = RenameGdlFile(timelineFiles(i), i) : Next
                        'parse files
                        For i = 0 To timelineFiles.Count - 1
                            j = JsonDocument.Parse(timelineFiles(i).GetText)
                            If Not j Is Nothing Then
                                If i = 0 Then
                                    Dim resValue$ = j.Value({"data", "user", "result"}, "__typename").StringTrim.StringToLower
                                    If resValue.IsEmptyString Then
                                        UserExists = False
                                        j.Dispose()
                                        Exit Sub
                                    ElseIf resValue = "userunavailable" Then
                                        UserSuspended = True
                                        j.Dispose()
                                        Exit Sub
                                    Else
                                        With j({"data", "user", "result"})
                                            If .ListExists Then
                                                If ID.IsEmptyString Then
                                                    ID = .Value("rest_id")
                                                    If Not ID.IsEmptyString Then _ForceSaveUserInfo = True
                                                End If
                                                With .Item({"legacy"})
                                                    If .ListExists Then
                                                        If .Value("screen_name").StringToLower = Name.ToLower Then
                                                            UserSiteNameUpdate(.Value("name"))
                                                            UserDescriptionUpdate(.Value("description"))
                                                            Dim __getImage As Action(Of String) = Sub(ByVal img As String)
                                                                                                      If Not img.IsEmptyString Then
                                                                                                          Dim __imgFile As SFile = UrlFile(img, True)
                                                                                                          If Not __imgFile.Name.IsEmptyString Then
                                                                                                              If __imgFile.Extension.IsEmptyString Then __imgFile.Extension = "jpg"
                                                                                                              __imgFile.Path = MyFile.CutPath.Path
                                                                                                              If Not __imgFile.Exists Then GetWebFile(img, __imgFile, EDP.None)
                                                                                                              If __imgFile.Exists Then IconBannerDownloaded = True
                                                                                                          End If
                                                                                                      End If
                                                                                                  End Sub
                                                            Dim icon$ = .Value("profile_image_url_https")
                                                            If Not icon.IsEmptyString Then icon = icon.Replace("_normal", String.Empty)
                                                            If DownloadIconBanner Then
                                                                __getImage.Invoke(.Value("profile_banner_url"))
                                                                __getImage.Invoke(icon)
                                                            End If
                                                        End If
                                                    End If
                                                End With
                                            End If
                                        End With
                                    End If
                                Else
                                    With j({"globalObjects", "tweets"})
                                        If .ListExists Then
                                            ProgressPre.ChangeMax(.Count)
                                            For Each nn In .Self
                                                ProgressPre.Perform()
                                                If nn.Count > 0 Then
                                                    PostID = nn.Value("id")

                                                    'Date Pattern:
                                                    'Sat Jan 01 01:10:15 +0000 2000
                                                    If nn.Contains("created_at") Then PostDate = nn("created_at").Value Else PostDate = String.Empty
                                                    Select Case CheckDatesLimit(PostDate, Declarations.DateProvider)
                                                        Case DateResult.Skip : Continue For
                                                        Case DateResult.Exit : Exit Sub
                                                    End Select

                                                    If Not _TempPostsList.Contains(PostID) Then
                                                        NewPostDetected = True
                                                        _TempPostsList.Add(PostID)
                                                    Else
                                                        ExistsDetected = True
                                                        Continue For
                                                    End If

                                                    tmpUserId = nn.ItemF({"extended_entities", "media", 0, "source_user_id"}).
                                                                XmlIfNothingValue.IfNullOrEmpty(nn.Value("user_id")).IfNullOrEmpty("/")

                                                    If Not ParseUserMediaOnly OrElse (Not ID.IsEmptyString AndAlso tmpUserId = ID) Then _
                                                       ObtainMedia(nn, PostID, PostDate)
                                                End If
                                            Next
                                        End If
                                    End With
                                End If
                                j.Dispose()
                            End If
                        Next
                    End If
                End If
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]")
            Finally
                If Not tCache Is Nothing Then tCache.Dispose()
            End Try
        End Sub
        Private Sub DownloadData_SavedPosts(ByVal Token As CancellationToken)
            Try
                Dim f As SFile = GetDataFromGalleryDL("https://twitter.com/i/bookmarks", Settings.Cache, True, Token)
                Dim files As List(Of SFile) = SFile.GetFiles(f, "*.txt")
                If files.ListExists Then
                    ResetFileNameProvider(Math.Max(files.Count.ToString.Length, 3))
                    Dim id$
                    Dim j As EContainer, jj As EContainer
                    Dim jErr As New ErrorsDescriber(EDP.ReturnValue)
                    For i% = 0 To files.Count - 1
                        f = RenameGdlFile(files(i), i)
                        j = JsonDocument.Parse(f.GetText, jErr)
                        If Not j Is Nothing Then
                            With j.ItemF({"data", 0, "timeline", "instructions", 0, "entries"})
                                If .ListExists Then
                                    ProgressPre.ChangeMax(.Count)
                                    For Each jj In .Self
                                        ProgressPre.Perform()
                                        With jj({"content", "itemContent", "tweet_results", "result", "legacy"})
                                            If .ListExists Then
                                                id = .Value("id_str")
                                                If _TempPostsList.Contains(id) Then j.Dispose() : Exit Sub Else ObtainMedia(.Self, id, .Value("created_at"))
                                            End If
                                        End With
                                    Next
                                End If
                            End With
                            j.Dispose()
                        End If
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex, Token, "data downloading error (Saved Posts)")
            End Try
        End Sub
#End Region
#Region "Obtain media"
        Private Sub ObtainMedia(ByVal e As EContainer, ByVal PostID As String, ByVal PostDate As String, Optional ByVal State As UStates = UStates.Unknown)
            Dim s As EContainer = e.ItemF({"extended_entities", "media"})
            If s Is Nothing OrElse s.Count = 0 Then s = e.ItemF({"retweeted_status", "extended_entities", "media"})
            If If(s?.Count, 0) > 0 Then
                Dim mUrl$
                For Each m As EContainer In s
                    If Not CheckVideoNode(m, PostID, PostDate, State) Then
                        mUrl = m.Value("media_url").IfNullOrEmpty(m.Value("media_url_https"))
                        If Not mUrl.IsEmptyString Then
                            Dim dName$ = UrlFile(mUrl)
                            If Not dName.IsEmptyString AndAlso Not _DataNames.Contains(dName) Then
                                _DataNames.Add(dName)
                                _TempMediaList.ListAddValue(MediaFromData(mUrl, PostID, PostDate, GetPictureOption(m), State, UTypes.Picture), LNC)
                            End If
                        End If
                    End If
                Next
            End If
        End Sub
        Private Function CheckVideoNode(ByVal w As EContainer, ByVal PostID As String, ByVal PostDate As String,
                                        Optional ByVal State As UStates = UStates.Unknown) As Boolean
            Try
                If CheckForGif(w, PostID, PostDate, State) Then Return True
                Dim URL$ = GetVideoNodeURL(w)
                If Not URL.IsEmptyString Then
                    Dim f$ = UrlFile(URL)
                    If Not f.IsEmptyString AndAlso Not _DataNames.Contains(f) Then
                        _DataNames.Add(f)
                        _TempMediaList.ListAddValue(MediaFromData(URL, PostID, PostDate,, State, UTypes.Video), LNC)
                    End If
                    Return True
                End If
                Return False
            Catch ex As Exception
                LogError(ex, "[API.Twitter.UserData.CheckVideoNode]")
                Return False
            End Try
        End Function
        Private Function CheckForGif(ByVal w As EContainer, ByVal PostID As String, ByVal PostDate As String,
                                     Optional ByVal State As UStates = UStates.Unknown) As Boolean
            Try
                Dim gifUrl As Predicate(Of EContainer) = Function(e) Not e.Value("content_type").IsEmptyString AndAlso
                                                                     e.Value("content_type").Contains("mp4") AndAlso
                                                                     Not e.Value("url").IsEmptyString
                Dim url$, ff$
                Dim f As SFile
                Dim m As UserMedia
                If w.ListExists Then
                    For Each n As EContainer In w
                        If n.Value("type") = "animated_gif" Then
                            With n({"video_info", "variants"})
                                If .ListExists Then
                                    With .ItemF({gifUrl})
                                        If .ListExists Then
                                            url = .Value("url")
                                            ff = UrlFile(url)
                                            If Not ff.IsEmptyString Then
                                                If GifsDownload And Not _DataNames.Contains(ff) Then
                                                    m = MediaFromData(url, PostID, PostDate,, State, UTypes.Video)
                                                    f = m.File
                                                    If Not f.IsEmptyString And Not GifsPrefix.IsEmptyString Then f.Name = $"{GifsPrefix}{f.Name}" : m.File = f
                                                    If Not GifsSpecialFolder.IsEmptyString Then m.SpecialFolder = $"{GifsSpecialFolder}*"
                                                    _TempMediaList.ListAddValue(m, LNC)
                                                End If
                                                Return True
                                            End If
                                        End If
                                    End With
                                End If
                            End With
                        End If
                    Next
                End If
                Return False
            Catch ex As Exception
                LogError(ex, "[API.Twitter.UserData.CheckForGif]")
                Return False
            End Try
        End Function
        Private Function GetVideoNodeURL(ByVal w As EContainer) As String
            With w({"video_info", "variants"})
                If .ListExists Then
                    Dim l As New List(Of Sizes)
                    Dim u$
                    For Each n As EContainer In .Self
                        If n.Count > 0 Then
                            If n("content_type").XmlIfNothingValue("none").Contains("mp4") AndAlso n.Contains("url") Then
                                u = n.Value("url")
                                l.Add(New Sizes(RegexReplace(u, VideoSizeRegEx), u))
                            End If
                        End If
                    Next
                    If l.Count > 0 Then l.RemoveAll(Function(s) s.HasError)
                    If l.Count > 0 Then l.Sort() : Return l(0).Data
                End If
            End With
            Return String.Empty
        End Function
#End Region
#Region "Gallery-DL Support"
        Private Class TwitterGDL : Inherits GDL.GDLBatch
            Private Property Token As CancellationToken
            Friend Sub New(ByVal Dir As SFile, ByVal _Token As CancellationToken)
                MyBase.New
                Commands.Clear()
                ChangeDirectory(Dir)
                Token = _Token
            End Sub
            Protected Overrides Async Function Validate(ByVal Value As String) As Task
                If Not ProcessKilled AndAlso Await Task.Run(Function() Token.IsCancellationRequested OrElse IdExists(Value)) Then Kill()
            End Function
            Private Function IdExists(ByVal Value As String) As Boolean
                Try
                    Value = Value.StringTrim
                    If Not Value.IsEmptyString AndAlso (Value.StartsWith("*") Or Value.StartsWith(".\gallery-dl\")) Then
                        Dim id$ = Value.Split("\").Last.Split(".").First.Split("_").First
                        If Not id.IsEmptyString Then Return TempPostsList.Contains(id)
                    End If
                Catch ex As Exception
                End Try
                Return False
            End Function
        End Class
        Private Function GetDataFromGalleryDL(ByVal URL As String, ByVal Cache As CacheKeeper, ByVal UseTempPostList As Boolean,
                                              Optional ByVal Token As CancellationToken = Nothing) As SFile
            Dim command$ = $"""{Settings.GalleryDLFile}"" --verbose --no-download --no-skip --cookies ""{MySettings.CookiesNetscapeFile}"" --write-pages "
            Try
                Dim dir As SFile = Cache.NewPath
                If dir.Exists(SFO.Path,, EDP.ThrowException) Then
                    Using batch As New TwitterGDL(dir, Token)
                        If UseTempPostList Then
                            batch.TempPostsList = _TempPostsList
                            command &= GdlGetIdFilterString()
                        End If
                        command &= URL
                        '#If DEBUG Then
                        '                        Debug.WriteLine(command)
                        '#End If
                        batch.Execute(command)
                    End Using
                    Return dir
                End If
                Return Nothing
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, $"{ToStringForLog()}: GetDataFromGalleryDL({command})")
            End Try
        End Function
        Private Function GetTimelineFromGalleryDL(ByVal Cache As CacheKeeper, ByVal Token As CancellationToken) As SFile
            Dim command$ = String.Empty
            Try
                Dim conf As SFile = $"{Cache.NewPath.PathWithSeparator}TwitterGdlConfig.conf"
                Dim confText$ = "{""extractor"":{""cookies"": """ & MySettings.CookiesNetscapeFile.ToString.Replace("\", "/") &
                                """,""cookies-update"": false,""twitter"":{""cards"": false,""conversations"": false,""pinned"": false,""quoted"": false,""replies"": true,""retweets"": true,""strategy"": null,""text-tweets"": false,""twitpic"": false,""unique"": true,""users"": ""timeline"",""videos"": true}}}"
                If conf.Exists(SFO.Path, True, EDP.ThrowException) Then TextSaver.SaveTextToFile(confText, conf)
                If Not conf.Exists Then Throw New IO.FileNotFoundException("Can't find Twitter GDL config file", conf)

                command = $"""{Settings.GalleryDLFile}"" --verbose --no-download --no-skip --config ""{conf}"" --write-pages "
                command &= GdlGetIdFilterString()
                command &= $"https://twitter.com/search?q=from:{Name}+include:nativeretweets"
                Dim dir As SFile = Cache.NewPath
                dir.Exists(SFO.Path, True, EDP.ThrowException)
                '#If DEBUG Then
                '                Debug.WriteLine(command)
                '#End If
                Using tgdl As New TwitterGDL(dir, Token) With {.TempPostsList = _TempPostsList} : tgdl.Execute(command) : End Using
                Return dir
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, $"{ToStringForLog()}: GetTimelineFromGalleryDL({command})")
            End Try
        End Function
        Private Function GdlGetIdFilterString() As String
            Return If(_TempPostsList.Count > 0, $"--filter ""int(tweet_id) > {_TempPostsList.Last} or abort()"" ", String.Empty)
        End Function
#End Region
#Region "ReparseMissing"
        Protected Overrides Sub ReparseMissing(ByVal Token As CancellationToken)
            Const SinglePostPattern$ = "https://twitter.com/{0}/status/{1}"
            Dim rList As New List(Of Integer)
            Dim URL$ = String.Empty
            Dim cache As CacheKeeper = Nothing
            Try
                If ContentMissingExists Then
                    Dim m As UserMedia
                    Dim PostDate$
                    Dim j As EContainer
                    Dim f As SFile
                    Dim i%, ii%
                    Dim files As List(Of SFile)
                    ResetFileNameProvider()
                    If IsSingleObjectDownload Then
                        cache = Settings.Cache
                    Else
                        cache = New CacheKeeper(DownloadContentDefault_GetRootDir.CSFilePS)
                    End If
                    ProgressPre.ChangeMax(_ContentList.Count)
                    For i = 0 To _ContentList.Count - 1
                        ProgressPre.Perform()
                        If _ContentList(i).State = UStates.Missing Then
                            m = _ContentList(i)
                            If Not m.Post.ID.IsEmptyString Or (IsSingleObjectDownload And Not m.URL_BASE.IsEmptyString) Then
                                ThrowAny(Token)
                                If IsSingleObjectDownload Then
                                    URL = m.URL_BASE
                                Else
                                    URL = String.Format(SinglePostPattern, Name, m.Post.ID)
                                End If
                                f = GetDataFromGalleryDL(URL, cache, Favorite, Token)
                                If Not f.IsEmptyString Then
                                    files = SFile.GetFiles(f, "*.txt")
                                    If files.ListExists Then
                                        For ii = 0 To files.Count - 1
                                            f = RenameGdlFile(files(ii), ii)
                                            j = JsonDocument.Parse(f.GetText)
                                            If Not j Is Nothing Then
                                                With j.ItemF({"data", 0, "instructions", 0, "entries", 0,
                                                             "content", "itemContent", "tweet_results", "result", "legacy"})
                                                    If .ListExists Then
                                                        PostDate = String.Empty
                                                        If .Contains("created_at") Then PostDate = .Value("created_at") Else PostDate = String.Empty
                                                        ObtainMedia(.Self, m.Post.ID, PostDate, UStates.Missing)
                                                        rList.Add(i)
                                                    End If
                                                End With
                                                j.Dispose()
                                            End If
                                        Next
                                        files.Clear()
                                    End If
                                End If
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex, Token, $"ReparseMissing error [{URL}]")
            Finally
                If Not cache Is Nothing And Not IsSingleObjectDownload Then cache.Dispose()
                If rList.Count > 0 Then
                    For i% = rList.Count - 1 To 0 Step -1 : _ContentList.RemoveAt(i) : Next
                    rList.Clear()
                End If
            End Try
        End Sub
#End Region
#Region "DownloadSingleObject"
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            _ContentList.Add(New UserMedia(Data.URL) With {.State = UStates.Missing})
            ReparseMissing(Token)
        End Sub
#End Region
#Region "Picture options"
        Private Function GetPictureOption(ByVal w As EContainer) As String
            Const P4K As String = "4096x4096"
            Try
                Dim ww As EContainer = w("sizes")
                If ww.ListExists Then
                    Dim l As New List(Of Sizes)
                    Dim Orig As Sizes? = New Sizes(w.Value({"original_info"}, "height").FromXML(Of Integer)(-1), P4K)
                    If Orig.Value.Value = -1 Then Orig = Nothing
                    Dim LargeContained As Boolean = ww.Contains("large")
                    For Each v As EContainer In ww
                        If v.Count > 0 AndAlso v.Contains("h") Then l.Add(New Sizes(v.Value("h"), v.Name))
                    Next
                    If l.Count > 0 Then
                        l.Sort()
                        If Orig.HasValue AndAlso l(0).Value < Orig.Value.Value Then
                            Return P4K
                        ElseIf l(0).Data.IsEmptyString Then
                            Return P4K
                        Else
                            Return l(0).Data
                        End If
                    Else
                        Return P4K
                    End If
                ElseIf Not w.Value({"original_info"}, "height").IsEmptyString Then
                    Return P4K
                Else
                    Return String.Empty
                End If
            Catch ex As Exception
                LogError(ex, "[API.Twitter.UserData.GetPictureOption]")
                Return String.Empty
            End Try
        End Function
#End Region
#Region "UrlFile"
        Private Function UrlFile(ByVal URL As String, Optional ByVal GetWithoutExtension As Boolean = False) As String
            Try
                If Not URL.IsEmptyString Then
                    Dim f As SFile = CStr(RegexReplace(LinkFormatterSecure(RegexReplace(URL.Replace("\", String.Empty), LinkPattern)), FilesPattern))
                    If f.IsEmptyString And GetWithoutExtension Then
                        URL = LinkFormatterSecure(RegexReplace(URL.Replace("\", String.Empty), LinkPattern))
                        If Not URL.IsEmptyString Then f = New SFile With {.Name = URL.Split("/").LastOrDefault}
                    End If
                    If Not f.IsEmptyString Then Return f.File
                End If
                Return String.Empty
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function
#End Region
#Region "Create media"
        Private Function MediaFromData(ByVal _URL As String, ByVal PostID As String, ByVal PostDate As String,
                                       Optional ByVal _PictureOption As String = Nothing,
                                       Optional ByVal State As UStates = UStates.Unknown,
                                       Optional ByVal Type As UTypes = UTypes.Undefined) As UserMedia
            _URL = LinkFormatterSecure(RegexReplace(_URL.Replace("\", String.Empty), LinkPattern))
            Dim m As New UserMedia(_URL) With {.PictureOption = _PictureOption, .Post = New UserPost With {.ID = PostID}, .Type = Type}
            If Not m.URL.IsEmptyString Then m.File = CStr(RegexReplace(m.URL, FilesPattern))
            If Not m.PictureOption.IsEmptyString And Not m.File.IsEmptyString And Not m.URL.IsEmptyString Then
                m.URL = $"{m.URL.Replace($".{m.File.Extension}", String.Empty)}?format={m.File.Extension}&name={m.PictureOption}"
            End If
            If Not PostDate.IsEmptyString Then m.Post.Date = AConvert(Of Date)(PostDate, Declarations.DateProvider, Nothing) Else m.Post.Date = Nothing
            m.State = State
            Return m
        End Function
#End Region
#Region "Downloader"
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
#End Region
#Region "Exception"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            Return 0
        End Function
#End Region
#Region "IDisposable support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then _DataNames.Clear()
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace