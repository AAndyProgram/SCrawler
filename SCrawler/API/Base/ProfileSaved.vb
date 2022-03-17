' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin.Hosts
Imports System.Threading
Imports PersonalUtilities.Forms.Toolbars
Imports PDownload = SCrawler.Plugin.ISiteSettings.Download
Namespace API.Base
    Friend NotInheritable Class ProfileSaved
        Private ReadOnly Property HOST As SettingsHost
        Private ReadOnly Property Progress As MyProgress
        Friend Sub New(ByRef h As SettingsHost, ByRef Bar As MyProgress)
            HOST = h
            Progress = Bar
        End Sub
        Friend Sub Download(ByVal Token As CancellationToken)
            Try
                If HOST.Source.ReadyToDownload(PDownload.SavedPosts) Then
                    If HOST.Available(PDownload.SavedPosts) Then
                        HOST.DownloadStarted(PDownload.SavedPosts)
                        Dim u As New UserInfo With {.Plugin = HOST.Key, .Site = HOST.Name, .SpecialPath = HOST.SavedPostsPath}
                        Using user As IUserData = HOST.GetInstance(PDownload.SavedPosts, Nothing, False, False)
                            If Not user Is Nothing AndAlso (Not user.Name.IsEmptyString Or Not HOST.IsMyClass) Then
                                u.Name = user.Name
                                With DirectCast(user, UserDataBase).User
                                    u.IsChannel = .IsChannel
                                    u.UpdateUserFile()
                                End With
                                With DirectCast(user, UserDataBase)
                                    .User = u
                                    .LoadUserInformation()
                                    .IsSavedPosts = True
                                    .Progress = Progress
                                    If Not .FileExists Then .UpdateUserInformation()
                                End With
                                HOST.BeforeStartDownload(user, PDownload.SavedPosts)
                                user.DownloadData(Token)
                                Progress.InformationTemporary = $"Images: {user.DownloadedPictures(False)}; Videos: {user.DownloadedVideos(False)}"
                                HOST.AfterDownload(user, PDownload.SavedPosts)
                            End If
                        End Using
                    Else
                        Progress.InformationTemporary = $"Host [{HOST.Name}] is unavailable"
                    End If
                Else
                    Progress.InformationTemporary = $"Host [{HOST.Name}] is nor ready"
                End If
            Catch ex As Exception
                Progress.InformationTemporary = $"{HOST.Name} downloading error"
                ErrorsDescriber.Execute(EDP.SendInLog, ex, $"[API.Base.ProfileSaved.Download({HOST.Key})]")
            Finally
                HOST.DownloadDone(PDownload.SavedPosts)
            End Try
        End Sub
    End Class
End Namespace