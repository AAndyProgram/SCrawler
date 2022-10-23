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
Friend Class ChannelsStatsForm
    Private WithEvents MyDefs As DefaultFormOptions
    Friend Property DeletedChannels As Integer = 0
    Friend Sub New()
        InitializeComponent()
        MyDefs = New DefaultFormOptions(Me, Settings.Design)
    End Sub
    Private Sub ChannelsStatsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        With MyDefs
            .MyViewInitialize()
            .AddOkCancelToolbar()
            If Settings.Channels.Count > 0 Then RefillList() Else MsgBoxE("Channels not found", vbExclamation)
            .DelegateClosingChecker = False
            .EndLoaderOperations()
        End With
    End Sub
    Private Sub RefillList()
        CMB_CHANNELS.Items.Clear()
        If Settings.Channels.Count > 0 Then
            CMB_CHANNELS.BeginUpdate()
            CMB_CHANNELS.Items.AddRange(Settings.Channels.Select(Function(c) New ListItem({$"[{c.ID}]: {c.GetChannelStats(False)}", c.ID})))
            CMB_CHANNELS.EndUpdate()
        End If
    End Sub
    Private Sub MyDefs_ButtonDeleteClickOC(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonDeleteClickOC
        Const MsgTitle$ = "Deleting channels"
        Try
            Dim c As List(Of String) = CMB_CHANNELS.Items.CheckedItems.Select(Function(cc) CStr(cc.Value(1))).ListIfNothing
            If c.ListExists Then
                If MsgBoxE({$"The following channels will be deleted:{vbCr}{c.ListToString(vbCr)}", MsgTitle}, vbExclamation,,, {"Confirm", "Cancel"}) = 0 Then
                    For Each CID$ In c : Settings.Channels.Remove(Settings.Channels.Find(CID)) : Next
                    MyMainLOG = $"Deleted channels:{vbNewLine}{c.ListToString(vbNewLine)}"
                    MsgBoxE({"Channels deleted", MsgTitle})
                    DeletedChannels += c.Count
                    c.Clear()
                    MyDefs.ChangesDetected = False
                    RefillList()
                Else
                    MsgBoxE({"Operation canceled", MsgTitle})
                End If
            Else
                MsgBoxE({"No channels marked for deletion", MsgTitle}, vbExclamation)
            End If
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, MsgTitle)
        End Try
    End Sub
    Private Sub CMB_CHANNELS_ActionOnChangeDetected(ByVal c As Boolean) Handles CMB_CHANNELS.ActionOnChangeDetected
        If Not MyDefs.Initializing Then MyDefs.MyOkCancel.EnableDelete = CMB_CHANNELS.ListCheckedIndexes.Count > 0
    End Sub
    Private Sub CMB_CHANNELS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As EventArgs) Handles CMB_CHANNELS.ActionOnButtonClick
        If Sender.DefaultButton = ActionButton.DefaultButtons.Clear Then CMB_CHANNELS.ListCheckedIndexes = Nothing
    End Sub
End Class