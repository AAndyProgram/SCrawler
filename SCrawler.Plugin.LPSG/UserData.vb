' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.RegularExpressions
Imports UStates = SCrawler.Plugin.PluginUserMedia.States
Imports UTypes = SCrawler.Plugin.PluginUserMedia.Types
Imports Converters = PersonalUtilities.Functions.SymbolsConverter.Converters
Public Class UserData : Implements IPluginContentProvider
#Region "XML names"
    Private Const Name_LatestPage As String = "LatestPage"
#End Region
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
        If Fields.ListExists Then
            For Each f As KeyValuePair(Of String, String) In Fields
                If f.Key = Name_LatestPage Then LatestPage = f.Value
            Next
        End If
    End Sub
    Public Function XmlFieldsGet() As List(Of KeyValuePair(Of String, String)) Implements IPluginContentProvider.XmlFieldsGet
        Return New List(Of KeyValuePair(Of String, String)) From {New KeyValuePair(Of String, String)(Name_LatestPage, LatestPage)}
    End Function
#End Region
    Private Property LatestPage As String = String.Empty
    Private Property Responser As Response = Nothing
    Private Enum Mode : Internal : External : End Enum
    Public Sub GetMedia() Implements IPluginContentProvider.GetMedia
        Try
            If Not Responser Is Nothing Then Responser.Dispose()
            Responser = New Response
            With Responser : .Copy(Settings.Responser) : .Error = EDP.ThrowException : End With

            Dim NextPage$
            Dim r$
            Dim _LPage As Func(Of String) = Function() If(LatestPage.IsEmptyString, String.Empty, $"page-{LatestPage}")

            Do
                r = Responser.GetResponse($"https://www.lpsg.com/threads/{Name}/{_LPage.Invoke}")
                UserExists = True
                UserSuspended = False
                Thrower.ThrowAny()
                If Not r.IsEmptyString Then
                    NextPage = RegexReplace(r, NextPageRegex)
                    UpdateMediaList(RegexReplace(r, PhotoRegEx), Mode.Internal)
                    UpdateMediaList(RegexReplace(r, PhotoRegExExt), Mode.External)
                    If NextPage = LatestPage Or NextPage.IsEmptyString Then Exit Do Else LatestPage = NextPage
                Else
                    Exit Do
                End If
            Loop

            If TempMediaList.ListExists And ExistingContentList.ListExists Then _
               TempMediaList.RemoveAll(Function(m) ExistingContentList.Exists(Function(mm) mm.URL = m.URL))
        Catch oex As OperationCanceledException
        Catch dex As ObjectDisposedException
        Catch ex As Exception
            If Responser.StatusCode = Net.HttpStatusCode.ServiceUnavailable Then
                LogProvider.Add("LPSG not available")
            Else
                LogProvider.Add(ex, $"[LPSG.UserData.GetMedia({Name})]")
            End If
        End Try
    End Sub
    Private Sub UpdateMediaList(ByVal l As List(Of String), ByVal m As Mode)
        If l.ListExists Then
            Dim f As SFile
            Dim u$
            Dim exists As Boolean
            Dim r As RParams
            Dim ude As New ErrorsDescriber(EDP.ReturnValue)
            For Each url$ In l
                If Not url.IsEmptyString Then u = SymbolsConverter.Decode(url, {Converters.HTML, Converters.ASCII}, ude) Else u = String.Empty
                If Not u.IsEmptyString Then
                    exists = Not IsEmptyString(RegexReplace(u, FileExistsRegEx))
                    If m = Mode.Internal Then
                        r = FileRegEx
                    Else
                        r = FileRegExExt
                        If Not exists Then
                            r = FileRegExExt2
                            exists = Not IsEmptyString(RegexReplace(u, FileRegExExt2))
                        End If
                    End If
                    If exists Then
                        f = CStr(RegexReplace(u, r))
                        f.Path = DataPath.CSFilePSN
                        f.Separator = "\"
                        If f.Extension.IsEmptyString Then f.Extension = "jpg"
                        TempMediaList.ListAddValue(New PluginUserMedia With {.ContentType = UTypes.Picture, .URL = url, .File = f}, TempListAddParams)
                    End If
                End If
            Next
        End If
    End Sub
    Public Sub Download() Implements IPluginContentProvider.Download
        Try
            With Responser : .UseWebClient = True : .UseWebClientCookies = True : .ResetError() : End With
            If TempMediaList.ListExists Then
                Dim m As PluginUserMedia
                Dim eweb As ErrorsDescriber = EDP.ThrowException
                RaiseEvent TotalCountChanged(TempMediaList.Count)
                For i% = 0 To TempMediaList.Count - 1
                    Thrower.ThrowAny()
                    m = TempMediaList(i)
                    m.DownloadState = UStates.Tried
                    Try
                        If Not m.URL.IsEmptyString And Not m.File.IsEmptyString Then
                            Responser.DownloadFile(m.URL, m.File, eweb)
                            m.DownloadState = UStates.Downloaded
                        Else
                            m.DownloadState = UStates.Skipped
                        End If
                    Catch wex As Exception
                        If Responser.Client.StatusCode = Net.HttpStatusCode.ServiceUnavailable Then
                            LogProvider.Add("LPSG not available")
                        Else
                            m.DownloadState = UStates.Missing
                            m.Attempts += 1
                        End If
                    End Try
                    RaiseEvent ProgressChanged(1)
                    TempMediaList(i) = m
                Next
            End If
        Catch oex As OperationCanceledException
        Catch dex As ObjectDisposedException
        Catch ex As Exception
            LogProvider.Add(ex, "[LPSG.UserData.Download]")
        End Try
    End Sub
#Region "IDisposable Support"
    Private disposedValue As Boolean = False
    Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                If Not Responser Is Nothing Then Responser.Dispose()
                If ExistingContentList.ListExists Then ExistingContentList.Clear()
                If TempPostsList.ListExists Then TempPostsList.Clear()
                If TempMediaList.ListExists Then TempMediaList.Clear()
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