' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.YouTube.Base
Imports SCrawler.API.YouTube.Objects
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Namespace API.YouTube.Controls
    Friend Class YTDataFilter
        Private Const Name_Types As String = "Types"
        Private Const Name_IsMusic As String = "IsMusic"
        Private Const Name_IsVideo As String = "IsVideo"
        Private Const Name_UserList As String = "UserList"
        Friend Structure SimpleUser : Implements IEContainerProvider, IComparable(Of SimpleUser)
            Private Const Name_UID As String = "UID"
            Friend Title As String
            Friend ID As String
            Friend Sub New(ByVal _Title As String, ByVal _ID As String)
                Title = _Title
                ID = _ID
            End Sub
            Private Sub New(ByVal u As YouTubeMediaContainerBase)
                Title = u.Title
                ID = u.ID
            End Sub
            Public Shared Widening Operator CType(ByVal u As YouTubeMediaContainerBase) As SimpleUser
                Return New SimpleUser(u.UserTitle, u.UserID)
            End Operator
            Public Shared Widening Operator CType(ByVal e As EContainer) As SimpleUser
                Return New SimpleUser(e.Value, e.Attribute(Name_UID).Value)
            End Operator
            Public Overrides Function ToString() As String
                Return String.Format(CStr(Interaction.Switch(Title.IsEmptyString, "{1}", ID.IsEmptyString, "{0}", True, "{0} ({1})")), Title, ID)
            End Function
            Public Overrides Function Equals(ByVal Obj As Object) As Boolean
                Return Not IsDBNull(Obj) AndAlso ToString() = DirectCast(Obj, SimpleUser).ToString
            End Function
            Private Function CompareTo(ByVal Other As SimpleUser) As Integer Implements IComparable(Of SimpleUser).CompareTo
                Return ToString.CompareTo(Other.ToString)
            End Function
            Private Function ToEContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
                Return New EContainer("User", Title, {New EAttribute(Name_UID, ID)})
            End Function
        End Structure
        Friend ReadOnly Property Types As List(Of YouTubeMediaType)
        Friend Property IsMusic As Boolean = True
        Friend Property IsVideo As Boolean = True
        Friend ReadOnly Property UserList As List(Of SimpleUser)
        Private ReadOnly File As New SFile("Settings\YouTubeFilter.xml")
        Friend Sub New(Optional ByVal LoadFromFile As Boolean = True)
            Types = New List(Of YouTubeMediaType) From {YouTubeMediaType.Undefined}
            UserList = New List(Of SimpleUser)
            If LoadFromFile AndAlso File.Exists Then
                Using e As New XmlFile(File, Protector.Modes.All, False) With {.AllowSameNames = True}
                    e.LoadData()
                    If e.Count > 0 Then
                        Types.Clear()
                        Types.ListAddList(e.Value(Name_Types).StringToList(Of Integer)(","), LAP.NotContainsOnly)
                        IsMusic = e.Value(Name_IsMusic).FromXML(Of Boolean)(True)
                        IsVideo = e.Value(Name_IsVideo).FromXML(Of Boolean)(True)
                        UserList.ListAddList(e(Name_UserList), LAP.IgnoreICopier)
                    End If
                End Using
            End If
        End Sub
        Friend Sub New(ByVal f As YTDataFilter)
            Me.New(False)
            With f
                Types.ListAddList(.Types, LAP.NotContainsOnly)
                IsMusic = .IsMusic
                IsVideo = .IsVideo
                UserList.ListAddList(.UserList)
            End With
        End Sub
        Friend Sub Reset(Optional ByVal AddDefType As Boolean = True, Optional ByVal ClearUserList As Boolean = True)
            Types.Clear()
            If AddDefType Then Types.Add(YouTubeMediaType.Undefined)
            IsMusic = True
            IsVideo = True
            If ClearUserList Then UserList.Clear()
        End Sub
        Friend Sub Update()
            Using x As New XmlFile With {.AllowSameNames = True}
                With x
                    .Add(Name_Types, Types.ListToStringE(","))
                    .Add(Name_IsMusic, IsMusic.BoolToInteger)
                    .Add(Name_IsVideo, IsVideo.BoolToInteger)
                    .Add(Name_UserList, String.Empty)
                    .Self()(Name_UserList).AddRange(UserList)
                    .Name = "FILTER"
                    .Save(File)
                End With
            End Using
        End Sub
        Friend Function Ready(ByVal Item As YouTubeMediaContainerBase, Optional ByVal IgnoreUserList As Boolean = False) As Boolean
            With Item
                If Not IsMusic Or Not IsVideo Then
                    If .IsMusic And Not IsMusic Then Return False
                    If Not .IsMusic And Not IsVideo Then Return False
                End If
                If Not Types.Contains(YouTubeMediaType.Undefined) AndAlso Not Types.Contains(.ObjectType) Then Return False
                If Not IgnoreUserList AndAlso UserList.Count > 0 AndAlso Not UserList.Contains(Item) Then Return False
            End With
            Return True
        End Function
        Friend Overloads Sub RemoveAll(ByRef Data As List(Of IYouTubeMediaContainer))
            Data.RemoveAll(Function(item) Not Ready(item))
        End Sub
        Friend Overloads Sub RemoveAll(ByRef Data As List(Of YouTubeMediaContainerBase))
            Data.RemoveAll(Function(item) Not Ready(item))
        End Sub
        Friend Sub Populate(ByVal InitList As List(Of IYouTubeMediaContainer), ByVal DestList As List(Of IYouTubeMediaContainer), ByVal IgnoreUserList As Boolean)
            DestList.Clear()
            If InitList.Count > 0 Then InitList.ForEach(Sub(item) If Ready(item, IgnoreUserList) Then DestList.ListAddValue(item))
        End Sub
    End Class
End Namespace