' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API
Imports SCrawler.API.Base
Friend Class ListImagesLoader
    Private ReadOnly Property MyList As ListView
    Private Class UserOption : Implements IComparable(Of UserOption)
        Friend ReadOnly User As IUserData
        Friend ReadOnly LVI As ListViewItem
        Friend Index As Integer
        Friend ReadOnly Property Key As String
            Get
                Return LVI.Name
            End Get
        End Property
        Friend [Image] As Image
        Friend Sub New(ByVal u As IUserData, ByVal l As ListView, ByVal GetImage As Boolean)
            User = u
            LVI = u.GetLVI(l)
            Index = u.Index
            If GetImage Then Image = u.GetPicture
        End Sub
        Friend Sub UpdateImage()
            Image = User.GetPicture
        End Sub
        Friend Function CompareTo(ByVal Other As UserOption) As Integer Implements IComparable(Of UserOption).CompareTo
            Return Index.CompareTo(Other.Index)
        End Function
    End Class
    Friend Sub New(ByRef l As ListView)
        MyList = l
    End Sub
    Friend Sub Update()
        Dim a As Action = Sub()
                              With MyList
                                  .Items.Clear()
                                  If Not .LargeImageList Is Nothing Then .LargeImageList.Images.Clear()
                                  .LargeImageList = New ImageList
                                  If Not .SmallImageList Is Nothing Then .SmallImageList.Images.Clear()
                                  .SmallImageList = New ImageList
                                  If Settings.ViewModeIsPicture Then
                                      .LargeImageList.ColorDepth = ColorDepth.Depth32Bit
                                      .SmallImageList.ColorDepth = ColorDepth.Depth32Bit
                                      .LargeImageList.ImageSize = New Size(DivideWithZeroChecking(Settings.MaxLargeImageHeight.Value, 100) * 75, Settings.MaxLargeImageHeight.Value)
                                      .SmallImageList.ImageSize = New Size(DivideWithZeroChecking(Settings.MaxSmallImageHeight.Value, 100) * 75, Settings.MaxSmallImageHeight.Value)
                                  End If
                              End With
                          End Sub
        If MyList.InvokeRequired Then MyList.Invoke(a) Else a.Invoke
        If Settings.Users.Count > 0 Then
            Settings.Users.Sort()
            Dim v As View = Settings.ViewMode.Value
            Dim i%

            With MyList
                MyList.BeginUpdate()

                If Settings.FastProfilesLoading Then
                    Settings.Users.ListReindex
                    Dim UData As List(Of UserOption)

                    If Settings.ViewModeIsPicture Then
                        UData = GetUsersWithImages()
                        If UData.ListExists Then
                            UData.Sort()
                            Select Case v
                                Case View.LargeIcon : .LargeImageList.Images.AddRange(UData.Select(Function(u) u.Image).ToArray)
                                Case View.SmallIcon : .SmallImageList.Images.AddRange(UData.Select(Function(u) u.Image).ToArray)
                            End Select
                        End If
                    Else
                        UData = (From u As IUserData In Settings.Users Where u.FitToAddParams Select New UserOption(u, MyList, False)).ListIfNothing
                        If UData.ListExists Then UData.Sort()
                    End If

                    If UData.ListExists Then
                        If Settings.ViewModeIsPicture Then
                            For i = 0 To UData.Count - 1
                                Select Case v
                                    Case View.LargeIcon : .LargeImageList.Images.SetKeyName(i, UData(i).Key)
                                    Case View.SmallIcon : .SmallImageList.Images.SetKeyName(i, UData(i).Key)
                                End Select
                            Next
                        End If
                        .Items.AddRange(UData.Select(Function(u) u.LVI).ToArray)
                        UData.Clear()
                    End If
                Else
                    Dim t As New List(Of Task)
                    For Each User As IUserData In Settings.Users
                        If User.FitToAddParams Then
                            If Settings.ViewModeIsPicture Then
                                t.Add(Task.Run(Sub() UpdateUser(User, True)))
                            Else
                                UpdateUser(User, True)
                            End If
                        End If
                    Next
                    If t.Count > 0 Then Task.WhenAll(t.ToArray) : t.Clear()
                End If
            End With
            MyList.EndUpdate()
        End If
    End Sub
    Friend Sub UpdateUser(ByVal User As IUserData, ByVal Add As Boolean)
        Try
            Dim a As Action
            If Add Then
                a = Sub()
                        With MyList
                            Select Case Settings.ViewMode.Value
                                Case View.LargeIcon : .LargeImageList.Images.Add(User.Key, User.GetPicture())
                                Case View.SmallIcon : .SmallImageList.Images.Add(User.Key, User.GetPicture())
                            End Select
                            .Items.Add(User.GetLVI(MyList))
                        End With
                    End Sub
            Else
                a = Sub()
                        With MyList
                            Dim i% = .Items.IndexOfKey(User.Key)
                            Dim ImgIndx%
                            If i >= 0 Then
                                Select Case Settings.ViewMode.Value
                                    Case View.LargeIcon
                                        ImgIndx = .LargeImageList.Images.IndexOfKey(User.Key)
                                        If ImgIndx >= 0 Then .LargeImageList.Images(ImgIndx) = User.GetPicture()
                                    Case View.SmallIcon
                                        ImgIndx = .SmallImageList.Images.IndexOfKey(User.Key)
                                        If ImgIndx >= 0 Then .SmallImageList.Images(ImgIndx) = User.GetPicture()
                                End Select
                                With .Items(i) : .Text = User.ToString() : .Group = User.GetLVIGroup(MyList) : End With
                                ApplyLVIColor(User, .Items(i), False)
                            End If
                        End With
                    End Sub
            End If
            If MyList.InvokeRequired Then MyList.Invoke(a) Else a.Invoke
        Catch ex As Exception
        End Try
    End Sub
    Friend Shared Function ApplyLVIColor(ByVal User As IUserData, ByVal LVI As ListViewItem, ByVal IsInit As Boolean) As ListViewItem
        With LVI
            If Not User.Exists Then
                .BackColor = MyColor.DeleteBack
                .ForeColor = MyColor.DeleteFore
            ElseIf User.Suspended Then
                .BackColor = MyColor.EditBack
                .ForeColor = MyColor.EditFore
            ElseIf CheckUserCollection(User) Then
                .BackColor = Color.LightSkyBlue
                .ForeColor = Color.MidnightBlue
            ElseIf Not IsInit Then
                .BackColor = SystemColors.Window
                .ForeColor = SystemColors.WindowText
            End If
        End With
        Return LVI
    End Function
    Private Shared Function CheckUserCollection(ByVal User As IUserData) As Boolean
        If User.IsCollection Then
            With DirectCast(User, UserDataBind)
                If .Count > 0 Then Return .Collections.Exists(Function(c) Not c.Exists) Else Return False
            End With
        Else
            Return False
        End If
    End Function
    Private Function GetUsersWithImages() As List(Of UserOption)
        Try
            Dim t As New List(Of Task)
            Dim l As New List(Of UserOption)
            For Each u As IUserData In Settings.Users
                If u.FitToAddParams Then t.Add(Task.Run(Sub() l.Add(New UserOption(u, MyList, True))))
            Next
            If t.Count > 0 Then Task.WaitAll(t.ToArray)
            If l.Count > 0 Then
                For i% = 0 To l.Count - 1
                    If l(i) Is Nothing Then Throw New ArgumentNullException("UserOption", $"One of the UserOptions [{i} / {l.Count - 1}] is null.")
                    If l(i).Image Is Nothing Then l(i).UpdateImage()
                Next
            End If
            Return l
        Catch ex As Exception
            Return ErrorsDescriber.Execute(EDP.LogMessageValue, ex,
                                           "Image fast loading error." & vbCr &
                                           "Click the ""Refresh"" button to manually refresh the user list." & vbCr &
                                           "[ListImagesLoader.GetUsersWithImages]")
        End Try
    End Function
End Class