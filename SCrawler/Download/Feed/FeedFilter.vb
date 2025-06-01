' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports SCrawler.API.Base
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace DownloadObjects
    Friend Class FeedFilter : Implements IEContainerProvider, IComparable, ICopier
#Region "XML"
        Private Const Name_Name As String = "Name"
        Private Const Name_Types As String = "Types"
        Private Const Name_Users As String = "Users"
        Private Const Name_Sites As String = "Sites"
#End Region
#Region "Properties"
        Friend Property Name As String
        Friend ReadOnly Property Types As List(Of UTypes)
        Friend ReadOnly Property Users As List(Of UserInfo)
        Friend ReadOnly Property Sites As List(Of String)
#End Region
#Region "Initializers"
        Friend Sub New()
            Types = New List(Of UTypes)
            Users = New List(Of UserInfo)
            Sites = New List(Of String)
        End Sub
        Friend Sub New(ByVal e As EContainer)
            Me.New
            Name = e.Value(Name_Name)
            Types.ListAddList(e.Value(Name_Types).StringToList(Of Integer)(","))
            Sites.ListAddList(e.Value(Name_Sites).StringToList(Of String)("|"))
            If If(e(Name_Users)?.Count, 0) > 0 Then
                Users.ListAddList(e(Name_Users), LAP.IgnoreICopier)
                Users.RemoveAll(Function(u) Not u.File.Exists)
            End If
        End Sub
        Public Shared Widening Operator CType(ByVal e As EContainer) As FeedFilter
            Return New FeedFilter(e)
        End Operator
        Public Shared Widening Operator CType(ByVal f As FeedFilter) As String
            Return f.ToString
        End Operator
#End Region
#Region "Base overrides"
        Public Overrides Function ToString() As String
            Return Name
        End Function
        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            If Not Obj Is Nothing AndAlso Not IsDBNull(Obj) Then
                Return DirectCast(Obj, FeedFilter).Name = Name
            Else
                Return False
            End If
        End Function
#End Region
#Region "IComparable Support"
        Private Function CompareTo(ByVal Obj As Object) As Integer Implements IComparable.CompareTo
            If Not Obj Is Nothing AndAlso Not IsDBNull(Obj) Then
                Return Name.CompareTo(DirectCast(Obj, FeedFilter).Name)
            Else
                Return 0
            End If
        End Function
#End Region
#Region "IEContainerProvider Support"
        Private Function ToEContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
            Dim ee As New EContainer("User") With {.AllowSameNames = True}
            ee.Add(New EContainer(Name_Name, Name))
            ee.Add(New EContainer(Name_Types, Types.ListToString(",")))
            ee.Add(New EContainer(Name_Sites, Sites.ListToString("|")))
            If Users.Count > 0 Then
                ee.Add(New EContainer(Name_Users, String.Empty))
                ee(Name_Users).AddRange(Users)
            End If
            Return ee
        End Function
#End Region
#Region "ICopier Support"
        Friend Overloads Function Copy() As Object Implements ICopier.Copy
            Return (New FeedFilter).Copy(Me)
        End Function
        Friend Overloads Function Copy(ByVal Source As Object) As Object Implements ICopier.Copy
            With DirectCast(Source, FeedFilter)
                Name = .Name
                Types.Clear()
                Types.ListAddList(.Types, LAP.NotContainsOnly)
                Users.Clear()
                Users.ListAddList(.Users, LAP.NotContainsOnly)
                Sites.Clear()
                Sites.ListAddList(.Sites, LAP.NotContainsOnly)
            End With
            Return Me
        End Function
#End Region
    End Class
    Friend Class FeedFilterCollection : Implements IEnumerable(Of FeedFilter), IMyEnumerator(Of FeedFilter)
#Region "XML"
        Private Const Name_CurrentFilterName As String = "CurrentFilterName"
        Private Const Name_FiltersNode As String = "Filters"
#End Region
#Region "Declarations"
        Private ReadOnly MyFilters As List(Of FeedFilter)
#Region "Filters: current, temp"
        Private _CurrentFilterName As String = String.Empty
        Private _CurrentFilterNameLast As String = String.Empty
        Friend Property CurrentFilterName As String
            Get
                Return _CurrentFilterName
            End Get
            Set(ByVal NewName As String)
                If Not _CurrentFilterName = NewName Then _CurrentFilterNameLast = _CurrentFilterName
                _CurrentFilterName = NewName
            End Set
        End Property
        Private _TEMP As FeedFilter = Nothing
        Friend Property TEMP As FeedFilter
            Get
                Return _TEMP
            End Get
            Set(ByVal f As FeedFilter)
                _TEMP = f
                If _TEMP Is Nothing And _CurrentFilterName.IsEmptyString And Not _CurrentFilterNameLast.IsEmptyString Then _
                   _CurrentFilterName = _CurrentFilterNameLast
            End Set
        End Property
        Friend ReadOnly Property Current(Optional ByVal Any As Boolean = True, Optional ByVal GetTemp As Boolean = True) As FeedFilter
            Get
                If Any Then
                    If CurrentFilterName.IsEmptyString Then
                        Return TEMP
                    Else
                        Return Item(CurrentFilterName)
                    End If
                ElseIf GetTemp Then
                    Return TEMP
                ElseIf Not CurrentFilterName.IsEmptyString Then
                    Return Item(CurrentFilterName)
                Else
                    Return Nothing
                End If
            End Get
        End Property
        Friend ReadOnly Property Exists As Boolean
            Get
                Return Not Current Is Nothing
            End Get
        End Property
        Friend Sub Disable(Optional ByVal All As Boolean = True, Optional ByVal Saved As Boolean = True, Optional ByVal Temp As Boolean = True)
            If All Or Saved Then
                _CurrentFilterName = String.Empty
                _CurrentFilterNameLast = String.Empty
                Update()
            End If
            If All Or Temp Then _TEMP = Nothing
        End Sub
#End Region
        Private ReadOnly File As New SFile($"{SettingsFolderName}\FeedDataFilters.xml")
        Friend ReadOnly UsersToStringProvider As New CustomProvider(Function(ByVal v As UserInfo) As Object
                                                                        Dim u As UserDataBase = Settings.GetUser(v)
                                                                        Return If(u?.ToStringExt(True), UserDataBase.ToStringExt(v))
                                                                    End Function)
#End Region
#Region "Initializers"
        Friend Sub New()
            MyFilters = New List(Of FeedFilter)
            If File.Exists Then
                Using x As New XmlFile(File, Protector.Modes.All, False) With {.AllowSameNames = True}
                    x.LoadData()
                    If x.Count > 0 Then
                        CurrentFilterName = x.Value(Name_CurrentFilterName)
                        MyFilters.ListAddList(x(Name_FiltersNode), LAP.IgnoreICopier)
                    End If
                End Using
            End If
        End Sub
#End Region
#Region "Count"
        Friend ReadOnly Property Count As Integer Implements IMyEnumerator(Of FeedFilter).MyEnumeratorCount
            Get
                Return MyFilters.Count
            End Get
        End Property
#End Region
#Region "Defaults: Item"
        Default Friend ReadOnly Property Item(ByVal Index As Integer) As FeedFilter Implements IMyEnumerator(Of FeedFilter).MyEnumeratorObject
            Get
                Return MyFilters(Index)
            End Get
        End Property
        Default Friend ReadOnly Property Item(ByVal Name As String) As FeedFilter
            Get
                Return MyFilters.Find(Function(ff) ff.Name = Name)
            End Get
        End Property
#End Region
#Region "Collection functions"
        Friend Function Add(ByVal f As FeedFilter, Optional ByVal SetAsCurrent As Boolean = False) As Integer
            If Not MyFilters.Contains(f) Then
                MyFilters.Add(f)
                MyFilters.Sort()
                Update()
            End If
            Dim i% = MyFilters.IndexOf(f)
            If i >= 0 And SetAsCurrent Then CurrentFilterName = Item(i).Name : _TEMP = Nothing
            Return i
        End Function
        Friend Function Delete(ByVal f As FeedFilter) As Boolean
            Dim i% = MyFilters.IndexOf(f)
            If i >= 0 Then
                MyFilters.RemoveAt(i)
                Update()
                Return True
            Else
                Return False
            End If
        End Function
        Friend Overloads Function Update(ByVal f As FeedFilter) As Integer
            Dim i% = MyFilters.IndexOf(f)
            If i >= 0 Then MyFilters(i) = f : Update()
            Return i
        End Function
        Friend Overloads Sub Update()
            If MyFilters.Count > 0 Then
                Using x As New XmlFile With {.AllowSameNames = True}
                    x.Add(Name_CurrentFilterName, CurrentFilterName)
                    x.Add(Name_FiltersNode, String.Empty)
                    x(Name_FiltersNode).AddRange(MyFilters)
                    x.Name = "MyFilters"
                    x.Save(File)
                End Using
            Else
                File.Delete(SFO.File, Settings.DeleteMode, EDP.ReturnValue)
            End If
        End Sub
#End Region
#Region "IEnumerable Support"
        Private Function GetEnumerator() As IEnumerator(Of FeedFilter) Implements IEnumerable(Of FeedFilter).GetEnumerator
            Return New MyEnumerator(Of FeedFilter)(Me)
        End Function
        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
#End Region
    End Class
End Namespace