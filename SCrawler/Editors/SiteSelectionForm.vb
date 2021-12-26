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
                CMB_SITES.Items.AddRange({New ListItem({Sites.Reddit.ToString, CInt(Sites.Reddit)}),
                                          New ListItem({Sites.Twitter.ToString, CInt(Sites.Twitter)}),
                                          New ListItem({Sites.Instagram.ToString, CInt(Sites.Instagram)})})
                Dim l As New List(Of Integer)
                If SelectedSites.Count > 0 Then
                    If SelectedSites.Contains(Sites.Reddit) Then l.Add(0)
                    If SelectedSites.Contains(Sites.Twitter) Then l.Add(1)
                    If SelectedSites.Contains(Sites.Instagram) Then l.Add(2)
                End If
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
                SelectedSites.Clear()
                Dim l As List(Of Integer) = CMB_SITES.ListCheckedIndexes
                If l.ListExists Then
                    For Each i% In l
                        Select Case i
                            Case 0 : SelectedSites.Add(Sites.Reddit)
                            Case 1 : SelectedSites.Add(Sites.Twitter)
                            Case 2 : SelectedSites.Add(Sites.Instagram)
                        End Select
                    Next
                End If
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