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
Imports PersonalUtilities.Tools
Namespace DownloadObjects
    Friend Class VideosDownloaderForm
#Region "Declarations"
        Private MyView As FormView
        Private ReadOnly DownloadingUrlsFile As SFile = $"{SettingsFolderName}\VideosUrls.txt"
        Private ReadOnly MyJob As JobThread(Of String)
        Friend Property IsStandalone As Boolean = False
#End Region
#Region "Initializer"
        Public Sub New()
            InitializeComponent()
            MyJob = New JobThread(Of String) With {.Progress = New Toolbars.MyProgress(ToolbarBOTTOM, PR_V, LBL_STATUS, "Downloading video")}
            If DownloadingUrlsFile.Exists Then _
               MyJob.Items.ListAddList(DownloadingUrlsFile.GetText.StringToList(Of String, List(Of String))(Environment.NewLine), LAP.NotContainsOnly)
        End Sub
#End Region
#Region "Form handlers"
        Private Sub VideosDownloaderForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            MyView = New FormView(Me)
            MyView.Import(Settings.Design)
            MyView.SetFormSize()
            RefillList(False)
        End Sub
        Private Sub VideosDownloaderForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            If Not IsStandalone Then e.Cancel = True : Hide()
        End Sub
        Private Sub VideosDownloaderForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            If Not MyView Is Nothing Then MyView.Dispose(Settings.Design)
            If MyJob.Count > 0 Then UpdateUrlsFile()
            MyJob.Dispose()
        End Sub
        Private Sub VideosDownloaderForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            Dim b As Boolean = True
            Select Case e.KeyCode
                Case Keys.Insert : AddItem()
                Case Keys.F5 : StartDownloading()
                Case Keys.F8 : DeleteItem()
                Case Keys.Escape : If IsStandalone Then b = False Else Hide()
                Case Else : b = False
            End Select
            If b Then e.Handled = True
        End Sub
#End Region
#Region "Refill, Update file"
        Private Sub RefillList(Optional ByVal Update As Boolean = True)
            Try
                Dim a As Action = Sub()
                                      With LIST_VIDEOS
                                          .Items.Clear()
                                          If MyJob.Count > 0 Then MyJob.Items.ForEach(Sub(u) .Items.Add(u))
                                          If _LatestSelected.ValueBetween(0, .Items.Count - 1) Then .SelectedIndex = _LatestSelected
                                          If Update Then UpdateUrlsFile()
                                      End With
                                  End Sub
                If LIST_VIDEOS.InvokeRequired Then LIST_VIDEOS.Invoke(a) Else a.Invoke
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendInLog, ex, "Error on list refill")
            End Try
        End Sub
        Private Sub UpdateUrlsFile()
            If MyJob.Count > 0 Then
                TextSaver.SaveTextToFile(MyJob.ListToString(Environment.NewLine), DownloadingUrlsFile, True,, EDP.SendInLog)
            Else
                DownloadingUrlsFile.Delete(, Settings.DeleteMode, EDP.SendInLog)
            End If
        End Sub
#End Region
#Region "Add, Delete"
        Private Sub AddItem() Handles BTT_ADD.Click
            Dim URL$ = InputBoxE("Enter video URL:", "Download video by URL", GetCurrentBuffer())
            If Not URL.IsEmptyString Then
                If Not MyJob.Contains(URL) Then
                    MyJob.Add(URL)
                    RefillList()
                Else
                    MsgBoxE("This URL has already been added to the list")
                End If
            End If
        End Sub
        Private Sub AddItemsRange() Handles BTT_ADD_LIST.Click
            Dim l$ = InputBoxE("Enter URLs (new line as delimiter):", "URLs list", GetCurrentBuffer(),,,,,, True)
            If Not l.IsEmptyString Then
                Dim ub% = MyJob.Count
                MyJob.Items.ListAddList(l.StringFormatLines.StringToList(Of String, List(Of String))(vbCrLf).ListForEach(Function(u, i) u.Trim,, False))
                If Not MyJob.Count = ub Then RefillList()
            End If
        End Sub
        Private Sub DeleteItem() Handles BTT_DELETE.Click
            If _LatestSelected.ValueBetween(0, MyJob.Count - 1) Then
                If MsgBoxE({$"Are you sure you want to delete the video URL:{vbCr}{MyJob(_LatestSelected)}", "Deleting URL..."}, vbExclamation + vbYesNo) = vbYes Then
                    MyJob.Items.RemoveAt(_LatestSelected)
                    RefillList()
                End If
            Else
                MsgBoxE("URL not selected", MsgBoxStyle.Exclamation)
            End If
        End Sub
#End Region
#Region "Start, Stop"
        Private Sub BTT_DOWN_Click(sender As Object, e As EventArgs) Handles BTT_DOWN.Click
            StartDownloading()
        End Sub
        Private Sub BTT_STOP_Click(sender As Object, e As EventArgs) Handles BTT_STOP.Click
            ControlInvoke(ToolbarTOP, BTT_STOP, Sub() BTT_STOP.Enabled = False)
            MyJob.Stop()
        End Sub
#End Region
#Region "Downloading"
        Private Sub StartDownloading()
            If Not MyJob.Working And MyJob.Count > 0 Then
                ControlInvoke(ToolbarTOP, BTT_DOWN, Sub() BTT_DOWN.Enabled = False)
                ControlInvoke(ToolbarTOP, BTT_STOP, Sub() BTT_STOP.Enabled = True)
                MyJob.Start(AddressOf DownloadVideos, Threading.ApartmentState.STA)
            End If
        End Sub
        Private Sub DownloadVideos()
            MyJob.Start()
            If MyJob.Count > 0 Then
                MyJob.Progress.Maximum = MyJob.Count
                MyJob.Progress.Visible = True
                Dim IsFirst As Boolean = True
                For i% = MyJob.Count - 1 To 0 Step -1
                    If MyJob.IsCancellationRequested Then Exit For
                    If DownloadVideoByURL(MyJob(i), IsFirst, True) Then MyJob.Items.RemoveAt(i)
                    MyJob.Progress.Perform()
                    IsFirst = False
                Next
                MyJob.Progress.Done()
                RefillList()
                MyJob.Progress.Visible = False
            End If
            ControlInvoke(ToolbarTOP, BTT_DOWN, Sub() BTT_DOWN.Enabled = True)
            ControlInvoke(ToolbarTOP, BTT_STOP, Sub() BTT_STOP.Enabled = False)
            If Not IsStandalone Then MainFrameObj.UpdateLogButton()
            MyJob.Stopped()
        End Sub
#End Region
#Region "List handlers"
        Private _LatestSelected As Integer = -1
        Private Sub LIST_VIDEOS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LIST_VIDEOS.SelectedIndexChanged
            _LatestSelected = LIST_VIDEOS.SelectedIndex
        End Sub
#End Region
#Region "Open path"
        Private Sub BTT_OPEN_PATH_Click(sender As Object, e As EventArgs) Handles BTT_OPEN_PATH.Click
            With Settings.LatestSavingPath
                If Not .Value.IsEmptyString Then
                    If .Value.Exists(SFO.Path, False) Then
                        GlobalOpenPath(.Value, EDP.ShowMainMsg)
                    Else
                        MsgBoxE($"Path [{ .Value}] does not exists!", MsgBoxStyle.Exclamation)
                    End If
                Else
                    MsgBoxE("Save path not specified!", MsgBoxStyle.Exclamation)
                End If
            End With
        End Sub
#End Region
    End Class
End Namespace