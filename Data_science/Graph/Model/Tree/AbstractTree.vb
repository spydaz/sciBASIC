﻿Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Linq

Public Class AbstractTree(Of T As AbstractTree(Of T, K), K) : Inherits Vertex

    ''' <summary>
    ''' Childs table, key is the property <see cref="Vertex.Label"/>
    ''' </summary>
    ''' <returns></returns>
    Public Property Childs As Dictionary(Of K, T)
    Public Property Parent As T

    Dim qualDeli$ = "."

    Default Public ReadOnly Property Child(key As K) As T
        Get
            Return Childs(key)
        End Get
    End Property

    ''' <summary>
    ''' Not null child count in this tree node.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Count As Integer
        Get
            Dim childs = Me.EnumerateChilds _
                .SafeQuery _
                .Where(Function(c) Not c Is Nothing) _
                .ToArray

            If childs.IsNullOrEmpty Then
                ' 自己算一个节点，所以数量总是1的
                Return 1
            Else
                Dim n% = childs.Length

                For Each node In childs
                    ' 如果节点没有childs，则会返回1，因为他自身就是一个节点
                    n += node.Count
                Next

                Return n
            End If
        End Get
    End Property

    Public ReadOnly Property QualifyName As String
        Get
            If Not Parent Is Nothing Then
                Return Parent.QualifyName & qualDeli & Label
            Else
                Return Label
            End If
        End Get
    End Property

    Public ReadOnly Property IsRoot As Boolean
        Get
            Return Parent Is Nothing
        End Get
    End Property

    Public ReadOnly Property IsLeaf As Boolean
        Get
            Return Childs.IsNullOrEmpty
        End Get
    End Property

    Sub New(Optional qualDeli$ = ".")
        Me.qualDeli = qualDeli
    End Sub

    ''' <summary>
    ''' Returns the values of <see cref="Childs"/>
    ''' </summary>
    ''' <returns></returns>
    Public Function EnumerateChilds() As IEnumerable(Of T)
        Return Childs?.Values
    End Function

    Public Overrides Function ToString() As String
        Return QualifyName
    End Function

    ''' <summary>
    ''' 计算出所有的叶节点的总数，包括自己的child的叶节点
    ''' </summary>
    ''' <returns></returns>
    ''' 
    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function CountLeafs() As Integer
        Return CountLeafs(Me, 0)
    End Function

    ''' <summary>
    ''' 对某一个节点的所有的叶节点进行计数
    ''' </summary>
    ''' <param name="node"></param>
    ''' <param name="count"></param>
    ''' <returns></returns>
    Public Shared Function CountLeafs(node As T, count As Integer) As Integer
        If node.IsLeaf Then
            count += 1
        End If

        For Each child As T In node.EnumerateChilds.SafeQuery
            count += child.CountLeafs()
        Next

        Return count
    End Function
End Class