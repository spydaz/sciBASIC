﻿Imports stdNum = System.Math

Namespace Convolutional

    Public Class Pool : Inherits Layer

        Public pool As Integer()
        Public stride As Integer()

        Public Overrides ReadOnly Property type As LayerTypes
            Get
                Return LayerTypes.Pool
            End Get
        End Property

        Public Sub New(inputTensorDims As Integer(), pad As Integer())
            Call MyBase.New(inputTensorDims, pad)

            pool = New Integer(1) {}
            stride = New Integer(1) {}
        End Sub

        Public Overloads Sub setOutputDims()
            outputDims = New Integer(2) {
                CInt(stdNum.Floor(inputTensorDims(0) / stride(0))),
                CInt(stdNum.Floor(inputTensorDims(1) / stride(1))),
                inputTensorDims(2)
            }
        End Sub

        Protected Overrides Function layerFeedNext() As Layer
            Dim inputHeight = inputTensorDims(0)
            Dim inputWidth = inputTensorDims(1)
            Dim channelCount = inputTensorDims(2)
            Dim poolHeight = pool(0)
            Dim poolWidth = pool(1)
            Dim inputInd = New Integer() {0, 0, 0}
            Dim outputInd = New Integer() {0, 0, 0}
            Dim max As Single
            Dim i As Integer = 0
            Dim j As Integer = 0

            While inputInd(2) < channelCount
                outputInd(2) = inputInd(2)
                i = 0

                While i <= inputHeight - poolHeight
                    outputInd(0) = CInt(stdNum.Floor(i / stride(0)))
                    j = 0

                    While j <= inputWidth - poolWidth
                        outputInd(1) = CInt(stdNum.Floor(j / stride(1)))
                        max = Single.MinValue
                        inputInd(0) = i

                        While inputInd(0) < i + poolHeight
                            inputInd(1) = j

                            While inputInd(1) < j + poolWidth
                                Dim f As Single = inputTensor(inputInd)

                                If f > max Then
                                    max = f
                                End If

                                inputInd(1) += 1
                            End While

                            inputInd(0) += 1
                        End While

                        Call writeNextLayerInput(outputInd, max)

                        j += stride(1)
                    End While

                    i += stride(0)
                End While

                inputInd(2) += 1
            End While

            disposeInputTensor()

            Return Me
        End Function
    End Class
End Namespace
