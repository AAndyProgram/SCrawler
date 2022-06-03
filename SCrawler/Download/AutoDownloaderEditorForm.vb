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
        Private ReadOnly MyDefs As DefaultFormProps
        Private ReadOnly MyGroups As List(Of String)
        Friend Property IsControlForm As Boolean = False
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormProps
            MyGroups.ListAddList(Settings.Automation.Groups, LAP.NotContainsOnly)
        End Sub
        Friend Class AutomationTimerChecker : Implements IFieldsCheckerProvider
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
        Private _Loaded As Boolean = False
        Friend Shadows Sub Show()
            MyBase.Show()
            If Not _Loaded And IsControlForm Then AutoDownloaderEditorForm_Load(Nothing, EventArgs.Empty)
        End Sub
        Private Sub AutoDownloaderEditorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                If Not IsControlForm Then
                    .MyViewInitialize(Me, Settings.Design, True)
                    .AddOkCancelToolbar()
                End If
                With Settings.Automation
                    Select Case .Mode
                        Case DModes.None : OPT_DISABLED.Checked = True
                        Case DModes.All : OPT_ALL.Checked = True
                        Case DModes.Default : OPT_DEFAULT.Checked = True
                        Case DModes.Specified : OPT_SPEC.Checked = True
                        Case DModes.Groups : OPT_GROUP.Checked = True
                    End Select
                    ChangeEnabled()
                    DEF_GROUP.Set(Settings.Automation)
                    If MyGroups.Count > 0 Then TXT_GROUPS.Text = MyGroups.ListToString
                    If Settings.Groups.Count = 0 Then TXT_GROUPS.Clear() : TXT_GROUPS.Enabled = False
                    CH_NOTIFY.Checked = .ShowNotifications
                    TXT_TIMER.Text = .Timer
                    LBL_LAST_TIME_UP.Text &= .LastDownloadDate.ToStringDate(ADateTime.Formats.BaseDateTime)
                End With
                If Not IsControlForm Then
                    .MyFieldsChecker = New FieldsChecker
                    With DirectCast(.MyFieldsChecker, FieldsChecker)
                        .AddControl(Of Integer)(TXT_TIMER, TXT_TIMER.CaptionText,, New AutomationTimerChecker)
                        .EndLoaderOperations()
                    End With
                    .DelegateClosingChecker()
                    .AppendDetectors()
                    .EndLoaderOperations()
                End If
            End With
            _Loaded = True
        End Sub
        Private Sub AutoDownloaderEditorForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            MyGroups.Clear()
        End Sub
        Friend Sub SaveSetiings() Implements IOkCancelToolbar.ToolbarBttOK
            If If(MyDefs.MyFieldsChecker?.AllParamsOK, True) Then
                With Settings.Automation
                    Select Case True
                        Case OPT_DISABLED.Checked : .Mode = DModes.None
                        Case OPT_ALL.Checked : .Mode = DModes.All
                        Case OPT_DEFAULT.Checked : .Mode = DModes.Default
                        Case OPT_SPEC.Checked : .Mode = DModes.Specified
                        Case OPT_GROUP.Checked : .Mode = DModes.Groups
                    End Select
                    DEF_GROUP.Get(Settings.Automation)
                    .Groups.Clear()
                    .Groups.ListAddList(MyGroups)
                    .Timer = AConvert(Of Integer)(TXT_TIMER.Text, AutoDownloader.DefaultTimer)
                    .Update()
                End With
                If Not IsControlForm Then MyDefs.CloseForm()
            End If
        End Sub
        Private Sub ToolbarBttCancel() Implements IOkCancelToolbar.ToolbarBttCancel
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
            CH_NOTIFY.Enabled = Not OPT_DISABLED.Checked
        End Sub
    End Class
End Namespace