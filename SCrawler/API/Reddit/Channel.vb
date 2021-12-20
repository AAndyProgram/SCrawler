' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Functions.XML
Imports SCrawler.API.Base
Imports System.Threading
Namespace API.Reddit
    Friend Class Channel : Implements ICollection(Of UserPost), IEquatable(Of Channel), IComparable(Of Channel),
            IRangeSwitcherContainer(Of UserPost), ILoaderSaver, IMyEnumerator(Of UserPost), IChannelLimits, IUserData, IDisposable
#Region "XML Nodes' Names"
        Private Const Name_Name As String = "Name"
        Private Const Name_ID As String = "ID"
        Private Const Name_Date As String = "Date"
        Private Const Name_PostsNode As String = "Posts"
#End Region
        Friend Const DefaultDownloadLimitCount As Integer = 1000
#Region "IUserData Support"
        Private Event OnUserUpdated As IUserData.OnUserUpdatedEventHandler Implements IUserData.OnUserUpdated
        Friend Property Instance As IUserData
        Private Property IUserData_ParseUserMediaOnly As Boolean = False Implements IUserData.ParseUserMediaOnly
        Private Property IUserData_Exists As Boolean Implements IUserData.Exists
            Get
                Return Instance.Exists
            End Get
            Set(ByVal e As Boolean)
            End Set
        End Property
        Private Property IUserData_Suspended As Boolean Implements IUserData.Suspended
            Get
                Return Instance.Suspended
            End Get
            Set(ByVal s As Boolean)
            End Set
        End Property
        Private ReadOnly Property IUserData_IsCollection As Boolean Implements IUserData.IsCollection
            Get
                Return Instance.IsCollection
            End Get
        End Property
        Private Property IUserData_CollectionName As String Implements IUserData.CollectionName
            Get
                Return Instance.CollectionName
            End Get
            Set(ByVal NewName As String)
                Instance.CollectionName = NewName
            End Set
        End Property
        Private ReadOnly Property IUserData_IncludedInCollection As Boolean Implements IUserData.IncludedInCollection
            Get
                Return Instance.IncludedInCollection
            End Get
        End Property
        Private ReadOnly Property IUserData_Labels As List(Of String) Implements IUserData.Labels
            Get
                Return Instance.Labels
            End Get
        End Property
        Private ReadOnly Property IUserData_IsChannel As Boolean = True Implements IUserData.IsChannel
        Private Property IUserData_ReadyForDownload As Boolean Implements IUserData.ReadyForDownload
            Get
                Return Instance.ReadyForDownload
            End Get
            Set(ByVal IsReady As Boolean)
                Instance.ReadyForDownload = IsReady
            End Set
        End Property
        Private Property IUserData_File As SFile Implements IUserData.File
            Get
                Return Instance.File
            End Get
            Set(ByVal NewFile As SFile)
                Instance.File = NewFile
            End Set
        End Property
        Private Property IUserData_FileExists As Boolean Implements IUserData.FileExists
            Get
                Return Instance.FileExists
            End Get
            Set(ByVal IsExists As Boolean)
                Instance.FileExists = IsExists
            End Set
        End Property
        Private Property IUserData_DownloadedPictures As Integer Implements IUserData.DownloadedPictures
            Get
                Return Instance.DownloadedPictures
            End Get
            Set(ByVal c As Integer)
                Instance.DownloadedPictures = c
            End Set
        End Property
        Private Property IUserData_DownloadedVideos As Integer Implements IUserData.DownloadedVideos
            Get
                Return Instance.DownloadedVideos
            End Get
            Set(ByVal c As Integer)
                Instance.DownloadedVideos = c
            End Set
        End Property
        Private ReadOnly Property IUserData_DownloadedTotal(Optional Total As Boolean = True) As Integer Implements IUserData.DownloadedTotal
            Get
                Return Instance.DownloadedTotal
            End Get
        End Property
        Private ReadOnly Property IUserData_DownloadedInformation As String Implements IUserData.DownloadedInformation
            Get
                Return Instance.DownloadedInformation
            End Get
        End Property
        Private Property IUserData_HasError As Boolean Implements IUserData.HasError
            Get
                Return Instance.HasError
            End Get
            Set(ByVal e As Boolean)
                Instance.HasError = e
            End Set
        End Property
        Private ReadOnly Property IUserData_FitToAddParams As Boolean Implements IUserData.FitToAddParams
            Get
                Return Instance.FitToAddParams
            End Get
        End Property
        Private ReadOnly Property IUserData_LVIKey As String Implements IUserData.LVIKey
            Get
                Return Instance.LVIKey
            End Get
        End Property
        Private ReadOnly Property IUserData_LVIIndex As Integer Implements IUserData.LVIIndex
            Get
                Return Instance.LVIIndex
            End Get
        End Property
        Private Property IUserData_DownloadImages As Boolean Implements IUserData.DownloadImages
            Get
                Return Instance.DownloadImages
            End Get
            Set(ByVal d As Boolean)
                Instance.DownloadImages = d
            End Set
        End Property
        Private Property IUserData_DownloadVideos As Boolean Implements IUserData.DownloadVideos
            Get
                Return Instance.DownloadVideos
            End Get
            Set(ByVal d As Boolean)
                Instance.DownloadVideos = d
            End Set
        End Property
        Private ReadOnly Property IUserData_Self As IUserData Implements IUserData.Self
            Get
                Return Instance
            End Get
        End Property
        Private Property IUserData_DownloadTopCount As Integer? Implements IUserData.DownloadTopCount
            Get
                Return Instance.DownloadTopCount
            End Get
            Set(ByVal c As Integer?)
                Instance.DownloadTopCount = c
            End Set
        End Property
        Friend Property Site As Sites = Sites.Reddit Implements IContentProvider.Site
        Private Property IUserData_FriendlyName As String Implements IContentProvider.FriendlyName
            Get
                Return Instance.FriendlyName
            End Get
            Set(ByVal NewName As String)
                Instance.FriendlyName = NewName
            End Set
        End Property
        Private Property IUserData_Description As String Implements IContentProvider.Description
            Get
                Return Instance.Description
            End Get
            Set(ByVal d As String)
                Instance.Description = d
            End Set
        End Property
        Private Property IUserData_Favorite As Boolean Implements IContentProvider.Favorite
            Get
                Return Instance.Favorite
            End Get
            Set(ByVal f As Boolean)
                Instance.Favorite = f
            End Set
        End Property
        Private Property IUserData_Temporary As Boolean Implements IContentProvider.Temporary
            Get
                Return Instance.Temporary
            End Get
            Set(ByVal t As Boolean)
                Instance.Temporary = t
            End Set
        End Property
        Private Sub IUserData_SetPicture(ByVal f As SFile) Implements IUserData.SetPicture
            Instance.SetPicture(f)
        End Sub
        Private Sub IUserData_LoadUserInformation() Implements IUserData.LoadUserInformation
            Instance.LoadUserInformation()
        End Sub
        Private Sub IUserData_UpdateUserInformation() Implements IUserData.UpdateUserInformation
            Instance.UpdateUserInformation()
        End Sub
        Private Sub IUserData_OpenFolder() Implements IUserData.OpenFolder
            Instance.OpenFolder()
        End Sub
        Private Sub IUserData_OpenSite() Implements IContentProvider.OpenSite
            Instance.OpenSite()
        End Sub
        Private Sub IUserData_DownloadData(ByVal Token As CancellationToken) Implements IContentProvider.DownloadData
            DownloadData(Token, False, Nothing)
        End Sub
        Private Function IUserData_GetPicture() As Image Implements IUserData.GetPicture
            Return Instance.GetPicture()
        End Function
        Private Function IUserData_GetLVI(ByVal Destination As ListView) As ListViewItem Implements IUserData.GetLVI
            Return Instance.GetLVI(Destination)
        End Function
        Private Function IUserData_GetLVIGroup(ByVal Destination As ListView) As ListViewGroup Implements IUserData.GetLVIGroup
            Return Instance.GetLVIGroup(Destination)
        End Function
        Private Function IUserData_Delete() As Integer Implements IUserData.Delete
            Return DirectCast(Instance, UserDataBase).DeleteF(Me)
        End Function
        Private Function IUserData_MoveFiles(ByVal CollectionName As String) As Boolean Implements IUserData.MoveFiles
            Return DirectCast(Instance, UserDataBase).MoveFilesF(Me, CollectionName)
        End Function
#End Region
        Private _Name As String = String.Empty
        Friend Property Name As String Implements IUserData.Name
            Get
                If IsRegularChannel Then
                    Return Instance.Name
                Else
                    Return _Name
                End If
            End Get
            Set(ByVal NewName As String)
                If IsRegularChannel Then
                    Instance.Name = NewName
                Else
                    _Name = NewName
                End If
            End Set
        End Property
        Private _ID As String = String.Empty
        Friend Property ID As String Implements IUserData.ID
            Get
                If IsRegularChannel Then
                    Return Instance.ID
                Else
                    Return _ID
                End If
            End Get
            Set(ByVal NewID As String)
                If IsRegularChannel Then
                    Instance.ID = NewID
                Else
                    _ID = NewID
                End If
            End Set
        End Property
        Friend ReadOnly Property CUser As UserInfo
            Get
                Return New UserInfo(Me)
            End Get
        End Property
        Friend ReadOnly Property PostsLatest As List(Of UserPost)
        Friend ReadOnly Property Posts As List(Of UserPost)
        Friend ReadOnly Property PostsAll As List(Of UserPost)
            Get
                Return ListAddList(Nothing, Posts).ListAddList(PostsLatest).ListSort
            End Get
        End Property
        Private ReadOnly Property Source As IEnumerable(Of UserPost) Implements IRangeSwitcherContainer(Of UserPost).Source
            Get
                Return Posts
            End Get
        End Property
        Friend Property LatestParsedDate As Date? = Nothing
        Private _Downloading As Boolean = False
        Friend ReadOnly Property Downloading As Boolean
            Get
                Return _Downloading
            End Get
        End Property
        Friend ReadOnly Property File As SFile
            Get
                Return $"{ChannelsCollection.ChannelsPath.PathWithSeparator}{ID}.xml"
            End Get
        End Property
        Friend ReadOnly Property CachePath As SFile
            Get
                Return $"{ChannelsCollection.ChannelsPathCache.PathWithSeparator}{ID}\"
            End Get
        End Property
        Friend ReadOnly Property Count As Integer Implements ICollection(Of UserPost).Count, IMyEnumerator(Of UserPost).MyEnumeratorCount
            Get
                Return Posts.Count
            End Get
        End Property
        Default Friend ReadOnly Property Item(ByVal Index As Integer) As UserPost Implements IMyEnumerator(Of UserPost).MyEnumeratorObject
            Get
                Return Posts(Index)
            End Get
        End Property
        Private ReadOnly Property Range As RangeSwitcher(Of UserPost)
#Region "Limits Support"
        Private _DownloadLimitCount As Integer? = Nothing
        Friend Property DownloadLimitCount As Integer? Implements IChannelLimits.DownloadLimitCount
            Get
                If AutoGetLimits Then
                    If LatestParsedDate.HasValue OrElse Not DownloadLimitPost.IsEmptyString Then
                        Return Nothing
                    ElseIf _DownloadLimitCount.HasValue Then
                        Return _DownloadLimitCount
                    Else
                        Return DefaultDownloadLimitCount
                    End If
                Else
                    Return _DownloadLimitCount
                End If
            End Get
            Set(ByVal NewLimit As Integer?)
                _DownloadLimitCount = NewLimit
            End Set
        End Property
        Private _DownloadLimitPost As String = String.Empty
        Friend Property DownloadLimitPost As String Implements IChannelLimits.DownloadLimitPost
            Get
                Dim PID$ = ListAddList(Nothing, Posts, LAP.NotContainsOnly).ListAddList(PostsLatest, LAP.NotContainsOnly).ListSort.FirstOrDefault.ID
                If AutoGetLimits And Not PID.IsEmptyString Then
                    Return PID
                Else
                    Return _DownloadLimitPost
                End If
            End Get
            Set(ByVal NewLimit As String)
                _DownloadLimitPost = NewLimit
            End Set
        End Property
        Private _DownloadLimitDate As Date? = Nothing
        Friend Property DownloadLimitDate As Date? Implements IChannelLimits.DownloadLimitDate
            Get
                If AutoGetLimits And LatestParsedDate.HasValue Then
                    Return LatestParsedDate
                Else
                    Return _DownloadLimitDate
                End If
            End Get
            Set(ByVal NewLimit As Date?)
                _DownloadLimitDate = NewLimit
            End Set
        End Property
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
        Friend ReadOnly IsRegularChannel As Boolean = False
        Friend Sub New()
            Posts = New List(Of UserPost)
            PostsLatest = New List(Of UserPost)
            Range = New RangeSwitcher(Of UserPost)(Me)
        End Sub
        Friend Sub New(ByVal f As SFile)
            Me.New
            LoadData(f, False)
        End Sub
        Friend Sub New(ByVal u As UserInfo, Optional ByVal _LoadUserInformation As Boolean = True)
            Me.New
            Instance = New UserData(u, _LoadUserInformation) With {.SaveToCache = False, .SkipExistsUsers = False, .ChannelInfo = Me}
            AutoGetLimits = True
            DirectCast(Instance, UserData).SetLimit(Me)
            IsRegularChannel = True
        End Sub
        Public Shared Widening Operator CType(ByVal f As SFile) As Channel
            Return New Channel(f)
        End Operator
        Public Shared Widening Operator CType(ByVal c As Channel) As UserDataBase
            Return DirectCast(c.Instance, UserDataBase)
        End Operator
        Public Overrides Function ToString() As String
            If Not Name.IsEmptyString Then
                Return Name
            Else
                Return ID
            End If
        End Function
        Friend Sub Delete()
            If File.Exists Then File.Delete()
        End Sub
        Friend Sub DownloadData(ByVal Token As CancellationToken, Optional ByVal SkipExists As Boolean = True,
                                Optional ByVal p As MyProgress = Nothing)
            Try
                _Downloading = True
                If Not Instance Is Nothing Then
                    Instance.DownloadData(Token)
                Else
                    Using d As New UserData(CUser, False, False) With {
                        .Progress = p,
                        .SaveToCache = True,
                        .SkipExistsUsers = SkipExists,
                        .ChannelInfo = Me
                    }
                        d.SetLimit(Me)
                        d.DownloadData(Token)
                        Posts.ListAddList(d.GetNewChannelPosts(), LAP.NotContainsOnly)
                        Posts.Sort()
                        LatestParsedDate = If(Posts.FirstOrDefault(Function(pp) pp.Date.HasValue).Date, LatestParsedDate)
                    End Using
                End If
            Catch oex As OperationCanceledException When Token.IsCancellationRequested
            Finally
                _Downloading = False
            End Try
        End Sub
#Region "ICollection Support"
        Private ReadOnly Property IsReadOnly As Boolean = False Implements ICollection(Of UserPost).IsReadOnly
        Friend Sub Add(ByVal _Item As UserPost) Implements ICollection(Of UserPost).Add
            If Not Contains(_Item) Then Posts.Add(_Item)
        End Sub
        Friend Sub AddRange(ByVal _Items As IEnumerable(Of UserPost))
            If _Items.ListExists Then
                For Each i As UserPost In _Items : Add(i) : Next
            End If
        End Sub
        Friend Sub Clear() Implements ICollection(Of UserPost).Clear
            Posts.Clear()
        End Sub
        Friend Function Contains(ByVal _Item As UserPost) As Boolean Implements ICollection(Of UserPost).Contains
            Return Count > 0 AndAlso Posts.Contains(_Item)
        End Function
        Private Sub CopyTo(ByVal _Array() As UserPost, ByVal ArrayIndex As Integer) Implements ICollection(Of UserPost).CopyTo
            Throw New NotImplementedException()
        End Sub
        Friend Function Remove(ByVal _Item As UserPost) As Boolean Implements ICollection(Of UserPost).Remove
            Return Posts.Remove(_Item)
        End Function
#End Region
#Region "IEnumerable Support"
        Friend Function GetEnumerator() As IEnumerator(Of UserPost) Implements IEnumerable(Of UserPost).GetEnumerator
            Return New MyEnumerator(Of UserPost)(Me)
        End Function
        Friend Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
#End Region
#Region "IEquatable Support"
        Friend Overloads Function Equals(ByVal Other As Channel) As Boolean Implements IEquatable(Of Channel).Equals
            Return ID = Other.ID
        End Function
        Private Overloads Function Equals(ByVal Other As UserDataBase) As Boolean Implements IEquatable(Of UserDataBase).Equals
            If Not Instance Is Nothing Then
                Return Instance.Equals(Other)
            Else
                Return False
            End If
        End Function
        Public Overloads Overrides Function Equals(ByVal Obj As Object) As Boolean
            If Not Obj Is Nothing Then
                If TypeOf Obj Is String Then
                    Return ID = CStr(Obj)
                ElseIf TypeOf Obj Is Channel Then
                    Return Equals(DirectCast(Obj, Channel))
                ElseIf TypeOf Obj Is UserDataBase Then
                    Return Equals(DirectCast(Obj, UserDataBase))
                End If
            End If
            Return False
        End Function
#End Region
#Region "IComparable Support"
        Friend Overloads Function CompareTo(ByVal Other As Channel) As Integer Implements IComparable(Of Channel).CompareTo
            If Not Name.IsEmptyString And Not Other.Name.IsEmptyString Then
                Return Name.CompareTo(Other.Name)
            Else
                Return ID.CompareTo(Other.ID)
            End If
        End Function
        Private Overloads Function CompareTo(ByVal Other As UserDataBase) As Integer Implements IComparable(Of UserDataBase).CompareTo
            If Not Instance Is Nothing Then
                Return Instance.CompareTo(Other)
            Else
                Return 0
            End If
        End Function
        Private Overloads Function CompareTo(ByVal Obj As Object) As Integer Implements IComparable.CompareTo
            If TypeOf Obj Is Channel Then
                Return CompareTo(DirectCast(Obj, Channel))
            ElseIf TypeOf Obj Is UserDataBase And Not Instance Is Nothing Then
                Return Instance.CompareTo(Obj)
            Else
                Return 0
            End If
        End Function
#End Region
#Region "ILoaderSaver Support"
        Friend Overloads Function LoadData(Optional ByVal f As SFile = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Boolean Implements ILoaderSaver.Load
            Return LoadData(File, False, e)
        End Function
        Friend Overloads Function LoadData(ByVal f As SFile, ByVal PartialLoad As Boolean, Optional ByVal e As ErrorsDescriber = Nothing) As Boolean
            If f.Exists Then
                Using x As New XmlFile(f, Protector.Modes.All, False) With {.XmlReadOnly = True, .AllowSameNames = True}
                    x.LoadData()
                    If x.Count > 0 Then
                        Dim XMLDateProvider As New ADateTime(ADateTime.Formats.BaseDateTime)
                        Name = x.Value(Name_Name)
                        ID = x.Value(Name_ID)
                        LatestParsedDate = AConvert(Of Date)(x.Value(Name_Date), XMLDateProvider, Nothing)
                        If Not PartialLoad Then
                            With x(Name_PostsNode).XmlIfNothing
                                If .Count > 0 Then .ForEach(Sub(ee) PostsLatest.Add(New UserPost With {
                                                                        .ID = ee.Attribute(Name_ID),
                                                                        .[Date] = AConvert(Of Date)(ee.Attribute(Name_Date).Value, XMLDateProvider, Nothing)}))
                            End With
                        End If
                    End If
                End Using
            End If
            Return True
        End Function
        Friend Overloads Function Save(Optional ByVal f As SFile = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Boolean Implements ILoaderSaver.Save
            Dim XMLDateProvider As New ADateTime(ADateTime.Formats.BaseDateTime)
            Using x As New XmlFile With {.AllowSameNames = True, .Name = "Channel"}
                x.Add(Name_Name, Name)
                x.Add(Name_ID, ID)
                If Posts.Count > 0 Or PostsLatest.Count > 0 Then
                    Dim tmpPostList As List(Of UserPost) = Nothing
                    tmpPostList.ListAddList(Posts).ListAddList(PostsLatest)
                    tmpPostList.Sort()
                    LatestParsedDate = tmpPostList.FirstOrDefault(Function(pd) pd.Date.HasValue).Date
                    x.Add(Name_Date, AConvert(Of String)(LatestParsedDate, XMLDateProvider, String.Empty))
                    x.Add(Name_PostsNode, String.Empty)
                    With x(Name_PostsNode)
                        tmpPostList.Take(200).ToList.ForEach(Sub(p) .Add(New EContainer("Post",
                                                                                  String.Empty,
                                                                                  {
                                                                                    New EAttribute(Name_ID, p.ID),
                                                                                    New EAttribute(Name_Date, AConvert(Of String)(p.Date, XMLDateProvider, String.Empty))
                                                                                  })
                                                                  )
                                                      )
                    End With
                    tmpPostList.Clear()
                End If
                x.Save(File)
            End Using
            Return True
        End Function
#End Region
#Region "IDisposable Support"
        Private disposedValue As Boolean = False
        Friend ReadOnly Property Disposed As Boolean Implements IUserData.Disposed
            Get
                Return disposedValue
            End Get
        End Property
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Posts.Clear()
                    PostsLatest.Clear()
                    Range.Dispose()
                    If Not Instance Is Nothing Then Instance.Dispose()
                    If CachePath.Exists(SFO.Path, False) Then CachePath.Delete(SFO.Path, False, False, EDP.SendInLog)
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
End Namespace