' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Drawing
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.WEB
<Manifest("AndyProgram_XVIDEOS"), SpecialForm(True)>
Public Class SiteSettings : Implements ISiteSettings
    Public ReadOnly Property Icon As Icon Implements ISiteSettings.Icon
        Get
            Return My.Resources.Icon32
        End Get
    End Property
    Public ReadOnly Property Image As Image Implements ISiteSettings.Image
        Get
            Return My.Resources.IconPic32
        End Get
    End Property
    Public ReadOnly Property Site As String = "XVIDEOS" Implements ISiteSettings.Site
    Public Property Logger As ILogProvider Implements ISiteSettings.Logger
#Region "M3U8"
    Private ReadOnly OS64 As Boolean
    Friend ReadOnly FfmpegExists As Boolean
    Friend ReadOnly FfmpegFile As SFile
    Friend ReadOnly Property UseM3U8 As Boolean
        Get
            Return OS64 And FfmpegExists
        End Get
    End Property
#End Region
    <PXML("Domains")> Private Property SiteDomains As PropertyValue
    <PropertyOption(ControlText:="Download UHD", ControlToolTip:="Download UHD (4K) content"), PXML>
    Public Property DownloadUHD As PropertyValue
    Friend ReadOnly Property Domains As List(Of String)
    Public ReadOnly Property Responser As Response
    Private Const DomainsDefault As String = "xvideos.com|xnxx.com"
    Private _Initialized As Boolean = False
    Public Sub New()
        Responser = New Response($"Settings\Responser_{Site}.xml")
        With Responser
            If .File.Exists Then
                .LoadSettings()
            Else
                .CookiesDomain = "www.xvideos.com"
                .SaveSettings()
            End If
        End With
        OS64 = Environment.Is64BitOperatingSystem
        FfmpegFile = "ffmpeg.exe"
        FfmpegExists = FfmpegFile.Exists
        Domains = New List(Of String)
        SiteDomains = New PropertyValue(DomainsDefault, GetType(String), Sub(s) UpdateDomains())
        DownloadUHD = New PropertyValue(False)
    End Sub
    Public Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider Implements ISiteSettings.GetInstance
        Return New UserData
    End Function
    Public Sub BeginInit() Implements ISiteSettings.BeginInit
    End Sub
    Public Sub EndInit() Implements ISiteSettings.EndInit
        _Initialized = True
        UpdateDomains()
    End Sub
    Public Sub Load(ByVal XMLValues As IEnumerable(Of KeyValuePair(Of String, String))) Implements ISiteSettings.Load
    End Sub
    Private _DomainsUpdateInProgress As Boolean = False
    Friend Sub UpdateDomains()
        If Not _Initialized Then Exit Sub
        If Not _DomainsUpdateInProgress Then
            _DomainsUpdateInProgress = True
            If Not ACheck(SiteDomains.Value) Then SiteDomains.Value = DomainsDefault
            Domains.ListAddList(CStr(SiteDomains.Value).Split("|"), LAP.NotContainsOnly, LAP.ClearBeforeAdd)
            Domains.ListAddList(DomainsDefault.Split("|"), LAP.NotContainsOnly)
            SiteDomains.Value = Domains.ListToString("|")
            _DomainsUpdateInProgress = False
        End If
    End Sub
#Region "Downloading"
    Public Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean Implements ISiteSettings.Available
        Return UseM3U8
    End Function
    Public Function ReadyToDownload(ByVal What As ISiteSettings.Download) As Boolean Implements ISiteSettings.ReadyToDownload
        Return UseM3U8
    End Function
    Public Sub DownloadStarted(ByVal What As ISiteSettings.Download) Implements ISiteSettings.DownloadStarted
    End Sub
    Public Sub BeforeStartDownload(ByVal User As Object, ByVal What As ISiteSettings.Download) Implements ISiteSettings.BeforeStartDownload
    End Sub
    Public Sub AfterDownload(ByVal User As Object, ByVal What As ISiteSettings.Download) Implements ISiteSettings.AfterDownload
    End Sub
    Public Sub DownloadDone(ByVal What As ISiteSettings.Download) Implements ISiteSettings.DownloadDone
    End Sub
#End Region
    Public Sub BeginEdit() Implements ISiteSettings.BeginEdit
    End Sub
    Public Sub EndEdit() Implements ISiteSettings.EndEdit
    End Sub
    Public Sub BeginUpdate() Implements ISiteSettings.BeginUpdate
    End Sub
    Public Sub EndUpdate() Implements ISiteSettings.EndUpdate
    End Sub
    Public Sub Update() Implements ISiteSettings.Update
        UpdateDomains()
        Responser.SaveSettings()
    End Sub
    Public Sub Reset() Implements ISiteSettings.Reset
    End Sub
    Public Sub OpenSettingsForm() Implements ISiteSettings.OpenSettingsForm
        Using Design As New XmlFile("Settings\Design_XVIDEOS.xml")
            Using f As New SettingsForm(Me, Design) : f.ShowDialog() : End Using
        End Using
    End Sub
    Public Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean) Implements ISiteSettings.UserOptions
        Options = Nothing
    End Sub
    Public Function GetUserUrl(ByVal UserName As String, ByVal Channel As Boolean) As String Implements ISiteSettings.GetUserUrl
        Dim user$ = UserName.Split("_").FirstOrDefault
        user &= $"/{UserName.Replace($"{user}_", String.Empty)}"
        Return user
    End Function
    Private Const UserRegexDefault As String = "/(profiles|[\w]*?[-]{0,1}channels)/([^/]+)(\Z|.*?)"
    Private Const URD As String = ".*?{0}{1}"
    Public Function IsMyUser(ByVal UserURL As String) As ExchangeOptions Implements ISiteSettings.IsMyUser
        If Not UserURL.IsEmptyString Then
            If Domains.Count > 0 Then
                Dim uName$, uOpt$, fStr$
                For i% = 0 To Domains.Count - 1
                    fStr = String.Format(URD, Domains(i), UserRegexDefault)
                    uName = RegexReplace(UserURL, RParams.DMS(fStr, 2))
                    If Not uName.IsEmptyString Then
                        uOpt = RegexReplace(UserURL, RParams.DMS(fStr, 1))
                        If Not uOpt.IsEmptyString Then Return New ExchangeOptions(Site, $"{uOpt}_{uName}")
                    End If
                Next
            End If
        End If
        Return Nothing
    End Function
    Public Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions Implements ISiteSettings.IsMyImageVideo
        If Not URL.IsEmptyString And Domains.Count > 0 Then
            If Domains.Exists(Function(d) URL.Contains(d)) Then Return New ExchangeOptions With {.UserName = URL, .Exists = True}
        End If
        Return Nothing
    End Function
    Private Class TempThrower : Implements IThrower
        Private Sub ThrowAny() Implements IThrower.ThrowAny
        End Sub
    End Class
    Public Function GetSpecialData(ByVal URL As String, ByVal Path As String, ByVal AskForPath As Boolean) As IEnumerable(Of PluginUserMedia) Implements ISiteSettings.GetSpecialData
        If Not URL.IsEmptyString And UseM3U8 Then
            Dim f As SFile = Path.CSFileP
            f.Name = "video"
            f.Extension = "mp4"
#Disable Warning BC40000
            If AskForPath Then f = SFile.SaveAs(f,, True, "mp4")
#Enable Warning
            If Not f.IsEmptyString Then
                Using user As New UserData With {
                    .LogProvider = Logger,
                    .Thrower = New TempThrower,
                    .ESettings = Me,
                    .DataPath = f.Path
                }
                    With user
                        .TempMediaList = New List(Of PluginUserMedia) From {UserData.GetVideoData(URL, Responser.Copy, DownloadUHD.Value, String.Empty, Logger)}
                        If Not .TempMediaList(0).URL.IsEmptyString Then
                            .Download()
                            If .TempMediaList(0).DownloadState = PluginUserMedia.States.Downloaded Then
                                Dim p As PluginUserMedia = .TempMediaList(0)
                                p.SpecialFolder = p.File
                                Return {p}
                            End If
                        End If
                    End With
                End Using
            End If
        End If
        Return Nothing
    End Function
    Public Function GetUserPostUrl(ByVal UserID As String, ByVal PostID As String) As String Implements ISiteSettings.GetUserPostUrl
        Return String.Empty
    End Function
End Class