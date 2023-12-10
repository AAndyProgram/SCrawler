' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports SCrawler.API.YouTube
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls.KeyClick
Public Class MainFrame
    Private WithEvents MyActivator As FormActivator
    Public Sub New()
        InitializeComponent()
        AppMode = True
        MyActivator = New FormActivator(Me, TRAY_ICON)
    End Sub
    Protected Overrides Sub VideoListForm_Load(sender As Object, e As EventArgs)
        MyBase.VideoListForm_Load(sender, e)
        TRAY_ICON.Visible = MyYouTubeSettings.CloseToTray
        CheckNewReleaseFolder()
        CheckVersionImpl(False)
    End Sub
    Private _CloseInvoked As Boolean = False
    Private _IgnoreTrayOptions As Boolean = False
    Private _IgnoreCloseConfirm As Boolean = False
    Protected Overrides Async Sub VideoListForm_Closing(sender As Object, e As CancelEventArgs)
        If MyYouTubeSettings.CloseToTray And Not _IgnoreTrayOptions Then
            e.Cancel = True
            Hide()
        Else
            If CheckForClose(_IgnoreCloseConfirm) Then
                If _CloseInvoked Then GoTo CloseContinue
                If MyJob.Working Then
                    If MsgBoxE({"The program is still downloading something..." & vbNewLine &
                                "Are you sure you want to stop downloading and exit the program?",
                                "Downloading in progress"},
                               MsgBoxStyle.Exclamation,,,
                               {"Stop downloading and close", "Cancel"}) = 0 Then
                        _CloseInvoked = True
                        e.Cancel = True
                        MyJob.Cancel()
                        Await Task.Run(Sub()
                                           While MyJob.Working : Threading.Thread.Sleep(500) : End While
                                       End Sub)
                    End If
                End If
            Else
                GoTo DropCloseParams
            End If
            GoTo CloseContinue
DropCloseParams:
            e.Cancel = True
            _IgnoreTrayOptions = False
            _IgnoreCloseConfirm = False
            _CloseInvoked = False
            Exit Sub
CloseContinue:
            If _CloseInvoked Then Close()
CloseResume:
        End If
    End Sub
    Protected Overrides Sub VideoListForm_Disposed(sender As Object, e As EventArgs)
        MyActivator.Dispose()
        MyBase.VideoListForm_Disposed(sender, e)
    End Sub
    Private Sub BTT_TRAY_CLOSE_Click(sender As Object, e As EventArgs) Handles BTT_TRAY_CLOSE.Click
        If CheckForClose(False) Then _IgnoreCloseConfirm = True : _IgnoreTrayOptions = True : Close()
    End Sub
    Private Sub MyActivator_TrayIconClick(ByVal Sender As Object, ByVal e As KeyClickEventArgs) Handles MyActivator.TrayIconClick
        If e.MouseButton = MouseButtons.Left And e.Control Then
            BTT_ADD_KeyClick(Nothing, New KeyClickEventArgs)
            e.Handled = Not MyYouTubeSettings.ShowFormDownTrayClick
        End If
    End Sub
    Private Function CheckForClose(ByVal _Ignore As Boolean) As Boolean
        If MyYouTubeSettings.ExitConfirm And Not _Ignore Then
            Return MsgBoxE({"Do you want to close the program?", "Closing the program"}, MsgBoxStyle.YesNo) = MsgBoxResult.Yes
        Else
            Return True
        End If
    End Function
    Protected Overrides Sub BTT_SETTINGS_Click(sender As Object, e As EventArgs)
        MyBase.BTT_SETTINGS_Click(sender, e)
        TRAY_ICON.Visible = MyYouTubeSettings.CloseToTray
    End Sub
    Protected Overrides Sub BTT_ADD_KeyClick(ByVal Sender As ToolStripMenuItemKeyClick, ByVal e As KeyClickEventArgs) Handles CONTEXT_BTT_ADD.KeyClick
        MyBase.BTT_ADD_KeyClick(Sender, e)
    End Sub
    Protected Overrides Sub MyJob_Started(ByVal Sender As Object, ByVal e As EventArgs)
        TRAY_ICON.Icon = My.Resources.ArrowDownIcon_Orange_24
    End Sub
    Protected Overrides Sub MyJob_Finished(ByVal Sender As Object, ByVal e As EventArgs)
        TRAY_ICON.Icon = My.Resources.SiteYouTube.YouTubeIcon_32
        MyBase.MyJob_Finished(Sender, e)
    End Sub
End Class