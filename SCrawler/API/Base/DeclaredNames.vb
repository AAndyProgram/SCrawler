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

        Friend Const DownloadTextCaption As String = "Download text"
        Friend Const DownloadTextTip As String = "Download text (if available) for posts with image and video" & vbCr & "If this checkbox is checked, the post text will be downloaded along with the file and saved under the same name but with the 'txt' extension."
        Friend Const DownloadTextPostsCaption As String = "Download text posts"
        Friend Const DownloadTextPostsTip As String = "Download text (if available) for text posts (no image and video)"
        Friend Const DownloadTextSpecialFolderCaption As String = "Text special folder"
        Friend Const DownloadTextSpecialFolderTip As String = "If checked, text files will be saved to a separate folder"
        Private Sub New()
        End Sub
    End Class
End Namespace