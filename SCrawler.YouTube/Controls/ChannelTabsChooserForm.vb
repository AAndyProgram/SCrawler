' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports SCrawler.API.YouTube.Base
Namespace API.YouTube.Controls
    Friend Class ChannelTabsChooserForm : Implements IDesignXMLContainer
        Private WithEvents MyDefs As DefaultFormOptions
        Friend Property DesignXML As EContainer Implements IDesignXMLContainer.DesignXML
        Private Property DesignXMLNodes As String() Implements IDesignXMLContainer.DesignXMLNodes
        Private Property DesignXMLNodeName As String Implements IDesignXMLContainer.DesignXMLNodeName
        Private _Result As YouTubeChannelTab = YouTubeChannelTab.All
        Friend ReadOnly Property Result As YouTubeChannelTab
            Get
                Return _Result
            End Get
        End Property
        Friend ReadOnly Property URL As String
            Get
                Return TXT_URL.Text
            End Get
        End Property
        Friend ReadOnly Property MyUrlAsIs As Boolean
            Get
                Return CH_URL_ASIS.Checked
            End Get
        End Property
        Friend Sub New(ByVal InitVal As YouTubeChannelTab, ByVal _URL As String)
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me)
            _Result = InitVal
            TXT_URL.Text = _URL
        End Sub
        Private Sub ChannelTabsChooserForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDefs
                    MyDefs.MyXML = DesignXML
                    If Not DesignXML Is Nothing Then .MyViewInitialize(True)
                    .AddOkCancelToolbar()
                    If _Result = YouTubeChannelTab.All And Not MyYouTubeSettings Is Nothing Then _Result = MyYouTubeSettings.ChannelsDownload
                    Dim r() As YouTubeChannelTab = _Result.EnumExtract(Of YouTubeChannelTab)
                    If r.ListExists Then
                        For Each value As YouTubeChannelTab In r
                            Select Case value
                                Case YouTubeChannelTab.All : CH_ALL.Checked = True
                                Case YouTubeChannelTab.Videos : CH_VIDEOS.Checked = True
                                Case YouTubeChannelTab.Shorts : CH_SHORTS.Checked = True
                                Case YouTubeChannelTab.Playlists : CH_PLS.Checked = True
                            End Select
                        Next
                    Else
                        CH_ALL.Checked = True
                    End If
                    UpdateCheckBoxes()
                    .EndLoaderOperations()
                    .MyOkCancel.EnableOK = True
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            _Result = YouTubeChannelTab.All
            If Not CH_ALL.Checked And {CH_VIDEOS, CH_SHORTS, CH_PLS}.Any(Function(c) c.Checked) Then
                If CH_VIDEOS.Checked Then _Result += YouTubeChannelTab.Videos
                If CH_SHORTS.Checked Then _Result += YouTubeChannelTab.Shorts
                If CH_PLS.Checked Then _Result += YouTubeChannelTab.Playlists
            End If
            MyDefs.CloseForm()
        End Sub
        Private Sub UpdateCheckBoxes() Handles CH_ALL.CheckedChanged, CH_URL_ASIS.CheckedChanged
            Dim e As Boolean = Not CH_ALL.Checked And Not CH_URL_ASIS.Checked
            CH_VIDEOS.Enabled = e
            CH_SHORTS.Enabled = e
            CH_PLS.Enabled = e
        End Sub
    End Class
End Namespace