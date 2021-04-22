﻿Imports System.Collections.Generic

Namespace PdfReader
    Public Class PdfNameTree
        Inherits PdfObject

        Private _LimitMin As String, _LimitMax As String
        Private ReadOnly _root As Boolean
        Private _dictionary As PdfDictionary
        Private _children As List(Of PdfNameTree)
        Private _names As Dictionary(Of String, PdfObject)

        Public Sub New(ByVal dictionary As PdfDictionary, ByVal Optional root As Boolean = True)
            MyBase.New(dictionary.Parent)
            _dictionary = dictionary
            _root = root

            If _root Then
                Load()
            Else
                Dim limits = _dictionary.MandatoryValue(Of PdfArray)("Limits")
                LimitMin = CType(limits.Objects(0), PdfString).Value
                LimitMax = CType(limits.Objects(1), PdfString).Value
            End If
        End Sub

        Public Overrides Sub Visit(ByVal visitor As IPdfObjectVisitor)
            visitor.Visit(Me)
        End Sub

        Public Property LimitMin As String
            Get
                Return _LimitMin
            End Get
            Private Set(ByVal value As String)
                _LimitMin = value
            End Set
        End Property

        Public Property LimitMax As String
            Get
                Return _LimitMax
            End Get
            Private Set(ByVal value As String)
                _LimitMax = value
            End Set
        End Property

        Default Public ReadOnly Property Item(ByVal name As String) As PdfObject
            Get
                Dim ret As PdfObject = Nothing

                If String.Compare(name, LimitMin) >= 0 AndAlso String.Compare(name, LimitMax) <= 0 Then
                    If _names Is Nothing AndAlso _children Is Nothing Then Load()

                    If _names IsNot Nothing Then
                        _names.TryGetValue(name, ret)
                    Else
                        ' Linear search, could improve perf by using a binary search
                        For Each child In _children

                            If String.Compare(name, child.LimitMin) >= 0 AndAlso String.Compare(name, child.LimitMax) <= 0 Then
                                ret = child(name)
                                Exit For
                            End If
                        Next
                    End If
                End If

                Return ret
            End Get
        End Property

        Private Sub Load()
            Dim kids = _dictionary.OptionalValue(Of PdfArray)("Kids")

            If kids IsNot Nothing Then
                ' Must load all the children as objects immediately, so we can then calculate the overall limits
                _children = New List(Of PdfNameTree)()

                For Each reference As PdfObjectReference In kids.Objects
                    _children.Add(New PdfNameTree(Document.IndirectObjects.MandatoryValue(Of PdfDictionary)(reference), False))
                Next

                ' Only the root calculates the limits by examining the children
                If _root Then
                    LimitMin = _children(0).LimitMin
                    LimitMax = _children(_children.Count - 1).LimitMax
                End If
            Else
                ' Without 'Kids' the 'Names' is mandatory
                Dim array = _dictionary.MandatoryValue(Of PdfArray)("Names")
                _names = New Dictionary(Of String, PdfObject)()
                Dim count = array.Objects.Count

                For i = 0 To count - 1 Step 2
                    Dim key = CType(array.Objects(i), PdfString)
                    _names.Add(key.Value, array.Objects(i + 1))

                    ' Only the root calculates the limits by examining the enties
                    If _root Then
                        If i = 0 Then
                            LimitMin = key.Value
                        ElseIf i = count - 1 Then
                            LimitMax = key.Value
                        End If
                    End If
                Next
            End If
        End Sub
    End Class
End Namespace
