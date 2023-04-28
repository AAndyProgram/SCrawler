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
Namespace Editors
    Friend Class SiteSelectionForm
        Private WithEvents MyDefs As DefaultFormOptions
        Friend ReadOnly Property SelectedSites As List(Of String)
        Friend Sub New(ByVal s As List(Of String))
            InitializeComponent()
            SelectedSites.ListAddList(s)
            If SelectedSites Is Nothing Then SelectedSites = New List(Of String)
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
        End Sub
        Private Sub SiteSelectionForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDefs
                    .MyViewInitialize(True)
                    .AddOkCancelToolbar()
                    CMB_SITES.BeginUpdate()
                    Dim sl As List(Of String) = Settings.Plugins.Select(Function(p) p.Name).ToList
                    CMB_SITES.Items.AddRange(sl.Select(Function(s) New ListItem(s)))
                    Dim l As New List(Of Integer)
                    If SelectedSites.Count > 0 Then sl.ForEach(Sub(s) If SelectedSites.Contains(s) Then l.Add(sl.IndexOf(s)))
                    sl.Clear()
                    CMB_SITES.EndUpdate()
                    If l.Count > 0 Then CMB_SITES.ListCheckedIndexes = l : l.Clear()
                    .DelegateClosingChecker = False
                    .EndLoaderOperations()
                    .MyOkCancel.EnableOK = True
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub SiteSelectionForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            SelectedSites.Clear()
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            Try
                SelectedSites.ListAddList(CMB_SITES.Items.CheckedItems.Select(Function(i) CStr(i.Value(0))), LAP.ClearBeforeAdd)
                MyDefs.CloseForm()
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex)
            End Try
        End Sub
    End Class
End Namespace