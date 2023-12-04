' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Base.GDL
    Friend Module Declarations
        Private Structure GDLURL : Implements IRegExCreator
            Private _URL As String
            Friend ReadOnly Property URL As String
                Get
                    Return _URL
                End Get
            End Property
            Public Shared Widening Operator CType(ByVal u As String) As GDLURL
                Return New GDLURL With {._URL = u}
            End Operator
            Public Shared Widening Operator CType(ByVal u As GDLURL) As String
                Return u.URL
            End Operator
            Private Function CreateFromArray(ByVal ParamsArray() As String) As Object Implements IRegExCreator.CreateFromArray
                If ParamsArray.ListExists(2) Then
                    Dim u$ = ParamsArray(0).StringTrim.StringTrimEnd("/"), u2$
                    If Not u.IsEmptyString Then
                        u2 = ParamsArray(1).StringTrim
                        If Not u2.IsEmptyString AndAlso u2.StartsWith("GET", StringComparison.OrdinalIgnoreCase) Then
                            u2 = u2.Remove(0, 3).StringTrim.StringTrimStart("/")
                            If Not u2.IsEmptyString Then _URL = $"{u}/{u2}"
                        End If
                    End If
                End If
                Return Me
            End Function
            Public Shared Operator =(ByVal x As GDLURL, ByVal y As GDLURL) As Boolean
                Return x.URL = y.URL
            End Operator
            Public Shared Operator <>(ByVal x As GDLURL, ByVal y As GDLURL) As Boolean
                Return Not x.URL = y.URL
            End Operator
            Public Overrides Function ToString() As String
                Return URL
            End Function
            Public Overrides Function Equals(ByVal Obj As Object) As Boolean
                Return URL = CType(Obj, String)
            End Function
        End Structure
        Private ReadOnly Property GdlUrlPattern As RParams = RParams.DM(GDLBatch.UrlLibStart.Replace("[", "\[").Replace("]", "\]") &
                                                                        "([^""]+?)""(GET [^""]+)""", 0, EDP.ReturnValue)
        Friend Function GetUrlsFromGalleryDl(ByVal Batch As BatchExecutor, ByVal Command As String) As List(Of String)
            Dim urls As New List(Of String)
            Dim u As GDLURL
            With Batch
                .Execute(Command)
                If .ErrorOutputData.Count > 0 Then
                    For Each eValue$ In .ErrorOutputData
                        u = RegexFields(Of GDLURL)(eValue, {GdlUrlPattern}, {1, 2}, EDP.ReturnValue).ListIfNothing.FirstOrDefault
                        If Not u.URL.IsEmptyString Then urls.ListAddValue(u, LNC)
                    Next
                End If
            End With
            Return urls
        End Function
    End Module
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