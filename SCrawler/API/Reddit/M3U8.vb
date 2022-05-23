' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Net
Imports SCrawler.API.Reddit.M3U8_Declarations
Imports PersonalUtilities.Tools.WEB
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Reddit
    Namespace M3U8_Declarations
        Friend Module M3U8_Declarations
            Friend ReadOnly BaseUrlPattern As RParams = RParams.DM("([htps:/]{7,8}.+?/.+?)(?=/)", 0, EDP.ReturnValue)
            Friend ReadOnly PlayListRegEx_1 As RParams = RParams.DM("(#EXT-X-STREAM-INF)(.+)(RESOLUTION=)(\d+)(.+?[\r\n]{1,2})(.+?)([\r\n]{1,2})", 0,
                                                                    RegexReturn.List, EDP.SendInLog, EDP.ReturnValue)
            Friend ReadOnly PlayListRegEx_2 As RParams = RParams.DM("(?<=#EXT-X-BYTERANGE.+?[\r\n]{1,2})(.+)(?=[\r\n]{0,2})", 0,
                                                                    RegexReturn.List, EDP.SendInLog, EDP.ReturnValue)
            Friend ReadOnly DPED As New ErrorsDescriber(EDP.SendInLog + EDP.ReturnValue)
        End Module
    End Namespace
    Friend NotInheritable Class M3U8
        Private Sub New()
        End Sub
        Private Structure Resolution : Implements IRegExCreator, IComparable(Of Resolution)
            Friend File As String
            Friend Resolution As Integer
            Friend Function CreateFromArray(ByVal ParamsArray() As String) As Object Implements IRegExCreator.CreateFromArray
                If ParamsArray.ArrayExists Then
                    File = ParamsArray(0)
                    If ParamsArray.Length > 1 Then Resolution = AConvert(Of Integer)(ParamsArray(1), 0)
                End If
                Return Me
            End Function
            Friend Function CompareTo(ByVal Other As Resolution) As Integer Implements IComparable(Of Resolution).CompareTo
                Return Resolution.CompareTo(Other.Resolution) * -1
            End Function
        End Structure
        Private Shared Function GetPlaylistUrls(ByVal PlayListURL As String, ByVal BaseUrl As String) As List(Of String)
            Try
                If Not BaseUrl.IsEmptyString Then
                    Using w As New WebClient
                        Dim r$ = w.DownloadString(PlayListURL)
                        If Not r.IsEmptyString Then
                            Dim l As List(Of Resolution) = FNF.RegexFields(Of Resolution)(r, {PlayListRegEx_1}, {6, 4})
                            If l.ListExists Then
                                l.Sort()
                                Dim pls$ = $"{BaseUrl}/{l.First.File}"
                                r = w.DownloadString(pls)
                                If Not r.IsEmptyString Then
                                    Dim lp As New ListAddParams(LAP.NotContainsOnly) With {
                                        .Converter = Function(input) $"{BaseUrl}/{input}",
                                        .Error = New ErrorsDescriber(False, False, True, New List(Of String))}
                                    Return ListAddList(Of String, List(Of String))(Nothing, DirectCast(RegexReplace(r, PlayListRegEx_2), List(Of String)), lp).ListIfNothing
                                End If
                            End If
                        End If
                    End Using
                End If
                Return New List(Of String)
            Catch ex As Exception
                Return ErrorsDescriber.Execute(DPED, ex, "[M3U8.GetPlaylistUrls]", New List(Of String))
            End Try
        End Function
        Private Shared Function Save(ByVal URLs As List(Of String), ByVal f As SFile) As SFile
            Dim CachePath As SFile = Nothing
            Try
                If URLs.ListExists Then
                    Dim ConcatFile As SFile = f
                    ConcatFile.Name = "PlayListFile"
                    ConcatFile.Extension = "mp4"
                    CachePath = $"{f.PathWithSeparator}_Cache\{SFile.GetDirectories($"{f.PathWithSeparator}_Cache\",,, EDP.ReturnValue).ListIfNothing.Count + 1}\"
                    If CachePath.Exists(SFO.Path) Then
                        Dim p As New SFileNumbers(ConcatFile.Name,,, New ANumbers With {.Format = ANumbers.Formats.General})
                        ConcatFile = SFile.Indexed_IndexFile(ConcatFile,, p, EDP.ReturnValue)
                        Dim i%
                        Dim eFiles As New List(Of SFile)
                        Dim dFile As SFile = CachePath
                        dFile.Extension = New SFile(URLs(0)).Extension
                        If dFile.Extension.IsEmptyString Then dFile.Extension = "ts"
                        Using w As New WebClient
                            For i = 0 To URLs.Count - 1
                                dFile.Name = $"ConPart_{i}"
                                w.DownloadFile(URLs(i), dFile)
                                eFiles.Add(dFile)
                            Next
                        End Using
                        f = FFMPEG.ConcatenateFiles(eFiles, Settings.FfmpegFile, ConcatFile, p, DPED)
                        eFiles.Clear()
                        Return f
                    End If
                End If
                Return Nothing
            Catch ex As Exception
                Return ErrorsDescriber.Execute(DPED, ex, "[M3U8.Save]", New SFile)
            Finally
                CachePath.Delete(SFO.Path, SFODelete.None, DPED)
            End Try
        End Function
        Friend Shared Function Download(ByVal URL As String, ByVal f As SFile) As SFile
            Dim BaseUrl$ = RegexReplace(URL, BaseUrlPattern)
            Return Save(GetPlaylistUrls(URL, BaseUrl), f)
        End Function
    End Class
End Namespace