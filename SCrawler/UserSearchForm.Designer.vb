' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Friend Class UserSearchForm : Inherits System.Windows.Forms.Form
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
        Dim TP_OPTIONS As System.Windows.Forms.TableLayoutPanel
        Dim TT_MAIN As System.Windows.Forms.ToolTip
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UserSearchForm))
        Me.TXT_SEARCH = New System.Windows.Forms.RichTextBox()
        Me.LIST_SEARCH = New System.Windows.Forms.ListBox()
        Me.CH_SEARCH_IN_DESCR = New System.Windows.Forms.CheckBox()
        Me.CH_SEARCH_IN_NAME = New System.Windows.Forms.CheckBox()
        Me.CH_SEARCH_IN_LABEL = New System.Windows.Forms.CheckBox()
        Me.CH_LEAVE_OPEN = New System.Windows.Forms.CheckBox()
        TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
        TP_OPTIONS = New System.Windows.Forms.TableLayoutPanel()
        TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
        TP_MAIN.SuspendLayout()
        TP_OPTIONS.SuspendLayout()
        Me.SuspendLayout()
        '
        'TP_MAIN
        '
        TP_MAIN.ColumnCount = 1
        TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        TP_MAIN.Controls.Add(Me.TXT_SEARCH, 0, 0)
        TP_MAIN.Controls.Add(Me.LIST_SEARCH, 0, 1)
        TP_MAIN.Controls.Add(TP_OPTIONS, 0, 2)
        TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
        TP_MAIN.Location = New System.Drawing.Point(0, 0)
        TP_MAIN.Name = "TP_MAIN"
        TP_MAIN.RowCount = 3
        TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56.0!))
        TP_MAIN.Size = New System.Drawing.Size(334, 311)
        TP_MAIN.TabIndex = 0
        '
        'TXT_SEARCH
        '
        Me.TXT_SEARCH.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TXT_SEARCH.Location = New System.Drawing.Point(1, 1)
        Me.TXT_SEARCH.Margin = New System.Windows.Forms.Padding(1)
        Me.TXT_SEARCH.Name = "TXT_SEARCH"
        Me.TXT_SEARCH.Size = New System.Drawing.Size(332, 48)
        Me.TXT_SEARCH.TabIndex = 0
        Me.TXT_SEARCH.Text = ""
        '
        'LIST_SEARCH
        '
        Me.LIST_SEARCH.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LIST_SEARCH.FormattingEnabled = True
        Me.LIST_SEARCH.Location = New System.Drawing.Point(1, 51)
        Me.LIST_SEARCH.Margin = New System.Windows.Forms.Padding(1)
        Me.LIST_SEARCH.Name = "LIST_SEARCH"
        Me.LIST_SEARCH.Size = New System.Drawing.Size(332, 203)
        Me.LIST_SEARCH.TabIndex = 1
        '
        'TP_OPTIONS
        '
        TP_OPTIONS.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
        TP_OPTIONS.ColumnCount = 2
        TP_OPTIONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        TP_OPTIONS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        TP_OPTIONS.Controls.Add(Me.CH_SEARCH_IN_DESCR, 1, 0)
        TP_OPTIONS.Controls.Add(Me.CH_SEARCH_IN_NAME, 0, 0)
        TP_OPTIONS.Controls.Add(Me.CH_SEARCH_IN_LABEL, 0, 1)
        TP_OPTIONS.Controls.Add(Me.CH_LEAVE_OPEN, 1, 1)
        TP_OPTIONS.Dock = System.Windows.Forms.DockStyle.Fill
        TP_OPTIONS.Location = New System.Drawing.Point(3, 258)
        TP_OPTIONS.Name = "TP_OPTIONS"
        TP_OPTIONS.RowCount = 2
        TP_OPTIONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        TP_OPTIONS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        TP_OPTIONS.Size = New System.Drawing.Size(328, 50)
        TP_OPTIONS.TabIndex = 2
        '
        'CH_SEARCH_IN_DESCR
        '
        Me.CH_SEARCH_IN_DESCR.AutoSize = True
        Me.CH_SEARCH_IN_DESCR.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CH_SEARCH_IN_DESCR.Location = New System.Drawing.Point(167, 4)
        Me.CH_SEARCH_IN_DESCR.Name = "CH_SEARCH_IN_DESCR"
        Me.CH_SEARCH_IN_DESCR.Size = New System.Drawing.Size(157, 17)
        Me.CH_SEARCH_IN_DESCR.TabIndex = 2
        Me.CH_SEARCH_IN_DESCR.Text = "Search in description"
        TT_MAIN.SetToolTip(Me.CH_SEARCH_IN_DESCR, "Search also in the description")
        Me.CH_SEARCH_IN_DESCR.UseVisualStyleBackColor = True
        '
        'CH_SEARCH_IN_NAME
        '
        Me.CH_SEARCH_IN_NAME.AutoSize = True
        Me.CH_SEARCH_IN_NAME.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CH_SEARCH_IN_NAME.Location = New System.Drawing.Point(4, 4)
        Me.CH_SEARCH_IN_NAME.Name = "CH_SEARCH_IN_NAME"
        Me.CH_SEARCH_IN_NAME.Size = New System.Drawing.Size(156, 17)
        Me.CH_SEARCH_IN_NAME.TabIndex = 3
        Me.CH_SEARCH_IN_NAME.Text = "Search in name"
        Me.CH_SEARCH_IN_NAME.UseVisualStyleBackColor = True
        '
        'CH_SEARCH_IN_LABEL
        '
        Me.CH_SEARCH_IN_LABEL.AutoSize = True
        Me.CH_SEARCH_IN_LABEL.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CH_SEARCH_IN_LABEL.Location = New System.Drawing.Point(4, 28)
        Me.CH_SEARCH_IN_LABEL.Name = "CH_SEARCH_IN_LABEL"
        Me.CH_SEARCH_IN_LABEL.Size = New System.Drawing.Size(156, 18)
        Me.CH_SEARCH_IN_LABEL.TabIndex = 4
        Me.CH_SEARCH_IN_LABEL.Text = "Search in labels"
        Me.CH_SEARCH_IN_LABEL.UseVisualStyleBackColor = True
        '
        'CH_LEAVE_OPEN
        '
        Me.CH_LEAVE_OPEN.AutoSize = True
        Me.CH_LEAVE_OPEN.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CH_LEAVE_OPEN.Location = New System.Drawing.Point(167, 28)
        Me.CH_LEAVE_OPEN.Name = "CH_LEAVE_OPEN"
        Me.CH_LEAVE_OPEN.Size = New System.Drawing.Size(157, 18)
        Me.CH_LEAVE_OPEN.TabIndex = 5
        Me.CH_LEAVE_OPEN.Text = "Leave open"
        TT_MAIN.SetToolTip(Me.CH_LEAVE_OPEN, "Leave the form open after double clicking on the found user")
        Me.CH_LEAVE_OPEN.UseVisualStyleBackColor = True
        '
        'UserSearchForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(334, 311)
        Me.Controls.Add(TP_MAIN)
        Me.Icon = Global.SCrawler.My.Resources.UsersIcon_32
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(250, 250)
        Me.Name = "UserSearchForm"
        Me.Text = "Search users"
        TP_MAIN.ResumeLayout(False)
        TP_OPTIONS.ResumeLayout(False)
        TP_OPTIONS.PerformLayout()
        Me.ResumeLayout(False)
    End Sub
    Private WithEvents TXT_SEARCH As RichTextBox
    Private WithEvents LIST_SEARCH As ListBox
    Private WithEvents CH_SEARCH_IN_DESCR As CheckBox
    Private WithEvents CH_SEARCH_IN_NAME As CheckBox
    Private WithEvents CH_SEARCH_IN_LABEL As CheckBox
    Private WithEvents CH_LEAVE_OPEN As CheckBox
End Class