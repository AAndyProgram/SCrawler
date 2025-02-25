' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin.Attributes
Namespace API.Pinterest
    Friend Class EditorExchangeOptions
        <PSetting(Caption:="Get Sub-Boards", ToolTip:="Extract the Sub-Boards from the boards and download them")>
        Friend Property ExtractSubBoards As Boolean = True
        Friend Sub New(ByVal u As UserData)
            ExtractSubBoards = u.ExtractSubBoards
        End Sub
        Friend Sub New()
        End Sub
    End Class
End Namespace