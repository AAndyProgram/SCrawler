' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Tools.WEB
Imports PersonalUtilities.Tools.WebDocuments.JSON
Imports PersonalUtilities.Functions.XML
Imports System.Net
Imports System.Threading
Imports SCrawler.API.Base
Namespace API.Twitter
    Friend Class UserData : Inherits UserDataBase
#Region "Declarations"
        Friend Overrides Property Site As Sites = Sites.Twitter
#End Region
#Region "Initializer"
        Friend Sub New(ByVal u As UserInfo, Optional ByVal _LoadUserInformation As Boolean = True)
            User = u
            If _LoadUserInformation Then LoadUserInformation()
        End Sub
#End Region
#Region "Load and Update user info"
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
        End Sub
#End Region
#Region "Download functions"
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            DownloadData(String.Empty, Token)
        End Sub
        Private Overloads Sub DownloadData(ByVal POST As String, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                Dim PostID$ = String.Empty
                Dim PostDate$
                Dim m As EContainer, nn As EContainer, s As EContainer
                Dim NewPostDetected As Boolean = False
                Dim ExistsDetected As Boolean = False

                Dim PicNode As Predicate(Of EContainer) = Function(e) e.Count > 0 AndAlso e.Contains("media_url")
                Dim UID As Func(Of EContainer, String) = Function(e) e.XmlIfNothing.Item({"user", "id"}).XmlIfNothingValue

                URL = $"https://api.twitter.com/1.1/statuses/user_timeline.json?screen_name={Name}&count=200&exclude_replies=false&include_rts=1&tweet_mode=extended"
                If Not POST.IsEmptyString Then URL &= $"&max_id={POST}"

                ThrowAny(Token)
                Dim r$ = Responser.GetResponse(URL,, EDP.ThrowException)
                If Not r.IsEmptyString Then
                    Using w As EContainer = JsonDocument.Parse(r)
                        If Not w Is Nothing AndAlso w.Count > 0 Then
                            For Each nn In w
                                ThrowAny(Token)
                                If nn.Count > 0 Then
                                    PostID = nn.Value("id")
                                    If ID.IsEmptyString Then
                                        ID = UID(nn)
                                        If Not ID.IsEmptyString Then UpdateUserInformation()
                                    End If

                                    'Date Pattern:
                                    'Sat Jan 01 01:10:15 +0000 2000
                                    If nn.Contains("created_at") Then PostDate = nn("created_at").Value Else PostDate = String.Empty

                                    If Not _TempPostsList.Contains(PostID) Then
                                        NewPostDetected = True
                                        _TempPostsList.Add(PostID)
                                    Else
                                        ExistsDetected = True
                                        Continue For
                                    End If

                                    If Not ParseUserMediaOnly OrElse (Not nn.Contains("retweeted_status") OrElse
                                                                     (Not ID.IsEmptyString AndAlso UID(nn("retweeted_status")) = ID)) Then
                                        If Not CheckVideoNode(nn, PostID, PostDate) Then
                                            s = nn.ItemF({"extended_entities", "media"})
                                            If s Is Nothing OrElse s.Count = 0 Then s = nn.ItemF({"retweeted_status", "extended_entities", "media"})
                                            If Not s Is Nothing AndAlso s.Count > 0 Then
                                                For Each m In s
                                                    If m.Count > 0 AndAlso m.Contains("media_url") Then
                                                        _TempMediaList.ListAddValue(MediaFromData(m("media_url").Value,
                                                                                                  PostID, PostDate, GetPictureOption(m)), LNC)
                                                    End If
                                                Next
                                            End If
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End Using
                    If POST.IsEmptyString And ExistsDetected Then Exit Sub
                    If Not PostID.IsEmptyString And NewPostDetected Then DownloadData(PostID, Token)
                End If
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]")
            End Try
        End Sub
        Friend Shared Function GetVideoInfo(ByVal URL As String) As IEnumerable(Of UserMedia)
            Try
                If URL.Contains("twitter") Then
                    Dim PostID$ = RegexReplace(URL, New RegexStructure("(?<=/)\d+", True, False,,,,, String.Empty))
                    If Not PostID.IsEmptyString Then
                        Dim r$ = DirectCast(Settings(Sites.Twitter).Responser.Copy(), Response).
                                            GetResponse($"https://api.twitter.com/1.1/statuses/show.json?id={PostID}",, EDP.ReturnValue)
                        If Not r.IsEmptyString Then
                            Using j As EContainer = JsonDocument.Parse(r)
                                If j.ListExists Then
                                    Dim u$ = GetVideoNodeURL(j)
                                    If Not u.IsEmptyString Then Return {MediaFromData(u, PostID, String.Empty)}
                                End If
                            End Using
                        End If
                    End If
                End If
                Return Nothing
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.ShowMainMsg + EDP.SendInLog, ex, "Video searching error")
            End Try
        End Function
#Region "Picture options"
        Private Function GetPictureOption(ByVal w As EContainer) As String
            Try
                Dim ww As EContainer = w("sizes")
                If Not ww Is Nothing AndAlso ww.Count > 0 Then
                    Dim l As New List(Of Sizes)
                    Dim LargeContained As Boolean = ww.Contains("large")
                    For Each v As EContainer In ww
                        If v.Count > 0 AndAlso v.Contains("h") Then l.Add(New Sizes(v.Value("h"), v.Name))
                    Next
                    If l.Count > 0 Then
                        l.Sort()
                        If l(0).Data.IsEmptyString And LargeContained Then Return "large" Else Return l(0).Data
                    End If
                End If
                Return String.Empty
            Catch ex As Exception
                LogError(ex, "[API.Twitter.UserData.GetPictureOption]")
                Return String.Empty
            End Try
        End Function
#End Region
#Region "Video options"
        Private Function CheckVideoNode(ByVal w As EContainer, ByVal PostID As String, ByVal PostDate As String) As Boolean
            Try
                Dim URL$ = GetVideoNodeURL(w)
                If Not URL.IsEmptyString Then _TempMediaList.ListAddValue(MediaFromData(URL, PostID, PostDate), LNC) : Return True
                Return False
            Catch ex As Exception
                LogError(ex, "[API.Twitter.UserData.CheckVideoNode]")
                Return False
            End Try
        End Function
        Private Shared Function GetVideoNodeURL(ByVal w As EContainer) As String
            Dim v As EContainer = w.GetNode(VideoNode)
            If Not v Is Nothing AndAlso v.Count > 0 Then
                Dim l As New List(Of Sizes)
                Dim u$
                Dim nn As EContainer
                For Each n As EContainer In v
                    If n.Count > 0 Then
                        For Each nn In n
                            If nn("content_type").XmlIfNothingValue("none").Contains("mp4") AndAlso nn.Contains("url") Then
                                u = nn.Value("url")
                                l.Add(New Sizes(RegexReplace(u, VideoSizeRegEx), u))
                            End If
                        Next
                    End If
                Next
                If l.Count > 0 Then l.RemoveAll(Function(s) s.HasError)
                If l.Count > 0 Then l.Sort() : Return l(0).Data
            End If
            Return String.Empty
        End Function
        Protected Overrides Sub ReparseVideo(ByVal Token As CancellationToken)
        End Sub
#End Region
        Private Shared Function MediaFromData(ByVal _URL As String, ByVal PostID As String, ByVal PostDate As String,
                                              Optional ByVal _PictureOption As String = "") As UserMedia
            _URL = LinkFormatterSecure(RegexReplace(_URL.Replace("\", String.Empty), LinkPattern))
            Dim m As New UserMedia(_URL) With {.PictureOption = _PictureOption, .Post = New UserPost With {.ID = PostID}}
            If Not m.URL.IsEmptyString Then m.File = CStr(RegexReplace(m.URL, FilesPattern))
            If Not m.PictureOption.IsEmptyString And Not m.File.IsEmptyString And Not m.URL.IsEmptyString Then
                m.URL_BASE = $"{m.URL.Replace($".{m.File.Extension}", String.Empty)}?format={m.File.Extension}&name={m.PictureOption}"
            End If
            If Not PostDate.IsEmptyString Then m.Post.Date = AConvert(Of Date)(PostDate, Declarations.DateProvider, Nothing) Else m.Post.Date = Nothing
            Return m
        End Function
#End Region
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False) As Integer
            If Responser.StatusCode = HttpStatusCode.NotFound Then
                UserExists = False
            ElseIf Responser.StatusCode = HttpStatusCode.Unauthorized Then
                UserSuspended = True
            ElseIf Responser.StatusCode = HttpStatusCode.BadRequest Then
                MyMainLOG = "Twitter has invalid credentials"
            Else
                If Not FromPE Then LogError(ex, Message) : HasError = True
                Return 0
            End If
            Return 1
        End Function
    End Class
End Namespace