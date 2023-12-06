' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Threading
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.Web
Imports PersonalUtilities.Functions.Messaging
Imports SCrawler.DownloadObjects.STDownloader
Public Module MainModShared
    Public Property BATCH As BatchExecutor
    Private _BatchLogSent As Boolean = False
    ''' <param name="e"><see cref="EDP.None"/></param>
    Public Sub GlobalOpenPath(ByVal f As SFile, Optional ByVal e As ErrorsDescriber = Nothing)
        Dim b As Boolean = False
        If Not e.Exists Then e = EDP.None
        Try
            If f.Exists(SFO.Path, False) Then
                If MyDownloaderSettings.OpenFolderInOtherProgram AndAlso Not MyDownloaderSettings.OpenFolderInOtherProgram_Command.IsEmptyString Then
                    If BATCH Is Nothing Then BATCH = New BatchExecutor With {.RedirectStandardError = True}
                    b = True
                    With BATCH
                        .Reset()
                        .Execute({String.Format(MyDownloaderSettings.OpenFolderInOtherProgram_Command, f.PathWithSeparator)}, EDP.SendToLog + EDP.ThrowException)
                        If .HasError Or Not .ErrorOutput.IsEmptyString Then Throw New Exception(.ErrorOutput, .ErrorException)
                    End With
                Else
                    f.Open(SFO.Path,, e)
                End If
            End If
        Catch ex As Exception
            If b Then
                If Not _BatchLogSent Then ErrorsDescriber.Execute(EDP.SendToLog, ex, $"GlobalOpenPath({f.Path})") : _BatchLogSent = True
                f.Open(SFO.Path,, e)
            End If
        End Try
    End Sub
    Public Sub CheckNewReleaseFolder()
        Try
            Const updaterFolderName$ = "Updater\"
            Dim f As SFile = SCrawler.Shared.NewReleaseFolderName.CSFileP
            If f.Exists(SFO.Path, False) Then
                Dim updater As SFile = updaterFolderName
                Dim updaterNR As SFile = f.PathWithSeparator & updaterFolderName
                If updaterNR.Exists(SFO.Path, False) Then
                    If updater.Exists(SFO.Path, False) Then updater.Delete(SFO.Path, SFODelete.DeletePermanently, EDP.ReturnValue)
                    SFile.Move(updaterNR, updater, SFO.Path, True, SFODelete.DeletePermanently, EDP.ReturnValue)
                End If
                f.Delete(SFO.Path, SFODelete.DeletePermanently, EDP.None)
            End If
        Catch ex As Exception
        End Try
    End Sub
    Public Sub ShowProgramInfo(ByVal ProgramText As String, ByVal CurrentVersion As Version, ByVal CheckForUpdate As Boolean, ByVal Force As Boolean,
                               ByVal EnvirData As IDownloaderSettings, ByVal IsYouTube As Boolean,
                               Optional ByRef NewVersionDestination As String = Nothing, Optional ByRef ShowNewVersionNotification As Boolean = True,
                               Optional ByVal AdditText As String = Nothing)
        Try
            Dim GoToSite As New MsgBoxButton("Go to site") With {.CallBack = Sub(r, n, b) Process.Start("https://github.com/AAndyProgram/SCrawler/releases/latest")}
            If CheckForUpdate AndAlso GitHub.NewVersionExists(CurrentVersion, "AAndyProgram", "SCrawler", NewVersionDestination) Then
                If ShowNewVersionNotification Or Force Then
                    Dim b As New List(Of MsgBoxButton)
                    Dim updaterFile As SFile = Nothing
                    Dim updateBtt As New MsgBoxButton("Update", "Update the program using the updater") With {
                        .CallBack = Sub(r, n, btt)
                                        Dim th As New Thread(Sub() Process.Start(New ProcessStartInfo(updaterFile, 1))) With {.IsBackground = True}
                                        th.SetApartmentState(ApartmentState.MTA)
                                        th.Start()
                                    End Sub}
                    With SFile.GetFiles("Updater\", "*.exe",, EDP.ReturnValue).ListIfNothing
                        If .ListExists Then
                            With .FirstOrDefault(Function(f) f.Name = "Updater")
                                If Not .IsEmptyString Then updaterFile = .Self
                            End With
                        End If
                    End With
                    b.AddRange({"OK", GoToSite})
                    If Not updaterFile.IsEmptyString Then b.Add(updateBtt)
                    b.Add(New MsgBoxButton("Disable notifications") With {.CallBackObject = 10})
                    If AConvert(Of Integer)(
                                MsgBoxE(New MMessage($"{ProgramText}: new version detected" & vbCr &
                                                     $"Current version: {CurrentVersion}" & vbCr &
                                                     $"New version: {NewVersionDestination}",
                                                     "New version", b) With {.ButtonsPerRow = 4}).Button.CallBackObject, -1) = 10 Then _
                       ShowNewVersionNotification = False
                End If
            Else
                If Force Then
                    Dim pVer$ = $"{ProgramText} v{CurrentVersion} ({IIf(Environment.Is64BitProcess, "x64", "x86")})"
                    Dim eText$ = Editors.ProgramInfo.GetProgramBaseText(ProgramText, CurrentVersion, AdditText)
                    Dim m As New MMessage($"{pVer}" & vbCr &
                                          "Address: https://github.com/AAndyProgram/SCrawler" & vbCr &
                                          "Created by Greek LGBT person Andy (Gay)",
                                          "Program information",
                                          {"OK",
                                           GoToSite,
                                           New MsgBoxButton("Environment", "Show program environment") With {
                                                            .IsDialogResultButton = False,
                                                            .CallBack = Sub(r, n, b) ShowProgramEnvir(EnvirData, IsYouTube, eText)}
                                          }) With {.DefaultButton = 0, .CancelButton = 0}
                    If Not AdditText.IsEmptyString Then m.Text &= $"{vbCr}{AdditText}"
                    m.Show()
                End If
                ShowNewVersionNotification = True
            End If
        Catch ex As Exception
        End Try
    End Sub
    Private Sub ShowProgramEnvir(ByVal EnvirData As IDownloaderSettings, ByVal IsYouTube As Boolean, ByVal AdditCopyText As String)
        Dim m As New MMessage(Editors.ProgramInfo.GetProgramEnvirText(EnvirData, IsYouTube), "Program environment", {"OK", "Copy"}) With {.Editable = True, .DefaultButton = 0, .CancelButton = 0}
        If m.Text = Editors.ProgramInfo.EnvironmentNotFound Then m.Style = vbCritical
        m.Text = $"{AdditCopyText}{vbCr}{m.Text}"
        If m.Show() = 1 Then BufferText = m.Text
    End Sub
End Module
Namespace Editors
    Public NotInheritable Class ProgramInfo
        Public Const EnvironmentNotFound As String = "Environment not found"
        Private Sub New()
        End Sub
        Public Shared Function GetProgramText(ByVal ProgramText As String, ByVal CurrentVersion As Version, ByVal IsYouTube As Boolean,
                                              ByVal EnvirData As IDownloaderSettings, Optional ByVal AdditText As String = Nothing) As String
            Return GetProgramBaseText(ProgramText, CurrentVersion, AdditText) & vbNewLine & GetProgramEnvirText(EnvirData, IsYouTube)
        End Function
        Public Shared Function GetProgramBaseText(ByVal ProgramText As String, ByVal CurrentVersion As Version, Optional ByVal AdditText As String = Nothing) As String
            Dim pVer$ = $"{ProgramText} v{CurrentVersion} ({IIf(Environment.Is64BitProcess, "x64", "x86")})"
            Dim WinVer$ = String.Empty
            Try : WinVer = $"OS: {My.Computer.Info.OSFullName} ({IIf(Environment.Is64BitOperatingSystem, "x64", "x86")})" : Catch : End Try
            Return pVer.StringDup(1).StringAppendLine(WinVer).StringAppendLine(AdditText)
        End Function
        Public Shared Function GetProgramEnvirText(ByVal EnvirData As IDownloaderSettings, ByVal IsYouTube As Boolean) As String
            Try
                Dim output$ = String.Empty
                Using b As New BatchExecutor(True)
                    Dim f As SFile
                    Dim cmd$, ff$, vText$

                    For i% = 0 To IIf(IsYouTube, 1, 3)
                        cmd = "--version"
                        Select Case i
                            Case 0 : f = EnvirData.ENVIR_FFMPEG : ff = "ffmpeg" : cmd = "-version"
                            Case 1 : f = EnvirData.ENVIR_YTDLP : ff = "yt-dlp"
                            Case 2 : f = EnvirData.ENVIR_GDL : ff = "gallery-dl"
                            Case 3 : f = EnvirData.ENVIR_CURL : ff = "cURL"
                            Case Else : f = Nothing : ff = Nothing : cmd = Nothing
                        End Select
                        If Not ff.IsEmptyString Then
                            If f.IsEmptyString Then
                                output.StringAppendLine($"[{ff}] NOT FOUND")
                            Else
                                b.Reset()
                                b.Execute($"""{f}"" {cmd}", EDP.None)
                                If b.OutputData.Count > 3 Then vText = b.OutputData(3) Else vText = "undefined"
                                output.StringAppendLine($"{ff} version: {vText}")
                            End If
                        End If
                    Next

                    If output.IsEmptyString Then output = EnvironmentNotFound
                End Using
                Return output
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog + EDP.ReturnValue, ex, "[ProgramInfo.GetProgramEnvirText]", String.Empty)
            End Try
        End Function
    End Class
End Namespace