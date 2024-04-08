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
    Partial Friend Class MusicPlaylistsForm : Inherits System.Windows.Forms.Form
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
            Dim TP_MAIN As System.Windows.Forms.TableLayoutPanel
            Dim TP_BUTTONS As System.Windows.Forms.TableLayoutPanel
            Dim TP_PLS As System.Windows.Forms.TableLayoutPanel
            Dim TP_PLS_BUTTONS As System.Windows.Forms.TableLayoutPanel
            Dim TP_PLS_ITEMS As System.Windows.Forms.TableLayoutPanel
            Dim TP_SETTINGS As System.Windows.Forms.TableLayoutPanel
            Dim TP_FORMATS As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MusicPlaylistsForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim LBL_FORMAT As System.Windows.Forms.Label
            Dim TP_LYRICS As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton4 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton5 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton6 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton7 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton8 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton9 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton10 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton11 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ListColumn1 As PersonalUtilities.Forms.Controls.Base.ListColumn = New PersonalUtilities.Forms.Controls.Base.ListColumn()
            Dim ListColumn2 As PersonalUtilities.Forms.Controls.Base.ListColumn = New PersonalUtilities.Forms.Controls.Base.ListColumn()
            Dim ActionButton12 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton13 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton14 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton15 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton16 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton17 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton18 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim TT_MAIN As System.Windows.Forms.ToolTip
            Me.BTT_DOWN = New System.Windows.Forms.Button()
            Me.BTT_CANCEL = New System.Windows.Forms.Button()
            Me.SPLITTER_MAIN = New System.Windows.Forms.SplitContainer()
            Me.LIST_PLAYLISTS = New System.Windows.Forms.CheckedListBox()
            Me.BTT_PLS_ALL = New System.Windows.Forms.Button()
            Me.BTT_PLS_NONE = New System.Windows.Forms.Button()
            Me.LIST_ITEMS = New System.Windows.Forms.CheckedListBox()
            Me.TXT_FORMATS_ADDIT = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CMB_FORMATS = New System.Windows.Forms.ComboBox()
            Me.TXT_SUBS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_DOWN_LYRICS = New System.Windows.Forms.CheckBox()
            Me.TXT_OUTPUT_PATH = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.CMB_PLS = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.TXT_AUDIO_BITRATE = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            TP_BUTTONS = New System.Windows.Forms.TableLayoutPanel()
            TP_PLS = New System.Windows.Forms.TableLayoutPanel()
            TP_PLS_BUTTONS = New System.Windows.Forms.TableLayoutPanel()
            TP_PLS_ITEMS = New System.Windows.Forms.TableLayoutPanel()
            TP_SETTINGS = New System.Windows.Forms.TableLayoutPanel()
            TP_FORMATS = New System.Windows.Forms.TableLayoutPanel()
            LBL_FORMAT = New System.Windows.Forms.Label()
            TP_LYRICS = New System.Windows.Forms.TableLayoutPanel()
            TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
            TP_MAIN.SuspendLayout()
            TP_BUTTONS.SuspendLayout()
            CType(Me.SPLITTER_MAIN, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SPLITTER_MAIN.Panel1.SuspendLayout()
            Me.SPLITTER_MAIN.Panel2.SuspendLayout()
            Me.SPLITTER_MAIN.SuspendLayout()
            TP_PLS.SuspendLayout()
            TP_PLS_BUTTONS.SuspendLayout()
            TP_PLS_ITEMS.SuspendLayout()
            TP_SETTINGS.SuspendLayout()
            TP_FORMATS.SuspendLayout()
            CType(Me.TXT_FORMATS_ADDIT, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_LYRICS.SuspendLayout()
            CType(Me.TXT_SUBS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_OUTPUT_PATH, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.CMB_PLS, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_AUDIO_BITRATE, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'TP_MAIN
            '
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Controls.Add(TP_BUTTONS, 0, 2)
            TP_MAIN.Controls.Add(Me.SPLITTER_MAIN, 0, 1)
            TP_MAIN.Controls.Add(TP_SETTINGS, 0, 0)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Margin = New System.Windows.Forms.Padding(0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 3
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.Size = New System.Drawing.Size(434, 317)
            TP_MAIN.TabIndex = 0
            '
            'TP_BUTTONS
            '
            TP_BUTTONS.ColumnCount = 3
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
            TP_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100.0!))
            TP_BUTTONS.Controls.Add(Me.BTT_DOWN, 1, 0)
            TP_BUTTONS.Controls.Add(Me.BTT_CANCEL, 2, 0)
            TP_BUTTONS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_BUTTONS.Location = New System.Drawing.Point(0, 292)
            TP_BUTTONS.Margin = New System.Windows.Forms.Padding(0)
            TP_BUTTONS.Name = "TP_BUTTONS"
            TP_BUTTONS.RowCount = 1
            TP_BUTTONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_BUTTONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_BUTTONS.Size = New System.Drawing.Size(434, 25)
            TP_BUTTONS.TabIndex = 1
            '
            'BTT_DOWN
            '
            Me.BTT_DOWN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_DOWN.Location = New System.Drawing.Point(236, 2)
            Me.BTT_DOWN.Margin = New System.Windows.Forms.Padding(2)
            Me.BTT_DOWN.Name = "BTT_DOWN"
            Me.BTT_DOWN.Size = New System.Drawing.Size(96, 21)
            Me.BTT_DOWN.TabIndex = 0
            Me.BTT_DOWN.Text = "Download"
            Me.BTT_DOWN.UseVisualStyleBackColor = True
            '
            'BTT_CANCEL
            '
            Me.BTT_CANCEL.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.BTT_CANCEL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_CANCEL.Location = New System.Drawing.Point(336, 2)
            Me.BTT_CANCEL.Margin = New System.Windows.Forms.Padding(2)
            Me.BTT_CANCEL.Name = "BTT_CANCEL"
            Me.BTT_CANCEL.Size = New System.Drawing.Size(96, 21)
            Me.BTT_CANCEL.TabIndex = 1
            Me.BTT_CANCEL.Text = "Cancel"
            Me.BTT_CANCEL.UseVisualStyleBackColor = True
            '
            'SPLITTER_MAIN
            '
            Me.SPLITTER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.SPLITTER_MAIN.Location = New System.Drawing.Point(3, 143)
            Me.SPLITTER_MAIN.Name = "SPLITTER_MAIN"
            '
            'SPLITTER_MAIN.Panel1
            '
            Me.SPLITTER_MAIN.Panel1.Controls.Add(TP_PLS)
            Me.SPLITTER_MAIN.Panel1MinSize = 110
            '
            'SPLITTER_MAIN.Panel2
            '
            Me.SPLITTER_MAIN.Panel2.Controls.Add(TP_PLS_ITEMS)
            Me.SPLITTER_MAIN.Size = New System.Drawing.Size(428, 146)
            Me.SPLITTER_MAIN.SplitterDistance = 142
            Me.SPLITTER_MAIN.TabIndex = 0
            '
            'TP_PLS
            '
            TP_PLS.ColumnCount = 1
            TP_PLS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_PLS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_PLS.Controls.Add(Me.LIST_PLAYLISTS, 0, 0)
            TP_PLS.Controls.Add(TP_PLS_BUTTONS, 0, 1)
            TP_PLS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_PLS.Location = New System.Drawing.Point(0, 0)
            TP_PLS.Margin = New System.Windows.Forms.Padding(0)
            TP_PLS.Name = "TP_PLS"
            TP_PLS.RowCount = 2
            TP_PLS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_PLS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_PLS.Size = New System.Drawing.Size(142, 146)
            TP_PLS.TabIndex = 0
            '
            'LIST_PLAYLISTS
            '
            Me.LIST_PLAYLISTS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LIST_PLAYLISTS.FormattingEnabled = True
            Me.LIST_PLAYLISTS.Location = New System.Drawing.Point(3, 3)
            Me.LIST_PLAYLISTS.Name = "LIST_PLAYLISTS"
            Me.LIST_PLAYLISTS.Size = New System.Drawing.Size(136, 115)
            Me.LIST_PLAYLISTS.TabIndex = 0
            Me.LIST_PLAYLISTS.ThreeDCheckBoxes = True
            '
            'TP_PLS_BUTTONS
            '
            TP_PLS_BUTTONS.ColumnCount = 3
            TP_PLS_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
            TP_PLS_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
            TP_PLS_BUTTONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_PLS_BUTTONS.Controls.Add(Me.BTT_PLS_ALL, 0, 0)
            TP_PLS_BUTTONS.Controls.Add(Me.BTT_PLS_NONE, 1, 0)
            TP_PLS_BUTTONS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_PLS_BUTTONS.Location = New System.Drawing.Point(0, 121)
            TP_PLS_BUTTONS.Margin = New System.Windows.Forms.Padding(0)
            TP_PLS_BUTTONS.Name = "TP_PLS_BUTTONS"
            TP_PLS_BUTTONS.RowCount = 1
            TP_PLS_BUTTONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_PLS_BUTTONS.Size = New System.Drawing.Size(142, 25)
            TP_PLS_BUTTONS.TabIndex = 1
            '
            'BTT_PLS_ALL
            '
            Me.BTT_PLS_ALL.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_PLS_ALL.Location = New System.Drawing.Point(2, 2)
            Me.BTT_PLS_ALL.Margin = New System.Windows.Forms.Padding(2)
            Me.BTT_PLS_ALL.Name = "BTT_PLS_ALL"
            Me.BTT_PLS_ALL.Size = New System.Drawing.Size(46, 21)
            Me.BTT_PLS_ALL.TabIndex = 0
            Me.BTT_PLS_ALL.Tag = "a"
            Me.BTT_PLS_ALL.Text = "All"
            TT_MAIN.SetToolTip(Me.BTT_PLS_ALL, "Select all")
            Me.BTT_PLS_ALL.UseVisualStyleBackColor = True
            '
            'BTT_PLS_NONE
            '
            Me.BTT_PLS_NONE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.BTT_PLS_NONE.Location = New System.Drawing.Point(52, 2)
            Me.BTT_PLS_NONE.Margin = New System.Windows.Forms.Padding(2)
            Me.BTT_PLS_NONE.Name = "BTT_PLS_NONE"
            Me.BTT_PLS_NONE.Size = New System.Drawing.Size(46, 21)
            Me.BTT_PLS_NONE.TabIndex = 1
            Me.BTT_PLS_NONE.Tag = "n"
            Me.BTT_PLS_NONE.Text = "None"
            TT_MAIN.SetToolTip(Me.BTT_PLS_NONE, "Select none")
            Me.BTT_PLS_NONE.UseVisualStyleBackColor = True
            '
            'TP_PLS_ITEMS
            '
            TP_PLS_ITEMS.ColumnCount = 1
            TP_PLS_ITEMS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_PLS_ITEMS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_PLS_ITEMS.Controls.Add(Me.LIST_ITEMS, 0, 0)
            TP_PLS_ITEMS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_PLS_ITEMS.Location = New System.Drawing.Point(0, 0)
            TP_PLS_ITEMS.Margin = New System.Windows.Forms.Padding(0)
            TP_PLS_ITEMS.Name = "TP_PLS_ITEMS"
            TP_PLS_ITEMS.RowCount = 2
            TP_PLS_ITEMS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_PLS_ITEMS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_PLS_ITEMS.Size = New System.Drawing.Size(282, 146)
            TP_PLS_ITEMS.TabIndex = 1
            '
            'LIST_ITEMS
            '
            Me.LIST_ITEMS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LIST_ITEMS.FormattingEnabled = True
            Me.LIST_ITEMS.Location = New System.Drawing.Point(3, 3)
            Me.LIST_ITEMS.Name = "LIST_ITEMS"
            Me.LIST_ITEMS.Size = New System.Drawing.Size(276, 115)
            Me.LIST_ITEMS.TabIndex = 0
            '
            'TP_SETTINGS
            '
            TP_SETTINGS.ColumnCount = 1
            TP_SETTINGS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_SETTINGS.Controls.Add(TP_FORMATS, 0, 1)
            TP_SETTINGS.Controls.Add(TP_LYRICS, 0, 0)
            TP_SETTINGS.Controls.Add(Me.TXT_OUTPUT_PATH, 0, 3)
            TP_SETTINGS.Controls.Add(Me.CMB_PLS, 0, 4)
            TP_SETTINGS.Controls.Add(Me.TXT_AUDIO_BITRATE, 0, 2)
            TP_SETTINGS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_SETTINGS.Location = New System.Drawing.Point(0, 0)
            TP_SETTINGS.Margin = New System.Windows.Forms.Padding(0)
            TP_SETTINGS.Name = "TP_SETTINGS"
            TP_SETTINGS.RowCount = 5
            TP_SETTINGS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_SETTINGS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_SETTINGS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_SETTINGS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_SETTINGS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            TP_SETTINGS.Size = New System.Drawing.Size(434, 140)
            TP_SETTINGS.TabIndex = 1
            '
            'TP_FORMATS
            '
            TP_FORMATS.ColumnCount = 3
            TP_FORMATS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
            TP_FORMATS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65.0!))
            TP_FORMATS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_FORMATS.Controls.Add(Me.TXT_FORMATS_ADDIT, 2, 0)
            TP_FORMATS.Controls.Add(LBL_FORMAT, 0, 0)
            TP_FORMATS.Controls.Add(Me.CMB_FORMATS, 1, 0)
            TP_FORMATS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_FORMATS.Location = New System.Drawing.Point(0, 28)
            TP_FORMATS.Margin = New System.Windows.Forms.Padding(0)
            TP_FORMATS.Name = "TP_FORMATS"
            TP_FORMATS.RowCount = 1
            TP_FORMATS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_FORMATS.Size = New System.Drawing.Size(434, 28)
            TP_FORMATS.TabIndex = 5
            '
            'TXT_FORMATS_ADDIT
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Enabled = False
            ActionButton1.Name = "Open"
            ActionButton1.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Open
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Enabled = False
            ActionButton2.Name = "Refresh"
            ActionButton2.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Refresh
            ActionButton3.BackgroundImage = CType(resources.GetObject("ActionButton3.BackgroundImage"), System.Drawing.Image)
            ActionButton3.Enabled = False
            ActionButton3.Name = "Clear"
            ActionButton3.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_FORMATS_ADDIT.Buttons.Add(ActionButton1)
            Me.TXT_FORMATS_ADDIT.Buttons.Add(ActionButton2)
            Me.TXT_FORMATS_ADDIT.Buttons.Add(ActionButton3)
            Me.TXT_FORMATS_ADDIT.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
            Me.TXT_FORMATS_ADDIT.CaptionText = "Additional formats"
            Me.TXT_FORMATS_ADDIT.CaptionToolTipEnabled = True
            Me.TXT_FORMATS_ADDIT.CaptionToolTipText = "Convert every downloaded track to the formats you choose."
            Me.TXT_FORMATS_ADDIT.CaptionWidth = 115.0R
            Me.TXT_FORMATS_ADDIT.ClearTextByButtonClear = False
            Me.TXT_FORMATS_ADDIT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_FORMATS_ADDIT.Location = New System.Drawing.Point(118, 3)
            Me.TXT_FORMATS_ADDIT.Name = "TXT_FORMATS_ADDIT"
            Me.TXT_FORMATS_ADDIT.Size = New System.Drawing.Size(313, 22)
            Me.TXT_FORMATS_ADDIT.TabIndex = 2
            Me.TXT_FORMATS_ADDIT.Tag = "a"
            Me.TXT_FORMATS_ADDIT.TextBoxReadOnly = True
            '
            'LBL_FORMAT
            '
            LBL_FORMAT.Dock = System.Windows.Forms.DockStyle.Fill
            LBL_FORMAT.Location = New System.Drawing.Point(3, 0)
            LBL_FORMAT.Margin = New System.Windows.Forms.Padding(3, 0, 0, 0)
            LBL_FORMAT.Name = "LBL_FORMAT"
            LBL_FORMAT.Size = New System.Drawing.Size(47, 28)
            LBL_FORMAT.TabIndex = 0
            LBL_FORMAT.Text = "Format:"
            LBL_FORMAT.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            TT_MAIN.SetToolTip(LBL_FORMAT, "Output files format")
            '
            'CMB_FORMATS
            '
            Me.CMB_FORMATS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_FORMATS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.CMB_FORMATS.FormattingEnabled = True
            Me.CMB_FORMATS.Location = New System.Drawing.Point(53, 3)
            Me.CMB_FORMATS.Name = "CMB_FORMATS"
            Me.CMB_FORMATS.Size = New System.Drawing.Size(59, 21)
            Me.CMB_FORMATS.TabIndex = 1
            '
            'TP_LYRICS
            '
            TP_LYRICS.ColumnCount = 2
            TP_LYRICS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 115.0!))
            TP_LYRICS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_LYRICS.Controls.Add(Me.TXT_SUBS, 1, 0)
            TP_LYRICS.Controls.Add(Me.CH_DOWN_LYRICS, 0, 0)
            TP_LYRICS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_LYRICS.Location = New System.Drawing.Point(0, 0)
            TP_LYRICS.Margin = New System.Windows.Forms.Padding(0)
            TP_LYRICS.Name = "TP_LYRICS"
            TP_LYRICS.RowCount = 1
            TP_LYRICS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_LYRICS.Size = New System.Drawing.Size(434, 28)
            TP_LYRICS.TabIndex = 6
            '
            'TXT_SUBS
            '
            ActionButton4.BackgroundImage = CType(resources.GetObject("ActionButton4.BackgroundImage"), System.Drawing.Image)
            ActionButton4.Enabled = False
            ActionButton4.Name = "Open"
            ActionButton4.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Open
            ActionButton5.BackgroundImage = CType(resources.GetObject("ActionButton5.BackgroundImage"), System.Drawing.Image)
            ActionButton5.Enabled = False
            ActionButton5.Name = "Refresh"
            ActionButton5.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Refresh
            ActionButton6.BackgroundImage = CType(resources.GetObject("ActionButton6.BackgroundImage"), System.Drawing.Image)
            ActionButton6.Enabled = False
            ActionButton6.Name = "Clear"
            ActionButton6.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_SUBS.Buttons.Add(ActionButton4)
            Me.TXT_SUBS.Buttons.Add(ActionButton5)
            Me.TXT_SUBS.Buttons.Add(ActionButton6)
            Me.TXT_SUBS.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
            Me.TXT_SUBS.CaptionText = "Additional lyrics"
            Me.TXT_SUBS.CaptionToolTipEnabled = True
            Me.TXT_SUBS.CaptionToolTipText = "Convert all downloaded lyrics to the formats you choose."
            Me.TXT_SUBS.CaptionWidth = 115.0R
            Me.TXT_SUBS.ClearTextByButtonClear = False
            Me.TXT_SUBS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_SUBS.Location = New System.Drawing.Point(118, 3)
            Me.TXT_SUBS.Name = "TXT_SUBS"
            Me.TXT_SUBS.Size = New System.Drawing.Size(313, 22)
            Me.TXT_SUBS.TabIndex = 1
            Me.TXT_SUBS.Tag = "s"
            Me.TXT_SUBS.TextBoxReadOnly = True
            '
            'CH_DOWN_LYRICS
            '
            Me.CH_DOWN_LYRICS.AutoSize = True
            Me.CH_DOWN_LYRICS.CheckAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.CH_DOWN_LYRICS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DOWN_LYRICS.Location = New System.Drawing.Point(3, 3)
            Me.CH_DOWN_LYRICS.Name = "CH_DOWN_LYRICS"
            Me.CH_DOWN_LYRICS.Size = New System.Drawing.Size(109, 22)
            Me.CH_DOWN_LYRICS.TabIndex = 0
            Me.CH_DOWN_LYRICS.Text = "Download lyrics"
            Me.CH_DOWN_LYRICS.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            TT_MAIN.SetToolTip(Me.CH_DOWN_LYRICS, "Download lyrics if available")
            Me.CH_DOWN_LYRICS.UseVisualStyleBackColor = True
            '
            'TXT_OUTPUT_PATH
            '
            ActionButton7.BackgroundImage = CType(resources.GetObject("ActionButton7.BackgroundImage"), System.Drawing.Image)
            ActionButton7.Name = "Save"
            ActionButton7.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Save
            ActionButton7.ToolTipText = "Save destination"
            ActionButton8.BackgroundImage = CType(resources.GetObject("ActionButton8.BackgroundImage"), System.Drawing.Image)
            ActionButton8.Name = "Open"
            ActionButton8.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Open
            ActionButton8.ToolTipText = "Choose a new location (Ctrl+O)"
            ActionButton9.BackgroundImage = CType(resources.GetObject("ActionButton9.BackgroundImage"), System.Drawing.Image)
            ActionButton9.Name = "Add"
            ActionButton9.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Add
            ActionButton9.ToolTipText = "Choose a new location and add it to the list (Alt+O)"
            ActionButton10.BackgroundImage = CType(resources.GetObject("ActionButton10.BackgroundImage"), System.Drawing.Image)
            ActionButton10.Name = "Clear"
            ActionButton10.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            ActionButton11.BackgroundImage = CType(resources.GetObject("ActionButton11.BackgroundImage"), System.Drawing.Image)
            ActionButton11.Name = "ArrowDown"
            ActionButton11.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.ArrowDown
            Me.TXT_OUTPUT_PATH.Buttons.Add(ActionButton7)
            Me.TXT_OUTPUT_PATH.Buttons.Add(ActionButton8)
            Me.TXT_OUTPUT_PATH.Buttons.Add(ActionButton9)
            Me.TXT_OUTPUT_PATH.Buttons.Add(ActionButton10)
            Me.TXT_OUTPUT_PATH.Buttons.Add(ActionButton11)
            Me.TXT_OUTPUT_PATH.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
            Me.TXT_OUTPUT_PATH.CaptionText = "Output path"
            Me.TXT_OUTPUT_PATH.CaptionToolTipEnabled = True
            Me.TXT_OUTPUT_PATH.CaptionToolTipText = "If this checkbox is selected, this path is absolute and artist folder will not be" &
    " created in it"
            Me.TXT_OUTPUT_PATH.CaptionVisible = True
            Me.TXT_OUTPUT_PATH.CaptionWidth = 112.0R
            Me.TXT_OUTPUT_PATH.ChangeControlsEnableOnCheckedChange = False
            ListColumn1.Name = "COL_NAME"
            ListColumn1.Text = "Name"
            ListColumn1.Width = -1
            ListColumn2.DisplayMember = True
            ListColumn2.Name = "COL_VALUE"
            ListColumn2.Text = "Value"
            ListColumn2.ValueMember = True
            ListColumn2.Visible = False
            Me.TXT_OUTPUT_PATH.Columns.Add(ListColumn1)
            Me.TXT_OUTPUT_PATH.Columns.Add(ListColumn2)
            Me.TXT_OUTPUT_PATH.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_OUTPUT_PATH.Location = New System.Drawing.Point(3, 87)
            Me.TXT_OUTPUT_PATH.Name = "TXT_OUTPUT_PATH"
            Me.TXT_OUTPUT_PATH.Size = New System.Drawing.Size(428, 22)
            Me.TXT_OUTPUT_PATH.TabIndex = 2
            Me.TXT_OUTPUT_PATH.TextBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            '
            'CMB_PLS
            '
            ActionButton12.BackgroundImage = CType(resources.GetObject("ActionButton12.BackgroundImage"), System.Drawing.Image)
            ActionButton12.Name = "Save"
            ActionButton12.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Save
            ActionButton12.ToolTipText = "Save playlist"
            ActionButton13.BackgroundImage = CType(resources.GetObject("ActionButton13.BackgroundImage"), System.Drawing.Image)
            ActionButton13.Name = "List"
            ActionButton13.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.List
            ActionButton13.ToolTipText = "Select multiple playlists"
            ActionButton14.BackgroundImage = CType(resources.GetObject("ActionButton14.BackgroundImage"), System.Drawing.Image)
            ActionButton14.Name = "Open"
            ActionButton14.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Open
            ActionButton14.ToolTipText = "Choose an output file"
            ActionButton15.BackgroundImage = CType(resources.GetObject("ActionButton15.BackgroundImage"), System.Drawing.Image)
            ActionButton15.Name = "Add"
            ActionButton15.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Add
            ActionButton15.ToolTipText = "Choose an output file (add a new location to the list)"
            ActionButton16.BackgroundImage = CType(resources.GetObject("ActionButton16.BackgroundImage"), System.Drawing.Image)
            ActionButton16.Name = "Clear"
            ActionButton16.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            ActionButton17.BackgroundImage = CType(resources.GetObject("ActionButton17.BackgroundImage"), System.Drawing.Image)
            ActionButton17.Name = "ArrowDown"
            ActionButton17.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.ArrowDown
            Me.CMB_PLS.Buttons.Add(ActionButton12)
            Me.CMB_PLS.Buttons.Add(ActionButton13)
            Me.CMB_PLS.Buttons.Add(ActionButton14)
            Me.CMB_PLS.Buttons.Add(ActionButton15)
            Me.CMB_PLS.Buttons.Add(ActionButton16)
            Me.CMB_PLS.Buttons.Add(ActionButton17)
            Me.CMB_PLS.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.Label
            Me.CMB_PLS.CaptionText = "Playlist"
            Me.CMB_PLS.CaptionToolTipEnabled = True
            Me.CMB_PLS.CaptionToolTipText = "Add downloaded item(s) to playlist"
            Me.CMB_PLS.CaptionVisible = True
            Me.CMB_PLS.CaptionWidth = 50.0R
            Me.CMB_PLS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_PLS.Location = New System.Drawing.Point(3, 115)
            Me.CMB_PLS.Name = "CMB_PLS"
            Me.CMB_PLS.Size = New System.Drawing.Size(428, 22)
            Me.CMB_PLS.TabIndex = 3
            Me.CMB_PLS.TextBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            '
            'TXT_AUDIO_BITRATE
            '
            ActionButton18.BackgroundImage = CType(resources.GetObject("ActionButton18.BackgroundImage"), System.Drawing.Image)
            ActionButton18.Name = "Clear"
            ActionButton18.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_AUDIO_BITRATE.Buttons.Add(ActionButton18)
            Me.TXT_AUDIO_BITRATE.CaptionText = "Audio bitrate"
            Me.TXT_AUDIO_BITRATE.CaptionToolTipEnabled = True
            Me.TXT_AUDIO_BITRATE.CaptionToolTipText = "Default audio bitrate if you want to change it during download"
            Me.TXT_AUDIO_BITRATE.CaptionWidth = 112.0R
            Me.TXT_AUDIO_BITRATE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_AUDIO_BITRATE.Location = New System.Drawing.Point(3, 59)
            Me.TXT_AUDIO_BITRATE.Name = "TXT_AUDIO_BITRATE"
            Me.TXT_AUDIO_BITRATE.Size = New System.Drawing.Size(428, 22)
            Me.TXT_AUDIO_BITRATE.TabIndex = 4
            '
            'MusicPlaylistsForm
            '
            Me.AcceptButton = Me.BTT_DOWN
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.BTT_CANCEL
            Me.ClientSize = New System.Drawing.Size(434, 317)
            Me.Controls.Add(TP_MAIN)
            Me.Icon = Global.SCrawler.My.Resources.SiteYouTube.YouTubeMusicIcon_32
            Me.KeyPreview = True
            Me.MinimumSize = New System.Drawing.Size(450, 356)
            Me.Name = "MusicPlaylistsForm"
            Me.Text = "Albums"
            TP_MAIN.ResumeLayout(False)
            TP_BUTTONS.ResumeLayout(False)
            Me.SPLITTER_MAIN.Panel1.ResumeLayout(False)
            Me.SPLITTER_MAIN.Panel2.ResumeLayout(False)
            CType(Me.SPLITTER_MAIN, System.ComponentModel.ISupportInitialize).EndInit()
            Me.SPLITTER_MAIN.ResumeLayout(False)
            TP_PLS.ResumeLayout(False)
            TP_PLS_BUTTONS.ResumeLayout(False)
            TP_PLS_ITEMS.ResumeLayout(False)
            TP_SETTINGS.ResumeLayout(False)
            TP_FORMATS.ResumeLayout(False)
            CType(Me.TXT_FORMATS_ADDIT, System.ComponentModel.ISupportInitialize).EndInit()
            TP_LYRICS.ResumeLayout(False)
            TP_LYRICS.PerformLayout()
            CType(Me.TXT_SUBS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_OUTPUT_PATH, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.CMB_PLS, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_AUDIO_BITRATE, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents BTT_DOWN As Button
        Private WithEvents BTT_CANCEL As Button
        Private WithEvents LIST_PLAYLISTS As CheckedListBox
        Private WithEvents LIST_ITEMS As CheckedListBox
        Private WithEvents TXT_SUBS As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents BTT_PLS_ALL As Button
        Private WithEvents BTT_PLS_NONE As Button
        Private WithEvents TXT_FORMATS_ADDIT As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents CMB_FORMATS As ComboBox
        Private WithEvents SPLITTER_MAIN As SplitContainer
        Private WithEvents CH_DOWN_LYRICS As CheckBox
        Private WithEvents TXT_OUTPUT_PATH As PersonalUtilities.Forms.Controls.ComboBoxExtended
        Private WithEvents CMB_PLS As PersonalUtilities.Forms.Controls.ComboBoxExtended
        Private WithEvents TXT_AUDIO_BITRATE As PersonalUtilities.Forms.Controls.TextBoxExtended
    End Class
End Namespace