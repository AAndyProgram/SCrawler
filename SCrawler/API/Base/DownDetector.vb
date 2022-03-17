' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Net
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Base
    Friend NotInheritable Class DownDetector
        Private Shared ReadOnly Property Params As New RParams("x:.'([\S]+?)',.y:.(\d+)", -1, Nothing, RegexReturn.List)
        Private Sub New()
        End Sub
        Friend Structure Data : Implements IRegExCreator, IComparable(Of Data)
            Friend [Date] As Date
            Friend Value As Integer
            Friend Function CreateFromArray(ByVal ParamsArray() As String) As Object Implements IRegExCreator.CreateFromArray
                If ParamsArray.ListExists Then
                    Try : [Date] = Date.Parse(ParamsArray(0)) : Catch : End Try
                    If ParamsArray.Length > 1 Then Value = AConvert(Of Integer)(ParamsArray(1), 0)
                End If
                Return Me
            End Function
            Public Overrides Function ToString() As String
                Return $"{AConvert(Of String)([Date], ADateTime.Formats.BaseDateTime, String.Empty)} [{Value}]"
            End Function
            Friend Function CompareTo(ByVal Other As Data) As Integer Implements IComparable(Of Data).CompareTo
                Return [Date].CompareTo(Other.Date) * -1
            End Function
        End Structure
        Friend Shared Function GetData(ByVal Site As String) As List(Of Data)
            Try
                Dim l As List(Of Data) = Nothing
                Using w As New WebClient
                    Dim r$ = w.DownloadString($"https://downdetector.co.uk/status/{Site}/")
                    If Not r.IsEmptyString Then
                        l = FNF.RegexFields(Of Data)(r, {Params}, {1, 2})
                        If l.ListExists(2) Then
                            Dim lDate As Date = l(0).Date
                            Dim i%
                            Dim indx% = -1
                            For i = 1 To l.Count - 1
                                If l(i).Date < lDate Then indx = i : Exit For Else lDate = l(i).Date
                            Next
                            If indx >= 0 Then
                                For i = indx To 0 Step -1 : l.RemoveAt(i) : Next
                            End If
                            l.Sort()
                        End If
                    End If
                End Using
                Return l
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendInLog + EDP.ReturnValue, ex, $"[DownDetector.GetData({Site})]")
            End Try
        End Function
    End Class
End Namespace