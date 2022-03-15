﻿#Region "Microsoft.VisualBasic::7895b78b6d7923a1de231156a2660081, sciBASIC#\Data_science\Mathematica\Math\Math\Algebra\Solvers\SOR.vb"

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

    '   Total Lines: 47
    '    Code Lines: 29
    ' Comment Lines: 9
    '   Blank Lines: 9
    '     File Size: 1.62 KB


    '     Module SOR
    ' 
    '         Function: Solve
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Math.LinearAlgebra.Matrix
Imports stdNum = System.Math

Namespace LinearAlgebra.Solvers

    Public Module SOR

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="A"></param>
        ''' <param name="b"></param>
        ''' <param name="Omiga">松弛因子</param>
        ''' <param name="e">误差容限</param>
        ''' <param name="Iteration">最大允许迭代次数</param>
        ''' <returns></returns>
        Public Function Solve(A As GeneralMatrix, b As Vector, Optional Omiga As Double = 1.2, Optional e As Double = 0.00000001, Optional Iteration As Integer = 50) As Vector
            Dim N As Integer = A.ColumnDimension
            Dim x1 As Vector = New Vector(N), x As Vector = New Vector(N)

            For k As Integer = 0 To Iteration
                For i As Integer = 0 To N - 1
                    Dim sum As Double
                    For j As Integer = 0 To N - 1
                        If j < i Then
                            sum += A(i, j) * x(j)
                        ElseIf j > i Then
                            sum += A(i, j) * x1(j)
                        End If
                    Next

                    x(i) = (b(i) - sum) * Omiga / A(i, i) + (1.0 - Omiga) * x1(i)
                Next

                Dim dx As Vector = x - x1, err As Double = stdNum.Sqrt(dx.Mod)

                If err < e Then
                    Exit For
                End If

                x1 = x
            Next

            Return x
        End Function
    End Module
End Namespace
