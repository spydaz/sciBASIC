﻿Imports System.Drawing
Imports Microsoft.VisualBasic.Linq

''' <summary>
''' a generic grid graph for fast query of the 2D geometry data
''' </summary>
''' <typeparam name="T"></typeparam>
Public Class Grid(Of T)

    ReadOnly matrix2D As Dictionary(Of Long, Dictionary(Of Long, GridCell(Of T)))

    Private Sub New(points As IEnumerable(Of GridCell(Of T)))
        matrix2D = points _
            .GroupBy(Function(d) d.index.X) _
            .ToDictionary(Function(d) CLng(d.Key),
                          Function(d)
                              Return d _
                                  .GroupBy(Function(p) p.index.Y) _
                                  .ToDictionary(Function(p) CLng(p.Key),
                                                Function(p)
                                                    Return p.First
                                                End Function)
                          End Function)
    End Sub

    ''' <summary>
    ''' populate all of the cell data in current grid graph
    ''' </summary>
    ''' <returns></returns>
    Public Iterator Function EnumerateData() As IEnumerable(Of T)
        For Each row In matrix2D
            For Each col In row.Value
                Yield col.Value.data
            Next
        Next
    End Function

    ''' <summary>
    ''' get target cell data via a given pixel point
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    ''' <param name="hit"></param>
    ''' <returns>
    ''' nothing will be returns if there is no data on the given ``[x,y]`` pixel point.
    ''' </returns>
    Public Function GetData(x As Integer, y As Integer, Optional ByRef hit As Boolean = False) As T
        Dim xkey = CLng(x), ykey = CLng(y)

        If Not matrix2D.ContainsKey(xkey) Then
            hit = False
            Return Nothing
        ElseIf Not matrix2D(xkey).ContainsKey(ykey) Then
            hit = False
            Return Nothing
        Else
            hit = True
        End If

        Return matrix2D(xkey)(ykey).data
    End Function

    ''' <summary>
    ''' get a range of nearby cell data via a given pixel point data 
    ''' and query size of the cell block rectangle.
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    ''' <param name="gridSize"></param>
    ''' <returns></returns>
    Public Function Query(x As Integer, y As Integer, gridSize As Integer) As IEnumerable(Of T)
        Return Query(x, y, New Size(gridSize, gridSize))
    End Function

    Public Iterator Function Query(x As Integer, y As Integer, gridSize As Size) As IEnumerable(Of T)
        Dim q As T
        Dim hit As Boolean = False

        For i As Integer = x - gridSize.Width To x + gridSize.Width
            For j As Integer = y - gridSize.Height To y + gridSize.Height
                q = GetData(i, j, hit)

                If hit Then
                    Yield q
                End If
            Next
        Next
    End Function

    Public Shared Function Create(data As IEnumerable(Of T), getX As Func(Of T, Integer), getY As Func(Of T, Integer)) As Grid(Of T)
        Return data _
            .SafeQuery _
            .Select(Function(d)
                        Return New GridCell(Of T)(getX(d), getY(d), d)
                    End Function) _
            .DoCall(Function(vec)
                        Return New Grid(Of T)(vec)
                    End Function)
    End Function

End Class
