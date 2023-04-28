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
    Friend Class DownloaderUrlsArrForm
        Private WithEvents MyDefs As DefaultFormOptions
        Friend ReadOnly Property OutputPath As SFile
            Get
                Return TXT_OUTPUT.Text.CSFileP
            End Get
        End Property
        Friend ReadOnly Property URLs As IEnumerable(Of String)
            Get
                If TXT_URLS.Text.IsEmptyString Then
                    Return New String() {}
                Else
                    Return TXT_URLS.Lines
                End If
            End Get
        End Property
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
        End Sub
        Private Sub MyForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize()
                .AddOkCancelToolbar()
                TXT_OUTPUT.Text = Settings.LatestSavingPath.Value.PathWithSeparator
                If TXT_OUTPUT.Text.IsEmptyString Then TXT_OUTPUT.Text = Application.StartupPath.CSFileP.PathWithSeparator
                .MyFieldsChecker = New FieldsChecker
                With .MyFieldsCheckerE
                    .AddControl(Of String)(TXT_OUTPUT, TXT_OUTPUT.CaptionText)
                    .EndLoaderOperations()
                End With
                .EndLoaderOperations()
            End With
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            If MyDefs.MyFieldsChecker.AllParamsOK Then MyDefs.CloseForm()
        End Sub
        Private Sub TXT_OUTPUT_ActionOnButtonClick(ByVal Sender As Object, ByVal e As ActionButtonEventArgs) Handles TXT_OUTPUT.ActionOnButtonClick
            If e.DefaultButton = ActionButton.DefaultButtons.Open Then
                Dim f As SFile = TXT_OUTPUT.Text.CSFileP
                f = SFile.SelectPath(f, "Select a folder for files", EDP.ReturnValue)
                If Not f.IsEmptyString Then TXT_OUTPUT.Text = f.PathWithSeparator
            End If
        End Sub
    End Class
End Namespace