﻿Imports System.Text
Imports Microsoft.VisualBasic.DataStorage.HDSPack
Imports Microsoft.VisualBasic.DataStorage.HDSPack.FileSystem
Imports Microsoft.VisualBasic.Text.Xml.Models

Module Program

    ReadOnly testfile As String = "./test.hds"

    Sub Main(args As String())
        Call writePackTest()
        Call readPackTest()
        Call filesystemTest()
    End Sub

    Sub filesystemTest()
        Using hds As New StreamPack(testfile)
            For Each file In hds.ListFiles
                Call Console.WriteLine(file.ToString)
            Next
        End Using
    End Sub

    Sub readPackTest()
        Using hds As New StreamPack(testfile)
            Dim buf = hds.OpenBlock("/another_folder/text_data/\GCModeller\src\runtime\sciBASIC#\etc\(๑•̀ㅂ•́)و✧.svg")
            Dim bytes As Byte() = New Byte(buf.Length - 1) {}
            Dim obj = hds.GetObject("/another_folder/text_data/\GCModeller\src\runtime\sciBASIC#\etc\(๑•̀ㅂ•́)و✧.svg")

            Call buf.Read(bytes, Scan0, bytes.Length)

            Dim xml As String = Encoding.UTF8.GetString(bytes)

            Call Console.WriteLine(xml)
            Call xml.SaveTo("./test_text_exports.svg")

            buf = hds.OpenBlock("/another_folder/text_data/data.txt")
            bytes = New Byte(buf.Length - 1) {}

            Call buf.Read(bytes, Scan0, bytes.Length)

            Dim str = Encoding.Unicode.GetString(bytes)

            Call Console.WriteLine()
            Call Console.WriteLine(str)

            buf = hds.OpenBlock("/root_text.txt")
            bytes = New Byte(buf.Length - 1) {}

            Call buf.Read(bytes, Scan0, bytes.Length)

            str = Encoding.UTF32.GetString(bytes)

            Call Console.WriteLine()
            Call Console.WriteLine(str)

            Dim obj2 = hds.GetObject("/another_folder\text_data\\\\\\\data.txt")

            Console.WriteLine(obj2.description)
            Console.WriteLine(obj2.GetAttribute("aabbcc"))
            Console.WriteLine(obj2.GetAttribute("time"))
            Console.WriteLine(obj2.GetAttribute("empty"))
        End Using
    End Sub

    Sub writePackTest()
        Using hds As StreamPack = StreamPack.CreateNewStream(testfile)
            Dim image = "/GCModeller\src\runtime\sciBASIC#\etc\ch07_18.png".ReadBinary
            Dim block = hds.OpenBlock("/path/to/the/image/file/ch07-18.png")

            Call block.Write(image, Scan0, image.Length)
            Call block.Flush()
            Call block.Dispose()

            image = "/GCModeller\src\runtime\sciBASIC#\etc\ggplot2.png".ReadBinary
            block = hds.OpenBlock("/another_folder\ggplot-logo.png")

            Call block.Write(image, Scan0, image.Length)
            Call block.Flush()
            Call block.Dispose()

            image = "/GCModeller\src\GCModeller.sln".ReadBinary

            block = hds.OpenBlock("/another_folder/git/////////////\gcmodeller.sln")

            Call block.Write(image, Scan0, image.Length)
            Call block.Flush()
            Call block.Dispose()

            Dim textBuf As Byte() = "/GCModeller\src\runtime\sciBASIC#\etc\(๑•̀ㅂ•́)و✧.svg".ReadBinary
            Dim block2 = hds.OpenBlock("/another_folder/text_data/\GCModeller\src\runtime\sciBASIC#\etc\(๑•̀ㅂ•́)و✧.svg")

            Call block2.Write(textBuf, Scan0, textBuf.Length)
            Call block2.Flush()
            Call block2.Dispose()

            textBuf = Encoding.Unicode.GetBytes("你好，世界（Hello World！）")
            block2 = hds.OpenBlock("/another_folder/text_data/data.txt")

            Call block2.Write(textBuf, Scan0, textBuf.Length)
            Call block2.Flush()
            Call block2.Dispose()

            Call hds.SetAttribute("/another_folder/text_data/data.txt", New Dictionary(Of String, Object) From {
                {"description", "我也不知道要写什么"},
                {"aabbcc", "ccbbAA"},
                {"time", Now},
                {"ints", {56, 986, 44, 8, 8888}},
                {"tuple", New NamedValue With {.name = "aaa", .text = "cccccccccccccccccccccccccccccccccccccccccccccc,还好啦"}},
                {"empty", Nothing}
            })

            textBuf = Encoding.UTF32.GetBytes("我是谁？（Who am I）")

            block2 = hds.OpenBlock("/root_text.txt")

            Call block2.Write(textBuf, Scan0, textBuf.Length)
            Call block2.Flush()
            Call block2.Dispose()

            image = "/GCModeller\src\runtime\sciBASIC#\mime\PDF32000_2008.pdf".ReadBinary
            block = hds.OpenBlock("/another_folder\lagerfile.pdf")

            Call block.Write(image, Scan0, image.Length)
            Call block.Flush()
            Call block.Dispose()

            image = "/mzkit\Rscript\Library\mzkit_app\data\KEGG_maps.msgpack".ReadBinary
            block = hds.OpenBlock("/another_folder\lager2\file.pdf")

            Call block.Write(image, Scan0, image.Length)
            Call block.Flush()
            Call block.Dispose()

            Call hds.WriteText("Hello world! this is a note text about the root directory!", "/readme.txt")

        End Using
    End Sub
End Module
