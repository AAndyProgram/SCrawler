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
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Namespace Editors
    Friend Class CollectionEditorForm
        Private WithEvents MyDefs As DefaultFormOptions
        Private ReadOnly Collections As List(Of String)
        Friend Property MyCollection As String = String.Empty
        Private _MyCollectionSpecialPath As SFile = Nothing
        Friend ReadOnly Property MyCollectionSpecialPath As SFile
            Get
                Return _MyCollectionSpecialPath
            End Get
        End Property
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            Collections = New List(Of String)
            Icon = PersonalUtilities.Tools.ImageRenderer.GetIcon(My.Resources.DBPic_32, EDP.ReturnValue)
            If Not Icon Is Nothing Then ShowIcon = True
        End Sub
        Private Sub CollectionEditorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDefs
                    .MyViewInitialize()
                    .AddOkCancelToolbar()
                    Collections.ListAddList(Settings.LastCollections)
                    Dim ecol As List(Of String) = ListAddList(Nothing, (From c In Settings.Users Where c.IsCollection Select c.CollectionName), LAP.NotContainsOnly)
                    If ecol.ListExists Then ecol.Sort() : Collections.ListAddList(ecol, LAP.NotContainsOnly) : ecol.Clear()
                    If Collections.Count > 0 Then CMB_COLLECTIONS.Items.AddRange(Collections.Select(Function(c) New ListItem(c)))
                    If Not MyCollection.IsEmptyString And Collections.Contains(MyCollection) Then CMB_COLLECTIONS.SelectedIndex = Collections.IndexOf(MyCollection)
                    .DelegateClosingChecker = False
                    .EndLoaderOperations()
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub CollectionEditorForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            Collections.Clear()
        End Sub
        Private Sub CollectionEditorForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            If e.KeyCode = Keys.Insert Or (e.KeyCode = Keys.O And e.Control) Then AddNewCollection(Not e.KeyCode = Keys.Insert) : e.Handled = True
        End Sub
        Private Sub MyDefs_ButtonOkClick() Handles MyDefs.ButtonOkClick
            If CMB_COLLECTIONS.SelectedIndex >= 0 Then
                MyCollection = CMB_COLLECTIONS.Value.ToString
                With Settings.LastCollections
                    If .Contains(MyCollection) Then .Remove(MyCollection)
                    If .Count = 0 Then .Add(MyCollection) Else .Insert(0, MyCollection)
                End With
                MyDefs.CloseForm()
            Else
                MsgBoxE("Collection not selected", MsgBoxStyle.Exclamation)
            End If
        End Sub
        Private Sub CMB_COLLECTIONS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles CMB_COLLECTIONS.ActionOnButtonClick
            If e.DefaultButton = ADB.Add Or e.DefaultButton = ADB.Open Then AddNewCollection(e.DefaultButton = ADB.Open)
        End Sub
        Private Sub CMB_COLLECTIONS_ActionOnListDoubleClick(ByVal Sender As Object, ByVal e As EventArgs, ByVal Item As ListViewItem) Handles CMB_COLLECTIONS.ActionOnListDoubleClick
            Item.Selected = True
            MyDefs_ButtonOkClick()
        End Sub
        Private Sub AddNewCollection(ByVal OpenMode As Boolean)
            Dim c$ = String.Empty
            If OpenMode Then
                Using f As New GlobalLocationsChooserForm With {.MyIsCollectionSelector = True}
                    f.ShowDialog()
                    If f.DialogResult = DialogResult.OK Then
                        c = f.MyCollectionName
                        _MyCollectionSpecialPath = $"{f.MyDestination.Path.CSFilePS}{c}\"
                    End If
                End Using
            Else
                _MyCollectionSpecialPath = Nothing
                c = InputBoxE("Enter new collection name:", "Collection name")
            End If
            If Not c.IsEmptyString Then
                If Not Collections.Contains(c) Then
                    Collections.Add(c)
                    CMB_COLLECTIONS.Items.Add(c)
                    CMB_COLLECTIONS.SelectedIndex = CMB_COLLECTIONS.Count - 1
                Else
                    Dim i% = Collections.IndexOf(c)
                    If i >= 0 Then
                        CMB_COLLECTIONS.SelectedIndex = i
                        _MyCollectionSpecialPath = Settings.UsersList.FirstOrDefault(Function(u) u.CollectionName = c).SpecialCollectionPath
                        MsgBoxE({$"The '{c}' collection already exists", "Add a new collection"}, vbExclamation)
                    End If
                End If
            End If
        End Sub
    End Class
End Namespace