' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.Messaging
Imports System.Threading
Imports SCrawler.API.Base
Namespace API
    Friend Class UserDataBind : Inherits UserDataBase : Implements ICollection(Of IUserData), IMyEnumerator(Of IUserData)
        Friend Event OnCollectionSelfRemoved(ByVal Collection As IUserData)
        Friend Event OnUserRemoved(ByVal User As IUserData)
#Region "Declarations"
        Friend ReadOnly Property Collections As List(Of IUserData)
        Private _CollectionName As String = String.Empty
        Friend Overrides Property CollectionName As String
            Get
                If Count > 0 Then
                    Return Collections(0).CollectionName
                Else
                    Return _CollectionName
                End If
            End Get
            Set(ByVal NewName As String)
                ChangeCollectionName(NewName, True)
            End Set
        End Property
        Friend Overrides Property Name As String
            Get
                Return CollectionName
            End Get
            Set(ByVal NewCollectionName As String)
                CollectionName = NewCollectionName
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
        Friend Overrides Sub ChangeCollectionName(ByVal NewName As String, ByVal UpdateSettings As Boolean)
            _CollectionName = NewName
            If Count > 0 Then Collections.ForEach(Sub(c) c.CollectionName = NewName)
        End Sub
        Friend Overrides Property FriendlyName As String
            Get
                If Count > 0 Then
                    Return Collections(0).FriendlyName
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal NewName As String)
                If Count > 0 Then Collections.ForEach(Sub(c)
                                                          c.FriendlyName = NewName
                                                          c.UpdateUserInformation()
                                                      End Sub)
            End Set
        End Property
#Region "Images"
        Friend Overrides Sub SetPicture(ByVal f As SFile)
            If Count > 0 Then Collections.ForEach(Sub(c) c.SetPicture(f))
        End Sub
        Friend Overrides Function GetUserPicture() As Image
            If Count > 0 Then
                Return Collections(0).GetPicture
            Else
                Return GetNullPicture(Settings.MaxLargeImageHeight)
            End If
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
        Friend ReadOnly Property Count As Integer Implements ICollection(Of IUserData).Count, IMyEnumerator(Of IUserData).MyEnumeratorCount
            Get
                If Collections Is Nothing Then
                    Return 0
                Else
                    Return Collections.Count
                End If
            End Get
        End Property
        Friend Overrides Property MyFile As SFile
            Get
                If Count > 0 Then Return Collections(0).File Else Return Nothing
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
                If Count > 0 Then
                    Return DirectCast(Collections(0), UserDataBase).DataMerging
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
        Friend Overrides Function GetUserInformation() As String
            Dim OutStr$ = String.Empty
            If Count > 0 Then Collections.ForEach(Sub(c) OutStr.StringAppendLine(DirectCast(c, UserDataBase).GetUserInformation(), $"{vbCrLf}{vbCrLf}"))
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
        Friend Overrides ReadOnly Property FitToAddParams As Boolean
            Get
                Return Count > 0 AndAlso Collections.Exists(Function(c) c.FitToAddParams)
            End Get
        End Property
        Friend Overrides Property ScriptUse As Boolean
            Get
                Return Count > 0 AndAlso Collections.Exists(Function(c) c.ScriptUse)
            End Get
            Set(ByVal u As Boolean)
                If Count > 0 Then Collections.ForEach(Sub(ByVal c As IUserData)
                                                          Dim b As Boolean = c.ScriptUse = u
                                                          c.ScriptUse = u
                                                          If Not b Then c.UpdateUserInformation()
                                                      End Sub)
            End Set
        End Property
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
            'ImageHandler(Me, True)
        End Sub
        Friend Sub New(ByVal _Name As String)
            Me.New
            CollectionName = _Name
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
                If Count > 0 Then
                    Return Collections(0).DownloadTopCount
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal NewLimit As Integer?)
                If Count > 0 Then Collections.ForEach(Sub(c) c.DownloadTopCount = NewLimit)
            End Set
        End Property
        Friend Overrides Sub DownloadData(ByVal Token As CancellationToken)
            If Count > 0 Then Downloader.AddRange(Collections)
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
        End Sub
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
        End Sub
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False) As Integer
            Return 0
        End Function
        Private Sub User_OnUserUpdated(ByVal User As IUserData)
            OnUserUpdated()
        End Sub
#End Region
#Region "Open site, folder"
        Friend Overrides Sub OpenSite(Optional ByVal e As ErrorsDescriber = Nothing)
            If Not e.Exists Then e = New ErrorsDescriber(EDP.SendInLog)
            If Count > 0 Then Collections.ForEach(Sub(c) c.OpenSite(e))
        End Sub
        Friend Overrides Sub OpenFolder()
            Try
                If Count > 0 Then GlobalOpenPath(Collections(0).File.CutPath(2))
            Catch
            End Try
        End Sub
#End Region
#Region "ICollection Support"
        Default Friend ReadOnly Property Item(ByVal Index As Integer) As IUserData Implements IMyEnumerator(Of IUserData).MyEnumeratorObject
            Get
                Return Collections(Index)
            End Get
        End Property
        Private ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of IUserData).IsReadOnly
            Get
                Return False
            End Get
        End Property
        Private Sub CopyTo(ByVal _Array() As IUserData, ByVal _ArrayIndex As Integer) Implements ICollection(Of IUserData).CopyTo
            Throw New NotImplementedException("[CopyTo] method does not supported in collections context")
        End Sub
        Friend Sub Clear() Implements ICollection(Of IUserData).Clear
            Collections.ListClearDispose
        End Sub
#End Region
#Region "Add"
        ''' <exception cref="InvalidOperationException"></exception>
        Friend Overloads Sub Add(ByVal _Item As IUserData) Implements ICollection(Of IUserData).Add
            With _Item
                If .MoveFiles(CollectionName) Then
                    If DataMerging Then DirectCast(.Self, UserDataBase).MergeData()
                    Collections.Add(_Item)
                    With Collections.Last
                        If Count > 1 Then
                            If _CollectionName.IsEmptyString Then _CollectionName = .CollectionName
                            .Temporary = Temporary
                            .Favorite = Favorite
                            .ReadyForDownload = ReadyForDownload
                            ConsolidateLabels()
                            ConsolidateScripts()
                            .UpdateUserInformation()
                        End If
                        MainFrameObj.ImageHandler(_Item, False)
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
                        .CreateButtons(Count - 1)
                        AddHandler .BTT_CONTEXT_DELETE.Click, AddressOf DeleteRemoveUserFromCollection
                    Else
                        RemoveHandler .BTT_CONTEXT_DELETE.Click, AddressOf DeleteRemoveUserFromCollection
                    End If
                End With
            Catch ex As Exception
            End Try
        End Sub
        Private Sub ConsolidateLabels()
            If Count > 1 Then
                Dim l As New List(Of String)
                Dim lp As New ListAddParams(LAP.ClearBeforeAdd)
                l.ListAddList(Collections.SelectMany(Function(c) c.Labels), LNC)
                Collections.ForEach(Sub(c) c.Labels.ListAddList(l, lp))
            End If
        End Sub
        Private Sub ConsolidateScripts()
            If Count > 1 AndAlso ScriptUse Then Collections.ForEach(Sub(c) c.ScriptUse = True)
        End Sub
        Friend Sub AddRange(ByVal _Items As IEnumerable(Of IUserData))
            If _Items.ListExists Then
                For i% = 0 To _Items.Count - 1 : Add(_Items(i)) : Next
            End If
        End Sub
#End Region
#Region "Move, Merge"
        Friend Overrides Function MoveFiles(ByVal __CollectionName As String) As Boolean
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
#Region "Remove, Delete"
        Friend Function Remove(ByVal _Item As IUserData) As Boolean Implements ICollection(Of IUserData).Remove
            If DataMerging Then
                MsgBoxE($"Collection [{CollectionName}] data is already merged" & vbCr &
                        "Combined data can not be undone" & vbCr &
                        "Operation canceled", MsgBoxStyle.Critical)
                Return False
            Else
                DirectCast(_Item, UserDataBase).MoveFiles(String.Empty)
                MainFrameObj.ImageHandler(_Item)
                AddRemoveBttDeleteHandler(_Item, False)
                RaiseEvent OnUserRemoved(_Item)
                Return Collections.Remove(_Item)
            End If
        End Function
        Friend Overrides Function Delete() As Integer
            If Count > 0 Then
                Dim f As SFile
                If MsgBoxE({$"Collection may contain data{vbCr}Do you really want to delete collection and all of it files?", "Collection deleting"},
                           MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    f = Collections(0).File.CutPath(IIf(DataMerging, 1, 2)).PathWithSeparator
                    Settings.Users.Remove(Me)
                    Collections.ForEach(Sub(c) c.Delete())
                    Downloader.UserRemove(Me)
                    MainFrameObj.ImageHandler(Me, False)
                    Collections.ListClearDispose
                    Dispose(False)
                    f.Delete(SFO.Path, SFODelete.EmptyOnly + Settings.DeleteMode, EDP.SendInLog)
                    Return 2
                Else
                    If DataMerging Then
                        MsgBoxE($"Collection [{CollectionName}] data are already merged{vbCr}Cannot split merged collection{vbCr}Operation canceled", MsgBoxStyle.Exclamation)
                        Return 0
                    End If
                    If MsgBoxE({"Do you want to delete only the collection and split users' profiles??" & vbCr &
                                "Users will be removed from the collection and split by sites." & vbCr &
                                "All user data will remain.", "Collection deleting"},
                               MsgBoxStyle.Question + MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                        f = Collections(0).File.CutPath(2)
                        Settings.Users.Remove(Me)
                        Collections.ForEach(Sub(c)
                                                c.MoveFiles(String.Empty)
                                                MainFrameObj.ImageHandler(c)
                                            End Sub)
                        Collections.Clear()
                        f.Delete(SFO.Path, SFODelete.Default + Settings.DeleteMode, EDP.SendInLog)
                        Downloader.UserRemove(Me)
                        MainFrameObj.ImageHandler(Me, False)
                        Dispose(False)
                        Return 3
                    Else
                        MsgBoxE("Operation canceled")
                    End If
                End If
            End If
            Return 0
        End Function
        Private Sub DeleteRemoveUserFromCollection(sender As Object, e As EventArgs)
            With DirectCast(sender, ToolStripMenuItem)
                Dim i% = AConvert(Of Integer)(.Tag, -1)
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
                                            .ToolTip = "Remove a user from the collection only. All its data will remain. The user will appear in the program."},
                                          New MsgBoxButton("Delete") With {
                                            .ToolTip = "Delete a user from the collection and erase their data."},
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
            End With
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
        Friend Overrides Function Equals(ByVal Other As UserDataBase) As Boolean
            If Other.IsCollection Then
                Return CollectionName = Other.CollectionName
            Else
                Return Count > 0 AndAlso Collections.Exists(Function(u) u.Equals(Other))
            End If
        End Function
#Region "IDisposable Support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then Collections.ListClearDispose
            End If
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace