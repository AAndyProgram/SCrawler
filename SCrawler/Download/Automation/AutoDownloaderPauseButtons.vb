' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms
Imports PauseModes = SCrawler.DownloadObjects.AutoDownloader.PauseModes
Imports TimeSelectionModes = PersonalUtilities.Forms.DateTimeSelectionForm.Modes
Namespace DownloadObjects
    Friend Class AutoDownloaderPauseButtons : Implements IDisposable
#Region "Declarations"
        Friend Event Updating()
        Private WithEvents TrayButtons As AutoDownloaderPauseButtons
        Friend Enum ButtonsPlace : MainFrame : Tray : Scheduler : End Enum
#Region "Controls"
        Private WithEvents BTT_PAUSE_H1 As ToolStripMenuItem
        Private WithEvents BTT_PAUSE_H2 As ToolStripMenuItem
        Private WithEvents BTT_PAUSE_H3 As ToolStripMenuItem
        Private WithEvents BTT_PAUSE_H4 As ToolStripMenuItem
        Private WithEvents BTT_PAUSE_H6 As ToolStripMenuItem
        Private WithEvents BTT_PAUSE_H12 As ToolStripMenuItem
        Private ReadOnly SEP_1 As ToolStripSeparator
        Private ReadOnly SEP_2 As ToolStripSeparator
        Private ReadOnly SEP_3 As ToolStripSeparator
        Private WithEvents BTT_PAUSE_UNTIL As ToolStripMenuItem
        Private WithEvents BTT_PAUSE_UNLIMITED As ToolStripMenuItem
        Private WithEvents BTT_PAUSE_DISABLE As ToolStripMenuItem
#End Region
        Private Property Toolbar As ToolStrip
        Friend Property MainFrameButtonsInstance As AutoDownloaderPauseButtons
        Friend Property PlanIndex As Integer = -1
        Private ReadOnly Place As ButtonsPlace
#End Region
#Region "Initializer"
        Friend Sub New(ByVal Place As ButtonsPlace)
            Me.Place = Place
            CreateButton(BTT_PAUSE_H1, "1 hour", PauseModes.H1)
            CreateButton(BTT_PAUSE_H2, "2 hours", PauseModes.H2)
            CreateButton(BTT_PAUSE_H3, "3 hours", PauseModes.H3)
            CreateButton(BTT_PAUSE_H4, "4 hours", PauseModes.H4)
            CreateButton(BTT_PAUSE_H6, "6 hours", PauseModes.H6)
            CreateButton(BTT_PAUSE_H12, "12 hours", PauseModes.H12)
            CreateButton(BTT_PAUSE_UNTIL, "Until...", PauseModes.Until, "You will be prompted to enter the time you want to pause the task(s).")
            CreateButton(BTT_PAUSE_UNLIMITED, "Unlimited", PauseModes.Unlimited, "Pause the task(s) until you turn it off or close the program.")
            CreateButton(BTT_PAUSE_DISABLE, "Disable", PauseModes.Disabled, "Disable pause")
            SEP_1 = New ToolStripSeparator
            SEP_2 = New ToolStripSeparator
            SEP_3 = New ToolStripSeparator
            If Place = ButtonsPlace.MainFrame Then TrayButtons = New AutoDownloaderPauseButtons(ButtonsPlace.Tray)
        End Sub
        Private Sub CreateButton(ByRef BTT As ToolStripMenuItem, ByVal Text As String, ByVal Tag As PauseModes, Optional ByVal ToolTip As String = "")
            BTT = New ToolStripMenuItem With {
                .Text = Text,
                .DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                .Image = My.Resources.Pause_Blue_16,
                .AutoToolTip = Not ToolTip.IsEmptyString,
                .ToolTipText = ToolTip,
                .Tag = Tag
            }
        End Sub
#End Region
#Region "Add"
        Private ReadOnly Property ButtonsArray As ToolStripItem()
            Get
                Return {BTT_PAUSE_H1, BTT_PAUSE_H2, BTT_PAUSE_H3, BTT_PAUSE_H4, BTT_PAUSE_H6, BTT_PAUSE_H12,
                        SEP_1, BTT_PAUSE_UNTIL, SEP_2, BTT_PAUSE_UNLIMITED, SEP_3, BTT_PAUSE_DISABLE}
            End Get
        End Property
        ''' <summary>MainFrame</summary>
        Friend Overloads Sub AddButtons()
            With MainFrameObj.MF
                If Place = ButtonsPlace.MainFrame Then
                    .BTT_DOWN_AUTOMATION_PAUSE.DropDownItems.AddRange(ButtonsArray)
                    TrayButtons.AddButtons()
                ElseIf Place = ButtonsPlace.Tray Then
                    .BTT_TRAY_PAUSE_AUTOMATION.DropDownItems.AddRange(ButtonsArray)
                End If
            End With
        End Sub
        ''' <summary>Scheduler</summary>
        Friend Overloads Sub AddButtons(ByRef Destination As ToolStripDropDownItem, ByVal Toolbar As ToolStrip)
            Destination.DropDownItems.AddRange(ButtonsArray)
            Me.Toolbar = Toolbar
        End Sub
#End Region
#Region "Buttons handlers"
        Private Sub BTT_PAUSE_AUTOMATION_Click(ByVal Sender As ToolStripMenuItem, ByVal e As EventArgs) Handles BTT_PAUSE_H1.Click, BTT_PAUSE_H2.Click,
                                                                                                                BTT_PAUSE_H3.Click, BTT_PAUSE_H4.Click,
                                                                                                                BTT_PAUSE_H6.Click, BTT_PAUSE_H12.Click,
                                                                                                                BTT_PAUSE_UNTIL.Click, BTT_PAUSE_UNLIMITED.Click,
                                                                                                                BTT_PAUSE_DISABLE.Click
            Dim p As PauseModes = CInt(AConvert(Of Integer)(Sender.Tag, -10))
            If p > -10 AndAlso ((Place = ButtonsPlace.Scheduler And PlanIndex >= 0 And PlanIndex.ValueBetween(0, Settings.Automation.Count - 1)) OrElse
               Not Place = ButtonsPlace.Scheduler OrElse
               (Place = ButtonsPlace.Scheduler AndAlso PlanIndex = -1 AndAlso
                MsgBoxE({$"Do you want to turn {IIf(p = PauseModes.Disabled, "off", "on")} pause for all plans?", "Pause plan"},
                        vbExclamation + vbYesNo) = vbYes)) Then
                Dim d As Date? = Nothing
                Dim _SetPauseValue As Action = Sub()
                                                   If Place = ButtonsPlace.Scheduler And PlanIndex.ValueBetween(0, Settings.Automation.Count - 1) Then
                                                       Settings.Automation(PlanIndex).Pause(d) = p
                                                   ElseIf Not Place = ButtonsPlace.Scheduler Or Place = ButtonsPlace.Scheduler And PlanIndex = -1 Then
                                                       Settings.Automation.Pause(d) = p
                                                   End If
                                               End Sub
                If p = PauseModes.Until Then
                    Using f As New DateTimeSelectionForm(TimeSelectionModes.End + TimeSelectionModes.Date + TimeSelectionModes.Time, Settings.Design)
                        f.ShowDialog()
                        If f.DialogResult = DialogResult.OK Then d = f.MyDateEnd
                    End Using
                    If d.HasValue Then _SetPauseValue.Invoke
                Else
                    _SetPauseValue.Invoke
                End If
                UpdatePauseButtons()
            ElseIf p > -10 And Place = ButtonsPlace.Scheduler And PlanIndex = -1 Then
                MsgBoxE({"The plan to be paused is not selected", "Pause plan"}, vbExclamation)
            End If
        End Sub
#End Region
#Region "Update buttons"
        Friend Overloads Sub UpdatePauseButtons_Handler(ByVal Value As PauseModes)
            UpdatePauseButtons()
        End Sub
        Friend Overloads Sub UpdatePauseButtons() Handles TrayButtons.Updating
            UpdatePauseButtons(True)
        End Sub
        Friend Overloads Sub UpdatePauseButtons(ByVal UpdateBase As Boolean, Optional ByVal FromMainFrame As Boolean = False)
            Try
                With Settings.Automation
                    Dim p As PauseModes = PauseModes.Disabled
                    Dim VerifyAll As Boolean = Not Place = ButtonsPlace.Scheduler
                    If Place = ButtonsPlace.Scheduler Then
                        If PlanIndex.ValueBetween(0, Settings.Automation.Count - 1) Then p = Settings.Automation(PlanIndex).Pause
                    Else
                        p = .Pause
                    End If
                    Dim cntList As New List(Of ToolStripMenuItem) From {BTT_PAUSE_H1, BTT_PAUSE_H2, BTT_PAUSE_H3, BTT_PAUSE_H4,
                                                                        BTT_PAUSE_H6, BTT_PAUSE_H12, BTT_PAUSE_UNTIL,
                                                                        BTT_PAUSE_UNLIMITED, BTT_PAUSE_DISABLE}

                    If UpdateBase Then UpdateBaseButton(Not p = PauseModes.Disabled)
                    If Not VerifyAll OrElse Settings.Automation.All(Function(ByVal plan As AutoDownloader) As Boolean
                                                                        If plan.Mode = AutoDownloader.Modes.None Then
                                                                            Return True
                                                                        Else
                                                                            Return plan.Pause = p
                                                                        End If
                                                                    End Function) Then
                        Dim cnt As ToolStripMenuItem = Nothing
                        Select Case p
                            Case PauseModes.H1 : cnt = BTT_PAUSE_H1
                            Case PauseModes.H2 : cnt = BTT_PAUSE_H2
                            Case PauseModes.H3 : cnt = BTT_PAUSE_H3
                            Case PauseModes.H4 : cnt = BTT_PAUSE_H4
                            Case PauseModes.H6 : cnt = BTT_PAUSE_H6
                            Case PauseModes.H12 : cnt = BTT_PAUSE_H12
                            Case PauseModes.Until : cnt = BTT_PAUSE_UNTIL
                            Case PauseModes.Unlimited : cnt = BTT_PAUSE_UNLIMITED
                        End Select
                        If Not cnt Is Nothing Then
                            cntList.Remove(cnt)
                            ApplyButtonStyle(cnt, Sub() cnt.Checked = True)
                        End If
                    End If

                    cntList.ForEach(Sub(c) ApplyButtonStyle(c, Sub() c.Checked = False))
                    cntList.Clear()
                End With
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[MainFrame.UpdatePauseButtons]")
            Finally
                Select Case Place
                    Case ButtonsPlace.MainFrame : TrayButtons.UpdatePauseButtons(True, True)
                    Case ButtonsPlace.Scheduler : MainFrameButtonsInstance.UpdatePauseButtons() : RaiseEvent Updating()
                    Case ButtonsPlace.Tray : If Not FromMainFrame Then RaiseEvent Updating()
                End Select
            End Try
        End Sub
        Private Sub UpdateBaseButton(ByVal Checked As Boolean)
            With MainFrameObj.MF
                Select Case Place
                    Case ButtonsPlace.MainFrame : ApplyButtonStyle(.BTT_DOWN_AUTOMATION_PAUSE, Sub()
                                                                                                   .BTT_DOWN_AUTOMATION_PAUSE.Checked = Checked
                                                                                                   With .MENU_DOWN_ALL
                                                                                                       If Checked Then
                                                                                                           .BackColor = MyColor.UpdateBack
                                                                                                           .ForeColor = MyColor.UpdateFore
                                                                                                       Else
                                                                                                           .BackColor = Control.DefaultBackColor
                                                                                                           .ForeColor = Control.DefaultForeColor
                                                                                                       End If
                                                                                                   End With
                                                                                               End Sub)
                    Case ButtonsPlace.Tray : ApplyButtonStyle(MainFrameObj.MF.BTT_TRAY_PAUSE_AUTOMATION, Sub() MainFrameObj.MF.BTT_TRAY_PAUSE_AUTOMATION.Checked = Checked)
                End Select
            End With
        End Sub
        Private Sub ApplyButtonStyle(ByVal cnt As ToolStripMenuItem, ByVal a As Action)
            With MainFrameObj.MF
                Select Case Place
                    Case ButtonsPlace.MainFrame : ControlInvokeFast(.Toolbar_TOP, cnt, a)
                    Case ButtonsPlace.Scheduler : ControlInvokeFast(Toolbar, cnt, a)
                    Case ButtonsPlace.Tray : If .InvokeRequired Then .Invoke(a) Else a.Invoke
                End Select
            End With
        End Sub
#End Region
#Region "IDisposable Support"
        Private disposedValue As Boolean = False
        Protected Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    BTT_PAUSE_H1.Dispose()
                    BTT_PAUSE_H2.Dispose()
                    BTT_PAUSE_H3.Dispose()
                    BTT_PAUSE_H4.Dispose()
                    BTT_PAUSE_H6.Dispose()
                    BTT_PAUSE_H12.Dispose()
                    BTT_PAUSE_UNTIL.Dispose()
                    BTT_PAUSE_UNLIMITED.Dispose()
                    BTT_PAUSE_DISABLE.Dispose()
                    SEP_1.Dispose()
                    SEP_2.Dispose()
                    SEP_3.Dispose()
                End If
                BTT_PAUSE_H1 = Nothing
                BTT_PAUSE_H2 = Nothing
                BTT_PAUSE_H3 = Nothing
                BTT_PAUSE_H4 = Nothing
                BTT_PAUSE_H6 = Nothing
                BTT_PAUSE_H12 = Nothing
                BTT_PAUSE_UNTIL = Nothing
                BTT_PAUSE_UNLIMITED = Nothing
                BTT_PAUSE_DISABLE = Nothing
                disposedValue = True
            End If
        End Sub
        Protected Overrides Sub Finalize()
            Dispose(False)
            MyBase.Finalize()
        End Sub
        Friend Overloads Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class
End Namespace