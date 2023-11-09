' Copyright (C) 2023  Andy https://github.com/AAndyProgram
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
Imports System.Threading
Imports SCrawler.API.Base
Imports SCrawler.Plugin.Hosts
Imports SCrawler.API.Reddit.RedditViewExchange
Imports View = SCrawler.API.Reddit.IRedditView.View
Imports Period = SCrawler.API.Reddit.IRedditView.Period
Namespace API.Reddit
    Friend Class Channel : Implements ICollection(Of UserPost), IEquatable(Of Channel), IComparable(Of Channel),
            IRangeSwitcherContainer(Of UserPost), ILoaderSaver, IMyEnumerator(Of UserPost), IChannelLimits, IRedditView, IDisposable
#Region "XML Names"
        Private Const Name_Name As String = "Name"
        Private Const Name_ID As String = "ID"
        Private Const Name_Date As String = "Date"
        Private Const Name_PostsNode As String = "Posts"
        Private Const Name_UsersAdded As String = "UsersAdded"
        Private Const Name_UsersExistent As String = "UsersExistent"
        Private Const Name_PostsDownloaded As String = "PostsDownloaded"
#End Region
        Friend Const DefaultDownloadLimitCount As Integer = 1000
        Friend ReadOnly Property Site As String = RedditSite
        Friend Property Name As String
        Friend Property ID As String
        Friend ReadOnly Property CUser As UserInfo
            Get
                Return New UserInfo(Me)
            End Get
        End Property
        Friend ReadOnly Property PostsLatest As List(Of UserPost)
        Friend ReadOnly Property Posts As List(Of UserPost)
        Friend ReadOnly Property PostsNames As List(Of String)
        Friend ReadOnly Property PostsAll As List(Of UserPost)
            Get
                Return ListAddList(Nothing, Posts).ListAddList(PostsLatest).ListSort
            End Get
        End Property
        Private Property Source As IEnumerable(Of UserPost) Implements IRangeSwitcherContainer(Of UserPost).Source
            Get
                Return Posts
            End Get
            Set(ByVal s As IEnumerable(Of UserPost))
            End Set
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
        Private ReadOnly Property FilePosts As SFile
            Get
                Dim f As SFile = File
                f.Name &= "_Posts"
                f.Extension = "txt"
                Return f
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
        Friend Property ViewMode As View = View.New Implements IRedditView.ViewMode
        Friend Property ViewPeriod As Period = Period.All Implements IRedditView.ViewPeriod
        Friend Property RedGifsAccount As String = String.Empty Implements IRedditView.RedGifsAccount
        Friend Property RedditAccount As String = String.Empty Implements IRedditView.RedditAccount
        Friend Sub SetView(ByVal Options As IRedditView) Implements IRedditView.SetView
            If Not Options Is Nothing Then
                ViewMode = Options.ViewMode
                ViewPeriod = Options.ViewPeriod
                RedditAccount = Options.RedditAccount
                RedGifsAccount = Options.RedGifsAccount
            End If
        End Sub
#Region "Statistics support"
        Private ReadOnly CountOfAddedUsers As List(Of Integer)
        Private ReadOnly CountOfLoadedPostsPerSession As List(Of Integer)
        Friend ReadOnly Property ChannelExistentUserNames As List(Of String)
        Private _FirstUserAdded As Boolean = False
        Friend Sub UserAdded(ByVal UserName As String, Optional ByVal IsAdded As Boolean = True)
            If Not _FirstUserAdded Then CountOfAddedUsers.Add(0) : _FirstUserAdded = True
            Dim v% = CountOfAddedUsers.Last
            v += IIf(IsAdded, 1, -1)
            If v < 0 Then v = 0
            CountOfAddedUsers(CountOfAddedUsers.Count - 1) = v
            If Not ChannelExistentUserNames.Contains(UserName) Then ChannelExistentUserNames.Add(UserName)
        End Sub
        Friend Sub UpdateUsersStats()
            If Posts.Count > 0 Or PostsLatest.Count > 0 Then
                ChannelExistentUserNames.ListAddList((From p As UserPost In PostsAll
                                                      Where Not p.UserID.IsEmptyString AndAlso
                                                            Settings.UsersList.Exists(Function(u) u.Site = Site And u.Name = p.UserID)
                                                      Select p.UserID), LNC)
                ChannelExistentUserNames.RemoveAll(Function(u) Not Settings.UsersList.Exists(Function(uu) uu.Site = Site And uu.Name = u))
            End If
        End Sub
        Friend Function GetChannelStats(ByVal Extended As Boolean) As String
            UpdateUsersStats()
            Dim s$ = String.Empty
            Dim p As New ANumbers With {.FormatOptions = ANumbers.Options.GroupIntegral}
            If Extended Then
                s.StringAppendLine($"Users added from this channel: {CountOfAddedUsers.Sum.NumToString(p)}")
                s.StringAppendLine($"Users added from this channel (avg): {CountOfAddedUsers.DefaultIfEmpty(0).Average.RoundDown.NumToString(p)}")
                s.StringAppendLine($"Users added from this channel (session): {CountOfAddedUsers.LastOrDefault.NumToString(p)}")
                s.StringAppendLine($"Posts downloaded (avg): {CountOfLoadedPostsPerSession.DefaultIfEmpty(0).Average.RoundUp.NumToString(p)}")
                s.StringAppendLine($"Posts downloaded (session): {CountOfLoadedPostsPerSession.LastOrDefault.NumToString(p)}")
                s.StringAppendLine($"My users in this channel: {ChannelExistentUserNames.Count.NumToString(p)}")
            Else
                s.StringAppend($"Users: {CountOfAddedUsers.Sum.NumToString(p)} (avg: {CountOfAddedUsers.DefaultIfEmpty(0).Average.RoundDown.NumToString(p)}; s: {CountOfAddedUsers.LastOrDefault.NumToString(p)})")
                s.StringAppend($"Posts: {CountOfLoadedPostsPerSession.DefaultIfEmpty(0).Average.RoundUp.NumToString(p)} (s: {CountOfLoadedPostsPerSession.LastOrDefault.NumToString(p)})", "; ")
                s.StringAppend($"My users: {ChannelExistentUserNames.Count.NumToString(p)}", "; ")
            End If
            Return s
        End Function
#End Region
#Region "Limits Support"
        Private _DownloadLimitCount As Integer? = Nothing
        Friend Property DownloadLimitCount As Integer? Implements IChannelLimits.DownloadLimitCount
            Get
                If Not ViewMode = View.New And AutoGetLimits Then
                    Return _DownloadLimitCount
                Else
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
                End If
            End Get
            Set(ByVal NewLimit As Integer?)
                _DownloadLimitCount = NewLimit
            End Set
        End Property
        Private _DownloadLimitPost As String = String.Empty
        Friend Property DownloadLimitPost As String Implements IChannelLimits.DownloadLimitPost
            Get
                If Not ViewMode = View.New And AutoGetLimits Then
                    Return _DownloadLimitPost
                Else
                    Dim PID$ = ListAddList(Nothing, Posts, LNC).ListAddList(PostsLatest, LNC).ListSort.FirstOrDefault.ID
                    If AutoGetLimits And Not PID.IsEmptyString Then
                        Return PID
                    Else
                        Return _DownloadLimitPost
                    End If
                End If
            End Get
            Set(ByVal NewLimit As String)
                _DownloadLimitPost = NewLimit
            End Set
        End Property
        Private _DownloadLimitDate As Date? = Nothing
        Friend Property DownloadLimitDate As Date? Implements IChannelLimits.DownloadLimitDate
            Get
                If Not ViewMode = View.New And AutoGetLimits Then
                    Return _DownloadLimitDate
                Else
                    If AutoGetLimits And LatestParsedDate.HasValue Then
                        Return LatestParsedDate
                    Else
                        Return _DownloadLimitDate
                    End If
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
            If Not ViewMode = View.New And AutoGetLimits Then
                DownloadLimitDate = Nothing
                DownloadLimitCount = Nothing
                DownloadLimitPost = String.Empty
            End If
        End Sub
        Friend Property AutoGetLimits As Boolean = True Implements IChannelLimits.AutoGetLimits
#End Region
        Private _HOST As SettingsHost
        Friend ReadOnly Property HOST As SettingsHost
            Get
                _HOST = Settings(RedditSiteKey, RedditAccount)
                If _HOST Is Nothing Then
                    MyMainLOG = $"Reddit account '{RedditAccount}' for channel '{Name}' not found in the accounts. The default account will be used."
                    _HOST = Settings(RedditSiteKey).Default
                End If
                Return _HOST
            End Get
        End Property
        Friend Sub New()
            Posts = New List(Of UserPost)
            PostsLatest = New List(Of UserPost)
            PostsNames = New List(Of String)
            CountOfAddedUsers = New List(Of Integer)
            CountOfLoadedPostsPerSession = New List(Of Integer)
            ChannelExistentUserNames = New List(Of String)
        End Sub
        Friend Sub New(ByVal f As SFile)
            Me.New
            LoadData(f, False)
        End Sub
        Public Shared Widening Operator CType(ByVal f As SFile) As Channel
            Return New Channel(f)
        End Operator
        Public Overrides Function ToString() As String
            Return If(Name.IsEmptyString, ID, Name)
        End Function
        Friend Sub Delete()
            Dim f As SFile = ChannelsCollection.ChannelsDeletedPath
            With File
                f.Name = .Name
                f.Extension = .Extension
                .Copy(f,, True, SFODelete.DeleteToRecycleBin)
                .Delete(, SFODelete.DeleteToRecycleBin)
            End With
            With FilePosts
                f.Name = .Name
                f.Extension = .Extension
                .Copy(f,, True, SFODelete.DeleteToRecycleBin)
                .Delete(, SFODelete.DeleteToRecycleBin)
            End With
        End Sub
        Friend Sub DownloadData(ByVal Token As CancellationToken, Optional ByVal SkipExists As Boolean = True,
                                Optional ByVal p As MyProgress = Nothing)
            Try
                _Downloading = True
                Using d As New UserData With {
                    .Progress = p,
                    .SaveToCache = True,
                    .SkipExistsUsers = SkipExists,
                    .ChannelInfo = Me,
                    .IsChannel = True
                }
                    With d
                        .SetEnvironment(HOST, CUser, False)
                        .RemoveUpdateHandlers()
                        .SetLimit(Me)
                        .SetView(Me)
                        .DownloadData(Token)
                    End With
                    Dim b% = Posts.Count
                    Posts.ListAddList(d.GetNewChannelPosts(), LNC)
                    If Posts.Count - b > 0 Then _Saved = False : CountOfLoadedPostsPerSession.Add(Posts.Count - b)
                    Posts.Sort()
                    LatestParsedDate = If(Posts.FirstOrDefault(Function(pp) pp.Date.HasValue).Date, LatestParsedDate)
                    UpdateUsersStats()
                End Using
            Catch oex As OperationCanceledException When Token.IsCancellationRequested
            Finally
                SaveUnsaved()
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
        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
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
                ElseIf TypeOf Obj Is Channel Then
                    Return Equals(DirectCast(Obj, Channel))
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
                        Dim lc As New ListAddParams(LAP.ClearBeforeAdd)
                        Name = x.Value(Name_Name)
                        ID = x.Value(Name_ID)
                        ViewMode = x.Value(Name_ViewMode).FromXML(Of Integer)(CInt(View.[New]))
                        ViewPeriod = x.Value(Name_ViewPeriod).FromXML(Of Integer)(CInt(Period.All))
                        RedGifsAccount = x.Value(Name_RedGifsAccount)
                        RedditAccount = x.Value(Name_RedditAccount)
                        If FilePosts.Exists Then PostsNames.ListAddList(FilePosts.GetText.StringToList(Of String)("|"), LNC)
                        LatestParsedDate = AConvert(Of Date)(x.Value(Name_Date), DateTimeDefaultProvider, Nothing)
                        CountOfAddedUsers.ListAddList(x.Value(Name_UsersAdded).StringToList(Of Integer)("|"), lc)
                        CountOfLoadedPostsPerSession.ListAddList(x.Value(Name_PostsDownloaded).StringToList(Of Integer)("|"), lc)
                        ChannelExistentUserNames.ListAddList(x.Value(Name_UsersExistent).StringToList(Of String)("|"), LNC)
                        If Not PartialLoad Then
                            With x(Name_PostsNode).XmlIfNothing
                                If .Count > 0 Then .ForEach(Sub(ee) PostsLatest.Add(New UserPost With {
                                                                        .ID = ee.Attribute(Name_ID),
                                                                        .[Date] = AConvert(Of Date)(ee.Attribute(Name_Date).Value, DateTimeDefaultProvider, Nothing)}))
                            End With
                        End If
                    End If
                End Using
            End If
            Return True
        End Function
        Private _Saved As Boolean = True
        Friend Function SaveUnsaved() As Boolean
            Return _Saved OrElse Save()
        End Function
        Friend Overloads Function Save(Optional ByVal f As SFile = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Boolean Implements ILoaderSaver.Save
            Try
                UpdateUsersStats()
                If Not ViewMode = View.New Then
                    Dim l As New List(Of String)
                    If Posts.Count > 0 Or PostsLatest.Count > 0 Then l.ListAddList((From p In PostsAll Where Not p.ID.IsEmptyString Select p.ID), LNC)
                    l.ListAddList(PostsNames, LNC)
                    If l.Count > 0 Then TextSaver.SaveTextToFile(l.ListToString("|"), FilePosts, True,, EDP.SendToLog)
                End If
                Using x As New XmlFile With {.AllowSameNames = True, .Name = "Channel"}
                    x.Add(Name_Name, Name)
                    x.Add(Name_ID, ID)
                    x.Add(Name_ViewMode, CInt(ViewMode))
                    x.Add(Name_ViewPeriod, CInt(ViewPeriod))
                    x.Add(Name_UsersAdded, CountOfAddedUsers.ListToString("|"))
                    x.Add(Name_PostsDownloaded, CountOfLoadedPostsPerSession.ListToString("|"))
                    x.Add(Name_UsersExistent, ChannelExistentUserNames.ListToString("|"))
                    x.Add(Name_RedGifsAccount, RedGifsAccount)
                    x.Add(Name_RedditAccount, RedditAccount)
                    If Posts.Count > 0 Or PostsLatest.Count > 0 Then
                        Dim tmpPostList As List(Of UserPost) = Nothing
                        tmpPostList.ListAddList(Posts).ListAddList(PostsLatest)
                        tmpPostList.Sort()
                        LatestParsedDate = tmpPostList.FirstOrDefault(Function(pd) pd.Date.HasValue).Date
                        x.Add(Name_Date, AConvert(Of String)(LatestParsedDate, DateTimeDefaultProvider, String.Empty))
                        x.Add(Name_PostsNode, String.Empty)
                        With x(Name_PostsNode)
                            tmpPostList.Take(200).ToList.ForEach(Sub(p) .Add(New EContainer("Post",
                                                                                      String.Empty,
                                                                                      {
                                                                                        New EAttribute(Name_ID, p.ID),
                                                                                        New EAttribute(Name_Date, AConvert(Of String)(p.Date, DateTimeDefaultProvider, String.Empty))
                                                                                      })
                                                                      )
                                                          )
                        End With
                        tmpPostList.Clear()
                    End If
                    _Saved = x.Save(File)
                End Using
                Return True
            Catch ex As Exception
                If Not e.Exists Then e = EDP.ReturnValue
                Return ErrorsDescriber.Execute(e, ex, "API.Reddit.Channel.Save", _Saved)
            End Try
        End Function
#End Region
#Region "IDisposable Support"
        Private disposedValue As Boolean = False
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Posts.Clear()
                    PostsLatest.Clear()
                    PostsNames.Clear()
                    CountOfAddedUsers.Clear()
                    CountOfLoadedPostsPerSession.Clear()
                    ChannelExistentUserNames.Clear()
                    CachePath.Delete(SFO.Path, SFODelete.None, EDP.SendToLog)
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