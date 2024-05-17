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
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Documents.JSON
Namespace API.RedGifs
    <Manifest(RedGifsSiteKey)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
        <PropertyOption(ControlToolTip:="Bearer token", AllowNull:=False), DependentFields(NameOf(UserAgent)), ControlNumber(1), PClonable(Clone:=False), HiddenControl>
        Friend ReadOnly Property Token As PropertyValue
        <PropertyOption, ControlNumber(2), PClonable, HiddenControl>
        Private ReadOnly Property UserAgent As PropertyValue
        <PXML> Friend ReadOnly Property TokenLastDateUpdated As PropertyValue
        Private Const TokenName As String = "authorization"
#Region "TokenUpdateInterval"
        <PropertyOption(ControlText:="Token refresh interval", ControlToolTip:="Interval (in minutes) to refresh the token", AllowNull:=False, LeftOffset:=120),
            PXML, ControlNumber(0), PClonable>
        Friend ReadOnly Property TokenUpdateInterval As PropertyValue
        <Provider(NameOf(TokenUpdateInterval), FieldsChecker:=True)>
        Private ReadOnly Property TokenUpdateIntervalProvider As IFormatProvider
#End Region
#End Region
#Region "Initializer"
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New(RedGifsSite, "redgifs.com", AccName, Temp, My.Resources.SiteResources.RedGifsIcon_32, My.Resources.SiteResources.RedGifsPic_32)
            Dim t$ = String.Empty
            With Responser
                .Mode = Responser.Modes.WebClient
                .ClientWebUseCookies = False
                .ClientWebUseHeaders = True
                t = .Headers.Value(TokenName)
            End With
            Token = New PropertyValue(t, GetType(String), Sub(v) UpdateResponse(NameOf(Token), v))
            UserAgent = New PropertyValue(If(Responser.UserAgentExists, Responser.UserAgent, String.Empty), GetType(String), Sub(v) UpdateResponse(NameOf(UserAgent), v))
            TokenLastDateUpdated = New PropertyValue(Now.AddYears(-1), GetType(Date))
            TokenUpdateInterval = New PropertyValue(60 * 12, GetType(Integer))
            TokenUpdateIntervalProvider = New TokenRefreshIntervalProvider
            _AllowUserAgentUpdate = False
            UrlPatternUser = "https://www.redgifs.com/users/{0}/"
            UserRegex = RParams.DMS(String.Format(UserRegexDefaultPattern, "redgifs.com/users/"), 1)
            ImageVideoContains = "redgifs"
        End Sub
#End Region
#Region "Response updater"
        Private Sub UpdateResponse(ByVal Name As String, ByVal Value As String)
            Select Case Name
                Case NameOf(Token) : Responser.Headers.Add(TokenName, Value)
                Case NameOf(UserAgent) : Responser.UserAgent = Value
            End Select
            Responser.SaveSettings()
        End Sub
#End Region
#Region "Token updaters"
        Friend Function UpdateTokenIfRequired() As Boolean
            Dim d As Date? = AConvert(Of Date)(TokenLastDateUpdated.Value, AModes.Var, Nothing)
            If Not d.HasValue OrElse d.Value < Now.AddMinutes(-CInt(TokenUpdateInterval.Value)) Then
                Return UpdateToken()
            Else
                Return True
            End If
        End Function
        <PropertyUpdater(NameOf(Token))>
        Friend Function UpdateToken() As Boolean
            Try
                Dim r$
                Dim NewToken$ = String.Empty, NewAgent$ = String.Empty
                Using resp As New Responser : r = resp.GetResponse("https://api.redgifs.com/v2/auth/temporary",, EDP.ThrowException) : End Using
                If Not r.IsEmptyString Then
                    Dim j As EContainer = JsonDocument.Parse(r)
                    If Not j Is Nothing Then
                        NewToken = j.Value("token")
                        NewAgent = j.Value("agent")
                        j.Dispose()
                    End If
                End If
                If Not NewToken.IsEmptyString Then
                    If Not NewAgent.IsEmptyString Then UserAgent.Value = NewAgent
                    Token.Value = $"Bearer {NewToken}"
                    TokenLastDateUpdated.Value = Now
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "[API.RedGifs.SiteSettings.UpdateToken]", False)
            End Try
        End Function
#End Region
#Region "Update settings"
        Private _LastTokenValue As String = String.Empty
        Friend Overrides Sub BeginEdit()
            _LastTokenValue = AConvert(Of String)(Token.Value, AModes.Var, String.Empty)
            MyBase.BeginEdit()
        End Sub
        Friend Overrides Sub Update()
            If _SiteEditorFormOpened Then
                Dim NewToken$ = AConvert(Of String)(Token.Value, AModes.Var, String.Empty)
                If Not _LastTokenValue = NewToken And Not NewToken.IsEmptyString Then TokenLastDateUpdated.Value = Now
                If Responser.CookiesExists AndAlso MsgBoxE({"RedGifs doesn't require cookies! Do you still want to use cookies?", "RedGifs cookies"},
                                                           vbExclamation,,, {"Use", "Don't use"}) = 1 Then Responser.Cookies.Clear()
            End If
            MyBase.Update()
        End Sub
        Friend Overrides Sub EndEdit()
            _LastTokenValue = String.Empty
            MyBase.EndEdit()
        End Sub
#End Region
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            Return $"https://www.redgifs.com/watch/{Media.Post.ID}"
        End Function
        Friend Overrides Function BaseAuthExists() As Boolean
            Return UpdateTokenIfRequired() AndAlso ACheck(Token.Value)
        End Function
    End Class
End Namespace