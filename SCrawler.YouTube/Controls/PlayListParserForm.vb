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
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.YouTube.Controls
    Friend Class PlayListParserForm : Implements IDesignXMLContainer
        Private WithEvents MyDefs As DefaultFormOptions
        Friend Property DesignXML As EContainer Implements IDesignXMLContainer.DesignXML
        Private Property DesignXMLNodes As String() Implements IDesignXMLContainer.DesignXMLNodes
        Private Property DesignXMLNodeName As String Implements IDesignXMLContainer.DesignXMLNodeName
        Friend ReadOnly Property PlayLists As List(Of String)
            Get
                If Not TXT_OUT.Text.IsEmptyString Then
                    Return TXT_OUT.Lines.ToList
                Else
                    Return New List(Of String)
                End If
            End Get
        End Property
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me)
        End Sub
        Private Sub MyForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize()
                .AddOkCancelToolbar()
                .EndLoaderOperations()
            End With
        End Sub
        Private Sub UpdatePlaylists(sender As Object, e As EventArgs) Handles TXT_IN.TextChanged, TXT_LIMIT.ActionOnTextChanged
            Try
                If Not TXT_IN.Text.IsEmptyString Then
                    Dim l As List(Of String) = ListAddList(Of String)(Nothing, RegexReplace(TXT_IN.Text, RParams.DMS("playlistId"": ""([^""]+)""", 1,
                                                                                                                     RegexReturn.List, EDP.ReturnValue)),
                                                                      LAP.NotContainsOnly, EDP.ReturnValue)
                    If Not TXT_LIMIT.Text.IsEmptyString And l.ListExists Then l.RemoveAll(Function(id) id.StringToLower.StartsWith(TXT_LIMIT.Text.ToLower))
                    If l.ListExists Then
                        TXT_OUT.Text = l.Select(Function(id) $"https://music.youtube.com/playlist?list={id}").ListToString(vbNewLine, EDP.ReturnValue)
                    Else
                        TXT_OUT.Text = String.Empty
                    End If
                Else
                    TXT_OUT.Text = String.Empty
                End If
            Catch ex As Exception
                TXT_OUT.Text = String.Empty
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "[PlayListParserForm.UpdatePlaylists]")
            End Try
        End Sub
    End Class
End Namespace