﻿#Region "Microsoft.VisualBasic::b283d05b5c493782cbba4f96295f082c, sciBASIC#\Data_science\Visualization\Plots\g\HeatMapPlot.vb"

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

    '   Total Lines: 45
    '    Code Lines: 37
    ' Comment Lines: 0
    '   Blank Lines: 8
    '     File Size: 1.50 KB


    '     Class HeatMapPlot
    ' 
    '         Properties: mapLevels
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: GetBrushes, GetColors, Z
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.Distributions

Namespace Graphic

    Public MustInherit Class HeatMapPlot : Inherits Plot

        Public Property mapLevels As Integer

        Protected Sub New(theme As Theme)
            MyBase.New(theme)
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Protected Function GetBrushes() As SolidBrush()
            Return Designer.GetBrushes(theme.colorSet, n:=mapLevels)
        End Function

        Protected Function GetColors() As Color()
            Return Designer.GetColors(theme.colorSet, n:=mapLevels)
        End Function

        Public Shared Iterator Function Z(scatter As IEnumerable(Of PixelData)) As IEnumerable(Of PixelData)
            Dim allSpots As PixelData() = scatter.ToArray
            Dim v As Double() = allSpots _
                .Select(Function(p) p.Scale) _
                .AsVector _
                .Z _
                .ToArray

            For i As Integer = 0 To v.Length - 1
                Yield New PixelData With {
                    .Scale = v(i),
                    .X = allSpots(i).X,
                    .Y = allSpots(i).Y
                }
            Next
        End Function
    End Class
End Namespace
