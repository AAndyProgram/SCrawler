' Copyright (C) 2023  Andy https://github.com/AAndyProgram
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
            Private Function CreateFromArray(ByVal ParamsArray() As String) As Object Implements IRegExCreator.CreateFromArray
                If ParamsArray.ListExists Then
                    Try : [Date] = Date.Parse(ParamsArray(0)) : Catch : End Try
                    If ParamsArray.Length > 1 Then Value = AConvert(Of Integer)(ParamsArray(1), 0)
                End If
                Return Me
            End Function
            Public Overrides Function ToString() As String
                Return $"{AConvert(Of String)([Date], ADateTime.Formats.BaseDateTime, String.Empty)} [{Value}]"
            End Function
            Private Function CompareTo(ByVal Other As Data) As Integer Implements IComparable(Of Data).CompareTo
                Return [Date].CompareTo(Other.Date) * -1
            End Function
        End Structure
        Friend Shared Function GetData(ByVal Site As String) As List(Of Data)
            Try
                Dim l As List(Of Data) = Nothing
                Dim l2 As List(Of Data) = Nothing
                Using w As New WebClient
                    Dim r$ = w.DownloadString($"https://downdetector.co.uk/status/{Site}/")
                    If Not r.IsEmptyString Then
                        l = RegexFields(Of Data)(r, {Params}, {1, 2})
                        If l.ListExists(2) Then
                            l.Sort()
                            l2 = New List(Of Data)
                            Dim d As Data
                            Dim eDates As New List(Of Date)
                            Dim MaxValue As Func(Of Date, Integer) = Function(dd) (From ddd In l Where ddd.Date = dd Select ddd.Value).DefaultIfEmpty(0).Max
                            For i% = 0 To l.Count - 1
                                If Not eDates.Contains(l(i).Date) Then
                                    d = l(i)
                                    d.Value = MaxValue(d.Date)
                                    l2.Add(d)
                                    eDates.Add(d.Date)
                                End If
                            Next
                            eDates.Clear()
                            l.Clear()
                            l2.Sort()
                        End If
                    End If
                End Using
                Return l2
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendInLog + EDP.ReturnValue, ex, $"[DownDetector.GetData({Site})]")
            End Try
        End Function
    End Class
End Namespace