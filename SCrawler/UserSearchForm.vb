' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports PersonalUtilities.Forms
Imports SCrawler.API
Imports SCrawler.API.Base
Friend Class UserSearchForm
    Private ReadOnly MyView As FormView
    Private ReadOnly Results As List(Of SearchResult)
    Private ReadOnly RLP As New ListAddParams(LAP.NotContainsOnly)
    Private _UFInit As Boolean = True
    Private Structure SearchResult : Implements IComparable(Of SearchResult)
        Friend Enum Modes : URL : Name : Description : Label : End Enum
        Friend ReadOnly Key As String
        Friend ReadOnly Text As String
        Friend ReadOnly IsCollection As Boolean
        Friend ReadOnly Mode As Modes
        Friend Sub New(ByVal User As IUserData, ByVal Mode As Modes)
            Key = User.Key
            IsCollection = User.IsCollection
            Me.Mode = Mode
            Text = $"[{Mode.ToString.First.ToString.ToUpper}] [{IIf(IsCollection, "C", "U")}] "
            If IsCollection Then
                Text &= $"[{User.CollectionName}] "
            Else
                If User.IncludedInCollection Then Text &= $"[{User.CollectionName}] "
                Text &= $"[{User.Site}] [{User.Name}]"
                If Not User.FriendlyName.IsEmptyString Then Text &= $" ({User.FriendlyName})"
            End If
        End Sub
        Private Function CompareTo(ByVal Other As SearchResult) As Integer Implements IComparable(Of SearchResult).CompareTo
            If CInt(Mode).CompareTo(CInt(Other.Mode)) = 0 Then
                Return IsCollection.CompareTo(Other.IsCollection)
            Else
                Return CInt(Mode).CompareTo(CInt(Other.Mode)) = 0
            End If
        End Function
        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            With DirectCast(Obj, SearchResult) : Return Key = .Key : End With
        End Function
    End Structure
    Public Sub New()
        InitializeComponent()
        Results = New List(Of SearchResult)
        MyView = New FormView(Me)
        MyView.Import(Settings.Design)
    End Sub
    Private Sub UserSearchForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        MyView.SetFormSize()
        CH_SEARCH_IN_NAME.Checked = Settings.SearchInName
        CH_SEARCH_IN_DESCR.Checked = Settings.SearchInDescription
        CH_SEARCH_IN_LABEL.Checked = Settings.SearchInLabel
        _UFInit = False
    End Sub
    Private Sub UserSearchForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        e.Cancel = True
        Hide()
    End Sub
    Private Sub UserSearchForm_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged
        If Not _UFInit And Visible Then TXT_SEARCH.Select() : TXT_SEARCH.SelectAll()
    End Sub
    Private Sub UserSearchForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then Hide() : e.Handled = True
    End Sub
    Private Sub UserSearchForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        MyView.Dispose()
        Results.Clear()
    End Sub
    Private Sub TXT_SEARCH_TextChanged(sender As Object, e As EventArgs) Handles TXT_SEARCH.TextChanged
        SearchUser()
    End Sub
    Private Sub TXT_SEARCH_KeyDown(sender As Object, e As KeyEventArgs) Handles TXT_SEARCH.KeyDown
        If e.KeyCode = Keys.Enter And Not e.Shift Then SearchUser() : e.Handled = True
    End Sub
    Private Sub LIST_SEARCH_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles LIST_SEARCH.MouseDoubleClick
        FocusUser()
    End Sub
    Private Sub LIST_SEARCH_KeyDown(sender As Object, e As KeyEventArgs) Handles LIST_SEARCH.KeyDown
        If e.KeyCode = Keys.Enter Then FocusUser() : e.Handled = True
    End Sub
    Private Sub CH_SEARCH_IN_NAME_CheckedChanged(sender As Object, e As EventArgs) Handles CH_SEARCH_IN_NAME.CheckedChanged
        If Not _UFInit Then Settings.SearchInName.Value = CH_SEARCH_IN_NAME.Checked
    End Sub
    Private Sub CH_SEARCH_IN_DESCR_CheckedChanged(sender As Object, e As EventArgs) Handles CH_SEARCH_IN_DESCR.CheckedChanged
        If Not _UFInit Then Settings.SearchInDescription.Value = CH_SEARCH_IN_DESCR.Checked
    End Sub
    Private Sub CH_SEARCH_IN_LABEL_CheckedChanged(sender As Object, e As EventArgs) Handles CH_SEARCH_IN_LABEL.CheckedChanged
        If Not _UFInit Then Settings.SearchInLabel.Value = CH_SEARCH_IN_LABEL.Checked
    End Sub
    Private Sub SearchUser()
        Try
            LIST_SEARCH.BeginUpdate()
            ControlInvokeFast(LIST_SEARCH, Sub() LIST_SEARCH.Items.Clear())
            Results.Clear()
            Dim t$ = TXT_SEARCH.Text.StringTrim.StringToLower
            With Settings
                If Not t.IsEmptyString And .Users.Count > 0 Then
                    Dim i%
                    Dim s As Plugin.ExchangeOptions = Nothing
                    Dim __descr As Boolean = CH_SEARCH_IN_DESCR.Checked
                    Dim __name As Boolean = CH_SEARCH_IN_NAME.Checked
                    Dim __lbl As Boolean = CH_SEARCH_IN_LABEL.Checked
                    Dim __isUrl As Boolean = t.StartsWith("http")
                    Dim __urlFound As Boolean = False
                    Dim _p_url As Predicate(Of IUserData) = Function(u) __urlFound AndAlso ((u.Site = s.SiteName Or u.HOST.Key = s.HostKey) And u.Name.ToLower = s.UserName.ToLower)
                    Dim _p_descr As Predicate(Of IUserData) = Function(u) __descr AndAlso Not u.Description.IsEmptyString AndAlso u.Description.ToLower.Contains(t)
                    Dim _p_name_friendly As Predicate(Of IUserData) = Function(u) Not u.FriendlyName.IsEmptyString AndAlso u.FriendlyName.ToLower.Contains(t)
                    Dim _p_labels_p As Predicate(Of String) = Function(l) l.ToLower.Contains(t)
                    Dim _p_labels As Predicate(Of IUserData) = Function(u) __lbl AndAlso u.Labels.ListExists AndAlso u.Labels.Exists(_p_labels_p)
                    Dim _addValue As Action(Of IUserData, SearchResult.Modes, Predicate(Of IUserData)) = Sub(u, m, p) If p.Invoke(u) Then Results.ListAddValue(New SearchResult(u, m), RLP)

                    If __isUrl Then
                        For Each p In Settings.Plugins
                            s = p.Settings.IsMyUser(t)
                            If Not s.UserName.IsEmptyString Then Exit For
                        Next
                        __urlFound = Not s.UserName.IsEmptyString
                    End If

                    For Each user As IUserData In .Users
                        If Not __isUrl AndAlso __name AndAlso (user.Name.ToLower.Contains(t) OrElse _p_name_friendly.Invoke(user)) Then Results.ListAddValue(New SearchResult(user, SearchResult.Modes.Name), RLP)
                        If user.IsCollection Then
                            With DirectCast(user, UserDataBind)
                                If .Count > 0 Then
                                    For i = 0 To .Count - 1
                                        With .Item(i)
                                            If Not __isUrl AndAlso __name AndAlso (.Self.Name.ToLower.Contains(t) OrElse _p_name_friendly.Invoke(.Self)) Then _
                                               Results.ListAddValue(New SearchResult(.Self, SearchResult.Modes.Name), RLP)
                                            _addValue(.Self, SearchResult.Modes.URL, _p_url)
                                            _addValue(.Self, SearchResult.Modes.Description, _p_descr)
                                            _addValue(.Self, SearchResult.Modes.Label, _p_labels)
                                        End With
                                    Next
                                End If
                            End With
                        Else
                            _addValue(user, SearchResult.Modes.URL, _p_url)
                            _addValue(user, SearchResult.Modes.Description, _p_descr)
                            _addValue(user, SearchResult.Modes.Label, _p_labels)
                        End If
                    Next
                    If Results.Count > 0 Then
                        Results.Sort()
                        ControlInvokeFast(LIST_SEARCH, Sub() Results.ForEach(Sub(d) LIST_SEARCH.Items.Add(d.Text)))
                    End If
                End If
            End With
        Catch aex As ArgumentOutOfRangeException
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.SendToLog, ex, $"[UserSearchForm.SearchUser({TXT_SEARCH.Text})]")
        Finally
            LIST_SEARCH.EndUpdate()
        End Try
    End Sub
    Private Sub FocusUser()
        Dim i% = LIST_SEARCH.SelectedIndex
        If i.ValueBetween(0, Results.Count - 1) Then MainFrameObj.FocusUser(Results(i).Key, True)
        If Not CH_LEAVE_OPEN.Checked Then Hide()
    End Sub
End Class