' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Tools
Imports SCrawler.API.YouTube.Base
Imports SCrawler.API.YouTube.Objects
Imports SCrawler.DownloadObjects.STDownloader
Imports UMTypes = SCrawler.Plugin.UserMediaTypes
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Namespace API.YouTube.Controls
    Friend Class VideoOptionsForm : Implements IDesignXMLContainer
#Region "Declarations"
        Private MyView As FormView
        Friend Property DesignXML As EContainer Implements IDesignXMLContainer.DesignXML
        Private Property DesignXMLNodes As String() Implements IDesignXMLContainer.DesignXMLNodes
        Private Property DesignXMLNodeName As String Implements IDesignXMLContainer.DesignXMLNodeName
        Private Const ControlsRow As Integer = 6
        Private ReadOnly Property CNT_PROCESSOR As TableControlsProcessor
        Friend Property MyContainer As YouTubeMediaContainerBase
        Private Initialization As Boolean = True
        Private ReadOnly IsSavedObject As Boolean
#End Region
#Region "Initializers"
        Friend Sub New(ByVal Container As YouTubeMediaContainerBase, Optional ByVal IsSavedObject As Boolean = False)
            InitializeComponent()
            MyContainer = Container
            CNT_PROCESSOR = New TableControlsProcessor(TP_CONTROLS)
            Me.IsSavedObject = IsSavedObject
        End Sub
#End Region
#Region "Form handlers"
        Private Sub VideoOptionsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            If Not DesignXML Is Nothing Then
                MyView = New FormView(Me) With {.LocationOnly = True}
                MyView.Import()
                MyView.SetFormSize()
            End If

            MyYouTubeSettings.DownloadLocations.PopulateComboBox(TXT_FILE)

            If Not MyContainer Is Nothing Then
                With MyContainer
                    Dim i%
                    Dim arr$() = Nothing
                    Dim arrComparer As New FComparer(Of String)(Function(x, y) x.ToLower = y.ToLower)
                    Dim setDef As Action(Of ComboBox, String) =
                        Sub(ByVal cmb As ComboBox, ByVal compValue As String)
                            i = -1
                            If Not compValue.IsEmptyString Then i = arr.ListIndexOf(compValue, arrComparer, EDP.ReturnValue)
                            If i >= 0 Then cmb.SelectedIndex = i Else cmb.SelectedIndex = 0
                        End Sub
                    Dim __audioOnly As Boolean = False
                    Dim __optionValue$

                    If .HasElements Then
                        Text = "Playlist"
                        If Not .PlaylistTitle.IsEmptyString Or Not .Title.IsEmptyString Then Text &= $" - { .PlaylistTitle.IfNullOrEmpty(.Title)}"
                        TP_MAIN.Controls.Remove(TP_HEADER_BASE)
                        TP_MAIN.RowStyles(0).Height = 0
                        Dim def% = If(IsSavedObject, .ArrayMaxResolution, MyYouTubeSettings.DefaultVideoDefinition.Value)
                        If IsSavedObject Then
                            __audioOnly = def = -2
                            If def <= 0 Then def = MyYouTubeSettings.DefaultVideoDefinition
                        Else
                            If Not def.ValueBetween(-1, 10000) Then def = 1080
                        End If
                        NUM_RES.Value = def
                    Else
                        TP_OPTIONS.Controls.Remove(NUM_RES)
                        TP_OPTIONS.ColumnStyles(3).Width = 0
                        Dim img As Image = Nothing
                        Dim imgUrl$ = .ThumbnailUrlMedia
                        If Not imgUrl.IsEmptyString Then
                            img = ImageRenderer.GetImage(SFile.GetBytesFromNet(imgUrl, EDP.ReturnValue), EDP.ReturnValue)
                            If Not img Is Nothing Then ICON_VIDEO.Image = img : ICON_VIDEO.InitialImage = img
                        End If
                        LBL_TITLE.Text = .Title
                        LBL_TIME.Text = AConvert(Of String)(.Duration, TimeToStringProvider, String.Empty)
                        TP_HEADER_INFO_2.ColumnStyles(1).Width = MeasureTextDefault(LBL_TIME.Text, LBL_TIME.Font).Width + PaddingE.GetOf({LBL_TIME}).Horizontal
                        TP_HEADER_INFO_2.Refresh()
                        LBL_URL.Text = .URL
                    End If

                    If .IsMusic Or __audioOnly Then
                        OPT_AUDIO.Checked = True
                    Else
                        OPT_VIDEO.Checked = True
                    End If
                    CMB_FORMAT.Enabled = OPT_VIDEO.Checked

                    arr = AvailableVideoFormats
                    CMB_FORMAT.Items.AddRange(arr)
                    If IsSavedObject Then
                        __optionValue = .OutputVideoExtension.IfNullOrEmpty(MyYouTubeSettings.DefaultVideoFormat.Value)
                    Else
                        __optionValue = MyYouTubeSettings.DefaultVideoFormat.Value
                    End If
                    setDef(CMB_FORMAT, __optionValue)

                    arr = AvailableAudioFormats
                    CMB_AUDIO_CODEC.Items.AddRange(arr)
                    If IsSavedObject Then
                        __optionValue = .OutputAudioCodec.IfNullOrEmpty(IIf(.IsMusic, MyYouTubeSettings.DefaultAudioCodecMusic.Value, MyYouTubeSettings.DefaultAudioCodec.Value))
                    Else
                        __optionValue = IIf(.IsMusic, MyYouTubeSettings.DefaultAudioCodecMusic.Value, MyYouTubeSettings.DefaultAudioCodec.Value)
                    End If
                    setDef(CMB_AUDIO_CODEC, __optionValue)

                    arr = AvailableSubtitlesFormats
                    CMB_SUBS_FORMAT.Items.AddRange(arr)
                    If IsSavedObject Then
                        __optionValue = .OutputSubtitlesFormat.IfNullOrEmpty(IIf(.IsMusic, "LRC", MyYouTubeSettings.DefaultSubtitlesFormat.Value))
                    Else
                        __optionValue = IIf(.IsMusic, "LRC", MyYouTubeSettings.DefaultSubtitlesFormat.Value)
                    End If
                    setDef(CMB_SUBS_FORMAT, __optionValue)

                    TP_SUBS.Enabled = .Subtitles.Count > 0
                    TXT_SUBS_ADDIT.Enabled = .Subtitles.Count > 0
                    RefillTextBoxes()
                    TXT_SUBS_ADDIT.Checked = .PostProcessing_OutputSubtitlesFormats.Count > 0
                    TXT_EXTRA_AUDIO_FORMATS.Checked = .PostProcessing_OutputAudioFormats.Count > 0
                    TXT_FILE.Text = .File
                    RefillList()
                    If OPT_VIDEO.Checked Then
                        ChangeFileExtension(CMB_FORMAT.Text)
                    Else
                        If .HasElements Then NUM_RES.Enabled = False
                        ChangeFileExtension(CMB_AUDIO_CODEC.Text)
                    End If
                End With
            End If
            Initialization = False
        End Sub
        Private Sub VideoOptionsForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            MyView.DisposeIfReady()
        End Sub
#End Region
#Region "Refill"
        Private Sub RefillList()
            Dim i%
            Dim h% = -1
            Dim rh% = IIf(MyContainer.HasElements, 60, 25)
            Dim s As New Size(Width, h)
            Dim CalculateSize As Action(Of Integer) =
                Sub(ByVal InitHeight As Integer)
                    With TP_MAIN.RowStyles(ControlsRow) : .SizeType = SizeType.Absolute : .Height = InitHeight : End With
                    s.Height = InitHeight
                    For ii% = 0 To TP_MAIN.RowStyles.Count - 1
                        If Not ii = ControlsRow And TP_MAIN.RowStyles(ii).SizeType = SizeType.Absolute Then s.Height += TP_MAIN.RowStyles(ii).Height
                    Next
                    s.Height += PaddingE.GetOf({TP_MAIN}).Vertical(TP_MAIN.RowStyles.Count - 3 - IIf(MyContainer.HasElements, 1, 0))
                End Sub
            Dim __contentType As UMTypes = IIf(OPT_VIDEO.Checked, UMTypes.Video, UMTypes.Audio)
            With TP_CONTROLS
                If .Controls.Count > 0 Then
                    For Each cnt As Control In .Controls : cnt.Dispose() : Next
                    .Controls.Clear()
                End If
                .RowStyles.Clear()
                .RowCount = 0
            End With
            With MyContainer
                Dim audio As MediaObject = Nothing
                If __contentType = UMTypes.Video Then audio = .SelectedAudio
                Dim data As IEnumerable(Of Control)

                If .HasElements Then
                    data = .Elements.Select(Function(ee) New MediaItem(ee) With {.Dock = DockStyle.Fill, .Checked = ee.Checked, .IgnoreDownloadState = True})
                Else
                    data = (From m As MediaObject In .Self.MediaObjects
                            Where m.Type = __contentType
                            Select New VideoOption(m, audio) With {.Dock = DockStyle.Fill, .Checked = m.Index = MyContainer.SelectedVideoIndex})
                End If

                If data.ListExists Then
                    With TP_CONTROLS
                        With .RowStyles
                            .Clear()
                            For i = 0 To data.Count - 1 : .Add(New RowStyle(SizeType.Absolute, rh)) : Next
                            .Add(New RowStyle(SizeType.AutoSize))
                        End With
                        .RowCount = .RowStyles.Count
                        For i = 0 To data.Count - 1 : .Controls.Add(data(i), 0, i) : Next
                        .Controls.Cast(Of Control).ToList.ForEach(Sub(ByVal d As Control)
                                                                      DirectCast(d, ISupportInitialize).EndInit()
                                                                      If MyContainer.HasElements Then
                                                                          With DirectCast(d, MediaItem)
                                                                              AddHandler .CheckedChanged, AddressOf MediaItem_CheckedChanged
                                                                              AddHandler .Click, AddressOf CNT_PROCESSOR.MediaItem_Click
                                                                              AddHandler .KeyDown, AddressOf CNT_PROCESSOR.MediaItem_KeyDown
                                                                          End With
                                                                      End If
                                                                  End Sub)
                        If MyContainer.HasElements Then
                            If .Controls.Count > 0 Then
                                Dim cIndx% = 0
                                Dim c As Color
                                For Each cnt As MediaItem In .Controls
                                    cIndx += 1
                                    If (cIndx Mod 2) = 0 Then c = SystemColors.ControlLight Else c = SystemColors.Window
                                    cnt.BackColor = c
                                Next
                            End If
                        Else
                            If Not data.ListExists(Function(d As VideoOption) d.Checked) Then
                                If MyYouTubeSettings.DefaultVideoDefinition > 0 Then
                                    For Each cnt As VideoOption In .Controls
                                        If cnt.MyMedia.Height <= MyYouTubeSettings.DefaultVideoDefinition Then cnt.Checked = True : Exit For
                                    Next
                                Else
                                    DirectCast(.Controls(0), VideoOption).Checked = True
                                End If
                            End If
                        End If
                    End With

                    h = data.Count * rh + PaddingE.GetOf({TP_CONTROLS}).Vertical(1.5)
                    CalculateSize(h)
                    Dim hh% = Screen.PrimaryScreen.WorkingArea.Height - 20
                    If s.Height > hh Then
                        h = 5 * rh + PaddingE.GetOf({TP_CONTROLS}).Vertical(1.5)
                        CalculateSize(h)
                        With TP_CONTROLS
                            .AutoSizeMode = AutoSizeMode.GrowAndShrink
                            .AutoScroll = True
                            Dim p As Padding = .Padding
                            p.Right += 3
                            .Padding = p
                            .VerticalScroll.Visible = True
                            .VerticalScroll.Enabled = True
                            .HorizontalScroll.Visible = False
                            .HorizontalScroll.Enabled = False
                        End With
                    End If

                    With TP_CONTROLS
                        .PerformLayout()
                        .Select()
                        If .Controls.Count > 0 Then .Controls(0).Focus()
                    End With
                End If
            End With

            If s.Height = -1 Then s.Height = rh
            MaximumSize = Nothing
            MinimumSize = Nothing
            Size = s
            MinimumSize = Size
            MaximumSize = Size
        End Sub
#End Region
#Region "Media items' handlers"
        Private Sub MediaItem_CheckedChanged(ByVal Sender As MediaItem, ByVal Container As IYouTubeMediaContainer)
            ControlInvokeFast(TP_CONTROLS, Sub() Container.Checked = Sender.Checked, EDP.None)
        End Sub
#End Region
#Region "OK, Cancel"
        Private Sub BTT_DOWN_Click(sender As Object, e As EventArgs) Handles BTT_DOWN.Click
            Try
                Dim f As SFile
                If MyContainer.HasElements Then
                    f = TXT_FILE.Text.CSFileP
                Else
                    f = TXT_FILE.Text
                End If
                If f.IsEmptyString Then Throw New ArgumentNullException("File", "The output file cannot be null")
                With MyContainer
                    .OutputVideoExtension = CMB_FORMAT.Text.StringToLower
                    .OutputAudioCodec = CMB_AUDIO_CODEC.Text.StringToLower
                    .OutputSubtitlesFormat = CMB_SUBS_FORMAT.Text.StringToLower

                    If Not .HasElements Then
                        Dim cntIndex% = -1
                        With TP_CONTROLS.Controls
                            If .Count > 0 Then
                                For Each cnt As VideoOption In .Self
                                    If cnt.Checked Then cntIndex = cnt.MyMedia.Index : Exit For
                                Next
                            End If
                        End With
                        If cntIndex = -1 Then Throw New ArgumentOutOfRangeException("Download option", "What to download is not selected")
                        If OPT_VIDEO.Checked Then
                            .SelectedVideoIndex = cntIndex
                        Else
                            .SelectedVideoIndex = -1
                            .SelectedAudioIndex = cntIndex
                        End If
                        .File = f
                        .FileSetManually = True
                        .UpdateInfoFields()
                        '#If DEBUG Then
                        'Debug.WriteLine(.Command(False))
                        '#End If
                    Else
                        If OPT_AUDIO.Checked Then
                            .SetMaxResolution(-2)
                        Else
                            .SetMaxResolution(NUM_RES.Value)
                        End If
                        .File = f
                    End If
                End With

                If MyYouTubeSettings.OutputPathAutoChange Then MyYouTubeSettings.OutputPath.Value = f
                If MyDownloaderSettings.OutputPathAutoAddPaths Then MyYouTubeSettings.DownloadLocations.Add(f, False)

                DialogResult = DialogResult.OK
                Close()
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog + EDP.ShowMainMsg, ex, $"Download {IIf(MyContainer.HasElements, "playlist", "video")}")
            End Try
        End Sub
        Private Sub BTT_CANCEL_Click(sender As Object, e As EventArgs) Handles BTT_CANCEL.Click
            DialogResult = DialogResult.Cancel
            Close()
        End Sub
#End Region
#Region "Controls' handlers"
#Region "Header"
        Private Sub LBL_URL_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LBL_URL.LinkClicked
            If Not LBL_URL.Text.IsEmptyString Then
                Try : Process.Start(LBL_URL.Text) : Catch : End Try
            End If
        End Sub
#End Region
#Region "Settings"
        Private Sub OPT_VIDEO_AUDIO_CheckedChanged(sender As Object, e As EventArgs) Handles OPT_VIDEO.CheckedChanged, OPT_AUDIO.CheckedChanged
            If Not Initialization Then
                CMB_FORMAT.Enabled = OPT_VIDEO.Checked
                If MyContainer.HasElements Then
                    NUM_RES.Enabled = OPT_VIDEO.Checked
                Else
                    RefillList()
                    If OPT_VIDEO.Checked Then
                        ChangeFileExtension(CMB_FORMAT.Text)
                    Else
                        ChangeFileExtension(CMB_AUDIO_CODEC.Text)
                    End If
                End If
            End If
        End Sub
        Private Sub CMB_FORMAT_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CMB_FORMAT.SelectedIndexChanged
            If Not Initialization AndAlso OPT_VIDEO.Checked Then ChangeFileExtension(CMB_FORMAT.Text)
        End Sub
        Private Sub CMB_AUDIO_CODEC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CMB_AUDIO_CODEC.SelectedIndexChanged
            If Not Initialization AndAlso OPT_AUDIO.Checked Then ChangeFileExtension(CMB_AUDIO_CODEC.Text)
        End Sub
        Private Sub ChangeFileExtension(ByVal NewExt As String)
            If Not MyContainer.HasElements Then
                Dim f As SFile = TXT_FILE.Text
                f.Extension = NewExt.StringToLower
                TXT_FILE.Text = f
            End If
        End Sub
        Private Sub TXT_SUBS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles TXT_SUBS.ActionOnButtonClick
            Select Case Sender.DefaultButton
                Case ADB.Open
                    If MyContainer.Subtitles.Count > 0 Then
                        Using f As New SimpleListForm(Of String)(MyContainer.Subtitles.Select(Function(s) s.Name)) With {
                            .DesignXML = DesignXML,
                            .DesignXMLNodeName = SimpleArraysFormNode,
                            .Mode = SimpleListFormModes.CheckedItems,
                            .FormText = "Subtitles"
                        }
                            With MyContainer
                                If .SubtitlesSelectedIndexes.Count > 0 Then f.DataSelectedIndexes.AddRange(.SubtitlesSelectedIndexes)
                                If f.ShowDialog() = DialogResult.OK Then
                                    .SubtitlesSelectedIndexes.Clear()
                                    If f.DataResultIndexes.Count > 0 Then .SubtitlesSelectedIndexes.AddRange(f.DataResultIndexes)
                                    RefillTextBoxes()
                                End If
                            End With
                        End Using
                    End If
                Case ADB.Refresh
                    MyContainer.SubtitlesSelectedIndexesReset()
                    RefillTextBoxes()
                Case ADB.Clear
                    MyContainer.SubtitlesSelectedIndexes.Clear()
                    RefillTextBoxes()
            End Select
        End Sub
        Private Sub CONTROLS_ADDIT_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles TXT_SUBS_ADDIT.ActionOnButtonClick,
                                                                                                                               TXT_EXTRA_AUDIO_FORMATS.ActionOnButtonClick
            Dim isSubs As Boolean = CStr(DirectCast(e.AssociatedControl, TextBoxExtended).Tag) = "s"
            Select Case Sender.DefaultButton
                Case ADB.Open
                    Using f As New SimpleListForm(Of String)(If(isSubs, AvailableSubtitlesFormats, AvailableAudioFormats)) With {
                        .DesignXML = DesignXML,
                        .DesignXMLNodeName = SimpleArraysFormNode,
                        .Mode = SimpleListFormModes.CheckedItems,
                        .FormText = DirectCast(e.AssociatedControl, TextBoxExtended).CaptionText
                    }
                        With MyContainer
                            With If(isSubs, .PostProcessing_OutputSubtitlesFormats, .PostProcessing_OutputAudioFormats)
                                If .Self.Count > 0 Then f.DataSelected.AddRange(.Self)
                                If f.ShowDialog() = DialogResult.OK Then
                                    .Self.Clear()
                                    If f.DataResultIndexes.Count > 0 Then .Self.AddRange(f.DataResult)
                                    DirectCast(e.AssociatedControl, TextBoxExtended).Text = .ListToString
                                    RefillTextBoxes()
                                End If
                            End With
                        End With
                    End Using
                Case ADB.Refresh
                    If isSubs Then
                        MyContainer.PostProcessing_OutputSubtitlesFormats_Reset()
                    Else
                        MyContainer.PostProcessing_OutputAudioFormats_Reset()
                    End If
                    RefillTextBoxes()
                Case ADB.Clear
                    If isSubs Then
                        MyContainer.PostProcessing_OutputSubtitlesFormats.Clear()
                    Else
                        MyContainer.PostProcessing_OutputAudioFormats.Clear()
                    End If
                    RefillTextBoxes()
            End Select
        End Sub
#End Region
#Region "Footer"
        Private Sub BTT_BROWSE_MouseClick(sender As Object, e As MouseEventArgs) Handles BTT_BROWSE.MouseClick
            Dim f As SFile
#Disable Warning BC40000
            If MyContainer.HasElements Then
                f = TXT_FILE.Text.CSFileP
                f = SFile.SelectPath(f, "Select the destination of the video files", EDP.ReturnValue)
            Else
                f = TXT_FILE.Text
                Dim sPattern$ = $"Video|{AvailableVideoFormats.Select(Function(vf) $"*.{vf.ToLower}").ListToString(";")}" &
                                $"|Audio|{AvailableAudioFormats.Select(Function(af) $"*.{af.ToLower}").ListToString(";")}" &
                                "|All Files|*.*"
                f = SFile.SaveAs(f, "Select the destination of the video file",,, sPattern, EDP.ReturnValue)
            End If
#Enable Warning
            If Not f.IsEmptyString Then
                If e.Button = MouseButtons.Right Then
                    MyYouTubeSettings.DownloadLocations.Add(f, MyDownloaderSettings.OutputPathAskForName)
                    MyYouTubeSettings.DownloadLocations.PopulateComboBox(TXT_FILE, f)
                End If
                TXT_FILE.Text = f
            End If
        End Sub
#End Region
#End Region
#Region "Functions"
        Private Sub RefillTextBoxes()
            With MyContainer
                If .SubtitlesSelectedIndexes.Count > 0 Then
                    TXT_SUBS.Text = ListAddList(Nothing, .Subtitles.Select(Function(s, i) If(.SubtitlesSelectedIndexes.Contains(i), s.ID, String.Empty)),
                                                LAP.NotContainsOnly, EDP.ReturnValue).ListToString(",")
                Else
                    TXT_SUBS.Clear()
                End If
                If .PostProcessing_OutputSubtitlesFormats.Count > 0 Then
                    TXT_SUBS_ADDIT.Text = .PostProcessing_OutputSubtitlesFormats.ListToString
                Else
                    TXT_SUBS_ADDIT.Clear()
                End If
                If .PostProcessing_OutputAudioFormats.Count > 0 Then
                    TXT_EXTRA_AUDIO_FORMATS.Text = .PostProcessing_OutputAudioFormats.ListToString
                Else
                    TXT_EXTRA_AUDIO_FORMATS.Clear()
                End If
            End With
        End Sub
#End Region
    End Class
End Namespace