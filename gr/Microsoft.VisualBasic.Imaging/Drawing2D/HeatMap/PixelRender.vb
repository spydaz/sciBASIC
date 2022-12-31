﻿#Region "Microsoft.VisualBasic::64a9cafe5b1cdcc61e56df6500e96c6e, sciBASIC#\gr\Microsoft.VisualBasic.Imaging\Drawing2D\HeatMap\PixelRender.vb"

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

'   Total Lines: 111
'    Code Lines: 78
' Comment Lines: 16
'   Blank Lines: 17
'     File Size: 4.18 KB


'     Class PixelRender
' 
'         Constructor: (+1 Overloads) Sub New
'         Function: RenderRasterImage, ScalePixels
' 
' 
' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Drawing.Imaging
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Imaging.BitmapImage
Imports Microsoft.VisualBasic.Imaging.Drawing2D.Colors

Namespace Drawing2D.HeatMap

    Public Class PixelRender

        ReadOnly colors As Color()
        ReadOnly indexRange As DoubleRange
        ReadOnly defaultColor As Color

        Sub New(colorSet As String, mapLevels As Integer, Optional defaultColor As Color? = Nothing)
            colors = Designer.GetColors(colorSet, mapLevels)
            indexRange = New Double() {0, mapLevels - 1}

            If defaultColor Is Nothing Then
                Me.defaultColor = colors.First
            Else
                Me.defaultColor = defaultColor
            End If
        End Sub

        Public Function ScalePixels(allPixels As Pixel()) As IEnumerable(Of Pixel)
            Return ScalePixels(allPixels, indexRange)
        End Function

        ''' <summary>
        ''' scale raw data into <see cref="indexRange"/> for get 
        ''' corresponding color data.
        ''' </summary>
        ''' <returns></returns>
        Public Shared Iterator Function ScalePixels(allPixels As Pixel(), indexRange As DoubleRange) As IEnumerable(Of Pixel)
            Dim range As DoubleRange = allPixels _
                .Select(Function(p) p.Scale) _
                .ToArray
            Dim newPixel As Pixel

            For Each pixel As Pixel In allPixels
                newPixel = New PixelData With {
                    .Scale = range.ScaleMapping(pixel.Scale, indexRange),
                    .X = pixel.X,
                    .Y = pixel.Y
                }

                Yield newPixel
            Next
        End Function

        ''' <summary>
        ''' rendering pixels with no size scaling.
        ''' </summary>
        ''' <param name="pixels"></param>
        ''' <param name="size"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' do heatmap rendering based on the <see cref="HeatMapRaster"/>
        ''' </remarks>
        Public Function RenderRasterImage(Of T As Pixel)(pixels As IEnumerable(Of T),
                                                         size As Size,
                                                         Optional fillRect As Boolean = True) As Bitmap

            Dim raw As New Bitmap(size.Width, size.Height, PixelFormat.Format32bppArgb)
            Dim full As New Rectangle(0, 0, raw.Width, raw.Height)
            Dim g As IGraphics = raw.CreateCanvas2D(directAccess:=True)
            Dim raster As Pixel() = New HeatMapRaster(Of T)() _
                .SetDatas(pixels) _
                .GetRasterPixels _
                .ToArray

            Call g.Clear(defaultColor)

            ' 20220525 set pixels is not working on the
            ' linux server platform
            '
            If fillRect Then
                Call FillRectangles(
                    g:=g,
                    raster:=raster,
                    cw:=1,
                    ch:=1,
                    colors:=colors,
                    defaultColor:=defaultColor
                )
            Else
                Call SetPixels(raw, raster)
            End If

            Return raw
        End Function

        Private Sub SetPixels(raw As Bitmap, raster As Pixel())
            Using buffer As BitmapBuffer = BitmapBuffer.FromBitmap(raw, ImageLockMode.WriteOnly)
                For Each point As Pixel In ScalePixels(raster)
                    Dim level = CInt(point.Scale)
                    Dim color As Color

                    If level <= 0.0 Then
                        color = defaultColor
                    Else
                        color = colors(level)
                    End If

                    ' imzXML里面的坐标是从1开始的
                    ' 需要减一转换为.NET中从零开始的位置
                    Call buffer.SetPixel(point.X - 1, point.Y - 1, color)
                Next
            End Using
        End Sub

        Public Shared Sub FillRectangles(Of T As Pixel)(g As IGraphics,
                                                        raster As T(),
                                                        colors As Color(),
                                                        defaultColor As Color,
                                                        cw As Double,
                                                        ch As Double)
            Dim solids As SolidBrush() = colors _
                .Select(Function(c) New SolidBrush(c)) _
                .ToArray
            Dim paint As SolidBrush
            Dim defaultPaint As New SolidBrush(defaultColor)
            Dim indexRange As New DoubleRange(0, solids.Length - 1)

            For Each point As Pixel In ScalePixels(raster, indexRange)
                Dim level = CInt(point.Scale)
                Dim pixel As RectangleF

                If level <= 0.0 Then
                    paint = defaultPaint
                Else
                    paint = solids(level)
                End If

                pixel = New RectangleF(point.X, point.Y, cw, ch)
                g.FillRectangle(paint, pixel)
            Next
        End Sub
    End Class
End Namespace
