' Copyright (C) 2022  Andy
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
        Private ReadOnly Labels As List(Of String)
        Public Sub New()
            Labels = New List(Of String)
            TXT_LABELS = New TextBoxExtended
            With TXT_LABELS
                .BeginInit()
                .Buttons.AddRange({ADB.Edit, ADB.Clear})
                .CaptionText = "Labels"
                .CaptionWidth = 50
                .Dock = DockStyle.Fill
                .EndInit()
            End With
            CH_TEMPORARY = New CheckBox With {.Text = "Temporary", .Name = "CH_TEMPORARY", .ThreeState = True, .CheckState = CheckState.Indeterminate, .Dock = DockStyle.Fill}
            CH_FAV = New CheckBox With {.Text = "Favorite", .Name = "CH_FAV", .ThreeState = True, .CheckState = CheckState.Indeterminate, .Dock = DockStyle.Fill}
            CH_READY_FOR_DOWN = New CheckBox With {.Text = "Ready for download", .Name = "CH_READY_FOR_DOWN", .Checked = True, .Dock = DockStyle.Fill}
            CH_READY_FOR_DOWN_IGNORE = New CheckBox With {.Text = "Ignore ready for download", .Name = "CH_READY_FOR_DOWN_IGNORE", .Checked = False, .Dock = DockStyle.Fill}
            TP_1 = New TableLayoutPanel With {.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single, .Margin = New Padding(0), .Dock = DockStyle.Fill}
            FillTP(TP_1, CH_TEMPORARY, CH_FAV)
            TP_2 = New TableLayoutPanel With {.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single, .Margin = New Padding(0), .Dock = DockStyle.Fill}
            FillTP(TP_2, CH_READY_FOR_DOWN, CH_READY_FOR_DOWN_IGNORE)
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
                RowCount = 5
                RowStyles.Add(New RowStyle(SizeType.Absolute, 25))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 25))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 25))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 28))
                RowStyles.Add(New RowStyle(SizeType.Percent, 100))
            End If
            Controls.Add(TP_1, 0, 1)
            Controls.Add(TP_2, 0, 2)
            Controls.Add(TXT_LABELS, 0, 3)
        End Sub
        Private Sub TXT_LABELS_ActionOnButtonClick(ByVal Sender As ActionButton) Handles TXT_LABELS.ActionOnButtonClick
            Select Case Sender.DefaultButton
                Case ADB.Edit
                    Using f As New LabelsForm(Labels)
                        f.ShowDialog()
                        If f.DialogResult = DialogResult.OK Then
                            Labels.ListAddList(f.LabelsList, LAP.NotContainsOnly, LAP.ClearBeforeAdd)
                            TXT_LABELS.Clear()
                            TXT_LABELS.Text = Labels.ListToString
                        End If
                    End Using
                Case ADB.Clear : Labels.Clear()
            End Select
        End Sub
        Friend Sub [Get](ByRef Instance As IGroup)
            If Not Instance Is Nothing Then
                With Instance
                    .Temporary = CH_TEMPORARY.CheckState
                    .Favorite = CH_FAV.CheckState
                    .ReadyForDownload = CH_READY_FOR_DOWN.Checked
                    .ReadyForDownloadIgnore = CH_READY_FOR_DOWN_IGNORE.Checked
                    .Labels.Clear()
                    .Labels.ListAddList(Labels)
                End With
            End If
        End Sub
        Friend Sub [Set](ByVal Instance As IGroup)
            If Not Instance Is Nothing Then
                With Instance
                    CH_TEMPORARY.CheckState = .Temporary
                    CH_FAV.CheckState = .Favorite
                    CH_READY_FOR_DOWN.Checked = .ReadyForDownload
                    CH_READY_FOR_DOWN_IGNORE.Checked = .ReadyForDownloadIgnore
                    Labels.ListAddList(.Labels)
                    TXT_LABELS.Text = Labels.ListToString
                End With
            End If
        End Sub
        Private _Enabled As Boolean = True
        Friend Shadows Property Enabled As Boolean
            Get
                Return _Enabled
            End Get
            Set(ByVal e As Boolean)
                _Enabled = e
                TP_1.Enabled = e
                TP_2.Enabled = e
                TXT_LABELS.Enabled = e
            End Set
        End Property
    End Class
End Namespace