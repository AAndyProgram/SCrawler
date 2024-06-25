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
Imports PersonalUtilities.Tools
Imports SCrawler.API.Base
Imports SCrawler.Plugin.Hosts
Imports SCrawler.DownloadObjects
Friend Module MainMod
    Friend Settings As SettingsCLS
    Friend Const SettingsFolderName As String = XML.XmlFile.SettingsFolder
    Friend Const UserRegexDefaultPattern As String = "{0}([^/\?&]+)"
    Friend ReadOnly LinkPattern As RParams = RParams.DMS("[htps:]{0,6}[/]{0,2}(.+)", 1)
    Friend ReadOnly FilesPattern As RParams = RParams.DM("[^\./]+?\.\w+", 1, EDP.ReturnValue)
    Friend Delegate Sub NotificationEventHandler(ByVal Sender As SettingsCLS.NotificationObjects, ByVal Message As String)
    Friend Delegate Sub UserDownloadStateChangedEventHandler(ByVal User As IUserData, ByVal IsDownloading As Boolean)
    Friend Delegate Sub UsersAddedEventHandler(ByVal StartIndex As Integer)
    Friend Delegate Function PathMoverHandler(ByVal User As UserInfo, ByVal DestinationPattern As SFile) As SFile
    Friend Const LVI_TempOption As String = "Temp"
    Friend Const LVI_FavOption As String = "Favorite"
    Friend Const LVI_CollectionOption As String = "Collection"
    Friend Sub ExecuteCommand(ByVal Obj As XMLValueAttribute(Of String, Boolean))
        Try
            If Obj.Attribute And Not Obj.IsEmptyString Then
                Using b As New BatchExecutor With {.RedirectStandardError = True}
                    With b
                        .Execute({Obj.Value}, EDP.SendToLog + EDP.ThrowException)
                        If .HasError Or Not .ErrorOutput.IsEmptyString Then Throw New Exception(.ErrorOutput, .ErrorException)
                    End With
                End Using
            End If
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.SendToLog, ex, $"[{Obj.Name}] command: [{Obj.Value}]")
        End Try
    End Sub
    Friend Enum ViewModes As Integer
        IconLarge = View.LargeIcon
        IconSmall = View.SmallIcon
        List = View.Tile
        Details = View.Details
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
    Friend Enum PathCreationModel As Integer
        Undefined = -1
        Path = 1
        Path_UserName = 2
        Path_UserSite_UserName = 3
        DefaultUser = Path_UserSite_UserName
        Collection = 4
    End Enum
    Friend Downloader As TDownloader
    Friend InfoForm As DownloadedInfoForm
    Friend VideoDownloader As STDownloader.VideoDownloaderForm
    Friend UserListLoader As ListImagesLoader
    Friend MyProgressForm As ActiveDownloadingProgress
    Friend MainFrameObj As MainFrameObjects
    ''' <summary>Alt+F1</summary>
    Friend ReadOnly ShowUsersButtonKey As New PersonalUtilities.Forms.ButtonKey(Keys.F1,, True)
    Friend ReadOnly DateTimeDefaultProvider As New ADateTime(ADateTime.Formats.BaseDateTime)
    <Extension> Friend Function ToStringDateDef(ByVal _DateN As Date?) As String
        Return If(_DateN.HasValue, AConvert(Of String)(_DateN, DateTimeDefaultProvider, String.Empty), String.Empty)
    End Function
    <Extension> Friend Function ToStringDateDef(ByVal _Date As Date) As String
        Return ToStringDateDef(_DateN:=_Date)
    End Function
    <Extension> Friend Function ToDateDef(ByVal DateStr As String, Optional ByVal NothingArg As Object = Nothing) As Object
        Return AConvert(Of Date)(DateStr, DateTimeDefaultProvider, NothingArg)
    End Function
    Friend ReadOnly SessionDateTimeProvider As New ADateTime("yyyyMMdd_HHmmss")
    Friend ReadOnly FeedVideoLengthProvider As New ADateTime("hh\:mm\:ss") With {.TimeParseMode = ADateTime.TimeModes.TimeSpan}
    Friend ReadOnly LogConnector As New LogHost
    Friend SiteSettingsShowHiddenControls As Boolean = False
#Region "NonExistingUsersLog"
    Friend ReadOnly NonExistingUsersLog As New TextSaver($"LOGs\NonExistingUsers.txt") With {.LogMode = True, .AutoSave = True}
    Friend Sub AddNonExistingUserToLog(ByVal Message As String)
        MyMainLOG = Message
        NonExistingUsersLog.AppendLine(Message)
    End Sub
#End Region
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
    Friend Property MainProgress As MyProgressExt
    Friend Sub ShowOperationCanceledMsg(Optional ByVal MsgTitle As String = Nothing)
        Dim m As New MMessage("Operation canceled")
        If Not MsgTitle.IsEmptyString Then m.Title = MsgTitle
        m.Show()
    End Sub
    Friend Function GetLviGroupName(ByVal Host As SettingsHost, ByVal IsCollection As Boolean) As ListViewGroup()
        Dim l As New List(Of ListViewGroup)
        Dim t$
        t = GetLviGroupName(Host, False, True, IsCollection)
        l.Add(New ListViewGroup(t, t))
        t = GetLviGroupName(Host, False, False, IsCollection)
        l.Add(New ListViewGroup(t, t))
        t = GetLviGroupName(Host, True, False, IsCollection)
        l.Add(New ListViewGroup(t, t))
        Return l.ToArray
    End Function
    Friend Function GetLviGroupName(ByVal Host As SettingsHost, ByVal Temp As Boolean, ByVal Fav As Boolean, ByVal IsCollection As Boolean) As String
        Dim Opt$ = String.Empty
        If Temp Then
            Opt = LVI_TempOption
        ElseIf Fav Then
            Opt = LVI_FavOption
        End If
        If Not Opt.IsEmptyString Then Opt = $" ({Opt})"
        If IsCollection Then
            Return $"{LVI_CollectionOption}{Opt}"
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
    Friend Sub CheckVersion(ByVal Force As Boolean)
        With Settings
            If .CheckUpdatesAtStart Or Force Then
                ShowProgramInfo(.ProgramText.Value.IfNullOrEmpty("SCrawler"),
                                SCrawler.Shared.GetCurrentMaxVer(Application.StartupPath.CSFileP).IfNullOrEmpty(My.Application.Info.Version),
                                True, Force, .Self, False,
                                .LatestVersion.Value, .ShowNewVersionNotification.Value, .ProgramDescription)
            End If
        End With
    End Sub
End Module