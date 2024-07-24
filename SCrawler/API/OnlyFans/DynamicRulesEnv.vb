' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Functions.XML
Imports PersonalUtilities.Functions.XML.Base
Imports PersonalUtilities.Functions.RegularExpressions
Imports PersonalUtilities.Tools
Imports PersonalUtilities.Tools.Web.Clients
Imports PersonalUtilities.Tools.Web.Clients.Base
Imports PersonalUtilities.Tools.Web.Documents.JSON
Imports System.Text.RegularExpressions
Namespace API.OnlyFans
    Friend Structure DynamicRulesValue : Implements IComparable(Of DynamicRulesValue), IEquatable(Of DynamicRulesValue), IEContainerProvider
#Region "XML names"
        Private Const Name_UrlRepo As String = "UrlRepo"
        Private Const Name_UrlRaw As String = "UrlRaw"
        Private Const Name_UrlLatestCommit As String = "UrlLatestCommit"
        Private Const Name_UpdatedAt As String = "UpdatedAt"
        Private Const Name_Broken As String = "Broken"
        Private Const Name_Exists As String = "Exists"
#End Region
#Region "Declarations"
        Friend UrlRepo As String
        Friend UrlRaw As String
        Friend UrlLatestCommit As String
        Friend UpdatedAt As Date
        Friend Broken As Boolean
        Friend Exists As Boolean
        Friend ReadOnly Property Valid As Boolean
            Get
                Return Not UrlRepo.IsEmptyString And Not UrlRaw.IsEmptyString
            End Get
        End Property
#End Region
#Region "Initializers"
        Friend Sub New(ByVal e As EContainer)
            UrlRepo = e.Value(Name_UrlRepo)
            UrlRaw = e.Value(Name_UrlRaw)
            UrlLatestCommit = e.Value(Name_UrlLatestCommit)
            UpdatedAt = e.Value(Name_UpdatedAt).ToDateDef(Now.AddYears(-10))
            Broken = e.Value(Name_Broken).FromXML(Of Boolean)(False)
            Exists = e.Value(Name_Exists).FromXML(Of Boolean)(True)
        End Sub
        Public Shared Widening Operator CType(ByVal e As EContainer) As DynamicRulesValue
            Return New DynamicRulesValue(e)
        End Operator
        Public Shared Widening Operator CType(ByVal rule As DynamicRulesValue) As String
            Return rule.ToString
        End Operator
#End Region
#Region "Base functions"
        Public Overrides Function GetHashCode() As Integer
            Return ToString.GetHashCode
        End Function
        Public Overrides Function ToString() As String
            Return UrlRaw
        End Function
#End Region
#Region "IComparable Support"
        Private Function CompareTo(ByVal Other As DynamicRulesValue) As Integer Implements IComparable(Of DynamicRulesValue).CompareTo
            Return UpdatedAt.CompareTo(Other.UpdatedAt) * -1
        End Function
#End Region
#Region "IEquatable Support"
        Public Overloads Overrides Function Equals(ByVal Obj As Object) As Boolean
            If Not IsNothing(Obj) Then
                If TypeOf Obj Is String Then
                    Dim _obj$ = CStr(Obj).StringTrim.StringToLower
                    Return UrlRepo = _obj Or UrlRaw = _obj
                Else
                    Return Equals(DirectCast(Obj, DynamicRulesValue))
                End If
            Else
                Return False
            End If
        End Function
        Friend Overloads Function Equals(ByVal Other As DynamicRulesValue) As Boolean Implements IEquatable(Of DynamicRulesValue).Equals
            Return UrlRepo = Other.UrlRepo Or UrlRaw = Other.UrlRaw
        End Function
#End Region
#Region "IEContainerProvider Support"
        Private Function ToEContainer(Optional ByVal e As ErrorsDescriber = Nothing) As EContainer Implements IEContainerProvider.ToEContainer
            Return New EContainer("Rule") From {
                New EContainer(Name_UrlRepo, UrlRepo),
                New EContainer(Name_UrlRaw, UrlRaw),
                New EContainer(Name_UrlLatestCommit, UrlLatestCommit),
                New EContainer(Name_UpdatedAt, UpdatedAt.ToStringDateDef),
                New EContainer(Name_Broken, Broken.BoolToInteger),
                New EContainer(Name_Exists, Exists.BoolToInteger)
            }
        End Function
#End Region
    End Structure
    Friend Class DynamicRulesEnv : Implements ICopier, IEnumerable(Of DynamicRulesValue), IMyEnumerator(Of DynamicRulesValue), IDisposable
        Friend Enum Modes As Integer
            List = 0
            Personal = 1
        End Enum
#Region "Constants"
        Friend Const UpdateIntervalDefault As Integer = 1440 '60 * 24
        Friend Const DynamicRulesConfigNodeName_URL As String = "DYNAMIC_GENERIC_URL"
        Friend Const DynamicRulesConfigNodeName_RULES As String = "DYNAMIC_RULE"

        Friend Const DynamicRulesConfig_Mode_NodeName As String = "dynamic-mode-default"
        'Friend Const DynamicRulesConfig_Mode_NodeValue As String = "generic"

        Friend Const DynamicRulesConfigNodeName_URL_CONST_NAME As String = "RULE_VALUE"
#End Region
#Region "XML names"
        Private Const Name_LastUpdateTimeFile As String = "LastUpdateTimeFile"
        Private Const Name_LastUpdateTimeRules As String = "LastUpdateTimeRules"
        Private Const Name_ProtectFile As String = "ProtectFile"
        Private Const Name_UpdateInterval As String = "UpdateInterval"
        Private Const Name_Mode As String = "Mode"
        Private Const Name_PersonalRule As String = "PersonalRule"
        Private Const Name_RulesForceUpdateRequired As String = "RulesForceUpdateRequired"
        Private Const Name_AddErrorsToLog As String = "AddErrorsToLog"
        Private Const Name_ConfigLastDateUpdate As String = "ConfigLastDateUpdate"
        Private Const Name_ConfigAutoUpdate As String = "ConfigAutoUpdate"
        Private Const Name_RulesConfigManualMode As String = "RulesConfigManualMode"
        Private Const Name_RulesUpdateConst As String = "RulesUpdateConst"
        Private Const Name_RulesReplaceConfig As String = "RulesReplaceConfig"
#End Region
#Region "Declarations"
        Private ReadOnly Rules As List(Of DynamicRulesValue)
        Friend ReadOnly Property RulesConstants As Dictionary(Of String, String)
#Region "Regex patterns"
        Private ReadOnly ReplacePattern_RepoToRaw As RParams
        Private ReadOnly ReplacePattern_RawToRepo As RParams
        Private ReadOnly ReplacePattern_JsonInfo As RParams
        Private ReadOnly ConfigRulesExtract As RParams
#End Region
#Region "Dates"
        Private LastUpdateTimeFile As Date = Now.AddYears(-1)
        Private LastUpdateTimeRules As Date = Now.AddYears(-1)
#End Region
#Region "Files"
        Friend ReadOnly OFScraperConfigPatternFile As SFile = $"{SettingsFolderName}\OFScraperConfigPattern.json"
        Friend ReadOnly OFScraperConfigPatternFileConst As SFile = $"{SettingsFolderName}\OFScraperConfigPatternConstants.txt"
        Friend ReadOnly Property AuthFile As New SFile($"{SettingsFolderName}\OnlyFans_Auth.json")
        Private ReadOnly DynamicRulesFile As SFile
        Private ReadOnly DynamicRulesXml As SFile
        Private Shared ReadOnly Property DynamicRulesFileImpl As SFile
            Get
                Return $"{SettingsFolderName}\OnlyFansDynamicRules.txt"
            End Get
        End Property
        Friend Shared Sub ValidateRulesFile()
            Dim f As SFile = DynamicRulesFileImpl
            If Not f.Exists Then TextSaver.SaveTextToFile(My.Resources.OFResources.DynamicRules, DynamicRulesFileImpl, True)
        End Sub
        Friend Property ProtectFile As Boolean = False
#End Region
        Friend Property UpdateInterval As Integer = UpdateIntervalDefault
        Friend Property Mode As Modes = Modes.List
        Friend Property PersonalRule As String = String.Empty
        Friend Property RulesForceUpdateRequired As Boolean = False
        Friend Property RulesUpdateConst As Boolean = True
        Friend Property RulesReplaceConfig As Boolean = True
        Private ReadOnly Responser As New Responser With {.Accept = "application/json"}
        Private ReadOnly RulesLinesComparer As New FComparer(Of String)(Function(x, y) x.StringToLower = y.StringToLower)
        Private ReadOnly OFLOG As TextSaver
        Private ReadOnly OFError As ErrorsDescriber
        Friend Property AddErrorsToLog As Boolean = True
        Friend Property NeedToSave As Boolean = False
        Private ReadOnly Property ConfigAddress As DynamicRulesValue
        Private ReadOnly Property ConfigConstAddress As DynamicRulesValue
        Private Property ConfigLastDateUpdate As Date = Now.AddYears(-1)
        Friend Property ConfigAutoUpdate As Boolean = True
        Friend Property RulesConfigManualMode As Boolean = True
#End Region
#Region "Current, Item, Count"
        Private _CurrentRule As DynamicRulesValue
        Private _CurrentContainer As EContainer
        Private _CurrentContainerRulesText As String = String.Empty
        Friend ReadOnly Property CurrentRule As DynamicRulesValue
            Get
                Return _CurrentRule
            End Get
        End Property
        Friend ReadOnly Property CurrentContainer As EContainer
            Get
                Return _CurrentContainer
            End Get
        End Property
        Friend ReadOnly Property CurrentContainerRulesText As String
            Get
                If _CurrentContainerRulesText.IsEmptyString AndAlso AuthFile.Exists Then _
                   _CurrentContainerRulesText = AuthFile.GetText(OFError).StringTrim
                Return _CurrentContainerRulesText
            End Get
        End Property
        Friend ReadOnly Property Exists As Boolean
            Get
                Return CurrentContainer.ListExists
            End Get
        End Property
        Default Friend ReadOnly Property Item(ByVal Index As Integer) As DynamicRulesValue Implements IMyEnumerator(Of DynamicRulesValue).MyEnumeratorObject
            Get
                Return Rules(Index)
            End Get
        End Property
        Friend ReadOnly Property Count As Integer Implements IMyEnumerator(Of DynamicRulesValue).MyEnumeratorCount
            Get
                Return Rules.Count
            End Get
        End Property
#End Region
#Region "Initializer"
        Friend Sub New()
            Rules = New List(Of DynamicRulesValue)
            DynamicRulesFile = DynamicRulesFileImpl
            DynamicRulesXml = DynamicRulesFile
            DynamicRulesXml.Extension = "xml"
            ReplacePattern_RepoToRaw = New RParams("(.*github.com/([^/]+)/([^/]+)/blob/(.+))", Nothing, 0,
                                                   RegexReturn.ReplaceChangeListMatch, EDP.ReturnValue) With {
                                                   .PatternReplacement = "https://raw.githubusercontent.com/{2}/{3}/{4}"}
            ReplacePattern_JsonInfo = ReplacePattern_RepoToRaw.Copy
            ReplacePattern_JsonInfo.PatternReplacement = "https://github.com/{2}/{3}/latest-commit/{4}"
            ReplacePattern_RawToRepo = ReplacePattern_RepoToRaw.Copy
            ReplacePattern_RawToRepo.Pattern = "(.*raw.githubusercontent.com/([^/]+)/([^/]+)/([^/]+)/(.+))"
            ReplacePattern_RawToRepo.PatternReplacement = "https://github.com/{2}/{3}/blob/{4}/{5}"
            ConfigRulesExtract = RParams.DMS("DYNAMIC_RULE"":(\{.+?\}[\r\n]+)", 1, RegexOptions.Singleline, EDP.ReturnValue)
            OFLOG = New TextSaver($"LOGs\OF_{Now:yyyyMMdd_HHmmss}.txt") With {.LogMode = True, .AutoSave = True, .AutoClear = True}
            AddHandler OFLOG.TextSaved, AddressOf OFLOG_TextSaved
            OFError = New ErrorsDescriber(EDP.SendToLog + EDP.ReturnValue) With {.DeclaredMessage = New MMessage With {.Loggers = {OFLOG}, .Exists = True}}
            Responser.DeclaredError = OFError
            Responser.ProcessExceptionDecision =
                Function(ByVal Status As IResponserStatus, ByVal NullArg As Object, ByVal CurrentError As ErrorsDescriber) As ErrorsDescriber
                    If Status.StatusCode = Net.HttpStatusCode.NotFound Then
                        CurrentError.SendToLogOnlyMessage = True
                        Dim m As MMessage = CurrentError.DeclaredMessage.Clone
                        m.Text = $"Nothing found at URL: {Responser.LatestUrlString}"
                        CurrentError.DeclaredMessage = m
                        Status.ErrorException = New ErrorsDescriberException(m.Text,,, Status.ErrorException) With {.ReplaceMainMessage = True}
                    End If
                    Return CurrentError
                End Function
            ConfigAddress = ParseURL("https://github.com/AAndyProgram/SCrawler/blob/main/SCrawler/API/OnlyFans/OFScraperConfigPattern.json")
            ConfigConstAddress = ParseURL("https://github.com/AAndyProgram/SCrawler/blob/main/SCrawler/API/OnlyFans/OFScraperConfigPatternConstants.txt")
            RulesConstants = New Dictionary(Of String, String)
        End Sub
#End Region
#Region "Log handlers"
        Private _OFLOG_ProcessNotify As Boolean = True
        Private Sub OFLOG_TextSaved(sender As Object, e As EventArgs)
            If _OFLOG_ProcessNotify And AddErrorsToLog Then _OFLOG_ProcessNotify = False : MyMainLOG = $"The OnlyFans log contains errors: {OFLOG.File}"
        End Sub
#End Region
#Region "ParseURL"
        Private Const SiteGitHub As String = "github.com"
        Private Const SiteGitHubRaw As String = "raw.githubusercontent.com"
        Friend Function ParseURL(ByVal URL As String) As DynamicRulesValue
            URL = URL.StringTrim
            If Not URL.IsEmptyString Then
                Dim r As New DynamicRulesValue
                Dim rGet As Func(Of String, RParams, String) = Function(__url, pattern) DirectCast(RegexReplace(__url, pattern), IEnumerable(Of String)).FirstOrDefault
                If URL.ToLower.Contains(SiteGitHubRaw) Then
                    r.UrlRaw = URL
                    r.UrlRepo = rGet(URL, ReplacePattern_RawToRepo)
                ElseIf URL.ToLower.Contains(SiteGitHub) Then
                    r.UrlRepo = URL
                    r.UrlRaw = rGet(URL, ReplacePattern_RepoToRaw)
                End If

                If r.Valid Then
                    r.UpdatedAt = Now.AddYears(-1)
                    r.UrlLatestCommit = rGet(r.UrlRepo, ReplacePattern_JsonInfo)
                    r.Exists = True
                    Return r
                End If
            End If
            Return Nothing
        End Function
#End Region
#Region "GetFormat"
        Private Shared ReadOnly Property ConfigNodes As String()
            Get
                Return {"advanced_options", "DYNAMIC_RULE"}
            End Get
        End Property
        Private Const FormatMidPart As String = ":{0}:{1:x}:"
        Private ReadOnly FormatExtract As RParams = RParams.DM("(\S+)\s*:\s*\{\s*\d?\s*\}\s*:\s*\{\s*\d?\s*:\s*x\s*\}\s*:\s*(\S+)", 0, RegexReturn.ListByMatch, EDP.ReturnValue)
        Private ReadOnly ContainerStrConv As New CustomProvider(Function(input) If(ACheck(Of Integer)(input), input, $"""{input}"""))
        Private ReadOnly ContainerConv As New CustomProvider(Function(ByVal e As Object) As Object
                                                                 With DirectCast(e, EContainer)
                                                                     Dim value$ = String.Empty
                                                                     If .ListExists Then
                                                                         value = .Select(Function(ee) ee(0).Value).ListToStringE(",", ContainerStrConv, False, String.Empty, EDP.ReturnValue)
                                                                         If Not value.IsEmptyString Then value = $"[{value}]"
                                                                     Else
                                                                         value = AConvert(Of String)(.Value, ContainerStrConv, String.Empty, EDP.SendToLog, EDP.ReturnValue)
                                                                     End If
                                                                     If Not value.IsEmptyString Then
                                                                         value = $"""{ .Name}"": {value}"
                                                                     Else
                                                                         value = $"""{ .Name}"": """""
                                                                     End If
                                                                     Return value
                                                                 End With
                                                             End Function)
        Friend Shared Function GetFormat(ByVal j As EContainer, Optional ByVal Check As Boolean = False,
                                         Optional ByRef CheckResult As Boolean = False,
                                         Optional ByVal TryConfig As Boolean = False, Optional ByRef IsConfig As Boolean = False) As String
            Dim pattern$ = String.Empty
            With If(TryConfig, j(ConfigNodes), j)
                If .ListExists Then
                    If Not .Value("format").IsEmptyString Then
                        pattern = .Value("format").Replace("{}", "{0}").Replace("{:x}", "{1:x}")
                    ElseIf Not .Value("prefix").IsEmptyString And Not .Value("suffix").IsEmptyString Then
                        pattern = .Value("prefix") & FormatMidPart & .Value("suffix")
                    ElseIf Not .Value("start").IsEmptyString And Not .Value("end").IsEmptyString Then
                        pattern = .Value("start") & FormatMidPart & .Value("end")
                    End If

                    Dim result As Boolean = Not pattern.IsEmptyString And .Item("checksum_indexes").ListExists And
                                            Not .Value("static_param").IsEmptyString And Not .Value("checksum_constant").IsEmptyString
                    If Check Then CheckResult = result
                    If Not result And Not TryConfig Then Return GetFormat(j, Check, CheckResult, True, IsConfig)
                End If
            End With
            Return pattern
        End Function
        Private Function ConvertAuthText() As String
            Dim result$ = String.Empty
            With CurrentContainer
                If .ListExists Then
                    Dim f$ = GetFormat(.Self)
                    If Not f.IsEmptyString Then
                        Dim l As List(Of String) = RegexReplace(f, FormatExtract)
                        If l.ListExists(3) Then
                            Dim s$ = l(1), e$ = l(2)
                            .Value("format") = s & FormatMidPart & e
                            .Value("prefix") = s
                            .Value("suffix") = e
                            .Value("start") = s
                            .Value("end") = e
                            Dim t$ = .ListToStringE(",", ContainerConv, False)
                            If Not t.IsEmptyString Then t = "{" & t & "}"
                            Return t
                        End If
                    End If
                End If
            End With
            Return String.Empty
        End Function
#End Region
#Region "Load, Save"
        Private Function GetTextLines(ByVal Input As String) As List(Of String)
            If Not Input.IsEmptyString Then
                Return ListAddList(Nothing, Input.StringTrim.Split(vbLf), LAP.NotContainsOnly, EDP.ReturnValue,
                                   CType(Function(inp$) inp.StringTrim, Func(Of Object, Object)))
            Else
                Return New List(Of String)
            End If
        End Function
        Private Sub ParseConsts(ByVal Source As String)
            If Not Source.IsEmptyString Then
                Dim l As List(Of String) = GetTextLines(Source)
                Dim v$()
                If l.ListExists Then
                    RulesConstants.Clear()
                    For Each value$ In l
                        If Not value.IsEmptyString Then
                            v = value.Split("=")
                            If v.ListExists(2) Then RulesConstants.Add(v(0), v(1))
                        End If
                    Next
                End If
            End If
        End Sub
        Private Const RulesNode As String = "Rules"
        Private _InitialValuesLoaded As Boolean = False
        Private Sub LoadInitialValues()
            If Not _InitialValuesLoaded Then
                _InitialValuesLoaded = True

                If Not OFScraperConfigPatternFile.Exists Then
                    Dim t$ = Text.Encoding.UTF8.GetString(My.Resources.OFResources.OFScraperConfigPattern)
                    TextSaver.SaveTextToFile(t, OFScraperConfigPatternFile, True)
                End If

                If Not OFScraperConfigPatternFileConst.Exists Then _
                   TextSaver.SaveTextToFile(My.Resources.OFResources.OFScraperConfigPatternConstants, OFScraperConfigPatternFileConst, True)

                If OFScraperConfigPatternFileConst.Exists Then ParseConsts(OFScraperConfigPatternFileConst.GetText(OFError))

                If DynamicRulesXml.Exists Then
                    Rules.Clear()
                    Using x As New XmlFile(DynamicRulesXml, Protector.Modes.All, False) With {.XmlReadOnly = True, .AllowSameNames = True}
                        x.LoadData(OFError)
                        Dim dNull As Date = Now.AddYears(-1)
                        LastUpdateTimeFile = x.Value(Name_LastUpdateTimeFile).ToDateDef(dNull)
                        LastUpdateTimeRules = x.Value(Name_LastUpdateTimeRules).ToDateDef(dNull)
                        ProtectFile = x.Value(Name_ProtectFile).FromXML(Of Boolean)(False)
                        Mode = x.Value(Name_Mode).FromXML(Of Integer)(Modes.List)
                        UpdateInterval = x.Value(Name_UpdateInterval).FromXML(Of Integer)(UpdateIntervalDefault)
                        PersonalRule = x.Value(Name_PersonalRule)
                        RulesForceUpdateRequired = x.Value(Name_RulesForceUpdateRequired).FromXML(Of Boolean)(False)
                        RulesUpdateConst = x.Value(Name_RulesUpdateConst).FromXML(Of Boolean)(True)
                        RulesReplaceConfig = x.Value(Name_RulesReplaceConfig).FromXML(Of Boolean)(True)
                        AddErrorsToLog = x.Value(Name_AddErrorsToLog).FromXML(Of Boolean)(False)
                        ConfigAutoUpdate = x.Value(Name_ConfigAutoUpdate).FromXML(Of Boolean)(True)
                        RulesConfigManualMode = x.Value(Name_RulesConfigManualMode).FromXML(Of Boolean)(True)
                        ConfigLastDateUpdate = x.Value(Name_ConfigLastDateUpdate).ToDateDef(Now.AddYears(-1))
                        If x.Contains(RulesNode) Then Rules.ListAddList(x({RulesNode}), LAP.IgnoreICopier, OFError)
                    End Using
                End If
            End If
        End Sub
        Friend Sub Save()
            Using x As New XmlFile With {.AllowSameNames = True, .Name = "DynamicRules"}
                x.Add(Name_LastUpdateTimeFile, LastUpdateTimeFile.ToStringDateDef)
                x.Add(Name_LastUpdateTimeRules, LastUpdateTimeRules.ToStringDateDef)
                x.Add(Name_ProtectFile, ProtectFile.BoolToInteger)
                x.Add(Name_Mode, CInt(Mode))
                x.Add(Name_UpdateInterval, UpdateInterval)
                x.Add(Name_PersonalRule, PersonalRule)
                x.Add(Name_RulesForceUpdateRequired, RulesForceUpdateRequired.BoolToInteger)
                x.Add(Name_RulesUpdateConst, RulesUpdateConst.BoolToInteger)
                x.Add(Name_RulesReplaceConfig, RulesReplaceConfig.BoolToInteger)
                x.Add(Name_AddErrorsToLog, AddErrorsToLog.BoolToInteger)
                x.Add(Name_ConfigAutoUpdate, ConfigAutoUpdate.BoolToInteger)
                x.Add(Name_RulesConfigManualMode, RulesConfigManualMode.BoolToInteger)
                x.Add(Name_ConfigLastDateUpdate, ConfigLastDateUpdate.ToStringDateDef)
                If Count > 0 Then
                    Rules.Sort()
                    x.Add(New EContainer(RulesNode))
                    x.Last.AddRange(Rules)
                End If
                x.Save(DynamicRulesXml, OFError)
            End Using
            If Count > 0 Then
                Using t As New TextSaver(DynamicRulesFile)
                    Rules.ForEach(Sub(r) If Not r.UrlRepo.IsEmptyString Then t.AppendLine(r.UrlRepo))
                    t.Save(OFError)
                End Using
            End If
        End Sub
#End Region
#Region "Update"
        Private _UpdateInProgress As Boolean = False
        Private _ForcedUpdate As Boolean = False
        Friend Function Update(ByVal Force As Boolean, Optional ByVal LoadListOnly As Boolean = False) As Boolean
            Dim skip As Boolean = _UpdateInProgress
            If skip And _ForcedUpdate Then Force = False
            _ForcedUpdate = Force
            While _UpdateInProgress : Threading.Thread.Sleep(200) : End While
            If Not skip Or Force Then UpdateImpl(Force Or RulesForceUpdateRequired, LoadListOnly)
            Return Exists
        End Function
        Private Sub UpdateImpl(ByVal Force As Boolean, Optional ByVal LoadListOnly As Boolean = False)
            Try
                If Not _UpdateInProgress Then
                    _UpdateInProgress = True

                    LoadInitialValues()

                    Dim r$
                    Dim process As Boolean = False, updated As Boolean = False
                    Dim forceSave As Boolean = RulesForceUpdateRequired Or Not DynamicRulesFile.Exists Or Not DynamicRulesXml.Exists
                    Dim textLocal As List(Of String)
                    Dim i%
                    Dim rule As DynamicRulesValue
                    Dim e As EContainer
                    Dim errDate As Date = Now.AddYears(-1)
                    Dim d As Date?
                    '2024-06-12T12:44:06.000-05:00
                    Dim dateProvider As New ADateTime("yyyy-MM-ddTHH:mm:ss.fff%K")

                    RulesForceUpdateRequired = False

                    If Not DynamicRulesFile.Exists Then process = True : ValidateRulesFile()

                    'update rules list
                    If Not LoadListOnly And (LastUpdateTimeFile.AddMinutes(UpdateInterval) < Now Or process Or Force) Then
                        LastUpdateTimeFile = Now
                        r = Responser.GetResponse("https://raw.githubusercontent.com/AAndyProgram/SCrawler/main/SCrawler/API/OnlyFans/DynamicRules.txt")
                        If Not r.IsEmptyString Then
                            Dim textWeb As List(Of String) = GetTextLines(r)
                            Dim fileText$
                            If textWeb.ListExists Then
                                Using t As New TextSaver(DynamicRulesFile)
                                    If ProtectFile Then
                                        fileText = DynamicRulesFile.GetText(OFError)
                                        t.Append(fileText)
                                        textLocal = GetTextLines(fileText)
                                        If textLocal.ListExists Then _
                                           textLocal.ForEach(Sub(tt) If Not tt.IsEmptyString AndAlso Not textWeb.Contains(tt, RulesLinesComparer) Then _
                                                                     t.AppendLine(tt) : updated = True) : textLocal.Clear()
                                    Else
                                        t.Append(r)
                                        updated = True
                                    End If
                                    t.Save(OFError)
                                End Using
                                textWeb.Clear()
                            End If
                        End If
                    End If

                    'update config and consts
                    If Not LoadListOnly AndAlso ConfigAutoUpdate AndAlso ConfigLastDateUpdate.AddMinutes(UpdateInterval) < Now Then
                        Dim __upConf As Boolean = False
                        Dim __dConf As Date = ConfigLastDateUpdate
                        Dim parseConfigFiles As Action(Of DynamicRulesValue, SFile, Boolean) =
                            Sub(ByVal __rule As DynamicRulesValue, ByVal __fileSave As SFile, ByVal isConstFile As Boolean)
                                r = Responser.GetResponse(__rule.UrlLatestCommit)
                                If Not r.IsEmptyString Then
                                    e = JsonDocument.Parse(r, OFError)
                                    If e.ListExists Then
                                        d = AConvert(Of Date)(e.Value("date"), dateProvider, Nothing)
                                        Dim dConf As Date = If(d, errDate)
                                        If dConf > __dConf Then
                                            __dConf = dConf
                                            __upConf = True
                                            updated = True
                                            r = Responser.GetResponse(__rule.UrlRaw)
                                            If Not r.IsEmptyString Then
                                                TextSaver.SaveTextToFile(r, __fileSave, True, False, OFError)
                                                If isConstFile Then ParseConsts(r)
                                            End If
                                        End If
                                        e.Dispose()
                                    End If
                                End If
                            End Sub
                        'Update consts
                        If RulesUpdateConst Then parseConfigFiles(ConfigConstAddress, OFScraperConfigPatternFileConst, True)
                        'Update config
                        parseConfigFiles(ConfigAddress, OFScraperConfigPatternFile, False)
                        If __upConf Then ConfigLastDateUpdate = Now
                    End If

                    'generate rules, update rules dates
                    If LastUpdateTimeRules.AddMinutes(UpdateInterval) < Now Or updated Or Force Or LoadListOnly Then
                        process = True
                        If Mode = Modes.Personal And Not PersonalRule.IsEmptyString Then
                            If Not LoadListOnly Then LastUpdateTimeRules = Now : updated = True
                        Else
                            If Not LoadListOnly Then LastUpdateTimeRules = Now : updated = True
                            textLocal = GetTextLines(DynamicRulesFile.GetText(OFError))
                            If textLocal.ListExists Then
                                If Not LoadListOnly And Count > 0 Then
                                    For i = 0 To Count - 1
                                        rule = Rules(i)
                                        rule.Exists = False
                                        Rules(i) = rule
                                    Next
                                End If
                                For Each url$ In textLocal
                                    url = url.StringTrim
                                    If Not url.IsEmptyString Then
                                        i = IndexOf(url)
                                        If i >= 0 Then
                                            rule = Rules(i)
                                        Else
                                            rule = ParseURL(url)
                                            If rule.Valid Then
                                                i = Add(rule, False, False)
                                            Else
                                                rule = Nothing
                                            End If
                                        End If

                                        If Not LoadListOnly Then
                                            If i >= 0 And rule.Valid And Not rule.UrlLatestCommit.IsEmptyString Then
                                                rule.Exists = True
                                                r = Responser.GetResponse(rule.UrlLatestCommit)
                                                If Not r.IsEmptyString Then
                                                    e = JsonDocument.Parse(r, OFError)
                                                    If e.ListExists Then
                                                        d = AConvert(Of Date)(e.Value("date"), dateProvider, Nothing)
                                                        rule.UpdatedAt = If(d, errDate)
                                                        e.Dispose()
                                                    Else
                                                        rule.Broken = True
                                                    End If
                                                Else
                                                    rule.Broken = True
                                                End If
                                                Rules(i) = rule
                                            End If
                                            If Rules.RemoveAll(Function(rr) Not rr.Exists) > 0 Then updated = True
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    End If

                    If Count > 0 Then Rules.Sort()

                    'download and load the rule
                    If (LoadListOnly And AuthFile.Exists) Or (Not LoadListOnly And ((updated And Count > 0) Or Not AuthFile.Exists)) Then
                        _CurrentRule = Nothing
                        _CurrentContainer.DisposeIfReady
                        _CurrentContainer = Nothing
                        Dim processRule As Func(Of DynamicRulesValue, Boolean, DialogResult) =
                            Function(ByVal __rule As DynamicRulesValue, ByVal reparseAuth As Boolean) As DialogResult
                                Dim fromAuthFile As Boolean = (LoadListOnly Or reparseAuth) AndAlso AuthFile.Exists
                                If fromAuthFile Then
                                    r = AuthFile.GetText(OFError)
                                Else
                                    r = GetWebString(__rule.UrlRaw,, OFError)
                                End If
                                Dim j As EContainer = JsonDocument.Parse(r, OFError)
                                Dim checkResult As Boolean = False
                                Dim isConfig As Boolean = False
                                Dim textToSave As String = r
                                If j.ListExists AndAlso Not GetFormat(j, True, checkResult,, isConfig).IsEmptyString AndAlso checkResult Then
                                    If isConfig Then textToSave = RegexReplace(r, ConfigRulesExtract)
                                    If textToSave.IsEmptyString Then
                                        Return DialogResult.Retry
                                    Else
                                        _CurrentRule = __rule
                                        _CurrentContainer = If(isConfig, j(ConfigNodes), j)
                                        textToSave = ConvertAuthText()
                                        _CurrentContainerRulesText = textToSave
                                        If (Not fromAuthFile Or Not textToSave.StringTrim = r.StringTrim) And Not textToSave.IsEmptyString Then
                                            TextSaver.SaveTextToFile(textToSave, AuthFile, True, False, OFError)
                                            If Not reparseAuth Then processRule(__rule, True)
                                        End If
                                        Return DialogResult.OK
                                    End If
                                End If
                                Return DialogResult.No
                            End Function
                        If Mode = Modes.Personal And Not PersonalRule.IsEmptyString Then
                            processRule(New DynamicRulesValue With {.UrlRepo = PersonalRule, .UrlRaw = PersonalRule}, False)
                        Else
                            For Each rule In Rules
                                If rule.Valid And Not rule.Broken Then
                                    Select Case processRule(rule, False)
                                        Case DialogResult.Retry : Continue For
                                        Case DialogResult.OK : Exit For
                                    End Select
                                End If
                            Next
                        End If
                    End If

                    If updated Or forceSave Then Save()

                    _UpdateInProgress = False
                End If
            Catch ex As Exception
                ErrorsDescriber.Execute(OFError, ex, "[OnlyFans.DynamicRulesEnv.UpdateImpl]")
                _UpdateInProgress = False
            End Try
        End Sub
#End Region
#Region "Add, IndexOf"
        Friend Function Add(ByVal Rule As DynamicRulesValue, Optional ByVal AutoSort As Boolean = True, Optional ByVal AutoSave As Boolean = False) As Integer
            If Rule.Valid Then
                Dim i% = IndexOf(Rule)
                If i = -1 Then
                    Rules.Add(Rule)
                    i = Count - 1
                    If AutoSort Then Rules.Sort() : i = IndexOf(Rule)
                    If AutoSave Then Save()
                End If
                Return i
            Else
                Return -1
            End If
        End Function
        Friend Function RemoveAt(ByVal Index As Integer) As Boolean
            If Index.ValueBetween(0, Count - 1) Then
                Rules.RemoveAt(Index)
                Return True
            Else
                Return False
            End If
        End Function
        Friend Function IndexOf(ByVal URL As String) As Integer
            If Count > 0 Then
                URL = URL.StringToLower.Trim
                Return Rules.FindIndex(Function(r) r.UrlRepo.StringToLower = URL Or r.UrlRaw.StringToLower = URL Or r.UrlLatestCommit.StringToLower = URL)
            Else
                Return -1
            End If
        End Function
#End Region
#Region "ICopier Support"
        Friend Overloads Function Copy() As Object Implements ICopier.Copy
            Return (New DynamicRulesEnv).Copy(Me)
        End Function
        Friend Overloads Function Copy(ByVal Source As Object) As Object Implements ICopier.Copy
            Return Copy(Source, False)
        End Function
        Friend Overloads Function Copy(ByVal Source As Object, ByVal UpdateForceProperty As Boolean) As Object
            If Not Source Is Nothing Then
                With DirectCast(Source, DynamicRulesEnv)
                    If Not RulesForceUpdateRequired And UpdateForceProperty Then _
                       RulesForceUpdateRequired = Not Rules.ListEquals(.Rules) Or Not Mode = .Mode Or
                                                  (.Mode = Modes.Personal And Not PersonalRule = .PersonalRule)
                    ProtectFile = .ProtectFile
                    Mode = .Mode
                    UpdateInterval = .UpdateInterval
                    PersonalRule = .PersonalRule
                    If Not RulesForceUpdateRequired Then RulesForceUpdateRequired = .RulesForceUpdateRequired
                    RulesUpdateConst = .RulesUpdateConst
                    RulesReplaceConfig = .RulesReplaceConfig
                    AddErrorsToLog = .AddErrorsToLog
                    ConfigAutoUpdate = .ConfigAutoUpdate
                    RulesConfigManualMode = .RulesConfigManualMode
                    Rules.Clear()
                    If .Count > 0 Then Rules.AddRange(.Rules)
                End With
                Return Me
            Else
                Return Nothing
            End If
        End Function
#End Region
#Region "IEnumerable Support"
        Private Function GetEnumerator() As IEnumerator(Of DynamicRulesValue) Implements IEnumerable(Of DynamicRulesValue).GetEnumerator
            Return New MyEnumerator(Of DynamicRulesValue)(Me)
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
                    Rules.Clear()
                    _CurrentContainer.DisposeIfReady
                    Responser.DisposeIfReady
                End If
                _CurrentContainer = Nothing
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