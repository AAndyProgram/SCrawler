' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API.Base
Imports SCrawler.DownloadObjects.STDownloader
Imports PersonalUtilities.Forms
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Forms.Controls.Base
Imports ADB = PersonalUtilities.Forms.Controls.Base.ActionButton.DefaultButtons
Namespace Editors
    Friend Class GlobalLocationsChooserForm
#Region "Statics"
        Friend Shared ReadOnly Property ModelHandler(ByVal Model As PathCreationModel) As PathMoverHandler
            Get
                Dim pattern$
                Select Case Model
                    Case PathCreationModel.Path : pattern = "{0}\"
                    Case PathCreationModel.Path_UserName : pattern = "{0}\{2}\"
                    Case PathCreationModel.Path_UserSite_UserName : pattern = "{0}\{1}\{2}\"
                    Case PathCreationModel.Collection : pattern = UserInfo.CollectionUserPathPattern
                    Case Else : Return Nothing
                End Select
                Return New PathMoverHandler(Function(u, d) String.Format(pattern, d.PathNoSeparator, u.Site, u.Name).CSFileP)
            End Get
        End Property
        Friend Shared Function GetModelByLocation(ByVal Locations As SFile) As Integer
            If Settings.GlobalLocations.Count > 0 Then
                Dim i% = Settings.GlobalLocations.IndexOf(Locations, True)
                If i >= 0 Then Return Settings.GlobalLocations(i).Model
            End If
            Return -1
        End Function
#End Region
#Region "Declarations"
        Private WithEvents MyDefs As DefaultFormOptions
        Friend ReadOnly Property MyModelHandler As PathMoverHandler
            Get
                Select Case True
                    Case OPT_LOCATION_1.Checked : Return ModelHandler(If(MyIsCollectionSelector, PathCreationModel.Collection, PathCreationModel.Path))
                    Case OPT_LOCATION_2.Checked : Return ModelHandler(PathCreationModel.Path_UserName)
                    Case OPT_LOCATION_3.Checked : Return ModelHandler(PathCreationModel.Path_UserSite_UserName)
                    Case Else : Return Nothing
                End Select
            End Get
        End Property
        Friend ReadOnly Property MyModel As PathCreationModel
            Get
                Select Case True
                    Case OPT_LOCATION_1.Checked : Return If(MyIsCollectionSelector, PathCreationModel.Collection, PathCreationModel.Path)
                    Case OPT_LOCATION_2.Checked : Return PathCreationModel.Path_UserName
                    Case OPT_LOCATION_3.Checked : Return PathCreationModel.Path_UserSite_UserName
                    Case Else : Return PathCreationModel.Path_UserSite_UserName
                End Select
            End Get
        End Property
        Friend ReadOnly Property MyDestination As DownloadLocation
            Get
                Return New DownloadLocation With {.Path = CMB_LOCATIONS.Text.CSFileP, .Model = MyModel}
            End Get
        End Property
        Friend Property MyIsMultipleUsers As Boolean = False
        Friend Property MyInitialLocation As SFile
        Private _MyNonMyltipleUser As IUserData
        Private _UserSite As String = String.Empty
        Private _UserName As String = String.Empty
        Friend Property MyNonMyltipleUser As IUserData
            Get
                Return _MyNonMyltipleUser
            End Get
            Set(ByVal u As IUserData)
                _MyNonMyltipleUser = u
                If Not u Is Nothing And Not u.IsCollection Then
                    _UserSite = u.Site
                    _UserName = u.Name
                End If
            End Set
        End Property
        Friend Property MyIsCollectionSelector As Boolean = False
        Friend Property MyCollectionName As String
            Get
                Return TXT_COL_NAME.Text
            End Get
            Set(ByVal NewName As String)
                TXT_COL_NAME.Text = NewName
            End Set
        End Property
#End Region
#Region "Initializer"
        Friend Sub New()
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            Icon = ImageRenderer.GetIcon(My.Resources.FolderPic_32, EDP.ReturnValue)
            If Icon Is Nothing Then ShowIcon = False
        End Sub
#End Region
#Region "Form handlers"
        Private Sub GlobalLocationsChooserForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            With MyDefs
                .MyViewInitialize()
                .AddOkCancelToolbar()
                If Not MyIsCollectionSelector Then
                    TP_MAIN.Controls.Remove(TXT_COL_NAME)
                    TP_MAIN.RowStyles(1).Height = 0
                End If
                Settings.GlobalLocations.PopulateComboBox(CMB_LOCATIONS)
                If MyIsCollectionSelector Then
                    FRM_LOCATIONS.Enabled = False
                    OPT_LOCATION_1.Checked = True
                Else
                    OPT_LOCATION_3.Checked = True
                End If
                .MyFieldsChecker = New FieldsChecker
                With .MyFieldsCheckerE
                    .AddControl(Of String)(CMB_LOCATIONS, "Location")
                    If MyIsCollectionSelector Then .AddControl(Of String)(TXT_COL_NAME, TXT_COL_NAME.CaptionText)
                    .EndLoaderOperations()
                End With
                .EndLoaderOperations()
                UpdateOptions(False)
            End With
        End Sub
        Private Sub GlobalLocationsChooserForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
            If e.KeyCode = Keys.O And e.Control Then
                CMB_LOCATIONS.Button(ADB.Open).PerformClick()
                e.Handled = True
            End If
        End Sub
#End Region
#Region "Ok, Cancel"
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            If MyDefs.MyFieldsChecker.AllParamsOK Then
                If CMB_LOCATIONS.Checked Then _
                    Settings.GlobalLocations.Add(New DownloadLocation With {.Path = CMB_LOCATIONS.Text.CSFileP, .Model = MyModel},
                                                 Settings.STDownloader_OutputPathAskForName)
                MyDefs.CloseForm()
            End If
        End Sub
#End Region
#Region "Controls"
        Private Sub TXT_COL_NAME_ActionOnTextChanged(sender As Object, e As EventArgs) Handles TXT_COL_NAME.ActionOnTextChanged
            UpdateOptions(True, False, False)
        End Sub
        Private Sub CMB_LOCATIONS_ActionOnButtonClick(ByVal Sender As ActionButton, ByVal e As ActionButtonEventArgs) Handles CMB_LOCATIONS.ActionOnButtonClick
            If e.DefaultButton = ADB.Open Or e.DefaultButton = ADB.Add Then
                Dim t$ = "Select a new destination for "
                If MyIsMultipleUsers Then
                    t &= "multiple users"
                Else
                    t &= $"{IIf(If(MyNonMyltipleUser?.IsCollection, False), "collection", "user")}"
                    If Not MyNonMyltipleUser Is Nothing Then t &= $" [{MyNonMyltipleUser}]"
                End If

                Dim f As SFile = SFile.SelectPath(MyInitialLocation, t)
                If Not f.IsEmptyString Then
                    _SuspendUpdate = True
                    CMB_LOCATIONS.Text = f.PathWithSeparator
                    _SuspendUpdate = False
                    UpdateOptions(True)
                End If
            End If
        End Sub
        Private Sub CMB_LOCATIONS_ActionSelectedItemBeforeChanged(ByVal Sender As Object, ByVal e As EventArgs, ByVal Item As ListViewItem) Handles CMB_LOCATIONS.ActionSelectedItemBeforeChanged
            _SuspendUpdate = True
        End Sub
        Private Sub CMB_LOCATIONS_ActionSelectedItemChanged(ByVal Sender As Object, ByVal e As EventArgs, ByVal Item As ListViewItem) Handles CMB_LOCATIONS.ActionSelectedItemChanged
            Dim i% = CMB_LOCATIONS.SelectedIndex
            If i.ValueBetween(0, Settings.DownloadLocations.Count - 1) Then
                Select Case Settings.DownloadLocations(i).Model
                    Case PathCreationModel.Path, PathCreationModel.Collection : OPT_LOCATION_1.Checked = True
                    Case PathCreationModel.Path_UserName : OPT_LOCATION_2.Checked = True
                    Case PathCreationModel.Path_UserSite_UserName : OPT_LOCATION_3.Checked = True
                End Select
            End If
            _SuspendUpdate = False
            UpdateOptions(False, False, False)
        End Sub
        Private Sub CMB_LOCATIONS_ActionOnCheckedChange(ByVal Sender As Object, ByVal e As EventArgs, ByVal Checked As Boolean) Handles CMB_LOCATIONS.ActionOnCheckedChange
            If Not MyDefs.Initializing Then CMB_LOCATIONS.CaptionText = IIf(Checked, "Global path", "Path")
        End Sub
        Private Sub CMB_LOCATIONS_ActionOnTextChanged(ByVal Sender As Object, ByVal e As EventArgs) Handles CMB_LOCATIONS.ActionOnTextChanged
            UpdateOptions(False)
        End Sub
        Private _SuspendUpdate As Boolean = False
        Private Sub UpdateOptions(ByVal ResetColName As Boolean, Optional ByVal UpdateLocation As Boolean = True, Optional ByVal UpdateColName As Boolean = True)
            Const uSiteDef$ = "[UserSite]"
            Const uNameDef$ = "[UserName]"
            If Not _SuspendUpdate Then
                _SuspendUpdate = True
                Dim loc As SFile = SFile.GetPath(CMB_LOCATIONS.Text)
                If Not loc.IsEmptyString Then
                    If MyIsCollectionSelector And ResetColName Then
                        If UpdateColName Then TXT_COL_NAME.Text = loc.Segments.LastOrDefault
                        If UpdateLocation Then loc = loc.CutPath
                        CMB_LOCATIONS.Text = loc.PathWithSeparator
                    End If
                    Dim cName$ = TXT_COL_NAME.Text
                    If Not cName.IsEmptyString Then cName &= "\"
                    Dim p$ = loc.PathWithSeparator
                    Dim uSite$ = If(MyIsMultipleUsers, uSiteDef, _UserSite.IfNullOrEmpty(uSiteDef))
                    Dim uName$ = If(MyIsMultipleUsers, uNameDef, _UserName.IfNullOrEmpty(uNameDef))
                    OPT_LOCATION_1.Text = p & vbCr & $"{p}{cName}{uSite}_{uName}\"
                    OPT_LOCATION_2.Text = $"{p}{uName}\"
                    OPT_LOCATION_3.Text = $"{p}{uSite}\{uName}\"
                Else
                    With {OPT_LOCATION_1, OPT_LOCATION_2, OPT_LOCATION_3}.ToList : .ForEach(Sub(opt) opt.Text = String.Empty) : End With
                End If
                _SuspendUpdate = False
            End If
        End Sub
#End Region
    End Class
End Namespace