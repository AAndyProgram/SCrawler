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
#Region "Declarations"
        Private WithEvents MyDefs As DefaultFormOptions
        Private WithEvents MyRange As RangeSwitcherToolbar(Of UserMediaD)
        Private ReadOnly DataList As List(Of UserMediaD)
        Private WithEvents BTT_DELETE_SELECTED As ToolStripButton
        Private DataRows As Integer = 10
        Private DataColumns As Integer = 1
        Private FeedEndless As Boolean = False
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
#End Region
#Region "Initializer"
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            MyRange = New RangeSwitcherToolbar(Of UserMediaD)(ToolbarTOP)
            DataList = New List(Of UserMediaD)
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
                    .ButtonKey(RCI.Previous) = Keys.F3
                    .ButtonKey(RCI.Next) = Keys.F4
                    .ButtonKey(RCI.GoTo) = New ButtonKey(Keys.G, True)
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
                RefillList()
                .EndLoaderOperations(False)
            End With
        End Sub
        Private Sub DownloadFeedForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            e.Cancel = True
            Hide()
        End Sub
        Private Sub DownloadFeedForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            ClearTable()
            MyRange.Dispose()
            BTT_CLEAR.Dispose()
            DataList.Clear()
        End Sub
        Private Sub DownloadFeedForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            If e.KeyCode = Keys.F5 Then RefillList() : e.Handled = True
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
                                                      '.DropDownItems.RemoveAt(i)
                                                  End If
                                              Next
                                          End If
                                      End Sub, EDP.None)
                End With
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "Feed removed")
                MainFrameObj.UpdateLogButton()
            End Try
        End Sub
        Private Sub Feed_SPEC_LOAD(ByVal Source As ToolStripMenuItem, ByVal e As EventArgs)
            Dim f As FeedSpecial = Source.Tag
            If Not f Is Nothing AndAlso Not f.Disposed Then
                DataList.Clear()
                If f.Count > 0 Then DataList.ListAddList(f) : CleanDataList()
                RefillList(False)
            End If
        End Sub
        Private Sub Feed_SPEC_ADD(ByVal Source As ToolStripMenuItem, ByVal e As EventArgs)
            Dim f As FeedSpecial = Source.Tag
            If Not f Is Nothing AndAlso Not f.Disposed Then f.Add(GetCheckedMedia())
        End Sub
        Private Sub Feed_SPEC_CLEAR(ByVal Source As ToolStripMenuItem, ByVal e As EventArgs)
            Dim f As FeedSpecial = Source.Tag
            If Not f Is Nothing AndAlso Not f.Disposed Then
                If MsgBoxE({$"Are you sure you want to clear the '{f.Name}' feed?", "Clear feed"}, vbExclamation,,, {"Process", "Cancel"}) = 0 Then f.Clear()
            End If
        End Sub
        Private Sub Feed_SPEC_REMOVE(ByVal Source As ToolStripMenuItem, ByVal e As EventArgs)
            Dim f As FeedSpecial = Source.Tag
            If Not f Is Nothing AndAlso Not f.Disposed Then f.Remove(GetCheckedMedia())
        End Sub
        Private Sub Feed_SPEC_DELETE(ByVal Source As ToolStripMenuItem, ByVal e As EventArgs)
            Dim f As FeedSpecial = Source.Tag
            If Not f Is Nothing AndAlso Not f.Disposed Then
                If MsgBoxE({$"Are you sure you want to delete the '{f.Name}' feed?", "Delete feed"}, vbExclamation,,, {"Process", "Cancel"}) = 0 AndAlso
                   f.Delete() Then Feed_FeedRemoved(Settings.Feeds, f)
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

                Dim fsd As Boolean = .FeedStoreSessionsData
                ControlInvoke(ToolbarTOP, MENU_LOAD_SESSION, Sub()
                                                                 MENU_LOAD_SESSION.Visible = fsd
                                                                 SEP_0.Visible = fsd
                                                             End Sub)
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
                If Not MyDefs.Initializing Then RefillList(False)
            End With
        End Sub
#End Region
#Region "Refill"
        Private Sub RefillList(Optional ByVal RefillDataList As Boolean = True)
            DataPopulated = False
            If RefillDataList Then
                If Not IsSubscription Then
                    Try : Downloader.Files.RemoveAll(FileNotExist) : Catch : End Try
                End If
                DataList.Clear()
                DataList.ListAddList(Downloader.Files.Where(If(IsSubscription, FilterSubscriptions, FilterUsers)), LAP.NotContainsOnly)
            End If
            MyRange.Source = DataList
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
            RefillList()
        End Sub
        Private Sub BTT_LOAD_SESSION_LAST_Click(sender As Object, e As EventArgs) Handles BTT_LOAD_SESSION_LAST.Click
            SessionChooser(True)
        End Sub
        Private Sub BTT_LOAD_SESSION_CHOOSE_Click(sender As Object, e As EventArgs) Handles BTT_LOAD_SESSION_CHOOSE.Click
            SessionChooser(False)
        End Sub
        Private Sub SessionChooser(ByVal GetLast As Boolean, Optional ByVal GetFilesOnly As Boolean = False,
                                   Optional ByRef ResultFilesList As List(Of SFile) = Nothing)
            Try
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
                If Not GetLast Then __getFiles.Invoke
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
                                Else
                                    DataList.Clear()
                                    For Each f In fList
                                        x = New XmlFile(f,, False) With {.AllowSameNames = True, .XmlReadOnly = True}
                                        x.LoadData()
                                        If x.Count > 0 Then DataList.ListAddList(x, lcr)
                                        x.Dispose()
                                    Next
                                    CleanDataList()
                                    RefillList(False)
                                End If
                            Else
                                MsgBoxE(m)
                            End If
                        Else
                            MsgBoxE(m)
                        End If
                    End Using
                ElseIf Downloader.FilesSessionActual(False).Exists OrElse __getFiles.Invoke Then
                    If Downloader.FilesSessionActual(False).Exists Then
                        f = Downloader.FilesSessionActual(False)
                    Else
                        f = fList(0)
                    End If
                    x = New XmlFile(f,, False) With {.AllowSameNames = True, .XmlReadOnly = True}
                    x.LoadData()
                    If x.Count > 0 Then DataList.Clear() : DataList.ListAddList(x, lcr)
                    x.Dispose()
                    CleanDataList()
                    RefillList(False)
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
#Region "Load fav, spec"
        Private Sub BTT_LOAD_FAV_Click(sender As Object, e As EventArgs) Handles BTT_LOAD_FAV.Click
            DataList.Clear()
            With Settings.Feeds.Favorite
                .RemoveNotExist(FileNotExist)
                If .Count > 0 Then DataList.AddRange(.Self) : CleanDataList() : RefillList(False)
            End With
        End Sub
        Private Sub BTT_LOAD_SPEC_Click(sender As Object, e As EventArgs) Handles BTT_LOAD_SPEC.Click
            With FeedSpecialCollection.ChooseFeeds(False)
                If .ListExists Then
                    DataList.Clear()
                    Dim d As New List(Of UserMediaD)
                    .ForEach(Sub(ByVal f As FeedSpecial)
                                 f.RemoveNotExist(FileNotExist)
                                 If f.Count > 0 Then d.AddRange(f)
                             End Sub)
                    If d.Count > 0 Then DataList.ListAddList(d.Distinct)
                    CleanDataList()
                    RefillList(False)
                End If
            End With
        End Sub
#End Region
#Region "Add remove fav spec"
        Private Sub BTT_FEED_ADD_FAV_Click(sender As Object, e As EventArgs) Handles BTT_FEED_ADD_FAV.Click
            Settings.Feeds.Favorite.Add(GetCheckedMedia())
        End Sub
        Private Sub BTT_FEED_REMOVE_FAV_Click(sender As Object, e As EventArgs) Handles BTT_FEED_REMOVE_FAV.Click
            Settings.Feeds.Favorite.Remove(GetCheckedMedia())
        End Sub
        Private Sub BTT_FEED_ADD_SPEC_Click(sender As Object, e As EventArgs) Handles BTT_FEED_ADD_SPEC.Click
            Dim c As IEnumerable(Of UserMediaD) = GetCheckedMedia()
            If c.ListExists Then
                With FeedSpecialCollection.ChooseFeeds(False)
                    If .ListExists Then .ForEach(Sub(f) f.Add(c))
                End With
            Else
                MsgBoxE({"You haven't selected media to add to your feed(s)", "Add to feed(s)"}, vbExclamation)
            End If
        End Sub
        Private Sub BTT_FEED_REMOVE_SPEC_Click(sender As Object, e As EventArgs) Handles BTT_FEED_REMOVE_SPEC.Click
            Dim c As IEnumerable(Of UserMediaD) = GetCheckedMedia()
            If c.ListExists Then
                With FeedSpecialCollection.ChooseFeeds(False)
                    If .ListExists Then .ForEach(Sub(f) f.Remove(c))
                End With
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
                    If MsgBoxE({$"Are you sure you want to clear the following feeds?{vbCr}{vbCr}{ .ListToString(vbCr)}", "Clear feed"}, vbExclamation,,,
                               {"Process", "Cancel"}) = 0 Then .ForEach(Sub(f) f.Clear())
                End If
            End With
        End Sub
        Private Sub BTT_FEED_DELETE_SPEC_Click(sender As Object, e As EventArgs) Handles BTT_FEED_DELETE_SPEC.Click
            With FeedSpecialCollection.ChooseFeeds(False)
                If .ListExists Then
                    If MsgBoxE({$"Are you sure you want to delete the following feeds?{vbCr}{vbCr}{ .ListToString(vbCr)}", "Delete feed"}, vbExclamation,,,
                               {"Process", "Cancel"}) = 0 Then .ForEach(Sub(f) f.Delete())
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
                MainFrameObj.UpdateLogButton()
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
                MainFrameObj.UpdateLogButton()
            End Try
        End Sub
#End Region
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
        Friend Sub Downloader_FilesChanged(ByVal Added As Boolean)
            ControlInvokeFast(ToolbarTOP, BTT_REFRESH, Sub() BTT_REFRESH.ToolTipText = If(Added, "New files found", "Some files have been removed"))
            BTT_REFRESH.ControlChangeColor(ToolbarTOP, Added, False)
        End Sub
        Private Sub BTT_REFRESH_Click(sender As Object, e As EventArgs) Handles BTT_REFRESH.Click
            RefillList()
        End Sub
        Private Sub BTT_CLEAR_Click(sender As Object, e As EventArgs) Handles BTT_CLEAR.Click
            Downloader.Files.Clear()
            ClearTable()
            RefillList()
        End Sub
#End Region
#Region "Download"
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
#End Region
#Region "Delete"
        Private Sub BTT_DELETE_SELECTED_Click(sender As Object, e As EventArgs) Handles BTT_DELETE_SELECTED.Click
            Const MsgTitle$ = "Deleting marked files"
            Try
                Dim c As IEnumerable(Of FeedMedia) = ControlInvoke(TP_DATA, Function() If(TP_DATA.Controls.Count > 0, TP_DATA.Controls.ToObjectsList.Cast(Of FeedMedia)().Where(Function(f) f.Checked), New FeedMedia() {}))
                If c.ListExists Then
                    If MsgBoxE({$"Are you sure you want to delete {c.Count} file(s)?", MsgTitle}, vbExclamation,,, {"Process", "Cancel"}) = 0 Then
                        Dim indx% = MyRange.CurrentIndex
                        Dim d% = 0
                        ControlInvoke(TP_DATA, Sub()
                                                   With TP_DATA
                                                       .SuspendLayout()
                                                       LatestScrollValueDisabled = True
                                                       For Each fm As FeedMedia In c
                                                           If fm.DeleteFile(True) Then
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
        Private Sub FeedMedia_MediaDeleted(ByVal Sender As FeedMedia)
            Try
                ControlInvoke(TP_DATA, Sub() TPRemoveControl(Sender, True))
                DataList.RemoveAll(Function(dd) dd.Data.File = Sender.File)
                RefillAfterDelete()
            Catch
            End Try
        End Sub
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
                        RefillList()
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
                                      fmList.Add(New FeedMedia(de, w, h))
                                      With fmList.Last
                                          AddHandler .MediaDeleted, AddressOf FeedMedia_MediaDeleted
                                          AddHandler .MediaDownload, AddressOf FeedMedia_Download
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