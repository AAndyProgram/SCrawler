' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Text.RegularExpressions
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.JustForFans
    Friend Module Declarations
        Friend Const NamePhotoLarge As String = "expandable"
        Friend Const NamePhotoSmall As String = "galThumb"
        Private Const PostDateUrlPattern As String = "\<div.class=.mbsc-card-subtitle[^\>]*?location.href='([^']+)[^\>]*?\>([^\<]+?)\<"
        Friend ReadOnly RegexUser As RParams = RParams.DMS("GetStats2\(UserID\)\{\s*var Hash = '([^']+)'[;\s/]*(var Hash = '([^']+)'|)", 1)
        Friend ReadOnly RegexVideoBlock As RParams =
            RParams.DM("((\<div mbsc-card class=""mbsc-card[^\>]*?id=""([^""]+)[^\>]*?\>)\s*\<div class=""mbsc-card-header.+?\<a class=""gridAction[^\>]*?\>[^\<\>]*?\</a\>\s*\</div>)",
                       0, RegexReturn.List, RegexOptions.Singleline, RegexOptions.IgnoreCase, EDP.ReturnValue)
        Friend ReadOnly Regex_Video As RParams = RParams.DMS("<script>.\s*/\*\s*\$\(document\).ready\(function\(\) \{\s*MakeMovieVideoJS\(.*?(\{.+?\})", 1,
                                                             RegexOptions.IgnoreCase, RegexOptions.Singleline, EDP.ReturnValue)
        Friend ReadOnly Regex_Photo As RParams = RParams.DM("\<img.+?class=""(expandable|galThumb)"".*?(data-lazy|src)=""([^""]+)""", 0,
                                                            RegexReturn.List, RegexOptions.IgnoreCase, RegexOptions.Singleline, EDP.ReturnValue)
        Friend ReadOnly Regex_Gallery As RParams = RParams.DM("\<div[^\>]+?class=.imageGallery", 0, EDP.ReturnValue)
        Friend ReadOnly Regex_PostDate As RParams = RParams.DMS(PostDateUrlPattern, 2, RegexOptions.IgnoreCase, RegexOptions.Singleline, EDP.ReturnValue,
                                                                CType(Function(Input$) Input.StringTrim, Func(Of String, String)))
        Friend ReadOnly Regex_PostURL As RParams = RParams.DMS(PostDateUrlPattern, 1, RegexOptions.IgnoreCase, RegexOptions.Singleline, EDP.ReturnValue,
                                                               CType(Function(Input$) Input.StringTrim, Func(Of String, String)))
        Friend ReadOnly Regex_PostID As RParams = RParams.DMS("[&\?]{1}post=([^&\?]+)", 1, RegexOptions.IgnoreCase, EDP.ReturnValue)
        Friend ReadOnly DateProvider As New ADateTime("MMMM d, yyyy, h:mm tt")
        Friend ReadOnly DateProviderVideoFileName As New ADateTime("yyyyMMdd_HHmmss")
    End Module
End Namespace