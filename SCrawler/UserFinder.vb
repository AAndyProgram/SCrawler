' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.Messaging
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Friend Class UserFinder : Implements IDisposable
    Private Structure SkippedUser
        Friend User As UserInfo
        Friend Reason As String
    End Structure
    Private ReadOnly Paths As List(Of SFile)
    Private ReadOnly FoundUsers As List(Of UserInfo)
    Private ReadOnly Added As List(Of UserInfo)
    Private ReadOnly Skipped As List(Of SkippedUser)
    Private ReadOnly Duplicates As List(Of UserInfo)
    Private ReadOnly IgnoredCollections As List(Of String)
    Private ReadOnly NotRecognized As List(Of SFile)
    Private OriginalLocations As Boolean = False
    Private PathStr As String
    Private Const LabelImported As String = "Imported"
    Private ReadOnly Labels As List(Of String)
    Friend ReadOnly Property Count As Integer
        Get
            Return FoundUsers.Count
        End Get
    End Property
    Friend Sub New(ByVal Path As SFile)
        Paths = New List(Of SFile) From {Path}
        PathStr = vbCr & Path.ToString
        FoundUsers = New List(Of UserInfo)
        Added = New List(Of UserInfo)
        Skipped = New List(Of SkippedUser)
        Duplicates = New List(Of UserInfo)
        IgnoredCollections = New List(Of String)
        NotRecognized = New List(Of SFile)
        Labels = New List(Of String)
    End Sub
    Private Function GetFiles() As List(Of SFile)
        Dim files As New List(Of SFile)
        If Paths.Count > 0 Then
            For Each path As SFile In Paths
                files.ListAddList(SFile.GetFiles(path, "User_*.xml", IO.SearchOption.AllDirectories, EDP.ReturnValue), LAP.NotContainsOnly)
            Next
        End If
        Return files
    End Function
    Friend Function Find(ByVal OriginalLocations As Boolean) As Boolean
        Try
            Me.OriginalLocations = OriginalLocations
            If OriginalLocations Then
                Paths.Clear()
                PathStr = String.Empty
                Paths.ListAddList(Settings.Plugins.SelectMany(Function(p) p.Settings.Select(Function(pp) pp.Path)), LAP.NotContainsOnly)
                Paths.ListAddValue(Settings.CollectionsPathF, LAP.NotContainsOnly)
                PathStr = vbCr & Paths.ListToString(vbCr)
            End If
            FoundUsers.Clear()
            If Paths.Count > 0 Then
                Dim files As List(Of SFile) = GetFiles()
                If files.ListExists Then files.RemoveAll(Function(ff) ff.Name.EndsWith("_Data"))
                If files.ListExists Then
                    Dim x As XmlFile
                    Dim xErr As New ErrorsDescriber(EDP.None)
                    Dim u As UserInfo
                    For Each f As SFile In files
                        x = New XmlFile(f, Protector.Modes.All, False) With {.XmlReadOnly = True}
                        x.LoadData(xErr)
                        If Not x.HasError And x.Count > 0 Then
                            u = New UserInfo With {
                                .Name = x.Value(UserDataBase.Name_UserName),
                                .Site = x.Value(UserInfo.Name_Site),
                                .Plugin = x.Value(UserInfo.Name_Plugin),
                                .AccountName = x.Value(UserInfo.Name_AccountName),
                                .File = f,
                                .SpecialPath = x.Value(UserInfo.Name_SpecialPath),
                                .SpecialCollectionPath = x.Value(UserInfo.Name_SpecialCollectionPath),
                                .UserModel = x.Value(UserInfo.Name_Model_User).FromXML(Of Integer)(UsageModel.Default),
                                .CollectionModel = x.Value(UserInfo.Name_Model_Collection).FromXML(Of Integer)(UsageModel.Default),
                                .CollectionName = x.Value(UserInfo.Name_Collection),
                                .IsSubscription = x.Value(UserInfo.Name_IsSubscription).FromXML(Of Boolean)(False)
                            }
                            u.Merged = x.Value(UserInfo.Name_Merged).FromXML(Of Boolean)(False)
                            FoundUsers.Add(u)
                        Else
                            If x.HasError Then NotRecognized.Add(f)
                        End If
                        x.Dispose()
                    Next
                End If
            End If
            Return Count > 0
        Catch ex As Exception
            Return ErrorsDescriber.Execute(EDP.LogMessageValue, ex, $"[UserFinder.Find:{PathStr}]", False)
        End Try
    End Function
    Friend Sub Verify()
        Try
            Added.Clear()
            Skipped.Clear()
            Duplicates.Clear()
            IgnoredCollections.Clear()
            If Count > 0 Then
                Dim u As UserInfo
                Dim s As Plugin.Hosts.SettingsHost
                Dim pIndx%
                For i% = 0 To Count - 1
                    u = FoundUsers(i)
                    s = Nothing
                    If u.Plugin.IsEmptyString Then
                        pIndx = Settings.Plugins.FindIndex(Function(pp) pp.Name.ToLower = u.Site.ToLower)
                        If pIndx >= 0 Then s = Settings.Plugins(pIndx).Settings.Default
                    Else
                        s = Settings(u.Plugin).Default
                    End If
                    If Not s Is Nothing Then
                        u.Plugin = s.Key
                        If Not OriginalLocations Then
                            If u.IncludedInCollection And u.UserModel = UsageModel.Default Then
                                u.SpecialCollectionPath = u.File.CutPath(IIf(u.Merged, 1, 2)).Path.CSFileP
                            Else
                                u.SpecialPath = u.File.CutPath(1).Path.CSFileP
                            End If
                        End If
                        u.UpdateUserFile()
                        If Settings.UsersList.Contains(u) Then
                            Duplicates.Add(u)
                        ElseIf u.File.Exists And (u.CollectionName.IsEmptyString OrElse
                                                  IgnoredCollections.Contains(u.CollectionName.ToLower) OrElse
                                                  Not Settings.UsersList.Exists(Function(uu) uu.CollectionName.StringToLower = u.CollectionName.ToLower)) Then
                            Added.Add(u)
                            If Not IgnoredCollections.Contains(u.CollectionName) Then IgnoredCollections.Add(u.CollectionName)
                        Else
                            Skipped.Add(New SkippedUser With {.User = u, .Reason = "file path generation / collection exists"})
                        End If
                    Else
                        Skipped.Add(New SkippedUser With {.User = u, .Reason = "user plugin not recognized"})
                    End If
                Next
            End If
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, $"[UserFinder.Verify:{PathStr}]")
        End Try
    End Sub
    Friend Function Dialog() As Boolean
        Const MsgTitle$ = "Import users"
        Const DesignNode$ = "ImportUserSelector"
        Try
            Dim uStr As Func(Of UserInfo, String) = Function(u) $"{IIf(u.CollectionName.IsEmptyString, String.Empty, $"[{u.CollectionName}]: ")}{u.Site} - {u.Name}"
            Dim uc As Comparison(Of UserInfo) = Function(ByVal x As UserInfo, ByVal y As UserInfo) As Integer
                                                    If Not x.CollectionName.IsEmptyString And Not y.CollectionName.IsEmptyString Then
                                                        Return x.CollectionName.CompareTo(y.CollectionName)
                                                    ElseIf Not x.CollectionName.IsEmptyString Then
                                                        Return -1
                                                    ElseIf Not y.CollectionName.IsEmptyString Then
                                                        Return 1
                                                    Else
                                                        Return uStr(x).CompareTo(uStr(y))
                                                    End If
                                                End Function
            Dim __added$ = String.Empty
            Dim __dup$ = String.Empty
            Dim __skipped$ = String.Empty
            Dim __labelText$
            If Added.Count > 0 Then Added.Sort(uc) : __added = $"The following users will be added to SCrawler:{vbCr}{Added.Select(uStr).ListToString(vbCr)}"
            If Duplicates.Count > 0 Then Duplicates.Sort(uc) : __dup = $"The following users already exist In SCrawler and will not be added:{vbCr}{Duplicates.Select(uStr).ListToString(vbCr)}"
            If Skipped.Count > 0 Then
                Skipped.Sort(Function(x, y) uc(x.User, y.User))
                __skipped = $"The following users will not be added to SCrawler{vbCr}{Skipped.Select(Function(u) $"{uStr(u.User)} ({u.Reason})").ListToString(vbCr)}"
            End If
            __added = {__added, __dup, __skipped}.ListToString(vbCr.StringDup(2))
            If Not __added.IsEmptyString Then
                Using t As New TextSaver("LOGs\ImportUsers.txt") With {.FileForceAddDateTimeToName = True}
                    t.Append(__added)
                    If Added.Count > 0 Then
                        t.AppendLine(vbNewLine.StringDup(2))
                        t.AppendLine($"Added:{vbNewLine}{Added.Select(Function(u) u.File.ToString).ListToString(vbNewLine)}")
                    End If
                    If Duplicates.Count > 0 Then
                        t.AppendLine(vbNewLine.StringDup(2))
                        t.AppendLine($"Duplicates:{vbNewLine}{Duplicates.Select(Function(u) u.File.ToString).ListToString(vbNewLine)}")
                    End If
                    If Skipped.Count > 0 Then
                        t.AppendLine(vbNewLine.StringDup(2))
                        t.AppendLine($"Duplicates:{vbNewLine}{Skipped.Select(Function(u) u.User.File.ToString).ListToString(vbNewLine)}")
                    End If
                    If NotRecognized.Count > 0 Then
                        t.AppendLine(vbNewLine.StringDup(2))
                        t.AppendLine($"Not recognized:{vbNewLine}{NotRecognized.ListToString(vbNewLine)}")
                    End If
                    t.Save()
                End Using
                Dim msg As New MMessage(__added, MsgTitle,, vbQuestion) With {.Editable = True}
                Dim BttSelect As New MsgBoxButton("Select", "Select users to import") With {
                    .IsDialogResultButton = False,
                    .CallBack = Sub(r, m, b)
                                    If Not Settings.Design.Contains(DesignNode) Then Settings.Design.Add(DesignNode, String.Empty)
                                    Using f As New SimpleListForm(Of UserInfo)(Added, Settings.Design(DesignNode)) With {
                                        .Icon = My.Resources.UsersIcon_32,
                                        .FormText = MsgTitle,
                                        .Mode = SimpleListFormModes.CheckedItemsAutoCheckAll,
                                        .ButtonInsertKey = Nothing,
                                        .Provider = New CustomProvider(Function(v, d, p, n, e) uStr(v))
                                    }
                                        If f.ShowDialog() = DialogResult.OK Then
                                            Added.Clear()
                                            Added.ListAddList(f.DataResult)
                                        End If
                                    End Using
                                End Sub}
                msg.Buttons = If(Added.Count > 0, {New MsgBoxButton("Process"), BttSelect, New MsgBoxButton("Cancel")}, Nothing)
                If MsgBoxE(msg) = 0 Then
                    If Added.Count > 0 Then
                        Add()
                        If Labels.Count = 0 Then
                            __labelText = String.Empty
                        ElseIf Labels.Count = 1 Then
                            __labelText = $"{vbCr}{vbCr}The '{Labels(0)}' label has been added to each user."
                        Else
                            __labelText = $"{vbCr}{vbCr}The following labels have been added to each user: {Labels.ListToString}."
                        End If
                        MsgBoxE(New MMessage($"Restart SCrawler to take effect.{__labelText}{vbCr}{vbCr}" &
                                             $"The following users have been added to SCrawler:{vbCr}" &
                                             Added.Select(uStr).ListToString(vbCr), MsgTitle) With {.Editable = True})
                        Return True
                    End If
                Else
                    If Added.Count > 0 Then MsgBoxE({"Operation canceled", MsgTitle})
                End If
            Else
                MsgBoxE({"No users found", MsgTitle})
            End If
            Return False
        Catch ex As Exception
            Return ErrorsDescriber.Execute(EDP.LogMessageValue, ex, $"[UserFinder.Dialog:{PathStr}]", False)
        End Try
    End Function
    Friend Sub Add()
        Try
            Labels.Clear()
            Select Case MsgBoxE({"Do you want to add an 'Imported' label to each user?", "User labels"}, vbQuestion,,,
                                {"Yes", New MsgBoxButton("Select", "Select labels to add"), "No"}).Index
                Case 0 : Labels.Add(LabelImported)
                Case 1
                    Labels.ListAddList(GetLabels())
                    If Labels.Count = 0 AndAlso MsgBoxE({"You have not selected any labels." &
                                                         "Do you want to add an 'Imported' label to each user?", "User labels"},
                                                        vbExclamation + vbYesNo) = vbYes Then Labels.Add(LabelImported)
            End Select
            If Labels.Count > 0 Then
                Dim x As XmlFile
                Dim l As List(Of String)
                Dim lp As New ListAddParams(LAP.NotContainsOnly)
                For Each u As UserInfo In Added
                    x = New XmlFile(u.File, Protector.Modes.All)
                    l = x.Value(UserDataBase.Name_LabelsName).StringToList(Of String, List(Of String))("|", EDP.ReturnValue)
                    If Not l.ListExists Then l = New List(Of String)
                    l.ListAddList(Labels, lp)
                    x.Value(UserDataBase.Name_LabelsName) = l.ListToString("|")
                    x.UpdateData()
                    x.Dispose()
                Next
            End If
            Settings.UsersList.AddRange(Added)
            Settings.UpdateUsersList()
        Catch ex As Exception
            ErrorsDescriber.Execute(EDP.LogMessageValue, ex, $"[UserFinder.Add:{PathStr}]")
        End Try
    End Sub
    Private Function GetLabels() As List(Of String)
        Const DesignNode$ = "ImportUserSelectorLabels"
        Try
            Dim __add As EventHandler(Of SimpleListFormEventArgs) = Sub(sender, e) e.Item = InputBoxE("Enter a new label name", "New label").IfNullOrEmptyE(Nothing)
            Dim l As List(Of String) = ListAddList(Nothing, Settings.Labels, LAP.NotContainsOnly).ListAddValue(LabelImported, LAP.NotContainsOnly)
            If l.Count > 0 Then l.Sort()
            If Not Settings.Design.Contains(DesignNode) Then Settings.Design.Add(DesignNode, String.Empty)
            Using f As New SimpleListForm(Of String)(l, Settings.Design(DesignNode)) With {
                .Icon = My.Resources.TagIcon_32,
                .FormText = "Labels for imported users",
                .Mode = SimpleListFormModes.CheckedItems,
                .Buttons = {ADB.Add}
            }
                f.DataSelected.Add(LabelImported)
                AddHandler f.AddClick, __add
                If f.ShowDialog() = DialogResult.OK Then
                    l.Clear()
                    l.ListAddList(f.DataResult, LAP.NotContainsOnly)
                    Return l
                End If
            End Using
            Return Nothing
        Catch ex As Exception
            Return ErrorsDescriber.Execute(EDP.LogMessageValue, ex, $"[UserFinder.GetLabels:{PathStr}]")
        End Try
    End Function
#Region "IDisposable Support"
    Private disposedValue As Boolean = False
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                Paths.Clear()
                FoundUsers.Clear()
                Added.Clear()
                Skipped.Clear()
                Duplicates.Clear()
                IgnoredCollections.Clear()
                NotRecognized.Clear()
                Labels.Clear()
            End If
            disposedValue = True
        End If
    End Sub
    Protected Overrides Sub Finalize()
        Dispose(False)
        MyBase.Finalize()
    End Sub
    Friend Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class