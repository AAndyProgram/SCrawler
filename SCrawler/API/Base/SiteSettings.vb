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
#Region "Instagram"
        Friend ReadOnly Property InstaHash As XMLValue(Of String)
        Friend ReadOnly Property InstaHash_SP As XMLValue(Of String)
        Friend ReadOnly Property InstaHashUpdateRequired As XMLValue(Of Boolean)
        Friend ReadOnly Property InstagramDownloadingErrorDate As XMLValue(Of Date)
        Friend Property InstagramLastApplyingValue As Integer? = Nothing
        Friend ReadOnly Property InstagramReadyForDownload As Boolean
            Get
                With InstagramDownloadingErrorDate
                    If .ValueF.Exists Then
                        Return .ValueF.Value.AddMinutes(If(InstagramLastApplyingValue, 10)) < Now
                    Else
                        Return True
                    End If
                End With
            End Get
        End Property
        Friend ReadOnly Property InstagramLastDownloadDate As XMLValue(Of Date)
        Friend ReadOnly Property InstagramLastRequestsCount As XMLValue(Of Integer)
        Private InstagramTooManyRequestsReadyForCatch As Boolean = True
        Friend Function GetInstaWaitDate() As Date
            With InstagramDownloadingErrorDate
                If .ValueF.Exists Then
                    Return .ValueF.Value.AddMinutes(If(InstagramLastApplyingValue, 10))
                Else
                    Return Now
                End If
            End With
        End Function
        Friend Sub InstagramTooManyRequests(ByVal Catched As Boolean)
            With InstagramDownloadingErrorDate
                If Catched Then
                    If Not .ValueF.Exists Then
                        .Value = Now
                        If InstagramTooManyRequestsReadyForCatch Then
                            InstagramLastApplyingValue = If(InstagramLastApplyingValue, 0) + 10
                            InstagramTooManyRequestsReadyForCatch = False
                            MyMainLOG = $"Instagram downloading error: too many requests. Try again after {If(InstagramLastApplyingValue, 10)} minutes..."
                        End If
                    End If
                Else
                    .ValueF = Nothing
                    InstagramLastApplyingValue = Nothing
                    InstagramTooManyRequestsReadyForCatch = True
                End If
            End With
        End Sub
        Friend ReadOnly Property RequestsWaitTimer As XMLValue(Of Integer)
        Friend ReadOnly Property RequestsWaitTimerTaskCount As XMLValue(Of Integer)
        Friend ReadOnly Property SleepTimerOnPostsLimit As XMLValue(Of Integer)
#End Region
        Friend ReadOnly Property Temporary As XMLValue(Of Boolean)
        Friend ReadOnly Property DownloadImages As XMLValue(Of Boolean)
        Friend ReadOnly Property DownloadVideos As XMLValue(Of Boolean)
        Friend ReadOnly Property GetUserMediaOnly As XMLValue(Of Boolean)
        Friend ReadOnly Property SavedPostsUserName As XMLValue(Of String)
        Private ReadOnly SettingsFile As SFile
        Friend Sub New(ByVal s As Sites, ByRef _XML As XmlFile, ByVal GlobalPath As SFile,
                       ByRef _Temp As XMLValue(Of Boolean), ByRef _Imgs As XMLValue(Of Boolean), ByRef _Vids As XMLValue(Of Boolean))
            Site = s
            SettingsFile = $"{SettingsFolderName}\Responser_{s}.xml"
            Responser = New WEB.Response(SettingsFile)

            If SettingsFile.Exists Then
                Responser.LoadSettings()
            Else
                Select Case Site
                    Case Sites.Twitter
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
                    Case Sites.Reddit
                        Responser.CookiesDomain = "reddit.com"
                        Responser.Decoders.Add(SymbolsConverter.Converters.Unicode)
                    Case Sites.Instagram : Responser.CookiesDomain = "instagram.com"
                    Case Sites.RedGifs : Responser.CookiesDomain = "redgifs.com"
                End Select
                Responser.SaveSettings()
            End If

            Dim n() As String = {SettingsCLS.Name_Node_Sites, Site.ToString}
            _Path = New XMLValue(Of SFile)("Path", SFile.GetPath($"{GlobalPath.PathWithSeparator}{Site}"), _XML, n, XMLValue(Of SFile).ToFilePath)
            _Path.ReplaceByValue("Path", {Site.ToString})
            _XML.Remove(Site.ToString)

            Temporary = New XMLValue(Of Boolean)
            Temporary.SetExtended("Temporary", False, _XML, n)
            Temporary.SetDefault(_Temp)

            DownloadImages = New XMLValue(Of Boolean)
            DownloadImages.SetExtended("DownloadImages", True, _XML, n)
            DownloadImages.SetDefault(_Imgs)

            DownloadVideos = New XMLValue(Of Boolean)
            DownloadVideos.SetExtended("DownloadVideos", True, _XML, n)
            DownloadVideos.SetDefault(_Vids)

            If Site = Sites.Twitter Then
                GetUserMediaOnly = New XMLValue(Of Boolean)("GetUserMediaOnly", True, _XML, n)
                GetUserMediaOnly.ReplaceByValue("TwitterDefaultGetUserMedia", n)
            Else
                GetUserMediaOnly = New XMLValue(Of Boolean)
            End If

            CreateProp(InstaHashUpdateRequired, Sites.Instagram, "InstaHashUpdateRequired", True, _XML, n)
            CreateProp(InstaHash, Sites.Instagram, "InstaHash", String.Empty, _XML, n)
            If Site = Sites.Instagram AndAlso (InstaHash.IsEmptyString Or InstaHashUpdateRequired) AndAlso Responser.Cookies.ListExists Then GatherInstaHash()
            CreateProp(InstaHash_SP, Sites.Instagram, "InstaHashSavedPosts", String.Empty, _XML, n)
            CreateProp(InstagramLastDownloadDate, Sites.Instagram, "LastDownloadDate", Now.AddDays(-1), _XML, n)
            CreateProp(InstagramLastRequestsCount, Sites.Instagram, "LastRequestsCount", 0, _XML, n)
            CreateProp(RequestsWaitTimer, Sites.Instagram, "RequestsWaitTimer", 1000, _XML, n)
            CreateProp(RequestsWaitTimerTaskCount, Sites.Instagram, "RequestsWaitTimerTaskCount", 1, _XML, n)
            CreateProp(SleepTimerOnPostsLimit, Sites.Instagram, "SleepTimerOnPostsLimit", 60000, _XML, n)
            If Site = Sites.Instagram Then
                InstagramDownloadingErrorDate = New XMLValue(Of Date) With {.ToStringFunction = Function(ss, vv) AConvert(Of String)(vv, Nothing)}
                InstagramDownloadingErrorDate.SetExtended("InstagramDownloadingErrorDate", Now.AddYears(-10), _XML, n)
            Else
                InstagramDownloadingErrorDate = New XMLValue(Of Date)
            End If

            SavedPostsUserName = New XMLValue(Of String)("SavedPostsUserName", String.Empty, _XML, n)
        End Sub
        Private Sub CreateProp(Of T)(ByRef p As XMLValue(Of T), ByVal s As Sites,
                                     ByVal p_Name As String, ByVal p_Value As T, ByRef x As XmlFile, ByVal n() As String)
            If Site = s Then
                p = New XMLValue(Of T)(p_Name, p_Value, x, n)
            Else
                p = New XMLValue(Of T)
            End If
        End Sub
        Friend Sub Update()
            Responser.SaveSettings()
        End Sub
        Friend Function GatherInstaHash() As Boolean
            Try
                Dim rs As New RegexStructure("=" & Chr(34) & "([^" & Chr(34) & "]+?ConsumerLibCommons[^" & Chr(34) & "]+?.js)" & Chr(34), 1) With {
                    .UseTimeOut = True,
                    .MatchTimeOutSeconds = 10
                }
                Dim r$ = Responser.GetResponse("https://instagram.com",, EDP.ThrowException)
                If Not r.IsEmptyString Then
                    Dim hStr$ = RegexReplace(r, rs)
                    If Not hStr.IsEmptyString Then
                        Do While Left(hStr, 1) = "/" : hStr = Right(hStr, hStr.Length - 1) : Loop
                        hStr = $"https://instagram.com/{hStr}"
                        r = Responser.GetResponse(hStr,, EDP.ThrowException)
                        If Not r.IsEmptyString Then
                            rs = New RegexStructure("generatePaginationActionCreators.+?.profilePosts.byUserId.get.+?queryId:.([\d\w\S]+?)" & Chr(34), 1) With {
                                .UseTimeOut = True,
                                .MatchTimeOutSeconds = 10
                            }
                            Dim h$ = RegexReplace(r, rs)
                            If Not h.IsEmptyString Then
                                InstaHash.Value = h
                                InstaHashUpdateRequired.Value = False
                                Return True
                            End If
                        End If
                    End If
                End If
                Return False
            Catch ex As Exception
                InstaHashUpdateRequired.Value = True
                InstaHash.Value = String.Empty
                Return ErrorsDescriber.Execute(EDP.SendInLog + EDP.ReturnValue, ex, "[SiteSettings.GaterInstaHash]", False)
            End Try
        End Function
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