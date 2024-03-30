' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports SCrawler.API.Base
Imports PersonalUtilities.Forms
Namespace DownloadObjects
    Friend Class DownloadedInfoForm
#Region "Events"
        Friend Event UserFind(ByVal Key As String)
#End Region
#Region "Declarations"
        Private MyView As FormView
        Private Opened As Boolean = False
        Friend ReadOnly Property ReadyToOpen As Boolean
            Get
                Return Settings.DownloadOpenInfo And (Not Opened Or Settings.DownloadOpenInfo.Attribute) And Not Visible
            End Get
        End Property
        Friend Enum ViewModes As Integer
            Session = 0
            All = 1
        End Enum
        Friend Property ViewMode As ViewModes
            Get
                Return IIf(ControlInvokeFast(ToolbarTOP, MENU_VIEW_ALL, Function() MENU_VIEW_ALL.Checked, True, EDP.ReturnValue), ViewModes.All, ViewModes.Session)
            End Get
            Set(ByVal SMode As ViewModes)
                Settings.InfoViewMode.Value = CInt(SMode)
            End Set
        End Property
        Private ReadOnly _UsersListSession As List(Of IUserData)
        Private ReadOnly _UsersListAll As List(Of IUserData)
        Private ReadOnly Property Current As List(Of IUserData)
            Get
                Return If(ViewMode = ViewModes.All, _UsersListAll, _UsersListSession)
            End Get
        End Property
        Private Overloads ReadOnly Property SelectedUser As IUserData
            Get
                If ViewMode = ViewModes.All Then
                    If _LatestSelected.ValueBetween(0, _UsersListAll.Count - 1) Then Return _UsersListAll(_LatestSelected)
                Else
                    If _LatestSelected.ValueBetween(0, _UsersListSession.Count - 1) Then Return _UsersListSession(_LatestSelected)
                End If
                Return Nothing
            End Get
        End Property
#End Region
#Region "Initializer"
        Public Sub New()
            InitializeComponent()
            _UsersListSession = New List(Of IUserData)
            _UsersListAll = New List(Of IUserData)
            If Settings.InfoViewMode.Value = ViewModes.All Then
                MENU_VIEW_SESSION.Checked = False
                MENU_VIEW_ALL.Checked = True
            Else
                MENU_VIEW_SESSION.Checked = True
                MENU_VIEW_ALL.Checked = False
            End If
            OPT_DEFAULT.Checked = Settings.InfoViewDefault
            OPT_SUBSCRIPTIONS.Checked = Not Settings.InfoViewDefault
        End Sub
#End Region
#Region "Form handlers"
        Private Sub DownloadedInfoForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                If MyView Is Nothing Then
                    MyView = New FormView(Me, Settings.Design)
                    MyView.Import()
                    MyView.SetFormSize()
                End If
                ControlInvokeFast(ToolbarTOP, BTT_CLEAR, Sub() BTT_CLEAR.Visible = ViewMode = ViewModes.Session, EDP.None)
            Catch
            Finally
                Opened = True
                RefillList()
            End Try
        End Sub
        Private Sub DownloadedInfoForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            e.Cancel = True
            Hide()
        End Sub
        Private Sub DownloadedInfoForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            MyView.DisposeIfReady()
            _UsersListSession.Clear()
            _UsersListAll.Clear()
        End Sub
        Private Sub DownloadedInfoForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            Dim b As Boolean = True
            Select Case e.KeyCode
                Case Keys.F5 : RefillList()
                Case Keys.F2 : UpdateNavigationButtons(1)
                Case Keys.F3 : UpdateNavigationButtons(-1)
                Case Keys.F4 : BTT_FIND.PerformClick()
                Case Keys.Enter : LIST_DOWN_MouseDoubleClick(Nothing, Nothing)
                Case Else : b = False
            End Select
            If b Then e.Handled = True
        End Sub
#End Region
#Region "Refill"
        Private Class UsersDateOrder : Implements IComparer(Of IUserData)
            Friend Function Compare(ByVal x As IUserData, ByVal y As IUserData) As Integer Implements IComparer(Of IUserData).Compare
                Dim xv& = If(DirectCast(x, UserDataBase).LastUpdated.HasValue, DirectCast(x, UserDataBase).LastUpdated.Value.Ticks, 0)
                Dim yv& = If(DirectCast(y, UserDataBase).LastUpdated.HasValue, DirectCast(y, UserDataBase).LastUpdated.Value.Ticks, 0)
                Return xv.CompareTo(yv) * -1
            End Function
        End Class
        Private Sub RefillList() Handles BTT_REFRESH.Click
            If Opened Then
                Try
                    ControlInvokeFast(LIST_DOWN, Sub() LIST_DOWN.Items.Clear())
                    If ViewMode = ViewModes.Session Then
                        With Downloader.Downloaded
                            If .Count > 0 Then
                                With .Select(Function(u) Settings.GetUser(u, False)).Reverse
                                    If _UsersListSession.Count > 0 Then _UsersListSession.ListWithRemove(.Self, New ListAddParams With {.DisableDispose = True})
                                    If _UsersListSession.Count > 0 Then
                                        _UsersListSession.InsertRange(0, .Self)
                                    Else
                                        _UsersListSession.AddRange(.Self)
                                    End If
                                End With
                            End If
                        End With
                    Else
                        _UsersListAll.ListAddList(Settings.GetUsers(Function(u) True), LAP.ClearBeforeAdd)
                        If _UsersListAll.Count > 0 Then _UsersListAll.Sort(New UsersDateOrder)
                    End If
                    Dim isDefault As Boolean = OPT_DEFAULT.Checked
                    If Current.Count > 0 Then Current.RemoveAll(Function(u) u Is Nothing OrElse u.IsSubscription = isDefault)
                    If Current.Count > 0 Then
                        ControlInvokeFast(LIST_DOWN,
                                          Sub()
                                              For Each user As IUserData In Current
                                                  LIST_DOWN.Items.Add(user.DownloadedInformation)
                                              Next
                                              If _LatestSelected.ValueBetween(0, LIST_DOWN.Items.Count - 1) Then
                                                  LIST_DOWN.SelectedIndex = _LatestSelected
                                              Else
                                                  _LatestSelected = -1
                                              End If
                                          End Sub)
                    Else
                        _LatestSelected = -1
                    End If
                Catch ies As InvalidOperationException
                Catch ex As Exception
                    ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadedInfoForm.RefillList]")
                Finally
                    UpdateNavigationButtons(Nothing)
                End Try
            End If
        End Sub
#End Region
#Region "Toolbar controls"
        Private Sub MENU_VIEW_Click(ByVal Sender As ToolStripMenuItem, ByVal e As EventArgs) Handles MENU_VIEW_SESSION.Click, MENU_VIEW_ALL.Click
            Try
                Dim __refill As Boolean = False
                Dim clicked As ToolStripMenuItem = Sender
                Dim other As ToolStripMenuItem = If(Sender Is MENU_VIEW_SESSION, MENU_VIEW_ALL, MENU_VIEW_SESSION)
                ControlInvokeFast(ToolbarTOP, clicked, Sub()
                                                           If other.Checked Then
                                                               clicked.Checked = True
                                                               other.Checked = False
                                                               __refill = True
                                                           Else
                                                               clicked.Checked = False
                                                           End If
                                                           ViewMode = IIf(MENU_VIEW_SESSION.Checked, ViewModes.Session, ViewModes.All)
                                                           BTT_CLEAR.Visible = ViewMode = ViewModes.Session
                                                       End Sub, EDP.SendToLog)
                If __refill Then RefillList()
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "DownloadedInfoForm.ViewChange")
            End Try
        End Sub
        Private Sub OPT_Click(ByVal Sender As ToolStripMenuItem, ByVal e As EventArgs) Handles OPT_DEFAULT.Click, OPT_SUBSCRIPTIONS.Click
            Try
                Dim __refill As Boolean = False
                Dim clicked As ToolStripMenuItem = Sender
                Dim other As ToolStripMenuItem = If(Sender Is OPT_DEFAULT, OPT_SUBSCRIPTIONS, OPT_DEFAULT)
                ControlInvokeFast(ToolbarTOP, clicked, Sub()
                                                           If other.Checked Then
                                                               clicked.Checked = True
                                                               other.Checked = False
                                                               __refill = True
                                                           Else
                                                               clicked.Checked = False
                                                           End If
                                                       End Sub, EDP.SendToLog)
                Settings.InfoViewDefault.Value = OPT_DEFAULT.Checked
                If __refill Then RefillList()
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "DownloadedInfoForm.SubscriptionChange")
            End Try
        End Sub
        Private Sub BTT_FIND_Click(sender As Object, e As EventArgs) Handles BTT_FIND.Click
            Try : RaiseEvent UserFind(If(Settings.GetUser(SelectedUser, True)?.Key, String.Empty)) : Catch : End Try
        End Sub
        Private Sub BTT_CLEAR_Click(sender As Object, e As EventArgs) Handles BTT_CLEAR.Click
            If LIST_DOWN.Items.Count > 0 Then Downloader.Downloaded.Clear() : RefillList()
        End Sub
#End Region
#Region "List handlers"
        Private _LatestSelected As Integer = -1
        Private Sub LIST_DOWN_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LIST_DOWN.SelectedIndexChanged
            _LatestSelected = LIST_DOWN.SelectedIndex
            UpdateNavigationButtons(Nothing)
        End Sub
        Private Sub LIST_DOWN_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles LIST_DOWN.MouseDoubleClick
            Try
                Dim u As IUserData = SelectedUser
                If Not If(u?.Disposed, True) Then u.OpenFolder()
            Catch
            End Try
        End Sub
#End Region
#Region "Navigation"
        Private Sub BTT_UP_Click(sender As Object, e As EventArgs) Handles BTT_UP.Click
            UpdateNavigationButtons(-1)
        End Sub
        Private Sub BTT_DOWN_Click(sender As Object, e As EventArgs) Handles BTT_DOWN.Click
            UpdateNavigationButtons(1)
        End Sub
        Private Sub UpdateNavigationButtons(ByVal Offset As Integer?)
            Dim u As Boolean = False, d As Boolean = False
            With LIST_DOWN
                If .Items.Count > 0 Then
                    u = _LatestSelected > 0
                    d = _LatestSelected < .Items.Count - 1
                End If
                ControlInvokeFast(ToolbarTOP, BTT_UP, Sub()
                                                          BTT_UP.Enabled = u
                                                          BTT_DOWN.Enabled = d
                                                      End Sub, EDP.None)
                If Offset.HasValue AndAlso .Items.Count > 0 AndAlso (_LatestSelected + Offset.Value).ValueBetween(0, .Items.Count - 1) Then _
                   ControlInvokeFast(LIST_DOWN, Sub() .SelectedIndex = _LatestSelected + Offset.Value, EDP.None)
            End With
        End Sub
#End Region
#Region "Downloader handlers"
        Friend Sub Downloader_DownloadCountChange()
            If Opened AndAlso ViewMode = ViewModes.Session Then RefillList()
        End Sub
#End Region
    End Class
End Namespace