' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports SCrawler.API.Base
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Functions.Messaging
Imports ECI = PersonalUtilities.Forms.Toolbars.EditToolbar.ControlItem
Namespace DownloadObjects
    Friend Class MissingPostsForm
#Region "Declarations"
        Private WithEvents MyDefs As DefaultFormOptions
        Private ReadOnly MUsers As List(Of IUserData)
        Private WithEvents BTT_DELETE_ALL As ToolStripButton
        Private WithEvents BTT_DOWN_ALL As ToolStripButton
        Private WithEvents BTT_INFO As ToolStripButton
#End Region
#Region "Initializer"
        Public Sub New()
            InitializeComponent()
            MUsers = New List(Of IUserData)
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            BTT_DELETE_ALL = New ToolStripButton With {
                .Text = "Delete ALL",
                .ToolTipText = String.Empty,
                .AutoToolTip = False,
                .Image = PersonalUtilities.My.Resources.DeletePic_Red_24,
                .DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
            }
            BTT_DOWN_ALL = New ToolStripButton With {
                .Text = "Download ALL",
                .ToolTipText = String.Empty,
                .AutoToolTip = False,
                .Image = My.Resources.StartPic_Green_16,
                .DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
            }
            BTT_INFO = New ToolStripButton With {
                .Text = "Info",
                .ToolTipText = "Show information about the missing post (F1)",
                .AutoToolTip = True,
                .Image = My.Resources.InfoPic_32,
                .DisplayStyle = ToolStripItemDisplayStyle.ImageAndText
            }
        End Sub
#End Region
#Region "Form handlers"
        Private Sub MissingPostsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize()
                .AddEditToolbar({ECI.Update, ECI.Separator, ECI.Delete, BTT_DELETE_ALL, ECI.Separator, BTT_DOWN_ALL, BTT_INFO})
                .EndLoaderOperations(False)
            End With
            RefillList()
        End Sub
        Private Sub MissingPostsForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            e.Cancel = True
            Hide()
        End Sub
        Private Sub MissingPostsForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            MUsers.Clear()
        End Sub
        Private Sub MissingPostsForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            If e.KeyCode = Keys.F1 Then ShowPostInformation() : e.Handled = True
        End Sub
#End Region
#Region "RefillList"
        Private Overloads Sub RefillList() Handles MyDefs.ButtonUpdateClick
            Try
                MUsers.Clear()
                LIST_DATA.Items.Clear()
                LIST_DATA.Groups.Clear()
                If Settings.Users.Count > 0 Then
                    MUsers.ListAddList(Settings.Users.SelectMany(Function(ByVal user As IUserData) As IEnumerable(Of IUserData)
                                                                     DirectCast(user, UserDataBase).LoadContentInformation()
                                                                     If user.IsCollection Then
                                                                         With DirectCast(user, API.UserDataBind)
                                                                             If .Count > 0 Then Return .Where(Function(u) DirectCast(u, UserDataBase).ContentMissingExists)
                                                                         End With
                                                                     ElseIf DirectCast(user, UserDataBase).ContentMissingExists Then
                                                                         Return {user}
                                                                     End If
                                                                     Return New IUserData() {}
                                                                 End Function), LAP.IgnoreICopier)
                End If
                If MUsers.Count > 0 Then
                    Dim gName$ = String.Empty
                    Dim g As ListViewGroup = Nothing
                    Dim i% = -1
                    Dim cm As List(Of UserMedia)
                    For Each uu As UserDataBase In MUsers
                        i += 1
                        cm = uu.ContentMissing
                        If cm.Count > 0 Then
                            gName = String.Empty
                            If uu.IncludedInCollection Then gName = $"{uu.CollectionName} - "
                            gName &= $"{uu.User.Name} ({uu.Site})"
                            ControlInvoke(LIST_DATA, Sub()
                                                         LIST_DATA.Groups.Add(New ListViewGroup(gName) With {.Tag = uu.LVIKey})
                                                         g = LIST_DATA.Groups(LIST_DATA.Groups.Count - 1)
                                                     End Sub)
                            For i% = 0 To cm.Count - 1 : ControlInvoke(LIST_DATA, Sub() LIST_DATA.Items.Add(New ListViewItem(cm(i).Post.ID, g))) : Next
                        End If
                    Next
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadObjects.MissingPostsForm.RefillList]")
            End Try
        End Sub
#End Region
#Region "Post actions"
        Private Sub BTT_DOWN_ALL_Click(sender As Object, e As EventArgs) Handles BTT_DOWN_ALL.Click
            Try
                Const MsgTitle$ = "Download users"
                If MUsers.Count > 0 Then
                    If MsgBoxE({$"You are trying to download missing posts of {MUsers.Count} user(s).", MsgTitle}, vbExclamation,,, {"Process", "Cancel"}) = 0 Then
                        MUsers.ForEach(Sub(u) u.DownloadMissingOnly = True)
                        Downloader.AddRange(MUsers, True)
                    Else
                        MsgBoxE({"Operation canceled", MsgTitle})
                    End If
                Else
                    MsgBoxE({"No users found", MsgTitle}, vbExclamation)
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadObjects.MissingPostsForm.DownloadAll]")
            End Try
        End Sub
        Private Sub BTT_DOWN_Click(sender As Object, e As EventArgs) Handles BTT_DOWN.Click
            Try
                Const MsgTitle$ = "Download users"
                If LIST_DATA.SelectedItems.Count > 0 Then
                    Dim users As List(Of IUserData) = LIST_DATA.SelectedItems.ToObjectsList.ListCast(Of ListViewItem)().
                                                      Select(Function(d) Settings.GetUser(CStr(d.Group.Tag))).ListWithRemove(Function(d) d Is Nothing)
                    If users.ListExists Then
                        If MsgBoxE({"The following users will be added to the download queue:" & vbCr & vbCr &
                                    users.Select(Function(u) u.ToString).ListToString(vbNewLine), MsgTitle},,,, {"Process", "Cancel"}) = 0 Then
                            users.ForEach(Sub(u) u.DownloadMissingOnly = True)
                            Downloader.AddRange(users, True)
                            users.Clear()
                        End If
                    End If
                Else
                    MsgBoxE({"No selected posts", MsgTitle})
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadObjects.MissingPostsForm.Download]")
            End Try
        End Sub
        Private Sub BTT_OPEN_POST_Click(sender As Object, e As EventArgs) Handles BTT_OPEN_POST.Click
            Try
                If LIST_DATA.SelectedItems.Count > 0 Then
                    If LIST_DATA.SelectedItems.Count = 1 OrElse
                       MsgBoxE({$"Are you sure you want to open {LIST_DATA.SelectedItems.Count} posts?", "Open multiple posts"}, vbExclamation + vbYesNo) = vbYes Then
                        Dim data As List(Of ListViewItem) = LIST_DATA.SelectedItems.ToObjectsList.ListCast(Of ListViewItem)
                        If data.ListExists Then
                            Dim uKey$, url$
                            Dim u As IUserData = Nothing
                            Dim i%
                            Dim cm As List(Of UserMedia)
                            For Each _d In data
                                uKey = _d.Group.Tag
                                If u Is Nothing OrElse Not u.Key = uKey Then u = Settings.GetUser(uKey)
                                If Not u Is Nothing Then
                                    i = -1
                                    With DirectCast(u, UserDataBase)
                                        cm = .ContentMissing
                                        If cm.Count > 0 Then i = cm.FindIndex(Function(c) c.Post.ID = _d.Text)
                                        If i >= 0 Then
                                            url = UserDataBase.GetPostUrl(u, cm(i))
                                            If Not url.IsEmptyString Then
                                                Try : Process.Start(url) : Catch : End Try
                                            End If
                                        End If
                                    End With
                                End If
                            Next
                        End If
                    End If
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadObjects.MissingPostsForm.OpenPost]")
            End Try
        End Sub
        Private Sub BTT_OPEN_USER_Click(sender As Object, e As EventArgs) Handles BTT_OPEN_USER.Click
            Try
                If LIST_DATA.SelectedItems.Count > 0 Then
                    Dim users As List(Of IUserData) = LIST_DATA.SelectedItems.ToObjectsList.ListCast(Of ListViewItem)().
                                                      Select(Function(d) Settings.GetUser(CStr(d.Group.Tag))).ListWithRemove(Function(d) d Is Nothing)
                    If users.ListExists Then users.ForEach(Sub(u) u.OpenFolder())
                Else
                    MsgBoxE("No selected posts")
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"[DownloadObjects.MissingPostsForm.OpenUser]")
            End Try
        End Sub
        Private Sub ShowPostInformation() Handles BTT_INFO.Click, BTT_CONTEXT_SHOW_POST_INFO.Click, LIST_DATA.DoubleClick
            Try
                If LIST_DATA.SelectedItems.Count > 0 Then
                    Dim data As ListViewItem = LIST_DATA.SelectedItems.ToObjectsList.ListCast(Of ListViewItem)().First
                    Dim uKey$, url$
                    Dim u As IUserData = Nothing
                    Dim i%
                    Dim cm As List(Of UserMedia)
                    Dim m As UserMedia
                    uKey = data.Group.Tag
                    If Not uKey.IsEmptyString Then u = Settings.GetUser(uKey)
                    If Not u Is Nothing Then
                        i = -1
                        With DirectCast(u, UserDataBase)
                            cm = .ContentMissing
                            If cm.Count > 0 Then i = cm.FindIndex(Function(c) c.Post.ID = data.Text)
                            If i >= 0 Then
                                m = cm(i)
                                url = UserDataBase.GetPostUrl(u, m)
                                Dim msg As New MMessage("", "Post information") With {.Editable = True}
                                Dim b As New List(Of MsgBoxButton)
                                If Not url.IsEmptyString Then b.Add(New MsgBoxButton("Open") With {.IsDialogResultButton = False,
                                                                                                   .ToolTip = "Open post in browser",
                                                                                                   .KeyCode = Keys.F1,
                                                                                                   .CallBack = Sub(result, message, button)
                                                                                                                   Try : Process.Start(url) : Catch : End Try
                                                                                                               End Sub})
                                b.Add(New MsgBoxButton("OK"))
                                With msg
                                    .Buttons = b
                                    .DefaultButton = If(b.Count = 2, 1, 0)
                                    .CancelButton = .DefaultButton
                                    .Text = $"Type: {m.Type}"
                                    .Text.StringAppendLine($"Address: {url}")
                                    If m.Post.Date.HasValue Then .Text.StringAppendLine($"Date: {m.Post.Date.Value.ToStringDate(ADateTime.Formats.BaseDateTime)}")
                                    .Text &= vbNewLine.StringDup(2)
                                    If u.IncludedInCollection Then .Text.StringAppendLine($"User collection: {u.CollectionName}")
                                    .Text.StringAppendLine($"User site: {u.Site}")
                                    .Text.StringAppendLine($"User name: {IIf(Not u.FriendlyName.IsEmptyString And Not u.IncludedInCollection, u.FriendlyName, u.Name)}")
                                End With
                                MsgBoxE(msg)
                                b.Clear()
                                cm.Clear()
                            End If
                        End With
                    End If
                Else
                    MsgBoxE("No selected posts")
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadObjects.MissingPostsForm.ShowPostInformation]")
            End Try
        End Sub
        Private Sub BTT_FIND_USER_Click(sender As Object, e As EventArgs) Handles BTT_FIND_USER.Click
            Try
                If LIST_DATA.SelectedItems.Count > 0 Then
                    Dim user As IUserData = LIST_DATA.SelectedItems.ToObjectsList.ListCast(Of ListViewItem)().
                                            Select(Function(d) Settings.GetUser(CStr(d.Group.Tag))).ListWithRemove(Function(d) d Is Nothing).
                                            DefaultIfEmpty(Nothing).First
                    If Not user Is Nothing Then MainFrameObj.FocusUser(user.Key, True)
                Else
                    MsgBoxE("No selected posts")
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"[DownloadObjects.MissingPostsForm.FindUser]")
            End Try
        End Sub
        Private Sub DeletePost(ByVal Sender As Object, ByVal e As EventArgs) Handles MyDefs.ButtonDeleteClickE, BTT_DELETE.Click, BTT_DELETE_ALL.Click
            Const MsgTitle$ = "Remove missing posts"
            Dim UsersToUpdate As New List(Of UserDataBase)
            Try
                Dim data As List(Of ListViewItem)
                Dim isAll As Boolean = Sender Is BTT_DELETE_ALL
                If isAll Then
                    data = LIST_DATA.Items.ToObjectsList.ListCast(Of ListViewItem)
                Else
                    data = LIST_DATA.SelectedItems.ToObjectsList.ListCast(Of ListViewItem)
                End If
                If data.ListExists Then
                    Dim lp As New ListAddParams(LAP.NotContainsOnly)
                    Dim usersCount% = ListAddList(Nothing, data.Select(Function(d) d.Group.Header), LAP.NotContainsOnly).ListIfNothing.Count
                    If MsgBoxE({"Are you sure you want to delete the selected missing posts?" & vbCr &
                                $"Number of affected users: {usersCount}." & vbCr &
                                $"Number of posts to be deleted: {data.Count}.", MsgTitle}, vbExclamation,,, {"Process", "Cancel"}) = 0 Then
                        Dim uKey$
                        Dim u As UserDataBase = Nothing
                        Dim cm As List(Of UserMedia)
                        Dim i%
                        For Each _d In data
                            uKey = _d.Group.Tag
                            If u Is Nothing OrElse Not u.LVIKey = uKey Then u = Settings.GetUser(uKey)
                            If Not u Is Nothing Then
                                i = -1
                                cm = u.ContentMissing
                                If cm.Count > 0 Then i = cm.FindIndex(Function(c) c.Post.ID = _d.Text)
                                If i >= 0 Then u.RemoveMedia(cm(i), UserMedia.States.Missing) : UsersToUpdate.ListAddValue(u, lp)
                            End If
                        Next
                        MsgBoxE({"The selected posts have been successfully deleted", MsgTitle})
                    Else
                        MsgBoxE({"Operation canceled", MsgTitle})
                    End If
                Else
                    MsgBoxE({IIf(isAll, "No posts found to delete", "No selected posts"), MsgTitle})
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadObjects.MissingPostsForm.DeletePost]")
            Finally
                If UsersToUpdate.Count > 0 Then
                    UpdateUsers(UsersToUpdate)
                    UsersToUpdate.Clear()
                    RefillList()
                End If
            End Try
        End Sub
        Private Sub UpdateUsers(ByVal UserList As List(Of UserDataBase))
            Try
                If UserList.ListExists Then UserList.ForEach(Sub(u) u.UpdateContentInformation())
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadObjects.MissingPostsForm.UpdateUsers]")
            End Try
        End Sub
#End Region
    End Class
End Namespace