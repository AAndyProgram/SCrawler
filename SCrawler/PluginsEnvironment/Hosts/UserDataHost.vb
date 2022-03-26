' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML
Imports SCrawler.API.Base
Imports System.Threading
Imports System.Reflection
Imports UStates = SCrawler.Plugin.PluginUserMedia.States
Imports UTypes = SCrawler.Plugin.PluginUserMedia.Types
Namespace Plugin.Hosts
    Friend Class UserDataHost : Inherits UserDataBase
        Private ReadOnly UseInternalDownloader As Boolean
        Friend Overrides Function ExchangeOptionsGet() As Object
            Return ExternalPlugin.ExchangeOptionsGet
        End Function
        Friend Overrides Sub ExchangeOptionsSet(ByVal Obj As Object)
            ExternalPlugin.ExchangeOptionsSet(Obj)
        End Sub
        Friend Sub New(ByVal SourceClass As IPluginContentProvider)
            ExternalPlugin = SourceClass
            UseInternalDownloader = Not ExternalPlugin.GetType.GetCustomAttribute(Of Attributes.UseInternalDownloader)() Is Nothing
            AddHandler ExternalPlugin.ProgressChanged, AddressOf ExternalPlugin_ProgressChanged
            AddHandler ExternalPlugin.TotalCountChanged, AddressOf ExternalPlugin_TotalCountChanged
        End Sub
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            If Loading Then
                ExternalPlugin.XmlFieldsSet(ToKeyValuePair(Of String, EContainer)(Container))
            Else
                Dim fl As List(Of KeyValuePair(Of String, String)) = ExternalPlugin.XmlFieldsGet
                If fl.ListExists Then
                    For Each fle As KeyValuePair(Of String, String) In fl : Container.Add(fle.Key, fle.Value) : Next
                    fl.Clear()
                End If
            End If
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            With ExternalPlugin
                .Settings = HOST.Source
                .Thrower = Me
                .LogProvider = LogConnector
                .Name = Name
                .ID = ID
                .ParseUserMediaOnly = ParseUserMediaOnly
                .UserDescription = UserDescription
                .UserExists = .UserExists
                .UserSuspended = UserSuspended
                .IsSavedPosts = IsSavedPosts
                .SeparateVideoFolder = SeparateVideoFolderF
                .DataPath = MyFile.CutPath.PathNoSeparator
                .PostsNumberLimit = DownloadTopCount
                .PostsDateLimit = DownloadToDate

                .ExistingContentList = New List(Of PluginUserMedia)
                .TempMediaList = New List(Of PluginUserMedia)
                .TempPostsList = New List(Of String)

                If _ContentList.Count > 0 Then ExternalPlugin.ExistingContentList = _ContentList.Select(Function(u) u.PluginUserMedia).ToList
                ExternalPlugin.TempPostsList = ListAddList(Nothing, _TempPostsList)

                .GetMedia()

                _TempPostsList.ListAddList(.TempPostsList, LNC)
                If .TempMediaList.ListExists Then _TempMediaList.ListAddList(.TempMediaList.Select(Function(tm) New UserMedia(tm)), LNC)

                If Not .Name = Name Then Name = .Name
                ID = .ID
                UserDescriptionUpdate(.UserDescription)
                UserExists = .UserExists
                UserSuspended = .UserSuspended
            End With
        End Sub
        Protected Overrides Sub ReparseVideo(ByVal Token As CancellationToken)
        End Sub
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            If UseInternalDownloader Then
                DownloadContentDefault(Token)
            Else
                With ExternalPlugin
                    If .TempMediaList.ListExists Then .TempMediaList.Clear()
                    .TempMediaList = New List(Of PluginUserMedia)
                    .TempMediaList.ListAddList(_ContentNew.Select(Function(c) c.PluginUserMedia()))
                    .Download()
                    _ContentNew.Clear()
                    If .TempMediaList.ListExists Then
                        _ContentNew.ListAddList(.TempMediaList.Select(Function(c) New UserMedia(c)))
                        DownloadedPictures(False) = .TempMediaList.LongCount(Function(m) m.DownloadState = UStates.Downloaded And
                                                                                         (m.ContentType = UTypes.Picture Or m.ContentType = UTypes.GIF))
                        DownloadedVideos(False) = .TempMediaList.LongCount(Function(m) m.DownloadState = UStates.Downloaded And
                                                                                       (m.ContentType = UTypes.Video Or m.ContentType = UTypes.m3u8))
                    End If
                End With
            End If
        End Sub
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False) As Integer
            LogError(ex, Message)
            HasError = True
            Return 0
        End Function
        Private Sub ExternalPlugin_ProgressChanged(ByVal Count As Integer)
            Progress.Perform(Count)
        End Sub
        Private Sub ExternalPlugin_TotalCountChanged(ByVal Count As Integer)
            Progress.TotalCount += Count
        End Sub
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing And Not disposedValue Then
                With ExternalPlugin
                    If .ExistingContentList.ListExists Then .ExistingContentList.Clear()
                    If .TempMediaList.ListExists Then .TempMediaList.Clear()
                    If .TempPostsList.ListExists Then .TempPostsList.Clear()
                    .Dispose()
                End With
            End If
            MyBase.Dispose(disposing)
        End Sub
    End Class
End Namespace