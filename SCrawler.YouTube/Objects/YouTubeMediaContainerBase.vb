' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports SCrawler.Plugin
Imports SCrawler.API.YouTube.Base
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Functions.XML.Attributes
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports UMTypes = SCrawler.Plugin.UserMediaTypes
Imports UMStates = SCrawler.Plugin.UserMediaStates
Imports CollectionModes = PersonalUtilities.Functions.XML.Objects.IXMLValuesCollection.Modes
Namespace API.YouTube.Objects
    Public Class ContainerDateComparer : Implements IComparer(Of IYouTubeMediaContainer)
        Public Function Compare(ByVal x As IYouTubeMediaContainer, ByVal y As IYouTubeMediaContainer) As Integer Implements IComparer(Of IYouTubeMediaContainer).Compare
            Return x.DateCreated.CompareTo(y.DateCreated) * -1
        End Function
    End Class
    Public MustInherit Class YouTubeMediaContainerBase : Implements IYouTubeMediaContainer
#Region "Events"
        Public Event CheckedChange As EventHandler Implements IDownloadableMedia.CheckedChange
        Public Event FileDownloaded As EventHandler Implements IYouTubeMediaContainer.FileDownloaded
        Public Event FileDownloadStarted As EventHandler Implements IYouTubeMediaContainer.FileDownloadStarted
        Public Event DataDownloaded As EventHandler Implements IYouTubeMediaContainer.DataDownloaded
        Public Event ThumbnailChanged As EventHandler Implements IDownloadableMedia.ThumbnailChanged
        Public Event StateChanged As EventHandler Implements IDownloadableMedia.StateChanged
#End Region
#Region "XML names"
        Protected Friend Const Name_ObjectType As String = "ObjectType"
        Protected Friend Const Name_MediaType As String = "MediaType"
        Protected Friend Const Name_SiteKey As String = "SiteKey"
        Protected Friend Const Name_IsMusic As String = "IsMusic"
        Protected Friend Const Name_CachePath As String = "CachePath"

        Private Const Name_CheckedElements As String = "CheckedElements"
        Private Const Name_CheckedAttribute As String = "Checked"
#End Region
#Region "Base data"
        Protected _ObjectType As YouTubeMediaType = YouTubeMediaType.Undefined
        <XMLEC(Name_ObjectType)> Public Property ObjectType As YouTubeMediaType Implements IYouTubeMediaContainer.ObjectType
            Get
                Return _ObjectType
            End Get
            Set(ByVal t As YouTubeMediaType)
                _ObjectType = t
            End Set
        End Property
        Protected _MediaType As UMTypes = UMTypes.Undefined
        <XMLEC(Name_MediaType)> Public Property MediaType As UMTypes Implements IYouTubeMediaContainer.MediaType, IUserMedia.ContentType
            Get
                Return _MediaType
            End Get
            Set(ByVal t As UMTypes)
                _MediaType = t
            End Set
        End Property
        Protected _MediaState As UMStates = UMStates.Unknown
        Protected _MediaStateOnLoad As UMStates = UMStates.Unknown
        <XMLEC> Public Property MediaState As UMStates Implements IYouTubeMediaContainer.MediaState, IUserMedia.DownloadState
            Get
                If _MediaState = UMStates.Unknown And HasElements Then
                    Return If(Elements.Exists(Function(e) e.MediaState = UMStates.Downloaded), UMStates.Downloaded, _MediaState)
                Else
                    Return _MediaState
                End If
            End Get
            Set(ByVal s As UMStates)
                _MediaState = s
            End Set
        End Property
        Protected _SiteIcon As Image = Nothing
        Protected _SiteIconSetManually As Boolean = False
        Public Property SiteIcon As Image Implements IDownloadableMedia.SiteIcon
            Get
                If _SiteIconSetManually Then
                    Return _SiteIcon
                Else
                    Return If(IsMusic, My.Resources.SiteYouTube.YouTubeMusicPic_96, My.Resources.SiteYouTube.YouTubePic_96)
                End If
            End Get
            Set(ByVal Img As Image)
                _SiteIcon = Img
                _SiteIconSetManually = True
            End Set
        End Property
        Protected _Site As String = YouTubeSite
        <XMLEC> Public Property Site As String Implements IDownloadableMedia.Site
            Get
                Return _Site
            End Get
            Set(ByVal s As String)
                _Site = s
            End Set
        End Property
        Protected _SiteKey As String = YouTubeSiteKey
        <XMLEC(Name_SiteKey)> Public Property SiteKey As String Implements IDownloadableMedia.SiteKey
            Get
                Return _SiteKey
            End Get
            Set(ByVal Key As String)
                _SiteKey = Key
            End Set
        End Property
        <XMLEC> Public Property AccountName As String = String.Empty Implements IDownloadableMedia.AccountName
        <XMLEC(Name_IsMusic)> Public Property IsMusic As Boolean = False Implements IYouTubeMediaContainer.IsMusic
        <XMLEC> Public Property IsShorts As Boolean = False Implements IYouTubeMediaContainer.IsShorts
        <XMLEC> Public Property ID As String Implements IYouTubeMediaContainer.ID, IUserMedia.PostID
        <XMLEC> Public Property Title As String Implements IDownloadableMedia.Title
        <XMLEC> Public Property Description As String Implements IYouTubeMediaContainer.Description
        <XMLEC> Public Property PlaylistID As String Implements IYouTubeMediaContainer.PlaylistID
        <XMLEC> Public Property PlaylistTitle As String Implements IYouTubeMediaContainer.PlaylistTitle
        <XMLEC> Public Property UserID As String Implements IYouTubeMediaContainer.UserID
        <XMLEC> Public Property UserTitle As String Implements IYouTubeMediaContainer.UserTitle
#End Region
#Region "Playlist support"
        Private _ElementsNumber As Integer = 0
        <XMLEC> Protected Property ElementsNumber As Integer
            Get
                Return If(HasElements, Count, _ElementsNumber)
            End Get
            Set(ByVal _ElementsNumber As Integer)
                Me._ElementsNumber = _ElementsNumber
            End Set
        End Property
        Friend ReadOnly Property Elements As List(Of IYouTubeMediaContainer) Implements IYouTubeMediaContainer.Elements
        Friend ReadOnly Property HasElements As Boolean Implements IYouTubeMediaContainer.HasElements
            Get
                Return Count > 0
            End Get
        End Property
        Friend ReadOnly Property Count As Integer Implements IYouTubeMediaContainer.Count
            Get
                Return Elements.Count
            End Get
        End Property
        <XMLEC> Public Property PlaylistIndex As Integer = -1 Implements IYouTubeMediaContainer.PlaylistIndex
        <XMLEC> Protected Friend PlaylistCount As Integer = 0
#End Region
#Region "Data info"
        Friend ReadOnly Property MediaObjects As List(Of MediaObject) Implements IYouTubeMediaContainer.MediaObjects
        Friend Property [Protected] As Boolean = False
        Friend Property IsAudioSelected As Boolean = False
#Region "Array"
        ''' <summary>[-10] = disabled; [-1] = max; [-2] = audio only</summary>
        <XMLEC> Friend Property ArrayMaxResolution As Integer = -10
        ''' <param name="Value">[-1] = max; [-2] = audio only</param>
        Friend Sub SetMaxResolution(ByVal Value As Integer)
            If Not [Protected] Then
                ArrayMaxResolution = Value
                SelectedVideoIndex = -1
                If MediaObjects.Count > 0 And Value <> -2 Then
                    If Value = -1 Then
                        SelectedVideoIndex = MediaObjects.FindIndex(Function(mo) mo.Type = UMTypes.Video)
                    Else
                        SelectedVideoIndex = MediaObjects.FindIndex(Function(mo) mo.Type = UMTypes.Video And mo.Height <= Value)
                        If SelectedVideoIndex = -1 Then SelectedVideoIndex = MediaObjects.FindIndex(Function(mo) mo.Type = UMTypes.Video)
                    End If
                End If
                If HasElements Then Elements.ForEach(Sub(e As YouTubeMediaContainerBase) e.SetMaxResolution(Value))
            End If
        End Sub
#End Region
#Region "Thumbnails"
        Public ReadOnly Property Thumbnails As List(Of Thumbnail) Implements IYouTubeMediaContainer.Thumbnails
        Protected _ThumbnailUrl As String = String.Empty
        <XMLEC> Public Overridable Property ThumbnailUrl As String Implements IDownloadableMedia.ThumbnailUrl
            Get
                If Not CoverURL.IsEmptyString Then
                    Return CoverURL
                ElseIf _ThumbnailUrl.IsEmptyString And Thumbnails.Count > 0 Then
                    Return Thumbnails.FirstOrDefault.URL
                Else
                    Return _ThumbnailUrl
                End If
            End Get
            Set(ByVal url As String)
                _ThumbnailUrl = url
            End Set
        End Property
        Public ReadOnly Property ThumbnailUrlMedia As String Implements IYouTubeMediaContainer.ThumbnailUrlMedia
            Get
                If _ThumbnailUrl.IsEmptyString And Thumbnails.Count > 0 Then
                    Dim u$ = Thumbnails.FirstOrDefault(Function(t) Not t.URL.Contains(".webp")).URL
                    If u.IsEmptyString Then u = Thumbnails.First.URL
                    If u.IsEmptyString Then Return ThumbnailUrl Else Return u
                ElseIf HasElements Then
                    Return If(Elements.FirstOrDefault(Function(e) Not e.ThumbnailUrlMedia.IsEmptyString)?.ThumbnailUrlMedia, String.Empty).IfNullOrEmpty(_ThumbnailUrl)
                Else
                    Return _ThumbnailUrl
                End If
            End Get
        End Property
        <XMLEC> Protected _ThumbnailFile As SFile = Nothing
        Public ReadOnly Property ThumbnailFile As SFile Implements IYouTubeMediaContainer.ThumbnailFile
            Get
                Return _ThumbnailFile
            End Get
        End Property
        Private Property IDownloadableMedia_ThumbnailFile As String Implements IDownloadableMedia.ThumbnailFile
            Get
                Return ThumbnailFile
            End Get
            Set(ByVal f As String)
                _ThumbnailFile = f
            End Set
        End Property
#End Region
#Region "Video"
        <XMLEC> Friend Property SelectedVideoIndex As Integer = -1 Implements IYouTubeMediaContainer.SelectedVideoIndex
        Friend ReadOnly Property SelectedVideo As MediaObject
            Get
                If SelectedVideoIndex >= 0 Then Return MediaObjects(SelectedVideoIndex) Else Return Nothing
            End Get
        End Property
        Protected _OutputVideoExtension As String
        <XMLEC> Friend Property OutputVideoExtension As String Implements IYouTubeMediaContainer.OutputVideoExtension
            Get
                Return _OutputVideoExtension
            End Get
            Set(ByVal _OutputVideoExtension As String)
                If Not [Protected] Then
                    Me._OutputVideoExtension = _OutputVideoExtension
                    If HasElements Then Elements.ForEach(Sub(e) e.OutputVideoExtension = _OutputVideoExtension)
                End If
            End Set
        End Property
        <XMLEC("OutputVideoFPS")> Protected _OutputVideoFPS As Double = -1
        Friend Property OutputVideoFPS As Double
            Get
                Return _OutputVideoFPS
            End Get
            Set(ByVal fps As Double)
                If Not [Protected] Then
                    _OutputVideoFPS = fps
                    If HasElements Then Elements.ForEach(Sub(elem) DirectCast(elem, YouTubeMediaContainerBase).OutputVideoFPS = fps)
                End If
            End Set
        End Property
#End Region
#Region "Audio"
        <XMLEC> Friend Property SelectedAudioIndex As Integer = -1 Implements IYouTubeMediaContainer.SelectedAudioIndex
        Friend ReadOnly Property SelectedAudio As MediaObject
            Get
                If SelectedAudioIndex >= 0 Then Return MediaObjects(SelectedAudioIndex) Else Return Nothing
            End Get
        End Property
        Protected _OutputAudioCodec As String
        <XMLEC> Friend Property OutputAudioCodec As String Implements IYouTubeMediaContainer.OutputAudioCodec
            Get
                Return _OutputAudioCodec
            End Get
            Set(ByVal _OutputAudioCodec As String)
                If Not [Protected] Then
                    Me._OutputAudioCodec = _OutputAudioCodec
                    If HasElements Then Elements.ForEach(Sub(e) e.OutputAudioCodec = _OutputAudioCodec)
                End If
            End Set
        End Property
        <XMLEC(CollectionMode:=CollectionModes.String)>
        Friend ReadOnly Property PostProcessing_OutputAudioFormats As List(Of String)
        Friend Sub PostProcessing_OutputAudioFormats_Reset()
            With PostProcessing_OutputAudioFormats
                .Clear()
                .ListAddList(MyYouTubeSettings.DefaultAudioCodecAddit)
                If .Count > 0 Then .Sort()
            End With
        End Sub
        <XMLEC("OutputAudioBitrate")> Protected _OutputAudioBitrate As Integer = -1
        Friend Property OutputAudioBitrate As Integer
            Get
                Return _OutputAudioBitrate
            End Get
            Set(ByVal NewBitrate As Integer)
                If Not [Protected] Then
                    _OutputAudioBitrate = NewBitrate
                    If HasElements Then Elements.ForEach(Sub(elem) DirectCast(elem, YouTubeMediaContainerBase).OutputAudioBitrate = NewBitrate)
                End If
            End Set
        End Property
#End Region
#Region "Subtitles"
        Protected ReadOnly _Subtitles As List(Of Subtitles)
        Private ReadOnly _SubtitlesDelegated As List(Of Subtitles)
        Friend ReadOnly Property Subtitles As List(Of Subtitles) Implements IYouTubeMediaContainer.Subtitles
            Get
                If HasElements Then
                    If _SubtitlesDelegated.Count > 0 Then
                        Return _SubtitlesDelegated
                    Else
                        Return _Subtitles.Concat(Elements.SelectMany(Function(e) e.Subtitles)).Distinct.ListIfNothing.ListSort
                    End If
                ElseIf _SubtitlesDelegated.Count > 0 Then
                    Return _SubtitlesDelegated
                Else
                    Return _Subtitles
                End If
            End Get
        End Property
        <XMLEC(CollectionMode:=CollectionModes.String)>
        Friend ReadOnly Property SubtitlesSelectedIndexes As List(Of Integer) Implements IYouTubeMediaContainer.SubtitlesSelectedIndexes
        Protected _OutputSubtitlesFormat As String
        <XMLEC> Friend Property OutputSubtitlesFormat As String Implements IYouTubeMediaContainer.OutputSubtitlesFormat
            Get
                Return _OutputSubtitlesFormat
            End Get
            Set(ByVal _OutputSubtitlesFormat As String)
                If Not [Protected] Then
                    Me._OutputSubtitlesFormat = _OutputSubtitlesFormat
                    If HasElements Then Elements.ForEach(Sub(e) e.OutputSubtitlesFormat = _OutputSubtitlesFormat)
                End If
            End Set
        End Property
        <XMLEC(CollectionMode:=CollectionModes.String)>
        Friend ReadOnly Property PostProcessing_OutputSubtitlesFormats As List(Of String)
        Friend Sub PostProcessing_OutputSubtitlesFormats_Reset()
            With PostProcessing_OutputSubtitlesFormats
                .Clear()
                .ListAddList(MyYouTubeSettings.DefaultSubtitlesFormatAddit)
                If .Count > 0 Then .Sort()
            End With
        End Sub
        Friend Sub SubtitlesSelectedIndexesReset()
            With SubtitlesSelectedIndexes
                .Clear()
                Dim subs As List(Of Subtitles) = Subtitles
                .ListAddList(MyYouTubeSettings.DefaultSubtitles.Select(Function(s) subs.FindIndex(Function(ss) ss.ID = s)))
                If .Count > 0 Then .Sort() : .RemoveAll(Function(s) s = -1)
            End With
        End Sub
        Private Sub SetElementsSubtitles(ByVal Source As YouTubeMediaContainerBase)
            If Not Source Is Nothing And HasElements Then
                Dim subs As List(Of Subtitles) = Source.Subtitles
                For Each elem As YouTubeMediaContainerBase In Elements
                    With elem
                        ._SubtitlesDelegated.Clear()
                        If subs.Count > 0 Then ._SubtitlesDelegated.AddRange(subs)
                        .SubtitlesSelectedIndexes.Clear()
                        If Source.SubtitlesSelectedIndexes.Count > 0 Then .SubtitlesSelectedIndexes.AddRange(Source.SubtitlesSelectedIndexes)
                        .OutputSubtitlesFormat = Source.OutputSubtitlesFormat
                        .PostProcessing_OutputSubtitlesFormats.Clear()
                        If Source.PostProcessing_OutputSubtitlesFormats.Count > 0 Then .PostProcessing_OutputSubtitlesFormats.AddRange(Source.PostProcessing_OutputSubtitlesFormats)
                    End With
                Next
            End If
        End Sub
#End Region
#Region "IUserMedia Support"
        <XMLEC> Private Property Attempts As Integer Implements IUserMedia.Attempts
        Private _Object As Object = Nothing
        Private Property [Object] As Object Implements IUserMedia.Object
            Get
                Return If(_Object, Me)
            End Get
            Set(ByVal Obj As Object)
                _Object = Obj
            End Set
        End Property
        <XMLEC("MD5")> Private Property IUserMedia_MD5 As String Implements IUserMedia.MD5
        Public Shared Sub Update(ByVal Source As IUserMedia, ByVal Destination As IYouTubeMediaContainer)
            If Not Source Is Nothing And Not Destination Is Nothing Then
                Destination.ContentType = Source.ContentType
                Destination.URL = Source.URL
                Destination.URL_BASE = Source.URL_BASE
                Destination.MD5 = Source.MD5
                Destination.File = Source.File
                Destination.DownloadState = Source.DownloadState
                Destination.ID = Source.PostID
                Destination.PostDate = Source.PostDate
                Destination.SpecialFolder = Source.SpecialFolder
                Destination.Attempts = Source.Attempts
            End If
        End Sub
#End Region
        Protected _Duration As TimeSpan = Nothing
        <XMLEC(Provider:=GetType(DurationXmlConverter))>
        Public Overridable Property Duration As TimeSpan Implements IDownloadableMedia.Duration
            Get
                If HasElements Then
                    Return TimeSpan.FromSeconds(Elements.Sum(Function(e) If(e.Checked, e.Duration.TotalSeconds, 0)))
                Else
                    Return _Duration
                End If
            End Get
            Set(ByVal d As TimeSpan)
                _Duration = d
            End Set
        End Property
        Protected _Size As Integer = 0
        <XMLEC("SizeRecalculated")> Protected _SizeRecalculated As Boolean = False
        <XMLEC> Public Overridable Property Size As Integer Implements IDownloadableMedia.Size
            Get
                If HasElements Then
                    Return Elements.Sum(Function(e) If(e.Checked, e.Size, 0))
                ElseIf _SizeRecalculated Then
                    Return _Size
                Else
                    If Checked Then
                        If IsMusic And SelectedAudioIndex.ValueBetween(0, MediaObjects.Count - 1) Then
                            Return MediaObjects(SelectedAudioIndex).Size
                        ElseIf Not IsMusic And SelectedVideoIndex.ValueBetween(0, MediaObjects.Count - 1) Then
                            Return MediaObjects(SelectedVideoIndex).Size +
                                   If(SelectedAudioIndex.ValueBetween(0, MediaObjects.Count - 1), MediaObjects(SelectedAudioIndex).Size, 0)
                        Else
                            Return _Size
                        End If
                    Else
                        Return 0
                    End If
                End If
            End Get
            Set(ByVal s As Integer)
                _Size = s
            End Set
        End Property
        Public ReadOnly Property SizeStr As String Implements IYouTubeMediaContainer.SizeStr
            Get
                If Size > 0 Then
                    Dim sv% = Size / 1024
                    Dim value$
                    If sv >= 1000 Then
                        value = AConvert(Of String)(sv / 1024, VideoSizeProvider)
                        value &= " GB"
                    Else
                        value = AConvert(Of String)(sv, VideoSizeProvider)
                        value &= " MB"
                    End If
                    Return value
                Else
                    Return String.Empty
                End If
            End Get
        End Property
        <XMLEC> Public Property Height As Integer Implements IYouTubeMediaContainer.Height
        Friend ReadOnly Property HeightBase As Integer
            Get
                If Height > 0 Then
                    Return Height
                ElseIf SelectedVideoIndex.ValueBetween(0, MediaObjects.Count - 1) Then
                    Return SelectedVideo.Height
                ElseIf SelectedAudioIndex.ValueBetween(0, MediaObjects.Count - 1) Then
                    Return SelectedAudio.Height
                Else
                    Return 0
                End If
            End Get
        End Property
        Protected _Bitrate As Integer = 0
        <XMLEC> Public Overridable Property Bitrate As Integer Implements IYouTubeMediaContainer.Bitrate
            Get
                If HasElements Then
                    Try
                        Return Elements.Average(Function(e) e.Bitrate)
                    Catch
                        Return _Bitrate
                    End Try
                Else
                    Return _Bitrate
                End If
            End Get
            Set(ByVal _Bitrate As Integer)
                Me._Bitrate = _Bitrate
            End Set
        End Property
        Friend ReadOnly Property BitrateBase As Integer
            Get
                If Bitrate > 0 Then
                    Return Bitrate
                ElseIf OutputAudioBitrate > 0 Then
                    Return OutputAudioBitrate
                ElseIf HasElements Then
                    Try : Return Elements.Average(Function(e) DirectCast(e, YouTubeMediaContainerBase).BitrateBase) : Catch : End Try
                ElseIf SelectedAudioIndex.ValueBetween(0, MediaObjects.Count - 1) Then
                    Return SelectedAudio.Bitrate
                End If
                Return 0
            End Get
        End Property
        <XMLEC> Public Property DateCreated As Date = Now Implements IYouTubeMediaContainer.DateCreated
        <XMLEC> Public Property DateAdded As Date Implements IYouTubeMediaContainer.DateAdded
        Private Property IUserMedia_PostDate As Date? Implements IUserMedia.PostDate
            Get
                Return DateAdded
            End Get
            Set(ByVal d As Date?)
                If d.HasValue Then DateAdded = d.Value Else DateAdded = New Date
            End Set
        End Property
        <XMLEC> Public Property DateDownloaded As Date Implements IYouTubeMediaContainer.DateDownloaded
#End Region
#Region "HasError, Exists"
        Protected _HasError As Boolean = False
        Public ReadOnly Property HasError As Boolean Implements IDownloadableMedia.HasError
            Get
                Return _HasError
            End Get
        End Property
        Protected _Exists As Boolean = True
        Public Overridable ReadOnly Property Exists As Boolean Implements IDownloadableMedia.Exists
            Get
                If Not _Exists Then
                    Return False
                ElseIf Me.MediaState = UMStates.Downloaded Then
                    Return _Exists
                ElseIf ObjectType = YouTubeMediaType.PlayList Or ObjectType = YouTubeMediaType.Channel Then
                    Return HasElements
                Else
                    Return MediaObjects.Count > 0
                End If
            End Get
        End Property
        Protected Overridable Property IDownloadableMedia_Instance As IPluginContentProvider Implements IDownloadableMedia.Instance
#End Region
#Region "Checked"
        <XMLEC> Protected _Checked As Boolean = True
        Public Property Checked As Boolean Implements IDownloadableMedia.Checked
            Get
                If HasElements Then
                    Return Elements.Exists(Function(e) e.Checked)
                Else
                    Return _Checked
                End If
            End Get
            Set(ByVal _Checked As Boolean)
                Dim b As Boolean = Not Me._Checked = _Checked
                Me._Checked = _Checked
                If HasElements Then Elements.ForEach(Sub(e) e.Checked = _Checked)
                If b Then RaiseEvent CheckedChange(Me, Nothing)
            End Set
        End Property
        Public ReadOnly Property CheckState As CheckState Implements IYouTubeMediaContainer.CheckState
            Get
                If HasElements Then
                    Dim ecs As IEnumerable(Of CheckState) = Elements.Select(Function(e) e.CheckState)
                    If ecs.All(Function(c) c = CheckState.Checked) Then
                        Return CheckState.Checked
                    ElseIf ecs.All(Function(c) c = CheckState.Unchecked) Then
                        Return CheckState.Unchecked
                    Else
                        Return CheckState.Indeterminate
                    End If
                ElseIf Checked Then
                    Return CheckState.Checked
                Else
                    Return CheckState.Unchecked
                End If
            End Get
        End Property
#End Region
#Region "URL, File, Files, CachePath, SpecialPath, FileSettings"
        <XMLEC(Name_CachePath)> Private CachePath As SFile
        <XMLEC> Public Property URL As String Implements IUserMedia.URL
        Private _IUserMedia_URL_BASE As String = String.Empty
        <XMLEC("URL_BASE")> Private Property IUserMedia_URL_BASE As String Implements IUserMedia.URL_BASE
            Get
                Return _IUserMedia_URL_BASE.IfNullOrEmpty(URL)
            End Get
            Set(ByVal u As String)
                _IUserMedia_URL_BASE = u
            End Set
        End Property
        Friend Function GetUrls() As IEnumerable(Of String)
            Dim urls As New List(Of String)
            urls.ListAddList({URL, IUserMedia_URL_BASE}, LAP.NotContainsOnly)
            If HasElements And Not IsMusic Then urls.ListAddList(Elements.SelectMany(Function(elem As YouTubeMediaContainerBase) elem.GetUrls()), LAP.NotContainsOnly)
            Return urls
        End Function
        Protected Overridable Sub GenerateFileName()
        End Sub
        Protected Function GetPlayListTitle() As String
            Dim plsTitle$ = String.Empty
            If IsMusic And Not DateAdded = New Date Then plsTitle = $"{DateAdded.Year} - "
            plsTitle &= PlaylistTitle
            If IsShorts Then plsTitle &= " - Shorts"
            Return plsTitle
        End Function
        <XMLEC> Public Property SpecialPathDisabled As Boolean = False
        Protected _SpecialPath As String = String.Empty
        <XMLEC> Public Property SpecialPath As String Implements IUserMedia.SpecialFolder
            Get
                If SpecialPathDisabled Then
                    Return String.Empty
                ElseIf Not _SpecialPath.IsEmptyString Then
                    Return _SpecialPath
                ElseIf IsShorts Then
                    Return "Shorts"
                ElseIf HasElements Or PlaylistCount > 0 Then
                    Return PlaylistTitle.IfNullOrEmpty(Title).IfNullOrEmpty(UserTitle)
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal p As String)
                _SpecialPath = p
            End Set
        End Property
        Public Sub SpecialPathSetForPlaylist(ByVal Path As String)
            _SpecialPath = Path
            _FileIsPlaylistObject = True
            If ObjectType = YouTubeMediaType.Single AndAlso Not GetPlayListTitle.IsEmptyString Then _SpecialPath.StringAppend(GetPlayListTitle(), "\")
            If Elements.Count > 0 Then Elements.ForEach(Sub(e) e.SpecialFolder = Path)
        End Sub
        Private ReadOnly _Files As List(Of SFile)
        <XMLEC> Protected Friend Property Files As List(Of SFile) Implements IYouTubeMediaContainer.Files
            Get
                If HasElements Then
                    Return GetFilesFiles()
                Else
                    Return _Files
                End If
            End Get
            Set(ByVal f As List(Of SFile))
                _Files.ListAddList(f, LAP.NotContainsOnly)
            End Set
        End Property
        Protected Overloads Sub AddFile(ByVal f As SFile)
            _Files.ListAddValue(f, LAP.NotContainsOnly)
        End Sub
        Protected Overloads Sub AddFile(ByVal f As IEnumerable(Of SFile))
            _Files.ListAddList(f, LAP.NotContainsOnly)
        End Sub
        <XMLEC> Protected _File As SFile
        <XMLEC> Protected Friend Property FileSetManually As Boolean = False
        Public Property FileIgnorePlaylist As Boolean = False
        Private _FileIsPlaylistObject As Boolean = False
        ''' <summary>Compatible property for IUserMedia. Default: <see langword="False"/>.</summary>
        ''' <remarks>DON'T USE IN STD!</remarks>
        Public ReadOnly Property FileIsPlaylistObject As Boolean
            Get
                Return _FileIsPlaylistObject
            End Get
        End Property
        Private _AbsolutePath As Boolean = False
        Public Property AbsolutePath As Boolean
            Get
                Return _AbsolutePath
            End Get
            Set(ByVal ap As Boolean)
                If Not [Protected] Then
                    _AbsolutePath = ap
                    If Elements.Count > 0 Then Elements.ForEach(Sub(e As YouTubeMediaContainerBase) e.AbsolutePath = ap)
                End If
            End Set
        End Property
        Public Overridable Property File As SFile Implements IYouTubeMediaContainer.File
            Get
                Return _File
            End Get
            Set(ByVal f As SFile)
                If Not [Protected] Then
                    Select Case ObjectType
                        Case YouTubeMediaType.Channel : _File = f.Path
                        Case YouTubeMediaType.PlayList
                            If AbsolutePath Then
                                _File.Path = f.Path
                            Else
                                _File.Path = $"{f.PathWithSeparator}{GetPlayListTitle()}"
                            End If
                        Case YouTubeMediaType.Single
                            If PlaylistCount > 0 And Not FileIgnorePlaylist Then
                                _File.Path = f.Path
                                Dim pls$ = If(AbsolutePath, String.Empty, GetPlayListTitle())
                                If Not _File.Path.Contains(pls) Then _File.Path = $"{_File.PathWithSeparator(Not pls.IsEmptyString)}{pls}"
                            ElseIf Not f.Name.IsEmptyString Then
                                _File = f
                            Else
                                _File.Path = f.Path
                            End If
                        Case Else : _File = f
                    End Select
                    GenerateFileName()
                    If HasElements Then Elements.ForEach(Sub(e) e.File = _File)
                End If
            End Set
        End Property
        Friend Sub FileDateUpdate()
            Dim n$ = _File.Name.StringTrim
            Dim s$ = IIf(n.IsEmptyString, String.Empty, " ")
            Dim c$ = AccountName.IfNullOrEmpty(UserID)
            Select Case MyYouTubeSettings.FileAddDateToFileName.Value
                Case FileDateMode.Before : n = $"[{DateAdded:yyyy-MM-dd}]{s}{n}"
                Case FileDateMode.After : n = $"{n}{s}[{DateAdded:yyyy-MM-dd}]"
            End Select
            If Not c.IsEmptyString Then
                Select Case MyYouTubeSettings.FileAddChannelToFileName.Value
                    Case FileDateMode.Before : n = $"[{c}] {n}"
                    Case FileDateMode.After : n = $"{n} [{c}]"
                End Select
            End If
            _File.Name = n
        End Sub
        Public Property FileSettings As SFile
        Private Property IUserMedia_File As String Implements IUserMedia.File
            Get
                Return File
            End Get
            Set(ByVal f As String)
                File = f
            End Set
        End Property
        Friend Function GetFiles() As IEnumerable(Of SFile)
            Dim urls As New List(Of String)({File})
            If HasElements And Not IsMusic Then urls.ListAddList(Elements.SelectMany(Function(elem As YouTubeMediaContainerBase) elem.GetFiles()), LAP.NotContainsOnly)
            Return urls
        End Function
        Private Function GetFilesFiles() As IEnumerable(Of SFile)
            Dim f As New List(Of SFile)
            If File.Exists Then f.Add(File)
            If _Files.Count > 0 Then f.AddRange(_Files)
            If ThumbnailFile.Exists Then f.Add(ThumbnailFile)
            If HasElements Then f.ListAddList(Elements.SelectMany(Function(elem As YouTubeMediaContainerBase) elem.GetFilesFiles()), LAP.NotContainsOnly)
            Return f
        End Function
        Private _M3U8_PlaylistFiles As IEnumerable(Of SFile) = Nothing
        Friend Property M3U8_PlaylistFiles As IEnumerable(Of SFile)
            Get
                Return _M3U8_PlaylistFiles
            End Get
            Set(ByVal f As IEnumerable(Of SFile))
                If Not [Protected] Then
                    _M3U8_PlaylistFiles = f
                    If HasElements Then Elements.ForEach(Sub(e As YouTubeMediaContainerBase) e.M3U8_PlaylistFiles = f)
                End If
            End Set
        End Property
#End Region
#Region "Command"
        <XMLEC> Public Property UseCookies As Boolean = MyYouTubeSettings.DefaultUseCookies Implements IYouTubeMediaContainer.UseCookies
        Protected Const mp3 As String = "mp3"
        Private Const aac As String = "aac"
        Private Const ac3 As String = "ac3"
        Protected PostProcessing_AudioAC3 As Boolean = False
        Protected PostProcessing_AudioMP3 As Boolean = False
        Public Overridable ReadOnly Property Command(ByVal WithCookies As Boolean) As String Implements IYouTubeMediaContainer.Command
            Get
                If Not File.IsEmptyString Then
                    If File.Exists Then File = SFile.IndexReindex(File)
                    Dim cmd$ = String.Empty, formats$ = String.Empty, subs$ = String.Empty, remux$ = String.Empty
                    Dim embedThumbArgAdded As Boolean = False
                    _Size = 0
                    Height = 0
                    Bitrate = 0
                    _MediaType = UMTypes.Undefined
                    If SelectedVideoIndex >= 0 Then
                        '2023.3.4 -> 2023.7.6
                        'cmd.StringAppend($"bv*[format_id={SelectedVideo.ID}]")
                        cmd.StringAppend(SelectedVideo.ID)
                        _Size = SelectedVideo.Size
                        _MediaType = UMTypes.Video
                        Height = SelectedVideo.Height
                        _File.Extension = OutputVideoExtension
                        If Not embedThumbArgAdded And MyYouTubeSettings.DefaultVideoEmbedThumbnail Then
                            formats.StringAppend("--embed-thumbnail", " ")
                            embedThumbArgAdded = True
                        End If
                    Else
                        formats.StringAppend("--extract-audio", " ")
                        _MediaType = UMTypes.Audio
                    End If
                    If SelectedAudioIndex >= 0 Then
                        Dim atCodec$
                        '2023.3.4 -> 2023.7.6
                        'cmd.StringAppend($"ba*[format_id={SelectedAudio.ID}]", "+")
                        cmd.StringAppend(SelectedAudio.ID, "+")
                        If OutputAudioCodec.StringToLower = ac3 Then
                            PostProcessing_AudioAC3 = True
                            formats.StringAppend($"--audio-format {aac}", " ")
                            atCodec = aac
                        ElseIf SelectedVideoIndex >= 0 And OutputAudioCodec.StringToLower = mp3 Then
                            PostProcessing_AudioMP3 = True
                            formats.StringAppend($"--audio-format {aac}", " ")
                            atCodec = aac
                        Else
                            formats.StringAppend($"--audio-format {OutputAudioCodec.StringToLower}", " ")
                            atCodec = OutputAudioCodec.StringToLower
                        End If
                        If SelectedVideoIndex = -1 Then
                            formats.StringAppend("--add-metadata", " ")
                            If Not embedThumbArgAdded And MyYouTubeSettings.DefaultAudioEmbedThumbnail Then
                                formats.StringAppend("--embed-thumbnail", " ")
                                embedThumbArgAdded = True
                            End If
                        End If
                        _Size += SelectedAudio.Size
                        If _MediaType = UMTypes.Undefined Then _MediaType = UMTypes.Audio
                        Bitrate = SelectedAudio.Bitrate
                        Dim aCodec$ = SelectedAudio.Codec.StringToLower
                        If Not aCodec.IsEmptyString AndAlso Not aCodec.StringToLower = atCodec Then
                            remux.StringAppend($"{aCodec}>{atCodec}")
                            If SelectedVideoIndex = -1 Then
                                remux &= $"/{atCodec}"
                                _File.Extension = atCodec
                            End If
                        End If
                    End If
                    If SelectedVideoIndex >= 0 And Not SelectedVideo.Extension.StringToLower = OutputVideoExtension.StringToLower Then _
                       remux.StringAppend($"{SelectedVideo.Extension.StringToLower}>{OutputVideoExtension.StringToLower}/{OutputVideoExtension.StringToLower}", "/")
                    If Not remux.IsEmptyString Then formats.StringAppend($"--remux-video ""{remux}""", " ")
                    If SubtitlesSelectedIndexes.Count > 0 Then
                        subs = ListAddList(Nothing, Subtitles.Select(Function(s, i) If(SubtitlesSelectedIndexes.Contains(i), s.FullID, String.Empty)),
                                           LAP.NotContainsOnly, EDP.ReturnValue).ListToString(",")
                        subs = $"--write-subs --write-auto-subs --sub-format {OutputSubtitlesFormat.StringToLower} --sub-langs ""{subs}"" --convert-subs {OutputSubtitlesFormat.StringToLower}"
                    End If
                    If Not cmd.IsEmptyString Then
                        '2023.3.4 -> 2023.7.6
                        'cmd = $"yt-dlp -f ""{cmd}"""
                        'cmd = $"yt-dlp -f {cmd}"
                        cmd = $"{YTDLP_NAME} -f {cmd}"
                        If Not MyYouTubeSettings.ReplaceModificationDate Then cmd &= " --no-mtime"
                        cmd.StringAppend(formats, " ")
                        cmd.StringAppend(subs, " ")
                        cmd.StringAppend(YouTubeFunctions.GetCookiesCommand(WithCookies, YouTubeCookieNetscapeFile), " ")
                        cmd &= $" {URL} -o ""{File.PathWithSeparator}{File.Name}"""
                        File.Exists(SFO.Path, True)
                        Return cmd
                    End If
                End If
                Return String.Empty
            End Get
        End Property
#End Region
#Region "Initializer"
        Protected Sub New()
            Elements = New List(Of IYouTubeMediaContainer)
            Thumbnails = New List(Of Thumbnail)
            _Subtitles = New List(Of Subtitles)
            _SubtitlesDelegated = New List(Of Subtitles)
            SubtitlesSelectedIndexes = New List(Of Integer)
            MediaObjects = New List(Of MediaObject)
            _Files = New List(Of SFile)

            PostProcessing_OutputSubtitlesFormats = New List(Of String)
            PostProcessing_OutputSubtitlesFormats.ListAddList(MyYouTubeSettings.DefaultSubtitlesFormatAddit)
            PostProcessing_OutputAudioFormats = New List(Of String)
            PostProcessing_OutputAudioFormats.ListAddList(MyYouTubeSettings.DefaultAudioCodecAddit)
        End Sub
#End Region
#Region "ToString, GetHashCode, Equals"
        Public NotOverridable Overloads Overrides Function ToString() As String Implements IDownloadableMedia.ToString
            Return ToString(False)
        End Function
        Public Overridable Overloads Function ToString(ByVal ForMediaItem As Boolean) As String Implements IDownloadableMedia.ToString
            Return Title
        End Function
        Public Overrides Function GetHashCode() As Integer
            Return $"{ID}.{PlaylistID}.{UserID}.{Title}".GetHashCode
        End Function
        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            If Not Obj Is Nothing AndAlso Obj.GetType Is Me.GetType Then Return GetHashCode() = Obj.GetHashCode Else Return False
        End Function
#End Region
#Region "Delete, UpdateInfoFields, ThrowAny"
        Public Overridable Sub Delete(ByVal RemoveFiles As Boolean) Implements IDownloadableMedia.Delete
            If CachePath.Exists(SFO.Path, False) Then CachePath.Delete(SFO.Path, SFODelete.DeletePermanently, EDP.None)
            If FileSettings.Exists Then FileSettings.Delete(SFO.File, SFODelete.DeletePermanently, EDP.None)
            If RemoveFiles Then
                Dim fErr As New ErrorsDescriber(EDP.None)
                Dim dMode As SFODelete = SFODelete.DeleteToRecycleBin
                Dim paths As New List(Of SFile)
                Dim l As New ListAddParams(LAP.NotContainsOnly) With {.Comparer = New FComparer(Of SFile)(Function(x, y) x.PathNoSeparator = y.PathNoSeparator)}
                Dim isArr As Boolean = ObjectType <> YouTubeMediaType.Single And ObjectType <> YouTubeMediaType.Undefined
                If isArr And AbsolutePath Then paths.ListAddValue(File, l)
                File.Delete(SFO.File, dMode, fErr)
                If isArr Then paths.ListAddValue(ThumbnailFile, l)
                ThumbnailFile.Delete(SFO.File, dMode, fErr)
                If Files.Count > 0 Then
                    If isArr Then paths.ListAddList(Files, l)
                    Files.ForEach(Sub(f) f.Delete(SFO.File, dMode, fErr))
                End If
                If paths.Count > 0 Then paths.ForEach(Sub(p) If SFile.GetFiles(p,, IO.SearchOption.AllDirectories, EDP.ReturnValue).Count = 0 Then _
                                                                p.Delete(SFO.Path, dMode, EDP.SendToLog))
            End If
            If HasElements Then Elements.ForEach(Sub(e) e.Delete(RemoveFiles))
        End Sub
        Friend Sub UpdateInfoFields() Implements IYouTubeMediaContainer.UpdateInfoFields
            _Size = 0
            If SelectedVideoIndex >= 0 Then _Size += SelectedVideo.Size
            If SelectedAudioIndex >= 0 Then _Size += SelectedAudio.Size
            If HasElements Then Elements.ForEach(Sub(e) e.UpdateInfoFields())
        End Sub
        Protected Sub ThrowAny(ByVal Token As CancellationToken)
            Token.ThrowIfCancellationRequested()
            If disposedValue Then Throw New ObjectDisposedException(ToString(), "Object disposed")
        End Sub
#End Region
#Region "Download"
        Protected Shared Function CreateUrlFile(ByVal URL As String, ByVal File As SFile) As SFile
            Try
                File.Extension = "url"
                Using t As New TextSaver(File)
                    t.AppendLine("[InternetShortcut]")
                    t.AppendLine("IDList=")
                    t.AppendLine($"URL={URL}")
                    t.AppendLine()
                    t.Save(EDP.None)
                End Using
                Return File
            Catch ex As Exception
                Return Nothing
            End Try
        End Function
        Private Function GetPlaylistRow(ByVal Element As YouTubeMediaContainerBase, Optional ByVal __file As SFile = Nothing,
                                        Optional ByVal Mode As M3U8CreationMode = M3U8CreationMode.Absolute) As String
            Const m3u8DataRow$ = "#EXTINF:{0},{1}" & vbCrLf & "{2}"
            With Element
                Dim f As SFile = __file.IfNullOrEmpty(.File)
                Dim fStr$ = f.ToString.StringReplaceSymbols({"\"}, "/", EDP.ReturnValue)
                Dim __f$ = SymbolsConverter.ASCII.Extended.EncodeSymbolsOnly(If(Mode = M3U8CreationMode.Absolute, fStr, f.File), M3U8ExcludedSymbols)
                If Mode = M3U8CreationMode.Absolute Then __f = $"file:///{__f}"
                Dim fName$ = .Title.IfNullOrEmpty(f.Name)
                If MyYouTubeSettings.MusicPlaylistCreate_M3U8_AppendNumber And .PlaylistIndex > 0 Then fName = $"{ .PlaylistIndex}. {fName}"
                If Not .UserTitle.IsEmptyString Then
                    fName = $"{ .UserTitle} - {fName}"
                    If MyYouTubeSettings.MusicPlaylistCreate_M3U8_AppendArtist Then fName = $"{ .UserTitle} - {fName}"
                End If
                If MyYouTubeSettings.MusicPlaylistCreate_M3U8_AppendExt Then fName &= $".{f.Extension}"
                Return String.Format(m3u8DataRow, CInt(.Duration.TotalSeconds), fName, __f)
            End With
        End Function
        Private ReadOnly DownloadProgressPattern As RParams = RParams.DMS("\[download\]\s*([\d\.,]+)", 1, EDP.ReturnValue)
        Public Property Progress As MyProgress Implements IYouTubeMediaContainer.Progress
        Private Property IDownloadableMedia_Progress As Object Implements IDownloadableMedia.Progress
            Get
                Return Progress
            End Get
            Set(ByVal p As Object)
                If Not p Is Nothing Then
                    If TypeOf p Is MyProgress Then Progress = p
                Else
                    Progress = Nothing
                End If
            End Set
        End Property
        Private Sub DownloadElementsApply()
            If HasElements Then
                SetElementsSubtitles(Me)
                For Each elem As YouTubeMediaContainerBase In Elements
                    With elem
                        .OutputAudioCodec = OutputAudioCodec
                        .OutputSubtitlesFormat = OutputSubtitlesFormat
                        .OutputVideoExtension = OutputVideoExtension
                        .PostProcessing_OutputAudioFormats.Clear()
                        If PostProcessing_OutputAudioFormats.Count > 0 Then .PostProcessing_OutputAudioFormats.AddRange(PostProcessing_OutputAudioFormats)
                    End With
                Next
            End If
        End Sub
        Public Overridable Sub Download(ByVal UseCookies As Boolean, ByVal Token As CancellationToken) Implements IDownloadableMedia.Download
            DownloadElementsApply()
            If ObjectType = YouTubeMediaType.Single Then
                DownloadCommand(UseCookies, Token)
            Else
                DownloadCommandArray(UseCookies, Token)
                If HasElements AndAlso Elements(0).ObjectType = YouTubeMediaType.Single AndAlso Elements(0).IsMusic Then
                    Dim t As TextSaver = Nothing
                    Try
                        Dim f As SFile
                        Dim arr As M3U8CreationMode() = If(MyYouTubeSettings.MusicPlaylistCreate_CreationMode.Value = M3U8CreationMode.Both,
                                                           {M3U8CreationMode.Relative, M3U8CreationMode.Absolute},
                                                           {MyYouTubeSettings.MusicPlaylistCreate_CreationMode.Value})
                        Dim postfix$
                        Dim added As Boolean
                        Dim checkFile As Func(Of IYouTubeMediaContainer, Boolean) = Function(ByVal e As IYouTubeMediaContainer) As Boolean
                                                                                        If e.File.Exists Then
                                                                                            added = True
                                                                                            Return True
                                                                                        Else
                                                                                            Return False
                                                                                        End If
                                                                                    End Function
                        For Each cm As M3U8CreationMode In arr
                            If arr.Length > 1 AndAlso cm = M3U8CreationMode.Absolute Then postfix = "Abs" Else postfix = String.Empty
                            added = False
                            If MyYouTubeSettings.MusicPlaylistCreate_M3U8 Then
                                t = New TextSaver
                                t.AppendLine("#EXTM3U")
                                Elements.ForEach(Sub(e) If checkFile(e) Then t.AppendLine(GetPlaylistRow(e,, cm)))
                                f = $"{Elements(0).File.PathWithSeparator}Playlist{postfix}.m3u8"
                                If added Then t.SaveAs(f, EDP.SendToLog)
                                If f.Exists Then AddFile(f)
                                t.Dispose()
                            End If
                            added = False
                            If MyYouTubeSettings.MusicPlaylistCreate_M3U Then
                                t = New TextSaver
                                Elements.ForEach(Sub(e) If checkFile(e) Then t.AppendLine(If(cm = M3U8CreationMode.Relative, e.File.File, e.File.ToString)))
                                f = $"{Elements(0).File.PathWithSeparator}Playlist{postfix}.m3u"
                                If added Then t.SaveAs(f, EDP.SendToLog)
                                If f.Exists Then AddFile(f)
                                t.Dispose()
                            End If
                        Next
                    Catch ex As Exception
                        ErrorsDescriber.Execute(EDP.SendToLog, ex, "[YouTubeMediaContainerBase.Download.CreatePlaylist]")
                    End Try
                    t.DisposeIfReady
                End If
            End If
            RaiseEvent DataDownloaded(Me, Nothing)
        End Sub
        Private Function DownloadGetElemCountSingle() As Integer
            If ObjectType = YouTubeMediaType.Single Then
                Return 1
            Else
                Return Elements.Sum(Function(e) DirectCast(e, YouTubeMediaContainerBase).DownloadGetElemCountSingle())
            End If
        End Function
        Protected Sub DownloadCommandArray(ByVal UseCookies As Boolean, ByVal Token As CancellationToken)
            Try
                If HasElements Then
                    Dim prExists As Boolean = Not Progress Is Nothing
                    Dim fDown As EventHandler = Sub(ByVal Sender As Object, ByVal e As EventArgs)
                                                    RaiseEvent FileDownloadStarted(Sender, e)
                                                    If prExists Then Progress.Perform()
                                                End Sub
                    If prExists Then
                        With Progress
                            .Visible = True
                            .Value = 0
                            .Maximum = DownloadGetElemCountSingle()
                            .Information = "Downloading"
                        End With
                    End If

                    Dim cDown As Boolean = False
                    Dim fCover As SFile = Nothing
                    Dim cUrl$ = String.Empty
                    For Each elem In Elements
                        With DirectCast(elem, YouTubeMediaContainerBase)
                            'If Not .CoverDownloaded Then .CoverDownloaded = cDown
                            .CoverDownloaded = cDown
                            .CoverFile = fCover
                            .CoverURL = cUrl
                            AddHandler .FileDownloadStarted, fDown
                            .Download(UseCookies, Token)
                            cDown = .CoverDownloaded
                            fCover = .CoverFile
                            cUrl = .CoverURL
                            RemoveHandler .FileDownloadStarted, fDown
                        End With
                        If Token.IsCancellationRequested Or disposedValue Then Exit For
                    Next

                    If prExists Then
                        With Progress
                            .Value = .Maximum
                            .Perform(0)
                            .InformationTemporary = "Download completed"
                        End With
                    End If
                End If
            Catch oex As OperationCanceledException When Token.IsCancellationRequested
                Throw oex
            Catch dex As ObjectDisposedException When disposedValue
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"YTContainer.DownloadArrayError{ToString()}")
            Finally
                If Not Token.IsCancellationRequested And Not disposedValue Then
                    DateDownloaded = Now
                    MediaState = UMStates.Downloaded
                End If
            End Try
        End Sub
        Protected CoverDownloaded As Boolean = False
        Protected CoverFile As SFile = Nothing
        Protected CoverURL As String = String.Empty
        Private Sub DownloadPlaylistCover(ByVal PlsId As String, ByVal f As SFile, ByVal UseCookies As Boolean)
            Try
                Dim url$ = $"https://{IIf(IsMusic, "music", "www")}.youtube.com/playlist?list={PlsId}"
                Dim r$
                If DownloadObjects.STDownloader.MyDownloaderSettings.CreateUrlFiles Then
                    Dim ff As SFile = f
                    ff.Name = "album"
                    ff.Extension = "url"
                    CreateUrlFile(url, ff)
                    If ff.Exists Then AddFile(ff)
                End If
                If MyYouTubeSettings.CreateThumbnails_Music Then
                    Using resp As New Responser
                        If UseCookies And MyYouTubeSettings.Cookies.Count > 0 Then resp.Cookies.AddRange(MyYouTubeSettings.Cookies,, EDP.SendToLog)
                        r = resp.GetResponse(url,, EDP.ReturnValue)
                        If Not r.IsEmptyString Then
                            Dim p As RParams = RParams.DM("(?<=https:[\\/]{2,4})[^\.]*[\.]?googleusercontent.com[^\,]+?w(\d+).h(\d+)[^\,]+?(?=\\x22)", 0, RegexReturn.List, EDP.ReturnValue)
                            Dim l As List(Of String) = RegexReplace(r, p)
                            If l.ListExists Then l.RemoveAll(Function(uu) uu.IsEmptyString)
                            If l.ListExists Then
                                Dim u$ = l.Last
                                u = u.Replace("\/", "/").TrimStart("/")
                                Dim position%
                                Dim ch$
                                Do
                                    position = InStr(u, "\")
                                    If position > 0 Then
                                        ch = $"%{Mid(u, position + 2, 2)}"
                                        ch = SymbolsConverter.ASCII.Decode(ch, New ErrorsDescriber(False, False, False, String.Empty))
                                        u = u.Replace(Mid(u, position, 4), ch)
                                    End If
                                Loop While position > 0
                                url = LinkFormatterSecure(u)
                                f.Name = "cover"
                                f.Extension = "jpg"
                                If resp.DownloadFile(url, f, EDP.ReturnValue) And f.Exists Then _
                                   CoverFile = f : CoverURL = url : CoverDownloaded = True : AddFile(f)
                            End If
                        End If
                    End Using
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"DownloadPlaylistCover({PlsId}, {f})")
            End Try
        End Sub
        Private Structure TempFileConversion
            Friend File As SFile
            Friend Requested As Boolean
            Friend ToReplace As Boolean
            Friend ReadOnly Property Exists As Boolean
                Get
                    Return File.Exists
                End Get
            End Property
            Friend Sub Delete()
                If Not Requested Then File.Delete()
            End Sub
            Private Sub New(ByVal f As SFile)
                File = f
                Requested = False
                ToReplace = False
            End Sub
            Friend Sub New(ByVal f As SFile, ByVal Source As YouTubeMediaContainerBase)
                Me.New(f)
                Requested = Source.PostProcessing_OutputAudioFormats.Count > 0 AndAlso
                            Source.PostProcessing_OutputAudioFormats.Exists(Function(af) af.StringToLower = f.Extension)
            End Sub
            Public Shared Widening Operator CType(ByVal f As SFile) As TempFileConversion
                Return New TempFileConversion(f)
            End Operator
            Public Shared Widening Operator CType(ByVal f As TempFileConversion) As SFile
                Return f.File
            End Operator
            Public Overrides Function Equals(ByVal Obj As Object) As Boolean
                If Not IsNothing(Obj) Then
                    If TypeOf Obj Is TempFileConversion Then
                        Return DirectCast(Obj, TempFileConversion).File = File
                    ElseIf TypeOf Obj Is SFile Then
                        Return DirectCast(Obj, SFile) = File
                    ElseIf TypeOf Obj Is String Then
                        Return New TempFileConversion(CStr(Obj)).File = File
                    End If
                End If
                Return False
            End Function
        End Structure
        Protected Sub DownloadCommand(ByVal UseCookies As Boolean, ByVal Token As CancellationToken)
            Dim dCommand$ = String.Empty
            Try
                ThrowAny(Token)
                If MediaState = UMStates.Downloaded Or Not Checked Then Exit Sub
                RaiseEvent FileDownloadStarted(Me, Nothing)
                Using batch As New BatchExecutor(True) With {.Encoding = 65001}
                    Dim h As DataReceivedEventHandler = Sub(ByVal Sender As Object, ByVal e As DataReceivedEventArgs)
                                                            If Not e.Data.IsEmptyString Then
                                                                Dim v# = AConvert(Of Double)(RegexReplace(e.Data, DownloadProgressPattern), NumberProvider, -1)
                                                                If v >= 0 Then Progress.Value = v : Progress.Perform(0)
                                                                If Token.IsCancellationRequested Then batch.Kill()
                                                            End If
                                                        End Sub
                    With batch
                        Dim prExists As Boolean = Not Progress Is Nothing
                        If prExists Then
                            AddHandler .OutputDataReceived, h
                            With Progress
                                .Visible = True
                                .Value = 0
                                .Maximum = 100
                                .Provider = ProgressProvider
                                .Information = "Downloading"
                            End With
                        End If
                        .MainProcessName = MyYouTubeSettings.YTDLP.Name '"yt-dlp"
                        .FileExchanger = MyCache.NewInstance(Of BatchFileExchanger)(CachePath, EDP.ReturnValue)
                        .FileExchanger.DeleteCacheOnDispose = True
                        .AddCommand("chcp 65001")
                        .ChangeDirectory(MyYouTubeSettings.YTDLP.Value)
                        dCommand = Command(UseCookies)
#If DEBUG Then
                        Debug.WriteLine(dCommand)
#End If
                        Task.WaitAll({Task.Run(Sub() .Execute(dCommand))}, Token)
                        If Token.IsCancellationRequested Then .Kill(EDP.None)
                        ThrowAny(Token)
                        If prExists Then
                            RemoveHandler .OutputDataReceived, h
                            Progress.Value = 100
                            Progress.Perform(0)
                        End If
                        If Not File.Exists Then _File.Name = File.File
                        If File.Exists Then

                            M3U8_Append()

                            If DownloadObjects.STDownloader.MyDownloaderSettings.CreateUrlFiles Then
                                Dim fileUrl As SFile = File
                                fileUrl.Extension = "url"
                                CreateUrlFile(URL, fileUrl)
                                If fileUrl.Exists Then AddFile(fileUrl)
                            End If

                            With MyYouTubeSettings
                                If .CreateDescriptionFiles And (Not Description.IsEmptyString Or .CreateDescriptionFiles_CreateWithNoDescription) Then
                                    Dim fileDesr As SFile = File
                                    fileDesr.Extension = "txt"
                                    Using fileDesrText As New TextSaver(fileDesr)
                                        fileDesrText.Append($"Uploaded: {DateAdded:yyyy-MM-dd HH:mm:ss}")
                                        fileDesrText.AppendLine()
                                        fileDesrText.AppendLine($"URL: {URL}")
                                        fileDesrText.AppendLine($"Channel name: {AccountName}")
                                        fileDesrText.AppendLine($"Channel ID: {UserID}")
                                        If Not Description.IsEmptyString Then
                                            If Not fileDesrText.IsEmptyString Then fileDesrText.AppendLine.AppendLine()
                                            fileDesrText.Append(Description)
                                        End If
                                        fileDesrText.Save(EDP.None)
                                    End Using
                                    If fileDesr.Exists Then AddFile(fileDesr)
                                End If
                            End With

                            If PlaylistCount > 0 And Not CoverDownloaded And Not PlaylistID.IsEmptyString Then DownloadPlaylistCover(PlaylistID, File, UseCookies)
                            If prExists Then Progress.InformationTemporary = "Downloading: post processing"
                            _ThumbnailFile = File
                            _ThumbnailFile.Name &= "_thumb"
                            _ThumbnailFile.Extension = "jpg"
                            If Not ThumbnailUrl.IsEmptyString And
                               If(IsMusic, MyYouTubeSettings.CreateThumbnails_Music, MyYouTubeSettings.CreateThumbnails_Video).Value Then _
                               GetWebFile(ThumbnailUrl, _ThumbnailFile, EDP.None)

                            ThrowAny(Token)
                            If MyYouTubeSettings.FFMPEG.Value.Exists Then
                                .Reset()
                                .CommandsPermanent.Clear()
                                .CommandsPermanent.AddRange({"chcp 65001", BatchExecutor.GetDirectoryCommand(MyYouTubeSettings.FFMPEG.Value)})
                                .AutoReset = True
                                Dim files As IEnumerable(Of SFile)
                                Dim f As SFile
                                Dim commandFile As SFile
                                Dim format$
                                Dim fPattern$ = $"{File.PathWithSeparator}{File.Name}." & "{0}"
                                Dim fPatternFiles$ = $"{File.Name}*." & "{0}"
                                Dim fAacAudio As New TempFileConversion(New SFile(String.Format(fPattern, aac)), Me)
                                Dim mp3ThumbEmbedded As Boolean = False

                                Dim tempFilesList As New List(Of TempFileConversion)
                                Dim ttFile As TempFileConversion

                                Dim __updateBitrate As Boolean = OutputAudioBitrate > 0 AndAlso (SelectedAudioIndex = -1 OrElse SelectedAudio.Bitrate <> OutputAudioBitrate)
                                If __updateBitrate Then Bitrate = OutputAudioBitrate
                                Dim updateBitrate As Action(Of SFile) =
                                    Sub(ByVal sourceFile As SFile)
                                        If __updateBitrate AndAlso sourceFile.Exists Then
                                            Dim destFile As SFile = sourceFile
                                            destFile.Name &= "_new00"
                                            .Execute($"ffmpeg -i ""{sourceFile}"" -crf {MyYouTubeSettings.DefaultAudioBitrate_crf.Value} -b:a {OutputAudioBitrate}k ""{destFile}""")
                                            If destFile.Exists AndAlso sourceFile.Delete Then SFile.Rename(destFile, sourceFile)
                                        End If
                                    End Sub
                                Dim __getAAC_tried As Boolean = False
                                Dim AACExists As Func(Of Boolean) = Function() As Boolean
                                                                        If Not __getAAC_tried Then
                                                                            __getAAC_tried = True
                                                                            .Execute($"ffmpeg -i ""{File}"" -vn -acodec {aac} ""{fAacAudio.File}""")
                                                                            tempFilesList.Add(fAacAudio)
                                                                            updateBitrate.Invoke(fAacAudio.File)
                                                                        End If
                                                                        Return fAacAudio.Exists
                                                                    End Function
                                Dim tryToConvert As Action(Of String, SFile) =
                                    Sub(ByVal codec As String, ByVal dFile As SFile)
                                        ThrowAny(Token)
                                        .Execute($"ffmpeg -i ""{File}"" -vn -acodec {codec} ""{dFile}""")
                                        If Not codec = aac AndAlso Not dFile.Exists AndAlso AACExists.Invoke Then
                                            ThrowAny(Token)
                                            .Execute($"ffmpeg -i ""{fAacAudio.File}"" -f {codec} ""{dFile}""")
                                        End If
                                    End Sub
                                Dim embedThumbTo As Action(Of SFile) =
                                    Sub(ByVal dFile As SFile)
                                        If dFile.Exists And CoverFile.IfNullOrEmpty(ThumbnailFile).Exists Then
                                            Dim dFileNew As SFile = dFile
                                            dFileNew.Name &= "_NEW"
                                            .Execute($"ffmpeg -i ""{dFile}"" -i ""{CoverFile.IfNullOrEmpty(ThumbnailFile)}"" -map 0:0 -map 1:0 -c copy -id3v2_version 3 -metadata:s:v title=""Cover"" -metadata:s:v comment=""Cover"" ""{dFileNew}""")
                                            If dFileNew.Exists AndAlso dFile.Delete(,, EDP.ReturnValue) Then SFile.Rename(dFileNew, dFile)
                                        End If
                                    End Sub

                                'Subtitles
                                ThrowAny(Token)
                                If SubtitlesSelectedIndexes.Count > 0 And Not OutputSubtitlesFormat.IsEmptyString Then
                                    files = SFile.GetFiles(File, String.Format(fPatternFiles, OutputSubtitlesFormat.StringToLower),, EDP.ReturnValue)
                                    If files.ListExists Then
                                        AddFile(files)
                                        If PostProcessing_OutputSubtitlesFormats.Count > 0 Then
                                            For Each f In files
                                                For Each format In PostProcessing_OutputSubtitlesFormats
                                                    format = format.StringToLower
                                                    commandFile = $"{f.PathWithSeparator}{f.Name}.{format}"
                                                    AddFile(commandFile)
                                                    ThrowAny(Token)
                                                    .Execute($"ffmpeg -i ""{f}"" ""{commandFile}""")
                                                Next
                                            Next
                                        End If
                                    End If
                                End If

                                'Audio
                                ThrowAny(Token)
                                If PostProcessing_OutputAudioFormats.Count > 0 Or PostProcessing_AudioAC3 Or PostProcessing_AudioMP3 Or __updateBitrate Then

                                    If PostProcessing_AudioAC3 Then
                                        ttFile = New TempFileConversion(New SFile(String.Format(fPattern, ac3)), Me) With {.ToReplace = True}
                                        tempFilesList.Add(ttFile)
                                        If Not ttFile.Exists Then tryToConvert.Invoke(ac3, ttFile.File)
                                        updateBitrate.Invoke(ttFile.File)
                                    End If

                                    If PostProcessing_AudioMP3 Then
                                        ttFile = New TempFileConversion(New SFile(String.Format(fPattern, mp3)), Me) With {.ToReplace = True}
                                        tempFilesList.Add(ttFile)
                                        If Not ttFile.Requested Then ttFile.Requested = SelectedVideoIndex = -1 And OutputAudioCodec.StringToLower = mp3
                                        If Not ttFile.Exists Then tryToConvert.Invoke(mp3, ttFile.File)
                                        updateBitrate.Invoke(ttFile.File)
                                        embedThumbTo.Invoke(ttFile.File)
                                        mp3ThumbEmbedded = True
                                    End If

                                    If __updateBitrate Then
                                        format = OutputAudioCodec.StringToLower
                                        If Not format.IsEmptyString Then
                                            f = String.Format(fPattern, format)
                                            ttFile = New TempFileConversion(f, Me) With {.ToReplace = True}
                                            If Not ttFile.Requested Then ttFile.Requested = SelectedVideoIndex = -1
                                            If Not f.Exists Then
                                                tempFilesList.ListAddValue(ttFile, LAP.NotContainsOnly)
                                                tryToConvert.Invoke(format, f)
                                                updateBitrate.Invoke(f)
                                            ElseIf Not tempFilesList.Contains(ttFile) Then
                                                tempFilesList.Add(ttFile)
                                                updateBitrate.Invoke(f)
                                            End If
                                        End If
                                    End If

                                    If PostProcessing_OutputAudioFormats.Count > 0 Then
                                        For Each format In PostProcessing_OutputAudioFormats
                                            format = format.StringToLower
                                            f = String.Format(fPattern, format)
                                            AddFile(f)
                                            If Not f.Exists Then
                                                tryToConvert.Invoke(format, f)
                                                updateBitrate(f)
                                                If format = mp3 And Not mp3ThumbEmbedded And MyYouTubeSettings.DefaultAudioEmbedThumbnail_ExtractedFiles Then _
                                                   embedThumbTo.Invoke(f) : mp3ThumbEmbedded = True
                                                If Not M3U8_PlaylistFiles.ListExists AndAlso f.Exists Then M3U8_Append(f)
                                                If format = mp3 AndAlso f.Exists AndAlso MyYouTubeSettings.VideoPlaylist_AddExtractedMP3.Value Then M3U8_Append(f)
                                            End If
                                        Next
                                    End If
                                End If

                                'mp3
                                If IsMusic And ObjectType = YouTubeMediaType.Single And File.Extension = mp3 And
                                   Not mp3ThumbEmbedded And CoverFile.Exists And MyYouTubeSettings.DefaultAudioEmbedThumbnail_Cover Then embedThumbTo.Invoke(File)

                                'Update video
                                ThrowAny(Token)
                                If SelectedVideoIndex >= 0 AndAlso tempFilesList.Count > 0 AndAlso tempFilesList.Exists(Function(tf) tf.ToReplace) Then
                                    f = File
                                    f.Name &= "tmp00"
                                    Dim tfr As SFile = tempFilesList.FirstOrDefault(Function(tf) tf.ToReplace).File
                                    If tfr.Exists And Not f.Exists Then
                                        ThrowAny(Token)
                                        .Execute($"ffmpeg -i ""{File}"" -i ""{tfr}"" -c:v copy -c copy -map 0:v:0 -map 1:a:0 ""{f}""")
                                    End If
                                    If f.Exists Then
                                        File.Delete()
                                        If SelectedVideoIndex >= 0 Then SFile.Rename(f, File,, EDP.LogMessageValue)
                                    End If
                                End If

                                'Delete unrequsted files
                                If tempFilesList.Count > 0 Then tempFilesList.ForEach(Sub(tfr) If Not tfr.Requested Then tfr.File.Delete(,, EDP.None)) : tempFilesList.Clear()

                                If SelectedVideoIndex >= 0 Then
                                    Dim reencodeFile As Action(Of String) =
                                        Sub(ByVal ffmpegCommand As String)
                                            f = File
                                            f.Name &= "tmp00"
                                            .Execute(String.Format(ffmpegCommand, File.ToString, f.ToString))
                                            If f.Exists Then
                                                If f.Size > 0 Then
                                                    File.Delete()
                                                    SFile.Rename(f, File,, EDP.LogMessageValue)
                                                Else
                                                    f.Delete(, SFODelete.DeletePermanently, EDP.None)
                                                End If
                                            End If
                                        End Sub
                                    'Change video codec to AVC
                                    If MyYouTubeSettings.DefaultVideoConvertNonAVC.Value AndAlso
                                       Not SelectedVideo.Codec.IsEmptyString AndAlso Not SelectedVideo.Codec.Trim.ToLower.StartsWith("avc") Then _
                                       reencodeFile("ffmpeg -i ""{0}"" -c:a copy -c:v libx264 ""{1}""")

                                    'Update video FPS
                                    If OutputVideoFPS > 0 AndAlso SelectedVideo.Bitrate <> OutputVideoFPS Then _
                                       reencodeFile("ffmpeg -i ""{0}"" -filter:v fps=" & OutputVideoFPS.ToString.Replace(", ", ".") & " -c:a copy ""{1}""")
                                End If
                            End If
                        End If
                    End With

                    Dim newSize# = 0
                    If File.Exists Then newSize += File.Size
                    If Files.Count > 0 Then newSize += (From eFile As SFile In Files Where eFile.Exists Select eFile.Size).Sum
                    If ThumbnailFile.Exists Then newSize += ThumbnailFile.Size
                    If newSize > 0 Then newSize /= 1024 : Size = newSize : _SizeRecalculated = True
                End Using
                _MediaState = UMStates.Downloaded
            Catch oex As OperationCanceledException When Token.IsCancellationRequested
                Throw oex
            Catch dex As ObjectDisposedException When disposedValue
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"YTContainer.DownloadError: {ToString()}".StringAppendLine(dCommand))
            Finally
                If Not Token.IsCancellationRequested And Not disposedValue Then
                    DateDownloaded = Now
                    MediaState = UMStates.Downloaded
                    If Not Progress Is Nothing Then Progress.InformationTemporary = "Download completed"
                    RaiseEvent FileDownloaded(Me, Nothing)
                End If
            End Try
        End Sub
        Private Sub M3U8_Append(Optional ByVal __file As SFile = Nothing)
            If M3U8_PlaylistFiles.ListExists Then
                For Each m3u8_file As SFile In M3U8_PlaylistFiles
                    If Not m3u8_file.IsEmptyString Then
                        Dim m3u8Row$ = String.Empty
                        If Not m3u8_file.Extension.IsEmptyString Then
                            If m3u8_file.Extension.ToLower = "m3u8" Then
                                m3u8Row = GetPlaylistRow(Me, __file)
                            ElseIf m3u8_file.Extension.ToLower = "m3u" Then
                                m3u8Row = __file.IfNullOrEmpty(File).ToString
                            End If
                        End If
                        If Not m3u8Row.IsEmptyString Then
                            Dim m3u8Text$
                            If m3u8_file.Exists Then
                                m3u8Text = m3u8_file.GetText
                                m3u8_file.Delete(SFO.File, SFODelete.DeleteToRecycleBin, EDP.SendToLog)
                            Else
                                m3u8Text = "#EXTM3U"
                            End If
                            m3u8Text.StringAppendLine(m3u8Row, vbCrLf)
                            TextSaver.SaveTextToFile(m3u8Text, m3u8_file,,, EDP.SendToLog)
                        End If
                    End If
                Next
            End If
        End Sub
#End Region
#Region "Load"
        Private Sub ApplyElementCheckedValue(ByVal e As EContainer)
            If HasElements And e.Count > 0 Then
                Dim obj As YouTubeMediaContainerBase
                For Each elem As EContainer In e
                    If Not elem.Value.IsEmptyString Then
                        obj = GetElementByID(elem.Value, True)
                        If Not obj Is Nothing Then
                            If obj.HasElements Then
                                obj.ApplyElementCheckedValue(elem)
                            Else
                                obj.Checked = elem.Attribute(Name_CheckedAttribute).Value.FromXML(Of Boolean)(True)
                            End If
                        End If
                    End If
                Next
            End If
        End Sub
        Private Function GetElementByID(ByVal ID As String, Optional ByVal IgnoreCurrentInstance As Boolean = False) As YouTubeMediaContainerBase
            If HasElements Then
                Dim obj As YouTubeMediaContainerBase
                For Each elem As YouTubeMediaContainerBase In Elements
                    If elem.ID = ID Then
                        Return elem
                    Else
                        obj = elem.GetElementByID(ID)
                        If Not obj Is Nothing Then Return obj
                    End If
                Next
            ElseIf Not IgnoreCurrentInstance And Me.ID = ID Then
                Return Me
            End If
            Return Nothing
        End Function
        Private Sub IDownloadableMedia_Load(ByVal File As String) Implements IDownloadableMedia.Load
            Load(File)
        End Sub
        Public Overridable Sub Load(ByVal f As SFile) Implements IYouTubeMediaContainer.Load
            Try
                FileSettings = f
                If f.Exists Then
                    Using x As New XmlFile(f, Protector.Modes.All, False) With {.AllowSameNames = True, .XmlReadOnly = True}
                        x.LoadData()
                        Dim fc As SFile = x.Value(Name_CachePath).CSFileP
                        If fc.Exists(SFO.Path, False) AndAlso SFile.GetFiles(fc, "*.json",, EDP.ReturnValue).Count > 0 Then Parse(Nothing, fc, IsMusic)
                        XMLPopulateData(Me, x)
                        _MediaStateOnLoad = _MediaState
                        If Me.MediaState = UMStates.Downloaded Then
                            _Exists = File.Exists(IIf(ObjectType = YouTubeMediaType.Single, SFO.File, SFO.Path), False)
                        Else
                            _Exists = True
                        End If
                        If If(x(Name_CheckedElements)?.Count, 0) > 0 Then ApplyElementCheckedValue(x(Name_CheckedElements))
                        If ArrayMaxResolution <> -10 Then SetMaxResolution(ArrayMaxResolution)
                    End Using
                Else
                    _Exists = False
                End If
            Catch ex As Exception
                _HasError = True
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"YouTubeMediaContainerBase.Load({f})")
            End Try
        End Sub
#End Region
#Region "Save"
        Protected Function NeedToSave() As Boolean
            Return Not _MediaStateOnLoad = _MediaState And Not FileSettings.Exists
        End Function
        Private Function GetThumbnails() As IEnumerable(Of SFile)
            If HasElements Then
                Return ListAddList(Of SFile)(New List(Of SFile)({ThumbnailFile}),
                                             Elements.SelectMany(Function(ee As YouTubeMediaContainerBase) ee.GetThumbnails))
            Else
                Return {ThumbnailFile}
            End If
        End Function
        Public Overridable Sub Save() Implements IDownloadableMedia.Save
            Try
                If NeedToSave() Then
                    Dim fSettings As SFile = FileSettings
                    If fSettings.IsEmptyString Then fSettings = MyCacheSettings.NewFile
                    Dim f As SFile = fSettings

                    If Not MediaState = UMStates.Downloaded Then
                        If CachePath.Exists(SFO.Path, False) AndAlso Not CachePath.Path.Contains(MyCacheSettings.RootDirectory.Path) Then
                            f = $"{f.PathWithSeparator}{f.Name}\"
                            If f.Exists(SFO.Path) Then
                                Dim files As List(Of SFile) = SFile.GetFiles(CachePath, "*.json", IO.SearchOption.AllDirectories, EDP.ReturnValue)
                                If files.ListExists Then
                                    CachePath = f
                                    Dim fd As SFile = f
                                    fd.Extension = "json"
                                    For Each f In files
                                        fd.Name = f.Name
                                        SFile.Move(f, fd)
                                    Next
                                Else
                                    If CachePath.Exists(SFO.Path, False) Then CachePath.Delete(SFO.Path, SFODelete.DeletePermanently, EDP.None)
                                    CachePath = Nothing
                                End If
                            End If
                        End If
                    Else
                        If CachePath.Exists(SFO.Path, False) Then CachePath.Delete(SFO.Path, SFODelete.DeletePermanently, EDP.None)
                        CachePath = Nothing
                        If ThumbnailFile.IsEmptyString And HasElements Then
                            With ListAddList(Nothing, GetThumbnails, LAP.NotContainsOnly).ListWithRemove(Function(tf) tf.IsEmptyString)
                                If .ListExists Then _ThumbnailFile = .FirstOrDefault(Function(tf) tf.Exists)
                            End With
                        End If
                    End If

                    Using x As New XmlFile With {.AllowSameNames = True}
                        fSettings.Extension = "xml"
                        FileSettings = fSettings
                        If NeedToSave() Then
                            x.AddRange(ToEContainer.Elements)
                            x.Name = "MediaContainer"
                            x.Save(fSettings)
                        End If
                    End Using
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"YouTubeMediaContainerBase.Save({FileSettings})")
            End Try
        End Sub
#End Region
#Region "Parse"
        Friend Const DRC As String = "drc"
        Public Overridable Function Parse(ByVal Container As EContainer, ByVal Path As SFile, ByVal IsMusic As Boolean,
                                          Optional ByVal Token As CancellationToken = Nothing, Optional ByVal Progress As IMyProgress = Nothing) As Boolean Implements IYouTubeMediaContainer.Parse
            Try
                Me.IsMusic = IsMusic
                CachePath = Path
                If Not Path.IsEmptyString And Not Container Is Nothing Then Throw New InvalidOperationException("Both arguments (Container, Path) are not null")
                If Path.IsEmptyString And Container Is Nothing Then Throw New InvalidOperationException("Both arguments (Container, Path) are null")
                If Not Path.IsEmptyString AndAlso Not Path.Exists(SFO.File, False) AndAlso Path.Exists(SFO.Path, False) Then
                    Dim files As List(Of SFile) = SFile.GetFiles(Path,, IO.SearchOption.AllDirectories, EDP.ReturnValue)
                    If files.Count > 0 Then
                        If files.Count = 1 Then
                            Path = files(0)
                        Else
                            If ParseFiles(Path, IsMusic, Token, Progress) Then
                                File = MyYouTubeSettings.OutputPath
                                Return True
                            Else
                                Return False
                            End If
                        End If
                    End If
                End If
                ThrowAny(Token)
                If Not Path.IsEmptyString AndAlso Not Path.File.IsEmptyString AndAlso Path.Exists(SFO.File, False) Then
                    Dim t$ = Path.GetText(EDP.ReturnValue)
                    If Not t.IsEmptyString Then Container = JsonDocument.Parse(t, EDP.ReturnValue)
                End If
                If Not Container Is Nothing Then
                    With Container
                        ID = .Value("id")
                        Title = TitleHtmlConverter.Invoke(.Value("title"))
                        Description = .Value("description")
                        URL = .Value("webpage_url").ToMusicUrl(IsMusic)

                        PlaylistID = .Value("playlist_id")
                        PlaylistCount = .Value("n_entries").IfNullOrEmpty(.Value("playlist_count")).FromXML(Of Integer)(0)
                        PlaylistIndex = .Value("playlist_index").FromXML(Of Integer)(-1)
                        PlaylistTitle = TitleHtmlConverter.Invoke(.Value("album").IfNullOrEmpty(.Value("playlist_title")).IfNullOrEmpty(.Value("playlist")))
                        If Not PlaylistTitle.IsEmptyString And .Value("album").IsEmptyString Then
                            Dim tmpPls$ = PlaylistTitle.Replace("Album", String.Empty).StringTrimStart(" ", "-")
                            IsShorts = Not tmpPls.IsEmptyString AndAlso PlaylistTitle.Contains(" - Shorts")
                            tmpPls = tmpPls.Replace("Shorts", String.Empty).StringTrimStart(" ", "-")
                            If Not tmpPls.IsEmptyString Then PlaylistTitle = tmpPls
                        End If

                        UserID = .Value("uploader_id")
                        UserTitle = TitleHtmlConverter.Invoke(.Value("uploader"))
                        If Not UserTitle.IsEmptyString Then
                            Dim tmpTitle$ = UserTitle.Replace("Topic", String.Empty).StringTrimEnd(" ", "-")
                            If Not tmpTitle.IsEmptyString Then UserTitle = tmpTitle
                        End If

                        Dim ext$ = IIf(IsMusic,
                                       MyYouTubeSettings.DefaultAudioCodecMusic.Value.StringToLower,
                                       MyYouTubeSettings.DefaultVideoFormat.Value.StringToLower)
                        If ext.IsEmptyString Then ext = IIf(IsMusic, "mp3", "mp4")
                        If Not Title.IsEmptyString Then
                            _File = $"{Title}.{ext}"
                        Else
                            _File.Name = $"{ID}.{ext}"
                        End If
                        If Not MyYouTubeSettings.OutputPath.IsEmptyString Then _File.Path = MyYouTubeSettings.OutputPath.Value.Path
                        _File = CleanFileName(_File)
                        File = _File

                        If .Contains("duration") Then
                            Dim tValue%? = AConvert(Of Integer)(.Value("duration"), AModes.Var, Nothing)
                            If tValue.HasValue Then Duration = TimeSpan.FromSeconds(tValue.Value)
                        End If
                        DateAdded = AConvert(Of Date)(.Value("release_date").IfNullOrEmpty(.Value("upload_date")), DateAddedProvider, New Date)
                        If Not IsMusic Then FileDateUpdate()

                        ParseFormats(.Self)

                        ParseThumbnails(.Self)

                        ParseSubtitles(.Self)
                    End With
                    Return True
                End If
                If Not Progress Is Nothing Then Progress.Perform()
                Return False
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[API.YouTube.Objects.YouTubeMediaContainerBase.Parse]")
                _HasError = True
                Return False
            End Try
        End Function
        Protected Function ParseFiles(ByVal Path As SFile, ByVal IsMusic As Boolean, Optional ByVal Token As CancellationToken = Nothing,
                                      Optional ByVal Progress As IMyProgress = Nothing) As Boolean
            Me.IsMusic = IsMusic
            CachePath = Path
            If Path.Exists(SFO.Path, False) Then
                Dim files As List(Of SFile) = SFile.GetFiles(Path,, IO.SearchOption.AllDirectories, EDP.ReturnValue)
                If files.Count > 0 Then
                    Dim progressExists As Boolean = Not Progress Is Nothing
                    Dim pErr As New ErrorsDescriber(EDP.ReturnValue)
                    Dim e As EContainer
                    Dim obj As IYouTubeMediaContainer
                    Dim t$
                    Dim playListDataObtained As Boolean = False
                    If progressExists Then Progress.Maximum += files.Count
                    For Each f As SFile In files
                        t = f.GetText(pErr)
                        If Not t.IsEmptyString Then
                            ThrowAny(Token)
                            e = JsonDocument.Parse(t, pErr)
                            If Not e Is Nothing Then
                                If e.Count > 0 Then
                                    If IsMusic Then obj = New Track Else obj = New Video
                                    ThrowAny(Token)
                                    obj.Parse(e, Nothing, IsMusic)
                                    If progressExists Then Progress.Perform()
                                    ThrowAny(Token)
                                    If obj.Exists And Not obj.HasError Then
                                        Duration += obj.Duration
                                        DirectCast(obj, YouTubeMediaContainerBase).CachePath = Path
                                        'Size += obj.Size
                                        If Not playListDataObtained Then
                                            playListDataObtained = True
                                            With obj
                                                ID = .PlaylistID
                                                Title = .PlaylistTitle
                                                Bitrate = .Bitrate
                                                UserID = .UserID
                                                UserTitle = .UserTitle
                                            End With
                                        End If
                                        Elements.Add(obj)
                                    End If
                                End If
                                e.Dispose()
                            End If
                        End If
                    Next
                    File = MyYouTubeSettings.OutputPath
                End If
            End If
            _Exists = HasElements
            Return _Exists
        End Function
        Protected Sub ParseFormats(ByVal e As EContainer)
            Const av As UMTypes = UMTypes.Audio + UMTypes.Video
            If If(e({"formats"})?.Count, 0) > 0 Then
                Dim obj As MediaObject
                Dim nValue#
                Dim sValue$
                Dim allowWebm As Boolean = MyYouTubeSettings.DefaultVideoAllowWebm
                Dim validCodecValue As Func(Of String, Boolean) = Function(codec) Not codec.IsEmptyString AndAlso Not codec = "none"

                For Each ee In e({"formats"})
                    obj = New MediaObject With {
                        .ID = ee.Value("format_id"),
                        .URL = ee.Value("url"),
                        .Extension = ee.Value("ext"),
                        .ID_DRC = Not .ID.IsEmptyString AndAlso .ID.StringToLower.Contains(DRC)
                    }
                    obj.Width = AConvert(Of Integer)(ee.Value("width"), NumberProvider, -1)
                    obj.Height = AConvert(Of Integer)(ee.Value("height"), NumberProvider, -1)
                    obj.FPS = AConvert(Of Double)(ee.Value("fps"), NumberProvider, -1)
                    obj.Bitrate = AConvert(Of Double)(ee.Value("tbr"), NumberProvider, -1)
                    obj.Protocol = ee.Value("protocol")
                    If Not obj.Protocol.IsEmptyString Then obj.Protocol = obj.Protocol.Split("_").FirstOrDefault
                    nValue = AConvert(Of Double)(ee.Value("filesize"), NumberProvider, -1)
                    If nValue > 0 Then obj.Size = (nValue / 1024).RoundVal(2)
                    If obj.Size <= 0 Then
                        nValue = AConvert(Of Double)(ee.Value("filesize_approx"), NumberProvider, -1)
                        If nValue > 0 Then obj.Size = (nValue / 1024).RoundVal(2)
                    End If
                    If obj.Size <= 0 And obj.Bitrate > 0 And Duration.TotalSeconds > 0 Then _
                       obj.Size = (obj.Bitrate / 8 * Duration.TotalSeconds).RoundVal(2)

                    sValue = ee.Value("vcodec")
                    If validCodecValue(sValue) Then
                        obj.Type = UMTypes.Video
                        obj.Codec = sValue.Split(".").First
                        If validCodecValue(ee.Value("acodec")) Then obj.Type = av
                    Else
                        sValue = ee.Value("acodec")
                        If validCodecValue(sValue) Then
                            obj.Type = UMTypes.Audio
                            obj.Codec = sValue.Split(".").First
                        Else
                            Continue For
                        End If
                    End If
                    MediaObjects.Add(obj)
                Next
                MediaObjects.RemoveAll(Function(m) (m.Type = UMTypes.Video And (m.Width <= 0 Or m.Height <= 0)) Or m.URL.IsEmptyString)
                Dim DupRemover As Action(Of UMTypes) =
                    Sub(ByVal t As UMTypes)
                        Const webm$ = "webm"
                        Const avc$ = "avc"
                        Dim data As New List(Of MediaObject)(MediaObjects.Where(Function(mo) mo.Type = t And mo.Extension = webm))
                        If data.Count > 0 Then
                            Dim d As MediaObject = Nothing
                            Dim expWebm As Predicate(Of MediaObject) = Function(mo) mo.Extension = webm
                            Dim expAVC As Predicate(Of MediaObject) = Function(mo) mo.Codec.IfNullOrEmpty("/").ToLower.StartsWith(avc)
                            Dim comp As Func(Of MediaObject, Predicate(Of MediaObject), Boolean, Boolean, Boolean) =
                                Function(mo, exp, isTrue, checkHttp) mo.Type = t And exp.Invoke(mo) = isTrue And mo.Width = d.Width And
                                                                     (Not checkHttp OrElse mo.ProtocolType = Protocols.https)
                            Dim CountWebm As Func(Of MediaObject, Boolean) = Function(mo) comp.Invoke(mo, expWebm, False, allowWebm)
                            Dim RemoveWebm As Predicate(Of MediaObject) = Function(mo) comp.Invoke(mo, expWebm, True, allowWebm)
                            Dim CountAVC As Func(Of MediaObject, Boolean) = Function(mo) comp.Invoke(mo, expAVC, True, False)
                            Dim RemoveAVC As Predicate(Of MediaObject) = Function(mo) comp.Invoke(mo, expAVC, False, False)
                            For Each d In data
                                If MediaObjects.Count = 0 Then Exit For
                                If MediaObjects.LongCount(CountWebm) > 0 Then MediaObjects.RemoveAll(RemoveWebm)
                                If MediaObjects.Count > 0 AndAlso MediaObjects.LongCount(CountAVC) > 0 Then MediaObjects.RemoveAll(RemoveAVC)
                            Next
                        End If
                        If t = UMTypes.Audio And MediaObjects.Count > 0 Then
                            Dim __audioComparerCount As Func(Of MediaObject, MediaObject, Boolean) =
                                Function(mo, mo2) (mo2.Type = t And mo2.Extension = mo.Extension And mo2.Bitrate = mo.Bitrate) AndAlso
                                                   mo2.Size.RoundDown = mo.Size.RoundDown AndAlso ACheck(Of Integer)(mo2.ID)
                            Dim RemoveDRC As Predicate(Of MediaObject) = Function(mo) mo.Type = t AndAlso Not ACheck(Of Integer)(mo.ID) AndAlso
                                                                                      MediaObjects.LongCount(Function(mo2) __audioComparerCount.Invoke(mo, mo2)) > 0
                            MediaObjects.RemoveAll(RemoveDRC)
                        End If
                    End Sub
                Dim protocolCleaner As Action =
                    Sub()
                        If Not MyYouTubeSettings.DefaultProtocol.Value = Protocols.Undefined And
                           Not MyYouTubeSettings.DefaultProtocol.Value = Protocols.Any Then
                            Dim data As New List(Of MediaObject)(MediaObjects.Where(Function(mo) mo.ProtocolType = MyYouTubeSettings.DefaultProtocol.Value))
                            If data.ListExists Then
                                Dim dRem As Protocols = IIf(MyYouTubeSettings.DefaultProtocol.Value = Protocols.https, Protocols.m3u8, Protocols.https)
                                Dim d As MediaObject
                                Dim dr As New FPredicate(Of MediaObject)(Function(mo) mo.Height = d.Height And mo.ProtocolType = dRem)
                                For Each d In data
                                    If MediaObjects.Count = 0 Then
                                        Exit For
                                    ElseIf MediaObjects.LongCount(dr) > 0 Then
                                        MediaObjects.RemoveAll(dr)
                                    End If
                                Next
                            End If
                        End If
                    End Sub
                If MediaObjects.Count > 0 And Not MyYouTubeSettings.DefaultVideoIncludeNullSize Then MediaObjects.RemoveAll(Function(mo) mo.Size <= 0)
                If MediaObjects.Count > 0 Then DupRemover.Invoke(UMTypes.Audio)
                If MediaObjects.Count > 0 Then DupRemover.Invoke(UMTypes.Video)
                If MediaObjects.Count > 0 Then protocolCleaner.Invoke
                If MediaObjects.Count > 0 Then
                    MediaObjects.Sort()
                    SelectedAudioIndex = MediaObjects.FindIndex(Function(mo) mo.Type = UMTypes.Audio)
                    If SelectedAudioIndex >= 0 Then
                        Dim aSize# = MediaObjects(SelectedAudioIndex).Size
                        If aSize > 0 Then
                            For i% = 0 To MediaObjects.Count - 1
                                obj = MediaObjects(i)
                                If obj.Type = UMTypes.Video Then obj.Size += aSize : MediaObjects(i) = obj
                            Next
                        End If
                    End If

                    With MyYouTubeSettings
                        If Not .DefaultVideoFormat.IsEmptyString Then OutputVideoExtension = .DefaultVideoFormat
                        PostProcessing_OutputAudioFormats_Reset()
                        If Not IsMusic Then
                            If .DefaultVideoDefinition > 0 Then _
                               SelectedVideoIndex = MediaObjects.FindIndex(Function(mo) mo.Type = UMTypes.Video And mo.Height <= .DefaultVideoDefinition)
                            If SelectedVideoIndex = -1 Then MediaObjects.FindIndex(Function(mo) mo.Type = UMTypes.Video)
                            If Not .DefaultAudioCodec.IsEmptyString Then OutputAudioCodec = .DefaultAudioCodec
                        Else
                            If Not .DefaultAudioCodecMusic.IsEmptyString Then OutputAudioCodec = .DefaultAudioCodecMusic
                            SelectedVideoIndex = -1
                        End If
                    End With
                    MediaObjects.ListReindex
                End If
            End If
        End Sub
        Protected Sub ParseThumbnails(ByVal e As EContainer)
            If If(e({"thumbnails"})?.Count, 0) > 0 Then
                Dim thumb As Thumbnail
                For Each ee In e({"thumbnails"})
                    thumb = New Thumbnail With {.ID = ee.Value("id"), .URL = ee.Value("url")}
                    thumb.Width = AConvert(Of Integer)(ee.Value("width"), NumberProvider, -1)
                    thumb.Height = AConvert(Of Integer)(ee.Value("height"), NumberProvider, -1)
                    If thumb.Width > 0 And thumb.Height > 0 And Not thumb.URL.IsEmptyString Then Thumbnails.Add(thumb)
                Next
                If Thumbnails.Count > 0 Then
                    Thumbnails.Sort()
                    Thumbnails.ListReindex
                    _ThumbnailUrl = Thumbnails.FirstOrDefault(Function(t) Not t.URL.Contains(".webp")).URL
                    If _ThumbnailUrl.IsEmptyString Then _ThumbnailUrl = Thumbnails.First.URL
                End If
            End If
        End Sub
        Protected Sub ParseSubtitles(ByVal e As EContainer)
            Dim subt As Subtitles
            Dim ee As EContainer
            Dim eSUB As EContainer = e({"subtitles"})
            Dim eCC As EContainer = e({"automatic_captions"})
            If If(eSUB?.Count, 0) = 0 OrElse (eSUB.Count = 1 And eSUB(0).Name = "live_chat") Then eSUB = Nothing
            If If(eCC?.Count, 0) = 0 OrElse (eCC.Count = 1 And eCC(0).Name = "live_chat") Then eCC = Nothing
            If If(eSUB?.Count, 0) > 0 Or If(eCC?.Count, 0) > 0 Then
                Dim sl As New List(Of EContainer)
                Dim ccExists As Boolean = False
                Dim ccIndx% = -1, rIndx% = -1
                If If(eSUB?.Count, 0) > 0 Then sl.Add(eSUB) : ccIndx += 1
                If If(eCC?.Count, 0) > 0 Then sl.Add(eCC) : ccIndx += 1 : ccExists = True
                For Each se As EContainer In sl
                    rIndx += 1
                    For Each ee In se
                        subt = New Subtitles With {.ID = ee.Name, .CC = rIndx = ccIndx And ccExists}
                        If ee.Count > 0 Then
                            subt.Name = ee(0).Value("name")
                            subt.Formats = ee.Select(Function(f) f.Value("ext")).ListToString(",")
                        End If
                        If Not subt.ID.IsEmptyString Then _Subtitles.Add(subt)
                    Next
                Next
                With MyYouTubeSettings
                    If Not .DefaultSubtitlesFormat.IsEmptyString Then OutputSubtitlesFormat = .DefaultSubtitlesFormat
                    If _Subtitles.Count > 0 And .DefaultSubtitles.Count > 0 Then
                        _Subtitles.Sort()
                        _Subtitles.ListReindex
                        SubtitlesSelectedIndexesReset()
                        PostProcessing_OutputSubtitlesFormats_Reset()
                    End If
                End With
            End If
        End Sub
#End Region
#Region "IEContainerProvider Support"
        Private Function GetElementsChecked() As IEnumerable(Of EContainer)
            If HasElements Then
                Return Elements.SelectMany(Function(elem As YouTubeMediaContainerBase) elem.GetElementsChecked())
            Else
                Return {New EContainer("Element", ID, {New EAttribute(Name_CheckedAttribute, Checked.BoolToInteger)}) With {.AllowSameNames = True}}
            End If
        End Function
        Public Function ToEContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
            Dim c As New EContainer("DataItems") With {.AllowSameNames = True}
            c.AddRange(XMLGenerateContainers(Me))
            If HasElements AndAlso Not Elements.All(Function(cc) cc.CheckState = CheckState.Checked) Then
                c.Add(New EContainer(Name_CheckedElements) With {.AllowSameNames = True})
                c.Last.AddRange(Of EContainer)(GetElementsChecked())
            End If
            Return ToEContainer_Addit(c)
        End Function
        Protected Overridable Function ToEContainer_Addit(ByRef Container As EContainer) As EContainer
            Return Container
        End Function
#End Region
#Region "IComparable Support"
        Protected Overridable Function CompareTo(ByVal Other As IYouTubeMediaContainer) As Integer Implements IComparable(Of IYouTubeMediaContainer).CompareTo
            If CInt(ObjectType).CompareTo(CInt(Other.ObjectType)) = 0 Then
                Select Case ObjectType
                    Case YouTubeMediaType.PlayList
                        If DateAdded.CompareTo(Other.DateAdded) = 0 Then
                            If Not PlaylistTitle.IsEmptyString AndAlso Not Other.PlaylistTitle.IsEmptyString Then
                                Return PlaylistTitle.CompareTo(Other.PlaylistTitle)
                            Else
                                Return 0
                            End If
                        Else
                            Return DateAdded.CompareTo(Other.DateAdded)
                        End If
                    Case YouTubeMediaType.Single
                        If PlaylistIndex.CompareTo(Other.PlaylistIndex) = 0 Then
                            If Not Title.IsEmptyString And Not Other.Title.IsEmptyString Then
                                Return Title.CompareTo(Other.Title)
                            Else
                                Return 0
                            End If
                        Else
                            Return PlaylistIndex.CompareTo(Other.PlaylistIndex)
                        End If
                    Case YouTubeMediaType.Channel
                        If Not Title.IsEmptyString And Not Other.Title.IsEmptyString Then
                            Return Title.CompareTo(Other.Title)
                        Else
                            Return 0
                        End If
                    Case Else : Return 0
                End Select
            Else
                Return CInt(ObjectType).CompareTo(CInt(Other.ObjectType))
            End If
        End Function
#End Region
#Region "IDisposable Support"
        Protected disposedValue As Boolean = False
        Public ReadOnly Property IsDisposed As Boolean Implements IYouTubeMediaContainer.IsDisposed
            Get
                Return disposedValue
            End Get
        End Property
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Elements.ListClearDispose
                    Thumbnails.Clear()
                    _Subtitles.Clear()
                    _SubtitlesDelegated.Clear()
                    SubtitlesSelectedIndexes.Clear()
                    MediaObjects.Clear()
                    _Files.Clear()
                    PostProcessing_OutputAudioFormats.Clear()
                    PostProcessing_OutputSubtitlesFormats.Clear()
                End If
                disposedValue = True
            End If
        End Sub
        Protected NotOverridable Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub
        Public Overloads Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace