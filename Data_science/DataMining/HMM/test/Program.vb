﻿Imports Microsoft.VisualBasic.DataMining.HiddenMarkovChain
Imports Microsoft.VisualBasic.DataMining.HiddenMarkovChain.Models

Module Program

    Sub Main()
        Call HMMTest()
    End Sub

    Sub HMMTest()

        Dim hiddenStates As StatesObject() = {
            New StatesObject With {.state = "AT-rich", .prob = {0.95, 0.05}},
            New StatesObject With {.state = "CG-rich", .prob = {0.1, 0.9}}
        }
        Dim observables As Observable() = {
              New Observable With {.obs = "A", .prob = {0.4, 0.05}},
              New Observable With {.obs = "C", .prob = {0.1, 0.45}},
              New Observable With {.obs = "G", .prob = {0.1, 0.45}},
              New Observable With {.obs = "T", .prob = {0.4, 0.05}}
        }
        Dim hiddenInit = {0.65, 0.35}

        Dim HMModel = New HMM(hiddenStates, observables, hiddenInit)
        Dim observation = "A"
        Dim hiddenState = "AT-rich"
        ' 0.9369369369369369
        Dim bayesResult = HMModel.bayesTheorem(observation, hiddenState)

        Dim obSequence As New Chain(Function(a, b) a = b) With {.obSequence = {"T", "C", "G", "G", "A"}}
        ' alphaF 0.0003171642187500001
        Dim forwardProbability = HMModel.forwardAlgorithm(obSequence)
        ' betaF 0.0003171642187500001
        Dim backwardProbability = HMModel.backwardAlgorithm(obSequence)

        obSequence = New Chain(Function(a, b) a = b) With {.obSequence = {"A", "T", "C", "G", "C", "G", "T", "C", "A", "T", "C", "G", "T", "C", "G", "T", "C", "C", "G"}}

        ' 'AT-rich', 'AT-rich', 'CG-rich', 'CG-rich', 'CG-rich', ...
        ' 1.972455831264621E-14
        Dim viterbiResult = HMModel.viterbiAlgorithm(obSequence)

        obSequence = New Chain(Function(a, b) a = b) With {.obSequence = {"A", "T", "C", "G", "C", "G", "T", "C", "A", "T", "C", "G", "T", "C", "G", "T", "C", "C", "G"}}

        ' [ [ 0.748722257770877, 0.251277742229123 ], [ 0.08173322039272721, 0.9182667796072727 ] ]
        Dim maximizedModel = HMModel.baumWelchAlgorithm(obSequence)
        Dim max = maximizedModel.GetTransMatrix

        Pause()
    End Sub

    Sub MarkovChainTest()
        Dim states As StatesObject() = {
           New StatesObject With {.state = "sunny", .prob = {0.4, 0.4, 0.2}},
           New StatesObject With {.state = "cloudy", .prob = {0.3, 0.3, 0.4}},
           New StatesObject With {.state = "rainy", .prob = {0.2, 0.5, 0.3}}
       }
        Dim init As Double() = {0.4, 0.3, 0.3}
        Dim markovChain As New MarkovChain(states, init)

        Dim stateSeq As New Chain(Function(a, b) a = b) With {.obSequence = {"sunny", "rainy", "sunny", "sunny", "cloudy"}}
        ' 0.002560000000000001
        Dim seqProbability = markovChain.SequenceProb(stateSeq)

        ' Call Console.WriteLine(markovChain.GetTransMatrix)

        Pause()
    End Sub

End Module