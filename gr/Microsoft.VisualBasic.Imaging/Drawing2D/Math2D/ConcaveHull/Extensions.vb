﻿#Region "Microsoft.VisualBasic::cdadca718d96cfde616f0c4466e646a7, sciBASIC#\gr\Microsoft.VisualBasic.Imaging\Drawing2D\Math2D\ConcaveHull\Extensions.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xie (genetics@smrucc.org)
    '       xieguigang (xie.guigang@live.com)
    ' 
    ' Copyright (c) 2018 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
    ' 
    ' 
    ' This program is free software: you can redistribute it and/or modify
    ' it under the terms of the GNU General Public License as published by
    ' the Free Software Foundation, either version 3 of the License, or
    ' (at your option) any later version.
    ' 
    ' This program is distributed in the hope that it will be useful,
    ' but WITHOUT ANY WARRANTY; without even the implied warranty of
    ' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    ' GNU General Public License for more details.
    ' 
    ' You should have received a copy of the GNU General Public License
    ' along with this program. If not, see <http://www.gnu.org/licenses/>.



    ' /********************************************************************************/

    ' Summaries:


    ' Code Statistics:

    '   Total Lines: 18
    '    Code Lines: 15
    ' Comment Lines: 0
    '   Blank Lines: 3
    '     File Size: 544.00 B


    '     Module Extensions
    ' 
    '         Function: ConcaveHull
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.CompilerServices

Namespace Drawing2D.Math2D.ConcaveHull

    <HideModuleName>
    Public Module Extensions

        <Extension> Public Function ConcaveHull(points As IEnumerable(Of PointF), Optional r# = -1) As PointF()
            With New BallConcave(points)
                If r# <= 0 Then
                    r# = .RecomandedRadius
                End If
                Return .GetConcave_Ball(r).ToArray
            End With
        End Function
    End Module
End Namespace
