' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Runtime.CompilerServices
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Base
    Friend Module Declarations
        Friend Const UserLabelName As String = "User"
        Friend Const SearchRequestLabelName As String = "Search request"
        Friend ReadOnly LNC As New ListAddParams(LAP.NotContainsOnly)
        Friend ReadOnly UnixDate32Provider As New ADateTime(ADateTime.Formats.Unix32)
        Friend ReadOnly UnixDate64Provider As New ADateTime(ADateTime.Formats.Unix64)
        Friend ReadOnly HtmlConverter As Func(Of String, String) = Function(Input) SymbolsConverter.HTML.Decode(Input, EDP.ReturnValue)
        Friend ReadOnly TitleHtmlConverter As Func(Of String, String) =
               Function(Input) SymbolsConverter.HTML.Decode(SymbolsConverter.Convert(Input, EDP.ReturnValue), EDP.ReturnValue).
                               StringRemoveWinForbiddenSymbols().StringTrim()
        Friend ReadOnly Regex_VideosThumb_OG_IMAGE As RParams = RParams.DMS("meta.property=.og.image..content=""([^""]+)""", 1, EDP.ReturnValue)
        Friend Class ConcurrentDownloadsProvider : Inherits FieldsCheckerProviderBase
            Public Overrides Sub Reset()
                ErrorMessage = String.Empty
                MyBase.Reset()
            End Sub
            Public Overrides Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                              Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object
                Dim v% = AConvert(Of Integer)(Value, -1)
                Dim defV% = Settings.MaxUsersJobsCount
                If v.ValueBetween(1, defV) Then
                    Return Value
                Else
                    HasError = True
                    If ACheck(Of Integer)(Value) Then
                        ErrorMessage = $"The number of concurrent downloads must be greater than 0 and equal to or less than {defV} (global limit)."
                    Else
                        TypeError = True
                    End If
                    Return Nothing
                End If
            End Function
        End Class
        Friend Class TokenRefreshIntervalProvider : Inherits FieldsCheckerProviderBase
            Public Overrides Sub Reset()
                ErrorMessage = String.Empty
                MyBase.Reset()
            End Sub
            Public Overrides Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                              Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object
                Dim v% = AConvert(Of Integer)(Value, -1, EDP.ReturnValue)
                If v > 0 Then
                    Return Value
                ElseIf Not ACheck(Of Integer)(Value) Then
                    TypeError = True
                Else
                    ErrorMessage = $"The value of [{Name}] field must be greater than or equal to 1"
                End If
                HasError = True
                Return Nothing
            End Function
        End Class
        Friend ReadOnly Property CacheDeletionError(ByVal RootPath As SFile) As ErrorsDescriber
            Get
                Return New ErrorsDescriber(EDP.None) With {.Action = Sub(ee, eex, msg, obj) Settings.Cache.AddPath(RootPath)}
            End Get
        End Property
        Friend Function ValidateChangeSearchOptions(ByVal User As String, ByVal NewQuery As String, ByVal CurrentQuery As String) As Boolean
            Return MsgBoxE({$"Are you sure you want to change the query for user '{User}'?{vbCr}" &
                            "It is highly recommended to add a new user with this query instead of changing current one." & vbCr &
                            $"Current query: [{CurrentQuery}]{vbCr}New query: [{NewQuery}]",
                            "Changing a query"}, vbExclamation,,, {"Process", "Cancel"}) = 0
        End Function
        <Extension> Friend Function GetCookieValue(ByVal Cookies As IEnumerable(Of System.Net.Cookie), ByVal CookieName As String) As String
            If Cookies.ListExists Then Return If(Cookies.FirstOrDefault(Function(c) c.Name.ToLower = CookieName.ToLower)?.Value, String.Empty) Else Return String.Empty
        End Function
        <Extension> Friend Function GetCookieValue(ByVal Cookies As IEnumerable(Of System.Net.Cookie), ByVal CookieName As String,
                                                   ByVal PropName As String, ByVal PropNameComp As String) As String
            Return If(PropName = PropNameComp, Cookies.GetCookieValue(CookieName), String.Empty)
        End Function
    End Module
End Namespace