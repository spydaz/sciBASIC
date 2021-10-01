﻿#Region "Microsoft.VisualBasic::bca50246b74ffaebaa2aa8a1bc0df11c, gr\Microsoft.VisualBasic.Imaging\Drawing2D\Math2D\MarchingSquares\MeasureData.vb"

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

    '     Class MeasureData
    ' 
    '         Properties: X, Y, Z
    ' 
    '         Constructor: (+2 Overloads) Sub New
    '         Function: ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Drawing2D.Math2D.MarchingSquares

    ''' <summary>
    ''' 测量数据
    ''' </summary>
    Public Class MeasureData

        ''' <summary>
        ''' 坐标X
        ''' </summary>
        Public Property X As Integer

        ''' <summary>
        ''' 坐标Y
        ''' </summary>
        Public Property Y As Integer

        ''' <summary>
        ''' 高度
        ''' </summary>
        Public Property Z As Double

        Sub New()
        End Sub

        ''' <summary>
        ''' 初始化测量数据
        ''' </summary>
        ''' <param name="x">坐标x</param>
        ''' <param name="y">坐标y</param>
        ''' <param name="z">高度</param>
        Public Sub New(x As Integer, y As Integer, Optional z As Double = 0)
            Me.X = x
            Me.Y = y
            Me.Z = z
        End Sub

        Public Overrides Function ToString() As String
            Return $"[{X}, {Y}] {Z}"
        End Function
    End Class
End Namespace
