Imports PersonalUtilities.Tools
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Functions.XML
Imports SCrawler.API.Base
Namespace API.Reddit
    Friend Class Channel : Implements ICollection(Of UserPost), IEquatable(Of Channel), IComparable(Of Channel),
            IRangeSwitcherContainer(Of UserPost), ILoaderSaver, IMyEnumerator(Of UserPost), IChannelLimits, IDisposable
#Region "XML Nodes' Names"
        Private Const Name_Name As String = "Name"
        Private Const Name_ID As String = "ID"
        Private Const Name_Date As String = "Date"
        Private Const Name_PostsNode As String = "Posts"
#End Region
        Friend Const DefaultDownloadLimitCount As Integer = 1000
        Friend Property Name As String = String.Empty
        Friend Property ID As String = String.Empty
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
        Friend Sub New()
            Posts = New List(Of UserPost)
            PostsLatest = New List(Of UserPost)
            Range = New RangeSwitcher(Of UserPost)(Me)
        End Sub
        Friend Sub New(ByVal f As SFile)
            Me.New
            LoadData(f, False)
        End Sub
        Public Shared Widening Operator CType(ByVal f As SFile) As Channel
            Return New Channel(f)
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
        Friend Sub DownloadData(ByVal Token As Threading.CancellationToken, Optional ByVal SkipExists As Boolean = True,
                                Optional ByVal p As MyProgress = Nothing)
            Try
                _Downloading = True
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
                    Token.ThrowIfCancellationRequested()
                End Using
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
        Public Overloads Overrides Function Equals(ByVal Obj As Object) As Boolean
            If Not Obj Is Nothing Then
                If TypeOf Obj Is String Then
                    Return ID = CStr(Obj)
                Else
                    Return Equals(DirectCast(Obj, Channel))
                End If
            Else
                Return False
            End If
        End Function
#End Region
#Region "IComparable Support"
        Friend Function CompareTo(ByVal Other As Channel) As Integer Implements IComparable(Of Channel).CompareTo
            If Not Name.IsEmptyString And Not Other.Name.IsEmptyString Then
                Return Name.CompareTo(Other.Name)
            Else
                Return ID.CompareTo(Other.ID)
            End If
        End Function
#End Region
#Region "IXMLContainer Support"
        Friend Overloads Function LoadData(Optional ByVal f As SFile = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Boolean Implements ILoaderSaver.Load
            Return LoadData(File, False, e)
        End Function
        Friend Overloads Function LoadData(ByVal f As SFile, ByVal PartialLoad As Boolean, Optional ByVal e As ErrorsDescriber = Nothing) As Boolean
            If f.Exists Then
                Using x As New XmlFile(f, ProtectionLevels.All, False) With {.XmlReadOnly = True, .AllowSameNames = True}
                    x.LoadData()
                    x.DefaultsLoading(False)
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
                x.DefaultsLoading(False)
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
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Posts.Clear()
                    PostsLatest.Clear()
                    Range.Dispose()
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