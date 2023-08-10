' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace Editors
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class GlobalLocationsChooserForm : Inherits System.Windows.Forms.Form
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
            Dim CONTAINER_MAIN As System.Windows.Forms.ToolStripContainer
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GlobalLocationsChooserForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim TP_LOCATIONS_USER As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton4 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Me.TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            Me.CMB_LOCATIONS = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.FRM_LOCATIONS = New System.Windows.Forms.GroupBox()
            Me.OPT_LOCATION_1 = New System.Windows.Forms.RadioButton()
            Me.OPT_LOCATION_2 = New System.Windows.Forms.RadioButton()
            Me.OPT_LOCATION_3 = New System.Windows.Forms.RadioButton()
            Me.TXT_COL_NAME = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_LOCATIONS_USER = New System.Windows.Forms.TableLayoutPanel()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            Me.TP_MAIN.SuspendLayout()
            CType(Me.CMB_LOCATIONS, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.FRM_LOCATIONS.SuspendLayout()
            TP_LOCATIONS_USER.SuspendLayout()
            CType(Me.TXT_COL_NAME, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(Me.TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(584, 251)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(584, 251)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            Me.TP_MAIN.ColumnCount = 1
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.Controls.Add(Me.CMB_LOCATIONS, 0, 0)
            Me.TP_MAIN.Controls.Add(Me.FRM_LOCATIONS, 0, 2)
            Me.TP_MAIN.Controls.Add(Me.TXT_COL_NAME, 0, 1)
            Me.TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.TP_MAIN.Name = "TP_MAIN"
            Me.TP_MAIN.RowCount = 3
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.Size = New System.Drawing.Size(584, 251)
            Me.TP_MAIN.TabIndex = 0
            '
            'CMB_LOCATIONS
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Name = "Open"
            ActionButton1.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Open
            ActionButton1.ToolTipText = "Choose a new location (Ctrl+O)"
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Name = "Clear"
            ActionButton2.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            ActionButton3.BackgroundImage = CType(resources.GetObject("ActionButton3.BackgroundImage"), System.Drawing.Image)
            ActionButton3.Name = "ArrowDown"
            ActionButton3.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.ArrowDown
            Me.CMB_LOCATIONS.Buttons.Add(ActionButton1)
            Me.CMB_LOCATIONS.Buttons.Add(ActionButton2)
            Me.CMB_LOCATIONS.Buttons.Add(ActionButton3)
            Me.CMB_LOCATIONS.CaptionChecked = True
            Me.CMB_LOCATIONS.CaptionMode = PersonalUtilities.Forms.Controls.Base.ICaptionControl.Modes.CheckBox
            Me.CMB_LOCATIONS.CaptionText = "Global path"
            Me.CMB_LOCATIONS.CaptionToolTipEnabled = True
            Me.CMB_LOCATIONS.CaptionToolTipText = "If checked, the path will be added to the global paths"
            Me.CMB_LOCATIONS.CaptionVisible = True
            Me.CMB_LOCATIONS.ChangeControlsEnableOnCheckedChange = False
            Me.CMB_LOCATIONS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_LOCATIONS.Lines = New String(-1) {}
            Me.CMB_LOCATIONS.Location = New System.Drawing.Point(3, 3)
            Me.CMB_LOCATIONS.Name = "CMB_LOCATIONS"
            Me.CMB_LOCATIONS.Size = New System.Drawing.Size(578, 22)
            Me.CMB_LOCATIONS.TabIndex = 0
            Me.CMB_LOCATIONS.TextBoxBorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            '
            'FRM_LOCATIONS
            '
            Me.FRM_LOCATIONS.Controls.Add(TP_LOCATIONS_USER)
            Me.FRM_LOCATIONS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.FRM_LOCATIONS.Location = New System.Drawing.Point(3, 59)
            Me.FRM_LOCATIONS.Name = "FRM_LOCATIONS"
            Me.FRM_LOCATIONS.Size = New System.Drawing.Size(578, 189)
            Me.FRM_LOCATIONS.TabIndex = 2
            Me.FRM_LOCATIONS.TabStop = False
            Me.FRM_LOCATIONS.Text = "Locations"
            '
            'TP_LOCATIONS_USER
            '
            TP_LOCATIONS_USER.ColumnCount = 1
            TP_LOCATIONS_USER.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_LOCATIONS_USER.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_LOCATIONS_USER.Controls.Add(Me.OPT_LOCATION_1, 0, 0)
            TP_LOCATIONS_USER.Controls.Add(Me.OPT_LOCATION_2, 0, 1)
            TP_LOCATIONS_USER.Controls.Add(Me.OPT_LOCATION_3, 0, 2)
            TP_LOCATIONS_USER.Dock = System.Windows.Forms.DockStyle.Fill
            TP_LOCATIONS_USER.Location = New System.Drawing.Point(3, 16)
            TP_LOCATIONS_USER.Margin = New System.Windows.Forms.Padding(0)
            TP_LOCATIONS_USER.Name = "TP_LOCATIONS_USER"
            TP_LOCATIONS_USER.RowCount = 3
            TP_LOCATIONS_USER.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_LOCATIONS_USER.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_LOCATIONS_USER.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            TP_LOCATIONS_USER.Size = New System.Drawing.Size(572, 170)
            TP_LOCATIONS_USER.TabIndex = 0
            '
            'OPT_LOCATION_1
            '
            Me.OPT_LOCATION_1.AutoSize = True
            Me.OPT_LOCATION_1.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_LOCATION_1.Location = New System.Drawing.Point(3, 3)
            Me.OPT_LOCATION_1.Name = "OPT_LOCATION_1"
            Me.OPT_LOCATION_1.Size = New System.Drawing.Size(566, 50)
            Me.OPT_LOCATION_1.TabIndex = 0
            Me.OPT_LOCATION_1.TabStop = True
            Me.OPT_LOCATION_1.Text = "Location user 1" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Location collection 1"
            Me.OPT_LOCATION_1.UseVisualStyleBackColor = True
            '
            'OPT_LOCATION_2
            '
            Me.OPT_LOCATION_2.AutoSize = True
            Me.OPT_LOCATION_2.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_LOCATION_2.Location = New System.Drawing.Point(3, 59)
            Me.OPT_LOCATION_2.Name = "OPT_LOCATION_2"
            Me.OPT_LOCATION_2.Size = New System.Drawing.Size(566, 50)
            Me.OPT_LOCATION_2.TabIndex = 1
            Me.OPT_LOCATION_2.TabStop = True
            Me.OPT_LOCATION_2.Text = "Location user 2" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Location collection 2"
            Me.OPT_LOCATION_2.UseVisualStyleBackColor = True
            '
            'OPT_LOCATION_3
            '
            Me.OPT_LOCATION_3.AutoSize = True
            Me.OPT_LOCATION_3.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_LOCATION_3.Location = New System.Drawing.Point(3, 115)
            Me.OPT_LOCATION_3.Name = "OPT_LOCATION_3"
            Me.OPT_LOCATION_3.Size = New System.Drawing.Size(566, 52)
            Me.OPT_LOCATION_3.TabIndex = 2
            Me.OPT_LOCATION_3.TabStop = True
            Me.OPT_LOCATION_3.Text = "Location user 3" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "Location collection 3"
            Me.OPT_LOCATION_3.UseVisualStyleBackColor = True
            '
            'TXT_COL_NAME
            '
            ActionButton4.BackgroundImage = CType(resources.GetObject("ActionButton4.BackgroundImage"), System.Drawing.Image)
            ActionButton4.Name = "Clear"
            ActionButton4.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_COL_NAME.Buttons.Add(ActionButton4)
            Me.TXT_COL_NAME.CaptionText = "Collection name"
            Me.TXT_COL_NAME.CaptionToolTipEnabled = True
            Me.TXT_COL_NAME.CaptionToolTipText = "Collection folder to be created in the destination"
            Me.TXT_COL_NAME.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_COL_NAME.Lines = New String(-1) {}
            Me.TXT_COL_NAME.Location = New System.Drawing.Point(3, 31)
            Me.TXT_COL_NAME.Name = "TXT_COL_NAME"
            Me.TXT_COL_NAME.PlaceholderEnabled = True
            Me.TXT_COL_NAME.PlaceholderText = "Enter collection name here..."
            Me.TXT_COL_NAME.Size = New System.Drawing.Size(578, 22)
            Me.TXT_COL_NAME.TabIndex = 1
            '
            'GlobalLocationsChooserForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(584, 251)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.KeyPreview = True
            Me.MinimumSize = New System.Drawing.Size(600, 290)
            Me.Name = "GlobalLocationsChooserForm"
            Me.ShowInTaskbar = False
            Me.Text = "Choose a new location"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            Me.TP_MAIN.ResumeLayout(False)
            CType(Me.CMB_LOCATIONS, System.ComponentModel.ISupportInitialize).EndInit()
            Me.FRM_LOCATIONS.ResumeLayout(False)
            TP_LOCATIONS_USER.ResumeLayout(False)
            TP_LOCATIONS_USER.PerformLayout()
            CType(Me.TXT_COL_NAME, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents CMB_LOCATIONS As PersonalUtilities.Forms.Controls.ComboBoxExtended
        Private WithEvents OPT_LOCATION_1 As RadioButton
        Private WithEvents OPT_LOCATION_2 As RadioButton
        Private WithEvents OPT_LOCATION_3 As RadioButton
        Private WithEvents FRM_LOCATIONS As GroupBox
        Private WithEvents TP_MAIN As TableLayoutPanel
        Private WithEvents TXT_COL_NAME As PersonalUtilities.Forms.Controls.TextBoxExtended
    End Class
End Namespace