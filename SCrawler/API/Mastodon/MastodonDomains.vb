' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin
Imports SCrawler.API.Base
Imports PersonalUtilities.Functions.XML
Namespace API.Mastodon
    Friend Class MastodonDomains : Inherits DomainsContainer
        Friend ReadOnly Property Credentials As List(Of Credentials)
        Friend ReadOnly Property CredentialsTemp As List(Of Credentials)
        Private ReadOnly CredentialsFile As SFile = $"{SettingsFolderName}\Responser_Mastodon_DomainsCredentials.xml"
        Friend Sub New(ByVal _Instance As ISiteSettings, ByVal DefaultValue As String)
            MyBase.New(_Instance, DefaultValue)
            Credentials = New List(Of Credentials)
            CredentialsTemp = New List(Of Credentials)
            If CredentialsFile.Exists Then
                Using x As New XmlFile(CredentialsFile,, False) With {.AllowSameNames = True, .XmlReadOnly = True}
                    x.LoadData()
                    If x.Count > 0 Then Credentials.ListAddList(x, LAP.IgnoreICopier)
                End Using
            End If
        End Sub
        Friend Overrides Function Apply() As Boolean
            If Changed Then
                Credentials.Clear()
                If CredentialsTemp.Count > 0 Then Credentials.AddRange(CredentialsTemp)
                CredentialsTemp.Clear()
            End If
            Return MyBase.Apply()
        End Function
        Friend Overrides Sub Save()
            If Credentials.Count > 0 Then
                Using x As New XmlFile With {.AllowSameNames = True}
                    x.AddRange(Credentials)
                    x.Name = "DomainsCredentials"
                    x.Save(CredentialsFile)
                End Using
            Else
                CredentialsFile.Delete(,, EDP.None)
            End If
            MyBase.Save()
        End Sub
        Friend Overrides Sub Reset()
            CredentialsTemp.Clear()
            MyBase.Reset()
        End Sub
        Friend Overrides Sub OpenSettingsForm()
            Using f As New SettingsForm(Instance)
                f.ShowDialog()
                If f.DialogResult = DialogResult.OK Then
                    Changed = True
                    CredentialsTemp.Clear()
                    If f.MyCredentials.Count > 0 Then CredentialsTemp.AddRange(f.MyCredentials)
                    DomainsTemp.Clear()
                    If f.MyDomains.Count > 0 Then DomainsTemp.ListAddList(f.MyDomains, LAP.NotContainsOnly)
                End If
            End Using
        End Sub
    End Class
End Namespace