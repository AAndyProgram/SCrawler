' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Namespace DownloadObjects.Groups
    Friend Class GroupEditorForm
        Private WithEvents MyDefs As DefaultFormOptions
        Friend Property MyGroup As DownloadGroup
        Friend Property DownloadMode As Boolean = False
        Friend Property FilterMode As Boolean = False
        Friend Property IsTemporaryGroup As Boolean = False
        Friend Property IsClone As Boolean
        Friend Sub New(ByRef g As DownloadGroup)
            InitializeComponent()
            MyGroup = g
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
        End Sub
        Friend Class NameChecker : Inherits FieldsCheckerProviderBase
            Private ReadOnly ExistingGroupName As String
            Private ReadOnly Property Source As IEnumerable(Of IGroup)
            Private ReadOnly ParamName As String
            Friend Sub New(ByVal _ExistingGroupName As String, ByRef _Source As IEnumerable(Of IGroup), ByVal Param As String)
                ExistingGroupName = _ExistingGroupName
                Source = _Source
                ParamName = Param
            End Sub
            Public Overrides Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                              Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object
                If Not ACheck(Value) Then
                    ErrorMessage = $"{ParamName} name cannot be empty"
                    HasError = True
                ElseIf Not ExistingGroupName.IsEmptyString AndAlso CStr(Value) = ExistingGroupName Then
                    Return Value
                ElseIf Source.Count > 0 AndAlso Source.LongCount(Function(g) g.Name = CStr(Value)) > 0 Then
                    ErrorMessage = $"A {ParamName.ToLower} with the same name already exists"
                    HasError = True
                Else
                    Return Value
                End If
                Return Nothing
            End Function
        End Class
        Private Sub GroupEditorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize(True)
                .AddOkCancelToolbar()
                If Not MyGroup Is Nothing Then
                    With MyGroup
                        DEFS_GROUP.Set(MyGroup)
                        Text &= $" { .Name}"
                    End With
                ElseIf DownloadMode Then
                    Text = "Download options"
                ElseIf FilterMode Then
                    Text = "Filter options"
                Else
                    Text = "New Group"
                End If
                .MyFieldsChecker = New FieldsChecker
                If DownloadMode Or FilterMode Or IsTemporaryGroup Then
                    DEFS_GROUP.HideName()
                    Dim s As Size = Size
                    s.Height -= 31
                    MaximumSize = Nothing
                    MinimumSize = Nothing
                    Size = s
                    MinimumSize = s
                    MaximumSize = s
                Else
                    .MyFieldsCheckerE.AddControl(Of String)(DEFS_GROUP.TXT_NAME, DEFS_GROUP.TXT_NAME.CaptionText,,
                                                            New NameChecker(If(Not IsClone, If(MyGroup?.Name, String.Empty), String.Empty), Settings.Groups, "Group"))
                End If
                .MyFieldsChecker.EndLoaderOperations()
                .EndLoaderOperations()
                .MyOkCancel.EnableOK = True
            End With
        End Sub
        Private Sub GroupEditorForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            Try
                If e = ShowUsersButtonKey Then
                    Using g As New GroupParameters
                        DEFS_GROUP.Get(g)
                        GroupUsersViewer.Show(DownloadGroup.GetUsers(g), $"{IIf(FilterMode, "F", "G")} {g.Name}")
                    End Using
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Show plan users")
            End Try
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            If MyDefs.MyFieldsChecker.AllParamsOK Then
                If MyGroup Is Nothing Then MyGroup = New DownloadGroup(Not FilterMode)
                With MyGroup
                    .NameBefore = .Name
                    DEFS_GROUP.Get(MyGroup)
                End With
                MyDefs.CloseForm()
            End If
        End Sub
    End Class
End Namespace