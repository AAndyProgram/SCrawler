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
    Friend Class DownloadGroup : Inherits GroupParameters : Implements IIndexable, IEContainerProvider, IComparable(Of DownloadGroup)
#Region "Events"
        Friend Delegate Sub GroupEventHandler(ByVal Sender As DownloadGroup)
        Friend Event Deleted As GroupEventHandler
        Friend Event Updated As GroupEventHandler
#End Region
#Region "XML names"
        Private Const Name_FilterViewMode As String = "FilterViewMode"
        Private Const Name_FilterGroupUsers As String = "FilterGroupUsers"
        Private Const Name_FilterShowGroupsInsteadLabels As String = "FilterShowGroupsInsteadLabels"
        Private Const Name_FilterShowAllUsers As String = "FilterShowAllUsers"
#End Region
#Region "Declarations"
#Region "Controls"
        Private WithEvents BTT_EDIT As ToolStripMenuItem
        Private WithEvents BTT_DELETE As ToolStripMenuItem
        Private WithEvents BTT_DOWNLOAD As ToolStripKeyMenuItem
        Private WithEvents BTT_CLONE_ADD As ToolStripMenuItem
        Private WithEvents BTT_CLONE_TEMP As ToolStripKeyMenuItem
        Private ReadOnly SEP_1 As ToolStripSeparator
        Private ReadOnly SEP_2 As ToolStripSeparator
        Private WithEvents BTT_MENU As ToolStripKeyMenuItem
#End Region
#Region "Filter declarations"
        Friend Property FilterViewMode As ViewModes = ViewModes.IconLarge
        Friend Property FilterGroupUsers As Boolean = True
        Friend Property FilterShowGroupsInsteadLabels As Boolean = True
        Friend Property FilterShowAllUsers As Boolean = False
#End Region
        Private File As SFile = Nothing
        Friend Overrides Property Name As String
            Get
                Return MyBase.Name
            End Get
            Set(ByVal NewName As String)
                Dim b As Boolean = Not MyBase.Name.IsEmptyString AndAlso Not MyBase.Name = NewName
                MyBase.Name = NewName
                If b Then RaiseEvent Updated(Me)
            End Set
        End Property
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
        Friend Shared ReadOnly Property GroupImage As Bitmap
            Get
                Return My.Resources.GroupByIcon_16.ToBitmap
            End Get
        End Property
#End Region
#Region "Initializers"
        Friend ReadOnly NeedToSave As Boolean = False
        Friend Sub New()
            Me.New(True)
        End Sub
        Friend Sub New(ByVal InitButtons As Boolean)
            If InitButtons Then
                BTT_MENU = New ToolStripKeyMenuItem With {
                .ToolTipText = "Download users of this group",
                .AutoToolTip = True,
                .Image = GroupImage
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
                BTT_CLONE_ADD = New ToolStripMenuItem With {
                    .Image = PersonalUtilities.My.Resources.PlusPic_Green_24,
                    .BackColor = MyColor.OkBack,
                    .ForeColor = MyColor.OkFore,
                    .Text = "Clone and add",
                    .ToolTipText = "Clone the group, change parameters and add this group as a new one",
                    .AutoToolTip = True
                }
                BTT_CLONE_TEMP = New ToolStripKeyMenuItem With {
                    .Image = PersonalUtilities.My.Resources.PlusPic_Green_24,
                    .BackColor = MyColor.OkBack,
                    .ForeColor = MyColor.OkFore,
                    .Text = "Clone and download",
                    .ToolTipText = "Clone the group, change parameters and download filtered users (this group will not be added as a new one)",
                    .AutoToolTip = True
                }
                SEP_1 = New ToolStripSeparator
                SEP_2 = New ToolStripSeparator
                BTT_DOWNLOAD = New ToolStripKeyMenuItem With {
                    .Image = My.Resources.StartPic_Green_16,
                    .Text = "Download",
                    .ToolTipText = "Download users of this group (respect the 'Ready for download' parameter)",
                    .AutoToolTip = True
                }
                BTT_MENU.DropDownItems.AddRange({BTT_EDIT, BTT_DELETE, SEP_1, BTT_CLONE_ADD, BTT_CLONE_TEMP, SEP_2, BTT_DOWNLOAD})
            End If
        End Sub
        Friend Sub New(ByVal e As EContainer)
            Me.New(Not e.Value(Name_IsViewFilter).FromXML(Of Boolean)(False))
            Import(e)
        End Sub
#End Region
#Region "Import/Export"
        Protected Overrides Sub Import(ByVal e As EContainer)
            MyBase.Import(e)
            If IsViewFilter Then
                FilterViewMode = e.Value(Name_FilterViewMode).FromXML(Of Integer)(ViewModes.IconLarge)
                FilterGroupUsers = e.Value(Name_FilterGroupUsers).FromXML(Of Boolean)(True)
                FilterShowGroupsInsteadLabels = e.Value(Name_FilterShowGroupsInsteadLabels).FromXML(Of Boolean)(True)
                FilterShowAllUsers = e.Value(Name_FilterShowAllUsers).FromXML(Of Boolean)(False)
            End If
        End Sub
        Protected Overrides Function Export(ByVal e As EContainer) As EContainer
            MyBase.Export(e)
            e.AddRange({New EContainer(Name_FilterViewMode, CInt(FilterViewMode)),
                        New EContainer(Name_FilterGroupUsers, FilterGroupUsers.BoolToInteger),
                        New EContainer(Name_FilterShowGroupsInsteadLabels, FilterShowGroupsInsteadLabels.BoolToInteger),
                        New EContainer(Name_FilterShowAllUsers, FilterShowAllUsers.BoolToInteger)})
            Return e
        End Function
#End Region
#Region "Copy"
        Friend Overloads Overrides Function Copy() As Object
            Return (New DownloadGroup).Copy(Me)
        End Function
        Friend Overloads Overrides Function Copy(ByVal Source As Object) As Object
            MyBase.Copy(Source)
            If TypeOf Source Is DownloadGroup Then
                With DirectCast(Source, DownloadGroup)
                    If .IsViewFilter Then
                        FilterViewMode = .FilterViewMode
                        FilterGroupUsers = .FilterGroupUsers
                        FilterShowGroupsInsteadLabels = .FilterShowGroupsInsteadLabels
                        FilterShowAllUsers = .FilterShowAllUsers
                    End If
                End With
            End If
            Return Me
        End Function
#End Region
#Region "ToString"
        Public Overrides Function ToString() As String
            Return $"{IIf(Index.ValueBetween(0, 8), $"#{Index + 1}: ", String.Empty)}{Name}"
        End Function
        Friend Overrides Function ToStringViewFilters() As String
            Return $"{IIf(IsViewFilter, "View filter", "Group")} '{Name}'"
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
        Private Sub BTT_MENU_Click(ByVal Sender As Object, ByVal e As MyKeyEventArgs) Handles BTT_MENU.KeyClick
            Try
                With BTT_MENU
                    .HideDropDown()
                    Dim obj As Object = .Owner
                    Dim r% = 0
                    Do While Not obj Is Nothing And r < 5 : obj = TryHide(obj) : r += 1 : Loop
                End With
            Catch
            End Try
            ProcessDownloadUsers(e.IncludeInTheFeed)
        End Sub
        Private Function TryHide(ByVal Sender As Object) As Object
            Dim retObj As Object = Nothing
            Try
                If Not Sender Is Nothing Then
                    If TypeOf Sender Is ToolStripDropDownMenu Then
                        With DirectCast(Sender, ToolStripDropDownMenu)
                            retObj = .OwnerItem
                            .Hide()
                        End With
                    ElseIf TypeOf Sender Is ToolStripMenuItem Then
                        With DirectCast(Sender, ToolStripMenuItem)
                            retObj = .Owner
                            .HideDropDown()
                        End With
                    End If
                End If
            Catch
            End Try
            If Not retObj Is Nothing AndAlso Not (TypeOf retObj Is ToolStripMenuItem Or TypeOf retObj Is ToolStripDropDownMenu) Then retObj = Nothing
            Return retObj
        End Function
        Private Sub BTT_EDIT_Click(sender As Object, e As EventArgs) Handles BTT_EDIT.Click
            Using f As New GroupEditorForm(Me)
                f.ShowDialog()
                If f.DialogResult = DialogResult.OK Then RaiseEvent Updated(Me)
            End Using
        End Sub
        Private Sub BTT_DELETE_Click(sender As Object, e As EventArgs) Handles BTT_DELETE.Click
            Delete()
        End Sub
        Friend Function Delete(Optional ByVal Silent As Boolean = False) As Boolean
            Dim msgTitle$ = $"Deleting a {IIf(IsViewFilter, "filter", "group")}"
            If Silent OrElse MsgBoxE({$"Are you sure you want to delete the '{Name}' {IIf(IsViewFilter, "filter", "group")}?", msgTitle}, vbExclamation + vbYesNo) = vbYes Then
                If Not Settings.Automation Is Nothing AndAlso Settings.Automation.Count > 0 Then
                    Dim aIncl As New List(Of String)
                    For Each plan As AutoDownloader In Settings.Automation
                        If plan.Mode = AutoDownloader.Modes.Groups AndAlso plan.Groups.Count > 0 AndAlso plan.Groups.Contains(Name) Then aIncl.Add(plan.Name)
                    Next
                    If aIncl.Count > 0 Then
                        MsgBoxE({$"The '{Name}' group cannot be deleted because it is included in the following scheduler plans:{vbCr}{vbCr}" &
                                 aIncl.ListToString(vbCr), msgTitle}, vbCritical)
                        aIncl.Clear()
                        Return False
                    End If
                End If
                RaiseEvent Deleted(Me)
                If Not Silent Then MsgBoxE({$"{IIf(IsViewFilter, "Filter", "Group")} '{Name}' deleted", msgTitle})
                Return True
            End If
            Return False
        End Function
        Private Sub BTT_CLONE_ADD_Click(sender As Object, e As EventArgs) Handles BTT_CLONE_ADD.Click
            Settings.Groups.CloneAndAdd(Me)
        End Sub
        Private Sub BTT_CLONE_TEMP_Click(ByVal Sender As Object, ByVal e As MyKeyEventArgs) Handles BTT_CLONE_TEMP.KeyClick
            Using f As New GroupEditorForm(New DownloadGroup(False).Copy(Me)) With {.IsTemporaryGroup = True}
                f.ShowDialog()
                If f.DialogResult = DialogResult.OK AndAlso Not f.MyGroup Is Nothing Then
                    f.MyGroup.Name = String.Empty
                    f.MyGroup.ProcessDownloadUsers(e.IncludeInTheFeed)
                End If
                If Not f.MyGroup Is Nothing Then f.MyGroup.Dispose()
            End Using
        End Sub
        Private Sub BTT_DOWNLOAD_Click(ByVal Sender As Object, ByVal e As MyKeyEventArgs) Handles BTT_DOWNLOAD.KeyClick
            ProcessDownloadUsers(e.IncludeInTheFeed)
        End Sub
#End Region
#Region "Get users"
        Friend Overloads Function GetUsers() As IEnumerable(Of IUserData)
            Return GetUsers(Me)
        End Function
        Friend Overloads Shared Function GetUsers(ByVal Instance As IGroup) As IEnumerable(Of IUserData)
            Try
                If Settings.Users.Count > 0 Then
                    With Instance
                        Dim downDate As Date? = Nothing
                        If .DaysNumber > 0 Then
                            With Now.AddDays(- .DaysNumber) : downDate = New Date(.Year, .Month, .Day, 0, 0, 0) : End With
                        End If
                        Dim CheckUserExists As Predicate(Of IUserData) = Function(ByVal user As IUserData) As Boolean
                                                                             If Not user.Exists Then
                                                                                 Return .UserDeleted
                                                                             ElseIf user.Suspended Then
                                                                                 Return .UserSuspended
                                                                             Else
                                                                                 Return .UserExists
                                                                             End If
                                                                         End Function
                        Dim CheckUserCategory As Predicate(Of IUserData) = Function(ByVal user As IUserData) As Boolean
                                                                               If user.Favorite Then
                                                                                   Return .Favorite
                                                                               ElseIf user.Temporary Then
                                                                                   Return .Temporary
                                                                               Else
                                                                                   Return .Regular
                                                                               End If
                                                                           End Function
                        Dim CheckParams As Predicate(Of IUserData) = Function(user) _
                            CheckUserCategory.Invoke(user) And
                            (.ReadyForDownloadIgnore Or user.ReadyForDownload = .ReadyForDownload) And CheckUserExists.Invoke(user)
                        Dim CheckSubscription As Predicate(Of IUserData) = Function(user) (.DownloadSubscriptions And user.IsSubscription) Or
                                                                                          (.DownloadUsers And Not user.IsSubscription)
                        Dim CheckLabelsExcluded As Predicate(Of IUserData) = Function(ByVal user As IUserData) As Boolean
                                                                                 If Not .LabelsExcludedIgnore Then
                                                                                     If .LabelsExcluded.Count = 0 Then
                                                                                         Return True
                                                                                     ElseIf user.Labels.Count = 0 Then
                                                                                         Return True
                                                                                     Else
                                                                                         Return Not user.Labels.ListContains(.LabelsExcluded)
                                                                                     End If
                                                                                 Else
                                                                                     Return True
                                                                                 End If
                                                                             End Function
                        Dim CheckLabels As Predicate(Of IUserData) = Function(ByVal user As IUserData) As Boolean
                                                                         If .LabelsNo Then
                                                                             Return user.Labels.Count = 0
                                                                         ElseIf .Labels.Count = 0 Then
                                                                             Return CheckLabelsExcluded.Invoke(user)
                                                                         ElseIf user.Labels.Count = 0 Then
                                                                             Return False
                                                                         Else
                                                                             Return user.Labels.ListContains(.Labels) And CheckLabelsExcluded.Invoke(user)
                                                                         End If
                                                                     End Function
                        Dim CheckDays As Predicate(Of IUserData) = Function(ByVal user As IUserData) As Boolean
                                                                       If downDate.HasValue Then
                                                                           Dim ld As Date? = DirectCast(user, UserDataBase).LastUpdated
                                                                           If .DaysIsDownloaded Then
                                                                               Return ld.HasValue AndAlso ld.Value >= downDate.Value
                                                                           Else
                                                                               Return Not ld.HasValue OrElse ld.Value < downDate.Value
                                                                           End If
                                                                       Else
                                                                           Return True
                                                                       End If
                                                                   End Function
                        Dim CheckDateRange As Predicate(Of IUserData) =
                            Function(ByVal user As IUserData) As Boolean
                                If Not .DateMode = ShowingDates.Off Then
                                    Dim ld As Date? = DirectCast(user, UserDataBase).LastUpdated
                                    If ld.HasValue Then
                                        Dim df As Date = If(.DateFrom, Date.MinValue.Date)
                                        Dim dt As Date = If(.DateTo, Date.MaxValue.Date)
                                        Return ld.Value.ValueBetween(df, dt) = (.DateMode = ShowingDates.In)
                                    End If
                                End If
                                Return True
                            End Function
                        Dim CheckSites As Predicate(Of IUserData) = Function(user) _
                            (.Sites.Count = 0 OrElse .Sites.Contains(user.Site)) AndAlso
                            (.SitesExcluded.Count = 0 OrElse Not .SitesExcluded.Contains(user.Site))
                        Dim users As IEnumerable(Of IUserData) =
                            Settings.GetUsers(Function(user) CheckLabels.Invoke(user) AndAlso CheckSites.Invoke(user) AndAlso
                                                             CheckParams.Invoke(user) AndAlso CheckSubscription.Invoke(user) AndAlso
                                                             CheckDays.Invoke(user) AndAlso CheckDateRange.Invoke(user))
                        If .UsersCount <> 0 And users.ListExists Then
                            users = users.ListTake(If(.UsersCount > 0, -1, -2), Math.Abs(.UsersCount))
                            If .UsersCount < 0 Then users = users.ListReverse
                        End If
                        Return users
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
        Friend Sub ProcessDownloadUsers(Optional ByVal IncludeInTheFeed As Boolean = True, Optional ByVal ShowNoUsersMessage As Boolean = True)
            Try
                If Settings.Users.Count > 0 Then
                    Dim u As IEnumerable(Of IUserData) = GetUsers(Me)
                    If u.ListExists Then
                        Downloader.AddRange(u, IncludeInTheFeed)
                    ElseIf ShowNoUsersMessage Then
                        MsgBoxE({$"No users found{If(Not Name.IsEmptyString, $" in group '{Name}'", String.Empty)}!", "No users found"}, vbExclamation)
                    End If
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadGroup.DownloadUsers]")
            End Try
        End Sub
#End Region
#Region "Advanced filter support"
        Friend Sub LoadFromFile(ByVal f As SFile)
            File = f
            If f.Exists Then
                Using x As New XmlFile(f) With {.XmlReadOnly = True} : Import(x) : End Using
            End If
        End Sub
        Friend Sub UpdateFile()
            Using x As New XmlFile
                Export(x)
                x.Name = "AdvancedFilter"
                x.Save(File)
            End Using
        End Sub
#End Region
#Region "IEContainerProvider Support"
        Friend Function ToEContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
            Return Export(New EContainer("Group"))
        End Function
#End Region
#Region "IComparable Support"
        Private Function CompareTo(ByVal Other As DownloadGroup) As Integer Implements IComparable(Of DownloadGroup).CompareTo
            If IsViewFilter Then
                Return IIf(Other.IsViewFilter, Name.CompareTo(Other.Name), 1)
            ElseIf Other.IsViewFilter Then
                Return -1
            Else
                Return Index.CompareTo(Other.Index)
            End If
        End Function
#End Region
#Region "IDisposable Support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    BTT_EDIT.DisposeIfReady
                    BTT_DELETE.DisposeIfReady
                    BTT_DOWNLOAD.DisposeIfReady
                    BTT_CLONE_ADD.DisposeIfReady
                    BTT_CLONE_TEMP.DisposeIfReady
                    SEP_1.DisposeIfReady
                    SEP_2.DisposeIfReady
                    If Not BTT_MENU Is Nothing Then BTT_MENU.DropDownItems.Clear()
                    BTT_MENU.DisposeIfReady
                End If
                BTT_EDIT = Nothing
                BTT_DELETE = Nothing
                BTT_DOWNLOAD = Nothing
                BTT_CLONE_ADD = Nothing
                BTT_CLONE_TEMP = Nothing
                BTT_MENU = Nothing
            End If
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace