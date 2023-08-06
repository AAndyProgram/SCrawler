' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports PersonalUtilities.Tools
Namespace API.Base
    Friend Class TokenBatch : Inherits BatchExecutor
        Protected ReadOnly Token As CancellationToken
        Friend Sub New(ByVal _Token As CancellationToken)
            MyBase.New(True)
            Token = _Token
        End Sub
        Protected Overrides Async Sub OutputDataReceiver(ByVal Sender As Object, ByVal e As DataReceivedEventArgs)
            MyBase.OutputDataReceiver(Sender, e)
            Await Task.Run(Sub() If Token.IsCancellationRequested Then Kill())
        End Sub
        Protected Overrides Async Sub ErrorDataReceiver(ByVal Sender As Object, ByVal e As DataReceivedEventArgs)
            MyBase.ErrorDataReceiver(Sender, e)
            Await Task.Run(Sub() If Token.IsCancellationRequested Then Kill())
        End Sub
    End Class
End Namespace