' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Tools
Imports SCrawler.DownloadObjects.Groups
Imports System.Threading
Namespace DownloadObjects
    Friend Class Scheduler : Implements IEnumerable(Of AutoDownloader), IMyEnumerator(Of AutoDownloader), IDisposable
        Friend Const Name_Plan As String = "Plan"
        Private ReadOnly Plans As List(Of AutoDownloader)
        Private ReadOnly File As SFile = $"Settings\AutoDownload.xml"
        Private ReadOnly PlanWorking As Predicate(Of AutoDownloader) = Function(Plan) Plan.Working
        Private ReadOnly PlanDownloading As Predicate(Of AutoDownloader) = Function(Plan) Plan.Downloading
        Private ReadOnly PlansWaiter As Action(Of Predicate(Of AutoDownloader)) = Sub(ByVal Predicate As Predicate(Of AutoDownloader))
                                                                                      While Plans.Exists(Predicate) : Thread.Sleep(200) : End While
                                                                                  End Sub
        Friend Sub New()
            Plans = New List(Of AutoDownloader)
            If File.Exists Then
                Using x As New XmlFile(File,, False) With {.AllowSameNames = True}
                    x.LoadData()
                    If x.Contains(Name_Plan) Then
                        For Each e In x : Plans.Add(New AutoDownloader(e)) : Next
                    Else
                        Plans.Add(New AutoDownloader(x))
                    End If
                End Using
            End If
            If Plans.Count > 0 Then Plans.ForEach(Sub(p) p.Source = Me)
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
        Friend Function NotificationClicked(ByVal Key As String) As Boolean
            Return Count > 0 AndAlso Plans.Exists(Function(p) p.NotificationClicked(Key))
        End Function
        Friend Sub Add(ByVal Plan As AutoDownloader)
            Plan.Source = Me
            Plans.Add(Plan)
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
#Region "Groups Support"
        Friend Sub GROUPS_Updated(ByVal Sender As DownloadGroup)
            If Count > 0 Then Plans.ForEach(Sub(p) p.GROUPS_Updated(Sender))
        End Sub
        Friend Sub GROUPS_Deleted(ByVal Sender As DownloadGroup)
            If Count > 0 Then Plans.ForEach(Sub(p) p.GROUPS_Deleted(Sender))
        End Sub
#End Region
#Region "Execution"
        Friend Async Sub Start(ByVal Init As Boolean)
            If Count > 0 Then
                If Plans.Exists(PlanDownloading) Then Await Task.Run(Sub() PlansWaiter(PlanDownloading))
                For Each Plan In Plans : Plan.Start(Init) : Thread.Sleep(200) : Next
            End If
        End Sub
        Friend Sub [Stop]()
            If Count > 0 Then Plans.ForEach(Sub(p) p.Stop())
        End Sub
        Friend Property Pause As Boolean
            Get
                If Count > 0 Then Return Plans(0).Pause Else Return False
            End Get
            Set(ByVal p As Boolean)
                If Count > 0 Then Plans.ForEach(Sub(pp) pp.Pause = p)
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