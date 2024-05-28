' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports System.ComponentModel
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Toolbars
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Tools
Imports ECI = PersonalUtilities.Forms.Toolbars.EditToolbar.ControlItem
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Namespace DownloadObjects
    Friend Class SchedulerEditorForm
#Region "Declarations"
        Private Const TitleDefault As String = "Scheduler"
        Private WithEvents MyDefs As DefaultFormOptions
        Private WithEvents BTT_SETTINGS As ToolStripButton
        Private WithEvents BTT_CLONE As ToolStripButton
        Private ReadOnly MENU_SKIP As ToolStripDropDownButton
        Private WithEvents BTT_SKIP As ToolStripMenuItem
        Private WithEvents BTT_SKIP_MIN As ToolStripMenuItem
        Private WithEvents BTT_SKIP_DATE As ToolStripMenuItem
        Private WithEvents BTT_SKIP_RESET As ToolStripMenuItem
        Private WithEvents BTT_START As ToolStripButton
        Private WithEvents BTT_START_FORCE As ToolStripButton
        Private WithEvents BTT_PAUSE As ToolStripDropDownButton
        Private WithEvents PauseArr As AutoDownloaderPauseButtons
        Private WithEvents BTT_MOVE_UP As ToolStripButton
        Private WithEvents BTT_MOVE_DOWN As ToolStripButton
#End Region
#Region "Initializer"
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            BTT_SETTINGS = New ToolStripButton With {
                .Text = String.Empty,
                .AutoToolTip = True,
                .ToolTipText = "Change scheduler",
                .DisplayStyle = ToolStripItemDisplayStyle.Image,
                .Image = My.Resources.ScriptPic_32
            }
            BTT_CLONE = New ToolStripButton With {
                .Text = "Clone",
                .DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                .Image = My.Resources.PlusPic_24,
                .ToolTipText = "Create a copy of the selected plan",
                .AutoToolTip = True
            }
            MENU_SKIP = New ToolStripDropDownButton With {
                .Text = "Skip",
                .ToolTipText = String.Empty,
                .AutoToolTip = False,
                .DisplayStyle = ToolStripItemDisplayStyle.Text
            }
            BTT_SKIP = New ToolStripMenuItem With {
                .Text = "Skip",
                .ToolTipText = "Delay for the number of minutes configured in the task",
                .AutoToolTip = True,
                .DisplayStyle = ToolStripItemDisplayStyle.Text
            }
            BTT_SKIP_MIN = New ToolStripMenuItem With {
                .Text = "Delay for minutes",
                .ToolTipText = "Delay for a specific number of minutes",
                .AutoToolTip = True,
                .DisplayStyle = ToolStripItemDisplayStyle.Text,
                .Tag = "m"
            }
            BTT_SKIP_DATE = New ToolStripMenuItem With {
                .Text = "Delay by date/time",
                .ToolTipText = String.Empty,
                .AutoToolTip = False,
                .DisplayStyle = ToolStripItemDisplayStyle.Text,
                .Tag = "d"
            }
            BTT_SKIP_RESET = New ToolStripMenuItem With {
                .Text = "Delay reset",
                .ToolTipText = "Reset the delay you set earlier",
                .AutoToolTip = True,
                .DisplayStyle = ToolStripItemDisplayStyle.Text,
                .Tag = "r"
            }
            MENU_SKIP.DropDownItems.AddRange({BTT_SKIP, BTT_SKIP_MIN, BTT_SKIP_DATE, New ToolStripSeparator, BTT_SKIP_RESET})
            BTT_START = New ToolStripButton With {
                .Text = "Start",
                .Image = My.Resources.StartPic_Green_16,
                .ToolTipText = "Run selected plan",
                .AutoToolTip = True
            }
            BTT_START_FORCE = New ToolStripButton With {
                .Text = "Start (force)",
                .ToolTipText = "Force start of the current task",
                .AutoToolTip = True,
                .Image = My.Resources.StartPic_Green_16
            }
            BTT_PAUSE = New ToolStripDropDownButton With {
                .Text = "Pause",
                .Image = My.Resources.Pause_Blue_16,
                .ToolTipText = "Pause task",
                .AutoToolTip = True
            }
            BTT_MOVE_UP = New ToolStripButton With {
                .Text = String.Empty,
                .Image = PersonalUtilities.My.Resources.ArrowUpPic_Blue_32,
                .ToolTipText = "Move the selected task higher in the list",
                .AutoToolTip = True
            }
            BTT_MOVE_DOWN = New ToolStripButton With {
                .Text = String.Empty,
                .Image = PersonalUtilities.My.Resources.ArrowDownPic_Blue_32,
                .ToolTipText = "Move the selected task lower in the list",
                .AutoToolTip = True
            }
            PauseArr = New AutoDownloaderPauseButtons(AutoDownloaderPauseButtons.ButtonsPlace.Scheduler) With {
                       .MainFrameButtonsInstance = MainFrameObj.PauseButtons}
            Icon = ImageRenderer.GetIcon(My.Resources.ScriptPic_32, EDP.ReturnValue)
        End Sub
#End Region
#Region "Form handlers"
        Private Sub SchedulerEditorForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize()
                .AddEditToolbar({BTT_SETTINGS, ECI.Separator, ECI.Add, BTT_CLONE, ECI.Edit, ECI.Delete, ECI.Update, ECI.Separator,
                                 BTT_MOVE_UP, BTT_MOVE_DOWN, ECI.Separator,
                                 BTT_START, BTT_START_FORCE, MENU_SKIP, BTT_PAUSE})
                PauseArr.AddButtons(BTT_PAUSE, .MyEditToolbar.ToolStrip)
                Refill()
                SetTitle()
                .EndLoaderOperations(False)
            End With
            SchedulerCheckerThread = New Thread(New ThreadStart(AddressOf SchedulerChecker))
            SchedulerCheckerThread.SetApartmentState(ApartmentState.MTA)
            SchedulerCheckerThread.Start()
            Try : AddHandler Settings.Automation.PlanChanged, AddressOf Scheduler_PauseChanged : Catch : End Try
        End Sub
        Private _CloseRequested As Boolean = False
        Private Sub SchedulerEditorForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            _CloseRequested = True
            While If(SchedulerCheckerThread?.IsAlive, False) : Thread.Sleep(200) : End While
            Try : RemoveHandler Settings.Automation.PlanChanged, AddressOf Scheduler_PauseChanged : Catch : End Try
        End Sub
        Private Sub SchedulerEditorForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            If e.KeyCode = Keys.Escape Then
                Close()
            ElseIf e = ShowUsersButtonKey Then
                ShowPlanUsers()
                e.Handled = True
            End If
        End Sub
        Private Sub SchedulerEditorForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
            PauseArr.Dispose()
        End Sub
        Private Sub SchedulerEditorForm_SizeChanged(sender As Object, e As EventArgs) Handles Me.SizeChanged
            Try : ControlInvokeFast(LIST_PLANS, Sub()
                                                    COL_MAIN.Width = -2
                                                    LIST_PLANS.Refresh()
                                                End Sub, EDP.None) : Catch : End Try
        End Sub
#End Region
        Private Property ListSelectedIndex As Integer
            Get
                Return If(LIST_PLANS.SelectedIndices.Count > 0, LIST_PLANS.SelectedIndices(0), -1)
            End Get
            Set(ByVal indx As Integer)
                Try : ControlInvokeFast(LIST_PLANS, Sub()
                                                        With LIST_PLANS
                                                            .SelectedItems.Clear()
                                                            .Items(indx).Selected = True
                                                            .FocusedItem = .Items(indx)
                                                            .Refresh()
                                                            .EnsureVisible(indx)
                                                        End With
                                                    End Sub, EDP.None) : Catch : End Try
            End Set
        End Property
        Private _RefillInProgress As Boolean = False
        Private Sub Refill() Handles MyDefs.ButtonUpdateClick
            Try
                ControlInvokeFast(LIST_PLANS,
                Sub()
                    If Not _RefillInProgress Then
                        _RefillInProgress = True
                        With LIST_PLANS.Items
                            .Clear()
                            If Settings.Automation.Count > 0 Then
                                .AddRange(Settings.Automation.Select(Function(a) a.LVI).ToArray)
                                If _LatestSelected.ValueBetween(0, .Count - 1) Then ListSelectedIndex = _LatestSelected
                            Else
                                _LatestSelected = -1
                            End If
                        End With
                        _RefillInProgress = False
                    End If
                End Sub)
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[DownloadObjects.SchedulerEditorForm.Refill]")
            End Try
        End Sub
        Private Sub SetTitle()
            Try
                If GetSchedulerFiles.ListExists(2) Then
                    Text = $"{TitleDefault} [{Settings.Automation.Name}]"
                Else
                    Text = TitleDefault
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[SchedulerEditorForm.SetTitle]")
            End Try
        End Sub
#Region "Add, Edit, Delete"
        Private Sub MyDefs_ButtonAddClick(ByVal Sender As Object, ByVal e As EventArgs) Handles MyDefs.ButtonAddClick, BTT_CLONE.Click
            Dim a As AutoDownloader = Nothing
            If Sender Is BTT_CLONE Then
                If _LatestSelected.ValueBetween(0, Settings.Automation.Count - 1) Then a = Settings.Automation(_LatestSelected).Copy
            Else
                a = New AutoDownloader(True)
            End If
            If Not a Is Nothing Then
                Using f As New AutoDownloaderEditorForm(a)
                    f.ShowDialog()
                    If f.DialogResult = DialogResult.OK Then
                        Settings.Automation.Add(a)
                        Refill()
                    Else
                        a.Dispose()
                    End If
                End Using
            End If
        End Sub
        Private Sub Edit() Handles MyDefs.ButtonEditClick
            If _LatestSelected.ValueBetween(0, LIST_PLANS.Items.Count - 1) Then
                Using f As New AutoDownloaderEditorForm(Settings.Automation(_LatestSelected)) : f.ShowDialog() : End Using
                LIST_PLANS_Refresh(_LatestSelected)
            Else
                MsgBoxE("You have not selected a plan to edit.", vbExclamation)
            End If
        End Sub
        Private _DeleteInProgress As Boolean = False
        Private Async Sub MyDefs_ButtonDeleteClickE(ByVal Sender As Object, ByVal e As EditToolbarEventArgs) Handles MyDefs.ButtonDeleteClickE
            Const MsgTitle$ = "Deleting a plan..."
            If Not _DeleteInProgress Then
                If _LatestSelected.ValueBetween(0, LIST_PLANS.Items.Count - 1) Then
                    _DeleteInProgress = True
                    Dim n$ = Settings.Automation(_LatestSelected).Name
                    If MsgBoxE({$"Are you sure you want to delete the [{n}] plan?", MsgTitle}, vbExclamation + vbYesNo) = vbYes Then
                        Await Settings.Automation.RemoveAt(_LatestSelected)
                        Refill()
                        MsgBoxE({$"Plan [{n}] deleted", MsgTitle})
                    End If
                    _DeleteInProgress = False
                Else
                    MsgBoxE({"You have not selected a plan to delete.", MsgTitle}, vbExclamation)
                End If
            Else
                MsgBoxE({"One of the plans is currently in progress. Wait until this plan is stopped and deleted.", MsgTitle}, vbExclamation)
            End If
        End Sub
#End Region
#Region "List handlers"
        Private _LatestSelected As Integer = -1
        Private _LatestSelectedChangeEnabled As Boolean = True
        Private Sub LIST_PLANS_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LIST_PLANS.SelectedIndexChanged
            If _LatestSelectedChangeEnabled Then
                _LatestSelected = ListSelectedIndex
                PauseArr.PlanIndex = _LatestSelected
                PauseArr.UpdatePauseButtons(False)
            End If
        End Sub
        Private Sub LIST_PLANS_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles LIST_PLANS.MouseDoubleClick
            Edit()
        End Sub
        Private Sub LIST_PLANS_Refresh(Optional ByVal PlanIndex As Integer = -1)
            _LatestSelectedChangeEnabled = False
            Try
                ControlInvokeFast(LIST_PLANS,
                  Sub()
                      Dim indx% = If(PlanIndex >= 0, PlanIndex, ListSelectedIndex)
                      If indx = -1 And PlanIndex = -1 Then indx = _LatestSelected
                      If indx.ValueBetween(0, Settings.Automation.Count - 1) Then _
                         LIST_PLANS.Items(indx) = Settings.Automation(indx).LVI
                      If _LatestSelected.ValueBetween(0, LIST_PLANS.Items.Count - 1) Then ListSelectedIndex = _LatestSelected
                  End Sub, EDP.None)
            Catch
            Finally
                _LatestSelectedChangeEnabled = True
            End Try
        End Sub
        Private Sub Scheduler_PauseChanged(ByVal Plan As AutoDownloader)
            LIST_PLANS_Refresh(Plan.Index)
        End Sub
        Private SchedulerCheckerThread As Thread
        Private Sub SchedulerChecker()
            Try
                While Not _CloseRequested
                    Try
                        With Settings.Automation
                            If .Count > 0 Then
                                For i% = 0 To .Count - 1
                                    If .Item(i).LVIStateChanged Then LIST_PLANS_Refresh(i)
                                    If _CloseRequested Then Exit Sub
                                    Thread.Sleep(200)
                                Next
                            Else
                                If _CloseRequested Then Exit Sub
                                Thread.Sleep(200)
                            End If
                        End With
                    Catch
                    End Try
                End While
            Catch
            End Try
        End Sub
#End Region
#Region "Settings, Start, Skip, Pause"
        Private Function GetSchedulerFiles() As List(Of SFile)
            Return SFile.GetFiles(SettingsFolderName.CSFileP, $"{Scheduler.FileNameDefault}*.xml",, EDP.ReturnValue)
        End Function
        Private Class SchedulerList : Inherits SimpleListForm(Of String)
            Friend Sub New(ByVal Source As IEnumerable(Of String), Optional ByRef DesignXML As EContainer = Nothing)
                MyBase.New(Source, DesignXML)
            End Sub
            Protected Overrides Sub MyForm_Load(sender As Object, e As EventArgs)
                MyBase.MyForm_Load(sender, e)
                CMB_DATA.Button(ADB.Add).ToolTipText = "Create a new scheduler"
                CMB_DATA.Button(ADB.SaveAs).ToolTipText = "Clone an existing scheduler and save it as a new one"
                CMB_DATA.Button(ADB.Delete).ToolTipText = "Delete the selected scheduler"
                CMB_DATA.Buttons.UpdateButtonsPositions()
            End Sub
            Protected Overrides Sub CMB_DATA_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs)
                If e.DefaultButton = ADB.SaveAs Then
                    AddNewItem(e, e.Key, e.KeyEventArgs)
                Else
                    MyBase.CMB_DATA_ActionOnButtonClick(Sender, e)
                End If
            End Sub
        End Class
        Private Sub BTT_SETTINGS_Click(sender As Object, e As EventArgs) Handles BTT_SETTINGS.Click
            Const msgTitle$ = "Change scheduler"
            Try
                Const defName$ = "Default"
                Dim l As New Dictionary(Of SFile, String)
                With GetSchedulerFiles()
                    If .ListExists Then .ForEach(Sub(ff) l.Add(ff, ff.Name.Replace(Scheduler.FileNameDefault, String.Empty).StringTrimStart("_").IfNullOrEmpty(defName)))
                End With
                If l.Count > 0 Then
                    Using chooser As New SchedulerList(l.Values.Cast(Of String), Settings.Design) With {
                        .DesignXMLNodeName = "SchedulerChooserForm",
                        .Icon = ImageRenderer.GetIcon(My.Resources.ScriptPic_32, EDP.ReturnValue),
                        .FormText = "Schedulers",
                        .Mode = SimpleListFormModes.SelectedItems,
                        .MultiSelect = False
                    }
                        With chooser
                            Dim i%
                            Dim f As SFile
                            Dim selectedName$
                            Dim addedObj$ = String.Empty
                            Dim addedObjIsClone As Boolean = False
                            Dim createSchedulerPath As Func(Of String, SFile) = Function(n) $"{SettingsFolderName}\{Scheduler.FileNameDefault}_{n.StringRemoveWinForbiddenSymbols}.xml"
                            .ClearButtons()
                            .Buttons = {ADB.Add, ADB.SaveAs, ADB.Delete}
                            AddHandler .AddClick, Sub(ByVal obj As Object, ByVal args As SimpleListFormEventArgs)
                                                      If addedObj.IsEmptyString Then
                                                          addedObj = InputBoxE("Enter a new scheduler name:", msgTitle)
                                                          args.Result = Not addedObj.IsEmptyString
                                                          If args.Result Then
                                                              If l.Values.Count > 0 AndAlso l.Values.ListIndexOf(Function(n) n.StringToLower = addedObj.StringToLower) >= 0 Then
                                                                  args.Result = False
                                                                  MsgBoxE({$"A scheduler named '{addedObj}' already exists", msgTitle}, vbCritical)
                                                              Else
                                                                  args.Item = addedObj
                                                                  addedObjIsClone = Not args.ButtonEventArgs Is Nothing AndAlso
                                                                                    TypeOf args.ButtonEventArgs Is ActionButtonEventArgs AndAlso
                                                                                    DirectCast(args.ButtonEventArgs, ActionButtonEventArgs).DefaultButton = ADB.SaveAs
                                                                  If addedObjIsClone Then
                                                                      Dim cloneF As SFile = createSchedulerPath.Invoke(addedObj)
                                                                      If Not cloneF.Exists And Settings.Automation.File.Exists Then
                                                                          Using x As New XmlFile(Settings.Automation.File, Protector.Modes.All, False) With {.AllowSameNames = True, .XmlReadOnly = True}
                                                                              x.LoadData()
                                                                              x.Save(cloneF, EDP.SendToLog)
                                                                          End Using
                                                                      End If
                                                                  End If
                                                              End If
                                                          End If
                                                      Else
                                                          MsgBoxE({"You can only create one scheduler at a time", "Create a new scheduler"}, vbCritical)
                                                      End If
                                                  End Sub
                            AddHandler .DeleteClick, Sub(ByVal obj As Object, ByVal args As SimpleListFormEventArgs)
                                                         Dim n$ = AConvert(Of String)(args.Item, String.Empty)
                                                         If Not n.IsEmptyString Then
                                                             If MsgBoxE({$"Are you sure you want to delete the '{n}' scheduler?", msgTitle}, vbExclamation,,,
                                                                        {"Process", "Cancel"}) = 0 Then
                                                                 Dim delF As SFile = createSchedulerPath.Invoke(n)
                                                                 If delF.Exists AndAlso delF.Delete Then
                                                                     args.Result = True
                                                                     If l.ContainsKey(delF) Then
                                                                         l.Remove(delF)
                                                                     Else
                                                                         Dim delIndx% = l.ListIndexOf(Function(dd) dd.Value = n)
                                                                         If delIndx >= 0 Then l.Remove(l.Keys(delIndx))
                                                                     End If
                                                                 End If
                                                             End If
                                                         End If
                                                     End Sub
                            If Settings.Automation.File.Name = Scheduler.FileNameDefault Then
                                .DataSelectedIndexes.Add(0)
                            Else
                                i = l.Keys.ListIndexOf(Function(ff) ff = Settings.Automation.File)
                                If i >= 0 Then .DataSelectedIndexes.Add(i)
                            End If
                            If .ShowDialog() = DialogResult.OK Then
                                selectedName = .DataResult.FirstOrDefault
                                If Not selectedName.IsEmptyString Then
                                    If selectedName = defName Then
                                        f = Settings.Automation.FileDefault
                                    Else
                                        f = createSchedulerPath.Invoke(selectedName)
                                    End If
                                    If Not Settings.Automation.File = f AndAlso Settings.Automation.Reset(f, False) Then
                                        Settings.Automation.File = f
                                        If selectedName = defName Then
                                            Settings.AutomationFile.Value = String.Empty
                                        Else
                                            Settings.AutomationFile.Value = f.File
                                        End If
                                        PauseArr.UpdatePauseButtons()
                                        Refill()
                                        If Not .DataSource.Count = l.Count Then
                                            For i = l.Count - 1 To 0 Step -1
                                                If Not .DataSource.Contains(l(l.Keys(i))) Then l.Keys(i).Delete(, SFODelete.DeleteToRecycleBin, EDP.SendToLog)
                                            Next
                                        End If
                                    End If
                                    SetTitle()
                                End If
                            End If
                        End With
                    End Using
                    l.Clear()
                Else
                    MsgBoxE({"There are no plans created", msgTitle}, vbExclamation)
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, msgTitle)
            End Try
        End Sub
        Private Sub BTT_START_Click(sender As Object, e As EventArgs) Handles BTT_START.Click
            If _LatestSelected.ValueBetween(0, LIST_PLANS.Items.Count - 1) Then
                With Settings.Automation(_LatestSelected) : .Start(.IsNewPlan) : End With
                LIST_PLANS_Refresh(_LatestSelected)
            End If
        End Sub
        Private Sub BTT_START_FORCE_Click(sender As Object, e As EventArgs) Handles BTT_START_FORCE.Click
            If _LatestSelected.ValueBetween(0, LIST_PLANS.Items.Count - 1) Then
                With Settings.Automation(_LatestSelected)
                    If .Working Or .IsManual Then .ForceStart() : LIST_PLANS_Refresh(_LatestSelected)
                End With
            End If
        End Sub
        Private Sub BTT_SKIP_Click(ByVal Sender As ToolStripMenuItem, ByVal e As EventArgs) Handles BTT_SKIP.Click, BTT_SKIP_MIN.Click, BTT_SKIP_DATE.Click, BTT_SKIP_RESET.Click
            If _LatestSelected.ValueBetween(0, LIST_PLANS.Items.Count - 1) Then
                Dim mode$ = AConvert(Of String)(Sender.Tag, String.Empty)
                Select Case mode
                    Case String.Empty
                        Settings.Automation(_LatestSelected).Skip()
                        LIST_PLANS_Refresh(_LatestSelected)
                    Case "m"
                        Dim mins% = AConvert(Of Integer)(InputBoxE("Enter a number of minutes you want to delay:", Sender.Text, 60), -1)
                        If mins > 0 Then Settings.Automation(_LatestSelected).Skip(mins) : LIST_PLANS_Refresh(_LatestSelected)
                    Case "d"
                        Dim d As Date? = Nothing
                        Using f As New DateTimeSelectionForm(DateTimeSelectionForm.Modes.Date +
                                                             DateTimeSelectionForm.Modes.Time +
                                                             DateTimeSelectionForm.Modes.Start, Settings.Design) With {
                            .MyDateStart = Now.AddMinutes(60)
                        }
                            f.ShowDialog()
                            If f.DialogResult = DialogResult.OK Then d = f.MyDateStart
                        End Using
                        If d.HasValue Then Settings.Automation(_LatestSelected).Skip(d.Value) : LIST_PLANS_Refresh(_LatestSelected)
                    Case "r"
                        Settings.Automation(_LatestSelected).SkipReset()
                        LIST_PLANS_Refresh(_LatestSelected)
                End Select
            End If
        End Sub
#End Region
#Region "Move"
        Private Sub BTT_MOVE_UP_DOWN_Click(sender As Object, e As EventArgs) Handles BTT_MOVE_UP.Click, BTT_MOVE_DOWN.Click
            If _LatestSelected.ValueBetween(0, LIST_PLANS.Items.Count - 1) Then
                Dim v% = Settings.Automation.Move(_LatestSelected, sender Is BTT_MOVE_UP)
                If v >= 0 Then _LatestSelected = v
                Refill()
            End If
        End Sub
#End Region
#Region "ShowPlanUsers"
        Private Sub ShowPlanUsers()
            Try
                If _LatestSelected.ValueBetween(0, Settings.Automation.Count - 1) Then
                    With Settings.Automation(_LatestSelected)
                        Dim users As New List(Of API.Base.IUserData)
                        If Not .Mode = AutoDownloader.Modes.None Then
                            If .Mode = AutoDownloader.Modes.Groups Then
                                If .Groups.Count > 0 Then
                                    Dim i%
                                    For Each groupName$ In .Groups
                                        i = Settings.Groups.IndexOf(groupName)
                                        If i >= 0 Then users.ListAddList(Groups.DownloadGroup.GetUsers(Settings.Groups(i)), LAP.NotContainsOnly, LAP.IgnoreICopier)
                                    Next
                                End If
                            Else
                                users.ListAddList(Groups.DownloadGroup.GetUsers(.Self))
                            End If
                        End If
                        Groups.GroupUsersViewer.Show(users, $"S { .Name}")
                        users.Clear()
                    End With
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.LogMessageValue, ex, "Show plan users")
            End Try
        End Sub
#End Region
    End Class
End Namespace