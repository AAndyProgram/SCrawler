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
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Functions.RegularExpressions
Imports Download = SCrawler.Plugin.ISiteSettings.Download
Namespace API.Instagram
    <Manifest(InstagramSiteKey), SeparatedTasks(1), SavedPosts, SpecialForm(False), SpecialForm(True)>
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
        Private Class TimersChecker : Implements IFieldsCheckerProvider
            Private Property ErrorMessage As String Implements IFieldsCheckerProvider.ErrorMessage
            Private Property Name As String Implements IFieldsCheckerProvider.Name
            Private Property TypeError As Boolean Implements IFieldsCheckerProvider.TypeError
            Private ReadOnly LVProvider As New ANumbers With {.FormatOptions = ANumbers.Options.GroupIntegral}
            Private ReadOnly _LowestValue As Integer
            Friend Sub New(ByVal LowestValue As Integer)
                _LowestValue = LowestValue
            End Sub
            Private Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                     Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object Implements ICustomProvider.Convert
                TypeError = False
                ErrorMessage = String.Empty
                If Not ACheck(Of Integer)(Value) Then
                    TypeError = True
                ElseIf CInt(Value) < _LowestValue Then
                    ErrorMessage = $"The value of [{Name}] field must be greater than or equal to {_LowestValue.NumToString(LVProvider)}"
                Else
                    Return Value
                End If
                Return Nothing
            End Function
            Private Function GetFormat(ByVal FormatType As Type) As Object Implements IFormatProvider.GetFormat
                Throw New NotImplementedException("[GetFormat] is not available in the context of [TimersChecker]")
            End Function
        End Class
        Private Class TaggedNotifyLimitChecker : Implements IFieldsCheckerProvider
            Private Property ErrorMessage As String Implements IFieldsCheckerProvider.ErrorMessage
            Private Property Name As String Implements IFieldsCheckerProvider.Name
            Private Property TypeError As Boolean Implements IFieldsCheckerProvider.TypeError
            Private Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                     Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object Implements ICustomProvider.Convert
                Dim v% = AConvert(Of Integer)(Value, -10)
                If v > 0 Or v = -1 Then
                    Return Value
                Else
                    ErrorMessage = $"The value of [{Name}] field must be greater than 0 or equal to -1"
                    Return Nothing
                End If
            End Function
            Private Function GetFormat(ByVal FormatType As Type) As Object Implements IFormatProvider.GetFormat
                Throw New NotImplementedException("[GetFormat] is not available in the context of [TaggedNotifyLimitChecker]")
            End Function
        End Class
#End Region
#Region "Authorization properties"
        <PropertyOption(ControlText:="Hash", ControlToolTip:="Instagram session hash", IsAuth:=True, AllowNull:=False), PXML("InstaHash"), ControlNumber(0)>
        Friend ReadOnly Property Hash As PropertyValue
        Private Const HashSavedPosts_Text As String = "Hash 2"
        <PropertyOption(ControlText:=HashSavedPosts_Text, ControlToolTip:="Instagram session hash for saved posts", IsAuth:=True), PXML("InstaHashSavedPosts"), ControlNumber(1)>
        Friend ReadOnly Property HashSavedPosts As PropertyValue
        <PropertyOption(ControlText:="x-csrftoken", ControlToolTip:="Instagram token for tagged data", IsAuth:=True, AllowNull:=False), ControlNumber(2)>
        Friend ReadOnly Property CSRF_TOKEN As PropertyValue
        <PropertyOption(ControlText:="x-ig-app-id", IsAuth:=True, AllowNull:=False), ControlNumber(3)>
        Friend Property IG_APP_ID As PropertyValue
        <PropertyOption(ControlText:="x-ig-www-claim", IsAuth:=True, AllowNull:=False), ControlNumber(4)>
        Friend Property IG_WWW_CLAIM As PropertyValue
        Private Const SavedPostsUserName_Text As String = "Saved posts user"
        <PropertyOption(ControlText:=SavedPostsUserName_Text, ControlToolTip:="Personal profile username", IsAuth:=True), PXML("SavedPostsUserName"), ControlNumber(5)>
        Friend ReadOnly Property SavedPostsUserName As PropertyValue
        Friend Overrides Function BaseAuthExists() As Boolean
            Return If(Responser.Cookies?.Count, 0) > 0 And ACheck(IG_APP_ID.Value) And ACheck(IG_WWW_CLAIM.Value) And ACheck(CSRF_TOKEN.Value)
        End Function
        Private Const Header_IG_APP_ID As String = "x-ig-app-id"
        Private Const Header_IG_WWW_CLAIM As String = "x-ig-www-claim"
        Private Const Header_CSRF_TOKEN As String = "x-csrftoken"
        Private Sub ChangeResponserFields(ByVal PropName As String, ByVal Value As Object)
            If Not PropName.IsEmptyString Then
                Dim f$ = String.Empty
                Select Case PropName
                    Case NameOf(IG_APP_ID) : f = Header_IG_APP_ID
                    Case NameOf(IG_WWW_CLAIM) : f = Header_IG_WWW_CLAIM
                    Case NameOf(CSRF_TOKEN) : f = Header_CSRF_TOKEN
                End Select
                If Not f.IsEmptyString Then
                    Responser.HeadersRemove(f)
                    If Not CStr(Value).IsEmptyString Then Responser.HeadersAdd(f, CStr(Value))
                    Responser.SaveSettings()
                End If
            End If
        End Sub
#End Region
#Region "Download properties"
        <PropertyOption(ControlText:="Request timer", AllowNull:=False), PXML("RequestsWaitTimer"), ControlNumber(6)>
        Friend ReadOnly Property RequestsWaitTimer As PropertyValue
        <Provider(NameOf(RequestsWaitTimer), FieldsChecker:=True)>
        Private ReadOnly Property RequestsWaitTimerProvider As IFormatProvider
        <PropertyOption(ControlText:="Request timer counter", AllowNull:=False, LeftOffset:=120), PXML("RequestsWaitTimerTaskCount"), ControlNumber(7)>
        Friend ReadOnly Property RequestsWaitTimerTaskCount As PropertyValue
        <Provider(NameOf(RequestsWaitTimerTaskCount), FieldsChecker:=True)>
        Private ReadOnly Property RequestsWaitTimerTaskCountProvider As IFormatProvider
        <PropertyOption(ControlText:="Posts limit timer", AllowNull:=False), PXML("SleepTimerOnPostsLimit"), ControlNumber(8)>
        Friend ReadOnly Property SleepTimerOnPostsLimit As PropertyValue
        <Provider(NameOf(SleepTimerOnPostsLimit), FieldsChecker:=True)>
        Private ReadOnly Property SleepTimerOnPostsLimitProvider As IFormatProvider
        <PropertyOption(ControlText:="Get stories"), PXML, ControlNumber(9)>
        Friend ReadOnly Property GetStories As PropertyValue
        <PropertyOption(ControlText:="Get tagged photos"), PXML, ControlNumber(10)>
        Friend ReadOnly Property GetTagged As PropertyValue
        <PropertyOption(ControlText:="Tagged notify limit",
                        ControlToolTip:="If the number of tagged posts exceeds this number you will be notified." & vbCr &
                        "-1 to disable"), PXML, ControlNumber(11)>
        Friend ReadOnly Property TaggedNotifyLimit As PropertyValue
        <Provider(NameOf(TaggedNotifyLimit), FieldsChecker:=True)>
        Private ReadOnly Property TaggedNotifyLimitProvider As IFormatProvider
#End Region
#Region "Download ready"
        Friend ReadOnly Property DownloadTimeline As XMLValue(Of Boolean)
        Friend ReadOnly Property DownloadStoriesTagged As XMLValue(Of Boolean)
        Friend ReadOnly Property DownloadSaved As XMLValue(Of Boolean)
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

            With Responser
                If .Headers.Count > 0 Then
                    token = .HeadersValue(Header_CSRF_TOKEN)
                    app_id = .HeadersValue(Header_IG_APP_ID)
                    www_claim = .HeadersValue(Header_IG_WWW_CLAIM)
                End If
                If Not .Cookies Is Nothing Then
                    .Cookies.ChangedAllowInternalDrop = False
                    .Cookies.Changed = False
                End If
            End With

            Dim n() As String = {SettingsCLS.Name_Node_Sites, Site.ToString}

            SavedPostsUserName = New PropertyValue(String.Empty, GetType(String))

            Hash = New PropertyValue(String.Empty, GetType(String))
            HashSavedPosts = New PropertyValue(String.Empty, GetType(String))
            CSRF_TOKEN = New PropertyValue(token, GetType(String), Sub(v) ChangeResponserFields(NameOf(CSRF_TOKEN), v))
            IG_APP_ID = New PropertyValue(app_id, GetType(String), Sub(v) ChangeResponserFields(NameOf(IG_APP_ID), v))
            IG_WWW_CLAIM = New PropertyValue(www_claim, GetType(String), Sub(v) ChangeResponserFields(NameOf(IG_WWW_CLAIM), v))

            DownloadTimeline = New XMLValue(Of Boolean)("DownloadTimeline", True, _XML, n)
            DownloadStoriesTagged = New XMLValue(Of Boolean)("DownloadStoriesTagged", True, _XML, n)
            DownloadSaved = New XMLValue(Of Boolean)("DownloadSaved", True, _XML, n)

            RequestsWaitTimer = New PropertyValue(1000)
            RequestsWaitTimerProvider = New TimersChecker(100)
            RequestsWaitTimerTaskCount = New PropertyValue(1)
            RequestsWaitTimerTaskCountProvider = New TimersChecker(1)
            SleepTimerOnPostsLimit = New PropertyValue(60000)
            SleepTimerOnPostsLimitProvider = New TimersChecker(10000)

            GetStories = New PropertyValue(False)
            GetTagged = New PropertyValue(False)
            TaggedNotifyLimit = New PropertyValue(200)
            TaggedNotifyLimitProvider = New TaggedNotifyLimitChecker

            DownloadingErrorDate = New XMLValue(Of Date) With {.Provider = New XMLValueConversionProvider(Function(ss, vv) AConvert(Of String)(vv, AModes.Var, Nothing))}
            DownloadingErrorDate.SetExtended("InstagramDownloadingErrorDate", Now.AddYears(-10), _XML, n)
            LastDownloadDate = New XMLValue(Of Date)("LastDownloadDate", Now.AddDays(-1), _XML, n)
            LastRequestsCount = New XMLValue(Of Integer)("LastRequestsCount", 0, _XML, n)
            LastRequestsCountLabel = New PropertyValue(LastRequestsCountLabelStr.Invoke(LastRequestsCount.Value))
            AddHandler LastRequestsCount.OnValueChanged, Sub(sender, __name, __value) LastRequestsCountLabel.Value = LastRequestsCountLabelStr.Invoke(DirectCast(__value, Existable(Of Integer)).Value)

            UrlPatternUser = "https://www.instagram.com/{0}/"
            UserRegex = RParams.DMS("[htps:/]{7,8}.*?instagram.com/([^/]+)", 1)
            ImageVideoContains = "instagram.com"
        End Sub
        Private Structure LatestValues
            Friend Hash As String
            Friend Hash2 As String
            Friend Token As String
            Friend AppID As String
            Friend WwwClaim As String
            Friend Exists As Boolean
            Friend Sub New(ByVal Source As SiteSettings)
                Exists = True
                With Source
                    Hash = AConvert(Of String)(.Hash.Value, String.Empty)
                    Hash2 = AConvert(Of String)(.HashSavedPosts.Value, String.Empty)
                    With .Responser
                        Token = .HeadersValue(Header_CSRF_TOKEN)
                        AppID = .HeadersValue(Header_IG_APP_ID)
                        WwwClaim = .HeadersValue(Header_IG_WWW_CLAIM)
                    End With
                End With
            End Sub
        End Structure
        Private LV As LatestValues = Nothing
        Private ASO As SettingsExchangeOptions = Nothing
        Friend Overrides Sub BeginEdit()
            LV = New LatestValues(Me)
            ASO = Nothing
            MyBase.BeginEdit()
        End Sub
        Friend Overrides Sub EndEdit()
            LV = Nothing
            ASO = Nothing
            MyBase.EndEdit()
        End Sub
        Friend Overrides Sub Update()
            If LV.Exists Then
                Dim __lv As New LatestValues(Me)
                If If(Responser.Cookies?.Count, 0) > 0 Then
                    Dim _cookiesChanged As Boolean = If(Responser.Cookies?.Changed, False)
                    If Not DownloadTimeline AndAlso (_cookiesChanged Or
                       (Not LV.Hash = __lv.Hash And Not __lv.Hash.IsEmptyString)) Then DownloadTimeline.Value = True
                    If Not DownloadSaved AndAlso (_cookiesChanged Or (Not LV.Hash2 = __lv.Hash2 And Not __lv.Hash2.IsEmptyString)) Then DownloadSaved.Value = True
                    If Not DownloadStoriesTagged AndAlso (
                           _cookiesChanged Or (
                                (Not LV.Hash = __lv.Hash Or Not LV.Token = __lv.Token Or Not LV.AppID = __lv.AppID Or Not LV.WwwClaim = __lv.WwwClaim) And
                                (Not __lv.Hash.IsEmptyString And Not __lv.Token.IsEmptyString And Not __lv.AppID.IsEmptyString And Not __lv.WwwClaim.IsEmptyString)
                            )) Then DownloadStoriesTagged.Value = True
                End If
            End If
            If ASO.Changed Then
                DownloadTimeline.Value = ASO.DownloadTimeline
                DownloadStoriesTagged.Value = ASO.DownloadStoriesTagged
                DownloadSaved.Value = ASO.DownloadSaved
            End If
            LV = Nothing
            ASO = Nothing
            If Not Responser.Cookies Is Nothing Then Responser.Cookies.Changed = False
            MyBase.Update()
        End Sub
#End Region
#Region "PropertiesDataChecker"
        <PropertiesDataChecker({NameOf(Hash), NameOf(HashSavedPosts)})>
        Private Function CheckHashControls(ByVal p As IEnumerable(Of PropertyData)) As Boolean
            If p.ListExists(2) Then
                Dim h$ = String.Empty
                Dim hsp$ = String.Empty
                For Each pp As PropertyData In p
                    Select Case pp.Name
                        Case NameOf(Hash) : h = AConvert(Of String)(pp.Value, String.Empty)
                        Case NameOf(HashSavedPosts) : hsp = AConvert(Of String)(pp.Value, String.Empty)
                    End Select
                Next
                If h.IsEmptyString And hsp.IsEmptyString Then
                    Return True
                Else
                    If h = hsp Then
                        MsgBoxE({"InstaHash for saved posts must be different from InstaHash!", "InstaHash are equal"}, vbCritical)
                        Return False
                    Else
                        Return True
                    End If
                End If
            Else
                Return False
            End If
        End Function
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
        <PropertiesDataChecker({NameOf(HashSavedPosts), NameOf(SavedPostsUserName)})>
        Private Function CheckSavedOptions(ByVal p As IEnumerable(Of PropertyData)) As Boolean
            If p.ListExists Then
                Const MsgTitle$ = "Saved posts credentials"
                Dim __hash$ = String.Empty
                Dim __name$ = String.Empty
                Dim _OptionlErrorText$ = $"For download saved posts, you must to set both [{HashSavedPosts_Text}] and [{SavedPostsUserName_Text}]."
                For i% = 0 To p.Count - 1
                    Select Case p(i).Name
                        Case NameOf(HashSavedPosts) : __hash = p(i).Value
                        Case NameOf(SavedPostsUserName) : __name = p(i).Value
                    End Select
                Next
                If __hash = __name Then
                    If __hash.IsEmptyString Then
                        Return True
                    Else
                        MsgBoxE({$"[{HashSavedPosts_Text}] and [{SavedPostsUserName_Text}] for saved posts cannot be equal!", MsgTitle}, vbCritical)
                    End If
                Else
                    If __hash.IsEmptyString Then
                        MsgBoxE({$"[{HashSavedPosts_Text}] not set.{vbCr}{_OptionlErrorText}", MsgTitle}, vbCritical)
                    ElseIf __name.IsEmptyString Then
                        MsgBoxE({$"[{SavedPostsUserName_Text}] not set.{vbCr}{_OptionlErrorText}", MsgTitle}, vbCritical)
                    Else
                        Return True
                    End If
                End If
            End If
            Return False
        End Function
#End Region
#Region "Plugin functions"
        Friend Overrides Function GetInstance(ByVal What As Download) As IPluginContentProvider
            Select Case What
                Case Download.Main : Return New UserData
                Case Download.SavedPosts
                    Dim u As New UserData
                    DirectCast(u, UserDataBase).User = New UserInfo With {.Name = CStr(AConvert(Of String)(SavedPostsUserName.Value, String.Empty))}
                    Return u
            End Select
            Return Nothing
        End Function
#Region "Downloading"
        Friend Property SkipUntilNextSession As Boolean = False
        Friend Overrides Function ReadyToDownload(ByVal What As Download) As Boolean
            If ActiveJobs < 2 AndAlso Not SkipUntilNextSession AndAlso ReadyForDownload AndAlso BaseAuthExists() Then
                Select Case What
                    Case Download.Main : Return ACheck(Hash.Value) And DownloadTimeline
                    Case Download.SavedPosts : Return ACheck(HashSavedPosts.Value) And DownloadSaved
                End Select
            End If
            Return False
        End Function
        Private ActiveJobs As Integer = 0
        Private _NextWNM As UserData.WNM = UserData.WNM.Notify
        Private _NextTagged As Boolean = True
        Friend Overrides Sub DownloadStarted(ByVal What As Download)
            ActiveJobs += 1
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
        Friend Overrides Function GetSpecialData(ByVal URL As String, ByVal Path As String, ByVal AskForPath As Boolean) As IEnumerable
            Return UserData.GetVideoInfo(URL, Responser)
        End Function
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse Not TypeOf Options Is EditorExchangeOptions Then Options = New EditorExchangeOptions(Me)
            If OpenForm Then
                Using f As New OptionsForm(Options) : f.ShowDialog() : End Using
            End If
        End Sub
        Friend Overrides Sub OpenSettingsForm()
            Using f As New AdditionalSettingsForm(If(ASO.Changed, ASO, New SettingsExchangeOptions(Me)))
                f.ShowDialog()
                If f.DialogResult = DialogResult.OK Then ASO = f.MyParameters
            End Using
        End Sub
#End Region
    End Class
End Namespace