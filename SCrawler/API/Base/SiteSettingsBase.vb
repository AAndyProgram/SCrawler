' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.WEB
Imports SCrawler.Plugin
Imports Download = SCrawler.Plugin.ISiteSettings.Download
Namespace API.Base
    Friend MustInherit Class SiteSettingsBase : Implements ISiteSettings
        Friend ReadOnly Property Site As String Implements ISiteSettings.Site
        Friend Overridable ReadOnly Property Icon As Icon Implements ISiteSettings.Icon
        Friend Overridable ReadOnly Property Image As Image Implements ISiteSettings.Image
        Private Property Logger As ILogProvider = LogConnector Implements ISiteSettings.Logger
        Friend Overridable ReadOnly Property Responser As Response
        Friend MustOverride Function GetInstance(ByVal What As Download) As IPluginContentProvider Implements ISiteSettings.GetInstance
        Friend Sub New(ByVal SiteName As String)
            Site = SiteName
        End Sub
        Friend Sub New(ByVal SiteName As String, ByVal CookiesDomain As String)
            Site = SiteName
            Responser = New Response($"{SettingsFolderName}\Responser_{Site}.xml")
            With Responser
                If .File.Exists Then .LoadSettings() Else .CookiesDomain = CookiesDomain : .SaveSettings()
            End With
        End Sub
#Region "XML"
        Friend Overridable Sub Load(ByVal XMLValues As IEnumerable(Of KeyValuePair(Of String, String))) Implements ISiteSettings.Load
        End Sub
#End Region
#Region "Initialize"
        Friend Overridable Sub BeginInit() Implements ISiteSettings.BeginInit
        End Sub
        Friend Overridable Sub EndInit() Implements ISiteSettings.EndInit
        End Sub
        Friend Overridable Sub BeginUpdate() Implements ISiteSettings.BeginUpdate
        End Sub
        Friend Overridable Sub EndUpdate() Implements ISiteSettings.EndUpdate
        End Sub
#End Region
#Region "Before and After Download"
        Friend Overridable Sub DownloadStarted(ByVal What As Download) Implements ISiteSettings.DownloadStarted
        End Sub
        Friend Overridable Sub BeforeStartDownload(ByVal User As Object, ByVal What As Download) Implements ISiteSettings.BeforeStartDownload
        End Sub
        Friend Overridable Sub AfterDownload(ByVal User As Object, ByVal What As Download) Implements ISiteSettings.AfterDownload
        End Sub
        Friend Overridable Sub DownloadDone(ByVal What As Download) Implements ISiteSettings.DownloadDone
        End Sub
#End Region
#Region "User info"
        Protected UrlPatternUser As String = String.Empty
        Protected UrlPatternChannel As String = String.Empty
        Friend Overridable Function GetUserUrl(ByVal UserName As String, ByVal Channel As Boolean) As String Implements ISiteSettings.GetUserUrl
            If Channel Then
                If Not UrlPatternChannel.IsEmptyString Then Return String.Format(UrlPatternChannel, UserName)
            Else
                If Not UrlPatternUser.IsEmptyString Then Return String.Format(UrlPatternUser, UserName)
            End If
            Return String.Empty
        End Function
        Protected UserRegex As RParams = Nothing
        Friend Overridable Function IsMyUser(ByVal UserURL As String) As ExchangeOptions Implements ISiteSettings.IsMyUser
            Try
                If Not UserRegex Is Nothing Then
                    Dim s$ = RegexReplace(UserURL, UserRegex)
                    If Not s.IsEmptyString Then Return New ExchangeOptions(Site, s)
                End If
                Return Nothing
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendInLog + EDP.ReturnValue, ex, "[API.Base.SiteSettingsBase.IsMyUser]")
            End Try
        End Function
        Protected ImageVideoContains As String = String.Empty
        Friend Overridable Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions Implements ISiteSettings.IsMyImageVideo
            If Not ImageVideoContains.IsEmptyString AndAlso URL.Contains(ImageVideoContains) Then
                Return New ExchangeOptions With {.Exists = True}
            Else
                Return Nothing
            End If
        End Function
        Friend Overridable Function GetSpecialData(ByVal URL As String, ByVal Path As String, ByVal AskForPath As Boolean) As IEnumerable(Of PluginUserMedia) Implements ISiteSettings.GetSpecialData
            Return Nothing
        End Function
        Friend Overridable Function GetSpecialDataF(ByVal URL As String) As IEnumerable(Of UserMedia)
            Return Nothing
        End Function
#End Region
#Region "Ready, Available"
        Friend Overridable Function Available(ByVal What As Download, ByVal Silent As Boolean) As Boolean Implements ISiteSettings.Available
            Return True
        End Function
        Friend Overridable Function ReadyToDownload(ByVal What As Download) As Boolean Implements ISiteSettings.ReadyToDownload
            Return True
        End Function
#End Region
        Friend Overridable Sub Update() Implements ISiteSettings.Update
            If Not Responser Is Nothing Then Responser.SaveSettings()
        End Sub
        Friend Overridable Sub Reset() Implements ISiteSettings.Reset
        End Sub
        Friend Overridable Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean) Implements ISiteSettings.UserOptions
            Options = Nothing
        End Sub
        Friend Overridable Sub OpenSettingsForm() Implements ISiteSettings.OpenSettingsForm
        End Sub
    End Class
End Namespace