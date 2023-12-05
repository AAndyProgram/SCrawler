' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports SCrawler.DownloadObjects
Imports SCrawler.Plugin.Hosts
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls.KeyClick
Friend Class DownloadSavedPostsForm
    Friend Event DownloadDone As NotificationEventHandler
    Friend Event FeedFilesChanged As TDownloader.FeedFilesChangedEventHandler
    Private MyView As FormView
    Private ReadOnly JobsList As List(Of DownloadProgress)
    Friend ReadOnly Property Working As Boolean
        Get
            Return JobsList.Count > 0 AndAlso JobsList.Exists(Function(j) j.Job.Working)
        End Get
    End Property
    Friend Sub New()
        InitializeComponent()
        JobsList = New List(Of DownloadProgress)
        If Settings.Plugins.Count > 0 Then
            Dim j As TDownloader.Job
            For Each p As PluginHost In Settings.Plugins
                If p.Settings.Default.IsSavedPostsCompatible Then
                    j = New TDownloader.Job(Plugin.ISiteSettings.Download.SavedPosts)
                    j.AddHost(p.Settings)
                    JobsList.Add(New DownloadProgress(j))
                End If
            Next
        End If
    End Sub
    Private Sub DownloadSavedPostsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        MyView = New FormView(Me) With {.LocationOnly = True}
        MyView.Import(Settings.Design)
        MyView.SetFormSize()
        If JobsList.Count > 0 Then
            For Each j As DownloadProgress In JobsList
                AddHandler j.DownloadDone, AddressOf Jobs_DownloadDone
                AddHandler j.FeedFilesChanged, AddressOf Jobs_FeedFilesChanged
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
    Private Sub [Start]() Handles BTT_DOWN_ALL.Click
        If JobsList.Count > 0 Then
            Dim ses% = Downloader.SessionSavedPosts
            Dim args As New KeyClickEventArgs
            args.Reset()
            JobsList.ForEach(Sub(ByVal j As DownloadProgress)
                                 ses += 1
                                 j.Start(True, ses, Not args.Control)
                             End Sub)
            Downloader.SessionSavedPosts = ses
        End If
    End Sub
    Friend Sub [Stop]() Handles BTT_STOP_ALL.Click
        If JobsList.Count > 0 Then JobsList.ForEach(Sub(j) j.Stop())
    End Sub
    Private Sub Jobs_DownloadDone(ByVal Obj As SettingsCLS.NotificationObjects, ByVal Message As String)
        RaiseEvent DownloadDone(SettingsCLS.NotificationObjects.SavedPosts, Message)
    End Sub
    Private Sub Jobs_FeedFilesChanged(ByVal Added As Boolean)
        RaiseEvent FeedFilesChanged(Added)
    End Sub
End Class