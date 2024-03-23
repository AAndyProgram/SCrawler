' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Friend Structure ViewFilter : Implements IEContainerProvider, IComparable(Of ViewFilter)
#Region "Names"
    Private Const Name_Name As String = "Name"
    Private Const Name_ViewMode As String = "ViewMode"
    Private Const Name_GroupUsers As String = "GroupUsers"
    Private Const Name_ShowUsers As String = "ShowUsers"
    Private Const Name_ShowSubscriptions As String = "ShowSubscriptions"
    Private Const Name_Sites As String = "Sites"
    Private Const Name_ShowingMode As String = "ShowingMode"
    Private Const Name_Labels As String = "Labels"
    Private Const Name_ExcludedLabels As String = "ExcludedLabels"
    Private Const Name_IgnoreExcludedLabels As String = "IgnoreExcludedLabels"
    Private Const Name_ShowGroupsInsteadLabels As String = "ShowGroupsInsteadLabels"
    Private Const Name_FilterADV As String = "FilterADV"
    Private Const Name_DateFrom As String = "DateFrom"
    Private Const Name_DateTo As String = "DateTo"
    Private Const Name_DateMode As String = "DateMode"
#End Region
#Region "Declarations"
    Friend Name As String

    Friend ViewMode As ViewModes

    Friend GroupUsers As Boolean

    Friend ShowUsers As Boolean
    Friend ShowSubscriptions As Boolean

    Friend Sites As IEnumerable(Of String)

    Friend ShowingMode As ShowingModes

    Friend Labels As IEnumerable(Of String)

    Friend ExcludedLabels As IEnumerable(Of String)
    Friend IgnoreExcludedLabels As Boolean
    Friend ShowGroupsInsteadLabels As Boolean

    Friend FilterADV As DownloadObjects.Groups.DownloadGroup

    Friend DateFrom As Date?
    Friend DateTo As Date?
    Friend DateMode As ShowingDates
    Friend ReadOnly Property LimitDates As Boolean
        Get
            Return (DateFrom.HasValue Or DateTo.HasValue) And Not DateMode = ShowingDates.Off
        End Get
    End Property
    Friend Function GetAllLabels() As IEnumerable(Of String)
        Return ListAddList(Nothing, Labels).ListAddList(ExcludedLabels).ListAddList({FilterADV}.GetGroupsLabels).ListIfNothing
    End Function
#End Region
#Region "Initializers"
    Friend Sub New(ByVal e As EContainer)
        With e
            Name = .Value(Name_Name)
            ViewMode = .Value(Name_ViewMode).FromXML(Of Integer)(ViewModes.IconLarge)
            GroupUsers = .Value(Name_GroupUsers).FromXML(Of Boolean)(True)
            ShowUsers = .Value(Name_ShowUsers).FromXML(Of Boolean)(True)
            ShowSubscriptions = .Value(Name_ShowSubscriptions).FromXML(Of Boolean)(True)
            Sites = .Value(Name_Sites).StringToList(Of String)("|")
            ShowingMode = .Value(Name_ShowingMode).FromXML(Of Integer)(ShowingModes.All)
            Labels = .Value(Name_Labels).StringToList(Of String)("|")
            ExcludedLabels = .Value(Name_ExcludedLabels).StringToList(Of String)("|")
            IgnoreExcludedLabels = .Value(Name_IgnoreExcludedLabels).FromXML(Of Boolean)(False)
            ShowGroupsInsteadLabels = .Value(Name_ShowGroupsInsteadLabels).FromXML(Of Boolean)(True)
            With .Item(Name_FilterADV)
                If If(?.Count, 0) > 0 Then
                    FilterADV = New DownloadObjects.Groups.DownloadGroup(.Item(0))
                Else
                    FilterADV = New DownloadObjects.Groups.DownloadGroup
                End If
            End With
            DateFrom = AConvert(Of Date)(.Value(Name_DateFrom), DateTimeDefaultProvider, Nothing)
            DateTo = AConvert(Of Date)(.Value(Name_DateTo), DateTimeDefaultProvider, Nothing)
            DateMode = .Value(Name_DateMode).FromXML(Of Integer)(ShowingDates.Off)
        End With
    End Sub
    Friend Shared Function FromCurrent(Optional ByVal Name As String = "") As ViewFilter
        Dim f As New ViewFilter
        With Settings
            f.Name = Name

            f.ViewMode = .ViewMode

            f.GroupUsers = .GroupUsers

            f.ShowUsers = .MainFrameUsersShowDefaults
            f.ShowSubscriptions = .MainFrameUsersShowSubscriptions

            If .SelectedSites.Count > 0 Then f.Sites = .SelectedSites

            f.ShowingMode = .ShowingMode

            If .Labels.Current.Count > 0 Then f.Labels = .Labels.Current

            If .Labels.Excluded.Count > 0 Then f.ExcludedLabels = .Labels.Excluded
            f.IgnoreExcludedLabels = .Labels.ExcludedIgnore
            f.ShowGroupsInsteadLabels = .ShowGroupsInsteadLabels

            f.FilterADV = .AdvancedFilter.Copy

            f.DateFrom = .ViewDateFrom
            f.DateTo = .ViewDateTo
            f.DateMode = .ViewDateMode
        End With
        Return f
    End Function
#End Region
    Friend Sub Populate()
        With Settings
            .BeginUpdate()

            .ViewMode.Value = ViewMode

            .GroupUsers.Value = GroupUsers

            .MainFrameUsersShowDefaults.Value = ShowUsers
            .MainFrameUsersShowSubscriptions.Value = ShowSubscriptions

            With .SelectedSites
                .Clear()
                If Sites.ListExists Then .AddRange(Sites)
                .Update()
            End With

            .ShowingMode.Value = ShowingMode
            With .Labels
                With .Current
                    .Clear()
                    If Labels.ListExists Then .AddRange(Labels)
                    .Update()
                End With

                With .Excluded
                    .Clear()
                    If ExcludedLabels.ListExists Then .AddRange(ExcludedLabels)
                    .Update()
                End With

                .ExcludedIgnore.Value = IgnoreExcludedLabels
            End With
            .ShowGroupsInsteadLabels.Value = ShowGroupsInsteadLabels

            .AdvancedFilter.Copy(FilterADV)
            .AdvancedFilter.UpdateFile()

            .ViewDateFrom = DateFrom
            .ViewDateTo = DateTo
            .ViewDateMode.Value = DateMode

            .EndUpdate()
        End With
    End Sub
    Public Overrides Function ToString() As String
        Return Name
    End Function
    Public Overrides Function Equals(ByVal Obj As Object) As Boolean
        Return Name.StringToLower = DirectCast(Obj, ViewFilter).Name
    End Function
    Private Function CompareTo(ByVal Other As ViewFilter) As Integer Implements IComparable(Of ViewFilter).CompareTo
        Return Name.CompareTo(Other.Name)
    End Function
    Friend Function ToEContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
        Return New EContainer("Filter") From {
            New EContainer(Name_Name, Name),
            New EContainer(Name_ViewMode, CInt(ViewMode)),
            New EContainer(Name_GroupUsers, GroupUsers.BoolToInteger),
            New EContainer(Name_ShowUsers, ShowUsers.BoolToInteger),
            New EContainer(Name_ShowSubscriptions, ShowSubscriptions.BoolToInteger),
            New EContainer(Name_Sites, Sites.ListToString("|")),
            New EContainer(Name_ShowingMode, CInt(ShowingMode)),
            New EContainer(Name_Labels, Labels.ListToString("|")),
            New EContainer(Name_ExcludedLabels, ExcludedLabels.ListToString("|")),
            New EContainer(Name_IgnoreExcludedLabels, IgnoreExcludedLabels.BoolToInteger),
            New EContainer(Name_ShowGroupsInsteadLabels, ShowGroupsInsteadLabels.BoolToInteger),
            New EContainer(Name_FilterADV) From {FilterADV.ToEContainer},
            New EContainer(Name_DateFrom, AConvert(Of String)(DateFrom, DateTimeDefaultProvider, String.Empty)),
            New EContainer(Name_DateTo, AConvert(Of String)(DateTo, DateTimeDefaultProvider, String.Empty)),
            New EContainer(Name_DateMode, CInt(DateMode))
        }
    End Function
End Structure
Friend Class ViewFilterCollection : Implements IEnumerable(Of ViewFilter), IMyEnumerator(Of ViewFilter)
    Private ReadOnly Filters As List(Of ViewFilter)
    Private ReadOnly File As SFile = $"{SettingsFolderName}\SavedFilters.xml"
    Friend Sub New()
        Filters = New List(Of ViewFilter)
        If File.Exists Then
            Using x As New XmlFile(File, Protector.Modes.All, False) With {.AllowSameNames = True}
                x.LoadData()
                If x.Count > 0 Then Filters.ListAddList(x, LAP.IgnoreICopier)
            End Using
        End If
        If Filters.Count > 0 Then Filters.Sort()
    End Sub
    Default Friend ReadOnly Property Item(ByVal Index As Integer) As ViewFilter Implements IMyEnumerator(Of ViewFilter).MyEnumeratorObject
        Get
            Return Filters(Index)
        End Get
    End Property
    Friend ReadOnly Property Count As Integer Implements IMyEnumerator(Of ViewFilter).MyEnumeratorCount
        Get
            Return Filters.Count
        End Get
    End Property
    Friend Function GetAllLabels() As IEnumerable(Of String)
        If Count = 0 Then
            Return New String() {}
        Else
            Return ListAddList(Nothing, Filters.SelectMany(Function(f) f.GetAllLabels), LAP.NotContainsOnly)
        End If
    End Function
    Friend Sub Update()
        If Count > 0 Then
            Filters.Sort()
            Using x As New XmlFile With {.AllowSameNames = True}
                x.AddRange(Filters)
                x.Name = "Filters"
                x.Save(File, EDP.LogMessageValue)
            End Using
        Else
            If File.Exists Then File.Delete()
        End If
    End Sub
    Friend Sub Add(ByVal Item As ViewFilter, Optional ByVal AutoUpdate As Boolean = True)
        Dim i% = IndexOf(Item)
        If i >= 0 Then
            Filters(i) = Item
        Else
            Filters.Add(Item)
        End If
        Filters.Sort()
        If AutoUpdate Then Update()
    End Sub
    Friend Overloads Function IndexOf(ByVal Item As ViewFilter) As Integer
        If Count > 0 Then
            Return Filters.IndexOf(Item)
        Else
            Return -1
        End If
    End Function
    Friend Overloads Function IndexOf(ByVal Name As String) As Integer
        Return IndexOf(New ViewFilter With {.Name = Name})
    End Function
    Private Function GetEnumerator() As IEnumerator(Of ViewFilter) Implements IEnumerable(Of ViewFilter).GetEnumerator
        Return New MyEnumerator(Of ViewFilter)(Me)
    End Function
    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return GetEnumerator()
    End Function
End Class