' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Functions.XML
Namespace DownloadObjects.Groups
    Friend Class DownloadGroupCollection : Implements IEnumerable(Of DownloadGroup), IMyEnumerator(Of DownloadGroup)
#Region "Events"
        Friend Event Deleted As DownloadGroup.GroupEventHandler
        Friend Event Added As DownloadGroup.GroupEventHandler
        Friend Event Updated As DownloadGroup.GroupEventHandler
#End Region
#Region "Declarations"
        Private ReadOnly GroupsList As List(Of DownloadGroup)
        Private ReadOnly GroupFile As SFile = "Settings\Groups.xml"
#End Region
#Region "Initializer"
        Friend Sub New()
            GroupsList = New List(Of DownloadGroup)
            If GroupFile.Exists Then
                Using x As New XmlFile(GroupFile,, False) With {.XmlReadOnly = True, .AllowSameNames = True}
                    x.LoadData()
                    If x.Count > 0 Then GroupsList.ListAddList(x, LAP.IgnoreICopier)
                End Using
                With GroupsList
                    If .Count > 0 Then
                        .ForEach(Sub(ByVal g As DownloadGroup)
                                     AddHandler g.Deleted, AddressOf OnGroupDeleted
                                     AddHandler g.Updated, AddressOf OnGroupUpdated
                                 End Sub)
                        If .Exists(Function(g) g.NeedToSave) Then Update()
                    End If
                End With
            End If
            Reindex()
        End Sub
#End Region
#Region "Base properties"
        Default Friend ReadOnly Property Item(ByVal Index As Integer) As DownloadGroup Implements IMyEnumerator(Of DownloadGroup).MyEnumeratorObject
            Get
                Return GroupsList(Index)
            End Get
        End Property
        Friend ReadOnly Property Count As Integer Implements IMyEnumerator(Of DownloadGroup).MyEnumeratorCount
            Get
                Return GroupsList.Count
            End Get
        End Property
#End Region
#Region "Update, BeginUpdate, EndUpdate"
        Friend Sub Update()
            If Not _UpdateMode Then
                If Count > 0 Then
                    Using x As New XmlFile With {.Name = "Groups", .AllowSameNames = True} : x.AddRange(GroupsList) : x.Save(GroupFile) : End Using
                Else
                    GroupFile.Delete()
                End If
            End If
        End Sub
        Private _UpdateMode As Boolean = False
        Friend Sub BeginUpdate()
            _UpdateMode = True
        End Sub
        Friend Sub EndUpdate()
            _UpdateMode = False
        End Sub
#End Region
#Region "Sort, Reindex"
        Friend Sub Sort()
            GroupsList.Sort()
        End Sub
        Friend Sub Reindex()
            Dim initUpValue As Boolean = _UpdateMode
            BeginUpdate()
            GroupsList.ListReindex
            If Not initUpValue Then EndUpdate()
        End Sub
#End Region
#Region "Group handlers"
        Private _GroupAddInProgress As Boolean = False
        Private Sub OnGroupUpdated(ByVal Sender As DownloadGroup)
            If Not _GroupAddInProgress Then
                Update()
                If Not Sender.IsViewFilter Then RaiseEvent Updated(Sender)
            End If
        End Sub
        Private Sub OnGroupDeleted(ByVal Sender As DownloadGroup)
            If Not Sender.IsViewFilter Then RaiseEvent Deleted(Sender)
            Dim i% = GroupsList.FindIndex(Function(g) g.Key = Sender.Key)
            If i >= 0 Then
                GroupsList(i).Dispose()
                GroupsList.RemoveAt(i)
                Reindex()
                Update()
            End If
        End Sub
#End Region
#Region "Add"
        Friend Sub CloneAndAdd(ByVal Group As DownloadGroup)
            Using f As New GroupEditorForm(Group.Copy) With {.IsClone = True}
                f.ShowDialog()
                If f.DialogResult = DialogResult.OK Then Add(f.MyGroup)
            End Using
        End Sub
        Friend Overloads Function Add() As Integer
            Using f As New GroupEditorForm(Nothing)
                f.ShowDialog()
                If f.DialogResult = DialogResult.OK Then Add(f.MyGroup) : Return IndexOf(f.MyGroup.Name)
            End Using
            Return -1
        End Function
        Friend Overloads Sub Add(ByVal Item As DownloadGroup, Optional ByVal IsFilter As Boolean = False, Optional ByVal ReplaceExisting As Boolean = False)
            Dim i% = IndexOf(Item.Name, IsFilter)
            Dim exists As Boolean = i >= 0
            _GroupAddInProgress = True

            If exists Then
                If ReplaceExisting Then
                    GroupsList(i).Dispose()
                    GroupsList(i) = Item
                Else
                    i = -1
                End If
            Else
                GroupsList.Add(Item)
                i = Count - 1
            End If

            If i >= 0 Then
                With GroupsList(i)
                    AddHandler .Deleted, AddressOf OnGroupDeleted
                    AddHandler .Updated, AddressOf OnGroupUpdated
                    If Not exists Then
                        Reindex()
                        GroupsList.Sort()
                        Reindex()
                        If Not Item.IsViewFilter And Not _UpdateMode Then RaiseEvent Added(.Self)
                    Else
                        If Not Item.IsViewFilter And Not _UpdateMode Then RaiseEvent Updated(.Self)
                    End If
                End With
                Update()
            End If
            _GroupAddInProgress = False
        End Sub
#End Region
#Region "DownloadGroupIfExists"
        Friend Function DownloadGroupIfExists(ByVal Index As Integer) As Boolean
            If Index.ValueBetween(0, Count - 1) Then Item(Index).ProcessDownloadUsers() : Return True Else Return False
        End Function
#End Region
#Region "IndexOf"
        Friend Function IndexOf(ByVal Name As String, Optional ByVal IsFilter As Boolean = False) As Integer
            If Count > 0 Then
                Return GroupsList.FindIndex(Function(g) g.Name = Name And g.IsViewFilter = IsFilter)
            Else
                Return -1
            End If
        End Function
#End Region
#Region "IEnumerable Support"
        Private Function GetEnumerator() As IEnumerator(Of DownloadGroup) Implements IEnumerable(Of DownloadGroup).GetEnumerator
            Return New MyEnumerator(Of DownloadGroup)(Me)
        End Function
        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
#End Region
    End Class
End Namespace