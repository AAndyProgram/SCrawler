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
Namespace DownloadObjects.Groups
    Friend Class GroupEditorForm : Implements IOkCancelToolbar
        Private ReadOnly MyDefs As DefaultFormProps
        Friend Property MyGroup As DownloadGroup
        Private ReadOnly MyLabels As List(Of String)
        Friend Sub New(ByRef g As DownloadGroup)
            InitializeComponent()
            MyGroup = g
            MyLabels = New List(Of String)
            If Not MyGroup Is Nothing Then MyLabels.ListAddList(MyGroup.Labels)
            MyDefs = New DefaultFormProps
        End Sub
        Private Class NameChecker : Implements IFieldsCheckerProvider
            Private Property ErrorMessage As String Implements IFieldsCheckerProvider.ErrorMessage
            Private Property Name As String Implements IFieldsCheckerProvider.Name
            Private Property TypeError As Boolean Implements IFieldsCheckerProvider.TypeError
            Private ReadOnly ExistingGroupName As String
            Friend Sub New(ByVal _ExistingGroupName As String)
                ExistingGroupName = _ExistingGroupName
            End Sub
            Private Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                     Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object Implements ICustomProvider.Convert
                If Not ACheck(Value) Then
                    ErrorMessage = "Group name cannot be empty"
                ElseIf Not ExistingGroupName.IsEmptyString AndAlso CStr(Value) = ExistingGroupName Then
                    Return Value
                ElseIf Settings.Groups.Count > 0 AndAlso Settings.Groups.LongCount(Function(g) g.Name = CStr(Value)) > 0 Then
                    ErrorMessage = "A group with the same name already exists"
                Else
                    Return Value
                End If
                Return Nothing
            End Function
            Private Function GetFormat(ByVal FormatType As Type) As Object Implements IFormatProvider.GetFormat
                Throw New NotImplementedException("GetFormat is not available in this context")
            End Function
        End Class
        Private Sub GroupEditorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize(Me, Settings.Design, True)
                .AddOkCancelToolbar()
                .DelegateClosingChecker()
                If Not MyGroup Is Nothing Then
                    With MyGroup
                        TXT_NAME.Text = .Name
                        CH_TEMPORARY.CheckState = .Temporary
                        CH_FAV.CheckState = .Favorite
                        CH_READY_FOR_DOWN.Checked = .ReadyForDownload
                        CH_READY_FOR_DOWN_IGNORE.Checked = .ReadyForDownloadIgnore
                        TXT_LABELS.Text = MyLabels.ListToString
                        Text &= $" { .Name}"
                    End With
                Else
                    Text = "New Group"
                End If
                .MyFieldsChecker = New FieldsChecker
                DirectCast(.MyFieldsChecker, FieldsChecker).AddControl(Of String)(TXT_NAME, TXT_NAME.CaptionText,, New NameChecker(If(MyGroup?.Name, String.Empty)))
                .MyFieldsChecker.EndLoaderOperations()
                .AppendDetectors()
                .EndLoaderOperations()
            End With
        End Sub
        Private Sub GroupEditorForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            MyLabels.Clear()
        End Sub
        Private Sub ToolbarBttOK() Implements IOkCancelToolbar.ToolbarBttOK
            If MyDefs.MyFieldsChecker.AllParamsOK Then
                If MyGroup Is Nothing Then MyGroup = New DownloadGroup
                With MyGroup
                    .Name = TXT_NAME.Text
                    .Temporary = CH_TEMPORARY.CheckState
                    .Favorite = CH_FAV.CheckState
                    .ReadyForDownload = CH_READY_FOR_DOWN.Checked
                    .ReadyForDownloadIgnore = CH_READY_FOR_DOWN_IGNORE.Checked
                    .Labels.ListAddList(MyLabels, LAP.ClearBeforeAdd, LAP.NotContainsOnly)
                End With
                MyDefs.CloseForm()
            End If
        End Sub
        Private Sub ToolbarBttCancel() Implements IOkCancelToolbar.ToolbarBttCancel
            MyDefs.CloseForm(DialogResult.Cancel)
        End Sub
        Private Sub TXT_LABELS_ActionOnButtonClick(ByVal Sender As ActionButton) Handles TXT_LABELS.ActionOnButtonClick
            Select Case Sender.DefaultButton
                Case ActionButton.DefaultButtons.Edit
                    Using f As New LabelsForm(MyLabels)
                        f.ShowDialog()
                        If f.DialogResult = DialogResult.OK Then
                            MyLabels.ListAddList(f.LabelsList, LAP.NotContainsOnly, LAP.ClearBeforeAdd)
                            TXT_LABELS.Clear()
                            TXT_LABELS.Text = MyLabels.ListToString
                        End If
                    End Using
                Case ActionButton.DefaultButtons.Clear
                    MyLabels.Clear()
                    TXT_LABELS.Clear()
            End Select
        End Sub
    End Class
End Namespace