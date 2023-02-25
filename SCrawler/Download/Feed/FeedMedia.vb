' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports SCrawler.API.Base
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Tools
Imports UserMediaD = SCrawler.DownloadObjects.TDownloader.UserMediaD
Namespace DownloadObjects
    <ToolboxItem(False), DesignTimeVisible(False)>
    Public Class FeedMedia
#Region "Events"
        Friend Event MediaDeleted(ByVal Sender As Object)
#End Region
#Region "Declarations"
        Private Const VideoHeight As Integer = 450
        Private WithEvents MyPicture As PictureBox
        Private ReadOnly MyImage As ImageRenderer
        Private ReadOnly MyVideo As FeedVideo
        Friend ReadOnly Property Exists As Boolean
            Get
                Return Not MyPicture Is Nothing Or Not MyVideo Is Nothing
            End Get
        End Property
        Friend ReadOnly Property HasError As Boolean
        Friend ReadOnly File As SFile
        Public Shadows Property Width As Integer
            Get
                Return MyBase.Width
            End Get
            Set(ByVal w As Integer)
                If Size.Width <> w Then
                    Dim s As New Size(w, If(MyImage Is Nothing, VideoHeight, MyImage.FitToWidthF(w).Height))
                    Dim objSize As Size = s
                    objSize.Height += (TP_MAIN.RowStyles(0).Height + PaddingE.GetOf({TP_MAIN}).Vertical(2))
                    MinimumSize = objSize
                    MyBase.MaximumSize = objSize
                    Size = objSize
                    If Not MyImage Is Nothing Then
                        With MyPicture
                            .MinimumSize = Nothing
                            .MaximumSize = Nothing
                            .Size = s
                            .MinimumSize = s
                            .MaximumSize = s
                        End With
                    End If
                End If
            End Set
        End Property
        Private ReadOnly UserKey As String
        Private ReadOnly Post As UserMedia
        Friend ReadOnly Property Checked As Boolean
            Get
                Return CH_CHECKED.Checked
            End Get
        End Property
        Friend ReadOnly Property Information As String
#End Region
#Region "Initializers"
        Public Sub New()
            InitializeComponent()
        End Sub
        Friend Sub New(ByVal Media As UserMediaD, ByVal Width As Integer, ByVal Handler As MediaDeletedEventHandler)
            Try
                InitializeComponent()
                File = Media.Data.File
                If Not File.Exists And Media.Data.Type = UserMedia.Types.Video Then File.Path = $"{File.Path.CSFilePS}Video"
                If Not File.Exists Then
                    If Not Media.Data.SpecialFolder.IsEmptyString Then
                        File.Path = $"{File.Path.CSFilePS}{Media.Data.SpecialFolder}".CSFileP
                        If Not File.Exists And Media.Data.Type = UserMedia.Types.Video Then File.Path = $"{File.Path.CSFilePS}Video"
                    End If
                End If
                If File.Exists Then
                    Information = $"Type: {Media.Data.Type}"
                    Information.StringAppendLine($"File: {File.File}")
                    Information.StringAppendLine($"Address: {File}")
                    Information.StringAppendLine($"Downloaded: {Media.Date.ToStringDate(ADateTime.Formats.BaseDateTime)}")
                    If Media.Data.Post.Date.HasValue Then Information.StringAppendLine($"Posted: {Media.Data.Post.Date.Value.ToStringDate(ADateTime.Formats.BaseDateTime)}")
                    Dim infoType As UserMedia.Types = Media.Data.Type
                    Dim h%
                    Dim s As Size

                    Post = Media.Data

                    Select Case Media.Data.Type
                        Case UserMedia.Types.Picture, UserMedia.Types.GIF
                            MyImage = New ImageRenderer(File)
                            s = MyImage.FitToWidthF(Width)
                            h = s.Height
                            MyPicture = New PictureBox With {
                                .SizeMode = PictureBoxSizeMode.Zoom,
                                .Image = MyImage,
                                .InitialImage = .Image,
                                .Dock = DockStyle.None,
                                .Anchor = AnchorStyles.Left + AnchorStyles.Top,
                                .Size = s,
                                .MinimumSize = s,
                                .MaximumSize = s,
                                .Tag = File,
                                .Margin = New Padding(0),
                                .Padding = New Padding(0),
                                .ContextMenuStrip = CONTEXT_DATA
                            }
                            TP_MAIN.Controls.Add(MyPicture, 0, 1)
                            BTT_CONTEXT_OPEN_MEDIA.Text &= " picture"
                            BTT_CONTEXT_DELETE.Text &= " picture"
                        Case UserMedia.Types.Video, UserMedia.Types.m3u8
                            infoType = UserMedia.Types.Video
                            MyVideo = New FeedVideo(File) With {.Tag = File, .Dock = DockStyle.Fill, .ContextMenuStrip = CONTEXT_DATA}
                            If MyVideo.HasError Then HasError = True
                            TP_MAIN.Controls.Add(MyVideo, 0, 1)
                            BTT_CONTEXT_OPEN_MEDIA.Text &= " video"
                            BTT_CONTEXT_DELETE.Text &= " video"
                            h = VideoHeight
                        Case Else : Throw New ArgumentNullException With {.HelpLink = 1}
                    End Select

                    Dim info$ = $"[{infoType}] - "

                    If Not Media.User Is Nothing Then
                        With Media.User
                            UserKey = .Key
                            Information &= vbNewLine.StringDup(2)
                            If .IncludedInCollection Then Information.StringAppendLine($"User collection: { .CollectionName}")
                            Information.StringAppendLine($"User site: { .Site}")
                            Information.StringAppendLine($"User name: {IIf(Not .FriendlyName.IsEmptyString And Not .IncludedInCollection, .FriendlyName, .Name)}")
                            If .Site = API.Instagram.InstagramSite Then BTT_CONTEXT_OPEN_USER_POST.Visible = False
                            If .IncludedInCollection Then info &= $"[{ .CollectionName}]: "
                            info &= $"{ .Site} - {IIf(Not .FriendlyName.IsEmptyString And Not .IncludedInCollection, .FriendlyName, .Name)}"
                        End With
                    End If

                    If Settings.FeedAddSessionToCaption Then info = $"[{Media.Session}] {info}"
                    If Settings.FeedAddDateToCaption Then info &= $" ({Media.Date.ToStringDate(ADateTime.Formats.BaseDateTime)})"
                    LBL_INFO.Text = info

                    s = New Size(Width, h + TP_MAIN.RowStyles(0).Height + PaddingE.GetOf({TP_MAIN}).Vertical(2))
                    Size = s
                    MinimumSize = s
                    MaximumSize = s

                    If Not Handler Is Nothing Then AddHandler Me.MediaDeleted, Handler
                Else
                    Throw New ArgumentNullException With {.HelpLink = 1}
                End If
            Catch aex As ArgumentNullException When aex.HelpLink = 1
                HasError = True
            Catch tex As Threading.ThreadStateException
                HasError = True
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendInLog, ex, $"[DownloadObjects.FeedMedia({File})]")
                HasError = True
            End Try
        End Sub
#End Region
#Region "Dispose"
        Private Sub FeedImage_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            If Not MyImage Is Nothing Then MyImage.Dispose()
            If Not MyPicture Is Nothing Then MyPicture.Dispose()
            If Not MyVideo Is Nothing Then MyVideo.Dispose()
        End Sub
#End Region
#Region "LBL"
        Private Sub LBL_INFO_MouseClick(sender As Object, e As MouseEventArgs) Handles LBL_INFO.MouseClick
            If e.Button = MouseButtons.Left Then ControlInvoke(CH_CHECKED, Sub() CH_CHECKED.Checked = Not CH_CHECKED.Checked)
        End Sub
        Private Sub LBL_INFO_DoubleClick(sender As Object, e As EventArgs) Handles LBL_INFO.DoubleClick
            If Not UserKey.IsEmptyString Then
                Dim u As IUserData = Settings.GetUser(UserKey)
                If Not u Is Nothing Then u.OpenFolder()
            End If
        End Sub
#End Region
#Region "Picture / Video objects"
        Private Sub MyPicture_DoubleClick(sender As Object, e As EventArgs) Handles MyPicture.DoubleClick
            Try : Process.Start(File) : Catch : End Try
        End Sub
#End Region
#Region "Context"
        Private Sub BTT_CONTEXT_OPEN_MEDIA_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_OPEN_MEDIA.Click
            File.Open(, EDP.None)
        End Sub
        Private Sub BTT_CONTEXT_OPEN_USER_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_OPEN_USER.Click
            If Not UserKey.IsEmptyString Then
                Dim u As IUserData = Settings.GetUser(UserKey)
                If Not u Is Nothing Then u.OpenFolder()
            End If
        End Sub
        Private Sub BTT_CONTEXT_OPEN_USER_URL_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_OPEN_USER_URL.Click
            If Not UserKey.IsEmptyString Then
                Dim u As IUserData = Settings.GetUser(UserKey)
                If Not u Is Nothing Then u.OpenSite()
            End If
        End Sub
        Private Sub BTT_CONTEXT_OPEN_USER_POST_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_OPEN_USER_POST.Click
            Try
                If Not UserKey.IsEmptyString And Not Post.Post.ID.IsEmptyString Then
                    Dim u As IUserData = Settings.GetUser(UserKey)
                    If Not u Is Nothing Then
                        Dim url$ = UserDataBase.GetPostUrl(u, Post)
                        If Not url.IsEmptyString Then
                            Try : Process.Start(url) : Catch : End Try
                        End If
                    End If
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, $"[FeedMedia.OpenPost({UserKey}, {Post.Post.ID})]")
            End Try
        End Sub
        Private Sub BTT_CONTEXT_FIND_USER_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_FIND_USER.Click
            If Not UserKey.IsEmptyString Then MainFrameObj.FocusUser(UserKey, True)
        End Sub
        Private Sub BTT_CONTEXT_INFO_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_INFO.Click
            MsgBoxE({Information, "Post information"})
        End Sub
        Private Sub BTT_CONTEXT_DELETE_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_DELETE.Click
            DeleteFile(False)
        End Sub
        Friend Function DeleteFile(ByVal Silent As Boolean) As Boolean
            Const msgTitle$ = "Deleting a file"
            Try
                If Silent OrElse MsgBoxE({$"Are you sure you want to delete the [{File.File}] file?{vbCr}{File}", msgTitle}, vbExclamation,,, {"Process", "Cancel"}) = 0 Then
                    If Not MyVideo Is Nothing Then MyVideo.Stop()
                    If File.Delete(SFO.File, Settings.DeleteMode, EDP.ThrowException) Then
                        If Not Silent Then RaiseEvent MediaDeleted(Me) : MsgBoxE({"File deleted", msgTitle})
                        LBL_INFO.Height = 0
                        Height = 0
                        Return True
                    End If
                End If
                Return False
            Catch ex As Exception
                Dim e As New ErrorsDescriber(EDP.LogMessageValue) With {.ShowMainMsg = Not Silent, .ShowExMsg = .ShowMainMsg}
                Return ErrorsDescriber.Execute(e, ex, $"[FeedMedia.DeleteFile({File})]", False)
            End Try
        End Function
#End Region
    End Class
End Namespace