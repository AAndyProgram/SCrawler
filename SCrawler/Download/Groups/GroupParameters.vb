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
        Property LabelsNo As Boolean
        ReadOnly Property Labels As List(Of String)
        ReadOnly Property LabelsExcluded As List(Of String)
        Property LabelsExcludedIgnore As Boolean
        ReadOnly Property Sites As List(Of String)
        ReadOnly Property SitesExcluded As List(Of String)
        Property Regular As Boolean
        Property Temporary As Boolean
        Property Favorite As Boolean
        Property ReadyForDownload As Boolean
        Property ReadyForDownloadIgnore As Boolean
        Property DownloadUsers As Boolean
        Property DownloadSubscriptions As Boolean
        Property UsersCount As Integer
        Property DaysNumber As Integer
        Property DaysIsDownloaded As Boolean
        Property UserDeleted As Boolean
        Property UserSuspended As Boolean
        Property UserExists As Boolean
        Property DateFrom As Date?
        Property DateTo As Date?
        Property DateMode As ShowingDates
    End Interface
    Friend Class GroupParameters : Implements IGroup, IDisposable, ICopier
#Region "XML names"
#Region "Old"
        Protected Const Name_Subscriptions As String = "Subscriptions"
        Protected Const Name_SubscriptionsOnly As String = "SubscriptionsOnly"
#End Region
        Protected Const Name_Name As String = "Name"
        Protected Const Name_Regular As String = "Regular"
        Protected Const Name_Temporary As String = "Temporary"
        Protected Const Name_Favorite As String = "Favorite"
        Protected Const Name_ReadyForDownload As String = "RFD"
        Protected Const Name_ReadyForDownloadIgnore As String = "RFDI"

        Protected Const Name_DownloadUsers As String = "DownloadUsers"
        Protected Const Name_DownloadSubscriptions As String = "DownloadSubscriptions"

        Protected Const Name_UsersCount As String = "UsersCount"
        Protected Const Name_LabelsNo As String = "LabelsNo"
        Protected Const Name_Labels As String = "Labels"
        Protected Const Name_Labels_Excluded As String = "LabelsExcluded"
        Protected Const Name_LabelsExcludedIgnore As String = "LabelsExcludedIgnore"
        Protected Const Name_Sites As String = "Sites"
        Protected Const Name_Sites_Excluded As String = "SitesExcluded"
        Protected Const Name_DaysNumber As String = "DaysNumber"
        Protected Const Name_DaysIsDownloaded As String = "DaysIsDownloaded"
        Protected Const Name_UserDeleted As String = "UserDeleted"
        Protected Const Name_UserSuspended As String = "UserSuspended"
        Protected Const Name_UserExists As String = "UserExists"
        Protected Const Name_DateFrom As String = "DateFrom"
        Protected Const Name_DateTo As String = "DateTo"
        Protected Const Name_DateMode As String = "DateMode"
        Protected Const Name_IsViewFilter As String = "IsViewFilter"
#End Region
#Region "Declarations"
        Friend Overridable Property Name As String Implements IGroup.Name
        Friend Property LabelsNo As Boolean = False Implements IGroup.LabelsNo
        Friend ReadOnly Property Labels As List(Of String) Implements IGroup.Labels
        Friend ReadOnly Property LabelsExcluded As List(Of String) Implements IGroup.LabelsExcluded
        Friend Property LabelsExcludedIgnore As Boolean = False Implements IGroup.LabelsExcludedIgnore
        Friend ReadOnly Property Sites As List(Of String) Implements IGroup.Sites
        Friend ReadOnly Property SitesExcluded As List(Of String) Implements IGroup.SitesExcluded
        Friend Property Regular As Boolean = True Implements IGroup.Regular
        Friend Property Temporary As Boolean = True Implements IGroup.Temporary
        Friend Property Favorite As Boolean = True Implements IGroup.Favorite
        Friend Property ReadyForDownload As Boolean = True Implements IGroup.ReadyForDownload
        Friend Property ReadyForDownloadIgnore As Boolean = False Implements IGroup.ReadyForDownloadIgnore
        Friend Property DownloadUsers As Boolean = True Implements IGroup.DownloadUsers
        Friend Property DownloadSubscriptions As Boolean = True Implements IGroup.DownloadSubscriptions
        Friend Property UsersCount As Integer = 0 Implements IGroup.UsersCount
        Friend Property DaysNumber As Integer = -1 Implements IGroup.DaysNumber
        Friend Property DaysIsDownloaded As Boolean = False Implements IGroup.DaysIsDownloaded
        Friend Property UserDeleted As Boolean = False Implements IGroup.UserDeleted
        Friend Property UserSuspended As Boolean = True Implements IGroup.UserSuspended
        Friend Property UserExists As Boolean = True Implements IGroup.UserExists
        Friend Property DateFrom As Date? = Nothing Implements IGroup.DateFrom
        Friend Property DateTo As Date? = Nothing Implements IGroup.DateTo
        Friend Property DateMode As ShowingDates = ShowingDates.Off Implements IGroup.DateMode
        Friend Property IsViewFilter As Boolean = False
#End Region
#Region "Initializer"
        Friend Sub New()
            Labels = New List(Of String)
            LabelsExcluded = New List(Of String)
            Sites = New List(Of String)
            SitesExcluded = New List(Of String)
        End Sub
#End Region
#Region "Base functions"
        Public Overrides Function ToString() As String
            Return Name
        End Function
        Friend Overridable Function ToStringViewFilters() As String
            Return ToString()
        End Function
#End Region
#Region "ICopier Support"
        Friend Overridable Overloads Function Copy() As Object Implements ICopier.Copy
            Return (New GroupParameters).Copy(Me)
        End Function
        Friend Overridable Overloads Function Copy(ByVal Source As Object) As Object Implements ICopier.Copy
            With DirectCast(Source, GroupParameters)
                Name = .Name
                LabelsNo = .LabelsNo
                Labels.ListAddList(.Labels, LAP.ClearBeforeAdd)
                LabelsExcluded.ListAddList(.LabelsExcluded, LAP.ClearBeforeAdd)
                LabelsExcludedIgnore = .LabelsExcludedIgnore
                Sites.ListAddList(.Sites, LAP.ClearBeforeAdd)
                SitesExcluded.ListAddList(.SitesExcluded, LAP.ClearBeforeAdd)
                Regular = .Regular
                Temporary = .Temporary
                Favorite = .Favorite
                ReadyForDownload = .ReadyForDownload
                ReadyForDownloadIgnore = .ReadyForDownloadIgnore
                DownloadUsers = .DownloadUsers
                DownloadSubscriptions = .DownloadSubscriptions
                UsersCount = .UsersCount
                DaysNumber = .DaysNumber
                DaysIsDownloaded = .DaysIsDownloaded
                UserDeleted = .UserDeleted
                UserSuspended = .UserSuspended
                UserExists = .UserExists
                DateFrom = .DateFrom
                DateTo = .DateTo
                DateMode = .DateMode
                IsViewFilter = .IsViewFilter
            End With
            Return Me
        End Function
#End Region
#Region "Import/Export"
        Protected Overridable Sub Import(ByVal e As EContainer)
            Name = e.Value(Name_Name)

            Dim l As New ListAddParams(LAP.NotContainsOnly)
            LabelsNo = e.Value(Name_LabelsNo).FromXML(Of Boolean)(False)
            If Not e.Value(Name_Labels).IsEmptyString Then Labels.ListAddList(e.Value(Name_Labels).Split("|"), l)
            If Not e.Value(Name_Labels_Excluded).IsEmptyString Then LabelsExcluded.ListAddList(e.Value(Name_Labels_Excluded).Split("|"), l)
            LabelsExcludedIgnore = e.Value(Name_LabelsExcludedIgnore).FromXML(Of Boolean)(False)
            If Not e.Value(Name_Sites).IsEmptyString Then Sites.ListAddList(e.Value(Name_Sites).Split("|"), l)
            If Not e.Value(Name_Sites_Excluded).IsEmptyString Then SitesExcluded.ListAddList(e.Value(Name_Sites_Excluded).Split("|"), l)

            Regular = e.Value(Name_Regular).FromXML(Of Boolean)(True)
            Temporary = e.Value(Name_Temporary).FromXML(Of Boolean)(True)
            Favorite = e.Value(Name_Favorite).FromXML(Of Boolean)(True)
            ReadyForDownload = e.Value(Name_ReadyForDownload).FromXML(Of Boolean)(True)
            ReadyForDownloadIgnore = e.Value(Name_ReadyForDownloadIgnore).FromXML(Of Boolean)(False)

            If e.Contains(Name_SubscriptionsOnly) Then
                DownloadUsers = Not e.Value(Name_SubscriptionsOnly).FromXML(Of Boolean)(False)
            Else
                DownloadUsers = e.Value(Name_DownloadUsers).FromXML(Of Boolean)(True)
            End If
            If e.Contains(Name_Subscriptions) Then
                DownloadSubscriptions = e.Value(Name_Subscriptions).FromXML(Of Boolean)(False)
            Else
                DownloadSubscriptions = e.Value(Name_DownloadSubscriptions).FromXML(Of Boolean)(False)
            End If

            UsersCount = e.Value(Name_UsersCount).FromXML(Of Integer)(0)
            DaysNumber = e.Value(Name_DaysNumber).FromXML(Of Integer)(-1)
            DaysIsDownloaded = e.Value(Name_DaysIsDownloaded).FromXML(Of Boolean)(False)
            UserDeleted = e.Value(Name_UserDeleted).FromXML(Of Boolean)(False)
            UserSuspended = e.Value(Name_UserSuspended).FromXML(Of Boolean)(True)
            UserExists = e.Value(Name_UserExists).FromXML(Of Boolean)(True)
            DateFrom = AConvert(Of Date)(e.Value(Name_DateFrom), DateTimeDefaultProvider, Nothing)
            DateTo = AConvert(Of Date)(e.Value(Name_DateTo), DateTimeDefaultProvider, Nothing)
            DateMode = e.Value(Name_DateMode).FromXML(Of Integer)(ShowingDates.Off)
            IsViewFilter = e.Value(Name_IsViewFilter).FromXML(Of Boolean)(False)
        End Sub
        Protected Overridable Function Export(ByVal e As EContainer) As EContainer
            e.AddRange({New EContainer(Name_Name, Name),
                        New EContainer(Name_LabelsNo, LabelsNo.BoolToInteger),
                        New EContainer(Name_Labels, Labels.ListToString("|")),
                        New EContainer(Name_Labels_Excluded, LabelsExcluded.ListToString("|")),
                        New EContainer(Name_LabelsExcludedIgnore, LabelsExcludedIgnore.BoolToInteger),
                        New EContainer(Name_Sites, Sites.ListToString("|")),
                        New EContainer(Name_Sites_Excluded, SitesExcluded.ListToString("|")),
                        New EContainer(Name_Regular, Regular.BoolToInteger),
                        New EContainer(Name_Temporary, Temporary.BoolToInteger),
                        New EContainer(Name_Favorite, Favorite.BoolToInteger),
                        New EContainer(Name_ReadyForDownload, ReadyForDownload.BoolToInteger),
                        New EContainer(Name_ReadyForDownloadIgnore, ReadyForDownloadIgnore.BoolToInteger),
                        New EContainer(Name_DownloadUsers, DownloadUsers.BoolToInteger),
                        New EContainer(Name_DownloadSubscriptions, DownloadSubscriptions.BoolToInteger),
                        New EContainer(Name_UsersCount, UsersCount),
                        New EContainer(Name_DaysNumber, DaysNumber),
                        New EContainer(Name_DaysIsDownloaded, DaysIsDownloaded.BoolToInteger),
                        New EContainer(Name_UserDeleted, UserDeleted.BoolToInteger),
                        New EContainer(Name_UserSuspended, UserSuspended.BoolToInteger),
                        New EContainer(Name_UserExists, UserExists.BoolToInteger),
                        New EContainer(Name_DateFrom, AConvert(Of String)(DateFrom, DateTimeDefaultProvider, String.Empty)),
                        New EContainer(Name_DateTo, AConvert(Of String)(DateTo, DateTimeDefaultProvider, String.Empty)),
                        New EContainer(Name_DateMode, CInt(DateMode)),
                        New EContainer(Name_IsViewFilter, IsViewFilter.BoolToInteger)})
            Return e
        End Function
#End Region
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