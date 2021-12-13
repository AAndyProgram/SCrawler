' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Forms.Toolbars
Namespace Editors
    Friend Class CollectionEditorForm : Implements IOkCancelToolbar
        Private ReadOnly MyDefs As DefaultFormProps
        Private ReadOnly Collections As List(Of String)
        Friend Property [Collection] As String = String.Empty
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormProps
            Collections = New List(Of String)
        End Sub
        Friend Sub New(ByVal CollectionName As String)
            Me.New
            Collection = CollectionName
        End Sub
        Private Sub CollectionEditorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDefs
                    .MyViewInitialize(Me, Settings.Design)
                    .MyOkCancel = New OkCancelToolbar(Me, Me, CONTAINER_MAIN.BottomToolStripPanel)
                    .MyOkCancel.AddThisToolbar()
                    Collections.ListAddList((From c In Settings.Users Where c.IsCollection Select c.CollectionName), LAP.NotContainsOnly, EDP.ThrowException)
                    If Collections.ListExists Then CMB_COLLECTIONS.Items.AddRange(From c In Collections Select New Controls.Base.ListItem(c))
                    If Not Collection.IsEmptyString And Collections.Contains(Collection) Then CMB_COLLECTIONS.SelectedIndex = Collections.IndexOf(Collection)
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub CollectionEditorForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            If Not BeforeCloseChecker(MyDefs.ChangesDetected) Then
                e.Cancel = True
            Else
                Collections.Clear()
                MyDefs.Dispose()
            End If
        End Sub
        Private Sub CollectionEditorForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            If e.KeyCode = Keys.Insert Then
                AddNewCollection()
                e.Handled = True
            Else
                e.Handled = False
            End If
        End Sub
        Private Sub ToolbarBttOK() Implements IOkCancelToolbar.ToolbarBttOK
            If CMB_COLLECTIONS.SelectedIndex >= 0 Then
                Collection = CMB_COLLECTIONS.Value.ToString
                MyDefs.ChangesDetected = False
                DialogResult = DialogResult.OK
                Close()
            Else
                MsgBoxE("Collection does not selected", MsgBoxStyle.Exclamation)
            End If
        End Sub
        Private Sub ToolbarBttCancel() Implements IOkCancelToolbar.ToolbarBttCancel
            MyDefs.ChangesDetected = False
            DialogResult = DialogResult.Cancel
            Close()
        End Sub
        Private Sub CMB_COLLECTIONS_ActionOnButtonClick(ByVal Sender As ActionButton) Handles CMB_COLLECTIONS.ActionOnButtonClick
            If Sender.DefaultButton = ActionButton.DefaultButtons.Add Then AddNewCollection()
        End Sub
        Private Sub AddNewCollection()
            Dim c$ = InputBoxE("Enter new collection name:", "Collection name")
            If Not c.IsEmptyString Then
                If Not Collections.Contains(c) Then
                    Collections.Add(c)
                    CMB_COLLECTIONS.Items.Add(c)
                    CMB_COLLECTIONS.SelectedIndex = CMB_COLLECTIONS.Count - 1
                Else
                    Dim i% = Collections.IndexOf(c)
                    If i >= 0 Then CMB_COLLECTIONS.SelectedIndex = i
                End If
            End If
        End Sub
    End Class
End Namespace