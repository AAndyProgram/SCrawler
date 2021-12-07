<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Friend Class DownloadedInfoForm : Inherits System.Windows.Forms.Form
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(DownloadedInfoForm))
        Me.ToolbarTOP = New System.Windows.Forms.ToolStrip()
        Me.MENU_VIEW = New System.Windows.Forms.ToolStripDropDownButton()
        Me.MENU_VIEW_SESSION = New System.Windows.Forms.ToolStripMenuItem()
        Me.MENU_VIEW_ALL = New System.Windows.Forms.ToolStripMenuItem()
        Me.BTT_REFRESH = New System.Windows.Forms.ToolStripButton()
        Me.ToolbarBOTTOM = New System.Windows.Forms.StatusStrip()
        Me.LIST_DOWN = New System.Windows.Forms.ListBox()
        Me.ToolbarTOP.SuspendLayout()
        Me.SuspendLayout()
        '
        'ToolbarTOP
        '
        Me.ToolbarTOP.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolbarTOP.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MENU_VIEW, Me.BTT_REFRESH})
        Me.ToolbarTOP.Location = New System.Drawing.Point(0, 0)
        Me.ToolbarTOP.Name = "ToolbarTOP"
        Me.ToolbarTOP.Size = New System.Drawing.Size(554, 25)
        Me.ToolbarTOP.TabIndex = 0
        '
        'MENU_VIEW
        '
        Me.MENU_VIEW.AutoToolTip = False
        Me.MENU_VIEW.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.MENU_VIEW.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MENU_VIEW_SESSION, Me.MENU_VIEW_ALL})
        Me.MENU_VIEW.Image = CType(resources.GetObject("MENU_VIEW.Image"), System.Drawing.Image)
        Me.MENU_VIEW.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.MENU_VIEW.Name = "MENU_VIEW"
        Me.MENU_VIEW.Size = New System.Drawing.Size(45, 22)
        Me.MENU_VIEW.Text = "View"
        '
        'MENU_VIEW_SESSION
        '
        Me.MENU_VIEW_SESSION.AutoToolTip = True
        Me.MENU_VIEW_SESSION.Name = "MENU_VIEW_SESSION"
        Me.MENU_VIEW_SESSION.Size = New System.Drawing.Size(180, 22)
        Me.MENU_VIEW_SESSION.Text = "Session"
        Me.MENU_VIEW_SESSION.ToolTipText = "Show downloaded users by this session"
        '
        'MENU_VIEW_ALL
        '
        Me.MENU_VIEW_ALL.AutoToolTip = True
        Me.MENU_VIEW_ALL.Name = "MENU_VIEW_ALL"
        Me.MENU_VIEW_ALL.Size = New System.Drawing.Size(180, 22)
        Me.MENU_VIEW_ALL.Text = "All"
        Me.MENU_VIEW_ALL.ToolTipText = "Show all users (sorted by latest download)"
        '
        'BTT_REFRESH
        '
        Me.BTT_REFRESH.Image = Global.SCrawler.My.Resources.Resources.Refresh
        Me.BTT_REFRESH.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_REFRESH.Name = "BTT_REFRESH"
        Me.BTT_REFRESH.Size = New System.Drawing.Size(89, 22)
        Me.BTT_REFRESH.Text = "Refresh (F5)"
        Me.BTT_REFRESH.ToolTipText = "Force list refresh"
        '
        'ToolbarBOTTOM
        '
        Me.ToolbarBOTTOM.Location = New System.Drawing.Point(0, 389)
        Me.ToolbarBOTTOM.Name = "ToolbarBOTTOM"
        Me.ToolbarBOTTOM.Size = New System.Drawing.Size(554, 22)
        Me.ToolbarBOTTOM.TabIndex = 1
        '
        'LIST_DOWN
        '
        Me.LIST_DOWN.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LIST_DOWN.FormattingEnabled = True
        Me.LIST_DOWN.Location = New System.Drawing.Point(0, 25)
        Me.LIST_DOWN.Name = "LIST_DOWN"
        Me.LIST_DOWN.Size = New System.Drawing.Size(554, 364)
        Me.LIST_DOWN.TabIndex = 2
        '
        'DownloadedInfoForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(554, 411)
        Me.Controls.Add(Me.LIST_DOWN)
        Me.Controls.Add(Me.ToolbarBOTTOM)
        Me.Controls.Add(Me.ToolbarTOP)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(570, 450)
        Me.Name = "DownloadedInfoForm"
        Me.ShowIcon = False
        Me.Text = "Downloaded items"
        Me.ToolbarTOP.ResumeLayout(False)
        Me.ToolbarTOP.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents ToolbarTOP As ToolStrip
    Private WithEvents MENU_VIEW As ToolStripDropDownButton
    Private WithEvents MENU_VIEW_SESSION As ToolStripMenuItem
    Private WithEvents MENU_VIEW_ALL As ToolStripMenuItem
    Private WithEvents BTT_REFRESH As ToolStripButton
    Private WithEvents ToolbarBOTTOM As StatusStrip
    Private WithEvents LIST_DOWN As ListBox
End Class