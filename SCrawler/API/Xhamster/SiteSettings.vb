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
    <Manifest(XhamsterSiteKey), SavedPosts, SpecialForm(True), SpecialForm(False), TaskGroup(SettingsCLS.TaskStackNamePornSite)>
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
        Friend ReadOnly Property DownloadUHD As PropertyValue
        <PropertyOption(ControlText:="Re-encode downloaded videos if necessary",
                        ControlToolTip:="If enabled and the video is downloaded in a non-native format, the video will be re-encoded." & vbCr &
                                        "Attention! Enabling this setting results in maximum CPU usage."), PXML, PClonable>
        Friend ReadOnly Property ReencodeVideos As PropertyValue
#End Region
#Region "Initializer"
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New("XHamster", "xhamster.com", AccName, Temp, My.Resources.SiteResources.XhamsterIcon_32, My.Resources.SiteResources.XhamsterPic_32)

            _Domains = New DomainsContainer(Me, "xhamster.com")
            SiteDomains = New PropertyValue(Domains.DomainsDefault, GetType(String))
            Domains.DestinationProp = SiteDomains
            DownloadUHD = New PropertyValue(False)
            ReencodeVideos = New PropertyValue(False)

            _SubscriptionsAllowed = True
            UrlPatternUser = "https://xhamster.com/{0}/{1}"
            UserRegex = RParams.DMS($"/({UserOption}|{ChannelOption}|{P_Creators})/([^/]+)(\Z|.*)", 0, RegexReturn.ListByMatch)
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
            With DirectCast(User, UserData)
                If Not .SiteMode = SiteModes.User Then
                    Return .GetNonUserUrl(0)
                Else
                    Return String.Format(UrlPatternUser, IIf(.IsChannel, ChannelOption, UserOption), .NameTrue)
                End If
            End With
        End Function
#Region "IsMyUser, IsMyImageVideo"
        Friend Const ChannelOption As String = "channels"
        Private Const UserOption As String = "users"
        Friend Const P_Search As String = "search"
        Friend Const P_Tags As String = "tags"
        Friend Const P_Categories As String = "categories"
        Friend Const P_Pornstars As String = "pornstars"
        Friend Const P_Creators As String = "creators"
        Private ReadOnly NonUsersRegex As RParams = RParams.DM("https?://[^/]+/((gay)/|(shemale)/|)(pornstars|creators|tags|categories|search)/([^/\?]+)[/\?]?(.*)", 0,
                                                               RegexReturn.ListByMatch, EDP.ReturnValue)
        Private ReadOnly PageRemover_1 As RParams = RParams.DM("[\?&]?[Pp]age=\d+", 0, RegexReturn.Replace, EDP.ReturnValue,
                                                               CType(Function(input) String.Empty, Func(Of String, String)))
        Private ReadOnly PageRemover_2 As RParams = RParams.DM("/\d+\?", 0, RegexReturn.Replace, EDP.ReturnValue,
                                                               CType(Function(input) "?", Func(Of String, String)))
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            If Not UserURL.IsEmptyString AndAlso Domains.Domains.Count > 0 AndAlso Domains.Domains.Exists(Function(d) UserURL.ToLower.Contains(d.ToLower)) Then
                Dim n$, opt$
                Dim tryNext As Boolean = False
                Dim data As List(Of String) = RegexReplace(UserURL, UserRegex)
                If data.ListExists(3) AndAlso Not data(2).IsEmptyString Then
                    n = data(2)
                    If Not data(1).IsEmptyString Then
                        If data(1) = ChannelOption Then
                            n &= $"@{data(1)}"
                        ElseIf data(1) = P_Creators Then
                            tryNext = True
                        End If
                    End If
                    If Not tryNext Then Return New ExchangeOptions(Site, n)
                Else
                    tryNext = True
                End If

                If tryNext Then
                    data = RegexReplace(UserURL, NonUsersRegex)
                    If data.ListExists(7) AndAlso Not data(5).IsEmptyString Then
                        n = data(5).StringRemoveWinForbiddenSymbols
                        If Not n.IsEmptyString And Not data(4).IsEmptyString Then
                            Dim mode As SiteModes
                            Select Case data(4)
                                Case P_Search : mode = SiteModes.Search
                                Case P_Tags : mode = SiteModes.Tags
                                Case P_Categories : mode = SiteModes.Categories
                                Case P_Pornstars : mode = SiteModes.Pornstars
                                Case P_Creators : mode = SiteModes.User
                                Case Else : Return Nothing
                            End Select
                            n = $"{CInt(mode)}@{n}"
                            Dim tmpOpt$ = data(6)

                            If Not tmpOpt.IsEmptyString Then
                                tmpOpt = RegexReplace(tmpOpt, PageRemover_1)
                                tmpOpt = RegexReplace(tmpOpt, PageRemover_2)
                            End If
                            'mode@gay@tags@arguments@query
                            opt = $"{CInt(mode)}@{data(2)}@{data(4)}@{tmpOpt}@{data(5)}"
                            Return New ExchangeOptions(Site, n) With {.Options = opt}
                        End If
                    End If
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