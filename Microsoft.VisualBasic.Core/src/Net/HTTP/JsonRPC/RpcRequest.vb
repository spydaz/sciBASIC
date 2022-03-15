﻿#Region "Microsoft.VisualBasic::18405d65aa3be87564e42bfce65fa3a8, sciBASIC#\Microsoft.VisualBasic.Core\src\Net\HTTP\JsonRPC\RpcRequest.vb"

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

    '   Total Lines: 13
    '    Code Lines: 8
    ' Comment Lines: 0
    '   Blank Lines: 5
    '     File Size: 286.00 B


    '     Class RpcRequest
    ' 
    '         Properties: id, jsonrpc, method, params
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Namespace Net.Http.JsonRPC

    Public Class RpcRequest

        Public Property jsonrpc As String
        Public Property method As String
        Public Property params As Dictionary(Of String, Object)
        Public Property id As Integer

    End Class
End Namespace



