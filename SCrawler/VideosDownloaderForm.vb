Imports System.ComponentModel
Imports PersonalUtilities.Forms
Friend Class VideosDownloaderForm
    Private MyView As FormsView
    Private ReadOnly MyPR As Toolbars.MyProgress
    Private ReadOnly UrlList As List(Of String)
    Private ReadOnly DownloadingUrlsFile As SFile = $"{SettingsFolderName}\VideosUrls.txt"
    Friend Sub New()
        InitializeComponent()
        UrlList = New List(Of String)
        MyPR = New Toolbars.MyProgress(ToolbarBOTTOM, PR_V, LBL_STATUS, "Downloading video")
        If DownloadingUrlsFile.Exists Then
            UrlList.ListAddList(DownloadingUrlsFile.GetText.StringToList(Of String, List(Of String))(Environment.NewLine), LAP.NotContainsOnly)
        End If
    End Sub
    Private Sub VideosDownloaderForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            MyView = New FormsView(Me)
            MyView.ImportFromXML(Settings.Design)
            MyView.SetMeSize()
            RefillList()
        Catch ex As Exception
        End Try
    End Sub
    Private Sub VideosDownloaderForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        e.Cancel = True
        Hide()
    End Sub
    Private Sub VideosDownloaderForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        If Not MyView Is Nothing Then MyView.Dispose(Settings.Design)
        If UrlList.Count > 0 Then
            TextSaver.SaveTextToFile(UrlList.ListToString(, Environment.NewLine), DownloadingUrlsFile, True,, EDP.SendInLog)
        End If
        UrlList.Clear()
    End Sub
    Private Sub VideosDownloaderForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Dim b As Boolean = True
        Select Case e.KeyCode
            Case Keys.Insert : AddVideo()
            Case Keys.F5 : DownloadVideos()
            Case Keys.F8 : BTT_DELETE_Click(Nothing, EventArgs.Empty)
            Case Else : b = False
        End Select
        If b Then e.Handled = True
    End Sub
    Private Sub RefillList()
        Try
            With LIST_VIDEOS
                .Items.Clear()
                If UrlList.Count > 0 Then UrlList.ForEach(Sub(u) .Items.Add(u))
                If .Items.Count > 0 And _LatestSelected >= 0 And _LatestSelected <= .Items.Count - 1 Then .SelectedIndex = _LatestSelected
            End With
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Error on list refill")
        End Try
    End Sub
    Private Sub BTT_ADD_Click(sender As Object, e As EventArgs) Handles BTT_ADD.Click
        AddVideo()
    End Sub
    Private Sub AddVideo()
        Dim URL$ = GetNewVideoURL()
        If Not URL.IsEmptyString Then
            If Not UrlList.Contains(URL) Then
                UrlList.Add(URL)
                RefillList()
            Else
                MsgBoxE("This URL already added to list")
            End If
        End If
    End Sub
    Private Sub BTT_ADD_LIST_Click(sender As Object, e As EventArgs) Handles BTT_ADD_LIST.Click
        Dim l$ = InputBoxE("Enter URLs (new line as delimiter):", "URLs list", GetCurrentBuffer(),,,,,, True)
        If Not l.IsEmptyString Then
            Dim ub% = UrlList.Count
            UrlList.ListAddList(l.StringToList(Of String, List(Of String))(Environment.NewLine))
            If Not UrlList.Count = ub Then RefillList()
        End If
    End Sub
    Private Sub BTT_DELETE_Click(sender As Object, e As EventArgs) Handles BTT_DELETE.Click
        If _LatestSelected >= 0 And _LatestSelected <= UrlList.Count - 1 Then
            If MsgBoxE({$"Do you really want to delete video URL:{vbCr}{UrlList(_LatestSelected)}", "Deleting URL..."},
                       MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo).DialogResult = MsgBoxResult.Yes Then
                UrlList.RemoveAt(_LatestSelected)
                RefillList()
            End If
        Else
            MsgBoxE("URL does not selected", MsgBoxStyle.Exclamation)
        End If
    End Sub
    Private Sub BTT_DOWN_Click(sender As Object, e As EventArgs) Handles BTT_DOWN.Click
        DownloadVideos()
    End Sub
    Private Sub DownloadVideos()
        If UrlList.Count > 0 Then
            MyPR.TotalCount = UrlList.Count
            MyPR.Enabled = True
            Dim IsFirst As Boolean = True
            For i% = UrlList.Count - 1 To 0 Step -1
                If DownloadVideoByURL(UrlList(i), IsFirst, True) Then UrlList.RemoveAt(i)
                MyPR.Perform()
                IsFirst = False
            Next
            MyPR.Done()
            RefillList()
            MyPR.Enabled = False
        Else
            MsgBoxE("No one video added", MsgBoxStyle.Exclamation)
        End If
    End Sub
    Private _LatestSelected As Integer = -1
    Private Sub LIST_VIDEOS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LIST_VIDEOS.SelectedIndexChanged
        _LatestSelected = LIST_VIDEOS.SelectedIndex
    End Sub
    Private Sub BTT_OPEN_PATH_Click(sender As Object, e As EventArgs) Handles BTT_OPEN_PATH.Click
        With Settings.LatestSavingPath
            If Not .Value.IsEmptyString Then
                If .Value.Exists(SFO.Path, False) Then
                    .Value.Open(SFO.Path, EDP.ShowMainMsg)
                Else
                    MsgBoxE($"Path [{ .Value}] does not exists!", MsgBoxStyle.Exclamation)
                End If
            Else
                MsgBoxE("Saving path does not set!", MsgBoxStyle.Exclamation)
            End If
        End With
    End Sub
End Class