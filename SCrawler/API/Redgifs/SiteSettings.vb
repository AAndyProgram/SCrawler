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
Imports PersonalUtilities.Tools.WEB
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Imports UStates = SCrawler.API.Base.UserMedia.States
Namespace API.RedGifs
    <Manifest(RedGifsSiteKey)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return My.Resources.SiteResources.RedGifsIcon_32
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.SiteResources.RedGifsPic_32
            End Get
        End Property
        <PropertyOption(AllowNull:=False, ControlText:="Token", ControlToolTip:="Bearer token")>
        Friend Property Token As PropertyValue
        Private Const TokenName As String = "authorization"
        Friend Sub New()
            MyBase.New(RedGifsSite, "redgifs.com")
            Dim t$ = String.Empty
            With Responser
                Dim b As Boolean = Not .UseWebClient Or Not .UseWebClientCookies Or Not .UseWebClientAdditionalHeaders
                .UseWebClient = True
                .UseWebClientCookies = True
                .UseWebClientAdditionalHeaders = True
                If .Headers.Count > 0 AndAlso .Headers.ContainsKey(TokenName) Then t = .Headers(TokenName)
                If b Then .SaveSettings()
            End With
            Token = New PropertyValue(t, GetType(String), Sub(v) UpdateResponse(v))
            UrlPatternUser = "https://www.redgifs.com/users/{0}/"
            UserRegex = RParams.DMS("[htps:/]{7,8}.*?redgifs.com/users/([^/]+)", 1)
            ImageVideoContains = "redgifs"
        End Sub
        Private Sub UpdateResponse(ByVal Value As String)
            With Responser.Headers
                If .Count = 0 OrElse Not .ContainsKey(TokenName) Then .Add(TokenName, Value) Else .Item(TokenName) = Value
                Responser.SaveSettings()
            End With
        End Sub
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
        Friend Overrides Function GetSpecialData(ByVal URL As String, ByVal Path As String, ByVal AskForPath As Boolean) As IEnumerable
            If BaseAuthExists() Then
                Using resp As Response = Responser.Copy
                    Dim m As UserMedia = UserData.GetDataFromUrlId(URL, False, resp, Settings(RedGifsSiteKey))
                    If Not m.State = UStates.Missing And Not m.State = UserData.DataGone And (m.Type = UTypes.Picture Or m.Type = UTypes.Video) Then
                        Try
                            Dim spf$ = String.Empty
                            Dim f As SFile = GetSpecialDataFile(Path, AskForPath, spf)
                            If f.IsEmptyString Then
                                f = m.File.File
                            Else
                                f.Name = m.File.Name
                                f.Extension = m.File.Extension
                            End If
                            resp.DownloadFile(m.URL, f, EDP.ThrowException)
                            m.State = UStates.Downloaded
                            m.SpecialFolder = spf
                            Return {m}
                        Catch ex As Exception
                            ErrorsDescriber.Execute(EDP.SendInLog, ex, $"Redgifs standalone download error: [{URL}]")
                        End Try
                    End If
                End Using
            End If
            Return Nothing
        End Function
        Friend Overrides Function GetUserPostUrl(ByVal UserID As String, ByVal PostID As String) As String
            Return $"https://www.redgifs.com/watch/{PostID}"
        End Function
        Friend Overrides Function BaseAuthExists() As Boolean
            Return If(Responser.Cookies?.Count, 0) > 0 AndAlso ACheck(Token.Value)
        End Function
    End Class
End Namespace