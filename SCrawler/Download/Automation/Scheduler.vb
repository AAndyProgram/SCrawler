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
        Friend Event PauseChanged As AutoDownloader.PauseChangedEventHandler
        Private Sub OnPauseChanged(ByVal Value As PauseModes)
            RaiseEvent PauseChanged(Pause)
        End Sub
        Private ReadOnly Plans As List(Of AutoDownloader)
        Friend Const FileNameDefault As String = "AutoDownload"
        Friend ReadOnly FileDefault As SFile = $"{SettingsFolderName}\{FileNameDefault}.xml"
        Friend File As SFile = Nothing
        Private ReadOnly PlanWorking As Predicate(Of AutoDownloader) = Function(Plan) Plan.Working
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
            Plans.Add(Plan)
            Plans.ListReindex
            Update()
        End Sub
        Friend Async Function RemoveAt(ByVal Index As Integer) As Task
            If Index.ValueBetween(0, Count - 1) Then
                With Plans(Index)
                    .Stop()
                    If .Working Then
                        Await Task.Run(Sub()
                                           While .Working : Thread.Sleep(510) : End While
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
            If Plans.Count > 0 Then
                If Not Plans.Exists(PlanWorking) Then
                    Pause = PauseModes.Unlimited
                    If Plans.Exists(PlanWorking) Then
                        MsgBoxE({$"Some plans are already being worked.{vbCr}Wait for the plans to complete their work and try again.",
                                 "Change scheduler"}, vbCritical)
                        Pause = PauseModes.Unlimited
                        Return False
                    End If
                End If
                [Stop]()
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
                If Plans.Count > 0 Then Plans.ForEach(Sub(ByVal p As AutoDownloader)
                                                          p.Source = Me
                                                          If Not IsInit Then p.Pause = PauseModes.Unlimited
                                                          AddHandler p.PauseChanged, AddressOf OnPauseChanged
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
        Friend Async Function Start(ByVal Init As Boolean) As Task
            Try
                Await Task.Run(Sub()
                                   Dim r% = 0
                                   Do
                                       r += 1
                                       Try
                                           If Count > 0 Then
                                               If Plans.Exists(PlanDownloading) Then PlansWaiter(PlanDownloading)
                                               For Each Plan In Plans
                                                   Plan.Start(Init)
                                                   PlansWaiter(PlanDownloading)
                                                   Thread.Sleep(1000)
                                               Next
                                           End If
                                           Exit Do
                                       Catch io_ex As InvalidOperationException 'Collection was modified; enumeration operation may not execute
                                       End Try
                                   Loop While r < 10
                               End Sub)
            Catch ex As Exception
                If Init Then
                    ErrorsDescriber.Execute(EDP.SendToLog, ex, "Start automation")
                    MainFrameObj.UpdateLogButton()
                Else
                    Throw ex
                End If
            End Try
        End Function
        Friend Sub [Stop]()
            If Count > 0 Then Plans.ForEach(Sub(p) p.Stop())
        End Sub
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
                    If Plans.Exists(PlanWorking) Then Task.WaitAll(Task.Run(Sub() PlansWaiter(PlanWorking)))
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