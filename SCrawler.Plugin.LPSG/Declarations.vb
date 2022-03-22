' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.RegularExpressions
Friend Module Declarations
    Friend ReadOnly Property PhotoRegEx As RParams = RParams.DM("(https://www.lpsg.com/attachments)(.+?)(?="")", 0, RegexReturn.List)
    Friend ReadOnly Property NextPageRegex As RParams = RParams.DMS("<link rel=""next"" href=""(.+?/page-(\d+))""", 2)
    Private Const FileUrlRegexDefault As String = "(.+[^/]+?)(jpg|jpeg|gif|png)"
    Friend ReadOnly Property FileRegEx As New RParams(FileUrlRegexDefault, Nothing, 0) With {
        .Converter = Function(ByVal Input As String) As String
                         If Not Input.IsEmptyString Then
                             Dim lv$ = Input.Split("-").LastOrDefault
                             If Not lv.IsEmptyString Then
                                 Input = Input.Replace($"-{lv}", String.Empty)
                                 Input &= $".{lv}"
                             End If
                         End If
                         Return Input
                     End Function}
    Friend ReadOnly Property FileExistsRegEx As RParams = RParams.DMS(FileUrlRegexDefault, 2)
End Module