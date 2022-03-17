' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Instagram
    Friend Module Declarations
        Friend Const InstagramSite As String = "Instagram"
        Friend ReadOnly FilesPattern As RParams = RParams.DMS(".+?([^/\?]+?\.[\w\d]{3,4})(?=(\?|\Z))", 1, EDP.ReturnValue)
        Friend ReadOnly Property DateProvider As New JsonDate
        Friend Class JsonDate : Implements ICustomProvider
            Friend Function Convert(ByVal Value As Object, ByVal DestinationType As Type, ByVal Provider As IFormatProvider,
                                    Optional ByVal NothingArg As Object = Nothing, Optional ByVal e As ErrorsDescriber = Nothing) As Object Implements ICustomProvider.Convert
                Return ADateTime.ParseUnicode(Value)
            End Function
            Private Function GetFormat(ByVal FormatType As Type) As Object Implements IFormatProvider.GetFormat
                Throw New NotImplementedException("GetFormat is not available in this context")
            End Function
        End Class
    End Module
End Namespace