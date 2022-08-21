' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.RedGifs
    Friend Module Declarations
        Friend Const RedGifsSite As String = "RedGifs"
        Friend ReadOnly DateProvider As New CustomProvider(Function(v, d, p, n, e) ADateTime.ParseUnicode(v, n, e))
    End Module
End Namespace