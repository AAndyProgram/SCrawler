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
        Private MyVew As FormView
        Private ReadOnly Tokens As List(Of CancellationTokenSource)
        Private Loaded As Boolean = False
        Private ReadOnly Pending As List(Of ListUser)
        Private Structure ListUser
            Friend IsDownloading As Boolean
            Private ReadOnly _UserString As String
            Private ReadOnly Property UserString As String
                Get
                    Return $"[{IIf(IsDownloading, "-", "+")}] {_UserString}"
                End Get
            End Property
            Friend ReadOnly Key As String
            Friend ReadOnly User As IUserData
            Friend Sub New(ByVal _User As IUserData)
                Try
                    User = _User
                    Key = _User.Key
                    IsDownloading = True
                    _UserString = DirectCast(_User, UserDataBase).ToStringForLog()
                    If Not _User.FriendlyName.IsEmptyString Then _UserString &= $" ({_User.FriendlyName})"
                Catch
                End Try
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
            Tokens = New List(Of CancellationTokenSource)
            Pending = New List(Of ListUser)
        End Sub
        Private Sub UserDownloadQueueForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                If MyVew Is Nothing Then
                    MyVew = New FormView(Me, Settings.Design)
                    MyVew.Import()
                    MyVew.SetFormSize()
                End If
            Catch
            Finally
                Loaded = True
                FillPending()
            End Try
        End Sub
        Private Sub FillPending()
            Try
                If Pending.Count > 0 Then Pending.ForEach(Sub(u) Downloader_UserDownloadStateChanged(u.User, u.IsDownloading)) : Pending.Clear()
            Catch
            End Try
        End Sub
        Private Sub UserDownloadQueueForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            e.Cancel = True
            Hide()
        End Sub
        Private Sub UserDownloadQueueForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            MyVew.DisposeIfReady
            Tokens.ListClearDispose
            Pending.Clear()
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
        Friend Sub Downloader_Downloading(ByVal Value As Boolean)
            Try
                If Not Value Then
                    If Not Loaded Then
                        Pending.Clear()
                    Else
                        ControlInvokeFast(LIST_QUEUE, Sub()
                                                          LIST_QUEUE.Items.Clear()
                                                          Tokens.ListClearDispose
                                                      End Sub, EDP.None)
                    End If
                End If
            Catch
            End Try
        End Sub
        Friend Sub Downloader_UserDownloadStateChanged(ByVal User As IUserData, ByVal IsDownloading As Boolean)
            Try
                If Not Loaded Then
                    Dim newUser As New ListUser(User)
                    If IsDownloading Then
                        If Pending.Count = 0 OrElse Not Pending.Contains(newUser) Then Pending.Add(newUser)
                    Else
                        If Pending.Count > 0 Then Pending.Remove(newUser)
                    End If
                Else
                    ControlInvokeFast(LIST_QUEUE, Sub()
                                                      Dim u As New ListUser(User)
                                                      ApplyHandlers(User, IsDownloading)
                                                      If IsDownloading Then
                                                          LIST_QUEUE.Items.Add(u)
                                                      Else
                                                          LIST_QUEUE.Items.Remove(u)
                                                      End If
                                                      LIST_QUEUE.Refresh()
                                                  End Sub, EDP.None)
                End If
            Catch
            End Try
        End Sub
        Private Sub User_UserDownloadStateChanged(ByVal User As IUserData, ByVal IsDownloading As Boolean)
            Try
                If Not Loaded Then
                    Dim __user As New ListUser(User)
                    If Pending.Count > 0 Then
                        Dim uIndx% = Pending.IndexOf(__user)
                        If uIndx >= 0 Then
                            __user.IsDownloading = IsDownloading
                            Pending(uIndx) = __user
                        End If
                    End If
                Else
                    ControlInvokeFast(LIST_QUEUE, Sub()
                                                      Dim lu As New ListUser(User)
                                                      Dim i% = LIST_QUEUE.Items.IndexOf(lu)
                                                      If i >= 0 Then
                                                          lu = LIST_QUEUE.Items(i)
                                                          If Not lu.Key.IsEmptyString And Not lu.IsDownloading = IsDownloading Then
                                                              lu.IsDownloading = IsDownloading
                                                              LIST_QUEUE.Items(i) = lu
                                                              LIST_QUEUE.Refresh()
                                                          End If
                                                      End If
                                                  End Sub, EDP.None)
                End If
            Catch
            End Try
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
                If Not lu.Key.IsEmptyString AndAlso
                   MsgBoxE({$"Are you sure you want to stop downloading the following user?{vbCr}{lu}", msgTitle}, vbExclamation + vbYesNo) = vbYes Then
                    Dim token As New CancellationTokenSource
                    Dim u As IUserData = Settings.GetUser(lu.Key)
                    If Not u Is Nothing Then
                        DirectCast(u, UserDataBase).TokenPersonal = token.Token
                        token.Cancel()
                        Tokens.Add(token)
                    End If
                End If
            Catch
            End Try
        End Sub
        Private Sub FindUser()
            Try : MainFrameObj.FocusUser(GetUserSelectedUser().Key, True) : Catch : End Try
        End Sub
        Private Function GetUserSelectedUser() As ListUser
            Try
                Dim lu As ListUser = Nothing
                ControlInvokeFast(LIST_QUEUE, Sub()
                                                  Dim sIndx% = LIST_QUEUE.SelectedIndex
                                                  If sIndx >= 0 Then lu = LIST_QUEUE.Items(sIndx)
                                              End Sub, EDP.None)
                Return lu
            Catch
                Return Nothing
            End Try
        End Function
    End Class
End Namespace