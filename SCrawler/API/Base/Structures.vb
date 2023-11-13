' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.Plugin
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Functions.RegularExpressions
Namespace API.Base
    Friend Module Structures
        Friend Enum SiteModes As Integer
            User = 0
            Search = 1
            Tags = 2
            Categories = 3
            Pornstars = 4
            Playlists = 5
        End Enum
        Friend Structure UserMedia : Implements IUserMedia, IEquatable(Of UserMedia), IEContainerProvider
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
                Picture = 1
                Video = 2
                Audio = 200
                Text = 4
                VideoPre = 10
                AudioPre = 215
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
            Friend [Object] As Object
#Region "Interface Support"
            Private Property IUserMedia_Type As UserMediaTypes Implements IUserMedia.ContentType
                Get
                    Return CInt(Type)
                End Get
                Set(ByVal Type As UserMediaTypes)
                    Me.Type = CInt(Type)
                End Set
            End Property
            Private Property IUserMedia_URL_BASE As String Implements IUserMedia.URL_BASE
                Get
                    Return URL_BASE
                End Get
                Set(ByVal URL_BASE As String)
                    Me.URL_BASE = URL_BASE
                End Set
            End Property
            Private Property IUserMedia_URL As String Implements IUserMedia.URL
                Get
                    Return URL
                End Get
                Set(ByVal URL As String)
                    Me.URL = URL
                End Set
            End Property
            Private Property IUserMedia_MD5 As String Implements IUserMedia.MD5
                Get
                    Return MD5
                End Get
                Set(ByVal MD5 As String)
                    Me.MD5 = MD5
                End Set
            End Property
            Private Property IUserMedia_File As String Implements IUserMedia.File
                Get
                    Return File
                End Get
                Set(ByVal File As String)
                    Me.File = File
                End Set
            End Property
            Private Property IUserMedia_State As UserMediaStates Implements IUserMedia.DownloadState
                Get
                    Return CInt(State)
                End Get
                Set(ByVal State As UserMediaStates)
                    Me.State = CInt(State)
                End Set
            End Property
            Private Property IUserMedia_PostID As String Implements IUserMedia.PostID
                Get
                    Return Post.ID
                End Get
                Set(ByVal PostID As String)
                    Post = New UserPost(PostID, Post.Date)
                End Set
            End Property
            Private Property IUserMedia_PostDate As Date? Implements IUserMedia.PostDate
                Get
                    Return Post.Date
                End Get
                Set(ByVal PostDate As Date?)
                    Post = New UserPost(Post.ID, PostDate)
                End Set
            End Property
            Private Property IUserMedia_SpecialFolder As String Implements IUserMedia.SpecialFolder
                Get
                    Return SpecialFolder
                End Get
                Set(ByVal SpecialFolder As String)
                    Me.SpecialFolder = SpecialFolder
                End Set
            End Property
            Private Property IUserMedia_Attempts As Integer Implements IUserMedia.Attempts
                Get
                    Return Attempts
                End Get
                Set(ByVal Attempts As Integer)
                    Me.Attempts = Attempts
                End Set
            End Property
            Private Property IUserMedia_Object As Object Implements IUserMedia.Object
                Get
                    Return Me.Object
                End Get
                Set(ByVal Obj As Object)
                    Me.Object = Obj
                End Set
            End Property
#End Region
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
            Friend Sub New(ByVal m As IUserMedia)
                [Type] = m.ContentType
                URL = m.URL
                URL_BASE = m.URL_BASE
                MD5 = m.MD5
                File = m.File
                Post = New UserPost With {.ID = m.PostID, .[Date] = m.PostDate}
                State = m.DownloadState
                SpecialFolder = m.SpecialFolder
                Attempts = m.Attempts
                Me.Object = m.Object
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

                'TODO: UserMedia.SpecialFolder
                SpecialFolder = e.Attribute(Name_SpecialFolder).Value
                If Not SpecialFolder.IsEmptyString Then upath &= $"{SpecialFolder}\"
                If vp.HasValue AndAlso vp.Value Then upath &= $"Video\"
                If Not upath.IsEmptyString Then File = $"{upath.CSFilePS}{File.File}"

                Post = New UserPost With {
                    .ID = e.Attribute(Name_MediaPostID).Value,
                    .[Date] = AConvert(Of Date)(e.Attribute(Name_MediaPostDate).Value, DateTimeDefaultProvider, Nothing)
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
                                                                 New EAttribute(Name_MediaPostDate, AConvert(Of String)(Post.Date, DateTimeDefaultProvider, String.Empty))
                                                                }
                                     )
            End Function
        End Structure
        Friend Structure UserPost : Implements IEquatable(Of UserPost), IComparable(Of UserPost)
            ''' <summary>Post ID</summary>
            Friend ID As String
            Friend [Date] As Date?
            Friend UserID As String
            Friend CachedFile As SFile
#Region "Initializers"
            Public Sub New(ByVal ID As String)
                Me.ID = ID
            End Sub
            Public Sub New(ByVal [Date] As Date?)
                Me.Date = [Date]
            End Sub
            Public Sub New(ByVal ID As String, ByVal [Date] As Date?)
                Me.ID = ID
                Me.Date = [Date]
            End Sub
            Public Shared Widening Operator CType(ByVal ID As String) As UserPost
                Return New UserPost(ID)
            End Operator
            Public Shared Widening Operator CType(ByVal Post As UserPost) As String
                Return Post.ID
            End Operator
#End Region
#Region "ToString"
            Public Overrides Function ToString() As String
                Return ID
            End Function
#End Region
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