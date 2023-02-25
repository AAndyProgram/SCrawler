' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Runtime.CompilerServices
Imports PersonalUtilities.Functions.XML.Objects
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.Web
Imports SCrawler.API
Imports SCrawler.API.Base
Imports SCrawler.Plugin.Hosts
Imports SCrawler.DownloadObjects
Friend Module MainMod
    Friend Settings As SettingsCLS
    Friend Const SettingsFolderName As String = "Settings"
    Friend ReadOnly LinkPattern As RParams = RParams.DMS("[htps:]{0,6}[/]{0,2}(.+)", 1)
    Friend ReadOnly FilesPattern As RParams = RParams.DM("[^\./]+?\.\w+", 1, EDP.ReturnValue)
    Friend Delegate Sub NotificationEventHandler(ByVal Sender As SettingsCLS.NotificationObjects, ByVal Message As String)
    Friend Const LVI_TempOption As String = "Temp"
    Friend Const LVI_FavOption As String = "Favorite"
    Friend Const CannelsLabelName As String = "Channels"
    Friend Const LVI_CollectionOption As String = "Collection"
    Friend Const LVI_ChannelOption As String = "Channel"
    Friend Property BATCH As BatchExecutor
    Private _BatchLogSent As Boolean = False
    ''' <param name="e"><see cref="EDP.None"/></param>
    Friend Sub GlobalOpenPath(ByVal f As SFile, Optional ByVal e As ErrorsDescriber = Nothing)
        Dim b As Boolean = False
        If Not e.Exists Then e = EDP.None
        Try
            If f.Exists(SFO.Path, False) Then
                If Settings.OpenFolderInOtherProgram.Attribute.Value AndAlso Not Settings.OpenFolderInOtherProgram.IsEmptyString Then
                    If BATCH Is Nothing Then BATCH = New BatchExecutor With {.RedirectStandardError = True}
                    b = True
                    With BATCH
                        .Reset()
                        .Execute({String.Format(Settings.OpenFolderInOtherProgram.Value, f.PathWithSeparator)}, EDP.SendInLog + EDP.ThrowException)
                        If .HasError Or Not .ErrorOutput.IsEmptyString Then Throw New Exception(.ErrorOutput, .ErrorException)
                    End With
                Else
                    f.Open(SFO.Path, e)
                End If
            End If
        Catch ex As Exception
            If b Then
                If Not _BatchLogSent Then ErrorsDescriber.Execute(EDP.SendInLog, ex, $"GlobalOpenPath({f.Path})") : _BatchLogSent = True
                f.Open(SFO.Path, e)
            End If
        End Try
    End Sub
    Friend Sub ExecuteCommand(ByVal Obj As XMLValueAttribute(Of String, Boolean))
        Try
            If Obj.Attribute And Not Obj.IsEmptyString Then
                Using b As New BatchExecutor With {.RedirectStandardError = True}
                    With b
                        .Execute({Obj.Value}, EDP.SendInLog + EDP.ThrowException)
                        If .HasError Or Not .ErrorOutput.IsEmptyString Then Throw New Exception(.ErrorOutput, .ErrorException)
                    End With
                End Using
            End If
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.SendInLog, ex, $"[{Obj.Name}] command: [{Obj.Value}]")
        End Try
    End Sub
    Friend Enum ViewModes As Integer
        IconLarge = View.LargeIcon
        IconSmall = View.SmallIcon
        List = View.Tile
        Details = View.Details
    End Enum
    Friend Enum ShowingModes As Integer
        All = 0
        Regular = 20
        Temporary = 50
        Favorite = 100
        Labels = 500
        NoLabels = 1000
        Deleted = 10000
        Suspended = 12000
    End Enum
    Friend Enum ShowingDates As Integer
        [Off] = 0
        [Not] = 1
        [In] = 2
    End Enum
    Friend Enum FileNameReplaceMode As Integer
        None = 0
        Replace = 1
        Add = 2
    End Enum
    Friend Enum UsageModel As Integer
        [Default] = 0
        Virtual = 1
    End Enum
    Friend Downloader As TDownloader
    Friend InfoForm As DownloadedInfoForm
    Friend VideoDownloader As VideosDownloaderForm
    Friend UserListLoader As ListImagesLoader
    Friend MyProgressForm As ActiveDownloadingProgress
    Friend MainFrameObj As MainFrameObjects
    Friend ReadOnly ParsersDataDateProvider As New ADateTime(ADateTime.Formats.BaseDateTime)
    Friend ReadOnly FeedVideoLengthProvider As New ADateTime("hh\:mm\:ss") With {.TimeParseMode = ADateTime.TimeModes.TimeSpan}
    Friend ReadOnly UserExistsPredicate As New FPredicate(Of IUserData)(Function(u) u.Exists)
    Friend ReadOnly LogConnector As New LogHost
    Friend DefaultUserAgent As String = String.Empty
#Region "File name operations"
    Friend FileDateAppenderProvider As IFormatProvider
    ''' <summary>File, Date</summary>
    Friend FileDateAppenderPattern As String
    Friend Class NumberedFile : Inherits SFileNumbers
        Friend Sub New(ByVal f As SFile)
            FileName = f.Name
            NumberProvider = New ANumbers With {.Format = ANumbers.Formats.NumberGroup, .GroupSize = 5}
        End Sub
    End Class
#End Region
    Friend Property MainProgress As MyProgress
    Friend Function GetLviGroupName(ByVal Host As SettingsHost, ByVal IsCollection As Boolean, ByVal IsChannel As Boolean) As ListViewGroup()
        Dim l As New List(Of ListViewGroup)
        Dim t$
        t = GetLviGroupName(Host, False, True, IsCollection, IsChannel)
        l.Add(New ListViewGroup(t, t))
        t = GetLviGroupName(Host, False, False, IsCollection, IsChannel)
        l.Add(New ListViewGroup(t, t))
        t = GetLviGroupName(Host, True, False, IsCollection, IsChannel)
        l.Add(New ListViewGroup(t, t))
        Return l.ToArray
    End Function
    Friend Function GetLviGroupName(ByVal Host As SettingsHost, ByVal Temp As Boolean, ByVal Fav As Boolean,
                                    ByVal IsCollection As Boolean, ByVal IsChannel As Boolean) As String
        Dim Opt$ = String.Empty
        If Temp Then
            Opt = LVI_TempOption
        ElseIf Fav Then
            Opt = LVI_FavOption
        End If
        If Not Opt.IsEmptyString Then Opt = $" ({Opt})"
        If IsCollection Then
            Return $"{LVI_CollectionOption}{Opt}"
        ElseIf IsChannel Then
            Return $"{LVI_ChannelOption}{Opt}"
        Else
            Return $"{If(Host?.Name, String.Empty)}{Opt}"
        End If
    End Function
    <Extension> Friend Function GetGroupsLabels(Of T As Groups.IGroup)(ByVal Groups As IEnumerable(Of T)) As List(Of String)
        If Groups.ListExists Then
            Return ListAddList(Nothing, Groups.SelectMany(Function(g) g.Labels), LAP.NotContainsOnly).
                   ListAddList(Groups.SelectMany(Function(g) g.LabelsExcluded), LAP.NotContainsOnly)
        Else
            Return Nothing
        End If
    End Function
#Region "Standalone video download functions"
    Friend Function GetCurrentBuffer() As String
        Dim b$ = BufferText
        If Not (Not b.IsEmptyString AndAlso b.Length > 4 AndAlso b.StartsWith("http")) Then b = String.Empty
        Return b
    End Function
    Friend Function DownloadVideoByURL(ByVal URL As String, ByVal AskForPath As Boolean, ByVal Silent As Boolean) As Boolean
        Dim e As New ErrorsDescriber(Not Silent, Not Silent, True, False)
        Try
            Dim Result As Boolean = False
            If Not URL.IsEmptyString Then
                Dim um As IEnumerable(Of UserMedia) = Nothing
                Dim found As Boolean = False
                Dim d As Plugin.ExchangeOptions
                If Settings.Plugins.Count > 0 Then
                    For Each p As PluginHost In Settings.Plugins
                        d = p.Settings.IsMyImageVideo(URL)
                        If d.Exists Then
                            um = Settings(d.HostKey).GetSpecialData(URL, Settings.LatestSavingPath.Value, AskForPath)
                            found = True
                            If um.ListExists Then
                                If AskForPath And Not um(0).SpecialFolder.IsEmptyString Then Settings.LatestSavingPath.Value = um(0).SpecialFolder
                                If um(0).State = UserMedia.States.Downloaded Then Return True
                            End If
                            Exit For
                        End If
                    Next
                End If
                If Not found Then
                    If URL.Contains("gfycat") Then
                        um = Gfycat.Envir.GetVideoInfo(URL)
                        If um.ListExists AndAlso um(0).URL.Contains("redgifs.com") Then Return DownloadVideoByURL(um(0).URL, AskForPath, Silent)
                    ElseIf URL.Contains("imgur.com") Then
                        um = Imgur.Envir.GetVideoInfo(URL)
                    Else
                        MsgBoxE("Site of video URL not recognized" & vbCr & "Operation canceled", MsgBoxStyle.Exclamation, e)
                        Return False
                    End If
                End If

                If um.ListExists Then
                    Dim f As SFile, ff As SFile
                    Dim dURL$
                    Dim FileDownloaded As Boolean = False

                    For Each u As UserMedia In um
                        If Not u.URL.IsEmptyString Or Not u.URL_BASE.IsEmptyString Then
                            f = u.File
                            If f.Name.IsEmptyString Then f.Name = $"video_{u.Post.ID}"
                            If f.Extension.IsEmptyString Then f.Extension = "mp4"
                            If Not Settings.LatestSavingPath.IsEmptyString And
                                   Settings.LatestSavingPath.Value.Exists(SFO.Path, False) Then f.Path = Settings.LatestSavingPath.Value
                            If AskForPath OrElse Not f.Exists(SFO.Path, False) Then
#Disable Warning BC40000
                                ff = SFile.SaveAs(f, "Files destination",,,, EDP.ReturnValue)
#Enable Warning
                                If Not ff.IsEmptyString Then
                                    f.Path = ff.Path
                                Else
                                    f = Nothing
                                End If
                                AskForPath = False
                            End If
                            If Not f.IsEmptyString Then
                                Settings.LatestSavingPath.Value = f.PathWithSeparator
                                FileDownloaded = False
                                Using w As New Net.WebClient
                                    For i% = 0 To 1
                                        If i = 0 Then dURL = u.URL Else dURL = u.URL_BASE
                                        If Not dURL.IsEmptyString Then
                                            Try
                                                w.DownloadFile(dURL, f)
                                                FileDownloaded = True
                                                Exit For
                                            Catch wex As Exception
                                                ErrorsDescriber.Execute(EDP.SendInLog, wex, "DownloadVideoByURL")
                                            End Try
                                        End If
                                    Next
                                End Using
                                If FileDownloaded Then
                                    If um.Count = 1 Then
                                        MsgBoxE($"File downloaded to [{f}]",, e)
                                        Return True
                                    Else
                                        Result = True
                                    End If
                                Else
                                    If um.Count = 1 Then MsgBoxE("File not downloaded", MsgBoxStyle.Critical, e)
                                End If
                            Else
                                If um.Count = 1 Then MsgBoxE("File destination not specified" & vbCr & "Operation canceled",, e)
                            End If
                        Else
                            If um.Count = 1 Then MsgBoxE("File URL not found!", MsgBoxStyle.Critical, e)
                        End If
                    Next
                End If
            Else
                MsgBoxE("URL is empty", MsgBoxStyle.Exclamation, e)
            End If
            Return Result
        Catch ex As Exception
            Return ErrorsDescriber.Execute(e, ex, $"Error when trying to download video from URL: [{URL}]", False)
        End Try
    End Function
#End Region
    Friend Sub CheckVersion(ByVal Force As Boolean)
        If Settings.CheckUpdatesAtStart Or Force Then _
           GitHub.DefaultVersionChecker(My.Application.Info.Version, "AAndyProgram", "SCrawler",
                                        Settings.LatestVersion.Value, Settings.ShowNewVersionNotification.Value, Force)
    End Sub
End Module