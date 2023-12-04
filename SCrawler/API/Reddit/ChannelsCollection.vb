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
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Forms.Toolbars
Namespace API.Reddit
    Friend Class ChannelsCollection : Implements ICollection(Of Channel), IMyEnumerator(Of Channel), IChannelLimits, IDisposable
        Friend Shared ReadOnly Property ChannelsPath As SFile
            Get
                Return $"{SettingsFolderName}\Channels\"
            End Get
        End Property
        Friend Shared ReadOnly Property ChannelsDeletedPath As SFile
            Get
                Return $"{SettingsFolderName}\ChannelsDeleted\"
            End Get
        End Property
        Friend Shared ReadOnly Property ChannelsPathCache As SFile
            Get
                Return $"{Settings.GlobalPath.Value.PathWithSeparator}_CachedData\"
            End Get
        End Property
        Private ReadOnly Channels As List(Of Channel)
        Friend Structure ChannelImage : Implements IEquatable(Of ChannelImage)
            Friend File As SFile
            Friend Channel As String
            Friend Sub New(ByVal ChannelName As String, ByVal f As SFile)
                Channel = ChannelName
                File = f
            End Sub
            Friend Overloads Function Equals(ByVal Other As ChannelImage) As Boolean Implements IEquatable(Of ChannelImage).Equals
                Return Channel = Other.Channel And File.File = Other.File.File
            End Function
            Public Overloads Overrides Function Equals(ByVal Obj As Object) As Boolean
                Return Equals(DirectCast(Obj, ChannelImage))
            End Function
        End Structure
        Friend ReadOnly Property Downloading As Boolean
            Get
                Return Count > 0 AndAlso Channels.Exists(Function(c) c.Downloading)
            End Get
        End Property
        Friend Function GetUserFiles(ByVal UserName As String) As IEnumerable(Of ChannelImage)
            Try
                If Settings.ChannelsAddUserImagesFromAllChannels.Value And Count > 0 Then
                    Return Channels.SelectMany(Function(c) From p As UserPost In c.Posts Where p.UserID = UserName Select New ChannelImage(c.Name, p.CachedFile))
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, "[API.Reddit.ChannelsCollection.GetUserFiles]")
            End Try
        End Function
        Friend Sub UpdateUsersStats()
            If Channels.Count > 0 Then Channels.ForEach(Sub(c) c.UpdateUsersStats())
        End Sub
#Region "Limits Support"
        Friend Property DownloadLimitCount As Integer? Implements IChannelLimits.DownloadLimitCount
        <Obsolete("This property cannot be used in collections", True)> Private Property DownloadLimitPost As String Implements IChannelLimits.DownloadLimitPost
        Friend Property DownloadLimitDate As Date? Implements IChannelLimits.DownloadLimitDate
        Friend Overloads Sub SetLimit(Optional ByVal MaxPost As String = "", Optional ByVal MaxCount As Integer? = Nothing,
                                      Optional ByVal MinDate As Date? = Nothing) Implements IChannelLimits.SetLimit
            'DownloadLimitPost = MaxPost
            DownloadLimitCount = MaxCount
            DownloadLimitDate = MinDate
        End Sub
        Friend Overloads Sub SetLimit(ByVal Source As IChannelLimits) Implements IChannelLimits.SetLimit
            With Source
                DownloadLimitCount = .DownloadLimitCount
                DownloadLimitDate = .DownloadLimitDate
                AutoGetLimits = .AutoGetLimits
            End With
        End Sub
        Friend Property AutoGetLimits As Boolean = True Implements IChannelLimits.AutoGetLimits
#End Region
        Friend Sub New()
            Channels = New List(Of Channel)
        End Sub
        Friend Sub Load()
            If ChannelsPath.Exists(SFO.Path, False) Then
                Dim files As List(Of SFile) = SFile.GetFiles(ChannelsPath, "*.xml")
                If files.ListExists Then files.ForEach(Sub(f) Add(f))
            End If
        End Sub
        Friend Sub Update()
            If Count > 0 Then Channels.ForEach(Sub(c) c.SaveUnsaved())
        End Sub
        Friend ReadOnly Property Count As Integer Implements ICollection(Of Channel).Count, IMyEnumerator(Of Channel).MyEnumeratorCount
            Get
                Return Channels.Count
            End Get
        End Property
        Default Friend ReadOnly Property Item(ByVal Index As Integer) As Channel Implements IMyEnumerator(Of Channel).MyEnumeratorObject
            Get
                Return Channels(Index)
            End Get
        End Property
        ''' <exception cref="ArgumentException"></exception>
        Friend ReadOnly Property Find(ByVal ChannelID As String) As Channel
            Get
                If Count > 0 Then
                    For i% = 0 To Count - 1
                        If Item(i).ID = ChannelID Then Return Item(i)
                    Next
                End If
                Throw New ArgumentException($"Channel ID [{ChannelID}] not found in channel collection", "ChannelID") With {.HelpLink = 1}
            End Get
        End Property
        Friend Sub DownloadData(ByVal Token As CancellationToken, Optional ByVal SkipExists As Boolean = True,
                                Optional ByVal p As MyProgress = Nothing)
            Try
                Dim m% = Settings.ChannelsMaxJobsCount
                If Count > 0 Then
                    Dim t As New List(Of Task)
                    Dim i% = 0
                    For Each c As Channel In Channels
                        If Not c.Downloading Then t.Add(Task.Run(Sub()
                                                                     c.SetLimit(Me)
                                                                     c.DownloadData(Token, SkipExists, p)
                                                                 End Sub)) : i += 1
                        If t.Count > 0 And i >= m Then Task.WaitAll(t.ToArray, Token) : t.Clear() : i = 0
                    Next
                    If t.Count > 0 Then Task.WaitAll(t.ToArray, Token) : t.Clear()
                End If
            Catch oex As OperationCanceledException When Token.IsCancellationRequested
            End Try
        End Sub
#Region "ICollection Support"
        Private ReadOnly Property IsReadOnly As Boolean = False Implements ICollection(Of Channel).IsReadOnly
        Friend Sub Add(ByVal _Item As Channel) Implements ICollection(Of Channel).Add
            If Not Contains(_Item) Then Channels.Add(_Item)
        End Sub
        Friend Sub Clear() Implements ICollection(Of Channel).Clear
            Channels.ListClearDispose
        End Sub
        Private Sub CopyTo(ByVal _Array() As Channel, ByVal ArrayIndex As Integer) Implements ICollection(Of Channel).CopyTo
            Throw New NotImplementedException()
        End Sub
        Friend Function Contains(ByVal _Item As Channel) As Boolean Implements ICollection(Of Channel).Contains
            Return Count > 0 AndAlso Channels.Contains(_Item)
        End Function
        Friend Function Remove(ByVal _Item As Channel) As Boolean Implements ICollection(Of Channel).Remove
            If Count > 0 And Not _Item Is Nothing Then
                Dim i% = Channels.IndexOf(_Item)
                If i >= 0 Then
                    With Channels(i) : .Delete() : .Dispose() : End With
                    Channels.RemoveAt(i)
                    Return True
                End If
            End If
            Return False
        End Function
#End Region
#Region "IEnumerable Support"
        Friend Function GetEnumerator() As IEnumerator(Of Channel) Implements IEnumerable(Of Channel).GetEnumerator
            Return New MyEnumerator(Of Channel)(Me)
        End Function
        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
#End Region
#Region "IDisposable Support"
        Private disposedValue As Boolean = False
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then Update() : Clear()
                disposedValue = True
            End If
        End Sub
        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub
        Friend Overloads Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace