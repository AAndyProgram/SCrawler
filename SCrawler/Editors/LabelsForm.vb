Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Forms.Controls
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Functions.Messaging
Friend Class LabelsForm : Implements IOkCancelToolbar
    Private ReadOnly MyDefs As DefaultFormProps
    Friend ReadOnly Property LabelsList As List(Of String)
    Private _AnyLabelAdd As Boolean = False
    Friend Property MultiUser As Boolean = False
    Public Property MultiUserClearExists As Boolean = False
    Friend Sub New(ByVal LabelsArr As IEnumerable(Of String))
        InitializeComponent()
        LabelsList = New List(Of String)
        LabelsList.ListAddList(LabelsArr)
        MyDefs = New DefaultFormProps
    End Sub
    Private Sub LabelsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            With MyDefs
                .MyViewInitialize(Me, Settings.Design)
                .MyOkCancel = New OkCancelToolbar(Me, Me, CONTAINER_MAIN.BottomToolStripPanel)
                .MyOkCancel.AddThisToolbar()
                .DelegateClosingChecker()

                If Settings.Labels.Count > 0 Then
                    Dim items As New List(Of Integer)
                    CMB_LABELS.BeginUpdate()
                    For i% = 0 To Settings.Labels.Count - 1
                        If LabelsList.Contains(Settings.Labels(i)) Then items.Add(i)
                        CMB_LABELS.Items.Add(New ListItem(Settings.Labels(i)))
                    Next
                    CMB_LABELS.EndUpdate()
                    CMB_LABELS.ListCheckedIndexes = items
                End If

                .AppendDetectors()
                .EndLoaderOperations()
            End With
        Catch ex As Exception
            MyDefs.InvokeLoaderError(ex)
        End Try
    End Sub
    Private Sub LabelsForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Insert Then AddNewLabel() : e.Handled = True
    End Sub
    Private Sub LabelsForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        LabelsList.Clear()
    End Sub
    Private Sub ToolbarBttOK() Implements IOkCancelToolbar.ToolbarBttOK
        Try
            If MultiUser Then
                Dim m As New MMessage("You are changing labels for more one user" & vbNewLine & "What do you want to do?",
                                      "MultiUser labels changing",
                                      {New MsgBoxButton("Replace exists") With {.ToolTip = "For each user: all exists labels will be deleted and replaced to these labels"},
                                       New MsgBoxButton("Add to exists") With {.ToolTip = "For each user: these labels will be appended to exists labels"},
                                       New MsgBoxButton("Cancel")},
                                      MsgBoxStyle.Exclamation)
                Select Case MsgBoxE(m).Index
                    Case 0 : MultiUserClearExists = True
                    Case 1 : MultiUserClearExists = False
                    Case 2 : Exit Sub
                End Select
            End If
            LabelsList.Clear()
            Dim s As List(Of Integer) = CMB_LABELS.ListCheckedIndexes.ListIfNothing
            If s.Count > 0 Then
                For Each i% In s : LabelsList.Add(Settings.Labels(i)) : Next
            End If
            If _AnyLabelAdd Then Settings.Labels.Update()
            MyDefs.CloseForm()
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Choosing labels")
        End Try
    End Sub
    Private Sub ToolbarBttCancel() Implements IOkCancelToolbar.ToolbarBttCancel
        MyDefs.CloseForm(DialogResult.Cancel)
    End Sub
    Private Sub CMB_LABELS_ActionOnButtonClick(ByVal Sender As ActionButton) Handles CMB_LABELS.ActionOnButtonClick
        If Sender.DefaultButton = ActionButton.DefaultButtons.Add Then AddNewLabel()
    End Sub
    Private Sub CMB_LABELS_ActionOnButtonClearClick() Handles CMB_LABELS.ActionOnButtonClearClick
        CMB_LABELS.Clear(ComboBoxExtended.ClearMode.CheckedInexes)
    End Sub
    Private Sub AddNewLabel()
        Dim nl$ = InputBoxE("Enter new label name:", "New label")
        If Not nl.IsEmptyString Then
            If Settings.Labels.Contains(nl) Then
                MsgBoxE($"Label [{nl}] already exists")
            Else
                Settings.Labels.Add(nl)
                _AnyLabelAdd = True
                CMB_LABELS.Items.Add(New ListItem(nl))
            End If
        End If
    End Sub
End Class