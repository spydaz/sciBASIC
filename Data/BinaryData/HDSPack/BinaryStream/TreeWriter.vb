﻿Imports System.IO
Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Data.IO
Imports Microsoft.VisualBasic.Text

Friend Module TreeWriter

    <Extension>
    Public Function GetBuffer(root As StreamGroup, type As Index(Of String)) As Byte()
        Using ms As New MemoryStream, bin As New BinaryDataWriter(ms, encoding:=Encodings.UTF8WithoutBOM)
            Dim buf As Byte()
            Dim attrs As Byte() = root.Pack(type)

            Call bin.Write(root.files.Length)
            Call bin.Write(root.referencePath.ToString, BinaryStringFormat.ZeroTerminated)
            Call bin.Write(attrs.Length)
            Call bin.Write(attrs)

            For Each file As StreamObject In root.files
                If TypeOf file Is StreamGroup Then
                    buf = DirectCast(file, StreamGroup).GetBuffer(type)
                    bin.Write(DirectCast(file, StreamGroup).files.Length + 1)
                Else
                    buf = DirectCast(file, StreamBlock).GetBuffer(type)
                    bin.Write(0)
                End If

                bin.Write(buf)
                bin.Flush()
            Next

            Return ms.ToArray
        End Using
    End Function

    <Extension>
    Private Function GetBuffer(file As StreamBlock, type As Index(Of String)) As Byte()
        Using ms As New MemoryStream, bin As New BinaryDataWriter(ms, encoding:=Encodings.UTF8WithoutBOM)
            Dim attrs As Byte() = file.Pack(type)

            Call bin.Write(file.referencePath.ToString, BinaryStringFormat.ZeroTerminated)
            Call bin.Write(file.offset)
            Call bin.Write(file.size)
            Call bin.Write(attrs.Length)
            Call bin.Write(attrs)
            Call bin.Flush()

            Return ms.ToArray
        End Using
    End Function

End Module
