' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports SCrawler.Plugin.Hosts
Namespace API.Base
    Friend Interface IUserData : Inherits IComparable(Of UserDataBase), IComparable, IEquatable(Of UserDataBase), IIndexable, IDisposable
        Event UserUpdated(ByVal User As IUserData)
        Enum EraseMode As Integer
            None = 0
            Data = 1
            History = 2
        End Enum
        ReadOnly Property Site As String
        ReadOnly Property Name As String
        Property NameTrue As String
        Property ID As String
        Property Options As String
        Property FriendlyName As String
        Property Description As String
        Property Favorite As Boolean
        Property Temporary As Boolean
        Property BackColor As Color?
        Property ForeColor As Color?
        Sub OpenSite(Optional ByVal e As ErrorsDescriber = Nothing)
        Sub DownloadData(ByVal Token As CancellationToken)
        Sub DownloadSingleObject(ByVal Data As YouTube.Objects.IYouTubeMediaContainer, ByVal Token As CancellationToken)
        Property ParseUserMediaOnly As Boolean
        ReadOnly Property IsSubscription As Boolean
        ReadOnly Property IsUser As Boolean
#Region "Images"
        Function GetPicture() As Image
        Sub SetPicture(ByVal f As SFile)
#End Region
#Region "Collection support"
        ReadOnly Property IsCollection As Boolean
        ReadOnly Property CollectionName As String
        ReadOnly Property CollectionPath As SFile
        ReadOnly Property IncludedInCollection As Boolean
        ReadOnly Property UserModel As UsageModel
        ReadOnly Property CollectionModel As UsageModel
        ReadOnly Property IsVirtual As Boolean
        ReadOnly Property Labels As List(Of String)
#End Region
        Property Exists As Boolean
        Property Suspended As Boolean
        Property ReadyForDownload As Boolean
        Property HOST As SettingsHost
        Property HostStatic As Boolean
        Property AccountName As String
        Property [File] As SFile
        Property FileExists As Boolean
        Property DownloadedPictures(ByVal Total As Boolean) As Integer
        Property DownloadedVideos(ByVal Total As Boolean) As Integer
        ReadOnly Property DownloadedTotal(Optional ByVal Total As Boolean = True) As Integer
        ReadOnly Property DownloadedInformation As String
        Property HasError As Boolean
        ReadOnly Property Key As String
        Property DownloadImages As Boolean
        Property DownloadVideos As Boolean
        Property DownloadMissingOnly As Boolean
        Property ScriptUse As Boolean
        Property ScriptData As String
        Function GetLVI(ByVal Destination As ListView) As ListViewItem
        Function GetLVIGroup(ByVal Destination As ListView) As ListViewGroup
        Sub LoadUserInformation()
        Sub UpdateUserInformation()
        ''' <summary>
        ''' 0 - Nothing removed<br/>
        ''' 1 - User removed<br/>
        ''' 2 - Collection removed<br/>
        ''' 3 - Collection split
        ''' </summary>
        Function Delete(Optional ByVal Multiple As Boolean = False, Optional ByVal CollectionValue As Integer = -1) As Integer
        Function EraseData(ByVal Mode As EraseMode) As Boolean
        Function MoveFiles(ByVal CollectionName As String, ByVal SpecialCollectionPath As SFile, Optional ByVal NewUser As SplitCollectionUserInfo? = Nothing) As Boolean
        Function CopyFiles(ByVal DestinationPath As SFile, Optional ByVal e As ErrorsDescriber = Nothing) As Boolean
        Sub OpenFolder()
        Property DownloadTopCount As Integer?
        Property DownloadDateFrom As Date?
        Property DownloadDateTo As Date?
        Sub SetEnvironment(ByRef h As SettingsHost, ByVal u As UserInfo, ByVal _LoadUserInformation As Boolean,
                           Optional ByVal AttachUserInfo As Boolean = True)
        ReadOnly Property Disposed As Boolean
    End Interface
End Namespace