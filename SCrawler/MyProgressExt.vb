' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms.Toolbars
Friend Class PreProgress : Implements IDisposable
    Private ReadOnly Progress As MyProgressExt = Nothing
    Private ReadOnly ProgressExists As Boolean = False
    Private ReadOnly Property Ready As Boolean
        Get
            Return ProgressExists And Not disposedValue
        End Get
    End Property
    Friend Sub New(ByVal PR As MyProgress)
        If Not PR Is Nothing AndAlso TypeOf PR Is MyProgressExt Then
            Progress = PR
            ProgressExists = True
        End If
    End Sub
    Private _Maximum As Integer = 0
    Friend Sub ChangeMax(ByVal Value As Integer, Optional ByVal Add As Boolean = True)
        If Ready Then
            If Add Then
                _Maximum += Value
                If Value > 0 Then Progress.Maximum0 += Value
            Else
                _Maximum = Value
                Progress.Maximum0 = Value
            End If
        End If
    End Sub
    Private CumulVal As Integer = 0
    Friend Sub Perform(Optional ByVal Value As Integer = 1)
        If Ready Then
            CumulVal += Value
            Progress.Perform0(Value)
        End If
    End Sub
    Friend Sub Reset()
        _Maximum = 0
        CumulVal = 0
    End Sub
    Friend Sub Done()
        If Ready Then
            Dim v# = _Maximum - CumulVal
            If v > 0 Then
                With Progress
                    If v + .Value0 > .Maximum0 Then v = .Maximum0 - .Value0
                    If v < 0 Then v = 0
                    .Perform0(v)
                    Reset()
                End With
            End If
        End If
    End Sub
#Region "IDisposable Support"
    Private disposedValue As Boolean = False
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If Not disposedValue Then
            If disposing Then Done()
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
Friend Class MyProgressExt : Inherits MyProgress
    Private ReadOnly _Progress0ChangedEventHandlers As List(Of EventHandler(Of ProgressEventArgs))
    Friend Custom Event Progress0Changed As EventHandler(Of ProgressEventArgs)
        AddHandler(ByVal h As EventHandler(Of ProgressEventArgs))
            If Not _Progress0ChangedEventHandlers.Contains(h) Then _Progress0ChangedEventHandlers.Add(h)
        End AddHandler
        RemoveHandler(ByVal h As EventHandler(Of ProgressEventArgs))
            _Progress0ChangedEventHandlers.Remove(h)
        End RemoveHandler
        RaiseEvent(ByVal Sender As Object, ByVal e As ProgressEventArgs)
            If _Progress0ChangedEventHandlers.Count > 0 Then
                Try
                    For i% = 0 To _Progress0ChangedEventHandlers.Count - 1
                        Try : _Progress0ChangedEventHandlers(i).Invoke(Sender, e) : Catch : End Try
                    Next
                Catch
                End Try
            End If
        End RaiseEvent
    End Event
    Private ReadOnly _Maximum0ChangedEventHandlers As List(Of EventHandler(Of ProgressEventArgs))
    Friend Custom Event Maximum0Changed As EventHandler(Of ProgressEventArgs)
        AddHandler(ByVal h As EventHandler(Of ProgressEventArgs))
            If Not _Maximum0ChangedEventHandlers.Contains(h) Then _Maximum0ChangedEventHandlers.Add(h)
        End AddHandler
        RemoveHandler(ByVal h As EventHandler(Of ProgressEventArgs))
            _Maximum0ChangedEventHandlers.Remove(h)
        End RemoveHandler
        RaiseEvent(ByVal Sender As Object, ByVal e As ProgressEventArgs)
            If _Maximum0ChangedEventHandlers.Count > 0 Then
                Try
                    For i% = 0 To _Maximum0ChangedEventHandlers.Count - 1
                        Try : _Maximum0ChangedEventHandlers(i).Invoke(Sender, e) : Catch : End Try
                    Next
                Catch
                End Try
            End If
        End RaiseEvent
    End Event
    Friend Sub New()
        _Progress0ChangedEventHandlers = New List(Of EventHandler(Of ProgressEventArgs))
        _Maximum0ChangedEventHandlers = New List(Of EventHandler(Of ProgressEventArgs))
    End Sub
    Friend Sub New(ByRef StatusStrip As StatusStrip, ByRef ProgressBar As ToolStripProgressBar, ByRef Label As ToolStripStatusLabel,
                   Optional ByVal Information As String = Nothing)
        MyBase.New(StatusStrip, ProgressBar, Label, Information)
        _Progress0ChangedEventHandlers = New List(Of EventHandler(Of ProgressEventArgs))
        _Maximum0ChangedEventHandlers = New List(Of EventHandler(Of ProgressEventArgs))
    End Sub
    Friend Sub New(ByRef ProgressBar As ProgressBar, ByRef Label As Label, Optional ByVal Information As String = Nothing)
        MyBase.New(ProgressBar, Label, Information)
        _Progress0ChangedEventHandlers = New List(Of EventHandler(Of ProgressEventArgs))
        _Maximum0ChangedEventHandlers = New List(Of EventHandler(Of ProgressEventArgs))
    End Sub
    Private _Maximum0 As Double = 0
    Friend Property Maximum0 As Double
        Get
            Return _Maximum0
        End Get
        Set(ByVal v As Double)
            Dim b As Boolean = Not _Maximum0 = v
            _Maximum0 = v
            If ResetProgressOnMaximumChanges Then Value0 = 0
            If b Then RaiseEvent Maximum0Changed(Me, Nothing)
        End Set
    End Property
    Friend Property Value0 As Double = 0
    Friend Sub Perform0(Optional ByVal Value As Double = 1)
        Value0 += Value
        If Perform(0, 10, False, False) Then RaiseEvent Progress0Changed(Me, Nothing)
    End Sub
    Public Overloads Overrides Sub Perform(Optional ByVal Value As Double = 1)
        If Perform(Value, PerformMod, True, True) Then OnProgressChanged()
    End Sub
    Public Overloads Function Perform(ByVal Value As Double, ByVal pm As Integer, ByVal SetText As Boolean, ByVal InvokeProgressChangeHandler As Boolean) As Boolean
        Me.Value += Value
        If Me.Value < 0 Then Me.Value = 0
        Dim v# = Me.Value + Value0
        Dim m# = Maximum + Maximum0
        If pm = 0 OrElse (v Mod pm) = 0 OrElse v = m Then PerformImpl(GetPercentage(v, m), SetText, InvokeProgressChangeHandler) : Return True
        Return False
    End Function
    Public Overrides Sub Done()
        Value0 = Maximum0
        MyBase.Done()
    End Sub
    Public Overrides Sub Reset()
        MyBase.Reset()
        Value0 = 0
        Maximum0 = 0
    End Sub
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If Not disposedValue And disposing Then
            _Progress0ChangedEventHandlers.Clear()
            _Maximum0ChangedEventHandlers.Clear()
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class