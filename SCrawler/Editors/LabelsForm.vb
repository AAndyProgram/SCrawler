' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Friend Class LabelsForm
    Private WithEvents MyDefs As DefaultFormOptions
    Friend ReadOnly Property LabelsList As List(Of String)
    Private ReadOnly NewLabels As List(Of String)
    Private ReadOnly _Source As IEnumerable(Of String) = Nothing
    Private ReadOnly Property Source As IEnumerable(Of String)
        Get
            If Not _Source Is Nothing Then
                Return _Source
            ElseIf AddNoParsed Then
                Return ListAddList(Nothing, Settings.Labels).ListAddValue(LabelsKeeper.NoParsedUser, LAP.NotContainsOnly)
            Else
                Return Settings.Labels
            End If
        End Get
    End Property
    Friend Property WithDeleteButton As Boolean = False
    Private ReadOnly AddNoParsed As Boolean = False
    Friend Property IsGroups As Boolean = False
    Friend Sub New(ByVal LabelsArr As IEnumerable(Of String), Optional ByVal AddNoParsed As Boolean = True)
        InitializeComponent()
        Me.AddNoParsed = AddNoParsed
        LabelsList = New List(Of String)
        LabelsList.ListAddList(LabelsArr)
        NewLabels = New List(Of String)
        MyDefs = New DefaultFormOptions(Me, Settings.Design)
    End Sub
    Friend Sub New(ByVal Current As IEnumerable(Of String), ByVal Source As IEnumerable(Of String))
        Me.New(Current, False)
        _Source = Source
    End Sub
    Private Sub LabelsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            With MyDefs
                .MyViewInitialize()
                .AddOkCancelToolbar()
                .MyOkCancel.BTT_DELETE.Visible = WithDeleteButton
                Dim s As List(Of String) = ListAddList(Nothing, Source).ListAddList(LabelsList, LAP.NotContainsOnly)
                If s.ListExists Then
                    Dim items As New List(Of Integer)
                    s.Sort()
                    CMB_LABELS.BeginUpdate()
                    For i% = 0 To s.Count - 1
                        If LabelsList.Contains(s(i)) Then items.Add(i)
                        CMB_LABELS.Items.Add(s(i))
                    Next
                    If Not _Source Is Nothing Then CMB_LABELS.Buttons.Clear()
                    CMB_LABELS.EndUpdate()
                    CMB_LABELS.ListCheckedIndexes = items
                End If
                .EndLoaderOperations()
            End With
        Catch ex As Exception
            MyDefs.InvokeLoaderError(ex)
        End Try
    End Sub
    Private Sub LabelsForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Dim b As Boolean = True
        If e.KeyCode = Keys.Insert And _Source Is Nothing Then
            AddNewLabel()
        ElseIf e.KeyCode = Keys.F3 And IsGroups Then
            EditSelectedGroup()
        Else
            b = False
        End If
        If b Then e.Handled = True
    End Sub
    Private Sub LabelsForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        LabelsList.Clear()
        NewLabels.Clear()
    End Sub
    Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
        Try
            LabelsList.ListAddList(CMB_LABELS.Items.CheckedItems.Select(Function(l) CStr(l.Value(0))), LAP.ClearBeforeAdd, LAP.NotContainsOnly)
            If _Source Is Nothing Then Settings.Labels.AddRange(NewLabels, True)
            MyDefs.CloseForm()
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Label selection")
        End Try
    End Sub
    Private Sub MyDefs_ButtonDeleteClickOC(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonDeleteClickOC
        LabelsList.Clear()
        MyDefs.CloseForm()
    End Sub
    Private Sub CMB_LABELS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As EventArgs) Handles CMB_LABELS.ActionOnButtonClick
        Select Case Sender.DefaultButton
            Case ActionButton.DefaultButtons.Add : AddNewLabel()
            Case ActionButton.DefaultButtons.Clear : CMB_LABELS.Clear(ComboBoxExtended.ClearMode.CheckedIndexes)
        End Select
    End Sub
    Private Sub AddNewLabel()
        Dim nl$ = InputBoxE("Enter new label name:", "New label")
        If Not nl.IsEmptyString Then
            If Settings.Labels.Contains(nl) Or NewLabels.Contains(nl) Then
                MsgBoxE($"Label [{nl}] already exists")
            Else
                NewLabels.Add(nl)
                CMB_LABELS.Items.Add(nl)
            End If
        End If
    End Sub
    Private Sub EditSelectedGroup()
        Try
            If CMB_LABELS.Count > 0 And CMB_LABELS.SelectedIndex >= 0 Then
                Dim gName$ = CMB_LABELS.Value
                Dim i%
                If Not gName.IsEmptyString Then
                    i = Settings.Groups.IndexOf(gName)
                    If i >= 0 Then
                        Using f As New DownloadObjects.Groups.GroupEditorForm(Settings.Groups(i)) : f.ShowDialog() : End Using
                    End If
                End If
            End If
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Show group")
        End Try
    End Sub
End Class