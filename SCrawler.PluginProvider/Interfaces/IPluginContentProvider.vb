' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace Plugin
    Public Interface IPluginContentProvider : Inherits IDisposable
        Event ProgressChanged(ByVal Value As Integer)
        Event ProgressMaximumChanged(ByVal Value As Integer, ByVal Add As Boolean)
        Event ProgressPreChanged As ProgressChangedEventHandler
        Event ProgressPreMaximumChanged As ProgressMaximumChangedEventHandler
        Property Thrower As IThrower
        Property LogProvider As ILogProvider
        Property Settings As ISiteSettings
        Property AccountName As String
        Property Name As String
        Property NameTrue As String
        Property ID As String
        Property Options As String
        Property ParseUserMediaOnly As Boolean
        Property UserDescription As String
        Property ExistingContentList As List(Of IUserMedia)
        Property TempPostsList As List(Of String)
        Property TempMediaList As List(Of IUserMedia)
        Property UserExists As Boolean
        Property UserSuspended As Boolean
        Property IsSavedPosts As Boolean
        Property IsSubscription As Boolean
        Property SeparateVideoFolder As Boolean
        Property DataPath As String
        Property PostsNumberLimit As Integer?
        Property DownloadDateFrom As Date?
        Property DownloadDateTo As Date?
        Function ExchangeOptionsGet() As Object
        Sub ExchangeOptionsSet(ByVal Obj As Object)
        Sub XmlFieldsSet(ByVal Fields As List(Of KeyValuePair(Of String, String)))
        Function XmlFieldsGet() As List(Of KeyValuePair(Of String, String))
        Sub GetMedia(ByVal Token As Threading.CancellationToken)
        Sub Download(ByVal Token As Threading.CancellationToken)
        Sub DownloadSingleObject(ByVal Data As IDownloadableMedia, ByVal Token As Threading.CancellationToken)
        Sub ResetHistoryData()
    End Interface
End Namespace