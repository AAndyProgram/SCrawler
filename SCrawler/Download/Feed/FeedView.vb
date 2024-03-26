' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Namespace DownloadObjects
    Friend Structure FeedView : Implements IEContainerProvider, IComparable(Of FeedView)
#Region "Names"
        Private Const Name_Name As String = "Name"
        Private Const Name_Rows As String = "Rows"
        Private Const Name_Columns As String = "Columns"
        Private Const Name_CenterImage As String = "CenterImage"
        Private Const Name_CenterImageUse As String = "CenterImageUse"
        Private Const Name_Endless As String = "Endless"
        Private Const Name_BackColor As String = "BackColor"
        Private Const Name_ForeColor As String = "ForeColor"
        Private Const Name_View As String = "View"
#End Region
#Region "Declarations"
        Friend Name As String
        Friend Rows As Integer
        Friend Columns As Integer
        Friend CenterImage As Integer
        Friend CenterImageUse As Boolean
        Friend Endless As Boolean
        Friend BackColor As Color?
        Friend ForeColor As Color?
        Friend View As FormView
#End Region
#Region "Initializers"
        Friend Sub New(ByVal e As EContainer)
            With e
                Name = .Value(Name_Name)
                Rows = .Value(Name_Rows).FromXML(Of Integer)(10)
                Columns = .Value(Name_Columns).FromXML(Of Integer)(1)
                CenterImage = .Value(Name_CenterImage).FromXML(Of Integer)(1)
                CenterImageUse = .Value(Name_CenterImageUse).FromXML(Of Boolean)(False)
                Endless = .Value(Name_Endless).FromXML(Of Boolean)(True)
                BackColor = AConvert(Of Color)(.Value(Name_BackColor), AModes.Var, Nothing)
                ForeColor = AConvert(Of Color)(.Value(Name_ForeColor), AModes.Var, Nothing)
                View = New FormView
                View.Import(e, {Name_View})
            End With
        End Sub
        Friend Shared Function FromCurrent(Optional ByVal Name As String = "") As FeedView
            Dim v As New FeedView
            With Settings
                v.Name = Name
                v.Rows = .FeedDataRows
                v.Columns = .FeedDataColumns
                v.Endless = .FeedEndless
                v.CenterImage = .FeedCenterImage
                v.CenterImageUse = .FeedCenterImage.Use
                If .FeedBackColor.Exists Then
                    v.BackColor = .FeedBackColor
                Else
                    v.BackColor = Nothing
                End If
                If .FeedForeColor.Exists Then
                    v.ForeColor = .FeedForeColor
                Else
                    v.ForeColor = Nothing
                End If
                v.View = New FormView
                With MainFrameObj.MF.MyFeed.MyDefs.MyView
                    v.View.Location = .Location
                    v.View.LocationOnly = .LocationOnly
                    v.View.Size = .Size
                    v.View.WindowState = .WindowState
                End With
            End With
            Return v
        End Function
#End Region
        Friend Sub Populate()
            With Settings
                .BeginUpdate()

                .FeedDataRows.Value = Rows
                .FeedDataColumns.Value = Columns
                .FeedEndless.Value = Endless
                .FeedCenterImage.Value = CenterImage
                .FeedCenterImage.Use = CenterImageUse
                If BackColor.HasValue Then
                    .FeedBackColor.Value = BackColor.Value
                Else
                    .FeedBackColor.ValueF = Nothing
                End If
                If ForeColor.HasValue Then
                    .FeedForeColor.Value = ForeColor.Value
                Else
                    .FeedForeColor.ValueF = Nothing
                End If
                If Not View Is Nothing Then
                    With MainFrameObj.MF.MyFeed.MyDefs.MyView
                        .Location = View.Location
                        .LocationOnly = View.LocationOnly
                        .Size = View.Size
                        .WindowState = View.WindowState
                    End With
                End If

                .EndUpdate()
            End With
        End Sub
        Public Overrides Function ToString() As String
            Return Name
        End Function
        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            If Not IsNothing(Obj) AndAlso TypeOf Obj Is FeedView Then
                Return Name.StringToLower = DirectCast(Obj, FeedView).Name.StringToLower
            Else
                Return False
            End If
        End Function
        Private Function CompareTo(ByVal Other As FeedView) As Integer Implements IComparable(Of FeedView).CompareTo
            Return Name.CompareTo(Other.Name)
        End Function
        Private Function ToEContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
            Dim container As New EContainer("FeedView") From {
                New EContainer(Name_Name, Name),
                New EContainer(Name_Rows, Rows),
                New EContainer(Name_Columns, Columns),
                New EContainer(Name_CenterImage, CenterImage),
                New EContainer(Name_CenterImageUse, CenterImageUse.BoolToInteger),
                New EContainer(Name_Endless, Endless.BoolToInteger),
                New EContainer(Name_BackColor, AConvert(Of String)(BackColor, AModes.Var, String.Empty)),
                New EContainer(Name_ForeColor, AConvert(Of String)(ForeColor, AModes.Var, String.Empty))
            }
            View.Export(container, {Name_View})
            Return container
        End Function
    End Structure
    Friend Class FeedViewCollection : Implements IEnumerable(Of FeedView), IMyEnumerator(Of FeedView)
        Private ReadOnly Views As List(Of FeedView)
        Private ReadOnly File As SFile = $"{SettingsFolderName}\FeedView.xml"
        Friend Sub New()
            Views = New List(Of FeedView)
            If File.Exists Then
                Using x As New XmlFile(File, Protector.Modes.All, False) With {.AllowSameNames = True}
                    x.LoadData()
                    If x.Count > 0 Then Views.ListAddList(x, LAP.IgnoreICopier)
                End Using
            End If
            If Views.Count > 0 Then Views.Sort()
        End Sub
        Default Friend ReadOnly Property Item(ByVal Index As Integer) As FeedView Implements IMyEnumerator(Of FeedView).MyEnumeratorObject
            Get
                Return Views(Index)
            End Get
        End Property
        Friend ReadOnly Property Count As Integer Implements IMyEnumerator(Of FeedView).MyEnumeratorCount
            Get
                Return Views.Count
            End Get
        End Property
        Friend Sub Update()
            If Count > 0 Then
                Views.Sort()
                Using x As New XmlFile With {.AllowSameNames = True}
                    x.AddRange(Views)
                    x.Name = "FeedViews"
                    x.Save(File, EDP.LogMessageValue)
                End Using
            Else
                If File.Exists Then File.Delete()
            End If
        End Sub
        Friend Sub Add(ByVal Item As FeedView, Optional ByVal AutoUpdate As Boolean = True)
            Dim i% = IndexOf(Item)
            If i >= 0 Then
                Views(i) = Item
            Else
                Views.Add(Item)
            End If
            Views.Sort()
            If AutoUpdate Then Update()
        End Sub
        Friend Overloads Function IndexOf(ByVal Item As FeedView) As Integer
            If Count > 0 Then
                Return Views.IndexOf(Item)
            Else
                Return -1
            End If
        End Function
        Friend Overloads Function IndexOf(ByVal Name As String) As Integer
            Return IndexOf(New FeedView With {.Name = Name})
        End Function
        Private Function GetEnumerator() As IEnumerator(Of FeedView) Implements IEnumerable(Of FeedView).GetEnumerator
            Return New MyEnumerator(Of FeedView)(Me)
        End Function
        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
    End Class
End Namespace