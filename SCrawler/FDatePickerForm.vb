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
Friend Class FDatePickerForm : Implements IOkCancelDeleteToolbar
    Private MyDefs As DefaultFormProps
    Friend ReadOnly Property SelectedDate As Date?
        Get
            If DT.Checked Then Return DT.Value.Date Else Return Nothing
        End Get
    End Property
    Private ReadOnly _InitialValue As Date?
    Friend Sub New(ByVal d As Date?)
        InitializeComponent()
        _InitialValue = d
    End Sub
    Private Sub FDatePickerForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            MyDefs = New DefaultFormProps
            With MyDefs
                .MyViewInitialize(Me, Settings.Design, True)
                .AddOkCancelToolbar()
                If _InitialValue.HasValue Then
                    DT.Checked = True
                    DT.Value = _InitialValue.Value.Date
                Else
                    DT.Checked = False
                End If
                .DelegateClosingChecker = False
                .EndLoaderOperations()
                MyDefs.MyOkCancel.EnableOK = True
            End With
        Catch ex As Exception
            MyDefs.InvokeLoaderError(ex)
        End Try
    End Sub
    Private Sub OK() Implements IOkCancelToolbar.OK
        MyDefs.CloseForm()
    End Sub
    Private Sub Cancel() Implements IOkCancelToolbar.Cancel
        MyDefs.CloseForm(DialogResult.Cancel)
    End Sub
    Private Sub Delete() Implements IOkCancelDeleteToolbar.Delete
        MyDefs.CloseForm(DialogResult.Abort)
    End Sub
End Class