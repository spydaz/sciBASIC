﻿#Region "Microsoft.VisualBasic::a3e732bfb65b63a05b72bcf4e2d75d6a, Data\GraphQuery\Query\Query.vb"

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

    ' Class Query
    ' 
    '     Properties: isArray, members, name, parser
    ' 
    '     Function: ToString
    ' 
    '     Sub: Add
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.Text

''' <summary>
''' the object model of a query
''' </summary>
Public Class Query

    ''' <summary>
    ''' the query name
    ''' </summary>
    ''' <returns></returns>
    Public Property name As String
    Public Property parser As Parser
    Public Property isArray As Boolean

    Dim m_memberList As New List(Of Query)

    Public Property members As Query()
        Get
            Return m_memberList.ToArray
        End Get
        Set(value As Query())
            m_memberList = New List(Of Query)(value)
        End Set
    End Property

    Public Sub Add(member As Query)
        m_memberList.Add(member)
    End Sub

    Public Overrides Function ToString() As String
        Dim sb As New StringBuilder

        Call sb.Append(name)

        If isArray Then
            Call sb.Append("[]")
        End If

        If Not parser Is Nothing Then
            Call sb.Append(": ")
            Call sb.Append(parser.ToString)
        End If

        If m_memberList.Count > 0 Then
            sb.Append(" ")
            sb.Append("{")
            sb.Append(members.JoinBy(", "))
            sb.Append("}")
        End If

        Return sb.ToString
    End Function

End Class


