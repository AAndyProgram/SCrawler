' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Functions.XML
Imports SCrawler.DownloadObjects.Groups
Imports PauseModes = SCrawler.DownloadObjects.AutoDownloader.PauseModes
Namespace DownloadObjects
    Friend Class Scheduler : Implements IEnumerable(Of AutoDownloader), IMyEnumerator(Of AutoDownloader), IDisposable
        Friend Const Name_Plan As String = "Plan"
        Friend Delegate Sub PlanChangedEventHandler(ByVal Plan As AutoDownloader)
        Friend Event PauseChanged As AutoDownloader.PauseChangedEventHandler
        Private Sub OnPauseChanged(ByVal Value As PauseModes)
            RaiseEvent PauseChanged(Pause)
        End Sub
        Friend Event PlanChanged As PlanChangedEventHandler
        Private Sub OnPlanChanged(ByVal Plan As AutoDownloader)
            Try : RaiseEvent PlanChanged(Plan) : Catch : End Try
        End Sub
        Private ReadOnly Plans As List(Of AutoDownloader)
        Friend Const FileNameDefault As String = "AutoDownload"
        Friend ReadOnly FileDefault As SFile = $"{SettingsFolderName}\{FileNameDefault}.xml"
        Friend File As SFile = Nothing
        Private ReadOnly PlanDownloading As Predicate(Of AutoDownloader) = Function(Plan) Plan.Downloading
        Private ReadOnly PlansWaiter As Action(Of Predicate(Of AutoDownloader)) = Sub(ByVal Predicate As Predicate(Of AutoDownloader))
                                                                                      While Plans.Exists(Predicate) : Thread.Sleep(200) : End While
                                                                                  End Sub
        Friend ReadOnly Property Name As String
            Get
                If Not File.Name.IsEmptyString AndAlso Not File.Name = FileNameDefault Then
                    Return File.Name.Replace(FileNameDefault, String.Empty).StringTrimStart("_").IfNullOrEmpty("Default")
                Else
                    Return "Default"
                End If
            End Get
        End Property
        Friend Sub New()
            Plans = New List(Of AutoDownloader)
            Dim sFolder As SFile = SettingsFolderName.CSFileP
            File = New SFile(Settings.AutomationFile.Value.IfNullOrEmpty(FileDefault.ToString))
            If File.Path.IsEmptyString OrElse Not File.Path.StartsWith(sFolder.Path) Then
                Dim updateSetting As Boolean = File.Path.IsEmptyString OrElse Not File.Path.StartsWith(sFolder.CutPath.Path)
                File.Path = sFolder.Path
                If File.Exists And updateSetting Then Settings.AutomationFile.Value = File.File
            End If
            If Not File.Exists Then File = FileDefault
            Reset(File, True)
        End Sub
        Default Friend ReadOnly Property Item(ByVal Index As Integer) As AutoDownloader Implements IMyEnumerator(Of AutoDownloader).MyEnumeratorObject
            Get
                Return Plans(Index)
            End Get
        End Property
        Friend ReadOnly Property Count As Integer Implements IMyEnumerator(Of AutoDownloader).MyEnumeratorCount
            Get
                Return Plans.Count
            End Get
        End Property
        Friend Function NotificationClicked(ByVal Key As String, ByRef Found As Boolean, ByRef ActivateForm As Boolean) As Boolean
            If Count > 0 Then
                For Each plan As AutoDownloader In Plans
                    If plan.NotificationClicked(Key, Found, ActivateForm) Then Return True
                Next
            End If
            Return False
        End Function
        Friend Sub Add(ByVal Plan As AutoDownloader)
            Plan.Source = Me
            AddHandler Plan.PauseChanged, AddressOf OnPauseChanged
            AddHandler Plan.PlanChanged, AddressOf OnPlanChanged
            Plans.Add(Plan)
            Plans.ListReindex
            Update()
        End Sub
        Friend Async Function RemoveAt(ByVal Index As Integer) As Task
            If Index.ValueBetween(0, Count - 1) Then
                With Plans(Index)
                    .Stop()
                    If .Downloading Then
                        Await Task.Run(Sub()
                                           While .Downloading : Thread.Sleep(510) : End While
                                       End Sub)
                    End If
                    .Dispose()
                End With
                Plans.RemoveAt(Index)
                Plans.ListReindex
                Update()
            End If
        End Function
        Private _UpdateRequired As Boolean = False
        Friend Sub Update()
            _UpdateRequired = True
            Try
                If Plans.Count > 0 Then
                    Using x As New XmlFile With {.Name = "Scheduler", .AllowSameNames = True} : x.AddRange(Plans) : x.Save(File) : End Using
                Else
                    File.Delete()
                End If
                _UpdateRequired = False
            Catch
            End Try
        End Sub
        Friend Function Reset(ByVal f As SFile, ByVal IsInit As Boolean) As Boolean
            If Count > 0 Then
                If Not Plans.Exists(PlanDownloading) Then
                    Pause = PauseModes.Unlimited
                    If Plans.Exists(PlanDownloading) Then
                        MsgBoxE({$"Some plans are already being worked.{vbCr}Wait for the plans to complete their work and try again.",
                                 "Change scheduler"}, vbCritical)
                        Pause = PauseModes.Unlimited
                        Return False
                    End If
                End If
                [Stop]()
                While Working : Thread.Sleep(200) : End While
                If _UpdateRequired Then Update()
                Plans.ListClearDispose(,, EDP.LogMessageValue)
            End If
            If f.Exists Then
                File = f
                Using x As New XmlFile(File,, False) With {.AllowSameNames = True}
                    x.LoadData()
                    If x.Contains(Name_Plan) Then
                        For Each e In x : Plans.Add(New AutoDownloader(e)) : Next
                    Else
                        Plans.Add(New AutoDownloader(x))
                    End If
                End Using
                If Count > 0 Then Plans.ForEach(Sub(ByVal p As AutoDownloader)
                                                    p.Source = Me
                                                    If Not IsInit Then p.Pause = PauseModes.Unlimited
                                                    AddHandler p.PauseChanged, AddressOf OnPauseChanged
                                                    AddHandler p.PlanChanged, AddressOf OnPlanChanged
                                                End Sub) : Plans.ListReindex
            End If
            Return True
        End Function
        Friend Function Move(ByVal Index As Integer, ByVal Up As Boolean) As Integer
            Try
                If Index.ValueBetween(0, Count - 1) Then
                    Plans.ListReindex
                    Dim v% = IIf(Up, -1, 1)
                    Dim newIndx%
                    Item(Index).Index += v
                    newIndx = Item(Index).Index
                    If newIndx.ValueBetween(0, Count - 1) Then Item(newIndx).Index += v * -1
                    Plans.Sort()
                    Plans.ListReindex
                    Update()
                    Return newIndx
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[Scheduler.Move]")
            End Try
            Return -1
        End Function
#Region "Groups Support"
        Friend Sub GROUPS_Updated(ByVal Sender As DownloadGroup)
            If Count > 0 Then Plans.ForEach(Sub(p) p.GROUPS_Updated(Sender))
        End Sub
        Friend Sub GROUPS_Deleted(ByVal Sender As DownloadGroup)
            If Count > 0 Then Plans.ForEach(Sub(p) p.GROUPS_Deleted(Sender))
        End Sub
#End Region
#Region "Execution"
        Private AThread As Thread = Nothing
        Private _StopRequested As Boolean = False
        Friend ReadOnly Property Working As Boolean
            Get
                Return If(AThread?.IsAlive, False)
            End Get
        End Property
        Friend Async Function Start(ByVal Init As Boolean) As Task
            Try
                _StopRequested = False
                Await Task.Run(Sub()
                                   Dim r% = 0
                                   Do
                                       r += 1
                                       Try
                                           If Count > 0 Then PlansWaiter(PlanDownloading) : Plans.ForEach(Sub(p) p.Start(Init))
                                           Exit Do
                                       Catch io_ex As InvalidOperationException 'Collection was modified; enumeration operation may not execute
                                       End Try
                                   Loop While r < 10
                               End Sub)
                If Not Working Then
                    AThread = New Thread(New ThreadStart(AddressOf Checker))
                    AThread.SetApartmentState(ApartmentState.MTA)
                    AThread.Start()
                End If
            Catch ex As Exception
                If Init Then
                    ErrorsDescriber.Execute(EDP.SendToLog, ex, "Start automation")
                Else
                    Throw ex
                End If
            End Try
        End Function
        Friend Sub [Stop]()
            If Working Then _StopRequested = True
            If Count > 0 Then Plans.ForEach(Sub(p) p.Stop())
        End Sub
        Private Sub Checker()
            Do
                Try
                    If Count = 0 Or _StopRequested Then Exit Sub
                    PlansWaiter.Invoke(PlanDownloading)
                    Dim i% = Checker_GetNextPlanIndex()
                    If i >= 0 Then Checker_DownloadPlan(i)
                    Thread.Sleep(500)
                Catch dex As ArgumentOutOfRangeException When disposedValue Or _StopRequested
                Catch ex As Exception
                    ErrorsDescriber.Execute(EDP.SendToLog, ex, "[Scheduler.Checker]")
                End Try
            Loop While Not _StopRequested
            _StopRequested = False
        End Sub
        Private Sub Checker_DownloadPlan(ByVal PlanIndex As Integer)
            While Downloader.Working : Thread.Sleep(200) : End While
            With Plans(PlanIndex)
                If .Downloading Then
                    PlansWaiter.Invoke(PlanDownloading)
                ElseIf .DownloadReady Then
                    .Download()
                    If Settings.AutomationScript.Use AndAlso Not Settings.AutomationScript.Value.IsEmptyString AndAlso
                       (Not .IsManual Or Not Settings.AutomationScript_ExcludeManual) Then ExecuteCommand(Settings.AutomationScript)
                End If
            End With
        End Sub
        Private Function Checker_GetNextPlanIndex() As Integer
            Try
                Dim result% = -1
                Dim l As New List(Of KeyValuePair(Of Integer, Date))
                Dim d As Date?
                If Count > 0 Then
                    For i% = 0 To Count - 1
                        With Plans(i)
                            If .DownloadReady Then
                                d = .NextDate
                                If d.HasValue Then l.Add(New KeyValuePair(Of Integer, Date)(i, d.Value))
                            End If
                        End With
                    Next
                End If
                If l.Count > 0 Then
                    Dim md As Date = l.Min(Function(p) p.Value)
                    result = l.Find(Function(p) p.Value = md).Key
                    l.Clear()
                End If
                Return result
            Catch
                Return -1
            End Try
        End Function
        Friend Property Pause(Optional ByVal LimitDate As Date? = Nothing) As PauseModes
            Get
                If Count > 0 Then Return Plans.FirstOrDefault(Function(p) p.Pause >= PauseModes.Disabled).Pause Else Return PauseModes.Disabled
            End Get
            Set(ByVal p As PauseModes)
                If Count > 0 Then Plans.ForEach(Sub(pp) pp.Pause(LimitDate) = p)
            End Set
        End Property
#End Region
#Region "IEnumerable Support"
        Private Function GetEnumerator() As IEnumerator(Of AutoDownloader) Implements IEnumerable(Of AutoDownloader).GetEnumerator
            Return New MyEnumerator(Of AutoDownloader)(Me)
        End Function
        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
#End Region
#Region "IDisposable Support"
        Private disposedValue As Boolean = False
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    [Stop]()
                    If Plans.Exists(PlanDownloading) Then Task.WaitAll(Task.Run(Sub() PlansWaiter(PlanDownloading)))
                    While Working : Thread.Sleep(200) : End While
                    If _UpdateRequired Then Update()
                    Plans.ListClearDispose
                End If
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
End Namespace