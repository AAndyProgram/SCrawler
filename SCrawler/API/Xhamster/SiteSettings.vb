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
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Xhamster
    <Manifest(XhamsterSiteKey), SavedPosts, SpecialForm(True), TaskGroup(SettingsCLS.TaskStackNamePornSite)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return My.Resources.SiteResources.XhamsterIcon_32
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.SiteResources.XhamsterPic_32
            End Get
        End Property
        <PXML("Domains")> Private ReadOnly Property SiteDomains As PropertyValue
        Friend ReadOnly Property Domains As DomainsContainer
        <PropertyOption(ControlText:="Download UHD", ControlToolTip:="Download UHD (4K) content"), PXML>
        Friend Property DownloadUHD As PropertyValue
#End Region
#Region "Initializer"
        Friend Sub New()
            MyBase.New("XHamster", "xhamster.com")

            Domains = New DomainsContainer(Me, "xhamster.com")
            SiteDomains = New PropertyValue(Domains.DomainsDefault, GetType(String))
            Domains.DestinationProp = SiteDomains
            DownloadUHD = New PropertyValue(False)


            UrlPatternUser = "https://xhamster.com/{0}/{1}"
            UserRegex = RParams.DMS($"/({UserOption}|{ChannelOption})/([^/]+)(\Z|.*)", 0, RegexReturn.ListByMatch)
            ImageVideoContains = "xhamster"
        End Sub
        Friend Overrides Sub EndInit()
            Domains.PopulateInitialDomains(SiteDomains.Value)
            MyBase.EndInit()
        End Sub
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
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            If Settings.UseM3U8 AndAlso MyBase.Available(What, Silent) Then
                If What = ISiteSettings.Download.SavedPosts Then
                    Return Responser.CookiesExists
                Else
                    Return True
                End If
            Else
                Return False
            End If
        End Function
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider) As String
            With DirectCast(User, UserData) : Return String.Format(UrlPatternUser, IIf(.IsChannel, ChannelOption, UserOption), .TrueName) : End With
        End Function
#Region "IsMyUser, IsMyImageVideo"
        Friend Const ChannelOption As String = "channels"
        Private Const UserOption As String = "users"
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            If Not UserURL.IsEmptyString AndAlso Domains.Domains.Count > 0 AndAlso Domains.Domains.Exists(Function(d) UserURL.ToLower.Contains(d.ToLower)) Then
                Dim data As List(Of String) = RegexReplace(UserURL, UserRegex)
                If data.ListExists(3) AndAlso Not data(2).IsEmptyString Then
                    Dim n$ = data(2)
                    If Not data(1).IsEmptyString AndAlso data(1) = ChannelOption Then n &= $"@{data(1)}"
                    Return New ExchangeOptions(Site, n)
                End If
            End If
            Return Nothing
        End Function
        Friend Overrides Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions
            If Not URL.IsEmptyString And Domains.Domains.Count > 0 Then
                If Domains.Domains.Exists(Function(d) URL.Contains(d)) Then Return New ExchangeOptions(Site, URL)
            End If
            Return Nothing
        End Function
#End Region
    End Class
End Namespace