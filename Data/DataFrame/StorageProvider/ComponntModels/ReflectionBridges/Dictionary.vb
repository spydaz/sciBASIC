﻿#Region "Microsoft.VisualBasic::853113e2b5a45591c2f8cb33fc040466, sciBASIC#\Data\DataFrame\StorageProvider\ComponntModels\ReflectionBridges\Dictionary.vb"

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

    '   Total Lines: 40
    '    Code Lines: 30
    ' Comment Lines: 0
    '   Blank Lines: 10
    '     File Size: 1.30 KB


    '     Class MetaAttribute
    ' 
    '         Properties: Dictionary, MetaAttribute, Name, ProviderId
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Function: CreateDictionary, ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Reflection
Imports Microsoft.VisualBasic.Data.csv.StorageProvider.Reflection

Namespace StorageProvider.ComponentModels

    Public Class MetaAttribute : Inherits StorageProvider

        Public Property MetaAttribute As Reflection.MetaAttribute

        Public Overrides ReadOnly Property Name As String
            Get
                Return BindProperty.Name
            End Get
        End Property

        Public ReadOnly Property Dictionary As Type

        Public Overrides ReadOnly Property ProviderId As ProviderIds
            Get
                Return ProviderIds.MetaAttribute
            End Get
        End Property

        Sub New(attr As Reflection.MetaAttribute, BindProperty As PropertyInfo)
            Call MyBase.New(BindProperty, attr.TypeId)

            Me.MetaAttribute = attr
            Me.Dictionary = GetType(Dictionary(Of ,)) _
                .MakeGenericType(GetType(String), attr.TypeId)
        End Sub

        Public Function CreateDictionary() As IDictionary
            Return DirectCast(Activator.CreateInstance(Dictionary), IDictionary)
        End Function

        Public Overrides Function ToString([object] As Object) As String
            Return ""
        End Function
    End Class
End Namespace
