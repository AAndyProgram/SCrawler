' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Partial Friend Module MainMod
    Friend Structure UserBan
        Friend ReadOnly Name As String
        Friend ReadOnly Reason As String
        Friend ReadOnly Exists As Boolean
        Friend Sub New(ByVal Value As String)
            If Not Value.IsEmptyString Then
                Dim v$() = Value.Split("|")
                If v.ListExists Then
                    Name = v(0)
                    If v.Length > 1 Then Reason = v(1)
                    Exists = True
                End If
            End If
        End Sub
        Friend Sub New(ByVal _Name As String, ByVal _Reason As String)
            Name = _Name
            Reason = _Reason
            Exists = True
        End Sub
        Public Shared Widening Operator CType(ByVal Value As String) As UserBan
            Return New UserBan(Value)
        End Operator
        Public Shared Widening Operator CType(ByVal b As UserBan) As String
            Return b.ToString
        End Operator
        Public Overrides Function ToString() As String
            Return $"{Name}|{Reason}"
        End Function
        Friend Function Info() As String
            If Not Reason.IsEmptyString Then
                Return $"[{Name}] ({Reason})"
            Else
                Return Name
            End If
        End Function
        Public Overrides Function Equals(ByVal Obj As Object) As Boolean
            If Not IsNothing(Obj) Then
                If TypeOf Obj Is UserBan Then
                    Return Name = DirectCast(Obj, UserBan).Name
                Else
                    Return Name = New UserBan(CStr(Obj)).Name
                End If
            End If
            Return False
        End Function
    End Structure
    Friend Function UserBanned(ByVal UserNames() As String) As String()
        If UserNames.ListExists Then
            Dim i%
            Dim Found As New List(Of UserBan)
            For Each user In UserNames
                i = Settings.BlackList.FindIndex(Function(u) u.Name = user)
                If i >= 0 Then Found.Add(Settings.BlackList(i))
            Next
            If Found.Count = 0 Then
                Return New String() {}
            Else
                Dim m As New MMessage With {
                    .Title = "Banned user found",
                    .Buttons = {"Remove from ban and add", "Leave in ban and add", "Skip"},
                    .Style = MsgBoxStyle.Exclamation,
                    .Exists = True
                }
                If Found.Count = 1 Then
                    m.Text = $"This user is banned:{vbNewLine}User: {Found(0).Name}"
                    If Not Found(0).Reason.IsEmptyString Then m.Text.StringAppendLine($"Reason: {Found(0).Reason}")
                Else
                    m.Text = $"These users have been banned:{vbNewLine.StringDup(2)}{Found.Select(Function(u) u.Info).ListToString(vbNewLine)}"
                End If
                Dim r% = MsgBoxE(m)
                If r = 2 Then
                    Return Found.Select(Function(u) u.Name).ToArray
                Else
                    If r = 0 Then
                        Settings.BlackList.ListDisposeRemove(Found)
                        Settings.UpdateBlackList()
                    End If
                End If
            End If
        End If
        Return New String() {}
    End Function
    Friend Function UserBanned(ByVal UserName As String) As Boolean
        Return UserBanned({UserName}).ListExists
    End Function
End Module