' Copyright (C) 2022  Andy
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
        Private MyView As FormsView
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
            MyView = New FormsView(Me)
            MyView.ImportFromXML(Settings.Design)
            MyView.SetMeSize()
            Opened = True
        End Sub
        Private Sub ActiveDownloadingProgress_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            MyView.ExportToXML(Settings.Design)
            e.Cancel = True
            Hide()
        End Sub
        Private Sub Downloader_Reconfigured()
            Const RowHeight% = 30
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
                                                  .Controls.Add(JobsList.Last.Get, 0, .RowStyles.Count - 1)
                                              End With
                                          Next
                                          TP_MAIN.RowStyles.Add(New RowStyle(SizeType.Percent, 100))
                                          TP_MAIN.RowCount += 1
                                      End If
                                      Dim s As Size = Size
                                      s.Height = TP_MAIN.RowStyles.Count * RowHeight + PaddingE.GetOf({TP_MAIN}).Vertical(TP_MAIN.RowStyles.Count) - TP_MAIN.RowStyles.Count * 2
                                      MinimumSize = New Size(MinWidth, s.Height)
                                      Size = s
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
    End Class
End Namespace