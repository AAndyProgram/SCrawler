' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace Plugin
    Public Structure ExchangeOptions
        Public UserName As String
        Public SiteName As String
        Public HostKey As String
        Public IsChannel As Boolean
        Public Exists As Boolean
        Public Sub New(ByVal Site As String, ByVal Name As String)
            UserName = Name
            SiteName = Site
        End Sub
        Public Sub New(ByVal Site As String, ByVal Name As String, ByVal IsChannel As Boolean)
            Me.New(Site, Name)
            Me.IsChannel = IsChannel
        End Sub
    End Structure
End Namespace