' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports SCrawler.API.YouTube.Base
Namespace API.YouTube.Controls
    <DefaultEvent("CheckedChanged"), DesignTimeVisible(False), ToolboxItem(False)>
    Friend Class VideoOption : Implements ISupportInitialize
        Friend Event CheckedChanged As EventHandler
        <Browsable(False)> Friend Property MyMedia As MediaObject
        <Browsable(False)> Friend Property Checked As Boolean
            Get
                Return OPT_CHECKED.Checked
            End Get
            Set(ByVal _Checked As Boolean)
                OPT_CHECKED.Checked = _Checked
            End Set
        End Property
        Friend Sub New()
            InitializeComponent()
        End Sub
        Friend Sub New(ByVal m As MediaObject, Optional ByVal SelectedAudio As MediaObject = Nothing)
            Me.New
            Const d$ = " " & ChrW(183) & " "
            Const DRC$ = Objects.YouTubeMediaContainerBase.DRC
            MyMedia = m
            If m.Type = Plugin.UserMediaTypes.Audio Then
                If m.Bitrate >= 320 Then
                    LBL_DEFINITION_INFO.Text = "High Quality"
                ElseIf m.Bitrate >= 190 Then
                    LBL_DEFINITION_INFO.Text = "Medium Quality"
                Else
                    LBL_DEFINITION_INFO.Text = "Low Quality"
                End If
                LBL_DEFINITION.Text = $"{m.Bitrate}k"
                LBL_CODECS.Text = $"{m.Extension} {d} {m.Codec} {d} {m.Bitrate}k"
                If Not m.ID.IsEmptyString AndAlso m.ID.StringToLower.Contains(DRC) Then LBL_CODECS.Text &= $" {d} DRC"
            Else
                If m.Height >= 1440 Then
                    LBL_DEFINITION_INFO.Text = "Ultra High Definition"
                ElseIf m.Height >= 720 Then
                    LBL_DEFINITION_INFO.Text = "High Definition"
                ElseIf m.Height >= 480 Then
                    LBL_DEFINITION_INFO.Text = "Medium Definition"
                ElseIf m.Height >= 360 Then
                    LBL_DEFINITION_INFO.Text = "Normal Definition"
                Else
                    LBL_DEFINITION_INFO.Text = "Low Definition"
                End If
                LBL_DEFINITION.Text = $"{m.Height}p"
                LBL_CODECS.Text = $"{m.Extension.StringToUpper}{d}{m.Codec.StringToUpper}{d}{m.FPS}fps{d}{m.Bitrate}k"
                If Not m.Protocol.IsEmptyString Then LBL_CODECS.Text &= $" ({m.Protocol})"
                If Not m.ID.IsEmptyString AndAlso m.ID.StringToLower.Contains(DRC) Then LBL_CODECS.Text &= $"{d}DRC"
                If Not SelectedAudio.ID.IsEmptyString Then LBL_CODECS.Text &= $" / {SelectedAudio.Extension}{d}{SelectedAudio.Codec}{d}{SelectedAudio.Bitrate}k"
                If Not SelectedAudio.ID.IsEmptyString AndAlso SelectedAudio.ID.StringToLower.Contains(DRC) Then LBL_CODECS.Text &= $"{d}DRC"

                If MyYouTubeSettings.DefaultVideoHighlightFPS_H > 0 AndAlso m.FPS > MyYouTubeSettings.DefaultVideoHighlightFPS_H Then _
                   BackColor = MyColor.DeleteBack : ForeColor = MyColor.DeleteFore
                If MyYouTubeSettings.DefaultVideoHighlightFPS_L > 0 AndAlso m.FPS < MyYouTubeSettings.DefaultVideoHighlightFPS_L Then _
                   BackColor = MyColor.UpdateBack : ForeColor = MyColor.UpdateFore
            End If

            Dim sv% = m.Size / 1024
            If sv >= 1000 Then
                LBL_SIZE.Text = AConvert(Of String)(sv / 1024, VideoSizeProvider)
                LBL_SIZE.Text &= " GB"
            Else
                LBL_SIZE.Text = AConvert(Of String)(sv, VideoSizeProvider)
                LBL_SIZE.Text &= " MB"
            End If
        End Sub
        Private Sub OPT_CHECKED_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_CHECKED.CheckedChanged
            RaiseEvent CheckedChanged(Me, e)
        End Sub
        Private Sub Labels_Click(sender As Object, e As EventArgs) Handles LBL_DEFINITION_INFO.Click, LBL_DEFINITION.Click, LBL_CODECS.Click, LBL_SIZE.Click
            OPT_CHECKED.Checked = True
        End Sub
        Private Sub BindedControl_CheckedChanged(sender As Object, e As EventArgs)
            If Not sender Is Nothing AndAlso Not sender Is Me AndAlso DirectCast(sender, VideoOption).Checked Then Checked = False
        End Sub
        Private Sub BeginInit() Implements ISupportInitialize.BeginInit
        End Sub
        Private Sub EndInit() Implements ISupportInitialize.EndInit
            If Not Parent Is Nothing AndAlso Parent.Controls.Count > 0 Then
                For Each cnt As Control In Parent.Controls
                    If TypeOf cnt Is VideoOption And Not cnt Is Me Then _
                       AddHandler DirectCast(cnt, VideoOption).CheckedChanged, AddressOf BindedControl_CheckedChanged
                Next
            End If
        End Sub
    End Class
End Namespace