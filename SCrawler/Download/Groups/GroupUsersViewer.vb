' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports PersonalUtilities.Forms
Namespace DownloadObjects.Groups
    Friend Class GroupUsersViewer : Inherits SimpleListForm(Of IUserData)
        Friend Sub New(ByVal Users As IEnumerable(Of IUserData), ByVal AdditText As String)
            MyBase.New(Users, Settings.Design)
            DesignXMLNodeName = "GroupUsersViewer"
            Provider = New CustomProvider(Function(u As UserDataBase) u.ToStringForLog)
            FormText = $"Users ({If(Users?.Count, 0)})"
            If Not AdditText.IsEmptyString Then
                Dim a$ = AdditText.Trim.Take(100).ListToStringE(String.Empty,,, " ", EDP.ReturnValue)
                If Not a = AdditText Then AdditText &= "..."
                FormText &= $" [{a}]"
            End If
            Icon = My.Resources.UsersIcon_32
            MyDefs.DelegateClosingChecker = False
            Mode = SimpleListFormModes.SelectedItems
            MultiSelect = False
            Size = New Size(300, 400)
        End Sub
        Protected Overrides Sub MyForm_KeyDown(sender As Object, e As KeyEventArgs)
            Try
                Dim b As Boolean = True
                If e.KeyCode = Keys.F And e.Control Then
                    FocusUser()
                ElseIf e.KeyCode = Keys.F3 Then
                    EditUser(e.Alt)
                ElseIf e = ShowUsersButtonKey Then
                    MsgBoxE(New MMessage(DataSourceCollection.ListToStringE(vbCr, Provider,,, EDP.LogMessageValue), "User list") With {.Editable = True})
                ElseIf e.KeyCode = Keys.F1 And Not e.Alt And Not e.Control Then
                    MsgBoxE({$"Hotkeys:{vbCr}Alt+F1 - show user list{vbCr}Ctrl+F - find the selected user in the main window{vbCr}" &
                             $"F3 - edit selected user{vbCr}Alt+F3 - edit selected collection", "Hotkeys"})
                Else
                    b = False
                End If
                If b Then e.Handled = True
            Catch
            End Try
        End Sub
        Friend Overloads Shared Sub Show(ByVal Users As IEnumerable(Of IUserData), ByVal AdditText As String)
            If Users.ListExists Then
                MainFrameObj.OpenedGroupUsersForms.Add(New GroupUsersViewer(Users, AdditText))
                MainFrameObj.OpenedGroupUsersForms.Last.Show()
            Else
                MsgBoxE({"No users were found based on the selected parameters", "Show group users"}, vbExclamation)
            End If
        End Sub
        Protected Overrides Sub CMB_DATA_ActionOnListDoubleClick(ByVal Sender As Object, ByVal e As EventArgs, ByVal Item As ListViewItem)
            FocusUser()
        End Sub
        Private Sub FocusUser()
            Try
                If _LatestSelected.ValueBetween(0, DataSourceCollection.Count - 1) Then MainFrameObj.FocusUser(DataSourceCollection(_LatestSelected).Key, True)
            Catch
            End Try
        End Sub
        Private Sub EditUser(ByVal EditCollection As Boolean)
            Try
                If _LatestSelected.ValueBetween(0, DataSourceCollection.Count - 1) Then
                    Dim u As IUserData = Settings.GetUser(DataSourceCollection(_LatestSelected).Key, EditCollection)
                    If Not u Is Nothing Then ControlInvokeFast(MainFrameObj.MF, Sub() MainFrameObj.MF.EditSelectedUser(u), EDP.None)
                End If
            Catch
            End Try
        End Sub
        Friend Overloads Sub Show()
            MyForm.Show()
        End Sub
    End Class
End Namespace