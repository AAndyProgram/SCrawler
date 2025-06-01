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
        <PSetting(Address:=SettingAddress.User, Caption:=DN.DownloadTextCaption, ToolTip:=DN.DownloadTextTip)>
        Friend Overridable Property DownloadText As Boolean = False
        <PSetting(Address:=SettingAddress.User, Caption:=DN.DownloadTextPostsCaption, ToolTip:=DN.DownloadTextPostsTip)>
        Friend Overridable Property DownloadTextPosts As Boolean = False
        <PSetting(Address:=SettingAddress.User, Caption:=DN.DownloadTextSpecialFolderCaption, ToolTip:=DN.DownloadTextSpecialFolderTip)>
        Friend Overridable Property DownloadTextSpecialFolder As Boolean = False
        Friend Sub New(ByVal u As UserDataBase)
            UserName = u.NameTrue(True)
            DownloadText = u.DownloadText
            DownloadTextPosts = u.DownloadTextPosts
            DownloadTextSpecialFolder = u.DownloadTextSpecialFolder
        End Sub
        Friend Sub New(ByVal s As SiteSettingsBase)
            DownloadText = s.DownloadText.Value
            DownloadTextPosts = s.DownloadTextPosts.Value
            DownloadTextSpecialFolder = s.DownloadTextSpecialFolder.Value
        End Sub
        Friend Sub New()
        End Sub
        Protected _ApplyBase_Name As Boolean = True
        Protected _ApplyBase_Text As Boolean = True
        Friend Sub ApplyBase(ByRef u As UserDataBase)
            If _ApplyBase_Name Then u.NameTrue = UserName
            If _ApplyBase_Text Then
                u.DownloadText = DownloadText
                u.DownloadTextPosts = DownloadTextPosts
                u.DownloadTextSpecialFolder = DownloadTextSpecialFolder
            End If
        End Sub
    End Class
End Namespace