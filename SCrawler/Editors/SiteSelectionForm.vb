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
Imports PersonalUtilities.Forms.Controls.Base
Namespace Editors
    Friend Class SiteSelectionForm : Implements IOkCancelToolbar
        Private ReadOnly MyDefs As DefaultFormProps
        Friend ReadOnly Property SelectedSites As List(Of Sites)
        Friend Sub New(ByVal s As List(Of Sites))
            InitializeComponent()
            SelectedSites.ListAddList(s)
            If SelectedSites Is Nothing Then SelectedSites = New List(Of Sites)
            MyDefs = New DefaultFormProps
        End Sub
        Private Sub SiteSelectionForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize(Me, Settings.Design, True)
                .DelegateClosingChecker()
                .AddOkCancelToolbar()
                CMB_SITES.BeginUpdate()
                Dim sl As List(Of Sites) = ListAddList(Of Sites)(Nothing, [Enum].GetValues(GetType(Sites))).ListWithRemove(Sites.Undefined)
                CMB_SITES.Items.AddRange(sl.Select(Function(s) New ListItem({s.ToString, CInt(s)})))
                Dim l As New List(Of Integer)
                If SelectedSites.Count > 0 Then sl.ForEach(Sub(s) If SelectedSites.Contains(s) Then l.Add(sl.IndexOf(s)))
                sl.Clear()
                CMB_SITES.EndUpdate()
                If l.Count > 0 Then CMB_SITES.ListCheckedIndexes = l : l.Clear()
                .EndLoaderOperations()
                .MyOkCancel.EnableOK = True
            End With
        End Sub
        Private Sub SiteSelectionForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            SelectedSites.Clear()
        End Sub
        Public Sub ToolbarBttOK() Implements IOkCancelToolbar.ToolbarBttOK
            Try
                SelectedSites.ListAddList(CMB_SITES.Items.CheckedItems.Select(Function(i) DirectCast(i.Value(1), Sites)), LAP.ClearBeforeAdd)
                MyDefs.CloseForm()
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex)
            End Try
        End Sub
        Public Sub ToolbarBttCancel() Implements IOkCancelToolbar.ToolbarBttCancel
            MyDefs.CloseForm(DialogResult.Cancel)
        End Sub
    End Class
End Namespace