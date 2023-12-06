' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Net
Imports System.IO.Compression
Imports PersonalUtilities.Functions
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports SCrawler.Shared
Public Module MainMod
    Private MyProcessID As Integer = -1
    Private MyWorkingPath As SFile = Nothing
    Private ReadOnly ProcessNames As String() = {"SCrawler", "YouTubeDownloader", "Updater"}
    Private Silent As Boolean = False
    Private Function GetConsoleResponse(ByVal Request As String) As String
        Console.Write(Request)
        Return Console.ReadLine
    End Function
    Private Function DownloadFile(ByVal URL As String, ByVal Destination As SFile) As Boolean
        Try
            Dim lastPerc% = -1
            Dim currentCursor% = Console.CursorTop
            Using w As New RWebClient With {.AsyncMode = True}
                AddHandler w.DownloadProgressChanged,
                           New DownloadProgressChangedEventHandler(Sub(ByVal Sender As Object, ByVal e As DownloadProgressChangedEventArgs)
                                                                       If lastPerc < e.ProgressPercentage Then
                                                                           lastPerc = e.ProgressPercentage
                                                                           Console.SetCursorPosition(0, currentCursor)
                                                                           Console.Write("{0}% completed", e.ProgressPercentage)
                                                                       End If
                                                                   End Sub)
                Return w.DownloadFile(URL, Destination, EDP.ReturnValue)
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Sub Main()
        Try
            MyWorkingPath = AppDomain.CurrentDomain.BaseDirectory.CSFileP
            MyProcessID = Process.GetCurrentProcess.Id
            Console.Title = "SCrawler updater"
            With Environment.GetCommandLineArgs
                If .ListExists(2) Then Silent = .Self()(1).FromXML(Of Boolean)(False)
            End With

            Dim currentDir As SFile = MyWorkingPath.CutPath
            Dim extractionDir As SFile = $"{currentDir.CSFilePS}{NewReleaseFolderName}\"
            If extractionDir.Exists(SFO.Path, False) Then extractionDir.Delete(SFO.Path, SFODelete.DeletePermanently, EDP.None)

            Dim currVer As Version = GetCurrentMaxVer(currentDir)
            If currVer Is Nothing Then
                Console.WriteLine("The current version of the program cannot be determined")
            Else
                Console.WriteLine($"The current version is {currVer} (x{IIf(Environment.Is64BitProcess, 64, 86)})")
                Dim release As GitRelease = GetGitRelease()
                If Not release.URL.IsEmptyString And Not release.Version Is Nothing Then
                    If release.Version > currVer Then
                        Console.WriteLine($"The new version is {release.Version} ({release.Name})")

                        If Not Silent AndAlso GetConsoleResponse("Do you want to update the program? (y/n): ").IfNullOrEmpty("n") = "n" Then Exit Sub

                        If ActiveProcessesExist() Then
                            Console.WriteLine("One of the SCrawler programs is still running. Waiting for all SCrawler programs to close.")
                            While ActiveProcessesExist() : Threading.Thread.Sleep(100) : End While
                            Console.WriteLine("All SCrawler programs are closed.")
                        End If

                        If extractionDir.Exists(SFO.Path, True) Then
                            Dim destFile As SFile = $"{extractionDir.CSFilePS}{New SFile(release.URL).File}"
                            Console.WriteLine("Downloading new version...")
                            If DownloadFile(release.URL, destFile) Then
                                Console.WriteLine("")
                                Console.WriteLine("New version downloaded!")
                                Console.WriteLine("Extracting files...")
                                ZipFile.ExtractToDirectory(destFile, extractionDir)
                                Console.WriteLine("Files extracted!")
                                destFile.Delete(SFO.File, SFODelete.DeletePermanently, EDP.None)
                                If Not MoveFiles(extractionDir, currentDir) Then GetConsoleResponse("Unable to update the program. Press Enter to exit") : Exit Sub
                            Else
                                extractionDir.Delete(SFO.Path, SFODelete.DeletePermanently, EDP.None)
                                Console.WriteLine("Unable to download new version")
                            End If
                        Else
                            Console.WriteLine("Unable to create temp directory")
                        End If
                    Else
                        Console.WriteLine("The program is up to date")
                    End If
                Else
                    Console.WriteLine("Unable to get information about new version")
                End If
            End If
        Catch ex As Exception
            Console.WriteLine("An error occurred during update")
            Console.WriteLine(ex.Message)
        Finally
            GetConsoleResponse("Press Enter to exit")
        End Try
    End Sub
    Private Function MoveFiles(ByVal Source As SFile, ByVal Destination As SFile) As Boolean
        Console.WriteLine("Updating files")
        Try

            Dim oldFiles As List(Of SFile) = SFile.GetFiles(Destination,,, EDP.ReturnValue)
            Dim oldFolders As List(Of SFile) = SFile.GetDirectories(Destination,,, EDP.ReturnValue)

            Dim newFiles As List(Of SFile) = SFile.GetFiles(Source,,, EDP.ReturnValue)
            Dim newFolders As List(Of SFile) = SFile.GetDirectories(Source,,, EDP.ReturnValue)

            Dim obj As SFile = Nothing
            Dim wSegment As String = MyWorkingPath.Segments.Last
            Dim filesPredicate As Predicate(Of SFile) = Function(ByVal f As SFile) As Boolean
                                                            If obj = f Or obj.Name = f.Name Then
                                                                f.Delete(SFO.File, SFODelete.DeleteToRecycleBin, EDP.None)
                                                                Return True
                                                            Else
                                                                Return False
                                                            End If
                                                        End Function
            Dim foldersPredicate As Predicate(Of SFile) = Function(ByVal f As SFile) As Boolean
                                                              Dim ls$ = f.Segments.Last
                                                              If ls = obj.Segments.Last And Not ls = NewReleaseFolderName And Not ls = wSegment Then
                                                                  f.Delete(SFO.Path, SFODelete.DeleteToRecycleBin, EDP.None)
                                                                  Return True
                                                              Else
                                                                  Return False
                                                              End If
                                                          End Function

            Dim getDestFile As Func(Of SFile, Boolean, SFile) = Function(ByVal f As SFile, ByVal isFolder As Boolean) As SFile
                                                                    Dim ff As SFile = f
                                                                    If isFolder Then
                                                                        ff = $"{Destination.PathWithSeparator}{f.Segments.Last}\"
                                                                    Else
                                                                        ff.Path = Destination.Path
                                                                    End If
                                                                    Console.WriteLine(ff)
                                                                    Return ff
                                                                End Function
            If newFiles.ListExists Then
                If oldFiles.ListExists Then
                    For Each obj In newFiles : oldFiles.RemoveAll(filesPredicate) : Next
                End If
                newFiles.ForEach(Sub(ff) SFile.Move(ff, getDestFile(ff, False), SFO.File, True, SFODelete.DeleteToRecycleBin, EDP.None))
            End If

            If newFolders.ListExists Then
                If oldFolders.ListExists Then
                    For Each obj In newFolders : oldFolders.RemoveAll(foldersPredicate) : Next
                End If
                newFolders.ForEach(Sub(ff) If Not ff.Segments.Last = wSegment Then _
                                              SFile.Move(ff, getDestFile(ff, True), SFO.Path, True, SFODelete.DeleteToRecycleBin, EDP.None))
            End If

            Console.WriteLine("Files updated")
            Return True
        Catch
            Return False
        End Try
    End Function
    Private Function ActiveProcessesExist() As Boolean
        Try
            Return Process.GetProcesses.Any(Function(p) ProcessNames.Contains(p.ProcessName) And Not p.Id = MyProcessID)
        Catch
            Return True
        End Try
    End Function
    Private Structure GitRelease
        Friend URL As String
        Friend Name As String
        Friend Version As Version
    End Structure
    Private Function GetGitRelease() As GitRelease
        Try
            Dim nameEnd$ = $"_x{IIf(Environment.Is64BitProcess, 64, 86)}.zip"
            Dim name$, relName$, relTag$

            Using resp As New Responser With {.Accept = "application/vnd.github.v3+json"}
                Dim r$ = resp.GetResponse("https://api.github.com/repos/AAndyProgram/SCrawler/releases",, EDP.ReturnValue)
                If Not r.IsEmptyString Then
                    Dim getver As Func(Of String, Version) = Function(ByVal input As String) As Version
                                                                 Try
                                                                     If Not input.IsEmptyString Then
                                                                         If input.ToLower.StartsWith("scrawler") Then
                                                                             Return New Version(input.Split("_")(1))
                                                                         Else
                                                                             Return New Version(input)
                                                                         End If
                                                                     End If
                                                                 Catch
                                                                 End Try
                                                                 Return Nothing
                                                             End Function
                    Using j As EContainer = JsonDocument.Parse(r, EDP.ReturnValue)
                        If j.ListExists Then
                            With j.FirstOrDefault(Function(e) Not e.Value("draft").FromXML(Of Boolean) And Not e.Value("prerelease").FromXML(Of Boolean))
                                If .ListExists Then
                                    relName = .Value("name")
                                    relTag = .Value("tag_name")
                                    With .Item("assets")
                                        If .ListExists Then
                                            For Each asset As EContainer In .Self
                                                name = asset.Value("name")
                                                If Not name.IsEmptyString AndAlso name.EndsWith(nameEnd) Then _
                                                   Return New GitRelease With {
                                                    .Name = name,
                                                    .URL = asset.Value("browser_download_url"),
                                                    .Version = getver(name).IfNullOrEmpty(getver(relName).IfNullOrEmpty(getver(relTag)))}
                                            Next
                                        End If
                                    End With
                                End If
                            End With
                        End If
                    End Using
                End If
            End Using
        Catch
        End Try
        Return Nothing
    End Function
End Module