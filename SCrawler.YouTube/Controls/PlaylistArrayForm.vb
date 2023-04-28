' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Namespace API.YouTube.Controls
    Friend Class PlaylistArrayForm : Implements IDesignXMLContainer
        Private WithEvents MyDefs As DefaultFormOptions
        Friend Property DesignXML As EContainer Implements IDesignXMLContainer.DesignXML
        Private Property DesignXMLNodes As String() Implements IDesignXMLContainer.DesignXMLNodes
        Private Property DesignXMLNodeName As String Implements IDesignXMLContainer.DesignXMLNodeName
        Friend ReadOnly Property URLs As List(Of String)
            Get
                If Not TXT_URLS.Text.IsEmptyString Then
                    Return ListAddList(Nothing, TXT_URLS.Text.StringFormatLines.StringToList(Of String)(vbNewLine),
                                       LAP.NotContainsOnly, EDP.ReturnValue, CType(Function(Input$) Input.StringTrim, Func(Of Object, Object))).ListIfNothing
                Else
                    Return New List(Of String)
                End If
            End Get
        End Property
        Friend ReadOnly Property IsOneArtist As Boolean
            Get
                Return CH_PLS_ONE.Checked
            End Get
        End Property
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me)
        End Sub
        Private Sub PlaylistArrayForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize()
                .AddOkCancelToolbar()
                .EndLoaderOperations()
            End With
        End Sub
        Private Sub PlaylistArrayForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            If e.KeyCode = Keys.O And e.Control Then
                Using f As New PlayListParserForm With {.DesignXML = DesignXML}
                    f.ShowDialog()
                    If f.DialogResult = DialogResult.OK Then TXT_URLS.Text = f.PlayLists.ListToString(vbNewLine, EDP.ReturnValue)
                End Using
                e.Handled = True
            End If
        End Sub
    End Class
End Namespace