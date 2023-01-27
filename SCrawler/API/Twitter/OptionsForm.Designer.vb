' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.Twitter
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class OptionsForm : Inherits System.Windows.Forms.Form
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
            Dim TP_MAIN As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OptionsForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Me.CH_DOWN_GIFS = New System.Windows.Forms.CheckBox()
            Me.TXT_GIF_FOLDER = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_GIF_PREFIX = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            TP_MAIN.SuspendLayout()
            CType(Me.TXT_GIF_FOLDER, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_GIF_PREFIX, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(304, 84)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(304, 109)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            TP_MAIN.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_MAIN.Controls.Add(Me.CH_DOWN_GIFS, 0, 0)
            TP_MAIN.Controls.Add(Me.TXT_GIF_FOLDER, 0, 1)
            TP_MAIN.Controls.Add(Me.TXT_GIF_PREFIX, 0, 2)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 4
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Size = New System.Drawing.Size(304, 84)
            TP_MAIN.TabIndex = 0
            '
            'CH_DOWN_GIFS
            '
            Me.CH_DOWN_GIFS.AutoSize = True
            Me.CH_DOWN_GIFS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_DOWN_GIFS.Location = New System.Drawing.Point(4, 4)
            Me.CH_DOWN_GIFS.Name = "CH_DOWN_GIFS"
            Me.CH_DOWN_GIFS.Padding = New System.Windows.Forms.Padding(100, 0, 0, 0)
            Me.CH_DOWN_GIFS.Size = New System.Drawing.Size(296, 19)
            Me.CH_DOWN_GIFS.TabIndex = 0
            Me.CH_DOWN_GIFS.Text = "Download GIFs"
            Me.CH_DOWN_GIFS.UseVisualStyleBackColor = True
            '
            'TXT_GIF_FOLDER
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Name = "Clear"
            ActionButton1.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_GIF_FOLDER.Buttons.Add(ActionButton1)
            Me.TXT_GIF_FOLDER.CaptionText = "GIFs special folder"
            Me.TXT_GIF_FOLDER.CaptionToolTipText = "Put the GIFs in a special folder"
            Me.TXT_GIF_FOLDER.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_GIF_FOLDER.Location = New System.Drawing.Point(4, 30)
            Me.TXT_GIF_FOLDER.Name = "TXT_GIF_FOLDER"
            Me.TXT_GIF_FOLDER.Size = New System.Drawing.Size(296, 22)
            Me.TXT_GIF_FOLDER.TabIndex = 1
            '
            'TXT_GIF_PREFIX
            '
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Name = "Clear"
            ActionButton2.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_GIF_PREFIX.Buttons.Add(ActionButton2)
            Me.TXT_GIF_PREFIX.CaptionText = "GIF prefix"
            Me.TXT_GIF_PREFIX.CaptionToolTipText = "This prefix will be added to the beginning of the filename"
            Me.TXT_GIF_PREFIX.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_GIF_PREFIX.Location = New System.Drawing.Point(4, 59)
            Me.TXT_GIF_PREFIX.Name = "TXT_GIF_PREFIX"
            Me.TXT_GIF_PREFIX.Size = New System.Drawing.Size(296, 22)
            Me.TXT_GIF_PREFIX.TabIndex = 2
            '
            'OptionsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(304, 109)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Icon = Global.SCrawler.My.Resources.SiteResources.TwitterIcon_32
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(320, 148)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(320, 148)
            Me.Name = "OptionsForm"
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Options"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            TP_MAIN.PerformLayout()
            CType(Me.TXT_GIF_FOLDER, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_GIF_PREFIX, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents CH_DOWN_GIFS As CheckBox
        Private WithEvents TXT_GIF_FOLDER As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_GIF_PREFIX As PersonalUtilities.Forms.Controls.TextBoxExtended
    End Class
End Namespace