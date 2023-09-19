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
                Return OpenFolderInOtherProgram.Use
            End Get
            Set(ByVal use As Boolean)
                OpenFolderInOtherProgram.Use = use
            End Set
        End Property
        <Browsable(True), GridVisible(False), Category("EnvironmentFolder"), DisplayName("Open folders in another program (command)"),
            Description("The command to open a folder."), DefaultValue("")>
        Private Property IDownloaderSettings_OpenFolderInOtherProgram_Command As String Implements IDownloaderSettings.OpenFolderInOtherProgram_Command
            Get
                Return OpenFolderInOtherProgram
            End Get
            Set(ByVal command As String)
                OpenFolderInOtherProgram.Value = command
            End Set
        End Property
#End Region
#Region "Defaults"
        <Browsable(True), GridVisible, XMLVN({"Defaults"}), Category("Defaults"), DisplayName("Replace modification date"),
            Description("Set the file date to the date the video was added (website) (if available). Default: false.")>
        Public ReadOnly Property ReplaceModificationDate As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"Defaults"}), Category("Defaults"), DisplayName("Use cookies"),
            Description("By default, use cookies when downloading from YouTube.")>
        Public ReadOnly Property DefaultUseCookies As XMLValue(Of Boolean)
        <Browsable(True), GridVisible, XMLVN({"Defaults"}, Protocols.Any), Category("Defaults"), DisplayName("Protocol"),
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
        <Browsable(True), GridVisible, XMLVN({"DefaultsVideo"}), Category("Defaults Video"), DisplayName("Include zero size formats"),
            Description("Include formats with zero size (or undefined size).")>
        Public ReadOnly Property DefaultVideoIncludeNullSize As XMLValue(Of Boolean)
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
            DownloadLocations = New DownloadLocationsCollection
            DownloadLocations.Load(False, True)
            XML = New XmlFile(YouTubeSettingsFile,, False) With {.AutoUpdateFile = True}
            XML.LoadData(EDP.None)
            DesignXml = New XmlFile("Settings\DesignDownloader.xml", Protector.Modes.All, False)
            DesignXml.LoadData(EDP.None)
            InitializeXMLValueProperties(Me)
            AddHandler ShowNotificationsEveryDownload.TempValueChanged, AddressOf ShowNotificationsEveryDownload_TempValueChanged
            Cookies = New CookieKeeper
            Grid.Abstract.DesignerXmlSource.Add(New Grid.Abstract.DesignerXmlData(GetType(CookieListForm2), DesignXml, "CookiesListForm"))
            If YouTubeCookieNetscapeFile.Exists Then Cookies.AddRange(CookieKeeper.ParseNetscapeText(YouTubeCookieNetscapeFile.GetText(EDP.ReturnValue), EDP.None),, EDP.None)
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