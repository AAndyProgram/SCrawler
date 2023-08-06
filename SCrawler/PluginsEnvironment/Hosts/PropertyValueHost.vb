' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Reflection
Imports SCrawler.Plugin.Attributes
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Functions.XML.Objects
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Namespace Plugin.Hosts
    Friend Class PropertyValueHost : Implements IPropertyValue, IComparable(Of PropertyValueHost)
#Region "Events"
        Private Event ValueChanged As IPropertyValue.ValueChangedEventHandler Implements IPropertyValue.ValueChanged
#End Region
#Region "Declarations"
        Private ReadOnly Keeper As SettingsHost
        Protected Source As Object 'ReadOnly
        Protected Member As MemberInfo
        Friend ReadOnly Options As PropertyOption
        Friend Overridable ReadOnly Property Name As String
        Protected _Type As Type
        Friend Overridable Property [Type] As Type Implements IPropertyValue.Type
            Get
                Return If(ExternalValue?.Type, _Type)
            End Get
            Set(ByVal t As Type)
                _Type = t
            End Set
        End Property
        Friend ReadOnly IsTaskCounter As Boolean
        Friend ReadOnly Exists As Boolean = False
#Region "XML"
        Private ReadOnly _XmlName As String
#End Region
#Region "Control"
        Friend Property Control As Control
        Protected ControlNumber As Integer = -1
        Friend ReadOnly Property ControlHeight As Integer
            Get
                If Not Control Is Nothing Then
                    Return IIf(TypeOf Control Is CheckBox, 25, 28)
                Else
                    Return 0
                End If
            End Get
        End Property
        Friend Const LeftOffsetDefault As Integer = 100
        Protected _LeftOffset As Integer? = Nothing
        Friend Overridable Property LeftOffset As Integer
            Get
                If _LeftOffset.HasValue Then
                    Return _LeftOffset
                Else
                    Return If(Options?.LeftOffset, LeftOffsetDefault)
                End If
            End Get
            Set(ByVal NewOffset As Integer)
                _LeftOffset = NewOffset
            End Set
        End Property
        Protected Overridable ReadOnly Property Control_ThreeStates As Boolean
            Get
                Return Options.ThreeStates
            End Get
        End Property
        Protected Overridable ReadOnly Property Control_ToolTip As String
            Get
                Return Options.ControlToolTip
            End Get
        End Property
        Protected Overridable ReadOnly Property Control_Caption As String
            Get
                Return Options.ControlText
            End Get
        End Property
        Protected Overridable ReadOnly Property Control_IsInformationLabel As Boolean
            Get
                Return Options.IsInformationLabel
            End Get
        End Property
        Protected Overridable ReadOnly Property Control_InfoLabelTextAlign As ContentAlignment
            Get
                Return Options.LabelTextAlign
            End Get
        End Property
        Friend Sub CreateControl(Optional ByVal TT As ToolTip = Nothing)
            If Control_IsInformationLabel Then
                Control = New Label
                With DirectCast(Control, Label)
                    .Padding = New PaddingE(Control.Padding) With {.Left = LeftOffset}
                    .Text = CStr(AConvert(Of String)(Value, String.Empty))
                    .TextAlign = Control_InfoLabelTextAlign
                End With
                If Not Control_ToolTip.IsEmptyString And Not TT Is Nothing Then TT.SetToolTip(Control, Control_ToolTip)
            Else
                If Type Is GetType(Boolean) Or Control_ThreeStates Then
                    Control = New CheckBox
                    If Not Control_ToolTip.IsEmptyString And Not TT Is Nothing Then TT.SetToolTip(Control, Control_ToolTip)
                    DirectCast(Control, CheckBox).ThreeState = Control_ThreeStates
                    DirectCast(Control, CheckBox).Text = Control_Caption
                    If Control_ThreeStates Then
                        DirectCast(Control, CheckBox).CheckState = CInt(AConvert(Of Integer)(Value, CInt(CheckState.Indeterminate)))
                    Else
                        DirectCast(Control, CheckBox).Checked = CBool(AConvert(Of Boolean)(Value, False))
                    End If
                    Control.Padding = New PaddingE(Control.Padding) With {.Left = LeftOffset}
                Else
                    Control = New TextBoxExtended
                    With DirectCast(Control, TextBoxExtended)
                        .CaptionText = Control_Caption
                        .CaptionToolTipEnabled = Not Control_ToolTip.IsEmptyString
                        If LeftOffset > 0 Then
                            .CaptionWidth = LeftOffset
                        Else
                            Using l As New Label : .CaptionWidth = .CaptionText.MeasureTextDefault(l.Font).Width : End Using
                        End If
                        If Not Control_ToolTip.IsEmptyString Then .CaptionToolTipText = Control_ToolTip : .CaptionToolTipEnabled = True
                        .Text = CStr(AConvert(Of String)(Value, String.Empty))
                        With .Buttons
                            .BeginInit()
                            If Not Source Is Nothing And Not UpdateMethod Is Nothing Then .Add(New ActionButton(ADB.Refresh))
                            .Add(ADB.Clear)
                            .EndInit(True)
                        End With
                        AddHandler .ActionOnButtonClick, AddressOf TextBoxClick
                        If Not ProviderValue Is Nothing AndAlso ProviderValueInteraction Then AddHandler .ActionOnTextChanged, AddressOf TextBoxTextChanged
                    End With
                End If
            End If
            Control.Tag = Name
            Control.Dock = DockStyle.Fill
        End Sub
        Friend Sub DisposeControl()
            If Not Control Is Nothing Then Control.Dispose() : Control = Nothing
        End Sub
        Private Sub TextBoxClick(ByVal Sender As ActionButton, ByVal e As EventArgs)
            Try
                If Sender.DefaultButton = ADB.Refresh AndAlso Not Source Is Nothing AndAlso Not UpdateMethod Is Nothing Then
                    Dim args As Object = Nothing
                    Dim i%
                    If UpdateMethodArguments.ListExists Then
                        Dim a As New List(Of String)
                        For Each arg$ In UpdateMethodArguments
                            i = Keeper.PropList.FindIndex(Function(p) Not p Is Me And p.Name = arg)
                            If i >= 0 Then a.Add(AConvert(Of String)(Keeper.PropList(i).GetControlValue(), String.Empty)) Else a.Add(String.Empty)
                        Next
                        If a.Count > 0 Then args = a.ToArray
                    End If
                    If CBool(UpdateMethod.Invoke(Source, args)) Then
                        DirectCast(Control, TextBoxExtended).Text = CStr(AConvert(Of String)(Value, String.Empty))
                        If Dependents.Count > 0 Then Dependents.ForEach(Sub(d) d.UpdateDependence())
                    End If
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, $"Updating [{Name}] property")
            End Try
        End Sub
        Private Sub TextBoxTextChanged(ByVal Sender As Object, ByVal e As EventArgs)
            UpdateProviderPropertyName()
            With DirectCast(Sender, TextBoxExtended)
                If ProviderValueIsPropertyProvider Then DirectCast(ProviderValue, IPropertyProvider).PropertyName = .Tag
                Dim s% = .SelectionStart
                Dim t$ = AConvert(Of String)(.Text, ProviderValue, String.Empty)
                If Not t = .Text Then .Text = t : .Select(s, 0)
            End With
        End Sub
        Friend Sub UpdateValueByControl()
            If Not Control Is Nothing AndAlso Not TypeOf Control Is Label Then
                If TypeOf Control Is CheckBox Then
                    With DirectCast(Control, CheckBox)
                        If Control_ThreeStates Then Value = CInt(.CheckState) Else Value = .Checked
                    End With
                Else
                    UpdateProviderPropertyName()
                    Value = AConvert(DirectCast(Control, TextBoxExtended).Text, AModes.Var, [Type],,,, ProviderValue)
                End If
            End If
        End Sub
        Friend Function GetControlValue() As Object
            If Not Control Is Nothing Then
                If TypeOf Control Is CheckBox Then
                    With DirectCast(Control, CheckBox)
                        If Control_ThreeStates Then Return CInt(.CheckState) Else Return .Checked
                    End With
                Else
                    UpdateProviderPropertyName()
                    Return AConvert(DirectCast(Control, TextBoxExtended).Text, AModes.Var, [Type],,,, ProviderValue)
                End If
            Else
                Return Nothing
            End If
        End Function
#End Region
#Region "Providers"
        Friend Property ProviderFieldsChecker As IFormatProvider
        Private Property ProviderValue As IFormatProvider
        Private Property ProviderValueInteraction As Boolean = False
        Private Property ProviderValueIsPropertyProvider As Boolean = False
        Friend Sub SetProvider(ByVal Provider As IFormatProvider, ByVal Instance As Provider)
            If Instance.FieldsChecker Then
                ProviderFieldsChecker = Provider
            Else
                ProviderValue = Provider
                ProviderValueIsPropertyProvider = TypeOf ProviderValue Is IPropertyProvider
                ProviderValueInteraction = Instance.Interaction
            End If
        End Sub
        Private Sub UpdateProviderPropertyName()
            If ProviderValueIsPropertyProvider Then DirectCast(ProviderValue, IPropertyProvider).PropertyName = Name
        End Sub
#End Region
#Region "Updaters, Checkers"
        Friend PropertiesChecking As String()
        Friend PropertiesCheckingMethod As MethodInfo
        Private UpdateMethod As MethodInfo
        Private UpdateMethodArguments As String()
        Friend Sub SetUpdateMethod(ByVal m As MethodInfo, ByVal _UpdateMethodArguments As String())
            UpdateMethod = m
            UpdateMethodArguments = _UpdateMethodArguments
        End Sub
#End Region
#Region "Dependents"
        Private ReadOnly DependentNames As New List(Of String)
        Private ReadOnly Dependents As New List(Of PropertyValueHost)
        Private Sub UpdateDependence()
            If TypeOf Control Is CheckBox Then
                DirectCast(Control, CheckBox).Checked = CBool(AConvert(Of Boolean)(Value, False))
            Else
                DirectCast(Control, TextBoxExtended).Text = CStr(AConvert(Of String)(Value, String.Empty))
            End If
        End Sub
#End Region
#End Region
#Region "Initializer"
        Protected Sub New()
        End Sub
        Friend Sub New(ByRef Keeper As SettingsHost, ByRef PropertySource As Object, ByVal Member As MemberInfo)
            Me.Keeper = Keeper
            Source = PropertySource
            Name = Member.Name

            ControlNumber = If(Member.GetCustomAttribute(Of ControlNumber)()?.PropertyNumber, -1)

            If DirectCast(Member, PropertyInfo).PropertyType Is GetType(PropertyValue) Then
                ExternalValue = DirectCast(DirectCast(Member, PropertyInfo).GetValue(Source), PropertyValue)
                _Value = ExternalValue.Value
                AddHandler ExternalValue.ValueChanged, AddressOf ExternalValueChanged
                Options = Member.GetCustomAttribute(Of PropertyOption)()
                IsTaskCounter = Not Member.GetCustomAttribute(Of TaskCounter)() Is Nothing
                _XmlName = If(Member.GetCustomAttribute(Of PXML)()?.ElementName, String.Empty)
                If Not _XmlName.IsEmptyString Then XValue = CreateXMLValueInstance([Type], True)
                DependentNames.ListAddList(Member.GetCustomAttribute(Of DependentFields)?.Fields, LAP.NotContainsOnly)
                Exists = True
            End If
        End Sub
        Friend Sub SetXmlEnvironment(ByRef Container As Object, Optional ByVal _Nodes() As String = Nothing,
                                     Optional ByVal FormatProvider As IFormatProvider = Nothing)
            If Not _XmlName.IsEmptyString And Not XValue Is Nothing Then
                XValue.SetEnvironment(_XmlName, _Value, Container, _Nodes, If(ProviderValue, FormatProvider))
                Value(False) = XValue.Value
            End If
        End Sub
        Friend Sub SetDependents(ByVal Props As List(Of PropertyValueHost))
            If DependentNames.Count > 0 And Props.Count > 0 Then
                For Each prop As PropertyValueHost In Props
                    If DependentNames.Contains(prop.Name) Then Dependents.Add(prop)
                Next
            End If
        End Sub
#End Region
#Region "Value"
        Protected ReadOnly Property ExternalValue As PropertyValue
        Friend ReadOnly Property XValue As IXMLValue
        Protected _Value As Object
        Friend Overloads Property Value As Object Implements IPropertyValue.Value
            Get
                Return _Value
            End Get
            Set(ByVal NewValue As Object)
                Value(True) = NewValue
            End Set
        End Property
        Private Overloads WriteOnly Property Value(ByVal UpdateXML As Boolean) As Object
            Set(ByVal NewValue As Object)
                _Value = NewValue
                If Not ExternalValue Is Nothing And Not _ExternalInvoked Then ExternalValue.Value = _Value
                If ExternalValue Is Nothing And Not Member Is Nothing Then Member.SetMemberValue(Source, NewValue)
                If UpdateXML And Not XValue Is Nothing Then XValue.Value = NewValue
                RaiseEvent ValueChanged(_Value)
            End Set
        End Property
        Private _ExternalInvoked As Boolean = False
        Private Sub ExternalValueChanged(ByVal NewValue As Object)
            If Not _ExternalInvoked Then
                _ExternalInvoked = True
                Value = NewValue
                _ExternalInvoked = False
            End If
        End Sub
#End Region
#Region "IComparable Support"
        Private Function CompareTo(ByVal Other As PropertyValueHost) As Integer Implements IComparable(Of PropertyValueHost).CompareTo
            Return ControlNumber.CompareTo(Other.ControlNumber)
        End Function
#End Region
    End Class
End Namespace