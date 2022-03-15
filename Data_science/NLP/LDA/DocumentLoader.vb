﻿#Region "Microsoft.VisualBasic::1ca896b39f0a54ad4974714ee6afcd80, sciBASIC#\Data_science\NLP\LDA\DocumentLoader.vb"

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

    '   Total Lines: 81
    '    Code Lines: 56
    ' Comment Lines: 6
    '   Blank Lines: 19
    '     File Size: 2.49 KB


    '     Module DocumentLoader
    ' 
    '         Function: load, loadDocument
    ' 
    ' 
    ' /********************************************************************************/

#End Region

Imports System.IO
Imports System.Text
Imports Microsoft.VisualBasic.Language
Imports Microsoft.VisualBasic.Language.Values

Namespace LDA

    Module DocumentLoader

        ''' <summary>
        ''' Load documents from disk
        ''' </summary>
        ''' <param name="folderPath"> is a folder, which contains text documents. </param>
        ''' <returns> a corpus </returns>
        ''' <exception cref="IOException"> </exception>
        Public Function load(folderPath As String) As Corpus
            Dim corpus As Corpus = New Corpus()

            For Each filepath As String In folderPath.ListFiles()
                Dim file = filepath.Open(doClear:=False, [readOnly]:=True)
                Dim br As New StreamReader(file, Encoding.UTF8)
                Dim line As New Value(Of String)
                Dim wordList As New List(Of String)()

                While Not line = br.ReadLine() Is Nothing
                    Dim words = line.Split(" ")

                    For Each word As String In words
                        If word.Trim().Length < 2 Then
                            Continue For
                        End If

                        wordList.Add(word)
                    Next
                End While

                br.Close()
                corpus.addDocument(wordList)
            Next

            If corpus.VocabularySize = 0 Then
                Return Nothing
            End If

            Return corpus
        End Function

        Public Function loadDocument(path As String, vocabulary As Vocabulary) As Integer()
            Dim br As New StreamReader(path)
            Dim line As New Value(Of String)
            Dim wordList As New List(Of Integer)()

            While Not line = br.ReadLine() Is Nothing
                Dim words = line.Split(" ")

                For Each word As String In words
                    If word.Trim().Length < 2 Then
                        Continue For
                    End If

                    Dim id = vocabulary.getId(word)

                    If id IsNot Nothing Then
                        wordList.Add(id)
                    End If
                Next
            End While

            Call br.Close()

            Dim result = New Integer(wordList.Count - 1) {}
            Dim i As i32 = 0

            For Each [integer] As Integer In wordList
                result(++i) = [integer]
            Next

            Return result
        End Function
    End Module
End Namespace
