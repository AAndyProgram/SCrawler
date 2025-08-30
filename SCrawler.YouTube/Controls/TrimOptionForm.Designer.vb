' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.YouTube.Controls
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class TrimOptionForm : Inherits System.Windows.Forms.Form
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(TrimOptionForm))
            Dim TP_OPTIONS As System.Windows.Forms.TableLayoutPanel
            Me.TXT_NAME = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TP_TIME_TIME = New System.Windows.Forms.TableLayoutPanel()
            Me.TXT_FROM_DATE = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_TO_DATE = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.OPT_INT = New System.Windows.Forms.RadioButton()
            Me.OPT_TIME = New System.Windows.Forms.RadioButton()
            Me.TP_TIME_INT = New System.Windows.Forms.TableLayoutPanel()
            Me.TXT_FROM_INT = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_TO_INT = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            TP_OPTIONS = New System.Windows.Forms.TableLayoutPanel()
            CONTAINER_MAIN.ContentPanel.SuspendLayout()
            CONTAINER_MAIN.SuspendLayout()
            TP_MAIN.SuspendLayout()
            CType(Me.TXT_NAME, System.ComponentModel.ISupportInitialize).BeginInit()
            TP_OPTIONS.SuspendLayout()
            Me.TP_TIME_TIME.SuspendLayout()
            CType(Me.TXT_FROM_DATE, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_TO_DATE, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.TP_TIME_INT.SuspendLayout()
            CType(Me.TXT_FROM_INT, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_TO_INT, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(334, 112)
            CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            CONTAINER_MAIN.LeftToolStripPanelVisible = False
            CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            CONTAINER_MAIN.RightToolStripPanelVisible = False
            CONTAINER_MAIN.Size = New System.Drawing.Size(334, 112)
            CONTAINER_MAIN.TabIndex = 0
            CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'TP_MAIN
            '
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_MAIN.Controls.Add(Me.TXT_NAME, 0, 0)
            TP_MAIN.Controls.Add(TP_OPTIONS, 0, 1)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 2
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.Size = New System.Drawing.Size(334, 112)
            TP_MAIN.TabIndex = 0
            '
            'TXT_NAME
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Name = "Clear"
            ActionButton1.Tag = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons.Clear
            Me.TXT_NAME.Buttons.Add(ActionButton1)
            Me.TXT_NAME.CaptionText = "Name"
            Me.TXT_NAME.CaptionToolTipText = "Section name"
            Me.TXT_NAME.CaptionWidth = 50.0R
            Me.TXT_NAME.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_NAME.Location = New System.Drawing.Point(3, 3)
            Me.TXT_NAME.Name = "TXT_NAME"
            Me.TXT_NAME.Size = New System.Drawing.Size(328, 22)
            Me.TXT_NAME.TabIndex = 0
            '
            'TP_OPTIONS
            '
            TP_OPTIONS.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            TP_OPTIONS.ColumnCount = 2
            TP_OPTIONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30.0!))
            TP_OPTIONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_OPTIONS.Controls.Add(Me.TP_TIME_TIME, 1, 1)
            TP_OPTIONS.Controls.Add(Me.OPT_INT, 0, 0)
            TP_OPTIONS.Controls.Add(Me.OPT_TIME, 0, 1)
            TP_OPTIONS.Controls.Add(Me.TP_TIME_INT, 1, 0)
            TP_OPTIONS.Dock = System.Windows.Forms.DockStyle.Fill
            TP_OPTIONS.Location = New System.Drawing.Point(3, 31)
            TP_OPTIONS.Name = "TP_OPTIONS"
            TP_OPTIONS.RowCount = 3
            TP_OPTIONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_OPTIONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            TP_OPTIONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_OPTIONS.Size = New System.Drawing.Size(328, 78)
            TP_OPTIONS.TabIndex = 1
            '
            'TP_TIME_TIME
            '
            Me.TP_TIME_TIME.ColumnCount = 2
            Me.TP_TIME_TIME.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TP_TIME_TIME.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TP_TIME_TIME.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TP_TIME_TIME.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TP_TIME_TIME.Controls.Add(Me.TXT_FROM_DATE, 0, 0)
            Me.TP_TIME_TIME.Controls.Add(Me.TXT_TO_DATE, 1, 0)
            Me.TP_TIME_TIME.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_TIME_TIME.Location = New System.Drawing.Point(32, 27)
            Me.TP_TIME_TIME.Margin = New System.Windows.Forms.Padding(0)
            Me.TP_TIME_TIME.Name = "TP_TIME_TIME"
            Me.TP_TIME_TIME.RowCount = 1
            Me.TP_TIME_TIME.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_TIME_TIME.Size = New System.Drawing.Size(295, 25)
            Me.TP_TIME_TIME.TabIndex = 3
            '
            'TXT_FROM_DATE
            '
            Me.TXT_FROM_DATE.CaptionPadding = New System.Windows.Forms.Padding(0, 0, 6, 0)
            Me.TXT_FROM_DATE.CaptionText = "From"
            Me.TXT_FROM_DATE.CaptionWidth = 40.0R
            Me.TXT_FROM_DATE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_FROM_DATE.Location = New System.Drawing.Point(3, 3)
            Me.TXT_FROM_DATE.Name = "TXT_FROM_DATE"
            Me.TXT_FROM_DATE.PlaceholderEnabled = True
            Me.TXT_FROM_DATE.PlaceholderText = "h:mm:ss"
            Me.TXT_FROM_DATE.Size = New System.Drawing.Size(141, 22)
            Me.TXT_FROM_DATE.TabIndex = 0
            '
            'TXT_TO_DATE
            '
            Me.TXT_TO_DATE.CaptionPadding = New System.Windows.Forms.Padding(0, 0, 6, 0)
            Me.TXT_TO_DATE.CaptionText = "To"
            Me.TXT_TO_DATE.CaptionWidth = 40.0R
            Me.TXT_TO_DATE.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_TO_DATE.Location = New System.Drawing.Point(150, 3)
            Me.TXT_TO_DATE.Name = "TXT_TO_DATE"
            Me.TXT_TO_DATE.PlaceholderEnabled = True
            Me.TXT_TO_DATE.PlaceholderText = "h:mm:ss"
            Me.TXT_TO_DATE.Size = New System.Drawing.Size(142, 22)
            Me.TXT_TO_DATE.TabIndex = 1
            '
            'OPT_INT
            '
            Me.OPT_INT.AutoSize = True
            Me.OPT_INT.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter
            Me.OPT_INT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_INT.Location = New System.Drawing.Point(4, 4)
            Me.OPT_INT.Name = "OPT_INT"
            Me.OPT_INT.Size = New System.Drawing.Size(24, 19)
            Me.OPT_INT.TabIndex = 0
            Me.OPT_INT.TabStop = True
            Me.OPT_INT.UseVisualStyleBackColor = True
            '
            'OPT_TIME
            '
            Me.OPT_TIME.AutoSize = True
            Me.OPT_TIME.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter
            Me.OPT_TIME.Dock = System.Windows.Forms.DockStyle.Fill
            Me.OPT_TIME.Location = New System.Drawing.Point(4, 30)
            Me.OPT_TIME.Name = "OPT_TIME"
            Me.OPT_TIME.Size = New System.Drawing.Size(24, 19)
            Me.OPT_TIME.TabIndex = 1
            Me.OPT_TIME.TabStop = True
            Me.OPT_TIME.UseVisualStyleBackColor = True
            '
            'TP_TIME_INT
            '
            Me.TP_TIME_INT.ColumnCount = 2
            Me.TP_TIME_INT.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TP_TIME_INT.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            Me.TP_TIME_INT.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TP_TIME_INT.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TP_TIME_INT.Controls.Add(Me.TXT_FROM_INT, 0, 0)
            Me.TP_TIME_INT.Controls.Add(Me.TXT_TO_INT, 1, 0)
            Me.TP_TIME_INT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_TIME_INT.Location = New System.Drawing.Point(32, 1)
            Me.TP_TIME_INT.Margin = New System.Windows.Forms.Padding(0)
            Me.TP_TIME_INT.Name = "TP_TIME_INT"
            Me.TP_TIME_INT.RowCount = 1
            Me.TP_TIME_INT.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_TIME_INT.Size = New System.Drawing.Size(295, 25)
            Me.TP_TIME_INT.TabIndex = 2
            '
            'TXT_FROM_INT
            '
            Me.TXT_FROM_INT.CaptionPadding = New System.Windows.Forms.Padding(0, 0, 6, 0)
            Me.TXT_FROM_INT.CaptionText = "From"
            Me.TXT_FROM_INT.CaptionWidth = 40.0R
            Me.TXT_FROM_INT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_FROM_INT.Location = New System.Drawing.Point(3, 3)
            Me.TXT_FROM_INT.Name = "TXT_FROM_INT"
            Me.TXT_FROM_INT.Size = New System.Drawing.Size(141, 22)
            Me.TXT_FROM_INT.TabIndex = 0
            '
            'TXT_TO_INT
            '
            Me.TXT_TO_INT.CaptionPadding = New System.Windows.Forms.Padding(0, 0, 6, 0)
            Me.TXT_TO_INT.CaptionText = "To"
            Me.TXT_TO_INT.CaptionWidth = 40.0R
            Me.TXT_TO_INT.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_TO_INT.Location = New System.Drawing.Point(150, 3)
            Me.TXT_TO_INT.Name = "TXT_TO_INT"
            Me.TXT_TO_INT.Size = New System.Drawing.Size(142, 22)
            Me.TXT_TO_INT.TabIndex = 1
            '
            'TrimOptionForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(334, 112)
            Me.Controls.Add(CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(350, 151)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(350, 151)
            Me.Name = "TrimOptionForm"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Trim option"
            CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            CONTAINER_MAIN.ResumeLayout(False)
            CONTAINER_MAIN.PerformLayout()
            TP_MAIN.ResumeLayout(False)
            CType(Me.TXT_NAME, System.ComponentModel.ISupportInitialize).EndInit()
            TP_OPTIONS.ResumeLayout(False)
            TP_OPTIONS.PerformLayout()
            Me.TP_TIME_TIME.ResumeLayout(False)
            CType(Me.TXT_FROM_DATE, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_TO_DATE, System.ComponentModel.ISupportInitialize).EndInit()
            Me.TP_TIME_INT.ResumeLayout(False)
            CType(Me.TXT_FROM_INT, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_TO_INT, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents TXT_NAME As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents OPT_INT As RadioButton
        Private WithEvents OPT_TIME As RadioButton
        Private WithEvents TP_TIME_INT As TableLayoutPanel
        Private WithEvents TP_TIME_TIME As TableLayoutPanel
        Private WithEvents TXT_FROM_DATE As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_TO_DATE As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_FROM_INT As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_TO_INT As PersonalUtilities.Forms.Controls.TextBoxExtended
    End Class
End Namespace