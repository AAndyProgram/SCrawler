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
    <Manifest(XvideosSiteKey), SavedPosts, SpecialForm(True), SpecialForm(False), TaskGroup(SettingsCLS.TaskStackNamePornSite)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
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
        <PropertyOption(ControlText:="Download UHD", ControlToolTip:="Download UHD (4K) content"), PXML, PClonable>
        Friend Property DownloadUHD As PropertyValue
        <PropertyOption(ControlText:="Playlist of saved videos",
                        ControlToolTip:="Your personal videos playlist to download as 'saved posts'. " & vbCr &
                                        "This playlist must be private (Visibility = 'Only me'). It also required cookies." & vbCr &
                                        "This playlist must be entered by pattern: https://www.xvideos.com/favorite/01234567/playlistname.",
                        LeftOffset:=130), PXML, PClonable(Clone:=False)>
        Friend ReadOnly Property SavedVideosPlaylist As PropertyValue
#End Region
#Region "Initializer"
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New("XVIDEOS", "www.xvideos.com", AccName, Temp, My.Resources.SiteResources.XvideosIcon_48, My.Resources.SiteResources.XvideosPic_32)
            _Domains = New DomainsContainer(Me, "xvideos.com|xnxx.com")
            SiteDomains = New PropertyValue(Domains.DomainsDefault, GetType(String))
            Domains.DestinationProp = SiteDomains
            DownloadUHD = New PropertyValue(False)
            SavedVideosPlaylist = New PropertyValue(String.Empty, GetType(String))

            _SubscriptionsAllowed = True
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
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider) As String
            Return DirectCast(User, UserData).GetUserUrl(0)
        End Function
#End Region
#Region "IsMyUser, IsMyImageVideo"
        Private Const UserRegexDefault As String = "/(profiles|[\w]*?[-]{0,1}channels)/([^/]+)(\Z|.*?)"
        Private Const URD As String = ".*?{0}{1}"
        Private ReadOnly AbstractRegex As RParams = RParams.DM("[^/]+", 0, RegexReturn.List, EDP.ReturnValue)
        Private ReadOnly SearchRegex As RParams = RParams.DMS("\?k=([^&]+)&?((.*)(&p=\d+)|(.*))", 0, RegexReturn.ListByMatch, EDP.ReturnValue)
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            If Not UserURL.IsEmptyString Then
                UserURL = UserURL.ToLower
                If Domains.Count > 0 AndAlso Domains.Domains.Exists(Function(d) UserURL.Contains(d)) Then
                    Dim uName$, uOpt$, fStr$
                    Dim uErr As New ErrorsDescriber(EDP.ReturnValue)
                    For i% = 0 To Domains.Count - 1
                        fStr = String.Format(URD, Domains(i), UserRegexDefault)
                        uName = RegexReplace(UserURL, RParams.DMS(fStr, 2, uErr))
                        If Not uName.IsEmptyString Then
                            uOpt = RegexReplace(UserURL, RParams.DMS(fStr, 1))
                            If Not uOpt.IsEmptyString Then Return New ExchangeOptions(Site, $"{uOpt}@{uName}")
                        End If
                    Next

                    Dim absList As List(Of String) = RegexReplace(UserURL, AbstractRegex)
                    If absList.ListExists(3) AndAlso Not absList(2).IsEmptyString Then
                        If absList(2) = "c" Then
                            If absList.Count > 3 AndAlso Not absList.Last.IsEmptyString AndAlso IsNumeric(absList.Last) Then absList.RemoveAt(absList.Count - 1)
                            If absList.Count > 3 Then
                                uName = $"{CInt(SiteModes.Categories)}@{absList.Last}"
                                uOpt = $"{absList.Last}@"
                                absList.RemoveAt(absList.Count - 1)
                                If absList.Count > 3 Then uOpt &= absList.ListTake(2, absList.Count).ListToString("/")
                                Return New ExchangeOptions(Site, uName) With {.Options = uOpt}
                            End If
                        ElseIf absList(2) = "tags" And absList.Count >= 4 Then
                            If Not absList.Last.IsEmptyString AndAlso IsNumeric(absList.Last) Then absList.RemoveAt(absList.Count - 1)
                            If absList.Count > 3 Then
                                uOpt = String.Empty
                                uName = absList.Last
                                absList.RemoveAt(absList.Count - 1)
                                If absList.Count > 3 Then uOpt = absList.ListTake(2, 100, EDP.ReturnValue).ListToString("/").StringTrimStart("/").StringTrimEnd("/")
                                uOpt = $"{uName}@{uOpt}"
                                uName = $"{CInt(SiteModes.Tags)}@{uName.StringRemoveWinForbiddenSymbols}"
                                Return New ExchangeOptions(Site, uName) With {.Options = uOpt}
                            End If
                        ElseIf absList.Count = 3 And Not absList(2).IsEmptyString Then
                            absList = RegexReplace(absList(2), SearchRegex)
                            If absList.ListExists(6) AndAlso Not absList(1).IsEmptyString Then
                                uName = $"{CInt(SiteModes.Search)}@{absList(1).StringRemoveWinForbiddenSymbols}"
                                uOpt = $"{absList(1)}@{absList(3).IfNullOrEmpty(absList(5))}"
                                Return New ExchangeOptions(Site, uName) With {.Options = uOpt}
                            End If
                        End If
                    End If
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
#Region "UserOptions"
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse Not TypeOf Options Is UserExchangeOptions Then Options = New UserExchangeOptions
            If OpenForm Then
                Using f As New InternalSettingsForm(Options, Me, False) : f.ShowDialog() : End Using
            End If
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