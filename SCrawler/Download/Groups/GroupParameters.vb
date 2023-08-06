' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML
Namespace DownloadObjects.Groups
    Friend Interface IGroup
        Property Name As String
        ReadOnly Property Labels As List(Of String)
        ReadOnly Property LabelsExcluded As List(Of String)
        ReadOnly Property Sites As List(Of String)
        ReadOnly Property SitesExcluded As List(Of String)
        Property Temporary As CheckState
        Property Favorite As CheckState
        Property ReadyForDownload As Boolean
        Property ReadyForDownloadIgnore As Boolean
        Property Subscriptions As Boolean
        Property SubscriptionsOnly As Boolean
        Property UsersCount As Integer
    End Interface
    Friend Class GroupParameters : Implements IGroup, IDisposable
        Protected Const Name_Name As String = "Name"
        Protected Const Name_Temporary As String = "Temporary"
        Protected Const Name_Favorite As String = "Favorite"
        Protected Const Name_ReadyForDownload As String = "RFD"
        Protected Const Name_ReadyForDownloadIgnore As String = "RFDI"
        Protected Const Name_Subscriptions As String = "Subscriptions"
        Protected Const Name_SubscriptionsOnly As String = "SubscriptionsOnly"
        Protected Const Name_UsersCount As String = "UsersCount"
        Protected Const Name_Labels As String = "Labels"
        Protected Const Name_Labels_Excluded As String = "LabelsExcluded"
        Protected Const Name_Sites As String = "Sites"
        Protected Const Name_Sites_Excluded As String = "SitesExcluded"
        Friend Property Name As String Implements IGroup.Name
        Friend ReadOnly Property Labels As List(Of String) Implements IGroup.Labels
        Friend ReadOnly Property LabelsExcluded As List(Of String) Implements IGroup.LabelsExcluded
        Friend ReadOnly Property Sites As List(Of String) Implements IGroup.Sites
        Friend ReadOnly Property SitesExcluded As List(Of String) Implements IGroup.SitesExcluded
        Friend Property Temporary As CheckState = CheckState.Indeterminate Implements IGroup.Temporary
        Friend Property Favorite As CheckState = CheckState.Indeterminate Implements IGroup.Favorite
        Friend Property ReadyForDownload As Boolean = True Implements IGroup.ReadyForDownload
        Friend Property ReadyForDownloadIgnore As Boolean = False Implements IGroup.ReadyForDownloadIgnore
        Friend Property Subscriptions As Boolean = False Implements IGroup.Subscriptions
        Friend Property SubscriptionsOnly As Boolean = False Implements IGroup.SubscriptionsOnly
        Friend Property UsersCount As Integer = 0 Implements IGroup.UsersCount
        Friend Sub New()
            Labels = New List(Of String)
            LabelsExcluded = New List(Of String)
            Sites = New List(Of String)
            SitesExcluded = New List(Of String)
        End Sub
        Protected Sub Import(ByVal e As EContainer)
            Name = e.Value(Name_Name)
            Temporary = e.Value(Name_Temporary).FromXML(Of Integer)(CInt(CheckState.Indeterminate))
            Favorite = e.Value(Name_Favorite).FromXML(Of Integer)(CInt(CheckState.Indeterminate))
            ReadyForDownload = e.Value(Name_ReadyForDownload).FromXML(Of Boolean)(True)
            ReadyForDownloadIgnore = e.Value(Name_ReadyForDownloadIgnore).FromXML(Of Boolean)(False)
            Subscriptions = e.Value(Name_Subscriptions).FromXML(Of Boolean)(False)
            SubscriptionsOnly = e.Value(Name_SubscriptionsOnly).FromXML(Of Boolean)(False)
            UsersCount = e.Value(Name_UsersCount).FromXML(Of Integer)(0)

            Dim l As New ListAddParams(LAP.NotContainsOnly)
            If Not e.Value(Name_Labels).IsEmptyString Then Labels.ListAddList(e.Value(Name_Labels).Split("|"), l)
            If Not e.Value(Name_Labels_Excluded).IsEmptyString Then LabelsExcluded.ListAddList(e.Value(Name_Labels_Excluded).Split("|"), l)
            If Not e.Value(Name_Sites).IsEmptyString Then Sites.ListAddList(e.Value(Name_Sites).Split("|"), l)
            If Not e.Value(Name_Sites_Excluded).IsEmptyString Then SitesExcluded.ListAddList(e.Value(Name_Sites_Excluded).Split("|"), l)
        End Sub
        Protected Function Export(ByVal e As EContainer) As EContainer
            e.AddRange({New EContainer(Name_Name, Name),
                        New EContainer(Name_Temporary, CInt(Temporary)),
                        New EContainer(Name_Favorite, CInt(Favorite)),
                        New EContainer(Name_ReadyForDownload, ReadyForDownload.BoolToInteger),
                        New EContainer(Name_ReadyForDownloadIgnore, ReadyForDownloadIgnore.BoolToInteger),
                        New EContainer(Name_Subscriptions, Subscriptions.BoolToInteger),
                        New EContainer(Name_SubscriptionsOnly, SubscriptionsOnly.BoolToInteger),
                        New EContainer(Name_UsersCount, UsersCount),
                        New EContainer(Name_Labels, Labels.ListToString("|")),
                        New EContainer(Name_Labels_Excluded, LabelsExcluded.ListToString("|")),
                        New EContainer(Name_Sites, Sites.ListToString("|")),
                        New EContainer(Name_Sites_Excluded, SitesExcluded.ListToString("|"))})
            Return e
        End Function
#Region "IDisposable Support"
        Protected disposedValue As Boolean = False
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Labels.Clear()
                    LabelsExcluded.Clear()
                    Sites.Clear()
                    SitesExcluded.Clear()
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