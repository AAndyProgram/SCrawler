' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin
Imports SCrawler.Plugin.Hosts
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Tools.Web.Cookies
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Namespace Editors
    Friend Class SiteEditorForm
        Private ReadOnly LBL_AUTH As Label
        Private ReadOnly LBL_OTHER As Label
        Private WithEvents MyDefs As DefaultFormOptions
        Private WithEvents SpecialButton As Button
#Region "Providers"
        Private Class SavedPostsChecker : Implements IFieldsCheckerProvider
            Private Property ErrorMessage As String Implements IFieldsCheckerProvider.ErrorMessage
            Private Property Name As String Implements IFieldsCheckerProvider.Name
            Private Property TypeError As Boolean Implements IFieldsCheckerProvider.TypeError
            Private Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                     Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object Implements ICustomProvider.Convert
                If ACheck(Value) AndAlso CStr(Value).Contains("/") Then
                    ErrorMessage = $"Path [{Name}] contains forbidden character ""/"""
                    Return Nothing
                Else
                    Return Value
                End If
            End Function
            Private Function GetFormat(ByVal FormatType As Type) As Object Implements IFormatProvider.GetFormat
                Throw New NotImplementedException("[GetFormat] is not available in the context of [SavedPostsChecker]")
            End Function
        End Class
#End Region
        Private ReadOnly Property Host As SettingsHost
        Friend Sub New(ByVal h As SettingsHost)
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            Host = h
            LBL_AUTH = New Label With {.Text = "Authorization", .TextAlign = ContentAlignment.MiddleCenter, .Dock = DockStyle.Fill}
            LBL_OTHER = New Label With {.Text = "Other Parameters", .TextAlign = ContentAlignment.MiddleCenter, .Dock = DockStyle.Fill}
            Host.Source.BeginEdit()
        End Sub
        Private Sub SiteEditorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDefs
                    .MyViewInitialize(True)
                    .AddOkCancelToolbar()

                    .MyFieldsChecker = New FieldsChecker
                    With Host
                        With .Source
                            Text = .Site
                            If Not .Icon Is Nothing Then Icon = .Icon Else ShowIcon = False
                        End With

                        SetCookieText()

                        TXT_PATH.Text = .Path(False)
                        TXT_PATH_SAVED_POSTS.Text = .SavedPostsPath(False)
                        CH_DOWNLOAD_SITE_DATA.Checked = .DownloadSiteData
                        CH_GET_USER_MEDIA_ONLY.Checked = .GetUserMediaOnly

                        SiteDefaultsFunctions.SetChecker(TP_SITE_PROPS, Host)

                        With MyDefs.MyFieldsCheckerE
                            .AddControl(Of String)(TXT_PATH, TXT_PATH.CaptionText, True, New SavedPostsChecker)
                            .AddControl(Of String)(TXT_PATH_SAVED_POSTS, TXT_PATH_SAVED_POSTS.CaptionText, True, New SavedPostsChecker)
                        End With

                        Dim offset% = PropertyValueHost.LeftOffsetDefault
                        Dim h% = 0, c% = 0
                        Dim AddTpControl As Action(Of Control, Integer) = Sub(ByVal cnt As Control, ByVal _height As Integer)
                                                                              TP_SITE_PROPS.RowStyles.Add(New RowStyle(SizeType.Absolute, _height))
                                                                              TP_SITE_PROPS.RowCount += 1
                                                                              TP_SITE_PROPS.Controls.Add(cnt, 0, TP_SITE_PROPS.RowStyles.Count - 1)
                                                                              h += _height
                                                                              c += 1
                                                                          End Sub

                        If Host.Responser Is Nothing Then
                            h -= 28
                            TXT_COOKIES.Enabled = False
                            TXT_COOKIES.Visible = False
                            TP_MAIN.RowStyles(2).Height = 0
                        End If

                        If .PropList.Count > 0 Then
                            Dim laAdded As Boolean = False
                            Dim loAdded As Boolean = False
                            Dim pArr() As Boolean
                            If .PropList.Exists(Function(p) If(p.Options?.IsAuth, False)) Then pArr = {True, False} Else pArr = {False}
                            .PropList.Sort()
                            For Each pAuth As Boolean In pArr
                                For Each prop As PropertyValueHost In .PropList
                                    If Not prop.Options Is Nothing Then
                                        With prop
                                            If .Options.IsAuth = pAuth Then

                                                If pArr.Length = 2 Then
                                                    Select Case pAuth
                                                        Case True
                                                            If Not laAdded Then AddTpControl(LBL_AUTH, 25) : laAdded = True
                                                        Case False
                                                            If Not loAdded Then AddTpControl(LBL_OTHER, 25) : loAdded = True
                                                    End Select
                                                End If

                                                .CreateControl(TT_MAIN)
                                                AddTpControl(.Control, .ControlHeight)
                                                If .LeftOffset > offset Then offset = .LeftOffset
                                                If Not .Options.AllowNull Or Not .ProviderFieldsChecker Is Nothing Then _
                                                   MyDefs.MyFieldsCheckerE.AddControl(.Control, .Options.ControlText, .Type,
                                                                                      .Options.AllowNull, .ProviderFieldsChecker)
                                            End If
                                        End With
                                    End If
                                Next
                            Next
                        End If

                        SpecialButton = .GetSettingsButtonInternal
                        If Not SpecialButton Is Nothing Then AddTpControl(SpecialButton, 28)
                        TP_SITE_PROPS.BaseControlsPadding = New Padding(offset, 0, 0, 0)
                        offset += PaddingE.GetOf({TP_SITE_PROPS}).Left
                        TXT_PATH.CaptionWidth = offset
                        TXT_PATH_SAVED_POSTS.CaptionWidth = offset
                        TXT_COOKIES.CaptionWidth = offset
                        CH_DOWNLOAD_SITE_DATA.Padding = New PaddingE(CH_DOWNLOAD_SITE_DATA.Padding) With {.Left = offset}
                        CH_GET_USER_MEDIA_ONLY.Padding = New PaddingE(CH_GET_USER_MEDIA_ONLY.Padding) With {.Left = offset}
                        If c > 0 Or h <> 0 Then
                            Dim ss As New Size(Size.Width, Size.Height + h + c)
                            MinimumSize = ss
                            Size = ss
                            MaximumSize = ss
                        End If
                    End With

                    .MyFieldsChecker.EndLoaderOperations()
                    .EndLoaderOperations()
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub SiteEditorForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            If Host.PropList.Count > 0 Then Host.PropList.ForEach(Sub(p) p.DisposeControl())
            If Not SpecialButton Is Nothing Then SpecialButton.Dispose()
            LBL_AUTH.Dispose()
            LBL_OTHER.Dispose()
            Host.Source.EndEdit()
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            If MyDefs.MyFieldsChecker.AllParamsOK Then
                Dim i%, ii%
                With Host
                    Dim indxList As New List(Of Integer)
                    For i = 0 To .PropList.Count - 1
                        If .PropList(i).PropertiesChecking.ListExists And Not .PropList(i).PropertiesCheckingMethod Is Nothing Then indxList.Add(i)
                    Next
                    If indxList.Count > 0 Then
                        Dim pList As New List(Of PropertyData)
                        Dim n$()
                        For i = 0 To indxList.Count - 1
                            n = .PropList(indxList(i)).PropertiesChecking
                            For ii = 0 To .PropList.Count - 1
                                With .PropList(ii)
                                    If n.Contains(.Name) Then pList.Add(New PropertyData(.Name, .GetControlValue))
                                End With
                            Next
                            If pList.Count > 0 AndAlso Not CBool(.PropList(indxList(i)).PropertiesCheckingMethod.Invoke(.Source, {pList})) Then Exit Sub
                        Next
                    End If

                    Settings.BeginUpdate()

                    SiteDefaultsFunctions.SetPropByChecker(TP_SITE_PROPS, Host)
                    If TXT_PATH.IsEmptyString Then .Path = Nothing Else .Path = TXT_PATH.Text
                    .SavedPostsPath = TXT_PATH_SAVED_POSTS.Text
                    .DownloadSiteData.Value = CH_DOWNLOAD_SITE_DATA.Checked
                    .GetUserMediaOnly.Value = CH_GET_USER_MEDIA_ONLY.Checked

                    If .PropList.Count > 0 Then .PropList.ForEach(Sub(p) If Not p.Options Is Nothing Then p.UpdateValueByControl())

                    .Source.Update()
                End With

                Settings.EndUpdate()

                MyDefs.CloseForm()
            End If
        End Sub
        Private Sub TXT_PATH_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As EventArgs) Handles TXT_PATH.ActionOnButtonClick
            ChangePath(Sender, Host.Path(False), TXT_PATH)
        End Sub
        Private Sub TXT_PATH_SAVED_POSTS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As EventArgs) Handles TXT_PATH_SAVED_POSTS.ActionOnButtonClick
            ChangePath(Sender, Host.SavedPostsPath(False), TXT_PATH_SAVED_POSTS)
        End Sub
        Private Sub ChangePath(ByVal Sender As ActionButton, ByVal PathValue As SFile, ByRef CNT As TextBoxExtended)
            If Sender.DefaultButton = ADB.Open Then
                Dim f As SFile = SFile.SelectPath(PathValue).IfNullOrEmpty(PathValue)
                If Not f.IsEmptyString Then CNT.Text = f
            End If
        End Sub
        Private Sub TXT_COOKIES_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As EventArgs) Handles TXT_COOKIES.ActionOnButtonClick
            Select Case Sender.DefaultButton
                Case ADB.Edit
                    If Not Host.Responser Is Nothing Then
                        Using f As New CookieListForm With {.DesignXML = Settings.Design, .ShowGrid = False}
                            f.SetCollection(Host.Responser.Cookies)
                            f.ShowDialog()
                            If f.DialogResult = DialogResult.OK Then
                                f.GetCollection(Host.Responser)
                                MyDefs.MyOkCancel.EnableOK = True
                            End If
                        End Using
                        SetCookieText()
                    End If
                Case ADB.Clear
                    If Not Host.Responser Is Nothing Then
                        Host.Responser.Cookies.Clear()
                        MyDefs.MyOkCancel.EnableOK = True
                        SetCookieText()
                    End If
            End Select
        End Sub
        Private Sub SetCookieText()
            If Not Host.Responser Is Nothing Then TXT_COOKIES.Text = $"{Host.Responser.Cookies.Count} cookies"
        End Sub
        Private Sub SpecialButton_Click(sender As Object, e As EventArgs) Handles SpecialButton.Click
            MyDefs.Detector()
        End Sub
    End Class
End Namespace