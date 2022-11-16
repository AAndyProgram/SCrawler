' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API
Imports SCrawler.API.Base
Imports PersonalUtilities.Tools.Notifications
Imports NotifyObj = SCrawler.SettingsCLS.NotificationObjects
Friend Class MainFrameObjects
    Friend ReadOnly Property MF As MainFrame
    Private WithEvents Notificator As NotificationsManager
    Friend ReadOnly Property PauseButtons As DownloadObjects.AutoDownloaderPauseButtons
    Friend Sub New(ByRef f As MainFrame)
        MF = f
        Notificator = New NotificationsManager
        PauseButtons = New DownloadObjects.AutoDownloaderPauseButtons(DownloadObjects.AutoDownloaderPauseButtons.ButtonsPlace.MainFrame)
    End Sub
#Region "Users"
    Friend Sub FocusUser(ByVal Key As String, Optional ByVal ActivateForm As Boolean = False)
        MF.FocusUser(Key, ActivateForm)
    End Sub
#End Region
#Region "Image handlers"
    Friend Sub ImageHandler(ByVal User As IUserData)
        ImageHandler(User, False)
        ImageHandler(User, True)
    End Sub
    Friend Sub ImageHandler(ByVal User As IUserData, ByVal Add As Boolean)
        Try
            If Add Then
                AddHandler User.UserUpdated, AddressOf MF.User_OnUserUpdated
            Else
                RemoveHandler User.UserUpdated, AddressOf MF.User_OnUserUpdated
            End If
        Catch
        End Try
    End Sub
    Friend Sub CollectionHandler(ByVal [Collection] As UserDataBind)
        Try
            AddHandler Collection.OnCollectionSelfRemoved, AddressOf MF.CollectionRemoved
            AddHandler Collection.OnUserRemoved, AddressOf MF.UserRemovedFromCollection
        Catch
        End Try
    End Sub
#End Region
#Region "Form functions"
    Friend Sub Focus(Optional ByVal Show As Boolean = False)
        ControlInvokeFast(MF, Sub()
                                  If Not MF.Visible And Show Then MF.Show()
                                  If MF.Visible Then MF.BringToFront() : MF.Activate()
                              End Sub)
    End Sub
    Friend Sub ChangeCloseVisible()
        ControlInvokeFast(MF.TRAY_CONTEXT, Sub() MF.BTT_TRAY_CLOSE_NO_SCRIPT.Visible =
                                                 Settings.ClosingCommand.Attribute And Not Settings.ClosingCommand.IsEmptyString)
    End Sub
    Friend Sub UpdateLogButton()
        MyMainLOG_UpdateLogButton(MF.BTT_LOG, MF.Toolbar_TOP)
    End Sub
#End Region
#Region "Notifications"
    Private Const NotificationInternalKey As String = "NotificationInternalKey"
    Friend Sub ShowNotification(ByVal Sender As NotifyObj, ByVal Message As String)
        If Settings.ProcessNotification(Sender) Then
            Using n As New Notification(Message) With {.Key = $"{NotificationInternalKey}_{Sender}"} : n.Show() : End Using
        End If
    End Sub
    Friend Sub ClearNotifications()
        Notificator.Clear()
    End Sub
    Private Sub Notificator_OnClicked(ByVal Key As String) Handles Notificator.OnClicked
        If Not Key.IsEmptyString Then
            If Key.StartsWith(NotificationInternalKey) Then
                Select Case Key
                    Case $"{NotificationInternalKey}_{NotifyObj.Channels}" : MF.MyChannels.FormShowS()
                    Case $"{NotificationInternalKey}_{NotifyObj.SavedPosts}" : MF.MySavedPosts.FormShowS()
                    Case Else : Focus(True)
                End Select
            ElseIf Settings.Automation Is Nothing OrElse Not Settings.Automation.NotificationClicked(Key) Then
                Focus(True)
            Else
                Focus(True)
            End If
        End If
    End Sub
#End Region
End Class