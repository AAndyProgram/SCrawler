' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports PersonalUtilities.Functions.XML
Imports System.Threading
Imports System.Reflection
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
                ExternalPlugin.XmlFieldsSet(Container.ToKeyValuePair)
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

                If _ContentList.Count > 0 Then ExternalPlugin.ExistingContentList = _ContentList.Select(Function(u) u.PluginUserMedia).ToList
                ExternalPlugin.TempPostsList = ListAddList(Nothing, _TempPostsList)

                .GetMedia()

                If .TempMediaList.ListExists Then _TempMediaList.ListAddList(.TempMediaList.Select(Function(tm) New UserMedia(tm)), LNC)

                If Not .Name = Name Then Name = .Name
                ID = .ID
                UserDescription = .UserDescription
                UserExists = .UserExists
                UserSuspended = .UserSuspended
            End With
        End Sub
        Protected Overrides Sub ReparseVideo(ByVal Token As CancellationToken)
        End Sub
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            If UseInternalDownloader Then DownloadContentDefault(Token) Else ExternalPlugin.Download()
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
    End Class
End Namespace