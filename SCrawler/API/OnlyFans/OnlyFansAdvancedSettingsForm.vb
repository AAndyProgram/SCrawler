' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Forms.Toolbars
Namespace API.OnlyFans
    Friend Class OnlyFansAdvancedSettingsForm
#Region "Declarations"
        Private WithEvents MyDefs As DefaultFormOptions
        Friend Property CurrentRulesEnv As DynamicRulesEnv
        Private ReadOnly Property CurrentRulesEnv_LIST As DynamicRulesEnv
#End Region
#Region "Initializer"
        Friend Sub New(ByVal rules As DynamicRulesEnv)
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            CurrentRulesEnv = rules
            CurrentRulesEnv_LIST = New DynamicRulesEnv
            CurrentRulesEnv_LIST.Copy(rules, False)
        End Sub
#End Region
#Region "Form handlers"
        Private Sub OnlyFansAdvancedSettingsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize()
                .AddOkCancelToolbar()

                .MyEditToolbar = New EditToolbar(Me,, CONTAINER_LIST.TopToolStripPanel) With {.Buttons = Nothing}
                .MyEditToolbar.AddThisToolbar()

                With CurrentRulesEnv
                    Select Case .Mode
                        Case DynamicRulesEnv.Modes.List : OPT_RULES_LIST.Checked = True
                        Case DynamicRulesEnv.Modes.Personal : TXT_PERSONAL_RULE.Checked = True
                    End Select
                    CH_LOG_ERR.Checked = .AddErrorsToLog
                    CH_RULES_REPLACE_CONFIG.Checked = .RulesReplaceConfig
                    CH_UPDATE_RULES_CONST.Checked = .RulesUpdateConst
                    CH_CONFIG_MANUAL_MODE.Checked = .RulesConfigManualMode
                    CH_UPDATE_CONF.Checked = .ConfigAutoUpdate
                    TXT_UP_INTERVAL.Text = .UpdateInterval
                    If Not .PersonalRule.IsEmptyString Then TXT_PERSONAL_RULE.Text = .PersonalRule
                    Refill()
                    CH_PROTECTED.Checked = .ProtectFile
                    CH_FORCE_UPDATE.Checked = .RulesForceUpdateRequired
                End With

                .MyFieldsCheckerE = New FieldsChecker
                .MyFieldsCheckerE.AddControl(Of Integer)(TXT_UP_INTERVAL, TXT_UP_INTERVAL.CaptionText,,
                                                         New FieldsCheckerProviderSimple(Function(v) IIf(AConvert(Of Integer)(v, 0, EDP.ReturnValue) > 0, v, Nothing),
                                                                                                     "The value of [{0}] field must be greater than 0"))
                .MyFieldsCheckerE.EndLoaderOperations()

                .EndLoaderOperations()
            End With
        End Sub
        Private Sub OnlyFansAdvancedSettingsForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            CurrentRulesEnv_LIST.Dispose()
        End Sub
#End Region
#Region "Refill"
        Private Sub Refill()
            With CurrentRulesEnv_LIST
                Dim ls% = _LatestSelected
                LIST_RULES.Items.Clear()
                If .Count > 0 Then LIST_RULES.Items.AddRange(.Select(Function(r) r.UrlRepo).Cast(Of Object).ToArray)
                Dim lim% = LIST_RULES.Items.Count - 1
                If (ls - 1).ValueBetween(0, lim) Then
                    ls -= 1
                ElseIf (ls + 1).ValueBetween(0, lim) Then
                    ls += 1
                End If
                If ls.ValueBetween(0, lim) Then LIST_RULES.SelectedIndex = ls Else ls = -1
                _LatestSelected = ls
            End With
        End Sub
#End Region
#Region "OK, Cancel"
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            If MyDefs.MyFieldsChecker.AllParamsOK Then
                With CurrentRulesEnv
                    .Copy(CurrentRulesEnv_LIST, True)
                    .ProtectFile = CH_PROTECTED.Checked
                    .UpdateInterval = AConvert(Of Integer)(TXT_UP_INTERVAL.Text, DynamicRulesEnv.UpdateIntervalDefault)
                    .Mode = If(TXT_PERSONAL_RULE.Checked, DynamicRulesEnv.Modes.Personal, DynamicRulesEnv.Modes.List)
                    .PersonalRule = TXT_PERSONAL_RULE.Text
                    .RulesReplaceConfig = CH_RULES_REPLACE_CONFIG.Checked
                    .RulesUpdateConst = CH_UPDATE_RULES_CONST.Checked
                    .RulesConfigManualMode = CH_CONFIG_MANUAL_MODE.Checked
                    .ConfigAutoUpdate = CH_UPDATE_CONF.Checked
                    .AddErrorsToLog = CH_LOG_ERR.Checked
                    If CH_FORCE_UPDATE.Checked Then .RulesForceUpdateRequired = True
                    .NeedToSave = True
                End With
                MyDefs.CloseForm()
            End If
        End Sub
#End Region
#Region "Add, Delete"
        Private Sub MyDefs_ButtonAddClick(ByVal Sender As Object, ByVal e As EditToolbarEventArgs) Handles MyDefs.ButtonAddClick
            Const msgTitle$ = "Add a rule"
            Dim i%
            Dim rule As DynamicRulesValue
            Dim r$ = InputBoxE("Enter a valid rules URL:", msgTitle)
            If Not r.IsEmptyString Then
                rule = Rules.ParseURL(r)
                If rule.Valid Then
                    i = CurrentRulesEnv_LIST.IndexOf(r)
                    If i >= 0 Then
                        MsgBoxE({$"The rule you entered already exists:{vbCr}{rule.UrlRepo}", msgTitle}, vbCritical)
                    Else
                        CurrentRulesEnv_LIST.Add(rule, False, False)
                        Refill()
                    End If
                Else
                    MsgBoxE({$"The rule you entered has an incompatible format:{vbCr}{r}", msgTitle}, vbCritical)
                End If
            End If
        End Sub
        Private Sub MyDefs_ButtonDeleteClickE(ByVal Sender As Object, ByVal e As EditToolbarEventArgs) Handles MyDefs.ButtonDeleteClickE
            If _LatestSelected.ValueBetween(0, LIST_RULES.Items.Count - 1) Then
                Dim r$ = LIST_RULES.Items(_LatestSelected)
                If MsgBoxE({$"Are you sure you want to delete the following rule?{vbCr}{r}", "Delete a rule"}, vbExclamation,,, {"Process", "Cancel"}) = 0 Then
                    If CurrentRulesEnv_LIST.RemoveAt(_LatestSelected) Then
                        LIST_RULES.Items.RemoveAt(_LatestSelected)
                        Refill()
                    Else
                        MsgBoxE({$"The following rule cannot be deleted:{vbCr}{r}", "Delete a rule"}, vbCritical)
                    End If
                End If
            End If
        End Sub
#End Region
#Region "Options"
        Private Sub TXT_UP_INTERVAL_ActionOnButtonClick(ByVal Sender As Object, ByVal e As ActionButtonEventArgs) Handles TXT_UP_INTERVAL.ActionOnButtonClick
            If e.DefaultButton = ActionButton.DefaultButtons.Refresh Then TXT_UP_INTERVAL.Text = DynamicRulesEnv.UpdateIntervalDefault
        End Sub
        Private Sub TXT_PERSONAL_RULE_ActionOnCheckedChange(ByVal Sender As Object, ByVal e As EventArgs, ByVal Checked As Boolean) Handles TXT_PERSONAL_RULE.ActionOnCheckedChange
            Mode_CheckedChanged()
        End Sub
        Private Sub OPT_RULES_LIST_CheckedChanged(sender As Object, e As EventArgs)
            Mode_CheckedChanged()
        End Sub
        Private Sub Mode_CheckedChanged()
            Dim e As Boolean = TXT_PERSONAL_RULE.Checked
            TXT_PERSONAL_RULE.Enabled(False) = e
            CONTAINER_LIST.Enabled = Not e
            CH_PROTECTED.Enabled = Not e
            CH_FORCE_UPDATE.Enabled = Not e
        End Sub
#End Region
#Region "List handlers"
        Private _LatestSelected As Integer = -1
        Private Sub LIST_RULES_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LIST_RULES.SelectedIndexChanged
            _LatestSelected = LIST_RULES.SelectedIndex
        End Sub
#End Region
    End Class
End Namespace