' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Tools.WEB
Namespace Editors
    Friend Class SiteEditorForm : Implements IOkCancelToolbar
        Private ReadOnly MyDefs As DefaultFormProps(Of FieldsChecker)
        Private ReadOnly MySite As Sites
        Friend Sub New(ByVal s As Sites)
            InitializeComponent()
            MySite = s
            MyDefs = New DefaultFormProps(Of FieldsChecker)
        End Sub
        Private Sub SiteEditorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDefs
                    .MyViewInitialize(Me, Settings.Design, True)
                    .AddOkCancelToolbar()
                    .DelegateClosingChecker()
                    Select Case MySite
                        Case Sites.Reddit : Icon = My.Resources.RedditIcon
                        Case Sites.Twitter : Icon = My.Resources.TwitterIcon
                        Case Sites.Instagram : Icon = My.Resources.InstagramIcon
                        Case Else : ShowIcon = False
                    End Select
                    Text = MySite.ToString

                    With Settings(MySite)
                        TXT_PATH.Text = .Path(False)
                        With .Responser
                            If .Cookies Is Nothing Then .Cookies = New CookieKeeper(.CookiesDomain)
                            SetCookieText()
                            If MySite = Sites.Twitter Then
                                TXT_TOKEN.Text = .Headers(API.Base.SiteSettings.Header_Twitter_Token)
                                TXT_AUTH.Text = .Headers(API.Base.SiteSettings.Header_Twitter_Authorization)
                            End If
                        End With
                        If MySite = Sites.Instagram Then
                            TXT_TOKEN.Text = .InstaHash
                            TXT_AUTH.Text = .InstaHash_SP
                        End If
                    End With

                    If MySite = Sites.Twitter Or MySite = Sites.Instagram Then
                        If MySite = Sites.Instagram Then
                            TXT_TOKEN.CaptionText = "Hash"
                            TXT_TOKEN.CaptionToolTipText = "Instagram session hash"
                            TXT_TOKEN.Buttons.Clear()
                            TXT_TOKEN.Buttons.AddRange({ActionButton.DefaultButtons.Refresh, ActionButton.DefaultButtons.Clear})
                            TXT_AUTH.CaptionText = "Hash 2"
                            TXT_AUTH.CaptionToolTipText = "Instagram session hash for saved posts"
                        End If
                    Else
                        TXT_AUTH.Visible = False
                        TXT_TOKEN.Visible = False
                        Dim p As PaddingE = PaddingE.GetOf({TP_MAIN})
                        Dim s As New Size(Size.Width, Size.Height - p.Vertical(2) - TXT_AUTH.NeededHeight - TXT_TOKEN.NeededHeight)
                        With TP_MAIN
                            .RowStyles(2).Height = 0
                            .RowStyles(3).Height = 0
                        End With
                        MinimumSize = s
                        Size = s
                        MaximumSize = s
                    End If

                    .MyFieldsChecker = New FieldsChecker
                    With .MyFieldsChecker
                        If MySite = Sites.Twitter Or MySite = Sites.Instagram Then
                            .AddControl(Of String)(TXT_TOKEN, TXT_TOKEN.CaptionText)
                            .AddControl(Of String)(TXT_AUTH, TXT_AUTH.CaptionText, MySite = Sites.Instagram)
                        End If
                        .EndLoaderOperations()
                    End With
                    TextBoxExtended.SetFalseDetector(Me, True, AddressOf .Detector)
                    .EndLoaderOperations()
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub ToolbarBttOK() Implements IOkCancelToolbar.ToolbarBttOK
            If MyDefs.MyFieldsChecker.AllParamsOK Then
                If MySite = Sites.Instagram Then
                    If Not TXT_TOKEN.IsEmptyString AndAlso Not TXT_AUTH.IsEmptyString AndAlso TXT_TOKEN.Text = TXT_AUTH.Text Then
                        MsgBoxE({"InstaHash for saved posts must be different from InstaHash!", "InstaHash are equal"}, vbCritical)
                        Exit Sub
                    End If
                End If
                With Settings(MySite)
                    If TXT_PATH.IsEmptyString Then .Path = Nothing Else .Path = TXT_PATH.Text
                    Select Case MySite
                        Case Sites.Twitter
                            With .Responser
                                .Headers(API.Base.SiteSettings.Header_Twitter_Token) = TXT_TOKEN.Text
                                .Headers(API.Base.SiteSettings.Header_Twitter_Authorization) = TXT_AUTH.Text
                            End With
                        Case Sites.Instagram
                            .InstaHash.Value = TXT_TOKEN.Text
                            .InstaHash_SP.Value = TXT_AUTH.Text
                    End Select
                    .Update()
                End With
                MyDefs.CloseForm()
            End If
        End Sub
        Private Sub ToolbarBttCancel() Implements IOkCancelToolbar.ToolbarBttCancel
            MyDefs.CloseForm(DialogResult.Cancel)
        End Sub
        Private Sub TXT_TOKEN_ActionOnButtonClick(ByVal Sender As ActionButton) Handles TXT_TOKEN.ActionOnButtonClick
            If Sender.DefaultButton = ActionButton.DefaultButtons.Refresh Then
                With Settings(Sites.Instagram)
                    If .GatherInstaHash() Then
                        .InstaHashUpdateRequired.Value = Not .InstaHash.IsEmptyString
                        TXT_TOKEN.Text = .InstaHash
                    End If
                End With
            End If
        End Sub
        Private Sub TXT_PATH_ActionOnButtonClick(ByVal Sender As ActionButton) Handles TXT_PATH.ActionOnButtonClick
            If Sender.DefaultButton = ActionButton.DefaultButtons.Open Then
                Dim f As SFile = SFile.SelectPath(Settings(MySite).Path(False))
                If Not f.IsEmptyString Then TXT_PATH.Text = f
            End If
        End Sub
        Private Sub TXT_COOKIES_ActionOnButtonClick(ByVal Sender As ActionButton) Handles TXT_COOKIES.ActionOnButtonClick
            If Sender.DefaultButton = ActionButton.DefaultButtons.Edit Then
                Using f As New CookieListForm(Settings(MySite).Responser.Cookies) With {.MyDesignXML = Settings.Design} : f.ShowDialog() : End Using
                SetCookieText()
            End If
        End Sub
        Private Sub TXT_COOKIES_ActionOnButtonClearClick() Handles TXT_COOKIES.ActionOnButtonClearClick
            With Settings(MySite).Responser
                If Not .Cookies Is Nothing Then .Cookies.Dispose()
                .Cookies = New CookieKeeper(.CookiesDomain)
            End With
            SetCookieText()
        End Sub
        Private Sub SetCookieText()
            TXT_COOKIES.Text = $"{If(Settings(MySite).Responser.Cookies?.Count, 0)} cookies"
        End Sub
    End Class
End Namespace