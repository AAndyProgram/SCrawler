' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Namespace API.Base
    Friend Class SiteSettings : Implements IDisposable
        Friend Const Header_Twitter_Authorization As String = "authorization"
        Friend Const Header_Twitter_Token As String = "x-csrf-token"
        Friend ReadOnly Site As Sites
        Friend ReadOnly Responser As WEB.Response
        Private ReadOnly _Path As XMLValue(Of SFile)
        Friend Property Path As SFile
            Get
                If _Path.IsEmptyString Then _Path.Value = SFile.GetPath($"{Settings.GlobalPath.Value.PathWithSeparator}{Site}")
                Return _Path.Value
            End Get
            Set(ByVal NewFile As SFile)
                _Path.Value = NewFile
            End Set
        End Property
        Private ReadOnly SettingsFile As SFile
        Friend Sub New(ByVal s As Sites, ByRef _XML As XmlFile, ByVal GlobalPath As SFile)
            Site = s
            SettingsFile = $"{SettingsFolderName}\Responser_{s}.xml"
            Responser = New WEB.Response(SettingsFile)

            If SettingsFile.Exists Then
                Responser.LoadSettings()
            Else
                If Site = Sites.Twitter Then
                    With Responser
                        .ContentType = "application/json"
                        .Accept = "*/*"
                        .CookiesDomain = "twitter.com"
                        .Decoders.Add(SymbolsConverter.Converters.Unicode)
                        With .Headers
                            .Add("sec-ch-ua", " Not;A Brand" & Chr(34) & ";v=" & Chr(34) & "99" & Chr(34) & ", " & Chr(34) &
                                 "Google Chrome" & Chr(34) & ";v=" & Chr(34) & "91" & Chr(34) & ", " & Chr(34) & "Chromium" &
                                 Chr(34) & ";v=" & Chr(34) & "91" & Chr(34))
                            .Add("sec-ch-ua-mobile", "?0")
                            .Add("sec-fetch-dest", "empty")
                            .Add("sec-fetch-mode", "cors")
                            .Add("sec-fetch-site", "same-origin")
                            .Add(Header_Twitter_Token, String.Empty)
                            .Add("x-twitter-active-user", "yes")
                            .Add("x-twitter-auth-type", "OAuth2Session")
                            .Add(Header_Twitter_Authorization, String.Empty)
                        End With
                    End With
                ElseIf Site = Sites.Reddit Then
                    Responser.CookiesDomain = "reddit.com"
                    Responser.Decoders.Add(SymbolsConverter.Converters.Unicode)
                End If
                Responser.SaveSettings()
            End If
            _Path = New XMLValue(Of SFile)("Path", SFile.GetPath($"{GlobalPath.PathWithSeparator}{Site}"), _XML, {Site.ToString}, XMLValue(Of SFile).ToFilePath)
        End Sub
        Friend Sub Update()
            Responser.SaveSettings()
        End Sub
#Region "IDisposable Support"
        Private disposedValue As Boolean = False
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then Responser.Dispose()
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