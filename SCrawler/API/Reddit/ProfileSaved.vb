' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Namespace API.Reddit
    Friend NotInheritable Class ProfileSaved
        Private Sub New()
        End Sub
        Friend Shared Sub Download(ByRef Toolbar As StatusStrip, ByRef PR As ToolStripProgressBar)
            Try
                Dim Bar = New PersonalUtilities.Forms.Toolbars.MyProgress(Toolbar, PR, Nothing)
                Dim u As New UserInfo(Settings(Sites.Reddit).SavedPostsUserName.Value, Sites.Reddit) With {
                    .IsChannel = True,
                    .SpecialPath = $"{Settings(Sites.Reddit).Path.PathWithSeparator}\!Saved\"
                }
                u.UpdateUserFile()
                Using user As IUserData = UserDataBase.GetInstance(u)
                    DirectCast(user.Self, UserDataBase).IsSavedPosts = True
                    Bar.Enabled = True
                    DirectCast(user.Self, UserData).Progress = Bar
                    If Not user.FileExists Then user.UpdateUserInformation()
                    user.DownloadData(Nothing)
                    Dim m As New MMessage("Reddit saved posts download complete", "Saved posts downloading", {"OK", "Open folder"})
                    m.Text.StringAppendLine($"Downloaded images: {user.DownloadedPictures}")
                    m.Text.StringAppendLine($"Downloaded videos: {user.DownloadedVideos}")
                    If MsgBoxE(m) = 1 Then u.File.CutPath.Open(SFO.Path)
                    Bar.Enabled = False
                End Using
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendInLog, ex, "[API.Reddit.ProfileSaved.Download]")
            End Try
        End Sub
    End Class
End Namespace