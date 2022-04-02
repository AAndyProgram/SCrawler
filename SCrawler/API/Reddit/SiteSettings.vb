' Copyright (C) 2022  Andy
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
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Functions.RegularExpressions
Imports DownDetector = SCrawler.API.Base.DownDetector
Imports Download = SCrawler.Plugin.ISiteSettings.Download
Namespace API.Reddit
    <Manifest(RedditSiteKey), UseClassAsIs, SavedPosts, SpecialForm(False)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return My.Resources.RedditIcon
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.RedditPic512
            End Get
        End Property
        <PropertyOption(ControlText:="Saved posts user"), PXML("SavedPostsUserName")>
        Friend ReadOnly Property SavedPostsUserName As PropertyValue
        Friend Overrides ReadOnly Property Responser As WEB.Response
        Friend Sub New()
            MyBase.New(RedditSite)
            Responser = New WEB.Response($"{SettingsFolderName}\Responser_{Site}.xml")

            With Responser
                If .File.Exists Then
                    .LoadSettings()
                Else
                    .CookiesDomain = "reddit.com"
                    .Decoders.Add(SymbolsConverter.Converters.Unicode)
                    .SaveSettings()
                End If
            End With
            SavedPostsUserName = New PropertyValue(String.Empty, GetType(String))
            UrlPatternUser = "https://www.reddit.com/user/{0}/"
            UrlPatternChannel = "https://www.reddit.com/r/{0}/"
            ImageVideoContains = "redgifs"
        End Sub
        Friend Overrides Function GetInstance(ByVal What As Download) As IPluginContentProvider
            Select Case What
                Case Download.Main : Return New UserData
                Case Download.Channel : Return New UserData With {.SaveToCache = False, .SkipExistsUsers = False, .AutoGetLimits = True}
                Case Download.SavedPosts
                    Dim u As New UserData With {.IsSavedPosts = True}
                    DirectCast(u, UserDataBase).User = New UserInfo With {.Name = CStr(AConvert(Of String)(SavedPostsUserName.Value, String.Empty))}
                    Return u
            End Select
            Return Nothing
        End Function
        Private ReadOnly RedditRegEx1 As RParams = RParams.DMS("[htps:/]{7,8}.*?reddit.com/user/([^/]+)", 1)
        Private ReadOnly RedditRegEx2 As RParams = RParams.DMS(".?u/([^/]+)", 1)
        Private ReadOnly RedditChannelRegEx1 As RParams = RParams.DMS("[htps:/]{7,8}.*?reddit.com/r/([^/]+)", 1)
        Private ReadOnly RedditChannelRegEx2 As RParams = RParams.DMS(".?r/([^/]+)", 1)
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            Dim s$
            Dim c% = 0
            For Each r As RParams In {RedditRegEx1, RedditRegEx2, RedditChannelRegEx1, RedditChannelRegEx2}
                s = RegexReplace(UserURL, r)
                If Not s.IsEmptyString Then Return New ExchangeOptions(Site, s, c > 1)
                c += 1
            Next
            Return Nothing
        End Function
        Friend Overrides Function Available(ByVal What As Download) As Boolean
            Try
                Dim dl As List(Of DownDetector.Data) = DownDetector.GetData("reddit")
                If dl.ListExists Then
                    dl = dl.Take(4).ToList
                    Dim avg% = dl.Average(Function(d) d.Value)
                    If avg > 100 Then
                        Return MsgBoxE({"Over the past hour, Reddit has received an average of " &
                                        avg.NumToString(New ANumbers With {.FormatOptions = ANumbers.Options.GroupIntegral}) & " outage reports:" & vbCr &
                                        dl.ListToString(, vbCr) & vbCr & vbCr &
                                        "Do you want to continue parsing Reddit data?", "There are outage reports on Reddit"}, vbYesNo) = vbYes
                    End If
                End If
                Return True
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendInLog + EDP.ReturnValue, ex, "[API.Reddit.SiteSettings.Available]", True)
            End Try
        End Function
        Friend Overrides Function GetSpecialDataF(ByVal URL As String) As IEnumerable(Of UserMedia)
            Return UserData.GetVideoInfo(URL, Responser)
        End Function
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse Not TypeOf Options Is RedditViewExchange Then Options = New RedditViewExchange
            If OpenForm Then
                Using f As New RedditViewSettingsForm(Options) : f.ShowDialog() : End Using
            End If
        End Sub
    End Class
End Namespace