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
Imports DModes = SCrawler.DownloadObjects.AutoDownloader.Modes
Namespace DownloadObjects
    Friend Class AutoDownloaderEditorForm
        Private WithEvents MyDefs As DefaultFormOptions
        Private ReadOnly MyGroups As List(Of String)
        Private ReadOnly Property Plan As AutoDownloader
        Friend Sub New(ByRef _Plan As AutoDownloader)
            InitializeComponent()
            Plan = _Plan
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            MyGroups.ListAddList(Plan.Groups, LAP.NotContainsOnly)
        End Sub
        Private Class AutomationTimerChecker : Inherits FieldsCheckerProviderBase
            Public Overrides Property ErrorMessage As String
                Get
                    Return "The timer value must be greater than 0"
                End Get
                Set(ByVal msg As String)
                    MyBase.ErrorMessage = msg
                End Set
            End Property
            Public Overrides Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                              Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object
                If CInt(AConvert(Of Integer)(Value, -10)) > 0 Then
                    Return Value
                Else
                    HasError = True
                    Return Nothing
                End If
            End Function
        End Class
        Private Sub AutoDownloaderEditorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize(True)
                .AddOkCancelToolbar()
                With Plan
                    Select Case .Mode
                        Case DModes.None : OPT_DISABLED.Checked = True
                        Case DModes.All : OPT_ALL.Checked = True
                        Case DModes.Default : OPT_DEFAULT.Checked = True
                        Case DModes.Specified : OPT_SPEC.Checked = True
                        Case DModes.Groups : OPT_GROUP.Checked = True
                    End Select
                    DEF_GROUP.Set(Plan)
                    If MyGroups.Count > 0 Then TXT_GROUPS.Text = MyGroups.ListToString
                    If Settings.Groups.Count = 0 Then TXT_GROUPS.Clear() : TXT_GROUPS.Enabled = False
                    CH_NOTIFY.Checked = .ShowNotifications
                    CH_NOTIFY_SIMPLE.Checked = .ShowSimpleNotification
                    CH_SHOW_PIC.Checked = .ShowPictureDownloaded
                    CH_SHOW_PIC_USER.Checked = .ShowPictureUser
                    TXT_TIMER.Text = .Timer
                    NUM_DELAY.Value = .StartupDelay
                    LBL_LAST_TIME_UP.Text = .Information
                    ChangeEnabled()
                End With
                .MyFieldsChecker = New FieldsChecker
                With .MyFieldsCheckerE
                    .AddControl(Of String)(DEF_GROUP.TXT_NAME, DEF_GROUP.TXT_NAME.CaptionText,,
                                           New Groups.GroupEditorForm.NameChecker(Plan.Name, Settings.Automation, "Plan"))
                    .AddControl(Of Integer)(TXT_TIMER, TXT_TIMER.CaptionText,, New AutomationTimerChecker)
                    .EndLoaderOperations()
                End With
                .EndLoaderOperations()
                If DEF_GROUP.TXT_NAME.IsEmptyString And Settings.Automation.Count = 0 Then DEF_GROUP.TXT_NAME.Text = "Default"
            End With
        End Sub
        Private Sub AutoDownloaderEditorForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            MyGroups.Clear()
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            If MyDefs.MyFieldsChecker.AllParamsOK Then
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
                    .ShowNotifications = CH_NOTIFY.Checked
                    .ShowSimpleNotification = CH_NOTIFY_SIMPLE.Checked
                    .ShowPictureDownloaded = CH_SHOW_PIC.Checked
                    .ShowPictureUser = CH_SHOW_PIC_USER.Checked
                    .Timer = AConvert(Of Integer)(TXT_TIMER.Text, AutoDownloader.DefaultTimer)
                    .StartupDelay = NUM_DELAY.Value
                    .Update()
                End With
                MyDefs.CloseForm()
            End If
        End Sub
        Private Sub TXT_GROUPS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As EventArgs) Handles TXT_GROUPS.ActionOnButtonClick
            Select Case Sender.DefaultButton
                Case ActionButton.DefaultButtons.Edit
                    Using f As New LabelsForm(MyGroups, Settings.Groups.Select(Function(g) g.Name)) With {.Text = "Groups", .Icon = My.Resources.GroupByIcon_16}
                        f.ShowDialog()
                        If f.DialogResult = DialogResult.OK Then MyGroups.ListAddList(f.LabelsList, LAP.ClearBeforeAdd) : TXT_GROUPS.Text = MyGroups.ListToString
                    End Using
                Case ActionButton.DefaultButtons.Clear : MyGroups.Clear()
            End Select
        End Sub
        Private Sub ChangeEnabled() Handles OPT_DISABLED.CheckedChanged, OPT_ALL.CheckedChanged, OPT_DEFAULT.CheckedChanged,
                                            OPT_SPEC.CheckedChanged, OPT_GROUP.CheckedChanged,
                                            CH_NOTIFY.CheckedChanged, CH_NOTIFY_SIMPLE.CheckedChanged
            DEF_GROUP.Enabled(OPT_ALL.Checked Or OPT_DEFAULT.Checked Or OPT_SPEC.Checked, OPT_ALL.Checked) = OPT_SPEC.Checked
            TXT_GROUPS.Enabled = OPT_GROUP.Checked
            TXT_TIMER.Enabled = Not OPT_DISABLED.Checked
            NUM_DELAY.Enabled = Not OPT_DISABLED.Checked
            CH_NOTIFY.Enabled = Not OPT_DISABLED.Checked
            CH_NOTIFY_SIMPLE.Enabled = CH_NOTIFY.Enabled And CH_NOTIFY.Checked
            CH_SHOW_PIC.Enabled = CH_NOTIFY.Checked And Not OPT_DISABLED.Checked And Not CH_NOTIFY_SIMPLE.Checked
            CH_SHOW_PIC_USER.Enabled = CH_NOTIFY.Checked And Not OPT_DISABLED.Checked And Not CH_NOTIFY_SIMPLE.Checked
        End Sub
        Private Sub NUM_DELAY_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As EventArgs) Handles NUM_DELAY.ActionOnButtonClick
            If Sender.DefaultButton = ActionButton.DefaultButtons.Clear Then NUM_DELAY.Value = 0
        End Sub
    End Class
End Namespace