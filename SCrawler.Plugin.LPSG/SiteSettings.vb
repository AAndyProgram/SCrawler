' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Drawing
Imports PersonalUtilities.Functions.RegularExpressions
<Manifest("AndyProgram_LPSG")>
Public Class SiteSettings : Implements ISiteSettings
    Public ReadOnly Property Icon As Icon Implements ISiteSettings.Icon
        Get
            Return My.Resources.Icon32
        End Get
    End Property
    Public ReadOnly Property Image As Image Implements ISiteSettings.Image
        Get
            Return My.Resources.IconPic_32
        End Get
    End Property
    Public ReadOnly Property Site As String = "LPSG" Implements ISiteSettings.Site
    Public Property Logger As ILogProvider Implements ISiteSettings.Logger
    Friend ReadOnly Property Responser As Response
    Public Sub New()
        Responser = New Response($"Settings\Responser_LPSG.xml")
        With Responser
            If .File.Exists Then
                .LoadSettings()
            Else
                .CookiesDomain = "www.lpsg.com"
                .Cookies = New CookieKeeper(.CookiesDomain)
            End If
        End With
    End Sub
    Public Sub BeginInit() Implements ISiteSettings.BeginInit
    End Sub
    Public Sub EndInit() Implements ISiteSettings.EndInit
    End Sub
    Public Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider Implements ISiteSettings.GetInstance
        Return New UserData
    End Function
    Public Sub Load(ByVal XMLValues As IEnumerable(Of KeyValuePair(Of String, String))) Implements ISiteSettings.Load
    End Sub
#Region "Download functions"
    Public Sub DownloadStarted(ByVal What As ISiteSettings.Download) Implements ISiteSettings.DownloadStarted
    End Sub
    Public Sub BeforeStartDownload(ByVal User As Object, ByVal What As ISiteSettings.Download) Implements ISiteSettings.BeforeStartDownload
    End Sub
    Public Sub AfterDownload(ByVal User As Object, ByVal What As ISiteSettings.Download) Implements ISiteSettings.AfterDownload
    End Sub
    Public Sub DownloadDone(ByVal What As ISiteSettings.Download) Implements ISiteSettings.DownloadDone
    End Sub
#End Region
#Region "Update"
    Public Sub BeginEdit() Implements ISiteSettings.BeginEdit
    End Sub
    Public Sub EndEdit() Implements ISiteSettings.EndEdit
    End Sub
    Public Sub BeginUpdate() Implements ISiteSettings.BeginUpdate
    End Sub
    Public Sub EndUpdate() Implements ISiteSettings.EndUpdate
    End Sub
    Public Sub Update() Implements ISiteSettings.Update
        Responser.SaveSettings()
    End Sub
#End Region
    Public Sub Reset() Implements ISiteSettings.Reset
    End Sub
    Public Sub OpenSettingsForm() Implements ISiteSettings.OpenSettingsForm
    End Sub
    Public Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean) Implements ISiteSettings.UserOptions
        Options = Nothing
    End Sub
    Public Function GetUserUrl(ByVal UserName As String, ByVal Channel As Boolean) As String Implements ISiteSettings.GetUserUrl
        Return $"https://www.lpsg.com/threads/{UserName}/"
    End Function
    Private ReadOnly UserRegEx As RParams = RParams.DMS(".+?lpsg.com/threads/([^/]+)", 1)
    Public Function IsMyUser(ByVal UserURL As String) As ExchangeOptions Implements ISiteSettings.IsMyUser
        Try
            Dim r$ = RegexReplace(UserURL, UserRegEx)
            If Not r.IsEmptyString Then
                Return New ExchangeOptions(Site, r)
            Else
                Return Nothing
            End If
        Catch
            Return Nothing
        End Try
    End Function
    Public Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions Implements ISiteSettings.IsMyImageVideo
        Return Nothing
    End Function
    Public Function GetSpecialData(ByVal URL As String, ByVal Path As String, ByVal AskForPath As Boolean) As IEnumerable(Of PluginUserMedia) Implements ISiteSettings.GetSpecialData
        Return Nothing
    End Function
    Public Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean Implements ISiteSettings.Available
        Return If(Responser.Cookies?.Count, 0) > 0
    End Function
    Public Function ReadyToDownload(ByVal What As ISiteSettings.Download) As Boolean Implements ISiteSettings.ReadyToDownload
        Return True
    End Function
    Public Function GetUserPostUrl(ByVal UserID As String, ByVal PostID As String) As String Implements ISiteSettings.GetUserPostUrl
        Return String.Empty
    End Function
End Class