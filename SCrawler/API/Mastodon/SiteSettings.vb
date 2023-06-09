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
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports TS = SCrawler.API.Twitter.SiteSettings
Namespace API.Mastodon
    <Manifest(MastodonSiteKey), SavedPosts, SpecialForm(True), SpecialForm(False)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return My.Resources.SiteResources.MastodonIcon_48
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.SiteResources.MastodonPic_48
            End Get
        End Property
#Region "Domains"
        <PXML("Domains")> Private ReadOnly Property SiteDomains As PropertyValue
        Friend ReadOnly Property Domains As MastodonDomains
        <PXML> Private ReadOnly Property DomainsLastUpdateDate As PropertyValue
#End Region
#Region "Auth"
        <PropertyOption(IsAuth:=True, AllowNull:=False, ControlText:="My domain",
                        ControlToolTip:="Your account domain without 'https://' (for example, 'mastodon.social')"), PXML>
        Friend ReadOnly Property MyDomain As PropertyValue
        <PropertyOption(AllowNull:=False, IsAuth:=True, ControlText:="Authorization",
                        ControlToolTip:="Set authorization from [authorization] response header. This field must start from [Bearer] key word")>
        Friend ReadOnly Property Auth As PropertyValue
        <PropertyOption(AllowNull:=False, IsAuth:=True, ControlText:="Token", ControlToolTip:="Set token from [x-csrf-token] response header")>
        Friend ReadOnly Property Token As PropertyValue
        Private Sub ChangeResponserFields(ByVal PropName As String, ByVal Value As Object)
            If Not PropName.IsEmptyString Then
                Dim f$ = String.Empty
                Select Case PropName
                    Case NameOf(Auth) : f = TS.Header_Authorization
                    Case NameOf(Token) : f = TS.Header_Token
                End Select
                If Not f.IsEmptyString Then
                    Responser.Headers.Remove(f)
                    If Not CStr(Value).IsEmptyString Then Responser.Headers.Add(f, CStr(Value))
                    Responser.SaveSettings()
                End If
            End If
        End Sub
#End Region
#Region "Other properties"
        <PropertyOption(IsAuth:=False, ControlText:=TS.GifsDownload_Text), PXML>
        Friend ReadOnly Property GifsDownload As PropertyValue
        <PropertyOption(IsAuth:=False, ControlText:=TS.GifsSpecialFolder_Text, ControlToolTip:=TS.GifsSpecialFolder_ToolTip), PXML>
        Friend ReadOnly Property GifsSpecialFolder As PropertyValue
        <PropertyOption(IsAuth:=False, ControlText:=TS.GifsPrefix_Text, ControlToolTip:=TS.GifsPrefix_ToolTip), PXML>
        Friend ReadOnly Property GifsPrefix As PropertyValue
        <Provider(NameOf(GifsSpecialFolder), Interaction:=True), Provider(NameOf(GifsPrefix), Interaction:=True)>
        Private ReadOnly Property GifStringChecker As IFormatProvider
        <PropertyOption(IsAuth:=False, ControlText:=TS.UseMD5Comparison_Text, ControlToolTip:=TS.UseMD5Comparison_ToolTip), PXML>
        Friend ReadOnly Property UseMD5Comparison As PropertyValue
        <PropertyOption(IsAuth:=False, ControlText:="User related to my domain",
                        ControlToolTip:="Open user profiles and user posts through my domain."), PXML>
        Friend ReadOnly Property UserRelatedToMyDomain As PropertyValue
#End Region
#End Region
#Region "Initializer"
        Friend Sub New()
            MyBase.New("Mastodon", "mastodon.social")

            Domains = New MastodonDomains(Me, "mastodon.social")
            SiteDomains = New PropertyValue(Domains.DomainsDefault, GetType(String))
            Domains.DestinationProp = SiteDomains
            DomainsLastUpdateDate = New PropertyValue(Now.AddYears(-1))

            Auth = New PropertyValue(Responser.Headers.Value(TS.Header_Authorization), GetType(String), Sub(v) ChangeResponserFields(NameOf(Auth), v))
            Token = New PropertyValue(Responser.Headers.Value(TS.Header_Token), GetType(String), Sub(v) ChangeResponserFields(NameOf(Token), v))

            GifsDownload = New PropertyValue(True)
            GifsSpecialFolder = New PropertyValue(String.Empty, GetType(String))
            GifsPrefix = New PropertyValue("GIF_")
            GifStringChecker = New TS.GifStringProvider
            UseMD5Comparison = New PropertyValue(False)
            MyDomain = New PropertyValue(String.Empty, GetType(String))
            UserRelatedToMyDomain = New PropertyValue(False)

            UserRegex = RParams.DMS("", 0, RegexReturn.ListByMatch, EDP.ReturnValue)
        End Sub
        Friend Overrides Sub EndInit()
            Domains.PopulateInitialDomains(SiteDomains.Value)
            If CDate(DomainsLastUpdateDate.Value).AddDays(7) < Now Then UpdateServersList()
            MyBase.EndInit()
        End Sub
#End Region
#Region "GetInstance"
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
#End Region
#Region "Domains Support"
        Protected Overrides Sub DomainsApply()
            Domains.Apply()
            MyBase.DomainsApply()
        End Sub
        Protected Overrides Sub DomainsReset()
            Domains.Reset()
            MyBase.DomainsReset()
        End Sub
        Friend Overrides Sub OpenSettingsForm()
            Domains.OpenSettingsForm()
        End Sub
#End Region
#Region "Update"
        Friend Overrides Sub Update()
            If _SiteEditorFormOpened Then
                Dim tf$ = GifsSpecialFolder.Value
                If Not tf.IsEmptyString Then tf = tf.StringTrim("\") : GifsSpecialFolder.Value = tf
                Dim md$ = AConvert(Of String)(MyDomain.Value, String.Empty)
                If Not md.IsEmptyString AndAlso Not Domains.Domains.Contains(md) AndAlso Not Domains.DomainsTemp.Contains(md) Then
                    If Domains.Changed Then
                        Domains.DomainsTemp.Add(md)
                    Else
                        Domains.Domains.Add(md)
                        Domains.Save()
                    End If
                End If
            End If
            MyBase.Update()
        End Sub
#End Region
#Region "UserOptions"
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse (Not TypeOf Options Is EditorExchangeOptions OrElse
                                          Not DirectCast(Options, EditorExchangeOptions).SiteKey = MastodonSiteKey) Then _
               Options = New EditorExchangeOptions(Me) With {.SiteKey = MastodonSiteKey}
            If OpenForm Then
                Using f As New InternalSettingsForm(Options, Me, False) : f.ShowDialog() : End Using
            End If
        End Sub
#End Region
#Region "Download"
        Friend Overrides Function BaseAuthExists() As Boolean
            Return ACheck(Token.Value) And ACheck(Auth.Value) And ACheck(MyDomain.Value)
        End Function
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            If What = ISiteSettings.Download.SavedPosts Or What = ISiteSettings.Download.SingleObject Then
                If Not ACheck(MyDomain.Value) Then MyMainLOG = "Mastodon account domain not set" : Return False
            Else
                If CDate(DomainsLastUpdateDate.Value).AddDays(7) < Now Then UpdateServersList()
            End If
            Return MyBase.Available(What, Silent)
        End Function
#End Region
#Region "GetUserUrl, GetUserPostUrl"
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider) As String
            If Not ACheck(MyDomain.Value) Then Return String.Empty
            With DirectCast(User, UserData)
                If UserRelatedToMyDomain.Value Then
                    If MyDomain.Value = .UserDomain Then
                        Return $"https://{ .UserDomain}/@{ .TrueName}"
                    Else
                        Return $"https://{MyDomain.Value}/@{ .TrueName}@{ .UserDomain}"
                    End If
                Else
                    Return $"https://{ .UserDomain}/@{ .TrueName}"
                End If
            End With
        End Function
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            If Not ACheck(MyDomain.Value) Then Return String.Empty
            Return $"{GetUserUrl(User)}/{Media.Post.ID}"
        End Function
#End Region
#Region "IsMyUser, IsMyImageVideo"
        Private Const UserRegexDefault As String = "https?://{0}/@([^/@]+)@?([^/]*)"
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            If Domains.Count > 0 Then
                Dim l As List(Of String)
                For Each domain$ In Domains
                    UserRegex.Pattern = String.Format(UserRegexDefault, domain)
                    l = RegexReplace(UserURL, UserRegex)
                    If l.ListExists(2) Then Return New ExchangeOptions(Site, $"{l(2).IfNullOrEmpty(domain)}@{l(1)}")
                Next
            End If
            Return Nothing
        End Function
        Friend Overrides Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions
            If Not URL.IsEmptyString And Domains.Count > 0 Then
                If Domains.Domains.Exists(Function(d) URL.Contains(d)) Then Return New ExchangeOptions(Site, URL) With {.Exists = True}
            End If
            Return Nothing
        End Function
#End Region
#Region "UpdateServersList"
        Private Sub UpdateServersList()
            Try
                Dim r$ = GetWebString("https://api.joinmastodon.org/servers?language=&category=&region=&ownership=&registrations=",, EDP.ThrowException)
                If Not r.IsEmptyString Then
                    Dim j As EContainer = JsonDocument.Parse(r, EDP.ReturnValue)
                    If If(j?.Count, 0) > 0 Then
                        Domains.Domains.ListAddList(j.Select(Function(e) e.Value("domain")), LAP.NotContainsOnly, EDP.ReturnValue)
                        Domains.Domains.Sort()
                        Domains.Save()
                        j.Dispose()
                    End If
                End If
                DomainsLastUpdateDate.Value = Now
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[API.Mastodon.SiteSettings.UpdateServersList]")
            End Try
        End Sub
#End Region
    End Class
End Namespace