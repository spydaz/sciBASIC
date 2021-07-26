﻿Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Linq
Imports Microsoft.VisualBasic.Math.Scripting.MathExpression.Impl

Namespace Scripting.MathExpression

    ''' <summary>
    ''' (a+b)^2 = a^2 + ab + ab + b ^ 2
    ''' </summary>
    Public Module PolyExpansion

        <Extension>
        Public Function Expands(expression As Expression) As Expression
            If Not TypeOf expression Is BinaryExpression Then
                Return expression
            Else
                Return ExpandsBinary(expression)
            End If
        End Function

        Private Function ExpandsBinary(bin As BinaryExpression) As Expression
            Select Case bin.operator
                Case "^"
                    If TypeOf bin.right Is Literal AndAlso DirectCast(bin.right, Literal).isInteger Then
                        If TypeOf bin.left Is BinaryExpression Then
                            Return ExpandsPower(bin.left, bin.right.Evaluate(Nothing))
                        Else
                            Return bin
                        End If
                    Else
                        Return bin
                    End If
                Case "+", "-"
                    Return bin
                Case "*", "/"
                    Return bin
                Case Else
                    Throw New NotImplementedException(bin.operator)
            End Select
        End Function

        Private Function ExpandsPower(base As BinaryExpression, power As Integer) As Expression
            If Not (base.operator = "+" OrElse base.operator = "-") Then
                Return BinaryExpression.Power(base, power)
            ElseIf power = 1 Then
                Return base
            ElseIf power = 0 Then
                Return New Literal(1)
            End If

            Dim a As Expression = base.left
            Dim b As Expression = base.right

            If base.operator = "-" Then
                b = New UnaryExpression With {.[operator] = "-", .value = Expands(b)}
            End If

            Dim combines As New List(Of Expression())

            combines.Add({a, a})
            combines.Add({a, b})
            combines.Add({b, a})
            combines.Add({b, b})

            For i As Integer = 3 To power
                For Each x As Expression In {a, b}
                    For Each line As Expression() In combines
                        combines.Add(line.JoinIterates({x}).ToArray)
                    Next
                Next
            Next

            Dim simplify = combines _
                .GroupBy(Function(r)
                             Return r.Select(Function(t) t.ToString).OrderBy(Function(t) t).JoinBy("*")
                         End Function) _
                .Select(Function(tokens)
                            Return simple(tokens)
                        End Function)

            Dim bin As BinaryExpression = Nothing
            Dim left As Expression = simplify.First

            For Each i In simplify.Skip(1)
                bin = New BinaryExpression(left, i, "+")
            Next

            Return bin
        End Function

        Private Function simple(group As IGrouping(Of String, Expression())) As Expression
            Dim PI As BinaryExpression = Nothing
            Dim all = group.ToArray
            Dim merge = all.First
            Dim left As Expression = merge.First

            For Each i In merge.Skip(1)
                PI = New BinaryExpression(left, i, "*")
            Next

            If all.Length = 1 Then
                Return PI
            Else
                Return New BinaryExpression(New Literal(all.Length), PI, "*")
            End If
        End Function
    End Module
End Namespace