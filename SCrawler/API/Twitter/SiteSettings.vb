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
Imports PersonalUtilities.Tools.WEB
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Twitter
    <Manifest("AndyProgram_Twitter"), SavedPosts, UseClassAsIs>
    Friend Class SiteSettings : Inherits SiteSettingsBase
        Friend Const Header_Authorization As String = "authorization"
        Friend Const Header_Token As String = "x-csrf-token"
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return My.Resources.TwitterIcon
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.TwitterPic400
            End Get
        End Property
        <PropertyOption(AllowNull:=False, ControlText:="Authorization",
                        ControlToolTip:="Set authorization from [authorization] response header. This field must start from [Bearer] key word")>
        Private ReadOnly Property Auth As PropertyValue
        <PropertyOption(AllowNull:=False, ControlText:="Token", ControlToolTip:="Set token from [x-csrf-token] response header")>
        Private ReadOnly Property Token As PropertyValue
        <PropertyOption(ControlText:="Saved posts user name", ControlToolTip:="Personal profile username", LeftOffset:=120), PXML>
        Friend ReadOnly Property SavedPostsUserName As PropertyValue
        Friend Overrides ReadOnly Property Responser As Response
        Friend Sub New()
            MyBase.New(TwitterSite)
            Responser = New Response($"{SettingsFolderName}\Responser_{Site}.xml")

            Dim a$ = String.Empty
            Dim t$ = String.Empty

            With Responser
                If .File.Exists Then
                    .LoadSettings()
                    With .Headers
                        If .ContainsKey(Header_Authorization) Then a = .Item(Header_Authorization)
                        If .ContainsKey(Header_Token) Then t = .Item(Header_Token)
                    End With
                Else
                    .ContentType = "application/json"
                    .Accept = "*/*"
                    .CookiesDomain = "twitter.com"
                    .Cookies = New CookieKeeper(.CookiesDomain)
                    .Decoders.Add(SymbolsConverter.Converters.Unicode)
                    With .Headers
                        .Add("sec-ch-ua", " Not;A Brand"";v=""99"", ""Google Chrome"";v=""91"", ""Chromium"";v=""91""")
                        .Add("sec-ch-ua-mobile", "?0")
                        .Add("sec-fetch-dest", "empty")
                        .Add("sec-fetch-mode", "cors")
                        .Add("sec-fetch-site", "same-origin")
                        .Add(Header_Token, String.Empty)
                        .Add("x-twitter-active-user", "yes")
                        .Add("x-twitter-auth-type", "OAuth2Session")
                        .Add(Header_Authorization, String.Empty)
                    End With
                    .SaveSettings()
                End If
            End With

            Auth = New PropertyValue(a, GetType(String), Sub(v) ChangeResponserFields(NameOf(Auth), v))
            Token = New PropertyValue(t, GetType(String), Sub(v) ChangeResponserFields(NameOf(Token), v))
            SavedPostsUserName = New PropertyValue(String.Empty, GetType(String))

            UserRegex = RParams.DMS("[htps:/]{7,8}.*?twitter.com/([^/]+)", 1)
            UrlPatternUser = "https://twitter.com/{0}"
            ImageVideoContains = "twitter"
        End Sub
        Private Sub ChangeResponserFields(ByVal PropName As String, ByVal Value As Object)
            If Not PropName.IsEmptyString Then
                Dim f$ = String.Empty
                Select Case PropName
                    Case NameOf(Auth) : f = Header_Authorization
                    Case NameOf(Token) : f = Header_Token
                End Select
                If Not f.IsEmptyString Then
                    If Responser.Headers.Count > 0 AndAlso Responser.Headers.ContainsKey(f) Then Responser.Headers.Remove(f)
                    If Not CStr(Value).IsEmptyString Then Responser.Headers.Add(f, CStr(Value))
                    Responser.SaveSettings()
                End If
            End If
        End Sub
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            If What = ISiteSettings.Download.SavedPosts Then
                Return New UserData With {.IsSavedPosts = True, .User = New UserInfo With {.Name = CStr(AConvert(Of String)(SavedPostsUserName.Value, String.Empty))}}
            Else
                Return New UserData
            End If
        End Function
        Friend Overrides Function GetSpecialDataF(ByVal URL As String) As IEnumerable(Of UserMedia)
            Return UserData.GetVideoInfo(URL, Responser)
        End Function
    End Class
End Namespace