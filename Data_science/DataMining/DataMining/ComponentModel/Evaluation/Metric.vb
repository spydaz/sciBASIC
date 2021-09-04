﻿Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports stdNum = System.Math

Namespace ComponentModel.Evaluation

    Public Enum Metrics
        none
        ''' <summary>
        ''' Metric.accuracy(pred, label)
        ''' </summary>
        acc
        ''' <summary>
        ''' Metric.error(pred, label)
        ''' </summary>
        [error]
        ''' <summary>
        ''' Metric.mean_square_error(pred, label)
        ''' </summary>
        mse
        ''' <summary>
        ''' Metric.mean_absolute_error(pred, label)
        ''' </summary>
        mae
        ''' <summary>
        ''' Metric.auc(pred, label)
        ''' </summary>
        auc
    End Enum

    Public Delegate Function IMetric(pred As Double(), label As Double()) As Double

    Public NotInheritable Class Metric

        Private Sub New()
        End Sub

        Public Shared Function Parse(metric As String) As Metrics
            Select Case Strings.LCase(metric)
                Case NameOf(Metrics.acc) : Return Metrics.acc
                Case NameOf(Metrics.auc) : Return Metrics.auc
                Case NameOf(Metrics.error) : Return Metrics.error
                Case NameOf(Metrics.mae) : Return Metrics.mae
                Case NameOf(Metrics.mse) : Return Metrics.mse
                Case Else
                    Return Metrics.mse
            End Select
        End Function

        Public Shared Function GetMetric(metric As Metrics) As IMetric
            Select Case metric
                Case Metrics.acc : Return AddressOf accuracy
                Case Metrics.error : Return AddressOf [error]
                Case Metrics.mse : Return AddressOf mean_square_error
                Case Metrics.mae : Return AddressOf mean_absolute_error
                Case Metrics.auc : Return AddressOf auc
                Case Else
                    Throw New NotImplementedException()
            End Select
        End Function

        Public Shared Function accuracy(pred As Double(), label As Double()) As Double
            Dim hit = 0.0

            For i = 0 To pred.Length - 1

                If label(i) = 0 AndAlso pred(i) < 0.5 OrElse label(i) = 1 AndAlso pred(i) > 0.5 Then
                    hit += 1
                End If
            Next

            Return hit / pred.Length
        End Function

        Public Shared Function [error](pred As Double(), label As Double()) As Double
            Return 1.0 - Metric.accuracy(pred, label)
        End Function

        Public Shared Function mean_square_error(pred As Double(), label As Double()) As Double
            Dim sum = 0.0

            For i = 0 To pred.Length - 1
                sum += stdNum.Pow(pred(i) - label(i), 2.0)
            Next

            Return sum / pred.Length
        End Function

        Public Shared Function mean_absolute_error(pred As Double(), label As Double()) As Double
            Dim sum = 0.0

            For i = 0 To pred.Length - 1
                sum += stdNum.Abs(pred(i) - label(i))
            Next

            Return sum / pred.Length
        End Function

        Public Shared Function auc(pred As Double(), label As Double()) As Double
            Dim n_pos As Double = 0

            For Each v In label
                n_pos += v
            Next

            Dim n_neg = pred.Length - n_pos
            Dim label_pred As Double()() = MAT(Of Double)(pred.Length, 2)

            For i = 0 To pred.Length - 1
                label_pred(i)(0) = label(i)
                label_pred(i)(1) = pred(i)
            Next

            Array.Sort(label_pred, New Metric.ComparatorAnonymousInnerClass())
            Dim accumulated_neg As Double = 0
            Dim satisfied_pair As Double = 0

            For i = 0 To label_pred.Length - 1

                If label_pred(i)(0) = 1 Then
                    satisfied_pair += accumulated_neg
                Else
                    accumulated_neg += 1
                End If
            Next

            Return satisfied_pair / n_neg / n_pos
        End Function

        Private Class ComparatorAnonymousInnerClass : Implements IComparer(Of Double())

            Public Sub New()
            End Sub

            Public Overridable Function compare(a As Double(), b As Double()) As Integer Implements IComparer(Of Double()).Compare
                Return a(1).CompareTo(b(1))
            End Function
        End Class
    End Class
End Namespace