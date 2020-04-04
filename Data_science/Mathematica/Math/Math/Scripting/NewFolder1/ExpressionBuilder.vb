﻿Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Language

Public Module ExpressionBuilder

    ReadOnly operatorPriority As String() = {"^", "*/%", "+-"}

    <Extension>
    Private Function AsExpression(token As MathToken) As Expression
        Select Case token.name
            Case MathTokens.Literal
                If token.text.Last = "!"c Then
                    Return New Factorial(token.text)
                Else
                    Return New Literal(token.text)
                End If
            Case MathTokens.Symbol
                Return New SymbolExpression(token.text)
            Case MathTokens.Open, MathTokens.Close, MathTokens.Invalid, MathTokens.Operator, MathTokens.Terminator
                Throw New SyntaxErrorException
            Case Else
                Throw New NotImplementedException
        End Select
    End Function

    Public Function BuildExpression(tokens As MathToken()) As Expression
        Dim blocks = tokens.SplitByTopLevelDelimiter(MathTokens.Operator)

        If blocks = 1 Then
            If blocks(Scan0).Length > 1 Then
                ' (....)
                tokens = blocks(Scan0)
                tokens = tokens.Skip(1).Take(tokens.Length - 2).ToArray
                blocks = tokens.SplitByTopLevelDelimiter(MathTokens.Operator)
            Else
                With blocks(Scan0)(Scan0)
                    Return .AsExpression
                End With
            End If
        End If

        If blocks = 1 Then
            Return BuildExpression(blocks(Scan0))
        End If

        Dim buf As New List(Of [Variant](Of Expression, String))
        Dim oplist As New List(Of String)

        Call blocks.joinNegatives(buf, oplist)
        ' 算数操作符以及字符串操作符按照操作符的优先度进行构建
        Call buf.processOperators(oplist, operatorPriority, test:=Function(op, o) op.IndexOf(o) > -1)

        If buf > 1 Then
            Throw New SyntaxErrorException
        Else
            Return buf(Scan0)
        End If
    End Function

    ''' <summary>
    ''' *
    ''' </summary>
    ''' <param name="tokens"></param>
    ''' <param name="operators"></param>
    ''' <returns></returns>
    <Extension>
    Public Function isOperator(tokens As MathToken(), ParamArray operators As String()) As Boolean
        If tokens.Length = 1 AndAlso tokens(Scan0).name = MathTokens.Operator Then
            If operators.Length > 0 Then
                Dim op$ = tokens(Scan0).text

                For Each test As String In operators
                    If op = test Then
                        Return True
                    End If
                Next

                Return False
            Else
                Return True
            End If
        Else
            Return False
        End If
    End Function

    <Extension>
    Private Sub joinNegatives(tokenBlocks As List(Of MathToken()), ByRef buf As List(Of [Variant](Of Expression, String)), ByRef oplist As List(Of String))
        Dim syntaxResult As Expression
        Dim index As i32 = Scan0

        If tokenBlocks(Scan0).Length = 1 AndAlso tokenBlocks(Scan0)(Scan0) = (MathTokens.Operator, {"-", "+"}) Then
            ' insert a ZERO before
            tokenBlocks.Insert(Scan0, {New MathToken(MathTokens.Literal, 0)})
        End If

        For i As Integer = Scan0 To tokenBlocks.Count - 1
            If ++index Mod 2 = 0 Then
                If tokenBlocks(i).isOperator("+", "-") Then
                    syntaxResult = ExpressionBuilder.BuildExpression(tokenBlocks(i + 1))
                    syntaxResult = New BinaryExpression(
                                left:=New Literal(0),
                                right:=syntaxResult,
                                op:=tokenBlocks(i)(Scan0).text
                            )
                    i += 1
                Else
                    syntaxResult = ExpressionBuilder.BuildExpression(tokenBlocks(i))
                End If

                Call buf.Add(syntaxResult)
            Else
                Call buf.Add(tokenBlocks(i)(Scan0).text)
                Call oplist.Add(buf.Last.VB)
            End If
        Next
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="buf"></param>
    ''' <param name="oplist"></param>
    ''' <param name="operators$"></param>
    ''' <param name="test">test(op, o)</param>
    <Extension>
    Private Sub processOperators(buf As List(Of [Variant](Of Expression, String)), oplist As List(Of String), operators$(), test As Func(Of String, String, Boolean))
        Dim binExp As Expression
        Dim a As Expression
        Dim b As Expression

        For Each op As String In operators
            Dim nop As Integer = oplist _
                .AsEnumerable _
                .Count(Function(o) test(op, o))

            ' 从左往右计算
            For i As Integer = 0 To nop - 1
                For j As Integer = 0 To buf.Count - 1
                    If buf(j) Like GetType(String) AndAlso test(op, buf(j).VB) Then
                        ' j-1 and j+1
                        If i - 1 < 0 Then
                            Throw New SyntaxErrorException
                        Else
                            a = buf(j - 1).TryCast(Of Expression)
                        End If
                        If j + 1 > buf.Count Then
                            Throw New SyntaxErrorException
                        Else
                            b = buf(j + 1).TryCast(Of Expression)
                        End If

                        binExp = New BinaryExpression(a, b, buf(j).VB)

                        Call buf.RemoveRange(j - 1, 3)
                        Call buf.Insert(j - 1, binExp)

                        Exit For
                    End If
                Next
            Next
        Next
    End Sub
End Module
