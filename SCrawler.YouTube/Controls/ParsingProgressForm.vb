' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports PersonalUtilities.Forms.Toolbars
Namespace API.YouTube.Controls
    Public Class ParsingProgressForm
        Public ReadOnly Property MyProgress As MyProgress
        Private ReadOnly TokenSource As CancellationTokenSource
        Public ReadOnly Property Token As CancellationToken
            Get
                Return TokenSource.Token
            End Get
        End Property
        Private ReadOnly CountMax As Integer
        Private CountCurrent As Integer = 1
        Friend Sub NextPlaylist()
            CountCurrent += 1
            MyProgress.InformationTemporary(True) = InfoStr
            MyProgress.Information = InfoStr
        End Sub
        Private ReadOnly Property InfoStr As String
            Get
                Const MainMsg$ = "Data parsing in progress"
                If CountMax > 1 Then
                    Return $"{MainMsg} [{CountCurrent - 1}/{CountMax}]"
                Else
                    Return MainMsg
                End If
            End Get
        End Property
        Public Sub New(Optional ByVal _Count As Integer = 1)
            InitializeComponent()
            CountMax = _Count
            MyProgress = New MyProgress(PR_MAIN, LBL_MAIN, InfoStr) With {.ResetProgressOnMaximumChanges = False}
            TokenSource = New CancellationTokenSource
        End Sub
        Public Sub SetInitialValues(ByVal Count As Integer, ByVal Info As String)
            With MyProgress
                .Maximum = Count
                .Visible = True
                .Perform(0.5)
                .Value = 0
                .InformationTemporary = Info
            End With
        End Sub
        Private _KeyDownDisabled As Boolean = False
        Private Sub ParsingProgressForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            If e.KeyCode = Keys.Escape AndAlso Not _KeyDownDisabled AndAlso MsgBoxE({"Data parsing in progress." & vbCr &
                                                                                     "Are you sure you want to stop parsing and cancel the operation?",
                                                                                     "Stop parsing"}, vbExclamation + vbYesNo) = vbYes Then
                _KeyDownDisabled = True
                TokenSource.Cancel()
            End If
        End Sub
        Private Sub ParsingProgressForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            MyProgress.Dispose()
            TokenSource.Dispose()
        End Sub
    End Class
End Namespace