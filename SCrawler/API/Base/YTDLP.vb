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
        Friend Sub New(ByVal _Token As Threading.CancellationToken)
            MyBase.New(_Token)
            Commands.Clear()
            MainProcessName = Settings.YtdlpFile.File.Name '"yt-dlp"
            ChangeDirectory(Settings.YtdlpFile.File)
        End Sub
    End Class
End Namespace