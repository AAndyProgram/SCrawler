' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls.Base
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Imports UserMediaD = SCrawler.DownloadObjects.TDownloader.UserMediaD
Namespace DownloadObjects
    Friend Class FeedFilterForm
#Region "Declarations"
        Private WithEvents MyDefs As DefaultFormOptions
        Friend ReadOnly Property MyFilter As FeedFilter = Nothing
        Private ReadOnly IsTempFilter As Boolean
        Private ReadOnly MyData As List(Of UserMediaD)
        Friend Property ShowAllUsers As Boolean? = Nothing
        Friend Property AllowNameEdit As Boolean = False
        Private Class NameChecker : Inherits FieldsCheckerProviderBase
            Public Overrides Property ErrorMessage As String
                Get
                    Return MyBase.ErrorMessage
                End Get
                Set(ByVal m As String)
                    MyBase.ErrorMessage = m
                    HasError = True
                End Set
            End Property
            Public Overrides Sub Reset()
                MyBase.Reset()
                MyBase.ErrorMessage = String.Empty
            End Sub
            Friend ReadOnly Property ENames As List(Of String)
            Friend Sub New()
                ENames = New List(Of String)(MainFrameObj.MF.MyFeed.FILTERS.Select(Function(f) f.Name))
            End Sub
            Public Overrides Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                              Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object
                Dim v$ = AConvert(Of String)(Value, String.Empty)
                If Not v.IsEmptyString Then
                    If ENames.Count = 0 OrElse Not ENames.Contains(v) Then
                        Return Value
                    Else
                        ErrorMessage = $"The '{v}' filter already exists!"
                    End If
                End If
                Return Nothing
            End Function
        End Class
        Private ReadOnly Property MyNameChecker As NameChecker
#End Region
#Region "Initializer"
        Friend Sub New(Optional ByVal f As FeedFilter = Nothing, Optional ByVal Data As IEnumerable(Of UserMediaD) = Nothing,
                       Optional ByVal IsTempFilter As Boolean = False)
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            Me.IsTempFilter = IsTempFilter
            MyFilter = f?.Copy
            If MyFilter Is Nothing Then MyFilter = New FeedFilter
            MyData = New List(Of UserMediaD)
            If Data.ListExists Then MyData.ListAddList(Data)
            Icon = My.Resources.FilterIcon
            MyNameChecker = New NameChecker
        End Sub
#End Region
#Region "Form handlers"
        Private Sub MyForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDefs
                    .MyViewInitialize()
                    .AddOkCancelToolbar(IsTempFilter)

                    If IsTempFilter Then
                        TXT_NAME.Visible = False
                        TP_MAIN.RowStyles(0).Height = 0
                        .MyOkCancel.BTT_DELETE.Text = "Disable"
                        .MyOkCancel.ToolTipDelete = "Disable filter"
                    End If

                    With MyFilter
                        If Not .Name.IsEmptyString Then
                            Text &= $" [{ .Name}]"
                            TXT_NAME.Text = .Name
                            If Not AllowNameEdit Then TXT_NAME.Enabled = False
                        End If
                        If .Types.Count = 0 Then
                            CH_T_ALL.Checked = True
                        Else
                            If .Types.Contains(UTypes.Picture) Then CH_T_IMG.Checked = True
                            If .Types.Contains(UTypes.Video) Then CH_T_VID.Checked = True
                            If .Types.Contains(UTypes.GIF) Then CH_T_GIF.Checked = True
                            If .Types.Contains(UTypes.Text) Then CH_T_TXT.Checked = True
                        End If
                        CH_U_USE.Checked = .Users.Count > 0
                        CH_U_SHOW_ALL.Checked = If(ShowAllUsers, False)
                    End With

                    If Not IsTempFilter And (MyFilter.Name.IsEmptyString Or AllowNameEdit) Then
                        .MyFieldsCheckerE = New FieldsChecker
                        .MyFieldsCheckerE.AddControl(Of String)(TXT_NAME, TXT_NAME.CaptionText,, MyNameChecker)
                        .MyFieldsCheckerE.EndLoaderOperations()
                    End If

                    UpdateSitesText()

                    .EndLoaderOperations()
                    RefillList(True)
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub FeedFilterForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            MyData.ListClearDispose
        End Sub
#End Region
#Region "Refill"
        Private Structure DataUserTmp : Implements IComparable(Of DataUserTmp)
            Friend UserInfo As UserInfo
            Public Shared Widening Operator CType(ByVal u As UserInfo) As DataUserTmp
                Return New DataUserTmp With {.UserInfo = u}
            End Operator
            Public Shared Widening Operator CType(ByVal d As DataUserTmp) As UserInfo
                Return d.UserInfo
            End Operator
            Public Overrides Function ToString() As String
                Return AConvert(Of String)(UserInfo, MainFrameObj.MF.MyFeed.FILTERS.UsersToStringProvider, String.Empty)
            End Function
            Public Overrides Function Equals(ByVal Obj As Object) As Boolean
                If Not IsDBNull(Obj) Then
                    Return UserInfo.Equals(DirectCast(Obj, DataUserTmp).UserInfo)
                Else
                    Return False
                End If
            End Function
            Private Function CompareTo(ByVal Other As DataUserTmp) As Integer Implements IComparable(Of DataUserTmp).CompareTo
                Return ToString.CompareTo(Other.ToString)
            End Function
            Friend Shared Function GetItem(ByVal Obj As Object) As UserInfo
                Return DirectCast(Obj, DataUserTmp).UserInfo
            End Function
        End Structure
        Private Sub RefillList(Optional ByVal Init As Boolean = False)
            If Not MyDefs.Initializing Then
                If Not Init Then ApplyFilter()
                LIST_USERS.Items.Clear()
                Dim dataUsers As New List(Of DataUserTmp)
                If CH_U_SHOW_ALL.Checked Or MyData.Count = 0 Then
                    dataUsers.ListAddList(Settings.UsersList)
                Else
                    dataUsers.ListAddList(MyData.Select(Function(d) d.UserInfo), LAP.NotContainsOnly)
                End If
                Dim i%
                If dataUsers.Count > 0 Then
                    dataUsers.Sort()
                    With MyFilter
                        If .Sites.Count > 0 Then dataUsers.RemoveAll(Function(d) Not .Sites.Contains(d.UserInfo.Site))
                        If dataUsers.Count > 0 Then
                            dataUsers.ForEach(Sub(d) LIST_USERS.Items.Add(d, True))
                            If .Users.Count > 0 Then
                                For i = 0 To LIST_USERS.Items.Count - 1 : LIST_USERS.SetItemChecked(i, .Users.Contains(DataUserTmp.GetItem(LIST_USERS.Items(i)))) : Next
                            End If
                        End If
                    End With
                End If
            End If
        End Sub
#End Region
#Region "ApplyFilter"
        Private Sub ApplyFilter()
            With MyFilter
                .Name = TXT_NAME.Text
                .Types.Clear()
                If Not CH_T_ALL.Checked Then
                    If CH_T_IMG.Checked Then .Types.Add(UTypes.Picture)
                    If CH_T_VID.Checked Then .Types.Add(UTypes.Video)
                    If CH_T_GIF.Checked Then .Types.Add(UTypes.GIF)
                    If CH_T_TXT.Checked Then .Types.Add(UTypes.Text)
                End If
                .Users.Clear()
                If CH_U_USE.Checked Then
                    If LIST_USERS.CheckedItems.Count > 0 Then
                        For Each item As Object In LIST_USERS.CheckedItems : .Users.Add(DataUserTmp.GetItem(item)) : Next
                    End If
                End If
            End With
        End Sub
#End Region
#Region "OK"
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            If MyDefs.MyFieldsChecker Is Nothing OrElse MyDefs.MyFieldsChecker.AllParamsOK Then
                ApplyFilter()
                MyDefs.CloseForm()
            End If
        End Sub
#End Region
#Region "Form controls"
        Private Sub UpdateControlOptions(ByRef CNT As CheckBox, ByVal v As Boolean)
            If v Then CNT.Checked = True
            CNT.Enabled = Not v
        End Sub
        Private Sub CH_T_ALL_CheckedChanged(sender As Object, e As EventArgs) Handles CH_T_ALL.CheckedChanged
            Dim v As Boolean = CH_T_ALL.Checked
            UpdateControlOptions(CH_T_IMG, v)
            UpdateControlOptions(CH_T_VID, v)
            UpdateControlOptions(CH_T_GIF, v)
            UpdateControlOptions(CH_T_TXT, v)
        End Sub
        Private Sub CH_U_USE_CheckedChanged(sender As Object, e As EventArgs) Handles CH_U_USE.CheckedChanged
            Dim v As Boolean = CH_U_USE.Checked
            CH_U_SHOW_ALL.Enabled = v
            BTT_ALL.Enabled = v
            BTT_NONE.Enabled = v
            LIST_USERS.Enabled = v
        End Sub
        Private Sub CH_U_SHOW_ALL_CheckedChanged(sender As Object, e As EventArgs) Handles CH_U_SHOW_ALL.CheckedChanged
            RefillList()
        End Sub
        Private Sub BTT_ALL_NONE_Click(sender As Object, e As EventArgs) Handles BTT_ALL.Click, BTT_NONE.Click
            Dim checked As Boolean = sender Is BTT_ALL
            With LIST_USERS
                If .Items.Count > 0 Then
                    For i% = 0 To .Items.Count - 1 : .SetItemChecked(i, checked) : Next
                End If
            End With
        End Sub
        Private Sub TXT_SITE_ActionOnButtonClick(ByVal Sender As Object, ByVal e As ActionButtonEventArgs) Handles TXT_SITE.ActionOnButtonClick
            Dim refill As Boolean = False
            Select Case e.DefaultButton
                Case ActionButton.DefaultButtons.Edit
                    Using f As New Editors.SiteSelectionForm(MyFilter.Sites)
                        f.ShowDialog()
                        If f.DialogResult = DialogResult.OK Then
                            refill = Not MyFilter.Sites.ListEquals(f.SelectedSites)
                            MyFilter.Sites.ListAddList(f.SelectedSites, LAP.NotContainsOnly, LAP.ClearBeforeAdd)
                            UpdateSitesText()
                        End If
                    End Using
                Case ActionButton.DefaultButtons.Clear : refill = MyFilter.Sites.Count > 0 : MyFilter.Sites.Clear() : UpdateSitesText()
            End Select
            If refill Then RefillList()
        End Sub
        Private Sub UpdateSitesText()
            TXT_SITE.Clear()
            TXT_SITE.Text = MyFilter.Sites.ListToString
        End Sub
#End Region
    End Class
End Namespace