' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.ComponentModel
Imports PersonalUtilities.Bots
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Forms.Controls.Base
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.Messaging
Imports BStyle = PersonalUtilities.Bots.IBot.Styles
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Namespace Editors
    Public Class BugReporterForm
#Region "Declarations"
        Private Const MsgTitle As String = "Bug report"
        Private ReadOnly MyView As FormView
        Private ReadOnly MyFieldsChecker As FieldsChecker
        Private MyProgramInfo As String
        Private MyProgramInfoPopulated As Boolean = False
        Private ReadOnly MyProgramText As String
        Private ReadOnly MyCurrentVersion As Version
        Private ReadOnly MyIsYouTube As Boolean
        Private ReadOnly MyEnvirData As DownloadObjects.STDownloader.IDownloaderSettings
        Private ReadOnly MyAdditText As String
        Private ReadOnly MyCache As CacheKeeper
#End Region
#Region "Initializer"
        Public Sub New(ByVal Cache As CacheKeeper, ByVal DesignXML As EContainer, ByVal ProgramText As String, ByVal CurrentVersion As Version, ByVal IsYouTube As Boolean,
                       ByVal EnvirData As DownloadObjects.STDownloader.IDownloaderSettings, Optional ByVal AdditText As String = Nothing)
            InitializeComponent()
            MyView = New FormView(Me, DesignXML)
            MyFieldsChecker = New FieldsChecker
            MyCache = Cache
            MyProgramText = ProgramText
            MyCurrentVersion = CurrentVersion
            MyIsYouTube = IsYouTube
            MyEnvirData = EnvirData
            MyAdditText = AdditText
            Icon = ImageRenderer.GetIcon(My.Resources.MailPic_16, EDP.ReturnValue)
        End Sub
#End Region
#Region "Form handlers"
        Private Async Sub BugReporterForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            MyView.Import()
            MyView.SetFormSize()
            With MyFieldsChecker
                .AddControl(Of String)(TXT_DESCR, TXT_DESCR.GroupBoxText)
                .EndLoaderOperations()
            End With
            TXT_LOG.Text = MyMainLOG
            Await Task.Run(Sub()
                               MyProgramInfo = ProgramInfo.GetProgramText(MyProgramText.IfNullOrEmpty(IIf(MyIsYouTube, "YouTube downloader", "SCrawler")),
                                                                          MyCurrentVersion, MyIsYouTube, MyEnvirData, MyAdditText)
                               MyProgramInfoPopulated = True
                           End Sub)
        End Sub
        Private Sub BugReporterForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            MyView.Dispose()
            MyFieldsChecker.Dispose()
        End Sub
#End Region
#Region "Message"
        Private Sub WaitLoadingDone()
            While Not MyProgramInfoPopulated : Threading.Thread.Sleep(100) : End While
        End Sub
        Private Function CreateMessage(ByVal ForGitHub As Boolean, Optional ByVal ForDiscord As Boolean = False) As Object
            Try
                Dim nl$ = vbNewLine.StringDup(2)
                Dim data As New List(Of BotMessage)
                Dim t$ = String.Empty
                Dim discordAppendNl As Action = Sub() data.Add(New BotMessage(vbNewLine))
                Dim appendNewLine As Action = Sub() If ForDiscord Then data.Add(New BotMessage(nl)) Else t &= nl
                Dim ghBold As Func(Of String, Object) = Function(ByVal input As String) As Object
                                                            If ForDiscord Then
                                                                Return New BotMessage(input, BStyle.Bold)
                                                            Else
                                                                Return String.Format("{1}{0}{1}", input, IIf(ForGitHub, "**", ""))
                                                            End If
                                                        End Function
                Dim appendData As Action(Of Object) = Sub(ByVal input As Object)
                                                          If ForDiscord Then
                                                              discordAppendNl.Invoke
                                                              data.Add(If(TypeOf input Is BotMessage, input, New BotMessage(input.ToString)))
                                                          Else
                                                              t.StringAppendLine(input)
                                                          End If
                                                      End Sub

                appendData(ghBold("Describe the bug"))
                appendData(TXT_DESCR.Text)
                If Not TXT_URL_PROFILE.IsEmptyString Then appendData($"Profile URL: {TXT_URL_PROFILE.Text}")
                If Not TXT_URL_POST.IsEmptyString Then appendData($"Post URL: {TXT_URL_POST.Text}")
                If Not TXT_REPRODUCE.IsEmptyString Then
                    appendNewLine.Invoke
                    appendData(ghBold("To Reproduce"))
                    appendData(TXT_REPRODUCE.Text)
                End If
                If Not TXT_EXPECT.IsEmptyString Then
                    appendNewLine.Invoke
                    appendData(ghBold("Expected behavior"))
                    appendData(TXT_EXPECT.Text)
                End If
                If Not TXT_LOG.IsEmptyString Then
                    appendNewLine.Invoke
                    If ForDiscord Then
                        data.Add(New BotMessage(TXT_LOG.Text, BStyle.Code))
                    ElseIf ForGitHub Then
                        appendData($"<details><summary>Log data</summary><pre>{TXT_LOG.Text}</pre></details>")
                    Else
                        appendData(ghBold("LOG"))
                        appendData(TXT_LOG.Text)
                    End If
                End If

                WaitLoadingDone()
                appendNewLine.Invoke
                appendData(ghBold("Release information:"))
                appendData(MyProgramInfo)

                Return If(ForDiscord, data, t)
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, "[BugReporterForm.CreateMessage]")
            End Try
        End Function
        Private Function ValidateFields(Optional ByVal SimpleMode As Boolean = False) As Boolean
            If MyFieldsChecker.AllParamsOK Then
                Dim opts$ = String.Empty
                If TXT_URL_PROFILE.IsEmptyString Then opts.StringAppend("profile URL")
                If TXT_URL_POST.IsEmptyString Then opts.StringAppend("post URL")
                If TXT_LOG.Text.IsEmptyString Then opts.StringAppend("LOG")
                Return opts.IsEmptyString OrElse SimpleMode OrElse
                       MsgBoxE({$"You haven't completed the following fields: {opts}.{vbCr}Are you sure you want to skip them?",
                                MsgTitle}, vbExclamation,,, {"Process", "Cancel"}) = 0
            End If
            Return False
        End Function
#End Region
#Region "Buttons"
        Private Sub BTT_ANON_Click(sender As Object, e As EventArgs) Handles BTT_ANON.Click
            Try
                If ValidateFields(True) Then
                    Dim files As List(Of SFile) = Nothing
                    If TXT_FILES.Lines.ListExists Then files.ListAddList(TXT_FILES.Lines, LAP.NotContainsOnly)
                    Dim msgs As New List(Of BotMessage)
                    Dim isSimple As Boolean = False
                    Dim aMsg$ = String.Empty
                    Select Case MsgBoxE(New MMessage("Do you want to send a simple message or report a bug?", MsgTitle,
                                                     {New MsgBoxButton("Nice", "Say something nice to the developer." & vbCr &
                                                                               "You can also attach cat picture :-)" & vbCr &
                                                                               $"The message will be sent from the '{TXT_DESCR.GroupBoxText}' field."),
                                                      New MsgBoxButton("Simple", $"The developer will only receive the message from the '{TXT_DESCR.GroupBoxText}' field."),
                                                      New MsgBoxButton("Bug report", "The developer will receive a full bug report."),
                                                      "Cancel"}, vbQuestion) With {.ButtonsPerRow = 4, .DefaultButton = 2, .CancelButton = 3}).Index
                        Case 0 : msgs.Add(TXT_DESCR.Text) : aMsg = $"{vbCr}Thank you very much. I'm very grateful for your messages. You are awesome!"
                        Case 1 : isSimple = True : msgs.Add(TXT_DESCR.Text)
                        Case 2 : msgs = CreateMessage(False, True)
                        Case Else : Exit Sub
                    End Select
                    If msgs.ListExists Then
                        Dim nErr As New ErrorsDescriber(EDP.None)
                        Using d As New DiscordBot With {.Credential = DiscordWebHook, .User = "Anonymous user"}
                            d.SendMessage(New BotMessage(msgs.ToArray), EDP.ThrowException)
                            If isSimple Then WaitLoadingDone() : d.SendMessage(MyProgramInfo, nErr)
                            If files.ListExists Then files.ForEach(Sub(ff) d.SendFile(BotMessage.FromFile(ff),, nErr))
                        End Using
                        msgs.Clear()
                        MsgBoxE({$"Your message has been sent to the developer.{aMsg}", MsgTitle})
                    End If
                End If
            Catch ex As Exception
                MsgBoxE({"Something is wrong. Your message has not been sent to the developer.", MsgTitle}, vbCritical)
            End Try
        End Sub
        Private Sub BTT_EMAIL_Click(sender As Object, e As EventArgs) Handles BTT_EMAIL.Click
            If ValidateFields() Then
                Dim msg$ = CreateMessage(False)
                Dim cmd$ = "START mailto:""andyprogram@proton.me?to=andyprogram@proton.me&subject=Application%%20bug%%20report"""
                BufferText = msg
                MsgBoxE({"The message has been copied to your clipboard. Click OK and paste this message into the window that opens.", MsgTitle})
                Using b As New BatchExecutor
                    b.FileExchanger = MyCache.NewInstance(Of BatchFileExchanger)
                    b.Execute(cmd)
                End Using
            End If
        End Sub
        Private Sub BTT_GITHUB_Click(sender As Object, e As EventArgs) Handles BTT_GITHUB.Click
            If ValidateFields() Then
                Dim msg$ = CreateMessage(True)
                BufferText = msg
                MsgBoxE({"The message has been copied to your clipboard. Create a new issue on GitHub and paste this message.", MsgTitle})
                Try : Process.Start("https://github.com/AAndyProgram/SCrawler/issues/new?assignees=&labels=&projects=&template=custom.md&title=") : Catch : End Try
            End If
        End Sub
        Private Sub BTT_COPY_Click(sender As Object, e As EventArgs) Handles BTT_COPY.Click
            If ValidateFields() Then
                Dim msg$ = CreateMessage(MsgBoxE({"Will you post this message on GitHub?", MsgTitle}, vbQuestion + vbYesNo) = vbYes)
                BufferText = msg
                MsgBoxE({"The message has been copied to your clipboard.", MsgTitle})
            End If
        End Sub
        Private Sub BTT_CANCEL_Click(sender As Object, e As EventArgs) Handles BTT_CANCEL.Click
            DialogResult = DialogResult.Cancel
            Close()
        End Sub
#End Region
#Region "Logs"
        Private Sub TXT_LOG_ActionOnButtonClick(ByVal Sender As Object, ByVal e As ActionButtonEventArgs) Handles TXT_LOG.ActionOnButtonClick
            If e.DefaultButton = ADB.Open Then
                Dim files As List(Of SFile) = SFile.SelectFiles("LOGs\",, "Select log files", "Log files|*.txt|All files|*.*", EDP.ReturnValue)
                If files.ListExists Then
                    Dim t$
                    For Each file As SFile In files
                        t = file.GetText
                        If Not t.IsEmptyString Then _
                           TXT_LOG.Text = $"{TXT_LOG.Text}{If(TXT_LOG.Text.IsEmptyString, String.Empty, vbNewLine.StringDup(2))}{file.Name}{vbNewLine}{t}"
                    Next
                End If
            End If
        End Sub
        Private Sub TXT_FILES_ActionOnButtonClick(ByVal Sender As Object, ByVal e As ActionButtonEventArgs) Handles TXT_FILES.ActionOnButtonClick
            Try
                If e.DefaultButton = ADB.Add Then
                    Dim f As List(Of SFile) = SFile.SelectFiles(,, "Select files to be sent", "Images|*.jpg;*.jpeg;*.png;*.webp;*.webm;*.gif|All files|*.*", EDP.ReturnValue)
                    If f.ListExists Then TXT_FILES.Lines = ListAddList(Nothing, TXT_FILES.Lines.Concat(f.Select(Function(ff) ff.ToString)),
                                                                       LAP.NotContainsOnly, EDP.ReturnValue).ToArray
                End If
            Catch ex As Exception
            End Try
        End Sub
#End Region
    End Class
End Namespace