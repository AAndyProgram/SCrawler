' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
'Imports System.Globalization
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Xhamster
    Friend Module Declarations
        Friend Const XhamsterSiteKey As String = "AndyProgram_XHamster"
        Friend ReadOnly HtmlScript As RParams = RParams.DMS("\<script id='initials-script'\>window.initials=(\{.+?\});\</script\>", 1, EDP.ReturnValue,
                                                            CType(Function(Input$) Input.StringTrim, Func(Of String, String)))
        Friend ReadOnly FirstM3U8FileRegEx As RParams = RParams.DM("RESOLUTION=\d+x(\d+).*?[\r\n]+?([^#]*?\.m3u8.*)", 0, RegexReturn.List)
        Friend ReadOnly SecondM3U8FileRegEx As RParams = RParams.DM("(#EXT-X-MAP.URI=""([^""]+((?<=\.)([^\?\.]{2,5})(?=(\?|\Z|"")))(.+|))""|#EXTINF[^\r\n]*[\r\n]+(([^\r\n]+((?<=\.)([^\?\.\r\n]{2,5})(?=(\?[^\r\n]+|[\r\n]+)))([^\r\n]+|))([\r\n]+|\Z)))", 0, RegexReturn.List)
    End Module
End Namespace