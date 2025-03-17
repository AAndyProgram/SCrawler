' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.IO
Imports System.Net
Imports System.Threading
Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports SCrawler.Plugin
Imports SCrawler.Plugin.Hosts
Imports PersonalUtilities.Functions.Messaging
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Objects
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.ImageRenderer
Imports UStates = SCrawler.API.Base.UserMedia.States
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Imports CookieUpdateModes = PersonalUtilities.Tools.Web.Cookies.CookieKeeper.UpdateModes
Namespace API.Base
    Friend MustInherit Class UserDataBase : Implements IUserData, IPluginContentProvider, IThrower
        Friend Const UserFileAppender As String = "User"
#Region "Events"
        Private ReadOnly UserUpdatedEventHandlers As List(Of IUserData.UserUpdatedEventHandler)
        Friend Custom Event UserUpdated As IUserData.UserUpdatedEventHandler Implements IUserData.UserUpdated
            AddHandler(ByVal e As IUserData.UserUpdatedEventHandler)
                If Not UserUpdatedEventHandlers.Contains(e) Then UserUpdatedEventHandlers.Add(e)
            End AddHandler
            RemoveHandler(ByVal e As IUserData.UserUpdatedEventHandler)
                If UserUpdatedEventHandlers.Contains(e) Then UserUpdatedEventHandlers.Remove(e)
            End RemoveHandler
            RaiseEvent(ByVal User As IUserData)
                Try
                    If UserUpdatedEventHandlers.Count > 0 Then
                        For i% = 0 To UserUpdatedEventHandlers.Count - 1
                            Try : UserUpdatedEventHandlers(i).Invoke(User) : Catch : End Try
                        Next
                    End If
                Catch
                End Try
            End RaiseEvent
        End Event
        Protected Sub OnUserUpdated()
            RaiseEvent UserUpdated(Me)
        End Sub
        Friend Sub RemoveUpdateHandlers()
            UserUpdatedEventHandlers.Clear()
        End Sub
        Private ReadOnly UserDownloadStateChangedEventHandlers As List(Of UserDownloadStateChangedEventHandler)
        Friend Custom Event UserDownloadStateChanged As UserDownloadStateChangedEventHandler
            AddHandler(ByVal h As UserDownloadStateChangedEventHandler)
                If Not UserDownloadStateChangedEventHandlers.Contains(h) Then UserDownloadStateChangedEventHandlers.Add(h)
            End AddHandler
            RemoveHandler(ByVal h As UserDownloadStateChangedEventHandler)
                UserDownloadStateChangedEventHandlers.Remove(h)
            End RemoveHandler
            RaiseEvent(ByVal User As IUserData, ByVal IsDownloading As Boolean)
                Try
                    If UserDownloadStateChangedEventHandlers.Count > 0 Then
                        For i% = 0 To UserDownloadStateChangedEventHandlers.Count - 1
                            Try : UserDownloadStateChangedEventHandlers(i).Invoke(User, IsDownloading) : Catch : End Try
                        Next
                    End If
                Catch
                End Try
            End RaiseEvent
        End Event
        Private Sub OnUserDownloadStateChanged(ByVal IsDownloading As Boolean)
            RaiseEvent UserDownloadStateChanged(Me, IsDownloading)
        End Sub
#End Region
#Region "Collection buttons"
        Private _CollectionButtonsExists As Boolean = False
        Private _CollectionButtonsColorsSet As Boolean = False
        Friend WithEvents BTT_CONTEXT_DOWN As ToolStripKeyMenuItem
        Friend WithEvents BTT_CONTEXT_DOWN_LIMIT As ToolStripKeyMenuItem
        Friend WithEvents BTT_CONTEXT_DOWN_DATE As ToolStripKeyMenuItem
        Friend WithEvents BTT_CONTEXT_EDIT As ToolStripMenuItem
        Friend WithEvents BTT_CONTEXT_DELETE As ToolStripMenuItem
        Friend WithEvents BTT_CONTEXT_ERASE As ToolStripMenuItem
        Friend WithEvents BTT_CONTEXT_OPEN_PATH As ToolStripMenuItem
        Friend WithEvents BTT_CONTEXT_OPEN_SITE As ToolStripMenuItem
        Friend Sub CreateButtons()
            Dim tn$ = $"[{Site}] - {Name}"
            Dim _tn$ = $"{Site}{Name}"
            Dim tnn As Func(Of String, String) = Function(Input) $"{Input}{_tn}"
            Dim i As Image = Nothing
            With HOST.Source
                If Not .Icon Is Nothing Then
                    i = .Icon.ToBitmap
                ElseIf Not .Image Is Nothing Then
                    i = .Image
                End If
            End With
            BTT_CONTEXT_DOWN = New ToolStripKeyMenuItem(tn, i) With {.Name = tnn("DOWN"), .Tag = Me}
            BTT_CONTEXT_DOWN_LIMIT = New ToolStripKeyMenuItem(tn, i) With {.Name = tnn("DOWN_LIMIT"), .Tag = Me}
            BTT_CONTEXT_DOWN_DATE = New ToolStripKeyMenuItem(tn, i) With {.Name = tnn("DOWN_DATE"), .Tag = Me}
            BTT_CONTEXT_EDIT = New ToolStripMenuItem(tn, i) With {.Name = tnn("EDIT"), .Tag = Me}
            BTT_CONTEXT_DELETE = New ToolStripMenuItem(tn, i) With {.Name = tnn("DELETE"), .Tag = Me}
            BTT_CONTEXT_ERASE = New ToolStripMenuItem(tn, i) With {.Name = tnn("ERASE"), .Tag = Me}
            BTT_CONTEXT_OPEN_PATH = New ToolStripMenuItem(tn, i) With {.Name = tnn("PATH"), .Tag = Me}
            BTT_CONTEXT_OPEN_SITE = New ToolStripMenuItem(tn, i) With {.Name = tnn("SITE"), .Tag = Me}
            UpdateButtonsColor()
            _CollectionButtonsExists = True
            If _UserInformationLoaded Then _CollectionButtonsColorsSet = True
        End Sub
        Private Sub UpdateButtonsColor()
            Dim cb As Color = SystemColors.Control
            Dim cf As Color = SystemColors.ControlText
            If Not UserExists Then
                cb = MyColor.DeleteBack
                cf = MyColor.DeleteFore
            ElseIf UserSuspended Then
                cb = MyColor.EditBack
                cf = MyColor.EditFore
            End If
            For Each b As ToolStripMenuItem In {BTT_CONTEXT_DOWN, BTT_CONTEXT_DOWN_LIMIT, BTT_CONTEXT_DOWN_DATE,
                                                BTT_CONTEXT_EDIT, BTT_CONTEXT_DELETE, BTT_CONTEXT_ERASE,
                                                BTT_CONTEXT_OPEN_PATH, BTT_CONTEXT_OPEN_SITE}
                If Not b Is Nothing Then b.BackColor = cb : b.ForeColor = cf
            Next
            If _UserInformationLoaded Then _CollectionButtonsColorsSet = True
        End Sub
#End Region
#Region "XML Declarations"
        Private Const Name_Site As String = UserInfo.Name_Site
        Private Const Name_Plugin As String = UserInfo.Name_Plugin
        Private Const Name_AccountName As String = UserInfo.Name_AccountName
        Protected Const Name_IsChannel As String = "IsChannel"
        Friend Const Name_UserName As String = "UserName"
        Private Const Name_Model_User As String = UserInfo.Name_Model_User
        Private Const Name_Model_Collection As String = UserInfo.Name_Model_Collection
        Private Const Name_Merged As String = UserInfo.Name_Merged
        Private Const Name_SpecialPath As String = UserInfo.Name_SpecialPath
        Private Const Name_SpecialCollectionPath As String = UserInfo.Name_SpecialCollectionPath

        Private Const Name_UserExists As String = "UserExists"
        Private Const Name_UserSuspended As String = "UserSuspended"
        Protected Const Name_FriendlyName As String = "FriendlyName"
        Protected Const Name_UserSiteName As String = "UserSiteName"
        Protected Const Name_UserID As String = "UserID"
        Protected Const Name_Options As String = "Options"
        Protected Const Name_Description As String = "Description"
        Protected Const Name_ParseUserMediaOnly As String = "ParseUserMediaOnly"
        Private Const Name_IsSubscription As String = UserInfo.Name_IsSubscription
        Private Const Name_Temporary As String = "Temporary"
        Private Const Name_Favorite As String = "Favorite"
        Private Const Name_BackColor As String = "BackColor"
        Private Const Name_ForeColor As String = "ForeColor"
        Private Const Name_CreatedByChannel As String = "CreatedByChannel"

        Private Const Name_SeparateVideoFolder As String = "SeparateVideoFolder"
        Private Const Name_CollectionName As String = UserInfo.Name_Collection
        Friend Const Name_LabelsName As String = "Labels"

        Private Const Name_ReadyForDownload As String = "ReadyForDownload"
        Private Const Name_DownloadImages As String = "DownloadImages"
        Private Const Name_DownloadVideos As String = "DownloadVideos"
        Private Const Name_IconBannerDownloaded As String = "IconBannerDownloaded"

        Private Const Name_VideoCount As String = "VideoCount"
        Private Const Name_PicturesCount As String = "PicturesCount"
        Private Const Name_LastUpdated As String = "LastUpdated"

        Private Const Name_ScriptUse As String = "ScriptUse"
        Private Const Name_ScriptData As String = "ScriptData"

        Protected Const Name_UseMD5Comparison As String = "UseMD5Comparison"
        Protected Const Name_RemoveExistingDuplicates As String = "RemoveExistingDuplicates"
        Protected Const Name_StartMD5Checked As String = "StartMD5Checked"
#Region "Additional names"
        Protected Const Name_SiteMode As String = "SiteMode"
        Protected Const Name_TrueName As String = "TrueName"
        'TODELETE Name_TrueName2
        <Obsolete> Protected Const Name_TrueName2 As String = "NameTrue"
        Protected Const Name_Arguments As String = "Arguments"
#End Region
#End Region
#Region "Declarations"
#Region "Host, Site, Progress"
        Friend Property HostCollection As SettingsHostCollection
        Private Function HostObtainCollection() As Boolean
            If HostCollection Is Nothing Then
                Dim k$ = If(_HOST?.Key, _HostKey)
                If Not k.IsEmptyString Then HostCollection = Settings(k)
            End If
            Return Not HostCollection Is Nothing
        End Function
        Private _HOST As SettingsHost
        Private _HostKey As String = String.Empty
        Private _HostObtained As Boolean = False
        Friend Property HOST As SettingsHost Implements IUserData.HOST
            Get
                If _HostObtained Or HostStatic Then
                    Return _HOST
                ElseIf HostObtainCollection() Then
                    _HOST = HostCollection(AccountName)
                    _HostObtained = Not _HOST Is Nothing
                    Return _HOST
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal h As SettingsHost)
                _HOST = h
                If Not h Is Nothing Then _HostKey = h.Key
            End Set
        End Property
        Friend Sub ResetHost()
            _HostObtained = False
        End Sub
        Friend Property HostStatic As Boolean = False Implements IUserData.HostStatic
        Private _AccountName As String = String.Empty
        Friend Overridable Property AccountName As String Implements IUserData.AccountName, IPluginContentProvider.AccountName
            Get
                Return _AccountName.IfNullOrEmpty(User.AccountName)
            End Get
            Set(ByVal name As String)
                If Not _AccountName = name Then ResetHost()
                _AccountName = name
            End Set
        End Property
        Friend ReadOnly Property Site As String Implements IUserData.Site
            Get
                Return HOST.Name
            End Get
        End Property
        Private _Progress As MyProgress
        Friend Property Progress As MyProgress
            Get
                Return _Progress
            End Get
            Set(ByVal p As MyProgress)
                _Progress = p
                If Not ProgressPre Is Nothing Then ProgressPre.Reset() : ProgressPre.Dispose()
                ProgressPre = New PreProgress(_Progress)
            End Set
        End Property
        Protected Property ProgressPre As PreProgress = Nothing
#End Region
#Region "User name, ID, exist, suspend, options"
        Friend User As UserInfo
        Friend Property IsSavedPosts As Boolean Implements IPluginContentProvider.IsSavedPosts
        Private _UserExists As Boolean = True
        Friend Overridable Property UserExists As Boolean Implements IUserData.Exists, IPluginContentProvider.UserExists
            Get
                Return _UserExists
            End Get
            Set(ByVal _UserExists As Boolean)
                If Not Me._UserExists = _UserExists Then EnvirChanged(_UserExists)
                Me._UserExists = _UserExists
            End Set
        End Property
        Private _UserSuspended As Boolean = False
        Friend Overridable Property UserSuspended As Boolean Implements IUserData.Suspended, IPluginContentProvider.UserSuspended
            Get
                Return _UserSuspended
            End Get
            Set(ByVal _UserSuspended As Boolean)
                If Not Me._UserSuspended = _UserSuspended Then EnvirChanged(_UserSuspended)
                Me._UserSuspended = _UserSuspended
            End Set
        End Property
        Private Property IPluginContentProvider_Name As String Implements IPluginContentProvider.Name
            Get
                Return Name
            End Get
            Set(ByVal NewName As String)
            End Set
        End Property
        Friend Overridable ReadOnly Property Name As String Implements IUserData.Name
            Get
                Return User.Name
            End Get
        End Property
        Private _NameTrue As String = String.Empty
        Friend Overridable Overloads Property NameTrue As String Implements IUserData.NameTrue, IPluginContentProvider.NameTrue
            Get
                Return NameTrue(False)
            End Get
            Set(ByVal NewName As String)
                If Not _NameTrue = NewName Then EnvirChanged(NewName)
                _NameTrue = NewName
            End Set
        End Property
        Friend Overloads ReadOnly Property NameTrue(ByVal Exact As Boolean) As String
            Get
                Return If(Exact, _NameTrue, _NameTrue.IfNullOrEmpty(Name))
            End Get
        End Property
        Friend Overridable Property ID As String = String.Empty Implements IUserData.ID, IPluginContentProvider.ID
        Protected _FriendlyName As String = String.Empty
        Friend Overridable Property FriendlyName As String Implements IUserData.FriendlyName
            Get
                If Settings.UserSiteNameAsFriendly Then
                    Return _FriendlyName.IfNullOrEmpty(UserSiteName)
                Else
                    Return _FriendlyName
                End If
            End Get
            Set(ByVal n As String)
                _FriendlyName = n
            End Set
        End Property
        Friend ReadOnly Property FriendlyNameOrig As String
            Get
                Return _FriendlyName
            End Get
        End Property
        Friend ReadOnly Property FriendlyNameIsSiteName As Boolean
            Get
                If Settings.UserSiteNameAsFriendly Then
                    Return Not FriendlyName.IsEmptyString And Not _FriendlyName = UserSiteName And FriendlyName = UserSiteName
                Else
                    Return False
                End If
            End Get
        End Property
        Private _UserSiteName As String = String.Empty
        Friend Property UserSiteName As String
            Get
                Return _UserSiteName
            End Get
            Set(ByVal _UserSiteName As String)
                If Not Me._UserSiteName = _UserSiteName Then EnvirChanged(_UserSiteName)
                Me._UserSiteName = _UserSiteName
            End Set
        End Property
        Protected Sub UserSiteNameUpdate(ByVal NewName As String)
            If Not NewName.IsEmptyString And (UserSiteName.IsEmptyString Or Settings.UpdateUserSiteNameEveryTime) Then UserSiteName = NewName
        End Sub
        Friend ReadOnly Property UserModel As UsageModel Implements IUserData.UserModel
            Get
                Return User.UserModel
            End Get
        End Property
        Friend Overridable ReadOnly Property CollectionModel As UsageModel Implements IUserData.CollectionModel
            Get
                Return User.CollectionModel
            End Get
        End Property
        Friend Overridable ReadOnly Property IsVirtual As Boolean Implements IUserData.IsVirtual
            Get
                Return UserModel = UsageModel.Virtual
            End Get
        End Property
        Friend Property Options As String = String.Empty Implements IUserData.Options, IPluginContentProvider.Options
        Friend Overridable ReadOnly Property FeedIsUser As Boolean
            Get
                Return True
            End Get
        End Property
#End Region
#Region "Description"
        Friend Property UserDescription As String = String.Empty Implements IUserData.Description, IPluginContentProvider.UserDescription
        Protected _DescriptionEveryTime As Boolean = False
        Protected _DescriptionChecked As Boolean = False
        Protected Function UserDescriptionNeedToUpdate() As Boolean
            Return (UserDescription.IsEmptyString Or _DescriptionEveryTime) And Not _DescriptionChecked
        End Function
        Protected Sub UserDescriptionUpdate(ByVal Descr As String, Optional ByVal Force As Boolean = False,
                                            Optional ByVal InsertFirst As Boolean = False, Optional ByVal AppendDate As Boolean = False)
            If UserDescriptionNeedToUpdate() Or Force Then
                If AppendDate Then Descr = $"{Now.ToStringDateDef}: {Descr}"
                If UserDescription.IsEmptyString Then
                    UserDescription = Descr
                    _ForceSaveUserInfo = True
                ElseIf Not UserDescription.Contains(Descr) Then
                    If InsertFirst Then
                        UserDescription = $"{Descr}{vbNewLine}{UserDescription}"
                    Else
                        UserDescription &= $"{vbNewLine}----{vbNewLine}{Descr}"
                    End If
                    _ForceSaveUserInfo = True
                End If
                _DescriptionChecked = True
            End If
        End Sub
#End Region
#Region "Favorite, Temporary, Colors"
        Protected _Favorite As Boolean = False
        Friend Overridable Property Favorite As Boolean Implements IUserData.Favorite
            Get
                Return _Favorite
            End Get
            Set(ByVal Fav As Boolean)
                _Favorite = Fav
                If _Favorite Then _Temporary = False
            End Set
        End Property
        Protected _Temporary As Boolean = False
        Friend Overridable Property Temporary As Boolean Implements IUserData.Temporary
            Get
                Return _Temporary
            End Get
            Set(ByVal Temp As Boolean)
                _Temporary = Temp
                If _Temporary Then _Favorite = False
            End Set
        End Property
        Private _BackColor As Color? = Nothing
        Friend Overridable Property BackColor As Color? Implements IUserData.BackColor
            Get
                Return _BackColor
            End Get
            Set(ByVal b As Color?)
                _BackColor = b
            End Set
        End Property
        Private _ForeColor As Color? = Nothing
        Friend Overridable Property ForeColor As Color? Implements IUserData.ForeColor
            Get
                Return _ForeColor
            End Get
            Set(ByVal f As Color?)
                _ForeColor = f
            End Set
        End Property
#End Region
#Region "Channel"
        Friend Property CreatedByChannel As Boolean = False
#End Region
#Region "Images"
        Friend Overridable Function GetUserPicture() As Image Implements IUserData.GetPicture
            If Settings.ViewModeIsPicture Then
                Return GetPicture(Of Image)()
            Else
                Return Nothing
            End If
        End Function
        Friend Function GetUserPictureToastAddress() As SFile
            Return GetPicture(Of SFile)(False, True)
        End Function
        Friend Overridable Sub SetPicture(ByVal f As SFile) Implements IUserData.SetPicture
            Try
                If f.Exists Then UserImage.NewUserPicture(f, MyFile)
            Catch
            End Try
        End Sub
        Protected Function GetNullPicture(ByVal MaxHeigh As XMLValue(Of Integer)) As Bitmap
            Return New Bitmap(CInt(DivideWithZeroChecking(MaxHeigh.Value, 100) * 75), MaxHeigh.Value)
        End Function
        Friend Function GetPicture(Of T)(Optional ByVal ReturnNullImageOnNothing As Boolean = True, Optional ByVal GetToast As Boolean = False) As T
            Dim rsfile As Boolean = GetType(T) Is GetType(SFile)
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
                        f.Delete(SFO.Path, Settings.DeleteMode, EDP.None)
                        DelPath = False
                    End If
                End If
            End If
BlockPictureScan:
            On Error GoTo BlockDeletePictureFolder
            Dim NewPicFile As SFile = SFile.GetFiles(MyFile.CutPath, "*.jpg|*.jpeg|*.png",,
                                                     New ErrorsDescriber(EDP.ReturnValue) With {
                                                         .ReturnValue = New List(Of SFile),
                                                         .ReturnValueExists = True}).FirstOrDefault
            If NewPicFile.Exists Then p = UserImage.NewUserPicture(NewPicFile, MyFile,, True) : GoTo BlockReturn
BlockDeletePictureFolder:
            On Error GoTo BlockReturn
            If DelPath Then
                f = SFile.GetPath($"{MyFile.PathWithSeparator}Pictures")
                If f.Exists(SFO.Path, False) Then f.Delete(SFO.Path, Settings.DeleteMode)
            End If
BlockReturn:
            On Error GoTo BlockNullPicture
            If Not p Is Nothing Then
                Dim i As Image = Nothing
                Dim a As SFile = Nothing
                If rsfile Then
                    If GetToast Then
                        a = p.Large.Address
                    Else
                        a = p.Address
                    End If
                Else
                    Select Case Settings.ViewMode.Value
                        Case View.LargeIcon : i = p.Large.OriginalImage.Clone
                        Case View.SmallIcon : i = p.Small.OriginalImage.Clone
                    End Select
                End If
                p.Dispose()
                If rsfile Then Return CObj(a) Else Return CObj(i)
            End If
BlockNullPicture:
            If ReturnNullImageOnNothing Then
                Select Case Settings.ViewMode.Value
                    Case View.LargeIcon : Return CObj(GetNullPicture(Settings.MaxLargeImageHeight))
                    Case View.SmallIcon : Return CObj(GetNullPicture(Settings.MaxSmallImageHeight))
                End Select
            End If
            Return Nothing
        End Function
#End Region
#Region "Separate folder"
        Friend Property SeparateVideoFolder As Boolean?
        Protected ReadOnly Property SeparateVideoFolderF As Boolean
            Get
                Return If(SeparateVideoFolder, Settings.SeparateVideoFolder.Value)
            End Get
        End Property
#End Region
#Region "Collections"
        Protected _IsCollection As Boolean = False
        Protected Friend ReadOnly Property IsCollection As Boolean Implements IUserData.IsCollection
            Get
                Return _IsCollection
            End Get
        End Property
        Friend Overridable ReadOnly Property CollectionName As String Implements IUserData.CollectionName
            Get
                Return User.CollectionName
            End Get
        End Property
        Friend Overridable ReadOnly Property CollectionPath As SFile Implements IUserData.CollectionPath
            Get
                Return User.GetCollectionRootPath
            End Get
        End Property
        Friend ReadOnly Property IncludedInCollection As Boolean Implements IUserData.IncludedInCollection
            Get
                Return User.IncludedInCollection
            End Get
        End Property
        Friend Overridable ReadOnly Property Labels As List(Of String) Implements IUserData.Labels
        Protected ReadOnly Property LabelsString As String
            Get
                Return Labels.ListToString("|", EDP.ReturnValue)
            End Get
        End Property
        Friend Overridable ReadOnly Property SpecialLabels As IEnumerable(Of String)
            Get
                Return New String() {}
            End Get
        End Property
        ''' <summary>
        ''' 0 add<br/>
        ''' 1 replace<br/>
        ''' 2 remove
        ''' </summary>
        ''' <returns>true = w/special</returns>
        Friend Shared Function UpdateLabelsKeepSpecial(ByVal Mode As Byte) As Boolean
            Dim m As New MMessage("", "Update labels",, vbQuestion + vbYesNo) With {.DefaultButton = 0, .CancelButton = 0}
            Select Case Mode
                Case 0 : m.Text = "Do you want to exclude site-specific labels from adding?"
                Case 1, 2 : m.Text = "Do you want to keep site-specific labels?"
                Case Else : Return False
            End Select
            Return m.Show = vbYes
        End Function
        ''' <inheritdoc cref="UpdateLabelsKeepSpecial(Byte)"/>
        Friend Shared Sub UpdateLabels(ByVal User As UserDataBase, ByVal NewLabels As IEnumerable(Of String), ByVal Mode As Byte, ByVal KeepSpecial As Boolean)
            Try
                If User.IsCollection Then
                    With DirectCast(User, UserDataBind)
                        If .Count > 0 Then .Collections.ForEach(Sub(u) UpdateLabels(u, NewLabels, Mode, KeepSpecial))
                    End With
                Else
                    Dim nl As List(Of String)
                    If NewLabels.ListExists Then nl = NewLabels.ToList Else nl = New List(Of String)

                    Dim lex As List(Of String) = User.SpecialLabels.ToList
                    If lex.ListExists Then
                        If User.Labels.Count = 0 Or Not KeepSpecial Then
                            lex.Clear()
                        Else
                            lex.ListDisposeRemove(Function(l) Not User.Labels.Contains(l))
                        End If
                    End If

                    Select Case Mode
                        Case 0 'add
                            If KeepSpecial Then nl.ListAddList(lex, LNC)
                            User.Labels.ListAddList(nl, LNC)
                        Case 1 'replace
                            If KeepSpecial Then
                                nl.ListAddList(lex, LNC)
                            Else
                                nl.ListWithRemove(lex)
                            End If
                            User.Labels.Clear()
                            User.Labels.ListAddList(nl, LNC)
                        Case 2 'remove
                            If KeepSpecial Then nl.ListWithRemove(lex)
                            User.Labels.ListWithRemove(nl)
                    End Select

                    If User.Labels.Count > 0 Then User.Labels.Sort()
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[UserDataBase.UpdateLabels]")
            End Try
        End Sub
#End Region
#Region "Downloading"
        Protected _DataLoaded As Boolean = False
        Protected _DataParsed As Boolean = False
        Friend Property ParseUserMediaOnly As Boolean = False Implements IUserData.ParseUserMediaOnly, IPluginContentProvider.ParseUserMediaOnly
        Friend Overridable ReadOnly Property IsSubscription As Boolean Implements IUserData.IsSubscription
            Get
                Return User.IsSubscription
            End Get
        End Property
        Friend Overridable ReadOnly Property IsUser As Boolean Implements IUserData.IsUser
            Get
                Return True
            End Get
        End Property
        Private Property IPluginContentProvider_IsSubscription As Boolean Implements IPluginContentProvider.IsSubscription
            Get
                Return IsSubscription
            End Get
            Set : End Set
        End Property
        Friend Overridable Property ReadyForDownload As Boolean = True Implements IUserData.ReadyForDownload
        Friend Property DownloadImages As Boolean = True Implements IUserData.DownloadImages
        Friend Property DownloadVideos As Boolean = True Implements IUserData.DownloadVideos
        Friend Property DownloadMissingOnly As Boolean = False Implements IUserData.DownloadMissingOnly
        Private _IconBannerDownloaded As Boolean = False
        Friend WriteOnly Property IconBannerDownloaded As Boolean
            Set(ByVal IsDownloaded As Boolean)
                If Not _IconBannerDownloaded = IsDownloaded Then _ForceSaveUserInfo = True
                _IconBannerDownloaded = IsDownloaded
            End Set
        End Property
        Friend ReadOnly Property DownloadIconBanner As Boolean
            Get
                Return Not _IconBannerDownloaded Or Settings.UpdateUserIconBannerEveryTime
            End Get
        End Property
#End Region
#Region "Content"
        Protected ReadOnly _ContentList As List(Of UserMedia)
        Protected ReadOnly _ContentNew As List(Of UserMedia)
        Friend ReadOnly Property LatestData As List(Of UserMedia)
        Protected ReadOnly MissingFinder As Predicate(Of UserMedia) = Function(c) c.State = UStates.Missing
        Friend ReadOnly Property ContentMissing As List(Of UserMedia)
            Get
                If _ContentList.Count > 0 Then
                    Return _ContentList.Where(Function(c) MissingFinder(c)).ListIfNothing
                Else
                    Return New List(Of UserMedia)
                End If
            End Get
        End Property
        Friend Overridable ReadOnly Property ContentMissingExists As Boolean
            Get
                Return _ContentList.Exists(MissingFinder)
            End Get
        End Property
        Friend Sub RemoveMedia(ByVal m As UserMedia, ByVal State As UStates?)
            Dim i% = If(State.HasValue, _ContentList.FindIndex(Function(mm) mm.State = State.Value And mm.Equals(m)), _ContentList.IndexOf(m))
            If i >= 0 Then _ContentList.RemoveAt(i)
        End Sub
        Protected ReadOnly _TempMediaList As List(Of UserMedia)
        Protected ReadOnly _TempPostsList As List(Of String)
        Private ReadOnly _MD5List As List(Of String)
        Friend Function GetLastImageAddress() As SFile
            If _ContentList.Count > 0 Then
                Return _ContentList.LastOrDefault(Function(c) c.Type = UTypes.Picture And Not c.File.IsEmptyString And Not c.File.Extension = "gif").File
            Else
                Return Nothing
            End If
        End Function
#End Region
#Region "Files"
        Friend Overridable Property MyFile As SFile Implements IUserData.File
            Get
                If IsSavedPosts Then
                    Return MyFileSettings
                Else
                    Return User.File
                End If
            End Get
            Set(ByVal f As SFile)
                User.File = f
                Settings.UpdateUsersList(User)
            End Set
        End Property
        Protected MyFileSettings As SFile
        Protected MyFileData As SFile
        Protected MyFilePosts As SFile
        Private MyMD5File As SFile
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
#Region "Information, counters, error, update date"
        Friend Overridable Property LastUpdated As Date?
        Friend Overridable Property HasError As Boolean = False Implements IUserData.HasError
        Private _DownloadedPicturesTotal As Integer = 0
        Private _DownloadedPicturesSession As Integer = 0
        Friend Property DownloadedPictures(ByVal Total As Boolean) As Integer Implements IUserData.DownloadedPictures
            Get
                Return IIf(Total, _DownloadedPicturesTotal, _DownloadedPicturesSession)
            End Get
            Set(ByVal NewValue As Integer)
                If Total Then
                    _DownloadedPicturesTotal = NewValue
                Else
                    _DownloadedPicturesSession = NewValue
                End If
            End Set
        End Property
        Private _DownloadedVideosTotal As Integer = 0
        Private _DownloadedVideosSession As Integer = 0
        Friend Property DownloadedVideos(ByVal Total As Boolean) As Integer Implements IUserData.DownloadedVideos
            Get
                Return IIf(Total, _DownloadedVideosTotal, _DownloadedVideosSession)
            End Get
            Set(ByVal NewValue As Integer)
                If Total Then
                    _DownloadedVideosTotal = NewValue
                Else
                    _DownloadedVideosSession = NewValue
                End If
            End Set
        End Property
        Friend Overridable ReadOnly Property DownloadedTotal(Optional ByVal Total As Boolean = True) As Integer Implements IUserData.DownloadedTotal
            Get
                Return DownloadedPictures(Total) + DownloadedVideos(Total)
            End Get
        End Property
        Friend ReadOnly Property DownloadedInformation As String Implements IUserData.DownloadedInformation
            Get
                Dim luv$ = String.Empty
                If LastUpdated.HasValue Then luv = $"{LastUpdated.Value.ToStringDate(ADateTime.Formats.BaseDateTime)}: "
                Return $"{luv}{Name} [{Site}]{IIf(HasError, " (with errors)", String.Empty)}: P - {DownloadedPictures(False)}; V - {DownloadedVideos(False)}" &
                       $" (P - {DownloadedPictures(True)}; V - {DownloadedVideos(True)})"
            End Get
        End Property
        Friend Overridable Function GetUserInformation() As String
            Dim OutStr$ = $"User: {Name} (site: {Site}"
            If IncludedInCollection Then
                OutStr &= $"; collection: {CollectionName}"
                If CollectionModel = UsageModel.Default And UserModel = UsageModel.Virtual Then OutStr &= "; virtual"
            End If
            OutStr &= ")"
            OutStr.StringAppendLine($"Labels: {Labels.ListToString}")
            OutStr.StringAppendLine($"Path: {MyFile.CutPath.Path}")
            OutStr.StringAppendLine($"Total downloaded ({DownloadedTotal(True).NumToString(ANumbers.Formats.Number, 3)}):")
            OutStr.StringAppendLine($"Pictures: {DownloadedPictures(True).NumToString(ANumbers.Formats.Number, 3)}")
            OutStr.StringAppendLine($"Videos: {DownloadedVideos(True).NumToString(ANumbers.Formats.Number, 3)}")
            If Not UserDescription.IsEmptyString Then
                OutStr.StringAppendLine(String.Empty)
                OutStr.StringAppendLine(UserDescription)
            End If
            OutStr.StringAppendLine(String.Empty)
            OutStr.StringAppendLine($"Last updated at: {AConvert(Of String)(LastUpdated, ADateTime.Formats.BaseDateTime, "not yet")}")
            If _DataParsed Then
                OutStr.StringAppendLine("Downloaded now:")
                OutStr.StringAppendLine($"Pictures: {DownloadedTotal(False).NumToString(ANumbers.Formats.Number, 3)}")
                OutStr.StringAppendLine($"Videos: {DownloadedVideos(False).NumToString(ANumbers.Formats.Number, 3)}")
            End If
            Return OutStr
        End Function
#End Region
#Region "Script"
        Friend Overridable Property ScriptUse As Boolean = False Implements IUserData.ScriptUse
        Friend Overridable Property ScriptData As String Implements IUserData.ScriptData
#End Region
#End Region
#Region "Plugins Support"
        Protected Event ProgressChanged As IPluginContentProvider.ProgressChangedEventHandler Implements IPluginContentProvider.ProgressChanged
        Protected Event ProgressMaximumChanged As IPluginContentProvider.ProgressMaximumChangedEventHandler Implements IPluginContentProvider.ProgressMaximumChanged
        Protected Event ProgressPreChanged As IPluginContentProvider.ProgressChangedEventHandler Implements IPluginContentProvider.ProgressPreChanged
        Protected Event ProgressPreMaximumChanged As IPluginContentProvider.ProgressMaximumChangedEventHandler Implements IPluginContentProvider.ProgressPreMaximumChanged
        Private Property IPluginContentProvider_Settings As ISiteSettings Implements IPluginContentProvider.Settings
            Get
                Return HOST.Source
            End Get
            Set(ByVal s As ISiteSettings)
            End Set
        End Property
        Private Property IPluginContentProvider_Thrower As IThrower Implements IPluginContentProvider.Thrower
        Private Property IPluginContentProvider_LogProvider As ILogProvider Implements IPluginContentProvider.LogProvider
        Friend Property ExternalPlugin As IPluginContentProvider
        Private Property IPluginContentProvider_ExistingContentList As List(Of IUserMedia) Implements IPluginContentProvider.ExistingContentList
        Private Property IPluginContentProvider_TempPostsList As List(Of String) Implements IPluginContentProvider.TempPostsList
        Private Property IPluginContentProvider_TempMediaList As List(Of IUserMedia) Implements IPluginContentProvider.TempMediaList
        Private Property IPluginContentProvider_SeparateVideoFolder As Boolean Implements IPluginContentProvider.SeparateVideoFolder
        Private Property IPluginContentProvider_DataPath As String Implements IPluginContentProvider.DataPath
        Private Sub IPluginContentProvider_XmlFieldsSet(ByVal Fields As List(Of KeyValuePair(Of String, String))) Implements IPluginContentProvider.XmlFieldsSet
        End Sub
        Private Function IPluginContentProvider_XmlFieldsGet() As List(Of KeyValuePair(Of String, String)) Implements IPluginContentProvider.XmlFieldsGet
            Return Nothing
        End Function
        Private Sub IPluginContentProvider_GetMedia(ByVal Token As CancellationToken) Implements IPluginContentProvider.GetMedia
        End Sub
        Private Sub IPluginContentProvider_Download(ByVal Token As CancellationToken) Implements IPluginContentProvider.Download
        End Sub
        Private Sub IPluginContentProvider_DownloadSingleObject(ByVal Data As IDownloadableMedia, ByVal Token As CancellationToken) Implements IPluginContentProvider.DownloadSingleObject
        End Sub
        Friend Overridable Function ExchangeOptionsGet() As Object Implements IPluginContentProvider.ExchangeOptionsGet
            Return Nothing
        End Function
        Friend Overridable Sub ExchangeOptionsSet(ByVal Obj As Object) Implements IPluginContentProvider.ExchangeOptionsSet
        End Sub
#End Region
#Region "IIndexable Support"
        Friend Property Index As Integer = 0 Implements IIndexable.Index
        Private Function SetIndex(ByVal Obj As Object, ByVal Index As Integer) As Object Implements IIndexable.SetIndex
            DirectCast(Obj, UserDataBase).Index = Index
            Return Obj
        End Function
#End Region
#Region "LVI"
        Friend ReadOnly Property LVIKey As String Implements IUserData.Key
            Get
                If Not _IsCollection Then
                    Return $"{IIf(IsSubscription, "SSSS", String.Empty)}{Site.ToString.ToUpper}_{Name}"
                Else
                    Return $"CCCC_{CollectionName}"
                End If
            End Get
        End Property
        Friend Function GetLVI(ByVal Destination As ListView) As ListViewItem Implements IUserData.GetLVI
            If Settings.ViewModeIsPicture Then
                Return ListImagesLoader.ApplyLVIColor(Me, New ListViewItem(ToString(), LVIKey, GetLVIGroup(Destination)) With {.Name = LVIKey, .Tag = LVIKey}, True)
            Else
                Return ListImagesLoader.ApplyLVIColor(Me, New ListViewItem(ToString(), GetLVIGroup(Destination)) With {.Name = LVIKey, .Tag = LVIKey}, True)
            End If
        End Function
        Friend Function GetLVIGroup(ByVal Destination As ListView) As ListViewGroup Implements IUserData.GetLVIGroup
            Try
                With Settings
                    If Not .ShowAllUsers.Value AndAlso (.AdvancedFilter.Labels.Count > 0 Or .AdvancedFilter.LabelsNo) AndAlso Not .ShowGroupsInsteadLabels Then
                        If Labels.Count > 0 And .AdvancedFilter.Labels.Count > 0 Then
                            For i% = 0 To Labels.Count - 1
                                If .AdvancedFilter.Labels.Contains(Labels(i)) Then Return Destination.Groups.Item(Labels(i))
                            Next
                        End If
                    ElseIf Settings.GroupUsers Then
                        Return Destination.Groups.Item(GetLviGroupName(HOST, Temporary, Favorite, IsCollection))
                    End If
                End With
                Return Destination.Groups.Item(LabelsKeeper.NoLabeledName)
            Catch ex As Exception
                Return Destination.Groups.Item(LabelsKeeper.NoLabeledName)
            End Try
        End Function
#End Region
#Region "Initializer"
        ''' <summary>By using this constructor you must set UserName and MyFile manually</summary>
        Friend Sub New(Optional ByVal InvokeImageHandler As Boolean = True)
            _ContentList = New List(Of UserMedia)
            _ContentNew = New List(Of UserMedia)
            LatestData = New List(Of UserMedia)
            _TempMediaList = New List(Of UserMedia)
            _TempPostsList = New List(Of String)
            _MD5List = New List(Of String)
            Labels = New List(Of String)
            UserUpdatedEventHandlers = New List(Of IUserData.UserUpdatedEventHandler)
            UserDownloadStateChangedEventHandlers = New List(Of UserDownloadStateChangedEventHandler)
            If InvokeImageHandler Then MainFrameObj.ImageHandler(Me)
        End Sub
        Friend Sub SetEnvironment(ByRef h As SettingsHost, ByVal u As UserInfo, ByVal _LoadUserInformation As Boolean,
                                  Optional ByVal AttachUserInfo As Boolean = True) Implements IUserData.SetEnvironment
            HOST = h
            HostObtainCollection()
            If AttachUserInfo Then
                User = u
                If _LoadUserInformation Then LoadUserInformation()
            End If
        End Sub
        ''' <exception cref="ArgumentOutOfRangeException"></exception>
        Friend Shared Function GetInstance(ByVal u As UserInfo, Optional ByVal _LoadUserInformation As Boolean = True) As IUserData
            If Not u.Plugin.IsEmptyString Then
                Return Settings(u.Plugin).Default.GetInstance(ISiteSettings.Download.Main, u, _LoadUserInformation)
            Else
                Throw New ArgumentOutOfRangeException("Plugin", $"Plugin [{u.Plugin}] information does not recognized by loader")
            End If
        End Function
        Friend Shared Function GetPostUrl(ByVal u As IUserData, ByVal PostData As UserMedia) As String
            Dim uName$ = String.Empty
            Try
                If Not u Is Nothing AndAlso Not u.IsCollection Then
                    With DirectCast(u, UserDataBase)
                        If Not .User.Plugin.IsEmptyString Then
                            uName = .User.Name
                            Return Settings(.User.Plugin).Default.GetUserPostUrl(.Self, PostData)
                        End If
                    End With
                End If
                Return String.Empty
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, $"GetPostUrl({uName}, {PostData.Post.ID})", String.Empty)
            End Try
        End Function
#End Region
#Region "Information & Content data files loader and saver"
#Region "User information"
        Private _UserInformationLoaded As Boolean = False
        Friend Overridable Sub LoadUserInformation() Implements IUserData.LoadUserInformation
            Try
                UpdateDataFiles()
                If MyFileSettings.Exists Then
                    FileExists = True
                    Using x As New XmlFile(MyFileSettings) With {.XmlReadOnly = True}
                        If User.Name.IsEmptyString Then User.Name = x.Value(Name_UserName)
                        _NameTrue = x.Value(Name_TrueName)
#Disable Warning BC40008
                        If _NameTrue.IsEmptyString AndAlso x.Contains(Name_TrueName2) Then _NameTrue = x.Value(Name_TrueName2)
#Enable Warning
                        UserExists = x.Value(Name_UserExists).FromXML(Of Boolean)(True)
                        UserSuspended = x.Value(Name_UserSuspended).FromXML(Of Boolean)(False)
                        ID = x.Value(Name_UserID)
                        Options = x.Value(Name_Options)
                        _FriendlyName = x.Value(Name_FriendlyName)
                        UserSiteName = x.Value(Name_UserSiteName)
                        UserDescription = x.Value(Name_Description)
                        ParseUserMediaOnly = x.Value(Name_ParseUserMediaOnly).FromXML(Of Boolean)(False)
                        Temporary = x.Value(Name_Temporary).FromXML(Of Boolean)(False)
                        Favorite = x.Value(Name_Favorite).FromXML(Of Boolean)(False)

                        If Not x.Value(Name_BackColor).IsEmptyString Then
                            BackColor = AConvert(Of Color)(x.Value(Name_BackColor), Nothing, EDP.ReturnValue)
                        Else
                            BackColor = Nothing
                        End If
                        If Not x.Value(Name_ForeColor).IsEmptyString Then
                            ForeColor = AConvert(Of Color)(x.Value(Name_ForeColor), Nothing, EDP.ReturnValue)
                        Else
                            ForeColor = Nothing
                        End If

                        CreatedByChannel = x.Value(Name_CreatedByChannel).FromXML(Of Boolean)(False)
                        SeparateVideoFolder = AConvert(Of Boolean)(x.Value(Name_SeparateVideoFolder), AModes.Var, Nothing)
                        ReadyForDownload = x.Value(Name_ReadyForDownload).FromXML(Of Boolean)(True)
                        DownloadImages = x.Value(Name_DownloadImages).FromXML(Of Boolean)(True)
                        DownloadVideos = x.Value(Name_DownloadVideos).FromXML(Of Boolean)(True)
                        _IconBannerDownloaded = x.Value(Name_IconBannerDownloaded).FromXML(Of Boolean)(False)
                        DownloadedVideos(True) = x.Value(Name_VideoCount).FromXML(Of Integer)(0)
                        DownloadedPictures(True) = x.Value(Name_PicturesCount).FromXML(Of Integer)(0)
                        LastUpdated = AConvert(Of Date)(x.Value(Name_LastUpdated), ADateTime.Formats.BaseDateTime, Nothing)
                        ScriptUse = x.Value(Name_ScriptUse).FromXML(Of Boolean)(False)
                        ScriptData = x.Value(Name_ScriptData)
                        DataMerging = x.Value(Name_Merged).FromXML(Of Boolean)(False)
                        Labels.ListAddList(x.Value(Name_LabelsName).StringToList(Of String, List(Of String))("|", EDP.ReturnValue), LAP.NotContainsOnly, LAP.ClearBeforeAdd)
                        LoadUserInformation_OptionalFields(x, True)
                    End Using
                    UpdateDataFiles()
                    _UserInformationLoaded = True
                    If _CollectionButtonsExists And Not _CollectionButtonsColorsSet And (Not UserExists Or UserSuspended) Then UpdateButtonsColor()
                End If
            Catch ex As Exception
                LogError(ex, "user information loading error")
            End Try
        End Sub
        Private Sub UpdateUserInformation_Ex()
            If _ForceSaveUserInfoOnException Then UpdateUserInformation()
        End Sub
        Friend Overridable Overloads Sub UpdateUserInformation() Implements IUserData.UpdateUserInformation
            UpdateUserInformation(False)
        End Sub
        Friend Overridable Overloads Sub UpdateUserInformation(ByVal DisableUserInfoUpdate As Boolean)
            Try
                UpdateDataFiles()
                MyFileSettings.Exists(SFO.Path)
                Using x As New XmlFile With {.Name = "User"}
                    x.Add(Name_Site, Site)
                    x.Add(Name_Plugin, HOST.Key)
                    x.Add(Name_AccountName, AccountName)
                    x.Add(Name_UserName, User.Name)
                    x.Add(Name_TrueName, _NameTrue)
                    x.Add(Name_Model_User, CInt(UserModel))
                    x.Add(Name_Model_Collection, CInt(CollectionModel))
                    x.Add(Name_SpecialPath, User.SpecialPath)
                    x.Add(Name_SpecialCollectionPath, User.SpecialCollectionPath)
                    x.Add(Name_UserExists, UserExists.BoolToInteger)
                    x.Add(Name_UserSuspended, UserSuspended.BoolToInteger)
                    x.Add(Name_UserID, ID)
                    x.Add(Name_Options, Options)
                    x.Add(Name_FriendlyName, _FriendlyName)
                    x.Add(Name_UserSiteName, UserSiteName)
                    x.Add(Name_Description, UserDescription)
                    x.Add(Name_ParseUserMediaOnly, ParseUserMediaOnly.BoolToInteger)
                    x.Add(Name_IsSubscription, IsSubscription.BoolToInteger)
                    x.Add(Name_Temporary, Temporary.BoolToInteger)
                    x.Add(Name_Favorite, Favorite.BoolToInteger)

                    x.Add(Name_BackColor, CStr(AConvert(Of String)(BackColor, String.Empty, EDP.ReturnValue)))
                    x.Add(Name_ForeColor, CStr(AConvert(Of String)(ForeColor, String.Empty, EDP.ReturnValue)))

                    x.Add(Name_CreatedByChannel, CreatedByChannel.BoolToInteger)
                    If SeparateVideoFolder.HasValue Then
                        x.Add(Name_SeparateVideoFolder, SeparateVideoFolder.Value.BoolToInteger)
                    Else
                        x.Add(Name_SeparateVideoFolder, String.Empty)
                    End If
                    x.Add(Name_ReadyForDownload, ReadyForDownload.BoolToInteger)
                    x.Add(Name_DownloadImages, DownloadImages.BoolToInteger)
                    x.Add(Name_DownloadVideos, DownloadVideos.BoolToInteger)
                    x.Add(Name_IconBannerDownloaded, _IconBannerDownloaded.BoolToInteger)
                    x.Add(Name_VideoCount, DownloadedVideos(True))
                    x.Add(Name_PicturesCount, DownloadedPictures(True))
                    x.Add(Name_LastUpdated, AConvert(Of String)(LastUpdated, ADateTime.Formats.BaseDateTime, String.Empty))
                    x.Add(Name_ScriptUse, ScriptUse.BoolToInteger)
                    x.Add(Name_ScriptData, ScriptData)
                    x.Add(Name_CollectionName, CollectionName)
                    x.Add(Name_LabelsName, LabelsString)
                    x.Add(Name_Merged, DataMerging.BoolToInteger)

                    LoadUserInformation_OptionalFields(x, False)

                    x.Save(MyFileSettings)
                End Using
                If Not IsSavedPosts And Not DisableUserInfoUpdate Then Settings.UpdateUsersList(User, True)
            Catch ex As Exception
                LogError(ex, "user information saving error")
            End Try
        End Sub
        ''' <param name="Loading"><see langword="True"/>: Loading; <see langword="False"/>: Saving</param>
        Protected MustOverride Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
#End Region
#Region "User data"
        Friend Overridable Overloads Sub LoadContentInformation(Optional ByVal Force As Boolean = False)
            Try
                UpdateDataFiles()
                If Not MyFileData.Exists Or (_DataLoaded And Not Force) Then Exit Sub
                Using x As New XmlFile(MyFileData, Protector.Modes.All, False) With {.XmlReadOnly = True, .AllowSameNames = True}
                    x.LoadData()
                    If x.Count > 0 Then
                        For Each v As EContainer In x : _ContentList.Add(New UserMedia(v, Me)) : Next
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
                    If _ContentList.Count > 0 Then x.AddRange(_ContentList)
                    x.Save(MyFileData)
                End Using
                If Not MyMD5File.IsEmptyString And _MD5List.Count > 0 Then _
                   TextSaver.SaveTextToFile(_MD5List.ListToString(Environment.NewLine), MyMD5File, True,, EDP.None)
            Catch ex As Exception
                LogError(ex, "history saving error")
            End Try
        End Sub
#End Region
#End Region
#Region "Open site, folder"
        Friend Overridable Sub OpenSite(Optional ByVal e As ErrorsDescriber = Nothing) Implements IUserData.OpenSite
            Try
                Dim URL$ = HOST.Source.GetUserUrl(Me)
                If Not URL.IsEmptyString Then Process.Start(URL)
            Catch ex As Exception
                If Not e.Exists Then e = New ErrorsDescriber(EDP.ShowAllMsg)
                MsgBoxE({$"Error when trying to open [{Site}] page of user [{Name}]", $"User [{ToString()}]"}, MsgBoxStyle.Critical, e, ex)
            End Try
        End Sub
        Friend Overridable Sub OpenFolder() Implements IUserData.OpenFolder
            If MyFile.IsEmptyString And IsSavedPosts Then UpdateDataFiles()
            GlobalOpenPath(MyFile.CutPath)
        End Sub
#End Region
#Region "Download limits"
        Protected Enum DateResult : [Continue] : [Skip] : [Exit] : End Enum
        Friend Overridable Property DownloadTopCount As Integer? = Nothing Implements IUserData.DownloadTopCount, IPluginContentProvider.PostsNumberLimit
        Friend Overridable Property IncludeInTheFeed As Boolean = True
        Private _DownloadDateFrom As Date? = Nothing
        Private _DownloadDateFromF As Date
        Friend Overridable Property DownloadDateFrom As Date? Implements IUserData.DownloadDateFrom, IPluginContentProvider.DownloadDateFrom
            Get
                Return _DownloadDateFrom
            End Get
            Set(ByVal d As Date?)
                _DownloadDateFrom = d
                If _DownloadDateFrom.HasValue Then _DownloadDateFromF = _DownloadDateFrom.Value.Date Else _DownloadDateFromF = Date.MinValue.Date
            End Set
        End Property
        Private _DownloadDateTo As Date? = Nothing
        Private _DownloadDateToF As Date
        Friend Overridable Property DownloadDateTo As Date? Implements IUserData.DownloadDateTo, IPluginContentProvider.DownloadDateTo
            Get
                Return _DownloadDateTo
            End Get
            Set(ByVal d As Date?)
                _DownloadDateTo = d
                If _DownloadDateTo.HasValue Then _DownloadDateToF = _DownloadDateTo.Value Else _DownloadDateToF = Date.MaxValue.Date
            End Set
        End Property
        Protected Function CheckDatesLimit(ByVal DateObj As Object, ByVal DateProvider As IFormatProvider) As DateResult
            Try
                If (DownloadDateFrom.HasValue Or DownloadDateTo.HasValue) AndAlso ACheck(DateObj) Then
                    Dim td As Date? = AConvert(DateObj, AModes.Var, GetType(Date),, True, Nothing, DateProvider)
                    If td.HasValue Then
                        If td.Value.ValueBetween(_DownloadDateFromF, _DownloadDateToF) Then
                            Return DateResult.Continue
                        ElseIf td.Value > _DownloadDateToF Then
                            Return DateResult.Skip
                        Else
                            Return DateResult.Exit
                        End If
                    End If
                End If
                Return DateResult.Continue
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, $"[UserDataBase.CheckDatesLimit({If(TypeOf DateObj Is String, CStr(DateObj), "?")})]", DateResult.Continue)
            End Try
        End Function
#End Region
#Region "Download functions and options"
        Private __DOWNLOAD_IN_PROGRESS As Boolean = False
        Friend ReadOnly Property DownloadInProgress As Boolean
            Get
                Return __DOWNLOAD_IN_PROGRESS
            End Get
        End Property
        Private TokenQueue As CancellationToken
        Friend TokenPersonal As CancellationToken
        Protected Responser As Responser
        Protected UseResponserClient As Boolean = False
        Protected UseClientTokens As Boolean = False
        Protected _ForceSaveUserData As Boolean = False
        Protected _ForceSaveUserInfo As Boolean = False
        Protected _ForceSaveUserInfoOnException As Boolean = False
        Private _DownloadInProgress As Boolean = False
        Private _EnvirUserExists As Boolean
        Private _EnvirUserSuspended As Boolean
        Private _EnvirCreatedByChannel As Boolean
        Private _EnvirChanged As Boolean = False
        Private _PictureExists As Boolean
        Private _EnvirInvokeUserUpdated As Boolean = False
        Protected _ResponserAutoUpdateCookies As Boolean = False
        Protected _ResponserAddResponseReceivedHandler As Boolean = False
        Protected Sub EnvirDownloadSet()
            TokenPersonal = Nothing
            ProgressPre.Reset()
            UpdateDataFiles()
            _MD5Loaded = False
            _DownloadInProgress = True
            _DescriptionChecked = False
            _DescriptionEveryTime = Settings.UpdateUserDescriptionEveryTime
            _ForceSaveUserData = False
            _ForceSaveUserInfo = False
            _ForceSaveUserInfoOnException = False
            _EnvirUserExists = UserExists
            _EnvirUserSuspended = UserSuspended
            _EnvirCreatedByChannel = CreatedByChannel
            _EnvirChanged = False
            _EnvirInvokeUserUpdated = False
            UserExists = True
            UserSuspended = False
            DownloadedPictures(False) = 0
            DownloadedVideos(False) = 0
            _PictureExists = Settings.ViewModeIsPicture AndAlso Not GetPicture(Of Image)(False) Is Nothing
        End Sub
        Private Sub EnvirChanged(ByVal NewValue As Object, <CallerMemberName> Optional ByVal Caller As String = Nothing)
            If _DownloadInProgress Then
                Select Case Caller
                    Case NameOf(UserExists) : If Not _EnvirUserExists = CBool(NewValue) Then _EnvirChanged = True : _EnvirInvokeUserUpdated = True
                    Case NameOf(UserSuspended) : If Not _EnvirUserSuspended = CBool(NewValue) Then _EnvirChanged = True : _EnvirInvokeUserUpdated = True
                    Case NameOf(NameTrue) : _EnvirChanged = True : _EnvirInvokeUserUpdated = True : _ForceSaveUserInfo = True : _ForceSaveUserInfoOnException = True
                    Case Else : _EnvirChanged = True
                End Select
            End If
        End Sub
        Friend Overridable Sub DownloadData(ByVal Token As CancellationToken) Implements IUserData.DownloadData
            ResetHost()
            __DOWNLOAD_IN_PROGRESS = True
            OnUserDownloadStateChanged(True)
            Dim Canceled As Boolean = False
            TokenQueue = Token
            Try
                If HOST Is Nothing Then Throw New ExitException($"Host '{AccountName}' not found")
                EnvirDownloadSet()
                If Not Responser Is Nothing Then Responser.Dispose()
                Responser = New Responser
                If Not HOST.Responser Is Nothing Then Responser.Copy(HOST.Responser)
                If Not Responser Is Nothing And (_ResponserAutoUpdateCookies Or _ResponserAddResponseReceivedHandler) Then
                    If _ResponserAutoUpdateCookies Then
                        Responser.CookiesUpdateMode = CookieUpdateModes.ReplaceByNameAll
                        Responser.CookiesExtractMode = Responser.CookiesExtractModes.Any
                        Responser.CookiesExtractedAutoSave = False
                    End If
                    If _ResponserAddResponseReceivedHandler Then AddHandler Responser.ResponseReceived, AddressOf Responser_ResponseReceived
                End If
                Responser.DecodersError = New ErrorsDescriber(EDP.SendToLog + EDP.ReturnValue) With {
                    .DeclaredMessage = New MMessage($"SymbolsConverter error: [{ToStringForLog()}]", ToStringForLog())}

                Dim _downContent As Func(Of UserMedia, Boolean) = Function(c) c.State = UStates.Downloaded
                _TempMediaList.Clear()
                _TempPostsList.Clear()
                LatestData.Clear()
                Dim __isChannelsSupport As Boolean = CreatedByChannel And Settings.FromChannelDownloadTopUse

                LoadContentInformation()

                If MyFilePosts.Exists Then _TempPostsList.ListAddList(File.ReadAllLines(MyFilePosts))
                If _ContentList.Count > 0 Then _TempPostsList.ListAddList(_ContentList.Select(Function(u) u.Post.ID), LNC)

                If Not DownloadMissingOnly Or IsSubscription Then
                    ThrowAny(Token)
                    DownloadDataF(Token)
                    ProgressPre.Done()
                    ThrowAny(Token)
                    If Settings.ReparseMissingInTheRoutine Then ReparseMissing(Token) : ProgressPre.Done() : ThrowAny(Token)
                Else
                    ReparseMissing(Token)
                    ProgressPre.Done()
                End If

                If _TempMediaList.Count > 0 Then
                    If Not DownloadImages Then _TempMediaList.RemoveAll(Function(m) m.Type = UTypes.GIF Or m.Type = UTypes.Picture)
                    If Not DownloadVideos Then _TempMediaList.RemoveAll(Function(m) m.Type = UTypes.Video Or
                                                                                    m.Type = UTypes.VideoPre Or m.Type = UTypes.m3u8)
                    If DownloadMissingOnly Then _TempMediaList.RemoveAll(Function(m) Not m.State = UStates.Missing)
                End If

                ReparseVideo(Token)
                ProgressPre.Done()
                ThrowAny(Token)

                If RemoveExistingDuplicates And Not IsSubscription Then ValidateMD5(Token) : ProgressPre.Done() : ThrowAny(Token)

                If _TempPostsList.Count > 0 And Not DownloadMissingOnly And Not __isChannelsSupport Then
                    If _TempPostsList.Count > 1000 Then _TempPostsList.ListAddList(_TempPostsList.ListTake(-2, 1000, EDP.ReturnValue).ListReverse, LAP.ClearBeforeAdd)
                    TextSaver.SaveTextToFile(_TempPostsList.ListToString(Environment.NewLine), MyFilePosts, True,, EDP.None)
                End If

                _ContentNew.ListAddList(_TempMediaList, LAP.ClearBeforeAdd)
                If IsSubscription Then
                    _ContentNew.ListAddList(_ContentNew.ListForEachCopy(Function(ByVal tmpC As UserMedia, ByVal ii As Integer) As UserMedia
                                                                            tmpC.State = UStates.Downloaded
                                                                            If tmpC.Type = UTypes.Picture Or tmpC.Type = UTypes.GIF Then
                                                                                DownloadedPictures(False) += 1
                                                                            Else
                                                                                DownloadedVideos(False) += 1
                                                                            End If
                                                                            Return tmpC
                                                                        End Function))
                Else
                    DownloadContent(Token)
                    ThrowIfDisposed()
                End If

                CreatedByChannel = False

                If IncludeInTheFeed Or IsSubscription Then LatestData.ListAddList(_ContentNew.Where(_downContent), LNC)
                Dim mcb& = If(ContentMissingExists, _ContentList.LongCount(Function(c) MissingFinder(c)), 0)
                _ContentList.ListAddList(_ContentNew.Where(Function(c) _downContent(c) Or MissingFinder(c)), LNC)
                Dim mca& = If(ContentMissingExists, _ContentList.LongCount(Function(c) MissingFinder(c)), 0)
                If DownloadedTotal(False) > 0 Or _EnvirChanged Or Not mcb = mca Or _ForceSaveUserData Then
                    If Not __isChannelsSupport Then
                        If DownloadedTotal(False) > 0 Then
                            LastUpdated = Now
                            RunScript()
                        End If
                        DownloadedPictures(True) = SFile.GetFiles(MyFile.CutPath, "*.jpg|*.jpeg|*.png|*.gif|*.webm",, EDP.ReturnValue).Count
                        DownloadedVideos(True) = SFile.GetFiles(MyFile.CutPath, "*.mp4|*.mkv|*.mov", SearchOption.AllDirectories, EDP.ReturnValue).Count
                        If Labels.Contains(LabelsKeeper.NoParsedUser) Then Labels.Remove(LabelsKeeper.NoParsedUser)
                        UpdateContentInformation()
                    Else
                        DownloadedVideos(False) = 0
                        DownloadedPictures(False) = 0
                        _ContentList.Clear()
                        CreatedByChannel = False
                    End If
                    UpdateUserInformation()
                    If _CollectionButtonsExists AndAlso _EnvirChanged Then UpdateButtonsColor()
                ElseIf _ForceSaveUserInfo Or __isChannelsSupport Or Not _EnvirCreatedByChannel = CreatedByChannel Then
                    UpdateUserInformation()
                End If
                ThrowIfDisposed()
                If Not _PictureExists Or _EnvirInvokeUserUpdated Then OnUserUpdated()
            Catch oex As OperationCanceledException When Token.IsCancellationRequested Or TokenPersonal.IsCancellationRequested Or TokenQueue.IsCancellationRequested
                UpdateUserInformation_Ex()
                MyMainLOG = $"{ToStringForLog()}: downloading canceled"
                Canceled = True
            Catch exit_ex As ExitException
                UpdateUserInformation_Ex()
                If Not exit_ex.Silent Then
                    If exit_ex.SimpleLogLine Then
                        LogError(Nothing, $"downloading interrupted (exit) ({exit_ex.Message})")
                    Else
                        LogError(exit_ex, "downloading interrupted (exit)")
                    End If
                End If
                If _EnvirInvokeUserUpdated Then OnUserUpdated()
                Canceled = True
            Catch dex As ObjectDisposedException When Disposed
                Canceled = True
            Catch ex As Exception
                UpdateUserInformation_Ex()
                LogError(ex, "downloading data error")
                If _EnvirInvokeUserUpdated Then OnUserUpdated()
                HasError = True
            Finally
                If Not UserExists Then AddNonExistingUserToLog($"User '{ToStringForLog()}' not found on the site")
                If Not Responser Is Nothing Then Responser.Dispose() : Responser = Nothing
                If Not Canceled Then _DataParsed = True
                TokenPersonal = Nothing
                _ContentNew.Clear()
                _DownloadInProgress = False
                DownloadTopCount = Nothing
                DownloadDateFrom = Nothing
                DownloadDateTo = Nothing
                DownloadMissingOnly = False
                _ForceSaveUserData = False
                _ForceSaveUserInfo = False
                ProgressPre.Done()
                __DOWNLOAD_IN_PROGRESS = False
                OnUserDownloadStateChanged(False)
                If _ResponserAddResponseReceivedHandler Then Responser_ResponseReceived_RemoveHandler()
            End Try
        End Sub
        Protected Sub UpdateDataFiles()
            If Not User.File.IsEmptyString OrElse IsSavedPosts Then
                MyFileSettings = Nothing
                If IsSavedPosts Then
                    User = New UserInfo(SettingsHost.SavedPostsFolderName, HOST)
                    User.File.Path = $"{HOST.SavedPostsPath.PathWithSeparator}{SettingsFolderName}"
                    MyFileSettings = User.File
                    MyFileSettings.Name = MyFileSettings.Name.Replace(SettingsHost.SavedPostsFolderName, "SavedPosts")
                End If
                If MyFileSettings.IsEmptyString Then MyFileSettings = User.File
                MyFileData = MyFileSettings
                MyFileData.Name &= "_Data"
                MyFilePosts = MyFileSettings
                MyFilePosts.Name &= "_Posts"
                MyFilePosts.Extension = "txt"
                If Not IsSavedPosts Then
                    MyMD5File = MyFileSettings
                    MyMD5File.Name &= "_MD5"
                    MyMD5File.Extension = "txt"
                End If
            Else
                Throw New ArgumentNullException("User.File", "User file not detected")
            End If
        End Sub
        Protected MustOverride Sub DownloadDataF(ByVal Token As CancellationToken)
        Protected Overridable Sub Responser_ResponseReceived(ByVal Sender As Object, ByVal e As EventArguments.WebDataResponse)
        End Sub
        Protected Sub Responser_ResponseReceived_RemoveHandler()
            If Not Responser Is Nothing And _ResponserAddResponseReceivedHandler And Not Disposed Then
                Try : RemoveHandler Responser.ResponseReceived, AddressOf Responser_ResponseReceived : Catch : End Try
            End If
        End Sub
        Protected Function CreateCache() As CacheKeeper
            Dim Cache As New CacheKeeper($"{DownloadContentDefault_GetRootDir()}\_tCache\")
            Cache.CacheDeleteError = CacheDeletionError(Cache)
            If Cache.RootDirectory.Exists(SFO.Path, False) Then Cache.RootDirectory.Delete(SFO.Path, SFODelete.DeletePermanently, EDP.ReturnValue)
            Cache.Validate()
            Return Cache
        End Function
#Region "DownloadSingleObject"
        Protected IsSingleObjectDownload As Boolean = False
        Friend Overridable Sub DownloadSingleObject(ByVal Data As YouTube.Objects.IYouTubeMediaContainer, ByVal Token As CancellationToken) Implements IUserData.DownloadSingleObject
            Dim URL$ = String.Empty
            Try
                ResetHost()
                URL = Data.URL
                AccountName = Data.AccountName
                TokenQueue = Token
                If HOST Is Nothing Then Throw New ExitException($"Host '{AccountName}' not found")
                Data.DownloadState = UserMediaStates.Tried
                Progress = Data.Progress
                If Not Progress Is Nothing Then Progress.ResetProgressOnMaximumChanges = False
                If Not Responser Is Nothing Then Responser.Dispose()
                Responser = New Responser
                If Not HOST Is Nothing AndAlso HOST.Available(ISiteSettings.Download.SingleObject, True) AndAlso
                   Not HOST.Responser Is Nothing Then Responser.Copy(HOST.Responser)
                SeparateVideoFolder = False
                IsSingleObjectDownload = True
                DownloadSingleObject_GetPosts(Data, Token)
                DownloadSingleObject_CreateMedia(Data, Token)
                DownloadSingleObject_Download(Data, Token)
                DownloadSingleObject_PostProcessing(Data)
            Catch oex As OperationCanceledException When Token.IsCancellationRequested
                Data.DownloadState = UserMediaStates.Missing
                ErrorsDescriber.Execute(EDP.SendToLog, oex, $"{Site} download canceled: {URL}")
            Catch dex As ObjectDisposedException When Disposed
            Catch exit_ex As ExitException
                If Not exit_ex.Silent Then
                    If exit_ex.SimpleLogLine Then
                        MyMainLOG = $"{URL}: downloading canceled (exit) ({exit_ex.Message})"
                    Else
                        ErrorsDescriber.Execute(EDP.SendToLog, exit_ex, $"{URL}: downloading canceled (exit)")
                    End If
                End If
            Catch ex As Exception
                Data.DownloadState = UserMediaStates.Missing
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"{Site} single data downloader error: {URL}")
            End Try
        End Sub
        Protected Overridable Sub DownloadSingleObject_CreateMedia(ByVal Data As YouTube.Objects.IYouTubeMediaContainer, ByVal Token As CancellationToken)
            If _TempMediaList.Count > 0 Then
                For Each m As UserMedia In _TempMediaList
                    m.File = DownloadSingleObject_CreateFile(Data, m.File)
                    _ContentNew.Add(m)
                Next
            End If
        End Sub
        Protected Overridable Sub DownloadSingleObject_PostProcessing(ByVal Data As YouTube.Objects.IYouTubeMediaContainer, Optional ByVal ResetTitle As Boolean = True)
            If _ContentNew.Count > 0 Then
                If _ContentNew.Any(Function(mm) mm.State = UStates.Downloaded) Then
                    Data.DownloadState = UserMediaStates.Downloaded
                    Dim thumbAlong As Boolean = False
                    If TypeOf Data Is DownloadableMediaHost Then thumbAlong = DirectCast(Data, DownloadableMediaHost).ThumbAlong
                    If _ContentNew(0).Type = UTypes.Picture Or _ContentNew(0).Type = UTypes.GIF Then
                        DirectCast(Data, IDownloadableMedia).ThumbnailFile = _ContentNew(0).File
                    ElseIf Settings.STDownloader_TakeSnapshot And Settings.FfmpegFile.Exists And Not Settings.STDownloader_RemoveDownloadedAutomatically Then
                        Dim f As SFile = _ContentNew(0).File
                        Dim ff As SFile
                        If Settings.STDownloader_SnapshotsKeepWithFiles Or thumbAlong Then
                            ff = f
                        Else
                            ff = Settings.CacheSnapshots(Settings.STDownloader_SnapShotsCachePermamnent).NewFile
                        End If
                        ff.Name &= "_thumb"
                        ff.Extension = "jpg"
                        f = Web.FFMPEG.TakeSnapshot(f, ff, Settings.FfmpegFile, TimeSpan.FromSeconds(1),,, EDP.SendToLog + EDP.ReturnValue)
                        If f.Exists Then DirectCast(Data, IDownloadableMedia).ThumbnailFile = f
                    End If
                    Dim filesSize# = (From mm As UserMedia In _ContentNew Where mm.State = UStates.Downloaded AndAlso mm.File.Exists Select mm.File.Size).Sum
                    If filesSize > 0 Then filesSize /= 1024
                    Data.Size = filesSize
                Else
                    Data.DownloadState = UserMediaStates.Missing
                End If
                YouTube.Objects.YouTubeMediaContainerBase.Update(_ContentNew(0), Data)
                If _ContentNew.Count > 1 Then Data.Files.ListAddList(_ContentNew.Select(Function(cc) cc.File), LNC)
                If ResetTitle And Not _ContentNew(0).File.Name.IsEmptyString Then Data.Title = _ContentNew(0).File.Name
            Else
                Data.DownloadState = UserMediaStates.Missing
            End If
        End Sub
        Protected Function DownloadSingleObject_CreateFile(ByVal Data As YouTube.Objects.IYouTubeMediaContainer, ByVal DFile As SFile) As SFile
            If Not Data.File.Path.IsEmptyString Then DFile.Path = Data.File.Path
            If DFile.Name.IsEmptyString Then DFile.Name = "OutputFile"
            Return DFile
        End Function
        Protected Overridable Sub DownloadSingleObject_Download(ByVal Data As YouTube.Objects.IYouTubeMediaContainer, ByVal Token As CancellationToken)
            DownloadContent(Token)
        End Sub
        Protected Overridable Sub DownloadSingleObject_GetPosts(ByVal Data As YouTube.Objects.IYouTubeMediaContainer, ByVal Token As CancellationToken)
        End Sub
#End Region
#Region "ReparseVideo, ReparseMissing"
        Protected Overridable Sub ReparseVideo(ByVal Token As CancellationToken)
        End Sub
        ''' <summary>
        ''' Missing posts must be collected from [<see cref="_ContentList"/>].<br/>
        ''' Reparsed post must be added to [<see cref="_TempMediaList"/>].<br/>
        ''' At the end of the function, reparsed posts must be removed from [<see cref="_ContentList"/>].
        ''' </summary>
        Protected Overridable Sub ReparseMissing(ByVal Token As CancellationToken)
        End Sub
#End Region
#Region "MD5 support"
        Private Const VALIDATE_MD5_ERROR As String = "VALIDATE_MD5_ERROR"
        Friend Property UseMD5Comparison As Boolean = False
        Protected Property StartMD5Checked As Boolean = False
        Friend Property RemoveExistingDuplicates As Boolean = False
        Private ReadOnly ErrMD5 As New ErrorsDescriber(EDP.ReturnValue)
        Private _MD5Loaded As Boolean = False
        Private Sub LoadMD5()
            Try
                If Not _MD5Loaded Then
                    _MD5Loaded = True
                    _MD5List.Clear()
                    If _ContentList.Count > 0 Then _MD5List.ListAddList(_ContentList.Select(Function(c) c.MD5), LAP.NotContainsOnly, EDP.ReturnValue)
                    If MyMD5File.Exists Then _MD5List.ListAddList(MyMD5File.GetLines, LAP.NotContainsOnly, EDP.ThrowException)
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "LoadMD5")
            End Try
        End Sub
        Private Function ValidateMD5_GetMD5(ByVal __data As UserMedia, ByVal IsUrl As Boolean) As String
            Try
                Dim ImgFormat As Imaging.ImageFormat = Nothing
                Dim hash$ = String.Empty
                Dim __isGif As Boolean = False
                If __data.Type = UTypes.GIF Then
                    ImgFormat = Imaging.ImageFormat.Gif
                    __isGif = True
                ElseIf Not __data.File.IsEmptyString Then
                    ImgFormat = GetImageFormat(__data.File)
                End If
                If ImgFormat Is Nothing Then ImgFormat = Imaging.ImageFormat.Jpeg
                If IsUrl And Not __isGif Then
                    hash = ByteArrayToString(GetMD5(SFile.GetBytesFromNet(__data.URL.IfNullOrEmpty(__data.URL_BASE), ErrMD5), ImgFormat, ErrMD5))
                ElseIf IsUrl And __isGif Then
                    hash = ByteArrayToString(GetMD5FromBytes(SFile.GetBytesFromNet(__data.URL.IfNullOrEmpty(__data.URL_BASE), ErrMD5), ErrMD5))
                Else
                    hash = ByteArrayToString(GetMD5(SFile.GetBytes(__data.File, ErrMD5), ImgFormat, ErrMD5))
                End If
                If hash.IsEmptyString And Not __isGif Then
                    If ImgFormat Is Imaging.ImageFormat.Jpeg Then ImgFormat = Imaging.ImageFormat.Png Else ImgFormat = Imaging.ImageFormat.Jpeg
                    If IsUrl Then
                        hash = ByteArrayToString(GetMD5(SFile.GetBytesFromNet(__data.URL.IfNullOrEmpty(__data.URL_BASE), ErrMD5), ImgFormat, ErrMD5))
                    Else
                        hash = ByteArrayToString(GetMD5(SFile.GetBytes(__data.File, ErrMD5), ImgFormat, ErrMD5))
                    End If
                End If
                Return hash
            Catch
                Return String.Empty
            End Try
        End Function
        Private Sub ValidateMD5(ByVal Token As CancellationToken)
            Try
                Dim missingMD5 As Predicate(Of UserMedia) = Function(d) (d.Type = UTypes.GIF Or d.Type = UTypes.Picture) And d.MD5.IsEmptyString
                If RemoveExistingDuplicates Then
                    RemoveExistingDuplicates = False
                    _ForceSaveUserInfo = True
                    LoadMD5()
                    Dim i%
                    Dim itemsCount% = 0
                    Dim limit% = If(DownloadTopCount, 0)
                    Dim data As UserMedia = Nothing
                    Dim f As SFile

                    If Not StartMD5Checked Then
                        StartMD5Checked = True
                        Dim existingFiles As List(Of SFile) = SFile.GetFiles(MyFileSettings.CutPath, "*.jpg|*.jpeg|*.png|*.gif",, EDP.ReturnValue).ListIfNothing
                        Dim eIndx%
                        Dim eFinder As Predicate(Of SFile) = Function(ff) ff.File = data.File.File

                        If existingFiles.Count > 0 Then
                            Dim h$
                            ProgressPre.ChangeMax(existingFiles.Count)
                            For i = existingFiles.Count - 1 To 0 Step -1
                                ProgressPre.Perform()
                                h = ValidateMD5_GetMD5(New UserMedia With {.File = existingFiles(i)}, False)
                                If Not h.IsEmptyString Then
                                    If _MD5List.Contains(h) Then
                                        MyMainLOG = $"{ToStringForLog()}: Removed image [{existingFiles(i).File}] (duplicate)"
                                        existingFiles(i).Delete(SFO.File, SFODelete.DeleteToRecycleBin, ErrMD5)
                                        existingFiles.RemoveAt(i)
                                    Else
                                        _MD5List.Add(h)
                                    End If
                                End If
                            Next
                        End If

                        If _ContentList.Count > 0 AndAlso _ContentList.Exists(missingMD5) Then
                            ProgressPre.ChangeMax(_ContentList.Count)
                            For i = 0 To _ContentList.Count - 1
                                data = _ContentList(i)
                                ProgressPre.Perform()
                                If data.Type = UTypes.GIF Or data.Type = UTypes.Picture Then
                                    If data.MD5.IsEmptyString Then
                                        ThrowAny(Token)
                                        eIndx = existingFiles.FindIndex(eFinder)
                                        If eIndx >= 0 Then
                                            data.MD5 = ValidateMD5_GetMD5(New UserMedia With {.File = existingFiles(eIndx)}, False)
                                            If Not data.MD5.IsEmptyString Then _ContentList(i) = data : _ForceSaveUserData = True
                                        End If
                                    End If
                                    existingFiles.RemoveAll(eFinder)
                                End If
                            Next
                        End If

                        If existingFiles.Count > 0 Then
                            ProgressPre.ChangeMax(existingFiles.Count)
                            For i = 0 To existingFiles.Count - 1
                                f = existingFiles(i)
                                ProgressPre.Perform()
                                data = New UserMedia(f.File) With {
                                    .State = UStates.Downloaded,
                                    .Type = IIf(f.Extension = "gif", UTypes.GIF, UTypes.Picture),
                                    .File = f
                                }
                                ThrowAny(Token)
                                data.MD5 = ValidateMD5_GetMD5(data, False)
                                If Not data.MD5.IsEmptyString Then _ContentList.Add(data) : _ForceSaveUserData = True
                            Next
                            existingFiles.Clear()
                        End If
                    End If

                    If _ContentList.Count > 0 Then _MD5List.ListAddList(_ContentList.Select(Function(d) d.MD5), LAP.NotContainsOnly, EDP.ReturnValue)
                End If
            Catch iex As ArgumentOutOfRangeException When Disposed
            Catch ex As Exception
                ProcessException(ex, Token, "ValidateMD5",, VALIDATE_MD5_ERROR)
            Finally
                ProgressPre.Done()
            End Try
        End Sub
#End Region
#Region "DownloadContent"
        Protected MustOverride Sub DownloadContent(ByVal Token As CancellationToken)
        Private NotInheritable Class OptionalWebClient : Inherits DownloadObjects.WebClient2
            Private ReadOnly Source As UserDataBase
            Friend Sub New(ByRef Source As UserDataBase)
                Me.Source = Source
                UseResponserClient = Source.UseResponserClient
                If UseResponserClient Then
                    Client = Source.Responser
                Else
                    Client = New RWebClient With {.UseNativeClient = Not Source.IsSingleObjectDownload}
                End If
                If Source.IsSingleObjectDownload Then DelegateEvents = True
            End Sub
            Private _LastProgressValue As Integer = 0
            Protected Overrides Sub Client_DownloadProgressChanged(ByVal Sender As Object, ByVal e As DownloadProgressChangedEventArgs)
                Dim v% = e.ProgressPercentage
                If v > _LastProgressValue Then
                    If v > 100 Then v = 100
                    Source.Progress.Value = v
                    Source.Progress.Perform(0)
                End If
                _LastProgressValue = e.ProgressPercentage
            End Sub
            Protected Overrides Sub Client_DownloadFileCompleted(ByVal Sender As Object, ByVal e As AsyncCompletedEventArgs)
                Source.Progress.Done()
            End Sub
        End Class
        Protected Const VideoFolderName As String = "Video"
        Protected Sub DownloadContentDefault(ByVal Token As CancellationToken)
            Try
                Dim i%
                Dim dCount% = 0, dTotal% = 0
                ThrowAny(Token)
                If _ContentNew.Count > 0 Then
                    _ContentNew.RemoveAll(Function(c) c.URL.IsEmptyString)
                    If _ContentNew.Count > 0 Then
                        If UseMD5Comparison Then LoadMD5()
                        MyFile.Exists(SFO.Path)
                        Dim MissingErrorsAdd As Boolean = Settings.AddMissingErrorsToLog
                        Dim MyDir$ = DownloadContentDefault_GetRootDir()
                        Dim vsf As Boolean = SeparateVideoFolderF
                        Dim __isVideo As Boolean
                        Dim __interrupt As Boolean
                        Dim f As SFile
                        Dim v As UserMedia
                        Dim __fileDeleted As Boolean
                        Dim fileNumProvider As SFileNumbers = SFileNumbers.Default
                        Dim __deleteFile As Action(Of SFile, String) = Sub(ByVal FileToDelete As SFile, ByVal FileUrl As String)
                                                                           Try
                                                                               If FileToDelete.Exists Then FileToDelete.Delete(,, EDP.ThrowException)
                                                                           Catch file_io_ex As IOException
                                                                               MyMainLOG = "File download aborted. You should download the following file again." & vbCr &
                                                                                           $"File: {FileToDelete}{vbCr}URL: {FileUrl}"
                                                                           Catch file_del_ex As Exception
                                                                               ErrorsDescriber.Execute(EDP.SendToLog, file_del_ex)
                                                                           End Try
                                                                       End Sub
                        Dim updateDownCount As Action = Sub()
                                                            Dim __n% = IIf(__fileDeleted, -1, 1)
                                                            If __isVideo Then
                                                                v.Type = UTypes.Video
                                                                DownloadedVideos(False) += __n
                                                            ElseIf v.Type = UTypes.GIF Then
                                                                DownloadedPictures(False) += __n
                                                            Else
                                                                v.Type = UTypes.Picture
                                                                DownloadedPictures(False) += __n
                                                            End If
                                                        End Sub

                        Using w As New OptionalWebClient(Me)
                            If vsf Then CSFileP($"{MyDir}\{VideoFolderName}\").Exists(SFO.Path)
                            Progress.Maximum += _ContentNew.Count
                            If IsSingleObjectDownload Then
                                If _ContentNew.Count = 1 And _ContentNew(0).Type = UTypes.Video Then
                                    Progress.Value = 0
                                    Progress.Maximum = 100
                                    Progress.Provider = MyProgressNumberProvider.Percentage
                                ElseIf _ContentNew(0).Type = UTypes.m3u8 Then
                                    Progress.Provider = MyProgressNumberProvider.Percentage
                                Else
                                    w.DelegateEvents = False
                                End If
                            End If

                            For i = 0 To _ContentNew.Count - 1
                                ThrowAny(Token)
                                v = _ContentNew(i)
                                v.State = UStates.Tried
                                If v.File.IsEmptyString Then
                                    f = CreateFileFromUrl(v.URL)
                                Else
                                    f = v.File
                                End If
                                f.Separator = "\"
                                If Not IsSingleObjectDownload Then f.Path = MyDir

                                If v.URL_BASE.IsEmptyString Then v.URL_BASE = v.URL

                                __fileDeleted = False

                                If Not f.IsEmptyString And Not v.URL.IsEmptyString Then
                                    Try
                                        __isVideo = v.Type = UTypes.Video Or f.Extension = "mp4" Or v.Type = UTypes.m3u8

                                        If f.Extension.IsEmptyString Then
                                            Select Case v.Type
                                                Case UTypes.Picture : f.Extension = "jpg"
                                                Case UTypes.Video, UTypes.m3u8 : f.Extension = "mp4"
                                                Case UTypes.GIF : f.Extension = "gif"
                                            End Select
                                        ElseIf f.Extension = "webp" And Settings.DownloadNativeImageFormat Then
                                            f.Extension = "jpg"
                                        End If

                                        If Not v.SpecialFolder.IsEmptyString Then
                                            f.Path = $"{f.PathWithSeparator}{v.SpecialFolder.StringTrimEnd("*")}\".CSFileP.Path
                                            f.Exists(SFO.Path)
                                        End If
                                        If __isVideo And vsf Then
                                            If v.SpecialFolder.IsEmptyString OrElse Not v.SpecialFolder.EndsWith("*") Then
                                                f.Path = $"{f.PathWithSeparator}{VideoFolderName}"
                                                If Not v.SpecialFolder.IsEmptyString Then f.Exists(SFO.Path)
                                            End If
                                        End If

                                        If __isVideo Then fileNumProvider.FileName = f.Name : f = SFile.IndexReindex(f,,, fileNumProvider)

                                        __interrupt = False
                                        If IsSingleObjectDownload Then f.Exists(SFO.Path, True)
                                        If v.Type = UTypes.m3u8 And UseInternalM3U8Function Then
                                            f = DownloadM3U8(v.URL, v, f, Token)
                                            If f.IsEmptyString Then Throw New Exception("M3U8 download failed")
                                        ElseIf UseInternalDownloadFileFunction AndAlso ValidateDownloadFile(v.URL, v, __interrupt) Then
                                            f = DownloadFile(v.URL, v, f, Token)
                                            If f.IsEmptyString Then Throw New Exception("InternalFunc download failed")
                                        Else
                                            If UseInternalDownloadFileFunction And __interrupt Then Throw New Exception("InternalFunc download interrupted")
                                            If UseClientTokens Then
                                                w.DownloadFile(v.URL, f, Token)
                                            Else
                                                w.DownloadFile(v.URL, f)
                                            End If
                                        End If

                                        updateDownCount()

                                        v.File = ChangeFileNameByProvider(f, v)
                                        v.State = UStates.Downloaded
                                        DownloadContentDefault_PostProcessing(v, f, Token)
                                        If UseMD5Comparison And (v.Type = UTypes.GIF Or v.Type = UTypes.Picture) Then
                                            If v.File.Exists Then
                                                v.MD5 = ValidateMD5_GetMD5(v, False)
                                                If Not v.MD5.IsEmptyString Then
                                                    If _MD5List.Contains(v.MD5) Then
                                                        __fileDeleted = v.File.Delete(SFO.File, SFODelete.DeletePermanently, EDP.ReturnValue)
                                                        If __fileDeleted Then dCount -= 1 : updateDownCount()
                                                    Else
                                                        _MD5List.Add(v.MD5)
                                                    End If
                                                End If
                                            Else
                                                dCount -= 1
                                            End If
                                        End If
                                        dCount += 1
                                    Catch woex As OperationCanceledException When Token.IsCancellationRequested
                                        __deleteFile.Invoke(f, v.URL_BASE)
                                        v.State = UStates.Missing
                                        v.Attempts += 1
                                        _ContentNew(i) = v
                                        Throw woex
                                    Catch wex As Exception
                                        If DownloadContentDefault_ProcessDownloadException() Then
                                            v.Attempts += 1
                                            v.State = UStates.Missing
                                            If MissingErrorsAdd Then ErrorDownloading(f, v.URL)
                                        End If
                                    End Try
                                Else
                                    v.State = UStates.Skipped
                                End If
                                If Not __fileDeleted Then _ContentNew(i) = v
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
            Catch iex As IndexOutOfRangeException When Disposed
            Catch oex As OperationCanceledException When Token.IsCancellationRequested
            Catch dex As ObjectDisposedException When Disposed
            Catch ex As Exception
                LogError(ex, "content downloading error")
                HasError = True
            End Try
        End Sub
        Protected UseInternalM3U8Function As Boolean = False
        Protected Overridable Function DownloadM3U8(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile,
                                                    ByVal Token As CancellationToken) As SFile
            Return Nothing
        End Function
        Protected UseInternalDownloadFileFunction As Boolean = False
        Protected Overridable Function DownloadFile(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile,
                                                    ByVal Token As CancellationToken) As SFile
            Return Nothing
        End Function
        Protected Overridable Function ValidateDownloadFile(ByVal URL As String, ByVal Media As UserMedia, ByRef Interrupt As Boolean) As Boolean
            Return True
        End Function
        ''' <returns><c>MyFile.CutPath(IIf(IsSingleObjectDownload, 0, 1)).PathNoSeparator</c></returns>
        Protected Overridable Function DownloadContentDefault_GetRootDir() As String
            Return MyFile.CutPath(IIf(IsSingleObjectDownload, 0, 1)).PathNoSeparator
        End Function
        Protected Overridable Sub DownloadContentDefault_PostProcessing(ByRef m As UserMedia, ByVal File As SFile, ByVal Token As CancellationToken)
        End Sub
        Protected Overridable Function DownloadContentDefault_ProcessDownloadException() As Boolean
            Return True
        End Function
#End Region
#Region "ProcessException"
        Protected Const EXCEPTION_OPERATION_CANCELED As Integer = -1
        ''' <param name="RDE">Request DownloadingException</param>
        ''' <returns>0 - exit</returns>
        Protected Function ProcessException(ByVal ex As Exception, ByVal Token As CancellationToken, ByVal Message As String,
                                            Optional ByVal RDE As Boolean = True, Optional ByVal EObj As Object = Nothing,
                                            Optional ByVal ThrowEx As Boolean = True) As Integer
            If TypeOf ex Is ExitException Then
                Throw ex
            ElseIf Not ((TypeOf ex Is OperationCanceledException And (Token.IsCancellationRequested Or TokenPersonal.IsCancellationRequested Or TokenQueue.IsCancellationRequested)) Or
                        (TypeOf ex Is ObjectDisposedException And Disposed)) Then
                If RDE Then
                    Dim v% = DownloadingException(ex, Message, True, EObj)
                    If v = 0 Then LogError(ex, Message) : HasError = True
                    Return v
                End If
            Else
                If ThrowEx Then Throw ex Else Return EXCEPTION_OPERATION_CANCELED
            End If
            Return 0
        End Function
        ''' <summary>0 - Execute LogError and set HasError</summary>
        Protected MustOverride Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False, Optional ByVal EObj As Object = Nothing) As Integer
#End Region
#Region "ChangeFileNameByProvider, RunScript"
        Protected Overridable Function CreateFileFromUrl(ByVal URL As String) As SFile
            Return New SFile(URL)
        End Function
        Protected Overridable Function SimpleDownloadAvatar(ByVal ImageAddress As String, Optional ByVal FileCreateFunc As Func(Of String, SFile) = Nothing,
                                                            Optional ByVal e As ErrorsDescriber = Nothing) As SFile
            Try
                If Not ImageAddress.IsEmptyString Then
                    Dim f As SFile
                    If FileCreateFunc Is Nothing Then
                        f = CreateFileFromUrl(ImageAddress)
                    Else
                        f = FileCreateFunc.Invoke(ImageAddress)
                    End If
                    If Not f.Name.IsEmptyString Then f.Name = f.Name.StringRemoveWinForbiddenSymbols.StringTrim
                    If Not f.Name.IsEmptyString Then
                        f.Path = DownloadContentDefault_GetRootDir()
                        f.Separator = "\"
                        If f.Extension.IsEmptyString Then f.Extension = "jpg"
                        If Not f.Exists Then GetWebFile(ImageAddress, f, EDP.ReturnValue)
                        If f.Exists Then IconBannerDownloaded = True : Return f
                    End If
                End If
                Return Nothing
            Catch ex As Exception
                If Not e.Exists Then e = New ErrorsDescriber(EDP.ReturnValue)
                Return ErrorsDescriber.Execute(e, ex, $"SimpleDownloadAvatar({ImageAddress})", New SFile)
            End Try
        End Function
        Protected Overridable Function ChangeFileNameByProvider(ByVal f As SFile, ByVal m As UserMedia) As SFile
            Dim ff As SFile = Nothing
            Try
                If f.Exists Then
                    If Not Settings.FileReplaceNameByDate.Value = FileNameReplaceMode.None Then
                        ff = f
                        ff.Name = String.Format(FileDateAppenderPattern, f.Name, CStr(AConvert(Of String)(If(m.Post.Date, Now), FileDateAppenderProvider, String.Empty)))
                        ff = SFile.IndexReindex(ff,,, New NumberedFile(ff))
                    End If
                    If Not ff.Name.IsEmptyString Then My.Computer.FileSystem.RenameFile(f, ff.File) : Return ff
                End If
                Return f
            Catch ex As Exception
                LogError(ex, $"change file name from [{f}] to [{ff}]")
                Return f
            End Try
        End Function
        Private Sub RunScript()
            Try
                Const spa$ = "{0}"
                If ScriptUse Then
                    Dim ScriptPattern$
                    If Not ScriptData.IsEmptyString Then
                        ScriptPattern = ScriptData
                    Else
                        ScriptPattern = Settings.ScriptData.Value
                    End If
                    If Not ScriptPattern.IsEmptyString Then
                        If Not ScriptPattern.Contains(spa) Then ScriptPattern &= $" ""{spa}"""
                        Using b As New BatchExecutor With {.RedirectStandardError = True}
                            b.Execute({String.Format(ScriptPattern, MyFile.CutPath(1).PathNoSeparator)}, EDP.SendToLog + EDP.ThrowException)
                            If b.HasError Or Not b.ErrorOutput.IsEmptyString Then Throw New Exception(b.ErrorOutput, b.ErrorException)
                        End Using
                    End If
                End If
            Catch ex As Exception
                LogError(ex, "script execution error")
            End Try
        End Sub
#End Region
#End Region
#Region "Erase, Delete, Move, Merge, Copy"
        Friend Shared Function GetEraseMode(ByVal Users As IEnumerable(Of IUserData)) As IUserData.EraseMode
            Dim mode As IUserData.EraseMode = IUserData.EraseMode.None
            If Users.ListExists Then
                Dim m As New MMessage("The data of the following users will be erased:" & vbCr & vbCr, "Erase data",
                                      {New MsgBoxButton("History and Data", "All files (images and videos) will be deleted; download history will be deleted."),
                                       New MsgBoxButton("Data", "All files (images and videos) will be deleted; download history will not be affected."),
                                       New MsgBoxButton("History", "All files (images and videos) will not be affected; download history will be deleted."),
                                       New MsgBoxButton("Cancel")
                                      }, MsgBoxStyle.Exclamation) With {.ButtonsPerRow = 4}
                Dim collectionsCount% = Users.Count(Function(u) u.IsCollection)
                m.Text &= Users.ListToStringE(vbNewLine, MainFrameObj.GetUserListProvider(collectionsCount > 0))
                m.Text &= vbCr.StringDup(2)
                If collectionsCount > 0 Then
                    If collectionsCount = 1 And Users.Count = 1 Then
                        m.Text &= $"THIS USER IS A COLLECTION OF {DirectCast(Users(0), UserDataBind).Count} USERS. THE DATA WILL BE ERASED FOR ALL OF THEM."
                    Else
                        m.Text &= "ONE OR MORE USERS IN THE LIST IS A COLLECTION. THE DATA WILL BE ERASED FOR EACH USER OF EACH COLLECTION."
                    End If
                    m.Text &= vbCr.StringDup(2)
                End If
                m.Text &= "Are you sure you want to erase the data?"
                Select Case m.Show
                    Case 0 : mode = IUserData.EraseMode.Data + IUserData.EraseMode.History
                    Case 1 : mode = IUserData.EraseMode.Data
                    Case 2 : mode = IUserData.EraseMode.History
                End Select
            End If
            Return mode
        End Function
        Friend Overridable Function EraseData(ByVal Mode As IUserData.EraseMode) As Boolean Implements IUserData.EraseData
            Try
                Dim result As Boolean = False
                If Not Mode = IUserData.EraseMode.None And Not DataMerging Then
                    Dim m() As IUserData.EraseMode = Mode.EnumExtract(Of IUserData.EraseMode)
                    If m.ListExists Then
                        Dim e As New ErrorsDescriber(EDP.ReturnValue)
                        If m.Contains(IUserData.EraseMode.History) Then
                            If MyFilePosts.Delete(SFO.File, SFODelete.DeleteToRecycleBin, e) Then result = True
                            If MyFileData.Delete(SFO.File, SFODelete.DeleteToRecycleBin, e) Then result = True
                            If MyMD5File.Delete(SFO.File, SFODelete.DeleteToRecycleBin, e) Then result = True
                            LastUpdated = Nothing
                            EraseData_AdditionalDataFiles()
                            UpdateUserInformation()
                        End If
                        If m.Contains(IUserData.EraseMode.Data) Then
                            Dim files As List(Of SFile) = SFile.GetFiles(DownloadContentDefault_GetRootDir.CSFileP,, SearchOption.AllDirectories, e)
                            If files.ListExists Then files.RemoveAll(Function(f) Not f.Extension.IsEmptyString AndAlso (f.Extension = "txt" Or f.Extension = "xml"))
                            If files.ListExists Then files.ForEach(Sub(f) f.Delete(SFO.File, Settings.DeleteMode, e))
                            LatestData.Clear()
                            result = True
                        End If
                        If result Then
                            _TempPostsList.Clear()
                            _TempMediaList.Clear()
                            _ContentNew.Clear()
                            _ContentList.Clear()
                            _MD5List.Clear()
                            _MD5Loaded = False
                        End If
                    End If
                End If
                Return result
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, $"EraseData({CInt(Mode)}): {ToStringForLog()}", False)
            End Try
        End Function
        Protected Overridable Sub EraseData_AdditionalDataFiles() Implements IPluginContentProvider.ResetHistoryData
        End Sub
        Friend Overridable Function Delete(Optional ByVal Multiple As Boolean = False, Optional ByVal CollectionValue As Integer = -1) As Integer Implements IUserData.Delete
            Dim f As SFile = SFile.GetPath(MyFile.CutPath.Path)
            If f.Exists(SFO.Path, False) AndAlso (User.Merged OrElse f.Delete(SFO.Path, Settings.DeleteMode)) Then
                If Not IncludedInCollection Then MainFrameObj.ImageHandler(Me, False)
                Settings.UsersList.Remove(User)
                Settings.UpdateUsersList()
                If Not IncludedInCollection Then Settings.Users.Remove(Me)
                Downloader.UserRemove(Me)
                Dispose(True)
                Return 1
            Else
                Return 0
            End If
        End Function
        Friend Function SplitCollectionGetNewUserInfo() As SplitCollectionUserInfo
            Dim u As New SplitCollectionUserInfo With {.UserOrig = User, .UserNew = User}
            With u.UserNew
                .CollectionName = String.Empty
                .SpecialCollectionPath = Nothing
                .UserModel = UsageModel.Default
                .CollectionModel = UsageModel.Default
                .UpdateUserFile()
            End With
            Return u
        End Function
        Friend Overridable Function MoveFiles(ByVal __CollectionName As String, ByVal __SpecialCollectionPath As SFile, Optional ByVal NewUser As SplitCollectionUserInfo? = Nothing) As Boolean Implements IUserData.MoveFiles
            Dim UserBefore As UserInfo = User
            Dim Removed As Boolean = True
            Dim _TurnBack As Boolean = False
            Try
                Dim f As SFile
                Dim v As Boolean = IsVirtual
                Settings.Feeds.Load()

                If IncludedInCollection And __CollectionName.IsEmptyString And __SpecialCollectionPath.IsEmptyString Then
                    Settings.Users.Add(Me)
                    Removed = False
                    User.CollectionName = String.Empty
                    User.SpecialCollectionPath = String.Empty
                    User.UserModel = UsageModel.Default
                    User.CollectionModel = UsageModel.Default
                    If NewUser.HasValue Then User.SpecialPath = NewUser.Value.UserNew.SpecialPath
                Else
                    Settings.Users.Remove(Me)
                    Removed = True
                    User.CollectionName = __CollectionName
                    User.SpecialCollectionPath = __SpecialCollectionPath
                    If Not IsVirtual Then User.SpecialPath = Nothing
                End If
                _TurnBack = True
                User.UpdateUserFile()

                If Not v Then
                    f = User.File.CutPath(, EDP.ThrowException)
                    If f.Exists(SFO.Path, False) Then
                        If If(SFile.GetFiles(f,, SearchOption.AllDirectories), New List(Of SFile)).Count > 0 AndAlso
                           MsgBoxE({$"Destination directory [{f.Path}] already exists and contains files!" & vbCr &
                                    "By continuing, this directory and all files will be deleted",
                                    "Destination directory is not empty!"}, MsgBoxStyle.Exclamation,,, {"Delete", "Cancel"}) = 1 Then
                            MsgBoxE("Operation canceled", MsgBoxStyle.Exclamation)
                            User = UserBefore
                            If Removed Then Settings.Users.Add(Me) Else Settings.Users.Remove(Me)
                            _TurnBack = False
                            Return False
                        End If
                        f.Delete(SFO.Path, Settings.DeleteMode, EDP.ThrowException)
                    End If
                    f.CutPath.Exists(SFO.Path)
                    SFile.Move(UserBefore.File.CutPath(, EDP.ThrowException), f, SFO.Path,,
                               SFODelete.EmptyOnly + SFODelete.DeleteToRecycleBin + SFODelete.OnCancelThrowException, EDP.ThrowException)
                    If Not ScriptData.IsEmptyString AndAlso ScriptData.Contains(UserBefore.File.PathNoSeparator) Then _
                       ScriptData = ScriptData.Replace(UserBefore.File.PathNoSeparator, MyFile.PathNoSeparator)
                End If

                Settings.UsersList.Remove(UserBefore)
                Settings.UpdateUsersList(User)
                Settings.Feeds.UpdateUsers(UserBefore, User)
                UpdateUserInformation()
                Return True
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Files moving error")
                User = UserBefore
                If _TurnBack Then
                    If Removed Then Settings.Users.Add(Me) Else Settings.Users.Remove(Me)
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
                        UserBefore.File.CutPath.Delete(SFO.Path, Settings.DeleteMode, EDP.SendToLog)
                    End If
                    If Not ScriptData.IsEmptyString AndAlso ScriptData.Contains(UserBefore.File.PathNoSeparator) Then _
                       ScriptData = ScriptData.Replace(UserBefore.File.PathNoSeparator, MyFile.PathNoSeparator)
                    Settings.Feeds.UpdateUsers(UserBefore, User)
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
        Private Class FilesCopyingException : Inherits ErrorsDescriberException
            Friend Sub New(ByVal User As IUserData, ByVal Msg As String, ByVal Path As SFile)
                SendToLogOnlyMessage = True
                If User.IncludedInCollection Then _MainMessage = $"[{User.CollectionName}] - "
                _MainMessage &= $"[{User.Site}] - [{User.Name}]. {Msg}: {Path.Path}."
            End Sub
        End Class
        Friend Overridable Function CopyFiles(ByVal DestinationPath As SFile, Optional ByVal e As ErrorsDescriber = Nothing) As Boolean Implements IUserData.CopyFiles
            Dim fSource As SFile = Nothing
            Dim fDest As SFile = Nothing
            Try
                Dim pOffset%
                If IncludedInCollection Then
                    If DataMerging Then pOffset = 1 Else pOffset = 2
                Else
                    pOffset = 1
                End If
                fSource = MyFile.CutPath(pOffset).Path.CSFileP

                Dim OptPath$ = String.Empty
                If IncludedInCollection Then
                    OptPath = $"Collections\{CollectionName}" 'Copying a collection based on the first file
                Else
                    OptPath = $"{Site}\{Name}"
                End If
                fDest = $"{DestinationPath.PathWithSeparator}{OptPath}".CSFileP
                If fDest.Exists(SFO.Path, False) AndAlso MsgBoxE({$"The following path already exists:{vbCr}{fDest.Path}" & vbCr &
                                                                  "Do you want to copy files here?", "Copying files"}, vbExclamation + vbYesNo) = vbNo Then _
                   Throw New FilesCopyingException(Me, "The following path already exists", fDest)

                If DestinationPath.Exists(SFO.Path, True) Then
                    My.Computer.FileSystem.CopyDirectory(fSource, fDest, FileIO.UIOption.OnlyErrorDialogs, FileIO.UICancelOption.ThrowException)
                Else
                    Throw New FilesCopyingException(Me, "Cannot create the following path", fDest)
                End If
                Return True
            Catch cex As OperationCanceledException
                Return ErrorsDescriber.Execute(e, New FilesCopyingException(Me, "Copy canceled", fDest),, False)
            Catch ex As Exception
                Return ErrorsDescriber.Execute(e, ex,, False)
            End Try
        End Function
#End Region
#Region "Errors functions"
        Protected Sub LogError(ByVal ex As Exception, ByVal Message As String, Optional ByVal e As ErrorsDescriber = Nothing)
            ErrorsDescriber.Execute(If(e.Exists, e, New ErrorsDescriber(EDP.SendToLog)), ex, $"{ToStringForLog()}: {Message}")
        End Sub
        Protected Sub ErrorDownloading(ByVal f As SFile, ByVal URL As String)
            If Not f.Exists Then MyMainLOG = $"Error downloading from [{URL}] to [{f}]"
        End Sub
        ''' <exception cref="ObjectDisposedException"></exception>
        Protected Sub ThrowIfDisposed()
            If Disposed Then Throw New ObjectDisposedException(ToString(), "Object disposed")
        End Sub
        ''' <inheritdoc cref="ThrowAny(CancellationToken)"/>
        Private Overloads Sub ThrowAny() Implements IThrower.ThrowAny
            ThrowAny(TokenQueue)
        End Sub
        ''' <summary><c>ThrowAnyImpl(Token)</c></summary>
        ''' <exception cref="OperationCanceledException"></exception>
        ''' <exception cref="ObjectDisposedException"></exception>
        Friend Overridable Overloads Sub ThrowAny(ByVal Token As CancellationToken)
            ThrowAnyImpl(Token)
        End Sub
        Protected Sub ThrowAnyImpl(ByVal Token As CancellationToken)
            Token.ThrowIfCancellationRequested()
            TokenQueue.ThrowIfCancellationRequested()
            TokenPersonal.ThrowIfCancellationRequested()
            ThrowIfDisposed()
        End Sub
#End Region
        Friend Function ToStringForLog() As String
            Return $"{IIf(IncludedInCollection, $"[{CollectionName}] - ", String.Empty)}[{Site}] - {Name}"
        End Function
        Public Overrides Function ToString() As String
            If IsCollection Then
                Return CollectionName
            Else
                Return IIf(FriendlyName.IsEmptyString, Name, FriendlyName)
            End If
        End Function
        Public Overrides Function GetHashCode() As Integer
            Dim hcStr$
            If Not CollectionName.IsEmptyString Then
                hcStr = CollectionName
            Else
                hcStr = IIf(FriendlyName.IsEmptyString, Name, FriendlyName)
            End If
            If hcStr.IsEmptyString Then hcStr = LVIKey
            Return hcStr.GetHashCode
        End Function
#Region "Buttons actions"
        Private Sub BTT_CONTEXT_DOWN_KeyClick(sender As Object, e As MyKeyEventArgs) Handles BTT_CONTEXT_DOWN.KeyClick
            Downloader.Add(Me, e.IncludeInTheFeed)
        End Sub
        Private Sub BTT_CONTEXT_DOWN_LIMIT_KeyClick(sender As Object, e As MyKeyEventArgs) Handles BTT_CONTEXT_DOWN_LIMIT.KeyClick
            ControlInvokeFast(MainFrameObj.MF, Sub() MainFrameObj.MF.DownloadSelectedUser(MainFrame.DownUserLimits.Number, e.IncludeInTheFeed, Me), EDP.SendToLog)
        End Sub
        Private Sub BTT_CONTEXT_DOWN_DATE_KeyClick(sender As Object, e As MyKeyEventArgs) Handles BTT_CONTEXT_DOWN_DATE.KeyClick
            ControlInvokeFast(MainFrameObj.MF, Sub() MainFrameObj.MF.DownloadSelectedUser(MainFrame.DownUserLimits.Date, e.IncludeInTheFeed, Me), EDP.SendToLog)
        End Sub
        Private Sub BTT_CONTEXT_EDIT_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_EDIT.Click
            Using f As New Editors.UserCreatorForm(Me)
                f.ShowDialog()
                If f.DialogResult = DialogResult.OK Then UpdateUserInformation()
            End Using
        End Sub
        Private Sub BTT_CONTEXT_DELETE_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_DELETE.Click
        End Sub
        Private Sub BTT_CONTEXT_ERASE_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_ERASE.Click
            Const msgTitle$ = "Erase data"
            Try
                Dim m As IUserData.EraseMode = GetEraseMode({Me})
                If Not m = IUserData.EraseMode.None Then
                    If EraseData(m) Then
                        MsgBoxE({"User data has been erased.", msgTitle})
                    Else
                        MsgBoxE({"User data has not been erased.", msgTitle}, vbExclamation)
                    End If
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, msgTitle)
            End Try
        End Sub
        Private Sub BTT_CONTEXT_OPEN_PATH_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_OPEN_PATH.Click
            OpenFolder()
        End Sub
        Private Sub BTT_CONTEXT_OPEN_SITE_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_OPEN_SITE.Click
            OpenSite()
        End Sub
#End Region
#Region "IComparable Support"
        Friend Overridable Overloads Function CompareTo(ByVal Other As UserDataBase) As Integer Implements IComparable(Of UserDataBase).CompareTo
            If TypeOf Other Is UserDataBind Then
                Return 1
            ElseIf IsCollection Then
                Return Name.CompareTo(Other.Name)
            Else
                Return FriendlyName.IfNullOrEmpty(Name).StringTrim.CompareTo(Other.FriendlyName.IfNullOrEmpty(Other.Name).StringTrim)
            End If
        End Function
        Friend Overridable Overloads Function CompareTo(ByVal Obj As Object) As Integer Implements IComparable.CompareTo
            If Not Obj Is Nothing AndAlso TypeOf Obj Is UserDataBase Then
                Return CompareTo(DirectCast(Obj, UserDataBase))
            Else
                Return 0
            End If
        End Function
#End Region
#Region "IEquatable Support"
        Friend Overridable Overloads Function Equals(ByVal Other As UserDataBase) As Boolean Implements IEquatable(Of UserDataBase).Equals
            Return LVIKey = Other.LVIKey And IsSavedPosts = Other.IsSavedPosts
        End Function
        Public Overloads Overrides Function Equals(ByVal Obj As Object) As Boolean
            If Not Obj Is Nothing AndAlso TypeOf Obj Is UserDataBase Then
                Return Equals(DirectCast(Obj, UserDataBase))
            Else
                Return False
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
                    LatestData.Clear()
                    _TempMediaList.Clear()
                    _TempPostsList.Clear()
                    _MD5List.Clear()
                    TokenPersonal = Nothing
                    If Not ProgressPre Is Nothing Then ProgressPre.Reset() : ProgressPre.Dispose()
                    If Not Responser Is Nothing Then Responser.Dispose()
                    If Not BTT_CONTEXT_DOWN Is Nothing Then BTT_CONTEXT_DOWN.Dispose()
                    If Not BTT_CONTEXT_DOWN_LIMIT Is Nothing Then BTT_CONTEXT_DOWN_LIMIT.Dispose()
                    If Not BTT_CONTEXT_DOWN_DATE Is Nothing Then BTT_CONTEXT_DOWN_DATE.Dispose()
                    If Not BTT_CONTEXT_EDIT Is Nothing Then BTT_CONTEXT_EDIT.Dispose()
                    If Not BTT_CONTEXT_DELETE Is Nothing Then BTT_CONTEXT_DELETE.Dispose()
                    If Not BTT_CONTEXT_ERASE Is Nothing Then BTT_CONTEXT_ERASE.Dispose()
                    If Not BTT_CONTEXT_OPEN_PATH Is Nothing Then BTT_CONTEXT_OPEN_PATH.Dispose()
                    If Not BTT_CONTEXT_OPEN_SITE Is Nothing Then BTT_CONTEXT_OPEN_SITE.Dispose()
                    UserUpdatedEventHandlers.Clear()
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
End Namespace