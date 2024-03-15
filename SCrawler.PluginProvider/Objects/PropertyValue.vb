' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace Plugin
    Public NotInheritable Class PropertyValue : Implements IPropertyValue
        Public Event ValueChanged As IPropertyValue.ValueChangedEventHandler Implements IPropertyValue.ValueChanged
        Public Event CheckedChanged As IPropertyValue.ValueChangedEventHandler
        Public Property [Type] As Type Implements IPropertyValue.Type
        Public Property OnChangeFunction As IPropertyValue.ValueChangedEventHandler
        Public Property OnCheckboxCheckedChange As EventHandler(Of PropertyValueEventArgs)
        Private _Checked As Boolean = False
        Public Property Checked As Boolean
            Get
                Return _Checked
            End Get
            Set(ByVal IsChecked As Boolean)
                _Checked = IsChecked
                If Not _Initialization Then
                    If Not OnCheckboxCheckedChange Is Nothing Then OnCheckboxCheckedChange.Invoke(Me, EventArgs.Empty)
                    RaiseEvent CheckedChanged(_Checked)
                End If
            End Set
        End Property
        Private _Initialization As Boolean = False
        ''' <inheritdoc cref="PropertyValue.New(Object, Type, ByRef IPropertyValue.ValueChangedEventHandler)"/>
        ''' <exception cref="ArgumentNullException"></exception>
        Public Sub New(ByVal InitValue As Object)
            _Value = InitValue
            If IsNothing(InitValue) Then
                Throw New ArgumentNullException("InitValue", "InitValue cannot be null")
            Else
                [Type] = _Value.GetType
            End If
        End Sub
        ''' <inheritdoc cref="PropertyValue.New(Object, Type, ByRef IPropertyValue.ValueChangedEventHandler)"/>
        Public Sub New(ByVal InitValue As Object, ByVal T As Type)
            _Value = InitValue
            [Type] = T
        End Sub
        ''' <summary>New property value instance</summary>
        ''' <param name="InitValue">Initialization value</param>
        ''' <param name="T">Value type</param>
        ''' <param name="RFunction">CallBack function on value change</param>
        Public Sub New(ByVal InitValue As Object, ByVal T As Type, ByRef RFunction As IPropertyValue.ValueChangedEventHandler)
            Me.New(InitValue, T)
            OnChangeFunction = RFunction
        End Sub
        Private _Value As Object
        Public Property Value As Object Implements IPropertyValue.Value
            Get
                Return _Value
            End Get
            Set(ByVal NewValue As Object)
                _Value = NewValue
                If Not _Initialization Then
                    If Not OnChangeFunction Is Nothing Then OnChangeFunction.Invoke(Value)
                    RaiseEvent ValueChanged(_Value)
                End If
            End Set
        End Property
        Public Sub BeginInit()
            _Initialization = True
        End Sub
        Public Sub EndInit()
            _Initialization = False
        End Sub
        Public Sub Clone(ByVal Source As PropertyValue)
            _Initialization = True
            Type = Source.Type
            OnChangeFunction = Source.OnChangeFunction
            _Value = Source._Value
            _Checked = Source._Checked
            _Initialization = False
        End Sub
    End Class
    Public Interface IPropertyValue
        ''' <summary>Event for internal exchange</summary>
        ''' <param name="Value">New value</param>
        Event ValueChanged(ByVal Value As Object)
        ''' <summary>Value type</summary>
        Property [Type] As Type
        ''' <summary>Property value</summary>
        Property Value As Object
    End Interface
    Public Class PropertyValueEventArgs : Inherits EventArgs
        Public Property Checked As Boolean = False
        Public Property ControlEnabled As Boolean = True
    End Class
End Namespace