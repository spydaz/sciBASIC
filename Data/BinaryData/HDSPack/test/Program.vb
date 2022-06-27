﻿Imports System
Imports System.Text
Imports Microsoft.VisualBasic.DataStorage.HDSPack

Module Program

    ReadOnly testfile As String = "./test.hds"

    Sub Main(args As String())
        Call writePackTest()
        Call readPackTest()
    End Sub

    Sub readPackTest()
        Using hds As New StreamPack(testfile)
            Dim buf = hds.OpenBlock("/another_folder/text_data/\GCModeller\src\runtime\sciBASIC#\etc\(๑•̀ㅂ•́)و✧.svg")
            Dim bytes As Byte() = New Byte(buf.Length - 1) {}

            Call buf.Read(bytes, Scan0, bytes.Length)

            Dim xml As String = Encoding.UTF8.GetString(bytes)

            Call Console.WriteLine(xml)
            Call xml.SaveTo("./test_text_exports.svg")

            buf = hds.OpenBlock("/another_folder/text_data/data.txt")
            bytes = New Byte(buf.Length - 1) {}

            Call buf.Read(bytes, Scan0, bytes.Length)

            Call Console.WriteLine()
            Call Console.WriteLine(Encoding.Unicode.GetString(bytes))
        End Using
    End Sub

    Sub writePackTest()
        Using hds As StreamPack = StreamPack.CreateNewStream(testfile)
            Dim image = "D:\GCModeller\src\runtime\sciBASIC#\etc\ch07_18.png".ReadBinary
            Dim block = hds.OpenBlock("/path/to/the/image/file/ch07-18.png")

            Call block.Write(image, Scan0, image.Length)
            Call block.Flush()
            Call block.Dispose()

            Dim textBuf As Byte() = "D:\GCModeller\src\runtime\sciBASIC#\etc\(๑•̀ㅂ•́)و✧.svg".ReadBinary
            Dim block2 = hds.OpenBlock("/another_folder/text_data/\GCModeller\src\runtime\sciBASIC#\etc\(๑•̀ㅂ•́)و✧.svg")

            Call block2.Write(textBuf, Scan0, textBuf.Length)
            Call block2.Flush()
            Call block2.Dispose()

            textBuf = Encoding.Unicode.GetBytes("你好，世界（Hello World！）")
            block2 = hds.OpenBlock("/another_folder/text_data/data.txt")

            Call block2.Write(textBuf, Scan0, textBuf.Length)
            Call block2.Flush()
            Call block2.Dispose()
        End Using
    End Sub
End Module
