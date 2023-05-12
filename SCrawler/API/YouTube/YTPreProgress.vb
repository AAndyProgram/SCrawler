' Copyright (C) 2023  Andy https://github.com/AAndyProgram
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
'
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY
Imports PersonalUtilities.Forms.Toolbars
Namespace API.YouTube
    Friend Class YTPreProgress : Inherits MyProgress
        Private ReadOnly AssocProgress As PreProgress
        Friend Sub New(ByRef ExtProgress As PreProgress)
            AssocProgress = ExtProgress
        End Sub
        Public Overrides Property Maximum As Double
            Get
                Return _Maximum
            End Get
            Set(ByVal Max As Double)
                _Maximum = Max
                AssocProgress.ChangeMax(Max, False)
            End Set
        End Property
        Public Overrides Sub Perform(Optional ByVal Value As Double = 1)
            AssocProgress.Perform(Value)
        End Sub
        Public Overrides Sub Done()
            AssocProgress.Done()
        End Sub
        Public Overrides Property Visible(Optional ByVal ProgressBar As Boolean = True, Optional ByVal Label As Boolean = True) As Boolean
            Get
                Return True
            End Get
            Set : End Set
        End Property
    End Class
End Namespace