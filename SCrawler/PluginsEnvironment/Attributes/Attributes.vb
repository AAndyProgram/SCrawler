' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Runtime.CompilerServices
Namespace Plugin.Attributes
    Public Enum SettingAddress : Both : Settings : User : None : End Enum
    Public Class PSettingAttribute : Inherits Attribute
        Public Number As Integer = 0
        Friend ReadOnly Name As String
        Friend ReadOnly NameAssoc As String
        Friend ReadOnly NameAssocInstance As String
        Public ToolTip As String
        Public Caption As String
        Public ThreeState As Boolean = False
        Public AllowNull As Boolean = True
        Public Provider As Type
        Private _LeftOffset As Integer? = Nothing
        Friend ReadOnly Property LeftOffsetGet As Integer?
            Get
                Return _LeftOffset
            End Get
        End Property
        Public WriteOnly Property LeftOffset As Integer
            Set(ByVal offset As Integer)
                _LeftOffset = offset
            End Set
        End Property
        Public MinimumWidth As Integer? = Nothing
        Public Address As SettingAddress = SettingAddress.Both
        Public Sub New(<CallerMemberName> Optional ByVal _Name As String = Nothing)
            Name = _Name
        End Sub
        Public Sub New(ByVal AssocPropName As String, ByVal AssocPropInstance As String, <CallerMemberName> Optional ByVal _Name As String = Nothing)
            NameAssoc = AssocPropName
            NameAssocInstance = AssocPropInstance
            Name = _Name
        End Sub
    End Class
    Public Class PClonableAttribute : Inherits Attribute
        Public Clone As Boolean = True
        Public Update As Boolean = True
    End Class
    Public Class HiddenControlAttribute : Inherits Attribute
        Public ReadOnly IsHidden As Boolean = True
        Public Sub New()
        End Sub
        Public Sub New(ByVal _IsHidden As Boolean)
            IsHidden = _IsHidden
        End Sub
    End Class
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=True)>
    Public Class CookieValueExtractorAttribute : Inherits Attribute
        Public ReadOnly PropertyName As String
        Public Sub New(ByVal _PropertyName As String)
            PropertyName = _PropertyName
        End Sub
    End Class
    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=False)>
    Public Class UseDownDetectorAttribute : Inherits Attribute
    End Class
End Namespace