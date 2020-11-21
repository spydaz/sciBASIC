﻿#Region "Microsoft.VisualBasic::6f46b7c605d8abc716fe1d92812d1859, mime\application%pdf\PdfFileWriter\PDF\PdfShadingFunction.vb"

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

    '     Class PdfShadingFunction
    ' 
    '         Constructor: (+2 Overloads) Sub New
    '         Sub: Constructorhelper
    ' 
    ' 
    ' /********************************************************************************/

#End Region

''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
'
'	PdfFileWriter
'	PDF File Write C# Class Library.
'
'	PdfShadingFunction
'	Support class for both axial and radial shading resources.
'
'	Uzi Granot
'	Version: 1.0
'	Date: April 1, 2013
'	Copyright (C) 2013-2019 Uzi Granot. All Rights Reserved
'
'	PdfFileWriter C# class library and TestPdfFileWriter test/demo
'  application are free software.
'	They is distributed under the Code Project Open License (CPOL).
'	The document PdfFileWriterReadmeAndLicense.pdf contained within
'	the distribution specify the license agreement and other
'	conditions and notes. You must read this document and agree
'	with the conditions specified in order to use this software.
'
'	For version history please refer to PdfDocument.cs
'
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

Imports System
Imports System.Drawing
Imports System.Windows.Media
Imports Color = System.Drawing.Color
Imports SysMedia = System.Windows.Media


    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    ''' <summary>
    ''' PDF shading function class
    ''' </summary>
    ''' <remarks>
    ''' PDF function to convert a number between 0 and 1 into a
    ''' color red green and blue based on the sample color array.
    ''' </remarks>
    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
    Public Class PdfShadingFunction
        Inherits PdfObject
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ''' <summary>
        ''' PDF Shading function constructor
        ''' </summary>
        ''' <param name="Document">Document object parent of this function.</param>
        ''' <param name="ColorArray">Array of colors.</param>
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Public Sub New(ByVal Document As PdfDocument, ByVal ColorArray As Color())      ' PDF document object
            ' Array of colors. Minimum 2.
            MyBase.New(Document, ObjectType.Stream)
            ' build dictionary
            Constructorhelper(ColorArray.Length)

            ' add color array to contents stream
            For Each Color As Color In ColorArray
                ObjectValueList.Add(Color.R)    ' red
                ObjectValueList.Add(Color.G)    ' green
                ObjectValueList.Add(Color.B)    ' blue
            Next

            Return
        End Sub

        ''' <summary>
        ''' PDF Shading function constructor
        ''' </summary>
        ''' <param name="Document">Document object parent of this function.</param>
        ''' <param name="Brush">System.Windows.Media gradient brush</param>
        Public Sub New(ByVal Document As PdfDocument, ByVal Brush As SysMedia.GradientBrush)
            MyBase.New(Document, ObjectType.Stream)
            ' build dictionary
            Constructorhelper(Brush.GradientStops.Count)

            ' add color array to contents stream
            For Each [Stop] As GradientStop In Brush.GradientStops
                ObjectValueList.Add([Stop].Color.R) ' red
                ObjectValueList.Add([Stop].Color.G) ' green
                ObjectValueList.Add([Stop].Color.B) ' blue
            Next

            Return
        End Sub

        Private Sub Constructorhelper(ByVal Length As Integer)
            ' test for error
            If Length < 2 Then Throw New ApplicationException("Shading function color array must have two or more items")

            ' the shading function is a sampled function
            Dictionary.Add("/FunctionType", "0")

            ' input variable is between 0 and 1
            Dictionary.Add("/Domain", "[0 1]")

            ' output variables are red, green and blue color components between 0 and 1
            Dictionary.Add("/Range", "[0 1 0 1 0 1]")

            ' each color components in the stream is 8 bits
            Dictionary.Add("/BitsPerSample", "8")

            ' number of colors in the stream must be two or more
            Dictionary.AddFormat("/Size", "[{0}]", Length)
            Return
        End Sub
    End Class

