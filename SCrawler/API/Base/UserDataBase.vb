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
Imports PersonalUtilities.Forms.Toolbars
Imports System.IO
Imports System.Net
Imports System.Threading
Imports UStates = SCrawler.API.Base.UserMedia.States
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace API.Base
    Friend MustInherit Class UserDataBase : Implements IUserData
        Friend Const UserFileAppender As String = "User"
        Friend Event OnUserUpdated As IUserData.OnUserUpdatedEventHandler Implements IUserData.OnUserUpdated
        Protected Sub Raise_OnUserUpdated()
            RaiseEvent OnUserUpdated(Me)
        End Sub
#Region "Collection buttons"
        Friend WithEvents BTT_CONTEXT_DOWN As ToolStripMenuItem
        Friend WithEvents BTT_CONTEXT_EDIT As ToolStripMenuItem
        Friend WithEvents BTT_CONTEXT_DELETE As ToolStripMenuItem
        Friend WithEvents BTT_CONTEXT_OPEN_PATH As ToolStripMenuItem
        Friend WithEvents BTT_CONTEXT_OPEN_SITE As ToolStripMenuItem
        Friend Sub CreateButtons(ByVal CollectionIndex As Integer)
            Dim tn$ = $"[{Site}] - {Name}"
            Dim _tn$ = $"{Site}{Name}"
            Dim tnn As Func(Of String, String) = Function(Input) $"{Input}{_tn}"
            Dim i As Image = Nothing
            Select Case Site
                Case Sites.Reddit : i = My.Resources.RedditIcon.ToBitmap
                Case Sites.Twitter : i = My.Resources.TwitterIcon.ToBitmap
            End Select
            BTT_CONTEXT_DOWN = New ToolStripMenuItem(tn, i) With {.Name = tnn("DOWN"), .Tag = CollectionIndex}
            BTT_CONTEXT_EDIT = New ToolStripMenuItem(tn, i) With {.Name = tnn("EDIT"), .Tag = CollectionIndex}
            BTT_CONTEXT_DELETE = New ToolStripMenuItem(tn, i) With {.Name = tnn("DELETE"), .Tag = CollectionIndex}
            BTT_CONTEXT_OPEN_PATH = New ToolStripMenuItem(tn, i) With {.Name = tnn("PATH"), .Tag = CollectionIndex}
            BTT_CONTEXT_OPEN_SITE = New ToolStripMenuItem(tn, i) With {.Name = tnn("SITE"), .Tag = CollectionIndex}
        End Sub
#End Region
#Region "XML Declarations"
        Private Const Name_Site As String = "Site"
        Private Const Name_IsChannel As String = "IsChannel"
        Private Const Name_UserName As String = "UserName"
        Private Const Name_UserExists As String = "UserExists"
        Private Const Name_UserSuspended As String = "UserSuspended"
        Private Const Name_FriendlyName As String = "FriendlyName"
        Private Const Name_UserID As String = "UserID"
        Private Const Name_Description As String = "Description"
        Private Const Name_ParseUserMediaOnly As String = "ParseUserMediaOnly"
        Private Const Name_Temporary As String = "Temporary"
        Private Const Name_Favorite As String = "Favorite"
        Private Const Name_CreatedByChannel As String = "CreatedByChannel"

        Private Const Name_SeparateVideoFolder As String = "SeparateVideoFolder"
        Private Const Name_CollectionName As String = "Collection"
        Private Const Name_LabelsName As String = "Labels"

        Private Const Name_ReadyForDownload As String = "ReadyForDownload"
        Private Const Name_DownloadImages As String = "DownloadImages"
        Private Const Name_DownloadVideos As String = "DownloadVideos"

        Private Const Name_VideoCount As String = "VideoCount"
        Private Const Name_PicturesCount As String = "PicturesCount"
        Private Const Name_LastUpdated As String = "LastUpdated"

        Private Const Name_DataMerging As String = "DataMerging"
#Region "Downloaded data"
        Private Const Name_MediaType As String = "Type"
        Private Const Name_MediaURL As String = "URL"
        Private Const Name_MediaHash As String = "Hash"
        Private Const Name_MediaFile As String = "File"
        Private Const Name_MediaPostID As String = "ID"
        Private Const Name_MediaPostDate As String = "Date"
#End Region
#End Region
#Region "Declarations"
        Friend MustOverride Property Site As Sites Implements IContentProvider.Site
        Protected _Progress As MyProgress
        Friend Overridable Property Progress As MyProgress
            Get
                If _Progress Is Nothing Then Return MainProgress Else Return _Progress
            End Get
            Set(ByVal p As MyProgress)
                _Progress = p
            End Set
        End Property
        Friend User As UserInfo
        Friend Property IsSavedPosts As Boolean
        Friend Overridable Property UserExists As Boolean = True Implements IUserData.Exists
        Friend Overridable Property UserSuspended As Boolean = False Implements IUserData.Suspended
        Friend Overridable Property Name As String Implements IContentProvider.Name
            Get
                Return User.Name
            End Get
            Set(ByVal NewName As String)
                User.Name = NewName
                User.UpdateUserFile()
                Settings.UpdateUsersList(User)
            End Set
        End Property
        Friend Overridable Property ID As String = String.Empty Implements IContentProvider.ID
        Friend Overridable Property FriendlyName As String = String.Empty Implements IContentProvider.FriendlyName
        Friend Property UserDescription As String = String.Empty Implements IContentProvider.Description
        Friend Property ParseUserMediaOnly As Boolean = False Implements IUserData.ParseUserMediaOnly
        Protected _Favorite As Boolean = False
        Friend Overridable Property Favorite As Boolean Implements IContentProvider.Favorite
            Get
                Return _Favorite
            End Get
            Set(ByVal Fav As Boolean)
                _Favorite = Fav
                If _Favorite Then _Temporary = False
            End Set
        End Property
        Protected _Temporary As Boolean = False
        Friend Overridable Property Temporary As Boolean Implements IContentProvider.Temporary
            Get
                Return _Temporary
            End Get
            Set(ByVal Temp As Boolean)
                _Temporary = Temp
                If _Temporary Then _Favorite = False
            End Set
        End Property
        Friend Overridable ReadOnly Property IsChannel As Boolean Implements IUserData.IsChannel
            Get
                Return User.IsChannel
            End Get
        End Property
        Friend Property CreatedByChannel As Boolean = False
        Friend ReadOnly Property Self As IUserData Implements IUserData.Self
            Get
                Return Me
            End Get
        End Property
#Region "Images"
        Friend Overridable Function GetUserPicture() As Image Implements IUserData.GetPicture
            If Settings.ViewModeIsPicture Then
                Return GetPicture()
            Else
                Return Nothing
            End If
        End Function
        Friend Overridable Sub SetPicture(ByVal f As SFile) Implements IUserData.SetPicture
            Try
                If Not f.IsEmptyString AndAlso f.Exists Then
                    Using p As New UserImage(f, User.File) : p.Save() : End Using
                End If
            Catch ex As Exception
            End Try
        End Sub
        Protected Function GetNullPicture(ByVal MaxHeigh As XML.Base.XMLValue(Of Integer)) As Bitmap
            Return New Bitmap(CInt(DivideWithZeroChecking(MaxHeigh.Value, 100) * 75), MaxHeigh.Value)
        End Function
        Protected Function GetPicture(Optional ByVal ReturnNullImageOnNothing As Boolean = True) As Image
            Dim f As SFile = Nothing
            Dim p As UserImage = Nothing
            Dim DelPath As Boolean = True
BlockPictureFolder:
            On Error GoTo BlockPictureScan
            f = SFile.GetPath($"{MyFile.PathWithSeparator}Pictures")
            If f.Exists(SFO.Path, False) Then
                Dim PicList As List(Of SFile) = SFile.GetFiles(f, $"{UserImage.ImagePrefix}*.jpg")
                If PicList.ListExists Then
                    PicList.Sort()
                    Dim l As SFile, s As SFile
                    l = PicList.Find(Function(ff) ff.Name.Contains(UserImage.ImagePostfix_Large))
                    If Not l.IsEmptyString Then PicList.Remove(l)
                    s = PicList.Find(Function(ff) ff.Name.Contains(UserImage.ImagePostfix_Small))
                    If Not s.IsEmptyString Then PicList.Remove(s)
                    If PicList.Count > 0 Then
                        p = New UserImage(PicList.First, l, s, MyFile)
                        GoTo BlockReturn
                    Else
                        f.Delete(SFO.Path, False, False, EDP.None)
                        DelPath = False
                    End If
                End If
            End If
BlockPictureScan:
            On Error GoTo BlockDeletePictureFolder
            Dim NewPicFile As SFile = SFile.GetFiles(MyFile.CutPath, "*.jpg|*.png",,
                                                     New ErrorsDescriber(EDP.ReturnValue) With {
                                                         .ReturnValue = New List(Of SFile),
                                                         .ReturnValueExists = True}).FirstOrDefault
            If Not NewPicFile.IsEmptyString AndAlso NewPicFile.Exists Then
                p = New UserImage(NewPicFile, MyFile)
                p.Save()
                GoTo BlockReturn
            End If
BlockDeletePictureFolder:
            On Error GoTo BlockReturn
            If DelPath Then
                f = SFile.GetPath($"{MyFile.PathWithSeparator}Pictures")
                If f.Exists(SFO.Path, False) Then f.Delete(SFO.Path, False, False)
            End If
BlockReturn:
            On Error GoTo BlockNullPicture
            If Not p Is Nothing Then
                Dim i As Image = Nothing
                Select Case Settings.ViewMode.Value
                    Case View.LargeIcon : i = p.Large.OriginalImage.Clone
                    Case View.SmallIcon : i = p.Small.OriginalImage.Clone
                End Select
                p.Dispose()
                Return i
            End If
BlockNullPicture:
            If ReturnNullImageOnNothing Then
                Select Case Settings.ViewMode.Value
                    Case View.LargeIcon : Return GetNullPicture(Settings.MaxLargeImageHeigh)
                    Case View.SmallIcon : Return GetNullPicture(Settings.MaxSmallImageHeigh)
                End Select
            End If
            Return Nothing
        End Function
#End Region
#Region "Separate folder"
        Friend Property SeparateVideoFolder As Boolean?
        Protected ReadOnly Property SeparateVideoFolderF As Boolean
            Get
                Return (SeparateVideoFolder.HasValue AndAlso SeparateVideoFolder.Value) OrElse Settings.SeparateVideoFolder.Value
            End Get
        End Property
#End Region
#Region "Collections support"
        Protected _IsCollection As Boolean = False
        Protected Friend ReadOnly Property IsCollection As Boolean Implements IUserData.IsCollection
            Get
                Return _IsCollection
            End Get
        End Property
        Friend Overridable Property CollectionName As String Implements IUserData.CollectionName
            Get
                Return User.CollectionName
            End Get
            Set(ByVal NewCollection As String)
                ChangeCollectionName(NewCollection, True)
            End Set
        End Property
        Friend ReadOnly Property IncludedInCollection As Boolean Implements IUserData.IncludedInCollection
            Get
                Return User.IncludedInCollection
            End Get
        End Property
        Friend Overridable Sub ChangeCollectionName(ByVal NewName As String, ByVal UpdateSettings As Boolean)
            Dim u As UserInfo = User
            u.CollectionName = NewName
            u.IncludedInCollection = Not NewName.IsEmptyString
            User = u
            If UpdateSettings Then Settings.UpdateUsersList(User)
        End Sub
        Friend Overridable ReadOnly Property Labels As List(Of String) Implements IUserData.Labels
#End Region
#Region "Downloading params"
        Protected _DataLoaded As Boolean = False
        Protected _DataParsed As Boolean = False
        Friend Overridable Property ReadyForDownload As Boolean = True Implements IUserData.ReadyForDownload
        Friend Property DownloadImages As Boolean = True Implements IUserData.DownloadImages
        Friend Property DownloadVideos As Boolean = True Implements IUserData.DownloadVideos
#End Region
#Region "Content"
        Protected ReadOnly _ContentList As List(Of UserMedia)
        Protected ReadOnly _ContentNew As List(Of UserMedia)
        Protected ReadOnly _TempMediaList As List(Of UserMedia)
        Protected ReadOnly _TempPostsList As List(Of String)
#End Region
#Region "Files"
        Friend Overridable Property MyFile As SFile Implements IUserData.File
            Get
                Return User.File
            End Get
            Set(ByVal f As SFile)
                User.File = f
                Settings.UpdateUsersList(User)
            End Set
        End Property
        Protected MyFileData As SFile
        Protected MyFilePosts As SFile
        Friend Overridable Property FileExists As Boolean = False Implements IUserData.FileExists
        Friend Overridable Property DataMerging As Boolean
            Get
                Return User.Merged
            End Get
            Set(ByVal IsMerged As Boolean)
                If Not User.Merged = IsMerged Then
                    User.Merged = IsMerged
                    User.UpdateUserFile()
                    Settings.UpdateUsersList(User)
                End If
            End Set
        End Property
#End Region
#Region "Information"
        Protected _CountVideo As Integer = 0
        Protected Property _CountPictures As Integer = 0
        Friend Overridable Property LastUpdated As Date?
        Friend ReadOnly Property TotalContentCount As Integer
            Get
                Return _CountVideo + _CountPictures
            End Get
        End Property
        Friend Overridable Property HasError As Boolean = False Implements IUserData.HasError
        Private _DownloadedPicturesTotal As Integer = 0
        Private _DownloadedPicturesSession As Integer = 0
        Friend Property DownloadedPictures As Integer Implements IUserData.DownloadedPictures
            Get
                Return _DownloadedPicturesSession
            End Get
            Set(ByVal NewValue As Integer)
                _DownloadedPicturesSession = NewValue
            End Set
        End Property
        Private _DownloadedVideosTotal As Integer = 0
        Private _DownloadedVideosSession As Integer = 0
        Friend Property DownloadedVideos As Integer Implements IUserData.DownloadedVideos
            Get
                Return _DownloadedVideosSession
            End Get
            Set(ByVal NewValue As Integer)
                _DownloadedVideosSession = NewValue
            End Set
        End Property
        Friend Overridable ReadOnly Property DownloadedTotal(Optional ByVal Total As Boolean = True) As Integer Implements IUserData.DownloadedTotal
            Get
                If Total Then
                    Return _DownloadedPicturesTotal + _DownloadedVideosTotal
                Else
                    Return _DownloadedPicturesSession + _DownloadedVideosSession
                End If
            End Get
        End Property
        Friend ReadOnly Property DownloadedInformation As String Implements IUserData.DownloadedInformation
            Get
                Dim luv$ = String.Empty
                If LastUpdated.HasValue Then luv = $"{LastUpdated.Value.ToStringDate(ADateTime.Formats.BaseDateTime)}: "
                Return $"{luv}{Name} [{Site}]{IIf(HasError, " (with errors)", String.Empty)}: P - {_DownloadedPicturesTotal}; V - {_DownloadedVideosTotal}" &
                       $" (P - {_CountPictures}; V - {_CountVideo})"
            End Get
        End Property
#End Region
#End Region
#Region "LVI"
        Friend ReadOnly Property LVIKey As String Implements IUserData.LVIKey
            Get
                If Not _IsCollection Then
                    Return $"{Site.ToString.ToUpper}_{Name}"
                Else
                    Return $"CCCC_{CollectionName}"
                End If
            End Get
        End Property
        Private _LVIIndex As Integer = -1
        Private ReadOnly Property LVIIndex As Integer Implements IUserData.LVIIndex
            Get
                Return _LVIIndex
            End Get
        End Property
        Friend Function GetLVI(ByVal Destination As ListView) As ListViewItem Implements IUserData.GetLVI
            _LVIIndex = Destination.Items.Count
            If Settings.ViewModeIsPicture Then
                Return New ListViewItem(ToString(), LVIKey, GetLVIGroup(Destination)) With {.Name = LVIKey, .Tag = LVIKey}
            Else
                Return New ListViewItem(ToString(), GetLVIGroup(Destination)) With {.Name = LVIKey, .Tag = LVIKey}
            End If
        End Function
        Friend Overridable ReadOnly Property FitToAddParams As Boolean Implements IUserData.FitToAddParams
            Get
                If Settings.SelectedSites.Count = 0 OrElse Settings.SelectedSites.Contains(Site) Then
                    Select Case Settings.ShowingMode.Value
                        Case ShowingModes.Regular : Return Not Temporary And Not Favorite
                        Case ShowingModes.Temporary : Return Temporary
                        Case ShowingModes.Favorite : Return Favorite
                        Case ShowingModes.Labels : Return Settings.Labels.CurrentSelection.ListContains(Labels)
                        Case ShowingModes.NoLabels : Return Labels.Count = 0
                        Case Else : Return True
                    End Select
                Else
                    Return False
                End If
            End Get
        End Property
        Friend Function GetLVIGroup(ByVal Destination As ListView) As ListViewGroup Implements IUserData.GetLVIGroup
            Try
                If Settings.ShowingMode.Value = ShowingModes.Labels Then
                    If Labels.Count > 0 And Settings.Labels.CurrentSelection.Count > 0 Then
                        For i% = 0 To Labels.Count - 1
                            If Settings.Labels.CurrentSelection.Contains(Labels(i)) Then Return Destination.Groups.Item(Labels(i))
                        Next
                        Return Destination.Groups.Item(LabelsKeeper.NoLabeledName)
                    Else
                        Return Destination.Groups.Item(LabelsKeeper.NoLabeledName)
                    End If
                Else
                    Return Destination.Groups.Item(GetLviGroupName(Site, Temporary, Favorite, IsCollection, IsChannel))
                End If
            Catch ex As Exception
                Return Destination.Groups.Item(LabelsKeeper.NoLabeledName)
            End Try
        End Function
        Friend Overridable Function GetUserInformation() As String
            Dim OutStr$ = $"User: {Name}"
            OutStr.StringAppendLine($"Path: {MyFile.CutPath.Path}")
            OutStr.StringAppendLine($"Total downloaded ({TotalContentCount.NumToString(ANumbers.Formats.Number, 3)}):")
            OutStr.StringAppendLine($"Pictures: {_CountPictures.NumToString(ANumbers.Formats.Number, 3)}")
            OutStr.StringAppendLine($"Videos: {_CountVideo.NumToString(ANumbers.Formats.Number, 3)}")
            If Not UserDescription.IsEmptyString Then
                OutStr.StringAppendLine(String.Empty)
                OutStr.StringAppendLine(UserDescription)
            End If
            OutStr.StringAppendLine(String.Empty)
            OutStr.StringAppendLine($"Last updated at: {AConvert(Of String)(LastUpdated, ADateTime.Formats.BaseDateTime, "not yet")}")
            If _DataParsed Then
                OutStr.StringAppendLine("Downloaded now:")
                OutStr.StringAppendLine($"Pictures: {_CountPictures.NumToString(ANumbers.Formats.Number, 3)}")
                OutStr.StringAppendLine($"Videos: {_CountVideo.NumToString(ANumbers.Formats.Number, 3)}")
            End If
            Return OutStr
        End Function
#End Region
#Region "Initializer"
        Private ReadOnly _InvokeImageHandler As Boolean
        ''' <summary>By using this constructor you must set UserName and MyFile manually</summary>
        Friend Sub New(Optional ByVal InvokeImageHandler As Boolean = True)
            _InvokeImageHandler = InvokeImageHandler
            _ContentList = New List(Of UserMedia)
            _ContentNew = New List(Of UserMedia)
            _TempMediaList = New List(Of UserMedia)
            _TempPostsList = New List(Of String)
            Labels = New List(Of String)
            If InvokeImageHandler Then ImageHandler(Me)
        End Sub
        ''' <exception cref="ArgumentOutOfRangeException"></exception>
        Friend Overloads Shared Function GetInstance(ByVal u As UserInfo, Optional ByVal _LoadUserInformation As Boolean = True) As IUserData
            Select Case u.Site
                Case Sites.Reddit
                    If u.IsChannel Then
                        Return New Reddit.Channel(u, _LoadUserInformation)
                    Else
                        Return New Reddit.UserData(u, _LoadUserInformation)
                    End If
                Case Sites.Twitter : Return New Twitter.UserData(u, _LoadUserInformation)
                Case Sites.Instagram : Return New Instagram.UserData(u, _LoadUserInformation)
                Case Sites.RedGifs : Return New RedGifs.UserData(u, _LoadUserInformation)
                Case Else : Throw New ArgumentOutOfRangeException("Site", $"Site [{u.Site}] information does not recognized by loader")
            End Select
        End Function
#End Region
#Region "Information & Content data files loader and saver"
#Region "User information"
        Friend Overridable Sub LoadUserInformation() Implements IUserData.LoadUserInformation
            Try
                If MyFile.Exists Then
                    FileExists = True
                    Using x As New XmlFile(MyFile) With {.XmlReadOnly = True}
                        User.Site = Site
                        Site = x.Value(Name_Site).FromXML(Of Integer)(0)
                        User.Name = x.Value(Name_UserName)
                        UserExists = x.Value(Name_UserExists).FromXML(Of Boolean)(True)
                        UserSuspended = x.Value(Name_UserSuspended).FromXML(Of Boolean)(False)
                        ID = x.Value(Name_UserID)
                        FriendlyName = x.Value(Name_FriendlyName)
                        UserDescription = x.Value(Name_Description)
                        ParseUserMediaOnly = x.Value(Name_ParseUserMediaOnly).FromXML(Of Boolean)(False)
                        Temporary = x.Value(Name_Temporary).FromXML(Of Boolean)(False)
                        Favorite = x.Value(Name_Favorite).FromXML(Of Boolean)(False)
                        CreatedByChannel = x.Value(Name_CreatedByChannel).FromXML(Of Boolean)(False)
                        SeparateVideoFolder = AConvert(Of Boolean)(x.Value(Name_SeparateVideoFolder), AModes.Var, Nothing)
                        ReadyForDownload = x.Value(Name_ReadyForDownload).FromXML(Of Boolean)(True)
                        DownloadImages = x.Value(Name_DownloadImages).FromXML(Of Boolean)(True)
                        DownloadVideos = x.Value(Name_DownloadVideos).FromXML(Of Boolean)(True)
                        _CountVideo = x.Value(Name_VideoCount).FromXML(Of Integer)(0)
                        _CountPictures = x.Value(Name_PicturesCount).FromXML(Of Integer)(0)
                        LastUpdated = AConvert(Of Date)(x.Value(Name_LastUpdated), ADateTime.Formats.BaseDateTime, Nothing)
                        DataMerging = x.Value(Name_DataMerging).FromXML(Of Boolean)(False)
                        ChangeCollectionName(x.Value(Name_CollectionName), False)
                        Labels.ListAddList(x.Value(Name_LabelsName).StringToList(Of String, List(Of String))("|", EDP.ReturnValue), LAP.NotContainsOnly, LAP.ClearBeforeAdd)
                        LoadUserInformation_OptionalFields(x, True)
                    End Using
                    UpdateDataFiles()
                End If
            Catch ex As Exception
                LogError(ex, "user information loading error")
            End Try
        End Sub
        Friend Overridable Sub UpdateUserInformation() Implements IUserData.UpdateUserInformation
            Try
                MyFile.Exists(SFO.Path)
                Using x As New XmlFile With {.Name = "User"}
                    x.Add(Name_Site, CInt(Site))
                    x.Add(Name_UserName, User.Name)
                    x.Add(Name_UserExists, UserExists.BoolToInteger)
                    x.Add(Name_UserSuspended, UserSuspended.BoolToInteger)
                    x.Add(Name_UserID, ID)
                    x.Add(Name_FriendlyName, FriendlyName)
                    x.Add(Name_Description, UserDescription)
                    x.Add(Name_ParseUserMediaOnly, ParseUserMediaOnly.BoolToInteger)
                    x.Add(Name_Temporary, Temporary.BoolToInteger)
                    x.Add(Name_Favorite, Favorite.BoolToInteger)
                    x.Add(Name_CreatedByChannel, CreatedByChannel.BoolToInteger)
                    If SeparateVideoFolder.HasValue Then
                        x.Add(Name_SeparateVideoFolder, SeparateVideoFolder.Value.BoolToInteger)
                    Else
                        x.Add(Name_SeparateVideoFolder, String.Empty)
                    End If
                    x.Add(Name_ReadyForDownload, ReadyForDownload.BoolToInteger)
                    x.Add(Name_DownloadImages, DownloadImages.BoolToInteger)
                    x.Add(Name_DownloadVideos, DownloadVideos.BoolToInteger)
                    x.Add(Name_VideoCount, _CountVideo)
                    x.Add(Name_PicturesCount, _CountPictures)
                    x.Add(Name_LastUpdated, AConvert(Of String)(LastUpdated, ADateTime.Formats.BaseDateTime, String.Empty))
                    x.Add(Name_CollectionName, CollectionName)
                    x.Add(Name_LabelsName, Labels.ListToString(, "|", EDP.ReturnValue))
                    x.Add(Name_DataMerging, DataMerging.BoolToInteger)

                    LoadUserInformation_OptionalFields(x, False)

                    x.Save(MyFile)
                End Using
                If Not IsSavedPosts Then Settings.UpdateUsersList(User)
            Catch ex As Exception
                LogError(ex, "user information saving error")
            End Try
        End Sub
        ''' <param name="Loading"><see langword="True"/>: Loading; <see langword="False"/>: Saving</param>
        Protected MustOverride Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
#End Region
#Region "User data"
        Friend Overridable Overloads Sub LoadContentInformation()
            Try
                UpdateDataFiles()
                If Not MyFileData.Exists Then Exit Sub
                Using x As New XmlFile(MyFileData, Protector.Modes.All, False) With {.XmlReadOnly = True, .AllowSameNames = True}
                    x.LoadData()
                    If x.Count > 0 Then
                        Dim fs$ = MyFile.CutPath.PathWithSeparator
                        Dim gfn As Func(Of String, String) = Function(ByVal Input As String) As String
                                                                 If Input.IsEmptyString Then
                                                                     Return String.Empty
                                                                 Else
                                                                     If Input.Contains("\") Then
                                                                         Return New SFile(Input).File
                                                                     Else
                                                                         Return Input
                                                                     End If
                                                                 End If
                                                             End Function
                        For Each v As EContainer In x
                            _ContentList.Add(New UserMedia With {
                                           .Type = AConvert(Of Integer)(v.Attribute(Name_MediaType).Value, 0),
                                           .URL = v.Attribute(Name_MediaURL).Value,
                                           .URL_BASE = v.Value,
                                           .MD5 = v.Attribute(Name_MediaHash).Value,
                                           .File = fs & gfn.Invoke(v.Attribute(Name_MediaFile).Value),
                                           .Post = New UserPost With {
                                                       .ID = v.Attribute(Name_MediaPostID).Value,
                                                       .[Date] = AConvert(Of Date)(v.Attribute(Name_MediaPostDate).Value, ParsersDataDateProvider, Nothing)}
                                       })
                        Next
                    End If
                    _DataLoaded = True
                End Using
            Catch ex As Exception
                LogError(ex, "history loading error")
            End Try
        End Sub
        Friend Sub UpdateContentInformation()
            Try
                UpdateDataFiles()
                If MyFileData.IsEmptyString Then Exit Sub
                MyFileData.Exists(SFO.Path)
                Using x As New XmlFile With {.AllowSameNames = True, .Name = "Data"}
                    If _ContentList.Count > 0 Then
                        For Each i As UserMedia In _ContentList
                            x.Add(New EContainer("MediaData", i.URL_BASE,
                                                 {New EAttribute(Name_MediaType, CInt(i.Type)),
                                                  New EAttribute(Name_MediaURL, i.URL),
                                                  New EAttribute(Name_MediaHash, i.MD5),
                                                  New EAttribute(Name_MediaFile, i.File.File),
                                                  New EAttribute(Name_MediaPostID, i.Post.ID),
                                                  New EAttribute(Name_MediaPostDate, AConvert(Of String)(i.Post.Date, ParsersDataDateProvider, String.Empty))
                                                 }))
                        Next
                    End If
                    x.Save(MyFileData)
                End Using
            Catch ex As Exception
                LogError(ex, "history saving error")
            End Try
        End Sub
#End Region
#End Region
#Region "Open site, folder"
        Friend Overridable Sub OpenSite() Implements IContentProvider.OpenSite
            Try
                Dim URL$ = String.Empty
                Select Case Site
                    Case Sites.Reddit : URL = $"https://www.reddit.com/{IIf(IsChannel, "r", "user")}/{Name}/"
                    Case Sites.Twitter : URL = $"https://twitter.com/{Name}"
                    Case Sites.Instagram : URL = $"https://www.instagram.com/{Name}/"
                    Case Sites.RedGifs : URL = $"https://www.redgifs.com/users/{Name}/"
                    Case Else : MsgBoxE($"Site [{Site}] opening not implemented", MsgBoxStyle.Exclamation)
                End Select
                If Not URL.IsEmptyString Then Process.Start(URL)
            Catch ex As Exception
                MsgBoxE($"Error on trying to open [{Site}] page of user [{Name}]", MsgBoxStyle.Critical)
            End Try
        End Sub
        Friend Overridable Sub OpenFolder() Implements IUserData.OpenFolder
            MyFile.CutPath.Open(SFO.Path, EDP.None)
        End Sub
#End Region
#Region "Download functions and options"
        Friend Overridable Property DownloadTopCount As Integer? = Nothing Implements IUserData.DownloadTopCount
        Protected Responser As PersonalUtilities.Tools.WEB.Response
        Friend Overridable Sub DownloadData(ByVal Token As CancellationToken) Implements IContentProvider.DownloadData
            Dim Canceled As Boolean = False
            Try
                UpdateDataFiles()
                If Not Responser Is Nothing Then Responser.Dispose()
                Responser = New PersonalUtilities.Tools.WEB.Response
                Responser.Copy(Settings(Site).Responser)
                Dim UpPic As Boolean = Settings.ViewModeIsPicture AndAlso GetPicture(False) Is Nothing
                Dim sEnvir() As Boolean = {UserExists, UserSuspended}
                Dim EnvirChanged As Func(Of Boolean) = Function() Not sEnvir(0) = UserExists Or Not sEnvir(1) = UserSuspended
                UserExists = True
                UserSuspended = False
                _DownloadedPicturesSession = 0
                _DownloadedVideosSession = 0
                _TempMediaList.Clear()
                _TempPostsList.Clear()
                Dim __SaveData As Boolean = Not CreatedByChannel Or Not Settings.FromChannelDownloadTopUse

                If Not _DataLoaded Then LoadContentInformation()

                If MyFilePosts.Exists Then _TempPostsList.ListAddList(File.ReadAllLines(MyFilePosts))
                If _ContentList.Count > 0 Then _TempPostsList.ListAddList(_ContentList.Select(Function(u) u.Post.ID), LNC)
                ThrowAny(Token)
                DownloadDataF(Token)
                ThrowAny(Token)

                If _TempMediaList.Count > 0 Then
                    If Not DownloadImages Then _TempMediaList.RemoveAll(Function(m) m.Type = UTypes.GIF Or m.Type = UTypes.Picture)
                    If Not DownloadVideos Then _TempMediaList.RemoveAll(Function(m) m.Type = UTypes.Video Or
                                                                                    m.Type = UTypes.VideoPre Or m.Type = UTypes.m3u8)
                End If

                ReparseVideo(Token)
                ThrowAny(Token)
                If _TempPostsList.Count > 0 And __SaveData Then TextSaver.SaveTextToFile(_TempPostsList.ListToString(, Environment.NewLine), MyFilePosts, True,, EDP.None)
                _ContentNew.ListAddList(_TempMediaList, LAP.ClearBeforeAdd)
                DownloadContent(Token)
                ThrowIfDisposed()
                _ContentList.ListAddList(_ContentNew.Where(Function(c) c.State = UStates.Downloaded), LNC)
                _CountPictures = _ContentList.LongCount(Function(c) c.Type = UTypes.Picture)
                _CountVideo = _ContentList.LongCount(Function(c) c.Type = UTypes.Video)
                If DownloadedPictures + DownloadedVideos > 0 Or EnvirChanged.Invoke Then
                    If __SaveData Then
                        LastUpdated = Now
                        If Labels.Contains(LabelsKeeper.NoParsedUser) Then Labels.Remove(LabelsKeeper.NoParsedUser)
                        UpdateContentInformation()
                    Else
                        _CountVideo = 0
                        _CountPictures = 0
                        _ContentList.Clear()
                        CreatedByChannel = False
                    End If
                    If Not UserExists Then ReadyForDownload = False
                    UpdateUserInformation()
                End If
                ThrowIfDisposed()
                If Not CreatedByChannel Then
                    _DownloadedPicturesTotal += _DownloadedPicturesSession
                    _DownloadedVideosTotal += _DownloadedVideosSession
                End If
                If UpPic Or EnvirChanged.Invoke Then Raise_OnUserUpdated()
            Catch oex As OperationCanceledException When Token.IsCancellationRequested
                MyMainLOG = $"{Site} - {Name}: downloading canceled"
                Canceled = True
            Catch dex As ObjectDisposedException When Disposed
                Canceled = True
            Catch ex As Exception
                LogError(ex, "downloading data error")
                HasError = True
            Finally
                If Not Responser Is Nothing Then Responser.Dispose() : Responser = Nothing
                If Not Canceled Then _DataParsed = True ': LastUpdated = Now
                _ContentNew.Clear()
                DownloadTopCount = Nothing
            End Try
        End Sub
        Protected Sub UpdateDataFiles()
            If Not User.File.IsEmptyString Then
                MyFileData = User.File
                MyFileData.Name &= "_Data"
                MyFilePosts = User.File
                MyFilePosts.Name &= "_Posts"
                MyFilePosts.Extension = "txt"
            Else
                Throw New ArgumentNullException("User.File", "User file does not detected")
            End If
        End Sub
        Protected MustOverride Sub DownloadDataF(ByVal Token As CancellationToken)
        Protected MustOverride Sub ReparseVideo(ByVal Token As CancellationToken)
        Protected MustOverride Sub DownloadContent(ByVal Token As CancellationToken)
        Protected Sub DownloadContentDefault(ByVal Token As CancellationToken)
            Try
                Dim i%
                Dim dCount% = 0, dTotal% = 0
                ThrowAny(Token)
                If _ContentNew.Count > 0 Then
                    _ContentNew.RemoveAll(Function(c) c.URL.IsEmptyString)
                    If _ContentNew.Count > 0 Then
                        MyFile.Exists(SFO.Path)
                        Dim MyDir$ = MyFile.CutPath.PathNoSeparator
                        Dim vsf As Boolean = SeparateVideoFolderF
                        Dim __isVideo As Boolean
                        Dim f As SFile
                        Dim v As UserMedia
                        Using w As New WebClient
                            If vsf Then SFileShares.SFileExists($"{MyDir}\Video\", SFO.Path)
                            Progress.TotalCount += _ContentNew.Count
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

                                If Not v.File.IsEmptyString And Not v.URL_BASE.IsEmptyString Then
                                    Try
                                        __isVideo = v.Type = UTypes.Video Or f.Extension = "mp4"

                                        If f.Extension.IsEmptyString Then
                                            Select Case v.Type
                                                Case UTypes.Picture : f.Extension = "jpg"
                                                Case UTypes.Video : f.Extension = "mp4"
                                                Case UTypes.GIF : f.Extension = "gif"
                                            End Select
                                        End If

                                        If __isVideo And vsf Then f.Path = $"{f.PathWithSeparator}Video"
                                        w.DownloadFile(v.URL_BASE, f.ToString)

                                        If __isVideo Then
                                            v.Type = UTypes.Video
                                            DownloadedVideos += 1
                                            _CountVideo += 1
                                        Else
                                            v.Type = UTypes.Picture
                                            DownloadedPictures += 1
                                            _CountPictures += 1
                                        End If

                                        v.File = ChangeFileNameByProvider(f, v)
                                        v.State = UStates.Downloaded
                                        dCount += 1
                                    Catch wex As Exception
                                        ErrorDownloading(f, v.URL_BASE)
                                    End Try
                                Else
                                    v.State = UStates.Skipped
                                End If
                                _ContentNew(i) = v
                                If DownloadTopCount.HasValue AndAlso dCount >= DownloadTopCount.Value Then
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
        ''' <param name="RDE">Request DownloadingException</param>
        Protected Sub ProcessException(ByVal ex As Exception, ByVal Token As CancellationToken, ByVal Message As String, Optional ByVal RDE As Boolean = True)
            If Not ((TypeOf ex Is OperationCanceledException And Token.IsCancellationRequested) Or
                    (TypeOf ex Is ObjectDisposedException And Disposed)) Then
                If RDE AndAlso DownloadingException(ex, Message, True) = 0 Then LogError(ex, Message) : HasError = True
            End If
        End Sub
        ''' <summary>0 - Execute LogError and set HasError</summary>
        Protected MustOverride Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False) As Integer
        Protected Function ChangeFileNameByProvider(ByVal f As SFile, ByVal m As UserMedia) As SFile
            Dim ff As SFile = Nothing
            Try
                If Not f.IsEmptyString AndAlso f.Exists Then
                    Dim d As Date? = m.Post.Date
                    If Settings.FileReplaceNameByDate Then
                        Dim dd$ = AConvert(Of String)(If(d, Now), FileDateAppenderProvider, String.Empty)
                        ff = f
                        ff.Name = dd
                        ff = SFile.Indexed_IndexFile(ff,, New NumberedFile(ff))
                    ElseIf d.HasValue AndAlso (Settings.FileAddDateToFileName Or Settings.FileAddTimeToFileName) AndAlso
                                              (Not FileDateAppenderProvider Is Nothing And Not FileDateAppenderPattern.IsEmptyString) Then
                        ff = f
                        ff.Name = String.Format(FileDateAppenderPattern, f.Name, CStr(AConvert(Of String)(d.Value, FileDateAppenderProvider, String.Empty)))
                    End If
                    If Not ff.Name.IsEmptyString Then My.Computer.FileSystem.RenameFile(f, ff.File) : Return ff
                End If
                Return f
            Catch ex As Exception
                LogError(ex, $"change file name from [{f}] to [{ff}]")
                Return f
            End Try
        End Function
#End Region
#Region "Delete, Move, Merge"
        Friend Overridable Function Delete() As Integer Implements IUserData.Delete
            Return DeleteF(Me)
        End Function
        Friend Function DeleteF(ByVal Instance As IUserData) As Integer
            Dim f As SFile = SFile.GetPath(MyFile.CutPath.Path)
            If f.Exists(SFO.Path, False) AndAlso (User.Merged OrElse f.Delete(SFO.Path, False, False)) Then
                ImageHandler(Me, False)
                Settings.UsersList.Remove(User)
                Settings.UpdateUsersList()
                Settings.Users.Remove(Instance)
                Downloader.UserRemove(Instance)
                Dispose(True)
                Return 1
            Else
                Return 0
            End If
        End Function
        Friend Overridable Function MoveFiles(ByVal __CollectionName As String) As Boolean Implements IUserData.MoveFiles
            Return MoveFilesF(Me, __CollectionName)
        End Function
        Friend Function MoveFilesF(ByRef Instance As IUserData, ByVal __CollectionName As String) As Boolean
            Dim UserBefore As UserInfo = User
            Dim Removed As Boolean = True
            Dim _TurnBack As Boolean = False
            Try
                Dim f As SFile
                If IncludedInCollection Then
                    Settings.Users.Add(Instance)
                    Removed = False
                    User.CollectionName = String.Empty
                    User.IncludedInCollection = False
                Else
                    Settings.Users.Remove(Instance)
                    Removed = True
                    User.CollectionName = __CollectionName
                    User.IncludedInCollection = True
                End If
                _TurnBack = True
                User.UpdateUserFile()
                f = User.File.CutPath(, EDP.ThrowException)
                If f.Exists(SFO.Path, False) Then
                    If If(SFile.GetFiles(f,, SearchOption.AllDirectories), New List(Of SFile)).Count > 0 AndAlso
                       MsgBoxE({$"Destination directory [{f.Path}] already exists and contains files!" & vbCr &
                                "By continuing, this directory and all files will be deleted",
                                "Destination directory is not empty!"}, MsgBoxStyle.Exclamation,,, {"Delete", "Cancel"}) = 1 Then
                        MsgBoxE("Operation canceled", MsgBoxStyle.Exclamation)
                        User = UserBefore
                        If Removed Then Settings.Users.Add(Instance) Else Settings.Users.Remove(Instance)
                        _TurnBack = False
                        Return False
                    End If
                    f.Delete(SFO.Path, False, False, EDP.ThrowException)
                End If
                f.CutPath.Exists(SFO.Path)
                Directory.Move(UserBefore.File.CutPath(, EDP.ThrowException).Path, f.Path)
                Settings.UsersList.Remove(UserBefore)
                Settings.UpdateUsersList(User)
                UpdateUserInformation()
                Return True
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Files moving error")
                User = UserBefore
                If _TurnBack Then
                    If Removed Then Settings.Users.Add(Instance) Else Settings.Users.Remove(Instance)
                End If
                Return False
            End Try
        End Function
        Friend Overloads Sub MergeData()
            Dim UserBefore As UserInfo = User
            Try
                If DataMerging Then
                    Throw New InvalidOperationException($"{Site} - {Name}: data already merged") With {.HelpLink = 1}
                Else
                    Dim files As List(Of SFile) = Nothing
                    Dim allFiles As List(Of SFile) = Nothing
                    Dim fSource As SFile, fDest As SFile
                    Dim ReplacingPath$ = UserBefore.File.CutPath.Path
                    Dim dirs As List(Of SFile) = SFile.GetDirectories(UserBefore.File.CutPath,, SearchOption.AllDirectories)
                    Dim FilesMover As Action = Sub()
                                                   If files.ListExists Then
                                                       For Each fSource In files
                                                           fDest = fSource
                                                           fDest.Path = User.File.CutPath.PathWithSeparator & fSource.Path.Replace(ReplacingPath, String.Empty)
                                                           fDest.Exists(SFO.Path,, EDP.ThrowException)
                                                           fDest = CheckFile(fDest, allFiles)
                                                           File.Move(fSource, fDest)
                                                       Next
                                                       files.Clear()
                                                   End If
                                               End Sub
                    DataMerging = True
                    If dirs.ListExists Then
                        For Each dir As SFile In dirs
                            allFiles = SFile.GetFiles(SFile.GetPath(User.File.CutPath.PathWithSeparator &
                                                                    dir.Path.Replace(ReplacingPath, String.Empty)),,, EDP.ReturnValue)
                            files = SFile.GetFiles(dir,,, EDP.ReturnValue)
                            FilesMover.Invoke
                        Next
                    End If
                    allFiles = SFile.GetFiles(User.File.CutPath,,, EDP.ReturnValue)

                    files = SFile.GetFiles(UserBefore.File.CutPath,,, EDP.ReturnValue)
                    FilesMover.Invoke
                    If SFile.GetFiles(UserBefore.File.CutPath,, SearchOption.AllDirectories,
                                      New ErrorsDescriber(False, False, False, New List(Of SFile))).Count = 0 Then
                        UserBefore.File.CutPath.Delete(SFO.Path, False, False, EDP.SendInLog)
                    End If
                    UpdateUserInformation()
                End If
            Catch ioex As InvalidOperationException When ioex.HelpLink = 1
                MsgBoxE(ioex.Message, vbCritical)
            Catch ex As Exception
                LogError(ex, "[UserDataBase.MergeData]")
            End Try
        End Sub
        Private Function CheckFile(ByVal f As SFile, ByRef List As IEnumerable(Of SFile)) As SFile
            If List.ListExists Then
                Dim p As RParams = RParams.DMS(".+?\s{0,1}\((\d+)\)|.+", 0, EDP.ReturnValue)
                Dim i% = List.Where(Function(ff) CStr(RegexReplace(ff.Name, p)).Trim.ToLower = f.Name.Trim.ToLower).Count
                If i > 0 Then f.Name &= $" ({i + 1})"
            End If
            Return f
        End Function
#End Region
#Region "Errors functions"
        Protected Sub LogError(ByVal ex As Exception, ByVal Message As String)
            ErrorsDescriber.Execute(EDP.SendInLog, ex, $"{IIf(IncludedInCollection, $"{CollectionName}-", String.Empty)}{Site} - {Name}: {Message}")
        End Sub
        Protected Sub ErrorDownloading(ByVal f As SFile, ByVal URL As String)
            If Not f.Exists Then MyMainLOG = $"Error downloading from [{URL}] to [{f}]"
        End Sub
        ''' <exception cref="ObjectDisposedException"></exception>
        Protected Sub ThrowIfDisposed()
            If Disposed Then Throw New ObjectDisposedException(ToString(), "Object disposed")
        End Sub
        ''' <exception cref="OperationCanceledException"></exception>
        ''' <exception cref="ObjectDisposedException"></exception>
        Protected Sub ThrowAny(ByVal Token As CancellationToken)
            Token.ThrowIfCancellationRequested()
            ThrowIfDisposed()
        End Sub
#End Region
        Public Overrides Function ToString() As String
            If Settings.ViewModeIsPicture Then
                If IsCollection Then
                    Return CollectionName
                Else
                    Return IIf(FriendlyName.IsEmptyString, Name, FriendlyName)
                End If
            Else
                Dim t$ = String.Empty
                If Temporary Then
                    t = " (T)"
                ElseIf Favorite Then
                    t = " (F)"
                End If
                If IsCollection Then
                    Return $"Collection [{CollectionName}]{t}"
                Else
                    Return $"[{Site}]{t} {IIf(FriendlyName.IsEmptyString, Name, FriendlyName)}"
                End If
            End If
        End Function
#Region "Buttons actions"
        Private Sub BTT_CONTEXT_DOWN_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_DOWN.Click
            Downloader.Add(Me)
        End Sub
        Private Sub BTT_CONTEXT_EDIT_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_EDIT.Click
            Using f As New Editors.UserCreatorForm(Me)
                f.ShowDialog()
                If f.DialogResult = DialogResult.OK Then UpdateUserInformation()
            End Using
        End Sub
        Private Sub BTT_CONTEXT_DELETE_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_DELETE.Click
        End Sub
        Private Sub BTT_CONTEXT_OPEN_PATH_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_OPEN_PATH.Click
            OpenFolder()
        End Sub
        Private Sub BTT_CONTEXT_OPEN_SITE_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_OPEN_SITE.Click
            OpenSite()
        End Sub
#End Region
#Region "IComparable Support"
        Friend Overridable Function CompareTo(ByVal Other As UserDataBase) As Integer Implements IComparable(Of UserDataBase).CompareTo
            Dim x% = CompareValue(Me)
            Dim y% = CompareValue(Other)
            If x.CompareTo(y) = 0 Then
                Return Name.CompareTo(Other.Name)
            Else
                Return x.CompareTo(y)
            End If
        End Function
        Protected Function CompareValue(ByVal x As UserDataBase) As Integer
            Dim OutValue% = CInt(x.Site) * 10000
            If x.IsCollection Then OutValue -= 1000
            If x.Temporary Then OutValue += 2000
            If x.Favorite Then OutValue -= 500
            Return OutValue
        End Function
        Friend Overridable Function CompareTo(ByVal Obj As Object) As Integer Implements IComparable.CompareTo
            If TypeOf Obj Is Reddit.Channel Then
                Return CompareTo(DirectCast(DirectCast(Obj, Reddit.Channel).Instance, UserDataBase))
            Else
                Return CompareTo(DirectCast(Obj, UserDataBase))
            End If
        End Function
#End Region
#Region "IEquatable Support"
        Friend Overridable Overloads Function Equals(ByVal Other As UserDataBase) As Boolean Implements IEquatable(Of UserDataBase).Equals
            Return Site = Other.Site And Name = Other.Name And IsSavedPosts = Other.IsSavedPosts
        End Function
        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            If TypeOf Obj Is Reddit.Channel Then
                Return Equals(DirectCast(DirectCast(Obj, Reddit.Channel).Instance, UserDataBase))
            Else
                Return Equals(DirectCast(Obj, UserDataBase))
            End If
        End Function
#End Region
#Region "IDisposable Support"
        Protected disposedValue As Boolean = False
        Friend ReadOnly Property Disposed As Boolean Implements IUserData.Disposed
            Get
                Return disposedValue
            End Get
        End Property
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    _ContentList.Clear()
                    _ContentNew.Clear()
                    _TempMediaList.Clear()
                    _TempPostsList.Clear()
                    If Not Responser Is Nothing Then Responser.Dispose()
                    If Not BTT_CONTEXT_DOWN Is Nothing Then BTT_CONTEXT_DOWN.Dispose()
                    If Not BTT_CONTEXT_EDIT Is Nothing Then BTT_CONTEXT_EDIT.Dispose()
                    If Not BTT_CONTEXT_DELETE Is Nothing Then BTT_CONTEXT_DELETE.Dispose()
                    If Not BTT_CONTEXT_OPEN_PATH Is Nothing Then BTT_CONTEXT_OPEN_PATH.Dispose()
                    If Not BTT_CONTEXT_OPEN_SITE Is Nothing Then BTT_CONTEXT_OPEN_SITE.Dispose()
                    If _InvokeImageHandler Then ImageHandler(Me, False)
                End If
                disposedValue = True
            End If
        End Sub
        Protected Overloads Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub
        Friend Overloads Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
    Friend Interface IContentProvider
        Property Site As Sites
        Property Name As String
        Property ID As String
        Property FriendlyName As String
        Property Description As String
        Property Favorite As Boolean
        Property Temporary As Boolean
        Sub OpenSite()
        Sub DownloadData(ByVal Token As CancellationToken)
    End Interface
    Friend Interface IUserData : Inherits IContentProvider, IComparable(Of UserDataBase), IComparable, IEquatable(Of UserDataBase), IDisposable
        Event OnUserUpdated(ByVal User As IUserData)
        Property ParseUserMediaOnly As Boolean
#Region "Images"
        Function GetPicture() As Image
        Sub SetPicture(ByVal f As SFile)
#End Region
#Region "Collection support"
        ReadOnly Property IsCollection As Boolean
        Property CollectionName As String
        ReadOnly Property IncludedInCollection As Boolean
        ReadOnly Property Labels As List(Of String)
#End Region
        ReadOnly Property IsChannel As Boolean
        Property Exists As Boolean
        Property Suspended As Boolean
        Property ReadyForDownload As Boolean
        Property [File] As SFile
        Property FileExists As Boolean
        Property DownloadedPictures As Integer
        Property DownloadedVideos As Integer
        ReadOnly Property DownloadedTotal(Optional ByVal Total As Boolean = True) As Integer
        ReadOnly Property DownloadedInformation As String
        Property HasError As Boolean
        ReadOnly Property FitToAddParams As Boolean
        ReadOnly Property LVIKey As String
        ReadOnly Property LVIIndex As Integer
        Property DownloadImages As Boolean
        Property DownloadVideos As Boolean
        Function GetLVI(ByVal Destination As ListView) As ListViewItem
        Function GetLVIGroup(ByVal Destination As ListView) As ListViewGroup
        Sub LoadUserInformation()
        Sub UpdateUserInformation()
        ''' <summary>
        ''' 0 - Nothing removed<br/>
        ''' 1 - User removed<br/>
        ''' 2 - Collection removed<br/>
        ''' 3 - Collection split
        ''' </summary>
        Function Delete() As Integer
        Function MoveFiles(ByVal CollectionName As String) As Boolean
        Sub OpenFolder()
        ReadOnly Property Self As IUserData
        Property DownloadTopCount As Integer?
        ReadOnly Property Disposed As Boolean
    End Interface
    Friend Interface IChannelLimits
        Property AutoGetLimits As Boolean
        Property DownloadLimitCount As Integer?
        Property DownloadLimitPost As String
        Property DownloadLimitDate As Date?
        Overloads Sub SetLimit(Optional ByVal Post As String = "", Optional ByVal Count As Integer? = Nothing, Optional ByVal [Date] As Date? = Nothing)
        Overloads Sub SetLimit(ByVal Source As IChannelLimits)
    End Interface
    Friend Interface IChannelData : Inherits IContentProvider, IChannelLimits
        Property SkipExistsUsers As Boolean
        Property SaveToCache As Boolean
    End Interface
End Namespace