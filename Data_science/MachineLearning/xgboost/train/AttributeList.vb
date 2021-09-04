﻿Namespace train
    Public Class AttributeList
        Public feature_dim As Integer
        Private attribute_list As Single()()()
        Public missing_value_attribute_list As Integer()()
        Public cutting_inds As Integer()()()
        Public cutting_thresholds As Single()()
        Public origin_feature As Single()()
        Public cat_features_cols As List(Of Integer?)

        Public Sub New(ByVal data As TrainData)
            missing_value_attribute_list = data.missing_index
            feature_dim = data.feature_dim
            attribute_list = data.feature_value_index
            origin_feature = data.origin_feature
            cat_features_cols = data.cat_features_cols
            sort_attribute_list()
            initialize_cutting_inds_thresholds()
            clean_up()
        End Sub

        'pre-sort: for each feature,sort (value,index) by the value
        Private Sub sort_attribute_list()
            For i = 0 To feature_dim - 1
                Arrays.sort(attribute_list(i), New ComparatorAnonymousInnerClass(Me))
            Next
        End Sub

        Private Class ComparatorAnonymousInnerClass
            Implements IComparer(Of Single())

            Private ReadOnly outerInstance As AttributeList

            Public Sub New(ByVal outerInstance As AttributeList)
                Me.outerInstance = outerInstance
            End Sub

            Public Overridable Function compare(ByVal a As Single(), ByVal b As Single()) As Integer
                Return a(0).CompareTo(b(0))
            End Function
        End Class

        Private Sub initialize_cutting_inds_thresholds()
            cutting_inds = New Integer(feature_dim - 1)()() {}
            cutting_thresholds = New Single(feature_dim - 1)() {}

            For i = 0 To feature_dim - 1
                'for this feature, get its cutting index
                Dim list As List(Of Integer?) = New List(Of Integer?)()
                Dim last_index = 0

                For j = 0 To attribute_list(i).Length - 1

                    If attribute_list(i)(j)(0) = attribute_list(i)(last_index)(0) Then
                        last_index = j
                    Else
                        list.Add(last_index)
                        last_index = j
                    End If
                Next
                'for this feature,store its cutting threshold
                cutting_thresholds(i) = New Single(list.Count + 1 - 1) {}

                For t = 0 To cutting_thresholds(i).Length - 1 - 1
                    cutting_thresholds(i)(t) = attribute_list(i)(list(t))(0)
                Next

                cutting_thresholds(i)(list.Count) = attribute_list(i)(list(list.Count - 1) + 1)(0)

                'for this feature,store inds of each interval
                cutting_inds(i) = New Integer(list.Count + 1 - 1)() {} 'list.size()+1 interval
                list.Insert(0, -1)
                list.Add(attribute_list(i).Length - 1)

                For k = 0 To cutting_inds(i).Length - 1
                    Dim start_ind As Integer = list(k) + 1
                    Dim end_ind = list(k + 1).Value
                    cutting_inds(i)(k) = New Integer(end_ind - start_ind + 1 - 1) {}

                    For m = 0 To cutting_inds(i)(k).Length - 1
                        cutting_inds(i)(k)(m) = CInt(attribute_list(i)(start_ind + m)(1))
                    Next
                Next
            Next
        End Sub

        Private Sub clean_up()
            attribute_list = Nothing
        End Sub
    End Class
End Namespace
