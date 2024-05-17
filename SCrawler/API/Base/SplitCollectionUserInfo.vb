' Copyright (C) Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.Base
    Friend Structure SplitCollectionUserInfo
        Friend UserOrig As UserInfo
        Friend UserNew As UserInfo
        Friend Changed As Boolean
        Friend ReadOnly Property SameDrive As Boolean
            Get
                Return GetUserDrive(UserOrig) = GetUserDrive(UserNew)
            End Get
        End Property
        Private Shared Function GetUserDrive(ByVal User As UserInfo) As String
            Dim u As UserInfo = User
            If u.File.IsEmptyString Then u.UpdateUserFile()
            Return u.File.Segments.FirstOrDefault.StringToLower
        End Function
        Public Overrides Function ToString() As String
            Return $"[{UserOrig.File.CutPath.PathWithSeparator}] -> [{UserNew.File.CutPath.PathWithSeparator}]"
        End Function
    End Structure
End Namespace