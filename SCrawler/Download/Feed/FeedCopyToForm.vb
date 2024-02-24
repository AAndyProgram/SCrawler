' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API
Imports SCrawler.API.Base
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Namespace DownloadObjects
    Friend Class FeedCopyToForm
#Region "Declarations"
        Private WithEvents MyDefs As DefaultFormOptions
        Private _Result As FeedMoveCopyTo
        Friend ReadOnly Property Result As FeedMoveCopyTo
            Get
                Return _Result
            End Get
        End Property
        Private ReadOnly Profiles As List(Of IUserData)
#End Region
#Region "Initializer"
        Friend Sub New(ByVal Files As IEnumerable(Of SFile), ByVal IsCopy As Boolean)
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            If Files.ListExists Then TXT_FILES.Text = Files.ListToString(vbNewLine)
            Profiles = New List(Of IUserData)
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
#End Region
#Region "Form handlers"
        Private Sub FeedCopyToForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDefs
                    .MyViewInitialize()
                    .AddOkCancelToolbar()
                    .MyFieldsCheckerE = New FieldsChecker
                    With .MyFieldsCheckerE
                        .AddControl(Of String)(CMB_DEST, "Destination")
                        .EndLoaderOperations()
                    End With
                    Settings.DownloadLocations.PopulateComboBox(CMB_DEST)
                    With Settings
                        CMB_DEST.Text = .FeedMoveCopyLastLocation.Value
                        CH_VIDEO_SEP.Checked = .FeedMoveCopySeparateVideo
                        CH_PROFILE_REPLACE.Checked = .FeedMoveCopyReplaceUserProfile
                        CH_PROFILE_CREATE.Checked = .FeedMoveCopyCreatePathProfile
                    End With
                    If Settings.Users.Count > 0 Then _
                       Profiles.AddRange(Settings.Users.SelectMany(Of IUserData)(Function(ByVal p As IUserData) As IEnumerable(Of IUserData)
                                                                                     If p.IsCollection Then
                                                                                         Return DirectCast(p, UserDataBind).Collections
                                                                                     Else
                                                                                         Return {p}
                                                                                     End If
                                                                                 End Function))
                    If Profiles.Count > 0 Then
                        CMB_PROFILE.BeginUpdate()
                        CMB_PROFILE_PATH.BeginUpdate()
                        For i% = 0 To Profiles.Count - 1
                            With DirectCast(Profiles(i), UserDataBase)
                                If If(.HOST?.Key, String.Empty) = PathPlugin.PluginKey Then
                                    CMB_PROFILE_PATH.Items.Add(New ListItem({ .Self.ToStringForLog, i}))
                                Else
                                    CMB_PROFILE.Items.Add(New ListItem({ .Self.ToStringForLog, i}))
                                End If
                            End With
                        Next
                        CMB_PROFILE.EndUpdate()
                        CMB_PROFILE_PATH.EndUpdate()
                    End If
                    UpdateCombo()
                    If Not CMB_DEST.Text.IsEmptyString Then
                        _ComboChangingByDest = True
                        UpdateComboIndex(CMB_PROFILE)
                        UpdateComboIndex(CMB_PROFILE_PATH)
                        _ComboChangingByDest = False
                    End If
                    UpdateComboCheck()
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
        Private Sub FeedCopyToForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            Profiles.Clear()
        End Sub
#End Region
#Region "Ok, Cancel"
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            Try
                If MyDefs.MyFieldsChecker.AllParamsOK Then
                    _Result = New FeedMoveCopyTo With {
                        .Destination = CMB_DEST.Text,
                        .SeparateVideoFolder = CH_VIDEO_SEP.Checked,
                        .ReplaceUserProfile = CH_PROFILE_REPLACE.Checked,
                        .ReplaceUserProfile_CreateIfNull = CH_PROFILE_CREATE.Checked
                    }
                    If CH_PROFILE_REPLACE.Checked Then
                        If CMB_PROFILE.Enabled And CMB_PROFILE.Checked Then
                            With CMB_PROFILE
                                If .SelectedIndex >= 0 Then _
                                   _Result.ReplaceUserProfile_Profile = Settings.GetUser(GetComboProfile(.Self), False)
                            End With
                        ElseIf CMB_PROFILE_PATH.Enabled And CMB_PROFILE_PATH.Checked Then
                            With CMB_PROFILE_PATH
                                If .SelectedIndex >= 0 Then _
                                   _Result.ReplaceUserProfile_Profile = Settings.GetUser(GetComboProfile(.Self), False)
                            End With
                        End If
                    End If
                    With Settings
                        .BeginUpdate()
                        .FeedMoveCopyLastLocation.Value = CMB_DEST.Text
                        .FeedMoveCopyIsProfileChecked.Value = CMB_PROFILE.Checked
                        .FeedMoveCopySeparateVideo.Value = CH_VIDEO_SEP.Checked
                        .FeedMoveCopyReplaceUserProfile.Value = CH_PROFILE_REPLACE.Checked
                        .FeedMoveCopyCreatePathProfile.Value = CH_PROFILE_CREATE.Checked
                        .EndUpdate()
                    End With
                    MyDefs.CloseForm()
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "[FeedCopyToForm.OK]")
            End Try
        End Sub
#End Region
#Region "Controls"
        Private _TextChangingByCombo As Boolean = False
        Private _ComboChangingByDest As Boolean = False
        Private Sub CMB_DEST_ActionOnButtonClick(ByVal Sender As Object, ByVal e As ActionButtonEventArgs) Handles CMB_DEST.ActionOnButtonClick
            If Sender.DefaultButton = ADB.Open Or Sender.DefaultButton = ADB.Add Then _
               Settings.DownloadLocations.ChooseNewLocation(CMB_DEST, Sender.DefaultButton = ADB.Add, Sender.DefaultButton = ADB.Add)
        End Sub
        Private Sub CMB_DEST_ActionOnTextChanged(ByVal Sender As Object, ByVal e As EventArgs) Handles CMB_DEST.ActionOnTextChanged
            If Not _TextChangingByCombo And Not CMB_DEST.Checked And Not CMB_DEST.Text.IsEmptyString Then
                _ComboChangingByDest = True
                UpdateComboIndex(CMB_PROFILE)
                UpdateComboIndex(CMB_PROFILE_PATH)
                UpdateComboCheck()
                _ComboChangingByDest = False
            End If
        End Sub
        Private Sub UpdateComboIndex(ByVal CMB As ComboBoxExtended)
            Try
                With CMB
                    If .Count > 0 Then
                        Dim t$ = CMB_DEST.Text.CSFilePSN.StringToLower
                        Dim lvi_check As Func(Of ListItem, Boolean, Boolean) =
                            Function(ByVal lvi As ListItem, ByVal exact As Boolean) As Boolean
                                Dim ii% = lvi.Value(1)
                                If ii.ValueBetween(0, Profiles.Count - 1) Then
                                    With DirectCast(Profiles(ii), UserDataBase)
                                        If exact Then
                                            Return t = .MyFile.CutPath.PathNoSeparator.StringToLower
                                        Else
                                            Return t.StartsWith(.MyFile.CutPath.PathNoSeparator.StringToLower)
                                        End If
                                    End With
                                End If
                                Return False
                            End Function
                        Dim i% = CMB.Items.ListIndexOf(Function(lvi) lvi_check.Invoke(lvi, True))
                        If i = -1 Then i = CMB.Items.ListIndexOf(Function(lvi) lvi_check.Invoke(lvi, False))
                        CMB.SelectedIndex = i
                    End If
                End With
            Catch ex As Exception
                CMB.SelectedIndex = -1
            End Try
        End Sub
        Private Sub UpdateComboCheck()
            Try
                Dim isProfile As Boolean
                If CMB_PROFILE.SelectedIndex >= 0 And CMB_PROFILE_PATH.SelectedIndex >= 0 Then
                    isProfile = Settings.FeedMoveCopyIsProfileChecked
                ElseIf CMB_PROFILE.SelectedIndex >= 0 Then
                    isProfile = True
                ElseIf CMB_PROFILE_PATH.SelectedIndex >= 0 Then
                    isProfile = False
                Else
                    isProfile = Settings.FeedMoveCopyIsProfileChecked
                End If
                If isProfile Then
                    CMB_PROFILE.Checked = True
                Else
                    CMB_PROFILE_PATH.Checked = True
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[FeedCopyToForm.UpdateComboCheck]")
            End Try
        End Sub
        Private Sub CH_PROFILE_REPLACE_CheckedChanged(sender As Object, e As EventArgs) Handles CH_PROFILE_REPLACE.CheckedChanged
            UpdateCombo()
        End Sub
        Private Sub UpdateCombo()
            Dim e As Boolean = CH_PROFILE_REPLACE.Checked
            CH_PROFILE_CREATE.Enabled = e
            CMB_PROFILE.Enabled(True) = e And CMB_PROFILE.Count > 0
            CMB_PROFILE_PATH.Enabled(True) = e And CMB_PROFILE_PATH.Count > 0
        End Sub
#Region "Comboboxes"
        Private Sub CMB_PROFILE_ActionSelectedItemChanged(ByVal Sender As Object, ByVal e As EventArgs, ByVal Item As ListViewItem) Handles CMB_PROFILE.ActionSelectedItemChanged
            If Not MyDefs.Initializing Then SetDestinationByCombo(CMB_PROFILE)
        End Sub
        Private Sub CMB_PROFILE_ActionOnCheckedChange(ByVal Sender As Object, ByVal e As EventArgs, ByVal Checked As Boolean) Handles CMB_PROFILE.ActionOnCheckedChange
            If Checked And Not MyDefs.Initializing And Not _ComboChangingByDest Then SetDestinationByCombo(CMB_PROFILE)
        End Sub
        Private Sub CMB_PROFILE_ActionOnButtonClick(ByVal Sender As Object, ByVal e As ActionButtonEventArgs) Handles CMB_PROFILE.ActionOnButtonClick
            If e.DefaultButton = ADB.Clear Then CMB_PROFILE.SelectedIndex = -1
        End Sub
        Private Sub CMB_PROFILE_PATH_ActionSelectedItemChanged(ByVal Sender As Object, ByVal e As EventArgs, ByVal Item As ListViewItem) Handles CMB_PROFILE_PATH.ActionSelectedItemChanged
            If Not MyDefs.Initializing Then SetDestinationByCombo(CMB_PROFILE_PATH)
        End Sub
        Private Sub CMB_PROFILE_PATH_ActionOnButtonClick(ByVal Sender As Object, ByVal e As ActionButtonEventArgs) Handles CMB_PROFILE_PATH.ActionOnButtonClick
            If e.DefaultButton = ADB.Clear Then CMB_PROFILE_PATH.SelectedIndex = -1
        End Sub
        Private Sub CMB_PROFILE_PATH_ActionOnCheckedChange(ByVal Sender As Object, ByVal e As EventArgs, ByVal Checked As Boolean) Handles CMB_PROFILE_PATH.ActionOnCheckedChange
            If Checked And Not MyDefs.Initializing And Not _ComboChangingByDest Then SetDestinationByCombo(CMB_PROFILE_PATH)
        End Sub
        Private Sub SetDestinationByCombo(ByVal CMB As ComboBoxExtended)
            Try
                With CMB
                    If .Checked And .Enabled And Not CMB_DEST.Checked And Not _ComboChangingByDest And .SelectedIndex >= 0 Then
                        _TextChangingByCombo = True
                        CMB_DEST.Text = GetComboProfile(.Self).MyFile.CutPath.PathWithSeparator
                        _TextChangingByCombo = False
                    End If
                End With
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[FeedCopyToForm.SetDestinationByCombo]")
            End Try
        End Sub
        Private Function GetComboProfile(ByVal CMB As ComboBoxExtended) As UserDataBase
            Try
                With CMB
                    If .SelectedIndex >= 0 Then
                        Dim i% = .Items(.SelectedIndex).Value(1)
                        If i.ValueBetween(0, Profiles.Count - 1) Then Return Profiles(i)
                    End If
                End With
                Return Nothing
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, "[FeedCopyToForm.GetComboProfile]")
            End Try
        End Function
#End Region
#End Region
    End Class
End Namespace