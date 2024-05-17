' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports System.Collections.ObjectModel
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Toolbars
Imports ECI = PersonalUtilities.Forms.Toolbars.EditToolbar.ControlItem
Namespace DownloadObjects.Groups
    Friend Class GroupListForm
#Region "Declarations"
        Private WithEvents MyDefs As DefaultFormOptions
        Private ReadOnly IsViewFilter As Boolean
        Private WithEvents BTT_MOVE_UP As ToolStripButton
        Private WithEvents BTT_MOVE_DOWN As ToolStripButton
        Private WithEvents BTT_DOWNLOAD As ToolStripKeyMenuItem
        Private ReadOnly MyGroups As ObservableCollection(Of String)
        Private ReadOnly MyGroupParams As List(Of GroupParameters)
        Private _GroupsUpdated As Boolean = False
        Friend ReadOnly Property GroupsUpdated As Boolean
            Get
                Return _GroupsUpdated
            End Get
        End Property
        Private _GroupToDownload As String = String.Empty
        Friend ReadOnly Property GroupToDownload As String
            Get
                Return _GroupToDownload
            End Get
        End Property
        Private _GroupToDownloadIncludeInTheFeed As Boolean = True
        Friend ReadOnly Property GroupToDownloadIncludeInTheFeed As Boolean
            Get
                Return _GroupToDownloadIncludeInTheFeed
            End Get
        End Property
        Private _FilterSelected As GroupParameters = Nothing
        Friend ReadOnly Property FilterSelected As GroupParameters
            Get
                Return _FilterSelected
            End Get
        End Property
#End Region
#Region "Initializer"
        Friend Sub New(ByVal _IsViewFilter As Boolean)
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            MyGroups = New ObservableCollection(Of String)
            MyGroupParams = New List(Of GroupParameters)
            IsViewFilter = _IsViewFilter
            If Not IsViewFilter Then
                BTT_MOVE_UP = New ToolStripButton With {
                    .Image = PersonalUtilities.My.Resources.ArrowUpPic_Blue_32,
                    .Text = String.Empty,
                    .DisplayStyle = ToolStripItemDisplayStyle.Image,
                    .ToolTipText = "Move up",
                    .AutoToolTip = True
                }
                BTT_MOVE_DOWN = New ToolStripButton With {
                    .Image = PersonalUtilities.My.Resources.ArrowDownPic_Blue_32,
                    .Text = String.Empty,
                    .DisplayStyle = ToolStripItemDisplayStyle.Image,
                    .ToolTipText = "Move down",
                    .AutoToolTip = True
                }
                BTT_DOWNLOAD = New ToolStripKeyMenuItem("Download", My.Resources.StartPic_Green_16) With {.ToolTipText = "Download selected group"}
            Else
                Text = "Filters"
            End If
        End Sub
#End Region
#Region "Form handlers"
        Private Sub GroupListForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDefs
                    .MyViewInitialize()

                    If Not IsViewFilter Then
                        .AddEditToolbarPlus({BTT_MOVE_UP, BTT_MOVE_DOWN, ECI.Separator, BTT_DOWNLOAD})
                        With Settings.Groups.Where(Function(g) g.IsViewFilter = IsViewFilter)
                            If .ListExists Then .ListForEach(Sub(g, i) MyGroups.Add(g.Name))
                        End With
                    Else
                        .AddEditToolbar()
                        With .MyEditToolbar : .ButtonKey(ECI.Add) = Nothing : .Enabled(ECI.Add) = False : End With
                        With Settings.Groups.Cast(Of GroupParameters).Concat(Settings.Automation.Cast(Of GroupParameters)).ToList.
                             ListSort(New FComparer(Of GroupParameters)(
                                      Function(ByVal x As GroupParameters, ByVal y As GroupParameters) As Integer
                                          Dim getVal As Func(Of GroupParameters, Long) =
                                                        Function(ByVal g As GroupParameters) As Long
                                                            Dim v& = 0
                                                            If TypeOf g Is AutoDownloader Then
                                                                v = CLng(Integer.MaxValue) * 1000
                                                            ElseIf TypeOf g Is DownloadGroup Then
                                                                If Not g.IsViewFilter Then v = CLng(Integer.MaxValue) * 100
                                                            End If
                                                            v += g.ToStringViewFilters.GetHashCode
                                                            Return v
                                                        End Function
                                          Return getVal(x).CompareTo(getVal(y))
                                      End Function))
                            If .ListExists Then .ListForEach(Sub(ByVal g As GroupParameters, ByVal __indx As Integer)
                                                                 MyGroups.Add(g.Name)
                                                                 MyGroupParams.Add(g)
                                                             End Sub)
                        End With
                        CONTAINER_MAIN.BottomToolStripPanel.Visible = True
                        .AddOkCancelToolbar()
                        .MyOkCancel.ToolTipOk = "Apply selected filter"
                    End If

                    RefillList()
                    If Not IsViewFilter Then Settings.Groups.BeginUpdate()

                    If IsViewFilter And LIST_GROUPS.Items.Count > 0 Then .MyOkCancel.EnableOK = True : _LatestSelected = 0

                    .DelegateClosingChecker = False

                    .EndLoaderOperations()
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub GroupListForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            If Not IsViewFilter And GroupsUpdated And MyGroups.Count > 0 Then
                With Settings.Groups
                    Dim myIndx%, grIndx%
                    For myIndx = 0 To MyGroups.Count - 1
                        grIndx = .IndexOf(MyGroups(myIndx))
                        If grIndx >= 0 Then .Item(grIndx).Index = myIndx
                    Next
                    .Sort()
                    .Reindex()
                    .Sort()
                End With
            End If
            MyGroups.Clear()
            MyGroupParams.Clear()
            Settings.Groups.EndUpdate()
            If GroupsUpdated Then Settings.Groups.Update()
        End Sub
        Private Sub GroupListForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            If e.KeyCode = Keys.Escape Then
                Close()
            ElseIf e = ShowUsersButtonKey Then
                ShowUsers()
            End If
        End Sub
#End Region
#Region "Refill"
        Private Sub RefillList(Optional ByVal OffsetIndex As Integer? = Nothing)
            Try
                With LIST_GROUPS
                    .BeginUpdate()
                    With .Items
                        .Clear()
                        If IsViewFilter Then
                            If MyGroupParams.Count > 0 Then .AddRange(MyGroupParams.Select(Function(g) g.ToStringViewFilters).Cast(Of Object).ToArray)
                        Else
                            If MyGroups.Count > 0 Then .AddRange(MyGroups.Cast(Of Object).ToArray)
                        End If
                    End With
                    .EndUpdate()
                    If OffsetIndex.HasValue Then
                        Dim newIndx% = _LatestSelected + OffsetIndex.Value
                        If newIndx.ValueBetween(0, .Items.Count - 1) Then .SelectedIndex = newIndx : _LatestSelected = newIndx
                    ElseIf .Items.Count > 0 Then
                        If _LatestSelected.ValueBetween(0, .Items.Count - 1) Then
                            .SelectedIndex = _LatestSelected
                        ElseIf (_LatestSelected - 1).ValueBetween(0, .Items.Count - 1) Then
                            _LatestSelected -= 1
                            .SelectedIndex = _LatestSelected
                        ElseIf (_LatestSelected + 1).ValueBetween(0, .Items.Count - 1) Then
                            _LatestSelected += 1
                            .SelectedIndex = _LatestSelected
                        End If
                    End If
                End With
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[GroupListForm.RefillList]")
            End Try
        End Sub
#End Region
#Region "Ok Cancel"
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            If _LatestSelected.ValueBetween(0, MyGroupParams.Count - 1) Then
                _FilterSelected = MyGroupParams(_LatestSelected)
                MyDefs.CloseForm()
            Else
                MsgBoxE({"You haven't selected a filter", "Apply filter"}, vbCritical)
            End If
        End Sub
#End Region
#Region "Toolbar buttons"
        Private Sub MyDefs_ButtonAddClick(ByVal Sender As Object, ByVal e As EditToolbarEventArgs) Handles MyDefs.ButtonAddClick
            If Not IsViewFilter Then
                Dim i% = Settings.Groups.Add
                If i >= 0 Then
                    MyGroups.Add(Settings.Groups(i).Name)
                    _LatestSelected = MyGroups.Count - 1
                    _GroupsUpdated = True
                    RefillList()
                End If
            End If
        End Sub
        Private Sub MyDefs_ButtonEditClick(ByVal Sender As Object, ByVal e As EditToolbarEventArgs) Handles MyDefs.ButtonEditClick
            EditItem()
        End Sub
        Private Function ItemEditDeleteReady(ByVal Operation As String) As DialogResult
            If IsViewFilter Then
                If _LatestSelected.ValueBetween(0, MyGroupParams.Count - 1) Then
                    With MyGroupParams(_LatestSelected)
                        If TypeOf .Self Is AutoDownloader Then
                            MsgBoxE({$"You cannot {Operation} the scheduler plan through this form", $"{Operation.ToUpperFirstChar} filter"}, vbCritical)
                            Return DialogResult.Abort
                        ElseIf TypeOf .Self Is DownloadGroup AndAlso Not DirectCast(.Self, DownloadGroup).IsViewFilter Then
                            MsgBoxE({$"You cannot {Operation} the group through this form", $"{Operation.ToUpperFirstChar} filter"}, vbCritical)
                            Return DialogResult.Abort
                        End If
                    End With
                End If
            End If
            Return DialogResult.OK
        End Function
        Private Sub EditItem()
            If _LatestSelected.ValueBetween(0, MyGroups.Count - 1) AndAlso ItemEditDeleteReady("edit") = DialogResult.OK Then
                Dim i% = Settings.Groups.IndexOf(MyGroups(_LatestSelected), IsViewFilter)
                If i >= 0 Then
                    Using f As New GroupEditorForm(Settings.Groups(i))
                        f.ShowDialog()
                        If f.DialogResult = DialogResult.OK Then
                            If IsViewFilter Then
                                MyGroups(_LatestSelected) = Settings.Groups(i).ToStringViewFilters
                                Settings.Groups.Update()
                            Else
                                MyGroups(_LatestSelected) = Settings.Groups(i).Name
                                _GroupsUpdated = True
                            End If
                            RefillList()
                        End If
                    End Using
                End If
            End If
        End Sub
        Private Sub MyDefs_ButtonDeleteClickEdit(ByVal Sender As Object, ByVal e As EditToolbarEventArgs) Handles MyDefs.ButtonDeleteClickE
            Try
                If _LatestSelected.ValueBetween(0, MyGroups.Count - 1) AndAlso ItemEditDeleteReady("delete") = DialogResult.OK Then
                    Dim i% = Settings.Groups.IndexOf(MyGroups(_LatestSelected), IsViewFilter)
                    If i >= 0 Then
                        Dim n$ = Settings.Groups(i).Name
                        If Settings.Groups(i).Delete Then
                            If _LatestSelected.ValueBetween(0, MyGroupParams.Count - 1) Then MyGroupParams.RemoveAt(_LatestSelected)
                            MyGroups.Remove(n)
                            RefillList()
                            If IsViewFilter Then
                                Settings.Groups.Update()
                            Else
                                _GroupsUpdated = True
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[GroupListForm.Delete]")
            End Try
        End Sub
        Private Sub BTT_MOVE_UP_DOWN_Click(sender As Object, e As EventArgs) Handles BTT_MOVE_UP.Click, BTT_MOVE_DOWN.Click
            Try
                Dim offset% = IIf(sender Is BTT_MOVE_UP, -1, 1)
                If _LatestSelected.ValueBetween(0, MyGroups.Count - 1) And (_LatestSelected + offset).ValueBetween(0, MyGroups.Count - 1) Then
                    MyGroups.Move(_LatestSelected, _LatestSelected + offset)
                    RefillList(offset)
                    _GroupsUpdated = True
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[GroupListForm.Move]")
            End Try
        End Sub
        Private Sub BTT_DOWNLOAD_KeyClick(ByVal Sender As Object, ByVal e As MyKeyEventArgs) Handles BTT_DOWNLOAD.KeyClick
            If Not IsViewFilter AndAlso _LatestSelected.ValueBetween(0, MyGroups.Count - 1) Then
                Dim i% = Settings.Groups.IndexOf(MyGroups(_LatestSelected))
                If i >= 0 Then _GroupToDownload = Settings.Groups(i).Name : _GroupToDownloadIncludeInTheFeed = e.IncludeInTheFeed : Close()
            End If
        End Sub
#End Region
#Region "List handlers"
        Private _LatestSelected As Integer = -1
        Private Sub LIST_GROUPS_Click(sender As Object, e As EventArgs) Handles LIST_GROUPS.Click
            _LatestSelected = LIST_GROUPS.SelectedIndex
        End Sub
        Private Sub LIST_GROUPS_DoubleClick(sender As Object, e As EventArgs) Handles LIST_GROUPS.DoubleClick
            If IsViewFilter Then
                MyDefs.MyOkCancel.BTT_OK.PerformClick()
            Else
                EditItem()
            End If
        End Sub
#End Region
#Region "ShowUsers"
        Private Sub ShowUsers()
            Try
                If _LatestSelected.ValueBetween(0, MyGroups.Count - 1) Then
                    Dim i%
                    Dim n$ = String.Empty
                    Dim users As New List(Of API.Base.IUserData)
                    If Not IsViewFilter Then
                        i = Settings.Groups.IndexOf(MyGroups(_LatestSelected))
                        If i >= 0 Then users.ListAddList(DownloadGroup.GetUsers(Settings.Groups(i))) : n = $"F {Settings.Groups(i).Name}"
                    ElseIf _LatestSelected.ValueBetween(0, MyGroupParams.Count - 1) Then
                        With MyGroupParams(_LatestSelected)
                            If TypeOf .Self Is AutoDownloader Then
                                With DirectCast(.Self, AutoDownloader)
                                    If Not .Mode = AutoDownloader.Modes.None Then
                                        If .Mode = AutoDownloader.Modes.Groups Then
                                            If .Groups.Count > 0 Then
                                                For Each groupName$ In .Groups
                                                    i = Settings.Groups.IndexOf(groupName)
                                                    If i >= 0 Then users.ListAddList(DownloadGroup.GetUsers(Settings.Groups(i)), LAP.NotContainsOnly, LAP.IgnoreICopier)
                                                Next
                                            End If
                                        Else
                                            users.ListAddList(DownloadGroup.GetUsers(.Self))
                                        End If
                                    End If
                                    n = $"S { .Name}"
                                End With
                            ElseIf TypeOf .Self Is DownloadGroup Then
                                i = Settings.Groups.IndexOf(.Name, .IsViewFilter)
                                If i >= 0 Then users.ListAddList(DownloadGroup.GetUsers(Settings.Groups(i))) : n = $"G {Settings.Groups(i).Name}"
                            End If
                        End With
                    End If
                    GroupUsersViewer.Show(users, n)
                    users.Clear()
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Show plan users")
            End Try
        End Sub
#End Region
    End Class
End Namespace