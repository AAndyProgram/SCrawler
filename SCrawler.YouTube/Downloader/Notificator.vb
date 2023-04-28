' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Tools.Notifications
Namespace DownloadObjects.STDownloader
    Public Interface INotificator
        Sub Clear()
        Sub ShowNotification(ByVal Text As String, ByVal Image As SFile)
    End Interface
    Friend Class YTNotificator : Implements INotificator
        Private WithEvents Notificator As NotificationsManager
        Private ReadOnly Property SourceForm As Form
        Friend Sub New(ByRef Source As Form)
            Notificator = New NotificationsManager
            SourceForm = Source
        End Sub
        Friend Sub Clear() Implements INotificator.Clear
            Notificator.Clear()
        End Sub
        Friend Sub ShowNotification(ByVal Text As String, ByVal Image As SFile) Implements INotificator.ShowNotification
            If MyDownloaderSettings.ShowNotifications Then Notification.ShowNotification(Text,,, Image)
        End Sub
        Private Sub Notificator_OnClicked(ByVal Key As String) Handles Notificator.OnClicked
            SourceForm.FormShowS
        End Sub
    End Class
End Namespace