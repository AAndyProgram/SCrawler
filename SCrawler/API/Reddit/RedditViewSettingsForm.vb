' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin.Hosts
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Imports CView = SCrawler.API.Reddit.IRedditView.View
Imports CPeriod = SCrawler.API.Reddit.IRedditView.Period
Namespace API.Reddit
    Friend Class RedditViewSettingsForm
        Private WithEvents MyDefs As DefaultFormOptions
        Private ReadOnly Property MyOptions As IRedditView
        Private ReadOnly Property IsUserSettings As Boolean
        Friend Sub New(ByRef opt As IRedditView, ByVal _IsUserSettings As Boolean)
            InitializeComponent()
            MyOptions = opt
            IsUserSettings = _IsUserSettings
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
        End Sub
        Private Sub RedditViewSettingsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                Dim n$ = String.Empty
                If TypeOf MyOptions Is Channel Then
                    n = $"Channel [{DirectCast(MyOptions, Channel).Name}]"
                ElseIf TypeOf MyOptions Is Base.IUserData Then
                    n = $"User [{DirectCast(MyOptions, Base.IUserData).Name}]"
                End If
                If Not n.IsEmptyString Then Text = n
                With MyDefs
                    .MyViewInitialize(True)
                    .AddOkCancelToolbar()
                    Select Case MyOptions.ViewMode
                        Case CView.Hot : OPT_VIEW_MODE_HOT.Checked = True
                        Case CView.Top : OPT_VIEW_MODE_TOP.Checked = True
                        Case Else : OPT_VIEW_MODE_NEW.Checked = True
                    End Select
                    Select Case MyOptions.ViewPeriod
                        Case CPeriod.Hour : OPT_PERIOD_HOUR.Checked = True
                        Case CPeriod.Day : OPT_PERIOD_DAY.Checked = True
                        Case CPeriod.Week : OPT_PERIOD_WEEK.Checked = True
                        Case CPeriod.Month : OPT_PERIOD_MONTH.Checked = True
                        Case CPeriod.Year : OPT_PERIOD_YEAR.Checked = True
                        Case Else : OPT_PERIOD_ALL.Checked = True
                    End Select
                    ChangePeriodEnabled()

                    PopulateCMB(Settings(RedditSiteKey), CMB_REDDIT_ACC, MyOptions.RedditAccount)
                    PopulateCMB(Settings(RedGifs.RedGifsSiteKey), CMB_REDGIFS_ACC, MyOptions.RedGifsAccount)
                    If IsUserSettings Then
                        TP_MAIN.Controls.Remove(CMB_REDDIT_ACC)
                        TP_MAIN.RowStyles(2).Height = 0
                        TP_MAIN.Refresh()
                        Dim s As Size = Size
                        s.Height -= 28
                        MaximumSize = Nothing
                        MinimumSize = Nothing
                        Size = s
                        MinimumSize = s
                        MaximumSize = s
                        Refresh()
                    End If

                    .EndLoaderOperations()
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub PopulateCMB(ByVal Plugin As SettingsHostCollection, ByRef CMB As ComboBoxExtended, ByVal Acc As String)
            With CMB
                Dim indx% = 0
                .BeginUpdate()
                If Plugin.Count = 1 Then
                    .Text = SettingsHost.NameAccountNameDefault
                    .LeaveDefaultButtons = False
                    .Buttons.Clear()
                    .Buttons.UpdateButtonsPositions(True)
                    .CaptionWidth -= 1
                Else
                    Dim data As List(Of String) = Plugin.Select(Function(h) h.AccountName.IfNullOrEmpty(SettingsHost.NameAccountNameDefault)).ToList
                    If Not Acc.IsEmptyString Then
                        indx = data.IndexOf(Acc)
                        If indx = -1 Then indx = 0
                    End If
                    .Items.AddRange(data.Select(Function(d) New ListItem(d)))
                End If
                .EndUpdate(True)
                If .Count > 0 Then .SelectedIndex = indx
                .Enabled = .Count > 1
            End With
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            With MyOptions
                Select Case True
                    Case OPT_VIEW_MODE_HOT.Checked : .ViewMode = CView.Hot
                    Case OPT_VIEW_MODE_TOP.Checked : .ViewMode = CView.Top
                    Case Else : .ViewMode = CView.New
                End Select
                Select Case True
                    Case OPT_PERIOD_HOUR.Checked : .ViewPeriod = CPeriod.Hour
                    Case OPT_PERIOD_DAY.Checked : .ViewPeriod = CPeriod.Day
                    Case OPT_PERIOD_WEEK.Checked : .ViewPeriod = CPeriod.Week
                    Case OPT_PERIOD_MONTH.Checked : .ViewPeriod = CPeriod.Month
                    Case OPT_PERIOD_YEAR.Checked : .ViewPeriod = CPeriod.Year
                    Case Else : .ViewPeriod = CPeriod.All
                End Select
                .RedGifsAccount = CMB_REDGIFS_ACC.Text
                If Not IsUserSettings Then .RedditAccount = CMB_REDDIT_ACC.Text
            End With
            MyDefs.CloseForm()
        End Sub
        Private Sub ChangePeriodEnabled() Handles OPT_VIEW_MODE_NEW.CheckedChanged, OPT_VIEW_MODE_HOT.CheckedChanged, OPT_VIEW_MODE_TOP.CheckedChanged
            TP_PERIOD.Enabled = OPT_VIEW_MODE_TOP.Checked
        End Sub
    End Class
End Namespace