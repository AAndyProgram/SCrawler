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
                If .Decoders.Count = 0 OrElse Not .Decoders.Contains(SymbolsConverter.Converters.Unicode) Then _
                   .Decoders.Add(SymbolsConverter.Converters.Unicode) : .SaveSettings()
            End With
            SavedPostsUserName = New PropertyValue(String.Empty, GetType(String))
            UseM3U8 = New PropertyValue(True)
            UrlPatternUser = "https://www.reddit.com/user/{0}/"
            UrlPatternChannel = "https://www.reddit.com/r/{0}/"
            ImageVideoContains = "reddit.com"
            UserRegex = RParams.DM("[htps:/]{7,8}.*?reddit.com/([user]{1,4})/([^/]+)", 0, RegexReturn.ListByMatch, EDP.ReturnValue)
        End Sub
        Friend Overrides Function GetInstance(ByVal What As Download) As IPluginContentProvider
            Select Case What
                Case Download.Main : Return New UserData
                Case Download.Channel : Return New UserData With {.SaveToCache = False, .SkipExistsUsers = False, .AutoGetLimits = True}
                Case Download.SavedPosts
                    Dim u As New UserData With {.IsSavedPosts = True}
                    DirectCast(u, UserDataBase).User = New UserInfo With {
                        .Name = CStr(AConvert(Of String)(SavedPostsUserName.Value, String.Empty)),
                        .IsChannel = True
                    }
                    Return u
            End Select
            Return Nothing
        End Function
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            Dim l As List(Of String) = RegexReplace(UserURL, UserRegex)
            If l.ListExists(3) Then Return New ExchangeOptions(Site, l(2), l(1) = "r") Else Return Nothing
        End Function
        Friend Overrides Function Available(ByVal What As Download, ByVal Silent As Boolean) As Boolean
            Try
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
                                Return True
                            Else
                                Return False
                            End If
                        End If
                    End If
                End If
                UpdateRedGifsToken()
                Return True
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendInLog + EDP.ReturnValue, ex, "[API.Reddit.SiteSettings.Available]", True)
            End Try
        End Function
        Private Sub UpdateRedGifsToken()
            DirectCast(Settings(RedGifs.RedGifsSiteKey).Source, RedGifs.SiteSettings).UpdateTokenIfRequired()
        End Sub
        Friend Overrides Function GetSpecialData(ByVal URL As String, ByVal Path As String, ByVal AskForPath As Boolean) As IEnumerable
            Dim spf$ = String.Empty
            Dim f As SFile = GetSpecialDataFile(Path, AskForPath, spf)
            f = $"{f.PathWithSeparator}OptionalPath\"
            Return UserData.GetVideoInfo(URL, Responser, f, spf)
        End Function
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse Not TypeOf Options Is RedditViewExchange Then Options = New RedditViewExchange
            If OpenForm Then
                Using f As New RedditViewSettingsForm(Options) : f.ShowDialog() : End Using
            End If
        End Sub
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            Return $"https://www.reddit.com/comments/{Media.Post.ID.Split("_").LastOrDefault}/"
        End Function
    End Class
End Namespace