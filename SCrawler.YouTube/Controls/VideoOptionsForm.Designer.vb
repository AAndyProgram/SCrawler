' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.YouTube.Controls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class VideoOptionsForm : Inherits System.Windows.Forms.Form
        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub
        Private components As System.ComponentModel.IContainer
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Dim TP_HEADER As System.Windows.Forms.TableLayoutPanel
            Dim TP_HEADER_INFO As System.Windows.Forms.TableLayoutPanel
            Dim ICON_CLOCK As System.Windows.Forms.PictureBox
            Dim ICON_LINK As System.Windows.Forms.PictureBox
            Dim TP_FOOTER As System.Windows.Forms.TableLayoutPanel
            Dim TP_DESTINATION As System.Windows.Forms.TableLayoutPanel
            Dim TP_OK_CANCEL As System.Windows.Forms.TableLayoutPanel
            Dim LB_SEP_1 As System.Windows.Forms.Label
            Dim LB_SEP_2 As System.Windows.Forms.Label
            Dim TP_WHAT As System.Windows.Forms.TableLayoutPanel
            Dim LBL_WHAT As System.Windows.Forms.Label
            Dim LBL_FORMAT As System.Windows.Forms.Label
            Dim LBL_SUBS_FORMAT As System.Windows.Forms.Label
            Dim TT_MAIN As System.Windows.Forms.ToolTip
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(VideoOptionsForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton4 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton5 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton6 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton7 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton8 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton9 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Me.ICON_VIDEO = New System.Windows.Forms.PictureBox()
            Me.LBL_TITLE = New System.Windows.Forms.Label()
            Me.TP_HEADER_INFO_2 = New System.Windows.Forms.TableLayoutPanel()
            Me.LBL_TIME = New System.Windows.Forms.Label()
            Me.LBL_URL = New System.Windows.Forms.LinkLabel()
            Me.TXT_FILE = New System.Windows.Forms.TextBox()
            Me.BTT_BROWSE = New System.Windows.Forms.Button()
            Me.BTT_DOWN = New System.Windows.Forms.Button()
            Me.BTT_CANCEL = New System.Windows.Forms.Button()
            Me.OPT_VIDEO = New System.Windows.Forms.RadioButton()
            Me.OPT_AUDIO = New System.Windows.Forms.RadioButton()
            Me.LBL_AUDIO_CODEC = New System.Windows.Forms.Label()
            Me.TP_HEADER_BASE = New System.Windows.Forms.TableLayoutPanel()
            Me.TP_SUBS = New System.Windows.Forms.TableLayoutPanel()
            Me.TXT_SUBS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CMB_SUBS_FORMAT = New System.Windows.Forms.ComboBox()
            Me.TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            Me.TP_OPTIONS = New System.Windows.Forms.TableLayoutPanel()
            Me.CMB_FORMAT = New System.Windows.Forms.ComboBox()
            Me.CMB_AUDIO_CODEC = New System.Windows.Forms.ComboBox()
            Me.NUM_RES = New System.Windows.Forms.NumericUpDown()
            Me.TP_CONTROLS = New System.Windows.Forms.TableLayoutPanel()
            Me.TXT_SUBS_ADDIT = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_EXTRA_AUDIO_FORMATS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            TP_HEADER = New System.Windows.Forms.TableLayoutPanel()
            TP_HEADER_INFO = New System.Windows.Forms.TableLayoutPanel()
            ICON_CLOCK = New System.Windows.Forms.PictureBox()
            ICON_LINK = New System.Windows.Forms.PictureBox()
            TP_FOOTER = New System.Windows.Forms.TableLayoutPanel()
            TP_DESTINATION = New System.Windows.Forms.TableLayoutPanel()
            TP_OK_CANCEL = New System.Windows.Forms.TableLayoutPanel()
            LB_SEP_1 = New System.Windows.Forms.Label()
            LB_SEP_2 = New System.Windows.Forms.Label()
            TP_WHAT = New System.Windows.Forms.TableLayoutPanel()
            LBL_WHAT = New System.Windows.Forms.Label()
            LBL_FORMAT = New System.Windows.Forms.Label()
            LBL_SUBS_FORMAT = New System.Windows.Forms.Label()
            TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
            TP_HEADER.SuspendLayout()
            CType(Me.ICON_VIDEO, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_HEADER_INFO.SuspendLayout()
            Me.TP_HEADER_INFO_2.SuspendLayout()
            CType(ICON_CLOCK, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(ICON_LINK, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_FOOTER.SuspendLayout()
            TP_DESTINATION.SuspendLayout()
            TP_OK_CANCEL.SuspendLayout()
            TP_WHAT.SuspendLayout()
            Me.TP_HEADER_BASE.SuspendLayout()
            Me.TP_SUBS.SuspendLayout()
            CType(Me.TXT_SUBS, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TP_MAIN.SuspendLayout()
            Me.TP_OPTIONS.SuspendLayout()
            CType(Me.NUM_RES, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_SUBS_ADDIT, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_EXTRA_AUDIO_FORMATS, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'TP_HEADER
            '
            TP_HEADER.BackColor = System.Drawing.SystemColors.Window
            TP_HEADER.ColumnCount = 2
            TP_HEADER.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130.0!))
            TP_HEADER.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_HEADER.Controls.Add(Me.ICON_VIDEO, 0, 0)
            TP_HEADER.Controls.Add(TP_HEADER_INFO, 1, 0)
            TP_HEADER.Dock = System.Windows.Forms.DockStyle.Fill
            TP_HEADER.Location = New System.Drawing.Point(1, 1)
            TP_HEADER.Margin = New System.Windows.Forms.Padding(0)
            TP_HEADER.Name = "TP_HEADER"
            TP_HEADER.RowCount = 1
            TP_HEADER.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_HEADER.Size = New System.Drawing.Size(599, 63)
            TP_HEADER.TabIndex = 0
            '
            'ICON_VIDEO
            '
            Me.ICON_VIDEO.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
            Me.ICON_VIDEO.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ICON_VIDEO.Location = New System.Drawing.Point(1, 1)
            Me.ICON_VIDEO.Margin = New System.Windows.Forms.Padding(1)
            Me.ICON_VIDEO.Name = "ICON_VIDEO"
            Me.ICON_VIDEO.Size = New System.Drawing.Size(128, 61)
            Me.ICON_VIDEO.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            Me.ICON_VIDEO.TabIndex = 0
            Me.ICON_VIDEO.TabStop = False
            '
            'TP_HEADER_INFO
            '
            TP_HEADER_INFO.ColumnCount = 1
            TP_HEADER_INFO.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_HEADER_INFO.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_HEADER_INFO.Controls.Add(Me.LBL_TITLE, 0, 0)
            TP_HEADER_INFO.Controls.Add(Me.TP_HEADER_INFO_2, 0, 1)
            TP_HEADER_INFO.Dock = System.Windows.Forms.DockStyle.Fill
            TP_HEADER_INFO.Location = New System.Drawing.Point(130, 0)
            TP_HEADER_INFO.Margin = New System.Windows.Forms.Padding(0)
            TP_HEADER_INFO.Name = "TP_HEADER_INFO"
            TP_HEADER_INFO.RowCount = 2
            TP_HEADER_INFO.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_HEADER_INFO.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_HEADER_INFO.Size = New System.Drawing.Size(469, 63)
            TP_HEADER_INFO.TabIndex = 0
            '
            'LBL_TITLE
            '
            Me.LBL_TITLE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LBL_TITLE.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
            Me.LBL_TITLE.Location = New System.Drawing.Point(3, 0)
            Me.LBL_TITLE.Name = "LBL_TITLE"
            Me.LBL_TITLE.Size = New System.Drawing.Size(463, 31)
            Me.LBL_TITLE.TabIndex = 0
            Me.LBL_TITLE.Text = "Video title"
            Me.LBL_TITLE.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'TP_HEADER_INFO_2
            '
            Me.TP_HEADER_INFO_2.ColumnCount = 4
            Me.TP_HEADER_INFO_2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TP_HEADER_INFO_2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62.0!))
            Me.TP_HEADER_INFO_2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TP_HEADER_INFO_2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_HEADER_INFO_2.Controls.Add(ICON_CLOCK, 0, 0)
            Me.TP_HEADER_INFO_2.Controls.Add(Me.LBL_TIME, 1, 0)
            Me.TP_HEADER_INFO_2.Controls.Add(ICON_LINK, 2, 0)
            Me.TP_HEADER_INFO_2.Controls.Add(Me.LBL_URL, 3, 0)
            Me.TP_HEADER_INFO_2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_HEADER_INFO_2.Location = New System.Drawing.Point(0, 31)
            Me.TP_HEADER_INFO_2.Margin = New System.Windows.Forms.Padding(0)
            Me.TP_HEADER_INFO_2.Name = "TP_HEADER_INFO_2"
            Me.TP_HEADER_INFO_2.RowCount = 1
            Me.TP_HEADER_INFO_2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_HEADER_INFO_2.Size = New System.Drawing.Size(469, 32)
            Me.TP_HEADER_INFO_2.TabIndex = 1
            '
            'ICON_CLOCK
            '
            ICON_CLOCK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
            ICON_CLOCK.Dock = System.Windows.Forms.DockStyle.Fill
            ICON_CLOCK.Image = Global.SCrawler.My.Resources.Resources.ClockPic_16
            ICON_CLOCK.Location = New System.Drawing.Point(3, 3)
            ICON_CLOCK.Name = "ICON_CLOCK"
            ICON_CLOCK.Size = New System.Drawing.Size(19, 26)
            ICON_CLOCK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            ICON_CLOCK.TabIndex = 0
            ICON_CLOCK.TabStop = False
            '
            'LBL_TIME
            '
            Me.LBL_TIME.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LBL_TIME.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
            Me.LBL_TIME.ForeColor = System.Drawing.SystemColors.ControlDarkDark
            Me.LBL_TIME.Location = New System.Drawing.Point(28, 0)
            Me.LBL_TIME.Name = "LBL_TIME"
            Me.LBL_TIME.Size = New System.Drawing.Size(56, 32)
            Me.LBL_TIME.TabIndex = 0
            Me.LBL_TIME.Text = "00:00:00"
            Me.LBL_TIME.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'ICON_LINK
            '
            ICON_LINK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
            ICON_LINK.Dock = System.Windows.Forms.DockStyle.Fill
            ICON_LINK.Image = Global.SCrawler.My.Resources.Resources.LinkPic_32
            ICON_LINK.Location = New System.Drawing.Point(90, 3)
            ICON_LINK.Name = "ICON_LINK"
            ICON_LINK.Size = New System.Drawing.Size(19, 26)
            ICON_LINK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
            ICON_LINK.TabIndex = 2
            ICON_LINK.TabStop = False
            '
            'LBL_URL
            '
            Me.LBL_URL.AutoSize = True
            Me.LBL_URL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LBL_URL.Font = New System.Drawing.Font("Arial", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(204, Byte))
            Me.LBL_URL.LinkColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer))
            Me.LBL_URL.Location = New System.Drawing.Point(115, 0)
            Me.LBL_URL.Name = "LBL_URL"
            Me.LBL_URL.Size = New System.Drawing.Size(351, 32)
            Me.LBL_URL.TabIndex = 1
            Me.LBL_URL.TabStop = True
            Me.LBL_URL.Text = "https://www.youtube.com/watch?v=abcdefghijk"
            Me.LBL_URL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'TP_FOOTER
            '
            TP_FOOTER.ColumnCount = 1
            TP_FOOTER.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_FOOTER.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_FOOTER.Controls.Add(TP_DESTINATION, 0, 0)
            TP_FOOTER.Controls.Add(TP_OK_CANCEL, 0, 1)
            TP_FOOTER.Dock = System.Windows.Forms.DockStyle.Fill
            TP_FOOTER.Location = New System.Drawing.Point(6, 215)
            TP_FOOTER.Margin = New System.Windows.Forms.Padding(6, 3, 6, 3)
            TP_FOOTER.Name = "TP_FOOTER"
            TP_FOOTER.RowCount = 2
            TP_FOOTER.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_FOOTER.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_FOOTER.Size = New System.Drawing.Size(589, 52)
            TP_FOOTER.TabIndex = 5
            '
            'TP_DESTINATION
            '
            TP_DESTINATION.ColumnCount = 2
            TP_DESTINATION.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_DESTINATION.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
            TP_DESTINATION.Controls.Add(Me.TXT_FILE, 0, 0)
            TP_DESTINATION.Controls.Add(Me.BTT_BROWSE, 1, 0)
            TP_DESTINATION.Dock = System.Windows.Forms.DockStyle.Fill
            TP_DESTINATION.Location = New System.Drawing.Point(0, 0)
            TP_DESTINATION.Margin = New System.Windows.Forms.Padding(0)
            TP_DESTINATION.Name = "TP_DESTINATION"
            TP_DESTINATION.RowCount = 1
            TP_DESTINATION.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_DESTINATION.Size = New System.Drawing.Size(589, 26)
            TP_DESTINATION.TabIndex = 0
            '
            'TXT_FILE
            '
            Me.TXT_FILE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_FILE.Location = New System.Drawing.Point(3, 3)
            Me.TXT_FILE.Name = "TXT_FILE"
            Me.TXT_FILE.Size = New System.Drawing.Size(503, 20)
            Me.TXT_FILE.TabIndex = 0
            Me.TXT_FILE.WordWrap = False
            '
            'BTT_BROWSE
            '
            Me.BTT_BROWSE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_BROWSE.Location = New System.Drawing.Point(512, 2)
            Me.BTT_BROWSE.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
            Me.BTT_BROWSE.Name = "BTT_BROWSE"
            Me.BTT_BROWSE.Size = New System.Drawing.Size(74, 22)
            Me.BTT_BROWSE.TabIndex = 1
            Me.BTT_BROWSE.Text = "Browse"
            TT_MAIN.SetToolTip(Me.BTT_BROWSE, "Choose an output file")
            Me.BTT_BROWSE.UseVisualStyleBackColor = True
            '
            'TP_OK_CANCEL
            '
            TP_OK_CANCEL.ColumnCount = 3
            TP_OK_CANCEL.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_OK_CANCEL.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
            TP_OK_CANCEL.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
            TP_OK_CANCEL.Controls.Add(Me.BTT_DOWN, 1, 0)
            TP_OK_CANCEL.Controls.Add(Me.BTT_CANCEL, 2, 0)
            TP_OK_CANCEL.Dock = System.Windows.Forms.DockStyle.Fill
            TP_OK_CANCEL.Location = New System.Drawing.Point(0, 26)
            TP_OK_CANCEL.Margin = New System.Windows.Forms.Padding(0)
            TP_OK_CANCEL.Name = "TP_OK_CANCEL"
            TP_OK_CANCEL.RowCount = 1
            TP_OK_CANCEL.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_OK_CANCEL.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26.0!))
            TP_OK_CANCEL.Size = New System.Drawing.Size(589, 26)
            TP_OK_CANCEL.TabIndex = 1
            '
            'BTT_DOWN
            '
            Me.BTT_DOWN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_DOWN.Location = New System.Drawing.Point(432, 2)
            Me.BTT_DOWN.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
            Me.BTT_DOWN.Name = "BTT_DOWN"
            Me.BTT_DOWN.Size = New System.Drawing.Size(74, 22)
            Me.BTT_DOWN.TabIndex = 0
            Me.BTT_DOWN.Text = "Download"
            Me.BTT_DOWN.UseVisualStyleBackColor = True
            '
            'BTT_CANCEL
            '
            Me.BTT_CANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.BTT_CANCEL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_CANCEL.Location = New System.Drawing.Point(512, 2)
            Me.BTT_CANCEL.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
            Me.BTT_CANCEL.Name = "BTT_CANCEL"
            Me.BTT_CANCEL.Size = New System.Drawing.Size(74, 22)
            Me.BTT_CANCEL.TabIndex = 1
            Me.BTT_CANCEL.Text = "Cancel"
            Me.BTT_CANCEL.UseVisualStyleBackColor = True
            '
            'LB_SEP_1
            '
            LB_SEP_1.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            LB_SEP_1.BackColor = System.Drawing.SystemColors.ControlDark
            LB_SEP_1.Location = New System.Drawing.Point(6, 179)
            LB_SEP_1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
            LB_SEP_1.Name = "LB_SEP_1"
            LB_SEP_1.Size = New System.Drawing.Size(589, 1)
            LB_SEP_1.TabIndex = 3
            '
            'LB_SEP_2
            '
            LB_SEP_2.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            LB_SEP_2.BackColor = System.Drawing.SystemColors.ControlDark
            LB_SEP_2.Location = New System.Drawing.Point(6, 209)
            LB_SEP_2.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
            LB_SEP_2.Name = "LB_SEP_2"
            LB_SEP_2.Size = New System.Drawing.Size(589, 1)
            LB_SEP_2.TabIndex = 5
            '
            'TP_WHAT
            '
            TP_WHAT.ColumnCount = 3
            TP_WHAT.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65.0!))
            TP_WHAT.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_WHAT.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_WHAT.Controls.Add(LBL_WHAT, 0, 0)
            TP_WHAT.Controls.Add(Me.OPT_VIDEO, 1, 0)
            TP_WHAT.Controls.Add(Me.OPT_AUDIO, 2, 0)
            TP_WHAT.Dock = System.Windows.Forms.DockStyle.Fill
            TP_WHAT.Location = New System.Drawing.Point(0, 0)
            TP_WHAT.Margin = New System.Windows.Forms.Padding(0)
            TP_WHAT.Name = "TP_WHAT"
            TP_WHAT.RowCount = 1
            TP_WHAT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_WHAT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_WHAT.Size = New System.Drawing.Size(189, 28)
            TP_WHAT.TabIndex = 0
            '
            'LBL_WHAT
            '
            LBL_WHAT.AutoSize = True
            LBL_WHAT.Dock = System.Windows.Forms.DockStyle.Fill
            LBL_WHAT.Location = New System.Drawing.Point(3, 0)
            LBL_WHAT.Name = "LBL_WHAT"
            LBL_WHAT.Size = New System.Drawing.Size(59, 28)
            LBL_WHAT.TabIndex = 4
            LBL_WHAT.Text = "Download"
            LBL_WHAT.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'OPT_VIDEO
            '
            Me.OPT_VIDEO.AutoSize = True
            Me.OPT_VIDEO.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_VIDEO.Location = New System.Drawing.Point(68, 3)
            Me.OPT_VIDEO.Name = "OPT_VIDEO"
            Me.OPT_VIDEO.Size = New System.Drawing.Size(56, 22)
            Me.OPT_VIDEO.TabIndex = 0
            Me.OPT_VIDEO.TabStop = True
            Me.OPT_VIDEO.Text = "Video"
            Me.OPT_VIDEO.UseVisualStyleBackColor = True
            '
            'OPT_AUDIO
            '
            Me.OPT_AUDIO.AutoSize = True
            Me.OPT_AUDIO.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_AUDIO.Location = New System.Drawing.Point(130, 3)
            Me.OPT_AUDIO.Name = "OPT_AUDIO"
            Me.OPT_AUDIO.Size = New System.Drawing.Size(56, 22)
            Me.OPT_AUDIO.TabIndex = 1
            Me.OPT_AUDIO.TabStop = True
            Me.OPT_AUDIO.Text = "Audio"
            Me.OPT_AUDIO.UseVisualStyleBackColor = True
            '
            'LBL_FORMAT
            '
            LBL_FORMAT.Dock = System.Windows.Forms.DockStyle.Fill
            LBL_FORMAT.Location = New System.Drawing.Point(192, 0)
            LBL_FORMAT.Name = "LBL_FORMAT"
            LBL_FORMAT.Size = New System.Drawing.Size(74, 28)
            LBL_FORMAT.TabIndex = 4
            LBL_FORMAT.Text = "Format:"
            LBL_FORMAT.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            TT_MAIN.SetToolTip(LBL_FORMAT, "Output Video Format")
            '
            'LBL_SUBS_FORMAT
            '
            LBL_SUBS_FORMAT.AutoSize = True
            LBL_SUBS_FORMAT.Dock = System.Windows.Forms.DockStyle.Fill
            LBL_SUBS_FORMAT.Location = New System.Drawing.Point(432, 0)
            LBL_SUBS_FORMAT.Name = "LBL_SUBS_FORMAT"
            LBL_SUBS_FORMAT.Size = New System.Drawing.Size(74, 28)
            LBL_SUBS_FORMAT.TabIndex = 2
            LBL_SUBS_FORMAT.Text = "Format:"
            LBL_SUBS_FORMAT.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            TT_MAIN.SetToolTip(LBL_SUBS_FORMAT, "Output Subtitles Format")
            '
            'LBL_AUDIO_CODEC
            '
            Me.LBL_AUDIO_CODEC.AutoSize = True
            Me.LBL_AUDIO_CODEC.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LBL_AUDIO_CODEC.Location = New System.Drawing.Point(432, 0)
            Me.LBL_AUDIO_CODEC.Name = "LBL_AUDIO_CODEC"
            Me.LBL_AUDIO_CODEC.Size = New System.Drawing.Size(74, 28)
            Me.LBL_AUDIO_CODEC.TabIndex = 5
            Me.LBL_AUDIO_CODEC.Text = "Audio Codec"
            Me.LBL_AUDIO_CODEC.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            TT_MAIN.SetToolTip(Me.LBL_AUDIO_CODEC, "Output Audio Codec")
            '
            'TP_HEADER_BASE
            '
            Me.TP_HEADER_BASE.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            Me.TP_HEADER_BASE.ColumnCount = 1
            Me.TP_HEADER_BASE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_HEADER_BASE.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TP_HEADER_BASE.Controls.Add(TP_HEADER, 0, 0)
            Me.TP_HEADER_BASE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_HEADER_BASE.Location = New System.Drawing.Point(0, 0)
            Me.TP_HEADER_BASE.Margin = New System.Windows.Forms.Padding(0)
            Me.TP_HEADER_BASE.Name = "TP_HEADER_BASE"
            Me.TP_HEADER_BASE.RowCount = 1
            Me.TP_HEADER_BASE.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_HEADER_BASE.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64.0!))
            Me.TP_HEADER_BASE.Size = New System.Drawing.Size(601, 65)
            Me.TP_HEADER_BASE.TabIndex = 6
            '
            'TP_SUBS
            '
            Me.TP_SUBS.ColumnCount = 3
            Me.TP_SUBS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_SUBS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
            Me.TP_SUBS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
            Me.TP_SUBS.Controls.Add(Me.TXT_SUBS, 0, 0)
            Me.TP_SUBS.Controls.Add(LBL_SUBS_FORMAT, 1, 0)
            Me.TP_SUBS.Controls.Add(Me.CMB_SUBS_FORMAT, 2, 0)
            Me.TP_SUBS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_SUBS.Location = New System.Drawing.Point(6, 93)
            Me.TP_SUBS.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
            Me.TP_SUBS.Name = "TP_SUBS"
            Me.TP_SUBS.RowCount = 1
            Me.TP_SUBS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_SUBS.Size = New System.Drawing.Size(589, 28)
            Me.TP_SUBS.TabIndex = 2
            '
            'TXT_SUBS
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Name = "Open"
            ActionButton1.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Open
            ActionButton1.ToolTipText = "Choose subtitles"
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Name = "Refresh"
            ActionButton2.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Refresh
            ActionButton2.ToolTipText = "Reset subtitles to initial selected"
            ActionButton3.BackgroundImage = CType(resources.GetObject("ActionButton3.BackgroundImage"), System.Drawing.Image)
            ActionButton3.Name = "Clear"
            ActionButton3.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            ActionButton3.ToolTipText = "Clear subtitles selection (don't download subtitles)"
            Me.TXT_SUBS.Buttons.Add(ActionButton1)
            Me.TXT_SUBS.Buttons.Add(ActionButton2)
            Me.TXT_SUBS.Buttons.Add(ActionButton3)
            Me.TXT_SUBS.CaptionText = "Subtitles"
            Me.TXT_SUBS.CaptionToolTipEnabled = True
            Me.TXT_SUBS.CaptionToolTipText = "The selected subtitles will also be downloaded"
            Me.TXT_SUBS.CaptionWidth = 60.0R
            Me.TXT_SUBS.ClearTextByButtonClear = False
            Me.TXT_SUBS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_SUBS.Location = New System.Drawing.Point(3, 3)
            Me.TXT_SUBS.Name = "TXT_SUBS"
            Me.TXT_SUBS.Size = New System.Drawing.Size(423, 22)
            Me.TXT_SUBS.TabIndex = 0
            Me.TXT_SUBS.TextBoxReadOnly = True
            '
            'CMB_SUBS_FORMAT
            '
            Me.CMB_SUBS_FORMAT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_SUBS_FORMAT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.CMB_SUBS_FORMAT.FormattingEnabled = True
            Me.CMB_SUBS_FORMAT.Location = New System.Drawing.Point(512, 3)
            Me.CMB_SUBS_FORMAT.Name = "CMB_SUBS_FORMAT"
            Me.CMB_SUBS_FORMAT.Size = New System.Drawing.Size(74, 21)
            Me.CMB_SUBS_FORMAT.TabIndex = 1
            '
            'TP_MAIN
            '
            Me.TP_MAIN.ColumnCount = 1
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.Controls.Add(Me.TP_HEADER_BASE, 0, 0)
            Me.TP_MAIN.Controls.Add(TP_FOOTER, 0, 8)
            Me.TP_MAIN.Controls.Add(Me.TP_OPTIONS, 0, 1)
            Me.TP_MAIN.Controls.Add(Me.TP_CONTROLS, 0, 6)
            Me.TP_MAIN.Controls.Add(LB_SEP_1, 0, 5)
            Me.TP_MAIN.Controls.Add(LB_SEP_2, 0, 7)
            Me.TP_MAIN.Controls.Add(Me.TP_SUBS, 0, 2)
            Me.TP_MAIN.Controls.Add(Me.TXT_SUBS_ADDIT, 0, 3)
            Me.TP_MAIN.Controls.Add(Me.TXT_EXTRA_AUDIO_FORMATS, 0, 4)
            Me.TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.TP_MAIN.Name = "TP_MAIN"
            Me.TP_MAIN.RowCount = 10
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 58.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_MAIN.Size = New System.Drawing.Size(601, 271)
            Me.TP_MAIN.TabIndex = 0
            '
            'TP_OPTIONS
            '
            Me.TP_OPTIONS.ColumnCount = 6
            Me.TP_OPTIONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_OPTIONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
            Me.TP_OPTIONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
            Me.TP_OPTIONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
            Me.TP_OPTIONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
            Me.TP_OPTIONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80.0!))
            Me.TP_OPTIONS.Controls.Add(LBL_FORMAT, 1, 0)
            Me.TP_OPTIONS.Controls.Add(TP_WHAT, 0, 0)
            Me.TP_OPTIONS.Controls.Add(Me.CMB_FORMAT, 2, 0)
            Me.TP_OPTIONS.Controls.Add(Me.LBL_AUDIO_CODEC, 4, 0)
            Me.TP_OPTIONS.Controls.Add(Me.CMB_AUDIO_CODEC, 5, 0)
            Me.TP_OPTIONS.Controls.Add(Me.NUM_RES, 3, 0)
            Me.TP_OPTIONS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_OPTIONS.Location = New System.Drawing.Point(6, 65)
            Me.TP_OPTIONS.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
            Me.TP_OPTIONS.Name = "TP_OPTIONS"
            Me.TP_OPTIONS.RowCount = 1
            Me.TP_OPTIONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_OPTIONS.Size = New System.Drawing.Size(589, 28)
            Me.TP_OPTIONS.TabIndex = 1
            '
            'CMB_FORMAT
            '
            Me.CMB_FORMAT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_FORMAT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.CMB_FORMAT.FormattingEnabled = True
            Me.CMB_FORMAT.Location = New System.Drawing.Point(272, 3)
            Me.CMB_FORMAT.Name = "CMB_FORMAT"
            Me.CMB_FORMAT.Size = New System.Drawing.Size(74, 21)
            Me.CMB_FORMAT.TabIndex = 1
            '
            'CMB_AUDIO_CODEC
            '
            Me.CMB_AUDIO_CODEC.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_AUDIO_CODEC.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.CMB_AUDIO_CODEC.FormattingEnabled = True
            Me.CMB_AUDIO_CODEC.Location = New System.Drawing.Point(512, 3)
            Me.CMB_AUDIO_CODEC.Name = "CMB_AUDIO_CODEC"
            Me.CMB_AUDIO_CODEC.Size = New System.Drawing.Size(74, 21)
            Me.CMB_AUDIO_CODEC.TabIndex = 3
            '
            'NUM_RES
            '
            Me.NUM_RES.Dock = System.Windows.Forms.DockStyle.Fill
            Me.NUM_RES.Location = New System.Drawing.Point(352, 3)
            Me.NUM_RES.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
            Me.NUM_RES.Minimum = New Decimal(New Integer() {1, 0, 0, -2147483648})
            Me.NUM_RES.Name = "NUM_RES"
            Me.NUM_RES.Size = New System.Drawing.Size(74, 20)
            Me.NUM_RES.TabIndex = 2
            Me.NUM_RES.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
            Me.NUM_RES.Value = New Decimal(New Integer() {1080, 0, 0, 0})
            '
            'TP_CONTROLS
            '
            Me.TP_CONTROLS.ColumnCount = 1
            Me.TP_CONTROLS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_CONTROLS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_CONTROLS.Location = New System.Drawing.Point(3, 182)
            Me.TP_CONTROLS.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
            Me.TP_CONTROLS.Name = "TP_CONTROLS"
            Me.TP_CONTROLS.RowCount = 1
            Me.TP_CONTROLS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_CONTROLS.Size = New System.Drawing.Size(595, 25)
            Me.TP_CONTROLS.TabIndex = 0
            '
            'TXT_SUBS_ADDIT
            '
            ActionButton4.BackgroundImage = CType(resources.GetObject("ActionButton4.BackgroundImage"), System.Drawing.Image)
            ActionButton4.Enabled = False
            ActionButton4.Name = "Open"
            ActionButton4.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Open
            ActionButton4.ToolTipText = "Choose additional formats"
            ActionButton5.BackgroundImage = CType(resources.GetObject("ActionButton5.BackgroundImage"), System.Drawing.Image)
            ActionButton5.Enabled = False
            ActionButton5.Name = "Refresh"
            ActionButton5.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Refresh
            ActionButton5.ToolTipText = "Fill in additional formats from the defaults"
            ActionButton6.BackgroundImage = CType(resources.GetObject("ActionButton6.BackgroundImage"), System.Drawing.Image)
            ActionButton6.Enabled = False
            ActionButton6.Name = "Clear"
            ActionButton6.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            ActionButton6.ToolTipText = "Remove all additional formats"
            Me.TXT_SUBS_ADDIT.Buttons.Add(ActionButton4)
            Me.TXT_SUBS_ADDIT.Buttons.Add(ActionButton5)
            Me.TXT_SUBS_ADDIT.Buttons.Add(ActionButton6)
            Me.TXT_SUBS_ADDIT.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
            Me.TXT_SUBS_ADDIT.CaptionText = "Additional subtitle formats"
            Me.TXT_SUBS_ADDIT.CaptionToolTipEnabled = True
            Me.TXT_SUBS_ADDIT.CaptionToolTipText = "Subtitles will be downloaded in 'SRT' format and converted to additional formats"
            Me.TXT_SUBS_ADDIT.CaptionWidth = 150.0R
            Me.TXT_SUBS_ADDIT.ClearTextByButtonClear = False
            Me.TXT_SUBS_ADDIT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_SUBS_ADDIT.Location = New System.Drawing.Point(6, 124)
            Me.TXT_SUBS_ADDIT.Margin = New System.Windows.Forms.Padding(6, 3, 6, 3)
            Me.TXT_SUBS_ADDIT.Name = "TXT_SUBS_ADDIT"
            Me.TXT_SUBS_ADDIT.Size = New System.Drawing.Size(589, 22)
            Me.TXT_SUBS_ADDIT.TabIndex = 3
            Me.TXT_SUBS_ADDIT.Tag = "s"
            Me.TXT_SUBS_ADDIT.TextBoxReadOnly = True
            '
            'TXT_EXTRA_AUDIO_FORMATS
            '
            ActionButton7.BackgroundImage = CType(resources.GetObject("ActionButton7.BackgroundImage"), System.Drawing.Image)
            ActionButton7.Enabled = False
            ActionButton7.Name = "Open"
            ActionButton7.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Open
            ActionButton7.ToolTipText = "Choose additional formats"
            ActionButton8.BackgroundImage = CType(resources.GetObject("ActionButton8.BackgroundImage"), System.Drawing.Image)
            ActionButton8.Enabled = False
            ActionButton8.Name = "Refresh"
            ActionButton8.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Refresh
            ActionButton8.ToolTipText = "Fill in additional formats from the defaults"
            ActionButton9.BackgroundImage = CType(resources.GetObject("ActionButton9.BackgroundImage"), System.Drawing.Image)
            ActionButton9.Enabled = False
            ActionButton9.Name = "Clear"
            ActionButton9.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            ActionButton9.ToolTipText = "Choose additional formats"
            Me.TXT_EXTRA_AUDIO_FORMATS.Buttons.Add(ActionButton7)
            Me.TXT_EXTRA_AUDIO_FORMATS.Buttons.Add(ActionButton8)
            Me.TXT_EXTRA_AUDIO_FORMATS.Buttons.Add(ActionButton9)
            Me.TXT_EXTRA_AUDIO_FORMATS.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
            Me.TXT_EXTRA_AUDIO_FORMATS.CaptionText = "Additional audio formats"
            Me.TXT_EXTRA_AUDIO_FORMATS.CaptionToolTipEnabled = True
            Me.TXT_EXTRA_AUDIO_FORMATS.CaptionWidth = 150.0R
            Me.TXT_EXTRA_AUDIO_FORMATS.ClearTextByButtonClear = False
            Me.TXT_EXTRA_AUDIO_FORMATS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_EXTRA_AUDIO_FORMATS.Location = New System.Drawing.Point(6, 152)
            Me.TXT_EXTRA_AUDIO_FORMATS.Margin = New System.Windows.Forms.Padding(6, 3, 6, 3)
            Me.TXT_EXTRA_AUDIO_FORMATS.Name = "TXT_EXTRA_AUDIO_FORMATS"
            Me.TXT_EXTRA_AUDIO_FORMATS.Size = New System.Drawing.Size(589, 22)
            Me.TXT_EXTRA_AUDIO_FORMATS.TabIndex = 4
            Me.TXT_EXTRA_AUDIO_FORMATS.Tag = "a"
            Me.TXT_EXTRA_AUDIO_FORMATS.TextBoxReadOnly = True
            '
            'VideoOptionsForm
            '
            Me.AcceptButton = Me.BTT_DOWN
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.BTT_CANCEL
            Me.ClientSize = New System.Drawing.Size(601, 271)
            Me.Controls.Add(Me.TP_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Icon = Global.SCrawler.My.Resources.SiteYouTube.YouTubeIcon_32
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "VideoOptionsForm"
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Download video"
            TP_HEADER.ResumeLayout(False)
            CType(Me.ICON_VIDEO, System.ComponentModel.ISupportInitialize).EndInit()
            TP_HEADER_INFO.ResumeLayout(False)
            Me.TP_HEADER_INFO_2.ResumeLayout(False)
            Me.TP_HEADER_INFO_2.PerformLayout()
            CType(ICON_CLOCK, System.ComponentModel.ISupportInitialize).EndInit()
            CType(ICON_LINK, System.ComponentModel.ISupportInitialize).EndInit()
            TP_FOOTER.ResumeLayout(False)
            TP_DESTINATION.ResumeLayout(False)
            TP_DESTINATION.PerformLayout()
            TP_OK_CANCEL.ResumeLayout(False)
            TP_WHAT.ResumeLayout(False)
            TP_WHAT.PerformLayout()
            Me.TP_HEADER_BASE.ResumeLayout(False)
            Me.TP_SUBS.ResumeLayout(False)
            Me.TP_SUBS.PerformLayout()
            CType(Me.TXT_SUBS, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TP_MAIN.ResumeLayout(False)
            Me.TP_OPTIONS.ResumeLayout(False)
            Me.TP_OPTIONS.PerformLayout()
            CType(Me.NUM_RES, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_SUBS_ADDIT, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_EXTRA_AUDIO_FORMATS, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents ICON_VIDEO As PictureBox
        Private WithEvents LBL_TITLE As Label
        Private WithEvents LBL_TIME As Label
        Private WithEvents LBL_URL As LinkLabel
        Private WithEvents TP_OPTIONS As TableLayoutPanel
        Private WithEvents LBL_AUDIO_CODEC As Label
        Private WithEvents TP_SUBS As TableLayoutPanel
        Private WithEvents NUM_RES As NumericUpDown
        Private WithEvents TP_HEADER_BASE As TableLayoutPanel
        Private WithEvents TP_CONTROLS As TableLayoutPanel
        Private WithEvents TP_MAIN As TableLayoutPanel
        Private WithEvents CMB_AUDIO_CODEC As ComboBox
        Private WithEvents OPT_VIDEO As RadioButton
        Private WithEvents OPT_AUDIO As RadioButton
        Private WithEvents CMB_FORMAT As ComboBox
        Private WithEvents TXT_SUBS As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CMB_SUBS_FORMAT As ComboBox
        Private WithEvents TXT_SUBS_ADDIT As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_EXTRA_AUDIO_FORMATS As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_FILE As TextBox
        Private WithEvents BTT_BROWSE As Button
        Private WithEvents BTT_DOWN As Button
        Private WithEvents BTT_CANCEL As Button
        Private WithEvents TP_HEADER_INFO_2 As TableLayoutPanel
    End Class
End Namespace