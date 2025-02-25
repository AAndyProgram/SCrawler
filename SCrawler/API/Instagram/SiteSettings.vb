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
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Cookies
Imports Download = SCrawler.Plugin.ISiteSettings.Download
Imports DN = SCrawler.API.Base.DeclaredNames
Namespace API.Instagram
    <Manifest(InstagramSiteKey), SeparatedTasks(1), SavedPosts, SpecialForm(False), UseDownDetector>
    Friend Class SiteSettings : Inherits SiteSettingsBase : Implements DownDetector.IDownDetector
#Region "Declarations"
#Region "Providers"
        Friend Class TimersChecker : Inherits FieldsCheckerProviderBase
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
                    ErrorMessage = $"The value of '{Name}' field must be greater than or equal to {_LowestValue.NumToString(LVProvider)}"
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
                    ErrorMessage = $"The value of '{Name}' field must be greater than 0 or equal to -1"
                    HasError = True
                    Return Nothing
                End If
            End Function
        End Class
#End Region
#Region "Categories"
        Private Const CAT_DOWN As String = "Download data"
        Private Const CAT_UserDefs_VIDEO As String = DN.CAT_UserDefs & ": extract image from video"
        Private Const CAT_ERRORS As String = "Errors"
#End Region
#Region "Properties"
#Region "Authorization"
        Friend Const Header_IG_APP_ID As String = "x-ig-app-id"
        Friend Const Header_IG_WWW_CLAIM As String = "x-ig-www-claim"
        Friend Const Header_CSRF_TOKEN As String = "x-csrftoken"
        Friend Const Header_CSRF_TOKEN_COOKIE As String = "csrftoken"
        Friend Const Header_ASBD_ID As String = "X-Asbd-Id"
        Friend Const Header_Browser As String = "Sec-Ch-Ua"
        Friend Const Header_BrowserExt As String = "Sec-Ch-Ua-Full-Version-List"
        Friend Const Header_Platform_Verion As String = "Sec-Ch-Ua-Platform-Version"
        <PropertyOption(ControlText:="x-csrftoken", ControlToolTip:="Can be automatically extracted from cookies", IsAuth:=True, AllowNull:=True), PClonable(Clone:=False)>
        Friend ReadOnly Property HH_CSRF_TOKEN As PropertyValue
        <CookieValueExtractor(NameOf(HH_CSRF_TOKEN))>
        Private Function GetValueFromCookies(ByVal PropName As String, ByVal c As CookieKeeper) As String
            Return c.GetCookieValue(Header_CSRF_TOKEN_COOKIE, PropName, NameOf(HH_CSRF_TOKEN))
        End Function
        <PropertyOption(ControlText:="x-ig-app-id", IsAuth:=True, AllowNull:=False), PClonable(Clone:=False)>
        Friend ReadOnly Property HH_IG_APP_ID As PropertyValue
        <PropertyOption(ControlText:="x-asbd-id", IsAuth:=True, AllowNull:=True), PClonable(Clone:=False)>
        Friend ReadOnly Property HH_ASBD_ID As PropertyValue
        'PropertyOption(ControlText:="x-ig-www-claim", IsAuth:=True, AllowNull:=True)
        <PClonable(Clone:=False)>
        Friend ReadOnly Property HH_IG_WWW_CLAIM As PropertyValue
        Private ReadOnly Property HH_IG_WWW_CLAIM_IS_ZERO As Boolean
            Get
                Dim v$ = AConvert(Of String)(HH_IG_WWW_CLAIM.Value, String.Empty)
                Return Not v.IsEmptyString AndAlso v = "0"
            End Get
        End Property
        <PropertyOption(ControlText:="sec-ch-ua", IsAuth:=True, AllowNull:=True,
                        InheritanceName:=SettingsCLS.HEADER_DEF_sec_ch_ua), PClonable, PXML(OnlyForChecked:=True)>
        Private ReadOnly Property HH_BROWSER As PropertyValue
        <PropertyOption(ControlText:="sec-ch-ua-full", ControlToolTip:="sec-ch-ua-full-version-list", IsAuth:=True, AllowNull:=True,
                        InheritanceName:=SettingsCLS.HEADER_DEF_sec_ch_ua_full_version_list), PClonable, PXML(OnlyForChecked:=True)>
        Private ReadOnly Property HH_BROWSER_EXT As PropertyValue
        <PropertyOption(ControlText:="sec-ch-ua-platform-ver", ControlToolTip:="sec-ch-ua-platform-version", IsAuth:=True, AllowNull:=True, LeftOffset:=135,
                        InheritanceName:=SettingsCLS.HEADER_DEF_sec_ch_ua_platform_version), PClonable, PXML(OnlyForChecked:=True)>
        Private ReadOnly Property HH_PLATFORM As PropertyValue
        <PropertyOption(ControlText:="UserAgent", IsAuth:=True, AllowNull:=True,
                        InheritanceName:=SettingsCLS.HEADER_DEF_UserAgent), PClonable, PXML(OnlyForChecked:=True)>
        Private ReadOnly Property HH_USER_AGENT As PropertyValue
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
                    Case NameOf(HH_PLATFORM) : f = Header_Platform_Verion
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
#Region "HH_IG_WWW_CLAIM"
        <PropertyOption(ControlText:="ig-www-claim update interval", IsAuth:=True, LeftOffset:=150), PXML, PClonable, HiddenControl>
        Private ReadOnly Property HH_IG_WWW_CLAIM_UPDATE_INTERVAL As PropertyValue
        <PropertyOption(ControlText:="ig-www-claim: always 0", ControlToolTip:="Keep token value always = 0", IsAuth:=True),
            PXML, PClonable, HiddenControl>
        Friend ReadOnly Property HH_IG_WWW_CLAIM_ALWAYS_ZERO As PropertyValue
        <PropertyOption(ControlText:="ig-www-claim: reset each session", ControlToolTip:="Set 'x-ig-www-claim' to '0' before each session", IsAuth:=True),
            PXML, PClonable, HiddenControl>
        Friend ReadOnly Property HH_IG_WWW_CLAIM_RESET_EACH_SESSION As PropertyValue
        <PropertyOption(ControlText:="ig-www-claim: reset each target", ControlToolTip:="Set 'x-ig-www-claim' to '0' before each target", IsAuth:=True),
            PXML, PClonable, HiddenControl>
        Friend ReadOnly Property HH_IG_WWW_CLAIM_RESET_EACH_TARGET As PropertyValue
        <PropertyOption(ControlText:="ig-www-claim: use in requests", IsAuth:=True), PXML, PClonable, HiddenControl>
        Friend ReadOnly Property HH_IG_WWW_CLAIM_USE As PropertyValue
        <PropertyOption(ControlText:="ig-www-claim: use default algorithm to update", IsAuth:=True), PXML, PClonable, HiddenControl>
        Friend ReadOnly Property HH_IG_WWW_CLAIM_USE_DEFAULT_ALGO As PropertyValue
        <Provider(NameOf(HH_IG_WWW_CLAIM_UPDATE_INTERVAL), FieldsChecker:=True)>
        Private ReadOnly Property TokenUpdateIntervalProvider As IFormatProvider
#End Region
        <PropertyOption(ControlText:="Use GraphQL to download", IsAuth:=True), PXML, PClonable>
        Friend ReadOnly Property USE_GQL As PropertyValue
#End Region
#Region "Download data"
        <PropertyOption(ControlText:="Download timeline", Category:=CAT_DOWN), PXML, PClonable>
        Friend ReadOnly Property DownloadTimeline As PropertyValue
        <PXML> Private ReadOnly Property DownloadTimeline_Def As PropertyValue
        <PropertyOption(ControlText:="Download reels", Category:=CAT_DOWN), PXML, PClonable>
        Friend ReadOnly Property DownloadReels As PropertyValue
        <PXML> Private ReadOnly Property DownloadReels_Def As PropertyValue
        <PropertyOption(ControlText:="Download stories", Category:=CAT_DOWN), PXML, PClonable>
        Friend ReadOnly Property DownloadStories As PropertyValue
        <PXML> Private ReadOnly Property DownloadStories_Def As PropertyValue
        <PropertyOption(ControlText:="Download stories: user", Category:=CAT_DOWN), PXML, PClonable>
        Friend ReadOnly Property DownloadStoriesUser As PropertyValue
        <PXML> Private ReadOnly Property DownloadStoriesUser_Def As PropertyValue
        <PropertyOption(ControlText:="Download tagged posts", Category:=CAT_DOWN), PXML, PClonable>
        Friend ReadOnly Property DownloadTagged As PropertyValue
        <PXML> Private ReadOnly Property DownloadTagged_Def As PropertyValue
#End Region
#Region "Timers"
        Friend Const TimersUrgentTip As String = vbCr & "It is highly recommended not to change the default value."
        <PropertyOption(ControlText:="Request timer (any)",
                        ControlToolTip:="The timer (in milliseconds) that SCrawler should wait before executing the next request." &
                        vbCr & "The default value is 1'000." & vbCr & "The minimum value is 0." & TimersUrgentTip, AllowNull:=False, Category:=DN.CAT_Timers),
                        PXML, PClonable>
        Friend ReadOnly Property RequestsWaitTimer_Any As PropertyValue
        <Provider(NameOf(RequestsWaitTimer_Any), FieldsChecker:=True)>
        Private ReadOnly Property RequestsWaitTimer_AnyProvider As IFormatProvider
        <PropertyOption(ControlText:="Request timer",
                        ControlToolTip:="The time value (in milliseconds) that the program will wait before processing the next 'Request time counter' request." &
                                        vbCr & "The default value is 1'000." & vbCr & "The minimum value is 100." & TimersUrgentTip,
                        AllowNull:=False, Category:=DN.CAT_Timers), PXML, PClonable>
        Friend ReadOnly Property RequestsWaitTimer As PropertyValue
        <Provider(NameOf(RequestsWaitTimer), FieldsChecker:=True)>
        Private ReadOnly Property RequestsWaitTimerProvider As IFormatProvider
        <PropertyOption(ControlText:="Request timer counter",
                        ControlToolTip:="How many requests will be sent to Instagram before the program waits 'Request timer'." &
                                        vbCr & "The default value is 1." & vbCr & "The minimum value is 1." & TimersUrgentTip,
                        AllowNull:=False, LeftOffset:=120, Category:=DN.CAT_Timers), PXML, PClonable>
        Friend ReadOnly Property RequestsWaitTimerTaskCount As PropertyValue
        <Provider(NameOf(RequestsWaitTimerTaskCount), FieldsChecker:=True)>
        Private ReadOnly Property RequestsWaitTimerTaskCountProvider As IFormatProvider
        <PropertyOption(ControlText:="Posts limit timer",
                        ControlToolTip:="The time value (in milliseconds) the program will wait before processing the next request after 195 requests." &
                                        vbCr & "The default value is 60'000." & vbCr & "The minimum value is 10'000." & TimersUrgentTip,
                        AllowNull:=False, Category:=DN.CAT_Timers), PXML, PClonable>
        Friend ReadOnly Property SleepTimerOnPostsLimit As PropertyValue
        <Provider(NameOf(SleepTimerOnPostsLimit), FieldsChecker:=True)>
        Private ReadOnly Property SleepTimerOnPostsLimitProvider As IFormatProvider
#End Region
#Region "New user defaults"
        <PropertyOption(ControlText:="Get timeline", ControlToolTip:="Default value for new users", Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property GetTimeline As PropertyValue
        <PropertyOption(ControlText:="From timeline", ControlToolTip:="Default value for new users", Category:=CAT_UserDefs_VIDEO), PXML, PClonable>
        Friend ReadOnly Property GetTimeline_VideoPic As PropertyValue
        <PropertyOption(ControlText:="Get reels", ControlToolTip:="Default value for new users", Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property GetReels As PropertyValue
        <PropertyOption(ControlText:="From reels", ControlToolTip:="Default value for new users", Category:=CAT_UserDefs_VIDEO), PXML, PClonable>
        Friend ReadOnly Property GetReels_VideoPic As PropertyValue
        <PropertyOption(ControlText:="Get stories", ControlToolTip:="Default value for new users", Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property GetStories As PropertyValue
        <PropertyOption(ControlText:="From stories", ControlToolTip:="Default value for new users", Category:=CAT_UserDefs_VIDEO), PXML, PClonable>
        Friend ReadOnly Property GetStories_VideoPic As PropertyValue
        <PropertyOption(ControlText:="Get stories: user", ControlToolTip:="Default value for new users", Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property GetStoriesUser As PropertyValue
        <PropertyOption(ControlText:="From stories: user", ControlToolTip:="Default value for new users", Category:=CAT_UserDefs_VIDEO), PXML, PClonable>
        Friend ReadOnly Property GetStoriesUser_VideoPic As PropertyValue
        <PropertyOption(ControlText:="Get tagged posts", ControlToolTip:="Default value for new users", Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property GetTagged As PropertyValue
        <PropertyOption(ControlText:="From tagged posts", ControlToolTip:="Default value for new users", Category:=CAT_UserDefs_VIDEO), PXML, PClonable>
        Friend ReadOnly Property GetTagged_VideoPic As PropertyValue
        <PropertyOption(ControlText:="From saved posts", ControlToolTip:="Default value for new users", Category:=CAT_UserDefs_VIDEO), PXML, PClonable>
        Friend ReadOnly Property GetSavedPosts_VideoPic As PropertyValue
        <PropertyOption(ControlText:="Place the extracted image into the video folder", ControlToolTip:="Default value for new users", Category:=CAT_UserDefs_VIDEO), PXML, PClonable>
        Friend ReadOnly Property PutImageVideoFolder As PropertyValue
#End Region
#Region "Errors"
        Private Const ErrorsDefault As String = "572"
        <PropertyOption(ControlText:="Skip errors",
                        ControlToolTip:="Skip the following errors (comma separated)." & vbCr &
                                        "Facing these errors will not disable the download, but will add a simple line to the log.", Category:=CAT_ERRORS),
         PClonable, PXML>
        Private ReadOnly Property SkipErrors As PropertyValue
        <PropertyOption(ControlText:="Add skipped errors to the log", Category:=CAT_ERRORS), PClonable, PXML>
        Private ReadOnly Property SkipErrors_AddToLog As PropertyValue
        <PropertyOption(ControlText:="Skip errors (exclude)",
                        ControlToolTip:="Exclude the following errors from being added to the log (comma separated)", Category:=CAT_ERRORS), PClonable, PXML>
        Private ReadOnly Property SkipErrors_AddToLog_Silent As PropertyValue
        Friend ReadOnly Property ErrorSpecialHandling(ByVal ErrCode As Integer) As Boolean
            Get
                With CStr(SkipErrors.Value) : Return Not .IsEmptyString AndAlso .Contains(ErrCode) : End With
            End Get
        End Property
        Friend ReadOnly Property ErrorSpecialHandling_AddToLog(ByVal ErrCode As Integer) As Boolean
            Get
                With CStr(SkipErrors_AddToLog_Silent.Value)
                    Return CBool(SkipErrors_AddToLog.Value) AndAlso (.IsEmptyString OrElse Not .Contains(ErrCode))
                End With
            End Get
        End Property
        <PropertyOption(ControlText:="Ignore stories downloading errors (560)",
                        ControlToolTip:="If checked, error 560 will be skipped and the download will continue. Otherwise, the download will be interrupted.",
                        Category:=CAT_ERRORS), PClonable, PXML>
        Friend ReadOnly Property IgnoreStoriesDownloadingErrors As PropertyValue
#End Region
#Region "Other params"
        <PropertyOption(ControlText:="DownDetector",
                        ControlToolTip:="Use 'DownDetector' to determine if the site is accessible. -1 to disable." & vbCr &
                                        "The value represents the average number of error reports over the last 4 hours"),
            PClonable, PXML>
        Private ReadOnly Property DownDetectorValue As PropertyValue
        <Provider(NameOf(DownDetectorValue), FieldsChecker:=True)>
        Private ReadOnly Property DownDetectorValueProvider As IFormatProvider
        <PropertyOption(ControlText:="Add 'DownDetector' information to the log."), PClonable, PXML, HiddenControl>
        Private ReadOnly Property DownDetectorValueAddToLog As PropertyValue
        <PropertyOption(ControlText:="Tagged notify limit",
                        ControlToolTip:="If the number of tagged posts exceeds this number you will be notified." & vbCr &
                        "-1 to disable"), PXML, PClonable>
        Friend ReadOnly Property TaggedNotifyLimit As PropertyValue
        <Provider(NameOf(TaggedNotifyLimit), FieldsChecker:=True)>
        Private ReadOnly Property TaggedNotifyLimitProvider As IFormatProvider
#End Region
#End Region
#Region "IDownDetector Support"
        Private ReadOnly Property IDownDetector_Value As Integer Implements DownDetector.IDownDetector.Value
            Get
                Return DownDetectorValue.Value
            End Get
        End Property
        Private ReadOnly Property IDownDetector_AddToLog As Boolean Implements DownDetector.IDownDetector.AddToLog
            Get
                Return DownDetectorValueAddToLog.Value
            End Get
        End Property
        Private ReadOnly Property IDownDetector_CheckSite As String Implements DownDetector.IDownDetector.CheckSite
            Get
                Return "instagram"
            End Get
        End Property
        Private Function IDownDetector_Available(ByVal What As Download, ByVal Silent As Boolean) As Boolean Implements DownDetector.IDownDetector.Available
            Return MDD.Available(What, Silent)
        End Function
#End Region
#Region "429 bypass"
        <PXML("InstagramDownloadingErrorDate")>
        Private ReadOnly Property DownloadingErrorDate As PropertyValue
        <Provider(NameOf(DownloadingErrorDate))>
        Private ReadOnly Property DownloadingErrorDateProvider As IFormatProvider =
                                  New CustomProvider(Function(ByVal v As Object, ByVal d As Type) As Object
                                                         If d Is GetType(Date) Then
                                                             Return AConvert(Of Date)(v, AModes.Var, Nothing)
                                                         ElseIf d Is GetType(String) Then
                                                             If Not IsNothing(v) AndAlso TypeOf v Is Date AndAlso CDate(v) = Date.MinValue Then
                                                                 Return String.Empty
                                                             Else
                                                                 Return AConvert(Of String)(v, AModes.XML, String.Empty)
                                                             End If
                                                         Else
                                                             Return Nothing
                                                         End If
                                                     End Function)
        Friend Property LastApplyingValue As Integer? = Nothing
        Friend ReadOnly Property ReadyForDownload As Boolean
            Get
                If SkipUntilNextSession Then Return False
                With DownloadingErrorDate
                    If ACheck(Of Date)(.Value) Then
                        Return CDate(.Value).AddMinutes(If(LastApplyingValue, 10)) < Now
                    Else
                        Return True
                    End If
                End With
            End Get
        End Property
        Private Const LastDownloadDateResetInterval As Integer = 60
        Private TooManyRequestsReadyForCatch As Boolean = True
        Friend Function GetWaitDate() As Date
            With DownloadingErrorDate
                If ACheck(Of Date)(.Value) Then
                    Return CDate(.Value).AddMinutes(If(LastApplyingValue, 10))
                Else
                    Return Now
                End If
            End With
        End Function
        Friend Sub TooManyRequests(ByVal Catched As Boolean)
            With DownloadingErrorDate
                If Catched Then
                    If Not ACheck(Of Date)(.Value) Then
                        .Value = Now
                        If TooManyRequestsReadyForCatch Then
                            LastApplyingValue = If(LastApplyingValue, 0) + 10
                            TooManyRequestsReadyForCatch = False
                            MyMainLOG = $"Instagram downloading error: too many requests. Try again after {If(LastApplyingValue, 10)} minutes..."
                        End If
                    End If
                Else
                    .Value = Nothing
                    LastApplyingValue = Nothing
                    TooManyRequestsReadyForCatch = True
                End If
            End With
        End Sub
#End Region
#Region "LastRequestsCount, Label"
        <PXML> Private ReadOnly Property LastDownloadDate As PropertyValue
        <PXML> Private ReadOnly Property LastRequestsCount As PropertyValue
        <PropertyOption(IsInformationLabel:=True)>
        Private ReadOnly Property LastRequestsCountLabel As PropertyValue
        Private ReadOnly MyLastRequests As Dictionary(Of Date, Integer)
        Private ReadOnly Property MyLastRequestsDate As Date
            Get
                Try
                    Return If(MyLastRequests.Count > 0, MyLastRequests.Keys.Max, Now.AddDays(-1))
                Catch ex As Exception
                    Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, "[SiteSettings.Instagram.MyLastRequestsDate]", Now.AddDays(-1))
                End Try
            End Get
        End Property
        Private Property MyLastRequestsCount As Integer
            Get
                Try
                    Return If(MyLastRequests.Count > 0, MyLastRequests.Values.Sum, 0)
                Catch ex As Exception
                    Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, "[SiteSettings.Instagram.MyLastRequestsCount]", 0)
                End Try
            End Get
            Set(ByVal NewValue As Integer)
                If Not MyLastRequests.ContainsKey(ActiveSessionDate) Then
                    MyLastRequests.Add(ActiveSessionDate, NewValue)
                Else
                    MyLastRequests(ActiveSessionDate) += NewValue
                End If
            End Set
        End Property
        Private Sub RefreshMyLastRequests(Optional ByVal DateToReplace As Date? = Nothing)
            Try
                With MyLastRequests
                    If .Count > 0 Then
                        Dim d As Date
                        For i% = .Count - 1 To 0 Step -1
                            d = .Keys(i)
                            If (Not DateToReplace.HasValue OrElse ActiveJobs < 1 OrElse d <> ActiveSessionDate) And
                               d.AddMinutes(LastDownloadDateResetInterval) < Now Then .Remove(d)
                        Next
                    End If
                    If .Count > 0 Then
                        If DateToReplace.HasValue Then
                            If .Keys.Contains(ActiveSessionDate) Then
                                Dim v% = .Item(ActiveSessionDate)
                                .Remove(ActiveSessionDate)
                                .Add(DateToReplace.Value, v)
                            End If
                        End If
                        LastDownloadDate.Value = .Keys.Max
                        LastRequestsCount.Value = .Values.Sum
                    End If
                End With
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[SiteSettings.Instagram.RefreshMyLastRequests]")
            End Try
        End Sub
#End Region
#End Region
#Region "Initializer"
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New(InstagramSite, "instagram.com", AccName, Temp, My.Resources.SiteResources.InstagramIcon_32, My.Resources.SiteResources.InstagramPic_76)

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
                If .UserAgentExists Then useragent = .UserAgent Else .UserAgent = String.Empty
                With .Headers
                    If .Count > 0 Then
                        token = .Value(Header_CSRF_TOKEN)
                        app_id = .Value(Header_IG_APP_ID)
                        www_claim = .Value(Header_IG_WWW_CLAIM)
                        asbd = .Value(Header_ASBD_ID)
                        browser = .Value(Header_Browser)
                        browserExt = .Value(Header_BrowserExt)
                        platform = .Value(Header_Platform_Verion)
                    End If
                    '.Add(Header_IG_WWW_CLAIM, 0)
                    .Add("Origin", "https://www.instagram.com")
                    .Add("authority", "www.instagram.com")
                    .Add("Dnt", 1)
                    '.Add("Dpr", 1)
                    .Remove("Dpr")
                    .Add("Sec-Ch-Ua-Mobile", "?0")
                    .Add("Sec-Ch-Ua-Model", """""")
                    .Add("Sec-Ch-Ua-Platform", """Windows""")
                    .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecFetchDest, "empty"))
                    .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.SecFetchMode, "cors"))
                    .Add("Sec-Fetch-Site", "same-origin")
                    .Add("X-Requested-With", "XMLHttpRequest")
                End With
                .CookiesExtractMode = Responser.CookiesExtractModes.Response
                .CookiesUpdateMode = CookieKeeper.UpdateModes.ReplaceByNameAll
                .CookiesExtractedAutoSave = False
            End With

            HH_CSRF_TOKEN = New PropertyValue(token, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_CSRF_TOKEN), v))
            HH_IG_APP_ID = New PropertyValue(app_id, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_IG_APP_ID), v))
            HH_ASBD_ID = New PropertyValue(asbd, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_ASBD_ID), v))
            HH_IG_WWW_CLAIM = New PropertyValue(www_claim.IfNullOrEmpty(0), GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_IG_WWW_CLAIM), v))
            HH_BROWSER = New PropertyValue(browser, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_BROWSER), v))
            HH_BROWSER_EXT = New PropertyValue(browserExt, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_BROWSER_EXT), v))
            HH_PLATFORM = New PropertyValue(platform, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_PLATFORM), v))
            HH_USER_AGENT = New PropertyValue(useragent, GetType(String), Sub(v) ChangeResponserFields(NameOf(HH_USER_AGENT), v))

            HH_IG_WWW_CLAIM_UPDATE_INTERVAL = New PropertyValue(120)
            HH_IG_WWW_CLAIM_ALWAYS_ZERO = New PropertyValue(False)
            HH_IG_WWW_CLAIM_RESET_EACH_SESSION = New PropertyValue(True)
            HH_IG_WWW_CLAIM_RESET_EACH_TARGET = New PropertyValue(True)
            HH_IG_WWW_CLAIM_USE = New PropertyValue(True)
            HH_IG_WWW_CLAIM_USE_DEFAULT_ALGO = New PropertyValue(True)
            TokenUpdateIntervalProvider = New TokenRefreshIntervalProvider
            USE_GQL = New PropertyValue(False)

            DownloadTimeline = New PropertyValue(True)
            DownloadTimeline_Def = New PropertyValue(DownloadTimeline.Value, GetType(Boolean))
            DownloadReels = New PropertyValue(False)
            DownloadReels_Def = New PropertyValue(DownloadReels.Value, GetType(Boolean))
            DownloadStories = New PropertyValue(True)
            DownloadStories_Def = New PropertyValue(DownloadStories.Value, GetType(Boolean))
            DownloadStoriesUser = New PropertyValue(True)
            DownloadStoriesUser_Def = New PropertyValue(DownloadStoriesUser.Value, GetType(Boolean))
            DownloadTagged = New PropertyValue(False)
            DownloadTagged_Def = New PropertyValue(DownloadTagged.Value, GetType(Boolean))

            RequestsWaitTimer_Any = New PropertyValue(1000)
            RequestsWaitTimer_AnyProvider = New TimersChecker(0)
            RequestsWaitTimer = New PropertyValue(1000)
            RequestsWaitTimerProvider = New TimersChecker(100)
            RequestsWaitTimerTaskCount = New PropertyValue(1)
            RequestsWaitTimerTaskCountProvider = New TimersChecker(1)
            SleepTimerOnPostsLimit = New PropertyValue(60000)
            SleepTimerOnPostsLimitProvider = New TimersChecker(10000)

            GetTimeline = New PropertyValue(True)
            GetTimeline_VideoPic = New PropertyValue(True)
            GetReels = New PropertyValue(False)
            GetReels_VideoPic = New PropertyValue(True)
            GetStories = New PropertyValue(False)
            GetStories_VideoPic = New PropertyValue(True)
            GetStoriesUser = New PropertyValue(False)
            GetStoriesUser_VideoPic = New PropertyValue(True)
            GetTagged = New PropertyValue(False)
            GetTagged_VideoPic = New PropertyValue(True)
            GetSavedPosts_VideoPic = New PropertyValue(True)
            PutImageVideoFolder = New PropertyValue(False)

            SkipErrors = New PropertyValue(ErrorsDefault)
            SkipErrors_AddToLog = New PropertyValue(True)
            SkipErrors_AddToLog_Silent = New PropertyValue(String.Empty, GetType(String))
            IgnoreStoriesDownloadingErrors = New PropertyValue(False)

            DownDetectorValue = New PropertyValue(20)
            DownDetectorValueProvider = New TimersChecker(-1)
            DownDetectorValueAddToLog = New PropertyValue(False)
            TaggedNotifyLimit = New PropertyValue(200)
            TaggedNotifyLimitProvider = New TaggedNotifyLimitChecker

            DownloadingErrorDate = New PropertyValue(Now.AddYears(-10), GetType(Date))
            LastDownloadDate = New PropertyValue(Now.AddDays(-1))
            LastRequestsCount = New PropertyValue(0)
            LastRequestsCountLabel = New PropertyValue(String.Empty, GetType(String))
            MyLastRequests = New Dictionary(Of Date, Integer)

            MDD = New DownDetector.Checker(Of SiteSettings)(Me)

            _AllowUserAgentUpdate = False
            UrlPatternUser = "https://www.instagram.com/{0}/"
            UserRegex = RParams.DMS(String.Format(UserRegexDefaultPattern, "instagram.com/"), 1)
            ImageVideoContains = "instagram.com"
        End Sub
        Private Const SettingsVersionCurrent As Integer = 2
        Friend Overrides Sub EndInit()
            Try : MyLastRequests.Add(LastDownloadDate.Value, LastRequestsCount.Value) : Catch : End Try
            If Not CBool(HH_IG_WWW_CLAIM_USE.Value) Then Responser.Headers.Remove(Header_IG_WWW_CLAIM)
            If CInt(SettingsVersion.Value) < SettingsVersionCurrent Then
                SettingsVersion.Value = SettingsVersionCurrent
                HH_IG_WWW_CLAIM_UPDATE_INTERVAL.Value = 120
                HH_IG_WWW_CLAIM_ALWAYS_ZERO.Value = False
                HH_IG_WWW_CLAIM_RESET_EACH_SESSION.Value = True
                HH_IG_WWW_CLAIM_RESET_EACH_TARGET.Value = True
                HH_IG_WWW_CLAIM_USE.Value = True
                HH_IG_WWW_CLAIM_USE_DEFAULT_ALGO.Value = True
            End If
            MyBase.EndInit()
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
        Private ReadOnly MDD As DownDetector.Checker(Of SiteSettings)
        Private Sub ResetDownloadOptions()
            If ActiveJobs < 1 Then
                MDD.Reset()
                If ActiveSessionRequestsExists Then RefreshMyLastRequests(Now)
                ActiveSessionRequestsExists = False
                _NextWNM = UserData.WNM.Notify
                _NextTagged = True
                SkipUntilNextSession = False
                AvailableText = String.Empty
                ActiveJobs = 0
            End If
        End Sub
        Friend Overrides Function Available(ByVal What As Download, ByVal Silent As Boolean) As Boolean
            Return MyBase.Available(What, Silent) And ActiveJobs < 2
        End Function
        Friend Property SkipUntilNextSession As Boolean = False
        Friend Overrides Function ReadyToDownload(ByVal What As Download) As Boolean
            Return ActiveJobs < 2 AndAlso Not SkipUntilNextSession AndAlso ReadyForDownload AndAlso BaseAuthExists() AndAlso CBool(DownloadTimeline.Value)
        End Function
        Private ActiveJobs As Integer = 0
        Private ActiveSessionDate As Date
        Private ActiveSessionRequestsExists As Boolean = False
        Private _NextWNM As UserData.WNM = UserData.WNM.Notify
        Private _NextTagged As Boolean = True
        Friend Overrides Sub DownloadStarted(ByVal What As Download)
            ResetDownloadOptions()
            ActiveJobs += 1
            If ActiveJobs = 1 Then ActiveSessionDate = Now
            If Not HH_IG_WWW_CLAIM_IS_ZERO AndAlso
               (
                    (CBool(HH_IG_WWW_CLAIM_USE_DEFAULT_ALGO.Value) AndAlso MyLastRequestsDate.AddMinutes(HH_IG_WWW_CLAIM_UPDATE_INTERVAL.Value) < Now) Or
                    Not ACheck(HH_IG_WWW_CLAIM.Value) Or
                    (
                        Not (
                             CBool(HH_IG_WWW_CLAIM_USE_DEFAULT_ALGO.Value) And
                             (CBool(HH_IG_WWW_CLAIM_RESET_EACH_SESSION.Value) Or CBool(HH_IG_WWW_CLAIM_ALWAYS_ZERO.Value))
                            )
                    )
                ) Then HH_IG_WWW_CLAIM.Value = "0"
        End Sub
        Friend Overrides Sub BeforeStartDownload(ByVal User As Object, ByVal What As Download)
            With DirectCast(User, UserData)
                If What = Download.Main Then
                    .WaitNotificationMode = _NextWNM
                    .TaggedCheckSession = _NextTagged
                End If
                If MyLastRequestsDate.AddMinutes(LastDownloadDateResetInterval) > Now Then
                    .RequestsCount = MyLastRequestsCount
                Else
                    .RequestsCount = 0
                End If
            End With
        End Sub
        Friend Overrides Sub AfterDownload(ByVal User As Object, ByVal What As Download)
            With DirectCast(User, UserData)
                _NextWNM = .WaitNotificationMode
                If _NextWNM = UserData.WNM.SkipTemp Or _NextWNM = UserData.WNM.SkipCurrent Then _NextWNM = UserData.WNM.Notify
                _NextTagged = .TaggedCheckSession
                MyLastRequestsCount = .RequestsCountSession
                If .RequestsCountSession > 0 Then ActiveSessionRequestsExists = True
                _FieldsChangerSuspended = True
                HH_IG_WWW_CLAIM.Value = Responser.Headers.Value(Header_IG_WWW_CLAIM)
                HH_CSRF_TOKEN.Value = Responser.Headers.Value(Header_CSRF_TOKEN)
                _FieldsChangerSuspended = False
            End With
        End Sub
        Friend Overrides Sub DownloadDone(ByVal What As Download)
            ActiveJobs -= 1
            ResetDownloadOptions()
        End Sub
#End Region
#Region "Settings"
        Private ____HH_CSRF_TOKEN As String = String.Empty
        Private ____HH_IG_APP_ID As String = String.Empty
        Private ____HH_ASBD_ID As String = String.Empty
        Private ____HH_BROWSER As String = String.Empty
        Private ____HH_BROWSER_EXT As String = String.Empty
        Private ____HH_PLATFORM As String = String.Empty
        Private ____HH_USER_AGENT As String = String.Empty
        Private ____Cookies As CookieKeeper = Nothing
        Private __DownloadTimeline As Boolean = False
        Private __DownloadReels As Boolean = False
        Private __DownloadStories As Boolean = False
        Private __DownloadStoriesUser As Boolean = False
        Private __DownloadTagged As Boolean = False
        Friend Overrides Sub BeginEdit()
            RefreshMyLastRequests()
            Dim v% = MyLastRequestsCount
            Dim d$ = String.Empty
            If v > 0 Then d = $" ({MyLastRequestsDate.ToStringDate(DateTimeDefaultProvider)})"
            LastRequestsCountLabel.Value = $"Number of spent requests: {v.NumToGroupIntegral}{d}"
            ____HH_CSRF_TOKEN = AConvert(Of String)(HH_CSRF_TOKEN.Value, String.Empty)
            ____HH_IG_APP_ID = AConvert(Of String)(HH_IG_APP_ID.Value, String.Empty)
            ____HH_ASBD_ID = AConvert(Of String)(HH_ASBD_ID.Value, String.Empty)
            ____HH_BROWSER = AConvert(Of String)(HH_BROWSER.Value, String.Empty)
            ____HH_BROWSER_EXT = AConvert(Of String)(HH_BROWSER_EXT.Value, String.Empty)
            ____HH_PLATFORM = AConvert(Of String)(HH_PLATFORM.Value, String.Empty)
            ____HH_USER_AGENT = AConvert(Of String)(HH_USER_AGENT.Value, String.Empty)
            ____Cookies = Responser.Cookies.Copy
            __DownloadTimeline = DownloadTimeline.Value
            __DownloadReels = DownloadReels.Value
            __DownloadStories = DownloadStories.Value
            __DownloadStoriesUser = DownloadStoriesUser.Value
            __DownloadTagged = DownloadTagged.Value
            MyBase.BeginEdit()
        End Sub
        Friend Overrides Sub Update()
            If _SiteEditorFormOpened Then
                Dim vals() = {New With {.ValueOld = ____HH_CSRF_TOKEN, .ValueNew = AConvert(Of String)(HH_CSRF_TOKEN.Value, String.Empty).ToString},
                              New With {.ValueOld = ____HH_IG_APP_ID, .ValueNew = AConvert(Of String)(HH_IG_APP_ID.Value, String.Empty).ToString},
                              New With {.ValueOld = ____HH_ASBD_ID, .ValueNew = AConvert(Of String)(HH_ASBD_ID.Value, String.Empty).ToString},
                              New With {.ValueOld = ____HH_BROWSER, .ValueNew = AConvert(Of String)(HH_BROWSER.Value, String.Empty).ToString},
                              New With {.ValueOld = ____HH_BROWSER_EXT, .ValueNew = AConvert(Of String)(HH_BROWSER_EXT.Value, String.Empty).ToString},
                              New With {.ValueOld = ____HH_PLATFORM, .ValueNew = AConvert(Of String)(HH_PLATFORM.Value, String.Empty).ToString},
                              New With {.ValueOld = ____HH_USER_AGENT, .ValueNew = AConvert(Of String)(HH_USER_AGENT.Value, String.Empty).ToString}
                }
                Dim credentialsUpdated As Boolean = False
                If vals.Any(Function(v) Not v.ValueOld = v.ValueNew) OrElse
                   Not Responser.Cookies.ListEquals(____Cookies) Then HH_IG_WWW_CLAIM.Value = 0 : credentialsUpdated = True
                If Responser.CookiesExists Then
                    Dim csrf$ = GetValueFromCookies(NameOf(HH_CSRF_TOKEN), Responser.Cookies)
                    If Not csrf.IsEmptyString Then
                        If Not AEquals(Of String)(CStr(HH_CSRF_TOKEN.Value), csrf) Then credentialsUpdated = True
                        HH_CSRF_TOKEN.Value = csrf
                    End If
                End If
                If credentialsUpdated AndAlso {New With {.ValueOld = __DownloadTimeline, .ValueNew = CBool(DownloadTimeline.Value)},
                                               New With {.ValueOld = __DownloadReels, .ValueNew = CBool(DownloadReels.Value)},
                                               New With {.ValueOld = __DownloadStories, .ValueNew = CBool(DownloadStories.Value)},
                                               New With {.ValueOld = __DownloadStoriesUser, .ValueNew = CBool(DownloadStoriesUser.Value)},
                                               New With {.ValueOld = __DownloadTagged, .ValueNew = CBool(DownloadTagged.Value)}}.
                                               All(Function(v) v.ValueOld = v.ValueNew) Then
                    DownloadTimeline.Value = DownloadTimeline_Def.Value
                    DownloadReels.Value = DownloadReels_Def.Value
                    DownloadStories.Value = DownloadStories_Def.Value
                    DownloadStoriesUser.Value = DownloadStoriesUser_Def.Value
                    DownloadTagged.Value = DownloadTagged_Def.Value
                End If
                DownloadTimeline_Def.Value = DownloadTimeline.Value
                DownloadReels_Def.Value = DownloadReels.Value
                DownloadStories_Def.Value = DownloadStories.Value
                DownloadStoriesUser_Def.Value = DownloadStoriesUser.Value
                DownloadTagged_Def.Value = DownloadTagged.Value
            End If
            MyBase.Update()
        End Sub
        Friend Overrides Sub EndEdit()
            If _SiteEditorFormOpened Then ____Cookies.DisposeIfReady(False) : ____Cookies = Nothing
            MyBase.EndEdit()
        End Sub
#End Region
#Region "UserOptions, GetUserUrl, GetUserPostUrl"
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse Not TypeOf Options Is EditorExchangeOptions Then Options = New EditorExchangeOptions(Me)
            If OpenForm Then
                Using f As New InternalSettingsForm(Options, Me, False) : f.ShowDialog() : End Using
            End If
        End Sub
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            Try
                Dim code$ = DirectCast(User, UserData).GetPostCodeById(Media.Post.ID)
                If Not code.IsEmptyString Then Return $"https://instagram.com/p/{code}/" Else Return String.Empty
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "Can't open user's post", String.Empty)
            End Try
        End Function
#End Region
#Region "IDisposable Support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing And Not MyLastRequests Is Nothing Then MyLastRequests.Clear()
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace