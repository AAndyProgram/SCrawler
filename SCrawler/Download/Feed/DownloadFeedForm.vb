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
        Private ReadOnly FileNotExist As New FPredicate(Of UserMediaD)(Function(d) Not d.Data.File.Exists)
        Private BttRefreshToolTipText As String = "Refresh data list"
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
                    .ButtonKey(RCI.Previous) = Keys.F3
                    .ButtonKey(RCI.Next) = Keys.F4
                    .AddThisToolbar()
                End With
                ToolbarTOP.Items.AddRange({New ToolStripSeparator, BTT_DELETE_SELECTED})
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
#Region "Settings"
        Friend Sub UpdateSettings()
            With Settings
                Dim c% = .FeedDataRows * .FeedDataColumns
                Dim rangeChanged As Boolean = Not c = DataRows * DataColumns
                DataRows = .FeedDataRows
                DataColumns = .FeedDataColumns
                FeedEndless = .FeedEndless
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
                If Not MyDefs.Initializing And rangeChanged Then RefillList()
            End With
        End Sub
#End Region
#Region "Refill"
        Friend Sub Downloader_FilesChanged(ByVal Added As Boolean)
            ControlInvokeFast(ToolbarTOP, BTT_REFRESH, Sub() BTT_REFRESH.ToolTipText = If(Added, "New files found", "Some files have been removed"))
            BTT_REFRESH.ControlChangeColor(ToolbarTOP, Added, False)
        End Sub
        Private Sub BTT_REFRESH_Click(sender As Object, e As EventArgs) Handles BTT_REFRESH.Click
            RefillList()
        End Sub
        Private Sub RefillList(Optional ByVal RefillDataList As Boolean = True)
            DataPopulated = False
            If RefillDataList Then
                Try : Downloader.Files.RemoveAll(FileNotExist) : Catch : End Try
                DataList.ListAddList(Downloader.Files, LAP.ClearBeforeAdd, LAP.NotContainsOnly)
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
        Private Sub BTT_CLEAR_Click(sender As Object, e As EventArgs) Handles BTT_CLEAR.Click
            Downloader.Files.Clear()
            ClearTable()
            RefillList()
        End Sub
        Private Sub BTT_LOAD_SESSION_LAST_Click(sender As Object, e As EventArgs) Handles BTT_LOAD_SESSION_LAST.Click
            SessionChooser(True)
        End Sub
        Private Sub BTT_LOAD_SESSION_CHOOSE_Click(sender As Object, e As EventArgs) Handles BTT_LOAD_SESSION_CHOOSE.Click
            SessionChooser(False)
        End Sub
        Private Sub SessionChooser(ByVal GetLast As Boolean)
            Try
                Dim f As SFile = TDownloader.SessionsPath.CSFileP
                Dim fList As List(Of SFile) = Nothing
                Dim m As New MMessage("Saved sessions not selected", "Sessions",, vbExclamation)
                Dim x As XmlFile
                Dim lcr As New ListAddParams(LAP.NotContainsOnly + LAP.IgnoreICopier)
                If Not GetLast AndAlso f.Exists(SFO.Path, False) Then fList = SFile.GetFiles(f, "*.xml",, EDP.ReturnValue)
                If Not GetLast AndAlso fList.ListExists Then
                    Using chooser As New SimpleListForm(Of SFile)(fList, Settings.Design) With {
                        .FormText = "Sessions",
                        .Icon = My.Resources.ArrowDownIcon_Blue_24,
                        .Mode = SimpleListFormModes.CheckedItems,
                        .Provider = New CustomProvider(Function(v, d, p, n, ee) DirectCast(v, SFile).File)
                    }
                        chooser.ClearButtons()
                        If chooser.ShowDialog = DialogResult.OK Then
                            fList = chooser.DataResult
                            If fList.ListExists Then
                                DataList.Clear()
                                For Each f In fList
                                    x = New XmlFile(f,, False) With {.AllowSameNames = True, .XmlReadOnly = True}
                                    x.LoadData()
                                    If x.Count > 0 Then DataList.ListAddList(x, lcr)
                                    x.Dispose()
                                Next
                                DataList.RemoveAll(FileNotExist)
                                RefillList(False)
                            Else
                                MsgBoxE(m)
                            End If
                        Else
                            MsgBoxE(m)
                        End If
                    End Using
                ElseIf Downloader.FilesSessionActual.Exists Then
                    x = New XmlFile(Downloader.FilesSessionActual,, False) With {.AllowSameNames = True, .XmlReadOnly = True}
                    x.LoadData()
                    If x.Count > 0 Then DataList.Clear() : DataList.ListAddList(x, lcr)
                    x.Dispose()
                    RefillList(False)
                Else
                    m.Text = "Saved sessions not found"
                    MsgBoxE(m)
                End If
                If fList.ListExists Then fList.Clear()
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendInLog, ex, $"[DownloadObjects.DownloadFeedForm.SessionChooser({GetLast})]")
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
                                                           If LatestScrollValue.HasValue Then TP_DATA.VerticalScroll.Value = LatestScrollValue.Value
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
                        If LatestScrollValue.HasValue Then TP_DATA.VerticalScroll.Value = LatestScrollValue.Value
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
                        If LatestScrollValue.HasValue Then
                            TP_DATA.VerticalScroll.Value = LatestScrollValue.Value
                            TP_DATA.PerformLayout()
                        End If
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
                    Dim d As List(Of UserMediaD) = MyRange.Current
                    Dim d2 As List(Of UserMediaD)
                    Dim i%
                    If d.ListExists And d.All(FileNotExist) Then
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
                                 Where(Function(md) Not FileNotExist.Predicate(md)).ListTake(-2, DataColumns, EDP.ReturnValue).ListIfNothing
                            If d2.Count > 0 Then d.InsertRange(0, d2) : d2.Clear()
                        End If
                        Dim w% = GetWidth()
                        Dim p As New TPCELL(DataRows, DataColumns)
                        Dim fmList As New List(Of FeedMedia)
                        d.ForEach(Sub(de) fmList.Add(New FeedMedia(de, w, AddressOf FeedMedia_MediaDeleted)))
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
                ErrorsDescriber.Execute(EDP.SendInLog, ex, $"[DownloadObjects.DownloadFeedForm.Range.IndexChanged({Sender.CurrentIndex})]")
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
        Private Sub DownloadFeedForm_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
            ResizeGrid()
        End Sub
        Private Sub DownloadFeedForm_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
            If Not LastWinState = WindowState And Not If(MyDefs?.Initializing, True) Then LastWinState = WindowState : ResizeGrid()
        End Sub
        Private Sub ResizeGrid()
            ControlInvoke(TP_DATA, Sub()
                                       With TP_DATA
                                           If .Controls.Count > 0 Then
                                               Dim w% = GetWidth()
                                               Dim p As TableLayoutPanelCellPosition
                                               Dim rh As New Dictionary(Of Integer, List(Of Integer))
                                               For Each cnt As FeedMedia In .Controls
                                                   cnt.Width = w
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
                                                                          TP_DATA.VerticalScroll.Value = LatestScrollValue.Value
                                                                          TP_DATA.PerformLayout()
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
#End Region
        Private Sub ClearTable()
            ControlInvoke(TP_DATA, Sub()
                                       If TP_DATA.Controls.Count > 0 Then
                                           For Each cnt As Control In TP_DATA.Controls : cnt.Dispose() : Next
                                           TP_DATA.Controls.Clear()
                                       End If
                                   End Sub)
        End Sub
    End Class
End Namespace