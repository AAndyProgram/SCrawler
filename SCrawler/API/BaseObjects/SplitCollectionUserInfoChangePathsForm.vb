' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Functions.Messaging
Namespace API.Base
    Friend Class SplitCollectionUserInfoChangePathsForm
        Private WithEvents MyDefs As DefaultFormOptions
        Friend ReadOnly Property Users As List(Of SplitCollectionUserInfo)
        ''' <summary>
        ''' Cancel = use initial<br/>
        ''' Abort = abort operation<br/>
        ''' OK = use changes
        ''' </summary>
        Friend Sub New(ByVal _Users As IEnumerable(Of SplitCollectionUserInfo))
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            Users = New List(Of SplitCollectionUserInfo)(_Users)
        End Sub
        Private Sub SplitCollectionUserInfoChangePathsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize()
                .AddOkCancelToolbar()
                LIST_USERS.Items.AddRange(Users.Cast(Of Object).ToArray)
                .EndLoaderOperations()
                .MyOkCancel.EnableOK = True
            End With
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            MyDefs.CloseForm()
        End Sub
        Private Sub MyDefs_ButtonCancelClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonCancelClick
            Dim m As New MMessage("You have canceled the change. Do you want to process user(s) as is or cancel the operation?", "Change user paths",
                                  {New MsgBoxButton("Initial", "Process users as is (IGNORE changes to this form)") With {.CallBackObject = DialogResult.Cancel},
                                   New MsgBoxButton("Process", "Process users as is (INCLUDE changes here)") With {.CallBackObject = DialogResult.OK},
                                   New MsgBoxButton("Abort", "Abort operation") With {.CallBackObject = DialogResult.Abort},
                                   New MsgBoxButton("Cancel", "Continue editing here") With {.CallBackObject = DialogResult.Retry}},
                                  vbExclamation) With {.ButtonsPerRow = 4}
            Dim result As DialogResult = CInt(MsgBoxE(m).Button.CallBackObject)
            If result = DialogResult.Retry Then
                e.Handled = True
                Exit Sub
            Else
                MyDefs.CloseForm(result)
            End If
        End Sub
        Private Sub SplitCollectionUserInfoChangePathsForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            Users.Clear()
        End Sub
        Private Sub LIST_USERS_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles LIST_USERS.MouseDoubleClick
            Try
                With LIST_USERS
                    If .SelectedIndex >= 0 Then
                        Dim obj As SplitCollectionUserInfo = .Items(.SelectedIndex)
                        Using f As New SplitCollectionUserInfoPathForm(obj)
                            f.ShowDialog()
                            If f.DialogResult = DialogResult.OK Then
                                obj = f.User
                                If obj.Changed Then
                                    Users(.SelectedIndex) = obj
                                    .Items(.SelectedIndex) = obj
                                    .Refresh()
                                End If
                            End If
                        End Using
                    End If
                End With
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Change user paths")
            End Try
        End Sub
    End Class
End Namespace