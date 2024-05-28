' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports System.ComponentModel
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Toolbars
Imports SCrawler.API.Base
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace Editors
    Friend Class UsersInfoForm
#Region "Declarations"
        Private ReadOnly MyView As FormView
        Private ReadOnly MyProgress As MyProgress
        Private MyThread As Thread = Nothing
        Private TokenSource As CancellationTokenSource = Nothing
        Private Token As CancellationToken = Nothing
        Private ReadOnly MyUsers As List(Of UserOpt)
        Private ReadOnly LetterGroups As Dictionary(Of String, ListViewGroup)
        Private ReadOnly MyNumberProvider As ANumbers
        Private ReadOnly SizeNumberProvider As ANumbers
        Private Enum EComparers As Integer
            Size = 0
            [Date] = 1
            Amount = 2
        End Enum
#Region "Comparers declarations"
        Private ReadOnly MyComparerDate As New ComparerDate
        Private ReadOnly MyComparerSize As New ComparerSize
        Private ReadOnly MyComparerAmount As New ComparerAmount
#End Region
#Region "Comparers classes"
        Private Class ComparerDate : Implements IComparer(Of UserOpt)
            Protected _Order As Integer = -1
            Friend Property Order As SortOrder
                Get
                    Return IIf(_Order = -1, SortOrder.Descending, SortOrder.Ascending)
                End Get
                Set(ByVal _Order As SortOrder)
                    If _Order = SortOrder.Descending Then Me._Order = -1 Else Me._Order = 1
                End Set
            End Property
            Friend Overridable Function Compare(ByVal x As UserOpt, ByVal y As UserOpt) As Integer Implements IComparer(Of UserOpt).Compare
                Dim xd& = If(x.User.LastUpdated, New Date).Ticks
                Dim yd& = If(y.User.LastUpdated, New Date).Ticks
                Return xd.CompareTo(yd) * _Order
            End Function
        End Class
        Private Class ComparerSize : Inherits ComparerDate
            Friend Overrides Function Compare(ByVal x As UserOpt, ByVal y As UserOpt) As Integer
                Return x.TotalSize.CompareTo(y.TotalSize) * _Order
            End Function
        End Class
        Private Class ComparerAmount : Inherits ComparerDate
            Friend Overrides Function Compare(ByVal x As UserOpt, ByVal y As UserOpt) As Integer
                Return x.Files.Count.CompareTo(y.Files.Count) * _Order
            End Function
        End Class
#End Region
#Region "Classes"
        Private Structure FileOpt
            Friend File As SFile
            Friend Size As Double
            Friend Type As UTypes
            Friend Sub New(ByVal f As SFile, Optional ByVal CalculateSize As Boolean = False)
                File = f
                If CalculateSize Then Size = File.Size
                Type = UTypes.Undefined
                If Not f.Extension.IsEmptyString Then
                    Select Case f.Extension
                        Case "jpg", "jped", "png", "webp" : Type = UTypes.Picture
                        Case "gif" : Type = UTypes.GIF
                        Case "mp4", "mkv" : Type = UTypes.Video
                    End Select
                End If
            End Sub
            Public Shared Widening Operator CType(ByVal f As SFile) As FileOpt
                Return New FileOpt(f)
            End Operator
            Public Shared Widening Operator CType(ByVal f As FileOpt) As SFile
                Return f.File
            End Operator
            Public Shared Narrowing Operator CType(ByVal f As FileOpt) As Double
                Return f.Size
            End Operator
        End Structure
        Private NotInheritable Class UserOpt : Implements IComparable(Of UserOpt), IDisposable
            Friend Property User As UserDataBase
            Friend Property UserPath As SFile
            Friend Property Letter As String
            Friend ReadOnly Property Files As List(Of FileOpt)
            Friend Property TotalSize As Double = 0
            Friend Property CollectionName As String
            Friend Property Name As String
            Friend Property Site As String
            Friend Property Key As String
            Private ReadOnly NumberProvider As New ANumbers With {.FormatOptions = ANumbers.Options.GroupIntegral, .DecimalDigits = 2, .TrimDecimalDigits = True}
            Friend Sub New(ByVal User As UserDataBase)
                Me.User = User
                Files = New List(Of FileOpt)
                CollectionName = User.CollectionName
                Site = User.Site
                Name = User.FriendlyName.IfNullOrEmpty(User.Name)
                Key = User.LVIKey
                UserPath = User.User.File.CutPath
                Letter = UserPath.Segments.FirstOrDefault.StringToUpper.StringTrimEnd(":")
            End Sub
            Friend Sub GetFiles()
                If UserPath.Exists(SFO.Path, False) Then
                    Dim files As List(Of SFile) = SFile.GetFiles(UserPath,, IO.SearchOption.AllDirectories, EDP.ReturnValue)
                    If files.ListExists Then
                        For Each f As SFile In files : Me.Files.Add(New FileOpt(f, True)) : Next
                        TotalSize = Me.Files.Sum(Function(ff) ff.Size)
                    End If
                End If
            End Sub
            Friend Function GetLVI(ByVal LetterGroup As ListViewGroup, ByVal CollectionGroup As Boolean) As ListViewItem
                Dim lvi As New ListViewItem
                Dim s$ = String.Empty
                If Not CollectionName.IsEmptyString Then s = $"{IIf(CollectionGroup, "    ", String.Empty)}{CollectionName}"
                s.StringAppend(Site, ".")
                s.StringAppend(Name, ".")
                s &= $" [{GetSizeStr(TotalSize)}]"
                If Not User.UserExists Then
                    s &= " DELETED"
                ElseIf User.UserSuspended Then
                    s &= " SUSPENDED"
                End If
                s &= ": "
                Dim infoStr$ = String.Empty
                infoStr.StringAppend(GetInfoStr(UTypes.Picture), "; ")
                infoStr.StringAppend(GetInfoStr(UTypes.GIF), "; ")
                infoStr.StringAppend(GetInfoStr(UTypes.Video), "; ")
                infoStr.StringAppend(GetInfoStr(UTypes.Undefined), "; ")
                If Not infoStr.IsEmptyString Then infoStr &= "; "
                If User.LastUpdated.HasValue Then
                    infoStr &= $"({User.LastUpdated.Value.ToStringDate(ADateTime.Formats.BaseDate)})"
                Else
                    infoStr &= "(not downloaded yet)"
                End If
                s &= infoStr
                lvi.Text = s
                lvi.Name = Key
                lvi.Tag = Me
                lvi.Group = LetterGroup
                Return lvi
            End Function
            Private Function GetSizeStr(ByVal Value As Double) As String
                If Value > 0 Then
                    Dim sizeText$ = "Mb"
                    Dim sizeValue# = Value / 1024 / 1024
                    If sizeValue > 1000 Then sizeValue /= 1024 : sizeText = "Gb"
                    Return $"{sizeValue.RoundVal(2).NumToString(NumberProvider)}{sizeText}"
                Else
                    Return "0Kb"
                End If
            End Function
            Private Function GetInfoStr(ByVal t As UTypes, Optional ByVal Separator As String = " ") As String
                Dim OutStr$ = String.Empty
                Dim d As IEnumerable(Of FileOpt) = Files.Where(Function(f) f.Type = t)
                If d.ListExists Then
                    Return $"{t} ({d.Count.NumToString(NumberProvider)}){Separator}[{GetSizeStr(d.Sum(Function(dd) dd.Size))}]"
                Else
                    Return String.Empty
                End If
            End Function
            Friend Function GetInfornation() As String
                Dim s$ = String.Empty

                If Not CollectionName.IsEmptyString Then s &= $"Collection: {CollectionName}"
                s.StringAppendLine(Site)
                s.StringAppendLine(Name)
                s.StringAppendLine($"Total size: {GetSizeStr(TotalSize)}")

                s &= vbNewLine

                s.StringAppendLine(GetInfoStr(UTypes.Picture, ": "))
                s.StringAppendLine(GetInfoStr(UTypes.GIF, ": "))
                s.StringAppendLine(GetInfoStr(UTypes.Video, ": "))
                s.StringAppendLine(GetInfoStr(UTypes.Undefined, ": "))

                If Not User.UserExists Then
                    s.StringAppendLine("User DELETED")
                ElseIf User.UserSuspended Then
                    s.StringAppendLine("User SUSPENDED")
                End If

                s.StringAppendLine("Last download date: ")
                If User.LastUpdated.HasValue Then
                    s &= User.LastUpdated.Value.ToStringDate(ADateTime.Formats.BaseDate)
                Else
                    s &= "not downloaded yet"
                End If

                Return s
            End Function
#Region "IComparable Support"
            Private Function CompareTo(ByVal Other As UserOpt) As Integer Implements IComparable(Of UserOpt).CompareTo
                Return TotalSize.CompareTo(Other.TotalSize) * -1
            End Function
#End Region
#Region "IDisposable Support"
            Private disposedValue As Boolean = False
            Protected Overloads Sub Dispose(ByVal disposing As Boolean)
                If Not disposedValue Then
                    If disposing Then Files.Clear()
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
#End Region
#End Region
#Region "Initializer"
        Public Sub New()
            InitializeComponent()
            MyView = New FormView(Me, Settings.Design)
            MyProgress = New MyProgress(Toolbar_BOTTOM, PR_MAIN, LBL_STATUS)
            MyUsers = New List(Of UserOpt)
            LetterGroups = New Dictionary(Of String, ListViewGroup)
            MyNumberProvider = New ANumbers With {.FormatOptions = ANumbers.Options.GroupIntegral}
            SizeNumberProvider = New ANumbers With {.FormatOptions = ANumbers.Options.GroupIntegral, .DecimalDigits = 2, .TrimDecimalDigits = True}
        End Sub
#End Region
#Region "Form handlers"
        Private Sub UsersInfoForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            MyView.Import()
            MyView.SetFormSize()

            OPT_DATE.Tag = CInt(EComparers.Date)
            OPT_SIZE.Tag = CInt(EComparers.Size)
            OPT_AMOUNT.Tag = CInt(EComparers.Amount)
            Select Case Settings.UMetrics_What.Value
                Case EComparers.Date : OPT_DATE.Checked = True
                Case EComparers.Amount : OPT_AMOUNT.Checked = True
                Case Else : OPT_SIZE.Checked = True
            End Select

            OPT_ASC.Tag = CInt(SortOrder.Ascending)
            OPT_DESC.Tag = CInt(SortOrder.Descending)
            If Settings.UMetrics_Order.Value = SortOrder.Ascending Then
                OPT_ASC.Checked = True
            Else
                OPT_DESC.Checked = True
            End If

            CH_GROUP_DRIVE.Checked = Settings.UMetrics_ShowDrives
            CH_GROUP_COL.Checked = Settings.UMetrics_ShowCollections
            LIST_DATA.ShowGroups = CH_GROUP_DRIVE.Checked

            COL_DEFAULT.Width = -2
        End Sub
        Private Sub UsersInfoForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            e.Cancel = True
            Hide()
        End Sub
        Private Sub UsersInfoForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            Abort()
            MyProgress.Dispose()
            MyView.Dispose()
        End Sub
        Private Sub UsersInfoForm_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
            Try : ControlInvokeFast(LIST_DATA, Sub() COL_DEFAULT.Width = -2, EDP.None) : Catch : End Try
        End Sub
#End Region
#Region "Calculating"
        Private Sub Abort()
            Try
                If If(MyThread?.IsAlive, False) Then TokenSource.Cancel() : MyThread.Abort()
            Catch ex As Exception
            End Try
        End Sub
        Private _CalculationInProgress As Boolean = False
        Private Sub BTT_START_Click(sender As Object, e As EventArgs) Handles BTT_START.Click
            If Not If(MyThread?.IsAlive, False) Then
                _CalculationInProgress = True
                MyUsers.ListClearDispose
                LetterGroups.Clear()
                LIST_DATA.Groups.Clear()
                LIST_DATA.Items.Clear()
                If Not TokenSource Is Nothing Then TokenSource.Dispose()
                TokenSource = New CancellationTokenSource
                Token = TokenSource.Token
                ChangeControlsEnabled(True)
                MyThread = New Thread(New ThreadStart(AddressOf Calculate))
                MyThread.SetApartmentState(ApartmentState.MTA)
                MyThread.IsBackground = True
                MyThread.Start()
            Else
                MsgBoxE({"The calculating is already underway", "Calculating"}, vbCritical)
            End If
        End Sub
        Private Sub BTT_CANCEL_Click(sender As Object, e As EventArgs) Handles BTT_CANCEL.Click
            TokenSource.Cancel()
            ControlInvokeFast(Toolbar_TOP, BTT_CANCEL, Sub() BTT_CANCEL.Enabled = False, EDP.None)
        End Sub
        Private Sub ChangeControlsEnabled(ByVal Working As Boolean)
            Try
                ControlInvokeFast(Toolbar_TOP, BTT_START, Sub()
                                                              BTT_START.Enabled = Not Working
                                                              BTT_CANCEL.Enabled = Working
                                                          End Sub, EDP.None)
            Catch
            End Try
        End Sub
        Private Sub Calculate()
            Try
                MyProgress.Visible = True
                MyProgress.Reset()
                If Settings.Users.Count > 0 Then
                    With Settings.Users.SelectMany(Function(ByVal u As IUserData) As IEnumerable(Of IUserData)
                                                       If u.IsCollection Then
                                                           With DirectCast(u, API.UserDataBind)
                                                               If .Count > 0 Then Return .Collections Else Return New UserDataBase() {}
                                                           End With
                                                       Else
                                                           Return {u}
                                                       End If
                                                   End Function)
                        If .ListExists Then .ToList.ForEach(Sub(u As UserDataBase) MyUsers.Add(New UserOpt(u)))
                    End With
                End If

                If MyUsers.Count > 0 Then
                    MyProgress.Maximum += MyUsers.Count
                    Dim i% = 0

                    Dim letters As IEnumerable(Of String) = MyUsers.Select(Function(u) u.Letter).Distinct
                    LetterGroups.Clear()
                    If letters.ListExists(2) Then
                        ControlInvokeFast(LIST_DATA, Sub()
                                                         For Each l$ In letters
                                                             LetterGroups.Add(l, New ListViewGroup(l, $"Drive {l}"))
                                                             LIST_DATA.Groups.Add(LetterGroups.Last.Value)
                                                         Next
                                                     End Sub, EDP.None)
                    End If

                    MyProgress.Information = "Calculating of user metrics"
                    For Each user As UserOpt In MyUsers
                        Token.ThrowIfCancellationRequested()
                        i += 1
                        MyProgress.Perform()
                        user.GetFiles()
                    Next

                    _CalculationInProgress = False
                    RefillList()
                End If
                MyProgress.Done()
                MyProgress.InformationTemporary = "All user metrics have been calculated."
            Catch oex As OperationCanceledException
                MyProgress.Done()
                MyProgress.InformationTemporary = "Operation canceled"
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[UsersInfoForm.Calculate]")
                MyProgress.Done()
                MyProgress.InformationTemporary = "An error occurred while calculating user metrics."
            Finally
                MyProgress.Visible(, False) = False
                ChangeControlsEnabled(False)
                _CalculationInProgress = False
            End Try
        End Sub
        Private _RefillInProgress As Boolean = False
        Private Sub RefillList()
            If Not _CalculationInProgress AndAlso Not _RefillInProgress AndAlso MyUsers.Count > 0 Then
                _RefillInProgress = True
                ControlInvokeFast(LIST_DATA, Sub() LIST_DATA.Items.Clear(), EDP.None)
                If MyUsers.Count > 0 Then
                    Dim i% = 0
                    Dim g As Func(Of UserOpt, ListViewGroup) = Function(u) If(LetterGroups.Count > 1, LetterGroups(u.Letter), Nothing)
                    Dim comparer As IComparer(Of UserOpt)

                    Select Case True
                        Case OPT_DATE.Checked : comparer = MyComparerDate
                        Case OPT_AMOUNT.Checked : comparer = MyComparerAmount
                        Case Else : comparer = MyComparerSize
                    End Select
                    DirectCast(comparer, ComparerDate).Order = IIf(OPT_ASC.Checked, SortOrder.Ascending, SortOrder.Descending)

                    MyUsers.Sort(comparer)
                    ControlInvokeFast(LIST_DATA, Sub()
                                                     Dim user As UserOpt
                                                     Dim gg As Boolean = CH_GROUP_COL.Checked
                                                     Dim colUsers As New Dictionary(Of String, List(Of UserOpt))
                                                     Dim colUsersNo As New List(Of UserOpt)
                                                     Dim lvi As ListViewItem
                                                     Dim s#
                                                     Dim sn$

                                                     For Each user In MyUsers
                                                         If gg And Not user.CollectionName.IsEmptyString Then
                                                             If colUsers.ContainsKey(user.CollectionName) Then
                                                                 colUsers(user.CollectionName).Add(user)
                                                             Else
                                                                 colUsers.Add(user.CollectionName, New List(Of UserOpt) From {user})
                                                             End If
                                                         Else
                                                             colUsersNo.Add(user)
                                                         End If
                                                     Next

                                                     If colUsers.Count > 0 Then
                                                         For Each kv As KeyValuePair(Of String, List(Of UserOpt)) In colUsers
                                                             sn = "Mb"
                                                             s = kv.Value.Sum(Function(v) v.TotalSize) / 1024 / 1024
                                                             If s > 1000 Then s /= 1024 : sn = "Gb"
                                                             lvi = New ListViewItem($"Collection: {kv.Key}: {s.RoundVal(2).NumToString(SizeNumberProvider)}{sn}") With {
                                                                .Tag = kv.Value(0),
                                                                .Name = Settings.GetUser(kv.Value(0).User, True).Key,
                                                                .Group = g(kv.Value(0))
                                                             }
                                                             LIST_DATA.Items.Add(lvi)
                                                             For Each user In kv.Value : LIST_DATA.Items.Add(user.GetLVI(g(user), gg)) : Next
                                                         Next
                                                     End If

                                                     If colUsersNo.Count > 0 Then
                                                         For Each user In colUsersNo : LIST_DATA.Items.Add(user.GetLVI(g(user), gg)) : Next
                                                     End If

                                                     COL_DEFAULT.Width = -2
                                                 End Sub, EDP.None)
                End If
                _RefillInProgress = False
            End If
        End Sub
#End Region
#Region "View"
        Private Sub OPT_SORT_Click(ByVal Sender As ToolStripMenuItem, ByVal e As EventArgs) Handles OPT_DATE.Click, OPT_SIZE.Click, OPT_AMOUNT.Click
            If Not Sender.Checked Then
                Sender.Checked = True
            Else
                Settings.UMetrics_What.Value = Sender.Tag
                For Each obj As ToolStripMenuItem In {OPT_DATE, OPT_SIZE, OPT_AMOUNT}
                    If Not obj Is Sender Then obj.Checked = False
                Next
                RefillList()
            End If
        End Sub
        Private Sub OPT_ASC_DESC_Click(ByVal Sender As ToolStripMenuItem, ByVal e As EventArgs) Handles OPT_ASC.Click, OPT_DESC.Click
            If Not Sender.Checked Then
                Sender.Checked = True
            Else
                Settings.UMetrics_Order.Value = Sender.Tag
                For Each obj As ToolStripMenuItem In {OPT_ASC, OPT_DESC}
                    If Not obj Is Sender Then obj.Checked = False
                Next
                RefillList()
            End If
        End Sub
        Private Sub CH_GROUP_DRIVE_Click(ByVal Sender As ToolStripMenuItem, ByVal e As EventArgs) Handles CH_GROUP_DRIVE.Click
            LIST_DATA.ShowGroups = Sender.Checked
            Settings.UMetrics_ShowDrives.Value = Sender.Checked
        End Sub
        Private Sub CH_GROUP_COL_Click(ByVal Sender As ToolStripMenuItem, ByVal e As EventArgs) Handles CH_GROUP_COL.Click
            Settings.UMetrics_ShowCollections.Value = Sender.Checked
            RefillList()
        End Sub
#End Region
#Region "Context handlers"
        Private Function GetUserFromList() As UserOpt
            Try
                If LIST_DATA.SelectedItems.Count > 0 Then
                    Dim i As ListViewItem = LIST_DATA.SelectedItems(0)
                    If Not i Is Nothing Then Return i.Tag
                End If
            Catch ex As Exception
            End Try
            Return Nothing
        End Function
        Private Sub CONTEXT_BTT_FIND_Click(sender As Object, e As EventArgs) Handles CONTEXT_BTT_FIND.Click
            MainFrameObj.FocusUser(If(GetUserFromList()?.Key, String.Empty), True)
        End Sub
        Private Sub CONTEXT_BTT_INFO_Click(sender As Object, e As EventArgs) Handles CONTEXT_BTT_INFO.Click
            Dim info$ = If(GetUserFromList()?.GetInfornation(), String.Empty)
            If Not info.IsEmptyString Then MsgBoxE({info, "User information"})
        End Sub
        Private Sub CONTEXT_BTT_OPEN_FOLDER_Click(sender As Object, e As EventArgs) Handles CONTEXT_BTT_OPEN_FOLDER.Click
            OpenUserFolder()
        End Sub
        Private Sub CONTEXT_BTT_OPEN_SITE_Click(sender As Object, e As EventArgs) Handles CONTEXT_BTT_OPEN_SITE.Click
            Dim u As UserOpt = GetUserFromList()
            If Not u Is Nothing Then u.User.OpenSite()
        End Sub
#End Region
#Region "List handlers"
        Private Sub LIST_DATA_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles LIST_DATA.MouseDoubleClick
            OpenUserFolder()
        End Sub
#End Region
#Region "Functions"
        Private Sub OpenUserFolder()
            Dim u As UserOpt = GetUserFromList()
            If Not u Is Nothing Then u.User.OpenFolder()
        End Sub
#End Region
    End Class
End Namespace