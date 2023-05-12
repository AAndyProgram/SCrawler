' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports PersonalUtilities.Forms
Namespace DownloadObjects
    Friend Class ActiveDownloadingProgress
        Private Const MinWidth As Integer = 450
        Private MyView As FormView
        Private Opened As Boolean = False
        Friend ReadOnly Property ReadyToOpen As Boolean
            Get
                Return Settings.DownloadOpenProgress And (Not Opened Or Settings.DownloadOpenProgress.Attribute) And Not Visible
            End Get
        End Property
        Private ReadOnly JobsList As List(Of DownloadProgress)
        Friend Property DisableProgressChange As Boolean = False
        Friend Sub New()
            InitializeComponent()
            JobsList = New List(Of DownloadProgress)
            AddHandler Downloader.Reconfigured, AddressOf Downloader_Reconfigured
            Downloader_Reconfigured()
        End Sub
        Private Sub ActiveDownloadingProgress_Load(sender As Object, e As EventArgs) Handles Me.Load
            MyView = New FormView(Me)
            MyView.Import(Settings.Design)
            MyView.SetFormSize()
            Opened = True
        End Sub
        Private Sub ActiveDownloadingProgress_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            MyView.Export(Settings.Design)
            e.Cancel = True
            Hide()
        End Sub
        Private Sub ActiveDownloadingProgress_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            MyView.DisposeIfReady()
        End Sub
        Private Sub Downloader_Reconfigured()
            Const RowHeight% = 30
            Const LowestValue% = 39
            Dim a As Action = Sub()
                                  With TP_MAIN
                                      If .Controls.Count > 0 Then
                                          For Each c As Control In .Controls
                                              If Not c Is Nothing Then c.Dispose()
                                          Next
                                          .Controls.Clear()
                                      End If
                                      .RowStyles.Clear()
                                      .RowCount = 0
                                  End With
                                  JobsList.ListClearDispose
                                  With Downloader
                                      If .Pool.Count > 0 Then
                                          For Each j As TDownloader.Job In .Pool
                                              With TP_MAIN
                                                  .RowStyles.Add(New RowStyle(SizeType.Absolute, RowHeight))
                                                  .RowCount += 1
                                                  JobsList.Add(New DownloadProgress(j))
                                                  AddHandler JobsList.Last.ProgressMaximumChanged, AddressOf Jobs_ProgressMaximumChanged
                                                  AddHandler JobsList.Last.ProgressMaximum0Changed, AddressOf Jobs_ProgressMaximum0Changed
                                                  .Controls.Add(JobsList.Last.Get, 0, .RowStyles.Count - 1)
                                              End With
                                          Next
                                          TP_MAIN.RowStyles.Add(New RowStyle(SizeType.AutoSize))
                                          TP_MAIN.RowCount += 1

                                          Dim s As Size = Size
                                          Dim ss As Size = Screen.PrimaryScreen.WorkingArea.Size
                                          Dim c% = TP_MAIN.RowStyles.Count - 1
                                          s.Height = c * RowHeight + LowestValue + (PaddingE.GetOf({TP_MAIN}).Vertical(c) / c).RoundDown
                                          If s.Height > ss.Height Then s.Height = ss.Height
                                          MinimumSize = Nothing
                                          Size = s
                                          MinimumSize = New Size(MinWidth, s.Height)
                                      End If
                                  End With
                                  TP_MAIN.Refresh()
                              End Sub
            If TP_MAIN.InvokeRequired Then TP_MAIN.Invoke(a) Else a.Invoke
        End Sub
        Private Sub Jobs_ProgressMaximumChanged()
            If JobsList.Count > 0 And Not DisableProgressChange Then
                MainProgress.Maximum = JobsList.Sum(Function(j) CLng(j.Job.Progress.Maximum))
                MainProgress.Value = Math.Max(JobsList.Sum(Function(j) CLng(j.Job.Progress.Value)) - 1, 0)
                If MainProgress.Value > 0 Then MainProgress.Perform()
            End If
        End Sub
        Private Sub Jobs_ProgressMaximum0Changed()
            If JobsList.Count > 0 And Not DisableProgressChange Then _
               MainProgress.Maximum0 = JobsList.Sum(Function(j) CLng(DirectCast(j.Job.Progress, MyProgressExt).Maximum0))
        End Sub
    End Class
End Namespace