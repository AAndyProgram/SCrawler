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
        Friend Event MediaDownload As EventHandler
        Friend Event FeedAddWithRemove(ByVal Sender As FeedMedia, ByVal Feeds As IEnumerable(Of String), ByVal Media As UserMediaD, ByVal RemoveOperation As Boolean)
        Friend Event MediaMove(ByVal Sender As FeedMedia, ByVal MCTOptions As FeedMoveCopyTo, ByRef Result As Boolean)
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
        Public Shadows Property Width(Optional ByVal UpdateImage As Boolean = True) As Integer
            Get
                Return MyBase.Width
            End Get
            Set(ByVal w As Integer)
                If Size.Width <> w Then
                    Dim s As New Size(w, If(MyImage Is Nothing, VideoHeight, If(UpdateImage, MyImage.FitToWidthF(w).Height, MyPicture.Height)))
                    Dim objSize As Size = s
                    objSize.Height += ObjectsPaddingHeight
                    MinimumSize = objSize
                    MyBase.MaximumSize = objSize
                    Size = objSize
                    If UpdateImage AndAlso Not MyImage Is Nothing Then
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
        Private ReadOnly Property ObjectsPaddingHeight As Integer
            Get
                Return TP_MAIN.RowStyles(0).Height + TP_MAIN.RowStyles(1).Height + PaddingE.GetOf({TP_MAIN}).Vertical(3)
            End Get
        End Property
        Private ReadOnly UserKey As String
        Friend ReadOnly Post As UserMedia
        Friend ReadOnly Media As UserMediaD
        Friend Property Checked As Boolean
            Get
                Return CH_CHECKED.Checked
            End Get
            Set(ByVal c As Boolean)
                If Not CH_CHECKED.Checked = c Then ControlInvokeFast(CH_CHECKED, Sub() CH_CHECKED.Checked = c)
            End Set
        End Property
        Friend ReadOnly Property Information As String
        Private ReadOnly Property IsSubscription As Boolean = False
        Private Function GetImageResize(ByVal Width As Integer, ByVal Height As Integer) As Size
            If Height > 0 Then
                Dim h% = Height - ObjectsPaddingHeight
                If h <= 0 Then h = Height
                Dim s As Size = MyImage.FitToHeightF(h)
                s = MyImage.FitToWidthF(s, Width, False)
                If s.Height > MyImage.Height Then s = MyImage.Size
                Return s
            Else
                Return MyImage.FitToWidthF(Width)
            End If
        End Function
        Friend Sub RerenderObject(ByVal Width As Integer, ByVal Height As Integer)
            If Not MyImage Is Nothing Then
                Dim s As Size
                If Height > 0 Then
                    s = GetImageResize(Width, Height)
                    With MyPicture
                        .MinimumSize = Nothing
                        .MaximumSize = Nothing
                        .Size = s
                        .MinimumSize = s
                        .MaximumSize = s
                        .Anchor = AnchorStyles.Top
                    End With
                    Me.Width(False) = Width
                Else
                    Me.Width = Width
                    MyPicture.Anchor = AnchorStyles.Left + AnchorStyles.Top
                End If
            Else
                Me.Width = Width
            End If
        End Sub
        Private Sub ApplyColors(ByVal Media As UserMediaD)
            Dim b As Color? = Nothing, f As Color? = Nothing
            If Not Media.User Is Nothing Then
                If Media.User.BackColor.HasValue Then b = Media.User.BackColor
                If Media.User.ForeColor.HasValue Then f = Media.User.ForeColor
                If Media.User.IsSubscription And Media.User.IsUser Then
                    If Not b.HasValue And Settings.MainFrameUsersSubscriptionsColorBack_USERS.Exists Then b = Settings.MainFrameUsersSubscriptionsColorBack_USERS.Value
                    If Not f.HasValue And Settings.MainFrameUsersSubscriptionsColorFore_USERS.Exists Then f = Settings.MainFrameUsersSubscriptionsColorFore_USERS.Value
                End If
            End If
            If Not b.HasValue And Settings.FeedBackColor.Exists Then b = Settings.FeedBackColor.Value
            If Not f.HasValue And Settings.FeedForeColor.Exists Then f = Settings.FeedForeColor.Value
            If b.HasValue Then
                BackColor = b.Value
                LBL_INFO.BackColor = b.Value
                If Not LBL_TITLE.IsDisposed Then LBL_TITLE.BackColor = b.Value
            End If
            If f.HasValue Then
                ForeColor = f.Value
                LBL_INFO.ForeColor = f.Value
                If Not LBL_TITLE.IsDisposed Then LBL_TITLE.ForeColor = f.Value
            End If
        End Sub
#End Region
#Region "Converter"
        Private Const ExtWebp As String = "webp"
        Private Const ExtJpg As String = "jpg"
        Private Function ConvertWebp(ByVal file As SFile, Optional ByVal NewCacheDir As Boolean = False) As SFile
            If file.Extension = ExtWebp Then
                If Settings.FfmpegFile.Exists Then
                    Dim dir As SFile
                    If NewCacheDir Then dir = Settings.Cache.NewPath Else dir = Settings.Cache
                    Dim f As SFile = file
                    f.Path = dir.Path
                    f.Extension = ExtJpg
                    Using imgBatch As New BatchExecutor
                        With imgBatch
                            .ChangeDirectory(dir)
                            .Execute($"""{Settings.FfmpegFile}"" -i ""{file}"" ""{f}""")
                        End With
                    End Using
                    If f.Exists Then Return f
                End If
            Else
                Return file
            End If
            Return Nothing
        End Function
#End Region
#Region "Initializers"
        Public Sub New()
            InitializeComponent()
        End Sub
        Friend Sub New(ByVal Media As UserMediaD, ByVal Width As Integer, ByVal Height As Integer, ByVal IsSession As Boolean)
            Try
                InitializeComponent()
                Me.Media = Media
                IsSubscription = If(Media.User?.IsSubscription, False)

                If IsSubscription Then
                    LBL_TITLE.Text = Media.Data.PictureOption.IfNullOrEmpty(Media.Data.File.Name)
                    If LBL_TITLE.Text.IsEmptyString Then
                        TP_MAIN.Controls.Remove(LBL_TITLE)
                        LBL_TITLE.Dispose()
                        TP_MAIN.RowStyles(1).Height = 0
                    End If

                    BTT_CONTEXT_DOWN.Visible = True
                    CONTEXT_SEP_0.Visible = True
                    BTT_CONTEXT_OPEN_USER.Visible = False
                    BTT_CONTEXT_OPEN_FILE_FOLDER.Visible = False
                    CONTEXT_SEP_5.Visible = False
                    BTT_CONTEXT_DELETE.Visible = False

                    CONTEXT_SEP_2.Visible = False
                    BTT_COPY_TO.Visible = False
                    BTT_MOVE_TO.Visible = False

                    If Not Media.Data.URL.IsEmptyString Then
                        Dim ext$ = Media.Data.URL.CSFile.Extension
                        Dim imgFile As New SFile With {.Path = Settings.Cache.RootDirectory.Path}
                        With Media.User
                            imgFile.Name = $"{IIf(.IncludedInCollection, .CollectionName, String.Empty)}{ .Site}{ .Name}_"
                            imgFile.Name &= (CLng(Media.Data.URL.GetHashCode) + CLng(Media.Data.File.GetHashCode)).ToString
                            imgFile.Name = imgFile.Name.StringRemoveWinForbiddenSymbols
                            imgFile.Extension = ExtJpg
                            If Not imgFile.Exists AndAlso Not ext.IsEmptyString AndAlso ext.ToLower = ExtWebp Then imgFile.Extension = ExtWebp
                        End With
                        If Not imgFile.Exists Then
                            Settings.Cache.Validate()
                            If GetWebFile(Media.Data.URL, imgFile, EDP.None) AndAlso imgFile.Exists Then File = ConvertWebp(imgFile)
                        Else
                            File = imgFile
                        End If
                    End If
                Else
                    TP_MAIN.Controls.Remove(LBL_TITLE)
                    LBL_TITLE.Dispose()
                    TP_MAIN.RowStyles(1).Height = 0
                    File = Media.Data.File
                    If Not File.Exists And Media.Data.Type = UserMedia.Types.Video Then File.Path = $"{File.Path.CSFilePS}Video"
                End If

                If Not File.Exists And Not IsSubscription Then
                    If Not Media.Data.SpecialFolder.IsEmptyString Then
                        File.Path = $"{File.Path.CSFilePS}{Media.Data.SpecialFolder}".CSFileP
                        If Not File.Exists And Media.Data.Type = UserMedia.Types.Video Then File.Path = $"{File.Path.CSFilePS}Video"
                    End If
                End If

                If File.Exists Then
                    Information = $"Type: {Media.Data.Type}"
                    Information.StringAppendLine($"File: {File.File}")
                    Information.StringAppendLine($"Address: {File}")
                    Information.StringAppendLine($"Downloaded: {Media.Date.ToStringDateDef}")
                    If Media.Data.Post.Date.HasValue Then Information.StringAppendLine($"Post date: {Media.Data.Post.Date.Value.ToStringDateDef}")
                    Dim infoType As UserMedia.Types = If(IsSubscription, UserMedia.Types.Picture, Media.Data.Type)
                    Dim h%
                    Dim s As Size

                    Post = Media.Data

                    Select Case infoType
                        Case UserMedia.Types.Picture, UserMedia.Types.GIF
                            Dim tmpMediaFile As SFile = ConvertWebp(File, True)
                            If tmpMediaFile.IsEmptyString Then Throw New ArgumentNullException With {.HelpLink = 1}
                            Try
                                MyImage = New ImageRenderer(tmpMediaFile, EDP.ThrowException)
                            Catch
                                MyImage.DisposeIfReady
                                MyImage = New ImageRenderer(New Bitmap(10, 10))
                            End Try
                            Dim a As AnchorStyles = AnchorStyles.Top + If(Height > 0, 0, AnchorStyles.Left)
                            s = GetImageResize(Width, Height)
                            h = s.Height
                            MyPicture = New PictureBox With {
                                .SizeMode = PictureBoxSizeMode.Zoom,
                                .Image = MyImage,
                                .InitialImage = .Image,
                                .Dock = DockStyle.None,
                                .Anchor = a,
                                .Size = s,
                                .MinimumSize = s,
                                .MaximumSize = s,
                                .Tag = File,
                                .Margin = New Padding(0),
                                .Padding = New Padding(0),
                                .ContextMenuStrip = CONTEXT_DATA
                            }
                            TP_MAIN.Controls.Add(MyPicture, 0, 2)
                            BTT_CONTEXT_OPEN_MEDIA.Text &= " picture"
                            BTT_CONTEXT_DELETE.Text &= " picture"
                        Case UserMedia.Types.Video, UserMedia.Types.m3u8
                            infoType = UserMedia.Types.Video
                            MyVideo = New FeedVideo(File) With {.Tag = File, .Dock = DockStyle.Fill, .ContextMenuStrip = CONTEXT_DATA}
                            If MyVideo.HasError Then HasError = True
                            TP_MAIN.Controls.Add(MyVideo, 0, 2)
                            BTT_CONTEXT_OPEN_MEDIA.Text &= " video"
                            BTT_CONTEXT_DELETE.Text &= " video"
                            h = VideoHeight
                            AddHandler MyVideo.DoubleClick, AddressOf MyPicture_DoubleClick
                        Case Else : Throw New ArgumentNullException With {.HelpLink = 1}
                    End Select

                    Dim info$ = If(Settings.FeedAddTypeToCaption.Value And Not IsSubscription, $"[{infoType}] - ", String.Empty)

                    If Not Media.User Is Nothing Then
                        With Media.User
                            Dim otherName$ = If(Media.IsSavedPosts, "Saved", Media.UserInfo.Name)
                            Dim site$ = If(Settings.FeedAddSiteToCaption.Value, $"{ .Site} - ", String.Empty)

                            UserKey = .Key
                            Information &= vbNewLine.StringDup(2)
                            If .IncludedInCollection Then Information.StringAppendLine($"User collection: { .CollectionName}")
                            Information.StringAppendLine($"User site: { .Site}")
                            Information.StringAppendLine($"User name: {CStr(IIf(Not .FriendlyName.IsEmptyString And Not .IncludedInCollection, .FriendlyName, .Name)).IfNullOrEmpty(otherName)}")
                            If .IncludedInCollection Then info &= $"[{ .CollectionName}]: "
                            If Settings.FeedShowFriendlyNames Or Not DirectCast(.Self, UserDataBase).FeedIsUser Then
                                info &= $"{site}{ .FriendlyName.IfNullOrEmpty(.Name).IfNullOrEmpty(otherName)}"
                            Else
                                info &= $"{site}{CStr(IIf(Not .FriendlyName.IsEmptyString And Not .IncludedInCollection, .FriendlyName, .Name)).IfNullOrEmpty(otherName)}"
                            End If
                        End With
                    End If

                    If Settings.FeedAddSessionToCaption And IsSession Then info = $"[{Media.Session}] {info}"
                    If Settings.FeedAddDateToCaption Then info &= $" ({Media.Date.ToStringDate(ADateTime.Formats.BaseDateTime)})"
                    LBL_INFO.Text = info
                    If Not Media.User Is Nothing AndAlso Not Media.User.HOST Is Nothing Then
                        With Media.User.HOST.Source
                            If Not .Image Is Nothing Then
                                ICON_SITE.Image = .Image
                            ElseIf Not .Icon Is Nothing Then
                                ICON_SITE.Image = .Icon.ToBitmap
                            End If
                        End With
                    End If

                    s = New Size(Width, h + ObjectsPaddingHeight)
                    Size = s
                    MinimumSize = s
                    MaximumSize = s
                    ApplyColors(Media)
                Else
                    Throw New ArgumentNullException With {.HelpLink = 1}
                End If

                If Media.IsSavedPosts Then
                    BTT_CONTEXT_OPEN_USER_URL.Visible = False
                    BTT_CONTEXT_FIND_USER.Visible = False
                End If

                If Settings.Feeds.FavoriteExists AndAlso Settings.Feeds.Favorite.Contains(Media) Then BTT_FEED_ADD_FAV.ControlChangeColor(True, False)
                If Settings.FeedShowSpecialFeedsMediaItem Then
                    With Settings.Feeds
                        AddHandler .FeedAdded, AddressOf Feed_FeedAdded
                        AddHandler .FeedRemoved, AddressOf Feed_FeedRemoved
                        If .Count > 0 Then
                            For Each fItem As FeedSpecial In .Self
                                If Not fItem.IsFavorite Then
                                    DownloadFeedForm.AddNewFeedItem(BTT_FEED_ADD_SPEC, CONTEXT_DATA, fItem, Nothing, AddressOf Feed_SPEC_ADD)
                                    DownloadFeedForm.AddNewFeedItem(BTT_FEED_ADD_SPEC_REMOVE, CONTEXT_DATA, fItem, Nothing, AddressOf Feed_SPEC_ADD_REMOVE)
                                    DownloadFeedForm.AddNewFeedItem(BTT_FEED_REMOVE_SPEC, CONTEXT_DATA, fItem, Nothing, AddressOf Feed_SPEC_REMOVE)
                                End If
                            Next
                        End If
                    End With
                End If
            Catch aex As ArgumentNullException When aex.HelpLink = 1
                HasError = True
            Catch tex As Threading.ThreadStateException
                HasError = True
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"[DownloadObjects.FeedMedia({File})]")
                HasError = True
            End Try
        End Sub
#End Region
#Region "Feed handlers"
        Private Sub Feed_FeedAdded(ByVal Source As FeedSpecialCollection, ByVal Feed As FeedSpecial)
            DownloadFeedForm.AddNewFeedItem(BTT_FEED_ADD_SPEC, CONTEXT_DATA, Feed, My.Resources.RSSPic_512, AddressOf Feed_SPEC_ADD, True)
            DownloadFeedForm.AddNewFeedItem(BTT_FEED_ADD_SPEC_REMOVE, CONTEXT_DATA, Feed, My.Resources.RSSPic_512, AddressOf Feed_SPEC_ADD_REMOVE, True)
            DownloadFeedForm.AddNewFeedItem(BTT_FEED_REMOVE_SPEC, CONTEXT_DATA, Feed, My.Resources.RSSPic_512, AddressOf Feed_SPEC_REMOVE, True)
        End Sub
        Private Sub Feed_FeedRemoved(ByVal Source As FeedSpecialCollection, ByVal Feed As FeedSpecial)
            DownloadFeedForm.Feed_FeedRemoved(BTT_FEED_ADD_SPEC, CONTEXT_DATA, Feed)
            DownloadFeedForm.Feed_FeedRemoved(BTT_FEED_REMOVE_SPEC, CONTEXT_DATA, Feed)
        End Sub
        Private Sub Feed_SPEC_ADD(ByVal Source As ToolStripMenuItem, ByVal e As EventArgs)
            Feed_SPEC_ADD_Impl(Source)
        End Sub
        Private Function Feed_SPEC_ADD_Impl(ByVal Source As ToolStripMenuItem) As FeedSpecial
            Dim f As FeedSpecial = Source.Tag
            If Not f Is Nothing AndAlso Not f.Disposed Then f.Add(Media) : Return f
            Return Nothing
        End Function
        Private Sub Feed_SPEC_ADD_REMOVE(ByVal Source As ToolStripMenuItem, ByVal e As EventArgs)
            Dim f As FeedSpecial = Feed_SPEC_ADD_Impl(Source)
            If Not f Is Nothing Then RaiseEvent FeedAddWithRemove(Me, {f.Name}, Media, False)
        End Sub
        Private Sub Feed_SPEC_REMOVE(ByVal Source As ToolStripMenuItem, ByVal e As EventArgs)
            Dim f As FeedSpecial = Source.Tag
            If Not f Is Nothing AndAlso Not f.Disposed Then f.Remove(Media) : RaiseEvent FeedAddWithRemove(Me, {f.Name}, Media, True)
        End Sub
#End Region
#Region "Dispose"
        Private Sub FeedImage_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            If Not MyImage Is Nothing Then MyImage.Dispose()
            If Not MyPicture Is Nothing Then MyPicture.Dispose() : MyPicture = Nothing
            If Not MyVideo Is Nothing Then MyVideo.Dispose()
            Try
                If Settings.FeedShowSpecialFeedsMediaItem Then
                    With Settings.Feeds
                        RemoveHandler .FeedAdded, AddressOf Feed_FeedAdded
                        RemoveHandler .FeedRemoved, AddressOf Feed_FeedRemoved
                    End With
                End If
            Catch
            End Try
        End Sub
#End Region
#Region "LBL"
        Private Sub LBL_INFO_MouseClick(sender As Object, e As MouseEventArgs) Handles LBL_INFO.MouseClick
            If e.Button = MouseButtons.Left Then ControlInvoke(CH_CHECKED, Sub() CH_CHECKED.Checked = Not CH_CHECKED.Checked)
        End Sub
        Private Sub LBL_INFO_DoubleClick(sender As Object, e As EventArgs) Handles LBL_INFO.DoubleClick
            If Not UserKey.IsEmptyString And Not IsSubscription Then
                Dim u As IUserData = Settings.GetUser(UserKey)
                If Not u Is Nothing Then u.OpenFolder()
            End If
        End Sub
        Private Sub LBL_TITLE_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles LBL_TITLE.MouseDoubleClick
            If Not Post.URL_BASE.IsEmptyString Then
                Try : Process.Start(Post.URL_BASE) : Catch : End Try
            End If
        End Sub
#End Region
#Region "Picture / Video objects"
        Private Sub MyPicture_DoubleClick(sender As Object, e As EventArgs) Handles MyPicture.DoubleClick
            Try : Process.Start(IIf(IsSubscription, Post.URL_BASE, File.ToString)) : Catch : End Try
        End Sub
#End Region
#Region "Context"
#Region "Down"
        Private Sub BTT_CONTEXT_DOWN_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_DOWN.Click
            RaiseEvent MediaDownload(Me, EventArgs.Empty)
        End Sub
#End Region
#Region "Open media, folder"
        Private Sub BTT_CONTEXT_OPEN_MEDIA_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_OPEN_MEDIA.Click, BTT_CONTEXT_OPEN_FILE_FOLDER.Click
            If Not sender Is Nothing AndAlso sender Is BTT_CONTEXT_OPEN_FILE_FOLDER Then
                GlobalOpenPath(File)
            Else
                File.Open()
            End If
        End Sub
        Private Sub BTT_CONTEXT_OPEN_USER_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_OPEN_USER.Click
            If Not UserKey.IsEmptyString Then
                Dim u As IUserData = Nothing
                If Not Media.IsSavedPosts Then
                    u = Settings.GetUser(UserKey)
                Else
                    If Not Media.UserInfo.Plugin.IsEmptyString Then
                        Dim host As Plugin.Hosts.SettingsHost = Settings(Media.UserInfo.Plugin, Media.UserInfo.AccountName)
                        If Not host Is Nothing Then
                            u = host.GetInstance(Plugin.ISiteSettings.Download.SavedPosts, Media.UserInfo, False, False)
                            With DirectCast(u, UserDataBase)
                                .IsSavedPosts = True
                                .HostStatic = True
                            End With
                        End If
                    End If
                End If
                If Not u Is Nothing Then
                    u.OpenFolder()
                    If Media.IsSavedPosts Then u.Dispose()
                End If
            End If
        End Sub
#End Region
#Region "Open URL"
        Private Sub BTT_CONTEXT_OPEN_USER_URL_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_OPEN_USER_URL.Click
            If Not UserKey.IsEmptyString And Not Media.IsSavedPosts Then
                Dim u As IUserData = Settings.GetUser(UserKey)
                If Not u Is Nothing Then u.OpenSite()
            End If
        End Sub
        Private Sub BTT_CONTEXT_OPEN_USER_POST_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_OPEN_USER_POST.Click
            Try
                Dim url$ = String.Empty
                If IsSubscription Then
                    url = Post.URL_BASE
                ElseIf Not Media.PostUrl.IsEmptyString Then
                    url = Media.PostUrl
                Else
                    If Not UserKey.IsEmptyString And Not Post.Post.ID.IsEmptyString Then
                        Dim u As IUserData
                        If Media.IsSavedPosts Then
                            If Not Media.UserInfo.Plugin.IsEmptyString Then
                                Dim host As Plugin.Hosts.SettingsHostCollection = Settings(Media.UserInfo.Plugin)
                                If Not host Is Nothing Then
                                    u = host.Default.GetInstance(Plugin.ISiteSettings.Download.SavedPosts, Media.UserInfo, False, False)
                                    If Not u Is Nothing AndAlso Not u.HOST Is Nothing Then
                                        With DirectCast(u, UserDataBase)
                                            .IsSavedPosts = True
                                            .HostStatic = True
                                        End With
                                        Try : url = u.HOST.Source.GetUserPostUrl(u, Post) : Catch : End Try
                                        u.Dispose()
                                    End If
                                End If
                            End If
                        Else
                            u = Settings.GetUser(UserKey)
                            If Not u Is Nothing Then url = UserDataBase.GetPostUrl(u, Post)
                        End If
                    End If
                End If
                If Not url.IsEmptyString Then
                    Try : Process.Start(url) : Catch : End Try
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, $"[FeedMedia.OpenPost({UserKey}, {Post.Post.ID})]")
            End Try
        End Sub
#End Region
#Region "Copy, Move"
        Private Sub BTT_COPY_MOVE_TO_Click(sender As Object, e As EventArgs) Handles BTT_COPY_TO.Click, BTT_MOVE_TO.Click
            Const MsgTitle$ = "Copy/Move checked files"
            Try
                If Not File.IsEmptyString Then
                    Dim isCopy As Boolean = sender Is BTT_COPY_TO
                    Dim moveOptions As FeedMoveCopyTo = Nothing
                    Dim ff As SFile = File
                    Dim result As Boolean = False

                    Using f As New FeedCopyToForm({File}, isCopy)
                        f.ShowDialog()
                        If f.DialogResult = DialogResult.OK Then moveOptions = f.Result
                    End Using
                    If Not moveOptions.Destination.IsEmptyString Then
                        ff.Path = moveOptions.DestinationTrue(Media).Path
                        If isCopy Then
                            result = File.Copy(ff)
                        Else
                            RaiseEvent MediaMove(Me, moveOptions, result)
                        End If
                        If result Then MsgBoxE(New MMessage($"File {IIf(isCopy, "copied", "moved")}{vbCr}Source: '{File}'{vbCr}Destination: '{ff}'", MsgTitle) With {.Editable = True})
                    End If
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, MsgTitle)
            End Try
        End Sub
#End Region
#Region "Feed"
        Private Sub BTT_FEED_ADD_FAV_Click(sender As Object, e As EventArgs) Handles BTT_FEED_ADD_FAV.Click, BTT_FEED_ADD_FAV_REMOVE.Click
            With Settings.Feeds.Favorite
                If Not .Contains(Media) Then .Add(Media)
                BTT_FEED_ADD_FAV.ControlChangeColor(True, False)
                If sender Is BTT_FEED_ADD_FAV_REMOVE Then RaiseEvent FeedAddWithRemove(Me, {FeedSpecial.FavoriteName}, Media, False)
            End With
        End Sub
        Private Sub BTT_FEED_ADD_SPEC_Click(sender As Object, e As EventArgs) Handles BTT_FEED_ADD_SPEC.Click, BTT_FEED_ADD_SPEC_REMOVE.Click
            With FeedSpecialCollection.ChooseFeeds(True)
                If .ListExists Then
                    .ForEach(Sub(f) f.Add(Media))
                    If sender Is BTT_FEED_ADD_SPEC_REMOVE Then RaiseEvent FeedAddWithRemove(Me, .Select(Function(f) f.Name), Media, False)
                End If
            End With
        End Sub
        Private Sub BTT_FEED_REMOVE_FAV_Click(sender As Object, e As EventArgs) Handles BTT_FEED_REMOVE_FAV.Click
            With Settings.Feeds.Favorite
                If .Contains(Media) Then .Remove(Media)
                BTT_FEED_ADD_FAV.ControlChangeColor(True)
                RaiseEvent FeedAddWithRemove(Me, {FeedSpecial.FavoriteName}, Media, True)
            End With
        End Sub
        Private Sub BTT_FEED_REMOVE_SPEC_Click(sender As Object, e As EventArgs) Handles BTT_FEED_REMOVE_SPEC.Click
            With FeedSpecialCollection.ChooseFeeds(False)
                If .ListExists Then
                    .ForEach(Sub(f) f.Remove(Media))
                    RaiseEvent FeedAddWithRemove(Me, .Select(Function(f) f.Name), Media, True)
                End If
            End With
        End Sub
#End Region
#Region "Info"
        Private Sub BTT_CONTEXT_FIND_USER_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_FIND_USER.Click
            If Not UserKey.IsEmptyString Then MainFrameObj.FocusUser(UserKey, True)
        End Sub
        Private Sub BTT_CONTEXT_INFO_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_INFO.Click
            MsgBoxE(New MMessage(Information, "Post information") With {.Editable = True})
        End Sub
#End Region
#Region "Delete"
        Private Sub BTT_CONTEXT_DELETE_Click(sender As Object, e As EventArgs) Handles BTT_CONTEXT_DELETE.Click
            DeleteFile(False)
        End Sub
#End Region
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