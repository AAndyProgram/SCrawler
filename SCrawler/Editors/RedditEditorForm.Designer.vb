Namespace Editors
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class RedditEditorForm : Inherits System.Windows.Forms.Form
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
            Dim TP_MAIN As System.Windows.Forms.TableLayoutPanel
            Dim ActionButton1 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(RedditEditorForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton3 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Dim ActionButton4 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Me.TXT_PATH = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.TXT_COOKIES = New PersonalUtilities.Forms.Controls.TextBoxExtended()
            Me.CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            TP_MAIN = New System.Windows.Forms.TableLayoutPanel()
            TP_MAIN.SuspendLayout()
            CType(Me.TXT_PATH, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TXT_COOKIES, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.CONTAINER_MAIN.ContentPanel.SuspendLayout()
            Me.CONTAINER_MAIN.SuspendLayout()
            Me.SuspendLayout()
            '
            'TP_MAIN
            '
            TP_MAIN.ColumnCount = 1
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
            TP_MAIN.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            TP_MAIN.Controls.Add(Me.TXT_PATH, 0, 0)
            TP_MAIN.Controls.Add(Me.TXT_COOKIES, 0, 1)
            TP_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            TP_MAIN.Location = New System.Drawing.Point(0, 0)
            TP_MAIN.Name = "TP_MAIN"
            TP_MAIN.RowCount = 2
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_MAIN.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
            TP_MAIN.Size = New System.Drawing.Size(524, 80)
            TP_MAIN.TabIndex = 0
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
            Me.TXT_PATH.CaptionToolTipText = "Specific path to store Reddit files"
            Me.TXT_PATH.Dock = System.Windows.Forms.DockStyle.Fill
            Me.TXT_PATH.Location = New System.Drawing.Point(3, 3)
            Me.TXT_PATH.Name = "TXT_PATH"
            Me.TXT_PATH.Size = New System.Drawing.Size(518, 22)
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
            Me.TXT_COOKIES.Location = New System.Drawing.Point(3, 43)
            Me.TXT_COOKIES.Name = "TXT_COOKIES"
            Me.TXT_COOKIES.Size = New System.Drawing.Size(518, 22)
            Me.TXT_COOKIES.TabIndex = 1
            Me.TXT_COOKIES.TextBoxReadOnly = True
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            Me.CONTAINER_MAIN.ContentPanel.Controls.Add(TP_MAIN)
            Me.CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(524, 80)
            Me.CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CONTAINER_MAIN.LeftToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            Me.CONTAINER_MAIN.RightToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Size = New System.Drawing.Size(524, 80)
            Me.CONTAINER_MAIN.TabIndex = 0
            Me.CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'RedditEditorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(524, 80)
            Me.Controls.Add(Me.CONTAINER_MAIN)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
            Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
            Me.KeyPreview = True
            Me.MaximizeBox = False
            Me.MaximumSize = New System.Drawing.Size(540, 119)
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(540, 119)
            Me.Name = "RedditEditorForm"
            Me.ShowInTaskbar = False
            Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
            Me.Text = "Reddit"
            TP_MAIN.ResumeLayout(False)
            CType(Me.TXT_PATH, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TXT_COOKIES, System.ComponentModel.ISupportInitialize).EndInit()
            Me.CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            Me.CONTAINER_MAIN.ResumeLayout(False)
            Me.CONTAINER_MAIN.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents CONTAINER_MAIN As ToolStripContainer
        Private WithEvents TXT_PATH As PersonalUtilities.Forms.Controls.TextBoxExtended
        Private WithEvents TXT_COOKIES As PersonalUtilities.Forms.Controls.TextBoxExtended
    End Class
End Namespace