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
    Friend ReadOnly Progress As MyProgressExt = Nothing
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
    Friend Sub ChangeMax(ByVal Value As Integer, Optional ByVal Add As Boolean = True)
        If Ready Then
            If Add Then
                If Value > 0 Then Progress.Maximum0 += Value
            Else
                Progress.Maximum0 = Value
            End If
        End If
    End Sub
    Friend Sub Perform(Optional ByVal Value As Integer = 1)
        If Ready Then Progress.Perform0(Value)
    End Sub
    Friend Sub Reset()
        If Ready Then Progress.Reset0()
    End Sub
    Friend Sub Done()
        If Ready Then Progress.Done0()
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
            InvokeHandlers(_Progress0ChangedEventHandlers, Sender, e)
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
            InvokeHandlers(_Maximum0ChangedEventHandlers, Sender, e)
        End RaiseEvent
    End Event
    Private ReadOnly _Progress0CompletedEventHandlers As List(Of EventHandler(Of ProgressEventArgs))
    Friend Custom Event Progress0Completed As EventHandler(Of ProgressEventArgs)
        AddHandler(ByVal h As EventHandler(Of ProgressEventArgs))
            If Not _Progress0CompletedEventHandlers.Contains(h) Then _Progress0CompletedEventHandlers.Add(h)
        End AddHandler
        RemoveHandler(ByVal h As EventHandler(Of ProgressEventArgs))
            _Progress0CompletedEventHandlers.Remove(h)
        End RemoveHandler
        RaiseEvent(ByVal Sender As Object, ByVal e As ProgressEventArgs)
            InvokeHandlers(_Progress0CompletedEventHandlers, Sender, e)
        End RaiseEvent
    End Event
    Private WithEvents PR_PRE As MyProgress
    Private Sub PR_PRE_ProgressChanged(ByVal Sender As Object, ByVal e As ProgressEventArgs) Handles PR_PRE.ProgressChanged
        RaiseEvent Progress0Changed(Sender, e)
    End Sub
    Private Sub PR_PRE_MaximumChanged(ByVal Sender As Object, ByVal e As ProgressEventArgs) Handles PR_PRE.MaximumChanged
        RaiseEvent Maximum0Changed(Sender, e)
    End Sub
    Private Sub PR_PRE_ProgressCompleted(ByVal Sender As Object, ByVal e As ProgressEventArgs) Handles PR_PRE.ProgressCompleted
        RaiseEvent Progress0Completed(Sender, e)
    End Sub
    Friend Sub New()
        _Progress0ChangedEventHandlers = New List(Of EventHandler(Of ProgressEventArgs))
        _Maximum0ChangedEventHandlers = New List(Of EventHandler(Of ProgressEventArgs))
        _Progress0CompletedEventHandlers = New List(Of EventHandler(Of ProgressEventArgs))
    End Sub
    Friend Sub New(ByRef StatusStrip As StatusStrip, ByRef ProgressBar As ToolStripProgressBar, ByRef ProgressBarPre As ToolStripProgressBar, ByRef Label As ToolStripStatusLabel,
                   Optional ByVal Information As String = Nothing)
        MyBase.New(StatusStrip, ProgressBar, Label, Information)
        PR_PRE = New MyProgress(StatusStrip, ProgressBarPre, Nothing) With {.PerformMod = 10, .ResetProgressOnMaximumChanges = False}
        _Progress0ChangedEventHandlers = New List(Of EventHandler(Of ProgressEventArgs))
        _Maximum0ChangedEventHandlers = New List(Of EventHandler(Of ProgressEventArgs))
        _Progress0CompletedEventHandlers = New List(Of EventHandler(Of ProgressEventArgs))
    End Sub
    Friend Sub New(ByRef ProgressBar As ProgressBar, ByRef ProgressBarPre As ProgressBar, ByRef Label As Label, Optional ByVal Information As String = Nothing)
        MyBase.New(ProgressBar, Label, Information)
        PR_PRE = New MyProgress(ProgressBarPre, Nothing) With {.PerformMod = 10, .ResetProgressOnMaximumChanges = False}
        _Progress0ChangedEventHandlers = New List(Of EventHandler(Of ProgressEventArgs))
        _Maximum0ChangedEventHandlers = New List(Of EventHandler(Of ProgressEventArgs))
        _Progress0CompletedEventHandlers = New List(Of EventHandler(Of ProgressEventArgs))
    End Sub
    Friend Property Maximum0 As Double
        Get
            Return PR_PRE.Maximum
        End Get
        Set(ByVal v As Double)
            PR_PRE.Maximum = v
        End Set
    End Property
    Friend Property Value0 As Double
        Get
            Return PR_PRE.Value
        End Get
        Set(ByVal v As Double)
            PR_PRE.Value = v
        End Set
    End Property
    Friend Sub Perform0(Optional ByVal Value As Double = 1)
        PR_PRE.Perform(Value)
    End Sub
    Public Overrides Sub Done()
        PR_PRE.Done()
        MyBase.Done()
    End Sub
    Friend Sub Done0()
        PR_PRE.Done()
    End Sub
    Public Overrides Sub Reset()
        PR_PRE.Reset()
        MyBase.Reset()
    End Sub
    Friend Sub Reset0()
        PR_PRE.Reset()
    End Sub
    Public Overrides Property Visible(Optional ByVal ProgressBar As Boolean = True, Optional ByVal Label As Boolean = True) As Boolean
        Get
            Return MyBase.Visible(ProgressBar, Label)
        End Get
        Set(ByVal _Visible As Boolean)
            MyBase.Visible(ProgressBar, Label) = _Visible
            PR_PRE.Visible(ProgressBar, Label) = _Visible
        End Set
    End Property
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If Not disposedValue And disposing Then
            _Progress0ChangedEventHandlers.Clear()
            _Maximum0ChangedEventHandlers.Clear()
            _Progress0CompletedEventHandlers.Clear()
            PR_PRE.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub
End Class