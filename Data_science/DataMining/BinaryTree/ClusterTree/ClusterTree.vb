﻿#Region "Microsoft.VisualBasic::12a499948b960c10c7059dbec582e36c, sciBASIC#\Data_science\DataMining\BinaryTree\ClusterTree\ClusterTree.vb"

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

'   Total Lines: 80
'    Code Lines: 50
' Comment Lines: 19
'   Blank Lines: 11
'     File Size: 2.73 KB


' Class ClusterTree
' 
'     Properties: Members
' 
'     Function: GetClusters
' 
'     Sub: (+2 Overloads) Add, populateNodes
' 
' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.GraphTheory

''' <summary>
''' just provides the reference id of the peaks ms2 object
''' </summary>
''' <remarks>
''' implements the Molecule Networking via the tree clustering operation in mzkit
''' </remarks>
Public Class ClusterTree : Inherits Tree(Of String)

    ''' <summary>
    ''' This property includes all data in current cluster tree node, 
    ''' also includes the <see cref="Data"/> member.
    ''' </summary>
    ''' <returns></returns>
    Public Property Members As New List(Of String)

    ''' <summary>
    ''' build tree
    ''' </summary>
    ''' <param name="tree"></param>
    ''' <param name="target"></param>
    ''' <param name="alignment"></param>
    ''' <param name="threshold">
    ''' the cutoff value for set current element 
    ''' <paramref name="target"/> as the member of
    ''' current node <paramref name="tree"/>.
    ''' </param>
    Public Overloads Shared Sub Add(tree As ClusterTree,
                                    target As String,
                                    alignment As ComparisonProvider,
                                    threshold As Double,
                                    Optional ds As Double = 0.05)

        If tree.Data.StringEmpty Then
            tree.Data = target
            tree.Childs = New Dictionary(Of String, Tree(Of String))
            tree.Members = New List(Of String) From {target}
        Else
            Dim score As Double = alignment.GetSimilarity(tree.Data, target)
            Dim key As String = "zero"

            If score > 0.0 Then
                For v As Double = ds To 1 Step ds
                    If score < v Then
                        key = $"<{v.ToString("F1")}"
                        Exit For
                    ElseIf v >= threshold Then
                        key = ""
                        Exit For
                    End If
                Next
            End If

            If key = "" Then
                ' is cluster member
                tree.Members.Add(target)
            ElseIf tree.Childs.ContainsKey(key) Then
                Call Add(tree(key), target, alignment, threshold)
            Else
                Call tree.Add(key)
                Call Add(tree(key), target, alignment, threshold)
            End If
        End If
    End Sub

    Public Shared Function GetClusters(root As ClusterTree) As IEnumerable(Of ClusterTree)
        Dim links As New List(Of ClusterTree)
        Call populateNodes(root, links)
        Return links
    End Function

    Private Shared Sub populateNodes(root As ClusterTree, ByRef links As List(Of ClusterTree))
        Call links.Add(root)

        If Not root.Childs.IsNullOrEmpty Then
            For Each child As Tree(Of String) In root.Childs.Values
                Call populateNodes(DirectCast(child, ClusterTree), links)
            Next
        End If
    End Sub

    ''' <summary>
    ''' add a new child node
    ''' </summary>
    ''' <param name="label"></param>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Private Overloads Sub Add(label As String)
        Call Add(New ClusterTree With {.label = label})
    End Sub

End Class
