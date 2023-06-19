' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Reflection
Imports SCrawler.Plugin.Attributes
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Objects
Imports PersonalUtilities.Tools.WEB.GitHub
Namespace Plugin.Hosts
    Friend Class PluginHost
        Friend Const PluginsPath As String = "Plugins\"
        Friend ReadOnly Property Settings As SettingsHost
        Friend ReadOnly Property Name As String
            Get
                Return Settings.Name
            End Get
        End Property
        Friend ReadOnly Property Key As String
            Get
                Return Settings.Key
            End Get
        End Property
        Friend ReadOnly Property Exists As Boolean
            Get
                Return Not Settings Is Nothing
            End Get
        End Property
        Private ReadOnly GitHubInfo As Github
        Private ReadOnly AssemblyVersion As Version
        Friend ReadOnly Property HasNewVersion As Boolean
            Get
                If Not GitHubInfo Is Nothing Then
                    Return NewVersionExists(AssemblyVersion, GitHubInfo.UserName, GitHubInfo.Repository)
                Else
                    Return False
                End If
            End Get
        End Property
        Friend ReadOnly Property HasError As Boolean
        Private Sub New(ByVal s As ISiteSettings, ByRef _XML As XmlFile, ByVal GlobalPath As SFile,
                        ByRef _Temp As XMLValue(Of Boolean), ByRef _Imgs As XMLValue(Of Boolean), ByRef _Vids As XMLValue(Of Boolean))
            Settings = New SettingsHost(s, _XML, GlobalPath, _Temp, _Imgs, _Vids)
        End Sub
        Private Sub New(ByVal AssemblyFile As SFile, ByRef _XML As XmlFile, ByVal GlobalPath As SFile,
                        ByRef _Temp As XMLValue(Of Boolean), ByRef _Imgs As XMLValue(Of Boolean), ByRef _Vids As XMLValue(Of Boolean))
            Try
                Dim a As Assembly = Assembly.Load(AssemblyName.GetAssemblyName(AssemblyFile))
                If Not a Is Nothing Then
                    GitHubInfo = a.GetCustomAttribute(Of Github)()
                    AssemblyVersion = New Version(FileVersionInfo.GetVersionInfo(a.Location).FileVersion)
                    Dim t() As Type = a.GetTypes
                    If t.ListExists Then
                        Dim tSettings$ = GetType(ISiteSettings).FullName
                        For Each tt As Type In t
                            If tt.IsInterface Or tt.IsAbstract Then
                                Continue For
                            Else
                                If Not tt.GetInterface(tSettings) Is Nothing Then
                                    Settings = New SettingsHost(Activator.CreateInstance(tt), _XML, GlobalPath, _Temp, _Imgs, _Vids)
                                End If
                            End If
                        Next
                    End If
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"[PluginHost.New({AssemblyFile})]")
                _HasError = True
            End Try
        End Sub
        Friend Shared Function GetMyHosts(ByRef _XML As XmlFile, ByVal GlobalPath As SFile,
                                          ByRef _Temp As XMLValue(Of Boolean), ByRef _Imgs As XMLValue(Of Boolean),
                                          ByRef _Vids As XMLValue(Of Boolean)) As IEnumerable(Of PluginHost)
            Return {New PluginHost(New API.Reddit.SiteSettings, _XML, GlobalPath, _Temp, _Imgs, _Vids),
                    New PluginHost(New API.Twitter.SiteSettings, _XML, GlobalPath, _Temp, _Imgs, _Vids),
                    New PluginHost(New API.Mastodon.SiteSettings, _XML, GlobalPath, _Temp, _Imgs, _Vids),
                    New PluginHost(New API.Instagram.SiteSettings(_XML, GlobalPath), _XML, GlobalPath, _Temp, _Imgs, _Vids),
                    New PluginHost(New API.RedGifs.SiteSettings, _XML, GlobalPath, _Temp, _Imgs, _Vids),
                    New PluginHost(New API.YouTube.SiteSettings, _XML, GlobalPath, _Temp, _Imgs, _Vids),
                    New PluginHost(New API.Pinterest.SiteSettings, _XML, GlobalPath, _Temp, _Imgs, _Vids),
                    New PluginHost(New API.TikTok.SiteSettings, _XML, GlobalPath, _Temp, _Imgs, _Vids),
                    New PluginHost(New API.LPSG.SiteSettings, _XML, GlobalPath, _Temp, _Imgs, _Vids),
                    New PluginHost(New API.PornHub.SiteSettings, _XML, GlobalPath, _Temp, _Imgs, _Vids),
                    New PluginHost(New API.Xhamster.SiteSettings, _XML, GlobalPath, _Temp, _Imgs, _Vids),
                    New PluginHost(New API.XVIDEOS.SiteSettings, _XML, GlobalPath, _Temp, _Imgs, _Vids),
                    New PluginHost(New API.ThisVid.SiteSettings, _XML, GlobalPath, _Temp, _Imgs, _Vids),
                    New PluginHost(New API.PathPlugin.SiteSettings, _XML, GlobalPath, _Temp, _Imgs, _Vids),
                    New PluginHost(New API.OnlyFans.SiteSettings, _XML, GlobalPath, _Temp, _Imgs, _Vids)}
        End Function
        Friend Shared Function GetPluginsHosts(ByRef _XML As XmlFile, ByVal GlobalPath As SFile,
                                               ByRef _Temp As XMLValue(Of Boolean), ByRef _Imgs As XMLValue(Of Boolean),
                                               ByRef _Vids As XMLValue(Of Boolean)) As IEnumerable(Of PluginHost)
            Try
                Dim pList As New List(Of PluginHost)
                Dim PluginsDir As SFile = PluginsPath
                PluginsDir.Exists(SFO.Path)
                Dim fList As List(Of SFile) = SFile.GetFiles(PluginsDir, "*.dll",, EDP.ReturnValue).ListIfNothing
                If fList.Count > 0 Then
                    For Each f As SFile In fList : pList.Add(New PluginHost(f, _XML, GlobalPath, _Temp, _Imgs, _Vids)) : Next
                    pList.RemoveAll(Function(p) Not p.Exists Or p.HasError)
                End If
                Return pList
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"[PluginHost.GetPluginsHosts({GlobalPath})]")
                Return Nothing
            End Try
        End Function
    End Class
End Namespace