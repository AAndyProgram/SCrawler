' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.Base.GDL
    Friend Class GDLBatch : Inherits TokenBatch
        Friend Const UrlLibStart As String = "[urllib3.connectionpool][debug]"
        Friend Const UrlTextStart As String = UrlLibStart & " https"
        Friend Sub New(ByVal _Token As Threading.CancellationToken)
            MyBase.New(_Token)
            MainProcessName = "gallery-dl"
            ChangeDirectory(Settings.GalleryDLFile.File)
        End Sub
        Protected Overrides Async Sub OutputDataReceiver(ByVal Sender As Object, ByVal e As DataReceivedEventArgs)
            If Not ProcessKilled Then
                MyBase.OutputDataReceiver(Sender, e)
                Await Validate(e.Data)
            End If
        End Sub
    End Class
End Namespace