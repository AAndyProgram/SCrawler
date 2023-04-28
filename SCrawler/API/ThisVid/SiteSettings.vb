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
Namespace API.ThisVid
    <Manifest(ThisVidSiteKey), SeparatedTasks(1), SpecialForm(False), SavedPosts>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return My.Resources.SiteResources.ThisVidIcon_16
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.SiteResources.ThisVidPic_16
            End Get
        End Property
        <PXML, PropertyOption(ControlText:="Public videos", ControlToolTip:="Download public videos")>
        Friend ReadOnly Property DownloadPublic As PropertyValue
        <PXML, PropertyOption(ControlText:="Private videos", ControlToolTip:="Download private videos")>
        Friend ReadOnly Property DownloadPrivate As PropertyValue
        <PXML, PropertyOption(ControlText:="Different folders",
                              ControlToolTip:="Use different folders to store video files." & vbCr &
                                              "If true, then public videos will be stored in the 'Public' folder, private - in the 'Private' folder." & vbCr &
                                              "If false, all videos will be stored in the 'Video' folder.")>
        Friend ReadOnly Property DifferentFolders As PropertyValue
#End Region
#Region "Initializer"
        Friend Sub New()
            MyBase.New("ThisVid", "thisvid.com")
            DownloadPublic = New PropertyValue(True)
            DownloadPrivate = New PropertyValue(True)
            DifferentFolders = New PropertyValue(True)
            CheckNetscapeCookiesOnEndInit = True
            UseNetscapeCookies = True
            UserRegex = RParams.DMS("thisvid.com/members/(\d+)", 1)
            UrlPatternUser = "https://thisvid.com/members/{0}/"
            ImageVideoContains = "https://thisvid.com/videos/"
        End Sub
#End Region
#Region "GetInstance, GetSpecialData"
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
#End Region
#Region "Downloading"
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            Return Settings.YtdlpFile.Exists And (What = ISiteSettings.Download.SingleObject Or Responser.CookiesExists)
        End Function
#End Region
#Region "UserOptions"
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse Not TypeOf Options Is UserExchangeOptions Then Options = New UserExchangeOptions(Me)
            If OpenForm Then
                Using f As New InternalSettingsForm(Options, Me, False) : f.ShowDialog() : End Using
            End If
        End Sub
#End Region
    End Class
End Namespace