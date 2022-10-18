' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports SCrawler.API
Imports SCrawler.API.Base
Imports SCrawler.Plugin.Hosts
Imports DownOptions = SCrawler.Plugin.ISiteSettings.Download
Partial Friend Module MainMod
    Friend Structure UserInfo : Implements IComparable(Of UserInfo), IEquatable(Of UserInfo), ICloneable, IEContainerProvider
#Region "XML Names"
        Friend Const Name_Site As String = "Site"
        Friend Const Name_Plugin As String = "Plugin"
        Friend Const Name_Collection As String = "Collection"
        Friend Const Name_Merged As String = "Merged"
        Friend Const Name_IsChannel As String = "IsChannel"
        Friend Const Name_SpecialPath As String = "SpecialPath"
        Friend Const Name_UserNode As String = "User"
#End Region
        Friend Name As String
        Friend Site As String
        Friend Plugin As String
        Friend File As SFile
        Friend SpecialPath As SFile
        Friend Merged As Boolean
        Friend IncludedInCollection As Boolean
        Friend CollectionName As String
        Friend IsChannel As Boolean
        Friend [Protected] As Boolean
        Friend ReadOnly Property DownloadOption As DownOptions
            Get
                If IsChannel Then
                    Return DownOptions.Channel
                Else
                    Return DownOptions.Main
                End If
            End Get
        End Property
        Friend Sub New(ByVal _Name As String, ByVal Host As SettingsHost, Optional ByVal Collection As String = Nothing,
                       Optional ByVal _Merged As Boolean = False, Optional ByVal _SpecialPath As SFile = Nothing)
            Name = _Name
            Site = Host.Name
            Plugin = Host.Key
            IncludedInCollection = Not Collection.IsEmptyString
            CollectionName = Collection
            Merged = _Merged
            SpecialPath = _SpecialPath
            UpdateUserFile()
        End Sub
        Private Sub New(ByVal x As EContainer)
            Name = x.Value
            Site = x.Attribute(Name_Site).Value
            Plugin = x.Attribute(Name_Plugin).Value
            CollectionName = x.Attribute(Name_Collection).Value
            IncludedInCollection = Not CollectionName.IsEmptyString
            Merged = x.Attribute(Name_Merged).Value.FromXML(Of Boolean)(False)
            SpecialPath = SFile.GetPath(x.Attribute(Name_SpecialPath).Value)
            IsChannel = x.Attribute(Name_IsChannel).Value.FromXML(Of Boolean)(False)
        End Sub
        Friend Sub New(ByVal c As Reddit.Channel)
            Name = c.Name
            Site = Reddit.RedditSite
            Plugin = Reddit.RedditSiteKey
            File = c.File
            IsChannel = True
        End Sub
        Public Shared Widening Operator CType(ByVal x As EContainer) As UserInfo
            Return New UserInfo(x)
        End Operator
        Public Shared Widening Operator CType(ByVal u As UserInfo) As String
            Return u.Name
        End Operator
        Public Shared Operator =(ByVal x As UserInfo, ByVal y As UserInfo)
            Return x.Equals(y)
        End Operator
        Public Shared Operator <>(ByVal x As UserInfo, ByVal y As UserInfo)
            Return Not x.Equals(y)
        End Operator
        Public Overrides Function ToString() As String
            Return Name
        End Function
        Friend Sub UpdateUserFile()
            File = New SFile With {
                .Separator = "\",
                .Path = GetFilePathByParams(),
                .Extension = "xml",
                .Name = $"{UserDataBase.UserFileAppender}_{Site}_{Name}"
            }
        End Sub
        Private Function GetFilePathByParams() As String
            If [Protected] Then Return String.Empty
            If Not SpecialPath.IsEmptyString Then
                Return $"{SpecialPath.PathWithSeparator}{SettingsFolderName}"
            ElseIf Merged And IncludedInCollection Then
                Return $"{Settings.CollectionsPathF.PathNoSeparator}\{CollectionName}\{SettingsFolderName}"
            Else
                If IncludedInCollection Then
                    Return $"{Settings.CollectionsPathF.PathNoSeparator}\{CollectionName}\{Site}_{Name}\{SettingsFolderName}"
                ElseIf Not Settings(Plugin) Is Nothing Then
                    Return $"{Settings(Plugin).Path.PathNoSeparator}\{Name}\{SettingsFolderName}"
                Else
                    Dim s$ = Site.ToLower
                    Dim i% = Settings.Plugins.FindIndex(Function(p) p.Name.ToLower = s)
                    If i >= 0 Then Return $"{Settings.Plugins(i).Settings.Path.PathNoSeparator}\{Name}\{SettingsFolderName}" Else Return String.Empty
                End If
            End If
        End Function
        Friend Function ToEContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
            Return New EContainer(Name_UserNode, Name, {New EAttribute(Name_Site, Site),
                                                        New EAttribute(Name_Plugin, Plugin),
                                                        New EAttribute(Name_Collection, CollectionName),
                                                        New EAttribute(Name_Merged, Merged.BoolToInteger),
                                                        New EAttribute(Name_IsChannel, IsChannel.BoolToInteger),
                                                        New EAttribute(Name_SpecialPath, SpecialPath.PathWithSeparator)})
        End Function
        Friend Function CompareTo(ByVal Other As UserInfo) As Integer Implements IComparable(Of UserInfo).CompareTo
            If Site = Other.Site Then
                Return Name.CompareTo(Other.Name)
            Else
                Return Site.CompareTo(Other.Site)
            End If
        End Function
        Friend Overloads Function Equals(ByVal Other As UserInfo) As Boolean Implements IEquatable(Of UserInfo).Equals
            Return Site = Other.Site And Name = Other.Name
        End Function
        Public Overloads Overrides Function Equals(ByVal Obj As Object) As Boolean
            Return Equals(DirectCast(Obj, UserInfo))
        End Function
        Friend Function Clone() As Object Implements ICloneable.Clone
            Return New UserInfo With {
                .Name = Name,
                .Site = Site,
                .Plugin = Plugin,
                .File = File,
                .SpecialPath = SpecialPath,
                .Merged = Merged,
                .IncludedInCollection = IncludedInCollection,
                .CollectionName = CollectionName,
                .IsChannel = IsChannel,
                .[Protected] = [Protected]
            }
        End Function
    End Structure
End Module