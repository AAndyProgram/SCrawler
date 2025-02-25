' Copyright (C) Andy https://github.com/AAndyProgram
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
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Documents.JSON
Namespace API.Bluesky
    <Manifest(BlueskySiteKey), SpecialForm(False)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
        <PropertyOption(ControlText:="Cookies enabled", ControlToolTip:="If checked, cookies will be used in requests", IsAuth:=True), PXML, PClonable, HiddenControl>
        Friend ReadOnly Property CookiesEnabled As PropertyValue
        <PropertyOption(ControlText:="User name", IsAuth:=True, AllowNull:=False), PXML>
        Friend ReadOnly Property UserHandle As PropertyValue
        <PropertyOption(ControlText:="Password", IsAuth:=True, AllowNull:=False), PXML>
        Friend ReadOnly Property UserPassword As PropertyValue
        <PXML> Friend ReadOnly Property Token As PropertyValue
        <PXML> Friend ReadOnly Property TokenUpdateTime As PropertyValue
        <PropertyOption(ControlText:="Token update", ControlToolTip:="Token refresh interval (in minutes)." & vbCr & "Default: 120.", IsAuth:=True), PXML, PClonable, HiddenControl>
        Friend ReadOnly Property TokenRefreshInterval As PropertyValue
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New("Bluesky", "bsky.app", AccName, Temp, My.Resources.SiteResources.BlueskyIcon_32, My.Resources.SiteResources.BlueskyPic_32)

            Responser.ContentType = "application/json"

            CookiesEnabled = New PropertyValue(False)
            UserHandle = New PropertyValue(String.Empty, GetType(String))
            UserPassword = New PropertyValue(String.Empty, GetType(String))
            Token = New PropertyValue(String.Empty, GetType(String))
            TokenUpdateTime = New PropertyValue(Now.AddYears(-1))
            TokenRefreshInterval = New PropertyValue(120)

            _AllowUserAgentUpdate = False
            UrlPatternUser = "https://bsky.app/profile/{0}"
            ImageVideoContains = "bsky.app"
            UserRegex = RParams.DMS("bsky.app/profile/([^/\?]+)", 1, EDP.ReturnValue)
            UserOptionsType = GetType(EditorExchangeOptionsBase)
        End Sub
        Protected Overrides Function UserOptionsValid(ByVal Options As Object) As Boolean
            Return DirectCast(Options, EditorExchangeOptionsBase).SiteKey = BlueskySiteKey
        End Function
        Protected Overrides Sub UserOptionsSetParameters(ByRef Options As Object)
            DirectCast(Options, EditorExchangeOptionsBase).SiteKey = BlueskySiteKey
        End Sub
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
        Friend Overrides Function BaseAuthExists() As Boolean
            Return Not CStr(UserHandle.Value).IsEmptyString And Not CStr(UserPassword.Value).IsEmptyString
        End Function
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            Return MyBase.Available(What, Silent) AndAlso UpdateToken()
        End Function
        Private _TokenUpdating As Boolean = False
        Friend Function UpdateToken(Optional ByVal Force As Boolean = False) As Boolean
            Try
                While _TokenUpdating : Threading.Thread.Sleep(100) : End While
                _TokenUpdating = True
                If BaseAuthExists() Then
                    If CDate(TokenUpdateTime.Value).AddMinutes(TokenRefreshInterval.Value) < Now Or Force Then
                        Using resp As Responser = Responser.Copy
                            With resp
                                .Mode = Responser.Modes.Curl
                                .Method = "POST"
                                .CurlSslNoRevoke = True
                                .CurlInsecure = True
                                .CurlArgumentsLeft = "-d ""{\" & $"""identifier\"": \""{UserHandle.Value}\"", \""password\"": \""{UserPassword.Value}\""" & "}"""

                                Dim r$ = .GetResponse("https://bsky.social/xrpc/com.atproto.server.createSession")
                                If Not r.IsEmptyString Then
                                    Using j As EContainer = JsonDocument.Parse(r, EDP.ReturnValue)
                                        If j.ListExists Then
                                            Dim t$ = j.Value("accessJwt")
                                            If Not t.IsEmptyString Then Token.Value = $"Bearer {t}" : TokenUpdateTime.Value = Now : Return True
                                        End If
                                    End Using
                                End If
                            End With
                        End Using
                    Else
                        Return True
                    End If
                End If
                Return False
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, "Bluesky.SiteSettings.UpdateToken", False)
            Finally
                _TokenUpdating = False
            End Try
        End Function
    End Class
End Namespace