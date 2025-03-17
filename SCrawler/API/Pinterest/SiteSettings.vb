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
Namespace API.Pinterest
    <Manifest("AndyProgram_Pinterest"), SavedPosts, SeparatedTasks, SpecialForm(False)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
        <PropertyOption(ControlText:=DeclaredNames.ConcurrentDownloadsCaption,
                        ControlToolTip:=DeclaredNames.ConcurrentDownloadsToolTip, AllowNull:=False, LeftOffset:=120), PXML, TaskCounter, PClonable>
        Friend ReadOnly Property ConcurrentDownloads As PropertyValue
        <Provider(NameOf(ConcurrentDownloads), FieldsChecker:=True)>
        Private ReadOnly Property MyConcurrentDownloadsProvider As IFormatProvider
        <PropertyOption(ControlText:=DeclaredNames.SavedPostsUserNameCaption, ControlToolTip:=DeclaredNames.SavedPostsUserNameToolTip), PXML, PClonable(Clone:=False)>
        Friend ReadOnly Property SavedPostsUserName As PropertyValue
#End Region
#Region "Initializer"
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New("Pinterest", "pinterest.com", AccName, Temp, My.Resources.SiteResources.PinterestIcon_32, My.Resources.SiteResources.PinterestPic_48)
            SavedPostsUserName = New PropertyValue(String.Empty, GetType(String))
            ConcurrentDownloads = New PropertyValue(1)
            MyConcurrentDownloadsProvider = New ConcurrentDownloadsProvider
            CheckNetscapeCookiesOnEndInit = True
            UseNetscapeCookies = True
            UserRegex = RParams.DMS("https?://w{0,3}.?[^/]*?.?pinterest.com/([^/]+)/?(?(_)|([^/]*))/?([^/\?]*)", 0, RegexReturn.ListByMatch, EDP.ReturnValue)
            UserOptionsType = GetType(EditorExchangeOptions)
        End Sub
#End Region
#Region "GetInstance, Available"
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            Return Settings.GalleryDLFile.Exists And (Not What = ISiteSettings.Download.SavedPosts OrElse ACheck(SavedPostsUserName.Value))
        End Function
#End Region
#Region "IsMyUser, IsMyImageVideo, GetUserUrl, GetUserPostUrl, UserOptions"
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            If Not UserURL.IsEmptyString Then
                Dim l As List(Of String) = RegexReplace(UserURL, UserRegex)
                If l.ListExists(3) Then
                    Dim n$ = l(1)
                    If Not l(2).IsEmptyString Then n &= $"@{l(2)}"
                    If l.Count > 3 AndAlso Not l(3).IsEmptyString Then n &= $"@{l(3)}"
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