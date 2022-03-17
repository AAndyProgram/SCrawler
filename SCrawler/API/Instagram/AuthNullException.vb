' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports Sections = SCrawler.API.Instagram.UserData.Sections
Namespace API.Instagram
    Friend Class AuthNullException : Inherits ArgumentNullException
        Public Overrides ReadOnly Property ParamName As String
        Public Overrides ReadOnly Property Message As String
        Friend Sub New(ByVal s As Sections, ByVal IsSavedPosts As Boolean)
            If IsSavedPosts Then
                ParamName = "HashSavedPosts"
            ElseIf s = Sections.Timeline Then
                ParamName = "Hash"
            Else
                ParamName = "IG_APP_ID, IG_WWW_CLAIM"
            End If
            Message = $"Instagram auth for [{s}] is not set"
        End Sub
        Friend Shared Sub ThrowIfNull(ByVal s As Sections, ByVal IsSavedPosts As Boolean, ByVal Host As SiteSettings)
            Dim b As Boolean = False
            If IsSavedPosts Then
                If Not ACheck(Host.HashSavedPosts.Value) Then b = True
            ElseIf s = Sections.Timeline Then
                If Not ACheck(Host.Hash.Value) Then Host.HashUpdateRequired.Value = True : b = True
            Else
                If Not Host.StoriesAndTaggedReady Then b = True
            End If
            If b Then Throw New AuthNullException(s, IsSavedPosts)
        End Sub
    End Class
End Namespace