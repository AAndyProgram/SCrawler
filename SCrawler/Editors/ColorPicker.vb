' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Functions.XML.Objects
Namespace Editors
    Public Class ColorPicker : Implements IChangeDetectorCompatible
        Private Event DataChanged As EventHandler Implements IChangeDetectorCompatible.DataChanged
        Public Sub New()
            InitializeComponent()
        End Sub
#Region "Appearance"
        <Category("Appearance2"), DefaultValue(105)>
        Public Property CaptionWidth As Integer
            Get
                Return TP_MAIN.ColumnStyles(0).Width
            End Get
            Set(ByVal w As Integer)
                TP_MAIN.ColumnStyles(0).Width = w
            End Set
        End Property
        Private ReadOnly ButtonsMarginDefault As New Padding(2)
        <Category("Appearance2")>
        Public Property ButtonsMargin As Padding
            Get
                Return BTT_COLORS_CLEAR.Margin
            End Get
            Set(ByVal m As Padding)
                BTT_COLORS_BACK.Margin = m
                BTT_COLORS_FORE.Margin = m
                BTT_COLORS_CLEAR.Margin = m
            End Set
        End Property
        Private Function ShouldSerializeButtonsMargin() As Boolean
            Return Not ButtonsMargin.Equals(ButtonsMarginDefault)
        End Function
        Private Sub ResetButtonsMargin()
            ButtonsMargin = ButtonsMarginDefault
        End Sub
        <Category("Appearance2"), DefaultValue("")>
        Public Property CaptionText As String
            Get
                Return LBL_CAPTION.Text
            End Get
            Set(ByVal t As String)
                LBL_CAPTION.Text = t
            End Set
        End Property
#End Region
#Region "Colors"
        Private BackColorDefault As Color = DefaultBackColor
        Private _BackColorImpl As Color? = Nothing
        Private Property BackColorImpl As Color?
            Get
                Return _BackColorImpl
            End Get
            Set(ByVal c As Color?)
                _BackColorImpl = c
                If _BackColorImpl.HasValue Then LBL_COLORS.BackColor = _BackColorImpl.Value Else LBL_COLORS.BackColor = BackColorDefault
            End Set
        End Property
        Private ForeColorDefault As Color = DefaultForeColor
        Private _ForeColorImpl As Color? = Nothing
        Private Property ForeColorImpl As Color?
            Get
                Return _ForeColorImpl
            End Get
            Set(ByVal c As Color?)
                _ForeColorImpl = c
                If _ForeColorImpl.HasValue Then LBL_COLORS.ForeColor = _ForeColorImpl.Value Else LBL_COLORS.ForeColor = ForeColorDefault
            End Set
        End Property
#End Region
#Region "Get, Set"
        Friend Sub ColorsSet(ByVal b As XMLValue(Of Color), ByVal f As XMLValue(Of Color), ByVal bDefault As Color, ByVal fDefault As Color)
            BackColorDefault = bDefault
            If b.Exists Then
                BackColorImpl = b.Value
            Else
                BackColorImpl = Nothing
            End If
            ForeColorDefault = fDefault
            If f.Exists Then
                ForeColorImpl = f.Value
            Else
                ForeColorImpl = Nothing
            End If
        End Sub
        Friend Sub ColorsGet(ByRef b As XMLValue(Of Color), ByRef f As XMLValue(Of Color))
            If BackColorImpl.HasValue Then b.Value = BackColorImpl.Value Else b.ValueF = Nothing
            If ForeColorImpl.HasValue Then f.Value = ForeColorImpl.Value Else f.ValueF = Nothing
        End Sub
#End Region
#Region "Buttons handlers"
        Private Sub COLOR_BUTTONS_Click(ByVal Sender As Button, ByVal e As EventArgs) Handles BTT_COLORS_BACK.Click,
                                                                                              BTT_COLORS_FORE.Click,
                                                                                              BTT_COLORS_CLEAR.Click
            Select Case CStr(Sender.Tag)
                Case "F" : ForeColorImpl = AFontColor.SelectNewColor(ForeColorImpl, EDP.ReturnValue)
                Case "C" : BackColorImpl = AFontColor.SelectNewColor(BackColorImpl, EDP.ReturnValue)
                Case "D" : BackColorImpl = Nothing : ForeColorImpl = Nothing
            End Select
            RaiseEvent DataChanged(Me, Nothing)
        End Sub
#End Region
    End Class
End Namespace