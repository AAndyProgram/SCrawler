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
        Private _UseAccountName As Boolean = False
        Friend ReadOnly Property UseAccountName As Boolean
            Get
                Return _UseAccountName
            End Get
        End Property
        Friend Sub New(ByVal InitialList As IEnumerable(Of String))
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            If InitialList.ListExists Then TXT_URLS.Text = InitialList.ListToString(vbNewLine)
        End Sub
        Private Sub MyForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize()
                .AddOkCancelToolbar()
                Settings.DownloadLocations.PopulateComboBox(TXT_OUTPUT)
                TXT_OUTPUT.Text = Settings.LatestSavingPath.Value.PathWithSeparator
                If TXT_OUTPUT.Text.IsEmptyString Then TXT_OUTPUT.Text = Application.StartupPath.CSFileP.PathWithSeparator
                TXT_URLS_TextChanged()
                CH_THUMB_ALONG.Checked = Settings.STDownloader_SnapshotsKeepWithFiles_ThumbAlong
                .MyFieldsChecker = New FieldsChecker
                With .MyFieldsCheckerE
                    .AddControl(Of String)(TXT_OUTPUT, TXT_OUTPUT.CaptionText)
                    .EndLoaderOperations()
                End With
                .EndLoaderOperations()
                .MyOkCancel.EnableOK = True
            End With
        End Sub
        Private Sub DownloaderUrlsArrForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            Dim b As Boolean = True
            If e.KeyCode = Keys.O And e.Control Then
                Settings.DownloadLocations.ChooseNewLocation(TXT_OUTPUT, False, Settings.STDownloader_OutputPathAskForName)
            ElseIf e.KeyCode = Keys.O And e.Alt Then
                Settings.DownloadLocations.ChooseNewLocation(TXT_OUTPUT, True, Settings.STDownloader_OutputPathAskForName)
            Else
                b = False
            End If
            If b Then e.Handled = True
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            If MyDefs.MyFieldsChecker.AllParamsOK Then MyDefs.CloseForm()
        End Sub
        Private Sub TXT_OUTPUT_ActionOnButtonClick(ByVal Sender As Object, ByVal e As ActionButtonEventArgs) Handles TXT_OUTPUT.ActionOnButtonClick
            If Sender.DefaultButton = ADB.Open Or Sender.DefaultButton = ADB.Add Then _
               Settings.DownloadLocations.ChooseNewLocation(TXT_OUTPUT, Sender.DefaultButton = ADB.Add, Settings.STDownloader_OutputPathAskForName)
        End Sub
        Private Sub TXT_URLS_TextChanged() Handles TXT_URLS.TextChanged
            Try
                With CMB_ACCOUNT
                    .BeginUpdate()
                    .Items.Clear()
                    .Text = String.Empty
                    _UseAccountName = False
                    If Not TXT_URLS.Text.IsEmptyString Then
                        Dim plugins As IEnumerable(Of String) = TXT_URLS.Lines.Select(Function(u) DownloaderUrlForm.GetMediaPluginKey(u)).Distinct
                        Dim fpl As Func(Of String, Boolean) = Function(p) Not p.IsEmptyString
                        If plugins.ListExists AndAlso plugins.LongCount(fpl) = 1 Then
                            CMB_ACCOUNT.Items.AddRange(Settings(plugins.First(fpl)).Select(Function(p) New ListItem(p.AccountName.IfNullOrEmpty(
                                                                                                                    Plugin.Hosts.SettingsHost.NameAccountNameDefault))))
                            _UseAccountName = True
                        End If
                    End If
                    .LeaveDefaultButtons = .Items.Count > 1
                    .Buttons.UpdateButtonsPositions()
                    .EndUpdate()
                    If .Items.Count > 0 Then .SelectedIndex = 0
                    .Enabled = .Items.Count > 1
                End With
            Catch ex As Exception
                _UseAccountName = True
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[STDownloader.DownloaderUrlsArrForm.TXT_URLS_TextChanged]")
            End Try
        End Sub
    End Class
End Namespace