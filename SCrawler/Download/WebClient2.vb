' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Net
Imports System.ComponentModel
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Clients.Base
Namespace DownloadObjects
    Friend Class WebClient2 : Implements IDisposable
        Protected Client As IResponserClient
        Private ReadOnly RCERROR As New ErrorsDescriber(EDP.ThrowException)
        Protected UseResponserClient As Boolean
        Private _DelegateEvents As Boolean = False
        Friend Property DelegateEvents As Boolean
            Get
                Return _DelegateEvents
            End Get
            Set(ByVal d As Boolean)
                Dim b As Boolean = Not _DelegateEvents = d
                _DelegateEvents = d
                If b Then
                    Try
                        If Not Client Is Nothing Then
                            If _DelegateEvents Then
                                AddHandler Client.DownloadProgressChanged, AddressOf Client_DownloadProgressChanged
                                AddHandler Client.DownloadFileCompleted, AddressOf Client_DownloadFileCompleted
                            Else
                                If UseResponserClient AndAlso TypeOf Client Is Responser Then
                                    With DirectCast(Client, Responser)
                                        If Not .ClientRWebClient Is Nothing Then .ClientRWebClient.AsyncMode = False
                                    End With
                                ElseIf TypeOf Client Is RWebClient Then
                                    DirectCast(Client, RWebClient).AsyncMode = False
                                End If
                                RemoveHandler Client.DownloadProgressChanged, AddressOf Client_DownloadProgressChanged
                                RemoveHandler Client.DownloadFileCompleted, AddressOf Client_DownloadFileCompleted
                            End If
                        End If
                    Catch
                    End Try
                End If
            End Set
        End Property
        Protected Sub New()
        End Sub
        Friend Sub New(ByVal Responser As Responser)
            If Not Responser Is Nothing Then
                UseResponserClient = True
                Client = Responser
            Else
                UseResponserClient = False
                Client = New RWebClient With {.UseNativeClient = True}
            End If
        End Sub
        Friend Overloads Sub DownloadFile(ByVal URL As String, ByVal File As SFile)
            Client.DownloadFile(URL, File, RCERROR)
        End Sub
        Friend Overloads Sub DownloadFile(ByVal URL As String, ByVal File As SFile, ByVal Token As Threading.CancellationToken)
            Client.DownloadFile(URL, File, Token, RCERROR)
        End Sub
        Protected Overridable Sub Client_DownloadProgressChanged(ByVal Sender As Object, ByVal e As DownloadProgressChangedEventArgs)
        End Sub
        Protected Overridable Sub Client_DownloadFileCompleted(ByVal Sender As Object, ByVal e As AsyncCompletedEventArgs)
        End Sub
#Region "IDisposable Support"
        Private disposedValue As Boolean = False
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    DelegateEvents = False
                    If Not UseResponserClient And Not Client Is Nothing Then Client.Dispose()
                End If
                Client = Nothing
            End If
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