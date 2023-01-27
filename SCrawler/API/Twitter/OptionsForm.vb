' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Reflection
Imports SCrawler.Plugin.Attributes
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls
Namespace API.Twitter
    Friend Class OptionsForm
        Private WithEvents MyDefs As DefaultFormOptions
        Private ReadOnly Property MyExchangeOptions As EditorExchangeOptions
        Private ReadOnly MyGifTextProvider As SiteSettings.GifStringProvider
        Friend Sub New(ByRef ExchangeOptions As EditorExchangeOptions)
            InitializeComponent()
            MyExchangeOptions = ExchangeOptions
            MyGifTextProvider = New SiteSettings.GifStringProvider
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
        End Sub
        Private Sub OptionsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize(True)
                .AddOkCancelToolbar()
                With MyExchangeOptions
                    CH_DOWN_GIFS.Checked = .GifsDownload
                    TXT_GIF_FOLDER.Text = .GifsSpecialFolder
                    TXT_GIF_FOLDER.Tag = NameOf(SiteSettings.GifsSpecialFolder)
                    TXT_GIF_PREFIX.Text = .GifsPrefix
                    TXT_GIF_PREFIX.Tag = NameOf(SiteSettings.GifsPrefix)

                    Try
                        Dim p As PropertyOption
                        With Settings(TwitterSiteKey)
                            p = .PropList.Find(Function(pp) pp.Name = TXT_GIF_FOLDER.Tag).Options
                            If Not p Is Nothing Then
                                TXT_GIF_FOLDER.CaptionText = p.ControlText
                                TXT_GIF_FOLDER.CaptionToolTipText = p.ControlToolTip
                                TXT_GIF_FOLDER.CaptionToolTipEnabled = Not TXT_GIF_FOLDER.CaptionToolTipText.IsEmptyString
                            End If

                            p = .PropList.Find(Function(pp) pp.Name = TXT_GIF_PREFIX.Tag).Options
                            If Not p Is Nothing Then
                                TXT_GIF_PREFIX.CaptionText = p.ControlText
                                TXT_GIF_PREFIX.CaptionToolTipText = p.ControlToolTip
                                TXT_GIF_PREFIX.CaptionToolTipEnabled = Not TXT_GIF_PREFIX.CaptionToolTipText.IsEmptyString
                            End If
                        End With
                    Catch
                    End Try
                End With
                .EndLoaderOperations()
            End With
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            With MyExchangeOptions
                .GifsDownload = CH_DOWN_GIFS.Checked
                .GifsSpecialFolder = TXT_GIF_FOLDER.Text
                .GifsPrefix = TXT_GIF_PREFIX.Text
            End With
            MyDefs.CloseForm()
        End Sub
        Private Sub TXT_ActionOnTextChanged(ByVal Sender As TextBoxExtended, ByVal e As EventArgs) Handles TXT_GIF_FOLDER.ActionOnTextChanged,
                                                                                                           TXT_GIF_PREFIX.ActionOnTextChanged
            If Not MyDefs.Initializing Then
                With Sender
                    MyGifTextProvider.PropertyName = .Tag
                    Dim s% = .SelectionStart
                    Dim t$ = AConvert(Of String)(.Text, String.Empty, MyGifTextProvider)
                    If Not .Text = t Then .Text = t : .Select(s, 0)
                End With
            End If
        End Sub
    End Class
End Namespace