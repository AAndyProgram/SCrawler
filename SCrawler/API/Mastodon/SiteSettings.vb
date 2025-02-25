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
Imports DN = SCrawler.API.Base.DeclaredNames
Namespace API.Mastodon
    <Manifest(MastodonSiteKey), SavedPosts, SpecialForm(True), SpecialForm(False)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
#Region "Domains"
        <PXML("Domains"), PClonable> Private ReadOnly Property SiteDomains As PropertyValue
        Private Shadows ReadOnly Property DefaultInstance As SiteSettings
            Get
                Return MyBase.DefaultInstance
            End Get
        End Property
        Private ReadOnly _Domains As DomainsContainer
        Friend ReadOnly Property Domains As DomainsContainer
            Get
                Return If(DefaultInstance?.Domains, _Domains)
            End Get
        End Property
        <PXML("DomainsLastUpdateDate")> Private ReadOnly Property Base_DomainsLastUpdateDate As PropertyValue
        Private ReadOnly Property DomainsLastUpdateDate As PropertyValue
            Get
                Return If(DefaultInstance?.DomainsLastUpdateDate, Base_DomainsLastUpdateDate)
            End Get
        End Property
#End Region
#Region "Auth"
        <PropertyOption(IsAuth:=True, AllowNull:=False, ControlText:="My domain",
                        ControlToolTip:="Your account domain without 'https://' (for example, 'mastodon.social')"), PXML, PClonable(Clone:=False)>
        Friend ReadOnly Property MyDomain As PropertyValue
        <PropertyOption(AllowNull:=False, IsAuth:=True, ControlText:="Authorization",
                        ControlToolTip:="Set authorization from [authorization] response header. This field must start from [Bearer] key word"), PClonable(Clone:=False)>
        Friend ReadOnly Property Auth As PropertyValue
        <PropertyOption(AllowNull:=False, IsAuth:=True, ControlText:="Token", ControlToolTip:="Set token from [x-csrf-token] response header"), PClonable(Clone:=False)>
        Friend ReadOnly Property Token As PropertyValue
        Private Sub ChangeResponserFields(ByVal PropName As String, ByVal Value As Object)
            If Not PropName.IsEmptyString Then
                Dim f$ = String.Empty
                Select Case PropName
                    Case NameOf(Auth) : f = DN.Header_Authorization
                    Case NameOf(Token) : f = DN.Header_CSRFToken
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
        <PropertyOption(IsAuth:=False, ControlText:=DN.GifsDownloadCaption, Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property GifsDownload As PropertyValue
        <PropertyOption(IsAuth:=False, ControlText:=DN.GifsSpecialFolderCaption, ControlToolTip:=DN.GifsSpecialFolderToolTip, Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property GifsSpecialFolder As PropertyValue
        <PropertyOption(IsAuth:=False, ControlText:=DN.GifsPrefixCaption, ControlToolTip:=DN.GifsPrefixToolTip, Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property GifsPrefix As PropertyValue
        <Provider(NameOf(GifsSpecialFolder), Interaction:=True), Provider(NameOf(GifsPrefix), Interaction:=True)>
        Private ReadOnly Property GifStringChecker As IFormatProvider
        <PropertyOption(IsAuth:=False, ControlText:=DN.UseMD5ComparisonCaption, ControlToolTip:=DN.UseMD5ComparisonToolTip, Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property UseMD5Comparison As PropertyValue
        <PropertyOption(IsAuth:=False, ControlText:="User related to my domain",
                        ControlToolTip:="Open user profiles and user posts through my domain."), PXML, PClonable>
        Friend ReadOnly Property UserRelatedToMyDomain As PropertyValue
#End Region
#End Region
#Region "Initializer"
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New("Mastodon", "mastodon.social", AccName, Temp, My.Resources.SiteResources.MastodonIcon_48, My.Resources.SiteResources.MastodonPic_48)

            _Domains = New DomainsContainer(Me, "mastodon.social")
            SiteDomains = New PropertyValue(Domains.DomainsDefault, GetType(String))
            Domains.DestinationProp = SiteDomains
            Base_DomainsLastUpdateDate = New PropertyValue(Now.AddYears(-1))

            Auth = New PropertyValue(Responser.Headers.Value(DN.Header_Authorization), GetType(String), Sub(v) ChangeResponserFields(NameOf(Auth), v))
            Token = New PropertyValue(Responser.Headers.Value(DN.Header_CSRFToken), GetType(String), Sub(v) ChangeResponserFields(NameOf(Token), v))

            GifsDownload = New PropertyValue(True)
            GifsSpecialFolder = New PropertyValue(String.Empty, GetType(String))
            GifsPrefix = New PropertyValue("GIF_")
            GifStringChecker = New API.Twitter.SiteSettings.GifStringProvider
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
                        Return $"https://{ .UserDomain}/@{ .NameTrue}"
                    Else
                        Return $"https://{MyDomain.Value}/@{ .NameTrue}@{ .UserDomain}"
                    End If
                Else
                    Return $"https://{ .UserDomain}/@{ .NameTrue}"
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
        Friend Overloads Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            If Domains.Count > 0 Then
                Dim e As ExchangeOptions
                If ACheck(MyDomain.Value) Then
                    e = IsMyUser(UserURL, MyDomain.Value)
                    If Not e.SiteName.IsEmptyString Then Return e
                End If
                For Each domain$ In Domains
                    e = IsMyUser(UserURL, domain)
                    If Not e.SiteName.IsEmptyString Then Return e
                Next
            End If
            Return Nothing
        End Function
        Private Overloads Function IsMyUser(ByVal UserURL As String, ByVal Domain As String) As ExchangeOptions
            UserRegex.Pattern = String.Format(UserRegexDefault, Domain)
            Dim l As List(Of String) = RegexReplace(UserURL, UserRegex)
            If l.ListExists(2) Then Return New ExchangeOptions(Site, $"{l(2).IfNullOrEmpty(Domain)}@{l(1)}") Else Return Nothing
        End Function
        Friend Overrides Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions
            If Not URL.IsEmptyString And Domains.Count > 0 Then
                Dim urlDomain$ = RegexReplace(URL, RParams.DM("[^/]+", 1, EDP.ReturnValue, String.Empty))
                If Not urlDomain.IsEmptyString Then
                    urlDomain = urlDomain.StringToLower
                    If Domains.Domains.Exists(Function(d) urlDomain = d.StringToLower) Then Return New ExchangeOptions(Site, URL) With {.Exists = True}
                End If
            End If
            Return Nothing
        End Function
#End Region
#Region "UpdateServersList"
        Private Sub UpdateServersList()
            Try
                Using resp As New Responser With {
                    .ProcessExceptionDecision = Function(rr, obj, e) If(rr.StatusCode = Net.HttpStatusCode.ServiceUnavailable,
                                                                        EDP.ReturnValue, EDP.ThrowException)}
                    Dim r$ = resp.GetResponse("https://api.joinmastodon.org/servers?language=&category=&region=&ownership=&registrations=")
                    If Not r.IsEmptyString Then
                        Dim j As EContainer = JsonDocument.Parse(r, EDP.ReturnValue)
                        If If(j?.Count, 0) > 0 Then
                            Domains.Domains.ListAddList(j.Select(Function(e) e.Value("domain")), LAP.NotContainsOnly, EDP.ReturnValue)
                            Domains.Domains.Sort()
                            Domains.Save()
                            j.Dispose()
                        End If
                        DomainsLastUpdateDate.Value = Now
                    End If
                End Using
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[API.Mastodon.SiteSettings.UpdateServersList]")
            End Try
        End Sub
#End Region
#Region "IDisposable Support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then _Domains.Dispose()
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace