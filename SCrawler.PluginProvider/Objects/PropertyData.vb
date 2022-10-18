' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace Plugin
    Public Structure PropertyData
        Public ReadOnly Name As String
        Public ReadOnly Value As Object
        Public Sub New(ByVal Name As String, ByVal Value As Object)
            Me.Name = Name
            Me.Value = Value
        End Sub
    End Structure
End Namespace