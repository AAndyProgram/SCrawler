' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Tools.Web.Clients
Namespace EncryptCookies
    Friend Module EncryptFunction
        Friend CookiesEncrypted As Boolean = False
        Friend Sub ValidateCookiesEncrypt(ByRef Responser As Responser)
            If Not Responser Is Nothing Then
                With Responser
                    If .CookiesEncryptKey.IsEmptyString Then .CookiesEncryptKey = SettingsCLS.CookieEncryptKey : .SaveSettings()
                End With
            End If
        End Sub
    End Module
End Namespace