' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Tools
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Namespace API.Base
    Friend Class DomainsContainer : Implements IEnumerable(Of String), IMyEnumerator(Of String), IDisposable
        Friend Event DomainsUpdated(ByVal Sender As DomainsContainer)
        Friend ReadOnly Property Domains As List(Of String)
        Friend ReadOnly Property DomainsTemp As List(Of String)
        Friend ReadOnly Property DomainsDefault As String
        Friend Property Changed As Boolean
        Private DomainsUpdateInProgress As Boolean = False
        Friend Property UpdatedBySite As Boolean
        Protected ReadOnly Property Instance As ISiteSettings
        Friend Property DestinationProp As PropertyValue
        Default Friend ReadOnly Property Item(ByVal Index As Integer) As String Implements IMyEnumerator(Of String).MyEnumeratorObject
            Get
                Return Domains(Index)
            End Get
        End Property
        Friend ReadOnly Property Count As Integer Implements IMyEnumerator(Of String).MyEnumeratorCount
            Get
                Return Domains.Count
            End Get
        End Property
        Friend Sub New(ByVal _Instance As ISiteSettings, ByVal DefaultValue As String)
            Domains = New List(Of String)
            DomainsTemp = New List(Of String)
            Instance = _Instance
            DomainsDefault = DefaultValue
            If Not DomainsDefault.IsEmptyString Then Domains.ListAddList(CStr(DomainsDefault).Split("|"), LAP.NotContainsOnly)
        End Sub
        Friend Sub PopulateInitialDomains(ByVal InitialValue As String)
            If Not InitialValue.IsEmptyString Then Domains.ListAddList(CStr(InitialValue).Split("|"), LAP.NotContainsOnly)
        End Sub
        Public Overrides Function ToString() As String
            Return Domains.ListToString("|")
        End Function
        Friend Sub Add(ByVal NewDomains As IEnumerable(Of String), ByVal UpdateBySite As Boolean)
            If Not DomainsUpdateInProgress Then
                DomainsUpdateInProgress = True
                Domains.ListAddList(NewDomains, LAP.NotContainsOnly)
                If UpdateBySite Then Me.UpdatedBySite = True
                Save()
                DomainsUpdateInProgress = False
                RaiseEvent DomainsUpdated(Me)
            End If
        End Sub
        Friend Overridable Function Apply() As Boolean
            If Changed Then
                Domains.Clear()
                Domains.ListAddList(DomainsTemp, LAP.NotContainsOnly)
                Save()
                RaiseEvent DomainsUpdated(Me)
                Return True
            Else
                Return False
            End If
        End Function
        Friend Overridable Sub Save()
            If Not DestinationProp Is Nothing Then DestinationProp.Value = ToString()
        End Sub
        Friend Overridable Sub Reset()
            Changed = False
            DomainsTemp.Clear()
        End Sub
        Friend Overridable Sub OpenSettingsForm()
            Dim __add As EventHandler(Of SimpleListFormEventArgs) = Sub(sender, e) e.Item = InputBoxE($"Enter a new domain using the pattern [{Instance.Site}.com]:", "New domain").IfNullOrEmptyE(Nothing)
            Dim __delete As EventHandler(Of SimpleListFormEventArgs) = Sub(sender, e)
                                                                           Dim n$ = AConvert(Of String)(e.Item, AModes.Var, String.Empty)
                                                                           e.Result = MsgBoxE({$"Are you sure you want to delete the [{n}] domain?",
                                                                                              "Removing domains"}, vbYesNo) = vbYes
                                                                       End Sub
            Using f As New SimpleListForm(Of String)(If(Changed, DomainsTemp, Domains), Settings.Design) With {
                .Buttons = {ADB.Add, ADB.Delete},
                .Mode = SimpleListFormModes.Remaining,
                .FormText = Instance.Site,
                .Icon = Instance.Icon,
                .LocationOnly = True,
                .Size = New Size(400, 330),
                .DesignXMLNodeName = $"{Instance.Site}_DomainsForm"
            }
                AddHandler f.AddClick, __add
                AddHandler f.DeleteClick, __delete
                f.ShowDialog()
                If f.DialogResult = DialogResult.OK Then
                    Changed = True
                    DomainsTemp.Clear()
                    DomainsTemp.ListAddList(f.DataResult, LAP.NotContainsOnly)
                End If
            End Using
        End Sub
#Region "IEnumerable Support"
        Private Function GetEnumerator() As IEnumerator(Of String) Implements IEnumerable(Of String).GetEnumerator
            Return New MyEnumerator(Of String)(Me)
        End Function
        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
#End Region
#Region "IDisposable Support"
        Private disposedValue As Boolean = False
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Domains.Clear()
                    DomainsTemp.Clear()
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