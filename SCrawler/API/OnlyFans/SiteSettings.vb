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
Imports DN = SCrawler.API.Base.DeclaredNames
Namespace API.OnlyFans
    <Manifest("AndyProgram_OnlyFans"), SavedPosts, SpecialForm(False), SpecialForm(True), SeparatedTasks(1)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
#Region "Categories"
        Private Const CAT_OFS As String = "OF-Scraper support"
        Private Const CAT_ERRORS As String = "Errors"
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
        Private Const AppTokenDefault As String = "33d57ade8c02dbc5a333db99ff9ae26a"
        <PropertyOption(ControlText:=HeaderUserID, AllowNull:=False, IsAuth:=True), PClonable(Clone:=False)>
        Friend ReadOnly Property HH_USER_ID As PropertyValue
        <PropertyOption(ControlText:=HeaderXBC, AllowNull:=False, IsAuth:=True), PClonable(Clone:=False)>
        Private ReadOnly Property HH_X_BC As PropertyValue
        <PropertyOption(ControlText:=HeaderAppToken, AllowNull:=False, IsAuth:=True), PClonable(Clone:=False)>
        Private ReadOnly Property HH_APP_TOKEN As PropertyValue
        <PropertyUpdater(NameOf(HH_APP_TOKEN))>
        Private Function UpdateAppToken() As Boolean
            HH_APP_TOKEN.Value = AppTokenDefault
            Return True
        End Function
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
        <PropertyOption(ControlText:="Update cookies during requests",
                        ControlToolTip:="If unchecked, cookies will not be updated during requests. Initial cookies will always be used.", IsAuth:=True),
                        PClonable, PXML, HiddenControl>
        Friend ReadOnly Property EnableCookiesUpdate As PropertyValue
#End Region
#Region "Errors"
        <PClonable, PXML("UpdateRules401")> Private ReadOnly Property UpdateRules401_XML As PropertyValue
        <PropertyOption(ControlText:="Try updating rules when you get a 401 error", Category:=CAT_ERRORS), HiddenControl>
        Friend ReadOnly Property UpdateRules401 As PropertyValue
            Get
                If Not DefaultInstance Is Nothing Then
                    Return DirectCast(DefaultInstance, SiteSettings).UpdateRules401_XML
                Else
                    Return UpdateRules401_XML
                End If
            End Get
        End Property
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
        <PropertyOption(ControlText:="key-mode-default", ControlToolTip:="Examples: cdrm, cdrm2, keydb, manual", Category:=CAT_OFS)>
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
        <PClonable, PXML("KEYS_Key")> Private ReadOnly Property OFS_KEYS_Key_XML As PropertyValue
        <PropertyOption(ControlText:="Private key", ControlToolTip:="Path to the DRM key file 'private_key.pem'", Category:=CAT_OFS)>
        Friend ReadOnly Property OFS_KEYS_Key As PropertyValue
            Get
                If Not DefaultInstance Is Nothing Then
                    Return DirectCast(DefaultInstance, SiteSettings).OFS_KEYS_Key_XML
                Else
                    Return OFS_KEYS_Key_XML
                End If
            End Get
        End Property
        <PClonable, PXML("KEYS_ClientID")> Private ReadOnly Property OFS_KEYS_ClientID_XML As PropertyValue
        <PropertyOption(ControlText:="Client ID", ControlToolTip:="Path to the DRM key file 'client_id.bin'", Category:=CAT_OFS)>
        Friend ReadOnly Property OFS_KEYS_ClientID As PropertyValue
            Get
                If Not DefaultInstance Is Nothing Then
                    Return DirectCast(DefaultInstance, SiteSettings).OFS_KEYS_ClientID_XML
                Else
                    Return OFS_KEYS_ClientID_XML
                End If
            End Get
        End Property
        <PropertiesDataChecker({NameOf(KeyModeDefault), NameOf(OFS_KEYS_Key), NameOf(OFS_KEYS_ClientID)})>
        Private Function OFS_KEYS_CHECKER(ByVal p As IEnumerable(Of PropertyData)) As Boolean
            Const manualMode$ = "manual"
            If p.ListExists Then
                Dim m$ = String.Empty, k$ = String.Empty, cid$ = String.Empty
                For Each pp As PropertyData In p
                    Select Case pp.Name
                        Case NameOf(KeyModeDefault) : m = pp.Value
                        Case NameOf(OFS_KEYS_Key) : k = pp.Value
                        Case NameOf(OFS_KEYS_ClientID) : cid = pp.Value
                        Case Else : Throw New ArgumentException($"Property name '{pp.Name}' is not implemented", "Property Name")
                    End Select
                Next
                If k.IsEmptyString And cid.IsEmptyString Then
                    Return True
                ElseIf Not k.IsEmptyString And Not cid.IsEmptyString Then
                    If m = manualMode Then
                        Return True
                    Else
                        Return MsgBoxE({$"You are using key files and have selected '{m}' mode." & vbCr &
                                        $"To use key files, you should use the '{manualMode}' mode" & vbCr &
                                        "Are you sure you want to use this mode?", "Incorrect mode"}, vbExclamation + vbYesNo) = vbYes
                    End If
                End If
                Dim t As New MMessage("", "Key missing",, vbCritical)
                If k.IsEmptyString Then
                    t.Text = "'Private key' is missing"
                ElseIf cid.IsEmptyString Then
                    t.Text = "'Client ID' is missing"
                End If
                If Not t.Text.IsEmptyString Then t.Show()
            End If
            Return False
        End Function
#End Region
#End Region
#Region "Initializer"
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New("OnlyFans", ".onlyfans.com", AccName, Temp, My.Resources.SiteResources.OnlyFansIcon_32, My.Resources.SiteResources.OnlyFansPic_32)

            If Rules Is Nothing Then
                Rules = New DynamicRulesEnv
                Rules.Update(False, True)
            End If

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
                    HH_APP_TOKEN = New PropertyValue(.Value(HeaderAppToken).IfNullOrEmpty(AppTokenDefault), GetType(String), Sub(v) UpdateHeader(NameOf(HH_APP_TOKEN), v))
                    HH_BROWSER = New PropertyValue(.Value(HeaderBrowser), GetType(String), Sub(v) UpdateHeader(NameOf(HH_BROWSER), v))
                End With
                UserAgent = New PropertyValue(IIf(.UserAgentExists, .UserAgent, String.Empty), GetType(String), Sub(v) UpdateHeader(NameOf(UserAgent), v))
            End With

            EnableCookiesUpdate = New PropertyValue(False)

            DownloadTimeline = New PropertyValue(True)
            DownloadStories = New PropertyValue(True)
            DownloadHighlights = New PropertyValue(True)
            DownloadChatMedia = New PropertyValue(True)

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
            OFS_KEYS_Key_XML = New PropertyValue(String.Empty, GetType(String))
            OFS_KEYS_ClientID_XML = New PropertyValue(String.Empty, GetType(String))

            UpdateRules401_XML = New PropertyValue(False)

            UserRegex = RParams.DMS(String.Format(UserRegexDefaultPattern, "onlyfans.com/"), 1, EDP.ReturnValue)
            UrlPatternUser = "https://onlyfans.com/{0}"
            ImageVideoContains = "onlyfans.com"
        End Sub
        Private Const SettingsVersionCurrent As Integer = 1
        Friend Overrides Sub EndInit()
            If CInt(SettingsVersion.Value) < SettingsVersionCurrent Then
                If CStr(HH_APP_TOKEN.Value).IsEmptyString Then HH_APP_TOKEN.Value = AppTokenDefault
                EnableCookiesUpdate.Value = False
                SettingsVersion.Value = SettingsVersionCurrent
            End If
            MyBase.EndInit()
        End Sub
#End Region
#Region "GetInstance"
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
#End Region
#Region "Update"
        Friend Overrides Sub BeginUpdate()
            _TempSettingsEnv.DisposeIfReady
            _TempSettingsEnv = Nothing
            MyBase.BeginUpdate()
        End Sub
        Friend Overrides Sub Update()
            If _SiteEditorFormOpened Then
                Responser.Cookies.Changed = False
                If If(_TempSettingsEnv?.NeedToSave, False) Then Rules.Copy(_TempSettingsEnv, True) : Rules.Save()
            End If
            MyBase.Update()
        End Sub
        Friend Overrides Sub EndUpdate()
            _TempSettingsEnv.DisposeIfReady
            _TempSettingsEnv = Nothing
            MyBase.EndUpdate()
        End Sub
        Private _TempSettingsEnv As DynamicRulesEnv = Nothing
        Friend Overrides Sub OpenSettingsForm()
            If _TempSettingsEnv Is Nothing Then _TempSettingsEnv = New DynamicRulesEnv : _TempSettingsEnv.Copy(Rules, False)
            Using f As New OnlyFansAdvancedSettingsForm(_TempSettingsEnv) : f.ShowDialog() : End Using
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
            Return String.Format(UrlPatternUser, If(User.ID.IsEmptyString, User.NameTrue.IfNullOrEmpty(User.Name), $"u{User.ID}"))
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