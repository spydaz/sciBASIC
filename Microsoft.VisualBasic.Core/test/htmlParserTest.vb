﻿#Region "Microsoft.VisualBasic::07fd10d466b3e3e3a5c170394dbee05a, sciBASIC#\Microsoft.VisualBasic.Core\test\htmlParserTest.vb"

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

    '   Total Lines: 30
    '    Code Lines: 18
    ' Comment Lines: 0
    '   Blank Lines: 12
    '     File Size: 741 B


    ' Module htmlParserTest
    ' 
    '     Sub: Main, UrlescapeTest
    ' 
    ' /********************************************************************************/

#End Region

Imports Microsoft.VisualBasic.Text.Parser.HtmlParser

Module htmlParserTest

    Sub Main1()

        Call UrlescapeTest()

        Dim page$ = "https://www.ncbi.nlm.nih.gov/nuccore/NC_014248".GET
        Dim metaInfo = page.ParseHtmlMeta

        Pause()

        Dim testhtml = "D:\GCModeller\src\runtime\sciBASIC#\Microsoft.VisualBasic.Core\test\testhtml.txt".ReadAllText

        Dim list = testhtml.HtmlList
        Dim disable = list.Where(Function(li) li.classList.IndexOf("disabled") > -1).FirstOrDefault


        Pause()
    End Sub

    Sub UrlescapeTest()
        Dim token = "Ethinyl estradiol".UrlEncode(jswhitespace:=True)
        Dim plus = "A+B".UrlEncode


        Pause()
    End Sub
End Module
