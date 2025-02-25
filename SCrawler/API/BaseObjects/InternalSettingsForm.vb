' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports System.Reflection
Imports System.ComponentModel
Imports PersonalUtilities.Forms
Imports SCrawler.Plugin
Imports SCrawler.Plugin.Attributes
Namespace API.Base
    Friend Class InternalSettingsForm
        Private WithEvents MyDefs As DefaultFormOptions
        Private ReadOnly Property MySettingsInstance As ISiteSettings
        Private ReadOnly Property MyObject As Object
        Private ReadOnly IsSettingsForm As Boolean = True
        Private ReadOnly Property MyMembers As List(Of MemberOption)
        ''' <summary>Default: 200</summary>
        Friend Property MinimumWidth As Integer = 200
        Private Class MemberOption : Inherits Hosts.PropertyValueHost
            Friend ToolTip As String
            Friend Caption As String
            Friend ThreeState As Boolean = False
            Friend AllowNull As Boolean = True
            Friend Provider As Type
            Friend Overrides Property Type As Type
                Get
                    Return _Type
                End Get
                Set(ByVal t As Type)
                    MyBase.Type = t
                End Set
            End Property
            Friend Overrides ReadOnly Property Name As String
                Get
                    Return Member.Name
                End Get
            End Property
            Friend Overrides Property LeftOffset As Integer
                Get
                    Return If(_LeftOffset, 0)
                End Get
                Set(ByVal NewOffset As Integer)
                    MyBase.LeftOffset = NewOffset
                End Set
            End Property
            Private ReadOnly _MinimumWidth As Integer? = Nothing
            Friend ReadOnly Property Width As Integer
                Get
                    Return LeftOffset + If(_MinimumWidth, 0) + If(TypeOf Control Is CheckBox, 0, 200) +
                           PaddingE.GetOf({Control}).Horizontal(2) + MeasureText(Caption, Control.Font).Width
                End Get
            End Property
            Friend OptName As String = String.Empty
            Friend Sub New(ByRef PropertySource As Object, ByVal m As MemberInfo, ByVal ps As PSettingAttribute, ByVal po As PropertyOption)
                Source = PropertySource
                Member = m
                _Type = Member.GetMemberType
                _Value = Member.GetMemberValue(PropertySource)
                With ps
                    ToolTip = .ToolTip
                    Caption = .Caption
                    ThreeState = .ThreeState
                    AllowNull = .AllowNull
                    Provider = .Provider
                    _LeftOffset = .LeftOffsetGet
                    ControlNumber = .Number
                    _MinimumWidth = .MinimumWidth
                End With
                If Not po Is Nothing Then
                    With po
                        OptName = po.Name
                        If ToolTip.IsEmptyString Then ToolTip = .ControlToolTip
                        If Caption.IsEmptyString Then Caption = .ControlText
                    End With
                End If
            End Sub
            Protected Overrides ReadOnly Property Control_IsInformationLabel As Boolean
                Get
                    Return False
                End Get
            End Property
            Protected Overrides ReadOnly Property Control_ThreeStates As Boolean
                Get
                    Return ThreeState
                End Get
            End Property
            Protected Overrides ReadOnly Property Control_Caption As String
                Get
                    Return Caption
                End Get
            End Property
            Protected Overrides ReadOnly Property Control_ToolTip As String
                Get
                    Return ToolTip
                End Get
            End Property
            Friend Overloads Sub CreateControl(ByVal f As FieldsChecker, ByVal TT As ToolTip)
                CreateControl(TT)
                If Not Provider Is Nothing Then f.AddControl(Control, Caption, Type, AllowNull, Activator.CreateInstance(Provider))
            End Sub
        End Class
        Friend Sub New(ByVal Obj As Object, ByVal s As ISiteSettings, ByVal _IsSettingsForm As Boolean)
            InitializeComponent()
            MyDefs = New DefaultFormOptions(Me, Settings.Design)
            MyMembers = New List(Of MemberOption)

            MyObject = Obj
            MySettingsInstance = s
            IsSettingsForm = _IsSettingsForm
            If _IsSettingsForm Then
                Text = "Settings"
            Else
                Text = "Options"
            End If
            Icon = s.Icon
        End Sub
        Private Sub InternalSettingsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                With MyDefs
                    .MyViewInitialize(True)
                    .AddOkCancelToolbar()

                    Dim members As IEnumerable(Of MemberInfo)
                    Dim member As MemberInfo
                    Dim attr As PSettingAttribute
                    Dim opt As PropertyOption
                    Dim providersMembersSettings As IEnumerable(Of MemberInfo)
                    Dim providersMembersObj As IEnumerable(Of MemberInfo)
                    Dim providersPredicate As Func(Of MemberInfo, Boolean) = Function(m) m.MemberType = MemberTypes.Property AndAlso
                                                                                         m.GetMemberCustomAttributes(Of Provider).ListExists
                    Dim m1 As MemberInfo, m2 As MemberInfo
                    Dim tmpObj As Object
                    Dim maxOffset%

                    members = GetObjectMembers(MyObject, Function(m) (m.MemberType = MemberTypes.Field Or m.MemberType = MemberTypes.Property) AndAlso
                                                                     Not m.GetCustomAttribute(Of PSettingAttribute) Is Nothing,, True,
                                                                     New FComparer(Of MemberInfo)(Function(mm1, mm2) mm1.Name = mm2.Name))
                    providersMembersSettings = GetObjectMembers(MySettingsInstance, providersPredicate)
                    providersMembersObj = GetObjectMembers(MyObject, providersPredicate)

                    If members.ListExists Then
                        For Each member In members
                            attr = member.GetMemberCustomAttribute(Of PSettingAttribute)
                            If Not attr Is Nothing AndAlso
                               (attr.Address = SettingAddress.Both OrElse
                                (
                                    (IsSettingsForm And attr.Address = SettingAddress.Settings) Or
                                    (Not IsSettingsForm And attr.Address = SettingAddress.User)
                                )
                               ) Then
                                opt = Nothing

                                If Not attr.NameAssoc.IsEmptyString Then
                                    m1 = GetObjectMembers(MyObject, Function(m) m.Name = attr.NameAssocInstance).FirstOrDefault
                                    If Not m1 Is Nothing AndAlso (m1.MemberType = MemberTypes.Property Or
                                                                  m1.MemberType = MemberTypes.Field Or
                                                                  m1.MemberType = MemberTypes.Method) Then
                                        tmpObj = m1.GetMemberValue(MyObject)
                                        If Not tmpObj Is Nothing Then
                                            m2 = GetObjectMembers(tmpObj, Function(m) m.Name = attr.NameAssoc).FirstOrDefault
                                            If Not m2 Is Nothing Then opt = m2.GetMemberCustomAttribute(Of PropertyOption)
                                        End If
                                    End If
                                End If

                                MyMembers.Add(New MemberOption(MyObject, member, attr, opt))
                            End If
                        Next
                    End If

                    .MyFieldsCheckerE = New FieldsChecker

                    If MyMembers.Count > 0 Then

                        maxOffset = MyMembers.Max(Function(mm) mm.LeftOffset)
                        If maxOffset > 0 Then MyMembers.ForEach(Sub(mm) mm.LeftOffset = maxOffset)

                        Dim prov As IEnumerable(Of Provider)
                        Dim _prov As Provider
                        Dim si% = -1
                        Dim i%
                        For Each provEnum In {providersMembersObj, providersMembersSettings}
                            si += 1
                            If provEnum.ListExists Then
                                For Each member In provEnum
                                    prov = member.GetMemberCustomAttributes(Of Provider)
                                    If prov.ListExists Then
                                        For Each _prov In prov
                                            i = MyMembers.FindIndex(Function(m) If(si = 0, m.Name, m.OptName) = _prov.Name)
                                            If i >= 0 Then MyMembers(i).SetProvider(member.GetMemberValue(If(si = 0, MyObject, CObj(MySettingsInstance))), _prov)
                                        Next
                                    End If
                                Next
                            End If
                        Next

                        TP_MAIN.RowStyles.Clear()
                        TP_MAIN.RowCount = 0
                        For i% = 0 To MyMembers.Count - 1
                            With MyMembers(i)
                                .CreateControl(MyDefs.MyFieldsCheckerE, TT_MAIN)
                                TP_MAIN.RowStyles.Add(New RowStyle(SizeType.Absolute, .ControlHeight))
                                TP_MAIN.RowCount += 1
                                TP_MAIN.Controls.Add(.Control, 0, TP_MAIN.RowStyles.Count - 1)
                            End With
                        Next
                    Else
                        Throw New ArgumentOutOfRangeException("Members", "Settings instance does not contain settings members")
                    End If

                    .MyFieldsChecker.EndLoaderOperations()

                    Dim s As Size = Size
                    s.Height += (MyMembers.Sum(Function(m) m.ControlHeight) +
                                (PaddingE.GetOf({TP_MAIN},,,,,, 0).Vertical(MyMembers.Count - 1) / 2).RoundDown + MyMembers.Count - 1)
                    s.Width = MyMembers.Max(Function(m) m.Width) + PaddingE.GetOf({TP_MAIN, CONTAINER_MAIN, CONTAINER_MAIN.ContentPanel, Me}, False).Horizontal(2)
                    If MinimumWidth > 0 And s.Width < MinimumWidth Then s.Width = MinimumWidth
                    Size = s
                    MinimumSize = s
                    MaximumSize = s

                    .EndLoaderOperations()
                End With
            Catch ex As Exception
                MyDefs.InvokeLoaderError(ex)
            End Try
        End Sub
        Private Sub InternalSettingsForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
            TP_MAIN.Controls.Clear()
            MyMembers.ListClearDispose
        End Sub
        Private Sub MyDefs_ButtonOkClick(ByVal Sender As Object, ByVal e As KeyHandleEventArgs) Handles MyDefs.ButtonOkClick
            If MyDefs.MyFieldsChecker.AllParamsOK Then
                MyMembers.ForEach(Sub(m) m.UpdateValueByControl())
                MyDefs.CloseForm()
            End If
        End Sub
    End Class
End Namespace