﻿#Region "Microsoft.VisualBasic::6d4303d7d23e16dd25c4bc9be5d14421, sciBASIC#\gr\network-visualization\Visualizer\Styling\Expression\MapExpression.vb"

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

    '   Total Lines: 48
    '    Code Lines: 30
    ' Comment Lines: 12
    '   Blank Lines: 6
    '     File Size: 1.53 KB


    '     Enum MapperTypes
    ' 
    '         Continuous, Discrete, Passthrough
    ' 
    '  
    ' 
    ' 
    ' 
    '     Structure MapExpression
    ' 
    '         Properties: AsDictionary
    ' 
    '         Function: ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Styling

    Public Structure MapExpression

        Dim propertyName As String
        Dim type As MapperTypes
        Dim values As String()

        Public ReadOnly Property AsDictionary As Dictionary(Of String, String)
            <MethodImpl(MethodImplOptions.AggressiveInlining)>
            Get
                Return values _
                    .Select(Function(s) s.GetTagValue("=", trim:=True)) _
                    .ToDictionary(Function(t) t.Name,
                                  Function(t) t.Value)
            End Get
        End Property

        Public Overrides Function ToString() As String
            If type = MapperTypes.Continuous Then
                Return $"Dim '{propertyName}' = [{values.JoinBy(", ")}]"
            Else
                Return $"Dim '{propertyName}' = {Me.AsDictionary.GetJson}"
            End If
        End Function
    End Structure
End Namespace
