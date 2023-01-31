﻿#Region "Microsoft.VisualBasic::f59476bc1647bd45bb9921eab52a197f, sciBASIC#\Data_science\Mathematica\Math\MathLambda\MathMLCompiler.vb"

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

    '   Total Lines: 54
    '    Code Lines: 46
    ' Comment Lines: 3
    '   Blank Lines: 5
    '     File Size: 2.86 KB


    ' Module MathMLCompiler
    ' 
    '     Function: (+2 Overloads) CreateBinary, CreateLambda
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Linq.Expressions
Imports Microsoft.VisualBasic.MIME.application.xml
Imports ML = Microsoft.VisualBasic.MIME.application.xml.MathML.BinaryExpression
Imports MLLambda = Microsoft.VisualBasic.MIME.application.xml.MathML.LambdaExpression
Imports MLSymbol = Microsoft.VisualBasic.MIME.application.xml.MathML.SymbolExpression

''' <summary>
''' mathML -> lambda -> linq expression -> compile VB lambda
''' </summary>
Public Class MathMLCompiler

    Dim symbols As SymbolIndex

    Public Shared Function CreateLambda(lambda As MLLambda) As LambdaExpression
        Dim parameters = SymbolIndex.FromLambda(lambda)
        Dim compiler As New MathMLCompiler With {.symbols = parameters}
        Dim body As Expression = compiler.CastExpression(lambda.lambda)
        Dim expr As LambdaExpression = Expression.Lambda(body, parameters.Alignments)

        Return expr
    End Function

    Private Function CastExpression(lambda As MathML.MathExpression) As Expression
        Select Case lambda.GetType
            Case GetType(MLSymbol) : Return CreateLiteral(lambda)
            Case GetType(MathML.MathFunctionExpression) : Return CreateMathCalls(lambda)
            Case GetType(ML) : Return CreateBinary(TryCast(lambda, ML))
            Case Else
                Throw New NotImplementedException(lambda.GetType.FullName)
        End Select
    End Function

    Private Function CreateMathCalls(func As MathML.MathFunctionExpression) As Expression

    End Function

    Private Function CreateLiteral(symbol As MLSymbol) As Expression
        If symbol.isNumericLiteral Then
            Return Expression.Constant(ParseDouble(symbol.text), GetType(Double))
        Else
            Return symbols(symbol.text)
        End If
    End Function

    Private Function CreateBinary(member As ML) As Expression
        Select Case MathML.ContentBuilder.SimplyOperator(member.operator)
            Case "+" : Return Expression.Add(CastExpression(member.applyleft), CastExpression(member.applyright))
            Case "-"
                If member.applyright Is Nothing Then
                    Return Expression.Negate(CastExpression(member.applyleft))
                Else
                    Return Expression.Subtract(
                        CastExpression(member.applyleft),
                        CastExpression(member.applyright)
                    )
                End If
            Case "*" : Return Expression.Multiply(CastExpression(member.applyleft), CastExpression(member.applyright))
            Case "/" : Return Expression.Divide(CastExpression(member.applyleft), CastExpression(member.applyright))
            Case "^" : Return Expression.Power(CastExpression(member.applyleft), CastExpression(member.applyright))
            Case Else
                Throw New InvalidCastException(member.operator)
        End Select
    End Function
End Class
