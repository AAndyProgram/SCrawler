' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin.Hosts
Imports SCrawler.API.YouTube.Base
Imports SCrawler.API.YouTube.Objects
Imports PersonalUtilities.Forms.Controls.KeyClick
Namespace DownloadObjects.STDownloader
    Friend Class VideoDownloaderForm
        Private WithEvents BTT_ADD_URLS_ARR As ToolStripMenuItemKeyClick
        Private WithEvents BTT_ADD_URLS_EXTERNAL As ToolStripMenuItemKeyClick
        Private Const UrlsArrTag As String = "URLS_ARR"
        Private Const TAG_EXTERNAL As String = "EXTERNAL"
        Private ReadOnly ControlNonYT As New FPredicate(Of MediaItem)(Function(i) Not i.MyContainer.SiteKey = API.YouTube.YouTubeSiteKey)
        Private ReadOnly ControlsDownloadedNonYT As New FPredicate(Of MediaItem)(Function(i) i.MyContainer.MediaState = Plugin.UserMediaStates.Downloaded And ControlNonYT.Invoke(i))
        Private ReadOnly Property ExternalUrlsTemp As List(Of String)
        Public Sub New()
            InitializeComponent()
            ExternalUrlsTemp = New List(Of String)
            AppMode = False
            Icon = My.Resources.ArrowDownIcon_Blue_24
            BTT_ADD_PLS_ARR.Text = $"YouTube: {BTT_ADD_PLS_ARR.Text}"
            BTT_ADD_URLS_ARR = New ToolStripMenuItemKeyClick("Add an array of URLs", PersonalUtilities.My.Resources.PlusPic_Green_24) With {.Tag = UrlsArrTag}
            BTT_ADD_URLS_EXTERNAL = New ToolStripMenuItemKeyClick With {.Tag = TAG_EXTERNAL}
            MENU_ADD.DropDownItems.Insert(1, BTT_ADD_URLS_ARR)
            Text = "Video downloader"
        End Sub
        Protected Overrides Sub VideoListForm_Disposed(sender As Object, e As EventArgs)
            ExternalUrlsTemp.Clear()
            MyBase.VideoListForm_Disposed(sender, e)
        End Sub
        Friend Sub ADD_URLS_EXTERNAL(ByVal UrlsList As IEnumerable(Of String))
            ExternalUrlsTemp.ListAddList(UrlsList, LAP.ClearBeforeAdd, LAP.NotContainsOnly)
            If ExternalUrlsTemp.Count > 0 Then BTT_ADD_URLS_EXTERNAL.PerformClick()
        End Sub
        Protected Overrides Function LoadData_GetFiles() As List(Of IYouTubeMediaContainer)
            Try
                Dim l As List(Of IYouTubeMediaContainer) = Nothing
                If Settings.STDownloader_LoadYTVideos Then l = MyBase.LoadData_GetFiles()
                If l Is Nothing Then l = New List(Of IYouTubeMediaContainer)
                Dim path As SFile = DownloaderDataFolder
                If path.Exists(SFO.Path, False) Then
                    Dim files As List(Of SFile) = SFile.GetFiles(path, "*.xml",, EDP.ReturnValue)
                    If files.Count > 0 Then files.ForEach(Sub(f) l.Add(New DownloadableMediaHost(f)))
                End If
                If l.Count > 0 Then l.ListDisposeRemoveAll(Function(c) Not c.Exists)
                Return l
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "VideoListForm.LoadData_GetFiles", New List(Of IYouTubeMediaContainer))
            End Try
        End Function
        Protected Overrides Sub BTT_ADD_KeyClick(ByVal Sender As ToolStripMenuItemKeyClick, ByVal e As KeyClickEventArgs) Handles BTT_ADD_URLS_ARR.KeyClick,
                                                                                                                                  BTT_ADD_URLS_EXTERNAL.KeyClick
            Dim __tag$ = UniversalFunctions.IfNullOrEmpty(Of Object)(Sender.Tag, String.Empty)
            If Not __tag = "a" And Not __tag = UrlsArrTag And Not __tag = TAG_EXTERNAL Then
                MyBase.BTT_ADD_KeyClick(Sender, e)
            Else
                Dim url$ = String.Empty
                Try
                    Dim isExternal As Boolean = __tag = TAG_EXTERNAL
                    If Not isExternal Then url = BufferText
                    Dim disableDown As Boolean = e.Shift
                    Dim output As SFile = Settings.LatestSavingPath
                    Dim acc$ = String.Empty
                    Dim thumbAlong As Boolean = False
                    Dim isArr As Boolean = (__tag = UrlsArrTag Or (isExternal And ExternalUrlsTemp.Count > 1))
                    Dim formOpened As Boolean = False
                    Dim media As IYouTubeMediaContainer
                    Dim formValues As Func(Of DialogResult) = Function() As DialogResult
                                                                  formOpened = True
                                                                  If url.IsEmptyString OrElse Not url.StartsWith("http") Then url = String.Empty
                                                                  Using f As New DownloaderUrlForm With {.URL = url}
                                                                      f.ShowDialog()
                                                                      If f.DialogResult = DialogResult.OK Then
                                                                          url = f.URL
                                                                          output = f.OutputPath
                                                                          acc = f.AccountName
                                                                          thumbAlong = f.ThumbAlong
                                                                          Settings.STDownloader_SnapshotsKeepWithFiles_ThumbAlong.Value = thumbAlong
                                                                          Settings.LatestSavingPath.Value = output
                                                                          If Settings.STDownloader_UpdateYouTubeOutputPath Then _
                                                                             API.YouTube.MyYouTubeSettings.OutputPath.Value = output
                                                                          If Settings.STDownloader_OutputPathAutoAddPaths Then _
                                                                             Settings.DownloadLocations.Add(output, False)
                                                                          Return DialogResult.OK
                                                                      Else
                                                                          Return DialogResult.Cancel
                                                                      End If
                                                                  End Using
                                                              End Function
                    Dim TryYouTube As Func(Of Boolean) = Function() As Boolean
                                                             If YouTubeFunctions.IsMyUrl(url) Then
                                                                 BufferText = url
                                                                 MyBase.BTT_ADD_KeyClick(Sender, e)
                                                                 Return True
                                                             Else
                                                                 Return False
                                                             End If
                                                         End Function
                    Dim canProcessUrl As Func(Of String, Boolean, Boolean) =
                        Function(ByVal __url As String, ByVal ShowMsg As Boolean) As Boolean
                            Dim exists As Boolean = False
                            If Not __url.IsEmptyString And TP_CONTROLS.Controls.Count > 0 Then _
                               exists = ControlInvokeFast(TP_CONTROLS, Function() TP_CONTROLS.Controls.Cast(Of MediaItem).ListExists(Function(m) _
                                                                                  If(m.MyContainer?.URL, String.Empty) = __url), False, EDP.ReturnValue)
                            If exists Then
                                If ShowMsg Then
                                    Return MsgBoxE({$"The URL you are trying to add already exists.{vbCr}Do you still want to add it again?{vbCr}{__url}",
                                                   "Add URL..."}, vbExclamation,,, {"Add", "Cancel"}) = 0
                                Else
                                    Return False
                                End If
                            Else
                                Return True
                            End If
                        End Function

                    If output.IsEmptyString Then output = API.YouTube.MyYouTubeSettings.OutputPath

                    If isArr Then
                        Dim urls As List(Of String) = Nothing
                        If isExternal Then urls = New List(Of String)(ExternalUrlsTemp)
                        Using fa As New DownloaderUrlsArrForm(urls)
                            fa.ShowDialog()
                            If fa.DialogResult = DialogResult.OK Then
                                urls = fa.URLs.ToList
                                output = fa.OutputPath
                                If fa.UseAccountName Then acc = fa.AccountName
                                thumbAlong = fa.ThumbAlong
                                Settings.STDownloader_SnapshotsKeepWithFiles_ThumbAlong.Value = thumbAlong
                                If Settings.STDownloader_UpdateYouTubeOutputPath Then API.YouTube.MyYouTubeSettings.OutputPath.Value = output
                                If Settings.STDownloader_OutputPathAutoAddPaths Then Settings.DownloadLocations.Add(output, False)
                            Else
                                Exit Sub
                            End If
                        End Using
                        If urls.ListExists Then
                            urls.ListForEach(Function(uu, ii) uu.StringTrim,, False)
                            urls.RemoveAll(Function(uu) uu.IsEmptyString OrElse Not uu.StartsWith("http") OrElse Not canProcessUrl(uu, False))
                        End If
                        If urls.ListExists Then
                            output.Exists(SFO.Path, True)
                            For Each url In urls
                                If Not TryYouTube.Invoke Then
                                    media = FindSource(url, output)
                                    If Not media Is Nothing AndAlso ValidateContainerURL(media) Then
                                        media.AccountName = acc
                                        If TypeOf media Is DownloadableMediaHost Then DirectCast(media, DownloadableMediaHost).ThumbAlong = thumbAlong
                                        ControlCreateAndAdd(media, disableDown)
                                    End If
                                End If
                            Next
                            urls.Clear()
                        Else
                            MsgBoxE({"There are no valid URLs in the list", "Add URLs array"}, vbCritical)
                        End If
                    Else
                        If isExternal Then url = ExternalUrlsTemp.FirstOrDefault
                        If formValues.Invoke = DialogResult.Cancel Then Exit Sub
                        If canProcessUrl(url, True) Then
                            If TryYouTube.Invoke Then Exit Sub
                            media = FindSource(url, output)
                        Else
                            Exit Sub
                        End If
                        If media Is Nothing And Not formOpened Then
                            If formValues.Invoke = DialogResult.Cancel Then
                                Exit Sub
                            Else
                                If canProcessUrl(url, True) Then
                                    If TryYouTube.Invoke Then Exit Sub
                                    media = FindSource(url, output)
                                Else
                                    Exit Sub
                                End If
                            End If
                        End If
                        If media Is Nothing Then
                            MsgBoxE({$"The URL you entered is not recognized by existing plugins.{vbCr}{url}", "Download video"}, vbCritical)
                        ElseIf ValidateContainerURL(media) Then
                            media.AccountName = acc
                            If TypeOf media Is DownloadableMediaHost Then DirectCast(media, DownloadableMediaHost).ThumbAlong = thumbAlong
                            output.Exists(SFO.Path, True)
                            ControlCreateAndAdd(media, disableDown)
                        End If
                    End If
                Catch ex As Exception
                    ErrorsDescriber.Execute(EDP.LogMessageValue, ex, $"Error when trying to download video from URL: [{url}]")
                Finally
                    ExternalUrlsTemp.Clear()
                End Try
            End If
        End Sub
        Private Function FindSource(ByVal URL As String, ByVal OutputFile As SFile) As IYouTubeMediaContainer
            If Not URL.IsEmptyString Then
                If URL.Contains("gfycat") Then
                    Return API.Gfycat.Envir.GetSingleMediaInstance(URL, OutputFile)
                ElseIf URL.Contains("imgur.com") Then
                    Return API.Imgur.Envir.GetSingleMediaInstance(URL, OutputFile)
                Else
                    For i% = 0 To Settings.Plugins.Count - 1
                        With Settings.Plugins(i).Settings
                            If .IsMyImageVideo(URL).Exists Then Return .GetSingleMediaInstance(URL, OutputFile)
                        End With
                    Next
                End If
            End If
            Return Nothing
        End Function
        Protected Overrides Sub BTT_CLEAR_DONE_Click(sender As Object, e As EventArgs)
            If Settings.STDownloader_RemoveYTVideosOnClear Then
                MyBase.BTT_CLEAR_DONE_Click(sender, e)
            Else
                RemoveControls(ControlsDownloadedNonYT, False)
            End If
        End Sub
        Protected Overrides Sub BTT_CLEAR_ALL_Click(sender As Object, e As EventArgs)
            If Settings.STDownloader_RemoveYTVideosOnClear Then
                MyBase.BTT_CLEAR_ALL_Click(sender, e)
            Else
                RemoveControls(ControlNonYT, False)
            End If
        End Sub
    End Class
End Namespace