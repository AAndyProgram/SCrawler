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
            Friend Enum States : Unknown : Tried : Downloaded : Skipped : End Enum
            Friend Type As Types
            Friend URL_BASE As String
            Friend URL As String
            Friend MD5 As String
            Friend [File] As SFile
            Friend Post As UserPost
            Friend PictureOption As String
            Friend State As States
            Friend Sub New(ByVal _URL As String)
                URL = _URL
                URL_BASE = _URL
                File = URL
                Type = Types.Undefined
            End Sub
            Friend Sub New(ByVal _URL As String, ByVal _Type As Types)
                Me.New(_URL)
                Type = _Type
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
    End Module
End Namespace