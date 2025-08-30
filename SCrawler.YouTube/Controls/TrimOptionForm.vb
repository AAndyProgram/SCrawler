' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Toolbars
Imports SCrawler.API.YouTube.Base
Namespace API.YouTube.Controls
	Friend Class TrimOptionForm
		Private WithEvents MyDefs As DefaultFormOptions
		Friend Property MyTrimOption As TrimOption
		Private ReadOnly FieldsDateProvider As New CustomProvider(Function(v) AConvert(Of TimeSpan)(v, Nothing, EDP.ReturnValue))
		Private ReadOnly TCE As New ErrorsDescriber(False, False, False, New TimeSpan)
		Friend Sub New(Optional ByVal Opt As TrimOption = Nothing)
			InitializeComponent()
			MyDefs = New DefaultFormOptions(Me, MyYouTubeSettings.DesignXml)
			MyTrimOption = Opt
		End Sub
		Private Sub TrimOptionForm_Load(sender As Object, e As EventArgs) Handles Me.Load
			Try
				With MyDefs
					.MyViewInitialize()
					.AddOkCancelToolbar()

					TXT_NAME.Text = MyTrimOption.Name
					TXT_FROM_INT.Text = MyTrimOption.Start
					TXT_TO_INT.Text = MyTrimOption.End

					OPT_INT.Checked = True

					.MyFieldsCheckerE = New FieldsChecker
					With .MyFieldsCheckerE
						.AddControl(Of Integer)(TXT_FROM_INT, "From")
						.AddControl(Of Integer)(TXT_TO_INT, "To")
						.AddControl(Of String)(TXT_FROM_DATE, "From (time)",, FieldsDateProvider)
						.AddControl(Of String)(TXT_TO_DATE, "To (time)",, FieldsDateProvider)
						.EndLoaderOperations()
					End With

					.EndLoaderOperations()
				End With
			Catch ex As Exception
				MyDefs.InvokeLoaderError(ex)
			End Try
		End Sub
		Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
			If MyDefs.MyFieldsChecker.AllParamsOK Then
				MyTrimOption = New TrimOption With {
					.Name = TXT_NAME.Text,
					.Start = AConvert(Of Integer)(TXT_FROM_INT.Text, 0),
					.[End] = AConvert(Of Integer)(TXT_TO_INT.Text, 0)
				}
				MyDefs.CloseForm()
			End If
		End Sub
		Private Sub OPT_INT_TIME_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_INT.CheckedChanged, OPT_TIME.CheckedChanged
			TP_TIME_INT.Enabled = OPT_INT.Checked
			TP_TIME_TIME.Enabled = OPT_TIME.Checked
		End Sub
		Private _TextHandlersEnabled As Boolean = True
		Private Sub TXT_FROM_TO_TextChanged(sender As Object, e As EventArgs) Handles TXT_FROM_INT.ActionOnTextChanged, TXT_TO_INT.ActionOnTextChanged,
																					  TXT_FROM_DATE.ActionOnTextChanged, TXT_TO_DATE.ActionOnTextChanged
			If _TextHandlersEnabled Then
				_TextHandlersEnabled = False
				Select Case DirectCast(sender, Control).Name
					Case TXT_FROM_INT.Name : TXT_FROM_DATE.Value = AConvert(Of String)(TimeSpan.FromSeconds(AConvert(Of Integer)(TXT_FROM_INT.Text, 0)), TimeToStringProviderH)
					Case TXT_TO_INT.Name : TXT_TO_DATE.Value = AConvert(Of String)(TimeSpan.FromSeconds(AConvert(Of Integer)(TXT_TO_INT.Text, 0)), TimeToStringProviderH)
					Case TXT_FROM_DATE.Name : TXT_FROM_INT.Text = CInt(CType(AConvert(Of TimeSpan)(TXT_FROM_DATE.Text, TCE), TimeSpan).TotalSeconds)
					Case TXT_TO_DATE.Name : TXT_TO_INT.Text = CInt(CType(AConvert(Of TimeSpan)(TXT_TO_DATE.Text, TCE), TimeSpan).TotalSeconds)
				End Select
				_TextHandlersEnabled = True
			End If
		End Sub
	End Class
End Namespace