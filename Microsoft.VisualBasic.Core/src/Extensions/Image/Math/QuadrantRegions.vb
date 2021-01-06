﻿#Region "Microsoft.VisualBasic::7463f59d14794f842f0e5e46595536c6, Microsoft.VisualBasic.Core\src\Extensions\Image\Math\QuadrantRegions.vb"

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

    '     Enum QuadrantRegions
    ' 
    '         LeftBottom, LeftTop, RightBottom, RightTop, XLeft
    '         XRight, YBottom, YTop
    ' 
    '  
    ' 
    ' 
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Imaging.Math2D

    ''' <summary>
    ''' 请注意，视图上面的象限的位置和计算机之中的象限是反过来的
    ''' </summary>
    Public Enum QuadrantRegions

        ''' <summary>
        ''' 重叠在一起
        ''' </summary>
        Origin = 0

        ''' <summary>
        ''' quadrant 1 = 0,90 ~ -90,0 ~ 270,360
        ''' </summary>
        RightTop
        YTop
        ''' <summary>
        ''' quadrant 2 = 90,180 ~ -180,-90 ~ 180,270
        ''' </summary>
        LeftTop
        XLeft
        ''' <summary>
        ''' quadrant 3 = 180,270 ~ -270,-180 ~ 90,180 
        ''' </summary>
        LeftBottom
        YBottom
        ''' <summary>
        ''' quadrant 4 = 270,360 ~ -270, -360 ~ 0, 90
        ''' </summary>
        RightBottom
        XRight
    End Enum
End Namespace
