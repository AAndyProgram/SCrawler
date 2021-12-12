Imports PersonalUtilities.Functions.XML
Imports SCrawler.API
Imports SCrawler.API.Base
Friend Module MainMod
    Friend Settings As SettingsCLS
    Friend Const SettingsFolderName As String = "Settings"
    Friend ReadOnly LinkPattern As New RegexStructure("[htps:]{0,6}[/]{0,2}(.+)", 1)
    Friend ReadOnly FilesPattern As New RegexStructure("[^\./]+?\.\w+", True, False, 1)
    Friend Const LVI_TempOption As String = "Temp"
    Friend Const LVI_FavOption As String = "Favorite"
    Friend Const CannelsLabelName As String = "Channels"
    Friend Const LVI_CollectionOption As String = "Collection"
    Friend Enum ViewModes As Integer
        IconLarge = 0
        IconSmall = 2
        List = 3
    End Enum
    Friend Enum ShowingModes As Integer
        All = 0
        Regular = 20
        Temporary = 50
        Favorite = 100
        Labels = 500
        NoLabels = 1000
    End Enum
    Friend Downloader As TDownloader
    Friend InfoForm As DownloadedInfoForm
    Friend VideoDownloader As VideosDownloaderForm
    Friend ReadOnly ParsersDataDateProvider As New ADateTime(ADateTime.Formats.BaseDateTime)
#Region "File name operations"
    Friend FileDateAppenderProvider As IFormatProvider
    ''' <summary>File, Date</summary>
    Friend FileDateAppenderPattern As String
    Friend Class NumberedFile : Inherits SFileNumbers
        Friend Sub New(ByVal f As SFile)
            FileName = f.Name
            NumberProvider = New ANumbers With {.FormatMode = ANumbers.Formats.NumberGroup, .GroupSize = 5}
        End Sub
    End Class
#End Region
    Friend Property MainProgress As PersonalUtilities.Forms.Toolbars.MyProgress
    Friend Function GetLviGroupName(ByVal Site As Sites, ByVal Temp As Boolean, ByVal Fav As Boolean, ByVal IsCollection As Boolean) As String
        Dim Opt$ = String.Empty
        If Temp Then
            Opt = LVI_TempOption
        ElseIf Fav Then
            Opt = LVI_FavOption
        End If
        If Not Opt.IsEmptyString Then Opt = $" ({Opt})"
        If IsCollection Then
            Return $"{LVI_CollectionOption}{Opt}"
        Else
            Return $"{Site}{Opt}"
        End If
    End Function
    Friend Enum Sites As Integer
        Undefined = 0
        Reddit = 1
        Twitter = 2
    End Enum
    Friend Structure UserInfo : Implements IComparable(Of UserInfo), IEquatable(Of UserInfo), ICloneable
        Friend Const Name_Site As String = "Site"
        Friend Const Name_Collection As String = "Collection"
        Friend Const Name_Merged As String = "Merged"
        Friend Const Name_IsChannel As String = "IsChannel"
        Friend Name As String
        Friend Site As Sites
        Friend File As SFile
        Friend Merged As Boolean
        Friend IncludedInCollection As Boolean
        Friend CollectionName As String
        Friend IsChannel As Boolean
        Friend Sub New(ByVal _Name As String, ByVal s As Sites, Optional ByVal Collection As String = Nothing,
                       Optional ByVal _Merged As Boolean = False)
            Name = _Name
            Site = s
            IncludedInCollection = Not Collection.IsEmptyString
            CollectionName = Collection
            Merged = _Merged
            UpdateUserFile()
        End Sub
        Friend Sub New(ByVal x As EContainer)
            Me.New(x.Value,
                   x.Attribute(Name_Site).Value.FromXML(Of Integer)(CInt(Sites.Undefined)),
                   x.Attribute(Name_Collection).Value, x.Attribute(Name_Merged).Value.FromXML(Of Boolean)(False))
            IsChannel = x.Attribute(Name_IsChannel).Value.FromXML(Of Boolean)(False)
        End Sub
        Friend Sub New(ByVal c As Reddit.Channel)
            Name = c.Name
            Site = Sites.Reddit
            File = c.File
            IsChannel = True
        End Sub
        Public Shared Widening Operator CType(ByVal x As EContainer) As UserInfo
            Return New UserInfo(x)
        End Operator
        Public Shared Widening Operator CType(ByVal u As UserInfo) As String
            Return u.Name
        End Operator
        Public Shared Operator =(ByVal x As UserInfo, ByVal y As UserInfo)
            Return x.Equals(y)
        End Operator
        Public Shared Operator <>(ByVal x As UserInfo, ByVal y As UserInfo)
            Return Not x.Equals(y)
        End Operator
        Public Overrides Function ToString() As String
            Return Name
        End Function
        Friend Sub UpdateUserFile()
            File = New SFile With {
                .Separator = "\",
                .Path = GetFilePathByParams(),
                .Extension = "xml",
                .Name = $"{UserDataBase.UserFileAppender}_{Site}_{Name}"
            }
        End Sub
        Private Function GetFilePathByParams() As String
            If Merged And IncludedInCollection Then
                Return $"{Settings.CollectionsPathF.PathNoSeparator}\{CollectionName}\{SettingsFolderName}"
            Else
                If IncludedInCollection Then
                    Return $"{Settings.CollectionsPathF.PathNoSeparator}\{CollectionName}\{Site}_{Name}\{SettingsFolderName}"
                Else
                    Return $"{Settings.Site(Site).Path.PathNoSeparator}\{Name}\{SettingsFolderName}"
                End If
            End If
        End Function
        Friend Function GetContainer() As EContainer
            Return New EContainer("User", Name, {New EAttribute(Name_Site, CInt(Site)),
                                                 New EAttribute(Name_Collection, CollectionName),
                                                 New EAttribute(Name_Merged, Merged.BoolToInteger),
                                                 New EAttribute(Name_IsChannel, IsChannel.BoolToInteger)})
        End Function
        Friend Function CompareTo(ByVal Other As UserInfo) As Integer Implements IComparable(Of UserInfo).CompareTo
            If Site = Other.Site Then
                Return Name.CompareTo(Other.Name)
            Else
                Return CInt(Site).CompareTo(CInt(Other.Site))
            End If
        End Function
        Friend Overloads Function Equals(ByVal Other As UserInfo) As Boolean Implements IEquatable(Of UserInfo).Equals
            Return Site = Other.Site And Name = Other.Name
        End Function
        Public Overloads Overrides Function Equals(ByVal Obj As Object) As Boolean
            Return Equals(DirectCast(Obj, UserInfo))
        End Function
        Friend Function Clone() As Object Implements ICloneable.Clone
            Return New UserInfo With {
                .Name = Name,
                .Site = Site,
                .File = File,
                .Merged = Merged,
                .IncludedInCollection = IncludedInCollection,
                .CollectionName = CollectionName
            }
        End Function
    End Structure
#Region "Image Handlers management"
    Friend Sub ImageHandler(ByVal User As IUserData)
        ImageHandler(User, False)
        ImageHandler(User, True)
    End Sub
    Friend Sub ImageHandler(ByVal User As IUserData, ByVal Add As Boolean)
        Try
            If Add Then
                AddHandler User.Self.OnPictureUpdated, AddressOf MainFrame.User_OnPictureUpdated
            Else
                RemoveHandler User.Self.OnPictureUpdated, AddressOf MainFrame.User_OnPictureUpdated
            End If
        Catch ex As Exception
        End Try
    End Sub
    Friend Sub CollectionHandler(ByVal [Collection] As UserDataBind)
        Try
            AddHandler Collection.OnCollectionSelfRemoved, AddressOf MainFrame.RefillList
        Catch ex As Exception
        End Try
    End Sub
#End Region
#Region "Standalone video download functions"
    Friend Function GetCurrentBuffer() As String
        Dim b$ = BufferText
        If Not (Not b.IsEmptyString AndAlso b.Length > 4 AndAlso b.StartsWith("http")) Then b = String.Empty
        Return b
    End Function
    Friend Function GetNewVideoURL() As String
        Dim b$ = GetCurrentBuffer()
        Dim URL$ = InputBox("Enter video URL:", "Download video by URL", b)
        If Not URL.IsEmptyString Then Return URL Else Return String.Empty
    End Function
    Friend Sub DownloadVideoByURL()
        If VideoDownloader Is Nothing Then VideoDownloader = New VideosDownloaderForm
        If VideoDownloader.Visible Then
            VideoDownloader.BringToFront()
        Else
            VideoDownloader.Show()
        End If
    End Sub
    Friend Function DownloadVideoByURL(ByVal URL As String, ByVal AskForPath As Boolean, ByVal Silent As Boolean) As Boolean
        Dim e As New ErrorsDescriber(Not Silent, Not Silent, True, False)
        Try
            If Not URL.IsEmptyString Then
                Dim u As UserMedia = Nothing
                If URL.Contains("twitter") Then
                    u = Twitter.UserData.GetVideoInfo(URL)
                ElseIf URL.Contains("redgifs") Then
                    u = Reddit.UserData.GetVideoInfo(URL)
                Else
                    MsgBoxE("Site of video URL does not recognized" & vbCr & "Operation canceled", MsgBoxStyle.Exclamation, e)
                    Return False
                End If

                If Not u.URL.IsEmptyString Or Not u.URL_BASE.IsEmptyString Then
                    Dim f As SFile = u.File
                    If f.Name.IsEmptyString Then f.Name = $"video_{u.Post.ID}"
                    If f.Extension.IsEmptyString Then f.Extension = "mp4"
                    If Not Settings.LatestSavingPath.IsEmptyString And
                        Settings.LatestSavingPath.Value.Exists(SFO.Path, False) Then f.Path = Settings.LatestSavingPath.Value
                    If AskForPath OrElse Not f.Exists(SFO.Path, False) Then
#Disable Warning BC40000
                        f = SFile.SaveAs(f, "Video file destination", True, "mp4", "Video|*.mp4|All files|*.*", EDP.ReturnValue)
#Enable Warning
                    End If
                    If Not f.IsEmptyString Then
                        Settings.LatestSavingPath.Value = f.PathWithSeparator
                        Dim dURL$
                        Dim FileDownloaded As Boolean = False
                        Using w As New Net.WebClient
                            For i% = 0 To 1
                                If i = 0 Then dURL = u.URL Else dURL = u.URL_BASE
                                If Not dURL.IsEmptyString Then
                                    Try
                                        w.DownloadFile(dURL, f)
                                        FileDownloaded = True
                                        Exit For
                                    Catch wex As Exception
                                        ErrorsDescriber.Execute(EDP.SendInLog, wex, "DownloadVideoByURL")
                                    End Try
                                End If
                            Next
                        End Using
                        If FileDownloaded Then
                            MsgBoxE($"File downloaded to [{f}]",, e)
                            Return True
                        Else
                            MsgBoxE("File does not downloaded", MsgBoxStyle.Critical, e)
                        End If
                    Else
                        MsgBoxE("File destination does not pointed" & vbCr & "Operation canceled",, e)
                    End If
                Else
                    MsgBoxE("File URL does not found!", MsgBoxStyle.Critical, e)
                End If
            Else
                MsgBoxE("URL is empty", MsgBoxStyle.Exclamation, e)
            End If
            Return False
        Catch ex As Exception
            Return ErrorsDescriber.Execute(e, ex, "Downloading video by URL error", False)
        End Try
    End Function
#End Region
#Region "Blacklist Support"
    Friend Structure UserBan
        Friend ReadOnly Name As String
        Friend ReadOnly Reason As String
        Friend ReadOnly Exists As Boolean
        Friend Sub New(ByVal Value As String)
            If Not Value.IsEmptyString Then
                Dim v$() = Value.Split("|")
                If v.ListExists Then
                    Name = v(0)
                    If v.Length > 1 Then Reason = v(1)
                    Exists = True
                End If
            End If
        End Sub
        Friend Sub New(ByVal _Name As String, ByVal _Reason As String)
            Name = _Name
            Reason = _Reason
            Exists = True
        End Sub
        Public Shared Widening Operator CType(ByVal Value As String) As UserBan
            Return New UserBan(Value)
        End Operator
        Public Shared Widening Operator CType(ByVal b As UserBan) As String
            Return b.ToString
        End Operator
        Public Overrides Function ToString() As String
            Return $"{Name}|{Reason}"
        End Function
        Friend Function Info() As String
            If Not Reason.IsEmptyString Then
                Return $"[{Name}] ({Reason})"
            Else
                Return Name
            End If
        End Function
        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            If Not IsNothing(Obj) Then
                If TypeOf Obj Is UserBan Then
                    Return Name = DirectCast(Obj, UserBan).Name
                Else
                    Return Name = New UserBan(CStr(Obj)).Name
                End If
            End If
            Return False
        End Function
    End Structure
    Friend Function UserBanned(ByVal UserNames() As String) As String()
        If UserNames.ListExists Then
            Dim i%
            Dim Found As New List(Of UserBan)
            For Each user In UserNames
                i = Settings.BlackList.FindIndex(Function(u) u.Name = user)
                If i >= 0 Then Found.Add(Settings.BlackList(i))
            Next
            If Found.Count = 0 Then
                Return New String() {}
            Else
                Dim m As New MMessage With {
                    .Title = "Banned user found",
                    .Buttons = {"Remove from ban and add", "Leave in ban and add", "Skip"},
                    .Style = MsgBoxStyle.Exclamation,
                    .Exists = True
                }
                If Found.Count = 1 Then
                    m.Text = $"This user is banned:{vbNewLine}User: {Found(0).Name}"
                    If Not Found(0).Reason.IsEmptyString Then m.Text.StringAppendLine($"Reason: {Found(0).Reason}")
                Else
                    m.Text = $"These users was banned:{vbNewLine.StringDup(2)}{Found.Select(Function(u) u.Info).ListToString(, vbNewLine)}"
                End If
                Dim r% = MsgBoxE(m)
                If r = 2 Then
                    Return Found.Select(Function(u) u.Name).ToArray
                Else
                    If r = 0 Then
                        Settings.BlackList.ListDisposeRemove(Found, False)
                        Settings.UpdateBlackList()
                    End If
                End If
            End If
        End If
        Return New String() {}
    End Function
    Friend Function UserBanned(ByVal UserName As String) As Boolean
        Return UserBanned({UserName}).ListExists
    End Function
#End Region
End Module