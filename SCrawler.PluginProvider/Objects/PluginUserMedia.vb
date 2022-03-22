' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace Plugin
    Public Structure PluginUserMedia
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
        Public ContentType As Integer
        Public URL As String
        Public MD5 As String
        Public File As String
        Public DownloadState As Integer
        Public PostID As String
        Public PostDate As Date?
        Public SpecialFolder As String
    End Structure
End Namespace