' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Reflection
Imports SCrawler.API.Base
Imports SCrawler.API.YouTube.Objects
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Objects
Imports Download = SCrawler.Plugin.ISiteSettings.Download
Imports PauseModes = SCrawler.DownloadObjects.AutoDownloader.PauseModes
Imports MsgBoxButton = PersonalUtilities.Functions.Messaging.MsgBoxButton
Namespace Plugin.Hosts
    Friend Class SettingsHostCollection : Implements IEnumerable(Of SettingsHost), IMyEnumerator(Of SettingsHost), IDisposable
#Region "Declarations"
        Private Const FileNamePrefix As String = "Host_"
        Private Const FileNamePattern As String = FileNamePrefix & "{0}_{1}_"
        Private Const FileNamePatternFull As String = FileNamePattern & "{2}"
        Private ReadOnly Hosts As List(Of SettingsHost)
        Private ReadOnly HostsUnavailableIndexes As List(Of Integer)
        Private ReadOnly HostsXml As List(Of XmlFile)
        Private Const NoPauseMode As Integer = DownloadObjects.AutoDownloader.NoPauseMode
#Region "Controls"
        Private WithEvents BTT_SETTINGS As ToolStripMenuItem
        Private BTT_SETTINGS_SEP_1 As ToolStripSeparator
        Private WithEvents BTT_SETTINGS_ACTIONS_ADD As ToolStripMenuItem
        Friend Function GetSettingsButton() As ToolStripItem
            UpdateSettingsButton()
            Return BTT_SETTINGS
        End Function
        Private Sub UpdateSettingsButton()
            If BTT_SETTINGS Is Nothing Then
                BTT_SETTINGS = New ToolStripMenuItem With {.Text = [Default].Source.Site}
                If Not [Default].Source.Image Is Nothing Then BTT_SETTINGS.Image = [Default].Source.Image
            End If
            BTT_SETTINGS.DropDownItems.Clear()
            If Count > 1 Then
                If BTT_SETTINGS_SEP_1 Is Nothing Then BTT_SETTINGS_SEP_1 = New ToolStripSeparator
                If BTT_SETTINGS_ACTIONS_ADD Is Nothing Then BTT_SETTINGS_ACTIONS_ADD = New ToolStripMenuItem("Add new profile", My.Resources.PlusPic_24)
                Hosts.ForEach(Sub(h) BTT_SETTINGS.DropDownItems.Add(h.GetSettingsButton))
                BTT_SETTINGS.DropDownItems.AddRange({BTT_SETTINGS_SEP_1, BTT_SETTINGS_ACTIONS_ADD})
            End If
        End Sub
        Private Sub BTT_SETTINGS_Click(sender As Object, e As EventArgs) Handles BTT_SETTINGS.Click
            If Count = 1 Then [Default].SettingsPerformClick()
        End Sub
#End Region
        Friend ReadOnly Property Key As String
            Get
                Return [Default].Key
            End Get
        End Property
        Friend ReadOnly Property Name As String
            Get
                Return [Default].Name
            End Get
        End Property
        Private ReadOnly Property PluginType As Type
        Private ReadOnly PluginConstructorArguments As Integer = 0
        Private ReadOnly PluginConstructor As ConstructorInfo = Nothing
        Friend ReadOnly Property [Default] As SettingsHost
            Get
                Return Hosts(0)
            End Get
        End Property
        Friend Sub UpdateInheritance()
            If [Default].InheritanceValueExists Then Hosts.ForEach(Sub(h) h.UpdateInheritance())
        End Sub
#End Region
#Region "Initializer"
        Friend Sub New(ByVal Plugin As Type, ByRef _XML As XmlFile, ByVal GlobalPath As SFile,
                       ByRef _Temp As XMLValue(Of Boolean), ByRef _Imgs As XMLValue(Of Boolean), ByRef _Vids As XMLValue(Of Boolean))
            PluginType = Plugin
            With Plugin.GetTypeInfo.DeclaredConstructors
                If .ListExists Then
                    With .Where(Function(m) If(m.GetParameters?.Count, 0).ValueBetween(0, 2))
                        If .ListExists Then
                            PluginConstructorArguments = .Max(Of Integer)(Function(m) If(m.GetParameters?.Count, 0))
                            PluginConstructor = .First(Function(m) If(m.GetParameters?.Count, 0) = PluginConstructorArguments)
                        Else
                            PluginConstructorArguments = -1
                        End If
                    End With
                Else
                    PluginConstructorArguments = -1
                End If
            End With
            HostsUnavailableIndexes = New List(Of Integer)
            Dim defInstance As ISiteSettings = CreateInstance()
            HostsXml = New List(Of XmlFile) From {
                GetNewXmlFile($"{SettingsFolderName}\{SiteSettingsBase.ResponserFilePrefix}{defInstance.Site}_Settings.xml")
            }
            Hosts = New List(Of SettingsHost) From {New SettingsHost(defInstance, True, HostsXml(0), GlobalPath, _Temp, _Imgs, _Vids)}

            Dim hostFiles As List(Of SFile) = SFile.GetFiles(SettingsFolderName.CSFileP, $"{String.Format(FileNamePattern, Key, Name)}*.xml",, EDP.ReturnValue)
            If hostFiles.ListExists Then
                For Each f As SFile In hostFiles
                    HostsXml.Add(GetNewXmlFile(f))
                    Hosts.Add(New SettingsHost(CreateInstance(HostsXml.Last.Value(SettingsHost.NameXML_AccountName)), False, HostsXml.Last,
                                               GlobalPath, _Temp, _Imgs, _Vids))
                Next
            End If
            Hosts.ListReindex
            Hosts.ForEach(Sub(h) SetHostHandlers(h))
        End Sub
        Private Function GetNewXmlFile(ByVal f As SFile) As XmlFile
            Dim x As New XmlFile(f,, False) With {.AutoUpdateFile = True}
            If Not f.Exists Then x.Name = "SiteSettings"
            x.LoadData()
            Return x
        End Function
#End Region
#Region "CreateInstance"
        Private Function CreateInstance(Optional ByVal Name As String = Nothing, Optional ByVal Abstract As Boolean = False) As ISiteSettings
            Select Case PluginConstructorArguments
                Case 2 : Return PluginConstructor.Invoke({Name, Abstract})
                Case 1 : Return PluginConstructor.Invoke({Name})
                Case 0 : Return PluginConstructor.Invoke(Nothing)
                Case Else : Return Activator.CreateInstance(PluginType)
            End Select
        End Function
#End Region
#Region "ToString"
        Public Overrides Function ToString() As String
            Return Name
        End Function
#End Region
#Region "Host handlers"
#Region "Edit"
        Private Sub Hosts_OnBeginEdit(ByVal Obj As SettingsHost)
            If Obj.Index.ValueBetween(0, HostsXml.Count - 1) Then HostsXml(Obj.Index).BeginUpdate()
        End Sub
        Private Sub Hosts_OnUpdate(ByVal Obj As SettingsHost)
        End Sub
        Private Sub Hosts_OnEndEdit(ByVal Obj As SettingsHost)
            If Obj.Index.ValueBetween(0, HostsXml.Count - 1) Then
                With HostsXml(Obj.Index)
                    .EndUpdate()
                    If .ChangesDetected Then .UpdateData(EDP.SendToLog)
                End With
            End If
        End Sub
#End Region
        Private Sub SetHostHandlers(ByVal Host As SettingsHost)
            AddHandler Host.OkClick, AddressOf Hosts_OkClick
            AddHandler Host.Deleted, AddressOf Hosts_Deleted
            AddHandler Host.CloneClick, AddressOf Hosts_CloneClick
            AddHandler Host.OnBeginEdit, AddressOf Hosts_OnBeginEdit
            AddHandler Host.OnUpdate, AddressOf Hosts_OnUpdate
            AddHandler Host.OnEndEdit, AddressOf Hosts_OnEndEdit
            If Host.Index > 0 Then Host.Source.DefaultInstance = [Default].Source : Host.DefaultInstanceChanged()
        End Sub
        Private Sub Hosts_OkClick(ByVal Obj As SettingsHost)
            If Obj.Index = -1 Then
                HostsXml.Add(GetNewXmlFile($"{SettingsFolderName}\{String.Format(FileNamePatternFull, Key, Name, Obj.AccountName)}.xml"))
                With Settings : Hosts.Add(Obj.Apply(HostsXml.Last, .GlobalPath,
                                                    .DefaultTemporary, .DefaultDownloadImages, .DefaultDownloadVideos)) : End With
                HostsXml.Last.UpdateData()
                Obj.Dispose()
                Hosts.ListReindex
                SetHostHandlers(Hosts.Last)
                UpdateSettingsButton()
            End If
        End Sub
        Private Sub Hosts_CloneClick(ByVal Obj As SettingsHost)
            Try
                Using host As SettingsHost = Obj.Clone : SetHostHandlers(host) : host.SettingsPerformClick() : End Using
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, "[SettingsHostCollection.HostClone]")
            End Try
        End Sub
        Private Const ChngUACC_MsgTitle As String = "Changing user accounts"
        Private Sub Hosts_Deleted(ByVal Obj As SettingsHost)
            Try
                If Obj.Index > 0 Then
                    Select Case Hosts_Deleted_MoveAcc(Obj)
                        Case -1 : ShowOperationCanceledMsg(ChngUACC_MsgTitle) : Exit Sub
                        Case 1
                            MsgBoxE({$"An error occurred while changing user accounts (see log for details).{vbCr}Operation canceled.", ChngUACC_MsgTitle}, vbCritical)
                            Exit Sub
                    End Select
                    With HostsXml(Obj.Index)
                        .File.Delete(SFO.File, SFODelete.DeleteToRecycleBin, EDP.None)
                        .Dispose()
                    End With
                    HostsXml.RemoveAt(Obj.Index)
                    Hosts.RemoveAt(Obj.Index)
                    Hosts.ListReindex
                    Obj.Source.Delete()
                    Obj.Dispose()
                    UpdateSettingsButton()
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(EDP.SendToLog, ex, $"Deleting an account '{Obj.AccountName}'")
            End Try
        End Sub
        ''' <summary>
        ''' -1 - cancel;
        ''' 0 - process;
        ''' 1 - error
        ''' </summary>
        Private Function Hosts_Deleted_MoveAcc(ByVal Obj As SettingsHost) As Integer
            Dim p As PauseModes = NoPauseMode
            Dim changedUsers As New List(Of String)
            Try
                With Settings
                    p = .Automation.Pause
                    If .UsersList.Count > 0 Then
                        Dim users As New List(Of UserInfo)
                        users.ListAddList(.UsersList.Where(Function(u) u.Plugin = Key And u.AccountName = Obj.AccountName))
                        If users.ListExists Then
                            .Automation.Pause = PauseModes.Unlimited
                            Dim m As New MMessage($"There are {users.Count} user profiles using this ({Obj.AccountName}) account.", ChngUACC_MsgTitle,, vbExclamation) With {.Editable = True}
                            Dim b As New List(Of MsgBoxButton)
                            If Count - 1 = 1 Then
                                m.Text.StringAppendLine("Change their account to the default account?")
                                b.Add(New MsgBoxButton("Change") With {.CallBackObject = 1})
                            Else
                                m.Text.StringAppendLine("Change their account?")
                                b.Add(New MsgBoxButton("Default", "Change their account to the default account.") With {.CallBackObject = 1})
                                b.Add(New MsgBoxButton("Select", "Select an account for these users. A selection form will open.") With {.CallBackObject = 2})
                            End If
                            m.Text &= vbCr & vbCr
                            m.Text &= users.ListToStringE(vbCr, New CustomProvider(Function(u As UserInfo) u.Name))
                            b.Add(New MsgBoxButton("Leave", "Leave the existing account as is") With {.CallBackObject = 3})
                            b.Add(New MsgBoxButton("Cancel", "Cancel operation") With {.CallBackObject = 4})
                            If b.Count = 4 Then m.ButtonsPerRow = 2
                            m.Buttons = b
                            Dim selectedAcc% = -1
                            Select Case CInt(m.Show.Button.CallBackObject)
                                Case 1 : selectedAcc = 0
                                Case 2
                                    Using f As New SimpleListForm(Of String)(Hosts.Select(Function(h) h.AccountName.
                                                                                          IfNullOrEmpty(SettingsHost.NameAccountNameDefault)),
                                                                             .Design) With {
                                        .DesignXMLNodeName = "AccountsChooserForm",
                                        .Mode = SimpleListFormModes.SelectedItems,
                                        .MultiSelect = False,
                                        .Icon = [Default].Source.Icon,
                                        .FormText = "Select profile"
                                    }
                                        f.DataSelectedIndexes.Add(0)
                                        If f.ShowDialog = DialogResult.OK Then selectedAcc = f.DataResultIndexes.FirstOrDefault
                                    End Using
                                    If selectedAcc = -1 OrElse selectedAcc = Obj.Index OrElse
                                       MsgBoxE({$"Are you sure you want to change user accounts to {Hosts(selectedAcc).AccountName.
                                                IfNullOrEmpty(SettingsHost.NameAccountNameDefault)}?", ChngUACC_MsgTitle},
                                               vbQuestion,,, {"Confirm", "Cancel"}) = 1 Then Return -1
                                Case 3 : Return 0
                                Case 4 : Return -1
                            End Select

                            If selectedAcc >= 0 Then
                                Dim tUser As UserInfo, userBefore As UserInfo
                                Dim tUserIndx%
                                Dim tUserBase As UserDataBase
                                For Each tUser In users
                                    userBefore = tUser
                                    UpdateUserAccount(tUser, Obj, Hosts(selectedAcc), True, tUserIndx)
                                    tUserBase = .GetUser(tUser)
                                    tUserBase.AccountName = String.Empty
                                    tUserBase.User = tUser
                                    tUserBase.UpdateUserInformation()
                                    Settings.Feeds.UpdateUsers(userBefore, tUser)
                                    changedUsers.Add(tUserBase.ToStringForLog)
                                Next
                                .UpdateUsersList()
                            End If
                        Else
                            p = NoPauseMode
                        End If
                    Else
                        p = NoPauseMode
                    End If
                End With
                Return 0
            Catch ex As Exception
                Dim msg$ = $"Changing user accounts when deleting a profile ({Obj.AccountName})."
                If changedUsers.Count > 0 Then
                    msg.StringAppendLine($"The following users' accounts have been changed{vbCr}{changedUsers.ListToString(vbCr)}")
                    Settings.UpdateUsersList()
                End If
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, msg, 1)
            Finally
                If p <> NoPauseMode Then Settings.Automation.Pause = p
            End Try
        End Function
        Friend Shared Function UpdateUserAccount(ByRef ChangingUser As UserInfo, ByVal HostOld As SettingsHost, ByVal HostNew As SettingsHost,
                                                 ByVal UpdateUserInTheList As Boolean, Optional ByRef UserIndex As Integer = -1,
                                                 Optional ByVal ForceCollections As Boolean = False) As Boolean
            Dim result As Boolean = False
            With Settings
                UserIndex = .UsersList.IndexOf(ChangingUser)
                If UserIndex = -1 Then
                    Throw New KeyNotFoundException("User not found in the collection")
                Else
                    Dim processUserPath As Boolean
                    Dim samePath As Boolean = HostOld.Path(False) = HostNew.Path(False)
                    With ChangingUser
                        If (Not samePath Or ForceCollections) AndAlso .SpecialPath.IsEmptyString AndAlso .SpecialCollectionPath.IsEmptyString Then
                            processUserPath = False
                            If .IncludedInCollection Then
                                If Not .IsVirtual Then
                                    .SpecialCollectionPath = .GetCollectionRootPath
                                    result = True
                                Else
                                    If Not samePath Then processUserPath = True
                                End If
                            End If
                            If Not .IncludedInCollection Or processUserPath Then .SpecialPath = .File.CutPath.PathWithSeparator : result = True
                        End If
                    End With
                    ChangingUser.AccountName = HostNew.AccountName
                    ChangingUser.UpdateUserFile()
                    If UpdateUserInTheList Then .UsersList(UserIndex) = ChangingUser
                End If
            End With
            Return result
        End Function
        Friend Shared Function UpdateHostPath_CheckDownloader() As Boolean
            If Downloader.Working Then
                MsgBoxE({"You cannot change global paths while the downloader is working!", "Changing paths"}, vbCritical)
                Return False
            Else
                Return True
            End If
        End Function
        Friend Overloads Shared Function UpdateHostPath(ByVal PathOld As SFile, ByVal PathNew As SFile,
                                                        ByVal ColNameOld As String, ByVal ColNameNew As String) As Boolean
            Dim p As PauseModes = NoPauseMode
            Try
                If Not UpdateHostPath_CheckDownloader() Then Return False
                If Not AEquals(Of String)(PathOld.PathWithSeparator, PathNew.PathWithSeparator) Or Not AEquals(Of String)(ColNameOld, ColNameNew) Then
                    p = Settings.Automation.Pause
                    Settings.Automation.Pause = PauseModes.Unlimited
                    With Settings.Plugins
                        If .Count > 0 Then
                            Dim h As SettingsHost
                            For Each plugin As PluginHost In .Self
                                If plugin.Settings.Count > 0 Then
                                    For Each h In plugin.Settings
                                        If Not UpdateHostPath(h, PathOld, PathNew, False, False, Not ColNameOld = ColNameNew) Then Return False
                                    Next
                                End If
                            Next
                        End If
                    End With
                End If
                Return True
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "[SettingsHostCollection.UpdateHostPath]", False)
            Finally
                If p <> NoPauseMode Then Settings.Automation.Pause = p
            End Try
        End Function
        Friend Overloads Shared Function UpdateHostPath(ByVal Host As SettingsHost, ByVal PathOld As SFile, ByVal PathNew As SFile,
                                                        Optional ByVal Abs As Boolean = True,
                                                        Optional ByVal PauseDownloader As Boolean = True,
                                                        Optional ByVal ForceCollections As Boolean = False) As Boolean
            Dim p As PauseModes = NoPauseMode
            Try
                If Not UpdateHostPath_CheckDownloader() Then Return False
                If Not PathNew.IsEmptyString And Settings.UsersList.Count > 0 Then
                    Dim hp As SFile = Host.Path(False, True)
                    Dim diffPaths As Boolean = (Abs And hp.PathWithSeparator = PathOld.PathWithSeparator) Or
                                               (Not Abs And hp.PathWithSeparator.StartsWith(PathOld.PathWithSeparator))
                    If Not hp.IsEmptyString AndAlso (diffPaths Or ForceCollections) Then
                        If PauseDownloader Then
                            p = Settings.Automation.Pause
                            Settings.Automation.Pause = PauseModes.Unlimited
                        End If
                        Dim checkAccName As Func(Of UserInfo, Boolean) = Function(u) _
                            (
                                (Host.AccountName.IsEmptyString Or Host.AccountName = SettingsHost.NameAccountNameDefault) And
                                (u.AccountName.IsEmptyString Or u.AccountName = SettingsHost.NameAccountNameDefault)
                            ) Or
                            (Host.AccountName = u.AccountName)
                        Dim tUser As UserInfo, tUserNew As UserInfo
                        Dim tUserBase As UserDataBase
                        Dim i%
                        Dim newHost As SettingsHost = Nothing
                        Dim userListUpdated As Boolean = False
                        For i = 0 To Settings.UsersList.Count - 1
                            tUser = Settings.UsersList(i)
                            tUserNew = tUser
                            If tUser.Plugin = Host.Key And checkAccName.Invoke(tUser) Then
                                If newHost Is Nothing Then
                                    newHost = Host.Clone
                                    newHost.AccountName = Host.AccountName
                                    If Abs Then
                                        newHost.Path = PathNew
                                    Else
                                        newHost.Path = $"{PathNew.PathWithSeparator}{Host.Source.Site}".CSFileP
                                    End If
                                End If
                                If UpdateUserAccount(tUserNew, Host, newHost, False,, ForceCollections) Then
                                    tUserBase = Settings.GetUser(tUser)
                                    If Not tUserBase Is Nothing Then tUserBase.User = tUserNew : tUserBase.UpdateUserInformation(True)
                                    Settings.UsersList(i) = tUserNew
                                    userListUpdated = True
                                End If
                            End If
                        Next
                        newHost.DisposeIfReady(False)
                        If userListUpdated Then Settings.UpdateUsersList()
                        If Abs Then
                            Host.Path = PathNew
                        Else
                            Host.Path = $"{PathNew.PathWithSeparator}{Host.Source.Site}".CSFileP
                        End If
                    End If
                End If
                Return True
            Catch ex As Exception
                Return ErrorsDescriber.Execute(EDP.SendToLog, ex, "[SettingsHostCollection.UpdateHostPath(HOST)]", False)
            Finally
                If p <> NoPauseMode Then Settings.Automation.Pause = p
            End Try
        End Function
#End Region
#Region "Count, Item"
        Friend ReadOnly Property Count As Integer Implements IMyEnumerator(Of SettingsHost).MyEnumeratorCount
            Get
                Return Hosts.Count
            End Get
        End Property
        Friend ReadOnly Property CountUnavailable As Integer
            Get
                Return HostsUnavailableIndexes.Count
            End Get
        End Property
        Default Friend Overloads ReadOnly Property Item(ByVal Index As Integer) As SettingsHost Implements IMyEnumerator(Of SettingsHost).MyEnumeratorObject
            Get
                Return Hosts(Index)
            End Get
        End Property
        Default Friend Overloads ReadOnly Property Item(ByVal Name As String, Optional ByVal ReturnDef As Boolean = False) As SettingsHost
            Get
                Dim i% = IndexOf(Name)
                If i >= 0 Then
                    Return Hosts(i)
                ElseIf Settings.UseDefaultAccountIfMissing Or ReturnDef Then
                    Return [Default]
                Else
                    Return Nothing
                End If
            End Get
        End Property
#End Region
#Region "Forks"
        Friend Function Available(ByVal What As Download, ByVal Silent As Boolean, Optional ByVal FillIndexes As Boolean = True,
                                  Optional ByVal HostNames As IEnumerable(Of String) = Nothing,
                                  Optional ByVal HostNamesPassed As Boolean = False) As Boolean
            If FillIndexes Then HostsUnavailableIndexes.Clear()
            Dim hnExists As Boolean = HostNames.ListExists
            If Count = 1 Then
                If Not Silent AndAlso HostNamesPassed AndAlso
                   (Not hnExists OrElse Not HostNames.Contains([Default].AccountName.IfNullOrEmpty(SettingsHost.NameAccountNameDefault))) Then Silent = True
                If [Default].Available(What, Silent) Then
                    Return True
                Else
                    If FillIndexes Then HostsUnavailableIndexes.Add(0)
                    Return False
                End If
            Else
                Dim a As Boolean = False, n As Boolean = False, aDown As Boolean = True
                Dim t$ = String.Empty
                Dim tExists As Boolean = False
                Dim singleHost As Boolean = hnExists AndAlso HostNames.Count = 1
                Dim m As New MMessage("", "Some of the hosts are unavailable",, vbExclamation)
                If [Default].IsDownDetectorCompatible Then
                    Dim sh As SettingsHost = Nothing
                    Dim defdvalue% = [Default].DownDetectorValue
                    If hnExists AndAlso Not Hosts.All(Function(h) h.DownDetectorValue = defdvalue) Then _
                       sh = Hosts.Find(Function(h) h.AccountName = HostNames(0))
                    If sh Is Nothing Then sh = [Default]
                    aDown = sh.AvailableDownDetector(What, Silent)
                    Hosts.ForEach(Sub(ByVal h As SettingsHost)
                                      h.AvailableDownDetectorAsked = True
                                      If Not aDown And Not Silent Then h.AvailableValue = False : h.AvailableAsked = True
                                  End Sub)
                End If
                If aDown Then
                    For i% = 0 To Count - 1
                        If Not hnExists OrElse HostNames.Contains(Hosts(i).AccountName) Then
                            If Hosts(i).Available(What, True) Then
                                a = True
                            Else
                                n = True
                                If Not Hosts(i).AvailableText.IsEmptyString Then
                                    t &= vbCr
                                    t.StringAppendLine($"{Name} - {Hosts(i).AccountName.IfNullOrEmpty(SettingsHost.NameAccountNameDefault)}:")
                                    t.StringAppendLine(Hosts(i).AvailableText)
                                    tExists = True
                                Else
                                    t.StringAppendLine($"{Name} - {Hosts(i).AccountName.IfNullOrEmpty(SettingsHost.NameAccountNameDefault)}")
                                End If
                                If FillIndexes Then HostsUnavailableIndexes.Add(i)
                            End If
                        End If
                    Next
                Else
                    If Not Silent Then Silent = True
                    a = False
                    n = True
                    If Not [Default].AvailableText.IsEmptyString Then t = [Default].AvailableText : tExists = Not t.IsEmptyString
                End If

                t = t.StringTrim
                If singleHost Then
                    m.Text = "The host is unavailable."
                Else
                    m.Text = "Some of the hosts are unavailable."
                End If
                If HostNamesPassed And Not hnExists And aDown Then Silent = True
                If Not aDown And Not Silent Then
                    Return False
                ElseIf a And Not n Then
                    Return True
                ElseIf Not a And n Then
                    If Not Silent And tExists Then m.Text &= $"{vbCr}{vbCr}{t}" : m.Show()
                    Return False
                ElseIf Not t.IsEmptyString And tExists And Not Silent Then
                    m.Style(True) = m.Style + vbYesNo
                    m.Text &= $" Do you want to continue?{vbCr}{vbCr}{t}"
                    Return m.Show = vbYes
                Else
                    Return True
                End If
            End If
        End Function
        Friend Function AvailablePartial(ByVal AccountName As String) As Boolean
            Dim i% = IndexOf(AccountName)
            Return i >= 0 AndAlso Not HostsUnavailableIndexes.Contains(i)
        End Function
        Friend Sub DownloadStarted(ByVal What As Download)
            Hosts.ForEach(Sub(h) h.DownloadStarted(What))
        End Sub
        Friend Sub DownloadDone(ByVal What As Download)
            Hosts.ForEach(Sub(h) h.DownloadDone(What))
            HostsUnavailableIndexes.Clear()
        End Sub
        Friend Sub UpdateEnvironmentPrograms(ByVal EnvironmentPrograms As IEnumerable(Of String), ByVal CMDEncoding As String)
            If Count > 0 Then Hosts.ForEach(Sub(h) h.UpdateEnvironmentPrograms(EnvironmentPrograms, CMDEncoding))
        End Sub
        Friend Function IsMyUser(ByVal UserURL As String) As ExchangeOptions
            Return [Default].IsMyUser(UserURL)
        End Function
        Friend Function IsMyImageVideo(ByVal URL As String) As ExchangeOptions
            Return [Default].IsMyImageVideo(URL)
        End Function
        Friend Function GetSingleMediaInstance(ByVal URL As String, ByVal OutputFile As SFile) As IYouTubeMediaContainer
            Return [Default].GetSingleMediaInstance(URL, OutputFile)
        End Function
#End Region
#Region "BeginUpdate, EndUpdate"
        Friend Sub BeginUpdate()
            Hosts.ForEach(Sub(h) h.Source.BeginUpdate())
            HostsXml.ForEach(Sub(x) x.BeginUpdate())
        End Sub
        Friend Sub EndUpdate()
            Hosts.ForEach(Sub(h) h.Source.EndUpdate())
            If HostsXml.Count > 0 Then HostsXml.ForEach(Sub(ByVal x As XmlFile)
                                                            x.EndUpdate()
                                                            If x.ChangesDetected Then x.UpdateData(EDP.SendToLog)
                                                        End Sub)
        End Sub
#End Region
#Region "CreateAbstract"
        Friend Sub CreateAbstract() Handles BTT_SETTINGS_ACTIONS_ADD.Click
            If Count = 1 AndAlso MsgBoxE({"Do you want to clone an existing one or create empty settings?", "New profile settings"},
                                         vbQuestion,,, {"Empty", "Clone"}) = 1 Then
                Hosts_CloneClick([Default])
            Else
                With Settings
                    Using host As New SettingsHost(CreateInstance(, True), .GlobalPath, .DefaultTemporary, .DefaultDownloadImages, .DefaultDownloadVideos)
                        SetHostHandlers(host)
                        host.SettingsPerformClick()
                    End Using
                End With
            End If
        End Sub
#End Region
#Region "IndexOf"
        Friend Function IndexOf(ByVal Name As String) As Integer
            If Name.IsEmptyString OrElse Name = SettingsHost.NameAccountNameDefault Then
                Return 0
            Else
                Return Hosts.FindIndex(Function(h) h.AccountName = Name)
            End If
        End Function
#End Region
#Region "IEnumerable Support"
        Private Function GetEnumerator() As IEnumerator(Of SettingsHost) Implements IEnumerable(Of SettingsHost).GetEnumerator
            Return New MyEnumerator(Of SettingsHost)(Me)
        End Function
        Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return GetEnumerator()
        End Function
#End Region
#Region "IDisposable Support"
        Private disposedValue As Boolean = False
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    Hosts.ListClearDispose(, False, EDP.SendToLog)
                    HostsUnavailableIndexes.Clear()
                    HostsXml.ListClearDispose(, False, EDP.SendToLog)
                    BTT_SETTINGS.DisposeIfReady
                    BTT_SETTINGS_SEP_1.DisposeIfReady
                    BTT_SETTINGS_ACTIONS_ADD.DisposeIfReady
                End If
                BTT_SETTINGS = Nothing
                BTT_SETTINGS_SEP_1 = Nothing
                BTT_SETTINGS_ACTIONS_ADD = Nothing
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