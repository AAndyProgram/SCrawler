' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Drawing
Namespace Plugin
    Public Interface ISiteSettings
        Enum Download As Integer
            Main = 0
            SavedPosts = 1
            Channel = 2
        End Enum
        ReadOnly Property Icon As Icon
        ReadOnly Property Image As Image
        ReadOnly Property Site As String
        Property Logger As ILogProvider
        Function GetUserUrl(ByVal UserName As String, ByVal Channel As Boolean) As String
        Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
        Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions
        Function GetSpecialData(ByVal URL As String, ByVal Path As String, ByVal AskForPath As Boolean) As IEnumerable(Of PluginUserMedia)
        Function GetInstance(ByVal What As Download) As IPluginContentProvider
        Function GetUserPostUrl(ByVal UserID As String, ByVal PostID As String) As String
#Region "XML Support"
        Sub Load(ByVal XMLValues As IEnumerable(Of KeyValuePair(Of String, String)))
#End Region
#Region "Initialization"
        Sub BeginInit()
        Sub EndInit()
        Sub BeginUpdate()
        Sub EndUpdate()
#End Region
#Region "Site availability"
        Function Available(ByVal What As Download, ByVal Silent As Boolean) As Boolean
        Function ReadyToDownload(ByVal What As Download) As Boolean
#End Region
#Region "Downloading"
        Sub DownloadStarted(ByVal What As Download)
        Sub BeforeStartDownload(ByVal User As Object, ByVal What As Download)
        Sub AfterDownload(ByVal User As Object, ByVal What As Download)
        Sub DownloadDone(ByVal What As Download)
#End Region
        Sub Update()
        Sub Reset()
        Sub OpenSettingsForm()
        Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
    End Interface
End Namespace