﻿Imports biz.k11i.xgboost.gbm
Imports System
Imports PredictorConfiguration = biz.k11i.xgboost.config.PredictorConfiguration
Imports GradBooster = biz.k11i.xgboost.gbm.GradBooster
Imports ObjFunction = biz.k11i.xgboost.learner.ObjFunction
Imports SparkModelParam = biz.k11i.xgboost.spark.SparkModelParam
Imports FVec = biz.k11i.xgboost.util.FVec
Imports ModelReader = biz.k11i.xgboost.util.ModelReader

Namespace biz.k11i.xgboost


    ''' <summary>
    ''' Predicts using the Xgboost model.
    ''' </summary>
    <Serializable>
    Public Class Predictor
        Private mparam As ModelParam
        'JAVA TO C# CONVERTER CRACKED BY X-CRACKER NOTE: Fields cannot have the same name as methods:
        Private sparkModelParam_Renamed As SparkModelParam
        Private name_obj As String
        Private name_gbm As String
        Private obj As ObjFunction
        Private gbm As GradBooster

        'JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Method 'throws' clauses are not available in .NET:
        'ORIGINAL LINE: public Predictor(java.io.InputStream in) throws java.io.IOException
        Public Sub New(ByVal [in] As Stream)
            Me.New([in], Nothing)
        End Sub

        ''' <summary>
        ''' Instantiates with the Xgboost model
        ''' </summary>
        ''' <paramname="in"> input stream </param>
        ''' <paramname="configuration"> configuration </param>
        ''' <exceptioncref="IOException"> If an I/O error occurs </exception>
        'JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Method 'throws' clauses are not available in .NET:
        'ORIGINAL LINE: public Predictor(java.io.InputStream in, biz.k11i.xgboost.config.PredictorConfiguration configuration) throws java.io.IOException
        Public Sub New(ByVal [in] As Stream, ByVal configuration As PredictorConfiguration)
            If configuration Is Nothing Then
                configuration = PredictorConfiguration.DEFAULT
            End If

            Dim reader As ModelReader = New ModelReader([in])
            readParam(reader)
            initObjFunction(configuration)
            initObjGbm()
            gbm.loadModel(reader, mparam.saved_with_pbuffer <> 0)
        End Sub

        'JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Method 'throws' clauses are not available in .NET:
        'ORIGINAL LINE: void readParam(biz.k11i.xgboost.util.ModelReader reader) throws java.io.IOException
        Friend Overridable Sub readParam(ByVal reader As ModelReader)
            Dim first4Bytes = reader.readByteArray(4)
            Dim next4Bytes = reader.readByteArray(4)
            Dim base_score As Single
            Dim num_feature As Integer

            If first4Bytes(0) = &H62 AndAlso first4Bytes(1) = &H69 AndAlso first4Bytes(2) = &H6e AndAlso first4Bytes(3) = &H66 Then

                ' Old model file format has a signature "binf" (62 69 6e 66)
                base_score = reader.asFloat(next4Bytes)
                num_feature = reader.readUnsignedInt()
            ElseIf first4Bytes(0) = &H00 AndAlso first4Bytes(1) = &H05 AndAlso first4Bytes(2) = &H5F Then

                ' Model generated by xgboost4j-spark?
                Dim modelType As String = Nothing

                If first4Bytes(3) = &H63 AndAlso next4Bytes(0) = &H6c AndAlso next4Bytes(1) = &H73 AndAlso next4Bytes(2) = &H5F Then
                    ' classification model
                    modelType = SparkModelParam.MODEL_TYPE_CLS
                ElseIf first4Bytes(3) = &H72 AndAlso next4Bytes(0) = &H65 AndAlso next4Bytes(1) = &H67 AndAlso next4Bytes(2) = &H5F Then
                    ' regression model
                    modelType = SparkModelParam.MODEL_TYPE_REG
                End If

                If Not ReferenceEquals(modelType, Nothing) Then
                    Dim len As Integer = (next4Bytes(3) << 8) + (reader.readByteAsInt())
                    Dim featuresCol = reader.readUTF(len)
                    sparkModelParam_Renamed = New SparkModelParam(modelType, featuresCol, reader)
                    base_score = reader.readFloat()
                    num_feature = reader.readUnsignedInt()
                Else
                    base_score = reader.asFloat(first4Bytes)
                    num_feature = reader.asUnsignedInt(next4Bytes)
                End If
            Else
                base_score = reader.asFloat(first4Bytes)
                num_feature = reader.asUnsignedInt(next4Bytes)
            End If

            mparam = New ModelParam(base_score, num_feature, reader)
            name_obj = reader.readString()
            name_gbm = reader.readString()
        End Sub

        Friend Overridable Sub initObjFunction(ByVal configuration As PredictorConfiguration)
            obj = configuration.objFunction

            If obj Is Nothing Then
                obj = ObjFunction.fromName(name_obj)
            End If
        End Sub

        Friend Overridable Sub initObjGbm()
            obj = ObjFunction.fromName(name_obj)
            gbm = GradBooster_Factory.createGradBooster(name_gbm)
            gbm.numClass = mparam.num_class
        End Sub

        ''' <summary>
        ''' Generates predictions for given feature vector.
        ''' </summary>
        ''' <paramname="feat"> feature vector </param>
        ''' <returns> prediction values </returns>
        Public Overridable Function predict(ByVal feat As FVec) As Double()
            Return predict(feat, False)
        End Function

        ''' <summary>
        ''' Generates predictions for given feature vector.
        ''' </summary>
        ''' <paramname="feat">          feature vector </param>
        ''' <paramname="output_margin"> whether to only predict margin value instead of transformed prediction </param>
        ''' <returns> prediction values </returns>
        Public Overridable Function predict(ByVal feat As FVec, ByVal output_margin As Boolean) As Double()
            Return predict(feat, output_margin, 0)
        End Function

        ''' <summary>
        ''' Generates predictions for given feature vector.
        ''' </summary>
        ''' <paramname="feat">          feature vector </param>
        ''' <paramname="output_margin"> whether to only predict margin value instead of transformed prediction </param>
        ''' <paramname="ntree_limit">   limit the number of trees used in prediction </param>
        ''' <returns> prediction values </returns>
        Public Overridable Function predict(ByVal feat As FVec, ByVal output_margin As Boolean, ByVal ntree_limit As Integer) As Double()
            Dim preds = predictRaw(feat, ntree_limit)

            If Not output_margin Then
                Return obj.predTransform(preds)
            End If

            Return preds
        End Function

        Friend Overridable Function predictRaw(ByVal feat As FVec, ByVal ntree_limit As Integer) As Double()
            Dim preds = gbm.predict(feat, ntree_limit)

            For i = 0 To preds.Length - 1
                preds(i) += mparam.base_score
            Next

            Return preds
        End Function

        ''' <summary>
        ''' Generates a prediction for given feature vector.
        ''' <para>
        ''' This method only works when the model outputs single value.
        ''' </para>
        ''' </summary>
        ''' <paramname="feat"> feature vector </param>
        ''' <returns> prediction value </returns>
        Public Overridable Function predictSingle(ByVal feat As FVec) As Double
            Return predictSingle(feat, False)
        End Function

        ''' <summary>
        ''' Generates a prediction for given feature vector.
        ''' <para>
        ''' This method only works when the model outputs single value.
        ''' </para>
        ''' </summary>
        ''' <paramname="feat">          feature vector </param>
        ''' <paramname="output_margin"> whether to only predict margin value instead of transformed prediction </param>
        ''' <returns> prediction value </returns>
        Public Overridable Function predictSingle(ByVal feat As FVec, ByVal output_margin As Boolean) As Double
            Return predictSingle(feat, output_margin, 0)
        End Function

        ''' <summary>
        ''' Generates a prediction for given feature vector.
        ''' <para>
        ''' This method only works when the model outputs single value.
        ''' </para>
        ''' </summary>
        ''' <paramname="feat">          feature vector </param>
        ''' <paramname="output_margin"> whether to only predict margin value instead of transformed prediction </param>
        ''' <paramname="ntree_limit">   limit the number of trees used in prediction </param>
        ''' <returns> prediction value </returns>
        Public Overridable Function predictSingle(ByVal feat As FVec, ByVal output_margin As Boolean, ByVal ntree_limit As Integer) As Double
            Dim pred = predictSingleRaw(feat, ntree_limit)

            If Not output_margin Then
                Return obj.predTransform(pred)
            End If

            Return pred
        End Function

        Friend Overridable Function predictSingleRaw(ByVal feat As FVec, ByVal ntree_limit As Integer) As Double
            Return gbm.predictSingle(feat, ntree_limit) + mparam.base_score
        End Function

        ''' <summary>
        ''' Predicts leaf index of each tree.
        ''' </summary>
        ''' <paramname="feat"> feature vector </param>
        ''' <returns> leaf indexes </returns>
        Public Overridable Function predictLeaf(ByVal feat As FVec) As Integer()
            Return predictLeaf(feat, 0)
        End Function

        ''' <summary>
        ''' Predicts leaf index of each tree.
        ''' </summary>
        ''' <paramname="feat">        feature vector </param>
        ''' <paramname="ntree_limit"> limit </param>
        ''' <returns> leaf indexes </returns>
        Public Overridable Function predictLeaf(ByVal feat As FVec, ByVal ntree_limit As Integer) As Integer()
            Return gbm.predictLeaf(feat, ntree_limit)
        End Function

        Public Overridable ReadOnly Property sparkModelParam As SparkModelParam
            Get
                Return sparkModelParam_Renamed
            End Get
        End Property

        ''' <summary>
        ''' Returns number of class.
        ''' </summary>
        ''' <returns> number of class </returns>
        Public Overridable ReadOnly Property numClass As Integer
            Get
                Return mparam.num_class
            End Get
        End Property

        ''' <summary>
        ''' Parameters.
        ''' </summary>
        <Serializable>
        Friend Class ModelParam
            ' \brief global bias 
            Friend ReadOnly base_score As Single
            ' \brief number of features  
            Friend ReadOnly num_feature As Integer
            ' \brief number of class, if it is multi-class classification  
            Friend ReadOnly num_class As Integer
            ' ! \brief whether the model itself is saved with pbuffer 
            Friend ReadOnly saved_with_pbuffer As Integer
            ' ! \brief reserved field 
            Friend ReadOnly reserved As Integer()

            'JAVA TO C# CONVERTER CRACKED BY X-CRACKER WARNING: Method 'throws' clauses are not available in .NET:
            'ORIGINAL LINE: ModelParam(float base_score, int num_feature, biz.k11i.xgboost.util.ModelReader reader) throws java.io.IOException
            Friend Sub New(ByVal base_score As Single, ByVal num_feature As Integer, ByVal reader As ModelReader)
                Me.base_score = base_score
                Me.num_feature = num_feature
                num_class = reader.readInt()
                saved_with_pbuffer = reader.readInt()
                reserved = reader.readIntArray(30)
            End Sub
        End Class
    End Class
End Namespace
