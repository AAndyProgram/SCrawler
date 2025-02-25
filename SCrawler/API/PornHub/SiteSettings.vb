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
Imports PersonalUtilities.Tools.Web.Clients
Namespace API.PornHub
    <Manifest("AndyProgram_PornHub"), SavedPosts, SpecialForm(False), SeparatedTasks(1)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
        <PropertyOption(ControlText:="Download UHD", ControlToolTip:="Download UHD (4K) content"), PXML, PClonable>
        Friend Property DownloadUHD As PropertyValue
        <PropertyOption(ControlText:="Download uploaded", ControlToolTip:="Download uploaded videos"), PXML, PClonable>
        Friend Property DownloadUploaded As PropertyValue
        <PropertyOption(ControlText:="Download tagged", ControlToolTip:="Download tagged videos"), PXML, PClonable>
        Friend Property DownloadTagged As PropertyValue
        <PropertyOption(ControlText:="Download private", ControlToolTip:="Download private videos"), PXML, PClonable>
        Friend Property DownloadPrivate As PropertyValue
        <PropertyOption(ControlText:="Download favorite", ControlToolTip:="Download favorite videos"), PXML, PClonable>
        Friend Property DownloadFavorite As PropertyValue
        <PropertyOption(ControlText:="Download GIF", ControlToolTip:="Default for new users", ThreeStates:=True), PXML, PClonable>
        Friend ReadOnly Property DownloadGifs As PropertyValue
        <PropertyOption(ControlText:="Download GIFs as mp4", ControlToolTip:="Download gifs in 'mp4' format instead of native 'webm'"), PXML, PClonable>
        Friend ReadOnly Property DownloadGifsAsMp4 As PropertyValue
        <PropertyOption(ControlText:=DeclaredNames.SavedPostsUserNameCaption, ControlToolTip:=DeclaredNames.SavedPostsUserNameToolTip), PXML, PClonable(Clone:=False)>
        Friend ReadOnly Property SavedPostsUserName As PropertyValue
#End Region
#Region "Initializer"
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New("PornHub", "pornhub.com", AccName, Temp, My.Resources.SiteResources.PornHubIcon_16, My.Resources.SiteResources.PornHubPic_16)
            With Responser : .CurlSslNoRevoke = True : .CurlInsecure = True : End With

            DownloadUHD = New PropertyValue(False)
            DownloadUploaded = New PropertyValue(True)
            DownloadTagged = New PropertyValue(False)
            DownloadPrivate = New PropertyValue(False)
            DownloadFavorite = New PropertyValue(False)
            DownloadGifsAsMp4 = New PropertyValue(True)
            DownloadGifs = New PropertyValue(CInt(CheckState.Indeterminate), GetType(Integer))
            SavedPostsUserName = New PropertyValue(String.Empty, GetType(String))

            _SubscriptionsAllowed = True
            UrlPatternUser = "https://www.pornhub.com/{0}/{1}"
            UserRegex = RParams.DMS("pornhub.com/(model|user[s]?|pornstar|channel[s]?)/([^/]+).*?", 0, RegexReturn.ListByMatch)
            ImageVideoContains = "pornhub.com"
        End Sub
#End Region
#Region "GetInstance"
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
#End Region
#Region "Downloading"
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            Responser.CurlPath = Settings.CurlFile
            Return Settings.UseM3U8 And Settings.CurlFile.Exists And
                   (Not What = ISiteSettings.Download.SavedPosts OrElse (ACheck(SavedPostsUserName.Value) And Responser.CookiesExists))
        End Function
#End Region
#Region "IsMyUser"
        Private ReadOnly NonUserRegex As RParams = RParams.DM("(?<=pornhub.com/)((.+?)(?=[\?&]{1}page=\d+)|(.+))", 0, EDP.ReturnValue)
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            Try
                If Not UserURL.IsEmptyString AndAlso UserURL.ToLower.Contains("pornhub.com") Then
                    Dim alist As List(Of String) = RegexReplace(UserURL.ToLower, UserRegex)
                    If alist.ListExists(3) Then
                        Return New ExchangeOptions(Site, $"{alist(1)}_{alist(2)}")
                    Else
                        Dim opt$ = RegexReplace(UserURL, NonUserRegex)
                        If Not opt.IsEmptyString Then Return New ExchangeOptions(Site, opt.StringRemoveWinForbiddenSymbols) With {.Options = opt}
                    End If
                End If
                Return Nothing
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, $"[API.PornHub.SiteSettings.IsMyUser({UserURL})]", New ExchangeOptions)
            End Try
        End Function
#End Region
#Region "GetUserUrl"
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider) As String
            With DirectCast(User, UserData)
                If .IsUser Then
                    Return String.Format(UrlPatternUser, .PersonType, .NameTrue)
                Else
                    Return .GetNonUserUrl(0)
                End If
            End With
        End Function
#End Region
#Region "User options"
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse Not TypeOf Options Is UserExchangeOptions Then Options = New UserExchangeOptions(Me)
            If OpenForm Then
                Using f As New InternalSettingsForm(Options, Me, False) : f.ShowDialog() : End Using
            End If
        End Sub
#End Region
    End Class
End Namespace