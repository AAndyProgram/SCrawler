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
Imports SCrawler.API.YouTube.Objects
Imports SCrawler.API.YouTube.Base
Imports ETC = PersonalUtilities.Forms.Toolbars.EditToolbar.ControlItem
Namespace API.YouTube.Controls
	Friend Class VideoOptionsTrimForm
#Region "Declarations"
		Private WithEvents MyDefs As DefaultFormOptions
		Private ReadOnly Property MyMedia As YouTubeMediaContainerBase
		Private ReadOnly Property TrimData As List(Of TrimOption)
		Private WithEvents BTT_CLEAR_ALL As ToolStripButton
		Private WithEvents BTT_CHAPTERS As ToolStripButton
#End Region
#Region "Initializer"
		Friend Sub New(ByRef Media As YouTubeMediaContainerBase)
			InitializeComponent()
			MyDefs = New DefaultFormOptions(Me, MyYouTubeSettings.DesignXml)
			MyMedia = Media
			TrimData = New List(Of TrimOption)
			TrimData.ListAddList(Media.TrimOptions)
			BTT_CLEAR_ALL = New ToolStripButton("Clear", PersonalUtilities.My.Resources.DeletePic_Red_24) With {
				.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
				.BackColor = MyColor.DeleteBack,
				.ForeColor = MyColor.DeleteFore
			}
			BTT_CHAPTERS = New ToolStripButton("Chapters") With {.DisplayStyle = ToolStripItemDisplayStyle.Text, .Enabled = Media.Chapters.Count > 0}
		End Sub
#End Region
#Region "Form handlers"
		Private Sub VideoOptionsTrimForm_Load(sender As Object, e As EventArgs) Handles Me.Load
			With MyDefs
				.MyViewInitialize()
				.AddOkCancelToolbar()
				.AddEditToolbar({ETC.Add, ETC.Edit, ETC.Delete, BTT_CLEAR_ALL, ETC.Separator, BTT_CHAPTERS})

				Refill()

				With MyMedia
					If Not .TrimOptionsSet Then
						With MyYouTubeSettings
							CH_DEL_ORIG.Checked = .TrimDeleteOriginalFile
							CH_ADD_M3U8.Checked = .TrimAddTrimmedFilesToM3U8
							CH_SEP_FOLDER.Checked = .TrimSeparateFolder
						End With
					Else
						CH_DEL_ORIG.Checked = .TrimDeleteOriginalFile
						CH_ADD_M3U8.Checked = .TrimAddTrimmedFilesToM3U8
						CH_SEP_FOLDER.Checked = .TrimSeparateFolder
					End If
				End With

				.EndLoaderOperations()
			End With
		End Sub
		Private Sub VideoOptionsTrimForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
			TrimData.Clear()
		End Sub
#End Region
#Region "Refill"
		Private Sub Refill(Optional ByVal DEL As Boolean = False)
			Dim indx% = _CurrentIndex
			With LIST_TRIM
				.BeginUpdate()
				.Items.Clear()
				If TrimData.Count > 0 Then
					TrimData.Sort()
					.Items.AddRange(TrimData.Cast(Of Object).ToArray)
					If DEL Then indx -= IIf(indx - 1 < 0, 0, 1)
					If indx.ValueBetween(0, TrimData.Count - 1) Then
						.SelectedIndex = indx
					ElseIf (indx - 1).ValueBetween(0, TrimData.Count - 1) Then
						.SelectedIndex = indx - 1
					ElseIf (indx + 1).ValueBetween(0, TrimData.Count - 1) Then
						.SelectedIndex = indx + 1
					End If
				End If
				.EndUpdate()
			End With
		End Sub
#End Region
#Region "Ok"
		Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
			With MyMedia
				.TrimOptions.Clear()
				.TrimDeleteOriginalFile = CH_DEL_ORIG.Checked
				.TrimAddTrimmedFilesToM3U8 = CH_ADD_M3U8.Checked
				.TrimSeparateFolder = CH_SEP_FOLDER.Checked
				.TrimOptionsSet = True
			End With

			If TrimData.Count > 0 Then
				TrimData.Sort()
				Dim indx% = 0
				Dim c%
				Dim opt As TrimOption
				Dim dic As New Dictionary(Of String, Integer)
				For i% = 0 To TrimData.Count - 1
					opt = TrimData(i)
					If opt.Name.IsEmptyString Then
						indx += 1
						opt.Name = indx
					Else
						c = TrimData.LongCount(Function(d) Not d.Name.IsEmptyString AndAlso d.Name = opt.Name)
						If c > 1 Then
							If Not dic.ContainsKey(opt.Name) Then dic.Add(opt.Name, 0)
							dic(opt.Name) += 1
							opt.Name &= $"_{dic(opt.Name)}"
						End If
					End If
					TrimData(i) = opt
				Next
				dic.Clear()
				MyMedia.TrimOptions.AddRange(TrimData)
			End If

			MyDefs.CloseForm()
		End Sub
#End Region
#Region "Edit"
		Private Sub MyDefs_ButtonAddClick(ByVal Sender As Object, ByVal e As EditToolbarEventArgs) Handles MyDefs.ButtonAddClick
			Using f As New TrimOptionForm
				f.ShowDialog()
				If f.DialogResult = DialogResult.OK AndAlso
				   (TrimData.Count = 0 OrElse Not TrimData.Any(Function(t) t.End = f.MyTrimOption.End And t.Start = f.MyTrimOption.Start)) Then _
						TrimData.Add(f.MyTrimOption) : Refill()
			End Using
		End Sub
		Private Sub MyDefs_ButtonEditClick(ByVal Sender As Object, ByVal e As EditToolbarEventArgs) Handles MyDefs.ButtonEditClick
			If _CurrentIndex.ValueBetween(0, TrimData.Count - 1) Then
				Using f As New TrimOptionForm(TrimData(_CurrentIndex))
					f.ShowDialog()
					If f.DialogResult = DialogResult.OK Then TrimData(_CurrentIndex) = f.MyTrimOption : Refill()
				End Using
			End If
		End Sub
		Private Sub MyDefs_ButtonDeleteClickE(ByVal Sender As Object, ByVal e As EditToolbarEventArgs) Handles MyDefs.ButtonDeleteClickE
			If _CurrentIndex.ValueBetween(0, TrimData.Count - 1) AndAlso
				MsgBoxE({$"Are you sure you want to remove the following trim option?{vbCr}{vbCr}{TrimData(_CurrentIndex)}", "Remove trim option"}, vbYesNo + vbExclamation) = vbYes Then _
				TrimData.RemoveAt(_CurrentIndex) : Refill(True)
		End Sub
		Private Sub BTT_CLEAR_ALL_Click(sender As Object, e As EventArgs) Handles BTT_CLEAR_ALL.Click
			If MsgBoxE({$"Are you sure you want to remove ALL trim options?", "Remove trim option"}, vbYesNo + vbCritical) = vbYes Then TrimData.Clear() : Refill()
		End Sub
		Private Sub BTT_CHAPTERS_Click(sender As Object, e As EventArgs) Handles BTT_CHAPTERS.Click
			Using f As New ChaptersForm(MyMedia, TrimData)
				f.ShowDialog()
				If f.DialogResult = DialogResult.OK AndAlso f.MyResult.Count > 0 Then TrimData.ListAddList(f.MyResult, LAP.NotContainsOnly) : Refill()
			End Using
		End Sub
#End Region
#Region "List"
		Private _CurrentIndex As Integer = -1
		Private Sub LIST_TRIM_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LIST_TRIM.SelectedIndexChanged
			_CurrentIndex = LIST_TRIM.SelectedIndex
		End Sub
#End Region
	End Class
End Namespace