' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports SCrawler.API.Base
Imports SCrawler.API.YouTube.Objects
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports UTypes = SCrawler.API.Base.UserMedia.Types
Namespace API.JustForFans
    Friend Class UserData : Inherits UserDataBase
#Region "Declarations"
        Private ReadOnly Property MySettings As SiteSettings
            Get
                Return HOST.Source
            End Get
        End Property
        Private ResponserNoHandlers As Responser = Nothing
#End Region
#Region "Structures"
        Private Class FileSerial
            Private InitNumber As Integer
            Private ReadOnly Provider As New ANumbers With {.FormatOptions = ANumbers.Options.FormatNumberGroup, .GroupSize = 9}
            Friend Sub New(ByVal Root As String)
                Try
                    Dim r$ = Root.CSFilePS
                    InitNumber = SFile.GetFiles(r,,, EDP.ReturnValue).Count +
                                 SFile.GetFiles($"{r}Video\",,, EDP.ReturnValue).Count +
                                 SFile.GetFiles($"{r}Videos\",,, EDP.ReturnValue).Count
                Catch
                    InitNumber = 0
                End Try
            End Sub
            Friend Function ApplyHash(ByVal f As SFile) As SFile
                InitNumber += 1
                f.Name &= $"_{InitNumber.NumToString(Provider)}_{$"{Now:O}_{Rnd()}".GetHashCode()}"
                Return f
            End Function
        End Class
        Private Structure PhotoData : Implements IRegExCreator
            Friend IsLarge As Boolean
            Friend URL As String
            Private Function CreateFromArray(ByVal ParamsArray() As String) As Object Implements IRegExCreator.CreateFromArray
                If ParamsArray.ListExists Then
                    IsLarge = Not ParamsArray(0).IsEmptyString AndAlso ParamsArray(0).StringToLower = NamePhotoLarge
                    URL = ParamsArray(1)
                End If
                Return Me
            End Function
        End Structure
        Private Structure PostBlock : Implements IRegExCreator
            Friend PostID As String
            Friend PostDate As Date?
            Friend PostUrl As String
            Friend Pinned As Boolean
            Private Data As String
            Private File As SFile
            Private FileNameDefault As String
            Friend Type As UTypes
            Friend Values As IEnumerable(Of String)
            Friend ReadOnly Property Valid As Boolean
                Get
                    Return Values.ListExists And Not Type = UTypes.Undefined And Not PostID.IsEmptyString And Not PostUrl.IsEmptyString
                End Get
            End Property
            Private Function CreateFromArray(ByVal ParamsArray() As String) As Object Implements IRegExCreator.CreateFromArray
                If ParamsArray.ListExists(3) Then
                    Data = ParamsArray(0)
                    Pinned = Not ParamsArray(1).IsEmptyString AndAlso ParamsArray(1).ToLower.Contains("pinned")
                    PostDate = AConvert(Of Date)(RegexReplace(Data, Regex_PostDate), DateProvider, Nothing)
                    PostUrl = RegexReplace(Data, Regex_PostURL)
                    If Not PostUrl.IsEmptyString Then PostUrl = $"https://justfor.fans/{PostUrl.Trim.StringTrimStart("/")}"
                    PostID = RegexReplace(PostUrl, Regex_PostID)
                    If Not Data.IsEmptyString Then
                        FileNameDefault = AConvert(Of String)(If(PostDate, Now), DateProviderVideoFileName, String.Empty)

                        Dim found As Boolean = False
                        Dim tmpData$ = RegexReplace(Data, Regex_Video)

                        If Not tmpData.IsEmptyString Then
                            found = True
                            File.Name = FileNameDefault
                            File.Extension = "mp4"
                            Using j As EContainer = JsonDocument.Parse(tmpData, EDP.ReturnValue)
                                If j.ListExists Then
                                    Dim vr As RParams = RParams.DM("(\d+)", 0, EDP.ReturnValue)
                                    Dim l As New List(Of Sizes)
                                    Dim s As Sizes
                                    Dim all$ = String.Empty
                                    Dim t As UTypes = UTypes.m3u8
                                    For Each jj As EContainer In j
                                        If jj.Name.StringToLower = "all" Then
                                            all = jj.Value
                                        Else
                                            s = New Sizes(RegexReplace(jj.Name, vr), jj.Value)
                                            If Not s.HasError Then l.Add(s)
                                        End If
                                    Next
                                    If l.Count = 0 Then l.Add(New Sizes(0, all)) : t = M3U8.AllVid
                                    If l.Count > 0 Then
                                        l.Sort()
                                        Values = {l(0).Data}
                                        Type = t
                                        If Not Values(0).Contains("m3u8") Then Type = UTypes.Video
                                    End If
                                End If
                            End Using
                        End If

                        If Not found AndAlso Not CStr(RegexReplace(Data, Regex_Gallery)).IsEmptyString Then
                            found = True
                            File = Nothing
                            Dim pData As List(Of PhotoData) = RegexFields(Of PhotoData)(Data, {Regex_Photo}, {1, 3}, EDP.ReturnValue)
                            If pData.ListExists Then
                                Type = UTypes.Picture
                                If pData.Exists(Function(d) d.IsLarge) Then
                                    Values = (From d As PhotoData In pData Where d.IsLarge Select d.URL).ToArray
                                Else
                                    Values = pData.Select(Function(d) d.URL).Distinct
                                End If
                            End If
                        End If

                        If Not found Then
                            File = Nothing
                            Dim pp As RParams = Regex_Photo.Copy
                            pp.Match = Nothing
                            pp.MatchSub = 3
                            pp.WhatGet = RegexReturn.Value
                            Dim v$ = RegexReplace(Data, pp)
                            If Not v.IsEmptyString Then found = True : Type = UTypes.Picture : Values = {v}
                        End If
                    End If
                End If
                Return Me
            End Function
            Friend Function GetUserMedia(ByVal FS As FileSerial) As IEnumerable(Of UserMedia)
                If Values.ListExists Then
                    Dim m As UserMedia
                    Dim f As SFile
                    Dim outList As New List(Of UserMedia)
                    For Each url$ In Values
                        m = New UserMedia(url, Type) With {.URL_BASE = PostUrl.IfNullOrEmpty(.URL_BASE), .Post = New UserPost(PostID, PostDate)}
                        f = New SFile With {.Name = FileNameDefault, .Extension = m.File.Extension}
                        If Not Type = UTypes.Picture And Not Type = UTypes.GIF Then f.Extension = "mp4"
                        f = FS.ApplyHash(f)
                        m.File = f
                        outList.Add(m)
                    Next
                    Return outList
                Else
                    Return New UserMedia() {}
                End If
            End Function
        End Structure
#End Region
#Region "Loader"
        Protected Overrides Sub LoadUserInformation_OptionalFields(ByRef Container As XmlFile, ByVal Loading As Boolean)
        End Sub
#End Region
#Region "Initializer"
        Friend Sub New()
            UseInternalM3U8Function = True
            _ResponserAutoUpdateCookies = True
        End Sub
#End Region
#Region "Download functions"
        Private _DownloadedPostsCount As Integer = 0
        Private _Limit As Integer = -1
        Private FileSerialInstance As FileSerial
        Private _UserHash4 As String = String.Empty
        Private Sub InitializeFileSerial()
            If FileSerialInstance Is Nothing Then FileSerialInstance = New FileSerial(DownloadContentDefault_GetRootDir())
        End Sub
        Protected Overrides Sub DownloadDataF(ByVal Token As CancellationToken)
            Try
                _UserHash4 = MySettings.UserHash4.Value
                InitializeFileSerial()
                Responser.Cookies.Changed = False
                If Not ResponserNoHandlers Is Nothing Then ResponserNoHandlers.Dispose() : ResponserNoHandlers = Nothing
                ResponserNoHandlers = Responser.Copy
                AddHandler Responser.ResponseReceived, AddressOf Responser_ResponseReceived
                _DownloadedPostsCount = 0
                _Limit = If(DownloadTopCount, -1)
                DownloadData(0, Token)
            Finally
                If DownloadTopCount.HasValue Then DownloadTopCount = Nothing
                Responser_ResponseReceived_RemoveHandler()
                MySettings.UpdateResponser(Responser)
            End Try
        End Sub
        Protected Overrides Sub Responser_ResponseReceived(ByVal Source As Object, ByVal e As EventArguments.WebDataResponse)
            If e.CookiesExists Then
                Dim hv$ = If(e.Cookies.FirstOrDefault(Function(cc) cc.Name.StringToLower = SiteSettings.UserHash4_CookieName)?.Value, String.Empty)
                If Not hv.IsEmptyString And Not _UserHash4 = hv Then _UserHash4 = hv
            End If
        End Sub
        Private Overloads Sub DownloadData(ByVal Cursor As Integer, ByVal Token As CancellationToken)
            Dim URL$ = String.Empty
            Try
                Dim processed As Boolean = False

                ThrowAny(Token)

                If IsSavedPosts Then
                    URL = $"https://justfor.fans/home?Tab=Saved&Page={Cursor + 1}"
                Else
                    If ID.IsEmptyString Then GetUserID() : ThrowAny(Token)
                    If ID.IsEmptyString Then Throw New ArgumentNullException("ID", "The user ID cannot be null")
                    If _UserHash4.IsEmptyString Then Throw New ArgumentNullException("UserHash4", "[UserHash4] cannot be null")
                    URL = $"https://justfor.fans/ajax/getPosts.php?Type=One&UserID={MySettings.UserID.Value}&PosterID={ID}&StartAt={Cursor}&Page=Profile&UserHash4={_UserHash4}&SplitTest=0"
                End If

                Dim r$ = Responser.GetResponse(URL)

                If Not r.IsEmptyString Then
                    Dim data As List(Of PostBlock) = RegexFields(Of PostBlock)(r, {RegexVideoBlock}, {0, 2, 3}, EDP.ReturnValue)
                    If data.ListExists Then
                        For Each post As PostBlock In data
                            If post.Valid Then
                                processed = True
                                If Not post.PostID.IsEmptyString Then
                                    If _TempPostsList.Contains(post.PostID) Then
                                        If post.Pinned Then Continue For Else Exit Sub
                                    Else
                                        _TempPostsList.Add(post.PostID)
                                    End If
                                End If

                                Select Case CheckDatesLimit(post.PostDate, Nothing)
                                    Case DateResult.Skip : Continue For
                                    Case DateResult.Exit : If post.Pinned Then Continue For Else Exit Sub
                                End Select

                                _DownloadedPostsCount += 1
                                _TempMediaList.ListAddList(post.GetUserMedia(FileSerialInstance), LNC)

                                If _Limit > 0 And _DownloadedPostsCount >= _Limit Then Exit For
                            End If
                        Next
                    End If
                End If

                If processed And (_Limit = -1 Or _DownloadedPostsCount < _Limit) Then DownloadData(Cursor + IIf(IsSavedPosts, 1, 10), Token)
            Catch ex As Exception
                ProcessException(ex, Token, $"data downloading error [{URL}]")
            End Try
        End Sub
        Private Sub GetUserID()
            Try
                Dim r$, hash$, new_id$, profilePic$
                If ID.IsEmptyString Then
                    r = Responser.GetResponse($"https://justfor.fans/{Name}")
                    If Not r.IsEmptyString Then
                        hash = RegexReplace(r, RegexUser)
                        profilePic = RegexReplace(r, RParams.DMS("<img class=.mainProfilePic..+?src=""([^""]+)", 1, EDP.ReturnValue))
                        If Not hash.IsEmptyString Then
                            r = Responser.GetResponse($"https://justfor.fans/ajax/getAssetCount.php?User={Name}&Ver={hash}")
                            If Not r.IsEmptyString Then
                                Using j As EContainer = JsonDocument.Parse(r)
                                    If j.ListExists Then
                                        new_id = j.Value("UserID")
                                        If Not new_id.IsEmptyString Then
                                            new_id = RegexReplace(new_id, RParams.DM("\D", -1, RegexReturn.Replace,
                                                                                     CType(Function(input$) String.Empty, Func(Of String, String)),
                                                                                     String.Empty, EDP.ReturnValue))
                                            If Not new_id.IsEmptyString Then
                                                ID = new_id
                                                _ForceSaveUserInfo = True
                                                If Not profilePic.IsEmptyString Then GetWebFile(profilePic, $"{DownloadContentDefault_GetRootDir.CSFilePS}ProfilePic.jpg", EDP.None)
                                            End If
                                        End If
                                    End If
                                End Using
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                LogError(ex, "can't get user ID")
            End Try
        End Sub
#End Region
#Region "ReparseMissing"
        Protected Overrides Sub ReparseMissing(ByVal Token As CancellationToken)
            Dim rList As New List(Of Integer)
            Try
                If ContentMissingExists Then
                    InitializeFileSerial()
                    Dim r$
                    Dim m As UserMedia
                    Dim stateRefill As Func(Of UserMedia, Integer, UserMedia) = Function(ByVal input As UserMedia, ByVal ii As Integer) As UserMedia
                                                                                    input.State = UserMedia.States.Missing
                                                                                    input.Attempts = m.Attempts
                                                                                    Return input
                                                                                End Function
                    Dim p As PostBlock
                    Dim rErr As New ErrorsDescriber(EDP.ReturnValue)
                    For i% = 0 To _ContentList.Count - 1
                        m = _ContentList(i)
                        If m.State = UserMedia.States.Missing And Not m.URL_BASE.IsEmptyString Then
                            ThrowAny(Token)
                            r = Responser.GetResponse(m.URL_BASE,, rErr)
                            If Not r.IsEmptyString Then
                                With RegexFields(Of PostBlock)(r, {RegexVideoBlock}, {0, 2, 3}, rErr)
                                    If .ListExists Then
                                        rList.Add(i)
                                        For Each p In .Self
                                            If p.Valid Then _TempMediaList.ListAddList(p.GetUserMedia(FileSerialInstance).ListForEachCopy(stateRefill, True), LNC)
                                        Next
                                    End If
                                End With
                            End If
                        End If
                    Next
                End If
            Catch ex As Exception
                ProcessException(ex, Token, "missing data downloading error")
            Finally
                If rList.Count > 0 Then
                    For i% = rList.Count - 1 To 0 Step -1 : _ContentList.RemoveAt(rList(i)) : Next
                    rList.Clear()
                End If
            End Try
        End Sub
#End Region
#Region "DownloadContent"
        Protected Overrides Sub DownloadContent(ByVal Token As CancellationToken)
            DownloadContentDefault(Token)
        End Sub
        Protected Overrides Function DownloadM3U8(ByVal URL As String, ByVal Media As UserMedia, ByVal DestinationFile As SFile, ByVal Token As CancellationToken) As SFile
            Return M3U8.Download(Media, DestinationFile, ResponserNoHandlers, Me, Progress, Not IsSingleObjectDownload)
        End Function
#End Region
#Region "DownloadSingleObject"
        Protected Overrides Sub DownloadSingleObject_GetPosts(ByVal Data As IYouTubeMediaContainer, ByVal Token As CancellationToken)
            ResponserNoHandlers = Responser.Copy
            _ContentList.Add(New UserMedia(Data.URL) With {.State = UserMedia.States.Missing})
            ReparseMissing(Token)
        End Sub
#End Region
#Region "DownloadingException"
        Protected Overrides Function DownloadingException(ByVal ex As Exception, ByVal Message As String, Optional ByVal FromPE As Boolean = False,
                                                          Optional ByVal EObj As Object = Nothing) As Integer
            Return 0
        End Function
#End Region
#Region "IDisposable Support"
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue And disposing Then FileSerialInstance = Nothing
            MyBase.Dispose(disposing)
        End Sub
#End Region
    End Class
End Namespace