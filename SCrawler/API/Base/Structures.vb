' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.Base
    Friend Module Structures
        Friend Structure UserMedia : Implements IEquatable(Of UserMedia)
            Friend Enum Types As Integer
                Undefined = 0
                [Picture] = 1
                [Video] = 2
                [Text] = 3
                VideoPre = 10
                GIF = 50
                m3u8 = 100
            End Enum
            Friend Enum States As Integer : Unknown = 0 : Tried = 1 : Downloaded = 2 : Skipped = 3 : End Enum
            Friend [Type] As Types
            Friend URL_BASE As String
            Friend URL As String
            Friend MD5 As String
            Friend [File] As SFile
            Friend Post As UserPost
            Friend PictureOption As String
            Friend State As States
            ''' <summary>
            ''' SomeFolder<br/>
            ''' SomeFolder\SomeFolder2
            ''' </summary>
            Friend SpecialFolder As String
            Friend Sub New(ByVal _URL As String)
                URL = _URL
                URL_BASE = _URL
                File = URL
                Type = Types.Undefined
            End Sub
            Friend Sub New(ByVal _URL As String, ByVal _Type As Types)
                Me.New(_URL)
                [Type] = _Type
            End Sub
            Friend Sub New(ByVal m As Plugin.IPluginUserMedia)
                If Not IsNothing(m) Then
                    [Type] = m.ContentType
                    URL = m.URL
                    MD5 = m.MD5
                    File = m.File
                    Post = New UserPost With {.ID = m.PostID, .[Date] = m.PostDate}
                    State = m.DownloadState
                    SpecialFolder = m.SpecialFolder
                End If
            End Sub
            Public Shared Widening Operator CType(ByVal _URL As String) As UserMedia
                Return New UserMedia(_URL)
            End Operator
            Public Shared Widening Operator CType(ByVal m As UserMedia) As String
                Return m.URL
            End Operator
            Public Overrides Function ToString() As String
                Return URL
            End Function
            Friend Function PluginUserMedia() As Plugin.PluginUserMedia
                Return New Plugin.PluginUserMedia With {
                    .ContentType = Type,
                    .DownloadState = State,
                    .File = File,
                    .MD5 = MD5,
                    .URL = URL,
                    .SpecialFolder = SpecialFolder,
                    .PostID = Post.ID,
                    .PostDate = Post.Date
                }
            End Function
            Friend Overloads Function Equals(ByVal Other As UserMedia) As Boolean Implements IEquatable(Of UserMedia).Equals
                Return URL = Other.URL
            End Function
            Public Overrides Function Equals(ByVal Obj As Object) As Boolean
                Return Equals(CType(Obj, UserMedia))
            End Function
        End Structure
        Friend Structure UserPost : Implements IEquatable(Of UserPost), IComparable(Of UserPost)
            ''' <summary>Post ID</summary>
            Friend ID As String
            Friend [Date] As Date?
#Region "Channel compatible fields"
            Friend UserID As String
            Friend CachedFile As SFile
#End Region
            Friend Function GetImage(ByVal s As Size, ByVal e As ErrorsDescriber, ByVal NullArg As Image) As Image
                If Not CachedFile.IsEmptyString Then
                    Return If(PersonalUtilities.Tools.ImageRenderer.GetImage(SFile.GetBytes(CachedFile), s, e), NullArg.Clone)
                Else
                    Return NullArg.Clone
                End If
            End Function
#Region "IEquatable, IComparable Support"
            Friend Overloads Function Equals(ByVal Other As UserPost) As Boolean Implements IEquatable(Of UserPost).Equals
                Return ID = Other.ID
            End Function
            Public Overloads Overrides Function Equals(ByVal Obj As Object) As Boolean
                Return Equals(DirectCast(Obj, UserPost))
            End Function
            Friend Function CompareTo(ByVal Other As UserPost) As Integer Implements IComparable(Of UserPost).CompareTo
                Return GetCompareValue(Me).CompareTo(GetCompareValue(Other))
            End Function
#End Region
            Private Function GetCompareValue(ByVal Post As UserPost) As Long
                Dim v& = 0
                If Post.Date.HasValue Then v = Post.Date.Value.Ticks * -1
                Return v
            End Function
        End Structure
        Friend Structure Sizes : Implements IComparable(Of Sizes)
            Friend Value As Integer
            Friend Data As String
            Friend ReadOnly HasError As Boolean
            Friend Sub New(ByVal _Value As String, ByVal _Data As String)
                Try
                    Value = _Value
                    Data = _Data
                Catch ex As Exception
                    HasError = True
                End Try
            End Sub
            Friend Function CompareTo(ByVal Other As Sizes) As Integer Implements IComparable(Of Sizes).CompareTo
                Return Value.CompareTo(Other.Value) * -1
            End Function
        End Structure
    End Module
End Namespace