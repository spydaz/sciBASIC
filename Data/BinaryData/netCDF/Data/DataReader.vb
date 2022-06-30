﻿#Region "Microsoft.VisualBasic::a52ee04a484c4abd75a20b6f5dc18edb, sciBASIC#\Data\BinaryData\DataStorage\netCDF\Data\DataReader.vb"

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

'   Total Lines: 104
'    Code Lines: 58
' Comment Lines: 34
'   Blank Lines: 12
'     File Size: 4.45 KB


'     Module DataReader
' 
'         Function: CreateArray, nonRecord, record
' 
' 
' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.DataStorage.netCDF.Components
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Linq

Namespace Data

    ''' <summary>
    ''' Data reader methods for a given variable data value.
    ''' (在这个模块之中读取<see cref="variable.value"/>数据变量的值)
    ''' </summary>
    Module DataReader

        <Extension>
        Private Function CreateArray(x As variable, size As Integer) As Array
            Select Case x.type
                Case CDFDataTypes.BOOLEAN : Return New Boolean(size - 1) {}
                Case CDFDataTypes.BYTE : Return New Byte(size - 1) {}
                Case CDFDataTypes.CHAR : Return New Char(size - 1) {}
                Case CDFDataTypes.DOUBLE : Return New Double(size - 1) {}
                Case CDFDataTypes.FLOAT : Return New Single(size - 1) {}
                Case CDFDataTypes.INT : Return New Integer(size - 1) {}
                Case CDFDataTypes.LONG : Return New Long(size - 1) {}
                Case CDFDataTypes.SHORT : Return New Short(size - 1) {}
                Case Else
                    Throw New InvalidDataException("invalid data type!")
            End Select
        End Function

        ''' <summary>
        ''' Read data for the given non-record variable
        ''' </summary>
        ''' <param name="buffer">Buffer for the file data</param>
        ''' <param name="variable">Variable metadata</param>
        ''' <returns>Data of the element</returns>
        ''' <remarks>
        ''' 非记录类型则是一个数组
        ''' </remarks>
        Public Function nonRecord(buffer As BinaryDataReader, variable As variable) As Array
            ' size of the data
            Dim size As Integer = variable.size / sizeof(variable.type)
            ' iterates over the data
            Dim data As Array = buffer.readVector(size, variable.type) ' variable.CreateArray(size)

            ' 读取的结果是一个T()数组
            ' For i As Integer = 0 To size - 1
            ' data(i) = Utils.readType(buffer, variable.type, 1)
            ' Next

            Return data
        End Function

        ''' <summary>
        ''' Read data for the given record variable
        ''' (读取结构体数据?)
        ''' </summary>
        ''' <param name="buffer">Buffer for the file data</param>
        ''' <param name="variable">Variable metadata</param>
        ''' <param name="recordDimension">Record dimension metadata</param>
        ''' <returns>Data of the element</returns>
        ''' <remarks>
        ''' 记录类型的数据可能是一个矩阵类型
        ''' </remarks>
        Public Function record(buffer As BinaryDataReader, variable As variable, recordDimension As recordDimension) As Array
            Dim width% = If(variable.size, variable.size / sizeof(variable.type), 1)
            ' size of the data
            ' TODO streaming data
            Dim size As Integer = recordDimension.length
            ' iterates over the data
            Dim data As Array = Array.CreateInstance(variable.type.ToType, size)
            Dim [step] As Integer = recordDimension.recordStep
            Dim reader As Func(Of Byte(), Object) = Utils.GetRecordReader(variable.type)
            Dim base As Stream = buffer.BaseStream
            Dim chunkSize As Long = [step] * size
            Dim mem As Byte() = New Byte(chunkSize - 1) {}
            Dim i As i32 = Scan0
            Dim parallel As Boolean = size >= 100000

            ' 20220630
            ' why needs offset of 4 bytes?
            Call base.Seek(4, SeekOrigin.Current)
            Call base.Read(mem, Scan0, chunkSize)

            ' 读取的结果可能是一个T()()矩阵或者T()数组

            If parallel Then
                For Each item As SeqValue(Of Object) In mem _
                    .SplitIterator([step]) _
                    .SeqIterator _
                    .AsParallel _
                    .Select(Function(j)
                                Return New SeqValue(Of Object)(j.i, reader(j.value))
                            End Function)

                    data(item.i) = item.value
                Next
            Else
                For Each block As Byte() In mem.SplitIterator([step])
                    data(++i) = reader(block)
                Next
            End If

            Return data
        End Function
    End Module
End Namespace
