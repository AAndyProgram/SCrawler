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
Namespace API.XVIDEOS
    <Manifest(XvideosSiteKey), SavedPosts, SpecialForm(True), TaskGroup(SettingsCLS.TaskStackNamePornSite)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return My.Resources.SiteResources.XvideosIcon_48
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.SiteResources.XvideosPic_32
            End Get
        End Property
        <PXML("Domains")> Private ReadOnly Property SiteDomains As PropertyValue
        Friend ReadOnly Property Domains As DomainsContainer
        <PropertyOption(ControlText:="Download UHD", ControlToolTip:="Download UHD (4K) content"), PXML>
        Friend Property DownloadUHD As PropertyValue
        <PropertyOption(ControlText:="Playlist of saved videos",
                        ControlToolTip:="Your personal videos playlist to download as 'saved posts'. " & vbCr &
                                        "This playlist must be private (Visibility = 'Only me'). It also required cookies." & vbCr &
                                        "This playlist must be entered by pattern: https://www.xvideos.com/favorite/01234567/playlistname.",
                        LeftOffset:=130), PXML>
        Friend ReadOnly Property SavedVideosPlaylist As PropertyValue
#End Region
#Region "Initializer"
        Friend Sub New()
            MyBase.New("XVIDEOS", "www.xvideos.com")
            Domains = New DomainsContainer(Me, "xvideos.com|xnxx.com")
            SiteDomains = New PropertyValue(Domains.DomainsDefault, GetType(String))
            Domains.DestinationProp = SiteDomains
            DownloadUHD = New PropertyValue(False)
            SavedVideosPlaylist = New PropertyValue(String.Empty, GetType(String))
            UrlPatternUser = "https://xvideos.com/{0}"
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
#Region "Download"
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            If Settings.UseM3U8 Then
                If What = ISiteSettings.Download.SavedPosts Then
                    Return ACheck(SavedVideosPlaylist.Value) And Responser.CookiesExists
                Else
                    Return True
                End If
            Else
                Return False
            End If
        End Function
#End Region
#Region "User: get, check"
        Friend Function GetUserUrlPart(ByVal User As UserData) As String
            Dim __user$ = User.Name.Split("_").FirstOrDefault
            __user &= $"/{User.Name.Replace($"{__user}_", String.Empty)}"
            Return __user
        End Function
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider) As String
            Return String.Format(UrlPatternUser, GetUserUrlPart(User))
        End Function
#End Region
#Region "IsMyUser, IsMyImageVideo"
        Private Const UserRegexDefault As String = "/(profiles|[\w]*?[-]{0,1}channels)/([^/]+)(\Z|.*?)"
        Private Const URD As String = ".*?{0}{1}"
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            If Not UserURL.IsEmptyString Then
                If Domains.Count > 0 Then
                    Dim uName$, uOpt$, fStr$
                    Dim uErr As New ErrorsDescriber(EDP.ReturnValue)
                    For i% = 0 To Domains.Count - 1
                        fStr = String.Format(URD, Domains(i), UserRegexDefault)
                        uName = RegexReplace(UserURL, RParams.DMS(fStr, 2, uErr))
                        If Not uName.IsEmptyString Then
                            uOpt = RegexReplace(UserURL, RParams.DMS(fStr, 1))
                            If Not uOpt.IsEmptyString Then Return New ExchangeOptions(Site, $"{uOpt}_{uName}")
                        End If
                    Next
                End If
            End If
            Return Nothing
        End Function
        Friend Overrides Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions
            If Not URL.IsEmptyString And Domains.Count > 0 Then
                If Domains.Domains.Exists(Function(d) URL.Contains(d)) Then Return New ExchangeOptions(Site, URL)
            End If
            Return Nothing
        End Function
#End Region
    End Class
End Namespace