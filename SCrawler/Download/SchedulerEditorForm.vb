' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Toolbars
Namespace DownloadObjects
    Friend Class SchedulerEditorForm
        Private WithEvents MyDefs As DefaultFormOptions
        Private WithEvents BTT_SKIP As ToolStripButton
        Private WithEvents BTT_START As ToolStripButton
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            BTT_SKIP = New ToolStripButton With {
                .Text = "Skip",
                .ToolTipText = "Skip next run",
                .AutoToolTip = True,
                .DisplayStyle = ToolStripItemDisplayStyle.Text
            }
            BTT_START = New ToolStripButton With {
                .Text = "Start",
                .Image = My.Resources.StartPic_01_Green_16,
                .ToolTipText = "Run selected plan",
                .AutoToolTip = True
            }
        End Sub
        Private Sub SchedulerEditorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize()
                .AddEditToolbarPlus({BTT_START, BTT_SKIP})
                Refill()
                .EndLoaderOperations(False)
            End With
        End Sub
        Private Sub SchedulerEditorForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            If e.KeyCode = Keys.Escape Then Close()
        End Sub
        Private Sub Refill() Handles MyDefs.ButtonUpdateClick
            Try
                LIST_PLANS.Items.Clear()
                If Settings.Automation.Count > 0 Then
                    LIST_PLANS.Items.AddRange(Settings.Automation.Select(Function(a) a.ToString()).Cast(Of Object).ToArray)
                    If _LatestSelected.ValueBetween(0, LIST_PLANS.Items.Count - 1) Then LIST_PLANS.SelectedIndex = _LatestSelected
                Else
                    _LatestSelected = -1
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendInLog, ex)
            End Try
        End Sub
        Private Sub MyDefs_ButtonAddClick(ByVal Sender As Object, ByVal e As EditToolbar.EditToolbarEventArgs) Handles MyDefs.ButtonAddClick
            Dim a As New AutoDownloader(True)
            Using f As New AutoDownloaderEditorForm(a)
                f.ShowDialog()
                If f.DialogResult = DialogResult.OK Then
                    Settings.Automation.Add(a)
                    Refill()
                Else
                    a.Dispose()
                End If
            End Using
        End Sub
        Private Sub Edit() Handles MyDefs.ButtonEditClick
            If _LatestSelected.ValueBetween(0, LIST_PLANS.Items.Count - 1) Then
                Using f As New AutoDownloaderEditorForm(Settings.Automation(_LatestSelected)) : f.ShowDialog() : End Using
                Refill()
            Else
                MsgBoxE("You have not selected a plan to edit.", vbExclamation)
            End If
        End Sub
        Private _DeleteInProgress As Boolean = False
        Private Async Sub MyDefs_ButtonDeleteClickE(ByVal Sender As Object, ByVal e As EditToolbar.EditToolbarEventArgs) Handles MyDefs.ButtonDeleteClickE
            If Not _DeleteInProgress Then
                If _LatestSelected.ValueBetween(0, LIST_PLANS.Items.Count - 1) Then
                    _DeleteInProgress = True
                    Dim n$ = Settings.Automation(_LatestSelected).Name
                    If MsgBoxE({$"Are you sure you want to delete the [{n}] plan?", "Deleting a plan..."}, vbExclamation + vbYesNo) = vbYes Then
                        Await Settings.Automation.RemoveAt(_LatestSelected)
                        Refill()
                        MsgBoxE($"Plan [{n}] deleted")
                    End If
                    _DeleteInProgress = False
                Else
                    MsgBoxE("You have not selected a plan to delete.", vbExclamation)
                End If
            Else
                MsgBoxE({"One of the plans is currently in progress. Wait until this plan is stopped and deleted.", "Deleting a plan"}, vbExclamation)
            End If
        End Sub
        Private _LatestSelected As Integer = -1
        Private Sub LIST_PLANS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LIST_PLANS.SelectedIndexChanged
            _LatestSelected = LIST_PLANS.SelectedIndex
        End Sub
        Private Sub LIST_PLANS_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles LIST_PLANS.MouseDoubleClick
            Edit()
        End Sub
        Private Sub BTT_START_Click(sender As Object, e As EventArgs) Handles BTT_START.Click
            If _LatestSelected.ValueBetween(0, LIST_PLANS.Items.Count - 1) Then
                With Settings.Automation(_LatestSelected) : .Start(.IsNewPlan) : End With
                Refill()
            End If
        End Sub
        Private Sub BTT_SKIP_Click(sender As Object, e As EventArgs) Handles BTT_SKIP.Click
            If _LatestSelected.ValueBetween(0, LIST_PLANS.Items.Count - 1) Then
                Settings.Automation(_LatestSelected).Skip()
                Refill()
            End If
        End Sub
    End Class
End Namespace