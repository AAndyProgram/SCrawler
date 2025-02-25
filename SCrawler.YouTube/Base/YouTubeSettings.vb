' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Globalization
Imports System.Drawing.Design
Imports System.ComponentModel
Imports SCrawler.API.YouTube.Attributes
Imports SCrawler.DownloadObjects.STDownloader
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Functions.XML.Objects
Imports PersonalUtilities.Functions.XML.Attributes
Imports PersonalUtilities.Functions.XML.Attributes.Specialized
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.Grid.Base
Imports PersonalUtilities.Tools.Grid.Attributes
Imports PersonalUtilities.Tools.Grid.Collections
Imports PersonalUtilities.Tools.Grid.Specialized
Imports PersonalUtilities.Tools.Web.Cookies
Namespace API.YouTube.Base
    <TypeDescriptionProvider(GetType(FieldsTypeDescriptorProvider))>
    Public Class YouTubeSettings : Implements IXMLValuesContainer, IGridValuesContainer, IDownloaderSettings
#Region "Events"
        Private Event OnBeginUpdate As EventHandler Implements IXMLValuesContainer.OnBeginUpdate
        Private Event OnEndUpdate As EventHandler Implements IXMLValuesContainer.OnEndUpdate
#End Region
#Region "Declarations"
        <Browsable(False)> Private ReadOnly Property XML As XmlFile Implements IXMLValuesContainer.XML
        <Browsable(False)> Friend ReadOnly Property DesignXml As XmlFile
        <Browsable(False)> Private Property Mode As GridUpdateModes = GridUpdateModes.OnConfirm Implements IGridValuesContainer.Mode
        <Browsable(False), XMLVV(-1)> Friend ReadOnly Property PlaylistFormSplitterDistance As XMLValue(Of Integer)
        <Browsable(False)> Friend ReadOnly Property DownloadLocations As DownloadLocationsCollection
        <Browsable(False)> Friend ReadOnly Property PlaylistsLocations As DownloadLocationsCollection
        <Browsable(False)> Public Overridable Property AccountName As String
        <Browsable(False), XMLVV(0)> Private ReadOnly Property SettingsVersion As XMLValue(Of Integer)
#Region "Environment"
#Region "Programs"
        <Browsable(True), GridVisible(False), XMLVN({"Environment"}), Category("Environment programs"), DisplayName("Path to yt-dlp.exe"),
            Description("Path to yt-dlp.exe file")>
        Public ReadOnly Property YTDLP As XMLValue(Of SFile)
        <Browsable(True), GridVisible(False), XMLVN({"Environment"}), Category("Environment programs"), DisplayName("Path to ffmpeg.exe"),
            Description("Path to ffmpeg.exe file")>
        Public ReadOnly Property FFMPEG As XMLValue(Of SFile)
        <Browsable(False)> Private ReadOnly Property ENVIR_FFMPEG As SFile Implements IDownloaderSettings.ENVIR_FFMPEG
            Get
                Return FFMPEG
            End Get
        End Property
        <Browsable(False)> Private ReadOnly Property ENVIR_YTDLP As SFile Implements IDownloaderSettings.ENVIR_YTDLP
            Get
                Return YTDLP
            End Get
        End Property
        <Browsable(False)> Private ReadOnly Property ENVIR_GDL As SFile Implements IDownloaderSettings.ENVIR_GDL
            Get
                Return Nothing
            End Get
        End Property
        <Browsable(False)> Private ReadOnly Property ENVIR_CURL As SFile Implements IDownloaderSettings.ENVIR_CURL
            Get
                Return Nothing
            End Get
        End Property
#End Region
        <Browsable(True), GridVisible(False), Category("Environment"), Description("YouTube cookies"), GridCollectionForm(GetType(CookieListForm2)),
            EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)>
        Public ReadOnly Property Cookies As CookieKeeper
        Private Function ShouldSerializeCookies() As Boolean
            Return Cookies.Count > 0
        End Function
        Private Sub ResetCookies()
            Cookies.Clear()
        End Sub
        Private Class CookieListForm2 : Inherits CookieListForm
            Public Sub New()
                ShowGrid = False
            End Sub
        End Class
        <Browsable(True), GridVisible(False), XMLVN({"Environment"}, Provider:=GetType(XMLToFilePathProvider)), Category("Environment"), DisplayName("Output path"),
            Description("The default output path where files should be downloaded."),
            Editor(GetType(GridSFileTypeEditorPath), GetType(UITypeEditor))>
        Public ReadOnly Property OutputPath As XMLValue(Of SFile)
        <Browsable(True), GridVisible(False), XMLVN({"Environment"}), Category("Environment"), DisplayName("Playlist file"),
            Description("Last selected playlist file"),
            Editor(GetType(GridSFileTypeEditor_M3U8), GetType(UITypeEditor))>
        Public ReadOnly Property LatestPlaylistFile As XMLValue(Of SFile)
        Private Class GridSFileTypeEditor_M3U8 : Inherits GridSFileTypeEditor
            Public Overrides Function EditValue(ByVal Context As ITypeDescriptorContext, ByVal Provider As IServiceProvider, ByVal Value As Object) As Object
                Dim f As SFile = SFile.SelectFiles(New SFile(CStr(AConvert(Of String)(Value, AModes.Var, String.Empty))), False,
                                                   "Select playlist file", "Playlists|*.m3u;*.m3u8|All files|*.*", EDP.ReturnValue).FirstOrDefault()
                If Not f.IsEmptyString() Then Value = f
                Return Value
            End Function
        End Class
        <Browsable(True), GridVisible(False), XMLVN({"Environment"}), Category("Environment"), DisplayName("Output path auto change"),
            Description("Automatically change the output path when a new destination is selected in the opening forms.")>
        Public ReadOnly Property OutputPathAutoChange As XMLValue(Of Boolean)
        <Browsable(True), GridVisible(False), XMLVN({"Environment"}, True), Category("Environment"), DisplayName("Output path: ask for a name"),
            Description("Ask for a name when adding a new output path to the list.")>
        Public ReadOnly Property OutputPathAskForName As XMLValue(Of Boolean)
        Private ReadOnly Property IDownloaderSettings_OutputPathAskForName As Boolean Implements IDownloaderSettings.OutputPathAskForName
            Get
                Return OutputPathAskForName
            End Get
        End Property
        <Browsable(True), GridVisible(False), XMLVN({"Environment"}, True), Category("Environment"), DisplayName("Output path: auto add"),
            Description("Add new paths to the list automatically.")>
        Public ReadOnly Property OutputPathAutoAddPaths As XMLValue(Of Boolean)
        Private ReadOnly Property IDownloaderSettings_OutputPathAutoAddPaths As Boolean Implements IDownloaderSettings.OutputPathAutoAddPaths
            Get
                Return OutputPathAutoAddPaths
            End Get
        End Property
        <Browsable(True), GridVisible(False), XMLVN({"Environment"}, DoubleClickBehavior.Folder), Category("Environment"), DisplayName("On item double click"),
            Description("What should program open when you double-click on an item...")>
        Public ReadOnly Property OnItemDoubleClick As XMLValue(Of DoubleClickBehavior)
        Private ReadOnly Property IDownloaderSettings_OnItemDoubleClick As DoubleClickBehavior Implements IDownloaderSettings.OnItemDoubleClick
            Get
                Return OnItemDoubleClick
            End Get
        End Property
        <Browsable(False), GridVisible(False), XMLVN({"Environment"}), Category("Environment"), DisplayName("Open folders in another program"),
            Description("The command to open a folder.")>
        Public ReadOnly Property OpenFolderInOtherProgram As XMLValueUse(Of String)
        <Browsable(True), GridVisible(False), Category("EnvironmentFolder"), DisplayName("Open folders in another program"), DefaultValue(False)>
        Private Property IDownloaderSettings_OpenFolderInOtherProgram As Boolean Implements IDownloaderSettings.OpenFolderInOtherProgram
            Get
                Return OpenFolderInOtherProgram.Attribute.ValueTemp
            End Get
            Set(ByVal use As Boolean)
                OpenFolderInOtherProgram.Attribute.ValueTemp = use
            End Set
        End Property
        Private Function ShouldSerializeIDownloaderSettings_OpenFolderInOtherProgram() As Boolean
            Return DirectCast(OpenFolderInOtherProgram.Attribute, IGridValue).ShouldSerializeValue
        End Function
        <Browsable(True), GridVisible(False), Category("EnvironmentFolder"), DisplayName("Open folders in another program (command)"),
            Description("The command to open a folder."), DefaultValue("")>
        Private Property IDownloaderSettings_OpenFolderInOtherProgram_Command As String Implements IDownloaderSettings.OpenFolderInOtherProgram_Command
            Get
                Return OpenFolderInOtherProgram.ValueTemp
            End Get
            Set(ByVal command As String)
                OpenFolderInOtherProgram.ValueTemp = command
            End Set
        End Property
        Private Function ShouldSerializeIDownloaderSettings_OpenFolderInOtherProgram_Command() As Boolean
            Return DirectCast(OpenFolderInOtherProgram, IGridValue).ShouldSerializeValue
        End Function
        <Browsable(True), GridVisible(False), XMLVN({"Environment"}, True), Category("Environment"), DisplayName("Check new version at start")>
        Friend ReadOnly Property CheckUpdatesAtStart As XMLValue(Of Boolean)
#End Region
#Region "Info"
        <Browsable(True), GridVisible, XMLVN({"Info"}), Category("Info"), DisplayName("Create URL files"),
            Description("Create local URL files to link to the original page. Default: false.")>
        Public ReadOnly Property CreateUrlFiles As XMLValue(Of Boolean)
        Private ReadOnly Property IDownloaderSettings_CreateUrlFiles As Boolean Implements IDownloaderSettings.CreateUrlFiles
            Get
                Return CreateUrlFiles
            End Get
        End Property
        <Browsable(True), GridVisible, XMLVN({"Info"}), Category("Info"), DisplayName("Create description files"),
            Description("Create video description files. Default: false.")>
        Public ReadOnly Property CreateDescriptionFiles As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"Info"}, True), Category("Info"), DisplayName("Create description files: create without description"),
            Description("Create a description file with the upload date, even if the description does not exist. Default: true.")>
        Public ReadOnly Property CreateDescriptionFiles_CreateWithNoDescription As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"Info"}, True), Category("Info"), DisplayName("Create thumbnail files (video)"),
            Description("Create video thumbnail files. Default: true.")>
        Public ReadOnly Property CreateThumbnails_Video As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"Info"}, True), Category("Info"), DisplayName("Create thumbnail files (music)"),
            Description("Create music thumbnail files (covers). Default: true.")>
        Public ReadOnly Property CreateThumbnails_Music As XMLValue(Of Boolean)
#End Region
#Region "Defaults"
        <Browsable(True), GridVisible, XMLVN({"Defaults"}, True), Category("Defaults"), DisplayName("Standardize URLs"),
            Description("Standardize URLs by eliminating unwanted strings. Default: true.")>
        Public ReadOnly Property StandardizeURLs As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"Defaults"}), Category("Defaults"), DisplayName("Replace modification date"),
            Description("Set the file date to the date the video was added (website) (if available). Default: false.")>
        Public ReadOnly Property ReplaceModificationDate As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"Defaults"}), Category("Defaults"), DisplayName("Use cookies"),
            Description("By default, use cookies when downloading from YouTube.")>
        Public ReadOnly Property DefaultUseCookies As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"Defaults"}, Protocols.https), Category("Defaults"), DisplayName("Protocol"),
            Description("Priority download protocol. Default: 'Any'")>
        Public ReadOnly Property DefaultProtocol As XMLValue(Of Protocols)
        <Browsable(True), GridVisible(False), XMLVN({"Defaults"}), Category("Defaults"),
            DisplayName("Auto remove"), Description("Automatically remove downloaded items from the list.")>
        Public ReadOnly Property RemoveDownloadedAutomatically As XMLValue(Of Boolean)
        Private ReadOnly Property IDownloaderSettings_RemoveDownloadedAutomatically As Boolean Implements IDownloaderSettings.RemoveDownloadedAutomatically
            Get
                Return RemoveDownloadedAutomatically
            End Get
        End Property
        <Browsable(True), GridVisible(False), XMLVN({"Defaults"}, True), Category("Defaults"), DisplayName("Download Automatically"),
            Description("Download automatically when a new item is added.")>
        Public ReadOnly Property DownloadAutomatically As XMLValue(Of Boolean)
        Private ReadOnly Property IDownloaderSettings_DownloadAutomatically As Boolean Implements IDownloaderSettings.DownloadAutomatically
            Get
                Return DownloadAutomatically
            End Get
        End Property
        <Browsable(True), GridVisible(False), XMLVN({"Defaults"}, 1), Category("Defaults"), DisplayName("Max downloads"),
            Description("Maximum active downloads.")>
        Public ReadOnly Property MaxJobsCount As XMLValue(Of Integer)
        Private ReadOnly Property IDownloaderSettings_MaxJobsCount As Integer Implements IDownloaderSettings.MaxJobsCount
            Get
                Return MaxJobsCount
            End Get
        End Property
        <Browsable(True), GridVisible(False), XMLVN({"Defaults"}, True), Category("Defaults"), DisplayName("Show notifications"),
            Description("Show a notification when the download is complete. The default value is true.")>
        Public ReadOnly Property ShowNotifications As XMLValue(Of Boolean)
        Private ReadOnly Property IDownloaderSettings_ShowNotifications As Boolean Implements IDownloaderSettings.ShowNotifications
            Get
                Return ShowNotifications
            End Get
        End Property
        <Browsable(True), GridVisible(False), XMLVN({"Defaults"}, True), Category("Defaults"), DisplayName("Show notifications every time"),
            Description("If true, notifications will be shown every time a file download is complete; " &
                        "otherwise, the notification will only be shown when all downloads are completed. " &
                        "Only works if 'Show notifications' is true." &
                        "The default value is true.")>
        Public ReadOnly Property ShowNotificationsEveryDownload As XMLValue(Of Boolean)
        Private ReadOnly Property IDownloaderSettings_ShowNotificationsEveryDownload As Boolean Implements IDownloaderSettings.ShowNotificationsEveryDownload
            Get
                Return ShowNotifications And ShowNotificationsEveryDownload
            End Get
        End Property
        Private Sub ShowNotificationsEveryDownload_TempValueChanged(ByVal Sender As Object, ByVal e As EventArgs)
            If ShowNotificationsEveryDownload.ValueTemp Then ShowNotifications.ValueTemp = True
        End Sub
        <Browsable(True), GridVisible(False), XMLVN({"Defaults"}, False), Category("Defaults"), DisplayName("Close to tray"),
            Description("Close the program to tray.")>
        Public ReadOnly Property CloseToTray As XMLValue(Of Boolean)
        <Browsable(True), GridVisible(False), XMLVN({"Defaults"}, False), Category("Defaults"), DisplayName("Confirm exit"),
            Description("Exit confirmation when closing the program.")>
        Public ReadOnly Property ExitConfirm As XMLValue(Of Boolean)
        <Browsable(True), GridVisible(False), XMLVN({"Defaults"}), Category("Defaults"), DisplayName("Download on click in tray: show form"),
            Description("Show main window when download by clicking (Ctrl+Click) the tray icon. Default: false")>
        Public ReadOnly Property ShowFormDownTrayClick As XMLValue(Of Boolean)
        <Browsable(True), GridVisible(False), XMLVN({"Defaults"}), Category("Defaults"), DisplayName("Program title"),
            Description("Change the title of the main window if you need to")>
        Friend ReadOnly Property ProgramText As XMLValue(Of String)
        <Browsable(True), GridVisible(False), XMLVN({"Defaults"}), Category("Defaults"), DisplayName("Program description"),
            Description("Add some additional info to the program info if you need")>
        Friend ReadOnly Property ProgramDescription As XMLValue(Of String)
        <Browsable(True), GridVisible, XMLVN({"Defaults"}, "%"""), Category("Defaults"), DisplayName("Remove characters"),
            Description("Remove specific characters from a file name")>
        Public ReadOnly Property FileRemoveCharacters As XMLValue(Of String)
        <Browsable(True), GridVisible, XMLVN({"Defaults"}, FileDateMode.None), Category("Defaults"), DisplayName("Add date to file name"),
            Description("Add the video upload date before/after the file name")>
        Public ReadOnly Property FileAddDateToFileName As XMLValue(Of FileDateMode)
        <Browsable(True), GridVisible, XMLVN({"Defaults"}), Category("Defaults"), DisplayName("Add date to title: video form"),
            Description("Add video upload date before video title (visual only) in the video form")>
        Public ReadOnly Property FileAddDateToFileName_VideoForm As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"Defaults"}), Category("Defaults"), DisplayName("Add date to title: video list"),
            Description("Add video upload date before video title (visual only) in the video list")>
        Public ReadOnly Property FileAddDateToFileName_VideoList As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"Defaults"}, FileDateMode.None), Category("Defaults"), DisplayName("Add channel to file name"),
            Description("Add channel name before/after the file name")>
        Public ReadOnly Property FileAddChannelToFileName As XMLValue(Of FileDateMode)
#End Region
#Region "Defaults ChannelsDownload"
        <Browsable(True), GridVisible, XMLVN({"Defaults", "Channels"}), Category("Defaults"), DisplayName("Default download tabs for channels"),
            Description("Default download tabs for downloading channels"), TypeConverter(GetType(YouTubeChannelTabConverter))>
        Public ReadOnly Property ChannelsDownload As XMLValue(Of YouTubeChannelTab)
        Private Class YouTubeChannelTabConverter : Inherits TypeConverter
            Public Overrides Function ConvertTo(ByVal Context As ITypeDescriptorContext, ByVal Culture As CultureInfo, ByVal Value As Object,
                                                ByVal DestinationType As Type) As Object
                If Not DestinationType Is Nothing Then
                    If DestinationType Is GetType(String) Then
                        If IsNothing(Value) Then
                            Return YouTubeChannelTab.All.ToString
                        Else
                            Dim v As List(Of YouTubeChannelTab) = EnumExtract(Of YouTubeChannelTab)(Value,,, EDP.ReturnValue).ListIfNothing
                            If v.ListExists Then
                                v.Sort()
                                Return v.ListToStringE(, New ANumbers.EnumToStringProvider(GetType(YouTubeChannelTab)))
                            Else
                                Return YouTubeChannelTab.All.ToString
                            End If
                        End If
                    Else
                        If IsNothing(Value) Then
                            Return YouTubeChannelTab.All
                        Else
                            Return Value
                        End If
                    End If
                End If
                Return MyBase.ConvertTo(Context, Culture, Value, DestinationType)
            End Function
        End Class
#End Region
#Region "Defaults Video"
        <Browsable(True), GridVisible, XMLVN({"DefaultsVideo"}, "MKV"), Category("Defaults Video"), DisplayName("Default format"),
            TypeConverter(GetType(FieldsTypeConverter)), GridStandardValuesProvider(NameOf(AvailableVideoFormats_Impl)),
            Description("The default video format for downloading videos.")>
        Public ReadOnly Property DefaultVideoFormat As XMLValue(Of String)
        Private Function AvailableVideoFormats_Impl() As String()
            Return AvailableVideoFormats
        End Function
        <Browsable(True), GridVisible, XMLVN({"DefaultsVideo"}, 1080), Category("Defaults Video"), DisplayName("Default definition"),
            Description("The default maximum video resolution. -1 for max definition")>
        Public ReadOnly Property DefaultVideoDefinition As XMLValue(Of Integer)
        <Browsable(True), GridVisible, XMLVN({"DefaultsVideo"}, True), Category("Defaults Video"), DisplayName("Allow webm formats"),
            Description("Allow webm formats over http if mp4 formats are not available. Default: true.")>
        Public ReadOnly Property DefaultVideoAllowWebm As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"DefaultsVideo"}), Category("Defaults Video"), DisplayName("Convert non-AVC codecs to AVC"),
            Description("Convert non-AVC codecs (eg 'VP9') to AVC. Not recommended due to high CPU usage!")>
        Public ReadOnly Property DefaultVideoConvertNonAVC As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"DefaultsVideo"}, False), Category("Defaults Video"), DisplayName("Embed thumbnail (video)"),
            Description("Embed thumbnail in the video as cover art. Default: true.")>
        Public ReadOnly Property DefaultVideoEmbedThumbnail As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"DefaultsVideo"}), Category("Defaults Video"), DisplayName("Include zero size formats"),
            Description("Include formats with zero size (or undefined size).")>
        Public ReadOnly Property DefaultVideoIncludeNullSize As XMLValue(Of Boolean)
        <Browsable(False), XMLV("DefaultVideoFPS", {"DefaultsVideo"}, -1)>
        Private ReadOnly Property DefaultVideoFPS_XML As XMLValue(Of Double)
        <Browsable(True), GridVisible, Category("Defaults Video"), DisplayName("Default video FPS"),
            Description("Set default video FPS (only to reduce video FPS). Default: -1 (disabled)."),
            TypeConverter(GetType(FieldsTypeConverter)), GridFormatProvider(GetType(FpsFormatProvider))>
        Public Property DefaultVideoFPS As Double
            Get
                Return DefaultVideoFPS_XML
            End Get
            Set(ByVal fps As Double)
                DefaultVideoFPS_XML.Value = fps
            End Set
        End Property
        Private Function ShouldSerializeDefaultVideoFPS() As Boolean
            Return DefaultVideoFPS <> DefaultVideoFPS_XML.Value
        End Function
        Private Sub ResetDefaultVideoFPS()
            DefaultVideoFPS = -1
        End Sub
        Friend Class FpsFormatProvider : Implements IGridConversionProvider
            Private Property Converter As TypeConverter Implements IGridConversionProvider.Converter
            Private Property Context As ITypeDescriptorContext Implements IGridConversionProvider.Context
            Private Property DataType As Type Implements IGridConversionProvider.DataType
            Private Property Instance As Object Implements IGridConversionProvider.Instance
            Friend Shared ReadOnly Property MyProviderDefault As ANumbers
                Get
                    Return New ANumbers(ANumbers.Cultures.Primitive) With {.DecimalDigits = 5, .TrimDecimalDigits = True}
                End Get
            End Property
            Friend Const ErrorMessageDefault As String = "The fps value must be a number"
            Private ReadOnly MyProvider As ANumbers = MyProviderDefault
            Friend Function ToObject(ByVal Context As ITypeDescriptorContext, ByVal Culture As CultureInfo, ByVal Value As Object) As Object Implements IGridConversionProvider.ToObject
                Return AConvert(Of Double)(Value, MyProvider, -1)
            End Function
            Friend Overloads Function ToString(ByVal Context As ITypeDescriptorContext, ByVal Culture As CultureInfo, ByVal Value As Object,
                                               ByVal DestinationType As Type) As Object Implements IGridConversionProvider.ToString
                If ACheck(Of Double)(Value, AModes.Var, MyProvider) Then
                    Return Value.ToString
                Else
                    Return -1
                End If
            End Function
            Friend Function CreateInstance(ByVal Context As ITypeDescriptorContext, ByVal NewValue As Object, ByRef RefreshGrid As Boolean) As Object Implements IGridConversionProvider.CreateInstance
                If ACheck(Of Double)(NewValue, AModes.Var, MyProvider) Then
                    Return NewValue
                Else
                    RefreshGrid = True
                    Return -1
                End If
            End Function
            Friend Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                    Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object Implements ICustomProvider.Convert
                Return AConvert(Value, AModes.Var, DestinationType,, True, -1, MyProvider, EDP.ReturnValue)
            End Function
            Friend Function IsValid(ByVal Context As ITypeDescriptorContext, ByVal Value As Object, ByVal DestinationType As Type) As Boolean Implements IGridValidator.IsValid
                If ACheck(Of Double)(Value, AModes.Var, MyProvider) Then
                    Return True
                Else
                    Throw New FormatException(ErrorMessageDefault)
                End If
            End Function
            Private Function GetFormat(ByVal FormatType As Type) As Object Implements IFormatProvider.GetFormat
                Throw New NotImplementedException("'GetFormat' is not available in 'FpsFormatProvider'")
            End Function
        End Class
        <Browsable(True), GridVisible, XMLVN({"DefaultsVideo"}, 30), Category("Defaults Video"), DisplayName("Highlight FPS (higher)"),
            Description("Highlight frame rates higher than this value." & vbCr & "Default: 30" & vbCr & "-1 to disable")>
        Public ReadOnly Property DefaultVideoHighlightFPS_H As XMLValue(Of Integer)
        <Browsable(True), GridVisible, XMLVN({"DefaultsVideo"}, -1), Category("Defaults Video"), DisplayName("Highlight FPS (lower)"),
            Description("Highlight frame rates lower than this value." & vbCr & "Default: -1" & vbCr & "-1 to disable")>
        Public ReadOnly Property DefaultVideoHighlightFPS_L As XMLValue(Of Integer)
        <Browsable(True), GridVisible, XMLVN({"DefaultsVideo"}), Category("Defaults Video"), DisplayName("Add extracted MP3 to playlist"),
            Description("If you also extract MP3 when download the video, add the extracted MP3 to the playlist. Default: false.")>
        Public ReadOnly Property VideoPlaylist_AddExtractedMP3 As XMLValue(Of Boolean)
#End Region
#Region "Defaults Audio"
        <Browsable(True), GridVisible, XMLVN({"DefaultsAudio"}, "AAC"), Category("Defaults Audio"), DisplayName("Default codec"),
            TypeConverter(GetType(FieldsTypeConverter)), GridStandardValuesProvider(NameOf(AvailableAudioFormats_Impl)),
            Description("The default audio format for downloading videos.")>
        Public ReadOnly Property DefaultAudioCodec As XMLValue(Of String)
        Private Function AvailableAudioFormats_Impl() As String()
            Return AvailableAudioFormats
        End Function
        <Browsable(True), GridVisible, XMLVN({"DefaultsAudio"}, "MP3"), Category("Defaults Audio"), DisplayName("Default codec for music"),
            TypeConverter(GetType(FieldsTypeConverter)), GridStandardValuesProvider(NameOf(AvailableAudioFormats_Impl)),
            Description("The default audio format for downloading music.")>
        Public ReadOnly Property DefaultAudioCodecMusic As XMLValue(Of String)
        <Browsable(True), GridVisible, XMLVN({"DefaultsAudio"}), Category("Defaults Audio"), DisplayName("Additional codec"),
            Bindable(True), Editor(GetType(ValueCollectionEditor), GetType(UITypeEditor)),
            EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
            TypeConverter(GetType(ValueCollectionConverter)),
            Description("Additional audio format for downloading videos. This means that the audio will be extracted and saved as a separate file in these formats.")>
        Public ReadOnly Property DefaultAudioCodecAddit As XMLValuesCollection(Of String)
        <Browsable(True), GridVisible, XMLVN({"DefaultsAudio"}, True), Category("Defaults Audio"), DisplayName("Embed thumbnail"),
            Description("Embed thumbnail in the audio as cover art. Default: true.")>
        Public ReadOnly Property DefaultAudioEmbedThumbnail As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"DefaultsAudio"}, True), Category("Defaults Audio"), DisplayName("Embed thumbnail (cover)"),
            Description("Try embedding the playlist cover (if it exists) as cover art. Default: true.")>
        Public ReadOnly Property DefaultAudioEmbedThumbnail_Cover As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"DefaultsAudio"}, True), Category("Defaults Audio"), DisplayName("Embed thumbnail (extracted files)"),
            Description("Embed thumbnail in the extracted (additional file ('mp3' only)) audio as cover art. Default: true.")>
        Public ReadOnly Property DefaultAudioEmbedThumbnail_ExtractedFiles As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"DefaultsAudio"}, -1), Category("Defaults Audio"), DisplayName("Bitrate"),
            Description("Default audio bitrate if you want to change it during download. -1 to disable. Default: -1.")>
        Public ReadOnly Property DefaultAudioBitrate As XMLValue(Of Integer)
        <Browsable(True), GridVisible, XMLVN({"DefaultsAudio"}, 20), Category("Defaults Audio"), DisplayName("Bitrate: ffmpeg crf"),
            Description("This is the ffmpeg argument. Change it only if you know what you're doing. Default: 20.")>
        Public ReadOnly Property DefaultAudioBitrate_crf As XMLValue(Of Integer)
#Region "Music"
        <Browsable(True), GridVisible, XMLVN({"Playlists"}, True), Category("Music"), DisplayName("Create M3U8"),
            Description("Create M3U8 playlist for music. Default: true.")>
        Public ReadOnly Property MusicPlaylistCreate_M3U8 As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"Playlists"}), Category("Music"), DisplayName("Create M3U"),
            Description("Create M3U playlist for music. Default: false.")>
        Public ReadOnly Property MusicPlaylistCreate_M3U As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"Playlists"}), Category("Music"), DisplayName("M3U8 Append artist"),
            Description("Add artist to file name. Default: false.")>
        Public ReadOnly Property MusicPlaylistCreate_M3U8_AppendArtist As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"Playlists"}), Category("Music"), DisplayName("M3U8 Append file extension"),
            Description("Add file extension to file name. Default: false.")>
        Public ReadOnly Property MusicPlaylistCreate_M3U8_AppendExt As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"Playlists"}), Category("Music"), DisplayName("M3U8 Append file number"),
            Description("Add file number to file name. Default: false.")>
        Public ReadOnly Property MusicPlaylistCreate_M3U8_AppendNumber As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"Playlists"}, M3U8CreationMode.Relative), Category("Music"), DisplayName("Create M3U8: creation mode"),
            Description("Set the playlist creation mode: absolute links, relative links, or both. If 'Both' is selected, two playlists will be created. Default: 'Relative'.")>
        Public ReadOnly Property MusicPlaylistCreate_CreationMode As XMLValue(Of M3U8CreationMode)
#End Region
#End Region
#Region "Defaults Subtitles"
        <XMLVN({"DefaultsSubtitles"}, {"en"}, CollectionMode:=IXMLValuesCollection.Modes.String)>
        Public ReadOnly Property DefaultSubtitles As XMLValuesCollection(Of String)
        <Browsable(True), GridVisible, Category("Defaults Subtitles"), DisplayName("Default subtitles"),
            Description("The default subtitles that should be downloaded with the video. The comma is the separator.")>
        Private Property DefaultSubtitles_Impl As String
            Get
                If DefaultSubtitles.ValueTemp.Count > 0 Then
                    Return DefaultSubtitles.ValueTemp.ListToString(",")
                Else
                    Return String.Empty
                End If
            End Get
            Set(ByVal s As String)
                If s.IsEmptyString Then
                    DefaultSubtitles.ValueTemp = Nothing
                Else
                    DefaultSubtitles.ValueTemp = ListAddList(Nothing, s.Split(","), LAP.NotContainsOnly,
                                                             CType(Function(Input$) Input.StringTrim, Func(Of Object, Object)))
                End If
            End Set
        End Property
        Private Function ShouldSerializeDefaultSubtitles_Impl() As Boolean
            Return DirectCast(DefaultSubtitles, IGridValue).ShouldSerializeValue
        End Function
        Private Sub ResetDefaultSubtitles_Impl()
            DirectCast(DefaultSubtitles, IGridValue).ResetValue()
        End Sub
        <Browsable(True), GridVisible, XMLVN({"DefaultsSubtitles"}, "SRT"), Category("Defaults Subtitles"), DisplayName("Default format"),
            TypeConverter(GetType(FieldsTypeConverter)), GridStandardValuesProvider(NameOf(AvailableSubtitlesFormats_Impl)),
            Description("The default format for downloading subtitles.")>
        Public ReadOnly Property DefaultSubtitlesFormat As XMLValue(Of String)
        Private Function AvailableSubtitlesFormats_Impl() As String()
            Return AvailableSubtitlesFormats
        End Function
        <Browsable(True), GridVisible, XMLVN({"DefaultsSubtitles"}, CollectionMode:=IXMLValuesCollection.Modes.String),
            Category("Defaults Subtitles"), DisplayName("Additional format"),
            Bindable(True), Editor(GetType(ValueCollectionEditor), GetType(UITypeEditor)),
            EditorBrowsable(EditorBrowsableState.Never), DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
            TypeConverter(GetType(ValueCollectionConverter)),
            Description("Additional format for downloading subtitles. This means that all subtitles will be converted to the formats you choose and saved as separate files.")>
        Public ReadOnly Property DefaultSubtitlesFormatAddit As XMLValuesCollection(Of String)
#End Region
#End Region
#Region "Initializer"
        Public Sub New()
            Me.New(String.Empty)
        End Sub
        Public Sub New(ByVal AccountName As String)
            Me.AccountName = AccountName
            DownloadLocations = New DownloadLocationsCollection
            PlaylistsLocations = New DownloadLocationsCollection
            DownloadLocations.Load(False, True)
            PlaylistsLocations.Load(True, True, $"{XmlFile.SettingsFolder}\DownloadLocations_Playlists.xml")
            Dim acc$ = String.Empty
            If Not AccountName.IsEmptyString Then acc = $"_{AccountName}"
            Dim f As SFile = YouTubeSettingsFile
            f.Name &= acc
            XML = New XmlFile(f,, False) With {.AutoUpdateFile = True}
            XML.LoadData(EDP.None)
            DesignXml = New XmlFile("Settings\DesignDownloader.xml", Protector.Modes.All, False)
            DesignXml.LoadData(EDP.None)
            InitializeXMLValueProperties(Me)
            AddHandler ShowNotificationsEveryDownload.TempValueChanged, AddressOf ShowNotificationsEveryDownload_TempValueChanged
            Cookies = New CookieKeeper
            Grid.Abstract.DesignerXmlSource.Add(New Grid.Abstract.DesignerXmlData(GetType(CookieListForm2), DesignXml, "CookiesListForm"))
            f = YouTubeCookieNetscapeFile
            f.Name &= acc
            If f.Exists Then Cookies.AddRange(CookieKeeper.ParseNetscapeText(f.GetText(EDP.ReturnValue), EDP.None),, EDP.None)
            If Not YTDLP.Value.Exists Then YTDLP.Value = ProgramPath("yt-dlp.exe")
            If Not FFMPEG.Value.Exists Then FFMPEG.Value = ProgramPath("ffmpeg.exe")
            If Not OutputPath.Value.Exists(SFO.Path, False) Then OutputPath.Value = YouTubeDownloadPathDefault
            If XML.ChangesDetected Then XML.UpdateData()
        End Sub
        Private Function ProgramPath(ByVal Program As String) As SFile
            If Program.CSFile.Exists Then
                Return Program.CSFile
            ElseIf $"Environment\{Program}".CSFile.Exists Then
                Return $"Environment\{Program}"
            Else
                Return SystemEnvironment.FindFileInPaths(Program).ListIfNothing.FirstOrDefault
            End If
        End Function
#End Region
#Region "Edit, Update"
        Protected Overridable Sub BeginUpdate() Implements IXMLValuesContainer.BeginUpdate, IGridValuesContainer.BeginUpdate
            XML.BeginUpdate()
        End Sub
        Protected Overridable Sub EndUpdate() Implements IXMLValuesContainer.EndUpdate, IGridValuesContainer.EndUpdate
            XML.EndUpdate()
            If XML.ChangesDetected Then XML.UpdateData()
        End Sub
        Protected Overridable Sub Apply() Implements IGridValuesContainer.Apply
            XMLValuesApply(Me)
            ApplyCookies()
        End Sub
        Protected Sub ApplyCookies()
            If Cookies.Count > 0 Then Cookies.SaveNetscapeFile(YouTubeCookieNetscapeFile) Else YouTubeCookieNetscapeFile.Delete(,, EDP.None)
        End Sub
        Private Sub BeginEdit() Implements IGridValuesContainer.BeginEdit
            XMLValuesBeginEdit(Me)
        End Sub
        Protected Overridable Sub EndEdit() Implements IGridValuesContainer.EndEdit
            XMLValuesEndEdit(Me)
            Cookies.Clear()
            If YouTubeCookieNetscapeFile.Exists Then Cookies.AddRange(CookieKeeper.ParseNetscapeText(YouTubeCookieNetscapeFile.GetText(EDP.ReturnValue), EDP.None),, EDP.None)
        End Sub
        Public Sub ShowForm(ByVal AppMode As Boolean)
            Using f As New SimpleGridForm(Me) With {
                .GridShowToolbar = False,
                .InitialOkValue = True,
                .ShowIcon = True,
                .Icon = My.Resources.SiteYouTube.YouTubeIcon_32,
                .Text = "YouTube Settings",
                .DesignXML = DesignXml,
                .DesignXMLNodeName = "YouTubeSettingsForm"
            }
                f.GridBrowsableAttributes = New AttributeCollection(New BrowsableAttribute(True), New GridVisibleAttribute(Not AppMode))
                f.ShowDialog()
            End Using
        End Sub
#End Region
#Region "Close"
        Friend Sub Close()
            DesignXml.Dispose()
            XML.Dispose()
            Cookies.Dispose()
        End Sub
#End Region
#Region "Grid Support"
        Private Class ValueCollectionConverter : Inherits TypeConverter
            Public Overrides Function ConvertTo(ByVal Context As ITypeDescriptorContext, ByVal Culture As CultureInfo, ByVal Value As Object, ByVal DestinationType As Type) As Object
                If TypeOf Value Is IEnumerable Then
                    Return DirectCast(Value, IEnumerable).ToObjectsList(Of String).ListToString
                Else
                    Return String.Empty
                End If
            End Function
        End Class
        Private Class ValueCollectionEditor : Inherits GridStructureCollectionEditor
            Public Overrides Function EditValue(ByVal Context As ITypeDescriptorContext, ByVal Provider As IServiceProvider, ByVal Value As Object) As Object
                Dim eObj As IEnumerable(Of String) = Nothing
                Select Case Context.PropertyDescriptor.Name
                    Case NameOf(DefaultSubtitlesFormatAddit) : eObj = AvailableSubtitlesFormats
                    Case NameOf(DefaultAudioCodecAddit) : eObj = AvailableAudioFormats
                End Select
                Using f As New SimpleListForm(Of String)(eObj) With {
                    .Mode = SimpleListFormModes.CheckedItems,
                    .DesignXML = MyYouTubeSettings.DesignXml,
                    .DesignXMLNodeName = "YouTubeSettingsFormList",
                    .FormText = DirectCast(Context.PropertyDescriptor.Attributes.Cast(Of Attribute).First(Function(a) a.GetType Is GetType(DisplayNameAttribute)), DisplayNameAttribute).DisplayName,
                    .Icon = My.Resources.SiteYouTube.YouTubeIcon_32
                }
                    f.DataSelected.ListAddList(Value)
                    If f.ShowDialog() = DialogResult.OK Then
                        eObj = f.DataResult.ToList
                        With DirectCast(Value, List(Of String)) : .Clear() : .ListAddList(eObj) : End With
                    End If
                End Using
                Return Value
            End Function
        End Class
#End Region
    End Class
End Namespace