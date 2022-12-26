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
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return My.Resources.SiteResources.PornHubIcon_16
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.SiteResources.PornHubPic_16
            End Get
        End Property
        Private ReadOnly Property CurlPathExists As Boolean
        <PropertyOption(ControlText:="Download GIF", ControlToolTip:="Default for new users", ThreeStates:=True), PXML>
        Friend ReadOnly Property DownloadGifs As PropertyValue
        <PropertyOption(ControlText:="Download GIFs as mp4", ControlToolTip:="Download gifs in 'mp4' format instead of native 'webm'"), PXML>
        Friend ReadOnly Property DownloadGifsAsMp4 As PropertyValue
        <PropertyOption(ControlText:="Photo ModelHub only",
                        ControlToolTip:="Download photo only from ModelHub. Prornstar photos hosted on PornHub itself will not be downloaded." & vbCr &
                                        "Attention! Downloading photos hosted on PornHub is a very heavy job."), PXML>
        Friend ReadOnly Property DownloadPhotoOnlyFromModelHub As PropertyValue
        <PropertyOption(ControlText:="Saved posts user", ControlToolTip:="Personal profile username"), PXML>
        Friend ReadOnly Property SavedPostsUserName As PropertyValue
#End Region
#Region "Initializer"
        Friend Sub New()
            MyBase.New("PornHub", "pornhub.com")
            Responser.CurlPath = $"cURL\curl.exe"
            CurlPathExists = Responser.CurlPath.Exists
            Responser.DeclaredError = EDP.ThrowException

            DownloadGifsAsMp4 = New PropertyValue(True)
            DownloadGifs = New PropertyValue(CInt(CheckState.Indeterminate), GetType(Integer))
            DownloadPhotoOnlyFromModelHub = New PropertyValue(True)
            SavedPostsUserName = New PropertyValue(String.Empty, GetType(String))

            UrlPatternUser = "https://www.pornhub.com/{0}/{1}"
            UserRegex = RParams.DMS("pornhub.com/([^/]+)/([^/]+).*?", 0, RegexReturn.ListByMatch)
            ImageVideoContains = "pornhub"
        End Sub
#End Region
#Region "GetInstance, GetSpecialData"
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            If What = ISiteSettings.Download.SavedPosts Then
                Return New UserData With {
                    .IsSavedPosts = True,
                    .VideoPageModel = UserData.VideoPageModels.Favorite,
                    .PersonType = UserData.PersonTypeUser,
                    .User = New UserInfo With {.Name = $"{UserData.PersonTypeUser}_{CStr(AConvert(Of String)(SavedPostsUserName.Value, String.Empty))}"}
                }
            Else
                Return New UserData
            End If
        End Function
        Friend Overrides Function GetSpecialData(ByVal URL As String, ByVal Path As String, ByVal AskForPath As Boolean) As IEnumerable
            If Available(ISiteSettings.Download.Main, True) Then
                Using resp As Responser = Responser.Copy
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
#End Region
#Region "Downloading"
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            Return Settings.UseM3U8 And CurlPathExists And (Not What = ISiteSettings.Download.SavedPosts OrElse ACheck(SavedPostsUserName.Value))
        End Function
#End Region
#Region "IsMyUser"
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            Try
                If Not UserURL.IsEmptyString Then
                    Dim alist As List(Of String) = RegexReplace(UserURL.ToLower, UserRegex)
                    If alist.ListExists(3) Then Return New ExchangeOptions(Site, $"{alist(1)}_{alist(2)}")
                End If
                Return Nothing
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendInLog + EDP.ReturnValue, ex, $"[API.PornHub.SiteSettings.IsMyUser({UserURL})]", New ExchangeOptions)
            End Try
        End Function
#End Region
#Region "GetUserUrl, GetUserPostUrl"
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider, ByVal Channel As Boolean) As String
            With DirectCast(User, UserData) : Return String.Format(UrlPatternUser, .PersonType, .NameTrue) : End With
        End Function
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            Return Media.URL_BASE
        End Function
#End Region
#Region "User options"
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse Not TypeOf Options Is UserExchangeOptions Then Options = New UserExchangeOptions(Me)
            If OpenForm Then
                Using f As New OptionsForm(Options) : f.ShowDialog() : End Using
            End If
        End Sub
#End Region
    End Class
End Namespace