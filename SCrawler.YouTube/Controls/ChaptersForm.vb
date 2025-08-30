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
Imports SCrawler.API.YouTube.Objects
Namespace API.YouTube.Controls
	Friend Class ChaptersForm
		Private WithEvents MyDefs As DefaultFormOptions
		Private ReadOnly Property MyData As YouTubeMediaContainerBase
		Private ReadOnly Property MyDataSelected As List(Of TrimOption)
		Friend ReadOnly Property MyResult As List(Of TrimOption)
		Friend Sub New(ByRef data As YouTubeMediaContainerBase, ByVal selected As IEnumerable(Of TrimOption))
			InitializeComponent()
			MyDefs = New DefaultFormOptions(Me, MyYouTubeSettings.DesignXml)
			MyData = data
			MyDataSelected = New List(Of TrimOption)
			MyDataSelected.ListAddList(selected)
			MyResult = New List(Of TrimOption)
		End Sub
		Private Sub ChaptersForm_Load(sender As Object, e As EventArgs) Handles Me.Load
			With MyDefs
				.MyViewInitialize()
				.AddOkCancelToolbar()

				With LIST_CHAPTERS
					.BeginUpdate()
					.Items.AddRange(MyData.Chapters.Cast(Of Object).ToArray)
					If MyDataSelected.Count > 0 Then
						For i% = 0 To .Items.Count - 1 : .SetItemChecked(i, MyDataSelected.Contains(MyData.Chapters(i))) : Next
					End If
					.EndUpdate()
				End With

				.EndLoaderOperations()
			End With
		End Sub
		Private Sub ChaptersForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
			MyResult.Clear()
			MyDataSelected.Clear()
		End Sub
		Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
			With LIST_CHAPTERS
				For i% = 0 To .Items.Count - 1
					If .GetItemChecked(i) Then MyResult.Add(MyData.Chapters(i))
				Next
			End With
			MyDefs.CloseForm()
		End Sub
		Private Sub BTT_ALL_NONE_INVERT_Click(sender As Object, e As EventArgs) Handles BTT_ALL.Click, BTT_NONE.Click, BTT_INVERT.Click
			Dim v As Boolean = sender Is BTT_ALL
			Dim isInvert As Boolean = sender Is BTT_INVERT
			With LIST_CHAPTERS
				.BeginUpdate()
				For i% = 0 To .Items.Count - 1 : .SetItemChecked(i, If(isInvert, Not .GetItemChecked(i), v)) : Next
				.EndUpdate()
			End With
		End Sub
	End Class
End Namespace