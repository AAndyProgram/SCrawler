' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Forms.Toolbars
Imports SCrawler.API.Base
Imports System.Threading
Namespace API.Reddit
    Friend Class ChannelsCollection : Implements ICollection(Of Channel), IMyEnumerator(Of Channel), IChannelLimits, IDisposable
        Friend Shared ReadOnly Property ChannelsPath As SFile = $"{SettingsFolderName}\Channels\"
        Friend Shared ReadOnly Property ChannelsPathCache As SFile = $"{Settings.GlobalPath.Value.PathWithSeparator}_CachedData\"
        Private ReadOnly Channels As List(Of Channel)
        Friend ReadOnly Property Downloading As Boolean
            Get
                If Count > 0 Then
                    Return Channels.Exists(Function(c) c.Downloading)
                Else
                    Return False
                End If
            End Get
        End Property
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
            If Count > 0 Then Channels.ForEach(Sub(c) c.Save())
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
                Throw New ArgumentException($"Channel ID [{ChannelID}] does not found in channels collection", "ChannelID") With {.HelpLink = 1}
                'Return Nothing
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
            If Count > 0 Then
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