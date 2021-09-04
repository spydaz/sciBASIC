Imports System
Imports Microsoft.VisualBasic.ComponentModel.TagData
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.MachineLearning
Imports Microsoft.VisualBasic.MachineLearning.XGBoost
Imports Microsoft.VisualBasic.MachineLearning.XGBoost.util
Imports Microsoft.VisualBasic.Serialization.JSON

Module Program

    Public Sub Main(ByVal args As String())
        Dim data As DMatrix = loadData()
        Dim predictor As Predictor = New Predictor("E:\GCModeller\src\runtime\sciBASIC#\Data_science\MachineLearning\xgboost\test\resources\model\gbtree\v47\binary-logistic.model".Open)
        predictAndLogLoss(predictor, data)
        predictLeafIndex(predictor, data)

        Pause()
    End Sub

    ''' <summary>
    ''' Predicts probability and calculate its logarithmic loss using <seealsocref="Predictor"/>.
    ''' </summary>
    ''' <param name="predictor"> Predictor </param>
    ''' <param name="data">      test data </param>
    Friend Sub predictAndLogLoss(ByVal predictor As Predictor, ByVal data As DMatrix)
        Dim sum As Double = 0

        For Each pair As IntegerTagged(Of XGBoost.util.FVec) In data.enumerateData
            Dim predicted As Double() = predictor.predict(pair.Value)
            Dim predValue = Math.Min(Math.Max(predicted(0), 0.000000000000001), 1 - 0.000000000000001)
            Dim actual As Integer = pair.Tag
            sum = actual * Math.Log(predValue) + (1 - actual) * Math.Log(1 - predValue)
        Next

        Dim logLoss = -sum / data.size
        Console.WriteLine("Logloss: " & logLoss)
    End Sub

    ''' <summary>
    ''' Predicts leaf index of each tree.
    ''' </summary>
    ''' <param name="predictor"> Predictor </param>
    ''' <param name="data"> test data </param>
    Friend Sub predictLeafIndex(ByVal predictor As Predictor, ByVal data As DMatrix)
        Dim count As i32 = 0

        For Each pair As IntegerTagged(Of XGBoost.util.FVec) In data.enumerateData
            Dim leafIndexes As Integer() = predictor.predictLeaf(pair.Value)
            Console.Write("leafIndexes[{0:D}]: {1}{2}", ++count, (leafIndexes).GetJson, Environment.NewLine)
        Next
    End Sub

    ''' <summary>
    ''' Loads test data.
    ''' </summary>
    ''' <returns> test data </returns>
    Friend Function loadData() As DMatrix
        Dim handler = DMatrix.Builder

        For Each line As String In "E:\GCModeller\src\runtime\sciBASIC#\Data_science\MachineLearning\xgboost\test\resources\data\agaricus.txt.0.test".ReadAllLines
            Dim values = line.Split(" "c)
            Dim map As New Dictionary(Of Integer, Single)()

            For i = 1 To values.Length - 1
                Dim pair = values(i).Split(":"c)
                map(Integer.Parse(pair(0))) = Single.Parse(pair(1))
            Next

            handler.add(Integer.Parse(values(0)), XGBoost.util.FVecTransformer.fromMap(Of Single)(map))
        Next

        Return handler.getMatrix()
    End Function
End Module