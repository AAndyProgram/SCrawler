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
Imports SCrawler.API
Imports SCrawler.API.Base
Friend Class SettingsCLS : Implements IDisposable
    Friend Const DefaultMaxDownloadingTasks As Integer = 5
    Friend Const Name_Node_Sites As String = "Sites"
    Friend ReadOnly Design As XmlFile
    Private ReadOnly MyXML As XmlFile
    Friend ReadOnly OS64 As Boolean
    Friend ReadOnly FfmpegExists As Boolean
    Friend ReadOnly FfmpegFile As SFile
    Friend ReadOnly Property UseM3U8 As Boolean
        Get
            Return OS64 And FfmpegExists
        End Get
    End Property
    Private ReadOnly MySites As Dictionary(Of Sites, SiteSettings)
    Friend ReadOnly Property Users As List(Of IUserData)
    Friend ReadOnly Property UsersList As List(Of UserInfo)
    Friend Property Channels As Reddit.ChannelsCollection
    Friend ReadOnly Property Labels As LabelsKeeper
    Friend ReadOnly Property BlackList As List(Of UserBan)
    Private ReadOnly BlackListFile As SFile = $"{SettingsFolderName}\BlackList.txt"
    Private ReadOnly UsersSettingsFile As SFile = $"{SettingsFolderName}\Users.xml"
    Friend Sub New()
        OS64 = Environment.Is64BitOperatingSystem
        FfmpegFile = "ffmpeg.exe"
        FfmpegExists = FfmpegFile.Exists
        If OS64 And Not FfmpegExists Then MsgBoxE("[ffmpeg.exe] is missing", vbExclamation)
        Design = New XmlFile("Settings\Design.xml", Protector.Modes.All)
        MyXML = New XmlFile(Nothing) With {.AutoUpdateFile = True}
        Users = New List(Of IUserData)
        UsersList = New List(Of UserInfo)
        BlackList = New List(Of UserBan)

        GlobalPath = New XMLValue(Of SFile)("GlobalPath", New SFile($"{SFile.GetPath(Application.StartupPath).PathWithSeparator}Data\"), MyXML,,
                                            XMLValue(Of SFile).ToFilePath)
        MySites = New Dictionary(Of Sites, SiteSettings) From {
            {Sites.Reddit, New SiteSettings(Sites.Reddit, MyXML, GlobalPath.Value)},
            {Sites.Twitter, New SiteSettings(Sites.Twitter, MyXML, GlobalPath.Value)}
        }
        MySites(Sites.Reddit).Responser.Decoders.Add(SymbolsConverter.Converters.Unicode)

        SeparateVideoFolder = New XMLValue(Of Boolean)("SeparateVideoFolder", True, MyXML)
        CollectionsPath = New XMLValue(Of String)("CollectionsPath", "Collections", MyXML)

        Dim n() As String = {"Defaults"}
        DefaultTemporary = New XMLValue(Of Boolean)("Temporary", False, MyXML, n)
        DefaultTemporary.ReplaceByValue("DefaultTemporary")
        DefaultDownloadImages = New XMLValue(Of Boolean)("DownloadImages", True, MyXML, n)
        DefaultDownloadImages.ReplaceByValue("DefaultDownloadImages")
        DefaultDownloadVideos = New XMLValue(Of Boolean)("DownloadVideos", True, MyXML, n)
        DefaultDownloadVideos.ReplaceByValue("DefaultDownloadVideos")
        ChangeReadyForDownOnTempChange = New XMLValue(Of Boolean)("ChangeReadyForDownOnTempChange", True, MyXML, n)

        n = {Name_Node_Sites, Sites.Reddit.ToString}
        RedditTemporary = New XMLValue(Of Boolean)
        RedditTemporary.SetExtended("Temporary", False, MyXML, n)
        RedditTemporary.SetDefault(DefaultTemporary)

        RedditDownloadImages = New XMLValue(Of Boolean)
        RedditDownloadImages.SetExtended("DownloadImages", True, MyXML, n)
        RedditDownloadImages.SetDefault(DefaultDownloadImages)

        RedditDownloadVideos = New XMLValue(Of Boolean)
        RedditDownloadVideos.SetExtended("DownloadVideos", True, MyXML, n)
        RedditDownloadVideos.SetDefault(DefaultDownloadVideos)

        n = {Name_Node_Sites, Sites.Twitter.ToString}
        TwitterTemporary = New XMLValue(Of Boolean)
        TwitterTemporary.SetExtended("Temporary", False, MyXML, n)
        TwitterTemporary.SetDefault(DefaultTemporary)

        TwitterDownloadImages = New XMLValue(Of Boolean)
        TwitterDownloadImages.SetExtended("DownloadImages", True, MyXML, n)
        TwitterDownloadImages.SetDefault(DefaultDownloadImages)

        TwitterDownloadVideos = New XMLValue(Of Boolean)
        TwitterDownloadVideos.SetExtended("DownloadVideos", True, MyXML, n)
        TwitterDownloadVideos.SetDefault(DefaultDownloadVideos)

        TwitterDefaultGetUserMedia = New XMLValue(Of Boolean)("TwitterDefaultGetUserMedia", True, MyXML, n)

        MaxLargeImageHeigh = New XMLValue(Of Integer)("MaxLargeImageHeigh", 150, MyXML)
        MaxSmallImageHeigh = New XMLValue(Of Integer)("MaxSmallImageHeigh", 15, MyXML)
        InfoViewMode = New XMLValue(Of Integer)("InfoViewMode", DownloadedInfoForm.ViewModes.Session, MyXML)
        ViewMode = New XMLValue(Of Integer)("ViewMode", ViewModes.IconLarge, MyXML)
        ShowingMode = New XMLValue(Of Integer)("ShowingMode", ShowingModes.All, MyXML)

        LatestSavingPath = New XMLValue(Of SFile)("LatestSavingPath", Nothing, MyXML,, XMLValue(Of SFile).ToFilePath)
        LatestSelectedLabels = New XMLValue(Of String)("LatestSelectedLabels",, MyXML)
        LatestSelectedChannel = New XMLValue(Of String)("LatestSelectedChannel",, MyXML)

        n = {Name_Node_Sites, "Channels"}
        ChannelsDefaultReadyForDownload = New XMLValue(Of Boolean)("ChannelsDefaultReadyForDownload", False, MyXML, n)
        ChannelsDefaultTemporary = New XMLValue(Of Boolean)("ChannelsDefaultTemporary", True, MyXML, n)
        ChannelsRegularCheckMD5 = New XMLValue(Of Boolean)("ChannelsRegularCheckMD5", False, MyXML, n)
        ChannelsImagesRows = New XMLValue(Of Integer)("ImagesRows", 2, MyXML, n)
        ChannelsImagesRows.ReplaceByValue("ChannelsImagesRows")
        ChannelsImagesColumns = New XMLValue(Of Integer)("ImagesColumns", 5, MyXML, n)
        ChannelsImagesColumns.ReplaceByValue("ChannelsImagesColumns")
        ChannelsHideExistsUser = New XMLValue(Of Boolean)("HideExistsUser", True, MyXML, n)
        ChannelsHideExistsUser.ReplaceByValue("ChannelsHideExistsUser")
        ChannelsMaxJobsCount = New XMLValue(Of Integer)("MaxJobsCount", DefaultMaxDownloadingTasks, MyXML, n)
        ChannelsMaxJobsCount.ReplaceByValue("ChannelsMaxJobsCount")

        n = {"Users"}
        FromChannelDownloadTop = New XMLValue(Of Integer)("FromChannelDownloadTop", 10, MyXML, n)
        FromChannelDownloadTop.ReplaceByValue("FromChannelDownloadTop")
        FromChannelDownloadTopUse = New XMLValue(Of Boolean)("FromChannelDownloadTopUse", False, MyXML, n)
        FromChannelDownloadTopUse.ReplaceByValue("FromChannelDownloadTopUse")
        FromChannelCopyImageToUser = New XMLValue(Of Boolean)("FromChannelCopyImageToUser", True, MyXML, n)
        FromChannelCopyImageToUser.ReplaceByValue("FromChannelCopyImageToUser")

        n = {"Users", "FileName"}
        MaxUsersJobsCount = New XMLValue(Of Integer)("MaxJobsCount", DefaultMaxDownloadingTasks, MyXML, n)
        MaxUsersJobsCount.ReplaceByValue("MaxUsersJobsCount")
        FileAddDateToFileName = New XMLValue(Of Boolean)("FileAddDateToFileName", False, MyXML, n) With {.OnChangeFunction = AddressOf ChangeDateProvider}
        FileAddDateToFileName.ReplaceByValue("FileAddDateToFileName")
        FileAddTimeToFileName = New XMLValue(Of Boolean)("FileAddTimeToFileName", False, MyXML, n) With {.OnChangeFunction = AddressOf ChangeDateProvider}
        FileAddTimeToFileName.ReplaceByValue("FileAddTimeToFileName")
        FileDateTimePositionEnd = New XMLValue(Of Boolean)("FileDateTimePositionEnd", True, MyXML, n) With {.OnChangeFunction = AddressOf ChangeDateProvider}
        FileDateTimePositionEnd.ReplaceByValue("FileDateTimePositionEnd")
        FileReplaceNameByDate = New XMLValue(Of Boolean)("FileReplaceNameByDate", False, MyXML, n)
        FileReplaceNameByDate.ReplaceByValue("FileReplaceNameByDate")

        CheckUpdatesAtStart = New XMLValue(Of Boolean)("CheckUpdatesAtStart", True, MyXML)
        ShowNewVersionNotification = New XMLValue(Of Boolean)("ShowNewVersionNotification", True, MyXML)
        LatestVersion = New XMLValue(Of String)("LatestVersion", String.Empty, MyXML)

        If MyXML.ChangesDetected Then MyXML.Sort() : MyXML.UpdateData()
        Labels = New LabelsKeeper
        If Not LatestSelectedLabels.IsEmptyString Then Labels.CurrentSelection.ListAddList(LatestSelectedLabels.Value.StringToList(Of String, List(Of String))("|"))
        If BlackListFile.Exists Then
            BlackList.ListAddList(IO.File.ReadAllLines(BlackListFile), LAP.NotContainsOnly)
            If BlackList.Count > 0 Then BlackList.RemoveAll(Function(b) Not b.Exists)
        End If
        _UpdatesSuspended = False
        ChangeDateProvider(Nothing, Nothing, Nothing)
    End Sub
    Private Sub ChangeDateProvider(ByVal Sender As Object, ByVal Name As String, ByVal Value As Object)
        If Not _UpdatesSuspended Then
            Dim p$ = String.Empty
            If FileAddDateToFileName Then p = "yyyyMMdd"
            If FileAddTimeToFileName Then p.StringAppend("HHmmss", "_")
            If Not p.IsEmptyString Then FileDateAppenderProvider = New ADateTime(p) Else FileDateAppenderProvider = New ADateTime("yyyyMMdd_HHmmss")
            If FileDateTimePositionEnd Then FileDateAppenderPattern = "{0}_{1}" Else FileDateAppenderPattern = "{1}_{0}"
        End If
    End Sub
    Friend Sub LoadUsers()
        Try
            Users.Clear()
            If UsersSettingsFile.Exists Then
                Using x As New XmlFile(UsersSettingsFile, Protector.Modes.All, False) With {.AllowSameNames = True}
                    x.LoadData()
                    If x.Count > 0 Then x.ForEach(Sub(xx) UsersList.Add(xx))
                End Using
                Dim PNC As Func(Of UserInfo, Boolean) = Function(u) Not u.IncludedInCollection
                Dim NeedUpdate As Boolean = False
                If UsersList.Count > 0 Then
                    Dim cUsers As List(Of UserInfo) = UsersList.Where(Function(u) u.IncludedInCollection).ToList
                    If cUsers.ListExists Then
                        Dim d As New Dictionary(Of SFile, List(Of UserInfo))
                        cUsers = cUsers.ListForEachCopy(Of List(Of UserInfo))(Function(ByVal f As UserInfo, ByVal f_indx As Integer) As UserInfo
                                                                                  Dim m% = IIf(f.Merged, 1, 2)
                                                                                  If SFile.GetPath(f.File.CutPath(m - 1).Path).Exists(SFO.Path, False) Then
                                                                                      Dim fp As SFile = SFile.GetPath(f.File.CutPath(m).Path)
                                                                                      If Not d.ContainsKey(fp) Then
                                                                                          d.Add(fp, New List(Of UserInfo) From {f})
                                                                                      Else
                                                                                          d(f.File.CutPath(m).Path).Add(f)
                                                                                      End If
                                                                                      Return f
                                                                                  Else
                                                                                      NeedUpdate = True
                                                                                      UsersList.Remove(f)
                                                                                      Return Nothing
                                                                                  End If
                                                                              End Function, True)
                        Dim v%
                        If d.Count > 0 Then
                            For Each kv In d
                                Users.Add(New UserDataBind(kv.Value(0).CollectionName))
                                CollectionHandler(DirectCast(Users(Users.Count - 1), UserDataBind))
                                For v = 0 To kv.Value.Count - 1 : DirectCast(Users(Users.Count - 1), UserDataBind).Add(kv.Value(v), False) : Next
                            Next
                            d.Clear()
                        End If
                    End If

                    If UsersList.LongCount(PNC) > 0 Then UsersList.Where(PNC).ToList.ForEach(Sub(u) Users.Add(UserDataBase.GetInstance(u, False)))
                End If
                If Users.Count > 0 Then
                    Dim t As New List(Of Task)
                    For Each user As IUserData In Users : t.Add(Task.Run(AddressOf user.LoadUserInformation)) : Next
                    Task.WaitAll(t.ToArray)
                    t.Clear()
                    Dim du As List(Of UserInfo) = (From u As IUserData In Users
                                                   Where Not u.IsCollection AndAlso Not u.FileExists
                                                   Select DirectCast(u.Self, UserDataBase).User).ToList
                    If du.ListExists Then du.ForEach(Sub(u) UsersList.Remove(u)) : du.Clear()
                    Users.ListDisposeRemoveAll(Function(ByVal u As IUserData) As Boolean
                                                   If u.IsCollection Then
                                                       With DirectCast(u, UserDataBind)
                                                           If .Count > 0 Then
                                                               For i% = .Count - 1 To 0 Step -1
                                                                   If Not .Item(i).FileExists Then
                                                                       .Item(i).Delete()
                                                                       .Collections.RemoveAt(i)
                                                                   End If
                                                               Next
                                                           End If
                                                           Return Not .FileExists
                                                       End With
                                                   Else
                                                       Return Not u.FileExists
                                                   End If
                                               End Function)
                End If
                If NeedUpdate Then UpdateUsersList()
            End If
            If Users.Count > 0 Then
                Labels.ToList.ListAddList(Users.SelectMany(Function(u) u.Labels), LAP.NotContainsOnly)
                If Labels.NewLabelsExists Then Labels.Update() : Labels.NewLabels.Clear()
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private _UserListUpdateRequired As Boolean = False
    Friend ReadOnly Property UserListUpdateRequired As Boolean
        Get
            Return _UserListUpdateRequired
        End Get
    End Property
    Friend Overloads Sub UpdateUsersList(ByVal u As UserInfo)
        Dim i% = UsersList.IndexOf(u)
        If i >= 0 Then
            UsersList(i) = u
        Else
            UsersList.Add(u)
        End If
        UpdateUsersList()
    End Sub
    Friend Overloads Sub UpdateUsersList()
        Try
            If UsersList.Count > 0 Then
                Using x As New XmlFile With {.AllowSameNames = True, .Name = "Users"}
                    UsersList.ForEach(Sub(u) x.Add(u.GetContainer()))
                    x.Save(UsersSettingsFile)
                End Using
            End If
            _UserListUpdateRequired = False
        Catch ex As Exception
            _UserListUpdateRequired = True
        End Try
    End Sub
    Friend Sub UpdateBlackList()
        If BlackList.Count > 0 Then
            TextSaver.SaveTextToFile(BlackList.ListToString(, vbNewLine), BlackListFile, True, False, EDP.None)
        Else
            If BlackListFile.Exists Then BlackListFile.Delete()
        End If
    End Sub
    Friend Overloads Function UserExists(ByVal s As Sites, ByVal UserID As String) As Boolean
        Dim UserFinderBase As Predicate(Of IUserData) = Function(user) user.Site = s And user.Name = UserID
        Dim UserFinder As Predicate(Of IUserData) = Function(ByVal user As IUserData) As Boolean
                                                        If user.IsCollection Then
                                                            With DirectCast(user, UserDataBind)
                                                                Return .Count > 0 AndAlso .Collections.Exists(UserFinderBase)
                                                            End With
                                                        Else
                                                            Return UserFinderBase.Invoke(user)
                                                        End If
                                                    End Function
        Return Users.Count > 0 AndAlso Users.Exists(UserFinder)
    End Function
    Friend Overloads Function UserExists(ByVal _User As IUserData) As Boolean
        Return UserExists(_User.Site, _User.Name)
    End Function
    Friend Overloads Function UserExists(ByVal _User As UserInfo) As Boolean
        Return UserExists(_User.Site, _User.Name)
    End Function
    Private _UpdatesSuspended As Boolean = True
    Friend Sub BeginUpdate()
        MyXML.BeginUpdate()
        _UpdatesSuspended = True
    End Sub
    Friend Sub EndUpdate()
        MyXML.EndUpdate()
        If MyXML.ChangesDetected Then MyXML.UpdateData()
        _UpdatesSuspended = False
        ChangeDateProvider(Nothing, Nothing, Nothing)
    End Sub
    Friend ReadOnly Property Site(ByVal s As Sites) As SiteSettings
        Get
            Return MySites(s)
        End Get
    End Property
    Friend ReadOnly Property GlobalPath As XMLValue(Of SFile)
    Friend ReadOnly Property SeparateVideoFolder As XMLValue(Of Boolean)
    Friend ReadOnly Property CollectionsPath As XMLValue(Of String)
    Friend ReadOnly Property CollectionsPathF As SFile
        Get
            If GlobalPath.IsEmptyString Then
                Throw New ArgumentNullException("GlobalPath", "GlobalPath does not set")
            Else
                Return SFile.GetPath($"{GlobalPath.Value.PathWithSeparator}{CollectionsPath.Value}")
            End If
        End Get
    End Property
    Friend ReadOnly Property MaxUsersJobsCount As XMLValue(Of Integer)
#Region "Defaults"
    Friend ReadOnly Property DefaultTemporary As XMLValue(Of Boolean)
    Friend ReadOnly Property DefaultDownloadImages As XMLValue(Of Boolean)
    Friend ReadOnly Property DefaultDownloadVideos As XMLValue(Of Boolean)
    Friend ReadOnly Property ChangeReadyForDownOnTempChange As XMLValue(Of Boolean)
#Region "Reddit"
    Friend ReadOnly Property RedditTemporary As XMLValue(Of Boolean)
    Friend ReadOnly Property RedditDownloadImages As XMLValue(Of Boolean)
    Friend ReadOnly Property RedditDownloadVideos As XMLValue(Of Boolean)
#End Region
#Region "Twitter"
    Friend ReadOnly Property TwitterTemporary As XMLValue(Of Boolean)
    Friend ReadOnly Property TwitterDownloadImages As XMLValue(Of Boolean)
    Friend ReadOnly Property TwitterDownloadVideos As XMLValue(Of Boolean)
    Friend ReadOnly Property TwitterDefaultGetUserMedia As XMLValue(Of Boolean)
#End Region
#End Region
#Region "User data"
    Friend ReadOnly Property FromChannelDownloadTop As XMLValue(Of Integer)
    Friend ReadOnly Property FromChannelDownloadTopUse As XMLValue(Of Boolean)
    Friend ReadOnly Property FromChannelCopyImageToUser As XMLValue(Of Boolean)
#Region "File naming"
    Friend ReadOnly Property FileAddDateToFileName As XMLValue(Of Boolean)
    Friend ReadOnly Property FileAddTimeToFileName As XMLValue(Of Boolean)
    Friend ReadOnly Property FileDateTimePositionEnd As XMLValue(Of Boolean)
    Friend ReadOnly Property FileReplaceNameByDate As XMLValue(Of Boolean)
#End Region
#End Region
#Region "View"
    Friend ReadOnly Property MaxLargeImageHeigh As XMLValue(Of Integer)
    Friend ReadOnly Property MaxSmallImageHeigh As XMLValue(Of Integer)
    Friend ReadOnly Property InfoViewMode As XMLValue(Of Integer)
    Friend ReadOnly Property ViewMode As XMLValue(Of Integer)
    Friend ReadOnly Property ViewModeIsPicture As Boolean
        Get
            Select Case ViewMode.Value
                Case View.LargeIcon, View.SmallIcon : Return True
                Case Else : Return False
            End Select
        End Get
    End Property
    Friend ReadOnly Property ShowingMode As XMLValue(Of Integer)
#End Region
#Region "Latest values"
    Friend ReadOnly Property LatestSavingPath As XMLValue(Of SFile)
    Friend ReadOnly Property LatestSelectedLabels As XMLValue(Of String)
    Friend ReadOnly Property LatestSelectedChannel As XMLValue(Of String)
#End Region
#Region "Channels properties"
    Friend ReadOnly Property ChannelsDefaultReadyForDownload As XMLValue(Of Boolean)
    Friend ReadOnly Property ChannelsDefaultTemporary As XMLValue(Of Boolean)
    Friend ReadOnly Property ChannelsRegularCheckMD5 As XMLValue(Of Boolean)
    Friend ReadOnly Property ChannelsImagesRows As XMLValue(Of Integer)
    Friend ReadOnly Property ChannelsImagesColumns As XMLValue(Of Integer)
    Friend ReadOnly Property ChannelsHideExistsUser As XMLValue(Of Boolean)
    Friend ReadOnly Property ChannelsMaxJobsCount As XMLValue(Of Integer)
#End Region
#Region "New version properties"
    Friend ReadOnly Property CheckUpdatesAtStart As XMLValue(Of Boolean)
    Friend ReadOnly Property ShowNewVersionNotification As XMLValue(Of Boolean)
    Friend ReadOnly Property LatestVersion As XMLValue(Of String)
#End Region
#Region "IDisposable Support"
    Private disposedValue As Boolean = False
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                If UserListUpdateRequired Then UpdateUsersList()
                If Not Channels Is Nothing Then
                    Channels.Dispose()
                    If Reddit.ChannelsCollection.ChannelsPathCache.Exists(SFO.Path, False) Then _
                        Reddit.ChannelsCollection.ChannelsPathCache.Delete(SFO.Path, False, False, EDP.None)
                End If
                For Each kv In MySites : kv.Value.Dispose() : Next
                MySites.Clear()
                Users.ListClearDispose
                UsersList.Clear()
                Design.Dispose()
                MyXML.Dispose()
            End If
            disposedValue = True
        End If
    End Sub
    Protected Overrides Sub Finalize()
        Dispose(False)
        MyBase.Finalize()
    End Sub
    Friend Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class