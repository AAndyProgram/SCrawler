' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports SCrawler.Plugin.Attributes
Namespace API.Xhamster
    Friend NotInheritable Class UserExchangeOptions : Inherits API.Base.EditorExchangeOptionsBase_P
        <PSetting(Address:=SettingAddress.User, Caption:="Get moments")>
        Friend Property GetMoments As Boolean = False
        Friend Sub New()
            MyBase.New
        End Sub
        Friend Sub New(ByVal u As IPSite)
            MyBase.New(DirectCast(u, UserData))
            GetMoments = DirectCast(u, UserData).GetMoments
        End Sub
        Friend Overrides Sub Apply(ByRef u As IPSite)
            MyBase.Apply(u)
            DirectCast(u, UserData).GetMoments = GetMoments
        End Sub
    End Class
End Namespace