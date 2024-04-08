' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports SCrawler.API.YouTube.Objects
Imports SCrawler.DownloadObjects.STDownloader
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Namespace API.YouTube.Controls
    Friend Class MusicPlaylistsForm : Implements IDesignXMLContainer
#Region "Declarations"
        Private MyView As FormView
        Private ReadOnly MyFieldsChecker As FieldsChecker
        Friend Property DesignXML As EContainer Implements IDesignXMLContainer.DesignXML
        Private Property DesignXMLNodes As String() Implements IDesignXMLContainer.DesignXMLNodes
        Private Property DesignXMLNodeName As String Implements IDesignXMLContainer.DesignXMLNodeName
        Private ReadOnly MyContainer As IYouTubeMediaContainer
        Private ReadOnly M3U8Files As List(Of SFile)
        Private ReadOnly Property M3U8FilesFull As List(Of SFile)
            Get
                Return ListAddList(Nothing, M3U8Files, LAP.NotContainsOnly).ListAddValue(CMB_PLS.Text, LAP.NotContainsOnly)
            End Get
        End Property
        Private Initializing As Boolean = True
        Private ReadOnly Property Current As IYouTubeMediaContainer
            Get
                With MyContainer
                    If .ObjectType = Base.YouTubeMediaType.Channel Then
                        If _LatestSelected.ValueBetween(0, .Count - 1) Then Return .Elements(_LatestSelected)
                    Else
                        Return .Self
                    End If
                End With
                Return Nothing
            End Get
        End Property
#End Region
#Region "Initializer"
        Friend Sub New(ByVal Container As IYouTubeMediaContainer)
            InitializeComponent()
            M3U8Files = New List(Of SFile)
            MyContainer = Container
            MyFieldsChecker = New FieldsChecker
        End Sub
#End Region
#Region "Form handlers"
        Private Sub MusicPlaylistsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            If Not DesignXML Is Nothing Then
                MyView = New FormView(Me)
                MyView.Import()
                MyView.SetFormSize()
            End If

            MyYouTubeSettings.DownloadLocations.PopulateComboBox(TXT_OUTPUT_PATH)
            MyYouTubeSettings.PlaylistsLocations.PopulateComboBox(CMB_PLS,, True)
            CMB_PLS.Text = MyYouTubeSettings.LatestPlaylistFile.Value
            If MyYouTubeSettings.DefaultAudioBitrate > 0 Then TXT_AUDIO_BITRATE.Text = MyYouTubeSettings.DefaultAudioBitrate.Value

            CMB_FORMATS.Items.AddRange(AvailableAudioFormats)
            If MyYouTubeSettings.PlaylistFormSplitterDistance > 0 Then SPLITTER_MAIN.SplitterDistancePercentageSet(MyYouTubeSettings.PlaylistFormSplitterDistance)

            With MyContainer
                CH_DOWN_LYRICS.Checked = Not .OutputSubtitlesFormat.IsEmptyString
                Dim i%
                If Not .OutputAudioCodec.IsEmptyString Then
                    i = AvailableAudioFormats.ListIndexOf(Function(ff) ff.StringToLower = .OutputAudioCodec.StringToLower)
                    If i >= 0 Then CMB_FORMATS.SelectedIndex = i
                End If
                If CMB_FORMATS.SelectedIndex = -1 Then
                    Dim oac$ = MyYouTubeSettings.DefaultAudioCodecMusic.Value.IfNullOrEmpty("mp3").StringToLower
                    i = AvailableAudioFormats.ListIndexOf(Function(ff) ff.StringToLower = oac)
                    If i >= 0 Then CMB_FORMATS.SelectedIndex = i Else CMB_FORMATS.SelectedIndex = 0
                End If

                If .ObjectType = Base.YouTubeMediaType.Channel Then
                    If .HasElements Then
                        For Each elem In .Elements : LIST_PLAYLISTS.Items.Add(elem, elem.CheckState) : Next
                    End If
                ElseIf .ObjectType = Base.YouTubeMediaType.PlayList Then
                    LIST_PLAYLISTS.Items.Add(.Self, .CheckState)
                Else
                    Throw New InvalidOperationException($"The object type '{ .ObjectType}' is incompatible with 'MusicPlaylistsForm'.")
                End If
                LIST_PLAYLISTS.SelectedIndex = 0

                If .ObjectType = Base.YouTubeMediaType.Channel Then
                    With TXT_OUTPUT_PATH
                        .CaptionMode = ICaptionControl.Modes.Label
                        .CaptionToolTipText = String.Empty
                        .CaptionToolTipEnabled = False
                    End With
                End If

                TXT_OUTPUT_PATH.Text = MyYouTubeSettings.OutputPath.Value

                If Not .UserTitle.IsEmptyString Then
                    Text = .UserTitle
                    If .ObjectType = Base.YouTubeMediaType.PlayList Then
                        If Not .PlaylistTitle.IsEmptyString AndAlso Not .PlaylistTitle = .UserTitle Then
                            Text &= $" - { .PlaylistTitle}"
                        ElseIf Not .Title.IsEmptyString AndAlso Not .Title = .UserTitle Then
                            Text &= $" - { .Title}"
                        End If
                    End If
                    If Not TXT_OUTPUT_PATH.IsEmptyString AndAlso Not TXT_OUTPUT_PATH.Text.Contains(.UserTitle) Then TXT_OUTPUT_PATH.Text = $"{TXT_OUTPUT_PATH.Text.TrimEnd("\")}\{ .UserTitle}\"
                ElseIf Not .PlaylistTitle.IsEmptyString Then
                    Text = .PlaylistTitle
                End If

                MyFieldsChecker.AddControl(Of Integer)(TXT_AUDIO_BITRATE, TXT_AUDIO_BITRATE.CaptionText, True)
                MyFieldsChecker.EndLoaderOperations()

                UpdateSizeText()
            End With
            RefillAddit()
            Initializing = False
        End Sub
        Private Sub MusicPlaylistsForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            MyYouTubeSettings.PlaylistFormSplitterDistance.Value = SPLITTER_MAIN.SplitterDistancePercentageGet
            MyView.DisposeIfReady
            MyFieldsChecker.DisposeIfReady
            M3U8Files.Clear()
        End Sub
        Private Sub MusicPlaylistsForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            Dim b As Boolean = True
            If e.KeyCode = Keys.O And e.Control Then
                MyYouTubeSettings.DownloadLocations.ChooseNewLocation(TXT_OUTPUT_PATH, False, MyDownloaderSettings.OutputPathAskForName)
            ElseIf e.KeyCode = Keys.O And e.Alt Then
                MyYouTubeSettings.DownloadLocations.ChooseNewLocation(TXT_OUTPUT_PATH, True, MyDownloaderSettings.OutputPathAskForName)
            Else
                b = False
            End If
            If b Then e.Handled = True
        End Sub
#End Region
#Region "Form text"
        Private _InitialFormText As String = String.Empty
        Private Sub UpdateSizeText()
            If _InitialFormText.IsEmptyString Then _InitialFormText = Text
            Text = $"{_InitialFormText} ({MyContainer.SizeStr})"
        End Sub
#End Region
#Region "Settings"
        Private Sub TXT_SUBS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles TXT_SUBS.ActionOnButtonClick, TXT_FORMATS_ADDIT.ActionOnButtonClick
            Dim isLyrics As Boolean = DirectCast(e.AssociatedControl, Control).Tag = "s"
            With DirectCast(MyContainer, YouTubeMediaContainerBase)
                Select Case Sender.DefaultButton
                    Case ADB.Open
                        Using f As New SimpleListForm(Of String)(IIf(isLyrics, AvailableSubtitlesFormats, AvailableAudioFormats)) With {
                            .DesignXML = DesignXML,
                            .DesignXMLNodeName = SimpleArraysFormNode,
                            .FormText = DirectCast(e.AssociatedControl, TextBoxExtended).CaptionText,
                            .Icon = My.Resources.SiteYouTube.YouTubeMusicIcon_32,
                            .Mode = SimpleListFormModes.CheckedItems
                        }
                            f.DataSelected.ListAddList(IIf(isLyrics, .PostProcessing_OutputSubtitlesFormats, .PostProcessing_OutputAudioFormats))
                            If f.ShowDialog = DialogResult.OK Then
                                If isLyrics Then
                                    .PostProcessing_OutputSubtitlesFormats.Clear()
                                    .PostProcessing_OutputSubtitlesFormats.ListAddList(f.DataResult)
                                Else
                                    .PostProcessing_OutputAudioFormats.Clear()
                                    .PostProcessing_OutputAudioFormats.ListAddList(f.DataResult)
                                End If
                                RefillAddit()
                            End If
                        End Using
                    Case ADB.Refresh
                        If isLyrics Then
                            .PostProcessing_OutputSubtitlesFormats_Reset()
                        Else
                            .PostProcessing_OutputAudioFormats_Reset()
                        End If
                        RefillAddit()
                    Case ADB.Clear
                        If isLyrics Then
                            .PostProcessing_OutputSubtitlesFormats.Clear()
                        Else
                            .PostProcessing_OutputAudioFormats.Clear()
                        End If
                        RefillAddit()
                End Select
            End With
        End Sub
        Private Sub RefillAddit()
            With DirectCast(MyContainer, YouTubeMediaContainerBase)
                TXT_SUBS.Text = .PostProcessing_OutputSubtitlesFormats.ListToString
                TXT_FORMATS_ADDIT.Text = .PostProcessing_OutputAudioFormats.ListToString
            End With
        End Sub
        Private Sub TXT_OUTPUT_PATH_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles TXT_OUTPUT_PATH.ActionOnButtonClick
            Select Case e.DefaultButton
                Case ADB.Open, ADB.Add
                    MyYouTubeSettings.DownloadLocations.ChooseNewLocation(TXT_OUTPUT_PATH, e.DefaultButton = ADB.Add, MyDownloaderSettings.OutputPathAskForName)
                Case ADB.Save
                    If Not TXT_OUTPUT_PATH.Text.IsEmptyString Then
                        With MyYouTubeSettings.PlaylistsLocations
                            .Add(TXT_OUTPUT_PATH.Text, True)
                            .PopulateComboBox(TXT_OUTPUT_PATH, TXT_OUTPUT_PATH.Text)
                        End With
                    End If
            End Select
        End Sub
        Private Sub CMB_PLS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles CMB_PLS.ActionOnButtonClick
            Try
                Select Case e.DefaultButton
                    Case ADB.Add, ADB.Open
                        Dim f As SFile = Nothing
                        If Not CMB_PLS.Text.IsEmptyString Then
                            f = CMB_PLS.Text
                        ElseIf Not TXT_OUTPUT_PATH.Text.IsEmptyString Then
                            f = TXT_OUTPUT_PATH.Text
                        End If
                        f = SFile.SelectFiles(f, False, "Select a playlist...", "Playlists|*.m3u;*.m3u8|All files|*.*", EDP.ReturnValue).FirstOrDefault
                        If Not f.IsEmptyString Then
                            If Sender.DefaultButton = ADB.Add Then
                                MyYouTubeSettings.PlaylistsLocations.Add(f.ToString, True, True)
                                MyYouTubeSettings.PlaylistsLocations.PopulateComboBox(CMB_PLS, f, True)
                            End If
                            CMB_PLS.Text = f
                        End If
                    Case ADB.List
                        Dim result As Boolean = False
                        Dim selectedFiles As IEnumerable(Of SFile) = MyYouTubeSettings.PlaylistsLocations.ChooseNewPlaylistArray(CMB_PLS, result)
                        If result Then M3U8Files.ListAddList(selectedFiles, LAP.NotContainsOnly, LAP.ClearBeforeAdd)
                    Case ADB.Save
                        With MyYouTubeSettings.PlaylistsLocations
                            If Not CMB_PLS.Text.IsEmptyString AndAlso .IndexOf(CMB_PLS.Text,, True) = -1 Then
                                .Add(CMB_PLS.Text, True, True)
                                .PopulateComboBox(CMB_PLS, CMB_PLS.Text, True)
                            End If
                        End With
                    Case ADB.Clear : M3U8Files.Clear()
                End Select
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[API.YouTube.Controls.MusicPlaylistsForm.SelectPlaylist]")
            End Try
        End Sub
#End Region
#Region "Lists' handlers"
        Private _LatestSelected As Integer = -1
        Private Sub LIST_PLAYLISTS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LIST_PLAYLISTS.SelectedIndexChanged
            Dim i% = LIST_PLAYLISTS.SelectedIndex
            If i >= 0 Then
                _LatestSelected = i
                LIST_ITEMS.Items.Clear()
                With DirectCast(LIST_PLAYLISTS.SelectedItem, IYouTubeMediaContainer)
                    If .HasElements Then
                        For Each elem In .Elements : LIST_ITEMS.Items.Add(elem, elem.Checked) : Next
                    End If
                End With
            End If
        End Sub
        Private _CheckHandlersSuspended As Boolean = False
        Private Sub LIST_PLAYLISTS_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles LIST_PLAYLISTS.ItemCheck
            If Not Initializing And Not _CheckHandlersSuspended Then
                _CheckHandlersSuspended = True

                Dim checked As Boolean = Not e.NewValue = CheckState.Unchecked

                Dim current As IYouTubeMediaContainer = Me.Current
                If Not current Is Nothing Then
                    With current
                        .Checked = checked
                        If LIST_ITEMS.Items.Count > 0 Then
                            _ListCheckHandlersSuspended = True
                            For i% = 0 To .Count - 1
                                If i.ValueBetween(0, LIST_ITEMS.Items.Count - 1) Then LIST_ITEMS.SetItemChecked(i, checked)
                            Next
                            _ListCheckHandlersSuspended = False
                        End If
                        If LIST_PLAYLISTS.Items.Count.ValueBetween(0, _LatestSelected) Then LIST_PLAYLISTS.SetItemChecked(_LatestSelected, checked)
                    End With
                    UpdateSizeText()
                End If

                _CheckHandlersSuspended = False
            End If
        End Sub
        Private _ListCheckHandlersSuspended As Boolean = False
        Private Sub LIST_ITEMS_ItemCheck(sender As Object, e As ItemCheckEventArgs) Handles LIST_ITEMS.ItemCheck
            If Not Initializing Then
                Dim current As IYouTubeMediaContainer = Me.Current
                If Not current Is Nothing Then
                    With current
                        If e.Index.ValueBetween(0, .Count - 1) Then
                            .Elements(e.Index).Checked = e.NewValue
                            If Not _ListCheckHandlersSuspended And _LatestSelected.ValueBetween(0, LIST_PLAYLISTS.Items.Count - 1) Then
                                Dim checked As Boolean = .Elements(e.Index).Checked
                                _CheckHandlersSuspended = True
                                If .Elements.All(Function(ee) ee.Checked = checked) Then
                                    LIST_PLAYLISTS.SetItemChecked(_LatestSelected, checked)
                                Else
                                    LIST_PLAYLISTS.SetItemCheckState(_LatestSelected, CheckState.Indeterminate)
                                End If
                                _CheckHandlersSuspended = False
                                LIST_PLAYLISTS.Refresh()
                                UpdateSizeText()
                            End If
                        End If
                    End With
                End If
            End If
        End Sub
#End Region
#Region "Selection buttons"
        Private Sub BTT_PLS_Click(sender As Object, e As EventArgs) Handles BTT_PLS_ALL.Click, BTT_PLS_NONE.Click
            Dim checked As Boolean = DirectCast(sender, Button).Tag = "a"
            _CheckHandlersSuspended = True
            If LIST_PLAYLISTS.Items.Count > 0 Then
                For i% = 0 To LIST_PLAYLISTS.Items.Count - 1 : LIST_PLAYLISTS.SetItemChecked(i, checked) : Next
            End If
            MyContainer.Checked = checked
            If MyContainer.Count > 1 Then MyContainer.Elements.ForEach(Sub(ee) ee.Checked = checked)
            _CheckHandlersSuspended = False
            UpdateSizeText()
        End Sub
#End Region
#Region "Ok, Cancel"
        Private Sub BTT_DOWN_Click(sender As Object, e As EventArgs) Handles BTT_DOWN.Click
            If TXT_OUTPUT_PATH.IsEmptyString Then
                MsgBoxE({"The output path cannot be null.", "Download music"}, vbCritical)
            ElseIf MyFieldsChecker.AllParamsOK Then
                With DirectCast(MyContainer, YouTubeMediaContainerBase)
                    .OutputSubtitlesFormat = IIf(CH_DOWN_LYRICS.Checked, "LRC", String.Empty)
                    If Not TXT_SUBS.Checked Then .PostProcessing_OutputSubtitlesFormats.Clear()
                    .OutputAudioCodec = CMB_FORMATS.Text
                    If Not TXT_FORMATS_ADDIT.Checked Then .PostProcessing_OutputAudioFormats.Clear()
                    .AbsolutePath = TXT_OUTPUT_PATH.Checked
                    .File = TXT_OUTPUT_PATH.Text.CSFileP
                    .M3U8_PlaylistFiles = M3U8FilesFull
                    .OutputAudioBitrate = AConvert(Of Integer)(TXT_AUDIO_BITRATE.Text, -1)
                    If MyYouTubeSettings.OutputPathAutoChange Then MyYouTubeSettings.OutputPath.Value = .File
                    If MyDownloaderSettings.OutputPathAutoAddPaths Then MyYouTubeSettings.DownloadLocations.Add(.File, False)
                    If Not CMB_PLS.Text.IsEmptyString Then MyYouTubeSettings.PlaylistsLocations.Add(CMB_PLS.Text, False, True)
                    MyYouTubeSettings.LatestPlaylistFile.Value = CMB_PLS.Text
                End With
                DialogResult = DialogResult.OK
                Close()
            End If
        End Sub
        Private Sub BTT_CANCEL_Click(sender As Object, e As EventArgs) Handles BTT_CANCEL.Click
            DialogResult = DialogResult.Cancel
            Close()
        End Sub
#End Region
    End Class
End Namespace