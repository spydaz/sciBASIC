﻿Imports Microsoft.VisualBasic.DataMining.ComponentModel
Imports stdNum = System.Math

Public Class tSNE : Inherits IDataEmbedding

    ''' <summary>
    ''' effective number of nearest neighbors
    ''' </summary>
    Friend mPerplexity As Double
    ''' <summary>
    ''' learning rate
    ''' </summary>
    Friend mEpsilon As Double
    Friend mIter As Double
    Friend mRet As Boolean = False
    Friend mVal As Double = 0.0
    Friend mP As Double()
    ''' <summary>
    ''' Y is an array of 2-D points that you can plot
    ''' </summary>
    Friend mY As Double()()
    Friend mGains As Double()()
    Friend mYStep As Double()()
    Friend mN As Integer
    Friend mCost As Double
    Friend mGrad As Double()()

    ''' <summary>
    ''' dimensionality of the embedding
    ''' </summary>
    Friend ReadOnly mDim As Integer
    Friend ReadOnly random As RandomHelper
    Friend ReadOnly cost As CostFunction

    Public Overrides ReadOnly Property dimension As Integer
        Get
            Return mDim
        End Get
    End Property

    Public Sub New(perplexity As Double, [dim] As Integer, epsilon As Double)
        mPerplexity = perplexity
        mDim = [dim]
        mEpsilon = epsilon
        mIter = 0
        random = New RandomHelper(Me)
        cost = New CostFunction(Me)
    End Sub

    ''' <summary>
    ''' return current solution
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Function GetEmbedding() As Double()()
        Return mY
    End Function

    ''' <summary>
    ''' this function takes a set of high-dimensional points
    ''' and creates matrix P from them using gaussian kernel
    ''' </summary>
    ''' <param name="X"></param>
    Public Sub InitDataRaw(X As IEnumerable(Of Double()))
        With X.ToArray
            Dim N = .Length
            Dim D = DirectCast(.GetValue(0), Double()).Length

            ' convert X to distances using gaussian kernel
            Dim dists = xtod(X)

            ' attach to object
            ' then back up the size of the dataset
            mP = d2p(dists, mPerplexity, 0.0001)
            mN = N
        End With

        Call InitSolution()
    End Sub

    ' this function takes a given distance matrix and creates
    ' matrix P from them.
    ' D is assumed to be provided as a list of lists, and should be symmetric
    Public Sub InitDataDist(D As Double()())
        Dim N = D.Length
        ' convert D to a (fast) typed array version
        Dim dists = zeros(N * N) ' allocate contiguous array

        For i = 0 To N - 1

            For j = i + 1 To N - 1
                Dim lD = D(i)(j)
                dists(i * N + j) = lD
                dists(j * N + i) = lD
            Next
        Next

        mP = d2p(dists, mPerplexity, 0.0001)
        mN = N
        InitSolution() ' refresh this
    End Sub

    ' (re)initializes the solution to random
    Private Sub InitSolution()
        ' generate random solution to t-SNE
        mY = random.randn2d(mN, mDim) ' the solution
        mGains = RandomHelper.randn2d(mN, mDim, 1.0) ' step gains to accelerate progress in unchanging directions
        mYStep = RandomHelper.randn2d(mN, mDim, 0.0) ' momentum accumulator
        mIter = 0
    End Sub

    ' perform a single step of optimization to improve the embedding
    Public Function [Step]() As Double
        mIter += 1
        Dim N = mN
        Me.cost.CostGrad(mY) ' evaluate gradient
        Dim cost = mCost
        Dim grad = mGrad

        ' perform gradient step
        Dim ymean = zeros(mDim)

        For i = 0 To N - 1

            For d = 0 To mDim - 1
                Dim gid = grad(i)(d)
                Dim sid = mYStep(i)(d)
                Dim gainid = mGains(i)(d)

                ' compute gain update
                Dim newgain = If(stdNum.Sign(gid) = stdNum.Sign(sid), gainid * 0.8, gainid + 0.2)

                If newgain < 0.01 Then
                    ' clamp
                    newgain = 0.01
                End If

                ' store for next turn
                mGains(i)(d) = newgain

                ' compute momentum step direction
                Dim momval = If(mIter < 250, 0.5, 0.8)
                Dim newsid = momval * sid - mEpsilon * newgain * grad(i)(d)

                ' remember the step we took
                mYStep(i)(d) = newsid

                ' step!
                mY(i)(d) += newsid
                ' accumulate mean so that we can center later
                ymean(d) += mY(i)(d)
            Next
        Next

        ' reproject Y to be zero mean
        For i = 0 To N - 1
            For d = 0 To mDim - 1
                mY(i)(d) -= ymean(d) / N
            Next
        Next

        ' return current cost
        Return cost
    End Function
End Class
