﻿Namespace util

    <Serializable>
    Friend Class FVecMapImpl(Of T1 As IComparable) : Implements FVec

        Private ReadOnly values As IDictionary(Of Integer, T1)

        Public Sub New(values As IDictionary(Of Integer, T1))
            Me.values = values
        End Sub

        Public Overridable Function fvalue(index As Integer) As Double Implements FVec.fvalue
            Dim number As IComparable = values.GetValueOrNull(index)

            If number Is Nothing Then
                Return Double.NaN
            Else
                Return CType(number, Double)
            End If
        End Function
    End Class
End Namespace