﻿Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Microsoft.VisualBasic.Linq

Public Class GraphWriter

    ReadOnly graph As SoapGraph

    Sub New(type As Type)
        graph = SoapGraph.GetSchema(type, Serializations.XML)
    End Sub

    <MethodImpl(MethodImplOptions.AggressiveInlining)>
    Public Function Load(xml As XmlElement) As Object
        Return loadGraphTree(xml, graph)
    End Function

    Private Shared Function loadGraphTree(xml As XmlElement, parent As SoapGraph) As Object
        Dim members As New Dictionary(Of String, XmlElement())

        If Not xml.elements.IsNullOrEmpty Then
            members = xml.elements _
               .GroupBy(Function(xi) xi.name) _
               .ToDictionary(Function(xi) xi.Key,
                             Function(xi)
                                 Return xi.ToArray
                             End Function)
        End If

        Dim obj As Object = parent.Activate(
            parent:=parent,
            docs:=members.Keys.ToArray,
            schema:=parent
        )

        For Each objVal In members
            If parent.writers.ContainsKey(objVal.Key) Then
                Call WriteValue(
                    objKey:=objVal.Key,
                    obj:=obj,
                    docs:=objVal.Value,
                    define:=parent.writers(objVal.Key)
                )
            Else
                Call $"missing {xml.name}::{objVal.Key} from schema {parent.ToString}".Warning
            End If
        Next

        Return obj
    End Function

    Private Shared Sub WriteValue(objKey As String, obj As Object, docs As XmlElement(), define As PropertyInfo)
        Dim value As Object

        If Not define.PropertyType.IsArray Then
            If docs.Length > 1 Then
                ' warning
                Call $"{objKey}(array) -> {define.Name}(scalar) type mis-matched!".Warning
            End If

            value = loadGraphTree(
                xml:=docs(Scan0),
                parent:=SoapGraph.GetSchema(define.PropertyType, Serializations.XML)
            )
        Else
            Dim element As Type = define.PropertyType.GetElementType
            Dim array As Array = Array.CreateInstance(element, docs.Length)
            Dim elementGraph = SoapGraph.GetSchema(element, Serializations.XML)

            For i As Integer = 0 To array.Length - 1
                value = loadGraphTree(docs(i), elementGraph)
                array.SetValue(value, i)
            Next

            value = array
        End If

        Call define.SetValue(obj, value)
    End Sub

    Public Shared Function LoadXml(Of T)(xml As String) As T
        Dim doc As XmlElement = XmlElement.ParseXmlText(xml.SolveStream)
        Dim writer As New GraphWriter(GetType(T))
        Dim obj As Object = writer.Load(doc)

        Return obj
    End Function

End Class
