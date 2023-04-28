' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.Base
    Friend Module Declarations
        Friend Const UserLabelName As String = "User"
        Friend ReadOnly LNC As New ListAddParams(LAP.NotContainsOnly)
        Friend ReadOnly UnixDate32Provider As New ADateTime(ADateTime.Formats.Unix32)
        Friend ReadOnly UnixDate64Provider As New ADateTime(ADateTime.Formats.Unix64)
        Friend ReadOnly HtmlConverter As Func(Of String, String) = Function(Input) SymbolsConverter.HTML.Decode(Input, EDP.ReturnValue)
        Friend ReadOnly TitleHtmlConverter As Func(Of String, String) =
               Function(Input) SymbolsConverter.HTML.Decode(SymbolsConverter.Convert(Input, EDP.ReturnValue), EDP.ReturnValue).
                               StringRemoveWinForbiddenSymbols().StringTrim()
    End Module
End Namespace