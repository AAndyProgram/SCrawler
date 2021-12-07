Namespace Editors
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Friend Class CollectionEditorForm : Inherits System.Windows.Forms.Form
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CollectionEditorForm))
            Dim ActionButton2 As PersonalUtilities.Forms.Controls.Base.ActionButton = New PersonalUtilities.Forms.Controls.Base.ActionButton()
            Me.CONTAINER_MAIN = New System.Windows.Forms.ToolStripContainer()
            Me.CMB_COLLECTIONS = New PersonalUtilities.Forms.Controls.ComboBoxExtended()
            Me.CONTAINER_MAIN.ContentPanel.SuspendLayout()
            Me.CONTAINER_MAIN.SuspendLayout()
            CType(Me.CMB_COLLECTIONS, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'CONTAINER_MAIN
            '
            '
            'CONTAINER_MAIN.ContentPanel
            '
            Me.CONTAINER_MAIN.ContentPanel.Controls.Add(Me.CMB_COLLECTIONS)
            Me.CONTAINER_MAIN.ContentPanel.Padding = New System.Windows.Forms.Padding(2, 0, 2, 0)
            Me.CONTAINER_MAIN.ContentPanel.Size = New System.Drawing.Size(454, 251)
            Me.CONTAINER_MAIN.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CONTAINER_MAIN.LeftToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Location = New System.Drawing.Point(0, 0)
            Me.CONTAINER_MAIN.Name = "CONTAINER_MAIN"
            Me.CONTAINER_MAIN.RightToolStripPanelVisible = False
            Me.CONTAINER_MAIN.Size = New System.Drawing.Size(454, 251)
            Me.CONTAINER_MAIN.TabIndex = 0
            Me.CONTAINER_MAIN.TopToolStripPanelVisible = False
            '
            'CMB_COLLECTIONS
            '
            ActionButton1.BackgroundImage = CType(resources.GetObject("ActionButton1.BackgroundImage"), System.Drawing.Image)
            ActionButton1.Index = 0
            ActionButton1.Name = "BTT_COMBOBOX_ARROW"
            ActionButton1.Visible = False
            ActionButton2.BackgroundImage = CType(resources.GetObject("ActionButton2.BackgroundImage"), System.Drawing.Image)
            ActionButton2.Index = 1
            ActionButton2.Name = "BTT_ADD"
            ActionButton2.ToolTipText = "Add new collection"
            Me.CMB_COLLECTIONS.Buttons.Add(ActionButton1)
            Me.CMB_COLLECTIONS.Buttons.Add(ActionButton2)
            Me.CMB_COLLECTIONS.Dock = System.Windows.Forms.DockStyle.Fill
            Me.CMB_COLLECTIONS.ListDropDownStyle = PersonalUtilities.Forms.Controls.ComboBoxExtended.ListMode.Simple
            Me.CMB_COLLECTIONS.Location = New System.Drawing.Point(2, 0)
            Me.CMB_COLLECTIONS.Name = "CMB_COLLECTIONS"
            Me.CMB_COLLECTIONS.Size = New System.Drawing.Size(452, 252)
            Me.CMB_COLLECTIONS.TabIndex = 0
            '
            'CollectionEditorForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(454, 251)
            Me.Controls.Add(Me.CONTAINER_MAIN)
            Me.KeyPreview = True
            Me.MinimizeBox = False
            Me.MinimumSize = New System.Drawing.Size(470, 290)
            Me.Name = "CollectionEditorForm"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.Text = "Collection"
            Me.CONTAINER_MAIN.ContentPanel.ResumeLayout(False)
            Me.CONTAINER_MAIN.ResumeLayout(False)
            Me.CONTAINER_MAIN.PerformLayout()
            CType(Me.CMB_COLLECTIONS, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        Private WithEvents CONTAINER_MAIN As ToolStripContainer
        Private WithEvents CMB_COLLECTIONS As PersonalUtilities.Forms.Controls.ComboBoxExtended
    End Class
End Namespace