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
Imports SCrawler.DownloadObjects
Imports SCrawler.Plugin.Hosts
Friend Class DownloadSavedPostsForm
    Friend Event DownloadDone As NotificationEventHandler
    Private MyView As FormsView
    Private ReadOnly JobsList As List(Of DownloadProgress)
    Friend ReadOnly Property Working As Boolean
        Get
            Return JobsList.Count > 0 AndAlso JobsList.Exists(Function(j) j.Job.Working)
        End Get
    End Property
    Friend Sub [Stop]()
        If JobsList.Count > 0 Then JobsList.ForEach(Sub(j) j.Stop())
    End Sub
    Private Sub [Start]()
        If JobsList.Count > 0 Then JobsList.ForEach(Sub(j) j.Start())
    End Sub
    Friend Sub New()
        InitializeComponent()
        JobsList = New List(Of DownloadProgress)
        If Settings.Plugins.Count > 0 Then
            Dim j As TDownloader.Job
            For Each p As PluginHost In Settings.Plugins
                If p.Settings.IsSavedPostsCompatible Then
                    j = New TDownloader.Job(Plugin.ISiteSettings.Download.SavedPosts)
                    j.AddHost(p.Settings)
                    JobsList.Add(New DownloadProgress(j))
                End If
            Next
        End If
    End Sub
    Private Sub DownloadSavedPostsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        MyView = New FormsView(Me) With {.LocationOnly = True}
        MyView.ImportFromXML(Settings.Design)
        MyView.SetMeSize()
        If JobsList.Count > 0 Then
            For Each j As DownloadProgress In JobsList
                AddHandler j.DownloadDone, AddressOf Jobs_DownloadDone
                TP_MAIN.RowStyles.Add(New RowStyle(SizeType.Absolute, 60))
                TP_MAIN.RowCount += 1
                TP_MAIN.Controls.Add(j.Get, 0, TP_MAIN.RowStyles.Count - 1)
            Next
            Dim s As Size = Size
            s.Height += (60 * JobsList.Count + JobsList.Count)
            MinimumSize = s
            Size = s
            MaximumSize = s
        End If
    End Sub
    Private Sub DownloadSavedPostsForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        e.Cancel = True
        Hide()
    End Sub
    Private Sub DownloadSavedPostsForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        [Stop]()
        MyView.Dispose(Settings.Design)
    End Sub
    Private Sub Jobs_DownloadDone(ByVal Message As String)
        RaiseEvent DownloadDone(Message)
    End Sub
    Private Sub BTT_DOWN_ALL_Click(sender As Object, e As EventArgs) Handles BTT_DOWN_ALL.Click
        Start()
    End Sub
    Private Sub BTT_STOP_ALL_Click(sender As Object, e As EventArgs) Handles BTT_STOP_ALL.Click
        [Stop]()
    End Sub
End Class