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
Namespace API.Twitter
    <Manifest(TwitterSiteKey), SavedPosts, SeparatedTasks, SpecialForm(False)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
#Region "Token names"
        Friend Const Header_Authorization As String = "authorization"
        Friend Const Header_Token As String = "x-csrf-token"
#End Region
#Region "Properties constants"
        Friend Const GifsSpecialFolder_Text As String = "GIFs special folder"
        Friend Const GifsSpecialFolder_ToolTip As String = "Put the GIFs in a special folder" & vbCr &
                                                           "This is a folder name, not an absolute path." & vbCr &
                                                           "This folder(s) will be created relative to the user's root folder." & vbCr &
                                                           "Examples:" & vbCr & "SomeFolderName" & vbCr & "SomeFolderName\SomeFolderName2"
        Friend Const GifsPrefix_Text As String = "GIF prefix"
        Friend Const GifsPrefix_ToolTip As String = "This prefix will be added to the beginning of the filename"
        Friend Const GifsDownload_Text As String = "Download GIFs"
        Friend Const UseMD5Comparison_Text As String = "Use MD5 comparison"
        Friend Const UseMD5Comparison_ToolTip As String = "Each image will be checked for existence using MD5"
#End Region
#Region "Declarations"
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return My.Resources.SiteResources.TwitterIcon_32
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return My.Resources.SiteResources.TwitterPic_400
            End Get
        End Property
        'TODELETE: twitter headers
        '#Region "Auth"
        '        <PropertyOption(AllowNull:=False, IsAuth:=False, ControlText:="Authorization",
        '                        ControlToolTip:="Set authorization from [authorization] response header. This field must start from [Bearer] key word")>
        '        Private ReadOnly Property Auth As PropertyValue
        '        <PropertyOption(AllowNull:=False, IsAuth:=False, ControlText:="Token", ControlToolTip:="Set token from [x-csrf-token] response header")>
        '        Private ReadOnly Property Token As PropertyValue
        '#End Region
#Region "Other properties"
        <PropertyOption(ControlText:=GifsDownload_Text), PXML>
        Friend ReadOnly Property GifsDownload As PropertyValue
        <PropertyOption(ControlText:=GifsSpecialFolder_Text, ControlToolTip:=GifsSpecialFolder_ToolTip), PXML>
        Friend ReadOnly Property GifsSpecialFolder As PropertyValue
        <PropertyOption(ControlText:=GifsPrefix_Text, ControlToolTip:=GifsPrefix_ToolTip), PXML>
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
        <PropertyOption(ControlText:=UseMD5Comparison_Text, ControlToolTip:=UseMD5Comparison_ToolTip), PXML>
        Friend ReadOnly Property UseMD5Comparison As PropertyValue
        <PXML, PropertyOption(ControlText:="Concurrent downloads", ControlToolTip:="The number of concurrent downloads.", LeftOffset:=120), TaskCounter>
        Friend ReadOnly Property ConcurrentDownloads As PropertyValue
#End Region
        'TODELETE: twitter headers
        'Private Sub ChangeResponserFields(ByVal PropName As String, ByVal Value As Object)
        '    If Not PropName.IsEmptyString Then
        '        Dim f$ = String.Empty
        '        Select Case PropName
        '            Case NameOf(Auth) : f = Header_Authorization
        '            Case NameOf(Token) : f = Header_Token
        '        End Select
        '        If Not f.IsEmptyString Then
        '            Responser.Headers.Remove(f)
        '            If Not CStr(Value).IsEmptyString Then Responser.Headers.Add(f, CStr(Value))
        '            Responser.SaveSettings()
        '        End If
        '    End If
        'End Sub
#End Region
        Friend Sub New()
            MyBase.New(TwitterSite, "twitter.com")

            'TODELETE: twitter headers
            'Dim a$ = String.Empty
            'Dim t$ = String.Empty

            With Responser
                'TODELETE: twitter headers
                'a = .Headers.Value(Header_Authorization)
                't = .Headers.Value(Header_Token)
                .Cookies.ChangedAllowInternalDrop = False
                .Cookies.Changed = False
            End With

            'TODELETE: twitter headers
            'Auth = New PropertyValue(a, GetType(String), Sub(v) ChangeResponserFields(NameOf(Auth), v))
            'Token = New PropertyValue(t, GetType(String), Sub(v) ChangeResponserFields(NameOf(Token), v))

            GifsDownload = New PropertyValue(True)
            GifsSpecialFolder = New PropertyValue(String.Empty, GetType(String))
            GifsPrefix = New PropertyValue("GIF_")
            GifStringChecker = New GifStringProvider
            UseMD5Comparison = New PropertyValue(False)
            ConcurrentDownloads = New PropertyValue(1)

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