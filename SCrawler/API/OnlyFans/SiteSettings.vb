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
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Cookies
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.OnlyFans
    <Manifest("AndyProgram_OnlyFans"), SavedPosts, SeparatedTasks(1)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Icon"
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return My.Resources.SiteResources.OnlyFansIcon_32
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.SiteResources.OnlyFansPic_32
            End Get
        End Property
#End Region
#Region "Declarations"
        Private Const HeaderBrowser As String = "sec-ch-ua"
        Private Const HeaderUserID As String = "User-Id"
        Private Const HeaderXBC As String = "X-Bc"
        Private Const HeaderAppToken As String = "App-Token"
        <PropertyOption(ControlText:=HeaderUserID, AllowNull:=False)>
        Friend ReadOnly Property HH_USER_ID As PropertyValue
        <PropertyOption(ControlText:=HeaderXBC, AllowNull:=False)>
        Private ReadOnly Property HH_X_BC As PropertyValue
        <PropertyOption(ControlText:=HeaderAppToken, AllowNull:=False)>
        Private ReadOnly Property HH_APP_TOKEN As PropertyValue
        <PropertyOption(ControlText:=HeaderBrowser, AllowNull:=False)>
        Private ReadOnly Property HH_BROWSER As PropertyValue
        <PropertyOption(AllowNull:=False)>
        Private ReadOnly Property UserAgent As PropertyValue
        Private Sub UpdateHeader(ByVal PropertyName As String, ByVal Value As String)
            Dim hName$ = String.Empty
            Dim isUserAgent As Boolean = False
            Select Case PropertyName
                Case NameOf(HH_USER_ID) : hName = HeaderUserID
                Case NameOf(HH_X_BC) : hName = HeaderXBC
                Case NameOf(HH_APP_TOKEN) : hName = HeaderAppToken
                Case NameOf(HH_BROWSER) : hName = HeaderBrowser
                Case NameOf(UserAgent) : isUserAgent = True
            End Select
            If Not hName.IsEmptyString Then
                Responser.Headers.Add(hName, Value)
            ElseIf isUserAgent Then
                Responser.UserAgent = Value
            End If
        End Sub
        <PXML("LastDateUpdated")> Private ReadOnly Property LastDateUpdated_XML As PropertyValue
        Friend Property LastDateUpdated As Date
            Get
                Return LastDateUpdated_XML.Value
            End Get
            Set(ByVal d As Date)
                LastDateUpdated_XML.Value = d
            End Set
        End Property
        <PropertyOption(ControlText:="Use old authorization rules",
                        ControlToolTip:="Use old dynamic rules (from 'DATAHOARDERS') or new ones (from 'DIGITALCRIMINALS')." & vbCr &
                                        "Change this value only if you know what you are doing."), PXML>
        Friend ReadOnly Property UseOldAuthRules As PropertyValue
        <PropertyOption(ControlText:="Dynamic rules update", ControlToolTip:="'Dynamic rules' update interval (minutes). Default: 1440", LeftOffset:=110), PXML>
        Friend ReadOnly Property DynamicRulesUpdateInterval As PropertyValue
        <Provider(NameOf(DynamicRulesUpdateInterval), FieldsChecker:=True)>
        Private ReadOnly Property DynamicRulesUpdateIntervalProvider As IFormatProvider
        <PropertyOption(ControlText:="Dynamic rules",
                        ControlToolTip:="Overwrite 'Dynamic rules' with this URL" & vbCr &
                                        "Change this value only if you know what you are doing."), PXML>
        Friend ReadOnly Property DynamicRules As PropertyValue
#End Region
#Region "Initializer"
        Friend Sub New()
            MyBase.New("OnlyFans", ".onlyfans.com")

            With Responser
                .Accept = "application/json, text/plain, */*"
                .AutomaticDecompression = Net.DecompressionMethods.GZip
                .CookiesExtractMode = Responser.CookiesExtractModes.Any
                .CookiesExtractedAutoSave = False
                .CookiesUpdateMode = CookieKeeper.UpdateModes.Disabled
                .Cookies.ChangedAllowInternalDrop = False
                .Cookies.Changed = False
                With .Headers
                    .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecChUaPlatform))
                    .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecChUaMobile))
                    .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecFetchDest))
                    .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecFetchMode))
                    .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecFetchSite))
                    .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.DHT))
                    .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.Authority, "onlyfans.com"))
                    .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.AcceptEncoding))
                    HH_USER_ID = New PropertyValue(.Value(HeaderUserID), GetType(String), Sub(v) UpdateHeader(NameOf(HH_USER_ID), v))
                    HH_X_BC = New PropertyValue(.Value(HeaderXBC), GetType(String), Sub(v) UpdateHeader(NameOf(HH_X_BC), v))
                    HH_APP_TOKEN = New PropertyValue(.Value(HeaderAppToken), GetType(String), Sub(v) UpdateHeader(NameOf(HH_APP_TOKEN), v))
                    HH_BROWSER = New PropertyValue(.Value(HeaderBrowser), GetType(String), Sub(v) UpdateHeader(NameOf(HH_BROWSER), v))
                End With
                UserAgent = New PropertyValue(IIf(.UserAgentExists, .UserAgent, String.Empty), GetType(String), Sub(v) UpdateHeader(NameOf(UserAgent), v))
            End With

            LastDateUpdated_XML = New PropertyValue(Now.AddYears(-1), GetType(Date))
            UseOldAuthRules = New PropertyValue(False)
            DynamicRulesUpdateInterval = New PropertyValue(60 * 24)
            DynamicRulesUpdateIntervalProvider = New FieldsCheckerProviderSimple(Function(v) IIf(AConvert(Of Integer)(v, 0) > 0, v, Nothing),
                                                                                 "The value of [{0}] field must be greater than 0")
            DynamicRules = New PropertyValue(String.Empty, GetType(String))
            UserRegex = RParams.DMS("onlyfans.com/(\w+)", 1, EDP.ReturnValue)
            UrlPatternUser = "https://onlyfans.com/{0}"
            ImageVideoContains = "onlyfans.com"
        End Sub
#End Region
#Region "GetInstance"
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
#End Region
#Region "Update"
        Friend Overrides Sub Update()
            If _SiteEditorFormOpened Then Responser.Cookies.Changed = False
            MyBase.Update()
        End Sub
#End Region
#Region "Download"
        Friend Overrides Function BaseAuthExists() As Boolean
            Return Responser.CookiesExists And {HH_USER_ID, HH_X_BC, HH_APP_TOKEN, HH_BROWSER, UserAgent}.All(Function(v) ACheck(v.Value))
        End Function
        Friend Overrides Function ReadyToDownload(ByVal What As ISiteSettings.Download) As Boolean
            Return BaseAuthExists() And Not SessionAborted
        End Function
        Friend Property SessionAborted As Boolean = False
        Friend Overrides Sub AfterDownload(ByVal User As Object, ByVal What As ISiteSettings.Download)
            Responser.Cookies.Update(DirectCast(User, UserData).CCookie)
        End Sub
        Friend Overrides Sub DownloadDone(ByVal What As ISiteSettings.Download)
            MyBase.DownloadDone(What)
            SessionAborted = False
            If Responser.Cookies.Changed Then Responser.SaveCookies() : Responser.Cookies.Changed = False
        End Sub
#End Region
#Region "GetUserUrl, GetUserPostUrl"
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider) As String
            Return String.Format(UrlPatternUser, If(User.ID.IsEmptyString, User.Name, $"u{User.ID}"))
        End Function
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            If Not Media.Post.ID.IsEmptyString Then
                Return String.Format("https://onlyfans.com/{0}/{1}", Media.Post.ID, If(User.ID.IsEmptyString, User.Name, $"u{User.ID}"))
            Else
                Return String.Empty
            End If
        End Function
#End Region
    End Class
End Namespace