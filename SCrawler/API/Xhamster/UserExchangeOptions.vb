' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin.Attributes
Namespace API.Xhamster
    Friend Class UserExchangeOptions
        <PSetting(Address:=SettingAddress.User, Caption:="Query",
                  ToolTip:="Query string. Don't change this field when creating a user! Change it only for the same request.")>
        Friend Property QueryString As String
        Friend Sub New()
        End Sub
        Friend Sub New(ByVal u As UserData)
            QueryString = u.QueryString
        End Sub
    End Class
End Namespace