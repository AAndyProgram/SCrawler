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
Imports DN = SCrawler.API.Base.DeclaredNames
Namespace API.Facebook
    <Manifest("AndyProgram_Facebook"), SavedPosts, SeparatedTasks(1), SpecialForm(False)>
    Friend Class SiteSettings : Inherits ThreadsNet.SiteSettings
#Region "Declarations"
#Region "Auth"
        <PropertyOption(AllowNull:=False, ControlText:="Accept", ControlToolTip:="Header 'Accept'", IsAuth:=True), ControlNumber(21), PXML, PClonable>
        Friend ReadOnly Property Header_Accept As PropertyValue
        <PropertyOption(ControlText:="x-ig-app-id", AllowNull:=True, IsAuth:=True), HiddenControl>
        Friend Overrides ReadOnly Property HH_IG_APP_ID As PropertyValue
            Get
                Return __HH_IG_APP_ID
            End Get
        End Property
        <DoNotUse> Friend Overrides ReadOnly Property HH_CSRF_TOKEN As PropertyValue
            Get
                Return __HH_CSRF_TOKEN
            End Get
        End Property
#End Region
#Region "Defaults"
        <PropertyOption(ControlText:="Download photos", IsAuth:=False, Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property ParsePhotoBlock As PropertyValue
        <PropertyOption(ControlText:="Download videos", IsAuth:=False, Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property ParseVideoBlock As PropertyValue
        <PropertyOption(ControlText:="Download reels", IsAuth:=False, Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property ParseReelsBlock As PropertyValue
        <PropertyOption(ControlText:="Download stories", IsAuth:=False, Category:=DN.CAT_UserDefs), PXML, PClonable>
        Friend ReadOnly Property ParseStoriesBlock As PropertyValue
#End Region
#End Region
#Region "Initializer"
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New("Facebook", "facebook.com", AccName, Temp, My.Resources.SiteResources.FacebookIcon_32, My.Resources.SiteResources.FacebookPic_37)

            With Responser.Headers
                .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.Authority, "www.facebook.com"))
                .Add(HttpHeaderCollection.GetSpecialHeader(MyHeaderTypes.Origin, "https://www.facebook.com"))
                .Remove(Instagram.UserData.GQL_HEADER_FB_FRINDLY_NAME)
            End With
            Header_Accept = New PropertyValue(String.Empty, GetType(String))
            ParsePhotoBlock = New PropertyValue(True)
            ParseVideoBlock = New PropertyValue(True)
            ParseReelsBlock = New PropertyValue(False)
            ParseStoriesBlock = New PropertyValue(True)

            UrlPatternUser = "https://www.facebook.com/{0}"
            UserRegex = RParams.DMS("facebook.com/(profile.php\?id=\d+|[^\?&/]+)", 1)
            ImageVideoContains = "facebook.com"
            UserOptionsType = GetType(UserExchangeOptions)
        End Sub
#End Region
#Region "GetInstance"
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
#End Region
#Region "UpdateResponserData"
        Friend Overrides Sub UpdateResponserData(ByVal Resp As Responser)
            With Responser.Cookies
                .Update(Resp.Cookies)
                If .Changed Then Responser.SaveCookies() : .Changed = False
            End With
        End Sub
#End Region
#Region "BaseAuthExists, GetUserUrl, GetUserPostUrl, IsMyUser, IsMyImageVideo"
        Friend Overrides Function BaseAuthExists() As Boolean
            Return Responser.CookiesExists And CBool(DownloadData_Impl.Value) 'And ACheck(HH_IG_APP_ID.Value)
        End Function
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider) As String
            Return DirectCast(User, UserData).GetProfileUrl
        End Function
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            Return Media.URL_BASE
        End Function
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            Dim e As ExchangeOptions = MyBase.IsMyUser(UserURL)
            If e.Exists Then
                e.Options = e.UserName
                Dim v$ = RegexReplace(e.UserName, Regex_ProfileUrlID)
                If Not v.IsEmptyString Then
                    e.UserName = v
                Else
                    e.UserName = e.UserName.StringRemoveWinForbiddenSymbols
                End If
            End If
            Return e
        End Function
        Friend Overrides Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions
            If Not URL.IsEmptyString AndAlso Not CStr(AConvert(Of String)(URL, Regex_VideoIDFromURL, String.Empty)).IsEmptyString Then
                Return New ExchangeOptions(Site, String.Empty) With {.Exists = True}
            Else
                Return Nothing
            End If
        End Function
#End Region
    End Class
End Namespace