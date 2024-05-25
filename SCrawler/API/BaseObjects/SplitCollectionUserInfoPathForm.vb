' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls.Base
Imports SCrawler.DownloadObjects.STDownloader
Namespace API.Base
    Friend Class SplitCollectionUserInfoPathForm
        Private WithEvents MyDefs As DefaultFormOptions
        Friend User As SplitCollectionUserInfo
        Private ReadOnly UserNewPathDef As String
        Friend Sub New(ByVal _User As SplitCollectionUserInfo)
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            User = _User
            UserNewPathDef = User.UserNew.File.CutPath.PathWithSeparator
        End Sub
        Private Sub SplitCollectionUserInfoPathForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize()
                .AddOkCancelToolbar()

                TXT_PATH_CURR.Text = User.UserOrig.File.CutPath.PathWithSeparator
                TXT_PATH_NEW.Text = UserNewPathDef

                .MyFieldsCheckerE = New FieldsChecker
                .MyFieldsCheckerE.AddControl(Of String)(TXT_PATH_NEW, "New path")
                .MyFieldsCheckerE.EndLoaderOperations()

                .EndLoaderOperations()
            End With
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            If MyDefs.MyFieldsChecker.AllParamsOK Then MyDefs.CloseForm()
        End Sub
        Private Sub TXT_PATH_NEW_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles TXT_PATH_NEW.ActionOnButtonClick
            Select Case e.DefaultButton
                Case ActionButton.DefaultButtons.Refresh : TXT_PATH_NEW.Text = UserNewPathDef
                Case ActionButton.DefaultButtons.Open
                    Using ff As New Editors.GlobalLocationsChooserForm With {.MyInitialLocation = TXT_PATH_NEW.Text}
                        ff.ShowDialog()
                        If ff.DialogResult = DialogResult.OK Then
                            Dim dest As DownloadLocation = ff.MyDestination
                            If Not dest.Path.IsEmptyString Then
                                Dim ph As PathMoverHandler = Editors.GlobalLocationsChooserForm.ModelHandler(dest.Model)
                                If Not ph Is Nothing Then TXT_PATH_NEW.Text = ph.Invoke(User.UserNew, dest.Path.CSFileP).ToString
                            End If
                        End If
                    End Using
            End Select
        End Sub
        Private Sub TXT_PATH_NEW_ActionOnTextChanged(sender As Object, e As EventArgs) Handles TXT_PATH_NEW.ActionOnTextChanged
            If Not MyDefs.Initializing Then
                Dim f As SFile = TXT_PATH_NEW.Text.CSFileP
                If Not f.IsEmptyString Then
                    User.UserNew.SpecialPath = f
                    User.UserNew.UpdateUserFile()
                    User.Changed = Not User.UserNew.File.CutPath.PathWithSeparator = UserNewPathDef
                End If
            End If
        End Sub
    End Class
End Namespace