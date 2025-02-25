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
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Cookies
Imports DN = SCrawler.API.Base.DeclaredNames
Imports IG = SCrawler.API.Instagram.SiteSettings
Namespace API.Twitter
    <Manifest(TwitterSiteKey), SavedPosts, SeparatedTasks, SpecialForm(False)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
#Region "Icon"
        <PXML("UseNewIcon")> Private ReadOnly Property UseNewIconXML As PropertyValue
        <PropertyOption(ControlText:="Use the new Twitter icon (X)", ControlToolTip:="Restart SCrawler to take effect")>
        Private ReadOnly Property UseNewIcon As PropertyValue
            Get
                If Not DefaultInstance Is Nothing Then
                    Return DirectCast(DefaultInstance, SiteSettings).UseNewIconXML
                Else
                    Return UseNewIconXML
                End If
            End Get
        End Property
        Private Sub UpdateIcon()
            If CBool(UseNewIcon.Value) Then _Icon = My.Resources.SiteResources.TwitterIconNew_32 : _Image = _Icon.ToBitmap
        End Sub
#End Region
#Region "Categories"
        Private Const CAT_DOWN As String = "Downloading"
#End Region
#Region "Auth"
        <PropertyOption(ControlText:="Update cookies", ControlToolTip:="Update cookies during requests", IsAuth:=True), PXML, PClonable, HiddenControl>
        Friend ReadOnly Property CookiesUpdate As PropertyValue
        <PropertyOption(ControlText:="Use UserAgent", ControlToolTip:="Use UserAgent in requests", IsAuth:=True), PXML, PClonable>
        Friend ReadOnly Property UserAgentUse As PropertyValue
        <PropertyOption(ControlText:="UserAgent", IsAuth:=True, AllowNull:=True, InheritanceName:=SettingsCLS.HEADER_DEF_UserAgent),
         PXML("UserAgent", OnlyForChecked:=True), PClonable>
        Private ReadOnly Property UserAgentXML As PropertyValue
        Friend ReadOnly Property UserAgent As String
            Get
                If CBool(UserAgentUse.Value) AndAlso Not CStr(UserAgentXML.Value).IsEmptyString Then
                    Return UserAgentXML.Value
                Else
                    Return String.Empty
                End If
            End Get
        End Property
#End Region
#Region "Other properties"
        <PropertyOption(ControlText:="Use the appropriate model",
                        ControlToolTip:="Use the appropriate model for new users." & vbCr &
                        "If disabled, all download models will be used for the first download. " &
                        "Next, the appropriate download model will be automatically selected." & vbCr &
                        "Otherwise the appropriate download model will be selected right from the start.", Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property UseAppropriateModel As PropertyValue
#Region "End points"
        <PropertyOption(ControlText:="New endpoint: search", ControlToolTip:="Use new endpoint argument (-o search-endpoint=graphql) for the search model.",
                        Category:=CAT_DOWN), PXML, PClonable>
        Friend Property UseNewEndPointSearch As PropertyValue
        <PropertyOption(ControlText:="New endpoint: profiles", ControlToolTip:="Use new endpoint argument (-o search-endpoint=graphql) for the profile models.",
                        Category:=CAT_DOWN), PXML, PClonable>
        Friend Property UseNewEndPointProfiles As PropertyValue
#End Region
#Region "Limits"
        Friend Const TimerDisabled As Integer = -1
        Friend Const TimerFirstUseTheSame As Integer = -2
        <PropertyOption(ControlText:="Abort on limit", ControlToolTip:="Abort twitter downloading when limit is reached", Category:=CAT_DOWN), PXML, PClonable>
        Friend Property AbortOnLimit As PropertyValue
        <PropertyOption(ControlText:="Download already parsed", ControlToolTip:="Download already parsed content on abort", Category:=CAT_DOWN), PXML, PClonable>
        Friend Property DownloadAlreadyParsed As PropertyValue
#End Region
#Region "Timers"
        <PropertyOption(ControlText:="Sleep timer",
                        ControlToolTip:="Use sleep timer in requests." & vbCr &
                                        "You can set a timer value (in seconds) to wait before each subsequent request." & vbCr &
                                        "-1 to disable and use the default algorithm." & vbCr &
                                        "Default: 20", Category:=CAT_DOWN), PXML, PClonable>
        Friend ReadOnly Property SleepTimer As PropertyValue
        <Provider(NameOf(SleepTimer), FieldsChecker:=True)>
        Private ReadOnly Property SleepTimerProvider As IFormatProvider
        <PropertyOption(ControlText:="Sleep timer at start",
                        ControlToolTip:="Set a sleep timer (in seconds) before the first request." & vbCr &
                                        "-1 to disable" & vbCr &
                                        "-2 to use the 'Sleep timer' value" & vbCr &
                                        "Default: -2", Category:=CAT_DOWN), PXML, PClonable>
        Friend ReadOnly Property SleepTimerBeforeFirst As PropertyValue
        <Provider(NameOf(SleepTimerBeforeFirst), FieldsChecker:=True)>
        Private ReadOnly Property SleepTimerBeforeFirstProvider As IFormatProvider
#End Region
        <PropertyOption(ControlText:="Media Model: allow non-user tweets", ControlToolTip:="Allow downloading non-user tweets in the media-model.", Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property MediaModelAllowNonUserTweets As PropertyValue
        <PropertyOption(ControlText:=DN.GifsDownloadCaption, Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property GifsDownload As PropertyValue
        <PropertyOption(ControlText:=DN.GifsSpecialFolderCaption, ControlToolTip:=DN.GifsSpecialFolderToolTip, Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property GifsSpecialFolder As PropertyValue
        <PropertyOption(ControlText:=DN.GifsPrefixCaption, ControlToolTip:=DN.GifsPrefixToolTip, Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property GifsPrefix As PropertyValue
        <Provider(NameOf(GifsSpecialFolder), Interaction:=True), Provider(NameOf(GifsPrefix), Interaction:=True)>
        Private ReadOnly Property GifStringChecker As IFormatProvider
        Friend Class GifStringProvider : Implements ICustomProvider, IPropertyProvider
            Friend Property PropertyName As String Implements IPropertyProvider.PropertyName
            Private Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                     Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object Implements ICustomProvider.Convert
                Dim v$ = AConvert(Of String)(Value, String.Empty)
                If Not v.IsEmptyString Then
                    If PropertyName = NameOf(GifsPrefix) Then
                        v = v.StringRemoveWinForbiddenSymbols
                    Else
                        v = v.StringReplaceSymbols(GetWinForbiddenSymbols.ToList.ListWithRemove("\").ToArray, String.Empty, EDP.ReturnValue)
                    End If
                End If
                Return v
            End Function
            Private Function GetFormat(ByVal FormatType As Type) As Object Implements IFormatProvider.GetFormat
                Throw New NotImplementedException("[GetFormat] is not available in the context of [GifStringProvider]")
            End Function
        End Class
        <PropertyOption(ControlText:=DN.UseMD5ComparisonCaption, ControlToolTip:=DN.UseMD5ComparisonToolTip, Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property UseMD5Comparison As PropertyValue
        <PropertyOption(ControlText:=DN.ConcurrentDownloadsCaption,
                        ControlToolTip:=DN.ConcurrentDownloadsToolTip, AllowNull:=False, LeftOffset:=120, Category:=CAT_DOWN), PXML, TaskCounter, PClonable>
        Friend ReadOnly Property ConcurrentDownloads As PropertyValue
        <Provider(NameOf(ConcurrentDownloads), FieldsChecker:=True)>
        Private ReadOnly Property MyConcurrentDownloadsProvider As IFormatProvider
#End Region
        Friend Overrides Property DefaultInstance As ISiteSettings
            Get
                Return MyBase.DefaultInstance
            End Get
            Set(ByVal Instance As ISiteSettings)
                MyBase.DefaultInstance = Instance
                If Not Instance Is Nothing Then UpdateIcon()
            End Set
        End Property
#End Region
#Region "Initializer"
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New(TwitterSite, "x.com", AccName, Temp, My.Resources.SiteResources.TwitterIcon_32, My.Resources.SiteResources.TwitterIcon_32.ToBitmap)

            LimitSkippedUsers = New List(Of UserDataBase)

            With Responser
                .Cookies.ChangedAllowInternalDrop = False
                .Cookies.Changed = False
            End With

            UseNewIconXML = New PropertyValue(False)

            CookiesUpdate = New PropertyValue(False)
            UserAgentUse = New PropertyValue(True)
            UserAgentXML = New PropertyValue(If(Responser.UserAgentExists, Responser.UserAgent, String.Empty), GetType(String),
                                             Sub(ByVal Value As Object)
                                                 Responser.UserAgent = CStr(Value)
                                                 Responser.SaveSettings(, EDP.ReturnValue)
                                             End Sub)

            UseAppropriateModel = New PropertyValue(True)

            UseNewEndPointSearch = New PropertyValue(True)
            UseNewEndPointProfiles = New PropertyValue(True)

            AbortOnLimit = New PropertyValue(True)
            DownloadAlreadyParsed = New PropertyValue(True)
            SleepTimer = New PropertyValue(TimerDisabled)
            SleepTimerProvider = New IG.TimersChecker(TimerDisabled)
            SleepTimerBeforeFirst = New PropertyValue(TimerFirstUseTheSame)
            SleepTimerBeforeFirstProvider = New IG.TimersChecker(TimerFirstUseTheSame)

            MediaModelAllowNonUserTweets = New PropertyValue(False)
            GifsDownload = New PropertyValue(True)
            GifsSpecialFolder = New PropertyValue(String.Empty, GetType(String))
            GifsPrefix = New PropertyValue("GIF_")
            GifStringChecker = New GifStringProvider
            UseMD5Comparison = New PropertyValue(False)
            ConcurrentDownloads = New PropertyValue(1)
            MyConcurrentDownloadsProvider = New ConcurrentDownloadsProvider

            _AllowUserAgentUpdate = False
            UserRegex = RParams.DMS(String.Format(UserRegexDefaultPattern, $"/(twitter|x).com({CommunitiesUser}|)/"), 3)
            UrlPatternUser = "https://x.com/{0}"
            ImageVideoContains = "twitter"
            CheckNetscapeCookiesOnEndInit = True
            UseNetscapeCookies = True
        End Sub
        Friend Overrides Sub EndInit()
            UpdateIcon()
            MyBase.EndInit()
        End Sub
#End Region
#Region "GetInstance"
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
#End Region
#Region "BaseAuthExists, Available"
        Friend Overrides Function BaseAuthExists() As Boolean
            Return Responser.CookiesExists
        End Function
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            Return Settings.GalleryDLFile.Exists And BaseAuthExists()
        End Function
#End Region
#Region "Download"
        Friend Property LIMIT_ABORT As Boolean = False
        Friend ReadOnly Property LimitSkippedUsers As List(Of UserDataBase)
        Friend Property UserNumber As Integer = -1
        Friend Overrides Sub DownloadStarted(ByVal What As ISiteSettings.Download)
            UserNumber = 0
            MyBase.DownloadStarted(What)
        End Sub
        Friend Overrides Sub AfterDownload(ByVal User As Object, ByVal What As ISiteSettings.Download)
            If Not User Is Nothing AndAlso DirectCast(User, UserData).GDL_REQUESTS_COUNT > 0 Then UserNumber += 1
            MyBase.AfterDownload(User, What)
        End Sub
        Friend Overrides Sub DownloadDone(ByVal What As ISiteSettings.Download)
            If UserNumber > 0 Then
                If CBool(CookiesUpdate.Value) Then
                    With CookieKeeper.ParseNetscapeText(CookiesNetscapeFile.GetText(EDP.ReturnValue), EDP.ReturnValue)
                        If .ListExists Then
                            Responser.Cookies.Clear()
                            Responser.Cookies.AddRange(.Self,, EDP.ReturnValue)
                            Responser.SaveCookies(EDP.ReturnValue)
                        Else
                            Update_SaveCookiesNetscape(True)
                        End If
                    End With
                Else
                    Update_SaveCookiesNetscape(True)
                End If
            End If
            UserNumber = -1
            If LimitSkippedUsers.Count > 0 Then
                With LimitSkippedUsers
                    If .Count = 1 Then
                        MyMainLOG = $"{ .Item(0).ToStringForLog}: twitter limit reached. Data has not been downloaded."
                    Else
                        MyMainLOG = "The following twitter users have not been downloaded (twitter limit reached):" & vbNewLine &
                                    .ListToStringE(vbNewLine, New CustomProvider(Function(v As UserDataBase) $"{v.Name} ({v.ToStringForLog})"))
                    End If
                    .Clear()
                End With
            End If
            LIMIT_ABORT = False
            MyBase.DownloadDone(What)
        End Sub
#End Region
#Region "UserOptions, Update"
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse (Not TypeOf Options Is EditorExchangeOptions OrElse
                                          Not DirectCast(Options, EditorExchangeOptions).SiteKey = TwitterSiteKey) Then _
               Options = New EditorExchangeOptions(Me)
            If OpenForm Then
                Using f As New InternalSettingsForm(Options, Me, False) : f.ShowDialog() : End Using
            End If
        End Sub
        Friend Overrides Sub Update()
            If _SiteEditorFormOpened Then
                Dim tf$ = GifsSpecialFolder.Value
                If Not tf.IsEmptyString Then tf = tf.StringTrim("\") : GifsSpecialFolder.Value = tf
            End If
            MyBase.Update()
        End Sub
#End Region
#Region "IsMyUser, IsMyImageVideo, GetUserPostUrl, GetUserUrl"
        Friend Const CommunitiesUser As String = "/i/communities"
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            Dim e As ExchangeOptions = MyBase.IsMyUser(UserURL)
            If Not e.UserName.IsEmptyString Then
                If UserURL.Contains(CommunitiesUser) Then e.Options = CommunitiesUser : e.UserName &= "@c"
                Return e
            Else
                Return Nothing
            End If
        End Function
        Friend Overrides Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions
            If Not URL.IsEmptyString AndAlso (URL.Contains("twitter") Or URL.Contains("x.com")) Then
                Return New ExchangeOptions(Site, String.Empty) With {.Exists = True}
            Else
                Return Nothing
            End If
        End Function
        Friend Const SinglePostPattern As String = "https://x.com/i/web/status/{0}"
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            Return String.Format(SinglePostPattern, Media.Post.ID)
        End Function
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider) As String
            Return DirectCast(User, UserData).GetUserUrl
        End Function
#End Region
    End Class
End Namespace