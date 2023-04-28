' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls.Base
Namespace DownloadObjects.STDownloader
    Friend Class DownloaderUrlForm
        Private WithEvents MyDefs As DefaultFormOptions
        Friend Property URL As String
        Friend Property OutputPath As SFile
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
        End Sub
        Private Sub MyForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize(True)
                .AddOkCancelToolbar()
                TXT_URL.Text = URL
                TXT_PATH.Text = Settings.LatestSavingPath.Value
                If TXT_PATH.Text.IsEmptyString Then TXT_PATH.Text = Application.StartupPath.CSFileP.PathWithSeparator
                .MyFieldsChecker = New FieldsChecker
                With .MyFieldsCheckerE
                    .AddControl(Of String)(TXT_URL, TXT_URL.CaptionText)
                    .AddControl(Of String)(TXT_PATH, TXT_PATH.CaptionText)
                    .EndLoaderOperations()
                End With
                .EndLoaderOperations()
            End With
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            If MyDefs.MyFieldsChecker.AllParamsOK Then
                URL = TXT_URL.Text
                OutputPath = TXT_PATH.Text.CSFileP
                MyDefs.CloseForm()
            End If
        End Sub
        Private Sub TXT_PATH_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles TXT_PATH.ActionOnButtonClick
            If Sender.DefaultButton = ActionButton.DefaultButtons.Open Then
                Dim f As SFile = SFile.SelectPath(TXT_PATH.Text.CSFileP, "Select output directory", EDP.ReturnValue)
                If Not f.IsEmptyString Then TXT_PATH.Text = f.PathWithSeparator
            End If
        End Sub
    End Class
End Namespace