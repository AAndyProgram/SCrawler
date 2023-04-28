' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls.Base
Namespace API.Mastodon
    Friend Class SettingsForm
        Private WithEvents MyDefs As DefaultFormOptions
        Friend ReadOnly Property MyCredentials As List(Of Credentials)
        Friend ReadOnly Property MyDomains As List(Of String)
        Friend Sub New(ByVal s As SiteSettings)
            InitializeComponent()
            MyCredentials = New List(Of Credentials)
            If s.Domains.Credentials.Count > 0 Then MyCredentials.AddRange(s.Domains.Credentials)
            MyDomains = New List(Of String)
            MyDomains.ListAddList(s.Domains)
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
        End Sub
        Private Sub MyForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyView = New FormView(Me, Settings.Design, "MastodonSettingsForm")
                .MyView.Import()
                .MyView.SetFormSize()
                .AddOkCancelToolbar()
                RefillList()
                .EndLoaderOperations()
            End With
        End Sub
        Private Sub SettingsForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            MyCredentials.Clear()
            MyDomains.Clear()
        End Sub
        Private Sub RefillList()
            CMB_DOMAINS.Items.Clear()
            If MyDomains.Count > 0 Then
                MyDomains.Sort()
                CMB_DOMAINS.BeginUpdate()
                CMB_DOMAINS.Items.AddRange(MyDomains.Select(Function(d) New ListItem(d)))
                CMB_DOMAINS.EndUpdate(True)
            End If
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            ApplyCredentials()
            If MyCredentials.Count > 0 Then MyCredentials.RemoveAll(Function(c) c.Domain.IsEmptyString Or c.Bearer.IsEmptyString Or c.Csrf.IsEmptyString)
            If MyDomains.Count > 0 Then
                If MyCredentials.Count > 0 Then
                    MyCredentials.RemoveAll(Function(c) Not MyDomains.Contains(c.Domain))
                Else
                    MyCredentials.Clear()
                End If
            End If
            MyDefs.CloseForm()
        End Sub
        Private Sub CMB_DOMAINS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles CMB_DOMAINS.ActionOnButtonClick
            Try
                Dim d$
                Dim i% = -1
                Select Case e.DefaultButton
                    Case ActionButton.DefaultButtons.Add
                        d = InputBoxE("Enter a new domain using the pattern [mastodon.social]:", "New domain")
                        If Not d.IsEmptyString Then
                            If MyDomains.Count > 0 Then i = MyDomains.IndexOf(d)
                            If i >= 0 Then
                                MsgBoxE({$"Domain '{d}' already exists", "Add domain"}, vbExclamation)
                                If i <= CMB_DOMAINS.Count - 1 Then CMB_DOMAINS.SelectedIndex = i
                            Else
                                ApplyCredentials()
                                ClearCredentials()
                                MyDomains.Add(d)
                                _Suspended = True
                                RefillList()
                                _Suspended = False
                                i = MyDomains.IndexOf(d)
                                If i.ValueBetween(0, CMB_DOMAINS.Count - 1) Then
                                    CMB_DOMAINS.SelectedIndex = i
                                Else
                                    _LatestSelected = -1
                                    _CurrentCredentialsIndex = -1
                                    _CurrentDomain = String.Empty
                                End If
                            End If
                        End If
                    Case ActionButton.DefaultButtons.Delete
                        If _LatestSelected >= 0 Then
                            d = CMB_DOMAINS.Items(_LatestSelected).Value(0)
                            If Not d.IsEmptyString AndAlso MsgBoxE({$"Are you sure you want to delete the [{d}] domain?",
                                                                   "Removing domains"}, vbYesNo) = vbYes Then
                                i = MyDomains.IndexOf(d)
                                Dim l% = _LatestSelected
                                If i >= 0 Then
                                    ClearCredentials()
                                    MyDomains.RemoveAt(i)
                                    _Suspended = True
                                    RefillList()
                                    _Suspended = False
                                    If (l - 1).ValueBetween(0, CMB_DOMAINS.Count - 1) Then
                                        CMB_DOMAINS.SelectedIndex = l - 1
                                    Else
                                        _LatestSelected = -1
                                        _CurrentCredentialsIndex = -1
                                        _CurrentDomain = String.Empty
                                    End If
                                End If
                            End If
                        End If
                End Select
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "API.Mastodon.SettingsForm.ActionButtonClick")
            End Try
        End Sub
        Private _LatestSelected As Integer = -1
        Private _CurrentCredentialsIndex As Integer = -1
        Private _CurrentDomain As String = String.Empty
        Private _Suspended As Boolean = False
        Private Sub CMB_DOMAINS_ActionSelectedItemChanged(ByVal Sender As Object, ByVal e As EventArgs, ByVal Item As ListViewItem) Handles CMB_DOMAINS.ActionSelectedItemChanged
            If Not MyDefs.Initializing And Not _Suspended Then
                Dim DropCredentials As Boolean = True
                If Not Item Is Nothing Then
                    ApplyCredentials()
                    _LatestSelected = Item.Index
                    _CurrentDomain = Item.Text
                    If MyCredentials.Count > 0 And Not _CurrentDomain.IsEmptyString Then
                        _CurrentCredentialsIndex = MyCredentials.IndexOf(_CurrentDomain)
                        If _CurrentCredentialsIndex >= 0 Then
                            With MyCredentials(_CurrentCredentialsIndex) : TXT_AUTH.Text = .Bearer : TXT_TOKEN.Text = .Csrf : End With
                            DropCredentials = False
                        End If
                    Else
                        _CurrentCredentialsIndex = -1
                    End If
                End If
                If DropCredentials Then ClearCredentials()
            End If
        End Sub
        Private Sub ClearCredentials()
            TXT_AUTH.Clear()
            TXT_TOKEN.Clear()
        End Sub
        Private Sub ApplyCredentials()
            Try
                If _LatestSelected >= 0 And Not _CurrentDomain.IsEmptyString Then
                    Dim c As New Credentials With {.Domain = _CurrentDomain, .Bearer = TXT_AUTH.Text, .Csrf = TXT_TOKEN.Text}
                    If _CurrentCredentialsIndex.ValueBetween(0, MyCredentials.Count - 1) Then MyCredentials(_CurrentCredentialsIndex) = c Else MyCredentials.Add(c)
                End If
            Catch ex As Exception
            End Try
        End Sub
    End Class
End Namespace