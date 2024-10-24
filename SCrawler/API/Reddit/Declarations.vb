' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Reddit
    Friend Module Declarations
        Friend Const RedditSite As String = "Reddit"
        Friend Const RedditSiteKey As String = "AndyProgram_Reddit"
        Friend ReadOnly JsonNodesJson() As NodeParams = {New NodeParams("posts", True, True, True, True, 3)}
        Friend ReadOnly ChannelJsonNodes() As NodeParams = {New NodeParams("data", True, True, True, True, 1),
                                                            New NodeParams("children", True, True, True)}
        Friend ReadOnly SingleJsonNodes() As NodeParams = {New NodeParams("data", True, True, True, True, 2),
                                                           New NodeParams("children", True, True, True),
                                                           New NodeParams("data", True, True, True, True, 1)}
        Friend ReadOnly UrlBasePattern As RParams = RParams.DM("(?<=/)([^/]+?\.[\w]{3,4})(?=(\?|\Z))", 0)
        Friend ReadOnly VideoRegEx As RParams = RParams.DM("http.{0,1}://[^" & Chr(34) & "]+?mp4", 0)
        Private ReadOnly EUR_PROVIDER As New ANumbers(ANumbers.Cultures.EUR)
        Friend ReadOnly UnixDate32ProviderReddit As New CustomProvider(Function(v, d, p, n, e) ADateTime.ParseUnix32(AConvert(Of Double)(v, EUR_PROVIDER, v), n, e))
    End Module
End Namespace