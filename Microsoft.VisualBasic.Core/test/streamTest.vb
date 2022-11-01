﻿Imports Microsoft.VisualBasic.ApplicationServices

Module streamTest

    Public Sub Main1()
        Dim demo As Byte() = New Byte(1024 * 5 + 6) {}
        Dim b As Byte = 0
        Dim test_file As String = $"{App.HOME}/test.dat"

        For i As Integer = 0 To demo.Length - 1
            demo(i) = b

            If b = 255 Then
                b = 0
            Else
                b += 1
            End If
        Next

        demo(1026) = 199
        demo(1027) = 1

        Call demo.FlushStream(test_file)

        Dim stream = MemoryStreamPool.FromFile(test_file, buffer_size:=1024)

        ' seek test
        stream.Position = 1025
        ' p++
        stream.ReadByte()

        ' test stream reader
        Dim b199_1026 As Byte = stream.ReadByte
        Dim b1_1027 As Byte = stream.ReadByte

        If b199_1026 <> 199 OrElse b1_1027 <> 1 Then
            Throw New InvalidProgramException
        End If

        Pause()
    End Sub
End Module