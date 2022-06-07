' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Forms.Toolbars
Friend Class ChannelsStatsForm : Implements IOkCancelDeleteToolbar
    Private ReadOnly MyDefs As DefaultFormProps
    Friend Property DeletedChannels As Integer = 0
    Friend Sub New()
        InitializeComponent()
        MyDefs = New DefaultFormProps
    End Sub
    Private Sub ChannelsStatsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            With MyDefs
                .MyViewInitialize(Me, Settings.Design)
                .AddOkCancelToolbar()
                .MyOkCancel.EnableDelete = False
                If Settings.Channels.Count > 0 Then
                    RefillList()
                Else
                    MsgBoxE("Channels not found", vbExclamation)
                End If
                .EndLoaderOperations()
            End With
        Catch ex As Exception
            MyDefs.InvokeLoaderError(ex)
        End Try
    End Sub
    Private Sub ChannelsStatsForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        MyDefs.Dispose()
    End Sub
    Private Sub RefillList()
        CMB_CHANNELS.Items.Clear()
        If Settings.Channels.Count > 0 Then
            CMB_CHANNELS.BeginUpdate()
            CMB_CHANNELS.Items.AddRange(Settings.Channels.Select(Function(c) New ListItem({$"[{c.ID}]: {c.GetChannelStats(False)}", c.ID})))
            CMB_CHANNELS.EndUpdate()
        End If
    End Sub
    Private Sub OK() Implements IOkCancelToolbar.OK
        MyDefs.CloseForm()
    End Sub
    Private Sub Cancel() Implements IOkCancelToolbar.Cancel
        MyDefs.CloseForm(DialogResult.Cancel)
    End Sub
    Private Sub Delete() Implements IOkCancelDeleteToolbar.Delete
        Try
            Dim c As List(Of String) = CMB_CHANNELS.Items.CheckedItems.Select(Function(cc) CStr(cc.Value(1))).ListIfNothing
            If c.ListExists Then
                If MsgBoxE({$"The following channels will be deleted:{vbCr}{c.ListToString(vbCr)}", "Deleting channels"}, vbExclamation,,, {"Confirm", "Cancel"}) = 0 Then
                    For Each CID$ In c : Settings.Channels.Remove(Settings.Channels.Find(CID)) : Next
                    MyMainLOG = $"Deleted channels:{vbNewLine}{c.ListToString(vbNewLine)}"
                    MsgBoxE("Channels deleted")
                    DeletedChannels += c.Count
                    c.Clear()
                    MyDefs.ChangesDetected = False
                    RefillList()
                Else
                    MsgBoxE("Operation canceled")
                End If
            Else
                MsgBoxE("No one channel checked", vbExclamation)
            End If
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Deleting channels")
        End Try
    End Sub
    Private Sub CMB_CHANNELS_ActionOnChangeDetected(ByVal c As Boolean) Handles CMB_CHANNELS.ActionOnChangeDetected
        If Not MyDefs.Initializing Then MyDefs.MyOkCancel.EnableDelete = CMB_CHANNELS.ListCheckedIndexes.Count > 0
    End Sub
    Private Sub CMB_CHANNELS_ActionOnButtonClearClick() Handles CMB_CHANNELS.ActionOnButtonClearClick
        CMB_CHANNELS.ListCheckedIndexes = Nothing
    End Sub
End Class