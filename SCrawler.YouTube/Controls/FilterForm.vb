' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.YouTube.Base
Imports SCrawler.API.YouTube.Objects
Imports PersonalUtilities.Forms
Imports SimpleUser = SCrawler.API.YouTube.Controls.YTDataFilter.SimpleUser
Namespace API.YouTube.Controls
    Friend Class FilterForm
        Private WithEvents MyDefs As DefaultFormOptions
        Friend ReadOnly Property DATA As List(Of IYouTubeMediaContainer)
        Private ReadOnly Property DataTMP As List(Of IYouTubeMediaContainer)
        Private ReadOnly Property MyFilterTmp As YTDataFilter
        Private ReadOnly Property MyTmpUsers As List(Of SimpleUser)
        Friend Sub New(ByVal d As IEnumerable(Of IYouTubeMediaContainer))
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, MyYouTubeSettings.DesignXml)
            DATA = New List(Of IYouTubeMediaContainer)
            DATA.ListAddList(d)
            DataTMP = New List(Of IYouTubeMediaContainer)
            DataTMP.ListAddList(d)
            MyFilterTmp = New YTDataFilter(MyYouTubeSettings.FILTER)
            MyTmpUsers = New List(Of SimpleUser)
            Icon = My.Resources.FilterIcon
        End Sub
        Private Sub FilterForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDefs
                    .MyViewInitialize()
                    .AddOkCancelToolbar(True)

                    With MyYouTubeSettings.FILTER
                        With .Types
                            If .Count = 0 Or (.Count = 1 AndAlso .Item(0) = YouTubeMediaType.Undefined) Then
                                CH_FILTER_ALL.Checked = True
                            Else
                                If .Contains(YouTubeMediaType.Single) Then CH_FILTER_SINGLE.Checked = True
                                If .Contains(YouTubeMediaType.Channel) Then CH_FILTER_CHANNEL.Checked = True
                                If .Contains(YouTubeMediaType.PlayList) Then CH_FILTER_PLS.Checked = True
                            End If
                        End With
                        If .IsMusic And .IsVideo Then
                            CH_M_ALL.Checked = True
                        Else
                            CH_M_MUSIC.Checked = .IsMusic
                            CH_M_VIDEO.Checked = .IsVideo
                        End If

                        CH_USERS_USE.Checked = .UserList.Count > 0
                    End With

                    .EndLoaderOperations()
                    _RefillEnabled = True
                    RefillList(True)
                    CH_USERS_USE_CheckedChanged()
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub FilterForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            DATA.ListClearDispose
            DataTMP.ListClearDispose
            MyTmpUsers.ListClearDispose
        End Sub
        Private _RefillEnabled As Boolean = False
        Private Sub RefillList(Optional ByVal Init As Boolean = False)
            If Not MyDefs.Initializing And _RefillEnabled Then
                ApplyFilter(MyFilterTmp, Init)
                MyFilterTmp.Populate(DATA, DataTMP, CH_USERS_USE.Checked)
                With DataTMP
                    If .Count > 0 Then
                        Dim tmpUsers As New List(Of SimpleUser)
                        tmpUsers.ListAddList(DataTMP, LAP.NotContainsOnly, CType(Function(obj As YouTubeMediaContainerBase) CType(obj, SimpleUser), Func(Of Object, Object)))
                        If tmpUsers.Count > 0 Then
                            tmpUsers.Sort()
                            LIST_USERS.BeginUpdate()
                            LIST_USERS.Items.Clear()
                            tmpUsers.ForEach(Sub(u) LIST_USERS.Items.Add(u, True))

                            If CH_USERS_USE.Checked And MyFilterTmp.UserList.Count > 0 Then
                                For i% = 0 To LIST_USERS.Items.Count - 1
                                    LIST_USERS.SetItemChecked(i, MyFilterTmp.UserList.Contains(LIST_USERS.Items(i)))
                                Next
                            End If

                            LIST_USERS.EndUpdate()
                        End If
                    End If
                End With
            End If
        End Sub
        Private Sub ApplyFilter(ByRef Filter As YTDataFilter, ByVal Init As Boolean)
            With Filter
                .Reset(False, Not Init)
                With .Types
                    If CH_FILTER_ALL.Checked Then
                        .Add(YouTubeMediaType.Undefined)
                    Else
                        If CH_FILTER_SINGLE.Checked Then .Add(YouTubeMediaType.Single)
                        If CH_FILTER_CHANNEL.Checked Then .Add(YouTubeMediaType.Channel)
                        If CH_FILTER_PLS.Checked Then .Add(YouTubeMediaType.PlayList)
                    End If
                    If .Count = 0 Then .Add(YouTubeMediaType.Undefined)
                End With
                If CH_M_ALL.Checked Then
                    .IsMusic = True
                    .IsVideo = True
                Else
                    If CH_M_MUSIC.Checked Then .IsMusic = True
                    If CH_M_VIDEO.Checked Then .IsVideo = True
                End If
                If Not .IsVideo And Not .IsMusic Then .IsMusic = True : .IsVideo = True

                If CH_USERS_USE.Checked And Not Init Then
                    .UserList.Clear()
                    If LIST_USERS.CheckedItems.Count > 0 Then
                        For Each item In LIST_USERS.CheckedItems : .UserList.Add(item) : Next
                    End If
                End If
            End With
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            ApplyFilter(MyYouTubeSettings.FILTER, False)
            MyYouTubeSettings.FILTER.RemoveAll(DATA)
            MyYouTubeSettings.FILTER.Update()
            MyDefs.CloseForm()
        End Sub
        Private Sub MyDefs_ButtonDeleteClickOC(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonDeleteClickOC
            With MyYouTubeSettings.FILTER : .Reset() : .Update() : End With
            MyDefs.CloseForm()
        End Sub
        Private Sub UpdateControlOptions(ByRef CNT As CheckBox, ByVal v As Boolean)
            If v Then CNT.Checked = True
            CNT.Enabled = Not v
        End Sub
        Private Sub CH_FILTER_ALL_CheckedChanged(sender As Object, e As EventArgs) Handles CH_FILTER_ALL.CheckedChanged
            _RefillEnabled = False
            Dim v As Boolean = CH_FILTER_ALL.Checked
            UpdateControlOptions(CH_FILTER_SINGLE, v)
            UpdateControlOptions(CH_FILTER_CHANNEL, v)
            UpdateControlOptions(CH_FILTER_PLS, v)
            _RefillEnabled = True
            RefillList()
        End Sub
        Private Sub CH_FILTER_SINGLE_CHANNEL_PLS_CheckedChanged(sender As Object, e As EventArgs) Handles CH_FILTER_SINGLE.CheckedChanged,
                                                                                                          CH_FILTER_CHANNEL.CheckedChanged,
                                                                                                          CH_FILTER_PLS.CheckedChanged
            RefillList()
        End Sub
        Private Sub CH_M_ALL_CheckedChanged(sender As Object, e As EventArgs) Handles CH_M_ALL.CheckedChanged
            _RefillEnabled = False
            Dim v As Boolean = CH_M_ALL.Checked
            UpdateControlOptions(CH_M_VIDEO, v)
            UpdateControlOptions(CH_M_MUSIC, v)
            _RefillEnabled = True
            RefillList()
        End Sub
        Private Sub CH_M_VIDEO_MUSIC_CheckedChanged(sender As Object, e As EventArgs) Handles CH_M_VIDEO.CheckedChanged, CH_M_MUSIC.CheckedChanged
            RefillList()
        End Sub
        Private Sub CH_USERS_USE_CheckedChanged() Handles CH_USERS_USE.CheckedChanged
            LIST_USERS.Enabled = CH_USERS_USE.Checked
            BTT_SELECT_ALL.Enabled = CH_USERS_USE.Checked
            BTT_SELECT_NONE.Enabled = CH_USERS_USE.Checked
        End Sub
        Private Sub BTT_SELECT_ALL_NONE_Click(sender As Object, e As EventArgs) Handles BTT_SELECT_ALL.Click, BTT_SELECT_NONE.Click
            Dim checked As Boolean = sender Is BTT_SELECT_ALL
            With LIST_USERS
                If .Items.Count > 0 Then
                    For i% = 0 To .Items.Count - 1 : .SetItemChecked(i, checked) : Next
                End If
            End With
        End Sub
    End Class
End Namespace