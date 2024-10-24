' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.Base
    Friend NotInheritable Class DeclaredNames
        Friend Const Header_Authorization As String = "authorization"
        Friend Const Header_CSRFToken As String = "x-csrf-token"

        Friend Const CAT_UserDefs As String = "New user defaults"
        Friend Const CAT_Timers As String = "Timers"

        Friend Const ConcurrentDownloadsCaption As String = "Concurrent downloads"
        Friend Const ConcurrentDownloadsToolTip As String = "The number of concurrent downloads."
        Friend Const SavedPostsUserNameCaption As String = "Saved posts user"
        Friend Const SavedPostsUserNameToolTip As String = "Personal profile username"
        Friend Const GifsSpecialFolderCaption As String = "GIFs special folder"
        Friend Const GifsSpecialFolderToolTip As String = "Put the GIFs in a special folder" & vbCr &
                                                          "This is a folder name, not an absolute path." & vbCr &
                                                          "This folder(s) will be created relative to the user's root folder." & vbCr &
                                                          "Examples:" & vbCr & "SomeFolderName" & vbCr & "SomeFolderName\SomeFolderName2"
        Friend Const GifsPrefixCaption As String = "GIF prefix"
        Friend Const GifsPrefixToolTip As String = "This prefix will be added to the beginning of the filename"
        Friend Const GifsDownloadCaption As String = "Download GIFs"
        Friend Const UseMD5ComparisonCaption As String = "Use MD5 comparison"
        Friend Const UseMD5ComparisonToolTip As String = "Each image will be checked for existence using MD5"
        Friend Const UserNameChangeCaption As String = "UserName"
        Friend Const UserNameChangeToolTip As String = "If the user has changed their UserName, you can set a new name here. Not required for new users."
        Private Sub New()
        End Sub
    End Class
End Namespace