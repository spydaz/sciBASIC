﻿Imports Microsoft.VisualBasic.MachineLearning.XGBoost.util

Namespace gbm

    ''' <summary>
    ''' Interface of gradient boosting model.
    ''' </summary>
    Public Interface GradBooster
        WriteOnly Property numClass As Integer

        ''' <summary>
        ''' Loads model from stream.
        ''' </summary>
        ''' <paramname="reader">       input stream </param>
        ''' <paramname="with_pbuffer"> whether the incoming data contains pbuffer </param>
        ''' <exceptioncref="IOException"> If an I/O error occurs </exception>
        'JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Method 'throws' clauses are not available in .NET:
        'ORIGINAL LINE: void loadModel(biz.k11i.xgboost.util.ModelReader reader, boolean with_pbuffer) throws java.io.IOException;
        Sub loadModel(ByVal reader As ModelReader, ByVal with_pbuffer As Boolean)

        ''' <summary>
        ''' Generates predictions for given feature vector.
        ''' </summary>
        ''' <paramname="feat">        feature vector </param>
        ''' <paramname="ntree_limit"> limit the number of trees used in prediction </param>
        ''' <returns> prediction result </returns>
        Function predict(ByVal feat As FVec, ByVal ntree_limit As Integer) As Double()

        ''' <summary>
        ''' Generates a prediction for given feature vector.
        ''' <para>
        ''' This method only works when the model outputs single value.
        ''' </para>
        ''' </summary>
        ''' <paramname="feat">        feature vector </param>
        ''' <paramname="ntree_limit"> limit the number of trees used in prediction </param>
        ''' <returns> prediction result </returns>
        Function predictSingle(ByVal feat As FVec, ByVal ntree_limit As Integer) As Double

        ''' <summary>
        ''' Predicts the leaf index of each tree. This is only valid in gbtree predictor.
        ''' </summary>
        ''' <paramname="feat">        feature vector </param>
        ''' <paramname="ntree_limit"> limit the number of trees used in prediction </param>
        ''' <returns> predicted leaf indexes </returns>
        Function predictLeaf(ByVal feat As FVec, ByVal ntree_limit As Integer) As Integer()
    End Interface

    Public Class GradBooster_Factory
        ''' <summary>
        ''' Creates a gradient booster from given name.
        ''' </summary>
        ''' <paramname="name"> name of gradient booster </param>
        ''' <returns> created gradient booster </returns>
        Public Shared Function createGradBooster(ByVal name As String) As GradBooster
            If "gbtree".Equals(name) Then
                Return New GBTree()
            ElseIf "gblinear".Equals(name) Then
                Return New GBLinear()
            ElseIf "dart".Equals(name) Then
                Return New Dart()
            End If

            Throw New ArgumentException(name & " is not supported model.")
        End Function
    End Class

    <Serializable>
    Public MustInherit Class GBBase
        Implements GradBooster

        Public MustOverride Function predictLeaf(ByVal feat As FVec, ByVal ntree_limit As Integer) As Integer() Implements GradBooster.predictLeaf
        Public MustOverride Function predictSingle(ByVal feat As FVec, ByVal ntree_limit As Integer) As Double Implements GradBooster.predictSingle
        Public MustOverride Function predict(ByVal feat As FVec, ByVal ntree_limit As Integer) As Double() Implements GradBooster.predict
        Public MustOverride Sub loadModel(ByVal reader As ModelReader, ByVal with_pbuffer As Boolean) Implements GradBooster.loadModel
        Protected Friend num_class As Integer

        Public Overridable WriteOnly Property numClass As Integer Implements GradBooster.numClass
            Set(ByVal value As Integer)
                num_class = value
            End Set
        End Property
    End Class
End Namespace
