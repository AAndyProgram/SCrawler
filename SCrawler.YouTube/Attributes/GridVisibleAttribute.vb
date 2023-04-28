' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.YouTube.Attributes
    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=False, Inherited:=False)>
    Public Class GridVisibleAttribute : Inherits Attribute
        Private ReadOnly NonAppMode As Boolean = True
        Public Sub New()
        End Sub
        Public Sub New(ByVal NonAppMode As Boolean)
            Me.NonAppMode = NonAppMode
        End Sub
        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            If Not Obj Is Nothing AndAlso TypeOf Obj Is GridVisibleAttribute Then
                If NonAppMode Then
                    Return DirectCast(Obj, GridVisibleAttribute).NonAppMode
                Else
                    Return True
                End If
            Else
                Return False
            End If
        End Function
    End Class
End Namespace