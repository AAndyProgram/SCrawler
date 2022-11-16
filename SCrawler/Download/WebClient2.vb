' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Net
Imports PersonalUtilities.Tools.Web.Clients
Namespace DownloadObjects
    Friend Class WebClient2 : Implements IDisposable
        Protected WC As WebClient
        Protected RC As Response
        Private ReadOnly RCERROR As New ErrorsDescriber(EDP.ThrowException)
        Protected UseResponserClient As Boolean
        Friend Sub New()
        End Sub
        Friend Sub New(ByVal Responser As Response)
            If Not Responser Is Nothing Then
                RC = Responser
                UseResponserClient = True
            Else
                WC = New WebClient
            End If
        End Sub
        Friend Sub DownloadFile(ByVal URL As String, ByVal File As String)
            If UseResponserClient Then
                RC.DownloadFile(URL, File, RCERROR)
            Else
                WC.DownloadFile(URL, File)
            End If
        End Sub
#Region "IDisposable Support"
        Private disposedValue As Boolean = False
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing And Not WC Is Nothing Then WC.Dispose()
            disposedValue = True
        End Sub
        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub
        Friend Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace