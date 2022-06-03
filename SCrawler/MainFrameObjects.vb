' Copyright (C) 2022  Andy
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
Friend Class MainFrameObjects
    Private ReadOnly Property MF As MainFrame
    Private WithEvents Notificator As NotificationsManager
    Friend Sub New(ByRef f As MainFrame)
        MF = f
        Notificator = New NotificationsManager
    End Sub
    Friend Sub ImageHandler(ByVal User As IUserData)
        ImageHandler(User, False)
        ImageHandler(User, True)
    End Sub
    Friend Sub ImageHandler(ByVal User As IUserData, ByVal Add As Boolean)
        Try
            If Add Then
                AddHandler User.OnUserUpdated, AddressOf MF.User_OnUserUpdated
            Else
                RemoveHandler User.OnUserUpdated, AddressOf MF.User_OnUserUpdated
            End If
        Catch ex As Exception
        End Try
    End Sub
    Friend Sub CollectionHandler(ByVal [Collection] As UserDataBind)
        Try
            AddHandler Collection.OnCollectionSelfRemoved, AddressOf MF.CollectionRemoved
            AddHandler Collection.OnUserRemoved, AddressOf MF.UserRemovedFromCollection
        Catch ex As Exception
        End Try
    End Sub
    Friend Sub Focus(Optional ByVal Show As Boolean = False)
        If Not MF.Visible And Show Then MF.Show()
        If MF.Visible Then MF.BringToFront() : MF.Activate()
    End Sub
    Friend Sub ChangeCloseVisible()
        Dim a As Action = Sub() MF.BTT_TRAY_CLOSE_NO_SCRIPT.Visible = Settings.ClosingCommand.Attribute And Not Settings.ClosingCommand.IsEmptyString
        If MF.TRAY_CONTEXT.InvokeRequired Then MF.TRAY_CONTEXT.Invoke(a) Else a.Invoke
    End Sub
    Friend Overloads Sub ShowNotification(ByVal Message As String)
        MF.TrayIcon.ShowBalloonTip(2000, MF.TrayIcon.BalloonTipTitle, Message, ToolTipIcon.Info)
    End Sub
    Friend Overloads Sub ShowNotification(ByVal Message As String, ByVal Title As String)
        MF.TrayIcon.ShowBalloonTip(2000, Title, Message, ToolTipIcon.Info)
    End Sub
    Friend Overloads Sub ShowNotification(ByVal Message As String, ByVal Title As String, ByVal Icon As ToolTipIcon)
        MF.TrayIcon.ShowBalloonTip(2000, Title, Message, Icon)
    End Sub
    Friend Sub CLearNotifications()
        Notificator.Clear()
    End Sub
    Private Sub Notificator_OnClicked(ByVal Key As String) Handles Notificator.OnClicked
        If Settings.Automation Is Nothing OrElse Not Settings.Automation.NotificationClicked(Key) Then
            If Not MF.Visible Then MF.Show()
            Focus()
        End If
    End Sub
End Class