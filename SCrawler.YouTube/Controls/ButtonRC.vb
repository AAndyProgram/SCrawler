' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.YouTube.Controls
    Public Class ButtonRC : Inherits Button
        Private Const WM_CONTEXTMENU As Integer = 123 '&H7B
        Private Const WM_CANCELMODE As Integer = 31 '&H1F
        Private Const WM_INITMENUPOPUP As Integer = 279 '&H117
        Private Const SMTO_NOTIMEOUTIFNOTHUNG As Integer = 8
        Protected Overrides Sub WndProc(ByRef m As Message)
            If m.Msg = WM_CONTEXTMENU Or m.Msg = WM_CANCELMODE Or m.Msg = WM_INITMENUPOPUP Or m.Msg = SMTO_NOTIMEOUTIFNOTHUNG Then
                m.Result = IntPtr.Zero
            Else
                MyBase.WndProc(m)
            End If
        End Sub
    End Class
End Namespace