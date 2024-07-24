' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Drawing.Design
Imports System.ComponentModel
Imports PersonalUtilities.Tools.Grid.Attributes
Imports PersonalUtilities.Tools.Grid.EnumObjects
Namespace API.YouTube.Base
    Public Structure Thumbnail : Implements IIndexable, IComparable(Of Thumbnail)
        Public ID As String
        Public Width As Integer
        Public Height As Integer
        Public URL As String
        Public Property Index As Integer Implements IIndexable.Index
        Private Function SetIndex(ByVal Obj As Object, ByVal Index As Integer) As Object Implements IIndexable.SetIndex
            Dim t As Thumbnail = Obj
            t.Index = Index
            Return t
        End Function
        Private Function CompareTo(ByVal Other As Thumbnail) As Integer Implements IComparable(Of Thumbnail).CompareTo
            Return Width.CompareTo(Other.Width) * -1
        End Function
    End Structure
    Public Structure Subtitles : Implements IIndexable, IComparable(Of Subtitles)
        Public ID As String
        Private _Name As String
        Public Property Name As String
            Get
                Dim n$ = _Name.IfNullOrEmpty(ID)
                If CC Then n &= " (CC)"
                Return n
            End Get
            Set(ByVal NewName As String)
                _Name = NewName
            End Set
        End Property
        Public Formats As String
        Public CC As Boolean
        Public ReadOnly Property FullID As String
            Get
                Return IIf(ID = "en", "en.*", ID)
            End Get
        End Property
        Public Property Index As Integer Implements IIndexable.Index
        Private Function SetIndex(ByVal Obj As Object, ByVal Index As Integer) As Object Implements IIndexable.SetIndex
            Dim s As Subtitles = Obj
            s.Index = Index
            Return s
        End Function
        Private Function CompareTo(ByVal Other As Subtitles) As Integer Implements IComparable(Of Subtitles).CompareTo
            Return Name.CompareTo(Other.Name)
        End Function
    End Structure
    Public Enum YouTubeMediaType As Integer
        Undefined = 0
        [Single] = 1
        Channel = 2
        PlayList = 3
    End Enum
    <Editor(GetType(EnumDropDownEditor), GetType(UITypeEditor)), Flags>
    Public Enum YouTubeChannelTab As Integer
        <EnumValue(IsNullValue:=True)>
        All = 0
        Videos = 1
        Shorts = 3
        Playlists = 10
    End Enum
    <Editor(GetType(EnumDropDownEditor), GetType(UITypeEditor))>
    Public Enum Protocols As Integer
        <EnumValue(ExcludeFromList:=True)>
        Undefined = -1
        Any = 0
        https = 1
        m3u8 = 2
    End Enum
    <Editor(GetType(EnumDropDownEditor), GetType(UITypeEditor))>
    Public Enum FileDateMode As Integer
        None = 0
        Before = 1
        After = 2
    End Enum
    Public Enum M3U8CreationMode As Integer
        Relative = 0
        Absolute = 1
        Both = 2
    End Enum
    Public Structure MediaObject : Implements IIndexable, IComparable(Of MediaObject)
        Public Type As Plugin.UserMediaTypes
        Public ID As String
        Public ID_DRC As Boolean
        Public Extension As String
        Public Width As Integer
        Public Height As Integer
        Public FPS As Integer
        Public Bitrate As Integer
        ''' <summary>Kb</summary>
        Public Size As Double
        Public Codec As String
        Public Protocol As String
        Public ReadOnly Property ProtocolType As Protocols
            Get
                If Not Protocol.IsEmptyString Then
                    Select Case Protocol.StringToLower.StringTrim
                        Case "http", "https" : Return Protocols.https
                        Case "m3u8" : Return Protocols.m3u8
                    End Select
                End If
                Return Protocols.Undefined
            End Get
        End Property
        Public URL As String
        Public Property Index As Integer Implements IIndexable.Index
        Private Function SetIndex(ByVal Obj As Object, ByVal Index As Integer) As Object Implements IIndexable.SetIndex
            Dim m As MediaObject = Obj
            m.Index = Index
            Return m
        End Function
        Private Function CompareTo(ByVal Other As MediaObject) As Integer Implements IComparable(Of MediaObject).CompareTo
            If Type = Other.Type Then
                If ID_DRC.CompareTo(Other.ID_DRC) = 0 Then
                    If Width.CompareTo(Other.Width) = 0 Then
                        Return Size.CompareTo(Other.Size) * -1
                    Else
                        Return Width.CompareTo(Other.Width) * -1
                    End If
                Else
                    Return ID_DRC.CompareTo(Other.ID_DRC)
                End If
            Else
                Return CInt(Type).CompareTo(CInt(Other.Type))
            End If
        End Function
    End Structure
End Namespace