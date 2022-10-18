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
    Friend Class AdditionalSettingsForm
        Private WithEvents MyDefs As DefaultFormOptions
        Friend Property MyParameters As SettingsExchangeOptions
        Friend Sub New(ByVal Parameters As SettingsExchangeOptions)
            InitializeComponent()
            MyParameters = Parameters
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
        End Sub
        Private Sub MyForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize(True)
                .AddOkCancelToolbar()
                With MyParameters
                    CH_DOWN_TIME.Checked = .DownloadTimeline
                    CH_DOWN_TAG.Checked = .DownloadStoriesTagged
                    CH_DOWN_SAVED.Checked = .DownloadSaved
                End With
                .EndLoaderOperations()
            End With
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            MyParameters = New SettingsExchangeOptions With {
                .DownloadTimeline = CH_DOWN_TIME.Checked,
                .DownloadStoriesTagged = CH_DOWN_TAG.Checked,
                .DownloadSaved = CH_DOWN_SAVED.Checked,
                .Changed = True
            }
            MyDefs.CloseForm()
        End Sub
    End Class
End Namespace