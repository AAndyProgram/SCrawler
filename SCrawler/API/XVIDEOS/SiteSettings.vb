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
Namespace API.XVIDEOS
    <Manifest(XvideosSiteKey), SavedPosts, SpecialForm(True), TaskGroup(SettingsCLS.TaskStackNamePornSite)>
    Friend Class SiteSettings : Inherits SiteSettingsBase : Implements IDomainContainer
#Region "Declarations"
        Friend Overrides ReadOnly Property Icon As Icon Implements IDomainContainer.Icon
            Get
                Return My.Resources.SiteResources.XvideosIcon_48
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.SiteResources.XvideosPic_32
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
        Private ReadOnly Property DomainsDefault As String = "xvideos.com|xnxx.com" Implements IDomainContainer.DomainsDefault
#End Region
        <PropertyOption(ControlText:="Download UHD", ControlToolTip:="Download UHD (4K) content"), PXML>
        Friend Property DownloadUHD As PropertyValue
        Private Property Initialized As Boolean = False Implements IDomainContainer.Initialized
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
            Responser.DeclaredError = EDP.ThrowException
            Domains = New List(Of String)
            DomainsTemp = New List(Of String)
            SiteDomains = New PropertyValue(DomainsDefault, GetType(String), Sub(s) UpdateDomains())
            DownloadUHD = New PropertyValue(False)
            SavedVideosPlaylist = New PropertyValue(String.Empty, GetType(String))
        End Sub
        Friend Overrides Sub EndInit()
            Initialized = True
            DomainContainer.EndInit(Me)
            DomainsTemp.ListAddList(Domains)
        End Sub
#End Region
#Region "Edit"
        Private Property DomainsUpdateInProgress As Boolean = False Implements IDomainContainer.DomainsUpdateInProgress
        Private Property DomainsUpdatedBySite As Boolean = False Implements IDomainContainer.DomainsUpdatedBySite
        Friend Sub UpdateDomains() Implements IDomainContainer.UpdateDomains
            DomainContainer.UpdateDomains(Me)
        End Sub
        Friend Overrides Sub Update()
            DomainContainer.Update(Me)
            Responser.SaveSettings()
        End Sub
        Friend Overrides Sub EndEdit()
            DomainContainer.EndEdit(Me)
            MyBase.EndEdit()
        End Sub
        Friend Overrides Sub OpenSettingsForm()
            DomainContainer.OpenSettingsForm(Me)
        End Sub
#End Region
#Region "Download"
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            If What = ISiteSettings.Download.SavedPosts Then
                Return New UserData With {.IsSavedPosts = True, .User = New UserInfo With {.Name = "XVIDEOS"}}
            Else
                Return New UserData
            End If
        End Function
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            If Settings.UseM3U8 Then
                If What = ISiteSettings.Download.SavedPosts Then
                    Return ACheck(SavedVideosPlaylist.Value) And If(Responser.Cookies?.Count, 0) > 0
                Else
                    Return True
                End If
            Else
                Return False
            End If
        End Function
#End Region
#Region "User: get, check"
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider, ByVal Channel As Boolean) As String
            Dim __user$ = User.Name.Split("_").FirstOrDefault
            __user &= $"/{User.Name.Replace($"{User}_", String.Empty)}"
            Return __user
        End Function
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
#End Region
#Region "Get special data"
        Friend Overrides Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions
            If Not URL.IsEmptyString And Domains.Count > 0 Then
                If Domains.Exists(Function(d) URL.Contains(d)) Then Return New ExchangeOptions With {.UserName = URL, .Exists = True}
            End If
            Return Nothing
        End Function
        Friend Overrides Function GetSpecialData(ByVal URL As String, ByVal Path As String, ByVal AskForPath As Boolean) As IEnumerable
            If Not URL.IsEmptyString And Settings.UseM3U8 Then
                Dim spf$ = String.Empty
                Dim f As SFile = GetSpecialDataFile(Path, AskForPath, spf)
                f.Name = "video"
                f.Extension = "mp4"
                Using resp As Responser = Responser.Copy
                    Using user As New UserData With {.HOST = Settings(XvideosSiteKey)}
                        DirectCast(user, UserDataBase).User.File = f
                        Dim p As UserMedia = user.Download(URL, resp, DownloadUHD.Value, String.Empty)
                        If p.State = UserMedia.States.Downloaded Then p.SpecialFolder = spf : Return {p}
                    End Using
                End Using
            End If
            Return Nothing
        End Function
#End Region
    End Class
End Namespace