' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports SCrawler.Plugin
Imports SCrawler.Plugin.Attributes
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Clients
Imports DN = SCrawler.API.Base.DeclaredNames
Namespace API.Twitter
    <Manifest(TwitterSiteKey), SavedPosts, SeparatedTasks, SpecialForm(False)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Declarations"
#Region "Other properties"
        <PropertyOption(ControlText:="Use the appropriate model",
                        ControlToolTip:="Use the appropriate model for new users." & vbCr &
                        "If disabled, all download models will be used for the first download. " &
                        "Next, the appropriate download model will be automatically selected." & vbCr &
                        "Otherwise the appropriate download model will be selected right from the start."), PXML, PClonable>
        Friend ReadOnly Property UseAppropriateModel As PropertyValue
#Region "End points"
        <PropertyOption(ControlText:="New endpoint: search", ControlToolTip:="Use new endpoint argument (-o search-endpoint=graphql) for the search model."), PXML, PClonable>
        Friend Property UseNewEndPointSearch As PropertyValue
        <PropertyOption(ControlText:="New endpoint: profiles", ControlToolTip:="Use new endpoint argument (-o search-endpoint=graphql) for the profile models."), PXML, PClonable>
        Friend Property UseNewEndPointProfiles As PropertyValue
#End Region
#Region "Limits"
        <PropertyOption(ControlText:="Abort on limit", ControlToolTip:="Abort twitter downloading when limit is reached"), PXML, PClonable>
        Friend Property AbortOnLimit As PropertyValue
        <PropertyOption(ControlText:="Download already parsed", ControlToolTip:="Download already parsed content on abort"), PXML, PClonable>
        Friend Property DownloadAlreadyParsed As PropertyValue
#End Region
        <PropertyOption(ControlText:="Media Model: allow non-user tweets", ControlToolTip:="Allow downloading non-user tweets in the media-model."), PXML, PClonable>
        Friend ReadOnly Property MediaModelAllowNonUserTweets As PropertyValue
        <PropertyOption(ControlText:=DN.GifsDownloadCaption), PXML, PClonable>
        Friend ReadOnly Property GifsDownload As PropertyValue
        <PropertyOption(ControlText:=DN.GifsSpecialFolderCaption, ControlToolTip:=DN.GifsSpecialFolderToolTip), PXML, PClonable>
        Friend ReadOnly Property GifsSpecialFolder As PropertyValue
        <PropertyOption(ControlText:=DN.GifsPrefixCaption, ControlToolTip:=DN.GifsPrefixToolTip), PXML, PClonable>
        Friend ReadOnly Property GifsPrefix As PropertyValue
        <Provider(NameOf(GifsSpecialFolder), Interaction:=True), Provider(NameOf(GifsPrefix), Interaction:=True)>
        Private ReadOnly Property GifStringChecker As IFormatProvider
        Friend Class GifStringProvider : Implements ICustomProvider, IPropertyProvider
            Friend Property PropertyName As String Implements IPropertyProvider.PropertyName
            Private Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                     Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object Implements ICustomProvider.Convert
                Dim v$ = AConvert(Of String)(Value, String.Empty)
                If Not v.IsEmptyString Then
                    If PropertyName = NameOf(GifsPrefix) Then
                        v = v.StringRemoveWinForbiddenSymbols
                    Else
                        v = v.StringReplaceSymbols(GetWinForbiddenSymbols.ToList.ListWithRemove("\").ToArray, String.Empty, EDP.ReturnValue)
                    End If
                End If
                Return v
            End Function
            Private Function GetFormat(ByVal FormatType As Type) As Object Implements IFormatProvider.GetFormat
                Throw New NotImplementedException("[GetFormat] is not available in the context of [GifStringProvider]")
            End Function
        End Class
        <PropertyOption(ControlText:=DN.UseMD5ComparisonCaption, ControlToolTip:=DN.UseMD5ComparisonToolTip), PXML, PClonable>
        Friend ReadOnly Property UseMD5Comparison As PropertyValue
        <PropertyOption(ControlText:=DN.ConcurrentDownloadsCaption,
                        ControlToolTip:=DN.ConcurrentDownloadsToolTip, AllowNull:=False, LeftOffset:=120), PXML, TaskCounter, PClonable>
        Friend ReadOnly Property ConcurrentDownloads As PropertyValue
        <Provider(NameOf(ConcurrentDownloads), FieldsChecker:=True)>
        Private ReadOnly Property MyConcurrentDownloadsProvider As IFormatProvider
#End Region
#End Region
        Friend Sub New(ByVal AccName As String, ByVal Temp As Boolean)
            MyBase.New(TwitterSite, "twitter.com", AccName, Temp, My.Resources.SiteResources.TwitterIcon_32, My.Resources.SiteResources.TwitterIcon_32.ToBitmap)

            LimitSkippedUsers = New List(Of UserDataBase)

            With Responser
                .Cookies.ChangedAllowInternalDrop = False
                .Cookies.Changed = False
            End With

            UseAppropriateModel = New PropertyValue(True)
            UseNewEndPointSearch = New PropertyValue(True)
            UseNewEndPointProfiles = New PropertyValue(True)
            AbortOnLimit = New PropertyValue(True)
            DownloadAlreadyParsed = New PropertyValue(True)
            MediaModelAllowNonUserTweets = New PropertyValue(False)
            GifsDownload = New PropertyValue(True)
            GifsSpecialFolder = New PropertyValue(String.Empty, GetType(String))
            GifsPrefix = New PropertyValue("GIF_")
            GifStringChecker = New GifStringProvider
            UseMD5Comparison = New PropertyValue(False)
            ConcurrentDownloads = New PropertyValue(1)
            MyConcurrentDownloadsProvider = New ConcurrentDownloadsProvider

            UserRegex = RParams.DMS("[htps:/]{7,8}.*?twitter.com/([^/]+)", 1)
            UrlPatternUser = "https://twitter.com/{0}"
            ImageVideoContains = "twitter"
            CheckNetscapeCookiesOnEndInit = True
            UseNetscapeCookies = True
        End Sub
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
        Friend Overrides Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            Return $"https://twitter.com/{User.Name}/status/{Media.Post.ID}"
        End Function
        Friend Overrides Function BaseAuthExists() As Boolean
            Return Responser.CookiesExists
        End Function
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            Return Settings.GalleryDLFile.Exists And BaseAuthExists()
        End Function
        Friend Property LIMIT_ABORT As Boolean = False
        Friend ReadOnly Property LimitSkippedUsers As List(Of UserDataBase)
        Friend Overrides Sub DownloadDone(ByVal What As ISiteSettings.Download)
            If LimitSkippedUsers.Count > 0 Then
                With LimitSkippedUsers
                    If .Count = 1 Then
                        MyMainLOG = $"{ .Item(0).ToStringForLog}: twitter limit reached. Data has not been downloaded."
                    Else
                        MyMainLOG = "The following twitter users have not been downloaded (twitter limit reached):" & vbNewLine &
                                    .ListToStringE(vbNewLine, New CustomProvider(Function(v As UserDataBase) $"{v.Name} ({v.ToStringForLog})"))
                    End If
                    .Clear()
                End With
            End If
            LIMIT_ABORT = False
            MyBase.DownloadDone(What)
        End Sub
        Friend Overrides Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean)
            If Options Is Nothing OrElse (Not TypeOf Options Is EditorExchangeOptions OrElse
                                          Not DirectCast(Options, EditorExchangeOptions).SiteKey = TwitterSiteKey) Then _
               Options = New EditorExchangeOptions(Me)
            If OpenForm Then
                Using f As New InternalSettingsForm(Options, Me, False) : f.ShowDialog() : End Using
            End If
        End Sub
        Friend Overrides Sub Update()
            If _SiteEditorFormOpened Then
                Dim tf$ = GifsSpecialFolder.Value
                If Not tf.IsEmptyString Then tf = tf.StringTrim("\") : GifsSpecialFolder.Value = tf
            End If
            MyBase.Update()
        End Sub
    End Class
End Namespace