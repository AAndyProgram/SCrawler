' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Namespace API.Mastodon
    Friend Structure Credentials : Implements IEContainerProvider
        Private Const Name_Bearer As String = "Bearer"
        Private Const Name_Csrf As String = "Csrf"
        Friend Domain As String
        Friend Bearer As String
        Friend Csrf As String
        Friend ReadOnly Property Exists As Boolean
            Get
                Return Not Domain.IsEmptyString And Not Bearer.IsEmptyString And Not Csrf.IsEmptyString
            End Get
        End Property
        Private Sub New(ByVal e As EContainer)
            Domain = e.Value
            Bearer = e.Attribute(Name_Bearer)
            Csrf = e.Attribute(Name_Csrf)
        End Sub
        Public Shared Widening Operator CType(ByVal e As EContainer) As Credentials
            Return New Credentials(e)
        End Operator
        Public Shared Widening Operator CType(ByVal Domain As String) As Credentials
            Return New Credentials With {.Domain = Domain}
        End Operator
        Private Function ToEContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
            Return New EContainer("Item", Domain, {New EAttribute(Name_Bearer, Bearer), New EAttribute(Name_Csrf, Csrf)})
        End Function
        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            If Not IsNothing(Obj) Then
                If TypeOf Obj Is Credentials Then
                    Return Domain = DirectCast(Obj, Credentials).Domain
                Else
                    Return Domain = CStr(Obj)
                End If
            End If
            Return False
        End Function
    End Structure
End Namespace