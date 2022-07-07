' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports SCrawler.API
Imports SCrawler.API.Base
Namespace DownloadObjects.Groups
    Friend Class DownloadGroup : Inherits GroupParameters : Implements IIndexable, IEContainerProvider
        Friend Delegate Sub GroupEventHandler(ByVal Sender As DownloadGroup)
        Friend Event Deleted As GroupEventHandler
        Friend Event Updated As GroupEventHandler
        Private WithEvents BTT_EDIT As ToolStripMenuItem
        Private WithEvents BTT_DELETE As ToolStripMenuItem
        Private WithEvents BTT_DOWNLOAD As ToolStripMenuItem
        Private WithEvents BTT_DOWNLOAD_FULL As ToolStripMenuItem
        Private ReadOnly SEP_1 As ToolStripSeparator
        Private WithEvents BTT_MENU As ToolStripMenuItem
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
        Friend Sub New()
            BTT_MENU = New ToolStripMenuItem With {
                .ToolTipText = "Download users of this group",
                .AutoToolTip = True,
                .Image = My.Resources.GroupBy_284.ToBitmap
            }
            BTT_DELETE = New ToolStripMenuItem With {
                .Image = PersonalUtilities.My.Resources.DeletePic_02_Red_24,
                .BackColor = ColorBttDeleteBack,
                .ForeColor = ColorBttDeleteFore,
                .Text = "Delete",
                .ToolTipText = String.Empty,
                .AutoToolTip = False
            }
            BTT_EDIT = New ToolStripMenuItem With {
                .Image = PersonalUtilities.My.Resources.PencilPic_01_48,
                .BackColor = ColorBttEditBack,
                .ForeColor = ColorBttEditFore,
                .Text = "Edit",
                .ToolTipText = String.Empty,
                .AutoToolTip = False
            }
            SEP_1 = New ToolStripSeparator
            BTT_DOWNLOAD = New ToolStripMenuItem With {
                .Image = My.Resources.StartPic_01_Green_16,
                .Text = "Download",
                .ToolTipText = "Download users of this group (respect the 'Ready for download' parameter)",
                .AutoToolTip = True
            }
            BTT_DOWNLOAD_FULL = New ToolStripMenuItem With {
                .Image = My.Resources.StartPic_01_Green_16,
                .Text = "Download FULL",
                .ToolTipText = "Download users of this group (ignore the 'Ready for download' parameter)",
                .AutoToolTip = True
            }
            BTT_MENU.DropDownItems.AddRange({BTT_EDIT, BTT_DELETE, SEP_1, BTT_DOWNLOAD, BTT_DOWNLOAD_FULL})
        End Sub
        Friend Sub New(ByVal e As EContainer)
            Me.New
            Name = e.Attribute(Name_Name)
            Temporary = e.Attribute(Name_Temporary).Value.FromXML(Of Integer)(CInt(CheckState.Indeterminate))
            Favorite = e.Attribute(Name_Favorite).Value.FromXML(Of Integer)(CInt(CheckState.Indeterminate))
            ReadyForDownload = e.Attribute(Name_ReadyForDownload).Value.FromXML(Of Boolean)(True)
            ReadyForDownloadIgnore = e.Attribute(Name_ReadyForDownloadIgnore).Value.FromXML(Of Boolean)(False)
            If Not e.Value.IsEmptyString Then Labels.ListAddList(e.Value.Split("|"), LAP.NotContainsOnly)
        End Sub
        Public Overrides Function ToString() As String
            Return $"{IIf(Index >= 0 And Index <= 8, $"#{Index + 1}: ", String.Empty)}{Name}"
        End Function
        Private _ControlSent As Boolean = False
        Friend Function GetControl() As ToolStripMenuItem
            If Not _ControlSent Then
                BTT_MENU.Text = ToString()
                BTT_MENU.Tag = Key
                _ControlSent = True
            End If
            Return BTT_MENU
        End Function
        Private Function SetIndex(ByVal Obj As Object, ByVal _Index As Integer) As Object Implements IIndexable.SetIndex
            DirectCast(Obj, DownloadGroup).Index = _Index
            Return Obj
        End Function
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
                            (Not UseReadyOption Or .ReadyForDownloadIgnore Or user.ReadyForDownload = .ReadyForDownload)
                        Dim f As Func(Of IUserData, IEnumerable(Of IUserData)) = Function(ByVal user As IUserData) As IEnumerable(Of IUserData)
                                                                                     If user.IsCollection Then
                                                                                         With DirectCast(user, UserDataBind)
                                                                                             If .Count > 0 Then Return .Collections.SelectMany(f)
                                                                                         End With
                                                                                     Else
                                                                                         If .Labels.Count = 0 OrElse user.Labels.ListContains(.Labels) Then
                                                                                             If CheckParams.Invoke(user) Then Return {user}
                                                                                         End If
                                                                                     End If
                                                                                     Return New IUserData() {}
                                                                                 End Function
                        Return Settings.Users.SelectMany(f)
                    End With
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendInLog, ex, "[DownloadGroup.GetUsers]")
            End Try
        End Function
        Friend Sub DownloadUsers(ByVal UseReadyOption As Boolean)
            Try
                If Settings.Users.Count > 0 Then
                    Dim u As IEnumerable(Of IUserData) = GetUsers(Me, UseReadyOption)
                    If u.ListExists Then
                        Downloader.AddRange(u)
                    Else
                        MsgBoxE({$"No users found for group [{Name}].", "No users found"}, vbExclamation)
                    End If
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendInLog, ex, "[DownloadGroup.DownloadUsers]")
            End Try
        End Sub
#End Region
#Region "IEContainerProvider Support"
        Private Function ToEContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
            Return New EContainer("Group", Labels.ListToString("|"), {New EAttribute(Name_Name, Name),
                                                                      New EAttribute(Name_Temporary, CInt(Temporary)),
                                                                      New EAttribute(Name_Favorite, CInt(Favorite)),
                                                                      New EAttribute(Name_ReadyForDownload, ReadyForDownload.BoolToInteger),
                                                                      New EAttribute(Name_ReadyForDownloadIgnore, ReadyForDownloadIgnore.BoolToInteger)})
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