' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.YouTube.Controls
    Friend Class TableControlsProcessor
        Private ReadOnly Property TP_CONTROLS As TableLayoutPanel
        Friend Sub New(ByRef TP As TableLayoutPanel)
            TP_CONTROLS = TP
        End Sub
        Private _LatestSelected As Integer = -1
        Friend Sub MediaItem_Click(ByVal Sender As Object, ByVal e As EventArgs)
            Try
                _LatestSelected = TP_CONTROLS.GetPositionFromControl(Sender).Row
                DirectCast(Sender, Control).Focus()
            Catch ex As Exception
                _LatestSelected = -1
            End Try
        End Sub
        Friend Sub MediaItem_KeyDown(ByVal Sender As Object, ByVal e As KeyEventArgs)
            Try
                If e.KeyCode = Keys.Down Or e.KeyCode = Keys.Up Then
                    Dim newPosition% = _LatestSelected + IIf(e.KeyCode = Keys.Down, 1, -1)
                    If newPosition < 0 Then newPosition = 0
                    If newPosition <> _LatestSelected Then
                        Dim cnt As DownloadObjects.STDownloader.MediaItem = TP_CONTROLS.GetControlFromPosition(0, newPosition)
                        If Not cnt Is Nothing Then cnt.PerformClick()
                    End If
                End If
            Catch
            End Try
        End Sub
    End Class
End Namespace