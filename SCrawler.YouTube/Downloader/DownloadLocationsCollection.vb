' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Functions.XML.Attributes
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Tools
Namespace DownloadObjects.STDownloader
    Public Structure DownloadLocation : Implements IComparable(Of DownloadLocation), IEquatable(Of DownloadLocation), IEContainerProvider
        <XMLECA(NameOf(Path))> Public Name As String
        <XMLEC> Public Path As String
        <XMLECA(NameOf(Path), NullValue:=-1, NullValueExists:=True)>
        Public Model As Integer
        ''' <param name="Path">with separator</param>
        Public Sub New(ByVal Path As String)
            Me.New(Path, -1)
        End Sub
        ''' <inheritdoc cref="DownloadLocation.New(String)"/>
        Public Sub New(ByVal Path As String, ByVal Model As Integer)
            Me.Path = Path
            Me.Model = Model
        End Sub
        Public Shared Widening Operator CType(ByVal Path As String) As DownloadLocation
            Return New DownloadLocation(Path)
        End Operator
        Public Shared Narrowing Operator CType(ByVal Path As SFile) As DownloadLocation
            Return New DownloadLocation(Path.PathWithSeparator)
        End Operator
        Public Shared Widening Operator CType(ByVal Location As DownloadLocation) As String
            Return Location.Path
        End Operator
        Public Overrides Function ToString() As String
            Return Path
        End Function
        Public Overloads Overrides Function Equals(ByVal Obj As Object) As Boolean
            If Not IsNothing(Obj) Then
                If TypeOf Obj Is DownloadLocation Then
                    Return Equals(DirectCast(Obj, DownloadLocation))
                Else
                    Return Obj.ToString = Path
                End If
            Else
                Return False
            End If
        End Function
        Public Overloads Function Equals(ByVal Other As DownloadLocation) As Boolean Implements IEquatable(Of DownloadLocation).Equals
            Return Path = Other.Path And Model = Other.Model
        End Function
        Private Function ToEContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
            Return XMLGenerateContainers(Me).FirstOrDefault
        End Function
        Private Function CompareTo(ByVal Other As DownloadLocation) As Integer Implements IComparable(Of DownloadLocation).CompareTo
            Return Name.CompareTo(Other.Name)
        End Function
    End Structure
    Public Class DownloadLocationsCollection : Implements ICollection(Of DownloadLocation), IMyEnumerator(Of DownloadLocation)
        Private ReadOnly Property Locations As List(Of DownloadLocation)
        Private WorkingFile As SFile
        Public Sub New()
            Locations = New List(Of DownloadLocation)
        End Sub
        Public Sub Load(ByVal IsGlobal As Boolean, Optional ByVal IsYT As Boolean = False, Optional ByVal File As SFile = Nothing)
            If Not IsGlobal Then
                WorkingFile = $"Settings\DownloadLocations{IIf(IsYT, "YouTube", String.Empty)}.xml"
            ElseIf Not File.IsEmptyString Then
                WorkingFile = File
            Else
                Throw New ArgumentNullException("File", "File cannot be null in global locations instance")
            End If
            If WorkingFile.Exists Then
                Using x As New XmlFile(WorkingFile, Protector.Modes.All, False) With {.AllowSameNames = True}
                    x.LoadData()
                    Locations.ListAddList(x.XMLGenerateInstances(Of DownloadLocation), LAP.NotContainsOnly)
                End Using
            End If
        End Sub
        Private ReadOnly Property IsReadOnly As Boolean = False Implements ICollection(Of DownloadLocation).IsReadOnly
        Public ReadOnly Property Count As Integer Implements ICollection(Of DownloadLocation).Count, IMyEnumerator(Of DownloadLocation).MyEnumeratorCount
            Get
                Return Locations.Count
            End Get
        End Property
        Default Public ReadOnly Property Item(ByVal Index As Integer) As DownloadLocation Implements IMyEnumerator(Of DownloadLocation).MyEnumeratorObject
            Get
                Return Locations(Index)
            End Get
        End Property
        Public Shared Sub AddCmbColumns(ByRef CMB As ComboBoxExtended, Optional ByVal UseUpdate As Boolean = True)
            With CMB
                If UseUpdate Then .BeginUpdate()
                With .Columns
                    .Clear()
                    .Add(New ListColumn("COL_NAME", "Name") With {.DisplayMember = False, .ValueMember = False, .AutoWidth = True, .Width = -1})
                    .Add(New ListColumn("COL_VALUE", "Value") With {.DisplayMember = True, .ValueMember = True, .Visible = False})
                End With
                If UseUpdate Then .EndUpdate(True)
            End With
        End Sub
        Public Sub PopulateComboBox(ByRef CMB As ComboBoxExtended, Optional ByVal Current As SFile = Nothing, Optional ByVal IsFile As Boolean = False)
            Locations.Sort()
            With CMB
                .BeginUpdate()

                If .Columns.Count = 0 Then AddCmbColumns(CMB, False)

                .Items.Clear()

                If Count > 0 Then
                    .Items.AddRange(Locations.Select(Function(l) New ListItem({l.Name, l.Path})))
                    .LeaveDefaultButtons = True
                Else
                    .LeaveDefaultButtons = False
                End If

                .ListAutoCompleteMode = ComboBoxExtended.AutoCompleteModes.Disabled

                .EndUpdate()

                If Not Current.IsEmptyString And Locations.Count > 0 Then
                    Dim i% = IndexOf(If(IsFile, Current.ToString, Current.PathWithSeparator),, IsFile)
                    If i.ValueBetween(0, .Items.Count - 1) Then .SelectedIndex = i
                    If Current.File.IsEmptyString Then CMB.Text = Current.PathWithSeparator Else CMB.Text = Current
                End If
            End With
        End Sub
        Public Function ChooseNewLocation(ByRef CMB As ComboBoxExtended, ByVal AddToList As Boolean, ByVal AskForName As Boolean) As SFile
            Dim f As SFile = SFile.SelectPath(CMB.Text.CSFileP, "Select output directory", EDP.ReturnValue)
            If Not f.IsEmptyString Then
                CMB.Text = f.PathWithSeparator
                If AddToList Then
                    Add(New DownloadLocation(f.PathWithSeparator), AskForName)
                    PopulateComboBox(CMB, f)
                End If
            End If
            Return f
        End Function
        Friend Function ChooseNewPlaylistArray(ByRef CMB As ComboBoxExtended, ByRef Result As Boolean) As IEnumerable(Of SFile)
            Try
                Dim initFiles As IEnumerable(Of SFile) = Nothing
                Dim selectedFiles As IEnumerable(Of SFile) = Nothing
                If Count > 0 Then initFiles = Me.Select(Function(l) l.Path.CSFile)
                Dim addh As New EventHandler(Of SimpleListFormEventArgs)(Sub(ByVal s As Object, ByVal ee As SimpleListFormEventArgs)
                                                                             Dim ff As List(Of SFile) = SFile.SelectFiles(,, "Select playlist files", "Playlist|*.m3u;*.m3u8|AllFiles|*.*", EDP.ReturnValue)
                                                                             If ff.ListExists Then
                                                                                 ee.AddItem(ff.Cast(Of Object))
                                                                                 ee.Result = True
                                                                             Else
                                                                                 ee.Result = False
                                                                             End If
                                                                         End Sub)
                Using f As New SimpleListForm(Of SFile)(initFiles, API.YouTube.MyYouTubeSettings.DesignXml) With {
                    .DesignXMLNodeName = "M3U8SelectorForm",
                    .FormText = "Playlists",
                    .Buttons = {ActionButton.DefaultButtons.Add},
                    .Icon = ImageRenderer.GetIcon(My.Resources.StartPic_Green_16, EDP.ReturnValue),
                    .AddFunction = addh
                }
                    If f.ShowDialog = DialogResult.OK Then Result = True : selectedFiles = ListAddList(Nothing, f.DataResult, LAP.NotContainsOnly)
                End Using
                If selectedFiles.ListExists Then
                    Dim added As Boolean = False
                    selectedFiles.ListForEach(Sub(ByVal plsFile As SFile, ByVal ii As Integer)
                                                  If IndexOf(plsFile.ToString,, True) = -1 Then Add(plsFile.ToString, True, True) : added = True
                                              End Sub)
                    If added Then PopulateComboBox(CMB, selectedFiles(0).ToString, True)
                    CMB.Text = selectedFiles(0)
                    Return selectedFiles
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Select playlist array")
            End Try
        End Function
        Private Sub Update()
            If Locations.Count > 0 Then
                Using x As New XmlFile With {.AllowSameNames = True}
                    x.AddRange(Locations)
                    x.Name = "Locations"
                    x.Save(WorkingFile, EDP.SendToLog)
                End Using
            Else
                WorkingFile.Delete(,, EDP.None)
            End If
        End Sub
        Public Sub Clear() Implements ICollection(Of DownloadLocation).Clear
            If Locations.Count > 0 Then Locations.Clear() : Update()
        End Sub
        Public Overloads Sub Add(ByVal Item As DownloadLocation) Implements ICollection(Of DownloadLocation).Add
            Add(Item, True)
        End Sub
        Public Overloads Sub Add(ByVal Item As DownloadLocation, ByVal AskForName As Boolean, Optional ByVal IsFile As Boolean = False)
            If Not Item.Path.IsEmptyString Then
                Dim i% = IndexOf(Item,, IsFile)
                Dim processUpdate As Boolean = True
                If i >= 0 Then
                    If Locations(i).Model = Item.Model Then
                        processUpdate = False
                    Else
                        Locations(i) = Item
                    End If
                Else
                    If Item.Name.IsEmptyString And AskForName Then Item.Name = InputBoxE("Enter a new name for the new location", "Location name", Item.Path)
                    If Item.Name.IsEmptyString Then Item.Name = Item.Path
                    Locations.Add(Item)
                    Locations.Sort()
                End If
                If processUpdate Then Update()
            End If
        End Sub
        Private Sub CopyTo(ByVal Array() As DownloadLocation, ByVal ArrayIndex As Integer) Implements ICollection(Of DownloadLocation).CopyTo
            Locations.CopyTo(Array, ArrayIndex)
        End Sub
        Public Function Contains(ByVal Item As DownloadLocation) As Boolean Implements ICollection(Of DownloadLocation).Contains
            Return Not Item.Path.IsEmptyString AndAlso Locations.Contains(Item)
        End Function
        Public Function IndexOf(ByVal Item As DownloadLocation, Optional ByVal IgnoreModel As Boolean = False, Optional ByVal IsFile As Boolean = False) As Integer
            If Not IsFile Then
                Return Locations.FindIndex(Function(d) d.Path = Item.Path And (d.Model = Item.Model Or IgnoreModel))
            Else
                Return Locations.FindIndex(Function(d) d.Path = Item.Path)
            End If
        End Function
        Public Function Remove(ByVal Item As DownloadLocation) As Boolean Implements ICollection(Of DownloadLocation).Remove
            If Locations.Remove(Item) Then
                Update()
                Return True
            Else
                Return False
            End If
        End Function
        Private Function GetEnumerator() As IEnumerator(Of DownloadLocation) Implements IEnumerable(Of DownloadLocation).GetEnumerator
            Return New MyEnumerator(Of DownloadLocation)(Me)
        End Function
        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
    End Class
End Namespace
