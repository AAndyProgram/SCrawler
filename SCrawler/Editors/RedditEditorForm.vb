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
    Friend Class RedditEditorForm : Implements IOkCancelToolbar
        Private ReadOnly MyDefs As DefaultFormProps
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormProps
        End Sub
        Private Sub RedditEditorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDefs
                    .MyViewInitialize(Me, Settings.Design, True)
                    .AddOkCancelToolbar()
                    .DelegateClosingChecker()
                    With Settings.Site(Sites.Reddit)
                        TXT_PATH.Text = .Path
                        If .Responser.Cookies Is Nothing Then .Responser.Cookies = New CookieKeeper(.Responser.CookiesDomain)
                    End With
                    SetCookieText()
                    TextBoxExtended.SetFalseDetector(Me, True, AddressOf .Detector)
                    .EndLoaderOperations()
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub ToolbarBttOK() Implements IOkCancelToolbar.ToolbarBttOK
            With Settings.Site(Sites.Reddit)
                If TXT_PATH.IsEmptyString Then .Path = Nothing Else .Path = TXT_PATH.Text
                .Update()
            End With
            MyDefs.CloseForm()
        End Sub
        Private Sub ToolbarBttCancel() Implements IOkCancelToolbar.ToolbarBttCancel
            MyDefs.CloseForm(DialogResult.Cancel)
        End Sub
        Private Sub TXT_PATH_ActionOnButtonClick(ByVal Sender As ActionButton) Handles TXT_PATH.ActionOnButtonClick
            If Sender.DefaultButton = ActionButton.DefaultButtons.Open Then
                Dim f As SFile = SFile.SelectPath(Settings.Site(Sites.Reddit).Path)
                If Not f.IsEmptyString Then TXT_PATH.Text = f
            End If
        End Sub
        Private Sub TXT_COOKIES_ActionOnButtonClick(ByVal Sender As ActionButton) Handles TXT_COOKIES.ActionOnButtonClick
            If Sender.DefaultButton = ActionButton.DefaultButtons.Edit Then
                Using f As New CookieListForm(Settings.Site(Sites.Reddit).Responser.Cookies) : f.ShowDialog() : End Using
                SetCookieText()
            End If
        End Sub
        Private Sub TXT_COOKIES_ActionOnButtonClearClick() Handles TXT_COOKIES.ActionOnButtonClearClick
            With Settings.Site(Sites.Reddit).Responser
                If Not .Cookies Is Nothing Then .Cookies.Dispose()
                .Cookies = New CookieKeeper(.CookiesDomain)
            End With
            SetCookieText()
        End Sub
        Private Sub SetCookieText()
            Dim c% = 0
            With Settings.Site(Sites.Reddit).Responser
                If Not .Cookies Is Nothing Then c = .Cookies.Count
            End With
            TXT_COOKIES.Text = $"{c} cookies"
        End Sub
    End Class
End Namespace