' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports SCrawler.Plugin.Hosts
Namespace Editors
    Public Class SiteDefaults : Inherits TableLayoutPanel
        Private ReadOnly CH_TEMP As CheckBox
        Private ReadOnly CH_IMG As CheckBox
        Private ReadOnly CH_VID As CheckBox
        Public Sub New()
            InitCheckBox(CH_TEMP, "Temporary")
            InitCheckBox(CH_IMG, "Download images")
            InitCheckBox(CH_VID, "Download videos")
        End Sub
        Private Sub InitCheckBox(ByRef CH As CheckBox, ByVal Caption As String)
            CH = New CheckBox With {.Text = Caption, .Dock = DockStyle.Fill, .UseVisualStyleBackColor = True,
                                    .ThreeState = True, .CheckState = CheckState.Indeterminate}
        End Sub
        Private Sub SiteDefaults_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            CH_TEMP.Dispose()
            CH_IMG.Dispose()
            CH_VID.Dispose()
        End Sub
        Protected Overrides Sub InitLayout()
            MyBase.InitLayout()
            If ColumnStyles.Count = 2 Or RowStyles.Count = 2 Then
                ColumnStyles.Clear()
                RowStyles.Clear()
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
                ColumnCount = 1
                ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100))
                RowCount = 4
                RowStyles.Add(New RowStyle(SizeType.Absolute, 25))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 25))
                RowStyles.Add(New RowStyle(SizeType.Absolute, 25))
                RowStyles.Add(New RowStyle(SizeType.Percent, 100))
            End If
            Controls.Add(CH_TEMP, 0, 0)
            Controls.Add(CH_IMG, 0, 1)
            Controls.Add(CH_VID, 0, 2)
        End Sub
        Private _BaseControlsPadding As New Padding(0)
        <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            Category("Values"), DisplayName("Controls padding"), Description("Base controls padding")>
        Public Property BaseControlsPadding As Padding
            Get
                Return _BaseControlsPadding
            End Get
            Set(ByVal p As Padding)
                _BaseControlsPadding = p
                CH_TEMP.Padding = p
                CH_IMG.Padding = p
                CH_VID.Padding = p
            End Set
        End Property
        Private Function ShouldSerializeBaseControlsPadding() As Boolean
            Return Not _BaseControlsPadding.Equals(New Padding(0))
        End Function
        <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            Category("Values"), DefaultValue(CheckState.Indeterminate), DisplayName("Temporary"), Description("Temporary profile")>
        Public Property MyTemporary As CheckState
            Get
                Return CH_TEMP.CheckState
            End Get
            Set(ByVal s As CheckState)
                CH_TEMP.CheckState = s
            End Set
        End Property
        <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            Category("Values"), DefaultValue(CheckState.Indeterminate), DisplayName("Images"), Description("Download images")>
        Public Property MyImagesDown As CheckState
            Get
                Return CH_IMG.CheckState
            End Get
            Set(ByVal s As CheckState)
                CH_IMG.CheckState = s
            End Set
        End Property
        <Browsable(True), EditorBrowsable(EditorBrowsableState.Always), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
            Category("Values"), DefaultValue(CheckState.Indeterminate), DisplayName("Videos"), Description("Download videos")>
        Public Property MyVideosDown As CheckState
            Get
                Return CH_VID.CheckState
            End Get
            Set(ByVal s As CheckState)
                CH_VID.CheckState = s
            End Set
        End Property
    End Class
    Friend NotInheritable Class SiteDefaultsFunctions
        Private Sub New()
        End Sub
        Friend Overloads Shared Sub SetChecker(ByRef CH As SiteDefaults, ByRef h As SettingsHost)
            SetChecker(CH.MyTemporary, h.Temporary)
            SetChecker(CH.MyImagesDown, h.DownloadImages)
            SetChecker(CH.MyVideosDown, h.DownloadVideos)
        End Sub
        Private Overloads Shared Sub SetChecker(ByRef State As CheckState, ByVal Prop As XML.Objects.XMLValue(Of Boolean))
            If Prop.ValueF.Exists Then
                State = If(Prop.Value, CheckState.Checked, CheckState.Unchecked)
            Else
                State = CheckState.Indeterminate
            End If
        End Sub
        Friend Overloads Shared Sub SetPropByChecker(ByRef CH As SiteDefaults, ByRef h As SettingsHost)
            SetPropByChecker(CH.MyTemporary, h.Temporary)
            SetPropByChecker(CH.MyImagesDown, h.DownloadImages)
            SetPropByChecker(CH.MyVideosDown, h.DownloadVideos)
        End Sub
        Private Overloads Shared Sub SetPropByChecker(ByVal State As CheckState, ByRef Prop As XML.Objects.XMLValue(Of Boolean))
            Select Case State
                Case CheckState.Checked : Prop.Value = True
                Case CheckState.Unchecked : Prop.Value = False
                Case CheckState.Indeterminate : Prop.ValueF = Nothing
            End Select
        End Sub
    End Class
End Namespace