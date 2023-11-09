' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports SCrawler.Plugin
Imports SCrawler.Plugin.Attributes
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Cookies
Imports PersonalUtilities.Functions.RegularExpressions
Imports IG = SCrawler.API.Instagram.SiteSettings
Namespace API.ThreadsNet
    <Manifest("AndyProgram_ThreadsNet"), SeparatedTasks(1)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
#Region "Authorization"
        <PropertyOption(ControlText:="x-csrftoken", AllowNull:=False), PClonable(Clone:=False)>
        Friend ReadOnly Property HH_CSRF_TOKEN As PropertyValue
        <PropertyOption(ControlText:="x-ig-app-id", AllowNull:=False), PClonable>
        Friend Property HH_IG_APP_ID As PropertyValue
        <PropertyOption(ControlText:="x-asbd-id", AllowNull:=True), PClonable>
        Friend Property HH_ASBD_ID As PropertyValue
        <PropertyOption(ControlText:="sec-ch-ua", AllowNull:=True), PClonable>
        Private Property HH_BROWSER As PropertyValue
        <PropertyOption(ControlText:="sec-ch-ua-full", ControlToolTip:="sec-ch-ua-full-version-list", AllowNull:=True), PClonable>
        Private Property HH_BROWSER_EXT As PropertyValue
        <PropertyOption(ControlText:="sec-ch-ua-platform-ver", ControlToolTip:="sec-ch-ua-platform-version", AllowNull:=True, LeftOffset:=120), PClonable>
        Private Property HH_PLATFORM As PropertyValue
        <PropertyOption(ControlText:="UserAgent"), PClonable>
        Private ReadOnly Property HH_USER_AGENT As PropertyValue
        Private Sub ChangeResponserFields(ByVal PropName As String, ByVal Value As Object)
            If Not PropName.IsEmptyString Then
                Dim f$ = String.Empty
                Dim isUserAgent As Boolean = False
                Select Case PropName
                    Case NameOf(HH_IG_APP_ID) : f = IG.Header_IG_APP_ID
                    Case NameOf(HH_ASBD_ID) : f = IG.Header_ASBD_ID
                    Case NameOf(HH_CSRF_TOKEN) : f = IG.Header_CSRF_TOKEN
                    Case NameOf(HH_BROWSER) : f = IG.Header_Browser
                    Case NameOf(HH_BROWSER_EXT) : f = IG.Header_BrowserExt
                    Case NameOf(HH_PLATFORM) : f = IG.Header_Platform
                    Case NameOf(HH_USER_AGENT) : isUserAgent = True
                End Select
                If Not f.IsEmptyString Then
                    Responser.Headers.Remove(f)
                    If Not CStr(Value).IsEmptyString Then Responser.Headers.Add(f, CStr(Value))
                ElseIf isUserAgent Then
                    Responser.UserAgent = CStr(Value)
                End If
            End If
        End Sub
#End Region
#End Region
#Region "Initializer"
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New("Threads", "threads.net", AccName, Temp, My.Resources.SiteResources.ThreadsIcon_192, My.Resources.SiteResources.ThreadsIcon_192.ToBitmap)
            _AllowUserAgentUpdate = False

            Dim app_id$ = String.Empty
            Dim token$ = String.Empty
            Dim asbd$ = String.Empty
            Dim browser$ = String.Empty
            Dim browserExt$ = String.Empty
            Dim platform$ = String.Empty
            Dim useragent$ = String.Empty

            With Responser
                .Accept = "*/*"
                'URGENT: remove after debug
                .DeclaredError = EDP.SendToLog + EDP.ThrowException
                If .UserAgentExists Then useragent = .UserAgent
                With .Headers
                    If .Count > 0 Then
                        token = .Value(IG.Header_CSRF_TOKEN)
                        app_id = .Value(IG.Header_IG_APP_ID)
                        asbd = .Value(IG.Header_ASBD_ID)
                        browser = .Value(IG.Header_Browser)
                        browserExt = .Value(IG.Header_BrowserExt)
                        platform = .Value(IG.Header_Platform)
                    End If
                    .Add("Authority", "www.threads.net")
                    .Add("Origin", "https://www.threads.net")
                    .Add("Upgrade-Insecure-Requests", 1)
                    .Add("Sec-Ch-Ua-Model", "")
                    .Add("Sec-Ch-Ua-Mobile", "?0")
                    .Add("Sec-Ch-Ua-Platform", """Windows""")
                    .Add("Sec-Fetch-Dest", "empty")
                    .Add("Sec-Fetch-Mode", "cors")
                    .Add("Sec-Fetch-Site", "same-origin")
                    .Add("Sec-Fetch-User", "?1")
                    .Add("x-fb-friendly-name", "BarcelonaProfileThreadsTabRefetchableQuery")
                End With
                .CookiesExtractMode = Responser.CookiesExtractModes.Any
                .CookiesUpdateMode = CookieKeeper.UpdateModes.ReplaceByNameAll
                .CookiesExtractedAutoSave = False
                .Cookies.ChangedAllowInternalDrop = False
                .Cookies.Changed = False
            End With

            HH_CSRF_TOKEN = New PropertyValue(token, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_CSRF_TOKEN), v))
            HH_IG_APP_ID = New PropertyValue(app_id, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_IG_APP_ID), v))
            HH_ASBD_ID = New PropertyValue(asbd, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_ASBD_ID), v))
            HH_BROWSER = New PropertyValue(browser, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_BROWSER), v))
            HH_BROWSER_EXT = New PropertyValue(browserExt, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_BROWSER_EXT), v))
            HH_PLATFORM = New PropertyValue(platform, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_PLATFORM), v))
            HH_USER_AGENT = New PropertyValue(useragent, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_USER_AGENT), v))

            UrlPatternUser = "https://www.threads.net/@{0}"
            UserRegex = RParams.DMS("threads.net/@([^/\?&]+)", 1)
            ImageVideoContains = "threads.net"
        End Sub
#End Region
#Region "UpdateResponserData"
        Friend Sub UpdateResponserData(ByVal Resp As Responser)
            With Responser.Cookies
                Dim csrf$ = String.Empty
                .Update(Resp.Cookies)
                If .Changed Then
                    Responser.SaveCookies()
                    .Changed = False
                    csrf = If(.FirstOrDefault(Function(c) c.Name.StringToLower = IG.Header_CSRF_TOKEN_COOKIE)?.Value, String.Empty)
                End If
                If Not csrf.IsEmptyString AndAlso Not AEquals(Of String)(csrf, HH_CSRF_TOKEN.Value) Then HH_CSRF_TOKEN.Value = csrf : Responser.SaveSettings()
            End With
        End Sub
#End Region
#Region "GetInstance"
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
#End Region
#Region "BaseAuthExists, GetUserUrl, GetUserPostUrl"
        Friend Overrides Function BaseAuthExists() As Boolean
            Return Responser.CookiesExists And {HH_CSRF_TOKEN, HH_IG_APP_ID}.All(Function(v) ACheck(Of String)(v.Value))
        End Function
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider) As String
            Return String.Format(UrlPatternUser, DirectCast(User, UserData).NameTrue)
        End Function
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            Try
                Dim code$ = DirectCast(User, UserData).GetPostCodeById(Media.Post.ID)
                Dim name$ = DirectCast(User, UserData).NameTrue
                If Not code.IsEmptyString Then Return $"https://www.threads.net/@{name}/post/{code}/" Else Return String.Empty
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "Can't open user's post", String.Empty)
            End Try
        End Function
#End Region
    End Class
End Namespace