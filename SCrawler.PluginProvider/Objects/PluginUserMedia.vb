' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace Plugin
    Public Delegate Sub ProgressChange(ByVal Value As Double?, ByVal Maximum As Double?, ByVal Information As String)
    Public Enum UserMediaTypes As Integer
        Undefined = 0
        Picture = 1
        Video = 2
        Audio = 200
        Text = 4
        VideoPre = 10
        AudioPre = 215
        GIF = 50
        m3u8 = 100
    End Enum
    Public Enum UserMediaStates As Integer
        Unknown = 0
        Tried = 1
        Downloaded = 2
        Skipped = 3
        Missing = 4
    End Enum
    Public Structure PluginUserMedia : Implements IUserMedia
        Public Property ContentType As UserMediaTypes Implements IUserMedia.ContentType
        Public Property URL As String Implements IUserMedia.URL
        Public Property URL_BASE As String Implements IUserMedia.URL_BASE
        Public Property MD5 As String Implements IUserMedia.MD5
        Public Property File As String Implements IUserMedia.File
        Public Property DownloadState As UserMediaStates Implements IUserMedia.DownloadState
        Public Property PostID As String Implements IUserMedia.PostID
        Public Property PostDate As Date? Implements IUserMedia.PostDate
        Public Property SpecialFolder As String Implements IUserMedia.SpecialFolder
        Public Property Attempts As Integer Implements IUserMedia.Attempts
        Public Property [Object] As Object Implements IUserMedia.Object
    End Structure
    Public Interface IUserMedia
        Property ContentType As UserMediaTypes
        Property URL As String
        Property URL_BASE As String
        Property MD5 As String
        Property File As String
        Property DownloadState As UserMediaStates
        Property PostID As String
        Property PostDate As Date?
        Property SpecialFolder As String
        Property Attempts As Integer
        Property [Object] As Object
    End Interface
End Namespace