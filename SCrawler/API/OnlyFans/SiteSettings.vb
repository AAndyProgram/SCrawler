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
Imports DN = SCrawler.API.Base.DeclaredNames
Namespace API.OnlyFans
    <Manifest("AndyProgram_OnlyFans"), SavedPosts, SpecialForm(False), SeparatedTasks(1)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
#Region "Categories"
        Private Const CAT_OFS As String = "OF-Scraper support"
#End Region
#Region "Options"
        <PropertyOption(ControlText:="Download timeline", ControlToolTip:="Download user timeline", Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property DownloadTimeline As PropertyValue
        <PropertyOption(ControlText:="Download stories", ControlToolTip:="Download profile stories if they exists", Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property DownloadStories As PropertyValue
        <PropertyOption(ControlText:="Download highlights", ControlToolTip:="Download profile highlights if they exists", Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property DownloadHighlights As PropertyValue
        <PropertyOption(ControlText:="Download chat", ControlToolTip:="Download unlocked chat media", Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property DownloadChatMedia As PropertyValue
#End Region
#Region "Headers"
        Private Const HeaderBrowser As String = "sec-ch-ua"
        Private Const HeaderUserID As String = "User-Id"
        Friend Const HeaderXBC As String = "X-Bc"
        Friend Const HeaderAppToken As String = "App-Token"
        <PropertyOption(ControlText:=HeaderUserID, AllowNull:=False, IsAuth:=True), PClonable(Clone:=False)>
        Friend ReadOnly Property HH_USER_ID As PropertyValue
        <PropertyOption(ControlText:=HeaderXBC, AllowNull:=False, IsAuth:=True), PClonable(Clone:=False)>
        Private ReadOnly Property HH_X_BC As PropertyValue
        <PropertyOption(ControlText:=HeaderAppToken, AllowNull:=False, IsAuth:=True), PClonable(Clone:=False)>
        Private ReadOnly Property HH_APP_TOKEN As PropertyValue
        <PropertyOption(ControlText:=HeaderBrowser, ControlToolTip:="Can be null", AllowNull:=True,
                        InheritanceName:=SettingsCLS.HEADER_DEF_sec_ch_ua, IsAuth:=True), PClonable, PXML(OnlyForChecked:=True)>
        Private ReadOnly Property HH_BROWSER As PropertyValue
        <PropertyOption(AllowNull:=False, InheritanceName:=SettingsCLS.HEADER_DEF_UserAgent, IsAuth:=True), PClonable, PXML(OnlyForChecked:=True)>
        Friend ReadOnly Property UserAgent As PropertyValue
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
        <CookieValueExtractor(NameOf(HH_USER_ID)), CookieValueExtractor(NameOf(HH_X_BC))>
        Private Function GetValueFromCookies(ByVal PropName As String, ByVal c As CookieKeeper) As String
            If c.ListExists Then
                Select Case PropName
                    Case NameOf(HH_USER_ID) : Return c.GetCookieValue("auth_id")
                    Case NameOf(HH_X_BC) : Return c.GetCookieValue("fp")
                End Select
            End If
            Return String.Empty
        End Function
#End Region
#Region "Rules"
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
                                        "Change this value only if you know what you are doing.", IsAuth:=True), PXML, PClonable>
        Friend ReadOnly Property UseOldAuthRules As PropertyValue
        <PropertyOption(ControlText:="Dynamic rules update", ControlToolTip:="'Dynamic rules' update interval (minutes). Default: 1440",
                        LeftOffset:=110, IsAuth:=True), PXML, PClonable>
        Friend ReadOnly Property DynamicRulesUpdateInterval As PropertyValue
        <Provider(NameOf(DynamicRulesUpdateInterval), FieldsChecker:=True)>
        Private ReadOnly Property DynamicRulesUpdateIntervalProvider As IFormatProvider
        <PropertyOption(ControlText:="Dynamic rules",
                        ControlToolTip:="Overwrite 'Dynamic rules' with this URL" & vbCr &
                                        "Change this value only if you know what you are doing.", IsAuth:=True), PXML, PClonable>
        Friend ReadOnly Property DynamicRules As PropertyValue
#End Region
#Region "OFScraper"
        <PClonable, PXML("OFScraperPath")> Private ReadOnly Property OFScraperPath_XML As PropertyValue
        <PropertyOption(ControlText:="OF-Scraper path", ControlToolTip:="The path to the 'ofscraper.exe'", Category:=CAT_OFS)>
        Friend ReadOnly Property OFScraperPath As PropertyValue
            Get
                If Not DefaultInstance Is Nothing Then
                    Return DirectCast(DefaultInstance, SiteSettings).OFScraperPath_XML
                Else
                    Return OFScraperPath_XML
                End If
            End Get
        End Property
        <PClonable, PXML("OFScraperMP4decrypt")> Private ReadOnly Property OFScraperMP4decrypt_XML As PropertyValue
        <PropertyOption(ControlText:="mp4decrypt path", ControlToolTip:="The path to the 'mp4decrypt.exe'", Category:=CAT_OFS)>
        Friend ReadOnly Property OFScraperMP4decrypt As PropertyValue
            Get
                If Not DefaultInstance Is Nothing Then
                    Return DirectCast(DefaultInstance, SiteSettings).OFScraperMP4decrypt_XML
                Else
                    Return OFScraperMP4decrypt_XML
                End If
            End Get
        End Property
        Friend Const KeyModeDefault_Default As String = "cdrm"
        <PClonable, PXML("KeyModeDefault")> Private ReadOnly Property KeyModeDefault_XML As PropertyValue
        <PropertyOption(ControlText:="key-mode-default", Category:=CAT_OFS)>
        Friend ReadOnly Property KeyModeDefault As PropertyValue
            Get
                If Not DefaultInstance Is Nothing Then
                    Return DirectCast(DefaultInstance, SiteSettings).KeyModeDefault_XML
                Else
                    Return KeyModeDefault_XML
                End If
            End Get
        End Property
        <PClonable, PXML("keydb_api")> Private ReadOnly Property Keydb_Api_XML As PropertyValue
        <PropertyOption(ControlText:="keydb_api", Category:=CAT_OFS)>
        Friend ReadOnly Property Keydb_Api As PropertyValue
            Get
                If Not DefaultInstance Is Nothing Then
                    Return DirectCast(DefaultInstance, SiteSettings).Keydb_Api_XML
                Else
                    Return Keydb_Api_XML
                End If
            End Get
        End Property
#End Region
#End Region
#Region "Initializer"
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New("OnlyFans", ".onlyfans.com", AccName, Temp, My.Resources.SiteResources.OnlyFansIcon_32, My.Resources.SiteResources.OnlyFansPic_32)

            CheckOFSConfig()

            _AllowUserAgentUpdate = False

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

            DownloadTimeline = New PropertyValue(True)
            DownloadStories = New PropertyValue(True)
            DownloadHighlights = New PropertyValue(True)
            DownloadChatMedia = New PropertyValue(True)

            LastDateUpdated_XML = New PropertyValue(Now.AddYears(-1), GetType(Date))
            'URGENT: OF [UseOldAuthRules = True]
            UseOldAuthRules = New PropertyValue(True)
            DynamicRulesUpdateInterval = New PropertyValue(60 * 24)
            DynamicRulesUpdateIntervalProvider = New FieldsCheckerProviderSimple(Function(v) IIf(AConvert(Of Integer)(v, 0) > 0, v, Nothing),
                                                                                 "The value of [{0}] field must be greater than 0")
            DynamicRules = New PropertyValue(String.Empty, GetType(String))
            OFScraperPath_XML = New PropertyValue(String.Empty, GetType(String))
            If ACheck(OFScraperPath_XML.Value) Then
                Dim f As SFile = OFScraperPath_XML.Value
                If Not f.Exists AndAlso f.Exists(SFO.Path, False) Then
                    With SFile.GetFiles(f, "*.exe",, EDP.ReturnValue)
                        If .ListExists Then
                            f = .FirstOrDefault(Function(ff) ff.Name.StringToLower.StartsWith("ofscraper"))
                            If f.Exists Then OFScraperPath_XML.Value = f.ToString
                        End If
                    End With
                End If
            End If
            OFScraperMP4decrypt_XML = New PropertyValue(String.Empty, GetType(String))
            KeyModeDefault_XML = New PropertyValue(KeyModeDefault_Default)
            Keydb_Api_XML = New PropertyValue(String.Empty, GetType(String))

            UserRegex = RParams.DMS(String.Format(UserRegexDefaultPattern, "onlyfans.com/"), 1, EDP.ReturnValue)
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
        Private __UseOldAuthRules As Boolean = True
        Private __DynamicRules As String = String.Empty
        Friend Overrides Sub BeginUpdate()
            __UseOldAuthRules = UseOldAuthRules.Value
            __DynamicRules = AConvert(Of String)(DynamicRules.Value, String.Empty)
            MyBase.BeginUpdate()
        End Sub
        Friend Overrides Sub Update()
            If _SiteEditorFormOpened Then
                If Not __UseOldAuthRules = CBool(UseOldAuthRules.Value) Or Not AEquals(Of String)(__DynamicRules, DynamicRules.Value) Then _
                   LastDateUpdated = Now.AddYears(-1)
                Responser.Cookies.Changed = False
            End If
            MyBase.Update()
        End Sub
#End Region
#Region "Download"
        Friend Overrides Function BaseAuthExists() As Boolean
            Return Responser.CookiesExists And {HH_USER_ID, HH_X_BC, HH_APP_TOKEN, UserAgent}.All(Function(v) ACheck(v.Value))
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
#Region "GetUserUrl, GetUserPostUrl, UserOptions"
        Friend Const UserPostPattern As String = "https://onlyfans.com/{0}/{1}"
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider) As String
            Return String.Format(UrlPatternUser, If(User.ID.IsEmptyString, User.Name, $"u{User.ID}"))
        End Function
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            If Not Media.Post.ID.IsEmptyString Then
                Dim post$() = Media.Post.ID.Split("_")
                Dim p$ = String.Empty
                If post.ListExists Then
                    If post(0) = UserData.A_MESSAGE Then
                        If Not User.ID.IsEmptyString Then Return $"https://onlyfans.com/my/chats/chat/{User.ID}/"
                    ElseIf Not post(0) = UserData.A_HIGHLIGHT Then
                        p = post(0)
                    End If
                End If
                If p.IsEmptyString Then
                    Return GetUserUrl(User)
                Else
                    Return String.Format(UserPostPattern, p, If(User.ID.IsEmptyString, User.Name, $"u{User.ID}"))
                End If
            Else
                Return String.Empty
            End If
        End Function
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse Not TypeOf Options Is UserExchangeOptions Then Options = New UserExchangeOptions(Me)
            If OpenForm Then
                Using f As New InternalSettingsForm(Options, Me, False) : f.ShowDialog() : End Using
            End If
        End Sub
#End Region
    End Class
End Namespace