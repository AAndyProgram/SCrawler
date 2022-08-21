' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Friend Class FDatePickerForm
    Private ReadOnly MyDefs As DefaultFormOptions
    Friend ReadOnly Property SelectedDate As Date?
        Get
            If DT.Checked Then Return DT.Value.Date Else Return Nothing
        End Get
    End Property
    Private ReadOnly _InitialValue As Date?
    Friend Sub New(ByVal d As Date?)
        InitializeComponent()
        _InitialValue = d
        MyDefs = New DefaultFormOptions(Me, Settings.Design)
    End Sub
    Private Sub FDatePickerForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        With MyDefs
            .MyViewInitialize(True)
            .AddOkCancelToolbar(True)
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
    End Sub
End Class