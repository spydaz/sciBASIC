﻿#Region "Microsoft.VisualBasic::768f79291df9adec54f8d374bb657d8a, Data_science\Visualization\Plots\g\Plot.vb"

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

    '     Class Plot
    ' 
    '         Properties: legendTitle, main, xlabel, ylabel, zlabel
    ' 
    '         Constructor: (+1 Overloads) Sub New
    ' 
    '         Function: EvaluateLayout, Plot
    ' 
    '         Sub: DrawLegends, DrawMainTitle, (+2 Overloads) Plot
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Driver
Imports Microsoft.VisualBasic.MIME.Html.CSS
Imports Microsoft.VisualBasic.Scripting.Runtime

Namespace Graphic

    ''' <summary>
    ''' the chartting plot framework in sciBASIC 
    ''' </summary>
    Public MustInherit Class Plot

        Protected ReadOnly theme As Theme

        Public Property xlabel As String = "X"
        Public Property ylabel As String = "Y"
        Public Property zlabel As String = "Z"
        Public Property legendTitle As String = "Legend"

        ''' <summary>
        ''' the main title string
        ''' </summary>
        ''' <returns></returns>
        Public Property main As String = Nothing

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Sub New(theme As Theme)
            Me.theme = theme
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Function Plot(Optional size$ = Resolution2K.Size, Optional ppi As Integer = 300, Optional driver As Drivers = Drivers.Default) As GraphicsData
            Return g.GraphicsPlots(
                size:=size.SizeParser,
                padding:=theme.padding,
                bg:=theme.background,
                plotAPI:=AddressOf PlotInternal,
                driver:=driver,
                dpi:=$"{ppi},{ppi}"
            )
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="g"></param>
        ''' <param name="layout">
        ''' 一般而言，这个属性是<see cref="GraphicsRegion.PlotRegion"/>的属性值
        ''' </param>
        ''' 
        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub Plot(ByRef g As IGraphics, layout As Rectangle)
            Call PlotInternal(g, EvaluateLayout(g, layout))
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub Plot(ByRef g As IGraphics, canvas As GraphicsRegion)
            Call PlotInternal(g, canvas)
        End Sub

        Protected Shared Function EvaluateLayout(g As IGraphics, layout As Rectangle) As GraphicsRegion
            Dim padding As New Padding With {
                .Left = layout.Left,
                .Top = layout.Top,
                .Bottom = g.Size.Height - layout.Bottom,
                .Right = g.Size.Width - layout.Right
            }
            Dim canvas As New GraphicsRegion(g.Size, padding)

            Return canvas
        End Function

        Protected MustOverride Sub PlotInternal(ByRef g As IGraphics, canvas As GraphicsRegion)

        ''' <summary>
        ''' custom layout via <see cref="theme.legendLayout"/>
        ''' </summary>
        ''' <param name="g"></param>
        ''' <param name="legends"></param>
        ''' <param name="showBorder"></param>
        ''' <param name="canvas"></param>
        Protected Sub DrawLegends(g As IGraphics, legends As LegendObject(), showBorder As Boolean, canvas As GraphicsRegion)
            Dim legendLabelFont As Font = CSSFont.TryParse(theme.legendLabelCSS).GDIObject(g.Dpi)
            Dim lsize As SizeF = g.MeasureString("A", legendLabelFont)
            Dim legendParts As LegendObject()() = Nothing
            Dim maxWidth!
            Dim legendPos As PointF
            Dim legendSize$
            Dim region As Rectangle = canvas.PlotRegion

            Const ratio As Double = 0.65

            lsize = New SizeF(lsize.Height * ratio, lsize.Height * ratio)
            legendSize = $"{lsize.Width},{lsize.Height}"

            If theme.legendLayout Is Nothing Then
                Dim maxLen = legends.Select(Function(l) l.title).MaxLengthString
                Dim lFont As Font = CSSFont.TryParse(legends.First.fontstyle).GDIObject(g.Dpi)

                maxWidth! = g.MeasureString(maxLen, lFont).Width

                If theme.legendSplitSize > 0 AndAlso legends.Length > theme.legendSplitSize Then
                    legendParts = legends.Split(theme.legendSplitSize)
                    legendPos = New PointF With {
                        .X = region.Width - (lsize.Width + maxWidth + 5) * (legendParts.Length - 1),
                        .Y = region.Top + lFont.Height
                    }
                Else
                    legendPos = New PointF With {
                        .X = region.Size.Width - lsize.Width / 4 - maxWidth,
                        .Y = region.Top + lFont.Height
                    }
                End If
            Else
                legendPos = theme.legendLayout.GetLocation(canvas, Nothing)
            End If

            If legendParts.IsNullOrEmpty Then
                Call g.DrawLegends(
                    legendPos, legends, legendSize,
                    shapeBorder:=theme.legendBoxStroke,
                    regionBorder:=If(showBorder, theme.legendBoxStroke, Nothing),
                    fillBg:=theme.legendBoxBackground
                )
            Else
                For Each part As LegendObject() In legendParts
                    Call g.DrawLegends(
                        legendPos, part, legendSize,
                        shapeBorder:=theme.legendBoxStroke,
                        regionBorder:=If(showBorder, theme.legendBoxStroke, Nothing),
                        fillBg:=theme.legendBoxBackground
                    )

                    legendPos = New Point With {
                        .X = legendPos.X + maxWidth + lsize.Width + 5,
                        .Y = legendPos.Y
                    }
                Next
            End If
        End Sub

        Protected Sub DrawMainTitle(g As IGraphics, plotRegion As Rectangle)
            If Not main.StringEmpty Then
                Dim fontOfTitle As Font = CSSFont.TryParse(theme.mainCSS).GDIObject(g.Dpi)
                Dim titleSize As SizeF = g.MeasureString(main, fontOfTitle)
                Dim position As New PointF With {
                    .X = plotRegion.X + (plotRegion.Width - titleSize.Width) / 2,
                    .Y = plotRegion.Y - titleSize.Height * 1.125
                }
                Dim color As Brush = Brushes.Black

                If Not theme.mainTextColor.StringEmpty Then
                    color = theme.mainTextColor.GetBrush
                End If

                Call g.DrawString(main, fontOfTitle, color, position)
            End If
        End Sub

    End Class
End Namespace
