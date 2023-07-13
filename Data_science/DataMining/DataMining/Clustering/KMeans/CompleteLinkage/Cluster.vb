﻿#Region "Microsoft.VisualBasic::a8eea3aa3fa7f789a22257b2a1726788, sciBASIC#\Data_science\DataMining\DataMining\Clustering\KMeans\CompleteLinkage\Cluster.vb"

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

'   Total Lines: 26
'    Code Lines: 19
' Comment Lines: 0
'   Blank Lines: 7
'     File Size: 646 B


'     Class Cluster
' 
'         Constructor: (+3 Overloads) Sub New
'         Sub: Add
' 
' 
' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.DataMining.ComponentModel
Imports Microsoft.VisualBasic.Math.Correlations
Imports stdNum = System.Math

Namespace KMeans.CompleteLinkage

    ''' <summary>
    ''' A collection of the target entity object will be a cluster
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    Public Class Cluster(Of T As EntityBase(Of Double))

        Protected Friend ReadOnly m_innerList As New List(Of T)

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub New(points As List(Of T))
            m_innerList = New List(Of T)(points)
        End Sub

        Public Sub New()
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Sub New(p As T)
            Call m_innerList.Add(p)
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overridable Sub Add(p As T)
            Call m_innerList.Add(p)
        End Sub

        Public Function CompleteLinkageDistance(c2 As Cluster(Of T)) As Double
            Dim c1 = Me
            Dim points1 As List(Of T) = c1.m_innerList
            Dim points2 As List(Of T) = c2.m_innerList
            Dim numPointsInC1 As Integer = points1.Count
            Dim numPointsInC2 As Integer = points2.Count
            Dim maxDistance As Double = Double.MinValue
            Dim dist As Double

            For i1 As Integer = 0 To numPointsInC1 - 1
                For i2 As Integer = 0 To numPointsInC2 - 1
                    dist = points1(i1).entityVector.EuclideanDistance(points2(i2).entityVector)
                    maxDistance = stdNum.Max(dist, maxDistance)
                Next
            Next

            Return maxDistance
        End Function

        ''' <summary>
        ''' merge two cluster to populate a new cluster
        ''' </summary>
        ''' <param name="c1"></param>
        ''' <param name="c2"></param>
        ''' <returns></returns>
        Public Shared Operator +(c1 As Cluster(Of T), c2 As Cluster(Of T)) As Cluster(Of T)
            Dim mergedCluster As New Cluster(Of T)
            Dim pointsC1 As List(Of T) = c1.m_innerList

            ' due to the reason of add method is overridable
            ' do we can not use the inner list to add directly
            For i As Integer = 0 To pointsC1.Count - 1
                mergedCluster.Add(pointsC1(i))
            Next

            Dim pointsC2 As List(Of T) = c2.m_innerList

            For i As Integer = 0 To pointsC2.Count - 1
                mergedCluster.Add(pointsC2(i))
            Next

            Return mergedCluster
        End Operator
    End Class
End Namespace
