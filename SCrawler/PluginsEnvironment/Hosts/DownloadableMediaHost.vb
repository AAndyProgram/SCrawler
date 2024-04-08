' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports SCrawler.API.Base
Imports SCrawler.API.YouTube.Objects
Imports SCrawler.DownloadObjects.STDownloader
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Attributes
Namespace Plugin.Hosts
    Friend Class DownloadableMediaHost : Inherits YouTubeMediaContainerBase
        Protected Overrides Property IDownloadableMedia_Instance As IPluginContentProvider
            Get
                Return Instance
            End Get
            Set(ByVal _Instance As IPluginContentProvider)
                Instance = _Instance
            End Set
        End Property
        Friend Property Instance As UserDataBase
        Friend ReadOnly Property ExternalSource As IDownloadableMedia = Nothing
        <XMLEC> Friend Property ThumbAlong As Boolean = False
        Public Overrides ReadOnly Property Exists As Boolean
            Get
                If SiteKey = API.YouTube.YouTubeSiteKey Then
                    Return MyBase.Exists
                Else
                    Return _Exists
                End If
            End Get
        End Property
        Public Overrides Property File As SFile
            Get
                Return _File
            End Get
            Set(ByVal f As SFile)
                _File = f
                FileSetManually = True
            End Set
        End Property
        Public Overrides Sub Delete(ByVal RemoveFiles As Boolean)
            MyBase.Delete(RemoveFiles)
            If Not RemoveFiles And Not Settings.STDownloader_SnapshotsKeepWithFiles And Settings.STDownloader_SnapShotsCachePermamnent And Not ThumbAlong Then _
               ThumbnailFile.Delete(SFO.File, SFODelete.DeleteToRecycleBin, EDP.None)
        End Sub
        Friend Sub New(ByVal URL As String, ByVal OutputFile As SFile)
            Me.URL = URL
            Me.File = OutputFile
        End Sub
        Friend Sub New(ByVal f As SFile)
            Load(f)
            If _Exists Then
                Dim plugin As SettingsHostCollection
                SiteIcon = Nothing
                If Not MediaState = UserMediaStates.Downloaded Then
                    If Not SiteKey.IsEmptyString Then
                        If SiteKey = API.Gfycat.Envir.SiteKey Then
                            Instance = New API.Gfycat.Envir
                        ElseIf SiteKey = API.Imgur.Envir.SiteKey Then
                            Instance = New API.Imgur.Envir
                        Else
                            plugin = Settings(SiteKey)
                            If plugin Is Nothing Then
                                _Exists = False
                            Else
                                Instance = plugin(AccountName, True).GetInstance(ISiteSettings.Download.SingleObject, Nothing, False, False)
                                Instance.AccountName = AccountName
                            End If
                        End If
                        If _Exists And Not Instance Is Nothing AndAlso Not Instance.HOST Is Nothing Then SiteIcon = Instance.HOST.Source.Image
                    Else
                        _Exists = False
                    End If
                Else
                    plugin = Settings(SiteKey)
                    If Not plugin Is Nothing Then
                        Dim i As UserDataBase = plugin(AccountName, True).GetInstance(ISiteSettings.Download.SingleObject, Nothing, False, False)
                        If Not i Is Nothing Then
                            i.AccountName = AccountName
                            If Not i.HOST.Source.Image Is Nothing Then
                                SiteIcon = i.HOST.Source.Image
                            ElseIf Not i.HOST.Source.Icon Is Nothing Then
                                SiteIcon = i.HOST.Source.Icon.ToBitmap
                            End If
                            i.Dispose()
                        End If
                    End If
                End If
            End If
        End Sub
        Friend Sub New(ByVal Source As IDownloadableMedia)
            ExternalSource = Source
            ExchangeData(ExternalSource, Me)
            If Not ExternalSource Is Nothing Then
                With ExternalSource
                    SiteIcon = .SiteIcon
                    Site = .Site
                    SiteKey = .SiteKey
                    AccountName = .AccountName
                    If TypeOf ExternalSource Is DownloadableMediaHost Then ThumbAlong = DirectCast(ExternalSource, DownloadableMediaHost).ThumbAlong
                    _HasError = .HasError
                    _Exists = .Exists
                End With
            End If
        End Sub
        Public Overrides Function ToString(ByVal ForMediaItem As Boolean) As String
            If SiteKey = API.YouTube.YouTubeSiteKey Then
                Return MyBase.ToString(ForMediaItem)
            Else
                Return Title.IfNullOrEmpty(URL)
            End If
        End Function
        Public Overrides Sub Download(ByVal UseCookies As Boolean, ByVal Token As CancellationToken)
            ExchangeData(Me, ExternalSource)
            If Not ExternalSource Is Nothing Then
                With ExternalSource : .Progress = Progress : .Instance = Instance : End With
            End If
            Instance.DownloadSingleObject(If(ExternalSource, Me), Token)
            ExchangeData(ExternalSource, Me)
            Dim __url$ = DirectCast(Me, IDownloadableMedia).URL_BASE.IfNullOrEmpty(URL)
            If File.Exists And Not __url.IsEmptyString And MyDownloaderSettings.CreateUrlFiles Then
                Dim urlFile As SFile = CreateUrlFile(__url, File)
                If urlFile.Exists Then AddFile(urlFile)
            End If
            If Not ExternalSource Is Nothing Then
                With ExternalSource : _HasError = .HasError : _Exists = .Exists : End With
            End If
        End Sub
        Friend Sub ExchangeData(ByVal s As IDownloadableMedia, ByVal d As IDownloadableMedia)
            If Not s Is Nothing And Not d Is Nothing Then
                d.ContentType = s.ContentType
                d.URL = s.URL
                d.URL_BASE = s.URL_BASE
                d.MD5 = s.MD5
                d.File = s.File
                d.DownloadState = s.DownloadState
                d.PostID = s.PostID
                d.PostDate = s.PostDate
                d.SpecialFolder = s.SpecialFolder
                d.Attempts = s.Attempts
                d.ThumbnailUrl = s.ThumbnailUrl
                d.ThumbnailFile = s.ThumbnailFile
                d.Title = s.Title
                d.Size = s.Size
                d.Duration = s.Duration
                d.Checked = s.Checked
                d.AccountName = s.AccountName
                If TypeOf s Is DownloadableMediaHost And TypeOf d Is DownloadableMediaHost Then _
                   DirectCast(d, DownloadableMediaHost).ThumbAlong = DirectCast(s, DownloadableMediaHost).ThumbAlong
            End If
        End Sub
        Public Overrides Sub Load(ByVal f As SFile)
            MyBase.Load(f)
            If _Exists Then _Exists = Not MediaState = UserMediaStates.Downloaded OrElse File.Exists
        End Sub
        Public Overrides Sub Save()
            If FileSettings.IsEmptyString Then
                Dim f As SFile = MyFilesCache.NewFile
                f.Extension = "xml"
                FileSettings = f
            End If
            If NeedToSave() Then
                Using x As New XmlFile With {.AllowSameNames = True}
                    x.AddRange(ToEContainer.Elements)
                    x.Name = "MediaContainer"
                    x.Save(FileSettings)
                End Using
            End If
        End Sub
        Public Overrides Function GetHashCode() As Integer
            Return URL.GetHashCode
        End Function
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then Instance.DisposeIfReady() : ExternalSource.DisposeIfReady(False)
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace