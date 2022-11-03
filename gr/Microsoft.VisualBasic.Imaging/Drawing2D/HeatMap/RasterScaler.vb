﻿#Region "Microsoft.VisualBasic::6e50cdd4a72f1acc460812513a74d054, sciBASIC#\gr\Microsoft.VisualBasic.Imaging\Drawing2D\HeatMap\RasterScaler.vb"

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

    '   Total Lines: 74
    '    Code Lines: 43
    ' Comment Lines: 19
    '   Blank Lines: 12
    '     File Size: 2.76 KB


    '     Class RasterScaler
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Sub: (+2 Overloads) Dispose, Scale, ScaleTo
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Drawing
Imports System.Drawing.Imaging
Imports Microsoft.VisualBasic.Imaging.BitmapImage
Imports Microsoft.VisualBasic.Imaging.Drawing2D.HeatMap.hqx

Namespace Drawing2D.HeatMap

    ''' <summary>
    ''' do image size scaling
    ''' </summary>
    Public Class RasterScaler : Implements IDisposable

        Dim disposedValue As Boolean
        Dim buffer As BitmapBuffer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="heatmap">
        ''' a heatmap imaging result which is generated by the <see cref="PixelRender"/>
        ''' </param>
        Sub New(heatmap As Bitmap)
            buffer = BitmapBuffer.FromBitmap(heatmap, ImageLockMode.ReadOnly)
        End Sub

        Shared Sub New()
            Call RgbYuv.hqxInit()
        End Sub

        ''' <summary>
        ''' scale image size via rectangle drawing
        ''' </summary>
        ''' <param name="g"></param>
        ''' <param name="region"></param>
        Public Sub ScaleTo(g As IGraphics, region As Rectangle)
            Call Scale(g, region.Size, region.Location)
        End Sub

        Public Function Scale(newWidth As Integer, newHeight As Integer) As Bitmap
            Dim canvas As IGraphics = New Size(newWidth, newHeight).CreateGDIDevice(filled:=Color.Transparent)

            Call Scale(canvas, New Size(newWidth, newHeight))
            Call canvas.Flush()

            Return DirectCast(canvas, Graphics2D).ImageResource
        End Function

        ''' <summary>
        ''' scale image size via rectangle drawing
        ''' </summary>
        ''' <param name="g"></param>
        ''' <param name="newSize"></param>
        ''' <param name="offset"></param>
        Public Sub Scale(g As IGraphics, newSize As Size, Optional offset As Point = Nothing)
            Dim width As Single = newSize.Width / buffer.Width
            Dim height As Single = newSize.Height / buffer.Height
            Dim cellSize As New SizeF(width, height)
            Dim color As SolidBrush
            Dim c As Color

            For x As Integer = 0 To buffer.Width - 1
                For y As Integer = 0 To buffer.Height - 1
                    c = buffer.GetPixel(x, y)

                    If Not c.IsTransparent Then
                        color = New SolidBrush(c)
                        g.FillRectangle(color, x * width + offset.X, y * width + offset.Y, cellSize)
                    End If
                Next
            Next
        End Sub

        Public Function Scale(hqx As HqxScales) As Bitmap
            If hqx = HqxScales.None Then
                ' no scale
                ' returns the raw image directly
                Return buffer.GetImage
            Else
                Dim scales As New Bitmap(buffer.Width * hqx, buffer.Height * hqx, format:=PixelFormat.Format32bppArgb)
                Dim p As BitmapBuffer = BitmapBuffer.FromBitmap(scales)
                ' get source data
                Dim sp As UInteger() = buffer.GetARGBStream
                Dim dp As UInteger() = p.GetARGBStream

                Select Case hqx
                    Case HqxScales.Hqx_2x : Call Hqx_2x.hq2x_32_rb(sp, dp, buffer.Width, buffer.Height)
                    Case HqxScales.Hqx_3x : Call Hqx_3x.hq3x_32_rb(sp, dp, buffer.Width, buffer.Height)
                    Case HqxScales.Hqx_4x : Call Hqx_4x.hq4x_32_rb(sp, dp, buffer.Width, buffer.Height)
                    Case Else
                        Throw New InvalidProgramException($"invalid scale name: {hqx.ToString}!")
                End Select

                Call p.WriteARGBStream(dp)
                Call p.Dispose()

                Erase sp
                Erase dp

                Return scales
            End If
        End Function

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects)
                    Call buffer.Dispose()
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
                ' TODO: set large fields to null
                disposedValue = True
            End If
        End Sub

        ' ' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
        ' Protected Overrides Sub Finalize()
        '     ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        '     Dispose(disposing:=False)
        '     MyBase.Finalize()
        ' End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub
    End Class
End Namespace
