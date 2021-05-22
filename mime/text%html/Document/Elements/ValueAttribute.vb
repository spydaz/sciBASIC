﻿#Region "Microsoft.VisualBasic::7fb003dce9361a463b3bf1aa443d9420, mime\text%html\Document\Elements\ValueAttribute.vb"

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

    '     Structure ValueAttribute
    ' 
    '         Properties: IsEmpty, Name, Value, Values
    ' 
    '         Constructor: (+2 Overloads) Sub New
    '         Function: ToString
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.ComponentModel.Collection.Generic
Imports Microsoft.VisualBasic.Language.Default

Namespace Document

    Public Structure ValueAttribute : Implements INamedValue, IsEmpty

        Public Property Name As String Implements INamedValue.Key
        Public Property Values As List(Of String)

        Public ReadOnly Property IsEmpty As Boolean Implements IsEmpty.IsEmpty
            Get
                Return Name.StringEmpty AndAlso Values.IsNullOrEmpty
            End Get
        End Property

        Public ReadOnly Property Value As String
            Get
                Return Values?.FirstOrDefault
            End Get
        End Property

        Sub New(strText As String)
            Dim ep As Integer = InStr(strText, "=")
            Name = Mid(strText, 1, ep - 1)
            Dim Value = Mid(strText, ep + 1)
            If Value.First = """"c AndAlso Value.Last = """"c Then
                Value = Mid(Value, 2, Len(Value) - 2)
            End If

            Values = New List(Of String) From {Value}
        End Sub

        Sub New(name As String, value As String)
            Me.Name = name
            Me.Values = New List(Of String) From {value}
        End Sub

        Public Overrides Function ToString() As String
            Return $"{Name}={Values.Select(Function(v) $"""{v}""").JoinBy(", ")}"
        End Function
    End Structure

End Namespace
