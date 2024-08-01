' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Functions.XML
Imports UserMediaD = SCrawler.DownloadObjects.TDownloader.UserMediaD
Namespace DownloadObjects
    Friend Class FeedSpecial : Implements IEnumerable(Of UserMediaD), IMyEnumerator(Of UserMediaD), IDisposableSuspend
#Region "SEComparer"
        Friend Class SEComparer : Implements IComparer(Of UserMediaD)
            Friend Function Compare(ByVal x As UserMediaD, ByVal y As UserMediaD) As Integer Implements IComparer(Of UserMediaD).Compare
                Dim v% = x.Date.Ticks.CompareTo(y.Date.Ticks) * -1
                If v <> 0 Then Return v
                v = If(x.User?.GetHashCode, 0).CompareTo(If(y.User?.GetHashCode, 0))
                If v <> 0 Then Return v
                Return 0
            End Function
        End Class
#End Region
#Region "Events"
        Friend Event FeedDeleted As FeedSpecialCollection.FeedActionEventHandler
#End Region
#Region "Declarations"
        Friend Const FavoriteName As String = "Favorite"
        Friend Const SpecialName As String = "Special"
        Private ReadOnly Items As List(Of UserMediaD)
        Private _FilesUpdated As Boolean = False
        Private _File As SFile
        Friend ReadOnly Property File As SFile
            Get
                If _File.IsEmptyString AndAlso Not Name.IsEmptyString Then
                    If _IsFavorite Then
                        _File = $"{TDownloader.SessionsPath}{FavoriteName}.xml"
                    Else
                        _File = $"{TDownloader.SessionsPath}{SpecialName}_{Name}.xml"
                    End If
                End If
                Return _File
            End Get
        End Property
        Private _IsFavorite As Boolean
        Friend ReadOnly Property IsFavorite As Boolean
            Get
                Return _IsFavorite
            End Get
        End Property
        Private _Name As String
        Friend ReadOnly Property Name As String
            Get
                If _Name.IsEmptyString And IsFavorite Then
                    Return FavoriteName
                Else
                    Return _Name
                End If
            End Get
        End Property
#End Region
#Region "Initializers"
        Private Sub New()
            Items = New List(Of UserMediaD)
        End Sub
        Friend Sub New(ByVal f As SFile)
            Me.New
            _File = f
            If Not File.Name.IsEmptyString Then
                _IsFavorite = File.Name.StartsWith(FavoriteName)
                If Not _IsFavorite Then _Name = File.Name.Split("_").ListTake(0, 100, EDP.ReturnValue).ListToString("").StringTrim.StringTrimStart("_")
            End If
            Load()
        End Sub
        Friend Shared Function CreateFavorite() As FeedSpecial
            Return New FeedSpecial With {._IsFavorite = True}
        End Function
        Friend Shared Function CreateSpecial(ByVal Name As String) As FeedSpecial
            Return New FeedSpecial With {._Name = Name}
        End Function
        Friend Sub Load()
            If File.Exists Then
                Using x As New XmlFile(File, Protector.Modes.All, False) With {.AllowSameNames = True}
                    x.LoadData()
                    If x.Count > 0 Then Items.ListAddList(x, LAP.IgnoreICopier)
                End Using
            End If
        End Sub
#End Region
#Region "Data functions"
#Region "Item, Count"
        Default Friend ReadOnly Property Item(ByVal Index As Integer) As UserMediaD Implements IMyEnumerator(Of UserMediaD).MyEnumeratorObject
            Get
                Return Items(Index)
            End Get
        End Property
        Friend ReadOnly Property Count As Integer Implements IMyEnumerator(Of UserMediaD).MyEnumeratorCount
            Get
                Return Items.Count
            End Get
        End Property
#End Region
#Region "Clear, Sort, Save"
        Friend Function Clear()
            Dim result As Boolean = Count > 0
            Items.Clear()
            If result Then Save()
            Return result
        End Function
        Friend Sub Sort()
            If Count > 0 Then Items.Sort(Settings.Feeds.Comparer)
        End Sub
        Friend Sub Save()
            If Not File.IsEmptyString Then
                Sort()
                Using x As New XmlFile With {.Name = "Feed", .AllowSameNames = True}
                    x.AddRange(Items)
                    x.Save(File, EDP.SendToLog)
                End Using
            End If
        End Sub
#End Region
#Region "UpdateUsers"
        Friend Overloads Sub UpdateUsers(ByVal InitialUser As UserInfo, ByVal NewUser As UserInfo)
            Try
                If Count > 0 Then
                    Dim changed As Boolean = False
                    Dim result As Boolean
                    Dim item As UserMediaD
                    For i% = 0 To Count - 1
                        item = Items(i)
                        result = False
                        item = UpdateUsers(item, InitialUser, NewUser, result)
                        If result Then changed = True : Items(i) = item
                    Next
                    If changed Then Save()
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[FeedSpecial.UpdateUsers]")
            End Try
        End Sub
        Friend Overloads Shared Function UpdateUsers(ByVal Item As UserMediaD, ByVal InitialUser As UserInfo, ByVal NewUser As UserInfo,
                                                     ByRef Result As Boolean) As UserMediaD
            Dim data As UserMedia
            Dim user As IUserData
            Dim path$ = InitialUser.File.CutPath.PathWithSeparator
            Dim pathNew$ = NewUser.File.CutPath.PathWithSeparator
            Dim isSaved As Boolean = Item.IsSavedPosts
            If Item.UserInfo.Equals(InitialUser) Or Item.UserInfo.Equals(NewUser) Then
                If Item.Data.File.PathWithSeparator.Contains(path) Then
                    data = Item.Data
                    data.File = data.File.ToString.Replace(path, pathNew)
                    If Item.User Is Nothing Then
                        user = Settings.GetUser(NewUser)
                    Else
                        user = Item.User
                    End If
                    If isSaved Then isSaved = Item.UserInfo.Equals(DirectCast(user, UserDataBase).User)
                    If Not If(user?.IsSubscription, False) Then
                        Item = New UserMediaD(data, user, Item.Session, Item.Date) With {.IsSavedPosts = isSaved}
                        Result = True
                        Return Item
                    End If
                End If
            End If
            Result = False
            Return Item
        End Function
#End Region
#Region "UpdateDataByFile"
        Friend Sub UpdateDataByFile(ByVal InitialFile As SFile, ByVal NewFile As SFile, ByVal MCTOptions As FeedMoveCopyTo)
            Try
                Dim indx% = Items.FindIndex(Function(ii) ii.Data.File = InitialFile)
                If indx >= 0 Then
                    Dim m As UserMediaD = Items(indx)
                    Dim mm As UserMedia = m.Data
                    Dim user As UserInfo = m.UserInfo
                    Dim __isSavedPosts As Boolean = m.IsSavedPosts And MCTOptions.ReplaceUserProfile_Profile Is Nothing
                    mm.File = NewFile
                    m = New UserMediaD(mm, If(MCTOptions.ReplaceUserProfile_Profile, m.User), m.Session, m.Date) With {.IsSavedPosts = __isSavedPosts}
                    If __isSavedPosts Then m.UserInfo = user
                    Items(indx) = m
                    _FilesUpdated = True
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[FeedSpecial.UpdateDataByFile]")
            End Try
        End Sub
        Friend Sub UpdateIfRequired()
            If _FilesUpdated Then Save() : _FilesUpdated = False
        End Sub
#End Region
#Region "Add"
        Friend Overloads Function Add(ByVal Item As UserMediaD, Optional ByVal AutoSave As Boolean = True) As Boolean
            If Not Items.Contains(Item) Then
                Item.PostUrl = Item.PostUrl(True)
                Items.Add(Item)
                If AutoSave Then Save()
                Return True
            Else
                Return False
            End If
        End Function
        Friend Overloads Function Add(ByVal Items As IEnumerable(Of UserMediaD), Optional ByVal AutoSave As Boolean = True) As Integer
            Dim ri% = 0
            If Items.ListExists Then
                For Each item As UserMediaD In Items : ri += Add(item, False).BoolToInteger : Next
                If ri > 0 And AutoSave Then Save()
            End If
            Return ri
        End Function
#End Region
#Region "Remove"
        Friend Overloads Function Remove(ByVal Item As UserMediaD, Optional ByVal AutoSave As Boolean = True) As Boolean
            If Count > 0 Then
                Dim i% = Items.IndexOf(Item)
                If i >= 0 Then
                    Items.RemoveAt(i)
                    If AutoSave Then Save()
                    Return True
                End If
            End If
            Return False
        End Function
        Friend Overloads Function Remove(ByVal Items As IEnumerable(Of UserMediaD), Optional ByVal AutoSave As Boolean = True) As Integer
            Dim ri% = 0
            If Items.ListExists Then
                For Each item As UserMediaD In Items : ri += Me.Items.Remove(item).BoolToInteger : Next
                If ri > 0 And AutoSave Then Save()
            End If
            Return ri
        End Function
        Private _NotExistRemoved As Boolean = False
        Friend Function RemoveNotExist(ByVal p As Predicate(Of UserMediaD)) As Integer
            If Count > 0 And Not _NotExistRemoved Then
                _NotExistRemoved = True
                Dim ri% = 0
                If Settings.FeedSpecialSearchForMissing Then
                    Dim i% = -1
                    Dim d As UserMediaD = Nothing
                    Dim m As UserMedia
                    Dim f As SFile = Nothing
                    Dim ff As SFile
                    Dim postUrl$ = String.Empty
                    Dim user As IUserData
                    Dim processRemove As Boolean
                    Dim userArr As New List(Of IUserData)

                    Dim updateUser As Func(Of IUserData, Boolean) =
                        Function(ByVal replaceUser As IUserData) As Boolean
                            If f.Exists(SFO.Path, False) Then
                                ff = SFile.GetFiles(f, d.Data.File.File, IO.SearchOption.AllDirectories, EDP.ReturnValue).FirstOrDefault
                                If Not ff.IsEmptyString Then
                                    m = d.Data
                                    m.File = ff
                                    d = New UserMediaD(m, If(replaceUser, d.User), d.Session, d.Date) With {
                                        .IsSavedPosts = If(replaceUser Is Nothing, d.IsSavedPosts, DirectCast(replaceUser, UserDataBase).IsSavedPosts),
                                        .PostUrl = postUrl
                                    }
                                    Items(i) = d
                                    ri += 1
                                    processRemove = False
                                    Return True
                                End If
                            End If
                            Return False
                        End Function

                    For i = Count - 1 To 0 Step -1
                        If p.Invoke(Items(i)) Then
                            d = Items(i)
                            postUrl = d.PostUrl(True)
                            f = Nothing
                            ff = Nothing
                            processRemove = True

                            f = If(d.User?.File, New SFile).IfNullOrEmpty(d.UserInfo.File).CutPath
                            If updateUser(Nothing) Then Continue For

                            If Settings.FeedSpecialSearchForMissing_Deep Then
                                If userArr.Count = 0 Then userArr.ListAddList(Settings.GetUsers(Function(u) True))
                                If userArr.Count > 0 Then
                                    For Each user In userArr
                                        f = user.File.CutPath
                                        If updateUser(user) Then Exit For
                                    Next
                                End If
                            End If

                            If processRemove Then Items.RemoveAt(i) : ri += 1
                        End If
                    Next
                Else
                    ri = Items.RemoveAll(p)
                End If
                If ri > 0 Then Save()
                Return ri
            Else
                Return 0
            End If
        End Function
#End Region
#Region "Delete"
        Friend Overloads Function Delete() As Boolean
            If File.Delete(SFO.File, SFODelete.DeleteToRecycleBin, EDP.ReturnValue) Then
                Items.Clear()
                RaiseEvent FeedDeleted(Settings.Feeds, Me)
                Return True
            Else
                Return False
            End If
        End Function
        Friend Overloads Function Delete(ByVal Item As UserMediaD, Optional ByVal AutoSave As Boolean = True) As Boolean
            Dim result As Boolean = False
            If Item.Data.File.Exists Then result = Item.Data.File.Delete(SFO.File, SFODelete.DeleteToRecycleBin, EDP.ReturnValue)
            If result And AutoSave Then Save()
            Return result
        End Function
#End Region
#Region "Contains"
        Friend Function Contains(ByVal Item As UserMediaD) As Boolean
            Return Items.Contains(Item)
        End Function
#End Region
#End Region
#Region "Base Overrides"
        Public Overrides Function ToString() As String
            Return Name
        End Function
        Public Overrides Function GetHashCode() As Integer
            Return Name.GetHashCode
        End Function
        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            If Not IsNothing(Obj) Then
                If TypeOf Obj Is FeedSpecial Then
                    Return Name = DirectCast(Obj, FeedSpecial).Name
                ElseIf TypeOf Obj Is String Then
                    Return Name = CStr(Obj)
                End If
            End If
            Return False
        End Function
#End Region
#Region "IEnumerable Support"
        Private Function GetEnumerator() As IEnumerator(Of UserMediaD) Implements IEnumerable(Of UserMediaD).GetEnumerator
            Return New MyEnumerator(Of UserMediaD)(Me)
        End Function
        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
#End Region
#Region "IDisposable Support"
        Friend Property DisposeSuspended As Boolean Implements IDisposableSuspend.DisposeSuspended
            Get
                Return IsFavorite
            End Get
            Private Set : End Set
        End Property
        Friend ReadOnly Property Disposed As Boolean Implements IDisposableSuspend.Disposed
            Get
                Return disposedValue
            End Get
        End Property
        Private disposedValue As Boolean = False
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then Items.Clear()
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
End Namespace