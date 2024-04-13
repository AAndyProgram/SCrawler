' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API
Imports SCrawler.API.Base
Imports SCrawler.Plugin.Hosts
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Partial Friend Module MainMod
    Friend Structure UserInfo : Implements IComparable(Of UserInfo), IEquatable(Of UserInfo), IEContainerProvider
#Region "XML Names"
        Friend Const Name_UserNode As String = "User"
        Friend Const Name_Site As String = "Site"
        Friend Const Name_Plugin As String = "Plugin"
        Friend Const Name_AccountName As String = "AccountName"
        Friend Const Name_IsSubscription As String = "IsSubscription"
        Friend Const Name_Collection As String = "Collection"
        Friend Const Name_Model_User As String = "ModelUser"
        Friend Const Name_Model_Collection As String = "ModelCollection"
        Friend Const Name_Merged As String = "Merged"
        Friend Const Name_SpecialPath As String = "SpecialPath"
        Friend Const Name_SpecialCollectionPath As String = "SpecialCollectionPath"
        Private Const Name_LastSeen As String = "LastSeen"
#End Region
#Region "Declarations"
        Friend Name As String
        Friend Site As String
        Friend Plugin As String
        Friend AccountName As String
        Friend File As SFile
        Friend IsSubscription As Boolean
        Friend SpecialPath As SFile
        Friend SpecialCollectionPath As SFile
        Friend Merged As Boolean
        Friend ReadOnly Property IncludedInCollection As Boolean
            Get
                Return Not CollectionName.IsEmptyString
            End Get
        End Property
        Friend ReadOnly Property IsVirtual As Boolean
            Get
                Return CollectionModel = UsageModel.Virtual Or UserModel = UsageModel.Virtual
            End Get
        End Property
        Friend UserModel As UsageModel
        Friend CollectionName As String
        Friend CollectionModel As UsageModel
        Friend [Protected] As Boolean
        Friend ReadOnly Property IsProtected As Boolean
            Get
                Return [Protected] Or (LastSeen.HasValue AndAlso LastSeen.Value.AddDays(30) < Now)
            End Get
        End Property
        Friend LastSeen As Date?
#End Region
#Region "Initializers"
        Friend Sub New(ByVal _Name As String, ByVal Host As SettingsHost)
            Name = _Name
            Site = Host.Name
            Plugin = Host.Key
            UpdateUserFile()
        End Sub
        Private Sub New(ByVal x As EContainer)
            Name = x.Value
            Site = x.Attribute(Name_Site).Value
            Plugin = x.Attribute(Name_Plugin).Value
            AccountName = x.Attribute(Name_AccountName).Value
            IsSubscription = x.Attribute(Name_IsSubscription).Value.FromXML(Of Boolean)(False)
            CollectionName = x.Attribute(Name_Collection).Value
            CollectionModel = x.Attribute(Name_Model_Collection).Value.FromXML(Of Integer)(UsageModel.Default)
            UserModel = x.Attribute(Name_Model_User).Value.FromXML(Of Integer)(UsageModel.Default)
            Merged = x.Attribute(Name_Merged).Value.FromXML(Of Boolean)(False)
            SpecialPath = SFile.GetPath(x.Attribute(Name_SpecialPath).Value)
            SpecialCollectionPath = SFile.GetPath(x.Attribute(Name_SpecialCollectionPath).Value)
            If Not x.Attribute(Name_LastSeen).Value.IsEmptyString Then LastSeen = AConvert(Of Date)(x.Attribute(Name_LastSeen).Value, DateTimeDefaultProvider, Nothing)
            UpdateUserFile()
        End Sub
        Friend Sub New(ByVal c As Reddit.Channel)
            Name = c.Name
            Site = Reddit.RedditSite
            Plugin = Reddit.RedditSiteKey
            AccountName = c.RedditAccount
            File = c.File
        End Sub
        Public Shared Widening Operator CType(ByVal x As EContainer) As UserInfo
            Return New UserInfo(x)
        End Operator
        Public Shared Widening Operator CType(ByVal u As UserInfo) As String
            Return u.Name
        End Operator
#End Region
#Region "Operators"
        Public Shared Operator =(ByVal x As UserInfo, ByVal y As UserInfo)
            Return x.Equals(y)
        End Operator
        Public Shared Operator <>(ByVal x As UserInfo, ByVal y As UserInfo)
            Return Not x.Equals(y)
        End Operator
#End Region
#Region "ToString"
        Public Overrides Function ToString() As String
            Return Name
        End Function
#End Region
#Region "FilePath"
        Friend Const CollectionUserPathPattern As String = "{0}\{1}_{2}\"
        Friend Sub UpdateUserFile()
            File = New SFile With {
                .Separator = "\",
                .Path = GetFilePathByParams(),
                .Extension = "xml",
                .Name = $"{UserDataBase.UserFileAppender}_{Site}_{Name}"
            }
        End Sub
        Friend Function GetCollectionRootPath() As SFile
            If IncludedInCollection And Not IsVirtual Then
                Dim ColPath$ = If(SpecialCollectionPath.IsEmptyString, Settings.CollectionsPathF, SpecialCollectionPath).PathNoSeparator
                If SpecialCollectionPath.IsEmptyString Then ColPath &= $"\{CollectionName}"
                Return ColPath.CSFileP
            Else
                Return Nothing
            End If
        End Function
        Private Function GetFilePathByParams() As String
            If [Protected] Then Return String.Empty
            If IsSubscription Then
                If Not Settings(Plugin) Is Nothing Then _
                   Return $"{Application.StartupPath.CSFilePSN}\{SettingsFolderName}\Subscriptions\{Settings(Plugin).Key}\{Name}\{SettingsFolderName}"
            Else
                Dim ColPath$ = GetCollectionRootPath().PathNoSeparator
                Dim pluginSettings As SettingsHost
                If Not SpecialPath.IsEmptyString Then
                    Return $"{SpecialPath.PathWithSeparator}{SettingsFolderName}"
                ElseIf Merged And IncludedInCollection Then
                    Return $"{ColPath}\{SettingsFolderName}"
                Else
                    If IncludedInCollection And Not IsVirtual Then
                        Return $"{String.Format(CollectionUserPathPattern, ColPath, Site, Name)}{SettingsFolderName}"
                    ElseIf Not Settings(Plugin) Is Nothing Then
                        pluginSettings = Settings(Plugin)(AccountName)
                        If Not pluginSettings Is Nothing Then Return $"{pluginSettings.Path.PathNoSeparator}\{Name}\{SettingsFolderName}"
                    Else
                        Dim s$ = Site.ToLower
                        Dim i% = Settings.Plugins.FindIndex(Function(p) p.Name.ToLower = s)
                        If i >= 0 Then
                            pluginSettings = Settings.Plugins(i).Settings(AccountName)
                            If Not pluginSettings Is Nothing Then Return $"{pluginSettings.Path.PathNoSeparator}\{Name}\{SettingsFolderName}"
                        End If
                    End If
                End If
            End If
            Return String.Empty
        End Function
#End Region
#Region "ToEContainer Support"
        Friend Function ToEContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
            Return New EContainer(Name_UserNode, Name, {New EAttribute(Name_Site, Site),
                                                        New EAttribute(Name_Plugin, Plugin),
                                                        New EAttribute(Name_AccountName, AccountName),
                                                        New EAttribute(Name_IsSubscription, IsSubscription.BoolToInteger),
                                                        New EAttribute(Name_Collection, CollectionName),
                                                        New EAttribute(Name_Model_User, CInt(UserModel)),
                                                        New EAttribute(Name_Model_Collection, CInt(CollectionModel)),
                                                        New EAttribute(Name_Merged, Merged.BoolToInteger),
                                                        New EAttribute(Name_SpecialPath, SpecialPath.PathWithSeparator),
                                                        New EAttribute(Name_SpecialCollectionPath, SpecialCollectionPath.PathWithSeparator),
                                                        New EAttribute(Name_LastSeen, AConvert(Of String)(LastSeen, DateTimeDefaultProvider, String.Empty))})
        End Function
#End Region
#Region "IComparable Support"
        Friend Function CompareTo(ByVal Other As UserInfo) As Integer Implements IComparable(Of UserInfo).CompareTo
            If Site = Other.Site Then
                Return Name.CompareTo(Other.Name)
            Else
                Return Site.CompareTo(Other.Site)
            End If
        End Function
#End Region
#Region "IEquatable Support"
        Friend Shared Function ExactEquals(ByVal x As UserInfo, ByVal y As UserInfo) As Boolean
            Return x.Name = y.Name And
                   x.Site = y.Site And
                   x.Plugin = y.Plugin And
                   x.AccountName = y.AccountName And
                   x.File = y.File And
                   x.IsSubscription = y.IsSubscription And
                   x.SpecialPath = y.SpecialPath And
                   x.SpecialCollectionPath = y.SpecialCollectionPath And
                   x.Merged = y.Merged And
                   x.UserModel = y.UserModel And
                   x.CollectionName = y.CollectionName And
                   x.CollectionModel = y.CollectionModel And
                   x.[Protected] = y.[Protected] And
                   AEquals(Of Date)(x.LastSeen, y.LastSeen)
        End Function
        Friend Overloads Function Equals(ByVal Other As UserInfo) As Boolean Implements IEquatable(Of UserInfo).Equals
            Return Site.StringToLower = Other.Site.StringToLower And Name.StringToLower = Other.Name.StringToLower And
                   IsSubscription = Other.IsSubscription And (Not Plugin = PathPlugin.PluginKey Or SpecialPath = Other.SpecialPath)
        End Function
        Public Overloads Overrides Function Equals(ByVal Obj As Object) As Boolean
            Return Equals(DirectCast(Obj, UserInfo))
        End Function
#End Region
    End Structure
End Module