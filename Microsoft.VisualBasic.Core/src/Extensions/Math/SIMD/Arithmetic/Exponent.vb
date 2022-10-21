﻿Namespace Math.SIMD

    Public Class Exponent

        Public Shared Function f64_scalar_op_exponent_f64(v1 As Double, v2 As Double()) As Double()
            Dim result As Double() = New Double(v2.Length - 1) {}

            For i As Integer = 0 To v2.Length - 1
                result(i) = v1 ^ v2(i)
            Next

            Return result
        End Function

        Public Shared Function f64_op_exponent_f64_scalar(v1 As Double(), v2 As Double) As Double()
            Dim result As Double() = New Double(v1.Length - 1) {}

            For i As Integer = 0 To v1.Length - 1
                result(i) = v1(i) ^ v2
            Next

            Return result
        End Function

        Public Shared Function f64_op_exponent_f64(v1 As Double(), v2 As Double()) As Double()
            Dim result As Double() = New Double(v1.Length - 1) {}

            For i As Integer = 0 To v1.Length - 1
                result(i) = v1(i) ^ v2(i)
            Next

            Return result
        End Function
    End Class
End Namespace