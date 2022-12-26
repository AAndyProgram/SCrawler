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
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Cookies
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Imports UStates = SCrawler.API.Base.UserMedia.States
Namespace API.RedGifs
    <Manifest(RedGifsSiteKey)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
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
        <PXML> Friend Property TokenLastDateUpdated As PropertyValue
        Private Const TokenName As String = "authorization"
#End Region
#Region "Initializer"
        Friend Sub New()
            MyBase.New(RedGifsSite, "redgifs.com")
            Dim t$ = String.Empty
            With Responser
                Dim b As Boolean = Not .Mode = Responser.Modes.WebClient
                .Mode = Responser.Modes.WebClient
                t = .Headers.Value(TokenName)
                If b Then .SaveSettings()
            End With
            Token = New PropertyValue(t, GetType(String), Sub(v) UpdateResponse(v))
            TokenLastDateUpdated = New PropertyValue(Now.AddYears(-1), GetType(Date))
            UrlPatternUser = "https://www.redgifs.com/users/{0}/"
            UserRegex = RParams.DMS("[htps:/]{7,8}.*?redgifs.com/users/([^/]+)", 1)
            ImageVideoContains = "redgifs"
        End Sub
#End Region
#Region "Response updater"
        Private Sub UpdateResponse(ByVal Value As String)
            Responser.Headers.Add(TokenName, Value)
            Responser.SaveSettings()
        End Sub
#End Region
#Region "Token updaters"
        Friend Function UpdateTokenIfRequired() As Boolean
            Dim d As Date? = AConvert(Of Date)(TokenLastDateUpdated.Value, AModes.Var, Nothing)
            If Not d.HasValue OrElse d.Value < Now.AddDays(-1) Then
                Return UpdateToken()
            Else
                Return True
            End If
        End Function
        <PropertyUpdater(NameOf(Token))>
        Friend Function UpdateToken() As Boolean
            Try
                Dim r$
                Dim NewToken$ = String.Empty
                Using resp As New Responser : r = resp.GetResponse("https://api.redgifs.com/v2/auth/temporary",, EDP.ThrowException) : End Using
                If Not r.IsEmptyString Then
                    Dim j As EContainer = JsonDocument.Parse(r)
                    If Not j Is Nothing Then
                        NewToken = j.Value("token")
                        j.Dispose()
                    End If
                End If
                If Not NewToken.IsEmptyString Then
                    Token.Value = $"Bearer {NewToken}"
                    TokenLastDateUpdated.Value = Now
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendInLog, ex, "[API.RedGifs.SiteSettings.UpdateToken]", False)
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
            Dim NewToken$ = AConvert(Of String)(Token.Value, AModes.Var, String.Empty)
            If Not _LastTokenValue = NewToken Then TokenLastDateUpdated.Value = Now
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
        Friend Overrides Function GetSpecialData(ByVal URL As String, ByVal Path As String, ByVal AskForPath As Boolean) As IEnumerable
            If BaseAuthExists() Then
                Using resp As Responser = Responser.Copy
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
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            Return $"https://www.redgifs.com/watch/{Media.Post.ID}"
        End Function
        Friend Overrides Function BaseAuthExists() As Boolean
            Return UpdateTokenIfRequired() AndAlso ACheck(Token.Value)
        End Function
    End Class
End Namespace