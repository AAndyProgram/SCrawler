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
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.Messaging
Imports PersonalUtilities.Tools
Namespace API
    Friend Class UserDataBind : Inherits UserDataBase : Implements ICollection(Of IUserData), IMyEnumerator(Of IUserData)
#Region "Events"
        Friend Event OnCollectionSelfRemoved(ByVal Collection As IUserData)
        Friend Event OnUserRemoved(ByVal User As IUserData)
#End Region
#Region "Declarations"
        Friend ReadOnly Property Collections As List(Of IUserData)
#Region "Base class overrides"
        Friend Overrides ReadOnly Property IsVirtual As Boolean
            Get
                Return CollectionModel = UsageModel.Virtual
            End Get
        End Property
        Friend Overrides ReadOnly Property CollectionModel As UsageModel
            Get
                If Count > 0 Then Return Item(0).CollectionModel Else Return UsageModel.Default
            End Get
        End Property
        Friend Property CurrentlyEdited As Boolean = False
        Private _CollectionName As String = String.Empty
        Friend Overrides ReadOnly Property CollectionName As String
            Get
                If Count > 0 Then
                    Return Collections(0).CollectionName
                Else
                    Return _CollectionName
                End If
            End Get
        End Property
        Friend Sub ChangeVirtualCollectionName(ByVal NewName As String)
            If Count > 0 And Not NewName.IsEmptyString Then
                Dim u As UserInfo
                For Each user As UserDataBase In Collections
                    u = user.User
                    u.CollectionName = NewName
                    u.UpdateUserFile()
                    user.User = u
                    Settings.UpdateUsersList(u)
                Next
            End If
        End Sub
        Private _CollectionPath As SFile = Nothing
        Friend Overrides ReadOnly Property CollectionPath As SFile
            Get
                If Count > 0 And Not IsVirtual Then
                    Dim _RealUser As UserDataBase = GetRealUser()
                    If Not _RealUser Is Nothing Then Return _RealUser.User.SpecialCollectionPath
                End If
                Return _CollectionPath
            End Get
        End Property
        Friend Overrides ReadOnly Property Name As String
            Get
                Return CollectionName
            End Get
        End Property
        Friend Overrides Property FriendlyName As String
            Get
                Return CollectionName
            End Get
            Set(ByVal NewName As String)
            End Set
        End Property
        Friend Overrides Property UserExists As Boolean
            Get
                Return Count > 0 AndAlso Collections.Exists(Function(c) c.Exists)
            End Get
            Set(ByVal e As Boolean)
            End Set
        End Property
        Friend Overrides Property UserSuspended As Boolean
            Get
                Return Count > 0 AndAlso Collections.LongCount(Function(c) c.Suspended) = Count
            End Get
            Set(ByVal s As Boolean)
            End Set
        End Property
#Region "Images"
        Friend Overrides Sub SetPicture(ByVal f As SFile)
            If Count > 0 Then Collections.ForEach(Sub(c) c.SetPicture(f))
        End Sub
        Friend Overrides Function GetUserPicture() As Image
            If Count > 0 Then
                Dim img As Image
                For Each u As UserDataBase In Collections
                    img = u.GetPicture(Of Image)(False)
                    If Not img Is Nothing Then Return img
                Next
            End If
            Return GetNullPicture(If(Settings.ViewMode.Value = ViewModes.IconLarge, Settings.MaxLargeImageHeight, Settings.MaxSmallImageHeight))
        End Function
#End Region
        Friend Overrides ReadOnly Property DownloadedTotal(Optional ByVal Total As Boolean = True) As Integer
            Get
                If Count > 0 Then
                    Return Collections.Select(Function(u) u.DownloadedTotal(Total)).Sum
                Else
                    Return 0
                End If
            End Get
        End Property
        Friend Overrides ReadOnly Property ContentMissingExists As Boolean
            Get
                Return Count > 0 AndAlso Collections.Exists(Function(c) DirectCast(c, UserDataBase).ContentMissingExists)
            End Get
        End Property
        Friend Overrides Property MyFile As SFile
            Get
                If Count > 0 Then
                    If IsVirtual Then
                        Return GetRealUserFile.IfNullOrEmpty(Collections(0).File)
                    Else
                        Return Collections(0).File
                    End If
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal NewFile As SFile)
            End Set
        End Property
        Friend Overrides Property FileExists As Boolean
            Get
                If Count > 0 Then
                    Return Collections.Exists(Function(c) c.FileExists)
                Else
                    Return False
                End If
            End Get
            Set(ByVal IsExists As Boolean)
            End Set
        End Property
        Friend Overrides Property DataMerging As Boolean
            Get
                If Count > 0 AndAlso Collections.Exists(RealUser) Then
                    Return DirectCast(Collections.Find(RealUser), UserDataBase).DataMerging
                Else
                    Return False
                End If
            End Get
            Set(ByVal IsMerged As Boolean)
                MergeData(IsMerged)
            End Set
        End Property
        Friend Overrides Property HasError As Boolean
            Get
                Return MyBase.HasError Or (Count > 0 AndAlso Collections.Exists(Function(c) c.HasError))
            End Get
            Set(ByVal __HasError As Boolean)
                MyBase.HasError = __HasError
                If Count > 0 Then Collections.ForEach(Sub(c) c.HasError = False)
            End Set
        End Property
        Friend Overrides Property Temporary As Boolean
            Get
                If Count > 0 Then
                    Return Collections(0).Temporary
                Else
                    Return False
                End If
            End Get
            Set(ByVal Temp As Boolean)
                Collections.ForEach(Sub(c) c.Temporary = Temp)
                UpdateUserInformation()
            End Set
        End Property
        Friend Overrides Property Favorite As Boolean
            Get
                If Count > 0 Then
                    Return Collections(0).Favorite
                Else
                    Return False
                End If
            End Get
            Set(ByVal Fav As Boolean)
                Collections.ForEach(Sub(c) c.Favorite = Fav)
                UpdateUserInformation()
            End Set
        End Property
        Friend Overrides Property BackColor As Color?
            Get
                If Count > 0 Then
                    With Collections.Select(Function(u) u.BackColor)
                        If .All(Function(c) c.HasValue) Then
                            Dim cc As Color = Collections(0).BackColor.Value
                            If .All(Function(c) c.Value = cc) Then Return cc
                        End If
                    End With
                End If
                Return Nothing
            End Get
            Set(ByVal b As Color?)
                If Count > 0 Then Collections.ForEach(Sub(c) c.BackColor = b)
            End Set
        End Property
        Friend Overrides Property ForeColor As Color?
            Get
                If Count > 0 Then
                    With Collections.Select(Function(u) u.ForeColor)
                        If .All(Function(c) c.HasValue) Then
                            Dim cc As Color = Collections(0).ForeColor.Value
                            If .All(Function(c) c.Value = cc) Then Return cc
                        End If
                    End With
                End If
                Return Nothing
            End Get
            Set(ByVal f As Color?)
                If Count > 0 Then Collections.ForEach(Sub(c) c.ForeColor = f)
            End Set
        End Property
        Friend Overrides ReadOnly Property IsSubscription As Boolean
            Get
                Return Count > 0 AndAlso Collections.All(Function(u) u.IsSubscription)
            End Get
        End Property
        Friend Overrides Property ReadyForDownload As Boolean
            Get
                Return Count > 0 AndAlso Collections(0).ReadyForDownload
            End Get
            Set(ByVal IsReady As Boolean)
                If Count > 0 Then Collections.ForEach(Sub(c) c.ReadyForDownload = IsReady)
            End Set
        End Property
        Friend Overrides ReadOnly Property Labels As List(Of String)
            Get
                If Count > 0 Then
                    Return ListAddList(Nothing, Collections.SelectMany(Function(c) c.Labels), LAP.NotContainsOnly)
                Else
                    Return New List(Of String)
                End If
            End Get
        End Property
        Friend Overrides ReadOnly Property SpecialLabels As IEnumerable(Of String)
            Get
                If Count > 0 Then
                    With Collections.SelectMany(Function(u As UserDataBase) u.SpecialLabels)
                        If .ListExists Then Return .Distinct
                    End With
                End If
                Return New String() {}
            End Get
        End Property
        Friend Overrides Function GetUserInformation() As String
            Dim OutStr$ = String.Empty
            If IsVirtual Then OutStr = "This is a virtual collection."
            If Count > 0 Then Collections.ForEach(Sub(c) OutStr.StringAppendLine(DirectCast(c, UserDataBase).GetUserInformation(), vbNewLine.StringDup(2)))
            Return OutStr
        End Function
        Friend Overrides Property LastUpdated As Date?
            Get
                If Count > 0 Then
                    With (From c As IUserData In Collections
                          Where DirectCast(c, UserDataBase).LastUpdated.HasValue
                          Select DirectCast(c, UserDataBase).LastUpdated.Value).ToList
                        If .ListExists Then Return .Max
                    End With
                End If
                Return Nothing
            End Get
            Set(ByVal NewDate As Date?)
            End Set
        End Property
        Friend Overrides Property ScriptUse As Boolean
            Get
                Return Count > 0 AndAlso Collections.All(Function(c) c.ScriptUse)
            End Get
            Set(ByVal u As Boolean)
                If Count > 0 Then Collections.ForEach(Sub(ByVal c As IUserData)
                                                          Dim b As Boolean = c.ScriptUse = u
                                                          c.ScriptUse = u
                                                          If Not b Then c.UpdateUserInformation()
                                                      End Sub)
            End Set
        End Property
#End Region
#Region "Context buttons"
        Friend ReadOnly Property ContextDown As ToolStripMenuItem()
            Get
                If Count > 0 Then
                    Return Collections.Select(Function(c) DirectCast(c, UserDataBase).BTT_CONTEXT_DOWN).ToArray
                Else
                    Return New ToolStripMenuItem() {}
                End If
            End Get
        End Property
        Friend ReadOnly Property ContextDownLimit As ToolStripMenuItem()
            Get
                If Count > 0 Then
                    Return Collections.Select(Function(c) DirectCast(c, UserDataBase).BTT_CONTEXT_DOWN_LIMIT).ToArray
                Else
                    Return New ToolStripMenuItem() {}
                End If
            End Get
        End Property
        Friend ReadOnly Property ContextDownDate As ToolStripMenuItem()
            Get
                If Count > 0 Then
                    Return Collections.Select(Function(c) DirectCast(c, UserDataBase).BTT_CONTEXT_DOWN_DATE).ToArray
                Else
                    Return New ToolStripMenuItem() {}
                End If
            End Get
        End Property
        Friend ReadOnly Property ContextEdit As ToolStripMenuItem()
            Get
                If Count > 0 Then
                    Return Collections.Select(Function(c) DirectCast(c, UserDataBase).BTT_CONTEXT_EDIT).ToArray
                Else
                    Return New ToolStripMenuItem() {}
                End If
            End Get
        End Property
        Friend ReadOnly Property ContextDelete As ToolStripMenuItem()
            Get
                If Count > 0 Then
                    Return Collections.Select(Function(c) DirectCast(c, UserDataBase).BTT_CONTEXT_DELETE).ToArray
                Else
                    Return New ToolStripMenuItem() {}
                End If
            End Get
        End Property
        Friend ReadOnly Property ContextErase As ToolStripMenuItem()
            Get
                If Count > 0 Then
                    Return Collections.Select(Function(c) DirectCast(c, UserDataBase).BTT_CONTEXT_ERASE).ToArray
                Else
                    Return New ToolStripMenuItem() {}
                End If
            End Get
        End Property
        Friend ReadOnly Property ContextPath As ToolStripMenuItem()
            Get
                If Count > 0 Then
                    Return Collections.Select(Function(c) DirectCast(c, UserDataBase).BTT_CONTEXT_OPEN_PATH).ToArray
                Else
                    Return New ToolStripMenuItem() {}
                End If
            End Get
        End Property
        Friend ReadOnly Property ContextSite As ToolStripMenuItem()
            Get
                If Count > 0 Then
                    Return Collections.Select(Function(c) DirectCast(c, UserDataBase).BTT_CONTEXT_OPEN_SITE).ToArray
                Else
                    Return New ToolStripMenuItem() {}
                End If
            End Get
        End Property
#End Region
#End Region
#Region "Initializers"
        Friend Sub New()
            _IsCollection = True
            Collections = New List(Of IUserData)
        End Sub
        Friend Sub New(ByVal _Name As String, Optional ByVal _Path As SFile = Nothing)
            Me.New
            _CollectionName = _Name
            _CollectionPath = _Path
        End Sub
#End Region
#Region "Load, Update"
        Friend Overrides Sub LoadUserInformation()
            If Count > 0 Then Collections.ForEach(Sub(c) c.LoadUserInformation())
        End Sub
        Friend Overrides Sub UpdateUserInformation()
            If Count > 0 Then Collections.ForEach(Sub(c) c.UpdateUserInformation())
        End Sub
        Friend Overrides Sub LoadContentInformation(Optional ByVal Force As Boolean = False)
            If Count > 0 Then Collections.ForEach(Sub(c) DirectCast(c, UserDataBase).LoadContentInformation(Force))
        End Sub
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
        End Sub
#End Region
#Region "Download"
        Friend Overrides Property DownloadTopCount As Integer?
            Get
                Return If(Count > 0, Item(0).DownloadTopCount, Nothing)
            End Get
            Set(ByVal NewLimit As Integer?)
                If Count > 0 Then Collections.ForEach(Sub(c) c.DownloadTopCount = NewLimit)
            End Set
        End Property
        Friend Overrides Property IncludeInTheFeed As Boolean
            Get
                Return If(Count > 0, DirectCast(Item(0), UserDataBase).IncludeInTheFeed, Nothing)
            End Get
            Set(ByVal Include As Boolean)
                If Count > 0 Then Collections.ForEach(Sub(c) DirectCast(c, UserDataBase).IncludeInTheFeed = Include)
            End Set
        End Property
        Friend Overrides Property DownloadDateFrom As Date?
            Get
                Return If(Count > 0, Item(0).DownloadDateFrom, Nothing)
            End Get
            Set(ByVal d As Date?)
                If Count > 0 Then Collections.ForEach(Sub(c) c.DownloadDateFrom = d)
            End Set
        End Property
        Friend Overrides Property DownloadDateTo As Date?
            Get
                Return If(Count > 0, Item(0).DownloadDateTo, Nothing)
            End Get
            Set(ByVal d As Date?)
                If Count > 0 Then Collections.ForEach(Sub(c) c.DownloadDateTo = d)
            End Set
        End Property
        Friend Overrides Sub DownloadData(ByVal Token As CancellationToken)
            If Count > 0 Then Downloader.AddRange(Collections, True)
        End Sub
        Friend Overloads Sub DownloadData(ByVal Token As CancellationToken, ByVal __IncludedInTheFeed As Boolean)
            If Count > 0 Then Downloader.AddRange(Collections, __IncludedInTheFeed)
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
        End Sub
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
        End Sub
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal s As Object = Nothing) As Integer
            Return 0
        End Function
        Private Sub User_OnUserUpdated(ByVal User As IUserData)
            OnUserUpdated()
        End Sub
#End Region
#Region "Open site, folder"
        Friend Overrides Sub OpenSite(Optional ByVal e As ErrorsDescriber = Nothing)
            If Not e.Exists Then e = New ErrorsDescriber(EDP.SendToLog)
            If Count > 0 Then Collections.ForEach(Sub(c) c.OpenSite(e))
        End Sub
        Private ReadOnly RealUser As Predicate(Of IUserData) = Function(u) u.UserModel = UsageModel.Default And Not u.HOST.Key = PathPlugin.PluginKey
        Friend Overrides Sub OpenFolder()
            Try
                If Count > 0 Then
                    Dim i% = Collections.FindIndex(RealUser)
                    If i = -1 Then i = 0
                    If i >= 0 Then
                        If IsVirtual Or Collections(i).UserModel = UsageModel.Virtual Then
                            Collections(i).OpenFolder()
                        Else
                            GlobalOpenPath(Collections(i).File.CutPath(2))
                        End If
                    End If
                End If
            Catch
            End Try
        End Sub
        Friend Function GetRealUser() As IUserData
            Dim i% = -1
            If Count > 0 Then i = Collections.FindIndex(RealUser)
            Return If(i >= 0, Collections(i), Nothing)
        End Function
        Friend Function GetRealUserFile() As SFile
            Return If(GetRealUser()?.File, New SFile)
        End Function
#End Region
#Region "ICollection Support"
        Private ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of IUserData).IsReadOnly
            Get
                Return False
            End Get
        End Property
        Private Sub CopyTo(ByVal _Array() As IUserData, ByVal _ArrayIndex As Integer) Implements ICollection(Of IUserData).CopyTo
            Throw New NotImplementedException("The [CopyTo] method is not supported in a collection context")
        End Sub
#End Region
#Region "Item, Count, Clear"
        Default Friend ReadOnly Property Item(ByVal Index As Integer) As IUserData Implements IMyEnumerator(Of IUserData).MyEnumeratorObject
            Get
                Return Collections(Index)
            End Get
        End Property
        Friend ReadOnly Property Count As Integer Implements ICollection(Of IUserData).Count, IMyEnumerator(Of IUserData).MyEnumeratorCount
            Get
                If Collections Is Nothing Then
                    Return 0
                Else
                    Return Collections.Count
                End If
            End Get
        End Property
        Friend Sub Clear() Implements ICollection(Of IUserData).Clear
            Collections.ListClearDispose
        End Sub
#End Region
#Region "Add"
        ''' <exception cref="InvalidOperationException"></exception>
        Friend Overloads Sub Add(ByVal _Item As IUserData) Implements ICollection(Of IUserData).Add
            With _Item
                If .MoveFiles(CollectionName, CollectionPath) Then
                    If Not .Self.IsVirtual And DataMerging Then DirectCast(.Self, UserDataBase).MergeData()

                    ConsolidateLabels(.Self)
                    ConsolidateScripts(.Self)
                    ConsolidateColors(.Self)

                    Collections.Add(.Self)

                    With Collections.Last
                        If _CollectionName.IsEmptyString Then _CollectionName = .CollectionName
                        .Temporary = Temporary
                        .Favorite = Favorite
                        .ReadyForDownload = ReadyForDownload
                        .UpdateUserInformation()

                        MainFrameObj.ImageHandler(.Self, False)
                        AddRemoveBttDeleteHandler(.Self, True)
                        AddHandler .Self.UserUpdated, AddressOf User_OnUserUpdated
                    End With
                Else
                    Throw New InvalidOperationException("User data was not moved to the collection folder")
                End If
            End With
        End Sub
        ''' <summary>FOR SETTINGS START LOADING ONLY</summary>
        Friend Overloads Sub Add(ByVal u As UserInfo, Optional ByVal _LoadData As Boolean = True)
            Collections.Add(GetInstance(u, _LoadData))
            If Not Collections.Last Is Nothing Then
                With Collections.Last
                    If _CollectionName.IsEmptyString Then _CollectionName = .CollectionName
                    AddRemoveBttDeleteHandler(.Self, True)
                    AddHandler .UserUpdated, AddressOf User_OnUserUpdated
                End With
            Else
                Collections.RemoveAt(Count - 1)
            End If
        End Sub
        Private Sub AddRemoveBttDeleteHandler(ByRef User As IUserData, ByVal IsAdd As Boolean)
            Try
                With DirectCast(User, UserDataBase)
                    If IsAdd Then
                        .CreateButtons()
                        AddHandler .BTT_CONTEXT_DELETE.Click, AddressOf DeleteRemoveUserFromCollection
                    Else
                        RemoveHandler .BTT_CONTEXT_DELETE.Click, AddressOf DeleteRemoveUserFromCollection
                    End If
                End With
            Catch ex As Exception
            End Try
        End Sub
        Private Sub ConsolidateLabels(ByVal Destination As UserDataBase)
            If Count > 0 Then UpdateLabels(Destination, ListAddList(Nothing, Labels.ListWithRemove(SpecialLabels)), 0, True)
        End Sub
        Private Sub ConsolidateScripts(ByVal Destination As UserDataBase)
            If Count > 0 AndAlso ScriptUse Then
                Dim __scriptData$ = Collections(0).ScriptData
                Destination.ScriptUse = True
                If Collections.All(Function(c) c.ScriptData = __scriptData) Then Destination.ScriptData = __scriptData
            End If
        End Sub
        Private Sub ConsolidateColors(ByVal Destination As UserDataBase)
            If Count > 0 And Not Destination.ForeColor.HasValue And Not Destination.BackColor.HasValue Then
                Dim b As Color? = BackColor
                Dim f As Color? = ForeColor
                If b.HasValue AndAlso Not Collections.All(Function(u) Not u Is Destination AndAlso u.BackColor.HasValue AndAlso u.BackColor.Value = b.Value) Then b = Nothing
                If f.HasValue AndAlso Not Collections.All(Function(u) Not u Is Destination AndAlso u.ForeColor.HasValue AndAlso u.ForeColor.Value = f.Value) Then f = Nothing
                If b.HasValue Then Destination.BackColor = b
                If f.HasValue Then Destination.ForeColor = f
            End If
        End Sub
#End Region
#Region "Move, Merge"
        Friend Overrides Function MoveFiles(ByVal __CollectionName As String, ByVal __SpecialCollectionPath As SFile, Optional ByVal NewUser As SplitCollectionUserInfo? = Nothing) As Boolean
            Throw New NotImplementedException("Move files is not available in the collection context")
        End Function
        Friend Overloads Sub MergeData(ByVal Merging As Boolean)
            If Count > 0 Then
                If Merging Then
                    If DataMerging Then
                        MsgBoxE($"Collection [{CollectionName}] data already merged")
                    Else
                        If Collections.Count > 1 Then
                            Collections.ForEach(Sub(c) DirectCast(c, UserDataBase).MergeData())
                            MsgBoxE($"Collection [{CollectionName}] data merged")
                        Else
                            MsgBoxE($"Collection [{CollectionName}] contains only one user profile" & vbCr &
                                    "Data merging available from two and more profiles in collection!", MsgBoxStyle.Exclamation)
                        End If
                    End If
                Else
                    If DataMerging Then
                        MsgBoxE($"Collection [{CollectionName}] data is already merged" & vbCr &
                                "Combined data can not be undone", MsgBoxStyle.Critical)
                    Else
                        MsgBoxE($"Collection [{CollectionName}] data was never merged")
                    End If
                End If
            End If
        End Sub
#End Region
#Region "Erase, Remove, Delete"
        Friend Overrides Function EraseData(ByVal Mode As IUserData.EraseMode) As Boolean
            If Count > 0 Then
                Return Collections.All(Function(u) u.EraseData(Mode))
            Else
                Return True
            End If
        End Function
        Friend Function Remove(ByVal _Item As IUserData) As Boolean Implements ICollection(Of IUserData).Remove
            If DataMerging Then
                MsgBoxE($"Collection [{CollectionName}] data is already merged" & vbCr &
                        "Combined data can not be undone" & vbCr &
                        "Operation canceled", MsgBoxStyle.Critical)
                Return False
            Else
                Dim uObj As SplitCollectionUserInfo? = DirectCast(_Item, UserDataBase).SplitCollectionGetNewUserInfo
                If uObj.Value.SameDrive Then
                    uObj = Nothing
                Else
                    Using f As New SplitCollectionUserInfoChangePathsForm({uObj})
                        f.ShowDialog()
                        Select Case f.DialogResult
                            Case DialogResult.OK : If f.Users(0).Changed Then uObj = f.Users(0) Else uObj = Nothing
                            Case DialogResult.Abort : Return False
                        End Select
                    End Using
                End If
                _Item.MoveFiles(String.Empty, Nothing, uObj)
                MainFrameObj.ImageHandler(_Item)
                AddRemoveBttDeleteHandler(_Item, False)
                RaiseEvent OnUserRemoved(_Item)
                Return Collections.Remove(_Item)
            End If
        End Function
        Friend Overrides Function Delete(Optional ByVal Multiple As Boolean = False, Optional ByVal CollectionValue As Integer = -1) As Integer
            If Count > 0 Then
                Const MsgTitle$ = "Deleting a collection"
                Dim f As SFile = Nothing
                If Not IsVirtual Then
                    f = GetRealUserFile()
                    If Not f.IsEmptyString Then f = f.CutPath(IIf(DataMerging, 1, 2))
                End If
                Dim m As New MMessage($"Collection [{CollectionName} (number of profiles: {Count})] may contain data" & vbCr &
                                      "Are you sure you want to delete the collection and all of its files?", MsgTitle,
                                      {New MsgBoxButton("Delete", "Delete the collection and all files") With {.KeyCode = Keys.Enter},
                                       New MsgBoxButton("Split") With {
                                        .ToolTip = "Users will be removed from the collection and will be displayed in the program as separate users." & vbCr &
                                                   "All user data will remain.",
                                        .KeyCode = New ButtonKey(Keys.Enter, True)},
                                       "Cancel"}, vbExclamation)
                Dim v%
                If CollectionValue >= 0 Then
                    Select Case CollectionValue
                        Case 2 : v = 0
                        Case 3 : v = 1
                        Case Else : v = MsgBoxE(m)
                    End Select
                ElseIf Multiple Then
                    v = 0
                Else
                    v = MsgBoxE(m)
                End If
                Select Case v
                    Case 0
                        Collections.ForEach(Sub(c) c.Delete())
                        If Collections.All(Function(c As UserDataBase) c.Disposed) Then
                            Settings.Users.Remove(Me)
                            Downloader.UserRemove(Me)
                            MainFrameObj.ImageHandler(Me, False)
                            Collections.ListClearDispose
                            Dispose(False)
                            If Not f.IsEmptyString Then f.Delete(SFO.Path, SFODelete.EmptyOnly + Settings.DeleteMode, EDP.SendToLog)
                            Return 2
                        End If
                    Case 1
                        If DataMerging Then
                            MsgBoxE({$"Collection [{CollectionName}] data merged{vbCr}Unable to split merged collection{vbCr}Operation canceled", MsgTitle}, vbExclamation)
                            Return 0
                        Else
                            Dim uu As New List(Of SplitCollectionUserInfo)(Collections.Select(Function(uuu As UserDataBase) uuu.SplitCollectionGetNewUserInfo))
                            If uu.All(Function(uuu) uuu.SameDrive) Then
                                uu.Clear()
                            Else
                                Using colPaths As New SplitCollectionUserInfoChangePathsForm(uu)
                                    colPaths.ShowDialog()
                                    Select Case colPaths.DialogResult
                                        Case DialogResult.OK
                                            If colPaths.Users.Any(Function(uuu) uuu.Changed) Then
                                                uu = New List(Of SplitCollectionUserInfo)(colPaths.Users)
                                            Else
                                                uu.Clear()
                                            End If
                                        Case DialogResult.Abort : Return 0
                                    End Select
                                End Using
                            End If
                            Collections.ListForEach(Sub(ByVal c As IUserData, ByVal indx As Integer)
                                                        Dim uObj As SplitCollectionUserInfo? = Nothing
                                                        If uu.Count > 0 AndAlso indx.ValueBetween(0, uu.Count - 1) AndAlso uu(indx).Changed Then uObj = uu(indx)
                                                        If c.MoveFiles(String.Empty, Nothing, uObj) Then
                                                            UserListLoader.UpdateUser(Settings.GetUser(c), True)
                                                            MainFrameObj.ImageHandler(c)
                                                        End If
                                                    End Sub)
                            If Collections.All(Function(c) c.CollectionName.IsEmptyString) Then
                                Settings.Users.Remove(Me)
                                Collections.Clear()
                                If Not f.IsEmptyString Then f.Delete(SFO.Path, SFODelete.Default + Settings.DeleteMode, EDP.SendToLog)
                                Downloader.UserRemove(Me)
                                MainFrameObj.ImageHandler(Me, False)
                                Dispose(False)
                                Return 3
                            End If
                        End If
                    Case Else : If Not Multiple Then MsgBoxE({"Operation canceled", MsgTitle})
                End Select
            End If
            Return 0
        End Function
        Private Sub DeleteRemoveUserFromCollection(sender As Object, e As EventArgs)
            Dim obj As IUserData = DirectCast(sender, ToolStripMenuItem).Tag
            Dim i% = Collections.IndexOf(obj)
            If i >= 0 Then
                Dim n$ = Collections(i).Name
                Dim s$ = Collections(i).Site.ToString
                Dim RemoveMeIfNull As Action = Sub()
                                                   If Count = 0 Then
                                                       Settings.Users.Remove(Me)
                                                       MainFrameObj.ImageHandler(Me, False)
                                                       RaiseEvent OnCollectionSelfRemoved(Me)
                                                       Dispose(False)
                                                   End If
                                               End Sub
                Select Case MsgBoxE({$"Are you sure you want to remove user profile [{n}] of site [{s}] from collection [{Name}]?" & vbCr &
                                     "You can remove a user from the collection while keeping data (Remove) or deleting the data (Delete)" & vbCr &
                                     "Deleting this profile will remove it from the collection and all its data will be erased." & vbCr &
                                     "Removing this profile will remove it from the collection and all its data will remain." &
                                     "This user will still appear in the program, but not in the collection.",
                                     "Deleting a user"}, vbExclamation,,,
                                    {
                                        New MsgBoxButton("Remove") With {
                                            .ToolTip = "Remove a user from the collection only. All its data will remain. The user will appear in the program.",
                                            .KeyCode = Keys.Enter},
                                        New MsgBoxButton("Delete") With {
                                            .ToolTip = "Delete a user from the collection and erase their data.",
                                            .KeyCode = New ButtonKey(Keys.Enter, True)},
                                        "Cancel"
                                    }).Index
                    Case 0
                        Remove(Collections(i))
                        MsgBoxE($"User [{s} - {n}] has been removed from the collection. Now it should be displayed in the program.")
                        RemoveMeIfNull.Invoke
                    Case 1
                        Collections(i).Delete()
                        Collections(i).Dispose()
                        Collections.RemoveAt(i)
                        MsgBoxE($"User profile [{n}] of site [{s}] has been deleted")
                        RemoveMeIfNull.Invoke
                    Case Else : MsgBoxE("Operation canceled")
                End Select
            End If
        End Sub
#End Region
#Region "Copy"
        Friend Overrides Function CopyFiles(ByVal DestinationPath As SFile, Optional ByVal e As ErrorsDescriber = Nothing) As Boolean
            Return Count > 0 AndAlso Collections(0).CopyFiles(DestinationPath, e)
        End Function
#End Region
#Region "Contains"
        Friend Function Contains(ByVal _Item As IUserData) As Boolean Implements ICollection(Of IUserData).Contains
            Return Count > 0 AndAlso Collections.Contains(_Item)
        End Function
#End Region
#Region "IEnumerable Support"
        Private Function GetEnumerator() As IEnumerator(Of IUserData) Implements IEnumerable(Of IUserData).GetEnumerator
            Return New MyEnumerator(Of IUserData)(Me)
        End Function
        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
#End Region
#Region "IComparable Support"
        Friend Overrides Function CompareTo(ByVal Other As UserDataBase) As Integer
            If TypeOf Other Is UserDataBind Then
                Return CollectionName.CompareTo(Other.CollectionName)
            Else
                Return -1
            End If
        End Function
#End Region
#Region "IEquatable support"
        Friend Overrides Function Equals(ByVal Other As UserDataBase) As Boolean
            If Other.IsCollection Then
                Return CollectionName = Other.CollectionName
            Else
                Return False
            End If
        End Function
#End Region
#Region "IDisposable Support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then Collections.ListClearDispose
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace