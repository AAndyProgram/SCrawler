' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Namespace DownloadObjects.Groups
    Public Class GroupDefaults : Inherits TableLayoutPanel
#Region "Constants"
        Friend Const CaptionWidthDefault As Integer = 55
#End Region
#Region "Declarations"
        Private ReadOnly TP_1 As TableLayoutPanel 'CH_REGULAR, CH_TEMPORARY, CH_FAV
        Private ReadOnly TP_2 As TableLayoutPanel 'CH_READY_FOR_DOWN, CH_READY_FOR_DOWN_IGNORE
        Private ReadOnly TP_3 As TableLayoutPanel 'CH_USERS, CH_SUBSCRIPTIONS
        Private ReadOnly TP_4 As TableLayoutPanel 'CH_USER_EXISTS, CH_USER_SUSPENDED, CH_USER_DELETED
        Private ReadOnly TP_5 As TableLayoutPanel 'CH_LABELS_NO, CH_LABELS_EXCLUDED_IGNORE
        Private ReadOnly TP_6 As TableLayoutPanel 'CH_DATE_IN_RANGE, DT_FROM, DT_TO

        Private ReadOnly CH_REGULAR As CheckBox
        Private ReadOnly CH_TEMPORARY As CheckBox
        Private ReadOnly CH_FAV As CheckBox

        Private ReadOnly CH_READY_FOR_DOWN As CheckBox
        Private ReadOnly CH_READY_FOR_DOWN_IGNORE As CheckBox

        Private ReadOnly CH_USERS As CheckBox
        Private ReadOnly CH_SUBSCRIPTIONS As CheckBox

        Private ReadOnly CH_USER_EXISTS As CheckBox
        Private ReadOnly CH_USER_SUSPENDED As CheckBox
        Private ReadOnly CH_USER_DELETED As CheckBox

        Private ReadOnly CH_LABELS_NO As CheckBox
        Private ReadOnly CH_LABELS_EXCLUDED_IGNORE As CheckBox

        Private ReadOnly DT_FROM As TextBoxExtended
        Private ReadOnly DT_TO As TextBoxExtended
        Private ReadOnly CH_DATE_IN_RANGE As CheckBox

        Private WithEvents NUM_USERS_COUNT As TextBoxExtended
        Private WithEvents NUM_DAYS As TextBoxExtended

        Private WithEvents TXT_LABELS As TextBoxExtended
        Private WithEvents TXT_SITES As TextBoxExtended
        Friend WithEvents TXT_NAME As TextBoxExtended

        Private ReadOnly Labels As List(Of String)
        Private ReadOnly LabelsExcluded As List(Of String)
        Private ReadOnly Sites As List(Of String)
        Private ReadOnly SitesExcluded As List(Of String)
        Private ReadOnly TT_MAIN As ToolTip
#End Region
#Region "Initializer"
        Public Sub New()
            Labels = New List(Of String)
            LabelsExcluded = New List(Of String)
            Sites = New List(Of String)
            SitesExcluded = New List(Of String)
            TT_MAIN = New ToolTip

            InitTextBox(TXT_LABELS, "Labels", {New ActionButton(ADB.Edit) With {.ToolTipText = "Edit selected labels"},
                                               New ActionButton(ADB.Delete) With {.ToolTipText = "Edit excluded labels"}, ADB.Clear})
            TXT_LABELS.TextBoxReadOnly = True

            InitTextBox(TXT_SITES, "Sites", {New ActionButton(ADB.Edit) With {.ToolTipText = "Edit selected sites"},
                                             New ActionButton(ADB.Delete) With {.ToolTipText = "Edit excluded sites"}, ADB.Clear})
            TXT_SITES.TextBoxReadOnly = True

            InitTextBox(TXT_NAME, "Name", {ADB.Clear})

            CH_REGULAR = New CheckBox With {.Text = "Regular", .Name = "CH_REGULAR", .Checked = True, .Dock = DockStyle.Fill}
            TT_MAIN.SetToolTip(CH_REGULAR, "Users not marked as temporary or favorite")
            CH_TEMPORARY = New CheckBox With {.Text = "Temporary", .Name = "CH_TEMPORARY", .Checked = True, .Dock = DockStyle.Fill}
            TT_MAIN.SetToolTip(CH_TEMPORARY, "Users marked as temporary")
            CH_FAV = New CheckBox With {.Text = "Favorite", .Name = "CH_FAV", .Checked = True, .Dock = DockStyle.Fill}
            TT_MAIN.SetToolTip(CH_FAV, "Users marked as favorite")
            TP_1 = New TableLayoutPanel With {.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single, .Margin = New Padding(0), .Dock = DockStyle.Fill}
            FillTP(TP_1, CH_REGULAR, CH_TEMPORARY, CH_FAV)

            CH_READY_FOR_DOWN = New CheckBox With {.Text = "Ready for download", .Name = "CH_READY_FOR_DOWN", .Checked = True, .Dock = DockStyle.Fill}
            TT_MAIN.SetToolTip(CH_READY_FOR_DOWN, "Users marked as 'Ready for download'")
            CH_READY_FOR_DOWN_IGNORE = New CheckBox With {.Text = "Ignore ready for download", .Name = "CH_READY_FOR_DOWN_IGNORE", .Checked = False, .Dock = DockStyle.Fill}
            TT_MAIN.SetToolTip(CH_READY_FOR_DOWN_IGNORE, "Ignore the 'Ready for download' mark")
            TP_2 = New TableLayoutPanel With {.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single, .Margin = New Padding(0), .Dock = DockStyle.Fill}
            FillTP(TP_2, CH_READY_FOR_DOWN, CH_READY_FOR_DOWN_IGNORE)

            CH_USERS = New CheckBox With {.Text = "Users", .Name = "CH_USERS", .Checked = True, .Dock = DockStyle.Fill}
            TT_MAIN.SetToolTip(CH_USERS, "Download/filter users")
            CH_SUBSCRIPTIONS = New CheckBox With {.Text = "Subscriptions", .Name = "CH_SUBSCRIPTIONS", .Checked = False, .Dock = DockStyle.Fill}
            TT_MAIN.SetToolTip(CH_SUBSCRIPTIONS, "Download/filter subscriptions")
            TP_3 = New TableLayoutPanel With {.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single, .Margin = New Padding(0), .Dock = DockStyle.Fill}
            FillTP(TP_3, CH_USERS, CH_SUBSCRIPTIONS)

            CH_USER_EXISTS = New CheckBox With {.Text = "User exists", .Name = "CH_USER_EXISTS", .Checked = True, .Dock = DockStyle.Fill}
            TT_MAIN.SetToolTip(CH_USER_EXISTS, "Include users not marked as 'Suspended' or 'Deleted'")
            CH_USER_SUSPENDED = New CheckBox With {.Text = "User suspended", .Name = "CH_USER_SUSPENDED", .Checked = True, .Dock = DockStyle.Fill,
                                                   .BackColor = MyColor.EditBack, .ForeColor = MyColor.EditFore}
            TT_MAIN.SetToolTip(CH_USER_SUSPENDED, "Include users marked as 'Suspended'")
            CH_USER_DELETED = New CheckBox With {.Text = "User deleted", .Name = "CH_USER_DELETED", .Checked = False, .Dock = DockStyle.Fill,
                                                 .BackColor = MyColor.DeleteBack, .ForeColor = MyColor.DeleteFore}
            TT_MAIN.SetToolTip(CH_USER_DELETED, "Include users marked as 'Deleted'")
            TP_4 = New TableLayoutPanel With {.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single, .Margin = New Padding(0), .Dock = DockStyle.Fill}
            FillTP(TP_4, CH_USER_EXISTS, CH_USER_SUSPENDED, CH_USER_DELETED)

            CH_LABELS_NO = New CheckBox With {.Text = "No labels", .Name = "CH_LABELS_NO", .Checked = False, .Dock = DockStyle.Fill}
            TT_MAIN.SetToolTip(CH_LABELS_NO, "Only users that have no labels at all." & vbCr & "If checked, the list of labels will be ignored!")
            CH_LABELS_EXCLUDED_IGNORE = New CheckBox With {.Text = "Ignore excluded labels", .Name = "CH_LABELS_EXCLUDED_IGNORE", .Checked = False, .Dock = DockStyle.Fill}
            TT_MAIN.SetToolTip(CH_LABELS_EXCLUDED_IGNORE, "Ignore excluded labels if they exist")
            TP_5 = New TableLayoutPanel With {.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single, .Margin = New Padding(0), .Dock = DockStyle.Fill}
            FillTP(TP_5, CH_LABELS_NO, CH_LABELS_EXCLUDED_IGNORE)

            Dim initDtTxt As Action(Of TextBoxExtended, String, String) =
                Sub(ByVal cnt As TextBoxExtended, ByVal captionText As String, ByVal toolTip As String)
                    With cnt
                        .BeginInit()
                        .CaptionText = captionText
                        .CaptionToolTipText = toolTip
                        .CaptionToolTipEnabled = True
                        .CaptionWidth = 30
                        .ControlMode = TextBoxExtended.ControlModes.DateTimePicker
                        .DateShowCheckBox = True
                        .DateMin = DateTimePicker.MinimumDateTime
                        .DateMax = DateTimePicker.MaximumDateTime
                        .DateChecked = False
                        .DateShowUpDown = False
                        .CaptionMargin = New PaddingE(.CaptionMargin).Add(, 1)
                        .Dock = DockStyle.Fill
                        .Value = Now.Date
                        .EndInit()
                    End With
                End Sub
            DT_FROM = New TextBoxExtended
            initDtTxt.Invoke(DT_FROM, "From", "Minimum date")
            DT_TO = New TextBoxExtended
            initDtTxt.Invoke(DT_TO, "To", "Maximum date")
            CH_DATE_IN_RANGE = New CheckBox With {.Text = "In range", .Name = "CH_DATE_IN_RANGE", .Checked = True, .Dock = DockStyle.Fill}
            TT_MAIN.SetToolTip(CH_DATE_IN_RANGE, "Last download date range." & vbCr &
                                                 "If checked, filter users whose last download date is within the selected date range." & vbCr &
                                                 "If unchecked, filter users whose last download date is outside the selected date range." & vbCr &
                                                 "If no dates are checked, this option will be ignored.")
            CH_DATE_IN_RANGE.Margin = New PaddingE(CH_DATE_IN_RANGE.Margin).Add(, 2,, -2)
            TP_6 = New TableLayoutPanel With {.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single, .Margin = New Padding(0), .Dock = DockStyle.Fill}

            With TP_6
                .ColumnCount = 3
                With .ColumnStyles
                    .Add(New ColumnStyle(SizeType.Absolute, 80))
                    .Add(New ColumnStyle(SizeType.Percent, 50))
                    .Add(New ColumnStyle(SizeType.Percent, 50))
                End With
                .RowCount = 1
                .RowStyles.Add(New RowStyle(SizeType.Percent, 100))
                With .Controls
                    .Add(CH_DATE_IN_RANGE, 0, 0)
                    .Add(DT_FROM, 1, 0)
                    .Add(DT_TO, 2, 0)
                End With
            End With

            NUM_USERS_COUNT = New TextBoxExtended
            With NUM_USERS_COUNT
                .BeginInit()
                .CaptionText = "Users"
                .CaptionToolTipText = "The number of users that to be downloaded." & vbCr &
                                      "The number is 0 = all users." & vbCr &
                                      "Number greater than 0 = number of users from the beginning to the end of the list." & vbCr &
                                      "Number less than 0 = number of users from end to the beginning of the list."
                .CaptionToolTipEnabled = True
                .CaptionWidth = CaptionWidthDefault
                .ControlMode = TextBoxExtended.ControlModes.NumericUpDown
                .NumberMinimum = Integer.MinValue
                .NumberMaximum = Integer.MaxValue
                .NumberUpDownAlign = LeftRightAlignment.Left
                .Dock = DockStyle.Fill
                .Buttons.Add(New ActionButton(ADB.Clear) With {.ToolTipText = "Reset value"})
                .ClearTextByButtonClear = False
                .Value = 0
                .EndInit()
            End With
            NUM_DAYS = New TextBoxExtended
            With NUM_DAYS
                .BeginInit()
                .CaptionText = "Down"
                .CaptionToolTipText = "Filter users who have been (not)downloaded in the last x days." & vbCr &
                                      "-1 to disable" & vbCr &
                                      "Checked = downloaded in the last x days" & vbCr &
                                      "Unchecked = NOT downloaded in the last x days"
                .CaptionToolTipEnabled = True
                .CaptionMode = ICaptionControl.Modes.CheckBox
                .CaptionCheckAlign = ContentAlignment.MiddleLeft
                .CaptionMargin = New PaddingE(.CaptionMargin).Add(1)
                .CaptionWidth = CaptionWidthDefault
                .ChangeControlsEnableOnCheckedChange = False
                .ControlMode = TextBoxExtended.ControlModes.NumericUpDown
                .NumberMinimum = -1
                .NumberMaximum = Integer.MaxValue
                .NumberUpDownAlign = LeftRightAlignment.Left
                .Dock = DockStyle.Fill
                .Buttons.Add(New ActionButton(ADB.Clear) With {.ToolTipText = "Reset value"})
                .ClearTextByButtonClear = False
                .Value = -1
                .EndInit()
            End With
        End Sub
        Private Sub InitTextBox(ByRef TXT As TextBoxExtended, ByVal Caption As String, ByVal Buttons As ActionButton())
            TXT = New TextBoxExtended
            With TXT
                .BeginInit()
                .Buttons.AddRange(Buttons)
                .CaptionText = Caption
                .CaptionWidth = CaptionWidthDefault
                .Dock = DockStyle.Fill
                .EndInit()
            End With
        End Sub
        Private Sub FillTP(ByRef TP As TableLayoutPanel, ByVal ParamArray CNT As Control())
            With TP
                Dim i%
                .ColumnCount = CNT.Count
                For i = 0 To CNT.Count - 1 : .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50)) : Next
                .RowCount = 1
                .RowStyles.Add(New RowStyle(SizeType.Percent, 100))
                With .Controls
                    For i = 0 To CNT.Count - 1 : .Add(CNT(i), i, 0) : Next
                End With
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
                RowCount = 13
                RowStyles.Add(New RowStyle(SizeType.Absolute, 25))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 28))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 25))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 25))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 25))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 25))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 25))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 28))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 28))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 25))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 28))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 28))
                RowStyles.Add(New RowStyle(SizeType.Percent, 100))
            End If
            Controls.Add(TXT_NAME, 0, 1)
            Controls.Add(TP_1, 0, 2)
            Controls.Add(TP_4, 0, 3)
            Controls.Add(TP_2, 0, 4)
            Controls.Add(TP_3, 0, 5)
            Controls.Add(TP_6, 0, 6)

            Controls.Add(NUM_DAYS, 0, 5)
            Controls.Add(NUM_USERS_COUNT, 0, 8)

            Controls.Add(TP_5, 0, 9)

            Controls.Add(TXT_LABELS, 0, 10)
            Controls.Add(TXT_SITES, 0, 11)
        End Sub
#End Region
#Region "Control handlers"
        Private Sub GroupDefaults_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            Labels.Clear()
            LabelsExcluded.Clear()
            Sites.Clear()
            SitesExcluded.Clear()
            CH_REGULAR.Dispose()
            CH_TEMPORARY.Dispose()
            CH_FAV.Dispose()
            CH_READY_FOR_DOWN.Dispose()
            CH_READY_FOR_DOWN_IGNORE.Dispose()
            CH_USERS.Dispose()
            CH_SUBSCRIPTIONS.Dispose()
            CH_USER_EXISTS.Dispose()
            CH_USER_SUSPENDED.Dispose()
            CH_USER_DELETED.Dispose()
            CH_LABELS_NO.Dispose()
            CH_LABELS_EXCLUDED_IGNORE.Dispose()
            DT_FROM.Dispose()
            DT_TO.Dispose()
            CH_DATE_IN_RANGE.Dispose()
            NUM_USERS_COUNT.Dispose()
            NUM_DAYS.Dispose()
            TXT_LABELS.Dispose()
            TXT_SITES.Dispose()
            TXT_NAME.Dispose()
            TT_MAIN.Dispose()
            ClearTP(TP_1)
            ClearTP(TP_2)
            ClearTP(TP_3)
            ClearTP(TP_4)
            ClearTP(TP_5)
            ClearTP(TP_6)
        End Sub
        Private Sub ClearTP(ByRef TP As TableLayoutPanel)
            With TP
                .Controls.Clear()
                .RowStyles.Clear()
                .ColumnStyles.Clear()
                .Dispose()
            End With
        End Sub
#End Region
#Region "Controls"
        Friend Sub HideName()
            Controls.Remove(TXT_NAME)
            RowStyles(1).Height = 0
        End Sub
        Private Sub NUM_USERS_COUNT_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles NUM_USERS_COUNT.ActionOnButtonClick
            If Sender.DefaultButton = ADB.Clear Then NUM_USERS_COUNT.Value = 0
        End Sub
        Private Sub NUM_DAYS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles NUM_DAYS.ActionOnButtonClick
            If Sender.DefaultButton = ADB.Clear Then NUM_DAYS.Value = -1
        End Sub
        Private Sub TXT_LABELS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles TXT_LABELS.ActionOnButtonClick
            Select Case Sender.DefaultButton
                Case ADB.Edit, ADB.Delete
                    With If(Sender.DefaultButton = ADB.Edit, Labels, LabelsExcluded)
                        Using f As New LabelsForm(.Self)
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
#End Region
#Region "Get/set"
        Friend Sub [Get](ByRef Instance As IGroup)
            If Not Instance Is Nothing Then
                With Instance
                    .Name = TXT_NAME.Text
                    .Regular = CH_REGULAR.Checked
                    .Temporary = CH_TEMPORARY.Checked
                    .Favorite = CH_FAV.Checked
                    .ReadyForDownload = CH_READY_FOR_DOWN.Checked
                    .ReadyForDownloadIgnore = CH_READY_FOR_DOWN_IGNORE.Checked
                    .DownloadUsers = CH_USERS.Checked
                    .DownloadSubscriptions = CH_SUBSCRIPTIONS.Checked
                    .UsersCount = NUM_USERS_COUNT.Value
                    .DaysNumber = NUM_DAYS.Value
                    .DaysIsDownloaded = NUM_DAYS.Checked
                    .UserDeleted = CH_USER_DELETED.Checked
                    .UserSuspended = CH_USER_SUSPENDED.Checked
                    .UserExists = CH_USER_EXISTS.Checked

                    If DT_FROM.DateChecked Then .DateFrom = DT_FROM.Date.Date Else .DateFrom = Nothing
                    If DT_TO.DateChecked Then
                        With DT_TO.Date.Date : Instance.DateTo = New Date(.Year, .Month, .Day, 23, 59, 59) : End With
                    Else
                        .DateTo = Nothing
                    End If
                    If .DateFrom.HasValue Or .DateTo.HasValue Then
                        .DateMode = If(CH_DATE_IN_RANGE.Checked, ShowingDates.In, ShowingDates.Not)
                    Else
                        .DateMode = ShowingDates.Off
                    End If

                    .LabelsNo = CH_LABELS_NO.Checked
                    .LabelsExcludedIgnore = CH_LABELS_EXCLUDED_IGNORE.Checked

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
                    CH_REGULAR.Checked = .Regular
                    CH_TEMPORARY.Checked = .Temporary
                    CH_FAV.Checked = .Favorite
                    CH_READY_FOR_DOWN.Checked = .ReadyForDownload
                    CH_READY_FOR_DOWN_IGNORE.Checked = .ReadyForDownloadIgnore
                    CH_USERS.Checked = .DownloadUsers
                    CH_SUBSCRIPTIONS.Checked = .DownloadSubscriptions
                    NUM_USERS_COUNT.Value = .UsersCount
                    NUM_DAYS.Value = .DaysNumber
                    NUM_DAYS.Checked = .DaysIsDownloaded
                    CH_USER_DELETED.Checked = .UserDeleted
                    CH_USER_SUSPENDED.Checked = .UserSuspended
                    CH_USER_EXISTS.Checked = .UserExists

                    If .DateFrom.HasValue Then
                        DT_FROM.DateChecked = True
                        DT_FROM.Value = .DateFrom.Value
                    Else
                        DT_FROM.DateChecked = False
                        DT_FROM.Value = Now.Date
                    End If
                    If .DateTo.HasValue Then
                        DT_TO.DateChecked = True
                        DT_TO.Value = .DateTo.Value
                    Else
                        DT_TO.DateChecked = False
                        DT_TO.Value = Now.Date
                    End If
                    CH_DATE_IN_RANGE.Checked = .DateMode = ShowingDates.In

                    CH_LABELS_NO.Checked = .LabelsNo
                    CH_LABELS_EXCLUDED_IGNORE.Checked = .LabelsExcludedIgnore

                    Labels.ListAddList(.Labels)
                    LabelsExcluded.ListAddList(.LabelsExcluded)
                    UpdateLabelsText()

                    Sites.ListAddList(.Sites)
                    SitesExcluded.ListAddList(.SitesExcluded)
                    UpdateSitesText()
                End With
            End If
        End Sub
#End Region
#Region "Enabled"
        Private _Enabled As Boolean = True
        Private _JustExcludeOptions As Boolean = False
        Friend Overloads Property Enabled() As Boolean
            Get
                Return _Enabled
            End Get
            Set(ByVal e As Boolean)
                _Enabled = e
                _JustExcludeOptions = False
                TP_1.Enabled = e
                TP_2.Enabled = e
                TP_3.Enabled = e
                TP_4.Enabled = e
                TP_5.Enabled = e
                TP_6.Enabled = e
                NUM_USERS_COUNT.Enabled = e
                NUM_DAYS.Enabled = e
                TXT_LABELS.Enabled = e
                TXT_SITES.Enabled = e
                UpdateLabelsText()
                UpdateSitesText()
            End Set
        End Property
#End Region
    End Class
End Namespace