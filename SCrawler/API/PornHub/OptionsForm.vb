' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Namespace API.PornHub
    Friend Class OptionsForm
        Private WithEvents MyDefs As DefaultFormOptions
        Private ReadOnly MyExchangeOptions As UserExchangeOptions
        Friend Sub New(ByRef ExchangeOptions As UserExchangeOptions)
            InitializeComponent()
            MyExchangeOptions = ExchangeOptions
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
        End Sub
        Private Sub MyForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize(True)
                .AddOkCancelToolbar()
                CH_DOWN_GIFS.Checked = MyExchangeOptions.DownloadGifs
                CH_DOWN_PHOTO_MODELHUB.Checked = MyExchangeOptions.DownloadPhotoOnlyFromModelHub
                .EndLoaderOperations()
            End With
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            MyExchangeOptions.DownloadGifs = CH_DOWN_GIFS.Checked
            MyExchangeOptions.DownloadPhotoOnlyFromModelHub = CH_DOWN_PHOTO_MODELHUB.Checked
            MyDefs.CloseForm()
        End Sub
    End Class
End Namespace