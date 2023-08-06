' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Functions.RegularExpressions
Imports Download = SCrawler.Plugin.ISiteSettings.Download
Namespace API.Base
    Friend MustInherit Class SiteSettingsBase : Implements ISiteSettings, IResponserContainer
        Friend ReadOnly Property Site As String Implements ISiteSettings.Site
        Friend Overridable ReadOnly Property Icon As Icon Implements ISiteSettings.Icon
        Friend Overridable ReadOnly Property Image As Image Implements ISiteSettings.Image
        Protected _AllowUserAgentUpdate As Boolean = True
        Protected _SubscriptionsAllowed As Boolean = False
        Friend ReadOnly Property SubscriptionsAllowed As Boolean Implements ISiteSettings.SubscriptionsAllowed
            Get
                Return _SubscriptionsAllowed
            End Get
        End Property
        Private Property Logger As ILogProvider = LogConnector Implements ISiteSettings.Logger
        Friend Overridable ReadOnly Property Responser As Responser
        Friend ReadOnly Property CookiesNetscapeFile As SFile
        Protected CheckNetscapeCookiesOnEndInit As Boolean = False
        Private _UseNetscapeCookies As Boolean = False
        Protected Property UseNetscapeCookies As Boolean
            Get
                Return _UseNetscapeCookies
            End Get
            Set(ByVal use As Boolean)
                Dim b As Boolean = Not _UseNetscapeCookies = use
                _UseNetscapeCookies = use
                If Not Responser Is Nothing Then
                    Responser.Cookies.ChangedAllowInternalDrop = Not _UseNetscapeCookies
                    Responser.Cookies.Changed = False
                End If
                If b And _UseNetscapeCookies Then Update_SaveCookiesNetscape()
            End Set
        End Property
        Private Property IResponserContainer_Responser As Responser Implements IResponserContainer.Responser
            Get
                Return Responser
            End Get
            Set : End Set
        End Property
        Friend MustOverride Function GetInstance(ByVal What As Download) As IPluginContentProvider Implements ISiteSettings.GetInstance
        Friend Sub New(ByVal SiteName As String)
            Site = SiteName
            CookiesNetscapeFile = $"{SettingsFolderName}\Responser_{Site}_Cookies_Netscape.txt"
        End Sub
        Friend Sub New(ByVal SiteName As String, ByVal CookiesDomain As String)
            Me.New(SiteName)
            Responser = New Responser($"{SettingsFolderName}\Responser_{Site}.xml") With {.DeclaredError = EDP.ThrowException}
            With Responser
                .CookiesDomain = CookiesDomain
                .CookiesEncryptKey = SettingsCLS.CookieEncryptKey
                If .File.Exists Then .LoadSettings() Else .SaveSettings()
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
            If _AllowUserAgentUpdate And Not DefaultUserAgent.IsEmptyString And Not Responser Is Nothing Then Responser.UserAgent = DefaultUserAgent
            If CheckNetscapeCookiesOnEndInit Then Update_SaveCookiesNetscape(, True)
        End Sub
#End Region
#Region "Update, Edit"
        Friend Overridable Sub BeginUpdate() Implements ISiteSettings.BeginUpdate
        End Sub
        Friend Overridable Sub EndUpdate() Implements ISiteSettings.EndUpdate
        End Sub
        Protected _SiteEditorFormOpened As Boolean = False
        Friend Overridable Sub BeginEdit() Implements ISiteSettings.BeginEdit
            _SiteEditorFormOpened = True
        End Sub
        Friend Overridable Sub EndEdit() Implements ISiteSettings.EndEdit
            If _SiteEditorFormOpened Then DomainsReset()
            _SiteEditorFormOpened = False
        End Sub
        Friend Overridable Sub Update() Implements ISiteSettings.Update
            If _SiteEditorFormOpened Then
                If UseNetscapeCookies Then Update_SaveCookiesNetscape()
                If Not Responser Is Nothing Then
                    With Responser.Headers
                        If .Count > 0 Then .ListDisposeRemove(Function(h) h.Value.IsEmptyString)
                    End With
                End If
                DomainsApply()
            End If
            If Not Responser Is Nothing Then Responser.SaveSettings()
        End Sub
        Protected Sub Update_SaveCookiesNetscape(Optional ByVal Force As Boolean = False, Optional ByVal IsInit As Boolean = False)
            If Not Responser Is Nothing Then
                With Responser
                    If .Cookies.Changed Or Force Or IsInit Then
                        If IsInit And CookiesNetscapeFile.Exists Then Exit Sub
                        If .CookiesExists Then .Cookies.SaveNetscapeFile(CookiesNetscapeFile) Else CookiesNetscapeFile.Delete()
                        .Cookies.Changed = False
                    End If
                End With
            End If
        End Sub
#Region "Specialized"
        Protected Overridable Sub DomainsApply()
        End Sub
        Protected Overridable Sub DomainsReset()
        End Sub
#End Region
#End Region
#Region "Before and After Download"
        ''' <summary>
        ''' PRE<br/>
        ''' DownloadStarted<br/>
        ''' <br/>
        ''' BEFORE<br/>
        ''' Available<br/>
        ''' <br/>
        ''' IN<br/>
        ''' ReadyToDownload<br/>
        ''' BeforeStartDownload<br/>
        ''' AfterDownload<br/>
        ''' <br/>
        ''' AFTER<br/>
        ''' DownloadDone
        ''' </summary>
        Friend Overridable Sub DownloadStarted(ByVal What As Download) Implements ISiteSettings.DownloadStarted
        End Sub
        ''' <inheritdoc cref="DownloadStarted(Download)"/>
        Friend Overridable Sub BeforeStartDownload(ByVal User As Object, ByVal What As Download) Implements ISiteSettings.BeforeStartDownload
        End Sub
        ''' <inheritdoc cref="DownloadStarted(Download)"/>
        Friend Overridable Sub AfterDownload(ByVal User As Object, ByVal What As Download) Implements ISiteSettings.AfterDownload
        End Sub
        ''' <inheritdoc cref="DownloadStarted(Download)"/>
        Friend Overridable Sub DownloadDone(ByVal What As Download) Implements ISiteSettings.DownloadDone
        End Sub
#End Region
#Region "User info"
        Protected UrlPatternUser As String = String.Empty
        Friend Overridable Function GetUserUrl(ByVal User As IPluginContentProvider) As String Implements ISiteSettings.GetUserUrl
            If Not UrlPatternUser.IsEmptyString Then Return String.Format(UrlPatternUser, User.Name)
            Return String.Empty
        End Function
        Private Function ISiteSettings_GetUserPostUrl(ByVal User As IPluginContentProvider, ByVal Media As IUserMedia) As String Implements ISiteSettings.GetUserPostUrl
            Return GetUserPostUrl(User, Media)
        End Function
        Friend Overridable Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            Return Media.URL_BASE.IfNullOrEmpty(Media.URL)
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
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, $"[API.Base.SiteSettingsBase.IsMyUser({UserURL})]", New ExchangeOptions)
            End Try
        End Function
        Protected ImageVideoContains As String = String.Empty
        Friend Overridable Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions Implements ISiteSettings.IsMyImageVideo
            If Not ImageVideoContains.IsEmptyString AndAlso URL.Contains(ImageVideoContains) Then
                Return New ExchangeOptions(Site, String.Empty) With {.Exists = True}
            Else
                Return Nothing
            End If
        End Function
        Private Function ISiteSettings_GetSingleMediaInstance(ByVal URL As String, ByVal OutputFile As String) As IDownloadableMedia Implements ISiteSettings.GetSingleMediaInstance
            Return GetSingleMediaInstance(URL, OutputFile)
        End Function
        Friend Overridable Function GetSingleMediaInstance(ByVal URL As String, ByVal OutputFile As SFile) As IDownloadableMedia
            Return New Hosts.DownloadableMediaHost(URL, OutputFile)
        End Function
#End Region
#Region "Ready, Available"
        ''' <returns>True</returns>
        Friend Overridable Function BaseAuthExists() As Boolean
            Return True
        End Function
        ''' <returns>Return BaseAuthExists()</returns>
        ''' <inheritdoc cref="DownloadStarted(Download)"/>
        Friend Overridable Function Available(ByVal What As Download, ByVal Silent As Boolean) As Boolean Implements ISiteSettings.Available
            Return BaseAuthExists()
        End Function
        ''' <returns>True</returns>
        ''' <inheritdoc cref="DownloadStarted(Download)"/>
        Friend Overridable Function ReadyToDownload(ByVal What As Download) As Boolean Implements ISiteSettings.ReadyToDownload
            Return True
        End Function
#End Region
        Friend Overridable Sub Reset() Implements ISiteSettings.Reset
        End Sub
        Friend Overridable Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean) Implements ISiteSettings.UserOptions
            Options = Nothing
        End Sub
        Friend Overridable Sub OpenSettingsForm() Implements ISiteSettings.OpenSettingsForm
        End Sub
    End Class
End Namespace