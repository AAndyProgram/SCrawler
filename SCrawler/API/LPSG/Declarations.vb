' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.LPSG
    Friend Module Declarations
        Friend ReadOnly Property PhotoRegEx As RParams =
            RParams.DM("(?<=(https://www.lpsg.com|)/attachments/)([^/]+?[-\.]{1}(jpg|jpeg|gif|png|webm)\.?\d*)(?=/?"")", 0, RegexReturn.List,
                       CType(Function(Input$) If(Input.IsEmptyString, String.Empty, $"https://www.lpsg.com/attachments/{Input.StringTrimStart("/")}"),
                             Func(Of String, String)))
        Friend ReadOnly Property PhotoRegExExt As New RParams("img.data.src=""(/proxy[^""]+?)""", Nothing, 1, RegexReturn.List) With {
                                                              .Converter = Function(Input) $"https://www.lpsg.com/{SymbolsConverter.HTML.Decode(Input)}"}
        Friend ReadOnly Property NextPageRegex As RParams = RParams.DMS("<link rel=""next"" href=""(.+?/page-(\d+))""", 2)
        Private Const FileUrlRegexDefault As String = "([^/]+?)(jpg|jpeg|gif|png|webm)"
        Private ReadOnly InputFReplacer As New ErrorsDescriber(EDP.ReturnValue)
        Private ReadOnly InputForbidRemover As Func(Of String, String) = Function(Input) If(Input.IsEmptyString,
                                                                                            Input,
                                                                                            Input.StringRemoveWinForbiddenSymbols(, InputFReplacer)).
                                                                                            IfNullOrEmpty($"{Settings.Cache.NewFile.Name}.file")
        Private ReadOnly FileRegEx As RParams = RParams.DMS(FileUrlRegexDefault, 0, RegexReturn.ListByMatch, InputFReplacer)
#Disable Warning IDE0060
        Friend Function FileRegExF(ByVal Input As String, ByVal Index As Integer) As String
#Enable Warning
            If Not Input.IsEmptyString Then
                Dim l As List(Of String) = RegexReplace(Input, FileRegEx)
                If l.ListExists(3) Then
                    Dim ext$ = l(2)
                    Dim f$ = l(1).StringTrim("-", ".").StringRemoveWinForbiddenSymbols
                    If f.IsEmptyString Then f = Settings.Cache.NewFile.Name
                    Input = $"{f}.{ext}"
                End If
            End If
            Return Input
        End Function
        Private ReadOnly FileRegExExt As RParams = RParams.DM(FileUrlRegexDefault, 0, InputForbidRemover, InputFReplacer)
        Private ReadOnly FileRegExExt2 As RParams = RParams.DM("([^/]+?)(?=(\Z|&))", 0, InputForbidRemover, InputFReplacer)
        Friend Function FileRegExExtF(ByVal Input As String, ByVal Index As Integer) As String
            If Index = 0 Then
                Return RegexReplace(Input, FileRegExExt)
            Else
                Return RegexReplace(Input, FileRegExExt2)
            End If
        End Function
        Friend ReadOnly Property FileExistsRegEx As RParams = RParams.DMS(FileUrlRegexDefault, 2)
        Friend ReadOnly Property TempListAddParams As New ListAddParams(LAP.NotContainsOnly) With {.Comparer = New FComparer(Of UserMedia)(Function(x, y) x.URL = y.URL)}
        Friend ReadOnly Property ContentTitleRegEx As RParams = RParams.DMS("meta property=.og:title..content=""([^""]+)""", 1, InputFReplacer)
    End Module
End Namespace