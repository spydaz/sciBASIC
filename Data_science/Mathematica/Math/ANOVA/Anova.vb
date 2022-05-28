﻿Imports Microsoft.VisualBasic.Math.Statistics.Distributions.LinearMoments
Imports stdNum = System.Math

Public Class AnovaTest

    Public SSB As Double
    Public SSW As Double
    Public F_score As Double
    Public singlePvalue As Double
    Public doublePvalue As Double

    Public Const P_FIVE_PERCENT As String = "p<.05"
    Public Const P_ONE_PERCENT As String = "p<.01"

    Public SSW_sum_of_squares_within_groups As Double
    Public SSB_sum_of_squares_between_groups As Double
    Public SS_total_sum_of_squares As Double
    Public allObservationsMean As Double
    Public groups As New List(Of Group)()

    Private numenator_degrees_of_freedom As Integer = -1
    Private denomenator_degrees_of_freedom As Integer = -1

    Dim m_type As String = "this will be a constant to chose 1 or 5 %"

    Public Overridable ReadOnly Property numenator As Integer
        Get
            ' return groups.size() -1...
            ' for use with the Distribution table
            Return numenator_degrees_of_freedom
        End Get
    End Property

    Public Overridable ReadOnly Property denomenator As Integer
        Get
            ' return number of all observations - groups.size()... 
            ' for use with the Distribution table
            Return denomenator_degrees_of_freedom
        End Get
    End Property

    Public Overridable ReadOnly Property type As String
        Get
            ' return whether this is a 1% or a 5% test
            Return m_type
        End Get
    End Property

    Public Overridable ReadOnly Property criticalNumber As Double
        Get
            Dim table As New DistributionTable()
            Dim critical = table.getCriticalNumber(denomenator, numenator, type)

            Return critical
        End Get
    End Property

    ''' <summary>
    ''' step7
    ''' </summary>
    ''' <returns></returns>
    Public Overridable Function fScore_determineIt() As Double
        F_score = SSB / SSW
        singlePvalue = New FDistribution(numenator_degrees_of_freedom, denomenator_degrees_of_freedom).GetCDF(F_score)
        doublePvalue = singlePvalue * 2

        Return F_score
    End Function

    ''' <summary>
    ''' step6
    ''' </summary>
    Public Overridable Sub divide_by_degrees_of_freedom()

        numenator_degrees_of_freedom = groups.Count - 1

        SSB = SSB_sum_of_squares_between_groups / numenator_degrees_of_freedom
        Dim observations = 0
        For Each g In groups
            observations += g.ary.Length
        Next

        'degrees_of_freedom = observations - groups.size(); 
        denomenator_degrees_of_freedom = observations - groups.Count

        SSW = SSW_sum_of_squares_within_groups / denomenator_degrees_of_freedom
    End Sub

    ''' <summary>
    ''' step1
    ''' </summary>
    ''' <param name="matrix"></param>
    ''' <param name="type"></param>
    Public Overridable Sub populate(ByVal matrix As IEnumerable(Of Double()), ByVal type As String)
        m_type = type

        For Each v As Double() In matrix
            Call groups.Add(New Group(v))
        Next
    End Sub

    ''' <summary>
    ''' step2
    ''' </summary>
    Public Overridable Sub findWithinGroupMeans()
        Dim total As Double = 0
        Dim observationsCount = 0
        For i = 0 To groups.Count - 1
            Dim g = groups(i)
            Dim groupTotal As Double = 0
            For j = 0 To g.ary.Length - 1
                groupTotal += g.ary(j)
            Next
            total += groupTotal
            observationsCount += g.ary.Length
            g.mean = groupTotal / g.ary.Length
        Next
        allObservationsMean = total / observationsCount
    End Sub

    ''' <summary>
    ''' step3
    ''' </summary>
    Public Overridable Sub setSumOfSquaresOfGroups()

        For i = 0 To groups.Count - 1
            Dim g = groups(i)
            For j = 0 To g.ary.Length - 1
                Dim observation = g.ary(j)
                Dim result = observation - g.mean
                Dim answer = stdNum.Pow(result, 2)
                g.raisedSum += answer
            Next
            SSW_sum_of_squares_within_groups += g.raisedSum
        Next
    End Sub

    ''' <summary>
    ''' step4
    ''' </summary>
    Public Overridable Sub setTotalSumOfSquares()
        SS_total_sum_of_squares = 0
        For i = 0 To groups.Count - 1
            Dim g = groups(i)
            For j = 0 To g.ary.Length - 1
                Dim observation = g.ary(j)
                Dim result = observation - allObservationsMean

                SS_total_sum_of_squares += stdNum.Pow(result, 2)
            Next
        Next

        SSB_sum_of_squares_between_groups = SS_total_sum_of_squares - SSW_sum_of_squares_within_groups
    End Sub
End Class
