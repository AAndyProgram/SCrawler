Namespace Editors
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class SiteEditorForm : Inherits System.Windows.Forms.Form
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
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SiteEditorForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton4 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton5 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton6 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Me.TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            Me.TXT_PATH = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_COOKIES = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TP_SITE_PROPS = New SCrawler.Editors.SiteDefaults()
            Me.TXT_PATH_SAVED_POSTS = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CH_GET_USER_MEDIA_ONLY = New System.Windows.Forms.CheckBox()
            Me.CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            Me.TT_MAIN = New System.Windows.Forms.ToolTip(Me.components)
            Me.TP_MAIN.SuspendLayout()
            CType(Me.TXT_PATH, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_COOKIES, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_PATH_SAVED_POSTS, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.CONTAINER_MAIN.ContentPanel.SuspendLayout()
            Me.CONTAINER_MAIN.SuspendLayout()
            Me.SuspendLayout()
            '
            'TP_MAIN
            '
            Me.TP_MAIN.ColumnCount = 1
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.Controls.Add(Me.TXT_PATH, 0, 0)
            Me.TP_MAIN.Controls.Add(Me.TXT_COOKIES, 0, 2)
            Me.TP_MAIN.Controls.Add(Me.TP_SITE_PROPS, 0, 4)
            Me.TP_MAIN.Controls.Add(Me.TXT_PATH_SAVED_POSTS, 0, 1)
            Me.TP_MAIN.Controls.Add(Me.CH_GET_USER_MEDIA_ONLY, 0, 3)
            Me.TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.TP_MAIN.Name = "TP_MAIN"
            Me.TP_MAIN.RowCount = 5
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.Size = New System.Drawing.Size(544, 219)
            Me.TP_MAIN.TabIndex = 0
            '
            'TXT_PATH
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Name = "Open"
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Name = "Clear"
            Me.TXT_PATH.Buttons.Add(ActionButton1)
            Me.TXT_PATH.Buttons.Add(ActionButton2)
            Me.TXT_PATH.CaptionText = "Path"
            Me.TXT_PATH.CaptionToolTipEnabled = True
            Me.TXT_PATH.CaptionToolTipText = "Specific path to store Twitter files"
            Me.TXT_PATH.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_PATH.Location = New System.Drawing.Point(3, 3)
            Me.TXT_PATH.Name = "TXT_PATH"
            Me.TXT_PATH.Size = New System.Drawing.Size(538, 22)
            Me.TXT_PATH.TabIndex = 0
            '
            'TXT_COOKIES
            '
            ActionButton3.BackgroundImage = CType(resources.GetObject("ActionButton3.BackgroundImage"), System.Drawing.Image)
            ActionButton3.Name = "Edit"
            ActionButton4.BackgroundImage = CType(resources.GetObject("ActionButton4.BackgroundImage"), System.Drawing.Image)
            ActionButton4.Name = "Clear"
            Me.TXT_COOKIES.Buttons.Add(ActionButton3)
            Me.TXT_COOKIES.Buttons.Add(ActionButton4)
            Me.TXT_COOKIES.CaptionText = "Cookies"
            Me.TXT_COOKIES.ClearTextByButtonClear = False
            Me.TXT_COOKIES.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_COOKIES.Location = New System.Drawing.Point(3, 59)
            Me.TXT_COOKIES.Name = "TXT_COOKIES"
            Me.TXT_COOKIES.Size = New System.Drawing.Size(538, 22)
            Me.TXT_COOKIES.TabIndex = 2
            Me.TXT_COOKIES.TextBoxReadOnly = True
            '
            'TP_SITE_PROPS
            '
            Me.TP_SITE_PROPS.BaseControlsPadding = New System.Windows.Forms.Padding(97, 0, 0, 0)
            Me.TP_SITE_PROPS.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
            Me.TP_SITE_PROPS.ColumnCount = 1
            Me.TP_SITE_PROPS.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_SITE_PROPS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_SITE_PROPS.Location = New System.Drawing.Point(3, 112)
            Me.TP_SITE_PROPS.Name = "TP_SITE_PROPS"
            Me.TP_SITE_PROPS.RowCount = 4
            Me.TP_SITE_PROPS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TP_SITE_PROPS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TP_SITE_PROPS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
            Me.TP_SITE_PROPS.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_SITE_PROPS.Size = New System.Drawing.Size(538, 104)
            Me.TP_SITE_PROPS.TabIndex = 4
            '
            'TXT_PATH_SAVED_POSTS
            '
            ActionButton5.BackgroundImage = CType(resources.GetObject("ActionButton5.BackgroundImage"), System.Drawing.Image)
            ActionButton5.Name = "Open"
            ActionButton6.BackgroundImage = CType(resources.GetObject("ActionButton6.BackgroundImage"), System.Drawing.Image)
            ActionButton6.Name = "Clear"
            Me.TXT_PATH_SAVED_POSTS.Buttons.Add(ActionButton5)
            Me.TXT_PATH_SAVED_POSTS.Buttons.Add(ActionButton6)
            Me.TXT_PATH_SAVED_POSTS.CaptionText = "Saved posts path"
            Me.TXT_PATH_SAVED_POSTS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_PATH_SAVED_POSTS.Location = New System.Drawing.Point(3, 31)
            Me.TXT_PATH_SAVED_POSTS.Name = "TXT_PATH_SAVED_POSTS"
            Me.TXT_PATH_SAVED_POSTS.Size = New System.Drawing.Size(538, 22)
            Me.TXT_PATH_SAVED_POSTS.TabIndex = 1
            '
            'CH_GET_USER_MEDIA_ONLY
            '
            Me.CH_GET_USER_MEDIA_ONLY.AutoSize = True
            Me.CH_GET_USER_MEDIA_ONLY.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CH_GET_USER_MEDIA_ONLY.Location = New System.Drawing.Point(3, 87)
            Me.CH_GET_USER_MEDIA_ONLY.Name = "CH_GET_USER_MEDIA_ONLY"
            Me.CH_GET_USER_MEDIA_ONLY.Padding = New System.Windows.Forms.Padding(100, 0, 0, 0)
            Me.CH_GET_USER_MEDIA_ONLY.Size = New System.Drawing.Size(538, 19)
            Me.CH_GET_USER_MEDIA_ONLY.TabIndex = 3
            Me.CH_GET_USER_MEDIA_ONLY.Text = "Get user media only"
            Me.CH_GET_USER_MEDIA_ONLY.UseVisualStyleBackColor = True
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            Me.CONTAINER_MAIN.ContentPanel.Controls.Add(Me.TP_MAIN)
            Me.CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(544, 219)
            Me.CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CONTAINER_MAIN.LeftToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            Me.CONTAINER_MAIN.RightToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Size = New System.Drawing.Size(544, 219)
            Me.CONTAINER_MAIN.TabIndex = 0
            Me.CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'SiteEditorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(544, 219)
            Me.Controls.Add(Me.CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(560, 258)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(560, 258)
            Me.Name = "SiteEditorForm"
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Site"
            Me.TP_MAIN.ResumeLayout(False)
            Me.TP_MAIN.PerformLayout()
            CType(Me.TXT_PATH, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_COOKIES, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_PATH_SAVED_POSTS, System.ComponentModel.ISupportInitialize).EndInit()
            Me.CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            Me.CONTAINER_MAIN.ResumeLayout(False)
            Me.CONTAINER_MAIN.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents CONTAINER_MAIN As ToolStripContainer
        Private WithEvents TXT_PATH As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_COOKIES As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TP_MAIN As TableLayoutPanel
        Private WithEvents TXT_PATH_SAVED_POSTS As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TP_SITE_PROPS As SiteDefaults
        Private WithEvents CH_GET_USER_MEDIA_ONLY As CheckBox
        Private WithEvents TT_MAIN As ToolTip
    End Class
End Namespace