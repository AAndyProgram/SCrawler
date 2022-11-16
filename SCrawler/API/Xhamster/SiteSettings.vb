' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports SCrawler.API.BaseObjects
Imports SCrawler.Plugin
Imports SCrawler.Plugin.Attributes
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Clients
Namespace API.Xhamster
    <Manifest(XhamsterSiteKey), SavedPosts, SpecialForm(True), TaskGroup(SettingsCLS.TaskStackNamePornSite)>
    Friend Class SiteSettings : Inherits SiteSettingsBase : Implements IDomainContainer
#Region "Declarations"
        Friend Overrides ReadOnly Property Icon As Icon Implements IDomainContainer.Icon
            Get
                Return My.Resources.SiteResources.XhamsterIcon_32
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.SiteResources.XhamsterPic_32
            End Get
        End Property
#Region "Domains"
        Private ReadOnly Property IDomainContainer_Site As String Implements IDomainContainer.Site
            Get
                Return Site
            End Get
        End Property
        <PXML("Domains")> Private ReadOnly Property SiteDomains As PropertyValue Implements IDomainContainer.DomainsSettingProp
        Friend ReadOnly Property Domains As List(Of String) Implements IDomainContainer.Domains
        Private ReadOnly Property DomainsTemp As List(Of String) Implements IDomainContainer.DomainsTemp
        Private Property DomainsChanged As Boolean = False Implements IDomainContainer.DomainsChanged
        Friend ReadOnly Property DomainsUpdated As Boolean
            Get
                Return DomainsUpdatedBySite
            End Get
        End Property
        Private ReadOnly Property DomainsDefault As String = "xhamster.com" Implements IDomainContainer.DomainsDefault
#End Region
        <PropertyOption(ControlText:="Download UHD", ControlToolTip:="Download UHD (4K) content"), PXML>
        Friend Property DownloadUHD As PropertyValue
        Private Property Initialized As Boolean = False Implements IDomainContainer.Initialized
#End Region
#Region "Initializer"
        Friend Sub New()
            MyBase.New("XHamster", "xhamster.com")

            Responser.DeclaredError = EDP.ThrowException

            Domains = New List(Of String)
            DomainsTemp = New List(Of String)
            SiteDomains = New PropertyValue(DomainsDefault, GetType(String), Sub(s) UpdateDomains())
            DownloadUHD = New PropertyValue(False)

            UrlPatternUser = "https://xhamster.com/users/{0}"
            UserRegex = RParams.DMS("xhamster.com/users/([^/]+).*?", 1)
            ImageVideoContains = "xhamster"
        End Sub
        Friend Overrides Sub EndInit()
            Initialized = True
            DomainContainer.EndInit(Me)
            DomainsTemp.ListAddList(Domains)
            MyBase.EndInit()
        End Sub
#End Region
#Region "UpdateDomains"
        Private Property DomainsUpdateInProgress As Boolean = False Implements IDomainContainer.DomainsUpdateInProgress
        Private Property DomainsUpdatedBySite As Boolean = False Implements IDomainContainer.DomainsUpdatedBySite
        Friend Overloads Sub UpdateDomains() Implements IDomainContainer.UpdateDomains
            DomainContainer.UpdateDomains(Me)
        End Sub
        Friend Overloads Sub UpdateDomains(ByVal NewDomains As IEnumerable(Of String), ByVal Internal As Boolean)
            DomainContainer.UpdateDomains(Me, NewDomains, Internal)
        End Sub
#End Region
#Region "Edit"
        Friend Overrides Sub Update()
            DomainContainer.Update(Me)
            Responser.SaveSettings()
            MyBase.Update()
        End Sub
        Friend Overrides Sub EndEdit()
            DomainContainer.EndEdit(Me)
            MyBase.EndEdit()
        End Sub
        Friend Overrides Sub OpenSettingsForm()
            DomainContainer.OpenSettingsForm(Me)
        End Sub
#End Region
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            If What = ISiteSettings.Download.SavedPosts Then
                Return New UserData With {.IsSavedPosts = True, .User = New UserInfo With {.Name = "xhamster"}}
            Else
                Return New UserData
            End If
        End Function
        Friend Overrides Function GetSpecialData(ByVal URL As String, ByVal Path As String, ByVal AskForPath As Boolean) As IEnumerable
            If Available(ISiteSettings.Download.Main, True) Then
                Using resp As Response = Responser.Copy
                    Dim spf$ = String.Empty
                    Dim f As SFile = GetSpecialDataFile(Path, AskForPath, spf)
                    Dim m As UserMedia = UserData.GetVideoInfo(URL, resp, f)
                    If m.State = UserMedia.States.Downloaded Then
                        m.SpecialFolder = f
                        Return {m}
                    End If
                End Using
            End If
            Return Nothing
        End Function
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, Silent As Boolean) As Boolean
            If Settings.UseM3U8 AndAlso MyBase.Available(What, Silent) Then
                If What = ISiteSettings.Download.SavedPosts Then
                    Return If(Responser.Cookies?.Count, 0) > 0
                Else
                    Return True
                End If
            Else
                Return False
            End If
        End Function
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            Return Media.URL_BASE
        End Function
#Region "Is my user/data"
        Private Const UserRegexDefault As String = "{0}/users/([^/]+).*?"
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            Dim b As ExchangeOptions = MyBase.IsMyUser(UserURL)
            If b.Exists Then Return b
            If Not UserURL.IsEmptyString And Domains.Count > 0 Then
                Dim uName$, fStr$
                Dim uErr As New ErrorsDescriber(EDP.ReturnValue)
                For i% = 0 To Domains.Count - 1
                    fStr = String.Format(UserRegexDefault, Domains(i))
                    uName = RegexReplace(UserURL, RParams.DMS(fStr, 1, uErr))
                    If Not uName.IsEmptyString Then Return New ExchangeOptions(Site, uName)
                Next
            End If
            Return Nothing
        End Function
        Friend Overrides Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions
            If Not URL.IsEmptyString And Domains.Count > 0 Then
                If Domains.Exists(Function(d) URL.Contains(d)) Then Return New ExchangeOptions With {.UserName = URL, .Exists = True}
            End If
            Return Nothing
        End Function
#End Region
    End Class
End Namespace