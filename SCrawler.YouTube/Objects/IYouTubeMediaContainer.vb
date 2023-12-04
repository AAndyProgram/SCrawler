' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin
Imports SCrawler.API.YouTube.Base
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Forms.Toolbars
Imports UMTypes = SCrawler.Plugin.UserMediaTypes
Imports UMStates = SCrawler.Plugin.UserMediaStates
Namespace API.YouTube.Objects
    Public Interface IYouTubeMediaContainer : Inherits IDownloadableMedia, IEContainerProvider, IComparable(Of IYouTubeMediaContainer)
#Region "Events"
        Event FileDownloaded As EventHandler
        Event FileDownloadStarted As EventHandler
        Event DataDownloaded As EventHandler
#End Region
#Region "Base data"
        ReadOnly Property ObjectType As YouTubeMediaType
        ReadOnly Property MediaType As UMTypes
        ReadOnly Property MediaState As UMStates
        Property IsMusic As Boolean
        Property IsShorts As Boolean
        Property ID As String
        Property Description As String
        Property PlaylistID As String
        Property PlaylistTitle As String
        Property UserID As String
        Property UserTitle As String
#End Region
#Region "Playlist support"
        ReadOnly Property Elements As List(Of IYouTubeMediaContainer)
        ReadOnly Property HasElements As Boolean
        ReadOnly Property Count As Integer
        Property PlaylistIndex As Integer
#End Region
#Region "Data info"
#Region "Thumbnails"
        ReadOnly Property Thumbnails As List(Of Thumbnail)
        ReadOnly Property ThumbnailUrlMedia As String
        Overloads ReadOnly Property ThumbnailFile As SFile
#End Region
#Region "Subtitles"
        ReadOnly Property Subtitles As List(Of Subtitles)
        ReadOnly Property SubtitlesSelectedIndexes As List(Of Integer)
#End Region
#Region "MediaObjects"
        ReadOnly Property MediaObjects As List(Of MediaObject)
        Property SelectedAudioIndex As Integer
        Property SelectedVideoIndex As Integer
#End Region
        ReadOnly Property SizeStr As String
        Property Height As Integer
        Property Bitrate As Integer
        Property DateCreated As Date
        Property DateAdded As Date
        Property DateDownloaded As Date
        Property OutputVideoExtension As String
        Property OutputAudioCodec As String
        Property OutputSubtitlesFormat As String
#End Region
#Region "HasError, Exists, Checked"
        ReadOnly Property CheckState As CheckState
#End Region
#Region "URL, File, Command"
        Overloads Property File As SFile
        ReadOnly Property Files As List(Of SFile)
        Property UseCookies As Boolean
        ReadOnly Property Command(ByVal WithCookies As Boolean) As String
        Sub UpdateInfoFields()
#End Region
#Region "Download"
        Overloads Property Progress As MyProgress
#End Region
#Region "Parse, Load"
        Overloads Sub Load(ByVal f As SFile)
        Overloads Function Parse(ByVal Container As EContainer, ByVal Path As SFile, ByVal IsMusic As Boolean,
                                 Optional ByVal Token As Threading.CancellationToken = Nothing, Optional ByVal Progress As IMyProgress = Nothing) As Boolean
#End Region
#Region "IDisposable"
        ReadOnly Property IsDisposed As Boolean
#End Region
    End Interface
End Namespace