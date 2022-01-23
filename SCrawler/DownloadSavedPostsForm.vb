' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports System.Threading
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Toolbars
Imports SCrawler.API
Imports Job = SCrawler.TDownloader.Job
Friend Class DownloadSavedPostsForm
    Friend Event OnDownloadDone(ByVal Message As String)
    Private MyView As FormsView
    Private ReadOnly ProgressReddit As MyProgress
    Private ReadOnly ProgressInstgram As MyProgress
    Private JobReddit As Job
    Private JobInstagram As Job
    Friend ReadOnly Property Working As Boolean
        Get
            Return JobReddit Or JobInstagram
        End Get
    End Property
#Region "Start and Stop functions"
    Friend Overloads Sub [Stop]()
        [Stop](Sites.Reddit)
        [Stop](Sites.Instagram)
    End Sub
    Private Overloads Sub [Stop](ByVal Site As Sites)
        Select Case Site
            Case Sites.Reddit : If JobReddit Then JobReddit.Stop()
            Case Sites.Instagram : If JobInstagram Then JobInstagram.Stop()
        End Select
    End Sub
    Private Overloads Sub [Start]()
        Start(Sites.Reddit)
        Start(Sites.Instagram)
    End Sub
    Private Overloads Sub [Start](ByVal Site As Sites)
        Select Case Site
            Case Sites.Reddit : If Not JobReddit Then JobReddit.Start(New ThreadStart(Sub() DownloadData(Sites.Reddit)))
            Case Sites.Instagram
                If Not JobInstagram Then
                    If Not Downloader.Working(Sites.Instagram) Then
                        Downloader.InstagramSavedPostsDownloading = True
                        JobInstagram.Start(New ThreadStart(Sub() DownloadData(Sites.Instagram)))
                    Else
                        MsgBoxE({$"Downloading Instagram profiles still works.{vbCr}Wait for this to be done before starting.{vbCr}Operation canceled",
                                 "Instagram saved posts"}, MsgBoxStyle.Critical)
                    End If
                End If
        End Select
    End Sub
#End Region
#Region "Form functions"
    Friend Sub New()
        InitializeComponent()
        ProgressReddit = New MyProgress(PR_REDDIT, LBL_REDDIT)
        ProgressInstgram = New MyProgress(PR_INST, LBL_INST)
        JobReddit = New Job(ProgressReddit) With {.Site = Sites.Reddit}
        JobInstagram = New Job(ProgressInstgram) With {.Site = Sites.Instagram}
    End Sub
    Private Sub DownloadSavedPostsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        MyView = New FormsView(Me) With {.LocationOnly = True}
        MyView.ImportFromXML(Settings.Design)
        MyView.SetMeSize()
    End Sub
    Private Sub DownloadSavedPostsForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        e.Cancel = True
        Hide()
    End Sub
    Private Sub DownloadSavedPostsForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        [Stop]()
        MyView.Dispose(Settings.Design)
    End Sub
#End Region
#Region "Controls"
    Private Sub BTT_DOWN_ALL_Click(sender As Object, e As EventArgs) Handles BTT_DOWN_ALL.Click
        Start()
    End Sub
    Private Sub BTT_STOP_ALL_Click(sender As Object, e As EventArgs) Handles BTT_STOP_ALL.Click
        [Stop]()
    End Sub
#Region "Reddit"
    Private Sub BTT_REDDIT_START_Click(sender As Object, e As EventArgs) Handles BTT_REDDIT_START.Click
        Start(Sites.Reddit)
    End Sub
    Private Sub BTT_REDDIT_STOP_Click(sender As Object, e As EventArgs) Handles BTT_REDDIT_STOP.Click
        [Stop](Sites.Reddit)
    End Sub
    Private Sub BTT_REDDIT_OPEN_Click(sender As Object, e As EventArgs) Handles BTT_REDDIT_OPEN.Click
        OpenPath(Reddit.ProfileSaved.DataPath)
    End Sub
    Private Sub LBL_REDDIT_DoubleClick(sender As Object, e As EventArgs) Handles LBL_REDDIT.DoubleClick
        OpenPath(Reddit.ProfileSaved.DataPath)
    End Sub
#End Region
#Region "Instagram"
    Private Sub BTT_INST_START_Click(sender As Object, e As EventArgs) Handles BTT_INST_START.Click
        Start(Sites.Instagram)
    End Sub
    Private Sub BTT_INST_STOP_Click(sender As Object, e As EventArgs) Handles BTT_INST_STOP.Click
        [Stop](Sites.Instagram)
    End Sub
    Private Sub BTT_INST_OPEN_Click(sender As Object, e As EventArgs) Handles BTT_INST_OPEN.Click
        OpenPath(Instagram.ProfileSaved.DataPath)
    End Sub
    Private Sub LBL_INST_DoubleClick(sender As Object, e As EventArgs) Handles LBL_INST.DoubleClick
        OpenPath(Instagram.ProfileSaved.DataPath)
    End Sub
#End Region
#End Region
    Private Sub DownloadData(ByVal Site As Sites)
        Dim btte As Action(Of Button, Boolean) = Sub(b, e) If b.InvokeRequired Then b.Invoke(Sub() b.Enabled = e) Else b.Enabled = e
        Try
            Select Case Site
                Case Sites.Reddit
                    btte(BTT_REDDIT_START, False)
                    btte(BTT_REDDIT_STOP, True)
                    JobReddit.Progress.InformationTemporary = "Reddit downloading started"
                    JobReddit.Start()
                    Reddit.ProfileSaved.Download(JobReddit.Progress, JobReddit)
                Case Sites.Instagram
                    btte(BTT_INST_START, False)
                    btte(BTT_INST_STOP, True)
                    JobInstagram.Progress.InformationTemporary = "Instagram downloading started"
                    JobInstagram.Start()
                    Instagram.ProfileSaved.Download(JobInstagram.Progress, JobInstagram)
            End Select
            RaiseEvent OnDownloadDone($"Downloading saved {Site} posts is completed")
        Catch ex As Exception
            Select Case Site
                Case Sites.Reddit : JobReddit.Progress.InformationTemporary = "Reddit downloading error"
                Case Sites.Instagram : JobInstagram.Progress.InformationTemporary = "Instagram downloading error"
            End Select
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, {$"{Site} saved posts downloading error", "Saved posts"})
        Finally
            Select Case Site
                Case Sites.Reddit
                    JobReddit.Stopped()
                    btte(BTT_REDDIT_START, True)
                    btte(BTT_REDDIT_STOP, False)
                Case Sites.Instagram
                    JobInstagram.Stopped()
                    btte(BTT_INST_START, True)
                    btte(BTT_INST_STOP, False)
                    Downloader.InstagramSavedPostsDownloading = False
            End Select
        End Try
    End Sub
    Private Sub OpenPath(ByVal f As SFile)
        If f.Exists(SFO.Path, False) Then f.Open(SFO.Path)
    End Sub
End Class