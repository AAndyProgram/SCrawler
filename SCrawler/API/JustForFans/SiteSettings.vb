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
Namespace API.JustForFans
    <Manifest("AndyProgram_JustForFans"), SavedPosts, SeparatedTasks(1)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
        Friend Const UserHash4_CookieName As String = "userhash4"
        <PropertyOption(ControlText:="User ID", AllowNull:=False), PXML, PClonable(Clone:=False)>
        Friend ReadOnly Property UserID As PropertyValue
        <PropertyOption, PXML, PClonable(Clone:=False)>
        Friend ReadOnly Property UserHash4 As PropertyValue
        <CookieValueExtractor(NameOf(UserHash4))>
        Private Function GetValueFromCookies(ByVal PropName As String, ByVal c As CookieKeeper) As String
            Return c.GetCookieValue(UserHash4_CookieName, PropName, NameOf(UserHash4))
        End Function
        <PropertyOption(ControlText:="Accept", ControlToolTip:="Header 'Accept'"), PClonable>
        Friend ReadOnly Property HeaderAccept As PropertyValue
        <PropertyOption(InheritanceName:=SettingsCLS.HEADER_DEF_UserAgent), PClonable, PXML(OnlyForChecked:=True)>
        Friend ReadOnly Property UserAgent As PropertyValue
        Private Sub UpdateHeader(ByVal HeaderName As String, ByVal HeaderValue As String)
            Select Case HeaderName
                Case NameOf(HeaderAccept) : If HeaderValue.IsEmptyString Then Responser.Accept = Nothing Else Responser.Accept = HeaderValue
                Case NameOf(UserAgent) : If Not HeaderValue.IsEmptyString Then Responser.UserAgent = HeaderValue
            End Select
        End Sub
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New("JustForFans", "justfor.fans", AccName, Temp, My.Resources.SiteResources.JFFIcon_64, My.Resources.SiteResources.JFFPic_76)

            With Responser
                .CookiesExtractMode = Responser.CookiesExtractModes.Any
                .CookiesUpdateMode = CookieKeeper.UpdateModes.ReplaceByNameAll
                .CookiesExtractedAutoSave = False
                .Cookies.ChangedAllowInternalDrop = False
                .Cookies.Changed = False
            End With

            UserID = New PropertyValue(String.Empty, GetType(String))
            UserHash4 = New PropertyValue(String.Empty, GetType(String))
            HeaderAccept = New PropertyValue(Responser.Accept.Value, GetType(String), Sub(v) UpdateHeader(NameOf(HeaderAccept), v))
            UserAgent = New PropertyValue(If(Responser.UserAgentExists, Responser.UserAgent, String.Empty), GetType(String), Sub(v) UpdateHeader(NameOf(UserAgent), v))

            _AllowUserAgentUpdate = False
            UserRegex = RParams.DMS(String.Format(UserRegexDefaultPattern, "justfor.fans/"), 1, EDP.ReturnValue)
            UrlPatternUser = "https://justfor.fans/{0}"
            ImageVideoContains = "justfor.fans"
        End Sub
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
        Friend Overrides Sub Update()
            If _SiteEditorFormOpened Then UpdateUserHash4()
            MyBase.Update()
        End Sub
        Private Sub UpdateUserHash4()
            If Responser.CookiesExists Then
                Dim hv_current$ = UserHash4.Value
                Dim hv_cookie$ = GetValueFromCookies(NameOf(UserHash4), Responser.Cookies)
                If Not hv_cookie.IsEmptyString And Not hv_cookie = hv_current And Responser.Cookies.Changed Then UserHash4.Value = hv_cookie
            End If
        End Sub
        Friend Sub UpdateResponser(ByVal Source As Responser)
            If Source.Cookies.Changed Then
                Responser.Cookies.Update(Source.Cookies)
                UpdateUserHash4()
                If Responser.Cookies.Changed Then Responser.SaveCookies() : Responser.Cookies.Changed = False
            End If
        End Sub
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            Return Settings.FfmpegFile.Exists And Responser.CookiesExists And ACheck(UserID.Value) And ACheck(UserHash4.Value)
        End Function
    End Class
End Namespace