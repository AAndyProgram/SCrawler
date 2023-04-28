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
Imports DownDetector = SCrawler.API.Base.DownDetector
Imports Download = SCrawler.Plugin.ISiteSettings.Download
Namespace API.Reddit
    <Manifest(RedditSiteKey), SavedPosts, SpecialForm(False)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return My.Resources.SiteResources.RedditIcon_128
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.SiteResources.RedditPic_512
            End Get
        End Property
        <PropertyOption(ControlText:="Saved posts user", ControlToolTip:="Personal profile username"), PXML>
        Friend ReadOnly Property SavedPostsUserName As PropertyValue
        <PropertyOption(ControlText:="Use M3U8", ControlToolTip:="Use M3U8 or mp4 for Reddit videos"), PXML>
        Friend ReadOnly Property UseM3U8 As PropertyValue
        Friend Sub New()
            MyBase.New(RedditSite, "reddit.com")
            With Responser
                Dim d% = .Decoders.Count
                .Decoders.ListAddList({SymbolsConverter.Converters.Unicode, SymbolsConverter.Converters.HTML}, LAP.NotContainsOnly)
                If d <> .Decoders.Count Then .SaveSettings()
            End With
            SavedPostsUserName = New PropertyValue(String.Empty, GetType(String))
            UseM3U8 = New PropertyValue(True)
            UrlPatternUser = "https://www.reddit.com/{0}/{1}/"
            ImageVideoContains = "reddit.com"
            UserRegex = RParams.DM("[htps:/]{7,8}.*?reddit.com/([user]{1,4})/([^/]+)", 0, RegexReturn.ListByMatch, EDP.ReturnValue)
        End Sub
        Friend Overrides Function GetInstance(ByVal What As Download) As IPluginContentProvider
            Return New UserData
        End Function
        Friend Const ChannelOption As String = "r"
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            Dim l As List(Of String) = RegexReplace(UserURL, UserRegex)
            If l.ListExists(3) Then
                Dim n$ = l(2)
                If Not l(1).IsEmptyString AndAlso l(1) = ChannelOption Then n &= $"@{ChannelOption}"
                Return New ExchangeOptions(Site, n)
            Else
                Return Nothing
            End If
        End Function
        Friend Overrides Function Available(ByVal What As Download, ByVal Silent As Boolean) As Boolean
            Try
                Dim trueValue As Boolean = Not What = Download.SavedPosts OrElse (Responser.CookiesExists And ACheck(SavedPostsUserName.Value))
                If Not trueValue Then Return False
                Dim dl As List(Of DownDetector.Data) = DownDetector.GetData("reddit")
                If dl.ListExists Then
                    dl = dl.Take(4).ToList
                    Dim avg% = dl.Average(Function(d) d.Value)
                    If avg > 100 Then
                        If Silent Then
                            Return False
                        Else
                            If MsgBoxE({"Over the past hour, Reddit has received an average of " &
                                        avg.NumToString(New ANumbers With {.FormatOptions = ANumbers.Options.GroupIntegral}) & " outage reports:" & vbCr &
                                        dl.ListToString(vbCr) & vbCr & vbCr &
                                        "Do you want to continue parsing Reddit data?", "There are outage reports on Reddit"}, vbYesNo) = vbYes Then
                                UpdateRedGifsToken()
                                Return trueValue
                            Else
                                Return False
                            End If
                        End If
                    End If
                End If
                UpdateRedGifsToken()
                Return trueValue
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, "[API.Reddit.SiteSettings.Available]", True)
            End Try
        End Function
        Private Sub UpdateRedGifsToken()
            DirectCast(Settings(RedGifs.RedGifsSiteKey).Source, RedGifs.SiteSettings).UpdateTokenIfRequired()
        End Sub
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse Not TypeOf Options Is RedditViewExchange Then Options = New RedditViewExchange
            If OpenForm Then
                Using f As New RedditViewSettingsForm(Options) : f.ShowDialog() : End Using
            End If
        End Sub
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider) As String
            With DirectCast(User, UserData) : Return String.Format(UrlPatternUser, IIf(.IsChannel, ChannelOption, "user"), .TrueName) : End With
        End Function
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            If Not Media.Post.ID.IsEmptyString Then
                Return $"https://www.reddit.com/comments/{Media.Post.ID.Split("_").LastOrDefault}/"
            Else
                Return String.Empty
            End If
        End Function
    End Class
End Namespace