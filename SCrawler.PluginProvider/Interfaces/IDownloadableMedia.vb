' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace Plugin
    Public Interface IDownloadableMedia : Inherits IUserMedia, IDisposable
        Event CheckedChange As EventHandler
        Event ThumbnailChanged As EventHandler
        Event StateChanged As EventHandler
        ReadOnly Property SiteIcon As Drawing.Image
        ReadOnly Property Site As String
        ReadOnly Property SiteKey As String
        Property AccountName As String
        Property ThumbnailUrl As String
        Property ThumbnailFile As String
        Property Title As String
        Property Size As Integer
        Property Duration As TimeSpan
        Property Progress As Object
        ReadOnly Property HasError As Boolean
        ReadOnly Property Exists As Boolean
        Property Checked As Boolean
        Property Instance As IPluginContentProvider
        Sub Download(ByVal UseCookies As Boolean, ByVal Token As Threading.CancellationToken)
        Sub Delete(ByVal RemoveFiles As Boolean)
        Sub Load(ByVal File As String)
        Sub Save()
        Overloads Function ToString() As String
        Overloads Function ToString(ByVal ForMediaItem As Boolean) As String
    End Interface
End Namespace