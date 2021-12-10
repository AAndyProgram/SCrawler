Imports System.Threading
Imports EOptions = PersonalUtilities.Forms.Toolbars.MyProgress.EnableOptions
Imports SCrawler.API
Imports SCrawler.API.Base
Friend Class TDownloader : Implements IDisposable
    Friend Event OnJobsChange(ByVal JobsCount As Integer)
    Friend Event OnDownloadCountChange()
    Friend Event OnDownloading(ByVal Value As Boolean)
    Private TokenSource As CancellationTokenSource
    Private ReadOnly Items As List(Of IUserData)
    Friend ReadOnly Property Downloaded As List(Of IUserData)
    Private ReadOnly NProv As IFormatProvider
    Private _Working As Boolean = False
    Friend ReadOnly Property Working As Boolean
        Get
            Return _Working
        End Get
    End Property
    Private DThread As Thread
    Friend ReadOnly Property Count As Integer
        Get
            Return Items.Count
        End Get
    End Property
    Friend Sub New()
        Items = New List(Of IUserData)
        Downloaded = New List(Of IUserData)
        NProv = New ANumbers(ANumbers.Modes.USA) With {
            .FormatMode = ANumbers.Formats.Number,
            .GroupSize = 3,
            .GroupSeparator = ANumbers.DefaultGroupSeparator,
            .DecimalDigits = 0
        }
    End Sub
    Friend Sub [Start]()
        If Not _Working AndAlso Count > 0 AndAlso Not If(DThread?.IsAlive, False) Then
            DThread = New Thread(New ThreadStart(AddressOf StartDownloading))
            DThread.SetApartmentState(ApartmentState.MTA)
            DThread.Start()
        End If
    End Sub
    Private Sub StartDownloading()
        Dim Token As CancellationToken
        RaiseEvent OnDownloading(True)
        Try
            _Working = True
            TokenSource = New CancellationTokenSource
            Token = TokenSource.Token
            MainProgress.TotalCount = 0
            MainProgress.CurrentCounter = 0
            Do While Count > 0
                Token.ThrowIfCancellationRequested()
                UpdateJobsLabel()
                DownloadData(Token)
                Token.ThrowIfCancellationRequested()
                Thread.Sleep(500)
            Loop
            MainProgress.InformationTemporary = "All data downloaded"
        Catch oex As OperationCanceledException When Token.IsCancellationRequested
            MainProgress.InformationTemporary = "Downloading canceled"
        Catch ex As Exception
            MainProgress.InformationTemporary = "Downloading error"
            ErrorsDescriber.Execute(EDP.SendInLog, ex, "TDownloader.Start")
        Finally
            _Working = False
            TokenSource = Nothing
            UpdateJobsLabel()
            RaiseEvent OnDownloading(False)
        End Try
    End Sub
    Friend Sub [Stop]()
        If _Working Then TokenSource.Cancel()
    End Sub
    Private Sub UpdateJobsLabel()
        RaiseEvent OnJobsChange(Count)
    End Sub
    Private _CurrentDownloadingTasks As Integer = 0
    Private Sub DownloadData(ByVal Token As CancellationToken)
        Try
            If Items.Count > 0 Then
                Const nf As ANumbers.Formats = ANumbers.Formats.Number
                Dim t As New List(Of Task)
                Dim i% = -1
                Dim j% = Settings.MaxUsersJobsCount - 1
                Dim Keys As New List(Of String)
                For Each _Item As IUserData In Items
                    If Not _Item.Disposed Then
                        Keys.Add(_Item.LVIKey)
                        Token.ThrowIfCancellationRequested()
                        t.Add(Task.Run(Sub() _Item.DownloadData(Token)))
                        i += 1
                        If i >= j Then Exit For
                    End If
                Next
                If t.Count > 0 Then
                    _CurrentDownloadingTasks = t.Count
                    With MainProgress
                        .Enabled(EOptions.All) = True
                        .Information = $"Downloading {_CurrentDownloadingTasks.NumToString(nf, NProv)}/{Items.Count.NumToString(nf, NProv)} profiles' data"
                        .InformationTemporary = .Information
                    End With
                    Task.WaitAll(t.ToArray)
                    Dim dcc As Boolean = False
                    If Keys.Count > 0 Then
                        For Each k$ In Keys
                            i = Items.FindIndex(Function(ii) ii.LVIKey = k)
                            If i >= 0 Then
                                With Items(i)
                                    If Not .Disposed AndAlso Not .IsCollection AndAlso .DownloadedTotal(False) > 0 Then
                                        If Not Downloaded.Contains(.Self) Then Downloaded.Add(GetUserFromMainCollection(.Self))
                                        dcc = True
                                    End If
                                End With
                                Items.RemoveAt(i)
                            End If
                        Next
                    End If
                    Keys.Clear()
                    Items.RemoveAll(Function(ii) ii.Disposed)
                    If dcc Then Downloaded.RemoveAll(Function(u) u Is Nothing)
                    If dcc And Downloaded.Count > 0 Then RaiseEvent OnDownloadCountChange()
                    t.Clear()
                End If
            End If
        Catch aoex As ArgumentOutOfRangeException
            ErrorsDescriber.Execute(EDP.SendInLog, aoex, $"TDownloader.DownloadData: index out of range ({Count})")
        Catch oex As OperationCanceledException When Token.IsCancellationRequested
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.SendInLog, ex, "TDownloader.DownloadData")
        Finally
            MainProgress.Enabled(EOptions.ProgressBar) = False
        End Try
    End Sub
    Private Function GetUserFromMainCollection(ByVal User As IUserData) As IUserData
        Dim uSimple As Predicate(Of IUserData) = Function(u) u.Equals(DirectCast(User, UserDataBase))
        Dim uCol As Predicate(Of IUserData) = Function(ByVal u As IUserData) As Boolean
                                                  If u.IsCollection Then
                                                      Return DirectCast(u, UserDataBind).Collections.Exists(uSimple)
                                                  Else
                                                      Return False
                                                  End If
                                              End Function
        Dim uu As Predicate(Of IUserData)
        If User.IncludedInCollection Then uu = uCol Else uu = uSimple
        Dim i% = Settings.Users.FindIndex(uu)
        If i >= 0 Then
            If Settings.Users(i).IsCollection Then
                With DirectCast(Settings.Users(i), UserDataBind)
                    i = .Collections.FindIndex(uSimple)
                    If i >= 0 Then Return .Collections(i)
                End With
            Else
                Return Settings.Users(i)
            End If
        End If
        Return Nothing
    End Function
    Friend Sub Add(ByVal Item As IUserData)
        If Not Items.Contains(Item) Then
            If Item.IsCollection Then Item.DownloadData(Nothing) Else Items.Add(Item)
            UpdateJobsLabel()
        End If
        If Items.Count > 0 Then Start()
    End Sub
    Friend Sub AddRange(ByVal _Items As IEnumerable(Of IUserData))
        If _Items.ListExists Then
            For i% = 0 To _Items.Count - 1
                'If i = 5 Then UpdateJobsLabel() : Start()
                If _Items(i).IsCollection Then _Items(i).DownloadData(Nothing) Else Items.Add(_Items(i))
            Next
            UpdateJobsLabel()
        End If
        If Items.Count > 0 Then Start()
    End Sub
    Friend Sub UserRemove(ByVal _Item As IUserData)
        If Downloaded.Count > 0 AndAlso Downloaded.Contains(_Item) Then Downloaded.Remove(_Item) : RaiseEvent OnDownloadCountChange()
    End Sub
#Region "IDisposable Support"
    Private disposedValue As Boolean = False
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                [Stop]()
                Items.Clear()
                Downloaded.Clear()
            End If
            disposedValue = True
        End If
    End Sub
    Protected Overrides Sub Finalize()
        Dispose(False)
        MyBase.Finalize()
    End Sub
    Friend Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class