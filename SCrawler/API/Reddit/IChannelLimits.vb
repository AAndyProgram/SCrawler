' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.Reddit
    Friend Interface IChannelLimits
        Property AutoGetLimits As Boolean
        Property DownloadLimitCount As Integer?
        Property DownloadLimitPost As String
        Property DownloadLimitDate As Date?
        Overloads Sub SetLimit(Optional ByVal Post As String = "", Optional ByVal Count As Integer? = Nothing, Optional ByVal [Date] As Date? = Nothing)
        Overloads Sub SetLimit(ByVal Source As IChannelLimits)
    End Interface
End Namespace