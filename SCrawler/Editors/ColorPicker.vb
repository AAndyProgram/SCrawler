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
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Namespace Editors
    Public Class ColorPicker : Implements IChangeDetectorCompatible
        Private Event DataChanged As EventHandler Implements IChangeDetectorCompatible.DataChanged
        Private TT As ToolTip
        Public Sub New()
            InitializeComponent()
        End Sub
        Private Sub ColorPicker_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            TT.DisposeIfReady
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
                BTT_SELECT.Margin = m
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
        Private _TooltipText As String = String.Empty
        <Category("Appearance2"), DefaultValue("")>
        Public Property TooltipText As String
            Get
                Return _TooltipText
            End Get
            Set(ByVal NewText As String)
                _TooltipText = NewText
                If Not NewText.IsEmptyString Then
                    If TT Is Nothing Then TT = New ToolTip
                    TT.SetToolTip(LBL_CAPTION, _TooltipText)
                End If
            End Set
        End Property
        <Category("Appearance2"), DefaultValue(True)>
        Public Property ListButtonEnabled As Boolean
            Get
                Return BTT_SELECT.Enabled
            End Get
            Set(ByVal Enabled As Boolean)
                BTT_SELECT.Enabled = Enabled
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
        Friend Overloads Sub ColorsSet(ByVal b As XMLValue(Of Color), ByVal f As XMLValue(Of Color), ByVal bDefault As Color, ByVal fDefault As Color)
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
        Friend Overloads Sub ColorsSet(ByVal c As DataColor)
            BackColorImpl = c.BackColor
            ForeColorImpl = c.ForeColor
        End Sub
        Friend Sub ColorsSetUser(ByVal b As Color?, ByVal f As Color?)
            BackColorImpl = b
            ForeColorImpl = f
        End Sub
        Friend Overloads Sub ColorsGet(ByRef b As XMLValue(Of Color), ByRef f As XMLValue(Of Color))
            If BackColorImpl.HasValue Then b.Value = BackColorImpl.Value Else b.ValueF = Nothing
            If ForeColorImpl.HasValue Then f.Value = ForeColorImpl.Value Else f.ValueF = Nothing
        End Sub
        Friend Overloads Function ColorsGet() As DataColor
            Return New DataColor With {.BackColor = BackColorImpl, .ForeColor = ForeColorImpl}
        End Function
        Friend Sub ColorsGetUser(ByRef b As Color?, ByRef f As Color?)
            b = BackColorImpl
            f = ForeColorImpl
        End Sub
#End Region
#Region "Buttons handlers"
        Friend Sub RemoveAllButtons()
            With TP_MAIN
                With .Controls
                    .Remove(BTT_COLORS_BACK)
                    .Remove(BTT_COLORS_FORE)
                    .Remove(BTT_COLORS_CLEAR)
                    .Remove(BTT_SELECT)
                End With
                With .ColumnStyles
                    For i% = 2 To .Count - 1 : .Item(i).Width = 0 : Next
                End With
                .Refresh()
            End With
        End Sub
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
        Private Sub BTT_SELECT_Click(sender As Object, e As EventArgs) Handles BTT_SELECT.Click
            Try
                Using f As New SimpleListForm(Of DataColor)(Settings.Colors, Settings.Design) With {
                    .DesignXMLNodeName = "ColorsChooserForm",
                    .Buttons = {ADB.Add, ADB.Edit},
                    .AddFunction = Sub(ByVal __Sender As Object, ByVal ee As SimpleListFormEventArgs)
                                       Dim newColor As DataColor = Nothing
                                       Using ff As New ColorPickerInternalForm
                                           ff.ShowDialog()
                                           If ff.DialogResult = DialogResult.OK Then newColor = ff.ResultColor
                                       End Using
                                       If Settings.Colors.IndexOf(newColor) = -1 And newColor.Exists Then
                                           Settings.Colors.Add(newColor)
                                           ee.Item = newColor
                                           ee.Result = True
                                       Else
                                           ee.Result = False
                                       End If
                                   End Sub,
                    .FormText = "Colors",
                    .Mode = SimpleListFormModes.SelectedItems,
                    .MultiSelect = False
                }
                    AddHandler f.EditClick, Sub(ByVal __Sender As Object, ByVal ee As SimpleListFormEventArgs)
                                                If Not IsNothing(ee.Item) AndAlso TypeOf ee.Item Is DataColor Then
                                                    Using ff As New ColorPickerInternalForm With {.ResultColor = ee.Item} : ff.ShowDialog() : End Using
                                                End If
                                                ee.Result = False
                                            End Sub
                    Dim i% = Settings.Colors.IndexOf(New DataColor With {.BackColor = BackColorImpl, .ForeColor = ForeColorImpl})
                    If i >= 0 Then f.DataSelectedIndexes.Add(i)
                    If f.ShowDialog = DialogResult.OK Then
                        RaiseEvent DataChanged(Me, Nothing)
                        Dim resultColor As DataColor = f.DataResult.FirstOrDefault
                        BackColorImpl = resultColor.BackColor
                        ForeColorImpl = resultColor.ForeColor
                    End If
                End Using
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "[ColorPicker.SelectColor]")
            End Try
        End Sub
#End Region
    End Class
End Namespace