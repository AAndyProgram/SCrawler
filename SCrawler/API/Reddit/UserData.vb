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
#Region "Initializers"
        ''' <summary>Video downloader initializer</summary>
        Private Sub New()
            ChannelPostsNames = New List(Of String)
            _ExistsUsersNames = New List(Of String)
            _CrossPosts = New List(Of String)
        End Sub
        ''' <summary>Default initializer</summary>
        Friend Sub New(ByVal u As UserInfo, Optional ByVal _LoadUserInformation As Boolean = True, Optional ByVal InvokeImageHandler As Boolean = True)
            MyBase.New(InvokeImageHandler)
            ChannelPostsNames = New List(Of String)
            _ExistsUsersNames = New List(Of String)
            _CrossPosts = New List(Of String)
            User = u
            If _LoadUserInformation Then LoadUserInformation()
        End Sub
#End Region
#Region "Load and Update user info"
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
        End Sub
#End Region
#Region "Download Overrides"
        Friend Overrides Sub DownloadData(ByVal Token As CancellationToken)
            _CrossPosts.Clear()
            If Not IsSavedPosts AndAlso (IsChannel AndAlso Not ChannelInfo.IsRegularChannel) Then
                If Not Responser Is Nothing Then Responser.Dispose()
                Responser = New PersonalUtilities.Tools.WEB.Response
                Responser.Copy(Settings(Sites.Reddit).Responser)
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
            If IsSavedPosts Then
                DownloadDataChannel(String.Empty, Token)
            ElseIf IsChannel Then
                If ChannelInfo.IsRegularChannel Then
                    ChannelPostsNames.ListAddList(_TempPostsList, LNC)
                    If ChannelPostsNames.Count > 0 Then
                        DownloadLimitCount = Nothing
                        With _ContentList.Where(Function(c) c.Post.Date.HasValue)
                            If .Count > 0 Then DownloadLimitDate = .Max(Function(p) p.Post.Date.Value).AddMinutes(-10)
                        End With
                    End If
                    If DownloadTopCount.HasValue Then DownloadLimitCount = DownloadTopCount
                End If
                DownloadDataChannel(String.Empty, Token)
                If ChannelInfo.IsRegularChannel Then _TempPostsList.ListAddList(_TempMediaList.Select(Function(m) m.Post.ID), LNC)
            Else
                DownloadDataUser(String.Empty, Token)
            End If
        End Sub
#End Region
#Region "Download Functions (User, Channel)"
        Private _TotalPostsDownloaded As Integer = 0
        Private ReadOnly _CrossPosts As List(Of String)
        Private Sub DownloadDataUser(ByVal POST As String, ByVal Token As CancellationToken)
            Const CPRI$ = "crosspostRootId"
            Const CPPI$ = "crosspostParentId"
            Dim URL$ = String.Empty
            Try
                Dim PostID$ = String.Empty, PostTmp$ = String.Empty
                Dim PostDate$
                Dim n As EContainer, nn As EContainer, s As EContainer
                Dim NewPostDetected As Boolean = False
                Dim ExistsDetected As Boolean = False
                Dim _ItemsBefore%
                Dim added As Boolean
                Dim __ItemType$
                Dim tmpType As UTypes
                Dim IsCrossPost As Predicate(Of EContainer) = Function(e) Not (e.Value(CPRI).IsEmptyString And e.Value(CPPI).IsEmptyString)
                Dim CheckNode As Predicate(Of EContainer) = Function(e) Not ParseUserMediaOnly OrElse e("author").XmlIfNothingValue("/").ToLower.Equals(Name.ToLower)
                Dim UPicType As Func(Of String, UTypes) = Function(input) IIf(input = "image", UTypes.Picture, UTypes.GIF)
                Dim _PostID As Func(Of String) = Function() IIf(PostTmp.IsEmptyString, PostID, PostTmp)

                URL = $"https://gateway.reddit.com/desktopapi/v1/user/{Name}/posts?rtj=only&allow_quarantined=true&allow_over18=1&include=identity&after={POST}&dist=25&sort=new&t=all&layout=classic"
                ThrowAny(Token)
                Dim r$ = Responser.GetResponse(URL,, EDP.ThrowException)
                If Not r.IsEmptyString Then
                    Using w As EContainer = JsonDocument.Parse(r).XmlIfNothing
                        If w.Count > 0 Then
                            n = w.GetNode(JsonNodesJson)
                            If Not n Is Nothing AndAlso n.Count > 0 Then
                                For Each nn In n
                                    ThrowAny(Token)
                                    If nn.Count > 0 Then
                                        If CheckNode(nn) Then

                                            'Obtain post ID
                                            PostTmp = nn.Name
                                            If PostTmp.IsEmptyString Then PostTmp = nn.Value("id")
                                            If PostTmp.IsEmptyString Then Continue For
                                            'Check for CrossPost
                                            If IsCrossPost(nn) Then
                                                _CrossPosts.ListAddList({nn.Value(CPRI), nn.Value(CPPI)}, LNC)
                                                Continue For
                                            Else
                                                If Not _CrossPosts.Contains(PostTmp) Then PostID = PostTmp : PostTmp = String.Empty
                                            End If

                                            'Download decision
                                            If Not _TempPostsList.Contains(_PostID()) Then
                                                NewPostDetected = True
                                                _TempPostsList.Add(_PostID())
                                            Else
                                                If Not _CrossPosts.Contains(_PostID()) Then ExistsDetected = True
                                                Continue For
                                            End If
                                            If nn.Contains("created") Then PostDate = nn("created").Value Else PostDate = String.Empty

                                            _ItemsBefore = _TempMediaList.Count
                                            added = True
                                            s = nn.ItemF({"source", "url"})
                                            If s.XmlIfNothingValue("/").Contains("redgifs.com") Then
                                                _TempMediaList.ListAddValue(MediaFromData(UTypes.VideoPre, s.Value, _PostID(), PostDate,, IsChannel), LNC)
                                            ElseIf Not CreateImgurMedia(s.XmlIfNothingValue, _PostID(), PostDate,, IsChannel) Then
                                                s = nn.ItemF({"media"}).XmlIfNothing
                                                __ItemType = s("type").XmlIfNothingValue
                                                Select Case __ItemType
                                                    Case "gallery" : If Not DownloadGallery(s, _PostID(), PostDate) Then added = False
                                                    Case "image", "gifvideo"
                                                        If s.Contains("content") Then
                                                            _TempMediaList.ListAddValue(MediaFromData(UPicType(__ItemType), s.Value("content"),
                                                                                                      _PostID(), PostDate,, IsChannel), LNC)
                                                        Else
                                                            added = False
                                                        End If
                                                    Case "video"
                                                        If Settings.UseM3U8 AndAlso s("hlsUrl").XmlIfNothingValue("/").ToLower.Contains("m3u8") Then
                                                            _TempMediaList.ListAddValue(MediaFromData(UTypes.m3u8, s.Value("hlsUrl"),
                                                                                                      _PostID(), PostDate,, IsChannel), LNC)
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
                                                        _TempMediaList.ListAddValue(MediaFromData(tmpType, s.Value, _PostID(), PostDate,, IsChannel), LNC)
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
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]")
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

                If IsSavedPosts Then
                    URL = $"https://www.reddit.com/user/{Name}/saved.json?after={POST}"
                Else
                    URL = $"https://reddit.com/r/{Name}/new.json?allow_quarantined=true&allow_over18=1&include=identity&after={POST}&dist=25&sort=new&t=all&layout=classic"
                End If

                ThrowAny(Token)
                Dim r$ = Responser.GetResponse(URL,, EDP.ThrowException)
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
                                        ElseIf Not s.Value({"media", "reddit_video"}, "fallback_url").IsEmptyString Then
                                            tmpUrl = s.Value({"media", "reddit_video"}, "fallback_url")
                                            If SaveToCache Then
                                                tmpUrl = s.Value("thumbnail")
                                                If Not tmpUrl.IsEmptyString Then
                                                    _TempMediaList.ListAddValue(MediaFromData(UTypes.Picture, tmpUrl, PostID, PostDate, _UserID, IsChannel), LNC)
                                                    _TotalPostsDownloaded += 1
                                                End If
                                            Else
                                                _TempMediaList.ListAddValue(MediaFromData(UTypes.VideoPre + UTypes.m3u8, tmpUrl, PostID, PostDate, _UserID, IsChannel), LNC)
                                                _TotalPostsDownloaded += 1
                                            End If
                                        ElseIf CreateImgurMedia(tmpUrl, PostID, PostDate, _UserID, IsChannel) Then
                                            _TotalPostsDownloaded += 1
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
            Catch ex As Exception
                ProcessException(ex, Token, $"channel data downloading error [{URL}]")
            End Try
        End Sub
#End Region
#Region "Download Base Functions"
        Private Function ImgurPicture(ByVal Source As EContainer, ByVal Value As String) As String
            Try
                Dim e As EContainer = Source({"source", "url"}).XmlIfNothing
                If Not e.IsEmptyString AndAlso e.Value.ToLower.Contains("imgur") Then
                    Return e.Value
                Else
                    Return Value
                End If
            Catch ex As Exception
                LogError(ex, "[ImgurPicture]")
                Return Value
            End Try
        End Function
        Private Function CreateImgurMedia(ByVal _URL As String, ByVal PostID As String, ByVal PostDate As String,
                                          Optional ByVal _UserID As String = "", Optional ByVal IsChannel As Boolean = False) As Boolean
            If Not _URL.IsEmptyString AndAlso _URL.Contains("imgur") Then
                If _URL.StringContains({".jpg", ".png", ".jpeg"}) Then
                    _TempMediaList.ListAddValue(MediaFromData(UTypes.Picture, _URL, PostID, PostDate, _UserID, IsChannel), LNC)
                ElseIf _URL.Contains(".gifv") Then
                    If SaveToCache Then
                        _TempMediaList.ListAddValue(MediaFromData(UTypes.Picture, _URL.Replace(".gifv", ".gif"),
                                                                  PostID, PostDate, _UserID, IsChannel), LNC)
                    Else
                        _TempMediaList.ListAddValue(MediaFromData(UTypes.Video, _URL.Replace(".gifv", ".mp4"),
                                                                  PostID, PostDate, _UserID, IsChannel), LNC)
                    End If
                ElseIf _URL.Contains(".gif") Then
                    _TempMediaList.ListAddValue(MediaFromData(UTypes.GIF, _URL, PostID, PostDate, _UserID, IsChannel), LNC)
                Else
                    If Not TryFile(_URL) Then _URL &= ".jpg"
                    _TempMediaList.ListAddValue(MediaFromData(UTypes.Picture, _URL, PostID, PostDate, _UserID, IsChannel), LNC)
                End If
                Return True
            Else
                Return False
            End If
        End Function
        Private Function DownloadGallery(ByVal w As EContainer, ByVal PostID As String, ByVal PostDate As String,
                                         Optional ByVal _UserID As String = Nothing, Optional ByVal FirstOnly As Boolean = False) As Boolean
            Try
                Dim added As Boolean = False
                Dim cn$ = IIf(IsChannel, "media_metadata", "mediaMetadata")
                If Not w Is Nothing AndAlso w(cn).XmlIfNothing.Count > 0 Then
                    Dim t As EContainer
                    For Each n As EContainer In w(cn)
                        t = n.ItemF({"s", "u"})
                        If Not t Is Nothing AndAlso Not t.Value.IsEmptyString Then
                            _TempMediaList.ListAddValue(MediaFromData(UTypes.Picture, t.Value, PostID, PostDate, _UserID, IsChannel), LNC)
                            added = True
                            If FirstOnly Then Exit For
                        End If
                    Next
                End If
                Return added
            Catch ex As Exception
                ProcessException(ex, Nothing, "gallery parsing error", False)
                Return False
            End Try
        End Function
        Protected Overrides Sub ReparseVideo(ByVal Token As CancellationToken)
            Try
                ThrowAny(Token)
                Const v2 As UTypes = UTypes.VideoPre + UTypes.m3u8
                If _TempMediaList.Count > 0 AndAlso _TempMediaList.Exists(Function(p) p.Type = UTypes.VideoPre Or p.Type = v2) Then
                    Dim r$, v$
                    Dim e As New ErrorsDescriber(EDP.ReturnValue)
                    Dim m As UserMedia
                    For i% = _TempMediaList.Count - 1 To 0 Step -1
                        ThrowAny(Token)
                        If _TempMediaList(i).Type = UTypes.VideoPre Or _TempMediaList(i).Type = v2 Then
                            m = _TempMediaList(i)
                            If _TempMediaList(i).Type = UTypes.VideoPre Then r = Responser.GetResponse(m.URL,, e) Else r = m.URL
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
            Catch ex As Exception
                ProcessException(ex, Token, "video reparsing error", False)
            End Try
        End Sub
        Friend Shared Function GetVideoInfo(ByVal URL As String) As IEnumerable(Of UserMedia)
            Try
                If Not URL.IsEmptyString AndAlso URL.Contains("redgifs") Then
                    Using r As New UserData
                        r._TempMediaList.Add(MediaFromData(UTypes.VideoPre, URL, String.Empty, String.Empty,, False))
                        r.Responser = New PersonalUtilities.Tools.WEB.Response
                        r.Responser.Copy(Settings(Sites.Reddit).Responser)
                        r.ReparseVideo(Nothing)
                        If r._TempMediaList.ListExists Then Return {r._TempMediaList(0)}
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
                                                Optional ByVal _UserID As String = "", Optional ByVal IsChannel As Boolean = False) As UserMedia
            If _URL.IsEmptyString And t = UTypes.Picture Then Return Nothing
            _URL = LinkFormatterSecure(RegexReplace(_URL.Replace("\", String.Empty), LinkPattern))
            Dim m As New UserMedia(_URL, t) With {.Post = New UserPost With {.ID = PostID, .UserID = _UserID}}
            If t = UTypes.Picture Or t = UTypes.GIF Then m.File = UrlToFile(m.URL) Else m.File = Nothing
            If m.URL.Contains("preview") Then m.URL = $"https://i.redd.it/{m.File.File}"
            If Not PostDate.IsEmptyString Then m.Post.Date = AConvert(Of Date)(PostDate, If(IsChannel, DateProviderChannel, DateProvider), Nothing) Else m.Post.Date = Nothing
            Return m
        End Function
        Private Function TryFile(ByVal URL As String) As Boolean
            Try
                If Not URL.IsEmptyString AndAlso URL.StringContains({".jpg", ".png", ".jpeg"}) Then
                    Dim f As SFile = CStr(RegexReplace(URL, FilesPattern))
                    Return Not f.File.IsEmptyString
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
                Const _RFN$ = "RedditVideo"
                Const RFN$ = _RFN & "{0}"
                Dim i%
                Dim dCount% = 0, dTotal% = 0
                ThrowAny(Token)
                If _ContentNew.Count > 0 Then
                    _ContentNew.RemoveAll(Function(c) c.URL.IsEmptyString)
                    If _ContentNew.Count > 0 Then
                        MyFile.Exists(SFO.Path)
                        Dim MyDir$
                        If Not IsSavedPosts AndAlso (IsChannel And SaveToCache) Then
                            MyDir = ChannelInfo.CachePath.PathNoSeparator
                        Else
                            MyDir = MyFile.CutPath.PathNoSeparator
                        End If
                        Dim StartRFN% = 0
                        If _ContentNew.Exists(Function(c) c.Type = UTypes.Video And c.URL.Contains("redd.it")) Then
                            StartRFN = SFile.Indexed_GetMaxIndex($"{MyDir}\{IIf(SeparateVideoFolderF, "Video\", String.Empty)}{_RFN}.mp4",, New SFileNumbers(_RFN, String.Empty), EDP.ReturnValue)
                        End If
                        Dim HashList As New List(Of String)
                        If _ContentList.Count > 0 Then HashList.ListAddList((From h In _ContentList Where Not h.MD5.IsEmptyString Select h.MD5), LNC)
                        Dim f As SFile
                        Dim v As UserMedia
                        Dim cached As Boolean = IsChannel And SaveToCache
                        Dim vsf As Boolean = SeparateVideoFolderF
                        Dim UseMD5 As Boolean = Not IsChannel Or (Not cached And Settings.ChannelsRegularCheckMD5)
                        Dim bDP As New ErrorsDescriber(EDP.None)
                        Dim ImgurUrls As New List(Of String)
                        Dim TryBytes As Func(Of String, Imaging.ImageFormat, String) =
                            Function(ByVal __URL As String, ByVal ImgFormat As Imaging.ImageFormat) As String
                                Try
                                    Return ByteArrayToString(GetMD5(SFile.GetBytesFromNet(__URL, bDP), ImgFormat))
                                Catch hash_ex As Exception
                                    Return String.Empty
                                End Try
                            End Function
                        Dim MD5BS As Func(Of String, UTypes,
                                             SFile, Boolean, String) = Function(ByVal __URL As String, ByVal __MT As UTypes,
                                                                                ByVal __File As SFile, ByVal __IsBase As Boolean) As String
                                                                           Try
                                                                               ImgurUrls.Clear()
                                                                               Dim ImgFormat As Imaging.ImageFormat
                                                                               If __MT = UTypes.GIF Then
                                                                                   ImgFormat = Imaging.ImageFormat.Gif
                                                                               ElseIf __IsBase Then
                                                                                   ImgFormat = GetImageFormat(CStr(RegexReplace(__URL, UrlBasePattern)))
                                                                               Else
                                                                                   ImgFormat = GetImageFormat(__File)
                                                                               End If

                                                                               Dim tmpBytes$ = TryBytes(__URL, ImgFormat)
                                                                               If tmpBytes.IsEmptyString And Not __MT = UTypes.GIF Then
                                                                                   ImgFormat = Imaging.ImageFormat.Png
                                                                                   tmpBytes = TryBytes(__URL, ImgFormat)
                                                                                   If Not tmpBytes.IsEmptyString Then Return tmpBytes
                                                                               Else
                                                                                   Return tmpBytes
                                                                               End If

                                                                               If tmpBytes.IsEmptyString And Not __MT = UTypes.GIF And __URL.Contains("imgur.com") Then
                                                                                   For c% = 0 To 1
                                                                                       If c = 0 Then
                                                                                           ImgurUrls.ListAddList(Imgur.Envir.GetGallery(__URL))
                                                                                       Else
                                                                                           ImgurUrls.ListAddValue(Imgur.Envir.GetImage(__URL))
                                                                                       End If
                                                                                       If ImgurUrls.Count > 0 Then Exit For
                                                                                   Next
                                                                               End If
                                                                               Return tmpBytes
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
                                If (v.Type = UTypes.Picture Or v.Type = UTypes.GIF) And UseMD5 Then
                                    m = MD5BS(v.URL, v.Type, f, False)
                                    If ImgurUrls.Count = 0 AndAlso m.IsEmptyString AndAlso Not v.URL_BASE.IsEmptyString AndAlso Not v.URL_BASE = v.URL Then
                                        m = MD5BS(v.URL_BASE, v.Type, f, True)
                                        If Not m.IsEmptyString Then v.URL = v.URL_BASE
                                    End If
                                End If

                                If (Not m.IsEmptyString AndAlso Not HashList.Contains(m)) Or Not (v.Type = UTypes.Picture Or
                                                                                                  v.Type = UTypes.GIF) Or Not UseMD5 Or ImgurUrls.Count > 0 Then
                                    Do
                                        If Not cached And Not m.IsEmptyString Then HashList.Add(m)
                                        v.MD5 = m
                                        If ImgurUrls.Count > 0 Then
                                            If ImgurUrls(0).IsEmptyString Then ImgurUrls.RemoveAt(0) : Continue Do
                                            f = UrlToFile(ImgurUrls(0))
                                            If f.Extension.IsEmptyString Then f.Extension = "gif"
                                            If f.Name.IsEmptyString Then
                                                f.Path = MyDir
                                                f.Name = $"ImgurImg_{v.File.Name}"
                                                f = SFile.Indexed_IndexFile(f,,, EDP.ReturnValue)
                                            End If
                                        End If
                                        f.Path = MyDir
                                        Try
                                            If (v.Type = UTypes.Video Or v.Type = UTypes.m3u8 Or (ImgurUrls.Count > 0 AndAlso f.Extension = "mp4")) And
                                                vsf Then f.Path = $"{f.PathWithSeparator}Video"
                                            If v.Type = UTypes.Video AndAlso v.URL.Contains("redd.it") Then
                                                StartRFN += 1
                                                f.Name = String.Format(RFN, StartRFN)
                                            End If
                                            If v.Type = UTypes.m3u8 Then
                                                f = M3U8.Download(v.URL, f)
                                            ElseIf ImgurUrls.Count > 0 Then
                                                w.DownloadFile(ImgurUrls(0), f.ToString)
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
                                        If ImgurUrls.Count > 0 Then ImgurUrls.RemoveAt(0)
                                    Loop While ImgurUrls.Count > 0
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
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False) As Integer
            If Responser.StatusCode = HttpStatusCode.NotFound Then
                UserExists = False
            ElseIf Responser.StatusCode = HttpStatusCode.Forbidden Then
                UserSuspended = True
            ElseIf Responser.StatusCode = HttpStatusCode.BadGateway Or
                   Responser.StatusCode = HttpStatusCode.ServiceUnavailable Or
                   Responser.StatusCode = HttpStatusCode.GatewayTimeout Then
                MyMainLOG = "Reddit is currently unavailable"
            Else
                If Not FromPE Then LogError(ex, Message) : HasError = True
                Return 0
            End If
            Return 1
        End Function
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then ChannelPostsNames.Clear() : _ExistsUsersNames.Clear() : _CrossPosts.Clear()
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace