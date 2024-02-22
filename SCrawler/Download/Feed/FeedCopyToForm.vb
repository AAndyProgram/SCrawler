' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls.Base
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Namespace DownloadObjects
    Friend Class FeedCopyToForm
        Private WithEvents MyDefs As DefaultFormOptions
        Private _Destination As SFile = Nothing
        Friend ReadOnly Property Destination As SFile
            Get
                Return _Destination
            End Get
        End Property
        Friend Sub New(ByVal Files As IEnumerable(Of SFile), ByVal IsCopy As Boolean)
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            If Files.ListExists Then TXT_FILES.Text = Files.ListToString(vbNewLine)
            Text = $"{IIf(IsCopy, "Copy", "Move")} files to..."
            Try
                If IsCopy Then
                    Icon = ImageRenderer.GetIcon(My.Resources.PastePic_32, EDP.ThrowException)
                Else
                    Icon = ImageRenderer.GetIcon(My.Resources.CutPic_48, EDP.ThrowException)
                End If
            Catch
                ShowIcon = False
            End Try
        End Sub
        Private Sub FeedCopyToForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDefs
                    .MyViewInitialize(True)
                    .AddOkCancelToolbar()
                    .MyFieldsCheckerE = New FieldsChecker
                    With .MyFieldsCheckerE
                        .AddControl(Of String)(CMB_DEST, "Destination")
                        .EndLoaderOperations()
                    End With
                    Settings.DownloadLocations.PopulateComboBox(CMB_DEST)
                    CMB_DEST.Text = Settings.FeedLastCopyMoveLocation.Value
                    .EndLoaderOperations()
                    .MyOkCancel.EnableOK = True
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub FeedCopyToForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            Dim b As Boolean = True
            If e.KeyCode = Keys.O And e.Control Then
                Settings.DownloadLocations.ChooseNewLocation(CMB_DEST, False, False)
            ElseIf e.KeyCode = Keys.O And e.Alt Then
                Settings.DownloadLocations.ChooseNewLocation(CMB_DEST, True, True)
            Else
                b = False
            End If
            If b Then e.Handled = True
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            If MyDefs.MyFieldsChecker.AllParamsOK Then
                _Destination = CMB_DEST.Text
                Settings.FeedLastCopyMoveLocation.Value = _Destination
                MyDefs.CloseForm()
            End If
        End Sub
        Private Sub CMB_DEST_ActionOnButtonClick(ByVal Sender As Object, ByVal e As ActionButtonEventArgs) Handles CMB_DEST.ActionOnButtonClick
            If Sender.DefaultButton = ADB.Open Or Sender.DefaultButton = ADB.Add Then _
               Settings.DownloadLocations.ChooseNewLocation(CMB_DEST, Sender.DefaultButton = ADB.Add, Sender.DefaultButton = ADB.Add)
        End Sub
    End Class
End Namespace