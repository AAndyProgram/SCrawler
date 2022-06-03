' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Tools.Notifications
Imports SCrawler.DownloadObjects.Groups
Imports SCrawler.API
Imports SCrawler.API.Base
Namespace DownloadObjects
    Friend Class AutoDownloader : Inherits GroupParameters
        Friend Event UserFind(ByVal Key As String, ByVal Activate As Boolean)
        Friend Enum Modes As Integer
            None = 0
            [Default] = 1
            All = 2
            Specified = 3
            Groups = 4
        End Enum
        Friend Const DefaultTimer As Integer = 60
#Region "Notifications"
        Private Const KeyOpenFolder As String = "_____OPEN_FOLDER_SCRAWLER_AUTOMATION"
        Private Const KeyOpenSite As String = "_____OPEN_SITE_SCRAWLER_AUTOMATION"
        Private Const KeyBttDismiss As String = "_____DISMISS_SCRAWLER_AUTOMATION"
        Private Const KeyBttPhoto As String = "_____PHOTO_SCRAWLER_AUTOMATION"
        Private ReadOnly UserKeys As List(Of NotifiedUser)
        Private Class NotifiedUser : Implements IDisposable
            Private ReadOnly Property User As IUserData
            Private ReadOnly Property Key As String
            Private ReadOnly Property KeyFolder As String
            Private ReadOnly Property KeySite As String
            Private ReadOnly Property KeyDismiss As String
            Private ReadOnly Property Images As Dictionary(Of String, SFile)
            Private Sub New()
                Images = New Dictionary(Of String, SFile)
            End Sub
            Friend Sub New(ByVal _Key As String)
                Me.New
                Key = _Key
                KeyFolder = $"{Key}{KeyOpenFolder}"
                KeySite = $"{Key}{KeyOpenSite}"
                KeyDismiss = $"{Key}{KeyBttDismiss}"
            End Sub
            Friend Sub New(ByVal _Key As String, ByRef _User As IUserData)
                Me.New(_Key)
                User = _User
            End Sub
            Public Shared Widening Operator CType(ByVal Key As String) As NotifiedUser
                Return New NotifiedUser(Key)
            End Operator
            Friend Sub ShowNotification()
                Try
                    If Not User Is Nothing Then
                        Dim Text$ = $"{User.Site} - {User.Name}{vbNewLine}" &
                                    $"Downloaded: {User.DownloadedPictures(False)} images, {User.DownloadedVideos(False)} videos"
                        Dim Title$
                        If Not User.CollectionName.IsEmptyString Then
                            Title = User.CollectionName
                        Else
                            Title = User.ToString
                        End If
                        Using Notify As New Notification(Text, Title) With {.Key = Key}
                            Dim uPic As SFile = DirectCast(User, UserDataBase).GetUserPictureAddress
                            Dim uif As SFile = Nothing
                            Dim uifKey$ = String.Empty
                            If uPic.Exists Then Notify.Images = {New ToastImage(uPic)}
                            If User.DownloadedPictures(False) > 0 Then
                                uif = DirectCast(User, UserDataBase).GetLastImageAddress
                                If uif.Exists Then
                                    Notify.Images = {New ToastImage(uif, IImage.Modes.Inline)}
                                    uifKey = $"{Key}_{Images.Keys.Count + 1}_{KeyBttPhoto}"
                                    If Not Images.ContainsKey(uifKey) Then Images.Add(uifKey, uif)
                                End If
                            End If
                            Notify.Buttons = {
                                New ToastButton(KeyFolder, "Folder"),
                                New ToastButton(KeySite, "Site")
                            }
                            If Not uifKey.IsEmptyString Then Notify.Buttons = {New ToastButton(uifKey, "Photo")}
                            Notify.Buttons = {New ToastButton(KeyDismiss, "Dismiss")}
                            Notify.Show()
                        End Using
                    End If
                Catch ex As Exception
                    ErrorsDescriber.Execute(EDP.SendInLog, ex, "[AutoDownloader.NotifiedUser.ShowNotification]")
                    If Not User Is Nothing Then
                        MainFrameObj.ShowNotification($"Downloaded: {User.DownloadedPictures(False)} images, {User.DownloadedVideos(False)} videos",
                                                      User.ToString, IIf(User.HasError, ToolTipIcon.Warning, ToolTipIcon.Info))
                    End If
                End Try
            End Sub
            ''' <returns>True to activate</returns>
            Friend Function Open(ByVal _Key As String) As Boolean
                If Not User Is Nothing Then
                    If Key = _Key Then
                        Return True
                    ElseIf KeyFolder = _Key Then
                        User.OpenFolder()
                    ElseIf KeySite = _Key Then
                        User.OpenSite()
                    ElseIf Images.ContainsKey(_Key) Then
                        Images(_Key).Open(, EDP.None)
                    End If
                End If
                Return False
            End Function
            Public Overrides Function Equals(ByVal Obj As Object) As Boolean
                With CType(Obj, NotifiedUser)
                    Return .Key = Key Or .Key = KeyFolder Or .Key = KeySite Or .Key = KeyDismiss Or Images.ContainsKey(.Key)
                End With
            End Function
#Region "IDisposable Support"
            Private disposedValue As Boolean = False
            Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
                If Not disposedValue Then
                    If disposing Then Images.Clear()
                    disposedValue = True
                End If
            End Sub
            Protected Overrides Sub Finalize()
                Dispose(False)
                MyBase.Finalize()
            End Sub
            Friend Overloads Sub Dispose() Implements IDisposable.Dispose
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
#End Region
        End Class
#End Region
#Region "XML Names"
        Private Const Name_Mode As String = "Mode"
        Private Const Name_Groups As String = "Groups"
        Private Const Name_Labels As String = "Labels"
        Private Const Name_Timer As String = "Timer"
        Private Const Name_LastDownloadDate As String = "LastDownloadDate"
        Private Const Name_ShowNotifications As String = "Notify"
#End Region
#Region "Declarations"
        Friend Property Mode As Modes = Modes.None
        Friend ReadOnly Property Groups As List(Of String)
        Friend Property Timer As Integer = DefaultTimer
        Friend Property ShowNotifications As Boolean = True
        Friend Property LastDownloadDate As Date = Now.AddYears(-1)
        Private ReadOnly DateProvider As New ADateTime(ADateTime.Formats.BaseDateTime)
        Private File As SFile = $"Settings\AutoDownload.xml"
        Private AThread As Thread
#End Region
#Region "Initializer"
        Friend Sub New()
            Groups = New List(Of String)
            UserKeys = New List(Of NotifiedUser)
            If File.Exists Then
                Using x As New XmlFile(File)
                    Mode = x.Value(Name_Mode).FromXML(Of Integer)(Modes.None)
                    Groups.ListAddList(x.Value(Name_Groups).StringToList(Of String)("|"), LAP.NotContainsOnly)
                    Labels.ListAddList(x.Value(Name_Labels).StringToList(Of String)("|"), LAP.NotContainsOnly)
                    Temporary = x.Value(Name_Temporary).FromXML(Of Integer)(CheckState.Indeterminate)
                    Favorite = x.Value(Name_Favorite).FromXML(Of Integer)(CheckState.Indeterminate)
                    ReadyForDownload = x.Value(Name_ReadyForDownload).FromXML(Of Boolean)(True)
                    ReadyForDownloadIgnore = x.Value(Name_ReadyForDownloadIgnore).FromXML(Of Boolean)(False)
                    Timer = x.Value(Name_Timer).FromXML(Of Integer)(DefaultTimer)
                    ShowNotifications = x.Value(Name_ShowNotifications).FromXML(Of Boolean)(True)
                    LastDownloadDate = AConvert(Of Date)(x.Value(Name_LastDownloadDate), DateProvider, Now.AddYears(-1))
                End Using
            End If
        End Sub
#End Region
#Region "Groups Support"
        Friend Sub GROUPS_Updated(ByVal Sender As DownloadGroup)
            If Groups.Count > 0 Then
                Dim i% = Groups.IndexOf(Sender.NameBefore)
                If i >= 0 Then Groups(i) = Sender.Name : Update()
            End If
        End Sub
        Friend Sub GROUPS_Deleted(ByVal Sender As DownloadGroup)
            If Groups.Count > 0 Then
                Dim i% = Groups.IndexOf(Sender.Name)
                If i >= 0 Then Groups.RemoveAt(i) : Update()
            End If
        End Sub
#End Region
#Region "Update"
        Friend Sub Update()
            Try
                Using x As New XmlFile With {.Name = "Settings"}
                    x.Add(Name_Mode, CInt(Mode))
                    x.Add(Name_Groups, Groups.ListToString("|"))
                    x.Add(Name_Labels, Labels.ListToString("|"))
                    x.Add(Name_Temporary, CInt(Temporary))
                    x.Add(Name_Favorite, CInt(Favorite))
                    x.Add(Name_ReadyForDownload, ReadyForDownload.BoolToInteger)
                    x.Add(Name_ReadyForDownloadIgnore, ReadyForDownloadIgnore.BoolToInteger)
                    x.Add(Name_Timer, Timer)
                    x.Add(Name_ShowNotifications, ShowNotifications.BoolToInteger)
                    x.Add(Name_LastDownloadDate, CStr(AConvert(Of String)(LastDownloadDate, DateProvider, String.Empty)))
                    x.Save(File)
                End Using
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendInLog, ex, "[AutoDownloader.Update]")
            End Try
        End Sub
#End Region
#Region "Execution"
        Friend Sub Start()
            If Not If(AThread?.IsAlive, False) And Not Mode = Modes.None Then
                AThread = New Thread(New ThreadStart(AddressOf Checker))
                AThread.SetApartmentState(ApartmentState.MTA)
                AThread.Start()
            End If
        End Sub
        Private _StopRequested As Boolean = False
        Friend Sub [Stop]()
            If If(AThread?.IsAlive, False) Then _StopRequested = True
        End Sub
        Private Sub Checker()
            Try
                While Not _StopRequested
                    If LastDownloadDate.AddMinutes(Timer) < Now And Not Downloader.Working Then Download()
                    Thread.Sleep(500)
                End While
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendInLog, ex, "[AutoDownloader.Checker]")
            Finally
                _StopRequested = False
            End Try
        End Sub
        Private Sub Download()
            Dim Keys As New List(Of String)
            Try
                Dim users As New List(Of IUserData)
                Dim GName$
                Dim i%
                Dim l As New ListAddParams(LAP.IgnoreICopier + LAP.NotContainsOnly)
                Dim notify As Action = Sub()
                                           With Downloader.Downloaded
                                               If ShowNotifications And .Count > 0 Then .ForEach(Sub(ByVal u As IUserData)
                                                                                                     If Keys.Contains(u.Key) Then
                                                                                                         ShowNotification(u)
                                                                                                         Keys.Remove(u.Key)
                                                                                                     End If
                                                                                                 End Sub)
                                           End With
                                       End Sub
                Select Case Mode
                    Case Modes.All : users.ListAddList(Settings.Users)
                    Case Modes.Default
                        Using g As New GroupParameters : users.ListAddList(DownloadGroup.GetUsers(g, True)) : End Using
                    Case Modes.Specified : users.ListAddList(DownloadGroup.GetUsers(Me, True))
                    Case Modes.Groups
                        If Groups.Count > 0 And Settings.Groups.Count > 0 Then
                            For Each GName In Groups
                                i = Settings.Groups.IndexOf(GName)
                                If i >= 0 Then users.ListAddList(Settings.Groups(i).GetUsers, l)
                            Next
                        End If
                End Select
                If users.Count > 0 Then
                    Keys.ListAddList(users.SelectMany(Of String)(Function(ByVal user As IUserData) As IEnumerable(Of String)
                                                                     If user.IsCollection Then
                                                                         With DirectCast(user, UserDataBind)
                                                                             If .Count > 0 Then
                                                                                 Return .Collections.Select(Function(u) u.Key)
                                                                             Else
                                                                                 Return New String() {}
                                                                             End If
                                                                         End With
                                                                     Else
                                                                         Return {user.Key}
                                                                     End If
                                                                 End Function))
                    With Downloader
                        .AutoDownloaderWorking = True
                        If .Downloaded.Count > 0 Then .Downloaded.RemoveAll(Function(u) Keys.Contains(u.Key)) : .InvokeDownloadsChangeEvent()
                        .AddRange(users)
                        .DisableOpenForms = False
                        While .Working Or .Count > 0 : notify.Invoke() : Thread.Sleep(200) : End While
                        .AutoDownloaderWorking = False
                        notify.Invoke
                    End With
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendInLog, ex, "[AutoDownloader.Download]")
            Finally
                Keys.Clear()
                LastDownloadDate = Now
                Update()
            End Try
        End Sub
        Private Sub ShowNotification(ByVal u As IUserData)
            Dim i% = UserKeys.IndexOf(u.Key)
            If i >= 0 Then
                UserKeys(i).ShowNotification()
            Else
                UserKeys.Add(New NotifiedUser(u.Key, TDownloader.GetUserFromMainCollection(u)))
                UserKeys.Last.ShowNotification()
            End If
        End Sub
        Friend Function NotificationClicked(ByVal Key As String) As Boolean
            Dim i% = UserKeys.IndexOf(Key)
            If i >= 0 Then
                RaiseEvent UserFind(Key, UserKeys(i).Open(Key))
                Return True
            Else
                Return False
            End If
        End Function
#End Region
#Region "IDisposable Support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then
                [Stop]()
                UserKeys.ListClearDispose()
                Groups.Clear()
            End If
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace