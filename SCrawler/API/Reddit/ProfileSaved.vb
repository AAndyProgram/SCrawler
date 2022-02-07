' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports System.Threading
Imports PersonalUtilities.Forms.Toolbars
Namespace API.Reddit
    Friend NotInheritable Class ProfileSaved
        Friend Shared ReadOnly Property DataPath As SFile = Settings(Sites.Reddit).SavedPostsPath
        Private Sub New()
        End Sub
        Friend Shared Sub Download(ByRef Bar As MyProgress, ByVal Token As CancellationToken)
            Try
                Dim u As New UserInfo(Settings(Sites.Reddit).SavedPostsUserName.Value, Sites.Reddit) With {.IsChannel = True, .SpecialPath = DataPath}
                u.UpdateUserFile()
                Using user As New UserData(u,, False)
                    DirectCast(user.Self, UserDataBase).IsSavedPosts = True
                    user.Progress = Bar
                    If Not user.FileExists Then user.UpdateUserInformation()
                    user.DownloadData(Token)
                    Bar.InformationTemporary = $"Images: {user.DownloadedPictures}; Videos: {user.DownloadedVideos}"
                End Using
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendInLog, ex, "[API.Reddit.ProfileSaved.Download]")
            End Try
        End Sub
    End Class
End Namespace