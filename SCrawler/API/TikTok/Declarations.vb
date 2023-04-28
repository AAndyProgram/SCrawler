' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.TikTok
    Friend Module Declarations
        Friend ReadOnly RegexEnvir As New RegexParseEnvir
        Friend ReadOnly CheckDateProvider As New CustomProvider(Function(v) IIf(CType(v, Date?).HasValue, CObj(CType(v, Date?).Value), Nothing))
        Friend Class RegexParseEnvir
            Private ReadOnly UrlIdRegex As RParams = RParams.DMS("http[s]?://[w\.]{0,4}tiktok.com/[^/]+?/video/(\d+)", 1, EDP.ReturnValue)
            Private ReadOnly RegexItemsArrPre As RParams = RParams.DMS("ItemList"":\{""user-post"":\{""list"":\[([^\[]+)\]", 1)
            Private ReadOnly RegexItemsArr As RParams = RParams.DM("\d+", 0, RegexReturn.List)
            Private ReadOnly VideoPattern As New RParams(String.Empty, Nothing, 1, EDP.ReturnValue)
            Private ReadOnly DatePattern As New RParams(String.Empty, Nothing, 1, EDP.ReturnValue)
            Private ReadOnly UserIdFromVideo As RParams = RParams.DMS("/\?a=(\d+)", 1, EDP.ReturnValue)
            Friend Function GetIDList(ByVal r As String) As List(Of String)
                Try
                    If Not r.IsEmptyString Then
                        Dim l As List(Of String) = Nothing
                        Dim IdArr$ = RegexReplace(r, RegexItemsArrPre)
                        If Not IdArr.IsEmptyString Then l = RegexReplace(IdArr, RegexItemsArr)
                        If l.ListExists Then l.RemoveAll(Function(id) id.IsEmptyString)
                        Return l
                    End If
                    Return Nothing
                Catch ex As Exception
                    Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "[API.TikTok.RegexParseEnvir.GetIDList]")
                End Try
            End Function
            Friend Function GetVideoData(ByVal r As String, ByVal ID As String, ByRef URL As String, ByRef [Date] As Date?) As Boolean
                Try
                    [Date] = Nothing
                    URL = String.Empty
                    If Not r.IsEmptyString Then
                        VideoPattern.Pattern = "video"":\{""id"":""" & ID & """[^\}]+?""downloadAddr"":""([^""]+)"""
                        DatePattern.Pattern = """:{""id"":""" & ID & """,""desc"":.+?""createTime"":""(\d+)"
                        Dim u$ = RegexReplace(r, VideoPattern)
                        If Not u.IsEmptyString Then URL = SymbolsConverter.Unicode.Decode(u, EDP.ReturnValue)
                        Dim d$ = RegexReplace(r, DatePattern)
                        If Not d.IsEmptyString Then [Date] = ADateTime.ParseUnix32(d)
                        Return Not URL.IsEmptyString
                    End If
                    Return False
                Catch ex As Exception
                    Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "[API.TikTok.RegexParseEnvir.GetVideoData]", False)
                End Try
            End Function
            Friend Function ExtractPostID(ByVal URL As String) As String
                If Not URL.IsEmptyString Then Return RegexReplace(URL, UrlIdRegex) Else Return String.Empty
            End Function
            Friend Function ExtractUserID(ByVal VideoUrl As String) As String
                If Not VideoUrl.IsEmptyString Then Return RegexReplace(VideoUrl, UserIdFromVideo) Else Return String.Empty
            End Function
        End Class
    End Module
End Namespace