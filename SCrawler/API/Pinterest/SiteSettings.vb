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
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Pinterest
    <Manifest("AndyProgram_Pinterest"), SavedPosts, SeparatedTasks>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return My.Resources.SiteResources.PinterestIcon_32
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.SiteResources.PinterestPic_48
            End Get
        End Property
        Private Class ConcurrentDownloadsValidator : Inherits FieldsCheckerProviderBase
            Public Overrides Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                              Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object
                Dim v% = AConvert(Of Integer)(Value, -1)
                Dim defV% = Settings.MaxUsersJobsCount
                If v.ValueBetween(1, defV) Then
                    Return Value
                Else
                    ErrorMessage = $"The number of concurrent downloads must be greater than 0 and equal to or less than {defV} (global limit)."
                    HasError = True
                    Return Nothing
                End If
            End Function
        End Class
        <Provider(NameOf(ConcurrentDownloads), FieldsChecker:=True)>
        Private ReadOnly Property ConcurrentDownloadsProvider As IFormatProvider
        <PXML, PropertyOption(ControlText:="Concurrent downloads", ControlToolTip:="The number of concurrent downloads.", LeftOffset:=120), TaskCounter>
        Friend ReadOnly Property ConcurrentDownloads As PropertyValue
        <PXML, PropertyOption(ControlText:="Saved posts user", ControlToolTip:="Personal profile username")>
        Friend ReadOnly Property SavedPostsUserName As PropertyValue
#End Region
#Region "Initializer"
        Friend Sub New()
            MyBase.New("Pinterest", "pinterest.com")
            SavedPostsUserName = New PropertyValue(String.Empty, GetType(String))
            ConcurrentDownloads = New PropertyValue(1)
            ConcurrentDownloadsProvider = New ConcurrentDownloadsValidator
            CheckNetscapeCookiesOnEndInit = True
            UseNetscapeCookies = True
            UserRegex = RParams.DMS("https?://w{0,3}.?[^/]*?.?pinterest.com/([^/]+)/?(?(_)|([^/]*))", 0, RegexReturn.ListByMatch, EDP.ReturnValue)
        End Sub
#End Region
#Region "GetInstance, Available"
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            Return Settings.GalleryDLFile.Exists And (Not What = ISiteSettings.Download.SavedPosts OrElse
                                                     (Responser.CookiesExists And ACheck(SavedPostsUserName.Value)))
        End Function
#End Region
#Region "IsMyUser, IsMyImageVideo, GetUserUrl, GetUserPostUrl"
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            If Not UserURL.IsEmptyString Then
                Dim l As List(Of String) = RegexReplace(UserURL, UserRegex)
                If l.ListExists(3) Then
                    Dim n$ = l(1)
                    If Not l(2).IsEmptyString Then n &= $"@{l(2)}"
                    Return New ExchangeOptions(Site, n) With {.Exists = True}
                End If
            End If
            Return Nothing
        End Function
        Friend Overrides Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions
            Return IsMyUser(URL)
        End Function
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider) As String
            With DirectCast(User, UserData)
                Dim n$ = .TrueUserName
                Dim c$ = .TrueBoardName
                If Not c.IsEmptyString Then c &= "/"
                Return $"https://www.pinterest.com/{n}/{c}"
            End With
        End Function
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            If Not Media.Post.ID.IsEmptyString Then
                Return $"https://www.pinterest.com/pin/{Media.Post.ID}/"
            Else
                Return String.Empty
            End If
        End Function
#End Region
    End Class
End Namespace