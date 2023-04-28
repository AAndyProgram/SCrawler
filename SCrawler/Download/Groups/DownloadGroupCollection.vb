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
        Friend Event Deleted As DownloadGroup.GroupEventHandler
        Friend Event Added As DownloadGroup.GroupEventHandler
        Friend Event Updated As DownloadGroup.GroupEventHandler
        Private ReadOnly GroupsList As List(Of DownloadGroup)
        Private ReadOnly GroupFile As SFile = "Settings\Groups.xml"
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
            GroupsList.ListReindex
        End Sub
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
        Friend Sub Update()
            If Count > 0 Then
                Using x As New XmlFile With {.Name = "Groups", .AllowSameNames = True} : x.AddRange(GroupsList) : x.Save(GroupFile) : End Using
            Else
                GroupFile.Delete()
            End If
        End Sub
        Private _GroupAddInProgress As Boolean = False
        Private Sub OnGroupUpdated(ByVal Sender As DownloadGroup)
            If Not _GroupAddInProgress Then Update() : RaiseEvent Updated(Sender)
        End Sub
        Private Sub OnGroupDeleted(ByVal Sender As DownloadGroup)
            RaiseEvent Deleted(Sender)
            Dim i% = GroupsList.FindIndex(Function(g) g.Key = Sender.Key)
            If i >= 0 Then
                GroupsList(i).Dispose()
                GroupsList.RemoveAt(i)
                GroupsList.ListReindex
                Update()
            End If
        End Sub
        Friend Sub Add()
            Using f As New GroupEditorForm(Nothing)
                f.ShowDialog()
                If f.DialogResult = DialogResult.OK Then
                    _GroupAddInProgress = True
                    GroupsList.Add(f.MyGroup)
                    With GroupsList.Last
                        AddHandler .Deleted, AddressOf OnGroupDeleted
                        AddHandler .Updated, AddressOf OnGroupUpdated
                    End With
                    GroupsList.ListReindex
                    RaiseEvent Added(GroupsList.Last)
                    Update()
                    _GroupAddInProgress = False
                End If
            End Using
        End Sub
        Friend Function DownloadGroupIfExists(ByVal Index As Integer) As Boolean
            If Index.ValueBetween(0, Count - 1) Then Item(Index).DownloadUsers(True) : Return True Else Return False
        End Function
        Friend Function IndexOf(ByVal Name As String) As Integer
            If Count > 0 Then
                Return GroupsList.FindIndex(Function(g) g.Name = Name)
            Else
                Return -1
            End If
        End Function
        Private Function GetEnumerator() As IEnumerator(Of DownloadGroup) Implements IEnumerable(Of DownloadGroup).GetEnumerator
            Return New MyEnumerator(Of DownloadGroup)(Me)
        End Function
        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
    End Class
End Namespace