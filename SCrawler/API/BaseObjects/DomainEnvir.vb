' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Namespace API.BaseObjects
    Friend Interface IDomainContainer
        ReadOnly Property Icon As Icon
        ReadOnly Property Site As String
        ReadOnly Property Domains As List(Of String)
        ReadOnly Property DomainsTemp As List(Of String)
        ReadOnly Property DomainsDefault As String
        ReadOnly Property DomainsSettingProp As Plugin.PropertyValue
        Property DomainsChanged As Boolean
        Property Initialized As Boolean
        Property DomainsUpdateInProgress As Boolean
        Property DomainsUpdatedBySite As Boolean
        Sub UpdateDomains()
    End Interface
    Friend NotInheritable Class DomainContainer
        Private Sub New()
        End Sub
        Friend Shared Sub EndInit(ByVal s As IDomainContainer)
            If ACheck(s.DomainsSettingProp.Value) Then s.Domains.ListAddList(CStr(s.DomainsSettingProp.Value).Split("|"), LAP.NotContainsOnly)
        End Sub
        Friend Overloads Shared Sub UpdateDomains(ByVal s As IDomainContainer)
            UpdateDomains(s, Nothing, True)
        End Sub
        Friend Overloads Shared Sub UpdateDomains(ByVal s As IDomainContainer, ByVal NewDomains As IEnumerable(Of String), ByVal Internal As Boolean)
            With s
                If Not .Initialized Or (.DomainsUpdatedBySite And Not Internal) Then Exit Sub
                If Not .DomainsUpdateInProgress Then
                    .DomainsUpdateInProgress = True
                    .Domains.ListAddList(.DomainsDefault.Split("|"), LAP.NotContainsOnly)
                    .Domains.ListAddList(NewDomains, LAP.NotContainsOnly)
                    .DomainsSettingProp.Value = .Domains.ListToString("|")
                    If Not Internal Then .DomainsUpdatedBySite = True
                    .DomainsUpdateInProgress = False
                End If
            End With
        End Sub
        Friend Shared Sub Update(ByVal s As IDomainContainer)
            With s
                If .DomainsChanged Then
                    .Domains.Clear()
                    .Domains.ListAddList(.DomainsTemp, LAP.NotContainsOnly)
                    .UpdateDomains()
                End If
            End With
        End Sub
        Friend Shared Sub EndEdit(ByVal s As IDomainContainer)
            s.DomainsTemp.ListAddList(s.Domains, LAP.ClearBeforeAdd, LAP.NotContainsOnly)
            s.DomainsChanged = False
        End Sub
        Friend Shared Sub OpenSettingsForm(ByVal s As IDomainContainer)
            Dim __add As EventHandler(Of SimpleListFormEventArgs) = Sub(sender, e) e.ValueNew = InputBoxE($"Enter a new domain using the pattern [{s.Site}.com]:", "New domain").IfNullOrEmptyE(Nothing)
            Dim __delete As EventHandler(Of SimpleListFormEventArgs) = Sub(sender, e)
                                                                           Dim n$ = AConvert(Of String)(e.ValueCurrent, AModes.Var, String.Empty)
                                                                           e.Result = MsgBoxE({$"Are you sure you want to delete the [{n}] domain?",
                                                                                              "Removing domains"}, vbYesNo) = vbYes
                                                                       End Sub
            Using f As New SimpleListForm(Of String)(If(s.DomainsChanged, s.DomainsTemp, s.Domains), Settings.Design) With {
                .Buttons = {ADB.Add, ADB.Delete},
                .Mode = SimpleListFormModes.Remaining,
                .FormText = s.Site,
                .Icon = s.Icon,
                .LocationOnly = True,
                .Size = New Size(400, 330),
                .DesignXMLNodeName = s.Site
            }
                AddHandler f.AddClick, __add
                AddHandler f.DeleteClick, __delete
                f.ShowDialog()
                If f.DialogResult = DialogResult.OK Then
                    s.DomainsChanged = True
                    s.DomainsTemp.ListAddList(f.DataResult, LAP.ClearBeforeAdd, LAP.NotContainsOnly)
                End If
            End Using
        End Sub
    End Class
End Namespace