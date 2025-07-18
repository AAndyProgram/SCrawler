' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin.Attributes
Namespace API.Base
    Friend Interface IPSite
        Property QueryString As String
    End Interface
    Friend Class EditorExchangeOptionsBase_P : Inherits EditorExchangeOptionsBase : Implements IPSite
        <PSetting(Address:=SettingAddress.None)> Friend Overrides Property UserName As String
        <PSetting(Address:=SettingAddress.None)> Friend Overrides Property DownloadText As Boolean
        <PSetting(Address:=SettingAddress.None)> Friend Overrides Property DownloadTextPosts As Boolean
        <PSetting(Address:=SettingAddress.None)> Friend Overrides Property DownloadTextSpecialFolder As Boolean
        <PSetting(Address:=SettingAddress.User, Caption:="Query",
                  ToolTip:="Query string. Don't change this field when creating a user! Change it only for the same request.")>
        Friend Property QueryString As String Implements IPSite.QueryString
        Friend Sub New()
            DisableBase()
        End Sub
        Friend Sub New(ByVal u As UserDataBase)
            MyBase.New(u)
            DisableBase()
            If TypeOf u Is IPSite Then QueryString = DirectCast(u, IPSite).QueryString
        End Sub
        Friend Sub New(ByVal s As SiteSettingsBase)
            MyBase.New(s)
            DisableBase()
        End Sub
        Friend Overridable Sub Apply(ByRef u As IPSite)
            ApplyBase(u)
            u.QueryString = QueryString
        End Sub
        Protected Overridable Sub DisableBase()
            _ApplyBase_Name = False
            _ApplyBase_Text = False
        End Sub
    End Class
End Namespace