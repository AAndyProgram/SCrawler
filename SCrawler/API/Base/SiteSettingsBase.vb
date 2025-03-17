' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Reflection
Imports SCrawler.Plugin
Imports SCrawler.Plugin.Attributes
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.Web.Cookies
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Functions.RegularExpressions
Imports Download = SCrawler.Plugin.ISiteSettings.Download
Namespace API.Base
    Friend MustInherit Class SiteSettingsBase : Implements ISiteSettings, IResponserContainer
#Region "Declarations"
        <PXML> Protected ReadOnly Property SettingsVersion As PropertyValue
        Friend ReadOnly Property Site As String Implements ISiteSettings.Site
        Protected _Icon As Icon = Nothing
        Friend Overridable ReadOnly Property Icon As Icon Implements ISiteSettings.Icon
            Get
                Return _Icon
            End Get
        End Property
        Protected _Image As Image = Nothing
        Friend Overridable ReadOnly Property Image As Image Implements ISiteSettings.Image
            Get
                Return _Image
            End Get
        End Property
        Friend Property AccountName As String Implements ISiteSettings.AccountName
        Friend Property Temporary As Boolean = False Implements ISiteSettings.Temporary
        Friend Overridable Property DefaultInstance As ISiteSettings = Nothing Implements ISiteSettings.DefaultInstance
        Protected _UserAgentDefault As String = String.Empty
        Friend Overridable Property UserAgentDefault As String Implements ISiteSettings.UserAgentDefault
            Get
                Return _UserAgentDefault
            End Get
            Set(ByVal _UserAgentDefault As String)
                Me._UserAgentDefault = _UserAgentDefault
                If _AllowUserAgentUpdate And Not Responser Is Nothing And Not _UserAgentDefault.IsEmptyString Then Responser.UserAgent = _UserAgentDefault
            End Set
        End Property
        Protected _AllowUserAgentUpdate As Boolean = True
        Protected _SubscriptionsAllowed As Boolean = False
        Friend ReadOnly Property SubscriptionsAllowed As Boolean Implements ISiteSettings.SubscriptionsAllowed
            Get
                Return _SubscriptionsAllowed
            End Get
        End Property
        Private Property Logger As ILogProvider = LogConnector Implements ISiteSettings.Logger
        Friend Overridable ReadOnly Property Responser As Responser
        Private _UserOptionsExists As Boolean = False
        Private _UserOptionsType As Type = Nothing
        Protected Overridable Function UserOptionsValid(ByVal Options As Object) As Boolean
            Return True
        End Function
        Protected Overridable Sub UserOptionsSetParameters(ByRef Options As Object)
        End Sub
        Protected Property UserOptionsType As Type
            Get
                Return _UserOptionsType
            End Get
            Set(ByVal t As Type)
                _UserOptionsType = t
                _UserOptionsExists = Not t Is Nothing
            End Set
        End Property
#End Region
#Region "EnvironmentPrograms"
        Private Property CMDEncoding As String Implements ISiteSettings.CMDEncoding
        Private Property EnvironmentPrograms As IEnumerable(Of String) Implements ISiteSettings.EnvironmentPrograms
        Private Sub EnvironmentProgramsUpdated() Implements ISiteSettings.EnvironmentProgramsUpdated
        End Sub
#End Region
#Region "Responser and cookies support"
        Friend Const ResponserFilePrefix As String = "Responser_"
        Private _CookiesNetscapeFile As SFile = Nothing
        Friend ReadOnly Property CookiesNetscapeFile As SFile
            Get
                Return _CookiesNetscapeFile
            End Get
        End Property
        Protected CheckNetscapeCookiesOnEndInit As Boolean = False
        Private _UseNetscapeCookies As Boolean = False
        Protected Property UseNetscapeCookies As Boolean
            Get
                Return _UseNetscapeCookies
            End Get
            Set(ByVal use As Boolean)
                Dim b As Boolean = Not _UseNetscapeCookies = use
                _UseNetscapeCookies = use
                If Not Responser Is Nothing Then
                    Responser.Cookies.ChangedAllowInternalDrop = Not _UseNetscapeCookies
                    Responser.Cookies.Changed = False
                End If
                If b AndAlso _UseNetscapeCookies AndAlso Not CookiesNetscapeFile.Exists Then Update_SaveCookiesNetscape()
            End Set
        End Property
        Private Property IResponserContainer_Responser As Responser Implements IResponserContainer.Responser
            Get
                Return Responser
            End Get
            Set : End Set
        End Property
        Protected Sub UpdateResponserFile()
            Dim acc$ = If(AccountName.IsEmptyString OrElse AccountName = Hosts.SettingsHost.NameAccountNameDefault, String.Empty, $"_{AccountName}")
            Responser.File = $"{SettingsFolderName}\{ResponserFilePrefix}{Site}{acc}.xml"
            _CookiesNetscapeFile = Responser.File
            _CookiesNetscapeFile.Name &= "_Cookies_Netscape"
            _CookiesNetscapeFile.Extension = "txt"
        End Sub
#End Region
#Region "GetInstance"
        Friend MustOverride Function GetInstance(ByVal What As Download) As IPluginContentProvider Implements ISiteSettings.GetInstance
#End Region
#Region "Initializers"
        Friend Sub New(ByVal SiteName As String, Optional ByVal __Icon As Icon = Nothing, Optional ByVal __Image As Image = Nothing)
            Site = SiteName
            _Icon = __Icon
            _Image = __Image
            Responser = New Responser With {.DeclaredError = EDP.ThrowException}
            SettingsVersion = New PropertyValue(0)
            UpdateResponserFile()
        End Sub
        Friend Sub New(ByVal SiteName As String, ByVal CookiesDomain As String, ByVal AccName As String, ByVal Temp As Boolean,
                       Optional ByVal __Icon As Icon = Nothing, Optional ByVal __Image As Image = Nothing)
            Me.New(SiteName, __Icon, __Image)
            Temporary = Temp
            AccountName = AccName
            If Temporary Then
                With Responser
                    .File = Nothing
                    .Cookies.File = Nothing
                    .CookiesDomain = CookiesDomain
                    .CookiesEncryptKey = SettingsCLS.CookieEncryptKey
                End With
                _CookiesNetscapeFile = Nothing
            Else
                UpdateResponserFile()
                With Responser
                    .CookiesDomain = CookiesDomain
                    .CookiesEncryptKey = SettingsCLS.CookieEncryptKey
                    If .File.Exists Then .LoadSettings() Else .SaveSettings()
                End With
            End If
        End Sub
#End Region
#Region "Initialize"
        Friend Overridable Sub BeginInit() Implements ISiteSettings.BeginInit
        End Sub
        Friend Overridable Sub EndInit() Implements ISiteSettings.EndInit
            If CheckNetscapeCookiesOnEndInit Then Update_SaveCookiesNetscape(, True)
        End Sub
#End Region
#Region "Update, Edit"
        Friend Overridable Sub BeginUpdate() Implements ISiteSettings.BeginUpdate
        End Sub
        Friend Overridable Sub EndUpdate() Implements ISiteSettings.EndUpdate
        End Sub
        Protected _SiteEditorFormOpened As Boolean = False
        Friend Overridable Sub BeginEdit() Implements ISiteSettings.BeginEdit
            _SiteEditorFormOpened = True
            If UseNetscapeCookies And CookiesNetscapeFile.Exists Then
                With Responser.Cookies
                    .Clear()
                    .AddRange(CookieKeeper.ParseNetscapeText(CookiesNetscapeFile.GetText, EDP.SendToLog + EDP.ReturnValue),, EDP.None)
                End With
            End If
        End Sub
        Friend Overridable Sub EndEdit() Implements ISiteSettings.EndEdit
            If _SiteEditorFormOpened Then DomainsReset()
            _SiteEditorFormOpened = False
        End Sub
        Friend Overridable Overloads Sub Update() Implements ISiteSettings.Update
            If _SiteEditorFormOpened Then
                If UseNetscapeCookies Then Update_SaveCookiesNetscape()
                If Not Responser Is Nothing Then
                    With Responser.Headers
                        If .Count > 0 Then .ListDisposeRemove(Function(h) h.Value.IsEmptyString)
                    End With
                End If
                DomainsApply()
            End If
            If Not Responser Is Nothing Then Responser.SaveSettings()
        End Sub
        Friend Overridable Overloads Sub Update(ByVal Source As ISiteSettings) Implements ISiteSettings.Update
            AccountName = Source.AccountName
            If Not Responser Is Nothing Then Responser.Copy(DirectCast(Source, SiteSettingsBase).Responser) : UpdateResponserFile() : Responser.SaveSettings()
            Update_CloneProperties(Source)
            UpdateImpl(Source)
        End Sub
        Protected Overridable Sub Update_CloneProperties(ByVal Source As ISiteSettings)
            CLONE_PROPERTIES(Source, Me, True)
        End Sub
        Protected Overridable Sub UpdateImpl(ByVal Source As ISiteSettings)
        End Sub
        Protected Sub Update_SaveCookiesNetscape(Optional ByVal Force As Boolean = False, Optional ByVal IsInit As Boolean = False)
            If Not Responser Is Nothing Then
                With Responser
                    If .Cookies.Changed Or Force Or IsInit Then
                        If IsInit And CookiesNetscapeFile.Exists Then Exit Sub
                        If .CookiesExists Then .Cookies.SaveNetscapeFile(CookiesNetscapeFile) Else CookiesNetscapeFile.Delete()
                        .Cookies.Changed = False
                    End If
                End With
            End If
        End Sub
#Region "Specialized"
        Protected Overridable Sub DomainsApply()
        End Sub
        Protected Overridable Sub DomainsReset()
        End Sub
#End Region
#End Region
#Region "Before and After Download"
        ''' <summary>
        ''' PRE<br/>
        ''' DownloadStarted<br/>
        ''' <br/>
        ''' BEFORE<br/>
        ''' Available<br/>
        ''' <br/>
        ''' IN<br/>
        ''' ReadyToDownload<br/>
        ''' BeforeStartDownload<br/>
        ''' AfterDownload<br/>
        ''' <br/>
        ''' AFTER<br/>
        ''' DownloadDone
        ''' </summary>
        Friend Overridable Sub DownloadStarted(ByVal What As Download) Implements ISiteSettings.DownloadStarted
        End Sub
        ''' <inheritdoc cref="DownloadStarted(Download)"/>
        Friend Overridable Sub BeforeStartDownload(ByVal User As Object, ByVal What As Download) Implements ISiteSettings.BeforeStartDownload
        End Sub
        ''' <inheritdoc cref="DownloadStarted(Download)"/>
        Friend Overridable Sub AfterDownload(ByVal User As Object, ByVal What As Download) Implements ISiteSettings.AfterDownload
        End Sub
        ''' <inheritdoc cref="DownloadStarted(Download)"/>
        Friend Overridable Sub DownloadDone(ByVal What As Download) Implements ISiteSettings.DownloadDone
            AvailableText = String.Empty
        End Sub
#End Region
#Region "User info"
        Protected UrlPatternUser As String = String.Empty
        Friend Overridable Function GetUserUrl(ByVal User As IPluginContentProvider) As String Implements ISiteSettings.GetUserUrl
            If Not UrlPatternUser.IsEmptyString Then Return String.Format(UrlPatternUser, User.NameTrue.IfNullOrEmpty(User.Name))
            Return String.Empty
        End Function
        Private Function ISiteSettings_GetUserPostUrl(ByVal User As IPluginContentProvider, ByVal Media As IUserMedia) As String Implements ISiteSettings.GetUserPostUrl
            Return GetUserPostUrl(User, Media)
        End Function
        Friend Overridable Function GetUserPostUrl(ByVal User As UserDataBase, ByVal Media As UserMedia) As String
            Return Media.URL_BASE.IfNullOrEmpty(Media.URL)
        End Function
        Protected UserRegex As RParams = Nothing
        Friend Overridable Function IsMyUser(ByVal UserURL As String) As ExchangeOptions Implements ISiteSettings.IsMyUser
            Try
                If Not UserRegex Is Nothing Then
                    Dim s$ = RegexReplace(UserURL, UserRegex)
                    If Not s.IsEmptyString Then Return New ExchangeOptions(Site, s)
                End If
                Return Nothing
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, $"[API.Base.SiteSettingsBase.IsMyUser({UserURL})]", New ExchangeOptions)
            End Try
        End Function
        Protected ImageVideoContains As String = String.Empty
        Friend Overridable Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions Implements ISiteSettings.IsMyImageVideo
            If Not ImageVideoContains.IsEmptyString AndAlso URL.Contains(ImageVideoContains) Then
                Return New ExchangeOptions(Site, String.Empty) With {.Exists = True}
            Else
                Return Nothing
            End If
        End Function
        Private Function ISiteSettings_GetSingleMediaInstance(ByVal URL As String, ByVal OutputFile As String) As IDownloadableMedia Implements ISiteSettings.GetSingleMediaInstance
            Return GetSingleMediaInstance(URL, OutputFile)
        End Function
        Friend Overridable Function GetSingleMediaInstance(ByVal URL As String, ByVal OutputFile As SFile) As IDownloadableMedia
            Return New Hosts.DownloadableMediaHost(URL, OutputFile)
        End Function
#End Region
#Region "Ready, Available"
        Friend Property AvailableText As String Implements ISiteSettings.AvailableText
        ''' <returns>True</returns>
        Friend Overridable Function BaseAuthExists() As Boolean
            Return True
        End Function
        ''' <returns>Return BaseAuthExists()</returns>
        ''' <inheritdoc cref="DownloadStarted(Download)"/>
        Friend Overridable Function Available(ByVal What As Download, ByVal Silent As Boolean) As Boolean Implements ISiteSettings.Available
            Return BaseAuthExists()
        End Function
        ''' <returns>True</returns>
        ''' <inheritdoc cref="DownloadStarted(Download)"/>
        Friend Overridable Function ReadyToDownload(ByVal What As Download) As Boolean Implements ISiteSettings.ReadyToDownload
            Return True
        End Function
#End Region
        Protected Sub CLONE_PROPERTIES(ByVal Source As ISiteSettings, ByVal Destination As ISiteSettings, ByVal IsUpdate As Boolean,
                                       Optional ByVal Full As Boolean = True)
            Dim comparer As New MembersDistinctComparerExtended
            '0 = update
            '1 = clone
            '2 = any
            Dim filterUC As Func(Of MemberInfo, Byte, Boolean) = Function(ByVal m As MemberInfo, ByVal __mode As Byte) As Boolean
                                                                     If If(m.GetCustomAttribute(Of DoNotUse)?.Value, False) Then
                                                                         Return False
                                                                     Else
                                                                         With m.GetCustomAttribute(Of PClonableAttribute)
                                                                             Return Not .Self Is Nothing AndAlso (__mode = 2 OrElse If(__mode = 0, .Update, .Clone))
                                                                         End With
                                                                     End If
                                                                 End Function
            Dim filterAll As Func(Of MemberInfo, Boolean) = Function(m) filterUC.Invoke(m, 2)
            Dim filterC As Func(Of MemberInfo, Boolean) = Function(m) If(Full, filterAll.Invoke(m), filterUC.Invoke(m, 1))
            Dim filterU As Func(Of MemberInfo, Boolean) = Function(m) filterUC.Invoke(m, 0)
            Dim membersSource As IEnumerable(Of MemberInfo) = GetObjectMembers(Source, filterAll,, True, comparer)
            If membersSource.ListExists Then
                Dim membersDest As IEnumerable(Of MemberInfo) = GetObjectMembers(Destination, If(IsUpdate, filterU, filterC),, True, comparer)
                If membersDest.ListExists Then
                    Dim mSource As MemberInfo = Nothing, mDest As MemberInfo = Nothing
                    Dim destIndx%
                    Dim isPropertyValue As Boolean
                    Dim sourceValue As Object
                    For Each mSource In membersSource
                        destIndx = membersDest.ListIndexOf(mSource, comparer, EDP.ReturnValue)
                        If destIndx.ValueBetween(0, membersDest.Count - 1) Then mDest = membersDest(destIndx) Else mDest = Nothing
                        If Not mDest Is Nothing Then
                            sourceValue = mSource.GetMemberValue(Source)
                            If mDest.MemberType = MemberTypes.Property Then
                                isPropertyValue = DirectCast(mDest, PropertyInfo).PropertyType Is GetType(PropertyValue)
                            Else
                                isPropertyValue = DirectCast(mDest, FieldInfo).FieldType Is GetType(PropertyValue)
                            End If
                            If isPropertyValue Then
                                DirectCast(mDest.GetMemberValue(Destination), PropertyValue).Clone(sourceValue)
                            Else
                                mDest.SetMemberValue(Destination, sourceValue)
                            End If
                        End If
                    Next
                End If
            End If
        End Sub
        Protected Overridable Function CloneGetEmptySettingsInstance() As ISiteSettings
            Dim _max% = -1
            Dim c As ConstructorInfo = Nothing
            With Me.GetType.GetTypeInfo.DeclaredConstructors
                If .ListExists Then
                    With .Where(Function(m) If(m.GetParameters?.Count, 0).ValueBetween(0, 2))
                        If .ListExists Then
                            _max = .Max(Function(m) If(m.GetParameters?.Count, 0))
                            c = .First(Function(m) If(m.GetParameters?.Count, 0) = _max)
                        End If
                    End With
                End If
                Select Case _max
                    Case 2 : Return c.Invoke({String.Empty, True})
                    Case 1 : Return c.Invoke({String.Empty})
                    Case 0 : Return c.Invoke(Nothing)
                    Case Else : Return Activator.CreateInstance(Me.GetType)
                End Select
            End With
        End Function
        Friend Overridable Function Clone(ByVal Full As Boolean) As ISiteSettings Implements ISiteSettings.Clone
            Dim obj As ISiteSettings = CloneGetEmptySettingsInstance()
            CLONE_PROPERTIES(Me, obj, False, Full)
            Return obj
        End Function
        Friend Sub Delete() Implements ISiteSettings.Delete
            If Not Responser Is Nothing Then
                With Responser
                    If .File.Exists Then .File.Delete(SFO.File, SFODelete.DeleteToRecycleBin, EDP.None)
                    If .Cookies.File.Exists Then .Cookies.File.Delete(SFO.File, SFODelete.DeleteToRecycleBin, EDP.None)
                End With
                If _CookiesNetscapeFile.Exists Then _CookiesNetscapeFile.Delete(SFO.File, SFODelete.DeleteToRecycleBin, EDP.None)
            End If
        End Sub
        Friend Overridable Sub Reset() Implements ISiteSettings.Reset
        End Sub
        Friend Overridable Sub UserOptions(ByRef Options As Object, ByVal OpenForm As Boolean) Implements ISiteSettings.UserOptions
            If _UserOptionsExists Then
                If Options Is Nothing OrElse (Not Options.GetType Is _UserOptionsType OrElse Not UserOptionsValid(Options)) Then
                    Dim args% = 0
                    Dim constructor As ConstructorInfo = Nothing
                    With _UserOptionsType.GetTypeInfo.DeclaredConstructors
                        If .ListExists Then
                            With .Where(Function(ByVal c As ConstructorInfo) As Boolean
                                            With c.GetParameters
                                                If .ListExists Then
                                                    If .Count = 1 Then
                                                        Return .Self()(0).ParameterType Is Me.GetType
                                                    Else
                                                        Return False
                                                    End If
                                                Else
                                                    Return True
                                                End If
                                            End With
                                            Return If(c.GetParameters?.Count, 0).ValueBetween(0, 1)
                                        End Function)
                                If .ListExists Then
                                    args = .Max(Of Integer)(Function(c) If(c.GetParameters?.Count, 0))
                                    constructor = .First(Function(c) If(c.GetParameters?.Count, 0) = args)
                                End If
                            End With
                        End If
                    End With
                    If Not constructor Is Nothing Then
                        If args > 0 AndAlso constructor.GetParameters()(0).ParameterType.GetInterface(GetType(ISiteSettings).Name) Is Nothing Then _
                           Throw New Exception("Class Interface type is incompatible")
                        If args = 0 Then Options = constructor.Invoke(Nothing) Else Options = constructor.Invoke({Me})
                    End If
                    If Options Is Nothing Then Options = Activator.CreateInstance(_UserOptionsType)
                    If Not Options Is Nothing Then UserOptionsSetParameters(Options)
                End If
                If Not Options Is Nothing And OpenForm Then
                    Using f As New InternalSettingsForm(Options, Me, False) : f.ShowDialog() : End Using
                End If
            Else
                Options = Nothing
            End If
        End Sub
        Friend Overridable Sub OpenSettingsForm() Implements ISiteSettings.OpenSettingsForm
        End Sub
#Region "IDisposable Support"
        Protected disposedValue As Boolean = False
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then Responser.DisposeIfReady
                disposedValue = True
            End If
        End Sub
        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub
        Friend Overloads Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace