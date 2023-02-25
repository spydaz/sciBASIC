﻿Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.SchemaMaps
Imports Microsoft.VisualBasic.Linq

Public Class GraphWriter

    ReadOnly graph As SoapGraph

    Sub New(type As Type)
        graph = SoapGraph.GetSchema(type, Serializations.XML)
    End Sub

    Public Function Load(xml As XmlElement) As Object
        Return loadGraphTree(xml, graph)
    End Function

    Private Shared Function loadGraphTree(xml As XmlElement, parent As SoapGraph) As Object
        Dim members = xml.elements _
           .SafeQuery _
           .GroupBy(Function(xi) xi.name) _
           .ToDictionary(Function(xi) xi.Key,
                         Function(xi)
                             Return xi.ToArray
                         End Function)
        Dim obj As Object = parent.Activate(parent:=parent, docs:=members.Keys.ToArray, schema:=parent)

        Return obj
    End Function

    Public Shared Function LoadXml(Of T)(xml As String) As T
        Dim doc As XmlElement = XmlElement.ParseXmlText(xml.SolveStream)
        Dim writer As New GraphWriter(GetType(T))
        Dim obj As Object = writer.Load(doc)

        Return obj
    End Function

End Class
