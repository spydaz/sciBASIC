﻿#Region "Microsoft.VisualBasic::265f66cd28164f4d96cc73ceb6364c8a, sciBASIC#\Data_science\DataMining\UMAP\KNN\KNearestNeighbour.vb"

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

    '   Total Lines: 112
    '    Code Lines: 65
    ' Comment Lines: 29
    '   Blank Lines: 18
    '     File Size: 5.13 KB


    '     Class KNearestNeighbour
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: FindNeighbors, NearestNeighbors, Round
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.CommandLine.InteropService.Pipeline
Imports Microsoft.VisualBasic.DataMining.UMAP.Tree
Imports Microsoft.VisualBasic.Math
Imports Microsoft.VisualBasic.Math.LinearAlgebra.Matrix
Imports i32 = Microsoft.VisualBasic.Language.i32
Imports stdNum = System.Math

Namespace KNN

    ''' <summary>
    ''' K Nearest Neighbour Search
    ''' 
    ''' Uses a kd-tree to find the p number of near neighbours for each point in an input/output dataset.
    ''' 
    ''' Use the nn2 function from the RANN package, utilizes the Approximate Near Neighbor (ANN) C++ library, 
    ''' which can give the exact near neighbours or (as the name suggests) approximate near neighbours 
    ''' to within a specified error bound. For more information on the ANN library please 
    ''' visit http://www.cs.umd.edu/~mount/ANN/.
    ''' </summary>
    Public Class KNearestNeighbour

        ReadOnly m_k As Integer
        ReadOnly m_distanceFn As DistanceCalculation
        ReadOnly m_random As IProvideRandomValues

        Sub New(knn As Integer, Optional distanceFn As DistanceCalculation = Nothing, Optional random As IProvideRandomValues = Nothing)
            m_k = knn
            m_distanceFn = If(distanceFn, AddressOf DistanceFunctions.Cosine)
            m_random = If(random, DefaultRandomGenerator.Instance)
        End Sub

        ''' <summary>
        ''' Compute the ``nNeighbors`` nearest points for each data point in ``X`` - this may be exact, but more likely is approximated via nearest neighbor descent.
        ''' </summary>
        Friend Function NearestNeighbors(x As Double()(), progressReporter As RunSlavePipeline.SetProgressEventHandler) As KNNState
            Dim metricNNDescent = New NNDescent(m_distanceFn, m_random)

            Call progressReporter(0.05F, "Create NNDescent")

            Dim nTrees = 5 + Round(stdNum.Sqrt(x.Length) / 20)
            Dim nIters = stdNum.Max(5, CInt(stdNum.Floor(stdNum.Round(stdNum.Log(x.Length, 2)))))

            Call progressReporter(0.1F, "Set Iteration Parameters")

            Dim leafSize = stdNum.Max(10, m_k)
            Dim forestProgressReporter = ScaleProgressReporter(progressReporter, 0.1F, 0.4F)
            Dim i As i32 = Scan0
            Dim rpForest = New FlatTree(nTrees - 1) {}

            Call forestProgressReporter(0, $"make {nTrees} trees...")

            For Each node As (i%, Tree.FlatTree) In Enumerable.Range(0, nTrees) _
                .AsParallel _
                .Select(Function(n)
                            ' x is readonly in make tree
                            ' progress can be parallel
                            Return (n, Tree.FlattenTree(Tree.MakeTree(x, leafSize, n, m_random), leafSize))
                        End Function)

                rpForest(node.i) = node.Item2
                forestProgressReporter(CSng(++i) / nTrees, $"[{i}/{nTrees}] MakeTree")
            Next

            Dim leafArray As Integer()() = Tree.MakeLeafArray(rpForest)
            Dim nnDescendProgressReporter As RunSlavePipeline.SetProgressEventHandler = ScaleProgressReporter(progressReporter, 0.5F, 1)

            Call progressReporter(0.45F, "MakeNNDescent")

            ' Handle python3 rounding down from 0.5 discrpancy
            Return metricNNDescent.MakeNNDescent(
                data:=x,
                leafArray:=leafArray,
                nNeighbors:=m_k,
                nIters:=nIters,
                startingIteration:=Sub(n, max, msg)
                                       nnDescendProgressReporter(CSng(n) / max, msg)
                                   End Sub)
        End Function

        ''' <summary>
        ''' Handle python3 rounding down from 0.5 discrpancy
        ''' </summary>
        ''' <param name="n"></param>
        ''' <returns></returns>
        Private Shared Function Round(n As Double) As Integer
            If n = 0.5 Then
                Return 0
            Else
                Return stdNum.Floor(stdNum.Round(n))
            End If
        End Function

        ''' <summary>
        ''' K Nearest Neighbour Search
        ''' </summary>
        ''' <param name="data">matrix; input data matrix</param>
        ''' <param name="k">integer; number of nearest neighbours</param>
        ''' <returns>
        ''' a n-by-k matrix of neighbor indices
        ''' </returns>
        Public Shared Function FindNeighbors(data As NumericMatrix, k As Integer,
                                             Optional distanceFn As DistanceCalculation = Nothing,
                                             Optional random As IProvideRandomValues = Nothing,
                                             Optional report As RunSlavePipeline.SetProgressEventHandler = Nothing) As KNNState
            If report Is Nothing Then
                report = AddressOf RunSlavePipeline.SendProgress
            End If

            Return New KNearestNeighbour(k, distanceFn, random).NearestNeighbors(data.Array, report)
        End Function
    End Class
End Namespace
