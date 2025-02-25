' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace DownloadObjects
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class DownloadFeedForm : Inherits System.Windows.Forms.Form
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
            Dim SEP_1 As System.Windows.Forms.ToolStripSeparator
            Dim SEP_2 As System.Windows.Forms.ToolStripSeparator
            Dim MENU_VIEW As System.Windows.Forms.ToolStripDropDownButton
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DownloadFeedForm))
            Dim MENU_LOAD_SEP_1 As System.Windows.Forms.ToolStripSeparator
            Dim MENU_LOAD_SEP_2 As System.Windows.Forms.ToolStripSeparator
            Dim MENU_LOAD_SEP_3 As System.Windows.Forms.ToolStripSeparator
            Dim MENU_LOAD_SEP_4 As System.Windows.Forms.ToolStripSeparator
            Dim MENU_LOAD_SEP_5 As System.Windows.Forms.ToolStripSeparator
            Dim MENU_LOAD_SEP_6 As System.Windows.Forms.ToolStripSeparator
            Dim MENU_LOAD_SEP_7 As System.Windows.Forms.ToolStripSeparator
            Dim MENU_LOAD_SEP_0 As System.Windows.Forms.ToolStripSeparator
            Dim MENU_LOAD_SEP_8 As System.Windows.Forms.ToolStripSeparator
            Dim MENU_LOAD_SEP_9 As System.Windows.Forms.ToolStripSeparator
            Me.OPT_DEFAULT = New System.Windows.Forms.ToolStripMenuItem()
            Me.OPT_SUBSCRIPTIONS = New System.Windows.Forms.ToolStripMenuItem()
            Me.ToolbarTOP = New System.Windows.Forms.ToolStrip()
            Me.MENU_LOAD_SESSION = New System.Windows.Forms.ToolStripDropDownButton()
            Me.BTT_LOAD_SESSION_CURRENT = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_LOAD_SESSION_LAST = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_LOAD_SESSION_CHOOSE = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_COPY_TO = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_MOVE_TO = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_COPY_SPEC_TO = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_MOVE_SPEC_TO = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_LOAD_FAV = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_LOAD_SPEC = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FEED_ADD_FAV = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FEED_ADD_FAV_REMOVE = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FEED_REMOVE_FAV = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FEED_ADD_SPEC = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FEED_ADD_SPEC_REMOVE = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FEED_REMOVE_SPEC = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FEED_CLEAR_FAV = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FEED_CLEAR_SPEC = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FEED_DELETE_SPEC = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FEED_DELETE_DAILY_LIST = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_FEED_DELETE_DAILY_DATE = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CURR_SESSION_SET = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CURR_SESSION_SET_LAST = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_MERGE_SESSIONS = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CLEAR_DAILY = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_RESET_DAILY = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_MERGE_FEEDS = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CHECK_ALL = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_CHECK_NONE = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_VIEW_SAVE = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_VIEW_LOAD = New System.Windows.Forms.ToolStripMenuItem()
            Me.SEP_0 = New System.Windows.Forms.ToolStripSeparator()
            Me.MENU_DOWN = New System.Windows.Forms.ToolStripDropDownButton()
            Me.BTT_DOWN_ALL = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_DOWN_SELECTED = New System.Windows.Forms.ToolStripMenuItem()
            Me.BTT_REFRESH = New System.Windows.Forms.ToolStripButton()
            Me.TP_DATA = New System.Windows.Forms.TableLayoutPanel()
            Me.BTT_CHECK_INVERT = New System.Windows.Forms.ToolStripMenuItem()
            SEP_1 = New System.Windows.Forms.ToolStripSeparator()
            SEP_2 = New System.Windows.Forms.ToolStripSeparator()
            MENU_VIEW = New System.Windows.Forms.ToolStripDropDownButton()
            MENU_LOAD_SEP_1 = New System.Windows.Forms.ToolStripSeparator()
            MENU_LOAD_SEP_2 = New System.Windows.Forms.ToolStripSeparator()
            MENU_LOAD_SEP_3 = New System.Windows.Forms.ToolStripSeparator()
            MENU_LOAD_SEP_4 = New System.Windows.Forms.ToolStripSeparator()
            MENU_LOAD_SEP_5 = New System.Windows.Forms.ToolStripSeparator()
            MENU_LOAD_SEP_6 = New System.Windows.Forms.ToolStripSeparator()
            MENU_LOAD_SEP_7 = New System.Windows.Forms.ToolStripSeparator()
            MENU_LOAD_SEP_0 = New System.Windows.Forms.ToolStripSeparator()
            MENU_LOAD_SEP_8 = New System.Windows.Forms.ToolStripSeparator()
            MENU_LOAD_SEP_9 = New System.Windows.Forms.ToolStripSeparator()
            Me.ToolbarTOP.SuspendLayout()
            Me.SuspendLayout()
            '
            'SEP_1
            '
            SEP_1.Name = "SEP_1"
            SEP_1.Size = New System.Drawing.Size(6, 25)
            '
            'SEP_2
            '
            SEP_2.Name = "SEP_2"
            SEP_2.Size = New System.Drawing.Size(6, 25)
            '
            'MENU_VIEW
            '
            MENU_VIEW.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            MENU_VIEW.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OPT_DEFAULT, Me.OPT_SUBSCRIPTIONS})
            MENU_VIEW.Image = CType(resources.GetObject("MENU_VIEW.Image"), System.Drawing.Image)
            MENU_VIEW.ImageTransparentColor = System.Drawing.Color.Magenta
            MENU_VIEW.Name = "MENU_VIEW"
            MENU_VIEW.Size = New System.Drawing.Size(29, 22)
            MENU_VIEW.Text = "View"
            '
            'OPT_DEFAULT
            '
            Me.OPT_DEFAULT.Name = "OPT_DEFAULT"
            Me.OPT_DEFAULT.Size = New System.Drawing.Size(145, 22)
            Me.OPT_DEFAULT.Text = "Downloads"
            '
            'OPT_SUBSCRIPTIONS
            '
            Me.OPT_SUBSCRIPTIONS.Name = "OPT_SUBSCRIPTIONS"
            Me.OPT_SUBSCRIPTIONS.Size = New System.Drawing.Size(145, 22)
            Me.OPT_SUBSCRIPTIONS.Text = "Subscriptions"
            '
            'MENU_LOAD_SEP_1
            '
            MENU_LOAD_SEP_1.Name = "MENU_LOAD_SEP_1"
            MENU_LOAD_SEP_1.Size = New System.Drawing.Size(349, 6)
            '
            'MENU_LOAD_SEP_2
            '
            MENU_LOAD_SEP_2.Name = "MENU_LOAD_SEP_2"
            MENU_LOAD_SEP_2.Size = New System.Drawing.Size(349, 6)
            '
            'MENU_LOAD_SEP_3
            '
            MENU_LOAD_SEP_3.Name = "MENU_LOAD_SEP_3"
            MENU_LOAD_SEP_3.Size = New System.Drawing.Size(349, 6)
            '
            'MENU_LOAD_SEP_4
            '
            MENU_LOAD_SEP_4.Name = "MENU_LOAD_SEP_4"
            MENU_LOAD_SEP_4.Size = New System.Drawing.Size(349, 6)
            '
            'MENU_LOAD_SEP_5
            '
            MENU_LOAD_SEP_5.Name = "MENU_LOAD_SEP_5"
            MENU_LOAD_SEP_5.Size = New System.Drawing.Size(349, 6)
            '
            'MENU_LOAD_SEP_6
            '
            MENU_LOAD_SEP_6.Name = "MENU_LOAD_SEP_6"
            MENU_LOAD_SEP_6.Size = New System.Drawing.Size(349, 6)
            '
            'MENU_LOAD_SEP_7
            '
            MENU_LOAD_SEP_7.Name = "MENU_LOAD_SEP_7"
            MENU_LOAD_SEP_7.Size = New System.Drawing.Size(349, 6)
            '
            'MENU_LOAD_SEP_0
            '
            MENU_LOAD_SEP_0.Name = "MENU_LOAD_SEP_0"
            MENU_LOAD_SEP_0.Size = New System.Drawing.Size(349, 6)
            '
            'MENU_LOAD_SEP_8
            '
            MENU_LOAD_SEP_8.Name = "MENU_LOAD_SEP_8"
            MENU_LOAD_SEP_8.Size = New System.Drawing.Size(349, 6)
            '
            'MENU_LOAD_SEP_9
            '
            MENU_LOAD_SEP_9.Name = "MENU_LOAD_SEP_9"
            MENU_LOAD_SEP_9.Size = New System.Drawing.Size(349, 6)
            '
            'ToolbarTOP
            '
            Me.ToolbarTOP.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
            Me.ToolbarTOP.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MENU_LOAD_SESSION, Me.SEP_0, MENU_VIEW, SEP_1, Me.MENU_DOWN, Me.BTT_REFRESH, SEP_2})
            Me.ToolbarTOP.Location = New System.Drawing.Point(0, 0)
            Me.ToolbarTOP.Name = "ToolbarTOP"
            Me.ToolbarTOP.Size = New System.Drawing.Size(484, 25)
            Me.ToolbarTOP.TabIndex = 0
            '
            'MENU_LOAD_SESSION
            '
            Me.MENU_LOAD_SESSION.AutoToolTip = False
            Me.MENU_LOAD_SESSION.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.MENU_LOAD_SESSION.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_LOAD_SESSION_CURRENT, Me.BTT_LOAD_SESSION_LAST, Me.BTT_LOAD_SESSION_CHOOSE, MENU_LOAD_SEP_0, Me.BTT_COPY_TO, Me.BTT_MOVE_TO, MENU_LOAD_SEP_1, Me.BTT_COPY_SPEC_TO, Me.BTT_MOVE_SPEC_TO, MENU_LOAD_SEP_2, Me.BTT_LOAD_FAV, Me.BTT_LOAD_SPEC, MENU_LOAD_SEP_3, Me.BTT_FEED_ADD_FAV, Me.BTT_FEED_ADD_FAV_REMOVE, Me.BTT_FEED_REMOVE_FAV, MENU_LOAD_SEP_4, Me.BTT_FEED_ADD_SPEC, Me.BTT_FEED_ADD_SPEC_REMOVE, Me.BTT_FEED_REMOVE_SPEC, MENU_LOAD_SEP_5, Me.BTT_FEED_CLEAR_FAV, Me.BTT_FEED_CLEAR_SPEC, Me.BTT_FEED_DELETE_SPEC, Me.BTT_FEED_DELETE_DAILY_LIST, Me.BTT_FEED_DELETE_DAILY_DATE, MENU_LOAD_SEP_6, Me.BTT_CURR_SESSION_SET, Me.BTT_CURR_SESSION_SET_LAST, Me.BTT_MERGE_SESSIONS, Me.BTT_CLEAR_DAILY, Me.BTT_RESET_DAILY, MENU_LOAD_SEP_7, Me.BTT_MERGE_FEEDS, MENU_LOAD_SEP_8, Me.BTT_CHECK_ALL, Me.BTT_CHECK_NONE, Me.BTT_CHECK_INVERT, MENU_LOAD_SEP_9, Me.BTT_VIEW_SAVE, Me.BTT_VIEW_LOAD})
            Me.MENU_LOAD_SESSION.Image = Global.SCrawler.My.Resources.Resources.ArrowDownPic_Blue_24
            Me.MENU_LOAD_SESSION.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.MENU_LOAD_SESSION.Name = "MENU_LOAD_SESSION"
            Me.MENU_LOAD_SESSION.Size = New System.Drawing.Size(29, 22)
            Me.MENU_LOAD_SESSION.Text = "Load session"
            '
            'BTT_LOAD_SESSION_CURRENT
            '
            Me.BTT_LOAD_SESSION_CURRENT.Image = Global.SCrawler.My.Resources.Resources.ArrowDownPic_Blue_24
            Me.BTT_LOAD_SESSION_CURRENT.Name = "BTT_LOAD_SESSION_CURRENT"
            Me.BTT_LOAD_SESSION_CURRENT.Size = New System.Drawing.Size(352, 22)
            Me.BTT_LOAD_SESSION_CURRENT.Text = "Load current session"
            '
            'BTT_LOAD_SESSION_LAST
            '
            Me.BTT_LOAD_SESSION_LAST.Image = Global.SCrawler.My.Resources.Resources.ArrowDownPic_Blue_24
            Me.BTT_LOAD_SESSION_LAST.Name = "BTT_LOAD_SESSION_LAST"
            Me.BTT_LOAD_SESSION_LAST.Size = New System.Drawing.Size(352, 22)
            Me.BTT_LOAD_SESSION_LAST.Text = "Load last session"
            '
            'BTT_LOAD_SESSION_CHOOSE
            '
            Me.BTT_LOAD_SESSION_CHOOSE.Image = Global.SCrawler.My.Resources.Resources.ArrowDownPic_Blue_24
            Me.BTT_LOAD_SESSION_CHOOSE.Name = "BTT_LOAD_SESSION_CHOOSE"
            Me.BTT_LOAD_SESSION_CHOOSE.Size = New System.Drawing.Size(352, 22)
            Me.BTT_LOAD_SESSION_CHOOSE.Text = "Select loading session"
            '
            'BTT_COPY_TO
            '
            Me.BTT_COPY_TO.Image = Global.SCrawler.My.Resources.Resources.PastePic_32
            Me.BTT_COPY_TO.Name = "BTT_COPY_TO"
            Me.BTT_COPY_TO.Size = New System.Drawing.Size(352, 22)
            Me.BTT_COPY_TO.Text = "Copy checked to..."
            '
            'BTT_MOVE_TO
            '
            Me.BTT_MOVE_TO.Image = Global.SCrawler.My.Resources.Resources.CutPic_48
            Me.BTT_MOVE_TO.Name = "BTT_MOVE_TO"
            Me.BTT_MOVE_TO.Size = New System.Drawing.Size(352, 22)
            Me.BTT_MOVE_TO.Text = "Move checked to..."
            '
            'BTT_COPY_SPEC_TO
            '
            Me.BTT_COPY_SPEC_TO.AutoToolTip = True
            Me.BTT_COPY_SPEC_TO.Image = Global.SCrawler.My.Resources.Resources.PastePic_32
            Me.BTT_COPY_SPEC_TO.Name = "BTT_COPY_SPEC_TO"
            Me.BTT_COPY_SPEC_TO.Size = New System.Drawing.Size(352, 22)
            Me.BTT_COPY_SPEC_TO.Text = "Copy feed/session files to..."
            Me.BTT_COPY_SPEC_TO.ToolTipText = "Copy all the files of the loaded feed/session to..."
            '
            'BTT_MOVE_SPEC_TO
            '
            Me.BTT_MOVE_SPEC_TO.AutoToolTip = True
            Me.BTT_MOVE_SPEC_TO.Image = Global.SCrawler.My.Resources.Resources.CutPic_48
            Me.BTT_MOVE_SPEC_TO.Name = "BTT_MOVE_SPEC_TO"
            Me.BTT_MOVE_SPEC_TO.Size = New System.Drawing.Size(352, 22)
            Me.BTT_MOVE_SPEC_TO.Text = "Move feed/session files to..."
            Me.BTT_MOVE_SPEC_TO.ToolTipText = "Move all the files of the loaded feed/session to..."
            '
            'BTT_LOAD_FAV
            '
            Me.BTT_LOAD_FAV.Image = Global.SCrawler.My.Resources.Resources.HeartPic_32
            Me.BTT_LOAD_FAV.Name = "BTT_LOAD_FAV"
            Me.BTT_LOAD_FAV.Size = New System.Drawing.Size(352, 22)
            Me.BTT_LOAD_FAV.Text = "Load Favorite"
            '
            'BTT_LOAD_SPEC
            '
            Me.BTT_LOAD_SPEC.Image = Global.SCrawler.My.Resources.Resources.RSSPic_512
            Me.BTT_LOAD_SPEC.Name = "BTT_LOAD_SPEC"
            Me.BTT_LOAD_SPEC.Size = New System.Drawing.Size(352, 22)
            Me.BTT_LOAD_SPEC.Text = "Load special feed"
            '
            'BTT_FEED_ADD_FAV
            '
            Me.BTT_FEED_ADD_FAV.Image = Global.SCrawler.My.Resources.Resources.HeartPic_32
            Me.BTT_FEED_ADD_FAV.Name = "BTT_FEED_ADD_FAV"
            Me.BTT_FEED_ADD_FAV.Size = New System.Drawing.Size(352, 22)
            Me.BTT_FEED_ADD_FAV.Text = "Add checked to Favorite"
            '
            'BTT_FEED_ADD_FAV_REMOVE
            '
            Me.BTT_FEED_ADD_FAV_REMOVE.Image = Global.SCrawler.My.Resources.Resources.HeartPic_32
            Me.BTT_FEED_ADD_FAV_REMOVE.Name = "BTT_FEED_ADD_FAV_REMOVE"
            Me.BTT_FEED_ADD_FAV_REMOVE.Size = New System.Drawing.Size(352, 22)
            Me.BTT_FEED_ADD_FAV_REMOVE.Text = "Add checked to Favorite (remove from current)"
            '
            'BTT_FEED_REMOVE_FAV
            '
            Me.BTT_FEED_REMOVE_FAV.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
            Me.BTT_FEED_REMOVE_FAV.Name = "BTT_FEED_REMOVE_FAV"
            Me.BTT_FEED_REMOVE_FAV.Size = New System.Drawing.Size(352, 22)
            Me.BTT_FEED_REMOVE_FAV.Text = "Remove checked from Favorite"
            '
            'BTT_FEED_ADD_SPEC
            '
            Me.BTT_FEED_ADD_SPEC.Image = Global.SCrawler.My.Resources.Resources.RSSPic_512
            Me.BTT_FEED_ADD_SPEC.Name = "BTT_FEED_ADD_SPEC"
            Me.BTT_FEED_ADD_SPEC.Size = New System.Drawing.Size(352, 22)
            Me.BTT_FEED_ADD_SPEC.Text = "Add checked to special feed..."
            '
            'BTT_FEED_ADD_SPEC_REMOVE
            '
            Me.BTT_FEED_ADD_SPEC_REMOVE.Image = Global.SCrawler.My.Resources.Resources.RSSPic_512
            Me.BTT_FEED_ADD_SPEC_REMOVE.Name = "BTT_FEED_ADD_SPEC_REMOVE"
            Me.BTT_FEED_ADD_SPEC_REMOVE.Size = New System.Drawing.Size(352, 22)
            Me.BTT_FEED_ADD_SPEC_REMOVE.Text = "Add checked to special feed (remove from current)..."
            '
            'BTT_FEED_REMOVE_SPEC
            '
            Me.BTT_FEED_REMOVE_SPEC.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
            Me.BTT_FEED_REMOVE_SPEC.Name = "BTT_FEED_REMOVE_SPEC"
            Me.BTT_FEED_REMOVE_SPEC.Size = New System.Drawing.Size(352, 22)
            Me.BTT_FEED_REMOVE_SPEC.Text = "Remove checked from special feed..."
            '
            'BTT_FEED_CLEAR_FAV
            '
            Me.BTT_FEED_CLEAR_FAV.Image = Global.SCrawler.My.Resources.Resources.BrushToolPic_16
            Me.BTT_FEED_CLEAR_FAV.Name = "BTT_FEED_CLEAR_FAV"
            Me.BTT_FEED_CLEAR_FAV.Size = New System.Drawing.Size(352, 22)
            Me.BTT_FEED_CLEAR_FAV.Text = "Clear Favorite"
            '
            'BTT_FEED_CLEAR_SPEC
            '
            Me.BTT_FEED_CLEAR_SPEC.Image = Global.SCrawler.My.Resources.Resources.BrushToolPic_16
            Me.BTT_FEED_CLEAR_SPEC.Name = "BTT_FEED_CLEAR_SPEC"
            Me.BTT_FEED_CLEAR_SPEC.Size = New System.Drawing.Size(352, 22)
            Me.BTT_FEED_CLEAR_SPEC.Text = "Clear special feed..."
            '
            'BTT_FEED_DELETE_SPEC
            '
            Me.BTT_FEED_DELETE_SPEC.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
            Me.BTT_FEED_DELETE_SPEC.Name = "BTT_FEED_DELETE_SPEC"
            Me.BTT_FEED_DELETE_SPEC.Size = New System.Drawing.Size(352, 22)
            Me.BTT_FEED_DELETE_SPEC.Text = "Delete special feed..."
            '
            'BTT_FEED_DELETE_DAILY_LIST
            '
            Me.BTT_FEED_DELETE_DAILY_LIST.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
            Me.BTT_FEED_DELETE_DAILY_LIST.Name = "BTT_FEED_DELETE_DAILY_LIST"
            Me.BTT_FEED_DELETE_DAILY_LIST.Size = New System.Drawing.Size(352, 22)
            Me.BTT_FEED_DELETE_DAILY_LIST.Text = "Delete daily feed (by list)"
            '
            'BTT_FEED_DELETE_DAILY_DATE
            '
            Me.BTT_FEED_DELETE_DAILY_DATE.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
            Me.BTT_FEED_DELETE_DAILY_DATE.Name = "BTT_FEED_DELETE_DAILY_DATE"
            Me.BTT_FEED_DELETE_DAILY_DATE.Size = New System.Drawing.Size(352, 22)
            Me.BTT_FEED_DELETE_DAILY_DATE.Text = "Delete daily feed (by date)"
            '
            'BTT_CURR_SESSION_SET
            '
            Me.BTT_CURR_SESSION_SET.AutoToolTip = True
            Me.BTT_CURR_SESSION_SET.Image = Global.SCrawler.My.Resources.Resources.ArrowDownPic_Blue_24
            Me.BTT_CURR_SESSION_SET.Name = "BTT_CURR_SESSION_SET"
            Me.BTT_CURR_SESSION_SET.Size = New System.Drawing.Size(352, 22)
            Me.BTT_CURR_SESSION_SET.Text = "Set current session..."
            Me.BTT_CURR_SESSION_SET.ToolTipText = "Select one of the download sessions and set it as the current session"
            '
            'BTT_CURR_SESSION_SET_LAST
            '
            Me.BTT_CURR_SESSION_SET_LAST.Image = Global.SCrawler.My.Resources.Resources.ArrowDownPic_Blue_24
            Me.BTT_CURR_SESSION_SET_LAST.Name = "BTT_CURR_SESSION_SET_LAST"
            Me.BTT_CURR_SESSION_SET_LAST.Size = New System.Drawing.Size(352, 22)
            Me.BTT_CURR_SESSION_SET_LAST.Text = "Set last download session as current session"
            '
            'BTT_MERGE_SESSIONS
            '
            Me.BTT_MERGE_SESSIONS.AutoToolTip = True
            Me.BTT_MERGE_SESSIONS.Image = Global.SCrawler.My.Resources.Resources.DBPic_32
            Me.BTT_MERGE_SESSIONS.Name = "BTT_MERGE_SESSIONS"
            Me.BTT_MERGE_SESSIONS.Size = New System.Drawing.Size(352, 22)
            Me.BTT_MERGE_SESSIONS.Text = "Merge sessions"
            Me.BTT_MERGE_SESSIONS.ToolTipText = "Merge multiple session feeds into one"
            '
            'BTT_CLEAR_DAILY
            '
            Me.BTT_CLEAR_DAILY.AutoToolTip = True
            Me.BTT_CLEAR_DAILY.Image = Global.SCrawler.My.Resources.Resources.DeletePic_24
            Me.BTT_CLEAR_DAILY.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_CLEAR_DAILY.Name = "BTT_CLEAR_DAILY"
            Me.BTT_CLEAR_DAILY.Size = New System.Drawing.Size(352, 22)
            Me.BTT_CLEAR_DAILY.Text = "Clear session"
            Me.BTT_CLEAR_DAILY.ToolTipText = "Clear current session"
            '
            'BTT_RESET_DAILY
            '
            Me.BTT_RESET_DAILY.AutoToolTip = True
            Me.BTT_RESET_DAILY.Image = Global.SCrawler.My.Resources.Resources.RefreshPic_24
            Me.BTT_RESET_DAILY.Name = "BTT_RESET_DAILY"
            Me.BTT_RESET_DAILY.Size = New System.Drawing.Size(352, 22)
            Me.BTT_RESET_DAILY.Text = "Reset current session"
            Me.BTT_RESET_DAILY.ToolTipText = "A new file will be created for the current session"
            '
            'BTT_MERGE_FEEDS
            '
            Me.BTT_MERGE_FEEDS.AutoToolTip = True
            Me.BTT_MERGE_FEEDS.Image = Global.SCrawler.My.Resources.Resources.DBPic_32
            Me.BTT_MERGE_FEEDS.Name = "BTT_MERGE_FEEDS"
            Me.BTT_MERGE_FEEDS.Size = New System.Drawing.Size(352, 22)
            Me.BTT_MERGE_FEEDS.Text = "Merge special feeds"
            Me.BTT_MERGE_FEEDS.ToolTipText = "Merge multiple special feeds into one"
            '
            'BTT_CHECK_ALL
            '
            Me.BTT_CHECK_ALL.Name = "BTT_CHECK_ALL"
            Me.BTT_CHECK_ALL.Size = New System.Drawing.Size(352, 22)
            Me.BTT_CHECK_ALL.Text = "Select all"
            '
            'BTT_CHECK_NONE
            '
            Me.BTT_CHECK_NONE.Name = "BTT_CHECK_NONE"
            Me.BTT_CHECK_NONE.Size = New System.Drawing.Size(352, 22)
            Me.BTT_CHECK_NONE.Text = "Select none"
            '
            'BTT_VIEW_SAVE
            '
            Me.BTT_VIEW_SAVE.Name = "BTT_VIEW_SAVE"
            Me.BTT_VIEW_SAVE.Size = New System.Drawing.Size(352, 22)
            Me.BTT_VIEW_SAVE.Text = "Save current view"
            '
            'BTT_VIEW_LOAD
            '
            Me.BTT_VIEW_LOAD.AutoToolTip = True
            Me.BTT_VIEW_LOAD.Name = "BTT_VIEW_LOAD"
            Me.BTT_VIEW_LOAD.Size = New System.Drawing.Size(352, 22)
            Me.BTT_VIEW_LOAD.Text = "Load view (from saved)"
            Me.BTT_VIEW_LOAD.ToolTipText = "Load one of your previously saved views"
            '
            'SEP_0
            '
            Me.SEP_0.Name = "SEP_0"
            Me.SEP_0.Size = New System.Drawing.Size(6, 25)
            '
            'MENU_DOWN
            '
            Me.MENU_DOWN.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
            Me.MENU_DOWN.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_DOWN_ALL, Me.BTT_DOWN_SELECTED})
            Me.MENU_DOWN.Image = Global.SCrawler.My.Resources.Resources.StartPic_Green_16
            Me.MENU_DOWN.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.MENU_DOWN.Name = "MENU_DOWN"
            Me.MENU_DOWN.Size = New System.Drawing.Size(29, 22)
            Me.MENU_DOWN.Text = "Download"
            Me.MENU_DOWN.Visible = False
            '
            'BTT_DOWN_ALL
            '
            Me.BTT_DOWN_ALL.Image = Global.SCrawler.My.Resources.Resources.StartPic_Green_16
            Me.BTT_DOWN_ALL.Name = "BTT_DOWN_ALL"
            Me.BTT_DOWN_ALL.Size = New System.Drawing.Size(174, 22)
            Me.BTT_DOWN_ALL.Tag = "a"
            Me.BTT_DOWN_ALL.Text = "Download ALL"
            '
            'BTT_DOWN_SELECTED
            '
            Me.BTT_DOWN_SELECTED.Image = Global.SCrawler.My.Resources.Resources.StartPic_Green_16
            Me.BTT_DOWN_SELECTED.Name = "BTT_DOWN_SELECTED"
            Me.BTT_DOWN_SELECTED.Size = New System.Drawing.Size(174, 22)
            Me.BTT_DOWN_SELECTED.Tag = "s"
            Me.BTT_DOWN_SELECTED.Text = "Download selected"
            '
            'BTT_REFRESH
            '
            Me.BTT_REFRESH.Image = Global.SCrawler.My.Resources.Resources.RefreshPic_24
            Me.BTT_REFRESH.ImageTransparentColor = System.Drawing.Color.Magenta
            Me.BTT_REFRESH.Name = "BTT_REFRESH"
            Me.BTT_REFRESH.Size = New System.Drawing.Size(66, 22)
            Me.BTT_REFRESH.Text = "Refresh"
            Me.BTT_REFRESH.ToolTipText = "Refresh data list"
            '
            'TP_DATA
            '
            Me.TP_DATA.AutoScroll = True
            Me.TP_DATA.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.TP_DATA.ColumnCount = 1
            Me.TP_DATA.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_DATA.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TP_DATA.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_DATA.Location = New System.Drawing.Point(0, 25)
            Me.TP_DATA.Name = "TP_DATA"
            Me.TP_DATA.RowCount = 11
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.TP_DATA.Size = New System.Drawing.Size(484, 436)
            Me.TP_DATA.TabIndex = 1
            '
            'BTT_CHECK_INVERT
            '
            Me.BTT_CHECK_INVERT.Name = "BTT_CHECK_INVERT"
            Me.BTT_CHECK_INVERT.Size = New System.Drawing.Size(352, 22)
            Me.BTT_CHECK_INVERT.Text = "Invert selection"
            '
            'DownloadFeedForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.SystemColors.Window
            Me.ClientSize = New System.Drawing.Size(484, 461)
            Me.Controls.Add(Me.TP_DATA)
            Me.Controls.Add(Me.ToolbarTOP)
            Me.ForeColor = System.Drawing.SystemColors.WindowText
            Me.Icon = Global.SCrawler.My.Resources.Resources.RSSIcon_32
            Me.KeyPreview = True
            Me.MinimumSize = New System.Drawing.Size(300, 300)
            Me.Name = "DownloadFeedForm"
            Me.Text = "Feed"
            Me.ToolbarTOP.ResumeLayout(False)
            Me.ToolbarTOP.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Private WithEvents BTT_REFRESH As ToolStripButton
        Private WithEvents BTT_CLEAR_DAILY As ToolStripMenuItem
        Private WithEvents MENU_LOAD_SESSION As ToolStripDropDownButton
        Private WithEvents BTT_LOAD_SESSION_LAST As ToolStripMenuItem
        Private WithEvents BTT_LOAD_SESSION_CHOOSE As ToolStripMenuItem
        Private WithEvents SEP_0 As ToolStripSeparator
        Private WithEvents ToolbarTOP As ToolStrip
        Private WithEvents TP_DATA As TableLayoutPanel
        Private WithEvents OPT_DEFAULT As ToolStripMenuItem
        Private WithEvents OPT_SUBSCRIPTIONS As ToolStripMenuItem
        Private WithEvents MENU_DOWN As ToolStripDropDownButton
        Private WithEvents BTT_DOWN_ALL As ToolStripMenuItem
        Private WithEvents BTT_DOWN_SELECTED As ToolStripMenuItem
        Private WithEvents BTT_LOAD_SESSION_CURRENT As ToolStripMenuItem
        Private WithEvents BTT_FEED_ADD_FAV As ToolStripMenuItem
        Private WithEvents BTT_FEED_REMOVE_FAV As ToolStripMenuItem
        Private WithEvents BTT_FEED_ADD_SPEC As ToolStripMenuItem
        Private WithEvents BTT_FEED_REMOVE_SPEC As ToolStripMenuItem
        Private WithEvents BTT_LOAD_FAV As ToolStripMenuItem
        Private WithEvents BTT_LOAD_SPEC As ToolStripMenuItem
        Private WithEvents BTT_FEED_CLEAR_FAV As ToolStripMenuItem
        Private WithEvents BTT_FEED_CLEAR_SPEC As ToolStripMenuItem
        Private WithEvents BTT_FEED_DELETE_SPEC As ToolStripMenuItem
        Private WithEvents BTT_FEED_DELETE_DAILY_LIST As ToolStripMenuItem
        Private WithEvents BTT_FEED_DELETE_DAILY_DATE As ToolStripMenuItem
        Private WithEvents BTT_MERGE_SESSIONS As ToolStripMenuItem
        Private WithEvents BTT_MERGE_FEEDS As ToolStripMenuItem
        Private WithEvents BTT_FEED_ADD_FAV_REMOVE As ToolStripMenuItem
        Private WithEvents BTT_FEED_ADD_SPEC_REMOVE As ToolStripMenuItem
        Private WithEvents BTT_CHECK_ALL As ToolStripMenuItem
        Private WithEvents BTT_CHECK_NONE As ToolStripMenuItem
        Private WithEvents BTT_COPY_TO As ToolStripMenuItem
        Private WithEvents BTT_MOVE_TO As ToolStripMenuItem
        Private WithEvents BTT_VIEW_SAVE As ToolStripMenuItem
        Private WithEvents BTT_VIEW_LOAD As ToolStripMenuItem
        Private WithEvents BTT_CURR_SESSION_SET As ToolStripMenuItem
        Private WithEvents BTT_COPY_SPEC_TO As ToolStripMenuItem
        Private WithEvents BTT_MOVE_SPEC_TO As ToolStripMenuItem
        Private WithEvents BTT_RESET_DAILY As ToolStripMenuItem
        Private WithEvents BTT_CURR_SESSION_SET_LAST As ToolStripMenuItem
        Private WithEvents BTT_CHECK_INVERT As ToolStripMenuItem
    End Class
End Namespace