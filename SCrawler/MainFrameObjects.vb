' Copyright (C) 2022  Andy
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports SCrawler.API
Imports SCrawler.API.Base
Friend Class MainFrameObjects
    Private ReadOnly Property MF As MainFrame
    Friend Sub New(ByRef f As MainFrame)
        MF = f
    End Sub
    Friend Sub ImageHandler(ByVal User As IUserData)
        ImageHandler(User, False)
        ImageHandler(User, True)
    End Sub
    Friend Sub ImageHandler(ByVal User As IUserData, ByVal Add As Boolean)
        Try
            If Add Then
                AddHandler User.OnUserUpdated, AddressOf MF.User_OnUserUpdated
            Else
                RemoveHandler User.OnUserUpdated, AddressOf MF.User_OnUserUpdated
            End If
        Catch ex As Exception
        End Try
    End Sub
    Friend Sub CollectionHandler(ByVal [Collection] As UserDataBind)
        Try
            AddHandler Collection.OnCollectionSelfRemoved, AddressOf MF.CollectionRemoved
            AddHandler Collection.OnUserRemoved, AddressOf MF.UserRemovedFromCollection
        Catch ex As Exception
        End Try
    End Sub
End Class