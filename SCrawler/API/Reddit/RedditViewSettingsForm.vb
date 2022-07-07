' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Toolbars
Imports CView = SCrawler.API.Reddit.IRedditView.View
Imports CPeriod = SCrawler.API.Reddit.IRedditView.Period
Namespace API.Reddit
    Friend Class RedditViewSettingsForm : Implements IOkCancelToolbar
        Private ReadOnly MyDefs As DefaultFormOptions
        Private ReadOnly Property MyOptions As IRedditView
        Friend Sub New(ByRef opt As IRedditView)
            InitializeComponent()
            MyOptions = opt
            MyDefs = New DefaultFormOptions
        End Sub
        Private Sub ChannelSettingsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                Dim n$ = String.Empty
                If TypeOf MyOptions Is Channel Then
                    n = $"Channel [{DirectCast(MyOptions, Channel).Name}]"
                ElseIf TypeOf MyOptions Is Base.IUserData Then
                    n = $"User [{DirectCast(MyOptions, Base.IUserData).Name}]"
                End If
                If Not n.IsEmptyString Then Text = n
                With MyDefs
                    .MyViewInitialize(Me, Settings.Design, True)
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
                    .EndLoaderOperations()
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub OK() Implements IOkCancelToolbar.OK
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
            End With
            MyDefs.CloseForm()
        End Sub
        Private Sub Cancel() Implements IOkCancelToolbar.Cancel
            MyDefs.CloseForm(DialogResult.Cancel)
        End Sub
        Private Sub OPT_VIEW_MODE_NEW_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_VIEW_MODE_NEW.CheckedChanged
            ChangePeriodEnabled()
        End Sub
        Private Sub OPT_VIEW_MODE_HOT_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_VIEW_MODE_HOT.CheckedChanged
            ChangePeriodEnabled()
        End Sub
        Private Sub OPT_VIEW_MODE_TOP_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_VIEW_MODE_TOP.CheckedChanged
            ChangePeriodEnabled()
        End Sub
        Private Sub ChangePeriodEnabled()
            TP_PERIOD.Enabled = OPT_VIEW_MODE_TOP.Checked
        End Sub
    End Class
End Namespace