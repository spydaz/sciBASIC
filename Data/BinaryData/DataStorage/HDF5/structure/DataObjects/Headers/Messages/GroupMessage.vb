﻿#Region "Microsoft.VisualBasic::44006d9ef0d2fa1ede83ee27850a4130, Data\BinaryData\DataStorage\HDF5\structure\DataObjects\Headers\Messages\GroupMessage.vb"

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

'     Class GroupMessage
' 
'         Properties: bTreeAddress, nameHeapAddress
' 
'         Constructor: (+1 Overloads) Sub New
' 
'         Function: ToString
' 
'         Sub: printValues
' 
' 
' /********************************************************************************/

#End Region

'
' * Mostly copied from NETCDF4 source code.
' * refer : http://www.unidata.ucar.edu
' * 
' * Modified by iychoi@email.arizona.edu
' 


Imports Microsoft.VisualBasic.Data.IO.HDF5.IO
Imports BinaryReader = Microsoft.VisualBasic.Data.IO.HDF5.IO.BinaryReader

Namespace HDF5.[Structure]

    ''' <summary>
    ''' The Symbol Table Message
    ''' </summary>
    Public Class GroupMessage : Inherits Message

        ''' <summary>
        ''' This value is the address of the v1 B-tree containing the symbol table 
        ''' entries for the group.
        ''' </summary>
        ''' <returns></returns>
        Public Overridable ReadOnly Property bTreeAddress() As Long

        ''' <summary>
        ''' This value is the address of the local heap containing the link names 
        ''' for the symbol table entries for the group.
        ''' </summary>
        ''' <returns></returns>
        Public Overridable ReadOnly Property nameHeapAddress() As Long

        Public Sub New([in] As BinaryReader, sb As Superblock, address As Long)
            Call MyBase.New(address)

            [in].offset = address

            Me.bTreeAddress = ReadHelper.readO([in], sb)
            Me.nameHeapAddress = ReadHelper.readO([in], sb)
        End Sub

        Public Overrides Function ToString() As String
            Return $"{MyBase.ToString} {bTreeAddress} -> {nameHeapAddress}"
        End Function

        Protected Friend Overrides Sub printValues(console As System.IO.StringWriter)
            console.WriteLine("GroupMessage >>>")
            console.WriteLine("address : " & Me.m_address)
            Console.WriteLine("btree address : " & Me.bTreeAddress)
            Console.WriteLine("nameheap address : " & Me.nameHeapAddress)
            Console.WriteLine("GroupMessage <<<")
        End Sub

    End Class

End Namespace
