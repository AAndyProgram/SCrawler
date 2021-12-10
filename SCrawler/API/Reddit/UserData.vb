Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Tools.ImageRenderer
Imports PersonalUtilities.Tools.WebDocuments.JSON
Imports System.Net
Imports System.Threading
Imports SCrawler.API.Base
Imports UStates = SCrawler.API.Base.UserMedia.States
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace API.Reddit
    Friend Class UserData : Inherits UserDataBase : Implements IChannelData
        Friend Overrides Property Site As Sites = Sites.Reddit
#Region "Channels Support"
#Region "IChannelLimits Support"
        Friend Property DownloadLimitCount As Integer? Implements IChannelLimits.DownloadLimitCount
        Friend Property DownloadLimitPost As String Implements IChannelLimits.DownloadLimitPost
        Friend Property DownloadLimitDate As Date? Implements IChannelLimits.DownloadLimitDate
        Friend Overloads Sub SetLimit(Optional ByVal MaxPost As String = "", Optional ByVal MaxCount As Integer? = Nothing,
                                      Optional ByVal MinDate As Date? = Nothing) Implements IChannelLimits.SetLimit
            DownloadLimitPost = MaxPost
            DownloadLimitCount = MaxCount
            DownloadLimitDate = MinDate
        End Sub
        Friend Overloads Sub SetLimit(ByVal Source As IChannelLimits) Implements IChannelLimits.SetLimit
            With Source
                DownloadLimitCount = .DownloadLimitCount
                DownloadLimitPost = .DownloadLimitPost
                DownloadLimitDate = .DownloadLimitDate
                AutoGetLimits = .AutoGetLimits
            End With
        End Sub
        Friend Property AutoGetLimits As Boolean = True Implements IChannelLimits.AutoGetLimits
#End Region
        Friend Property ChannelInfo As Channel
        Private ReadOnly ChannelPostsNames As New List(Of String)
        Friend Property SkipExistsUsers As Boolean = True Implements IChannelData.SkipExistsUsers
        Private ReadOnly _ExistsUsersNames As List(Of String)
        Friend Property SaveToCache As Boolean = False Implements IChannelData.SaveToCache
        Friend Function GetNewChannelPosts() As IEnumerable(Of UserPost)
            If _ContentNew.Count > 0 Then Return (From c As UserMedia In _ContentNew
                                                  Where Not c.Post.CachedFile.IsEmptyString And c.State = UStates.Downloaded
                                                  Select c.Post) Else Return Nothing
        End Function
#End Region
        Private _Progress As MyProgress
        Friend Property Progress As MyProgress
            Get
                If _Progress Is Nothing Then Return MainProgress Else Return _Progress
            End Get
            Set(ByVal p As MyProgress)
                _Progress = p
            End Set
        End Property
#Region "Initializers"
        ''' <summary>Video downloader initializer</summary>
        Private Sub New()
        End Sub
        ''' <summary>Default initializer</summary>
        Friend Sub New(ByVal u As UserInfo, Optional ByVal _LoadUserInformation As Boolean = True, Optional ByVal InvokeImageHandler As Boolean = True)
            MyBase.New(InvokeImageHandler)
            ChannelPostsNames = New List(Of String)
            _ExistsUsersNames = New List(Of String)
            User = u
            If _LoadUserInformation Then LoadUserInformation()
        End Sub
#End Region
#Region "Download Overrides"
        Friend Overrides Sub DownloadData(ByVal Token As CancellationToken)
            If IsChannel Then
                ChannelPostsNames.ListAddList(ChannelInfo.PostsAll.Select(Function(p) p.ID), LNC)
                If SkipExistsUsers Then _ExistsUsersNames.ListAddList(Settings.UsersList.Select(Function(p) p.Name), LNC)
                DownloadDataF(Token)
                ReparseVideo(Token)
                _ContentNew.ListAddList(_TempMediaList, LAP.ClearBeforeAdd)
                DownloadContent(Token)
            Else
                MyBase.DownloadData(Token)
            End If
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            _TotalPostsDownloaded = 0
            If IsChannel Then
                DownloadDataChannel(String.Empty, Token)
            Else
                DownloadDataUser(String.Empty, Token)
            End If
        End Sub
#End Region
#Region "Download Functions (User, Channel)"
        Private _TotalPostsDownloaded As Integer = 0
        Private Sub DownloadDataUser(ByVal POST As String, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                Dim PostID$ = String.Empty
                Dim PostDate$, PostTitle$
                Dim n As EContainer, nn As EContainer, s As EContainer
                Dim NewPostDetected As Boolean = False
                Dim ExistsDetected As Boolean = False
                Dim _ItemsBefore%
                Dim added As Boolean
                Dim __ItemType$
                Dim tmpType As UTypes
                Dim CheckNode As Predicate(Of EContainer) = Function(e) e("author").XmlIfNothingValue("/").ToLower.Equals(Name.ToLower)
                Dim UPicType As Func(Of String, UTypes) = Function(input) IIf(input = "image", UTypes.Picture, UTypes.GIF)

                URL = $"https://gateway.reddit.com/desktopapi/v1/user/{Name}/posts?rtj=only&allow_quarantined=true&allow_over18=1&include=identity&after={POST}&dist=25&sort=new&t=all&layout=classic"
                ThrowAny(Token)
                Dim r$ = GetSiteResponse(URL)
                If Not r.IsEmptyString Then
                    Using w As EContainer = JsonDocument.Parse(r).XmlIfNothing
                        If w.Count > 0 Then
                            n = w.GetNode(JsonNodesJson)
                            If Not n Is Nothing AndAlso n.Count > 0 Then
                                For Each nn In n
                                    ThrowAny(Token)
                                    If nn.Count > 0 Then
                                        PostID = nn.Name
                                        If PostID.IsEmptyString AndAlso nn.Contains("id") Then PostID = nn("id").Value
                                        If nn.Contains("created") Then PostDate = nn("created").Value Else PostDate = String.Empty
                                        If Not _TempPostsList.Contains(PostID) Then
                                            NewPostDetected = True
                                            _TempPostsList.Add(PostID)
                                        Else
                                            ExistsDetected = True
                                            Continue For
                                        End If
                                        PostTitle = nn.Value("title")

                                        If CheckNode(nn) Then
                                            _ItemsBefore = _TempMediaList.Count
                                            added = True
                                            s = nn.ItemF({"source", "url"})
                                            If s.XmlIfNothingValue("/").Contains("redgifs.com") Then
                                                _TempMediaList.ListAddValue(MediaFromData(UTypes.VideoPre, s.Value, PostID, PostDate,, IsChannel, PostTitle), LNC)
                                            Else
                                                s = nn.ItemF({"media"}).XmlIfNothing
                                                __ItemType = s("type").XmlIfNothingValue
                                                Select Case __ItemType
                                                    Case "gallery" : If Not DownloadGallery(s, PostID, PostDate,,, PostTitle) Then added = False
                                                    Case "image", "gifvideo"
                                                        If s.Contains("content") Then
                                                            _TempMediaList.ListAddValue(MediaFromData(UPicType(__ItemType), s.Value("content"),
                                                                                                      PostID, PostDate,, IsChannel, PostTitle), LNC)
                                                        Else
                                                            added = False
                                                        End If
                                                    Case "video"
                                                        If Settings.UseM3U8 AndAlso s("hlsUrl").XmlIfNothingValue("/").ToLower.Contains("m3u8") Then
                                                            _TempMediaList.ListAddValue(MediaFromData(UTypes.m3u8, s.Value("hlsUrl"),
                                                                                                      PostID, PostDate,, IsChannel, PostTitle), LNC)
                                                        Else
                                                            added = False
                                                        End If
                                                    Case Else : added = False
                                                End Select
                                            End If
                                            If Not added Then
                                                s = nn.ItemF({"source", "url"}).XmlIfNothing
                                                If Not s.IsEmptyString AndAlso TryFile(s.Value) Then
                                                    With s.Value.ToLower
                                                        Select Case True
                                                            Case .Contains("redgifs") : tmpType = UTypes.VideoPre
                                                            Case .Contains("m3u8") : If Settings.UseM3U8 Then tmpType = UTypes.m3u8
                                                            Case .Contains(".gif") And TryFile(s.Value) : tmpType = UTypes.GIF
                                                            Case TryFile(s.Value) : tmpType = UTypes.Picture
                                                            Case Else : tmpType = UTypes.Undefined
                                                        End Select
                                                    End With
                                                    If Not tmpType = UTypes.Undefined Then
                                                        _TempMediaList.ListAddValue(MediaFromData(tmpType, s.Value, PostID, PostDate,, IsChannel, PostTitle), LNC)
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    End Using
                    If POST.IsEmptyString And ExistsDetected Then Exit Sub
                    If Not PostID.IsEmptyString And NewPostDetected Then DownloadDataUser(PostID, Token)
                End If
            Catch oex As OperationCanceledException When Token.IsCancellationRequested
            Catch dex As ObjectDisposedException When Disposed
            Catch ex As Exception
                LogError(ex, $"data downloading error [{URL}]")
                HasError = True
            End Try
        End Sub
        Private Sub DownloadDataChannel(ByVal POST As String, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                Dim PostID$ = String.Empty
                Dim PostDate$, _UserID$, tmpUrl$
                Dim n As EContainer, nn As EContainer, s As EContainer, ss As EContainer
                Dim NewPostDetected As Boolean = False
                Dim ExistsDetected As Boolean = False
                Dim eCount As Predicate(Of EContainer) = Function(e) e.Count > 0
                Dim lDate As Date?

                URL = $"https://reddit.com/r/{Name}/new.json?allow_quarantined=true&allow_over18=1&include=identity&after={POST}&dist=25&sort=new&t=all&layout=classic"
                ThrowAny(Token)
                Dim r$ = GetSiteResponse(URL)
                If Not r.IsEmptyString Then
                    Using w As EContainer = JsonDocument.Parse(r).XmlIfNothing
                        If w.Count > 0 Then
                            n = w.GetNode(ChannelJsonNodes)
                            If Not n Is Nothing AndAlso n.Count > 0 Then
                                For Each nn In n
                                    ThrowAny(Token)
                                    s = nn.ItemF({eCount})
                                    If Not s Is Nothing AndAlso s.Count > 0 Then
                                        PostID = s.Value("name")
                                        If PostID.IsEmptyString AndAlso s.Contains("id") Then PostID = s("id").Value

                                        If ChannelPostsNames.Contains(PostID) Then ExistsDetected = True : Continue For 'Exit Sub
                                        If DownloadLimitCount.HasValue AndAlso _TotalPostsDownloaded >= DownloadLimitCount.Value Then Exit Sub
                                        If Not DownloadLimitPost.IsEmptyString AndAlso DownloadLimitPost = PostID Then Exit Sub
                                        If DownloadLimitDate.HasValue AndAlso _TempMediaList.Count > 0 Then
                                            With (From __u In _TempMediaList Where __u.Post.Date.HasValue Select __u.Post.Date.Value)
                                                If .Count > 0 Then lDate = .Min Else lDate = Nothing
                                            End With
                                            If lDate.HasValue AndAlso lDate.Value <= DownloadLimitDate.Value Then Exit Sub
                                        End If
                                        NewPostDetected = True

                                        If s.Contains("created") Then PostDate = s("created").Value Else PostDate = String.Empty
                                        _UserID = s.Value("author")

                                        If SkipExistsUsers AndAlso _ExistsUsersNames.Count > 0 AndAlso
                                           Not _UserID.IsEmptyString AndAlso _ExistsUsersNames.Contains(_UserID) Then Continue For

                                        tmpUrl = s.Value("url")
                                        If Not tmpUrl.IsEmptyString AndAlso tmpUrl.Contains("redgifs.com") Then
                                            If SaveToCache Then
                                                tmpUrl = s.Value({"media", "oembed"}, "thumbnail_url")
                                                If Not tmpUrl.IsEmptyString Then
                                                    _TempMediaList.ListAddValue(MediaFromData(UTypes.Picture, tmpUrl, PostID, PostDate, _UserID, IsChannel), LNC)
                                                    _TotalPostsDownloaded += 1
                                                End If
                                            Else
                                                _TempMediaList.ListAddValue(MediaFromData(UTypes.VideoPre, tmpUrl, PostID, PostDate, _UserID, IsChannel), LNC)
                                                _TotalPostsDownloaded += 1
                                            End If
                                        ElseIf s.Item("media_metadata").XmlIfNothing.Count > 0 Then
                                            DownloadGallery(s, PostID, PostDate, _UserID, SaveToCache)
                                            _TotalPostsDownloaded += 1
                                        ElseIf s.Contains("preview") Then
                                            ss = s.ItemF({"preview", "images", eCount, "source", "url"}).XmlIfNothing
                                            If Not ss.Value.IsEmptyString Then
                                                _TempMediaList.ListAddValue(MediaFromData(UTypes.Picture, ss.Value, PostID, PostDate, _UserID, IsChannel), LNC)
                                                _TotalPostsDownloaded += 1
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    End Using
                    If POST.IsEmptyString And ExistsDetected Then Exit Sub
                    If Not PostID.IsEmptyString And NewPostDetected Then DownloadDataChannel(PostID, Token)
                End If
            Catch oex As OperationCanceledException When Token.IsCancellationRequested
            Catch dex As ObjectDisposedException When Disposed
            Catch ex As Exception
                LogError(ex, $"channel data downloading error [{URL}]")
                HasError = True
            End Try
        End Sub
#End Region
#Region "Download Base Functions"
        Private Function DownloadGallery(ByVal w As EContainer, ByVal PostID As String, ByVal PostDate As String,
                                         Optional ByVal _UserID As String = Nothing, Optional ByVal FirstOnly As Boolean = False,
                                         Optional ByVal Title As String = Nothing) As Boolean
            Try
                Dim added As Boolean = False
                Dim cn$ = IIf(IsChannel, "media_metadata", "mediaMetadata")
                If Not w Is Nothing AndAlso w(cn).XmlIfNothing.Count > 0 Then
                    Dim t As EContainer
                    For Each n As EContainer In w(cn)
                        t = n.ItemF({"s", "u"})
                        If Not t Is Nothing AndAlso Not t.Value.IsEmptyString Then
                            _TempMediaList.ListAddValue(MediaFromData(UTypes.Picture, t.Value, PostID, PostDate, _UserID, IsChannel, Title), LNC)
                            added = True
                            If FirstOnly Then Exit For
                        End If
                    Next
                End If
                Return added
            Catch ex As Exception
                LogError(ex, "gallery parsing error")
                HasError = True
                Return False
            End Try
        End Function
        Protected Overrides Sub ReparseVideo(ByVal Token As CancellationToken)
            Try
                ThrowAny(Token)
                If _TempMediaList.Count > 0 AndAlso _TempMediaList.Exists(Function(p) p.Type = UTypes.VideoPre) Then
                    Dim r$, v$
                    Dim e As New ErrorsDescriber(EDP.ReturnValue)
                    Dim m As UserMedia
                    For i% = _TempMediaList.Count - 1 To 0 Step -1
                        ThrowAny(Token)
                        If _TempMediaList(i).Type = UTypes.VideoPre Then
                            m = _TempMediaList(i)
                            r = GetSiteResponse(m.URL, e)
                            _TempMediaList(i) = New UserMedia
                            If Not r.IsEmptyString Then
                                v = RegexReplace(r, VideoRegEx)
                                If Not v.IsEmptyString Then
                                    _TempMediaList(i) = New UserMedia With {.Type = UTypes.Video, .URL = v, .File = v, .Post = m.Post}
                                Else
                                    _TempMediaList.RemoveAt(i)
                                End If
                            End If
                        End If
                    Next
                End If
            Catch oex As OperationCanceledException When Token.IsCancellationRequested
            Catch dex As ObjectDisposedException When Disposed
            Catch ex As Exception
                LogError(ex, "video reparsing error")
            End Try
        End Sub
        Friend Shared Function GetVideoInfo(ByVal URL As String) As UserMedia
            Try
                If Not URL.IsEmptyString AndAlso URL.Contains("redgifs") Then
                    Using r As New UserData
                        r._TempMediaList.Add(MediaFromData(UTypes.VideoPre, URL, String.Empty, String.Empty,, False))
                        r.ReparseVideo(Nothing)
                        If r._TempMediaList.ListExists Then Return r._TempMediaList(0)
                    End Using
                End If
                Return Nothing
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.ShowMainMsg + EDP.SendInLog, ex, "Video searching error")
            End Try
        End Function
#End Region
#Region "Structure creator"
        Protected Shared Function MediaFromData(ByVal t As UTypes, ByVal _URL As String, ByVal PostID As String, ByVal PostDate As String,
                                                Optional ByVal _UserID As String = "", Optional ByVal IsChannel As Boolean = False,
                                                Optional ByVal Title As String = Nothing) As UserMedia
            If _URL.IsEmptyString And t = UTypes.Picture Then Return Nothing
            _URL = LinkFormatterSecure(RegexReplace(_URL.Replace("\", String.Empty), LinkPattern))
            Dim m As New UserMedia(_URL, t) With {.Post = New UserPost With {.ID = PostID, .UserID = _UserID}}
            If t = UTypes.Picture Or t = UTypes.GIF Then m.File = UrlToFile(m.URL) Else m.File = Nothing
            If m.URL.Contains("preview") Then m.URL = $"https://i.redd.it/{m.File.File}"
            If Not PostDate.IsEmptyString Then m.Post.Date = AConvert(Of Date)(PostDate, If(IsChannel, DateProviderChannel, DateProvider), Nothing) Else m.Post.Date = Nothing
            If Not Title.IsEmptyString Then m.Post.Title = Title
            Return m
        End Function
        Private Function TryFile(ByVal URL As String) As Boolean
            Try
                If Not URL.IsEmptyString AndAlso URL.Contains(".jpg") Then
                    Dim f As SFile = CStr(RegexReplace(URL, FilesPattern))
                    Return Not f.IsEmptyString And Not f.File.IsEmptyString
                End If
                Return False
            Catch ex As Exception
                Return False
            End Try
        End Function
        Private Shared Function UrlToFile(ByVal URL As String) As SFile
            Return CStr(RegexReplace(URL, FilesPattern))
        End Function
#End Region
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            Try
                Dim i%
                Dim dCount% = 0, dTotal% = 0
                ThrowAny(Token)
                If _ContentNew.Count > 0 Then
                    _ContentNew.RemoveAll(Function(c) c.URL.IsEmptyString)
                    If _ContentNew.Count > 0 Then
                        MyFile.Exists(SFO.Path)
                        Dim MyDir$
                        If IsChannel And SaveToCache Then
                            MyDir = ChannelInfo.CachePath.PathNoSeparator
                        Else
                            MyDir = MyFile.CutPath.PathNoSeparator
                        End If
                        Dim HashList As New List(Of String)
                        If _ContentList.Count > 0 Then HashList.ListAddList((From h In _ContentList Where Not h.MD5.IsEmptyString Select h.MD5), LNC)
                        Dim f As SFile
                        Dim v As UserMedia
                        Dim cached As Boolean = IsChannel And SaveToCache
                        Dim vsf As Boolean = SeparateVideoFolderF
                        Dim ImgFormat As Imaging.ImageFormat
                        Dim bDP As New ErrorsDescriber(EDP.None)
                        Dim MD5BS As Func(Of String, UTypes,
                                             SFile, Boolean, String) = Function(ByVal __URL As String, ByVal __MT As UTypes,
                                                                                ByVal __File As SFile, ByVal __IsBase As Boolean) As String
                                                                           Try
                                                                               If __MT = UTypes.GIF Then
                                                                                   ImgFormat = Imaging.ImageFormat.Gif
                                                                               ElseIf __IsBase Then
                                                                                   ImgFormat = GetImageFormat(CStr(RegexReplace(__URL, UrlBasePattern)))
                                                                               Else
                                                                                   ImgFormat = GetImageFormat(__File)
                                                                               End If
                                                                               Return ByteArrayToString(GetMD5(SFile.GetBytesFromNet(__URL, bDP), ImgFormat))
                                                                           Catch hash_ex As Exception
                                                                               Return String.Empty
                                                                           End Try
                                                                       End Function
                        Dim m$
                        Using w As New WebClient
                            If vsf Then SFileShares.SFileExists($"{MyDir}\Video\", SFO.Path)
                            Progress.TotalCount += _ContentNew.Count
                            For i = 0 To _ContentNew.Count - 1
                                ThrowAny(Token)
                                v = _ContentNew(i)
                                v.State = UStates.Tried
                                If v.File.IsEmptyString Then
                                    f = UrlToFile(v.URL)
                                Else
                                    f = v.File
                                End If
                                f.Separator = "\"
                                m = String.Empty
                                If (v.Type = UTypes.Picture Or v.Type = UTypes.GIF) And Not cached Then
                                    m = MD5BS(v.URL, v.Type, f, False)
                                    If m.IsEmptyString AndAlso Not v.URL_BASE.IsEmptyString AndAlso Not v.URL_BASE = v.URL Then
                                        m = MD5BS(v.URL_BASE, v.Type, f, True)
                                        If Not m.IsEmptyString Then v.URL = v.URL_BASE
                                    End If
                                End If

                                If (Not m.IsEmptyString AndAlso Not HashList.Contains(m)) Or Not (v.Type = UTypes.Picture Or
                                                                                                  v.Type = UTypes.GIF) Or cached Then
                                    If Not cached Then HashList.Add(m)
                                    v.MD5 = m
                                    f.Path = MyDir
                                    Try
                                        If (v.Type = UTypes.Video Or v.Type = UTypes.m3u8) And vsf Then f.Path = $"{f.PathWithSeparator}Video"
                                        If v.Type = UTypes.m3u8 Then
                                            f = M3U8.Download(v.URL, f)
                                        Else
                                            w.DownloadFile(v.URL, f.ToString)
                                        End If
                                        If Not v.Type = UTypes.m3u8 Or Not f.IsEmptyString Then
                                            Select Case v.Type
                                                Case UTypes.Picture : DownloadedPictures += 1 : _CountPictures += 1
                                                Case UTypes.Video, UTypes.m3u8 : DownloadedVideos += 1 : _CountVideo += 1
                                            End Select
                                            If Not IsChannel Or Not SaveToCache Then
                                                v.File = ChangeFileNameByProvider(f, v)
                                            Else
                                                v.File = f
                                            End If
                                            v.Post.CachedFile = f
                                            v.State = UStates.Downloaded
                                            dCount += 1
                                        End If
                                    Catch wex As Exception
                                        If Not IsChannel Then ErrorDownloading(f, v.URL)
                                    End Try
                                Else
                                    v.State = UStates.Skipped
                                End If
                                _ContentNew(i) = v
                                If (CreatedByChannel And Settings.FromChannelDownloadTopUse And dCount >= Settings.FromChannelDownloadTop) Or
                                   (DownloadTopCount.HasValue AndAlso dCount >= DownloadTopCount.Value) Then
                                    Progress.Perform(_ContentNew.Count - dTotal)
                                    Exit Sub
                                Else
                                    dTotal += 1
                                    Progress.Perform()
                                End If
                            Next
                        End Using
                    End If
                End If
            Catch oex As OperationCanceledException When Token.IsCancellationRequested
            Catch dex As ObjectDisposedException When Disposed
            Catch ex As Exception
                LogError(ex, "content downloading error")
                HasError = True
            End Try
        End Sub
        Protected Function GetSiteResponse(ByVal URL As String, Optional ByVal e As ErrorsDescriber = Nothing) As String
            Try
                Return Settings.Site(Sites.Reddit).Responser.GetResponse(URL,, EDP.ThrowException)
            Catch ex As Exception
                HasError = True
                Dim OptText$ = String.Empty
                If Not e.Exists Then
                    Dim ee As EDP = EDP.SendInLog
                    If Settings.Site(Sites.Reddit).Responser.StatusCode = HttpStatusCode.NotFound Then
                        ee += EDP.ThrowException
                        OptText = ": USER NOT FOUND"
                    Else
                        ee += EDP.ReturnValue
                    End If
                    e = New ErrorsDescriber(ee)
                End If
                Return ErrorsDescriber.Execute(e, ex, $"[{Site} - {Name}: GetSiteResponse([{URL}])]{OptText}", String.Empty)
            End Try
        End Function
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then ChannelPostsNames.Clear() : _ExistsUsersNames.Clear()
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace