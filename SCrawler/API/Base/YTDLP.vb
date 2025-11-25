' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.Base.YTDLP
    Friend Class YTDLPBatch : Inherits GDL.GDLBatch
        Friend Sub New(ByVal _Token As Threading.CancellationToken, Optional ByVal __MainProcessName As String = Nothing, Optional ByVal WorkingDir As SFile = Nothing)
            MyBase.New(_Token, __MainProcessName.IfNullOrEmpty(Settings.YtdlpFile.File.Name), WorkingDir.IfNullOrEmpty(Settings.YtdlpFile.File))
        End Sub
    End Class
End Namespace