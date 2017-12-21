﻿#Region "Microsoft.VisualBasic::9dfaf48a2bdadbdbe61a863d65b9e74b, ..\sciBASIC#\Microsoft.VisualBasic.Architecture.Framework\Text\Xml\Models\Models.vb"

    ' Author:
    ' 
    '       asuka (amethyst.asuka@gcmodeller.org)
    '       xieguigang (xie.guigang@live.com)
    '       xie (genetics@smrucc.org)
    ' 
    ' Copyright (c) 2016 GPL3 Licensed
    ' 
    ' 
    ' GNU GENERAL PUBLIC LICENSE (GPL3)
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

#End Region

Imports System.Runtime.CompilerServices
Imports System.Xml.Serialization
Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel
Imports Microsoft.VisualBasic.ComponentModel.DataSourceModel.Repository
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Serialization.JSON

Namespace Text.Xml.Models

    Public Class StringValue : Implements Value(Of String).IValueOf

        <XmlAttribute> Public Property value As String Implements Value(Of String).IValueOf.Value

        Public Overrides Function ToString() As String
            Return value
        End Function
    End Class

    ''' <summary>
    ''' 键值对集合的键值<see cref="text"/>可能是一大段文本
    ''' </summary>
    <XmlType("data")> Public Structure NamedValue
        Implements INamedValue
        Implements Value(Of String).IValueOf

        <XmlAttribute> Public Property name As String Implements INamedValue.Key
        <XmlText> Public Property text As String Implements Value(Of String).IValueOf.Value

        Sub New(name$, value$)
            Me.name = name
            Me.text = value
        End Sub

        <MethodImpl(MethodImplOptions.AggressiveInlining)>
        Public Overrides Function ToString() As String
            Return Me.GetJson
        End Function
    End Structure

    ''' <summary>
    ''' Property Info (Property Name and Property Value).
    ''' (和<see cref="NamedValue"/>所不同的是，这个对象之中的键值对集合的键值都是小段字符串)
    ''' </summary>
    Public Structure [Property]

        <XmlAttribute> Public Property name As String
        <XmlAttribute> Public Property value As String
        <XmlText>
        Public Property Comment As String

        Sub New(name$, value$, comment$)
            Me.name = name
            Me.value = value
            Me.Comment = comment
        End Sub

        Public Overrides Function ToString() As String
            Return $"{name} = ""{value}"""
        End Function
    End Structure

    ''' <summary>
    ''' 代码行的模型？
    ''' </summary>
    Public Structure LineValue

        <XmlAttribute>
        Public Property line As Integer
        <XmlText>
        Public Property text As String

        Public Overrides Function ToString() As String
            Return $"[{line}]  {text}"
        End Function
    End Structure

    ''' <summary>
    ''' 在这里不实现<see cref="IEnumerable(Of T)"/>是为了方便的实现XML序列化操作
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    Public Structure NamedVector(Of T)
        Implements INamedValue

        <XmlAttribute>
        Public Property name As String Implements IKeyedEntity(Of String).Key
        Public Property vector As T()
        Public Property attributes As NamedValue()

        Sub New(namedCollection As NamedCollection(Of T))
            With namedCollection
                name = .Name
                vector = .Value
            End With
        End Sub

        Public Overrides Function ToString() As String
            Return name
        End Function
    End Structure
End Namespace