' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Tools
Imports RCI = PersonalUtilities.Forms.Toolbars.RangeSwitcherToolbar.ControlItem
Imports UserMediaD = SCrawler.DownloadObjects.TDownloader.UserMediaD
Imports DTSModes = PersonalUtilities.Forms.DateTimeSelectionForm.Modes
Namespace DownloadObjects
    Friend Class DownloadFeedForm
#Region "Events"
        Friend Event UsersAdded As UsersAddedEventHandler
#End Region
#Region "Declarations"
        Private Const FeedTitleDefault As String = "Feed"
        Friend WithEvents MyDefs As DefaultFormOptions
        Private WithEvents MyRange As RangeSwitcherToolbar(Of UserMediaD)
        Private ReadOnly DataList As List(Of UserMediaD)
        Private WithEvents BTT_DELETE_SELECTED As ToolStripButton
        Private DataRows As Integer = 10
        Private DataColumns As Integer = 1
        Private FeedEndless As Boolean = False
        Private ReadOnly GoToButton As New ButtonKey(Keys.G, True)
        Private ReadOnly FilterSubscriptions As New FPredicate(Of UserMediaD)(Function(d) If(d.User?.IsSubscription, False))
        Private ReadOnly FilterUsers As New FPredicate(Of UserMediaD)(Function(d) Not FilterSubscriptions.Invoke(d))
        Private ReadOnly FileNotExist As New FPredicate(Of UserMediaD)(Function(d) Not d.Data.File.Exists And Not FilterSubscriptions.Invoke(d))
        Private ReadOnly SessionDateStringProvider As New CustomProvider(Function(v As SFile) AConvert(Of String)(AConvert(Of Date)(v.Name, SessionDateTimeProvider, v.Name),
                                                                                                                  DateTimeDefaultProvider, v.Name))
        Private BttRefreshToolTipText As String = "Refresh data list"
        Private CenterImage As Boolean = False
        Private NumberOfVisibleImages As Integer = 1
        Private ReadOnly Property IsSubscription As Boolean
            Get
                Return OPT_SUBSCRIPTIONS.Checked
            End Get
        End Property
#Region "Feeds options"
        Private Enum FeedModes : Current : Saved : Special : End Enum
        Private FeedMode As FeedModes = FeedModes.Current
        Private LoadedSessionName As String = String.Empty
        Private ReadOnly Property IsSession As Boolean
            Get
                Return FeedMode = FeedModes.Current Or FeedMode = FeedModes.Saved
            End Get
        End Property
        Private ReadOnly LoadedFeedNames As List(Of String)
        Private Sub FeedChangeMode(ByVal Mode As FeedModes, Optional ByVal fNames As IEnumerable(Of String) = Nothing)
            FeedMode = Mode
            LoadedFeedNames.Clear()
            If fNames.ListExists Then LoadedFeedNames.AddRange(fNames)
            Try : ControlInvokeFast(Me, Sub()
                                            Select Case FeedMode
                                                Case FeedModes.Current : Text = $"{FeedTitleDefault}: current session"
                                                Case FeedModes.Saved : Text = $"{FeedTitleDefault}: saved session {LoadedSessionName.IfNullOrEmpty("(multiple)")}"
                                                Case FeedModes.Special : Text = $"{FeedTitleDefault}: {IIf(LoadedFeedNames.Count > 1, "multiple special feeds", LoadedFeedNames.FirstOrDefault.IfNullOrEmpty("?"))}"
                                                Case Else : Text = FeedTitleDefault
                                            End Select
                                        End Sub, EDP.None) : Catch : End Try
        End Sub
        Private Sub FeedRemoveCheckedMedia(ByVal MediaList As IEnumerable(Of UserMediaD), Optional ByVal OverriddenNames As List(Of String) = Nothing,
                                           Optional ByVal RemoveChecked As Boolean = True, Optional ByVal ExcludingNames As IEnumerable(Of String) = Nothing,
                                           Optional ByVal RemoveFromDataListOnly As Boolean = False)
            Try
                If FeedMode = FeedModes.Special Then
                    If LoadedFeedNames.Count > 0 Then
                        Dim dataRemoved As Boolean = False
                        If OverriddenNames.ListExists And Not LoadedFeedNames.ListContains(OverriddenNames) Then Exit Sub
                        If Not RemoveFromDataListOnly Then
                            Dim eNames As IEnumerable(Of String) = If(ExcludingNames, New String() {})
                            With If(OverriddenNames, LoadedFeedNames)
                                .ForEach(Sub(ByVal feedName As String)
                                             If Not eNames.Contains(feedName) Then
                                                 Dim indx% = Settings.Feeds.IndexOf(feedName)
                                                 If indx >= 0 Then
                                                     If Settings.Feeds(indx).Remove(MediaList) > 0 Then dataRemoved = True
                                                 End If
                                             End If
                                         End Sub)
                            End With
                        End If
                        If RemoveFromDataListOnly Then
                            RefillSpecialFeedsData()
                        ElseIf dataRemoved Then
                            DataList.ListDisposeRemove(MediaList)
                            If RemoveChecked Then
                                If RemoveCheckedMedia(False) Then RefillAfterDelete()
                            Else
                                RefillSpecialFeedsData()
                            End If
                        End If
                    End If
                ElseIf FeedMode = FeedModes.Current Then
                    If OverriddenNames Is Nothing AndAlso Downloader.Files.ListDisposeRemove(MediaList) > 0 AndAlso RemoveCheckedMedia(False) Then
                        DataList.ListDisposeRemove(MediaList)
                        RefillAfterDelete()
                    End If
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadFeedForm.FeedRemoveCheckedMedia]")
            End Try
        End Sub
#End Region
#End Region
#Region "Initializer"
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            MyRange = New RangeSwitcherToolbar(Of UserMediaD)(ToolbarTOP)
            DataList = New List(Of UserMediaD)
            LoadedFeedNames = New List(Of String)
            BTT_DELETE_SELECTED = New ToolStripButton With {
                .Text = "Delete selected",
                .AutoToolTip = True,
                .ToolTipText = "Delete marked files",
                .Image = My.Resources.DeletePic_24,
                .DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
            }
        End Sub
#End Region
#Region "Form handlers"
        Private Sub DownloadFeedForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            BttRefreshToolTipText = BTT_REFRESH.ToolTipText
            With MyDefs
                .MyViewInitialize()
                LastWinState = WindowState
                With MyRange
                    .AutoToolTip = True
                    .Buttons = {RCI.First, RCI.Previous, RCI.Label, RCI.Next, RCI.Last, RCI.Separator, RCI.GoTo}
                    '.ButtonKey(RCI.Previous) = Keys.F3
                    '.ButtonKey(RCI.Next) = Keys.F4
                    '.ButtonKey(RCI.GoTo) = GoToButton
                    .ToolTip(RCI.First) = "Go to first page (Home)"
                    .ToolTip(RCI.Last) = "Go to last page (End)"
                    .ToolTip(RCI.Previous) = "Previous (F3, Up, Page Up)"
                    .ToolTip(RCI.Next) = "Next (F4, Down, Page Down)"
                    .ToolTip(RCI.GoTo) = "GoTo (Ctrl+G)"
                    .AddThisToolbar()
                End With
                ToolbarTOP.Items.AddRange({New ToolStripSeparator, BTT_DELETE_SELECTED})
                With Settings
                    With .Feeds
                        .Load()
                        AddHandler .FeedAdded, AddressOf Feed_FeedAdded
                        AddHandler .FeedRemoved, AddressOf Feed_FeedRemoved
                        If .Count > 0 Then
                            For Each feed As FeedSpecial In .Self
                                If Not feed.IsFavorite Then
                                    AddNewFeedItem(BTT_LOAD_SPEC, feed, My.Resources.RSSPic_512, AddressOf Feed_SPEC_LOAD)
                                    AddNewFeedItem(BTT_FEED_ADD_SPEC, feed, My.Resources.RSSPic_512, AddressOf Feed_SPEC_ADD)
                                    AddNewFeedItem(BTT_FEED_ADD_SPEC_REMOVE, feed, My.Resources.RSSPic_512, AddressOf Feed_SPEC_ADD_REMOVE)
                                    AddNewFeedItem(BTT_FEED_REMOVE_SPEC, feed, My.Resources.RSSPic_512, AddressOf Feed_SPEC_REMOVE)
                                    AddNewFeedItem(BTT_FEED_DELETE_SPEC, feed, My.Resources.DeletePic_24, AddressOf Feed_SPEC_DELETE)
                                    AddNewFeedItem(BTT_FEED_CLEAR_SPEC, feed, My.Resources.BrushToolPic_16, AddressOf Feed_SPEC_CLEAR)
                                End If
                            Next
                        End If
                    End With
                    If .FeedOpenLastMode Then
                        If .FeedLastModeSubscriptions Then OPT_SUBSCRIPTIONS.Checked = True Else OPT_DEFAULT.Checked = True
                    Else
                        OPT_DEFAULT.Checked = True
                        Settings.FeedLastModeSubscriptions.Value = False
                    End If
                End With
                MENU_DOWN.Visible = OPT_SUBSCRIPTIONS.Checked
                UpdateSettings()
                FeedChangeMode(FeedModes.Current)
                Downloader.FilesLoadLastSession()
                RefillList(True, False)
                .EndLoaderOperations(False)
            End With
        End Sub
        Private Sub DownloadFeedForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            e.Cancel = True
            Hide()
        End Sub
        Private Sub DownloadFeedForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            'ClearTable()
            MyRange.Dispose()
            LoadedFeedNames.Clear()
            BTT_CLEAR_DAILY.Dispose()
            DataList.Clear()
        End Sub
        Private Sub DownloadFeedForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            If Not e.Handled Then
                Dim b As Boolean = False
                If e = GoToButton Then
                    b = True
                    MyRange.GoToF()
                Else
                    Dim changePage%? = Nothing
                    Dim gotoHome As Boolean? = Nothing
                    Select Case e.KeyCode
                        Case Keys.F5 : RefillList() : b = True
                        Case Keys.F3 : changePage = -1
                        Case Keys.F4 : changePage = 1
                        Case Keys.Up, Keys.Left, Keys.PageUp : changePage = -1
                        Case Keys.Down, Keys.Right, Keys.PageDown : changePage = 1
                        Case Keys.Home : gotoHome = True
                        Case Keys.End : gotoHome = False
                        Case Keys.Escape : If Settings.FeedEscToClose Then Close()
                        Case Else : If e.Control And e.KeyCode = Keys.W Then Close()
                    End Select
                    If changePage.HasValue Then
                        b = True
                        If MyRange.TryMove(changePage.Value) Then MyRange.Move(changePage.Value)
                    ElseIf gotoHome.HasValue Then
                        b = True
                        Dim indx% = IIf(gotoHome.Value, 0, MyRange.Count - 1)
                        If MyRange.CurrentIndex <> indx Then MyRange.GoTo(indx)
                    End If
                End If
                If b Then e.Handled = True
            End If
        End Sub
#End Region
#Region "Feeds handlers"
        Private Overloads Sub AddNewFeedItem(ByVal Destination As ToolStripMenuItem, ByVal Feed As FeedSpecial, ByVal Image As Image,
                                             ByVal Handler As EventHandler, Optional ByVal Insert As Boolean = False)
            AddNewFeedItem(Destination, ToolbarTOP, Feed, Image, Handler, Insert)
        End Sub
        Friend Overloads Shared Function AddNewFeedItem(ByVal Destination As ToolStripMenuItem, ByVal Toolbar As ToolStrip,
                                                        ByVal Feed As FeedSpecial, ByVal Image As Image,
                                                        ByVal Handler As EventHandler, Optional ByVal Insert As Boolean = False) As ToolStripMenuItem
            Dim item As New ToolStripMenuItem(Feed.Name, Image) With {.Tag = Feed}
            If Not Handler Is Nothing Then AddHandler item.Click, Handler
            ControlInvokeFast(Toolbar, Destination, Sub()
                                                        If Destination.DropDownItems.Count > 0 And Insert Then
                                                            Destination.DropDownItems.Insert(0, item)
                                                        Else
                                                            Destination.DropDownItems.Add(item)
                                                        End If
                                                    End Sub, EDP.None)
            Return item
        End Function
        Private Sub Feed_FeedAdded(ByVal Source As FeedSpecialCollection, ByVal Feed As FeedSpecial)
            AddNewFeedItem(BTT_LOAD_SPEC, Feed, My.Resources.RSSPic_512, AddressOf Feed_SPEC_LOAD, True)
            AddNewFeedItem(BTT_FEED_ADD_SPEC, Feed, My.Resources.RSSPic_512, AddressOf Feed_SPEC_ADD, True)
            AddNewFeedItem(BTT_FEED_ADD_SPEC_REMOVE, Feed, My.Resources.RSSPic_512, AddressOf Feed_SPEC_ADD_REMOVE, True)
            AddNewFeedItem(BTT_FEED_REMOVE_SPEC, Feed, My.Resources.RSSPic_512, AddressOf Feed_SPEC_REMOVE, True)
            AddNewFeedItem(BTT_FEED_DELETE_SPEC, Feed, My.Resources.DeletePic_24, AddressOf Feed_SPEC_DELETE, True)
            AddNewFeedItem(BTT_FEED_CLEAR_SPEC, Feed, My.Resources.BrushToolPic_16, AddressOf Feed_SPEC_CLEAR, True)
        End Sub
        Private Overloads Sub Feed_FeedRemoved(ByVal Source As FeedSpecialCollection, ByVal Feed As FeedSpecial)
            Feed_FeedRemoved(BTT_LOAD_SPEC, Feed)
            Feed_FeedRemoved(BTT_FEED_ADD_SPEC, Feed)
            Feed_FeedRemoved(BTT_FEED_REMOVE_SPEC, Feed)
            Feed_FeedRemoved(BTT_FEED_DELETE_SPEC, Feed)
            Feed_FeedRemoved(BTT_FEED_CLEAR_SPEC, Feed)
        End Sub
        Private Overloads Sub Feed_FeedRemoved(ByVal Destination As ToolStripMenuItem, ByVal Feed As FeedSpecial)
            Feed_FeedRemoved(Destination, ToolbarTOP, Feed)
        End Sub
        Friend Overloads Shared Sub Feed_FeedRemoved(ByVal Destination As ToolStripMenuItem, ByVal Toolbar As ToolStrip, ByVal Feed As FeedSpecial)
            Try
                With Destination
                    ControlInvokeFast(Toolbar, .Self,
                                      Sub()
                                          If .DropDownItems.Count > 0 Then
                                              Dim item As Object
                                              For i% = .DropDownItems.Count - 1 To 0 Step -1
                                                  item = .DropDownItems(i)
                                                  If TypeOf item Is ToolStripMenuItem AndAlso Feed.Equals(DirectCast(item, ToolStripMenuItem).Tag) Then
                                                      With DirectCast(item, ToolStripMenuItem) : .Tag = Nothing : .Dispose() : End With
                                                  End If
                                              Next
                                          End If
                                      End Sub, EDP.None)
                End With
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "Feed removed")
            End Try
        End Sub
        Private Sub Feed_SPEC_LOAD(ByVal Source As ToolStripMenuItem, ByVal e As EventArgs)
            Dim f As FeedSpecial = Source.Tag
            If Not f Is Nothing AndAlso Not f.Disposed Then
                FeedChangeMode(FeedModes.Special, {f.Name})
                RefillSpecialFeedsData(False)
            End If
        End Sub
        Private Sub Feed_SPEC_ADD(ByVal Source As ToolStripMenuItem, ByVal e As EventArgs)
            Dim f As FeedSpecial = Source.Tag
            If Not f Is Nothing AndAlso Not f.Disposed Then f.Add(GetCheckedMedia())
        End Sub
        Private Sub Feed_SPEC_ADD_REMOVE(ByVal Source As ToolStripMenuItem, ByVal e As EventArgs)
            Dim f As FeedSpecial = Source.Tag
            If Not f Is Nothing AndAlso Not f.Disposed Then
                Dim c As IEnumerable(Of UserMediaD) = GetCheckedMedia()
                If c.ListExists Then
                    f.Add(c)
                    FeedRemoveCheckedMedia(c,,, {f.Name})
                End If
            End If
        End Sub
        Private Sub Feed_SPEC_CLEAR(ByVal Source As ToolStripMenuItem, ByVal e As EventArgs)
            Dim f As FeedSpecial = Source.Tag
            If Not f Is Nothing AndAlso Not f.Disposed Then
                If MsgBoxE({$"Are you sure you want to clear the '{f.Name}' feed?", "Clear feed"}, vbExclamation,,, {"Process", "Cancel"}) = 0 Then
                    f.Clear()
                    If FeedMode = FeedModes.Special Then RefillSpecialFeedsData()
                End If
            End If
        End Sub
        Private Sub Feed_SPEC_REMOVE(ByVal Source As ToolStripMenuItem, ByVal e As EventArgs)
            Dim f As FeedSpecial = Source.Tag
            If Not f Is Nothing AndAlso Not f.Disposed Then
                Dim m As IEnumerable(Of UserMediaD) = GetCheckedMedia()
                If m.ListExists Then
                    f.Remove(m)
                    FeedRemoveCheckedMedia(m, {f.Name}.ToList)
                End If
            End If
        End Sub
        Private Sub Feed_SPEC_DELETE(ByVal Source As ToolStripMenuItem, ByVal e As EventArgs)
            Dim f As FeedSpecial = Source.Tag
            If Not f Is Nothing AndAlso Not f.Disposed Then
                Dim name$ = f.Name
                If MsgBoxE({$"Are you sure you want to delete the '{name}' feed?", "Delete feed"}, vbExclamation,,, {"Process", "Cancel"}) = 0 AndAlso f.Delete() Then
                    Feed_FeedRemoved(Settings.Feeds, f)
                    If LoadedFeedNames.Count > 0 AndAlso LoadedFeedNames.Contains(name) Then LoadedFeedNames.Remove(name) : RefillSpecialFeedsData()
                End If
            End If
        End Sub
#End Region
#Region "Settings"
        Friend Sub UpdateSettings()
            With Settings
                Dim c% = .FeedDataRows * .FeedDataColumns
                Dim rangeChanged As Boolean = Not c = DataRows * DataColumns
                DataRows = .FeedDataRows
                DataColumns = .FeedDataColumns
                FeedEndless = .FeedEndless
                If .FeedCenterImage.Use Then
                    CenterImage = True
                    NumberOfVisibleImages = .FeedCenterImage
                Else
                    CenterImage = False
                    NumberOfVisibleImages = 1
                End If

                If .FeedBackColor.Exists Then
                    BackColor = .FeedBackColor
                Else
                    BackColor = SystemColors.Window
                End If
                If .FeedForeColor.Exists Then
                    ForeColor = .FeedForeColor
                Else
                    ForeColor = SystemColors.WindowText
                End If

                If rangeChanged Then
                    ClearTable()
                    ControlInvoke(TP_DATA, Sub()
                                               With TP_DATA
                                                   .RowStyles.Clear()
                                                   .RowCount = 0
                                                   .ColumnStyles.Clear()
                                                   .ColumnCount = 0
                                                   Dim i%
                                                   Dim p% = IIf(DataColumns = 1, 100, 50)
                                                   For i = 0 To DataColumns - 1 : .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, p)) : Next
                                                   .ColumnCount = .ColumnStyles.Count
                                                   For i = 0 To DataRows : .RowStyles.Add(New RowStyle(SizeType.Absolute, 0)) : Next
                                                   .RowCount = .RowStyles.Count
                                                   .HorizontalScroll.Visible = False
                                               End With
                                           End Sub)
                End If
                MyRange.HandlersSuspended = True
                MyRange.Limit = c
                MyRange.HandlersSuspended = False
                If Not MyDefs.Initializing Then RefillList()
            End With
        End Sub
#End Region
#Region "Refill"
        Private Overloads Sub RefillList(Optional ByVal RememberPosition As Boolean? = Nothing)
            If IsSession Then
                RefillList(FeedMode = FeedModes.Current, If(RememberPosition, True))
            Else
                RefillSpecialFeedsData()
            End If
        End Sub
        Private Overloads Sub RefillList(ByVal RefillDataList As Boolean, ByVal RememberPosition As Boolean)
            DataPopulated = False
            Dim rIndx% = -1
            If RememberPosition Then rIndx = MyRange.CurrentIndex
            If RefillDataList Then
                If Not IsSubscription Then
                    Try : Downloader.Files.RemoveAll(FileNotExist) : Catch : End Try
                End If
                DataList.Clear()
                DataList.ListAddList(Downloader.Files.Where(If(IsSubscription, FilterSubscriptions, FilterUsers)), LAP.NotContainsOnly)
            End If
            MyRange.Source = DataList
            If rIndx >= 0 Then
                If Not rIndx.ValueBetween(0, MyRange.Count - 1) Then rIndx -= 1
                If rIndx.ValueBetween(0, MyRange.Count - 1) Then MyRange.CurrentIndex = rIndx
            End If
            ControlInvokeFast(ToolbarTOP, BTT_REFRESH, Sub() BTT_REFRESH.ToolTipText = BttRefreshToolTipText)
            BTT_REFRESH.ControlDropColor(ToolbarTOP)
            If DataList.Count = 0 Then
                ClearTable()
            ElseIf Not DataPopulated Then
                MyRange_IndexChanged(MyRange, Nothing)
            End If
        End Sub
        Private Sub CleanDataList()
            If DataList.Count > 0 Then
                If IsSubscription Then
                    DataList.RemoveAll(FilterUsers)
                Else
                    DataList.RemoveAll(FilterSubscriptions)
                    DataList.RemoveAll(FileNotExist)
                End If
            End If
        End Sub
#End Region
#Region "Toolbar controls"
#Region "Feed"
#Region "Load"
        Private Sub BTT_LOAD_SESSION_CURRENT_Click(sender As Object, e As EventArgs) Handles BTT_LOAD_SESSION_CURRENT.Click
            FeedChangeMode(FeedModes.Current)
            RefillList(True, False)
        End Sub
        Private Sub BTT_LOAD_SESSION_LAST_Click(sender As Object, e As EventArgs) Handles BTT_LOAD_SESSION_LAST.Click
            SessionChooser(True,,, FeedModes.Saved)
        End Sub
        Private Sub BTT_LOAD_SESSION_CHOOSE_Click(sender As Object, e As EventArgs) Handles BTT_LOAD_SESSION_CHOOSE.Click
            SessionChooser(False,,, FeedModes.Saved)
        End Sub
        Private Sub SessionChooser(ByVal GetLast As Boolean, Optional ByVal GetFilesOnly As Boolean = False,
                                   Optional ByRef ResultFilesList As List(Of SFile) = Nothing,
                                   Optional ByVal SelectedMode As FeedModes = -1,
                                   Optional ByVal GetSessionFile As Boolean = False,
                                   Optional ByRef SessionFile As SFile = Nothing)
            Try
                LoadedSessionName = String.Empty
                Downloader.ClearSessions()
                Dim f As SFile = TDownloader.SessionsPath.CSFileP
                Dim fList As List(Of SFile) = Nothing
                Dim m As New MMessage("Saved sessions not selected", "Sessions",, vbExclamation)
                Dim x As XmlFile
                Dim lcr As New ListAddParams(LAP.NotContainsOnly + LAP.IgnoreICopier)
                Dim __getFiles As Func(Of Boolean) = Function() As Boolean
                                                         If f.Exists(SFO.Path, False) Then fList = SFile.GetFiles(f, "*.xml",, EDP.ReturnValue)
                                                         If fList.ListExists Then fList.RemoveAll(Settings.Feeds.FeedSpecialRemover)
                                                         If fList.ListExists Then
                                                             fList.Reverse()
                                                             Return True
                                                         Else
                                                             Return False
                                                         End If
                                                     End Function
                __getFiles.Invoke
                If Not GetLast And GetFilesOnly And Not fList.ListExists Then
                    MsgBoxE({"No session files found", "Get session files"}, vbExclamation)
                ElseIf Not GetLast AndAlso fList.ListExists Then
                    Using chooser As New SimpleListForm(Of SFile)(fList, Settings.Design) With {
                        .FormText = "Sessions",
                        .Icon = My.Resources.ArrowDownIcon_Blue_24,
                        .Mode = SimpleListFormModes.CheckedItems,
                        .Provider = SessionDateStringProvider
                    }
                        chooser.ClearButtons()
                        If chooser.ShowDialog = DialogResult.OK Then
                            fList = chooser.DataResult
                            If fList.ListExists Then
                                If GetFilesOnly Then
                                    ResultFilesList.AddRange(fList)
                                ElseIf GetSessionFile Then
                                    If fList.Count > 1 Then
                                        MsgBoxE({"You must select one session file", "Get session file"}, vbExclamation)
                                    Else
                                        SessionFile = fList(0)
                                    End If
                                Else
                                    DataList.Clear()
                                    If SelectedMode >= 0 Then
                                        If SelectedMode = FeedModes.Saved And fList.Count = 1 Then LoadedSessionName = fList(0).Name
                                        FeedChangeMode(SelectedMode)
                                    End If
                                    For Each f In fList
                                        x = New XmlFile(f,, False) With {.AllowSameNames = True, .XmlReadOnly = True}
                                        x.LoadData()
                                        If x.Count > 0 Then DataList.ListAddList(x, lcr)
                                        x.Dispose()
                                    Next
                                    CleanDataList()
                                    RefillList(False, False)
                                End If
                            Else
                                MsgBoxE(m)
                            End If
                        Else
                            MsgBoxE(m)
                        End If
                    End Using
                ElseIf Downloader.FilesSessionActual(False).Exists OrElse fList.ListExists Then
                    If GetLast Then
                        If fList.ListExists Then
                            f = fList(IIf(fList.Count > 1 And Downloader.FilesSessionActual(False).Exists, 1, 0))
                        Else
                            f = Downloader.FilesSessionActual(False)
                        End If
                    Else
                        f = Downloader.FilesSessionActual(False)
                    End If
                    If f.Exists Then
                        If GetSessionFile Then
                            If Not Downloader.FilesSessionActual(False) = f Then SessionFile = f
                        Else
                            If SelectedMode >= 0 Then
                                If SelectedMode = FeedModes.Saved Then LoadedSessionName = f.Name
                                FeedChangeMode(SelectedMode)
                            End If
                            DataList.Clear()
                            x = New XmlFile(f,, False) With {.AllowSameNames = True, .XmlReadOnly = True}
                            x.LoadData()
                            If x.Count > 0 Then DataList.ListAddList(x, lcr)
                            x.Dispose()
                            CleanDataList()
                            RefillList(False, False)
                        End If
                    End If
                Else
                    m.Text = "Saved sessions not found"
                    MsgBoxE(m)
                End If
                If fList.ListExists Then fList.Clear()
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"[DownloadObjects.DownloadFeedForm.SessionChooser({GetLast})]")
            End Try
        End Sub
#End Region
#Region "Move/Copy"
        Private Sub BTT_COPY_MOVE_TO_Click(sender As Object, e As EventArgs) Handles BTT_COPY_TO.Click, BTT_MOVE_TO.Click
            MoveCopyFiles(True, sender, Nothing, Nothing)
        End Sub
        Private Sub BTT_COPY_MOVE_SPEC_TO_Click(sender As Object, e As EventArgs) Handles BTT_COPY_SPEC_TO.Click, BTT_MOVE_SPEC_TO.Click
            MoveCopyFiles(True, sender, Nothing, Nothing, False)
        End Sub
        Private Function MoveCopyFiles(ByVal IsInternal As Boolean, ByVal Sender As Object, ByVal MCTOptions As FeedMoveCopyTo,
                                       ByVal FeedMediaData As FeedMedia, Optional ByVal GetChecked As Boolean = True) As Boolean
            Dim MsgTitle$ = "Copy/Move checked files"
            Try
                Dim isCopy As Boolean = Not Sender Is Nothing AndAlso (Sender Is BTT_COPY_TO OrElse Sender Is BTT_COPY_SPEC_TO)
                Dim moveOptions As FeedMoveCopyTo = Nothing
                Dim ff As SFile = Nothing, df As SFile
                Dim data As IEnumerable(Of UserMediaD) = Nothing
                Dim dd As UserMediaD
                Dim __user As UserInfo
                Dim __isSavedPosts As Boolean
                Dim data_files As IEnumerable(Of SFile) = Nothing
                Dim new_files As New List(Of SFile)
                Dim mm As UserMediaD
                Dim mm_data As API.Base.UserMedia
                Dim indx%
                Dim renameExisting As Boolean = False
                Dim downloaderFilesUpdated As Boolean = False
                Dim eFiles As IEnumerable(Of SFile)
                Dim finder As Predicate(Of UserMediaD) = Function(media) media.Data.File = ff
                Dim x As XmlFile
                Dim sessionData As New List(Of UserMediaD)
                Dim sesFile As SFile
                Dim sesFilesReplaced As Boolean = False
                Dim filesReplace As New List(Of KeyValuePair(Of SFile, SFile))
                Dim updateFileLocations As Boolean = Settings.FeedMoveCopyUpdateFileLocationOnMove
                Dim postUrl$
                Dim result As Boolean = False

                If FeedMediaData Is Nothing Then
                    If GetChecked Then
                        data = GetCheckedMedia()
                    ElseIf DataList.Count > 0 Then
                        data = DataList.Where(Function(__dd) Not If(__dd.User?.IsSubscription, __dd.UserInfo.IsSubscription) AndAlso __dd.Data.File.Exists)
                    End If

                    With data
                        If .ListExists Then data_files = .Select(Function(m) m.Data.File)
                    End With
                Else
                    data = {FeedMediaData.Media}
                    data_files = {FeedMediaData.File}
                End If

                MsgTitle = $"{IIf(isCopy, "Copy", "Move")} {IIf(Not FeedMediaData Is Nothing Or GetChecked, "checked", "ALL")} files"

                If data.ListExists Then

                    If (FeedMediaData Is Nothing And Not GetChecked And Not isCopy) AndAlso
                       MsgBoxE({$"YOU ARE TRYING TO MOVE ALL FEED/SESSION DATA.{vbCr}EVERY FILE WILL BE MOVED, NOT JUST THE SELECTED ONES.", MsgTitle},
                               vbExclamation,,, {"Process", "Cancel"}) = 1 Then
                        ShowOperationCanceledMsg(MsgTitle)
                        Return False
                    End If

                    If MCTOptions.Destination.IsEmptyString Then
                        Using f As New FeedCopyToForm(data_files, isCopy)
                            f.ShowDialog()
                            If f.DialogResult = DialogResult.OK Then moveOptions = f.Result
                        End Using
                    Else
                        moveOptions = MCTOptions
                    End If

                    With moveOptions
                        If Not .Destination.IsEmptyString And .ReplaceUserProfile And .ReplaceUserProfile_CreateIfNull And .ReplaceUserProfile_Profile Is Nothing Then
                            Dim existingPathInstances As IEnumerable(Of String) = Nothing
                            Dim __host As Plugin.Hosts.SettingsHost = Settings(API.PathPlugin.PluginKey).Default
                            Dim __userName$ = .Destination.Segments.LastOrDefault
                            If Settings.UsersList.Count > 0 Then _
                               existingPathInstances = (From __uu As UserInfo In Settings.UsersList
                                                        Where __uu.Plugin = API.PathPlugin.PluginKey
                                                        Select __uu.Name.ToLower)
                            Do
                                __userName = InputBoxE("Enter a new username for the 'path' plugin:", MsgTitle, __userName)
                                If __userName.IsEmptyString Then
                                    Return False
                                Else
                                    If Not existingPathInstances.ListExists OrElse Not existingPathInstances.Contains(__userName.ToLower) Then
                                        Exit Do
                                    ElseIf MsgBoxE({$"The name you entered ({__userName}) already exists", MsgTitle}, vbCritical,,, {"Retry", "Cancel"}) = 1 Then
                                        Return False
                                    End If
                                End If
                            Loop
                            __user = New UserInfo(__userName, __host) With {.SpecialPath = moveOptions.Destination.CSFilePS}
                            __user.UpdateUserFile()
                            If Settings.UsersList.Count = 0 OrElse Not Settings.UsersList.Contains(__user) Then
                                Settings.UpdateUsersList(__user)
                                With Settings.Users
                                    Dim startIndx% = .Count
                                    .Add(API.Base.UserDataBase.GetInstance(__user))
                                    With .Last
                                        If Not .FileExists Then .UpdateUserInformation()
                                    End With
                                    RaiseEvent UsersAdded(startIndx)
                                    moveOptions.ReplaceUserProfile_Profile = .Last
                                End With

                            Else
                                MsgBoxE({$"The user list already contains the user you want to add.{vbCr}Operation canceled.", MsgTitle}, vbCritical)
                                Return False
                            End If
                        End If
                    End With

                    If Not moveOptions.Destination.IsEmptyString Then
                        If Not isCopy Then
                            Dim eFileResult As Func(Of UserMediaD, SFile) = Function(ByVal fff As UserMediaD) As SFile
                                                                                Dim _fff As SFile = fff.Data.File
                                                                                _fff.Path = moveOptions.DestinationTrue(fff).Path
                                                                                Return _fff
                                                                            End Function
                            eFiles = (From ef As UserMediaD In data Where eFileResult.Invoke(ef).Exists Select eFileResult.Invoke(ef))
                            If eFiles.ListExists Then _
                               renameExisting = MsgBoxE(New MMessage("The following files already exist at the destination. " &
                                                                     "Do you still want to move them? These files will be renamed and moved." & vbCr &
                                                                     $"Destination: {moveOptions.Destination.PathWithSeparator}{vbCr}{vbCr}" &
                                                                     eFiles.ListToString(vbCr), MsgTitle, {"Move", "Cancel"}, vbExclamation) With {.Editable = True}) = 0

                        End If
                        For Each dd In data
                            If Not dd.Data.File.IsEmptyString Then
                                ff = dd.Data.File
                                df = ff
                                df.Path = moveOptions.DestinationTrue(dd).Path
                                If isCopy Then
                                    If ff.Copy(df) Then new_files.Add(df) : result = True
                                Else
                                    If df.Exists And renameExisting Then df = SFile.IndexReindex(df,,,, New ErrorsDescriber(False, False, False, df))
                                    If SFile.Move(ff, df) Then
                                        new_files.Add(df)
                                        result = True
                                        If updateFileLocations Then
                                            filesReplace.Add(New KeyValuePair(Of SFile, SFile)(ff, df))
                                            indx = Downloader.Files.FindIndex(finder)
                                            If indx >= 0 Then
                                                mm = Downloader.Files(indx)
                                                __user = mm.UserInfo
                                                mm_data = mm.Data
                                                mm_data.File = df
                                                __isSavedPosts = mm.IsSavedPosts And moveOptions.ReplaceUserProfile_Profile Is Nothing
                                                postUrl = mm.PostUrl(True)
                                                mm = New UserMediaD(mm_data, If(moveOptions.ReplaceUserProfile_Profile, mm.User), mm.Session, mm.Date) With {
                                                    .IsSavedPosts = __isSavedPosts,
                                                    .PostUrl = postUrl
                                                }
                                                If __isSavedPosts Then mm.UserInfo = __user
                                                Downloader.Files(indx) = mm
                                                downloaderFilesUpdated = True
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        Next
                        If Not isCopy And updateFileLocations Then
                            If downloaderFilesUpdated Then Downloader.FilesSave()
                            If FeedMode = FeedModes.Saved And Not LoadedSessionName.IsEmptyString And filesReplace.Count > 0 Then
                                sesFile = $"{TDownloader.SessionsPath.CSFilePS}{LoadedSessionName}.xml"
                                If sesFile.Exists Then
                                    sessionData.Clear()
                                    x = New XmlFile(sesFile, Protector.Modes.All, False) With {.AllowSameNames = True}
                                    x.LoadData()
                                    If x.Count > 0 Then sessionData.ListAddList(x, LAP.IgnoreICopier)
                                    x.Dispose()
                                    If sessionData.Count > 0 Then
                                        For Each rfile As KeyValuePair(Of SFile, SFile) In filesReplace
                                            ff = rfile.Key
                                            df = rfile.Value
                                            indx = sessionData.FindIndex(finder)
                                            If indx >= 0 Then
                                                mm = sessionData(indx)
                                                __user = mm.UserInfo
                                                mm_data = mm.Data
                                                mm_data.File = df
                                                __isSavedPosts = mm.IsSavedPosts And moveOptions.ReplaceUserProfile_Profile Is Nothing
                                                postUrl = mm.PostUrl(True)
                                                mm = New UserMediaD(mm_data, If(moveOptions.ReplaceUserProfile_Profile, mm.User), mm.Session, mm.Date) With {
                                                    .IsSavedPosts = __isSavedPosts,
                                                    .PostUrl = postUrl
                                                }
                                                If __isSavedPosts Then mm.UserInfo = __user
                                                sessionData(indx) = mm
                                                sesFilesReplaced = True
                                                If DataList.Count > 0 Then
                                                    indx = DataList.FindIndex(finder)
                                                    If indx >= 0 Then DataList(indx) = mm
                                                End If
                                            End If
                                        Next
                                        If sesFilesReplaced Then
                                            x = New XmlFile With {.AllowSameNames = True}
                                            x.AddRange(sessionData)
                                            x.Name = TDownloader.Name_SessionXML
                                            x.Save(sesFile, EDP.SendToLog)
                                            x.Dispose()
                                        End If
                                        sessionData.Clear()
                                    End If
                                End If
                            End If
                            If filesReplace.Count > 0 Then filesReplace.ForEach(Sub(fr) Settings.Feeds.UpdateDataByFile(fr.Key, fr.Value, moveOptions))
                            filesReplace.Clear()
                        End If
                        If IsInternal Then MsgBoxE(New MMessage($"The following files were {IIf(isCopy, "copied", "moved")} to{vbCr}{moveOptions.Destination}{vbCr}{vbCr}" &
                                                                $"Source:{vbCr}{data_files.ListToString(vbCr)}{vbCr}" &
                                                                $"Destination:{vbCr}{new_files.ListToString(vbCr)}", MsgTitle) With {.Editable = True})
                        new_files.Clear()
                        If Not isCopy And updateFileLocations Then RefillList()
                    End If
                Else
                    MsgBoxE({"No files selected", MsgTitle}, vbExclamation)
                End If
                Return result
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.LogMessageValue, ex, MsgTitle, False)
            Finally
                Settings.Feeds.UpdateWhereDataReplaced()
            End Try
        End Function
#End Region
#Region "Load fav, spec"
        Private Sub BTT_LOAD_FAV_Click(sender As Object, e As EventArgs) Handles BTT_LOAD_FAV.Click
            FeedChangeMode(FeedModes.Special, {FeedSpecial.FavoriteName})
            RefillSpecialFeedsData(False)
        End Sub
        Private Sub BTT_LOAD_SPEC_Click(sender As Object, e As EventArgs) Handles BTT_LOAD_SPEC.Click
            With FeedSpecialCollection.ChooseFeeds(False)
                If .ListExists Then
                    FeedChangeMode(FeedModes.Special, .Select(Function(f) f.Name))
                    RefillSpecialFeedsData(False)
                End If
            End With
        End Sub
        Private Sub RefillSpecialFeedsData(Optional ByVal RememberPosition As Boolean = True)
            If LoadedFeedNames.Count > 0 Then
                Dim d As New List(Of UserMediaD)
                Dim lp As New ListAddParams(LAP.NotContainsOnly)
                LoadedFeedNames.ForEach(Sub(ByVal fName As String)
                                            Dim indx% = Settings.Feeds.IndexOf(fName)
                                            If indx >= 0 Then
                                                With Settings.Feeds(indx)
                                                    .RemoveNotExist(FileNotExist)
                                                    d.ListAddList(.Self, lp)
                                                End With
                                            End If
                                        End Sub)
                DataList.Clear()
                If d.Count > 0 Then
                    d.Sort(New FeedSpecial.SEComparer)
                    DataList.AddRange(d)
                    CleanDataList()
                    d.Clear()
                End If
                RefillList(False, RememberPosition)
            End If
        End Sub
#End Region
#Region "Add remove fav spec"
        Private Sub BTT_FEED_ADD_FAV_Click(sender As Object, e As EventArgs) Handles BTT_FEED_ADD_FAV.Click, BTT_FEED_ADD_FAV_REMOVE.Click
            Dim m As IEnumerable(Of UserMediaD) = GetCheckedMedia()
            If m.ListExists Then
                Settings.Feeds.Favorite.Add(m)
                If sender Is BTT_FEED_ADD_FAV_REMOVE Then FeedRemoveCheckedMedia(m,,, {FeedSpecial.FavoriteName})
            End If
        End Sub
        Private Sub BTT_FEED_REMOVE_FAV_Click(sender As Object, e As EventArgs) Handles BTT_FEED_REMOVE_FAV.Click
            Dim m As IEnumerable(Of UserMediaD) = GetCheckedMedia()
            If m.ListExists Then
                Settings.Feeds.Favorite.Remove(m)
                If FeedMode = FeedModes.Special Then FeedRemoveCheckedMedia(m, {FeedSpecial.FavoriteName}.ToList)
            End If
        End Sub
        Private Sub BTT_FEED_ADD_SPEC_Click(sender As Object, e As EventArgs) Handles BTT_FEED_ADD_SPEC.Click, BTT_FEED_ADD_SPEC_REMOVE.Click
            Dim c As IEnumerable(Of UserMediaD) = GetCheckedMedia()
            If c.ListExists Then
                Dim names As New List(Of String)
                With FeedSpecialCollection.ChooseFeeds(True)
                    If .ListExists Then .ForEach(Sub(ByVal f As FeedSpecial)
                                                     names.Add(f.Name)
                                                     f.Add(c)
                                                 End Sub)
                End With
                If sender Is BTT_FEED_ADD_SPEC_REMOVE Then FeedRemoveCheckedMedia(c,,, names)
                names.Clear()
            Else
                MsgBoxE({"You haven't selected media to add to your feed(s)", "Add to feed(s)"}, vbExclamation)
            End If
        End Sub
        Private Sub BTT_FEED_REMOVE_SPEC_Click(sender As Object, e As EventArgs) Handles BTT_FEED_REMOVE_SPEC.Click
            Dim c As IEnumerable(Of UserMediaD) = GetCheckedMedia()
            If c.ListExists Then
                Dim names As New List(Of String)
                With FeedSpecialCollection.ChooseFeeds(False)
                    If .ListExists Then .ForEach(Sub(ByVal f As FeedSpecial)
                                                     names.Add(f.Name)
                                                     f.Remove(c)
                                                 End Sub)
                End With
                If FeedMode = FeedModes.Special Then FeedRemoveCheckedMedia(c, names)
            Else
                MsgBoxE({"You haven't selected media to remove from your feed(s)", "Remove from feed(s)"}, vbExclamation)
            End If
        End Sub
        Private Function GetCheckedMedia() As IEnumerable(Of UserMediaD)
            Return ControlInvoke(Of IEnumerable(Of UserMediaD))(TP_DATA,
                   Function() As IEnumerable(Of UserMediaD)
                       If TP_DATA.Controls.Count > 0 Then
                           Return (From cnt As FeedMedia In TP_DATA.Controls Where cnt.Checked Select cnt.Media)
                       Else
                           Return New UserMediaD() {}
                       End If
                   End Function, New UserMediaD() {}, EDP.ReturnValue)
        End Function
#End Region
#Region "Clear delete"
        Private Sub BTT_FEED_CLEAR_FAV_Click(sender As Object, e As EventArgs) Handles BTT_FEED_CLEAR_FAV.Click
            If MsgBoxE({"Are you sure you want to clear your Favorite feed?", "Clear feed"}, vbExclamation,,, {"Process", "Cancel"}) = 0 Then _
               Settings.Feeds.Favorite.Delete()
        End Sub
        Private Sub BTT_FEED_CLEAR_SPEC_Click(sender As Object, e As EventArgs) Handles BTT_FEED_CLEAR_SPEC.Click
            With FeedSpecialCollection.ChooseFeeds(False)
                If .ListExists Then
                    If MsgBoxE({$"Are you sure you want to clear the following feeds?{vbCr}{vbCr}{ .ListToString(vbCr)}", "Clear feed"}, vbExclamation,,, {"Process", "Cancel"}) = 0 Then
                        Dim names As IEnumerable(Of String) = .Select(Function(f) f.Name)
                        .ForEach(Sub(f) f.Clear())
                        If FeedMode = FeedModes.Special Then
                            LoadedFeedNames.ListDisposeRemove(names)
                            RefillSpecialFeedsData()
                        End If
                    End If
                End If
            End With
        End Sub
        Private Sub BTT_FEED_DELETE_SPEC_Click(sender As Object, e As EventArgs) Handles BTT_FEED_DELETE_SPEC.Click
            With FeedSpecialCollection.ChooseFeeds(False)
                If .ListExists Then
                    If MsgBoxE({$"Are you sure you want to delete the following feeds?{vbCr}{vbCr}{ .ListToString(vbCr)}", "Delete feed"}, vbExclamation,,, {"Process", "Cancel"}) = 0 Then
                        Dim names As IEnumerable(Of String) = .Select(Function(f) f.Name)
                        .ForEach(Sub(f) f.Delete())
                        If FeedMode = FeedModes.Special Then
                            LoadedFeedNames.ListDisposeRemove(names)
                            RefillSpecialFeedsData()
                        End If
                    End If
                End If
            End With
        End Sub
        Private Sub BTT_FEED_DELETE_DAILY_LIST_Click(sender As Object, e As EventArgs) Handles BTT_FEED_DELETE_DAILY_LIST.Click
            Try
                Dim l As New List(Of SFile)
                SessionChooser(False, True, l)
                If l.Count > 0 Then
                    If MsgBoxE({$"Are you sure you want to delete the following sessions?{vbCr}{vbCr}{l.ListToStringE(vbCr, SessionDateStringProvider)}", "Delete session"}, vbExclamation,,,
                               {"Process", "Cancel"}) = 0 Then
                        l.ForEach(Sub(f) f.Delete(SFO.File, SFODelete.DeleteToRecycleBin, EDP.None))
                        MsgBoxE({"Sessions have been deleted", "Delete session"})
                    End If
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "Delete daily sessions (by list)")
            End Try
        End Sub
        Private Sub BTT_FEED_DELETE_DAILY_DATE_Click(sender As Object, e As EventArgs) Handles BTT_FEED_DELETE_DAILY_DATE.Click
            Try
                Using f As New DateTimeSelectionForm(DTSModes.CheckBoxes + DTSModes.Date + DTSModes.Start + DTSModes.End, Settings.Design) With {
                    .DesignXMLNodeName = "SessionRemoverDateTimeForm"
                }
                    f.ShowDialog()
                    If f.DialogResult = DialogResult.OK Then
                        Dim _dStart As Date? = f.MyDateStart
                        Dim _dEnd As Date? = f.MyDateEnd
                        If _dStart.HasValue Or _dEnd.HasValue Then
                            Dim dStart As Date = If(_dStart, Date.MinValue)
                            dStart = dStart.Date
                            Dim dEnd As Date = If(_dEnd, Date.MaxValue)
                            dEnd = dEnd.Date
                            Dim filesDir As SFile = TDownloader.SessionsPath.CSFileP
                            If filesDir.Exists(SFO.Path, False) Then
                                Dim files As List(Of SFile) = SFile.GetFiles(filesDir, "*.xml",, EDP.ReturnValue)
                                If files.ListExists Then files.RemoveAll(Settings.Feeds.FeedSpecialRemover)
                                If files.ListExists Then
                                    Dim fd As IEnumerable(Of KeyValuePair(Of SFile, Date?)) =
                                        files.Select(Function(ff) New KeyValuePair(Of SFile, Date?)(
                                                                  ff, AConvert(Of Date)(ff.Name, SessionDateTimeProvider, AModes.Var, Nothing)))
                                    If fd.ListExists Then
                                        For Each optFile As KeyValuePair(Of SFile, Date?) In fd
                                            If optFile.Value.HasValue AndAlso optFile.Value.Value.ValueBetween(dStart, dEnd) Then _
                                               optFile.Key.Delete(SFO.File, SFODelete.DeleteToRecycleBin, EDP.None)
                                        Next
                                    End If
                                End If
                            End If
                        End If
                    End If
                End Using
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "Delete daily sessions (by date)")
            End Try
        End Sub
#End Region
#Region "Sessions set, merge, clear"
        Private Sub BTT_CURR_SESSION_SET_Click(sender As Object, e As EventArgs) Handles BTT_CURR_SESSION_SET.Click, BTT_CURR_SESSION_SET_LAST.Click
            Try
                Dim f As SFile = Nothing
                SessionChooser(sender Is BTT_CURR_SESSION_SET_LAST,,,, True, f)
                If Not f.IsEmptyString AndAlso f.Exists Then
                    Downloader.FilesLoadLastSession(f)
                    FeedChangeMode(FeedModes.Current)
                    RefillList(True, False)
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Replace current session")
            End Try
        End Sub
        Private Sub BTT_MERGE_SESSIONS_Click(sender As Object, e As EventArgs) Handles BTT_MERGE_SESSIONS.Click
            Try
                Const msgTitle$ = "Merge feeds"
                Dim files As New List(Of SFile)
                Dim abs% = 0, prev% = 0, curr%, i%
                Dim x As XmlFile
                Dim f As SFile
                Dim um As UserMediaD
                Dim data As New List(Of UserMediaD)
                Dim tmpData As New List(Of UserMediaD)
                Dim lrc As New ListAddParams(LAP.NotContainsOnly + LAP.IgnoreICopier)
                SessionChooser(False, True, files)
                If files.ListExists(2) Then
                    files.Sort()
                    For Each f In files
                        x = New XmlFile(f,, False) With {.AllowSameNames = True, .XmlReadOnly = True}
                        x.LoadData(EDP.None)
                        If x.Count > 0 Then tmpData.ListAddList(x, lrc)
                        If tmpData.Count > 0 Then tmpData.Reverse() : data.AddRange(tmpData) : tmpData.Clear()
                        x.Dispose()
                    Next
                    If data.Count > 0 Then
                        For i = 0 To data.Count - 1
                            um = data(i)
                            curr = um.Session
                            If i = 0 Then
                                abs = curr
                            Else
                                If curr < abs And prev <> curr Then
                                    abs += 1
                                ElseIf curr >= abs Then
                                    abs = curr
                                End If
                            End If
                            prev = curr
                            um.Session = abs
                            data(i) = um
                        Next
                        data.Reverse()
                        x = New XmlFile With {.Name = TDownloader.Name_SessionXML, .AllowSameNames = True}
                        x.AddRange(data)
                        x.Save(files(0))
                        x.Dispose()
                        For i = 1 To files.Count - 1 : files(i).Delete(SFO.File, SFODelete.DeleteToRecycleBin, EDP.ReturnValue) : Next
                        MsgBoxE({$"Session data was combined into '{files(0).Name}'.{vbCr}{vbCr}" &
                                 files.ListToStringE(vbCr, New CustomProvider(Function(ff As SFile) ff.Name),,, EDP.ReturnValue), msgTitle})
                        files.Clear()
                        data.Clear()
                    Else
                        MsgBoxE({"There is no session data in the selected files", msgTitle}, vbExclamation)
                    End If
                ElseIf files.ListExists(1) Then
                    MsgBoxE({"You must select two or more files to merge feeds", msgTitle}, vbExclamation)
                Else
                    MsgBoxE({"You haven't selected any feeds", msgTitle}, vbExclamation)
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadFeedForm.MergeSessions]")
            End Try
        End Sub
        Private Sub BTT_CLEAR_DAILY_Click(sender As Object, e As EventArgs) Handles BTT_CLEAR_DAILY.Click
            If MsgBoxE({"Are you sure you want to clear this session data?", "Clear session"}, vbExclamation,,, {"Process", "Cancel"}) = 0 Then
                Downloader.Files.Clear()
                ClearTable()
                RefillList()
            End If
        End Sub
        Private Sub BTT_RESET_DAILY_Click(sender As Object, e As EventArgs) Handles BTT_RESET_DAILY.Click
            If MsgBoxE({"Are you sure you want to reset the current session?" & vbCr &
                        "A new file will be created for the current session", "Reset current session"}, vbExclamation,,, {"Process", "Cancel"}) = 0 Then
                Downloader.ResetSession()
                FeedChangeMode(FeedModes.Current)
                RefillList(True, False)
            End If
        End Sub
#End Region
#Region "Merge feeds"
        Private Sub BTT_MERGE_FEEDS_Click(sender As Object, e As EventArgs) Handles BTT_MERGE_FEEDS.Click
            Try
                Const msgTitle$ = "Merge feeds"
                If Settings.Feeds.Count > 1 Then
                    Dim mFrom As List(Of FeedSpecial) = FeedSpecialCollection.ChooseFeeds(False, "[SOURCE]", True)
                    Dim mTo As FeedSpecial
                    If mFrom.ListExists(2) Then
                        mTo = FeedSpecialCollection.ChooseFeeds(True, "[DESTINATION]", True).FirstOrDefault
                        If Not mTo Is Nothing Then
                            Dim names$() = mFrom.Select(Function(f) f.Name).ToArray
                            If MsgBoxE({$"Are you sure you want to merge the following feeds into '{mTo.Name}'?{vbCr}{vbCr}" &
                                        names.ListToString(vbCr), msgTitle}, vbQuestion + vbYesNo) = vbYes Then
                                mFrom.ForEach(Sub(f) mTo.Add(f, False))
                                mTo.Save()
                                mFrom.ForEach(Sub(f) If Not f.Equals(mTo) Then Settings.Feeds.Delete(f))
                                MsgBoxE({$"Feeds' data was combined into '{mTo.Name}'.{vbCr}It is highly recommended to restart SCrawler!{vbCr}{vbCr}" &
                                         names.ListToStringE(vbCr), msgTitle})
                            Else
                                ShowOperationCanceledMsg(msgTitle)
                            End If
                            mFrom.Clear()
                        Else
                            MsgBoxE({"No destination selected", msgTitle}, vbExclamation)
                        End If
                    ElseIf mFrom.ListExists(1) Then
                        MsgBoxE({"You must select two or more files to merge feeds", msgTitle}, vbExclamation)
                    Else
                        MsgBoxE({"You haven't selected any feeds", msgTitle}, vbExclamation)
                    End If
                ElseIf Settings.Feeds.Count = 1 Then
                    MsgBoxE({"You must have two or more files to merge feeds", msgTitle}, vbExclamation)
                Else
                    MsgBoxE({"No feeds found", msgTitle}, vbExclamation)
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadFeedForm.MergeFeeds]")
            End Try
        End Sub
#End Region
        Private Sub BTT_CHECK_ALL_NONE_Click(sender As Object, e As EventArgs) Handles BTT_CHECK_ALL.Click, BTT_CHECK_NONE.Click, BTT_CHECK_INVERT.Click
            Try
                Dim checked As Boolean = sender Is BTT_CHECK_ALL
                Dim isInvert As Boolean = sender Is BTT_CHECK_INVERT
                ControlInvokeFast(TP_DATA, Sub()
                                               With TP_DATA
                                                   If .Controls.Count > 0 Then
                                                       For Each cnt As FeedMedia In .Controls : cnt.Checked = If(isInvert, Not cnt.Checked, checked) : Next
                                                   End If
                                               End With
                                           End Sub, EDP.None)
            Catch
            End Try
        End Sub
#End Region
#Region "View modes"
        Private Sub OPT_Click(ByVal Sender As ToolStripMenuItem, ByVal e As EventArgs) Handles OPT_DEFAULT.Click, OPT_SUBSCRIPTIONS.Click
            Dim __refill As Boolean = False
            ControlInvokeFast(ToolbarTOP, Sender,
                              Sub()
                                  Dim clicked As ToolStripMenuItem = Sender
                                  Dim other As ToolStripMenuItem = If(Sender Is OPT_DEFAULT, OPT_SUBSCRIPTIONS, OPT_DEFAULT)
                                  If other.Checked Then
                                      __refill = True
                                      clicked.Checked = True
                                      other.Checked = False
                                  Else
                                      clicked.Checked = False
                                  End If
                                  Settings.FeedLastModeSubscriptions.Value = OPT_SUBSCRIPTIONS.Checked
                                  MENU_DOWN.Visible = OPT_SUBSCRIPTIONS.Checked
                              End Sub, EDP.None)
            If __refill Then RefillList()
        End Sub
#End Region
#Region "View changer"
        Private Sub BTT_VIEW_SAVE_Click(sender As Object, e As EventArgs) Handles BTT_VIEW_SAVE.Click
            Dim fName$ = String.Empty
            Dim __process As Boolean = False
            If Settings.FeedViews Is Nothing Then Settings.FeedViews = New FeedViewCollection
            Do
                fName = InputBoxE("Enter a new name for the view:", "Feed view name", fName)
                If Not fName.IsEmptyString Then
                    If Settings.FeedViews.IndexOf(fName) >= 0 Then
                        Select Case MsgBoxE({$"The '{fName}' feed view already exists!", "Save view"}, vbExclamation,,, {"Try again", "Replace", "Cancel"}).Index
                            Case 1 : __process = True
                            Case 2 : Exit Sub
                        End Select
                    Else
                        __process = True
                    End If
                Else
                    Exit Sub
                End If
            Loop While Not __process
            If __process Then
                Settings.FeedViews.Add(FeedView.FromCurrent(fName))
                MsgBoxE({$"The '{fName}' feed view has been saved", "Save view"})
            End If
        End Sub
        Private Sub BTT_VIEW_LOAD_Click(sender As Object, e As EventArgs) Handles BTT_VIEW_LOAD.Click
            Try
                If Settings.FeedViews Is Nothing Then Settings.FeedViews = New FeedViewCollection
                If Settings.FeedViews.Count = 0 Then
                    MsgBoxE({"There are no saved feed views", "Load feed view"}, vbExclamation)
                Else
                    Using f As New SimpleListForm(Of FeedView)(Settings.FeedViews, Settings.Design) With {
                        .DesignXMLNodeName = "SavedFeedViewsForm",
                        .FormText = "Feed view",
                        .Mode = SimpleListFormModes.SelectedItems,
                        .MultiSelect = False
                    }
                        If f.ShowDialog = DialogResult.OK Then
                            Dim v As FeedView = f.DataResult.FirstOrDefault
                            If Not v.Name.IsEmptyString Then
                                ControlInvokeFast(Me, Sub() WindowState = FormWindowState.Normal)
                                v.Populate()
                                ControlInvokeFast(Me, Sub() MyDefs.MyView.SetFormSize())
                                UpdateSettings()
                            End If
                        End If
                    End Using
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Load feed view")
            End Try
        End Sub
#End Region
        Friend Sub Downloader_FilesChanged(ByVal Added As Boolean)
            ControlInvokeFast(ToolbarTOP, BTT_REFRESH, Sub() BTT_REFRESH.ToolTipText = If(Added, "New files found", "Some files have been removed"))
            BTT_REFRESH.ControlChangeColor(ToolbarTOP, Added, False)
        End Sub
        Private Sub BTT_REFRESH_Click(sender As Object, e As EventArgs) Handles BTT_REFRESH.Click
            RefillList()
        End Sub
#End Region
#Region "FeedMedia handlers"
        Private Sub FeedMedia_MediaMove(ByVal Sender As FeedMedia, ByVal MCTOptions As FeedMoveCopyTo, ByRef Result As Boolean)
            Result = MoveCopyFiles(False, Nothing, MCTOptions, Sender)
        End Sub
        Private Sub FeedMedia_MediaDeleted(ByVal Sender As FeedMedia)
            Try
                ControlInvoke(TP_DATA, Sub() TPRemoveControl(Sender, True))
                DataList.RemoveAll(Function(dd) dd.Data.File = Sender.File)
                RefillAfterDelete()
            Catch
            End Try
        End Sub
        Private Sub FeedMedia_Download(ByVal Sender As Object, ByVal e As EventArgs) Handles BTT_DOWN_ALL.Click, BTT_DOWN_SELECTED.Click
            Try
                Dim urls As New List(Of String)
                If TypeOf Sender Is FeedMedia Then
                    urls.Add(DirectCast(Sender, FeedMedia).Post.URL_BASE)
                ElseIf TypeOf Sender Is ToolStripMenuItem Then
                    Dim all As Boolean = CStr(AConvert(Of String)(DirectCast(Sender, ToolStripMenuItem).Tag, String.Empty)).StringToLower = "a"
                    ControlInvokeFast(TP_DATA, Sub()
                                                   urls.ListAddList((From m As FeedMedia In TP_DATA.Controls
                                                                     Where m.Checked Or all
                                                                     Select m.Post.URL_BASE).ListIfNothing)
                                                   TP_DATA.Controls.Cast(Of FeedMedia).ToList.ForEach(Sub(cnt) cnt.Checked = False)
                                               End Sub)
                End If
                If urls.Count > 0 Then
                    VideoDownloader.FormShow
                    VideoDownloader.ADD_URLS_EXTERNAL(urls)
                    urls.Clear()
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Download subscription media")
            End Try
        End Sub
        Private Sub FeedMedia_FeedAddWithRemove(ByVal Sender As FeedMedia, ByVal Feeds As IEnumerable(Of String), ByVal Media As UserMediaD, ByVal RemoveOperation As Boolean)
            FeedRemoveCheckedMedia({Media},, False, Feeds, RemoveOperation)
        End Sub
#End Region
#Region "Delete / Remove"
        Private Sub BTT_DELETE_SELECTED_Click(sender As Object, e As EventArgs) Handles BTT_DELETE_SELECTED.Click
            Const MsgTitle$ = "Deleting marked files"
            Try
                Dim c As IEnumerable(Of FeedMedia) = GetCheckedMediaControls()
                If c.ListExists Then
                    If MsgBoxE({$"Are you sure you want to delete {c.Count} file(s)?", MsgTitle}, vbExclamation,,, {"Process", "Cancel"}) = 0 Then
                        Dim indx% = MyRange.CurrentIndex
                        Dim d% = RemoveCheckedMedia(True, c)
                        If d > 0 Then RefillAfterDelete()
                        MsgBoxE({$"{d}/{c.Count} file(s) deleted", MsgTitle})
                    Else
                        MsgBoxE({"Operation canceled", MsgTitle})
                    End If
                Else
                    MsgBoxE({"No files selected", MsgTitle}, vbExclamation)
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, MsgTitle)
            End Try
        End Sub
        Private Function GetCheckedMediaControls() As IEnumerable(Of FeedMedia)
            Return ControlInvoke(TP_DATA, Function() If(TP_DATA.Controls.Count > 0, TP_DATA.Controls.ToObjectsList.Cast(Of FeedMedia)().Where(Function(f) f.Checked), New FeedMedia() {}))
        End Function
        Private Function RemoveCheckedMedia(ByVal DeleteFiles As Boolean, Optional ByVal Controls As IEnumerable(Of FeedMedia) = Nothing) As Integer
            Try
                Dim d% = 0
                If Not Controls.ListExists Then Controls = GetCheckedMediaControls()
                If Controls.ListExists Then
                    ControlInvoke(TP_DATA, Sub()
                                               With TP_DATA
                                                   .SuspendLayout()
                                                   LatestScrollValueDisabled = True
                                                   For Each fm As FeedMedia In Controls
                                                       If Not DeleteFiles OrElse fm.DeleteFile(True) Then
                                                           d += 1
                                                           DataList.RemoveAll(Function(dd) dd.Data.File = fm.File)
                                                           TPRemoveControl(fm, False)
                                                       End If
                                                   Next
                                                   If d = 0 Then LatestScrollValueDisabled = False
                                                   .ResumeLayout(d = 0)
                                                   If d > 0 Then
                                                       .AutoScroll = False
                                                       .AutoScroll = True
                                                       SetScrollValue(False)
                                                       .PerformLayout()
                                                       LatestScrollValueDisabled = False
                                                   End If
                                               End With
                                           End Sub)
                End If
                Return d
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadFeedForm.RemoveCheckedMedia]", 0)
            End Try
        End Function
        Private Sub TPRemoveControl(ByVal CNT As FeedMedia, ByVal Suspend As Boolean)
            Dim HeightChanged As Boolean = False
            Try
                If Suspend Then TP_DATA.SuspendLayout() : LatestScrollValueDisabled = True
                Dim p As TableLayoutPanelCellPosition = TP_DATA.GetCellPosition(CNT)
                Dim DropHeight As Action = Sub()
                                               If Not LatestScrollValue.HasValue Then LatestScrollValue = TP_DATA.VerticalScroll.Value
                                               LatestScrollValue = LatestScrollValue.Value - TP_DATA.RowStyles(p.Row).Height
                                               If LatestScrollValue.Value < 0 Then LatestScrollValue = 0
                                               TP_DATA.RowStyles(p.Row).Height = 0
                                               HeightChanged = True
                                           End Sub

                TP_DATA.Controls.Remove(CNT)
                CNT.Dispose()

                If DataColumns = 1 Then
                    If p.Column >= 0 And p.Row >= 0 Then DropHeight.Invoke
                Else
                    If p.Row.ValueBetween(0, TP_DATA.RowStyles.Count - 1) And p.Column.ValueBetween(0, TP_DATA.ColumnStyles.Count - 1) Then
                        Dim found As Boolean = False
                        For i% = 0 To TP_DATA.ColumnStyles.Count - 1
                            If Not TP_DATA.GetControlFromPosition(i, p.Row) Is Nothing Then found = True : Exit For
                        Next
                        If Not found Then DropHeight.Invoke
                    End If
                End If
            Catch
            Finally
                If Suspend Then
                    TP_DATA.ResumeLayout(Not HeightChanged)
                    If HeightChanged Then
                        TP_DATA.AutoScroll = False
                        TP_DATA.AutoScroll = True
                        SetScrollValue(False)
                        TP_DATA.PerformLayout()
                        LatestScrollValueDisabled = False
                    End If
                End If
            End Try
        End Sub
        Private Sub RefillAfterDelete()
            Try
                With MyRange
                    Dim indx% = .CurrentIndex
                    Dim indxChanged As Boolean = False
                    .HandlersSuspended = True
                    .Update()
                    If .Count > 0 Then
                        If indx.ValueBetween(0, .Count - 1) Then
                            .CurrentIndex = indx
                        ElseIf (indx - 1).ValueBetween(0, .Count - 1) Then
                            .CurrentIndex = indx - 1
                            indxChanged = True
                        Else
                            .CurrentIndex = .Count - 1
                            indxChanged = Not indx = .CurrentIndex
                        End If
                        .UpdateControls()
                        .HandlersSuspended = False
                        If Not indxChanged Then LatestScrollValueDisabled = True
                        DirectCast(MyRange.Switcher, RangeSwitcher(Of UserMediaD)).PerformIndexChanged()
                        If Not indxChanged Then
                            LatestScrollValueDisabled = False
                            SetScrollValue(True)
                        End If
                    End If
                    .HandlersSuspended = False
                End With
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadFeedForm.RefillAfterDelete]")
            End Try
        End Sub
#End Region
#Region "Range"
        Private DataPopulated As Boolean = False
        Private Structure TPCELL
            Private ReadOnly RowsCount As Integer
            Private ReadOnly ColumnsCount As Integer
            Friend ReadOnly Row As Integer
            Friend ReadOnly Column As Integer
            Friend Sub New(ByVal RowsCount As Integer, ByVal ColumnsCount As Integer)
                Me.RowsCount = RowsCount
                Me.ColumnsCount = ColumnsCount
                Row = 0
                Column = 0
            End Sub
            Private Sub New(ByVal RowsCount As Integer, ByVal ColumnsCount As Integer, ByVal Row As Integer, ByVal Column As Integer)
                Me.New(RowsCount, ColumnsCount)
                Me.Row = Row
                Me.Column = Column
            End Sub
            Friend Function [Next]() As TPCELL
                Dim r% = Row
                Dim c% = Column + 1
                If Not c.ValueBetween(0, ColumnsCount - 1) Then c = 0 : r += 1
                Return New TPCELL(RowsCount, ColumnsCount, r, c)
            End Function
        End Structure
        Private RefillInProgress As Boolean = False
        Private Sub MyRange_IndexChanged(ByVal Sender As IRangeSwitcherProvider, ByVal e As EventArgs) Handles MyRange.IndexChanged
            Try
                If Not RefillInProgress AndAlso Sender.CurrentIndex >= 0 Then
                    RefillInProgress = True
                    AllowTopScroll = False
                    ScrollSuspended = True
                    Dim __isSubscriptions As Boolean = IsSubscription
                    Dim d As List(Of UserMediaD) = MyRange.Current
                    Dim d2 As List(Of UserMediaD)
                    Dim i%
                    If d.ListExists AndAlso Not IsSubscription AndAlso d.All(FileNotExist) Then
                        i = Sender.CurrentIndex
                        Sender.HandlersSuspended = True
                        RefillList(False)
                        If Sender.Count > 0 Then
                            If i.ValueBetween(0, Sender.Count - 1) Then Sender.CurrentIndex = i
                            Sender.HandlersSuspended = False
                        End If
                        RefillInProgress = False
                        Sender.HandlersSuspended = False
                        DirectCast(MyRange.Switcher, RangeSwitcher(Of UserMediaD)).PerformIndexChanged()
                        Exit Sub
                    End If
                    If d.ListExists Then
                        ClearTable()
                        If Sender.CurrentIndex > 0 And FeedEndless Then
                            d2 = DirectCast(MyRange.Switcher, RangeSwitcher(Of UserMediaD)).Item(Sender.CurrentIndex - 1).
                                 Where(Function(md) __isSubscriptions OrElse Not FileNotExist.Predicate(md)).ListTake(-2, DataColumns, EDP.ReturnValue).ListIfNothing
                            If d2.Count > 0 Then d.InsertRange(0, d2) : d2.Clear()
                        End If
                        Dim w% = GetWidth()
                        Dim h% = GetHeight()
                        Dim p As New TPCELL(DataRows, DataColumns)
                        Dim fmList As New List(Of FeedMedia)
                        d.ForEach(Sub(ByVal de As UserMediaD)
                                      fmList.Add(New FeedMedia(de, w, h, IsSession))
                                      With fmList.Last
                                          AddHandler .MediaDeleted, AddressOf FeedMedia_MediaDeleted
                                          AddHandler .MediaDownload, AddressOf FeedMedia_Download
                                          AddHandler .MediaMove, AddressOf FeedMedia_MediaMove
                                          AddHandler .FeedAddWithRemove, AddressOf FeedMedia_FeedAddWithRemove
                                      End With
                                  End Sub)
                        If fmList.Count > 0 Then fmList.ListDisposeRemoveAll(Function(fm) fm Is Nothing OrElse fm.HasError)
                        If fmList.Count > 0 Then
                            For i = 0 To fmList.Count - 1
                                ControlInvoke(TP_DATA, Sub() TP_DATA.Controls.Add(fmList(i), p.Column, p.Row))
                                p = p.Next
                            Next
                        End If
                        ResizeGrid()
                        fmList.Clear()
                        d.Clear()
                    End If
                    RefillInProgress = False
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"[DownloadObjects.DownloadFeedForm.Range.IndexChanged({Sender.CurrentIndex})]")
                RefillInProgress = False
            Finally
                If Not RefillInProgress AndAlso Sender.CurrentIndex >= 0 Then
                    ControlInvoke(TP_DATA, Sub()
                                               Dim v%
                                               Dim c As Boolean
                                               With TP_DATA.VerticalScroll
                                                   If Offset = 1 Or Not DataPopulated Then v = 0 Else v = .Maximum
                                                   c = Not .Value = v
                                                   ScrollSuspended = True
                                                   .Value = v
                                               End With
                                               If c Then TP_DATA.PerformLayout()
                                           End Sub)
                    ScrollSuspended = False
                    DataPopulated = True
                    IndexChanged = True
                End If
                ControlInvokeFast(Me, Sub()
                                          Activate()
                                          Focus()
                                      End Sub, EDP.None)
            End Try
        End Sub
#End Region
#Region "Size"
        Private LastWinState As FormWindowState = FormWindowState.Normal
        Private Function GetWidth() As Integer
            Return (TP_DATA.Width - PaddingE.GetOf({Me, TP_DATA}).Horizontal(2)) / DataColumns
        End Function
        Private Function GetHeight() As Integer
            If CenterImage And DataColumns = 1 Then
                Return (TP_DATA.Height - PaddingE.GetOf({Me, TP_DATA}).Vertical(2)) / IIf(NumberOfVisibleImages > 0, NumberOfVisibleImages, 1)
            Else
                Return -1
            End If
        End Function
        Private Sub DownloadFeedForm_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
            ResizeGrid()
            UpdateButton()
        End Sub
        Private Sub DownloadFeedForm_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
            If Not LastWinState = WindowState And Not If(MyDefs?.Initializing, True) Then LastWinState = WindowState : ResizeGrid()
        End Sub
        Private Sub ResizeGrid()
            ControlInvoke(TP_DATA, Sub()
                                       With TP_DATA
                                           If .Controls.Count > 0 Then
                                               Dim w% = GetWidth()
                                               Dim h% = GetHeight()
                                               Dim p As TableLayoutPanelCellPosition
                                               Dim rh As New Dictionary(Of Integer, List(Of Integer))
                                               For Each cnt As FeedMedia In .Controls
                                                   cnt.RerenderObject(w, h)
                                                   p = .GetCellPosition(cnt)
                                                   If Not rh.ContainsKey(p.Row) Then rh.Add(p.Row, New List(Of Integer))
                                                   rh(p.Row).Add(cnt.Height)
                                               Next
                                               For i% = 0 To .RowStyles.Count - 1 : .RowStyles(i).Height = 0 : Next
                                               If rh.Count > 0 Then
                                                   For Each kv In rh
                                                       .RowStyles(kv.Key).Height = kv.Value.Max
                                                       kv.Value.Clear()
                                                   Next
                                               End If
                                               rh.Clear()
                                               .AutoScroll = False
                                               .AutoScroll = True
                                           End If
                                       End With
                                   End Sub)
        End Sub
        Private Sub UpdateButton()
            ControlInvokeFast(ToolbarTOP, MENU_DOWN, Sub() MENU_DOWN.DisplayStyle = IIf(Width >= 540,
                                                                                        ToolStripItemDisplayStyle.ImageAndText,
                                                                                        ToolStripItemDisplayStyle.Image), EDP.None)
        End Sub
#End Region
#Region "Scroll"
        Private AllowTopScroll As Boolean = False
        Private ScrollSuspended As Boolean = False
        Private Offset As Integer = 1
        Private LatestScrollValue As Integer? = Nothing
        Private LatestScrollValueDisabled As Boolean = False
        Private IndexChanged As Boolean = False
        Private Sub DownloadFeedForm_Activated(sender As Object, e As EventArgs) Handles Me.Activated
            If LatestScrollValue.HasValue Then ControlInvoke(TP_DATA, Sub()
                                                                          Dim b As Boolean = ScrollSuspended
                                                                          If Not b Then ScrollSuspended = True
                                                                          SetScrollValue(True)
                                                                          If Not b Then ScrollSuspended = False
                                                                      End Sub)
        End Sub
        Private Sub DownloadFeedForm_Deactivate(sender As Object, e As EventArgs) Handles Me.Deactivate
            If Not LatestScrollValueDisabled Then LatestScrollValue = ControlInvokeFast(TP_DATA, Function() TP_DATA.VerticalScroll.Value, 0, EDP.ReturnValue)
        End Sub
        Private Sub TP_DATA_Paint(sender As Object, e As PaintEventArgs) Handles TP_DATA.Paint
            If Not MyDefs.Initializing And Not ScrollSuspended And FeedEndless Then
                If Not LatestScrollValueDisabled Then LatestScrollValue = ControlInvokeFast(TP_DATA, Function() TP_DATA.VerticalScroll.Value, 0)
                If IndexChanged Then IndexChanged = False : Exit Sub
                ControlInvoke(TP_DATA, Sub()
                                           With TP_DATA
                                               Offset = IIf(.VerticalScroll.Value = 0 And AllowTopScroll, -1, 1)
                                               If .VerticalScroll.Value + .VerticalScroll.LargeChange >= .DisplayRectangle.Height Or (.VerticalScroll.Value = 0 And AllowTopScroll) Then
                                                   If MyRange.TryMove(Offset) Then MyRange.Move(Offset)
                                               End If
                                               If Not AllowTopScroll And .VerticalScroll.Value > 0 Then AllowTopScroll = True
                                           End With
                                       End Sub)
            End If
        End Sub
        Private Sub TP_DATA_StyleChanged(sender As Object, e As EventArgs) Handles TP_DATA.StyleChanged
            ControlInvokeFast(TP_DATA, Sub()
                                           With TP_DATA
                                               .Padding = New Padding(0, 0, .VerticalScroll.Visible.BoolToInteger * 3, 0)
                                               .HorizontalScroll.Visible = False
                                               .HorizontalScroll.Enabled = False
                                               .PerformLayout()
                                           End With
                                       End Sub, EDP.None)
        End Sub
        Private Sub SetScrollValue(ByVal Perform As Boolean)
            With TP_DATA
                If LatestScrollValue.HasValue Then
                    If LatestScrollValue.Value < 0 Then LatestScrollValue = 0
                    If LatestScrollValue.Value > .VerticalScroll.Maximum Then LatestScrollValue = .VerticalScroll.Maximum
                    .VerticalScroll.Value = LatestScrollValue.Value
                    If Perform Then .PerformLayout()
                End If
            End With
        End Sub
#End Region
        Private Sub ClearTable()
            ControlInvoke(TP_DATA, Sub()
                                       If TP_DATA.Controls.Count > 0 Then
                                           For Each cnt As Control In TP_DATA.Controls : cnt.Dispose() : Next
                                           TP_DATA.Controls.Clear()
                                           GC.Collect()
                                           GC.WaitForPendingFinalizers()
                                           GC.WaitForFullGCComplete()
                                       End If
                                   End Sub)
        End Sub
    End Class
End Namespace