' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin
Imports PersonalUtilities.Functions.RegularExpressions
Imports Download = SCrawler.Plugin.ISiteSettings.Download
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
                Dim r$ = GetWebString($"https://downdetector.co.uk/status/{Site}/",, EDP.ThrowException)
                If Not r.IsEmptyString Then
                    l = RegexFields(Of Data)(r, {Params}, {1, 2})
                    If l.ListExists(2) Then
                        l.Sort()
                        l2 = New List(Of Data)
                        Dim d As Data
                        Dim eDates As New List(Of Date)
                        Dim MaxValue As Func(Of Date, Integer) = Function(dd) (From ddd As Data In l Where ddd.Date = dd Select ddd.Value).DefaultIfEmpty(0).Max
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
                Return l2
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, $"[DownDetector.GetData({Site})]")
            End Try
        End Function
        Friend Interface IDownDetector
            ReadOnly Property Value As Integer
            ReadOnly Property AddToLog As Boolean
            ReadOnly Property CheckSite As String
            Function Available(ByVal What As Download, ByVal Silent As Boolean) As Boolean
        End Interface
        Friend Class Checker(Of T As {ISiteSettings, IDownDetector})
            Protected ReadOnly Property Source As T
            Private ReadOnly NP As New ANumbers With {.FormatOptions = ANumbers.Options.GroupIntegral}
            Friend Sub New(ByRef _Source As T)
                Source = _Source
            End Sub
            Private ____AvailableChecked As Boolean = False
            Private ____AvailableResult As Boolean = False
            Friend Overridable Function Available(ByVal What As Download, ByVal Silent As Boolean) As Boolean
                If Settings.DownDetectorEnabled And Source.Value >= 0 Then
                    If Not ____AvailableChecked Then
                        ____AvailableResult = AvailableImpl(What, Silent)
                        ____AvailableChecked = True
                    End If
                    Return ____AvailableResult
                Else
                    Return True
                End If
            End Function
            Protected Overridable Function AvailableImpl(ByVal What As Download, ByVal Silent As Boolean) As Boolean
                Try
                    Source.AvailableText = String.Empty
                    If Source.Value < 0 Then
                        Return True
                    Else
                        Dim dl As List(Of Data) = GetData(Source.CheckSite)
                        If dl.ListExists Then
                            dl = dl.Take(4).ToList
                            Dim avg% = dl.Average(Function(d) d.Value)
                            If avg > Source.Value Then
                                Source.AvailableText = $"Over the past hour, {Source.Site} has received an average of {avg.NumToString(NP)} outage reports:{vbCr}{dl.ListToString(vbCr)}"
                                If Source.AddToLog Then MyMainLOG = Source.AvailableText
                                If Silent Then
                                    Return AvailableImpl_FALSE_SILENT()
                                Else
                                    If MsgBoxE({$"{Source.AvailableText}{vbCr}{vbCr}Do you want to continue parsing {Source.Site} data?",
                                                $"There are outage reports on {Source.Site}"}, vbYesNo) = vbYes Then
                                        Return AvailableImpl_FALSE_SILENT_NOT_MSG_YES()
                                    Else
                                        Return AvailableImpl_FALSE_SILENT_NOT_MSG_NO()
                                    End If
                                End If
                            End If
                        End If
                        Return AvailableImpl_TRUE()
                    End If
                Catch ex As Exception
                    Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, $"[API.{Source.Site}.SiteSettings.Available([DownDetector])]", True)
                End Try
            End Function
            Protected Overridable Function AvailableImpl_TRUE() As Boolean
                Return True
            End Function
            Protected Overridable Function AvailableImpl_FALSE_SILENT() As Boolean
                Return False
            End Function
            Protected Overridable Function AvailableImpl_FALSE_SILENT_NOT_MSG_YES() As Boolean
                Return True
            End Function
            Protected Overridable Function AvailableImpl_FALSE_SILENT_NOT_MSG_NO() As Boolean
                Return False
            End Function
            Friend Overridable Sub Reset()
                ____AvailableChecked = False
                ____AvailableResult = False
                Source.AvailableText = String.Empty
            End Sub
        End Class
    End Class
End Namespace