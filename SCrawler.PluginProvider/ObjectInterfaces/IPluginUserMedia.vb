' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace Plugin
    Public Structure PluginUserMedia : Implements IPluginUserMedia
        Public Property ContentType As Integer Implements IPluginUserMedia.ContentType
        Public Property URL As String Implements IPluginUserMedia.URL
        Public Property MD5 As String Implements IPluginUserMedia.MD5
        Public Property File As String Implements IPluginUserMedia.File
        Public Property DownloadState As Integer Implements IPluginUserMedia.DownloadState
        Public Property PostID As String Implements IPluginUserMedia.PostID
        Public Property PostDate As Date? Implements IPluginUserMedia.PostDate
        Public Property SpecialFolder As String Implements IPluginUserMedia.SpecialFolder
    End Structure
    Public Interface IPluginUserMedia
        Enum Types As Integer
            Undefined = 0
            [Picture] = 1
            [Video] = 2
            [Text] = 3
            VideoPre = 10
            GIF = 50
            m3u8 = 100
        End Enum
        Enum States As Integer : Unknown = 0 : Tried = 1 : Downloaded = 2 : Skipped = 3 : End Enum
        Property ContentType As Integer
        Property URL As String
        Property MD5 As String
        Property File As String
        Property DownloadState As Integer
        Property PostID As String
        Property PostDate As Date?
        Property SpecialFolder As String
    End Interface
End Namespace