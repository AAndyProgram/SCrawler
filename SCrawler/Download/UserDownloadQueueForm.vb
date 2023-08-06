' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports System.ComponentModel
Imports SCrawler.API.Base
Imports PersonalUtilities.Forms
Namespace DownloadObjects
    Friend Class UserDownloadQueueForm
        Private ReadOnly MyVew As FormView
        Private ReadOnly Tokens As List(Of CancellationTokenSource)
        Private Structure ListUser
            Friend ReadOnly User As UserDataBase
            Friend IsDownloading As Boolean
            Private ReadOnly _UserString As String
            Private ReadOnly Property UserString As String
                Get
                    Return $"[{IIf(IsDownloading, "-", "+")}] {_UserString}"
                End Get
            End Property
            Friend ReadOnly Key As String
            Friend Sub New(ByVal _User As IUserData)
                User = _User
                Key = _User.Key
                IsDownloading = True
                _UserString = DirectCast(User, UserDataBase).ToStringForLog()
                If Not User.FriendlyName.IsEmptyString Then _UserString &= $" ({User.FriendlyName})"
            End Sub
            Public Shared Widening Operator CType(ByVal _User As UserDataBase) As ListUser
                Return New ListUser(_User)
            End Operator
            Public Shared Widening Operator CType(ByVal _User As ListUser) As String
                Return _User.ToString
            End Operator
            Public Overrides Function ToString() As String
                Return UserString
            End Function
            Public Overrides Function Equals(ByVal Obj As Object) As Boolean
                Try : Return Not IsNothing(Obj) AndAlso TypeOf Obj Is ListUser AndAlso Key.Equals(CType(Obj, ListUser).Key) : Catch : End Try
                Return False
            End Function
        End Structure
        Public Sub New()
            InitializeComponent()
            MyVew = New FormView(Me, Settings.Design)
            Tokens = New List(Of CancellationTokenSource)
        End Sub
        Private Sub UserDownloadQueueForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                MyVew.Import()
                MyVew.SetFormSize()
                With Downloader
                    .QueueFormOpening = True
                    If .ActiveDownloading.Count > 0 Then
                        For Each user As UserDataBase In .ActiveDownloading
                            ApplyHandlers(user, user.DownloadInProgress)
                            LIST_QUEUE.Items.Add(New ListUser(user))
                        Next
                    End If
                    AddHandler .UserDownloadStateChanged, AddressOf Downloader_UserDownloadStateChanged
                    AddHandler .Downloading, AddressOf Downloader_Downloading
                    .QueueFormOpening = False
                End With
            Catch aoutex As ArgumentOutOfRangeException
            Catch iex As IndexOutOfRangeException
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog + EDP.ShowMainMsg, ex, "Error when opening user download queue form")
            Finally
                Downloader.QueueFormOpening = False
            End Try
        End Sub
        Private Sub UserDownloadQueueForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            e.Cancel = True
            Hide()
        End Sub
        Private Sub UserDownloadQueueForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            MyVew.Dispose()
            Tokens.ListClearDispose
        End Sub
        Private Sub UserDownloadQueueForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            Dim b As Boolean = True
            If e.KeyCode = Keys.Delete Then
                CancelUserDownload()
            ElseIf e.KeyCode = Keys.F And e.Control Then
                FindUser()
            Else
                b = False
            End If
            If b Then e.Handled = True
        End Sub
        Private Sub Downloader_Downloading(ByVal Value As Boolean)
            ControlInvokeFast(LIST_QUEUE, Sub() If Not Value Then LIST_QUEUE.Items.Clear() : Tokens.ListClearDispose, EDP.None)
        End Sub
        Private Async Sub Downloader_UserDownloadStateChanged(ByVal User As IUserData, ByVal IsDownloading As Boolean)
            Await Task.Run(Sub()
                               Try
                                   ControlInvokeFast(LIST_QUEUE, Sub()
                                                                     Dim u As New ListUser(User)
                                                                     ApplyHandlers(User, IsDownloading)
                                                                     If IsDownloading Then
                                                                         LIST_QUEUE.Items.Add(u)
                                                                     Else
                                                                         LIST_QUEUE.Items.Remove(u)
                                                                     End If
                                                                     LIST_QUEUE.Refresh()
                                                                 End Sub)
                               Catch ex As Exception
                               End Try
                           End Sub)
        End Sub
        Private Async Sub User_UserDownloadStateChanged(ByVal User As IUserData, ByVal IsDownloading As Boolean)
            Await Task.Run(Sub()
                               Try
                                   ControlInvokeFast(LIST_QUEUE,
                                                     Sub()
                                                         Dim lu As New ListUser(User)
                                                         Dim i% = LIST_QUEUE.Items.IndexOf(lu)
                                                         If i >= 0 Then
                                                             lu = LIST_QUEUE.Items(i)
                                                             If Not lu.User Is Nothing And Not lu.IsDownloading = IsDownloading Then
                                                                 lu.IsDownloading = IsDownloading
                                                                 LIST_QUEUE.Items(i) = lu
                                                                 LIST_QUEUE.Refresh()
                                                             End If
                                                         End If
                                                     End Sub)
                               Catch
                               End Try
                           End Sub)
        End Sub
        Private Sub ApplyHandlers(ByVal User As IUserData, ByVal IsDownloading As Boolean)
            Try
                If Not User Is Nothing Then
                    With DirectCast(User, UserDataBase)
                        If IsDownloading Then
                            AddHandler .UserDownloadStateChanged, AddressOf User_UserDownloadStateChanged
                        Else
                            RemoveHandler .UserDownloadStateChanged, AddressOf User_UserDownloadStateChanged
                        End If
                    End With
                End If
            Catch
            End Try
        End Sub
        Private Sub CancelUserDownload()
            Const msgTitle$ = "Stop user download"
            Try
                Dim lu As ListUser = GetUserSelectedUser()
                If Not lu.User Is Nothing AndAlso
                   MsgBoxE({$"Are you sure you want to stop downloading the following user?{vbCr}{lu}", msgTitle}, vbExclamation + vbYesNo) = vbYes Then
                    Dim token As New CancellationTokenSource
                    lu.User.PersonalToken = token.Token
                    token.Cancel()
                    Tokens.Add(token)
                    MsgBoxE({"Cancel user download processed.", msgTitle})
                End If
            Catch ex As Exception
            End Try
        End Sub
        Private Sub FindUser()
            Try
                MainFrameObj.FocusUser(GetUserSelectedUser().Key, True)
            Catch ex As Exception
            End Try
        End Sub
        Private Function GetUserSelectedUser() As ListUser
            Dim lu As ListUser = Nothing
            ControlInvokeFast(LIST_QUEUE, Sub()
                                              Dim sIndx% = LIST_QUEUE.SelectedIndex
                                              If sIndx >= 0 Then lu = LIST_QUEUE.Items(sIndx)
                                          End Sub)
            Return lu
        End Function
    End Class
End Namespace