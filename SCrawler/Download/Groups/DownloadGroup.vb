' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Namespace DownloadObjects.Groups
    Friend Class DownloadGroup : Inherits GroupParameters : Implements IIndexable, IEContainerProvider
#Region "Events"
        Friend Delegate Sub GroupEventHandler(ByVal Sender As DownloadGroup)
        Friend Event Deleted As GroupEventHandler
        Friend Event Updated As GroupEventHandler
#End Region
#Region "Declarations"
#Region "Controls"
        Private WithEvents BTT_EDIT As ToolStripMenuItem
        Private WithEvents BTT_DELETE As ToolStripMenuItem
        Private WithEvents BTT_DOWNLOAD As ToolStripMenuItem
        Private WithEvents BTT_DOWNLOAD_FULL As ToolStripMenuItem
        Private ReadOnly SEP_1 As ToolStripSeparator
        Private WithEvents BTT_MENU As ToolStripMenuItem
#End Region
        Friend Property NameBefore As String = String.Empty
        Private _Key As String = String.Empty
        Friend ReadOnly Property Key As String
            Get
                If _Key.IsEmptyString Then _Key = $"{Name}{Index}"
                Return _Key
            End Get
        End Property
        Private _Index As Integer = 0
        Friend Property Index As Integer Implements IIndexable.Index
            Get
                Return _Index
            End Get
            Set(ByVal NewIndex As Integer)
                Dim b As Boolean = Not _Index = NewIndex
                _Index = NewIndex
                If b Then RaiseEvent Updated(Me)
            End Set
        End Property
        Private Function SetIndex(ByVal Obj As Object, ByVal _Index As Integer) As Object Implements IIndexable.SetIndex
            DirectCast(Obj, DownloadGroup).Index = _Index
            Return Obj
        End Function
#End Region
#Region "Initializers"
        Friend ReadOnly NeedToSave As Boolean = False
        Friend Sub New()
            BTT_MENU = New ToolStripMenuItem With {
                .ToolTipText = "Download users of this group",
                .AutoToolTip = True,
                .Image = My.Resources.GroupByIcon_16.ToBitmap
            }
            BTT_DELETE = New ToolStripMenuItem With {
                .Image = PersonalUtilities.My.Resources.DeletePic_Red_24,
                .BackColor = MyColor.DeleteBack,
                .ForeColor = MyColor.DeleteFore,
                .Text = "Delete",
                .ToolTipText = String.Empty,
                .AutoToolTip = False
            }
            BTT_EDIT = New ToolStripMenuItem With {
                .Image = PersonalUtilities.My.Resources.PencilPic_16,
                .BackColor = MyColor.EditBack,
                .ForeColor = MyColor.EditFore,
                .Text = "Edit",
                .ToolTipText = String.Empty,
                .AutoToolTip = False
            }
            SEP_1 = New ToolStripSeparator
            BTT_DOWNLOAD = New ToolStripMenuItem With {
                .Image = My.Resources.StartPic_Green_16,
                .Text = "Download",
                .ToolTipText = "Download users of this group (respect the 'Ready for download' parameter)",
                .AutoToolTip = True
            }
            BTT_DOWNLOAD_FULL = New ToolStripMenuItem With {
                .Image = My.Resources.StartPic_Green_16,
                .Text = "Download FULL",
                .ToolTipText = "Download users of this group (ignore the 'Ready for download' parameter)",
                .AutoToolTip = True
            }
            BTT_MENU.DropDownItems.AddRange({BTT_EDIT, BTT_DELETE, SEP_1, BTT_DOWNLOAD, BTT_DOWNLOAD_FULL})
        End Sub
        Friend Sub New(ByVal e As EContainer)
            Me.New
            Import(e)
        End Sub
#End Region
#Region "ToString"
        Public Overrides Function ToString() As String
            Return $"{IIf(Index.ValueBetween(0, 8), $"#{Index + 1}: ", String.Empty)}{Name}"
        End Function
#End Region
#Region "GetControl"
        Private _ControlSent As Boolean = False
        Friend Function GetControl() As ToolStripMenuItem
            If Not _ControlSent Then
                BTT_MENU.Text = ToString()
                BTT_MENU.Tag = Key
                _ControlSent = True
            End If
            Return BTT_MENU
        End Function
#End Region
#Region "Buttons"
        Private Sub BTT_MENU_Click(sender As Object, e As EventArgs) Handles BTT_MENU.Click
            DownloadUsers(True)
        End Sub
        Private Sub BTT_EDIT_Click(sender As Object, e As EventArgs) Handles BTT_EDIT.Click
            Using f As New GroupEditorForm(Me)
                f.ShowDialog()
                If f.DialogResult = DialogResult.OK Then RaiseEvent Updated(Me)
            End Using
        End Sub
        Private Sub BTT_DELETE_Click(sender As Object, e As EventArgs) Handles BTT_DELETE.Click
            If MsgBoxE({$"Are you sure you want to delete the [{Name}] group?", "Deleting a group"}, vbExclamation + vbYesNo) = vbYes Then
                RaiseEvent Deleted(Me)
                MsgBoxE({$"Group [{Name}] deleted", "Deleting a group"})
            End If
        End Sub
        Private Sub BTT_DOWNLOAD_Click(sender As Object, e As EventArgs) Handles BTT_DOWNLOAD.Click
            DownloadUsers(True)
        End Sub
        Private Sub BTT_DOWNLOAD_FULL_Click(sender As Object, e As EventArgs) Handles BTT_DOWNLOAD_FULL.Click
            DownloadUsers(False)
        End Sub
#End Region
#Region "Get users"
        Friend Overloads Function GetUsers() As IEnumerable(Of IUserData)
            Return GetUsers(Me, True)
        End Function
        Friend Overloads Shared Function GetUsers(ByVal Instance As IGroup, ByVal UseReadyOption As Boolean) As IEnumerable(Of IUserData)
            Try
                If Settings.Users.Count > 0 Then
                    With Instance
                        Dim CheckParams As Predicate(Of IUserData) = Function(user) _
                            (.Temporary = CheckState.Indeterminate Or user.Temporary = CBool(.Temporary)) And
                            (.Favorite = CheckState.Indeterminate Or (user.Favorite = CBool(.Favorite))) And
                            (Not UseReadyOption Or .ReadyForDownloadIgnore Or user.ReadyForDownload = .ReadyForDownload) And user.Exists
                        Dim CheckSubscription As Predicate(Of IUserData) = Function(ByVal user As IUserData) As Boolean
                                                                               If .Subscriptions Then
                                                                                   If .SubscriptionsOnly Then
                                                                                       Return user.IsSubscription = True
                                                                                   Else
                                                                                       Return True
                                                                                   End If
                                                                               Else
                                                                                   Return user.IsSubscription = False
                                                                               End If
                                                                           End Function
                        Dim CheckLabelsExcluded As Predicate(Of IUserData) = Function(ByVal user As IUserData) As Boolean
                                                                                 If .LabelsExcluded.Count = 0 Then
                                                                                     Return True
                                                                                 ElseIf user.Labels.Count = 0 Then
                                                                                     Return True
                                                                                 Else
                                                                                     Return Not user.Labels.ListContains(.LabelsExcluded)
                                                                                 End If
                                                                             End Function
                        Dim CheckLabels As Predicate(Of IUserData) = Function(ByVal user As IUserData) As Boolean
                                                                         If .Labels.Count = 0 Then
                                                                             Return CheckLabelsExcluded.Invoke(user)
                                                                         ElseIf user.Labels.Count = 0 Then
                                                                             Return False
                                                                         Else
                                                                             Return user.Labels.ListContains(.Labels) And CheckLabelsExcluded.Invoke(user)
                                                                         End If
                                                                     End Function
                        Dim CheckSites As Predicate(Of IUserData) = Function(user) _
                            (.Sites.Count = 0 OrElse .Sites.Contains(user.Site)) AndAlso
                            (.SitesExcluded.Count = 0 OrElse Not .SitesExcluded.Contains(user.Site))
                        Dim users As IEnumerable(Of IUserData) =
                            Settings.GetUsers(Function(user) CheckLabels.Invoke(user) AndAlso CheckSites.Invoke(user) AndAlso
                                                             CheckParams.Invoke(user) AndAlso CheckSubscription.Invoke(user))
                        If .UsersCount = 0 Or Not users.ListExists Then
                            Return users
                        Else
                            users = users.ListTake(If(.UsersCount > 0, -1, -2), Math.Abs(.UsersCount))
                            If .UsersCount < 0 Then users = users.ListReverse
                            Return users
                        End If
                    End With
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadGroup.GetUsers]")
            End Try
        End Function
#End Region
#Region "Download users"
        Friend Sub DownloadUsers(ByVal UseReadyOption As Boolean)
            Try
                If Settings.Users.Count > 0 Then
                    Dim u As IEnumerable(Of IUserData) = GetUsers(Me, UseReadyOption)
                    If u.ListExists Then
                        Downloader.AddRange(u, True)
                    Else
                        MsgBoxE({$"No users found for group [{Name}].", "No users found"}, vbExclamation)
                    End If
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadGroup.DownloadUsers]")
            End Try
        End Sub
#End Region
#Region "IEContainerProvider Support"
        Private Function ToEContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
            Return Export(New EContainer("Group"))
        End Function
#End Region
#Region "IDisposable Support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then
                BTT_DELETE.Dispose()
                BTT_EDIT.Dispose()
                BTT_MENU.Dispose()
                SEP_1.Dispose()
                BTT_DOWNLOAD.Dispose()
                BTT_DOWNLOAD_FULL.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace