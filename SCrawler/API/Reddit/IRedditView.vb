' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Namespace API.Reddit
    Friend Interface IRedditView
        Enum View As Integer
            [New] = 0
            Hot = 1
            Top = 2
        End Enum
        Enum Period As Integer
            All = 0
            Hour = 1
            Day = 2
            Week = 3
            Month = 4
            Year = 5
        End Enum
        Property ViewMode As View
        Property ViewPeriod As Period
        Property RedGifsAccount As String
        Property RedditAccount As String
        Sub SetView(ByVal Options As IRedditView)
    End Interface
    Friend Class RedditViewExchange : Implements IRedditView
        Friend Const Name_ViewMode As String = "ViewMode"
        Friend Const Name_ViewPeriod As String = "ViewPeriod"
        Friend Const Name_RedGifsAccount As String = "RedGifsAccount"
        Friend Const Name_RedditAccount As String = "RedditAccount"
        Friend Property ViewMode As IRedditView.View Implements IRedditView.ViewMode
        Friend Property ViewPeriod As IRedditView.Period Implements IRedditView.ViewPeriod
        Friend Property RedGifsAccount As String Implements IRedditView.RedGifsAccount
        Friend Property RedditAccount As String Implements IRedditView.RedditAccount
        Friend Sub SetView(ByVal Options As IRedditView) Implements IRedditView.SetView
            If Not Options Is Nothing Then
                ViewMode = Options.ViewMode
                ViewPeriod = Options.ViewPeriod
                RedGifsAccount = Options.RedGifsAccount
                RedditAccount = Options.RedditAccount
            End If
        End Sub
    End Class
End Namespace