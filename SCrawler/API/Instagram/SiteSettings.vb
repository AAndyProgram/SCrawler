' Copyright (C) 2022  Andy
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
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Functions.RegularExpressions
Imports Download = SCrawler.Plugin.ISiteSettings.Download
Namespace API.Instagram
    <Manifest("AndyProgram_Instagram"), UseClassAsIs, SeparatedTasks(1), SavedPosts, SpecialForm(False)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Interface Declarations"
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return My.Resources.InstagramIcon
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.InstagramPic76
            End Get
        End Property
#End Region
#Region "Providers"
        Private Class TimersChecker : Implements ICustomProvider
            Private ReadOnly _LowestValue As Integer
            Friend Sub New(ByVal LowestValue As Integer)
                _LowestValue = LowestValue
            End Sub
            Private Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                     Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object Implements ICustomProvider.Convert
                If ACheck(Of Integer)(Value) AndAlso CInt(Value) >= _LowestValue Then
                    Return Value
                Else
                    Return Nothing
                End If
            End Function
            Private Function GetFormat(ByVal FormatType As Type) As Object Implements IFormatProvider.GetFormat
                Throw New NotImplementedException()
            End Function
        End Class
#End Region
#Region "Authorization properties"
        <PropertyOption(ControlText:="Hash", ControlToolTip:="Instagram session hash", IsAuth:=True), PXML("InstaHash"), ControlNumber(0)>
        Friend ReadOnly Property Hash As PropertyValue
        <PropertyOption(ControlText:="Hash 2", ControlToolTip:="Instagram session hash for saved posts", IsAuth:=True), PXML("InstaHashSavedPosts"), ControlNumber(1)>
        Friend ReadOnly Property HashSavedPosts As PropertyValue
        <PropertyOption(ControlText:="x-ig-app-id", IsAuth:=True), ControlNumber(2)>
        Friend Property IG_APP_ID As PropertyValue
        <PropertyOption(ControlText:="x-ig-www-claim", IsAuth:=True), ControlNumber(3)>
        Friend Property IG_WWW_CLAIM As PropertyValue
        <PropertyOption(ControlText:="Saved posts user", IsAuth:=True), PXML("SavedPostsUserName"), ControlNumber(4)>
        Friend ReadOnly Property SavedPostsUserName As PropertyValue
        Friend ReadOnly Property StoriesAndTaggedReady As Boolean
            Get
                Return ACheck(IG_APP_ID.Value) And ACheck(IG_WWW_CLAIM.Value)
            End Get
        End Property
#End Region
#Region "Download properties"
        Friend ReadOnly Property HashUpdateRequired As XMLValue(Of Boolean)
        <PropertyOption(ControlText:="Request timer", AllowNull:=False), PXML("RequestsWaitTimer"), ControlNumber(5)>
        Friend ReadOnly Property RequestsWaitTimer As PropertyValue
        <Provider(NameOf(RequestsWaitTimer), FieldsChecker:=True)>
        Private ReadOnly Property RequestsWaitTimerProvider As IFormatProvider
        <PropertyOption(ControlText:="Request timer counter", AllowNull:=False, LeftOffset:=120), PXML("RequestsWaitTimerTaskCount"), ControlNumber(6)>
        Friend ReadOnly Property RequestsWaitTimerTaskCount As PropertyValue
        <Provider(NameOf(RequestsWaitTimerTaskCount), FieldsChecker:=True)>
        Private ReadOnly Property RequestsWaitTimerTaskCountProvider As IFormatProvider
        <PropertyOption(ControlText:="Posts limit timer", AllowNull:=False), PXML("SleepTimerOnPostsLimit"), ControlNumber(7)>
        Friend ReadOnly Property SleepTimerOnPostsLimit As PropertyValue
        <Provider(NameOf(SleepTimerOnPostsLimit), FieldsChecker:=True)>
        Private ReadOnly Property SleepTimerOnPostsLimitProvider As IFormatProvider
        <PropertyOption(ControlText:="Get stories"), PXML, ControlNumber(8)>
        Friend ReadOnly Property GetStories As PropertyValue
        <PropertyOption(ControlText:="Get tagged photos"), PXML, ControlNumber(9)>
        Friend ReadOnly Property GetTagged As PropertyValue
#End Region
#Region "429 bypass"
        Friend ReadOnly Property DownloadingErrorDate As XMLValue(Of Date)
        Friend Property LastApplyingValue As Integer? = Nothing
        Friend ReadOnly Property ReadyForDownload As Boolean
            Get
                With DownloadingErrorDate
                    If .ValueF.Exists Then
                        Return .ValueF.Value.AddMinutes(If(LastApplyingValue, 10)) < Now
                    Else
                        Return True
                    End If
                End With
            End Get
        End Property
        Friend ReadOnly Property LastDownloadDate As XMLValue(Of Date)
        Friend ReadOnly Property LastRequestsCount As XMLValue(Of Integer)
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
        Friend Overrides ReadOnly Property Responser As WEB.Response
        Friend Sub New(ByRef _XML As XmlFile, ByVal GlobalPath As SFile)
            MyBase.New(InstagramSite)
            Responser = New WEB.Response($"{SettingsFolderName}\Responser_{Site}.xml")

            Dim app_id$ = String.Empty
            Dim www_claim$ = String.Empty

            With Responser
                If .File.Exists Then
                    .LoadSettings()
                    With .Headers
                        If .ContainsKey(Header_IG_APP_ID) Then app_id = .Item(Header_IG_APP_ID)
                        If .ContainsKey(Header_IG_WWW_CLAIM) Then www_claim = .Item(Header_IG_WWW_CLAIM)
                    End With
                Else
                    .CookiesDomain = "instagram.com"
                    .SaveSettings()
                End If
            End With

            Dim n() As String = {SettingsCLS.Name_Node_Sites, Site.ToString}

            SavedPostsUserName = New PropertyValue(String.Empty, GetType(String))

            HashUpdateRequired = New XMLValue(Of Boolean)("InstaHashUpdateRequired", True, _XML, n)
            Hash = New PropertyValue(String.Empty, GetType(String))
            HashSavedPosts = New PropertyValue(String.Empty, GetType(String))
            IG_APP_ID = New PropertyValue(app_id, GetType(String), Sub(v) ChangeResponserFields(NameOf(IG_APP_ID), v))
            IG_WWW_CLAIM = New PropertyValue(www_claim, GetType(String), Sub(v) ChangeResponserFields(NameOf(IG_WWW_CLAIM), v))

            RequestsWaitTimer = New PropertyValue(1000)
            RequestsWaitTimerProvider = New TimersChecker(100)
            RequestsWaitTimerTaskCount = New PropertyValue(1)
            RequestsWaitTimerTaskCountProvider = New TimersChecker(1)
            SleepTimerOnPostsLimit = New PropertyValue(60000)
            SleepTimerOnPostsLimitProvider = New TimersChecker(10000)

            GetStories = New PropertyValue(False)
            GetTagged = New PropertyValue(False)

            DownloadingErrorDate = New XMLValue(Of Date) With {
                .Provider = New XMLValueConversionProvider(Function(ss, vv) AConvert(Of String)(vv, AModes.Var, Nothing))}
            DownloadingErrorDate.SetExtended("InstagramDownloadingErrorDate", Now.AddYears(-10), _XML, n)
            LastDownloadDate = New XMLValue(Of Date)("LastDownloadDate", Now.AddDays(-1), _XML, n)
            LastRequestsCount = New XMLValue(Of Integer)("LastRequestsCount", 0, _XML, n)

            UrlPatternUser = "https://www.instagram.com/{0}/"
            UserRegex = RParams.DMS("[htps:/]{7,8}.*?instagram.com/([^/]+)", 1)
            ImageVideoContains = "instagram.com"
        End Sub
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
        Private Const Header_IG_APP_ID As String = "x-ig-app-id"
        Private Const Header_IG_WWW_CLAIM As String = "x-ig-www-claim"
        Private Sub ChangeResponserFields(ByVal PropName As String, ByVal Value As Object)
            If Not PropName.IsEmptyString Then
                Dim f$ = String.Empty
                Select Case PropName
                    Case NameOf(IG_APP_ID) : f = Header_IG_APP_ID
                    Case NameOf(IG_WWW_CLAIM) : f = Header_IG_WWW_CLAIM
                End Select
                If Not f.IsEmptyString Then
                    If Responser.Headers.Count > 0 AndAlso Responser.Headers.ContainsKey(f) Then Responser.Headers.Remove(f)
                    If Not CStr(Value).IsEmptyString Then Responser.Headers.Add(f, CStr(Value))
                    Responser.SaveSettings()
                End If
            End If
        End Sub
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
        Friend Overrides Sub BeginInit()
        End Sub
        Friend Overrides Sub EndInit()
            If (CStr(Hash.Value).IsEmptyString Or HashUpdateRequired) AndAlso Responser.Cookies.ListExists Then GatherInstaHash()
        End Sub
        Friend Overrides Function ReadyToDownload(ByVal What As Download) As Boolean
            Return ActiveJobs < 2 AndAlso ReadyForDownload
        End Function
#Region "Downloading"
        Private ActiveJobs As Integer = 0
        Private _NextWNM As UserData.WNM = UserData.WNM.Notify
        Friend Overrides Sub DownloadStarted(ByVal What As Download)
            If CStr(Hash.Value).IsEmptyString Or HashUpdateRequired Then GatherInstaHash()
            ActiveJobs += 1
        End Sub
        Friend Overrides Sub BeforeStartDownload(ByVal User As Object, ByVal What As Download)
            With DirectCast(User, UserData)
                If What = Download.Main Then .WaitNotificationMode = _NextWNM
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
                LastDownloadDate.Value = Now
                LastRequestsCount.Value = .RequestsCount
            End With
        End Sub
        Friend Overrides Sub DownloadDone(ByVal What As Download)
            _NextWNM = UserData.WNM.Notify
            LastDownloadDate.Value = Now
            ActiveJobs -= 1
            If HashUpdateRequired Then MyMainLOG = "Check your Instagram credentials"
        End Sub
#End Region
        <PropertyUpdater(NameOf(Hash))>
        Friend Function GatherInstaHash() As Boolean
            Try
                If Not Responser.Cookies.ListExists Then Throw New Exception("Instagram cookies does not set")
                Dim rs As New RParams("=""([^""]+?ConsumerLibCommons[^""]+?.js)""", Nothing, 1) With {.MatchTimeOut = 10}
                Dim r$ = Responser.GetResponse("https://instagram.com",, EDP.ThrowException)
                If Not r.IsEmptyString Then
                    Dim hStr$ = RegexReplace(r, rs)
                    If Not hStr.IsEmptyString Then
                        Do While Left(hStr, 1) = "/" : hStr = Right(hStr, hStr.Length - 1) : Loop
                        hStr = $"https://instagram.com/{hStr}"
                        r = Responser.GetResponse(hStr,, EDP.ThrowException)
                        If Not r.IsEmptyString Then
                            rs = New RParams("generatePaginationActionCreators.+?.profilePosts.byUserId.get.+?queryId:.([\d\w\S]+?)""", Nothing, 1) With {.MatchTimeOut = 10}
                            Dim h$ = RegexReplace(r, rs)
                            If Not h.IsEmptyString Then
                                Hash.Value = h
                                HashUpdateRequired.Value = False
                                Return True
                            End If
                        End If
                    End If
                End If
                Return False
            Catch ex As Exception
                HashUpdateRequired.Value = True
                Hash.Value = String.Empty
                Return ErrorsDescriber.Execute(EDP.SendInLog + EDP.ReturnValue, ex, "[SiteSettings.GaterInstaHash]", False)
            End Try
        End Function
        Friend Overrides Function GetSpecialDataF(ByVal URL As String) As IEnumerable(Of UserMedia)
            Return UserData.GetVideoInfo(URL, Responser)
        End Function
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse Not TypeOf Options Is EditorExchangeOptions Then Options = New EditorExchangeOptions(Me)
            If OpenForm Then
                Using f As New OptionsForm(Options) : f.ShowDialog() : End Using
            End If
        End Sub
    End Class
End Namespace