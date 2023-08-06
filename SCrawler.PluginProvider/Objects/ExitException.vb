' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace Plugin
    ''' <summary>Represents errors that occur during downloading to be thrown to the root downloading function.</summary>
    Public Class ExitException : Inherits Exception
        ''' <summary>Add only the message to the log, without adding a <see cref="StackTrace"/>. Default: <see langword="True"/>.</summary>
        ''' <returns><see langword="True"/> if only the message should be added to the log; otherwise the stack trace will also be added.</returns>
        Public Property SimpleLogLine As Boolean = True
        ''' <summary>Don't add a message to the log. Default: <see langword="False"/>.</summary>
        ''' <returns><see langword="True"/> if the error is exit-only and there is no need to add a message to the log; otherwise add a message to the log.</returns>
        Public Property Silent As Boolean = False
        ''' <summary>Initializes a new instance of the <see cref="ExitException"/> class.</summary>
        Public Sub New()
        End Sub
        ''' <summary>Initializes a new instance of the <see cref="ExitException"/> class with a specified error message.</summary>
        ''' <param name="Message">The message that describes the error.</param>
        Public Sub New(ByVal Message As String)
            MyBase.New(Message)
        End Sub
        ''' <summary>
        ''' Initializes a new instance of the <see cref="ExitException"/> class with a specified error message 
        ''' and a reference to the inner exception that is the cause of this exception.
        ''' </summary>
        ''' <param name="Message">The error message that explains the reason for the exception.</param>
        ''' <param name="InnerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        Public Sub New(ByVal Message As String, ByVal InnerException As Exception)
            MyBase.New(Message, InnerException)
        End Sub
    End Class
End Namespace