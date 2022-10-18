' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Base
    Friend Module Structures
        Friend Structure UserMedia : Implements IEquatable(Of UserMedia), IEContainerProvider
#Region "XML Names"
            Friend Const Name_MediaNode As String = "MediaData"
            Private Const Name_MediaType As String = "Type"
            Private Const Name_MediaState As String = "State"
            Private Const Name_MediaAttempts As String = "Attempts"
            Private Const Name_MediaURL As String = "URL"
            Private Const Name_MediaHash As String = "Hash"
            Private Const Name_MediaFile As String = "File"
            Private Const Name_MediaPostID As String = "ID"
            Private Const Name_MediaPostDate As String = "Date"
            Private Const Name_SpecialFolder As String = "SpecialFolder"
#End Region
            Friend Enum Types As Integer
                Undefined = 0
                [Picture] = 1
                [Video] = 2
                [Text] = 3
                VideoPre = 10
                GIF = 50
                m3u8 = 100
            End Enum
            Friend Enum States As Integer : Unknown = 0 : Tried = 1 : Downloaded = 2 : Skipped = 3 : Missing = 4 : End Enum
            Friend [Type] As Types
            Friend URL_BASE As String
            Friend URL As String
            Friend MD5 As String
            Friend [File] As SFile
            Friend Post As UserPost
            Friend PictureOption As String
            Friend State As States
            Friend Attempts As Integer
            ''' <summary>
            ''' SomeFolder<br/>
            ''' SomeFolder\SomeFolder2
            ''' </summary>
            Friend SpecialFolder As String
            Friend Sub New(ByVal URL As String)
                Me.URL = URL
                URL_BASE = URL
                File = URL
                Type = Types.Undefined
            End Sub
            Friend Sub New(ByVal URL As String, ByVal Type As Types)
                Me.New(URL)
                Me.Type = Type
            End Sub
            Friend Sub New(ByVal m As Plugin.PluginUserMedia)
                [Type] = m.ContentType
                URL = m.URL
                URL_BASE = URL
                MD5 = m.MD5
                File = m.File
                Post = New UserPost With {.ID = m.PostID, .[Date] = m.PostDate}
                State = m.DownloadState
                SpecialFolder = m.SpecialFolder
                Attempts = m.Attempts
            End Sub
            Friend Sub New(ByVal e As EContainer, ByVal UserInstance As IUserData)
                Type = e.Attribute(Name_MediaType).Value.FromXML(Of Integer)(CInt(Types.Undefined))
                State = e.Attribute(Name_MediaState).Value.FromXML(Of Integer)(CInt(States.Downloaded))
                Attempts = e.Attribute(Name_MediaAttempts).Value.FromXML(Of Integer)(0)
                URL = e.Attribute(Name_MediaURL).Value
                URL_BASE = e.Value
                MD5 = e.Attribute(Name_MediaHash).Value
                File = e.Attribute(Name_MediaFile).Value

                Dim vp As Boolean? = Nothing
                Dim upath$ = String.Empty
                If Not UserInstance Is Nothing Then
                    With DirectCast(UserInstance, UserDataBase)
                        vp = .SeparateVideoFolder
                        upath = .MyFile.CutPath.PathWithSeparator
                    End With
                End If

                SpecialFolder = e.Attribute(Name_SpecialFolder).Value
                If Not SpecialFolder.IsEmptyString Then upath &= $"{SpecialFolder}\"
                If vp.HasValue AndAlso vp.Value Then upath &= $"Video\"
                If Not upath.IsEmptyString Then File = $"{upath.CSFilePS}{File.File}"

                Post = New UserPost With {
                    .ID = e.Attribute(Name_MediaPostID).Value,
                    .[Date] = AConvert(Of Date)(e.Attribute(Name_MediaPostDate).Value, ParsersDataDateProvider, Nothing)
                }
            End Sub
            Public Shared Widening Operator CType(ByVal _URL As String) As UserMedia
                Return New UserMedia(_URL)
            End Operator
            Public Shared Widening Operator CType(ByVal m As UserMedia) As String
                Return m.URL
            End Operator
            Public Overrides Function GetHashCode() As Integer
                If Not File.IsEmptyString Then
                    Return File.GetHashCode
                ElseIf Not URL_BASE.IsEmptyString Then
                    Return URL_BASE.GetHashCode
                ElseIf Not URL.IsEmptyString Then
                    Return URL.GetHashCode
                Else
                    Return 0
                End If
            End Function
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
                    .PostDate = Post.Date,
                    .Attempts = Attempts
                }
            End Function
            Friend Overloads Function Equals(ByVal Other As UserMedia) As Boolean Implements IEquatable(Of UserMedia).Equals
                Return URL = Other.URL
            End Function
            Public Overrides Function Equals(ByVal Obj As Object) As Boolean
                Return Equals(CType(Obj, UserMedia))
            End Function
            Friend Function ToEContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
                Return New EContainer(Name_MediaNode, URL_BASE, {New EAttribute(Name_MediaType, CInt(Type)),
                                                                 New EAttribute(Name_MediaState, CInt(State)),
                                                                 New EAttribute(Name_MediaAttempts, Attempts),
                                                                 New EAttribute(Name_MediaURL, URL),
                                                                 New EAttribute(Name_MediaHash, MD5),
                                                                 New EAttribute(Name_MediaFile, File.File),
                                                                 New EAttribute(Name_SpecialFolder, SpecialFolder),
                                                                 New EAttribute(Name_MediaPostID, Post.ID),
                                                                 New EAttribute(Name_MediaPostDate, AConvert(Of String)(Post.Date, ParsersDataDateProvider, String.Empty))
                                                                }
                                     )
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
            Private Function GetCompareValue(ByVal Post As UserPost) As Long
                Dim v& = 0
                If Post.Date.HasValue Then v = Post.Date.Value.Ticks * -1
                Return v
            End Function
#End Region
        End Structure
        Friend Structure Sizes : Implements IRegExCreator, IComparable(Of Sizes)
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
            Private Function CreateFromArray(ByVal ParamsArray() As String) As Object Implements IRegExCreator.CreateFromArray
                If ParamsArray.ListExists(2) Then
                    Return New Sizes With {
                        .Value = AConvert(Of Integer)(ParamsArray(0), 0),
                        .Data = ParamsArray(1)
                    }
                Else
                    Return New Sizes
                End If
            End Function
            Friend Function CompareTo(ByVal Other As Sizes) As Integer Implements IComparable(Of Sizes).CompareTo
                Return Value.CompareTo(Other.Value) * -1
            End Function
        End Structure
    End Module
End Namespace