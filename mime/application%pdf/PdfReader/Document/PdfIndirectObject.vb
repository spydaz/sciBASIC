﻿#Region "Microsoft.VisualBasic::4ea9d1b05754ccc8dc3bc3f4105d0a1e, mime\application%pdf\PdfReader\Document\PdfIndirectObject.vb"

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

    '     Class PdfIndirectObject
    ' 
    '         Properties: Child, Gen, Id, Offset
    ' 
    '         Constructor: (+1 Overloads) Sub New
    '         Sub: Visit
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace PdfReader
    Public Class PdfIndirectObject
        Inherits PdfObject

        Private _Id As Integer, _Gen As Integer, _Offset As Long

        Public Sub New(ByVal parent As PdfObject, ByVal xref As TokenXRefEntry)
            MyBase.New(parent)
            Id = xref.Id
            Gen = xref.Gen
            Offset = xref.Offset
        End Sub

        Public Overrides Sub Visit(ByVal visitor As IPdfObjectVisitor)
            visitor.Visit(Me)
        End Sub

        Public Property Id As Integer
            Get
                Return _Id
            End Get
            Private Set(ByVal value As Integer)
                _Id = value
            End Set
        End Property

        Public Property Gen As Integer
            Get
                Return _Gen
            End Get
            Private Set(ByVal value As Integer)
                _Gen = value
            End Set
        End Property

        Public Property Offset As Long
            Get
                Return _Offset
            End Get
            Private Set(ByVal value As Long)
                _Offset = value
            End Set
        End Property

        Public Property Child As PdfObject
    End Class
End Namespace
