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
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Objects
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Cookies
Imports Download = SCrawler.Plugin.ISiteSettings.Download
Namespace API.Instagram
    <Manifest(InstagramSiteKey), SeparatedTasks(1), SavedPosts, SpecialForm(False)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
#Region "Images"
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return My.Resources.SiteResources.InstagramIcon_32
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.SiteResources.InstagramPic_76
            End Get
        End Property
#End Region
#Region "Providers"
        Private Class TimersChecker : Inherits FieldsCheckerProviderBase
            Private ReadOnly LVProvider As New ANumbers With {.FormatOptions = ANumbers.Options.GroupIntegral}
            Private ReadOnly _LowestValue As Integer
            Friend Sub New(ByVal LowestValue As Integer)
                _LowestValue = LowestValue
            End Sub
            Public Overrides Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                              Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object
                TypeError = False
                ErrorMessage = String.Empty
                If Not ACheck(Of Integer)(Value) Then
                    TypeError = True
                ElseIf CInt(Value) < _LowestValue Then
                    ErrorMessage = $"The value of [{Name}] field must be greater than or equal to {_LowestValue.NumToString(LVProvider)}"
                    HasError = True
                Else
                    Return Value
                End If
                Return Nothing
            End Function
        End Class
        Private Class TaggedNotifyLimitChecker : Inherits FieldsCheckerProviderBase
            Public Overrides Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                              Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object
                Dim v% = AConvert(Of Integer)(Value, -10)
                If v > 0 Or v = -1 Then
                    Return Value
                Else
                    ErrorMessage = $"The value of [{Name}] field must be greater than 0 or equal to -1"
                    HasError = True
                    Return Nothing
                End If
            End Function
        End Class
#End Region
#Region "Authorization properties"
        Friend Const Header_IG_APP_ID As String = "x-ig-app-id"
        Friend Const Header_IG_WWW_CLAIM As String = "x-ig-www-claim"
        Friend Const Header_CSRF_TOKEN As String = "x-csrftoken"
        Friend Const Header_CSRF_TOKEN_COOKIE As String = "csrftoken"
        Friend Const Header_ASBD_ID As String = "X-Asbd-Id"
        Friend Const Header_Browser As String = "Sec-Ch-Ua"
        Friend Const Header_BrowserExt As String = "Sec-Ch-Ua-Full-Version-List"
        Friend Const Header_Platform As String = "Sec-Ch-Ua-Platform-Version"
        <PropertyOption(ControlText:="Hash", ControlToolTip:="Instagram session hash for tagged posts", IsAuth:=True), PXML("InstaHash"), ControlNumber(0)>
        Friend ReadOnly Property HashTagged As PropertyValue
        <PropertyOption(ControlText:="x-csrftoken", IsAuth:=True, AllowNull:=False), ControlNumber(2)>
        Friend ReadOnly Property HH_CSRF_TOKEN As PropertyValue
        <PropertyOption(ControlText:="x-ig-app-id", IsAuth:=True, AllowNull:=False), ControlNumber(3)>
        Friend Property HH_IG_APP_ID As PropertyValue
        <PropertyOption(ControlText:="x-asbd-id", IsAuth:=True, AllowNull:=True), ControlNumber(4)>
        Friend Property HH_ASBD_ID As PropertyValue
        <PropertyOption(ControlText:="x-ig-www-claim", IsAuth:=True, AllowNull:=True), ControlNumber(5)>
        Friend Property HH_IG_WWW_CLAIM As PropertyValue
        <PropertyOption(ControlText:="sec-ch-ua", IsAuth:=True, AllowNull:=True), ControlNumber(6)>
        Private Property HH_BROWSER As PropertyValue
        <PropertyOption(ControlText:="sec-ch-ua-full", ControlToolTip:="sec-ch-ua-full-version-list", IsAuth:=True, AllowNull:=True), ControlNumber(7)>
        Private Property HH_BROWSER_EXT As PropertyValue
        <PropertyOption(ControlText:="sec-ch-ua-platform-ver", ControlToolTip:="sec-ch-ua-platform-version", IsAuth:=True, AllowNull:=True), ControlNumber(8)>
        Private Property HH_PLATFORM As PropertyValue
        <PropertyOption(ControlText:="UserAgent", IsAuth:=True, AllowNull:=True), ControlNumber(9)>
        Private Property HH_USER_AGENT As PropertyValue
        Friend Overrides Function BaseAuthExists() As Boolean
            Return Responser.CookiesExists And ACheck(HH_IG_APP_ID.Value) And ACheck(HH_CSRF_TOKEN.Value)
        End Function
        Private _FieldsChangerSuspended As Boolean = False
        Private Sub ChangeResponserFields(ByVal PropName As String, ByVal Value As Object)
            If Not _FieldsChangerSuspended And Not PropName.IsEmptyString Then
                Dim f$ = String.Empty
                Dim isUserAgent As Boolean = False
                Select Case PropName
                    Case NameOf(HH_IG_APP_ID) : f = Header_IG_APP_ID
                    Case NameOf(HH_ASBD_ID) : f = Header_ASBD_ID
                    Case NameOf(HH_IG_WWW_CLAIM) : f = Header_IG_WWW_CLAIM
                    Case NameOf(HH_CSRF_TOKEN) : f = Header_CSRF_TOKEN
                    Case NameOf(HH_BROWSER) : f = Header_Browser
                    Case NameOf(HH_BROWSER_EXT) : f = Header_BrowserExt
                    Case NameOf(HH_PLATFORM) : f = Header_Platform
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
#Region "Download properties"
        <PropertyOption(ControlText:="Request timer", AllowNull:=False), PXML("RequestsWaitTimer"), ControlNumber(20)>
        Friend ReadOnly Property RequestsWaitTimer As PropertyValue
        <Provider(NameOf(RequestsWaitTimer), FieldsChecker:=True)>
        Private ReadOnly Property RequestsWaitTimerProvider As IFormatProvider
        <PropertyOption(ControlText:="Request timer counter", AllowNull:=False, LeftOffset:=120), PXML("RequestsWaitTimerTaskCount"), ControlNumber(21)>
        Friend ReadOnly Property RequestsWaitTimerTaskCount As PropertyValue
        <Provider(NameOf(RequestsWaitTimerTaskCount), FieldsChecker:=True)>
        Private ReadOnly Property RequestsWaitTimerTaskCountProvider As IFormatProvider
        <PropertyOption(ControlText:="Posts limit timer", AllowNull:=False), PXML("SleepTimerOnPostsLimit"), ControlNumber(22)>
        Friend ReadOnly Property SleepTimerOnPostsLimit As PropertyValue
        <Provider(NameOf(SleepTimerOnPostsLimit), FieldsChecker:=True)>
        Private ReadOnly Property SleepTimerOnPostsLimitProvider As IFormatProvider
        <PropertyOption(ControlText:="Get timeline", ControlToolTip:="Default value for new users"), PXML, ControlNumber(23)>
        Friend ReadOnly Property GetTimeline As PropertyValue
        <PropertyOption(ControlText:="Get stories", ControlToolTip:="Default value for new users"), PXML, ControlNumber(24)>
        Friend ReadOnly Property GetStories As PropertyValue
        <PropertyOption(ControlText:="Get stories: user", ControlToolTip:="Default value for new users"), PXML, ControlNumber(25)>
        Friend ReadOnly Property GetStoriesUser As PropertyValue
        <PropertyOption(ControlText:="Get tagged photos", ControlToolTip:="Default value for new users"), PXML, ControlNumber(26)>
        Friend ReadOnly Property GetTagged As PropertyValue
        <PropertyOption(ControlText:="Tagged notify limit",
                        ControlToolTip:="If the number of tagged posts exceeds this number you will be notified." & vbCr &
                        "-1 to disable"), PXML, ControlNumber(27)>
        Friend ReadOnly Property TaggedNotifyLimit As PropertyValue
        <Provider(NameOf(TaggedNotifyLimit), FieldsChecker:=True)>
        Private ReadOnly Property TaggedNotifyLimitProvider As IFormatProvider
#End Region
#Region "Download ready"
        <PropertyOption(ControlText:="Download timeline", ControlToolTip:="Download timeline"), PXML, ControlNumber(10)>
        Friend ReadOnly Property DownloadTimeline As PropertyValue
        <PropertyOption(ControlText:="Download stories", ControlToolTip:="Download stories"), PXML, ControlNumber(11)>
        Friend ReadOnly Property DownloadStories As PropertyValue
        <PropertyOption(ControlText:="Download stories: user", ControlToolTip:="Download stories (user)"), PXML, ControlNumber(12)>
        Friend ReadOnly Property DownloadStoriesUser As PropertyValue
        <PropertyOption(ControlText:="Download tagged", ControlToolTip:="Download tagged posts"), PXML, ControlNumber(13)>
        Friend ReadOnly Property DownloadTagged As PropertyValue
#End Region
#Region "429 bypass"
        Private ReadOnly Property DownloadingErrorDate As XMLValue(Of Date)
        Friend Property LastApplyingValue As Integer? = Nothing
        Friend ReadOnly Property ReadyForDownload As Boolean
            Get
                If SkipUntilNextSession Then Return False
                With DownloadingErrorDate
                    If .ValueF.Exists Then
                        Return .ValueF.Value.AddMinutes(If(LastApplyingValue, 10)) < Now
                    Else
                        Return True
                    End If
                End With
            End Get
        End Property
        Private ReadOnly Property LastDownloadDate As XMLValue(Of Date)
        Private ReadOnly Property LastRequestsCount As XMLValue(Of Integer)
        <PropertyOption(IsInformationLabel:=True), ControlNumber(100)>
        Private Property LastRequestsCountLabel As PropertyValue
        Private ReadOnly LastRequestsCountLabelStr As Func(Of Integer, String) = Function(r) $"Number of spent requests: {r.NumToGroupIntegral}"
        Private TooManyRequestsReadyForCatch As Boolean = True
        Friend Function GetWaitDate() As Date
            With DownloadingErrorDate
                If .ValueF.Exists Then
                    Return .ValueF.Value.AddMinutes(If(LastApplyingValue, 10))
                Else
                    Return Now
                End If
            End With
        End Function
        Friend Sub TooManyRequests(ByVal Catched As Boolean)
            With DownloadingErrorDate
                If Catched Then
                    If Not .ValueF.Exists Then
                        .Value = Now
                        If TooManyRequestsReadyForCatch Then
                            LastApplyingValue = If(LastApplyingValue, 0) + 10
                            TooManyRequestsReadyForCatch = False
                            MyMainLOG = $"Instagram downloading error: too many requests. Try again after {If(LastApplyingValue, 10)} minutes..."
                        End If
                    End If
                Else
                    .ValueF = Nothing
                    LastApplyingValue = Nothing
                    TooManyRequestsReadyForCatch = True
                End If
            End With
        End Sub
#End Region
#End Region
#Region "Initializer"
        Friend Sub New(ByRef _XML As XmlFile, ByVal GlobalPath As SFile)
            MyBase.New(InstagramSite, "instagram.com")

            Dim app_id$ = String.Empty
            Dim www_claim$ = String.Empty
            Dim token$ = String.Empty
            Dim asbd$ = String.Empty
            Dim browser$ = String.Empty
            Dim browserExt$ = String.Empty
            Dim platform$ = String.Empty
            Dim useragent$ = String.Empty

            With Responser
                .Accept = "*/*"
                useragent = .UserAgent
                With .Headers
                    If .Count > 0 Then
                        token = .Value(Header_CSRF_TOKEN)
                        app_id = .Value(Header_IG_APP_ID)
                        www_claim = .Value(Header_IG_WWW_CLAIM)
                        asbd = .Value(Header_ASBD_ID)
                        browser = .Value(Header_Browser)
                        browserExt = .Value(Header_BrowserExt)
                        platform = .Value(Header_Platform)
                    End If
                    .Add("Dnt", 1)
                    .Add("Sec-Ch-Ua-Mobile", "?0")
                    .Add("Sec-Ch-Ua-Platform", """Windows""")
                    .Add("Sec-Fetch-Dest", "empty")
                    .Add("Sec-Fetch-Mode", "cors")
                    .Add("Sec-Fetch-Site", "same-origin")
                    .Add("X-Requested-With", "XMLHttpRequest")
                End With
                .CookiesExtractMode = Responser.CookiesExtractModes.Response
                .CookiesUpdateMode = CookieKeeper.UpdateModes.ReplaceByNameAll
                .CookiesExtractedAutoSave = False
            End With

            Dim n() As String = {SettingsCLS.Name_Node_Sites, Site.ToString}

            HashTagged = New PropertyValue(String.Empty, GetType(String))
            HH_CSRF_TOKEN = New PropertyValue(token, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_CSRF_TOKEN), v))
            HH_IG_APP_ID = New PropertyValue(app_id, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_IG_APP_ID), v))
            HH_ASBD_ID = New PropertyValue(asbd, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_ASBD_ID), v))
            HH_IG_WWW_CLAIM = New PropertyValue(www_claim.IfNullOrEmpty(0), GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_IG_WWW_CLAIM), v))
            HH_BROWSER = New PropertyValue(browser, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_BROWSER), v))
            HH_BROWSER_EXT = New PropertyValue(browserExt, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_BROWSER_EXT), v))
            HH_PLATFORM = New PropertyValue(platform, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_PLATFORM), v))
            HH_USER_AGENT = New PropertyValue(useragent, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_USER_AGENT), v))

            DownloadTimeline = New PropertyValue(True)
            DownloadStories = New PropertyValue(True)
            DownloadStoriesUser = New PropertyValue(True)
            DownloadTagged = New PropertyValue(False)

            RequestsWaitTimer = New PropertyValue(1000)
            RequestsWaitTimerProvider = New TimersChecker(100)
            RequestsWaitTimerTaskCount = New PropertyValue(1)
            RequestsWaitTimerTaskCountProvider = New TimersChecker(1)
            SleepTimerOnPostsLimit = New PropertyValue(60000)
            SleepTimerOnPostsLimitProvider = New TimersChecker(10000)

            GetTimeline = New PropertyValue(True)
            GetStories = New PropertyValue(False)
            GetStoriesUser = New PropertyValue(False)
            GetTagged = New PropertyValue(False)
            TaggedNotifyLimit = New PropertyValue(200)
            TaggedNotifyLimitProvider = New TaggedNotifyLimitChecker

            DownloadingErrorDate = New XMLValue(Of Date) With {.Provider = New XMLValueConversionProvider(Function(ss, nn, vv, dd) AConvert(Of String)(vv, AModes.Var, Nothing))}
            DownloadingErrorDate.SetExtended("InstagramDownloadingErrorDate", Now.AddYears(-10), _XML, n)
            LastDownloadDate = New XMLValue(Of Date)("LastDownloadDate", Now.AddDays(-1), _XML, n)
            LastRequestsCount = New XMLValue(Of Integer)("LastRequestsCount", 0, _XML, n)
            LastRequestsCountLabel = New PropertyValue(LastRequestsCountLabelStr.Invoke(LastRequestsCount.Value))
            AddHandler LastRequestsCount.ValueChanged, Sub(sender, e) LastRequestsCountLabel.Value = LastRequestsCountLabelStr.Invoke(DirectCast(sender, XMLValue(Of Integer)).ValueF.Value)

            _AllowUserAgentUpdate = False
            UrlPatternUser = "https://www.instagram.com/{0}/"
            UserRegex = RParams.DMS("[htps:/]{7,8}.*?instagram.com/([^/]+)", 1)
            ImageVideoContains = "instagram.com"
        End Sub
#End Region
#Region "PropertiesDataChecker"
        <PropertiesDataChecker({NameOf(TaggedNotifyLimit)})>
        Private Function CheckNotifyLimit(ByVal p As IEnumerable(Of PropertyData)) As Boolean
            If p.ListExists Then
                Dim pi% = p.ListIndexOf(Function(pp) pp.Name = NameOf(TaggedNotifyLimit))
                If pi >= 0 Then
                    Dim v% = AConvert(Of Integer)(p(pi).Value, -10)
                    If v > 0 Then
                        Return True
                    ElseIf v = -1 Then
                        Return MsgBoxE({"You turn off notifications for tagged posts. This is highly undesirable. Do you still want to do it?",
                                        "Disabling tagged notification limits"}, MsgBoxStyle.YesNo) = MsgBoxResult.Yes
                    Else
                        Return False
                    End If
                End If
            End If
            Return False
        End Function
#End Region
#Region "GetInstance"
        Friend Overrides Function GetInstance(ByVal What As Download) As IPluginContentProvider
            Return New UserData
        End Function
#End Region
#Region "Downloading"
        Friend Property SkipUntilNextSession As Boolean = False
        Friend Overrides Function ReadyToDownload(ByVal What As Download) As Boolean
            Return ActiveJobs < 2 AndAlso Not SkipUntilNextSession AndAlso ReadyForDownload AndAlso BaseAuthExists() AndAlso DownloadTimeline.Value
        End Function
        Private ActiveJobs As Integer = 0
        Private _NextWNM As UserData.WNM = UserData.WNM.Notify
        Private _NextTagged As Boolean = True
        Friend Overrides Sub DownloadStarted(ByVal What As Download)
            ActiveJobs += 1
            If LastDownloadDate.Value.AddMinutes(120) < Now Or Not ACheck(HH_IG_WWW_CLAIM.Value) Then HH_IG_WWW_CLAIM.Value = "0"
        End Sub
        Friend Overrides Sub BeforeStartDownload(ByVal User As Object, ByVal What As Download)
            With DirectCast(User, UserData)
                If What = Download.Main Then
                    .WaitNotificationMode = _NextWNM
                    .TaggedCheckSession = _NextTagged
                End If
                If LastDownloadDate.Value.AddMinutes(60) > Now Then
                    .RequestsCount = LastRequestsCount
                Else
                    LastRequestsCount.Value = 0
                    .RequestsCount = 0
                End If
            End With
        End Sub
        Friend Overrides Sub AfterDownload(ByVal User As Object, ByVal What As Download)
            With DirectCast(User, UserData)
                _NextWNM = .WaitNotificationMode
                If _NextWNM = UserData.WNM.SkipTemp Or _NextWNM = UserData.WNM.SkipCurrent Then _NextWNM = UserData.WNM.Notify
                _NextTagged = .TaggedCheckSession
                LastDownloadDate.Value = Now
                LastRequestsCount.Value = .RequestsCount
                _FieldsChangerSuspended = True
                HH_IG_WWW_CLAIM.Value = Responser.Headers.Value(Header_IG_WWW_CLAIM)
                HH_CSRF_TOKEN.Value = Responser.Headers.Value(Header_CSRF_TOKEN)
                _FieldsChangerSuspended = False
            End With
        End Sub
        Friend Overrides Sub DownloadDone(ByVal What As Download)
            _NextWNM = UserData.WNM.Notify
            _NextTagged = True
            LastDownloadDate.Value = Now
            ActiveJobs -= 1
            SkipUntilNextSession = False
        End Sub
#End Region
#Region "UserOptions, GetUserUrl, GetUserPostUrl"
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse Not TypeOf Options Is EditorExchangeOptions Then Options = New EditorExchangeOptions(Me)
            If OpenForm Then
                Using f As New InternalSettingsForm(Options, Me, False) : f.ShowDialog() : End Using
            End If
        End Sub
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider) As String
            Return String.Format(UrlPatternUser, DirectCast(User, UserData).NameTrue)
        End Function
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            Try
                Dim code$ = DirectCast(User, UserData).GetPostCodeById(Media.Post.ID)
                If Not code.IsEmptyString Then Return $"https://instagram.com/p/{code}/" Else Return String.Empty
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "Can't open user's post", String.Empty)
            End Try
        End Function
#End Region
    End Class
End Namespace