' Copyright (C) 2022  Andy
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
Imports DModes = SCrawler.DownloadObjects.AutoDownloader.Modes
Namespace DownloadObjects
    Friend Class AutoDownloaderEditorForm : Implements IOkCancelToolbar
        Private ReadOnly MyDefs As DefaultFormOptions
        Private ReadOnly MyGroups As List(Of String)
        Private ReadOnly Property Plan As AutoDownloader
        Friend Sub New(ByRef _Plan As AutoDownloader)
            InitializeComponent()
            Plan = _Plan
            MyDefs = New DefaultFormOptions
            MyGroups.ListAddList(Plan.Groups, LAP.NotContainsOnly)
        End Sub
        Private Class AutomationTimerChecker : Implements IFieldsCheckerProvider
            Private Property ErrorMessage As String = "The timer value must be greater than 0" Implements IFieldsCheckerProvider.ErrorMessage
            Private Property Name As String Implements IFieldsCheckerProvider.Name
            Private Property TypeError As Boolean Implements IFieldsCheckerProvider.TypeError
            Private Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                     Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object Implements ICustomProvider.Convert
                If CInt(AConvert(Of Integer)(Value, -10)) > 0 Then
                    Return Value
                Else
                    Return Nothing
                End If
            End Function
            Private Function GetFormat(ByVal FormatType As Type) As Object Implements IFormatProvider.GetFormat
                Throw New NotImplementedException()
            End Function
        End Class
        Private Sub AutoDownloaderEditorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize(Me, Settings.Design, True)
                .AddOkCancelToolbar()
                With Plan
                    Select Case .Mode
                        Case DModes.None : OPT_DISABLED.Checked = True
                        Case DModes.All : OPT_ALL.Checked = True
                        Case DModes.Default : OPT_DEFAULT.Checked = True
                        Case DModes.Specified : OPT_SPEC.Checked = True
                        Case DModes.Groups : OPT_GROUP.Checked = True
                    End Select
                    ChangeEnabled()
                    DEF_GROUP.Set(Plan)
                    If MyGroups.Count > 0 Then TXT_GROUPS.Text = MyGroups.ListToString
                    If Settings.Groups.Count = 0 Then TXT_GROUPS.Clear() : TXT_GROUPS.Enabled = False
                    CH_NOTIFY.Checked = .ShowNotifications
                    TXT_TIMER.Text = .Timer
                    NUM_DELAY.Value = .StartupDelay
                    LBL_LAST_TIME_UP.Text = .Information
                End With
                .MyFieldsChecker = New FieldsChecker
                With .MyFieldsCheckerE
                    .AddControl(Of String)(DEF_GROUP.TXT_NAME, DEF_GROUP.TXT_NAME.CaptionText,,
                                           New Groups.GroupEditorForm.NameChecker(Plan.Name, Settings.Automation, "Plan"))
                    .AddControl(Of Integer)(TXT_TIMER, TXT_TIMER.CaptionText,, New AutomationTimerChecker)
                    .EndLoaderOperations()
                End With
                .EndLoaderOperations()
            End With
        End Sub
        Private Sub AutoDownloaderEditorForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            MyGroups.Clear()
        End Sub
        Private Sub OK() Implements IOkCancelToolbar.OK
            If If(MyDefs.MyFieldsChecker?.AllParamsOK, True) Then
                With Plan
                    Select Case True
                        Case OPT_DISABLED.Checked : .Mode = DModes.None
                        Case OPT_ALL.Checked : .Mode = DModes.All
                        Case OPT_DEFAULT.Checked : .Mode = DModes.Default
                        Case OPT_SPEC.Checked : .Mode = DModes.Specified
                        Case OPT_GROUP.Checked : .Mode = DModes.Groups
                    End Select
                    DEF_GROUP.Get(Plan)
                    .Groups.Clear()
                    .Groups.ListAddList(MyGroups)
                    .Timer = AConvert(Of Integer)(TXT_TIMER.Text, AutoDownloader.DefaultTimer)
                    .StartupDelay = NUM_DELAY.Value
                    .Update()
                End With
                MyDefs.CloseForm()
            End If
        End Sub
        Private Sub Cancel() Implements IOkCancelToolbar.Cancel
            MyDefs.CloseForm(DialogResult.Cancel)
        End Sub
        Private Sub TXT_GROUPS_ActionOnButtonClick(ByVal Sender As ActionButton) Handles TXT_GROUPS.ActionOnButtonClick
            Select Case Sender.DefaultButton
                Case ActionButton.DefaultButtons.Edit
                    Using f As New LabelsForm(MyGroups, Settings.Groups.Select(Function(g) g.Name)) With {.Text = "Groups"}
                        f.ShowDialog()
                        If f.DialogResult = DialogResult.OK Then MyGroups.ListAddList(f.LabelsList, LAP.ClearBeforeAdd) : TXT_GROUPS.Text = MyGroups.ListToString
                    End Using
                Case ActionButton.DefaultButtons.Clear : MyGroups.Clear()
            End Select
        End Sub
        Private Sub OPT_DISABLED_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_DISABLED.CheckedChanged
            ChangeEnabled()
        End Sub
        Private Sub OPT_ALL_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_ALL.CheckedChanged
            ChangeEnabled()
        End Sub
        Private Sub OPT_DEFAULT_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_DEFAULT.CheckedChanged
            ChangeEnabled()
        End Sub
        Private Sub OPT_SPEC_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_SPEC.CheckedChanged
            ChangeEnabled()
        End Sub
        Private Sub OPT_GROUP_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_GROUP.CheckedChanged
            ChangeEnabled()
        End Sub
        Private Sub ChangeEnabled()
            DEF_GROUP.Enabled = OPT_SPEC.Checked
            TXT_GROUPS.Enabled = OPT_GROUP.Checked
            TXT_TIMER.Enabled = Not OPT_DISABLED.Checked
            NUM_DELAY.Enabled = Not OPT_DISABLED.Checked
            CH_NOTIFY.Enabled = Not OPT_DISABLED.Checked
        End Sub
        Private Sub NUM_DELAY_ActionOnButtonClick(ByVal Sender As ActionButton) Handles NUM_DELAY.ActionOnButtonClick
            If Sender.DefaultButton = ActionButton.DefaultButtons.Clear Then NUM_DELAY.Value = 0
        End Sub
    End Class
End Namespace