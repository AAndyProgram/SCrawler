Imports PersonalUtilities.Tools.WebDocuments.JSON
Imports PersonalUtilities.Functions.XML
Imports System.Net
Imports System.Threading
Imports SCrawler.API.Base
Imports UStates = SCrawler.API.Base.UserMedia.States
Namespace API.Twitter
    Friend Class UserData : Inherits UserDataBase
#Region "Declarations"
        Friend Overrides Property Site As Sites = Sites.Twitter
        Private Structure Sizes : Implements IComparable(Of Sizes)
            Friend Value As Integer
            Friend Name As String
            Friend ReadOnly HasError As Boolean
            Friend Sub New(ByVal _Value As String, ByVal _Name As String)
                Try
                    Value = _Value
                    Name = _Name
                Catch ex As Exception
                    HasError = True
                End Try
            End Sub
            Friend Function CompareTo(ByVal Other As Sizes) As Integer Implements IComparable(Of Sizes).CompareTo
                Return Value.CompareTo(Other.Value) * -1
            End Function
            Friend Shared Function Reparse(ByRef Current As Sizes, ByVal Other As Sizes, ByVal LargeContained As Boolean) As Sizes
                If LargeContained And Current.Name.IsEmptyString And Current.Value > Other.Value Then Current.Name = "large"
                Return Current
            End Function
            Friend Shared Function ApplyLarge(ByRef s As Sizes) As Sizes
                s.Name = "large"
                Return s
            End Function
        End Structure
#End Region
#Region "Initializer"
        Friend Sub New(ByVal u As UserInfo, Optional ByVal _LoadUserInformation As Boolean = True)
            User = u
            If _LoadUserInformation Then LoadUserInformation()
        End Sub
#End Region
#Region "Download functions"
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            DownloadData(String.Empty, Token)
        End Sub
        Private Overloads Sub DownloadData(ByVal POST As String, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                Dim PostID$ = String.Empty
                Dim PostDate$
                Dim m As EContainer, nn As EContainer, s As EContainer
                Dim NewPostDetected As Boolean = False
                Dim ExistsDetected As Boolean = False

                Dim PicNode As Predicate(Of EContainer) = Function(e) e.Count > 0 AndAlso e.Contains("media_url")
                Dim UID As Func(Of EContainer, String) = Function(e) e.XmlIfNothing.Item({"user", "id"}).XmlIfNothingValue

                URL = $"https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={Name}&count=200&exclude_replies=false&include_rts=1&tweet_mode=extended"
                If Not POST.IsEmptyString Then URL &= $"&max_id={POST}"

                ThrowAny(Token)
                Dim r$ = Settings.Site(Sites.Twitter).Responser.GetResponse(URL,, EDP.ThrowException)
                If Not r.IsEmptyString Then
                    Using w As EContainer = JsonDocument.Parse(r)
                        If Not w Is Nothing AndAlso w.Count > 0 Then
                            For Each nn In w
                                ThrowAny(Token)
                                If nn.Count > 0 Then
                                    PostID = nn.Value("id")
                                    If ID.IsEmptyString Then
                                        ID = UID(nn)
                                        If Not ID.IsEmptyString Then UpdateUserInformation()
                                    End If

                                    'Date Pattern:
                                    'Sat Jan 01 01:10:15 +0000 2000
                                    If nn.Contains("created_at") Then PostDate = nn("created_at").Value Else PostDate = String.Empty

                                    If Not _TempPostsList.Contains(PostID) Then
                                        NewPostDetected = True
                                        _TempPostsList.Add(PostID)
                                    Else
                                        ExistsDetected = True
                                        Continue For
                                    End If

                                    If Not ParseUserMediaOnly OrElse (Not nn.Contains("retweeted_status") OrElse
                                                                     (Not ID.IsEmptyString AndAlso UID(nn("retweeted_status")) = ID)) Then
                                        If Not CheckVideoNode(nn, PostID, PostDate) Then
                                            s = nn.ItemF({"extended_entities", "media"})
                                            If s Is Nothing OrElse s.Count = 0 Then s = nn.ItemF({"retweeted_status", "extended_entities", "media"})
                                            If Not s Is Nothing AndAlso s.Count > 0 Then
                                                For Each m In s
                                                    If m.Count > 0 AndAlso m.Contains("media_url") Then
                                                        _TempMediaList.ListAddValue(MediaFromData(m("media_url").Value,
                                                                                                  PostID, PostDate, GetPictureOption(m)), LNC)
                                                    End If
                                                Next
                                            End If
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End Using
                    If POST.IsEmptyString And ExistsDetected Then Exit Sub
                    If Not PostID.IsEmptyString And NewPostDetected Then DownloadData(PostID, Token)
                End If
            Catch oex As OperationCanceledException When Token.IsCancellationRequested
            Catch dex As ObjectDisposedException When Disposed
            Catch ex As Exception
                LogError(ex, $"data downloading error [{URL}]")
                HasError = True
            End Try
        End Sub
        Friend Shared Function GetVideoInfo(ByVal URL As String) As UserMedia
            Try
                If URL.Contains("twitter") Then
                    Dim PostID$ = RegexReplace(URL, New RegexStructure("(?<=/)\d+", True, False,,,,, String.Empty))
                    If Not PostID.IsEmptyString Then
                        Dim r$ = Settings.Site(Sites.Twitter).Responser.GetResponse($"https://api.twitter.com/1.1/statuses/show.json?id={PostID}",,
                                                                                    EDP.ReturnValue)
                        If Not r.IsEmptyString Then
                            Using j As EContainer = JsonDocument.Parse(r)
                                If j.ListExists Then
                                    Dim u$ = GetVideoNodeURL(j)
                                    If Not u.IsEmptyString Then Return MediaFromData(u, PostID, String.Empty)
                                End If
                            End Using
                        End If
                    End If
                End If
                Return Nothing
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.ShowMainMsg + EDP.SendInLog, ex, "Video searching error")
            End Try
        End Function
#Region "Picture options"
        Private Function GetPictureOption(ByVal w As EContainer) As String
            Try
                Dim ww As EContainer = w("sizes")
                If Not ww Is Nothing AndAlso ww.Count > 0 Then
                    Dim l As New List(Of Sizes)
                    Dim LargeContained As Boolean = ww.Contains("large")
                    For Each v As EContainer In ww
                        If v.Count > 0 AndAlso v.Contains("h") Then l.Add(New Sizes(v.Value("h"), v.Name))
                    Next
                    If l.Count > 0 Then
                        l.Sort()
                        If l(0).Name.IsEmptyString And LargeContained Then Return "large" Else Return l(0).Name
                    End If
                End If
                Return String.Empty
            Catch ex As Exception
                LogError(ex, "[GetPictureOption]")
                Return String.Empty
            End Try
        End Function
#End Region
#Region "Video options"
        Private Function CheckVideoNode(ByVal w As EContainer, ByVal PostID As String, ByVal PostDate As String) As Boolean
            Try
                Dim URL$ = GetVideoNodeURL(w)
                If Not URL.IsEmptyString Then _TempMediaList.ListAddValue(MediaFromData(URL, PostID, PostDate), LNC) : Return True
                Return False
            Catch ex As Exception
                LogError(ex, "[CheckVideoNode]")
                Return False
            End Try
        End Function
        Private Shared Function GetVideoNodeURL(ByVal w As EContainer) As String
            Dim v As EContainer = w.GetNode(VideoNode)
            If Not v Is Nothing AndAlso v.Count > 0 Then
                Dim l As New List(Of Sizes)
                Dim u$
                Dim nn As EContainer
                For Each n As EContainer In v
                    If n.Count > 0 Then
                        For Each nn In n
                            If nn("content_type").XmlIfNothingValue("none").Contains("mp4") AndAlso nn.Contains("url") Then
                                u = nn.Value("url")
                                l.Add(New Sizes(RegexReplace(u, VideoSizeRegEx), u))
                            End If
                        Next
                    End If
                Next
                If l.Count > 0 Then l.RemoveAll(Function(s) s.HasError)
                If l.Count > 0 Then l.Sort() : Return l(0).Name
            End If
            Return String.Empty
        End Function
        Protected Overrides Sub ReparseVideo(ByVal Token As CancellationToken)
        End Sub
#End Region
        Private Shared Function MediaFromData(ByVal _URL As String, ByVal PostID As String, ByVal PostDate As String,
                                              Optional ByVal _PictureOption As String = "") As UserMedia
            _URL = LinkFormatterSecure(RegexReplace(_URL.Replace("\", String.Empty), LinkPattern))
            Dim m As New UserMedia(_URL) With {.PictureOption = _PictureOption, .Post = New UserPost With {.ID = PostID}}
            If Not m.URL.IsEmptyString Then m.File = CStr(RegexReplace(m.URL, FilesPattern))
            If Not m.PictureOption.IsEmptyString And Not m.File.IsEmptyString And Not m.URL.IsEmptyString Then
                m.URL_BASE = $"{m.URL.Replace($".{m.File.Extension}", String.Empty)}?format={m.File.Extension}&name={m.PictureOption}"
            End If
            If Not PostDate.IsEmptyString Then m.Post.Date = AConvert(Of Date)(PostDate, Declarations.DateProvider, Nothing) Else m.Post.Date = Nothing
            Return m
        End Function
#End Region
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            Try
                Dim i%
                ThrowAny(Token)
                If _ContentNew.Count > 0 Then
                    _ContentNew.RemoveAll(Function(c) c.URL.IsEmptyString)
                    If _ContentNew.Count > 0 Then
                        MyFile.Exists(SFO.Path)
                        Dim MyDir$ = MyFile.CutPath.Path
                        Dim vsf As Boolean = SeparateVideoFolderF
                        Dim f As SFile
                        Dim v As UserMedia
                        Using w As New WebClient
                            If vsf Then SFileShares.SFileExists($"{MyDir}\Video\", SFO.Path)
                            MainProgress.TotalCount += _ContentNew.Count
                            For i = 0 To _ContentNew.Count - 1
                                ThrowAny(Token)
                                v = _ContentNew(i)
                                v.State = UStates.Tried
                                If v.File.IsEmptyString Then
                                    f = v.URL
                                Else
                                    f = v.File
                                End If
                                f.Separator = "\"
                                f.Path = MyDir

                                If v.URL_BASE.IsEmptyString Then v.URL_BASE = v.URL

                                If Not v.File.IsEmptyString AndAlso Not v.URL_BASE.IsEmptyString Then
                                    Try
                                        If f.Extension = "mp4" And vsf Then f.Path = $"{f.PathWithSeparator}Video"
                                        w.DownloadFile(v.URL_BASE, f.ToString)
                                        Select Case f.Extension
                                            Case "mp4" : v.Type = UserMedia.Types.Video : DownloadedVideos += 1 : _CountVideo += 1
                                            Case Else : v.Type = UserMedia.Types.Picture : DownloadedPictures += 1 : _CountPictures += 1
                                        End Select
                                        v.File = f
                                        v.State = UStates.Downloaded
                                    Catch wex As Exception
                                        ErrorDownloading(f, v.URL_BASE)
                                    End Try
                                Else
                                    v.State = UStates.Skipped
                                End If
                                _ContentNew(i) = v
                                MainProgress.Perform()
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
    End Class
End Namespace