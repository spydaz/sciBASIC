﻿#Region "Microsoft.VisualBasic::9bc73033c5aafb7d024daec3c94433eb, sciBASIC#\Microsoft.VisualBasic.Core\src\Text\Paragraph.vb"

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

'   Total Lines: 92
'    Code Lines: 64
' Comment Lines: 16
'   Blank Lines: 12
'     File Size: 3.47 KB


'     Module Paragraph
' 
'         Function: Chunks, SplitParagraph, Trim
' 
' 
' /********************************************************************************/

#End Region

Imports System.Runtime.CompilerServices
Imports Microsoft.VisualBasic.ComponentModel.Collection
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Text.Parser

Namespace Text

    Public Module Paragraph

        ''' <summary>
        ''' 对于空文本，这个函数返回一个空集合
        ''' </summary>
        ''' <param name="text">原始文本</param>
        ''' <param name="lineBreak">每一行文本的最大字符数量</param>
        ''' <returns></returns>
        <Extension>
        Public Iterator Function Chunks(text$, lineBreak%) As IEnumerable(Of String)
            If Not text.StringEmpty Then
                For i As Integer = 1 To text.Length Step lineBreak
                    Yield Mid(text, i, lineBreak)
                Next
            End If
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="text">一大段文本</param>
        ''' <param name="len">每一行文本的最大字符串数量长度</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 假若长度分割落在单词内，则添加一个连接符，假如是空格或者标点符号，则不处理
        ''' </remarks>
        <Extension>
        Public Iterator Function SplitParagraph(text$, len%,
                                                Optional delimiters As String = ";:,.-_&*!+'~ " & ASCII.TAB,
                                                Optional floatChars As Integer = 6) As IEnumerable(Of String)
            Dim lines$() = text.LineTokens
            Dim delIndex As Index(Of Char) = delimiters.Indexing

            For Each line As String In lines
                Dim buf As New List(Of Char)
                Dim i As New CharPtr(line)
                Dim c As Char

                Do While Not (i.EndRead OrElse i.NullEnd)
                    c = ++i
                    buf.Add(c)

                    If c Like delIndex Then
                        If buf.Count >= len OrElse len - buf.Count < floatChars Then
                            Yield buf.PopAll.CharString
                        End If
                    ElseIf buf.Count >= len Then
                        Dim floats = Enumerable _
                            .Range(0, floatChars) _
                            .Select(Function(ci) i(ci)) _
                            .Any(Function(ci)
                                     Return ci <> ASCII.NUL AndAlso ci Like delIndex
                                 End Function)

                        ' if the next 3 chars contains a delimiter
                        ' then not break current line
                        If Not floats Then
                            Yield buf.PopAll.CharString
                        End If
                    End If
                Loop

                If buf.Count > 0 Then
                    Yield buf.PopAll.CharString
                End If
            Next
        End Function
    End Module
End Namespace
