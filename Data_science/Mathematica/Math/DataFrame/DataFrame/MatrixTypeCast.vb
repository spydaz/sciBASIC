﻿Imports System.Runtime.CompilerServices

Public Module MatrixTypeCast

    <Extension>
    Public Function GetDataFrame(mat As DataMatrix) As DataFrame
        Dim table As New Dictionary(Of String, FeatureVector)
        Dim keys As String() = mat.names.Objects

        For i As Integer = 0 To keys.Length - 1
            table(keys(i)) = New FeatureVector(mat.matrix(i))
        Next

        Return New DataFrame With {
            .features = table,
            .rownames = keys
        }
    End Function
End Module
