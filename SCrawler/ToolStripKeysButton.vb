' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports PersonalUtilities.Forms.Controls.KeyClick
Public Class ToolStripKeyMenuItem : Inherits ToolStripMenuItemKeyClick
    Public Sub New()
        MyEventArgs = New MyKeyEventArgs
    End Sub
    Public Sub New(ByVal Text As String, ByVal Image As Image)
        MyBase.New(Text, Image)
        MyEventArgs = New MyKeyEventArgs
    End Sub
    Friend Const FeedToolTipText As String = "Click: download, include in the feed." & vbCr & "Ctrl+Click: download, exclude from feed."
    Private _AddFeedText As Boolean = True
    <Category("Behavior"), DefaultValue(True)> Public Property AddFeedText As Boolean
        Get
            Return _AddFeedText
        End Get
        Set(ByVal _AddFeedText As Boolean)
            Me._AddFeedText = _AddFeedText
            ToolTipText = _ToolTipTextOriginal
        End Set
    End Property
    Protected _ToolTipTextOriginal As String = String.Empty
    Public Overloads Property ToolTipText As String
        Get
            Return _ToolTipTextOriginal
        End Get
        Set(ByVal t As String)
            _ToolTipTextOriginal = t
            If AddFeedText Then
                If _ToolTipTextOriginal.IsEmptyString Then
                    MyBase.ToolTipText = FeedToolTipText
                Else
                    MyBase.ToolTipText = $"{_ToolTipTextOriginal}{vbCr}{FeedToolTipText}"
                End If
            Else
                MyBase.ToolTipText = _ToolTipTextOriginal
            End If
        End Set
    End Property
End Class
Public Class MyKeyEventArgs : Inherits KeyClickEventArgs
    Public Property IncludeInTheFeed As Boolean
        Get
            Return Not _Control
        End Get
        Set(ByVal Included As Boolean)
            _Control = Not Included
        End Set
    End Property
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal e As KeyEventArgs)
        MyBase.New(e)
    End Sub
End Class