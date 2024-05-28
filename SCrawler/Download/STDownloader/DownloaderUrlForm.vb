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
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Namespace DownloadObjects.STDownloader
    Friend Class DownloaderUrlForm
        Private WithEvents MyDefs As DefaultFormOptions
        Friend Property URL As String
        Friend Property OutputPath As SFile
        Friend ReadOnly Property AccountName As String
            Get
                Return CMB_ACCOUNT.Text
            End Get
        End Property
        Friend ReadOnly Property ThumbAlong As Boolean
            Get
                Return CH_THUMB_ALONG.Checked
            End Get
        End Property
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
        End Sub
        Private Sub MyForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize(True)
                .AddOkCancelToolbar()
                TXT_URL.Text = URL
                Settings.DownloadLocations.PopulateComboBox(TXT_PATH)
                TXT_PATH.Text = Settings.LatestSavingPath.Value
                If TXT_PATH.Text.IsEmptyString Then TXT_PATH.Text = Application.StartupPath.CSFileP.PathWithSeparator
                TXT_URL_ActionOnTextChanged()
                CH_THUMB_ALONG.Checked = Settings.STDownloader_SnapshotsKeepWithFiles_ThumbAlong
                .MyFieldsChecker = New FieldsChecker
                With .MyFieldsCheckerE
                    .AddControl(Of String)(TXT_URL, TXT_URL.CaptionText)
                    .AddControl(Of String)(TXT_PATH, TXT_PATH.CaptionText)
                    .EndLoaderOperations()
                End With
                .EndLoaderOperations()
                .MyOkCancel.EnableOK = True
            End With
        End Sub
        Private Sub DownloaderUrlForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            Dim b As Boolean = True
            If e.KeyCode = Keys.O And e.Control Then
                Settings.DownloadLocations.ChooseNewLocation(TXT_PATH, False, Settings.STDownloader_OutputPathAskForName)
            ElseIf e.KeyCode = Keys.O And e.Alt Then
                Settings.DownloadLocations.ChooseNewLocation(TXT_PATH, True, Settings.STDownloader_OutputPathAskForName)
            Else
                b = False
            End If
            If b Then e.Handled = True
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            If MyDefs.MyFieldsChecker.AllParamsOK Then
                URL = TXT_URL.Text
                OutputPath = TXT_PATH.Text.CSFileP
                MyDefs.CloseForm()
            End If
        End Sub
        Friend Shared Function GetMediaPluginKey(ByVal URL As String) As String
            If Not URL.IsEmptyString Then
                Return Settings.Plugins.Select(Function(p) p.Settings.IsMyImageVideo(URL).HostKey).FirstOrDefault(Function(p) Not p.IsEmptyString)
            Else
                Return String.Empty
            End If
        End Function
        Private Sub TXT_URL_ActionOnTextChanged() Handles TXT_URL.ActionOnTextChanged
            Try
                With CMB_ACCOUNT
                    .BeginUpdate()
                    .Items.Clear()
                    .Text = String.Empty
                    If Not TXT_URL.Text.IsEmptyString Then
                        Dim plugin$ = Settings.Plugins.Select(Function(p) p.Settings.IsMyImageVideo(TXT_URL.Text).HostKey).FirstOrDefault(Function(p) Not p.IsEmptyString)
                        If Not plugin.IsEmptyString Then _
                           CMB_ACCOUNT.Items.AddRange(Settings(plugin).Select(Function(p) New ListItem(p.AccountName.IfNullOrEmpty(
                                                                                                       SCrawler.Plugin.Hosts.SettingsHost.NameAccountNameDefault))))
                    End If
                    .LeaveDefaultButtons = .Items.Count > 1
                    .Buttons.UpdateButtonsPositions()
                    .EndUpdate()
                    If .Items.Count > 0 Then .SelectedIndex = 0
                    .Enabled = .Items.Count > 1
                End With
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[STDownloader.DownloaderUrlForm.TXT_URL_ActionOnTextChanged]")
            End Try
        End Sub
        Private Sub TXT_PATH_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles TXT_PATH.ActionOnButtonClick
            If Sender.DefaultButton = ADB.Open Or Sender.DefaultButton = ADB.Add Then _
               Settings.DownloadLocations.ChooseNewLocation(TXT_PATH, Sender.DefaultButton = ADB.Add, Settings.STDownloader_OutputPathAskForName)
        End Sub
    End Class
End Namespace