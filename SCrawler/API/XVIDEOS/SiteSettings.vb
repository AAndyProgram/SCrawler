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
Namespace API.XVIDEOS
    <Manifest(XvideosSiteKey), SpecialForm(True)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Images"
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
#End Region
#Region "Declarations"
        <PXML("Domains")> Private Property SiteDomains As PropertyValue
        <PropertyOption(ControlText:="Download UHD", ControlToolTip:="Download UHD (4K) content"), PXML>
        Public Property DownloadUHD As PropertyValue
        Friend ReadOnly Property Domains As List(Of String)
        Private Const DomainsDefault As String = "xvideos.com|xnxx.com"
        Private _Initialized As Boolean = False
#End Region
#Region "Initializer"
        Friend Sub New()
            MyBase.New("XVIDEOS", "www.xvideos.com")
            Domains = New List(Of String)
            SiteDomains = New PropertyValue(DomainsDefault, GetType(String), Sub(s) UpdateDomains())
            DownloadUHD = New PropertyValue(False)
        End Sub
        Friend Overrides Sub EndInit()
            _Initialized = True
            UpdateDomains()
        End Sub
#End Region
#Region "Update"
        Private _DomainsUpdateInProgress As Boolean = False
        Friend Sub UpdateDomains()
            If Not _Initialized Then Exit Sub
            If Not _DomainsUpdateInProgress Then
                _DomainsUpdateInProgress = True
                If Not ACheck(SiteDomains.Value) Then SiteDomains.Value = DomainsDefault
                Domains.ListAddList(CStr(SiteDomains.Value).Split("|"), LAP.NotContainsOnly, LAP.ClearBeforeAdd)
                Domains.ListAddList(DomainsDefault.Split("|"), LAP.NotContainsOnly)
                SiteDomains.Value = Domains.ListToString("|")
                _DomainsUpdateInProgress = False
            End If
        End Sub
        Friend Overrides Sub Update()
            UpdateDomains()
            Responser.SaveSettings()
        End Sub
#End Region
#Region "Download"
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            Return Settings.UseM3U8
        End Function
#End Region
#Region "User: get, check"
        Friend Overrides Function GetUserUrl(ByVal UserName As String, ByVal Channel As Boolean) As String
            Dim user$ = UserName.Split("_").FirstOrDefault
            user &= $"/{UserName.Replace($"{user}_", String.Empty)}"
            Return user
        End Function
        Private Const UserRegexDefault As String = "/(profiles|[\w]*?[-]{0,1}channels)/([^/]+)(\Z|.*?)"
        Private Const URD As String = ".*?{0}{1}"
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            If Not UserURL.IsEmptyString Then
                If Domains.Count > 0 Then
                    Dim uName$, uOpt$, fStr$
                    For i% = 0 To Domains.Count - 1
                        fStr = String.Format(URD, Domains(i), UserRegexDefault)
                        uName = RegexReplace(UserURL, RParams.DMS(fStr, 2))
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
#Region "Settings"
        Friend Overrides Sub OpenSettingsForm()
            Using f As New SettingsForm(Me) : f.ShowDialog() : End Using
        End Sub
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
                Using resp As Response = Responser.Copy
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