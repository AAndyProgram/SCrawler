' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.WEB
Imports PersonalUtilities.Forms.Toolbars
Imports SCrawler.API
Imports SCrawler.API.Base
Imports SCrawler.Plugin.Hosts
Imports SCrawler.DownloadObjects
Imports DownOptions = SCrawler.Plugin.ISiteSettings.Download
Friend Module MainMod
    Friend Settings As SettingsCLS
    Friend Const SettingsFolderName As String = "Settings"
    Friend ReadOnly LinkPattern As RParams = RParams.DMS("[htps:]{0,6}[/]{0,2}(.+)", 1)
    Friend ReadOnly FilesPattern As RParams = RParams.DM("[^\./]+?\.\w+", 1, EDP.ReturnValue)
    Friend Const LVI_TempOption As String = "Temp"
    Friend Const LVI_FavOption As String = "Favorite"
    Friend Const CannelsLabelName As String = "Channels"
    Friend Const LVI_CollectionOption As String = "Collection"
    Friend Const LVI_ChannelOption As String = "Channel"
    Friend Property BATCH As BatchExecutor
    Private _BatchLogSent As Boolean = False
    ''' <param name="e"><see cref="EDP.None"/></param>
    Friend Sub GlobalOpenPath(ByVal f As SFile, Optional ByVal e As ErrorsDescriber = Nothing)
        Dim b As Boolean = False
        If Not e.Exists Then e = EDP.None
        Try
            If f.Exists(SFO.Path, False) Then
                If Settings.OpenFolderInOtherProgram.Attribute.Value AndAlso Not Settings.OpenFolderInOtherProgram.IsEmptyString Then
                    If BATCH Is Nothing Then BATCH = New BatchExecutor With {.RedirectStandardError = True}
                    b = True
                    With BATCH
                        .Reset()
                        .Execute({String.Format(Settings.OpenFolderInOtherProgram.Value, f.PathWithSeparator)}, EDP.SendInLog + EDP.ThrowException)
                        If .HasError Or Not .ErrorOutput.IsEmptyString Then Throw New Exception(.ErrorOutput, .ErrorException)
                    End With
                Else
                    f.Open(SFO.Path, e)
                End If
            End If
        Catch ex As Exception
            If b Then
                If Not _BatchLogSent Then ErrorsDescriber.Execute(EDP.SendInLog, ex, $"GlobalOpenPath({f.Path})") : _BatchLogSent = True
                f.Open(SFO.Path, e)
            End If
        End Try
    End Sub
    Friend Enum ViewModes As Integer
        IconLarge = View.LargeIcon
        IconSmall = View.SmallIcon
        List = View.Tile
        Details = View.Details
    End Enum
    Friend Enum ShowingModes As Integer
        All = 0
        Regular = 20
        Temporary = 50
        Favorite = 100
        Labels = 500
        NoLabels = 1000
        Deleted = 10000
        Suspended = 12000
    End Enum
    Friend Downloader As TDownloader
    Friend InfoForm As DownloadedInfoForm
    Friend VideoDownloader As VideosDownloaderForm
    Friend UserListLoader As ListImagesLoader
    Friend MyProgressForm As ActiveDownloadingProgress
    Friend MainFrameObj As MainFrameObjects
    Friend ReadOnly ParsersDataDateProvider As New ADateTime(ADateTime.Formats.BaseDateTime)
    Friend ReadOnly LogConnector As New LogHost
#Region "File name operations"
    Friend FileDateAppenderProvider As IFormatProvider
    ''' <summary>File, Date</summary>
    Friend FileDateAppenderPattern As String
    Friend Class NumberedFile : Inherits SFileNumbers
        Friend Sub New(ByVal f As SFile)
            FileName = f.Name
            NumberProvider = New ANumbers With {.Format = ANumbers.Formats.NumberGroup, .GroupSize = 5}
        End Sub
    End Class
#End Region
    Friend Property MainProgress As MyProgress
    Friend Function GetLviGroupName(ByVal Host As SettingsHost, ByVal IsCollection As Boolean, ByVal IsChannel As Boolean) As ListViewGroup()
        Dim l As New List(Of ListViewGroup)
        Dim t$
        t = GetLviGroupName(Host, False, True, IsCollection, IsChannel)
        l.Add(New ListViewGroup(t, t))
        t = GetLviGroupName(Host, False, False, IsCollection, IsChannel)
        l.Add(New ListViewGroup(t, t))
        t = GetLviGroupName(Host, True, False, IsCollection, IsChannel)
        l.Add(New ListViewGroup(t, t))
        Return l.ToArray
    End Function
    Friend Function GetLviGroupName(ByVal Host As SettingsHost, ByVal Temp As Boolean, ByVal Fav As Boolean,
                                    ByVal IsCollection As Boolean, ByVal IsChannel As Boolean) As String
        Dim Opt$ = String.Empty
        If Temp Then
            Opt = LVI_TempOption
        ElseIf Fav Then
            Opt = LVI_FavOption
        End If
        If Not Opt.IsEmptyString Then Opt = $" ({Opt})"
        If IsCollection Then
            Return $"{LVI_CollectionOption}{Opt}"
        ElseIf IsChannel Then
            Return $"{LVI_ChannelOption}{Opt}"
        Else
            Return $"{If(Host?.Name, String.Empty)}{Opt}"
        End If
    End Function
    Friend Structure UserInfo : Implements IComparable(Of UserInfo), IEquatable(Of UserInfo), ICloneable, IEContainerProvider
        Friend Const Name_Site As String = "Site"
        Friend Const Name_Plugin As String = "Plugin"
        Friend Const Name_Collection As String = "Collection"
        Friend Const Name_Merged As String = "Merged"
        Friend Const Name_IsChannel As String = "IsChannel"
        Friend Const Name_SpecialPath As String = "SpecialPath"
        Friend Name As String
        Friend Site As String
        Friend Plugin As String
        Friend File As SFile
        Friend SpecialPath As SFile
        Friend Merged As Boolean
        Friend IncludedInCollection As Boolean
        Friend CollectionName As String
        Friend IsChannel As Boolean
        Friend [Protected] As Boolean
        Friend ReadOnly Property DownloadOption As DownOptions
            Get
                If IsChannel Then
                    Return DownOptions.Channel
                Else
                    Return DownOptions.Main
                End If
            End Get
        End Property
        Friend Sub New(ByVal _Name As String, ByVal Host As SettingsHost, Optional ByVal Collection As String = Nothing,
                       Optional ByVal _Merged As Boolean = False, Optional ByVal _SpecialPath As SFile = Nothing)
            Name = _Name
            Site = Host.Name
            Plugin = Host.Key
            IncludedInCollection = Not Collection.IsEmptyString
            CollectionName = Collection
            Merged = _Merged
            SpecialPath = _SpecialPath
            UpdateUserFile()
        End Sub
        Private Sub New(ByVal x As EContainer)
            Name = x.Value
            Site = x.Attribute(Name_Site).Value
            Plugin = x.Attribute(Name_Plugin).Value
            CollectionName = x.Attribute(Name_Collection).Value
            IncludedInCollection = Not CollectionName.IsEmptyString
            Merged = x.Attribute(Name_Merged).Value.FromXML(Of Boolean)(False)
            SpecialPath = SFile.GetPath(x.Attribute(Name_SpecialPath).Value)
            IsChannel = x.Attribute(Name_IsChannel).Value.FromXML(Of Boolean)(False)
            'UpdateUserFile()
        End Sub
        Friend Sub New(ByVal c As Reddit.Channel)
            Name = c.Name
            Site = Reddit.RedditSite
            Plugin = Reddit.RedditSiteKey
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
            If [Protected] Then Return String.Empty
            If Not SpecialPath.IsEmptyString Then
                Return $"{SpecialPath.PathWithSeparator}{SettingsFolderName}"
            ElseIf Merged And IncludedInCollection Then
                Return $"{Settings.CollectionsPathF.PathNoSeparator}\{CollectionName}\{SettingsFolderName}"
            Else
                If IncludedInCollection Then
                    Return $"{Settings.CollectionsPathF.PathNoSeparator}\{CollectionName}\{Site}_{Name}\{SettingsFolderName}"
                ElseIf Not Settings(Plugin) Is Nothing Then
                    Return $"{Settings(Plugin).Path.PathNoSeparator}\{Name}\{SettingsFolderName}"
                Else
                    Dim s$ = Site.ToLower
                    Dim i% = Settings.Plugins.FindIndex(Function(p) p.Name.ToLower = s)
                    If i >= 0 Then Return $"{Settings.Plugins(i).Settings.Path.PathNoSeparator}\{Name}\{SettingsFolderName}" Else Return String.Empty
                End If
            End If
        End Function
        Friend Function GetContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
            Return New EContainer("User", Name, {New EAttribute(Name_Site, Site),
                                                 New EAttribute(Name_Plugin, Plugin),
                                                 New EAttribute(Name_Collection, CollectionName),
                                                 New EAttribute(Name_Merged, Merged.BoolToInteger),
                                                 New EAttribute(Name_IsChannel, IsChannel.BoolToInteger),
                                                 New EAttribute(Name_SpecialPath, SpecialPath.PathWithSeparator)})
        End Function
        Friend Function CompareTo(ByVal Other As UserInfo) As Integer Implements IComparable(Of UserInfo).CompareTo
            If Site = Other.Site Then
                Return Name.CompareTo(Other.Name)
            Else
                Return Site.CompareTo(Other.Site)
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
                .Plugin = Plugin,
                .File = File,
                .SpecialPath = SpecialPath,
                .Merged = Merged,
                .IncludedInCollection = IncludedInCollection,
                .CollectionName = CollectionName,
                .IsChannel = IsChannel,
                .[Protected] = [Protected]
            }
        End Function
    End Structure
#Region "Image Handlers management"
    Friend Sub ImageHandler(ByVal User As IUserData)
        ImageHandler(User, False)
        ImageHandler(User, True)
    End Sub
    Friend Sub ImageHandler(ByVal User As IUserData, ByVal Add As Boolean)
        MainFrameObj.ImageHandler(User, Add)
    End Sub
    Friend Sub CollectionHandler(ByVal [Collection] As UserDataBind)
        MainFrameObj.CollectionHandler(Collection)
    End Sub
#End Region
#Region "Standalone video download functions"
    Friend Function GetCurrentBuffer() As String
        Dim b$ = BufferText
        If Not (Not b.IsEmptyString AndAlso b.Length > 4 AndAlso b.StartsWith("http")) Then b = String.Empty
        Return b
    End Function
    Friend Function GetNewVideoURL() As String
        Dim URL$ = InputBoxE("Enter video URL:", "Download video by URL", GetCurrentBuffer())
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
            Dim Result As Boolean = False
            If Not URL.IsEmptyString Then
                Dim um As IEnumerable(Of UserMedia) = Nothing
                Dim found As Boolean = False
                Dim d As Plugin.ExchangeOptions
                If Settings.Plugins.Count > 0 Then
                    For Each p As PluginHost In Settings.Plugins
                        d = p.Settings.IsMyImageVideo(URL)
                        If d.Exists Then
                            um = Settings(d.HostKey).GetSpecialData(URL, Settings.LatestSavingPath.Value, AskForPath)
                            found = True
                            If um.ListExists Then
                                If AskForPath And Not um(0).SpecialFolder.IsEmptyString And Not p.Settings.IsMyClass Then _
                                   Settings.LatestSavingPath.Value = um(0).SpecialFolder
                                If um(0).State = UserMedia.States.Downloaded Then Return True
                            End If
                            Exit For
                        End If
                    Next
                End If
                If Not found Then
                    If URL.Contains("gfycat") Then
                        um = Gfycat.Envir.GetVideoInfo(URL)
                    ElseIf URL.Contains("imgur.com") Then
                        um = Imgur.Envir.GetVideoInfo(URL)
                    Else
                        MsgBoxE("Site of video URL does not recognized" & vbCr & "Operation canceled", MsgBoxStyle.Exclamation, e)
                        Return False
                    End If
                End If

                If um.ListExists Then
                    Dim f As SFile, ff As SFile
                    Dim dURL$
                    Dim FileDownloaded As Boolean = False

                    For Each u As UserMedia In um
                        If Not u.URL.IsEmptyString Or Not u.URL_BASE.IsEmptyString Then
                            f = u.File
                            If f.Name.IsEmptyString Then f.Name = $"video_{u.Post.ID}"
                            If f.Extension.IsEmptyString Then f.Extension = "mp4"
                            If Not Settings.LatestSavingPath.IsEmptyString And
                                   Settings.LatestSavingPath.Value.Exists(SFO.Path, False) Then f.Path = Settings.LatestSavingPath.Value
                            If AskForPath OrElse Not f.Exists(SFO.Path, False) Then
#Disable Warning BC40000
                                ff = SFile.SaveAs(f, "Files destination",,,, EDP.ReturnValue)
                                If Not ff.IsEmptyString Then
                                    f.Path = ff.Path
                                Else
                                    f = Nothing
                                End If
#Enable Warning
                                AskForPath = False
                            End If
                            If Not f.IsEmptyString Then
                                Settings.LatestSavingPath.Value = f.PathWithSeparator
                                FileDownloaded = False
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
                                    If um.Count = 1 Then
                                        MsgBoxE($"File downloaded to [{f}]",, e)
                                        Return True
                                    Else
                                        Result = True
                                    End If
                                Else
                                    If um.Count = 1 Then MsgBoxE("File does not downloaded", MsgBoxStyle.Critical, e)
                                End If
                            Else
                                If um.Count = 1 Then MsgBoxE("File destination does not pointed" & vbCr & "Operation canceled",, e)
                            End If
                        Else
                            If um.Count = 1 Then MsgBoxE("File URL does not found!", MsgBoxStyle.Critical, e)
                        End If
                    Next
                End If
            Else
                MsgBoxE("URL is empty", MsgBoxStyle.Exclamation, e)
            End If
            Return Result
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
    Friend Sub CheckVersion(ByVal Force As Boolean)
        If Settings.CheckUpdatesAtStart Or Force Then _
            GitHub.DefaultVersionChecker(My.Application.Info.Version, "AAndyProgram", "SCrawler",
                                         Settings.LatestVersion.Value, Settings.ShowNewVersionNotification.Value, Force)
    End Sub
End Module