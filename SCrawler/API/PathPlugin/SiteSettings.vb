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
Namespace API.PathPlugin
    <Manifest(PluginKey)>
    Friend Class SiteSettings : Inherits SiteSettingsBase
        Private ReadOnly _Icon As Icon = Nothing
        Friend Overrides ReadOnly Property Icon As Icon
            Get
                Return _Icon
            End Get
        End Property
        Friend Overrides ReadOnly Property Image As Image
            Get
                Return PersonalUtilities.My.Resources.FolderOpenPic_Orange_16
            End Get
        End Property
        Friend Sub New()
            MyBase.New(PluginName)
            _Icon = PersonalUtilities.Tools.ImageRenderer.GetIcon(PersonalUtilities.My.Resources.FolderOpenPic_Orange_16, EDP.ReturnValue)
        End Sub
        Friend Overrides Function GetInstance(ByVal What As ISiteSettings.Download) As IPluginContentProvider
            Return New UserData
        End Function
        Friend Overrides Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            Dim f As SFile = UserURL
            If Not f.IsEmptyString AndAlso f.PathNoSeparator = UserURL.StringTrimEnd("\") AndAlso (f.Location = SFOLocation.Local Or f.Location = SFOLocation.Network) Then
                Return New ExchangeOptions(Site, f)
            Else
                Return Nothing
            End If
        End Function
        Friend Overrides Function Available(ByVal What As ISiteSettings.Download, ByVal Silent As Boolean) As Boolean
            Return False
        End Function
        Friend Overrides Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions
            Return Nothing
        End Function
        Friend Overrides Function GetUserUrl(ByVal User As IPluginContentProvider) As String
            Return String.Empty
        End Function
    End Class
End Namespace