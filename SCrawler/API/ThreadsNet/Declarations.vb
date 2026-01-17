' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.ThreadsNet
    Friend Module Declarations
        Friend ReadOnly RegexUserID As RParams = RParams.DMS("""props"":\{[^\{\}]*?""user_id"":""(\d+)""", 1, EDP.ReturnValue)
        Friend ReadOnly RegexUserName As RParams = RParams.DMS("\<meta property=""og:title"" content=""([^\>]+)""\s*/\>", 1, TitleHtmlConverter, EDP.ReturnValue)
        Friend ReadOnly RegexUserDescr As RParams = RParams.DMS("\<meta property=""og:description"" content=""([^\>]+)""\s*/\>", 1, HtmlConverter, EDP.ReturnValue)
    End Module
End Namespace