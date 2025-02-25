' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports SCrawler.API.Base
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Clients
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Imports Converters = PersonalUtilities.Functions.SymbolsConverter.Converters
Namespace API.LPSG
    Friend Class UserData : Inherits UserDataBase
        Private Const Name_LatestPage As String = "LatestPage"
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
            If Loading Then
                LatestPage = Container.Value(Name_LatestPage)
            Else
                Container.Add(Name_LatestPage, LatestPage)
            End If
        End Sub
        Private Property LatestPage As String = String.Empty
        Private Enum Mode : Internal : External : End Enum
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                Responser.DeclaredError = EDP.ThrowException

                Dim NextPage$
                Dim r$
                Dim titleChecked As Boolean = False
                Dim _LPage As Func(Of String) = Function() If(LatestPage.IsEmptyString, String.Empty, $"page-{LatestPage}")

                Do
                    URL = $"https://www.lpsg.com/threads/{Name}/{_LPage.Invoke}"
                    r = Responser.GetResponse(URL)
                    UserExists = True
                    UserSuspended = False
                    ThrowAny(Token)
                    If Not r.IsEmptyString Then
                        If UserSiteName.IsEmptyString And Not titleChecked Then
                            UserSiteName = RegexReplace(r, ContentTitleRegEx)
                            If Not UserSiteName.IsEmptyString Then
                                _ForceSaveUserInfo = True
                                If FriendlyName.IsEmptyString Then FriendlyName = UserSiteName
                            End If
                        End If
                        titleChecked = True
                        NextPage = RegexReplace(r, NextPageRegex)
                        UpdateMediaList(RegexReplace(r, PhotoRegEx), Mode.Internal)
                        UpdateMediaList(RegexReplace(r, PhotoRegExExt), Mode.External)
                        If NextPage = LatestPage Or NextPage.IsEmptyString Then Exit Do Else LatestPage = NextPage
                    Else
                        Exit Do
                    End If
                Loop

                If _TempMediaList.ListExists And _ContentList.ListExists Then _
                   _TempMediaList.RemoveAll(Function(m) _ContentList.Exists(Function(mm) mm.URL = m.URL))
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]")
            End Try
        End Sub
        Private Sub UpdateMediaList(ByVal l As List(Of String), ByVal m As Mode)
            If l.ListExists Then
                Dim f As SFile
                Dim u$
                Dim exists As Boolean
                Dim r As Func(Of String, Integer, String)
                Dim indx% = 0
                Dim ude As New ErrorsDescriber(EDP.ReturnValue)
                ProgressPre.ChangeMax(l.Count)
                For Each url$ In l
                    ProgressPre.Perform()
                    If Not url.IsEmptyString Then u = SymbolsConverter.Decode(url, {Converters.HTML, Converters.ASCII}, ude) Else u = String.Empty
                    If Not u.IsEmptyString Then
                        exists = Not IsEmptyString(RegexReplace(u, FileExistsRegEx))
                        If m = Mode.Internal Then
                            r = AddressOf FileRegExF
                        Else
                            r = AddressOf FileRegExExtF
                            If Not exists Then
                                indx = 1
                                exists = Not IsEmptyString(FileRegExExtF(u, 1))
                            End If
                        End If
                        If exists Then
                            f = r.Invoke(u, indx)
                            f.Path = MyFile.CutPath.PathNoSeparator
                            f.Separator = "\"
                            If f.Extension.IsEmptyString Then f.Extension = "jpg"
                            _TempMediaList.ListAddValue(New UserMedia With {.Type = UTypes.Picture, .URL = url, .File = f}, TempListAddParams)
                        End If
                    End If
                Next
            End If
        End Sub
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            With Responser : .Mode = Responser.Modes.WebClient : .ResetStatus() : End With
            UseResponserClient = True
            DownloadContentDefault(Token)
        End Sub
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            If Responser.StatusCode = Net.HttpStatusCode.ServiceUnavailable Or
               Responser.StatusCode = Net.HttpStatusCode.Forbidden Then '503, 403
                MyMainLOG = $"{ToStringForLog()}: LPSG not available"
                Return 1
            ElseIf Responser.StatusCode = Net.HttpStatusCode.NotFound Then '404
                UserExists = False
                Return 1
            Else
                Return 0
            End If
        End Function
    End Class
End Namespace