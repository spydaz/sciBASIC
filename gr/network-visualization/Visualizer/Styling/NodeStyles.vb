﻿#Region "Microsoft.VisualBasic::62c872f71e92e09406128ffcce24fe8d, gr\network-visualization\Visualizer\Styling\NodeStyles.vb"

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

    '     Module NodeStyles
    ' 
    '         Function: (+2 Overloads) DegreeAsSize
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel
Imports Microsoft.VisualBasic.ComponentModel.Ranges
Imports Microsoft.VisualBasic.ComponentModel.Ranges.Model
Imports Microsoft.VisualBasic.Data.visualize.Network.FileStream.Generic
Imports Microsoft.VisualBasic.Data.visualize.Network.Graph

Namespace Styling

    Public Module NodeStyles

        <Extension> Public Function DegreeAsSize(nodes As IEnumerable(Of Node),
                                                 getDegree As Func(Of Node, Double),
                                                 sizeRange As DoubleRange) As Map(Of Node, Double)()
            Return nodes.RangeTransform(getDegree, sizeRange)
        End Function

        <Extension>
        Public Function DegreeAsSize(nodes As IEnumerable(Of Node), sizeRange As DoubleRange, Optional degree$ = NamesOf.REFLECTION_ID_MAPPING_DEGREE) As Map(Of Node, Double)()
            Dim valDegree = Function(node As Node)
                                Return node.data(degree).ParseDouble
                            End Function
            Return nodes.DegreeAsSize(
                getDegree:=valDegree,
                sizeRange:=sizeRange
            )
        End Function
    End Module
End Namespace
