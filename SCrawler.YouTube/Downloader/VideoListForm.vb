' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Forms.Controls.KeyClick
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Functions.Messaging
Imports SCrawler.API.YouTube
Imports SCrawler.API.YouTube.Base
Imports SCrawler.API.YouTube.Controls
Imports SCrawler.API.YouTube.Objects
Namespace DownloadObjects.STDownloader
    Public Class VideoListForm : Implements IDesignXMLContainer
#Region "Declarations"
        Private ReadOnly MyView As FormView
        Private ReadOnly MyProgress As MyProgress
        Protected WithEvents MyJob As JobThread(Of MediaItem)
        Public Property DesignXML As EContainer Implements IDesignXMLContainer.DesignXML
        Public Property DesignXMLNodes As String() Implements IDesignXMLContainer.DesignXMLNodes
        Public Property DesignXMLNodeName As String Implements IDesignXMLContainer.DesignXMLNodeName
        Private ReadOnly ControlsDownloaded As New FPredicate(Of MediaItem)(Function(i) i.MyContainer.MediaState = Plugin.UserMediaStates.Downloaded)
        Private ReadOnly ControlsChecked As Predicate(Of MediaItem) = Function(i) i.Checked
        Private ReadOnly CNT_PROCESSOR As TableControlsProcessor
        Protected AppMode As Boolean = True
#End Region
#Region "Initializer"
        Public Sub New()
            InitializeComponent()
            CNT_PROCESSOR = New TableControlsProcessor(TP_CONTROLS)
            MyView = New FormView(Me)
            MyProgress = New MyProgress(TOOLBAR_BOTTOM, PR_MAIN, LBL_INFO)
            MyJob = New JobThread(Of MediaItem)
        End Sub
#End Region
#Region "Form handlers"
        Protected Overridable Sub VideoListForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            If Not LicenseManager.UsageMode = LicenseUsageMode.Designtime Then
                If MyYouTubeSettings Is Nothing Then MyYouTubeSettings = New YouTubeSettings
                DesignXML = MyYouTubeSettings.DesignXml
                If MyCache Is Nothing Then MyCache = New CacheKeeper(YouTubeFunctions.YouTubeCachePathRoot)
            End If

            If AppMode Then
                If Now.Month.ValueBetween(6, 8) Then
                    Text = "SCrawler: Happy LGBT Pride Month! :-)"
                ElseIf Not MyYouTubeSettings Is Nothing AndAlso Not MyYouTubeSettings.ProgramText.IsEmptyString Then
                    Text = MyYouTubeSettings.ProgramText
                End If
                MyNotificator = New YTNotificator(Me)
                MyDownloaderSettings = MyYouTubeSettings
                ProgramLogInitialize()
                With ProgramLog
                    AddHandler .TextAdded, AddressOf ProgramLog_TextAdded
                    AddHandler .TextCleared, AddressOf ProgramLog_TextCleared
                End With
                UpdateLogButton()
            End If

            With MyView : .Import() : .SetFormSize() : End With
            BTT_DELETE.Enabled = False
            If Not AppMode Then
                BTT_SETTINGS.Visible = False
                SEP_1.Visible = False
                SEP_LOG.Visible = False
                BTT_LOG.Visible = False
                BTT_INFO.Visible = False
                BTT_DONATE.Visible = False
                BTT_BUG_REPORT.Visible = False
            End If
            MyProgress.Visible = False
            LoadData()
        End Sub
        Protected Overridable Sub VideoListForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            If Not AppMode Then e.Cancel = True : Hide()
        End Sub
        Protected Overridable Sub VideoListForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            MyView.Dispose()
            MyCache.DisposeIfReady()
            If AppMode Then
                MyNotificator.Clear()
                If Not MyMainLOG.IsEmptyString Then SaveLogToFile()
            End If
            If Not MyYouTubeSettings Is Nothing Then MyYouTubeSettings.Close()
        End Sub
        Private Sub VideoListForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            Dim b As Boolean = True
            Select Case e.KeyCode
                Case Keys.Insert : BTT_ADD.PerformClick()
                Case Keys.F5 : BTT_DOWN.PerformClick()
                Case Else : b = False
            End Select
            If b Then e.Handled = True
        End Sub
#End Region
#Region "Refill, save list"
        Protected Sub LoadData()
            Dim c As List(Of IYouTubeMediaContainer) = LoadData_GetFiles()
            If c.ListExists Then
                c.Sort(New ContainerDateComparer)
                SuspendLayout()
                For i% = c.Count - 1 To 0 Step -1 : ControlCreateAndAdd(c(i), True, i = 0, True) : Next
                ResumeLayout(False)
                PerformLayout()
            End If
        End Sub
        Protected Overridable Function LoadData_GetFiles() As List(Of IYouTubeMediaContainer)
            Try
                Dim l As New List(Of IYouTubeMediaContainer)
                Dim path As SFile = DownloaderDataFolderYouTube
                If path.Exists(SFO.Path, False) Then
                    Dim files As List(Of SFile) = SFile.GetFiles(path, "*.xml",, EDP.ReturnValue)
                    If files.Count > 0 Then files.ForEach(Sub(f) l.Add(YouTubeFunctions.CreateContainer(f)))
                End If
                If l.Count > 0 Then l.RemoveAll(Function(c) c Is Nothing)
                If l.Count > 0 Then l.ListDisposeRemoveAll(Function(c) Not c.Exists)
                Return l
            Catch ex As Exception
                Dim e As EDP = EDP.LogMessageValue
                If Not ex.HelpLink.IsEmptyString AndAlso ex.HelpLink = NameOf(YouTubeFunctions.CreateContainer) Then e = EDP.SendToLog + EDP.ReturnValue
                Return ErrorsDescriber.Execute(e, ex, "VideoListForm.LoadData_GetFiles", New List(Of IYouTubeMediaContainer))
            End Try
        End Function
#End Region
#Region "Controls"
        Protected Sub ControlCreateAndAdd(ByVal Container As IYouTubeMediaContainer, Optional ByVal DisableDownload As Boolean = False,
                                          Optional ByVal PerformClick As Boolean = True, Optional ByVal IsLoading As Boolean = False,
                                          Optional ByVal UseCookies As Boolean = False)
            ControlInvokeFast(TP_CONTROLS, Sub()
                                               With TP_CONTROLS
                                                   .SuspendLayout()
                                                   If Not IsLoading And (DisableDownload Or Not MyDownloaderSettings.DownloadAutomatically) Then Container.Save()
                                                   '.AutoScroll = True
                                                   '.HorizontalScroll.Visible = False
                                                   .RowStyles.Insert(0, New RowStyle(SizeType.Absolute, 60))
                                                   .RowCount = .RowStyles.Count
                                                   OffsetControls(0, True)
                                                   Dim cnt As New MediaItem(Container) With {.Dock = DockStyle.Fill, .Margin = New Padding(0), .UseCookies = UseCookies}
                                                   AddHandler cnt.FileDownloaded, AddressOf MediaControl_FileDownloaded
                                                   AddHandler cnt.Removal, AddressOf MediaControl_Removal
                                                   AddHandler cnt.DownloadAgain, AddressOf MediaControl_DownloadAgain
                                                   AddHandler cnt.DownloadRequested, AddressOf MediaControl_DownloadRequested
                                                   AddHandler cnt.CheckedChanged, AddressOf MediaControl_CheckedChanged
                                                   AddHandler cnt.Click, AddressOf CNT_PROCESSOR.MediaItem_Click
                                                   AddHandler cnt.KeyDown, AddressOf CNT_PROCESSOR.MediaItem_KeyDown
                                                   .Controls.Add(cnt, 0, 0)
                                                   .Controls.Cast(Of ISupportInitialize).ToList.ForEach(Sub(_cnt) _cnt.EndInit())
                                                   .ScrollControlIntoView(cnt)
                                                   cnt.Select()
                                                   RefillColors()
                                                   '.AutoScroll = False
                                                   '.AutoScroll = True
                                                   .ResumeLayout()
                                                   .PerformLayout()
                                                   UpdateScrolls(Me, Nothing)
                                                   If PerformClick Then cnt.PerformClick()
                                                   If Not DisableDownload And MyDownloaderSettings.DownloadAutomatically Then AddToDownload(cnt, True)
                                               End With
                                           End Sub, EDP.SendToLog)
        End Sub
#Region "Controls rendering"
        Private Overloads Sub OffsetControls()
            Try
                With TP_CONTROLS
                    If .Controls.Count > 0 Then
                        Dim i%, ri%
                        Dim cntIndx% = -1
                        Dim cnt As Control
                        For i = .Controls.Count - 1 To 0 Step -1
                            cnt = .Controls(i)
                            If Not cnt Is Nothing Then cntIndx += 1 : .SetCellPosition(cnt, New TableLayoutPanelCellPosition(0, cntIndx))
                        Next
                        For i = .RowStyles.Count - 1 To 0 Step -1
                            If Not .GetControlFromPosition(0, i) Is Nothing Then
                                If i + 1 < .RowStyles.Count - 1 Then
                                    For ri = .RowStyles.Count - 1 To i + 1 Step -1 : .RowStyles.RemoveAt(i) : Next
                                    .RowStyles.Add(New RowStyle(SizeType.AutoSize))
                                    .RowCount = .RowStyles.Count
                                End If
                                Exit For
                            End If
                        Next
                    Else
                        .RowStyles.Clear()
                        .RowCount = 0
                        .RowStyles.Add(New RowStyle(SizeType.AutoSize))
                        .RowCount = .RowStyles.Count
                    End If
                End With
            Catch
            End Try
        End Sub
        Private Overloads Sub OffsetControls(ByVal ReflectedRow As Integer, ByVal Add As Boolean)
            ControlInvokeFast(TP_CONTROLS, Sub()
                                               Dim offset% = IIf(Add, 1, -1)
                                               Dim cnt As Control
                                               With TP_CONTROLS
                                                   If .RowStyles.Count > 1 Then
                                                       For i% = .RowStyles.Count - 1 To ReflectedRow Step -1
                                                           cnt = .GetControlFromPosition(0, i)
                                                           If Not cnt Is Nothing Then .SetCellPosition(cnt, New TableLayoutPanelCellPosition(0, i + offset))
                                                       Next
                                                   End If
                                               End With
                                           End Sub, EDP.None)
        End Sub
        Private Sub RefillColors()
            ControlInvokeFast(TP_CONTROLS, Sub()
                                               With TP_CONTROLS
                                                   If .Controls.Count > 0 Then
                                                       Dim i% = 0
                                                       Dim c As Color
                                                       For Each cnt As MediaItem In .Controls
                                                           i += 1
                                                           If (i Mod 2) = 0 Then c = SystemColors.ControlLight Else c = SystemColors.Window
                                                           cnt.BackColor = c
                                                       Next
                                                   End If
                                               End With
                                           End Sub, EDP.None)
        End Sub
        Private Sub UpdateScrolls(sender As Object, e As EventArgs) Handles TP_CONTROLS.StyleChanged, Me.ResizeEnd, Me.SizeChanged
            ControlInvokeFast(TP_CONTROLS, Sub()
                                               With TP_CONTROLS
                                                   .SuspendLayout()
                                                   .Padding = New Padding(0, 0, .VerticalScroll.Visible.BoolToInteger * 3, 0)
                                                   .HorizontalScroll.Visible = False
                                                   .HorizontalScroll.Enabled = False
                                                   .ResumeLayout()
                                                   .PerformLayout()
                                               End With
                                           End Sub, EDP.None)
        End Sub
#End Region
#Region "Toolbar controls handlers"
        Protected Overridable Sub BTT_SETTINGS_Click(sender As Object, e As EventArgs) Handles BTT_SETTINGS.Click
            MyYouTubeSettings.ShowForm(AppMode)
        End Sub
        Protected Overridable Sub BTT_ADD_KeyClick(ByVal Sender As ToolStripMenuItemKeyClick, ByVal e As KeyClickEventArgs) Handles BTT_ADD.KeyClick, BTT_ADD_PLS_ARR.KeyClick
            Dim pForm As ParsingProgressForm = Nothing
            Try
                Dim useCookies As Boolean = MyYouTubeSettings.DefaultUseCookies
                Dim sTag$ = If(Sender?.Tag, String.Empty)
                Dim disableDown As Boolean = e.Shift
                If e.Control Then useCookies = True
                Dim useCookiesParse As Boolean? = Nothing
                If useCookies Then useCookiesParse = True
                Dim standardizeUrls As Boolean = MyYouTubeSettings.StandardizeURLs
                Dim standardize As Func(Of String, String) = Function(input) If(standardizeUrls, YouTubeFunctions.StandardizeURL(input), input.StringTrim)

                Dim c As IYouTubeMediaContainer = Nothing
                Dim url$ = String.Empty

                If sTag = "pls" Then
                    Using pf As New PlaylistArrayForm With {.DesignXML = DesignXML}
                        pf.ShowDialog()
                        If pf.DialogResult = DialogResult.OK Then
                            With pf.URLs
                                If .Count > 0 Then
                                    pForm = New ParsingProgressForm(.Count)
                                    pForm.Show(Me)
                                    pForm.SetInitialValues(.Count, "Parsing playlists...")
                                    Dim containers As New List(Of IYouTubeMediaContainer)
                                    For Each u$ In .Self
                                        containers.Add(YouTubeFunctions.Parse(standardize(u), useCookiesParse, pForm.Token, pForm.MyProgress))
                                        pForm.NextPlaylist()
                                        pForm.MyProgress.Perform()
                                    Next
                                    pForm.Dispose()
                                    If containers.Count > 0 Then containers.ListDisposeRemoveAll(Function(cc) cc.HasError Or Not cc.Exists)
                                    If containers.Count > 0 Then
                                        c = New Channel With {
                                            .UserTitle = IIf(pf.IsOneArtist, containers(0).UserTitle, "Playlists"),
                                            .IsMusic = containers.Any(Function(cc) cc.IsMusic)
                                        }
                                        c.Elements.AddRange(containers)
                                        Dim path$ = c.Elements(0).File.PathWithSeparator
                                        For Each list As List(Of String) In {
                                            c.Elements.Select(Function(cc) cc.UserTitle).ListWithRemove(Function(cc) cc.IsEmptyString).ListIfNothing,
                                            c.Elements.Select(Function(cc) cc.PlaylistTitle).ListWithRemove(Function(cc) cc.IsEmptyString).ListIfNothing
                                        }
                                            If list.Count > 0 AndAlso
                                               (list.Count = 1 OrElse
                                               ListAddList(Nothing, list, LAP.NotContainsOnly, EDP.ReturnValue).ListIfNothing.Count = 1) Then _
                                               path &= $"{list(0)}\"
                                        Next
                                        c.File = path
                                    End If
                                End If
                            End With
                        End If
                    End Using
                Else
                    url = BufferText
                    If url.IsEmptyString OrElse Not YouTubeFunctions.IsMyUrl(url) Then url = InputBoxE("Enter a valid URL to the YouTube video:", "YouTube link")
                End If

                If Not c Is Nothing OrElse YouTubeFunctions.IsMyUrl(url) Then
                    If c Is Nothing Then
                        Dim downAsIs As Boolean = False
                        Dim channelTab As YouTubeChannelTab? = Nothing
                        Dim __channelTab As YouTubeChannelTab = YouTubeChannelTab.All
                        Dim oType As YouTubeMediaType = YouTubeFunctions.Info_GetUrlType(url,,,,, __channelTab)
                        If oType = YouTubeMediaType.Channel Then
                            channelTab = __channelTab
                            Using channelTabForm As New ChannelTabsChooserForm(__channelTab, url) With {.DesignXML = DesignXML}
                                With channelTabForm
                                    .ShowDialog()
                                    If .DialogResult = DialogResult.OK Then
                                        channelTab = .Result
                                        downAsIs = .MyUrlAsIs
                                        url = YouTubeFunctions.StandardizeURL_Channel(.URL, Not downAsIs)
                                    Else
                                        Exit Sub
                                    End If
                                End With
                            End Using
                        End If
                        pForm = New ParsingProgressForm
                        pForm.Show(Me)
                        pForm.SetInitialValues(1, "Parsing data...")
                        c = YouTubeFunctions.Parse(standardize(url), useCookiesParse, pForm.Token, pForm.MyProgress,,, channelTab,
                                                   oType <> YouTubeMediaType.Channel Or downAsIs)
                        pForm.Dispose()
                    End If
                    If Not c Is Nothing Then
                        Dim f As Form
                        Select Case c.ObjectType
                            Case YouTubeMediaType.Single : f = New VideoOptionsForm(c) With {.UseCookies = useCookies}
                            Case YouTubeMediaType.Channel, YouTubeMediaType.PlayList
                                If c.IsMusic Then
                                    f = New MusicPlaylistsForm(c)
                                Else
                                    f = New VideoOptionsForm(c) With {.UseCookies = useCookies}
                                End If
                            Case Else : c.Dispose() : Throw New ArgumentException($"Object type {c.ObjectType} not implemented", "IYouTubeMediaContainer.ObjectType")
                        End Select
                        If Not f Is Nothing Then
                            If TypeOf f Is IDesignXMLContainer Then DirectCast(f, IDesignXMLContainer).DesignXML = DesignXML
                            f.ShowDialog()
                            If f.DialogResult = DialogResult.OK AndAlso ValidateContainerURL(c) Then ControlCreateAndAdd(c, disableDown,,, useCookies)
                            f.Dispose()
                        End If
                    End If
                End If
            Catch oex As OperationCanceledException
            Catch dex As ObjectDisposedException
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "VideoListForm.Add")
                UpdateLogButton()
            Finally
                If Not pForm Is Nothing Then pForm.Dispose()
            End Try
        End Sub
        Protected Function ValidateContainerURL(ByVal c As IYouTubeMediaContainer) As Boolean
            Try
                If Not c Is Nothing AndAlso Not c.IsMusic Then
                    Dim urls As List(Of String) = Nothing
                    Dim files As List(Of SFile) = Nothing
                    Dim msg As New MMessage("The media file to be added is already downloaded. Do you want to download it again?", "Download media file", {"Process", "Cancel"}, vbExclamation)
                    If TP_CONTROLS.Controls.Count > 0 Then
                        With TP_CONTROLS.Controls.Cast(Of MediaItem)
                            urls.ListAddList(.SelectMany(Function(ByVal m As MediaItem) As IEnumerable(Of String)
                                                             If Not m.MyContainer Is Nothing Then
                                                                 Return DirectCast(m.MyContainer, YouTubeMediaContainerBase).GetUrls()
                                                             Else
                                                                 Return New String() {}
                                                             End If
                                                         End Function), LAP.NotContainsOnly, EDP.ReturnValue)
                            files.ListAddList(.SelectMany(Function(ByVal m As MediaItem) As IEnumerable(Of SFile)
                                                              If Not m.MyContainer Is Nothing Then
                                                                  Return DirectCast(m.MyContainer, YouTubeMediaContainerBase).GetFiles()
                                                              Else
                                                                  Return New SFile() {}
                                                              End If
                                                          End Function), LAP.NotContainsOnly, EDP.ReturnValue)
                        End With
                    End If
                    If urls.ListExists Then
                        Dim cUrls As New List(Of String)
                        cUrls.ListAddList({c.URL, c.URL_BASE}, LAP.NotContainsOnly)
                        If urls.ListContains(cUrls) Then Return msg.Show = 0
                    End If
                    If files.ListExists And Not c.File.IsEmptyString Then Return Not files.Contains(c.File) OrElse msg.Show = 0
                    If c.ObjectType = YouTubeMediaType.Single AndAlso c.File.Exists Then
                        Dim callBack As MsgBoxButtonCallBack = Sub(r, m, b)
                                                                   Dim __sfo As SFO = IIf(r.Button.CallBackObject = 0, SFO.File, SFO.Path)
                                                                   If __sfo = SFO.File Then
                                                                       c.File.Open(__sfo)
                                                                   Else
                                                                       GlobalOpenPath(c.File)
                                                                   End If
                                                               End Sub
                        Return MsgBoxE(New MMessage("The following file already exists at the destination." & vbCr &
                                        "Do you want to download it again?" & vbCr & vbCr &
                                        $"File: {c.File}", "Download media file",
                                        {"Process",
                                         New MsgBoxButton("Open file") With {.IsDialogResultButton = False, .CallBackObject = 0, .CallBack = callBack},
                                         New MsgBoxButton("Open folder") With {.IsDialogResultButton = False, .CallBackObject = 1, .CallBack = callBack},
                                         "Cancel"}, vbExclamation) With {.ButtonsPerRow = 4}) = 0
                    End If
                    urls.ListClearDispose
                    files.ListClearDispose
                End If
                Return True
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, "[VideoListForm.ValidateContainerURL]", True)
            End Try
        End Function
        Private Sub BTT_DOWN_Click(sender As Object, e As EventArgs) Handles BTT_DOWN.Click
            With TP_CONTROLS
                If .Controls.Count > 0 Then
                    For Each cnt As MediaItem In .Controls
                        If Not cnt.MyContainer.MediaState = Plugin.UserMediaStates.Downloaded And Not cnt.Pending Then AddToDownload(cnt, False)
                    Next
                End If
            End With
            StartDownloading()
        End Sub
        Private Sub BTT_STOP_Click(sender As Object, e As EventArgs) Handles BTT_STOP.Click
            ControlInvoke(TOOLBAR_TOP, BTT_STOP, Sub() BTT_STOP.Enabled = False, EDP.SendToLog)
            MyJob.Cancel()
        End Sub
#Region "Delete / Clear"
        Private Sub BTT_DELETE_Click(sender As Object, e As EventArgs) Handles BTT_DELETE.Click
            RemoveControls(ControlsChecked, True)
        End Sub
        Private Sub BTT_CLEAR_SELECTED_Click(sender As Object, e As EventArgs) Handles BTT_CLEAR_SELECTED.Click
            RemoveControls(ControlsChecked, False)
        End Sub
        Protected Overridable Sub BTT_CLEAR_DONE_Click(sender As Object, e As EventArgs) Handles BTT_CLEAR_DONE.Click
            RemoveControls(ControlsDownloaded, False)
        End Sub
        Protected Overridable Sub BTT_CLEAR_ALL_Click(sender As Object, e As EventArgs) Handles BTT_CLEAR_ALL.Click
            RemoveControls(, False)
        End Sub
        Private Sub BTT_SELECT_ALL_Click(sender As Object, e As EventArgs) Handles BTT_SELECT_ALL.Click, BTT_SELECT_NONE.Click
            Try
                Dim checked As Boolean = sender Is BTT_SELECT_ALL
                ControlInvokeFast(TP_CONTROLS, Sub()
                                                   With TP_CONTROLS.Controls
                                                       If .Count > 0 Then
                                                           For Each cnt As MediaItem In .Self : cnt.Checked = checked : Next
                                                       End If
                                                   End With
                                               End Sub, EDP.None)
            Catch
            End Try
        End Sub
#End Region
#Region "LOG"
        Private Sub BTT_LOG_Click(sender As Object, e As EventArgs) Handles BTT_LOG.Click
            MyMainLOG_ShowForm(DesignXML,,,, AddressOf UpdateLogButton)
        End Sub
        Private Sub UpdateLogButton()
            If AppMode Then
                Try : MyMainLOG_UpdateLogButton(BTT_LOG, TOOLBAR_TOP) : Catch : End Try
            End If
        End Sub
        Private _LogUpdateButtonSuspended As Boolean = False
        Private Sub ProgramLog_TextAdded(ByVal Sender As Object, ByVal e As EventArgs)
            If Not _LogUpdateButtonSuspended Then
                _LogUpdateButtonSuspended = True
                Try : ControlInvokeFast(TOOLBAR_TOP, BTT_LOG, AddressOf UpdateLogButton, EDP.None) : Catch : End Try
            End If
        End Sub
        Private Sub ProgramLog_TextCleared(ByVal Sender As Object, ByVal e As EventArgs)
            _LogUpdateButtonSuspended = False
        End Sub
#End Region
        Private Sub BTT_BUG_REPORT_Click(sender As Object, e As EventArgs) Handles BTT_BUG_REPORT.Click
            Try
                With MyYouTubeSettings
                    Using f As New Editors.BugReporterForm(MyCache, .DesignXml, .ProgramText, My.Application.Info.Version,
                                                           True, .Self, .ProgramDescription) : f.ShowDialog() : End Using
                End With
            Catch
            End Try
        End Sub
        Private Sub BTT_DONATE_Click(sender As Object, e As EventArgs) Handles BTT_DONATE.Click
            Try : Process.Start("https://github.com/AAndyProgram/SCrawler/blob/main/HowToSupport.md") : Catch : End Try
        End Sub
        Private Sub BTT_INFO_Click(sender As Object, e As EventArgs) Handles BTT_INFO.Click
            CheckVersionImpl(True)
        End Sub
        Protected Sub CheckVersionImpl(ByVal Force As Boolean)
            CheckVersion(Force)
        End Sub
        Protected Overloads Sub RemoveControls(Optional ByVal Predicate As Predicate(Of MediaItem) = Nothing, Optional ByVal RemoveFiles As Boolean = False)
            ControlInvokeFast(TP_CONTROLS, Sub()
                                               With TP_CONTROLS
                                                   If .Controls.Count > 0 Then
                                                       Dim i%
                                                       Dim rCnt As New List(Of Integer)
                                                       Dim predicateExists As Boolean = Not Predicate Is Nothing
                                                       For i = 0 To .Controls.Count - 1
                                                           If Not predicateExists OrElse Predicate.Invoke(.Controls(i)) Then rCnt.Add(i)
                                                       Next
                                                       If rCnt.Count > 0 Then
                                                           Dim cnt As MediaItem
                                                           For i = rCnt.Count - 1 To 0 Step -1
                                                               cnt = .Controls(rCnt(i))
                                                               .Controls.RemoveAt(rCnt(i))
                                                               If Not cnt.MyContainer Is Nothing Then cnt.MyContainer.Delete(RemoveFiles) : cnt.MyContainer.Dispose()
                                                               cnt.Dispose()
                                                           Next
                                                       End If
                                                   End If
                                                   If .Controls.Count > 0 Then
                                                       OffsetControls()
                                                   Else
                                                       .RowStyles.Clear()
                                                       .RowStyles.Add(New RowStyle(SizeType.AutoSize))
                                                       .RowCount = 1
                                                   End If
                                               End With
                                               UpdateScrolls(Nothing, Nothing)
                                           End Sub, EDP.None)
        End Sub
        Private Overloads Sub RemoveControls(ByVal CNT As MediaItem, Optional ByVal RemoveFiles As Boolean = False)
            ControlInvokeFast(TP_CONTROLS, Sub()
                                               If Not CNT Is Nothing Then
                                                   If Not CNT.MyContainer Is Nothing Then CNT.MyContainer.Delete(RemoveFiles) : CNT.MyContainer.Dispose()
                                                   TP_CONTROLS.Controls.Remove(CNT)
                                                   OffsetControls()
                                                   CNT.Dispose()
                                               End If
                                           End Sub, EDP.None)
        End Sub
#End Region
#Region "Media controls' handlers"
        Private Sub MediaControl_FileDownloaded(ByVal Sender As MediaItem, ByVal Container As IYouTubeMediaContainer)
            If MyDownloaderSettings.ShowNotifications Then MyNotificator.ShowNotification(Container.ToString(), Container.ThumbnailFile)
            If MyDownloaderSettings.RemoveDownloadedAutomatically Then RemoveControls(Sender, False)
        End Sub
        Private Sub MediaControl_Removal(ByVal Sender As MediaItem, ByVal Container As IYouTubeMediaContainer)
            RemoveControls(Sender, False)
        End Sub
        Private Sub MediaControl_DownloadAgain(ByVal Sender As MediaItem, ByVal Container As IYouTubeMediaContainer)
            Try
                If Not Container.URL.IsEmptyString Then BufferText = Container.URL : BTT_ADD.PerformClick()
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[VideoListForm.DownloadAgain]")
            End Try
        End Sub
        Private Sub MediaControl_DownloadRequested(ByVal Sender As MediaItem, ByVal Container As IYouTubeMediaContainer)
            AddToDownload(Sender, True)
        End Sub
        Private Sub MediaControl_CheckedChanged(ByVal Sender As MediaItem, ByVal Container As IYouTubeMediaContainer)
            Try
                With TP_CONTROLS.Controls
                    ControlInvokeFast(TOOLBAR_TOP, BTT_DELETE,
                                      Sub() BTT_DELETE.Enabled = .Count > 0 AndAlso .Cast(Of MediaItem).ListExists(Function(cnt) cnt.Checked), EDP.None)
                End With
            Catch
            End Try
        End Sub
#End Region
#End Region
#Region "Downloading"
        Protected Overridable Sub MyJob_Started(ByVal Sender As Object, ByVal e As EventArgs) Handles MyJob.Started
        End Sub
        Protected Overridable Sub MyJob_Finished(ByVal Sender As Object, ByVal e As EventArgs) Handles MyJob.Finished
            UpdateLogButton()
        End Sub
        Protected Sub AddToDownload(ByRef Item As MediaItem, ByVal RunThread As Boolean)
            Try
                Dim hc% = Item.MyContainer.GetHashCode
                If MyJob.Count = 0 OrElse Not MyJob.Items.Exists(Function(i) i.MyContainer.GetHashCode = hc) Then
                    MyJob.Add(Item)
                    Item.AddToQueue()
                    If RunThread Then StartDownloading()
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[VideoListForm.AddToDownload]")
            End Try
        End Sub
        Private Sub StartDownloading()
            If Not MyJob.Working And MyJob.Count > 0 Then
                EnableDownloadButtons(True)
                MyJob.StartThread(AddressOf DownloadData)
            End If
        End Sub
        Private Sub EnableDownloadButtons(ByVal Downloading As Boolean)
            ControlInvoke(TOOLBAR_TOP, BTT_DOWN, Sub()
                                                     BTT_DOWN.Enabled = Not Downloading
                                                     BTT_STOP.Enabled = Downloading
                                                 End Sub, EDP.SendToLog)
        End Sub
        Private ReadOnly PNumProv As New ANumbers With {.FormatOptions = ANumbers.Options.GroupIntegral}
        Private Sub DownloadData()
            Try
                MyJob.Start()
                Const nf As ANumbers.Formats = ANumbers.Formats.Number
                Dim t As New List(Of Task)
                Dim i%, iAbs%
                Dim __item As MediaItem
                Dim Indexes As New List(Of Integer)
                Dim IndexesToRemove As New List(Of Integer)
                Dim maxJobCount% = MyDownloaderSettings.MaxJobsCount
                If maxJobCount <= 0 Then maxJobCount = 1
                MyProgress.Visible = True
                MyProgress.Maximum = MyJob.Count
                Do While MyJob.Count > 0 And Not MyJob.IsCancellationRequested
                    i = -1
                    iAbs = -1
                    Indexes.Clear()
                    IndexesToRemove.Clear()
                    For Each __item In MyJob.Items
                        iAbs += 1
                        If Not __item.IsDisposed And Not If(__item.MyContainer?.DownloadState, Plugin.UserMediaStates.Unknown) = Plugin.UserMediaStates.Downloaded Then
                            i += 1
                            If i <= maxJobCount - 1 Then
                                Indexes.Add(iAbs)
                                t.Add(Task.Run(Sub() __item.Download(MyJob.Token)))
                            Else
                                Exit For
                            End If
                        Else
                            IndexesToRemove.Add(iAbs)
                        End If
                    Next
                    If IndexesToRemove.Count > 0 Then
                        For i = IndexesToRemove.Count - 1 To 0 Step -1
                            If Not MyJob.Items(IndexesToRemove(i)).IsDisposed Then MyJob.Items(IndexesToRemove(i)).Pending = False
                            MyJob.Items.RemoveAt(IndexesToRemove(i))
                        Next
                    End If
                    If t.Count > 0 Then
                        MyProgress.Information = $"Downloading {t.Count.NumToString(nf, PNumProv)}/{MyJob.Count.NumToString(nf, PNumProv)}"
                        MyProgress.InformationTemporary = MyProgress.Information
                        Task.WaitAll(t.ToArray)
                        MyProgress.Perform(t.Count)
                        If Indexes.Count > 0 Then
                            For i = Indexes.Count - 1 To 0 Step -1
                                MyJob.Item(Indexes(i)).Pending = False
                                MyJob.Items.RemoveAt(Indexes(i))
                            Next
                        End If
                        t.Clear()
                    End If
                Loop
                Indexes.Clear()
                IndexesToRemove.Clear()
                MyProgress.Done()
                MyProgress.InformationTemporary = "Download completed"
            Catch aoex As ArgumentOutOfRangeException
            Catch oex As OperationCanceledException
                MyProgress.InformationTemporary = "Download canceled"
            Catch ex As Exception
                MyProgress.InformationTemporary = "Download error"
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[VideoListForm.DownloadData]")
            Finally
                MyProgress.Visible(, False) = False
                MyJob.Finish()
                EnableDownloadButtons(False)
            End Try
        End Sub
#End Region
    End Class
End Namespace