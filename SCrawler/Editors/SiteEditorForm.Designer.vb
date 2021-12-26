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
            Me.TXT_TOKEN = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_AUTH = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            Me.TP_MAIN.SuspendLayout()
            CType(Me.TXT_PATH, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_COOKIES, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_TOKEN, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_AUTH, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.CONTAINER_MAIN.ContentPanel.SuspendLayout()
            Me.CONTAINER_MAIN.SuspendLayout()
            Me.SuspendLayout()
            '
            'TP_MAIN
            '
            Me.TP_MAIN.ColumnCount = 1
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            Me.TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.TP_MAIN.Controls.Add(Me.TXT_PATH, 0, 0)
            Me.TP_MAIN.Controls.Add(Me.TXT_COOKIES, 0, 1)
            Me.TP_MAIN.Controls.Add(Me.TXT_TOKEN, 0, 2)
            Me.TP_MAIN.Controls.Add(Me.TXT_AUTH, 0, 3)
            Me.TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TP_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.TP_MAIN.Name = "TP_MAIN"
            Me.TP_MAIN.RowCount = 4
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            Me.TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            Me.TP_MAIN.Size = New System.Drawing.Size(544, 132)
            Me.TP_MAIN.TabIndex = 0
            '
            'TXT_PATH
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Index = 0
            ActionButton1.Name = "BTT_OPEN"
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Index = 1
            ActionButton2.Name = "BTT_CLEAR"
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
            ActionButton3.Index = 0
            ActionButton3.Name = "BTT_EDIT"
            ActionButton4.BackgroundImage = CType(resources.GetObject("ActionButton4.BackgroundImage"), System.Drawing.Image)
            ActionButton4.Index = 1
            ActionButton4.Name = "BTT_CLEAR"
            Me.TXT_COOKIES.Buttons.Add(ActionButton3)
            Me.TXT_COOKIES.Buttons.Add(ActionButton4)
            Me.TXT_COOKIES.CaptionText = "Cookies"
            Me.TXT_COOKIES.ClearTextByButtonClear = False
            Me.TXT_COOKIES.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_COOKIES.Location = New System.Drawing.Point(3, 36)
            Me.TXT_COOKIES.Name = "TXT_COOKIES"
            Me.TXT_COOKIES.Size = New System.Drawing.Size(538, 22)
            Me.TXT_COOKIES.TabIndex = 1
            Me.TXT_COOKIES.TextBoxReadOnly = True
            '
            'TXT_TOKEN
            '
            ActionButton5.BackgroundImage = CType(resources.GetObject("ActionButton5.BackgroundImage"), System.Drawing.Image)
            ActionButton5.Index = 0
            ActionButton5.Name = "BTT_CLEAR"
            Me.TXT_TOKEN.Buttons.Add(ActionButton5)
            Me.TXT_TOKEN.CaptionText = "Token"
            Me.TXT_TOKEN.CaptionToolTipEnabled = True
            Me.TXT_TOKEN.CaptionToolTipText = "Set token from [x-csrf-token] response header"
            Me.TXT_TOKEN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_TOKEN.Location = New System.Drawing.Point(3, 69)
            Me.TXT_TOKEN.Name = "TXT_TOKEN"
            Me.TXT_TOKEN.Size = New System.Drawing.Size(538, 22)
            Me.TXT_TOKEN.TabIndex = 2
            '
            'TXT_AUTH
            '
            ActionButton6.BackgroundImage = CType(resources.GetObject("ActionButton6.BackgroundImage"), System.Drawing.Image)
            ActionButton6.Index = 0
            ActionButton6.Name = "BTT_CLEAR"
            Me.TXT_AUTH.Buttons.Add(ActionButton6)
            Me.TXT_AUTH.CaptionText = "Authorization"
            Me.TXT_AUTH.CaptionToolTipEnabled = True
            Me.TXT_AUTH.CaptionToolTipText = "Set authorization from [authorization] response header. This field must start fro" &
    "m [Bearer] key word"
            Me.TXT_AUTH.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_AUTH.Location = New System.Drawing.Point(3, 102)
            Me.TXT_AUTH.Name = "TXT_AUTH"
            Me.TXT_AUTH.Size = New System.Drawing.Size(538, 22)
            Me.TXT_AUTH.TabIndex = 3
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            Me.CONTAINER_MAIN.ContentPanel.Controls.Add(Me.TP_MAIN)
            Me.CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(544, 132)
            Me.CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CONTAINER_MAIN.LeftToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            Me.CONTAINER_MAIN.RightToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Size = New System.Drawing.Size(544, 132)
            Me.CONTAINER_MAIN.TabIndex = 0
            Me.CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'SiteEditorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(544, 132)
            Me.Controls.Add(Me.CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(560, 171)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(560, 171)
            Me.Name = "SiteEditorForm"
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Site"
            Me.TP_MAIN.ResumeLayout(False)
            CType(Me.TXT_PATH, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_COOKIES, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_TOKEN, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_AUTH, System.ComponentModel.ISupportInitialize).EndInit()
            Me.CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            Me.CONTAINER_MAIN.ResumeLayout(False)
            Me.CONTAINER_MAIN.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents CONTAINER_MAIN As ToolStripContainer
        Private WithEvents TXT_PATH As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_COOKIES As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_TOKEN As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_AUTH As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TP_MAIN As TableLayoutPanel
    End Class
End Namespace