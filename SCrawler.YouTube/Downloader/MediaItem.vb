' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports SCrawler.API.YouTube
Imports SCrawler.API.YouTube.Objects
Imports SCrawler.API.YouTube.Controls
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Functions.Messaging
Namespace DownloadObjects.STDownloader
    Public Delegate Sub MediaItemEventHandler(ByVal Sender As MediaItem, ByVal Container As IYouTubeMediaContainer)
    <DefaultEvent("DoubleClick"), DesignTimeVisible(False), ToolboxItem(False)>
    Public Class MediaItem : Implements ISupportInitialize
#Region "Events"
        Public Event DownloadStarted As MediaItemEventHandler
        Public Event FileDownloaded As MediaItemEventHandler
        Public Event Removal As MediaItemEventHandler
        Public Event DownloadAgain As MediaItemEventHandler
        Public Event DownloadRequested As MediaItemEventHandler
        Public Event CheckedChanged As MediaItemEventHandler
        Public Event BeforeOpenEditor As MediaItemEventHandler
        Public Event BeforeOpenEditorFull As MediaItemEventHandler
#End Region
#Region "Declarations"
#Region "Controls"
        Private WithEvents TP_CONTROLS As TableLayoutPanel
        Private WithEvents TP_PROGRESS As TableLayoutPanel
        Private WithEvents ICON_SITE As PictureBox
        Private WithEvents ICON_CLOCK As PictureBox
        Private WithEvents ICON_WHAT As PictureBox
        Private WithEvents LBL_TIME As Label '54
        Private WithEvents ICON_SIZE As PictureBox
        Private WithEvents LBL_SIZE As Label '68
        Private WithEvents LBL_INFO As Label
        Private WithEvents LBL_PROGRESS As Label
        Private WithEvents PR_MAIN As ProgressBar
#End Region
        Private ReadOnly BindedControls As List(Of MediaItem)
        <Browsable(False)> Public Property MyContainer As IYouTubeMediaContainer
        Private ReadOnly Property MyProgress As MyProgress
        <Browsable(False)> Public Property UseCookies As Boolean
        <Browsable(False)> Public Property Pending As Boolean = False
        <Browsable(False)> Public Property Checked As Boolean
            Get
                Return ControlInvokeFast(CH_CHECKED, Function() CH_CHECKED.Checked, False, EDP.ReturnValue)
            End Get
            Set(ByVal _Checked As Boolean)
                ControlInvokeFast(CH_CHECKED, Sub() CH_CHECKED.Checked = _Checked, EDP.None)
            End Set
        End Property
        Private ReadOnly FileOption As SFO = SFO.File
        Private ReadOnly ContainerHasElements As Boolean = False
#End Region
#Region "Initializers"
        Public Sub New()
            InitializeComponent()
            BindedControls = New List(Of MediaItem)

            CreateLabel(LBL_PROGRESS)
            PR_MAIN = New ProgressBar With {.Anchor = AnchorStyles.Left + AnchorStyles.Right, .Size = New Size(.Size.Width, 18), .ContextMenuStrip = CONTEXT_MAIN}
            TP_CONTROLS = New TableLayoutPanel With {.Dock = DockStyle.Fill, .ContextMenuStrip = CONTEXT_MAIN}
            TP_PROGRESS = New TableLayoutPanel With {.Dock = DockStyle.Fill, .ContextMenuStrip = CONTEXT_MAIN}
            With TP_PROGRESS
                With .ColumnStyles
                    .Add(New ColumnStyle(SizeType.Percent, 40))
                    .Add(New ColumnStyle(SizeType.Percent, 60))
                End With
                .ColumnCount = .ColumnStyles.Count
                .RowStyles.Add(New RowStyle(SizeType.Percent, 100))
                .RowCount = .RowStyles.Count
                .Controls.Add(PR_MAIN, 0, 0)
                .Controls.Add(LBL_PROGRESS, 1, 0)
            End With
            With TP_CONTROLS
                .RowStyles.Add(New RowStyle(SizeType.Percent, 100))
                .RowCount = .RowStyles.Count
            End With

            CreateIcon(ICON_SITE)
            CreateIcon(ICON_WHAT)
            CreateIcon(ICON_CLOCK, My.Resources.ClockPic_16)
            CreateLabel(LBL_TIME)
            CreateIcon(ICON_SIZE, My.Resources.RulerPic_32)
            CreateLabel(LBL_SIZE)
            CreateLabel(LBL_INFO)

            MyProgress = New MyProgress(PR_MAIN, LBL_PROGRESS)
        End Sub
        Private Sub CreateLabel(ByRef LBL As Label)
            LBL = New Label With {
                .Text = String.Empty,
                .Margin = New Padding(0),
                .AutoSize = False,
                .Dock = DockStyle.Fill,
                .TextAlign = ContentAlignment.MiddleLeft,
                .Font = New Font("Arial", 9, FontStyle.Bold, GraphicsUnit.Point, 204),
                .ForeColor = ForeColorLabels,
                .ContextMenuStrip = CONTEXT_MAIN
            }
        End Sub
        Private Sub CreateIcon(ByRef Obj As PictureBox, Optional ByVal Image As Image = Nothing)
            Obj = New PictureBox With {
                .Margin = New Padding(3),
                .BackgroundImageLayout = ImageLayout.Zoom,
                .SizeMode = PictureBoxSizeMode.Zoom,
                .Dock = DockStyle.Fill,
                .Image = Image,
                .ContextMenuStrip = CONTEXT_MAIN
            }
        End Sub
        Public Sub New(ByVal Container As IYouTubeMediaContainer, Optional ByVal HasElements As Boolean = False)
            Me.New
            Const d$ = " " & ChrW(183) & " "
            MyContainer = Container
            ContainerHasElements = HasElements
            With MyContainer
                .Progress = MyProgress
                If HasElements Then BTT_PLS_ITEM_EDIT.Visible = True : BTT_PLS_ITEM_EDIT_FULL.Visible = True : SEP_PLS_ITEM_EDIT.Visible = True
                If .HasElements Then FileOption = SFO.Path Else FileOption = SFO.File
                If .DownloadState = Plugin.UserMediaStates.Downloaded AndAlso
                   (.ObjectType = Base.YouTubeMediaType.Channel Or .ObjectType = Base.YouTubeMediaType.PlayList) AndAlso FileOption = SFO.File AndAlso
                   Not .File.Exists AndAlso .File.Exists(SFO.Path, False) Then FileOption = SFO.Path : BTT_OPEN_FILE.Visible = False
                If Not .SiteKey = YouTubeSiteKey Then
                    BTT_DOWN_AGAIN.Visible = False
                    SEP_DOWN_AGAIN.Visible = False
                End If

                ICON_SITE.Image = .SiteIcon
                LBL_TIME.Text = AConvert(Of String)(.Duration, TimeToStringProvider, String.Empty)
                LBL_TITLE.Text = $"{If(MyYouTubeSettings.FileAddDateToFileName_VideoList.Value, $"[{ .DateAdded:yyyy-MM-dd}] ", String.Empty)}{ .ToString(True)}"
                Dim h%, b%
                If .Self.GetType Is GetType(YouTubeMediaContainerBase) OrElse (Not .Self.GetType.BaseType Is Nothing AndAlso .Self.GetType.BaseType Is GetType(YouTubeMediaContainerBase)) Then
                    With DirectCast(.Self, YouTubeMediaContainerBase) : h = .HeightBase : b = .BitrateBase : End With
                Else
                    h = .Height
                    b = .Bitrate
                End If
                If Not .SiteKey = YouTubeSiteKey And .ContentType = Plugin.UserMediaTypes.Picture Then
                    LBL_INFO.Text = .File.Extension.StringToUpper
                ElseIf Not .IsMusic And Not (.MediaType = Plugin.UserMediaTypes.Audio Or .MediaType = Plugin.UserMediaTypes.AudioPre) Then
                    If h > 0 Then
                        LBL_INFO.Text = $"{ .File.Extension.StringToUpper}{d}{h}p"
                    Else
                        LBL_INFO.Text = .File.Extension.StringToUpper
                    End If
                Else
                    If b > 0 Then
                        LBL_INFO.Text = $"{ .File.Extension.StringToUpper}{d}{b}k"
                    Else
                        LBL_INFO.Text = .File.Extension.StringToUpper
                    End If
                End If
            End With
            UpdateMediaIcon()
        End Sub
#End Region
#Region "Control handlers"
        Private Sub MediaItem_Load(sender As Object, e As EventArgs) Handles Me.Load
            RefillControls()
        End Sub
        Private Sub MediaItem_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            BindedControls.Clear()
            MyProgress.Dispose()
            ICON_SITE.Dispose()
            ICON_CLOCK.Dispose()
            ICON_WHAT.Dispose()
            LBL_TIME.Dispose()
            ICON_SIZE.Dispose()
            LBL_SIZE.Dispose()
            LBL_INFO.Dispose()
            LBL_PROGRESS.Dispose()
            PR_MAIN.Dispose()
            TP_CONTROLS.Controls.Clear()
            TP_CONTROLS.Dispose()
            TP_PROGRESS.Controls.Clear()
            TP_PROGRESS.Dispose()
        End Sub
#End Region
#Region "RefillControls"
        Private Sub UpdateMediaIcon()
            ControlInvokeFast(Me, Sub()
                                      With MyContainer
                                          If Not .SiteKey = YouTubeSiteKey And .ContentType = Plugin.UserMediaTypes.Picture Then
                                              ICON_WHAT.Image = My.Resources.ImagePic_32
                                          ElseIf .IsMusic Or .MediaType = Plugin.UserMediaTypes.Audio Or .MediaType = Plugin.UserMediaTypes.AudioPre Then
                                              ICON_WHAT.Image = My.Resources.AudioMusic_32
                                          Else
                                              ICON_WHAT.Image = My.Resources.VideoCamera_32
                                          End If
                                      End With
                                  End Sub, EDP.None)
        End Sub
        Private Sub RefillControls()
            ControlInvokeFast(Me, AddressOf RefillControlsImpl, EDP.None)
        End Sub
        Private Sub RefillControlsImpl()
            With MyContainer
                If ICON_VIDEO.Image Is Nothing Then
                    If .ThumbnailFile.Exists Then
                        ICON_VIDEO.Image = ImageRenderer.GetImage(SFile.GetBytes(.ThumbnailFile, EDP.ReturnValue), EDP.ReturnValue)
                    ElseIf Not .ThumbnailUrlMedia.IsEmptyString Then
                        ICON_VIDEO.Image = ImageRenderer.GetImage(SFile.GetBytesFromNet(.ThumbnailUrlMedia, EDP.ReturnValue), EDP.ReturnValue)
                    End If
                End If
                Dim s%, t%
                Dim sv% = .Size / 1024
                If sv >= 1000 Then
                    LBL_SIZE.Text = AConvert(Of String)(sv / 1024, VideoSizeProvider)
                    LBL_SIZE.Text &= " GB"
                Else
                    LBL_SIZE.Text = AConvert(Of String)(sv, VideoSizeProvider)
                    LBL_SIZE.Text &= " MB"
                End If
                If .Size > 0 Then
                    s = MeasureTextDefault(LBL_SIZE.Text, LBL_SIZE.Font).Width
                Else
                    s = 0
                End If
                If .Duration.TotalSeconds > 0 Then
                    t = MeasureTextDefault(LBL_TIME.Text, LBL_TIME.Font).Width
                Else
                    t = 0
                End If

                LBL_TITLE.Text = $"{If(MyYouTubeSettings.FileAddDateToFileName_VideoList.Value, $"[{ .DateAdded:yyyy-MM-dd}] ", String.Empty)}{ .ToString(True)}"

                If Not .SiteKey = YouTubeSiteKey Then BTT_VIEW_SETTINGS.Visible = False

                With TP_CONTROLS
                    .Controls.Clear()
                    .ColumnStyles.Clear()
                    .ColumnCount = 0
                    If ContainerHasElements Or MyContainer.MediaState = Plugin.UserMediaStates.Downloaded Then
                        UpdateMediaIcon()
                        If ContainerHasElements Then
                            BTT_OPEN_FOLDER.Visible = False
                            BTT_OPEN_FILE.Visible = False
                            SEP_FOLDER.Visible = False
                            If Not ContainerHasElements Then
                                BTT_PLS_ITEM_EDIT.Visible = False
                                BTT_PLS_ITEM_EDIT_FULL.Visible = False
                                SEP_PLS_ITEM_EDIT.Visible = False
                            End If
                            BTT_DOWN_AGAIN.Visible = False
                            SEP_DOWN_AGAIN.Visible = False
                            BTT_REMOVE_FROM_LIST.Visible = False
                            BTT_DELETE_FILE.Visible = False
                            SEP_DEL.Visible = False
                        End If
                        BTT_DOWN.Visible = False
                        SEP_DOWN.Visible = False
                        BTT_VIEW_SETTINGS.Visible = False
                        With .ColumnStyles
                            .Add(New ColumnStyle(SizeType.Absolute, 30))
                            .Add(New ColumnStyle(SizeType.Absolute, 30))
                            .Add(New ColumnStyle(SizeType.Absolute, IIf(t = 0, 0, 30)))
                            .Add(New ColumnStyle(SizeType.Absolute, t))
                            .Add(New ColumnStyle(SizeType.Absolute, IIf(s = 0, 0, 30)))
                            .Add(New ColumnStyle(SizeType.Absolute, s))
                            .Add(New ColumnStyle(SizeType.Percent, 100))
                        End With
                        .ColumnCount = .ColumnStyles.Count
                        With .Controls
                            .Add(ICON_SITE, 0, 0)
                            .Add(ICON_WHAT, 1, 0)
                            If t > 0 Then
                                .Add(ICON_CLOCK, 2, 0)
                                .Add(LBL_TIME, 3, 0)
                            End If
                            If s > 0 Then
                                .Add(ICON_SIZE, 4, 0)
                                .Add(LBL_SIZE, 5, 0)
                            End If
                            .Add(LBL_INFO, 6, 0)
                        End With
                    Else
                        With .ColumnStyles
                            .Add(New ColumnStyle(SizeType.Absolute, 100))
                            .Add(New ColumnStyle(SizeType.Percent, 100))
                        End With
                        .ColumnCount = .ColumnStyles.Count
                        With .Controls
                            .Add(PR_MAIN, 0, 0)
                            .Add(LBL_PROGRESS, 1, 0)
                        End With
                    End If
                End With
                TP_INFO.Controls.Add(TP_CONTROLS, 0, 1)
                BTT_OPEN_FOLDER.Enabled = .File.Exists(FileOption, False)
                BTT_DELETE_FILE.Enabled = BTT_OPEN_FOLDER.Enabled
            End With
        End Sub
#End Region
#Region "Context buttons' handlers"
        Public Sub AddToQueue()
            ControlInvokeFast(Me, Sub()
                                      Pending = True
                                      BTT_DOWN.Visible = False
                                      SEP_DOWN.Visible = False
                                  End Sub, EDP.None)
        End Sub
        Private Sub BTT_DOWN_Click(sender As Object, e As EventArgs) Handles BTT_DOWN.Click
            RaiseEvent DownloadRequested(Me, MyContainer)
        End Sub
        Public Sub Download(ByVal Token As Threading.CancellationToken)
            Try
                If Not MyContainer Is Nothing Then
                    RaiseEvent DownloadStarted(Me, MyContainer)
                    AddToQueue()
                    MyContainer.Download(UseCookies, Token)
                    MyContainer.Save()
                    Pending = False
                    RefillControls()
                    RaiseEvent FileDownloaded(Me, MyContainer)
                End If
            Catch dex As ObjectDisposedException When MyContainer.IsDisposed
            Catch oex As OperationCanceledException When Token.IsCancellationRequested
                Throw oex
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"MediaItem.Download:{vbCr}{MyContainer.ToString}{vbCr}{MyContainer.URL})")
            Finally
                Pending = False
            End Try
        End Sub
#End Region
#Region "Colors"
        Private ReadOnly ForeColorLabels As Color = SystemColors.ControlDark
        Private ForeColorDefault As Color
        Public Overrides Property ForeColor As Color
            Get
                Return MyBase.ForeColor
            End Get
            Set(ByVal c As Color)
                ForeColorDefault = c
                MyBase.ForeColor = c
            End Set
        End Property
        Private BackColorDefault As Color
        Public Overrides Property BackColor As Color
            Get
                Return MyBase.BackColor
            End Get
            Set(ByVal c As Color)
                BackColorDefault = c
                MyBase.BackColor = c
            End Set
        End Property
        Private IsActiveControl As Boolean = False
        Private Sub DropColor()
            IsActiveControl = False
            MyBase.BackColor = BackColorDefault
            MyBase.ForeColor = ForeColorDefault
            ChangeLabelsColor(ForeColorLabels)
        End Sub
        Private Sub ChangeLabelsColor(ByVal ForeColor As Color)
            ControlInvokeFast(Me, Sub()
                                      LBL_TIME.ForeColor = ForeColor
                                      LBL_SIZE.ForeColor = ForeColor
                                      LBL_INFO.ForeColor = ForeColor
                                      LBL_PROGRESS.ForeColor = ForeColor
                                  End Sub, EDP.None)
        End Sub
#End Region
#Region "Click handlers"
        Public Sub PerformClick()
            Controls_Click(Me, EventArgs.Empty)
        End Sub
        Private Sub Controls_Click(sender As Object, e As EventArgs) Handles ICON_VIDEO.MouseClick, CH_CHECKED.MouseClick, LBL_TITLE.MouseClick, TP_INFO.MouseClick,
                                                                             TP_CONTROLS.MouseClick, TP_PROGRESS.MouseClick, ICON_SITE.MouseClick, ICON_CLOCK.MouseClick,
                                                                             ICON_WHAT.MouseClick, LBL_TIME.MouseClick, ICON_SIZE.MouseClick, LBL_INFO.MouseClick,
                                                                             LBL_PROGRESS.MouseClick, PR_MAIN.MouseClick, CONTEXT_MAIN.Opened
            IsActiveControl = True
            MyBase.BackColor = SystemColors.Highlight
            MyBase.ForeColor = SystemColors.HighlightText
            ChangeLabelsColor(SystemColors.HighlightText)
            BindedControls.ForEach(Sub(c) c.DropColor())
            OnClick(e)
        End Sub
        Private Sub Controls_DoubleClick(sender As Object, e As EventArgs) Handles ICON_VIDEO.DoubleClick, LBL_TITLE.DoubleClick, TP_INFO.DoubleClick,
                                                                                   TP_CONTROLS.DoubleClick, TP_PROGRESS.DoubleClick, ICON_SITE.DoubleClick, ICON_CLOCK.DoubleClick,
                                                                                   ICON_WHAT.DoubleClick, LBL_TIME.DoubleClick, ICON_SIZE.DoubleClick, LBL_INFO.DoubleClick,
                                                                                   LBL_PROGRESS.DoubleClick, PR_MAIN.DoubleClick
            Controls_Click(sender, e)
            If Not ContainerHasElements AndAlso Not MyDownloaderSettings.OnItemDoubleClick = DoubleClickBehavior.None Then
                Dim m As New MMessage("The specified path was not found.", "Open file/folder",, vbExclamation)
                If MyDownloaderSettings.OnItemDoubleClick = DoubleClickBehavior.File Then
                    If FileOption = SFO.File And MyContainer.File.Exists(SFO.File, False) Then
                        MyContainer.File.Open(SFO.File,, EDP.ShowMainMsg)
                    ElseIf MyContainer.File.Exists(SFO.Path, False) Then
                        GlobalOpenPath(MyContainer.File, EDP.ShowMainMsg)
                    Else
                        m.Show()
                    End If
                Else
                    If MyContainer.File.Exists(SFO.Path, False) Then GlobalOpenPath(MyContainer.File, EDP.ShowMainMsg) Else m.Show()
                End If
            End If
            OnDoubleClick(e)
        End Sub
        Private Sub CH_CHECKED_CheckedChanged(sender As Object, e As EventArgs) Handles CH_CHECKED.CheckedChanged
            RaiseEvent CheckedChanged(Me, MyContainer)
        End Sub
        Protected Overrides Function ProcessDialogKey(ByVal KeyData As Keys) As Boolean
            If IsActiveControl Then
                If KeyData = Keys.Down Or KeyData = Keys.Up Then
                    OnKeyDown(New KeyEventArgs(KeyData))
                    Return True
                Else
                    Return MyBase.ProcessDialogKey(KeyData)
                End If
            Else
                Return False
            End If
        End Function
#End Region
#Region "Context buttons' handlers"
        Private Sub BTT_OPEN_FOLDER_Click(sender As Object, e As EventArgs) Handles BTT_OPEN_FOLDER.Click
            If MyContainer.File.Exists(FileOption, False) Then GlobalOpenPath(MyContainer.File)
        End Sub
        Private Sub BTT_OPEN_FILE_Click(sender As Object, e As EventArgs) Handles BTT_OPEN_FILE.Click
            If MyContainer.File.Exists(SFO.File) Then MyContainer.File.Open(,, EDP.ShowAllMsg)
        End Sub
        Private Sub BTT_PLS_ITEM_EDIT_Click(sender As Object, e As EventArgs) Handles BTT_PLS_ITEM_EDIT.Click, BTT_PLS_ITEM_EDIT_FULL.Click
            If ContainerHasElements Then
                With DirectCast(MyContainer, YouTubeMediaContainerBase)
                    Dim initProtected As Boolean = .Protected
                    Dim isFull As Boolean = sender Is BTT_PLS_ITEM_EDIT_FULL
                    .Protected = False
                    If isFull Then
                        RaiseEvent BeforeOpenEditorFull(Me, MyContainer)
                    Else
                        RaiseEvent BeforeOpenEditor(Me, MyContainer)
                    End If
                    Using f As New VideoOptionsForm(MyContainer, initProtected Or isFull) With {.UseCookies = UseCookies}
                        f.ShowDialog()
                        .Protected = IIf(f.DialogResult = DialogResult.OK, True, initProtected)
                    End Using
                End With
            End If
        End Sub
        Private Sub BTT_COPY_LINK_Click(sender As Object, e As EventArgs) Handles BTT_COPY_LINK.Click
            If Not MyContainer.URL.IsEmptyString Then
                BufferText = MyContainer.URL
            Else
                MsgBoxE({"Media URL is not found", "Copy media URL"}, vbExclamation)
            End If
        End Sub
        Private Sub BTT_OPEN_IN_BROWSER_Click(sender As Object, e As EventArgs) Handles BTT_OPEN_IN_BROWSER.Click
            If Not MyContainer.URL_BASE.IsEmptyString Then
                Try : Process.Start(MyContainer.URL_BASE) : Catch : End Try
            Else
                MsgBoxE({"Media URL is not found", "Open link in browser"}, vbExclamation)
            End If
        End Sub
        Private Sub BTT_DOWN_AGAIN_Click(sender As Object, e As EventArgs) Handles BTT_DOWN_AGAIN.Click
            RaiseEvent DownloadAgain(Me, MyContainer)
        End Sub
        Private Sub BTT_VIEW_SETTINGS_Click(sender As Object, e As EventArgs) Handles BTT_VIEW_SETTINGS.Click
            If Not MyContainer Is Nothing Then
                Dim f As Form = Nothing
                Select Case MyContainer.ObjectType
                    Case Base.YouTubeMediaType.Single : f = New VideoOptionsForm(MyContainer, True) With {.UseCookies = UseCookies}
                    Case Base.YouTubeMediaType.Channel, Base.YouTubeMediaType.PlayList
                        If MyContainer.IsMusic Then
                            f = New MusicPlaylistsForm(MyContainer)
                        Else
                            f = New VideoOptionsForm(MyContainer, True) With {.UseCookies = UseCookies}
                        End If
                End Select
                If Not f Is Nothing Then
                    f.ShowDialog()
                    If f.DialogResult = DialogResult.OK Then MyContainer.Save()
                    f.Dispose()
                End If
            End If
        End Sub
        Private Sub BTT_REMOVE_FROM_LIST_Click(sender As Object, e As EventArgs) Handles BTT_REMOVE_FROM_LIST.Click
            RaiseEvent Removal(Me, MyContainer)
        End Sub
        Private Sub BTT_DELETE_FILE_Click(sender As Object, e As EventArgs) Handles BTT_DELETE_FILE.Click
            Dim opt$
            Dim opt2$ = String.Empty
            If FileOption = SFO.File Then
                opt = "file"
            Else
                opt = "item"
                opt2 = "THE ITEM MAY CONTAIN MULTIPLE FILES" & vbCr
            End If
            Dim b As New List(Of MsgBoxButton) From {New MsgBoxButton("Process")}
            If Not opt2.IsEmptyString Then _
               b.Add(New MsgBoxButton("Show files", "Show files to delete") With {
                     .IsDialogResultButton = False,
                     .CallBack = Function(r, m, bb) MsgBoxE(New MMessage($"The following files will be deleted:{vbCr}{vbCr}{MyContainer.Files.ListToString(vbCr)}",
                                                                         "Files to delete",, vbExclamation) With {.Editable = True})})
            b.Add(New MsgBoxButton("Cancel"))
            If MsgBoxE({$"Are you sure you want to delete the following {opt}:{vbCr}{opt2}" &
                        If(FileOption = SFO.File, MyContainer.File.ToString, MyContainer.ToString(True)),
                        $"Deleting {opt}"}, vbExclamation,,, b) = 0 Then
                MyContainer.Delete(True)
                RaiseEvent Removal(Me, MyContainer)
            End If
            b.Clear()
        End Sub
#End Region
#Region "ISupportInitialize Support"
        Public Sub BeginInit() Implements ISupportInitialize.BeginInit
        End Sub
        Public Sub EndInit() Implements ISupportInitialize.EndInit
            If Not Parent Is Nothing AndAlso TypeOf Parent Is TableLayoutPanel Then
                With DirectCast(Parent, TableLayoutPanel)
                    If .Controls.Count > 0 Then
                        For Each cnt As Control In .Controls
                            If Not cnt Is Nothing AndAlso TypeOf cnt Is MediaItem AndAlso Not cnt Is Me Then
                                With DirectCast(cnt, MediaItem)
                                    If Not BindedControls.Contains(cnt) Then BindedControls.Add(cnt)
                                End With
                            End If
                        Next
                    End If
                End With
            End If
        End Sub
#End Region
    End Class
End Namespace