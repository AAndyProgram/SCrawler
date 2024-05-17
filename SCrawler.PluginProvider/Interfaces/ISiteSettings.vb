' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Drawing
Namespace Plugin
    Public Interface ISiteSettings : Inherits IDisposable
        Enum Download As Integer
            Main = 0
            SavedPosts = 1
            SingleObject = 2
        End Enum
        ReadOnly Property Icon As Icon
        ReadOnly Property Image As Image
        ReadOnly Property Site As String
        Property CMDEncoding As String
        Property EnvironmentPrograms As IEnumerable(Of String)
        Property UserAgentDefault As String
        Sub EnvironmentProgramsUpdated()
        Property AccountName As String
        Property Temporary As Boolean
        Property DefaultInstance As ISiteSettings
        ReadOnly Property SubscriptionsAllowed As Boolean
        Property Logger As ILogProvider
        Function GetUserUrl(ByVal User As IPluginContentProvider) As String
        Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
        Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions
        Function GetInstance(ByVal What As Download) As IPluginContentProvider
        Function GetSingleMediaInstance(ByVal URL As String, ByVal OutputFile As String) As IDownloadableMedia
        Function GetUserPostUrl(ByVal User As IPluginContentProvider, ByVal Media As IUserMedia) As String
#Region "Initialization"
        Sub BeginInit()
        Sub EndInit()
        Sub BeginUpdate()
        Sub EndUpdate()
        Sub BeginEdit()
        Sub EndEdit()
#End Region
#Region "Site availability"
        Property AvailableText As String
        Function Available(ByVal What As Download, ByVal Silent As Boolean) As Boolean
        Function ReadyToDownload(ByVal What As Download) As Boolean
#End Region
#Region "Downloading"
        Sub DownloadStarted(ByVal What As Download)
        Sub BeforeStartDownload(ByVal User As Object, ByVal What As Download)
        Sub AfterDownload(ByVal User As Object, ByVal What As Download)
        Sub DownloadDone(ByVal What As Download)
#End Region
        Function Clone(ByVal Full As Boolean) As ISiteSettings
        Sub Delete()
        Overloads Sub Update()
        Overloads Sub Update(ByVal Source As ISiteSettings)
        Sub Reset()
        Sub OpenSettingsForm()
        Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
    End Interface
End Namespace