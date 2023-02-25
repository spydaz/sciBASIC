﻿#Region "Microsoft.VisualBasic::e60b8ad39a90c810c090f46f60e62eea, sciBASIC#\mime\application%json\Serializer\ObjectSchema.vb"

' Author:
' 
'       asuka (amethyst.asuka@gcmodeller.org)
'       xie (genetics@smrucc.org)
'       xieguigang (xie.guigang@live.com)
' 
' Copyright (c) 2018 GPL3 Licensed
' 
' 
' GNU GENERAL PUBLIC LICENSE (GPL3)
' 
' 
' This program is free software: you can redistribute it and/or modify
' it under the terms of the GNU General Public License as published by
' the Free Software Foundation, either version 3 of the License, or
' (at your option) any later version.
' 
' This program is distributed in the hope that it will be useful,
' but WITHOUT ANY WARRANTY; without even the implied warranty of
' MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
' GNU General Public License for more details.
' 
' You should have received a copy of the GNU General Public License
' along with this program. If not, see <http://www.gnu.org/licenses/>.



' /********************************************************************************/

' Summaries:


' Code Statistics:

'   Total Lines: 126
'    Code Lines: 92
' Comment Lines: 18
'   Blank Lines: 16
'     File Size: 4.48 KB


' Class ObjectSchema
' 
'     Constructor: (+2 Overloads) Sub New
'     Function: CreateSchema, FindInterfaceImpementations, GetSchema, Score, ToString
' 
' /********************************************************************************/

#End Region

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports System.Runtime.Serialization
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.Emit.Delegates
Imports Microsoft.VisualBasic.Linq

Namespace ComponentModel.DataSourceModel.SchemaMaps

    Public Enum Serializations
        JSON
        XML
    End Enum

    ''' <summary>
    ''' object schema provider for json/xml
    ''' </summary>
    Public Class SoapGraph

        Public ReadOnly addMethod As MethodInfo
        Public ReadOnly isTable As Boolean
        Public ReadOnly writers As IReadOnlyDictionary(Of String, PropertyInfo)
        ''' <summary>
        ''' Value type of the dictionary
        ''' </summary>
        Public ReadOnly valueType As Type
        Public ReadOnly keyType As Type
        Public ReadOnly raw As Type

        Public ReadOnly knownTypes As Type()
        Public ReadOnly documentType As Serializations

        Private Sub New()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="addMethod"></param>
        ''' <param name="isTable"></param>
        ''' <param name="writers"></param>
        ''' <param name="valueType"></param>
        Private Sub New(addMethod As MethodInfo,
                        isTable As Boolean,
                        writers As IReadOnlyDictionary(Of String, PropertyInfo),
                        keyType As Type,
                        valueType As Type,
                        raw As Type,
                        knownTypes As Type(),
                        docTyp As Serializations)

            Me.addMethod = addMethod
            Me.isTable = isTable
            Me.writers = writers
            Me.valueType = valueType
            Me.keyType = keyType
            Me.raw = raw
            Me.knownTypes = knownTypes
            Me.documentType = docTyp
        End Sub

        ''' <summary>
        ''' just create a new and blank .net clr object
        ''' </summary>
        ''' <param name="schema"></param>
        ''' <param name="parent"></param>
        ''' <param name="docs"><see cref="Score"/></param>
        ''' <returns></returns>
        Public Function Activate(ByRef schema As SoapGraph, parent As SoapGraph, docs As String()) As Object
            Dim knownType As SoapGraph

            If Not schema.raw.IsInterface AndAlso Not schema.raw Is GetType(Object) Then
                Return Activator.CreateInstance(schema.raw)
            ElseIf schema.raw.IsInterface Then
                knownType = parent _
                .FindInterfaceImplementations(schema.raw) _
                .OrderByDescending(Function(a) a.Score(docs)) _
                .FirstOrDefault

                If knownType Is Nothing Then
                    Throw New InvalidProgramException($"can not create object from an interface type: {schema.raw.FullName}!")
                End If
            Else ' is object
                knownType = parent.knownTypes _
                .Select(Function(t) SoapGraph.GetSchema(t, documentType)) _
                .OrderByDescending(Function(a) a.Score(docs)) _
                .FirstOrDefault

                If knownType Is Nothing Then
                    Throw New InvalidProgramException($"can not create object...")
                End If
            End If

            schema = knownType

            Return Activator.CreateInstance(knownType.raw)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="obj">
        ''' the parsed object member names from the document file, example as
        ''' json field names or xml element names
        ''' </param>
        ''' <returns></returns>
        Public Function Score(obj As IEnumerable(Of String)) As Integer
            Dim hits As Integer = 0

            For Each name As String In obj
                If writers.ContainsKey(name) Then
                    hits += 1
                End If
            Next

            Return hits
        End Function

        ''' <summary>
        ''' get (or cache a new schema graph object if not exists) a schema graph object
        ''' </summary>
        ''' <param name="type"></param>
        ''' <returns></returns>
        Public Shared Function GetSchema(type As Type, Optional serializer As Serializations = Serializations.JSON) As SoapGraph
            Dim key As String = $"<{serializer.ToString}>{type.FullName}"
            Static cache As New Dictionary(Of String, SoapGraph)
            Return cache.ComputeIfAbsent(key:=key, lazyValue:=Function() CreateSchema(type, serializer))
        End Function

        Public Shared Function GetAddMethod(schema As Type) As MethodInfo
            Return schema.GetMethods _
                .Where(Function(m)
                           Dim params = m.GetParameters

                           Return Not m.IsStatic AndAlso
                               Not params.IsNullOrEmpty AndAlso
                               params.Length = 2 AndAlso
                               m.Name = "Add"
                       End Function) _
                .FirstOrDefault
        End Function

        Public Shared Function GetJSONCLRWriters(schema As Type) As Dictionary(Of String, PropertyInfo)
            Return schema.Schema(PropertyAccess.NotSure, PublicProperty, nonIndex:=True)
        End Function

        Public Shared Function GetXmlCLRWriters(schema As Type) As Dictionary(Of String, PropertyInfo)
            Dim properties As PropertyInfo() = schema _
                .GetProperties() _
                .Where(Function(p) p.CanWrite AndAlso p.GetIndexParameters.IsNullOrEmpty) _
                .ToArray
            Dim name As String
            Dim writers As New Dictionary(Of String, PropertyInfo)
            Dim tagName As XmlElementAttribute

            For Each prop As PropertyInfo In properties
                name = Nothing
                tagName = prop.GetCustomAttribute(Of XmlElementAttribute)

                If Not tagName Is Nothing Then
                    name = tagName.ElementName
                End If
                If name.StringEmpty Then
                    name = prop.Name
                End If

                writers.Add(name, prop)
            Next

            Return writers
        End Function

        Private Shared Function CreateSchema(schema As Type, serializer As Serializations) As SoapGraph
            Dim isTable As Boolean = schema.IsInheritsFrom(GetType(DictionaryBase)) OrElse schema.ImplementInterface(GetType(IDictionary))
            Dim writers As Dictionary(Of String, PropertyInfo)
            Dim addMethod As MethodInfo = GetAddMethod(schema)
            Dim keyType As Type = Nothing
            Dim valueType As Type = Nothing

            If isTable Then
                With schema.GetGenericArguments
                    If .Length = 1 Then
                        keyType = GetType(String)
                        valueType = .GetValue(Scan0)
                    Else
                        keyType = .GetValue(Scan0)
                        valueType = .GetValue(1)
                    End If
                End With
            End If

            Select Case serializer
                Case Serializations.JSON : writers = GetJSONCLRWriters(schema)
                Case Serializations.XML : writers = GetXmlCLRWriters(schema)
                Case Else
                    Throw New NotImplementedException(serializer.ToString)
            End Select

            ' for avoid a infinity loop that caused by the circular reference
            ' we just get the types at here
            ' create object schema graph object at required
            Dim knownTypes As Type() = schema _
                .GetCustomAttributes(Of KnownTypeAttribute) _
                .SafeQuery _
                .Select(Function(a) a.Type) _
                .ToArray

            Return New SoapGraph(
                raw:=schema,
                addMethod:=addMethod,
                writers:=writers,
                isTable:=isTable,
                valueType:=valueType,
                keyType:=keyType,
                knownTypes:=knownTypes,
                docTyp:=serializer
            )
        End Function

        ''' <summary>
        ''' Find the implementations of the abstract interface define
        ''' </summary>
        ''' <param name="type"></param>
        ''' <returns></returns>
        Public Iterator Function FindInterfaceImplementations(type As Type) As IEnumerable(Of SoapGraph)
            For Each known As Type In knownTypes
                If known.ImplementInterface(type) Then
                    Yield SoapGraph.GetSchema(known, documentType)
                End If
            Next
        End Function

        Public Overrides Function ToString() As String
            Return $"{raw.Namespace}::{raw.Name}"
        End Function

    End Class
End Namespace