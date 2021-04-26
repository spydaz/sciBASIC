﻿Public Class MarkovChain

    Dim states
    Dim transMatrix
    Dim initialProb
    Dim prob As calculateProb

    Sub New(states, init)
        Me.states = states.map(Function(s) s.state)
        Me.transMatrix = states.map(Function(s) s.prob)
        Me.initialProb = init
        Me.prob = New calculateProb(transMatrix, init, states)
    End Sub
End Class

