﻿#Region "Microsoft.VisualBasic::2b8b3091006060666c5b62a9ae599b56, Data_science\Visualization\Plots\Scatter\Plot\Scatter2D.vb"

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

'     Class Scatter2D
' 
'         Constructor: (+1 Overloads) Sub New
' 
'         Function: DrawScatter, GetDataScaler
' 
'         Sub: PlotInternal
' 
' 
' /********************************************************************************/

#End Region

Imports System.Drawing
Imports Microsoft.VisualBasic.ComponentModel.Algorithm.base
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Data.ChartPlots
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Axis
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Canvas
Imports Microsoft.VisualBasic.Data.ChartPlots.Graphic.Legend
Imports Microsoft.VisualBasic.Imaging
Imports Microsoft.VisualBasic.Imaging.Drawing2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Math2D
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Math2D.ConvexHull
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Shapes
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.Statistics.Linq

Namespace Plots

    ''' <summary>
    ''' A scatter plot (also called a scatterplot, scatter graph, scatter chart, 
    ''' scattergram, or scatter diagram)[3] is a type of plot or mathematical 
    ''' diagram using Cartesian coordinates to display values for typically two 
    ''' variables for a set of data. If the points are coded (color/shape/size), 
    ''' one additional variable can be displayed. The data are displayed as a 
    ''' collection of points, each having the value of one variable determining 
    ''' the position on the horizontal axis and the value of the other variable 
    ''' determining the position on the vertical axis.[4]
    ''' </summary>
    Public Class Scatter2D : Inherits Plot

        ReadOnly array As SerialData()
        ReadOnly scatterReorder As Boolean = False
        ReadOnly fillPie As Boolean = True
        ReadOnly ablines As Line()
        ReadOnly hullPolygonIndex As Index(Of String)

        Friend xlim As Double = -1
        Friend ylim As Double = -1
        Friend XaxisAbsoluteScalling As Boolean = False
        Friend YaxisAbsoluteScalling As Boolean = False

        Public Sub New(data As IEnumerable(Of SerialData), theme As Theme,
                       Optional scatterReorder As Boolean = False,
                       Optional fillPie As Boolean = True,
                       Optional ablines As Line() = Nothing,
                       Optional hullConvexList As IEnumerable(Of String) = Nothing)

            Call MyBase.New(theme)

            Me.hullPolygonIndex = hullConvexList.SafeQuery.ToArray
            Me.array = data.ToArray
            Me.scatterReorder = scatterReorder
            Me.fillPie = fillPie
            Me.ablines = ablines
        End Sub

        Public Function GetDataScaler(ByRef g As IGraphics, rect As GraphicsRegion) As DataScaler
            Dim XTicks#(), YTicks#()

            XTicks = array.Select(Function(s) s.pts).IteratesALL.Select(Function(p) CDbl(p.pt.X)).ToArray
            YTicks = array.Select(Function(s) s.pts).IteratesALL.Select(Function(p) CDbl(p.pt.Y)).ToArray

            Call Console.WriteLine($"xlim: {xlim}; ylim: {ylim}")

            If (Not xlim.IsNaNImaginary) AndAlso xlim > 0 Then
                XTicks = XTicks.JoinIterates({xlim}).ToArray
            End If
            If (Not ylim.IsNaNImaginary) AndAlso ylim > 0 Then
                YTicks = YTicks.JoinIterates({ylim}).ToArray
            End If
            If XaxisAbsoluteScalling Then
                XTicks = {0.0}.JoinIterates(XTicks).ToArray
            End If
            If YaxisAbsoluteScalling Then
                YTicks = {0.0}.JoinIterates(YTicks).ToArray
            End If

            XTicks = XTicks.Range.CreateAxisTicks
            YTicks = YTicks.Range.CreateAxisTicks

            'If ticksY > 0 Then
            '    YTicks = AxisScalling.GetAxisByTick(YTicks, tick:=ticksY)
            'End If

            Dim canvas As IGraphics = g
            Dim region As Rectangle = rect.PlotRegion
            Dim X As d3js.scale.Scaler
            Dim Y As d3js.scale.LinearScale

            ' 使用手动指定的范围
            ' 手动指定坐标轴值的范围的时候，X坐标轴无法使用term离散映射
            'If Not xaxis.StringEmpty AndAlso Not yaxis.StringEmpty Then
            '    XTicks = AxisProvider.TryParse(xaxis).AxisTicks
            '    YTicks = AxisProvider.TryParse(yaxis).AxisTicks
            '    X = XTicks.LinearScale.range(integers:={region.Left, region.Right})
            '    Y = YTicks.LinearScale.range(integers:={region.Bottom, region.Top})
            'Else
            ' 如果所有数据点都有单词，则X轴使用离散映射
            If array.All(Function(line) line.pts.All(Function(a) Not a.axisLabel.StringEmpty)) Then
                Dim allTermLabels As String() = array _
                    .Select(Function(line)
                                Return line.pts.Select(Function(a) a.axisLabel)
                            End Function) _
                    .IteratesALL _
                    .Distinct _
                    .ToArray

                X = d3js.scale _
                    .ordinal _
                    .domain(allTermLabels) _
                    .range(integers:={region.Left, region.Right})
            Else
                X = d3js.scale _
                    .linear _
                    .domain(XTicks) _
                    .range(integers:={region.Left, region.Right})
            End If

            Y = d3js.scale _
                .linear _
                .domain(YTicks) _
                .range(integers:={region.Bottom, region.Top})

            Return New DataScaler With {
                .X = X,
                .Y = Y,
                .region = region,
                .AxisTicks = (XTicks, YTicks)
            }
        End Function

        Public Shared Iterator Function DrawScatter(g As IGraphics,
                                                    scatter As IEnumerable(Of PointData),
                                                    scaler As DataScaler,
                                                    fillPie As Boolean,
                                                    shape As LegendStyles,
                                                    pointSize As Single,
                                                    getPointBrush As Func(Of PointData, Brush)) As IEnumerable(Of PointF)
            Dim r As Single = pointSize / 2
            Dim d As Single = pointSize
            Dim shapeSize As New Size(d, d)

            For Each pt As PointData In scatter
                Dim pt1 = scaler.Translate(pt)

                Yield pt1

                If fillPie Then
                    Select Case shape
                        Case LegendStyles.Circle
                            pt1 = New PointF(pt1.X - r, pt1.Y - r)
                        Case LegendStyles.Square
                            pt1 = New PointF(pt1.X, pt1.Y - d)
                        Case Else
                            ' do nothing
                    End Select

                    g.DrawLegendShape(pt1, shapeSize, shape, getPointBrush(pt))
                End If

                Call Parallel.DoEvents()
            Next
        End Function

        Protected Overrides Sub PlotInternal(ByRef g As IGraphics, rect As GraphicsRegion)
            Dim scaler As DataScaler = GetDataScaler(g, rect)
            Dim gSize As Size = rect.Size
            Dim canvas As IGraphics = g
            Dim region As Rectangle = rect.PlotRegion

            If theme.drawAxis Then
                Call g.DrawAxis(
                    rect, scaler, theme.drawGrid,
                    xlabel:=xlabel, ylabel:=ylabel,
                    htmlLabel:=theme.htmlLabel,
                    tickFontStyle:=theme.axisTickCSS,
                    labelFont:=theme.axisLabelCSS,
                    xlayout:=theme.xAxisLayout,
                    ylayout:=theme.yAxisLayout,
                    gridX:=theme.gridStrokeX,
                    gridY:=theme.gridStrokeY,
                    gridFill:=theme.gridFill,
                    XtickFormat:=theme.XaxisTickFormat,
                    YtickFormat:=theme.YaxisTickFormat,
                    axisStroke:=theme.axisStroke
                )
            End If

            Dim width As Double = rect.PlotRegion.Width / 200
            Dim annotations As New Dictionary(Of String, (raw As SerialData, line As SerialData))

            For Each line As SerialData In array
                Dim pen As Pen = line.GetPen
                Dim fillBrush As New SolidBrush(Color.FromArgb(100, baseColor:=line.color))
                Dim bottom! = gSize.Height - rect.PlotRegion.Bottom
                Dim scatter As IEnumerable(Of PointData)

                If scatterReorder Then
                    scatter = line.pts.OrderBy(Function(a) a.value)
                Else
                    scatter = line.pts
                End If

                Dim polygon As PointF() = DrawScatter(
                    g, scatter,
                    scaler:=scaler,
                    fillPie:=fillPie,
                    shape:=line.shape,
                    pointSize:=line.pointSize,
                    getPointBrush:=line.BrushHandler
                ) _
                .ToArray

                If line.title Like hullPolygonIndex Then
                    Call polygon _
                        .DoCall(AddressOf ConvexHull.JarvisMatch) _
                        .DoCall(Sub(hull)
                                    If hull.Length >= 3 Then
                                        Call HullPolygonDraw.DrawHullPolygon(canvas, hull, line.color)
                                    End If
                                End Sub)
                End If

                If Not line.DataAnnotations.IsNullOrEmpty Then
                    Dim raw As SerialData = array _
                        .Where(Function(s)
                                   Return s.title = line.title
                               End Function) _
                        .First

                    Call annotations.Add(line.title, (raw, line))
                End If
            Next

            For Each part In annotations.Values
                For Each annotation As Annotation In part.line.DataAnnotations
                    Call annotation.Draw(g, scaler, part.raw, rect)
                Next
            Next

            If theme.drawLegend Then
                Dim legends As LegendObject() = LinqAPI.Exec(Of LegendObject) _
 _
                    () <= From s As SerialData
                          In array
                          Let sColor As String = s.color.RGBExpression
                          Select New LegendObject With {
                                .color = sColor,
                                .fontstyle = theme.legendLabelCSS,
                                .style = s.shape,
                                .title = s.title
                            }

                Call DrawLegends(g, legends, showBorder:=True, canvas:=rect)
            End If

            Call DrawMainTitle(g, region)

            ' draw ablines
            For Each line As Line In ablines.SafeQuery
                Dim a As PointF = scaler.Translate(line.A)
                Dim b As PointF = scaler.Translate(line.B)

                Call g.DrawLine(line.Stroke, a, b)
            Next
        End Sub
    End Class
End Namespace
