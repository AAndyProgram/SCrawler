' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Tools
Friend Class UserImage : Inherits ImageRenderer
    Friend Const ImagePrefix As String = "UserPicture"
    Friend Const ImagePostfix_Large As String = "_Large"
    Friend Const ImagePostfix_Small As String = "_Small"
    Private _LargeAddress As SFile
    Private _SmallAddress As SFile
    Private _ForceSaveOrig As Boolean = False
    Friend Shared Function NewUserPicture(ByVal ImageOrig As SFile, ByVal Destination As SFile,
                                          Optional ByVal Save As Boolean = True, Optional ByVal GetInstance As Boolean = False) As UserImage
        Dim uImg As New UserImage(ImageOrig, Destination)
        With uImg
            ._ForceSaveOrig = ImageOrig.Extension.IsEmptyString OrElse ImageOrig.Extension.ToLower = "gif" OrElse Not {"jpg", "jpeg", "png"}.Contains(ImageOrig.Extension.ToLower)
            If Not ._ForceSaveOrig Then
                If .Address.Exists AndAlso Not .Address.Delete(SFO.File,, EDP.ReturnValue) Then ._ForceSaveOrig = True
                If Not ._ForceSaveOrig AndAlso Not ImageOrig.Copy(.Address) Then ._ForceSaveOrig = True
            End If
            If Not ._ForceSaveOrig Then
                ._SmallAddress.Extension = .Address.Extension
                ._LargeAddress.Extension = .Address.Extension
            End If
            If Save Then .Save()
        End With
        If Not GetInstance Then uImg.Dispose() : uImg = Nothing
        Return uImg
    End Function
    Friend Sub New(ByVal _ImgOriginal As SFile, ByVal Destination As SFile, Optional ByVal GenerateLargeSmallPictures As Boolean = True)
        MyBase.New(_ImgOriginal)
        Dim f As SFile = Destination
        f.Path = f.PathWithSeparator & "Pictures"
        f.Name = ImagePrefix
        f.Extension = "jpg"
        Address = f
        _LargeAddress = Address
        _LargeAddress.Name &= ImagePostfix_Large
        _SmallAddress = Address
        _SmallAddress.Name &= ImagePostfix_Small
        If GenerateLargeSmallPictures Then
            GetImage(Settings.MaxSmallImageHeight.Value, True)
            GetImage(Settings.MaxLargeImageHeight.Value, False)
        End If
    End Sub
    Friend Sub New(ByVal _ImgOriginal As SFile, ByVal _ImgLarge As SFile, ByVal _ImgSmall As SFile, ByVal Destination As SFile)
        Me.New(_ImgOriginal, Destination, False)
        Dim i As New ImageRenderer(_ImgLarge)
        ResizedImages.Add(i.Size, i)
        i = New ImageRenderer(_ImgSmall)
        ResizedImages.Add(i.Size, i)
        _LargeAddress = _ImgLarge
        _SmallAddress = _ImgSmall
    End Sub
    ''' <inheritdoc cref="GetImage(Integer, Boolean)"/>
    Friend ReadOnly Property Small As ImageRenderer
        Get
            Return GetImage(Settings.MaxSmallImageHeight.Value, True)
        End Get
    End Property
    ''' <inheritdoc cref="GetImage(Integer, Boolean)"/>
    Friend ReadOnly Property Large As ImageRenderer
        Get
            Return GetImage(Settings.MaxLargeImageHeight.Value, False)
        End Get
    End Property
    ''' <exception cref="ArgumentNullException"></exception>
    ''' <exception cref="ArgumentOutOfRangeException"></exception>
    Private Shadows Function GetImage(ByVal h As Integer, ByVal IsSmall As Boolean) As ImageRenderer
        With ResizedImages
            If .Count > 0 Then
                Dim ki% = .Keys.ToList.FindIndex(Function(s) s.Height = h)
                If ki >= 0 Then Return .Item(.Keys(ki))
            End If
            FitToHeight(h)
            If .Count = 0 Then
                Throw New ArgumentOutOfRangeException("ResizedImages", "No resized images found")
            Else
                With .Item(.Keys(.Keys.Count - 1))
                    .Address = IIf(IsSmall, _SmallAddress, _LargeAddress)
                    .Save()
                End With
                Return .Item(.Keys(.Keys.Count - 1))
            End If
        End With
    End Function
    Public Overrides Sub Save()
        If _ForceSaveOrig Then MyBase.Save()
        Small.Save(_SmallAddress)
        Large.Save(_LargeAddress)
    End Sub
End Class