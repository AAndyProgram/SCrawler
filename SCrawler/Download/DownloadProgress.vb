' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Forms.Controls.KeyClick
Imports Download = SCrawler.Plugin.ISiteSettings.Download
Imports TDJob = SCrawler.DownloadObjects.TDownloader.Job
Namespace DownloadObjects
    Friend Class DownloadProgress : Implements IDisposable
#Region "Events"
        Friend Event DownloadDone As NotificationEventHandler
        Friend Event ProgressChanged(ByVal Main As Boolean, ByVal IsMaxValue As Boolean, ByVal IsDone As Boolean)
        Friend Event FeedFilesChanged As TDownloader.FeedFilesChangedEventHandler
#End Region
#Region "Declarations"
#Region "Controls"
        Private ReadOnly TP_MAIN As TableLayoutPanel
        Private ReadOnly TP_CONTROLS As TableLayoutPanel
        Private WithEvents BTT_START As Button
        Private WithEvents BTT_STOP As Button
        Private WithEvents BTT_OPEN As Button
        Private ReadOnly PR_MAIN As ProgressBar
        Private ReadOnly PR_PRE As ProgressBar
        Private ReadOnly LBL_INFO As Label
        Private ReadOnly Icon As PictureBox
        Private ReadOnly TT_MAIN As ToolTip
#End Region
        Private ReadOnly Property Instance As API.Base.ProfileSaved
        Friend ReadOnly Property Job As TDJob
        Private ReadOnly InternalArgs As KeyClickEventArgs
#End Region
#Region "Initializer"
        Friend Sub New(ByVal _Job As TDJob)
            Job = _Job
            InternalArgs = New KeyClickEventArgs

            TT_MAIN = New ToolTip
            TP_MAIN = New TableLayoutPanel With {.Margin = New Padding(0), .Dock = DockStyle.Fill}
            TP_MAIN.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
            TP_MAIN.ColumnCount = 1
            TP_CONTROLS = New TableLayoutPanel With {.Margin = New Padding(0), .Dock = DockStyle.Fill}
            PR_MAIN = New ProgressBar With {.Dock = DockStyle.Fill}
            PR_PRE = New ProgressBar With {.Dock = DockStyle.Fill}
            LBL_INFO = New Label With {.Text = String.Empty, .Dock = DockStyle.Fill}
            Icon = New PictureBox With {
                .SizeMode = PictureBoxSizeMode.Zoom,
                .Dock = DockStyle.Fill,
                .Margin = New Padding(3),
                .Padding = New Padding(3)
            }
            CreateButton(BTT_STOP, My.Resources.DeletePic_24)
            Dim img As Image = Nothing
            If Not _Job.Host(String.Empty) Is Nothing Then
                With Job.Host(String.Empty).Source
                    If Not .Icon Is Nothing Then img = .Icon.ToBitmap
                    If img Is Nothing AndAlso Not .Image Is Nothing Then img = .Image
                End With
            End If
            If Not img Is Nothing Then Icon.Image = img : Icon.InitialImage = img

            If Job.Type = Download.Main Then
                LBL_INFO.Margin = New Padding(3)
                LBL_INFO.TextAlign = ContentAlignment.MiddleLeft
                With TP_MAIN
                    .RowStyles.Add(New RowStyle(SizeType.Percent, 100))
                    .RowCount = 1
                End With
                With TP_CONTROLS
                    .ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 30))
                    .ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 30))
                    .ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 75))
                    .ColumnStyles.Add(New ColumnStyle(SizeType.Absolute, 75)) '150
                    .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
                    .ColumnCount = .ColumnStyles.Count
                    .RowStyles.Add(New RowStyle(SizeType.Percent, 100))
                    .RowCount = 1
                    With .Controls
                        If Not img Is Nothing Then .Add(Icon, 0, 0)
                        .Add(BTT_STOP, 1, 0)
                        .Add(PR_PRE, 2, 0)
                        .Add(PR_MAIN, 3, 0)
                        .Add(LBL_INFO, 4, 0)
                    End With
                End With
                TP_MAIN.Controls.Add(TP_CONTROLS, 0, 0)
            Else
                LBL_INFO.Padding = New Padding(3, 0, 3, 0)
                LBL_INFO.TextAlign = ContentAlignment.TopCenter
                CreateButton(BTT_START, My.Resources.StartPic_Green_16)
                TT_MAIN.SetToolTip(BTT_START, "Ctrl+Click: download, exclude from feed.")
                CreateButton(BTT_OPEN, PersonalUtilities.My.Resources.FolderOpenPic_Black_16)
                With TP_CONTROLS
                    With .ColumnStyles
                        .Add(New ColumnStyle(SizeType.Absolute, 30))
                        .Add(New ColumnStyle(SizeType.Absolute, 30))
                        .Add(New ColumnStyle(SizeType.Absolute, 30))
                        .Add(New ColumnStyle(SizeType.Absolute, 30))
                        .Add(New ColumnStyle(SizeType.Percent, 50))
                        .Add(New ColumnStyle(SizeType.Percent, 50)) '100
                    End With
                    .ColumnCount = .ColumnStyles.Count
                    .RowStyles.Add(New RowStyle(SizeType.Percent, 50))
                    .RowCount = 1
                    With .Controls
                        If Not img Is Nothing Then .Add(Icon, 0, 0)
                        .Add(BTT_START, 1, 0)
                        .Add(BTT_STOP, 2, 0)
                        .Add(BTT_OPEN, 3, 0)
                        .Add(PR_PRE, 4, 0)
                        .Add(PR_MAIN, 5, 0)
                    End With
                End With
                With TP_MAIN
                    With .RowStyles
                        .Add(New RowStyle(SizeType.Absolute, 30))
                        .Add(New RowStyle(SizeType.Percent, 100))
                    End With
                    .RowCount = 2
                End With
                TP_MAIN.Controls.Add(TP_CONTROLS, 0, 0)
                TP_MAIN.Controls.Add(LBL_INFO, 0, 1)
            End If

            With Job
                .Progress = New MyProgressExt(PR_MAIN, PR_PRE, LBL_INFO) With {.ResetProgressOnMaximumChanges = False}
                With DirectCast(.Progress, MyProgressExt)
                    AddHandler .ProgressChanged, AddressOf JobProgress_ProgressChanged
                    AddHandler .MaximumChanged, AddressOf JobProgress_MaximumChanged
                    AddHandler .Maximum0Changed, AddressOf JobProgress_Maximum0Changed
                    AddHandler .Progress0Changed, AddressOf JobProgress_Progress0Changed
                    AddHandler .ProgressCompleted, AddressOf JobProgress_Done
                    AddHandler .Progress0Completed, AddressOf JobProgress_Done0
                End With
            End With

            If Job.Type = Download.SavedPosts And Not Job.Progress Is Nothing Then Job.Progress.InformationTemporary = Job.HostCollection.Name
            Instance = New API.Base.ProfileSaved(Job.HostCollection, Job.Progress)
        End Sub
        Private Sub CreateButton(ByRef BTT As Button, ByVal Img As Image)
            BTT = New Button With {
                .BackgroundImage = Img,
                .BackgroundImageLayout = ImageLayout.Zoom,
                .Text = String.Empty,
                .Dock = DockStyle.Fill
            }
        End Sub
#End Region
        Friend Function [Get]() As TableLayoutPanel
            Return TP_MAIN
        End Function
#Region "Buttons"
        Private Sub BTT_START_Click(sender As Object, e As EventArgs) Handles BTT_START.Click
            InternalArgs.Reset()
            Start(, Downloader.SessionSavedPosts, Not InternalArgs.Control)
        End Sub
        Private Sub BTT_STOP_Click(sender As Object, e As EventArgs) Handles BTT_STOP.Click
            [Stop]()
        End Sub
        Private Sub BTT_OPEN_Click(sender As Object, e As EventArgs) Handles BTT_OPEN.Click
            GlobalOpenPath(If(Job.HostCollection.FirstOrDefault(Function(h) h.DownloadSavedPosts), Job.HostCollection.Default).SavedPostsPath)
        End Sub
#End Region
#Region "Start, Stop"
        Private _IsMultiple As Boolean = False
        Private _Session As Integer = 0
        Private _IncludeInTheFeed As Boolean = True
        Friend Sub Start(Optional ByVal Multiple As Boolean = False, Optional ByVal Session As Integer = -1,
                         Optional ByVal IncludeInTheFeed As Boolean = True)
            _IsMultiple = Multiple
            _Session = Session
            _IncludeInTheFeed = IncludeInTheFeed
            Job.StartThread(AddressOf DownloadData)
        End Sub
        Friend Sub [Stop]()
            Job.Cancel()
        End Sub
#End Region
#Region "SavedPosts downloading"
        Private Sub DownloadData()
            Dim btte As Action(Of Button, Boolean) = Sub(b, e) If b.InvokeRequired Then b.Invoke(Sub() b.Enabled = e) Else b.Enabled = e
            Try
                btte.Invoke(BTT_START, False)
                btte.Invoke(BTT_STOP, True)
                Job.Progress.InformationTemporary = $"{Job.HostCollection.Name} downloading started"
                Job.Start()
                Instance.Session = _Session
                Instance.IncludeInTheFeed = _IncludeInTheFeed
                Instance.Download(Job.Token, _IsMultiple)
                If _IncludeInTheFeed And Instance.FeedDataExists Then RaiseEvent FeedFilesChanged(True)
                RaiseEvent DownloadDone(SettingsCLS.NotificationObjects.SavedPosts, $"Downloading saved {Job.HostCollection.Name} posts is completed")
            Catch ex As Exception
                Job.Progress.InformationTemporary = $"{Job.HostCollection.Name} downloading error"
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, {$"{Job.HostCollection.Name} saved posts downloading error", "Saved posts"})
            Finally
                _IsMultiple = False
                btte.Invoke(BTT_START, True)
                btte.Invoke(BTT_STOP, False)
                Job.Finish()
                If Job.Type = Download.SavedPosts Then Job.Progress.Maximum = 0 : Job.Progress.Value = 0
            End Try
        End Sub
#End Region
#Region "Progress, Jobs count"
        Private Sub JobProgress_MaximumChanged(ByVal Sender As Object, ByVal e As ProgressEventArgs)
            If Not Job.Type = Download.SavedPosts Then RaiseEvent ProgressChanged(True, True, False)
        End Sub
        Private Sub JobProgress_Maximum0Changed(ByVal Sender As Object, ByVal e As ProgressEventArgs)
            If Not Job.Type = Download.SavedPosts Then RaiseEvent ProgressChanged(False, True, False)
        End Sub
        Private Sub JobProgress_ProgressChanged(ByVal Sender As Object, ByVal e As ProgressEventArgs)
            If Not Job.Type = Download.SavedPosts Then MainProgress.Perform()
        End Sub
        Private Sub JobProgress_Progress0Changed(ByVal Sender As Object, ByVal e As ProgressEventArgs)
            If Not Job.Type = Download.SavedPosts Then MainProgress.Perform0()
        End Sub
        Private Sub JobProgress_Done(ByVal Sender As Object, ByVal e As ProgressEventArgs)
            If Not Job.Type = Download.SavedPosts Then RaiseEvent ProgressChanged(True, False, True)
        End Sub
        Private Sub JobProgress_Done0(ByVal Sender As Object, ByVal e As ProgressEventArgs)
            If Not Job.Type = Download.SavedPosts Then RaiseEvent ProgressChanged(False, False, True)
        End Sub
#End Region
#Region "IDisposable Support"
        Private disposedValue As Boolean = False
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    If Not BTT_START Is Nothing Then BTT_START.Dispose()
                    If Not BTT_STOP Is Nothing Then BTT_STOP.Dispose()
                    If Not BTT_OPEN Is Nothing Then BTT_OPEN.Dispose()
                    If Not Icon Is Nothing Then Icon.Dispose()
                    PR_MAIN.DisposeIfReady()
                    LBL_INFO.DisposeIfReady()
                    If Not TT_MAIN Is Nothing Then TT_MAIN.Dispose()
                    If Not TP_CONTROLS Is Nothing Then
                        TP_CONTROLS.Controls.Clear()
                        TP_CONTROLS.Dispose()
                    End If
                    If Not TP_MAIN Is Nothing Then
                        TP_MAIN.Controls.Clear()
                        TP_MAIN.Dispose()
                    End If
                End If
                disposedValue = True
            End If
        End Sub
        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub
        Friend Overloads Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace