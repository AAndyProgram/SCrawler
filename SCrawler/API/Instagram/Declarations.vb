' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Tools.Web.Cookies
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Clients.EventArguments
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Instagram
    Friend Module Declarations
        Friend Const InstagramSite As String = "Instagram"
        Friend Const InstagramSiteKey As String = "AndyProgram_Instagram"
        Friend ReadOnly FilesPattern As RParams = RParams.DMS(".+?([^/\?]+?\.[\w\d]{3,4})(?=(\?|\Z))", 1, EDP.ReturnValue)
        Friend ReadOnly ObtainMedia_SizeFuncPic_RegexP As RParams = RParams.DMS("_p(\d+)x(\d+)", 1, EDP.ReturnValue)
        Friend ReadOnly ObtainMedia_SizeFuncPic_RegexS As RParams = RParams.DMS("_s(\d+)x(\d+)", 1, EDP.ReturnValue)
        Friend Const PageTokenRegexPatternDefault As String = "\[\],{""token"":""(.*?)""},\d+\]"
        Friend ReadOnly Regex_UserToken_dtsg As RParams = RParams.DMS("DTSGInitialData["":,.\[\]]*?{\s*.token.:\s*""([^""]+)", 1, EDP.ReturnValue)
        Friend ReadOnly Regex_UserToken_lsd As RParams = RParams.DMS("LSD["":,.\[\]]*?{\s*.token.:\s*""([^""]+)", 1, EDP.ReturnValue)
        Friend Sub UpdateResponser(ByVal Source As IResponse, ByRef Destination As Responser, ByVal UpdateWwwClaim As Boolean)
            Const r_wwwClaimName$ = "x-ig-set-www-claim"
            Const r_tokenName$ = SiteSettings.Header_CSRF_TOKEN_COOKIE
            If Not Source Is Nothing Then
                Dim isInternal As Boolean = TypeOf Source Is WebDataResponse
                Dim wwwClaimName$, tokenName$
                If isInternal Then
                    wwwClaimName = r_wwwClaimName
                    tokenName = r_tokenName
                Else
                    wwwClaimName = SiteSettings.Header_IG_WWW_CLAIM
                    tokenName = SiteSettings.Header_CSRF_TOKEN
                End If
                Dim wwwClaim$ = String.Empty
                Dim token$ = String.Empty
                With Source
                    If isInternal Then
                        If UpdateWwwClaim And .HeadersExists Then wwwClaim = .Headers.Value(wwwClaimName)
                        If .CookiesExists Then token = If(.Cookies.FirstOrDefault(Function(c) c.Name = tokenName)?.Value, String.Empty)
                    Else
                        If .HeadersExists Then
                            If UpdateWwwClaim Then wwwClaim = .Headers.Value(wwwClaimName)
                            token = .Headers.Value(tokenName)
                        End If
                    End If
                End With

                If UpdateWwwClaim And Not wwwClaim.IsEmptyString Then Destination.Headers.Add(SiteSettings.Header_IG_WWW_CLAIM, wwwClaim)
                If Not token.IsEmptyString Then Destination.Headers.Add(SiteSettings.Header_CSRF_TOKEN, token)
                If Not isInternal Then
                    Destination.Cookies.Update(Source.Cookies, CookieKeeper.UpdateModes.ReplaceByNameAll, False, EDP.SendToLog)
                    Destination.Cookies.Update(EDP.SendToLog)
                    Destination.SaveSettings()
                End If
            End If
        End Sub
    End Module
End Namespace