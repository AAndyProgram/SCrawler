' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Tools.WEB
Imports PersonalUtilities.Tools.WebDocuments.JSON
Imports UStates = SCrawler.Plugin.PluginUserMedia.States
Imports UTypes = SCrawler.Plugin.PluginUserMedia.Types
Public Class UserData : Implements IPluginContentProvider
#Region "Interface declarations"
    Public Event ProgressChanged(ByVal Count As Integer) Implements IPluginContentProvider.ProgressChanged
    Public Event TotalCountChanged(ByVal Count As Integer) Implements IPluginContentProvider.TotalCountChanged
    Public Property Thrower As IThrower Implements IPluginContentProvider.Thrower
    Public Property LogProvider As ILogProvider Implements IPluginContentProvider.LogProvider
    Public Property ESettings As ISiteSettings Implements IPluginContentProvider.Settings
    Private ReadOnly Property Settings As SiteSettings
        Get
            Return DirectCast(ESettings, SiteSettings)
        End Get
    End Property
    Public Property Name As String Implements IPluginContentProvider.Name
    Public Property ID As String Implements IPluginContentProvider.ID
    Public Property ParseUserMediaOnly As Boolean Implements IPluginContentProvider.ParseUserMediaOnly
    Public Property UserDescription As String Implements IPluginContentProvider.UserDescription
    Public Property ExistingContentList As List(Of PluginUserMedia) Implements IPluginContentProvider.ExistingContentList
    Public Property TempPostsList As List(Of String) Implements IPluginContentProvider.TempPostsList
    Public Property TempMediaList As List(Of PluginUserMedia) Implements IPluginContentProvider.TempMediaList
    Public Property UserExists As Boolean Implements IPluginContentProvider.UserExists
    Public Property UserSuspended As Boolean Implements IPluginContentProvider.UserSuspended
    Public Property IsSavedPosts As Boolean Implements IPluginContentProvider.IsSavedPosts
    Public Property SeparateVideoFolder As Boolean Implements IPluginContentProvider.SeparateVideoFolder
    Public Property DataPath As String Implements IPluginContentProvider.DataPath
    Public Property PostsNumberLimit As Integer? Implements IPluginContentProvider.PostsNumberLimit
    Public Property DownloadDateFrom As Date? Implements IPluginContentProvider.DownloadDateFrom
    Public Property DownloadDateTo As Date? Implements IPluginContentProvider.DownloadDateTo
#End Region
#Region "Interface exchange options"
    Public Sub ExchangeOptionsSet(ByVal Obj As Object) Implements IPluginContentProvider.ExchangeOptionsSet
    End Sub
    Public Function ExchangeOptionsGet() As Object Implements IPluginContentProvider.ExchangeOptionsGet
        Return Nothing
    End Function
#End Region
#Region "Interface XML"
    Public Sub XmlFieldsSet(ByVal Fields As List(Of KeyValuePair(Of String, String))) Implements IPluginContentProvider.XmlFieldsSet
    End Sub
    Public Function XmlFieldsGet() As List(Of KeyValuePair(Of String, String)) Implements IPluginContentProvider.XmlFieldsGet
        Return Nothing
    End Function
#End Region
    Private Property Responser As Response
    Public Sub GetMedia() Implements IPluginContentProvider.GetMedia
        Try
            If Not Settings.UseM3U8 Then
                If Settings.FfmpegExists Then
                    LogProvider.Add($"XVIDEOS [{Name}]: The plugin only works with x64 OS.")
                Else
                    LogProvider.Add($"XVIDEOS [{Name}]: File [ffmpeg.exe] not found")
                End If
                Exit Sub
            End If
            If Not Responser Is Nothing Then Responser.Dispose()
            Responser = New Response
            Responser.Copy(Settings.Responser)

            Dim NextPage% = 0
            Dim r$
            Dim jj As EContainer
            Dim e As ErrorsDescriber = EDP.ThrowException
            Dim user$ = Settings.GetUserUrl(Name, False)
            Dim p As PluginUserMedia
            Dim EnvirSet As Boolean = False

            Do
                Thrower.ThrowAny()
                r = Responser.GetResponse($"https://www.xvideos.com/{user}/videos/new/{If(NextPage = 0, String.Empty, NextPage)}",, e)
                If Not r.IsEmptyString Then
                    If Not EnvirSet Then UserExists = True : UserSuspended = False : EnvirSet = True
                    With JsonDocument.Parse(r).XmlIfNothing
                        If .Contains("videos") Then
                            With .Item("videos")
                                If .Count > 0 Then
                                    NextPage += 1
                                    For Each jj In .Self
                                        p = New PluginUserMedia With {
                                            .PostID = jj.Value("id"),
                                            .URL = $"https://www.xvideos.com{jj.Value("u")}"
                                        }
                                        If Not p.PostID.IsEmptyString And Not jj.Value("u").IsEmptyString Then
                                            If Not TempPostsList.Contains(p.PostID) Then TempPostsList.Add(p.PostID) : TempMediaList.Add(p) Else .Dispose() : Exit Do
                                        End If
                                    Next
                                Else
                                    .Dispose()
                                    Exit Do
                                End If
                            End With
                        Else
                            .Dispose()
                            Exit Do
                        End If
                        .Dispose()
                    End With
                Else
                    Exit Do
                End If
            Loop

            If TempMediaList.Count > 0 Then
                For i% = 0 To TempMediaList.Count - 1
                    Thrower.ThrowAny()
                    With TempMediaList(i) : TempMediaList(i) = GetVideoData(.URL, Responser, Settings.DownloadUHD.Value, .PostID, LogProvider) : End With
                Next
                TempMediaList.RemoveAll(Function(m) m.URL.IsEmptyString)
            End If
        Catch oex As OperationCanceledException
        Catch dex As ObjectDisposedException
        Catch ex As Exception
            If Responser.StatusCode = Net.HttpStatusCode.NotFound Then
                UserExists = False
            Else
                LogProvider.Add(ex, "[XVIDEOS.UserData.GetMedia]")
            End If
        Finally
            If TempMediaList.ListExists Then TempMediaList.RemoveAll(Function(m) m.URL.IsEmptyString)
        End Try
    End Sub
    Private Structure VSize : Implements IRegExCreator, IComparable(Of VSize)
        Friend Size As Integer
        Friend Value As String
        Private Function CreateFromArray(ByVal ParamsArray() As String) As Object Implements IRegExCreator.CreateFromArray
            If ParamsArray.ListExists(2) Then
                Size = AConvert(Of Integer)(ParamsArray(0), 0)
                Value = ParamsArray(1)
            End If
            Return Me
        End Function
        Private Function CompareTo(ByVal Other As VSize) As Integer Implements IComparable(Of VSize).CompareTo
            Return Size.CompareTo(Other.Size) * -1
        End Function
    End Structure
    Friend Shared Function GetVideoData(ByVal URL As String, ByVal resp As Response, ByVal DownloadUHD As Boolean,
                                        ByVal ID As String, ByRef Logger As ILogProvider) As PluginUserMedia
        Try
            If Not URL.IsEmptyString Then
                Dim r$ = resp.GetResponse(URL,, EDP.ThrowException)
                If Not r.IsEmptyString Then
                    Dim m$ = RegexReplace(r, M3U8Regex)
                    If Not m.IsEmptyString Then
                        Dim appender$ = RegexReplace(m, M3U8Appender)
                        Dim t$ = RegexReplace(r, VideoTitleRegex)
                        r = resp.GetResponse(m,, EDP.ThrowException)
                        If Not r.IsEmptyString Then
                            Dim ls As List(Of VSize) = RegexFields(Of VSize)(r, {M3U8Reparse}, {1, 2})
                            If ls.ListExists And Not DownloadUHD Then ls.RemoveAll(Function(v) v.Size > 1080)
                            If ls.ListExists Then
                                ls.Sort()
                                m = $"{appender}/{ls(0).Value}"
                                ls.Clear()
                                Dim pID$ = ID
                                If pID.IsEmptyString Then pID = RegexReplace(r, VideoID)
                                If pID.IsEmptyString Then pID = "0"

                                If Not t.IsEmptyString Then t = t.StringRemoveWinForbiddenSymbols(" ")
                                If t.IsEmptyString Then
                                    t = pID
                                Else
                                    If t.Length > 100 Then t = Left(t, 100)
                                End If
                                If Not m.IsEmptyString Then
                                    Return New PluginUserMedia With {
                                        .ContentType = UTypes.m3u8,
                                        .PostID = pID,
                                        .URL = m,
                                        .File = $"{t}.mp4",
                                        .SpecialFolder = appender
                                    }
                                End If
                            End If
                        End If
                    End If
                End If
            End If
            Return Nothing
        Catch ex As Exception
            Logger.Add(ex, $"[XVIDEOS.UserData.GetVideoData({URL})]")
            Return Nothing
        End Try
    End Function
    Public Sub Download() Implements IPluginContentProvider.Download
        Try
            If TempMediaList.Count > 0 Then
                RaiseEvent TotalCountChanged(TempMediaList.Count - 1)
                Dim m As PluginUserMedia
                Dim f As SFile
                Dim DefPath As String = DataPath.CSFilePSN
                For i% = 0 To TempMediaList.Count - 1
                    Thrower.ThrowAny()
                    m = TempMediaList(i)
                    f = m.File
                    f.Path = DefPath
                    m.DownloadState = UStates.Tried
                    Try
                        f = M3U8.Download(m.URL, m.SpecialFolder, Settings.FfmpegFile, f, LogProvider)
                        m.File = f
                        m.DownloadState = UStates.Downloaded
                    Catch ex As Exception
                        m.DownloadState = UStates.Missing
                        m.Attempts += 1
                    End Try
                    TempMediaList(i) = m
                    RaiseEvent ProgressChanged(1)
                Next
            End If
        Catch oex As OperationCanceledException
        Catch dex As ObjectDisposedException
        Catch ex As Exception
            LogProvider.Add(ex, $"[XVIDEOS.UserData.Download]")
        End Try
    End Sub
#Region "IDisposable Support"
    Private disposedValue As Boolean = False
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                If Not Responser Is Nothing Then Responser.Dispose()
                If TempMediaList.ListExists Then TempMediaList.Clear()
                If TempPostsList.ListExists Then TempPostsList.Clear()
                If ExistingContentList.ListExists Then ExistingContentList.Clear()
            End If
            disposedValue = True
        End If
    End Sub
    Protected Overrides Sub Finalize()
        Dispose(False)
        MyBase.Finalize()
    End Sub
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
