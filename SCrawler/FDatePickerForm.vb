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
    Friend ReadOnly Property DateFrom As Date?
        Get
            If DT_FROM.Checked Then Return CDate(DT_FROM.Value).Date Else Return Nothing
        End Get
    End Property
    Friend ReadOnly Property DateTo As Date?
        Get
            If DT_TO.Checked Then Return CDate(DT_TO.Value).Date Else Return Nothing
        End Get
    End Property
    Friend Sub New(ByVal DateFrom As Date?, ByVal DateTo As Date?)
        InitializeComponent()
        MyDefs = New DefaultFormOptions(Me, Settings.Design)
        If DateFrom.HasValue Then DT_FROM.Value = DateFrom.Value
        If DateTo.HasValue Then DT_TO.Value = DateTo.Value
        DT_FROM.Checked = DateFrom.HasValue
        DT_TO.Checked = DateTo.HasValue
    End Sub
    Private Sub FDatePickerForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        With MyDefs
            .MyViewInitialize(True)
            .AddOkCancelToolbar(True)
            .DelegateClosingChecker = False
            .EndLoaderOperations()
            MyDefs.MyOkCancel.EnableOK = True
        End With
    End Sub
End Class