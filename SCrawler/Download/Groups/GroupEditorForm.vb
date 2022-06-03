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
Namespace DownloadObjects.Groups
    Friend Class GroupEditorForm : Implements IOkCancelToolbar
        Private ReadOnly MyDefs As DefaultFormProps
        Friend Property MyGroup As DownloadGroup
        Friend Sub New(ByRef g As DownloadGroup)
            InitializeComponent()
            MyGroup = g
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
                        DEFS_GROUP.Set(MyGroup)
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
        Private Sub ToolbarBttOK() Implements IOkCancelToolbar.ToolbarBttOK
            If MyDefs.MyFieldsChecker.AllParamsOK Then
                If MyGroup Is Nothing Then MyGroup = New DownloadGroup
                With MyGroup
                    .NameBefore = .Name
                    .Name = TXT_NAME.Text
                    DEFS_GROUP.Get(MyGroup)
                End With
                MyDefs.CloseForm()
            End If
        End Sub
        Private Sub ToolbarBttCancel() Implements IOkCancelToolbar.ToolbarBttCancel
            MyDefs.CloseForm(DialogResult.Cancel)
        End Sub
    End Class
End Namespace