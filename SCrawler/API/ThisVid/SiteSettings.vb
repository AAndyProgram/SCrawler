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
Imports PersonalUtilities.Tools.Web.Cookies
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.ThisVid
    <Manifest(ThisVidSiteKey), SeparatedTasks(1), SpecialForm(False), SavedPosts>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
        <PXML, PropertyOption(ControlText:="Public videos", ControlToolTip:="Download public videos"), PClonable>
        Friend ReadOnly Property DownloadPublic As PropertyValue
        <PXML, PropertyOption(ControlText:="Private videos", ControlToolTip:="Download private videos"), PClonable>
        Friend ReadOnly Property DownloadPrivate As PropertyValue
        <PXML, PropertyOption(ControlText:="Favourite videos", ControlToolTip:="Download favourite videos"), PClonable>
        Friend ReadOnly Property DownloadFavourite As PropertyValue
        <PXML, PropertyOption(ControlText:="Different folders",
                              ControlToolTip:="Use different folders to store video files." & vbCr &
                                              "If true, then public videos will be stored in the 'Public' folder, private - in the 'Private' folder." & vbCr &
                                              "If false, all videos will be stored in the 'Video' folder."), PClonable>
        Friend ReadOnly Property DifferentFolders As PropertyValue
#End Region
#Region "Initializer"
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New("ThisVid", "thisvid.com", AccName, Temp, My.Resources.SiteResources.ThisVidIcon_16, My.Resources.SiteResources.ThisVidPic_16)
            With Responser
                .CookiesExtractMode = Responser.CookiesExtractModes.Any
                .CookiesUpdateMode = CookieKeeper.UpdateModes.ReplaceByNameAll
                .CookiesExtractedAutoSave = False
            End With
            DownloadPublic = New PropertyValue(True)
            DownloadPrivate = New PropertyValue(True)
            DownloadFavourite = New PropertyValue(False)
            DifferentFolders = New PropertyValue(True)
            _SubscriptionsAllowed = True
            CheckNetscapeCookiesOnEndInit = True
            UseNetscapeCookies = True
            UserRegex = RParams.DMS("thisvid.com/members/(\d+)", 1)
            UrlPatternUser = "https://thisvid.com/members/{0}/"
            ImageVideoContains = "https://thisvid.com/videos/"
        End Sub
#End Region
#Region "GetInstance"
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
#End Region
#Region "UpdateCookies"
        Friend Sub UpdateCookies(ByVal Source As Responser)
            Responser.Cookies.Update(Source.Cookies)
            Update_SaveCookiesNetscape(True)
        End Sub
#End Region
#Region "Downloading"
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            Return Settings.YtdlpFile.Exists And (What = ISiteSettings.Download.SingleObject Or Responser.CookiesExists)
        End Function
        Friend Overrides Sub BeforeStartDownload(ByVal User As Object, ByVal What As ISiteSettings.Download)
            If CookiesNetscapeFile.Exists Then
                With Responser.Cookies
                    .Clear()
                    .AddRange(CookieKeeper.ParseNetscapeText(CookiesNetscapeFile.GetText, EDP.ReturnValue),, EDP.ReturnValue)
                End With
            End If
            MyBase.BeforeStartDownload(User, What)
        End Sub
#End Region
#Region "GetUserUrl, IsMyUser, UserOptions"
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider) As String
            With DirectCast(User, UserData)
                If .IsUser Then
                    Return MyBase.GetUserUrl(User)
                Else
                    Return .GetNonUserUrl(0)
                End If
            End With
        End Function
        Private ReadOnly AbstractExtractor As RParams = RParams.DM("[^/]+", 0, RegexReturn.List, EDP.ReturnValue)
        Private Const P_Albums As String = "albums"
        Friend Const P_Tags As String = "tags"
        Friend Const P_Categories As String = "categories"
        Friend Const P_Search As String = "search"
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            If Not UserURL.IsEmptyString AndAlso UserURL.ToLower.Contains("thisvid.com") Then
                Dim user$ = RegexReplace(UserURL, UserRegex)
                If Not user.IsEmptyString Then
                    Return New ExchangeOptions(Site, user)
                Else
                    Dim data As List(Of String) = RegexReplace(UserURL.ToLower, AbstractExtractor)
                    If data.ListExists Then
                        If data.Count >= 3 AndAlso Not data(2).IsEmptyString Then
                            Dim mode As SiteModes
                            Dim n$ = String.Empty, opt$ = String.Empty
                            Dim __data As Func(Of Integer, String) = Function(i) If(data.Count - 1 >= i, data(i), String.Empty)

                            Select Case data(2)
                                Case P_Albums
                                Case P_Tags
                                    mode = SiteModes.Tags
                                    If Not __data(3).IsEmptyString Then
                                        n = __data(3)
                                        If Not __data(4).IsEmptyString AndAlso Not IsNumeric(__data(4)) Then opt = __data(4)
                                    End If
                                Case P_Categories
                                    mode = SiteModes.Categories
                                    If Not __data(3).IsEmptyString Then
                                        n = __data(3)
                                        If Not __data(4).IsEmptyString AndAlso Not IsNumeric(__data(4)) Then opt = __data(4)
                                    End If
                                Case Else
                                    mode = SiteModes.Search
                                    If Not __data(3).IsEmptyString AndAlso Not IsNumeric(__data(3)) Then n = __data(3)
                                    If n.IsEmptyString AndAlso Not __data(4).IsEmptyString AndAlso Not IsNumeric(__data(4)) Then n = __data(4)
                                    If Not n.IsEmptyString Then n = n.TrimStart("?", "q", "=")
                                    If Not n.IsEmptyString Then
                                        If __data(2).IsEmptyString Then
                                            n = String.Empty
                                        Else
                                            opt = __data(2)
                                        End If
                                    End If
                            End Select

                            opt = $"{n}@{opt}"
                            n = n.StringRemoveWinForbiddenSymbols
                            If Not n.IsEmptyString Then
                                n = $"{CInt(mode)}@{n}"
                                Return New ExchangeOptions(Site, n) With {.Options = opt}
                            End If
                        End If
                    End If
                End If
            End If
            Return Nothing
        End Function
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse Not TypeOf Options Is UserExchangeOptions Then Options = New UserExchangeOptions(Me)
            If OpenForm Then
                Using f As New InternalSettingsForm(Options, Me, False) : f.ShowDialog() : End Using
            End If
        End Sub
#End Region
    End Class
End Namespace