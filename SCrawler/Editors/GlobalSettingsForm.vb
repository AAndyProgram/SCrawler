Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Forms.Toolbars
Namespace Editors
    Friend Class GlobalSettingsForm : Implements IOkCancelToolbar
        Private ReadOnly MyDefs As DefaultFormProps(Of FieldsChecker)
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormProps(Of FieldsChecker)
        End Sub
        Private Sub GlobalSettingsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDefs
                    .MyViewInitialize(Me, Settings.Design, True)
                    .AddOkCancelToolbar()
                    .DelegateClosingChecker()
                    With Settings
                        TXT_GLOBAL_PATH.Text = .GlobalPath.Value
                        TXT_IMAGE_LARGE.Value = .MaxLargeImageHeigh.Value
                        TXT_IMAGE_SMALL.Value = .MaxSmallImageHeigh.Value
                        TXT_COLLECTIONS_PATH.Text = .CollectionsPath
                        CH_SEPARATE_VIDEO_FOLDER.Checked = .SeparateVideoFolder.Value
                        CH_DEF_TEMP.Checked = .DefaultTemporary.Value
                        TXT_CHANNELS_COLUMNS.Value = .ChannelsImagesColumns.Value
                        TXT_CHANNELS_ROWS.Value = .ChannelsImagesRows.Value
                    End With
                    .MyFieldsChecker = New FieldsChecker
                    With .MyFieldsChecker
                        .AddControl(Of String)(TXT_GLOBAL_PATH, TXT_GLOBAL_PATH.CaptionText)
                        .AddControl(Of String)(TXT_COLLECTIONS_PATH, TXT_COLLECTIONS_PATH.CaptionText)
                        .EndLoaderOperations()
                    End With
                    .AppendDetectors()
                    .EndLoaderOperations()
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub ToolbarBttOK() Implements IOkCancelToolbar.ToolbarBttOK
            If MyDefs.MyFieldsChecker.AllParamsOK Then
                With Settings
                    .GlobalPath.Value = TXT_GLOBAL_PATH.Text
                    .MaxLargeImageHeigh.Value = CInt(TXT_IMAGE_LARGE.Value)
                    .MaxSmallImageHeigh.Value = CInt(TXT_IMAGE_SMALL.Value)
                    .SeparateVideoFolder.Value = CH_SEPARATE_VIDEO_FOLDER.Checked
                    .CollectionsPath.Value = TXT_COLLECTIONS_PATH.Text
                    .DefaultTemporary.Value = CH_DEF_TEMP.Checked
                    .ChannelsImagesRows.Value = CInt(TXT_CHANNELS_ROWS.Value)
                    .ChannelsImagesColumns.Value = CInt(TXT_CHANNELS_COLUMNS.Value)
                End With
                MyDefs.CloseForm()
            End If
        End Sub
        Private Sub ToolbarBttCancel() Implements IOkCancelToolbar.ToolbarBttCancel
            MyDefs.CloseForm(DialogResult.Cancel)
        End Sub
        Private Sub TXT_GLOBAL_PATH_ActionOnButtonClick(ByVal Sender As ActionButton) Handles TXT_GLOBAL_PATH.ActionOnButtonClick
            If Sender.DefaultButton = ActionButton.DefaultButtons.Open Then
                Dim f As SFile = SFile.SelectPath(Settings.GlobalPath.Value)
                If Not f.IsEmptyString Then TXT_GLOBAL_PATH.Text = f
            End If
        End Sub
    End Class
End Namespace