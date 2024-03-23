' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Tools
Namespace Editors
    Friend Structure DataColor : Implements IEContainerProvider, IComparable(Of DataColor)
        Private Const Name_BackColor As String = "BackColor"
        Private Const Name_ForeColor As String = "ForeColor"
        Friend BackColor As Color?
        Friend ForeColor As Color?
        Friend Name As String
        Friend ReadOnly Property Exists As Boolean
            Get
                Return BackColor.HasValue Or ForeColor.HasValue Or Not Name.IsEmptyString
            End Get
        End Property
        Friend Sub New(ByVal e As EContainer)
            If Not e Is Nothing Then
                BackColor = AConvert(Of Color)(e.Attribute(Name_BackColor).Value, AModes.Var, Nothing)
                ForeColor = AConvert(Of Color)(e.Attribute(Name_ForeColor).Value, AModes.Var, Nothing)
                Name = e.Value
            End If
        End Sub
        Public Overrides Function ToString() As String
            Return Name
        End Function
        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            If Not IsNothing(Obj) AndAlso TypeOf Obj Is DataColor Then
                With DirectCast(Obj, DataColor)
                    Return AEquals(BackColor, .BackColor) And AEquals(ForeColor, .ForeColor)
                End With
            Else
                Return False
            End If
        End Function
        Private Function CompareTo(ByVal Other As DataColor) As Integer Implements IComparable(Of DataColor).CompareTo
            Return Name.CompareTo(Other.Name)
        End Function
        Private Function ToEContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
            Return New EContainer("Color", Name, {New EAttribute(Name_BackColor, AConvert(Of String)(BackColor, AModes.Var, String.Empty)),
                                                  New EAttribute(Name_ForeColor, AConvert(Of String)(ForeColor, AModes.Var, String.Empty))})
        End Function
    End Structure
    Friend Class DataColorCollection : Implements IEnumerable(Of DataColor), IMyEnumerator(Of DataColor)
        Private ReadOnly Colors As List(Of DataColor)
        Private ReadOnly File As SFile = $"{SettingsFolderName}\Colors.xml"
        Friend Sub New()
            Colors = New List(Of DataColor)
            If File.Exists Then
                Using x As New XmlFile(File, Protector.Modes.All, False) With {.AllowSameNames = True}
                    x.LoadData()
                    If x.Count > 0 Then Colors.ListAddList(x, LAP.IgnoreICopier)
                End Using
            End If
            If Colors.Count > 0 Then Colors.Sort()
        End Sub
        Friend ReadOnly Property Item(ByVal Index As Integer) As DataColor Implements IMyEnumerator(Of DataColor).MyEnumeratorObject
            Get
                Return Colors(Index)
            End Get
        End Property
        Friend ReadOnly Property Count As Integer Implements IMyEnumerator(Of DataColor).MyEnumeratorCount
            Get
                Return Colors.Count
            End Get
        End Property
        Friend Sub Update()
            If Count > 0 Then
                Colors.Sort()
                Using x As New XmlFile With {.AllowSameNames = True}
                    x.AddRange(Colors)
                    x.Name = "Colors"
                    x.Save(File, EDP.SendToLog)
                End Using
            Else
                File.Delete()
            End If
        End Sub
        Friend Function Add(ByVal Item As DataColor, Optional ByVal AutoUpdate As Boolean = True) As Integer
            If IndexOf(Item) = -1 Then
                Colors.Add(Item)
                Colors.Sort()
                If AutoUpdate Then Update()
                Return IndexOf(Item)
            Else
                Return -1
            End If
        End Function
        Friend Function IndexOf(ByVal Item As DataColor) As Integer
            If Count > 0 Then Return Colors.IndexOf(Item) Else Return -1
        End Function
        Private Function GetEnumerator() As IEnumerator(Of DataColor) Implements IEnumerable(Of DataColor).GetEnumerator
            Return New MyEnumerator(Of DataColor)(Me)
        End Function
        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
    End Class
End Namespace