' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Namespace Editors
    Friend Class ColorPickerInternalForm
        Private WithEvents MyDefs As DefaultFormOptions
        Friend Property ResultColor As DataColor
        Private Class ColorNameProvider : Inherits FieldsCheckerProviderBase
            Private ReadOnly ExistNames As List(Of String)
            Private ReadOnly CurrentName As String
            Private LastValue As String
            Public Overrides Property ErrorMessage As String
                Get
                    If HasError Then
                        Return $"A color named '{LastValue}' already exists"
                    Else
                        Return String.Empty
                    End If
                End Get
                Set : End Set
            End Property
            Friend Sub New(ByVal _CurrentName As String)
                ExistNames = New List(Of String)
                CurrentName = _CurrentName.StringToLower
                If Settings.Colors.Count > 0 Then ExistNames.ListAddList(Settings.Colors.Select(Function(c) c.Name.StringToLower), LAP.NotContainsOnly)
            End Sub
            Public Overrides Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                              Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object
                If ACheck(Value) Then
                    LastValue = Value
                    Dim v$ = CStr(Value).StringToLower
                    If v = CurrentName OrElse (ExistNames.Count = 0 OrElse Not ExistNames.Contains(v)) Then
                        Return Value
                    Else
                        HasError = True
                    End If
                Else
                    HasError = True
                End If
                Return Nothing
            End Function
        End Class
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
        End Sub
        Private Sub ColorPickerInternalForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize(True)
                .AddOkCancelToolbar()

                If ResultColor.Exists Then
                    TXT_NAME.Text = ResultColor.Name
                    COLOR_PICKER.ColorsSet(ResultColor)
                    TXT_NAME.Buttons.Clear()
                    TXT_NAME.Buttons.UpdateButtonsPositions()
                    TXT_NAME.Enabled = False
                    COLOR_PICKER.RemoveAllButtons()
                End If

                .MyFieldsCheckerE = New FieldsChecker
                .MyFieldsCheckerE.AddControl(Of String)(TXT_NAME, TXT_NAME.CaptionText,, New ColorNameProvider(ResultColor.Name))
                .MyFieldsChecker.EndLoaderOperations()

                .EndLoaderOperations()
            End With
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            If MyDefs.MyFieldsChecker.AllParamsOK Then
                Dim c As DataColor = COLOR_PICKER.ColorsGet
                If c.Exists Then
                    Dim i% = Settings.Colors.IndexOf(c)
                    If i = -1 Then
                        c.Name = TXT_NAME.Text
                        ResultColor = c
                        MyDefs.CloseForm()
                    Else
                        MsgBoxE({$"The selected color is already present: '{Settings.Colors(i).Name}'", "Color selector"}, vbCritical)
                    End If
                Else
                    MsgBoxE({"You didn't choose a color", "Color selector"}, vbCritical)
                End If
            End If
        End Sub
    End Class
End Namespace