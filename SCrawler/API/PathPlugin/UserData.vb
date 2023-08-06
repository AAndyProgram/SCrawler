' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Namespace API.PathPlugin
    Friend Class UserData : Inherits UserDataBase
        Private Const DOWNLOAD_ERROR As String = "The path plugin only provides user paths."
        Friend Overrides Property UserExists As Boolean
            Get
                Return DownloadContentDefault_GetRootDir.CSFileP.Exists(SFO.Path, False)
            End Get
            Set(ByVal e As Boolean)
                MyBase.UserExists = e
            End Set
        End Property
        Friend Overrides Property UserSuspended As Boolean
            Get
                Return False
            End Get
            Set(ByVal s As Boolean)
                MyBase.UserSuspended = s
            End Set
        End Property
        Friend Overrides Sub OpenSite(Optional ByVal e As ErrorsDescriber = Nothing)
            OpenFolder()
        End Sub
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XML.XmlFile, ByVal Loading As Boolean)
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As Threading.CancellationToken)
            Throw New InvalidOperationException(DOWNLOAD_ERROR)
        End Sub
        Protected Overrides Sub DownloadContent(ByVal Token As Threading.CancellationToken)
            Throw New InvalidOperationException(DOWNLOAD_ERROR)
        End Sub
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            Throw New InvalidOperationException(DOWNLOAD_ERROR)
        End Function
    End Class
End Namespace