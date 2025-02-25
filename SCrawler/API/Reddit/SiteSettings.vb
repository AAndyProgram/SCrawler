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
Imports PersonalUtilities.Tools.Web.Clients.Base
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports DownDetector = SCrawler.API.Base.DownDetector
Imports Download = SCrawler.Plugin.ISiteSettings.Download
Namespace API.Reddit
    <Manifest(RedditSiteKey), SavedPosts, SpecialForm(False), UseDownDetector>
    Friend Class SiteSettings : Inherits SiteSettingsBase : Implements DownDetector.IDownDetector
#Region "Declarations"
#Region "Authorization"
        <PropertyOption(ControlText:="Login", ControlToolTip:="Your authorization username", IsAuth:=True), PXML, PClonable(Clone:=False)>
        Friend ReadOnly Property AuthUserName As PropertyValue
        <PropertyOption(ControlText:="Password", ControlToolTip:="Your authorization password", IsAuth:=True), PXML, PClonable(Clone:=False)>
        Friend ReadOnly Property AuthPassword As PropertyValue
        <PropertyOption(ControlText:="Client ID", ControlToolTip:="Your registered app client ID", IsAuth:=True), PXML, PClonable(Clone:=False)>
        Friend ReadOnly Property ApiClientID As PropertyValue
        <PropertyOption(ControlText:="Client Secret", ControlToolTip:="Your registered app client secret", IsAuth:=True), PXML, PClonable(Clone:=False)>
        Friend ReadOnly Property ApiClientSecret As PropertyValue
        <PropertyOption(ControlText:="Bearer token",
                        ControlToolTip:="Bearer token (can be null)." & vbCr &
                                        "If you are using cookies to download the timeline, it is highly recommended that you add a token." & vbCr &
                                        "You can find different tokens in the responses. Make sure that bearer token belongs to Reddit and not RedGifs." & vbCr &
                                        "There is not need to add a token if you are not using cookies to download the timeline.", IsAuth:=True)>
        Friend ReadOnly Property BearerToken As PropertyValue
        <PropertyOption(ControlText:="Use 'cUrl' to get a token", IsAuth:=True), PXML, PClonable, HiddenControl>
        Private ReadOnly Property BearerTokenUseCurl As PropertyValue
#Region "TokenUpdateInterval"
        <PropertyOption(ControlText:="Token refresh interval", ControlToolTip:="Interval (in minutes) to refresh the token",
                        AllowNull:=False, LeftOffset:=120, IsAuth:=True), PXML, PClonable>
        Friend ReadOnly Property TokenUpdateInterval As PropertyValue
        <Provider(NameOf(TokenUpdateInterval), FieldsChecker:=True)>
        Private ReadOnly Property TokenUpdateIntervalProvider As IFormatProvider
#End Region
        <PXML, PClonable> Private ReadOnly Property BearerTokenDateUpdate As PropertyValue
        <PropertyOption(ControlText:="Use the token to download the timeline", IsAuth:=True), PXML, PClonable>
        Friend ReadOnly Property UseTokenForTimelines As PropertyValue
        <PropertyOption(ControlText:="Use the token to download saved posts", IsAuth:=True), PXML, PClonable>
        Friend ReadOnly Property UseTokenForSavedPosts As PropertyValue
        <PropertyOption(ControlText:="Use cookies to download the timeline", IsAuth:=True), PXML, PClonable>
        Friend ReadOnly Property UseCookiesForTimelines As PropertyValue
        <PropertyOption(ControlText:=DeclaredNames.SavedPostsUserNameCaption, ControlToolTip:=DeclaredNames.SavedPostsUserNameToolTip, IsAuth:=True), PXML, PClonable(Clone:=False)>
        Friend ReadOnly Property SavedPostsUserName As PropertyValue
        Friend ReadOnly Property CredentialsExists As Boolean
            Get
                Return {AuthUserName.Value, AuthPassword.Value, ApiClientID.Value, ApiClientSecret.Value}.All(Function(v$) Not v.IsEmptyString)
            End Get
        End Property
#End Region
#Region "Other"
        <PropertyOption(ControlText:="Use M3U8", ControlToolTip:="Use M3U8 or mp4 for Reddit videos", IsAuth:=False), PXML, PClonable>
        Friend ReadOnly Property UseM3U8 As PropertyValue
        <PropertyOption(ControlText:="Check image", ControlToolTip:="Check the image if it exists before downloading (it makes downloading very slow)", IsAuth:=False), PXML, PClonable>
        Friend ReadOnly Property CheckImage As PropertyValue
        <PropertyOption(ControlText:="Check image: get original", ControlToolTip:="Get the original image if it exists", IsAuth:=False), PXML, PClonable>
        Friend ReadOnly Property CheckImageReturnOrig As PropertyValue
#End Region
#Region "IDownDetector Support"
        Private ReadOnly Property IDownDetector_Value As Integer Implements DownDetector.IDownDetector.Value
            Get
                Return 100
            End Get
        End Property
        Private ReadOnly Property IDownDetector_AddToLog As Boolean Implements DownDetector.IDownDetector.AddToLog
            Get
                Return False
            End Get
        End Property
        Private ReadOnly Property IDownDetector_CheckSite As String Implements DownDetector.IDownDetector.CheckSite
            Get
                Return "reddit"
            End Get
        End Property
        Private Function IDownDetector_Available(ByVal What As Download, ByVal Silent As Boolean) As Boolean Implements DownDetector.IDownDetector.Available
            Return MDD.Available(What, Silent)
        End Function
#End Region
#End Region
#Region "Initializer"
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New(RedditSite, "reddit.com", AccName, Temp, My.Resources.SiteResources.RedditIcon_128, My.Resources.SiteResources.RedditPic_512)

            Dim token$
            With Responser
                Dim d% = .Decoders.Count
                .Decoders.ListAddList({SymbolsConverter.Converters.Unicode, SymbolsConverter.Converters.HTML}, LAP.NotContainsOnly)
                token = .Headers.Value(DeclaredNames.Header_Authorization)
            End With

            AuthUserName = New PropertyValue(String.Empty, GetType(String))
            AuthPassword = New PropertyValue(String.Empty, GetType(String))
            ApiClientID = New PropertyValue(String.Empty, GetType(String))
            ApiClientSecret = New PropertyValue(String.Empty, GetType(String))
            BearerToken = New PropertyValue(token, GetType(String), Sub(v) Responser.Headers.Add(DeclaredNames.Header_Authorization, v))
            BearerTokenUseCurl = New PropertyValue(True)
            TokenUpdateInterval = New PropertyValue(360)
            TokenUpdateIntervalProvider = New TokenRefreshIntervalProvider
            BearerTokenDateUpdate = New PropertyValue(Now.AddYears(-1))
            UseTokenForTimelines = New PropertyValue(False)
            UseTokenForSavedPosts = New PropertyValue(False)
            UseCookiesForTimelines = New PropertyValue(False)
            SavedPostsUserName = New PropertyValue(String.Empty, GetType(String))

            UseM3U8 = New PropertyValue(True)
            CheckImage = New PropertyValue(False)
            CheckImageReturnOrig = New PropertyValue(True)

            MDD = New MyDownDetector(Me)

            UrlPatternUser = "https://www.reddit.com/{0}/{1}/"
            ImageVideoContains = "reddit.com"
            UserRegex = RParams.DM("[htps:/]{7,8}.*?reddit.com/([user]{1,4})/([^/\?&]+)", 0, RegexReturn.ListByMatch, EDP.ReturnValue)
        End Sub
        Private Const SettingsVersionCurrent As Integer = 1
        Friend Overrides Sub EndInit()
            If CInt(SettingsVersion.Value) < SettingsVersionCurrent Then
                SettingsVersion.Value = SettingsVersionCurrent
                TokenUpdateInterval.Value = 360
            End If
            MyBase.EndInit()
        End Sub
#End Region
#Region "GetInstance"
        Friend Overrides Function GetInstance(ByVal What As Download) As IPluginContentProvider
            Return New UserData
        End Function
#End Region
#Region "DownloadStarted, ReadyToDownload, Available, DownloadDone, UpdateRedGifsToken"
        Private ReadOnly MDD As MyDownDetector
        Private Class MyDownDetector : Inherits DownDetector.Checker(Of SiteSettings)
            Private __TrueValue As Boolean = False
            Friend Sub New(ByRef _Source As SiteSettings)
                MyBase.New(_Source)
            End Sub
            Protected Overrides Function AvailableImpl(ByVal What As Download, ByVal Silent As Boolean) As Boolean
                __TrueValue = Source.AvailableTrueValue(What)
                Return MyBase.AvailableImpl(What, Silent)
            End Function
            Protected Overrides Function AvailableImpl_TRUE() As Boolean
                Return AvailableImpl_TrueValueReturn()
            End Function
            Protected Overrides Function AvailableImpl_FALSE_SILENT_NOT_MSG_YES() As Boolean
                Return AvailableImpl_TrueValueReturn()
            End Function
            Private Function AvailableImpl_TrueValueReturn() As Boolean
                If __TrueValue Then Source.UpdateRedGifsToken()
                Return __TrueValue AndAlso Source.UpdateTokenIfRequired()
            End Function
            Friend Overrides Sub Reset()
                __TrueValue = False
                MyBase.Reset()
            End Sub
        End Class
        Friend Property SessionInterrupted As Boolean = False
        Friend Overrides Function ReadyToDownload(ByVal What As Download) As Boolean
            If What = Download.Main Then
                Return Not SessionInterrupted
            Else
                Return True
            End If
        End Function
        Friend Overrides Function Available(ByVal What As Download, ByVal Silent As Boolean) As Boolean
            Return AvailableTrueValue(What) AndAlso UpdateTokenIfRequired()
        End Function
        Private Function AvailableTrueValue(ByVal What As Download) As Boolean
            Return Not What = Download.SavedPosts OrElse (Responser.CookiesExists And ACheck(SavedPostsUserName.Value))
        End Function
        Friend Overrides Sub DownloadDone(ByVal What As Download)
            SessionInterrupted = False
            MDD.Reset()
            MyBase.DownloadDone(What)
        End Sub
        Private Sub UpdateRedGifsToken()
            Settings(RedGifs.RedGifsSiteKey).ListForEach(Sub(h, i) DirectCast(h.Source, RedGifs.SiteSettings).UpdateTokenIfRequired())
        End Sub
#End Region
#Region "IsMyUser, GetUserUrl, GetUserPostUrl"
        Friend Const ChannelOption As String = "r"
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            Dim l As List(Of String) = RegexReplace(UserURL, UserRegex)
            If l.ListExists(3) Then
                Dim n$ = l(2)
                If Not l(1).IsEmptyString AndAlso l(1) = ChannelOption Then n &= $"@{ChannelOption}"
                Return New ExchangeOptions(Site, n)
            Else
                Return Nothing
            End If
        End Function
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider) As String
            With DirectCast(User, UserData) : Return String.Format(UrlPatternUser, IIf(.IsChannel, ChannelOption, "user"), .NameTrue) : End With
        End Function
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            If Not Media.Post.ID.IsEmptyString Then
                Return $"https://www.reddit.com/comments/{Media.Post.ID.Split("_").LastOrDefault}/"
            Else
                Return String.Empty
            End If
        End Function
#End Region
#Region "UserOptions"
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse Not TypeOf Options Is RedditViewExchange Then Options = New RedditViewExchange
            If OpenForm Then
                Using f As New RedditViewSettingsForm(Options, True) : f.ShowDialog() : End Using
            End If
        End Sub
#End Region
#Region "BeginEdit, Update"
        Private _OldTokenValue As String = String.Empty
        Friend Overrides Sub BeginEdit()
            _OldTokenValue = BearerToken.Value
            MyBase.BeginEdit()
        End Sub
        Friend Overrides Sub Update()
            If _SiteEditorFormOpened Then
                Dim newTokenValue$ = BearerToken.Value
                If Not newTokenValue.IsEmptyString AndAlso Not newTokenValue = _OldTokenValue Then BearerTokenDateUpdate.Value = Now
            End If
            MyBase.Update()
        End Sub
#End Region
#Region "Token"
        <PropertiesDataChecker({NameOf(AuthUserName), NameOf(AuthPassword), NameOf(ApiClientID), NameOf(ApiClientSecret)})>
        Private Function TokenPropertiesChecker(ByVal p As IEnumerable(Of PropertyData)) As Boolean
            If p.ListExists Then
                Dim wrong As New List(Of String)
                For i% = 0 To p.Count - 1
                    If CStr(p(i).Value).IsEmptyString Then wrong.Add(p(i).Name)
                Next
                If wrong.Count > 0 And wrong.Count <> 4 Then
                    MsgBoxE({$"You have not completed the following fields: {wrong.ListToString}." & vbCr &
                            "To use OAuth authorization, all authorization fields must be filled in.", "Validate token fields"}, vbCritical)
                    Return False
                Else
                    Return True
                End If
            End If
            Return False
        End Function
        Private Function UpdateTokenIfRequired() As Boolean
            UpdateRedGifsToken()
            If (CBool(UseTokenForTimelines.Value) Or CBool(UseTokenForSavedPosts.Value)) AndAlso CredentialsExists Then
                If CDate(BearerTokenDateUpdate.Value).AddMinutes(TokenUpdateInterval.Value) <= Now Then Return UpdateToken()
            End If
            Return True
        End Function
        Private Overloads Function UpdateToken() As Boolean
            Return UpdateToken(AuthUserName.Value, AuthPassword.Value, ApiClientID.Value, ApiClientSecret.Value, EDP.SendToLog + EDP.ReturnValue)
        End Function
        <PropertyUpdater(NameOf(BearerToken), {NameOf(AuthUserName), NameOf(AuthPassword), NameOf(ApiClientID), NameOf(ApiClientSecret)})>
        Private Overloads Function UpdateToken(ByVal UserName As String, ByVal Password As String, ByVal ClientID As String, ByVal ClientSecret As String) As Boolean
            Return UpdateToken(UserName, Password, ClientID, ClientSecret, EDP.LogMessageValue)
        End Function
        Private Overloads Function UpdateToken(ByVal UserName As String, ByVal Password As String, ByVal ClientID As String, ByVal ClientSecret As String, ByVal e As ErrorsDescriber) As Boolean
            Try
                Dim result As Boolean = True
                If {UserName, Password, ClientID, ClientSecret}.All(Function(v) Not v.IsEmptyString) Then
                    result = False
                    Dim r$ = String.Empty
                    Dim c% = 0
                    Dim useCurl As Boolean = Settings.CurlFile.Exists And CBool(BearerTokenUseCurl.Value)
                    Dim curlUsed As Boolean = useCurl
                    Do
                        c += 1
                        Using resp As New Responser With {
                            .Method = "POST",
                            .ProcessExceptionDecision = Function(ByVal status As IResponserStatus, ByVal nullArg As Object, ByVal currErr As ErrorsDescriber) As ErrorsDescriber
                                                            If status.StatusCode = 429 Then
                                                                useCurl = False
                                                                Return EDP.ReturnValue
                                                            ElseIf status.StatusCode = Net.HttpStatusCode.Forbidden And Not useCurl And Settings.CurlFile.Exists Then
                                                                useCurl = True
                                                                Return EDP.ReturnValue
                                                            Else
                                                                Return currErr
                                                            End If
                                                        End Function
                        }
                            With resp
                                If useCurl Then
                                    If Settings.CurlFile.Exists Then
                                        curlUsed = True
                                        .Mode = Responser.Modes.Curl
                                        .CurlPath = Settings.CurlFile
                                        .CurlArgumentsLeft = $"-d ""grant_type=password&username={UserName}&password={Password}"" --user ""{ClientID}:{ClientSecret}"""
                                    Else
                                        Throw New ArgumentNullException("cUrl file", "The path to the cUrl file is not specified")
                                    End If
                                Else
                                    .Mode = Responser.Modes.Default
                                    With .PayLoadValues
                                        .Add("grant_type", "password")
                                        .Add("username", UserName)
                                        .Add("password", Password)
                                    End With
                                    .CredentialsUserName = ClientID
                                    .CredentialsPassword = ClientSecret
                                    .PreAuthenticate = True
                                End If
                            End With
                            r = resp.GetResponse("https://www.reddit.com/api/v1/access_token",, EDP.ThrowException)
                        End Using
                        If Not r.IsEmptyString Then
                            Using j As EContainer = JsonDocument.Parse(r)
                                If j.ListExists Then
                                    Dim newToken$ = j.Value("access_token")
                                    If Not newToken.IsEmptyString Then
                                        BearerToken.Value = $"Bearer {newToken}"
                                        BearerTokenDateUpdate.Value = Now
                                        Responser.SaveSettings()
                                        result = True
                                    End If
                                End If
                            End Using
                        End If
                    Loop While c < 5 And Not result
                End If
                Return result
            Catch ex As Exception
                Return ErrorsDescriber.Execute(e, ex, "[Reddit.SiteSettings.UpdateToken]", False)
            End Try
        End Function
#End Region
    End Class
End Namespace