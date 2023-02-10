﻿#Region "Microsoft.VisualBasic::3fa8d1c59c01ec3122b9420ff7ce4c46, sciBASIC#\Data_science\Visualization\Plots-statistics\Heatmap\BubbleHeatmap.vb"

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

    '   Total Lines: 52
    '    Code Lines: 39
    ' Comment Lines: 5
    '   Blank Lines: 8
    '     File Size: 2.10 KB


    '     Module BubbleHeatmap
    ' 
    '         Function: Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.csv.IO
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.Scripting.Runtime
Imports stdNum = System.Math

Namespace Heatmap

    ''' <summary>
    ''' 普通的热图是整体进行比对，使用填充在方格内的颜色来区分数值
    ''' 而泡泡热图，则是进行单个行数据在样本间的比较，行之间可以使用不同的颜色区分，数值使用泡泡的半径大小来表示
    ''' </summary>
    Public Module BubbleHeatmap

        <Extension>
        Public Function Plot(data As IEnumerable(Of DataSet),
                             Optional size$ = "300,2700",
                             Optional bg$ = "white",
                             Optional margin$ = g.DefaultLargerPadding,
                             Optional colors$ = DesignerTerms.GoogleMaterialPalette,
                             Optional minRadius! = 1,
                             Optional scaleMethod As DrawElements = DrawElements.Rows) As GraphicsData

            Throw New NotImplementedException

            Dim dataMatrix = data.ToArray
            Dim columnNames$() = dataMatrix.PropertyNames
            Dim nrows = dataMatrix.Length
            Dim ncols = dataMatrix(Scan0).Properties.Count

            Dim plotInternal =
                Sub(ByRef g As IGraphics, region As GraphicsRegion)
                    Dim plotRegion As Rectangle = region.PlotRegion
                    ' 应该是正方形的
                    Dim maxRadius = stdNum.Min(plotRegion.Width / ncols, plotRegion.Height / nrows)

                End Sub

            Return g.GraphicsPlots(
                size:=size.SizeParser,
                padding:=margin,
                bg:=bg,
                plotAPI:=plotInternal
            )
        End Function
    End Module
End Namespace
