' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Toolbars
Namespace API.XVIDEOS
    Friend Class SettingsForm
        Private Const SettingsDesignXmlNode As String = "XvideosSettingsForm"
        Private WithEvents MyDefs As DefaultFormOptions
        Private ReadOnly Property Source As SiteSettings
        Friend Sub New(ByRef s As SiteSettings)
            InitializeComponent()
            Source = s
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
        End Sub
        Private Sub SettingsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                If Not Settings.Design.Contains(SettingsDesignXmlNode) Then Settings.Design.Add(SettingsDesignXmlNode, String.Empty)
                .MyViewInitialize(Me, Settings.Design(SettingsDesignXmlNode), True)
                .AddEditToolbar()
                .AddOkCancelToolbar()
                If Source.Domains.Count > 0 Then Source.Domains.ForEach(Sub(d) LIST_DOMAINS.Items.Add(d))
                .EndLoaderOperations()
            End With
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            Source.Domains.Clear()
            With LIST_DOMAINS
                If .Items.Count > 0 Then
                    For Each i In .Items : Source.Domains.Add(i.ToString) : Next
                End If
            End With
            Source.UpdateDomains()
            MyDefs.CloseForm()
        End Sub
        Private Sub MyDefs_ButtonAddClick(ByVal Sender As Object, ByVal e As EditToolbarEventArgs) Handles MyDefs.ButtonAddClick
            Dim nd$ = InputBoxE("Enter a new domain using the pattern [xvideos.com]:", "New domain")
            If Not nd.IsEmptyString Then
                If Not LIST_DOMAINS.Items.Contains(nd) Then
                    LIST_DOMAINS.Items.Add(nd)
                Else
                    MsgBoxE($"The domain [{nd}] already added")
                End If
            End If
        End Sub
        Private Sub MyDefs_ButtonDeleteClickE(ByVal Sender As Object, ByVal e As EditToolbarEventArgs) Handles MyDefs.ButtonDeleteClickE
            Const MsgTitle$ = "Removing domains"
            If _LatestSelected.ValueBetween(0, LIST_DOMAINS.Items.Count - 1) Then
                Dim n$ = LIST_DOMAINS.Items(_LatestSelected)
                If MsgBoxE({$"Are you sure you want to delete the [{n}] domain?", MsgTitle}, MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    LIST_DOMAINS.Items.RemoveAt(_LatestSelected)
                    MsgBoxE({$"Domain [{n}] removed", MsgTitle})
                Else
                    MsgBoxE({"Operation canceled", MsgTitle})
                End If
            Else
                MsgBoxE({"No domain selected", MsgTitle}, vbExclamation)
            End If
        End Sub
        Private _LatestSelected As Integer = -1
        Private Sub LIST_DOMENS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LIST_DOMAINS.SelectedIndexChanged
            _LatestSelected = LIST_DOMAINS.SelectedIndex
        End Sub
    End Class
End Namespace