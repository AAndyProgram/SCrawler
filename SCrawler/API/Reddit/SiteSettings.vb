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
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports DownDetector = SCrawler.API.Base.DownDetector
Imports Download = SCrawler.Plugin.ISiteSettings.Download
Namespace API.Reddit
    <Manifest(RedditSiteKey), SavedPosts, SpecialForm(False)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
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
#End Region
#Region "Other"
        <PropertyOption(ControlText:="Use M3U8", ControlToolTip:="Use M3U8 or mp4 for Reddit videos", IsAuth:=False), PXML, PClonable>
        Friend ReadOnly Property UseM3U8 As PropertyValue
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
            TokenUpdateInterval = New PropertyValue(60 * 12)
            TokenUpdateIntervalProvider = New TokenRefreshIntervalProvider
            BearerTokenDateUpdate = New PropertyValue(Now.AddYears(-1))
            UseTokenForTimelines = New PropertyValue(False)
            UseTokenForSavedPosts = New PropertyValue(False)
            UseCookiesForTimelines = New PropertyValue(False)
            SavedPostsUserName = New PropertyValue(String.Empty, GetType(String))

            UseM3U8 = New PropertyValue(True)

            UrlPatternUser = "https://www.reddit.com/{0}/{1}/"
            ImageVideoContains = "reddit.com"
            UserRegex = RParams.DM("[htps:/]{7,8}.*?reddit.com/([user]{1,4})/([^/]+)", 0, RegexReturn.ListByMatch, EDP.ReturnValue)
        End Sub
#End Region
#Region "GetInstance"
        Friend Overrides Function GetInstance(ByVal What As Download) As IPluginContentProvider
            Return New UserData
        End Function
#End Region
#Region "DownloadStarted, ReadyToDownload, Available, DownloadDone, UpdateRedGifsToken"
        Private ____DownloadStarted As Boolean = False
        Friend Overrides Sub DownloadStarted(ByVal What As Download)
            If What = Download.Main Then ____DownloadStarted = True
            MyBase.DownloadStarted(What)
        End Sub
        Friend Property SessionInterrupted As Boolean = False
        Friend Overrides Function ReadyToDownload(ByVal What As Download) As Boolean
            If What = Download.Main Then
                Dim result As Boolean = Not SessionInterrupted
                If result Then
                    If ____DownloadStarted And ____AvailableRequested Then
                        ____AvailableResult = AvailableImpl(What, ____AvailableSilent)
                        ____AvailableChecked = True
                        ____AvailableRequested = False
                        result = ____AvailableResult
                    ElseIf ____AvailableChecked Then
                        result = ____AvailableResult
                    End If
                End If
                Return result
            Else
                Return True
            End If
        End Function
        Private ____AvailableRequested As Boolean = False
        Private ____AvailableSilent As Boolean = True
        Private ____AvailableChecked As Boolean = False
        Private ____AvailableResult As Boolean = False
        Friend Overrides Function Available(ByVal What As Download, ByVal Silent As Boolean) As Boolean
            If What = Download.Main And ____DownloadStarted Then
                ____AvailableRequested = True
                ____AvailableSilent = Silent
                Return True
            Else
                Return AvailableImpl(What, Silent)
            End If
        End Function
        Private Function AvailableImpl(ByVal What As Download, ByVal Silent As Boolean) As Boolean
            Try
                AvailableText = String.Empty
                Dim trueValue As Boolean = Not What = Download.SavedPosts OrElse (Responser.CookiesExists And ACheck(SavedPostsUserName.Value))
                If Not trueValue Then Return False
                Dim dl As List(Of DownDetector.Data) = DownDetector.GetData("reddit")
                If dl.ListExists Then
                    dl = dl.Take(4).ToList
                    Dim avg% = dl.Average(Function(d) d.Value)
                    If avg > 100 Then
                        AvailableText = "Over the past hour, Reddit has received an average of " &
                                        avg.NumToString(New ANumbers With {.FormatOptions = ANumbers.Options.GroupIntegral}) & " outage reports:" & vbCr &
                                        dl.ListToString(vbCr)
                        If Silent Then
                            Return False
                        Else
                            If MsgBoxE({$"{AvailableText}{vbCr}{vbCr}Do you want to continue parsing Reddit data?", "There are outage reports on Reddit"}, vbYesNo) = vbYes Then
                                If trueValue Then UpdateRedGifsToken()
                                Return trueValue AndAlso UpdateTokenIfRequired()
                            Else
                                Return False
                            End If
                        End If
                    End If
                End If
                If trueValue Then UpdateRedGifsToken()
                Return trueValue AndAlso UpdateTokenIfRequired()
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, "[API.Reddit.SiteSettings.Available]", True)
            End Try
        End Function
        Friend Overrides Sub DownloadDone(ByVal What As Download)
            SessionInterrupted = False
            ____DownloadStarted = False
            ____AvailableRequested = False
            ____AvailableChecked = False
            ____AvailableSilent = True
            ____AvailableResult = False
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
            With DirectCast(User, UserData) : Return String.Format(UrlPatternUser, IIf(.IsChannel, ChannelOption, "user"), .TrueName) : End With
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
            If (CBool(UseTokenForTimelines.Value) Or CBool(UseTokenForSavedPosts.Value)) AndAlso
               {AuthUserName.Value, AuthPassword.Value, ApiClientID.Value, ApiClientSecret.Value}.All(Function(v$) Not v.IsEmptyString) Then
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
                    Dim _found As Boolean
                    Do
                        c += 1
                        Using resp As New Responser With {
                            .Method = "POST",
                            .ProcessExceptionDecision = Function(status, obj, ee) If(status.StatusCode = 429, EDP.ReturnValue, ee)
                        }
                            With resp
                                With .PayLoadValues
                                    .Add("grant_type", "password")
                                    .Add("username", UserName)
                                    .Add("password", Password)
                                End With
                                .CredentialsUserName = ClientID
                                .CredentialsPassword = ClientSecret
                                .PreAuthenticate = True
                            End With
                            r = resp.GetResponse("https://www.reddit.com/api/v1/access_token",, EDP.ThrowException)
                        End Using
                        If Not r.IsEmptyString Then
                            Using j As EContainer = JsonDocument.Parse(r)
                                If j.ListExists Then
                                    _found = True
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
                    Loop While c < 5 And Not _found
                End If
                Return result
            Catch ex As Exception
                Return ErrorsDescriber.Execute(e, ex, "[Reddit.SiteSettings.UpdateToken]", False)
            End Try
        End Function
#End Region
    End Class
End Namespace