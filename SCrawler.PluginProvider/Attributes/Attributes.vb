' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Runtime.CompilerServices
Namespace Plugin.Attributes
    ''' <summary>Create a control for a property</summary>
    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=False, Inherited:=False)> Public NotInheritable Class PropertyOption : Inherits Attribute
        ''' <summary>Property name</summary>
        Public ReadOnly Property Name As String
        ''' <summary>Property value type</summary>
        Public Property [Type] As Type
        Private _ControlText As String
        ''' <summary>This text will be displayed on the control information.<br/>Default: equals the name (property name)</summary>
        Public Property ControlText As String
            Get
                Return If(String.IsNullOrEmpty(_ControlText), Name, _ControlText)
            End Get
            Set(ByVal NewText As String)
                _ControlText = NewText
            End Set
        End Property
        ''' <summary>This tooltip will be displayed on the control.<br/>Default: <see langword="String.Empty"/></summary>
        Public Property ControlToolTip As String
        ''' <summary>CheckBox ThreeStates mode</summary>
        Public Property ThreeStates As Boolean = False
        ''' <summary>Property allows null values</summary>
        Public Property AllowNull As Boolean = True
        ''' <summary>Offset the control from the left border of the form.<br/>Default: 100</summary>
        Public Property LeftOffset As Integer = 100
        ''' <summary>This control is an information label.<br/>Default: <see langword="False"/></summary>
        Public Property IsInformationLabel As Boolean = False
        ''' <summary>Label text alignment.<br/>Default: <see cref="Drawing.ContentAlignment.TopCenter"/></summary>
        Public Property LabelTextAlign As Drawing.ContentAlignment = Drawing.ContentAlignment.TopCenter
        ''' <summary>This is an authorization property</summary>
        Public Property IsAuth As Boolean = False
        ''' <summary>Initialize a new property option attribute</summary>
        ''' <param name="PropertyName">Property name</param>
        Public Sub New(<CallerMemberName()> Optional ByVal PropertyName As String = Nothing)
            Name = PropertyName
        End Sub
    End Class
    ''' <summary>Store property value in settings XML file</summary>
    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=False, Inherited:=False)> Public NotInheritable Class PXML : Inherits Attribute
        Public ReadOnly ElementName As String
        ''' <summary>Initialize a new XML attribute</summary>
        ''' <param name="XMLElementName">XML element name</param>
        Public Sub New(<CallerMemberName()> Optional ByVal XMLElementName As String = Nothing)
            ElementName = XMLElementName
        End Sub
    End Class
    ''' <summary>Special property updater</summary>
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=True, Inherited:=False)> Public NotInheritable Class PropertyUpdater : Inherits Attribute
        Public ReadOnly Name As String
        Public ReadOnly Dependencies As String()
        ''' <inheritdoc cref="PropertyUpdater.New(String, String())"/>
        Public Sub New(ByVal UpdatingPropertyName As String)
            Name = UpdatingPropertyName
        End Sub
        ''' <summary>Initialize a new PropertyUpdater attribute</summary>
        ''' <param name="UpdatingPropertyName">The name of the property to be updated</param>
        Public Sub New(ByVal UpdatingPropertyName As String, ByVal Dependent As String())
            Name = UpdatingPropertyName
            Dependencies = Dependent
        End Sub
    End Class
    ''' <summary>Plugin key</summary>
    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=False)> Public NotInheritable Class Manifest : Inherits Attribute
        Public ReadOnly GUID As String
        ''' <summary>Initialize a new Manifest attribute</summary>
        ''' <param name="ClassGuid">Plugin key</param>
        Public Sub New(ByVal ClassGuid As String)
            GUID = ClassGuid
        End Sub
    End Class
    ''' <summary>Special form attribute for settings forms and user creator form</summary>
    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=True, Inherited:=False)> Public NotInheritable Class SpecialForm : Inherits Attribute
        Public ReadOnly SettingsForm As Boolean
        ''' <summary>Initialize a new SpecialForm attribute</summary>
        ''' <param name="IsSettingsForm">
        ''' <see langword="True"/> - for setting form<br/>
        ''' <see langword="False"/> - for user creator form
        ''' </param>
        Public Sub New(ByVal IsSettingsForm As Boolean)
            SettingsForm = IsSettingsForm
        End Sub
    End Class
    ''' <summary>Property provider</summary>
    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=True, Inherited:=False)> Public NotInheritable Class Provider : Inherits Attribute
        Public ReadOnly Name As String
        ''' <summary>
        ''' <see langword="True"/> - form field validation provider. Must return null if the value is invalid.<br/>
        ''' <see langword="False"/> - only for conversion
        ''' </summary>
        Public FieldsChecker As Boolean = False
        ''' <summary>Initialize a new Provider attribute. <see cref="IFormatProvider"/> is only allowed</summary>
        ''' <param name="PropertyName">The name of the property for which this provider is used</param>
        Public Sub New(ByVal PropertyName As String)
            Name = PropertyName
        End Sub
    End Class
    ''' <summary>Sort attribute for settings form</summary>
    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=False, Inherited:=False)> Public NotInheritable Class ControlNumber : Inherits Attribute
        Public ReadOnly PropertyNumber As String
        ''' <summary>Initialize a new sort attribute instance for the settings form</summary>
        ''' <param name="Number">Object position number in the settings form</param>
        Public Sub New(ByVal Number As Integer)
            PropertyNumber = Number
        End Sub
    End Class
    ''' <summary>Attribute for properties values validation methods</summary>
    <AttributeUsage(AttributeTargets.Method, AllowMultiple:=True, Inherited:=False)> Public NotInheritable Class PropertiesDataChecker : Inherits Attribute
        Public ReadOnly ComparableNames As String()
        ''' <summary>Initialize a new PropertiesDataChecker attribute.</summary>
        ''' <param name="Names">Array of the property names</param>
        Public Sub New(ByVal Names As String())
            ComparableNames = Names
        End Sub
    End Class
    ''' <summary>This attribute specifies that users should be downloaded on a separate thread.</summary>
    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=False)> Public NotInheritable Class SeparatedTasks : Inherits Attribute
        Public ReadOnly TasksCount As Integer
        ''' <summary>Initialize a new SeparatedTasks attribute.</summary>
        ''' <param name="JobsCount">
        ''' Predefined task counter.<br/>
        ''' <see cref="TaskCounter"/> will take precedence if it is defined.
        ''' </param>
        Public Sub New(Optional ByVal JobsCount As Integer = -1)
            TasksCount = JobsCount
        End Sub
    End Class
    ''' <summary>A property attribute that specifies how many users should be downloaded at the same time in one thread</summary>
    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=False, Inherited:=False)> Public NotInheritable Class TaskCounter : Inherits Attribute
    End Class
    ''' <summary>This attribute indicates that the plugin has a SavedPosts environment</summary>
    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=False)> Public NotInheritable Class SavedPosts : Inherits Attribute
    End Class
    ''' <summary>This is an attribute of the UserData instance. Specifies that the default internal SCrawler downloader should be used.</summary>
    <AttributeUsage(AttributeTargets.Class, AllowMultiple:=False, Inherited:=False)> Public NotInheritable Class UseInternalDownloader : Inherits Attribute
    End Class
    ''' <summary>GitHub plugin info</summary>
    <AttributeUsage(AttributeTargets.Assembly, AllowMultiple:=False, Inherited:=False)> Public NotInheritable Class Github : Inherits Attribute
        Public ReadOnly UserName As String
        Public ReadOnly Repository As String
        ''' <summary>Initialize a new Github attribute.</summary>
        ''' <param name="Name">Developer GitHub username</param>
        ''' <param name="RepoName">Plugin repository name</param>
        Public Sub New(ByVal Name As String, ByVal RepoName As String)
            UserName = Name
            Repository = RepoName
        End Sub
    End Class
End Namespace