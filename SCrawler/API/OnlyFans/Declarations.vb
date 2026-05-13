' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.OnlyFans
    Friend Module Declarations
        Friend ReadOnly DateProvider As New ADateTime("O")
        Friend ReadOnly RegExPostID As RParams = RParams.DM("(?<=onlyfans\.com/)(\d+)", 0, EDP.ReturnValue)
        Friend ReadOnly FilesSources As New List(Of Object()) From {
            {{"source", "source"}},
            {{"files", "source", "url"}},
            {{"files", "full", "url"}}
        }
        Friend Property Rules As DynamicRulesEnv
        Friend Class CookieDateProvider : Implements ICustomProvider
            Private ReadOnly DefaultProvider As IFormatProvider
            Friend Sub New()
                DefaultProvider = PersonalUtilities.Tools.Web.Cookies.CookieKeeper.DateProviderDefault
            End Sub
            Friend Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                    Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object Implements ICustomProvider.Convert
                If Not IsNothing(Value) AndAlso TypeOf Value Is String AndAlso Not CStr(Value).IsEmptyString Then
                    Dim v$ = Value
                    If v.Contains(",_") Then v = v.Substring(0, v.IndexOf(",_"))
                    Value = v
                End If
                Return AConvert(Of Date)(Value, DefaultProvider, Nothing)
            End Function
            Private Function GetFormat(ByVal FormatType As Type) As Object Implements IFormatProvider.GetFormat
                Throw New NotImplementedException("'GetFormat' is not available in the 'CookieDateProvider'")
            End Function
        End Class
    End Module
End Namespace