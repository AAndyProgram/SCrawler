' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Namespace DownloadObjects.Groups
    Public Class GroupDefaults : Inherits TableLayoutPanel
        Private ReadOnly TP_1 As TableLayoutPanel
        Private ReadOnly TP_2 As TableLayoutPanel
        Private ReadOnly CH_TEMPORARY As CheckBox
        Private ReadOnly CH_FAV As CheckBox
        Private ReadOnly CH_READY_FOR_DOWN As CheckBox
        Private ReadOnly CH_READY_FOR_DOWN_IGNORE As CheckBox
        Private WithEvents TXT_LABELS As TextBoxExtended
        Private WithEvents TXT_SITES As TextBoxExtended
        Friend WithEvents TXT_NAME As TextBoxExtended
        Private ReadOnly Labels As List(Of String)
        Private ReadOnly LabelsExcluded As List(Of String)
        Private ReadOnly Sites As List(Of String)
        Private ReadOnly SitesExcluded As List(Of String)
        Public Sub New()
            Labels = New List(Of String)
            LabelsExcluded = New List(Of String)
            Sites = New List(Of String)
            SitesExcluded = New List(Of String)

            InitTextBox(TXT_LABELS, "Labels", {New ActionButton(ADB.Edit) With {.ToolTipText = "Edit selected labels"},
                                               New ActionButton(ADB.Delete) With {.ToolTipText = "Edit excluded labels"}, ADB.Clear})
            TXT_LABELS.TextBoxReadOnly = True

            InitTextBox(TXT_SITES, "Sites", {New ActionButton(ADB.Edit) With {.ToolTipText = "Edit selected sites"},
                                             New ActionButton(ADB.Delete) With {.ToolTipText = "Edit excluded sites"}, ADB.Clear})
            TXT_SITES.TextBoxReadOnly = True

            InitTextBox(TXT_NAME, "Name", {ADB.Clear})

            CH_TEMPORARY = New CheckBox With {.Text = "Temporary", .Name = "CH_TEMPORARY", .ThreeState = True, .CheckState = CheckState.Indeterminate, .Dock = DockStyle.Fill}
            CH_FAV = New CheckBox With {.Text = "Favorite", .Name = "CH_FAV", .ThreeState = True, .CheckState = CheckState.Indeterminate, .Dock = DockStyle.Fill}
            CH_READY_FOR_DOWN = New CheckBox With {.Text = "Ready for download", .Name = "CH_READY_FOR_DOWN", .Checked = True, .Dock = DockStyle.Fill}
            CH_READY_FOR_DOWN_IGNORE = New CheckBox With {.Text = "Ignore ready for download", .Name = "CH_READY_FOR_DOWN_IGNORE", .Checked = False, .Dock = DockStyle.Fill}
            TP_1 = New TableLayoutPanel With {.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single, .Margin = New Padding(0), .Dock = DockStyle.Fill}
            FillTP(TP_1, CH_TEMPORARY, CH_FAV)
            TP_2 = New TableLayoutPanel With {.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single, .Margin = New Padding(0), .Dock = DockStyle.Fill}
            FillTP(TP_2, CH_READY_FOR_DOWN, CH_READY_FOR_DOWN_IGNORE)
        End Sub
        Private Sub InitTextBox(ByRef TXT As TextBoxExtended, ByVal Caption As String, ByVal Buttons As ActionButton())
            TXT = New TextBoxExtended
            With TXT
                .BeginInit()
                .Buttons.AddRange(Buttons)
                .CaptionText = Caption
                .CaptionWidth = 50
                .Dock = DockStyle.Fill
                .EndInit()
            End With
        End Sub
        Private Sub FillTP(ByRef TP As TableLayoutPanel, ByVal CNT1 As Control, ByVal CNT2 As Control)
            With TP
                .ColumnCount = 2
                .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))
                .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50))
                .RowCount = 1
                .RowStyles.Add(New RowStyle(SizeType.Percent, 100))
                With .Controls : .Add(CNT1, 0, 0) : .Add(CNT2, 1, 0) : End With
            End With
        End Sub
        Private Sub GroupDefaults_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            Labels.Clear()
            CH_TEMPORARY.Dispose()
            CH_FAV.Dispose()
            CH_READY_FOR_DOWN.Dispose()
            CH_READY_FOR_DOWN_IGNORE.Dispose()
            TXT_LABELS.Dispose()
            With TP_1
                .Controls.Clear()
                .RowStyles.Clear()
                .ColumnStyles.Clear()
                .Dispose()
            End With
            With TP_2
                .Controls.Clear()
                .RowStyles.Clear()
                .ColumnStyles.Clear()
                .Dispose()
            End With
        End Sub
        Protected Overrides Sub InitLayout()
            MyBase.InitLayout()
            If ColumnStyles.Count = 2 Or RowStyles.Count = 2 Then
                ColumnStyles.Clear()
                RowStyles.Clear()
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
                ColumnCount = 1
                ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
                RowCount = 7
                RowStyles.Add(New RowStyle(SizeType.Absolute, 25))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 28))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 25))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 25))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 28))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 28))
                RowStyles.Add(New RowStyle(SizeType.Percent, 100))
            End If
            Controls.Add(TXT_NAME, 0, 1)
            Controls.Add(TP_1, 0, 2)
            Controls.Add(TP_2, 0, 3)
            Controls.Add(TXT_LABELS, 0, 4)
            Controls.Add(TXT_SITES, 0, 5)
        End Sub
        Private Sub TXT_LABELS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles TXT_LABELS.ActionOnButtonClick
            Select Case Sender.DefaultButton
                Case ADB.Edit, ADB.Delete
                    With If(Sender.DefaultButton = ADB.Edit, Labels, LabelsExcluded)
                        Using f As New LabelsForm(.Self, True)
                            If Sender.DefaultButton = ADB.Delete Then f.Text &= " excluded"
                            f.ShowDialog()
                            If f.DialogResult = DialogResult.OK Then
                                .AsList.ListAddList(f.LabelsList, LAP.NotContainsOnly, LAP.ClearBeforeAdd)
                                UpdateLabelsText()
                            End If
                        End Using
                    End With
                Case ADB.Clear : Labels.Clear() : LabelsExcluded.Clear() : TXT_LABELS.Clear() : UpdateLabelsText()
            End Select
        End Sub
        Private Sub TXT_SITES_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles TXT_SITES.ActionOnButtonClick
            Select Case Sender.DefaultButton
                Case ADB.Edit, ADB.Delete
                    With If(Sender.DefaultButton = ADB.Edit, Sites, SitesExcluded)
                        Using f As New Editors.SiteSelectionForm(.Self)
                            If Sender.DefaultButton = ADB.Delete Then f.Text &= " excluded"
                            f.ShowDialog()
                            If f.DialogResult = DialogResult.OK Then
                                .Self.ListAddList(f.SelectedSites, LAP.NotContainsOnly, LAP.ClearBeforeAdd)
                                UpdateSitesText()
                            End If
                        End Using
                    End With
                Case ADB.Clear : Sites.Clear() : SitesExcluded.Clear() : TXT_SITES.Clear() : UpdateSitesText()
            End Select
        End Sub
        Private Sub UpdateLabelsText()
            TXT_LABELS.Clear()
            If Not _JustExcludeOptions Then TXT_LABELS.Text = Labels.ListToString
            If LabelsExcluded.Count > 0 Then TXT_LABELS.Text.StringAppend($"EXCLUDED: {LabelsExcluded.ListToString}", "; ")
        End Sub
        Private Sub UpdateSitesText()
            TXT_SITES.Clear()
            If Not _JustExcludeOptions Then TXT_SITES.Text = Sites.ListToString
            If SitesExcluded.Count > 0 Then TXT_SITES.Text.StringAppend($"EXCLUDED: {SitesExcluded.ListToString}", "; ")
        End Sub
        Friend Sub [Get](ByRef Instance As IGroup)
            If Not Instance Is Nothing Then
                With Instance
                    .Name = TXT_NAME.Text
                    .Temporary = CH_TEMPORARY.CheckState
                    .Favorite = CH_FAV.CheckState
                    .ReadyForDownload = CH_READY_FOR_DOWN.Checked
                    .ReadyForDownloadIgnore = CH_READY_FOR_DOWN_IGNORE.Checked
                    .Labels.Clear()
                    .Labels.ListAddList(Labels)
                    .LabelsExcluded.Clear()
                    .LabelsExcluded.ListAddList(LabelsExcluded)
                    .Sites.Clear()
                    .Sites.ListAddList(Sites)
                    .SitesExcluded.Clear()
                    .SitesExcluded.ListAddList(SitesExcluded)
                End With
            End If
        End Sub
        Friend Sub [Set](ByVal Instance As IGroup)
            If Not Instance Is Nothing Then
                With Instance
                    TXT_NAME.Text = .Name
                    CH_TEMPORARY.CheckState = .Temporary
                    CH_FAV.CheckState = .Favorite
                    CH_READY_FOR_DOWN.Checked = .ReadyForDownload
                    CH_READY_FOR_DOWN_IGNORE.Checked = .ReadyForDownloadIgnore

                    Labels.ListAddList(.Labels)
                    LabelsExcluded.ListAddList(.LabelsExcluded)
                    UpdateLabelsText()

                    Sites.ListAddList(.Sites)
                    SitesExcluded.ListAddList(.SitesExcluded)
                    UpdateSitesText()
                End With
            End If
        End Sub
        Private _Enabled As Boolean = True
        Private _JustExcludeOptions As Boolean = False
        Friend Overloads Property Enabled(Optional ByVal LeaveExcludeOptions As Boolean = False) As Boolean
            Get
                Return _Enabled
            End Get
            Set(ByVal e As Boolean)
                _Enabled = e
                _JustExcludeOptions = False
                TP_1.Enabled = e
                TP_2.Enabled = e
                If e Then
                    TXT_LABELS.Enabled = True
                    TXT_SITES.Enabled = True
                ElseIf LeaveExcludeOptions Then
                    _JustExcludeOptions = True
                    TXT_LABELS.Enabled = True
                    TXT_LABELS.Button(ADB.Edit).Enabled = False
                    TXT_LABELS.Button(ADB.Delete).Enabled = True
                    TXT_LABELS.Button(ADB.Clear).Enabled = False

                    TXT_SITES.Enabled = True
                    TXT_SITES.Button(ADB.Edit).Enabled = False
                    TXT_SITES.Button(ADB.Delete).Enabled = True
                    TXT_SITES.Button(ADB.Clear).Enabled = False
                Else
                    TXT_LABELS.Enabled = False
                    TXT_SITES.Enabled = False
                End If
                UpdateLabelsText()
                UpdateSitesText()
            End Set
        End Property
    End Class
End Namespace