<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Friend Class VideosDownloaderForm : Inherits System.Windows.Forms.Form
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
        Me.ToolbarTOP = New System.Windows.Forms.ToolStrip()
        Me.BTT_ADD = New System.Windows.Forms.ToolStripButton()
        Me.BTT_ADD_LIST = New System.Windows.Forms.ToolStripButton()
        Me.BTT_DELETE = New System.Windows.Forms.ToolStripButton()
        Me.BTT_DOWN = New System.Windows.Forms.ToolStripButton()
        Me.BTT_OPEN_PATH = New System.Windows.Forms.ToolStripButton()
        Me.ToolbarBOTTOM = New System.Windows.Forms.StatusStrip()
        Me.PR_V = New System.Windows.Forms.ToolStripProgressBar()
        Me.LBL_STATUS = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LIST_VIDEOS = New System.Windows.Forms.ListBox()
        SEP_1 = New System.Windows.Forms.ToolStripSeparator()
        SEP_2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolbarTOP.SuspendLayout()
        Me.ToolbarBOTTOM.SuspendLayout()
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
        'ToolbarTOP
        '
        Me.ToolbarTOP.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolbarTOP.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BTT_ADD, Me.BTT_ADD_LIST, Me.BTT_DELETE, SEP_1, Me.BTT_DOWN, SEP_2, Me.BTT_OPEN_PATH})
        Me.ToolbarTOP.Location = New System.Drawing.Point(0, 0)
        Me.ToolbarTOP.Name = "ToolbarTOP"
        Me.ToolbarTOP.Size = New System.Drawing.Size(524, 25)
        Me.ToolbarTOP.TabIndex = 0
        '
        'BTT_ADD
        '
        Me.BTT_ADD.AutoToolTip = False
        Me.BTT_ADD.Image = Global.SCrawler.My.Resources.Resources.PlusPIC
        Me.BTT_ADD.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_ADD.Name = "BTT_ADD"
        Me.BTT_ADD.Size = New System.Drawing.Size(75, 22)
        Me.BTT_ADD.Text = "Add (Ins)"
        '
        'BTT_ADD_LIST
        '
        Me.BTT_ADD_LIST.AutoToolTip = False
        Me.BTT_ADD_LIST.Image = Global.SCrawler.My.Resources.Resources.PlusPIC
        Me.BTT_ADD_LIST.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_ADD_LIST.Name = "BTT_ADD_LIST"
        Me.BTT_ADD_LIST.Size = New System.Drawing.Size(67, 22)
        Me.BTT_ADD_LIST.Text = "Add list"
        '
        'BTT_DELETE
        '
        Me.BTT_DELETE.AutoToolTip = False
        Me.BTT_DELETE.Image = Global.SCrawler.My.Resources.Resources.Delete
        Me.BTT_DELETE.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_DELETE.Name = "BTT_DELETE"
        Me.BTT_DELETE.Size = New System.Drawing.Size(83, 22)
        Me.BTT_DELETE.Text = "Delete (F8)"
        '
        'BTT_DOWN
        '
        Me.BTT_DOWN.AutoToolTip = False
        Me.BTT_DOWN.Image = Global.SCrawler.My.Resources.Resources.StartPic_01_Green_16
        Me.BTT_DOWN.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_DOWN.Name = "BTT_DOWN"
        Me.BTT_DOWN.Size = New System.Drawing.Size(104, 22)
        Me.BTT_DOWN.Text = "Download (F5)"
        '
        'BTT_OPEN_PATH
        '
        Me.BTT_OPEN_PATH.AutoToolTip = False
        Me.BTT_OPEN_PATH.Image = Global.SCrawler.My.Resources.Resources.Folder_32
        Me.BTT_OPEN_PATH.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.BTT_OPEN_PATH.Name = "BTT_OPEN_PATH"
        Me.BTT_OPEN_PATH.Size = New System.Drawing.Size(120, 22)
        Me.BTT_OPEN_PATH.Text = "Open saving path"
        '
        'ToolbarBOTTOM
        '
        Me.ToolbarBOTTOM.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.PR_V, Me.LBL_STATUS})
        Me.ToolbarBOTTOM.Location = New System.Drawing.Point(0, 339)
        Me.ToolbarBOTTOM.Name = "ToolbarBOTTOM"
        Me.ToolbarBOTTOM.Size = New System.Drawing.Size(524, 22)
        Me.ToolbarBOTTOM.TabIndex = 1
        '
        'PR_V
        '
        Me.PR_V.Name = "PR_V"
        Me.PR_V.Size = New System.Drawing.Size(200, 16)
        Me.PR_V.Visible = False
        '
        'LBL_STATUS
        '
        Me.LBL_STATUS.Name = "LBL_STATUS"
        Me.LBL_STATUS.Size = New System.Drawing.Size(0, 17)
        '
        'LIST_VIDEOS
        '
        Me.LIST_VIDEOS.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LIST_VIDEOS.FormattingEnabled = True
        Me.LIST_VIDEOS.Location = New System.Drawing.Point(0, 25)
        Me.LIST_VIDEOS.Name = "LIST_VIDEOS"
        Me.LIST_VIDEOS.Size = New System.Drawing.Size(524, 314)
        Me.LIST_VIDEOS.TabIndex = 2
        '
        'VideosDownloaderForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(524, 361)
        Me.Controls.Add(Me.LIST_VIDEOS)
        Me.Controls.Add(Me.ToolbarBOTTOM)
        Me.Controls.Add(Me.ToolbarTOP)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(540, 400)
        Me.Name = "VideosDownloaderForm"
        Me.ShowIcon = False
        Me.Text = "Download Videos"
        Me.ToolbarTOP.ResumeLayout(False)
        Me.ToolbarTOP.PerformLayout()
        Me.ToolbarBOTTOM.ResumeLayout(False)
        Me.ToolbarBOTTOM.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents ToolbarTOP As ToolStrip
    Private WithEvents BTT_ADD As ToolStripButton
    Private WithEvents BTT_ADD_LIST As ToolStripButton
    Private WithEvents BTT_DELETE As ToolStripButton
    Private WithEvents ToolbarBOTTOM As StatusStrip
    Private WithEvents PR_V As ToolStripProgressBar
    Private WithEvents LBL_STATUS As ToolStripStatusLabel
    Private WithEvents LIST_VIDEOS As ListBox
    Private WithEvents BTT_DOWN As ToolStripButton
    Private WithEvents BTT_OPEN_PATH As ToolStripButton
End Class