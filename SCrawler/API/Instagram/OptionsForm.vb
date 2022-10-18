' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Namespace API.Instagram
    Friend Class OptionsForm
        Private WithEvents MyDefs As DefaultFormOptions
        Private ReadOnly Property MyExchangeOptions As EditorExchangeOptions
        Friend Sub New(ByRef ExchangeOptions As EditorExchangeOptions)
            InitializeComponent()
            MyExchangeOptions = ExchangeOptions
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
        End Sub
        Private Sub OptionsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize(True)
                .AddOkCancelToolbar()
                With MyExchangeOptions
                    CH_GET_STORIES.Checked = .GetStories
                    CH_GET_TAGGED.Checked = .GetTagged
                End With
                .EndLoaderOperations()
            End With
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            With MyExchangeOptions
                .GetStories = CH_GET_STORIES.Checked
                .GetTagged = CH_GET_TAGGED.Checked
            End With
            MyDefs.CloseForm()
        End Sub
    End Class
End Namespace