﻿#Region "Microsoft.VisualBasic::98ebe7e34dc7080d3b163132280877c3, sciBASIC#\Microsoft.VisualBasic.Core\src\ComponentModel\Algorithm\BlockSearchFunction.vb"

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

    '   Total Lines: 160
    '    Code Lines: 121
    ' Comment Lines: 13
    '   Blank Lines: 26
    '     File Size: 5.57 KB


    '     Structure Block
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: GetComparision, ToString
    ' 
    '     Structure SequenceTag
    ' 
    '         Function: ToString
    ' 
    '     Class BlockSearchFunction
    ' 
    '         Properties: Keys
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: getOrderSeq, Search
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq
Imports stdNum = System.Math

Namespace ComponentModel.Algorithm

    Friend Structure Block(Of T)

        Dim min As Double
        Dim max As Double
        Dim block As SequenceTag(Of T)()

        Friend Sub New(tmp As SequenceTag(Of T)())
            block = tmp
            min = block.First.tag
            max = block.Last.tag
        End Sub

        Public Overrides Function ToString() As String
            Return $"[{min} ~ {max}] {block.Length} elements"
        End Function

        Friend Shared Function GetComparision() As Comparison(Of Block(Of T))
            Return Function(source, target)
                       ' target is the input data to search
                       Dim x As Double = target.min

                       If x > source.min AndAlso x < source.max Then
                           Return 0
                       ElseIf source.min < x Then
                           Return -1
                       Else
                           Return 1
                       End If
                   End Function
        End Function

    End Structure

    Friend Structure SequenceTag(Of T)

        Dim i As Integer
        Dim tag As Double
        Dim data As T

        Public Overrides Function ToString() As String
            Return $"[{i}] {tag}"
        End Function

    End Structure

    Public Class BlockSearchFunction(Of T)

        Dim binary As BinarySearchFunction(Of Block(Of T), Block(Of T))
        Dim eval As Func(Of T, Double)
        Dim tolerance As Double

        ''' <summary>
        ''' get all keys which are evaluated from the input object
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Keys As Double()
            Get
                Return binary.rawOrder _
                    .Select(Function(b) b.block) _
                    .IteratesALL _
                    .Select(Function(i) i.tag) _
                    .ToArray
            End Get
        End Property

        Sub New(data As IEnumerable(Of T), eval As Func(Of T, Double), tolerance As Double, Optional factor As Double = 2)
            Dim input = getOrderSeq(data, eval).ToArray
            Dim blocks As New List(Of Block(Of T))
            Dim block As Block(Of T)
            Dim tmp As New List(Of SequenceTag(Of T))
            Dim min As Double = input.First.tag
            Dim max As Double = input.Last.tag
            Dim delta As Double = tolerance * factor
            Dim compares = Algorithm.Block(Of T).GetComparision

            For Each x In input
                If x.tag - min <= delta Then
                    tmp.Add(x)
                ElseIf tmp > 0 Then
                    block = New Block(Of T)(tmp.PopAll)
                    min = x.tag
                    blocks.Add(block)
                    tmp.Add(x)
                End If
            Next

            If tmp > 0 Then
                block = New Block(Of T)(tmp.PopAll)
                blocks.Add(block)
            End If

            Me.tolerance = tolerance
            Me.eval = eval
            Me.binary = New BinarySearchFunction(Of Block(Of T), Block(Of T))(blocks, Function(any) any, compares)
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Private Function getOrderSeq(data As IEnumerable(Of T), eval As Func(Of T, Double)) As IEnumerable(Of SequenceTag(Of T))
            Return data _
                .Select(Function(a) (a, eval(a))) _
                .SeqIterator _
                .OrderBy(Function(a) a.value.Item2) _
                .Select(Function(a)
                            Return New SequenceTag(Of T) With {
                                .i = a.i,
                                .data = a.value.a,
                                .tag = a.value.Item2
                            }
                        End Function)
        End Function

        ''' <summary>
        ''' query data with a given tolerance value
        ''' </summary>
        ''' <param name="x"></param>
        ''' <returns></returns>
        Public Iterator Function Search(x As T, Optional tolerance As Double? = Nothing) As IEnumerable(Of T)
            Dim wrap As New Block(Of T) With {.min = eval(x)}
            Dim i As Integer = binary.BinarySearch(target:=wrap)

            If i = -1 Then
                Return
            ElseIf tolerance Is Nothing Then
                tolerance = Me.tolerance
            End If

            Dim joint As New List(Of SequenceTag(Of T))

            If i = 0 Then
                ' 0+1
                joint.AddRange(binary(0).block)
                joint.AddRange(binary(1).block)
            ElseIf i = binary.size - 1 Then
                ' -2 | -1
                joint.AddRange(binary(-1).block)
                joint.AddRange(binary(-2).block)
            Else
                ' i-1 | i | i+1
                joint.AddRange(binary(i - 1).block)
                joint.AddRange(binary(i).block)
                joint.AddRange(binary(i + 1).block)
            End If

            Dim val As Double = eval(x)

            For Each a As SequenceTag(Of T) In joint
                If stdNum.Abs(a.tag - val) <= tolerance Then
                    Yield a.data
                End If
            Next
        End Function
    End Class
End Namespace
