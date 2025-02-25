' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin.Attributes
Imports DN = SCrawler.API.Base.DeclaredNames
Namespace API.Base
    Friend Class EditorExchangeOptionsBase
        Friend Overridable Property SiteKey As String
        <PSetting(Address:=SettingAddress.User, Caption:=DN.UserNameChangeCaption, ToolTip:=DN.UserNameChangeToolTip)>
        Friend Overridable Property UserName As String = String.Empty
        Friend Sub New(ByVal u As UserDataBase)
            UserName = u.NameTrue(True)
        End Sub
        Friend Sub New()
        End Sub
    End Class
End Namespace