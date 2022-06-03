' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace DownloadObjects.Groups
    Friend Interface IGroup
        ReadOnly Property Labels As List(Of String)
        Property Temporary As CheckState
        Property Favorite As CheckState
        Property ReadyForDownload As Boolean
        Property ReadyForDownloadIgnore As Boolean
    End Interface
    Friend Class GroupParameters : Implements IGroup, IDisposable
        Protected Const Name_Temporary As String = "Temporary"
        Protected Const Name_Favorite As String = "Favorite"
        Protected Const Name_ReadyForDownload As String = "RFD"
        Protected Const Name_ReadyForDownloadIgnore As String = "RFDI"
        Friend ReadOnly Property Labels As List(Of String) Implements IGroup.Labels
        Friend Property Temporary As CheckState = CheckState.Indeterminate Implements IGroup.Temporary
        Friend Property Favorite As CheckState = CheckState.Indeterminate Implements IGroup.Favorite
        Friend Property ReadyForDownload As Boolean = True Implements IGroup.ReadyForDownload
        Friend Property ReadyForDownloadIgnore As Boolean = False Implements IGroup.ReadyForDownloadIgnore
        Friend Sub New()
            Labels = New List(Of String)
        End Sub
#Region "IDisposable Support"
        Protected disposedValue As Boolean = False
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then Labels.Clear()
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